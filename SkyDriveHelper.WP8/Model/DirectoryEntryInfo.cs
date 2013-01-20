using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyDriveHelpers.WP8.Model
{
    public class DirectoryEntryInfo : ModelBase
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

    }
}
