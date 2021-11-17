using PrintManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PrintManager
{
    /// <summary>
    /// Interaction logic for AddDocumentForm.xaml
    /// </summary>
    public partial class AddDocumentForm : Window
    {
        public DocGridItem NewDoc;
        public AddDocumentForm()
        {
            InitializeComponent();
        }

        private void CreateDoc_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(printTime?.Text) && !string.IsNullOrEmpty(docName?.Text);
        }

        private void CreateDoc_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NewDoc = new DocGridItem(docName.Text, Convert.ToInt32(printTime.Text));
            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void PrintTime_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }
    }
}
