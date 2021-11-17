using PrintManager.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PrintManager.ViewModels
{
    public class PrintManagerViewModel
    {
        internal System.Collections.ObjectModel.ObservableCollection<DocGridItem> Docs { get; set; }
        private System.Collections.ObjectModel.ObservableCollection<(DocGridItem document, Task docTask, System.Threading.CancellationTokenSource ctSource)> DocTasks { get; set; }
        private System.Threading.CancellationTokenSource ManagerCancellationTokenSource { get; set; }

        public PrintManagerViewModel()
        {
            DocTasks = new System.Collections.ObjectModel.ObservableCollection<(DocGridItem doc, Task docTask, System.Threading.CancellationTokenSource ctSource)>();
            Docs = new System.Collections.ObjectModel.ObservableCollection<DocGridItem>();
        }

        internal void AddNewDoc()
        {
            var form = new AddDocumentForm();
            if (form.ShowDialog() == true)
            {
                var doc = form.NewDoc;
                doc.Id = Docs.Any() ? Docs.Max(s => s.Id) + 1 : 1;
                Docs.Add(doc);
                //doc.CancellationTokenSource = new System.Threading.CancellationTokenSource();
                var ctSource = new System.Threading.CancellationTokenSource();
                DocTasks.Add((doc, new Task(() =>
                {
                    try
                    {
                        doc.Printed = DocStatusEnum.Printing;
                        Task.Delay(doc.PrintTime * 1000, ctSource.Token).Wait(ctSource.Token);
                        doc.Printed = DocStatusEnum.Printed;
                    }
                    catch (OperationCanceledException)
                    {
                        doc.Printed = DocStatusEnum.Cancel;
                        ManagerCancellationTokenSource.Token.ThrowIfCancellationRequested();
                    }
                }, ctSource.Token), ctSource));
            }
        }
        internal async Task StartPrinting()
        {
            ManagerCancellationTokenSource = new System.Threading.CancellationTokenSource();
            var mainTask = Task.Run(async () =>
            {
                try
                {
                    await PrintNextDoc();
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }, ManagerCancellationTokenSource.Token);

            await mainTask;
        }

        /// <summary>
        /// Метод печати следующего документа со статусом "В очереди"
        /// </summary>
        /// <returns></returns>
        private async Task PrintNextDoc()
        {
            var docTask = DocTasks.Any(s => s.document.Printed == DocStatusEnum.Queue) ? DocTasks.First(s => s.document.Printed == DocStatusEnum.Queue).docTask : null;
            if (docTask == null) return;
            docTask.Start();
            await docTask;
            await PrintNextDoc();
        }

        /// <summary>
        /// Метод отменяет печать текущего документа и останавливает менеджер печати
        /// </summary>
        internal void StopPrinting()
        {
            var ctSource = DocTasks.Any(s => s.document.Printed == DocStatusEnum.Printing) ? DocTasks.First(s => s.document.Printed == DocStatusEnum.Printing).ctSource : null;
            ctSource?.Cancel();
            ManagerCancellationTokenSource.Cancel();
        }

        /// <summary>
        /// отмена печати выбранного документа
        /// </summary>
        /// <param name="selectedItem">выбранный документ</param>
        internal void CancelDocPrint(DocGridItem selectedItem)
        {
            if (selectedItem == null || selectedItem.Printed == DocStatusEnum.Printed || selectedItem.Printed == DocStatusEnum.Cancel) return;
            selectedItem.Printed = DocStatusEnum.Cancel;
            var ctSource = DocTasks.First(s => s.document == selectedItem).ctSource;
            //if (selectedItem.Printed == DocStatusEnum.Printing)
            ctSource.Cancel();
        }
    }
}
