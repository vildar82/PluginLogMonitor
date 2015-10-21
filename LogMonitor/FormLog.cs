using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogMonitor.Core;

namespace LogMonitor
{
   public partial class FormLog : Form
   {
      private LogService _logService;      

      public FormLog()
      {
         InitializeComponent();
         _logService = new LogService();         
      }

      private void buttonStart_Click(object sender, EventArgs e)
      {
         _logService.Start();
         UpdateState();
      }

      private void buttonStop_Click(object sender, EventArgs e)
      {
         _logService.Stop();
      }

      private void notifyIcon1_Click(object sender, EventArgs e)
      {
         if (this.WindowState == FormWindowState.Minimized)
         {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon1.Visible = false;
            UpdateState();
         }
      }

      private void UpdateState()
      {
         label1.Text = "Последнее сканирование в " + _logService.LastScan;         
         richTextBox1.Text = _logService.Body;
      }

      private void FormLog_Deactivate(object sender, EventArgs e)
      {
         if (this.WindowState == FormWindowState.Minimized)
         {
            this.ShowInTaskbar = false;
            notifyIcon1.Visible = true;
         }
      }
   }
}
