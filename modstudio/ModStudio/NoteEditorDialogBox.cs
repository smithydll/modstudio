/***************************************************************************
 *                           NoteEditorDialogBox.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: NoteEditorDialogBox.cs,v 1.2 2005-08-28 02:59:59 smithydll Exp $
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
	/// Summary description for NoteEditorDialog.
	/// </summary>
	public class NoteEditorDialogBox : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.TextBox textBoxNote;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// 
		/// </summary>
		public NoteEditorDialogBox()
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
			this.textBoxNote = new System.Windows.Forms.TextBox();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.buttonCancel);
			this.panel1.Controls.Add(this.buttonOk);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 214);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(504, 40);
			this.panel1.TabIndex = 0;
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonCancel.Location = new System.Drawing.Point(344, 8);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "&Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonOk
			// 
			this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonOk.Location = new System.Drawing.Point(424, 8);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.TabIndex = 2;
			this.buttonOk.Text = "&OK";
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// textBoxNote
			// 
			this.textBoxNote.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxNote.Location = new System.Drawing.Point(0, 0);
			this.textBoxNote.Multiline = true;
			this.textBoxNote.Name = "textBoxNote";
			this.textBoxNote.Size = new System.Drawing.Size(504, 214);
			this.textBoxNote.TabIndex = 1;
			this.textBoxNote.Text = "";
			// 
			// NoteEditorDialogBox
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(504, 254);
			this.ControlBox = false;
			this.Controls.Add(this.textBoxNote);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NoteEditorDialogBox";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Note Editor";
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// 
		/// </summary>
		public delegate void NoteEditorDialogBoxSaveHandler(object sender, NoteEditorDialogBoxSaveEventArgs e);
		/// <summary>
		/// 
		/// </summary>
		//public event NoteEditorDialogBoxSaveHandler Save;

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			this.Hide();
		}

		private void buttonOk_Click(object sender, System.EventArgs e)
		{
			/*this.Save(this, new NoteEditorDialogBoxSaveEventArgs(textBoxNote.Text, notetype));
			this.Hide();*/
		}
		
		private string notetype = "";

		/// <summary>
		/// 
		/// </summary>
		public string Note
		{
			get
			{
				return textBoxNote.Text.Replace("\r","");
			}
			set
			{
				textBoxNote.Text = value.Replace("\r","").Replace("\n","\r\n");
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="note"></param>
		/// <param name="type"></param>
		public void SetNote(string note, string type)
		{
			this.textBoxNote.Text = note;
			notetype = type;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class NoteEditorDialogBoxSaveEventArgs: EventArgs 
	{
		/// <summary>
		/// 
		/// </summary>
		public string Note;
		/// <summary>
		/// 
		/// </summary>
		public string Type;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="note"></param>
		/// <param name="type"></param>
		public NoteEditorDialogBoxSaveEventArgs(string note, string type)
		{
			this.Note = note;
			this.Type = type;
		}
	}
}
