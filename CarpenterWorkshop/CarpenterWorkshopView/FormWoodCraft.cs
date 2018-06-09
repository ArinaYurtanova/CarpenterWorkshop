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

        private List<BlankCraftViewModel> BlankCrafts;

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
                    var product = Task.Run(() => APIClient.GetRequestData<WoodCraftViewModel>("api/WoodCraft/Get/" + id.Value)).Result;
                    textBoxName.Text = product.WoodCraftsName;
                    textBoxPrice.Text = product.Price.ToString();
                    BlankCrafts = product.BlanksCrafts;
                    LoadData();
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                BlankCrafts = new List<BlankCraftViewModel>();
            }
        }

        private void LoadData()
        {
            try
            {
                if (BlankCrafts != null)
                {
                    dataGridView.DataSource = null;
                    dataGridView.DataSource = BlankCrafts;
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
                    BlankCrafts.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                var form = new FormBlankCraft();
                form.Model = BlankCrafts[dataGridView.SelectedRows[0].Cells[0].RowIndex];
                if (form.ShowDialog() == DialogResult.OK)
                {
                    BlankCrafts[dataGridView.SelectedRows[0].Cells[0].RowIndex] = form.Model;
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
                        BlankCrafts.RemoveAt(dataGridView.SelectedRows[0].Cells[0].RowIndex);
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
            if (BlankCrafts == null || BlankCrafts.Count == 0)
            {
                MessageBox.Show("Заполните компоненты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            List<BlankCraftBindingModel> BlankCraftBM = new List<BlankCraftBindingModel>();
            for (int i = 0; i < BlankCrafts.Count; ++i)
            {
                BlankCraftBM.Add(new BlankCraftBindingModel
                {
                    Id = BlankCrafts[i].Id,
                    WoodCraftsID = BlankCrafts[i].WoodCraftsID,
                    WoodBlanksID = BlankCrafts[i].WoodBlanksID,
                    Count = BlankCrafts[i].Count
                });
            }
            string name = textBoxName.Text;
            int price = Convert.ToInt32(textBoxPrice.Text);
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APIClient.PostRequestData("api/WoodCraft/UpdElement", new WoodCraftBindingModel
                {
                    Id = id.Value,
                    WoodCraftsName = name,
                    Price = price,
                    BlanksCrafts = BlankCraftBM
                }));
            }
            else
            {
                task = Task.Run(() => APIClient.PostRequestData("api/WoodCraft/AddElement", new WoodCraftBindingModel
                {
                    WoodCraftsName = name,
                    Price = price,
                    BlanksCrafts = BlankCraftBM
                }));
            }

            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information),
                TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith((prevTask) =>
            {
                var ex = (Exception)prevTask.Exception;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }, TaskContinuationOptions.OnlyOnFaulted);

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}