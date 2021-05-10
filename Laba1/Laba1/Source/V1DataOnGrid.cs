using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Serialization;

namespace Laba1
{
    [Serializable]
    public class V1DataOnGrid : V1Data, IEnumerable<DataItem>, ISerializable
    {
        public Grid Grid { get; }
        public Vector3[] Values { get; }

        public V1DataOnGrid(string info, DateTime date, Grid grid) : base(info, date )
        {
            Grid = grid;
            Values = new Vector3[grid.Number];
        }

        public void InitRandom(float minValue, float maxValue)
        {
            for (int i = 0; i < Values.Length; i++)
            {
                Values[i] = new Vector3();
                Random rand = new Random();
                Values[i].X = (float)(minValue + (maxValue - minValue) * rand.NextDouble());
                Values[i].Y = (float)(minValue + (maxValue - minValue) * rand.NextDouble());
                Values[i].Z = (float)(minValue + (maxValue - minValue) * rand.NextDouble());
            }
        }

        public static explicit operator V1DataCollection(V1DataOnGrid data)
        {
            V1DataCollection newData = new V1DataCollection(data.Info, data.Date);
            for (int i = 0; i < data.Values.Length; i++)
            {
                DataItem item = new DataItem(data.Grid.GetTime(i), data.Values[i]);
                newData.Values.Add(item);
            }
            return newData;
        }

        public override float[] NearZero(float eps)
        {
            List<float> result = new List<float>();
            for (int i = 0; i < Values.Length; i++)
            {
                Vector3 vector = Values[i];
                if (vector.Length() < eps)
                    result.Add(Grid.GetTime(i));
            }
            return result.ToArray();
        }

        public override string ToString()
        {
            return $"{GetType().Name} - {base.ToString()}, {Grid}";
        }

        public override string ToLongString()
        {
            string newStr = "\n";
            for (int i = 0; i < Values.Length; i++)
            {
                newStr += $"Time {Grid.GetTime(i)} \t Values {Values[i]}\n";
            }
            return $"{ToString()} \t {newStr}";
        }

        public override string ToLongString(string format)
        {
            string newStr = "\n";
            for (int i = 0; i < Values.Length; i++)
            {
                string time = Grid.GetTime(i).ToString(format);
                string value = Values[i].ToString(format);
                string length = Values[i].Length().ToString(format);
                newStr += $"Time_f {time}       Values_f {value}        Length_f {length}";
            }
            string result_str = ToString() + newStr + "\n";
            return result_str;
        }

        public override IEnumerator<DataItem> GetEnumerator()
        {
            return new DataEnumerator(Values, Grid);
        }

        private class DataEnumerator : IEnumerator<DataItem>
        {
            private Grid grid;
            private Vector3[] values;
            private int pos = -1;

            object IEnumerator.Current => Current;
            public DataItem Current
            {
                get
                {
                    return new DataItem(grid.GetTime(pos), values[pos]);
                }
            }

            public DataEnumerator(Vector3[] values, Grid grid)
            {
                this.values = values;
                this.grid = grid;
            }

            public bool MoveNext()
            {
                pos++;
                return pos < values.Length;
            }

            public void Reset()
            {
                pos = -1;
            }

            public void Dispose()
            {
                values = null;
            }


        }
        public V1DataOnGrid(SerializationInfo info, StreamingContext context)
                : base(info.GetString("baseInfo"), info.GetDateTime("baseDate"))
        {
            Grid = (Grid)info.GetValue("grid", typeof(Grid));
            Values = new Vector3[Grid.Number];
            for (int i = 0; i < Grid.Number; i++)
            {
                float x = info.GetSingle("Value_X_" + i.ToString());
                float y = info.GetSingle("Value_Y_" + i.ToString());
                float z = info.GetSingle("Value_Z_" + i.ToString());
                Values[i] = new Vector3(x, y, z);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("baseInfo", Info);
            info.AddValue("baseDate", Date);
            info.AddValue("grid", Grid, typeof(Grid));
            for (int i = 0; i < Grid.Number; i++)
            {
                info.AddValue("Value_X_" + i.ToString(), Values[i].X);
                info.AddValue("Value_Y_" + i.ToString(), Values[i].Y);
                info.AddValue("Value_Z_" + i.ToString(), Values[i].Z);
            }
        }
    }

}

