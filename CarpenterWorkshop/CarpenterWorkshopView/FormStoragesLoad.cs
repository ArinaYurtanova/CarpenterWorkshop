using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarpenterWorkshopView
{

    public partial class FormStoragesLoad : Form
    {
        public FormStoragesLoad()
        {
            InitializeComponent();
        }

        private void FormStoragesLoad_Load(object sender, EventArgs e)
        {
            try
            {
                dataGridView.Rows.Clear();
                foreach (var elem in Task.Run(() => APIClient.GetRequestData<List<StoragesLoadViewModel>>("api/Report/GetStoragesLoad")).Result)
                {
                    dataGridView.Rows.Add(new object[] { elem.StorageName, "", "" });
                    foreach (var listElem in elem.WoodBlanks)
                    {
                        dataGridView.Rows.Add(new object[] { "", listElem.WoodBlanksName, listElem.Count });
                    }
                    dataGridView.Rows.Add(new object[] { "Итого", "", elem.TotalCount });
                    dataGridView.Rows.Add(new object[] { });
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSaveToExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "xls|*.xls|xlsx|*.xlsx"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string fileName = sfd.FileName;
                Task task = Task.Run(() => APIClient.PostRequestData("api/Report/SaveStoragesLoad", new ReportBindingModel
                {
                    FileName = fileName
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Выполнено", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information),
                    TaskContinuationOptions.OnlyOnRanToCompletion);
                task.ContinueWith((prevTask) =>
                {
                    var ex = (Exception)prevTask.Exception;
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }, TaskContinuationOptions.OnlyOnFaulted);
            }
        }
    }
}