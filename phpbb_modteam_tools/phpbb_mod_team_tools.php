<?php
/*
 * Phpbb.ModTeam.Tools (PHP)
 * http://smithydll.id.au/
 * Copyright © 2007, David Lachlan Smith
 *
 * $Id: phpbb_mod_team_tools.php,v 1.1 2008-02-24 20:16:03 smithydll Exp $
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

/*******************************************
 * INSTRUCTIONS FOR MODIFYING THIS LIBRARY *
 *******************************************
 *
 * This code lives in the mod studio repository, make sure that this
 * repository contains the most up-to-date version of this code. Do not work
 * out of any other repositories.
 *
 * Bug fixes in the use of the PHP language
 *  - You may make these bug fixes at any time so long as you comitt the
 *    fixes to the correct repository.
 *
 * Refactoring code to suit project coding guidelines
 *  - Don't ask, just do it (please).
 *  - Inform everyone that you've done it so they can adjust any hooks their
 *    programme has into this library.
 *  - Remove this part of the message once the library has been refactored
 *  - See Also: http://area51.phpbb.com/docs/coding-guidelines.html
 *
 * Making changes to the algorithm
 *  - Changes in the algorithm must not be made without applying applicable
 *    changes to the c# version of this code first. The two code bases must
 *    remain in sync with each other. This ensures that all tools behave
 *    in the same predictable manner. If you cannot do this, ask someone who
 *    can.
 *
 * Accept Copyright Assignment
 * - All changes need to be signed over to copyright under my name before they
 *	 will be included in the official trunk.
 * - Authors retain all moral rights where applicable by law.
 *
 */

 if (!isset($phpbb_mod_team_tools_path)) $phpbb_mod_team_tools_path = './';

class path
{
	function combine($str1, $str2)
	{
		return rtrim($str1, '/') . '/' . $str2;
	}
}

include($phpbb_mod_team_tools_path . 'mod.php');
include($phpbb_mod_team_tools_path . 'mod_header.php');
include($phpbb_mod_team_tools_path . 'mod_actions.php');
include($phpbb_mod_team_tools_path . 'data_structures.php');
include($phpbb_mod_team_tools_path . 'modx_mod.php');
include($phpbb_mod_team_tools_path . 'text_mod.php');
include($phpbb_mod_team_tools_path . 'validation.php');
//require_once($phpbb_mod_team_tools_path . 'IO.php');
//require_once($phpbb_mod_team_tools_path . 'Sql.php');
//require_once($phpbb_mod_team_tools_path . '');

?>