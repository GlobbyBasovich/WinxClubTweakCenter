using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinxClubTweakCenter
{
    internal static class X86Opcodes
    {
        public enum R8 : byte
        {
            Al, Cl, Dl, Bl, Ah, Ch, Dh, Bh
        }
        public enum R32 : byte
        {
            Eax, Ecx, Edx, Ebx, Esp, Ebp, Esi, Edi
        }
        public enum Rseg : byte
        {
            Es, Cs, Ss, Ds, Fs, Gs
        }
        [Flags]
        public enum ModRM32 : byte
        {
            Eax, Ecx, Edx, Ebx, Sib, Ebp, Esi, Edi,

            NoDisp = 0b00_000_000,
            PlusDisp8 = 0b01_000_000,
            PlusDisp32 = 0b10_000_000,
            Reg = 0b11_000_000,

            RegOp0 = 0b00_000_000,
            RegOp1 = 0b00_001_000,
            RegOp2 = 0b00_010_000,
            RegOp3 = 0b00_011_000,
            RegOp4 = 0b00_100_000,
            RegOp5 = 0b00_101_000,
            RegOp6 = 0b00_110_000,
            RegOp7 = 0b00_111_000,

            Disp32 = NoDisp | Ebp
        }
        [Flags]
        public enum Sib : byte
        {
            BaseEax, BaseEcx, BaseEdx, BaseEbx, BaseEsp, BaseModDependent, BaseEsi, BaseEdi, //BaseModDependent: 00 - disp32, 01 - EBP+disp8, 02 - EBP+disp32

            Scale1 = 0b00_000_000,
            Scale2 = 0b01_000_000,
            Scale4 = 0b10_000_000,
            Scale8 = 0b11_000_000,

            IndexEax = 0b00_000_000,
            IndexEcx = 0b00_001_000,
            IndexEdx = 0b00_010_000,
            IndexEbx = 0b00_011_000,
            IndexNone = 0b00_100_000,
            IndexEbp = 0b00_101_000,
            IndexEsi = 0b00_110_000,
            IndexEdi = 0b00_111_000,
        }

        public const byte
            TwoByteInstructionPrefix = 0x0F,
            SubEaxIv = 0x2D,
            TestEbGb = 0x84,
            MovEvGv = 0x89,
            Nop = 0x90,
            Retn = 0xC3,
            MovEvIv = 0xC7,
            CallJv = 0xE8,
            JmpJv = 0xE9;

        public static readonly byte[] JzJv = { TwoByteInstructionPrefix, 0x84 };
        public static readonly byte[] JeJv = JzJv;

        public static ModRM32 RToRegOp(R8 r) => (ModRM32)((byte)r << 3);
        public static ModRM32 RToRegOp(R32 r) => (ModRM32)((byte)r << 3);
        public static ModRM32 RToRegOp(Rseg r) => (ModRM32)((byte)r << 3);

        public static ModRM32 RToRM(R8 r) => (ModRM32)(byte)r;
        public static ModRM32 RToRM(R32 r) => (ModRM32)(byte)r;
        public static ModRM32 RToRM(Rseg r) => (ModRM32)(byte)r;
    }
}
