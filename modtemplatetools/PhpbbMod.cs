/***************************************************************************
 *                                PhpbbMod.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: PhpbbMod.cs,v 1.13 2006-01-21 02:36:27 smithydll Exp $
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

		/// <summary>
		/// This is the default language phpBB is released in.
		/// </summary>
		private static string defaultLanguage = "en-GB";

		/// <summary>
		/// This is the default language phpBB is released in. It can be overridden globally.<br />
		/// <em>Expect this to change from static to non-static in the future.</em>
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
		public CodeIndents DescriptionIndent = CodeIndents.Space;
		public CodeIndents AuthorNotesIndent = CodeIndents.Space;
		public StartLine AuthorNotesStartLine = StartLine.Same;
		public CodeIndents ModFilesToEditIndent = CodeIndents.RightAligned;
		public CodeIndents ModIncludedFilesIndent = CodeIndents.RightAligned;

		private char[] TrimChars = {' ', '\t', '\n', '\r', '\b'};

		/// <summary>
		/// The MODs header.
		/// </summary>
		public ModHeader Header;

		/// <summary>
		/// The MODs actions.
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

		private ModFormats LastReadFormat = ModFormats.XMLMOD;

		/// <summary>
		/// The format the MOD was last read into.
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
		/// Constructor
		/// </summary>
		/// <param name="TemplatePath">Path to the templates folder.</param>
		public PhpbbMod(string TemplatePath)
		{
			TextTemplate = OpenTextFile(TemplatePath + Path.DirectorySeparatorChar + "MOD.mot");
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
		/// Estimate an Installation Time for this MOD.
		/// </summary>
		public void UpdateInstallationTime()
		{
			int totalinstalltime = 126;
			foreach (ModAction e in Actions)
			{
				switch (e.ActionType)
				{
					case "OPEN":
						totalinstalltime += 27;
						break;
					case "SQL":
						totalinstalltime += 50;
						break;
					case "COPY":
						totalinstalltime += e.ActionBody.Split('n').Length * 5;
						break;
					case "FIND":
					case "IN-LINE FIND":
						totalinstalltime += 12;
						break;
					case "AFTER, ADD":
					case "BEFORE, ADD":
					case "REPLACE WITH":
					case "INCREMENT":
					case "IN-LINE AFTER, ADD":
					case "IN-LINE BEFORE, ADD":
					case "IN-LINE REPLACE WITH":
					case "IN-LINE INCREMENT":
						totalinstalltime += 18;
						break;
					case "DIY INSTRUCTIONS":
						totalinstalltime += 60;
						break;
				}
			}
			Header.ModInstallationTime = totalinstalltime;
		}

		/// <summary>
		/// Update Included files for this MOD.
		/// </summary>
		public void UpdateIncludedFiles()
		{
			for (int i = 0; i < Actions.Count; i++)
			{
				if (Actions[i].ActionType == "COPY")
				{
					string[] lines = Actions[i].ActionBody.Split('\n');
					foreach (string line in lines)
					{
						if (line.TrimStart(TrimChars).ToLower().StartsWith("copy"))
						{
							Header.ModIncludedFiles.Add(Regex.Match(line.Trim(TrimChars), "copy (.+) to", RegexOptions.IgnoreCase).Value.Replace("copy ", "").Replace(" to", ""));
						}
					}
				}
			}
		}

		/// <summary>
		/// Update Files to Edit for this MOD.
		/// </summary>
		public void UpdateFilesToEdit()
		{
			Header.ModFilesToEdit.Clear();
			foreach (ModAction e in Actions)
			{
				if (e.ActionType == "OPEN")
				{
					Header.ModFilesToEdit.Add(e.ActionBody.Trim(TrimChars));
				}
			}
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
								Header.ModTitle = new StringLocalised(Regex.Replace(TextModLines[i], "\\#\\# MOD Title\\:", "", RegexOptions.IgnoreCase).Trim(TrimChars));
								StartOffset = i + 1;
								e++;
								i = TextModLines.Length;
							}
							break;
						case 3:
							if (TextModLines[i].ToUpper().StartsWith("## MOD AUTHOR")) 
							{
								string[] MODTempAuthor = Regex.Replace(TextModLines[i], "^## MOD Author(|, secondary):([\\W]+?)((?!n\\/a)[\\w\\s\\.\\-]+?|)\\W<(\\W|)(n\\/a|[a-z0-9\\(\\) \\.\\-_\\+\\[\\]@]+|)(\\W|)>\\W(\\((([\\w\\s\\.\\'\\-]+?)|n\\/a)\\)|)(\\W|)(([a-z]+?://){1}([a-z0-9\\-\\.,\\?!%\\*_\\#:;~\\\\&$@\\/=\\+\\(\\)]+)|n\\/a|)(([\\W]+?)|)$", "$3\t$5\t$8\t$11", RegexOptions.IgnoreCase).Split('\t');
								Header.ModAuthor.Add(new ModAuthorEntry(MODTempAuthor[0].TrimStart(' ').TrimStart('\t'), MODTempAuthor[2], MODTempAuthor[1].TrimEnd(' '), MODTempAuthor[3]));
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
								Header.ModDescription = new StringLocalised(Regex.Replace(TextModLines[i], "\\#\\# MOD Description\\:", "", RegexOptions.IgnoreCase).TrimStart(' ').TrimStart('\t').TrimEnd(' '));
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
										if (tempii.StartsWith("\t"))
										{
											DescriptionIndent = CodeIndents.Tab;
										}
										else if (tempii.StartsWith("    "))
										{
											DescriptionIndent = CodeIndents.RightAligned;
										}
										else if (tempii.StartsWith(" "))
										{
											DescriptionIndent = CodeIndents.Space;
										}
									}
									else
									{
										tempii = TextModLines[i];
									}
									Header.ModDescription[defaultLanguage] += Newline + tempii.TrimStart(' ').TrimStart('\t').TrimEnd(' ');
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

									if (tempii.StartsWith("\t"))
									{
										ModFilesToEditIndent = CodeIndents.Tab;
									}
									else if (tempii.StartsWith("    "))
									{
										ModFilesToEditIndent = CodeIndents.RightAligned;
									}
									else if (tempii.StartsWith(" "))
									{
										ModFilesToEditIndent = CodeIndents.Space;
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

									if (tempii.StartsWith("\t"))
									{
										ModIncludedFilesIndent = CodeIndents.Tab;
									}
									else if (tempii.StartsWith("    "))
									{
										ModIncludedFilesIndent = CodeIndents.RightAligned;
									}
									else if (tempii.StartsWith(" "))
									{
										ModIncludedFilesIndent = CodeIndents.Space;
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
								Header.ModAuthorNotes = new StringLocalised(Regex.Replace(TextModLines[i], @"\#\# Author Note(s|)\:(\W|)", "", RegexOptions.IgnoreCase));
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
									string tempii = null;
									if (TextModLines[i].StartsWith("##"))
									{
										tempii = TextModLines[i].Substring(2, TextModLines[i].Length - 2);
									}
									else
									{
										tempii = TextModLines[i];
									}

									if (tempii.StartsWith("\t"))
									{
										AuthorNotesIndent = CodeIndents.Tab;
									}
									else if (tempii.StartsWith("    "))
									{
										AuthorNotesIndent = CodeIndents.RightAligned;
									}
									else if (tempii.StartsWith(" "))
									{
										AuthorNotesIndent = CodeIndents.Space;
									}

									Header.ModAuthorNotes[defaultLanguage] += Newline + tempii.TrimStart(' ').TrimStart('\t').TrimEnd(' ');
								}
							}
							break;
						case 12:
							if (TextModLines[i].ToUpper().StartsWith("## MOD HISTORY")) 
							{
								InMultiLineElement = true;
								if (Header.ModHistory == null) Header.ModHistory = new ModHistory();
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

										Header.ModHistory.Add(new ModHistoryEntry(HVersion,System.DateTime.Parse(Regex.Match(TextModLines[i], "([0-9]+)(\\/|\\\\|\\-)([0-9]+)(\\/|\\\\|\\-)([0-9]+)").Value),""));

										int UB = Header.ModHistory.Count - 1;
										Header.ModHistory[UB].HistoryChangeLog = new ModHistoryChangeLogLocalised();
										Header.ModHistory[UB].HistoryChangeLog.Add(new ModHistoryChangeLog(), defaultLanguage);
									} 
									else 
									{
										if (!((TextModLines[i] == "##" || TextModLines[i] == "## "))) 
										{
											int UB = Header.ModHistory.Count - 1;
											int UB2 = Header.ModHistory[UB].HistoryChangeLog.Count - 1;
											string nextLine = Regex.Replace(TextModLines[i], @"##([\s]*)", "");
											if (nextLine.StartsWith("-"))
											{
												Header.ModHistory[UB].HistoryChangeLog[defaultLanguage].Add(nextLine.Substring(1));
											}
											else
											{
												Header.ModHistory[UB].HistoryChangeLog[defaultLanguage][UB2] += Newline + nextLine;
											}
										}
									}
								}
							}
							break;
					} // Switch
				} // For i
			} // For j
			Header.ModphpBBVersion = new ModVersion(2, 0, 0);
			if (Header.ModAuthorNotes[defaultLanguage].StartsWith("\r\n") || Header.ModAuthorNotes[defaultLanguage].StartsWith("\n"))
			{
				if (Header.ModAuthorNotes[defaultLanguage].StartsWith("\r\n"))
				{
					Header.ModAuthorNotes[defaultLanguage] = Header.ModAuthorNotes[defaultLanguage].Remove(0,2);
				}
				else if (Header.ModAuthorNotes[defaultLanguage].StartsWith("\n"))
				{
					Header.ModAuthorNotes[defaultLanguage] = Header.ModAuthorNotes[defaultLanguage].Remove(0,1);
				}
				//char[] trimChars = {'\r','\n'};
				//Header.ModAuthorNotes[defaultLanguage].TrimStart(trimChars);
				AuthorNotesStartLine = StartLine.Next;
			}
			else
			{
				AuthorNotesStartLine = StartLine.Same;
			}
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
						ModAction ma = new ModAction(ThisMODActionType, ThisMODActionBody, NextMODActionComm, ThisMODActionComm, IntFirstMALine);
						if (ma.ActionType == "DIY INSTRUCTIONS")
						{
							ma.Modifier = defaultLanguage;
						}
						Actions.Add(ma);
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
						// match to "#---[ ABC ]---   " an arbitrary amoung of end of line space and dashes
						// no end of line space is preffered as per the specs
						ThisMODActionType = Regex.Replace(ModTextLines[i], @"^#([\-]+)\[(\W)([A-Za-z, \/\-\\]+?) \]([\-]+)([ ]*)$", "$3", RegexOptions.IgnoreCase);
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

			//
			// MOD Author parsing
			//
			for (int i = 0; i < XmlDataSet._author_group.Rows.Count; i++)
			{
				for (int j = 0; j < XmlDataSet.author.Rows.Count; j++)
				{
					if ((int)XmlDataSet.author.Rows[j]["author-group_Id"] == (int)XmlDataSet._author_group.Rows[i]["author-group_Id"])
					{
						ModAuthorEntry tempAuthor = new ModAuthorEntry(XmlDataSet.author.Rows[j]["username"].ToString(), 
							XmlDataSet.author.Rows[j]["realname"].ToString(),
							XmlDataSet.author.Rows[j]["email"].ToString(),
							XmlDataSet.author.Rows[j]["homepage"].ToString());
						// TODO: contributions
						Header.ModAuthor.Add(tempAuthor);
					}
				}
			}

			//
			// Multilingual MOD Title
			//
			Header.ModTitle = new StringLocalised();
			for (int i = 0; i < XmlDataSet.title.Rows.Count; i++)
			{
				Header.ModTitle[XmlDataSet.title.Rows[i]["lang"].ToString()] = XmlDataSet.title.Rows[i]["title_Text"].ToString();
			}

			//
			// MOD Description
			//
			Header.ModDescription = new StringLocalised();
			for (int i = 0; i < XmlDataSet.description.Rows.Count; i++)
			{
				Header.ModDescription[XmlDataSet.description.Rows[i]["lang"].ToString()] = XmlDataSet.description.Rows[i]["description_Text"].ToString();
			}

			//
			// Author Notes
			//
			Header.ModAuthorNotes = new StringLocalised();
			for (int i = 0; i < XmlDataSet._author_notes.Rows.Count; i++)
			{
				Header.ModAuthorNotes[XmlDataSet._author_notes.Rows[i]["lang"].ToString()] = XmlDataSet._author_notes.Rows[i]["author-notes_Text"].ToString();
			}

			//
			// MOD History entries
			//
			Header.ModHistory = new ModHistory();
			for (int i = 0; i < XmlDataSet.entry.Rows.Count; i++)
			{
				Header.ModHistory.Add(new ModHistoryEntry());
				for (int j = 0; j < XmlDataSet._rev_version.Rows.Count; j++)
				{
					if ((int)XmlDataSet._rev_version.Rows[j]["entry_Id"] == (int)XmlDataSet.entry.Rows[i]["entry_Id"])
					{
						ModVersion tempVersion;
						tempVersion = new ModVersion(
							(UInt16)XmlDataSet._rev_version.Rows[j]["major"],
							(UInt16)XmlDataSet._rev_version.Rows[j]["minor"], 
							(UInt16)XmlDataSet._rev_version.Rows[j]["revision"]);
						if (XmlDataSet._rev_version.Rows[j]["release"].ToString().ToCharArray().Length == 1)
						{
							tempVersion.VersionRelease = XmlDataSet._rev_version.Rows[j]["release"].ToString().ToCharArray()[0];
						}
						Header.ModHistory[i].HistoryVersion = tempVersion;
					}
				}
				Header.ModHistory[i].HistoryDate = DateTime.Parse(XmlDataSet.entry.Rows[i]["date"].ToString());
				Header.ModHistory[i].HistoryChangeLog = new ModHistoryChangeLogLocalised();
				for (int j = 0; j < XmlDataSet.changelog.Rows.Count; j++)
				{
					if ((int)XmlDataSet.changelog.Rows[j]["entry_Id"] == (int)XmlDataSet.entry.Rows[i]["entry_Id"])
					{
						string language = (string)XmlDataSet.changelog.Rows[j]["lang"];
						Header.ModHistory[i].HistoryChangeLog.Add(new ModHistoryChangeLog(), language);
						for (int k = 0; k < XmlDataSet.change.Rows.Count; k++)
						{
							if ((int)XmlDataSet.change.Rows[k]["changelog_Id"] == (int)XmlDataSet.changelog.Rows[j]["changelog_Id"])
							{
								Header.ModHistory[i].HistoryChangeLog[language].Add((string)XmlDataSet.change.Rows[k]["change_Text"]);
							}
						}
					}
				}
			}

			//
			// meta
			//
			Header.Meta = new Hashtable();
			for (int i = 0; i < XmlDataSet.meta.Rows.Count; i++)
			{
				Header.Meta.Add(XmlDataSet.meta.Rows[i]["name"],XmlDataSet.meta.Rows[i]["content"]);
			}

			//
			// MOD Version
			//
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

			//
			// license
			//
			try { Header.License = (string)XmlDataSet.header.Rows[0]["license"]; }
			catch {}

			//
			// installation
			//
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
				Actions.Add(new ModAction("SQL", XmlDataSet.sql.Rows[i]["sql_Text"].ToString(), ""));
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
				Actions.Add(new ModAction("COPY", tempCopy.ToString(), ""));
			}

			// open
			// this is very specific, the FINDs always go BEFORE the actions
			// followed by IN-LINE actions, and lastly comments
			for (int i = 0; i < XmlDataSet.open.Rows.Count; i++)
			{
				Actions.Add(new ModAction("OPEN", XmlDataSet.open.Rows[i]["src"].ToString(), ""));
				for (int j = 0; j < XmlDataSet.edit.Rows.Count; j++)
				{
					if ((int)XmlDataSet.edit.Rows[j]["open_Id"] == (int)XmlDataSet.open.Rows[i]["open_Id"])
					{
						for (int k = 0; k < XmlDataSet.find.Rows.Count; k++)
						{
							if ((int)XmlDataSet.find.Rows[k]["edit_Id"] == (int)XmlDataSet.edit.Rows[j]["edit_Id"])
							{
								Actions.Add(new ModAction("FIND", XmlDataSet.find.Rows[k]["find_Text"].ToString(), "", XmlDataSet.find.Rows[k]["type"].ToString()));
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
										Actions.Add(new ModAction(actionTitle, XmlDataSet._inline_action.Rows[l]["inline-action_Text"].ToString(), ""));
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
				Actions.Add(new ModAction("DIY INSTRUCTIONS", XmlDataSet._diy_instructions.Rows[i]["diy-instructions_Text"].ToString(),"", XmlDataSet._diy_instructions.Rows[i]["lang"].ToString()));
			}

			Actions.Add(new ModAction("SAVE/CLOSE ALL FILES", "", "EoM"));
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

			// lets force these events to sync the MOD,
			UpdateFilesToEdit();
			UpdateIncludedFiles();
			UpdateInstallationTime();

			string BlankTemplate = TextTemplate;
			System.Text.StringBuilder NewModBody = new System.Text.StringBuilder();
			BlankTemplate = BlankTemplate.Replace("<mod.title/>", Header.ModTitle.GetValue());
			string MyMODAuthorS = null;
			for (int i = 0; i < Header.ModAuthor.Authors.Count; i++)
			{
				if (i == 0) 
				{
					MyMODAuthorS = Header.ModAuthor[i].ToString();
				} 
				else 
				{
					MyMODAuthorS += Newline + "## MOD Author: " + Header.ModAuthor[i].ToString();
				}
			}

			string DescriptionStartOfLine = "## ";
			switch (DescriptionIndent)
			{
				case CodeIndents.Space:
					// already assigned as "## ";
					break;
				case CodeIndents.Tab:
					DescriptionStartOfLine = "##\t";
					break;
				case CodeIndents.RightAligned:
					DescriptionStartOfLine = "##                  ";
					break;
			}

			BlankTemplate = BlankTemplate.Replace("<mod.author/>", MyMODAuthorS);
			BlankTemplate = BlankTemplate.Replace("<mod.description/>", Header.ModDescription.GetValue().Replace(Newline.ToString(), Newline.ToString() + DescriptionStartOfLine));
			BlankTemplate = BlankTemplate.Replace("<mod.version/>", Header.ModVersion.ToString());
			BlankTemplate = BlankTemplate.Replace("<mod.install_level/>", Header.ModInstallationLevel.ToString());
			BlankTemplate = BlankTemplate.Replace("<mod.install_time/>", Math.Ceiling(Header.ModInstallationTime / 60) + " minutes");

			string MyMODFTE = null;
			string ModFilesToEditStartOfLine = "## ";
			switch (ModFilesToEditIndent)
			{
				case CodeIndents.Space:
					// already assigned as "## ";
					break;
				case CodeIndents.Tab:
					ModFilesToEditStartOfLine = "##\t";
					break;
				case CodeIndents.RightAligned:
					ModFilesToEditStartOfLine = "##                ";
					break;
			}
			for (int i = 0; i < Header.ModFilesToEdit.Count; i++) 
			{
				if (i == 0) 
				{
					MyMODFTE = Header.ModFilesToEdit[i];
				} 
				else 
				{
					MyMODFTE += Newline + ModFilesToEditStartOfLine + Header.ModFilesToEdit[i];
				}
			}
			BlankTemplate = BlankTemplate.Replace("<mod.files_to_edit/>", MyMODFTE);

			string MyMODIC = null;
			string ModIncludedFilesStartOfLine = "## ";
			switch (ModIncludedFilesIndent)
			{
				case CodeIndents.Space:
					// already assigned as "## ";
					break;
				case CodeIndents.Tab:
					ModIncludedFilesStartOfLine = "##\t";
					break;
				case CodeIndents.RightAligned:
					ModIncludedFilesStartOfLine = "##                 ";
					break;
			}
			for (int i = 0; i < Header.ModIncludedFiles.Count; i++) 
			{
				if (i == 0) 
				{
					MyMODIC = Header.ModIncludedFiles[i];
				} 
				else 
				{
					MyMODIC += Newline + ModIncludedFilesStartOfLine + Header.ModIncludedFiles[i];
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

			string AuthorNotesStartOfLine = "## ";
			switch (AuthorNotesIndent)
			{
				case CodeIndents.Space:
					// already assigned as "## ";
					break;
				case CodeIndents.Tab:
					AuthorNotesStartOfLine = "##\t";
					break;
				case CodeIndents.RightAligned:
					AuthorNotesStartOfLine = "##               ";
					break;
			}
			string AuthorStartLine = "";
			if (AuthorNotesStartLine == StartLine.Next) AuthorStartLine = Newline.ToString();
			BlankTemplate = BlankTemplate.Replace("<mod.author_notes/>", ((String)(AuthorStartLine + Header.ModAuthorNotes.GetValue())).Replace(Newline.ToString(), Newline.ToString() + AuthorNotesStartOfLine));
			string MyMODHistory;
			System.Text.StringBuilder NewMyMODHistory = new System.Text.StringBuilder();
			if (Header.ModHistory.Count > 0)
			{
				NewMyMODHistory.Append("##############################################################" + Newline);
				NewMyMODHistory.Append("## MOD History:"  + Newline);
				foreach (ModHistoryEntry mhe in Header.ModHistory)
				{
					NewMyMODHistory.Append("## " + Newline);
					NewMyMODHistory.Append("## " + mhe.HistoryDate.ToString("yyyy-MM-dd") + " - Version " + mhe.HistoryVersion.ToString() + Newline);
					foreach (DictionaryEntry de in mhe.HistoryChangeLog)
					{
						string Language = (string)de.Key;
						ModHistoryChangeLog mhcl = (ModHistoryChangeLog)de.Value;
						if ((string)Language == defaultLanguage)
						{
							foreach (string le in mhcl)
							{
								NewMyMODHistory.Append("## -" + le.Replace(Newline.ToString(), "## " + Newline) + Newline);
							}
						}
					}
					if (mhe.HistoryChangeLog.Count == 0)
					{
						NewMyMODHistory.Append("## - " + Newline);
					}
				}
			}
			MyMODHistory = NewMyMODHistory.ToString();
			if (!(MyMODHistory.Length == 0)) 
			{
				MyMODHistory = Newline + MyMODHistory;
				MyMODHistory += "## ";
			}
			BlankTemplate = BlankTemplate.Replace("<mod.history/>", MyMODHistory);
			BlankTemplate = Regex.Replace(BlankTemplate, @"^#(.*?)([\s\S]*?)\n###END OF HEADER###\n", "", RegexOptions.Compiled);
			// TODO: re-add EMC
			/*if (Header.ModEasymodCompatibility.VersionMinor > 0 || Header.ModEasymodCompatibility.VersionRevision > 0) 
			{
				BlankTemplate = "## EasyMod " + Header.ModEasymodCompatibility.ToString() + " compliant" + Newline + BlankTemplate;
			}*/
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

			//
			// Title
			//
			foreach (string Language in Header.ModTitle)
			{
				mod.titleRow newRow = XmlDataSet.title.NewtitleRow();
				newRow.title_Text = Header.ModTitle[Language];
				newRow.lang = Language;
				XmlDataSet.title.Rows.Add(newRow);
			}

			//
			// Description
			//
			foreach (string Language in Header.ModDescription)
			{
				mod.descriptionRow newRow = XmlDataSet.description.NewdescriptionRow();
				newRow.description_Text = Header.ModTitle[Language];
				newRow.lang = Language;
				XmlDataSet.description.Rows.Add(newRow);
			}

			//
			// Author Notes
			//
			foreach (string Language in Header.ModAuthorNotes)
			{
				mod._author_notesRow newRow = XmlDataSet._author_notes.New_author_notesRow();
				newRow._author_notes_Text = Header.ModAuthorNotes[Language];
				newRow.lang = Language;
				XmlDataSet._author_notes.Rows.Add(newRow);
			}

			//
			// Authors
			//
			mod._author_groupRow authorGroupRow = XmlDataSet._author_group.New_author_groupRow();
			XmlDataSet._author_group.Rows.Add(authorGroupRow);
			foreach (ModAuthorEntry entry in Header.ModAuthor)
			{
				mod.authorRow newRow = XmlDataSet.author.NewauthorRow();
				newRow.email = entry.Email;
				newRow.homepage = entry.Homepage;
				newRow.realname = entry.RealName;
				newRow.username = entry.UserName;
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

			//
			// Version
			//
			mod._mod_versionRow versionRow = XmlDataSet._mod_version.New_mod_versionRow();
			versionRow.major = (ushort)Header.ModVersion.VersionMajor;
			versionRow.minor = (ushort)Header.ModVersion.VersionMinor;
			versionRow.revision = (ushort)Header.ModVersion.VersionRevision;
			if (Header.ModVersion.VersionRelease != ModVersion.nullChar)
			{
					versionRow.release = Header.ModVersion.VersionRelease.ToString();
			}
			XmlDataSet._mod_version.Rows.Add(versionRow);

			//
			// Installation
			//
			mod.installationRow installationRow = XmlDataSet.installation.NewinstallationRow();
			installationRow.level = Header.ModInstallationLevel.ToString();
			installationRow.time = (ulong)Header.ModInstallationTime;
			// TODO: target version, right now target version isn't supported by ModTemplateTools
			// full support to come, release is currently omitted, need full support!!!
			if (Header.ModphpBBVersion != null)
			{
				mod._target_versionRow targetVersionRow = XmlDataSet._target_version.New_target_versionRow();
				// major
				mod._target_majorRow targetMajorRow = XmlDataSet._target_major.New_target_majorRow();
				targetMajorRow.allow = "exact";
				targetMajorRow._target_major_Text = (ushort)Header.ModphpBBVersion.VersionMajor;
				targetMajorRow.SetParentRow(targetVersionRow);
				XmlDataSet._target_major.Rows.Add(targetMajorRow);
				// minor
				mod._target_minorRow targetMinorRow = XmlDataSet._target_minor.New_target_minorRow();
				targetMinorRow.allow = "exact";
				targetMinorRow._target_minor_Text = (ushort)Header.ModphpBBVersion.VersionMinor;
				targetMinorRow.SetParentRow(targetVersionRow);
				XmlDataSet._target_minor.Rows.Add(targetMinorRow);
				// release : TODO omitted
				targetVersionRow.SetParentRow(installationRow);
				XmlDataSet._target_version.Rows.Add(targetVersionRow);
			}
			XmlDataSet.installation.Rows.Add(installationRow);

			//
			// History
			//
			mod.historyRow historyRow = XmlDataSet.history.NewhistoryRow();
			if (Header.ModHistory.Count > 0)
			{
				XmlDataSet.history.Rows.Add(historyRow);
			}
			foreach (ModHistoryEntry mhe in Header.ModHistory)
			{
				mod.entryRow entryRow = XmlDataSet.entry.NewentryRow();
				entryRow.date = mhe.HistoryDate;
				// Version
				mod._rev_versionRow rev_versionRow = XmlDataSet._rev_version.New_rev_versionRow();
				rev_versionRow.major = (ushort)mhe.HistoryVersion.VersionMajor;
				rev_versionRow.minor = (ushort)mhe.HistoryVersion.VersionMinor;
				rev_versionRow.revision = (ushort)mhe.HistoryVersion.VersionRevision;
				if (mhe.HistoryVersion.VersionRelease != ModVersion.nullChar)
				{
					rev_versionRow.release = Header.ModVersion.VersionRelease.ToString();
				}
				rev_versionRow.SetParentRow(entryRow);
				XmlDataSet._rev_version.Rows.Add(rev_versionRow);
				// Changelogs
				foreach (DictionaryEntry de in mhe.HistoryChangeLog)
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

			//
			// meta
			//
			foreach (DictionaryEntry de in Header.Meta)
			{
				string key = (string)de.Key;
				string value = (string)de.Value;
				mod.metaRow newRow = XmlDataSet.meta.NewmetaRow();
				newRow.name = key;
				newRow.content = value;
				XmlDataSet.meta.Rows.Add(newRow);
			}

			//
			// actions
			//
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
				switch (ma.ActionType.ToUpper())
				{
					case "SQL":
						mod.sqlRow sqlRow = XmlDataSet.sql.NewsqlRow();
						sqlRow.sql_Text = ma.ActionBody;
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
						string[] lines = ma.ActionBody.Split('\n');
						foreach (string line in lines)
						{
							string from = "";
							string to = "";
							if (line.TrimStart(TrimChars).ToLower().StartsWith("copy"))
							{
								try
								{
									from = Regex.Match(line.Trim(TrimChars), "copy (.+) to", RegexOptions.IgnoreCase).Value.Replace("copy ", "").Replace(" to", "");
									to = Regex.Match(line.Trim(TrimChars), " to (.+)", RegexOptions.IgnoreCase).Value.Replace("copy ", "").Replace(" to ", "");

									mod.fileRow fileRow = XmlDataSet.file.NewfileRow();
									fileRow.from = from;
									fileRow.to = to;
									fileRow.SetParentRow(copyRow);
									XmlDataSet.file.Rows.Add(fileRow);
								}
								catch
								{
								}
							}
						}
						break;
					case "OPEN":
						currentOpenRow = XmlDataSet.open.NewopenRow();
						currentOpenRow.src = ma.ActionBody.Trim(TrimChars);
						currentOpenRow.SetParentRow(actiongroupRow);
						XmlDataSet.open.Rows.Add(currentOpenRow);
						break;
					case "FIND":
						if (lastActionType == "EDIT")
						{
							currentEditRow = XmlDataSet.edit.NeweditRow();
							currentEditRow.SetParentRow(currentOpenRow);
							XmlDataSet.edit.Rows.Add(currentEditRow);
						}
						mod.findRow findRow = XmlDataSet.find.NewfindRow();
						findRow.find_Text = ma.ActionBody;
						if (ma.Modifier != "")
						{
							findRow.type = ma.Modifier;
						}
						// TODO: needs work (merging)
						if (ma.AfterComment != "")
						{
							mod.commentRow commentRow = XmlDataSet.comment.NewcommentRow();
							commentRow.lang = defaultLanguage;
							commentRow.comment_Text = ma.AfterComment;
							commentRow.SetParentRow(currentEditRow);
							XmlDataSet.comment.Rows.Add(commentRow);
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
						switch (ma.ActionType.ToUpper())
						{
							case "AFTER, ADD":
								type = "after-add";
								break;
							case "BEFORE, ADD":
								type = "before-add";
								break;
							case "REPLACE WITH":
								type = "replace";
								break;
							case "INCREMENT":
								type = "operation";
								break;
						}
						mod.actionRow actionRow = XmlDataSet.action.NewactionRow();
						actionRow.action_Text = ma.ActionBody;
						actionRow.type = type;
						// TODO: comment
						actionRow.SetParentRow(currentEditRow);
						XmlDataSet.action.Rows.Add(actionRow);
						lastActionType = "EDIT";
						break;
					case "IN-LINE FIND":
						if (lastInLineActionType == "EDIT")
						{
							currentInLineEditRow = XmlDataSet._inline_edit.New_inline_editRow();
							currentInLineEditRow.SetParentRow(currentEditRow);
							XmlDataSet._inline_edit.Rows.Add(currentInLineEditRow);
						}
						mod._inline_findRow inLineFindRow = XmlDataSet._inline_find.New_inline_findRow();
						inLineFindRow._inline_find_Text = ma.ActionBody;
						if (ma.Modifier != "")
						{
							inLineFindRow.type = ma.Modifier;
						}
						inLineFindRow.SetParentRow(currentInLineEditRow);
						XmlDataSet._inline_find.Rows.Add(inLineFindRow);
						lastInLineActionType = "FIND";
						break;
					case "IN-LINE AFTER, ADD":
					case "IN-LINE BEFORE, ADD":
					case "IN-LINE REPLACE WITH":
					case "IN-LINE INCREMENT":
						type = "";
						switch (ma.ActionType.ToUpper())
						{
							case "IN-LINE AFTER, ADD":
								type = "after-add";
								break;
							case "IN-LINE BEFORE, ADD":
								type = "before-add";
								break;
							case "IN-LINE REPLACE WITH":
								type = "replace";
								break;
							case "IN-LINE INCREMENT":
								type = "operation";
								break;
						}
						mod._inline_actionRow inLineActionRow = XmlDataSet._inline_action.New_inline_actionRow();
						inLineActionRow._inline_action_Text = ma.ActionBody;
						inLineActionRow.type = type;
						// TODO: comment
						inLineActionRow.SetParentRow(currentInLineEditRow);
						XmlDataSet._inline_action.Rows.Add(inLineActionRow);
						lastInLineActionType = "EDIT";
						break;
					case "DIY INSTRUCTIONS":
						mod._diy_instructionsRow diyRow = XmlDataSet._diy_instructions.New_diy_instructionsRow();
						diyRow._diy_instructions_Text = ma.ActionBody;
						diyRow.lang = ma.Modifier;
						diyRow.SetParentRow(actiongroupRow);
						XmlDataSet._diy_instructions.Rows.Add(diyRow);
						break;
				}
			}

			return XmlDataSet.GetXml();
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
