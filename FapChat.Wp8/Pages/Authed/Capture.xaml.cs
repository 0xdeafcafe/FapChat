using System.Windows.Navigation;
using FapChat.Wp8.Helpers;
using Microsoft.Devices;

namespace FapChat.Wp8.Pages.Authed
{
    public partial class Capture
    {
        private PhotoCamera _cam;

        public Capture()
        {
            InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Check to see if the camera is available on the phone.
            if (Camera.IsCameraTypeSupported(CameraType.Primary) ||
                 Camera.IsCameraTypeSupported(CameraType.FrontFacing))
            {
                // Initialize the camera, when available.
                _cam = Camera.IsCameraTypeSupported(CameraType.FrontFacing) ? new PhotoCamera(CameraType.FrontFacing) : new PhotoCamera(CameraType.Primary);

                //Set the VideoBrush source to the camera.
                ViewfinderBrush.SetSource(_cam);
            }

            base.OnNavigatedTo(e);
        }

        private void ButtonSettings_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Navigation.NavigateTo(Navigation.NavigationTarget.Settings);
        }
    }
}