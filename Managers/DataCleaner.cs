using System.Collections.Generic;
using Enums;
using Unity.VisualScripting;
using UnityEngine;

namespace Managers
{
    public static class DataCleaner
    {
        public static string mainSeppar = ";";
        public static string additionMainSeppar = "/";
        public static string additionSeppar = ",";
        public static string strokeSeppar = "\n";
        public static string rangeSeppar = "-";

        public static string CleanData(string text, DataSepparType type = DataSepparType.Main)
        {
            if (text[text.Length - 1] == '\n')
            {
                text = text.Remove(text.Length - 1);
            }
            text = text.Replace("\r", "");
            text = text.Replace(mainSeppar + " ", mainSeppar);
            if (type == DataSepparType.Main) return text;
            text = text.Replace(additionSeppar + " ", additionSeppar);
            text = text.Replace(additionMainSeppar + " ", additionMainSeppar);
            // text = text.Replace("\"", "");
            return text;
        }

        public static string[] GetStrokesData(
            string text,
            DataSepparType type)
        {
            if (type == DataSepparType.Addition) return text.Split(additionSeppar);
            if (type == DataSepparType.Main) return text.Split(mainSeppar);
            if (type == DataSepparType.AdditionMain) return text.Split(additionMainSeppar);
            if (type == DataSepparType.Stroke) return text.Split(strokeSeppar);
            return null;
        }

        public static string[] CleanAndGetStrokesData(
            string text,
            DataSepparType cleanType = DataSepparType.Main,
            DataSepparType splitType = DataSepparType.Stroke,
            bool hasRange = false)
        {
            text = CleanData(text, cleanType);
            string[] data = GetStrokesData(text, splitType);
            if (hasRange)
            {
                return ReplaceRangePlaces(data);
            }
            return data;
        }

        public static string[] ReplaceRangePlaces(string[] textParts)
        {
            List<string> resultData = new List<string>();

            foreach (var textPart in textParts)
            {
                string[] strTextIndexes = textPart.Split(rangeSeppar);
                if (strTextIndexes.Length == 2)
                {
                    int startIndex = int.Parse(strTextIndexes[0]);
                    int endIndex = int.Parse(strTextIndexes[1]);
                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        resultData.Add(i.ToString());
                    }
                }
                else
                {
                    resultData.Add(textPart);
                }
            }
            
            return resultData.ConvertTo<string[]>();
        }

        public static Color GetColorFromString(string colorData)
        {
            if (string.IsNullOrEmpty(colorData)) return Color.clear;
            
            string[] RGBValues = CleanAndGetStrokesData(colorData,
                cleanType: DataSepparType.Addition, DataSepparType.Addition);
            int[] RGB = new[] {
                int.Parse(RGBValues[0]),
                int.Parse(RGBValues[1]),
                int.Parse(RGBValues[2])
            };
            return new Color(RGB[0] / 255f, RGB[1] / 255f, RGB[2] / 255f);
        }
    }
}