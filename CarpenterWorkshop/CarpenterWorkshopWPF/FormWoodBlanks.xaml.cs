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
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
                var response = APIClient.GetRequest("api/WoodBlank/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<WoodBlankViewModel> list = APIClient.GetElement<List<WoodBlankViewModel>>(response);
                    if (list != null)
                {
                    dataGridViewWoodBlanks.ItemsSource = list;
                    dataGridViewWoodBlanks.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewWoodBlanks.Columns[1].Width = DataGridLength.Auto;
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
                    try
                    {
                        var response = APIClient.PostRequest("api/WoodBlank/DelElement", new CustomerBidingModel { Id = id });
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
