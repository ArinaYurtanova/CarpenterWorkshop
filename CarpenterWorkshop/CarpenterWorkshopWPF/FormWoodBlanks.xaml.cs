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
    /// Логика взаимодействия для FormWoodBlanks.xaml
    /// </summary>
    public partial class FormWoodBlanks : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IWoodBlankService service;

        public FormWoodBlanks(IWoodBlankService service)
        {
            InitializeComponent();
            Loaded += FormWoodBlanks_Load;
            this.service = service;
        }

        private void FormWoodBlanks_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<WoodBlankViewModel> list = service.GetList();
                if (list != null)
                {
                    dataGridViewZagotovkis.ItemsSource = list;
                    dataGridViewZagotovkis.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewZagotovkis.Columns[1].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormWoodBlank>();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewZagotovkis.SelectedItem != null)
            {
                var form = Container.Resolve<FormWoodBlank>();
                form.Id = ((WoodBlankViewModel)dataGridViewZagotovkis.SelectedItem).Id;
                if (form.ShowDialog() == true)
                    LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewZagotovkis.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((WoodBlankViewModel)dataGridViewZagotovkis.SelectedItem).Id;
                    try
                    {
                        service.DelElement(id);
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
