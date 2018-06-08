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
    /// Логика взаимодействия для FormWorkers.xaml
    /// </summary>
    public partial class FormWorkers : Window
    {        

        public FormWorkers()
        {
            InitializeComponent();
            Loaded += FormWorkers_Load;           
        }

        private void FormWorkers_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var response = APIClient.GetRequest("api/Worker/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<WorkerVeiwModel> list = APIClient.GetElement<List<WorkerVeiwModel>>(response);
                    if (list != null)
                {
                    dataGridViewWorkers.ItemsSource = list;
                    dataGridViewWorkers.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewWorkers.Columns[1].Width = DataGridLength.Auto;
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
            var form = new FormWorker();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewWorkers.SelectedItem != null)
            {
                var form = new FormWorker();
                form.Id = ((WorkerVeiwModel)dataGridViewWorkers.SelectedItem).Id;
                if (form.ShowDialog() == true)
                    LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewWorkers.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((WorkerVeiwModel)dataGridViewWorkers.SelectedItem).Id;
                    try
                    {
                        var response = APIClient.PostRequest("api/Worker/DelElement", new CustomerBidingModel { Id = id });
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