using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.Intefaces;
using CarpenterWorkshopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using Unity;
using Unity.Attributes;
namespace CarpenterWorkshopWPF
{
    /// <summary>
    /// Логика взаимодействия для FormPutOnStorage.xaml
    /// </summary>
    public partial class FormPutOnStorage : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IStorageService serviceB;

        private readonly IWoodBlankService serviceZ;

        private readonly IMainService serviceG;

        public FormPutOnStorage(IStorageService serviceB, IWoodBlankService serviceZ, IMainService serviceG)
        {
            InitializeComponent();
            Loaded += FormPutOnStorage_Load;
            this.serviceB = serviceB;
            this.serviceZ = serviceZ;
            this.serviceG = serviceG;
        }

        private void FormPutOnStorage_Load(object sender, EventArgs e)
        {
            try
            {
                List<WoodBlankViewModel> listZ = serviceZ.GetList();
                if (listZ != null)
                {
                    comboBoxWoodBlanks.DisplayMemberPath = "WoodBlanksName";
                    comboBoxWoodBlanks.SelectedValuePath = "Id";
                    comboBoxWoodBlanks.ItemsSource = listZ;
                    comboBoxWoodBlanks.SelectedItem = null;
                }
                List<StorageViewModel> listB = serviceB.GetList();
                if (listB != null)
                {
                    comboBoxStorage.DisplayMemberPath = "StorageName";
                    comboBoxStorage.SelectedValuePath = "Id";
                    comboBoxStorage.ItemsSource = listB;
                    comboBoxStorage.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (comboBoxStorage.SelectedItem == null)
            {
                MessageBox.Show("Выберите базу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                serviceG.PutComponentOnStock(new StorageBlankBindingModel
                {
                    WoodBlanksID = Convert.ToInt32(comboBoxWoodBlanks.SelectedValue),
                    StorageID = Convert.ToInt32(comboBoxStorage.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
                });
                MessageBox.Show("Сохранение прошло успешно", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
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
