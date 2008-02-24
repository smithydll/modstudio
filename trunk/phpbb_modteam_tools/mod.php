<?php
/*
 * Phpbb.ModTeam.Tools (PHP)
 * http://smithydll.id.au/
 * Copyright © 2007, David Lachlan Smith
 *
 * $Id: mod.php,v 1.1 2008-02-24 20:16:03 smithydll Exp $
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

// namespace Phpbb.ModTeam.Tools

/// <summary>
/// Summary description for mod.
/// </summary>
class mod //extends IMod // PHP5 // abstract
{
	/// <summary>
	/// How the 'mod Description' field is to be indented.
	/// </summary>
	var $description_indent; //CodeIndents

	/// <summary>
	/// How the 'Author Notes' field is to be indented.
	/// </summary>
	var $author_notes_indent; //CodeIndents

	/// <summary>
	/// Which line of the 'Author Notes' field the notes start on.
	/// </summary>
	var $author_notes_start_line; //StartLine

	/// <summary>
	/// How the 'Files To Edit' section is to be indented.
	/// </summary>
	var $mod_files_to_edit_indent; //CodeIndents

	/// <summary>
	/// How the 'Included Files' section is to be indented.
	/// </summary>
	var $mod_included_files_indent; //CodeIndents

	/// <summary>
	/// Returns true if there were problems <i>detected</i> parsing the mod.
	/// </summary>
	var $parse_errors; //bool

	/// <summary>
	/// Contains the header information for the mod.
	/// </summary>
	var $header; //mod_header

	/// <summary>
	/// Contains the actions for the mod.
	/// </summary>
	var $actions; //mod_actions

	/// <summary>
	///
	/// </summary>
	var $last_read_format = ModFormats_Modx; //ModFormats | protected
	/// <summary>
	///
	/// </summary>
	var $text_template_read_only = false; //bool | protected
	/// <summary>
	///
	/// </summary>
	var $default_language = 'en-gb'; //string | protected | static

	/// <summary>
	///
	/// </summary>
	var $newline = "\n"; //char | protected | const
	/// <summary>
	///
	/// </summary>
	var $win_new_line = "\r\n"; //string | protected | const

	// if using trim() or rtrim(), omit the 2nd paramater as by default it trims these characters anyway
	// http://au.php.net/manual/en/function.trim.php
	// http://au.php.net/manual/en/function.rtrim.php
	/// <summary>
	///
	/// </summary>
	var $trim_chars = " \t\n\r\x0B"; //char[] | protected

	/// <summary>
	///
	/// </summary>
	function  mod() // constructor
	{
		$this->text_template_read_only = true;
		$this->header = new mod_header();
		$this->header->authors =  new mod_authors();
		$this->header->history = new mod_history();
		$this->header->included_files = array();
		$this->header->files_to_edit = array();
		$this->header->version = new mod_version();
		$this->header->phpbb_version = null;
		$this->actions = new mod_actions();
	}

	/// <summary>
	/// Estimate an Installation Time for this mod.
	/// </summary>
	function update_installation_time() //void
	{
		$total_install_time = 126;
		foreach ($this->actions->actions as $e)
		{
			switch ($e->type)
			{
				case "OPEN":
					$total_install_time += 27;
					break;
				case "SQL":
					$total_install_time += 50;
					break;
				case "COPY":
					$total_install_time += count(explode("\n", $e->body)) * 5;
					break;
				case "FIND":
				case "IN-LINE FIND":
					$total_install_time += 12;
					break;
				case "AFTER, ADD":
				case "BEFORE, ADD":
				case "REPLACE WITH":
				case "INCREMENT":
				case "IN-LINE AFTER, ADD":
				case "IN-LINE BEFORE, ADD":
				case "IN-LINE REPLACE WITH":
				case "IN-LINE INCREMENT":
					$total_install_time += 18;
					break;
				case "DIY INSTRUCTIONS":
					$total_install_time += 60;
					break;
			}
		}
		$this->header->installation_time = $total_install_time;
	}

	/// <summary>
	/// Update Included files for this mod.
	/// </summary>
	function update_included_files() //void
	{
		$this->header->included_files = array();
		for ($i = 0; $i <count($this->actions->actions); $i++)
		{
			if ($this->actions->actions[$i]->type == "COPY")
			{
				$lines = explode("\n", $this->actions->actions[$i]->body);
				foreach ($lines as $line)
				{
					if (strpos(strtolower(trim($line)), "copy") === 0)
					{
						$matches = '';
						preg_match("#copy (.+) to#", rtrim(trim($line)), $matches);
						$this->header->included_files[] = (str_replace(" to", "", str_replace("copy ", "", $matches[0])));
					}
				}
			}
		}
	}

	/// <summary>
	/// Update Files to Edit for this mod.
	/// </summary>
	function update_files_to_edit() //void
	{
		$this->header->files_to_edit = array();
		foreach ($this->actions->actions as $e)
		{
			if ($e->type == 'OPEN')
			{
				$this->header->files_to_edit[] = (rtrim(trim($e->body)));
			}
		}
	}

	/// <summary>
	///
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	function installation_level_parse($input) //ModInstallationLevel | static
	{
		$trim_chars = '()';
		if (strtoupper(rtrim(trim($input, $trim_chars), $trim_chars)) == 'EASY')
		{
			return ModInstallationLevel_Easy;
		}
		else if (strtoupper(rtrim(trim($input, $trim_chars), $trim_chars)) == 'MODERATE')
		{
			return ModInstallationLevel_Intermediate;
		}
		else if (strtoupper(rtrim(trim($input, $trim_chars), $trim_chars)) == 'INTERMEDIATE')
		{
			return ModInstallationLevel_Intermediate;
		}
		else if (strtoupper(rtrim(trim($input, $trim_chars), $trim_chars)) == 'HARD')
		{
			return ModInstallationLevel_Advanced;
		}
		else if (strtoupper(rtrim(trim($input, $trim_chars), $trim_chars)) == 'ADVANCED')
		{
			return ModInstallationLevel_Advanced;
		}
		else
		{
			return ModInstallationLevel_Easy;
		}
	}

	function installation_level_to_string($input)
	{
		switch ($input)
		{
			case ModInstallationLevel_Easy:
				return "Easy";
				break;
			case ModInstallationLevel_Intermediate:
				return "Intermediate";
				break;
			case ModInstallationLevel_Advanced:
				return "Advanced";
				break;
		}
	}

	/// <summary>
	///
	/// </summary>
	function get_default_language() //static
	{
		return 'en-gb';//$this->default_language;
	}

	/// <summary>
	///
	/// </summary>
	function set_default_language($value) //static
	{
		$this->default_language = $value;
	}

	/// <summary>
	///
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	function string_to_seconds($input) //int | static
	{
		$get_ints = explode(' ', $input);
		$seconds = 0;
		for ($i = 0; $i < count($get_ints); $i++)
		{
			if (stristr($get_ints[$i], "minute"))
			{
				$seconds += (int)(preg_replace("#^([0-9]+)\\-#", "", preg_replace("#^([A-Za-z~ ]*?)([0-9\\.]+?)([A-Za-z ]*?)$#", "\\2", $get_ints[$i - 1]))) * 60;
			}
			if (stristr($get_ints[$i], "second"))
			{
				$seconds += (int)(preg_replace("#^([A-Za-z~ ]*?)([0-9\\.]+?)([A-Za-z ]*?)$#", "\\2", $get_ints[$i - 1]));
			}
			if (stristr($get_ints[$i], "hour"))
			{
				$seconds += (int)(preg_replace("^([A-Za-z~ ]*?)([0-9\\.]+?)([A-Za-z ]*?)$#", "\\2", $get_ints[$i - 1])) * 60 * 60;
			}
		}
		return $seconds;
	}

	/// <summary>
    ///
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    function string_to_version_stage($input) // public static VersionStage
    {
        switch ($input)
        {
            case "alpha":
                return VersionStage_Alpha;
            case "beta":
                return VersionStage_Beta;
            case "release-candidate":
                return VersionStage_ReleaseCandidate;
            case "gamma":
                return VersionStage_Gamma;
            case "delta":
                return VersionStage_Delta;
        }
        return VersionStage_Stable;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="stage"></param>
    /// <returns></returns>
    function version_stage_to_string($stage) // public static string
    {
        switch ($stage)
        {
            case VersionStage_Alpha:
                return "alpha";
            case VersionStage_Beta:
                return "beta";
            case VersionStage_ReleaseCandidate:
                return "release-candidate";
            case VersionStage_Gamma:
                return "gamma";
            case VersionStage_Delta:
                return "delta";
            case VersionStage_Stable:
                return "stable";
        }
        return "stable";
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="stage"></param>
    /// <returns></returns>
    function version_stage_to_char($stage) //public static string
    {
        switch ($stage)
        {
            case VersionStage_Alpha:
                return "A";
            case VersionStage_Beta:
                return "B";
            case VersionStage_ReleaseCandidate:
                return "RC";
            case VersionStage_Gamma:
                return "C";
            case VersionStage_Delta:
                return "D";
            case VersionStage_Stable:
                return "";
        }
        return "";
    }

	/// <summary>
	///
	/// </summary>
	/// <param name="fileName"></param>
	function read($file_name) //void | abstract
	{
		exit("Cannot call Abstract methods on an Abstract type");
	}

	/// <summary>
	///
	/// </summary>
	/// <param name="fileName"></param>
	function write($file_name) //void | abstract
	{
		exit("Cannot call Abstract methods on an Abstract type");
	}

	/// <summary>
	///
	/// </summary>
	/// <returns></returns>
	function to_string($file_name) //string | abstract
	{
		exit("Cannot call methods on an Abstract type");
	}

	/// <summary>
	///
	/// </summary>
	/// <returns></returns>
	function validate($file_name) //Validation.report | abstract
	{
		exit("Cannot call Abstract methods on an Abstract type");
	}

	/// <summary>
	///
	/// </summary>
	function get_last_read_format()
	{
		return $this->last_read_format;
	}

	/// <summary>
	///
	/// </summary>
	function set_last_read_format($value)
	{
		$this->last_read_format = $value;
	}

	//region File Handling

	/// <summary>
	/// Open a text file
	/// </summary>
	/// <param name="fileName"></param>
	/// <returns></returns>
	function open_text_file($file_name) //string | protected | static
	{
		if (!($fp = fopen($file_name, 'rb')))
		{
			exit('error reading file');
		}
		$file_contents = fread($fp, filesize($file_name));
		fclose($fp);
		return $file_contents;
	}

	/// <summary>
	/// Save a text file
	/// </summary>
	/// <param name="fileToSave"></param>
	/// <param name="fileName"></param>
	function save_text_file($file_to_save, $file_name) //void | protected | static
	{
		//TODO: implement this
	}

	//endregion

}

?>