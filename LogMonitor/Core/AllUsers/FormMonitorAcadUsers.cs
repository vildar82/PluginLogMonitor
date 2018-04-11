using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogMonitor.Core.AllUsers
{
   public partial class FormMonitorAcadUsers : Form
   {
      public FormMonitorAcadUsers(string report)
      {
         InitializeComponent();
         richTextBox1.Text = report;
      }

      private void buttonSave_Click(object sender, EventArgs e)
      {
         var dialogSaveFile = new SaveFileDialog
         {
            AddExtension = true,
            CheckPathExists = true,
            DefaultExt = "txt"
         };

         if (dialogSaveFile.ShowDialog() == DialogResult.OK)
         {
            File.WriteAllText(dialogSaveFile.FileName, richTextBox1.Text);
         }
      }
   }
}
