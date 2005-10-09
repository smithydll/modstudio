/***************************************************************************
 *                            LocalisedTextBox.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: LocalisedTextBox.cs,v 1.1 2005-10-09 11:22:28 smithydll Exp $
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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using ModTemplateTools;
using ModTemplateTools.DataStructures;

namespace ModFormControls
{
	/// <summary>
	/// Summary description for LocalisedTextBox.
	/// </summary>
	public class LocalisedTextBox : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ContextMenu contextMenuLanguages;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private PropertyLang textLang;
		private System.Windows.Forms.MenuItem[] menuItems;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public LocalisedTextBox()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.contextMenuLanguages = new System.Windows.Forms.ContextMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Location = new System.Drawing.Point(0, 0);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(280, 20);
			this.textBox1.TabIndex = 0;
			this.textBox1.Text = "";
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label1.Dock = System.Windows.Forms.DockStyle.Right;
			this.label1.Location = new System.Drawing.Point(280, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 20);
			this.label1.TabIndex = 1;
			this.label1.Text = "en-GB";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label1.Click += new System.EventHandler(this.label1_Click);
			// 
			// contextMenuLanguages
			// 
			this.contextMenuLanguages.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																								 this.menuItem1,
																								 this.menuItem2});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.Text = "&Add Localisation";
			this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.Text = "-";
			// 
			// LocalisedTextBox
			// 
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label1);
			this.Name = "LocalisedTextBox";
			this.Size = new System.Drawing.Size(320, 20);
			this.ResumeLayout(false);

		}
		#endregion

		private void label1_Click(object sender, System.EventArgs e)
		{
			contextMenuLanguages.Show(label1, new Point(0, 20));
		}

		private void menuItem1_Click(object sender, System.EventArgs e)
		{
		
		}

		public void menuItems_Click(object sender, System.EventArgs e)
		{
			textLang[label1.Text] = textBox1.Text;
			textBox1.Text = textLang[((MenuItem)sender).Text]; //textLang.GetValue(((MenuItem)sender).Text);
			label1.Text = ((MenuItem)sender).Text;
		}

		public override string Text
		{
			get
			{
					return textBox1.Text;
			}
			set
			{
				textBox1.Text = value;
			}
		}

		public PropertyLang TextLang
		{
			get
			{
				textLang[label1.Text] = textBox1.Text;
				return textLang;
			}
			set
			{
				textLang = value;
				if (textLang != null)
				{
					menuItems = new MenuItem[textLang.Count];
					int i = 0;
					foreach (string Language in textLang)
					{
						menuItems[i] = new MenuItem(Language);
						menuItems[i].Click += new System.EventHandler(this.menuItems_Click);
						contextMenuLanguages.MenuItems.Add(menuItems[i]);
						i++;
					}
					if (textLang.Count > 0)
					{
						textBox1.Text = textLang[menuItems[0].Text];
						label1.Text = menuItems[0].Text;
					}
				}
			}
		}

		public bool LanguageSelectorVisible
		{
			get
			{
				return label1.Visible;
			}
			set
			{
				label1.Visible = value;
			}
		}

		public bool Multiline
		{
			get
			{
				return textBox1.Multiline;
			}
			set
			{
				textBox1.Multiline = value;
			}
		}
	}
}
