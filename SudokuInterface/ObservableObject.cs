using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SudokuInterface
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(String name) 
        {
            if (PropertyChanged != null) 
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
