using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManager
{
    public class PrintManagerCommands
    {
        static PrintManagerCommands()
        {
            CreateDoc = new System.Windows.Input.RoutedCommand("CreateDoc", typeof(MainWindow));
        }
        public static System.Windows.Input.RoutedCommand CreateDoc { get; set; }
    }
}
