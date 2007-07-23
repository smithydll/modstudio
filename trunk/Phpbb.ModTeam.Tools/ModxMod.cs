/***************************************************************************
 *                                 ModxMod.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: ModxMod.cs,v 1.2 2007-07-23 11:17:30 smithydll Exp $
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
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using Phpbb.ModTeam.Tools;
using Phpbb.ModTeam.Tools.DataStructures;
using Phpbb.ModTeam.Tools.Xml;

namespace Phpbb.ModTeam.Tools
{
	/// <summary>
	/// Summary description for ModXMod.
	/// </summary>
	public class ModxMod : Mod, IMod
	{

		public const string Default2XsltFile = "modx.subsilver.en.xsl";
        public const string Default3XsltFile = "modx.prosilver.en.xsl";
		private string XmlValidationMessage;
        private string modxVersion = "1.0.1";

		/// <summary>
		/// 
		/// </summary>
		public ModxMod() : base()
		{
		}

        public string GetModxVersion()
        {
            return modxVersion;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		public override void Read(string fileName)
		{
			lastReadFormat = ModFormats.Modx;
			mod xmlDataSet = new mod();
            xmlDataSet.ReadXml(fileName, System.Data.XmlReadMode.IgnoreSchema);
            if (xmlDataSet.title.Rows.Count == 0)
            {
                xmlDataSet.Namespace = "http://www.phpbb.com/mods/xml/modx-1.0.xsd";
                xmlDataSet.ReadXml(fileName, System.Data.XmlReadMode.IgnoreSchema);
                modxVersion = "1.0";
            }

			ReadAuthors(xmlDataSet);
			ReadTitle(xmlDataSet);
			ReadAuthorNotes(xmlDataSet);
			ReadDescription(xmlDataSet);
			ReadHistory(xmlDataSet);
			ReadMeta(xmlDataSet);
			ReadVersion(xmlDataSet);
			ReadLicense(xmlDataSet);
			ReadInstallation(xmlDataSet);

			ReadActions(xmlDataSet);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="modxMod"></param>
		public void ReadString(string modxMod)
		{
			lastReadFormat = ModFormats.Modx;
			mod xmlDataSet = new mod();
			// convert string to stream so it can then be read
			Stream modxStream = new MemoryStream();
			StreamWriter sw = new StreamWriter(modxStream);
			sw.Write(modxMod);
			xmlDataSet.ReadXml(modxStream, System.Data.XmlReadMode.IgnoreSchema);
            if (xmlDataSet.title.Rows.Count == 0)
            {
                xmlDataSet.Namespace = "http://www.phpbb.com/mods/xml/modx-1.0.xsd";
                xmlDataSet.ReadXml(modxStream, System.Data.XmlReadMode.IgnoreSchema);
                modxVersion = "1.0";
            }

			ReadAuthors(xmlDataSet);
			ReadTitle(xmlDataSet);
			ReadAuthorNotes(xmlDataSet);
			ReadDescription(xmlDataSet);
			ReadHistory(xmlDataSet);
			ReadMeta(xmlDataSet);
			ReadVersion(xmlDataSet);
			ReadLicense(xmlDataSet);
			ReadInstallation(xmlDataSet);

			ReadActions(xmlDataSet);
		}

		#region Read Xml Stuff
		/// <summary>
		/// 
		/// </summary>
		/// <param name="XmlDataSet"></param>
		private void ReadAuthors(mod XmlDataSet)
		{
			for (int i = 0; i < XmlDataSet._author_group.Rows.Count; i++)
			{
				for (int j = 0; j < XmlDataSet.author.Rows.Count; j++)
				{
					if ((int)XmlDataSet.author.Rows[j]["author-group_Id"] == (int)XmlDataSet._author_group.Rows[i]["author-group_Id"])
					{
						ModAuthor tempAuthor = new ModAuthor(XmlDataSet.author.Rows[j]["username"].ToString(), 
							XmlDataSet.author.Rows[j]["realname"].ToString(),
							XmlDataSet.author.Rows[j]["email"].ToString(),
							XmlDataSet.author.Rows[j]["homepage"].ToString());
						// TODO: contributions
						Header.Authors.Add(tempAuthor);
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="XmlDataSet"></param>
		private void ReadTitle(mod XmlDataSet)
		{
			Header.Title = new StringLocalised();
			for (int i = 0; i < XmlDataSet.title.Rows.Count; i++)
			{
				Header.Title[XmlDataSet.title.Rows[i]["lang"].ToString()] = XmlDataSet.title.Rows[i]["title_Text"].ToString();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="XmlDataSet"></param>
		private void ReadDescription(mod XmlDataSet)
		{
			Header.Description = new StringLocalised();
			for (int i = 0; i < XmlDataSet.description.Rows.Count; i++)
			{
				Header.Description.Add((string)XmlDataSet.description.Rows[i]["description_Text"], (string)XmlDataSet.description.Rows[i]["lang"]);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="XmlDataSet"></param>
		private void ReadAuthorNotes(mod XmlDataSet)
		{
			Header.AuthorNotes = new StringLocalised();
			for (int i = 0; i < XmlDataSet._author_notes.Rows.Count; i++)
			{
				Header.AuthorNotes[XmlDataSet._author_notes.Rows[i]["lang"].ToString()] = XmlDataSet._author_notes.Rows[i]["author-notes_Text"].ToString();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="XmlDataSet"></param>
		private void ReadHistory(mod XmlDataSet)
		{
			Header.History = new ModHistory();
			for (int i = 0; i < XmlDataSet.entry.Rows.Count; i++)
			{
				Header.History.Add(new ModHistoryEntry());
				for (int j = 0; j < XmlDataSet._rev_version.Rows.Count; j++)
				{
					if ((int)XmlDataSet._rev_version.Rows[j]["entry_Id"] == (int)XmlDataSet.entry.Rows[i]["entry_Id"])
					{
						ModVersion tempVersion;
						tempVersion = new ModVersion(
							(UInt16)XmlDataSet._rev_version.Rows[j]["major"],
							(UInt16)XmlDataSet._rev_version.Rows[j]["minor"],
                            (UInt16)XmlDataSet._rev_version.Rows[j]["revision"]);
                        if (!(XmlDataSet._rev_version.Rows[j]["stage"] is DBNull))
                        {
                            tempVersion.Stage = Mod.StringToVersionStage((string)XmlDataSet._rev_version.Rows[j]["stage"]);
                        }
						if (XmlDataSet._rev_version.Rows[j]["release"].ToString().ToCharArray().Length == 1)
						{
							tempVersion.Release = XmlDataSet._rev_version.Rows[j]["release"].ToString().ToCharArray()[0];
						}
						Header.History[i].Version = tempVersion;
					}
				}
				Header.History[i].Date = DateTime.Parse(XmlDataSet.entry.Rows[i]["date"].ToString());
				Header.History[i].ChangeLog = new ModHistoryChangeLogLocalised();
				for (int j = 0; j < XmlDataSet.changelog.Rows.Count; j++)
				{
					if ((int)XmlDataSet.changelog.Rows[j]["entry_Id"] == (int)XmlDataSet.entry.Rows[i]["entry_Id"])
					{
						string language = (string)XmlDataSet.changelog.Rows[j]["lang"];
						Header.History[i].ChangeLog.Add(new ModHistoryChangeLog(), language);
						for (int k = 0; k < XmlDataSet.change.Rows.Count; k++)
						{
							if ((int)XmlDataSet.change.Rows[k]["changelog_Id"] == (int)XmlDataSet.changelog.Rows[j]["changelog_Id"])
							{
								Header.History[i].ChangeLog[language].Add((string)XmlDataSet.change.Rows[k]["change_Text"]);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="XmlDataSet"></param>
		private void ReadVersion(mod XmlDataSet)
		{
			try
			{
				ModVersion tempVersion = new ModVersion();
				tempVersion.Major = int.Parse(XmlDataSet._mod_version.Rows[0]["major"].ToString());
				tempVersion.Minor = int.Parse(XmlDataSet._mod_version.Rows[0]["minor"].ToString());
				tempVersion.Revision = int.Parse(XmlDataSet._mod_version.Rows[0]["revision"].ToString());
                if (!(XmlDataSet._mod_version.Rows[0]["stage"] is DBNull))
                {
                    tempVersion.Stage = Mod.StringToVersionStage((string)XmlDataSet._mod_version.Rows[0]["stage"]);
                }
                
				try
				{
					if (XmlDataSet._mod_version.Rows[0]["revision"].ToString().ToCharArray().Length == 1)
					{
						tempVersion.Release = XmlDataSet._mod_version.Rows[0]["release"].ToString().ToCharArray()[0];
					}
				}
				catch
				{
				}
				Header.Version = tempVersion;
			}
			catch 
			{
				Header.Version = new ModVersion(0,0,0);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="XmlDataSet"></param>
		private void ReadLicense(mod XmlDataSet)
		{
			try { Header.License = (string)XmlDataSet.header.Rows[0]["license"]; }
			catch {}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="XmlDataSet"></param>
		private void ReadInstallation(mod XmlDataSet)
		{
			try 
			{ 
				Header.InstallationLevel = InstallationLevelParse(XmlDataSet.installation.Rows[0]["level"].ToString());
				Header.InstallationTime = int.Parse(XmlDataSet.installation.Rows[0]["time"].ToString());
				// TODO: easymod-compliant
				//Header.ModEasymodCompatibility = (bool)XmlDataSet.installation.Rows[0]["easymod-compliant"];
				// TODO: mod-config
			}
			catch {}
			Header.PhpbbVersion = new TargetVersionCases();
			try { Header.PhpbbVersion.Primary = (string)XmlDataSet._target_version.Rows[0]["target-primary"]; }
			catch {}

			// major
			for (int i = 0; i < XmlDataSet._target_major.Rows.Count; i++)
			{
				TargetVersionComparisson comparisson = TargetVersionComparisson.EqualTo;
				switch ((string)XmlDataSet._target_major.Rows[i]["allow"])
				{
					case "exact":
						comparisson = TargetVersionComparisson.EqualTo;
						break;
					case "after":
						comparisson = TargetVersionComparisson.GreaterThan;
						break;
					case "after-equal":
						comparisson = TargetVersionComparisson.GreaterThanEqual;
						break;
					case "before":
						comparisson = TargetVersionComparisson.LessThan;
						break;
					case "before-equal":
						comparisson = TargetVersionComparisson.LessThanEqual;
						break;
					case "not-equal":
						comparisson = TargetVersionComparisson.NotEqualTo;
						break;
				}
				Header.PhpbbVersion.Add(new TargetVersionCase(comparisson, TargetVersionPart.Major, (ushort)XmlDataSet._target_major.Rows[i]["target-major_Text"]));
			}

			// minor
			for (int i = 0; i < XmlDataSet._target_minor.Rows.Count; i++)
			{
				TargetVersionComparisson comparisson = TargetVersionComparisson.EqualTo;
				switch ((string)XmlDataSet._target_minor.Rows[i]["allow"])
				{
					case "exact":
						comparisson = TargetVersionComparisson.EqualTo;
						break;
					case "after":
						comparisson = TargetVersionComparisson.GreaterThan;
						break;
					case "after-equal":
						comparisson = TargetVersionComparisson.GreaterThanEqual;
						break;
					case "before":
						comparisson = TargetVersionComparisson.LessThan;
						break;
					case "before-equal":
						comparisson = TargetVersionComparisson.LessThanEqual;
						break;
					case "not-equal":
						comparisson = TargetVersionComparisson.NotEqualTo;
						break;
				}
				Header.PhpbbVersion.Add(new TargetVersionCase(comparisson, TargetVersionPart.Minor, (ushort)XmlDataSet._target_minor.Rows[i]["target-minor_Text"]));
			}

			// revision
			for (int i = 0; i < XmlDataSet._target_revision.Rows.Count; i++)
			{
				TargetVersionComparisson comparisson = TargetVersionComparisson.EqualTo;
				switch ((string)XmlDataSet._target_revision.Rows[i]["allow"])
				{
					case "exact":
						comparisson = TargetVersionComparisson.EqualTo;
						break;
					case "after":
						comparisson = TargetVersionComparisson.GreaterThan;
						break;
					case "after-equal":
						comparisson = TargetVersionComparisson.GreaterThanEqual;
						break;
					case "before":
						comparisson = TargetVersionComparisson.LessThan;
						break;
					case "before-equal":
						comparisson = TargetVersionComparisson.LessThanEqual;
						break;
					case "not-equal":
						comparisson = TargetVersionComparisson.NotEqualTo;
						break;
				}
                if (XmlDataSet._target_revision.Rows[i]["stage"] is DBNull)
                {
                    Header.PhpbbVersion.Add(new TargetVersionCase(comparisson, TargetVersionPart.Revision, (ushort)XmlDataSet._target_revision.Rows[i]["target-revision_Text"]));
                }
                else
                {
                    Header.PhpbbVersion.Add(new TargetVersionCase(comparisson, TargetVersionPart.Revision, Mod.StringToVersionStage((string)XmlDataSet._target_revision.Rows[i]["stage"]), (ushort)XmlDataSet._target_revision.Rows[i]["target-revision_Text"]));
                }
			}

			// release
			for (int i = 0; i < XmlDataSet._target_release.Rows.Count; i++)
			{
				TargetVersionComparisson comparisson = TargetVersionComparisson.EqualTo;
                switch ((string)XmlDataSet._target_release.Rows[i]["allow"])
				{
					case "exact":
						comparisson = TargetVersionComparisson.EqualTo;
						break;
					case "after":
						comparisson = TargetVersionComparisson.GreaterThan;
						break;
					case "after-equal":
						comparisson = TargetVersionComparisson.GreaterThanEqual;
						break;
					case "before":
						comparisson = TargetVersionComparisson.LessThan;
						break;
					case "before-equal":
						comparisson = TargetVersionComparisson.LessThanEqual;
						break;
					case "not-equal":
						comparisson = TargetVersionComparisson.NotEqualTo;
						break;
				}
				string release = (string)XmlDataSet._target_release.Rows[i]["target-release_Text"];
				if (release.Length > 0)
				{
					Header.PhpbbVersion.Add(new TargetVersionCase(comparisson, TargetVersionPart.Release, release[0]));
				}
				else
				{
					Header.PhpbbVersion.Add(new TargetVersionCase(comparisson, TargetVersionPart.Release));
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="XmlDataSet"></param>
		private void ReadMeta(mod XmlDataSet)
		{
			Header.Meta = new StringDictionary();
			for (int i = 0; i < XmlDataSet.meta.Rows.Count; i++)
			{
				Header.Meta.Add((string)XmlDataSet.meta.Rows[i]["name"], (string)XmlDataSet.meta.Rows[i]["content"]);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="XmlDataSet"></param>
		private void ReadActions(mod XmlDataSet)
		{
			ReadSqlActions(XmlDataSet);
			ReadCopyActions(XmlDataSet);
			ReadEditActions(XmlDataSet);
			ReadDiyInstructionsActions(XmlDataSet);

			Actions.Add(new ModAction("SAVE/CLOSE ALL FILES", "", "EoM"));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="XmlDataSet"></param>
		private void ReadSqlActions(mod XmlDataSet)
		{
			for (int i = 0; i < XmlDataSet.sql.Rows.Count; i++)
			{
				Actions.Add(new ModAction("SQL", XmlDataSet.sql.Rows[i]["sql_Text"].ToString(), ""));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="XmlDataSet"></param>
		private void ReadCopyActions(mod XmlDataSet)
		{
			StringBuilder tempCopy = new StringBuilder();
			for (int j = 0; j < XmlDataSet.file.Rows.Count; j++)
			{
				tempCopy.Append("copy ");
				tempCopy.Append(XmlDataSet.file.Rows[j]["from"].ToString());
				tempCopy.Append(" to ");
				tempCopy.Append(XmlDataSet.file.Rows[j]["to"].ToString());
				tempCopy.Append("\n");
			}
			if (tempCopy.Length > 0)
			{
				Actions.Add(new ModAction("COPY", tempCopy.ToString(), ""));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="XmlDataSet"></param>
		private void ReadEditActions(mod XmlDataSet)
		{
			bool firstFindInEdit = true;
			for (int i = 0; i < XmlDataSet.open.Rows.Count; i++)
			{
				Actions.Add(new ModAction("OPEN", XmlDataSet.open.Rows[i]["src"].ToString(), ""));
				for (int j = 0; j < XmlDataSet.edit.Rows.Count; j++)
				{
					if ((int)XmlDataSet.edit.Rows[j]["open_Id"] == (int)XmlDataSet.open.Rows[i]["open_Id"])
					{
						firstFindInEdit = true;
						for (int k = 0; k < XmlDataSet.find.Rows.Count; k++)
						{
							if ((int)XmlDataSet.find.Rows[k]["edit_Id"] == (int)XmlDataSet.edit.Rows[j]["edit_Id"])
							{
								StringLocalised thisComment = new StringLocalised();
								if (firstFindInEdit)
								{
									for(int l = 0; l < XmlDataSet.comment.Rows.Count; l++)
									{
										if ((int)XmlDataSet.comment.Rows[l]["edit_Id"] == (int)XmlDataSet.edit.Rows[j]["edit_Id"])
										{
											try
											{
												thisComment.Add((String)XmlDataSet.comment.Rows[l]["comment_Text"], (String)XmlDataSet.comment.Rows[l]["lang"]);
											}
											catch
											{
											}
										}
									}
								}
								else
								{
									thisComment = new StringLocalised("");
								}
								Actions.Add(new ModAction("FIND", XmlDataSet.find.Rows[k]["find_Text"].ToString(), thisComment, XmlDataSet.find.Rows[k]["type"].ToString()));
								firstFindInEdit = false;
							}
						}
						for (int k = 0; k < XmlDataSet.action.Rows.Count; k++)
						{
							if ((int)XmlDataSet.action.Rows[k]["edit_Id"] == (int)XmlDataSet.edit.Rows[j]["edit_Id"])
							{
								string actionTitle = "";
								switch (XmlDataSet.action.Rows[k]["type"].ToString())
								{
									case "after-add":
										actionTitle = "AFTER, ADD";
										break;
									case "before-add":
										actionTitle = "BEFORE, ADD";
										break;
									case "replace":
									case "replace-with":
										actionTitle = "REPLACE WITH";
										break;
									case "operation":
										actionTitle = "INCREMENT";
										break;
								}
								Actions.Add(new ModAction(actionTitle, XmlDataSet.action.Rows[k]["action_Text"].ToString(), ""));
							}
						}
						for (int k = 0; k < XmlDataSet._inline_edit.Rows.Count; k++)
						{
							if ((int)XmlDataSet._inline_edit.Rows[k]["edit_Id"] == (int)XmlDataSet.edit.Rows[j]["edit_Id"])
							{
								for (int l = 0; l < XmlDataSet._inline_find.Rows.Count; l++)
								{
									if ((int)XmlDataSet._inline_find.Rows[l]["inline-edit_Id"] == (int)XmlDataSet._inline_edit.Rows[k]["inline-edit_Id"])
									{
										Actions.Add(new ModAction("IN-LINE FIND", XmlDataSet._inline_find.Rows[l]["inline-find_Text"].ToString(), "", XmlDataSet._inline_find.Rows[l]["type"].ToString()));
									}
								}
								for (int l = 0; l < XmlDataSet._inline_action.Rows.Count; l++)
								{
									if ((int)XmlDataSet._inline_action.Rows[l]["inline-edit_Id"] == (int)XmlDataSet._inline_edit.Rows[k]["inline-edit_Id"])
									{
										string actionTitle = "";
										switch (XmlDataSet._inline_action.Rows[l]["type"].ToString())
										{
											case "after-add":
												actionTitle = "IN-LINE AFTER, ADD";
												break;
											case "before-add":
												actionTitle = "IN-LINE BEFORE, ADD";
												break;
											case "replace":
											case "replace-with":
												actionTitle = "IN-LINE REPLACE WITH";
												break;
											case "operation":
												actionTitle = "IN-LINE INCREMENT";
												break;
										}
										Actions.Add(new ModAction(actionTitle, XmlDataSet._inline_action.Rows[l]["inline-action_Text"].ToString(), ""));
									}
								}
							}
						}
					}
				}
			}
		}

		private void ReadDiyInstructionsActions(mod XmlDataSet)
		{
			for (int i = 0; i < XmlDataSet._diy_instructions.Rows.Count; i++)
			{
				Actions.Add(new ModAction("DIY INSTRUCTIONS", XmlDataSet._diy_instructions.Rows[i]["diy-instructions_Text"].ToString(),"", XmlDataSet._diy_instructions.Rows[i]["lang"].ToString()));
			}
		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		public override void Write(string fileName)
		{
            if (this.Header.PhpbbVersion.Contains(new TargetVersionCase(TargetVersionComparisson.EqualTo, TargetVersionPart.Major, 3)))
            {
                Write(fileName, Default3XsltFile);
            }
            else
            {
                Write(fileName, Default2XsltFile);
            }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="xsltFile"></param>
		public void Write(string fileName, string xsltFile)
		{
			SaveTextFile(this.ToString(xsltFile), fileName);
		}

		#region Write Xml Stuff
		/// <summary>
		/// 
		/// </summary>
		/// <param name="XmlDataSet"></param>
		private void WriteHeader(mod XmlDataSet)
		{
			mod.headerRow newHeaderRow = XmlDataSet.header.NewheaderRow();
			XmlDataSet.header.Rows.Add(newHeaderRow);

			WriteLicense(newHeaderRow);
			WriteTitle(XmlDataSet, newHeaderRow);
			WriteDescription(XmlDataSet, newHeaderRow);
			WriteAuthorNotes(XmlDataSet, newHeaderRow);
			WriteAuthors(XmlDataSet, newHeaderRow);
			WriteVersion(XmlDataSet, newHeaderRow);
			WriteInstallation(XmlDataSet, newHeaderRow);
			WriteHistory(XmlDataSet, newHeaderRow);
			WriteMeta(XmlDataSet, newHeaderRow);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="newHeaderRow"></param>
		private void WriteLicense(mod.headerRow newHeaderRow)
		{
			newHeaderRow.license = Header.License;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="XmlDataSet"></param>
		/// <param name="newHeaderRow"></param>
		private void WriteTitle(mod XmlDataSet, mod.headerRow newHeaderRow)
		{
			foreach (string language in Header.Title)
			{
				mod.titleRow newRow = XmlDataSet.title.NewtitleRow();
				newRow.title_Text = Header.Title[language];
				newRow.lang = language.ToLower();
				newRow.SetParentRow(newHeaderRow);
				XmlDataSet.title.Rows.Add(newRow);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="XmlDataSet"></param>
		/// <param name="newHeaderRow"></param>
		private void WriteDescription(mod XmlDataSet, mod.headerRow newHeaderRow)
		{
			foreach (string language in Header.Description)
			{
				mod.descriptionRow newRow = XmlDataSet.description.NewdescriptionRow();
				newRow.description_Text = Header.Description[language];
				newRow.lang = language;
				newRow.SetParentRow(newHeaderRow);
				XmlDataSet.description.Rows.Add(newRow);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="XmlDataSet"></param>
		/// <param name="newHeaderRow"></param>
		private void WriteAuthorNotes(mod XmlDataSet, mod.headerRow newHeaderRow)
		{
			foreach (string language in Header.AuthorNotes)
			{
				mod._author_notesRow newRow = XmlDataSet._author_notes.New_author_notesRow();
				newRow._author_notes_Text = Header.AuthorNotes[language];
				newRow.lang = language;
				newRow.SetParentRow(newHeaderRow);
				XmlDataSet._author_notes.Rows.Add(newRow);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="XmlDataSet"></param>
		/// <param name="newHeaderRow"></param>
		private void WriteAuthors(mod XmlDataSet, mod.headerRow newHeaderRow)
		{
			mod._author_groupRow authorGroupRow = XmlDataSet._author_group.New_author_groupRow();
			authorGroupRow.SetParentRow(newHeaderRow);
			XmlDataSet._author_group.Rows.Add(authorGroupRow);
			foreach (ModAuthor entry in Header.Authors)
			{
				mod.authorRow newRow = XmlDataSet.author.NewauthorRow();
				if (entry.Email.ToLower() != "") newRow.email = entry.Email;
				if (entry.Homepage.ToLower() != "") newRow.homepage = entry.Homepage;
				if (entry.RealName.ToLower() != "") newRow.realname = entry.RealName;
				if (entry.UserName.ToLower() != "") newRow.username = entry.UserName;
				newRow.SetParentRow(authorGroupRow);
				XmlDataSet.author.Rows.Add(newRow);
				mod.contributionsRow newContributionsRow = XmlDataSet.contributions.NewcontributionsRow();
				if (entry.AuthorFrom > 0)
				{
					newContributionsRow.from = new DateTime(entry.AuthorFrom, 1, 1);
				}
				if (entry.AuthorTo > 0)
				{
					newContributionsRow.to = new DateTime(entry.AuthorTo, 1 , 1);
				}
				if (entry.Status != ModAuthorStatus.NoneSelected)
				{
					newContributionsRow.status = entry.Status.ToString();
				}
				newContributionsRow.SetParentRow(newRow);
				XmlDataSet.contributions.Rows.Add(newContributionsRow);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="XmlDataSet"></param>
		/// <param name="newHeaderRow"></param>
		private void WriteVersion(mod XmlDataSet, mod.headerRow newHeaderRow)
		{
			mod._mod_versionRow versionRow = XmlDataSet._mod_version.New_mod_versionRow();
			versionRow.SetParentRow(newHeaderRow);
			versionRow.major = (ushort)Header.Version.Major;
			versionRow.minor = (ushort)Header.Version.Minor;
            //--------------------------------------------------------------------
			versionRow.revision = (ushort)Header.Version.Revision;
            if (Header.Version.Stage != VersionStage.Stable)
            {
                versionRow.stage = Mod.VersionStageToString(Header.Version.Stage);
            }
            //--------------------------------------------------------------------
			if (Header.Version.Release != ModVersion.nullChar)
			{
				versionRow.release = Header.Version.Release.ToString();
			}
			XmlDataSet._mod_version.Rows.Add(versionRow);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="XmlDataSet"></param>
		/// <param name="newHeaderRow"></param>
		private void WriteInstallation(mod XmlDataSet, mod.headerRow newHeaderRow)
		{
			mod.installationRow installationRow = XmlDataSet.installation.NewinstallationRow();
			installationRow.SetParentRow(newHeaderRow);
			installationRow.level = Header.InstallationLevel.ToString().ToLower();
			installationRow.time = (ulong)Header.InstallationTime;
			// TODO: target version, right now target version isn't supported by Phpbb.ModTeam.Tools
			// full support to come, release is currently omitted, need full support!!!
			// For the moment we will say everything is phpBB2.0 compliant
			if (Header.PhpbbVersion == null) Header.PhpbbVersion = new TargetVersionCases(new ModVersion(2, 0, 0));
			if (Header.PhpbbVersion != null)
			{
				// handle for case where Major version is non-sensical
				//if (Header.PhpbbVersion.Major == 0) Header.PhpbbVersion = new TargetVersionCases(new ModVersion(2, 0, 0));

				mod._target_versionRow targetVersionRow = XmlDataSet._target_version.New_target_versionRow();
				targetVersionRow._target_primary = Header.PhpbbVersion.Primary;
				foreach (TargetVersionCase versionCase in Header.PhpbbVersion)
				{
					switch (versionCase.Part)
					{
						case TargetVersionPart.Major:
							mod._target_majorRow targetMajorRow = XmlDataSet._target_major.New_target_majorRow();
							switch (versionCase.Comparisson)
							{
								case TargetVersionComparisson.EqualTo:
									targetMajorRow.allow = "exact";
									break;
								case TargetVersionComparisson.GreaterThan:
									targetMajorRow.allow = "after";
									break;
								case TargetVersionComparisson.GreaterThanEqual:
									targetMajorRow.allow = "after-equal";
									break;
								case TargetVersionComparisson.LessThan:
									targetMajorRow.allow = "before";
									break;
								case TargetVersionComparisson.LessThanEqual:
									targetMajorRow.allow = "before-equal";
									break;
								case TargetVersionComparisson.NotEqualTo:
									targetMajorRow.allow = "not-equal";
									break;
							}
							targetMajorRow._target_major_Text = ushort.Parse(versionCase.GetValue);
							targetMajorRow.SetParentRow(targetVersionRow);
							XmlDataSet._target_major.Rows.Add(targetMajorRow);
							break;
						case TargetVersionPart.Minor:
							mod._target_minorRow targetMinorRow = XmlDataSet._target_minor.New_target_minorRow();
							switch (versionCase.Comparisson)
							{
								case TargetVersionComparisson.EqualTo:
									targetMinorRow.allow = "exact";
									break;
								case TargetVersionComparisson.GreaterThan:
									targetMinorRow.allow = "after";
									break;
								case TargetVersionComparisson.GreaterThanEqual:
									targetMinorRow.allow = "after-equal";
									break;
								case TargetVersionComparisson.LessThan:
									targetMinorRow.allow = "before";
									break;
								case TargetVersionComparisson.LessThanEqual:
									targetMinorRow.allow = "before-equal";
									break;
								case TargetVersionComparisson.NotEqualTo:
									targetMinorRow.allow = "not-equal";
									break;
							}
							targetMinorRow._target_minor_Text = ushort.Parse(versionCase.GetValue);
							targetMinorRow.SetParentRow(targetVersionRow);
							XmlDataSet._target_minor.Rows.Add(targetMinorRow);
							break;
						case TargetVersionPart.Revision:
							mod._target_revisionRow targetRevisionRow = XmlDataSet._target_revision.New_target_revisionRow();
							switch (versionCase.Comparisson)
							{
								case TargetVersionComparisson.EqualTo:
									targetRevisionRow.allow = "exact";
									break;
								case TargetVersionComparisson.GreaterThan:
									targetRevisionRow.allow = "after";
									break;
								case TargetVersionComparisson.GreaterThanEqual:
									targetRevisionRow.allow = "after-equal";
									break;
								case TargetVersionComparisson.LessThan:
									targetRevisionRow.allow = "before";
									break;
								case TargetVersionComparisson.LessThanEqual:
									targetRevisionRow.allow = "before-equal";
									break;
								case TargetVersionComparisson.NotEqualTo:
									targetRevisionRow.allow = "not-equal";
									break;
							}
                            switch (versionCase.Stage)
                            {
                                case VersionStage.Alpha:
                                    targetRevisionRow.stage = "alpha";
                                    break;
                                case VersionStage.Beta:
                                    targetRevisionRow.stage = "beta";
                                    break;
                                case VersionStage.Delta:
                                    targetRevisionRow.stage = "delta";
                                    break;
                                case VersionStage.Gamma:
                                    targetRevisionRow.stage = "gamma";
                                    break;
                                case VersionStage.ReleaseCandidate:
                                    targetRevisionRow.stage = "release-candidate";
                                    break;
                            }
							targetRevisionRow._target_revision_Text = ushort.Parse(versionCase.GetValue);
							targetRevisionRow.SetParentRow(targetVersionRow);
							XmlDataSet._target_revision.Rows.Add(targetRevisionRow);
							break;
						case TargetVersionPart.Release:
							mod._target_releaseRow targetReleaseRow = XmlDataSet._target_release.New_target_releaseRow();
							switch (versionCase.Comparisson)
							{
								case TargetVersionComparisson.EqualTo:
									targetReleaseRow.allow = "exact";
									break;
								case TargetVersionComparisson.GreaterThan:
									targetReleaseRow.allow = "after";
									break;
								case TargetVersionComparisson.GreaterThanEqual:
									targetReleaseRow.allow = "after-equal";
									break;
								case TargetVersionComparisson.LessThan:
									targetReleaseRow.allow = "before";
									break;
								case TargetVersionComparisson.LessThanEqual:
									targetReleaseRow.allow = "before-equal";
									break;
								case TargetVersionComparisson.NotEqualTo:
									targetReleaseRow.allow = "not-equal";
									break;
							}
							if (versionCase.GetValue.Length > 0)
							{
								targetReleaseRow._target_release_Text = versionCase.GetValue[0].ToString();
							}
							else
							{
								targetReleaseRow._target_release_Text = "";
							}
							targetReleaseRow.SetParentRow(targetVersionRow);
							XmlDataSet._target_release.Rows.Add(targetReleaseRow);
							break;
					}
				}
				targetVersionRow.SetParentRow(installationRow);
				XmlDataSet._target_version.Rows.Add(targetVersionRow);
			}
			XmlDataSet.installation.Rows.Add(installationRow);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="XmlDataSet"></param>
		/// <param name="newHeaderRow"></param>
		private void WriteHistory(mod XmlDataSet, mod.headerRow newHeaderRow)
		{
			mod.historyRow historyRow = XmlDataSet.history.NewhistoryRow();
			historyRow.SetParentRow(newHeaderRow);
			if (Header.History.Count > 0)
			{
				XmlDataSet.history.Rows.Add(historyRow);
			}
			foreach (ModHistoryEntry mhe in Header.History)
			{
				mod.entryRow entryRow = XmlDataSet.entry.NewentryRow();
				entryRow.date = mhe.Date.Date;
				// Version
				mod._rev_versionRow rev_versionRow = XmlDataSet._rev_version.New_rev_versionRow();
				rev_versionRow.major = (ushort)mhe.Version.Major;
				rev_versionRow.minor = (ushort)mhe.Version.Minor;
                // -----------------------------------------------------------------
                rev_versionRow.revision = (ushort)mhe.Version.Revision;
                if (mhe.Version.Stage != VersionStage.Stable)
                {
                    rev_versionRow.stage = Mod.VersionStageToString(mhe.Version.Stage);
                }
                // -----------------------------------------------------------------
				if (mhe.Version.Release != ModVersion.nullChar)
				{
					rev_versionRow.release = mhe.Version.Release.ToString();
				}
				rev_versionRow.SetParentRow(entryRow);
				XmlDataSet._rev_version.Rows.Add(rev_versionRow);
				// Changelogs
				foreach (DictionaryEntry de in mhe.ChangeLog)
				{
					string Language = (string)de.Key;
					ModHistoryChangeLog mhcl = (ModHistoryChangeLog)de.Value;
					mod.changelogRow changelogRow = XmlDataSet.changelog.NewchangelogRow();
					changelogRow.lang = Language;
					// change
					foreach (string value in mhcl)
					{
						mod.changeRow changeRow = XmlDataSet.change.NewchangeRow();
						changeRow.change_Text = value;
						changeRow.SetParentRow(changelogRow);
						XmlDataSet.change.Rows.Add(changeRow);
					}
					changelogRow.SetParentRow(entryRow);
					XmlDataSet.changelog.Rows.Add(changelogRow);
				}
				entryRow.SetParentRow(historyRow);
				XmlDataSet.entry.Rows.Add(entryRow);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="XmlDataSet"></param>
		/// <param name="newHeaderRow"></param>
		private void WriteMeta(mod XmlDataSet, mod.headerRow newHeaderRow)
		{
			// let's set the generator first
			if (!Header.Meta.ContainsKey("generator"))
			{
				Header.Meta.Add("generator", "Phpbb.ModTeam.Tools (c#)");
			}
			else
			{
				Header.Meta["generator"] = "Phpbb.ModTeam.Tools (c#)";
			}
			foreach (DictionaryEntry de in Header.Meta)
			{
				string key = (string)de.Key;
				string value = (string)de.Value;
				mod.metaRow newRow = XmlDataSet.meta.NewmetaRow();
				newRow.name = key;
				newRow.content = value;
				newRow.SetParentRow(newHeaderRow);
				XmlDataSet.meta.Rows.Add(newRow);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="XmlDataSet"></param>
		private void WriteActions(mod XmlDataSet)
		{
			mod._action_groupRow actiongroupRow = XmlDataSet._action_group.New_action_groupRow();
			mod.copyRow copyRow = XmlDataSet.copy.NewcopyRow();
			mod.openRow currentOpenRow = XmlDataSet.open.NewopenRow();
			mod.editRow currentEditRow = XmlDataSet.edit.NeweditRow();
			mod._inline_editRow currentInLineEditRow = XmlDataSet._inline_edit.New_inline_editRow();
			string type = "";
			string lastActionType = "EDIT";
			string lastInLineActionType = "EDIT";
			foreach (ModAction ma in Actions)
			{
				switch (ma.Type.ToUpper())
				{
					case "SQL":
						mod.sqlRow sqlRow = XmlDataSet.sql.NewsqlRow();
						sqlRow.sql_Text = ma.Body;
						sqlRow.SetParentRow(actiongroupRow);
						XmlDataSet.sql.Rows.Add(sqlRow);
						break;
					case "COPY":
						if (XmlDataSet.copy.Rows.Count == 0)
						{
							copyRow = XmlDataSet.copy.NewcopyRow();
							copyRow.SetParentRow(actiongroupRow);
							XmlDataSet.copy.Rows.Add(copyRow);
						}
						string[] lines = ma.Body.Split('\n');
						foreach (string line in lines)
						{
							string from = "";
							string to = "";
							if (line.TrimStart(trimChars).ToLower().StartsWith("copy"))
							{
								try
								{
									from = Regex.Replace(Regex.Replace(Regex.Match(line.Trim(trimChars), @"^copy (.+) to(\s)", RegexOptions.IgnoreCase).Value, "copy ", "", RegexOptions.IgnoreCase), " to", "", RegexOptions.IgnoreCase).Trim(trimChars);
									to = Regex.Replace(Regex.Replace(Regex.Match(line.Trim(trimChars), " to (.+)$", RegexOptions.IgnoreCase).Value, "copy ", "", RegexOptions.IgnoreCase), @" to(\s)", "", RegexOptions.IgnoreCase).Trim(trimChars);

									mod.fileRow fileRow = XmlDataSet.file.NewfileRow();
									fileRow.from = from;
									fileRow.to = to;
									fileRow.SetParentRow(copyRow);
									XmlDataSet.file.Rows.Add(fileRow);
								} catch { }
							}
						}
						break;
					case "OPEN":
						currentOpenRow = XmlDataSet.open.NewopenRow();
						currentOpenRow.src = ma.Body.Trim(trimChars);
						currentOpenRow.SetParentRow(actiongroupRow);
						XmlDataSet.open.Rows.Add(currentOpenRow);
						break;
					case "FIND":
						if (lastActionType == "EDIT" || lastActionType == "INLINE")
						{
							currentEditRow = XmlDataSet.edit.NeweditRow();
							currentEditRow.SetParentRow(currentOpenRow);
							XmlDataSet.edit.Rows.Add(currentEditRow);
						}
						mod.findRow findRow = XmlDataSet.find.NewfindRow();
						findRow.find_Text = ma.Body;
						if (ma.Modifier != "")
						{
							findRow.type = ma.Modifier;
						}
						// TODO: needs work (merging)
						if (ma.AfterComment != null)
						{
							foreach (string Language in ma.AfterComment)
							{
								if (ma.AfterComment[Language] != "" && ma.AfterComment[Language] != null)
								{
									mod.commentRow commentRow = XmlDataSet.comment.NewcommentRow();
									commentRow.lang = Language;
									commentRow.comment_Text = ma.AfterComment[Language];
									commentRow.SetParentRow(currentEditRow);
									XmlDataSet.comment.Rows.Add(commentRow);
								}
							}
						}
						findRow.SetParentRow(currentEditRow);
						XmlDataSet.find.Rows.Add(findRow);
						lastActionType = "FIND";
						lastInLineActionType = "EDIT";
						break;
					case "AFTER, ADD":
					case "BEFORE, ADD":
					case "REPLACE WITH":
					case "INCREMENT":
						type = "";
					switch (ma.Type.ToUpper())
					{
						case "AFTER, ADD":
							type = "after-add";
							break;
						case "BEFORE, ADD":
							type = "before-add";
							break;
						case "REPLACE WITH":
							type = "replace-with";
							break;
						case "INCREMENT":
							type = "operation";
							break;
					}
						mod.actionRow actionRow = XmlDataSet.action.NewactionRow();
						actionRow.action_Text = ma.Body;
						actionRow.type = type;
						// TODO: comment
						actionRow.SetParentRow(currentEditRow);
						XmlDataSet.action.Rows.Add(actionRow);
						lastActionType = "EDIT";
						break;
					case "IN-LINE FIND":
						// add the inline-edit row to the inline-edit table
						if (lastInLineActionType == "EDIT")
						{
							currentInLineEditRow = XmlDataSet._inline_edit.New_inline_editRow();
							currentInLineEditRow.SetParentRow(currentEditRow);
							XmlDataSet._inline_edit.Rows.Add(currentInLineEditRow);
						}
						mod._inline_findRow inLineFindRow = XmlDataSet._inline_find.New_inline_findRow();
						inLineFindRow._inline_find_Text = ma.Body;
						if (ma.Modifier != "")
						{
							inLineFindRow.type = ma.Modifier;
						}
						inLineFindRow.SetParentRow(currentInLineEditRow);
						XmlDataSet._inline_find.Rows.Add(inLineFindRow);
						lastInLineActionType = "FIND";
						lastActionType = "INLINE";
						break;
					case "IN-LINE AFTER, ADD":
					case "IN-LINE BEFORE, ADD":
					case "IN-LINE REPLACE WITH":
					case "IN-LINE INCREMENT":
						type = "";
					switch (ma.Type.ToUpper())
					{
						case "IN-LINE AFTER, ADD":
							type = "after-add";
							break;
						case "IN-LINE BEFORE, ADD":
							type = "before-add";
							break;
						case "IN-LINE REPLACE WITH":
							type = "replace-with";
							break;
						case "IN-LINE INCREMENT":
							type = "operation";
							break;
					}
						mod._inline_actionRow inLineActionRow = XmlDataSet._inline_action.New_inline_actionRow();
						inLineActionRow._inline_action_Text = ma.Body;
						inLineActionRow.type = type;
						// TODO: comment
						inLineActionRow.SetParentRow(currentInLineEditRow);
						XmlDataSet._inline_action.Rows.Add(inLineActionRow);
						lastInLineActionType = "EDIT";
						break;
					case "DIY INSTRUCTIONS":
						mod._diy_instructionsRow diyRow = XmlDataSet._diy_instructions.New_diy_instructionsRow();
						diyRow._diy_instructions_Text = ma.Body;
						diyRow.lang = ma.Modifier;
						diyRow.SetParentRow(actiongroupRow);
						XmlDataSet._diy_instructions.Rows.Add(diyRow);
						break;
					case "SAVE/CLOSE ALL FILES":
						// lets ignore ;)
						break;
					default:
						Console.WriteLine("Problem with action: " + ma.Type);
						break;
				}
			}
			XmlDataSet._action_group.Rows.Add(actiongroupRow);
		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
            if (this.Header.PhpbbVersion.Contains(new TargetVersionCase(TargetVersionComparisson.EqualTo, TargetVersionPart.Major, 3)))
            {
                return ToString(Default3XsltFile);
            }
            else
            {
                return ToString(Default2XsltFile);
            }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="xsltFile"></param>
		/// <returns></returns>
		public string ToString(string xsltFile)
		{
			// lets force these events to sync the MOD,
			UpdateFilesToEdit();
			UpdateIncludedFiles();
			UpdateInstallationTime();

			mod xmlDataSet = new mod();

			WriteHeader(xmlDataSet);
			WriteActions(xmlDataSet);

			StringBuilder sb = new StringBuilder();
			StringWriter sw = new StringWriter(sb);
			XmlTextWriter xw = new ModxWriter(sw, xsltFile);
			xw.Formatting = Formatting.Indented;
			xmlDataSet.WriteXml(xw);

			return sb.ToString();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public override Phpbb.ModTeam.Tools.Validation.Report Validate(string fileName)
		{
			return Validate(fileName, "english");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		public Validation.Report Validate(string fileName, string language)
		{
			return Validate(fileName, language, new ModVersion(2, 0, 0), true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="language"></param>
		/// <param name="version">Can alter the checks performed based on the version</param>
		/// <param name="checks">Can disable addtional checks so they aren't duplicated in m|EAL</param>
		/// <returns></returns>
		public Validation.Report Validate(string fileName, string language, ModVersion version, bool checks)
		{
			Validation.Report report = new Validation.Report();
			ModxMod modification = new ModxMod();
			bool validateFlag = true;
			bool validateWarnFlag = true;
            string schema = "1.0.1";

			try
			{
				modification.Read(fileName);
                version = ModVersion.Parse(modification.Header.PhpbbVersion.Primary);
			}
			catch (Exception ex)
			{
				report.HeaderReport += "[color=red][b]Error reading MODX file[/b][/color]\n";
                report.HeaderReport += "[quote]\n";
                report.HeaderReport += ex.ToString() + "\n";
                report.HeaderReport += "[/quote]\n";
				return report;
			}

            if (modification != null)
            {
                schema = modification.GetModxVersion();
            }

			// validate XML against schema

			XmlValidationMessage = "";
			XmlTextReader xtr = new XmlTextReader(fileName);
            XmlReaderSettings xrs = new XmlReaderSettings();
            xrs.ConformanceLevel = ConformanceLevel.Document;
            xrs.ValidationEventHandler += new ValidationEventHandler(xvr_ValidationEventHandler);
            xrs.ValidationType = ValidationType.Schema;

            XmlSchema modx1_0Schema = new XmlSchema();
            modx1_0Schema.SourceUri = "http://www.phpbb.com/mods/xml/modx-1.0.xsd";

            XmlSchema modx1_0_1Schema = new XmlSchema();
            modx1_0Schema.SourceUri = "http://www.phpbb.com/mods/xml/modx-1.0.xsd";

            if (schema == "1.0")
            {
                xrs.Schemas.Add(modx1_0Schema);
            }
            else if (schema == "1.0.1")
            {
                xrs.Schemas.Add(modx1_0_1Schema);
            }

            XmlReader xr = XmlReader.Create(fileName, xrs);

            try
            {
                while (xr.Read())
                {
                }
            }
            catch
            {
                try
                {
                    if (schema == "1.0.1")
                    {
                        xrs.Schemas.Add(modx1_0Schema);
                    }
                    else if (schema == "1.0")
                    {
                        xrs.Schemas.Add(modx1_0_1Schema);
                    }
                }
                catch
                {
                    report.HeaderReport += "[color=red][b]Error reading MODX file[/b][/color]\n";
                }
            }

			if (XmlValidationMessage == "")
			{
				report.HeaderReport += string.Format("{0} XML Validation against the MODX XML Schema found no errors\n",
					Validator.ok);
			}
			else
			{
				report.HeaderReport += "[quote=\"Xml Validation Report output\"]";
				report.HeaderReport += string.Format("{0}/n",
					Validator.info);
				report.HeaderReport += XmlValidationMessage;
				report.HeaderReport += "[/quote]\n";
			}

			// validate for phpbb.com requirements
			report.HeaderReport += string.Format("{0} Please be aware that XML entities have been represented in text format for readability and do not reflect the structure of the MOD in the following report where appropriate.\n\n",
				Validator.info);

			foreach (ModAuthor ma in modification.Header.Authors)
			{
				if (ma.UserName == "" || ma.UserName == null)
				{
					report.HeaderReport += string.Format("{1} phpBB requires MODX files contain at least a valid phpBB.com usernames for authors.[quote]{0}[/quote]\n",
						ma.ToString(), Validator.error);
					validateFlag = false;
				}
				if (ma.Email.ToUpper() == "N/A")
				{
					report.HeaderReport += string.Format("{2} e-mail should be omitted or empty, not [i]{0}[/i].[quote]{1}[/quote]\n",
						ma.Email, ma.ToString(), Validator.warning);
					validateWarnFlag = false;
				}
				if (ma.RealName.ToUpper() == "N/A")
				{
					report.HeaderReport += string.Format("{2} real name should be omitted or empty, not [i]{0}[/i].[quote]{1}[/quote]\n",
						ma.Email, ma.ToString(), Validator.warning);
					validateWarnFlag = false;
				}
				if (ma.Homepage.ToUpper() == "N/A")
				{
					report.HeaderReport += string.Format("{2} homepage should be omitted or empty, not [i]{0}[/i].[quote]{1}[/quote]\n",
						ma.Email, ma.ToString(), Validator.warning);
					validateWarnFlag = false;
				}
			}

			if (modification.Header.License == "http://opensource.org/licenses/gpl-license.php GNU General Public License v2")
			{
				report.HeaderReport += string.Format("{0} [i]You are using the GPL License[/i].\n",
					Validator.pass);
			}
			else if (modification.Header.License == "" || modification.Header.License == null)
			{
				report.HeaderReport += string.Format("{0} [i]You have omitted the license field[/i]. Please be aware that most MODs are automatically licensed under the GPL and you may be required to relicense your MOD in accordance with the terms of the GPL inherited from the core phpBB package.\n",
					Validator.warning);
			}
			else
			{
				report.HeaderReport += string.Format("[i]You are not using the GPL License[/i]. Please be aware that most MODs are automatically licensed under the GPL and you may be required to relicense your MOD in accordance with the terms of the GPL inherited from the core phpBB package.\n",
					Validator.info);
			}

			foreach (ModAction ma in modification.Actions)
			{
				if (ma.Type == "OPEN")
				{
					Validator.LoadPhpbbFileList(language, version); // Load the phpBB file list for comparison in the OPEN check
					if (!Validator.PhpbbFileList.Contains(ma.Body))
					{
						report.ActionsReport += string.Format("{2} File to OPEN does not exist in phpBB standard installation package\n[quote]{1}[/quote]\n",
							0, ma.ToString(), Validator.warning);
						validateFlag = false;
					}
				}

				if(ma.Type == "COPY")
				{
					// TODO: fill this in
				}

				if (Validation.ModActions.GetType(ma.Type) == Validation.ModActions.ModActionType.Edit ||
					Validation.ModActions.GetType(ma.Type) == Validation.ModActions.ModActionType.InLineEdit)
				{
					if (checks)
					{
						if (ma.Body.IndexOf("<font") >= 0)
						{
							if (Regex.IsMatch(ma.Body, "<font(.*?)>")) 
							{
								report.HtmlReport += string.Format("{2} Unauthorised usage of the FONT tag. Please use the span tag, starting line: {0}\n[quote]{1}[/quote]\n",
									0, Regex.Replace(ma.ToString(), "<font(.*?)>", "[b]<font$1>[/b]"), Validator.fail);
								validateFlag = false;
							}
						}
						//if (Regex.IsMatch(Modification.Actions[i].ActionBody, "<br>")) 
						if (ma.Body.IndexOf("<br>") >= 0)
						{
							report.HtmlReport += string.Format("{2} Unauthorised usage of the <br> tag. Please use the <br /> tag.\n[quote]{1}[/quote]\n",
								0, Regex.Replace(ma.ToString(), "<br>", "[b]<br>[/b]"), Validator.fail);
							validateFlag = false;
						}
						if (ma.Body.IndexOf("<img") >= 0 &&
							ma.Body.IndexOf("/>") < 0)
						{
							report.HtmlReport += string.Format("Unauthorised usage of the <img> tag. Please make sure you use XHTML entities e.g. <img />.\n[quote]{1}[/quote]\n",
								0, Regex.Replace(ma.ToString(), "<img", "[b]<img[/b]"));
							validateFlag = false;
						}
						if (ma.Body.IndexOf("mysql_") >= 0)
						{
							if (ma.Body.IndexOf("mysql_connect") >= 0)
							{
								if (Regex.IsMatch(ma.Body, "mysql_connect\\((.*?)\\)")) 
								{
									report.DbalReport += string.Format("Unauthorised usage of mysql_connect, please use the DBAL\n[quote]{1}[/quote]\n",
										0, Regex.Replace(ma.ToString(), "mysql_connect\\((.*?)\\)", "[b]mysql_connect($1)[/b]"));
									validateFlag = false;
								}
							}
							if (ma.Body.IndexOf("mysql_error") >= 0)
							{
								if (Regex.IsMatch(ma.Body, "mysql_error\\((.*?)\\)")) 
								{
									report.DbalReport += string.Format("Unauthorised usage of mysql_error, please use the DBAL\n[quote]{1}[/quote]\n",
										0, Regex.Replace(ma.ToString(), "mysql_error\\((.*?)\\)", "[b]mysql_error($1)[/b]"));
									validateFlag = false;
								}
							}
						}
					} // end check blocking
				}
			}

			if (!validateFlag) 
			{
				report.Rating = "The MOD [b][color=red]failed[/color][/b] the MOD pre-validation process. Please review and fix your errors before submitting to the MOD DB.";
				report.Passed = false;
			} 
			else 
			{
				report.Rating = "The MOD [b][color=green]passed[/color][/b] the MOD pre-validation process, please check over for elements computers cannot detect.";
				report.Passed = true;
			}
			if (!validateWarnFlag)
			{
				report.Rating += "\nThere were some [b][color=orange]warnings[/color][/b] which should be looked at but aren't causes for denial. These warnings may cause your MOD to act in undetermined ways in tools other than EasyMod, and should be fixed for maximum compatibility.";
				report.Passed = false;
			}
			if (validateFlag && validateWarnFlag && checks)
			{
				report.ActionsReport += "\n[color=green]No problems[/color] were detected in this MODs Template in accordance with the phpBB MOD Team guidelines.";
			}
			if (report.HtmlReport == null && checks)
			{
				report.HtmlReport = "[color=green]No problems[/color] were detected in this MODs use HTML elements in accordance with the phpBB2 coding standards.";
			}
			if (report.DbalReport == null && checks)
			{
				report.DbalReport = "[color=green]No problems[/color] were detected in this MODs use of databases [size=9](if used)[/size] in accordance with the phpBB2 coding standards.";
			}

			return report;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void xvr_ValidationEventHandler(object sender, ValidationEventArgs e)
		{
			XmlValidationMessage += e.Message + "\n";
			XmlValidationMessage += "Severity: " + e.Severity + "\n\n";
		}
		#region IMod Members

		/// <summary>
		/// 
		/// </summary>
		public new Phpbb.ModTeam.Tools.DataStructures.ModFormats LastReadFormat
		{
			get
			{
				return base.LastReadFormat;
			}
			set
			{
				base.LastReadFormat = value;
			}
		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		public static explicit operator TextMod(ModxMod m)
		{
			TextMod n = new TextMod();
			n.Actions = m.Actions;
			n.AuthorNotesIndent = m.AuthorNotesIndent;
			n.AuthorNotesStartLine = m.AuthorNotesStartLine;
			n.Header = m.Header;
			n.LastReadFormat = m.LastReadFormat;
			n.ModFilesToEditIndent = m.ModFilesToEditIndent;
			n.ModIncludedFilesIndent = m.ModIncludedFilesIndent;
			return n;
		}
	}
}
