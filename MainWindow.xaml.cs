using log4net;
using NetDriveManage.Core;
using NetDriveManage.Entity;
using NetDriveManage.Message;
using NetDriveManage.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TextLocator.Util;

namespace NetDriveManage
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 正常驱动器信息列表
        /// </summary>
        private List<NetDriveInfo> _normalNetDriveInfos = new List<NetDriveInfo>();

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAreaInfoList();
        }

        /// <summary>
        /// 窗口激活的
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Activated(object sender, EventArgs e)
        {
            this.Show();
        }

        /// <summary>
        /// 窗口关闭中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        /// <summary>
        /// 加载驱动器信息列表
        /// <param name="driveInfos">驱动器信息列表</param>
        /// </summary>
        private void LoadAreaInfoList(List<NetDriveInfo> driveInfos = null)
        {
            // 外部传入驱动器信息列表为空
            if (driveInfos == null)
            {
                // 重新获取新的列表
                driveInfos = NetDriveUtil.GetDriveInfoList();
            }

            this.NetDriveInfoList.Children.Clear();

            if (driveInfos != null)
            {
                foreach (NetDriveInfo driveInfo in driveInfos)
                {
                    NetDriveInfoItem item = new NetDriveInfoItem(driveInfo);
                    // 连接按钮
                    item.ConnectButton.Tag = driveInfo;
                    item.ConnectButton.Click += ConnectButton_Click;
                    // 编辑按钮
                    item.EditButton.Tag = driveInfo;
                    item.EditButton.Click += EditButton_Click;
                    // 删除按钮
                    item.DeleteButton.Tag = driveInfo;
                    item.DeleteButton.Click += DeleteButton_Click;
                    // 选中或取消选中事件
                    item.NDI.Tag = driveInfo;
                    this.NetDriveInfoList.Children.Add(item);
                }
            }
            _normalNetDriveInfos = driveInfos;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            NetDriveInfo driveInfo = (NetDriveInfo)(sender as Button).Tag;
            if (driveInfo != null)
            {
                NetDriveUtil.DeleteDriveInfo(driveInfo);
                for (int i = 0; i < _normalNetDriveInfos.Count; i++)
                {
                    NetDriveInfo netDriveInfo = _normalNetDriveInfos[i];
                    if (netDriveInfo.DriveId == driveInfo.DriveId)
                    {
                        _normalNetDriveInfos.RemoveAt(i);
                        break;
                    }
                }

                // 重新加载区域信息列表（刷新）
                LoadAreaInfoList(_normalNetDriveInfos);
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            NetDriveInfo driveInfo = (NetDriveInfo)(sender as Button).Tag;
            ShowNetDriveEditDialog(driveInfo);
        }



        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ShowNetDriveEditDialog();
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            NetDriveInfo driveInfo = (NetDriveInfo)(sender as Button).Tag;
            if (!driveInfo.ConnectStatus)
            {
                // 连接
                NetUseUtil.Connect(driveInfo);
                (sender as Button).Background = Brushes.Red;
                (sender as Button).Content = "断开";
                driveInfo.ConnectStatus = true;
            }
            else
            {
                // 断开
                NetUseUtil.DisConnect(driveInfo);
                (sender as Button).Background = Brushes.Green;
                (sender as Button).Content = "连接";
                driveInfo.ConnectStatus = false;
            }
            (sender as Button).Tag = driveInfo;
        }

        /// <summary>
        /// 显示驱动器编辑
        /// </summary>
        /// <param name="driveInfo"></param>
        private void ShowNetDriveEditDialog(NetDriveInfo driveInfo = null)
        {
            CacheUtil.Put(AppConst.NET_DRIVE_INFOS_KEY, _normalNetDriveInfos);
            NetDriveEditWindow editDialog = new NetDriveEditWindow(driveInfo);
            editDialog.Topmost = true;
            editDialog.Owner = this;
            editDialog.ShowDialog();
            if (editDialog.DialogResult == true)
            {
                _normalNetDriveInfos = CacheUtil.Get<List<NetDriveInfo>>(AppConst.NET_DRIVE_INFOS_KEY);
                // 重新加载驱动器信息列表（刷新）
                LoadAreaInfoList(_normalNetDriveInfos);
            }
            RefreshDriveInfos();
        }

        private void RefreshDriveInfos()
        {
            // 保存正常的区域信息列表
            if (_normalNetDriveInfos != null)
            {
                foreach (NetDriveInfo driveInfo in _normalNetDriveInfos)
                {
                    NetDriveUtil.SaveDriveInfo(driveInfo);
                }
            }
        }
    }
}
