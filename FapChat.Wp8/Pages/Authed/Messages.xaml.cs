using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using FapChat.Core.Snapchat.Models;
using FapChat.Wp8.Helpers;
using FapChat.Wp8.Interfaces;
using Microsoft.Phone.Shell;

namespace FapChat.Wp8.Pages.Authed
{
    public partial class Messages : IPhonePageExtender
    {
        public Messages()
        {
            InitializeComponent();

            UpdateBindings();
        }

        private void UpdateBindings()
        {
            ItemsSnaps.DataContext =
                App.IsolatedStorage.UserAccount.Snaps.Where(s => s.MediaType != MediaType.FriendRequest);
        }

        #region Overrides

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // start refresh
            SetProgress("Syncing with Snapchat securely...");

            var username = App.IsolatedStorage.UserAccount.UserName;
            var authToken = App.IsolatedStorage.UserAccount.AuthToken;

            var update = App.IsolatedStorage.UserAccount;

            if (App.IsolatedStorage.UserAccountLastUpdate + new TimeSpan(0, 0, 0, 30) < DateTime.UtcNow)
                update = await Core.Snapchat.Functions.Update(username, authToken);

            HideProgress();
            if (update == null)
            {
                Navigation.NavigateTo(Navigation.NavigationTarget.Login);
                MessageBox.Show("You are not authorized, please log back in.", "Unable to authenticate",
                    MessageBoxButton.OK);
            }
            else
                App.IsolatedStorage.UserAccount = update;

            UpdateBindings();

            base.OnNavigatedTo(e);
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (_maskLayerIndex > 0)
                e.Cancel = true;

            base.OnBackKeyPress(e);
        }

        #endregion

        #region Interfaces

        private int _maskLayerIndex;
        public void SetProgress(string message, bool showMask = false)
        {
            if (showMask)
            {
                _maskLayerIndex++;
                PendingOverlay.Visibility = Visibility.Collapsed;
                ApplicationBar.IsMenuEnabled = false;
            }

            SystemTray.SetProgressIndicator(this,
                   new ProgressIndicator
                   {
                       IsVisible = true,
                       IsIndeterminate = true,
                       Text = message
                   });
        }

        public void HideProgress(bool hideMask = false)
        {
            if (hideMask)
                _maskLayerIndex--;

            if (_maskLayerIndex == 0)
            {
                PendingOverlay.Visibility = Visibility.Collapsed;
                ApplicationBar.IsMenuEnabled = true;
            }

            SystemTray.SetProgressIndicator(this,
                    new ProgressIndicator
                    {
                        IsVisible = false,
                        IsIndeterminate = true,
                        Text = "[hidden] Waiting for Command..."
                    });
        }

        #endregion
    }
}