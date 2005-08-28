/***************************************************************************
 *                           AuthorEditorDialog.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: AuthorEditorDialog.cs,v 1.3 2005-08-28 02:59:59 smithydll Exp $
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
using System.ComponentModel;
using System.Windows.Forms;
using ModTemplateTools;

/*
 * Inspired by:
 * http://groups.google.com.au/group/microsoft.public.dotnet.framework.windowsforms/browse_thread/thread/50554a22d9d040d5/c88268fc037ccad9?lnk=st&q=c%23+CommonDialog+inherit&rnum=5&hl=en#c88268fc037ccad9
 */
namespace ModStudio
{
	/// <summary>
	/// 
	/// </summary>
	public class AuthorEditorDialog : System.Windows.Forms.CommonDialog
	{
		private ModTemplateTools.PhpbbMod.ModAuthorEntry entry = new ModTemplateTools.PhpbbMod.ModAuthorEntry(true);
		/// <summary>
		/// 
		/// </summary>
		[DefaultValue(null)]
		public ModTemplateTools.PhpbbMod.ModAuthorEntry Entry
		{
			get
			{
				return entry;
			}

			set
			{
				entry = value;
			}
		}

		private int index = -1;
		/// <summary>
		/// 
		/// </summary>
		[DefaultValue(-1)]
		public int Index
		{
			get
			{
				return index;
			}
			set
			{
				index = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public AuthorEditorDialog() 
		{

		}

		/// <summary>
		/// 
		/// </summary>
		public event AuthorEditorDialogBox.AuthorEditorDialogBoxSaveHandler Save;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hWndOwner"></param>
		/// <returns></returns>
		protected override bool RunDialog(IntPtr hWndOwner)
		{
			AuthorEditorDialogBox dialogInstance = null;
			bool okTriggered = false;
			try
			{
				dialogInstance = new AuthorEditorDialogBox();
				dialogInstance.Owner = (Form.FromHandle(hWndOwner) as Form);
				dialogInstance.Entry = this.entry;
				if (dialogInstance.ShowDialog() == DialogResult.OK)
				{
					okTriggered = true;
					this.entry = dialogInstance.Entry; 
					this.Save(this, new AuthorEditorDialogBoxSaveEventArgs(entry));
				}
			}

			finally
			{
				if (dialogInstance != null)
				{
					dialogInstance.Dispose();
				}
			}
			return okTriggered;
		}

		/// <summary>
		/// 
		/// </summary>
		public override void Reset()
		{
			this.index = -1;
			this.entry = new ModTemplateTools.PhpbbMod.ModAuthorEntry(true);
		}
	}
} 