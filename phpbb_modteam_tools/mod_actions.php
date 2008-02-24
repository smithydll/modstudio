<?php
/*
 * Phpbb.ModTeam.Tools (PHP)
 * http://smithydll.id.au/
 * Copyright © 2007, David Lachlan Smith
 *
 * $Id: mod_actions.php,v 1.1 2008-02-24 20:16:03 smithydll Exp $
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License version 3 as
 * published by the Free Software Foundation.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

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
class mod_action
{
	/// <summary>
	///
	/// </summary>
	var $type; // string
	/// <summary>
	///
	/// </summary>
	var $body; // string
	/// <summary>
	/// Deprecated
	/// </summary>
	var $before_comment; // string
	/// <summary>
	/// This is comment
	/// </summary>
	var $after_comment; // string_localised
	/// <summary>
	/// For debugging, not used structually
	/// </summary>
	var $start_line; // int
	/// <summary>
	/// Modifier is a variable that modifies the behaviour of an actions, for example regex can
	/// modify a FIND to do a regular expression based find.
	/// </summary>
	var $modifier; // string

	/*/// <summary>
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
		this.AfterComment = new string_localised(afterComment);
		this.StartLine = startLine;
		this.Modifier = modifier;
	}*/

	/// <summary>
	///
	/// </summary>
	/// <param name="type">Type</param>
	/// <param name="body">Body</param>
	/// <param name="beforeComment">Before Comment</param>
	/// <param name="afterComment">After Comment</param>
	/// <param name="startLine">Start line</param>
	/// <param name="modifier">Modifier</param>
	function mod_action($type, $body, $before_comment = null, $after_comment = null, $start_line = 0, $modifier = null)
	{
		$this->type = $type;
		$this->body = $body;
		$this->before_comment = $before_comment;
		if (get_class($after_comment) != "string_localised")
		{
			$this->after_comment = new string_localised($after_comment);
		}
		else
		{
			$this->after_comment = $after_comment;
		}
		$this->start_line = $start_line;
		$this->modifier = $modifier;
	}

	/*/// <summary>
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
		this.AfterComment = new string_localised(afterComment);
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
	public ModAction(string type, string body, string beforeComment, string_localised afterComment, int startLine)
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
		this.AfterComment = new string_localised(afterComment);
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
	public ModAction(string type, string body, string_localised afterComment, string modifier)
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
		this.AfterComment = new string_localised(afterComment);
		this.StartLine = 0;
		this.Modifier = null;
	}

	/// <summary>
	///
	/// </summary>
	/// <param name="type">Type</param>
	/// <param name="body">Body</param>
	/// <param name="afterComment">After Comment</param>
	public ModAction(string type, string body, string_localised afterComment)
	{
		this.Type = type;
		this.Body = body;
		this.BeforeComment = null;
		this.AfterComment = afterComment;
		this.StartLine = 0;
		this.Modifier = null;
	}*/

	/// <summary>
	///
	/// </summary>
	/// <returns></returns>
	function to_string() // string
	{
		// TODO: niceify this
		$mod_build = ''; // string
		$mod_build += "#";
		$mod_build += "\n#-----[ " + $this->type + " ]------------------------------------------";
		$mod_build += "\n#";
		if (!($this->after_comment == null || $this->after_comment->get_value() == "\n"))
		{
			$after_comment_split = explode('\n', str_replace("\r", "", $this->after_comment->get_value())); // string[]
			for ($j = 0; $j < count($after_comment_split); $j++)
			{
				if (!(($after_comment_split[$j] == "" && $j == 0)))
				{
					$mod_build += "\n# " + $after_comment_split[$j];
				}
			}
		}
		$mod_build += "\n" + $this->body;
		return $mod_build;
	}

	/// <summary>
	/// Deep compare
	/// </summary>
	/// <param name="obj"></param>
	/// <returns></returns>
	function equals($obj) // bool
	{
		if (get_class($obj) != "ModAction") return false;
		$ma = $obj; // ModAction
		if ($this->type != $ma->type) return false;
		if ($this->body != $ma->body) return false;
		if (!$this->after_comment->equals($ma->after_comment)) return false;
		if ($this->before_comment != $ma->before_comment) return false;
		if (!($this->modifier == null && $ma->modifier == null))
		{
			if ($this->modifier != $ma->modifier) return false;
		}
		// ignore start line
		return true;
	}

	/// <summary>
	///
	/// </summary>
	/// <returns></returns>
	/*public override int GetHashCode()
	{
		return base.GetHashCode();
	}*/


}

/// <summary>
///
/// </summary>
class mod_actions //: System.Collections.IEnumerable, System.Collections.ICollection
{
	var $actions; // ArrayList

	/// <summary>
	///
	/// </summary>
	/*public bool IsSynchronized
	{
		get
		{
			return actions.IsSynchronized;
		}
	}*/

	/// <summary>
	///
	/// </summary>
	/*public object SyncRoot
	{
		get
		{
			return actions.SyncRoot;
		}
	}*/

	/// <summary>
	///
	/// </summary>
	function get_count()
	{
		return count($this->actions);
	}

	/// <summary>
	///
	/// </summary>
	/// <returns></returns>
	/*public System.Collections.IEnumerator GetEnumerator()
	{
		return actions.GetEnumerator();
	}*/

	/// <summary>
	///
	/// </summary>
	/// <param name="_array"></param>
	/// <param name="_int"></param>
	/*public void CopyTo(System.Array _array, int _int)
	{
		actions.CopyTo(_array, _int);
	}*/

	/// <summary>
	///
	/// </summary>
	function mod_actions()
	{
		$this->actions = array(); // new ArrayList()
	}

	/// <summary>
	///
	/// </summary>
	function clear() // void
	{
		$this->actions = array();
	}

	/// <summary>
	///
	/// </summary>
	/// <param name="o"></param>
	/// <returns></returns>
	function contains($o) // bool
	{
		if (get_class($o) != "ModAction") return false;
		return (array_intersect($this->actions, array($o)) > 0);
	}

	/// <summary>
	///
	/// </summary>
	/// <param name="action"></param>
	/// <returns></returns>
	/*public bool Contains(ModAction action)
	{
		return actions.Contains(action);
	}*/

	/// <summary>
	///
	/// </summary>
	/*public ModAction this[int index]
	{
		get
		{
			return (ModAction)actions[index];
		}
		set
		{
			actions[index] = (ModAction)value;
		}
	}*/

	function get($index)
	{
		return $this->actions[$index];
	}

	function set($value, $index)
	{
		$this->actions[$index] = $value;
	}

	/// <summary>
	///
	/// </summary>
	/// <param name="newaction"></param>
	function add($new_action) // int
	{
		return $this->actions[] = $new_action;
	}

	/// <summary>
	///
	/// </summary>
	/// <param name="BeforeAction"></param>
	/// <param name="newaction"></param>
	/// <returns></returns>
	function insert($before_action, $new_action) // void
	{
		array_splice($this->actions, $before_action, count($this->actions), array_merge(array($new_action), array_slice($this->actions, $before_action)));
	}

	/// <summary>
	///
	/// </summary>
	/// <param name="index"></param>
	function remove_at($index) // void
	{
		// TODO: verify
		//$this->actions->remove_at(index);
		unset($this->actions[$index]);
	}

	/// <summary>
	///
	/// </summary>
	/// <param name="o"></param>
	function remove($o) // void
	{
		unset($o);
	}

	/// <summary>
	///
	/// </summary>
	/// <param name="action"></param>
	/// <returns></returns>
	function index_of($action) // int
	{
		// TODO: verify, php4.2.0
		return array_search($action,$this->actions);
	}

	/// <summary>
	///
	/// </summary>
	/*public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}*/

	/// <summary>
	///
	/// </summary>
	/*public bool IsFixedSize
	{
		get
		{
			return false;
		}
	}*/

	/// <summary>
	///
	/// </summary>
	/// <returns></returns>
	function to_string() // string
	{
		$return_value = ''; // StringBuilder
		foreach ($this->actions as $ma)
		{
			if ($ma->type != null) $return_value .= "\n" . $ma->to_string();
		}
		return $return_value;
	}

	/// <summary>
	/// Deep compare
	/// </summary>
	/// <param name="obj"></param>
	/// <returns></returns>
	function equals($obj) // bool
	{
		if (get_class($obj) != "mod_actions") return false;
		$ma = $obj;
		if (count($this->actions) != count($ma->actions)) return false;
		for ($i = 0; $i < count($this->actions); $i++)
		{
			if (!$this->actions[$i]->equals($ma->actions[$i])) return false;
		}
		return true;
	}

	/// <summary>
	///
	/// </summary>
	/// <returns></returns>
	/*public override int GetHashCode()
	{
		return base.GetHashCode();
	}*/
}

?>