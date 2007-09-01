/***************************************************************************
 *                           OpenActionDialogBox.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: NewProjectDialogBox.cs,v 1.1 2007-09-01 13:52:38 smithydll Exp $
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using Phpbb.ModTeam.Tools.DataStructures;

namespace ModStudio
{
    /// <summary>
    /// Summary description for OpenActionDialogBox.
    /// </summary>
    class NewProjectDialogBox : System.Windows.Forms.Form
    {
        private Button button1;
        private Button button2;
        private Label label1;
        private TextBox textBox1;
        private Button button3;
        private Label label2;
        private TextBox textBox2;
        private FolderBrowserDialog folderBrowserDialog1;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private Label label3;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
		/// 
		/// </summary>
        public NewProjectDialogBox()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
        }

        #region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(326, 239);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(407, 239);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 184);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(69, 184);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(332, 20);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "Untitled Project";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(407, 210);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "Browse";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 216);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Location:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(69, 213);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(332, 20);
            this.textBox2.TabIndex = 6;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(124, 64);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(72, 17);
            this.radioButton1.TabIndex = 7;
            this.radioButton1.Text = "phpBB2.0";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Location = new System.Drawing.Point(124, 124);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(72, 17);
            this.radioButton2.TabIndex = 8;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "phpBB3.0";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(66, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(335, 34);
            this.label3.TabIndex = 9;
            this.label3.Text = "Create a new project for a phpBB modification. Choose a phpBB version, a project " +
                "name, and where you want to put it.";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ModStudio.Properties.Resources.phpbb_logo;
            this.pictureBox2.Location = new System.Drawing.Point(257, 111);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(144, 54);
            this.pictureBox2.TabIndex = 11;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ModStudio.Properties.Resources.logo_phpBB_med;
            this.pictureBox1.Location = new System.Drawing.Point(276, 46);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(125, 59);
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // NewProjectDialogBox
            // 
            this.ClientSize = new System.Drawing.Size(494, 274);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewProjectDialogBox";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create a New Project";
            this.Load += new System.EventHandler(this.NewProjectDialogBox_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public string ProjectName
        {
            get
            {
                return textBox1.Text;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ProjectPath
        {
            get
            {
                return textBox2.Text;
            }
        }

        public TargetVersionCases PhpbbVersion
        {
            get
            {
                TargetVersionCases tvc = new TargetVersionCases();
                if (radioButton1.Checked)
                {
                    tvc.Primary = "2.0";
                    tvc.Add(new TargetVersionCase(TargetVersionComparisson.EqualTo, TargetVersionPart.Major, 2));
                    tvc.Add(new TargetVersionCase(TargetVersionComparisson.EqualTo, TargetVersionPart.Minor, 0));
                }
                else if (radioButton2.Checked)
                {
                    tvc.Primary = "3.0";
                    tvc.Add(new TargetVersionCase(TargetVersionComparisson.EqualTo, TargetVersionPart.Major, 3));
                    tvc.Add(new TargetVersionCase(TargetVersionComparisson.EqualTo, TargetVersionPart.Minor, 0));
                }
                return tvc;
            }
        }

        private void NewProjectDialogBox_Load(object sender, EventArgs e)
        {
            textBox2.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Mods");
        }

        public delegate void NewProjectDialogBoxSaveNewHandler(object sender, NewProjectDialogBoxSaveNewEventArgs e);

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                this.DialogResult = DialogResult.None;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(textBox2.Text))
            {
                folderBrowserDialog1.SelectedPath = textBox2.Text;
            }
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog1.SelectedPath;
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class NewProjectDialogBoxSaveNewEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public string ProjectName;

        /// <summary>
        /// 
        /// </summary>
        public string ProjectPath;

        /// <summary>
        /// 
        /// </summary>
        public TargetVersionCases PhpbbVersion;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="projectPath"></param>
        /// <param name="phpbbVersion"></param>
        public NewProjectDialogBoxSaveNewEventArgs(string projectName, string projectPath, TargetVersionCases phpbbVersion)
        {
            this.ProjectName = projectName;
            this.ProjectPath = projectPath;
            this.PhpbbVersion = phpbbVersion;
        }
    }
}
