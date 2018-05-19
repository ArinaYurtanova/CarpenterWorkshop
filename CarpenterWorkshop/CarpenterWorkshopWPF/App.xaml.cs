using System;
using System.Data.Entity;
using System.Windows;
using CarpenterWorkshopService;
using CarpenterWorkshopService.CarpenterWorkshopService;
using CarpenterWorkshopService.ImplementationsBD;
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


            currentContainer.RegisterType<DbContext, AbstractDbContext>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ICustomerService, CustomerServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IWoodBlankService, WoodBlankServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IWorkerService, WorkerServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IWoodCraftService, WoodCraftServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IStorageService, StorageServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMainService, MainServiceBD>(new HierarchicalLifetimeManager());
            return currentContainer;
        }
    }
}