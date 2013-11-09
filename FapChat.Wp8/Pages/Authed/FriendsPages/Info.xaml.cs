using System;
using System.Linq;
using System.Windows.Navigation;
using FapChat.Core.Snapchat.Models;

namespace FapChat.Wp8.Pages.Authed.FriendsPages
{
    public partial class Info
    {
        public Info()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string friendName;
            NavigationContext.QueryString.TryGetValue("friend-name", out friendName);
            if (friendName == null) { NavigationService.GoBack(); return; }

            var friend = App.IsolatedStorage.UserAccount.Friends.First(f => f.Name == friendName);
            if (friend == null || App.IsolatedStorage.FriendsBests == null) { NavigationService.GoBack(); return; }

            LabelUserName.Text = friend.Name;

            Best usersBests;
            App.IsolatedStorage.FriendsBests.TryGetValue(friend.Name, out usersBests);
            if (usersBests == null)
            {
                usersBests = new Best
                {
                    BestFriends = new string[] {},
                    Score = 0
                };
            }
            DataContext = usersBests;

            base.OnNavigatedTo(e);
        }
    }
}