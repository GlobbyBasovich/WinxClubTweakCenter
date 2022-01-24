using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;

namespace WinxClubTweakCenter
{
    internal static class DisplayModesHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINTL
        {
            [MarshalAs(UnmanagedType.I4)]
            public int x;
            [MarshalAs(UnmanagedType.I4)]
            public int y;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DEVMODE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmDeviceName;
            [MarshalAs(UnmanagedType.U2)]
            public ushort dmSpecVersion;
            [MarshalAs(UnmanagedType.U2)]
            public ushort dmDriverVersion;
            [MarshalAs(UnmanagedType.U2)]
            public ushort dmSize;
            [MarshalAs(UnmanagedType.U2)]
            public ushort dmDriverExtra;
            [MarshalAs(UnmanagedType.U4)]
            public uint dmFields;
            public POINTL dmPosition;
            [MarshalAs(UnmanagedType.U4)]
            public uint dmDisplayOrientation;
            [MarshalAs(UnmanagedType.U4)]
            public uint dmDisplayFixedOutput;
            [MarshalAs(UnmanagedType.I2)]
            public short dmColor;
            [MarshalAs(UnmanagedType.I2)]
            public short dmDuplex;
            [MarshalAs(UnmanagedType.I2)]
            public short dmYResolution;
            [MarshalAs(UnmanagedType.I2)]
            public short dmTTOption;
            [MarshalAs(UnmanagedType.I2)]
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmFormName;
            [MarshalAs(UnmanagedType.U2)]
            public ushort dmLogPixels;
            [MarshalAs(UnmanagedType.U4)]
            public uint dmBitsPerPel;
            [MarshalAs(UnmanagedType.U4)]
            public uint dmPelsWidth;
            [MarshalAs(UnmanagedType.U4)]
            public uint dmPelsHeight;
            [MarshalAs(UnmanagedType.U4)]
            public uint dmDisplayFlags;
            [MarshalAs(UnmanagedType.U4)]
            public uint dmDisplayFrequency;
            [MarshalAs(UnmanagedType.U4)]
            public uint dmICMMethod;
            [MarshalAs(UnmanagedType.U4)]
            public uint dmICMIntent;
            [MarshalAs(UnmanagedType.U4)]
            public uint dmMediaType;
            [MarshalAs(UnmanagedType.U4)]
            public uint dmDitherType;
            [MarshalAs(UnmanagedType.U4)]
            public uint dmReserved1;
            [MarshalAs(UnmanagedType.U4)]
            public uint dmReserved2;
            [MarshalAs(UnmanagedType.U4)]
            public uint dmPanningWidth;
            [MarshalAs(UnmanagedType.U4)]
            public uint dmPanningHeight;
        }

        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumDisplaySettings(
            [param: MarshalAs(UnmanagedType.LPTStr)]
            string lpszDeviceName,
            [param: MarshalAs(UnmanagedType.U4)]
            int iModeNum,
            [In, Out]
            ref DEVMODE lpDevMode);

        public struct Resolution
        {
            public const int MaximumStringLength = 0xC;

            public Size Size;

            public Resolution(Size size)
            {
                Size = size;
            }

            public override string ToString() => $"{Size.Width} X {Size.Height}";
        }
        public struct ResolutionOffsetPair
        {
            public int WidthOffset, HeightOffset;

            public ResolutionOffsetPair(int widthOffset, int heightOffset)
            {
                WidthOffset = widthOffset;
                HeightOffset = heightOffset;
            }
        }

        public static IEnumerable<Resolution> GetSupportedResolutions()
        {
            var mode = new DEVMODE();
            mode.dmSize = (ushort)Marshal.SizeOf(mode);
            var used = new HashSet<Size>();
            for (int i = 0; EnumDisplaySettings(null, i, ref mode); i++)
            {
                var size = new Size((int)mode.dmPelsWidth, (int)mode.dmPelsHeight);
                if (!used.Contains(size))
                {
                    used.Add(size);
                    yield return new Resolution(size);
                }
            }
        }
    }
}
