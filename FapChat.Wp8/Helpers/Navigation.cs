using System;

namespace FapChat.Wp8.Helpers
{
    public static class Navigation
    {
        /// <summary>
        /// Targets Navigation can get to
        /// </summary>
        public enum NavigationTarget
        {
            Capture,
            Settings,
            Friends,
            Login
        }

        /// <summary>
        /// Generates a Windows Phone Path Url to the target you want to navigate to
        /// </summary>
        /// <param name="target">The target to navigate to</param>
        /// <returns>A string representation of the path</returns>
        public static string GenerateNavigateUrl(NavigationTarget target)
        {
            switch (target)
            {
                case NavigationTarget.Capture:
                    return "/Pages/Authed/Capture.xaml";

                case NavigationTarget.Settings:
                    return "/Pages/Authed/Settings.xaml";

                case NavigationTarget.Friends:
                    return "/Pages/Authed/Friends.xaml";

                case NavigationTarget.Login:
                default:
                    return "/Pages/Login.xaml";
            }
        }

        /// <summary>
        /// Generates a Windows Phone Path Uri to the target you want to navigate to
        /// </summary>
        /// <param name="target">The target to navigate to</param>
        /// <returns>A Uri representation of the path</returns>
        public static Uri GenerateNavigateUri(NavigationTarget target)
        {
            return new Uri(GenerateNavigateUrl(target), UriKind.Relative);
        }

        /// <summary>
        /// Navigates to the specified path
        /// </summary>
        /// <param name="path">The Uri path to navigate to.</param>
        public static void NavigateTo(Uri path)
        {
            App.RootFrame.Navigate(path);
        }

        /// <summary>
        /// Navigates to the specified path
        /// </summary>
        /// <param name="path">The Url path to navigate to.</param>
        public static void NavigateTo(string path)
        {
            NavigateTo(new Uri(path, UriKind.Relative));
        }

        /// <summary>
        /// Navigates to the specified path
        /// </summary>
        /// <param name="target">The target to navigate to. </param>
        public static void NavigateTo(NavigationTarget target)
        {
            NavigateTo(GenerateNavigateUrl(target));
        }
    }
}
