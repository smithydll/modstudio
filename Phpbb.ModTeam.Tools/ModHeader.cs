/***************************************************************************
 *                               ModHeader.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: ModHeader.cs,v 1.2 2007-07-23 11:17:29 smithydll Exp $
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
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Xsl;
using System.Data;

namespace Phpbb.ModTeam.Tools.DataStructures
{
	/// <summary>
	/// Respresents a modification header section.
	/// </summary>
	public class ModHeader
	{
		/// <summary>
		/// The title of the MOD, this field is localised
		/// </summary>
		public StringLocalised Title = new StringLocalised();
		/// <summary>
		/// A collection of authors
		/// </summary>
		public ModAuthors Authors;
		/// <summary>
		/// A description of what the MOD does, this field is localised
		/// </summary>
		public StringLocalised Description = new StringLocalised();
		/// <summary>
		/// The version of the MOD
		/// </summary>
		public ModVersion Version;
		/// <summary>
		/// The installation level for the MOD
		/// </summary>
		public ModInstallationLevel InstallationLevel;
		/// <summary>
		/// The time it takes to install the MOD.
		/// </summary>
		public int InstallationTime;
		/// <summary>
		/// The suggested amount of time it takes to install the MOD. (unused, ModInstallationTime is overriden without prompting)
		/// </summary>
		public int SuggestedInstallTime;
		/// <summary>
		/// The files the MOD is editing, updated automatically
		/// </summary>
		public StringCollection FilesToEdit;
		/// <summary>
		/// The files included with the MOD, updated automatically.
		/// </summary>
		public StringCollection IncludedFiles;
		/// <summary>
		/// The generator for the MOD.
		/// </summary>
		public string Generator;
		/// <summary>
		/// Author Notes, this field is localised
		/// </summary>
		public StringLocalised AuthorNotes = new StringLocalised();
		/// <summary>
		/// EasyMOD compatible?
		/// </summary>
		public ModVersion EasyModCompatibility;
		/// <summary>
		/// History collection
		/// </summary>
		public ModHistory History;
		/// <summary>
		/// Version of phpBB designed for
		/// </summary>
		public TargetVersionCases PhpbbVersion;
		/// <summary>
		/// Meta data
		/// </summary>
		public StringDictionary Meta = new StringDictionary();
		/// <summary>
		/// License
		/// </summary>
		public string License;

		/// <summary>
		/// Deep Compare
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(ModHeader)) return false;
			ModHeader mh = (ModHeader)obj;
			if (!Title.Equals(mh.Title)) return false;
			if (!Authors.Equals(mh.Authors)) return false;
			if (!Description.Equals(mh.Description)) return false;
			if (!Version.Equals(mh.Version)) return false;
			if (!InstallationLevel.Equals(mh.InstallationLevel)) return false;
			if (!InstallationTime.Equals(mh.InstallationTime)) return false;
			if (!SuggestedInstallTime.Equals(mh.SuggestedInstallTime)) return false;
			if (FilesToEdit.Count != mh.FilesToEdit.Count) return false;
			for (int i = 0; i < FilesToEdit.Count; i++)
			{
				if (!FilesToEdit[i].Equals(mh.FilesToEdit[i])) return false;
			}
			if (IncludedFiles.Count != mh.IncludedFiles.Count) return false;
			for (int i = 0; i < IncludedFiles.Count; i++)
			{
				if (!IncludedFiles[i].Equals(mh.IncludedFiles[i])) return false;
			}
			// ignore generator
			if (!AuthorNotes.Equals(mh.AuthorNotes)) return false;
			if (!EasyModCompatibility.Equals(mh.EasyModCompatibility)) return false;
			if (!History.Equals(mh.History)) return false;
			if (!PhpbbVersion.Equals(mh.PhpbbVersion)) return false;
			if (Meta.Count != mh.Meta.Count) return false;
			foreach (string key in Meta.Keys)
			{
				if (!mh.Meta.ContainsKey(key)) return false;
				if (!Meta[key].Equals(mh.Meta[key])) return false;
			}
			if (License != null)
			{
				if (!License.Equals(mh.License)) return false;
			}
			else
			{
				if (mh.License != null && mh.License != "") return false;
			}
			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}


	}
}