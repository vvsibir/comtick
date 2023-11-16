using ComTick;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace wComTick
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            init();
        }
        NotifyIcon notifyIcon1;
        ContextMenu menu;
        void init()
        {
            this.Text = Application.ProductName;
            this.Icon = wComTick.Properties.Resources.appIcon; ;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Minimized;

            // Create the NotifyIcon.
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon();

            // The Icon property sets the icon that will appear
            // in the systray for this application.
            notifyIcon1.Icon = this.Icon;
            

            // The Text property sets the text that will be displayed,
            // in a tooltip, when the mouse hovers over the systray icon.
            notifyIcon1.Text = "ComTick form";
            notifyIcon1.Visible = true;

            //notifyIcon1.Click += NotifyIcon1_Click;
            notifyIcon1.MouseClick += NotifyIcon1_MouseClick;
            notifyIcon1.ContextMenu = createMenu();

            SharedTools.log.Logged += Log_Logged;
            SharedTools.log.ClearLog += Log_ClearLog;


            try
            {
                cur.cmdr = new CommandURLReader() { URLCmd = cur.startArgs.getWithKeys(cur.startArgs.URLCmd), Interval = Convert.ToInt32(cur.startArgs.Interval ?? "1000") };
                cmd.Exiting += Cmd_Exiting;
            }
            catch (Exception ex)
            {
                SharedTools.log.Write("Error on init:\n" + ex.Message);
            }

            cmd.ManualExit = true;
            cur.cmdr.Run();


        }

        private void Log_ClearLog(object sender, EventArgs e)
        {
            if (File.Exists(getLogFileName()))
            {
                try
                {
                    File.Delete(getLogFileName());
                }
                catch (Exception ex)
                {
                    log(ex.Message);
                }
            }
        }

        private void Cmd_Exiting(object sender, EventArgs e)
        {
            // фу как некрасиво, но ничего не придумал еще, потому что иначе - ошибка "доступ к форме между потоками", т.к. команды cur.cmdr'а выполняются в другом потоке            
            BeginInvoke(new MethodInvoker(delegate
            {
                //wapi.CloseWindow(this.Handle);
                forceClose();
            }));
            
        }

        FormWindowState oldState = FormWindowState.Normal;

        private void NotifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                openMe();
            }
            //else if(e.Button == MouseButtons.Right)
            //{
            //    menu.Show(this, e.Location);
            //}
        }

        private void openMe()
        {
            WindowState = oldState;
            this.Show();
            this.BringToFront();
            this.Activate();
        }

        private ContextMenu createMenu()
        {
            menu = new ContextMenu();
            MenuItem mi = new MenuItem() { Text = "Закрыть" };
            mi.Click += Mi_Click;
            menu.MenuItems.Add(mi);

            return menu;
        }

        private void Mi_Click(object sender, EventArgs e)
        {
            forceClose();
        }

        private void forceClose()
        {
            try
            {
                menuClosing = true;
                this.Close();
            }
            finally
            {
                menuClosing = false;
            }
        }

        private void Log_Logged(object sender, SharedTools.log.LogEventArgs e)
        {
            //tLOG.AppendText(string.Format("{0}\n", e.GetMessage()));
            log(e.GetMessage());
        }

        int maxSize = 1024 * 1024;
        void log(string msg)
        {
            File.AppendAllText(getLogFileName(), msg + "\r\n");
            FileInfo f = new FileInfo(getLogFileName());
            if (f.Length >= maxSize)
            {
                f.MoveTo(Path.Combine(f.Directory.FullName, "old" + DateTime.Now.ToString("yyyyMMdd_HHmmss_fff") + ".log"));
                f.Create();
            }
        }

        private string getLogFileName()
        {
            return Application.ProductName + ".log";
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            Hide();
        }

        bool menuClosing = false;
        protected override void OnClosing(CancelEventArgs e)
        {
            if (!menuClosing)
            {
                e.Cancel = true;
                Hide();
            }
            else
                base.OnClosing(e);
        }

        //protected override void OnResizeBegin(EventArgs e)
        //{
        //    //oldState = this.WindowState; не будем делать, чтобы всегда восстанавливалось в нормальное окно
        //    base.OnResizeBegin(e);
        //}
        //protected override void OnResizeEnd(EventArgs e)
        //{
        //    base.OnResizeEnd(e);
        //    //if(this.WindowState == FormWindowState.Minimized) notifyIcon1.
        //    if (WindowState == FormWindowState.Minimized) ShowInTaskbar = false;
        //    else ShowInTaskbar = true;
        //}

        //protected override void OnDeactivate(EventArgs e)
        //{
        //    base.OnDeactivate(e);
        //    //ShowInTaskbar = false;
        //}
        //protected override void OnActivated(EventArgs e)
        //{
        //    base.OnActivated(e);
        //    //ShowInTaskbar = true;
        //}

        protected override void OnClosed(EventArgs e)
        {
            //notifyIcon1.Click -= NotifyIcon1_Click;
            //notifyIcon1.MouseClick -= NotifyIcon1_MouseClick;
            notifyIcon1.Visible = false;
            notifyIcon1.Dispose();
            notifyIcon1 = null;
            base.OnClosed(e);
        }
    }
}
