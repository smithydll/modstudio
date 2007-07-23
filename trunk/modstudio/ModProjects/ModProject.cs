using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Specialized;

namespace ModProjects
{
	/// <summary>
	/// Summary description for ModProject.
	/// </summary>
	[XmlRoot("mod-project")]
	public class ModProject
	{
		private string buildPath;
		private ArrayList files;

		public ModProject()
		{
			//
			// TODO: Add constructor logic here
			//
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

		[XmlArray("file")]
		public ArrayList Files
		{
			get
			{
				return files;
			}
			set
			{
				files = value;
			}
		}
	}

	public class ProjectFile
	{
		private string fileName;
		private BuildAction onBuild;
		private bool open;
	}

	public enum BuildAction
	{
		Ignore,
		Package
	}
}
