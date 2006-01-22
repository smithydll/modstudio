/***************************************************************************
 *                         LanguageSelectionDialog.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: LanguageSelectionDialog.cs,v 1.4 2006-01-22 23:38:12 smithydll Exp $
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
namespace ModFormControls
{
	/// <summary>
	/// 
	/// </summary>
	public class LanguageSelectionDialog : System.Windows.Forms.CommonDialog
	{
		private string language = null;

		/// <summary>
		/// 
		/// </summary>
		[DefaultValue(null)]
		public string Language
		{
			get
			{
				return language;
			}

			set
			{
				language = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public LanguageSelectionDialog() 
		{

		}

		/// <summary>
		/// 
		/// </summary>
		public event LanguageSelectionDialogBox.LanguageSelectionDialogBoxSaveHandler Save;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hWndOwner"></param>
		/// <returns></returns>
		protected override bool RunDialog(IntPtr hWndOwner)
		{
			LanguageSelectionDialogBox dialogInstance = null;
			bool okTriggered = false;
			try
			{
				dialogInstance = new LanguageSelectionDialogBox();
				dialogInstance.Owner = (Form.FromHandle(hWndOwner) as Form);
				dialogInstance.Language = this.language;
				if (dialogInstance.ShowDialog() == DialogResult.OK)
				{
					okTriggered = true;
					this.language = dialogInstance.Language; 
					this.Save(this, new LanguageSelectionDialogBoxSaveEventArgs(language));
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
			this.language = null;

		}
	}
} 