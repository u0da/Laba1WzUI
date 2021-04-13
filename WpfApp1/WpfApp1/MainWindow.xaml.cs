using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Laba1;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private V1MainCollection MainColl = new V1MainCollection();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = MainColl;
        }

        private void ClosingWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MainColl.UserCollectionChanged)
            {
                e.Cancel = UnsavedChangesSaving();
            }
        }


        private bool UnsavedChangesSaving()
        {
            MessageBoxResult message = MessageBox.Show("Do you want to save the file? All unsaved data will be lost!", "ATTENTION!", MessageBoxButton.YesNoCancel);
            if (message == MessageBoxResult.Yes)
            {
                Microsoft.Win32.SaveFileDialog FileDia = new Microsoft.Win32.SaveFileDialog();
                if ((bool)FileDia.ShowDialog())
                    MainColl.Save(FileDia.FileName);
            }
            else if (message == MessageBoxResult.Cancel)
            {
                return true;
            }
            return false;
        }

        /*********************************************************************** HANDLERS*****************************************************************/
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainColl.UserCollectionChanged)
            {
                UnsavedChangesSaving();
            }
            MainColl = new V1MainCollection();
            DataContext = MainColl;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog FileDia = new Microsoft.Win32.SaveFileDialog();
                if ((bool)FileDia.ShowDialog())
                    MainColl.Save(FileDia.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message) ;
            }
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MainColl.UserCollectionChanged)
                {
                    UnsavedChangesSaving();
                }
                Microsoft.Win32.OpenFileDialog FileDia = new Microsoft.Win32.OpenFileDialog();
                if ((bool)FileDia.ShowDialog())
                {
                    MainColl = new V1MainCollection();
                    MainColl.Load(FileDia.FileName);
                    DataContext = MainColl;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
    }
            

        private void AddDefaultsV1DataCollectionButton_Click(object sender, RoutedEventArgs e)
        {
            MainColl.AddDefaultDataCollection();
        }

        private void AddDefaultsButton_Click(object sender, RoutedEventArgs e)
        {
            MainColl.AddDefaults();
            DataContext = MainColl;
        }

        private void AddDefaultV1DataOnGridButton_Click(object sender, RoutedEventArgs e)
        {
            MainColl.AddDefaultDataOnGridCollection();
            
        }

        private void AddElementFromFileButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog FileDia = new Microsoft.Win32.OpenFileDialog();
                if ((bool)FileDia.ShowDialog())
                    MainColl.AddElementFromFile(FileDia.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var UserSelectedListBox = lisBox_Main.SelectedItems;
            List<V1Data> V1DataList = new List<V1Data>();
            V1DataList.AddRange(UserSelectedListBox.Cast<V1Data>());
            foreach (V1Data item in V1DataList)
            {
                MainColl.Remove(item.Info, item.Date);
            }
        }

        private void DataCollectionSubset(object sender, FilterEventArgs args) // выбираем эл-ты типа v1datacollection
        {
            var item = args.Item;
            if (item != null)
            {
                if (item.GetType() == typeof(V1DataCollection)) args.Accepted = true;
                else args.Accepted = false;
            }
        }
        private void DataOnGridSubset(object sender, FilterEventArgs args)
        {
            var item = args.Item;
            if (item != null)
            {
                if (item.GetType() == typeof(V1DataOnGrid)) args.Accepted = true;
                else args.Accepted = false;
            }
        }
    }
}
