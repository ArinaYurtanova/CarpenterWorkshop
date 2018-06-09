using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.ViewModels;
using CarpenterWorkshopView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                List<WoodBlankViewModel> listC = Task.Run(() => APIClient.GetRequestData<List<WoodBlankViewModel>>("api/WoodBlank/GetList")).Result;
                if (listC != null)
                {
                    comboBoxWoodBlank.DisplayMemberPath = "WoodBlanksName";
                    comboBoxWoodBlank.SelectedValuePath = "Id";
                    comboBoxWoodBlank.ItemsSource = listC;
                    comboBoxWoodBlank.SelectedItem = null;
                }
                List<StorageViewModel> listS = Task.Run(() => APIClient.GetRequestData<List<StorageViewModel>>("api/Storage/GetList")).Result;
                if (listS != null)
                {
                    comboBoxStorage.DisplayMemberPath = "StorageName";
                    comboBoxStorage.SelectedValuePath = "Id";
                    comboBoxStorage.ItemsSource = listS;
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
                int WoodBlanksID = Convert.ToInt32(comboBoxWoodBlank.SelectedValue);
                int StorageID = Convert.ToInt32(comboBoxStorage.SelectedValue);
                int count = Convert.ToInt32(textBoxCount.Text);
                Task task = Task.Run(() => APIClient.PostRequestData("api/Main/PutComponentOnStock", new StorageBlankBindingModel
                {
                    WoodBlanksID = WoodBlanksID,
                    StorageID = StorageID,
                    Count = count
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("База пополнен", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
                    TaskContinuationOptions.OnlyOnRanToCompletion);
                task.ContinueWith((prevTask) =>
                {
                    var ex = (Exception)prevTask.Exception;
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }, TaskContinuationOptions.OnlyOnFaulted);

                Close();
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
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
