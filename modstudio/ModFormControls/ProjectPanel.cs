using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Phpbb.ModTeam.Tools.DataStructures;
using ModProjects;

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
        private ModProject project;
        private ImageList imageList1;
        private IContainer components;
        private string projectPath;
        private Dictionary<TreeNode, ProjectFile> fileNodes = new Dictionary<TreeNode, ProjectFile>();
        private Dictionary<TreeNode, ProjectFolder> folderNodes = new Dictionary<TreeNode, ProjectFolder>();

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectPanel));
            this.titleBarPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.toolBar1 = new System.Windows.Forms.ToolBar();
            this.projectTree = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
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
            this.projectTree.ImageIndex = 0;
            this.projectTree.ImageList = this.imageList1;
            this.projectTree.Location = new System.Drawing.Point(0, 44);
            this.projectTree.Name = "projectTree";
            this.projectTree.SelectedImageIndex = 0;
            this.projectTree.Size = new System.Drawing.Size(200, 212);
            this.projectTree.TabIndex = 2;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "text.ico");
            this.imageList1.Images.SetKeyName(1, "xml.ico");
            this.imageList1.Images.SetKeyName(2, "studio.ico");
            this.imageList1.Images.SetKeyName(3, "tlb_new.ico");
            this.imageList1.Images.SetKeyName(4, "gpl.ico");
            this.imageList1.Images.SetKeyName(5, "language.ico");
            this.imageList1.Images.SetKeyName(6, "phpBB.ico");
            this.imageList1.Images.SetKeyName(7, "folder.ico");
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
            this.PerformLayout();

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

        public ModProject Project
        {
            set
            {
                project = value;
                UpdateProject();
            }
        }
        public string ProjectPath
        {
            set
            {
                projectPath = value;
            }
        }

        public ProjectFolder GetCurrentlySelectedFolder()
        {
            if (projectTree.SelectedNode != null)
            {
                if (folderNodes.ContainsKey(projectTree.SelectedNode))
                {
                    return folderNodes[projectTree.SelectedNode];
                }

                if (fileNodes.ContainsKey(projectTree.SelectedNode))
                {
                    return fileNodes[projectTree.SelectedNode].GetParent();
                }
            }

            // null
            foreach (ProjectFolder pf in project.Folders)
            {
                if (pf.FolderName == "")
                {
                    return pf;
                }
            }
            return null;
        }

        public void UpdateProjectFolder(ProjectFolder projectFolder, TreeNode folderNode)
        {
            if (projectFolder.Folders != null)
            {
                foreach (ProjectFolder pf in projectFolder.Folders)
                {
                    TreeNode pfNode = new TreeNode(pf.FolderName);
                    folderNode.Nodes.Add(pfNode);
                    pfNode.ImageIndex = pfNode.SelectedImageIndex = 7;
                    folderNodes.Add(pfNode, pf);

                    if (!pf.IsCollapsed)
                    {
                        pfNode.Expand();
                    }
                    else
                    {
                        pfNode.Collapse();
                    }

                    UpdateProjectFolder(pf, pfNode);
                }
            }

            if (projectFolder.Files != null)
            {
                foreach (ProjectFile pf in projectFolder.Files)
                {
                    TreeNode currentNode = new TreeNode(pf.FileName);
                    folderNode.Nodes.Add(currentNode);
                    currentNode.ImageIndex = currentNode.SelectedImageIndex = 3;
                    fileNodes.Add(currentNode, pf);

                    // types expected:
                    //  .xml (MODX)
                    //  .mod (Text Template)
                    //  .txt (Text Template)
                    //  .php (PHP)
                    //  .php (language file
                    /*if (pf.IsOpen)
                    {
                        // TODO:
                        //
                        // query parent for the window owning the current node that is open
                        // query the mod for a list of files edited
                        // add the list of mods to the tree
                        //
                        // query window for icon type
                    }
                    else
                    {*/
                    // query for the file type so we can change it's icon
                    try
                    {
                        string fileFullPath = "";
                        if (projectFolder.FolderName == "")
                        {
                            fileFullPath = Path.Combine(projectPath, pf.FileName);
                        }
                        else
                        {
                            fileFullPath = Path.Combine(Path.Combine(projectPath, projectFolder.GetFolderPath()), pf.FileName);
                        }
                        FileInfo projectFileInfo = new FileInfo(fileFullPath);
                        ProjectFileType pft = ReadFirstBytesInFile(fileFullPath);

                        switch (projectFileInfo.Extension.ToLower())
                        {
                            case ".mod":
                                if (pft == ProjectFileType.TextFile)
                                {
                                    currentNode.ImageIndex = currentNode.SelectedImageIndex = 0;
                                    openModFiles.Add(pf.FileName, currentNode);
                                }
                                break;
                            case ".txt":
                                switch (pft)
                                {
                                    case ProjectFileType.Gpl:
                                        currentNode.ImageIndex = currentNode.SelectedImageIndex = 4;
                                        break;
                                    case ProjectFileType.TextMod:
                                        currentNode.ImageIndex = currentNode.SelectedImageIndex = 0;
                                        break;
                                }
                                openModFiles.Add(pf.FileName, currentNode);
                                break;
                            case ".xml":
                                if (pft == ProjectFileType.ModxMox)
                                {
                                    currentNode.ImageIndex = currentNode.SelectedImageIndex = 1;
                                    openModFiles.Add(pf.FileName, currentNode);
                                }
                                break;
                            case ".php":
                                Console.WriteLine(">>> " + pft.ToString());
                                switch (pft)
                                {
                                    case ProjectFileType.PhpFile:
                                        currentNode.ImageIndex = currentNode.SelectedImageIndex = 6;
                                        Console.WriteLine("*** PHP file ***");
                                        break;
                                    case ProjectFileType.LanguageFile:
                                        currentNode.ImageIndex = currentNode.SelectedImageIndex = 5;
                                        Console.WriteLine("*** Language file ***");
                                        break;
                                }
                                openModFiles.Add(pf.FileName, currentNode);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        }

        public void UpdateProject()
        {
            projectTree.Nodes.Clear();
            
            TreeNode projectNode = new TreeNode(project.ProjectName);
            projectTree.Nodes.Add(projectNode);
            projectNode.ImageIndex = projectNode.SelectedImageIndex = 2;

            foreach(ProjectFolder pf in project.Folders)
            {
                if (pf.FolderName == "")
                {
                    folderNodes.Add(projectNode, pf);
                }
                UpdateProjectFolder(pf, projectNode);
            }
            projectNode.Expand();
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

        private Dictionary<string, TreeNode> openModFiles = new Dictionary<string, TreeNode>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actions"></param>
        public void OpenMod(string fileName, ModActions actions)
        {
            foreach (ModAction ma in actions)
            {
                if (ma.Type == "OPEN")
                {
                    openModFiles[fileName].Nodes.Add(ma.Body);
                    openModFiles[fileName].Expand();
                }
            }
        }

        public void CloseMod(string fileName)
        {
            if (!this.IsDisposed)
            {
                openModFiles[fileName].Nodes.Clear();
            }
        }

        public static ProjectFileType ReadFirstBytesInFile(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open);
            if (!fs.CanRead)
            {
                fs.Close();
                return ProjectFileType.Undetermined;
            }

            ASCIIEncoding ascii = new ASCIIEncoding();
            StringBuilder fileContents = new StringBuilder();
            byte[] buffer = new byte[4];
            int offset = 0;
            int bytesRead = 0;
            while (offset < 512 && fs.CanRead && offset < (fs.Length - 4) && (bytesRead = fs.Read(buffer, 0, 4)) > 0)
            {
                if (bytesRead == 0)
                {
                    break;
                }

                fileContents.Append(ascii.GetString(buffer, 0, bytesRead));

                if (offset == 0)
                {
                    if (fileContents.ToString().StartsWith("##"))
                    {
                        fs.Close();
                        return ProjectFileType.TextMod;
                    }
                }

                if (offset >= 32)
                {
                    if (fileContents.ToString().StartsWith("		    GNU GENERAL PUBLIC LICENSE"))
                    {
                        fs.Close();
                        return ProjectFileType.Gpl;
                    }
                }

                if (offset > 192)
                {
                    string phpFileContents = fileContents.ToString();
                    if (phpFileContents.StartsWith("<?php"))
                    {
                        Console.WriteLine("< PHP File >");
                        string[] phpFileLines = phpFileContents.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
                        if (phpFileLines.Length >= 3 && phpFileLines[2].StartsWith(" *                            lang_"))
                        {
                                fs.Close();
                                return ProjectFileType.LanguageFile;
                        }
                        else if (phpFileLines.Length >= 6 && phpFileLines[5].StartsWith("* @package language"))
                        {
                                fs.Close();
                                return ProjectFileType.LanguageFile;
                        }
                        else
                        {
                            /* 
                             * we've exhausted our patience to determine the type of php file,
                             * just assume it's a generic PHP file
                             */
                            fs.Close();
                            return ProjectFileType.PhpFile;
                        }
                    }
                }

                if (offset > 496)
                {
                    string xmlFileContents = fileContents.ToString();
                    if (xmlFileContents.StartsWith("<?xml"))
                    {
                        string[] xmlFileLines = xmlFileContents.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
                        if (xmlFileLines.Length >= 4)
                        {
                            if (xmlFileLines[3].StartsWith("<mod"))
                            {
                                fs.Close();
                                return ProjectFileType.ModxMox;
                            }
                        }
                    }
                }
                offset += 4;
            }

            // done reading bytes in the file, close it
            fs.Close();
            
            if (filename.EndsWith(".txt"))
            {
                return ProjectFileType.TextFile;
            }

            return ProjectFileType.Undetermined;
        }
	}

    public enum ProjectFileType
    {
        Undetermined,
        TextMod,
        ModxMox,
        TextFile,
        Lgpl,
        Gpl,
        LanguageFile,
        PhpFile,
        TemplateFile,
        HtmlFile,
        StyleSheet,
        ImageFile
    }
}
