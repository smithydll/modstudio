/***************************************************************************
 *                                PhpbbMod.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: PhpbbMod.cs,v 1.10 2005-10-09 11:19:37 smithydll Exp $
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
using ModTemplateTools.DataStructures;

namespace ModTemplateTools
{
	/// <summary>
	/// The phpBBMOD class is a class based memory representation of the phpBB MOD Template. Compatible with 
	/// both the Text and XML Templates as of the July 2005 updates.
	/// </summary>
	public class PhpbbMod
	{

		private static string defaultLanguage = "en-GB";

		/// <summary>
		/// 
		/// </summary>
		public static string DefaultLanguage
		{
			get
			{
				return defaultLanguage;
			}
			set
			{
				defaultLanguage = value;
			}
		}

		private const char Newline = '\n';
		private const string WinNewLine = "\r\n";

		private char[] TrimChars = {' ', '\t', '\n', '\r', '\b'};

		/// <summary>
		/// 
		/// </summary>
		public ModHeader Header;

		/// <summary>
		/// 
		/// </summary>
		public ModActions Actions;

		private string TextTemplate;

		/// <summary>
		/// The different supported MOD file formats.
		/// </summary>
		public enum ModFormats
		{
			/// <summary>
			/// An XML MOD file format.
			/// </summary>
			XMLMOD,
			/// <summary>
			/// A readable text based file format.
			/// </summary>
			TextMOD
		}

		/// <summary>
		/// Respresents a modification header section.
		/// </summary>
		public class ModHeader
		{
			/// <summary>
			/// 
			/// </summary>
			public PropertyLang ModTitle = new PropertyLang();
			/// <summary>
			/// 
			/// </summary>
			public ModAuthor ModAuthor;
			/// <summary>
			/// 
			/// </summary>
			public PropertyLang ModDescription = new PropertyLang();
			/// <summary>
			/// 
			/// </summary>
			public ModVersion ModVersion;
			/// <summary>
			/// 
			/// </summary>
			public ModInstallationLevel ModInstallationLevel;
			/// <summary>
			/// 
			/// </summary>
			public int ModInstallationTime;
			/// <summary>
			/// 
			/// </summary>
			public int ModSuggestedInstallTime;
			/// <summary>
			/// 
			/// </summary>
			public System.Collections.Specialized.StringCollection ModFilesToEdit; //string[]
			/// <summary>
			/// 
			/// </summary>
			public System.Collections.Specialized.StringCollection ModIncludedFiles; //string[]
			/// <summary>
			/// 
			/// </summary>
			public string ModGenerator;
			/// <summary>
			/// 
			/// </summary>
			public PropertyLang ModAuthorNotes = new PropertyLang();
			/// <summary>
			/// 
			/// </summary>
			public ModVersion ModEasymodCompatibility;
			/// <summary>
			/// 
			/// </summary>
			public ModHistory ModHistory;
			/// <summary>
			/// 
			/// </summary>
			public ModVersion ModphpBBVersion;
			/// <summary>
			/// 
			/// </summary>
			public Hashtable Meta = new Hashtable();
			/// <summary>
			/// 
			/// </summary>
			public string License;
		}
		
		/// <summary>
		/// 
		/// </summary>
		public struct ModVersion
		{
			/// <summary>
			/// 
			/// </summary>
			public int VersionMajor;
			/// <summary>
			/// 
			/// </summary>
			public int VersionMinor;
			/// <summary>
			/// 
			/// </summary>
			public int VersionRevision;
			/// <summary>
			/// 
			/// </summary>
			public char VersionRelease;
			/// <summary>
			/// 
			/// </summary>
			public const char nullChar = '-';

			/// <summary>
			/// 
			/// </summary>
			/// <param name="VersionMajor"></param>
			/// <param name="VersionMinor"></param>
			/// <param name="VersionRevision"></param>
			public ModVersion(int VersionMajor, int VersionMinor, int VersionRevision)
			{
				this.VersionMajor = VersionMajor;
				this.VersionMinor = VersionMinor;
				this.VersionRevision = VersionRevision;
				this.VersionRelease = nullChar;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="VersionMajor"></param>
			/// <param name="VersionMinor"></param>
			/// <param name="VersionRevision"></param>
			/// <param name="VersionRelease"></param>
			public ModVersion(int VersionMajor, int VersionMinor, int VersionRevision, char VersionRelease)
			{
				this.VersionMajor = VersionMajor;
				this.VersionMinor = VersionMinor;
				this.VersionRevision = VersionRevision;
				this.VersionRelease = VersionRelease;
			}

			/// <summary>
			/// Convert the current MOD Version to a string.
			/// </summary>
			/// <returns>A string of the form x.y.za</returns>
			public override string ToString()
			{
				if (this.VersionRelease == nullChar) 
				{
					return string.Format("{0}.{1}.{2}", this.VersionMajor.ToString(), this.VersionMinor.ToString(), this.VersionRevision.ToString());
				} 
				else 
				{
					return string.Format("{0}.{1}.{2}{3}", this.VersionMajor.ToString(), this.VersionMinor.ToString(), this.VersionRevision.ToString(), this.VersionRelease.ToString());
				}
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="input"></param>
			/// <returns></returns>
			public static ModVersion Parse(string input)
			{
				char[] TrimChars = {' ', '\t', '\n', '\r', '\b'};
				ModVersion MVersion = new ModVersion();
				input = Regex.Replace(input.Trim(TrimChars), "([\\d]+?)\\.([\\d]+?)(\\.|)([\\d]+?|)([a-zA-Z]{0,1}?)([ \\t]|)$", "$1.$2.$4.$5");
				string[] MV = input.Split('.');
				if (MV.Length >= 1) 
				{
					MVersion.VersionMajor = int.Parse(MV[0]);
				}
				if (MV.Length >= 2) 
				{
					if (!(MV[1] == null)) 
					{
						MVersion.VersionMinor = int.Parse(MV[1]);
					}
				}
				if (MV.Length >= 3) 
				{
					if (!(MV[2] == null)) 
					{
						MVersion.VersionRevision = int.Parse(MV[2]);
					}
				}
				if (MV.Length >= 4) 
				{
					if (MV[3].Length > 0)
					{
						if (Regex.IsMatch(MV[3], "^([a-zA-Z])$") && MV[3] != "\b")
						{
							MVersion.VersionRelease = MV[3].ToCharArray()[0];
						}
						else
						{
							MVersion.VersionRelease = ModVersion.nullChar;
						}
					}
					if (MVersion.VersionRelease.GetHashCode() == 0)
					{
						MVersion.VersionRelease = ModVersion.nullChar;
					}
				} 
				else 
				{
					MVersion.VersionRelease = ModVersion.nullChar;
				}
				return MVersion;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public enum ModInstallationLevel
		{
			/// <summary>
			/// 
			/// </summary>
			Easy,
			/// <summary>
			/// 
			/// </summary>
			Moderate,
			/// <summary>
			/// 
			/// </summary>
			Hard
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static ModInstallationLevel ModInstallationLevelParse(string input)
		{
			char[] trimChars = {'(', ')'};
			if (input.ToUpper().Trim(trimChars) == "EASY") 
			{
				return ModInstallationLevel.Easy;
			} 
			else if (input.ToUpper().Trim(trimChars) == "MODERATE") 
			{
				return ModInstallationLevel.Moderate;
			} 
			else if (input.ToUpper().Trim(trimChars) == "HARD") 
			{
				return ModInstallationLevel.Hard;
			} 
			else 
			{
				return ModInstallationLevel.Easy;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static int StringToSeconds(string input)
		{
			string[] getints = input.Split(' ');
			int seconds = 0;
			for (int i = 0; i <= getints.GetUpperBound(0); i++) 
			{
				if (Regex.IsMatch(getints[i], "minute", RegexOptions.IgnoreCase)) 
				{
					seconds += int.Parse(Regex.Replace(Regex.Replace(getints[i - 1], "^([A-Za-z~ ]*?)([0-9\\.]+?)([A-Za-z ]*?)$", "$2"), "^([0-9]+)\\-", "")) * 60;
				}
				if (Regex.IsMatch(getints[i], "second", RegexOptions.IgnoreCase)) 
				{
					seconds += int.Parse(Regex.Replace(getints[i - 1], "^([A-Za-z~ ]*?)([0-9\\.]+?)([A-Za-z ]*?)$", "$2"));
				}
				if (Regex.IsMatch(getints[i], "hour", RegexOptions.IgnoreCase)) 
				{
					seconds += System.Convert.ToInt32(Regex.Replace(getints[i - 1], "^([A-Za-z~ ]*?)([0-9\\.]+?)([A-Za-z ]*?)$", "$2")) * (int)(Math.Pow(60, 2));
				}
			}
			return seconds;
		}

		/// <summary>
		/// A MOD History Entry
		/// </summary>
		public struct ModHistoryEntry
		{
			/// <summary>
			/// 
			/// </summary>
			public ModVersion HistoryVersion;
			/// <summary>
			/// 
			/// </summary>
			public System.DateTime HistoryDate;
			/// <summary>
			/// 
			/// </summary>
			public PropertyLang HistoryChanges;

			/// <summary>
			/// 
			/// </summary>
			/// <param name="HistoryVersion"></param>
			/// <param name="HistoryDate"></param>
			/// <param name="HistoryChanges"></param>
			public ModHistoryEntry(ModVersion HistoryVersion, System.DateTime HistoryDate, string HistoryChanges)
			{
				this.HistoryVersion = HistoryVersion;
				this.HistoryDate = HistoryDate;
				this.HistoryChanges = new PropertyLang(HistoryChanges, PhpbbMod.DefaultLanguage);
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="HistoryVersion"></param>
			/// <param name="HistoryDate"></param>
			/// <param name="HistoryChanges"></param>
			public ModHistoryEntry(ModVersion HistoryVersion, System.DateTime HistoryDate, PropertyLang HistoryChanges)
			{
				this.HistoryVersion = HistoryVersion;
				this.HistoryDate = HistoryDate;
				this.HistoryChanges = HistoryChanges;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="yes"></param>
			public ModHistoryEntry(bool yes)
			{
				this.HistoryVersion = new ModVersion(0,0,0);
				this.HistoryDate = DateTime.Now;
				this.HistoryChanges = new PropertyLang();
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="_value"></param>
			/// <param name="_language"></param>
			public void AddLanguage(string _value, string _language)
			{
				HistoryChanges[_language] = _value;
			}
		}

		/// <summary>
		/// A collection of useful methods for organising ModHistory entries.
		/// </summary>
		public struct ModHistory
		{
			/// <summary>
			/// The string which represents an empty MOD Author field.
			/// </summary>
			public const string Empty = "N/A";

			/// <summary>
			/// 
			/// </summary>
			public ModHistoryEntry[] History;

			/// <summary>
			/// 
			/// </summary>
			/// <param name="HistoryVersion"></param>
			/// <param name="HistoryDate"></param>
			/// <param name="HistoryChanges"></param>
			public void AddEntry(ModVersion HistoryVersion, System.DateTime HistoryDate, string HistoryChanges)
			{
				AddEntry(new ModHistoryEntry(HistoryVersion, HistoryDate, HistoryChanges));
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="newhistory"></param>
			public void AddEntry(ModHistoryEntry newhistory)
			{
				if (History != null)
				{
					ModHistoryEntry[] tempArray = History;
					History = new ModHistoryEntry[tempArray.Length + 1];
					tempArray.CopyTo(History, 0);
				}
				else
				{
					History = new ModHistoryEntry[1];
				}

				History[History.GetUpperBound(0)] = newhistory;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="index"></param>
			public void RemoveEntry(int index)
			{
				ModHistoryEntry[] tempHistory = History;

				History = new ModHistoryEntry[History.Length -1];

				for (int i = 0; i < index; i++) 
				{
					History[i] = tempHistory[i];
				}
				for (int i = index; i < tempHistory.Length - 1; i++) 
				{
					History[i] = tempHistory[i + 1];
				}
				tempHistory = null;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public enum ModAuthorStatus
		{
			/// <summary>
			/// 
			/// </summary>
			Current,
			/// <summary>
			/// 
			/// </summary>
			Past
		}

		/// <summary>
		/// 
		/// </summary>
		public struct ModAuthorEntry
		{
			/// <summary>
			/// 
			/// </summary>
			public string UserName;
			/// <summary>
			/// 
			/// </summary>
			public string RealName;
			/// <summary>
			/// 
			/// </summary>
			public string Email;
			/// <summary>
			/// 
			/// </summary>
			public string Homepage;
			/// <summary>
			/// 
			/// </summary>
			public int AuthorFrom;
			/// <summary>
			/// 
			/// </summary>
			public int AuthorTo;
			/// <summary>
			/// 
			/// </summary>
			public ModAuthorStatus Status;

				/// <summary>
				/// 
				/// </summary>
				/// <param name="UserName">username</param>
				/// <param name="RealName">real name</param>
				/// <param name="Email">e-mail</param>
				/// <param name="Homepage">homepage</param>
			public ModAuthorEntry(string UserName, string RealName, string Email, string Homepage)
			{
				this.UserName = UserName;
				this.RealName = RealName;
				this.Email = Email;
				this.Homepage = Homepage;
				this.AuthorFrom = DateTime.Now.Year;
				this.AuthorTo = DateTime.Now.Year;
				this.Status = ModAuthorStatus.Current;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="UserName">username</param>
			/// <param name="RealName">real name</param>
			/// <param name="Email">e-mail</param>
			/// <param name="Homepage">homepage</param>
			/// <param name="AuthorFrom">Date author started</param>
			/// <param name="AuthorTo">Date author finished</param>
			/// <param name="Status">The MOD Authors Status, Current or Past</param>
			public ModAuthorEntry(string UserName, string RealName, string Email, string Homepage, int AuthorFrom, int AuthorTo, ModAuthorStatus Status)
			{
				this.UserName = UserName;
				this.RealName = RealName;
				this.Email = Email;
				this.Homepage = Homepage;
				this.AuthorFrom = AuthorFrom;
				this.AuthorTo = AuthorTo;
				this.Status = Status;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="yes"></param>
			public ModAuthorEntry(bool yes)
			{
				this.UserName = "N/A";
				this.RealName = "N/A";
				this.Email = "N/A";
				this.Homepage = "N/A";
				this.AuthorFrom = DateTime.Now.Year;
				this.AuthorTo = DateTime.Now.Year;
				this.Status = ModAuthorStatus.Current;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <returns></returns>
			public override string ToString()
			{
				return string.Format("{0} < {1} > ({2}) {3}", this.UserName, this.Email, this.RealName, this.Homepage);
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="input"></param>
			/// <returns></returns>
			public static ModAuthorEntry Parse(string input)
			{
				string[] MODTempAuthor = Regex.Replace(input, "^(## MOD Author(|, secondary):|)([\\W]+?)((?!n\\/a)[\\w\\s\\.\\-]+?|)\\W<(\\W|)(n\\/a|[a-z0-9\\(\\) \\.\\-_\\+\\[\\]@]+|)(\\W|)>\\W(\\((([\\w\\s\\.\\'\\-]+?)|n\\/a)\\)|)(\\W|)(([a-z]+?://){1}([a-z0-9\\-\\.,\\?!%\\*_\\#:;~\\\\&$@\\/=\\+\\(\\)]+)|n\\/a|)(([\\W]+?)|)$", "$3\t$5\t$8\t$11", RegexOptions.IgnoreCase).Split('\t');
				return new ModAuthorEntry(MODTempAuthor[0].TrimStart(' ').TrimStart('\t'), MODTempAuthor[2], MODTempAuthor[1].TrimEnd(' '), MODTempAuthor[3]);
			}

		}

		/// <summary>
		/// 
		/// </summary>
		public struct ModAuthor
		{
			/// <summary>
			/// 
			/// </summary>
			public ModAuthorEntry[] Authors;

			/// <summary>
			/// 
			/// </summary>
			/// <param name="UserName"></param>
			/// <param name="RealName"></param>
			/// <param name="Email"></param>
			/// <param name="Homepage"></param>
			public void AddEntry(string UserName, string RealName, string Email, string Homepage)
			{
				AddEntry(new ModAuthorEntry(UserName, RealName, Email, Homepage));
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="newauthor"></param>
			public void AddEntry(ModAuthorEntry newauthor)
			{
				if (Authors != null)
				{
					ModAuthorEntry[] tempArray = Authors;
					Authors = new ModAuthorEntry[tempArray.Length + 1];
					tempArray.CopyTo(Authors, 0);
				}
				else
				{
					Authors = new ModAuthorEntry[1];
				}

				Authors[Authors.GetUpperBound(0)] = newauthor;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="index"></param>
			public void RemoveEntry(int index)
			{
				ModAuthorEntry[] tempAuthors = Authors;

				Authors = new ModAuthorEntry[Authors.Length -1];

				for (int i = 0; i < index; i++) 
				{
					Authors[i] = tempAuthors[i];
				}
				for (int i = index; i < tempAuthors.Length - 1; i++) 
				{
					Authors[i] = tempAuthors[i + 1];
				}
				tempAuthors = null;
			}
		}

		/// <summary>
		/// A variable containing the last file format that was read used for saving purposes. Defaults to XML.
		/// </summary>
		private ModFormats LastReadFormat = ModFormats.XMLMOD;

		/// <summary>
		/// 
		/// </summary>
		public ModFormats lastReadFormat
		{
			get
			{
				return LastReadFormat;
			}
			set
			{
				LastReadFormat = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public PhpbbMod(string TemplatePath)
		{
			TextTemplate = OpenTextFile(TemplatePath + "\\MOD.mot");
			try 
			{
				TextTemplate = Regex.Replace(TextTemplate, "^#(.*?)([\\s\\S]*?)(|\\r)\\n###END OF HEADER###(|\\r)\\n", "");
				TextTemplate = TextTemplate.Replace("\r", "");
			} 
			catch 
			{
			}

			Header = new ModHeader();
			Header.ModAuthor = new ModAuthor();
			Header.ModHistory = new ModHistory();
			Header.ModIncludedFiles = new System.Collections.Specialized.StringCollection();
			Header.ModFilesToEdit = new System.Collections.Specialized.StringCollection();
			Actions = new ModActions();
		}

		/// <summary>
		/// Read and parse a text MOD.
		/// </summary>
		/// <param name="TextMod">A string containing the text of the MOD in Text format.</param>
		public void ReadText(string TextMod)
		{
			LastReadFormat = ModFormats.TextMOD;
			ReadTextHeader(TextMod);
			ReadTextActions(TextMod);
		}

		/// <summary>
		/// Read and parse a text MOD's header information. It assumes everything is in the default language i.e. English (en-GB).
		/// </summary>
		/// <param name="TextMod">The MOD to parse.</param>
		private void ReadTextHeader(string TextMod)
		{
			double start = Environment.TickCount;
			TextMod = TextMod.Replace("\r", "");
			string[] TextModLines = TextMod.Split(Newline);
			int StartOffset = 0;
			int e = 1;
			bool InMultiLineElement = false;
			for (int j = 1; j <= 12; j++) 
			{
				for (int i = StartOffset; i < TextModLines.Length; i++) 
				{
					if (TextModLines[i].StartsWith("## Before Adding This MOD"))
					{
						e += 1;
						i = TextModLines.Length;
					}
					
					switch (e)
					{
						case 1:
							if (TextModLines[i].StartsWith("## EasyMod")) 
							{
								ModVersion EMVersion = ModVersion.Parse((TextModLines[i].Replace("## EasyMod", "").Replace("compliant", "").Replace("compatible", "")));
								Header.ModEasymodCompatibility = EMVersion;
								StartOffset = i + 1;
								e++;
								i = TextModLines.Length;
								}
							if (TextModLines[i].StartsWith("##")) 
							{
								StartOffset = i + 1;
								e += 1;
								i = TextModLines.Length;
							}
							break;
						case 2:
							if (TextModLines[i].ToUpper().StartsWith("## MOD TITLE")) 
							{
								Header.ModTitle = new PropertyLang(Regex.Replace(TextModLines[i], "\\#\\# MOD Title\\:", "", RegexOptions.IgnoreCase).Trim(TrimChars));
								StartOffset = i + 1;
								e++;
								i = TextModLines.Length;
							}
							break;
						case 3:
							if (TextModLines[i].ToUpper().StartsWith("## MOD AUTHOR")) 
							{
								string[] MODTempAuthor = Regex.Replace(TextModLines[i], "^## MOD Author(|, secondary):([\\W]+?)((?!n\\/a)[\\w\\s\\.\\-]+?|)\\W<(\\W|)(n\\/a|[a-z0-9\\(\\) \\.\\-_\\+\\[\\]@]+|)(\\W|)>\\W(\\((([\\w\\s\\.\\'\\-]+?)|n\\/a)\\)|)(\\W|)(([a-z]+?://){1}([a-z0-9\\-\\.,\\?!%\\*_\\#:;~\\\\&$@\\/=\\+\\(\\)]+)|n\\/a|)(([\\W]+?)|)$", "$3\t$5\t$8\t$11", RegexOptions.IgnoreCase).Split('\t');
								Header.ModAuthor.AddEntry(MODTempAuthor[0].TrimStart(' ').TrimStart('\t'), MODTempAuthor[2], MODTempAuthor[1].TrimEnd(' '), MODTempAuthor[3]);
							} 
							else if (TextModLines[i].ToUpper().StartsWith("## MOD DESCRIPTION")) 
							{
								StartOffset = i;
								e++;
								i = TextModLines.Length;
							}
							break;
						case 4:
							if (TextModLines[i].ToUpper().StartsWith("## MOD DESCRIPTION")) 
							{
								Header.ModDescription = new PropertyLang(Regex.Replace(TextModLines[i], "\\#\\# MOD Description\\:", "", RegexOptions.IgnoreCase).TrimStart(' ').TrimStart('\t').TrimEnd(' '));
							} 
							else 
							{
								if (TextModLines[i].ToUpper().StartsWith("## MOD V")) 
								{
									StartOffset = i;
									e++;
									i = TextModLines.Length;
								} 
								else 
								{
									string tempii = null;
									if (TextModLines[i].StartsWith("##"))
									{
										tempii = TextModLines[i].Substring(2, TextModLines[i].Length - 2);
									}
									else
									{
										tempii = TextModLines[i];
									}
									Header.ModDescription.pValue += Newline + tempii.TrimStart(' ').TrimStart('\t').TrimEnd(' ');
								}
							}
							break;
						case 5:
							if (TextModLines[i].ToUpper().StartsWith("## MOD VERSION")) 
							{
								ModVersion MVersion = ModVersion.Parse(Regex.Replace(TextModLines[i], "\\#\\# MOD VERSION\\:([\\W]+?)([\\d]+?)\\.([\\d]+?)(\\.|)([\\d]+?|)([a-zA-Z]{0,1}?)", "$2.$3.$5$6", RegexOptions.IgnoreCase));
								Header.ModVersion = MVersion;
								StartOffset = i + 1;
								e++;
								i = TextModLines.Length;
							}
							break;
						case 6:
							if (TextModLines[i].ToUpper().StartsWith("## INSTALLATION LEVEL")) 
							{
								Header.ModInstallationLevel = ModInstallationLevelParse(Regex.Replace(TextModLines[i], "\\#\\# Installation level\\:", "", RegexOptions.IgnoreCase).TrimStart(' ').TrimStart('\t').TrimEnd(' '));
								StartOffset = i + 1;
								e++;
								i = TextModLines.Length;
							}
							break;
						case 7:
							if (TextModLines[i].ToUpper().StartsWith("## INSTALLATION TIME")) 
							{
								Header.ModInstallationTime = StringToSeconds(Regex.Replace(TextModLines[i], "\\#\\# Installation Time\\:", "", RegexOptions.IgnoreCase).TrimStart(' ').TrimStart('\t').TrimEnd(' '));
								StartOffset = i + 1;
								e++;
								i = TextModLines.Length;
							}
							break;
						case 8:
							if (TextModLines[i].ToUpper().StartsWith("## FILES TO EDIT")) 
							{
								//Header.ModFilesToEdit = new string[1]; 
								Header.ModFilesToEdit.Add(Regex.Replace(TextModLines[i], "\\#\\# Files To Edit\\:", "", RegexOptions.IgnoreCase).TrimStart(' ').TrimStart('\t').TrimEnd(' '));
							} 
							else 
							{
								if (TextModLines[i].ToUpper().StartsWith("## INCLUDED ")) 
								{
									StartOffset = i;
									e++;
									i = TextModLines.Length;
								} 
								else 
								{
									string tempii = null;
									if (TextModLines[i].StartsWith("##"))
									{
										tempii = TextModLines[i].Substring(2, TextModLines[i].Length - 2);
									}
									else
									{
										tempii = TextModLines[i];
									}
									/*string[] tempA = Header.ModFilesToEdit;
									Header.ModFilesToEdit = new string[tempA.Length + 1];
									tempA.CopyTo(Header.ModFilesToEdit, 0);*/

									Header.ModFilesToEdit.Add(tempii.TrimStart(' ').TrimStart('\t').TrimEnd(' '));
								}
							}
							break;
						case 9:
							if (TextModLines[i].ToUpper().StartsWith("## INCLUDED FILES")) 
							{
								//Header.ModIncludedFiles = new string[1];
								Header.ModIncludedFiles.Add(Regex.Replace(TextModLines[i], "\\#\\# Included Files\\:", "", RegexOptions.IgnoreCase).TrimStart(' ').TrimStart('\t').TrimEnd(' '));
							} 
							else 
							{
								if (TextModLines[i].ToUpper().StartsWith("######") || TextModLines[i].ToUpper().StartsWith("## GEN") || TextModLines[i].ToUpper().StartsWith("## LICENSE")) 
								{
									StartOffset = i;
									e++;
									i = TextModLines.Length;
								} 
								else 
								{
									string tempii = null;
									if (TextModLines[i].StartsWith("##"))
									{
										tempii = TextModLines[i].Substring(2, TextModLines[i].Length - 2);
									}
									else
									{
										tempii = TextModLines[i];
									}
									/*string[] tempA = Header.ModIncludedFiles;
									Header.ModIncludedFiles = new string[tempA.Length + 1];
									tempA.CopyTo(Header.ModIncludedFiles, 0);*/

									Header.ModIncludedFiles.Add(tempii.TrimStart(' ').TrimStart('\t').TrimEnd(' '));
								}
							}
							break;
						case 10:
							if (TextModLines[i].ToUpper().StartsWith("## LICENSE")) 
							{
								Header.License = Regex.Replace(TextModLines[i], "\\#\\# LICENSE\\:", "", RegexOptions.IgnoreCase).Trim(TrimChars);
								StartOffset = i + 1;
								e++;
								i = TextModLines.Length;
								break;
							}
							if (TextModLines[i].ToUpper().StartsWith("## AUTHOR NOTE")) 
							{
								StartOffset = i - 1;
								e++;
								i = TextModLines.Length;
							}
							break;
						case 11:
							if (TextModLines[i].ToUpper().StartsWith("## AUTHOR NOTE")) 
							{
								Header.ModAuthorNotes = new PropertyLang(Regex.Replace(TextModLines[i], "\\#\\# Author Note(s|)\\:(\\W|)", "", RegexOptions.IgnoreCase));
								InMultiLineElement = true;
							} 
							else 
							{
								if (TextModLines[i].ToUpper().StartsWith("####") && InMultiLineElement) 
								{
									StartOffset = i + 1;
									e++;
									InMultiLineElement = false;
									i = TextModLines.Length;
								} 
								else 
								{
									Header.ModAuthorNotes.pValue += Newline + TextModLines[i].Replace("## ", "").Replace("##", "");
								}
							}
							break;
						case 12:
							if (TextModLines[i].ToUpper().StartsWith("## MOD HISTORY")) 
							{
								InMultiLineElement = true;
							} 
							else 
							{
								if (TextModLines[i].ToUpper().StartsWith("####") && InMultiLineElement) 
								{
									StartOffset = i;
									e++;
									InMultiLineElement = false;
									i = TextModLines.Length;
								} 
								else 
								{
									if (Regex.IsMatch(TextModLines[i], " - Version", RegexOptions.IgnoreCase)) 
									{
										//ModVersion HVersion = ModVersion.Parse(Regex.Match(TextModLines[i], "([\\d]+?)\\.([\\d]+?)(\\.|)([\\d]+?|)([a-zA-Z]{1}?|)([ \\t]|)$").Value);
										//Console.WriteLine(Regex.Replace(TextModLines[i], "^\\#\\#([0-9\\- \\t]+?)Version ([\\d]+?)\\.([\\d]+?)(\\.|)([\\d]+?|)([a-zA-Z]{0,1}?)([ \\t]|)$", "$2.$3.$5$6", RegexOptions.IgnoreCase));
										ModVersion HVersion = ModVersion.Parse(Regex.Replace(TextModLines[i], "^\\#\\#([0-9\\- \\t]+?)Version ([\\d]+?)\\.([\\d]+?)(\\.|)([\\d]+?|)([a-zA-Z]{0,1}?)([ \\t]|)$", "$2.$3.$5$6", RegexOptions.IgnoreCase));
										Header.ModHistory.AddEntry(HVersion,System.DateTime.Parse(Regex.Match(TextModLines[i], "([0-9]+)(\\/|\\\\|\\-)([0-9]+)(\\/|\\\\|\\-)([0-9]+)").Value),"");
									} 
									else 
									{
										if (!((TextModLines[i] == "##" || TextModLines[i] == "## "))) 
										{
											int UB = Header.ModHistory.History.GetUpperBound(0);
											if (Header.ModHistory.History[UB].HistoryChanges.GetValue().Length > 0 ) Header.ModHistory.History[UB].HistoryChanges.pValue += Newline;
											Header.ModHistory.History[UB].HistoryChanges.pValue += Regex.Replace(TextModLines[i], "##([\\W]){1}", "");
										}
									}
								}
							}
							break;
					} // Switch
				} // For i
			} // For j
			return;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="TextMod"></param>
		public void ReadTextActions(string TextMod)
		{
			Actions = new ModActions();
			TextMod = TextMod.Replace("\r\n", "\n") + "\n#\n#-----[";
			string[] ModTextLines = TextMod.Split('\n');

			bool InMODAction = false;
			string ThisMODActionBody = "";
			string ThisMODActionType = "";
			string ThisMODActionComm = "";
			string NextMODActionComm = "";
			bool IsFirstMALine = true;
			bool IsFirstTCLine = true;
			int IntFirstMALine = 0;
			bool FirstActionFound = false;

			int HeaderEndLine = 0;
			bool Found = false;
			for (int i = ModTextLines.Length - 1; i != 0 && Found == false; i--) 
			{
				if (Regex.IsMatch(ModTextLines[i], "^(\\#){60}") && Found == false) 
				{
					Found = true;
					HeaderEndLine = i;
				}
			}

			for (int i = HeaderEndLine; i < ModTextLines.Length - 1; i++) 
			{
				if (!ModTextLines[i].StartsWith("##")) 
				{
					if (ModTextLines[i].StartsWith("#") && ModTextLines[i + 1].StartsWith("#-----[") && FirstActionFound) 
					{
						NextMODActionComm = NextMODActionComm.TrimStart('\n');
						ThisMODActionComm = ThisMODActionComm.TrimStart('\n');
						//ThisMODActionBody = ThisMODActionBody.TrimStart('\n');
						Actions.AddEntry(new ModAction(ThisMODActionType, ThisMODActionBody, NextMODActionComm, ThisMODActionComm, IntFirstMALine));
						ThisMODActionBody = "";
						ThisMODActionType = "";
						ThisMODActionComm = "";
						IsFirstMALine = true;
						IsFirstTCLine = true;
						IntFirstMALine = 0;
					}
					if (ModTextLines[i].StartsWith("#") && !(ModTextLines[i - 1].StartsWith("#"))) 
					{
						InMODAction = false;
					}
					if (InMODAction) 
					{
						if (ModTextLines[i].StartsWith("#")) 
						{
							if (!IsFirstTCLine)
							{
								ThisMODActionComm += Newline;
							}
							IsFirstTCLine = false;
							ThisMODActionComm += ModTextLines[i].TrimStart('#').TrimStart(' ');
						} 
						else 
						{
							if (!IsFirstMALine) 
							{
								ThisMODActionBody += Newline;
							}
							IsFirstMALine = false;
							ThisMODActionBody += ModTextLines[i];
						}
					}
					if (ModTextLines[i].StartsWith("#-----[")) 
					{
						InMODAction = true;
						FirstActionFound = true;
						ThisMODActionType = Regex.Replace(ModTextLines[i], "^#([\\-]+)\\[(\\W)([A-Za-z, \\/\\-\\\\]+?) \\]([\\-]+)(| )$", "$3", RegexOptions.IgnoreCase);
						if (IntFirstMALine == 0) 
						{
							IntFirstMALine = i;
						}
					}
				}
			}
		}

		/// <summary>
		/// Read and parse an XML MOD.
		/// I realise this is not the fastest code, but I am making sure this executes correctly.
		/// As the files will be only a few hundred kB at most and this only happens once it shouldn't
		/// be noticed too much. In future releases I will may implement XmlSerializer or filtering.
		/// </summary>
		/// <param name="FileName">A string containing the filename of the MOD to read.</param>
		public void ReadXml(string FileName)
		{
			LastReadFormat = ModFormats.XMLMOD;
			ModTemplateTools.mod XmlDataSet = new ModTemplateTools.mod();
			XmlDataSet.ReadXml(FileName);

			// MOD Author parsing
			for (int i = 0; i < XmlDataSet._author_group.Rows.Count; i++)
			{
				for (int j = 0; j < XmlDataSet.author.Rows.Count; j++)
				{
					if (XmlDataSet.author.Rows[j]["author-group_Id"] == XmlDataSet._author_group.Rows[i]["author-group_Id"])
					{
						ModAuthorEntry tempAuthor = new ModAuthorEntry(XmlDataSet.author.Rows[j]["username"].ToString(), 
							XmlDataSet.author.Rows[j]["realname"].ToString(),
							XmlDataSet.author.Rows[j]["email"].ToString(),
							XmlDataSet.author.Rows[j]["homepage"].ToString());
						// TODO: contributions
						Header.ModAuthor.AddEntry(tempAuthor);
					}
				}
			}

			// Multilingual MOD Title
			Header.ModTitle = new PropertyLang();
			for (int i = 0; i < XmlDataSet.title.Rows.Count; i++)
			{
				Header.ModTitle[XmlDataSet.title.Rows[i]["lang"].ToString()] = XmlDataSet.title.Rows[i]["title_Text"].ToString();
			}

			// MOD Description
			Header.ModDescription = new PropertyLang();
			for (int i = 0; i < XmlDataSet.description.Rows.Count; i++)
			{
				Header.ModDescription[XmlDataSet.description.Rows[i]["lang"].ToString()] = XmlDataSet.description.Rows[i]["description_Text"].ToString();
			}

			// Author Notes
			Header.ModAuthorNotes = new PropertyLang();
			for (int i = 0; i < XmlDataSet._author_notes.Rows.Count; i++)
			{
				Header.ModAuthorNotes[XmlDataSet._author_notes.Rows[i]["lang"].ToString()] = XmlDataSet._author_notes.Rows[i]["author-notes_Text"].ToString();
			}

			// MOD History entries
			for (int i = 0; i < XmlDataSet.entry.Rows.Count; i++)
			{
				ModHistoryEntry tempHistory = new ModHistoryEntry();
				for (int j = 0; j < XmlDataSet._rev_version.Rows.Count; j++)
				{
					if (XmlDataSet._rev_version.Rows[j]["entry_Id"] == XmlDataSet.entry.Rows[i]["entry_Id"])
					{
						ModVersion tempVersion;
						tempVersion = new ModVersion(
							int.Parse(XmlDataSet._rev_version.Rows[j]["major"].ToString()),
							int.Parse(XmlDataSet._rev_version.Rows[j]["minor"].ToString()), 
							int.Parse(XmlDataSet._rev_version.Rows[j]["revision"].ToString()));
						if (XmlDataSet._rev_version.Rows[j]["release"].ToString().ToCharArray().Length == 1)
						{
							tempVersion.VersionRelease = XmlDataSet._rev_version.Rows[j]["release"].ToString().ToCharArray()[0];
						}
						tempHistory.HistoryVersion = tempVersion;
					}
				}
				tempHistory.HistoryDate = DateTime.Parse(XmlDataSet.entry.Rows[i]["date"].ToString());
				StringBuilder tempChangeLog = new StringBuilder();
				for (int j = 0; j < XmlDataSet.changelog.Rows.Count; j++)
				{
					if (XmlDataSet.changelog.Rows[j]["entry_Id"] == XmlDataSet.entry.Rows[i]["entry_Id"])
					{
						for (int k = 0; k < XmlDataSet.change.Rows.Count; k++)
						{
							tempHistory.HistoryChanges = new PropertyLang();
							if (XmlDataSet.change.Rows[k]["changelog_Id"] == XmlDataSet.changelog.Rows[j]["changelog_Id"])
							{
								tempHistory.HistoryChanges[XmlDataSet.changelog.Rows[k]["lang"].ToString()] = XmlDataSet.change.Rows[k]["change_Text"].ToString();
							}
						}
					}
				}
				Header.ModHistory.AddEntry(tempHistory);
			}

			// meta
			Header.Meta = new Hashtable();
			for (int i = 0; i < XmlDataSet.meta.Rows.Count; i++)
			{
				Header.Meta.Add(XmlDataSet.meta.Rows[i]["name"],XmlDataSet.meta.Rows[i]["content"]);
			}

			// MOD Version
			try
			{
				ModVersion tempVersion = new ModVersion();
				tempVersion.VersionMajor = (int)XmlDataSet._mod_version.Rows[0]["major"];
				tempVersion.VersionMinor = (int)XmlDataSet._mod_version.Rows[0]["minor"];
				tempVersion.VersionRevision = (int)XmlDataSet._mod_version.Rows[0]["revision"];
				if (XmlDataSet._mod_version.Rows[0]["revision"].ToString().ToCharArray().Length == 1)
				{
					tempVersion.VersionRelease = XmlDataSet._mod_version.Rows[0]["release"].ToString().ToCharArray()[0];
				}
				Header.ModVersion = tempVersion;
			}
			catch 
			{
				Header.ModVersion = new ModVersion(0,0,0);
			}

			// license
			try { Header.License = XmlDataSet.header.Rows[0]["license"].ToString(); }
			catch {}

			// installation
			try 
			{ 
				Header.ModInstallationLevel = ModInstallationLevelParse(XmlDataSet.installation.Rows[0]["level"].ToString());
				Header.ModInstallationTime = (int)XmlDataSet.installation.Rows[0]["time"];
				// TODO: easymod-compliant
				//Header.ModEasymodCompatibility = (bool)XmlDataSet.installation.Rows[0]["easymod-compliant"];
				// TODO: mod-config
			}
			catch {}

			// TODO: target-version

			// actions
			// action-group
			// sql
			for (int i = 0; i < XmlDataSet.sql.Rows.Count; i++)
			{
				Actions.AddEntry(new ModAction("SQL", XmlDataSet.sql.Rows[i]["sql_Text"].ToString(), ""));
			}

			// copy
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
				Actions.AddEntry(new ModAction("COPY", tempCopy.ToString(), ""));
			}

			// open
			// this is very specific, the FINDs always go BEFORE the actions
			// followed by IN-LINE actions, and lastly comments
			for (int i = 0; i < XmlDataSet.open.Rows.Count; i++)
			{
				Actions.AddEntry(new ModAction("OPEN", XmlDataSet.open.Rows[i]["src"].ToString(), ""));
				for (int j = 0; j < XmlDataSet.edit.Rows.Count; j++)
				{
					if (XmlDataSet.edit.Rows[j]["open_Id"] == XmlDataSet.open.Rows[i]["open_Id"])
					{
						for (int k = 0; k < XmlDataSet.find.Rows.Count; k++)
						{
							if (XmlDataSet.find.Rows[k]["edit_Id"] == XmlDataSet.edit.Rows[j]["edit_Id"])
							{
								Actions.AddEntry(new ModAction("FIND", XmlDataSet.find.Rows[k]["find_Text"].ToString(), "", XmlDataSet.find.Rows[k]["type"].ToString()));
							}
						}
						for (int k = 0; k < XmlDataSet.action.Rows.Count; k++)
						{
							if (XmlDataSet.action.Rows[k]["edit_Id"] == XmlDataSet.edit.Rows[j]["edit_Id"])
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
									case "replace-with":
										actionTitle = "REPLACE WITH";
										break;
									case "operation":
										actionTitle = "INCREMENT";
										break;
								}
								Actions.AddEntry(new ModAction(actionTitle, XmlDataSet.action.Rows[k]["action_Text"].ToString(), ""));
							}
						}
						for (int k = 0; k < XmlDataSet._inline_edit.Rows.Count; k++)
						{
							if (XmlDataSet._inline_edit.Rows[k]["edit_Id"] == XmlDataSet.edit.Rows[j]["edit_Id"])
							{
								for (int l = 0; l < XmlDataSet._inline_find.Rows.Count; l++)
								{
									if (XmlDataSet._inline_find.Rows[l]["_inline_edit_Id"] == XmlDataSet._inline_edit.Rows[k]["_inline_edit_Id"])
									{
										Actions.AddEntry(new ModAction("IN-LINE FIND", XmlDataSet._inline_find.Rows[l]["inline-find_Text"].ToString(), "", XmlDataSet._inline_find.Rows[l]["type"].ToString()));
									}
								}
								for (int l = 0; l < XmlDataSet._inline_action.Rows.Count; l++)
								{
									if (XmlDataSet._inline_action.Rows[l]["_inline_edit_Id"] == XmlDataSet._inline_edit.Rows[k]["_inline_edit_Id"])
									{
										string actionTitle = "";
										switch (XmlDataSet._inline_action.Rows[l]["type"].ToString())
										{
											case "after-add":
												actionTitle = "AFTER, ADD";
												break;
											case "before-add":
												actionTitle = "BEFORE, ADD";
												break;
											case "replace-with":
												actionTitle = "REPLACE WITH";
												break;
											case "operation":
												actionTitle = "INCREMENT";
												break;
										}
										Actions.AddEntry(new ModAction(actionTitle, XmlDataSet._inline_action.Rows[l]["inline-action_Text"].ToString(), ""));
									}
								}
							}
						}
					}
				}
			}

			// diy instructions
			for (int i = 0; i < XmlDataSet._diy_instructions.Rows.Count; i++)
			{
				Actions.AddEntry(new ModAction("DIY INSTRUCTIONS", XmlDataSet._diy_instructions.Rows[i]["diy-instructions_Text"].ToString(),"", XmlDataSet._diy_instructions.Rows[i]["lang"].ToString()));
			}

			Actions.AddEntry(new ModAction("SAVE/CLOSE ALL FILES", "", "EoM"));
			//XmlDataSet.Dispose();
		}

		/// <summary>
		/// Read and parse a file containing a MOD.
		/// </summary>
		/// <param name="FileName">A string containing the file path to the MOD.</param>
		public void ReadFile(string FileName)
		{
			Read(FileName);
		}

		/// <summary>
		/// Read and parse a MOD automatically deciding if it's an XML or Text based MOD.
		/// </summary>
		/// <param name="FileName">A string containing the text of the MOD.</param>
		public void Read(string FileName)
		{
			if (FileName.ToLower().EndsWith(".txt") || FileName.ToLower().EndsWith(".mod"))
			{
				string textFile = OpenTextFile(FileName);
				if (textFile.StartsWith("##")) ReadText(textFile);
			}
			else if (FileName.ToLower().EndsWith(".xml"))
			{
				 ReadXml(FileName);
			}
		}

		/// <summary>
		/// Read and parse a MOD.
		/// </summary>
		/// <param name="ModToRead">A string containing the text of the MOD.</param>
		/// <param name="Format">The format that will be parsed.</param>
		public void Read(string ModToRead, ModFormats Format)
		{
			LastReadFormat = Format;
			switch (Format)
			{
				case ModFormats.TextMOD:
					ReadText(ModToRead);
					break;
				case ModFormats.XMLMOD:
					ReadXml(ModToRead);
					break;
			}
		}

		/// <summary>
		/// Write the MOD to a file in the format that was read. If no file was read save in XML file format.
		/// </summary>
		/// <param name="FileName">A string containing the file path where the MOD is to be saved.</param>
		public void WriteFile(string FileName)
		{
			WriteFile(FileName, this.LastReadFormat);
		}

		/// <summary>
		/// Write the MOD to a file in the designated file format.
		/// </summary>
		/// <param name="FileName">A string containing the file path where the MOD is to be saved.</param>
		/// <param name="Format">The format that will be saved.</param>
		public void WriteFile(string FileName, ModFormats Format)
		{
			switch (Format)
			{
				case ModFormats.TextMOD:
					SaveTextFile(WriteText().Replace("\r","").Replace("\n","\r\n"), FileName);
					break;
				case ModFormats.XMLMOD:
					SaveTextFile(WriteXml(), FileName);
					break;
			}
		}

		/// <summary>
		/// Write a MOD in Text format.
		/// </summary>
		/// <returns>A string of the Text format of the MOD.</returns>
		public string WriteText()
		{
			//
			// TODO: Not elegant, fixed up a little, still needs some work.
			//

			string BlankTemplate = TextTemplate;
			System.Text.StringBuilder NewModBody = new System.Text.StringBuilder();
			BlankTemplate = BlankTemplate.Replace("<mod.title/>", Header.ModTitle.GetValue());
			string MyMODAuthorS = null;
			for (int i = 0; i <= Header.ModAuthor.Authors.GetUpperBound(0); i++) 
			{
				if (i == 0) 
				{
					MyMODAuthorS = Header.ModAuthor.Authors[i].UserName + " < " + Header.ModAuthor.Authors[i].Email + " > (" + Header.ModAuthor.Authors[i].RealName + ") " + Header.ModAuthor.Authors[i].Homepage;
				} 
				else 
				{
					MyMODAuthorS += Newline + "## MOD Author: " + Header.ModAuthor.Authors[i].UserName + " < " + Header.ModAuthor.Authors[i].Email + " > (" + Header.ModAuthor.Authors[i].RealName + ") " + Header.ModAuthor.Authors[i].Homepage;
				}
			}

			BlankTemplate = BlankTemplate.Replace("<mod.author/>", MyMODAuthorS);
			BlankTemplate = BlankTemplate.Replace("<mod.description/>", Header.ModDescription.GetValue().Replace(Newline.ToString(), Newline.ToString() + "## "));
			BlankTemplate = BlankTemplate.Replace("<mod.version/>", Header.ModVersion.ToString());
			BlankTemplate = BlankTemplate.Replace("<mod.install_level/>", Header.ModInstallationLevel.ToString());
			BlankTemplate = BlankTemplate.Replace("<mod.install_time/>", Math.Ceiling(Header.ModInstallationTime / 60) + " minutes");

			string MyMODFTE = null;
			for (int i = 0; i < Header.ModFilesToEdit.Count; i++) 
			{
				if (i == 0) 
				{
					MyMODFTE = Header.ModFilesToEdit[i];
				} 
				else 
				{
					MyMODFTE += Newline + "##                " + Header.ModFilesToEdit[i];
				}
			}
			BlankTemplate = BlankTemplate.Replace("<mod.files_to_edit/>", MyMODFTE);

			string MyMODIC = null;
			for (int i = 0; i < Header.ModIncludedFiles.Count; i++) 
			{
				if (i == 0) 
				{
					MyMODIC = Header.ModIncludedFiles[i];
				} 
				else 
				{
					MyMODIC += Newline + "##                 " + Header.ModIncludedFiles[i];
				}
			}
			BlankTemplate = BlankTemplate.Replace("<mod.inc_files/>", MyMODIC);
			BlankTemplate = BlankTemplate.Replace("<mod.generator/>", Newline + "## Generator: MOD Studio [ ModTemplateTools " + System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion + " ]");
			if (Header.License != "")
			{
				BlankTemplate = BlankTemplate.Replace("<mod.license/>", Newline + "## License: " + Header.License);
			}
			else
			{
				BlankTemplate = BlankTemplate.Replace("<mod.license/>", "");
			}
			BlankTemplate = BlankTemplate.Replace("<mod.author_notes/>", Header.ModAuthorNotes.GetValue().Replace(Newline.ToString(), Newline.ToString() + "## "));
			string MyMODHistory;
			System.Text.StringBuilder NewMyMODHistory = new System.Text.StringBuilder();
			try
			{
				for (int i = 0; i < Header.ModHistory.History.Length; i++) 
				{
					if (i == 0 && !((Header.ModHistory.History[i].HistoryChanges.GetValue() == null))) 
					{
						NewMyMODHistory.Append("##############################################################");
						NewMyMODHistory.Append(Newline + "## MOD History:");
						NewMyMODHistory.Append(Newline + "## ");
						NewMyMODHistory.Append(Newline + "## " + Header.ModHistory.History[i].HistoryDate.ToString("yyyy-MM-dd") + " - Version " + Header.ModHistory.History[i].HistoryVersion.ToString());
						if (!(Header.ModHistory.History[i].HistoryChanges.GetValue() == null)) 
						{
							NewMyMODHistory.Append(Newline + "## " + Header.ModHistory.History[i].HistoryChanges.GetValue().Replace(Newline.ToString(), Newline.ToString() + "## "));
						} 
						else 
						{
							NewMyMODHistory.Append(Newline + "## - ");
						}
					} 
					else 
					{
						NewMyMODHistory.Append(Newline + "## ");
						NewMyMODHistory.Append(Newline + "## " + Header.ModHistory.History[i].HistoryDate.ToString("yyyy-MM-dd") + " - Version " + Header.ModHistory.History[i].HistoryVersion.ToString());
						if (!(Header.ModHistory.History[i].HistoryChanges.GetValue() == null)) 
						{
							NewMyMODHistory.Append(Newline + "## " + Header.ModHistory.History[i].HistoryChanges.GetValue().Replace(Newline.ToString(), Newline.ToString() + "## "));
						} 
						else 
						{
							NewMyMODHistory.Append(Newline + "## - ");
						}
					}
				}
			}
			catch
			{
			}
			MyMODHistory = NewMyMODHistory.ToString();
			if (!(MyMODHistory.Length == 0)) 
			{
				MyMODHistory = Newline + MyMODHistory;
				MyMODHistory += Newline + "## ";
			}
			BlankTemplate = BlankTemplate.Replace("<mod.history/>", MyMODHistory);
			BlankTemplate = Regex.Replace(BlankTemplate, "^#(.*?)([\\s\\S]*?)\\n###END OF HEADER###\\n", "", RegexOptions.Compiled);
			if (Header.ModEasymodCompatibility.VersionMinor > 0 || Header.ModEasymodCompatibility.VersionRevision > 0) 
			{
				BlankTemplate = "## EasyMod " + Header.ModEasymodCompatibility.ToString() + " compliant" + Newline + BlankTemplate;
			}
			NewModBody.Append(BlankTemplate);
			try 
			{
				foreach (ModAction MA in Actions) 
				{
					if (MA.ActionType != null) 
					{
						
						NewModBody.Append(Newline);
						NewModBody.Append("#");
						NewModBody.Append(Newline);
						NewModBody.Append("#-----[ " + MA.ActionType + " ]------------------------------------------");
						NewModBody.Append(Newline);
						NewModBody.Append("#");
						if (!((MA.AfterComment == null || MA.AfterComment == Newline.ToString()))) 
						{
							string[] ACsplit = MA.AfterComment.Replace("\r", "").Split(Newline);
							for (int j = 0; j <= ACsplit.GetUpperBound(0); j++) 
							{
								if (!((ACsplit[j] == "" && j == 0))) 
								{
									NewModBody.Append(Newline);
									NewModBody.Append("# " + ACsplit[j]);
								}
							}
						}
						NewModBody.Append(Newline);
						NewModBody.Append(MA.ActionBody);
					}
				}
			} 
			catch 
			{
			}

			return NewModBody.ToString();
		}

		/// <summary>
		/// Write a MOD in XML format.
		/// </summary>
		/// <returns>A string of the XML format of the MOD.</returns>
		public string WriteXml()
		{
			ModTemplateTools.mod XmlDataSet = new ModTemplateTools.mod();
			foreach (string Language in Header.ModTitle)
			{
				DataRow newRow = XmlDataSet.title.NewRow();
				XmlDataSet.title.Rows.Add(newRow);
			}
			return "";
		}

		/// <summary>
		/// Convert the MOD to a string in the designated format.
		/// </summary>
		/// <param name="Format">The format that will be returned.</param>
		/// <returns>A string of the text of the MOD in the reqested format.</returns>
		public string ToString(ModFormats Format)
		{
			switch (Format)
			{
				case ModFormats.XMLMOD:
					return WriteXml();
				case ModFormats.TextMOD:
					return WriteText();
			}
			return "";
		}

		/// <summary>
		/// Returns the XML representation of this MOD.
		/// </summary>
		/// <returns>A string containing the XML format of this MOD.</returns>
		public override string ToString()
		{
			return WriteXml();
		}

		/// <summary>
		/// Open a text file
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		private string OpenTextFile(string filename)
		{
			StreamReader myStreamReader;
			string temp;
			try 
			{
				myStreamReader = File.OpenText(filename);
				temp = myStreamReader.ReadToEnd();
				myStreamReader.Close();
			} 
			catch
			{
				temp = "";
			}
			return temp;
		}

		/// <summary>
		/// Save a text file
		/// </summary>
		/// <param name="filetosave"></param>
		/// <param name="filename"></param>
		private static void SaveTextFile(string filetosave, string filename)
		{
			StreamWriter myStreamWriter = File.CreateText(filename);
			myStreamWriter.Write(filetosave);
			myStreamWriter.Close();
		}

	}
}
