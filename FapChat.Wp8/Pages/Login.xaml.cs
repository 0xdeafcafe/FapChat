using System.Windows;
using System.Windows.Input;
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
                            // Save Acount Data
                            App.IsolatedStorage.UserAccount = repsonse.Item2;

                            // Navigate to Capture Page
                            NavigationService.Navigate(
                                Navigation.GenerateNavigateUri(Navigation.NavigationTarget.Capture));
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
            progress = new ProgressIndicator
            {
                IsVisible = false,
                IsIndeterminate = true,
                Text = "Waiting for Instructions..."
            };
            SystemTray.SetProgressIndicator(this, progress);
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
