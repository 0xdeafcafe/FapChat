using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;
using FapChat.Core.Snapchat.Models;
using FapChat.Wp8.Helpers;
using Microsoft.Devices;

namespace FapChat.Wp8.Pages.Authed
{
	public partial class Capture
	{
		public CameraType CurrentCameraMode = CameraType.FrontFacing;
		private PhotoCamera _camera;
		private Boolean _hasFrontCamera;
		private Boolean _hasRearCamera;
		private Boolean _hasTwoCameras;

		public Capture()
		{
			InitializeComponent();
		}

		public void UpdateBindings()
		{
			ButtonMessages.DataContext =
				App.IsolatedStorage.UserAccount.Snaps.Count(
					s => s.MediaType != MediaType.FriendRequest && s.Status == SnapStatus.Delivered);
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			_hasRearCamera = Camera.IsCameraTypeSupported(CameraType.Primary);
			_hasFrontCamera = Camera.IsCameraTypeSupported(CameraType.FrontFacing);
			_hasTwoCameras = (_hasRearCamera && _hasFrontCamera);

			// Check to see if the camera is available on the phone.
			if (_hasRearCamera || _hasFrontCamera)
				SetCameraType(_hasFrontCamera ? CameraType.FrontFacing : CameraType.Primary);

			ButtonSwitchCamera.IsEnabled = _hasTwoCameras;
			UpdateBindings();

			base.OnNavigatedTo(e);
		}

		protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
		{
			if (_camera == null) return;

			// Dispose camera to minimize power consumption and to expedite shutdown.
			_camera.Dispose();
		}


		private void SetCameraType(CameraType cameraType)
		{
			_camera = new PhotoCamera(CurrentCameraMode = cameraType);

			switch (cameraType)
			{
				case CameraType.FrontFacing:
					ViewfinderBrush.RelativeTransform =
						new CompositeTransform {CenterX = 0.5, CenterY = 0.5, Rotation = -90};
					break;

				case CameraType.Primary:
					ViewfinderBrush.RelativeTransform =
						new CompositeTransform {CenterX = 0.5, CenterY = 0.5, Rotation = 90};
					break;
			}

			ViewfinderBrush.SetSource(_camera);
		}

		private void ButtonSwitchCamera_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentCameraMode == CameraType.FrontFacing && _hasRearCamera)
				SetCameraType(CameraType.Primary);
			else if (CurrentCameraMode == CameraType.Primary && _hasFrontCamera)
				SetCameraType(CameraType.FrontFacing);
		}

		private void ButtonViewFriends_Click(object sender, RoutedEventArgs e)
		{
			Navigation.NavigateTo(Navigation.NavigationTarget.Friends);
		}

		private void ButtonMessages_Click(object sender, RoutedEventArgs e)
		{
			Navigation.NavigateTo(Navigation.NavigationTarget.Messages);
		}

		private void ButtonSettings_Click(object sender, RoutedEventArgs e)
		{
			Navigation.NavigateTo(Navigation.NavigationTarget.Settings);
		}
	}
}