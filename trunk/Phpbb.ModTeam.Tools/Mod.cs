/***************************************************************************
 *                                  Mod.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: Mod.cs,v 1.1 2006-07-03 12:49:23 smithydll Exp $
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
	/// Provides the abstract (<b>MustInherit</b> in Visual Basic) base class for a phpBB MOD document.
	/// </summary>
	public abstract class Mod : IMod
	{

		/// <summary>
		/// How the 'MOD Description' field is to be indented.
		/// </summary>
		public CodeIndents DescriptionIndent;

		/// <summary>
		/// How the 'Author Notes' field is to be indented.
		/// </summary>
		public CodeIndents AuthorNotesIndent;

		/// <summary>
		/// Which line of the 'Author Notes' field the notes start on.
		/// </summary>
		public StartLine AuthorNotesStartLine;

		/// <summary>
		/// How the 'Files To Edit' section is to be indented.
		/// </summary>
		public CodeIndents ModFilesToEditIndent;

		/// <summary>
		/// How the 'Included Files' section is to be indented.
		/// </summary>
		public CodeIndents ModIncludedFilesIndent;

		/// <summary>
		/// Returns true if there were problems <i>detected</i> parsing the MOD.
		/// </summary>
		public bool ParseErrors;

		/// <summary>
		/// Contains the header information for the MOD.
		/// </summary>
		public ModHeader Header;

		/// <summary>
		/// Contains the actions for the MOD.
		/// </summary>
		public ModActions Actions;

		/// <summary>
		/// The format the MOD was last read into.
		/// </summary>
		protected ModFormats lastReadFormat = ModFormats.Modx;
		/// <summary>
		/// Returns true if a TextMod class was initialised in read-only mode.
		/// </summary>
		protected bool textTemplateReadOnly = false;
		/// <summary>
		/// The default language MOD files are written in. By default this is "en-gb" and should not be changed.
		/// </summary>
		protected static string defaultLanguage = "en-gb";

		/// <summary>
		/// The new line character used by the system.
		/// </summary>
		protected const char newline = '\n';
		/// <summary>
		/// Representation of the Microsoft Windows new line string.
		/// </summary>
		protected const string WinNewLine = "\r\n";

		/// <summary>
		/// White space characters which can be trimmed from strings.
		/// </summary>
		protected char[] trimChars = {' ', '\t', '\n', '\r', '\b'};

		/// <summary>
		/// Initialises a new instance of the Mod class. Must be inherited (abstract).
		/// </summary>
		public Mod()
		{
			textTemplateReadOnly = true;
			Header = new ModHeader();
			Header.Authors = new ModAuthors();
			Header.History = new ModHistory();
			Header.IncludedFiles = new StringCollection();
			Header.FilesToEdit = new StringCollection();
			Actions = new ModActions();
			Header.EasyModCompatibility = new ModVersion();
			Header.Version = new ModVersion();
		}

		/// <summary>
		/// Estimate an Installation Time for this MOD.
		/// </summary>
		/// <example>
		/// <code>TextMod myMod = new TextMod();
		/// myMod.Read(".\thismod.mod");
		/// Console.WriteLine("The old installation time was: {0}", myMod.Header.InstallationTime);
		/// myMod.UpdateInstallationTime();
		/// Console.WriteLine("The new installation time is: {0}", myMod.Header.InstallationTime);</code>
		/// </example>
		/// <remarks>Installation Time is in seconds. The algorithm is based on the following assumptions:
		/// <list type="unordered">
		/// <item>It takes <b>27</b> seconds to find and open a file.</item>
		/// <item>It takes <b>50</b> seconds to execute an SQL block.</item>
		/// <item>It takes <b>5</b> seconds to copy each file.</item>
		/// <item>It takes <b>12</b> seconds to fine a line or in-line portion in a file.</item>
		/// <item>It takes <b>18</b> seconds to perform an edit to a file.</item>
		/// <item>It takes <b>60</b> seconds to perform a do-it-yourself set of instructions.</item>
		/// </list>
		/// While this is only an estimate, it is considered a "good-enough" conservative estimate.
		/// It may take longer or shorter to perform an install in real life.</remarks>
		public void UpdateInstallationTime()
		{
			int totalInstallTime = 126;
			foreach (ModAction e in Actions)
			{
				switch (e.Type)
				{
					case "OPEN":
						totalInstallTime += 27;
						break;
					case "SQL":
						totalInstallTime += 50;
						break;
					case "COPY":
						totalInstallTime += e.Body.Split('\n').Length * 5;
						break;
					case "FIND":
					case "IN-LINE FIND":
						totalInstallTime += 12;
						break;
					case "AFTER, ADD":
					case "BEFORE, ADD":
					case "REPLACE WITH":
					case "INCREMENT":
					case "IN-LINE AFTER, ADD":
					case "IN-LINE BEFORE, ADD":
					case "IN-LINE REPLACE WITH":
					case "IN-LINE INCREMENT":
						totalInstallTime += 18;
						break;
					case "DIY INSTRUCTIONS":
						totalInstallTime += 60;
						break;
				}
			}
			Header.InstallationTime = totalInstallTime;
		}

		/// <summary>
		/// Update Included files for this MOD.
		/// </summary>
		/// <example>
		/// <code>TextMod myMod = new TextMod();
		/// myMod.Read(".\thismod.mod");
		/// Console.WriteLine("The old number of included files was: {0}", myMod.Header.IncludedFiles.Count);
		/// myMod.UpdateIncludedFiles();
		/// Console.WriteLine("The new number of included files is: {0}", myMod.Header.IncludedFiles.Count);</code>
		/// </example>
		/// <remarks>MODs in the MODX format don't have a list of included files.</remarks>
		public void UpdateIncludedFiles()
		{
			Header.IncludedFiles = new StringCollection();
			Header.IncludedFiles.Clear();
			for (int i = 0; i < Actions.Count; i++)
			{
				if (Actions[i].Type == "COPY")
				{
					string[] lines = Actions[i].Body.Split('\n');
					foreach (string line in lines)
					{
						if (line.TrimStart(trimChars).ToLower().StartsWith("copy"))
						{
							//Header.IncludedFiles.Add(Regex.Match(line.Trim(trimChars), "copy (.+) to", RegexOptions.IgnoreCase).Value.Replace("copy ", "").Replace(" to", ""));
							string from = Regex.Replace(Regex.Replace(Regex.Match(line.Trim(trimChars), "^copy (.+) to", RegexOptions.IgnoreCase).Value, "copy ", "", RegexOptions.IgnoreCase), " to", "", RegexOptions.IgnoreCase).Trim(trimChars);
							Header.IncludedFiles.Add(from);
						}
					}
				}
			}
		}

		/// <summary>
		/// Update Files to Edit for this MOD.
		/// </summary>
		/// <example>
		/// <code>TextMod myMod = new TextMod();
		/// myMod.Read(".\thismod.mod");
		/// Console.WriteLine("The old number of files to edit was: {0}", myMod.Header.IncludedFiles.Count);
		/// myMod.UpdateFilesToEdit();
		/// Console.WriteLine("The new number of files to edit is: {0}", myMod.Header.IncludedFiles.Count);</code>
		/// </example>
		/// <remarks>MODs in the MODX format don't have a list of files to be edited.</remarks>
		public void UpdateFilesToEdit()
		{
			Header.FilesToEdit = new StringCollection();
			Header.FilesToEdit.Clear();
			foreach (ModAction e in Actions)
			{
				if (e.Type == "OPEN")
				{
					Header.FilesToEdit.Add(e.Body.Trim(trimChars));
				}
			}
		}

		/// <summary>
		/// Performs a non-case sensitive compare on the installation level and returns an enumerated result of type
		/// <b>ModInstallationLevel</b> with possible values of {<b>Easy</b>, <b>Intermediate</b>, <b>Advanced</b>}.
		/// </summary>
		/// <param name="input">A string representing an installation level for the MOD.</param>
		/// <returns>An installation level of type <b>ModInstallationLevel</b>.</returns>
		/// <remarks>Automatically trims brackets {<b>'('</b>, <b>')'</b>} from the installation level. Also converts "hard" to Advanced, and "moderate" to Intermediate.</remarks>
		/// <example>
		/// <code>
		/// ModInstallationLevel mil = Mod.InstallationLevelParse("easy");
		/// Console.WriteLine(mil.ToString());
		/// mil = Mod.InstallationLevelParse("Moderate");
		/// Console.WriteLine(mil.ToString());
		/// </code>
		/// Prints the following:
		/// <code>Easy
		/// Intermediate</code>
		/// </example>
		public static ModInstallationLevel InstallationLevelParse(string input)
		{
			char[] trimChars = {'(', ')'};
			switch (input.ToUpper().Trim(trimChars))
			{
				case "EASY":
					return ModInstallationLevel.Easy;
				case "MODERATE":
					return ModInstallationLevel.Intermediate;
				case "INTERMEDIATE":
					return ModInstallationLevel.Intermediate;
				case "HARD":
					return ModInstallationLevel.Advanced;
				case "ADVANCED":
					return ModInstallationLevel.Advanced;
				default:
					return ModInstallationLevel.Easy;
			}
		}

		/// <summary>
		/// The default language MOD files are written in. By default this is "en-gb" and should not be changed.
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

		/// <summary>
		/// Converts a natural language string denoting a unit of time to seconds.
		/// </summary>
		/// <param name="input">A string to parse.</param>
		/// <returns>Whole seconds represented by the unit of time.</returns>
		/// <remarks>Can parse from times given in seconds, minutes, and hours.</remarks>
		public static int StringToSeconds(string input)
		{
			string[] getInts = input.Split(' ');
			int seconds = 0;
			for (int i = 0; i <= getInts.GetUpperBound(0); i++) 
			{
				if (Regex.IsMatch(getInts[i], "minute", RegexOptions.IgnoreCase)) 
				{
					seconds += int.Parse(Regex.Replace(Regex.Replace(getInts[i - 1], "^([A-Za-z~ ]*?)([0-9\\.]+?)([A-Za-z ]*?)$", "$2"), "^([0-9]+)\\-", "")) * 60;
				}
				if (Regex.IsMatch(getInts[i], "second", RegexOptions.IgnoreCase)) 
				{
					seconds += int.Parse(Regex.Replace(getInts[i - 1], "^([A-Za-z~ ]*?)([0-9\\.]+?)([A-Za-z ]*?)$", "$2"));
				}
				if (Regex.IsMatch(getInts[i], "hour", RegexOptions.IgnoreCase)) 
				{
					seconds += System.Convert.ToInt32(Regex.Replace(getInts[i - 1], "^([A-Za-z~ ]*?)([0-9\\.]+?)([A-Za-z ]*?)$", "$2")) * (int)(Math.Pow(60, 2));
				}
			}
			return seconds;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		public abstract void Read(string fileName);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		public abstract void Write(string fileName);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public abstract override string ToString();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public abstract Validation.Report Validate(string fileName);

		/// <summary>
		/// 
		/// </summary>
		public ModFormats LastReadFormat
		{
			get
			{
				return lastReadFormat;
			}
			set
			{
				lastReadFormat = value;
			}
		}

		#region file handling

		/// <summary>
		/// Open a text file
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		protected static string OpenTextFile(string fileName)
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

		/// <summary>
		/// Save a text file
		/// </summary>
		/// <param name="fileToSave"></param>
		/// <param name="fileName"></param>
		protected static void SaveTextFile(string fileToSave, string fileName)
		{
			StreamWriter myStreamWriter = File.CreateText(fileName);
			myStreamWriter.Write(fileToSave);
			myStreamWriter.Close();
		}

		#endregion
	}
}
