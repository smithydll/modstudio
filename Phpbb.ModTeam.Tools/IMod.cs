/***************************************************************************
 *                                  IMod.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: IMod.cs,v 1.1 2006-07-03 12:49:23 smithydll Exp $
 *
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

using System;
using Phpbb.ModTeam.Tools.DataStructures;

namespace Phpbb.ModTeam.Tools
{
	/// <summary>
	/// Defines Read, Write and Validation methods for all MODs.
	/// </summary>
	public interface IMod
	{

		/// <summary>
		/// When implemented by a class, reads a MOD in the format supported by the class.
		/// </summary>
		/// <param name="fileName">Path to file to read.</param>
		void Read(string fileName);

		/// <summary>
		/// When implemented by a class, writes a MOD to a file in the format supported by the class.
		/// </summary>
		/// <param name="fileName">Path of the file to be written.</param>
		void Write(string fileName);

		/// <summary>
		/// When implemented by a class, validates a MOD file to the format supported by the class.
		/// </summary>
		/// <param name="fileName">Path to file to Validate.</param>
		/// <returns></returns>
		Validation.Report Validate(string fileName);

		/// <summary>
		/// When implemented by a class, returns a string representation of the MOD to the format supported by the class.
		/// </summary>
		string ToString();

		/// <summary>
		/// The format the MOD was last read into.
		/// </summary>
		ModFormats LastReadFormat
		{
			get;
			set;
		}

	}
}
