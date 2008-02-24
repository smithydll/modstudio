<?php
/*
 * Phpbb.ModTeam.Tools (PHP)
 * http://smithydll.id.au/
 * Copyright © 2007, David Lachlan Smith
 *
 * $Id: modx.test.php,v 1.1 2008-02-24 20:16:03 smithydll Exp $
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

$mtime = microtime();
$mtime = explode(" ", $mtime);
$mtime = $mtime[1] + $mtime[0];
$starttime = $mtime;

require_once('Phpbb.ModTeam.Tools.php');

$mod = new ModxMod();
$mod->Read('./test.xml');
echo '<textarea style="width: 98%; height: 95%;">';
$textmod = new TextMod('./');
$textmod->Actions = $mod->Actions;
$textmod->Header = $mod->Header;
echo $textmod->ToString();
echo '</textarea>';

$mtime = microtime();
$mtime = explode(" ", $mtime);
$mtime = $mtime[1] + $mtime[0];
$endtime = $mtime;
$totaltime = ($endtime - $starttime);

echo "Page generated in: ";
echo $totaltime;
echo " seconds.";

?>