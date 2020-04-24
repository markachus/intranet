using Intranet.App.Models;
using Intranet.App.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Intranet.App.ViewModels
{
    public class EtiquetaViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

		private EtiquetaModel _etiquetaModel;
		private EtiquetasServices _etiquetasManager = new EtiquetasServices();
		private readonly IList<EtiquetaModel> _tags;

		public Command GuardarCommand { get; set; }

		public EtiquetaViewModel()
		{
			_etiquetaModel = new EtiquetaModel();
		}

		public EtiquetaViewModel(IList<EtiquetaModel> list = null, EtiquetaModel model = null)
		{
			_tags = list;

			_etiquetaModel = model == null? new EtiquetaModel() : model;
			_nombre = _etiquetaModel.Nombre;

			GuardarCommand = new Command(async () => {
				try
				{
					if (String.IsNullOrEmpty(this.Nombre))
					{
						await Application.Current.MainPage.DisplayAlert(
							"Faltan campos", 
							"El nombre es obligatorio", 
							"OK");
						return;
					}
					_etiquetaModel.Nombre = this.Nombre;
					_etiquetasManager.AddEtiqueta(_etiquetaModel);
					if (_etiquetasManager.LastRequestOk)
					{
						_tags?.Insert(0, _etiquetaModel);
						await Application.Current.MainPage.Navigation.PopAsync();
					}
					else
					{
						await Application.Current.MainPage.DisplayAlert(
							"Atención", 
							_etiquetasManager.LastMessage, 
							"OK");
					}
				}
				catch (Exception ex)
				{
					await Application.Current.MainPage.DisplayAlert(
						"Error", ex.Message, "OK");
				}
			});
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
