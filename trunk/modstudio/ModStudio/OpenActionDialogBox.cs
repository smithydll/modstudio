/***************************************************************************
 *                           OpenActionDialogBox.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: OpenActionDialogBox.cs,v 1.1 2005-08-21 02:48:05 smithydll Exp $
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
		private System.Windows.Forms.TextBox textBoxFile;
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
			this.buttonOk = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
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
			// buttonOk
			// 
			this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonOk.Location = new System.Drawing.Point(376, 8);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.TabIndex = 0;
			this.buttonOk.Text = "&OK";
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonCancel.Location = new System.Drawing.Point(296, 8);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.TabIndex = 1;
			this.buttonCancel.Text = "&Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.button2_Click);
			// 
			// textBoxFile
			// 
			this.textBoxFile.Dock = System.Windows.Forms.DockStyle.Top;
			this.textBoxFile.Location = new System.Drawing.Point(0, 0);
			this.textBoxFile.Name = "textBoxFile";
			this.textBoxFile.Size = new System.Drawing.Size(456, 20);
			this.textBoxFile.TabIndex = 1;
			this.textBoxFile.Text = "";
			// 
			// listBoxFiles
			// 
			this.listBoxFiles.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBoxFiles.Location = new System.Drawing.Point(0, 20);
			this.listBoxFiles.Name = "listBoxFiles";
			this.listBoxFiles.Size = new System.Drawing.Size(456, 134);
			this.listBoxFiles.TabIndex = 2;
			this.listBoxFiles.SelectedIndexChanged += new System.EventHandler(this.listBoxFiles_SelectedIndexChanged);
			// 
			// OpenActionDialogBox
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(456, 198);
			this.ControlBox = false;
			this.Controls.Add(this.listBoxFiles);
			this.Controls.Add(this.textBoxFile);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OpenActionDialogBox";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select a file to open";
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// 
		/// </summary>
		public delegate void OpenActionDialogBoxSaveNewHandler(object sender, OpenActionDialogBoxSaveNewEventArgs e);
		/// <summary>
		/// 
		/// </summary>
		public event OpenActionDialogBoxSaveNewHandler SaveNew;

		private void button2_Click(object sender, System.EventArgs e)
		{
			this.Hide();
		}

		private void buttonOk_Click(object sender, System.EventArgs e)
		{
			this.SaveNew(this, new OpenActionDialogBoxSaveNewEventArgs(textBoxFile.Text));
		}

		private void listBoxFiles_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			textBoxFile.Text = listBoxFiles.SelectedValue.ToString();
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
