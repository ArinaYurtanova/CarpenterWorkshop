using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.Intefaces;
using CarpenterWorkshopService.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;
using Unity.Attributes;

namespace CarpenterWorkshopView
{
    public partial class FormWoodCraft : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        private List<BlankCraftViewModel> blankCrafts;

        public FormWoodCraft()
        {
            InitializeComponent();
        }

        private void FormWoodCraft_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = APIClient.GetRequest("api/WoodCraft/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var product = APIClient.GetElement<WoodCraftViewModel>(response);
                        textBoxName.Text = product.WoodCraftsName;
                        textBoxPrice.Text = product.Price.ToString();
                        blankCrafts = product.BlanksCrafts;
                        LoadData();
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
            else
            {
                blankCrafts = new List<BlankCraftViewModel>();
            }
        }

        private void LoadData()
        {
            try
            {
                if (blankCrafts != null)
                {
                    dataGridView.DataSource = null;
                    dataGridView.DataSource = blankCrafts;
                    dataGridView.Columns[0].Visible = false;
                    dataGridView.Columns[1].Visible = false;
                    dataGridView.Columns[2].Visible = false;
                    dataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormBlankCraft();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                    {
                        form.Model.WoodCraftsID = id.Value;
                    }
                    blankCrafts.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                var form = new FormBlankCraft();
                form.Model = blankCrafts[dataGridView.SelectedRows[0].Cells[0].RowIndex];
                if (form.ShowDialog() == DialogResult.OK)
                {
                    blankCrafts[dataGridView.SelectedRows[0].Cells[0].RowIndex] = form.Model;
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        blankCrafts.RemoveAt(dataGridView.SelectedRows[0].Cells[0].RowIndex);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (blankCrafts == null || blankCrafts.Count == 0)
            {
                MessageBox.Show("Заполните компоненты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                List<BlankCraftBindingModel> blankCraftBM = new List<BlankCraftBindingModel>();
                for (int i = 0; i < blankCrafts.Count; ++i)
                {
                    blankCraftBM.Add(new BlankCraftBindingModel
                    {
                        Id = blankCrafts[i].Id,
                        WoodCraftsID = blankCrafts[i].WoodCraftsID,
                        WoodBlanksID = blankCrafts[i].WoodBlanksID,
                        Count = blankCrafts[i].Count
                    });
                }
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = APIClient.PostRequest("api/WoodCraft/UpdElement", new WoodCraftBindingModel
                    {
                        Id = id.Value,
                        WoodCraftsName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        BlanksCrafts = blankCraftBM
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/WoodCraft/AddElement", new WoodCraftBindingModel
                    {
                        WoodCraftsName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        BlanksCrafts = blankCraftBM
                    });
                }
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