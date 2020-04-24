using Intranet.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.Data
{
    public class IntranetDbContext : DbContext
    {
        public IntranetDbContext() : base()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<IntranetDbContext,Migrations.Configuration>());
        }

        public DbSet<Entrada> Entradas { get; set; }
        public DbSet<Etiqueta> Etiquetas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Relación Many-to-Many entre Entrada y Etiquetas sin navegación de retorno
            modelBuilder.Entity<Entrada>().
                HasMany(e => e.Etiquetas).
                WithMany();
        }
    }
}
