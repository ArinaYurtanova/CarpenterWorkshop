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
    /// Логика взаимодействия для FormCreateOrder.xaml
    /// </summary>
    public partial class FormCreateOrder : Window
    {       
        public FormCreateOrder()
        {
            InitializeComponent();
            Loaded += FormCreateOrder_Load;
            comboBoxWoodCraft.SelectionChanged += comboBoxWoodCraft_SelectedIndexChanged;
            comboBoxWoodCraft.SelectionChanged += new SelectionChangedEventHandler(comboBoxWoodCraft_SelectedIndexChanged);           
        }

        private void FormCreateOrder_Load(object sender, EventArgs e)
        {
            try
            {
                var responseC = APIClient.GetRequest("api/Customer/GetList");
                if (responseC.Result.IsSuccessStatusCode)
                {
                    List<CustomerViewModel> list = APIClient.GetElement<List<CustomerViewModel>>(responseC);
                    if (list != null)
                    {
                        comboBoxClient.DisplayMemberPath = "CustomerFIO";
                        comboBoxClient.SelectedValuePath = "Id";
                        comboBoxClient.ItemsSource = list;
                        comboBoxWoodCraft.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseC));
                }
                var responseP = APIClient.GetRequest("api/WoodCraft/GetList");
                if (responseP.Result.IsSuccessStatusCode)
                {
                    List<WoodCraftViewModel> list = APIClient.GetElement<List<WoodCraftViewModel>>(responseP);
                    if (list != null)
                    {
                        comboBoxWoodCraft.DisplayMemberPath = "WoodCraftsName";
                        comboBoxWoodCraft.SelectedValuePath = "Id";
                        comboBoxWoodCraft.ItemsSource = list;
                        comboBoxWoodCraft.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseP));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalcSum()
        {
            if (comboBoxWoodCraft.SelectedItem != null && !string.IsNullOrEmpty(textBoxCount.Text))
            {
                try
                {
                    int id = ((WoodCraftViewModel)comboBoxWoodCraft.SelectedItem).Id;
                    var responseP = APIClient.GetRequest("api/WoodCraft/Get/" + id);
                    if (responseP.Result.IsSuccessStatusCode)
                    {
                        WoodCraftViewModel WoodCraft = APIClient.GetElement<WoodCraftViewModel>(responseP);
                        int count = Convert.ToInt32(textBoxCount.Text);
                        textBoxSum.Text = (count * (int)WoodCraft.Price).ToString();
                    }
                    else
                    {
                        throw new Exception(APIClient.GetError(responseP));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void textBoxCount_TextChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void comboBoxWoodCraft_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxClient.SelectedItem == null)
            {
                MessageBox.Show("Выберите получателя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxWoodCraft.SelectedItem == null)
            {
                MessageBox.Show("Выберите мебель", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                 var response = APIClient.PostRequest("api/Main/CreateOrder", new OrdProductBindingModel
                {
                    CustomerID = Convert.ToInt32(comboBoxClient.SelectedValue),
                    WoodCraftsID = Convert.ToInt32(comboBoxWoodCraft.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text),
                    Sum = Convert.ToInt32(textBoxSum.Text)
                });
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
