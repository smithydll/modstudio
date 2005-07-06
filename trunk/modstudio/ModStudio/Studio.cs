/***************************************************************************
 *                              Studio.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: Studio.cs,v 1.1 2005-07-06 05:13:25 smithydll Exp $
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
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Studio()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Studio));
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.menuItemFile = new System.Windows.Forms.MenuItem();
			this.menuItemEdit = new System.Windows.Forms.MenuItem();
			this.menuItemFileNew = new System.Windows.Forms.MenuItem();
			this.menuItemFileOpen = new System.Windows.Forms.MenuItem();
			this.menuItemFileClose = new System.Windows.Forms.MenuItem();
			this.menuItemSep1 = new System.Windows.Forms.MenuItem();
			this.menuItemFileSave = new System.Windows.Forms.MenuItem();
			this.menuItemFileSaveAs = new System.Windows.Forms.MenuItem();
			this.menuItemFileSaveAll = new System.Windows.Forms.MenuItem();
			this.menuItemSep2 = new System.Windows.Forms.MenuItem();
			this.menuItemFileExit = new System.Windows.Forms.MenuItem();
			this.menuItemSep3 = new System.Windows.Forms.MenuItem();
			this.menuItemFileValidate = new System.Windows.Forms.MenuItem();
			this.menuItemFileExport = new System.Windows.Forms.MenuItem();
			this.menuItemView = new System.Windows.Forms.MenuItem();
			this.menuItemTools = new System.Windows.Forms.MenuItem();
			this.menuItemWindow = new System.Windows.Forms.MenuItem();
			this.menuItemHelp = new System.Windows.Forms.MenuItem();
			this.menuItemHelpHelp = new System.Windows.Forms.MenuItem();
			this.menuItemSep4 = new System.Windows.Forms.MenuItem();
			this.menuItemHelpAbout = new System.Windows.Forms.MenuItem();
			this.menuItemToolsOptions = new System.Windows.Forms.MenuItem();
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
			// menuItemEdit
			// 
			this.menuItemEdit.Enabled = ((bool)(resources.GetObject("menuItemEdit.Enabled")));
			this.menuItemEdit.Index = 1;
			this.menuItemEdit.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemEdit.Shortcut")));
			this.menuItemEdit.ShowShortcut = ((bool)(resources.GetObject("menuItemEdit.ShowShortcut")));
			this.menuItemEdit.Text = resources.GetString("menuItemEdit.Text");
			this.menuItemEdit.Visible = ((bool)(resources.GetObject("menuItemEdit.Visible")));
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
			// 
			// menuItemFileSaveAs
			// 
			this.menuItemFileSaveAs.Enabled = ((bool)(resources.GetObject("menuItemFileSaveAs.Enabled")));
			this.menuItemFileSaveAs.Index = 5;
			this.menuItemFileSaveAs.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemFileSaveAs.Shortcut")));
			this.menuItemFileSaveAs.ShowShortcut = ((bool)(resources.GetObject("menuItemFileSaveAs.ShowShortcut")));
			this.menuItemFileSaveAs.Text = resources.GetString("menuItemFileSaveAs.Text");
			this.menuItemFileSaveAs.Visible = ((bool)(resources.GetObject("menuItemFileSaveAs.Visible")));
			// 
			// menuItemFileSaveAll
			// 
			this.menuItemFileSaveAll.Enabled = ((bool)(resources.GetObject("menuItemFileSaveAll.Enabled")));
			this.menuItemFileSaveAll.Index = 6;
			this.menuItemFileSaveAll.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemFileSaveAll.Shortcut")));
			this.menuItemFileSaveAll.ShowShortcut = ((bool)(resources.GetObject("menuItemFileSaveAll.ShowShortcut")));
			this.menuItemFileSaveAll.Text = resources.GetString("menuItemFileSaveAll.Text");
			this.menuItemFileSaveAll.Visible = ((bool)(resources.GetObject("menuItemFileSaveAll.Visible")));
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
			// menuItemSep3
			// 
			this.menuItemSep3.Enabled = ((bool)(resources.GetObject("menuItemSep3.Enabled")));
			this.menuItemSep3.Index = 7;
			this.menuItemSep3.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemSep3.Shortcut")));
			this.menuItemSep3.ShowShortcut = ((bool)(resources.GetObject("menuItemSep3.ShowShortcut")));
			this.menuItemSep3.Text = resources.GetString("menuItemSep3.Text");
			this.menuItemSep3.Visible = ((bool)(resources.GetObject("menuItemSep3.Visible")));
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
			// menuItemFileExport
			// 
			this.menuItemFileExport.Enabled = ((bool)(resources.GetObject("menuItemFileExport.Enabled")));
			this.menuItemFileExport.Index = 8;
			this.menuItemFileExport.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemFileExport.Shortcut")));
			this.menuItemFileExport.ShowShortcut = ((bool)(resources.GetObject("menuItemFileExport.ShowShortcut")));
			this.menuItemFileExport.Text = resources.GetString("menuItemFileExport.Text");
			this.menuItemFileExport.Visible = ((bool)(resources.GetObject("menuItemFileExport.Visible")));
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
			// menuItemToolsOptions
			// 
			this.menuItemToolsOptions.Enabled = ((bool)(resources.GetObject("menuItemToolsOptions.Enabled")));
			this.menuItemToolsOptions.Index = 0;
			this.menuItemToolsOptions.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItemToolsOptions.Shortcut")));
			this.menuItemToolsOptions.ShowShortcut = ((bool)(resources.GetObject("menuItemToolsOptions.ShowShortcut")));
			this.menuItemToolsOptions.Text = resources.GetString("menuItemToolsOptions.Text");
			this.menuItemToolsOptions.Visible = ((bool)(resources.GetObject("menuItemToolsOptions.Visible")));
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
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Closed += new System.EventHandler(this.Studio_Closed);

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
	}
}
