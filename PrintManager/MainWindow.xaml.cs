using PrintManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PrintManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PrintManagerViewModel PrintManagerModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            PrintManagerModel = new PrintManagerViewModel();
            //PrintManagerModel.AddDoc(new DocGridItem("qwe1", 1) { Id = 1 });
            //PrintManagerModel.AddDoc(new DocGridItem("qwe2", 2) { Id = 2 });
            //PrintManagerModel.AddDoc(new DocGridItem("qwe3", 3) { Id = 3 });
            docList.ItemsSource = PrintManagerModel.Docs;
        }

        private async void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;
            await PrintManagerModel.StartPrinting();
            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            PrintManagerModel.StopPrinting();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            PrintManagerModel.AddNewDoc();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            PrintManagerModel.CancelDocPrint((DocGridItem)docList.SelectedItem);
        }

        private void BtnAvgPrintTime_Click(object sender, RoutedEventArgs e)
        {
            var printedDocs = PrintManagerModel.Docs.Where(s => s.Printed == Enums.DocStatusEnum.Printed);
            if (!printedDocs.Any()) return;
            MessageBox.Show($"Среднее время печати: {printedDocs.Average(s => s.PrintTime)} сек.");
        }

        private void BtnDocsInfo_Click(object sender, RoutedEventArgs e)
        {
            var printedDocs = PrintManagerModel.Docs.Where(s => s.Printed == Enums.DocStatusEnum.Printed);
            if (!printedDocs.Any()) return;
            MessageBox.Show($"Напечатанные документы:{Environment.NewLine}{string.Join(Environment.NewLine, printedDocs.Select(s => s.Name))}");

        }
    }
}
