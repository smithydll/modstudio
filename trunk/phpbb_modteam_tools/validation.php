<?php
/*
 * Phpbb.ModTeam.Tools (PHP)
 * http://smithydll.id.au/
 * Copyright © 2007, David Lachlan Smith
 *
 * $Id: validation.php,v 1.1 2008-02-24 20:16:03 smithydll Exp $
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

define('Validation_notice', "[b][ [color=blue]NOTICE[/color] ][/b]");
define('Validation_error', "[b][ [color=red]ERROR[/color] ][/b]");
define('Validation_fail', "[b][ [color=red]FAIL[/color] ][/b]");
define('Validation_warning', "[b][ [color=orange]WARNING[/color] ][/b]");
define('Validation_ok', "[b][ [color=green]OK[/color] ][/b]");
define('Validation_pass', "[b][ [color=green]PASS[/color] ][/b]");
define('Validation_info', "[b][ [color=purple]INFO[/color] ][/b]");

// because PHP4 doesn't support static functions
$validator_actions = array();
$validator_phpbb_file_list = array();

/// <summary>
/// Summary description for text_mod.
/// </summary>
class validator
{
	//var $PhpbbFileList;
	//var $actions; // Hashtable

	function get_phpbb_file_list()
	{
		global $validator_phpbb_file_list;
		return $validator_phpbb_file_list;
	}

	function set_phpbb_file_list($value)
	{
		global $validator_phpbb_file_list;
		$validator_phpbb_file_list = $value;
	}

	function get_actions()
	{
		global $validator_actions;
		return $validator_actions;
	}

	function set_actions($value)
	{
		global $validator_actions;
		$validator_actions = $value;
	}

	/// <summary>
	///
	/// </summary>
	function fill_actions()
	{
		global $validator_actions;
		if (count($validator_actions) == 0)
		{
			$validator_actions['COPY'] = ModActionType_File;
			$validator_actions['OPEN'] = ModActionType_File;
			$validator_actions['FIND'] = ModActionType_Find;
			$validator_actions['REPLACE WITH'] = ModActionType_Edit;
			$validator_actions['AFTER, ADD'] = ModActionType_Edit;
			$validator_actions['BEFORE, ADD'] = ModActionType_Edit;
			$validator_actions['IN-LINE FIND'] = ModActionType_InLineFind;
			$validator_actions['IN-LINE REPLACE WITH'] = ModActionType_InLineEdit;
			$validator_actions['IN-LINE AFTER, ADD'] = ModActionType_InLineEdit;
			$validator_actions['IN-LINE BEFORE, ADD'] = ModActionType_InLineEdit;
			$validator_actions['SAVE/CLOSE ALL FILES'] = ModActionType_File;
			$validator_actions['SQL'] = ModActionType_Sql;
			$validator_actions['INCREMENT'] = ModActionType_Edit;
			$validator_actions['IN-LINE INCREMENT'] = ModActionType_InLineEdit;
			$validator_actions['DIY INSTRUCTIONS'] = ModActionType_Instruction;
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
	/// <p>The output is XHTML compliant (only if valid BBcode is given).</p>
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	function bbcode_to_html($input) // public static string
	{
		$input = str_replace("&", "&amp;", $input);
		$input = str_replace("<", "&lt;", $input);
		$input = str_replace(">", "&gt;", $input);

		$input = str_replace("[b]", "<strong>", $input);
		$input = str_replace("[/b]", "</strong>", $input);

		$input = str_replace("[i]", "<em>", $input);
		$input = str_replace("[/i]", "</em>", $input);

		$input = str_replace("[u]", "<u>", $input);
		$input = str_replace("[/u]", "</u>", $input);

		$input = str_replace("[code]","<pre><code>", $input);
		$input = str_replace("[/code]","</code></pre>", $input);

		$input = str_replace("[quote]","<blockquote>", $input);
		$input = preg_replace("#\\[quote\\=\"(.+?)\"\\]#", "<strong>\\1</strong><blockquote>", $input);
		$input = str_replace("[/quote]","</blockquote>", $input);

		$input = str_replace("\"", "&quot;", $input);

		$input = preg_replace("#\\[color\\=(\\#([A-Fa-f0-9]+?)|green|orange|red|purple|blue)\\]#", "<span style=\"color: \\1\">", $input);
		$input = str_replace("[/color]","</span>", $input);

		$input = preg_replace("#\\[size\\=([0-9]+?)\\]#", "<span style=\"font-size: \\1pt\">", $input);
		$input = str_replace("[/size]","</span>", $input);

		$input = str_replace("\n", "<br />\n", $input);

		return $input;
	}

	/// <summary>
	///
	/// </summary>
	/// <param name="language"></param>
	/// <param name="version"></param>
	function load_phpbb_file_list($language, $version = null) //public static void
	{
		global $validator_phpbb_file_list, $phpbb_mod_team_tools_path;
		if ($version == null)
		{
			$version = new mod_version(2,0,0);
		}

		$validator_phpbb_file_list = array();
		if ($version->major == 2 && $version->minor == 0)
		{
			$validator_phpbb_file_list = explode("\n", str_replace("\r\n", "\n", mod::open_text_file($phpbb_mod_team_tools_path . "files.txt")));
		}
		elseif ($version->major == 3 && $version->minor == 0)
		{
			$validator_phpbb_file_list = explode("\n", str_replace("\r\n", "\n", mod::open_text_file($phpbb_mod_team_tools_path . "files_3.0.txt")));
		}
		if ($language != "english")
		{
			for ($i = 0; $i < count($validator_phpbb_file_list); $i++)
			{
				$validator_phpbb_file_list[$i] = str_replace("english", language, $validator_phpbb_file_list[$i]);
			}
		}
	}
}

// namespace Phpbb.ModTeam.Tools.Validation

///<summary>
///
///</summary>
define('ReportFormat_Html', 0);
define('ReportFormat_Bbcode', 1);

class report
{
	///<summary>
	///
	///</summary>
	var $passed; // bool

	///<summary>
	///
	///</summary>
	var $warnings; // int

	///<summary>
	///
	///</summary>
	var $header_report; // string

	///<summary>
	///
	///</summary>
	var $actions_report; // string

	///<summary>
	///
	///</summary>
	var $html_report; // string

	///<summary>
	///
	///</summary>
	var $php_report; // string

	///<summary>
	///
	///</summary>
	var $dbal_report; // string

	///<summary>
	///
	///</summary>
	var $rating; // string

	/// <summary>
	///
	/// </summary>
	/// <returns>(Html|Bbcode) Format</returns>
	function to_string($format = ReportFormat_Html, $checks = true)
	{
		if ($checks)
		{
			$report = sprintf("[size=18]mod Template usage[/size]\n\n%s\n\n%s\n\n\n" .
			"[size=18]mod HTML usage[/size]\n\n%s\n\n\n" .
			"[size=18]mod DBAL usage[/size]\n\n%s\n\n\n" .
			"[size=22]Overall[/size]\n\n%s\n\n",
			$this->header_report, $this->actions_report, $this->html_report, $this->dbal_report, $this->rating);

			switch ($format)
			{
				case ReportFormat_Bbcode:
					return $report;
				case ReportFormat_Html:
					return validator::bbcode_to_html($report);
			}
			return "";
		}
		else // don't report Html and Php checks if another utility is used for them
		{
			$report = sprintf("%s\n%s\n" .
					"[size=13][b]Overall[/b][/size]\n%s\n",
					this.HeaderReport, this.ActionsReport, this.Rating);
			return $report;
		}
	}

}

define('ModActionType_Sql', 0);
define('ModActionType_File', 1);
define('ModActionType_Find', 2);
define('ModActionType_Edit', 3);
define('ModActionType_InLineFind', 4);
define('ModActionType_InLineEdit', 5);
define('ModActionType_Instruction', 6);
define('ModActionType_Null', 7);

class static_mod_actions
{
	var $action; // string
	var $type; // ModActionType

	/// <summary>
	///
	/// </summary>
	/// <param name="action"></param>
	/// <param name="type"></param>
	function static_mod_actions($action, $type)
	{
		$this->action = $action;
		$this->type =- $type;
	}

	// static
	/// <summary>
	///
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	function parse($input)
	{
		validator::fill_actions();

		if (array_key_exists($input, validator::get_actions()))
		{
			$validator_actions = validator::get_actions();
			return new static_mod_actions($input, $validator_actions[$input]);
		}
		return new static_mod_actions("", ModActionType_Null);
	}

	/// <summary>
	///
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	function get_type($input)
	{
		validator::fill_actions();
		if (array_key_exists($input, validator::get_actions()))
		{
			$validator_actions = validator::get_actions();
			return $validator_actions[$input];
		}
		return ModActionType_Null;
	}
}

?>