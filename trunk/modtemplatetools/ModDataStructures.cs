/***************************************************************************
 *                           ModDataStructures.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: ModDataStructures.cs,v 1.1 2005-10-09 11:19:37 smithydll Exp $
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
	public class PropertyLang : System.Collections.IEnumerable
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
			//return keyList.GetEnumerator();
			return keyList.Keys.GetEnumerator();
		}

		/// <summary>
		/// 
		/// </summary>
		/*public string[] Languages
		{
			get
			{
				return (string[])((ArrayList)keyList.Keys).ToArray(System.Type.GetType("System.String"));
			}
		}*/

		/// <summary>
		/// 
		/// </summary>
		public PropertyLang()
		{
			keyList = new StringDictionary();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_value"></param>
		/// <param name="_language"></param>
		public PropertyLang(string _value, string _language)
		{
			keyList = new StringDictionary();
			keyList.Add(_language, _value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_value"></param>
		public PropertyLang(string _value)
		{
			keyList = new StringDictionary();
			keyList.Add(PhpbbMod.DefaultLanguage, _value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_value"></param>
		/// <param name="_language"></param>
		public void AddLanguage(string _value, string _language)
		{
			keyList.Add(_language, _value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_language"></param>
		/// <returns></returns>
		public string GetValue(string _language)
		{
			if (keyList.ContainsKey(_language))
				return keyList[_language];
			return "";
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
		/// Set the default language value
		/// </summary>
		public void SetValue(string _value)
		{
			keyList[PhpbbMod.DefaultLanguage] = _value;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_value"></param>
		/// <param name="_language"></param>
		public void SetValue(string _value, string _language)
		{
			keyList[_language] = _value;
		}

		/// <summary>
		/// Property value
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

		/// <summary>
		/// 
		/// </summary>
		public string this[string _language]
		{
			get
			{
				if (keyList.ContainsKey(_language))
					return keyList[_language];
				return "";
			}
			set
			{
				keyList[_language] = value;
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
	public class ModActions : System.Collections.IList
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
		public object this[int index]
		{
			get
			{
				return actions[index];
			}
			set
			{
				actions[index] = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="o"></param>
		public int Add(object o)
		{
			return Add((ModAction)o);
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
		/// <param name="newAction"></param>
		public void AddEntry(ModAction newAction)
		{
			actions.Add(newAction);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="BeforeAction"></param>
		/// <param name="o"></param>
		/// <returns></returns>
		public void Insert(int BeforeAction, Object o)
		{
			Insert(BeforeAction, (ModAction)o);
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
		/// <param name="newaction"></param>
		/// <param name="BeforeAction"></param>
		public void AddEntry(ModAction newaction, int BeforeAction)
		{
			actions.Insert(BeforeAction, newaction);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		public void RemoveEntry(int index)
		{
			actions.RemoveAt(index);
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
		public void Remove(Object o)
		{
			actions.Remove(o);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public int IndexOf(Object o)
		{
			return IndexOf((ModAction)o);
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

}