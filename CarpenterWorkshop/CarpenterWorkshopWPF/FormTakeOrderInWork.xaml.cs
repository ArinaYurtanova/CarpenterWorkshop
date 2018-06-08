using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace CarpenterWorkshopWPF
{
    /// <summary>
    /// Логика взаимодействия для FormTakeOrderInWork.xaml
    /// </summary>
    public partial class FormTakeOrderInWork : Window
    {       
        public int Id { set { id = value; } }       

        private int? id;

        public FormTakeOrderInWork()
        {
            InitializeComponent();
            Loaded += FormTakeOrderInWork_Load;          
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
                var response = APIClient.GetRequest("api/Worker/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<WorkerVeiwModel> list = APIClient.GetElement<List<WorkerVeiwModel>>(response);
                    if (list != null)
                {
                    comboBoxWorker.DisplayMemberPath = "WorkerFIO";
                    comboBoxWorker.SelectedValuePath = "Id";
                    comboBoxWorker.ItemsSource = list;
                    comboBoxWorker.SelectedItem = null;

                }
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxWorker.SelectedItem == null)
            {
                MessageBox.Show("Выберите рабочего", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var response = APIClient.PostRequest("api/Main/TakeOrderInWork", new OrdProductBindingModel
                {
                    Id = id.Value,
                    WorkerID = ((WorkerVeiwModel)comboBoxWorker.SelectedItem).Id,
                });
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
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
