/***************************************************************************
 *                             DataStructures.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: DataStructures.cs,v 1.1 2006-07-03 12:49:23 smithydll Exp $
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
			keyList.Add(Mod.DefaultLanguage, value);
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
			if (keyList.ContainsKey(Mod.DefaultLanguage))
				return keyList[Mod.DefaultLanguage];
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool ContainsKey(string key)
		{
			return keyList.ContainsKey(key);
		}

		/// <summary>
		/// Deep compare
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(StringLocalised)) return false;
			StringLocalised k = (StringLocalised)obj;
			if (keyList.Count != k.keyList.Count) return false;
			foreach (string key in keyList.Keys)
			{
				if (k.ContainsKey(key))
				{
					if(k.keyList[key] != keyList[key]) return false;
				}
				else
				{
					return false;
				}
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

	/// <summary>
	/// The different supported MOD file formats.
	/// </summary>
	public enum ModFormats
	{
		/// <summary>
		/// MODX file format.
		/// </summary>
		Modx,
		/// <summary>
		/// A readable text based file format.
		/// </summary>
		TextMOD,
		/// <summary>
		/// Unix Diff, cannot be saved in this format
		/// </summary>
		Diff
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
		Intermediate,
		/// <summary>
		/// 
		/// </summary>
		Advanced
	}

	/// <summary>
	/// 
	/// </summary>
	public class ModAuthors : System.Collections.IEnumerable, System.Collections.ICollection
	{
		private ArrayList authors;
		//public ModAuthorEntry[] Authors;

		/// <summary>
		/// 
		/// </summary>
		public ModAuthors()
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
		public ModAuthor this[int index]
		{
			get
			{
				return (ModAuthor)authors[index];
			}
			set
			{
				authors[index] = (ModAuthor)value;
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
		public void Insert(int index, ModAuthor value)
		{
			authors.Insert(index, value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		public void Remove(ModAuthor value)
		{
			authors.Remove((ModAuthor)value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool Contains(ModAuthor value)
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
		public int IndexOf(ModAuthor value)
		{
			return authors.IndexOf(value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int Add(ModAuthor value)
		{
			return authors.Add((ModAuthor)value);
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

		/// <summary>
		/// Deep Compare
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(ModAuthors)) return false;
			ModAuthors as2 = (ModAuthors)obj;
			if (authors.Count != as2.authors.Count) return false;
			foreach(object a2 in as2.authors)
			{
				if (!authors.Contains(a2))
				{
					return false;
				}
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

	/// <summary>
	/// 
	/// </summary>
	public class ModAuthor : IComparable
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
		public ModAuthor()
		{
			this.UserName = "";
			this.RealName = "";
			this.Email = "";
			this.Homepage = "";
			this.AuthorFrom = -1;
			this.AuthorTo = -1;
			this.Status = ModAuthorStatus.NoneSelected;
			UpdateNA();
		}

		/// <summary>
		/// Construct an author object.
		/// </summary>
		/// <param name="UserName">username</param>
		/// <param name="RealName">real name</param>
		/// <param name="Email">e-mail</param>
		/// <param name="Homepage">homepage</param>
		public ModAuthor(string UserName, string RealName, string Email, string Homepage)
		{
			this.UserName = UserName;
			this.RealName = RealName;
			this.Email = Email;
			this.Homepage = Homepage;
			this.AuthorFrom = -1;
			this.AuthorTo = -1;
			this.Status = ModAuthorStatus.NoneSelected;
			UpdateNA();
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
		public ModAuthor(string UserName, string RealName, string Email, string Homepage, int AuthorFrom, int AuthorTo, ModAuthorStatus Status)
		{
			this.UserName = UserName;
			this.RealName = RealName;
			this.Email = Email;
			this.Homepage = Homepage;
			this.AuthorFrom = AuthorFrom;
			this.AuthorTo = AuthorTo;
			this.Status = Status;
			UpdateNA();
		}

		/// <summary>
		/// Deprecated
		/// </summary>
		/// <param name="yes"></param>
		public ModAuthor(bool yes)
		{
			throw new NotSupportedException("This constructor has been deprecated, please use the default constructor");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			string userName = (UserName == "") ? "n/a" : this.UserName;
			string email = (Email == "") ? "n/a" : this.Email;
			string realName = (RealName == "") ? "n/a" : this.RealName;
			string homepage = (Homepage == "") ? "n/a" : this.Homepage;
			return string.Format("{0} < {1} > ({2}) {3}", userName, email, realName, homepage);
		}

		/// <summary>
		/// 
		/// </summary>
		public void UpdateNA()
		{
			if (UserName.ToLower() == "n/a") UserName = "";
			if (Email.ToLower() == "n/a") Email = "";
			if (RealName.ToLower() == "n/a") RealName = "";
			if (Homepage.ToLower() == "n/a") Homepage = "";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static ModAuthor Parse(string input)
		{
			string tempTabSeparatedAuthor = "";
			try
			{
				tempTabSeparatedAuthor = Regex.Replace(input, @"^(## MOD Author(|, secondary):|)([\s]*?)((?!\s)(?!n\/a)[\w\s\=\$\.\-\|@\' ]+?|)\s<(\s|)(n\/a|[a-z0-9\(\) \.\-_\+\[\]@]+|)(\s|)>\s(\(\s{0,1}(([\w\s\.\'\-]+?)|n\/a)\s{0,1}\)|)(\s|)((([a-z]+?://){1}|)([a-z0-9\-\.,\?!%\*_\#:;~\\&$@\/=\+\(\)]+)|n\/a|)(([\s]+?)|)$", "$4\t$6\t$9\t$12", RegexOptions.IgnoreCase);
				string[] MODTempAuthor = tempTabSeparatedAuthor.Split('\t');
				//Console.Error.WriteLine(tempTabSeparatedAuthor);
				ModAuthor returnValue = new ModAuthor(MODTempAuthor[0].TrimStart(' ').TrimStart('\t'), MODTempAuthor[2], MODTempAuthor[1].TrimEnd(' '), MODTempAuthor[3]);
				returnValue.UpdateNA();
				return returnValue;
			}
			catch (Exception ex)
			{
				throw new ModAuthorParseException("Error for input case:\n\t" + input + "\n\n" + ex.ToString() + "\n\n" + tempTabSeparatedAuthor);
			}
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
			ModAuthor a2 = (ModAuthor)obj;
			return this.UserName.CompareTo(a2.UserName);
		}

		#endregion

		/// <summary>
		/// Deep compare
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(ModAuthor)) return false;
			ModAuthor a2 = (ModAuthor)obj;
			return (this.UserName.Equals(a2.UserName) &&
				this.AuthorFrom.Equals(a2.AuthorFrom) &&
				this.AuthorTo.Equals(a2.AuthorTo) &&
				this.Email.Equals(a2.Email) &&
				this.Homepage.Equals(a2.Homepage) &&
				this.RealName.Equals(a2.RealName) &&
				this.Status.Equals(a2.Status));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return base.GetHashCode ();
		}

		/// <summary>
		/// Compares only the username
		/// </summary>
		/// <param name="a2"></param>
		/// <returns></returns>
		public bool HasAuthor(ModAuthor a2)
		{
			return this.UserName.Equals(a2.UserName);
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
	/// 
	/// </summary>
	public class ModVersion
	{
		/// <summary>
		/// 
		/// </summary>
		public int Major;
		/// <summary>
		/// 
		/// </summary>
		public int Minor;
		/// <summary>
		/// 
		/// </summary>
		public int Revision;
		/// <summary>
		/// 
		/// </summary>
		public char Release;
		/// <summary>
		/// 
		/// </summary>
		public const char nullChar = '-';

		/// <summary>
		/// 
		/// </summary>
		public ModVersion()
		{
			this.Major = 1;
			this.Minor = 0;
			this.Revision = 0;
			this.Release = nullChar;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="major"></param>
		/// <param name="minor"></param>
		/// <param name="revision"></param>
		public ModVersion(int major, int minor, int revision)
		{
			this.Major = major;
			this.Minor = minor;
			this.Revision = revision;
			this.Release = nullChar;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="major"></param>
		/// <param name="minor"></param>
		/// <param name="revision"></param>
		/// <param name="release"></param>
		public ModVersion(int major, int minor, int revision, char release)
		{
			this.Major = major;
			this.Minor = minor;
			this.Revision = revision;
			this.Release = release;
		}

		/// <summary>
		/// Convert the current MOD Version to a string.
		/// </summary>
		/// <returns>A string of the form x.y.za</returns>
		public override string ToString()
		{
			if (this.Release == nullChar) 
			{
				return string.Format("{0}.{1}.{2}", this.Major.ToString(), this.Minor.ToString(), this.Revision.ToString());
			} 
			else 
			{
				return string.Format("{0}.{1}.{2}{3}", this.Major.ToString(), this.Minor.ToString(), this.Revision.ToString(), this.Release.ToString());
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
			Match inputMatch = Regex.Match(input.Trim(TrimChars), @"(\d+)\.(\d+)\.(\d+)([a-z]?)", RegexOptions.IgnoreCase);
			if (inputMatch.Groups.Count >= 5)
			{
				input = string.Format("{0}.{1}.{2}.{3}", inputMatch.Groups[1].Value, inputMatch.Groups[2].Value, inputMatch.Groups[3].Value, inputMatch.Groups[4].Value);
			}
			string[] MV = input.Split('.');
			try
			{
				if (MV.Length >= 1) 
				{
					MVersion.Major = int.Parse(MV[0]);
				}
				if (MV.Length >= 2) 
				{
					if (!(MV[1] == null)) 
					{
						MVersion.Minor = int.Parse(MV[1]);
					}
				}
				if (MV.Length >= 3) 
				{
					if (!(MV[2] == null)) 
					{
						MVersion.Revision = int.Parse(MV[2]);
					}
				}
				if (MV.Length >= 4) 
				{
					if (MV[3].Length > 0)
					{
						if (Regex.IsMatch(MV[3], "^([a-zA-Z])$") && MV[3] != "\b")
						{
							MVersion.Release = MV[3].ToCharArray()[0];
						}
						else
						{
							MVersion.Release = ModVersion.nullChar;
						}
					}
					if (MVersion.Release.GetHashCode() == 0)
					{
						MVersion.Release = ModVersion.nullChar;
					}
				} 
				else 
				{
					MVersion.Release = ModVersion.nullChar;
				}
			}
			catch (FormatException)
			{
				throw new NotAModVersionException(string.Format("Error, not a valid MOD version on input string: {0}", input));
			}
			return MVersion;
		}

		/// <summary>
		/// Deep Compare
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(ModVersion)) return false;
			ModVersion mv = (ModVersion)obj;
			if (!Major.Equals(mv.Major)) return false;
			if (!Minor.Equals(mv.Minor)) return false;
			if (!Revision.Equals(mv.Revision)) return false;
			if (!Release.Equals(mv.Release)) return false;
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

		/// <summary>
		/// Deep Compare
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(ModHistoryChangeLog)) return false;
			ModHistoryChangeLog mhcl = (ModHistoryChangeLog)obj;
			if (changeLog.Count != mhcl.changeLog.Count) return false;
			for (int i = 0; i < changeLog.Count; i++)
			{
				if (!((string)changeLog[i]).Equals(mhcl.changeLog[i])) return false;
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

		/// <summary>
		/// Deep compare
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(ModHistoryChangeLogLocalised)) return false;
			ModHistoryChangeLogLocalised mhcll = (ModHistoryChangeLogLocalised)obj;
			if (changeLogs.Count != mhcll.changeLogs.Count) return false;
			foreach (object okey in changeLogs.Keys)
			{
				if (!mhcll.changeLogs.ContainsKey(okey)) return false;
				if (!((ModHistoryChangeLog)changeLogs[okey]).Equals(mhcll.changeLogs[okey])) return false;
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

	/// <summary>
	/// A MOD History Entry
	/// </summary>
	public class ModHistoryEntry
	{
		/// <summary>
		/// 
		/// </summary>
		public ModVersion Version;
		/// <summary>
		/// 
		/// </summary>
		public System.DateTime Date;
		//public StringLocalised HistoryChanges;
		/// <summary>
		/// 
		/// </summary>
		public ModHistoryChangeLogLocalised ChangeLog;

		/// <summary>
		/// 
		/// </summary>
		public ModHistoryEntry()
		{
			this.Version = new ModVersion(0,0,0);
			this.Date = DateTime.Now;
			this.ChangeLog = new ModHistoryChangeLogLocalised();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="HistoryVersion"></param>
		/// <param name="HistoryDate"></param>
		/// <param name="HistoryChanges"></param>
		public ModHistoryEntry(ModVersion HistoryVersion, System.DateTime HistoryDate, string HistoryChanges)
		{
			this.Version = HistoryVersion;
			this.Date = HistoryDate;
			//this.HistoryChanges = new StringLocalised(HistoryChanges, PhpbbMod.DefaultLanguage);
			this.ChangeLog = new ModHistoryChangeLogLocalised();
			this.ChangeLog.Add(new ModHistoryChangeLog(), Mod.DefaultLanguage);
			this.ChangeLog[Mod.DefaultLanguage].Add(HistoryChanges);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="HistoryVersion"></param>
		/// <param name="HistoryDate"></param>
		/// <param name="HistoryChanges"></param>
		public ModHistoryEntry(ModVersion HistoryVersion, System.DateTime HistoryDate, ModHistoryChangeLogLocalised HistoryChanges)
		{
			this.Version = HistoryVersion;
			this.Date = HistoryDate;
			this.ChangeLog = HistoryChanges;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return ToString(Mod.DefaultLanguage);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="defaultLanguage"></param>
		/// <returns></returns>
		public string ToString(string defaultLanguage)
		{
			StringBuilder returnValue = new StringBuilder();
			returnValue.Append("## \n");
			returnValue.Append("## " + Date.ToString("yyyy-MM-dd") + " - Version " + Version.ToString() + "\n");
			foreach (DictionaryEntry dictEntry in ChangeLog)
			{
				string language = (string)dictEntry.Key;
				ModHistoryChangeLog mhcl = (ModHistoryChangeLog)dictEntry.Value;
				if (language == defaultLanguage)
				{
					foreach (string le in mhcl)
					{
						returnValue.Append("##      -" + le.Replace("\n", "## \n") + "\n");
					}
				}
			}
			if (ChangeLog.Count == 0)
			{
				returnValue.Append("## - \n");
			}
			return returnValue.ToString();
		}

		/// <summary>
		/// Deep Compare
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(ModHistoryEntry)) return false;
			ModHistoryEntry mhe = (ModHistoryEntry)obj;
			if (!ChangeLog.Equals(mhe.ChangeLog)) return false;
			if (!Date.Equals(mhe.Date)) return false;
			if (!Version.Equals(mhe.Version)) return false;
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

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return ToString(Mod.DefaultLanguage);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="defaultLanguage"></param>
		/// <returns></returns>
		public string ToString(string defaultLanguage)
		{
			System.Text.StringBuilder returnValue = new System.Text.StringBuilder();
			if (History.Count > 0)
			{
				returnValue.Append("##############################################################\n");
				returnValue.Append("## MOD History:\n");
				foreach (ModHistoryEntry mhe in History)
				{
					returnValue.Append(mhe.ToString(defaultLanguage));
				}
			}
			if (returnValue.Length != 0) 
			{
				returnValue.Insert(0, "\n");
				returnValue.Append("## ");
			}
			return returnValue.ToString();
		}

		/// <summary>
		/// Deep compare
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(ModHistory)) return false;
			ModHistory mh = (ModHistory)obj;
			if (History.Count != mh.History.Count) return false;
			for (int i = 0; i < History.Count; i++)
			{
				if (!((ModHistoryEntry)History[i]).Equals(mh.History[i])) return false;
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

	/// <summary>
	/// 
	/// </summary>
	public class NotAModVersionException : Exception
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public NotAModVersionException(string message) : base(message)
		{
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class TextTemplateReadOnlyModeException : Exception
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public TextTemplateReadOnlyModeException(string message) : base(message)
		{
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class ModAuthorParseException : Exception
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public ModAuthorParseException(string message) : base(message)
	{
	}
	}
}
