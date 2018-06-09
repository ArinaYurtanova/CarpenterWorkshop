using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.ViewModels;
using CarpenterWorkshopView;
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
    /// Логика взаимодействия для FormWoodBlank.xaml
    /// </summary>
    public partial class FormWoodBlank : Window
    {        

        public int Id { set { id = value; } }

        private int? id;

        public FormWoodBlank()
        {
            InitializeComponent();
            Loaded += FormWoodBlank_Load;          
        }

        private void FormWoodBlank_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var WoodBlank = Task.Run(() => APIClient.GetRequestData<WoodBlankViewModel>("api/WoodBlank/Get/" + id.Value)).Result;
                    textBoxName.Text = WoodBlank.WoodBlanksName;                   
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
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string name = textBoxName.Text;
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APIClient.PostRequestData("api/WoodBlank/UpdElement", new WoodBlanksBindingModel
                {
                    Id = id.Value,
                    WoodBlanksName = name
                }));
            }
            else
            {
                task = Task.Run(() => APIClient.PostRequestData("api/WoodBlank/AddElement", new WoodBlanksBindingModel
                {
                    WoodBlanksName = name
                }));
            }

            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
