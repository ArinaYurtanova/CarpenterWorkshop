using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
                var response = APIClient.GetRequest("api/WoodCraft/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<WoodCraftViewModel> list = APIClient.GetElement<List<WoodCraftViewModel>>(response);
                    if (list != null)
                    {
                        dataGridViewWoodCrafts.ItemsSource = list;
                        dataGridViewWoodCrafts.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewWoodCrafts.Columns[1].Width = DataGridLength.Auto;
                        dataGridViewWoodCrafts.Columns[3].Visibility = Visibility.Hidden;
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
                    try
                    {
                        var response = APIClient.PostRequest("api/WoodCraft/DelElement", new CustomerBidingModel { Id = id });
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
