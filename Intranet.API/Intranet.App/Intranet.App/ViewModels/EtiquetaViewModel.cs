using Intranet.App.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Intranet.App.ViewModels
{
    public class EtiquetaViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

		private EtiquetaModel _etiquetaModel;
		public EtiquetaViewModel()
		{
			_etiquetaModel = new EtiquetaModel();
		}

		public EtiquetaViewModel(EtiquetaModel model)
		{
			_etiquetaModel = model;
			_nombre = model.Nombre;
		}

		private string _nombre;

		public string Nombre
		{
			get { return _nombre; }
			set { 
				var args = new PropertyChangedEventArgs(nameof(Nombre));

				if (_nombre == value) return;
				_nombre = value;
				PropertyChanged?.Invoke(this, args);

			}
		}

	}
}
