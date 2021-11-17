using PrintManager.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManager.ViewModels
{
    public class DocGridItem : INotifyPropertyChanged
    {
        public DocGridItem(string name, int printTime)
        {
            Name = name;
            PrintTime = printTime;
            Printed = DocStatusEnum.Queue;
        }
        internal int Id { get; set; }
        public string Name { get; set; }
        public int PrintTime { get; set; }

        private DocStatusEnum _printed;

        internal DocStatusEnum Printed
        {
            get => _printed;
            set
            {
                _printed = value;
                OnPropertyChanged("Printed");
                OnPropertyChanged("Status");
            }
        }
        public string Status
        {
            get
            {
                switch (Printed)
                {
                    case DocStatusEnum.Queue:
                        return "В очереди";
                    case DocStatusEnum.Printing:
                        return "Печатается...";
                    case DocStatusEnum.Printed:
                        return "Напечатан";
                    case DocStatusEnum.Cancel:
                        return "Отменен";
                    default:
                        return null;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
