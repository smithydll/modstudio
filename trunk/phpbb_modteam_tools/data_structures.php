<?php
/*
 * Phpbb.ModTeam.Tools (PHP)
 * http://smithydll.id.au/
 * Copyright © 2007, David Lachlan Smith
 *
 * $Id: data_structures.php,v 1.1 2008-02-24 20:16:02 smithydll Exp $
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
	/// Instead of using Strings directly, we have to support a multitude of language in the mod Template.
	/// Therefore we have to start using this structure.
	/// </summary>
	class string_localised
	{
		var $key_list = array(); // should be private

		/// <summary>
		///
		/// </summary>
		function Count()
		{
			return Count($this->key_list);
		}

		/// <summary>
		///
		/// </summary>
		function string_localised($value = null, $language = null)
		{
			$this->key_list = array();
			if ($language == null)
			{
				$language = 'en-gb';
			}
			$this->add($value, $language);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <param name="language"></param>
		function add($value, $language)
		{
			$this->key_list[$language] = $value;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="language"></param>
		function remove($language)
		{
			unset($this->key_list[$language]);
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		function get_value()
		{
			if (array_key_exists('en-gb', $this->key_list))
			{
				return $this->key_list['en-gb'];
			}
		}

		/// <summary>
		///
		/// </summary>
		function get($language)
		{
			if (array_key_exists($language, $this->key_list))
			{
				return $this->key_list[$language];
			}
		}

		/// <summary>
		///
		/// </summary>
		function set($value, $language)
		{
			$this->key_list[$language] = $value;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		function contains_key($key)
		{
			return array_key_exists($key, $this->key_list);
		}

		/// <summary>
		/// Deep compare
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		function equals($obj)
		{
			if (get_class($obj) != "string_localised") return false;
			$k = $obj;
			if (keyList.Count != k.keyList.Count) return false;
			foreach ($this->key_list as $key)
			{
				if (array_key_exists($key, $k))
				{
					if($k->key_list[$key] != $this->key_list[$key]) return false;
				}
				else
				{
					return false;
				}
			}
			return true;
		}
	}

	/// <summary>
	/// The different supported mod file formats.
	/// </summary>
	define('ModFormats_Modx', 0);
	define('ModFormats_TextMOD', 1);
	define('ModFormats_Diff', 2);

	/// <summary>
	///
	/// </summary>
	define('ModInstallationLevel_Easy', 0);
	define('ModInstallationLevel_Intermediate', 1);
	define('ModInstallationLevel_Advanced', 2);

	/// <summary>
    ///
    /// </summary>
    define('ModAuthorStatus_NoneSelected', 0);
    define('ModAuthorStatus_Current', 1);
    define('ModAuthorStatus_Past', 2);

    /// <summary>
    ///
    /// </summary>
    define('CodeIndents_Space', 0);
    define('CodeIndents_Tab', 1);
    define('CodeIndents_RightAligned', 2);

    /// <summary>
    ///
    /// </summary>
    define('StartLine_Same', 0);
    define('StartLine_Next', 1);

	/// <summary>
	///
	/// </summary>
	class mod_authors
	{
		var $authors = array();
		//public ModAuthorEntry[] Authors;

		/// <summary>
		///
		/// </summary>
		function mod_authors()
		{
			$this->authors = array();
		}

		/// <summary>
		///
		/// </summary>
		function get_authors()
		{
			return $this->authors;
		}
		function set_authors($value)
		{
			$this->authors = $value;
		}

		/// <summary>
		///
		/// </summary>
		function get($index)
		{
			return $this->authors[$index];
		}
		function set($index, $value)
		{
			$this->authors[$index] = $value;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="index"></param>
		function remove_at($index)
		{
			unset($this->authors[$index]);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		function insert($index, $value)
		{
			array_splice($this->authors, $index, count($this->authors), array_merge(array($value), array_slice($this->authors, $index)));
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		function remove($value)
		{
			// TODO: verify
			unset($value);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		function contains($value)
		{
			// TODO: verify
			return (array_intersect($this->authors, array($value)) > 0);
		}

		/// <summary>
		///
		/// </summary>
		function clear()
		{
			$this->authors = array();
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		function index_of($value)
		{
			// TODO: verify, php4.2.0
			return array_search($value,$this->authors);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		function add($value)
		{
			$this->authors[] = $value;
		}

		#region ICollection Members

		/// <summary>
		///
		/// </summary>
		function get_count()
		{
			return count($this->authors);
		}

		#endregion

		/// <summary>
		/// Deep Compare
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		function equals($obj)
		{
			if (get_class($obj) != "mod_authors") return false;
			$as2 = $obj;
			if (count($this->authors) != count($as2->authors)) return false;
			foreach($as2->authors as $a2)
			{
				if (!(array_intersect($this->authors, array($a2)) > 0))
				{
					return false;
				}
			}
			return true;
		}
	}

	/// <summary>
	///
	/// </summary>
	class mod_author
	{
		/// <summary>
		/// phpBB.com username (or place of mod origin, usually phpBB.com)
		/// </summary>
		var $user_name;
		/// <summary>
		/// The authors real name (optional)
		/// </summary>
		var $real_name;
		/// <summary>
		/// The authors e-mail address (optional)
		/// </summary>
		var $email;
		/// <summary>
		/// The authors homepage (optional)
		/// </summary>
		var $homepage;
		/// <summary>
		/// The date the author started working on the mod.
		/// </summary>
		var $author_from;
		/// <summary>
		/// The last year the author started working on the mod.
		/// </summary>
		var $author_to;
		/// <summary>
		/// Is the author the present of a past developer of the mod?
		/// </summary>
		var $status;

		/// <summary>
		///
		/// </summary>
		/// <param name="UserName">username</param>
		/// <param name="RealName">real name</param>
		/// <param name="Email">e-mail</param>
		/// <param name="Homepage">homepage</param>
		/// <param name="AuthorFrom">Date author started</param>
		/// <param name="AuthorTo">Date author finished</param>
		/// <param name="Status">The mod Authors Status, Current or Past</param>
		function mod_author($user_name = "", $real_name = "", $email = "", $homepage = "", $author_from = -1, $author_to = -1, $status = ModAuthorStatus_NoneSelected)
		{
			$this->user_name = $user_name;
			$this->real_name = $real_name;
			$this->email = $email;
			$this->homepage = $homepage;
			$this->author_from = $author_from;
			$this->author_to = $author_to;
			$this->status = $status;
			$this->update_na();
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		function to_string()
		{
			$user_name = ($this->user_name == "") ? "n/a" : $this->user_name;
			$email = ($this->email == "") ? "n/a" : $this->email;
			$real_name = ($this->real_name == "") ? "n/a" : $this->real_name;
			$homepage = ($this->homepage == "") ? "n/a" : $this->homepage;
			return sprintf("%s < %s > (%s) %s", $user_name, $email, $real_name, $homepage);
		}

		/// <summary>
		///
		/// </summary>
		function update_na()
		{
			if (strtolower($this->user_name) == "n/a") $this->user_name = "";
			if (strtolower($this->email) == "n/a") $this->email = "";
			if (strtolower($this->real_name) == "n/a") $this->real_name = "";
			if (strtolower($this->homepage) == "n/a") $this->homepage = "";
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		function parse($input)
		{
			// TODO: verify
			$temp_tab_separated_author = preg_replace("#^(\\#\\# MOD Author(|, secondary):|)([\\s]*?)((?!\\s)(?!n\\/a)[\\w\\s\\=\\$\\.\\-\\|@\\'\\:\\[\\]\\|\\*\\(\\)<> ]+?|)\\s<(\\s|)(n\\/a|[a-z0-9\\(\\) \\.\\-_\\+\\[\\]@]+|)(\\s|)>\\s(\\(\\s{0,1}(([\\w\\s\\.\\'\\-]+?)|n\\/a)\\s{0,1}\\)|)(\\s|)((([a-z]+?://){1}|)([a-z0-9\\-\\.,\\?!%\\*_\\#:;~\\\\&$@\\/=\\+\\(\\)]+)|n\\/a|)(([\\s]+?)|)$#i", "\\4\t\\6\t\\9\t\\12", $input);
			$temp_author = explode("\t", $temp_tab_separated_author);
			//Console.Error.WriteLine(tempTabSeparatedAuthor);
			$return_value = new mod_author(trim($temp_author[0]), $temp_author[2], rtrim($temp_author[1]), $temp_author[3]);
			$return_value->update_na();
			return $return_value;
		}

		#region IComparable Members

		/// <summary>
		/// Compares two Authors based on their username, useful for ordering and making sure that no
		/// two authors have the same username.
		/// </summary>
		/// <param name="obj">Author Object to compare to</param>
		/// <returns></returns>
		function compare_to($obj)
		{
			$a2 = $obj;
			return strcmp($this->user_name, $a2->user_name);
		}

		#endregion

		/// <summary>
		/// Deep compare
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		function equals($obj)
		{
			if (get_class($obj) != "mod_author") return false;
			return ($this->UserName.Equals === $a2->user_name &&
				$this->author_from === $a2->author_from &&
				$this->author_to === $a2->author_to &&
				$this->email === $a2->email &&
				$this->homepage === $a2->homepage &&
				$this->real_name === $a2->real_name &&
				$this->status === $a2->status);
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		//public override int GetHashCode()
		//{
		//	return base.GetHashCode ();
		//}

		/// <summary>
		/// Compares only the username
		/// </summary>
		/// <param name="a2"></param>
		/// <returns></returns>
		function has_author($a2)
		{
			return $this->user_name === $a2->user_name;
		}

	}

	/// <summary>
	///
	/// </summary>
	class target_version_cases //public : System.Collections.IEnumerable, System.Collections.ICollection
	{
		var $cases; // private ArrayList
		var $primary; // private string

		/// <summary>
		///
		/// </summary>
		/// <param name="fromVersion"></param>
		function target_version_cases($fromVersion = null) // public
		{
			$this->cases = array();
			if ($fromVersion != null)
			{
				$this->primary = $fromVersion->to_string();
				$this->cases[] = new target_version_case(TargetVersionComparisson_EqualTo, TargetVersionPart_Major, $fromVersion->major);
				$this->cases[] = new target_version_case(TargetVersionComparisson_EqualTo, TargetVersionPart_Minor, $fromVersion->minor);
				$this->cases[] = new target_version_case(TargetVersionComparisson_EqualTo, TargetVersionPart_Revision, $fromVersion->revision);
				if ($fromVersion->release != nullChar)
				{
					$this->cases[] = new target_version_case(TargetVersionComparisson_EqualTo, TargetVersionPart_Release, $fromVersion->release);
				}
            }
		}

		/// <summary>
		///
		/// </summary>
		function get_primary()
		{
			return $this->primary;
		}

		function set_primary($value)
		{
			$this->primary = $value;
		}

		#region IEnumerable Members

		/*public IEnumerator GetEnumerator()
		{
			return cases.GetEnumerator();
		}*/

		#endregion

		#region ICollection Members

		/*public bool IsSynchronized
		{
			get
			{
				return cases.IsSynchronized;
			}
		}*/

		function get_count()
		{
			return count($this->cases);
		}

		//public void CopyTo(Array array, int index)
		//{
		//	cases.CopyTo(array, index);
		//}

		//public object SyncRoot
		//{
		//	get
		//	{
		//		return cases.SyncRoot;
		//	}
		//}

		#endregion

		/// <summary>
		///
		/// </summary>
		function clear()
		{
			$this->cases = array();
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="versionCase"></param>
		/// <returns></returns>
		function contains($version_case)
		{
			return (array_intersect($this->cases, array($version_case)) > 0);
		}

		/// <summary>
		///
		/// </summary>
		function get($index)
		{
			return $this->cases[$index];
		}

		/// <summary>
		///
		/// </summary>
		function set($index, $value)
		{
			$this->cases[$index] = $value;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="newCase"></param>
		/// <returns></returns>
		function add($new_case) // public int
		{
			$this->cases[] = $new_case;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="BeforeCase"></param>
		/// <param name="newCase"></param>
		function insert($before_case, $new_case) // public void
		{
			array_splice($this->cases, $before_case, count($this->cases), array_merge(array($new_case), array_slice($this->cases, $before_case)));
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="index"></param>
		function remove_at($index)
		{
			unset($this->cases[$index]);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="o"></param>
		function remove($o)
		{
			unset($o);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="versionCase"></param>
		/// <returns></returns>
		function index_of($version_case)
		{
			return array_search($version_case,$this->cases);
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		function to_string()
		{
			return $this->primary;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		function equals($obj)
		{
			if (get_class($obj) != "target_version_cases") return false;
            $tvc = $obj;
            if (!$this->primary === $tvc->primary) return false;
            if (!count($this->cases) === count($tvc->cases)) return false;
            for ($i = 0; $i < count($this->cases); $i++)
			{
				if (!$this->cases[$i].Equals($tvc->cases[$i])) return false;
			}
            return true;
		}

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}
	}

	/// <summary>
	///
	/// </summary>
	class target_version_case // public
	{
		/// <summary>
		/// The comparisson to be made
		/// </summary>
		var $comparisson; // public TargetVersionComparisson
		/// <summary>
		/// The part of the version to be compared with
		/// </summary>
		var $part; // public TargetVersionPart
		/// <summary>
        /// The stage of development (for release part)
        /// </summary>
        var $stage; // public VersionStage
		/// <summary>
		/// The value to be compared with
		/// </summary>
		var $case_value; // private string

		/// <summary>
		///
		/// </summary>
		/// <param name="comparisson"></param>
		/// <param name="part"></param>
		/// <param name="value"></param>
		function target_version_case($comparisson = TargetVersionComparisson_EqualTo, $part = TargetVersionPart_Major, $value = 2, $stage = VersionStage_Stable)
		{
			$this->comparisson = $comparisson;
			$this->part = $part;
			$this->case_value = $value;
			$this->stage = $stage;
		}

		/// <summary>
		///
		/// </summary>
		function set_value_char($value) // public char
		{
			$this->case_value = $value;
		}

		/// <summary>
		///
		/// </summary>
		function set_value_int($value) // public int
		{
			$this->case_value = $value;
		}

		/// <summary>
		///
		/// </summary>
		function get_value() // public string
		{
			return $this->case_value;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		function equals($obj) // public override bool
		{
			if (get_class($obj) != "target_version_case") return false;
            $tvc = $obj;
            if (!$this->comparisson === $tvc->comparisson) return false;
            if (!$this->part === $tvc->part) return false;
            if (!$this->case_value === $tvc->case_value) return false;
			return true;
		}

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}
	}

	/// <summary>
	///
	/// </summary>
	/*public enum TargetVersionComparisson
	{
		/// <summary>
		/// exact
		/// </summary>
		EqualTo,
		/// <summary>
		/// not-equal
		/// </summary>
		NotEqualTo,
		/// <summary>
		/// before
		/// </summary>
		LessThan,
		/// <summary>
		/// after
		/// </summary>
		GreaterThan,
		/// <summary>
		/// before-equal
		/// </summary>
		LessThanEqual,
		/// <summary>
		/// after-equal
		/// </summary>
		GreaterThanEqual
	}*/
	define('TargetVersionComparisson_EqualTo',0);
	define('TargetVersionComparisson_NotEqualTo',1);
	define('TargetVersionComparisson_LessThan',2);
	define('TargetVersionComparisson_GreaterThan',3);
	define('TargetVersionComparisson_LessThanEqual',4);
	define('TargetVersionComparisson_GreaterThanEqual',6);

	/// <summary>
	///
	/// </summary>
	/*public enum TargetVersionPart
	{
		/// <summary>
		///
		/// </summary>
		Major,
		/// <summary>
		///
		/// </summary>
		Minor,
		/// <summary>
		///
		/// </summary>
		Revision,
		/// <summary>
		///
		/// </summary>
		Release
	}*/
	define('TargetVersionPart_Major',0);
	define('TargetVersionPart_Minor',1);
	define('TargetVersionPart_Revision',2);
	define('TargetVersionPart_Release',3);

	/*/// <summary>
    ///
    /// </summary>
    public enum VersionStage
    {
        /// <summary>
        ///
        /// </summary>
        Alpha,
        /// <summary>
        ///
        /// </summary>
        Beta,
        /// <summary>
        ///
        /// </summary>
        ReleaseCandidate,
        /// <summary>
        ///
        /// </summary>
        Gamma,
        /// <summary>
        ///
        /// </summary>
        Delta,
        /// <summary>
        ///
        /// </summary>
        Stable
    }*/

    define('VersionStage_Stable',0);
    define('VersionStage_Alpha',1);
    define('VersionStage_Beta',2);
    define('VersionStage_ReleaseCandidate',3);
    define('VersionStage_Gamma',4);
    define('VersionStage_Delta',5);

	/// <summary>
    ///
    /// </summary>
	define('nullChar', '-'); // constant

    /// <summary>
    ///
    /// </summary>
    class mod_version
    {
        /// <summary>
        ///
        /// </summary>
        var $major;
        /// <summary>
        ///
        /// </summary>
        var $minor;
        /// <summary>
        ///
        /// </summary>
        var $revision;
        /// <summary>
        ///
        /// </summary>
        var $stage;
        /// <summary>
        ///
        /// </summary>
        var $release;

        /// <summary>
        ///
        /// </summary>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        /// <param name="revision"></param>
        /// <param name="release"></param>
        function mod_version($major = 1, $minor = 0, $revision = 0, $release = nullChar, $stage = VersionStage_Stable)
        {
            $this->major = $major;
            $this->minor = $minor;
            $this->revision = $revision;
            $this->release = $release;
            $this->stage = $stage;
        }

        /// <summary>
        /// Convert the current mod Version to a string.
        /// </summary>
        /// <returns>A string of the form x.y.za</returns>
        function to_string()
        {
			if ($this->release == nullChar)
			{
				return sprintf('%s.%s.%s', $this->major, $this->minor, $this->revision);
			}
			else
			{
				return sprintf('%s.%s.%s%s%s', $this->major, $this->minor, mod::version_stage_to_char($this->stage), $this->revision, $this->release);
			}
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        function parse($input)
        {
            $trim_chars = array(' ', '\t', '\n', '\r', '\b');
            $temp_mod_version = new mod_version();
            preg_match("/(\\d+)\\.(\\d+)\\.(A|B|C|D|RC|)(\\d+)([a-z]?)/i", ltrim($input), $input_match);
            if (count($input_match) >= 5)
            {
                $input = sprintf("%s.%s.%s%s.%s", $input_match[1], $input_match[2], $input_match[3], $input_match[4], $input_match[5]);
            }
            $mv = explode('.', $input);
            //try
            //{
                if (count($mv) >= 1)
                {
                    $temp_mod_version->major = intval($mv[0]);
                }
                if (count($mv) >= 2)
                {
                    if (!($mv[1] == ''))
                    {
                        $temp_mod_version->minor = intval($mv[1]);
                    }
                }
                if (count($mv) >= 3)
                {
                    if (!($mv[2] == ''))
                    {
						if (strpos($mv[2], 'A') === 0)
                        {
                            $temp_mod_version->stage = VersionStage_Alpha;
                            $temp_mod_version->revision = intval(substr($mv[2], 1));
                        }
                        else if (strpos($mv[2], 'B') === 0)
                        {
                            $temp_mod_version->stage = VersionStage_Beta;
                            $temp_mod_version->revision = intval(substr($mv[2], 1));
                        }
                        else if (strpos($mv[2], 'C') === 0)
                        {
                            $temp_mod_version->stage = VersionStage_Gamma;
                            $temp_mod_version->revision = intval(substr($mv[2], 1));
                        }
                        else if (strpos($mv[2], 'D') === 0)
                        {
                            $temp_mod_version->stage = VersionStage_Delta;
                            $temp_mod_version->revision = intval(substr($mv[2], 1));
                        }
                        else if (strpos($mv[2], 'RC') === 0)
                        {
                            $temp_mod_version->stage = VersionStage_ReleaseCandidate;
                            $temp_mod_version->revision = intval(substr($mv[2], 2));
                        }
                        else
                        {
                            $temp_mod_version->stage = VersionStage_Stable;
                            $temp_mod_version->revision = intval($mv[2]);
                        }
                    }
                }
                if (count($mv) >= 4)
                {
                    if (strlen($mv[3]) > 0)
                    {
                        if (preg_match("/^([a-zA-Z])$/", $mv[3]) && $mv[3] != "\b")
                        {
                            $temp_mod_version->release = $mv[3][0];
                        }
                        else
                        {
                            $temp_mod_version->release = nullChar;
                        }
                    }
                    //if ($temp_mod_version->Release.GetHashCode() == 0)
                    //{
                    //    $temp_mod_version->release = nullChar;
                    //}
                }
                else
                {
                    $temp_mod_version->release = nullChar;
                }
            //}
            //catch (FormatException)
            //{
            //    throw new NotAModVersionException(string.Format("Error, not a valid mod version on input string: {0}", input));
            //}
            return $temp_mod_version;
        }

        /// <summary>
        /// Deep Compare
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        function equals($obj)
        {
        	if (get_class($obj) != "mod_version") return false;
            $mv = $obj;
            if (!$this->major === $mv->major) return false;
            if (!$this->minor === $mv->minor) return false;
            if (!$this->revision === $mv->revision) return false;
            if (!$this->release === $mv->release) return false;
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}


    }

    /// <summary>
    /// A list of changes for a given language in a mod History Entry
    /// </summary>
    class mod_history_change_log //: System.Collections.IEnumerable, System.Collections.ICollection
    {
        var $change_log; // private ArrayList
        var $language; // private string

        /// <summary>
        ///
        /// </summary>
        function mod_history_change_log()
        {
            $this->change_log = array();
        }

        /// <summary>
        ///
        /// </summary>
        function get_language()
        {
            return $this->language;
        }
        function set_language($value)
        {
            $this->language = $value;
        }

        /// <summary>
        ///
        /// </summary>
        //public string this[int index]
        //{
        //    // TODO:
        //    get
        //    {
        //        try
        //        {
        //            return (string)changeLog[index];
        //        }
        //        catch
        //        {
        //            return "";
        //        }
        //    }
        //    set
        //    {
        //        try
        //        {
        //            changeLog[index] = value;
        //        }
        //        catch
        //        {
        //            changeLog.Add(value);
        //        }
        //    }
        //}

        /// <summary>
        ///
        /// </summary>
        /// <param name="change"></param>
        function add($change)
        {
            $this->change_log[] = $change;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        function remove_at($index)
        {
            unset($this->change_log[$index]);
        }

        #region IEnumerable Members

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        //public IEnumerator GetEnumerator()
        //{
        //    return changeLog.GetEnumerator();
        //}

        #endregion

        #region ICollection Members

        /// <summary>
        ///
        /// </summary>
        //public bool IsSynchronized
        //{
        //    get
        //    {
        //        return changeLog.IsSynchronized;
        //    }
        //}

        /// <summary>
        ///
        /// </summary>
        function get_count()
        {
            return count($this->change_log);
        }

        // TODO: LEAVE COMMENTED OUT UNTIL SOMEONE NEEDS TO USE THIS
        /// <summary>
        ///
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        //public void CopyTo(Array array, int index)
        //{
        //    changeLog.CopyTo(array, index);
        //}

        /// <summary>
        ///
        /// </summary>
        //public object SyncRoot
        //{
        //    get
        //    {
        //        return changeLog.SyncRoot;
        //    }
        //}

        #endregion

        /// <summary>
        /// Deep Compare
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        function equals($obj)
        {
            if (get_class($obj) != "mod_version") return false;
            $mhcl = $obj;
            if (count($this->change_log) != count($mhcl->change_log)) return false;
            for ($i = 0; $i < count($this->change_log); $i++)
            {
                if (!($this->change_log[$i] === $mhcl->change_log[$i])) return false;
            }
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}

    }

    /// <summary>
    ///
    /// </summary>
    class mod_history_change_log_localised //: System.Collections.IEnumerable
    {
        var $change_logs; //private Hashtable

        /// <summary>
        ///
        /// </summary>
		function mod_history_change_log_localised()
        {
            $this->change_logs = array();
        }

        /// <summary>
        ///
        /// </summary>
        //public mod_history_change_log this[string language]
        //{
        //    get
        //    {
        //        ((mod_history_change_log)changeLogs[language]).Language = language;
        //        return (mod_history_change_log)changeLogs[language];
        //    }
        //    set
        //    {
        //        changeLogs[language] = value;
        //        ((mod_history_change_log)changeLogs[language]).Language = language;
        //    }
        //}

        function get($language)
        {
			$this->change_logs[$language]->Language = $language;
			return $this->change_logs[$language];
        }

        function set($language, $value)
        {
        	$this->change_logs[$language] = $value;
			$this->change_logs[$language]->Language = $language;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="language"></param>
        function add($value, $language)
        {
            $this->change_logs[$language] = $value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="language"></param>
        function remove($language)
        {
            unset($this->change_logs[$language]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        function get_count()
        {
            return count($this->change_logs);
        }

        #region IEnumerable Members

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        //public IEnumerator GetEnumerator()
        //{
        //    return changeLogs.GetEnumerator();
        //}

        #endregion

        /// <summary>
        /// Deep compare
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        function equals($obj)
        {
        	if (get_class($obj) != "mod_history_change_log_localised") return false;
            $mhcll = $obj;
            if (count($this->change_logs) != count($mhcll->change_logs)) return false;
            foreach ($this->change_logs as $okey => $ovalue)
            {
                if (!array_key_exists($okey, $mhcll->change_logs)) return false;
                if (!($this->change_logs[$okey] === $mhcll->change_logs[$okey])) return false;
            }
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        ///public override int GetHashCode()
        ///{
        ///    return base.GetHashCode();
        ///}

    }

    /// <summary>
    /// A mod History Entry
    /// </summary>
    class mod_history_entry
    {
        /// <summary>
        ///
        /// </summary>
        var $version; //public mod_version
        /// <summary>
        ///
        /// </summary>
        var $date; //public System.DateTime
        //public string_localised HistoryChanges;
        /// <summary>
        ///
        /// </summary>
        var $change_log; //public mod_history_change_log_localised

        /// <summary>
        ///
        /// </summary>
        /// <param name="HistoryVersion"></param>
        /// <param name="HistoryDate"></param>
        /// <param name="HistoryChanges"></param>
        function mod_history_entry($history_version = -1, $history_date = -1, $history_changes = -1) //mod_history_change_log_localised
        {
        	// PHP is nub so we have to cheat
        	if (is_int($history_version))
        	{
        		$history_version = new mod_version(0, 0, 0);
        	}
        	if (is_int($history_date) && $history_date == -1)
        	{
        		$history_date = time();
        	}
        	if (is_int($history_changes))
        	{
        		$history_changes = new mod_history_change_log_localised();
        	}
        	if (is_string($history_changes))
        	{
        		$temp = $history_changes;
        		$history_changes = new mod_history_change_log_localised();
        		$history_changes->add(new mod_history_change_log(), mod::get_default_language()); // TODO: en-gb is the default language
        		$tempb = $history_changes->get(mod::get_default_language());
        		$tempb->add($temp);
        	}
            $this->version = $history_version;
            $this->date = $history_date;
            $this->change_log = $history_changes;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="defaultLanguage"></param>
        /// <returns></returns>
        function to_string($default_language = null)
        {
        	if ($default_language == null)
        	{
        		$default_language == mod::get_default_language();
        	}
            $return_value = ''; // StringBuilder
            $return_value .= "## \n";
            $return_value .= "## " . date("Y-m-d", $this->date) . " - Version " . $this->Version.ToString() . "\n";
            // TODO: verify
            foreach ($this->change_log->change_logs as $language => $value)
            {
                //$language = $dictEntry.Key;
                $mhcl = $value;
                if ($language == $default_language)
                {
                    foreach ($mhcl->change_log as $le)
                    {
                        $return_value .= "##      -" . str_replace("\n", "## \n", $le) . "\n";
                    }
                }
            }
            if (count($this->change_log) == 0)
            {
                $return_value .= "## - \n";
            }
            return $return_value;
        }

        /// <summary>
        /// Deep Compare
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        function equals($obj)
        {
        	if (get_class($obj) != "mod_history_entry") return false;
            $mhe = $obj;
            if (!$this->change_log->equals($mhe->change_log)) return false;
            if (!$this->date === $mhe.Date) return false;
            if (!$this->version->equals($mhe->Version)) return false;
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}

    }

    define('ModHistory_Empty', "N/A");

    /// <summary>
    /// A collection of useful methods for organising mod_history entries.
    /// </summary>
    class mod_history //: System.Collections.IEnumerable, System.Collections.ICollection
    {
        /// <summary>
        /// The string which represents an empty mod Author field.
        /// </summary>
        //public const string Empty = "N/A";

        /// <summary>
        ///
        /// </summary>
        var $history; //public ArrayList

        /// <summary>
        ///
        /// </summary>
        function mod_history()
        {
            $this->history = array();
        }

        /// <summary>
        ///
        /// </summary>
        //public mod_history_entry this[int index]
        //{
        //    get
        //    {
        //        return (mod_history_entry)History[index];
        //    }
        //    set
        //    {
        //        History[index] = (mod_history_entry)value;
        //    }
        //}

        function get($index)
        {
        	return $this->history[$index];
        }
        function set($index, $value)
        {
        	$this->history[$index] = $value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="newhistory"></param>
        function add($new_history)
        {
            if ($this->history == null) $history = array();
            $this->history[] = $new_history;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        function insert($index, $value)
        {
            array_splice($this->history, $index, count($this->history), array_merge(array($value), array_slice($this->history, $index)));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        function remove_at($index)
        {
            unset($this->history[$index]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        function remove($value)
        {
        	// TODO: verify
            unset($value);
        }

        #region IEnumerable Members

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        //public IEnumerator GetEnumerator()
        //{
        //    return History.GetEnumerator();
        //}

        #endregion

        #region ICollection Members

        /// <summary>
        ///
        /// </summary>
        //public bool IsSynchronized
        //{
        //    get
        //    {
        //        return History.IsSynchronized;
        //    }
        //}

        /// <summary>
        ///
        /// </summary>
        function get_count()
        {
            return count($this->history);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        //public void CopyTo(Array array, int index)
        //{
        //    History.CopyTo(array, index);
        //}

        /// <summary>
        ///
        /// </summary>
        //public object SyncRoot
        //{
        //    get
        //    {
        //        return History.SyncRoot;
        //    }
        //}

        #endregion

        /// <summary>
        ///
        /// </summary>
        /// <param name="defaultLanguage"></param>
        /// <returns></returns>
        function to_string($default_language = null)
        {
        	if ($default_language == null)
        	{
        		$default_language == mod::get_default_language();
        	}
            $return_value = ''; //System.Text.StringBuilder
            if (count($this->history) > 0)
            {
                $return_value .= "##############################################################\n";
                $return_value .= "## mod History:\n";
                foreach ($this->history as $mhe)
                {
                    $return_value .= $mhe->to_string($default_language);
                }
            }
            if (strlen($return_value) != 0)
            {
            	$return_value = "\n" . $return_value . "##";
            }
            return $return_value;
        }

        /// <summary>
        /// Deep compare
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        function equals($obj)
        {
        	if (get_class($obj) != "mod_history") return false;
            $mh = $obj;
            if (count($this->history) != count($mh->history)) return false;
            for ($i = 0; $i < count($this->history); $i++)
            {
                if (!($this->history[$i]->equals($mh->history[$i]))) return false;
            }
            return true;
        }


    }


?>