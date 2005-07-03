/***************************************************************************
 *                              ModValidator.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: ModValidator.cs,v 1.3 2005-07-03 03:31:21 smithydll Exp $
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
using System.Text.RegularExpressions;

namespace ModTemplateTools
{
	/// <summary>
	/// Summary description for ModValidator.
	/// </summary>
	public class ModValidator
	{

		private const string DefaultLanguage = "en-GB";
		private const char Newline = '\n';
		private const string WinNewLine = "\r\n";

		private char[] TrimChars = {' ', '\t', '\n', '\r', '\b'};

		private string[] PhpbbFileList;
		private string TemplatePath;

		/// <summary>
		/// 
		/// </summary>
		public struct ModValidationReport
		{
			/// <summary>
			/// 
			/// </summary>
			public enum ModValidationReportFormat
			{
				/// <summary>
				/// 
				/// </summary>
				HTML,
				/// <summary>
				/// 
				/// </summary>
				BBcode
			}

			/// <summary>
			/// 
			/// </summary>
			public bool Passed;

			/// <summary>
			/// 
			/// </summary>
			public int Warnings;

			/// <summary>
			/// 
			/// </summary>
			public string HeaderReport;

			/// <summary>
			/// 
			/// </summary>
			public string ActionsReport;

			/// <summary>
			/// 
			/// </summary>
			public string HTMLReport;

			/// <summary>
			/// 
			/// </summary>
			public string PHPReport;

			/// <summary>
			/// 
			/// </summary>
			public string DBALReport;

			/// <summary>
			/// 
			/// </summary>
			public string Rating;

			/// <summary>
			/// 
			/// </summary>
			/// <returns>HTML Format</returns>
			public override string ToString()
			{
				string Report;

				Report = string.Format("[size=18]MOD Template usage[/size]\n\n{0}\n\n{1}\n\n\n" + 
					"[size=18]MOD HTML usage[/size]\n\n{2}\n\n\n" + 
					"[size=18]MOD DBAL usage[/size]\n\n{3}\n\n\n" + 
					"[size=22]Overall[/size]\n\n{4}\n\n",
					this.HeaderReport, this.ActionsReport, this.HTMLReport, this.DBALReport, this.Rating);

				return Report;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="format"></param>
			/// <returns></returns>
			public string ToString(ModValidationReportFormat format)
			{
				switch (format)
				{
					case ModValidationReportFormat.BBcode:
						return this.ToString();
					case ModValidationReportFormat.HTML:
						return BBcodeToHtml(this.ToString());
				}
				return "";
			}
			
		}

		/// <summary>
		/// 
		/// </summary>
		public struct ModActions
		{
			/// <summary>
			/// 
			/// </summary>
			public enum ModActionType
			{
				/// <summary>
				/// 
				/// </summary>
				Sql,
				/// <summary>
				/// 
				/// </summary>
				File,
				/// <summary>
				/// 
				/// </summary>
				Find,
				/// <summary>
				/// 
				/// </summary>
				Edit,
				/// <summary>
				/// 
				/// </summary>
				InLineFind,
				/// <summary>
				/// 
				/// </summary>
				InLineEdit,
				/// <summary>
				/// 
				/// </summary>
				Null
			}

			/// <summary>
			/// 
			/// </summary>
			public string Action;
			/// <summary>
			/// 
			/// </summary>
			public ModActionType Type;

			/// <summary>
			/// 
			/// </summary>
			/// <param name="Action"></param>
			/// <param name="Type"></param>
			public ModActions(string Action, ModActionType Type)
			{
				this.Action = Action;
				this.Type = Type;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="input"></param>
			/// <returns></returns>
			public static ModActions Parse(string input)
			{
				ModActions[] Actions = new ModActions[12];
				Actions[0] = new ModActions("COPY", ModActions.ModActionType.File);
				Actions[1] = new ModActions("OPEN", ModActions.ModActionType.File);
				Actions[2] = new ModActions("FIND", ModActions.ModActionType.Find);
				Actions[3] = new ModActions("REPLACE WITH", ModActions.ModActionType.Edit);
				Actions[4] = new ModActions("AFTER, ADD", ModActions.ModActionType.Edit);
				Actions[5] = new ModActions("BEFORE, ADD", ModActions.ModActionType.Edit);
				Actions[6] = new ModActions("IN-LINE FIND", ModActions.ModActionType.InLineFind);
				Actions[7] = new ModActions("IN-LINE REPLACE WITH", ModActions.ModActionType.InLineEdit);
				Actions[8] = new ModActions("IN-LINE AFTER, ADD", ModActions.ModActionType.InLineEdit);
				Actions[9] = new ModActions("IN-LINE BEFORE, ADD", ModActions.ModActionType.InLineEdit);
				Actions[10] = new ModActions("SAVE/CLOSE ALL FILES", ModActions.ModActionType.File);
				Actions[11] = new ModActions("SQL", ModActions.ModActionType.Sql);

				for (int i = 0; i < Actions.Length; i++)
				{
					if (input == Actions[i].Action)
					{
						return Actions[i];
					}
				}
				return new ModActions("", ModActions.ModActionType.Null);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="TemplatePath"></param>
		public ModValidator(string TemplatePath)
		{
			this.TemplatePath = TemplatePath;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="TextMod"></param>
		/// <returns></returns>
		public ModValidationReport ValidateTextMod(string TextMod)
		{
			ModValidationReport Report = new ModValidationReport();

			TextMod = TextMod.Replace(WinNewLine, Newline.ToString());
			string[] TextModLines = TextMod.Split(Newline);
			int HeaderEndLine = TextModLines.Length - 1;

			ModActions[] Actions = new ModActions[12];
			Actions[0] = new ModActions("COPY", ModActions.ModActionType.File);
			Actions[1] = new ModActions("OPEN", ModActions.ModActionType.File);
			Actions[2] = new ModActions("FIND", ModActions.ModActionType.Find);
			Actions[3] = new ModActions("REPLACE WITH", ModActions.ModActionType.Edit);
			Actions[4] = new ModActions("AFTER, ADD", ModActions.ModActionType.Edit);
			Actions[5] = new ModActions("BEFORE, ADD", ModActions.ModActionType.Edit);
			Actions[6] = new ModActions("IN-LINE FIND", ModActions.ModActionType.InLineFind);
			Actions[7] = new ModActions("IN-LINE REPLACE WITH", ModActions.ModActionType.InLineEdit);
			Actions[8] = new ModActions("IN-LINE AFTER, ADD", ModActions.ModActionType.InLineEdit);
			Actions[9] = new ModActions("IN-LINE BEFORE, ADD", ModActions.ModActionType.InLineEdit);
			Actions[10] = new ModActions("SAVE/CLOSE ALL FILES", ModActions.ModActionType.File);
			Actions[11] = new ModActions("SQL", ModActions.ModActionType.Sql);

			int StartOffset = 0;

			//int MODLineCount = 0;
			//int i;
			//int j;
			int check;
			bool flag = true;
			//int s = 0;

			bool ActionValidate = false;
			bool WarnFlag = false;
			bool ValidateFlag = true;
			bool ValidateWarnFlag = true;
			//bool ValidatorFlag = reportLayoutBBcode;
			bool findINLineFlag = true;
			bool editINLineFlag = true;
			//string outputMODT = "";
			//string outputHTML = "";
			//string outputDBAL = "";
			check = 0;
			flag = false;
			ValidateFlag = true;
			int row = 0;
			//int MODCodeCount = MODLines.GetUpperBound(0);
			//int MODCodeCounti = MODCodeCount;
			//int MODCodeCounth = 0;

			bool Found = false;
			for (int i = TextModLines.Length - 1; i != 0 && Found == false; i--) 
			{
				if (Regex.IsMatch(TextModLines[i], "^(\\#){60}") && Found == false) 
				{
					Found = true;
					HeaderEndLine = i;
				}
			}

			int li;
			for (int j = 0; j < 13; j++) 
			{
				for (int i = StartOffset; i < HeaderEndLine; i++) 
				{
					row = i + 1;
					flag = false;
					if (StartOffset == i) 
					{
						if (Regex.IsMatch(TextModLines[i], "\\# MOD Author(, Secondary|)")) 
						{
							if (!(Regex.IsMatch(TextModLines[i], "\\# MOD Author(, Secondary|): ((?!n\\/a)[\\w\\s\\.\\-\\[\\]]+?) <( |)(n\\/a|[a-z0-9\\(\\) \\.\\-_\\+\\[\\]@]+)( |)> (\\((([\\w\\s\\.\\'\\-]+?)|n\\/a)\\)|)( |)(([a-z]+?://){1}([a-z0-9\\-\\.,\\?!%\\*_\\#:;~\\\\&$@\\/=\\+\\(\\)]+)|n\\/a|)( |)$", RegexOptions.IgnoreCase))) 
							{
								li = i + 1;
								Report.HeaderReport += string.Format("Incorrect MOD Author Syntax on line: {0}\n[code]{1}[/code]\n", li, TextModLines[i]);
								ValidateFlag = false;
							}
						}
					}
					if (check == 0) 
					{
						if (Regex.IsMatch(TextModLines[i], "\\#\\# EasyMod (.+?) Compliant", RegexOptions.IgnoreCase)) 
						{
							Report.HeaderReport += "[i]## EasyMod Compliant[/i]";
						}
						check = 1;
						StartOffset = i + 1;
						// TODO: goto exitForStatement1; Check why do we even have this one
					}
					if (check == 1) 
					{
						if (Regex.IsMatch(TextModLines[i], "MOD Title")) 
						{
							flag = true;
							check = 2;
							StartOffset = i + 1;
						}
						if (Regex.IsMatch(TextModLines[i], "Title", RegexOptions.IgnoreCase) && flag == false) 
						{
							Report.HeaderReport += string.Format("[b][color=orange]Warning[/color]:[/b] [i]MOD Title[/i], may fail with some tools, Line {0}\n", (i + 1).ToString());
							flag = true;
							WarnFlag = true;
							ValidateWarnFlag = false;
							check = 2;
						}
						if (HeaderEndLine == row && flag == false & WarnFlag == false) 
						{
							Report.HeaderReport += "Missing or incorrect [i]MOD Title[/i]\n";
							flag = true;
							WarnFlag = false;
							ValidateFlag = false;
							check = 2;
						}
					}
					if (check == 2) 
					{
						if (Regex.IsMatch(TextModLines[i], "MOD Author")) 
						{
							flag = true;
							check = 3;
							StartOffset = i + 1;
							if (Regex.IsMatch(TextModLines[i + 1], "(MOD |)Author", RegexOptions.IgnoreCase)) 
							{
								check = 2;
							}
						}
						if (Regex.IsMatch(TextModLines[i], "Author", RegexOptions.IgnoreCase) && flag == false) 
						{
							Report.HeaderReport += string.Format("[b][color=orange]Warning[/color]:[/b] [i]MOD Author[/i], may fail with some tools, Line {0}\n", (i + 1));
							flag = true;
							WarnFlag = true;
							ValidateWarnFlag = false;
							check = 3;
							if (Regex.IsMatch(TextModLines[i + 1], "(MOD |)Author", RegexOptions.IgnoreCase)) 
							{
								check = 2;
							}
						}
						if (HeaderEndLine == row && flag == false) 
						{
							Report.HeaderReport += "Missing or incorrect [i]MOD Author[/i]\n";
							flag = true;
							ValidateWarnFlag = false;
							check = 3;
							if (Regex.IsMatch(TextModLines[i + 1], "(MOD |)Author", RegexOptions.IgnoreCase)) 
							{
								check = 2;
							}
						}
					}
					if (check == 3) 
					{
						if (Regex.IsMatch(TextModLines[i], "MOD Description")) 
						{
							flag = true;
							check = 4;
							StartOffset = i + 1;
						}
						if (Regex.IsMatch(TextModLines[i], "Description", RegexOptions.IgnoreCase) && flag == false) 
						{
							Report.HeaderReport += string.Format("[b][color=orange]Warning[/color]:[/b] [i]MOD Description[/i], may fail with some tools, Line {0}\n", (i + 1));
							flag = true;
							WarnFlag = true;
							ValidateWarnFlag = false;
							check = 4;
						}
						if (HeaderEndLine == row && flag == false) 
						{
							Report.HeaderReport += "Missing or incorrect [i]MOD Description[/i]\n";
							flag = true;
							ValidateFlag = false;
							check = 4;
						}
					}
					if (check == 4) 
					{
						if (Regex.IsMatch(TextModLines[i], "MOD Version")) 
						{
							flag = true;
							check = 5;
							StartOffset = i + 1;
						}
						if (Regex.IsMatch(TextModLines[i], "\\#.Version", RegexOptions.IgnoreCase) && flag == false) 
						{
							Report.HeaderReport += string.Format("[b][color=orange]Warning[/color]:[/b] [i]MOD Version[/i], may fail with some tools, Line {0}\n", (i + 1));
							flag = true;
							WarnFlag = true;
							ValidateWarnFlag = false;
							check = 5;
						}
						if (HeaderEndLine == row && flag == false) 
						{
							if (CheckAbove(TextModLines, "Version", i)) 
							{
								Report.HeaderReport += "[b][color=orange]Warning[/color]:[/b] [i]MOD Version[/i] in wrong order, may fail with some tools\n";
								flag = true;
								WarnFlag = true;
								ValidateWarnFlag = false;
								check = 5;
							} 
							else 
							{
								Report.HeaderReport += "Missing [i]MOD Version[/i]. This is a mandatory field for the MOD Template.\n";
								flag = true;
								ValidateFlag = false;
								check = 5;
							}
						}
					}
					if (check == 5) 
					{
						if (Regex.IsMatch(TextModLines[i], "Installation Level")) 
						{
							flag = true;
							check = 6;
							StartOffset = i + 1;
						}
						if (HeaderEndLine == row && flag == false) 
						{
							if (CheckAbove(TextModLines, "Installation Level", i)) 
							{
								Report.HeaderReport += "[b][color=orange]Warning[/color]:[/b] [i]Installation Level[/i] in wrong order, may fail with some tools\n";
								flag = true;
								WarnFlag = true;
								ValidateWarnFlag = false;
								check = 6;
							} 
							else 
							{
								Report.HeaderReport += "Missing [i]Installation Level[/i]. This is a mandatory field for the MOD Template.\n";
								flag = true;
								ValidateFlag = false;
								check = 6;
							}
						}
					}
					if (check == 6) 
					{
						if (Regex.IsMatch(TextModLines[i], "Installation Time")) 
						{
							flag = true;
							check = 7;
							StartOffset = i + 1;
						}
						if (HeaderEndLine == row && flag == false) 
						{
							if (CheckAbove(TextModLines, "Installation Time", i)) 
							{
								Report.HeaderReport += "[b][color=orange]Warning[/color]:[/b] [i]Installation Time[/i] in wrong order, may fail with some tools.\n";
								flag = true;
								WarnFlag = true;
								ValidateWarnFlag = false;
								check = 7;
							} 
							else 
							{
								Report.HeaderReport += "Missing [i]Installation Time[/i]. This is a mandatory field for the MOD Template.\n";
								flag = true;
								ValidateFlag = false;
								check = 7;
							}
						}
					}
					if (check == 7) 
					{
						if (Regex.IsMatch(TextModLines[i], "Files To Edit")) 
						{
							flag = true;
							check = 9;
							StartOffset = i + 1;
						}
						if (Regex.IsMatch(TextModLines[i], "Files To Edit", RegexOptions.IgnoreCase) && flag == false) 
						{
							Report.HeaderReport += "[b][color=orange]Warning[/color]:[/b] [i]Files To Edit[/i] is in the wrong case which may cause it to fail in some tools.\n";
							flag = true;
							ValidateWarnFlag = false;
							check = 8;
							StartOffset = i + 1;
						}
						if (HeaderEndLine == row && flag == false) 
						{
							Report.HeaderReport += "[b][color=orange]Warning[/color]:[/b] [i]Files To Edit[/i] is missing. This is no cause for concern.\n";
							flag = true;
							ValidateWarnFlag = false;
							check = 8;
						}
					}
					if (check == 8) 
					{
						if (Regex.IsMatch(TextModLines[i], "Included Files")) 
						{
							flag = true;
							check = 9;
							StartOffset = i + 1;
						}
						if (HeaderEndLine == row && flag == false) 
						{
							Report.HeaderReport += "[b][color=orange]Warning[/color]:[/b] [i]Included Files[/i] is missing. This is no cause for concern.\n";
							flag = true;
							ValidateWarnFlag = false;
							check = 9;
						}
					}
					if (check == 9) 
					{
						if (Regex.IsMatch(TextModLines[i], "For Security Purposes, Please Check: http://www.phpbb.com/mods/ for the")) 
						{
							flag = true;
							check = 10;
							StartOffset = i + 1;
						}
						if (HeaderEndLine == row && flag == false) 
						{
							Report.HeaderReport += "Missing or incorrect [i]Security Disclaimer[/i] - The disclaimer was recently updated 3/06/2003.\n";
							flag = true;
							ValidateFlag = false;
							check = 10;
						}
					}
					if (check == 10) 
					{
						if (Regex.IsMatch(TextModLines[i], "Author Notes")) 
						{
							flag = true;
							check = 11;
							StartOffset = i + 1;
						}
						if (HeaderEndLine == row && flag == false) 
						{
							Report.HeaderReport += "Missing or incorrect [i]Author Notes[/i]\n";
							flag = true;
							ValidateFlag = false;
							check = 11;
						}
					}
					if (check == 11) 
					{
						if (Regex.IsMatch(TextModLines[i], "MOD History")) 
						{
							flag = true;
							check = 12;
							StartOffset = i + 1;
						}
						if (HeaderEndLine == row & flag == false) 
						{
							Report.HeaderReport += "[color=green]not used [i]MOD History[/i][/color] - [u]This is not an error[/u]\n";
							flag = true;
							check = 12;
						}
					}
					if (check == 12) 
					{
						if (Regex.IsMatch(TextModLines[i], "Before Adding This MOD To Your Forum, You Should Back Up All Files Related To This MOD")) 
						{
							flag = true;
							check = 13;
							StartOffset = i + 1;
						}
						if (HeaderEndLine == row && flag == false) 
						{
							Report.HeaderReport += "Missing or incorrect [i]Install disclaimer[/i]\n";
							flag = true;
							ValidateFlag = false;
							check = 13;
						}
					}
				}
				flag = false;
				WarnFlag = false;
			}

			//
			// Make sure # EoM is used, but not in the header
			//
			flag = false;
			WarnFlag = false;
			for (int i = HeaderEndLine; i < TextModLines.Length; i++)
			{
				row = i + 1;
				if (Regex.IsMatch(TextModLines[i], "\\# EoM")) 
				{
					flag = true;
					//StartOffset = i + 1; // We don't need this here do we.
				}
				if (Regex.IsMatch(TextModLines[i], "\\#( |)EoM", RegexOptions.IgnoreCase) && flag == false) 
				{
					Report.HeaderReport += "[b][color=red]Warning[/color]:[/b] [i]# EoM[/i] is the wrong syntax.\n";
					flag = true;
					WarnFlag = true;
					ValidateFlag = false;
					check = 14;
				}
				if (TextModLines.Length == row && flag == false) 
				{
					Report.HeaderReport += "Missing or incorrect [i]# EoM[/i]\n";
					flag = true;
					ValidateFlag = false;
					check = 14;
				}
			}

			for (int i = HeaderEndLine; i < TextModLines.Length; i++) 
			{
				if (Regex.IsMatch(TextModLines[i], "(\\-| )\\[ ([a-z0-9, ]+?)\\](\\-| )")) 
				{
					Report.HeaderReport += string.Format("Mod actions [b]must[/b] be in upper case. line {0}\n[code]{1}[/code]\n", (i + 1), TextModLines[i]);
					ValidateFlag = false;
				}
				if (Regex.IsMatch(TextModLines[i], " \\[ ([A-Za-z0-9, ]+?) \\]")) 
				{
					string db_info = Regex.Replace(TextModLines[i], "^\\#(\\-+?) \\[(.*?)$", "-$1^");

					Report.HeaderReport += string.Format("Mod actions [b]must not[/b] have spaces in action definition. line {0}\n[quote][b][color=red]-- [[/color] [color=green]--[[/color][/b]\n--^\n{1}\n{2}[/quote]\n", (i + 1), TextModLines[i], db_info);
					ValidateFlag = false;
				}
				if (Regex.IsMatch(TextModLines[i], "\\[ ([A-Za-z0-9, ]+?) \\] ")) 
				{
					string db_info = Regex.Replace(TextModLines[i], "^\\#(.*?)\\] -(.*?)$", "-$1-^");
					int db_info_len = db_info.Length;
					db_info = "-";
					for (int j = 0; j <= db_info_len - 3; j++) 
					{
						db_info += "-";
					}
					db_info += "^";

					Report.HeaderReport += string.Format("Mod actions [b]must not[/b] have spaces in action definition. line {0}\n[quote][b][color=red]] --[/color] [color=green]]--[/color][/b]\n-^\n{1}\n{2}[/quote]\n", (i + 1), TextModLines[i], db_info);
					ValidateFlag = false;
				}
			}
			//ReadMODActions();
			PhpbbMod Modification = new PhpbbMod(TemplatePath);
			//try
			//{
				Modification.ReadTextActions(TextMod);
			//}
			//catch
			//{
			//}

			if (Modification.Actions.Actions != null)
			{
				LoadPhpbbFileList(); // Load the phpBB file list for comparison in the OPEN check
				for (int i = 0; i < Modification.Actions.Actions.Length; i++) 
				{
					if (ModActions.Parse(Modification.Actions.Actions[i].ActionType).Type == ModActions.ModActionType.Edit ||
						ModActions.Parse(Modification.Actions.Actions[i].ActionType).Type == ModActions.ModActionType.InLineEdit) 
					{
						if (Regex.IsMatch(Modification.Actions.Actions[i].ActionBody, "<font(.*?)>")) 
						{
							Report.HTMLReport += string.Format("Unauthorised usage of the FONT tag. Please use the span tag, starting line: {0}\n[quote]{1}[/quote]\n",
								Modification.Actions.Actions[i].StartLine, Regex.Replace(Modification.Actions.Actions[i].ToString(), "<font(.*?)>", "[b]<font$1>[/b]"));
							ValidateFlag = false;
						}
						if (Regex.IsMatch(Modification.Actions.Actions[i].ActionBody, "<br>")) 
						{
							Report.HTMLReport += string.Format("Unauthorised usage of the <br> tag. Please use the <br /> tag., starting line: {0}\n[quote]{1}[/quote]\n",
								Modification.Actions.Actions[i].StartLine, Regex.Replace(Modification.Actions.Actions[i].ToString(), "<br>", "[b]<br>[/b]"));
							ValidateFlag = false;
						}
						if (Regex.IsMatch(Modification.Actions.Actions[i].ActionBody, "mysql_connect\\((.*?)\\)")) 
						{
							Report.DBALReport += string.Format("Unauthorised usage of mysql_connect, please use the DBAL, starting line: {0}\n[quote]{1}[/quote]\n",
								Modification.Actions.Actions[i].StartLine, Regex.Replace(Modification.Actions.Actions[i].ToString(), "mysql_connect\\((.*?)\\)", "[b]mysql_connect($1)[/b]"));
							ValidateFlag = false;
						}
						if (Regex.IsMatch(Modification.Actions.Actions[i].ActionBody, "mysql_error\\((.*?)\\)")) 
						{
							Report.DBALReport += string.Format("Unauthorised usage of mysql_error, please use the DBAL, starting line: {0}\n[quote]{1}[/quote]\n",
								Modification.Actions.Actions[i].StartLine, Regex.Replace(Modification.Actions.Actions[i].ToString(), "mysql_error\\((.*?)\\)", "[b]mysql_error($1)[/b]"));
							ValidateFlag = false;
						}
					}
					if (ModActions.Parse(Modification.Actions.Actions[i].ActionType).Type == ModActions.ModActionType.File) 
					{
						if (Regex.IsMatch(Modification.Actions.Actions[i].ActionBody, "\\.phpex")) 
						{
							Report.ActionsReport += string.Format("Unauthorised usage of path names (usage of .phpex rather than .php), starting line: {0}\n[quote]{1}[/quote]\n",
								Modification.Actions.Actions[i].StartLine, Regex.Replace(Modification.Actions.Actions[i].ToString(), "\\.phpex", "[b].phpex[/b]"));
							ValidateFlag = false;
						}
						if (Regex.IsMatch(Modification.Actions.Actions[i].ActionBody, "(\\/|)?php(BB|bb)(2|)\\/")) 
						{
							Report.ActionsReport += string.Format("Unauthorised usage of /phpBB2/, starting line: {0}\n[quote]{1}[/quote]\n",
								Modification.Actions.Actions[i].StartLine, Regex.Replace(Modification.Actions.Actions[i].ToString(), "(\\/|)?php(BB|bb)(2|)\\/", "[b]$1php$2$3/[/b]"));
							ValidateFlag = false;
						}
						if (Modification.Actions.Actions[i].ActionType == "OPEN")
						{
							bool OpenFound = false;
							// TODO: speed up with a non linear search algorithm on a sorted list.
							for (int j = 0; j < PhpbbFileList.Length; j++) 
							{
								if (PhpbbFileList[j] == Modification.Actions.Actions[i].ActionBody.Trim(TrimChars)) 
								{
									OpenFound = true;
								}
							}
							if (!OpenFound) 
							{
								Report.ActionsReport += string.Format("File to OPEN does not exist in phpBB standard installation package, starting line: {0}\n[quote]{1}[/quote]\n",
									Modification.Actions.Actions[i].StartLine, Modification.Actions.Actions[i].ToString());
								ValidateFlag = false;
							}
						}
						if (Modification.Actions.Actions[i].ActionType == "COPY")
						{
							bool CopyFlag = true;
							string[] CopyLines = Modification.Actions.Actions[i].ActionBody.Replace("\r\n", "\n").Split('\n');
							for (int j = 0; j < CopyLines.Length; j++) 
							{
								if (CopyLines[j].Length > 5) 
								{
									if (!Regex.IsMatch(CopyLines[j], "(copy|COPY) (.*?)\\.(.*?) (to|TO) (.*?)$")) 
									{
										CopyFlag = false;
									}
								}
							}
							if (!CopyFlag) 
							{
								Report.ActionsReport += string.Format("Unauthorised usage of COPY tag syntax, starting line: {0}\n[quote]{1}[/quote]\n",
									Modification.Actions.Actions[i].StartLine, Modification.Actions.Actions[i].ToString());
								ValidateFlag = false;
							}
						}
					}
					// TODO: non linear search algorithm here as well, shouldn't make too much of a difference 
					// considering how small amoung of actions to sort
					for (int j = 0; j < Actions.Length; j++) 
					{
						if (Modification.Actions.Actions[i].ActionType == Actions[j].Action) 
						{
							ActionValidate = true;
						}
					}
					if (Modification.Actions.Actions[i].ActionType == "IN-LINE FIND") // TODO: make sure this is correct, should be #6
					{
						// TODO: make sure #2 is FIND
						if (!((Modification.Actions.Actions[i - 1].ActionType == "FIND" || 
							ModActions.Parse(Modification.Actions.Actions[i - 1].ActionType).Type == ModActions.ModActionType.InLineEdit || 
							ModActions.Parse(Modification.Actions.Actions[i - 1].ActionType).Type == ModActions.ModActionType.Edit))) 
						{
							findINLineFlag = false;
						}
					}
					if (ModActions.Parse(Modification.Actions.Actions[i].ActionType).Type == ModActions.ModActionType.Edit) // TODO: make sure this is correct #4
					{
						if (ModActions.Parse(Modification.Actions.Actions[i - 1].ActionType).Type != ModActions.ModActionType.Edit) // TODO: #4
						{
							if (ModActions.Parse(Modification.Actions.Actions[i - 1].ActionType).Type != ModActions.ModActionType.Find) // TODO: #2
							{
								editINLineFlag = false;
							}
						}
					}
					if (!ActionValidate) 
					{
						Report.ActionsReport += string.Format("Unauthorised action, {0} please only use the official actions, starting line: {1}\n[quote]{2}[/quote]\n",
							Modification.Actions.Actions[i].ActionType, Modification.Actions.Actions[i].StartLine, Modification.Actions.Actions[i].ToString());
						ValidateFlag = false;
					}
					if (!findINLineFlag) 
					{
						Report.ActionsReport += string.Format("Unauthorised action, {0}. This action must be preceded by a FIND, 'edit' or an IN-LINE 'edit' action, starting line: {1}\n[quote]{2}\n{3}[/quote]\n",
							Modification.Actions.Actions[i].ActionType, Modification.Actions.Actions[i].StartLine, Modification.Actions.Actions[i - 1].ToString(), Modification.Actions.Actions[i].ToString());
						ValidateFlag = false;
					}
					ActionValidate = false;
					findINLineFlag = true;
					editINLineFlag = true;
				}
			}

			//string rating_report;
			if (!ValidateFlag) 
			{
				Report.Rating = "The MOD [b][color=red]failed[/color][/b] the MOD pre-validation process. Please review and fix your errors before submitting to the MOD DB.";
				Report.Passed = false;
			} 
			else 
			{
				Report.Rating = "The MOD [b][color=green]passed[/color][/b] the MOD pre-validation process, please check over for elements computers cannot detect.";
				Report.Passed = true;
			}
			if (!ValidateWarnFlag)
			{
				Report.Rating += "\nThere were some [b][color=orange]warnings[/color][/b] which should be looked at but aren't causes for denial. These warnings may cause your MOD to act in undetermined ways in tools other than EasyMod, and should be fixed for maximum compatibility.";
				Report.Passed = false;
			}
			if (ValidateFlag && ValidateWarnFlag) 
			{
				Report.ActionsReport += "\n[color=green]No problems[/color] were detected in this MODs Template in accordance with the phpBB MOD Team guidelines.";
			}
			if (Report.HTMLReport == null) 
			{
				Report.HTMLReport = "[color=green]No problems[/color] were detected in this MODs use HTML elements in accordance with the phpBB2 coding standards.";
			}
			if (Report.DBALReport == null) 
			{
				Report.DBALReport = "[color=green]No problems[/color] were detected in this MODs use of databases [size=9](if used)[/size] in accordance with the phpBB2 coding standards.";
			}

			return Report;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="XmlMod"></param>
		/// <returns></returns>
		public ModValidationReport ValidateXmlMod(string XmlMod)
		{
			return new ModValidationReport();
		}

		/// <summary>
		/// <p>Convert the BBcode output to HTML. This is a really really basic conversion that
		/// doesn't check for matching tags with only the tags required for mod validation.</p>
		/// 
		/// <list type=""><item>[quote][/quote]</item>
		/// <item>[code][/code]</item>
		/// <item>[b][/b]</item>
		/// <item>[i][/i]</item>
		/// <item>[color=][/color]</item>
		/// <item>[size=][/size]</item></list>
		/// 
		/// <p>The output is XHTML coshure.</p>
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private static string BBcodeToHtml(string input)
		{
			input = input.Replace("&", "&amp;");
			input = input.Replace("<", "&lt;");
			input = input.Replace(">", "&gt;");
			input = input.Replace("\"", "&quot;");

			input = input.Replace("[b]", "<strong>");
			input = input.Replace("[/b]", "</strong>");

			input = input.Replace("[i]", "<em>");
			input = input.Replace("[/i]", "</em>");

			input = input.Replace("[code]","<pre><code>");
			input = input.Replace("[/code]","</code></pre>");

			input = input.Replace("[quote]","<blockquote>");
			input = input.Replace("[/quote]","</blockquote>");

			input = Regex.Replace(input, "\\[color\\=([A-Fa-f0-9]+?]\\]", "<span style=\"color: $0\">");
			input = input.Replace("[/color]","</span>");

			input = Regex.Replace(input, "\\[size\\=([0-9]+?]\\]", "<span style=\"font-size: $0pt\">");
			input = input.Replace("[/size]","</span>");

			input = input.Replace("\n", "<br />\n");

			return input;
		}

		/// <summary>
		/// 
		/// </summary>
		private void LoadPhpbbFileList()
		{
			PhpbbFileList = OpenTextFile(TemplatePath + "\\files.txt").Replace("\r\n", "\n").Split('\n');
			//Console.WriteLine(":" + PhpbbFileList.Length);
		}

		/// <summary>
		/// Open a text file
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		private static string OpenTextFile(string filename)
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
				if (Regex.IsMatch(TextModLines[i], "" + needle + "")) 
				{
					return true;
				}
			}
			return false;
		}
	}
}
