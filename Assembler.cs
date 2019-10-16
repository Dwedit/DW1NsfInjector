using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DW1NsfInjector
{
    public class Assembler
    {
        public void SetMemory(byte[] memory, int memoryBase)
        {
            int currentAddress = memoryBase;
            if (this.bw != null)
            {
                currentAddress = this.CurrentAddress;
            }


            this.Memory = memory;
            this.bw = new BinaryWriter(new MemoryStream(this.Memory));
            MemoryBase = memoryBase;
        }

        public int CurrentAddress
        {
            get
            {
                return (int)this.bw.BaseStream.Position + MemoryBase;
            }
            set
            {
                this.bw.BaseStream.Position = value - MemoryBase;
            }
        }

        BinaryWriter bw;
        int MemoryBase;
        byte[] Memory;
        Dictionary<string, int> Labels = new Dictionary<string, int>();
        Dictionary<int, string> ByteFixups = new Dictionary<int, string>();
        Dictionary<int, string> HiByteFixups = new Dictionary<int, string>();
        Dictionary<int, string> WordFixups = new Dictionary<int, string>();
        Dictionary<int, string> BranchFixups = new Dictionary<int, string>();

        public enum Instruction
        {
            BRK = 0x00,
            PHP = 0x08,
            BPL = 0x10,
            CLC = 0x18,

            JSR = 0x20,
            PLP = 0x28,
            BMI = 0x30,
            SEC = 0x38,

            RTI = 0x40,
            PHA = 0x48,
            BVC = 0x50,
            CLI = 0x58,

            RTS = 0x60,
            PLS = 0x68,
            BVS = 0x70,
            SEI = 0x78,

            //? = 0x80,
            DEY = 0x88,
            BCC = 0x90,
            TYA = 0x98,

            LDY_IMM = 0xA0,
            TAY = 0xA8,
            BCS = 0xB0,
            CLV = 0xB8,

            CPY_IMM = 0xC0,
            INY = 0xC8,
            BNE = 0xD0,
            CLD = 0xD8,

            CPX_IMM = 0xE0,
            INX = 0xE8,
            BEQ = 0xF0,
            SED = 0xF8,

            ORA_X_IND = 0x01,
            ORA_IMM = 0x09,
            ORA_IND_Y = 0x11,
            ORA_ABS_Y = 0x19,

            AND_X_IND = 0x21,
            AND_IMM = 0x29,
            AND_IND_Y = 0x31,
            AND_ABS_Y = 0x39,

            EOR_X_IND = 0x41,
            EOR_IMM = 0x49,
            EOR_IND_Y = 0x51,
            EOR_ABS_Y = 0x59,

            ADC_X_IND = 0x61,
            ADC_IMM = 0x69,
            ADC_IND_Y = 0x71,
            ADC_ABS_Y = 0x79,

            STA_X_IND = 0x81,
            //??? --- = 0x89,
            STA_IND_Y = 0x91,
            STA_ABS_Y = 0x99,

            LDA_X_IND = 0xA1,
            LDA_IMM = 0xA9,
            LDA_IND_Y = 0xB1,
            LDA_ABS_Y = 0xB9,

            CMP_X_IND = 0xC1,
            CMP_IMM = 0xC9,
            CMP_IND_Y = 0xD1,
            CMP_ABS_Y = 0xD9,

            SBC_X_IND = 0xE1,
            SBC_IMM = 0xE9,
            SBC_IND_Y = 0xF1,
            SBC_ABS_Y = 0xF9,

            ORA_ZPG = 0x05,
            ORA_ABS = 0x0D,
            ORA_ZPG_X = 0x15,
            ORA_ABS_X = 0x1D,

            AND_ZPG = 0x25,
            AND_ABS = 0x2D,
            AND_ZPG_X = 0x35,
            AND_ABS_X = 0x3D,

            EOR_ZPG = 0x45,
            EOR_ABS = 0x4D,
            EOR_ZPG_X = 0x55,
            EOR_ABS_X = 0x5D,

            ADC_ZPG = 0x65,
            ADC_ABS = 0x6D,
            ADC_ZPG_X = 0x75,
            ADC_ABS_X = 0x7D,

            STA_ZPG = 0x85,
            STA_ABS = 0x8D,
            STA_ZPG_X = 0x95,
            STA_ABS_X = 0x9D,

            LDA_ZPG = 0xA5,
            LDA_ABS = 0xAD,
            LDA_ZPG_X = 0xB5,
            LDA_ABS_X = 0xBD,

            CMP_ZPG = 0xC5,
            CMP_ABS = 0xCD,
            CMP_ZPG_X = 0xD5,
            CMP_ABS_X = 0xDD,

            SBC_ZPG = 0xE5,
            SBC_ABS = 0xED,
            SBC_ZPG_X = 0xF5,
            SBC_ABS_X = 0xFD,

            ASL_ZPG = 0x06,
            ASL_ABS = 0x0E,
            ASL_ZPG_X = 0x16,
            ASL_ABS_X = 0x1E,

            ROL_ZPG = 0x26,
            ROL_ABS = 0x2E,
            ROL_ZPG_X = 0x36,
            ROL_ABS_X = 0x3E,

            LSR_ZPG = 0x46,
            LSR_ABS = 0x4E,
            LSR_ZPG_X = 0x56,
            LSR_ABS_X = 0x5E,

            ROR_ZPG = 0x66,
            ROR_ABS = 0x6E,
            ROR_ZPG_X = 0x76,
            ROR_ABS_X = 0x7E,

            STX_ZPG = 0x86,
            STX_ABS = 0x8E,
            STX_ZPG_Y = 0x96,
            //??? --- = 0x9E,

            LDX_ZPG = 0xA6,
            LDX_ABS = 0xAE,
            LDX_ZPG_Y = 0xB6,
            LDX_ABS_Y = 0xBE,

            DEC_ZPG = 0xC6,
            DEC_ABS = 0xCE,
            DEC_ZPG_X = 0xD6,
            DEC_ABS_X = 0xDE,

            INC_ZPG = 0xE6,
            INC_ABS = 0xEE,
            INC_ZPG_X = 0xF6,
            INC_ABS_X = 0xFE,

            //??? --- = 0x02,
            ASL_A = 0x0A,
            //??? --- = 0x12,
            //??? --- = 0x1A,

            //??? --- = 0x22,
            ROL_A = 0x2A,
            //??? ---   = 0x32,
            //??? ---   = 0x3A,

            //??? ---   = 0x42,
            LSR_A = 0x4A,
            //??? ---   = 0x52,
            //??? ---   = 0x5A,

            //??? ---   = 0x62,
            ROR_A = 0x6A,
            //??? ---   = 0x72,
            //??? ---   = 0x7A,

            //??? ---   = 0x82,
            TXA = 0x8A,
            //??? ---   = 0x92,
            TXS = 0x9A,

            LDX_IMM = 0xA2,
            TAX = 0xAA,
            //??? ---   = 0xB2,
            TSX = 0xBA,

            //??? ---   = 0xC2,
            DEX = 0xCA,
            //??? ---   = 0xD2,
            //??? ---   = 0xDA,

            //??? ---   = 0xE2,
            NOP = 0xEA,
            //??? ---   = 0xF2,
            //??? ---   = 0xFA,

            //??? ---   = 0x04,
            //??? ---   = 0x0C,
            //??? ---   = 0x14,
            //??? ---   = 0x1C,

            BIT_ZPG = 0x24,
            BIT_ABS = 0x2C,
            //??? ---   = 0x34,
            //??? ---   = 0x3C,

            //??? ---   = 0x44,
            JMP_ABS = 0x4C,
            //??? ---   = 0x54,
            //??? ---   = 0x5C,

            //??? ---   = 0x64,
            JMP_IND = 0x6C,
            //??? ---   = 0x74,
            //??? ---   = 0x7C,

            STY_ZPG = 0x84,
            STY_ABS = 0x8C,
            STY_ZPG_X = 0x94,
            //??? ---   = 0x9C,

            LDY_ZPG = 0xA4,
            LDY_ABS = 0xAC,
            LDY_ZPG_X = 0xB4,
            LDY_ABS_X = 0xBC,

            CPY_ZPG = 0xC4,
            CPY_ABS = 0xCC,
            //??? ---   = 0xD4,
            //??? ---   = 0xDC,

            CPX_ZPG = 0xE4,
            CPX_ABS = 0xEC,
            //??? ---   = 0xF4,
            //??? ---   = 0xFC,
        }

        //byte GetRelativeAddress(ushort address)
        //{
        //    unchecked
        //    {
        //        int currentAddress = CurrentAddress + 1;
        //        int difference = (currentAddress - (int)address);
        //        sbyte differenceByte1 = (sbyte)difference;
        //        return (byte)differenceByte1;
        //    }
        //}
        public int GetCurrentAddress()
        {
            return CurrentAddress;
        }
        public void Label(string labelName)
        {
            SetLabel(labelName, CurrentAddress);
        }
        public void SetLabel(string labelName)
        {
            SetLabel(labelName, CurrentAddress);
        }
        public void SetLabel(string labelName, int value)
        {
            Labels[labelName] = value;
        }
        public int GetLabel(string labelName)
        {
            return Labels[labelName];
        }
        public void AddByteFixup(string labelName)
        {
            ByteFixups[CurrentAddress - 1] = labelName;
        }
        public void AddHiByteFixup(string labelName)
        {
            HiByteFixups[CurrentAddress - 1] = labelName;
        }
        public void AddWordFixup(string labelName)
        {
            WordFixups[CurrentAddress - 2] = labelName;
        }
        public void AddBranchFixup(string labelName)
        {
            BranchFixups[CurrentAddress - 1] = labelName;
        }
        public void ApplyFixups()
        {
            int _currentAddress = CurrentAddress;
            foreach (var fixup in ByteFixups)
            {
                string labelName = fixup.Value;
                ushort address = (ushort)fixup.Key;
                int value = Labels[labelName];
                CurrentAddress = address;
                bw.Write((byte)value);
            }
            foreach (var fixup in HiByteFixups)
            {
                string labelName = fixup.Value;
                ushort address = (ushort)fixup.Key;
                int value = Labels[labelName];
                CurrentAddress = address;
                bw.Write((byte)(value >> 8));
            }
            foreach (var fixup in WordFixups)
            {
                string labelName = fixup.Value;
                ushort address = (ushort)fixup.Key;
                int value = Labels[labelName];
                CurrentAddress = address;
                bw.Write((ushort)value);
            }
            foreach (var fixup in BranchFixups)
            {
                string labelName = fixup.Value;
                ushort address = (ushort)fixup.Key;
                int value = Labels[labelName];

                int startAddress = address + 1;
                int targetAddress = value;
                int displacement = targetAddress - startAddress;
                sbyte displacement2 = (sbyte)displacement;
                byte displacement3 = (byte)displacement2;

                CurrentAddress = address;
                bw.Write(displacement3);
            }
            CurrentAddress = _currentAddress;
        }

        public void IncBin(byte[] bytes)
        {
            bw.Write(bytes);
        }

        public void BRK() { bw.Write((byte)Instruction.BRK); }
        public void PHP() { bw.Write((byte)Instruction.PHP); }
        public void BPL(string labelName) { bw.Write((byte)Instruction.BPL); bw.Write((byte)0); AddBranchFixup(labelName); }
        public void CLC() { bw.Write((byte)Instruction.CLC); }
        public void JSR(ushort address) { bw.Write((byte)Instruction.JSR); bw.Write((ushort)address); }
        public void JSR(string labelName) { bw.Write((byte)Instruction.JSR); bw.Write((ushort)0); AddWordFixup(labelName); }
        public void PLP() { bw.Write((byte)Instruction.PLP); }
        public void BMI(string labelName) { bw.Write((byte)Instruction.BMI); bw.Write((byte)0); AddBranchFixup(labelName); }
        public void SEC() { bw.Write((byte)Instruction.SEC); }
        public void RTI() { bw.Write((byte)Instruction.RTI); }
        public void PHA() { bw.Write((byte)Instruction.PHA); }
        public void BVC(string labelName) { bw.Write((byte)Instruction.BVC); bw.Write((byte)0); AddBranchFixup(labelName); }
        public void CLI() { bw.Write((byte)Instruction.CLI); }
        public void RTS() { bw.Write((byte)Instruction.RTS); }
        public void PLS() { bw.Write((byte)Instruction.PLS); }
        public void BVS(string labelName) { bw.Write((byte)Instruction.BVS); bw.Write((byte)0); AddBranchFixup(labelName); }
        public void SEI() { bw.Write((byte)Instruction.SEI); }
        public void DEY() { bw.Write((byte)Instruction.DEY); }
        public void BCC(string labelName) { bw.Write((byte)Instruction.BCC); bw.Write((byte)0); AddBranchFixup(labelName); }
        public void TYA() { bw.Write((byte)Instruction.TYA); }
        public void LDY_IMM(byte immediate) { bw.Write((byte)Instruction.LDY_IMM); bw.Write((byte)immediate); }
        public void TAY() { bw.Write((byte)Instruction.TAY); }
        public void BCS(string labelName) { bw.Write((byte)Instruction.BCS); bw.Write((byte)0); AddBranchFixup(labelName); }
        public void CLV() { bw.Write((byte)Instruction.CLV); }
        public void CPY_IMM(byte immediate) { bw.Write((byte)Instruction.CPY_IMM); bw.Write((byte)immediate); }
        public void INY() { bw.Write((byte)Instruction.INY); }
        public void BNE(string labelName) { bw.Write((byte)Instruction.BNE); bw.Write((byte)0); AddBranchFixup(labelName); }
        public void CLD() { bw.Write((byte)Instruction.CLD); }
        public void CPX_IMM(byte immediate) { bw.Write((byte)Instruction.CPX_IMM); bw.Write((byte)immediate); }
        public void INX() { bw.Write((byte)Instruction.INX); }
        public void BEQ(string labelName) { bw.Write((byte)Instruction.BEQ); bw.Write((byte)0); AddBranchFixup(labelName); }
        public void SED() { bw.Write((byte)Instruction.SED); }
        public void ORA_X_IND(byte address) { bw.Write((byte)Instruction.ORA_X_IND); bw.Write((byte)address); }
        public void ORA_IMM(byte immediate) { bw.Write((byte)Instruction.ORA_IMM); bw.Write((byte)immediate); }
        public void ORA_IND_Y(byte address) { bw.Write((byte)Instruction.ORA_IND_Y); bw.Write((byte)address); }
        public void ORA_ABS_Y(ushort address) { bw.Write((byte)Instruction.ORA_ABS_Y); bw.Write((ushort)address); }
        public void AND_X_IND(byte address) { bw.Write((byte)Instruction.AND_X_IND); bw.Write((byte)address); }
        public void AND_IMM(byte immediate) { bw.Write((byte)Instruction.AND_IMM); bw.Write((byte)immediate); }
        public void AND_IND_Y(byte address) { bw.Write((byte)Instruction.AND_IND_Y); bw.Write((byte)address); }
        public void AND_ABS_Y(ushort address) { bw.Write((byte)Instruction.AND_ABS_Y); bw.Write((ushort)address); }
        public void EOR_X_IND(byte address) { bw.Write((byte)Instruction.EOR_X_IND); bw.Write((byte)address); }
        public void EOR_IMM(byte immediate) { bw.Write((byte)Instruction.EOR_IMM); bw.Write((byte)immediate); }
        public void EOR_IND_Y(byte address) { bw.Write((byte)Instruction.EOR_IND_Y); bw.Write((byte)address); }
        public void EOR_ABS_Y(ushort address) { bw.Write((byte)Instruction.EOR_ABS_Y); bw.Write((ushort)address); }
        public void ADC_X_IND(byte address) { bw.Write((byte)Instruction.ADC_X_IND); bw.Write((byte)address); }
        public void ADC_IMM(byte immediate) { bw.Write((byte)Instruction.ADC_IMM); bw.Write((byte)immediate); }
        public void ADC_IND_Y(byte address) { bw.Write((byte)Instruction.ADC_IND_Y); bw.Write((byte)address); }
        public void ADC_ABS_Y(ushort address) { bw.Write((byte)Instruction.ADC_ABS_Y); bw.Write((ushort)address); }
        public void STA_X_IND(byte address) { bw.Write((byte)Instruction.STA_X_IND); bw.Write((byte)address); }
        public void STA_IND_Y(byte address) { bw.Write((byte)Instruction.STA_IND_Y); bw.Write((byte)address); }
        public void STA_ABS_Y(ushort address) { bw.Write((byte)Instruction.STA_ABS_Y); bw.Write((ushort)address); }
        public void LDA_X_IND(byte address) { bw.Write((byte)Instruction.LDA_X_IND); bw.Write((byte)address); }
        public void LDA_IMM(byte immediate) { bw.Write((byte)Instruction.LDA_IMM); bw.Write((byte)immediate); }
        public void LDA_IND_Y(byte address) { bw.Write((byte)Instruction.LDA_IND_Y); bw.Write((byte)address); }
        public void LDA_ABS_Y(ushort address) { bw.Write((byte)Instruction.LDA_ABS_Y); bw.Write((ushort)address); }
        public void CMP_X_IND(byte address) { bw.Write((byte)Instruction.CMP_X_IND); bw.Write((byte)address); }
        public void CMP_IMM(byte immediate) { bw.Write((byte)Instruction.CMP_IMM); bw.Write((byte)immediate); }
        public void CMP_IND_Y(byte address) { bw.Write((byte)Instruction.CMP_IND_Y); bw.Write((byte)address); }
        public void CMP_ABS_Y(ushort address) { bw.Write((byte)Instruction.CMP_ABS_Y); bw.Write((ushort)address); }
        public void SBC_X_IND(byte address) { bw.Write((byte)Instruction.SBC_X_IND); bw.Write((byte)address); }
        public void SBC_IMM(byte immediate) { bw.Write((byte)Instruction.SBC_IMM); bw.Write((byte)immediate); }
        public void SBC_IND_Y(byte address) { bw.Write((byte)Instruction.SBC_IND_Y); bw.Write((byte)address); }
        public void SBC_ABS_Y(ushort address) { bw.Write((byte)Instruction.SBC_ABS_Y); bw.Write((ushort)address); }
        public void ORA_ZPG(byte address) { bw.Write((byte)Instruction.ORA_ZPG); bw.Write((byte)address); }
        public void ORA_ABS(ushort address) { bw.Write((byte)Instruction.ORA_ABS); bw.Write((ushort)address); }
        public void ORA_ZPG_X(byte address) { bw.Write((byte)Instruction.ORA_ZPG_X); bw.Write((byte)address); }
        public void ORA_ABS_X(ushort address) { bw.Write((byte)Instruction.ORA_ABS_X); bw.Write((ushort)address); }
        public void AND_ZPG(byte address) { bw.Write((byte)Instruction.AND_ZPG); bw.Write((byte)address); }
        public void AND_ABS(ushort address) { bw.Write((byte)Instruction.AND_ABS); bw.Write((ushort)address); }
        public void AND_ZPG_X(byte address) { bw.Write((byte)Instruction.AND_ZPG_X); bw.Write((byte)address); }
        public void AND_ABS_X(ushort address) { bw.Write((byte)Instruction.AND_ABS_X); bw.Write((ushort)address); }
        public void EOR_ZPG(byte address) { bw.Write((byte)Instruction.EOR_ZPG); bw.Write((byte)address); }
        public void EOR_ABS(ushort address) { bw.Write((byte)Instruction.EOR_ABS); bw.Write((ushort)address); }
        public void EOR_ZPG_X(byte address) { bw.Write((byte)Instruction.EOR_ZPG_X); bw.Write((byte)address); }
        public void EOR_ABS_X(ushort address) { bw.Write((byte)Instruction.EOR_ABS_X); bw.Write((ushort)address); }
        public void ADC_ZPG(byte address) { bw.Write((byte)Instruction.ADC_ZPG); bw.Write((byte)address); }
        public void ADC_ABS(ushort address) { bw.Write((byte)Instruction.ADC_ABS); bw.Write((ushort)address); }
        public void ADC_ZPG_X(byte address) { bw.Write((byte)Instruction.ADC_ZPG_X); bw.Write((byte)address); }
        public void ADC_ABS_X(ushort address) { bw.Write((byte)Instruction.ADC_ABS_X); bw.Write((ushort)address); }
        public void STA_ZPG(byte address) { bw.Write((byte)Instruction.STA_ZPG); bw.Write((byte)address); }
        public void STA_ABS(ushort address) { bw.Write((byte)Instruction.STA_ABS); bw.Write((ushort)address); }
        public void STA_ZPG_X(byte address) { bw.Write((byte)Instruction.STA_ZPG_X); bw.Write((byte)address); }
        public void STA_ABS_X(ushort address) { bw.Write((byte)Instruction.STA_ABS_X); bw.Write((ushort)address); }
        public void LDA_ZPG(byte address) { bw.Write((byte)Instruction.LDA_ZPG); bw.Write((byte)address); }
        public void LDA_ABS(ushort address) { bw.Write((byte)Instruction.LDA_ABS); bw.Write((ushort)address); }
        public void LDA_ZPG_X(byte address) { bw.Write((byte)Instruction.LDA_ZPG_X); bw.Write((byte)address); }
        public void LDA_ABS_X(ushort address) { bw.Write((byte)Instruction.LDA_ABS_X); bw.Write((ushort)address); }
        public void CMP_ZPG(byte address) { bw.Write((byte)Instruction.CMP_ZPG); bw.Write((byte)address); }
        public void CMP_ABS(ushort address) { bw.Write((byte)Instruction.CMP_ABS); bw.Write((ushort)address); }
        public void CMP_ZPG_X(byte address) { bw.Write((byte)Instruction.CMP_ZPG_X); bw.Write((byte)address); }
        public void CMP_ABS_X(ushort address) { bw.Write((byte)Instruction.CMP_ABS_X); bw.Write((ushort)address); }
        public void SBC_ZPG(byte address) { bw.Write((byte)Instruction.SBC_ZPG); bw.Write((byte)address); }
        public void SBC_ABS(ushort address) { bw.Write((byte)Instruction.SBC_ABS); bw.Write((ushort)address); }
        public void SBC_ZPG_X(byte address) { bw.Write((byte)Instruction.SBC_ZPG_X); bw.Write((byte)address); }
        public void SBC_ABS_X(ushort address) { bw.Write((byte)Instruction.SBC_ABS_X); bw.Write((ushort)address); }
        public void ASL_ZPG(byte address) { bw.Write((byte)Instruction.ASL_ZPG); bw.Write((byte)address); }
        public void ASL_ABS(ushort address) { bw.Write((byte)Instruction.ASL_ABS); bw.Write((ushort)address); }
        public void ASL_ZPG_X(byte address) { bw.Write((byte)Instruction.ASL_ZPG_X); bw.Write((byte)address); }
        public void ASL_ABS_X(ushort address) { bw.Write((byte)Instruction.ASL_ABS_X); bw.Write((ushort)address); }
        public void ROL_ZPG(byte address) { bw.Write((byte)Instruction.ROL_ZPG); bw.Write((byte)address); }
        public void ROL_ABS(ushort address) { bw.Write((byte)Instruction.ROL_ABS); bw.Write((ushort)address); }
        public void ROL_ZPG_X(byte address) { bw.Write((byte)Instruction.ROL_ZPG_X); bw.Write((byte)address); }
        public void ROL_ABS_X(ushort address) { bw.Write((byte)Instruction.ROL_ABS_X); bw.Write((ushort)address); }
        public void LSR_ZPG(byte address) { bw.Write((byte)Instruction.LSR_ZPG); bw.Write((byte)address); }
        public void LSR_ABS(ushort address) { bw.Write((byte)Instruction.LSR_ABS); bw.Write((ushort)address); }
        public void LSR_ZPG_X(byte address) { bw.Write((byte)Instruction.LSR_ZPG_X); bw.Write((byte)address); }
        public void LSR_ABS_X(ushort address) { bw.Write((byte)Instruction.LSR_ABS_X); bw.Write((ushort)address); }
        public void ROR_ZPG(byte address) { bw.Write((byte)Instruction.ROR_ZPG); bw.Write((byte)address); }
        public void ROR_ABS(ushort address) { bw.Write((byte)Instruction.ROR_ABS); bw.Write((ushort)address); }
        public void ROR_ZPG_X(byte address) { bw.Write((byte)Instruction.ROR_ZPG_X); bw.Write((byte)address); }
        public void ROR_ABS_X(ushort address) { bw.Write((byte)Instruction.ROR_ABS_X); bw.Write((ushort)address); }
        public void STX_ZPG(byte address) { bw.Write((byte)Instruction.STX_ZPG); bw.Write((byte)address); }
        public void STX_ABS(ushort address) { bw.Write((byte)Instruction.STX_ABS); bw.Write((ushort)address); }
        public void STX_ZPG_Y(byte address) { bw.Write((byte)Instruction.STX_ZPG_Y); bw.Write((byte)address); }
        public void LDX_ZPG(byte address) { bw.Write((byte)Instruction.LDX_ZPG); bw.Write((byte)address); }
        public void LDX_ABS(ushort address) { bw.Write((byte)Instruction.LDX_ABS); bw.Write((ushort)address); }
        public void LDX_ZPG_Y(byte address) { bw.Write((byte)Instruction.LDX_ZPG_Y); bw.Write((byte)address); }
        public void LDX_ABS_Y(ushort address) { bw.Write((byte)Instruction.LDX_ABS_Y); bw.Write((ushort)address); }
        public void DEC_ZPG(byte address) { bw.Write((byte)Instruction.DEC_ZPG); bw.Write((byte)address); }
        public void DEC_ABS(ushort address) { bw.Write((byte)Instruction.DEC_ABS); bw.Write((ushort)address); }
        public void DEC_ZPG_X(byte address) { bw.Write((byte)Instruction.DEC_ZPG_X); bw.Write((byte)address); }
        public void DEC_ABS_X(ushort address) { bw.Write((byte)Instruction.DEC_ABS_X); bw.Write((ushort)address); }
        public void INC_ZPG(byte address) { bw.Write((byte)Instruction.INC_ZPG); bw.Write((byte)address); }
        public void INC_ABS(ushort address) { bw.Write((byte)Instruction.INC_ABS); bw.Write((ushort)address); }
        public void INC_ZPG_X(byte address) { bw.Write((byte)Instruction.INC_ZPG_X); bw.Write((byte)address); }
        public void INC_ABS_X(ushort address) { bw.Write((byte)Instruction.INC_ABS_X); bw.Write((ushort)address); }
        public void ASL_A() { bw.Write((byte)Instruction.ASL_A); }
        public void ROL_A() { bw.Write((byte)Instruction.ROL_A); }
        public void LSR_A() { bw.Write((byte)Instruction.LSR_A); }
        public void ROR_A() { bw.Write((byte)Instruction.ROR_A); }
        public void TXA() { bw.Write((byte)Instruction.TXA); }
        public void TXS() { bw.Write((byte)Instruction.TXS); }
        public void LDX_IMM(byte immediate) { bw.Write((byte)Instruction.LDX_IMM); bw.Write((byte)immediate); }
        public void TAX() { bw.Write((byte)Instruction.TAX); }
        public void TSX() { bw.Write((byte)Instruction.TSX); }
        public void DEX() { bw.Write((byte)Instruction.DEX); }
        public void NOP() { bw.Write((byte)Instruction.NOP); }
        public void BIT_ZPG(byte address) { bw.Write((byte)Instruction.BIT_ZPG); bw.Write((byte)address); }
        public void BIT_ABS(ushort address) { bw.Write((byte)Instruction.BIT_ABS); bw.Write((ushort)address); }
        public void JMP(ushort address) { bw.Write((byte)Instruction.JMP_ABS); bw.Write((ushort)address); }
        public void JMP(string labelName) { bw.Write((byte)Instruction.JMP_ABS); bw.Write((ushort)0); AddWordFixup(labelName); }
        public void JMP_IND(ushort address) { bw.Write((byte)Instruction.JMP_IND); bw.Write((ushort)address); }
        public void STY_ZPG(byte address) { bw.Write((byte)Instruction.STY_ZPG); bw.Write((byte)address); }
        public void STY_ABS(ushort address) { bw.Write((byte)Instruction.STY_ABS); bw.Write((ushort)address); }
        public void STY_ZPG_X(byte address) { bw.Write((byte)Instruction.STY_ZPG_X); bw.Write((byte)address); }
        public void LDY_ZPG(byte address) { bw.Write((byte)Instruction.LDY_ZPG); bw.Write((byte)address); }
        public void LDY_ABS(ushort address) { bw.Write((byte)Instruction.LDY_ABS); bw.Write((ushort)address); }
        public void LDY_ZPG_X(byte address) { bw.Write((byte)Instruction.LDY_ZPG_X); bw.Write((byte)address); }
        public void LDY_ABS_X(ushort address) { bw.Write((byte)Instruction.LDY_ABS_X); bw.Write((ushort)address); }
        public void CPY_ZPG(byte address) { bw.Write((byte)Instruction.CPY_ZPG); bw.Write((byte)address); }
        public void CPY_ABS(ushort address) { bw.Write((byte)Instruction.CPY_ABS); bw.Write((ushort)address); }
        public void CPX_ZPG(byte address) { bw.Write((byte)Instruction.CPX_ZPG); bw.Write((byte)address); }
        public void CPX_ABS(ushort address) { bw.Write((byte)Instruction.CPX_ABS); bw.Write((ushort)address); }
    }
}
