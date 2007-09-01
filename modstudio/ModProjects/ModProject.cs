using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using Phpbb.ModTeam.Tools.DataStructures;

namespace ModProjects
{
	/// <summary>
	/// Summary description for ModProject.
	/// </summary>
	[XmlRoot("mod-project")]
	public class ModProject
	{
		private string buildPath;
		private ArrayList folders;
        private TargetVersionCases phpbbVerion;
        private string projectName;

		public ModProject()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        [XmlElement("name", typeof(string))]
        public string ProjectName
        {
            get
            {
                return projectName;
            }
            set
            {
                projectName = value;
            }
        }

		[XmlElement("build-path", typeof(string))]
		public string BuildPath
		{
			get
			{
				return buildPath;
			}
			set
			{
				buildPath = value;
			}
		}

        [XmlElement("phpbb-version", typeof(TargetVersionCases))]
        public TargetVersionCases PhpbbVersion
        {
            get
            {
                return phpbbVerion;
            }
            set
            {
                phpbbVerion = value;
            }
        }

		[XmlArray("main-folders")]
        [XmlArrayItem("main-folder", typeof(ProjectFolder))]
		public ArrayList Folders
		{
			get
			{
				return folders;
			}
			set
			{
				folders = value;

                foreach (ProjectFolder pf in folders)
                {
                    pf.SetParent(null);
                }

                Console.WriteLine("\"\"\" Folder Parents Re-synced (null)");
			}
		}

        public void AddFolder(ProjectFolder pf)
        {
            if (folders == null)
            {
                folders = new ArrayList();
            }
            folders.Add(pf);
            // force it to be null
            pf.SetParent(null);
        }

        public void ReSyncAllParents()
        {
            foreach (ProjectFolder pf in folders)
            {
                pf.SetParent(null);
                ReSyncParent(pf);
            }
        }

        private void ReSyncParent(ProjectFolder projectFolder)
        {
            foreach (ProjectFolder pf in projectFolder.Folders)
            {
                pf.SetParent(projectFolder);
                ReSyncParent(pf);
            }

            foreach (ProjectFile pf in projectFolder.Files)
            {
                pf.SetParent(projectFolder);
            }
        }
	}

    /// <summary>
    /// 
    /// </summary>
    public class ProjectFolder
    {
        private string folderName;
        private bool collapsed;
        private ArrayList files;
        private ArrayList folders;
        private ProjectFolder parent;

        /// <summary>
        /// 
        /// </summary>
        public ProjectFolder()
        {
            this.folderName = "/";
            this.collapsed = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName"></param>
        public ProjectFolder(string folderName)
        {
            this.folderName = folderName;
            this.collapsed = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="collapsed"></param>
        public ProjectFolder(string folderName, bool collapsed)
        {
            this.folderName = folderName;
            this.collapsed = collapsed;
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("name")]
        public string FolderName
        {
            get
            {
                return folderName;
            }
            set
            {
                folderName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("collapsed")]
        public bool IsCollapsed
        {
            get
            {
                return collapsed;
            }
            set
            {
                collapsed = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlArray("files")]
        [XmlArrayItem("file", typeof(ProjectFile))]
        public ArrayList Files
        {
            get
            {
                return files;
            }
            set
            {
                files = value;

                foreach (ProjectFile pf in files)
                {
                    pf.SetParent(this);
                    Console.WriteLine("\"\"\" File Parents Re-synced {0}", pf.FileName);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlArray("folders")]
        [XmlArrayItem("folder", typeof(ProjectFolder))]
        public ArrayList Folders
        {
            get
            {
                return folders;
            }
            set
            {
                folders = value;

                foreach (ProjectFolder pf in folders)
                {
                    pf.SetParent(this);
                    Console.WriteLine("\"\"\" Folder Parents Re-synced {0}", pf.FolderName);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pf"></param>
        public void AddFile(ProjectFile pf)
        {
            if (files == null)
            {
                files = new ArrayList();
            }
            files.Add(pf);
            pf.SetParent(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pf"></param>
        public void AddFolder(ProjectFolder pf)
        {
            if (folders == null)
            {
                folders = new ArrayList();
            }
            folders.Add(pf);
            pf.SetParent(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ProjectFolder GetParent()
        {
            return parent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pf"></param>
        public void SetParent(ProjectFolder pf)
        {
            this.parent = pf;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetFolderPath()
        {
            ProjectFolder currentFolder = this;
            string path = this.FolderName;

            while (currentFolder.GetParent() != null)
            {
                path = Path.Combine(path, currentFolder.GetParent().FolderName);
                currentFolder = currentFolder.GetParent();
            }

            return path;
        }
    }

    /// <summary>
    /// 
    /// </summary>
	public class ProjectFile
	{
		private string fileName;
		private BuildAction onBuild;
		private bool open;
        private ProjectFolder parent;

        /// <summary>
        /// 
        /// </summary>
        public ProjectFile()
        {
            this.fileName = "";
            this.onBuild = BuildAction.Package;
            this.open = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public ProjectFile(string fileName)
        {
            this.fileName = fileName;
            this.onBuild = BuildAction.Package;
            this.open = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="onBuild"></param>
        public ProjectFile(string fileName, BuildAction onBuild)
        {
            this.fileName = fileName;
            this.onBuild = onBuild;
            this.open = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="onBuild"></param>
        /// <param name="isOpen"></param>
        public ProjectFile(string fileName, BuildAction onBuild, bool isOpen)
        {
            this.fileName = fileName;
            this.onBuild = onBuild;
            this.open = isOpen;
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("path")]
        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("on-build", typeof(BuildAction))]
        public BuildAction OnBuild
        {
            get
            {
                return onBuild;
            }
            set
            {
                onBuild = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("open", typeof(bool))]
        public bool IsOpen
        {
            get
            {
                return open;
            }
            set
            {
                open = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ProjectFolder GetParent()
        {
            return parent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pf"></param>
        public void SetParent(ProjectFolder pf)
        {
            this.parent = pf;
        }
    }

	public enum BuildAction
	{
		Ignore,
		Package
	}
}
