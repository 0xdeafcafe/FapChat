using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using FapChat.Core.Snapchat.Models;
using FapChat.Wp8.Helpers;
using System;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace FapChat.Wp8.Pages.Authed
{
    public partial class Friends
    {
        public Friends()
        {
            InitializeComponent();

            DataContext = App.IsolatedStorage.UserAccount;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemTray.SetProgressIndicator(this,
                   new ProgressIndicator
                   {
                       IsVisible = true,
                       IsIndeterminate = true,
                       Text = "Syncing with Snapchat securely..."
                   });

            var username = App.IsolatedStorage.UserAccount.UserName;
            var authToken = App.IsolatedStorage.UserAccount.AuthToken;

            var update = await Core.Snapchat.Functions.Update(username, authToken);
            var bests = await Core.Snapchat.Functions.GetBests(App.IsolatedStorage.UserAccount.Friends, username, authToken);

            SystemTray.SetProgressIndicator(this, new ProgressIndicator { IsVisible = false });
            if (update == null || bests == null)
            {
                // TODO: Signout
            }
            else
            {
                App.IsolatedStorage.UserAccount = update;
                App.IsolatedStorage.FriendsBests = bests;
            }

            base.OnNavigatedTo(e);
        }

        private void ButtonCapture_Click(object sender, EventArgs e)
        {
            Navigation.NavigateTo(Navigation.NavigationTarget.Capture);
        }

        private void ButtonFriendDetails_Click(object sender, RoutedEventArgs e)
        {
            var button = ((Button) sender);
            if (button == null)
                return;

            var tag = button.Tag;
            if (tag == null)
                return;

            var friend = (button.Tag is Friend) ? 
                button.Tag as Friend : 
                null;
            if (friend == null)
                return;

            Navigation.NavigateTo(Navigation.NavigationTarget.FriendInfo, new Dictionary<string, string> { { "friend-name", friend.Name } });
        }
    }
}
