
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopView;
using CarpenterWorkshopService.ViewModels;

namespace CarpenterWorkshopWPF
{
    /// <summary>
    /// Логика взаимодействия для FormCustomerOrders.xaml
    /// </summary>
    public partial class FormCustomerOrders : Window
    {
       
        public FormCustomerOrders()
        {
            InitializeComponent();
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
                reportViewer.LocalReport.ReportEmbeddedResource = "CarpenterWorkshopWPF.ReportCustomerZayavkas.rdlc";
                ReportParameter parameter = new ReportParameter("ReportParameterPeriod",
                                            "c " + Convert.ToDateTime(dateTimePickerFrom.SelectedDate).ToString("dd-MM") +
                                            " по " + Convert.ToDateTime(dateTimePickerTo.SelectedDate).ToString("dd-MM"));
                reportViewer.LocalReport.SetParameters(parameter);


                var dataSource = Task.Run(() => APIClient.PostRequestData<ReportBindingModel, List<CustomerOrdersModel>>("api/Report/GetCustomerZayavkas",
                     new ReportBindingModel
                     {
                         DateFrom = dateTimePickerFrom.SelectedDate,
                         DateTo = dateTimePickerTo.SelectedDate
                     })).Result;
               
                    ReportDataSource source = new ReportDataSource("DataSetZayavkas", dataSource);
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
                string fileName = sfd.FileName;
                Task task = Task.Run(() => APIClient.PostRequestData("api/Report/SaveCustomerOrder", new ReportBindingModel
                {
                        FileName = sfd.FileName,
                        DateFrom = dateTimePickerFrom.SelectedDate,
                        DateTo = dateTimePickerTo.SelectedDate
                    }));
                task.ContinueWith((prevTask) => MessageBox.Show("Список заявок сохранен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
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
            }
        }
    }
}