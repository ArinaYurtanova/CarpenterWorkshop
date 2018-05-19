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
    /// Логика взаимодействия для FormStorages.xaml
    /// </summary>
    public partial class FormStorages : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IStorageService service;

        public FormStorages(IStorageService service)
        {
            InitializeComponent();
            Loaded += FormStorages_Load;
            this.service = service;
        }

        private void FormStorages_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<StorageViewModel> list = service.GetList();
                if (list != null)
                {
                    dataGridViewStorages.ItemsSource = list;
                    dataGridViewStorages.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewStorages.Columns[1].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormStorage>();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewStorages.SelectedItem != null)
            {
                var form = Container.Resolve<FormStorage>();
                form.Id = ((StorageViewModel)dataGridViewStorages.SelectedItem).Id;
                if (form.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewStorages.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((StorageViewModel)dataGridViewStorages.SelectedItem).Id;
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
