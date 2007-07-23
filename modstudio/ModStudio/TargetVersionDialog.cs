using System;
using System.ComponentModel;
using System.Windows.Forms;
using Phpbb.ModTeam.Tools;
using Phpbb.ModTeam.Tools.DataStructures;

/*
 * Inspired by:
 * http://groups.google.com.au/group/microsoft.public.dotnet.framework.windowsforms/browse_thread/thread/50554a22d9d040d5/c88268fc037ccad9?lnk=st&q=c%23+CommonDialog+inherit&rnum=5&hl=en#c88268fc037ccad9
 */
namespace ModStudio
{
    public class TargetVersionDialog : System.Windows.Forms.CommonDialog
    {
        private TargetVersionCases cases = new TargetVersionCases();

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(null)]
        public TargetVersionCases Cases
        {
            get
            {
                return cases;
            }

            set
            {
                cases = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TargetVersionDialog()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public event TargetVersionDialogBox.TargetVersionDialogBoxSaveHandler Save;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWndOwner"></param>
        /// <returns></returns>
        protected override bool RunDialog(IntPtr hWndOwner)
        {
            TargetVersionDialogBox dialogInstance = null;
            bool okTriggered = false;
            try
            {
                dialogInstance = new TargetVersionDialogBox();
                dialogInstance.Owner = (Form.FromHandle(hWndOwner) as Form);
                dialogInstance.Cases = this.cases;
                if (dialogInstance.ShowDialog() == DialogResult.OK)
                {
                    okTriggered = true;
                    this.cases = dialogInstance.Cases;
                    this.Save(this, new TargetVersionDialogBoxSaveEventArgs(cases));
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
            this.cases = new TargetVersionCases();
        }
    }
}
