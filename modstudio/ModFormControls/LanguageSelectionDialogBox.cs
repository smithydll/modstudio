/***************************************************************************
 *                       LanguageSelectionDialogBox.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: LanguageSelectionDialogBox.cs,v 1.3 2006-01-21 02:50:52 smithydll Exp $
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
using System.Resources;

namespace ModFormControls
{
	/// <summary>
	/// Summary description for LanguageSelectionDialogBox.
	/// </summary>
	public class LanguageSelectionDialogBox : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ComboBox comboBoxLanguage;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public LanguageSelectionDialogBox()
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
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.comboBoxLanguage = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(152, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Select a localisation to add:";
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(176, 32);
			this.button1.Name = "button1";
			this.button1.TabIndex = 1;
			this.button1.Text = "Add";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// comboBoxLanguage
			// 
			this.comboBoxLanguage.Items.AddRange(new object[] {
																  "en",
																  "en-AU",
																  "en-GB",
																  "en-NZ",
																  "en-US",
																  "fr",
																  "fr-CA",
																  "fr-FR",
																  "jp"});
			this.comboBoxLanguage.Location = new System.Drawing.Point(48, 32);
			this.comboBoxLanguage.Name = "comboBoxLanguage";
			this.comboBoxLanguage.Size = new System.Drawing.Size(121, 21);
			this.comboBoxLanguage.TabIndex = 2;
			// 
			// LanguageSelectionDialogBox
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(274, 64);
			this.Controls.Add(this.comboBoxLanguage);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LanguageSelectionDialogBox";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Add Language";
			this.Load += new System.EventHandler(this.LanguageSelectionDialogBox_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void LanguageSelectionDialogBox_Load(object sender, System.EventArgs e)
		{
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.Hide();
		}

		public string Language
		{
			get
			{
				return comboBoxLanguage.Text;
			}
			set
			{
				if (comboBoxLanguage.Items.IndexOf(value) == -1)
				{
					comboBoxLanguage.Items.Add(value);
				}
				comboBoxLanguage.Text = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public delegate void LanguageSelectionDialogBoxSaveHandler(object sender, LanguageSelectionDialogBoxSaveEventArgs e);
	}

	/// <summary>
	/// 
	/// </summary>
	public class LanguageSelectionDialogBoxSaveEventArgs: EventArgs 
	{
		/// <summary>
		/// 
		/// </summary>
		public string Language;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="language"></param>
		public LanguageSelectionDialogBoxSaveEventArgs(string language) 
		{
			this.Language = language;
		}
	}
}
