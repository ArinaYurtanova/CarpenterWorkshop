using System;
using System.Data.Entity;
using System.Windows;
using CarpenterWorkshopService.ImplementationsList;
using CarpenterWorkshopService.Intefaces;
using Unity;
using Unity.Lifetime;

namespace CarpenterWorkshopWPF
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        /*App()
            {
                InitializeComponent();
            }*/

        [STAThread]
        public static void Main()
        {
            var container = BuildUnityContainer();

            var application = new App();
            //application.InitializeComponent();
            application.Run(container.Resolve<FormMain>());
            //App app = new App();
            //app.Run(container.Resolve<FormMain>());
        }

        public static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();

            currentContainer.RegisterType<ICustomerService, CustomerServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IWoodBlankService, WoodBlankServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IWorkerService, WorkerServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IWoodCraftService, WoodCraftServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IStorageService, StorageServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMainService, MainServiceList>(new HierarchicalLifetimeManager());
            return currentContainer;
        }
    }
}