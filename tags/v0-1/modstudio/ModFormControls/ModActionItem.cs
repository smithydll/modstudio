/***************************************************************************
 *                              ModActionItem.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: ModActionItem.cs,v 1.4 2005-08-27 12:12:25 smithydll Exp $
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

namespace ModFormControls
{
	/// <summary>
	/// Summary description for ModActionItem.
	/// </summary>
	public class ModActionItem : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ModActionItem()
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.panel3 = new System.Windows.Forms.Panel();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.SlateGray;
			this.panel1.Location = new System.Drawing.Point(4, 4);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(472, 84);
			this.panel1.TabIndex = 0;
			// 
			// panel2
			// 
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel2.Controls.Add(this.label2);
			this.panel2.Controls.Add(this.label1);
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(472, 84);
			this.panel2.TabIndex = 1;
			this.panel2.Click += new System.EventHandler(this.panel2_Click);
			this.panel2.DoubleClick += new System.EventHandler(this.panel2_DoubleClick);
			// 
			// label2
			// 
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(0, 22);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(470, 60);
			this.label2.TabIndex = 1;
			this.label2.Text = "label2";
			this.label2.Click += new System.EventHandler(this.label2_Click);
			this.label2.DoubleClick += new System.EventHandler(this.panel2_DoubleClick);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(58, 22);
			this.label1.TabIndex = 0;
			this.label1.Text = "label1";
			this.label1.Click += new System.EventHandler(this.label2_Click);
			this.label1.DoubleClick += new System.EventHandler(this.panel2_DoubleClick);
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.AliceBlue;
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(0, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(480, 90);
			this.panel3.TabIndex = 2;
			// 
			// ModActionItem
			// 
			this.BackColor = System.Drawing.Color.AliceBlue;
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel3);
			this.Name = "ModActionItem";
			this.Size = new System.Drawing.Size(480, 90);
			this.Resize += new System.EventHandler(this.ModActionItem_Resize);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;


		private int ActionIndex;

		public delegate void ActionItemClickHandler(object sender, ActionItemClickEventArgs e);
		public event ActionItemClickHandler ItemClick;
		public event ActionItemClickHandler ItemDoubleClick;

		private void ModActionItem_Resize(object sender, System.EventArgs e)
		{
			panel1.Width = this.Width - 8;
			panel2.Width = this.Width - 8;
		}

		public int Index
		{
			set
			{
				ActionIndex = value;
			}
			get
			{
				return ActionIndex;
			}
		}

		System.Drawing.Color thisBackColour = Color.AliceBlue;

		private void panel2_Click(object sender, System.EventArgs e)
		{
			this.ItemClick(this, new ActionItemClickEventArgs(ActionIndex));
		}

		private void label2_Click(object sender, System.EventArgs e)
		{
			panel2_Click(null, null);
		}

		private void panel2_DoubleClick(object sender, System.EventArgs e)
		{
			this.ItemDoubleClick(this, new ActionItemClickEventArgs(ActionIndex));
		}
	
		public override Color BackColor
		{
			get
			{
				return thisBackColour;
			}
			set
			{
				base.BackColor = Color.AliceBlue;
				thisBackColour = value;

				panel2.BackColor = value;
				base.BackColor = Color.AliceBlue;
			}
		}

		public string ActionTitle
		{
			set
			{
				label1.Text = value;
			}
			get
			{
				return label1.Text;
			}
		}

		public string ActionBody
		{
			set
			{
				label2.Text = value;
			}
			get
			{
				return label2.Text;
			}
		}
	}

	public class ActionItemClickEventArgs: EventArgs 
	{
		public int Index;
		public ActionItemClickEventArgs(int index) 
		{
			this.Index = index;
		}
	}
}
