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
using System.Windows.Controls;

namespace CarpenterWorkshopWPF
{
    /// <summary>
    /// Логика взаимодействия для FormWoodCraft.xaml
    /// </summary>
    public partial class FormWoodCraft : Window
    {       
        public int Id { set { id = value; } }

        private int? id;

        private List<BlankCraftViewModel> BlankCrafts;

        public FormWoodCraft()
        {
            InitializeComponent();
            Loaded += FormWoodCraft_Load;          
        }

        private void FormWoodCraft_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var WoodCraft = Task.Run(() => APIClient.GetRequestData<WoodCraftViewModel>("api/WoodCraft/Get/" + id.Value)).Result;
                    textBoxName.Text = WoodCraft.WoodCraftsName;
                        textBoxPrice.Text = WoodCraft.Price.ToString();
                        BlankCrafts = WoodCraft.BlanksCrafts;
                        LoadData();                    
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
            else
                BlankCrafts = new List<BlankCraftViewModel>();
        }

        private void LoadData()
        {
            try
            {
                if (BlankCrafts != null)
                {
                    dataGridViewWoodCraft.ItemsSource = null;
                    dataGridViewWoodCraft.ItemsSource = BlankCrafts;
                    dataGridViewWoodCraft.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewWoodCraft.Columns[1].Visibility = Visibility.Hidden;
                    dataGridViewWoodCraft.Columns[2].Visibility = Visibility.Hidden;
                    dataGridViewWoodCraft.Columns[3].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormBlankCraft();
            if (form.ShowDialog() == true)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                        form.Model.WoodCraftsID = id.Value;
                    BlankCrafts.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewWoodCraft.SelectedItem != null)
            {
                var form = new FormBlankCraft();
                form.Model = BlankCrafts[dataGridViewWoodCraft.SelectedIndex];
                if (form.ShowDialog() == true)
                {
                    BlankCrafts[dataGridViewWoodCraft.SelectedIndex] = form.Model;
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewWoodCraft.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        BlankCrafts.RemoveAt(dataGridViewWoodCraft.SelectedIndex);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (BlankCrafts == null || BlankCrafts.Count == 0)
            {
                MessageBox.Show("Заполните заготовки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
