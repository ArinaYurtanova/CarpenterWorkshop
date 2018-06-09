using CarpenterWorkshopService.ViewModels;
using CarpenterWorkshopView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CarpenterWorkshopView;
using CarpenterWorkshopService.ViewModels;
using CarpenterWorkshopService.BindingModels;

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
                List<WorkerVeiwModel> list = Task.Run(() => APIClient.GetRequestData<List<WorkerVeiwModel>>("api/Worker/GetList")).Result;
                if (list != null)
                {
                    dataGridViewWorkers.ItemsSource = list;
                    dataGridViewWorkers.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewWorkers.Columns[1].Width = DataGridLength.Auto;
                }          
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
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
                    Task task = Task.Run(() => APIClient.PostRequestData("api/Worker/DelElement", new CustomerBidingModel { Id = id }));

                    task.ContinueWith((prevTask) => MessageBox.Show("Запись удалена. Обновите список", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
                    TaskContinuationOptions.OnlyOnRanToCompletion);

                    task.ContinueWith((prevTask) =>
                    {
                        var ex = (Exception)prevTask.Exception;
                        while (ex.InnerException != null)
                        {
                            ex = ex.InnerException;
                        }
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }, TaskContinuationOptions.OnlyOnFaulted);
                }
            }
        }
        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}