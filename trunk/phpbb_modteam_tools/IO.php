<?php
/*
 * Phpbb.ModTeam.Tools (PHP)
 * http://smithydll.id.au/
 * Copyright © 2007, David Lachlan Smith
 *
 * $Id: IO.php,v 1.1 2008-02-24 20:16:02 smithydll Exp $
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

/* This code is officially abandoned. Use the transfer class avaliable in phpBB3 */

/*
 * The point of this namespace is to group all the Package manager file access methods under a single method
 */
require_once("./IO.File.php");
require_once("./IO.Ftp.php");
require_once("./IO.Sftp.php");

class IO // abstract
{
	var $globalRootPath; // string

	function Move($from, $to)
	{
		exit("Cannot call Abstract methods on an Abstract type");
	}

	function Copy($from, $to)
	{
		exit("Cannot call Abstract methods on an Abstract type");
	}

	function WriteTextFile($file, $fileName)
	{
		exit("Cannot call Abstract methods on an Abstract type");
	}

	function ReadTextFile($fileName)
	{
		exit("Cannot call Abstract methods on an Abstract type");
	}

	function WriteBinaryFile($file, $fileName)
	{
		exit("Cannot call Abstract methods on an Abstract type");
	}

	function ReadBinaryFile($fileName)
	{
		exit("Cannot call Abstract methods on an Abstract type");
	}

	function CreateDirectory($path)
	{
		exit("Cannot call Abstract methods on an Abstract type");
	}

	function SelfTest()
	{
		exit("Cannot call Abstract methods on an Abstract type");
	}

	function TestConnection()
	{
		exit("Cannot call Abstract methods on an Abstract type");
	}
}

?>