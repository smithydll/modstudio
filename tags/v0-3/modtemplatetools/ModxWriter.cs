/***************************************************************************
 *                               ModxWriter.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: ModxWriter.cs,v 1.2 2006-02-17 04:08:23 smithydll Exp $
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
using System.IO;
using System.Text;
using System.Xml;

/* Inspired by http://www.dotnet247.com/247reference/msgs/58/294108.aspx */

namespace ModTemplateTools
{
	/// <summary>
	/// Summary description for ModxWriter.
	/// </summary>
	public class ModxWriter : XmlTextWriter
	{

		private string xslStylesheetFilename;
		private bool xmlDeclWritten;

		public ModxWriter(StringWriter sw, string stylesheetFilename) : base(sw)
		{
			base.Formatting = Formatting.Indented;
			xslStylesheetFilename = stylesheetFilename;
			xmlDeclWritten = false;
		}

		public override void WriteStartDocument(bool standalone)
		{
			xmlDeclWritten = true;
			string standaloneString = "no";
			if (standalone) standaloneString = "yes";
			//base.WriteStartDocument(standalone);
			base.WriteProcessingInstruction("xml", String.Format("version=\"1.0\" encoding=\"utf-8\" standalone=\"{0}\"", standaloneString));
			base.WriteProcessingInstruction("xml-stylesheet", String.Format("type=\"text/xsl\" href=\"{0}\"", xslStylesheetFilename));
			base.WriteComment("For security purposes, please check: http://www.phpbb.com/mods/ for the latest version of this MOD. Although MODs are checked before being allowed in the MODs Database there is no guarantee that there are no security problems within the MOD. No support will be given for MODs not found within the MODs Database which can be found at http://www.phpbb.com/mods/");
		}

		public override void WriteStartElement(string prefix, string localName, string nsUri)
		{
			if (!xmlDeclWritten)
			{
				this.WriteStartDocument(true);
			}
			base.WriteStartElement(prefix, localName, nsUri);
		}

		public override void WriteString(string text)
		{
			if (text.IndexOf('>') >= 0 || text.IndexOf('<') >= 0 || text.IndexOf('&') >= 0)
			{
				// CDATA doesn't let you use ']]>' so fall back to WriteString
				if (text.IndexOf("]]>") >= 0)
				{
					base.WriteString(text);
				}
				else
				{
					base.WriteCData(text);
				}
			}
			else
			{
				base.WriteString(text);
			}
		}
	}
}
