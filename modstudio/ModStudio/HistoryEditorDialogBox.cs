/***************************************************************************
 *                         HistoryEditorDialogBox.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: HistoryEditorDialogBox.cs,v 1.3 2005-09-02 14:12:48 smithydll Exp $
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

namespace ModStudio
{
	/// <summary>
	/// Summary description for HistoryEditorDialog.
	/// </summary>
	public class HistoryEditorDialogBox : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.TextBox textBoxEntry;
		internal System.Windows.Forms.TextBox MODVersionRelease;
		internal System.Windows.Forms.NumericUpDown MODVersionMajor;
		internal System.Windows.Forms.NumericUpDown MODVersionMinor;
		internal System.Windows.Forms.NumericUpDown MODVersionRevision;
		internal System.Windows.Forms.DateTimePicker MODHistorydtp;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// 
		/// </summary>
		public HistoryEditorDialogBox()
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
		/// 
		/// </summary>
		/// <param name="entry"></param>
		public void SetEntry(ModTemplateTools.PhpbbMod.ModHistoryEntry entry)
		{
			textBoxEntry.Text = entry.HistoryChanges.GetValue().Replace("\r","").Replace("\n", "\r\n");
			MODVersionMajor.Value = entry.HistoryVersion.VersionMajor;
			MODVersionMinor.Value = entry.HistoryVersion.VersionMinor;
			MODVersionRevision.Value = entry.HistoryVersion.VersionRevision;
			MODVersionRelease.Text = entry.HistoryVersion.VersionRelease.ToString();
			MODHistorydtp.Value = entry.HistoryDate;
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
			this.panel2 = new System.Windows.Forms.Panel();
			this.MODVersionRelease = new System.Windows.Forms.TextBox();
			this.MODVersionMajor = new System.Windows.Forms.NumericUpDown();
			this.MODVersionMinor = new System.Windows.Forms.NumericUpDown();
			this.MODVersionRevision = new System.Windows.Forms.NumericUpDown();
			this.MODHistorydtp = new System.Windows.Forms.DateTimePicker();
			this.textBoxEntry = new System.Windows.Forms.TextBox();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.MODVersionMajor)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.MODVersionMinor)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.MODVersionRevision)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.buttonCancel);
			this.panel1.Controls.Add(this.buttonOk);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 152);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(504, 40);
			this.panel1.TabIndex = 2;
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
			// panel2
			// 
			this.panel2.Controls.Add(this.MODVersionRelease);
			this.panel2.Controls.Add(this.MODVersionMajor);
			this.panel2.Controls.Add(this.MODVersionMinor);
			this.panel2.Controls.Add(this.MODVersionRevision);
			this.panel2.Controls.Add(this.MODHistorydtp);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(504, 40);
			this.panel2.TabIndex = 3;
			// 
			// MODVersionRelease
			// 
			this.MODVersionRelease.Location = new System.Drawing.Point(368, 8);
			this.MODVersionRelease.MaxLength = 1;
			this.MODVersionRelease.Name = "MODVersionRelease";
			this.MODVersionRelease.Size = new System.Drawing.Size(40, 20);
			this.MODVersionRelease.TabIndex = 12;
			this.MODVersionRelease.Text = "";
			// 
			// MODVersionMajor
			// 
			this.MODVersionMajor.Location = new System.Drawing.Point(208, 8);
			this.MODVersionMajor.Maximum = new System.Decimal(new int[] {
																			99,
																			0,
																			0,
																			0});
			this.MODVersionMajor.Name = "MODVersionMajor";
			this.MODVersionMajor.Size = new System.Drawing.Size(40, 20);
			this.MODVersionMajor.TabIndex = 11;
			// 
			// MODVersionMinor
			// 
			this.MODVersionMinor.Location = new System.Drawing.Point(256, 8);
			this.MODVersionMinor.Maximum = new System.Decimal(new int[] {
																			99,
																			0,
																			0,
																			0});
			this.MODVersionMinor.Name = "MODVersionMinor";
			this.MODVersionMinor.Size = new System.Drawing.Size(40, 20);
			this.MODVersionMinor.TabIndex = 10;
			// 
			// MODVersionRevision
			// 
			this.MODVersionRevision.Location = new System.Drawing.Point(304, 8);
			this.MODVersionRevision.Maximum = new System.Decimal(new int[] {
																			   9999,
																			   0,
																			   0,
																			   0});
			this.MODVersionRevision.Name = "MODVersionRevision";
			this.MODVersionRevision.Size = new System.Drawing.Size(56, 20);
			this.MODVersionRevision.TabIndex = 9;
			// 
			// MODHistorydtp
			// 
			this.MODHistorydtp.Location = new System.Drawing.Point(8, 8);
			this.MODHistorydtp.Name = "MODHistorydtp";
			this.MODHistorydtp.TabIndex = 8;
			// 
			// textBoxEntry
			// 
			this.textBoxEntry.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxEntry.Location = new System.Drawing.Point(0, 40);
			this.textBoxEntry.Multiline = true;
			this.textBoxEntry.Name = "textBoxEntry";
			this.textBoxEntry.Size = new System.Drawing.Size(504, 112);
			this.textBoxEntry.TabIndex = 4;
			this.textBoxEntry.Text = "";
			// 
			// HistoryEditorDialogBox
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(504, 192);
			this.ControlBox = false;
			this.Controls.Add(this.textBoxEntry);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "HistoryEditorDialogBox";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "History Entry";
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.MODVersionMajor)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.MODVersionMinor)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.MODVersionRevision)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			this.Hide();
		}

		/// <summary>
		/// 
		/// </summary>
		public ModTemplateTools.PhpbbMod.ModHistoryEntry HistoryEntry
		{
			get
			{
				return new ModTemplateTools.PhpbbMod.ModHistoryEntry(new ModTemplateTools.PhpbbMod.ModVersion((int)MODVersionMajor.Value, (int)MODVersionMinor.Value, (int)MODVersionRevision.Value, MODVersionRelease.Text[0]), MODHistorydtp.Value, new ModTemplateTools.PhpbbMod.PropertyLang(textBoxEntry.Text.Replace("\r", "")));
			}
			set
			{
				SetEntry(value);
			}
		}


		private void buttonOk_Click(object sender, System.EventArgs e)
		{
			/*if (MODVersionRelease.Text.Length > 0)
			{
				this.Save(this, new HistoryEditorDialogBoxSaveEventArgs(new ModTemplateTools.PhpbbMod.ModHistoryEntry(new ModTemplateTools.PhpbbMod.ModVersion((int)MODVersionMajor.Value, (int)MODVersionMinor.Value, (int)MODVersionRevision.Value, MODVersionRelease.Text[0]), MODHistorydtp.Value, new ModTemplateTools.PhpbbMod.PropertyLang(textBoxEntry.Text))));
			}
			else
			{
				this.Save(this, new HistoryEditorDialogBoxSaveEventArgs(new ModTemplateTools.PhpbbMod.ModHistoryEntry(new ModTemplateTools.PhpbbMod.ModVersion((int)MODVersionMajor.Value, (int)MODVersionMinor.Value, (int)MODVersionRevision.Value), MODHistorydtp.Value, new ModTemplateTools.PhpbbMod.PropertyLang(textBoxEntry.Text))));
			}
			this.Hide();*/
		}

		/// <summary>
		/// 
		/// </summary>
		public delegate void HistoryEditorDialogBoxSaveHandler(object sender, HistoryEditorDialogBoxSaveEventArgs e);
		/// <summary>
		/// 
		/// </summary>
		//public event HistoryEditorDialogBoxSaveHandler Save;


	}

	/// <summary>
	/// 
	/// </summary>
	public class HistoryEditorDialogBoxSaveEventArgs: EventArgs 
	{
		/// <summary>
		/// 
		/// </summary>
		public ModTemplateTools.PhpbbMod.ModHistoryEntry Entry;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="entry"></param>
		public HistoryEditorDialogBoxSaveEventArgs(ModTemplateTools.PhpbbMod.ModHistoryEntry entry) 
		{
			this.Entry = entry;
		}
	}
}
