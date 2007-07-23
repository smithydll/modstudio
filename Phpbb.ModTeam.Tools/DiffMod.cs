/***************************************************************************
 *                                 DiffMod.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: DiffMod.cs,v 1.2 2007-07-23 11:17:24 smithydll Exp $
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
	/// Summary description for DiffMod.
	/// </summary>
	public class DiffMod : Mod, IMod
	{
		/// <summary>
		/// 
		/// </summary>
		public DiffMod() : base()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		public override void Read(string fileName)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		public override void Write(string fileName)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override Validation.Report Validate(string fileName)
		{
			throw new NotSupportedException();
		}
		#region IMod Members

		/// <summary>
		/// 
		/// </summary>
		public new Phpbb.ModTeam.Tools.DataStructures.ModFormats LastReadFormat
		{
			get
			{
				return base.LastReadFormat;
			}
			set
			{
				base.LastReadFormat = value;
			}
		}

		#endregion
	}
}
