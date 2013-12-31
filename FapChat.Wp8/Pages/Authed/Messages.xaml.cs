using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
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

            var checkExpireTimes = new DispatcherTimer();
            checkExpireTimes.Tick += (sender, args) =>
            {
                if (App.IsolatedStorage == null || App.IsolatedStorage.UserAccount == null ||
                    App.IsolatedStorage.UserAccount.Snaps == null)
                    return;

                foreach (var snap in App.IsolatedStorage.UserAccount.Snaps.Where(snap => snap.RecipientName == null))
                {
                    switch (snap.GetState)
                    {
                        case SnapState.Expired:
                            if (snap.Status != SnapStatus.Opened)
                            {
                                snap.RemainingSeconds = null;
                                snap.Status = SnapStatus.Opened;
                                UpdateBindings();

                                if (_currentSnap == snap)
                                    EndMedia();
                            }
                            break;

                        case SnapState.Available:
                            // ReSharper disable once PossibleInvalidOperationException
                            var secondsRemaining = Convert.ToInt32(((DateTime)snap.OpenedAt - DateTime.UtcNow).TotalSeconds) +
                                       snap.CaptureTime;

                            if (secondsRemaining <= 0)
                            {
                                snap.Status = SnapStatus.Opened;
                                snap.RemainingSeconds = null;
                                UpdateBindings();

                                if (_currentSnap == snap)
                                    EndMedia();
                            }
                            else
                                snap.RemainingSeconds = secondsRemaining;
                            break;
                    }
                }
            };
            checkExpireTimes.Interval = new TimeSpan(0, 0, 0, 1);
            checkExpireTimes.Start();
        }
        
        private bool _mediaIsBeingDisplayed;
        private Snap _currentSnap;
        private bool _mouseStillDown;
        private double _scrollYIndex;
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
            var openedAt = DateTime.UtcNow;
            App.IsolatedStorage.UserAccount.Snaps.First(s => s.Id == snap.Id).OpenedAt = openedAt;

            snap.Status = SnapStatus.Delivered;
            UpdateBindings();

            if (snap.CaptureTime == null) return;

            SetProgress("Syncing with Snapchat...");

            //await Core.Snapchat.Functions.SendViewedEvent(snap.Id,
            //    Core.Snapchat.Helpers.Timestamps.ConvertToUnixTimestamp(openedAt), (int)snap.CaptureTime, username,
            //    authToken);
            //App.IsolatedStorage.UserAccountUpdate(await Core.Snapchat.Functions.Update(username, authToken));
            //UpdateBindings();

            HideProgress();
        }
        private void ButtonSnap_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            if (_mouseStillDown)
                return;

            _mouseStillDown = true;
            _scrollYIndex = ScrollViewer.VerticalOffset;
            var timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 250) };
            timer.Tick += (o, args) =>
            {
                timer.Stop();
                if (!_mouseStillDown)
                    return;

                var diff = _scrollYIndex - ScrollViewer.VerticalOffset;
                Debug.WriteLine(diff);
                if (diff < -10 || diff > 10)
                    return;

                if (_mediaIsBeingDisplayed)
                {
                    EndMedia();
                    return;
                }

                var button = sender as Button;
                if (button == null || button.Tag == null) return;

                var snap = button.Tag as Snap;
                if (snap == null || snap.Status == SnapStatus.Downloading) return;

                if (!snap.HasMedia && snap.RecipientName == null) return;
                if (snap.Status != SnapStatus.Delivered) return;

                var cachedMediaBlob = App.IsolatedStorage.CachedMediaBlobs.FirstOrDefault(c => c.Id == snap.Id);
                if (cachedMediaBlob == null) return;

                StartMedia(cachedMediaBlob, snap);
            };
            timer.Start();
        }
        private void ButtonSnap_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            _mouseStillDown = false;

            if (!_mediaIsBeingDisplayed) return;
            EndMedia();
        }

        private void UpdateBindings()
        {
            ItemsSnaps.DataContext =
                App.IsolatedStorage.UserAccount.Snaps.Where(s => s.MediaType != MediaType.FriendRequest)
                    .OrderByDescending(s => s.SentTimestamp);
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

            if (_mediaIsBeingDisplayed)
            {
                e.Cancel = true;
                EndMedia();
            }

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

        private void StartMedia(CachedMediaBlob blob, Snap snap)
        {
            SystemTray.IsVisible = ApplicationBar.IsVisible = false;
            ScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            MediaContainer.Visibility = Visibility.Visible;

            var blobData = blob.RetrieveBlobData();
            MediaCountdownTimer.DataContext = snap;
            _currentSnap = snap;

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
                    var leSnap = App.IsolatedStorage.UserAccount.Snaps.FirstOrDefault(s => s.Id == snap.Id);
                    if (leSnap != null)
                        leSnap.CaptureTime = 69;
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
            MediaCountdownTimer.DataContext = null;
            _currentSnap = null;
        }

        private void MediaViewerVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (_mediaIsBeingDisplayed)
                EndMedia();
        }

        #endregion
    }
}
