/***************************************************************************
 *                            ModActionEditor.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: ModActionEditor.cs,v 1.13 2007-09-01 13:52:35 smithydll Exp $
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
using Phpbb.ModTeam.Tools;
using Phpbb.ModTeam.Tools.DataStructures;

namespace ModFormControls
{
	/// <summary>
	/// Summary description for ModActionEditor.
	/// </summary>
	public class ModActionEditor : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel4;
		private ICSharpCode.TextEditor.TextEditorControl textEditorControlActionBody;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ComboBox comboBoxActionType;
		private ModFormControls.LocalisedTextBox textEditorControlComment;

		public int actionIndex = 0;

		public delegate void ModActionEditorReturnHandler(object sender, ModActionEditorReturnEventArgs e);
		public event ModActionEditorReturnHandler Return;

		public ModActionEditor()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ModActionEditor));
			this.panel2 = new System.Windows.Forms.Panel();
			this.textEditorControlActionBody = new ICSharpCode.TextEditor.TextEditorControl();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel3 = new System.Windows.Forms.Panel();
			this.textEditorControlComment = new ModFormControls.LocalisedTextBox();
			this.panel5 = new System.Windows.Forms.Panel();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.comboBoxActionType = new System.Windows.Forms.ComboBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel4 = new System.Windows.Forms.Panel();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel5.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.AliceBlue;
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel2.Controls.Add(this.textEditorControlActionBody);
			this.panel2.Controls.Add(this.splitter1);
			this.panel2.Controls.Add(this.panel3);
			this.panel2.Controls.Add(this.comboBoxActionType);
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(472, 312);
			this.panel2.TabIndex = 6;
			// 
			// textEditorControlActionBody
			// 
			this.textEditorControlActionBody.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textEditorControlActionBody.Encoding = ((System.Text.Encoding)(resources.GetObject("textEditorControlActionBody.Encoding")));
			this.textEditorControlActionBody.IsIconBarVisible = false;
			this.textEditorControlActionBody.Location = new System.Drawing.Point(0, 26);
			this.textEditorControlActionBody.Name = "textEditorControlActionBody";
			this.textEditorControlActionBody.ShowEOLMarkers = true;
			this.textEditorControlActionBody.ShowSpaces = true;
			this.textEditorControlActionBody.ShowTabs = true;
			this.textEditorControlActionBody.ShowVRuler = true;
			this.textEditorControlActionBody.Size = new System.Drawing.Size(470, 181);
			this.textEditorControlActionBody.TabIndex = 3;
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter1.Location = new System.Drawing.Point(0, 207);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(470, 3);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.textEditorControlComment);
			this.panel3.Controls.Add(this.panel5);
			this.panel3.Controls.Add(this.label1);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 210);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(470, 100);
			this.panel3.TabIndex = 1;
			// 
			// textEditorControlComment
			// 
			this.textEditorControlComment.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textEditorControlComment.LanguageSelectorVisible = true;
			this.textEditorControlComment.Location = new System.Drawing.Point(0, 20);
			this.textEditorControlComment.Multiline = true;
			this.textEditorControlComment.Name = "textEditorControlComment";
			this.textEditorControlComment.Size = new System.Drawing.Size(382, 80);
			this.textEditorControlComment.TabIndex = 3;
			// 
			// panel5
			// 
			this.panel5.Controls.Add(this.button1);
			this.panel5.Controls.Add(this.button2);
			this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel5.Location = new System.Drawing.Point(382, 20);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(88, 80);
			this.panel5.TabIndex = 2;
			// 
			// button1
			// 
			this.button1.BackColor = System.Drawing.SystemColors.Control;
			this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(0, 0);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(88, 40);
			this.button1.TabIndex = 0;
			this.button1.Text = "OK";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.BackColor = System.Drawing.SystemColors.Control;
			this.button2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button2.Location = new System.Drawing.Point(0, 40);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(88, 40);
			this.button2.TabIndex = 1;
			this.button2.Text = "Cancel";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.SlateGray;
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(470, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Comment";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboBoxActionType
			// 
			this.comboBoxActionType.Dock = System.Windows.Forms.DockStyle.Top;
			this.comboBoxActionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.comboBoxActionType.Items.AddRange(new object[] {
																	"FIND",
																	"AFTER, ADD",
																	"BEFORE, ADD",
																	"REPLACE WITH",
																	"IN-LINE FIND",
																	"IN-LINE AFTER, ADD",
																	"IN-LINE BEFORE, ADD",
																	"IN-LINE REPLACE WITH"});
			this.comboBoxActionType.Location = new System.Drawing.Point(0, 0);
			this.comboBoxActionType.Name = "comboBoxActionType";
			this.comboBoxActionType.Size = new System.Drawing.Size(470, 26);
			this.comboBoxActionType.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.SlateGray;
			this.panel1.Location = new System.Drawing.Point(4, 4);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(472, 312);
			this.panel1.TabIndex = 5;
			// 
			// panel4
			// 
			this.panel4.BackColor = System.Drawing.Color.Transparent;
			this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel4.Location = new System.Drawing.Point(0, 0);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(480, 320);
			this.panel4.TabIndex = 7;
			// 
			// ModActionEditor
			// 
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel4);
			this.Name = "ModActionEditor";
			this.Size = new System.Drawing.Size(480, 320);
			this.Resize += new System.EventHandler(this.panel1_Resize);
			this.Leave += new System.EventHandler(this.ModActionEditor_Leave);
			this.panel2.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel5.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void panel1_Resize(object sender, System.EventArgs e)
		{
			panel1.Height = this.Height - 8;
			panel2.Height = this.Height - 8;
			panel1.Width = this.Width - 8;
			panel2.Width = this.Width - 8;
		}

		private void ModActionEditor_Leave(object sender, System.EventArgs e)
		{
			this.Hide();
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			this.Hide();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.Return(this, new ModActionEditorReturnEventArgs(actionIndex, string.Copy(comboBoxActionType.Text), string.Copy(textEditorControlActionBody.Text), textEditorControlComment.TextLang));
			this.Hide();
		}

		public void SetModAction(int index, string actionType, string actionBody, StringLocalised actionComment, bool isXml)
		{
			actionIndex = index;
			comboBoxActionType.Text = string.Copy(actionType);
			textEditorControlActionBody.Text = string.Copy(actionBody);
			StringLocalised tempLang = new StringLocalised();
			foreach (string Language in actionComment)
			{
				tempLang.Add(actionComment[Language], Language);
			}
			textEditorControlComment.TextLang = tempLang;

			if (isXml)
			{
				switch (actionType)
				{
					case "FIND":
						textEditorControlComment.Enabled = true;
						break;
					default:
						textEditorControlComment.Enabled = false;
						break;
				}
				textEditorControlComment.LanguageSelectorVisible = true;
			}
			else
			{
				textEditorControlComment.LanguageSelectorVisible = false;
				switch (actionType)
				{
					case "SAVE/CLOSE ALL FILES":
						textEditorControlComment.Enabled = false;
						break;
					default:
						textEditorControlComment.Enabled = true;
						break;
				}
			}
		}
	}

	public class ModActionEditorReturnEventArgs: EventArgs 
	{
		public int Index;
		public string ActionType;
		public string ActionBody;
		public StringLocalised ActionComment;
		public ModActionEditorReturnEventArgs(int index, string actionType, string actionBody, StringLocalised actionComment)
		{
			this.Index = index;
			this.ActionType = actionType;
			this.ActionBody = actionBody;
			this.ActionComment = actionComment;
		}
	}
}
