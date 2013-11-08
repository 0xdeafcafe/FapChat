using System;
using System.Windows;
using System.Windows.Navigation;

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
    }
}
