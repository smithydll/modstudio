/***************************************************************************
 *                           HistoryEditorDialog.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: HistoryEditorDialog.cs,v 1.8 2006-01-21 02:50:52 smithydll Exp $
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
using ModTemplateTools.DataStructures;

/*
 * Inspired by:
 * http://groups.google.com.au/group/microsoft.public.dotnet.framework.windowsforms/browse_thread/thread/50554a22d9d040d5/c88268fc037ccad9?lnk=st&q=c%23+CommonDialog+inherit&rnum=5&hl=en#c88268fc037ccad9
 */
namespace ModStudio
{
	/// <summary>
	/// 
	/// </summary>
	public class HistoryEditorDialog : System.Windows.Forms.CommonDialog
	{
		private ModHistoryEntry entry = new ModHistoryEntry();
		private bool localised = false;

		/// <summary>
		/// 
		/// </summary>
		[DefaultValue(false)]
		public bool Localised 
		{
			get 
			{
				return localised;
			}
			set
			{
				localised = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[DefaultValue(null)]
		public ModHistoryEntry Entry
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
		public HistoryEditorDialog() 
		{

		}

		/// <summary>
		/// 
		/// </summary>
		public event HistoryEditorDialogBox.HistoryEditorDialogBoxSaveHandler Save;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hWndOwner"></param>
		/// <returns></returns>
		protected override bool RunDialog(IntPtr hWndOwner)
		{
			HistoryEditorDialogBox dialogInstance = null;
			bool okTriggered = false;
			try
			{
				dialogInstance = new HistoryEditorDialogBox();
				dialogInstance.Owner = (Form.FromHandle(hWndOwner) as Form);
				dialogInstance.HistoryEntry = this.entry;
				dialogInstance.LanguageSelectorVisible = localised;
				if (dialogInstance.ShowDialog() == DialogResult.OK)
				{
					okTriggered = true;
					this.entry = dialogInstance.HistoryEntry; 
					this.Save(this, new HistoryEditorDialogBoxSaveEventArgs(entry));
					//this.Reset(); // reset the entry after it's been finished with
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
			this.entry = new ModHistoryEntry();
			this.index = -1;

		}
	}
} 