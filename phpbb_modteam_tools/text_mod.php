<?php
/*
 * Phpbb.ModTeam.Tools (PHP)
 * http://smithydll.id.au/
 * Copyright © 2007, David Lachlan Smith
 *
 * $Id: text_mod.php,v 1.1 2008-02-24 20:16:03 smithydll Exp $
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
/// Summary description for mod_author.
/// </summary>
class text_mod extends mod //, IMod
{
	var $text_template; //string | private

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="templatePath">Path to the templates folder.</param>
	function text_mod($template_path = null)
	{
		$this->mod();

		if ($template_path != null)
		{
			$this->text_template_read_only = false;
			$this->text_template = mod::open_text_file(Path::combine($template_path, "mod.mot"));
			@$this->text_template = preg_replace("#^\\#(.*?)(.*?)(|\r)\n\\#\\#\\#END OF HEADER\\#\\#\\#(|\r)\n#s", "", $this->text_template);
			@$this->text_template = str_replace("\r", "", $this->text_template);
		}
	}

	/// <summary>
	///
	/// </summary>
	/// <returns></returns>
	function to_string() //override | string | public
	{
		if ($this->text_template_read_only) exit("Class initated without specifying text template mot file to use. All Text Templates have been read in read only mode and cannot be saved back as a text template");
		//
		// TODO: Not elegant, fixed up a little, still needs some work.
		//

		// lets force these events to sync the mod,
		$this->update_files_to_edit();
		$this->update_included_files();
		$this->update_installation_time();

		$blank_template = $this->text_template;
		$new_mod_body = ''; // StringBuilder
		$blank_template = str_replace("<mod.title/>", $this->header->title->get_value(), $blank_template);
		$my_mod_author_string = '';
		for ($i = 0; $i < $this->header->authors->get_count(); $i++)
		{
			if ($i == 0)
			{
				$tempb = $this->header->authors->get($i);
				$my_mod_author_string = $tempb->to_string();
			}
			else
			{
				$tempb = $this->header->authors->get($i);
				$my_mod_author_string .= $this->newline . '## mod Author: ' . $tempb->to_string();
			}
		}

		$description_start_of_line = '## ';
		switch ($this->description_indent)
		{
			case CodeIndents_Space:
				// already assigned as "## ";
				break;
			case CodeIndents_Tab:
				$description_start_of_line = "##\t";
				break;
			case CodeIndents_RightAligned:
				$description_start_of_line = "##                  ";
				break;
		}

		$blank_template = str_replace("<mod.author/>", $my_mod_author_string, $blank_template);
		$blank_template = str_replace("<mod.description/>", str_replace($this->newline, $this->newline . $description_start_of_line, $this->header->description->get_value()), $blank_template);
		$blank_template = str_replace("<mod.version/>", $this->header->version->to_string(), $blank_template);
		$blank_template = str_replace("<mod.install_level/>", mod::installation_level_to_string($this->header->installation_level), $blank_template);
		$blank_template = str_replace("<mod.install_time/>", ceil($this->header->installation_time / 60) . " minutes", $blank_template);

		$my_mod_files_to_edit = null;
		$mod_files_to_edit_start_of_line = "## ";
		switch ($this->mod_files_to_edit_indent)
		{
			case CodeIndents_Space:
				// already assigned as "## ";
				break;
			case CodeIndents_Tab:
				$mod_files_to_edit_start_of_line = "##\t";
				break;
			case CodeIndents_RightAligned:
				$mod_files_to_edit_start_of_line = "##                ";
				break;
		}
		for ($i = 0; $i < count($this->header->files_to_edit); $i++)
		{
			if ($i == 0)
			{
				$my_mod_files_to_edit = $this->header->files_to_edit[$i];
			}
			else
			{
				$my_mod_files_to_edit .= $this->newline . $mod_files_to_edit_start_of_line . $this->header->files_to_edit[$i];
			}
		}
		$blank_template = str_replace("<mod.files_to_edit/>", $my_mod_files_to_edit, $blank_template);

		$my_mod_included_files = null;
		$mod_included_files_start_of_line = "## ";
		switch ($this->mod_included_files_indent)
		{
			case CodeIndents_Space:
				// already assigned as "## ";
				break;
			case CodeIndents_Tab:
				$mod_included_files_start_of_line = "##\t";
				break;
			case CodeIndents_RightAligned:
				$mod_included_files_start_of_line = "##                 ";
				break;
		}
		for ($i = 0; $i < count($this->header->included_files); $i++)
		{
			if ($i == 0)
			{
				$my_mod_included_files = $this->header->included_files[$i];
			}
			else
			{
				$my_mod_included_files .= $this->newline . $mod_included_files_start_of_line . $this->header->included_files[$i];
			}
		}
		$blank_template = str_replace("<mod.inc_files/>", $my_mod_included_files, $blank_template);
		$blank_template = str_replace("<mod.generator/>", $this->newline . "## Generator: Phpbb.ModTeam.Tools (php)", $blank_template);
		if ($this->header->license != "")
		{
			$blank_template = str_replace("<mod.license/>", $this->newline . "## License: " . $this->header->license, $blank_template);
		}
		else
		{
			$blank_template = str_replace("<mod.license/>", "", $blank_template);
		}

		$author_notes_start_of_line = "## ";
		switch ($this->author_notes_indent)
		{
			case CodeIndents_Space:
				// already assigned as "## ";
				break;
			case CodeIndents_Tab:
				$author_notes_start_of_line = "##\t";
				break;
			case CodeIndents_RightAligned:
				$author_notes_start_of_line = "##               ";
				break;
		}
		$author_start_line = "";
		if ($this->author_notes_start_line == StartLine_Next) $author_start_line = $this->newline;
		$blank_template = str_replace("<mod.author_notes/>", str_replace($this->newline, $this->newline . $author_notes_start_of_line, $author_start_line . $this->header->author_notes->get_value()), $blank_template);
		$my_mod_history = '';
		$new_my_mod_history = ''; //StringBuilder
		if (count($this->header->history->history) > 0)
		{
			$new_my_mod_history .= "##############################################################" . $this->newline;
			$new_my_mod_history .= "## mod History:" . $this->newline;
			foreach ($this->header->history->history as $mhe)
			{
				$new_my_mod_history .= "## " . $this->newline;
				$new_my_mod_history .= "## " . date("Y-m-d", $mhe->date) . " - Version " . $mhe->version->to_string() . $this->newline;
				foreach ($mhe->change_log->change_logs as $Language => $mhcl)
				{
					//$Language = $de->Key;
					//$mhcl = $de->Value;
					if ($Language == $this->default_language)
					{
						foreach ($mhcl->change_log as $le)
						{
							$new_my_mod_history .= "## -" . str_replace($this->newline, "## " . $this->newline, $le) . $this->newline;
						}
					}
				}
				if (count($mhe->change_log->change_logs) == 0)
				{
					$new_my_mod_history .= "## - " . $this->newline;
				}
			}
		}
		$my_mod_history = $new_my_mod_history;
		if (!(strlen($my_mod_history) == 0))
		{
			$my_mod_history = $this->newline . $my_mod_history;
			$my_mod_history .= "## ";
		}
		$blank_template = str_replace("<mod.history/>", $my_mod_history, $blank_template);
		$blank_template = preg_replace("/^#(.*?)(|\r)\n###END OF HEADER###(|\r)\n/s", "", $blank_template);
		// TODO: re-add EMC
		/*if (Header.ModEasymodCompatibility.VersionMinor > 0 || Header.ModEasymodCompatibility.VersionRevision > 0)
		{
			BlankTemplate = "## EasyMod " + Header.ModEasymodCompatibility.ToString() + " compliant" + Newline + BlankTemplate;
		}*/
		$new_mod_body .= $blank_template;
		//try
		//{
			foreach ($this->actions->actions as $ma)
			{
				if ($ma->type != null)
				{

					$new_mod_body .= $this->newline;
					$new_mod_body .= "#";
					$new_mod_body .= $this->newline;
					$new_mod_body .= "#-----[ " . $ma->type . " ]------------------------------------------";
					$new_mod_body .= $this->newline;
					$new_mod_body .= "#";
					if (!(($ma->after_comment == null || $ma->after_comment->get_value() == $this->newline)))
					{
						$ACsplit = explode($this->newline, preg_replace("#\r#", "", $ma->after_comment->get_value())); //string[]
						for ($j = 0; $j < count($ACsplit); $j++)
						{
							if (!(($ACsplit[$j] == "" && $j == 0)))
							{
								$new_mod_body .= $this->newline;
								$new_mod_body .= "# " . $ACsplit[$j];
							}
						}
					}
					$new_mod_body .= $this->newline;
					$new_mod_body .= $ma->body;
				}
			}
		//}
		//catch
		//{
		//}

		return $new_mod_body; // ToString
	}

	/// <summary>
	///
	/// </summary>
	/// <param name="fileName"></param>
	function read($file_name) //public | override | void
	{
		$extension = pathinfo($file_name, PATHINFO_EXTENSION);
		if ($extension == "txt" || $extension == "mod")
		{
			$text_file = mod::open_text_file($file_name);
			if (strpos($text_file, "##") === 0)
			{
				$this->read_header($text_file);
				$this->read_actions($text_file);
			}
			else
			{
				exit("Not a valid Text mod");
			}
		}
		else
		{
			exit("Cannot read file of type given");
		}
	}

    /// <summary>
    ///
    /// </summary>
    /// <param name="mod_author"></param>
    function read_string($text_mod)
    {
        if (strpos($text_mod, "##") === 0)
        {
            $this->read_header($text_mod);
            $this->read_actions($text_mod);
        }
        else
        {
            exit("Not a valid Text mod");
        }
    }

	/// <summary>
	///
	/// </summary>
	/// <param name="mod_author"></param>
	function read_header($text_mod)
	{
		//$start = Environment.TickCount; // double
		$text_mod = str_replace("\r\n", "\n", $text_mod);
		$text_mod = str_replace("\r", "\n", $text_mod);
		$text_mod_lines = explode("\n", $text_mod); // string[]
		$start_offset = 0; // int
		$e = 1; // int
		$in_multi_line_element = false; // bool
		for ($j = 1; $j <= 12; $j++)
		{
			for ($i = $start_offset; $i < count($text_mod_lines); $i++)
			{
				if (strpos($text_mod_lines[$i], "## Before Adding This mod") === 0)
				{
					$e += 1;
					$i = count($text_mod_lines) - 1;
				}

				switch ($e)
				{
					case 1:
						if (strpos($text_mod_lines[$i], "## EasyMod") === 0 || strpos($text_mod_lines[$i], "## EasyMOD") === 0 || strpos($text_mod_lines[$i], "## easymod") === 0)
						{
							//try
							//{
								$easy_mod_version = mod_version::parse(str_replace("compatible", "", str_replace("compliant", "", str_replace("## EasyMod", "", $text_mod_lines[$i]))));
								$this->header->easy_mod_compatibility = $easy_mod_version;
							//}
							//catch (NotAModVersionException)
							//{
							//	ParseErrors = true;
							//}
							$start_offset = $i + 1;
							$e++;
						}
						if (strpos($text_mod_lines[$i], "###") === 0)
						{
							$start_offset = $i + 1;
							$e++;
						}
						$i = count($text_mod_lines);
						break;
					case 2:
						if (strpos(strtoupper($text_mod_lines[$i]), "## MOD TITLE") === 0)
						{
							$this->header->title = new string_localised(trim(preg_replace("#\\#\\# MOD Title\\:#i", "", $text_mod_lines[$i]), $this->trim_chars));
							$start_offset = $i + 1;
							$e++;
							$i = count($text_mod_lines);
						}
						break;
					case 3:
						if (strpos(strtoupper($text_mod_lines[$i]), "## MOD AUTHOR") === 0)
						{
							$this->header->authors->add(mod_author::parse($text_mod_lines[$i]));
						}
						else if (strpos(strtoupper($text_mod_lines[$i]), "## MOD DESCRIPTION") === 0)
						{
							$start_offset = $i;
							$e++;
							$i = count($text_mod_lines);
						}
						break;
					case 4:
						if (strpos(strtoupper($text_mod_lines[$i]), "## MOD DESCRIPTION") === 0)
						{
							$this->header->description = new string_localised(rtrim(ltrim(ltrim(preg_replace("#\\#\\# MOD Description\\:#i", "", $text_mod_lines[$i]), ' '), "\t"), ' '));
						}
						else
						{
							if (strpos(strtoupper($text_mod_lines[$i]), "## MOD V") === 0)
							{
								$start_offset = $i;
								$e++;
								$i = count($text_mod_lines);
							}
							else
							{
								$tempii = null; // string
								if (strpos($text_mod_lines[$i], "##") === 0)
								{
									$tempii = substr($text_mod_lines[$i], 2, strlen($text_mod_lines[$i]) - 2);
									if (strpos($tempii, "\t") === 0)
									{
										$this->description_indent = CodeIndents_Tab;
									}
									else if (strpos($tempii, "    ") === 0)
									{
										$this->description_indent = CodeIndents_RightAligned;
									}
									else if (strpos($tempii, " ") === 0)
									{
										$this->description_indent = CodeIndents_Space;
									}
								}
								else
								{
									$tempii = $text_mod_lines[$i];
								}
								$tempb = $this->header->description->get($this->default_language);
								$tempb .= $this->newline . rtrim(ltrim(ltrim($tempii, ' '), "\t"), ' ');
								$this->header->description->set($tempb, $this->default_language);
							}
						}
						break;
					case 5:
						if (strpos(strtoupper($text_mod_lines[$i]), "## MOD VERSION") === 0)
						{
							$temp_mod_version = mod_version::parse(preg_replace("#\\#\\# MOD VERSION\\:([\\W]+?)([\\d]+?)\\.([\\d]+?)(\\.|)([\\d]+?|)([a-zA-Z]{0,1}?)#i", "\\2.\\3.\\5\\6", $text_mod_lines[$i]));
							$this->header->version = $temp_mod_version;
							$start_offset = $i + 1;
							$e++;
							$i = count($text_mod_lines);
						}
						break;
					case 6:
						if (strpos(strtoupper($text_mod_lines[$i]), "## INSTALLATION LEVEL") === 0)
						{
							$this->header->installation_level = mod::installation_level_parse(rtrim(ltrim(ltrim(preg_replace("#\\#\\# Installation level\\:#i", "", $text_mod_lines[$i]), ' '), "\t"), ' '));
							$start_offset = $i + 1;
							$e++;
							$i = count($text_mod_lines);
						}
						break;
					case 7:
						if (strpos(strtoupper($text_mod_lines[$i]), "## INSTALLATION TIME") === 0)
						{
							//try
							//{
								$this->header->installation_time = mod::string_to_seconds(rtrim(ltrim(ltrim(str_replace("#\\#\\# Installation Time\\:#i", "", $text_mod_lines[$i]), ' '), "\t"), ' '));
							//}
							//catch (System.FormatException)
							//{
							//	Header.InstallationTime = 0;
							//}
							$start_offset = $i + 1;
							$e++;
							$i = count($text_mod_lines);
						}
						break;
					case 8:
						if (strpos(strtoupper($text_mod_lines[$i]), "## FILES TO EDIT") === 0)
						{
							//Header.ModFilesToEdit = new string[1];
							$this->header->files_to_edit[] = rtrim(ltrim(ltrim(preg_replace("#\\#\\# Files To Edit\\:#i", "", $text_mod_lines[$i]), ' '), "\t"), ' ');
						}
						else
						{
							if (strpos(strtoupper($text_mod_lines[$i]), "## INCLUDED ") === 0)
							{
								$start_offset = $i;
								$e++;
								$i = count($text_mod_lines);
							}
							else
							{
								$tempii = null; // string
								if (strpos($text_mod_lines[$i], "##") === 0)
								{
									$tempii = substr($text_mod_lines[$i], 2, strlen($text_mod_lines[$i]) - 2);
								}
								else
								{
									$tempii = $text_mod_lines[$i];
								}

								if (strpos($tempii, "\t") === 0)
								{
									$this->mod_files_to_edit_indent = CodeIndents_Tab;
								}
								else if (strpos($tempii, "    ") === 0)
								{
									$this->mod_files_to_edit_indent = CodeIndents_RightAligned;
								}
								else if (strpos($tempii, " ") === 0)
								{
									$this->mod_files_to_edit_indent = CodeIndents_Space;
								}

								/*string[] tempA = Header.ModFilesToEdit;
								Header.ModFilesToEdit = new string[tempA.Length + 1];
								tempA.CopyTo(Header.ModFilesToEdit, 0);*/

								$this->header->files_to_edit[] = rtrim(ltrim(ltrim($tempii, ' '), "\t"), ' ');
							}
						}
						break;
					case 9:
						if (strpos(strtoupper($text_mod_lines[$i]), "## INCLUDED FILES") === 0)
						{
							//Header.ModIncludedFiles = new string[1];
							$this->header->included_files[] = rtrim(ltrim(ltrim(preg_replace("#\\#\\# Included Files\\:#i", "", $text_mod_lines[$i]), ' '), "\t"), ' ');
						}
						else
						{
							if (strpos(strtoupper($text_mod_lines[$i]), "######") === 0 || strpos(strtoupper($text_mod_lines[$i]), "## GEN") === 0 || strpos(strtoupper($text_mod_lines[$i]), "## LICENSE") === 0)
							{
								$start_offset = $i;
								$e++;
								$i = count($text_mod_lines);
							}
							else
							{
								$tempii = null; // string
								if (strpos($text_mod_lines[$i], "##") === 0)
								{
									$tempii = substr($text_mod_lines[$i], 2, strlen($text_mod_lines[$i]) - 2);
								}
								else
								{
									$tempii = $text_mod_lines[$i];
								}

								if (strpos($tempii, "\t") === 0)
								{
									$this->mod_included_files_indent = CodeIndents_Tab;
								}
								else if (strpos($tempii, "    ") === 0)
								{
									$this->mod_included_files_indent = CodeIndents_RightAligned;
								}
								else if (strpos($tempii, " ") === 0)
								{
									$this->mod_included_files_indent = CodeIndents_Space;
								}

								/*string[] tempA = Header.ModIncludedFiles;
								Header.ModIncludedFiles = new string[tempA.Length + 1];
								tempA.CopyTo(Header.ModIncludedFiles, 0);*/

								$this->header->included_files[] = rtrim(ltrim(ltrim($tempii, ' '), "\t"), ' ');
							}
						}
						break;
					case 10:
						if (strpos(strtoupper($text_mod_lines[$i]), "## LICENSE") === 0)
						{
							$this->header->license = ltrim(preg_replace("#\\#\\# LICENSE\\:#i", "", $text_mod_lines[$i]), $this->trim_chars);
							$start_offset = $i + 1;
							$e++;
							$i = count($text_mod_lines);
							break;
						}
						if (strpos(strtoupper($text_mod_lines[$i]), "## AUTHOR NOTE") === 0)
						{
							$start_offset = $i - 1;
							$e++;
							$i = count($text_mod_lines);
						}
						break;
					case 11:
						if (strpos(strtoupper($text_mod_lines[$i]), "## AUTHOR NOTE") === 0)
						{
							$this->header->author_notes = new string_localised(preg_replace("#\\#\\# Author Note(s|)\\:(\\W|)#i", "", $text_mod_lines[$i]));
							$in_multi_line_element = true;
						}
						else
						{
							if (strpos(strtoupper($text_mod_lines[$i]), "####") === 0 && $in_multi_line_element)
							{
								$start_offset = $i + 1;
								$e++;
								$in_multi_line_element = false;
								$i = count($text_mod_lines);
							}
							else
							{
								$tempii = null; // string
								if (strpos($text_mod_lines[$i], "##") === 0)
								{
									$tempii = substr($text_mod_lines[$i], 2, strlen($text_mod_lines[$i]) - 2);
								}
								else
								{
									$tempii = $text_mod_lines[$i];
								}

								if (strpos($tempii, "\t") === 0)
								{
									$this->author_notes_indent = CodeIndents_Tab;
								}
								else if (strpos($tempii, "    ") === 0)
								{
									$this->author_notes_indent = CodeIndents_RightAligned;
								}
								else if (strpos($tempii, " ") === 0)
								{
									$this->author_notes_indent = CodeIndents_Space;
								}

								$tempb = $this->header->author_notes->get($this->default_language);
								$tempb .= $this->newline . rtrim(ltrim(ltrim($tempii, ' '), "\t"), ' ');
								$this->header->author_notes->set($tempb, $this->default_language);
							}
						}
						break;
					case 12:
						if (strpos(strtoupper($text_mod_lines[$i]), "## MOD HISTORY") === 0)
						{
							$in_multi_line_element = true;
							if ($this->header->history == null) $this->header->history = new mod_history();
						}
						else
						{
							if (strpos(strtoupper($text_mod_lines[$i]), "####") === 0 && $in_multi_line_element)
							{
								$start_offset = $i;
								$e++;
								$in_multi_line_element = false;
								$i = count($text_mod_lines);
							}
							else
							{
								if (preg_match("#((([0-9\\-]+?)){3}) \\- Version#i", $text_mod_lines[$i]))
								{
									//mod_version HVersion = mod_version.Parse(Regex.Match(TextModLines[i], "([\\d]+?)\\.([\\d]+?)(\\.|)([\\d]+?|)([a-zA-Z]{1}?|)([ \\t]|)$").Value);
									//Console.WriteLine(Regex.Replace(TextModLines[i], "^\\#\\#([0-9\\- \\t]+?)Version ([\\d]+?)\\.([\\d]+?)(\\.|)([\\d]+?|)([a-zA-Z]{0,1}?)([ \\t]|)$", "$2.$3.$5$6", RegexOptions.IgnoreCase));
									$temp_history_version = ''; // mod_version
									//try
									//{
										$temp_history_version = mod_version::parse(preg_replace("#^\\#\\#([0-9\\- \\t]+?)Version ([\\d]+?)\\.([\\d]+?)(\\.|)([\\d]+?|)([a-zA-Z]{0,1}?)([ \\t]|)$#i", "\\2.\\3.\\5\\6", $text_mod_lines[$i]));
									//}
									//catch (NotAModVersionException)
									//{
									//	ParseErrors = true;
									//	HVersion = new mod_version();
									//}

									// TODO: PHP replacement for system.datetime.parse, RESEARCH, same for preg_match in this context
									$matches = array();
									preg_match("#([0-9]+)(\\/|\\\\|\\-)([0-9]+)(\\/|\\\\|\\-)([0-9]+)#", $text_mod_lines[$i], $matches);
									$this->header->history->add(new mod_history_entry($temp_history_version,strtotime($matches[0]),""));

									$upper_bound = $this->header->history->get_count() - 1; // int
									$tempb = $this->header->history->get($upper_bound);
									$tempb->change_log = new mod_history_change_log_localised();
									$tempb = $this->header->history->get($upper_bound);
									$tempb->change_log->add(new mod_history_change_log(), $this->default_language);
								}
								else
								{
									if (!(($text_mod_lines[$i] == "##" || $text_mod_lines[$i] == "## ")))
									{
										$upper_bound = $this->header->history->get_count() - 1; // int
										// only match changes if we are in a history entry
										if ($this->header->history->get_count() > 0)
										{
											$tempb = $this->header->history->get($upper_bound);
											$upper_bound_log = $tempb->change_log->get_count() - 1; // int

											$nextLine = preg_replace("/##([\\s]*)/", "", $text_mod_lines[$i]); // string
											if (strpos($nextLine, "-") === 0)
											{
												// TODO: verify this is indeed the correct structure
												$tempb = $this->header->history->get($upper_bound);
												$tempc = $tempb->change_log->get($this->default_language);
												$tempc->add(substr($nextLine, 1));
												$tempb->change_log->set($this->default_language, $tempc);
												$this->header->history->set($upper_bound, $tempb);
												$tempb = $this->header->history->get($upper_bound);
												$tempc = $tempb->change_log->get($this->default_language);
											}
											else
											{
												$tempb = $this->header->history->get($upper_bound);
												if (count($tempb->change_log->change_logs) > 0)
												{
													$tempb = $this->header->history->get($upper_bound);
													$temp = $tempb->change_log->get($this->default_language);
													$temp->change_log[$upper_bound_log] .= $this->newline . $nextLine;
													$tempb->change_log->set($this->default_language, $temp);
												}
											}
											$this->header->history->set($upper_bound, $tempb);
										}
									}
								}
							}
						}
						break;
				} // Switch
			} // For i
		} // For j
		$this->header->phpbb_version = new target_version_cases(new mod_version(2, 0, 0));
		if (strpos($this->header->author_notes->get($this->default_language), "\r\n") === 0 || strpos($this->header->author_notes->get($this->default_language), "\n") === 0)
		{
			if (strpos($this->header->author_notes->get($this->default_language), "\r\n") === 0)
			{
				$this->header->AuthorNotes.Set(substr($this->header->AuthorNotes.Get($this->default_language), 2), $this->default_language);
			}
			else if (strpos($this->header->author_notes->get($this->default_language), "\n") === 0)
			{
				$this->header->author_notes->set(substr($this->header->author_notes->get($this->default_language), 1), $this->default_language);
			}
			$this->author_notes_start_line = StartLine_Next;
		}
		else
		{
			$this->author_notes_start_line = StartLine_Same;
		}
		return;
	}

	/// <summary>
	///
	/// </summary>
	/// <param name="mod_author"></param>
	function read_actions($text_mod)
	{
		$this->actions = new mod_actions();
		$text_mod = str_replace("\r\n", "\n", $text_mod);
		$text_mod = str_replace("\r", "\n", $text_mod);
		$text_mod .= "\n#\n#-----[";
		$mod_text_lines = explode("\n", $text_mod); // string[]

		$in_mod_action = false; // bool
		$this_mod_action_body = ""; // string
		$this_mod_action_type = ""; // string
		$this_mod_action_comment = ""; // string
		$next_mod_action_comment = ""; // string
		$is_first_mod_action_line = true; // bool
		$is_first_hashed_line = true; // bool
		$first_mod_action_line = 0; // int
		$is_first_action_found = false; // bool

		$header_end_line = 0; // int
		$found = false; // bool
		for ($i = count($mod_text_lines) - 1; $i != 0 && $found == false; $i--)
		{
			if (preg_match("#^(\\#){60}#", $mod_text_lines[$i]) && $found == false)
			{
				$found = true;
				$header_end_line = $i;
			}
		}

		for ($i = $header_end_line; $i < count($mod_text_lines) - 1; $i++)
		{
			if (!(strpos($mod_text_lines[$i], "##") === 0))
			{
				if (strpos($mod_text_lines[$i], "#") === 0 && preg_match("#^\\#([\-]+)\[#", $mod_text_lines[$i + 1]) && $is_first_action_found)
				{
					$next_mod_action_comment = ltrim($next_mod_action_comment, "\n");
					$this_mod_action_comment = ltrim($this_mod_action_comment, "\n");

					switch($this_mod_action_type)
					{
						case "OPEN":
							$this_mod_action_body = trim($this_mod_action_body, $this->trim_chars);
							break;
					}
					$ma = new mod_action($this_mod_action_type, $this_mod_action_body, $next_mod_action_comment, $this_mod_action_comment, $first_mod_action_line); // ModAction
					if ($ma->type == "DIY INSTRUCTIONS")
					{
						$ma->modifier = $this->default_language;
					}
					$this->actions->add($ma);
					$this_mod_action_body = "";
					$this_mod_action_type = "";
					$this_mod_action_comment = "";
					$is_first_mod_action_line = true;
					$is_first_hashed_line = true;
					$first_mod_action_line = 0;
				}
				if (strpos($mod_text_lines[$i], "#") === 0 && !(strpos($mod_text_lines[$i - 1], "#") === 0))
				{
					$in_mod_action = false;
				}
				if ($in_mod_action)
				{
					if (strpos($mod_text_lines[$i], "#") === 0)
					{
						if (!$is_first_hashed_line)
						{
							$this_mod_action_comment .= $this->newline;
						}
						$is_first_hashed_line = false;
						$this_mod_action_comment .= ltrim(ltrim($mod_text_lines[$i], '#'), ' ');
					}
					else
					{
						if (!$is_first_mod_action_line)
						{
							$this_mod_action_body .= $this->newline;
						}
						$is_first_mod_action_line = false;
						$this_mod_action_body .= $mod_text_lines[$i];
					}
				}
				if (preg_match("#^\\#([\-]+)\[#", $mod_text_lines[$i]))
				{
					$in_mod_action = true;
					$is_first_action_found = true;
					// match to "#---[ ABC ]---   " an arbitrary amoung of end of line space and dashes
					// no end of line space is preffered as per the specs
					$this_mod_action_type = preg_replace("#^\\#([\\-]+)\\[(\\W)([A-Za-z, \\/\\-\\\\]+?) \\]([\\-]+)([ ]*)$#i", "\\3", $mod_text_lines[$i]);
					if ($first_mod_action_line == 0)
					{
						$first_mod_action_line = $i;
					}
				}
			}
		}
	}

	/// <summary>
	///
	/// </summary>
	/// <returns></returns>
	function validate($file_name, $isFile = true, $language = "english", $version = null, $checks = true) //Validation.report
	{
		if ($version == null)
		{
			$version = new mod_version(2, 0, 0);
		}
		$report = new report();

		if ($isFile)
		{
			$file_name =  mod::open_text_file($file_name); //string
		}

		$text_mod = str_replace($this->win_new_line, $this->newline, $file_name); //string
		$text_mod_lines = explode($this->newline, $text_mod); // string[]
		$header_end_line = count($text_mod_lines) - 1; // int
		validator::fill_actions();

		$start_offset = 0; // int

		$check = 0; // int
		$flag = true; // bool

		$action_validate = false; // bool
		$warn_flag = false; // bool
		$validate_flag = true; // bool
		$validate_warn_flag = true; // bool
		$find_in_line_flag = true; // bool
		$edit_in_line_flag = true; // bool
		$check = 0;
		$flag = false;
		$validate_flag = true;
		$row = 0; // int

		$found = false; // bool
		for ($i = count($text_mod_lines) - 1; $i != 0 && $found == false; $i--)
		{
			if (strpos($text_mod_lines[$i], "############################################################") === 0)
			{
				$found = true;
				$header_end_line = $i;
			}
		}

		$li = 0; // int
		for ($j = 0; $j < 14; $j++)
		{
			for ($i = $start_offset; $i < $header_end_line; $i++)
			{
				$row = $i + 1;
				$flag = false;
				if ($start_offset == $i)
				{
					if (strpos($text_mod_lines[$i], "## mod Author") === 0)
					{
						if (preg_match("#\\# mod Author(, Secondary|)#", $text_mod_lines[$i]))
						{
							if (!preg_match("#\\# mod Author: ((?!n\\/a)[\\w\\s\\=\\$\\.\\-\\|@\\'\\:\\[\\]\\(\\)<> ]+?) <( |)(n\\/a|[a-z0-9\(\\) \\.\\-_\\+\\[\\]@\\|\\*]+)( |)> (\\((?! +)(([\\w\\s\\.\\'\\-]+?)|n\\/a)(?! +)\\)|)( |)(([a-z]+?://){1}([a-z0-9\\-\\.,\\?!%\\*_\\#:;~\\\\&$@\\/=\\+\\(\\)]+)|n\\/a|)( |)$#i", $text_mod_lines[$i]))
							{
								$li = $i + 1;
								$report->header_report .= sprintf("%s Incorrect mod Author Syntax on line: %s\n[code]%s[/code]\n",
									Validation_fail, $li, $text_mod_lines[$i]);
									$validate_flag = false;
							}
						}
					}
				}
				if ($check == 0)
				{
					if (preg_match("#\\#\\# EasyMod (.+?) Compliant#i", $text_mod_lines[$i]))
					{
						$report->header_report .= sprintf("[i]## EasyMod Compliant[/i]\n",
							Validation_fail);
						$report->header_report .= "[i]EasyMod Compliant is not a ratified standard. It is not part of the mod Template.[/i]\n";
						$validate_flag = false;
					}
					$check = 1;
					$start_offset = $i + 1;
				}
				if ($check == 1)
				{
					if (strpos($text_mod_lines[$i], "## mod Title:") === 0)
					{
						$flag = true;
						$check = 2;
						$start_offset = $i + 1;
					}
					if (!$flag)
					{
						if (preg_match("#Title#i", $text_mod_lines[$i]))
						{
							$report->header_report .= sprintf("%s [i]mod Title[/i], may fail with some tools, Line %s\n",
								Validation_warning, ($i + 1));
							$flag = true;
							$validate_warn_flag = false;
							$check = 2;
						}
						if ($header_end_line == $row && $warn_flag == false)
						{
							$report->header_report .= sprintf("%s Missing or incorrect [i]mod Title[/i]\n",
								Validation_fail, ($i + 1));
							$flag = true;
							$warn_flag = false;
							$validate_flag = false;
							$check = 2;
						}
					}
				}
				if ($check == 2)
				{
					if (strpos($text_mod_lines[$i], "## mod Author:") === 0)
					{
						$flag = true;
						$check = 3;
						$start_offset = $i + 1;
						if (preg_match("#(mod |)Author#i", $text_mod_lines[$i + 1]))
						{
							$check = 2;
						}
					}
					if (!$flag)
					{
						if (preg_match("#Author#i", $text_mod_lines[$i]))
						{
							$report->header_report .= sprintf("%s [i]mod Author[/i], may fail with some tools, Line %s\n",
								Validation_warning, ($i + 1));
							$flag = true;
							$warn_flag = true;
							$validate_warn_flag = false;
							$check = 3;
							if (preg_match("#(mod |)Author#i", $text_mod_lines[$i + 1]))
							{
								$check = 2;
							}
						}
						if ($header_end_line == $row)
						{
							$report->header_report .= sprintf("%s Missing or incorrect [i]mod Author[/i]\n",
								Validation_fail);
							$flag = true;
							$validate_warn_flag = false;
							$check = 3;
							if (preg_match("#(mod |)Author#i", $text_mod_lines[$i + 1]))
							{
								$check = 2;
							}
						}
					}
				}
				if ($check == 3)
				{
					if (strpos($text_mod_lines[$i], "## mod Description:") === 0)
					{
						$flag = true;
						$check = 4;
						$start_offset = $i + 1;
					}
					if (!$flag)
					{
						if (preg_match("#Description#i", $text_mod_lines[$i]))
						{
							$report->header_report .= sprintf("%s [i]mod Description[/i], may fail with some tools, Line %s\n",
								Validation_warning, ($i + 1));
							$flag = true;
							$warn_flag = true;
							$validate_warn_flag = false;
							$check = 4;
						}
						if ($header_end_line == $row)
						{
							$report->header_report .= sprintf("%s Missing or incorrect [i]mod Description[/i]\n",
								Validation_fail);
							$flag = true;
							$validate_flag = false;
							$check = 4;
						}
					}
				}
				if ($check == 4)
				{
					if (strpos($text_mod_lines[$i], "## mod Version:") === 0)
					{
						$flag = true;
						$check = 5;
						$start_offset = $i + 1;
					}
					if (!$flag)
					{
						if (preg_match("#\\#.Version#i", $text_mod_lines[$i]))
						{
							$report->header_report .= sprintf("%s [i]mod Version[/i], may fail with some tools, Line %s\n",
								Validation_warning, ($i + 1));
							$flag = true;
							$warn_flag = true;
							$validate_warn_flag = false;
							$check = 5;
						}
						if ($header_end_line == $row)
						{
							if (CheckAbove($text_mod_lines, "Version", $i))
							{
								$report->header_report .= sprintf("%s [i]mod Version[/i] in wrong order, may fail with some tools\n",
									Validation_fail);
								$flag = true;
								$warn_flag = true;
								$validate_warn_flag = false;
								$check = 5;
							}
							else
							{
								$report->header_report .= sprintf("%s Missing [i]mod Version[/i]. This is a mandatory field for the mod Template.\n",
									Validation_fail);
								$flag = true;
								$validate_flag = false;
								$check = 5;
							}
						}
					}
				}
				if ($check == 5)
				{
					if (strpos($text_mod_lines[$i], "## Installation Level:") === 0)
					{
						$flag = true;
						$check = 6;
						$start_offset = $i + 1;
					}
					if ($header_end_line == $row && $flag == false)
					{
						if (CheckAbove($text_mod_lines, "Installation Level", $i))
						{
							$report->header_report .= sprintf("%s [i]Installation Level[/i] in wrong order, may fail with some tools\n",
								Validation_warning);
							$flag = true;
							$warn_flag = true;
							$validate_warn_flag = false;
							$check = 6;
						}
						else
						{
							$report->header_report .= sprintf("%s Missing [i]Installation Level[/i]. This is a mandatory field for the mod Template.\n",
							Validation_fail);
							$flag = true;
							$validate_flag = false;
							$check = 6;
						}
					}
				}
				if ($check == 6)
				{
					if (strpos($text_mod_lines[$i], "## Installation Time:") === 0)
					{
						$flag = true;
						$check = 7;
						$start_offset = $i + 1;
					}
					if ($header_end_line == $row && $flag == false)
					{
						if (CheckAbove($text_mod_lines, "Installation Time", $i))
						{
							$report->header_report .= sprintf("%s [i]Installation Time[/i] in wrong order, may fail with some tools.\n",
								Validation_warning);
							$flag = true;
							$warn_flag = true;
							$validate_warn_flag = false;
							$check = 7;
						}
						else
						{
							$report->header_report .= sprintf("%s Missing [i]Installation Time[/i]. This is a mandatory field for the mod Template.\n",
							Validation_fail);
							$flag = true;
							$validate_flag = false;
							$check = 7;
						}
					}
				}
				if ($check == 7)
				{
					if (strpos($text_mod_lines[$i], "## Files To Edit:") === 0)
					{
						$flag = true;
						$check = 8;
						$start_offset = $i + 1;
					}
					if (!$flag)
					{
						if (preg_match("#Files To Edit#i", $text_mod_lines[$i]))
						{
							$report->header_report .= sprintf("%s [i]Files To Edit[/i] is in the wrong case which may cause it to fail in some tools.\n",
								Validation_warning);
							$flag = true;
							$validate_warn_flag = false;
							$check = 8;
							$start_offset = $i + 1;
						}
						if ($header_end_line == $row)
						{
							$report->header_report .= sprintf("%s [i]Files To Edit[/i] is missing. This is no cause for concern.\n",
								Validation_warning);
							$flag = true;
							$validate_warn_flag = false;
							$check = 8;
						}
					}
				}
				if ($check == 8)
				{
					if (strpos($text_mod_lines[$i], "## Included Files:") === 0)
					{
						$flag = true;
						$check = 9;
						$start_offset = $i + 1;
					}
					if ($header_end_line == $row && $flag == false)
					{
						$report->header_report .= sprintf("%s [i]Included Files[/i] is missing. This is no cause for concern.\n",
							Validation_warning);
						$flag = true;
						$validate_warn_flag = false;
						$check = 9;
					}
				}
				if ($check == 9)
				{
					if (strpos($text_mod_lines[$i], "## License:") === 0)
					{
						if (preg_match("#http://opensource.org/licenses/gpl-license.php GNU General Public License v2#", $text_mod_lines[$i]))
						{
							$report->header_report .= sprintf("%s [i]You are using the GNU GPL License[/i].\n",
								Validation_ok);
						}
						elseif (preg_match("#http://opensource.org/licenses/gpl-license.php GNU Public License v2#", $text_mod_lines[$i]))
						{
							$report->header_report .= sprintf("%s [i]You are using the GNU GPL License, however the license statement in the mod Template has been updated to include the word 'General', you should update accordingly[/i]. (16/08/2005)\n",
								Validation_warning);
							$validate_warn_flag = false; // we will let them off with a warning, this time
						}
						else
						{
							$report->header_report .= sprintf("%s [i]You are not using the GPL License[/i]. Please be aware that most MODs are automatically licensed under the GPL and you may be required to relicense your mod in accordance with the terms of the GPL inherited from the core phpBB package.\n",
								Validation_notice);
						}
						$flag = true;
						$check = 10;
						$start_offset = $i + 1;
					}
					if ($header_end_line == $row && $flag == false)
					{

						$report->header_report .= sprintf("%s Missing or incorrect [i]License[/i] statement - A license statement is important for phpBB MODs. Please read the mod docs for more information.\n",
							Validation_fail);
						$flag = true;
						$validate_flag = false;
						$check = 10;
					}
				}
				if ($check == 10)
				{
					if (preg_match("#For security purposes, please check: http://www.phpbb.com/mods/#", $text_mod_lines[$i])
						&& preg_match("#for the latest version of this mod. Although MODs are checked#", $text_mod_lines[$i + 1])
						&& preg_match("#before being allowed in the MODs Database there is no guarantee#", $text_mod_lines[$i + 2])
						&& preg_match("#that there are no security problems within the mod. No support#", $text_mod_lines[$i + 3])
						&& preg_match("#will be given for MODs not found within the MODs Database which#", $text_mod_lines[$i + 4])
						&& preg_match("#can be found at http://www.phpbb.com/mods/#", $text_mod_lines[$i + 5]))
					{
						$flag = true;
						$check = 11;
						$start_offset = $i + 1;
					}
					if ($header_end_line == $row && $flag == false)
					{
						$report->header_report .= sprintf("%s Missing or incorrect [i]Security Disclaimer[/i] - The disclaimer was recently updated 24 July 2005.\n",
							Validation_fail);
						$flag = true;
						$validate_flag = false;
						$check = 11;
					}
				}
				if ($check == 11)
				{
					if (strpos($text_mod_lines[$i], "## Author Notes:") === 0)
					{
						$flag = true;
						$check = 12;
						$start_offset = $i + 1;
					}
					if ($header_end_line == $row && $flag == false)
					{
						$report->header_report .= sprintf("%s Missing or incorrect [i]Author Notes[/i]\n",
							Validation_fail);
						$flag = true;
						$validate_flag = false;
						$check = 12;
					}
				}
				if ($check == 12)
				{
					if (strpos($text_mod_lines[$i], "## mod History:") === 0)
					{
						$flag = true;
						$check = 13;
						$start_offset = $i + 1;
					}
					if ($header_end_line == $row && $flag == false)
					{
						$report->header_report .= sprintf("%s [color=green]not used [i]mod History[/i][/color] - [u]This is not an error[/u]\n",
							Validation_ok);
						$flag = true;
						$check = 13;
					}
				}
				if ($check == 13)
				{
					if (preg_match("#Before Adding This mod To Your Forum, You Should Back Up All Files Related To This mod#", $text_mod_lines[$i]))
					{
						$flag = true;
						$check = 14;
						$start_offset = $i + 1;
					}
					if ($header_end_line == $row && $flag == false)
					{
						$report->header_report .= sprintf("%s Missing or incorrect [i]Install disclaimer[/i]\n",
							Validation_fail);
						$flag = true;
						$validate_flag = false;
						$check = 14;
					}
				}
			}
			$flag = false;
			$warn_flag = false;
		}

		//
		// Make sure # EoM is used, but not in the header
		//
		$flag = false;
		$warn_flag = false;
		for($i = $header_end_line; $i < count($text_mod_lines); $i++)
		{
			$row = $i + 1;
			if (strpos($text_mod_lines[$i], "# EoM") === 0)
			{
				$flag = true;
			}
			if (!$flag)
			{
				if (preg_match("#\\#( |)EoM#i", $text_mod_lines[$i]))
				{
					$report->header_report .= sprintf("%s [i]# EoM[/i] is the wrong syntax.\n",
						Validation_warning);
					$flag = true;
					$warn_flag = true;
					$validate_flag = false;
					$check = 15;
				}
				if ($header_end_line == $row)
				{
					$report->header_report .= sprintf("%s Missing or incorrect [i]# EoM[/i]\n",
						Validation_fail);
					$flag = true;
					$validate_flag = false;
					$check = 15;
				}
			}
		}

		for ($i = $header_end_line; $i < count($text_mod_lines); $i++)
		{
			// silly to check this if it is not an action definition
			if (strpos($text_mod_lines[$i], "--") >= 0 && strpos($text_mod_lines[$i], "#") === 0)
			{
				if (preg_match("# \\[ ([A-Za-z0-9, ]+?) \\]#", $text_mod_lines[$i]))
				{
					$db_info = preg_replace("#^\\#(\\-+?) \\[(.*?)$#", "-\\1^", $text_mod_lines[$i]);
					$db_info_length = strpos($db_info, " [");
					$db_info = "-";
					for ($j = 0; $j <= $db_info_length - 2; $j++)
					{
						$db_info .= "-";
					}
					$db_info .= "^";

					$report->header_report .= sprintf("%s mod actions [b]must not[/b] have spaces in action definition. line %s\n[quote][b][color=red]-- [[/color] [color=green]--[[/color][/b]\n--^\n%s\n%s[/quote]\n",
						Validation_fail, ($i + 1), $text_mod_lines[$i], $db_info);
					$validate_flag = false;
				}
				if (preg_match("#\\[ ([A-Za-z0-9, ]+?) \\] #", $text_mod_lines[$i]))
				{
					$db_info = preg_replace("#^\\#(.*?)\\] -(.*?)$#", "-\\1-^", $text_mod_lines[$i]);
					$db_info_len = strpos($db_info, "] ");
					$db_info = "-";
					for ($j = 0; $j <= $db_info_len - 3; $j++)
					{
						$db_info .= "-";
					}
					$db_info .= "^";

					$report->header_report .= sprintf("%s mod actions [b]must not[/b] have spaces in action definition. line %s\n[quote][b][color=red]] --[/color] [color=green]]--[/color][/b]\n-^\n%s\n%s[/quote]\n",
						Validation_fail, ($i + 1), $text_mod_lines[$i], $db_info);
					$validate_flag = false;
				}
			}
		}

		$modification = new text_mod();

		//try
		$modification->read_actions($text_mod);

		if ($modification->actions != null)
		{
			validator::load_phpbb_file_list($language, $version); // Load the phpBB file list for comparison in the OPEN check
			for ($i = 0; $i < $modification->actions->get_count(); $i++)
			{
				$temp_action = $modification->actions->get($i);

				if ($temp_action->type != strtoupper($temp_action->type))
				{
					$report->header_report .= sprintf("%s mod actions [b]must[/b] be in upper case. line %s\n[code]%s[/code]\n",
						Validation_fail, $temp_action->start_line, $temp_action->body);
					$validate_flag = false;
				}
				if (static_mod_actions::get_type($temp_action->type) == ModActionType_Edit ||
					static_mod_actions::get_type($temp_action->type) == ModActionType_InLineEdit)
				{
					if ($checks)
					{
						if (strpos($temp_action->body, "<font") != false)
						{
							if (preg_match("#<font(.*?)>#", $temp_action->body))
							{
								$report->html_report .= sprintf("%s Unauthorised usage of the FONT tag. Please use the span tag, starting line: %s\n[quote]%s[/quote]\n",
									Validation_fail, $temp_action->start_line, preg_replace("#<font(.*?)>#", "[b]<font\\1>[/b]", $temp_action->body));
								$validate_flag = false;
							}
						}
						if (strpos($temp_action->body, "<br>") != false)
						{
							$report->html_report .= sprintf("%s Unauthorised usage of the <br> tag. Please use the <br /> tag., starting line: %s\n[quote]%s[/quote]\n",
								Validation_fail, $temp_action->start_line, preg_replace("#<br>#", "[b]<br>[/b]", $temp_action->body));
							$validate_flag = false;
						}
						if (strpos($temp_action->body, "<img") != false &&
							strpos($temp_action->body, "/>") == false)
						{
							$report->html_report .= sprintf("%s Unauthorised usage of the <img> tag. Please make sure you use XHTML entities e.g. <img />., starting line: %s\n[quote]%s[/quote]\n",
								Validation_fail, $temp_action->start_line, preg_replace("#<img#", "[b]<img[/b]", $temp_action->body));
							$validate_flag = false;
						}
						if (strpos($temp_action->body, "mysql_") != false)
						{
							if (strpos($temp_action->body, "mysql_connect") != false)
							{
								if (preg_match("#mysql_connect\\((.*?)\\)#", $temp_action->body))
								{
									$report->dbal_report .= sprintf("%s Unauthorised usage of mysql_connect, please use the DBAL, starting line: %s\n[quote]%s[/quote]\n",
										Validation_fail, $temp_action->start_line, preg_replace("#mysql_connect\\((.*?)\\)#", "[b]mysql_connect(\\1)[/b]", $temp_action->body));
									$validate_flag = false;
								}
							}
							if (strpos($temp_action->body, "mysql_error") != false)
							{
								if (preg_match("#mysql_error\\((.*?)\\)#", $temp_action->body))
								{
									$report->dbal_report .= sprintf("%s Unauthorised usage of mysql_error, please use the DBAL, starting line: %s\n[quote]%s[/quote]\n",
										Validation_fail, $temp_action->start_line, preg_replace("#mysql_error\\((.*?)\\)#", "[b]mysql_error(\\1)[/b]", $temp_action->body));
									$validate_flag = false;
								}
							}
						}
					} // check
				}
				if (static_mod_actions::get_type($temp_action->type) == ModActionType_File)
					//if (mod_actions.Parse(((ModAction)Modification.Actions[i]).ActionType).Type == mod_actions.ModActionType.File)
				{
					if (strpos($temp_action->body, ".phpex") != false)
					{
						$report->actions_report .= sprintf("%s Unauthorised usage of path names (usage of .phpex rather than .php), starting line: %s\n[quote]%s[/quote]\n",
							Validation_fail, $temp_action->start_line, preg_replace("#\\.phpex#", "[b].phpex[/b]", $temp_action->body));
						$validate_flag = false;
					}
					if (preg_match("#(\\/|)?php(BB|bb)(2|)\\/#", $temp_action->body))
					{
						$report->actions_report .= sprintf("%s Unauthorised usage of /phpBB2/, starting line: %s\n[quote]%s[/quote]\n",
							Validation_fail, $temp_action->start_line, preg_replace("#(\\/|)?php(BB|bb)(2|)\\/#", "[b]\\1php\\2\\3/[/b]", $temp_action->body));
						$validate_flag = false;
					}
					if ($temp_action->type == "OPEN")
					{
						$open_found = false; // bool
						if (!(array_search(trim($temp_action->body, $this->trim_chars), validator::get_phpbb_file_list()) === 0))
						{
							$open_found = true;
						}
						if (!$open_found)
						{
							$report->actions_report .= sprintf("%s File to OPEN does not exist in phpBB standard installation package, starting line: %s\n[quote]%s[/quote]\n",
								Validation_fail, $temp_action->start_line, $temp_action->to_string());
							$validate_flag = false;
						}
					}
					if ($temp_action->type == "COPY")
					{
						$copy_flag = true; // bool
						$copy_lines = explode("\n", str_replace("\r\n", "\n", $temp_action->body));
						for ($j = 0; $j < count($copy_lines); $j++)
						{
							if (strlen($copy_lines[$j]) > 5)
							{
								if (!preg_match("#(copy|COPY) (.*?)\\.(.*?) (to|TO) (.*?)$#", $copy_lines[$j]))
								{
									$copy_flag = false;
								}
							}
						}
						if (!$copy_flag)
						{
							$report->actions_report .= sprintf("%s Unauthorised usage of COPY tag syntax, starting line: %s\n[quote]%s[/quote]\n",
								Validation_fail, $temp_action->start_line, $temp_action->to_string());
							$validate_flag = false;
						}
					}
				}
				if (array_key_exists($temp_action->Type, validator::get_actions()))
				{
					$action_validate = true;
				}
				$temp_type = static_mod_actions::parse($temp_action->type);
				if ($temp_action->type == "IN-LINE FIND")
				{
					$temp_prev_action = $modification->actions->get($i - 1);
					$temp_prev_type = static_mod_actions::parse($temp_prev_action->type);
					if (!(($temp_prev_action->type == "FIND" ||
						$temp_prev_type->type == ModActionType_InLineEdit ||
						$temp_prev_type->type == ModActionType_Edit)))
					{
						$find_in_line_flag = false;
					}
				}
				if ($temp_type->type == ModActionType_Edit) // TODO: make sure this is correct #4
				{
					$temp_prev_action = $modification->actions->get($i - 1);
					$temp_prev_type = static_mod_actions::parse($temp_prev_action->type);
					if ($temp_prev_type->type != ModActionType_Edit) // TODO: #4
					{
						if ($temp_prev_type->type != ModActionType_Find) // TODO: #2
						{
							$edit_in_line_flag = false;
						}
					}
				}
				if (!$action_validate)
				{
					$report->actions_report .= sprintf("%s Unauthorised action, %s please only use the official actions, starting line: %s\n[quote]%s[/quote]\n",
						Validation_fail, $temp_action->Type, $temp_action->start_line, $temp_action->to_string());
					$validate_flag = false;
				}
				if (!$find_in_line_flag)
				{
					$report->actions_report .= sprintf("%s Unauthorised action, %s. This action must be preceded by a FIND, 'edit' or an IN-LINE 'edit' action, starting line: %s\n[quote]%s\n%s[/quote]\n",
						Validation_fail, $temp_action->Type, $temp_action->start_line, $temp_prev_action->to_string(), $temp_action->to_string());
					$validate_flag = false;
				}
				$action_validate = false;
				$find_in_line_flag = true;
				$edit_in_line_flag = true;
			}
		}

		// try, PHP4 non-goodness
		$modification->read_header($text_mod);

		if ($modification->header != null)
		{
			$install_time = $modification->header->installation_time; // int
			$modification->update_installation_time();
			if (abs($install_time - $modification->header->InstallationTime) / $install_time > 0.5)
			{
				$report->header_report .= sprintf("%s Installation time of %s minutes is more than 50%% out of realistic expectation, expectation was %s minutes\n",
					Validation_warning, $install_time / 60, round($modification->header->installation_time / 60));
				$validate_warn_flag = false;
			}

			$files_to_edit = $modification->header->files_to_edit;
			$modification->update_files_to_edit();
			if (!mod_author::compare_string_collection($files_to_edit, $modification->header->files_to_edit))
			{
				$report->header_report .= sprintf("%s Files To Edit in header does not equal files edited in mod\n",
					Validation_warning);
				$validate_warn_flag = false;
			}

			$included_files = $modification->header->included_files;
			$modification->update_included_files();
			if (!mod_author::compare_string_collection($included_files, $modification->header->included_files))
			{
				$report->header_report .= sprintf("%s Included Files in header does not equal filed copied over in mod\n",
					Validation_warning);
				$validate_warn_flag = false;
			}
		}

		// catch

		//
		//
		//

		//string rating_report;
		if (!$validate_flag)
		{
			$report->rating = "The mod [b][color=red]failed[/color][/b] the mod pre-validation process. Please review and fix your errors before submitting to the mod DB.";
			$report->passed = false;
		}
		else
		{
			$report->rating = "The mod [b][color=green]passed[/color][/b] the mod pre-validation process, please check over for elements computers cannot detect.";
			$report->passed = true;
		}
		if (!$validate_warn_flag)
		{
			$report->rating .= "\nThere were some [b][color=orange]warnings[/color][/b] which should be looked at but aren't causes for denial. These warnings may cause your mod to act in undetermined ways in tools other than EasyMod, and should be fixed for maximum compatibility.";
			$report->passed = false;
		}
		if ($validate_flag && $validate_warn_flag && $checks)
		{
			$report->actions_report .= "\n[color=green]No problems[/color] were detected in this MODs Template in accordance with the phpBB mod Team guidelines.";
		}
		if ($report->html_report == null  && $checks)
		{
			$report->html_report = "[color=green]No problems[/color] were detected in this MODs use HTML elements in accordance with the phpBB2 coding standards.";
		}
		if ($report->dbal_report == null  && $checks)
		{
			$report->dbal_report = "[color=green]No problems[/color] were detected in this MODs use of databases [size=9](if used)[/size] in accordance with the phpBB2 coding standards.";
		}

		return $report;
	}

	/// <summary>
	///
	/// </summary>
	/// <param name="sc1"></param>
	/// <param name="sc2"></param>
	/// <returns></returns>
	function compare_string_collection($sc1, $sc2) // private bool
	{
		$trim_chars = " \t\n\r\b";
		// check to see if every element is in
		$flag = true; // bool
		foreach ($sc1 as $s)
		{
			if (trim($s, $trim_chars) != "")
			{
				if (array_search($s, $sc2) != false)
				{
					$flag = true;
				}
				else
				{
					$flag = false;
				}
			}
		}
		return $flag;
	}

	/// <summary>
	///
	/// </summary>
	/// <param name="TextModLines"></param>
	/// <param name="needle"></param>
	/// <param name="line"></param>
	/// <returns></returns>
	function check_above($text_mod_lines, $needle, $line) // private bool
	{
		for ($i = 0; $i < $line; $i++)
		{
			if (strpos($text_mod_lines[$i], "" . needle . "") != false)
			{
				return true;
			}
		}
		return false;
	}

		/// <summary>
		///
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		function modx_mod($m) // public static explicit operator
		{
			$n = new modx_mod();
			$n->actions = $m->actions;
			$n->author_notes_indent = $m->author_notes_indent;
			$n->author_notes_start_line = $m->author_notes_start_line;
			$n->header = $m->header;
			$n->set_last_read_format($m->get_last_read_format());
			$n->mod_files_to_edit_indent = $m->mod_files_to_edit_indent;
			$n->mod_included_files_indent = $m->mod_included_files_indent;
			return $n;
		}
}
?>