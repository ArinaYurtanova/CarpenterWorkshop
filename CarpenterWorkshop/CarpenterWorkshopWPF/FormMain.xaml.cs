using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.Intefaces;
using CarpenterWorkshopService.ViewModels;
using CarpenterWorkshopWpf;
using Microsoft.Win32;
using Unity;
using Unity.Attributes;
namespace CarpenterWorkshopWPF
{
    /// <summary>
    /// Логика взаимодействия для FormMain.xaml
    /// </summary>
    public partial class FormMain : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IMainService service;

        private readonly IReportService reportService;

        public FormMain(IMainService service, IReportService reportService)
        {
            InitializeComponent();
            this.service = service;
            this.reportService = reportService;
        }

        private void LoadData()
        {
            try
            {
                List<OrdProductViewModel> list = service.GetList();
                if (list != null)
                {
                    dataGridViewMain.ItemsSource = list;
                    dataGridViewMain.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewMain.Columns[1].Visibility = Visibility.Hidden;
                    dataGridViewMain.Columns[3].Visibility = Visibility.Hidden;
                    dataGridViewMain.Columns[5].Visibility = Visibility.Hidden;
                    dataGridViewMain.Columns[1].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void получателиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormCustomers>();
            form.ShowDialog();
        }

        private void заготовкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormWoodBlanks>();
            form.ShowDialog();
        }

        private void мебельToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormWoodCrafts>();
            form.ShowDialog();
        }

        private void базыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormStorages>();
            form.ShowDialog();
        }

        private void рабочиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormWorkers>();
            form.ShowDialog();
        }

        private void пополнитьБазуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormPutOnStorage>();
            form.ShowDialog();
        }

        private void buttonCreateZakaz_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormCreateOrder>();
            form.ShowDialog();
            LoadData();
        }

        private void buttonTakeZakazInWork_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedItem != null)
            {
                var form = Container.Resolve<FormTakeOrderInWork>();
                form.Id = ((OrdProductViewModel)dataGridViewMain.SelectedItem).Id;
                form.ShowDialog();
                LoadData();
            }
        }

        private void buttonZakazReady_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedItem != null)
            {
                int id = ((OrdProductViewModel)dataGridViewMain.SelectedItem).Id;
                try
                {
                    service.FinishOrder(id);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonPayZakaz_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedItem != null)
            {
                int id = ((OrdProductViewModel)dataGridViewMain.SelectedItem).Id;
                try
                {
                    service.PayOrder(id);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void прайсМебелиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "doc|*.doc|docx|*.docx"
            };

            if (sfd.ShowDialog() == true)
            {

                try
                {

                    reportService.SaveProductPrice(new ReportBindingModel
                    {
                        FileName = sfd.FileName
                    });
                    System.Windows.MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void загруженностьБазToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "xls|*.xls|xlsx|*.xlsx"
            };
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    reportService.SaveStoragesLoad(new ReportBindingModel
                    {
                        FileName = sfd.FileName
                    });
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void заказыПолучателейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormCustomerOrder>();
            form.ShowDialog();
        }
    }
}
