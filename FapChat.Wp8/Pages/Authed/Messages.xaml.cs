using System;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
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

            MediaContainer.Visibility = Visibility.Collapsed;

            UpdateBindings();
        }

        private bool _mediaIsBeingDisplayed;
        private async void ButtonSnap_Click(object sender, RoutedEventArgs e)
        {
            if (_mediaIsBeingDisplayed)
            {
                EndMedia();
                return;
            }

            var button = sender as Button;
            if (button == null || button.Tag == null)
                return;

            var snap = button.Tag as Snap;
            if (snap == null)
                return;

            if (snap.Status == SnapStatus.Downloading)
                return;

            var username = App.IsolatedStorage.UserAccount.UserName;
            var authToken = App.IsolatedStorage.UserAccount.AuthToken;

            if (snap.HasMedia || snap.RecipientName != null || snap.Status != SnapStatus.Delivered) return;

            snap.Status = SnapStatus.Downloading;
            UpdateBindings();

            var blob = await Core.Snapchat.Functions.GetBlob(snap.Id, username, authToken);
            var cachedBlob = new CachedMediaBlob
            {
                BlobMediaType = snap.MediaType,
                Id = snap.Id
            };
            cachedBlob.SetLocalFileBytes(blob);

            App.IsolatedStorage.CachedMediaBlobs.Add(cachedBlob);
            App.IsolatedStorage.CachedMediaBlobs = App.IsolatedStorage.CachedMediaBlobs;

            snap.Status = SnapStatus.Delivered;
            UpdateBindings();
        }
        private void ButtonSnap_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            if (_mediaIsBeingDisplayed)
            {
                EndMedia();
                return;
            }

            var button = sender as Button;
            if (button == null || button.Tag == null) return;

            var snap = button.Tag as Snap;
            if (snap == null || snap.Status == SnapStatus.Downloading) return;

            if (!snap.HasMedia && snap.RecipientName == null && snap.Status == SnapStatus.Delivered) return;

            var cachedMediaBlob = App.IsolatedStorage.CachedMediaBlobs.FirstOrDefault(c => c.Id == snap.Id);
            if (cachedMediaBlob == null) return;

            StartMedia(cachedMediaBlob);
        }
        private void ButtonSnap_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            if (!_mediaIsBeingDisplayed) return;
            EndMedia();
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
                Navigation.NavigateToAndRemoveBackStack(Navigation.NavigationTarget.Login);
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

        #region Media

        private void StartMedia(CachedMediaBlob blob)
        {
            SystemTray.IsVisible = ApplicationBar.IsVisible = false;
            ScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            MediaContainer.Visibility = Visibility.Visible;

            var blobData = blob.RetrieveBlobData();

            switch (blob.BlobMediaType)
            {
                case MediaType.FriendRequestImage:
                case MediaType.Image:
                    MediaViewerImage.Source = blobData as BitmapImage;
                    break;

                case MediaType.Video:
                case MediaType.VideoNoAudio:
                case MediaType.FriendRequestVideo:
                case MediaType.FriendRequestVideoNoAudio:
                    MediaViewerVideo.SetSource(blobData as IsolatedStorageFileStream);
                    break;
            }

            _mediaIsBeingDisplayed = true;
        }

        private void EndMedia()
        {
            SystemTray.IsVisible = ApplicationBar.IsVisible = true;
            _mediaIsBeingDisplayed = false;

            MediaContainer.Visibility = Visibility.Collapsed;
            ScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            MediaViewerImage.Source = null;
            MediaViewerVideo.Source = null;
        }

        #endregion
    }
}
