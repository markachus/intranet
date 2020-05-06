using IntranetCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace IntranetCore.Data
{
    public class IntranetDbContext : DbContext
    {
        public IntranetDbContext(DbContextOptions<IntranetDbContext> options) : base(options)
        {
        }

        public DbSet<Entrada> Entradas { get; set; }
        public DbSet<Etiqueta> Etiquetas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //EntradaEtiqueta -> Clave compuesta (EtquetaId, EntradaId)
            modelBuilder.Entity<EntradaEtiqueta>()
                .HasKey(x => new { x.EntradaId, x.EtiquetaId });

            //Etiqueta.Nombre -> Indice Unique
            modelBuilder.Entity<Etiqueta>()
                .HasIndex(t => t.Nombre)
                .IsUnique();


            var tagPrestige = new Etiqueta { 
                Id = Guid.Parse("766c0959-47bc-47a6-a4f2-6c0a5cea7162"), 
                Nombre = "Prestige", 
                FechaCreacion = DateTime.Now, 
                UsuarioCreacion = "Admin", 
                FechaUltimaModificacion = DateTime.Now,
                UsuarioModificacion = "Admin"
            };

            var tagCorreo = new Etiqueta { 
                Id = Guid.Parse("75e97d33-8d01-46cb-8a6c-3e1bc062a29a"), 
                Nombre = "Correo", 
                FechaCreacion = DateTime.Now, 
                UsuarioCreacion = "Admin", 
                FechaUltimaModificacion = DateTime.Now,
                UsuarioModificacion = "Admin"
            };

            var tagSeguridad = new Etiqueta { 
                Id = Guid.Parse("dab328aa-1f07-423f-a5bf-8ffa1cc1c0a8"), 
                Nombre = "Seguridad", 
                FechaCreacion = DateTime.Now, 
                UsuarioCreacion = "Admin", 
                FechaUltimaModificacion = DateTime.Now,
                UsuarioModificacion = "Admin"
            };

            var tagPersonal = new Etiqueta { 
                Id = Guid.Parse("98c8e0f6-34c2-46a4-b2a8-f1ee855e9493"), 
                Nombre = "Personal", 
                FechaCreacion = DateTime.Now, 
                UsuarioCreacion = "Admin", 
                FechaUltimaModificacion = DateTime.Now,
                UsuarioModificacion = "Admin"
            };
            var tagGeneral = new Etiqueta { 
                Id = Guid.Parse("9d49c27b-2c4d-4c0e-8e09-d9f51e2f2cdb"),
                Nombre = "General", 
                FechaCreacion = DateTime.Now, 
                UsuarioCreacion = "Admin", 
                FechaUltimaModificacion = DateTime.Now,
                UsuarioModificacion = "Admin"
            };

            var tagMobile = new Etiqueta { 
                Id = Guid.Parse("a182f6bc-c2f4-442e-830b-ae5c6810f517"),
                Nombre = "MWC", 
                FechaCreacion = DateTime.Now, 
                UsuarioCreacion = "Admin", 
                FechaUltimaModificacion = DateTime.Now,
                UsuarioModificacion = "Admin"
            };

            modelBuilder.Entity<Etiqueta>().HasData(tagPrestige, tagCorreo, tagSeguridad, tagPersonal, tagGeneral, tagMobile);

            var entryAdjuntarCorreos = new Entrada
            {
                Id = Guid.Parse("cf7dc085-62a8-492d-8f60-509bcc836d02"),
                Titulo = "Adjuntar correos a correos en Gmail",
                Contenido = "Adjuntar correos en Gmail es muy sencillo",
                UsuarioCreacion = "Admin",
                FechaCreacion = DateTime.Today,
                UsuarioModificacion = "Admin",
                FechaUltimaModificacion = DateTime.Now
            };

            modelBuilder.Entity<Entrada>().HasData(entryAdjuntarCorreos);

            modelBuilder.Entity<EntradaEtiqueta>().HasData(
                new EntradaEtiqueta { EntradaId = entryAdjuntarCorreos.Id, EtiquetaId = tagCorreo.Id });

            var entryERTE = new Entrada
            {
                Id = Guid.Parse("36232aa4-81a1-4962-b732-06d3204113ee"),
                Titulo = "Solictud de ERTE por Covid-19",
                Contenido = "La empresa ha solicitado un ERTE con fecha efectiva 1 de Abril y esta esperando respuesta.",
                UsuarioCreacion = "Admin",
                FechaCreacion = DateTime.Today,
                UsuarioModificacion = "Admin",
                FechaUltimaModificacion = DateTime.Now
            };

            modelBuilder.Entity<Entrada>().HasData(entryERTE);

            modelBuilder.Entity<EntradaEtiqueta>().HasData(
                new EntradaEtiqueta { EntradaId = entryERTE.Id, EtiquetaId = tagPersonal.Id },
                new EntradaEtiqueta { EntradaId = entryERTE.Id, EtiquetaId = tagGeneral.Id });

            var entryVacaciones2020 = new Entrada
            {
                Id = Guid.Parse("98f1191f-beeb-4e35-a1fe-c7f5bccec64f"),
                Titulo = "Vacaciones 2020",
                Contenido = "Disponéis de 22 días laborables. Teneis el calendario laboral 2020 aquí: [link]",
                UsuarioCreacion = "Admin",
                FechaCreacion = DateTime.Today,
                UsuarioModificacion = "Admin",
                FechaUltimaModificacion = DateTime.Now
            };


            modelBuilder.Entity<Entrada>().HasData(entryVacaciones2020);
            modelBuilder.Entity<EntradaEtiqueta>().HasData(
                new EntradaEtiqueta { EntradaId = entryVacaciones2020.Id, EtiquetaId = tagPersonal.Id },
                new EntradaEtiqueta { EntradaId = entryVacaciones2020.Id, EtiquetaId = tagGeneral.Id });

            var entryPrestige = new Entrada
            {
                Id = Guid.Parse("c7ddb8bb-4ee3-4443-b7ab-33aefc176c3b"),
                Titulo = "Dirección web de PMS de cada Hotel",
                Contenido = "A continuación os indicamos la dirección de acceso al PMS de cada Hotel",
                UsuarioCreacion = "Admin",
                FechaCreacion = DateTime.Today,
                UsuarioModificacion = "Admin",
                FechaUltimaModificacion = DateTime.Now
            };

            modelBuilder.Entity<Entrada>().HasData(entryPrestige);
            modelBuilder.Entity<EntradaEtiqueta>().HasData(
                new EntradaEtiqueta { EntradaId = entryPrestige.Id, EtiquetaId = tagPrestige.Id });


            var entryMWC = new Entrada
            {
                Id = Guid.Parse("011499a9-911d-4ef0-8929-6a055f28acb5"),
                Titulo = "Fechas MWC 2020",
                Contenido = "Las fecha para 2020 son las siguientes. Además el congreso se ha prolongado un año más debido a la cancelación en 2019",
                UsuarioCreacion = "Admin",
                FechaCreacion = DateTime.Today,
                UsuarioModificacion = "Admin",
                FechaUltimaModificacion = DateTime.Now
            };

            modelBuilder.Entity<Entrada>().HasData(entryMWC);
            modelBuilder.Entity<EntradaEtiqueta>().HasData(
                new EntradaEtiqueta { EntradaId = entryMWC.Id, EtiquetaId = tagPrestige.Id });

            base.OnModelCreating(modelBuilder);
        }
    }
}
