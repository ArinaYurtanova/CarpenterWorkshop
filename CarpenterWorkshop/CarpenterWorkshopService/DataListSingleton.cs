using CarpenterWorkshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshopService
{
    public class DataListSingleton
    {
        private static DataListSingleton instance;

        public List<Сustomer> Сustomers { get; set; }

        public List<WoodBlank> WoodBlanks { get; set; }

        public List<Worker> Workers { get; set; }

        public List<OrdProduct> OrdProducts { get; set; }

        public List<WoodCraft> WoodCrafts { get; set; }

        public List<BlankCraft> BlanksCrafts { get; set; }

        public List<Storage> Storages { get; set; }

        public List<StorageBlank> StorageBlanks { get; set; }

        private DataListSingleton()
        {
            Сustomers = new List<Сustomer>();
            WoodBlanks = new List<WoodBlank>();
            Workers = new List<Worker>();
            OrdProducts = new List<OrdProduct>();
            WoodCrafts = new List<WoodCraft>();
            BlanksCrafts = new List<BlankCraft>();
            Storages = new List<Storage>();
            StorageBlanks = new List<StorageBlank>();
        }

        public static DataListSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new DataListSingleton();
            }

            return instance;
        }
    }
}
