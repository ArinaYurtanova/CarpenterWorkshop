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
                List<WorkerVeiwModel> list = Task.Run(() => APIClient.GetRequestData<List<WorkerVeiwModel>>("api/Worker/GetList")).Result;
                if (list != null)
                {
                    comboBoxWorker.DisplayMemberPath = "WorkerFIO";
                    comboBoxWorker.SelectedValuePath = "Id";
                    comboBoxWorker.ItemsSource = list;
                    comboBoxWorker.SelectedItem = null;
                }
            
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxWorker.SelectedItem == null)
            {
                MessageBox.Show("Выберите рабочего", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                int WorkerID = Convert.ToInt32(comboBoxWorker.SelectedValue);
                Task task = Task.Run(() => APIClient.PostRequestData("api/Main/TakeOrderInWork", new OrdProductBindingModel
                {
                    Id = id.Value,
                    WorkerID = WorkerID
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Заявка передана в работу. Обновите список", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
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
