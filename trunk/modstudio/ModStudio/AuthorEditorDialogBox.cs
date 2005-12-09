/***************************************************************************
 *                          AuthorEditorDialogBox.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: AuthorEditorDialogBox.cs,v 1.5 2005-12-09 00:50:06 smithydll Exp $
 *
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using ModTemplateTools;
using ModTemplateTools.DataStructures;

namespace ModStudio
{
	/// <summary>
	/// Summary description for AuthorEditorDialogBox.
	/// </summary>
	public class AuthorEditorDialogBox : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBoxUserName;
		private System.Windows.Forms.TextBox textBoxRealName;
		private System.Windows.Forms.TextBox textBoxEmail;
		private System.Windows.Forms.TextBox textBoxWebpage;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// 
		/// </summary>
		public AuthorEditorDialogBox()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panel1 = new System.Windows.Forms.Panel();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOk = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textBoxUserName = new System.Windows.Forms.TextBox();
			this.textBoxRealName = new System.Windows.Forms.TextBox();
			this.textBoxEmail = new System.Windows.Forms.TextBox();
			this.textBoxWebpage = new System.Windows.Forms.TextBox();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.buttonCancel);
			this.panel1.Controls.Add(this.buttonOk);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 110);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(504, 40);
			this.panel1.TabIndex = 1;
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonCancel.Location = new System.Drawing.Point(344, 8);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "&Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonOk
			// 
			this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOk.Location = new System.Drawing.Point(424, 8);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.TabIndex = 2;
			this.buttonOk.Text = "&OK";
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(112, 23);
			this.label1.TabIndex = 2;
			this.label1.Text = "Username:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(8, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(112, 23);
			this.label2.TabIndex = 2;
			this.label2.Text = "Real Name:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.label2.Click += new System.EventHandler(this.label2_Click);
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(8, 56);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(112, 23);
			this.label3.TabIndex = 2;
			this.label3.Text = "E-mail:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(8, 80);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(112, 23);
			this.label4.TabIndex = 2;
			this.label4.Text = "Webpage:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textBoxUserName
			// 
			this.textBoxUserName.Location = new System.Drawing.Point(128, 8);
			this.textBoxUserName.Name = "textBoxUserName";
			this.textBoxUserName.Size = new System.Drawing.Size(368, 20);
			this.textBoxUserName.TabIndex = 3;
			this.textBoxUserName.Text = "";
			// 
			// textBoxRealName
			// 
			this.textBoxRealName.Location = new System.Drawing.Point(128, 32);
			this.textBoxRealName.Name = "textBoxRealName";
			this.textBoxRealName.Size = new System.Drawing.Size(368, 20);
			this.textBoxRealName.TabIndex = 3;
			this.textBoxRealName.Text = "";
			// 
			// textBoxEmail
			// 
			this.textBoxEmail.Location = new System.Drawing.Point(128, 56);
			this.textBoxEmail.Name = "textBoxEmail";
			this.textBoxEmail.Size = new System.Drawing.Size(368, 20);
			this.textBoxEmail.TabIndex = 3;
			this.textBoxEmail.Text = "";
			// 
			// textBoxWebpage
			// 
			this.textBoxWebpage.Location = new System.Drawing.Point(128, 80);
			this.textBoxWebpage.Name = "textBoxWebpage";
			this.textBoxWebpage.Size = new System.Drawing.Size(368, 20);
			this.textBoxWebpage.TabIndex = 3;
			this.textBoxWebpage.Text = "";
			// 
			// AuthorEditorDialogBox
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(504, 150);
			this.ControlBox = false;
			this.Controls.Add(this.textBoxUserName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textBoxRealName);
			this.Controls.Add(this.textBoxEmail);
			this.Controls.Add(this.textBoxWebpage);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AuthorEditorDialogBox";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Author";
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// 
		/// </summary>
		public delegate void AuthorEditorDialogBoxSaveHandler(object sender, AuthorEditorDialogBoxSaveEventArgs e);
		/// <summary>
		/// 
		/// </summary>
		//public event AuthorEditorDialogBoxSaveHandler Save;

		private void label2_Click(object sender, System.EventArgs e)
		{
		
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			this.Hide();
		}

		private void buttonOk_Click(object sender, System.EventArgs e)
		{
			/*this.Save(this, new AuthorEditorDialogBoxSaveEventArgs(textBoxUserName.Text, textBoxRealName.Text, textBoxEmail.Text, textBoxWebpage.Text));
			this.Hide();*/
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="username"></param>
		/// <param name="realname"></param>
		/// <param name="email"></param>
		/// <param name="website"></param>
		public void SetAuthor(string username, string realname, string email, string website) 
		{
			textBoxUserName.Text = username;
			textBoxRealName.Text = realname;
			textBoxEmail.Text = email;
			textBoxWebpage.Text = website;
		}

		/// <summary>
		/// 
		/// </summary>
		public ModAuthorEntry Entry
		{
			get
			{
				return new ModAuthorEntry(textBoxUserName.Text, textBoxRealName.Text, textBoxEmail.Text, textBoxWebpage.Text);
			}
			set
			{
				textBoxUserName.Text = value.UserName;
				textBoxRealName.Text = value.RealName;
				textBoxEmail.Text = value.Email;
				textBoxWebpage.Text = value.Homepage;
			}
		}

	}

	/// <summary>
	/// 
	/// </summary>
	public class AuthorEditorDialogBoxSaveEventArgs: EventArgs 
	{
		/// <summary>
		/// 
		/// </summary>
		public ModAuthorEntry Entry;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="username"></param>
		/// <param name="realname"></param>
		/// <param name="email"></param>
		/// <param name="website"></param>
		public AuthorEditorDialogBoxSaveEventArgs(string username, string realname, string email, string website) 
		{
			Entry = new ModAuthorEntry(username, realname, email, website);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entry"></param>
		public AuthorEditorDialogBoxSaveEventArgs(ModAuthorEntry entry)
		{
			Entry = entry;
		}
	}
}
