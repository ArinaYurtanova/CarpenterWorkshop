using System;
using System.Windows;
using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.Intefaces;
using CarpenterWorkshopService.ViewModels;
using Unity;
using Unity.Attributes;
namespace CarpenterWorkshopWPF
{
    /// <summary>
    /// Логика взаимодействия для FormWorker.xaml
    /// </summary>
    public partial class FormWorker : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IWorkerService service;

        private int? id;

        public FormWorker(IWorkerService service)
        {
            InitializeComponent();
            Loaded += FormWorker_Load;
            this.service = service;
        }

        private void FormWorker_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    WorkerVeiwModel view = service.GetElement(id.Value);
                    if (view != null)
                        textBoxFullName.Text = view.WorkerFIO;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxFullName.Text))
            {
                MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                if (id.HasValue)
                {
                    service.UpdElement(new WorkerBindingModel
                    {
                        Id = id.Value,
                        WorkerFIO = textBoxFullName.Text
                    });
                }
                else
                {
                    service.AddElement(new WorkerBindingModel
                    {
                        WorkerFIO = textBoxFullName.Text
                    });
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
