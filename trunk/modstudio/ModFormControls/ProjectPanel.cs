using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Phpbb.ModTeam.Tools.DataStructures;

namespace ModFormControls
{
	/// <summary>
	/// Summary description for ProjectPanel.
	/// </summary>
	public class ProjectPanel : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Panel titleBarPanel;
		private System.Windows.Forms.ToolBar toolBar1;
		private System.Windows.Forms.TreeView projectTree;
		private ModActions actions;
		private System.Windows.Forms.Label label1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ProjectPanel()
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
			this.titleBarPanel = new System.Windows.Forms.Panel();
			this.toolBar1 = new System.Windows.Forms.ToolBar();
			this.projectTree = new System.Windows.Forms.TreeView();
			this.label1 = new System.Windows.Forms.Label();
			this.titleBarPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// titleBarPanel
			// 
			this.titleBarPanel.BackColor = System.Drawing.SystemColors.Highlight;
			this.titleBarPanel.Controls.Add(this.label1);
			this.titleBarPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.titleBarPanel.Location = new System.Drawing.Point(0, 0);
			this.titleBarPanel.Name = "titleBarPanel";
			this.titleBarPanel.Size = new System.Drawing.Size(200, 16);
			this.titleBarPanel.TabIndex = 0;
			// 
			// toolBar1
			// 
			this.toolBar1.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.toolBar1.DropDownArrows = true;
			this.toolBar1.Location = new System.Drawing.Point(0, 16);
			this.toolBar1.Name = "toolBar1";
			this.toolBar1.ShowToolTips = true;
			this.toolBar1.Size = new System.Drawing.Size(200, 28);
			this.toolBar1.TabIndex = 1;
			this.toolBar1.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
			// 
			// projectTree
			// 
			this.projectTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.projectTree.ImageIndex = -1;
			this.projectTree.Location = new System.Drawing.Point(0, 44);
			this.projectTree.Name = "projectTree";
			this.projectTree.SelectedImageIndex = -1;
			this.projectTree.Size = new System.Drawing.Size(200, 212);
			this.projectTree.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Left;
			this.label1.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Project Explorer";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ProjectPanel
			// 
			this.Controls.Add(this.projectTree);
			this.Controls.Add(this.toolBar1);
			this.Controls.Add(this.titleBarPanel);
			this.Name = "ProjectPanel";
			this.Size = new System.Drawing.Size(200, 256);
			this.titleBarPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public ModActions Actions
		{
			get
			{
				return actions;
			}
			set
			{
				actions = value;
			}
		}

		public void UpdateActions()
		{
			projectTree.Nodes.Clear();
			ArrayList tempNodes = new ArrayList();
			foreach (ModAction ma in actions)
			{
				if (ma.Type == "OPEN")
				{
					TreeNode openNode = new TreeNode(ma.Body);
					openNode.BackColor = Color.PaleGreen;
					tempNodes.Add(openNode);
				}
			}
			TreeNode editNode = new TreeNode("Edited Files", (TreeNode[])tempNodes.ToArray(typeof(TreeNode)));
			TreeNode[] editNodes = {editNode};
			TreeNode fileNode = new TreeNode("file.txt", editNodes);
			TreeNode[] fileNodes = {fileNode};
			TreeNode tempNode = new TreeNode("New Project", fileNodes);
			projectTree.Nodes.Add(tempNode);
			tempNode.ExpandAll();
		}
	}
}
