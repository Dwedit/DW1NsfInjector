using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DW1NsfInjector
{
    public static class Injector
    {
        public static void InjectNsf(string nesFileName, string nsfFileName, string outputFileName, byte songNumber)
        {
            NSF nsf = new NSF();
            nsf.Load(nsfFileName);
            byte[] nsfBank = nsf.GetNESMemory();
            byte[] nesRom = File.ReadAllBytes(nesFileName);

            MemoryStream msSource = new MemoryStream(nesRom);
            BinaryReader br = new BinaryReader(msSource);

            byte[] header = br.ReadBytes(16);
            byte[] bank0 = br.ReadBytes(0x4000);
            byte[] bank1 = br.ReadBytes(0x4000);
            byte[] bank2 = br.ReadBytes(0x4000);
            byte[] bank3 = br.ReadBytes(0x4000);    //bank 3 is not used after expansion
            byte[] bank4 = nsfBank.Skip(0).Take(0x4000).ToArray();
            byte[] bank5 = nsfBank.Skip(0x4000).Take(0x4000).ToArray();
            byte[] bank6 = bank2.ToArray();     //bank 6 is not used - copy bank 2
            byte[] bank7 = bank3.ToArray();
            byte[] chr = br.ReadBytes(0x4000);

            header[4] = 8;

            RamCode ramCode = new RamCode();
            byte[] ramCode2 = ramCode.Build(nsf.InitAddress, nsf.PlayAddress, songNumber);

            BinaryWriter bw = new BinaryWriter(new MemoryStream(bank5));
            bw.BaseStream.Position = 0x4000 - 6;
            bw.Write((ushort)ramCode.Nmi);
            bw.Write((ushort)ramCode.Reset);
            bw.Write((ushort)ramCode.Irq);

            LoaderCode loaderCode = new LoaderCode();
            loaderCode.SetMemory(bank3, 0x8000);
            loaderCode.CurrentAddress = 0x84E8;
            loaderCode.Build(ramCode2, ramCode.Entry);

            Assembler asm = new Assembler();
            asm.SetMemory(bank7, 0xC000);
            asm.CurrentAddress = 0xCCF3;
            asm.JMP(0x84E8);

            FileStream outputFileStream = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            bw = new BinaryWriter(outputFileStream);
            bw.Write(header);
            bw.Write(bank0);
            bw.Write(bank1);
            bw.Write(bank2);
            bw.Write(bank3);
            bw.Write(bank4);
            bw.Write(bank5);
            bw.Write(bank6);
            bw.Write(bank7);
            bw.Write(chr);
            outputFileStream.Flush();
            outputFileStream.Close();
            outputFileStream.Dispose();
        }

    }
    class RamCode : Assembler
    {
        public int Entry;
        public int Nmi;
        public int Irq;
        public int Reset;

        public byte[] Build(ushort initAddress, ushort playAddress, byte songNumber)
        {
            //code at $100:
            //      NOP
            //nmi:
            //      INC $0100
            //irq:
            //      RTI
            //main:
            //      LDA #songNumber
            //      JSR NSF.InitAddress
            //      LDA #$80
            //      STA $2000
            //mainloop:
            //      LDA #$00
            //      STA $0100
            //waitloop:
            //      LDA $0100
            //      BEQ waitloop
            //      JSR NSF.PlayAddress
            //      LDA #$00
            //      BEQ mainloop
            //reboot:
            //      LDA #$FF
            //      STA $FFFF
            //      JMP ($FFFC)
            //entry:
            //      LDA #$00
            //      STA $8000
            //      JMP main

            byte[] ramCode = new byte[256];
            SetMemory(ramCode, 0x0100);
            NOP();
            Label("nmi");
            INC_ABS(0x100);
            Label("irq");
            RTI();
            Label("main");
            //call Init function with song number
            LDA_IMM(songNumber);
            JSR(initAddress);
            //enable NMI
            LDA_IMM(0x80);
            STA_ABS(0x2000);
            Label("mainloop");
            LDA_IMM(0);
            STA_ABS(0x100);
            Label("waitloop");
            LDA_ABS(0x100);
            BEQ("waitloop");
            JSR(playAddress);
            JMP("mainloop");
            Label("reset");
            LDA_IMM(0xFF);
            STA_ABS(0xFFFF);
            JMP_IND(0xFFFC);
            //5th write to bank selection + 5 writes to control (A = 0)
            Label("entry");
            STA_ABS(0xE000);
            STA_ABS(0x8000);
            STA_ABS(0x8000);
            STA_ABS(0x8000);
            STA_ABS(0x8000);
            STA_ABS(0x8000);
            JMP("main");
            Label("end");

            this.Entry = (ushort)GetLabel("entry");
            this.Nmi = (ushort)GetLabel("nmi");
            this.Irq = (ushort)GetLabel("irq");
            this.Reset = (ushort)GetLabel("reset");
            int End = (ushort)GetLabel("end");

            ApplyFixups();

            byte[] RamCode2 = ramCode.Take(End - 0x100).ToArray();
            return RamCode2;
        }
    }

    class LoaderCode : Assembler
    {
        public void Build(byte[] ramCode2, int entryPoint)
        {
            //code for the patch - All of Bank 3 is free to use

            //ram clear and boot code
            SEI();
            LDA_IMM(0);
            STA_ABS(0x2000);    //disable NMI
            LDX_IMM(0xFF);
            TSX();              //reset stack
            TAX();
            Label("clrloop");
            STA_ZPG_X(0x00);
            STA_ABS_X(0x100);
            STA_ABS_X(0x200);
            STA_ABS_X(0x300);
            STA_ABS_X(0x400);
            STA_ABS_X(0x500);
            STA_ABS_X(0x600);
            STA_ABS_X(0x700);
            INX();
            BNE("clrloop");

            //disable frame IRQ
            LDA_IMM(0x40);
            STA_ABS(0x4017);
            STA_ABS(0x4015);

            //copy code to RAM
            LDX_IMM((byte)ramCode2.Length);
            Label("copy_loop");
            LDA_ABS_X(0);
            AddWordFixup("ram_copy_source");
            STA_ABS_X(0x100);
            DEX();
            BPL("copy_loop");

            //prepare bankswitch
            LDA_IMM(0x02);
            STA_ABS(0xE000);
            STA_ABS(0xE000);
            LSR_A();
            STA_ABS(0xE000);
            LSR_A();
            STA_ABS(0xE000);
            //jump to RAM code and complete the bankswitch
            JMP((ushort)entryPoint);
            Label("ram_copy_source");
            IncBin(ramCode2);
            ApplyFixups();
        }
    }
}
