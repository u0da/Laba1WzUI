using System;
using System.ComponentModel;
using Laba1;

namespace WpfApp1
{
    class DataOnGridBinding: IDataErrorInfo, INotifyPropertyChanged
    {
        private string str;
        private int number;
        private float min, max;

        public V1MainCollection MainColl;

        public DataOnGridBinding(ref V1MainCollection collection)
        {
            MainColl = collection;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public string Error
        {
            get
            {
                return "Error text";
            }
        }

        public string this[string PropertyString]
        {
            get
            {
                string message = null;

                switch (PropertyString)
                {
                    case "StringInfo":
                        if (MainColl == null)
                        {
                            break;
                        }
                        if (StringInfo == null)
                        {
                            message = "Info should contain something!";
                        }
                        else if (StringInfo.Length < 1)
                        {
                            message = "Info should contain more than one symbol!";
                        }
                        else
                        {
                            foreach (V1Data item in MainColl)
                            {
                                if (item.Info.Equals(StringInfo))
                                {
                                    message = "V1DataOnGrid element with the same name already exists!";
                                }
                            }
                        }
                        break;

                    case "Number":
                        if (Number <= 2)
                        {
                            message = "Number of grid nodes should be more than 2!";
                        }
                        break;

                    case "MinValue":
                        if (MinValue >= MaxValue)
                        {
                            message = "Min value should be less than max value!";
                        }
                        break;

                    case "MaxValue":
                        if (MaxValue <= MinValue)
                        {
                            message = "Max value should be greater than min value!";
                        }
                        break;

                    default:
                        break;
                }
                return message;
            }
        }

        public string StringInfo
        {
            get
            {
                return str;
            }
            set
            {
                str = value;
                OnPropertyChanged("StringInfo");
            }
        }

        public int Number
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
                OnPropertyChanged("Number");
            }
        }

        public float MinValue
        {
            get
            {
                return min;
            }
            set
            {
                min = value;
                OnPropertyChanged("MinValue");
            }
        }

        public float MaxValue
        {
            get
            {
                return max;
            }
            set
            {
                max = value;
                OnPropertyChanged("MaxValue");
            }
        }

        public void AddCustom()
        {
            V1DataOnGrid data = new V1DataOnGrid(StringInfo, DateTime.Now, new Grid(0.0f, 0.2f, Number));
            data.InitRandom(MinValue, MaxValue);
            MainColl.Add(data);
            OnPropertyChanged("StringInfo");
            OnPropertyChanged("Number");
            OnPropertyChanged("MinValue");
            OnPropertyChanged("MaxValue");
        }
    }
}
