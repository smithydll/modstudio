/***************************************************************************
 *                              ModEditor.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: ModEditor.cs,v 1.20 2007-09-01 13:52:37 smithydll Exp $
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
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using ModStudio;
using Phpbb.ModTeam.Tools;
using Phpbb.ModTeam.Tools.DataStructures;
using ModFormControls;

namespace ModStudio
{
	/// <summary>
	/// Summary description for ModEditor.
	/// </summary>
	public class ModEditor : System.Windows.Forms.Form, IEditor
	{
		private System.Windows.Forms.TabControl tabControlEditor;
		private System.Windows.Forms.TabPage tabPageOverview;
		private System.Windows.Forms.TabPage tabPageHeader;
		private System.Windows.Forms.TabPage tabPageActions;
		private System.Windows.Forms.Label labelOverviewTitle;
		private ModFormControls.ModDisplayBox modDisplayBox1;
		private System.Windows.Forms.ListView MODHistoryListView;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ListBox MODAuthorListBox;
		private System.Windows.Forms.ComboBox MODInstallationLevelComboBox;
		private System.Windows.Forms.DomainUpDown MODVersionReleaseDomainUpDown;
		private System.Windows.Forms.NumericUpDown MODVersionMajorNumericUpDown;
		internal System.Windows.Forms.Button Button16;
		private System.Windows.Forms.Label MODDescriptionLabel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown MODVersionMinorNumericUpDown;
		private System.Windows.Forms.NumericUpDown MODVersionRevisionNumericUpDown;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Label MODAuthorNotesLabel;
		internal System.Windows.Forms.Button button4;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label MODInstallationTimeLabel;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Button button7;
		internal Mod ThisMod;
		private ModFormControls.ModDisplayBox modDisplayBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label labelModInstallationTime;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button button8;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.ListBox listBoxFileEdits;
		private System.Windows.Forms.Button button9;
		private System.Windows.Forms.Label labelIncludedFiles;
		private System.Windows.Forms.ToolBar toolBar1;
		private System.Windows.Forms.ToolBarButton toolBarButtonEditAction;
		private System.Windows.Forms.ToolBarButton toolBarButtonAddAction;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private OpenActionDialog openActionDialog1;
		private AuthorEditorDialog authorEditorDialog1;
		private HistoryEditorDialog historyEditorDialog1;
		private NoteEditorDialog noteEditorDialog1;
		private ModFormControls.ModActionEditor modActionEditor1;
		private System.Windows.Forms.ContextMenu contextMenuAddAction;
		private System.Windows.Forms.MenuItem menuItemAddActionSql;
		private System.Windows.Forms.MenuItem menuItemAddActionCopy;
		private System.Windows.Forms.MenuItem menuItemAddActionOpen;
		private System.Windows.Forms.MenuItem menuItemAddActionFind;
		private System.Windows.Forms.MenuItem menuItemAddActionAfterAdd;
		private System.Windows.Forms.MenuItem menuItemAddActionBeforeAdd;
		private System.Windows.Forms.MenuItem menuItemAddActionReplaceWith;
		private System.Windows.Forms.MenuItem menuItemAddActionInLineFind;
		private System.Windows.Forms.MenuItem menuItemAddActionInLineAfterAdd;
		private System.Windows.Forms.MenuItem menuItemAddActionInLineBeforeAdd;
		private System.Windows.Forms.MenuItem menuItemAddActionInLineReplaceWith;
		private System.Windows.Forms.MenuItem menuItemAddActionIncrement;
		private System.Windows.Forms.MenuItem menuItemAddActionInLineIncrement;
		private System.Windows.Forms.MenuItem menuItemAddActionDiyInstruction;
		private System.Windows.Forms.ToolBarButton toolBarButtonDelete;
		private ModFormControls.LocalisedTextBox MODTitleTextBox;
		private System.Windows.Forms.PictureBox pictureBoxTextMod;
		private System.Windows.Forms.PictureBox pictureBoxXmlMod;
		private System.Windows.Forms.ImageList imageListModEditor;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label labelLicense;
		internal System.Windows.Forms.Button button10;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label labelPhpbbVersion;
        private TargetVersionDialog targetVersionDialog1;
        private TabPage tabPage1;
        private ComboBox ModVersionStageComboBox;
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// 
		/// </summary>
		public ModEditor() : this(false)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="isModx"></param>
		public ModEditor(bool isModx)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			((TextBox)this.MODVersionMajorNumericUpDown.Controls[1]).TextChanged += new System.EventHandler(this.MODTitleTextBox_TextChanged);
			((TextBox)this.MODVersionMajorNumericUpDown.Controls[1]).Click += new System.EventHandler(this.MODVersionMajorNumericUpDown_Click);
			((TextBox)this.MODVersionMajorNumericUpDown.Controls[1]).KeyUp += new KeyEventHandler(ModEditor_KeyUp);
			((TextBox)this.MODVersionMajorNumericUpDown.Controls[1]).KeyPress += new KeyPressEventHandler(ModEditor_KeyPress);

			((TextBox)this.MODVersionMinorNumericUpDown.Controls[1]).TextChanged += new System.EventHandler(this.MODTitleTextBox_TextChanged);
			((TextBox)this.MODVersionMinorNumericUpDown.Controls[1]).Click += new System.EventHandler(this.MODVersionMajorNumericUpDown_Click);
			((TextBox)this.MODVersionMinorNumericUpDown.Controls[1]).KeyUp += new KeyEventHandler(ModEditor_KeyUp);
			((TextBox)this.MODVersionMinorNumericUpDown.Controls[1]).KeyPress += new KeyPressEventHandler(ModEditor_KeyPress);

			((TextBox)this.MODVersionRevisionNumericUpDown.Controls[1]).TextChanged += new System.EventHandler(this.MODTitleTextBox_TextChanged);
			((TextBox)this.MODVersionRevisionNumericUpDown.Controls[1]).Click += new System.EventHandler(this.MODVersionMajorNumericUpDown_Click);
			((TextBox)this.MODVersionRevisionNumericUpDown.Controls[1]).KeyUp += new KeyEventHandler(ModEditor_KeyUp);
			((TextBox)this.MODVersionRevisionNumericUpDown.Controls[1]).KeyPress += new KeyPressEventHandler(ModEditor_KeyPress);

			((TextBox)this.MODVersionReleaseDomainUpDown.Controls[1]).TextChanged += new System.EventHandler(this.MODTitleTextBox_TextChanged);
			((TextBox)this.MODVersionReleaseDomainUpDown.Controls[1]).Click += new System.EventHandler(this.MODVersionMajorNumericUpDown_Click);
			((TextBox)this.MODVersionReleaseDomainUpDown.Controls[1]).KeyUp += new KeyEventHandler(ModEditor_KeyUp);
			((TextBox)this.MODVersionReleaseDomainUpDown.Controls[1]).KeyPress += new KeyPressEventHandler(ModEditor_KeyPress);

			this.KeyPreview = false;

			modDisplayBox2.CreateControl();
			Console.WriteLine(modDisplayBox2.CanFocus);

			if (isModx)
			{
				ThisMod = new ModxMod();
			}
			else
			{
				ThisMod = new TextMod(Application.StartupPath);
			}

			RegistryKey reg = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("VB and VBA Program Settings").OpenSubKey("MODStudio").OpenSubKey("mod-settings");
			ThisMod.Header.Authors.Add(new ModAuthor(
				reg.GetValue("author_username", "UserName").ToString(),
				reg.GetValue("author_realname", "RealName").ToString(),
				reg.GetValue("author_email", "invalid@invalid.invalid").ToString(),
				reg.GetValue("author_homepage", "http://invalid.invalid/").ToString()));
			ThisMod.DescriptionIndent = (CodeIndents)reg.GetValue("description_indent", ThisMod.DescriptionIndent);
			ThisMod.ModFilesToEditIndent = (CodeIndents)reg.GetValue("files-to-edit_indent", ThisMod.ModFilesToEditIndent);
			ThisMod.ModIncludedFilesIndent = (CodeIndents)reg.GetValue("included-files_indent", ThisMod.ModIncludedFilesIndent);
			ThisMod.AuthorNotesIndent = (CodeIndents)reg.GetValue("authornotes_indent", ThisMod.AuthorNotesIndent);
			ThisMod.AuthorNotesStartLine = (StartLine)reg.GetValue("authornotes_startline", ThisMod.AuthorNotesStartLine);
			//ThisMod.Header.ModAuthor.AddEntry(new PhpbbMod.ModAuthorEntry("UserName", "RealName", "invalid@invalid.invalid", "http://invalid.invalid/"));
			ThisMod.Header.Version = new ModVersion(0,0,0);
			ThisMod.Actions.Add(new ModAction("SAVE/CLOSE ALL FILES", "", "EoM", ""));
			//ThisMod.lastReadFormat = PhpbbMod.ModFormats.TextMOD;
			ThisMod.Header.License = "http://opensource.org/licenses/gpl-license.php GNU General Public License v2";
            if (isModx)
            {
                ThisMod.Header.PhpbbVersion = new TargetVersionCases(new ModVersion(3, 0, 0));
            }
            else
            {
                ThisMod.Header.PhpbbVersion = new TargetVersionCases(new ModVersion(2, 0, 0));
            }
			reg.Close();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filename"></param>
		public ModEditor(string filename):this()
		{
			// TODO: open MODX
			FileInfo fi = new FileInfo(filename);
			switch (fi.Extension.ToLower())
			{
				case ".mod":
				case ".txt":
					ThisMod = new TextMod(Application.StartupPath);
					ThisMod.Read(filename);
					break;
				case ".xml":
				case ".modx":
					ThisMod = new ModxMod();
					ThisMod.Read(filename);
					break;
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModEditor));
            Phpbb.ModTeam.Tools.DataStructures.ModAuthor modAuthor2 = new Phpbb.ModTeam.Tools.DataStructures.ModAuthor();
            Phpbb.ModTeam.Tools.DataStructures.ModHistoryEntry modHistoryEntry2 = new Phpbb.ModTeam.Tools.DataStructures.ModHistoryEntry();
            this.tabControlEditor = new System.Windows.Forms.TabControl();
            this.tabPageOverview = new System.Windows.Forms.TabPage();
            this.label13 = new System.Windows.Forms.Label();
            this.labelPhpbbVersion = new System.Windows.Forms.Label();
            this.button10 = new System.Windows.Forms.Button();
            this.pictureBoxXmlMod = new System.Windows.Forms.PictureBox();
            this.button9 = new System.Windows.Forms.Button();
            this.listBoxFileEdits = new System.Windows.Forms.ListBox();
            this.button8 = new System.Windows.Forms.Button();
            this.labelModInstallationTime = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBoxTextMod = new System.Windows.Forms.PictureBox();
            this.labelOverviewTitle = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelIncludedFiles = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.labelLicense = new System.Windows.Forms.Label();
            this.tabPageHeader = new System.Windows.Forms.TabPage();
            this.ModVersionStageComboBox = new System.Windows.Forms.ComboBox();
            this.MODTitleTextBox = new ModFormControls.LocalisedTextBox();
            this.MODHistoryListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.button1 = new System.Windows.Forms.Button();
            this.MODAuthorListBox = new System.Windows.Forms.ListBox();
            this.MODInstallationLevelComboBox = new System.Windows.Forms.ComboBox();
            this.MODVersionReleaseDomainUpDown = new System.Windows.Forms.DomainUpDown();
            this.MODVersionMajorNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.Button16 = new System.Windows.Forms.Button();
            this.MODDescriptionLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.MODVersionMinorNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.MODVersionRevisionNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.MODAuthorNotesLabel = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.MODInstallationTimeLabel = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.tabPageActions = new System.Windows.Forms.TabPage();
            this.modDisplayBox2 = new ModFormControls.ModDisplayBox();
            this.toolBar1 = new System.Windows.Forms.ToolBar();
            this.toolBarButtonEditAction = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonAddAction = new System.Windows.Forms.ToolBarButton();
            this.contextMenuAddAction = new System.Windows.Forms.ContextMenu();
            this.menuItemAddActionSql = new System.Windows.Forms.MenuItem();
            this.menuItemAddActionCopy = new System.Windows.Forms.MenuItem();
            this.menuItemAddActionOpen = new System.Windows.Forms.MenuItem();
            this.menuItemAddActionFind = new System.Windows.Forms.MenuItem();
            this.menuItemAddActionAfterAdd = new System.Windows.Forms.MenuItem();
            this.menuItemAddActionBeforeAdd = new System.Windows.Forms.MenuItem();
            this.menuItemAddActionReplaceWith = new System.Windows.Forms.MenuItem();
            this.menuItemAddActionInLineFind = new System.Windows.Forms.MenuItem();
            this.menuItemAddActionInLineAfterAdd = new System.Windows.Forms.MenuItem();
            this.menuItemAddActionInLineBeforeAdd = new System.Windows.Forms.MenuItem();
            this.menuItemAddActionInLineReplaceWith = new System.Windows.Forms.MenuItem();
            this.menuItemAddActionIncrement = new System.Windows.Forms.MenuItem();
            this.menuItemAddActionInLineIncrement = new System.Windows.Forms.MenuItem();
            this.menuItemAddActionDiyInstruction = new System.Windows.Forms.MenuItem();
            this.toolBarButtonDelete = new System.Windows.Forms.ToolBarButton();
            this.imageListModEditor = new System.Windows.Forms.ImageList(this.components);
            this.modActionEditor1 = new ModFormControls.ModActionEditor();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openActionDialog1 = new ModStudio.OpenActionDialog();
            this.authorEditorDialog1 = new ModStudio.AuthorEditorDialog();
            this.historyEditorDialog1 = new ModStudio.HistoryEditorDialog();
            this.noteEditorDialog1 = new ModStudio.NoteEditorDialog();
            this.targetVersionDialog1 = new ModStudio.TargetVersionDialog();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabControlEditor.SuspendLayout();
            this.tabPageOverview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxXmlMod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTextMod)).BeginInit();
            this.tabPageHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MODVersionMajorNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MODVersionMinorNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MODVersionRevisionNumericUpDown)).BeginInit();
            this.tabPageActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlEditor
            // 
            this.tabControlEditor.Controls.Add(this.tabPageOverview);
            this.tabControlEditor.Controls.Add(this.tabPageHeader);
            this.tabControlEditor.Controls.Add(this.tabPageActions);
            this.tabControlEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlEditor.Location = new System.Drawing.Point(0, 0);
            this.tabControlEditor.Name = "tabControlEditor";
            this.tabControlEditor.SelectedIndex = 0;
            this.tabControlEditor.Size = new System.Drawing.Size(792, 566);
            this.tabControlEditor.TabIndex = 0;
            this.tabControlEditor.TabIndexChanged += new System.EventHandler(this.tabControlEditor_TabIndexChanged);
            this.tabControlEditor.SelectedIndexChanged += new System.EventHandler(this.tabControlEditor_SelectedIndexChanged);
            // 
            // tabPageOverview
            // 
            this.tabPageOverview.BackColor = System.Drawing.SystemColors.Window;
            this.tabPageOverview.Controls.Add(this.label13);
            this.tabPageOverview.Controls.Add(this.labelPhpbbVersion);
            this.tabPageOverview.Controls.Add(this.button10);
            this.tabPageOverview.Controls.Add(this.pictureBoxXmlMod);
            this.tabPageOverview.Controls.Add(this.button9);
            this.tabPageOverview.Controls.Add(this.listBoxFileEdits);
            this.tabPageOverview.Controls.Add(this.button8);
            this.tabPageOverview.Controls.Add(this.labelModInstallationTime);
            this.tabPageOverview.Controls.Add(this.label1);
            this.tabPageOverview.Controls.Add(this.pictureBoxTextMod);
            this.tabPageOverview.Controls.Add(this.labelOverviewTitle);
            this.tabPageOverview.Controls.Add(this.label4);
            this.tabPageOverview.Controls.Add(this.labelIncludedFiles);
            this.tabPageOverview.Controls.Add(this.label10);
            this.tabPageOverview.Controls.Add(this.label8);
            this.tabPageOverview.Controls.Add(this.labelLicense);
            this.tabPageOverview.Location = new System.Drawing.Point(4, 22);
            this.tabPageOverview.Name = "tabPageOverview";
            this.tabPageOverview.Size = new System.Drawing.Size(784, 540);
            this.tabPageOverview.TabIndex = 0;
            this.tabPageOverview.Text = "Overview";
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(16, 328);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(120, 23);
            this.label13.TabIndex = 27;
            this.label13.Text = "phpBB Version";
            this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelPhpbbVersion
            // 
            this.labelPhpbbVersion.Location = new System.Drawing.Point(144, 328);
            this.labelPhpbbVersion.Name = "labelPhpbbVersion";
            this.labelPhpbbVersion.Size = new System.Drawing.Size(72, 23);
            this.labelPhpbbVersion.TabIndex = 28;
            this.labelPhpbbVersion.Text = "3.0.0";
            // 
            // button10
            // 
            this.button10.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button10.Location = new System.Drawing.Point(224, 328);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(24, 16);
            this.button10.TabIndex = 26;
            this.button10.Text = "...";
            this.button10.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // pictureBoxXmlMod
            // 
            this.pictureBoxXmlMod.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxXmlMod.Image")));
            this.pictureBoxXmlMod.Location = new System.Drawing.Point(8, 48);
            this.pictureBoxXmlMod.Name = "pictureBoxXmlMod";
            this.pictureBoxXmlMod.Size = new System.Drawing.Size(48, 16);
            this.pictureBoxXmlMod.TabIndex = 7;
            this.pictureBoxXmlMod.TabStop = false;
            // 
            // button9
            // 
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button9.Location = new System.Drawing.Point(56, 208);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(75, 23);
            this.button9.TabIndex = 6;
            this.button9.Text = "Add File";
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // listBoxFileEdits
            // 
            this.listBoxFileEdits.Location = new System.Drawing.Point(136, 176);
            this.listBoxFileEdits.Name = "listBoxFileEdits";
            this.listBoxFileEdits.Size = new System.Drawing.Size(432, 147);
            this.listBoxFileEdits.TabIndex = 5;
            this.listBoxFileEdits.DoubleClick += new System.EventHandler(this.listBoxFileEdits_DoubleClick);
            // 
            // button8
            // 
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button8.Location = new System.Drawing.Point(216, 108);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(120, 20);
            this.button8.TabIndex = 4;
            this.button8.Text = "Update from MOD";
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // labelModInstallationTime
            // 
            this.labelModInstallationTime.Location = new System.Drawing.Point(136, 80);
            this.labelModInstallationTime.Name = "labelModInstallationTime";
            this.labelModInstallationTime.Size = new System.Drawing.Size(184, 23);
            this.labelModInstallationTime.TabIndex = 3;
            this.labelModInstallationTime.Text = "0 minutes";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Installation Time";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // pictureBoxTextMod
            // 
            this.pictureBoxTextMod.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxTextMod.Image")));
            this.pictureBoxTextMod.Location = new System.Drawing.Point(8, 48);
            this.pictureBoxTextMod.Name = "pictureBoxTextMod";
            this.pictureBoxTextMod.Size = new System.Drawing.Size(48, 16);
            this.pictureBoxTextMod.TabIndex = 1;
            this.pictureBoxTextMod.TabStop = false;
            this.pictureBoxTextMod.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // labelOverviewTitle
            // 
            this.labelOverviewTitle.AutoSize = true;
            this.labelOverviewTitle.Font = new System.Drawing.Font("Verdana", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOverviewTitle.Location = new System.Drawing.Point(8, 8);
            this.labelOverviewTitle.Name = "labelOverviewTitle";
            this.labelOverviewTitle.Size = new System.Drawing.Size(219, 32);
            this.labelOverviewTitle.TabIndex = 0;
            this.labelOverviewTitle.Text = "{MOD TITLE}";
            this.labelOverviewTitle.DoubleClick += new System.EventHandler(this.labelOverviewTitle_DoubleClick);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(8, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 23);
            this.label4.TabIndex = 2;
            this.label4.Text = "Included Files";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelIncludedFiles
            // 
            this.labelIncludedFiles.Location = new System.Drawing.Point(136, 112);
            this.labelIncludedFiles.Name = "labelIncludedFiles";
            this.labelIncludedFiles.Size = new System.Drawing.Size(72, 23);
            this.labelIncludedFiles.TabIndex = 3;
            this.labelIncludedFiles.Text = "0 files";
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(8, 176);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(120, 23);
            this.label10.TabIndex = 2;
            this.label10.Text = "File Edits";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(8, 144);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(120, 23);
            this.label8.TabIndex = 2;
            this.label8.Text = "License";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelLicense
            // 
            this.labelLicense.Location = new System.Drawing.Point(136, 144);
            this.labelLicense.Name = "labelLicense";
            this.labelLicense.Size = new System.Drawing.Size(416, 23);
            this.labelLicense.TabIndex = 3;
            this.labelLicense.Text = "GPL";
            // 
            // tabPageHeader
            // 
            this.tabPageHeader.BackColor = System.Drawing.SystemColors.Info;
            this.tabPageHeader.Controls.Add(this.ModVersionStageComboBox);
            this.tabPageHeader.Controls.Add(this.MODTitleTextBox);
            this.tabPageHeader.Controls.Add(this.MODHistoryListView);
            this.tabPageHeader.Controls.Add(this.button1);
            this.tabPageHeader.Controls.Add(this.MODAuthorListBox);
            this.tabPageHeader.Controls.Add(this.MODInstallationLevelComboBox);
            this.tabPageHeader.Controls.Add(this.MODVersionReleaseDomainUpDown);
            this.tabPageHeader.Controls.Add(this.MODVersionMajorNumericUpDown);
            this.tabPageHeader.Controls.Add(this.Button16);
            this.tabPageHeader.Controls.Add(this.MODDescriptionLabel);
            this.tabPageHeader.Controls.Add(this.label2);
            this.tabPageHeader.Controls.Add(this.label3);
            this.tabPageHeader.Controls.Add(this.label5);
            this.tabPageHeader.Controls.Add(this.MODVersionMinorNumericUpDown);
            this.tabPageHeader.Controls.Add(this.MODVersionRevisionNumericUpDown);
            this.tabPageHeader.Controls.Add(this.label6);
            this.tabPageHeader.Controls.Add(this.label7);
            this.tabPageHeader.Controls.Add(this.button2);
            this.tabPageHeader.Controls.Add(this.button3);
            this.tabPageHeader.Controls.Add(this.MODAuthorNotesLabel);
            this.tabPageHeader.Controls.Add(this.button4);
            this.tabPageHeader.Controls.Add(this.label9);
            this.tabPageHeader.Controls.Add(this.MODInstallationTimeLabel);
            this.tabPageHeader.Controls.Add(this.label11);
            this.tabPageHeader.Controls.Add(this.label12);
            this.tabPageHeader.Controls.Add(this.button5);
            this.tabPageHeader.Controls.Add(this.button6);
            this.tabPageHeader.Controls.Add(this.button7);
            this.tabPageHeader.Location = new System.Drawing.Point(4, 22);
            this.tabPageHeader.Name = "tabPageHeader";
            this.tabPageHeader.Size = new System.Drawing.Size(784, 540);
            this.tabPageHeader.TabIndex = 1;
            this.tabPageHeader.Text = "Header";
            // 
            // ModVersionStageComboBox
            // 
            this.ModVersionStageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ModVersionStageComboBox.FormattingEnabled = true;
            this.ModVersionStageComboBox.Items.AddRange(new object[] {
            "Stable",
            "Alpha",
            "Beta",
            "ReleaseCandidate"});
            this.ModVersionStageComboBox.Location = new System.Drawing.Point(230, 95);
            this.ModVersionStageComboBox.Name = "ModVersionStageComboBox";
            this.ModVersionStageComboBox.Size = new System.Drawing.Size(105, 21);
            this.ModVersionStageComboBox.TabIndex = 40;
            this.ModVersionStageComboBox.SelectedIndexChanged += new System.EventHandler(this.ModVersionStageComboBox_SelectedIndexChanged);
            // 
            // MODTitleTextBox
            // 
            this.MODTitleTextBox.LanguageSelectorVisible = false;
            this.MODTitleTextBox.Location = new System.Drawing.Point(136, 8);
            this.MODTitleTextBox.Multiline = false;
            this.MODTitleTextBox.Name = "MODTitleTextBox";
            this.MODTitleTextBox.Size = new System.Drawing.Size(432, 20);
            this.MODTitleTextBox.TabIndex = 39;
            this.MODTitleTextBox.Enter += new System.EventHandler(this.MODTitleTextBox_Enter);
            this.MODTitleTextBox.SelectionChanged += new ModFormControls.LocalisedTextBox.SelectionChangedHandler(this.MODTitleTextBox_SelectionChanged);
            this.MODTitleTextBox.Leave += new System.EventHandler(this.MODTitleTextBox_Leave);
            // 
            // MODHistoryListView
            // 
            this.MODHistoryListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.MODHistoryListView.FullRowSelect = true;
            this.MODHistoryListView.GridLines = true;
            this.MODHistoryListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.MODHistoryListView.HideSelection = false;
            this.MODHistoryListView.Location = new System.Drawing.Point(136, 320);
            this.MODHistoryListView.Name = "MODHistoryListView";
            this.MODHistoryListView.Size = new System.Drawing.Size(432, 144);
            this.MODHistoryListView.TabIndex = 38;
            this.MODHistoryListView.UseCompatibleStateImageBehavior = false;
            this.MODHistoryListView.View = System.Windows.Forms.View.Details;
            this.MODHistoryListView.DoubleClick += new System.EventHandler(this.MODHistoryListView_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Version";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Date";
            this.columnHeader2.Width = 70;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Change Log";
            this.columnHeader3.Width = 290;
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Location = new System.Drawing.Point(8, 168);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(56, 20);
            this.button1.TabIndex = 37;
            this.button1.Text = "Add";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MODAuthorListBox
            // 
            this.MODAuthorListBox.Location = new System.Drawing.Point(136, 144);
            this.MODAuthorListBox.Name = "MODAuthorListBox";
            this.MODAuthorListBox.Size = new System.Drawing.Size(432, 69);
            this.MODAuthorListBox.TabIndex = 31;
            this.MODAuthorListBox.DoubleClick += new System.EventHandler(this.MODAuthorListBox_DoubleClick);
            // 
            // MODInstallationLevelComboBox
            // 
            this.MODInstallationLevelComboBox.Items.AddRange(new object[] {
            "Easy",
            "Moderate",
            "Hard"});
            this.MODInstallationLevelComboBox.Location = new System.Drawing.Point(136, 120);
            this.MODInstallationLevelComboBox.Name = "MODInstallationLevelComboBox";
            this.MODInstallationLevelComboBox.Size = new System.Drawing.Size(121, 21);
            this.MODInstallationLevelComboBox.TabIndex = 30;
            this.MODInstallationLevelComboBox.Text = "Easy";
            this.MODInstallationLevelComboBox.Leave += new System.EventHandler(this.MODTitleTextBox_Leave);
            this.MODInstallationLevelComboBox.Enter += new System.EventHandler(this.MODTitleTextBox_Enter);
            this.MODInstallationLevelComboBox.SelectedIndexChanged += new System.EventHandler(this.MODInstallationLevelComboBox_SelectedIndexChanged);
            this.MODInstallationLevelComboBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MODVersionMajorNumericUpDown_MouseUp);
            this.MODInstallationLevelComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ModEditor_KeyPress);
            this.MODInstallationLevelComboBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ModEditor_KeyUp);
            this.MODInstallationLevelComboBox.Click += new System.EventHandler(this.MODVersionMajorNumericUpDown_Click);
            // 
            // MODVersionReleaseDomainUpDown
            // 
            this.MODVersionReleaseDomainUpDown.Items.Add("");
            this.MODVersionReleaseDomainUpDown.Items.Add("a");
            this.MODVersionReleaseDomainUpDown.Items.Add("b");
            this.MODVersionReleaseDomainUpDown.Items.Add("c");
            this.MODVersionReleaseDomainUpDown.Items.Add("d");
            this.MODVersionReleaseDomainUpDown.Items.Add("e");
            this.MODVersionReleaseDomainUpDown.Items.Add("f");
            this.MODVersionReleaseDomainUpDown.Items.Add("g");
            this.MODVersionReleaseDomainUpDown.Items.Add("h");
            this.MODVersionReleaseDomainUpDown.Items.Add("i");
            this.MODVersionReleaseDomainUpDown.Items.Add("j");
            this.MODVersionReleaseDomainUpDown.Items.Add("k");
            this.MODVersionReleaseDomainUpDown.Items.Add("l");
            this.MODVersionReleaseDomainUpDown.Items.Add("m");
            this.MODVersionReleaseDomainUpDown.Items.Add("n");
            this.MODVersionReleaseDomainUpDown.Items.Add("o");
            this.MODVersionReleaseDomainUpDown.Items.Add("p");
            this.MODVersionReleaseDomainUpDown.Items.Add("q");
            this.MODVersionReleaseDomainUpDown.Items.Add("r");
            this.MODVersionReleaseDomainUpDown.Items.Add("s");
            this.MODVersionReleaseDomainUpDown.Items.Add("t");
            this.MODVersionReleaseDomainUpDown.Items.Add("u");
            this.MODVersionReleaseDomainUpDown.Items.Add("v");
            this.MODVersionReleaseDomainUpDown.Items.Add("w");
            this.MODVersionReleaseDomainUpDown.Items.Add("x");
            this.MODVersionReleaseDomainUpDown.Items.Add("y");
            this.MODVersionReleaseDomainUpDown.Items.Add("z");
            this.MODVersionReleaseDomainUpDown.Location = new System.Drawing.Point(395, 96);
            this.MODVersionReleaseDomainUpDown.Name = "MODVersionReleaseDomainUpDown";
            this.MODVersionReleaseDomainUpDown.Size = new System.Drawing.Size(48, 20);
            this.MODVersionReleaseDomainUpDown.Sorted = true;
            this.MODVersionReleaseDomainUpDown.TabIndex = 29;
            this.MODVersionReleaseDomainUpDown.Enter += new System.EventHandler(this.MODTitleTextBox_Enter);
            this.MODVersionReleaseDomainUpDown.Click += new System.EventHandler(this.MODVersionMajorNumericUpDown_Click);
            this.MODVersionReleaseDomainUpDown.SelectedItemChanged += new System.EventHandler(this.MODVersionReleaseDomainUpDown_SelectedItemChanged);
            this.MODVersionReleaseDomainUpDown.Leave += new System.EventHandler(this.MODTitleTextBox_Leave);
            this.MODVersionReleaseDomainUpDown.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ModEditor_KeyUp);
            this.MODVersionReleaseDomainUpDown.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ModEditor_KeyPress);
            this.MODVersionReleaseDomainUpDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MODVersionMajorNumericUpDown_MouseUp);
            // 
            // MODVersionMajorNumericUpDown
            // 
            this.MODVersionMajorNumericUpDown.Location = new System.Drawing.Point(136, 96);
            this.MODVersionMajorNumericUpDown.Name = "MODVersionMajorNumericUpDown";
            this.MODVersionMajorNumericUpDown.Size = new System.Drawing.Size(40, 20);
            this.MODVersionMajorNumericUpDown.TabIndex = 26;
            this.MODVersionMajorNumericUpDown.Enter += new System.EventHandler(this.MODTitleTextBox_Enter);
            this.MODVersionMajorNumericUpDown.Click += new System.EventHandler(this.MODVersionMajorNumericUpDown_Click);
            this.MODVersionMajorNumericUpDown.ValueChanged += new System.EventHandler(this.MODVersionMajorNumericUpDown_ValueChanged);
            this.MODVersionMajorNumericUpDown.Leave += new System.EventHandler(this.MODTitleTextBox_Leave);
            this.MODVersionMajorNumericUpDown.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ModEditor_KeyUp);
            this.MODVersionMajorNumericUpDown.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ModEditor_KeyPress);
            this.MODVersionMajorNumericUpDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MODVersionMajorNumericUpDown_MouseUp);
            // 
            // Button16
            // 
            this.Button16.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Button16.Location = new System.Drawing.Point(104, 48);
            this.Button16.Name = "Button16";
            this.Button16.Size = new System.Drawing.Size(24, 16);
            this.Button16.TabIndex = 25;
            this.Button16.Text = "...";
            this.Button16.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Button16.Click += new System.EventHandler(this.Button16_Click);
            // 
            // MODDescriptionLabel
            // 
            this.MODDescriptionLabel.Location = new System.Drawing.Point(136, 32);
            this.MODDescriptionLabel.Name = "MODDescriptionLabel";
            this.MODDescriptionLabel.Size = new System.Drawing.Size(432, 64);
            this.MODDescriptionLabel.TabIndex = 23;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 23);
            this.label2.TabIndex = 14;
            this.label2.Text = "MOD Title:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 23);
            this.label3.TabIndex = 12;
            this.label3.Text = "MOD Description:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(8, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 23);
            this.label5.TabIndex = 19;
            this.label5.Text = "MOD Version:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // MODVersionMinorNumericUpDown
            // 
            this.MODVersionMinorNumericUpDown.Location = new System.Drawing.Point(184, 96);
            this.MODVersionMinorNumericUpDown.Name = "MODVersionMinorNumericUpDown";
            this.MODVersionMinorNumericUpDown.Size = new System.Drawing.Size(40, 20);
            this.MODVersionMinorNumericUpDown.TabIndex = 27;
            this.MODVersionMinorNumericUpDown.Enter += new System.EventHandler(this.MODTitleTextBox_Enter);
            this.MODVersionMinorNumericUpDown.Click += new System.EventHandler(this.MODVersionMajorNumericUpDown_Click);
            this.MODVersionMinorNumericUpDown.ValueChanged += new System.EventHandler(this.MODVersionMinorNumericUpDown_ValueChanged);
            this.MODVersionMinorNumericUpDown.Leave += new System.EventHandler(this.MODTitleTextBox_Leave);
            this.MODVersionMinorNumericUpDown.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ModEditor_KeyUp);
            this.MODVersionMinorNumericUpDown.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ModEditor_KeyPress);
            this.MODVersionMinorNumericUpDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MODVersionMajorNumericUpDown_MouseUp);
            // 
            // MODVersionRevisionNumericUpDown
            // 
            this.MODVersionRevisionNumericUpDown.Location = new System.Drawing.Point(341, 96);
            this.MODVersionRevisionNumericUpDown.Name = "MODVersionRevisionNumericUpDown";
            this.MODVersionRevisionNumericUpDown.Size = new System.Drawing.Size(48, 20);
            this.MODVersionRevisionNumericUpDown.TabIndex = 28;
            this.MODVersionRevisionNumericUpDown.Enter += new System.EventHandler(this.MODTitleTextBox_Enter);
            this.MODVersionRevisionNumericUpDown.Click += new System.EventHandler(this.MODVersionMajorNumericUpDown_Click);
            this.MODVersionRevisionNumericUpDown.ValueChanged += new System.EventHandler(this.MODVersionRevisionNumericUpDown_ValueChanged);
            this.MODVersionRevisionNumericUpDown.Leave += new System.EventHandler(this.MODTitleTextBox_Leave);
            this.MODVersionRevisionNumericUpDown.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ModEditor_KeyUp);
            this.MODVersionRevisionNumericUpDown.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ModEditor_KeyPress);
            this.MODVersionRevisionNumericUpDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MODVersionMajorNumericUpDown_MouseUp);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(8, 120);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 23);
            this.label6.TabIndex = 13;
            this.label6.Text = "Installation Level:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(8, 144);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(120, 23);
            this.label7.TabIndex = 15;
            this.label7.Text = "MOD Authors:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button2.Location = new System.Drawing.Point(72, 168);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(56, 20);
            this.button2.TabIndex = 35;
            this.button2.Text = "Edit";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button3.Location = new System.Drawing.Point(8, 192);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(120, 20);
            this.button3.TabIndex = 36;
            this.button3.Text = "Delete";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // MODAuthorNotesLabel
            // 
            this.MODAuthorNotesLabel.Location = new System.Drawing.Point(136, 216);
            this.MODAuthorNotesLabel.Name = "MODAuthorNotesLabel";
            this.MODAuthorNotesLabel.Size = new System.Drawing.Size(432, 80);
            this.MODAuthorNotesLabel.TabIndex = 22;
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button4.Location = new System.Drawing.Point(104, 240);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(24, 16);
            this.button4.TabIndex = 24;
            this.button4.Text = "...";
            this.button4.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(8, 216);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(120, 23);
            this.label9.TabIndex = 18;
            this.label9.Text = "Author Notes:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // MODInstallationTimeLabel
            // 
            this.MODInstallationTimeLabel.Location = new System.Drawing.Point(136, 304);
            this.MODInstallationTimeLabel.Name = "MODInstallationTimeLabel";
            this.MODInstallationTimeLabel.Size = new System.Drawing.Size(104, 16);
            this.MODInstallationTimeLabel.TabIndex = 21;
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(8, 304);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(120, 23);
            this.label11.TabIndex = 16;
            this.label11.Text = "Installation Time:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(8, 328);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(120, 23);
            this.label12.TabIndex = 17;
            this.label12.Text = "MOD History:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // button5
            // 
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button5.Location = new System.Drawing.Point(8, 352);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(120, 20);
            this.button5.TabIndex = 32;
            this.button5.Text = "Add";
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button6.Location = new System.Drawing.Point(8, 376);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(120, 20);
            this.button6.TabIndex = 34;
            this.button6.Text = "Edit";
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button7.Location = new System.Drawing.Point(8, 400);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(120, 20);
            this.button7.TabIndex = 33;
            this.button7.Text = "Delete";
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // tabPageActions
            // 
            this.tabPageActions.BackColor = System.Drawing.Color.Transparent;
            this.tabPageActions.Controls.Add(this.modDisplayBox2);
            this.tabPageActions.Controls.Add(this.toolBar1);
            this.tabPageActions.Controls.Add(this.modActionEditor1);
            this.tabPageActions.Location = new System.Drawing.Point(4, 22);
            this.tabPageActions.Name = "tabPageActions";
            this.tabPageActions.Size = new System.Drawing.Size(784, 540);
            this.tabPageActions.TabIndex = 2;
            this.tabPageActions.Text = "Actions";
            this.tabPageActions.UseVisualStyleBackColor = true;
            // 
            // modDisplayBox2
            // 
            this.modDisplayBox2.AutoScroll = true;
            this.modDisplayBox2.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.modDisplayBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modDisplayBox2.Location = new System.Drawing.Point(0, 36);
            this.modDisplayBox2.Name = "modDisplayBox2";
            this.modDisplayBox2.SelectedIndex = 0;
            this.modDisplayBox2.Size = new System.Drawing.Size(784, 504);
            this.modDisplayBox2.TabIndex = 0;
            this.modDisplayBox2.SelectedIndexChanged += new System.EventHandler(this.modDisplayBox2_SelectedIndexChanged);
            this.modDisplayBox2.Click += new System.EventHandler(this.modDisplayBox2_Click);
            this.modDisplayBox2.ItemDoubleClick += new ModFormControls.ModActionItem.ActionItemClickHandler(this.modDisplayBox2_ItemDoubleClick);
            // 
            // toolBar1
            // 
            this.toolBar1.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButtonEditAction,
            this.toolBarButtonAddAction,
            this.toolBarButtonDelete});
            this.toolBar1.ButtonSize = new System.Drawing.Size(60, 22);
            this.toolBar1.DropDownArrows = true;
            this.toolBar1.ImageList = this.imageListModEditor;
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ShowToolTips = true;
            this.toolBar1.Size = new System.Drawing.Size(784, 36);
            this.toolBar1.TabIndex = 1;
            this.toolBar1.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
            // 
            // toolBarButtonEditAction
            // 
            this.toolBarButtonEditAction.ImageIndex = 0;
            this.toolBarButtonEditAction.Name = "toolBarButtonEditAction";
            this.toolBarButtonEditAction.Text = "Edit";
            // 
            // toolBarButtonAddAction
            // 
            this.toolBarButtonAddAction.DropDownMenu = this.contextMenuAddAction;
            this.toolBarButtonAddAction.ImageIndex = 1;
            this.toolBarButtonAddAction.Name = "toolBarButtonAddAction";
            this.toolBarButtonAddAction.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            this.toolBarButtonAddAction.Text = "Add Action";
            // 
            // contextMenuAddAction
            // 
            this.contextMenuAddAction.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemAddActionSql,
            this.menuItemAddActionCopy,
            this.menuItemAddActionOpen,
            this.menuItemAddActionFind,
            this.menuItemAddActionAfterAdd,
            this.menuItemAddActionBeforeAdd,
            this.menuItemAddActionReplaceWith,
            this.menuItemAddActionInLineFind,
            this.menuItemAddActionInLineAfterAdd,
            this.menuItemAddActionInLineBeforeAdd,
            this.menuItemAddActionInLineReplaceWith,
            this.menuItemAddActionIncrement,
            this.menuItemAddActionInLineIncrement,
            this.menuItemAddActionDiyInstruction});
            // 
            // menuItemAddActionSql
            // 
            this.menuItemAddActionSql.Index = 0;
            this.menuItemAddActionSql.Text = "SQL";
            this.menuItemAddActionSql.Click += new System.EventHandler(this.menuItemAddActionSql_Click);
            // 
            // menuItemAddActionCopy
            // 
            this.menuItemAddActionCopy.Index = 1;
            this.menuItemAddActionCopy.Text = "COPY";
            this.menuItemAddActionCopy.Click += new System.EventHandler(this.menuItemAddActionCopy_Click);
            // 
            // menuItemAddActionOpen
            // 
            this.menuItemAddActionOpen.Index = 2;
            this.menuItemAddActionOpen.Text = "OPEN";
            this.menuItemAddActionOpen.Click += new System.EventHandler(this.menuItemAddActionOpen_Click);
            // 
            // menuItemAddActionFind
            // 
            this.menuItemAddActionFind.Index = 3;
            this.menuItemAddActionFind.Text = "FIND";
            this.menuItemAddActionFind.Click += new System.EventHandler(this.menuItemAddActionFind_Click);
            // 
            // menuItemAddActionAfterAdd
            // 
            this.menuItemAddActionAfterAdd.Index = 4;
            this.menuItemAddActionAfterAdd.Text = "AFTER, ADD";
            this.menuItemAddActionAfterAdd.Click += new System.EventHandler(this.menuItemAddActionAfterAdd_Click);
            // 
            // menuItemAddActionBeforeAdd
            // 
            this.menuItemAddActionBeforeAdd.Index = 5;
            this.menuItemAddActionBeforeAdd.Text = "BEFORE, ADD";
            this.menuItemAddActionBeforeAdd.Click += new System.EventHandler(this.menuItemAddActionBeforeAdd_Click);
            // 
            // menuItemAddActionReplaceWith
            // 
            this.menuItemAddActionReplaceWith.Index = 6;
            this.menuItemAddActionReplaceWith.Text = "REPLACE WITH";
            this.menuItemAddActionReplaceWith.Click += new System.EventHandler(this.menuItemAddActionReplaceWith_Click);
            // 
            // menuItemAddActionInLineFind
            // 
            this.menuItemAddActionInLineFind.Index = 7;
            this.menuItemAddActionInLineFind.Text = "IN-LINE FIND";
            this.menuItemAddActionInLineFind.Click += new System.EventHandler(this.menuItemAddActionInLineFind_Click);
            // 
            // menuItemAddActionInLineAfterAdd
            // 
            this.menuItemAddActionInLineAfterAdd.Index = 8;
            this.menuItemAddActionInLineAfterAdd.Text = "IN-LINE AFTER, ADD";
            this.menuItemAddActionInLineAfterAdd.Click += new System.EventHandler(this.menuItemAddActionInLineAfterAdd_Click);
            // 
            // menuItemAddActionInLineBeforeAdd
            // 
            this.menuItemAddActionInLineBeforeAdd.Index = 9;
            this.menuItemAddActionInLineBeforeAdd.Text = "IN-LINE BEFORE, ADD";
            this.menuItemAddActionInLineBeforeAdd.Click += new System.EventHandler(this.menuItemAddActionInLineBeforeAdd_Click);
            // 
            // menuItemAddActionInLineReplaceWith
            // 
            this.menuItemAddActionInLineReplaceWith.Index = 10;
            this.menuItemAddActionInLineReplaceWith.Text = "IN-LINE REPLACE WITH";
            this.menuItemAddActionInLineReplaceWith.Click += new System.EventHandler(this.menuItemAddActionInLineReplaceWith_Click);
            // 
            // menuItemAddActionIncrement
            // 
            this.menuItemAddActionIncrement.Index = 11;
            this.menuItemAddActionIncrement.Text = "INCREMENT";
            this.menuItemAddActionIncrement.Click += new System.EventHandler(this.menuItemAddActionIncrement_Click);
            // 
            // menuItemAddActionInLineIncrement
            // 
            this.menuItemAddActionInLineIncrement.Index = 12;
            this.menuItemAddActionInLineIncrement.Text = "IN-LINE INCREMENT";
            this.menuItemAddActionInLineIncrement.Click += new System.EventHandler(this.menuItemAddActionInLineIncrement_Click);
            // 
            // menuItemAddActionDiyInstruction
            // 
            this.menuItemAddActionDiyInstruction.Index = 13;
            this.menuItemAddActionDiyInstruction.Text = "DIY INSTRUCTIONS";
            this.menuItemAddActionDiyInstruction.Click += new System.EventHandler(this.menuItemAddActionDiyInstruction_Click);
            // 
            // toolBarButtonDelete
            // 
            this.toolBarButtonDelete.ImageIndex = 2;
            this.toolBarButtonDelete.Name = "toolBarButtonDelete";
            this.toolBarButtonDelete.Text = "Delete Action";
            // 
            // imageListModEditor
            // 
            this.imageListModEditor.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListModEditor.ImageStream")));
            this.imageListModEditor.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListModEditor.Images.SetKeyName(0, "");
            this.imageListModEditor.Images.SetKeyName(1, "");
            this.imageListModEditor.Images.SetKeyName(2, "");
            // 
            // modActionEditor1
            // 
            this.modActionEditor1.Location = new System.Drawing.Point(10, 10);
            this.modActionEditor1.Name = "modActionEditor1";
            this.modActionEditor1.Size = new System.Drawing.Size(480, 320);
            this.modActionEditor1.TabIndex = 2;
            this.modActionEditor1.Visible = false;
            this.modActionEditor1.VisibleChanged += new System.EventHandler(this.modActionEditor1_VisibleChanged);
            this.modActionEditor1.Return += new ModFormControls.ModActionEditor.ModActionEditorReturnHandler(this.modActionEditor1_Return);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "untitled";
            this.saveFileDialog1.Filter = "phpBB2 MOD Files (*.mod,*.txt)|*.mod;*.txt|MODX Format (*.xml)|*.xml";
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // openActionDialog1
            // 
            this.openActionDialog1.SaveNew += new ModStudio.OpenActionDialogBox.OpenActionDialogBoxSaveNewHandler(this.openActionDialogBox1_SaveNew);
            // 
            // authorEditorDialog1
            // 
            this.authorEditorDialog1.Entry = modAuthor2;
            this.authorEditorDialog1.Save += new ModStudio.AuthorEditorDialogBox.AuthorEditorDialogBoxSaveHandler(this.authorEditorDialog1_Save);
            // 
            // historyEditorDialog1
            // 
            this.historyEditorDialog1.Entry = modHistoryEntry2;
            this.historyEditorDialog1.Save += new ModStudio.HistoryEditorDialogBox.HistoryEditorDialogBoxSaveHandler(this.historyEditorDialog1_Save);
            // 
            // noteEditorDialog1
            // 
            this.noteEditorDialog1.Save += new ModStudio.NoteEditorDialogBox.NoteEditorDialogBoxSaveHandler(this.noteEditorDialog1_Save);
            // 
            // targetVersionDialog1
            // 
            this.targetVersionDialog1.Save += new ModStudio.TargetVersionDialogBox.TargetVersionDialogBoxSaveHandler(this.targetVersionDialog1_Save);
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(784, 540);
            this.tabPage1.TabIndex = 3;
            this.tabPage1.Text = "SQL";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ModEditor
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.tabControlEditor);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ModEditor";
            this.Text = "untitled";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Resize += new System.EventHandler(this.ModEditor_Resize);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.ModEditor_Closing);
            this.Load += new System.EventHandler(this.ModEditor_Load);
            this.tabControlEditor.ResumeLayout(false);
            this.tabPageOverview.ResumeLayout(false);
            this.tabPageOverview.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxXmlMod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTextMod)).EndInit();
            this.tabPageHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MODVersionMajorNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MODVersionMinorNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MODVersionRevisionNumericUpDown)).EndInit();
            this.tabPageActions.ResumeLayout(false);
            this.tabPageActions.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			//e.Graphics.FillRectangle(new SolidBrush(Color.Orange),0,0,64,16);
		}

		private void tabControlEditor_TabIndexChanged(object sender, System.EventArgs e)
		{
		}

		private void UpdateHeader()
		{
			ThisMod.Header.Title = MODTitleTextBox.TextLang;
			if (MODVersionReleaseDomainUpDown.Text.ToCharArray().Length > 0)
			{
				ThisMod.Header.Version = new ModVersion((int)MODVersionMajorNumericUpDown.Value, (int)MODVersionMinorNumericUpDown.Value, (int)MODVersionRevisionNumericUpDown.Value, MODVersionReleaseDomainUpDown.Text.ToCharArray()[0]);
			}
			else
			{
				ThisMod.Header.Version = new ModVersion((int)MODVersionMajorNumericUpDown.Value, (int)MODVersionMinorNumericUpDown.Value, (int)MODVersionRevisionNumericUpDown.Value);
			}
            if (ModVersionStageComboBox.Text == VersionStage.Alpha.ToString())
            {
                ThisMod.Header.Version.Stage = VersionStage.Alpha;
            }
            else if (ModVersionStageComboBox.Text == VersionStage.Beta.ToString())
            {
                ThisMod.Header.Version.Stage = VersionStage.Beta;
            }
            else if (ModVersionStageComboBox.Text == VersionStage.ReleaseCandidate.ToString())
            {
                ThisMod.Header.Version.Stage = VersionStage.ReleaseCandidate;
            }
            else
            {
                ThisMod.Header.Version.Stage = VersionStage.Stable;
            }
			ThisMod.Header.InstallationLevel = Mod.InstallationLevelParse(MODInstallationLevelComboBox.Text);
		}

		private int LastSelectedIndex = 0;
		private void tabControlEditor_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			switch (LastSelectedIndex)
			{
				case 0: // Overview
					break;
				case 1: // Header
					UpdateHeader();
					break;
				case 2: // Actions
					//modDisplayBox2.ModActions = ThisMod.Actions;
					break;
			}

			switch (tabControlEditor.SelectedIndex)
			{
				case 0: // Overview
					UpdateOverviewDisplay();
					break;
				case 1: // Header
					UpdateHeaderDisplay();
					break;
				case 2: // Actions
					UpdateActionsDisplay();
					break;
			}

			LastSelectedIndex = tabControlEditor.SelectedIndex;
		}

		private void UpdateOverviewDisplay()
		{
			labelOverviewTitle.Text = ThisMod.Header.Title.GetValue();
			labelModInstallationTime.Text = string.Format("{0} minutes", (ThisMod.Header.InstallationTime / 60));
			listBoxFileEdits.Items.Clear();
			if (ThisMod.Actions != null)
			{
				foreach (ModAction a in ThisMod.Actions)
				{
					char[] TrimChars = {' ', '\t', '\n', '\r'};
					if (a.Type == "OPEN")
					{
						listBoxFileEdits.Items.Add(a.Body.Trim(TrimChars));
					}
				}
			}
			if (ThisMod.Header.IncludedFiles != null)
			{
				labelIncludedFiles.Text = string.Format("{0} files", (ThisMod.Header.IncludedFiles.Count));
				if (ThisMod.Header.IncludedFiles.Count > 0)
				{
					if (ThisMod.Header.IncludedFiles[0] == "" || ThisMod.Header.IncludedFiles[0].ToLower() == "n/a")
					{
						labelIncludedFiles.Text = "0 files";
					}
				}
				if (labelIncludedFiles.Text == "1 files")
				{
					labelIncludedFiles.Text = "1 file";
				}
			}
			else
			{
				labelIncludedFiles.Text = "0 files";
			}
			if (ThisMod.GetType() == typeof(TextMod))
			{
				pictureBoxXmlMod.Visible = false;
				pictureBoxTextMod.Visible = true;
				label13.Visible = false;
				labelPhpbbVersion.Visible = false;
				button10.Visible = false;
			}
			else if (ThisMod.GetType() == typeof(ModxMod))
			{
				pictureBoxXmlMod.Visible = true;
				pictureBoxTextMod.Visible = false;
				label13.Visible = true;
				labelPhpbbVersion.Visible = true;
				button10.Visible = true;
				try
				{
					labelPhpbbVersion.Text = ThisMod.Header.PhpbbVersion.ToString();
				}
				catch
				{
				}
			}
			labelLicense.Text = ThisMod.Header.License;
		}

		private void UpdateHeaderDisplay()
		{
			MODTitleTextBox.Text = ThisMod.Header.Title.GetValue();
			MODTitleTextBox.TextLang = ThisMod.Header.Title;
			MODAuthorListBox.Items.Clear();
			foreach (ModAuthor a in ThisMod.Header.Authors)
			{
				MODAuthorListBox.Items.Add(a.UserName);
			}
			MODInstallationLevelComboBox.Text = ThisMod.Header.InstallationLevel.ToString();
			MODDescriptionLabel.Text = ThisMod.Header.Description.GetValue();
			MODAuthorNotesLabel.Text = ThisMod.Header.AuthorNotes.GetValue();
			MODHistoryListView.Items.Clear();
			// TODO: properly handle the exception given by this when no MOD History is included
			try
			{
				foreach (ModHistoryEntry h in ThisMod.Header.History)
				{
					// TODO: 
					string sample = "";
					foreach (DictionaryEntry de in h.ChangeLog)
					{
						if ((string)de.Key == Mod.DefaultLanguage)
						{
							if (((ModHistoryChangeLog)de.Value).Count > 0)
							{
								sample = ((ModHistoryChangeLog)de.Value)[0];
							}
						}
					}
					string[] tempItem = {h.Version.ToString(), h.Date.ToShortDateString(), sample};
					MODHistoryListView.Items.Add(new ListViewItem(tempItem));
				}
			}
			catch
			{
			}
			MODVersionMajorNumericUpDown.Value = ThisMod.Header.Version.Major;
			MODVersionMinorNumericUpDown.Value = ThisMod.Header.Version.Minor;
			MODVersionRevisionNumericUpDown.Value = ThisMod.Header.Version.Revision;
			MODVersionReleaseDomainUpDown.Text = ThisMod.Header.Version.Release.ToString();
            ModVersionStageComboBox.Text = ThisMod.Header.Version.Stage.ToString();
			MODInstallationTimeLabel.Text = string.Format("{0} minutes", (ThisMod.Header.InstallationTime / 60));

			if (ThisMod.GetType() == typeof(TextMod))
			{
				MODTitleTextBox.LanguageSelectorVisible = false;
			}
			else if (ThisMod.GetType() == typeof(ModxMod))
			{
				MODTitleTextBox.LanguageSelectorVisible = true;
			}
		}

		private void UpdateActionsDisplay()
		{
			modDisplayBox2.ModActions = ThisMod.Actions;
			modDisplayBox2.UpdateLayout();
			Console.WriteLine(modDisplayBox2.CanFocus + ":f");
			modDisplayBox2.Focus();
			//modDisplayBox2.Select();
		}

		private void ModEditor_Load(object sender, System.EventArgs e)
		{
			tabControlEditor.SelectedIndex = 0;
			//tabControlEditor_SelectedIndexChanged(null,null);
			UpdateOverviewDisplay();
			UpdateHeaderDisplay();
			modDisplayBox2_SelectedIndexChanged(null, null);
			SetUnmodified();
			if (ThisMod.ParseErrors)
			{
				MessageBox.Show(this, "There were some errors while importing this MOD, please make sure to go and fix them.\r\nMost of the time this will only require a small edit to your MOD.", "Error(s) occured while importing MOD", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void listBoxFileEdits_DoubleClick(object sender, System.EventArgs e)
		{
			char[] TrimChars = {' ', '\t', '\n', '\r'};
			if (listBoxFileEdits.SelectedIndices.Count > 0)
			{
				tabControlEditor.SelectedIndex = 2;
				for (int i = 0; i < ThisMod.Actions.Count; i++)
				{
					if (ThisMod.Actions[i].Type == "OPEN")
					{
						if (ThisMod.Actions[i].Body.Trim(TrimChars) == listBoxFileEdits.Items[listBoxFileEdits.SelectedIndex].ToString())
						{
							modDisplayBox2.SelectedIndex = i;
						}
					}
				}
				modDisplayBox2.UpdateColours();
			}
		}

		private void ModEditor_Resize(object sender, System.EventArgs e)
		{
			if (tabControlEditor.SelectedIndex == 2)
			{
				modDisplayBox2.UpdateSize();
			}
			this.modActionEditor1.Size = new Size(tabPageActions.Width - 20 - 17, tabPageActions.Height - 20 - toolBar1.Height);
			this.modActionEditor1.Location = new Point(10,10 + toolBar1.Height);
		}

		private void modDisplayBox2_ItemDoubleClick(object sender, ModFormControls.ActionItemClickEventArgs e)
		{
			/*ModFormControls.ModActionEditor abc = new ModFormControls.ModActionEditor();
			abc.Location = new Point(10,10);
			abc.Size = new Size(tabPageActions.Width - 20 - 17, tabPageActions.Height - 20);
			abc.Visible = true;

			this.tabPageActions.Controls.Add(abc);*/

			bool isXml = false;
			if (ThisMod.GetType() == typeof(ModxMod)) isXml = true;

			modActionEditor1.actionIndex = modDisplayBox2.SelectedIndex;
			modActionEditor1.SetModAction(modDisplayBox2.SelectedIndex, ThisMod.Actions[modDisplayBox2.SelectedIndex].Type, ThisMod.Actions[modDisplayBox2.SelectedIndex].Body, ThisMod.Actions[modDisplayBox2.SelectedIndex].AfterComment, isXml);
			this.modActionEditor1.Size = new Size(tabPageActions.Width - 20 - 17, tabPageActions.Height - 20 - toolBar1.Height);
			this.modActionEditor1.Location = new Point(10,10 + toolBar1.Height);
			toolBar1.Enabled = false;
			modActionEditor1.Show();
			modActionEditor1.BringToFront();
			modActionEditor1.Select();
		}

		private void modActionEditor1_Return(object sender, ModFormControls.ModActionEditorReturnEventArgs e)
		{
			ModAction tempAction = ((ModAction)ThisMod.Actions[e.Index]);
			tempAction.Type = e.ActionType;
			tempAction.Body = e.ActionBody;
			tempAction.AfterComment = e.ActionComment;
			ThisMod.Actions[e.Index] = tempAction;

			//modDisplayBox2.ModActions = ThisMod.Actions;
			//modDisplayBox2.UpdateLayout();
			modDisplayBox2.UpdateText();
			//modDisplayBox2.Select();
			SetModified();
			toolBar1.Enabled = true;
		}

		private void modDisplayBox2_Click(object sender, System.EventArgs e)
		{
			modDisplayBox2.Focus();
		}

		/// <summary>
		/// 
		/// </summary>
		public void SaveFile()
		{
			UpdateModFilesToEdit();
			UpdateModInstallTime();
			UpdateHeader();
			if (this.Text.EndsWith("*")) // the file has been modified
			{
				if (this.Text.StartsWith("untitled")) // hasn't been saved before
				{
					SaveFileAs();
				}
				else
				{
					ThisMod.Write(this.Text.Substring(0, this.Text.Length - 1));
					SetUnmodified();
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void SaveFileAs()
		{
			if (ThisMod.GetType() == typeof(TextMod))
			{
				//if (ThisMod.Header.PhpbbVersion.Major == 3)
                if (ThisMod.Header.PhpbbVersion != null)
                {
                    foreach (TargetVersionCase tvc in ThisMod.Header.PhpbbVersion)
                    {
                        if (tvc.Part == TargetVersionPart.Major && int.Parse(tvc.GetValue) == 3)
                        {
                            // dialog box, you cannot do this
                            MessageBox.Show(this, "Can not save phpBB3 MODs in MOD Template format, must save as MODX.", "Can not save as MOD Template");
                            // abort savefileas
                            return;
                        }
                    }
                }
			}
			UpdateModFilesToEdit();
			UpdateModInstallTime();
			UpdateHeader();
			if (ThisMod.GetType() == typeof(ModxMod)) saveFileDialog1.FilterIndex = 2;
			if (ThisMod.GetType() == typeof(TextMod)) saveFileDialog1.FilterIndex = 1;
			saveFileDialog1.ShowDialog(this);
		}

		private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			bool typeChanged = false;
			// Force it to save in the format of the extension given.
			string fileNameLower = saveFileDialog1.FileName.ToLower();
			if (fileNameLower.EndsWith(".xml") || fileNameLower.EndsWith(".modx"))
			{
				if (ThisMod.GetType() == typeof(TextMod))
				{
					ThisMod = ((ModxMod)((TextMod)ThisMod));
					typeChanged = true;
				}
			}
			else if (fileNameLower.EndsWith(".mod") || fileNameLower.EndsWith(".txt"))
			{
				if (ThisMod.GetType() == typeof(ModxMod))
				{
					ThisMod = ((TextMod)((ModxMod)ThisMod));
					((TextMod)ThisMod).LoadTemplate(Application.StartupPath);
					typeChanged = true;
				}
			}
			ThisMod.Write(saveFileDialog1.FileName);
			this.Text = saveFileDialog1.FileName;
			SetUnmodified();
			if (typeChanged)
			{
				UpdateOverviewDisplay();
				UpdateHeaderDisplay();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void SetModified()
		{
			if (!this.Text.EndsWith("*"))
			{
				this.Text += "*";
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void SetUnmodified()
		{
			if (this.Text.EndsWith("*"))
			{
				this.Text = this.Text.Substring(0, this.Text.Length - 1);
			}
		}

		private void MODTitleTextBox_TextChanged(object sender, System.EventArgs e)
		{
			SetModified();
		}

		private void MODVersionMajorNumericUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			SetModified();
		}

		private void MODVersionMinorNumericUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			SetModified();
		}

		private void MODVersionRevisionNumericUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			SetModified();
		}

		private void MODVersionReleaseDomainUpDown_SelectedItemChanged(object sender, System.EventArgs e)
		{
			SetModified();
		}

		private void MODInstallationLevelComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			SetModified();
		}

		private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			switch (toolBar1.Buttons.IndexOf(e.Button))
			{
				case 0: // edit
					modDisplayBox2_ItemDoubleClick(null, null);
					break;
				case 1: // add action
					break;
				case 2: // delete action
					if (ThisMod.Actions[modDisplayBox2.SelectedIndex].Type != "SAVE/CLOSE ALL FILES")
					{
						modDisplayBox2.RemoveAt(modDisplayBox2.SelectedIndex);
						//modDisplayBox2.ModActions = ThisMod.Actions;
						//modDisplayBox2.UpdateLayout();
						modDisplayBox2.Select();
						SetModified();
					}
					break;
			}
		}

		private void modDisplayBox2_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			switch (ThisMod.Actions[modDisplayBox2.SelectedIndex].Type)
			{
				case "OPEN":
					menuItemAddActionAfterAdd.Enabled = false;
					menuItemAddActionFind.Enabled = true;
					menuItemAddActionBeforeAdd.Enabled = false;
					menuItemAddActionOpen.Enabled = true;
					menuItemAddActionInLineFind.Enabled = false;
					menuItemAddActionInLineAfterAdd.Enabled = false;
					menuItemAddActionInLineBeforeAdd.Enabled = false;
					menuItemAddActionReplaceWith.Enabled = false;
					menuItemAddActionInLineReplaceWith.Enabled = false;
					menuItemAddActionIncrement.Enabled = false;
					menuItemAddActionInLineIncrement.Enabled = false;
					menuItemAddActionDiyInstruction.Enabled = true;
					menuItemAddActionSql.Enabled = true;
					menuItemAddActionCopy.Enabled = true;
					break;
				case "COPY":
				case "SQL":
				case "DIY INSTRUCTION":
				case "SAVE/CLOSE ALL FILES":
					menuItemAddActionAfterAdd.Enabled = false;
					menuItemAddActionBeforeAdd.Enabled = false;
					menuItemAddActionOpen.Enabled = true;
					menuItemAddActionInLineFind.Enabled = false;
					menuItemAddActionInLineAfterAdd.Enabled = false;
					menuItemAddActionInLineBeforeAdd.Enabled = false;
					menuItemAddActionReplaceWith.Enabled = false;
					menuItemAddActionInLineReplaceWith.Enabled = false;
					menuItemAddActionIncrement.Enabled = false;
					menuItemAddActionInLineIncrement.Enabled = false;
					menuItemAddActionDiyInstruction.Enabled = true;
					menuItemAddActionSql.Enabled = true;
					menuItemAddActionCopy.Enabled = true;
					menuItemAddActionFind.Enabled = false;
					break;
				case "FIND":
				case "AFTER, ADD":
				case "BEFORE, ADD":
				case "REPLACE WITH":
					menuItemAddActionAfterAdd.Enabled = true;
					menuItemAddActionFind.Enabled = true;
					menuItemAddActionBeforeAdd.Enabled = true;
					menuItemAddActionOpen.Enabled = true;
					menuItemAddActionInLineFind.Enabled = true;
					menuItemAddActionInLineAfterAdd.Enabled = false;
					menuItemAddActionInLineBeforeAdd.Enabled = false;
					menuItemAddActionReplaceWith.Enabled = true;
					menuItemAddActionInLineReplaceWith.Enabled = false;
					menuItemAddActionIncrement.Enabled = true;
					menuItemAddActionInLineIncrement.Enabled = false;
					menuItemAddActionDiyInstruction.Enabled = true;
					menuItemAddActionSql.Enabled = true;
					menuItemAddActionCopy.Enabled = true;
					break;
				case "IN-LINE FIND":
				case "IN-LINE AFTER, ADD":
				case "IN-LINE BEFORE, ADD":
				case "IN-LINE REPLACE WITH":
					menuItemAddActionAfterAdd.Enabled = true;
					menuItemAddActionFind.Enabled = true;
					menuItemAddActionBeforeAdd.Enabled = true;
					menuItemAddActionOpen.Enabled = true;
					menuItemAddActionInLineFind.Enabled = true;
					menuItemAddActionInLineAfterAdd.Enabled = true;
					menuItemAddActionInLineBeforeAdd.Enabled = true;
					menuItemAddActionReplaceWith.Enabled = true;
					menuItemAddActionInLineReplaceWith.Enabled = true;
					menuItemAddActionIncrement.Enabled = true;
					menuItemAddActionInLineIncrement.Enabled = true;
					menuItemAddActionDiyInstruction.Enabled = true;
					menuItemAddActionSql.Enabled = true;
					menuItemAddActionCopy.Enabled = true;
					break;
			}
		}

		private void menuItemAddActionSql_Click(object sender, System.EventArgs e)
		{
			AddAction("SQL");
		}

		private void menuItemAddActionOpen_Click(object sender, System.EventArgs e)
		{
            openActionDialog1.PhpbbVersion = ThisMod.Header.PhpbbVersion;
			openActionDialog1.ShowDialog(this);
		}

		private void AddAction(string actionType)
		{
			AddAction(actionType, "");
		}

		private void menuItemAddActionDiyInstruction_Click(object sender, System.EventArgs e)
		{
			AddAction("DIY INSTRUCTIONS");
		}

		private void menuItemAddActionInLineIncrement_Click(object sender, System.EventArgs e)
		{
			AddAction("IN-LINE INCREMENT");
		}

		private void menuItemAddActionIncrement_Click(object sender, System.EventArgs e)
		{
			AddAction("INCREMENT");
		}

		private void menuItemAddActionInLineReplaceWith_Click(object sender, System.EventArgs e)
		{
			AddAction("IN-LINE REPLACE WITH");
		}

		private void menuItemAddActionInLineBeforeAdd_Click(object sender, System.EventArgs e)
		{
			AddAction("IN-LINE BEFORE, ADD");
		}

		private void menuItemAddActionInLineAfterAdd_Click(object sender, System.EventArgs e)
		{
			AddAction("IN-LINE AFTER, ADD");
		}

		private void menuItemAddActionInLineFind_Click(object sender, System.EventArgs e)
		{
			AddAction("IN-LINE FIND");
		}

		private void menuItemAddActionReplaceWith_Click(object sender, System.EventArgs e)
		{
			AddAction("REPLACE WITH");
		}

		private void menuItemAddActionBeforeAdd_Click(object sender, System.EventArgs e)
		{
			AddAction("BEFORE, ADD");
		}

		private void menuItemAddActionAfterAdd_Click(object sender, System.EventArgs e)
		{
			AddAction("AFTER, ADD");
		}

		private void menuItemAddActionCopy_Click(object sender, System.EventArgs e)
		{
			AddAction("COPY");
		}

		private void menuItemAddActionFind_Click(object sender, System.EventArgs e)
		{
			AddAction("FIND");
		}
	
		private void AddAction(string actionType, string actionBody)
		{
			int addBefore = modDisplayBox2.SelectedIndex;
			if (addBefore + 1 < ThisMod.Actions.Count) 
			{
				addBefore++;
			}
			modDisplayBox2.Insert(addBefore, new ModAction(actionType, actionBody, ""));
			//modDisplayBox2.ModActions = ThisMod.Actions;
			//modDisplayBox2.UpdateLayout();
			modDisplayBox2.Select();
			modDisplayBox2.SelectedIndex = addBefore;
			SetModified();
		}

		private void openActionDialogBox1_SaveNew(object sender, OpenActionDialogBoxSaveNewEventArgs e)
		{
			if (tabControlEditor.SelectedIndex == 2)
			{
				AddAction("OPEN", e.FileName);
			}
			else
			{
				ThisMod.Actions.Insert(ThisMod.Actions.Count - 1, new ModAction("OPEN", e.FileName, ""));
			}
			SetModified();
		}
		
		private void noteEditorDialog1_Save(object sender, NoteEditorDialogBoxSaveEventArgs e)
		{
			switch (e.Type)
			{
				case "Desc":
					ThisMod.Header.Description = e.Note;
					break;
				case "Auth":
					ThisMod.Header.AuthorNotes = e.Note;
					break;
			}
			SetModified();
			MODDescriptionLabel.Text = ThisMod.Header.Description.GetValue();
			MODAuthorNotesLabel.Text = ThisMod.Header.AuthorNotes.GetValue();
		}

		private void button9_Click(object sender, System.EventArgs e)
		{
            openActionDialog1.PhpbbVersion = ThisMod.Header.PhpbbVersion;
			openActionDialog1.ShowDialog(this);
			tabControlEditor_SelectedIndexChanged(null, null);
		}

		private void historyEditorDialog1_Save(object sender, HistoryEditorDialogBoxSaveEventArgs e)
		{
			if (historyEditorDialog1.Index == -1)
			{
				ThisMod.Header.History.Add(e.Entry);
			}
			else
			{
				ThisMod.Header.History[historyEditorDialog1.Index] = e.Entry;
			}
			tabControlEditor_SelectedIndexChanged(null, null);
			SetModified();
            this.Parent.Select();
		}

		private void button5_Click(object sender, System.EventArgs e)
		{
			historyEditorDialog1.Reset();
			historyEditorDialog1.Localised = (ThisMod.GetType() == typeof(ModxMod));
			historyEditorDialog1.ShowDialog(this);
		}

		private void authorEditorDialog1_Save(object sender, AuthorEditorDialogBoxSaveEventArgs e)
		{
			if (authorEditorDialog1.Index == -1)
			{
				ThisMod.Header.Authors.Add(e.Entry);
			}
			else
			{
				ThisMod.Header.Authors.Authors[authorEditorDialog1.Index] = e.Entry;
			}
			tabControlEditor_SelectedIndexChanged(null, null);
			SetModified();
            this.Parent.Select();
		}

		private void Button16_Click(object sender, System.EventArgs e)
		{
			noteEditorDialog1.Type = "Desc";
			noteEditorDialog1.Note = ThisMod.Header.Description;
			noteEditorDialog1.Localised = (ThisMod.GetType() == typeof(ModxMod));
			noteEditorDialog1.ShowDialog(this);
		}

		private void button4_Click(object sender, System.EventArgs e)
		{
			noteEditorDialog1.Type = "Auth";
			noteEditorDialog1.Note = ThisMod.Header.AuthorNotes;
			noteEditorDialog1.Localised = (ThisMod.GetType() == typeof(ModxMod));
			noteEditorDialog1.ShowDialog(this);
		}

		private void button6_Click(object sender, System.EventArgs e)
		{
			if (MODHistoryListView.SelectedItems.Count > 0)
			{
				historyEditorDialog1.Index = MODHistoryListView.SelectedItems[0].Index;
				historyEditorDialog1.Entry = ThisMod.Header.History[historyEditorDialog1.Index];
				historyEditorDialog1.Localised = (ThisMod.GetType() == typeof(ModxMod));
				historyEditorDialog1.ShowDialog(this);
			}
		}

		private void MODHistoryListView_DoubleClick(object sender, System.EventArgs e)
		{
			button6_Click(null, null);
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			authorEditorDialog1.Reset();
			authorEditorDialog1.ShowDialog(this);
		}

		private void button7_Click(object sender, System.EventArgs e)
		{
			if (MODHistoryListView.SelectedItems.Count > 0)
			{
				ThisMod.Header.History.RemoveAt(MODHistoryListView.SelectedItems[0].Index);
				tabControlEditor_SelectedIndexChanged(null, null);
			}
			SetModified();
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			try
			{
				authorEditorDialog1.Index = MODAuthorListBox.SelectedIndex;
				authorEditorDialog1.Entry = ThisMod.Header.Authors[authorEditorDialog1.Index];
				authorEditorDialog1.ShowDialog(this);
			}
			catch
			{
			}
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			if (ThisMod.Header.Authors.Authors.Count > 1)
			{
				ThisMod.Header.Authors.RemoveAt(MODAuthorListBox.SelectedIndex);
				tabControlEditor_SelectedIndexChanged(null, null);
			}
			else
			{
				MessageBox.Show(this, "A MOD must always have at least one author");
			}
			SetModified();
		}

		private void MODAuthorListBox_DoubleClick(object sender, System.EventArgs e)
		{
			button2_Click(null, null);
		}

		private void button8_Click(object sender, System.EventArgs e)
		{
			UpdateModIncludedFiles();
			tabControlEditor_SelectedIndexChanged(null, null);
			SetModified();
		}

		/// <summary>
		/// 
		/// </summary>
		public void UpdateModIncludedFiles()
		{
			ThisMod.Header.IncludedFiles.Clear();
			char[] trimChars = {' ', '\t', '\r'};
			for (int i = 0; i < ThisMod.Actions.Count; i++)
			{
				if (ThisMod.Actions[i].Type == "COPY")
				{
					string[] lines = ThisMod.Actions[i].Body.Split('\n');
					foreach (string line in lines)
					{
						if (line.TrimStart(trimChars).ToLower().StartsWith("copy"))
						{
							ThisMod.Header.IncludedFiles.Add(Regex.Match(line.Trim(trimChars), "copy (.+) to", RegexOptions.IgnoreCase).Value.Replace("copy ", "").Replace(" to", ""));
						}
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void UpdateModFilesToEdit()
		{
			ThisMod.Header.FilesToEdit.Clear();
			char[] trimChars = {' ', '\t', '\n', '\r'};
			foreach (ModAction e in ThisMod.Actions)
			{
				if (e.Type == "OPEN")
				{
					ThisMod.Header.FilesToEdit.Add(e.Body.Trim(trimChars));
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void UpdateModInstallTime()
		{
			int totalinstalltime = 126;
			foreach (ModAction e in ThisMod.Actions)
			{
				switch (e.Type)
				{
					case "OPEN":
						totalinstalltime += 27;
						break;
					case "SQL":
						totalinstalltime += 50;
						break;
					case "COPY":
						totalinstalltime += e.Body.Split('n').Length * 5;
						break;
					case "FIND":
					case "IN-LINE FIND":
						totalinstalltime += 12;
						break;
					case "AFTER, ADD":
					case "BEFORE, ADD":
					case "REPLACE WITH":
					case "INCREMENT":
					case "IN-LINE AFTER, ADD":
					case "IN-LINE BEFORE, ADD":
					case "IN-LINE REPLACE WITH":
					case "IN-LINE INCREMENT":
						totalinstalltime += 18;
						break;
					case "DIY INSTRUCTIONS":
						totalinstalltime += 60;
						break;
				}
			}
			ThisMod.Header.InstallationTime = totalinstalltime;
		}

		private void modActionEditor1_VisibleChanged(object sender, System.EventArgs e)
		{
			if (modActionEditor1.Visible == false)
			{
				toolBar1.Enabled = true;
				modDisplayBox2.Select();
			}
		}

		private void ModEditor_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (this.Text.EndsWith("*"))
			{
				switch (MessageBox.Show(this, "Do you want to save the current MOD?", "Save?", System.Windows.Forms.MessageBoxButtons.YesNoCancel))
				{
					case DialogResult.Yes:
						this.SaveFile();
						break;
					case DialogResult.No:
						// do nothing, let it close
						break;
					case DialogResult.Cancel:
						e.Cancel = true;
						break;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsCutCopyPaste()
		{
            if (this.ActiveControl == null)
            {
                return false;
            }

			if (this.ActiveControl.GetType() == typeof(LocalisedTextBox))
			{
				if (((LocalisedTextBox)this.ActiveControl).Enabled)
				{
					return true;
				}
			}
			else if (this.ActiveControl.GetType() == typeof(NumericUpDown))
			{
				return true;
			}
			else if (this.ActiveControl.GetType() == typeof(DomainUpDown))
			{
				return true;
			}
			else if (this.ActiveControl.GetType() == typeof(ComboBox))
			{
				return true;
			}
			else if (this.ActiveControl.GetType() == typeof(ModDisplayBox))
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsCut()
		{
			if (this.ActiveControl.GetType() == typeof(LocalisedTextBox))
			{
				if (((LocalisedTextBox)this.ActiveControl).IsTextSelected())
				{
					return true;
				}
				//Console.WriteLine(":-:");
			}
			else if (this.ActiveControl.GetType() == typeof(NumericUpDown))
			{
				NumericUpDown currentUpDown = (NumericUpDown)this.ActiveControl;
				try
				{
					//Console.WriteLine(":+:" + currentUpDown.Controls[1].GetType().ToString());
					if (((TextBox)currentUpDown.Controls[1]).SelectionLength > 0)
					{
						return true;
					}
					//Console.WriteLine(((TextBox)currentUpDown.Controls[1]).Text);
				}
				catch (Exception ex)
				{
					//Console.WriteLine(ex.ToString());
				}
			}
			else if (this.ActiveControl.GetType() == typeof(DomainUpDown))
			{
				DomainUpDown currentUpDown = (DomainUpDown)this.ActiveControl;
				try
				{
					//Console.WriteLine(":+:" + currentUpDown.Controls[1].GetType().ToString());
					if (((TextBox)currentUpDown.Controls[1]).SelectionLength > 0)
					{
						return true;
					}
					//Console.WriteLine(((TextBox)currentUpDown.Controls[1]).Text);
				}
				catch (Exception ex)
				{
					//Console.WriteLine(ex.ToString());
				}
			}
			else if (this.ActiveControl.GetType() == typeof(ComboBox))
			{
				ComboBox currentComboBox = (ComboBox)this.ActiveControl;
				if (currentComboBox.SelectedText.Length > 0)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsCopy()
		{
			if(this.IsCut())
			{
				return true;
			}
			else
			{
				if (this.ActiveControl.GetType() == typeof(LocalisedTextBox))
				{
					if (((LocalisedTextBox)this.ActiveControl).IsTextSelected())
					{
						return true;
					}
				}
				else if (this.ActiveControl.GetType() == typeof(ModDisplayBox))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsPaste()
		{
			if (this.ActiveControl.GetType() == typeof(LocalisedTextBox))
			{
				return true;
			}
			else if (this.ActiveControl.GetType() == typeof(NumericUpDown))
			{
				return true;
			}
			else if (this.ActiveControl.GetType() == typeof(DomainUpDown))
			{
				return true;
			}
			else if (this.ActiveControl.GetType() == typeof(ComboBox))
			{
				return true;
			}
			else if (this.ActiveControl.GetType() == typeof(ModDisplayBox))
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		public void DoCut()
		{
			if (this.ActiveControl.GetType() == typeof(LocalisedTextBox))
			{
				LocalisedTextBox currentLocalisedTextBox = (LocalisedTextBox)this.ActiveControl;
				currentLocalisedTextBox.DoCut();
			}
			else if (this.ActiveControl.GetType() == typeof(NumericUpDown))
			{
				NumericUpDown currentNumericUpDown = (NumericUpDown)this.ActiveControl;
				Clipboard.SetDataObject(((TextBox)currentNumericUpDown.Controls[1]).SelectedText);
				((TextBox)currentNumericUpDown.Controls[1]).SelectedText = "";
			}
			else if (this.ActiveControl.GetType() == typeof(DomainUpDown))
			{
				DomainUpDown currentDomainUpDown = (DomainUpDown)this.ActiveControl;
				Clipboard.SetDataObject(((TextBox)currentDomainUpDown.Controls[1]).SelectedText);
				((TextBox)currentDomainUpDown.Controls[1]).SelectedText = "";
			}
			else if (this.ActiveControl.GetType() == typeof(ComboBox))
			{
				ComboBox currentComboBox = (ComboBox)this.ActiveControl;
				Clipboard.SetDataObject(currentComboBox.SelectedText);
				currentComboBox.SelectedText = "";
				//return true;
			}
			else if (this.ActiveControl.GetType() == typeof(ModDisplayBox))
			{
				//return true;
			}
		}

        /// <summary>
        /// 
        /// </summary>
        public void DoCopy()
        {
            if (this.ActiveControl.GetType() == typeof(LocalisedTextBox))
            {
                LocalisedTextBox currentLocalisedTextBox = (LocalisedTextBox)this.ActiveControl;
                //currentLocalisedTextBox.DoCut();
            }
            else if (this.ActiveControl.GetType() == typeof(NumericUpDown))
            {
                NumericUpDown currentNumericUpDown = (NumericUpDown)this.ActiveControl;
                Clipboard.SetDataObject(((TextBox)currentNumericUpDown.Controls[1]).SelectedText);
            }
            else if (this.ActiveControl.GetType() == typeof(DomainUpDown))
            {
                DomainUpDown currentDomainUpDown = (DomainUpDown)this.ActiveControl;
                Clipboard.SetDataObject(((TextBox)currentDomainUpDown.Controls[1]).SelectedText);
            }
            else if (this.ActiveControl.GetType() == typeof(ComboBox))
            {
                ComboBox currentComboBox = (ComboBox)this.ActiveControl;
                Clipboard.SetDataObject(currentComboBox.SelectedText);
                //return true;
            }
            else if (this.ActiveControl.GetType() == typeof(ModDisplayBox))
            {
                //return true;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		public void DoPaste()
		{
            if (this.ActiveControl.GetType() == typeof(LocalisedTextBox))
            {
                LocalisedTextBox currentLocalisedTextBox = (LocalisedTextBox)this.ActiveControl;
                //currentLocalisedTextBox.DoCut();
            }
            else if (this.ActiveControl.GetType() == typeof(NumericUpDown))
            {
                NumericUpDown currentNumericUpDown = (NumericUpDown)this.ActiveControl;
                Clipboard.SetDataObject(((TextBox)currentNumericUpDown.Controls[1]).SelectedText);
                string clipData = Clipboard.GetText(TextDataFormat.Text);
                try
                {
                    currentNumericUpDown.Value = (decimal)int.Parse(clipData);
                }
                catch
                {
                }
            }
            else if (this.ActiveControl.GetType() == typeof(DomainUpDown))
            {
                DomainUpDown currentDomainUpDown = (DomainUpDown)this.ActiveControl;
                Clipboard.SetDataObject(((TextBox)currentDomainUpDown.Controls[1]).SelectedText);
                ((TextBox)currentDomainUpDown.Controls[1]).SelectedText = Clipboard.GetText(TextDataFormat.UnicodeText);
            }
            else if (this.ActiveControl.GetType() == typeof(ComboBox))
            {
                ComboBox currentComboBox = (ComboBox)this.ActiveControl;
                Clipboard.SetDataObject(currentComboBox.SelectedText);
                currentComboBox.SelectedText = Clipboard.GetText(TextDataFormat.UnicodeText);
                //return true;
            }
            else if (this.ActiveControl.GetType() == typeof(ModDisplayBox))
            {
                //return true;
            }
		}

		private void MODTitleTextBox_Enter(object sender, System.EventArgs e)
		{
			((Studio)this.MdiParent).UpdateCutCopyPaste(this, new EventArgs());
		}

		private void MODTitleTextBox_SelectionChanged(object sender, System.EventArgs e)
		{
			((Studio)this.MdiParent).UpdateCutCopyPaste(this, new EventArgs());
		}

		private void MODTitleTextBox_Leave(object sender, System.EventArgs e)
		{
			((Studio)this.MdiParent).UpdateCutCopyPaste(this, new EventArgs());
		}

		private void MODVersionMajorNumericUpDown_Click(object sender, System.EventArgs e)
		{
			((Studio)this.MdiParent).UpdateCutCopyPaste(this, new EventArgs());
		}

		private void ModEditor_KeyUp(object sender, KeyEventArgs e)
		{
			((Studio)this.MdiParent).UpdateCutCopyPaste(this, new EventArgs());
		}

		private void ModEditor_KeyPress(object sender, KeyPressEventArgs e)
		{
			((Studio)this.MdiParent).UpdateCutCopyPaste(this, new EventArgs());
		}

		private void MODVersionMajorNumericUpDown_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			((Studio)this.MdiParent).UpdateCutCopyPaste(this, new EventArgs());
		}

		private void labelOverviewTitle_DoubleClick(object sender, System.EventArgs e)
		{
			tabControlEditor.SelectedIndex = 1;
			MODTitleTextBox.Focus();
		}

        private void button10_Click(object sender, EventArgs e)
        {
            targetVersionDialog1.Reset();
            targetVersionDialog1.Cases = ThisMod.Header.PhpbbVersion;
            targetVersionDialog1.ShowDialog(this);
        }

        private void targetVersionDialog1_Save(object sender, TargetVersionDialogBoxSaveEventArgs e)
        {
            ThisMod.Header.PhpbbVersion = e.Cases;
            labelPhpbbVersion.Text = ThisMod.Header.PhpbbVersion.Primary;
            SetModified();
            this.Parent.Select();
        }

        private void ModVersionStageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetModified();
        }
	}
}
