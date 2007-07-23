using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace ModFormControls
{
	/// <summary>
	/// Summary description for TabBarTab.
	/// </summary>
	public class TabBarTab : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel1;
		private string text;
		private System.Windows.Forms.ImageList iconsImageList;
		private System.ComponentModel.IContainer components;
		private int iconIndex;
		private int tabIndex;
		private bool selected;

		public TabBarTab()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			selected = false;

			// TODO: Add any initialization after the InitializeComponent call

		}

		public TabBarTab(string text) : this()
		{
			this.Text = text;
			iconIndex = -1;
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(TabBarTab));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.iconsImageList = new System.Windows.Forms.ImageList(this.components);
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(4, 4);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(15, 16);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(20, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "label1";
			this.label1.Click += new System.EventHandler(this.label1_Click);
			this.label1.Resize += new System.EventHandler(this.label1_Resize);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.pictureBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(150, 24);
			this.panel1.TabIndex = 2;
			this.panel1.Click += new System.EventHandler(this.panel1_Click);
			this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
			// 
			// iconsImageList
			// 
			this.iconsImageList.ImageSize = new System.Drawing.Size(15, 16);
			this.iconsImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iconsImageList.ImageStream")));
			this.iconsImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// TabBarTab
			// 
			this.Controls.Add(this.panel1);
			this.Name = "TabBarTab";
			this.Size = new System.Drawing.Size(150, 24);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public override string Text
		{
			get
			{
				return text;
			}
			set
			{
				text = value;
				label1.Text = text;
			}
		}

		public int IconIndex
		{
			get
			{
				return iconIndex;
			}
			set
			{
				iconIndex = value;
				if (iconIndex >= 0)
				{
					pictureBox1.Image = iconsImageList.Images[iconIndex];
				}
				else
				{
					pictureBox1.Image = null;
				}
			}
		}

		public new int TabIndex
		{
			get
			{
				return tabIndex;
			}
			set
			{
				tabIndex = value;
			}
		}

		private void label1_Resize(object sender, System.EventArgs e)
		{
			this.Width = label1.Width + label1.Left + 4;
		}

		public delegate void TabSelectHandler(object sender, TabSelectEventArgs e);
		public event TabSelectHandler TabSelect;

		public void SetSelected()
		{
			//this.panel1.BackColor = SystemColors.Control;
			//this.panel1.BorderStyle = BorderStyle.Fixed3D;
			this.label1.Font = new Font(this.label1.Font, FontStyle.Bold);
			selected = true;
		}

		public void SetDeSelected()
		{
			this.panel1.BorderStyle = BorderStyle.None;
			this.label1.Font = new Font(this.label1.Font, FontStyle.Regular);
			selected = false;
		}

		private void panel1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if (selected)
			{
				panel1.BackColor = SystemColors.Control;
				e.Graphics.FillRectangle(new SolidBrush(SystemColors.Control), 0, 0, panel1.Width, panel1.Height);
				e.Graphics.DrawLine(new Pen(new SolidBrush(SystemColors.ControlDarkDark),1.0F),panel1.Width - 1,0,panel1.Width - 1,panel1.Height);
				e.Graphics.DrawLine(new Pen(new SolidBrush(Color.White),1.0F),0,0,panel1.Width, 0);
				e.Graphics.DrawLine(new Pen(new SolidBrush(Color.White),1.0F),0,0,0,panel1.Height);
			}
		}

		private void panel1_Click(object sender, System.EventArgs e)
		{
			this.TabSelect(this, new TabSelectEventArgs(tabIndex));
		}

		private void label1_Click(object sender, System.EventArgs e)
		{
			this.TabSelect(this, new TabSelectEventArgs(tabIndex));
		}

		private void pictureBox1_Click(object sender, System.EventArgs e)
		{
			this.TabSelect(this, new TabSelectEventArgs(tabIndex));
		}
	}

	public class TabSelectEventArgs: EventArgs 
	{
		public int Index;
		public TabSelectEventArgs(int index) 
		{
			this.Index = index;
		}
	}
}
