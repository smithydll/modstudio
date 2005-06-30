/***************************************************************************
 *                              ModValidator.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: ModValidator.cs,v 1.1 2005-06-30 06:21:00 smithydll Exp $
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

namespace ModTemplateTools
{
	/// <summary>
	/// Summary description for ModValidator.
	/// </summary>
	public class ModValidator
	{

		/// <summary>
		/// 
		/// </summary>
		public struct ModValidatorReport
		{
			/// <summary>
			/// 
			/// </summary>
			public bool passed;

			/// <summary>
			/// 
			/// </summary>
			public string report;

			/// <summary>
			/// 
			/// </summary>
			public int warnings;

			/// <summary>
			/// 
			/// </summary>
			public int passed_header;

			/// <summary>
			/// 
			/// </summary>
			public int passed_body;

			/// <summary>
			/// 
			/// </summary>
			public int passed_html;

			/// <summary>
			/// 
			/// </summary>
			public int passed_php;

			/// <summary>
			/// 
			/// </summary>
			public double creation_time;

			/// <summary>
			/// 
			/// </summary>
			public bool report_bbcode;
		}

		/// <summary>
		/// 
		/// </summary>
		public ModValidator()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
