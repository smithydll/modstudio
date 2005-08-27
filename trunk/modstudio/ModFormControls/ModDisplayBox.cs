/***************************************************************************
 *                              ModDisplayBox.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: ModDisplayBox.cs,v 1.6 2005-08-27 12:12:25 smithydll Exp $
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

namespace ModFormControls
{
	/// <summary>
	/// Summary description for ModDisplayBox.
	/// </summary>
	public class ModDisplayBox : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ModDisplayBox()
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
			this.ModDisplayPanel = new System.Windows.Forms.Panel();
			this.ModBoxScrollBar = new System.Windows.Forms.VScrollBar();
			this.SuspendLayout();
			// 
			// ModDisplayPanel
			// 
			this.ModDisplayPanel.AutoScroll = true;
			this.ModDisplayPanel.BackColor = System.Drawing.Color.AliceBlue;
			this.ModDisplayPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ModDisplayPanel.Location = new System.Drawing.Point(0, 0);
			this.ModDisplayPanel.Name = "ModDisplayPanel";
			this.ModDisplayPanel.Size = new System.Drawing.Size(480, 320);
			this.ModDisplayPanel.TabIndex = 0;
			this.ModDisplayPanel.TabStop = true;
			// 
			// ModBoxScrollBar
			// 
			this.ModBoxScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
			this.ModBoxScrollBar.LargeChange = 1;
			this.ModBoxScrollBar.Location = new System.Drawing.Point(463, 0);
			this.ModBoxScrollBar.Maximum = 0;
			this.ModBoxScrollBar.Name = "ModBoxScrollBar";
			this.ModBoxScrollBar.Size = new System.Drawing.Size(17, 320);
			this.ModBoxScrollBar.TabIndex = 1;
			this.ModBoxScrollBar.Visible = false;
			// 
			// ModDisplayBox
			// 
			this.Controls.Add(this.ModBoxScrollBar);
			this.Controls.Add(this.ModDisplayPanel);
			this.Name = "ModDisplayBox";
			this.Size = new System.Drawing.Size(480, 320);
			this.Enter += new System.EventHandler(this.ModDisplayBox_Enter);
			this.ResumeLayout(false);

		}
		#endregion

		private ModTemplateTools.PhpbbMod.ModActions Actions;
		private System.Windows.Forms.Panel ModDisplayPanel;
		private System.Windows.Forms.VScrollBar ModBoxScrollBar;
		private ModActionItem[] ActionItems;
		private int selectedIndex = 0;

		/// <summary>
		/// 
		/// </summary>
		public ModTemplateTools.PhpbbMod.ModActions ModActions
		{
			set
			{
				Actions = value;

				if (ActionItems != null)
				{
					for (int i = 0; i < ActionItems.Length; i++)
					{
						ActionItems[i].Dispose();
					}
					ActionItems = null;
				}
				if (Actions.Actions != null)
				{
					ActionItems = new ModActionItem[Actions.Actions.Length];

					this.ModDisplayPanel.SuspendLayout();
					this.SuspendLayout();
					ModDisplayPanel.Height = ActionItems.Length * 100;
					for (int i = 0; i < ActionItems.Length; i++)
					{
					
						ActionItems[i] = new ModActionItem();
						ActionItems[i].Location = new Point(10, i * 100 + 10 - ModBoxScrollBar.Value);
						ActionItems[i].Visible = true;
						ActionItems[i].Size = new Size(this.Width - 20 - ModBoxScrollBar.Width, 90);
						ActionItems[i].Visible = false;
						ActionItems[i].TabIndex = i + 2;
						ActionItems[i].Index = i;
						ActionItems[i].ItemClick += new ModActionItem.ActionItemClickHandler(this.ActionItems_SelectedIndexChanged);
						ActionItems[i].ItemDoubleClick += new ModActionItem.ActionItemClickHandler(this.ActionItmes_ItemDoubleClick);

						this.ModDisplayPanel.Controls.Add(this.ActionItems[i]);
					}
					this.ModDisplayPanel.ResumeLayout(false);
					this.ResumeLayout(false);
				}
			}
		}

		public event ModActionItem.ActionItemClickHandler ItemDoubleClick;
		public event EventHandler SelectedIndexChanged;

		private void ActionItems_SelectedIndexChanged(object sender, ActionItemClickEventArgs e)
		{
			selectedIndex = e.Index;
			UpdateColours();
			this.SelectedIndexChanged(this, new EventArgs());
		}

		public void ActionItmes_ItemDoubleClick(object sender, ActionItemClickEventArgs e)
		{
			this.ItemDoubleClick(this, e);
		}

		public void UpdateColours()
		{
			this.ModDisplayPanel.SuspendLayout();
			this.SuspendLayout();
			if (Actions.Actions != null)
			{
				for (int i = 0; i < ActionItems.Length; i++)
				{
					ActionItems[i].SuspendLayout();
					switch (Actions.Actions[i].ActionType)
					{
						case "SQL":
							ActionItems[i].BackColor = Color.LightCyan;
							break;
						case "COPY":
						case "SAVE/CLOSE ALL FILES":
							ActionItems[i].BackColor = Color.LightCyan;
							break;
						case "OPEN":
							ActionItems[i].BackColor = Color.LightYellow;
							if (Actions.Actions[i + 1].ActionType != "FIND") 
							{
								ActionItems[i].BackColor = Color.LightPink;
								// TODO: reinvestigate this line
								//Actions.Actions[i].toolTip1.SetToolTip(ModActionItem[i].pictureBox1, "This OPEN action is empty, please add one or more FIND child actions.");
							}
							break;
						case "FIND":
							ActionItems[i].BackColor = Color.LightGreen;

							if (Actions.Actions[i+1].ActionType != "AFTER, ADD" && Actions.Actions[i + 1].ActionType != "BEFORE, ADD" && Actions.Actions[i + 1].ActionType != "REPLACE WITH" && Actions.Actions[i + 1].ActionType != "IN-LINE FIND") 
							{
								ActionItems[i].BackColor = Color.LightPink;
							}
							break;
						case "IN-LINE FIND":
							ActionItems[i].BackColor = Color.PaleGreen;
							break;
						case "REPLACE WITH":
						case "AFTER, ADD":
						case "BEFORE, ADD":
							ActionItems[i].BackColor = Color.LightGoldenrodYellow;
							break;
						case "IN-LINE REPLACE WITH":
						case "IN-LINE AFTER, ADD":
						case "IN-LINE BEFORE, ADD":
							ActionItems[i].BackColor = Color.LemonChiffon;
							break;

					}

					if (selectedIndex == i)
					{
						ActionItems[i].BackColor = Color.GhostWhite;
					}
					ActionItems[i].Refresh();
				}
			}
			this.ModDisplayPanel.ResumeLayout();
			this.ResumeLayout();
		}

		public void UpdateSize()
		{
			this.ModDisplayPanel.SuspendLayout();
			this.SuspendLayout();
			if (Actions.Actions != null)
			{
				for (int i = 0; i < ActionItems.Length; i++)
				{
					ActionItems[i].SuspendLayout();
					ActionItems[i].Height = 90;
					ActionItems[i].Visible = true;
					//ActionItems[i].Location = new Point(0,0);

					switch (Actions.Actions[i].ActionType)
					{
						case "SQL":
						case "COPY":
						case "SAVE/CLOSE ALL FILES":
						case "OPEN":
							ActionItems[i].Width = this.Width - 20 - ModBoxScrollBar.Width;
							break;
						case "FIND":
							ActionItems[i].Width = this.Width - 40 - ModBoxScrollBar.Width;
							break;
						case "IN-LINE FIND":
						case "REPLACE WITH":
						case "AFTER, ADD":
						case "BEFORE, ADD":
							ActionItems[i].Width = this.Width - 60 - ModBoxScrollBar.Width;
							break;
						case "IN-LINE REPLACE WITH":
						case "IN-LINE AFTER, ADD":
						case "IN-LINE BEFORE, ADD":
							ActionItems[i].Width = this.Width - 80 - ModBoxScrollBar.Width;
							break;

					}
					ActionItems[i].Refresh();
				}
			}
			this.ModDisplayPanel.ResumeLayout();
			this.ResumeLayout();
		}

		public void UpdateLayout()
		{
			this.ModDisplayPanel.SuspendLayout();
			this.SuspendLayout();
			if (Actions.Actions != null)
			{
				ModBoxScrollBar.Maximum = Actions.Actions.Length * 100;

				ModDisplayPanel.ScrollControlIntoView(ActionItems[0]);
				for (int i = 0; i < ActionItems.Length; i++)
				{
					ActionItems[i].SuspendLayout();
					ActionItems[i].Height = 90;
					ActionItems[i].Visible = true;
					//ActionItems[i].Location = new Point(0,0);

					switch (Actions.Actions[i].ActionType)
					{
						case "SQL":
							ActionItems[i].Width = this.Width - 20 - ModBoxScrollBar.Width;
							ActionItems[i].Location = new Point(10, i * 100 + 10 - ModBoxScrollBar.Value);
							ActionItems[i].BackColor = Color.LightCyan;

							break;
						case "COPY":
						case "SAVE/CLOSE ALL FILES":
							ActionItems[i].Width = this.Width - 20 - ModBoxScrollBar.Width;
							ActionItems[i].Location = new Point(10, i * 100 + 10 - ModBoxScrollBar.Value);
							ActionItems[i].BackColor = Color.LightCyan;

							break;
						case "OPEN":
							ActionItems[i].Width = this.Width - 20 - ModBoxScrollBar.Width;
							ActionItems[i].Location = new Point(10, i * 100 + 10 - ModBoxScrollBar.Value);
							ActionItems[i].BackColor = Color.LightYellow;
							if (Actions.Actions[i + 1].ActionType != "FIND") 
							{
								ActionItems[i].BackColor = Color.LightPink;
								// TODO: reinvestigate this line
								//Actions.Actions[i].toolTip1.SetToolTip(ModActionItem[i].pictureBox1, "This OPEN action is empty, please add one or more FIND child actions.");
							}
							break;
						case "FIND":
							ActionItems[i].Width = this.Width - 40 - ModBoxScrollBar.Width;
							ActionItems[i].Location = new Point(30, i * 100 + 10 - ModBoxScrollBar.Value);
							ActionItems[i].BackColor = Color.LightGreen;

							if (Actions.Actions[i+1].ActionType != "AFTER, ADD" && Actions.Actions[i + 1].ActionType != "BEFORE, ADD" && Actions.Actions[i + 1].ActionType != "REPLACE WITH" && Actions.Actions[i + 1].ActionType != "IN-LINE FIND") 
							{
								ActionItems[i].BackColor = Color.LightPink;
							}
							break;
						case "IN-LINE FIND":

							ActionItems[i].Width = this.Width - 60 - ModBoxScrollBar.Width;
							ActionItems[i].Location = new Point(50, i * 100 + 10 - ModBoxScrollBar.Value);
							ActionItems[i].BackColor = Color.PaleGreen;
							break;
						case "REPLACE WITH":
						case "AFTER, ADD":
						case "BEFORE, ADD":
							ActionItems[i].Width = this.Width - 60 - ModBoxScrollBar.Width;
							ActionItems[i].Location = new Point(50, i * 100 + 10 - ModBoxScrollBar.Value);
							ActionItems[i].BackColor = Color.LightGoldenrodYellow;
							break;
						case "IN-LINE REPLACE WITH":
						case "IN-LINE AFTER, ADD":
						case "IN-LINE BEFORE, ADD":
							ActionItems[i].Width = this.Width - 80 - ModBoxScrollBar.Width;
							ActionItems[i].Location = new Point(70, i * 100 + 10 - ModBoxScrollBar.Value);
							ActionItems[i].BackColor = Color.LemonChiffon;
							break;

					}
					// TODO: come back to these
					ActionItems[i].ActionTitle = Actions.Actions[i].ActionType;
					ActionItems[i].ActionBody = Actions.Actions[i].ActionBody;

					if (selectedIndex == i)
					{
						ActionItems[i].BackColor = Color.GhostWhite;
						ModDisplayPanel.ScrollControlIntoView(ActionItems[i]);
					}
					ActionItems[i].Refresh();
				}
			}
			this.ModDisplayPanel.ResumeLayout();
			this.ResumeLayout();
		}

		private void ModDisplayBox_Enter(object sender, System.EventArgs e)
		{
			ModDisplayPanel.Select();
		}

		public int SelectedIndex
		{
			set
			{
				if (ActionItems != null)
				{
					if (value < ActionItems.Length)
					{
						UpdateColours();
						selectedIndex = value;
						ModDisplayPanel.ScrollControlIntoView(ActionItems[value]);
						this.SelectedIndexChanged(this, new EventArgs());
					}
					else
					{
						// error
					}
				}
			}
			get
			{
				return selectedIndex;
			}
		}

	}
}
