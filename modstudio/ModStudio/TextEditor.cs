using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace ModStudio
{
	/// <summary>
	/// Summary description for TextEditor.
	/// </summary>
    public class TextEditor : System.Windows.Forms.Form, IEditor
	{
        private ICSharpCode.TextEditor.TextEditorControl textEditorControl1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        /// <summary>
        /// 
        /// </summary>
		public TextEditor()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public TextEditor(string fileName) : this()
        {
            if (File.Exists(fileName))
            {
                textEditorControl1.Text = OpenTextFile(fileName);

                if (fileName.EndsWith(".php"))
                {
                    // PHP highlighting
                    ICSharpCode.TextEditor.Document.IHighlightingStrategy ihls = ICSharpCode.TextEditor.Document.HighlightingStrategyFactory.CreateHighlightingStrategyForFile(fileName);
                    textEditorControl1.Document.HighlightingStrategy = ihls;

                    textEditorControl1.Document.TextEditorProperties.IndentStyle = ICSharpCode.TextEditor.Document.IndentStyle.Auto;
                }
                else if (fileName.EndsWith(".html"))
                {
                    // HTML highlighting
                    ICSharpCode.TextEditor.Document.IHighlightingStrategy ihls = ICSharpCode.TextEditor.Document.HighlightingStrategyFactory.CreateHighlightingStrategyForFile(fileName);
                    textEditorControl1.Document.HighlightingStrategy = ihls;
                }
            }
            else
            {
                MessageBox.Show("Cannot open file, file does not exist");
                this.Close();
            }
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.textEditorControl1 = new ICSharpCode.TextEditor.TextEditorControl();
            this.SuspendLayout();
            // 
            // textEditorControl1
            // 
            this.textEditorControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textEditorControl1.Location = new System.Drawing.Point(0, 0);
            this.textEditorControl1.Name = "textEditorControl1";
            this.textEditorControl1.ShowEOLMarkers = true;
            this.textEditorControl1.ShowSpaces = true;
            this.textEditorControl1.ShowTabs = true;
            this.textEditorControl1.ShowVRuler = true;
            this.textEditorControl1.Size = new System.Drawing.Size(314, 264);
            this.textEditorControl1.TabIndex = 0;
            // 
            // TextEditor
            // 
            this.ClientSize = new System.Drawing.Size(314, 264);
            this.Controls.Add(this.textEditorControl1);
            this.Name = "TextEditor";
            this.ResumeLayout(false);

		}
		#endregion

        #region IEditor Members

        /// <summary>
        /// 
        /// </summary>
        public void SaveFile()
        {
            // TODO: verification etc...
            SaveTextFile(textEditorControl1.Text, this.Text);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SaveFileAs()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetModified()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetUnmodified()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        public void DoCut()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        public void DoPaste()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsCopy()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsCut()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsCutCopyPaste()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsPaste()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        public void DoCopy()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region file handling

        /// <summary>
        /// Open a text file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected static string OpenTextFile(string fileName)
        {
            StreamReader myStreamReader;
            string temp;
            try
            {
                myStreamReader = File.OpenText(fileName);
                temp = myStreamReader.ReadToEnd();
                myStreamReader.Close();
            }
            catch
            {
                temp = "";
            }
            return temp;
        }

        /// <summary>
        /// Save a text file
        /// </summary>
        /// <param name="fileToSave"></param>
        /// <param name="fileName"></param>
        protected static void SaveTextFile(string fileToSave, string fileName)
        {
            StreamWriter myStreamWriter = File.CreateText(fileName);
            myStreamWriter.Write(fileToSave);
            myStreamWriter.Close();
        }

        #endregion
    }
}
