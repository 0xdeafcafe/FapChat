using System.ComponentModel;
using System.Windows;
using System.Windows.Navigation;
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
            var username = App.IsolatedStorage.UserAccount.UserName;
            var authToken = App.IsolatedStorage.UserAccount.AuthToken;
            
            var update = await Core.Snapchat.Functions.Update(username, authToken);
            var bests = await Core.Snapchat.Functions.GetBests(App.IsolatedStorage.UserAccount.Friends, username, authToken);

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