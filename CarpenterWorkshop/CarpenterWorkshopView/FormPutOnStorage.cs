using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.Intefaces;
using CarpenterWorkshopService.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;
using Unity.Attributes;

namespace CarpenterWorkshopView
{
    public partial class FormPutOnStorage : Form
    {
        public FormPutOnStorage()
        {
            InitializeComponent();
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
                        comboBoxComponent.DisplayMember = "WoodBlanksName";
                        comboBoxComponent.ValueMember = "ID";
                        comboBoxComponent.DataSource = list;
                        comboBoxComponent.SelectedItem = null;
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
                        comboBoxStock.DisplayMember = "StorageName";
                        comboBoxStock.ValueMember = "ID";
                        comboBoxStock.DataSource = list;
                        comboBoxStock.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseC));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxComponent.SelectedValue == null)
            {
                MessageBox.Show("Выберите компонент", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxStock.SelectedValue == null)
            {
                MessageBox.Show("Выберите склад", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                var response = APIClient.PostRequest("api/Main/PutComponentOnStock", new StorageBlankBindingModel
                {
                    WoodBlanksID = Convert.ToInt32(comboBoxComponent.SelectedValue),
                    StorageID = Convert.ToInt32(comboBoxStock.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
                });
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}