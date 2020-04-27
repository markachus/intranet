namespace Intranet.Data.Migrations
{
    using Intranet.Data.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Intranet.Data.IntranetDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Intranet.Data.IntranetDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            try { 
            var tagPrestige = new Etiqueta { Nombre = "Prestige", FechaCreacion = DateTime.Now, UsuarioCreacion="Admin", FechaUltimaModificacion = DateTime.Now};
            var tagCorreo = new Etiqueta { Nombre = "Correo", FechaCreacion = DateTime.Now, UsuarioCreacion = "Admin", FechaUltimaModificacion = DateTime.Now };
            var tagSeguridad = new Etiqueta { Nombre = "Seguridad", FechaCreacion = DateTime.Now, UsuarioCreacion = "Admin", FechaUltimaModificacion = DateTime.Now };
            var tagPersonal = new Etiqueta { Nombre = "Personal", FechaCreacion = DateTime.Now, UsuarioCreacion = "Admin", FechaUltimaModificacion = DateTime.Now };
            var tagGeneral = new Etiqueta { Nombre = "General", FechaCreacion = DateTime.Now, UsuarioCreacion = "Admin", FechaUltimaModificacion = DateTime.Now };
            var tagMobile = new Etiqueta { Nombre = "MWC", FechaCreacion = DateTime.Now, UsuarioCreacion = "Admin", FechaUltimaModificacion = DateTime.Now };

            context.Etiquetas.AddOrUpdate(e => e.Nombre, tagPrestige, tagCorreo, tagSeguridad, tagPersonal, tagGeneral, tagMobile);
            context.SaveChanges();


            var entryAdjuntarCorreos = new Entrada
            {
                Titulo = "Adjuntar correos a correos en Gmail",
                Contenido = "Adjuntar correos en Gmail es muy sencillo",
                UsuarioCreacion = "Admin",
                FechaCreacion = DateTime.Today,
                Etiquetas = new List<Etiqueta>(new Etiqueta[] { tagCorreo })
            };

            var entryERTE = new Entrada
            {
                Titulo = "Solictud de ERTE por Covid-19",
                Contenido = "La empresa ha solicitado un ERTE con fecha efectiva 1 de Abril y esta esperando respuesta.",
                UsuarioCreacion = "Admin",
                FechaCreacion = DateTime.Today,
                Etiquetas = new List<Etiqueta>(new Etiqueta[] { tagPersonal, tagGeneral })
            };

            var entryVacaciones2020 = new Entrada
            {
                Titulo = "Vacaciones 2020",
                Contenido = "Disponéis de 22 días laborables. Teneis el calendario laboral 2020 aquí: [link]",
                UsuarioCreacion = "Admin",
                FechaCreacion = DateTime.Today,
                Etiquetas = new List<Etiqueta>(new Etiqueta[] { tagPersonal, tagGeneral })
            };

            var entryPrestige = new Entrada
            {
                Titulo = "Dirección web de PMS de cada Hotel",
                Contenido = "A continuación os indicamos la dirección de acceso al PMS de cada Hotel",
                UsuarioCreacion = "Admin",
                FechaCreacion = DateTime.Today,
                Etiquetas = new List<Etiqueta>(new Etiqueta[] { tagPrestige })
            };

            var entryMWC = new Entrada
            {
                Titulo = "Fechas MWC 2020",
                Contenido = "Las fecha para 2020 son las siguientes. Además el congreso se ha prolongado un año más debido a la cancelación en 2019",
                UsuarioCreacion = "Admin",
                FechaCreacion = DateTime.Today,
                Etiquetas = new List<Etiqueta>(new Etiqueta[] { tagPrestige })
            };

            context.Entradas.AddOrUpdate(e => e.Titulo, 
                entryAdjuntarCorreos, 
                entryERTE, 
                entryVacaciones2020, 
                entryPrestige, 
                entryMWC);

            context.SaveChanges();
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
