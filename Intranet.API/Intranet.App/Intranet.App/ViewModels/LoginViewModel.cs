using Intranet.App.Services;
using Intranet.App.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Forms;

namespace Intranet.App.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public Command LoginCommand { get; private set; }

		public LoginViewModel()
		{
			LoginCommand = new Command(async () => {

				if (string.IsNullOrWhiteSpace(UserName)
						&& string.IsNullOrWhiteSpace(Password)) {

					await Application.Current.MainPage.DisplayAlert("Atención", "Faltan datos", "Cerrar");
					return;
				}

				var authSvc = DependencyService.Get<AuthService>();
				var tokenResp = await authSvc.GetTokenAsync(this.UserName, this.Password);

				await Application.Current.MainPage.Navigation.PopAsync();
				await Application.Current.MainPage.Navigation.PushAsync(new TagsListViewPage());

			});
		}


		private string _username;

		public string UserName
		{
			get { return _username; }
			set {
				var args = new PropertyChangedEventArgs(nameof(UserName));

				if (_username == value) return;
				_username = value;
				PropertyChanged?.Invoke(this, args);
			}
		}

		private string _password;

		public string Password
		{
			get { return _password; }
			set
			{
				var args = new PropertyChangedEventArgs(nameof(Password));
				if (_password == value) return;
				_password = value;
				PropertyChanged?.Invoke(this, args);
			}
		}

	}
}
