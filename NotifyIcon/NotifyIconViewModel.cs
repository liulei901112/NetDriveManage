using NetDriveManage.Core;
using System.Windows;
using System.Windows.Input;

namespace NetDriveManage.NotifyIcon
{
    public class NotifyIconViewModel
    {
        /// <summary>
        /// 激活窗口
        /// </summary>
        public ICommand ShowWindowCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () =>
                    {
                        // 显示
                        Application.Current.MainWindow.Show();
                        // 标准化
                        Application.Current.MainWindow.Activate();
                    }
                };
            }
        }
        

        /// <summary>
        /// 隐藏窗口
        /// </summary>
        public ICommand HideWindowCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () => Application.Current.MainWindow.Hide()
                };
            }
        }


        /// <summary>
        /// 关闭软件
        /// </summary>
        public ICommand ExitApplicationCommand
        {
            get
            {
                return new DelegateCommand { CommandAction = () => AppCore.Shutdown() };
            }
        }
    }
}
