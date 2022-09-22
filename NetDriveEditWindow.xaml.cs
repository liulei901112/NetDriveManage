using NetDriveManage.Core;
using NetDriveManage.Entity;
using NetDriveManage.Message;
using System.Collections.Generic;
using System.Windows;
using TextLocator.Util;

namespace NetDriveManage
{
    /// <summary>
    /// NetDriveEditWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NetDriveEditWindow : Window
    {
        private NetDriveInfo _driveInfo;
        public NetDriveEditWindow(NetDriveInfo driveInfo)
        {
            InitializeComponent();

            _driveInfo = driveInfo;
            if (_driveInfo == null)
            {
                _driveInfo = new NetDriveInfo()
                {
                    DriveId = System.Guid.NewGuid().ToString("N")
                };
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.NetDriveName.Text = _driveInfo.DriveName;
            this.NetDriveTag.Text = _driveInfo.DriveTag;

            this.Host.Text = _driveInfo.Host;
            this.Folder.Text = _driveInfo.Folder;
            this.User.Text = _driveInfo.User;
            this.Password.Text = _driveInfo.Password;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string driveName = this.NetDriveName.Text.Trim();
            string driveTag = this.NetDriveTag.Text.Trim();
            string host = this.Host.Text.Trim();
            string folder = this.Folder.Text.Trim();
            string user = this.User.Text.Trim();
            string password = this.Password.Text.Trim();

            if (string.IsNullOrEmpty(driveName))
            {
                MessageCore.ShowWarning("驱动器别称不能为空");
                return;
            }
            if (string.IsNullOrEmpty(driveTag))
            {
                MessageCore.ShowWarning("驱动器盘符不能为空，且不能重复。");
                return;
            }
            if (string.IsNullOrEmpty(host))
            {
                MessageCore.ShowWarning("主机Host不能为空");
                return;
            }

            _driveInfo.DriveName = driveName;
            _driveInfo.DriveTag = driveTag;
            _driveInfo.Host = host;
            _driveInfo.Folder = folder;
            _driveInfo.User = user;
            _driveInfo.Password = password;

            // 保存区域信息
            // AreaUtil.SaveAreaInfo(_areaInfo);
            List<NetDriveInfo> driveInfos = CacheUtil.Get<List<NetDriveInfo>>(AppConst.NET_DRIVE_INFOS_KEY);
            if (driveInfos.Contains(_driveInfo))
            {
                for (int i = 0; i < driveInfos.Count; i++)
                {
                    NetDriveInfo driveInfo = driveInfos[i];
                    if (_driveInfo.DriveId == driveInfo.DriveId)
                    {
                        driveInfos[i] = _driveInfo;
                    }
                    break;
                }
            }
            else
            {
                driveInfos.Add(_driveInfo);
            }

            CacheUtil.Put(AppConst.NET_DRIVE_INFOS_KEY, driveInfos);

            this.DialogResult = true;
            this.Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
