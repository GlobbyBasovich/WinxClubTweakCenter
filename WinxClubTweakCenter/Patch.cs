using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WinxClubTweakCenter
{
    internal class Patch
    {
        public readonly int Offset;
        public readonly byte[] Data, VanillaData;
        
        public int Size => VanillaData.Length;
        public int OffsetAfter => Offset + Size;

        public Patch(int offset, byte[] data)
        {
            Offset = offset;
            Data = data;
        }
        public Patch(int offset, byte[] vanillaData, byte[] tweakData) : this(offset, tweakData)
        {
            if (vanillaData == null || tweakData == null || vanillaData.Length == 0 || vanillaData.Length != tweakData.Length)
                throw new ArgumentException($"{nameof(vanillaData)} and {nameof(tweakData)} must have equal length");
            this.VanillaData = vanillaData;
        }

        public void Apply(BinaryWriter writer, bool doRevert = false)
        {
            writer.Seek(Offset, SeekOrigin.Begin);
            if (doRevert)
            {
                if (VanillaData == null)
                    throw new ArgumentException("This patch is irrevertible", nameof(doRevert));
                writer.Write(VanillaData);
            }
            else
                writer.Write(Data);
        }

        public enum ExamineCurrentDataResult : byte
        {
            Vanilla, Tweak, Unknown
        }

        public ExamineCurrentDataResult ExamineCurrentData(BinaryReader reader)
        {
            reader.BaseStream.Seek(Offset, SeekOrigin.Begin);
            var currentData = reader.ReadBytes(Data.Length);
            if (VanillaData != null)
            {
                var isVanilla = true;
                for (int i = 0; i < currentData.Length; i++)
                {
                    if (currentData[i] != VanillaData[i])
                    {
                        isVanilla = false;
                        break;
                    }
                }
                if (isVanilla)
                    return ExamineCurrentDataResult.Vanilla;
            }
            for (int i = 0; i < currentData.Length; i++)
            {
                if (currentData[i] != Data[i])
                    return ExamineCurrentDataResult.Unknown;
            }
            return ExamineCurrentDataResult.Tweak;
        }

        public static ExamineCurrentDataResult ExamineMultiPatch(BinaryReader reader, Patch[] patches)
        {
            bool isVanilla = true, isTweak = true;
            foreach (var patch in patches)
            {
                var examinationResult = patch.ExamineCurrentData(reader);
                if (examinationResult != ExamineCurrentDataResult.Vanilla)
                    isVanilla = false;
                if (examinationResult != ExamineCurrentDataResult.Tweak)
                    isTweak = false;
                if (!isVanilla && !isTweak)
                    return ExamineCurrentDataResult.Unknown;
            }
            if (isVanilla)
                return ExamineCurrentDataResult.Vanilla;
            return ExamineCurrentDataResult.Tweak;
        }
    }
}
