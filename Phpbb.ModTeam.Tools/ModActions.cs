/***************************************************************************
 *                               ModActions.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: ModActions.cs,v 1.2 2007-07-23 11:17:27 smithydll Exp $
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
		public string Type;
		/// <summary>
		/// 
		/// </summary>
		public string Body;
		/// <summary>
		/// Deprecated
		/// </summary>
		public string BeforeComment;
		/// <summary>
		/// This is comment
		/// </summary>
		public StringLocalised AfterComment;
		/// <summary>
		/// For debugging, not used structually
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
		/// <param name="type">Type</param>
		/// <param name="body">Body</param>
		/// <param name="beforeComment">Before Comment</param>
		/// <param name="afterComment">After Comment</param>
		/// <param name="startLine">Start line</param>
		/// <param name="modifier">Modifier</param>
		public ModAction(string type, string body, string beforeComment, string afterComment, int startLine, string modifier)
		{
			this.Type = type;
			this.Body = body;
			this.BeforeComment = beforeComment;
			this.AfterComment = new StringLocalised(afterComment);
			this.StartLine = startLine;
			this.Modifier = modifier;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type">Type</param>
		/// <param name="body">Body</param>
		/// <param name="beforeComment">Before Comment</param>
		/// <param name="afterComment">After Comment</param>
		/// <param name="startLine">Start line</param>
		/// <param name="modifier">Modifier</param>
		public ModAction(string type, string body, string beforeComment, StringLocalised afterComment, int startLine, string modifier)
		{
			this.Type = type;
			this.Body = body;
			this.BeforeComment = beforeComment;
			this.AfterComment = afterComment;
			this.StartLine = startLine;
			this.Modifier = modifier;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type">Type</param>
		/// <param name="body">Body</param>
		/// <param name="beforeComment">Before Comment</param>
		/// <param name="afterComment">After Comment</param>
		/// <param name="startLine">Start line</param>
		public ModAction(string type, string body, string beforeComment, string afterComment, int startLine)
		{
			this.Type = type;
			this.Body = body;
			this.BeforeComment = beforeComment;
			this.AfterComment = new StringLocalised(afterComment);
			this.StartLine = startLine;
			this.Modifier = null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type">Type</param>
		/// <param name="body">Body</param>
		/// <param name="beforeComment">Before Comment</param>
		/// <param name="afterComment">After Comment</param>
		/// <param name="startLine">Start line</param>
		public ModAction(string type, string body, string beforeComment, StringLocalised afterComment, int startLine)
		{
			this.Type = type;
			this.Body = body;
			this.BeforeComment = beforeComment;
			this.AfterComment = afterComment;
			this.StartLine = startLine;
			this.Modifier = null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type">Type</param>
		/// <param name="body">Body</param>
		/// <param name="afterComment">After Comment</param>
		/// <param name="modifier">Modifier</param>
		public ModAction(string type, string body, string afterComment, string modifier)
		{
			this.Type = type;
			this.Body = body;
			this.BeforeComment = null;
			this.AfterComment = new StringLocalised(afterComment);
			this.StartLine = 0;
			this.Modifier = modifier;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type">Type</param>
		/// <param name="body">Body</param>
		/// <param name="afterComment">After Comment</param>
		/// <param name="modifier">Modifier</param>
		public ModAction(string type, string body, StringLocalised afterComment, string modifier)
		{
			this.Type = type;
			this.Body = body;
			this.BeforeComment = null;
			this.AfterComment = afterComment;
			this.StartLine = 0;
			this.Modifier = modifier;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type">Type</param>
		/// <param name="body">Body</param>
		/// <param name="afterComment">After Comment</param>
		public ModAction(string type, string body, string afterComment)
		{
			this.Type = type;
			this.Body = body;
			this.BeforeComment = null;
			this.AfterComment = new StringLocalised(afterComment);
			this.StartLine = 0;
			this.Modifier = null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type">Type</param>
		/// <param name="body">Body</param>
		/// <param name="afterComment">After Comment</param>
		public ModAction(string type, string body, StringLocalised afterComment)
		{
			this.Type = type;
			this.Body = body;
			this.BeforeComment = null;
			this.AfterComment = afterComment;
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
			MODBuild += "#";
			MODBuild += "\n#-----[ " + Type + " ]------------------------------------------";
			MODBuild += "\n#";
			if (!(AfterComment == null || AfterComment.GetValue() == "\n")) 
			{
				string[] ACsplit = AfterComment.GetValue().Replace("\r", "").Split('\n');
				for (int j = 0; j < ACsplit.Length; j++) 
				{
					if (!((ACsplit[j] == "" && j == 0))) 
					{
						MODBuild += "\n# " + ACsplit[j];
					}
				}
			}
			MODBuild += "\n" + Body;
			return MODBuild;
		}

		/// <summary>
		/// Deep compare
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(ModAction)) return false;
			ModAction ma = (ModAction)obj;
			if (!Type.Equals(ma.Type)) return false;
			if (!Body.Equals(ma.Body)) return false;
			if (!AfterComment.Equals(ma.AfterComment)) return false;
			if (!BeforeComment.Equals(ma.BeforeComment)) return false;
			if (!(Modifier == null && ma.Modifier == null))
			{
				if (!Modifier.Equals(ma.Modifier)) return false;
			}
			// ignore start line
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
	public class ModActions : System.Collections.IEnumerable, System.Collections.ICollection
	{
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
			if (o.GetType() != typeof(ModAuthor)) return false;
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

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder returnValue = new StringBuilder();
			foreach (ModAction ma in actions)
			{
				if (ma.Type != null) returnValue.Append("\n" + ma.ToString());
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
			if (obj.GetType() != typeof(ModActions)) return false;
			ModActions ma = (ModActions)obj;
			if (actions.Count != ma.actions.Count) return false;
			for (int i = 0; i < actions.Count; i++)
			{
				if (!((ModAction)actions[i]).Equals(ma.actions[i])) return false;
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

}