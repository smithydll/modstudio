/***************************************************************************
 *                                PhpbbMod.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: PhpbbMod.cs,v 1.4 2005-07-09 13:18:46 smithydll Exp $
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
	/// The phpBBMOD class is a class based memory representation of the phpBB MOD Template. Compatible with 
	/// both the Text and XML Templates as of the July 2005 updates.
	/// </summary>
	public class PhpbbMod
	{

		private const string DefaultLanguage = "en-GB";
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
		/// Instead of using Strings directly, we have to support a multitude of language in the MOD Template.
		/// Therefore we have to start using this structure.
		/// </summary>
		public struct PropertyLang
		{
			/// <summary>
			/// The target language for this version. Default is en-GB as specified in phpBB3.0 documentation.
			/// </summary>
			private string[] Language;
			private string[] Value;

			/// <summary>
			/// 
			/// </summary>
			/// <param name="_value"></param>
			/// <param name="_language"></param>
			public PropertyLang(string _value, string _language)
			{
				this.Language = new string[1];
				this.Language[0] = _language;
				this.Value = new string[1];
				this.Value[0] = _value;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="_value"></param>
			public PropertyLang(string _value)
			{
				this.Language = new string[1];
				this.Language[0] = DefaultLanguage;
				this.Value = new string[1];
				this.Value[0] = _value;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="_value"></param>
			/// <param name="_language"></param>
			public void AddLanguage(string _value, string _language)
			{
				if (Language != null)
				{
					string[] tempL = this.Language;
					string[] tempV = this.Value;

					this.Language = new string[tempL.Length + 1];
					tempL.CopyTo(this.Language,0);
					this.Language[Language.GetUpperBound(0)] = _language;
					this.Value = new string[tempV.Length + 1];
					tempV.CopyTo(this.Value,0);
					this.Value[Value.GetUpperBound(0)] = _value;
				}
				else
				{
					Language = new string[1];
					Language[0] = _language;
					Value = new string[1];
					Value[0] = _value;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="_language"></param>
			/// <returns></returns>
			public string GetValue(string _language)
			{
				if (Language != null)
				{
					for (int i = 0; i < this.Language.Length; i++)
					{
						if (_language == this.Language[i])
						{
							return this.Value[i];
						}
					}
				}
				return null;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <returns></returns>
			public string GetValue()
			{
				return GetValue(DefaultLanguage);
			}

			/// <summary>
			/// Set the default language value
			/// </summary>
			public void SetValue(string _value)
			{
				if (Language != null)
				{
					for (int i = 0; i < this.Language.Length; i++)
					{
						if (DefaultLanguage == this.Language[i])
						{
							this.Value[i] = _value;
						}
					}
				}
				else
				{
					Language = new string[1];
					Language[0] = DefaultLanguage;
					Value = new string[1];
					Value[0] = _value;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public string pValue
			{
				get
				{
					return GetValue();
				}
				set
				{
					SetValue(value);
				}
			}
		}

		/// <summary>
		/// <p>I suppose an explanation is in order.</p>
		/// <p>In the text format the following holds true.</p>
		/// <code>#
		/// # Before Comment
		/// #
		/// #-----[ ACTION:modifier ]---------------------
		/// #
		/// # After Comment
		/// #
		/// Action Body</code>
		/// <p>Use of the Before Comment is not recommended as it is not segregated from the 
		/// after comment and threrefore incompatible with the XML format. Also for technical
		/// reasons the Before Comment is not parsed by this parser and will be permanently lost.</p>
		/// <p>It is therefore recommended that if you wish to leave comments that the After 
		/// Comment or the Author Notes be used.</p>
		/// <p>The modifier is not an official element of the text format, however it is used
		/// as a compatibility measure between the text and XML formats in this parser.</p>
		/// </summary>
		public struct ModAction
		{
			/// <summary>
			/// 
			/// </summary>
			public string ActionType;
			/// <summary>
			/// 
			/// </summary>
			public string ActionBody;
			/// <summary>
			/// 
			/// </summary>
			public string BeforeComment;
			/// <summary>
			/// 
			/// </summary>
			public string AfterComment;
			/// <summary>
			/// 
			/// </summary>
			public int StartLine;
			/// <summary>
			/// Modifier is a variable that modifies the behaviour of an actions, for example regex can
			/// modify a FIND to do a regular expression based find.
			/// </summary>
			public string Modifier;

			/// <summary>
			/// 
			/// </summary>
			/// <param name="actiontype">Type</param>
			/// <param name="actionbody">Body</param>
			/// <param name="beforecomment">Before Comment</param>
			/// <param name="aftercomment">After Comment</param>
			/// <param name="startline">Start line</param>
			/// <param name="modifier">Modifier</param>
			public ModAction(string actiontype, string actionbody, string beforecomment, string aftercomment, int startline, string modifier)
			{
				this.ActionType = actiontype;
				this.ActionBody = actionbody;
				this.BeforeComment = beforecomment;
				this.AfterComment = aftercomment;
				this.StartLine = startline;
				this.Modifier = modifier;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="actiontype">Type</param>
			/// <param name="actionbody">Body</param>
			/// <param name="beforecomment">Before Comment</param>
			/// <param name="aftercomment">After Comment</param>
			/// <param name="startline">Start line</param>
			public ModAction(string actiontype, string actionbody, string beforecomment, string aftercomment, int startline)
			{
				this.ActionType = actiontype;
				this.ActionBody = actionbody;
				this.BeforeComment = beforecomment;
				this.AfterComment = aftercomment;
				this.StartLine = startline;
				this.Modifier = null;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="actiontype">Type</param>
			/// <param name="actionbody">Body</param>
			/// <param name="aftercomment">After Comment</param>
			/// <param name="modifier">Modifier</param>
			public ModAction(string actiontype, string actionbody, string aftercomment, string modifier)
			{
				this.ActionType = actiontype;
				this.ActionBody = actionbody;
				this.BeforeComment = null;
				this.AfterComment = aftercomment;
				this.StartLine = 0;
				this.Modifier = modifier;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="actiontype">Type</param>
			/// <param name="actionbody">Body</param>
			/// <param name="aftercomment">After Comment</param>
			public ModAction(string actiontype, string actionbody, string aftercomment)
			{
				this.ActionType = actiontype;
				this.ActionBody = actionbody;
				this.BeforeComment = null;
				this.AfterComment = aftercomment;
				this.StartLine = 0;
				this.Modifier = null;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <returns></returns>
			public override string ToString()
			{
				// TODO: niceify this
				string MODBuild = "";
				MODBuild += "#\n";
				MODBuild += "#-----[ " + ActionType + " ]------------------------------------------\n";
				MODBuild += "#\n";
				if (!(AfterComment == null || AfterComment == "\n")) 
				{
					string[] ACsplit = AfterComment.Replace("\r\n", "\n").Split('\n');
					for (int j = 0; j < ACsplit.Length; j++) 
					{
						if (!((ACsplit[j] == "" && j == 0))) 
						{
							//MODBuild += Newline;
							MODBuild += "\n# " + ACsplit[j];
						}
					}
				}
				//MODBuild += Newline;
				MODBuild += ActionBody;
				return MODBuild;
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
			public ModAction[] Actions;

			/// <summary>
			/// 
			/// </summary>
			/// <param name="newaction"></param>
			public void AddEntry(ModAction newaction)
			{
				if (Actions != null)
				{
					ModAction[] tempArray = Actions;
					Actions = new ModAction[tempArray.Length + 1];
					tempArray.CopyTo(Actions, 0);
				}
				else
				{
					Actions = new ModAction[1];
				}

				Actions[Actions.GetUpperBound(0)] = newaction;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="newaction"></param>
			/// <param name="BeforeAction"></param>
			public void AddEntry(ModAction newaction, int BeforeAction)
			{
				if (-10 >= BeforeAction && BeforeAction <= -1) 
				{
					AddEntry(newaction);
				} 
				else 
				{
					ModAction[] intermitentActions;
					intermitentActions = Actions;

					ModAction[] tempActions = Actions;
					Actions = new ModAction[tempActions.Length + 1];

					for (int i = 0; i <= BeforeAction - 1; i++) 
					{
						Actions[i] = intermitentActions[i];
					}

					Actions[BeforeAction] = newaction;

					for (int i = BeforeAction; i <= Actions.GetUpperBound(0) - 1; i++) 
					{
						Actions[i + 1] = intermitentActions[i];
					}
				}
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="index"></param>
			public void RemoveEntry(int index)
			{
				ModAction[] tempActions = Actions;

				Actions = new ModAction[Actions.Length -1];

				for (int i = 0; i < index; i++) 
				{
					Actions[i] = tempActions[i];
				}
				for (int i = index; i < tempActions.Length - 1; i++) 
				{
					Actions[i] = tempActions[i + 1];
				}
				tempActions = null;
			}
		}

		/// <summary>
		/// Respresents a modification header section.
		/// </summary>
		public struct ModHeader
		{
			/// <summary>
			/// 
			/// </summary>
			public PropertyLang ModTitle;
			/// <summary>
			/// 
			/// </summary>
			public ModAuthor ModAuthor;
			/// <summary>
			/// 
			/// </summary>
			public PropertyLang ModDescription;
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
			public string[] ModFilesToEdit;
			/// <summary>
			/// 
			/// </summary>
			public string[] ModIncludedFiles;
			/// <summary>
			/// 
			/// </summary>
			public string ModGenerator;
			/// <summary>
			/// 
			/// </summary>
			public PropertyLang ModAuthorNotes;
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
			public string[] Meta;
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
				input = Regex.Replace(input.Trim(TrimChars), "([\\d]+?)\\.([\\d]+?)(\\.|)([\\d]+?|)([a-zA-Z]{0,1}?)", "$1.$2.$4.$5");
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
				this.HistoryChanges = new PropertyLang(HistoryChanges, DefaultLanguage);
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
			/// <param name="_value"></param>
			/// <param name="_language"></param>
			public void AddLanguage(string _value, string _language)
			{
				HistoryChanges.AddLanguage(_value, _language);
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
			for (int j = 1; j <= 11; j++) 
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
								Header.ModFilesToEdit = new string[1]; 
								Header.ModFilesToEdit[0] = Regex.Replace(TextModLines[i], "\\#\\# Files To Edit\\:", "", RegexOptions.IgnoreCase).TrimStart(' ').TrimStart('\t').TrimEnd(' ');
							} 
							else 
							{
								if (TextModLines[i].ToUpper().StartsWith("## IN")) 
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
									string[] tempA = Header.ModFilesToEdit;
									Header.ModFilesToEdit = new string[tempA.Length + 1];
									tempA.CopyTo(Header.ModFilesToEdit, 0);

									Header.ModFilesToEdit[Header.ModFilesToEdit.GetUpperBound(0)] = tempii.TrimStart(' ').TrimStart('\t').TrimEnd(' ');
								}
							}
							break;
						case 9:
							if (TextModLines[i].ToUpper().StartsWith("## INCLUDED FILES")) 
							{
								Header.ModIncludedFiles = new string[1];
								Header.ModIncludedFiles[0] = Regex.Replace(TextModLines[i], "\\#\\# Included Files\\:", "", RegexOptions.IgnoreCase).TrimStart(' ').TrimStart('\t').TrimEnd(' ');
							} 
							else 
							{
								if (TextModLines[i].ToUpper().StartsWith("####") || TextModLines[i].ToUpper().StartsWith("## GEN")) 
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
									string[] tempA = Header.ModIncludedFiles;
									Header.ModIncludedFiles = new string[tempA.Length + 1];
									tempA.CopyTo(Header.ModIncludedFiles, 0);

									Header.ModIncludedFiles[Header.ModIncludedFiles.GetUpperBound(0)] = tempii.TrimStart(' ').TrimStart('\t').TrimEnd(' ');
								}
							}
							break;
						case 10:
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
						case 11:
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
										ModVersion HVersion = ModVersion.Parse(Regex.Match(TextModLines[i], "([\\d]+?)\\.([\\d]+?)(\\.|)([\\d]+?)([a-zA-Z]{1}?|)").Value);
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
			string ThisMODActionBody = null;
			string ThisMODActionType = null;
			string ThisMODActionComm = null;
			string NextMODActionComm = null;
			bool IsFirstMALine = false;
			bool IsFirstTCLine = false;
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
		/// </summary>
		/// <param name="XmlMod">A string containing the text of the MOD in XML format.</param>
		public void ReadXml(string XmlMod)
		{
			LastReadFormat = ModFormats.XMLMOD;
		}

		/// <summary>
		/// Read and parse a file containing a MOD.
		/// </summary>
		/// <param name="FileName">A string containing the file path to the MOD.</param>
		public void ReadFile(string FileName)
		{
			Read(OpenTextFile(FileName));
		}

		/// <summary>
		/// Read and parse a MOD automatically deciding if it's an XML or Text based MOD.
		/// </summary>
		/// <param name="ModToRead">A string containing the text of the MOD.</param>
		public void Read(string ModToRead)
		{
			if (ModToRead.StartsWith("##"))
			{
				ReadText(ModToRead);
			}
			else
			{
				ReadXml(ModToRead);
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
		}

		/// <summary>
		/// Write the MOD to a file in the designated file format.
		/// </summary>
		/// <param name="Filename">A string containing the file path where the MOD is to be saved.</param>
		/// <param name="Format">The format that will be saved.</param>
		public void WriteFile(string Filename, ModFormats Format)
		{
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
					MyMODAuthorS += Newline + "## MOD Author, secondary: " + Header.ModAuthor.Authors[i].UserName + " < " + Header.ModAuthor.Authors[i].Email + " > (" + Header.ModAuthor.Authors[i].RealName + ") " + Header.ModAuthor.Authors[i].Homepage;
				}
			}

			BlankTemplate = BlankTemplate.Replace("<mod.author/>", MyMODAuthorS);
			BlankTemplate = BlankTemplate.Replace("<mod.description/>", Header.ModDescription.GetValue().Replace(Newline.ToString(), Newline.ToString() + "## "));
			BlankTemplate = BlankTemplate.Replace("<mod.version/>", Header.ModVersion.ToString());
			BlankTemplate = BlankTemplate.Replace("<mod.install_level/>", Header.ModInstallationLevel.ToString());
			BlankTemplate = BlankTemplate.Replace("<mod.install_time/>", Math.Ceiling(Header.ModInstallationTime / 60) + " minutes");

			string MyMODFTE = null;
			for (int i = 0; i < Header.ModFilesToEdit.Length; i++) 
			{
				if (i == 0) 
				{
					MyMODFTE = Header.ModFilesToEdit[i];
				} 
				else 
				{
					MyMODFTE += Newline + "## " + Header.ModFilesToEdit[i];
				}
			}
			BlankTemplate = BlankTemplate.Replace("<mod.files_to_edit/>", MyMODFTE);

			string MyMODIC = null;
			for (int i = 0; i < Header.ModIncludedFiles.Length; i++) 
			{
				if (i == 0) 
				{
					MyMODIC = Header.ModIncludedFiles[i];
				} 
				else 
				{
					MyMODIC += Newline + "## " + Header.ModIncludedFiles[i];
				}
			}
			BlankTemplate = BlankTemplate.Replace("<mod.inc_files/>", MyMODIC);
			BlankTemplate = BlankTemplate.Replace("<mod.generator/>", Newline + "## Generator: MOD Studio [ ModTemplateTools " + System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion + " ]");
			BlankTemplate = BlankTemplate.Replace("<mod.author_notes/>", Header.ModAuthorNotes.GetValue().Replace(Newline.ToString(), Newline.ToString() + "## "));
			string MyMODHistory;
			System.Text.StringBuilder NewMyMODHistory = new System.Text.StringBuilder();
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
				foreach (ModAction MA in Actions.Actions) 
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
								if (!((ACsplit[j] == "" & j == 0))) 
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
		private void SaveTextFile(string filetosave, string filename)
		{
		}

	}
}
