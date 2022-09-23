using log4net;
using NetDriveManage.Entity;
using System;
using System.Windows.Controls;

namespace NetDriveManage
{
    /// <summary>
    /// NetDriveInfoItem.xaml 的交互逻辑
    /// </summary>
    public partial class NetDriveInfoItem : UserControl
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public NetDriveInfoItem(NetDriveInfo driveInfo)
        {
            InitializeComponent();
            try
            {
                Refresh(driveInfo);
            }
            catch
            {
                Dispatcher.InvokeAsync(() =>
                {
                    try
                    {
                        Refresh(driveInfo);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message, ex);
                    }
                });
            }
        }

        private void Refresh(NetDriveInfo driveInfo)
        {
            this.DriveName.Text = driveInfo.DriveName;
            this.DriveTag.Text = driveInfo.DriveTag;
            this.ConnectFolder.Text = driveInfo.Host;
            this.ConnectFolder.Text = driveInfo.Folder;
        }
    }
}
