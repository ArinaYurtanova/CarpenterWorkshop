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
    /// Логика взаимодействия для FormWorkers.xaml
    /// </summary>
    public partial class FormWorkers : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IWorkerService service;

        public FormWorkers(IWorkerService service)
        {
            InitializeComponent();
            Loaded += FormWorkers_Load;
            this.service = service;
        }

        private void FormWorkers_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<WorkerVeiwModel> list = service.GetList();
                if (list != null)
                {
                    dataGridViewWorkers.ItemsSource = list;
                    dataGridViewWorkers.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewWorkers.Columns[1].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormWorker>();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewWorkers.SelectedItem != null)
            {
                var form = Container.Resolve<FormWorker>();
                form.Id = ((WorkerVeiwModel)dataGridViewWorkers.SelectedItem).Id;
                if (form.ShowDialog() == true)
                    LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewWorkers.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((WorkerVeiwModel)dataGridViewWorkers.SelectedItem).Id;
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