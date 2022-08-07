using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WinxClubTweakCenter
{
    using static X86Opcodes;
    using static DisplayModesHelper;

    internal class Patches
    {
        public readonly static Patch[] AllowFirstPersonMovement;
        public readonly static Patch
            AllowFirstPersonAnywhere,
            EnableFlight,
            AllowLookingUp,
            AddTimeInStar01Challenge,
            AddTimeInStar02Challenge,
            AddTimeInStar03Challenge;

        static Patches()
        {
            const int imageBase = 0x400000;
            const int sectionRdataVirtualAddress = 0x2D9000, sectionRdataRawAddress = 0x2D8000;
            const int sectionRdataBias = imageBase + sectionRdataVirtualAddress - sectionRdataRawAddress;
            const int sectionTextSpareOffset = 0x2D74D4, sectionTextSpareSize = 0xB2C;

            Patch
                SkipFirstPersonExitOnMove,
                MakeSectionRdataWritable,
                AddProcSetCameraHeightForFirstPerson, JmpProcOnFirstPersonEnter,
                AddProcRestoreCameraHeight, JmpProcOnCameraModeChange;
            int offset, size;
            byte[] vanillaData, tweakData;
            Injection injection;
            Action<BinaryWriter, int> writeVanillaCode, writeProcCode;
            using (var dataBuilder = new BinaryWriter(new MemoryStream()))
            {
                offset = 0xE0382;
                dataBuilder.BaseStream.Seek(0, SeekOrigin.Begin);
                // mov [esi+29Ch], eax
                dataBuilder.Write(MovEvGv);
                dataBuilder.Write((byte)(ModRM32.PlusDisp32 | RToRegOp(R32.Eax) | ModRM32.Esi));
                dataBuilder.Write(0x29C);
                size = (int)dataBuilder.BaseStream.Position;
                dataBuilder.BaseStream.Seek(0, SeekOrigin.Begin);
                dataBuilder.BaseStream.Read(vanillaData = new byte[size], 0, size);
                // nop size
                tweakData = Enumerable.Repeat(Nop, size).ToArray();
                SkipFirstPersonExitOnMove = new Patch(offset, vanillaData, tweakData);

                offset = 0x1C7;
                // db 0100_0000b ; Read only section access
                vanillaData = new byte[] { 0b0100_0000 };
                // db 1100_0000b ; Read/Write section access
                tweakData = new byte[] { 0b1100_0000 };
                MakeSectionRdataWritable = new Patch(offset, vanillaData, tweakData);

                void writeProcSetCameraHeight(BinaryWriter b, int o, float h)
                {
                    // mov [6F4FC0], h
                    b.Write(MovEvIv);
                    b.Write((byte)ModRM32.Disp32);
                    b.Write(0x2F3FC0 + sectionRdataBias);
                    b.Write(h);
                }

                offset = 0xE0AD3;
                writeVanillaCode = (b, o) =>
                {
                    // call someVanillaSubprogram
                    b.Write(CallJv);
                    b.Write(0xDCD80 - (o + (int)b.BaseStream.Position + sizeof(int)));
                };
                writeProcCode = (b, o) => writeProcSetCameraHeight(b, o, 120f);
                injection = new Injection(offset, sectionTextSpareOffset, writeVanillaCode, writeProcCode);
                AddProcSetCameraHeightForFirstPerson = injection.AddProc;
                JmpProcOnFirstPersonEnter = injection.JmpProc;

                offset = 0xE09C4;
                writeVanillaCode = (b, o) =>
                {
                    // sub eax, 2713h
                    b.Write(SubEaxIv);
                    b.Write(0x2713);
                };
                writeProcCode = (b, o) => writeProcSetCameraHeight(b, o, 150f);
                injection = new Injection(offset, AddProcSetCameraHeightForFirstPerson.OffsetAfter, writeVanillaCode, writeProcCode);
                AddProcRestoreCameraHeight = injection.AddProc;
                JmpProcOnCameraModeChange = injection.JmpProc;

                if (AddProcRestoreCameraHeight.OffsetAfter > sectionTextSpareOffset + sectionTextSpareSize)
                    throw new Exception(".text section spare space size exceeded");

                offset = 0xE0AD8;
                dataBuilder.BaseStream.Seek(0, SeekOrigin.Begin);
                // test al, al
                dataBuilder.Write(TestEbGb);
                dataBuilder.Write((byte)(ModRM32.Reg | RToRegOp(R8.Al) | RToRM(R8.Al)));
                // jz someVanillaLocation
                dataBuilder.Write(JzJv);
                dataBuilder.Write(0xE0A33 - (offset + (int)dataBuilder.BaseStream.Position + sizeof(int)));
                size = (int)dataBuilder.BaseStream.Position;
                dataBuilder.BaseStream.Seek(0, SeekOrigin.Begin);
                dataBuilder.BaseStream.Read(vanillaData = new byte[size], 0, size);
                // nop size
                tweakData = Enumerable.Repeat(Nop, size).ToArray();
                AllowFirstPersonAnywhere = new Patch(offset, vanillaData, tweakData);

                offset = 0x124FED;
                // dd 12h ; operand in   mov eax, 12h
                vanillaData = BitConverter.GetBytes(0x12);
                // dd 0 ; operand in   mov eax, 0
                tweakData = BitConverter.GetBytes(0x00);
                EnableFlight = new Patch(offset, vanillaData, tweakData);

                offset = 0x5B4;
                // dd 0f ; 0f value of "Elevation Minimum (rad)" in Media\Characters\Bloom\Player.spt
                vanillaData = BitConverter.GetBytes(0f);
                // dd -1f ; -1f value of "Elevation Minimum (rad)" in Media\Characters\Bloom\Player.spt
                tweakData = BitConverter.GetBytes(-1f);
                AllowLookingUp = new Patch(offset, vanillaData, tweakData);

                offset = 0x70C;
                // dd 70000 ; 70000 value of "Starting Time (ms) on timer" in Media\Levels\Challenges\star_01.spt
                vanillaData = BitConverter.GetBytes(70000);
                // dd 135000 ; 135000 value of "Starting Time (ms) on timer" in Media\Levels\Challenges\star_01.spt
                tweakData = BitConverter.GetBytes(135000);
                AddTimeInStar01Challenge = new Patch(offset, vanillaData, tweakData);
                // dd 75000 ; 75000 value of "Starting Time (ms) on timer" in Media\Levels\Challenges\star_02.spt
                vanillaData = BitConverter.GetBytes(75000);
                // dd 140000 ; 140000 value of "Starting Time (ms) on timer" in Media\Levels\Challenges\star_02.spt
                tweakData = BitConverter.GetBytes(140000);
                AddTimeInStar02Challenge = new Patch(offset, vanillaData, tweakData);
                // dd 135000 ; 135000 value of "Starting Time (ms) on timer" in Media\Levels\Challenges\star_03.spt
                vanillaData = BitConverter.GetBytes(135000);
                // dd 230000 ; 230000 value of "Starting Time (ms) on timer" in Media\Levels\Challenges\star_03.spt
                tweakData = BitConverter.GetBytes(230000);
                AddTimeInStar03Challenge = new Patch(offset, vanillaData, tweakData);
            };

            AllowFirstPersonMovement = new Patch[] {
                SkipFirstPersonExitOnMove,
                MakeSectionRdataWritable,
                AddProcSetCameraHeightForFirstPerson, JmpProcOnFirstPersonEnter,
                AddProcRestoreCameraHeight, JmpProcOnCameraModeChange
            };

            LegacyHelper.AllowFirstPersonMovementV1_0 = SkipFirstPersonExitOnMove;
            tweakData = new byte[] {
                0xC7, 0x05, 0xC0, 0x4F, 0x6F, 0x00, 0x00, 0x00, 0x16, 0x43,
                0x2D, 0x13, 0x27, 0x00, 0x00,
                0xC3
            };
            vanillaData = Enumerable.Repeat(Injection.SpareSpaceValue, tweakData.Length).ToArray();
            var addProcRestoreCameraHeightV2_0 = new Patch(AddProcRestoreCameraHeight.Offset, vanillaData, tweakData);
            tweakData = new byte[] { 0xE8, 0x1F, 0x6B, 0x1F, 0x00 };
            var callProcRestoreCameraHeightV2_0 = new Patch(JmpProcOnCameraModeChange.Offset, JmpProcOnCameraModeChange.VanillaData, tweakData);
            LegacyHelper.AllowFirstPersonMovementV2_0 = new Patch[] {
                SkipFirstPersonExitOnMove,
                MakeSectionRdataWritable,
                AddProcSetCameraHeightForFirstPerson, JmpProcOnFirstPersonEnter,
                addProcRestoreCameraHeightV2_0, callProcRestoreCameraHeightV2_0
            };
        }

        private static readonly ResolutionOffsetPair[] resolutionOffsetPairs = {
            new ResolutionOffsetPair(0xD5E5, 0xD5EC),
            new ResolutionOffsetPair(0xD617, 0xD61E),
            new ResolutionOffsetPair(0x53BD1, 0x53BD8),
            new ResolutionOffsetPair(0xC2681, 0xC2689),
            new ResolutionOffsetPair(0xE7777, 0xE777F),
        };
        private static readonly int resolutionStringOffset = 0x30ED8C;
        private static readonly int fieldOfViewOffset = 0x33F100;
        private static readonly float squareResolutionFov = 0.01308997f;
        public static Patch[] GetResolutionPatches(Resolution resolution)
        {
            byte[]
                widthData = BitConverter.GetBytes(resolution.Size.Width),
                heightData = BitConverter.GetBytes(resolution.Size.Height);
            var result = new List<Patch>();
            foreach (var pair in resolutionOffsetPairs)
            {
                result.Add(new Patch(pair.WidthOffset, widthData));
                result.Add(new Patch(pair.HeightOffset, heightData));
            }
            result.Add(new Patch(resolutionStringOffset, Encoding.ASCII.GetBytes($"{resolution}\0")));
            var fovData = BitConverter.GetBytes(squareResolutionFov * resolution.Size.Width / resolution.Size.Height);
            result.Add(new Patch(fieldOfViewOffset, fovData));
            return result.ToArray();
        }
        public static bool TryGetCurrentResolution(BinaryReader reader, out Resolution resolution)
        {
            reader.BaseStream.Seek(resolutionOffsetPairs[0].WidthOffset, SeekOrigin.Begin);
            var width = reader.ReadInt32();
            reader.BaseStream.Seek(resolutionOffsetPairs[0].HeightOffset, SeekOrigin.Begin);
            var height = reader.ReadInt32();
            resolution = new Resolution(new Size(width, height));
            for (int i = 1; i < resolutionOffsetPairs.Length; i++)
            {
                reader.BaseStream.Seek(resolutionOffsetPairs[i].WidthOffset, SeekOrigin.Begin);
                width = reader.ReadInt32();
                reader.BaseStream.Seek(resolutionOffsetPairs[i].HeightOffset, SeekOrigin.Begin);
                height = reader.ReadInt32();
                if (new Resolution(new Size(width, height)).Size != resolution.Size)
                    return false;
            }
            reader.BaseStream.Seek(resolutionStringOffset, SeekOrigin.Begin);
            var currentResolutionStringBuilder = new StringBuilder();
            char c;
            while ((c = reader.ReadChar()) != '\0')
                currentResolutionStringBuilder.Append(c);
            if ($"{currentResolutionStringBuilder}" != $"{resolution}")
                return false;
            return true;
        }

        const string testCinematicKey = "testCinematic";
        const string cinematicToTestKey = "cinematicToTest";
        public static void ApplySkipLogos(FileInfo winxIniInfo, bool doRevert = false)
        {
            if (!winxIniInfo.Exists)
            {
                if (doRevert)
                    return;
                winxIniInfo.Create().Dispose();
            }
            var winxIniLines = new List<string>(File.ReadAllLines(winxIniInfo.FullName));
            var testCinematicValue = "true";
            var cinematicToTestValue = "82";
            if (doRevert)
            {
                testCinematicValue = "false";
                cinematicToTestValue = "0";
            }
            var testCinematicMissing = true;
            var cinematicToTestMissing = true;
            for (int i = 0; i < winxIniLines.Count; i++)
            {
                if (winxIniLines[i].StartsWith(testCinematicKey))
                {
                    winxIniLines[i] = $"{testCinematicKey}={testCinematicValue}";
                    testCinematicMissing = false;
                }
                else if (winxIniLines[i].StartsWith(cinematicToTestKey) && !doRevert)
                {
                    winxIniLines[i] = $"{cinematicToTestKey}={cinematicToTestValue}";
                    cinematicToTestMissing = false;
                }
            }
            if (testCinematicMissing)
                winxIniLines.Add($"{testCinematicKey}={testCinematicValue}");
            if (cinematicToTestMissing && !doRevert)
                winxIniLines.Add($"{cinematicToTestKey}={cinematicToTestValue}");
            File.WriteAllLines(winxIniInfo.FullName, winxIniLines.ToArray());
        }
        public static Patch.ExamineCurrentDataResult ExamineSkipLogos(FileInfo winxIniInfo)
        {
            if (!winxIniInfo.Exists)
                return Patch.ExamineCurrentDataResult.Vanilla;
            var winxIniLines = new List<string>(File.ReadAllLines(winxIniInfo.FullName));
            bool testCinematicIsTweak = false, cinematicToTestIsTweak = false;
            for (int i = 0; i < winxIniLines.Count; i++)
            {
                if (winxIniLines[i].StartsWith(testCinematicKey))
                {
                    var regex = new Regex($@"^{testCinematicKey} *= *t", RegexOptions.IgnoreCase);
                    if (regex.IsMatch(winxIniLines[i]))
                        testCinematicIsTweak = true;
                }
                else if (winxIniLines[i].StartsWith(cinematicToTestKey))
                {
                    var regex = new Regex($@"^{cinematicToTestKey} *= *(\d+)", RegexOptions.IgnoreCase);
                    if (!int.TryParse(regex.Match(winxIniLines[i]).Groups[1].Value, out int cinematicToTest) ||
                        cinematicToTest > 81)
                    {
                        cinematicToTestIsTweak = true;
                    }
                }
            }
            if (testCinematicIsTweak && cinematicToTestIsTweak)
                return Patch.ExamineCurrentDataResult.Tweak;
            return Patch.ExamineCurrentDataResult.Vanilla;
        }
    }
}
