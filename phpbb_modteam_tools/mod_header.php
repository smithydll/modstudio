<?php
/*
 * Phpbb.ModTeam.Tools (PHP)
 * http://smithydll.id.au/
 * Copyright © 2007, David Lachlan Smith
 *
 * $Id: mod_header.php,v 1.1 2008-02-24 20:16:03 smithydll Exp $
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
/// Respresents a modification header section.
/// </summary>
class mod_header
{
	/// <summary>
	/// The title of the mod, this field is localised
	/// </summary>
	var $title; // string_localised
	/// <summary>
	/// A collection of authors
	/// </summary>
	var $authors; // mod_authors
	/// <summary>
	/// A description of what the mod does, this field is localised
	/// </summary>
	var $description; // string_localised
	/// <summary>
	/// The version of the mod
	/// </summary>
	var $version; // mod_version
	/// <summary>
	/// The installation level for the mod
	/// </summary>
	var $installation_level; // ModInstallationLevel
	/// <summary>
	/// The time it takes to install the mod.
	/// </summary>
	var $installation_time; // int
	/// <summary>
	/// The suggested amount of time it takes to install the mod. (unused, ModInstallationTime is overriden without prompting)
	/// </summary>
	var $suggested_install_time; // int
	/// <summary>
	/// The files the mod is editing, updated automatically
	/// </summary>
	var $files_to_edit; // StringCollection
	/// <summary>
	/// The files included with the mod, updated automatically.
	/// </summary>
	var $included_files; // StringCollection
	/// <summary>
	/// The generator for the mod.
	/// </summary>
	var $generator; // string
	/// <summary>
	/// Author Notes, this field is localised
	/// </summary>
	var $author_notes; // string_localised
	/// <summary>
	/// EasyMOD compatible?
	/// </summary>
	var $easy_mod_compatibility; // mod_version
	/// <summary>
	/// History collection
	/// </summary>
	var $history; // mod_history
	/// <summary>
	/// Version of phpBB designed for
	/// </summary>
	var $phpbb_version; // target_version_cases
	/// <summary>
	/// Meta data
	/// </summary>
	var $meta; // StringDictionary
	/// <summary>
	/// License
	/// </summary>
	var $license; // string

	/// <summary>
	/// Constructor
	/// </summary>
	function mod_header()
	{
		$this->title = new string_localised();
		$this->description = new string_localised();
		$this->author_notes = new string_localised();
		$this->meta = array();
		$this->phpbb_version = null;
	}

	/// <summary>
	/// Deep Compare
	/// </summary>
	/// <param name="obj"></param>
	/// <returns></returns>
	function equals($obj) // public override bool
	{
		if (get_class($obj) != "mod_header") return false;
		$mh = $obj; // mod_header
		if (!$this->title->equals($mh->Title)) return false;
		if (!$this->authors->equals($mh->Authors)) return false;
		if (!$this->description->equals($mh->Description)) return false;
		if (!$this->version->equals($mh->Version)) return false;
		if ($this->installation_level != $mh->installation_level) return false;
		if ($this->installation_time != $mh->InstallationTime) return false;
		if (!$this->suggested_install_time === $mh->SuggestedInstallTime) return false;
		if (count($this->files_to_edit) != count($mh->files_to_edit)) return false;
		for ($i = 0; $i < $this->FilesToEdit.Count; $i++)
		{
			if ($this->files_to_edit[$i] != $mh->files_to_edit[$i]) return false;
		}
		if (count($this->included_files) != count($mh->included_files)) return false;
		for ($i = 0; $i < count($this->included_files); $i++)
		{
			if ($this->IncludedFiles[$i] != $mh->IncludedFiles[i]) return false;
		}
		// ignore generator
		if (!$this->author_notes->equals($mh->author_notes)) return false;
		if (!$this->easy_mod_compatibility->equals($mh->easy_mod_compatibility)) return false;
		if (!$this->history->equals($mh->history)) return false;
		if (!$this->phpbb_version->equals($mh->phpbb_version)) return false;
		if (count($this->meta) != count($mh->meta)) return false;
		// TODO: this
		/*foreach (string key in Meta.Keys)
		{
			if (!mh.Meta.ContainsKey(key)) return false;
			if (!Meta[key].Equals(mh.Meta[key])) return false;
		}*/
		if ($thisLicense != null)
		{
			if ($thisLicense != $mh->license) return false;
		}
		else
		{
			if ($mh->license != null && $mh->license != "") return false;
		}
		return true;
	}

	/// <summary>
	///
	/// </summary>
	/// <returns></returns>
	/*function GetHashCode()
	{
		return base.GetHashCode();
	}*/

}
?>