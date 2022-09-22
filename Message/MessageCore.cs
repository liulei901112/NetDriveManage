using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace NetDriveManage.Message
{
    /// <summary>
    /// 消息盒子
    /// </summary>
    public class MessageCore
    {
        /// <summary>
        /// Rubyer.Message参数containerIdentifier
        /// </summary>
        private const string MESSAGE_CONTAINER = "MessageContainers";
        /// <summary>
        /// Rubyer.MessageBoxR参数containerIdentifier
        /// </summary>
        private const string MESSAGE_BOX_CONTAINER = "MessageBoxContainers";

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="message"></param>
        /// <param name="container"></param>
        public static void ShowWarning(string message, string container = MESSAGE_CONTAINER)
        {
            void TryShow()
            {
                Rubyer.Message.ShowWarning(container, message);
            }
            try
            {
                TryShow();
            }
            catch
            {
                Dispatcher.CurrentDispatcher.InvokeAsync(() =>
                {
                    TryShow();
                });
            }
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="message"></param>
        /// <param name="container"></param>
        public static void ShowSuccess(string message, string container = MESSAGE_CONTAINER)
        {
            void TryShow()
            {
                Rubyer.Message.ShowSuccess(container, message);
            }
            try
            {
                TryShow();
            }
            catch
            {
                Dispatcher.CurrentDispatcher.InvokeAsync(() =>
                {
                    TryShow();
                });
            }
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="message"></param>
        /// <param name="container"></param>
        public static void ShowError(string message, string container = MESSAGE_CONTAINER)
        {
            void TryShow()
            {
                Rubyer.Message.ShowError(container, message);
            }
            try
            {
                TryShow();
            }
            catch
            {
                Dispatcher.CurrentDispatcher.InvokeAsync(() =>
                {
                    TryShow();
                });
            }
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="container"></param>
        public static void ShowInfo(string message, string container = MESSAGE_CONTAINER)
        {
            void TryShow()
            {
                Rubyer.Message.ShowInfo(container, message);
            }
            try
            {
                TryShow();
            }
            catch
            {
                Dispatcher.CurrentDispatcher.InvokeAsync(() =>
                {
                    TryShow();
                });
            }
        }

        /// <summary>
        /// 消息提示框
        /// </summary>
        /// <param name="message">内容</param>
        /// <param name="title">标题</param>
        /// <param name="button">功能按钮</param>
        /// <param name="icon">图标</param>
        /// <param name="container"></param>
        /// <returns></returns>
        public static Task<MessageBoxResult> ShowMessageBox(string message, string title, MessageBoxButton button = MessageBoxButton.OKCancel, Rubyer.MessageBoxIcon icon = Rubyer.MessageBoxIcon.Question, string container = MESSAGE_BOX_CONTAINER)
        {
            return Rubyer.MessageBoxR.ConfirmInContainer(container, message, title, button, icon);
        }

        /// <summary>
        /// 版本更新提示
        /// </summary>
        /// <param name="message">更新提示内容</param>
        /// <param name="container"></param>
        /// <returns></returns>
        public static Task<MessageBoxResult> ShowUpdateMessage(string message, string container = MESSAGE_BOX_CONTAINER)
        {
            return Rubyer.MessageBoxR.ConfirmInContainer(container, message, "版本更新", MessageBoxButton.YesNo, Rubyer.MessageBoxIcon.Success);
        }
    }
}
