/***************************************************************************
 *                              Studio.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: Studio.cs,v 1.17 2007-09-01 13:52:37 smithydll Exp $
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.Win32;
using Phpbb.ModTeam.Tools;
using Phpbb.ModTeam.Tools.DataStructures;
using Phpbb.ModTeam.Tools.Validation;
using System.IO;
using System.Net;
using ModFormControls;
using ModProjects;

using System.Xml;
using System.Xml.Serialization;

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

		private Point lastCoOrdinates;
		private System.Windows.Forms.OpenFileDialog openFileDialog2;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.MenuItem menuItem12;
		private System.Windows.Forms.MenuItem menuItem13;
		private System.Windows.Forms.ImageList imageList2;
		private ModFormControls.TabBar tabBar1;
		private ModFormControls.ProjectPanel projectPanel1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.MenuItem menuItem14;
		private System.Windows.Forms.MenuItem menuItem15;
		private System.Windows.Forms.MenuItem menuItem16;
		private System.Windows.Forms.MenuItem menuItem17;
		private System.Windows.Forms.MenuItem menuItem18;
		private System.Windows.Forms.MenuItem menuItem19;
		private System.Windows.Forms.MenuItem menuItem20;
		private System.Windows.Forms.MenuItem menuItem21;
		private System.Windows.Forms.MenuItem menuItem22;
		private System.Windows.Forms.MenuItem menuItem23;
		private System.Windows.Forms.ToolBarButton toolBarButtonSeparator1;
		private System.Windows.Forms.ToolBarButton toolBarButtonCut;
		private System.Windows.Forms.ToolBarButton toolBarButtonCopy;
		private System.Windows.Forms.ToolBarButton toolBarButtonPaste;
		private System.Windows.Forms.MenuItem menuItem24;
		private System.Windows.Forms.MenuItem menuItem25;
		private System.Windows.Forms.MenuItem menuItem26;
		private Size lastSize;
        private NewProjectDialog newProjectDialog1;

        private ModProject openProject;
        private string openProjectPath;
        private string openProjectFile;

        private Dictionary<IEditor, string> openDocumentList = new Dictionary<IEditor, string>();
        private OpenFileDialog openFileDialog3;
        private MenuItem menuItem27;
        private NewFileDialog newFileDialog1;

        private static AppDomain domain = AppDomain.CreateDomain("MODStudioDomain");

		/// <summary>
		/// 
		/// </summary>
		public Studio(string[] args)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			// Make sure the keys are created
			string[] subKeys = Registry.CurrentUser.OpenSubKey("Software").GetSubKeyNames();
			bool keyFound = false;
			foreach (string key in subKeys)	if (key == "VB and VBA Program Settings") keyFound = true;
			if (!keyFound) Registry.CurrentUser.OpenSubKey("Software", true).CreateSubKey("VB and VBA Program Settings");

			subKeys = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("VB and VBA Program Settings").GetSubKeyNames();
			keyFound = false;
			foreach (string key in subKeys)	if (key == "MODStudio") keyFound = true;
			if (!keyFound) Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("VB and VBA Program Settings", true).CreateSubKey("MODStudio");

			subKeys = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("VB and VBA Program Settings").OpenSubKey("MODStudio").GetSubKeyNames();
			keyFound = false;
			foreach (string key in subKeys)	if (key == "mod-settings") keyFound = true;
			if (!keyFound) Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("VB and VBA Program Settings").OpenSubKey("MODStudio", true).CreateSubKey("mod-settings");

			RegistryKey reg = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("VB and VBA Program Settings").OpenSubKey("MODStudio").OpenSubKey("mod-settings");
			string defaultLanguage = reg.GetValue("language", "en").ToString();

			/*ModEditors = new ModEditor[1];
			ModEditors[0] = new ModEditor();
			ModEditors[0].ThisMod.Header.ModTitle.AddLanguage("Untitled Mod", "en");
			ModEditors[0].MdiParent = this;
			ModEditors[0].Show();*/

			if (args.Length > 0)
			{
                if (args[0].ToLower().EndsWith(".mod") || args[0].ToLower().EndsWith(".txt") || args[0].ToLower().EndsWith(".xml"))
                {
                    OpenModFileWindow(args[0]);
                }
			}
			else
			{
                NewModxFileWindow();
			}
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Studio));
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuItemFile = new System.Windows.Forms.MenuItem();
            this.menuItemFileNew = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.menuItem13 = new System.Windows.Forms.MenuItem();
            this.menuItem17 = new System.Windows.Forms.MenuItem();
            this.menuItemFileOpen = new System.Windows.Forms.MenuItem();
            this.menuItemFileClose = new System.Windows.Forms.MenuItem();
            this.menuItem14 = new System.Windows.Forms.MenuItem();
            this.menuItem15 = new System.Windows.Forms.MenuItem();
            this.menuItem16 = new System.Windows.Forms.MenuItem();
            this.menuItemSep1 = new System.Windows.Forms.MenuItem();
            this.menuItemFileSave = new System.Windows.Forms.MenuItem();
            this.menuItemFileSaveAs = new System.Windows.Forms.MenuItem();
            this.menuItemFileSaveAll = new System.Windows.Forms.MenuItem();
            this.menuItemSep3 = new System.Windows.Forms.MenuItem();
            this.menuItemFileExport = new System.Windows.Forms.MenuItem();
            this.menuItemFileValidate = new System.Windows.Forms.MenuItem();
            this.menuItem26 = new System.Windows.Forms.MenuItem();
            this.menuItem25 = new System.Windows.Forms.MenuItem();
            this.menuItemSep2 = new System.Windows.Forms.MenuItem();
            this.menuItemFileExit = new System.Windows.Forms.MenuItem();
            this.menuItemEdit = new System.Windows.Forms.MenuItem();
            this.menuItem18 = new System.Windows.Forms.MenuItem();
            this.menuItem19 = new System.Windows.Forms.MenuItem();
            this.menuItem20 = new System.Windows.Forms.MenuItem();
            this.menuItem21 = new System.Windows.Forms.MenuItem();
            this.menuItem22 = new System.Windows.Forms.MenuItem();
            this.menuItem23 = new System.Windows.Forms.MenuItem();
            this.menuItemView = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem27 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuItemTools = new System.Windows.Forms.MenuItem();
            this.menuItemToolsOptions = new System.Windows.Forms.MenuItem();
            this.menuItemWindow = new System.Windows.Forms.MenuItem();
            this.menuItem24 = new System.Windows.Forms.MenuItem();
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
            this.toolBarButtonSeparator1 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonCut = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonCopy = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonPaste = new System.Windows.Forms.ToolBarButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.projectPanel1 = new ModFormControls.ProjectPanel();
            this.tabBar1 = new ModFormControls.TabBar();
            this.newProjectDialog1 = new ModStudio.NewProjectDialog();
            this.openFileDialog3 = new System.Windows.Forms.OpenFileDialog();
            this.newFileDialog1 = new ModStudio.NewFileDialog();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFile,
            this.menuItemEdit,
            this.menuItemView,
            this.menuItem1,
            this.menuItemTools,
            this.menuItemWindow,
            this.menuItemHelp});
            // 
            // menuItemFile
            // 
            this.menuItemFile.Index = 0;
            this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFileNew,
            this.menuItemFileOpen,
            this.menuItemFileClose,
            this.menuItem14,
            this.menuItem15,
            this.menuItem16,
            this.menuItemSep1,
            this.menuItemFileSave,
            this.menuItemFileSaveAs,
            this.menuItemFileSaveAll,
            this.menuItemSep3,
            this.menuItemFileExport,
            this.menuItemFileValidate,
            this.menuItem26,
            this.menuItem25,
            this.menuItemSep2,
            this.menuItemFileExit});
            resources.ApplyResources(this.menuItemFile, "menuItemFile");
            // 
            // menuItemFileNew
            // 
            this.menuItemFileNew.Index = 0;
            this.menuItemFileNew.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem10,
            this.menuItem11,
            this.menuItem12,
            this.menuItem13,
            this.menuItem17});
            resources.ApplyResources(this.menuItemFileNew, "menuItemFileNew");
            this.menuItemFileNew.Click += new System.EventHandler(this.menuItemFileNew_Click);
            this.menuItemFileNew.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.menuItemFileNew_DrawItem);
            this.menuItemFileNew.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.menuItemFileNew_MeasureItem);
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 0;
            resources.ApplyResources(this.menuItem10, "menuItem10");
            this.menuItem10.Click += new System.EventHandler(this.menuItem10_Click);
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 1;
            resources.ApplyResources(this.menuItem11, "menuItem11");
            // 
            // menuItem12
            // 
            this.menuItem12.Index = 2;
            resources.ApplyResources(this.menuItem12, "menuItem12");
            this.menuItem12.Click += new System.EventHandler(this.menuItem12_Click);
            // 
            // menuItem13
            // 
            this.menuItem13.Index = 3;
            resources.ApplyResources(this.menuItem13, "menuItem13");
            this.menuItem13.Click += new System.EventHandler(this.menuItem13_Click);
            // 
            // menuItem17
            // 
            resources.ApplyResources(this.menuItem17, "menuItem17");
            this.menuItem17.Index = 4;
            // 
            // menuItemFileOpen
            // 
            this.menuItemFileOpen.Index = 1;
            resources.ApplyResources(this.menuItemFileOpen, "menuItemFileOpen");
            this.menuItemFileOpen.Click += new System.EventHandler(this.menuItemFileOpen_Click);
            // 
            // menuItemFileClose
            // 
            this.menuItemFileClose.Index = 2;
            resources.ApplyResources(this.menuItemFileClose, "menuItemFileClose");
            this.menuItemFileClose.Click += new System.EventHandler(this.menuItemFileClose_Click);
            // 
            // menuItem14
            // 
            this.menuItem14.Index = 3;
            resources.ApplyResources(this.menuItem14, "menuItem14");
            // 
            // menuItem15
            // 
            this.menuItem15.Index = 4;
            resources.ApplyResources(this.menuItem15, "menuItem15");
            // 
            // menuItem16
            // 
            resources.ApplyResources(this.menuItem16, "menuItem16");
            this.menuItem16.Index = 5;
            // 
            // menuItemSep1
            // 
            this.menuItemSep1.Index = 6;
            resources.ApplyResources(this.menuItemSep1, "menuItemSep1");
            // 
            // menuItemFileSave
            // 
            this.menuItemFileSave.Index = 7;
            resources.ApplyResources(this.menuItemFileSave, "menuItemFileSave");
            this.menuItemFileSave.Click += new System.EventHandler(this.menuItemFileSave_Click);
            // 
            // menuItemFileSaveAs
            // 
            this.menuItemFileSaveAs.Index = 8;
            resources.ApplyResources(this.menuItemFileSaveAs, "menuItemFileSaveAs");
            this.menuItemFileSaveAs.Click += new System.EventHandler(this.menuItemFileSaveAs_Click);
            // 
            // menuItemFileSaveAll
            // 
            this.menuItemFileSaveAll.Index = 9;
            resources.ApplyResources(this.menuItemFileSaveAll, "menuItemFileSaveAll");
            this.menuItemFileSaveAll.Click += new System.EventHandler(this.menuItemFileSaveAll_Click);
            // 
            // menuItemSep3
            // 
            this.menuItemSep3.Index = 10;
            resources.ApplyResources(this.menuItemSep3, "menuItemSep3");
            // 
            // menuItemFileExport
            // 
            resources.ApplyResources(this.menuItemFileExport, "menuItemFileExport");
            this.menuItemFileExport.Index = 11;
            // 
            // menuItemFileValidate
            // 
            this.menuItemFileValidate.Index = 12;
            resources.ApplyResources(this.menuItemFileValidate, "menuItemFileValidate");
            this.menuItemFileValidate.Click += new System.EventHandler(this.menuItemFileValidate_Click);
            // 
            // menuItem26
            // 
            this.menuItem26.Index = 13;
            resources.ApplyResources(this.menuItem26, "menuItem26");
            // 
            // menuItem25
            // 
            this.menuItem25.Index = 14;
            resources.ApplyResources(this.menuItem25, "menuItem25");
            // 
            // menuItemSep2
            // 
            this.menuItemSep2.Index = 15;
            resources.ApplyResources(this.menuItemSep2, "menuItemSep2");
            // 
            // menuItemFileExit
            // 
            this.menuItemFileExit.Index = 16;
            resources.ApplyResources(this.menuItemFileExit, "menuItemFileExit");
            this.menuItemFileExit.Click += new System.EventHandler(this.menuItemFileExit_Click);
            // 
            // menuItemEdit
            // 
            this.menuItemEdit.Index = 1;
            this.menuItemEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem18,
            this.menuItem19,
            this.menuItem20,
            this.menuItem21,
            this.menuItem22,
            this.menuItem23});
            resources.ApplyResources(this.menuItemEdit, "menuItemEdit");
            // 
            // menuItem18
            // 
            resources.ApplyResources(this.menuItem18, "menuItem18");
            this.menuItem18.Index = 0;
            // 
            // menuItem19
            // 
            resources.ApplyResources(this.menuItem19, "menuItem19");
            this.menuItem19.Index = 1;
            // 
            // menuItem20
            // 
            this.menuItem20.Index = 2;
            resources.ApplyResources(this.menuItem20, "menuItem20");
            // 
            // menuItem21
            // 
            resources.ApplyResources(this.menuItem21, "menuItem21");
            this.menuItem21.Index = 3;
            this.menuItem21.Click += new System.EventHandler(this.menuItem21_Click);
            // 
            // menuItem22
            // 
            resources.ApplyResources(this.menuItem22, "menuItem22");
            this.menuItem22.Index = 4;
            // 
            // menuItem23
            // 
            resources.ApplyResources(this.menuItem23, "menuItem23");
            this.menuItem23.Index = 5;
            this.menuItem23.Click += new System.EventHandler(this.menuItem23_Click);
            // 
            // menuItemView
            // 
            resources.ApplyResources(this.menuItemView, "menuItemView");
            this.menuItemView.Index = 2;
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 3;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2,
            this.menuItem3,
            this.menuItem4,
            this.menuItem27,
            this.menuItem5,
            this.menuItem6,
            this.menuItem7,
            this.menuItem8,
            this.menuItem9});
            resources.ApplyResources(this.menuItem1, "menuItem1");
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            resources.ApplyResources(this.menuItem2, "menuItem2");
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            resources.ApplyResources(this.menuItem3, "menuItem3");
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            resources.ApplyResources(this.menuItem4, "menuItem4");
            // 
            // menuItem27
            // 
            this.menuItem27.Index = 3;
            resources.ApplyResources(this.menuItem27, "menuItem27");
            this.menuItem27.Click += new System.EventHandler(this.menuItem27_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 4;
            resources.ApplyResources(this.menuItem5, "menuItem5");
            this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 5;
            resources.ApplyResources(this.menuItem6, "menuItem6");
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 6;
            resources.ApplyResources(this.menuItem7, "menuItem7");
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 7;
            resources.ApplyResources(this.menuItem8, "menuItem8");
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 8;
            resources.ApplyResources(this.menuItem9, "menuItem9");
            // 
            // menuItemTools
            // 
            this.menuItemTools.Index = 4;
            this.menuItemTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemToolsOptions});
            resources.ApplyResources(this.menuItemTools, "menuItemTools");
            // 
            // menuItemToolsOptions
            // 
            this.menuItemToolsOptions.Index = 0;
            resources.ApplyResources(this.menuItemToolsOptions, "menuItemToolsOptions");
            this.menuItemToolsOptions.Click += new System.EventHandler(this.menuItemToolsOptions_Click);
            // 
            // menuItemWindow
            // 
            this.menuItemWindow.Index = 5;
            this.menuItemWindow.MdiList = true;
            this.menuItemWindow.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem24});
            resources.ApplyResources(this.menuItemWindow, "menuItemWindow");
            // 
            // menuItem24
            // 
            this.menuItem24.Index = 0;
            resources.ApplyResources(this.menuItem24, "menuItem24");
            this.menuItem24.Click += new System.EventHandler(this.menuItem24_Click);
            // 
            // menuItemHelp
            // 
            this.menuItemHelp.Index = 6;
            this.menuItemHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemHelpHelp,
            this.menuItemSep4,
            this.menuItemHelpAbout});
            resources.ApplyResources(this.menuItemHelp, "menuItemHelp");
            // 
            // menuItemHelpHelp
            // 
            resources.ApplyResources(this.menuItemHelpHelp, "menuItemHelpHelp");
            this.menuItemHelpHelp.Index = 0;
            // 
            // menuItemSep4
            // 
            this.menuItemSep4.Index = 1;
            resources.ApplyResources(this.menuItemSep4, "menuItemSep4");
            // 
            // menuItemHelpAbout
            // 
            this.menuItemHelpAbout.Index = 2;
            resources.ApplyResources(this.menuItemHelpAbout, "menuItemHelpAbout");
            this.menuItemHelpAbout.Click += new System.EventHandler(this.menuItemHelpAbout_Click);
            // 
            // openFileDialog1
            // 
            resources.ApplyResources(this.openFileDialog1, "openFileDialog1");
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // toolBar1
            // 
            resources.ApplyResources(this.toolBar1, "toolBar1");
            this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButtonNew,
            this.toolBarButtonOpen,
            this.toolBarButtonSave,
            this.toolBarButtonSaveAll,
            this.toolBarButtonSeparator1,
            this.toolBarButtonCut,
            this.toolBarButtonCopy,
            this.toolBarButtonPaste});
            this.toolBar1.ImageList = this.imageList1;
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
            // 
            // toolBarButtonNew
            // 
            resources.ApplyResources(this.toolBarButtonNew, "toolBarButtonNew");
            this.toolBarButtonNew.Name = "toolBarButtonNew";
            // 
            // toolBarButtonOpen
            // 
            resources.ApplyResources(this.toolBarButtonOpen, "toolBarButtonOpen");
            this.toolBarButtonOpen.Name = "toolBarButtonOpen";
            // 
            // toolBarButtonSave
            // 
            resources.ApplyResources(this.toolBarButtonSave, "toolBarButtonSave");
            this.toolBarButtonSave.Name = "toolBarButtonSave";
            // 
            // toolBarButtonSaveAll
            // 
            resources.ApplyResources(this.toolBarButtonSaveAll, "toolBarButtonSaveAll");
            this.toolBarButtonSaveAll.Name = "toolBarButtonSaveAll";
            // 
            // toolBarButtonSeparator1
            // 
            this.toolBarButtonSeparator1.Name = "toolBarButtonSeparator1";
            this.toolBarButtonSeparator1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButtonCut
            // 
            resources.ApplyResources(this.toolBarButtonCut, "toolBarButtonCut");
            this.toolBarButtonCut.Name = "toolBarButtonCut";
            // 
            // toolBarButtonCopy
            // 
            resources.ApplyResources(this.toolBarButtonCopy, "toolBarButtonCopy");
            this.toolBarButtonCopy.Name = "toolBarButtonCopy";
            // 
            // toolBarButtonPaste
            // 
            resources.ApplyResources(this.toolBarButtonPaste, "toolBarButtonPaste");
            this.toolBarButtonPaste.Name = "toolBarButtonPaste";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            this.imageList1.Images.SetKeyName(3, "");
            this.imageList1.Images.SetKeyName(4, "");
            this.imageList1.Images.SetKeyName(5, "");
            this.imageList1.Images.SetKeyName(6, "");
            // 
            // openFileDialog2
            // 
            resources.ApplyResources(this.openFileDialog2, "openFileDialog2");
            this.openFileDialog2.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog2_FileOk);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "");
            this.imageList2.Images.SetKeyName(1, "");
            // 
            // splitter1
            // 
            resources.ApplyResources(this.splitter1, "splitter1");
            this.splitter1.Name = "splitter1";
            this.splitter1.TabStop = false;
            // 
            // projectPanel1
            // 
            this.projectPanel1.Actions = null;
            resources.ApplyResources(this.projectPanel1, "projectPanel1");
            this.projectPanel1.Name = "projectPanel1";
            // 
            // tabBar1
            // 
            this.tabBar1.BackColor = System.Drawing.SystemColors.ButtonFace;
            resources.ApplyResources(this.tabBar1, "tabBar1");
            this.tabBar1.Name = "tabBar1";
            this.tabBar1.SelectedIndex = -1;
            this.tabBar1.SelectedTab = null;
            this.tabBar1.TabSelectedIndexChanged += new ModFormControls.TabBar.TabSelectedIndexChangedHandler(this.tabBar1_TabSelectedIndexChanged);
            this.tabBar1.TabClose += new ModFormControls.TabBar.TabCloseHandler(this.tabBar1_TabClose);
            // 
            // newProjectDialog1
            // 
            this.newProjectDialog1.NewProject += new ModStudio.NewProjectDialogBox.NewProjectDialogBoxSaveNewHandler(this.newProjectDialog1_NewProject);
            // 
            // openFileDialog3
            // 
            this.openFileDialog3.FileName = "openFileDialog3";
            this.openFileDialog3.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog3_FileOk);
            // 
            // newFileDialog1
            // 
            this.newFileDialog1.NewFile += new ModStudio.NewFileDialogBox.NewFileDialogBoxSaveNewHandler(this.newFileDialog1_NewFile);
            // 
            // Studio
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.projectPanel1);
            this.Controls.Add(this.tabBar1);
            this.Controls.Add(this.toolBar1);
            this.IsMdiContainer = true;
            this.Menu = this.mainMenu;
            this.Name = "Studio";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Studio_Paint);
            this.Closed += new System.EventHandler(this.Studio_Closed);
            this.Resize += new System.EventHandler(this.Studio_Resize);
            this.MdiChildActivate += new System.EventHandler(this.Studio_MdiChildActivate);
            this.ContextMenuChanged += new System.EventHandler(this.Studio_ContextMenuChanged);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Studio_Closing);
            this.LocationChanged += new System.EventHandler(this.Studio_LocationChanged);
            this.Load += new System.EventHandler(this.Studio_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) 
		{
			Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
			Application.DoEvents();
			Application.Run(new Studio(args));
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

        private void OpenProject(string filename)
        {
            if (!File.Exists(filename))
            {
                MessageBox.Show("Invalid project file");
                return;
            }
            FileInfo fi = new FileInfo(filename);

            // TODO: close all files opened under an already opened project

            XmlSerializer xs = new XmlSerializer(typeof(ModProject), "mod-proj");
            FileStream fs = new FileStream(filename, FileMode.Open);
            openProject = (ModProject)xs.Deserialize(fs);
            fs.Close();
            openProject.ReSyncAllParents();

            openProjectFile = filename;
            openProjectPath = fi.Directory.FullName;

            projectPanel1.ProjectPath = openProjectPath;
            projectPanel1.Project = openProject;
            projectPanel1.Visible = true;

            // open all files that were last time open
            foreach (ProjectFolder pf in openProject.Folders)
            {
                OpenOpenProjectFiles(pf);
            }

        }

        private void OpenOpenProjectFiles(ProjectFolder projectFolder)
        {
            foreach (ProjectFolder pf in projectFolder.Folders)
            {
                OpenOpenProjectFiles(pf);
            }

            foreach (ProjectFile pf in projectFolder.Files)
            {
                if (pf.IsOpen)
                {
                    string fileFullPath = "";
                    if (projectFolder.FolderName == "")
                    {
                        fileFullPath = Path.Combine(openProjectPath, pf.FileName);
                    }
                    else
                    {
                        fileFullPath = Path.Combine(Path.Combine(openProjectPath, projectFolder.GetFolderPath()), pf.FileName);
                    }
                    OpenFile(fileFullPath);
                }
            }
        }

		private void OpenFile(string filename)
		{
			if (this.MdiChildren.Length > 0)
			{
				if (this.ActiveMdiChild.GetType() == typeof(ModEditor))
				{
					if (((ModEditor)this.ActiveMdiChild).Text == "untitled")
					{
						this.ActiveMdiChild.Close();
					}
				}
			}

            ProjectFileType pft = ProjectPanel.ReadFirstBytesInFile(filename);
            if (filename.ToLower().EndsWith(".modproj"))
            {
                // open a project
                OpenProject(filename);
            }
			else if (filename.ToLower().EndsWith(".mod") ||
				filename.ToLower().EndsWith(".txt") ||
				filename.ToLower().EndsWith(".xml"))
			{
                switch (pft)
                {
                    case ProjectFileType.TextMod:
                    case ProjectFileType.ModxMox:
                        OpenModxFileWindow(filename);
                        break;
                    case ProjectFileType.TextFile:
                    case ProjectFileType.Gpl:
                    case ProjectFileType.Lgpl:
                        TextEditor newTextEditor = new TextEditor(filename);
                        newTextEditor.MdiParent = this;
                        newTextEditor.Text = filename;
                        newTextEditor.Show();
                        break;
                }

                //openDocumentList.Add(newEditor);
			}
			else if (filename.ToLower().EndsWith(".php"))
			{
				// assume language file for the moment, later add SMART filtering
                switch (pft)
                {
                    case ProjectFileType.LanguageFile:
                        LanguageEditor newEditor = new LanguageEditor();
                        newEditor.MdiParent = this;
                        newEditor.Text = filename;
                        newEditor.Open(filename);
                        newEditor.Show();
                        break;
                    case ProjectFileType.PhpFile:
                        TextEditor newTextEditor = new TextEditor(filename);
                        newTextEditor.MdiParent = this;
                        newTextEditor.Text = filename;
                        newTextEditor.Show();
                        break;
                }

                //openDocumentList.Add(newEditor);
			}
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
					menuItem13_Click(null, null);
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
				case 4: // separator
					break;
				case 5: // cut
					menuItem21_Click(null, null);
					break;
				case 6: // copy
					break;
				case 7: // paste
                    menuItem23_Click(null, null);
					break;
			}
		}

		private void menuItemFileSave_Click(object sender, System.EventArgs e)
		{
			if (this.MdiChildren.Length > 0)
			{
				if (this.ActiveMdiChild is IEditor)
				{
                    ((IEditor)(this.ActiveMdiChild)).SaveFile();
				}
				else if (this.ActiveMdiChild.GetType() == typeof(LanguageEditor))
				{
					((LanguageEditor)(this.ActiveMdiChild)).SaveFile();
				}
			}
		}

		private void menuItemFileSaveAs_Click(object sender, System.EventArgs e)
		{
			if (this.MdiChildren.Length > 0)
			{
				if (this.ActiveMdiChild.GetType() == typeof(ModEditor))
				{
					((ModEditor)(this.ActiveMdiChild)).SaveFileAs();
				}
				else if (this.ActiveMdiChild.GetType() == typeof(LanguageEditor))
				{
					((LanguageEditor)(this.ActiveMdiChild)).SaveFileAs();
				}
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

		private void menuItemFileClose_Click(object sender, System.EventArgs e)
		{
			if (this.MdiChildren.Length > 0)
			{
				this.ActiveMdiChild.Close();
			}
		}

		private void menuItemFileNew_Click(object sender, System.EventArgs e)
		{
		}

		private void menuItemHelpAbout_Click(object sender, System.EventArgs e)
		{
			//MessageBox.Show(this, "MOD Studio v4 (0.1) Copyright 2005 David Smith (smithy_dll).\nTextbox component from http://icsharpcode.net/");
			AboutBox aboutBox1 = new AboutBox();
			aboutBox1.ShowDialog(this);
		}

		private void Studio_Load(object sender, System.EventArgs e)
		{
			RegistryKey reg = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("VB and VBA Program Settings").OpenSubKey("MODStudio", true);
			this.lastSize = new Size(int.Parse(reg.GetValue("window-width",this.Width).ToString()), int.Parse(reg.GetValue("window-height",this.Height).ToString()));
			this.lastCoOrdinates = new Point(int.Parse(reg.GetValue("window-left",this.Left).ToString()), int.Parse(reg.GetValue("window-top",this.Top).ToString()));
			if (reg.GetValue("window-state", this.WindowState).ToString() == "Maximized")
			{
				this.WindowState = FormWindowState.Maximized;
			}
			else if (reg.GetValue("window-state", this.WindowState).ToString() == "Normal")
			{
				this.WindowState = FormWindowState.Normal;
				this.Location = this.lastCoOrdinates;
				this.Size = this.lastSize;
			}
			

			if (bool.Parse(reg.GetValue("first-run", true).ToString()))
			{
				DialogResult result = MessageBox.Show(this, "Do you want to automatically check for new versions of MOD Studio everytime it loads?", "Automatically check for updates?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (result == DialogResult.Yes)
				{
					reg.SetValue("automatic-check", true);
				}
				else if (result == DialogResult.No)
				{
					reg.SetValue("automatic-check", false);
				}
				reg.SetValue("first-run", false);
			}
			else
			{
				if (bool.Parse(reg.GetValue("automatic-check", false).ToString()))
				{
					if (checkNewVersion())
					{
						MessageBox.Show(this, "There are updates currently avaliable, a browser window will now open at the download page");
						System.Diagnostics.Process.Start(@"http://www.phpbb.com/phpBB/viewtopic.php?t=320507");
					}
					reg.SetValue("last-check", DateTime.Now);
				}
				else
				{
					if (reg.GetValue("last-check", null) != null)
					{
						// if been longer than 3 months since last check for update, ask if we want to check
						if (((TimeSpan)DateTime.Now.Subtract((DateTime.Parse(reg.GetValue("last-check").ToString())))).Days >= 90)
						{
							DialogResult result = MessageBox.Show(this, "It has been a while since you last checked for updates to MOD Studio.\nDo you want to check for updates to MOD Studio now?", "Check for updates?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
							if (result == DialogResult.Yes)
							{
								if (checkNewVersion())
								{
									MessageBox.Show(this, "There are updates currently avaliable, a browser window will now open at the download page");
									System.Diagnostics.Process.Start(@"http://www.phpbb.com/phpBB/viewtopic.php?t=320507");
								}
								else
								{
									MessageBox.Show(this, "There are no updates avaliable", "No updates");
								}
								reg.SetValue("last-check", DateTime.Now);
							}
						}
					}
					else
					{
						// lets set last-check if it's null
						reg.SetValue("last-check", DateTime.Now);
					}
				}
			}
			reg.SetValue("last-run", DateTime.Now);
			reg.Close();
		}

		private void Studio_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			RegistryKey reg = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("VB and VBA Program Settings").OpenSubKey("MODStudio", true);
			reg.SetValue("window-state", this.WindowState);
			reg.SetValue("window-width", this.Width);
			reg.SetValue("window-height", this.Height);
			reg.SetValue("window-left", this.Left);
			reg.SetValue("window-top", this.Top);
			reg.Close();

            /* save open project */
            if (openProject != null)
            {
                if (!string.IsNullOrEmpty(openProjectFile))
                {
                    SaveOpenProject();
                }
            }
		}

        private void SaveOpenProject()
        {
            if (openProject != null)
            {
                foreach (ProjectFolder pf in openProject.Folders)
                {
                    UpdateOpenProjectFolder(pf);
                }

                XmlSerializer xs = new XmlSerializer(typeof(ModProject), "mod-proj");
                FileStream projectOut = new FileStream(openProjectFile, FileMode.Create);
                xs.Serialize(projectOut, openProject);
                projectOut.Close();
            }
        }

        private void UpdateOpenProjectFolder(ProjectFolder projectFolder)
        {
            foreach (ProjectFolder pf in projectFolder.Folders)
            {
                UpdateOpenProjectFolder(pf);
            }

            foreach (ProjectFile pf in projectFolder.Files)
            {
                // generate the full file path
                string fileFullPath = "";
                if (projectFolder.FolderName == "")
                {
                    fileFullPath = Path.Combine(openProjectPath, pf.FileName);
                }
                else
                {
                    fileFullPath = Path.Combine(Path.Combine(openProjectPath, projectFolder.GetFolderPath()), pf.FileName);
                }

                // update file open status
                if (openDocumentList.ContainsValue(fileFullPath))
                {
                    pf.IsOpen = true;
                }
                else
                {
                    pf.IsOpen = false;
                }
            }
        }

		private void menuItemToolsOptions_Click(object sender, System.EventArgs e)
		{
			OptionsDialog options = new OptionsDialog();
			options.ShowDialog(this);
		}

		private void Studio_LocationChanged(object sender, System.EventArgs e)
		{
			if (this.WindowState == FormWindowState.Normal)
			{
				this.lastCoOrdinates = this.Location;
			}
		}

		private void Studio_Resize(object sender, System.EventArgs e)
		{
			if (this.WindowState == FormWindowState.Normal)
			{
				this.lastSize = this.Size;
			}
		}

		private void menuItemFileValidate_Click(object sender, System.EventArgs e)
		{
			if (openFileDialog2.ShowDialog(this) == DialogResult.OK)
			{
				if (openFileDialog2.FileName.ToLower().EndsWith(".txt") || openFileDialog2.FileName.ToLower().EndsWith(".mod"))
				{
					TextMod aMod = new TextMod(Application.StartupPath);
					Report report = aMod.Validate(openFileDialog2.FileName);
					SaveTextFile(report.ToString(Phpbb.ModTeam.Tools.Validation.Report.ReportFormat.Html), Application.UserAppDataPath + @"\validationReport.html");
					ValidationResult validationResultWindow = new ValidationResult();
					validationResultWindow.ShowDialog(this);
				}
				else if (openFileDialog2.FileName.ToLower().EndsWith(".xml") || openFileDialog2.FileName.ToLower().EndsWith(".modx"))
				{
					ModxMod aMod = new ModxMod();
					Report report = aMod.Validate(openFileDialog2.FileName);
					SaveTextFile(report.ToString(Phpbb.ModTeam.Tools.Validation.Report.ReportFormat.Html), Application.UserAppDataPath + @"\validationReport.html");
					ValidationResult validationResultWindow = new ValidationResult();
					validationResultWindow.ShowDialog(this);
				}
			}
		}

		private void openFileDialog2_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			
		}

		/// <summary>
		/// requires access to the internet
		/// </summary>
		/// <returns></returns>
		public bool checkNewVersion()
		{
			WebClient webClient = new WebClient();
			byte[] bytes;
            try
            {
                bytes = webClient.DownloadData("http://modstudio.sf.net/updatecheck/4-0-x.txt");
            }
            catch (System.Net.WebException)
            {
                // catch the exception of not having an internet connection to connect through
                return false;
            }

			string version = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
            if (version == "") return false;
            ModVersion newVersion = new ModVersion() ;
            try
            {
                newVersion = ModVersion.Parse(version);
            }
            catch
            {
                return false;
            }
			ModVersion thisVersion = ModVersion.Parse(Application.ProductVersion);
			Console.WriteLine(newVersion.ToString() + " [...] " + thisVersion.ToString());
			if (newVersion.Major > thisVersion.Major)
			{
				return true;
			}
			else if (newVersion.Major == thisVersion.Major
                && newVersion.Minor > thisVersion.Minor)
			{
				return true;
			}
			else if (newVersion.Release == thisVersion.Release
                && newVersion.Release > thisVersion.Release)
			{
				return true;
			}
			return false;
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

		/// <summary>
		/// Save a text file
		/// </summary>
		/// <param name="filetosave"></param>
		/// <param name="filename"></param>
		private static void SaveTextFile(string filetosave, string filename)
		{
			StreamWriter myStreamWriter = File.CreateText(filename);
			myStreamWriter.Write(filetosave);
			myStreamWriter.Close();
		}

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
            if (openProject != null)
            {
                // TODO: set default file type
                newFileDialog1.ShowDialog();
            }
		}

		private void menuItem3_Click(object sender, System.EventArgs e)
		{
            if (openProject != null)
            {
                // TODO: set the default file type
                newFileDialog1.ShowDialog();
            }
		}

		private bool suspendSelectedIndexChanged = true;
		private void Studio_MdiChildActivate(object sender, System.EventArgs e)
		{
			/*if (!((Form)sender).IsDisposed)
			{
				((Form)sender).Disposed += new EventHandler(frm_Disposed);
			}*/
			foreach(Form frm in this.MdiChildren)
			{
				//frm.Disposed += new EventHandler(frm_Disposed);

                if (frm.GetType() == typeof(ModEditor))
                {
                    if (!openDocumentList.ContainsKey((IEditor)frm))
                    {
                        frm.Disposed += new EventHandler(frm_Disposed);
                        openDocumentList.Add((IEditor)frm, frm.Text);
                    }
                }
                else if (frm is IEditor)
                {
                    if (!openDocumentList.ContainsKey((IEditor)frm))
                    {
                        openDocumentList.Add((IEditor)frm, frm.Text);
                    }
                }
			}
			UpdateTabBar();
			updateCutCopyPaste();
			if (this.MdiChildren.Length > 0)
			{
				if (!(this.ActiveMdiChild == null))
				{
					if (this.ActiveMdiChild.GetType() == typeof(ModEditor))
					{
						ModEditor me = (ModEditor)this.ActiveMdiChild;
						this.projectPanel1.Actions = me.ThisMod.Actions;
						//this.projectPanel1.UpdateActions();
					}
				}
			}
		}

		private void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			/*if (tabControl1.SelectedIndex >= 0 && tabControl1.TabPages.Count > 0 && !suspendSelectedIndexChanged)
			/{
				this.SuspendLayout();
				this.MdiChildren[tabControl1.SelectedIndex].Select();
				this.ResumeLayout();
			}*/
		}

		private void tabControl1_Click(object sender, System.EventArgs e)
		{
		}

		private void tabControl1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Middle)
			{
				tabControl1_MouseUp(sender, new System.Windows.Forms.MouseEventArgs(MouseButtons.Left, e.Clicks, e.X, e.Y, e.Delta));
				tabControl1_Click(sender, null);
				//foreach (TabPage tp in tabControl1.TabPages)
				//{
					/*if (e.X > tp.Left && e.X < tp.Left + tp.Width)
					{
						Console.WriteLine("{0},{1},{2}", e.X, tp.Left, tp.Right);
						MessageBox.Show(this, tabControl1.TabPages.IndexOf(tp).ToString());
					}*/
				//}
			}
		}

		private void UpdateTabBar()
		{
			if (!this.Disposing)
			{
				suspendSelectedIndexChanged = true;
				//tabControl1.TabPages.Clear();
				tabBar1.Tabs.Clear();
				int i = 0;
				bool disposing = false;
				foreach(Form frm in this.MdiChildren)
				{
					if (frm.Disposing)
					{
						disposing = true;
						continue;
					}
					//TabPage tp = new TabPage(frm.Text.Split('\\')[frm.Text.Split('\\').Length - 1]);
					//tabControl1.TabPages.Add(tp);
					TabBarTab tbt = new TabBarTab(frm.Text.Split('\\')[frm.Text.Split('\\').Length - 1]);
					tabBar1.Tabs.Add(tbt);
					if (this.ActiveMdiChild != null)
					{
						if (this.ActiveMdiChild.Equals(frm))
						{
							//tabControl1.SelectedTab = tp;
							tabBar1.SelectedTab = tbt;
						}
					}
					if (frm.GetType() == typeof(ModEditor))
					{
						ModEditor frmme = (ModEditor)frm;
						if (frmme.ThisMod.GetType() == typeof(TextMod))
						{
							//tp.ImageIndex = 0;
							tbt.IconIndex = 0;
						}
						else if (frmme.ThisMod.GetType() == typeof(ModxMod))
						{
							//tp.ImageIndex = 1;
							tbt.IconIndex = 1;
						}
					}
					i++;
				}
				tabBar1.updateTabs();
				if (disposing)
				{
					tabBar1.UpdateRightMargin();
				}
				suspendSelectedIndexChanged = false;
			}
		}

		private void updateCutCopyPaste()
		{
			if (this.MdiChildren.Length == 0)
			{
				DisableCut();
				DisableCopy();
				DisablePaste();
			}
			else
			{
				// ask the active child form if cut/copy/paste is allowed for the active control
				if (this.ActiveMdiChild == null)
				{
					return;
				}
				if (this.ActiveMdiChild.GetType() == typeof(ModEditor))
				{
					if (((ModEditor)this.ActiveMdiChild).IsCutCopyPaste())
					{
						if (((ModEditor)this.ActiveMdiChild).IsCut())
						{
							EnableCut();
						}
						else
						{
							DisableCut();
						}
						if (((ModEditor)this.ActiveMdiChild).IsCopy())
						{
							EnableCopy();
						}
						else
						{
							DisableCopy();
						}
						EnablePaste();
					}
					else
					{
						DisableCut();
						DisableCopy();
						DisablePaste();
					}
				}
				else if (this.ActiveMdiChild.GetType() == typeof(LanguageEditor))
				{
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void UpdateCutCopyPaste(object sender, EventArgs e)
		{
			updateCutCopyPaste();
		}

		private void frm_Disposed(object sender, EventArgs e)
		{
			UpdateTabBar();
			tabBar1.UpdateRightMargin();
            if (sender.GetType() == typeof(ModEditor))
            {
                if (openProject != null)
                {
                    Console.WriteLine(((ModEditor)sender).Text);
                    if (openDocumentList[(ModEditor)sender].StartsWith(openProjectPath))
                    {
                        projectPanel1.CloseMod(openDocumentList[(ModEditor)sender].TrimEnd(new char[] { '*' }).Remove(0, openProjectPath.Length).TrimStart(new char[] { Path.DirectorySeparatorChar }));
                    }
                }
            }
		}

		private void Studio_ContextMenuChanged(object sender, System.EventArgs e)
		{
		
		}

		private void tabBar1_TabSelectedIndexChanged(object sender, ModFormControls.TabSelectedIndexChangedEventArgs e)
		{
			if (e.Index >= 0)
			{
				if (e.Index < this.MdiChildren.Length)
				{
					this.SuspendLayout();
					//this.MdiChildren[e.Index].Select();
					//this.MdiChildren[e.Index].Activate();
					//this.MdiChildren[e.Index].BringToFront();
					this.MdiChildren[e.Index].Focus();
					//this.ActivateMdiChild(this.MdiChildren[e.Index]);
					//this.Invalidate(true);
					//this.MdiChildren[e.Index].f
					this.ResumeLayout();
				}
			}
		}

		private void tabBar1_TabClose(object sender, ModFormControls.TabCloseEventArgs e)
		{
			if (e.Index >= 0)
			{
				if (e.Index < this.MdiChildren.Length)
				{
					this.MdiChildren[e.Index].Close();
				}
			}
		}

		private void Studio_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public void EnableCut()
		{
			menuItem21.Enabled = true;
			toolBarButtonCut.Enabled = true;
		}

		/// <summary>
		/// 
		/// </summary>
		public void DisableCut()
		{
			menuItem21.Enabled = false;
			toolBarButtonCut.Enabled = false;
		}

		/// <summary>
		/// 
		/// </summary>
		public void EnableCopy()
		{
			menuItem22.Enabled = true;
			toolBarButtonCopy.Enabled = true;
		}

		/// <summary>
		/// 
		/// </summary>
		public void DisableCopy()
		{
			menuItem22.Enabled = false;
			toolBarButtonCopy.Enabled = false;
		}

		/// <summary>
		/// 
		/// </summary>
		public void EnablePaste()
		{
			IDataObject iData = Clipboard.GetDataObject();
			if (iData.GetDataPresent(DataFormats.Text))
			{
				menuItem23.Enabled = true;
				toolBarButtonPaste.Enabled = true;
			}
			else
			{
				DisablePaste();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void DisablePaste()
		{
			menuItem23.Enabled = false;
			toolBarButtonPaste.Enabled = false;
		}

		private void menuItem13_Click(object sender, System.EventArgs e)
		{
            NewModxFileWindow();
		}

		private void menuItem12_Click(object sender, System.EventArgs e)
		{
            NewTextModFileWindow();
		}

		private void menuItem24_Click(object sender, System.EventArgs e)
		{
			foreach (Form childForm in this.MdiChildren)
			{
				childForm.Close();
			}
		}

		private void menuItem25_Click(object sender, System.EventArgs e)
		{
		
		}

		private void menuItemFileNew_MeasureItem(object sender, System.Windows.Forms.MeasureItemEventArgs e)
		{
		
		}

		private void menuItemFileNew_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
		}

		/// <summary>
		/// cut
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem21_Click(object sender, System.EventArgs e)
		{
			if (this.ActiveMdiChild is IEditor)
			{
				((IEditor)this.ActiveMdiChild).DoCut();
			}
		}

        private void menuItem10_Click(object sender, EventArgs e)
        {
            newProjectDialog1.ShowDialog();
            //openProject = new ModProject();

            if (openProject != null)
            {
                projectPanel1.Visible = true;
            }
        }

        private void newProjectDialog1_NewProject(object sender, NewProjectDialogBoxSaveNewEventArgs e)
        {
            string projectDirectory = Path.Combine(e.ProjectPath, e.ProjectName);
            if (!Directory.Exists(projectDirectory))
            {
                /* generate a new project */
                openProject = new ModProject();
                projectPanel1.Visible = true;

                openProject.BuildPath = "publish";
                openProject.PhpbbVersion = e.PhpbbVersion;
                openProject.ProjectName = e.ProjectName;
                openProject.AddFolder(new ProjectFolder(""));

                Directory.CreateDirectory(projectDirectory);
                Directory.CreateDirectory(Path.Combine(projectDirectory, openProject.BuildPath));

                openProjectPath = projectDirectory;
                openProjectFile = Path.Combine(projectDirectory, e.ProjectName + ".modproj");

                // close the untitled document created by default
                if (this.MdiChildren.Length > 0)
                {
                    if (this.ActiveMdiChild.GetType() == typeof(ModEditor))
                    {
                        if (((ModEditor)this.ActiveMdiChild).Text == "untitled")
                        {
                            this.ActiveMdiChild.Close();
                        }
                    }
                }

                NewProjectFile("license.txt", ModStudioFileTypes.Gpl2License);

                if (e.PhpbbVersion.DeterminePrimary() == "2.0")
                {
                    NewTextModFileWindow(Path.Combine(openProjectPath, "install.mod"));
                }
                else if (e.PhpbbVersion.DeterminePrimary() == "3.0")
                {
                    NewProjectFile("modx.prosilver.en.xsl", ModStudioFileTypes.ProsilverXslt);
                    NewModxFileWindow(Path.Combine(openProjectPath, "install.xml"));
                }

                SaveOpenProject();

                projectPanel1.ProjectPath = openProjectPath;
                projectPanel1.Project = openProject;
            }
            else
            {
                MessageBox.Show("A project with that name already exists at the folder given, opening existing project.");
                OpenProject(Path.Combine(e.ProjectPath, e.ProjectName + ".modproj"));
            }
        }

        private void menuItem5_Click(object sender, EventArgs e)
        {
            openFileDialog3.ShowDialog();
        }

        private void openFileDialog3_FileOk(object sender, CancelEventArgs e)
        {
            if (openProject != null)
            {
                string filename = "";
                if (openFileDialog3.FileName.StartsWith(openProjectPath))
                {
                    filename = openFileDialog3.FileName.Remove(0, openProjectPath.Length).TrimStart(new char[] { Path.DirectorySeparatorChar });
                }
                else
                {
                    FileInfo fi = new FileInfo(openFileDialog3.FileName);
                    File.Copy(openFileDialog3.FileName, Path.Combine(openProjectPath, fi.Name));
                    filename = fi.Name;
                }

                if (filename != "")
                {
                    FileInfo fi = new FileInfo(filename);
                    ProjectFile pf = new ProjectFile(fi.Name, BuildAction.Package, false);
                    AddFileToCurrentProjectFolder(pf);
                }
            }
        }

        private void AddFileToCurrentProjectFolder(ProjectFile projectFile)
        {
            projectPanel1.GetCurrentlySelectedFolder().AddFile(projectFile);
        }

        private void menuItem23_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is IEditor)
            {
                ((IEditor)this.ActiveMdiChild).DoPaste();
            }
        }

        private void menuItem27_Click(object sender, EventArgs e)
        {
            newFileDialog1.ShowDialog();
        }

        private void newFileDialog1_NewFile(object sender, NewFileDialogBoxSaveNewEventArgs e)
        {
            NewProjectFile(e.FileName, e.FileType, true);
        }

        private void NewProjectFile(string fileName, ModStudioFileTypes fileType)
        {
            NewProjectFile(fileName, fileType, false);
        }

        private void NewProjectFile(string fileName, ModStudioFileTypes fileType, bool open)
        {
            // ignore if we don't have an open project
            if (openProject == null)
            {
                return;
            }

            string templateDirectory = Path.Combine(domain.BaseDirectory, "templates");
            string templateFile = "";
            string fileFullPath = "";
            ProjectFolder selectedFolder = projectPanel1.GetCurrentlySelectedFolder();
            if (selectedFolder.FolderName == "")
            {
                fileFullPath = Path.Combine(openProjectPath, fileName);
            }
            else
            {
                fileFullPath = Path.Combine(Path.Combine(openProjectPath, selectedFolder.GetFolderPath()), fileName);
            }

            switch (fileType)
            {
                case ModStudioFileTypes.ModxMod:
                    NewModxFileWindow(fileFullPath);
                    return;
                case ModStudioFileTypes.TextMod:
                    NewTextModFileWindow(fileFullPath);
                    return;
                case ModStudioFileTypes.PhpFile:
                    if (openProject.PhpbbVersion.DeterminePrimary().StartsWith("2.0"))
                    {
                        templateFile = "phpbb2.php";
                    }
                    else if (openProject.PhpbbVersion.DeterminePrimary().StartsWith("3.0"))
                    {
                        templateFile = "phpbb3.php";
                    }
                    break;
                case ModStudioFileTypes.HtmlFile:
                    templateFile = "html.html";
                    break;
                case ModStudioFileTypes.TemplateFile:
                    if (openProject.PhpbbVersion.DeterminePrimary().StartsWith("2.0"))
                    {
                        templateFile = "phpbb2.tpl";
                        fileFullPath = CleanFileExtension(fileFullPath) + ".tpl";
                    }
                    else if (openProject.PhpbbVersion.DeterminePrimary().StartsWith("3.0"))
                    {
                        templateFile = "phpbb3.html";
                        fileFullPath = CleanFileExtension(fileFullPath) + ".html";
                    }
                    break;
                case ModStudioFileTypes.CascadingStyleSheet:
                    templateFile = "css.css";
                    break;
                case ModStudioFileTypes.TextFile:
                    templateFile = "text.txt";
                    break;
                case ModStudioFileTypes.XsltFile:
                    templateFile = "xslt.xsl";
                    break;
                case ModStudioFileTypes.Gpl2License:
                    templateFile = "gpl.txt";
                    break;
                case ModStudioFileTypes.ProsilverXslt:
                    templateFile = "modx.prosilver.en.xsl";
                    break;
                case ModStudioFileTypes.SubsilverXslt:
                    templateFile = "modx.subsilver.en.xsl";
                    break;
            }

            // call the code to open a window once, that way we can add additional smarts to
            // prevent file conflicts
            if (!string.IsNullOrEmpty(templateFile))
            {
                if (File.Exists(fileFullPath))
                {
                    if (MessageBox.Show("File already exists, overwrite?", "Overwrite", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            File.Delete(fileFullPath);
                        }
                        catch
                        {
                            MessageBox.Show("Cannot create file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                // copy the template file
                File.Copy(Path.Combine(templateDirectory, templateFile), fileFullPath);

                // TODO: use the smart file determination to open the new window
                if (open)
                {
                    NewOpenTextEditorFileWindow(fileFullPath);
                }
                else
                {
                    ProjectFile pf = new ProjectFile(CleanProjectFileNameRelative(fileName), BuildAction.Package, false);
                    AddFileToCurrentProjectFolder(pf);
                }
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string CleanProjectFileNameRelative(string fileName)
        {
            if (fileName.StartsWith(openProjectPath))
            {
                fileName = fileName.Remove(0, openProjectPath.Length);
                fileName = fileName.TrimStart(new char[] { Path.DirectorySeparatorChar });
            }
            return fileName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string CleanFileExtension(string fileName)
        {
            int l = fileName.LastIndexOf('.');
            if (l >= 0)
            {
                return fileName.Remove(l);
            }
            else
            {
                return fileName;
            }
        }

        private void NewTextEditorFileWindow(string fileExt)
        {
            TextEditor newEditor = new TextEditor();
            newEditor.MdiParent = this;
            newEditor.Text = "untitled." + fileExt;
            newEditor.Show();
        }

        private void OpenTextEditorFileWindow(string fileName)
        {
            TextEditor newEditor = new TextEditor(fileName);
            newEditor.MdiParent = this;
            newEditor.Text = fileName;
            newEditor.Show();
        }

        private void NewOpenTextEditorFileWindow(string fileName)
        {
            FileInfo fi = new FileInfo(fileName);
            OpenTextEditorFileWindow(fileName);

            // add the file to the project
            ProjectFile pf = new ProjectFile(fi.Name, BuildAction.Package, true);
            AddFileToCurrentProjectFolder(pf);
        }

        private void NewModFileWindow(bool isModx)
        {
            ModEditor newEditor = new ModEditor(isModx);
            newEditor.MdiParent = this;
            newEditor.ThisMod.Header.Title.Add("Untitled Mod", "en");
            newEditor.Show();
        }

        private void NewModFileWindow(bool isModx, string fileName)
        {
            if (openProject == null)
            {
                return;
            }

            if (File.Exists(Path.Combine(openProjectPath, fileName)))
            {
                if (MessageBox.Show("A file with the name already exists, overwrite?", "File already exists", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    // TODO: remove existing file from project
                }
            }

            ModEditor newEditor = new ModEditor(isModx);
            newEditor.MdiParent = this;
            newEditor.ThisMod.Header.Title.Add(CleanFileExtension(CleanProjectFileNameRelative(fileName)), "en");
            newEditor.Text = fileName;
            newEditor.Show();
            newEditor.SetModified();
            newEditor.SaveFile();

            // add the new MODX Window to the project
            ProjectFile pf = new ProjectFile(CleanProjectFileNameRelative(fileName), BuildAction.Package, true);
            AddFileToCurrentProjectFolder(pf);
        }

        private void NewModxFileWindow()
        {
            NewModFileWindow(true);
        }

        private void NewModxFileWindow(string fileName)
        {
            NewModFileWindow(true, fileName);
        }

        private void OpenModFileWindow(string fileName)
        {
            ModEditor newEditor = new ModEditor(fileName);
            newEditor.MdiParent = this;
            newEditor.Text = fileName;
            newEditor.Show();

            if (openProject != null)
            {
                if (fileName.StartsWith(openProjectPath))
                {
                    projectPanel1.OpenMod(fileName.Remove(0, openProjectPath.Length).TrimStart(new char[] { Path.DirectorySeparatorChar }), newEditor.ThisMod.Actions);
                }
            }
        }

        private void OpenModxFileWindow(string fileName)
        {
            OpenModFileWindow(fileName);
        }

        private void NewTextModFileWindow()
        {
            NewModFileWindow(false);
        }

        private void NewTextModFileWindow(string fileName)
        {
            NewModFileWindow(false, fileName);
        }

        private void OpenTextModFileWindow(string fileName)
        {
            OpenModFileWindow(fileName);
        }

	}



}
