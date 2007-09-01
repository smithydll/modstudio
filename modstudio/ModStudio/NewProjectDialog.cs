/***************************************************************************
 *                            OpenActionDialog.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: NewProjectDialog.cs,v 1.1 2007-09-01 13:52:38 smithydll Exp $
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
using Phpbb.ModTeam.Tools.DataStructures;

/*
 * Inspired by:
 * http://groups.google.com.au/group/microsoft.public.dotnet.framework.windowsforms/browse_thread/thread/50554a22d9d040d5/c88268fc037ccad9?lnk=st&q=c%23+CommonDialog+inherit&rnum=5&hl=en#c88268fc037ccad9
 */
namespace ModStudio
{
    /// <summary>
    /// 
    /// </summary>
    class NewProjectDialog : System.Windows.Forms.CommonDialog
    {
        public event NewProjectDialogBox.NewProjectDialogBoxSaveNewHandler NewProject;

        public override void Reset()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override bool RunDialog(IntPtr hWndOwner)
        {
            NewProjectDialogBox dialogInstance = null;
            bool okTriggered = false;
            try
            {
                dialogInstance = new NewProjectDialogBox();
                //dialogInstance.SetPhpbbVersion(phpbbVersion);
                dialogInstance.Owner = (Form.FromHandle(hWndOwner) as Form);
                //dialogInstance.textBoxFile.Text = this.fileName;
                if (dialogInstance.ShowDialog() == DialogResult.OK)
                {
                    okTriggered = true;
                    //this.fileName = dialogInstance.textBoxFile.Text;
                    this.NewProject(this, new NewProjectDialogBoxSaveNewEventArgs(dialogInstance.ProjectName, dialogInstance.ProjectPath, dialogInstance.PhpbbVersion));
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
    }
}
