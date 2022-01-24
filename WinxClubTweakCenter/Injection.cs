using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WinxClubTweakCenter
{
    using static X86Opcodes;

    internal class Injection
    {
        public const byte SpareSpaceValue = 0x00;

        public readonly Patch JmpProc, AddProc;

        public Injection(int injectOffset, int procOffset, Action<BinaryWriter, int> writeVanillaCode, Action<BinaryWriter, int> writeProcCode)
        {
            byte[] vanillaData, tweakData;
            using (var dataBuilder = new BinaryWriter(new MemoryStream()))
            {
                dataBuilder.BaseStream.Seek(0, SeekOrigin.Begin);
                writeVanillaCode(dataBuilder, injectOffset);
                int injectSize = (int)dataBuilder.BaseStream.Position;
                if (injectSize < sizeof(byte) + sizeof(int))
                    throw new ArgumentException("Vanilla code must be at least 5 bytes long", nameof(writeVanillaCode));
                dataBuilder.BaseStream.Seek(0, SeekOrigin.Begin);
                dataBuilder.BaseStream.Read(vanillaData = new byte[injectSize], 0, injectSize);
                dataBuilder.BaseStream.Seek(0, SeekOrigin.Begin);
                // jmp proc
                dataBuilder.Write(JmpJv);
                dataBuilder.Write(procOffset - (injectOffset + sizeof(byte) + sizeof(int)));
                // nop vanillaCodeSize - jmpJvSize
                for (int i = sizeof(byte) + sizeof(int); i < injectSize; i++)
                    dataBuilder.Write(Nop);
                dataBuilder.BaseStream.Seek(0, SeekOrigin.Begin);
                dataBuilder.BaseStream.Read(tweakData = new byte[injectSize], 0, injectSize);
                JmpProc = new Patch(injectOffset, vanillaData, tweakData);

                dataBuilder.BaseStream.Seek(0, SeekOrigin.Begin);
                writeProcCode(dataBuilder, procOffset);
                writeVanillaCode(dataBuilder, procOffset);
                // jmp afterInject
                dataBuilder.Write(JmpJv);
                int procSize = (int)dataBuilder.BaseStream.Position + sizeof(int);
                dataBuilder.Write(injectOffset + injectSize - (procOffset + procSize));
                dataBuilder.BaseStream.Seek(0, SeekOrigin.Begin);
                dataBuilder.BaseStream.Read(tweakData = new byte[procSize], 0, procSize);
                vanillaData = Enumerable.Repeat(SpareSpaceValue, procSize).ToArray();
                AddProc = new Patch(procOffset, vanillaData, tweakData);
            }
        }
    }
}
