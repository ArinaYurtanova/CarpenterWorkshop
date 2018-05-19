using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CarpenterWorkshopService.Intefaces;
using CarpenterWorkshopService.ViewModels;
using Unity;
using Unity.Attributes;
namespace CarpenterWorkshopWPF
{
    /// <summary>
    /// Логика взаимодействия для FormWoodCrafts.xaml
    /// </summary>
    public partial class FormWoodCrafts : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IWoodCraftService service;

        public FormWoodCrafts(IWoodCraftService service)
        {
            InitializeComponent();
            Loaded += FormWoodCrafts_Load;
            this.service = service;
        }

        private void FormWoodCrafts_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<WoodCraftViewModel> list = service.GetList();
                if (list != null)
                {
                    dataGridViewCrafts.ItemsSource = list;
                    dataGridViewCrafts.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewCrafts.Columns[1].Width = DataGridLength.Auto;
                    dataGridViewCrafts.Columns[3].Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormWoodCraft>();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewCrafts.SelectedItem != null)
            {
                var form = Container.Resolve<FormWoodCraft>();
                form.Id = ((WoodCraftViewModel)dataGridViewCrafts.SelectedItem).Id;
                if (form.ShowDialog() == true)
                    LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewCrafts.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {

                    int id = ((WoodCraftViewModel)dataGridViewCrafts.SelectedItem).Id;
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
