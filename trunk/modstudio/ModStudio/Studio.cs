/***************************************************************************
 *                              Studio.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: Studio.cs,v 1.6 2005-08-27 12:10:47 smithydll Exp $
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
using System.Data;

namespace ModStudio
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Studio : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MenuItem menuItemFile;
		private System.Windows.Forms.MenuItem menuItemEdit;
		private System.Windows.Forms.MenuItem menuItemFileNew;
		private System.Windows.Forms.MenuItem menuItemFileOpen;
		private System.Windows.Forms.MenuItem menuItemFileClose;
		private System.Windows.Forms.MenuItem menuItemSep1;
		private System.Windows.Forms.MenuItem menuItemFileSave;
		private System.Windows.Forms.MenuItem menuItemFileSaveAs;
		private System.Windows.Forms.MenuItem menuItemFileSaveAll;
		private System.Windows.Forms.MenuItem menuItemSep2;
		private System.Windows.Forms.MenuItem menuItemFileExit;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem menuItemSep3;
		private System.Windows.Forms.MenuItem menuItemFileValidate;
		private System.Windows.Forms.MenuItem menuItemFileExport;
		private System.Windows.Forms.MenuItem menuItemView;
		private System.Windows.Forms.MenuItem menuItemTools;
		private System.Windows.Forms.MenuItem menuItemWindow;
		private System.Windows.Forms.MenuItem menuItemHelp;
		private System.Windows.Forms.MenuItem menuItemHelpHelp;
		private System.Windows.Forms.MenuItem menuItemSep4;
		private System.Windows.Forms.MenuItem menuItemHelpAbout;
		private System.Windows.Forms.MenuItem menuItemToolsOptions;
		private ModStudio.ModEditor[] ModEditors;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.ToolBar toolBar1;
		private System.Windows.Forms.ToolBarButton toolBarButtonNew;
		private System.Windows.Forms.ToolBarButton toolBarButtonOpen;
		private System.Windows.Forms.ToolBarButton toolBarButtonSave;
		private System.Windows.Forms.ToolBarButton toolBarButtonSaveAll;
		private System.Windows.Forms.ImageList imageList1;
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// 
		/// </summary>
		public Studio()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			ModEditors = new ModEditor[1];
			ModEditors[0] = new ModEditor();
			ModEditors[0].ThisMod.Header.ModTitle.AddLanguage("Untitled Mod", "en-GB");
			ModEditors[0].MdiParent = this;
			ModEditors[0].Show();

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Studio));
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.menuItemFile = new System.Windows.Forms.MenuItem();
			this.menuItemFileNew = new System.Windows.Forms.MenuItem();
			this.menuItemFileOpen = new System.Windows.Forms.MenuItem();
			this.menuItemFileClose = new System.Windows.Forms.MenuItem();
			this.menuItemSep1 = new System.Windows.Forms.MenuItem();
			this.menuItemFileSave = new System.Windows.Forms.MenuItem();
			this.menuItemFileSaveAs = new System.Windows.Forms.MenuItem();
			this.menuItemFileSaveAll = new System.Windows.Forms.MenuItem();
			this.menuItemSep3 = new System.Windows.Forms.MenuItem();
			this.menuItemFileExport = new System.Windows.Forms.MenuItem();
			this.menuItemFileValidate = new System.Windows.Forms.MenuItem();
			this.menuItemSep2 = new System.Windows.Forms.MenuItem();
			this.menuItemFileExit = new System.Windows.Forms.MenuItem();
			this.menuItemEdit = new System.Windows.Forms.MenuItem();
			this.menuItemView = new System.Windows.Forms.MenuItem();
			this.menuItemTools = new System.Windows.Forms.MenuItem();
			this.menuItemToolsOptions = new System.Windows.Forms.MenuItem();
			this.menuItemWindow = new System.Windows.Forms.MenuItem();
			this.menuItemHelp = new System.Windows.Forms.MenuItem();
			this.menuItemHelpHelp = new System.Windows.Forms.MenuItem();
			this.menuItemSep4 = new System.Windows.Forms.MenuItem();
			this.menuItemHelpAbout = new System.Windows.Forms.MenuItem();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.toolBar1 = new System.Windows.Forms.ToolBar();
			this.toolBarButtonNew = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonOpen = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSave = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSaveAll = new System.Windows.Forms.ToolBarButton();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.menuItemFile,
																					 this.menuItemEdit,
																					 this.menuItemView,
																					 this.menuItemTools,
																					 this.menuItemWindow,
																					 this.menuItemHelp});
			this.mainMenu.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mainMenu.RightToLeft")));
			// 
			// menuItemFile
			// 
			this.menuItemFile.Enabled = ((bool)(resources.GetObject("menuItemFile.Enabled")));
			this.menuItemFile.Index = 0;
			this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItemFileNew,
																						 this.menuItemFileOpen,
																						 this.menuItemFileClose,
																						 this.menuItemSep1,
																						 this.menuItemFileSave,
																						 this.menuItemFileSaveAs,
																						 this.menuItemFileSaveAll,
																						 this.menuItemSep3,
																						 this.menuItemFileExport,
																						 this.menuItemFileValidate,
																						 this.menuItemSep2,
																						 this.menuItemFileExit});
			this.menuItemFile.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemFile.Shortcut")));
			this.menuItemFile.ShowShortcut = ((bool)(resources.GetObject("menuItemFile.ShowShortcut")));
			this.menuItemFile.Text = resources.GetString("menuItemFile.Text");
			this.menuItemFile.Visible = ((bool)(resources.GetObject("menuItemFile.Visible")));
			// 
			// menuItemFileNew
			// 
			this.menuItemFileNew.Enabled = ((bool)(resources.GetObject("menuItemFileNew.Enabled")));
			this.menuItemFileNew.Index = 0;
			this.menuItemFileNew.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemFileNew.Shortcut")));
			this.menuItemFileNew.ShowShortcut = ((bool)(resources.GetObject("menuItemFileNew.ShowShortcut")));
			this.menuItemFileNew.Text = resources.GetString("menuItemFileNew.Text");
			this.menuItemFileNew.Visible = ((bool)(resources.GetObject("menuItemFileNew.Visible")));
			// 
			// menuItemFileOpen
			// 
			this.menuItemFileOpen.Enabled = ((bool)(resources.GetObject("menuItemFileOpen.Enabled")));
			this.menuItemFileOpen.Index = 1;
			this.menuItemFileOpen.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemFileOpen.Shortcut")));
			this.menuItemFileOpen.ShowShortcut = ((bool)(resources.GetObject("menuItemFileOpen.ShowShortcut")));
			this.menuItemFileOpen.Text = resources.GetString("menuItemFileOpen.Text");
			this.menuItemFileOpen.Visible = ((bool)(resources.GetObject("menuItemFileOpen.Visible")));
			this.menuItemFileOpen.Click += new System.EventHandler(this.menuItemFileOpen_Click);
			// 
			// menuItemFileClose
			// 
			this.menuItemFileClose.Enabled = ((bool)(resources.GetObject("menuItemFileClose.Enabled")));
			this.menuItemFileClose.Index = 2;
			this.menuItemFileClose.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemFileClose.Shortcut")));
			this.menuItemFileClose.ShowShortcut = ((bool)(resources.GetObject("menuItemFileClose.ShowShortcut")));
			this.menuItemFileClose.Text = resources.GetString("menuItemFileClose.Text");
			this.menuItemFileClose.Visible = ((bool)(resources.GetObject("menuItemFileClose.Visible")));
			// 
			// menuItemSep1
			// 
			this.menuItemSep1.Enabled = ((bool)(resources.GetObject("menuItemSep1.Enabled")));
			this.menuItemSep1.Index = 3;
			this.menuItemSep1.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemSep1.Shortcut")));
			this.menuItemSep1.ShowShortcut = ((bool)(resources.GetObject("menuItemSep1.ShowShortcut")));
			this.menuItemSep1.Text = resources.GetString("menuItemSep1.Text");
			this.menuItemSep1.Visible = ((bool)(resources.GetObject("menuItemSep1.Visible")));
			// 
			// menuItemFileSave
			// 
			this.menuItemFileSave.Enabled = ((bool)(resources.GetObject("menuItemFileSave.Enabled")));
			this.menuItemFileSave.Index = 4;
			this.menuItemFileSave.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemFileSave.Shortcut")));
			this.menuItemFileSave.ShowShortcut = ((bool)(resources.GetObject("menuItemFileSave.ShowShortcut")));
			this.menuItemFileSave.Text = resources.GetString("menuItemFileSave.Text");
			this.menuItemFileSave.Visible = ((bool)(resources.GetObject("menuItemFileSave.Visible")));
			this.menuItemFileSave.Click += new System.EventHandler(this.menuItemFileSave_Click);
			// 
			// menuItemFileSaveAs
			// 
			this.menuItemFileSaveAs.Enabled = ((bool)(resources.GetObject("menuItemFileSaveAs.Enabled")));
			this.menuItemFileSaveAs.Index = 5;
			this.menuItemFileSaveAs.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemFileSaveAs.Shortcut")));
			this.menuItemFileSaveAs.ShowShortcut = ((bool)(resources.GetObject("menuItemFileSaveAs.ShowShortcut")));
			this.menuItemFileSaveAs.Text = resources.GetString("menuItemFileSaveAs.Text");
			this.menuItemFileSaveAs.Visible = ((bool)(resources.GetObject("menuItemFileSaveAs.Visible")));
			this.menuItemFileSaveAs.Click += new System.EventHandler(this.menuItemFileSaveAs_Click);
			// 
			// menuItemFileSaveAll
			// 
			this.menuItemFileSaveAll.Enabled = ((bool)(resources.GetObject("menuItemFileSaveAll.Enabled")));
			this.menuItemFileSaveAll.Index = 6;
			this.menuItemFileSaveAll.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemFileSaveAll.Shortcut")));
			this.menuItemFileSaveAll.ShowShortcut = ((bool)(resources.GetObject("menuItemFileSaveAll.ShowShortcut")));
			this.menuItemFileSaveAll.Text = resources.GetString("menuItemFileSaveAll.Text");
			this.menuItemFileSaveAll.Visible = ((bool)(resources.GetObject("menuItemFileSaveAll.Visible")));
			this.menuItemFileSaveAll.Click += new System.EventHandler(this.menuItemFileSaveAll_Click);
			// 
			// menuItemSep3
			// 
			this.menuItemSep3.Enabled = ((bool)(resources.GetObject("menuItemSep3.Enabled")));
			this.menuItemSep3.Index = 7;
			this.menuItemSep3.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemSep3.Shortcut")));
			this.menuItemSep3.ShowShortcut = ((bool)(resources.GetObject("menuItemSep3.ShowShortcut")));
			this.menuItemSep3.Text = resources.GetString("menuItemSep3.Text");
			this.menuItemSep3.Visible = ((bool)(resources.GetObject("menuItemSep3.Visible")));
			// 
			// menuItemFileExport
			// 
			this.menuItemFileExport.Enabled = ((bool)(resources.GetObject("menuItemFileExport.Enabled")));
			this.menuItemFileExport.Index = 8;
			this.menuItemFileExport.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemFileExport.Shortcut")));
			this.menuItemFileExport.ShowShortcut = ((bool)(resources.GetObject("menuItemFileExport.ShowShortcut")));
			this.menuItemFileExport.Text = resources.GetString("menuItemFileExport.Text");
			this.menuItemFileExport.Visible = ((bool)(resources.GetObject("menuItemFileExport.Visible")));
			// 
			// menuItemFileValidate
			// 
			this.menuItemFileValidate.Enabled = ((bool)(resources.GetObject("menuItemFileValidate.Enabled")));
			this.menuItemFileValidate.Index = 9;
			this.menuItemFileValidate.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemFileValidate.Shortcut")));
			this.menuItemFileValidate.ShowShortcut = ((bool)(resources.GetObject("menuItemFileValidate.ShowShortcut")));
			this.menuItemFileValidate.Text = resources.GetString("menuItemFileValidate.Text");
			this.menuItemFileValidate.Visible = ((bool)(resources.GetObject("menuItemFileValidate.Visible")));
			// 
			// menuItemSep2
			// 
			this.menuItemSep2.Enabled = ((bool)(resources.GetObject("menuItemSep2.Enabled")));
			this.menuItemSep2.Index = 10;
			this.menuItemSep2.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemSep2.Shortcut")));
			this.menuItemSep2.ShowShortcut = ((bool)(resources.GetObject("menuItemSep2.ShowShortcut")));
			this.menuItemSep2.Text = resources.GetString("menuItemSep2.Text");
			this.menuItemSep2.Visible = ((bool)(resources.GetObject("menuItemSep2.Visible")));
			// 
			// menuItemFileExit
			// 
			this.menuItemFileExit.Enabled = ((bool)(resources.GetObject("menuItemFileExit.Enabled")));
			this.menuItemFileExit.Index = 11;
			this.menuItemFileExit.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemFileExit.Shortcut")));
			this.menuItemFileExit.ShowShortcut = ((bool)(resources.GetObject("menuItemFileExit.ShowShortcut")));
			this.menuItemFileExit.Text = resources.GetString("menuItemFileExit.Text");
			this.menuItemFileExit.Visible = ((bool)(resources.GetObject("menuItemFileExit.Visible")));
			this.menuItemFileExit.Click += new System.EventHandler(this.menuItemFileExit_Click);
			// 
			// menuItemEdit
			// 
			this.menuItemEdit.Enabled = ((bool)(resources.GetObject("menuItemEdit.Enabled")));
			this.menuItemEdit.Index = 1;
			this.menuItemEdit.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemEdit.Shortcut")));
			this.menuItemEdit.ShowShortcut = ((bool)(resources.GetObject("menuItemEdit.ShowShortcut")));
			this.menuItemEdit.Text = resources.GetString("menuItemEdit.Text");
			this.menuItemEdit.Visible = ((bool)(resources.GetObject("menuItemEdit.Visible")));
			// 
			// menuItemView
			// 
			this.menuItemView.Enabled = ((bool)(resources.GetObject("menuItemView.Enabled")));
			this.menuItemView.Index = 2;
			this.menuItemView.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemView.Shortcut")));
			this.menuItemView.ShowShortcut = ((bool)(resources.GetObject("menuItemView.ShowShortcut")));
			this.menuItemView.Text = resources.GetString("menuItemView.Text");
			this.menuItemView.Visible = ((bool)(resources.GetObject("menuItemView.Visible")));
			// 
			// menuItemTools
			// 
			this.menuItemTools.Enabled = ((bool)(resources.GetObject("menuItemTools.Enabled")));
			this.menuItemTools.Index = 3;
			this.menuItemTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.menuItemToolsOptions});
			this.menuItemTools.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemTools.Shortcut")));
			this.menuItemTools.ShowShortcut = ((bool)(resources.GetObject("menuItemTools.ShowShortcut")));
			this.menuItemTools.Text = resources.GetString("menuItemTools.Text");
			this.menuItemTools.Visible = ((bool)(resources.GetObject("menuItemTools.Visible")));
			// 
			// menuItemToolsOptions
			// 
			this.menuItemToolsOptions.Enabled = ((bool)(resources.GetObject("menuItemToolsOptions.Enabled")));
			this.menuItemToolsOptions.Index = 0;
			this.menuItemToolsOptions.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemToolsOptions.Shortcut")));
			this.menuItemToolsOptions.ShowShortcut = ((bool)(resources.GetObject("menuItemToolsOptions.ShowShortcut")));
			this.menuItemToolsOptions.Text = resources.GetString("menuItemToolsOptions.Text");
			this.menuItemToolsOptions.Visible = ((bool)(resources.GetObject("menuItemToolsOptions.Visible")));
			// 
			// menuItemWindow
			// 
			this.menuItemWindow.Enabled = ((bool)(resources.GetObject("menuItemWindow.Enabled")));
			this.menuItemWindow.Index = 4;
			this.menuItemWindow.MdiList = true;
			this.menuItemWindow.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemWindow.Shortcut")));
			this.menuItemWindow.ShowShortcut = ((bool)(resources.GetObject("menuItemWindow.ShowShortcut")));
			this.menuItemWindow.Text = resources.GetString("menuItemWindow.Text");
			this.menuItemWindow.Visible = ((bool)(resources.GetObject("menuItemWindow.Visible")));
			// 
			// menuItemHelp
			// 
			this.menuItemHelp.Enabled = ((bool)(resources.GetObject("menuItemHelp.Enabled")));
			this.menuItemHelp.Index = 5;
			this.menuItemHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItemHelpHelp,
																						 this.menuItemSep4,
																						 this.menuItemHelpAbout});
			this.menuItemHelp.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemHelp.Shortcut")));
			this.menuItemHelp.ShowShortcut = ((bool)(resources.GetObject("menuItemHelp.ShowShortcut")));
			this.menuItemHelp.Text = resources.GetString("menuItemHelp.Text");
			this.menuItemHelp.Visible = ((bool)(resources.GetObject("menuItemHelp.Visible")));
			// 
			// menuItemHelpHelp
			// 
			this.menuItemHelpHelp.Enabled = ((bool)(resources.GetObject("menuItemHelpHelp.Enabled")));
			this.menuItemHelpHelp.Index = 0;
			this.menuItemHelpHelp.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemHelpHelp.Shortcut")));
			this.menuItemHelpHelp.ShowShortcut = ((bool)(resources.GetObject("menuItemHelpHelp.ShowShortcut")));
			this.menuItemHelpHelp.Text = resources.GetString("menuItemHelpHelp.Text");
			this.menuItemHelpHelp.Visible = ((bool)(resources.GetObject("menuItemHelpHelp.Visible")));
			// 
			// menuItemSep4
			// 
			this.menuItemSep4.Enabled = ((bool)(resources.GetObject("menuItemSep4.Enabled")));
			this.menuItemSep4.Index = 1;
			this.menuItemSep4.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemSep4.Shortcut")));
			this.menuItemSep4.ShowShortcut = ((bool)(resources.GetObject("menuItemSep4.ShowShortcut")));
			this.menuItemSep4.Text = resources.GetString("menuItemSep4.Text");
			this.menuItemSep4.Visible = ((bool)(resources.GetObject("menuItemSep4.Visible")));
			// 
			// menuItemHelpAbout
			// 
			this.menuItemHelpAbout.Enabled = ((bool)(resources.GetObject("menuItemHelpAbout.Enabled")));
			this.menuItemHelpAbout.Index = 2;
			this.menuItemHelpAbout.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemHelpAbout.Shortcut")));
			this.menuItemHelpAbout.ShowShortcut = ((bool)(resources.GetObject("menuItemHelpAbout.ShowShortcut")));
			this.menuItemHelpAbout.Text = resources.GetString("menuItemHelpAbout.Text");
			this.menuItemHelpAbout.Visible = ((bool)(resources.GetObject("menuItemHelpAbout.Visible")));
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.Filter = resources.GetString("openFileDialog1.Filter");
			this.openFileDialog1.Title = resources.GetString("openFileDialog1.Title");
			this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
			// 
			// toolBar1
			// 
			this.toolBar1.AccessibleDescription = resources.GetString("toolBar1.AccessibleDescription");
			this.toolBar1.AccessibleName = resources.GetString("toolBar1.AccessibleName");
			this.toolBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("toolBar1.Anchor")));
			this.toolBar1.Appearance = ((System.Windows.Forms.ToolBarAppearance)(resources.GetObject("toolBar1.Appearance")));
			this.toolBar1.AutoSize = ((bool)(resources.GetObject("toolBar1.AutoSize")));
			this.toolBar1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("toolBar1.BackgroundImage")));
			this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						this.toolBarButtonNew,
																						this.toolBarButtonOpen,
																						this.toolBarButtonSave,
																						this.toolBarButtonSaveAll});
			this.toolBar1.ButtonSize = ((System.Drawing.Size)(resources.GetObject("toolBar1.ButtonSize")));
			this.toolBar1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("toolBar1.Dock")));
			this.toolBar1.DropDownArrows = ((bool)(resources.GetObject("toolBar1.DropDownArrows")));
			this.toolBar1.Enabled = ((bool)(resources.GetObject("toolBar1.Enabled")));
			this.toolBar1.Font = ((System.Drawing.Font)(resources.GetObject("toolBar1.Font")));
			this.toolBar1.ImageList = this.imageList1;
			this.toolBar1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("toolBar1.ImeMode")));
			this.toolBar1.Location = ((System.Drawing.Point)(resources.GetObject("toolBar1.Location")));
			this.toolBar1.Name = "toolBar1";
			this.toolBar1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("toolBar1.RightToLeft")));
			this.toolBar1.ShowToolTips = ((bool)(resources.GetObject("toolBar1.ShowToolTips")));
			this.toolBar1.Size = ((System.Drawing.Size)(resources.GetObject("toolBar1.Size")));
			this.toolBar1.TabIndex = ((int)(resources.GetObject("toolBar1.TabIndex")));
			this.toolBar1.TextAlign = ((System.Windows.Forms.ToolBarTextAlign)(resources.GetObject("toolBar1.TextAlign")));
			this.toolBar1.Visible = ((bool)(resources.GetObject("toolBar1.Visible")));
			this.toolBar1.Wrappable = ((bool)(resources.GetObject("toolBar1.Wrappable")));
			this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
			// 
			// toolBarButtonNew
			// 
			this.toolBarButtonNew.Enabled = ((bool)(resources.GetObject("toolBarButtonNew.Enabled")));
			this.toolBarButtonNew.ImageIndex = ((int)(resources.GetObject("toolBarButtonNew.ImageIndex")));
			this.toolBarButtonNew.Text = resources.GetString("toolBarButtonNew.Text");
			this.toolBarButtonNew.ToolTipText = resources.GetString("toolBarButtonNew.ToolTipText");
			this.toolBarButtonNew.Visible = ((bool)(resources.GetObject("toolBarButtonNew.Visible")));
			// 
			// toolBarButtonOpen
			// 
			this.toolBarButtonOpen.Enabled = ((bool)(resources.GetObject("toolBarButtonOpen.Enabled")));
			this.toolBarButtonOpen.ImageIndex = ((int)(resources.GetObject("toolBarButtonOpen.ImageIndex")));
			this.toolBarButtonOpen.Text = resources.GetString("toolBarButtonOpen.Text");
			this.toolBarButtonOpen.ToolTipText = resources.GetString("toolBarButtonOpen.ToolTipText");
			this.toolBarButtonOpen.Visible = ((bool)(resources.GetObject("toolBarButtonOpen.Visible")));
			// 
			// toolBarButtonSave
			// 
			this.toolBarButtonSave.Enabled = ((bool)(resources.GetObject("toolBarButtonSave.Enabled")));
			this.toolBarButtonSave.ImageIndex = ((int)(resources.GetObject("toolBarButtonSave.ImageIndex")));
			this.toolBarButtonSave.Text = resources.GetString("toolBarButtonSave.Text");
			this.toolBarButtonSave.ToolTipText = resources.GetString("toolBarButtonSave.ToolTipText");
			this.toolBarButtonSave.Visible = ((bool)(resources.GetObject("toolBarButtonSave.Visible")));
			// 
			// toolBarButtonSaveAll
			// 
			this.toolBarButtonSaveAll.Enabled = ((bool)(resources.GetObject("toolBarButtonSaveAll.Enabled")));
			this.toolBarButtonSaveAll.ImageIndex = ((int)(resources.GetObject("toolBarButtonSaveAll.ImageIndex")));
			this.toolBarButtonSaveAll.Text = resources.GetString("toolBarButtonSaveAll.Text");
			this.toolBarButtonSaveAll.ToolTipText = resources.GetString("toolBarButtonSaveAll.ToolTipText");
			this.toolBarButtonSaveAll.Visible = ((bool)(resources.GetObject("toolBarButtonSaveAll.Visible")));
			// 
			// imageList1
			// 
			this.imageList1.ImageSize = ((System.Drawing.Size)(resources.GetObject("imageList1.ImageSize")));
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// Studio
			// 
			this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
			this.AccessibleName = resources.GetString("$this.AccessibleName");
			this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
			this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
			this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
			this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
			this.Controls.Add(this.toolBar1);
			this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
			this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
			this.IsMdiContainer = true;
			this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
			this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
			this.Menu = this.mainMenu;
			this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
			this.Name = "Studio";
			this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
			this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
			this.Text = resources.GetString("$this.Text");
			this.Closed += new System.EventHandler(this.Studio_Closed);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Studio());
		}

		/// <summary>
		/// File -> Exit Button
		/// </summary>
		private void menuItemFileExit_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		private void Studio_Closed(object sender, System.EventArgs e)
		{
			//
			// We want to force the application to exit if the MDI parent is for
			// some reason closed and the application is still running.
			//
			Application.Exit();
		}

		private void OpenFile(string filename)
		{
			ModEditor newEditor = new ModEditor(filename);
			newEditor.MdiParent = this;
			newEditor.Text = filename;
			newEditor.Show();
		}

		private void menuItemFileOpen_Click(object sender, System.EventArgs e)
		{
			openFileDialog1.ShowDialog(this);
		}

		private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			OpenFile(openFileDialog1.FileName);
		}

		private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			switch (toolBar1.Buttons.IndexOf(e.Button))
			{
				case 0: // new
					break;
				case 1: // open
					menuItemFileOpen_Click(null, null);
					break;
				case 2: // save
					menuItemFileSave_Click(null, null);
					break;
				case 3: // saveall
					menuItemFileSaveAll_Click(null, null);
					break;
			}
		}

		private void menuItemFileSave_Click(object sender, System.EventArgs e)
		{
			if (this.MdiChildren.Length > 0)
			{
				((ModStudio.ModEditor)(this.ActiveMdiChild)).SaveFile();
			}
		}

		private void menuItemFileSaveAs_Click(object sender, System.EventArgs e)
		{
			if (this.MdiChildren.Length > 0)
			{
				((ModStudio.ModEditor)(this.ActiveMdiChild)).SaveFileAs();
			}
		}

		private void menuItemFileSaveAll_Click(object sender, System.EventArgs e)
		{
			if (this.MdiChildren.Length > 0)
			{
				foreach (ModEditor ms in this.MdiChildren)
				{
					if (ms.Text.StartsWith("untitled"))
					{
						ms.Focus();
					}
					ms.SaveFile();
				}
			}
		}
	}
}
