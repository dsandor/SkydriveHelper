using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SkyDriveTester.Resources;
using SkyDriveHelpers.WP8;
using SkyDriveHelpers.WP8.Model;
using System.ComponentModel;

namespace SkyDriveTester
{
    public partial class MainPage : PhoneApplicationPage
    {
        MainPageViewModel viewModel = new MainPageViewModel();

        public MainPage()
        {
            InitializeComponent();

            this.DataContext = viewModel;
        }

        private async void SignInButton_SessionChanged(object sender, Microsoft.Live.Controls.LiveConnectSessionChangedEventArgs e)
        {
            // When a user is signed in to live services this code executes to configure the SkyDriveHelper.
            if (e.Status == Microsoft.Live.LiveConnectSessionStatus.Connected)
            {
                viewModel.SetSession(e.Session);
                viewModel.UserInfo = await viewModel.SkyDriveHelper.GetLiveUserInfo();
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel.SetSyncContext(this.Dispatcher);
        }

        private async void LongListSelector_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            viewModel.SelectedDirectoryEntry = ((LongListSelector)sender).SelectedItem as DirectoryEntry;
            var sde = await viewModel.GetFolderInfo(viewModel.SelectedDirectoryEntry);
        }


    }
}