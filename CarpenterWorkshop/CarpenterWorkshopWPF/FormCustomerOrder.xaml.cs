using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.Intefaces;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using System;
using System.Windows;
using Unity;
using Unity.Attributes;

namespace CarpenterWorkshopWpf
{
    /// <summary>
    /// Логика взаимодействия для FormCustomerOrder.xaml
    /// </summary>
    public partial class FormCustomerOrder : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IReportService service;

        public FormCustomerOrder(IReportService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void buttonMake_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.SelectedDate >= dateTimePickerTo.SelectedDate)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                reportViewer.LocalReport.ReportEmbeddedResource = "CarpenterWorkshopWpf.ReportCustomersOrders.rdlc";
                ReportParameter parameter = new ReportParameter("ReportParameterPeriod",
                                            "c " + dateTimePickerFrom.SelectedDate.ToString() +
                                            " по " + dateTimePickerTo.SelectedDate.ToString());
                reportViewer.LocalReport.SetParameters(parameter);


                var dataSource = service.GetCustomerOrders(new ReportBindingModel
                {
                    DateFrom = dateTimePickerFrom.SelectedDate,
                    DateTo = dateTimePickerTo.SelectedDate
                });
                ReportDataSource source = new ReportDataSource("DataSetOrders", dataSource);
                reportViewer.LocalReport.DataSources.Add(source);




                reportViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonToPdf_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.SelectedDate >= dateTimePickerTo.SelectedDate)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "pdf|*.pdf"
            };
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    service.SaveCustomerOrders(new ReportBindingModel
                    {
                        FileName = sfd.FileName,
                        DateFrom = dateTimePickerFrom.SelectedDate,
                        DateTo = dateTimePickerTo.SelectedDate
                    });
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        
    }
}