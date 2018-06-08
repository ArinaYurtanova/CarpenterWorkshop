using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.ViewModels;
using Microsoft.Win32;
namespace CarpenterWorkshopWPF
{
    /// <summary>
    /// Логика взаимодействия для FormMain.xaml
    /// </summary>
    public partial class FormMain : Window
    {
       
        public FormMain()
        {
            InitializeComponent();          
        }

        private void LoadData()
        {
            try
            {
                var response = APIClient.GetRequest("api/Main/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<OrdProductViewModel> list = APIClient.GetElement<List<OrdProductViewModel>>(response);
                    if (list != null)
                {
                    dataGridViewMain.ItemsSource = list;
                    dataGridViewMain.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewMain.Columns[1].Visibility = Visibility.Hidden;
                    dataGridViewMain.Columns[3].Visibility = Visibility.Hidden;
                    dataGridViewMain.Columns[5].Visibility = Visibility.Hidden;
                    dataGridViewMain.Columns[1].Width = DataGridLength.Auto;
                }
            }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void получателиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormCustomers();
            form.ShowDialog();
        }

        private void заготовкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormWoodBlanks();
            form.ShowDialog();
        }

        private void мебельToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormWoodCrafts();
            form.ShowDialog();
        }

        private void базыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormStorages();
            form.ShowDialog();
        }

        private void рабочиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormWorkers();
            form.ShowDialog();
        }

        private void пополнитьБазуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormPutOnStorage();
            form.ShowDialog();
        }

        private void buttonCreateOrder_Click(object sender, EventArgs e)
        {
            var form = new FormCreateOrder();
            form.ShowDialog();
            LoadData();
        }

        private void buttonTakeOrderInWork_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedItem != null)
            {
                var form = new FormTakeOrderInWork();
                form.Id = ((OrdProductViewModel)dataGridViewMain.SelectedItem).Id;
                form.ShowDialog();
                LoadData();
            }
        }

        private void buttonZayavkaReady_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedItem != null)
            {
                int id = ((OrdProductViewModel)dataGridViewMain.SelectedItem).Id;
                try
                {
                    var response = APIClient.PostRequest("api/Main/FinishOrder", new OrdProductBindingModel
                    {
                        Id = id
                    });
                    if (response.Result.IsSuccessStatusCode)
                    {
                        LoadData();
                    }
                    else
                    {
                        throw new Exception(APIClient.GetError(response));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonPayOrder_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedItem != null)
            {
                int id = ((OrdProductViewModel)dataGridViewMain.SelectedItem).Id;
                try
                {
                   var response = APIClient.PostRequest("api/Main/PayOrder", new OrdProductBindingModel
                    {
                        Id = id
                    });
                    if (response.Result.IsSuccessStatusCode)
                    {
                        LoadData();
                    }
                    else
                    {
                        throw new Exception(APIClient.GetError(response));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void прайсМебелиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "doc|*.doc|docx|*.docx"
            };

            if (sfd.ShowDialog() == true)
            {

                try
                {

                    var response = APIClient.PostRequest("api/Report/SaveProductPrice", new ReportBindingModel
                    {
                        FileName = sfd.FileName
                    });
                    if (response.Result.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        throw new Exception(APIClient.GetError(response));
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void загруженностьБазToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "xls|*.xls|xlsx|*.xlsx"
            };
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    var response = APIClient.PostRequest("api/Report/SaveStoragesLoad", new ReportBindingModel
                    {
                        FileName = sfd.FileName
                    });
                    if (response.Result.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        throw new Exception(APIClient.GetError(response));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void заказыПолучателейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormCustomerOrders();
            form.ShowDialog();
        }
    }
}
