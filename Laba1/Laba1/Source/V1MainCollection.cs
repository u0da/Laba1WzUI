using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Laba1
{
    [Serializable]
    enum ChangeInfo
    {
        ItemChanged,
        Add,
        Remove,
        Replace
    }

    delegate void DataChangedEventHandler(object source, DataChangedEventArgs args);

    class DataChangedEventArgs
    {
        ChangeInfo CInfo { get; set; }

        string Str { get; set; }

        public DataChangedEventArgs(ChangeInfo change, string str)
        {
            CInfo = change;
            Str = str;
        }

        public override string ToString()
        {
            return GetType().Name + $" CInfo {CInfo} \t Str {Str}";
        }
    }


    public class V1MainCollection : IEnumerable<V1Data>, INotifyCollectionChanged, INotifyPropertyChanged
    {

        delegate void DataChangedEventHandler(object source, DataChangedEventArgs args);

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void OnCollectionChanged(NotifyCollectionChangedAction ev)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        class DataChangedEventArgs
    {
        ChangeInfo CInfo { get; set; }

        string Str { get; set; }

        public DataChangedEventArgs(ChangeInfo change, string str)
        {
            CInfo = change;
            Str = str;
        }

        public override string ToString()
        {
            return GetType().Name + $" CInfo {CInfo} \t Str {Str}";
        }
    }


        private List<V1Data> DataFields = new List<V1Data>(); // readonly deleted

       
        public bool UserCollectionChanged { get; private set; }

        public V1MainCollection()
        {
            CollectionChanged += OnChange;
        }

        private void OnChange(object sender, NotifyCollectionChangedEventArgs args)
        {
            UserCollectionChanged = true;
        }

        public void Save(string filename)
        {
            FileStream fileStream = null;
            try
            {
                fileStream = File.Open(filename, FileMode.OpenOrCreate);
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(fileStream, DataFields);
                UserCollectionChanged = false;
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
                UserCollectionChanged = false;
                OnPropertyChanged("UserCollectionChanged");
            }
        }
        public void Load(string filename)
        {
            FileStream fileStream = null;

            try
            {
                fileStream = File.OpenRead(filename);
                BinaryFormatter serializer = new BinaryFormatter();
                DataFields = (List<V1Data>)serializer.Deserialize(fileStream);
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                UserCollectionChanged = false;
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();

                UserCollectionChanged = true;
                OnCollectionChanged(NotifyCollectionChangedAction.Add);
                OnPropertyChanged("UserCollectionChanged");
                OnPropertyChanged("Count");
                OnPropertyChanged("MaxNumberofMesRes");
            }
        }

        //readonly DataChangedEventHandler DataChanged;
        //индексатор
        public V1Data this[int index]
        {
            get
            {
                return DataFields[index];
            }
            set
            {
                if (DataFields[index] != value)  // получаем через параметр value переданный объект v1data и сохраняем его в массив по индексу                                                 
                {
                    //DataChanged(this, new DataChangedEventArgs(ChangeInfo.Replace, DataFields[index].Info));
                    DataFields[index].PropertyChanged -= OnPropertyChanged;
                    value.PropertyChanged += OnPropertyChanged;
                    DataFields[index] = value;

                }
            }
        }
        public int Count
        {
            get
            {
                if (DataFields == null)
                    return 0;
                else
                    return DataFields.Count;
            }
        }

        public int MaxNumberOfMesRes
        {
            get
            {
                if(Count == 0) { return 0; }
                int selector(V1Data selection)
                {
                    var query = from dataitem in selection
                                select dataitem;
                    return query.Count();
                }
                return DataFields.Max(selector);
            }
        }

        public IEnumerable<DataItem> EnumerationLength
        {
            get
            {
                var query = from v1dataOb in DataFields
                            from dataitem in v1dataOb
                            orderby dataitem.Value.Length() descending
                            select dataitem;
                return query;
            }
        }

        public IEnumerable<float> EnumerationTime
        {
            get
            {
                var TimeQuery = from v1dataOb in DataFields            //отбираем время
                                from dataitem in v1dataOb
                                group dataitem by dataitem.T;
                var query = from time in TimeQuery                     //отбираем только 1 раз
                            where time.Count() == 1
                            select time.Key;
                return query;
            }
        }


        void Add(V1Data item)
        {
            try
            {
                DataFields.Add(item);
                OnCollectionChanged(NotifyCollectionChangedAction.Add);
                UserCollectionChanged = true;
                OnPropertyChanged("UserCollectionChanged");
                OnPropertyChanged("Count");
                OnPropertyChanged("MaxNumberofMesRes");
                //item.PropertyChanged += onPropertyChanged;
                // DataChanged(this, new DataChangedEventArgs(ChangeInfo.Add, item.Info));}
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool Remove(string id, DateTime dateTime)
        {
            bool result = false;
            for(int i = 0; i<DataFields.Count; i++)
            {
                if(Equals(DataFields[i].Info, id) == true && DataFields[i].Date.CompareTo(dateTime) == 0)
                {
                    DataFields.RemoveAt(i);
                    OnPropertyChanged("UserCollectionChanged");
                    OnCollectionChanged(NotifyCollectionChangedAction.Remove);
                    OnPropertyChanged("Count");
                    //item.PropertyChanged -= onPropertyChanged;
                    //DataChanged(this, new DataChangedEventArgs(ChangeInfo.Remove, item.Info));
                    result = true;
                }
            }
            return result;
        }

        public void AddDefaults()
        {
            //for (int i = 0; i < 2; i++) // length of vectors
            //{
            //    V1DataOnGrid data = new V1DataOnGrid($"id={i}", DateTime.Now, new Grid(0, 0.5f, 3));
            //    data.InitRandom(-1f, 1f);
            //    Add(data);
            //}
            //for (int i = 1; i <= 2; i++) // amount of vectors
            //{
            //    V1DataCollection data = new V1DataCollection($"id={i}", DateTime.Now);
            //    data.InitRandom(10 + 2 * i, 0f, 100f, -1f, 1f);
            //    Add(data);
            //}

            //Add(new V1DataOnGrid($"grid", DateTime.Now, new Grid(1f, 0, 0)));
            //Add(new V1DataCollection($"collection", DateTime.Now));

            V1DataOnGrid data = new V1DataOnGrid($"id={0}", DateTime.Now, new Grid(0, 0.5f, 3));
            data.InitRandom(-1f, 1f);
            Add(data);

            data = new V1DataOnGrid($"id={1}", DateTime.Now, new Grid(0, 0.5f, 5));
            data.InitRandom(-1f, 1f);
            Add(data);

            V1DataCollection datacol = new V1DataCollection($"id={2}", DateTime.Now);
            datacol.InitRandom(12, 0f, 100f, -1f, 1f);
            Add(datacol);

            datacol = new V1DataCollection($"id={3}", DateTime.Now);
            datacol.InitRandom(12, 0f, 100f, -1f, 1f);
            Add(datacol);
            OnCollectionChanged(NotifyCollectionChangedAction.Add);
            OnPropertyChanged("Count");
            OnPropertyChanged("MaxNumberofMesRes");
            UserCollectionChanged = true;
        }


        public override string ToString()
        {
            string result = string.Join<V1Data>("\n", DataFields) + "\n";
            return $"{GetType().Name} \n {result}";
        }

        public IEnumerator GetEnumerator() //ОТЕЦ  public interface IEnumerable<out T> : IEnumerable
        {
            return ((IEnumerable)DataFields).GetEnumerator();
        }

        IEnumerator<V1Data> IEnumerable<V1Data>.GetEnumerator() // СЫН  public interface IEnumerable
        {                                                       //IEnumerator GetEnumerator();

            return ((IEnumerable<V1Data>)DataFields).GetEnumerator();
        }

        public string ToLongString(string format)
        {
            string result = "";
            for (int i = 0; i < Count; i++)
            {
                result += DataFields[i].ToLongString(format);
            }
            return result;
        }

        
        private void OnPropertyChanged(object source, PropertyChangedEventArgs args)
        {
            string info = ((V1Data)source).Info + "\t" + args.PropertyName;
            //DataChanged(this, new DataChangedEventArgs(ChangeInfo.ItemChanged, info));
        }

       
        public void AddDefaultDataCollection()
        {
            V1DataCollection dataCollection = new V1DataCollection("default collection");
            dataCollection.InitRandom(10, 5, 10, 5, 10);
            Add(dataCollection);
        }

        public void AddDefaultDataOnGridCollection()
        {
            Grid grid = new Grid();
            DateTime dateTime = new DateTime(2020, 07, 17, 22, 10, 57);
            V1DataOnGrid dataOnGrid = new V1DataOnGrid("default data on grid",dateTime, grid);
            dataOnGrid.InitRandom(5, 10);
            Add(dataOnGrid);
        }

        public void AddElementFromFile(string filename)
        {
            try
            {
                V1DataCollection dataCollection = new V1DataCollection(filename);
                Add(dataCollection);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }

}