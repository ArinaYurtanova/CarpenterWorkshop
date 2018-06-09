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
                List<CustomerViewModel> listC = Task.Run(() => APIClient.GetRequestData<List<CustomerViewModel>>("api/Customer/GetList")).Result;
                if (listC != null)
                    {
                        comboBoxClient.DisplayMemberPath = "CustomerFIO";
                        comboBoxClient.SelectedValuePath = "Id";
                        comboBoxClient.ItemsSource = listC;
                        comboBoxWoodCraft.SelectedItem = null;
                    }

                List<WoodCraftViewModel> listP = Task.Run(() => APIClient.GetRequestData<List<WoodCraftViewModel>>("api/WoodCraft/GetList")).Result;
                if (listP != null)
                    {
                        comboBoxWoodCraft.DisplayMemberPath = "WoodCraftsName";
                        comboBoxWoodCraft.SelectedValuePath = "Id";
                        comboBoxWoodCraft.ItemsSource = listP;
                        comboBoxWoodCraft.SelectedItem = null;
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

        private void CalcSum()
        {
            if (comboBoxWoodCraft.SelectedItem != null && !string.IsNullOrEmpty(textBoxCount.Text))
            {
                try
                {
                    int id = ((WoodCraftViewModel)comboBoxWoodCraft.SelectedItem).Id;
                    WoodCraftViewModel product = Task.Run(() => APIClient.GetRequestData<WoodCraftViewModel>("api/WoodCraft/Get/" + id)).Result;
                    int count = Convert.ToInt32(textBoxCount.Text);
                    textBoxSum.Text = (count * (int)product.Price).ToString();                                      
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
            int CustomerID = Convert.ToInt32(comboBoxClient.SelectedValue);
            int WoodCraftsID = Convert.ToInt32(comboBoxWoodCraft.SelectedValue);
            int count = Convert.ToInt32(textBoxCount.Text);
            int sum = Convert.ToInt32(textBoxSum.Text);
            Task task = Task.Run(() => APIClient.PostRequestData("api/Main/CreateOrder", new OrdProductBindingModel
            {
                CustomerID = CustomerID,
                WoodCraftsID = WoodCraftsID,
                Count = count,
                Sum = sum
            }));

            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
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

            Close();
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
