/***************************************************************************
 *                               Validation.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: Validation.cs,v 1.2 2007-07-23 11:17:40 smithydll Exp $
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
using m = Phpbb.ModTeam.Tools.Validation;
using Phpbb.ModTeam.Tools.DataStructures;

namespace Phpbb.ModTeam.Tools
{
	/// <summary>
	/// 
	/// </summary>
	public class Validator
	{
		/// <summary>
		/// 
		/// </summary>
		public const string notice = "[b][ [color=blue]NOTICE[/color] ][/b]";
		/// <summary>
		/// 
		/// </summary>
		public const string error = "[b][ [color=red]ERROR[/color] ][/b]";
		/// <summary>
		/// 
		/// </summary>
		public const string fail = "[b][ [color=red]FAIL[/color] ][/b]";
		/// <summary>
		/// 
		/// </summary>
		public const string warning = "[b][ [color=orange]WARNING[/color] ][/b]";
		/// <summary>
		/// 
		/// </summary>
		public const string ok = "[b][ [color=green]OK[/color] ][/b]";
		/// <summary>
		/// 
		/// </summary>
		public const string pass = "[b][ [color=green]PASS[/color] ][/b]";
		/// <summary>
		/// 
		/// </summary>
		public const string info = "[b][ [color=purple]INFO[/color] ][/b]";

		private static AppDomain domain = AppDomain.CreateDomain("MODXdomain");

		/// <summary>
		/// 
		/// </summary>
		public static StringCollection PhpbbFileList = new StringCollection();

		/// <summary>
		/// 
		/// </summary>
		public static System.Collections.Hashtable Actions = new System.Collections.Hashtable();

		/// <summary>
		/// 
		/// </summary>
		public static void fillActions()
		{
			if (Actions.Count == 0)
			{
				Actions.Add("COPY", m.ModActions.ModActionType.File);
				Actions.Add("OPEN", m.ModActions.ModActionType.File);
				Actions.Add("FIND", m.ModActions.ModActionType.Find);
				Actions.Add("REPLACE WITH", m.ModActions.ModActionType.Edit);
				Actions.Add("AFTER, ADD", m.ModActions.ModActionType.Edit);
				Actions.Add("BEFORE, ADD", m.ModActions.ModActionType.Edit);
				Actions.Add("IN-LINE FIND", m.ModActions.ModActionType.InLineFind);
				Actions.Add("IN-LINE REPLACE WITH", m.ModActions.ModActionType.InLineEdit);
				Actions.Add("IN-LINE AFTER, ADD", m.ModActions.ModActionType.InLineEdit);
				Actions.Add("IN-LINE BEFORE, ADD", m.ModActions.ModActionType.InLineEdit);
				Actions.Add("SAVE/CLOSE ALL FILES", m.ModActions.ModActionType.File);
				Actions.Add("SQL", m.ModActions.ModActionType.Sql);
				Actions.Add("INCREMENT", m.ModActions.ModActionType.Edit);
				Actions.Add("IN-LINE INCREMENT", m.ModActions.ModActionType.InLineEdit);
				Actions.Add("DIY INSTRUCTIONS", m.ModActions.ModActionType.Instruction);
			}
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
		public static string BbcodeToHtml(string input)
		{
			input = input.Replace("&", "&amp;");
			input = input.Replace("<", "&lt;");
			input = input.Replace(">", "&gt;");

			input = input.Replace("[b]", "<strong>");
			input = input.Replace("[/b]", "</strong>");

			input = input.Replace("[i]", "<em>");
			input = input.Replace("[/i]", "</em>");

			input = input.Replace("[u]", "<u>");
			input = input.Replace("[/u]", "</u>");

			input = input.Replace("[code]","<pre><code>");
			input = input.Replace("[/code]","</code></pre>");

			input = input.Replace("[quote]","<blockquote>");
			input = Regex.Replace(input, "\\[quote\\=\"(.+?)\"\\]", "<strong>$1</strong><blockquote>");
			input = input.Replace("[/quote]","</blockquote>");

			input = input.Replace("\"", "&quot;");

			input = Regex.Replace(input, "\\[color\\=(#([A-Fa-f0-9]+?)|green|orange|red|purple|blue|magenta)\\]", "<span style=\"color: $1\">");
			input = input.Replace("[/color]","</span>");

			input = Regex.Replace(input, "\\[size\\=([0-9]+?)\\]", "<span style=\"font-size: $1pt\">");
			input = input.Replace("[/size]","</span>");

			input = input.Replace("\n", "<br />\n");

			return input;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="language"></param>
		/// <param name="version"></param>
		public static void LoadPhpbbFileList(string language, ModVersion version)
		{
			PhpbbFileList.Clear();
			if (version.Major == 2 && version.Minor == 0)
			{
				PhpbbFileList.AddRange(OpenTextFile(Path.Combine(domain.BaseDirectory, "files.txt")).Replace("\r\n", "\n").Split('\n'));
			}
			else if (version.Major == 3 && version.Minor == 0)
			{
				PhpbbFileList.AddRange(OpenTextFile(Path.Combine(domain.BaseDirectory, "files_3.0.txt")).Replace("\r\n", "\n").Split('\n'));
			}
			if (language != "english")
			{
				for (int i = 0; i < PhpbbFileList.Count; i++)
				{
					PhpbbFileList[i] = PhpbbFileList[i].Replace("english", language);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="language"></param>
		public static void LoadPhpbbFileList(string language)
		{
			LoadPhpbbFileList(language, new ModVersion(2, 0, 0));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="version"></param>
		public static void LoadPhpbbFileList(ModVersion version)
		{
			LoadPhpbbFileList("english", version);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void LoadPhpbbFileList()
		{
			LoadPhpbbFileList("english");
		}

		#region File Handling

		/// <summary>
		/// Open a text file
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		private static string OpenTextFile(string fileName)
		{
			StreamReader myStreamReader;
			string temp;
			try 
			{
				myStreamReader = File.OpenText(fileName);
				temp = myStreamReader.ReadToEnd();
				myStreamReader.Close();
			} 
			catch
			{
				temp = "";
			}
			return temp;
		}

		#endregion
	}
}

namespace Phpbb.ModTeam.Tools.Validation
{
	/// <summary>
	/// 
	/// </summary>
	public struct Report
	{
		/// <summary>
		/// 
		/// </summary>
		public enum ReportFormat
		{
			/// <summary>
			/// 
			/// </summary>
			Html,
			/// <summary>
			/// 
			/// </summary>
			Bbcode
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
		public string HtmlReport;

		/// <summary>
		/// 
		/// </summary>
		public string PhpReport;

		/// <summary>
		/// 
		/// </summary>
		public string DbalReport;

		/// <summary>
		/// 
		/// </summary>
		public string Rating;

		/// <summary>
		/// 
		/// </summary>
		/// <returns>Html Format</returns>
		public override string ToString()
		{
			string Report;

			Report = string.Format("[size=18]MOD Template usage[/size]\n\n{0}\n\n{1}\n\n\n" + 
				"[size=18]MOD HTML usage[/size]\n\n{2}\n\n\n" + 
				"[size=18]MOD DBAL usage[/size]\n\n{3}\n\n\n" + 
				"[size=22]Overall[/size]\n\n{4}\n\n",
				this.HeaderReport, this.ActionsReport, this.HtmlReport, this.DbalReport, this.Rating);

			return Report;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checks"></param>
        /// <returns></returns>
		public string ToString(bool checks)
		{
			if (checks)
			{
				return this.ToString();
			}
			else
			{
				string Report;

				Report = string.Format("{0}\n{1}\n" + 
					"[size=13][b]Overall[/b][/size]\n{4}\n",
					this.HeaderReport, this.ActionsReport, this.HtmlReport, this.DbalReport, this.Rating);

				return Report;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="format"></param>
		/// <returns></returns>
		public string ToString(ReportFormat format)
		{
			switch (format)
			{
				case ReportFormat.Bbcode:
					return this.ToString();
				case ReportFormat.Html:
					return Validator.BbcodeToHtml(this.ToString());
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
			/// Structured Query Language actions (SQL)
			/// </summary>
			Sql,
			/// <summary>
			/// File operation actions (OPEN, COPY, SAVE/CLOSE ALL FILES)
			/// </summary>
			File,
			/// <summary>
			/// Find operations (FIND)
			/// </summary>
			Find,
			/// <summary>
			/// Edit operations (AFTER, ADD, BEFORE, ADD, REPLACE WITH, INCREMENT)
			/// </summary>
			Edit,
			/// <summary>
			/// Inline find operations (IN-LINE FIND)
			/// </summary>
			InLineFind,
			/// <summary>
			/// Inline edit operations (IN-LINE AFTER, ADD, IN-LINE BEFORE, ADD, IN-LINE REPLACE WITH, IN-LINE INCREMENT)
			/// </summary>
			InLineEdit,
			/// <summary>
			/// Such as DIY INSTRUCTIONS
			/// </summary>
			Instruction,
			/// <summary>
			/// Null
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
			Validator.fillActions();

			if (Validator.Actions.Contains(input))
			{
				return new ModActions(input, (ModActions.ModActionType)Validator.Actions[input]);
			}
			return new ModActions("", ModActions.ModActionType.Null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static ModActionType GetType(string input)
		{
			Validator.fillActions();
			try
			{
				return (ModActionType)Validator.Actions[input];
			}
			catch
			{
				return ModActions.ModActionType.Null;
			}
		}
	}

}
