using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using FapChat.Core.Snapchat.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace FapChat.Wp8.Pages.Authed.FriendsPages
{
    public partial class Info
    {
        private bool _isInPendingState;
        private Friend _friend;

        public Info()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string friendName;
            NavigationContext.QueryString.TryGetValue("friend-name", out friendName);
            if (friendName == null) { NavigationService.GoBack(); return; }

            var friend = App.IsolatedStorage.UserAccount.Friends.First(f => f.Name == friendName);
            if (friend == null || App.IsolatedStorage.FriendsBests == null) { NavigationService.GoBack(); return; }
            _friend = friend;

            LabelUserName.DataContext = App.IsolatedStorage.UserAccount.Friends.First(f => f.Name == friendName).Name;
            LabelDisplayName.DataContext = App.IsolatedStorage.UserAccount.Friends.First(f => f.Name == friendName).Display;

            Best usersBests;
            App.IsolatedStorage.FriendsBests.TryGetValue(friend.Name, out usersBests);
            if (usersBests == null)
            {
                usersBests = new Best
                {
                    BestFriends = new string[] {},
                    Score = 0
                };
            }
            DataContext = usersBests;

            base.OnNavigatedTo(e);
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (_isInPendingState)
                e.Cancel = true;

            base.OnBackKeyPress(e);
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {

        }

        private void ButtonBlock_Click(object sender, EventArgs e)
        {

        }

        private void ButtonRename_Click(object sender, EventArgs e)
        {
            StartPendingState("Creating a Display Name for user...");

            var username = App.IsolatedStorage.UserAccount.UserName;
            var authToken = App.IsolatedStorage.UserAccount.AuthToken;

            var messageBox = new CustomMessageBox()
            {
                Caption = string.Format("Do you want to add a display name for {0}", _friend.Name),
                Message = "This display name will appear under their name on the friends list. and is just for you, nobody else will see it.",
                Content = new PhoneTextBox { Text = _friend.Display, Hint = string.Format("Pick a Custom Name for {0}", _friend.Name), Margin = new Thickness(0,10,20,0)},
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
                        var mb = ((CustomMessageBox) s1);
                        if (mb == null)
                            return;

                        var textbox = ((PhoneTextBox) mb.Content);
                        if (textbox == null)
                            return;

                        var response = await Core.Snapchat.Functions.Friend(_friend.Name, "display", username, authToken, new Dictionary<string, string> { { "display", textbox.Text } });
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
                                    _friend = response.Object;
                                    App.IsolatedStorage.UserAccount.Friends.Find(f => f.Name == _friend.Name).Display = response.Object.Display;
                                }
                                catch (Exception ex)
                                {
                                    // TODO: tell user something bad happened
                                }
                            }
                            else
                            {
                                // TODO: tell user shit failed
                            }
                        });
                        break;
                    case CustomMessageBoxResult.LeftButton:
                    case CustomMessageBoxResult.None:
                    default:
                        break;
                }
                Dispatcher.BeginInvoke(EndPendingStage);
            };

            messageBox.Show();
        }

        private void StartPendingState(string status)
        {
            _isInPendingState = true;
            PendingOverlay.Visibility = Visibility.Visible;
            ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = false;
            ((ApplicationBarIconButton)ApplicationBar.Buttons[1]).IsEnabled = false;
            ((ApplicationBarIconButton)ApplicationBar.Buttons[2]).IsEnabled = false;
            ApplicationBar.IsMenuEnabled = false;
            SystemTray.SetProgressIndicator(this, new ProgressIndicator
                {
                    IsVisible = true,
                    IsIndeterminate = true,
                    Text = status
                });
        }
        private void EndPendingStage()
        {
            _isInPendingState = false;
            PendingOverlay.Visibility = Visibility.Collapsed;
            ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = true;
            ((ApplicationBarIconButton)ApplicationBar.Buttons[1]).IsEnabled = true;
            ((ApplicationBarIconButton)ApplicationBar.Buttons[2]).IsEnabled = true;
            ApplicationBar.IsMenuEnabled = true;
            SystemTray.SetProgressIndicator(this, new ProgressIndicator { IsVisible = false });
        }
    }
}