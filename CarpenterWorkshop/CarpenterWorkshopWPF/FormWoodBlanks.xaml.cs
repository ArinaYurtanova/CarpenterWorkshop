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
    /// Логика взаимодействия для FormWoodBlanks.xaml
    /// </summary>
    public partial class FormWoodBlanks : Window
    {
        
        public FormWoodBlanks()
        {
            InitializeComponent();
            Loaded += FormWoodBlanks_Load;
        }

        private void FormWoodBlanks_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<WoodBlankViewModel> list = Task.Run(() => APIClient.GetRequestData<List<WoodBlankViewModel>>("api/WoodBlank/GetList")).Result;
                if (list != null)
                {
                    dataGridViewWoodBlanks.ItemsSource = list;
                    dataGridViewWoodBlanks.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewWoodBlanks.Columns[1].Width = DataGridLength.Auto;
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
            var form =new FormWoodBlank();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewWoodBlanks.SelectedItem != null)
            {
                var form = new FormWoodBlank();
                form.Id = ((WoodBlankViewModel)dataGridViewWoodBlanks.SelectedItem).Id;
                if (form.ShowDialog() == true)
                    LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewWoodBlanks.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((WoodBlankViewModel)dataGridViewWoodBlanks.SelectedItem).Id;
                    Task task = Task.Run(() => APIClient.PostRequestData("api/WoodBlank/DelElement", new CustomerBidingModel { Id = id }));

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
