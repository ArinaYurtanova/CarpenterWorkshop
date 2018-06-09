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
    /// Логика взаимодействия для FormWoodCrafts.xaml
    /// </summary>
    public partial class FormWoodCrafts : Window
    {
       
        public FormWoodCrafts()
        {
            InitializeComponent();
            Loaded += FormWoodCrafts_Load;
        }

        private void FormWoodCrafts_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<WoodCraftViewModel> list = Task.Run(() => APIClient.GetRequestData<List<WoodCraftViewModel>>("api/WoodCraft/GetList")).Result;
                if (list != null)
                    {
                        dataGridViewWoodCrafts.ItemsSource = list;
                        dataGridViewWoodCrafts.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewWoodCrafts.Columns[1].Width = DataGridLength.Auto;
                        dataGridViewWoodCrafts.Columns[3].Visibility = Visibility.Hidden;
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
            var form = new FormWoodCraft();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewWoodCrafts.SelectedItem != null)
            {
                var form = new FormWoodCraft();
                form.Id = ((WoodCraftViewModel)dataGridViewWoodCrafts.SelectedItem).Id;
                if (form.ShowDialog() == true)
                    LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewWoodCrafts.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {

                    int id = ((WoodCraftViewModel)dataGridViewWoodCrafts.SelectedItem).Id;

                    Task task = Task.Run(() => APIClient.PostRequestData("api/WoodCraft/DelElement", new CustomerBidingModel { Id = id }));

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
