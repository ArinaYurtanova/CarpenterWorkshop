using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.ViewModels;
using System;
using System.Collections.Generic;

using System.Windows;

namespace CarpenterWorkshopWPF
{
    /// <summary>
    /// Логика взаимодействия для FormPutOnStorage.xaml
    /// </summary>
    public partial class FormPutOnStorage : Window
    {      
        public FormPutOnStorage()
        {
            InitializeComponent();
            Loaded += FormPutOnStorage_Load;        
        }

        private void FormPutOnStorage_Load(object sender, EventArgs e)
        {
            try
            {
                var responseC = APIClient.GetRequest("api/WoodBlank/GetList");
                if (responseC.Result.IsSuccessStatusCode)
                {
                    List<WoodBlankViewModel> list = APIClient.GetElement<List<WoodBlankViewModel>>(responseC);
                    if (list != null)
                {
                    comboBoxWoodBlank.DisplayMemberPath = "WoodBlanksName";
                    comboBoxWoodBlank.SelectedValuePath = "Id";
                    comboBoxWoodBlank.ItemsSource = list;
                    comboBoxWoodBlank.SelectedItem = null;
                }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseC));
                }
                var responseS = APIClient.GetRequest("api/Storage/GetList");
                if (responseS.Result.IsSuccessStatusCode)
                {
                    List<StorageViewModel> list = APIClient.GetElement<List<StorageViewModel>>(responseS);
                    if (list != null)
                {
                    comboBoxStorage.DisplayMemberPath = "StorageName";
                    comboBoxStorage.SelectedValuePath = "Id";
                    comboBoxStorage.ItemsSource = list;
                    comboBoxStorage.SelectedItem = null;
                }
            }
                else
                {
                    throw new Exception(APIClient.GetError(responseC));
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
            if (comboBoxWoodBlank.SelectedItem == null)
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
                var response = APIClient.PostRequest("api/Main/PutComponentOnStock", new StorageBlankBindingModel
                {
                    WoodBlanksID = Convert.ToInt32(comboBoxWoodBlank.SelectedValue),
                    StorageID = Convert.ToInt32(comboBoxStorage.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
                });
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult =true;
                    Close();
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
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
