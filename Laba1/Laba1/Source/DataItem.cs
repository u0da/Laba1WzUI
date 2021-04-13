using System;
using System.Globalization;
using System.Numerics;
using System.Runtime.Serialization;

namespace Laba1
{
    [Serializable]
    public struct DataItem
    {
        public float T { get; }
        public Vector3 Value { get; }

        public DataItem(float t, Vector3 value)
        {
            T = t;
            Value = value;
        }

        public string ToString(string format)
        {
            CultureInfo.CurrentCulture = new CultureInfo(format);
            string res = ToString() + "\n vector length " + Value.Length()+ '\n';
            CultureInfo.CurrentCulture = CultureInfo.InstalledUICulture;
            return res;
        }

        public override string ToString()
        {
            return $" T {T}    Value {Value}";
        }

        public DataItem(SerializationInfo info)
        {
            T = info.GetSingle("T");
            float x = info.GetSingle("Value_X");
            float y = info.GetSingle("Value_Y");
            float z = info.GetSingle("Value_Z");
            Value = new Vector3(x, y, z);
        }

        public void GetObjectData(SerializationInfo info)
        {
            info.AddValue("T", T);
            info.AddValue("Value_X", Value.X);
            info.AddValue("Value_Y", Value.Y);
            info.AddValue("Value_Z", Value.Z);
        }
    }
}
