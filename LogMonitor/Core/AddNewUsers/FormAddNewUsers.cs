using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JetBrains.Annotations;
using LogMonitor.Core.NewUser;

namespace LogMonitor.Core.AddNewUsers
{
	public partial class FormAddNewUsers : Form
	{
		public FormAddNewUsers() : this(null)
		{
			
		}

		public FormAddNewUsers([CanBeNull] List<string> newUsers)
		{
			InitializeComponent();
			if (newUsers?.Any() == true)
			{
				foreach (var newUser in newUsers)
				{
					dgUsers.Rows.Add(newUser);
				}
			}
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow dgUsersRow in dgUsers.Rows)
			{
				var user = (string) dgUsersRow.Cells[0].Value;
				var group = (string) dgUsersRow.Cells[1].Value;
				if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(group))
				{
					continue;
				}
				try
				{
					NewUserService.RegNewUser(user, group);
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Ошибка регистрации пользователя {user} - {ex}");
				}
			}
			Close();
		}

		private void FormAddNewUsers_Load(object sender, EventArgs e)
		{

		}
	}
}
