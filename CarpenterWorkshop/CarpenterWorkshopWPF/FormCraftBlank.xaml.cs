using System;
using System.Collections.Generic;
using System.Windows;
using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.Intefaces;
using CarpenterWorkshopService.ViewModels;
using Unity;
using Unity.Attributes;
namespace CarpenterWorkshopWPF
{
    /// <summary>
    /// Логика взаимодействия для FormBlankCraft.xaml
    /// </summary>
    public partial class FormBlankCraft : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public BlankCraftViewModel Model { set { model = value; } get { return model; } }

        private readonly IWoodBlankService service;

        private BlankCraftViewModel model;

        public FormBlankCraft(IWoodBlankService service)
        {
            InitializeComponent();
            Loaded += FormBlankCraft_Load;
            this.service = service;
        }

        private void FormBlankCraft_Load(object sender, EventArgs e)
        {
            List<WoodBlankViewModel> list = service.GetList();
            try
            {
                if (list != null)
                {
                    comboBoxWoodBlanks.DisplayMemberPath = "WoodBlanksName";
                    comboBoxWoodBlanks.SelectedValuePath = "Id";
                    comboBoxWoodBlanks.ItemsSource = list;
                    comboBoxWoodBlanks.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (model != null)
            {
                comboBoxWoodBlanks.IsEnabled = false;
                foreach (WoodBlankViewModel item in list)
                {
                    if (item.WoodBlanksName == model.WoodBlanksName)
                    {
                        comboBoxWoodBlanks.SelectedItem = item;
                    }
                }
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
            if (comboBoxWoodBlanks.SelectedItem == null)
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
                        WoodBlanksID = Convert.ToInt32(comboBoxWoodBlanks.SelectedValue),
                        WoodBlanksName = comboBoxWoodBlanks.Text,
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
