using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDriveManage.Entity
{
    /// <summary>
    /// 网络驱动器信息
    /// </summary>
    public class NetDriveInfo
    {
        public bool IsEnable { get; set; }
        public string DriveId { get; set; }
        public string DriveName { get; set; }
        public string DriveTag { get; set; }
        public string Host { get; set; }
        public string Folder { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool ConnectStatus { get; set; }
    }
}
