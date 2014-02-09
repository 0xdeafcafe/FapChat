using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Navigation;
using FapChat.Core.Snapchat;
using FapChat.Core.Snapchat.Models;
using FapChat.Wp8.Helpers;

namespace FapChat.Wp8.Pages.Authed
{
	public partial class Loading
	{
		public Loading()
		{
			InitializeComponent();
		}

		#region Overrides

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			string username = App.IsolatedStorage.UserAccount.UserName;
			string authToken = App.IsolatedStorage.UserAccount.AuthToken;

			Account update = await Functions.Update(username, authToken);
			Dictionary<string, Best> bests =
				await Functions.GetBests(App.IsolatedStorage.UserAccount.Friends, username, authToken);

			if (update == null || bests == null)
			{
				Navigation.NavigateToAndRemoveBackStack(Navigation.NavigationTarget.Login);
				MessageBox.Show("You are not authorized, please log back in.", "Unable to authenticate",
					MessageBoxButton.OK);
			}
			else
			{
				App.IsolatedStorage.UserAccount = update;
				App.IsolatedStorage.FriendsBests = bests;
				Navigation.NavigateToAndRemoveBackStack(Navigation.NavigationTarget.Capture);
			}

			base.OnNavigatedTo(e);
		}

		protected override void OnBackKeyPress(CancelEventArgs e)
		{
			e.Cancel = true;

			base.OnBackKeyPress(e);
		}

		#endregion
	}
}