using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.Intefaces;
using CarpenterWorkshopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Unity;
using Unity.Attributes;
namespace CarpenterWorkshopWPF
{
    /// <summary>
    /// Логика взаимодействия для FormWoodCraft.xaml
    /// </summary>
    public partial class FormWoodCraft : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IWoodCraftService service;

        private int? id;

        private List<BlankCraftViewModel> BlankCrafts;

        public FormWoodCraft(IWoodCraftService service)
        {
            InitializeComponent();
            Loaded += FormWoodCraft_Load;
            this.service = service;
        }

        private void FormWoodCraft_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    WoodCraftViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.WoodCraftsName;
                        textBoxPrice.Text = view.Price.ToString();
                        BlankCrafts = view.BlanksCrafts;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
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
                    dataGridViewCraft.ItemsSource = null;
                    dataGridViewCraft.ItemsSource = BlankCrafts;
                    dataGridViewCraft.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewCraft.Columns[1].Visibility = Visibility.Hidden;
                    dataGridViewCraft.Columns[2].Visibility = Visibility.Hidden;
                    dataGridViewCraft.Columns[3].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormBlankCraft>();
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
            if (dataGridViewCraft.SelectedItem != null)
            {
                var form = Container.Resolve<FormBlankCraft>();
                form.Model = BlankCrafts[dataGridViewCraft.SelectedIndex];
                if (form.ShowDialog() == true)
                {
                    BlankCrafts[dataGridViewCraft.SelectedIndex] = form.Model;
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewCraft.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        BlankCrafts.RemoveAt(dataGridViewCraft.SelectedIndex);
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
            try
            {
                List<BlankCraftBindingModel> productComponentBM = new List<BlankCraftBindingModel>();
                for (int i = 0; i < BlankCrafts.Count; ++i)
                {
                    productComponentBM.Add(new BlankCraftBindingModel
                    {
                        Id = BlankCrafts[i].Id,
                        WoodCraftsID = BlankCrafts[i].WoodCraftsID,
                        WoodBlanksID = BlankCrafts[i].WoodBlanksID,
                        Count = BlankCrafts[i].Count
                    });
                }
                if (id.HasValue)
                {
                    service.UpdElement(new WoodCraftBindingModel
                    {
                        Id = id.Value,
                        WoodCraftsName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        BlanksCrafts = productComponentBM
                    });
                }
                else
                {
                    service.AddElement(new WoodCraftBindingModel
                    {
                        WoodCraftsName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        BlanksCrafts = productComponentBM
                    });
                }
                MessageBox.Show("Сохранение прошло успешно", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
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
