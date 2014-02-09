using System;
using FapChat.Core.Snapchat;
using FapChat.Wp8.Helpers;

namespace FapChat.Wp8.Pages.Authed
{
	public partial class Settings
	{
		public Settings()
		{
			InitializeComponent();
			DataContext = App.IsolatedStorage.UserAccount;
		}

		private void ButtonLogout_Click(object sender, EventArgs e)
		{
			Functions.Logout(App.IsolatedStorage.UserAccount.UserName, App.IsolatedStorage.UserAccount.AuthToken);

			App.IsolatedStorage.UserAccount = null;
			Navigation.NavigateTo(Navigation.NavigationTarget.Login);
		}
	}
}