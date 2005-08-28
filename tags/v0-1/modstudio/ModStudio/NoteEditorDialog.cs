/***************************************************************************
 *                            NoteEditorDialog.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: NoteEditorDialog.cs,v 1.3 2005-08-28 02:59:59 smithydll Exp $
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
	public class NoteEditorDialog : System.Windows.Forms.CommonDialog
	{
		private string note = null;
		private string type = null;
		/// <summary>
		/// 
		/// </summary>
		[DefaultValue(null)]
		public string Note
		{
			get
			{
				return note;
			}

			set
			{
				note = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[DefaultValue(null)]
		public string Type
		{
			get
			{
				return type;
			}
			set
			{
				type = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public NoteEditorDialog() 
		{

		}

		/// <summary>
		/// 
		/// </summary>
		public event NoteEditorDialogBox.NoteEditorDialogBoxSaveHandler Save;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hWndOwner"></param>
		/// <returns></returns>
		protected override bool RunDialog(IntPtr hWndOwner)
		{
			NoteEditorDialogBox dialogInstance = null;
			bool okTriggered = false;
			try
			{
				dialogInstance = new NoteEditorDialogBox();
				dialogInstance.Owner = (Form.FromHandle(hWndOwner) as Form);
				dialogInstance.Note = this.note;
				if (dialogInstance.ShowDialog() == DialogResult.OK)
				{
					okTriggered = true;
					this.note = dialogInstance.Note; 
					this.Save(this, new NoteEditorDialogBoxSaveEventArgs(note, type));
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
			this.note = null;

		}
	}
} 