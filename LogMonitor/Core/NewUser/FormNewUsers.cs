using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogMonitor.Core.NewUser;

namespace LogMonitor.Core.NewUser
{
    public partial class FormNewUsers : Form
    {
        public FormNewUsers ()
        {
            InitializeComponent();

            listBoxRegNewUsers.DataSource = NewUserService.NewUsers;
            listBoxErrors.DataSource = NewUserService.Errors;
        }
    }
}
