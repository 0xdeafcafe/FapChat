using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using FapChat.Core.Snapchat.Models;
using FapChat.Wp8.Helpers;
using System;
using FapChat.Wp8.Interfaces;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace FapChat.Wp8.Pages.Authed
{
    public partial class Friends : IPhonePageExtender
    {
        public Friends()
        {
            InitializeComponent();

            RefreshBindings();
        }

        private void RefreshBindings()
        {
            ItemsAddedFriends.DataContext =
                App.IsolatedStorage.UserAccount.Friends.Where(f => f.Type == FriendType.Accepted);

            ItemsRequestedFriends.DataContext =
                App.IsolatedStorage.UserAccount.Snaps.Where(s => s.MediaType == MediaType.FriendRequest && App.IsolatedStorage.UserAccount.Friends.All(f => f.Name != s.ScreenName)).GroupBy(x => x.ScreenName).Select(y => y.First());

            ItemsPendingFriends.DataContext =
                App.IsolatedStorage.UserAccount.Friends.Where(f => f.Type == FriendType.PendingAccept);

            ItemsBlockedFriends.DataContext =
                App.IsolatedStorage.UserAccount.Friends.Where(f => f.Type == FriendType.Blocked);
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

        private void ButtonAddFriend_Click(object sender, EventArgs e)
        {
            var username = App.IsolatedStorage.UserAccount.UserName;
            var authToken = App.IsolatedStorage.UserAccount.AuthToken;

            var messageBox = new CustomMessageBox()
            {
                Caption = "Add a Friend",
                Message = "Enter the Snapchat Username of the friend you want to add.",
                Content = new PhoneTextBox { Text = "", Hint = "Snapchat Username", Margin = new Thickness(0, 10, 20, 0) },
                LeftButtonContent = "cancel",
                RightButtonContent = "done",
                IsFullScreen = false
            };
            messageBox.Dismissed += async (s1, e1) =>
            {
                switch (e1.Result)
                {
                    case CustomMessageBoxResult.RightButton:
                        // rename
                        var mb = ((CustomMessageBox)s1);
                        if (mb == null)
                            return;

                        var textbox = ((PhoneTextBox)mb.Content);
                        if (textbox == null)
                            return;
                        SetProgress("Sending Snapchat Friend Request...", true);
                        var response = await Core.Snapchat.Functions.Friend(textbox.Text, "add", username, authToken);
                        HideProgress(true);

                        Dispatcher.BeginInvoke(delegate
                        {
                            if (response == null)
                            {
                                // TODO: tell user shit failed
                            }
                            else if (!response.Logged)
                            {
                                // Logged Out
                                // TODO: Sign Out
                            }
                            else if (response.Object != null)
                            {
                                // le worked
                                try
                                {
                                    App.IsolatedStorage.UserAccount.Friends.Add(response.Object);
                                    RefreshBindings();

                                    MessageBox.Show("", "Success", MessageBoxButton.OK);
                                }
                                catch (Exception ex)
                                {
                                    // TODO: tell user something bad happened
                                }
                            }

                            // TODO: tell user shit failed
                        });
                        break;
                    case CustomMessageBoxResult.LeftButton:
                    case CustomMessageBoxResult.None:
                    default:
                        break;
                }
            };
            messageBox.Show();
        }

        private void ButtonAcceptFriend_Click(object sender, RoutedEventArgs e)
        {
            // Accept the friend request
            var button = ((Button)sender);
            if (button == null)
                return;

            var tag = button.Tag;
            if (tag == null)
                return;

            var friend = (button.Tag is Snap) ?
                (button.Tag as Snap).ScreenName :
                null;
            if (friend == null)
                return;

            var messageBox = new CustomMessageBox { Title = string.Format("Are you sure you want to add {0}?", friend), Message = "You shall be able to send and recieve snapchats from/to each other.", LeftButtonContent = "Yes", RightButtonContent = "No" };

            messageBox.Dismissed += async (s1, e1) =>
            {
                switch (e1.Result)
                {
                    case CustomMessageBoxResult.LeftButton:
                        var username = App.IsolatedStorage.UserAccount.UserName;
                        var authToken = App.IsolatedStorage.UserAccount.AuthToken;

                        // Add User
                        SetProgress("Syncing with Snapchat securely...", true);
                        var response = await Core.Snapchat.Functions.Friend(friend, "add", username, authToken);
                        HideProgress(true);

                        if (response == null)
                            MessageBox.Show("Uh Oh", "Unable to connect to the Snapchat Servers. Check your connection and try again.",
                                MessageBoxButton.OK);
                        else if (!response.Logged)
                        {
                            // TODO: Logout
                        }
                        else if (response.Object != null)
                        {
                            // Add user to friends
                            App.IsolatedStorage.UserAccount.Friends.Add(response.Object);

                            RefreshBindings();

                            MessageBox.Show("Successfull", string.Format("You have added {0} as a friend on snapchat!", friend),
                                MessageBoxButton.OK);
                        }
                        else
                            MessageBox.Show("Uh Oh", string.Format("There was an error adding {0} as a friend on snapchat.", friend),
                                MessageBoxButton.OK);
                        break;

                    case CustomMessageBoxResult.RightButton:
                    case CustomMessageBoxResult.None:
                    default:
                        break;
                }
            };
            messageBox.Show();
        }

        #region Overrides

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            SetProgress("Syncing with Snapchat securely...");

            var username = App.IsolatedStorage.UserAccount.UserName;
            var authToken = App.IsolatedStorage.UserAccount.AuthToken;

            var update = App.IsolatedStorage.UserAccount;
            var bests = App.IsolatedStorage.FriendsBests;

            if (App.IsolatedStorage.UserAccountLastUpdate + new TimeSpan(0, 0, 0, 30) < DateTime.UtcNow)
            {
                update = await Core.Snapchat.Functions.Update(username, authToken);
                Debug.WriteLine("updated update");
            }

            if (App.IsolatedStorage.FriendsBestsLastUpdate + new TimeSpan(0, 0, 0, 30) < DateTime.UtcNow)
            {
                bests =
                    await Core.Snapchat.Functions.GetBests(App.IsolatedStorage.UserAccount.Friends, username, authToken);
                Debug.WriteLine("updated bests");
            }

            HideProgress();
            if (update == null || bests == null)
            {
                Navigation.NavigateTo(Navigation.NavigationTarget.Login);
                MessageBox.Show("You are not authorized, please log back in.", "Unable to authenticate",
                    MessageBoxButton.OK);
            }
            else
            {
                App.IsolatedStorage.UserAccount = update;
                App.IsolatedStorage.FriendsBests = bests;
            }

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
