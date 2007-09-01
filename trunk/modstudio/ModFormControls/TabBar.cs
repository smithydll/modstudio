using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace ModFormControls
{
	/// <summary>
	/// Summary description for TabBar.
	/// </summary>
	public class TabBar : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Label closeLabel;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel tabPanel;
		private TabBarTabCollection tabs;
		public int selectedIndex;
		public TabBarTab selectedTab;
		private System.Windows.Forms.Label leftLabel;
		private System.Windows.Forms.Label rightLabel;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TabBar()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			this.SetStyle(ControlStyles.AllPaintingInWmPaint |
				ControlStyles.DoubleBuffer |
				ControlStyles.UserPaint, true);
			this.UpdateStyles();

			this.tabs = new TabBarTabCollection();

			// TODO: Add any initialization after the InitializeComponent call
			SelectedIndex = -1;
			SelectedTab = null;

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
            this.closeLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rightLabel = new System.Windows.Forms.Label();
            this.leftLabel = new System.Windows.Forms.Label();
            this.tabPanel = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // closeLabel
            // 
            this.closeLabel.Font = new System.Drawing.Font("Webdings", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.closeLabel.Location = new System.Drawing.Point(128, 4);
            this.closeLabel.Name = "closeLabel";
            this.closeLabel.Size = new System.Drawing.Size(16, 20);
            this.closeLabel.TabIndex = 0;
            this.closeLabel.Text = "r";
            this.closeLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.closeLabel.MouseLeave += new System.EventHandler(this.closeLabel_MouseLeave);
            this.closeLabel.Click += new System.EventHandler(this.closeLabel_Click);
            this.closeLabel.MouseEnter += new System.EventHandler(this.closeLabel_MouseEnter);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rightLabel);
            this.panel1.Controls.Add(this.leftLabel);
            this.panel1.Controls.Add(this.tabPanel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(150, 24);
            this.panel1.TabIndex = 1;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // rightLabel
            // 
            this.rightLabel.Font = new System.Drawing.Font("Webdings", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.rightLabel.Location = new System.Drawing.Point(112, 5);
            this.rightLabel.Name = "rightLabel";
            this.rightLabel.Size = new System.Drawing.Size(16, 20);
            this.rightLabel.TabIndex = 2;
            this.rightLabel.Text = "4";
            this.rightLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rightLabel.MouseLeave += new System.EventHandler(this.closeLabel_MouseLeave);
            this.rightLabel.Click += new System.EventHandler(this.rightLabel_Click);
            this.rightLabel.MouseEnter += new System.EventHandler(this.closeLabel_MouseEnter);
            // 
            // leftLabel
            // 
            this.leftLabel.Font = new System.Drawing.Font("Webdings", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.leftLabel.Location = new System.Drawing.Point(96, 5);
            this.leftLabel.Name = "leftLabel";
            this.leftLabel.Size = new System.Drawing.Size(16, 20);
            this.leftLabel.TabIndex = 1;
            this.leftLabel.Text = "3";
            this.leftLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.leftLabel.MouseLeave += new System.EventHandler(this.closeLabel_MouseLeave);
            this.leftLabel.Click += new System.EventHandler(this.leftLabel_Click);
            this.leftLabel.MouseEnter += new System.EventHandler(this.closeLabel_MouseEnter);
            // 
            // tabPanel
            // 
            this.tabPanel.Location = new System.Drawing.Point(4, 4);
            this.tabPanel.Name = "tabPanel";
            this.tabPanel.Size = new System.Drawing.Size(40, 16);
            this.tabPanel.TabIndex = 0;
            // 
            // TabBar
            // 
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.closeLabel);
            this.Controls.Add(this.panel1);
            this.Name = "TabBar";
            this.Size = new System.Drawing.Size(150, 24);
            this.Resize += new System.EventHandler(this.TabBar_Resize);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		public TabBarTabCollection Tabs
		{
			get
			{
				return tabs;
			}
			set
			{
				tabs = value;
			}
		}

		public int SelectedIndex
		{
			get
			{
				return selectedIndex;
			}
			set
			{
				selectedIndex = value;
				this.updateTabs();
			}
		}

		public TabBarTab SelectedTab
		{
			get
			{
				return selectedTab;
			}
			set
			{
				selectedTab = value;
				this.updateTabs();
			}
		}

		private void TabBar_Resize(object sender, System.EventArgs e)
		{
			closeLabel.Left = this.Width - closeLabel.Width - 4;
			rightLabel.Left = this.Width - rightLabel.Width - closeLabel.Width - 4;
			leftLabel.Left = this.Width - leftLabel.Width - rightLabel.Width - closeLabel.Width - 4;
			tabPanel.Top = 4;
			tabPanel.Left = 4;
			tabPanel.Width = leftLabel.Left - 4;
			tabPanel.Height = 28;
			this.Refresh();

			int leftmost = 0;
			int rightmost = int.MinValue;
			foreach (Control ctrl in tabPanel.Controls)
			{
				leftmost = (leftmost < ctrl.Left) ? leftmost : ctrl.Left;
				rightmost = (rightmost > ctrl.Left + ctrl.Width) ? rightmost : ctrl.Left + ctrl.Width;
			}
			bool overflow = (rightmost - leftmost > tabPanel.Width);
			if (!overflow && scrollPosn < 0)
			{
				foreach (Control ctrl in tabPanel.Controls)
				{
					ctrl.Left += (0 - leftmost);
				}
			}
			UpdateScrollPosn();
		}

		private void UpdateScrollPosn()
		{
			int leftmost = 0;
			foreach (Control ctrl in tabPanel.Controls)
			{
				leftmost = (leftmost < ctrl.Left) ? leftmost : ctrl.Left;
			}
			scrollPosn = leftmost;
		}

		public void UpdateRightMargin()
		{
			int leftmost = 0;
			int rightmost = int.MinValue;
			foreach (Control ctrl in tabPanel.Controls)
			{
				leftmost = (leftmost < ctrl.Left) ? leftmost : ctrl.Left;
				rightmost = (rightmost > ctrl.Left + ctrl.Width) ? rightmost : ctrl.Left + ctrl.Width;
			}
			bool overflow = (rightmost - leftmost > tabPanel.Width);
			Console.WriteLine(overflow + "," + rightmost + "," + tabPanel.Width);
			if (!overflow && scrollPosn < 0)
			{
				foreach (Control ctrl in tabPanel.Controls)
				{
					ctrl.Left += (0 - leftmost);
				}
			}
			UpdateScrollPosn();
			if (overflow && rightmost < tabPanel.Width)
			{
				Console.WriteLine("winnar");
				scrollPosn = leftmost + (tabPanel.Width - rightmost);
			}
			updateTabs();
		}

		private void closeLabel_MouseEnter(object sender, System.EventArgs e)
		{
			((Label)sender).ForeColor = Color.Red;
		}

		private void closeLabel_MouseLeave(object sender, System.EventArgs e)
		{
			((Label)sender).ForeColor = SystemColors.ControlText;
		}

		private void Tabs_Modified(object sender, EventArgs e)
		{
			updateTabs();
		}

		public void updateTabs()
		{
			Console.WriteLine(scrollPosn);
			int left = scrollPosn;
			this.tabPanel.SuspendLayout();
			int i = 0;
			foreach (TabBarTab tab in tabs)
			{
				this.tabPanel.Controls.Add(tab);
				tab.Visible = true;
				tab.Left = left;
				left += tab.Width + 1;
				if (SelectedIndex == i)
				{
					tab.SetSelected();
				}
				else if (SelectedTab == tab)
				{
					tab.SetSelected();
					selectedIndex = i;
				}
				else
				{
					tab.SetDeSelected();
				}
				tab.TabIndex = i;
				tab.Click += new EventHandler(tab_Click);
				tab.TabSelect += new ModFormControls.TabBarTab.TabSelectHandler(tab_TabSelect);
				i++;
			}
			UpdateScroll();
			this.tabPanel.ResumeLayout();
		}

		private void UpdateScroll()
		{
			int leftmost = 0;
			int rightmost = int.MinValue;
			foreach (Control ctrl in tabPanel.Controls)
			{
				leftmost = (leftmost < ctrl.Left) ? leftmost : ctrl.Left;
				rightmost = (rightmost > ctrl.Left + ctrl.Width) ? rightmost : ctrl.Left + ctrl.Width;
			}
			leftLabel.Enabled = (leftmost < 0);
			rightLabel.Enabled = (rightmost > tabPanel.Width);
			closeLabel.Enabled = (tabs.Count > 0);
		}

		public delegate void TabCloseHandler(object sender, TabCloseEventArgs e);
		public event TabCloseHandler TabClose;
		public delegate void TabSelectedIndexChangedHandler(object sender, TabSelectedIndexChangedEventArgs e);
		public event TabSelectedIndexChangedHandler TabSelectedIndexChanged;

		private void closeLabel_Click(object sender, System.EventArgs e)
		{
			this.TabClose(this, new TabCloseEventArgs(this.selectedIndex));
		}

		private void tab_Click(object sender, EventArgs e)
		{
		}

		private void tab_TabSelect(object sender, TabSelectEventArgs e)
		{
			if (selectedIndex >= 0)
			{
				tabs[selectedIndex].SetDeSelected();
			}
			tabs[e.Index].SetSelected();
			if (selectedIndex != e.Index)
			{
				this.TabSelectedIndexChanged(this, new TabSelectedIndexChangedEventArgs(e.Index));
			}
			selectedIndex = e.Index;
		}

		private void panel1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			e.Graphics.DrawLine(new Pen(new SolidBrush(SystemColors.ControlLight),1.0F),this.Width - 1,0,this.Width - 1,this.Height);
			e.Graphics.DrawLine(new Pen(new SolidBrush(SystemColors.ControlDark),1.0F),0,0,this.Width,0);
			e.Graphics.DrawLine(new Pen(new SolidBrush(SystemColors.ControlDark),1.0F),0,0,0,this.Height);
		}

		const int scrollAmount = 120;
		private int scrollPosn = 0;
		private void leftLabel_Click(object sender, System.EventArgs e)
		{
			bool flag = false;
			int leftmost = 0;
			int rightmost = int.MinValue;
			foreach (Control ctrl in tabPanel.Controls)
			{
				leftmost = (leftmost < ctrl.Left) ? leftmost : ctrl.Left;
				rightmost = (rightmost > ctrl.Left + ctrl.Width) ? rightmost : ctrl.Left + ctrl.Width;
			}
			bool overflow = (rightmost - leftmost > tabPanel.Width);
			flag = (leftmost + scrollAmount <= 0 && overflow);
			if (flag)
			{
				foreach (Control ctrl in tabPanel.Controls)
				{
					ctrl.Left += scrollAmount;
				}
			}
			else if (leftmost < 0)
			{
				foreach (Control ctrl in tabPanel.Controls)
				{
					ctrl.Left += (0 - leftmost);
				}
			}
			UpdateScrollPosn();
			UpdateScroll();
		}

		private void rightLabel_Click(object sender, System.EventArgs e)
		{
			bool flag = false;
			int leftmost = 0;
			int rightmost = int.MinValue;
			foreach (Control ctrl in tabPanel.Controls)
			{
				leftmost = (leftmost < ctrl.Left) ? leftmost : ctrl.Left;
				rightmost = (rightmost > ctrl.Left + ctrl.Width) ? rightmost : ctrl.Left + ctrl.Width;
			}
			bool overflow = (rightmost - leftmost > tabPanel.Width);
			flag = (rightmost - scrollAmount >= tabPanel.Width && overflow);
			if (flag)
			{
				foreach (Control ctrl in tabPanel.Controls)
				{
					ctrl.Left -= scrollAmount;
				}
			}
			else if (rightmost > tabPanel.Width)
			{
				foreach (Control ctrl in tabPanel.Controls)
				{
					ctrl.Left -= (rightmost - tabPanel.Width);
				}
			}
			UpdateScrollPosn();
			UpdateScroll();
		}
	}

	public class TabCloseEventArgs: EventArgs 
	{
		public int Index;
		public TabCloseEventArgs(int index) 
		{
			this.Index = index;
		}
	}

	public class TabSelectedIndexChangedEventArgs: EventArgs 
	{
		public int Index;
		public TabSelectedIndexChangedEventArgs(int index) 
		{
			this.Index = index;
		}
	}
}
