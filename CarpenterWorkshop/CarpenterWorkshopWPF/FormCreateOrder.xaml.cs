using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.Intefaces;
using CarpenterWorkshopService.ViewModels;
using Unity;
using Unity.Attributes;
namespace CarpenterWorkshopWPF
{
    /// <summary>
    /// Логика взаимодействия для FormCreateOrder.xaml
    /// </summary>
    public partial class FormCreateOrder : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly ICustomerService serviceP;

        private readonly IWoodCraftService serviceM;

        private readonly IMainService serviceG;


        public FormCreateOrder(ICustomerService serviceP, IWoodCraftService serviceM, IMainService serviceG)
        {
            InitializeComponent();
            Loaded += FormCreateOrder_Load;
            comboBoxCraft.SelectionChanged += comboBoxCraft_SelectedIndexChanged;

            comboBoxCraft.SelectionChanged += new SelectionChangedEventHandler(comboBoxCraft_SelectedIndexChanged);
            this.serviceP = serviceP;
            this.serviceM = serviceM;
            this.serviceG = serviceG;
        }

        private void FormCreateOrder_Load(object sender, EventArgs e)
        {
            try
            {
                List<CustomerViewModel> listP = serviceP.GetList();
                if (listP != null)
                {
                    comboBoxClient.DisplayMemberPath = "CustomerFIO";
                    comboBoxClient.SelectedValuePath = "Id";
                    comboBoxClient.ItemsSource = listP;
                    comboBoxCraft.SelectedItem = null;
                }
                List<WoodCraftViewModel> listM = serviceM.GetList();
                if (listM != null)
                {
                    comboBoxCraft.DisplayMemberPath = "WoodCraftsName";
                    comboBoxCraft.SelectedValuePath = "Id";
                    comboBoxCraft.ItemsSource = listM;
                    comboBoxCraft.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalcSum()
        {
            if (comboBoxCraft.SelectedItem != null && !string.IsNullOrEmpty(textBoxCount.Text))
            {
                try
                {
                    int id = ((WoodCraftViewModel)comboBoxCraft.SelectedItem).Id;
                    WoodCraftViewModel product = serviceM.GetElement(id);
                    int count = Convert.ToInt32(textBoxCount.Text);
                    textBoxSum.Text = (count * product.Price).ToString();
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

        private void comboBoxCraft_SelectedIndexChanged(object sender, EventArgs e)
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
            if (comboBoxCraft.SelectedItem == null)
            {
                MessageBox.Show("Выберите изделие", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                serviceG.CreateOrder(new OrdProductBindingModel
                {
                    CustomerID = ((CustomerViewModel)comboBoxClient.SelectedItem).Id,
                    WoodCraftsID = ((WoodCraftViewModel)comboBoxCraft.SelectedItem).Id,
                    Count = Convert.ToInt32(textBoxCount.Text),
                    Sum = Convert.ToInt32(textBoxSum.Text)
                });
                MessageBox.Show("Сохранение прошло успешно", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
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
