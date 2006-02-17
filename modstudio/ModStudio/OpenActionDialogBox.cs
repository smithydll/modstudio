/***************************************************************************
 *                           OpenActionDialogBox.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: OpenActionDialogBox.cs,v 1.12 2006-02-17 04:11:45 smithydll Exp $
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
using System.IO;

namespace ModStudio
{
	/// <summary>
	/// Summary description for OpenActionDialogBox.
	/// </summary>
	public class OpenActionDialogBox : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Button buttonCancel;
		internal System.Windows.Forms.TextBox textBoxFile;
		private System.Windows.Forms.ListBox listBoxFiles;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// 
		/// </summary>
		public OpenActionDialogBox()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			LoadPhpbbFileList();
			listBoxFiles.Items.Clear();
			for (int i = 0; i < PhpbbFileList.Length; i++)
			{
				listBoxFiles.Items.Add(PhpbbFileList[i]);
			}
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
			this.textBoxFile = new System.Windows.Forms.TextBox();
			this.listBoxFiles = new System.Windows.Forms.ListBox();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.buttonCancel);
			this.panel1.Controls.Add(this.buttonOk);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 158);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(456, 40);
			this.panel1.TabIndex = 0;
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonCancel.Location = new System.Drawing.Point(296, 8);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.TabIndex = 1;
			this.buttonCancel.Text = "&Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.button2_Click);
			// 
			// buttonOk
			// 
			this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOk.Location = new System.Drawing.Point(376, 8);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.TabIndex = 0;
			this.buttonOk.Text = "&OK";
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// textBoxFile
			// 
			this.textBoxFile.Dock = System.Windows.Forms.DockStyle.Top;
			this.textBoxFile.Location = new System.Drawing.Point(0, 0);
			this.textBoxFile.Name = "textBoxFile";
			this.textBoxFile.Size = new System.Drawing.Size(456, 20);
			this.textBoxFile.TabIndex = 1;
			this.textBoxFile.Text = "";
			this.textBoxFile.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxFile_KeyPress);
			this.textBoxFile.TextChanged += new System.EventHandler(this.textBoxFile_TextChanged);
			this.textBoxFile.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxFile_KeyUp);
			// 
			// listBoxFiles
			// 
			this.listBoxFiles.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBoxFiles.Location = new System.Drawing.Point(0, 20);
			this.listBoxFiles.Name = "listBoxFiles";
			this.listBoxFiles.Size = new System.Drawing.Size(456, 134);
			this.listBoxFiles.TabIndex = 2;
			this.listBoxFiles.DoubleClick += new System.EventHandler(this.listBoxFiles_DoubleClick);
			this.listBoxFiles.SelectedIndexChanged += new System.EventHandler(this.listBoxFiles_SelectedIndexChanged);
			// 
			// OpenActionDialogBox
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(456, 198);
			this.ControlBox = false;
			this.Controls.Add(this.listBoxFiles);
			this.Controls.Add(this.textBoxFile);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OpenActionDialogBox";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select a file to open";
			this.Load += new System.EventHandler(this.OpenActionDialogBox_Load);
			this.VisibleChanged += new System.EventHandler(this.OpenActionDialogBox_VisibleChanged);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private string[] PhpbbFileList;

		/// <summary>
		/// 
		/// </summary>
		private void LoadPhpbbFileList()
		{
			PhpbbFileList = OpenTextFile(Application.StartupPath + "\\files.txt").Replace("\r\n", "\n").Split('\n');
			//Console.WriteLine(":" + PhpbbFileList.Length);
		}

		/// <summary>
		/// 
		/// </summary>
		public delegate void OpenActionDialogBoxSaveNewHandler(object sender, OpenActionDialogBoxSaveNewEventArgs e);
		/// <summary>
		/// 
		/// </summary>
		//public event OpenActionDialogBoxSaveNewHandler SaveNew;

		private void button2_Click(object sender, System.EventArgs e)
		{
			this.Hide();
		}

		private void buttonOk_Click(object sender, System.EventArgs e)
		{
			//this.SaveNew(this, new OpenActionDialogBoxSaveNewEventArgs(textBoxFile.Text));
			//this.Hide();
		}

		bool enableTextChange = true;
		private void listBoxFiles_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string temp = textBoxFile.Text;
			enableTextChange = false;
			textBoxFile.Text = listBoxFiles.Text;
			if (listBoxFiles.Text.StartsWith(temp))
			{
				textBoxFile.Select(temp.Length, textBoxFile.Text.Length - temp.Length);
			}
		}

		/// <summary>
		/// Open a text file
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		private static string OpenTextFile(string filename)
		{
			StreamReader myStreamReader;
			string temp;
			try 
			{
				myStreamReader = File.OpenText(filename);
				temp = myStreamReader.ReadToEnd();
				myStreamReader.Close();
			} 
			catch
			{
				temp = "";
			}
			return temp;
		}

		private void OpenActionDialogBox_Load(object sender, System.EventArgs e)
		{
		}

		private void listBoxFiles_DoubleClick(object sender, System.EventArgs e)
		{
			buttonOk_Click(null, null);
		}

		private void textBoxFile_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			
		}

		private void textBoxFile_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == System.Windows.Forms.Keys.Enter)
			{
				//buttonOk_Click(null, null);
				
			}
		}

		private void textBoxFile_TextChanged(object sender, System.EventArgs e)
		{
			if (enableTextChange)
			{
				for (int i = 0; i < PhpbbFileList.Length; i++)
				{
					if (PhpbbFileList[i].StartsWith(textBoxFile.Text))
					{
						string temp = textBoxFile.Text;
						listBoxFiles.SelectedIndex = i;
						
						enableTextChange = false;
						textBoxFile.Text = listBoxFiles.Text;
						if (listBoxFiles.Text.StartsWith(temp))
						{
							textBoxFile.Select(temp.Length, textBoxFile.Text.Length - temp.Length);
						}
						break;
					}
				}
			}
			enableTextChange = true;
		}

		private void OpenActionDialogBox_VisibleChanged(object sender, System.EventArgs e)
		{
			textBoxFile.Select();
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class OpenActionDialogBoxSaveNewEventArgs: EventArgs 
	{
		/// <summary>
		/// 
		/// </summary>
		public string FileName;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		public OpenActionDialogBoxSaveNewEventArgs(string fileName) 
		{
			this.FileName = fileName;
		}
	}
}
