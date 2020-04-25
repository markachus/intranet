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
		private IEtiquetaService _etiquetasService = DependencyService.Get<IEtiquetaService>();
		private readonly IList<EtiquetaModel> _tags;

		public Command GuardarCommand { get; set; }
		public Command CancelarCommand { get; set; }
		public Command EliminarCommand { get; set; }

		public Command SeleccionaColorCommand { get; set; }

		public EtiquetaViewModel()
		{
		}

		private void InitCommands()
		{
			//Navegacion hacia atrás
			CancelarCommand = new Command(async () => {
				await Application.Current.MainPage.Navigation.PopAsync();
			});


			if (_etiquetaModel != null) { //Si etiqueta existente

				//Eliminar etiqueta
				EliminarCommand = new Command(async () => { 

					if (await Application.Current.MainPage.DisplayAlert(
						"Eliminar", 
						"Estás seguro de eliminar la etiqueta?", 
						"OK", "Cancelar"))
					{
						_etiquetasService.Delete(this.Nombre);
						if (_etiquetasService.LastRequestOk)
						{
							_tags?.Remove(_etiquetaModel);
							await Application.Current.MainPage.Navigation.PopAsync();
						}
						else {
							await Application.Current.MainPage.DisplayAlert(
							"Atención",
							_etiquetasService.LastMessage,
							"OK");
						}
					}
				});
			
			}

			//Guardar etiqueta sea alta o modificación
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

					if (_etiquetaModel == null) {
						//Nueva etiqueta
						var nueva = new EtiquetaModel { Nombre = this.Nombre };
						_etiquetasService.Add(nueva);
						if (_etiquetasService.LastRequestOk)
						{
							_tags?.Insert(0, nueva);
							await Application.Current.MainPage.Navigation.PopAsync();
						}
						else
						{
							await Application.Current.MainPage.DisplayAlert(
								"Atención",
								_etiquetasService.LastMessage,
								"OK");
						}

					} else {
						//Etiqueta existente
						_etiquetaModel.HexColor = this.HexColor;
						_etiquetasService.Update(_etiquetaModel);
						if (_etiquetasService.LastRequestOk)
						{
							await Application.Current.MainPage.Navigation.PopAsync();
						}
						else
						{
							await Application.Current.MainPage.DisplayAlert(
								"Atención",
								_etiquetasService.LastMessage,
								"OK");
						}
					}
				}
				catch (Exception ex)
				{
					await Application.Current.MainPage.DisplayAlert(
						"Error", ex.Message, "OK");
				}
			});
		}

		public EtiquetaViewModel(IList<EtiquetaModel> list = null, EtiquetaModel model = null)
		{
			_tags = list;
			_etiquetaModel = model;
			_nombre = _etiquetaModel?.Nombre;
			HexColor = _etiquetaModel?.HexColor;

			//Creacion de acciones / commands
			InitCommands();
			
		}

		//Propiedades enlazables

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

		private Color _tagColor = Color.Default;

		public Color TagColor
		{
			get { return _tagColor; }
			set { 

				var args = new PropertyChangedEventArgs(nameof(TagColor));

				if (_tagColor == value) return;
				_tagColor = value;
				PropertyChanged?.Invoke(this, args);

			}
		}

		private string _hexColor;

		public string HexColor
		{
			get { return _hexColor; }
			set {

				var args = new PropertyChangedEventArgs(nameof(HexColor));

				if (_hexColor == value) return;
				_hexColor = value;
				PropertyChanged?.Invoke(this, args);

				TagColor = Color.FromHex(_hexColor);
			}
		}

		/***
		 * Cierto si la etiqueta es nueva; false, si es una exstente 
		 */
		public bool IsNew
		{
			get { return _etiquetaModel != null; }
		}

	}
}
