/***************************************************************************
 *                            OpenActionDialog.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: OpenActionDialog.cs,v 1.10 2006-07-03 13:05:58 smithydll Exp $
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

/*
 * Inspired by:
 * http://groups.google.com.au/group/microsoft.public.dotnet.framework.windowsforms/browse_thread/thread/50554a22d9d040d5/c88268fc037ccad9?lnk=st&q=c%23+CommonDialog+inherit&rnum=5&hl=en#c88268fc037ccad9
 */
namespace ModStudio
{
	/// <summary>
	/// 
	/// </summary>
	public class OpenActionDialog : System.Windows.Forms.CommonDialog
	{
		private string filename = null;
		/// <summary>
		/// 
		/// </summary>
		[DefaultValue(null)]
		public string fileName
		{
			get
			{
				return filename;
			}

			set
			{
				filename = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public OpenActionDialog() 
		{

		}

		/// <summary>
		/// 
		/// </summary>
		public event OpenActionDialogBox.OpenActionDialogBoxSaveNewHandler SaveNew;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hWndOwner"></param>
		/// <returns></returns>
		protected override bool RunDialog(IntPtr hWndOwner)
		{
			OpenActionDialogBox dialogInstance = null;
			bool okTriggered = false;
			try
			{
				dialogInstance = new OpenActionDialogBox();
				dialogInstance.Owner = (Form.FromHandle(hWndOwner) as Form);
				dialogInstance.textBoxFile.Text = this.fileName;
				if (dialogInstance.ShowDialog() == DialogResult.OK)
				{
					okTriggered = true;
					this.fileName = dialogInstance.textBoxFile.Text; 
					this.SaveNew(this, new OpenActionDialogBoxSaveNewEventArgs(dialogInstance.textBoxFile.Text));
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
			this.fileName = null;

		}
	}
} 