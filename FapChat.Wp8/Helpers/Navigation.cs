using System;
using System.Collections.Generic;
using System.Net;

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
            FriendInfo,
            Login
        }

        /// <summary>
        /// Generates a Windows Phone Path Url to the target you want to navigate to
        /// </summary>
        /// <param name="target">The target to navigate to</param>
        /// <param name="queryParams">Optional extra query string params.</param>
        /// <returns>A string representation of the path</returns>
        public static string GenerateNavigateUrl(NavigationTarget target, Dictionary<string, string> queryParams = null)
        {
            string path;
            switch (target)
            {
                case NavigationTarget.Capture:
                    path = "/Pages/Authed/Capture.xaml";
                    break;

                case NavigationTarget.Settings:
                    path = "/Pages/Authed/Settings.xaml";
                    break;

                case NavigationTarget.Friends:
                    path = "/Pages/Authed/Friends.xaml";
                    break;

                case NavigationTarget.FriendInfo:
                    path = "/Pages/Authed/FriendsPages/Info.xaml";
                    break;

                case NavigationTarget.Login:
                default:
                    path = "/Pages/Login.xaml";
                    break;
            }

            if (queryParams == null) return path;

            var first = true;
            foreach (var queryParam in queryParams)
            {
                if (first)
                    path += string.Format("?{0}={1}", queryParam.Key, HttpUtility.HtmlEncode(queryParam.Value));
                else
                    path += string.Format("&{0}={1}", queryParam.Key, HttpUtility.HtmlEncode(queryParam.Value));

                first = false;
            }
            return path;
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
        /// <param name="target">The target to navigate to.</param>
        /// <param name="queryParams">Optional extra query string params.</param>
        public static void NavigateTo(NavigationTarget target, Dictionary<string, string> queryParams = null)
        {
            NavigateTo(GenerateNavigateUrl(target, queryParams));
        }
    }
}
