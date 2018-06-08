using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
namespace CarpenterWorkshopWPF
{
    /// <summary>
    /// Логика взаимодействия для FormWoodCraft.xaml
    /// </summary>
    public partial class FormWoodCraft : Window
    {       
        public int Id { set { id = value; } }

        private int? id;

        private List<BlankCraftViewModel> blankCrafts;

        public FormWoodCraft()
        {
            InitializeComponent();
            Loaded += FormWoodCraft_Load;          
        }

        private void FormWoodCraft_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = APIClient.GetRequest("api/WoodCraft/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var WoodCraft = APIClient.GetElement<WoodCraftViewModel>(response);
                        textBoxName.Text = WoodCraft.WoodCraftsName;
                        textBoxPrice.Text = WoodCraft.Price.ToString();
                        blankCrafts = WoodCraft.BlanksCrafts;
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
            else
                blankCrafts = new List<BlankCraftViewModel>();
        }

        private void LoadData()
        {
            try
            {
                if (blankCrafts != null)
                {
                    dataGridViewWoodCraft.ItemsSource = null;
                    dataGridViewWoodCraft.ItemsSource = blankCrafts;
                    dataGridViewWoodCraft.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewWoodCraft.Columns[1].Visibility = Visibility.Hidden;
                    dataGridViewWoodCraft.Columns[2].Visibility = Visibility.Hidden;
                    dataGridViewWoodCraft.Columns[3].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormWoodBlankWoodCraft();
            if (form.ShowDialog() == true)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                        form.Model.WoodCraftsID = id.Value;
                    blankCrafts.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewWoodCraft.SelectedItem != null)
            {
                var form = new FormWoodBlankWoodCraft();
                form.Model = blankCrafts[dataGridViewWoodCraft.SelectedIndex];
                if (form.ShowDialog() == true)
                {
                    blankCrafts[dataGridViewWoodCraft.SelectedIndex] = form.Model;
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewWoodCraft.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        blankCrafts.RemoveAt(dataGridViewWoodCraft.SelectedIndex);
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (blankCrafts == null || blankCrafts.Count == 0)
            {
                MessageBox.Show("Заполните заготовки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                List<BlankCraftBindingModel> productComponentBM = new List<BlankCraftBindingModel>();
                for (int i = 0; i < blankCrafts.Count; ++i)
                {
                    productComponentBM.Add(new BlankCraftBindingModel
                    {
                        Id = blankCrafts[i].Id,
                        WoodCraftsID = blankCrafts[i].WoodCraftsID,
                        WoodBlanksID = blankCrafts[i].WoodBlanksID,
                        Count = blankCrafts[i].Count
                    });
                }
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = APIClient.PostRequest("api/WoodCraft/UpdElement", new WoodCraftBindingModel
                    {
                        Id = id.Value,
                        WoodCraftsName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        BlanksCrafts = productComponentBM
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/WoodCraft/AddElement", new WoodCraftBindingModel
                    {
                        WoodCraftsName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        BlanksCrafts = productComponentBM
                    });
                }
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
