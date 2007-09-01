/***************************************************************************
 *                             OptionsDialog.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: OptionsDialog.cs,v 1.9 2007-09-01 13:52:37 smithydll Exp $
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
using Microsoft.Win32;
using Phpbb.ModTeam.Tools;
using Phpbb.ModTeam.Tools.DataStructures;

namespace ModStudio
{
	/// <summary>
	/// Summary description for OptionsDialog.
	/// </summary>
	public class OptionsDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Label labelModAuthor;
		private AuthorEditorDialog authorEditorDialog1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox comboBox2;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox comboBoxDescriptionTabbing;
		private System.Windows.Forms.ComboBox comboBoxFilesToEditTabbing;
		private System.Windows.Forms.ComboBox comboBoxIncludedFilesTabbing;
		private System.Windows.Forms.ComboBox comboBoxAuthorNotesTabbing;
		private System.Windows.Forms.ComboBox comboBoxAuthorNotesStartLine;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// 
		/// </summary>
		public OptionsDialog()
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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.comboBox2 = new System.Windows.Forms.ComboBox();
			this.button3 = new System.Windows.Forms.Button();
			this.labelModAuthor = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.comboBoxDescriptionTabbing = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.comboBoxFilesToEditTabbing = new System.Windows.Forms.ComboBox();
			this.comboBoxIncludedFilesTabbing = new System.Windows.Forms.ComboBox();
			this.comboBoxAuthorNotesTabbing = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.comboBoxAuthorNotesStartLine = new System.Windows.Forms.ComboBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.authorEditorDialog1 = new AuthorEditorDialog();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(410, 320);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.comboBox2);
			this.tabPage1.Controls.Add(this.button3);
			this.tabPage1.Controls.Add(this.labelModAuthor);
			this.tabPage1.Controls.Add(this.comboBox1);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Controls.Add(this.label2);
			this.tabPage1.Controls.Add(this.label3);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(402, 294);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Defaults";
			// 
			// comboBox2
			// 
			this.comboBox2.Enabled = false;
			this.comboBox2.Items.AddRange(new object[] {
														   "Windows \\r\\n",
														   "Unix \\n",
														   "Mac (Legacy) \\r"});
			this.comboBox2.Location = new System.Drawing.Point(120, 112);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(121, 21);
			this.comboBox2.TabIndex = 4;
			this.comboBox2.Text = "Windows \\r\\n";
			// 
			// button3
			// 
			this.button3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button3.Location = new System.Drawing.Point(24, 24);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 18);
			this.button3.TabIndex = 3;
			this.button3.Text = "Edit";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// labelModAuthor
			// 
			this.labelModAuthor.Location = new System.Drawing.Point(112, 8);
			this.labelModAuthor.Name = "labelModAuthor";
			this.labelModAuthor.Size = new System.Drawing.Size(280, 72);
			this.labelModAuthor.TabIndex = 2;
			// 
			// comboBox1
			// 
			this.comboBox1.Enabled = false;
			this.comboBox1.Items.AddRange(new object[] {
														   "Text",
														   "Xml"});
			this.comboBox1.Location = new System.Drawing.Point(120, 88);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(121, 21);
			this.comboBox1.TabIndex = 1;
			this.comboBox1.Text = "Text";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.TabIndex = 0;
			this.label1.Text = "Default Author:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(8, 88);
			this.label2.Name = "label2";
			this.label2.TabIndex = 0;
			this.label2.Text = "MOD Format:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(8, 112);
			this.label3.Name = "label3";
			this.label3.TabIndex = 0;
			this.label3.Text = "Line Break:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.groupBox1);
			this.tabPage2.Controls.Add(this.label8);
			this.tabPage2.Controls.Add(this.comboBoxAuthorNotesStartLine);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(402, 294);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Formatting";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.comboBoxDescriptionTabbing);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.comboBoxFilesToEditTabbing);
			this.groupBox1.Controls.Add(this.comboBoxIncludedFilesTabbing);
			this.groupBox1.Controls.Add(this.comboBoxAuthorNotesTabbing);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(384, 136);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Tabbing";
			// 
			// comboBoxDescriptionTabbing
			// 
			this.comboBoxDescriptionTabbing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxDescriptionTabbing.Items.AddRange(new object[] {
																			"1 Space",
																			"1 Tab",
																			"Left Aligned"});
			this.comboBoxDescriptionTabbing.Location = new System.Drawing.Point(120, 24);
			this.comboBoxDescriptionTabbing.Name = "comboBoxDescriptionTabbing";
			this.comboBoxDescriptionTabbing.Size = new System.Drawing.Size(121, 21);
			this.comboBoxDescriptionTabbing.TabIndex = 2;
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(8, 24);
			this.label4.Name = "label4";
			this.label4.TabIndex = 1;
			this.label4.Text = "Description:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.Location = new System.Drawing.Point(8, 48);
			this.label5.Name = "label5";
			this.label5.TabIndex = 1;
			this.label5.Text = "Files to edit:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label6.Location = new System.Drawing.Point(8, 72);
			this.label6.Name = "label6";
			this.label6.TabIndex = 1;
			this.label6.Text = "Included Files:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label7.Location = new System.Drawing.Point(8, 96);
			this.label7.Name = "label7";
			this.label7.TabIndex = 1;
			this.label7.Text = "Author Notes:";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboBoxFilesToEditTabbing
			// 
			this.comboBoxFilesToEditTabbing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxFilesToEditTabbing.Items.AddRange(new object[] {
																			"1 Space",
																			"1 Tab",
																			"Left Aligned"});
			this.comboBoxFilesToEditTabbing.Location = new System.Drawing.Point(120, 48);
			this.comboBoxFilesToEditTabbing.Name = "comboBoxFilesToEditTabbing";
			this.comboBoxFilesToEditTabbing.Size = new System.Drawing.Size(121, 21);
			this.comboBoxFilesToEditTabbing.TabIndex = 2;
			// 
			// comboBoxIncludedFilesTabbing
			// 
			this.comboBoxIncludedFilesTabbing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxIncludedFilesTabbing.Items.AddRange(new object[] {
																			  "1 Space",
																			  "1 Tab",
																			  "Left Aligned"});
			this.comboBoxIncludedFilesTabbing.Location = new System.Drawing.Point(120, 72);
			this.comboBoxIncludedFilesTabbing.Name = "comboBoxIncludedFilesTabbing";
			this.comboBoxIncludedFilesTabbing.Size = new System.Drawing.Size(121, 21);
			this.comboBoxIncludedFilesTabbing.TabIndex = 2;
			// 
			// comboBoxAuthorNotesTabbing
			// 
			this.comboBoxAuthorNotesTabbing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxAuthorNotesTabbing.Items.AddRange(new object[] {
																			"1 Space",
																			"1 Tab",
																			"Left Aligned"});
			this.comboBoxAuthorNotesTabbing.Location = new System.Drawing.Point(120, 96);
			this.comboBoxAuthorNotesTabbing.Name = "comboBoxAuthorNotesTabbing";
			this.comboBoxAuthorNotesTabbing.Size = new System.Drawing.Size(121, 21);
			this.comboBoxAuthorNotesTabbing.TabIndex = 2;
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label8.Location = new System.Drawing.Point(16, 160);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(104, 23);
			this.label8.TabIndex = 1;
			this.label8.Text = "Start Author notes:";
			this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboBoxAuthorNotesStartLine
			// 
			this.comboBoxAuthorNotesStartLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxAuthorNotesStartLine.Items.AddRange(new object[] {
																			  "On same line",
																			  "On new line"});
			this.comboBoxAuthorNotesStartLine.Location = new System.Drawing.Point(128, 160);
			this.comboBoxAuthorNotesStartLine.Name = "comboBoxAuthorNotesStartLine";
			this.comboBoxAuthorNotesStartLine.Size = new System.Drawing.Size(121, 21);
			this.comboBoxAuthorNotesStartLine.TabIndex = 2;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.button2);
			this.panel1.Controls.Add(this.button1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 320);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(410, 48);
			this.panel1.TabIndex = 1;
			// 
			// button2
			// 
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button2.Location = new System.Drawing.Point(248, 8);
			this.button2.Name = "button2";
			this.button2.TabIndex = 1;
			this.button2.Text = "Ok";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button1
			// 
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(328, 8);
			this.button1.Name = "button1";
			this.button1.TabIndex = 0;
			this.button1.Text = "Cancel";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// authorEditorDialog1
			// 
			this.authorEditorDialog1.Save += new ModStudio.AuthorEditorDialogBox.AuthorEditorDialogBoxSaveHandler(authorEditorDialog1_Save);
			// 
			// OptionsDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(410, 368);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OptionsDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Options";
			this.Load += new System.EventHandler(this.OptionsDialog_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void OptionsDialog_Load(object sender, System.EventArgs e)
		{
			RegistryKey reg = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("VB and VBA Program Settings").OpenSubKey("MODStudio").OpenSubKey("mod-settings");
			labelModAuthor.Text = "Username: " + (String)reg.GetValue("author_username", "UserName");
			labelModAuthor.Text += "\r\nReal Name: " + (String)reg.GetValue("author_realname", "RealName");
			labelModAuthor.Text += "\r\nE-mail: " + (String)reg.GetValue("author_email", "invalid@invalid.invalid");
			labelModAuthor.Text += "\r\nWebsite: " + (String)reg.GetValue("author_homepage", "http://invalid.invalid/");
			if (((Form)this.Owner).MdiChildren.GetLength(0) > 0)
			{
				if (((Form)this.Owner).ActiveMdiChild.GetType() == typeof(ModEditor))
				{
					CodeIndents DescriptionIndent;
					CodeIndents FilesToEditIndent;
					CodeIndents IncludedFilesIndent;
					CodeIndents AuthorNotesIndent;
					StartLine AuthorNotesStartLine;

					DescriptionIndent = ((ModEditor)((Form)this.Owner).ActiveMdiChild).ThisMod.DescriptionIndent;
					FilesToEditIndent = ((ModEditor)((Form)this.Owner).ActiveMdiChild).ThisMod.ModFilesToEditIndent;
					IncludedFilesIndent = ((ModEditor)((Form)this.Owner).ActiveMdiChild).ThisMod.ModIncludedFilesIndent;
					AuthorNotesIndent = ((ModEditor)((Form)this.Owner).ActiveMdiChild).ThisMod.AuthorNotesIndent;
					AuthorNotesStartLine = ((ModEditor)((Form)this.Owner).ActiveMdiChild).ThisMod.AuthorNotesStartLine;

					comboBoxDescriptionTabbing.SelectedIndex = (int)DescriptionIndent;
					comboBoxFilesToEditTabbing.SelectedIndex = (int)FilesToEditIndent;
					comboBoxIncludedFilesTabbing.SelectedIndex = (int)IncludedFilesIndent;
					comboBoxAuthorNotesTabbing.SelectedIndex = (int)AuthorNotesIndent;
					comboBoxAuthorNotesStartLine.SelectedIndex = (int)AuthorNotesStartLine;
				}
				else
				{
					setupDefaultIndents(reg);
				}
			}
			else
			{
				setupDefaultIndents(reg);
			}
			reg.Close();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reg"></param>
		private void setupDefaultIndents(RegistryKey reg)
		{
			CodeIndents DescriptionIndent;
			CodeIndents FilesToEditIndent;
			CodeIndents IncludedFilesIndent;
			CodeIndents AuthorNotesIndent;
			StartLine AuthorNotesStartLine;

			DescriptionIndent = (CodeIndents)((int)reg.GetValue("description_indent", CodeIndents.Space));
			FilesToEditIndent = (CodeIndents)((int)reg.GetValue("files-to-edit_indent", CodeIndents.RightAligned));
			IncludedFilesIndent = (CodeIndents)((int)reg.GetValue("included-files_indent", CodeIndents.RightAligned));
			AuthorNotesIndent = (CodeIndents)((int)reg.GetValue("authornotes_indent", CodeIndents.Space));
			AuthorNotesStartLine = (StartLine)((int)reg.GetValue("authornotes_startline", StartLine.Same));

			comboBoxDescriptionTabbing.SelectedIndex = (int)DescriptionIndent;
			comboBoxFilesToEditTabbing.SelectedIndex = (int)FilesToEditIndent;
			comboBoxIncludedFilesTabbing.SelectedIndex = (int)IncludedFilesIndent;
			comboBoxAuthorNotesTabbing.SelectedIndex = (int)AuthorNotesIndent;
			comboBoxAuthorNotesStartLine.SelectedIndex = (int)AuthorNotesStartLine;
		}

		/// <summary>
		/// 
		/// </summary>
		private void applyIndents()
		{
			((ModEditor)((Form)this.Owner).ActiveMdiChild).ThisMod.DescriptionIndent = (CodeIndents)comboBoxDescriptionTabbing.SelectedIndex;
			((ModEditor)((Form)this.Owner).ActiveMdiChild).ThisMod.ModFilesToEditIndent = (CodeIndents)comboBoxFilesToEditTabbing.SelectedIndex;
			((ModEditor)((Form)this.Owner).ActiveMdiChild).ThisMod.ModIncludedFilesIndent = (CodeIndents)comboBoxIncludedFilesTabbing.SelectedIndex;
			((ModEditor)((Form)this.Owner).ActiveMdiChild).ThisMod.AuthorNotesIndent = (CodeIndents)comboBoxAuthorNotesTabbing.SelectedIndex;
			((ModEditor)((Form)this.Owner).ActiveMdiChild).ThisMod.AuthorNotesStartLine = (StartLine)comboBoxAuthorNotesStartLine.SelectedIndex;
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.Close();
			this.Dispose();
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			RegistryKey reg = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("VB and VBA Program Settings").OpenSubKey("MODStudio").OpenSubKey("mod-settings", true);
			reg.SetValue("description_indent", comboBoxDescriptionTabbing.SelectedIndex);
			reg.SetValue("files-to-edit_indent", comboBoxFilesToEditTabbing.SelectedIndex);
			reg.SetValue("included-files_indent", comboBoxIncludedFilesTabbing.SelectedIndex);
			reg.SetValue("authornotes_indent", comboBoxAuthorNotesTabbing.SelectedIndex);
			reg.SetValue("authornotes_startline", comboBoxAuthorNotesStartLine.SelectedIndex);
			reg.Close();

			if (((Form)this.Owner).ActiveMdiChild.GetType() == typeof(ModEditor))
			{
				applyIndents();
			}

			this.Close();
			this.Dispose();
		}

		private void authorEditorDialog1_Save(object sender, ModStudio.AuthorEditorDialogBoxSaveEventArgs e)
		{
			RegistryKey reg = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("VB and VBA Program Settings").OpenSubKey("MODStudio").OpenSubKey("mod-settings", true);
			reg.SetValue("author_username", e.Entry.UserName);
			reg.SetValue("author_realname", e.Entry.RealName);
			reg.SetValue("author_email", e.Entry.Email);
			reg.SetValue("author_homepage", e.Entry.Homepage);
			reg.Close();
			OptionsDialog_Load(this, null);
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			RegistryKey reg = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("VB and VBA Program Settings").OpenSubKey("MODStudio").OpenSubKey("mod-settings");
			authorEditorDialog1.Entry = new ModAuthor(
				reg.GetValue("author_username", "UserName").ToString(),
				reg.GetValue("author_realname", "RealName").ToString(),
				reg.GetValue("author_email", "invalid@invalid.invalid").ToString(),
				reg.GetValue("author_homepage", "http://invalid.invalid/").ToString());
			reg.Close();
			authorEditorDialog1.ShowDialog(this);
		}
	}
}
