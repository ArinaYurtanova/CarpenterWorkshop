using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.Intefaces;
using CarpenterWorkshopService.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using Unity;
using Unity.Attributes;
namespace CarpenterWorkshopWPF
{
    /// <summary>
    /// Логика взаимодействия для FormStorage.xaml
    /// </summary>
    public partial class FormStorage : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IStorageService service;

        private int? id;

        public FormStorage(IStorageService service)
        {
            InitializeComponent();
            Loaded += FormStorage_Load;
            this.service = service;
        }

        private void FormStorage_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    StorageViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.StorageName;
                        dataGridViewStorage.ItemsSource = view.StorageBlanks;
                        dataGridViewStorage.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewStorage.Columns[1].Visibility = Visibility.Hidden;
                        dataGridViewStorage.Columns[2].Visibility = Visibility.Hidden;
                        dataGridViewStorage.Columns[3].Width = DataGridLength.Auto;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                if (id.HasValue)
                {
                    service.UpdElement(new StorageBindingModel
                    {
                        Id = id.Value,
                        StorageName = textBoxName.Text
                    });
                }
                else
                {
                    service.AddElement(new StorageBindingModel
                    {
                        StorageName = textBoxName.Text
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
