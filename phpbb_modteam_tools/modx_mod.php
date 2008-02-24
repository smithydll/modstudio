<?php
/**
*
* @package Phpbb.ModTeam.Tools (PHP)
* @version $Id: modx_mod.php,v 1.1 2008-02-24 20:16:03 smithydll Exp $
* @copyright (c) 2005 phpBB Group
* @license http://opensource.org/licenses/gpl-license.php GNU Public License
*
*/

include($phpbb_mod_team_tools_path . 'cortex-xml/xml.php');
include($phpbb_mod_team_tools_path . 'cortex-xml/xml/xml_element.php');
include($phpbb_mod_team_tools_path . 'cortex-xml/xml/xml_reader.php');
include($phpbb_mod_team_tools_path . 'cortex-xml/xml/xml_writer.php');

	/// <summary>
	/// Summary description for modx_mod.
	/// </summary>
	class modx_mod extends mod // public : IMod
	{

		var $default_xslt_file = "modx.subsilver.en.xsl"; // string
		var $xml_validation_message; // string

		/// <summary>
		///
		/// </summary>
		function modx_mod()
		{
			$this->mod();
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="fileName"></param>
		function read($file_name) // public override void
		{
			$this->last_read_format = ModFormats_Modx;
			//$xml_data_set = new mod(); // mod
			// IN THE C# SOURCE WE COULD USE AN XMLREADER, IN PHP WE USE CORTEX
			// Vic is teh <3
			$xml_reader = new cortex_xml_reader();
			$xml_data_set = $xml_reader->parse_file($file_name);

			$xml_header = $xml_data_set->get_child('header');

			$this->read_authors($xml_header);
			$this->read_title($xml_header);
			$this->read_author_notes($xml_header);
			$this->read_description($xml_header);
			$this->read_history($xml_header);
			$this->read_meta($xml_header);
			$this->read_version($xml_header);
			$this->read_license($xml_header);
			$this->read_installation($xml_header);

			$this->read_actions($xml_data_set->get_child('action-group'));
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="modx_mod"></param>
		function read_string($modx_mod) // public void
		{
			$this->last_read_format = ModFormats_Modx;
			//$xml_data_set = new mod(); // mod
			// IN THE C# SOURCE WE COULD USE AN XMLREADER, IN PHP WE USE CORTEX
			// Vic is teh <3
			$xml_reader = new cortex_xml_reader();
			$xml_data_set = $xml_reader->parse_data($modx_mod);

			$xml_header = $xml_data_set->get_child('header');

			$this->read_authors($xml_header);
			$this->read_title($xml_header);
			$this->read_author_notes($xml_header);
			$this->read_description($xml_header);
			$this->read_history($xml_header);
			$this->read_meta($xml_header);
			$this->read_version($xml_header);
			$this->read_license($xml_header);
			$this->read_installation($xml_header);

			$this->read_actions($xml_data_set->get_child('action-group'));
		}

		#region Read Xml Stuff
		/// <summary>
		///
		/// </summary>
		/// <param name="XmlDataSet"></param>
		function read_authors($xml_header) // private void
	 	{
			for ($i = 0; $i < count($xml_header->children); $i++)
			{
				if ($xml_header->children[$i]->name != 'author-group') continue;
				for ($j = 0; $j < count($xml_header->children[$i]->children); $j++)
				{
					$xml_item = $xml_header->children[$i]->children[$j];
					if ($xml_item->name == 'author')
					{
						$xml_username = $xml_item->get_child('username');
						$xml_realname = $xml_item->get_child('realname');
						$xml_email = $xml_item->get_child('email');
						$xml_homepage = $xml_item->get_child('homepage');
						$temp_author = new mod_author((!empty($xml_username->content)) ? $xml_username->content : '',
							(!empty($xml_realname->content)) ? $xml_realname->content : '',
							(!empty($xml_email->content)) ? $xml_email->content : '',
							(!empty($xml_homepage->content)) ? $xml_homepage->content : ''); // mod_author
						// TODO: contributions
						$this->header->authors->add($temp_author);
					}
				}
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="XmlDataSet"></param>
		function read_title($xml_header)
		{
			$this->header->title = new string_localised();
			for ($i = 0; $i < count($xml_header->children); $i++)
			{
				if ($xml_header->children[$i]->name != 'title') continue;
				$this->header->title->add($xml_header->children[$i]->content, $xml_header->children[$i]->attributes['lang']);
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="XmlDataSet"></param>
		function read_description($xml_header)
		{
			$this->header->description = new string_localised();
			for ($i = 0; $i < count($xml_header->children); $i++)
			{
				if ($xml_header->children[$i]->name != 'description') continue;
				$this->header->description->add($xml_header->children[$i]->content, $xml_header->children[$i]->attributes['lang']);
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="XmlDataSet"></param>
		function read_author_notes($xml_header)
		{
			$this->header->author_notes = new string_localised();
			for ($i = 0; $i < count($xml_header->children); $i++)
			{
				if ($xml_header->children[$i]->name != 'author-notes') continue;
				$this->header->author_notes->add($xml_header->children[$i]->content, $xml_header->children[$i]->attributes['lang']);
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="XmlDataSet"></param>
		function read_history($xml_header)
		{
			$this->header->history = new mod_history();
			$xml_history = $xml_header->get_child('history');
			if (empty($xml_history))
			{
				return;
			}
			for ($i = 0; $i < count($xml_history->children); $i++)
			{
				$xml_entry = $xml_history->children[$i];
				if ($xml_entry->name != 'entry') continue;
				$this->header->history->add(new mod_history_entry());
				$this_mod_history_entry = $this->header->history->get($i);
				for ($j = 0; $j < count($xml_entry->children); $j++)
				{
					if ($xml_entry->children[$j]->name == 'rev-version')
					{
						$xml_major = $xml_entry->children[$j]->get_child('major');
						$xml_minor = $xml_entry->children[$j]->get_child('minor');
						$xml_revision = $xml_entry->children[$j]->get_child('revision');
						$xml_stage = $xml_entry->children[$j]->attributes['revision'];
						$xml_release = $xml_entry->children[$j]->get_child('release');

						$temp_version = new mod_version(
							$xml_major->content,
							$xml_minor->content,
							$xml_revision->content);
						if (isset($xml_stage))
                        {
                            $temp_version->stage = mod::string_to_version_stage($xml_stage);
                        }
						if (isset($xml_release->content))
						{
							if (strlen($xml_release->content) == 1)
							{
								$temp_version->release = $xml_release->content[0];
							}
						}
						$this_mod_history_entry->version = $temp_version;
					}
				}
				$xml_date = $xml_entry->get_child('date');
				// TODO, verify this
				$this_mod_history_entry->date = strtotime($xml_date->content);
				$this_mod_history_entry->change_log = new mod_history_change_log_localised();
				for ($j = 0; $j < count($xml_entry->children); $j++)
				{
					$xml_change_log = $xml_entry->children[$j];
					if ($xml_change_log->name == 'changelog')
					{
						$language = $xml_change_log->attributes['lang'];
						$this_mod_history_entry->change_log->add(new mod_history_change_log(), $language);
						for ($k = 0; $k < count($xml_change_log->children); $k++)
						{
							$xml_change = $xml_change_log->children[$k];
							if ($xml_change->name == 'change')
							{
								//$this_mod_history_entry->change_log->add($language, $xml_change->content);

								$this_mod_history_change = $this_mod_history_entry->change_log->get($language);
								$this_mod_history_change->add($xml_change->content);
								$this_mod_history_entry->change_log->set($language, $this_mod_history_change);
							}
						}
					}
				}
				$this->header->history->set($i, $this_mod_history_entry);
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="XmlDataSet"></param>
		function read_version($xml_header)
		{
			//try
			//{
				$xml_version = $xml_header->get_child('mod-version');
				$xml_major = $xml_version->get_child('major');
				$xml_minor = $xml_version->get_child('minor');
				$xml_revision = $xml_version->get_child('revision');
				$xml_stage = $xml_version->attributes['stage'];
				$xml_release = $xml_version->get_child('release');
				$temp_version = new mod_version();
				$temp_version->major = intval($xml_major->content);
				$temp_version->minor = intval($xml_minor->content);
				$temp_version->revision = intval($xml_revision->content);

				if (isset($xml_stage))
				{
					if (strlen($xml_stage) > 0)
					{
						$temp_version->stage = mod::string_to_version_stage($xml_stage);
					}
				}

				//try
				//{
				if (isset($xml_release->content))
				{
					if (strlen($xml_release->content) == 1)
					{
						$temp_version->release = $xml_release->content[0];
					}
				}
				//}
				//catch
				//{
				//}
				$this->header->version = $temp_version;
			//}
			//catch
			//{
			//	$this->header->version = new mod_version(0,0,0);
			//}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="XmlDataSet"></param>
		function read_license($xml_header)
		{
			//try {
			$xml_license = $xml_header->get_child('license');
			$this->header->license = $xml_license->content;
			//}
			//catch {}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="XmlDataSet"></param>
		function read_installation($xml_header)
		{
			//try
			//{
				$xml_installation = $xml_header->get_child('installation');
				$xml_installation_level = $xml_installation->get_child('level');
				$xml_installation_time = $xml_installation->get_child('time');
				$this->header->installation_level = mod::installation_level_parse($xml_installation_level->content);
				$this->header->installation_time = intval($xml_installation_time->content);
				// TODO: easymod-compliant
				//$this->header->ModEasymodCompatibility = (bool)XmlDataSet.installation.Rows[0]["easymod-compliant"];
				// TODO: mod-config
			//}
			//catch {}
			$xml_target_version = $xml_installation->get_child('target-version');
			$this->header->phpbb_version = new target_version_cases();
			//try {
				$this->header->phpbb_version->Primary = $xml_target_version->attributes['target-primary'];
			//}
			//catch {}

			// major
			for ($i = 0; $i < count($xml_target_version->children); $i++)
			{
				$xml_target_major = $xml_target_version->children[$i];
				if ($xml_target_major->name != 'target-major') continue;
				$comparisson = TargetVersionComparisson_EqualTo; // TargetVersionComparisson
				switch ($xml_target_major->attributes['allow'])
				{
					case "exact":
						$comparisson = TargetVersionComparisson_EqualTo;
						break;
					case "after":
						$comparisson = TargetVersionComparisson_GreaterThan;
						break;
					case "after-equal":
						$comparisson = TargetVersionComparisson_GreaterThanEqual;
						break;
					case "before":
						$comparisson = TargetVersionComparisson_LessThan;
						break;
					case "before-equal":
						$comparisson = TargetVersionComparisson_LessThanEqual;
						break;
					case "not-equal":
						$comparisson = TargetVersionComparisson_NotEqualTo;
						break;
				}
				$this->header->phpbb_version->add(new target_version_case($comparisson, TargetVersionPart_Major, $xml_target_major->content));
			}

			// minor
			for ($i = 0; $i < count($xml_target_version->children); $i++)
			{
				$xml_target_minor = $xml_target_version->children[$i];
				if ($xml_target_minor->name != 'target-minor') continue;
				$comparisson = TargetVersionComparisson_EqualTo;
				switch ($xml_target_minor->attributes['allow'])
				{
					case "exact":
						$comparisson = TargetVersionComparisson_EqualTo;
						break;
					case "after":
						$comparisson = TargetVersionComparisson_GreaterThan;
						break;
					case "after-equal":
						$comparisson = TargetVersionComparisson_GreaterThanEqual;
						break;
					case "before":
						$comparisson = TargetVersionComparisson_LessThan;
						break;
					case "before-equal":
						$comparisson = TargetVersionComparisson_LessThanEqual;
						break;
					case "not-equal":
						$comparisson = TargetVersionComparisson_NotEqualTo;
						break;
				}
				$this->header->phpbb_version->add(new target_version_case($comparisson, TargetVersionPart_Minor, $xml_target_minor->content));
			}

			// revision
			for ($i = 0; $i < count($xml_target_version->children); $i++)
			{
				$xml_target_revision = $xml_target_version->children[$i];
				if ($xml_target_revision->name != 'target-revision') continue;
				$comparisson = TargetVersionComparisson_EqualTo;
				switch ($xml_target_revision->attributes['allow'])
				{
					case "exact":
						$comparisson = TargetVersionComparisson_EqualTo;
						break;
					case "after":
						$comparisson = TargetVersionComparisson_GreaterThan;
						break;
					case "after-equal":
						$comparisson = TargetVersionComparisson_GreaterThanEqual;
						break;
					case "before":
						$comparisson = TargetVersionComparisson_LessThan;
						break;
					case "before-equal":
						$comparisson = TargetVersionComparisson_LessThanEqual;
						break;
					case "not-equal":
						$comparisson = TargetVersionComparisson_NotEqualTo;
						break;
				}
				if (!isset($xml_target_version->attributes['stage']))
                {
                    $this->header->phpbb_version->add(new target_version_case($comparisson, TargetVersionPart_Revision, $xml_target_revision->content));
                }
                else
                {
                    $this->header->phpbb_version->add(new target_version_case($comparisson, TargetVersionPart_Revision, $xml_target_revision->content, nullChar, mod::string_to_version_stage($xml_target_version->attributes['stage'])));
                }
			}

			// release
			for ($i = 0; $i < count($xml_target_version->children); $i++)
			{
				$xml_target_release = $xml_target_version->children[$i];
				if ($xml_target_release->name != 'target-release') continue;
				$comparisson = TargetVersionComparisson_EqualTo;
				switch ($xml_target_release->attributes['allow'])
				{
					case "exact":
						$comparisson = TargetVersionComparisson_EqualTo;
						break;
					case "after":
						$comparisson = TargetVersionComparisson_GreaterThan;
						break;
					case "after-equal":
						$comparisson = TargetVersionComparisson_GreaterThanEqual;
						break;
					case "before":
						$comparisson = TargetVersionComparisson_LessThan;
						break;
					case "before-equal":
						$comparisson = TargetVersionComparisson_LessThanEqual;
						break;
					case "not-equal":
						$comparisson = TargetVersionComparisson_NotEqualTo;
						break;
				}
				$release = $xml_target_release->content; // string
				if (strlen($release) > 0)
				{
					$this->header->phpbb_version->add(new target_version_case($comparisson, TargetVersionPart_Release, $release[0]));
				}
				else
				{
					$this->header->phpbb_version->add(new target_version_case($comparisson, TargetVersionPart_Release));
				}
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="XmlDataSet"></param>
		function read_meta($xml_header)
		{
			$this->header->meta = array();
			for ($i = 0; $i <count($xml_header->children); $i++)
			{
				$xml_meta = $xml_header->children[$i];
				if ($xml_meta->name != 'meta') continue;
				$this->header->meta[$xml_meta->attributes['name']] = $xml_meta->attributes['content'];
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="XmlDataSet"></param>
		function read_actions($xml_actions)
		{
			$this->read_sql_actions($xml_actions);
			$this->read_copy_actions($xml_actions);
			$this->read_edit_actions($xml_actions);
			$this->read_diy_instructions_actions($xml_actions);

			$this->actions->add(new mod_action("SAVE/CLOSE ALL FILES", "", "EoM"));
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="XmlDataSet"></param>
		function read_sql_actions($xml_actions)
		{
			for ($i = 0; $i < count($xml_actions->children); $i++)
			{
				$xml_sql = $xml_actions->children[$i];
				if ($xml_sql->name != 'sql') continue;
				$this->actions->add(new mod_action("SQL", $xml_sql->content, ""));
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="XmlDataSet"></param>
		function read_copy_actions($xml_actions)
		{
			$temp_copy = '';
			for ($i = 0; $i < count($xml_actions->children); $i++)
			{
				$xml_copy = $xml_actions->children[$i];
				if ($xml_copy->name != 'cppu') continue;
				for ($j = 0; $j < count($xml_copy->children); $j++)
				{
					$xml_file = $xml_copy->children[$i];
					if ($xml_file->name != 'file') continue;
					$temp_copy .= ("copy ");
					$temp_copy .= ($xml_file->attributes['from']);
					$temp_copy .= (" to ");
					$temp_copy .= ($xml_file->attributes['to']);
					$temp_copy .= ("\n");
				}
			}
			if (strlen($temp_copy) > 0)
			{
				$this->actions->add(new mod_action("COPY", $temp_copy, ""));
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="XmlDataSet"></param>
		function read_edit_actions($xml_actions)
		{
			$first_find_in_edit = true; // bool
			for ($i = 0; $i < count($xml_actions->children); $i++)
			{
				$xmlOpen = $xml_actions->children[$i];
				if ($xmlOpen->name != 'open') continue;
				$this->actions->add(new mod_action("OPEN", $xmlOpen->attributes['src'], ""));
				for ($j = 0; $j < count($xmlOpen->children); $j++)
				{
					$xml_edit = $xmlOpen->children[$j];
					if ($xml_edit->name == 'edit')
					{
						$first_find_in_edit = true;
						for ($k = 0; $k < count($xml_edit->children); $k++)
						{
							$xml_find = $xml_edit->children[$k];
							if ($xml_find->name == 'find')
							{
								$this_comment = new string_localised(); // string_localised
								if ($first_find_in_edit)
								{
									for($l = 0; $l < count($xml_edit->children); $l++)
									{
										$xml_comment = $xml_edit->children[$l];
										if ($xml_comment->name == 'comment')
										{
											//try
											//{
												$this_comment->add($xml_comment->content, $xml_comment->attributes['lang']);
											//}
											//catch
											//{
											//}
										}
									}
								}
								else
								{
									$this_comment = new string_localised("");
								}
								$this->actions->add(new mod_action("FIND", $xml_find->content, $this_comment, $xml_find->attributes['type']));
								$first_find_in_edit = false;
							}
						}
						for ($k = 0; $k < count($xml_edit->children); $k++)
						{
							$xml_action = $xml_edit->children[$k];
							if ($xml_action->name == 'action')
							{
								$action_title = ""; // string
								switch ($xml_action->attributes['type'])
								{
									case "after-add":
										$action_title = "AFTER, ADD";
										break;
									case "before-add":
										$action_title = "BEFORE, ADD";
										break;
									case "replace":
									case "replace-with":
										$action_title = "REPLACE WITH";
										break;
									case "operation":
										$action_title = "INCREMENT";
										break;
								}
								$this->actions->add(new mod_action($action_title, $xml_action->content, ""));
							}
						}
						for ($k = 0; $k < count($xml_edit->children); $k++)
						{
							$xml_edit_in_line = $xml_edit->children[$k];
							if ($xml_edit_in_line->name == 'inline-edit')
							{
								for ($l = 0; $l < count($xml_edit_in_line->children); $l++)
								{
									$xml_find_in_line = $xml_edit_in_line->children[$l];
									if ($xml_find_in_line->name == 'inline-find')
									{
										$this->actions->add(new mod_action("IN-LINE FIND", $xml_find_in_line->content, "", $xml_find_in_line->attributes['type']));
									}
								}
								for ($l = 0; $l < count($xml_edit_in_line->children); $l++)
								{
									$xml_action_in_line = $xml_edit_in_line->children[$l];
									if ($xml_action_in_line->name == 'inline-action')
									{
										$action_title = "";
										switch ($xml_action_in_line->attributes['type'])
										{
											case "after-add":
												$action_title = "IN-LINE AFTER, ADD";
												break;
											case "before-add":
												$action_title = "IN-LINE BEFORE, ADD";
												break;
											case "replace":
											case "replace-with":
												$action_title = "IN-LINE REPLACE WITH";
												break;
											case "operation":
												$action_title = "IN-LINE INCREMENT";
												break;
										}
										$this->actions->add(new mod_action($action_title, $xml_action_in_line->content, ""));
									}
								}
							}
						}
					}
				}
			}
		}

		function read_diy_instructions_actions($xml_actions)
		{
			for ($i = 0; $i < count($xml_actions->children); $i++)
			{
				$xml_diy = $xml_actions->children[$i];
				if ($xml_diy->name != 'diy-instructions') continue;
				$this->actions->add(new mod_action("DIY INSTRUCTIONS", $xml_diy->content,"", $xml_diy->attributes['lang']));
			}
		}

		#endregion

		/*/// <summary>
		///
		/// </summary>
		/// <param name="fileName"></param>
		public override void Write(string fileName)
		{
			Write(fileName, defaultXsltFile);
		}*/

		/// <summary>
		///
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="xsltFile"></param>
		function write($file_name, $xslt_file = null) // public void
		{
			if ($xslt_file == null) $xslt_file = $this->default_xslt_file;
			mod::save_text_file($this->to_string($xslt_file), $file_name);
		}

		#region Write Xml Stuff
		/// <summary>
		///
		/// </summary>
		/// <param name="XmlDataSet"></param>
		function write_header(&$xml_data_set) // private void
		{
			//mod.headerRow newHeaderRow = XmlDataSet.$this->header->NewheaderRow();
			$xml_header =& new cortex_xml_element('header');
			//XmlDataSet.$this->header->Rows.Add(newHeaderRow);

			$this->write_license($xml_header);
			$this->write_title($xml_data_set, $xml_header);
			$this->write_description($xml_data_set, $xml_header);
			$this->write_author_notes($xml_data_set, $xml_header);
			$this->write_authors($xml_data_set, $xml_header);
			$this->write_version($xml_data_set, $xml_header);
			$this->write_installation($xml_data_set, $xml_header);
			$this->write_history($xml_data_set, $xml_header);
			$this->write_meta($xml_data_set, $xml_header);
			$xml_data_set->add_child($xml_header);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="newHeaderRow"></param>
		function write_license(&$xml_header) // private void
		{
			$xml_license =& new cortex_xml_element('license');
			$xml_license->content = $this->header->license;
			$xml_header->add_child($xml_license);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="XmlDataSet"></param>
		/// <param name="newHeaderRow"></param>
		function write_title(&$xml_data_set, &$xml_header) // private void
		{
			foreach ($this->header->title->key_list as $language => $entry)
			{
				$xml_title =& new cortex_xml_element('title');
				$xml_title->content = $entry;
				$xml_title->add_attribute('lang', $language);
				$xml_header->add_child($xml_title);
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="XmlDataSet"></param>
		/// <param name="newHeaderRow"></param>
		function write_description(&$xml_data_set, &$xml_header) // private void
		{
			foreach ($this->header->description->key_list as $language  => $entry)
			{
				$xml_description =& new cortex_xml_element('description');
				$xml_description->content = $entry;
				$xml_description->add_attribute('lang', $language);
				$xml_header->add_child($xml_description);
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="XmlDataSet"></param>
		/// <param name="newHeaderRow"></param>
		function write_author_notes(&$xml_data_set, &$xml_header) // private void
		{
			foreach ($this->header->author_notes->key_list as $language  => $entry)
			{
				$xml_author_notes =& new cortex_xml_element('author-notes');
				$xml_author_notes->content = $entry;
				$xml_author_notes->add_attribute('lang', $language);
				$xml_header->add_child($xml_author_notes);
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="XmlDataSet"></param>
		/// <param name="newHeaderRow"></param>
		function write_authors(&$xml_data_set, &$xml_header) // private void
		{
			//mod._author_groupRow authorGroupRow = XmlDataSet._author_group.New_author_groupRow();
			$xml_author_group =& new cortex_xml_element('author-group');

			//authorGroupRow.SetParentRow(newHeaderRow);
			//XmlDataSet._author_group.Rows.Add(authorGroupRow);
			foreach ($this->header->authors->authors as $entry)
			{
				//mod.authorRow newRow = XmlDataSet.author.NewauthorRow();
				$xml_author =& new cortex_xml_element('author');

				if (strtolower($entry->email) != "")
				{
					$xml_email =& new cortex_xml_element('email');
					$xml_email->content = $entry->email;
					$xml_author->add_child($xml_email);
				}
				if (strtolower($entry->homepage) != "")
				{
					$xml_homepage =& new cortex_xml_element('homepage');
					$xml_homepage->content = $entry->homepage;
					$xml_author->add_child($xml_homepage);
				}
				if (strtolower($entry->real_name) != "")
				{
					$xml_realname =& new cortex_xml_element('realname');
					$xml_realname->content = $entry->real_name;
					$xml_author->add_child($xml_realname);
				}
				if (strtolower($entry->user_name) != "")
				{
					$xml_username =& new cortex_xml_element('username');
					$xml_username->content = $entry->user_name;
					$xml_author->add_child($xml_username);
				}
				//newRow.SetParentRow(authorGroupRow);
				//XmlDataSet.author.Rows.Add(newRow);
				//mod.contributionsRow newContributionsRow = XmlDataSet.contributions.NewcontributionsRow();
				$xml_contributions =& new cortex_xml_element('contributions');
				if ($entry->author_from > 0)
				{
					$xml_contributions->add_attribute('from', $entry->author_from);
				}
				if ($entry->author_to > 0)
				{
					$xml_contributions->add_attribute('to', $entry->author_to);
				}
				if ($entry->status != ModAuthorStatus_NoneSelected)
				{
					switch ($entry->status)
					{
						case ModAuthorStatus_Current:
							$xml_contributions->add_attribute('status', 'current');
							break;
						case ModAuthorStatus_Past:
							$xml_contributions->add_attribute('status', 'past');
							break;
					}
				}
				//newContributionsRow.SetParentRow(newRow);
				//XmlDataSet.contributions.Rows.Add(newContributionsRow);
				$xml_author->add_child($xml_contributions);
				$xml_author_group->add_child($xml_author);
			}
			$xml_header->add_child($xml_author_group);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="XmlDataSet"></param>
		/// <param name="newHeaderRow"></param>
		function write_version(&$xml_data_set, &$xml_header) // private void
		{
			$xml_version =& new cortex_xml_element('mod-version');

			$xml_major =& new cortex_xml_element('major');
			$xml_major->content = $this->header->version->major;
			$xml_version->add_child($xml_major);

			$xml_minor =& new cortex_xml_element('minor');
			$xml_minor->content = $this->header->version->minor;
			$xml_version->add_child($xml_minor);

			//--------------------------------------------------------------------
			$xml_revision =& new cortex_xml_element('revision');
			$xml_revision->content = $this->header->version->revision;
			$xml_version->add_child($xml_revision);
			if ($this->header->version->stage != VersionStage_Stable)
            {
				$xml_version->add_attribute('stage', mod::version_stage_to_string($this->header->version->stage));
            }
			//--------------------------------------------------------------------

			if ($this->header->version->release != nullChar)
			{
				$xml_release =& new cortex_xml_element('release');
				$xml_release->content = $this->header->version->release;
				$xml_version->add_child($xml_release);
			}

			$xml_header->add_child($xml_version);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="XmlDataSet"></param>
		/// <param name="newHeaderRow"></param>
		function write_installation(&$xml_data_set, &$xml_header) // private void
		{
			$xml_installation =& new cortex_xml_element('installation');

			$xml_level =& new cortex_xml_element('level');
			switch ($this->header->installation_level)
			{
				case ModInstallationLevel_Easy:
					$xml_level->content = 'easy';
					break;
				case ModInstallationLevel_Intermediate:
					$xml_level->content = 'intermediate';
					break;
				case ModInstallationLevel_Advanced:
					$xml_level->content = 'advanced';
					break;
			}

			$xml_installation->add_child($xml_level);

			$xml_time =& new cortex_xml_element('time');
			$xml_time->content = $this->header->installation_time;
			$xml_installation->add_child($xml_time);

			// TODO: target version, right now target version isn't supported by Phpbb.ModTeam.Tools
			// full support to come, release is currently omitted, need full support!!!
			// For the moment we will say everything is phpBB2.0 compliant
			if ($this->header->phpbb_version === null) $this->header->phpbb_version = new target_version_cases(new mod_version(2, 0, 0));
			if (!($this->header->phpbb_version === null))
			{
				// handle for case where Major version is non-sensical
				//if ($this->header->PhpbbVersion.Major == 0) $this->header->phpbb_version = new target_version_cases(new mod_version(2, 0, 0));

				$xml_target =& new cortex_xml_element('target-version');

				$xml_target_primary =& new cortex_xml_element('target-primary');
				$xml_target_primary->content = $this->header->phpbb_version->get_primary();
				$xml_target->add_child($xml_target_primary);

				foreach ($this->header->phpbb_version->cases as $version_case)
				{
					switch ($version_case->part)
					{
						case TargetVersionPart_Major:
							$xml_target_major =& new cortex_xml_element('target-major');
							switch ($version_case->comparisson)
							{
								case TargetVersionComparisson_EqualTo:
									$xml_target_major->add_attribute('allow', "exact");
									break;
								case TargetVersionComparisson_GreaterThan:
									$xml_target_major->add_attribute('allow', "after");
									break;
								case TargetVersionComparisson_GreaterThanEqual:
									$xml_target_major->add_attribute('allow', "after-equal");
									break;
								case TargetVersionComparisson_LessThan:
									$xml_target_major->add_attribute('allow', "before");
									break;
								case TargetVersionComparisson_LessThanEqual:
									$xml_target_major->add_attribute('allow', "before-equal");
									break;
								case TargetVersionComparisson_NotEqualTo:
									$xml_target_major->add_attribute('allow', "not-equal");
									break;
							}
							$xml_target_major->content = $version_case->get_value();
							$xml_target->add_child($xml_target_major);
							break;
						case TargetVersionPart_Minor:
							$xml_target_minor =& new cortex_xml_element('target-minor');
							switch ($version_case->comparisson)
							{
								case TargetVersionComparisson_EqualTo:
									$xml_target_minor->add_attribute('allow', "exact");
									break;
								case TargetVersionComparisson_GreaterThan:
									$xml_target_minor->add_attribute('allow', "after");
									break;
								case TargetVersionComparisson_GreaterThanEqual:
									$xml_target_minor->add_attribute('allow', "after-equal");
									break;
								case TargetVersionComparisson_LessThan:
									$xml_target_minor->add_attribute('allow', "before");
									break;
								case TargetVersionComparisson_LessThanEqual:
									$xml_target_minor->add_attribute('allow', "before-equal");
									break;
								case TargetVersionComparisson_NotEqualTo:
									$xml_target_minor->add_attribute('allow', "not-equal");
									break;
							}
							$xml_target_minor->content = $version_case->get_value();
							$xml_target->add_child($xml_target_minor);
							break;
						case TargetVersionPart_Revision:
							$xml_target_revision =& new cortex_xml_element('target-revision');
							switch ($version_case->comparisson)
							{
								case TargetVersionComparisson_EqualTo:
									$xml_target_revision->add_attribute('allow', "exact");
									break;
								case TargetVersionComparisson_GreaterThan:
									$xml_target_revision->add_attribute('allow', "after");
									break;
								case TargetVersionComparisson_GreaterThanEqual:
									$xml_target_revision->add_attribute('allow', "after-equal");
									break;
								case TargetVersionComparisson_LessThan:
									$xml_target_revision->add_attribute('allow', "before");
									break;
								case TargetVersionComparisson_LessThanEqual:
									$xml_target_revision->add_attribute('allow', "before-equal");
									break;
								case TargetVersionComparisson_NotEqualTo:
									$xml_target_revision->add_attribute('allow', "not-equal");
									break;
							}
							switch ($version_case->stage)
                            {
                                case VersionStage_Alpha:
									$xml_target_revision->add_attribute('stage', "alpha");
                                    break;
                                case VersionStage_Beta:
                                    $xml_target_revision->add_attribute('stage', "beta");
                                    break;
                                case VersionStage_Delta:
                                    $xml_target_revision->add_attribute('stage', "delta");
                                    break;
                                case VersionStage_Gamma:
                                    $xml_target_revision->add_attribute('stage', "gamma");
                                    break;
                                case VersionStage_ReleaseCandidate:
                                    $xml_target_revision->add_attribute('stage', "release-candidate");
                                    break;
                            }
							$xml_target_revision->content = $version_case->get_value();
							$xml_target->add_child($xml_target_revision);
							break;
						case TargetVersionPart_Release:
							//mod._target_releaseRow targetReleaseRow = XmlDataSet._target_release.New_target_releaseRow();
							$xml_target_release =& new cortex_xml_element('target-release');
							switch ($version_case->comparisson)
							{
								case TargetVersionComparisson.EqualTo:
									$xml_target_release->add_attribute('allow', "exact");
									break;
								case TargetVersionComparisson.GreaterThan:
									$xml_target_release->add_attribute('allow', "after");
									break;
								case TargetVersionComparisson.GreaterThanEqual:
									$xml_target_release->add_attribute('allow', "after-equal");
									break;
								case TargetVersionComparisson.LessThan:
									$xml_target_release->add_attribute('allow', "before");
									break;
								case TargetVersionComparisson.LessThanEqual:
									$xml_target_release->add_attribute('allow', "before-equal");
									break;
								case TargetVersionComparisson.NotEqualTo:
									$xml_target_release->add_attribute('allow', "not-equal");
									break;
							}
							if (strlen($version_case->get_value()) > 0)
							{
								$release_string = $version_case->get_value();
								$xml_target_release->content = $release_string[0];
							}
							else
							{
								$xml_target_release->content = '';
							}
							$xml_target->add_child($xml_target_release);
							break;
					}
				}
				$xml_installation->add_child($xml_target);
			}
			$xml_header->add_child($xml_installation);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="XmlDataSet"></param>
		/// <param name="newHeaderRow"></param>
		function write_history(&$xml_data_set, &$xml_header) // private void
		{
			$xml_history =& new cortex_xml_element('history');

			foreach ($this->header->history->history as $mhe)
			{
				$xml_entry =& new cortex_xml_element('entry');
				$xml_entry_date =& new cortex_xml_element('date');
				$xml_entry_date->content = date("Y-m-d", $mhe->date);
				$xml_entry->add_child($xml_entry_date);
				// Version
				$xml_entry_version =& new cortex_xml_element('rev-version');

				$xml_major =& new cortex_xml_element('major');
				$xml_major->content = $mhe->version->major;
				$xml_entry_version->add_child($xml_major);

				$xml_minor =& new cortex_xml_element('minor');
				$xml_minor->content = $mhe->version->minor;
				$xml_entry_version->add_child($xml_minor);

				// -----------------------------------------------------------------
				$xml_revision =& new cortex_xml_element('revision');
				$xml_revision->content = $mhe->version->revision;
				$xml_entry_version->add_child($xml_revision);

				if ($mhe->version->stage != VersionStage_Stable)
                {
                    $xml_entry_version->add_attribute('stage', mod::version_stage_to_string($mhe->version->stage));
                }
				// -----------------------------------------------------------------

				$xml_release =& new cortex_xml_element('release');
				$xml_release->content = $mhe->version->release;
				if ($mhe->version->release != nullChar)
				{
					$xml_entry_version->add_child($xml_release);
				}
				$xml_entry->add_child($xml_entry_version);
				// Changelogs
				foreach ($mhe->change_log->change_logs as $Language => $mhcl)
				{
					$xml_change_log =& new cortex_xml_element('changelog');
					$xml_change_log->add_attribute('lang', $Language);
					// change
					foreach ($mhcl->change_log as $value)
					{
						$xml_change =& new cortex_xml_element('change');
						$xml_change->content = $value;
						$xml_change_log->add_child($xml_change);
					}
					$xml_entry->add_child($xml_change_log);
				}
				$xml_history->add_child($xml_entry);
			}
			if (count($this->header->history->history) > 0)
			{
				$xml_header->add_child($xml_history);
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="XmlDataSet"></param>
		/// <param name="newHeaderRow"></param>
		function write_meta(&$xml_data_set, &$xml_header) // private void
		{
			// let's set the generator first
			if (!array_key_exists("generator", $this->header->meta))
			{
				$this->header->meta["generator"] = "Phpbb.ModTeam.Tools (php)";
			}
			else
			{
				$this->header->meta["generator"] = "Phpbb.ModTeam.Tools (php)";
			}
			foreach ($this->header->meta as $key => $value)
			{
				$xml_meta =& new cortex_xml_element('meta');
				$xml_meta->add_attribute('name', $key);
				$xml_meta->add_attribute('content', $value);
				$xml_header->add_child($xml_meta);
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="XmlDataSet"></param>
		function write_actions(&$xml_data_set) // private void
		{
			$xml_action_group =& new cortex_xml_element('action-group');
			$copy_row =& new cortex_xml_element('copy');
			$current_open_row =& new cortex_xml_element('open');
			$current_edit_row =& new cortex_xml_element('edit');
			$current_in_line_edit_row =& new cortex_xml_element('inline-edit');

			$type = ''; // string
			$last_action_type = "EDIT"; // string
			$last_in_line_action_type = "EDIT"; // string
			foreach ($this->actions->actions as $ma)
			{
				switch (strtoupper($ma->type))
				{
					case "SQL":
						//mod.sqlRow sqlRow = XmlDataSet.sql.NewsqlRow();
						$xml_sql =& new cortex_xml_element('sql');
						//sqlRow.sql_Text = ma.Body;
						$xml_sql->content = $ma->body;
						//sqlRow.SetParentRow(actiongroupRow);
						//XmlDataSet.sql.Rows.Add(sqlRow);
						$xml_action_group->add_child($xml_sql);
						break;
					case "COPY":
						//if (XmlDataSet.copy.Rows.Count == 0)
						if ($xml_action_group->get_child('copy') === false)
						{
							//copyRow = XmlDataSet.copy.NewcopyRow();
							//copyRow.SetParentRow(actiongroupRow);
							//XmlDataSet.copy.Rows.Add(copyRow);
							$copy_row =& new cortex_xml_element('copy');
							$xml_action_group->add_child($copy_row);
						}
						$lines = explode("\n", $ma->body); // string[]
						foreach ($lines as $line)
						{
							$from = ""; // string
							$to = ""; // string
							if (strpos(strtolower(ltrim($line, $this->trim_chars)), 'copy') === 0)
							{
								//try
								//{
									$from = trim(preg_replace("# to#i", "", preg_replace("#copy #i", "", preg_match("#^copy (.+) to(\\s)#i", trim($line, $this->trim_chars)))), $this->trim_chars);
									$to = trim(preg_replace("# to(\\s)#i", "", preg_replace("#copy #i", "", preg_match("# to (.+)$#i", trim($line, $this->trim_chars)))), $this->trim_chars);

									$xml_file =& new cortex_xml_element('file');
									$xml_file->add_attribute('from', $from);
									$xml_file->add_attribute('to', $to);
									$copy_row->add_child($xml_file);
								//} catch { }
							}
						}
						break;// */
					case "OPEN":
						$current_open_row =& new cortex_xml_element('open');
						$current_open_row->add_attribute('src', trim($ma->body, $this->trim_chars));
						$xml_action_group->add_child($current_open_row);
						break;
					case "FIND":
						if ($last_action_type == "EDIT" || $last_action_type == "INLINE")
						{
							$current_edit_row =& new cortex_xml_element('edit');
							$current_open_row->add_child($current_edit_row);
						}
						$xml_find =& new cortex_xml_element('find');
						$xml_find->content = $ma->body;
						if ($ma->modifier != "")
						{
							$xml_find->add_attribute('type', $ma->modifier);
						}
						// TODO: needs work (merging)
						if ($ma->after_comment != null)
						{
							foreach ($ma->after_comment->key_list as $Language => $value)
							{
								if ($value != "" && $value != null)
								{
									$xml_comment =& new cortex_xml_element('comment');
									$xml_comment->add_attribute('lang', $Language);
									$xml_comment->content = $value;
									$current_edit_row->add_child($xml_comment);
								}
							}
						}
						$current_edit_row->add_child($xml_find);
						$last_action_type = "FIND";
						$last_in_line_action_type = "EDIT";
						break;
					case "AFTER, ADD":
					case "BEFORE, ADD":
					case "REPLACE WITH":
					case "INCREMENT":
						$type = "";
						switch (strtoupper($ma->type))
						{
							case "AFTER, ADD":
								$type = "after-add";
								break;
							case "BEFORE, ADD":
								$type = "before-add";
								break;
							case "REPLACE WITH":
								$type = "replace-with";
								break;
							case "INCREMENT":
								$type = "operation";
								break;
						}
						$xml_action =& new cortex_xml_element('action');
						$xml_action->add_attribute('type', $type);
						$xml_action->content = $ma->body;
						// TODO: comment
						$current_edit_row->add_child($xml_action);
						$last_action_type = "EDIT";
						break;
					case "IN-LINE FIND":
						// add the inline-edit row to the inline-edit table
						if ($last_in_line_action_type == "EDIT")
						{
							$current_in_line_edit_row =& new cortex_xml_element('inline-edit');
							$current_edit_row->add_child($current_in_line_edit_row);
						}
						$xml_find_in_line =& new cortex_xml_element('inline-find');
						$xml_find_in_line->content = $ma->body;
						if ($ma->modifier != "")
						{
							$xml_find_in_line->add_attribute('type', $ma->modifier);
						}
						$current_in_line_edit_row->add_child($xml_find_in_line);
						$last_in_line_action_type = "FIND";
						$last_action_type = "INLINE";
						break;
					case "IN-LINE AFTER, ADD":
					case "IN-LINE BEFORE, ADD":
					case "IN-LINE REPLACE WITH":
					case "IN-LINE INCREMENT":
						$type = "";
						switch (strtoupper($ma->type))
						{
							case "IN-LINE AFTER, ADD":
								$type = "after-add";
								break;
							case "IN-LINE BEFORE, ADD":
								$type = "before-add";
								break;
							case "IN-LINE REPLACE WITH":
								$type = "replace-with";
								break;
							case "IN-LINE INCREMENT":
								$type = "operation";
								break;
						}
						$xml_action_in_line =& new cortex_xml_element('inline-action');
						$xml_action_in_line->content = $ma->body;
						$xml_action_in_line->add_attribute('type', $type);
						// TODO: comment
						$current_in_line_edit_row->add_child($xml_action_in_line);
						$last_in_line_action_type = "EDIT";
						break;
					case "DIY INSTRUCTIONS":
						$xml_diy =& new cortex_xml_element('diy-instructions');
						$xml_diy->content = $ma->body;
						$xml_diy->add_attribute('lang', $ma->modifier);
						$xml_action_group->add_child($xml_diy);
						break;
					case "SAVE/CLOSE ALL FILES":
						// lets ignore ;)
						break;
					default:
						//Console.WriteLine("Problem with action: " + ma.Type);
						break;
				}
			}
			//XmlDataSet._action_group.Rows.Add(actiongroupRow);
			$xml_data_set->add_child($xml_action_group);
		}// */

		#endregion

		/*/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return ToString(defaultXsltFile);
		}*/

		/// <summary>
		///
		/// </summary>
		/// <param name="xsltFile"></param>
		/// <returns></returns>
		function to_string($xslt_file = null) // public string
		{
			if ($xslt_file == null) $xslt_file = $this->default_xslt_file;
			// lets force these events to sync the mod,
			$this->update_files_to_edit();
			$this->update_included_files();
			$this->update_installation_time();

			$xml_data_set =& new cortex_xml_element('mod'); // mod
			$xml_data_set->add_attribute('xmlns', 'http://www.phpbb.com/mods/xml/modx-1.0.1.xsd');
			$xml_data_set->root = true;

			$this->write_header($xml_data_set);
			$this->write_actions($xml_data_set);

			$xw = new cortex_xml_writer();
			@$mv = new mod_version($this->header->phpbb_version->primary);
			if ($mv->major == 3)
			{
				$xw->xml_stylesheet = 'modx.prosilver.en.xsl';
			}
			else
			{
				$xw->xml_stylesheet = 'modx.subsilver.en.xsl';
			}

			$sb = $xw->object_to_xml($xml_data_set);

			return $sb;
		}

		/*/// <summary>
		///
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public override Phpbb.ModTeam.Tools.Validation.report Validate(string fileName)
		{
			return Validate(fileName, "english");
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		public Validation.report Validate(string fileName, string language)
		{
			return Validate(fileName, language, new mod_version(2, 0, 0), true);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="language"></param>
		/// <param name="version">Can alter the checks performed based on the version</param>
		/// <param name="checks">Can disable addtional checks so they aren't duplicated in m|EAL</param>
		/// <returns></returns>
		public Validation.report Validate(string fileName, string language, mod_version version, bool checks)
		{
			Validation.report report = new Validation.report();
			modx_mod modification = new modx_mod();
			bool validateFlag = true;
			bool validateWarnFlag = true;

			try
			{
				modification.Read(fileName);
			}
			catch
			{
				report.HeaderReport += "[color=red][b]Error reading MODX file[/b][/color]\n";
				return report;
			}

			// validate XML against schema

			XmlValidationMessage = "";
			XmlTextReader xtr = new XmlTextReader(fileName);
			XmlValidatingReader xvr = new XmlValidatingReader(xtr);

			xvr.ValidationType = ValidationType.Schema;
			xvr.ValidationEventHandler +=new ValidationEventHandler(xvr_ValidationEventHandler);
			xvr.Schemas.Add("http://www.phpbb.com/mods/xml/modx-1.0.xsd", "http://www.phpbb.com/mods/xml/modx-1.0.xsd");

			while(xvr.Read())
			{
			}

			if (XmlValidationMessage == "")
			{
				report.HeaderReport += string.Format("{0} XML Validation against the MODX XML Schema found no errors\n",
					validator.ok);
			}
			else
			{
				report.HeaderReport += "[quote=\"Xml Validation report output\"]";
				report.HeaderReport += string.Format("{0}/n",
					validator.info);
				report.HeaderReport += XmlValidationMessage;
				report.HeaderReport += "[/quote]\n";
			}

			// validate for phpbb.com requirements
			report.HeaderReport += string.Format("{0} Please be aware that XML entities have been represented in text format for readability and do not reflect the structure of the mod in the following report where appropriate.\n\n",
				validator.info);

			foreach (mod_author ma in modification.$this->header->Authors)
			{
				if (ma.UserName == "" || ma.UserName == null)
				{
					report.HeaderReport += string.Format("{1} phpBB requires MODX files contain at least a valid phpBB.com usernames for authors.[quote]{0}[/quote]\n",
						ma.ToString(), validator.error);
					validateFlag = false;
				}
				if (ma.Email.ToUpper() == "N/A")
				{
					report.HeaderReport += string.Format("{2} e-mail should be omitted or empty, not [i]{0}[/i].[quote]{1}[/quote]\n",
						ma.Email, ma.ToString(), validator.warning);
					validateWarnFlag = false;
				}
				if (ma.RealName.ToUpper() == "N/A")
				{
					report.HeaderReport += string.Format("{2} real name should be omitted or empty, not [i]{0}[/i].[quote]{1}[/quote]\n",
						ma.Email, ma.ToString(), validator.warning);
					validateWarnFlag = false;
				}
				if (ma.Homepage.ToUpper() == "N/A")
				{
					report.HeaderReport += string.Format("{2} homepage should be omitted or empty, not [i]{0}[/i].[quote]{1}[/quote]\n",
						ma.Email, ma.ToString(), validator.warning);
					validateWarnFlag = false;
				}
			}

			if (modification.$this->header->license == "http://opensource.org/licenses/gpl-license.php GNU General Public License v2")
			{
				report.HeaderReport += string.Format("{0} [i]You are using the GPL License[/i].\n",
					validator.pass);
			}
			else if (modification.$this->header->license == "" || modification.$this->header->license == null)
			{
				report.HeaderReport += string.Format("{0} [i]You have omitted the license field[/i]. Please be aware that most MODs are automatically licensed under the GPL and you may be required to relicense your mod in accordance with the terms of the GPL inherited from the core phpBB package.\n",
					validator.warning);
			}
			else
			{
				report.HeaderReport += string.Format("[i]You are not using the GPL License[/i]. Please be aware that most MODs are automatically licensed under the GPL and you may be required to relicense your mod in accordance with the terms of the GPL inherited from the core phpBB package.\n",
					validator.info);
			}

			foreach (ModAction ma in modification.Actions)
			{
				if (ma.Type == "OPEN")
				{
					validator.LoadPhpbbFileList(language); // Load the phpBB file list for comparison in the OPEN check
					if (!validator.PhpbbFileList.Contains(ma.Body))
					{
						report.ActionsReport += string.Format("{2} File to OPEN does not exist in phpBB standard installation package\n[quote]{1}[/quote]\n",
							0, ma.ToString(), validator.warning);
						validateFlag = false;
					}
				}

				if(ma.Type == "COPY")
				{
					// TODO: fill this in
				}

				if (Validation.mod_actions.GetType(ma.Type) == Validation.mod_actions.ModActionType.Edit ||
					Validation.mod_actions.GetType(ma.Type) == Validation.mod_actions.ModActionType.InLineEdit)
				{
					if (checks)
					{
						if (ma.Body.IndexOf("<font") >= 0)
						{
							if (Regex.IsMatch(ma.Body, "<font(.*?)>"))
							{
								report.HtmlReport += string.Format("{2} Unauthorised usage of the FONT tag. Please use the span tag, starting line: {0}\n[quote]{1}[/quote]\n",
									0, Regex.Replace(ma.ToString(), "<font(.*?)>", "[b]<font$1>[/b]"), validator.fail);
								validateFlag = false;
							}
						}
						//if (Regex.IsMatch(Modification.Actions[i].ActionBody, "<br>"))
						if (ma.Body.IndexOf("<br>") >= 0)
						{
							report.HtmlReport += string.Format("{2} Unauthorised usage of the <br> tag. Please use the <br /> tag.\n[quote]{1}[/quote]\n",
								0, Regex.Replace(ma.ToString(), "<br>", "[b]<br>[/b]"), validator.fail);
							validateFlag = false;
						}
						if (ma.Body.IndexOf("<img") >= 0 &&
							ma.Body.IndexOf("/>") < 0)
						{
							report.HtmlReport += string.Format("Unauthorised usage of the <img> tag. Please make sure you use XHTML entities e.g. <img />.\n[quote]{1}[/quote]\n",
								0, Regex.Replace(ma.ToString(), "<img", "[b]<img[/b]"));
							validateFlag = false;
						}
						if (ma.Body.IndexOf("mysql_") >= 0)
						{
							if (ma.Body.IndexOf("mysql_connect") >= 0)
							{
								if (Regex.IsMatch(ma.Body, "mysql_connect\\((.*?)\\)"))
								{
									report.DbalReport += string.Format("Unauthorised usage of mysql_connect, please use the DBAL\n[quote]{1}[/quote]\n",
										0, Regex.Replace(ma.ToString(), "mysql_connect\\((.*?)\\)", "[b]mysql_connect($1)[/b]"));
									validateFlag = false;
								}
							}
							if (ma.Body.IndexOf("mysql_error") >= 0)
							{
								if (Regex.IsMatch(ma.Body, "mysql_error\\((.*?)\\)"))
								{
									report.DbalReport += string.Format("Unauthorised usage of mysql_error, please use the DBAL\n[quote]{1}[/quote]\n",
										0, Regex.Replace(ma.ToString(), "mysql_error\\((.*?)\\)", "[b]mysql_error($1)[/b]"));
									validateFlag = false;
								}
							}
						}
					} // end check blocking
				}
			}

			if (!validateFlag)
			{
				report.Rating = "The mod [b][color=red]failed[/color][/b] the mod pre-validation process. Please review and fix your errors before submitting to the mod DB.";
				report.Passed = false;
			}
			else
			{
				report.Rating = "The mod [b][color=green]passed[/color][/b] the mod pre-validation process, please check over for elements computers cannot detect.";
				report.Passed = true;
			}
			if (!validateWarnFlag)
			{
				report.Rating += "\nThere were some [b][color=orange]warnings[/color][/b] which should be looked at but aren't causes for denial. These warnings may cause your mod to act in undetermined ways in tools other than EasyMod, and should be fixed for maximum compatibility.";
				report.Passed = false;
			}
			if (validateFlag && validateWarnFlag && checks)
			{
				report.ActionsReport += "\n[color=green]No problems[/color] were detected in this MODs Template in accordance with the phpBB mod Team guidelines.";
			}
			if (report.HtmlReport == null && checks)
			{
				report.HtmlReport = "[color=green]No problems[/color] were detected in this MODs use HTML elements in accordance with the phpBB2 coding standards.";
			}
			if (report.DbalReport == null && checks)
			{
				report.DbalReport = "[color=green]No problems[/color] were detected in this MODs use of databases [size=9](if used)[/size] in accordance with the phpBB2 coding standards.";
			}

			return report;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void xvr_ValidationEventHandler(object sender, ValidationEventArgs e)
		{
			XmlValidationMessage += e.Message + "\n";
			XmlValidationMessage += "Severity: " + e.Severity + "\n\n";
		}*/
		#region IMod Members

		/// <summary>
		///
		/// </summary>
		function get_last_read_format() // public new Phpbb.ModTeam.Tools.DataStructures.ModFormats
		{
			return $this->last_read_format;
		}
		function set_last_read_format($value)
		{
			$this->last_read_format = $value;
		}

		#endregion

		/// <summary>
		///
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		function text_mod($m) // public static explicit operator
		{
			global $phpbb_mod_team_tools_path;
			$n = new text_mod($phpbb_mod_team_tools_path);
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