/***************************************************************************
 *                         HistoryEditorDialogBox.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: HistoryEditorDialogBox.cs,v 1.6 2006-01-16 06:11:57 smithydll Exp $
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
	/// Summary description for HistoryEditorDialog.
	/// </summary>
	public class HistoryEditorDialogBox : System.Windows.Forms.Form
	{
		const int NewChange = -1;

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Panel panel2;
		internal System.Windows.Forms.TextBox MODVersionRelease;
		internal System.Windows.Forms.NumericUpDown MODVersionMajor;
		internal System.Windows.Forms.NumericUpDown MODVersionMinor;
		internal System.Windows.Forms.NumericUpDown MODVersionRevision;
		internal System.Windows.Forms.DateTimePicker MODHistorydtp;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ListView listViewModChanges;
		private System.Windows.Forms.ComboBox comboBoxLanguages;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private ModStudio.NoteEditorDialog noteEditorDialog1;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button5;
		private ModFormControls.LanguageSelectionDialog languageSelectionDialog1;
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
			entry = new ModHistoryEntry();
			entry.HistoryChangeLog = new ModHistoryChangeLogLocalised();
		}

		private ModHistoryEntry entry;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entry"></param>
		public void SetEntry(ModHistoryEntry entry)
		{
			if (entry.HistoryChangeLog.Count == 0)
			{
				entry.HistoryChangeLog.Add(new ModTemplateTools.DataStructures.ModHistoryChangeLog(), "en-GB");
			}
			//TODO: 
			//textBoxEntry.Text = entry.HistoryChanges.GetValue().Replace("\r","").Replace("\n", "\r\n");
			MODVersionMajor.Value = entry.HistoryVersion.VersionMajor;
			MODVersionMinor.Value = entry.HistoryVersion.VersionMinor;
			MODVersionRevision.Value = entry.HistoryVersion.VersionRevision;
			MODVersionRelease.Text = entry.HistoryVersion.VersionRelease.ToString();
			MODHistorydtp.Value = entry.HistoryDate;

			comboBoxLanguages.Items.Clear();
			comboBoxLanguages.Text = "";
			foreach (DictionaryEntry de in entry.HistoryChangeLog)
			{
				comboBoxLanguages.Items.Add((string)de.Key);
				if (comboBoxLanguages.Items.Count == 1)
				{
					comboBoxLanguages.SelectedIndex = 0;
					listViewModChanges.Clear();
					foreach (string change in ((ModHistoryChangeLog)de.Value))
					{
						listViewModChanges.Items.Add(change.Split('\n')[0]);
					}
				}
			}
			this.entry = entry;
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
			this.panel3 = new System.Windows.Forms.Panel();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.comboBoxLanguages = new System.Windows.Forms.ComboBox();
			this.listViewModChanges = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.noteEditorDialog1 = new ModStudio.NoteEditorDialog();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.button5 = new System.Windows.Forms.Button();
			languageSelectionDialog1 = new ModFormControls.LanguageSelectionDialog();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.MODVersionMajor)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.MODVersionMinor)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.MODVersionRevision)).BeginInit();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.buttonCancel);
			this.panel1.Controls.Add(this.buttonOk);
			this.panel1.Controls.Add(this.button2);
			this.panel1.Controls.Add(this.button4);
			this.panel1.Controls.Add(this.button5);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 184);
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
			this.panel2.Size = new System.Drawing.Size(504, 32);
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
			// panel3
			// 
			this.panel3.Controls.Add(this.button1);
			this.panel3.Controls.Add(this.label1);
			this.panel3.Controls.Add(this.comboBoxLanguages);
			this.panel3.Controls.Add(this.button3);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new System.Drawing.Point(0, 32);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(504, 24);
			this.panel3.TabIndex = 4;
			// 
			// button2
			// 
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button2.Location = new System.Drawing.Point(8, 8);
			this.button2.Name = "button2";
			this.button2.TabIndex = 3;
			this.button2.Text = "Add Change";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button1
			// 
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(216, 0);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(88, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "Add Language";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 23);
			this.label1.TabIndex = 1;
			this.label1.Text = "Language:";
			// 
			// comboBoxLanguages
			// 
			this.comboBoxLanguages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxLanguages.Location = new System.Drawing.Point(88, 0);
			this.comboBoxLanguages.Name = "comboBoxLanguages";
			this.comboBoxLanguages.Size = new System.Drawing.Size(121, 21);
			this.comboBoxLanguages.TabIndex = 0;
			this.comboBoxLanguages.SelectedIndexChanged += new System.EventHandler(this.comboBoxLanguages_SelectedIndexChanged);
			// 
			// listViewModChanges
			// 
			this.listViewModChanges.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																								 this.columnHeader1});
			this.listViewModChanges.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewModChanges.FullRowSelect = true;
			this.listViewModChanges.GridLines = true;
			this.listViewModChanges.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listViewModChanges.Location = new System.Drawing.Point(0, 56);
			this.listViewModChanges.MultiSelect = false;
			this.listViewModChanges.Name = "listViewModChanges";
			this.listViewModChanges.Size = new System.Drawing.Size(504, 128);
			this.listViewModChanges.TabIndex = 5;
			this.listViewModChanges.View = System.Windows.Forms.View.List;
			this.listViewModChanges.DoubleClick += new System.EventHandler(this.listViewModChanges_DoubleClick);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Changes";
			// 
			// noteEditorDialog1
			// 
			this.noteEditorDialog1.Save += new ModStudio.NoteEditorDialogBox.NoteEditorDialogBoxSaveHandler(this.noteEditorDialog1_Save);
			// 
			// button3
			// 
			this.button3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button3.Location = new System.Drawing.Point(312, 0);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(104, 23);
			this.button3.TabIndex = 2;
			this.button3.Text = "Remove Language";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button4
			// 
			this.button4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button4.Location = new System.Drawing.Point(88, 8);
			this.button4.Name = "button4";
			this.button4.TabIndex = 3;
			this.button4.Text = "Edit Change";
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// button5
			// 
			this.button5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button5.Location = new System.Drawing.Point(168, 8);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(96, 23);
			this.button5.TabIndex = 3;
			this.button5.Text = "Remove Change";
			this.button5.Click += new System.EventHandler(this.button5_Click);
			//
			// languageSelectionDialog1
			//
			this.languageSelectionDialog1.Language = "en-GB";
			this.languageSelectionDialog1.Save +=new ModFormControls.LanguageSelectionDialogBox.LanguageSelectionDialogBoxSaveHandler(languageSelectionDialog1_Save);
			// 
			// HistoryEditorDialogBox
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(504, 224);
			this.ControlBox = false;
			this.Controls.Add(this.listViewModChanges);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "HistoryEditorDialogBox";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "History Entry";
			this.Load += new System.EventHandler(this.HistoryEditorDialogBox_Load);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.MODVersionMajor)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.MODVersionMinor)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.MODVersionRevision)).EndInit();
			this.panel3.ResumeLayout(false);
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
		public ModHistoryEntry HistoryEntry
		{
			get
			{
				entry.HistoryVersion.VersionMajor = (int)MODVersionMajor.Value;
				entry.HistoryVersion.VersionMinor = (int)MODVersionMinor.Value;
				entry.HistoryVersion.VersionRevision = (int)MODVersionRevision.Value;
				entry.HistoryDate = MODHistorydtp.Value;
				if (MODVersionRelease.Text.Length > 0)
				{
					entry.HistoryVersion.VersionRelease = MODVersionRelease.Text[0];
				}
				return entry;
			}
			set
			{
				SetEntry(value);
			}
		}


		private void buttonOk_Click(object sender, System.EventArgs e)
		{
			entry.HistoryVersion.VersionMajor = (int)MODVersionMajor.Value;
			entry.HistoryVersion.VersionMinor = (int)MODVersionMinor.Value;
			entry.HistoryVersion.VersionRevision = (int)MODVersionRevision.Value;
			entry.HistoryDate = MODHistorydtp.Value;
			if (MODVersionRelease.Text.Length > 0)
			{
				entry.HistoryVersion.VersionRelease = MODVersionRelease.Text[0];
				//this.Save(this, new HistoryEditorDialogBoxSaveEventArgs(new ModTemplateTools.PhpbbMod.ModHistoryEntry(new ModTemplateTools.PhpbbMod.ModVersion((int)MODVersionMajor.Value, (int)MODVersionMinor.Value, (int)MODVersionRevision.Value, MODVersionRelease.Text[0]), MODHistorydtp.Value, new ModTemplateTools.PhpbbMod.PropertyLang(textBoxEntry.Text))));
			}
			//this.Save(this, new HistoryEditorDialogBoxSaveEventArgs(entry));
			this.Hide();
		}

		private void HistoryEditorDialogBox_Load(object sender, System.EventArgs e)
		{
		
		}

		private void comboBoxLanguages_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				listViewModChanges.Items.Clear();
				foreach (string change in entry.HistoryChangeLog[comboBoxLanguages.Text])
				{
					listViewModChanges.Items.Add(change.Split('\n')[0]);
				}
			}
			catch
			{
			}
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			languageSelectionDialog1.ShowDialog(this);
		}

		/// <summary>
		/// 
		/// </summary>
		public delegate void HistoryEditorDialogBoxSaveHandler(object sender, HistoryEditorDialogBoxSaveEventArgs e);
		/// <summary>
		/// 
		/// </summary>
		//public event HistoryEditorDialogBoxSaveHandler Save;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void noteEditorDialog1_Save(object sender, NoteEditorDialogBoxSaveEventArgs e)
		{
			int index = int.Parse(e.Type);
			if (index == NewChange)
			{
				entry.HistoryChangeLog[comboBoxLanguages.Text].Add(e.Note.GetValue());
			}
			else
			{
				entry.HistoryChangeLog[comboBoxLanguages.Text][index] = e.Note.GetValue();
			}
			comboBoxLanguages_SelectedIndexChanged(null, null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button2_Click(object sender, System.EventArgs e)
		{
			noteEditorDialog1.Note = new StringLocalised("");
			noteEditorDialog1.Localised = false;
			noteEditorDialog1.Type = NewChange.ToString();
			noteEditorDialog1.ShowDialog(this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listViewModChanges_DoubleClick(object sender, System.EventArgs e)
		{
			try
			{
				if (entry.HistoryChangeLog[comboBoxLanguages.Text].Count > 0)
				{
					if (listViewModChanges.SelectedIndices.Count > 0)
					{
						noteEditorDialog1.Note = new StringLocalised(entry.HistoryChangeLog[comboBoxLanguages.Text][listViewModChanges.SelectedIndices[0]]);
						noteEditorDialog1.Localised = false;
						noteEditorDialog1.Type = listViewModChanges.SelectedIndices[0].ToString();
						noteEditorDialog1.ShowDialog(this);
					}
				}
			}
			catch
			{
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button3_Click(object sender, System.EventArgs e)
		{
			if (entry.HistoryChangeLog.Count == 1)
			{
				MessageBox.Show(this, "There must always be at least one language", "Can't remove language", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
			}
			else
			{
				entry.HistoryChangeLog.Remove(comboBoxLanguages.Text);
				comboBoxLanguages.Items.Remove(comboBoxLanguages.SelectedItem);
				comboBoxLanguages.SelectedIndex = 0;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button4_Click(object sender, System.EventArgs e)
		{
			listViewModChanges_DoubleClick(null, null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button5_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (entry.HistoryChangeLog[comboBoxLanguages.Text].Count > 0)
				{
					if (listViewModChanges.SelectedIndices.Count > 0)
					{
						entry.HistoryChangeLog[comboBoxLanguages.Text].RemoveAt(listViewModChanges.SelectedIndices[0]);
					}
				}
			}
			catch
			{
			}
			comboBoxLanguages_SelectedIndexChanged(null, null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void languageSelectionDialog1_Save(object sender, ModFormControls.LanguageSelectionDialogBoxSaveEventArgs e)
		{
			try
			{
				entry.HistoryChangeLog.Add(new ModTemplateTools.DataStructures.ModHistoryChangeLog(), e.Language);
				comboBoxLanguages.Items.Add(e.Language);
				comboBoxLanguages.SelectedIndex = comboBoxLanguages.Items.Count - 1;
			}
			catch (ArgumentException ex)
			{
				MessageBox.Show(this, "You already have an entry for this language", "Can't add language", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
				Console.WriteLine(ex.ToString());
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool LanguageSelectorVisible
		{
			get
			{
				return panel3.Visible;
			}
			set
			{
				panel3.Visible = value;
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class HistoryEditorDialogBoxSaveEventArgs: EventArgs 
	{
		/// <summary>
		/// 
		/// </summary>
		public ModHistoryEntry Entry;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="entry"></param>
		public HistoryEditorDialogBoxSaveEventArgs(ModHistoryEntry entry) 
		{
			this.Entry = entry;
		}
	}
}
