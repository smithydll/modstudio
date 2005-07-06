/***************************************************************************
 *                              ModDisplayBox.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: ModDisplayBox.cs,v 1.1 2005-07-06 05:13:25 smithydll Exp $
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
			this.ModDisplayPanel.BackColor = System.Drawing.Color.AliceBlue;
			this.ModDisplayPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ModDisplayPanel.Location = new System.Drawing.Point(0, 0);
			this.ModDisplayPanel.Name = "ModDisplayPanel";
			this.ModDisplayPanel.Size = new System.Drawing.Size(480, 320);
			this.ModDisplayPanel.TabIndex = 0;
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
			this.ModBoxScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ModBoxScrollBar_Scroll);
			// 
			// ModDisplayBox
			// 
			this.Controls.Add(this.ModBoxScrollBar);
			this.Controls.Add(this.ModDisplayPanel);
			this.Name = "ModDisplayBox";
			this.Size = new System.Drawing.Size(480, 320);
			this.ResumeLayout(false);

		}
		#endregion

		private ModTemplateTools.PhpbbMod.ModActions Actions;
		private System.Windows.Forms.Panel ModDisplayPanel;
		private System.Windows.Forms.VScrollBar ModBoxScrollBar;
		private ModActionItem[] ActionItems;

		private void ModBoxScrollBar_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
		{
		
		}

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
						ActionItems[i].Location = new Point(0,0);
						ActionItems[i].Size = new Size(0,0);
						ActionItems[i].Visible = false;
						ActionItems[i].TabIndex = i + 2;
						ActionItems[i].Index = i;

						this.ModDisplayPanel.Controls.Add(this.ActionItems[i]);
					}
					this.ModDisplayPanel.ResumeLayout(false);
					this.ResumeLayout(false);
				}
			}
		}



	}
}
