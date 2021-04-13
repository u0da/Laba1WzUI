using System;
using System.Globalization;
namespace Laba1
{
    [Serializable]
    public struct Grid
    {
        public float Start { get; }
        public float Step { get; }
        public int Number { get; }

        public Grid(float start, float step, int number)
        {
            Start = start;
            Step = step;
            Number = number;
        }

        public float GetTime(int n)
        {
            return Start + n * Step; 
        }

        public string ToString(string format)
        {
            CultureInfo.CurrentCulture = new CultureInfo(format);
            string res = ToString();
            CultureInfo.CurrentCulture = CultureInfo.InstalledUICulture;
            return res;
        }

        public override string ToString()
        {
            return GetType().Name + $"Start {Start}, Step {Step}, Number {Number}";
        }
    }
}
