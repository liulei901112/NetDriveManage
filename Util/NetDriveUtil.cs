using NetDriveManage.Core;
using NetDriveManage.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDriveManage.Util
{
    /// <summary>
    /// 网络驱动器工具类
    /// </summary>
    public class NetDriveUtil
    {
        /// <summary>
        /// 驱动器配置
        /// </summary>
        private static string NetDriveConfig = AppConst.NET_DRIVE_CONFIG_KEY;
        /// <summary>
        /// 驱动器名称
        /// </summary>
        private static string NetDriveName = AppConst.NET_DRIVE_NAME_KEY;
        /// <summary>
        /// 驱动器盘符
        /// </summary>
        private static string NetDriveTag = AppConst.NET_DRIVE_TAG_KEY;
        /// <summary>
        /// 驱动器Host
        /// </summary>
        private static string ConnectHost = AppConst.CONNECT_HOST_KEY;
        /// <summary>
        /// 驱动器文件夹
        /// </summary>
        private static string ConnectFolder = AppConst.CONNECT_FOLDER_KEY;
        /// <summary>
        /// 驱动器访问用户
        /// </summary>
        private static string ConnectUser = AppConst.CONNECT_USER_KEY;
        /// <summary>
        /// 驱动器访问密码
        /// </summary>
        private static string ConnectPassword = AppConst.CONNECT_PASSWORD_KEY;

        /// <summary>
        /// 获取驱动器信息列表
        /// </summary>
        /// <returns></returns>
        public static List<NetDriveInfo> GetDriveInfoList()
        {
            List<NetDriveInfo> driveInfoList = new List<NetDriveInfo>();

            List<string> netDriveList = AppUtil.ReadSectionList(NetDriveConfig);
            if (netDriveList != null)
            {
                foreach (string driveId in netDriveList)
                {
                    // 根据驱动器获取明细配置
                    bool isEnable = bool.Parse(AppUtil.ReadValue(NetDriveConfig, driveId, "True"));
                    // 驱动器名称
                    string driveName = AppUtil.ReadValue(driveId, NetDriveName, "");
                    // 驱动器盘符
                    string driveTag = AppUtil.ReadValue(driveId, NetDriveTag, "");
                    // 驱动器主机
                    string host = AppUtil.ReadValue(driveId, ConnectHost, "");
                    // 驱动器文件夹
                    string folder = AppUtil.ReadValue(driveId, ConnectFolder, "");
                    // 访问用户
                    string user = AppUtil.ReadValue(driveId, ConnectUser, "");
                    // 访问密码
                    string password = AppUtil.ReadValue(driveId, ConnectPassword, "");

                    // 构造对象信息
                    NetDriveInfo netDriveInfo = new NetDriveInfo()
                    {
                        IsEnable = isEnable,
                        DriveId = driveId,
                        DriveName = driveName,
                        DriveTag = driveTag,
                        Host = host,
                        Folder = folder,
                        User = user,
                        Password = password
                    };
                    // 全部驱动器
                    driveInfoList.Add(netDriveInfo);
                }
            }
            return driveInfoList;
        }

        /// <summary>
        /// 保存驱动器信息
        /// </summary>
        /// <param name="areaInfo"></param>
        public static void SaveDriveInfo(NetDriveInfo driveInfo)
        {
            // 驱动器名称
            AppUtil.WriteValue(driveInfo.DriveId, NetDriveName, driveInfo.DriveName);
            // 盘符
            AppUtil.WriteValue(driveInfo.DriveId, NetDriveTag, driveInfo.DriveTag);
            // 主机
            AppUtil.WriteValue(driveInfo.DriveId, ConnectHost, driveInfo.Host);
            // 文件夹
            AppUtil.WriteValue(driveInfo.DriveId, ConnectFolder, driveInfo.Folder);
            // 用户
            AppUtil.WriteValue(driveInfo.DriveId, ConnectUser, driveInfo.User);
            // 密码
            AppUtil.WriteValue(driveInfo.DriveId, ConnectPassword, driveInfo.Password);

            // 驱动器列表
            AppUtil.WriteValue(NetDriveConfig, driveInfo.DriveId, driveInfo.IsEnable + "");
        }

        /// <summary>
        /// 删除驱动器信息
        /// </summary>
        /// <param name="areaInfo"></param>
        public static void DeleteDriveInfo(NetDriveInfo driveInfo)
        {
            AppUtil.WriteValue(driveInfo.DriveId, NetDriveName, null);
            AppUtil.WriteValue(driveInfo.DriveId, NetDriveTag, null);
            AppUtil.WriteValue(driveInfo.DriveId, ConnectHost, null);
            AppUtil.WriteValue(driveInfo.DriveId, ConnectFolder, null);
            AppUtil.WriteValue(driveInfo.DriveId, ConnectUser, null);
            AppUtil.WriteValue(driveInfo.DriveId, ConnectPassword, null);
            AppUtil.WriteValue(NetDriveConfig, driveInfo.DriveId, null);
        }
    }
}
