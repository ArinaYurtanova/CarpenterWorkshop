using CarpenterWorkshop;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshopService
{
    namespace CarpenterWorkshopService
    {

        [Table("AbstractDatabaseWPF")]
        public class AbstractDbContext : DbContext
        {
            public AbstractDbContext()
            {
                //настройки конфигурации для entity
                Configuration.ProxyCreationEnabled = false;
                Configuration.LazyLoadingEnabled = false;
                var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
            }

            public virtual DbSet<Сustomer> Customers { get; set; }

            public virtual DbSet<WoodBlank> WoodBlanks { get; set; }

            public virtual DbSet<Worker> Workers { get; set; }

            public virtual DbSet<OrdProduct> OrdProducts { get; set; }

            public virtual DbSet<WoodCraft> WoodCrafts { get; set; }

            public virtual DbSet<BlankCraft> BlankCrafts { get; set; }

            public virtual DbSet<Storage> Storages { get; set; }

            public virtual DbSet<StorageBlank> StorageBlanks { get; set; }
        }
    }
}
