using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


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
                    var response = APIClient.GetRequest("api/WoodBlank/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var WoodBlank = APIClient.GetElement<WoodBlankViewModel>(response);
                        textBoxName.Text = WoodBlank.WoodBlanksName;
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
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = APIClient.PostRequest("api/WoodBlank/UpdElement", new WoodBlanksBindingModel
                    {
                        Id = id.Value,
                        WoodBlanksName = textBoxName.Text
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/WoodBlank/AddElement", new WoodBlanksBindingModel
                    {
                        WoodBlanksName = textBoxName.Text
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
