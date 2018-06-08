using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
namespace CarpenterWorkshopWPF
{
    /// <summary>
    /// Логика взаимодействия для FormCustomers.xaml
    /// </summary>
    public partial class FormCustomers : Window
    {
       
        public FormCustomers()
        {
            InitializeComponent();
            Loaded += FormCustomers_Load;
        }

        private void FormCustomers_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var response = APIClient.GetRequest("api/Customer/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<CustomerViewModel> list = APIClient.GetElement<List<CustomerViewModel>>(response);
                    if (list != null)
                    {
                        dataGridViewCustomers.ItemsSource = list;
                        dataGridViewCustomers.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewCustomers.Columns[1].Width = DataGridLength.Auto;
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

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormCustomer();
            if (form.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewCustomers.SelectedItem != null)
            {
                var form = new FormCustomer();
                form.Id = ((CustomerViewModel)dataGridViewCustomers.SelectedItem).Id;
                if (form.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewCustomers.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((CustomerViewModel)dataGridViewCustomers.SelectedItem).Id;
                    try
                    {
                        var response = APIClient.PostRequest("api/Customer/DelElement", new CustomerBidingModel { Id = id });
                        if (!response.Result.IsSuccessStatusCode)
                        {
                            throw new Exception(APIClient.GetError(response));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
