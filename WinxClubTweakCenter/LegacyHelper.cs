using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WinxClubTweakCenter
{
    internal static class LegacyHelper
    {
        public enum Version : byte
        {
            Unknown, Vanilla, V1_0, V2_0, V2_1
        }

        public static Patch AllowFirstPersonMovementV1_0;
        public static Patch[] AllowFirstPersonMovementV2_0;

        public static Version GetAllowFirstPersonMovementVersion(BinaryReader reader)
        {
            Patch.ExamineCurrentDataResult examinationResult;

            examinationResult = Patch.ExamineMultiPatch(reader, Patches.AllowFirstPersonMovement);
            if (examinationResult == Patch.ExamineCurrentDataResult.Vanilla)
                return Version.Vanilla;
            if (examinationResult == Patch.ExamineCurrentDataResult.Tweak)
                return Version.V2_1;

            examinationResult = Patch.ExamineMultiPatch(reader, AllowFirstPersonMovementV2_0);
            if (examinationResult == Patch.ExamineCurrentDataResult.Vanilla)
                return Version.Vanilla;
            if (examinationResult == Patch.ExamineCurrentDataResult.Tweak)
                return Version.V2_0;

            examinationResult = AllowFirstPersonMovementV1_0.ExamineCurrentData(reader);
            if (examinationResult == Patch.ExamineCurrentDataResult.Vanilla)
                return Version.Vanilla;
            if (examinationResult == Patch.ExamineCurrentDataResult.Tweak)
                return Version.V1_0;

            return Version.Unknown;
        }
    }
}
