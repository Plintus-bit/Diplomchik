using System;
namespace Managers
{
    public static class UsefullDataGetter
    {
        public static int GetUsefulTicksData(long value, int fullPartDegree, int fractPartDegree)
        {
            Math.DivRem(value, (long) Math.Pow(10, fullPartDegree), out long data);
            data %= (long)Math.Pow(10, fractPartDegree);
            return (int) data;
        }
        
        public static int GetUsefulTicksData(int fullPartDegree = 6, int fractPartDegree = 3)
        {
            return GetUsefulTicksData(DateTime.Now.Ticks, fullPartDegree, fractPartDegree);
        }

        public static int GetDeltaTicks(
            int withWhatCompare,
            int fullPartDegree = 6, int fractPartDegree = 3)
        {
            int result = GetUsefulTicksData(
                fullPartDegree, fractPartDegree)
                         - withWhatCompare;
            return result;
        }
    }
}