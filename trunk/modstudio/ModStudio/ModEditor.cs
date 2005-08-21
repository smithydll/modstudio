/***************************************************************************
 *                              ModEditor.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: ModEditor.cs,v 1.6 2005-08-21 02:48:05 smithydll Exp $
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
using ModStudio;

namespace ModStudio
{
	/// <summary>
	/// Summary description for ModEditor.
	/// </summary>
	public class ModEditor : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TabControl tabControlEditor;
		private System.Windows.Forms.TabPage tabPageOverview;
		private System.Windows.Forms.TabPage tabPageHeader;
		private System.Windows.Forms.TabPage tabPageActions;
		private System.Windows.Forms.Label labelOverviewTitle;
		private System.Windows.Forms.PictureBox pictureBox1;
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
		private System.Windows.Forms.TextBox MODTitleTextBox;
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
		internal ModTemplateTools.PhpbbMod ThisMod;
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
		private OpenActionDialogBox openActionDialogBox1;
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
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// 
		/// </summary>
		public ModEditor()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			ThisMod = new PhpbbMod(".\\");

			ThisMod.Header.ModAuthor.AddEntry(new PhpbbMod.ModAuthorEntry("UserName", "RealName", "Email", "Homepage"));
			ThisMod.Header.ModVersion = new PhpbbMod.ModVersion(0,0,0);
			ThisMod.Actions.AddEntry(new PhpbbMod.ModAction("SAVE/CLOSE ALL FILES", "", "# EoM", ""));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filename"></param>
		public ModEditor(string filename):this()
		{
			ThisMod = new PhpbbMod(".\\");
			ThisMod.ReadFile(filename);
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ModEditor));
			this.tabControlEditor = new System.Windows.Forms.TabControl();
			this.tabPageOverview = new System.Windows.Forms.TabPage();
			this.button9 = new System.Windows.Forms.Button();
			this.listBoxFileEdits = new System.Windows.Forms.ListBox();
			this.button8 = new System.Windows.Forms.Button();
			this.labelModInstallationTime = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.labelOverviewTitle = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.labelIncludedFiles = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.tabPageHeader = new System.Windows.Forms.TabPage();
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
			this.MODTitleTextBox = new System.Windows.Forms.TextBox();
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
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.openActionDialogBox1 = new OpenActionDialogBox();
			this.tabControlEditor.SuspendLayout();
			this.tabPageOverview.SuspendLayout();
			this.tabPageHeader.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.MODVersionMajorNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.MODVersionMinorNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.MODVersionRevisionNumericUpDown)).BeginInit();
			this.tabPageActions.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControlEditor
			// 
			this.tabControlEditor.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
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
			this.tabPageOverview.BackColor = System.Drawing.Color.White;
			this.tabPageOverview.Controls.Add(this.button9);
			this.tabPageOverview.Controls.Add(this.listBoxFileEdits);
			this.tabPageOverview.Controls.Add(this.button8);
			this.tabPageOverview.Controls.Add(this.labelModInstallationTime);
			this.tabPageOverview.Controls.Add(this.label1);
			this.tabPageOverview.Controls.Add(this.pictureBox1);
			this.tabPageOverview.Controls.Add(this.labelOverviewTitle);
			this.tabPageOverview.Controls.Add(this.label4);
			this.tabPageOverview.Controls.Add(this.labelIncludedFiles);
			this.tabPageOverview.Controls.Add(this.label10);
			this.tabPageOverview.Location = new System.Drawing.Point(4, 25);
			this.tabPageOverview.Name = "tabPageOverview";
			this.tabPageOverview.Size = new System.Drawing.Size(784, 537);
			this.tabPageOverview.TabIndex = 0;
			this.tabPageOverview.Text = "Overview";
			// 
			// button9
			// 
			this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button9.Location = new System.Drawing.Point(56, 176);
			this.button9.Name = "button9";
			this.button9.TabIndex = 6;
			this.button9.Text = "Add File";
			this.button9.Click += new System.EventHandler(this.button9_Click);
			// 
			// listBoxFileEdits
			// 
			this.listBoxFileEdits.Location = new System.Drawing.Point(136, 144);
			this.listBoxFileEdits.Name = "listBoxFileEdits";
			this.listBoxFileEdits.Size = new System.Drawing.Size(456, 147);
			this.listBoxFileEdits.TabIndex = 5;
			this.listBoxFileEdits.DoubleClick += new System.EventHandler(this.listBoxFileEdits_DoubleClick);
			// 
			// button8
			// 
			this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button8.Location = new System.Drawing.Point(216, 108);
			this.button8.Name = "button8";
			this.button8.Size = new System.Drawing.Size(75, 20);
			this.button8.TabIndex = 4;
			this.button8.Text = "Edit";
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
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 80);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 23);
			this.label1.TabIndex = 2;
			this.label1.Text = "Installation Time";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(8, 48);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(48, 16);
			this.pictureBox1.TabIndex = 1;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
			// 
			// labelOverviewTitle
			// 
			this.labelOverviewTitle.AutoSize = true;
			this.labelOverviewTitle.Font = new System.Drawing.Font("Verdana", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelOverviewTitle.Location = new System.Drawing.Point(8, 8);
			this.labelOverviewTitle.Name = "labelOverviewTitle";
			this.labelOverviewTitle.Size = new System.Drawing.Size(219, 36);
			this.labelOverviewTitle.TabIndex = 0;
			this.labelOverviewTitle.Text = "{MOD TITLE}";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
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
			this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label10.Location = new System.Drawing.Point(8, 144);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(120, 23);
			this.label10.TabIndex = 2;
			this.label10.Text = "File Edits";
			this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tabPageHeader
			// 
			this.tabPageHeader.BackColor = System.Drawing.Color.LightYellow;
			this.tabPageHeader.Controls.Add(this.MODHistoryListView);
			this.tabPageHeader.Controls.Add(this.button1);
			this.tabPageHeader.Controls.Add(this.MODAuthorListBox);
			this.tabPageHeader.Controls.Add(this.MODInstallationLevelComboBox);
			this.tabPageHeader.Controls.Add(this.MODVersionReleaseDomainUpDown);
			this.tabPageHeader.Controls.Add(this.MODVersionMajorNumericUpDown);
			this.tabPageHeader.Controls.Add(this.Button16);
			this.tabPageHeader.Controls.Add(this.MODDescriptionLabel);
			this.tabPageHeader.Controls.Add(this.MODTitleTextBox);
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
			this.tabPageHeader.Location = new System.Drawing.Point(4, 25);
			this.tabPageHeader.Name = "tabPageHeader";
			this.tabPageHeader.Size = new System.Drawing.Size(784, 537);
			this.tabPageHeader.TabIndex = 1;
			this.tabPageHeader.Text = "Header";
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
			this.MODHistoryListView.Size = new System.Drawing.Size(448, 160);
			this.MODHistoryListView.TabIndex = 38;
			this.MODHistoryListView.View = System.Windows.Forms.View.Details;
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
			this.columnHeader3.Width = 300;
			// 
			// button1
			// 
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.button1.Location = new System.Drawing.Point(416, 144);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(32, 32);
			this.button1.TabIndex = 37;
			this.button1.Text = "Add";
			// 
			// MODAuthorListBox
			// 
			this.MODAuthorListBox.Location = new System.Drawing.Point(136, 144);
			this.MODAuthorListBox.Name = "MODAuthorListBox";
			this.MODAuthorListBox.Size = new System.Drawing.Size(272, 30);
			this.MODAuthorListBox.TabIndex = 31;
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
			this.MODInstallationLevelComboBox.SelectedIndexChanged += new System.EventHandler(this.MODInstallationLevelComboBox_SelectedIndexChanged);
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
			this.MODVersionReleaseDomainUpDown.Location = new System.Drawing.Point(288, 96);
			this.MODVersionReleaseDomainUpDown.Name = "MODVersionReleaseDomainUpDown";
			this.MODVersionReleaseDomainUpDown.Size = new System.Drawing.Size(48, 20);
			this.MODVersionReleaseDomainUpDown.Sorted = true;
			this.MODVersionReleaseDomainUpDown.TabIndex = 29;
			this.MODVersionReleaseDomainUpDown.SelectedItemChanged += new System.EventHandler(this.MODVersionReleaseDomainUpDown_SelectedItemChanged);
			// 
			// MODVersionMajorNumericUpDown
			// 
			this.MODVersionMajorNumericUpDown.Location = new System.Drawing.Point(136, 96);
			this.MODVersionMajorNumericUpDown.Name = "MODVersionMajorNumericUpDown";
			this.MODVersionMajorNumericUpDown.Size = new System.Drawing.Size(40, 20);
			this.MODVersionMajorNumericUpDown.TabIndex = 26;
			this.MODVersionMajorNumericUpDown.ValueChanged += new System.EventHandler(this.MODVersionMajorNumericUpDown_ValueChanged);
			// 
			// Button16
			// 
			this.Button16.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.Button16.Location = new System.Drawing.Point(104, 48);
			this.Button16.Name = "Button16";
			this.Button16.Size = new System.Drawing.Size(24, 16);
			this.Button16.TabIndex = 25;
			this.Button16.Text = "...";
			this.Button16.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// MODDescriptionLabel
			// 
			this.MODDescriptionLabel.Location = new System.Drawing.Point(136, 32);
			this.MODDescriptionLabel.Name = "MODDescriptionLabel";
			this.MODDescriptionLabel.Size = new System.Drawing.Size(448, 64);
			this.MODDescriptionLabel.TabIndex = 23;
			// 
			// MODTitleTextBox
			// 
			this.MODTitleTextBox.Location = new System.Drawing.Point(136, 8);
			this.MODTitleTextBox.Name = "MODTitleTextBox";
			this.MODTitleTextBox.Size = new System.Drawing.Size(448, 20);
			this.MODTitleTextBox.TabIndex = 20;
			this.MODTitleTextBox.Text = "";
			this.MODTitleTextBox.TextChanged += new System.EventHandler(this.MODTitleTextBox_TextChanged);
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(8, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(120, 23);
			this.label2.TabIndex = 14;
			this.label2.Text = "MOD Title:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(8, 32);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(120, 23);
			this.label3.TabIndex = 12;
			this.label3.Text = "MOD Description:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
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
			this.MODVersionMinorNumericUpDown.ValueChanged += new System.EventHandler(this.MODVersionMinorNumericUpDown_ValueChanged);
			// 
			// MODVersionRevisionNumericUpDown
			// 
			this.MODVersionRevisionNumericUpDown.Location = new System.Drawing.Point(232, 96);
			this.MODVersionRevisionNumericUpDown.Name = "MODVersionRevisionNumericUpDown";
			this.MODVersionRevisionNumericUpDown.Size = new System.Drawing.Size(48, 20);
			this.MODVersionRevisionNumericUpDown.TabIndex = 28;
			this.MODVersionRevisionNumericUpDown.ValueChanged += new System.EventHandler(this.MODVersionRevisionNumericUpDown_ValueChanged);
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label6.Location = new System.Drawing.Point(8, 120);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(120, 23);
			this.label6.TabIndex = 13;
			this.label6.Text = "Installation Level:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label7.Location = new System.Drawing.Point(8, 144);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(120, 23);
			this.label7.TabIndex = 15;
			this.label7.Text = "MOD Authors:";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// button2
			// 
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.button2.Location = new System.Drawing.Point(456, 144);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(40, 32);
			this.button2.TabIndex = 35;
			this.button2.Text = "Edit";
			// 
			// button3
			// 
			this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.button3.Location = new System.Drawing.Point(504, 144);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(48, 32);
			this.button3.TabIndex = 36;
			this.button3.Text = "Delete";
			// 
			// MODAuthorNotesLabel
			// 
			this.MODAuthorNotesLabel.Location = new System.Drawing.Point(136, 176);
			this.MODAuthorNotesLabel.Name = "MODAuthorNotesLabel";
			this.MODAuthorNotesLabel.Size = new System.Drawing.Size(448, 104);
			this.MODAuthorNotesLabel.TabIndex = 22;
			// 
			// button4
			// 
			this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.button4.Location = new System.Drawing.Point(104, 192);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(24, 16);
			this.button4.TabIndex = 24;
			this.button4.Text = "...";
			this.button4.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label9.Location = new System.Drawing.Point(8, 176);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(120, 23);
			this.label9.TabIndex = 18;
			this.label9.Text = "Author Notes:";
			this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// MODInstallationTimeLabel
			// 
			this.MODInstallationTimeLabel.Location = new System.Drawing.Point(136, 288);
			this.MODInstallationTimeLabel.Name = "MODInstallationTimeLabel";
			this.MODInstallationTimeLabel.Size = new System.Drawing.Size(104, 16);
			this.MODInstallationTimeLabel.TabIndex = 21;
			// 
			// label11
			// 
			this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label11.Location = new System.Drawing.Point(8, 288);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(120, 23);
			this.label11.TabIndex = 16;
			this.label11.Text = "Installation Time:";
			this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label12
			// 
			this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label12.Location = new System.Drawing.Point(8, 320);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(120, 23);
			this.label12.TabIndex = 17;
			this.label12.Text = "MOD History:";
			this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// button5
			// 
			this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.button5.Location = new System.Drawing.Point(8, 352);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(120, 20);
			this.button5.TabIndex = 32;
			this.button5.Text = "Add";
			// 
			// button6
			// 
			this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.button6.Location = new System.Drawing.Point(8, 376);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(120, 20);
			this.button6.TabIndex = 34;
			this.button6.Text = "Edit";
			// 
			// button7
			// 
			this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.button7.Location = new System.Drawing.Point(8, 400);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(120, 20);
			this.button7.TabIndex = 33;
			this.button7.Text = "Delete";
			// 
			// tabPageActions
			// 
			this.tabPageActions.BackColor = System.Drawing.SystemColors.Control;
			this.tabPageActions.Controls.Add(this.modDisplayBox2);
			this.tabPageActions.Controls.Add(this.toolBar1);
			this.tabPageActions.Location = new System.Drawing.Point(4, 25);
			this.tabPageActions.Name = "tabPageActions";
			this.tabPageActions.Size = new System.Drawing.Size(784, 537);
			this.tabPageActions.TabIndex = 2;
			this.tabPageActions.Text = "Actions";
			// 
			// modDisplayBox2
			// 
			this.modDisplayBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.modDisplayBox2.Location = new System.Drawing.Point(0, 28);
			this.modDisplayBox2.Name = "modDisplayBox2";
			this.modDisplayBox2.SelectedIndex = 0;
			this.modDisplayBox2.Size = new System.Drawing.Size(784, 509);
			this.modDisplayBox2.TabIndex = 0;
			this.modDisplayBox2.Click += new System.EventHandler(this.modDisplayBox2_Click);
			this.modDisplayBox2.ItemDoubleClick += new ModFormControls.ModActionItem.ActionItemClickHandler(this.modDisplayBox2_ItemDoubleClick);
			this.modDisplayBox2.SelectedIndexChanged += new System.EventHandler(this.modDisplayBox2_SelectedIndexChanged);
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
			this.toolBar1.Location = new System.Drawing.Point(0, 0);
			this.toolBar1.Name = "toolBar1";
			this.toolBar1.ShowToolTips = true;
			this.toolBar1.Size = new System.Drawing.Size(784, 28);
			this.toolBar1.TabIndex = 1;
			this.toolBar1.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
			this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
			// 
			// toolBarButtonEditAction
			// 
			this.toolBarButtonEditAction.Text = "Edit";
			// 
			// toolBarButtonAddAction
			// 
			this.toolBarButtonAddAction.DropDownMenu = this.contextMenuAddAction;
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
			this.menuItemAddActionDiyInstruction.Text = "DIY INSTRUCTION";
			this.menuItemAddActionDiyInstruction.Click += new System.EventHandler(this.menuItemAddActionDiyInstruction_Click);
			// 
			// toolBarButtonDelete
			// 
			this.toolBarButtonDelete.Text = "Delete Action";
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.FileName = "untitled.mod";
			this.saveFileDialog1.Filter = "phpBB2 Mod Files (*.mod,*.txt)|*.mod;*.txt";
			this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
			//
			// openActionDialogBox1
			//
			this.openActionDialogBox1.SaveNew += new ModStudio.OpenActionDialogBox.OpenActionDialogBoxSaveNewHandler(openActionDialogBox1_SaveNew);
			// 
			// ModEditor
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(792, 566);
			this.Controls.Add(this.tabControlEditor);
			this.Name = "ModEditor";
			this.Text = "untitled";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Resize += new System.EventHandler(this.ModEditor_Resize);
			this.Load += new System.EventHandler(this.ModEditor_Load);
			this.tabControlEditor.ResumeLayout(false);
			this.tabPageOverview.ResumeLayout(false);
			this.tabPageHeader.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.MODVersionMajorNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.MODVersionMinorNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.MODVersionRevisionNumericUpDown)).EndInit();
			this.tabPageActions.ResumeLayout(false);
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

		private int LastSelectedIndex = 0;
		private void tabControlEditor_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			switch (LastSelectedIndex)
			{
				case 0: // Overview
					break;
				case 1: // Header
					ThisMod.Header.ModTitle.SetValue(MODTitleTextBox.Text);
					if (MODVersionReleaseDomainUpDown.Text.ToCharArray().Length > 0)
					{
						ThisMod.Header.ModVersion = new PhpbbMod.ModVersion((int)MODVersionMajorNumericUpDown.Value, (int)MODVersionMinorNumericUpDown.Value, (int)MODVersionRevisionNumericUpDown.Value, MODVersionReleaseDomainUpDown.Text.ToCharArray()[0]);
					}
					else
					{
						ThisMod.Header.ModVersion = new PhpbbMod.ModVersion((int)MODVersionMajorNumericUpDown.Value, (int)MODVersionMinorNumericUpDown.Value, (int)MODVersionRevisionNumericUpDown.Value);
					}
					ThisMod.Header.ModInstallationLevel = PhpbbMod.ModInstallationLevelParse(MODInstallationLevelComboBox.Text);
					break;
				case 2: // Actions
					modDisplayBox2.ModActions = ThisMod.Actions;
					break;
			}

			switch (tabControlEditor.SelectedIndex)
			{
				case 0: // Overview
					labelOverviewTitle.Text = ThisMod.Header.ModTitle.pValue;
					labelModInstallationTime.Text = string.Format("{0} minutes", (ThisMod.Header.ModInstallationTime / 60));
					listBoxFileEdits.Items.Clear();
					if (ThisMod.Actions.Actions != null)
					{
						foreach (PhpbbMod.ModAction a in ThisMod.Actions.Actions)
						{
							char[] TrimChars = {' ', '\t', '\n', '\r'};
							if (a.ActionType == "OPEN")
							{
								listBoxFileEdits.Items.Add(a.ActionBody.Trim(TrimChars));
							}
						}
					}
					if (ThisMod.Header.ModIncludedFiles != null)
					{
						labelIncludedFiles.Text = string.Format("{0} files", (ThisMod.Header.ModIncludedFiles.Length));
					}
					else
					{
						labelIncludedFiles.Text = "0 files";
					}
					break;
				case 1: // Header
					MODTitleTextBox.Text = ThisMod.Header.ModTitle.pValue;
					MODAuthorListBox.Items.Clear();
					foreach (PhpbbMod.ModAuthorEntry a in ThisMod.Header.ModAuthor.Authors)
					{
						MODAuthorListBox.Items.Add(a.UserName);
					}
					MODInstallationLevelComboBox.Text = ThisMod.Header.ModInstallationLevel.ToString();
					MODAuthorNotesLabel.Text = ThisMod.Header.ModAuthorNotes.ToString();
					break;
				case 2: // Actions
					modDisplayBox2.ModActions = ThisMod.Actions;
					modDisplayBox2.UpdateLayout();
					modDisplayBox2.Select();
					break;
			}

			LastSelectedIndex = tabControlEditor.SelectedIndex;
		}

		private void ModEditor_Load(object sender, System.EventArgs e)
		{
			tabControlEditor.SelectedIndex = 0;
			tabControlEditor_SelectedIndexChanged(null,null);
			modDisplayBox2_SelectedIndexChanged(null, null);
		}

		private void listBoxFileEdits_DoubleClick(object sender, System.EventArgs e)
		{
			tabControlEditor.SelectedIndex = 2;
			for (int i = 0; i < ThisMod.Actions.Actions.Length; i++)
			{
				char[] TrimChars = {' ', '\t', '\n', '\r'};
				if (ThisMod.Actions.Actions[i].ActionType == "OPEN")
				{
					if (ThisMod.Actions.Actions[i].ActionBody.Trim(TrimChars) == listBoxFileEdits.Items[listBoxFileEdits.SelectedIndex].ToString())
					{
						modDisplayBox2.SelectedIndex = i;
					}
				}
			}
			modDisplayBox2.UpdateColours();
		}

		private void ModEditor_Resize(object sender, System.EventArgs e)
		{
			if (tabControlEditor.SelectedIndex == 2)
			{
				modDisplayBox2.UpdateSize();
			}
		}

		private void modDisplayBox2_ItemDoubleClick(object sender, ModFormControls.ActionItemClickEventArgs e)
		{
			ModFormControls.ModActionEditor abc = new ModFormControls.ModActionEditor();
			abc.Location = new Point(10,10);
			abc.Size = new Size(tabPageActions.Width - 20 - 17, tabPageActions.Height - 20);
			abc.Visible = true;

			this.tabPageActions.Controls.Add(abc);

			abc.Visible = true;
			abc.BringToFront();
		}

		private void modDisplayBox2_Click(object sender, System.EventArgs e)
		{
			modDisplayBox2.Select();
		}

		/// <summary>
		/// 
		/// </summary>
		public void SaveFile()
		{
			if (this.Text.EndsWith("*")) // the file has been modified
			{
				if (this.Text.StartsWith("untitled")) // hasn't been saved before
				{
					SaveFileAs();
				}
				else
				{
					ThisMod.WriteFile(this.Text.Substring(0, this.Text.Length - 1));
					SetUnmodified();
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void SaveFileAs()
		{
			saveFileDialog1.ShowDialog(this);
		}

		private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			ThisMod.WriteFile(saveFileDialog1.FileName);
			this.Text = saveFileDialog1.FileName;
			SetUnmodified();
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
					if (ThisMod.Actions.Actions[modDisplayBox2.SelectedIndex].ActionType != "SAVE/CLOSE ALL FILES")
					{
						ThisMod.Actions.RemoveEntry(modDisplayBox2.SelectedIndex);
						modDisplayBox2.ModActions = ThisMod.Actions;
						modDisplayBox2.UpdateLayout();
						modDisplayBox2.Select();
					}
					break;
			}
		}

		private void modDisplayBox2_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			switch (ThisMod.Actions.Actions[modDisplayBox2.SelectedIndex].ActionType)
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
			openActionDialogBox1.ShowDialog(this);
		}

		private void AddAction(string actionType)
		{
			AddAction(actionType, "");
		}

		private void menuItemAddActionDiyInstruction_Click(object sender, System.EventArgs e)
		{
			AddAction("DIY INSTRUCTION");
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
			if (addBefore + 1 < ThisMod.Actions.Actions.Length) 
			{
				addBefore++;
			}
			ThisMod.Actions.AddEntry(new ModTemplateTools.PhpbbMod.ModAction(actionType, actionBody, ""), addBefore);
			modDisplayBox2.ModActions = ThisMod.Actions;
			modDisplayBox2.UpdateLayout();
			modDisplayBox2.Select();
			modDisplayBox2.SelectedIndex = addBefore;
		}

		private void openActionDialogBox1_SaveNew(object sender, OpenActionDialogBoxSaveNewEventArgs e)
		{
			if (tabControlEditor.SelectedIndex == 2)
			{
				AddAction("OPEN", e.FileName);
			}
			else
			{
				ThisMod.Actions.AddEntry(new PhpbbMod.ModAction("OPEN", e.FileName, ""), ThisMod.Actions.Actions.GetUpperBound(0));
			}
		}

		private void button9_Click(object sender, System.EventArgs e)
		{
			openActionDialogBox1.ShowDialog(this);
			tabControlEditor_SelectedIndexChanged(null, null);
		}
	}
}
