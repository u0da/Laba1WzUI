using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace Laba1
{
    [Serializable]
    public abstract class V1Data : INotifyPropertyChanged, IEnumerable<DataItem>
    {
        private string info;
        private DateTime date;

        public string Info
        {
            get
            {
                return info;
            }
            set
            {
                info = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Info"));
            }
        }
        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Date"));
            }
        }
        public V1Data() {}
        public V1Data(string info, DateTime date)
        {
            Info = info;
            Date = date;
        }

        public event PropertyChangedEventHandler PropertyChanged; //  СОБЫТИЕ PropertyChanged, когда изменяются значения cвойств базового класса

        public abstract float[] NearZero(float eps);
        public abstract string ToLongString();
        public override string ToString()
        {
            return $"Date {Date.ToString(CultureInfo.GetCultureInfo("ru"))} \n Info {Info}";
        }

        public abstract string ToLongString(string format);
        public abstract IEnumerator<DataItem> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
