using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using SkyDriveHelpers.WP8;
using SkyDriveHelpers.WP8.Model;
using Microsoft.Live;
using System.ComponentModel;
using System.IO;

namespace SkyDriveTester
{
    public class MainPageViewModel : SkyDriveHelpers.WP8.Model.ModelBase
    {
        DispatcherSynchronizationContext _syncContext;

        public MainPageViewModel()
        {
            SkyDriveHelper     = new SkyDriveHelpers.WP8.SkyDriveHelper(null);
            Filenames          = new ObservableCollection<DirectoryEntry>();
            AddItemCommand     = new RelayCommand(OnAddItem);
            GetFileInfoCommand = new RelayCommand(OnGetFileInfo);
            GetFileCommand = new RelayCommand(OnGetFile);

            if (DesignerProperties.IsInDesignTool)
            {
                this.InitializeDesignData();
            }
        }

        private void OnGetFileInfo()
        {
            
        }

        public void SetSyncContext(Dispatcher dispatcher)
        {
            _syncContext = new DispatcherSynchronizationContext(App.Current.RootVisual.Dispatcher);
        }

        private async void OnAddItem()
        {
            List<DirectoryEntry> results = await SkyDriveHelper.GetDirectoryEntries("/");

            _syncContext.Send(delegate
            {
                foreach (var result in results)
                    Filenames.Add(result);
            }, null);


        }

        public void OnGetFile()
        {
            SkyDriveHelper.DownloadFile(SelectedDirectoryEntry.Id, "test.docx");

            return;
        }

        public async Task<LiveUserInfo> GetUserInfo()
        {
            var userInfo = await SkyDriveHelper.GetLiveUserInfo();

            _syncContext.Send(delegate
            {
                this.UserInfo = userInfo;
            }, null);

            return userInfo;
        }

        public async void SetSession(LiveConnectSession session)
        {
            _syncContext.Send(delegate
            {
                SkyDriveHelper.SetSession(session);
            }, null);
        }

        private DirectoryEntry _SelectedDirectoryEntry;
        public DirectoryEntry SelectedDirectoryEntry
        {
            get { return _SelectedDirectoryEntry; }
            set
            {
                _SelectedDirectoryEntry = value;
                SafeNotify("SelectedDirectoryEntry");
            }
        }
        
        public ICommand AddItemCommand { get; set; }
        public ICommand GetFileInfoCommand { get; set; }
        public ICommand GetFileCommand { get; set; }

        private ObservableCollection<DirectoryEntry> _Filenames;
        public ObservableCollection<DirectoryEntry> Filenames
        {
            get { return _Filenames; }
            set
            {
                _Filenames = value;
                SafeNotify("Filenames");
            }
        }

        private LiveUserInfo _UserInfo;
        public LiveUserInfo UserInfo
        {
            get { return _UserInfo; }
            set
            {
                _UserInfo = value;
                SafeNotify("UserInfo");
            }
        }

        private SkyDriveHelper _SkyDriveHelper;
        public SkyDriveHelper SkyDriveHelper
        {
            get { 
                return _SkyDriveHelper; }
            private set
            {
                _SkyDriveHelper = value;
                SafeNotify("SkyDriveHelper");
            }
        }

        public async void GetFiles()
        {
            List<DirectoryEntry> results = await SkyDriveHelper.GetDirectoryEntries("/");

            _syncContext.Send(delegate
            {
                this.Filenames = new ObservableCollection<DirectoryEntry>(results); ;
            }, null);
        }

        internal void InitializeDesignData()
        {
            Filenames.Add(new DirectoryEntry() { Name = "Directory 1", Type = "folder" });
            Filenames.Add(new DirectoryEntry() { Name = "Directory 2", Type = "folder" });
            Filenames.Add(new DirectoryEntry() { Name = "Directory 3", Type = "folder" });
            Filenames.Add(new DirectoryEntry() { Name = "Directory 4", Type = "folder" });

            Filenames.Add(new DirectoryEntry() { Name = "File 1", Type = "file" }); 
            Filenames.Add(new DirectoryEntry() { Name = "File 2", Type = "file" });
        }

        public async Task<DirectoryEntry> GetFolderInfo(DirectoryEntry directoryEntry)
        {
            
            var de = await SkyDriveHelper.GetDirectoryEntry(directoryEntry.Id);

            return de;
        }
    }
}
