/***************************************************************************
 *                                 TextMod.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: TextMod.cs,v 1.2 2007-07-23 11:17:39 smithydll Exp $
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
using Phpbb.ModTeam.Tools;
using Phpbb.ModTeam.Tools.DataStructures;

namespace Phpbb.ModTeam.Tools
{
	/// <summary>
	/// Summary description for TextMod.
	/// </summary>
	public class TextMod : Mod, IMod
	{
		private string TextTemplate;

		/// <summary>
		/// 
		/// </summary>
		public TextMod() : base()
		{
			textTemplateReadOnly = true;
		}

		/// <summary>
		/// Constructor, do not include trailing slash or file name, expecting
		/// a file MOD.mot to be present in the given directory
		/// </summary>
		/// <param name="templatePath">Path to the templates folder.</param>
		public TextMod(string templatePath) : base()
		{
			LoadTemplate(templatePath);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="templatePath"></param>
		public void LoadTemplate(string templatePath)
		{
			textTemplateReadOnly = false;
			TextTemplate = OpenTextFile(Path.Combine(templatePath, "MOD.mot"));
			try 
			{
				TextTemplate = Regex.Replace(TextTemplate, "^#(.*?)([\\s\\S]*?)(|\\r)\\n###END OF HEADER###(|\\r)\\n", "");
				TextTemplate = TextTemplate.Replace("\r", "");
			} 
			catch {}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			if (textTemplateReadOnly) throw new TextTemplateReadOnlyModeException("Class initated without specifying text template mot file to use. All Text Templates have been read in read only mode and cannot be saved back as a text template");

			// lets force these events to sync the MOD,
			UpdateFilesToEdit();
			UpdateIncludedFiles();
			UpdateInstallationTime();

			string blankTemplate = TextTemplate;
			System.Text.StringBuilder newModBody = new System.Text.StringBuilder();
			blankTemplate = blankTemplate.Replace("<mod.title/>", Header.Title.GetValue());
			string modAuthorsString = null;
			for (int i = 0; i < Header.Authors.Count;  i++)
			{
				if (i == 0) 
				{
					modAuthorsString = Header.Authors[i].ToString();
				} 
				else 
				{
					modAuthorsString += newline + "## MOD Author: " + Header.Authors[i].ToString();
				}
			}

			string descriptionStartOfLine = "## ";
			switch (DescriptionIndent)
			{
				case CodeIndents.Space:
					// already assigned as "## ";
					break;
				case CodeIndents.Tab:
					descriptionStartOfLine = "##\t";
					break;
				case CodeIndents.RightAligned:
					descriptionStartOfLine = "##                  ";
					break;
			}

			blankTemplate = blankTemplate.Replace("<mod.author/>", modAuthorsString);
			blankTemplate = blankTemplate.Replace("<mod.description/>", Header.Description.GetValue().Replace(newline.ToString(), newline.ToString() + descriptionStartOfLine));
			blankTemplate = blankTemplate.Replace("<mod.version/>", Header.Version.ToString());
			blankTemplate = blankTemplate.Replace("<mod.install_level/>", Header.InstallationLevel.ToString());
			blankTemplate = blankTemplate.Replace("<mod.install_time/>", Math.Ceiling(Header.InstallationTime / 60.0) + " minutes");

			string filesToEditString = "";
			string filesToEditStartOfLine = "## ";
			switch (ModFilesToEditIndent)
			{
				case CodeIndents.Space:
					// already assigned as "## ";
					break;
				case CodeIndents.Tab:
					filesToEditStartOfLine = "##\t";
					break;
				case CodeIndents.RightAligned:
					filesToEditStartOfLine = "##                ";
					break;
			}
			for (int i = 0; i < Header.FilesToEdit.Count; i++) 
			{
				if (i != 0) 
				{
					filesToEditString += newline + filesToEditStartOfLine;
				}
				filesToEditString += Header.FilesToEdit[i];
			}
			blankTemplate = blankTemplate.Replace("<mod.files_to_edit/>", filesToEditString);

			string includedFilesString = "";
			string includedFilesStartOfLine = "## ";
			switch (ModIncludedFilesIndent)
			{
				case CodeIndents.Space:
					// already assigned as "## ";
					break;
				case CodeIndents.Tab:
					includedFilesStartOfLine = "##\t";
					break;
				case CodeIndents.RightAligned:
					includedFilesStartOfLine = "##                 ";
					break;
			}
			for (int i = 0; i < Header.IncludedFiles.Count; i++) 
			{
				if (i != 0) 
				{
					includedFilesString += newline + includedFilesStartOfLine;
				}
				includedFilesString += Header.IncludedFiles[i];
			}
			blankTemplate = blankTemplate.Replace("<mod.inc_files/>", includedFilesString);
			blankTemplate = blankTemplate.Replace("<mod.generator/>", newline + "## Generator: Phpbb.ModTeam.Tools");
			if (Header.License != "")
			{
				blankTemplate = blankTemplate.Replace("<mod.license/>", newline + "## License: " + Header.License);
			}
			else
			{
				blankTemplate = blankTemplate.Replace("<mod.license/>", "");
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
			if (AuthorNotesStartLine == StartLine.Next) AuthorStartLine = newline.ToString();
			blankTemplate = blankTemplate.Replace("<mod.author_notes/>", ((String)(AuthorStartLine + Header.AuthorNotes.GetValue())).Replace(newline.ToString(), newline.ToString() + AuthorNotesStartOfLine));
			blankTemplate = blankTemplate.Replace("<mod.history/>", Header.History.ToString(defaultLanguage));
			blankTemplate = Regex.Replace(blankTemplate, @"^#(.*?)([\s\S]*?)\n###END OF HEADER###\n", "", RegexOptions.Compiled);

			newModBody.Append(blankTemplate);
			try 
			{
				newModBody.Append(Actions.ToString());
			} catch { }

			return newModBody.ToString();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		public override void Read(string fileName)
		{
			if (fileName.ToLower().EndsWith(".txt") || fileName.ToLower().EndsWith(".mod"))
			{
				string textFile = OpenTextFile(fileName);
				if (textFile.StartsWith("##")) 
				{
					ReadHeader(textFile);
					ReadActions(textFile);
				}
				else
				{
					throw new NotATextModException("Not a valid Text MOD");
				}
			}
			else
			{
				throw new Exception("Cannot read file of type given");
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="textMod"></param>
		public void ReadString(string textMod)
		{
			if (textMod.StartsWith("##")) 
			{
				ReadHeader(textMod);
				ReadActions(textMod);
			}
			else
			{
				throw new NotATextModException("Not a valid Text MOD");
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="TextMod"></param>
		public void ReadHeader(string TextMod)
		{
			double start = Environment.TickCount;
			TextMod = TextMod.Replace("\r\n", "\n");
			TextMod = TextMod.Replace("\r", "\n");
			string[] TextModLines = TextMod.Split(newline);
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
						i = TextModLines.Length - 1;
					}
					
					switch (e)
					{
						case 1:
							if (TextModLines[i].StartsWith("## EasyMod") || TextModLines[i].StartsWith("## EasyMOD") || TextModLines[i].StartsWith("## easymod")) 
							{
								try
								{
									ModVersion EMVersion = ModVersion.Parse((TextModLines[i].Replace("## EasyMod", "").Replace("compliant", "").Replace("compatible", "")));
									Header.EasyModCompatibility = EMVersion;
								}
								catch (NotAModVersionException)
								{
									ParseErrors = true;
								}
								StartOffset = i + 1;
								e++;
							}
							if (TextModLines[i].StartsWith("###")) 
							{
								StartOffset = i + 1;
								e++;
							}
							i = TextModLines.Length;
							break;
						case 2:
							if (TextModLines[i].ToUpper().StartsWith("## MOD TITLE")) 
							{
								Header.Title = new StringLocalised(Regex.Replace(TextModLines[i], "\\#\\# MOD Title\\:", "", RegexOptions.IgnoreCase).Trim(trimChars));
								StartOffset = i + 1;
								e++;
								i = TextModLines.Length;
							}
							break;
						case 3:
							if (TextModLines[i].ToUpper().StartsWith("## MOD AUTHOR")) 
							{
								Header.Authors.Add(ModAuthor.Parse(TextModLines[i]));
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
								Header.Description = new StringLocalised(Regex.Replace(TextModLines[i], "\\#\\# MOD Description\\:", "", RegexOptions.IgnoreCase).TrimStart(' ').TrimStart('\t').TrimEnd(' '));
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
									Header.Description[defaultLanguage] += newline + tempii.TrimStart(' ').TrimStart('\t').TrimEnd(' ');
								}
							}
							break;
						case 5:
							if (TextModLines[i].ToUpper().StartsWith("## MOD VERSION")) 
							{
								ModVersion MVersion = ModVersion.Parse(Regex.Replace(TextModLines[i], "\\#\\# MOD VERSION\\:([\\W]+?)([\\d]+?)\\.([\\d]+?)(\\.|)([\\d]+?|)([a-zA-Z]{0,1}?)", "$2.$3.$5$6", RegexOptions.IgnoreCase));
								Header.Version = MVersion;
								StartOffset = i + 1;
								e++;
								i = TextModLines.Length;
							}
							break;
						case 6:
							if (TextModLines[i].ToUpper().StartsWith("## INSTALLATION LEVEL")) 
							{
								Header.InstallationLevel = InstallationLevelParse(Regex.Replace(TextModLines[i], "\\#\\# Installation level\\:", "", RegexOptions.IgnoreCase).TrimStart(' ').TrimStart('\t').TrimEnd(' '));
								StartOffset = i + 1;
								e++;
								i = TextModLines.Length;
							}
							break;
						case 7:
							if (TextModLines[i].ToUpper().StartsWith("## INSTALLATION TIME")) 
							{
								try
								{
									Header.InstallationTime = StringToSeconds(Regex.Replace(TextModLines[i], "\\#\\# Installation Time\\:", "", RegexOptions.IgnoreCase).TrimStart(' ').TrimStart('\t').TrimEnd(' '));
								}
								catch (System.FormatException)
								{
									Header.InstallationTime = 0;
								}
								StartOffset = i + 1;
								e++;
								i = TextModLines.Length;
							}
							break;
						case 8:
							if (TextModLines[i].ToUpper().StartsWith("## FILES TO EDIT")) 
							{
								//Header.ModFilesToEdit = new string[1]; 
								Header.FilesToEdit.Add(Regex.Replace(TextModLines[i], "\\#\\# Files To Edit\\:", "", RegexOptions.IgnoreCase).TrimStart(' ').TrimStart('\t').TrimEnd(' '));
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

									Header.FilesToEdit.Add(tempii.TrimStart(' ').TrimStart('\t').TrimEnd(' '));
								}
							}
							break;
						case 9:
							if (TextModLines[i].ToUpper().StartsWith("## INCLUDED FILES")) 
							{
								//Header.ModIncludedFiles = new string[1];
								Header.IncludedFiles.Add(Regex.Replace(TextModLines[i], "\\#\\# Included Files\\:", "", RegexOptions.IgnoreCase).TrimStart(' ').TrimStart('\t').TrimEnd(' '));
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

									Header.IncludedFiles.Add(tempii.TrimStart(' ').TrimStart('\t').TrimEnd(' '));
								}
							}
							break;
						case 10:
							if (TextModLines[i].ToUpper().StartsWith("## LICENSE")) 
							{
								Header.License = Regex.Replace(TextModLines[i], "\\#\\# LICENSE\\:", "", RegexOptions.IgnoreCase).Trim(trimChars);
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
								Header.AuthorNotes = new StringLocalised(Regex.Replace(TextModLines[i], @"\#\# Author Note(s|)\:(\W|)", "", RegexOptions.IgnoreCase));
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

									Header.AuthorNotes[defaultLanguage] += newline + tempii.TrimStart(' ').TrimStart('\t').TrimEnd(' ');
								}
							}
							break;
						case 12:
							if (TextModLines[i].ToUpper().StartsWith("## MOD HISTORY")) 
							{
								InMultiLineElement = true;
								if (Header.History == null) Header.History = new ModHistory();
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
									if (Regex.IsMatch(TextModLines[i], @"((([0-9\-]+?)){3}) \- Version", RegexOptions.IgnoreCase)) 
									{
										//ModVersion HVersion = ModVersion.Parse(Regex.Match(TextModLines[i], "([\\d]+?)\\.([\\d]+?)(\\.|)([\\d]+?|)([a-zA-Z]{1}?|)([ \\t]|)$").Value);
										//Console.WriteLine(Regex.Replace(TextModLines[i], "^\\#\\#([0-9\\- \\t]+?)Version ([\\d]+?)\\.([\\d]+?)(\\.|)([\\d]+?|)([a-zA-Z]{0,1}?)([ \\t]|)$", "$2.$3.$5$6", RegexOptions.IgnoreCase));
										ModVersion HVersion;
										try
										{
											HVersion = ModVersion.Parse(Regex.Replace(TextModLines[i], "^\\#\\#([0-9\\- \\t]+?)Version ([\\d]+?)\\.([\\d]+?)(\\.|)([\\d]+?|)([a-zA-Z]{0,1}?)([ \\t]|)$", "$2.$3.$5$6", RegexOptions.IgnoreCase));
										}
										catch (NotAModVersionException)
										{
											ParseErrors = true;
											HVersion = new ModVersion();
										}

										Header.History.Add(new ModHistoryEntry(HVersion,System.DateTime.Parse(Regex.Match(TextModLines[i], "([0-9]+)(\\/|\\\\|\\-)([0-9]+)(\\/|\\\\|\\-)([0-9]+)").Value),""));

										int UB = Header.History.Count - 1;
										Header.History[UB].ChangeLog = new ModHistoryChangeLogLocalised();
										Header.History[UB].ChangeLog.Add(new ModHistoryChangeLog(), defaultLanguage);
									} 
									else 
									{
										if (!((TextModLines[i] == "##" || TextModLines[i] == "## "))) 
										{
											int UB = Header.History.Count - 1;
											// only match changes if we are in a history entry
											if (Header.History.Count > 0)
											{
												int UB2 = Header.History[UB].ChangeLog.Count - 1;
												
												string nextLine = Regex.Replace(TextModLines[i], @"##([\s]*)", "");
												if (nextLine.StartsWith("-"))
												{
													Header.History[UB].ChangeLog[defaultLanguage].Add(nextLine.Substring(1));
												}
												else
												{
													if (Header.History[UB].ChangeLog.Count > 0)
													{
														Header.History[UB].ChangeLog[defaultLanguage][UB2] += newline + nextLine;
													}
												}
											}
										}
									}
								}
							}
							break;
					} // Switch
				} // For i
			} // For j
			Header.PhpbbVersion = new TargetVersionCases(new ModVersion(2, 0, 0));
			if (Header.AuthorNotes[defaultLanguage].StartsWith("\r\n") || Header.AuthorNotes[defaultLanguage].StartsWith("\n"))
			{
				if (Header.AuthorNotes[defaultLanguage].StartsWith("\r\n"))
				{
					Header.AuthorNotes[defaultLanguage] = Header.AuthorNotes[defaultLanguage].Remove(0,2);
				}
				else if (Header.AuthorNotes[defaultLanguage].StartsWith("\n"))
				{
					Header.AuthorNotes[defaultLanguage] = Header.AuthorNotes[defaultLanguage].Remove(0,1);
				}
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
		public void ReadActions(string TextMod)
		{
			Actions = new ModActions();
			TextMod = TextMod.Replace("\r\n", "\n");
			TextMod = TextMod.Replace("\r", "\n");
			TextMod += "\n#\n#-----[";
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
					if (ModTextLines[i].StartsWith("#") && Regex.IsMatch(ModTextLines[i + 1], @"^\#([\-]+)\[") && FirstActionFound) 
					{
						NextMODActionComm = NextMODActionComm.TrimStart('\n');
						ThisMODActionComm = ThisMODActionComm.TrimStart('\n');

						switch(ThisMODActionType)
						{
							case "OPEN":
								ThisMODActionBody = ThisMODActionBody.Trim(trimChars);
								break;
						}
						ModAction ma = new ModAction(ThisMODActionType, ThisMODActionBody, NextMODActionComm, ThisMODActionComm, IntFirstMALine);
						if (ma.Type == "DIY INSTRUCTIONS")
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
								ThisMODActionComm += newline;
							}
							IsFirstTCLine = false;
							ThisMODActionComm += ModTextLines[i].TrimStart('#').TrimStart(' ');
						} 
						else 
						{
							if (!IsFirstMALine) 
							{
								ThisMODActionBody += newline;
							}
							IsFirstMALine = false;
							ThisMODActionBody += ModTextLines[i];
						}
					}
					if (Regex.IsMatch(ModTextLines[i], @"^\#([\-]+)\[")) 
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

		private void ReadActionLine()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		public override void Write(string fileName)
		{
			SaveTextFile(this.ToString(), fileName);
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
			Validation.Report Report = new Validation.Report();

			string textMod = OpenTextFile(fileName).Replace(WinNewLine, newline.ToString());
			string[] TextModLines = textMod.Split(newline);
			int HeaderEndLine = TextModLines.Length - 1;
			Validator.fillActions();

			int StartOffset = 0;

			int check;
			bool flag = true;

			bool actionValidate = false;
			bool warnFlag = false;
			bool validateFlag = true;
			bool validateWarnFlag = true;
			bool findINLineFlag = true;
			bool editINLineFlag = true;
			check = 0;
			flag = false;
			validateFlag = true;
			int row = 0;

			bool Found = false;
			for (int i = TextModLines.Length - 1; i != 0 && Found == false; i--) 
			{
				if (TextModLines[i].StartsWith("############################################################"))
				{
					Found = true;
					HeaderEndLine = i;
				}
			}

			int li;
			for (int j = 0; j < 14; j++) 
			{
				for (int i = StartOffset; i < HeaderEndLine; i++) 
				{
					row = i + 1;
					flag = false;
					if (StartOffset == i) 
					{
						if (TextModLines[i].StartsWith("## MOD Author"))
						{
							if (Regex.IsMatch(TextModLines[i], @"\# MOD Author(, Secondary|)")) 
							{
								if (!(Regex.IsMatch(TextModLines[i], @"\# MOD Author: ((?!n\/a)[\w\s\=\$\.\-\|@\'\:\[\]\(\)<> ]+?) <( |)(n\/a|[a-z0-9\(\) \.\-_\+\[\]@\|\*]+)( |)> (\((?! +)(([\w\s\.\'\-]+?)|n\/a)(?! +)\)|)( |)(([a-z]+?://){1}([a-z0-9\-\.,\?!%\*_\#:;~\\&$@\/=\+\(\)]+)|n\/a|)( |)$", RegexOptions.IgnoreCase))) 
								{
									li = i + 1;
									Report.HeaderReport += string.Format("{2} Incorrect MOD Author Syntax on line: {0}\n[code]{1}[/code]\n",
										li, TextModLines[i], Validator.fail);
									validateFlag = false;
								}
							}
						}
					}
					if (check == 0) 
					{
						if (Regex.IsMatch(TextModLines[i], "\\#\\# EasyMod (.+?) Compliant", RegexOptions.IgnoreCase)) 
						{
							Report.HeaderReport += string.Format("[i]## EasyMod Compliant[/i]\n",
								Validator.fail);
							Report.HeaderReport += "[i]EasyMod Compliant is not a ratified standard. It is not part of the MOD Template.[/i]\n";
							validateFlag = false;
						}
						check = 1;
						StartOffset = i + 1;
					}
					if (check == 1) 
					{
						if (TextModLines[i].StartsWith("## MOD Title:"))
						{
							flag = true;
							check = 2;
							StartOffset = i + 1;
						}
						if (!flag)
						{
							if (Regex.IsMatch(TextModLines[i], "Title", RegexOptions.IgnoreCase)) 
							{
								Report.HeaderReport += string.Format("{1} [i]MOD Title[/i], may fail with some tools, Line {0}\n",
									(i + 1).ToString(), Validator.warning);
								flag = true;
								warnFlag = true;
								validateWarnFlag = false;
								check = 2;
							}
							if (HeaderEndLine == row && warnFlag == false) 
							{
								Report.HeaderReport += string.Format("{0} Missing or incorrect [i]MOD Title[/i]\n",
									Validator.fail);
								flag = true;
								warnFlag = false;
								validateFlag = false;
								check = 2;
							}
						}
					}
					if (check == 2) 
					{
						if (TextModLines[i].StartsWith("## MOD Author:"))
						{
							flag = true;
							check = 3;
							StartOffset = i + 1;
							if (Regex.IsMatch(TextModLines[i + 1], "(MOD |)Author", RegexOptions.IgnoreCase)) 
							{
								check = 2;
							}
						}
						if (!flag)
						{
							if (Regex.IsMatch(TextModLines[i], "Author", RegexOptions.IgnoreCase)) 
							{
								Report.HeaderReport += string.Format("{1} [i]MOD Author[/i], may fail with some tools, Line {0}\n",
									(i + 1), Validator.warning);
								flag = true;
								warnFlag = true;
								validateWarnFlag = false;
								check = 3;
								if (Regex.IsMatch(TextModLines[i + 1], "(MOD |)Author", RegexOptions.IgnoreCase)) 
								{
									check = 2;
								}
							}
							if (HeaderEndLine == row) 
							{
								Report.HeaderReport += string.Format("{0} Missing or incorrect [i]MOD Author[/i]\n",
									Validator.fail);
								flag = true;
								validateWarnFlag = false;
								check = 3;
								if (Regex.IsMatch(TextModLines[i + 1], "(MOD |)Author", RegexOptions.IgnoreCase)) 
								{
									check = 2;
								}
							}
						}
					}
					if (check == 3) 
					{
						if (TextModLines[i].StartsWith("## MOD Description:"))
						{
							flag = true;
							check = 4;
							StartOffset = i + 1;
						}
						if (!flag)
						{
							if (Regex.IsMatch(TextModLines[i], "Description", RegexOptions.IgnoreCase)) 
							{
								Report.HeaderReport += string.Format("{1} [i]MOD Description[/i], may fail with some tools, Line {0}\n", 
									(i + 1), Validator.warning);
								flag = true;
								warnFlag = true;
								validateWarnFlag = false;
								check = 4;
							}
							if (HeaderEndLine == row) 
							{
								Report.HeaderReport += string.Format("{0} Missing or incorrect [i]MOD Description[/i]\n",
									Validator.fail);
								flag = true;
								validateFlag = false;
								check = 4;
							}
						}
					}
					if (check == 4) 
					{
						if (TextModLines[i].StartsWith("## MOD Version:"))
						{
							flag = true;
							check = 5;
							StartOffset = i + 1;
						}
						if (!flag)
						{
							if (Regex.IsMatch(TextModLines[i], "\\#.Version", RegexOptions.IgnoreCase)) 
							{
								Report.HeaderReport += string.Format("{1} [i]MOD Version[/i], may fail with some tools, Line {0}\n",
									(i + 1), Validator.warning);
								flag = true;
								warnFlag = true;
								validateWarnFlag = false;
								check = 5;
							}
							if (HeaderEndLine == row) 
							{
								if (CheckAbove(TextModLines, "Version", i)) 
								{
									Report.HeaderReport += string.Format("{0} [i]MOD Version[/i] in wrong order, may fail with some tools\n",
										Validator.warning);
									flag = true;
									warnFlag = true;
									validateWarnFlag = false;
									check = 5;
								} 
								else 
								{
									Report.HeaderReport += string.Format("{0} Missing [i]MOD Version[/i]. This is a mandatory field for the MOD Template.\n",
										Validator.fail);
									flag = true;
									validateFlag = false;
									check = 5;
								}
							}
						}
					}
					if (check == 5) 
					{
						if (TextModLines[i].StartsWith("## Installation Level:"))
						{
							flag = true;
							check = 6;
							StartOffset = i + 1;
						}
						if (HeaderEndLine == row && flag == false) 
						{
							if (CheckAbove(TextModLines, "Installation Level", i)) 
							{
								Report.HeaderReport += string.Format("{0} [i]Installation Level[/i] in wrong order, may fail with some tools\n",
									Validator.warning);
								flag = true;
								warnFlag = true;
								validateWarnFlag = false;
								check = 6;
							} 
							else 
							{
								Report.HeaderReport += string.Format("{0} Missing [i]Installation Level[/i]. This is a mandatory field for the MOD Template.\n",
									Validator.fail);
								flag = true;
								validateFlag = false;
								check = 6;
							}
						}
					}
					if (check == 6) 
					{
						if (TextModLines[i].StartsWith("## Installation Time:"))
						{
							flag = true;
							check = 7;
							StartOffset = i + 1;
						}
						if (HeaderEndLine == row && flag == false) 
						{
							if (CheckAbove(TextModLines, "Installation Time", i)) 
							{
								Report.HeaderReport += string.Format("{0} [i]Installation Time[/i] in wrong order, may fail with some tools.\n",
									Validator.warning);
								flag = true;
								warnFlag = true;
								validateWarnFlag = false;
								check = 7;
							} 
							else 
							{
								Report.HeaderReport += string.Format("{0} Missing [i]Installation Time[/i]. This is a mandatory field for the MOD Template.\n",
									Validator.fail);
								flag = true;
								validateFlag = false;
								check = 7;
							}
						}
					}
					if (check == 7) 
					{
						if (TextModLines[i].StartsWith("## Files To Edit:"))
						{
							flag = true;
							check = 9;
							StartOffset = i + 1;
						}
						if (!flag)
						{
							if (Regex.IsMatch(TextModLines[i], "Files To Edit", RegexOptions.IgnoreCase)) 
							{
								Report.HeaderReport += string.Format("{0} [i]Files To Edit[/i] is in the wrong case which may cause it to fail in some tools.\n",
									Validator.warning);
								flag = true;
								validateWarnFlag = false;
								check = 8;
								StartOffset = i + 1;
							}
							if (HeaderEndLine == row) 
							{
								Report.HeaderReport += string.Format("{0} [i]Files To Edit[/i] is missing. This is no cause for concern.\n",
									Validator.warning);
								flag = true;
								validateWarnFlag = false;
								check = 8;
							}
						}
					}
					if (check == 8) 
					{
						if (TextModLines[i].StartsWith("## Included Files:"))
						{
							flag = true;
							check = 9;
							StartOffset = i + 1;
						}
						if (HeaderEndLine == row && flag == false) 
						{
							Report.HeaderReport += string.Format("{0} [i]Included Files[/i] is missing. This is no cause for concern.\n",
								Validator.warning);
							flag = true;
							validateWarnFlag = false;
							check = 9;
						}
					}
					if (check == 9)
					{
						if (TextModLines[i].StartsWith("## License:"))
						{
							if (Regex.IsMatch(TextModLines[i], "http://opensource.org/licenses/gpl-license.php GNU General Public License v2"))
							{
								Report.HeaderReport += string.Format("{0} [i]You are using the GNU GPL License[/i].\n",
									Validator.ok);
							}
							else if (Regex.IsMatch(TextModLines[i], "http://opensource.org/licenses/gpl-license.php GNU Public License v2"))
							{
								Report.HeaderReport += string.Format("{0} [i]You are using the GPL License, however the license statement in the MOD Template has been updated to include the word 'General', you should update accordingly[/i]. (16/08/2005)\n",
									Validator.warning);
								validateWarnFlag = false; // we will let them off with a warning, this time
							}
							else
							{
								Report.HeaderReport += string.Format("{0} [i]You are not using the GPL License[/i]. Please be aware that most MODs are automatically licensed under the GPL and you may be required to relicense your MOD in accordance with the terms of the GPL inherited from the core phpBB package.\n",
									Validator.notice);
							}
							flag = true;
							check = 10;
							StartOffset = i + 1;
						}
						if (HeaderEndLine == row && flag == false)
						{
							Report.HeaderReport += string.Format("{0} Missing or incorrect [i]License[/i] statement - A license statement is important for phpBB MODs. Please read the MOD docs for more information.\n",
								Validator.fail);
							flag = true;
							validateFlag = false;
							check = 10;
						}
					}
					if (check == 10) 
					{
						if (Regex.IsMatch(TextModLines[i], "For security purposes, please check: http://www.phpbb.com/mods/")
							&& Regex.IsMatch(TextModLines[i + 1], "for the latest version of this MOD. Although MODs are checked")
							&& Regex.IsMatch(TextModLines[i + 2], "before being allowed in the MODs Database there is no guarantee")
							&& Regex.IsMatch(TextModLines[i + 3], "that there are no security problems within the MOD. No support")
							&& Regex.IsMatch(TextModLines[i + 4], "will be given for MODs not found within the MODs Database which")
							&& Regex.IsMatch(TextModLines[i + 5], "can be found at http://www.phpbb.com/mods/"))
						{
							flag = true;
							check = 11;
							StartOffset = i + 1;
						}
						if (HeaderEndLine == row && flag == false) 
						{
							Report.HeaderReport += string.Format("{0} Missing or incorrect [i]Security Disclaimer[/i] - The disclaimer was recently updated 24 July 2005.\n",
								Validator.fail);
							flag = true;
							validateFlag = false;
							check = 11;
						}
					}
					if (check == 11) 
					{
						if (TextModLines[i].StartsWith("## Author Notes:"))
						{
							flag = true;
							check = 12;
							StartOffset = i + 1;
						}
						if (HeaderEndLine == row && flag == false) 
						{
							Report.HeaderReport += string.Format("{0} Missing or incorrect [i]Author Notes[/i]\n",
								Validator.fail);
							flag = true;
							validateFlag = false;
							check = 12;
						}
					}
					if (check == 12) 
					{
						if (TextModLines[i].StartsWith("## MOD History:"))
						{
							flag = true;
							check = 13;
							StartOffset = i + 1;
						}
						if (HeaderEndLine == row && flag == false) 
						{
							Report.HeaderReport += string.Format("{0} [color=green]not used [i]MOD History[/i][/color] - [u]This is not an error[/u]\n",
								Validator.ok);
							flag = true;
							check = 13;
						}
					}
					if (check == 13) 
					{
						if (Regex.IsMatch(TextModLines[i], "Before Adding This MOD To Your Forum, You Should Back Up All Files Related To This MOD")) 
						{
							flag = true;
							check = 14;
							StartOffset = i + 1;
						}
						if (HeaderEndLine == row && flag == false) 
						{
							Report.HeaderReport += string.Format("{0} Missing or incorrect [i]Install disclaimer[/i]\n",
								Validator.fail);
							flag = true;
							validateFlag = false;
							check = 14;
						}
					}
				}
				flag = false;
				warnFlag = false;
			}

			//
			// Make sure # EoM is used, but not in the header
			//
			flag = false;
			warnFlag = false;
			for (int i = HeaderEndLine; i < TextModLines.Length; i++)
			{
				row = i + 1;
				if (TextModLines[i].StartsWith("# EoM"))
				{
					flag = true;
				}
				if (!flag)
				{
					if (Regex.IsMatch(TextModLines[i], "\\#( |)EoM", RegexOptions.IgnoreCase)) 
					{
						Report.HeaderReport += string.Format("{0} [i]# EoM[/i] is the wrong syntax.\n",
							Validator.warning);
						flag = true;
						warnFlag = true;
						validateFlag = false;
						check = 15;
					}
					if (TextModLines.Length == row) 
					{
						Report.HeaderReport += string.Format("{0} Missing or incorrect [i]# EoM[/i]\n",
							Validator.fail);
						flag = true;
						validateFlag = false;
						check = 15;
					}
				}
			}

			for (int i = HeaderEndLine; i < TextModLines.Length; i++) 
			{
				// silly to check this if it is not an action definition
				if (TextModLines[i].IndexOf("--") >= 0 && TextModLines[i].StartsWith("#"))
				{
					if (Regex.IsMatch(TextModLines[i], " \\[ ([A-Za-z0-9, ]+?) \\]", RegexOptions.Compiled)) 
					{
						string db_info = Regex.Replace(TextModLines[i], "^\\#(\\-+?) \\[(.*?)$", "-$1^");
						int db_info_len = db_info.IndexOf(" [");
						db_info = "-";
						for (int j = 0; j <= db_info_len - 2; j++) 
						{
							db_info += "-";
						}
						db_info += "^";

						Report.HeaderReport += string.Format("{3} Mod actions [b]must not[/b] have spaces in action definition. line {0}\n[quote][b][color=red]-- [[/color] [color=green]--[[/color][/b]\n--^\n{1}\n{2}[/quote]\n", 
							(i + 1), TextModLines[i], db_info, Validator.fail);
						validateFlag = false;
					}
					if (Regex.IsMatch(TextModLines[i], "\\[ ([A-Za-z0-9, ]+?) \\] ", RegexOptions.Compiled)) 
					{
						string db_info = Regex.Replace(TextModLines[i], "^\\#(.*?)\\] -(.*?)$", "-$1-^");
						int db_info_len = db_info.IndexOf("] ");
						db_info = "-";
						for (int j = 0; j <= db_info_len - 3; j++) 
						{
							db_info += "-";
						}
						db_info += "^";

						Report.HeaderReport += string.Format("{3} Mod actions [b]must not[/b] have spaces in action definition. line {0}\n[quote][b][color=red]] --[/color] [color=green]]--[/color][/b]\n-^\n{1}\n{2}[/quote]\n", 
							(i + 1), TextModLines[i], db_info, Validator.fail);
						validateFlag = false;
					}
				}
			}

			TextMod modification = new TextMod();
			//try
			//{
			modification.ReadActions(textMod);
			//}
			//catch
			//{
			//}

			// Validate actions
			if (modification.Actions != null)
			{
				Validator.LoadPhpbbFileList(language, version); // Load the phpBB file list for comparison in the OPEN check
				for (int i = 0; i < modification.Actions.Count; i++) 
				{
					if (modification.Actions[i].Type != modification.Actions[i].Type.ToUpper())
					{
						Report.HeaderReport += string.Format("{2} Mod actions [b]must[/b] be in upper case. line {0}\n[code]{1}[/code]\n",
							modification.Actions[i].StartLine, modification.Actions[i].Body, Validator.fail);
						validateFlag = false;
					}
					if (Validation.ModActions.GetType(modification.Actions[i].Type) == Validation.ModActions.ModActionType.Edit ||
						Validation.ModActions.GetType(modification.Actions[i].Type) == Validation.ModActions.ModActionType.InLineEdit)
					{
						if (checks)
						{
							if (modification.Actions[i].Body.IndexOf("<font") >= 0)
							{
								if (Regex.IsMatch(modification.Actions[i].Body, "<font(.*?)>")) 
								{
									Report.HtmlReport += string.Format("{2} Unauthorised usage of the FONT tag. Please use the span tag, starting line: {0}\n[quote]{1}[/quote]\n",
										modification.Actions[i].StartLine, Regex.Replace(modification.Actions[i].ToString(), "<font(.*?)>", "[b]<font$1>[/b]"), Validator.fail);
									validateFlag = false;
								}
							}
							if (modification.Actions[i].Body.IndexOf("<br>") >= 0)
							{
								Report.HtmlReport += string.Format("Unauthorised usage of the <br> tag. Please use the <br /> tag., starting line: {0}\n[quote]{1}[/quote]\n",
									modification.Actions[i].StartLine, Regex.Replace(modification.Actions[i].ToString(), "<br>", "[b]<br>[/b]"));
								validateFlag = false;
							}
							if (modification.Actions[i].Body.IndexOf("<img") >= 0 &&
								modification.Actions[i].Body.IndexOf("/>") < 0)
							{
								Report.HtmlReport += string.Format("Unauthorised usage of the <img> tag. Please make sure you use XHTML entities e.g. <img />., starting line: {0}\n[quote]{1}[/quote]\n",
									modification.Actions[i].StartLine, Regex.Replace(modification.Actions[i].ToString(), "<img", "[b]<img[/b]"));
								validateFlag = false;
							}
							if (modification.Actions[i].Body.IndexOf("mysql_") >= 0)
							{
								if (modification.Actions[i].Body.IndexOf("mysql_connect") >= 0)
								{
									if (Regex.IsMatch(modification.Actions[i].Body, "mysql_connect\\((.*?)\\)")) 
									{
										Report.DbalReport += string.Format("Unauthorised usage of mysql_connect, please use the DBAL, starting line: {0}\n[quote]{1}[/quote]\n",
											modification.Actions[i].StartLine, Regex.Replace(modification.Actions[i].ToString(), "mysql_connect\\((.*?)\\)", "[b]mysql_connect($1)[/b]"));
										validateFlag = false;
									}
								}
								if (modification.Actions[i].Body.IndexOf("mysql_error") >= 0)
								{
									if (Regex.IsMatch(modification.Actions[i].Body, "mysql_error\\((.*?)\\)")) 
									{
										Report.DbalReport += string.Format("Unauthorised usage of mysql_error, please use the DBAL, starting line: {0}\n[quote]{1}[/quote]\n",
											modification.Actions[i].StartLine, Regex.Replace(modification.Actions[i].ToString(), "mysql_error\\((.*?)\\)", "[b]mysql_error($1)[/b]"));
										validateFlag = false;
									}
								}
							}
						} // check
					}
					if (Validation.ModActions.GetType(modification.Actions[i].Type) == Validation.ModActions.ModActionType.File)
						//if (ModActions.Parse(((ModAction)Modification.Actions[i]).ActionType).Type == ModActions.ModActionType.File) 
					{
						if (modification.Actions[i].Body.IndexOf(".phpex") >= 0)
							//if (Regex.IsMatch(((ModAction)Modification.Actions[i]).ActionBody, "\\.phpex")) 
						{
							Report.ActionsReport += string.Format("Unauthorised usage of path names (usage of .phpex rather than .php), starting line: {0}\n[quote]{1}[/quote]\n",
								modification.Actions[i].StartLine, Regex.Replace(modification.Actions[i].ToString(), "\\.phpex", "[b].phpex[/b]"));
							validateFlag = false;
						}
						if (Regex.IsMatch(modification.Actions[i].Body, "(\\/|)?php(BB|bb)(2|)\\/", RegexOptions.Compiled)) 
						{
							Report.ActionsReport += string.Format("Unauthorised usage of /phpBB2/, starting line: {0}\n[quote]{1}[/quote]\n",
								modification.Actions[i].StartLine, Regex.Replace(modification.Actions[i].ToString(), "(\\/|)?php(BB|bb)(2|)\\/", "[b]$1php$2$3/[/b]"));
							validateFlag = false;
						}
						if (modification.Actions[i].Type == "OPEN")
						{
							bool OpenFound = false;
							if (Validator.PhpbbFileList.Contains(modification.Actions[i].Body.Trim(trimChars)))
							{
								OpenFound = true;
							}
							if (!OpenFound) 
							{
								Report.ActionsReport += string.Format("File to OPEN does not exist in phpBB standard installation package, starting line: {0}\n[quote]{1}[/quote]\n",
									modification.Actions[i].StartLine, modification.Actions[i].ToString());
								validateFlag = false;
							}
						}
						if (modification.Actions[i].Type == "COPY")
						{
							bool CopyFlag = true;
							string[] CopyLines = modification.Actions[i].Body.Replace("\r\n", "\n").Split('\n');
							for (int j = 0; j < CopyLines.Length; j++) 
							{
								if (CopyLines[j].Length > 5) 
								{
									if (!Regex.IsMatch(CopyLines[j], "(copy|COPY) (.*?)\\.(.*?) (to|TO) (.*?)$", RegexOptions.Compiled)) 
									{
										CopyFlag = false;
									}
								}
							}
							if (!CopyFlag) 
							{
								Report.ActionsReport += string.Format("Unauthorised usage of COPY tag syntax, starting line: {0}\n[quote]{1}[/quote]\n",
									modification.Actions[i].StartLine, modification.Actions[i].ToString());
								validateFlag = false;
							}
						}
					}
					if (Validator.Actions.ContainsKey(modification.Actions[i].Type))
					{
						actionValidate = true;
					}
					if (modification.Actions[i].Type == "IN-LINE FIND")
					{
						if (!((modification.Actions[i - 1].Type == "FIND" || 
							Validation.ModActions.Parse(modification.Actions[i - 1].Type).Type == Validation.ModActions.ModActionType.InLineEdit || 
							Validation.ModActions.Parse(modification.Actions[i - 1].Type).Type == Validation.ModActions.ModActionType.Edit))) 
						{
							findINLineFlag = false;
						}
					}
					if (Validation.ModActions.Parse(modification.Actions[i].Type).Type == Validation.ModActions.ModActionType.Edit) // TODO: make sure this is correct #4
					{
						if (Validation.ModActions.Parse(modification.Actions[i - 1].Type).Type != Validation.ModActions.ModActionType.Edit) // TODO: #4
						{
							if (Validation.ModActions.Parse(modification.Actions[i - 1].Type).Type != Validation.ModActions.ModActionType.Find) // TODO: #2
							{
								editINLineFlag = false;
							}
						}
					}
					if (!actionValidate) 
					{
						Report.ActionsReport += string.Format("Unauthorised action, {0} please only use the official actions, starting line: {1}\n[quote]{2}[/quote]\n",
							modification.Actions[i].Type, modification.Actions[i].StartLine, modification.Actions[i].ToString());
						validateFlag = false;
					}
					if (!findINLineFlag) 
					{
						Report.ActionsReport += string.Format("Unauthorised action, {0}. This action must be preceded by a FIND, 'edit' or an IN-LINE 'edit' action, starting line: {1}\n[quote]{2}\n{3}[/quote]\n",
							modification.Actions[i].Type, modification.Actions[i].StartLine, modification.Actions[i - 1].ToString(), modification.Actions[i].ToString());
						validateFlag = false;
					}
					actionValidate = false;
					findINLineFlag = true;
					editINLineFlag = true;
				}
			}

			try
			{
				modification.ReadHeader(textMod);

				if (modification.Header != null)
				{
					int installTime = modification.Header.InstallationTime;
					modification.UpdateInstallationTime();
					if (Math.Abs(installTime - modification.Header.InstallationTime) / installTime > 0.5)
					{
						Report.HeaderReport += string.Format("{2} Installation time of {0} minutes is more than 50% out of realistic expectation, expectation was {1} minutes\n",
							installTime / 60, Math.Round((float)modification.Header.InstallationTime / 60), Validator.warning);
						validateWarnFlag = false;
					}

					StringCollection filesToEdit = modification.Header.FilesToEdit;
					modification.UpdateFilesToEdit();
					if (!CompareStringCollection(filesToEdit, modification.Header.FilesToEdit))
					{
						Report.HeaderReport += string.Format("{0} Files To Edit in header does not equal files edited in MOD\n",
							Validator.warning);
						validateWarnFlag = false;
					}

					System.Collections.Specialized.StringCollection includedFiles = modification.Header.IncludedFiles;
					modification.UpdateFilesToEdit();
					if (!CompareStringCollection(includedFiles, modification.Header.IncludedFiles))
					{
						Report.HeaderReport += string.Format("{0} Included Files in header does not equal filed copied over in MOD\n",
							Validator.warning);
						validateWarnFlag = false;
					}
				}
			}
			catch
			{
				Report.HeaderReport += string.Format("{0} Couldn't parse MOD header.\n",
					Validator.error);
				validateFlag = false;
			}

			//string rating_report;
			if (!validateFlag) 
			{
				Report.Rating = "The MOD [b][color=red]failed[/color][/b] the MOD pre-validation process. Please review and fix your errors before submitting to the MOD DB.";
				Report.Passed = false;
			} 
			else 
			{
				Report.Rating = "The MOD [b][color=green]passed[/color][/b] the MOD pre-validation process, please check over for elements computers cannot detect.";
				Report.Passed = true;
			}
			if (!validateWarnFlag)
			{
				Report.Rating += "\nThere were some [b][color=orange]warnings[/color][/b] which should be looked at but aren't causes for denial. These warnings may cause your MOD to act in undetermined ways in tools other than EasyMod, and should be fixed for maximum compatibility.";
				Report.Passed = false;
			}
			if (validateFlag && validateWarnFlag && checks) 
			{
				Report.ActionsReport += "\n[color=green]No problems[/color] were detected in this MODs Template in accordance with the phpBB MOD Team guidelines.";
			}
			if (Report.HtmlReport == null  && checks) 
			{
				Report.HtmlReport = "[color=green]No problems[/color] were detected in this MODs use HTML elements in accordance with the phpBB2 coding standards.";
			}
			if (Report.DbalReport == null  && checks) 
			{
				Report.DbalReport = "[color=green]No problems[/color] were detected in this MODs use of databases [size=9](if used)[/size] in accordance with the phpBB2 coding standards.";
			}

			return Report;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sc1"></param>
		/// <param name="sc2"></param>
		/// <returns></returns>
		private bool CompareStringCollection(StringCollection sc1, StringCollection sc2)
		{
			char[] TrimChars = {' ', '\t', '\n', '\r', '\b'};
			// check to see if every element is in
			bool flag = true;
			foreach (string s in sc1)
			{
				if (s.Trim(TrimChars) != "")
				{
					if (sc2.Contains(s))
					{
						flag = true;
					}
					else
					{
						flag = false;
					}
				}
			}
			return flag;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="TextModLines"></param>
		/// <param name="needle"></param>
		/// <param name="line"></param>
		/// <returns></returns>
		private bool CheckAbove(string[] TextModLines, string needle, int line)
		{
			for (int i = 0; i < line; i++)
			{
				if (TextModLines[i].IndexOf("" + needle + "") >= 0)
				{
					return true;
				}
			}
			return false;
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
		public static explicit operator ModxMod(TextMod m)
		{
			ModxMod n = new ModxMod();
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

	/// <summary>
	/// 
	/// </summary>
	public class NotATextModException : Exception
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public NotATextModException(string message) : base(message)
		{
		}
	}
}
