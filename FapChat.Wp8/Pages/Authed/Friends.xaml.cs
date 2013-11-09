using FapChat.Wp8.Helpers;
using System;

namespace FapChat.Wp8.Pages.Authed
{
    public partial class Friends
    {
        public Friends()
        {
            InitializeComponent();
        }

        private void ButtonCapture_Click(object sender, EventArgs e)
        {
            Navigation.NavigateTo(Navigation.NavigationTarget.Capture);
        }
    }
}
