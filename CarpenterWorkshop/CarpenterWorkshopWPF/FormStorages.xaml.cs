using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.ViewModels;
using CarpenterWorkshopView;
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
    /// Логика взаимодействия для FormStorages.xaml
    /// </summary>
    public partial class FormStorages : Window
    {
       
        public FormStorages()
        {
            InitializeComponent();
            Loaded += FormStorages_Load;            
        }

        private void FormStorages_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<StorageViewModel> list = Task.Run(() => APIClient.GetRequestData<List<StorageViewModel>>("api/Storage/GetList")).Result;
                if (list != null)
                    {
                        dataGridViewStorages.ItemsSource = list;
                        dataGridViewStorages.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewStorages.Columns[1].Width = DataGridLength.Auto;
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
            var form = new FormStorage();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewStorages.SelectedItem != null)
            {
                var form = new FormStorage();
                form.Id = ((StorageViewModel)dataGridViewStorages.SelectedItem).Id;
                if (form.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewStorages.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((StorageViewModel)dataGridViewStorages.SelectedItem).Id;
                    Task task = Task.Run(() => APIClient.PostRequestData("api/Storage/DelElement", new CustomerBidingModel { Id = id }));

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
