using System;
using System.Windows.Media;
using System.Windows.Navigation;
using FapChat.Wp8.Helpers;
using Microsoft.Devices;

namespace FapChat.Wp8.Pages.Authed
{
    public partial class Capture
    {
        private PhotoCamera _cam;
        private Boolean _hasRearCamera;
        private Boolean _hasFrontCamera;
        private Boolean _hasTwoCameras;
        public CameraType CurrentCameraMode = CameraType.FrontFacing;

        public Capture()
        {
            InitializeComponent();
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

            base.OnNavigatedTo(e);
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (_cam == null) return;

            // Dispose camera to minimize power consumption and to expedite shutdown.
            _cam.Dispose();
        }
        private void SetCameraType(CameraType cameraType)
        {
            _cam = new PhotoCamera(CurrentCameraMode = cameraType);

            switch (cameraType)
            {
                case CameraType.FrontFacing:
                    ViewfinderBrush.RelativeTransform =
                        new CompositeTransform() { CenterX = 0.5, CenterY = 0.5, Rotation = -90 };
                    break;

                case CameraType.Primary:
                    ViewfinderBrush.RelativeTransform =
                        new CompositeTransform() { CenterX = 0.5, CenterY = 0.5, Rotation = 90 };
                    break;
            }

            ViewfinderBrush.SetSource(_cam);
        }

        private void ButtonSettings_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Navigation.NavigateTo(Navigation.NavigationTarget.Settings);
        }
        private void ButtonSwitchCamera_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (CurrentCameraMode == CameraType.FrontFacing && _hasRearCamera)
                SetCameraType(CameraType.Primary);
            else if (CurrentCameraMode == CameraType.Primary && _hasFrontCamera)
                SetCameraType(CameraType.FrontFacing);
        }
        private void ButtonViewFriends_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Navigation.NavigateTo(Navigation.NavigationTarget.Friends);
        }
    }
}