using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DW1NsfInjector
{
    public class NSF
    {
        public string Signature;
        public byte Version;
        public byte TotalSongs;
        public byte FirstSong;
        public ushort LoadAddress;
        public ushort InitAddress;
        public ushort PlayAddress;
        public string SongName;
        public string Artist;
        public string Copyright;
        public ushort PlaySpeedNTSC;
        public byte[] Banks;
        public ushort PlaySpeedPAL;
        public byte Region;
        public byte ExtraSound;
        public byte Reserved;
        public int ProgramDataSize;
        public byte[] ProgramData;
        public byte[] NSFData;

        public bool Load(string fileName)
        {
            var fileData = File.ReadAllBytes(fileName);
            return Load(fileData);
        }

        public bool Load(byte[] fileData)
        {
            MemoryStream ms = new MemoryStream(fileData);
            BinaryReader br = new BinaryReader(ms);
            Signature = br.ReadFixedLengthString(5);
            Version = br.ReadByte();
            TotalSongs = br.ReadByte();
            FirstSong = br.ReadByte();
            LoadAddress = br.ReadUInt16();
            InitAddress = br.ReadUInt16();
            PlayAddress = br.ReadUInt16();
            SongName = br.ReadFixedLengthString(32);
            Artist = br.ReadFixedLengthString(32);
            Copyright = br.ReadFixedLengthString(32);
            PlaySpeedNTSC = br.ReadUInt16();
            Banks = br.ReadBytes(8);
            PlaySpeedPAL = br.ReadUInt16();
            Region = br.ReadByte();
            ExtraSound = br.ReadByte();
            Reserved = br.ReadByte();
            ProgramDataSize = br.ReadUInt24();
            ProgramData = br.ReadBytes(ProgramDataSize);
            NSFData = br.ReadBytes(fileData.Length - (int)br.BaseStream.Position);
            if (Signature != "NESM\x1A") return false;
            if (FirstSong > TotalSongs) return false;
            if (TotalSongs == 0) return false;
            return true;
        }

        public byte[] GetNESMemory()
        {
            //if (Banks.Any(b => b != 0)) return null;
            byte[] memory = new byte[32768];
            MemoryStream msDest = new MemoryStream(memory);
            MemoryStream msSrc = new MemoryStream(this.NSFData);
            BinaryReader br = new BinaryReader(msSrc);
            BinaryWriter bw = new BinaryWriter(msDest);

            int loadAddress2 = LoadAddress - 0x8000;
            bw.BaseStream.Position = loadAddress2;
            br.BaseStream.Position = 0;
            bw.Write(br.ReadBytes((int)br.BaseStream.Length));
            return memory;
        }
    }

    public static partial class Extensions
    {
        public static string ReadFixedLengthString(this BinaryReader br, int size)
        {
            return ReadFixedLengthString(br, size, Encoding.UTF8);
        }

        public static string ReadFixedLengthString(this BinaryReader br, int size, Encoding encoding)
        {
            var bytes = br.ReadBytes(size);
            int indexOfNull = Array.IndexOf<byte>(bytes, 0);
            if (indexOfNull < 0)
            {
                indexOfNull = size;
            }
            return encoding.GetString(bytes, 0, indexOfNull);
        }

        public static string ReadNullTeriminatedString(this BinaryReader br, int size)
        {
            return ReadNullTeriminatedString(br, size, Encoding.UTF8);
        }

        public static string ReadNullTeriminatedString(this BinaryReader br, int size, Encoding encoding)
        {
            List<byte> bytes = new List<byte>();
            while (true)
            {
                byte b = br.ReadByte();
                if (b == 0) break;
                bytes.Add(b);
            }
            return encoding.GetString(bytes.ToArray());
        }

        public static int ReadUInt24(this BinaryReader br)
        {
            byte[] bytes = new byte[4];
            br.BaseStream.Read(bytes, 0, 3);
            return BitConverter.ToInt32(bytes, 0);
        }

    }

}
