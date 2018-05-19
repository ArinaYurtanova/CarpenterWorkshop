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
    /// Логика взаимодействия для FormPutOnBaza.xaml
    /// </summary>
    public partial class FormPutOnBaza : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IStorageService serviceB;

        private readonly IWoodBlankService serviceZ;

        private readonly IMainService serviceG;

        public FormPutOnBaza(IStorageService serviceB, IWoodBlankService serviceZ, IMainService serviceG)
        {
            InitializeComponent();
            Loaded += FormPutOnBaza_Load;
            this.serviceB = serviceB;
            this.serviceZ = serviceZ;
            this.serviceG = serviceG;
        }

        private void FormPutOnBaza_Load(object sender, EventArgs e)
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
                    comboBoxBaza.DisplayMemberPath = "StorageName";
                    comboBoxBaza.SelectedValuePath = "Id";
                    comboBoxBaza.ItemsSource = listB;
                    comboBoxBaza.SelectedItem = null;
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
            if (comboBoxBaza.SelectedItem == null)
            {
                MessageBox.Show("Выберите базу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                serviceG.PutComponentOnStock(new StorageBlankBindingModel
                {
                    WoodBlanksID = Convert.ToInt32(comboBoxWoodBlanks.SelectedValue),
                    StorageID = Convert.ToInt32(comboBoxBaza.SelectedValue),
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
