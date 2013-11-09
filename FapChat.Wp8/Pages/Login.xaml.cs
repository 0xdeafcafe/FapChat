using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using FapChat.Core.Snapchat;
using FapChat.Wp8.Helpers;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace FapChat.Wp8.Pages
{
    public partial class Login
    {
        private new enum State
        {
            Login,
            Register
        }

        private State _pivotState = State.Login;

        public Login()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Delete Everything
            App.IsolatedStorage.Reset();

            while (NavigationService.CanGoBack)
                NavigationService.RemoveBackEntry();

            base.OnNavigatedTo(e);
        }

        private void Pivot_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ApplicationBar == null || ApplicationBar.Buttons[0] == null || e.AddedItems == null ||
                e.AddedItems.Count <= 0) return;

            var pivotTitle = ((PivotItem) e.AddedItems[0]).Header.ToString();
            ((ApplicationBarIconButton) ApplicationBar.Buttons[0]).Text = pivotTitle;
            switch (pivotTitle.ToLower())
            {
                case "login":
                    _pivotState = State.Login;
                    break;

                case "register":
                    _pivotState = State.Register;
                    break;
            }
        }

        private async void ApplicationBarActionButton_Click(object sender, System.EventArgs e)
        {
            PagePivot.Focus();
            PendingOverlay.Visibility = Visibility.Visible;
            ((ApplicationBarIconButton) ApplicationBar.Buttons[0]).IsEnabled = false;
            ApplicationBar.IsMenuEnabled = false;
            var progress = new ProgressIndicator
            {
                IsVisible = true,
                IsIndeterminate = true
            };

            switch (_pivotState)
            {
                case State.Login:
                    progress.Text = "Logging into Snapchat securely...";
                    SystemTray.SetProgressIndicator(this, progress);

                    var repsonse = await Functions.Login(TextUsername.Text, TextPassword.Password);
                    switch (repsonse.Item1)
                    {
                         case TempEnumHolder.LoginStatus.InvalidCredentials:
                            MessageBox.Show("Your username/password is incorrect.", "Invalid Credentials",
                                MessageBoxButton.OK);
                            break;

                         case TempEnumHolder.LoginStatus.ServerError:
                            MessageBox.Show("Unable to connect to server. Check your Internet Connection.", "Oops",
                                MessageBoxButton.OK);
                            break;

                         case TempEnumHolder.LoginStatus.Success:
                            // Tell User we be syncing
                            progress.Text = "Syncing with Snapchat securely...";
                            SystemTray.SetProgressIndicator(this, progress);

                            // Sync Other Shit
                            var bests = await Functions.GetBests(repsonse.Item2.Friends, repsonse.Item2.UserName, repsonse.Item2.AuthToken);
                            if (bests != null)
                            {
                                App.IsolatedStorage.UserAccount = repsonse.Item2;
                                App.IsolatedStorage.FriendsBests = bests;

                                // Navigate to Capture Page
                                NavigationService.Navigate(
                                    Navigation.GenerateNavigateUri(Navigation.NavigationTarget.Capture));
                            }
                            else
                                MessageBox.Show("Unable to sync with Snapchat's servers. Check your internet connection and try logging in again.", "Syncing Error", MessageBoxButton.OK);
                            break;
                    }
                    break;
                case State.Register:
                    progress.Text = "Registering your account with Snapchat securely...";
                    SystemTray.SetProgressIndicator(this, progress);

                    MessageBox.Show("Haven't coded this yet", "nope.", MessageBoxButton.OK);
                    break;
            }

            PendingOverlay.Visibility = Visibility.Collapsed;
            progress = new ProgressIndicator { IsVisible = false };
            SystemTray.SetProgressIndicator(this, progress);
            ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = true;
            ApplicationBar.IsMenuEnabled = true;
        }

        private void TextUsername_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                TextPassword.Focus();
        }
        private void TextPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ApplicationBarActionButton_Click(null, null);
        }
    }
}
