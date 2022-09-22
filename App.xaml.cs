using Hardcodet.Wpf.TaskbarNotification;
using log4net;
using NetDriveManage.Core;
using NetDriveManage.SingleInstance;
using NetDriveManage.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TextLocator.Util;

namespace NetDriveManage
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application, ISingleInstanceApp
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // 托盘图标
        private static TaskbarIcon _taskbar;
        public static TaskbarIcon Taskbar { get => _taskbar; set => _taskbar = value; }

        public App()
        {
            // 应用新版本配置文件
            AppUtil.WriteValue("AppConfig", "Time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"));
        }

        /// <summary>
        /// 入口函数
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string uniqueName = string.Format(CultureInfo.InvariantCulture, "Local\\{{{0}}}{{{1}}}", assembly.GetType().GUID, assembly.GetName().Name);
            if (SingleInstance<App>.InitializeAsFirstInstance(uniqueName))
            {
                var app = new App();
                app.InitializeComponent();
                app.Run();

                SingleInstance<App>.Cleanup();
            }
        }

        /// <summary>
        /// 信号外部命令行参数
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            /*if (this.MainWindow.WindowState == WindowState.Minimized)
            {
                this.MainWindow.WindowState = CacheUtil.Get<WindowState>("WindowState");
            }*/

            this.MainWindow.Show();
            this.MainWindow.Activate();
            return true;
        }

        /// <summary>
        /// 重写OnStartup
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // 托盘图标
            _taskbar = (TaskbarIcon)FindResource("Taskbar");

            base.OnStartup(e);

            // UI线程未捕获异常处理事件
            DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            // Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            // 非UI线程未捕获异常处理事件
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        #region 异常处理
        /// <summary>
        /// 非UI线程未捕获异常处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            if (e.IsTerminating)
            {
                builder.Append("非UI线程发生致命错误。");
            }
            builder.Append("非UI线程异常：");
            if (e.ExceptionObject is Exception)
            {
                builder.Append((e.ExceptionObject as Exception).Message);
            }
            else
            {
                builder.Append(e.ExceptionObject);
            }
            log.Error(builder.ToString());
        }

        /// <summary>
        /// Task线程内未捕获异常处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            log.Error("Task线程内未处理异常：" + e.Exception.Message, e.Exception);
            e.SetObserved();
        }

        /// <summary>
        /// UI线程未捕获异常处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                log.Error("UI线程未捕获异常：" + e.Exception.Message, e.Exception);
                // 处理完后，我们需要将Handler=true表示已此异常已处理过
                e.Handled = true;
            }
            catch (Exception ex)
            {
                log.Fatal("程序出现严重错误：" + ex.Message, ex);
            }
        }
        #endregion
    }
}
