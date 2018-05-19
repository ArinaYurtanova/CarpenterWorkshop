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
    /// Логика взаимодействия для FormTakeOrderInWork.xaml
    /// </summary>
    public partial class FormTakeOrderInWork : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IWorkerService serviceR;

        private readonly IMainService serviceG;

        private int? id;

        public FormTakeOrderInWork(IWorkerService serviceR, IMainService serviceG)
        {
            InitializeComponent();
            Loaded += FormTakeOrderInWork_Load;
            this.serviceR = serviceR;
            this.serviceG = serviceG;
        }

        private void FormTakeOrderInWork_Load(object sender, EventArgs e)
        {
            try
            {
                if (!id.HasValue)
                {
                    MessageBox.Show("Не указана заявка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
                List<WorkerVeiwModel> listR = serviceR.GetList();
                if (listR != null)
                {
                    comboBoxWorker.DisplayMemberPath = "WorkerFIO";
                    comboBoxWorker.SelectedValuePath = "Id";
                    comboBoxWorker.ItemsSource = listR;
                    comboBoxWorker.SelectedItem = null;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxWorker.SelectedItem == null)
            {
                MessageBox.Show("Выберите рабочего", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                serviceG.TakeOrderInWork(new OrdProductBindingModel
                {
                    Id = id.Value,
                    WorkerID = ((WorkerVeiwModel)comboBoxWorker.SelectedItem).Id,
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
