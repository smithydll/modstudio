/***************************************************************************
 *                           ModDataStructures.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: ModDataStructures.cs,v 1.6 2006-01-25 02:08:10 smithydll Exp $
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
using ModTemplateTools;

namespace ModTemplateTools.DataStructures
{
	/// <summary>
	/// Instead of using Strings directly, we have to support a multitude of language in the MOD Template.
	/// Therefore we have to start using this structure.
	/// </summary>
	public class StringLocalised : System.Collections.IEnumerable
	{
		private StringDictionary keyList;

		/// <summary>
		/// 
		/// </summary>
		public int Count
		{
			get
			{
				return keyList.Count;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public System.Collections.IEnumerator GetEnumerator()
		{
			return keyList.Keys.GetEnumerator();
		}

		/// <summary>
		/// 
		/// </summary>
		public StringLocalised()
		{
			keyList = new StringDictionary();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="language"></param>
		public StringLocalised(string value, string language)
		{
			keyList = new StringDictionary();
			keyList.Add(language, value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		public StringLocalised(string value)
		{
			keyList = new StringDictionary();
			keyList.Add(PhpbbMod.DefaultLanguage, value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="language"></param>
		public void Add(string value, string language)
		{
			keyList.Add(language, value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="language"></param>
		public void Remove(string language)
		{
			keyList.Remove(language);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string GetValue()
		{
			if (keyList.ContainsKey(PhpbbMod.DefaultLanguage))
				return keyList[PhpbbMod.DefaultLanguage];
			return "";
		}

		/// <summary>
		/// 
		/// </summary>
		public string this[string language]
		{
			get
			{
				if (keyList.ContainsKey(language))
					return keyList[language];
				return "";
			}
			set
			{
				keyList[language] = value;
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
	public class ModActions : System.Collections.IEnumerable, System.Collections.ICollection
	{
		/// <summary>
		/// Legacy, please use the enumerator. This is considered depricated.
		/// </summary>
		/*public ModAction[] Actions
		{
			get
			{
				throw new NotSupportedException("This property has been depricated, please use the IList features");
				//return (ModAction[])(actions.ToArray(System.Type.GetType("ModTemplateTools.PhpbbMod.ModAction")));
			}
		}*/
		//public ModAction[] Actions;

		private ArrayList actions;

		/// <summary>
		/// 
		/// </summary>
		public bool IsSynchronized
		{
			get
			{
				return actions.IsSynchronized;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public object SyncRoot
		{
			get 
			{
				return actions.SyncRoot;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int Count
		{
			get
			{
				return actions.Count;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public System.Collections.IEnumerator GetEnumerator()
		{
			return actions.GetEnumerator();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_array"></param>
		/// <param name="_int"></param>
		public void CopyTo(System.Array _array, int _int)
		{
			actions.CopyTo(_array, _int);
		}

		/// <summary>
		/// 
		/// </summary>
		public ModActions()
		{
			actions = new ArrayList();
		}

		/// <summary>
		/// 
		/// </summary>
		public void Clear()
		{
			actions.Clear();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public bool Contains(object o)
		{
			return Contains((ModAction)o);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public bool Contains(ModAction action)
		{
			return actions.Contains(action);
		}

		/// <summary>
		/// 
		/// </summary>
		public ModAction this[int index]
		{
			get
			{
				return (ModAction)actions[index];
			}
			set
			{
				actions[index] = (ModAction)value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="newaction"></param>
		public int Add(ModAction newaction)
		{
			return actions.Add(newaction);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="BeforeAction"></param>
		/// <param name="newaction"></param>
		/// <returns></returns>
		public void Insert(int BeforeAction, ModAction newaction)
		{
			actions.Insert(BeforeAction, newaction);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			actions.RemoveAt(index);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="o"></param>
		public void Remove(ModAction o)
		{
			actions.Remove(o);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public int IndexOf(ModAction action)
		{
			return actions.IndexOf(action);
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class ModAuthor : System.Collections.IEnumerable, System.Collections.ICollection
	{
		private ArrayList authors;
		//public ModAuthorEntry[] Authors;

		/// <summary>
		/// 
		/// </summary>
		public ModAuthor()
		{
			authors = new ArrayList();
		}

		/// <summary>
		/// 
		/// </summary>
		public ArrayList Authors
		{
			get
			{
				return authors;
			}
			set
			{
				authors = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsReadOnly
		{
			get
			{
				return authors.IsReadOnly;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public ModAuthorEntry this[int index]
		{
			get
			{
				return (ModAuthorEntry)authors[index];
			}
			set
			{
				authors[index] = (ModAuthorEntry)value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			authors.RemoveAt(index);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public void Insert(int index, ModAuthorEntry value)
		{
			authors.Insert(index, value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		public void Remove(ModAuthorEntry value)
		{
			authors.Remove((ModAuthorEntry)value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool Contains(ModAuthorEntry value)
		{
			return authors.Contains(value);
		}

		/// <summary>
		/// 
		/// </summary>
		public void Clear()
		{
			authors.Clear();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int IndexOf(ModAuthorEntry value)
		{
			return authors.IndexOf(value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int Add(ModAuthorEntry value)
		{
			return authors.Add((ModAuthorEntry)value);
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsFixedSize
		{
			get
			{
				return authors.IsFixedSize;
			}
		}

		#region ICollection Members

		/// <summary>
		/// 
		/// </summary>
		public bool IsSynchronized
		{
			get
			{
				return authors.IsSynchronized;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int Count
		{
			get
			{
				return authors.Count;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		public void CopyTo(Array array, int index)
		{
			authors.CopyTo(array, index);
		}

		/// <summary>
		/// 
		/// </summary>
		public object SyncRoot
		{
			get
			{
				return authors.SyncRoot;
			}
		}

		#endregion

		#region IEnumerable Members

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return authors.GetEnumerator();
		}

		#endregion
	}

	/// <summary>
	/// 
	/// </summary>
	public class ModAuthorEntry : IComparable
	{
		/// <summary>
		/// phpBB.com username (or place of MOD origin, usually phpBB.com)
		/// </summary>
		public string UserName;
		/// <summary>
		/// The authors real name (optional)
		/// </summary>
		public string RealName;
		/// <summary>
		/// The authors e-mail address (optional)
		/// </summary>
		public string Email;
		/// <summary>
		/// The authors homepage (optional)
		/// </summary>
		public string Homepage;
		/// <summary>
		/// The date the author started working on the MOD.
		/// </summary>
		public int AuthorFrom;
		/// <summary>
		/// The last year the author started working on the MOD.
		/// </summary>
		public int AuthorTo;
		/// <summary>
		/// Is the author the present of a past developer of the MOD?
		/// </summary>
		public ModAuthorStatus Status;

		/// <summary>
		/// Construct an author object.
		/// </summary>
		public ModAuthorEntry()
		{
			this.UserName = "N/A";
			this.RealName = "N/A";
			this.Email = "N/A";
			this.Homepage = "N/A";
			this.AuthorFrom = -1;
			this.AuthorTo = -1;
			this.Status = ModAuthorStatus.NoneSelected;
		}

		/// <summary>
		/// Construct an author object.
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
			this.AuthorFrom = -1;
			this.AuthorTo = -1;
			this.Status = ModAuthorStatus.NoneSelected;
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
		/// Deprecated
		/// </summary>
		/// <param name="yes"></param>
		public ModAuthorEntry(bool yes)
		{
			throw new NotSupportedException("This constructor has been deprecated, please use the default constructor");
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

		#region IComparable Members

		/// <summary>
		/// Compares two Authors based on their username, useful for ordering and making sure that no 
		/// two authors have the same username.
		/// </summary>
		/// <param name="obj">Author Object to compare to</param>
		/// <returns></returns>
		public int CompareTo(object obj)
		{
			ModAuthorEntry a2 = (ModAuthorEntry)obj;
			return this.UserName.CompareTo(a2.UserName);
		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			ModAuthorEntry a2 = (ModAuthorEntry)obj;
			return this.UserName.Equals(a2.UserName);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return base.GetHashCode ();
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
		NoneSelected,
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
	/// Respresents a modification header section.
	/// </summary>
	public class ModHeader
	{
		/// <summary>
		/// 
		/// </summary>
		public StringLocalised ModTitle = new StringLocalised();
		/// <summary>
		/// 
		/// </summary>
		public ModAuthor ModAuthor;
		/// <summary>
		/// 
		/// </summary>
		public StringLocalised ModDescription = new StringLocalised();
		/// <summary>
		/// 
		/// </summary>
		public ModVersion ModVersion;
		/// <summary>
		/// 
		/// </summary>
		public PhpbbMod.ModInstallationLevel ModInstallationLevel;
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
		public StringLocalised ModAuthorNotes = new StringLocalised();
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
	public class ModVersion
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
		public ModVersion()
		{
			this.VersionMajor = 1;
			this.VersionMinor = 0;
			this.VersionRevision = 0;
			this.VersionRelease = nullChar;
		}

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
	/// A list of changes for a given language in a MOD History Entry
	/// </summary>
	public class ModHistoryChangeLog : System.Collections.IEnumerable, System.Collections.ICollection
	{
		private ArrayList changeLog;
		private string language;

		/// <summary>
		/// 
		/// </summary>
		public ModHistoryChangeLog()
		{
			changeLog = new ArrayList();
		}

		/// <summary>
		/// 
		/// </summary>
		public string Language
		{
			get
			{
				return language;
			}
			set
			{
				language = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string this[int index]
		{
			// TODO: 
			get
			{
				try
				{
					return (string)changeLog[index];
				}
				catch
				{
					return "";
				}
			}
			set
			{
				try
				{
					changeLog[index] = value;
				}
				catch
				{
					changeLog.Add(value);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="change"></param>
		public void Add(string change)
		{
			changeLog.Add(change);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			changeLog.RemoveAt(index);
		}

		#region IEnumerable Members

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return changeLog.GetEnumerator();
		}

		#endregion

		#region ICollection Members

		/// <summary>
		/// 
		/// </summary>
		public bool IsSynchronized
		{
			get
			{
				return changeLog.IsSynchronized;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int Count
		{
			get
			{
				return changeLog.Count;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		public void CopyTo(Array array, int index)
		{
			changeLog.CopyTo(array, index);
		}

		/// <summary>
		/// 
		/// </summary>
		public object SyncRoot
		{
			get
			{
				return changeLog.SyncRoot;
			}
		}

		#endregion
	}

	/// <summary>
	/// 
	/// </summary>
	public class ModHistoryChangeLogLocalised : System.Collections.IEnumerable
	{
		private Hashtable changeLogs;

		/// <summary>
		/// 
		/// </summary>
		public ModHistoryChangeLogLocalised()
		{
			changeLogs = new Hashtable();
		}

		/// <summary>
		/// 
		/// </summary>
		public ModHistoryChangeLog this[string language]
		{
			get
			{
				((ModHistoryChangeLog)changeLogs[language]).Language = language;
				return (ModHistoryChangeLog)changeLogs[language];
			}
			set
			{
				changeLogs[language] = value;
				((ModHistoryChangeLog)changeLogs[language]).Language = language;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="language"></param>
		public void Add(ModHistoryChangeLog value, string language)
		{
			changeLogs.Add(language, value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="language"></param>
		public void Remove(string language)
		{
			changeLogs.Remove(language);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public int Count
		{
			get
			{
				return changeLogs.Count;
			}
		}

		#region IEnumerable Members

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return changeLogs.GetEnumerator();
		}

		#endregion
	}

	/// <summary>
	/// A MOD History Entry
	/// </summary>
	public class ModHistoryEntry
	{
		/// <summary>
		/// 
		/// </summary>
		public ModVersion HistoryVersion;
		/// <summary>
		/// 
		/// </summary>
		public System.DateTime HistoryDate;
		//public StringLocalised HistoryChanges;
		/// <summary>
		/// 
		/// </summary>
		public ModHistoryChangeLogLocalised HistoryChangeLog;

		/// <summary>
		/// 
		/// </summary>
		public ModHistoryEntry()
		{
			this.HistoryVersion = new ModVersion(0,0,0);
			this.HistoryDate = DateTime.Now;
			//this.HistoryChanges = new StringLocalised();
			this.HistoryChangeLog = new ModHistoryChangeLogLocalised();
		}

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
			//this.HistoryChanges = new StringLocalised(HistoryChanges, PhpbbMod.DefaultLanguage);
			this.HistoryChangeLog = new ModHistoryChangeLogLocalised();
			this.HistoryChangeLog.Add(new ModHistoryChangeLog(), PhpbbMod.DefaultLanguage);
			this.HistoryChangeLog[PhpbbMod.DefaultLanguage].Add(HistoryChanges);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="HistoryVersion"></param>
		/// <param name="HistoryDate"></param>
		/// <param name="HistoryChanges"></param>
		public ModHistoryEntry(ModVersion HistoryVersion, System.DateTime HistoryDate, ModHistoryChangeLogLocalised HistoryChanges)
		{
			this.HistoryVersion = HistoryVersion;
			this.HistoryDate = HistoryDate;
			this.HistoryChangeLog = HistoryChanges;
		}
	}

	/// <summary>
	/// A collection of useful methods for organising ModHistory entries.
	/// </summary>
	public class ModHistory : System.Collections.IEnumerable, System.Collections.ICollection
	{
		/// <summary>
		/// The string which represents an empty MOD Author field.
		/// </summary>
		public const string Empty = "N/A";

		/// <summary>
		/// 
		/// </summary>
		public ArrayList History;

		/// <summary>
		/// 
		/// </summary>
		public ModHistory()
		{
			History = new ArrayList();
		}

		/// <summary>
		/// 
		/// </summary>
		public ModHistoryEntry this[int index]
		{
			get
			{
				return (ModHistoryEntry)History[index];
			}
			set
			{
				History[index] = (ModHistoryEntry)value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="HistoryVersion"></param>
		/// <param name="HistoryDate"></param>
		/// <param name="HistoryChanges"></param>
		public void Add(ModVersion HistoryVersion, System.DateTime HistoryDate, string HistoryChanges)
		{
			Add(new ModHistoryEntry(HistoryVersion, HistoryDate, HistoryChanges));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="newhistory"></param>
		public void Add(ModHistoryEntry newhistory)
		{
			if (History == null) History = new ArrayList();
			History.Add(newhistory);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public void Insert(int index, ModHistoryEntry value)
		{
			History.Insert(index, value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			History.RemoveAt(index);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		public void Remove(ModHistoryEntry value)
		{
			History.Remove(value);
		}

		#region IEnumerable Members

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return History.GetEnumerator();
		}

		#endregion

		#region ICollection Members

		/// <summary>
		/// 
		/// </summary>
		public bool IsSynchronized
		{
			get
			{
				return History.IsSynchronized;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int Count
		{
			get
			{
				return History.Count;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		public void CopyTo(Array array, int index)
		{
			History.CopyTo(array, index);
		}

		/// <summary>
		/// 
		/// </summary>
		public object SyncRoot
		{
			get
			{
				return History.SyncRoot;
			}
		}

		#endregion
	}

	/// <summary>
	/// 
	/// </summary>
	public enum CodeIndents
	{
		/// <summary>
		/// 
		/// </summary>
		Space,
		/// <summary>
		/// 
		/// </summary>
		Tab,
		/// <summary>
		/// 
		/// </summary>
		RightAligned
	}

	/// <summary>
	/// 
	/// </summary>
	public enum StartLine
	{
		/// <summary>
		/// 
		/// </summary>
		Same,
		/// <summary>
		/// 
		/// </summary>
		Next
	}
}