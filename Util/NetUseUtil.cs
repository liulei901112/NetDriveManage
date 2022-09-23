using log4net;
using NetDriveManage.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDriveManage.Util
{
    /// <summary>
    /// net use 工具类
    /// </summary>
    public class NetUseUtil
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Connect(NetDriveInfo driveInfo)
        {
            driveInfo.DriveTag = driveInfo.DriveTag.Replace("：", "").Replace(":", "");
            driveInfo.Host = driveInfo.Host.Replace("\\\\", "").Replace("//", "");

            string args = string.Format("use {0}: \\\\{1}\\{2}", driveInfo.DriveTag, driveInfo.Host, driveInfo.Folder);
            if (!string.IsNullOrEmpty(driveInfo.Password))
            {
                args += " " + driveInfo.Password;
            }
            if (!string.IsNullOrEmpty(driveInfo.User))
            {
                args += " /user:" + driveInfo.User;
            }
            ExecCMD(args);
        }

        public static void DisConnect(NetDriveInfo driveInfo)
        {
            driveInfo.DriveTag = driveInfo.DriveTag.Replace("：", "").Replace(":", "");
            driveInfo.Host = driveInfo.Host.Replace("\\\\", "").Replace("//", "");

            string args = string.Format("use {0}: /del /y", driveInfo.DriveTag);
            ExecCMD(args);
        }

        private static void ExecCMD(string args) {
            try
            {
                log.Debug("net.exe " + args); 
                Process.Start("net.exe", args);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }
    }
}
