using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyDriveHelpers.WP8.Model
{
    public class DirectoryEntry : ModelBase 
    {


        private string _Id;
        public string Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                SafeNotify("Id");
            }
        }

        private string _From;
        public string From
        {
            get { return _From; }
            set
            {
                _From = value;
                SafeNotify("From");
            }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                SafeNotify("Name");
            }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                _Description = value;
                SafeNotify("Description");
            }
        }

        private string _ParentId;
        public string ParentId
        {
            get { return _ParentId; }
            set
            {
                _ParentId = value;
                SafeNotify("ParentId");
            }
        }

        private long _Size;
        public long Size
        {
            get { return _Size; }
            set
            {
                _Size = value;
                SafeNotify("Size");
            }
        }

        private string _UploadLocation;
        public string UploadLocation
        {
            get { return _UploadLocation; }
            set
            {
                _UploadLocation = value;
                SafeNotify("UploadLocation");
            }
        }

        private string _Link;
        public string Link
        {
            get { return _Link; }
            set
            {
                _Link = value;
                SafeNotify("Link");
            }
        }

        private string _Type;
        public string Type
        {
            get { return _Type; }
            set
            {
                _Type = value;
                SafeNotify("Type");
            }
        }

    }
}
