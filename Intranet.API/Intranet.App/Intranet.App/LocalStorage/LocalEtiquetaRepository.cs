using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Intranet.App.LocalStorage;
using Intranet.App.Models;
using SQLite;
using Xamarin.Essentials;

namespace Intranet.App.LocalStorage
{
    public class LocalEtiquetaRepository : ILocalEtiquetaRepository
    {

        /// <summary>
        /// Mensaje de estado en caso de error
        /// </summary>
        public string StatusMessage { get; private set; }

        private SQLiteConnection _connection = null;

        public LocalEtiquetaRepository()
        {
            var libFolder = FileSystem.AppDataDirectory;
            _connection = new SQLiteConnection(libFolder + "intranetlocal.db3");

            _connection.CreateTable<EtiquetaModel>();
        }

        /// <summary>
        /// Obtiene todas las etiquetas
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EtiquetaModel> GetTags() {

            try
            {
                return _connection.Table<EtiquetaModel>()
                    .OrderByDescending(t => t.FechaCreacion)
                    .ToList();
            }
            catch (Exception)
            {
                return new List<EtiquetaModel>();
            }
        }


        public EtiquetaModel GetTag(string name) {

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            return _connection.Table<EtiquetaModel>()
                .Where(t => t.Nombre == name)
                .FirstOrDefault();
        }

        public void AddNewTag(EtiquetaModel tag) {
            _connection.Insert(tag);
        }

    }
}
