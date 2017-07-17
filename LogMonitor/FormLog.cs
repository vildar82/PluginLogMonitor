using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogMonitor.Core;
using LogMonitor.Core.AddNewUsers;
using LogMonitor.Core.AllUsers;
using Microsoft.Win32;

namespace LogMonitor
{
    public partial class FormLog : Form
    {
        private readonly LogService _logService;
        private string _reportMonitorAcadUsers = string.Empty;

        public FormLog ()
        {
            InitializeComponent();
            _logService = new LogService();
            StartMonitoring();
            RegisterInStartup(true);

            backgroundWorker1.DoWork += BackgroundWorker1_DoWork;
            backgroundWorker1.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            backgroundWorker1.RunWorkerAsync();
        }

        private void BackgroundWorker1_RunWorkerCompleted (object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            var formAcadUsers = new FormMonitorAcadUsers(_reportMonitorAcadUsers);
            formAcadUsers.Show();

            var frmNewUsers = new Core.NewUser.FormNewUsers ();
            frmNewUsers.Show();
        }

        private void BackgroundWorker1_DoWork (object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var monitorAcadUsers = new MonitorAcadUsers();
            monitorAcadUsers.CheckAllUsers();
            _reportMonitorAcadUsers = monitorAcadUsers.Report;
        }

        private void ShowFormMonitoringAcadUsers (MonitorAcadUsers monitorAcadUsers)
        {
            var formMonitoracadUsers = new FormMonitorAcadUsers(monitorAcadUsers.Report);
            formMonitoracadUsers.Show();
        }

        private void buttonStart_Click (object sender, EventArgs e)
        {
            StartMonitoring();
        }

        private void StartMonitoring ()
        {
            _logService.Start();
            UpdateState();
        }

        private void buttonStop_Click (object sender, EventArgs e)
        {
            _logService.Stop();
        }

        private void FormLog_Deactivate (object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_Click (object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                notifyIcon1.Visible = false;
                UpdateState();
            }
        }

        private void UpdateState ()
        {
            label1.Text = "Последнее сканирование в " + _logService.LastScan;
            richTextBox1.Text = _logService.Body;
        }

        private void RegisterInStartup (bool isChecked)
        {
            var registryKey = Registry.CurrentUser.OpenSubKey
                 ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (isChecked)
            {
                registryKey.SetValue("PluginLogMonitor", Application.ExecutablePath);
            }
            else
            {
                registryKey.DeleteValue("PluginLogMonitor");
            }
        }

		private void bAddNewUsers_Click(object sender, EventArgs e)
		{
			var formAddNewUsers = new FormAddNewUsers();
			formAddNewUsers.Show();
		}

		private void FormLog_Load(object sender, EventArgs e)
		{

		}
	}
}