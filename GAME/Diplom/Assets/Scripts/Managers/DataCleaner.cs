using Enums;

namespace Managers
{
    public static class DataCleaner
    {
        public static string mainSeppar = ";";
        public static string additionSeppar = ",";
        public static string strokeSeppar = "\n";

        public static string CleanData(string text, DataSepparType type = DataSepparType.Main)
        {
            if (type == DataSepparType.Main)
            {
                text = text.Replace(mainSeppar + " ", mainSeppar);
                return text;
            }
            text = text.Replace(additionSeppar + " ", additionSeppar);
            return text;
        }

        public static string[] GetStrokesData(string text, DataSepparType type)
        {
            if (type == DataSepparType.Addition) return text.Split(additionSeppar);
            if (type == DataSepparType.Main) return text.Split(mainSeppar);
            if (type == DataSepparType.Stroke) return text.Split(strokeSeppar);
            return null;
        }

        public static string[] CleanAndGetStrokesData(
            string text, DataSepparType type = DataSepparType.Main)
        {
            text = CleanData(text);
            return GetStrokesData(text, DataSepparType.Stroke);
        }
    }
}