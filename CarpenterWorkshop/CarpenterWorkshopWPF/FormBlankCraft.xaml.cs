using CarpenterWorkshopService.ViewModels;
using CarpenterWorkshopView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CarpenterWorkshopWPF
{
    /// <summary>
    /// Логика взаимодействия для FormBlankCraft.xaml
    /// </summary>
    public partial class FormBlankCraft : Window
    {     
        public BlankCraftViewModel Model { set { model = value; } get { return model; } }

        private BlankCraftViewModel model;

        public FormBlankCraft()
        {
            InitializeComponent();
            Loaded += FormBlankCraft_Load;           
        }

        private void FormBlankCraft_Load(object sender, EventArgs e)
        {           
            try
            {              
                comboBoxWoodBlank.DisplayMemberPath = "WoodBlanksName";
                comboBoxWoodBlank.SelectedValuePath = "Id";
                comboBoxWoodBlank.ItemsSource = Task.Run(() => APIClient.GetRequestData<List<WoodBlankViewModel>>("api/WoodBlank/GetList")).Result;
                comboBoxWoodBlank.SelectedItem = null;
               
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (model != null)
            {
                comboBoxWoodBlank.IsEnabled = false;                                    
                comboBoxWoodBlank.SelectedValue = model.WoodBlanksID;
                textBoxCount.Text = model.Count.ToString();
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxWoodBlank.SelectedItem == null)
            {
                MessageBox.Show("Выберите заготовку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                if (model == null)
                {
                    model = new BlankCraftViewModel
                    {
                        WoodBlanksID = Convert.ToInt32(comboBoxWoodBlank.SelectedValue),
                        WoodBlanksName = comboBoxWoodBlank.Text,
                        Count = Convert.ToInt32(textBoxCount.Text)
                    };
                }
                else
                {
                    model.Count = Convert.ToInt32(textBoxCount.Text);
                }
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
