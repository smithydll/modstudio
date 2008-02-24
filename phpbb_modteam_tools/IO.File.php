<?php
/*
 * Phpbb.ModTeam.Tools (PHP)
 * http://smithydll.id.au/
 * Copyright © 2007, David Lachlan Smith
 *
 * $Id: IO.File.php,v 1.1 2008-02-24 20:16:02 smithydll Exp $
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
 * Previously known as the server method in EasyMOD, it is now known as File IO
 */

class IO_File extends IO // abstract
{
	var $filePermissions;
	var $directoryPermissions;

	function IO_File($rootPath)
	{
		$this->globalRootPath = $rootPath;
		$this->filePermissions = 0664; // RW RW R
		$this->directoryPermissions = 0777; // RWX RWX RWX
	}

	function Move($from, $to)
	{
		exit("Cannot call Abstract methods on an Abstract type");
	}

	function Copy($from, $to)
	{
			$from = ((!strstr($from, $this->globalRootPath)) ? $this->globalRootPath : '') . $from;
			$to = $this->globalRootPath . str_replace($this->globalRootPath, '', $to);

			// copy the file
			@unlink($to);
			$error = @copy($from, $to);
			@chmod($to, $this->filePermissions);

			return $error;
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
		@unlink($file);
		$fileResource = @fopen($fileName, 'w');
		$error = @fwrite($fileResource, $file);
		@fclose($fileResource);

		@chmod($fileName, $this->filePermissions);
		return $error;
	}

	function ReadBinaryFile($fileName)
	{
		exit("Cannot call Abstract methods on an Abstract type");
	}

	function CreateDirectory($path)
	{

		$dir = explode('/', $path);
		$dirs = '';
		for($i = 0, $total = count($dir); $i < count($dir); $i++)
		{
			$result = true;
			if ($dir[$i] == '..' || $dir[$i] == '.')
			{
				continue;
			}
			$currentDirectory = $dir[$i] . '/';
			if (!file_exists($this->globalRootPath . $dirs . $cur_dir))
			{
				// make the directory
				$result = @mkdir($this->globalRootPath . $dirs . $currentDirectory);
				@chmod($this->globalRootPath . $dirs . $currentDirectory, $this->directoryPermissions);
			}
			$dirs .= $currentDirectory;
		}

		return $result;
	}

	function SelfTest()
	{
		exit("Cannot call Abstract methods on an Abstract type");
	}

	function TestConnection()
	{
		return true;
	}
}

?>