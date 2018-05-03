using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.ViewModels;
using System;
using System.Collections.Generic;
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
                var response = APIClient.GetRequest("api/Report/GetStoragesLoad");
                if (response.Result.IsSuccessStatusCode)
                {
                    dataGridView.Rows.Clear();
                    foreach (var elem in APIClient.GetElement<List<StoragesLoadViewModel>>(response))
                    {
                        dataGridView.Rows.Add(new object[] { elem.StorageName, "", "" });
                        foreach (var listElem in elem.WoodBlanks)
                        {
                            dataGridView.Rows.Add(new object[] { "", listElem.Item1, listElem.Item2 });
                        }
                        dataGridView.Rows.Add(new object[] { "Итого", "", elem.TotalCount });
                        dataGridView.Rows.Add(new object[] { });
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            catch (Exception ex)
            {
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
                try
                {
                    var response = APIClient.PostRequest("api/Report/SaveStoragesLoad", new ReportBindingModel
                    {
                        FileName = sfd.FileName
                    });
                    if (response.Result.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        throw new Exception(APIClient.GetError(response));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}