using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CarpenterWorkshopWPF
{
    /// <summary>
    /// Логика взаимодействия для FormWorker.xaml
    /// </summary>
    public partial class FormWorker : Window
    {     

        public int Id { set { id = value; } }

        private int? id;

        public FormWorker()
        {
            InitializeComponent();
            Loaded += FormWorker_Load;
        }

        private void FormWorker_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = APIClient.GetRequest("api/Worker/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var Worker = APIClient.GetElement<WorkerVeiwModel>(response);
                        textBoxFullName.Text = Worker.WorkerFIO;
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
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = APIClient.PostRequest("api/Worker/UpdElement", new WorkerBindingModel
                    {
                        Id = id.Value,
                        WorkerFIO = textBoxFullName.Text
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Worker/AddElement", new WorkerBindingModel
                    {
                        WorkerFIO = textBoxFullName.Text
                    });
                }
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
