namespace Laba1
{
    class Program
    {
        static void Main(/*string[] args*/)
        {
            /* Lab1

               Console.WriteLine("\t1.\n");

               Grid grid1 = new Grid(0, 0.001f, 10);
               DateTime dateTime1 = new DateTime(2020, 07, 17, 22, 10, 57);

               V1DataOnGrid data1 = new V1DataOnGrid("id1", dateTime1, grid1);

               for (int i = 0; i < data1.Values.Length; i++)
               {
                   data1.Values[i] = new Vector3(3 * i * i + 3, 4 * i, i - 2) * 0.001f;
               }

               Console.WriteLine(data1.ToLongString());
               V1DataCollection data2 = (V1DataCollection)data1;
               Console.WriteLine(data2.ToLongString());

               //

               Console.WriteLine("\n\t2.\n");
               V1MainCollection mainCollection = new V1MainCollection();
               mainCollection.AddDefaults();
               Console.WriteLine(mainCollection);

               //

               Console.WriteLine("\n\t3.\n\n eps=0.5\n");
               PrintNearZero(mainCollection, 0.5f);
               Console.WriteLine("\n eps=0.2\n");
               PrintNearZero(mainCollection, 0.2f);
           */

            // Lab2

            //string format = "ru-Ru";
            //V1DataCollection dataCollection = new V1DataCollection("test.txt");
            //Console.WriteLine(dataCollection.ToLongString(format));

            //V1MainCollection data = new V1MainCollection();
            //data.AddDefaults();
            //Console.WriteLine(data);
            //Console.WriteLine("\nMax vector length: " + data.MaxNumberOfMesRes);

            //Console.WriteLine("\nFrom the longest to the shortest lenght : \n");
            //foreach (DataItem elem in data.EnumerationLength)
            //{
            //    Console.WriteLine(elem.ToString(format));
            //}

            //Console.WriteLine("Time: " + string.Join(", ", data.EnumerationTime));


            // Lab3
            V1MainCollection mainCollection = new V1MainCollection();
            //mainCollection.DataChanged += DataChangedEventHandler;
            mainCollection.AddDefaults();
            V1Data data1 = mainCollection[2];
            mainCollection.Remove(data1.Info, data1.Date);
            mainCollection[2] = data1;
            mainCollection[1].Info += " changes made";

            /*static void DataChangedEventHandler(object source, DataChangedEventArgs args)
            {
                Console.WriteLine(args);
            }*/


            /*static void PrintNearZero(V1MainCollection mainCollection, float eps)
            {

                foreach (V1Data data in mainCollection)
                {
                    float[] time = data.NearZero(eps);
                    string str = "";
                    for (int i = 0; i < time.Length; i++)
                    {
                        str += time[i];
                        if (i != time.Length - 1) str += ", ";
                    }

                    Console.WriteLine($"( {str} )");
                }
            }*/
        }
    }
}
