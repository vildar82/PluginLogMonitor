using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogMonitor.Core.AllUsers;
using LogMonitor.Core.NewUser;

namespace LogMonitor.Core.CheckUser
{
	public partial class FormCheckUser : Form
	{
		public FormCheckUser()
		{
			InitializeComponent();
		}

		private void buttonCheck_Click(object sender, EventArgs e)
		{
			try
			{
				var user = textBoxLogin.Text;
				if (string.IsNullOrEmpty(user)) throw new ArgumentException("Введи логин пользователя.");

				// Наличие в UserList
				if (NewUserService.IsUserExistInExcelUserList(user, out var group))
				{
					textBoxExistInUserList.Text = @"Есть";
					textBoxUserListGroup.Text = group;
					textBoxUserListGroupAD.Text = NewUserService.GetGroupADName(group);
				}
				else
				{
					textBoxExistInUserList.Text = @"Нет";
					textBoxUserListGroup.Text = "";
					textBoxUserListGroupAD.Text = "";
				}

				// Проверка AD
				var userAd = ADUtils.GetUserGroups(user, out var fio);
				if (userAd != null)
				{
					textBoxFIO.Text = fio;
					listBoxADGroups.DataSource = userAd.Groups;
					if (!string.IsNullOrEmpty(textBoxUserListGroupAD.Text))
					{
						textBoxADGroup.Text = userAd.Groups.Any(a => a == textBoxUserListGroupAD.Text).ToString();
					}
					// Проверка логов
					AddLogs(user);
				}
				else
				{
					MessageBox.Show($@"Не найден юзер '{user}'");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void AddLogs(string user)
		{
			richTextBoxLog.Text = LogService.GetUserLogs(user);
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{

		}
	}
}
