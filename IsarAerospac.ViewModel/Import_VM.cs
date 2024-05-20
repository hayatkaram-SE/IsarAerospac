using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using IsarAerospac.Model;
using Microsoft.Win32;

namespace IsarAerospac.ViewModel
{
    public class Import_VM : INotifyPropertyChanged
    {
        private ObservableCollection<Import> _bookList;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        private ICommand _importButton;
        /// <summary>
        /// open import window
        /// </summary>
        public ICommand ImportButton
        {
            get
            {
                if (_importButton == null)
                    _importButton = new ImportExcel(ExecuteMethod, d => true);
                return _importButton;

            }
            set { _importButton = value; }
        }
        private ICommand _cancelButton;
        /// <summary>
        /// Close window
        /// </summary>
        public ICommand CancelButton
        {
            get
            {
                if (_cancelButton == null)
                    _cancelButton = new ImportExcel(ExecuteCancelMethod, d => true);
                return _cancelButton;

            }
            set { _cancelButton = value; }
        }
        private ICommand _descriptionButton;
        /// <summary>
        /// show description
        /// </summary>
        public ICommand DescriptionButton
        {
            get
            {
                if (_descriptionButton == null)
                    _descriptionButton = new ImportExcel(ExecuteDescriptionMethod, d => true);
                return _descriptionButton;

            }
            set { _descriptionButton = value; }
        }
        private ICommand _deleteButton;
        /// <summary>
        /// Delete a row from data grid
        /// </summary>
        public ICommand DeleteButton
        {
            get
            {
                if (_deleteButton == null)
                    _deleteButton = new ImportExcel(ExecuteDeleteMethod, d => true);
                return _deleteButton;

            }
            set { _deleteButton = value; }
        }
        /// <summary>
        /// Import CSV file
        /// </summary>
        /// <param name="parameter"></param>
        private void ExecuteMethod(object parameter)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            Nullable<bool> result = ofd.ShowDialog();
            if (result == true)
            {
                ReadImportedCSV(ofd.FileName);

            }
        }
        private void ExecuteCancelMethod(object parameter)
        {
            if (parameter is Window)
            {
                Window x = (Window)parameter;
                x.Close();
            }
        }
        /// <summary>
        /// Show description in a message box
        /// </summary>
        /// <param name="parameter"></param>
        private void ExecuteDescriptionMethod(object parameter)
        {
            var itemList = (parameter as ObservableCollection<object>).Cast<Import>().ToList();
            MessageBox.Show(itemList[0].Description.ToString());
        }
        /// <summary>
        /// Delete a row
        /// </summary>
        /// <param name="parameter"></param>
        private void ExecuteDeleteMethod(object parameter)
        {
            var itemList = (parameter as ObservableCollection<object>).Cast<Import>().ToList();
            BookList.Remove(itemList[0]);
        }
        public ObservableCollection<Import> BookList
        {
            get { return _bookList; }
            set
            {
                _bookList = value;
                OnPropertyChanged("BookList");
            }
        }
        /// <summary>
        /// Read a csv file and create list of import classes
        /// </summary>
        /// <param name="fileName"></param>
        public void ReadImportedCSV(string fileName)
        {
            try
            {
                string[] csv = File.ReadAllLines(System.IO.Path.ChangeExtension(fileName, ".csv"));
                csv = csv.Skip(1).ToArray();
                var temp = csv.Select(line =>
                  {
                      string[] data = line.Split(';');
                      try
                      {
                          var isChecked = false;
                          if (data[4] == "yes")
                              isChecked = true;
                          return new Import(data[0], data[1], data[2], data[3], data[5], 99, isChecked, data[6]);
                      }
                      catch (Exception ex)
                      {

                          throw ex;
                      }

                  });
                _bookList = new ObservableCollection<Import>(temp.Cast<Import>());
                BookList = _bookList;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

    }
    public class ImportExcel : ICommand
    {
        Action<object> _executeButtonMethod;
        Func<object, bool> _canExecuteButtonMethod;
        public ImportExcel(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            _executeButtonMethod = executeMethod;
            _canExecuteButtonMethod = canExecuteMethod;
        }
        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested += value;
            }
        }

        bool ICommand.CanExecute(object parameter)
        {
            if (_canExecuteButtonMethod != null)
                return _canExecuteButtonMethod(parameter);
            else
                return false;
        }

        void ICommand.Execute(object parameter)
        {
            _executeButtonMethod(parameter);
        }
    }
}
