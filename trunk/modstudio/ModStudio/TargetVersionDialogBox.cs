using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Phpbb.ModTeam.Tools.DataStructures;

namespace ModStudio
{
    /// <summary>
    /// 
    /// </summary>
    public class TargetVersionDialogBox : System.Windows.Forms.Form
    {

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox textBoxTargetVersionPrimary;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ListView listViewTargetVersionCases;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox2;
        private ComboBox comboBox3;
        private IContainer components;

        /// <summary>
        /// 
        /// </summary>
        public TargetVersionDialogBox()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.textBoxTargetVersionPrimary = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.listViewTargetVersionCases = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.panel3 = new System.Windows.Forms.Panel();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.textBoxTargetVersionPrimary);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(504, 32);
            this.panel1.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button2.Location = new System.Drawing.Point(324, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "Add Case";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button5
            // 
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button5.Location = new System.Drawing.Point(405, 4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(96, 23);
            this.button5.TabIndex = 9;
            this.button5.Text = "Remove Case";
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // textBoxTargetVersionPrimary
            // 
            this.textBoxTargetVersionPrimary.Location = new System.Drawing.Point(112, 8);
            this.textBoxTargetVersionPrimary.Name = "textBoxTargetVersionPrimary";
            this.textBoxTargetVersionPrimary.Size = new System.Drawing.Size(160, 20);
            this.textBoxTargetVersionPrimary.TabIndex = 1;
            this.toolTip1.SetToolTip(this.textBoxTargetVersionPrimary, "e.g. 3.0.0 RC1");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "phpBB Version:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonCancel);
            this.panel2.Controls.Add(this.buttonOk);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 174);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(504, 40);
            this.panel2.TabIndex = 1;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonCancel.Location = new System.Drawing.Point(346, 8);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "&Cancel";
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonOk.Location = new System.Drawing.Point(426, 8);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 4;
            this.buttonOk.Text = "&OK";
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(324, 7);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(74, 20);
            this.textBox1.TabIndex = 11;
            this.toolTip1.SetToolTip(this.textBox1, "e.g. 3.0.0 RC1");
            // 
            // listViewTargetVersionCases
            // 
            this.listViewTargetVersionCases.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listViewTargetVersionCases.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewTargetVersionCases.FullRowSelect = true;
            this.listViewTargetVersionCases.GridLines = true;
            this.listViewTargetVersionCases.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewTargetVersionCases.Location = new System.Drawing.Point(0, 64);
            this.listViewTargetVersionCases.MultiSelect = false;
            this.listViewTargetVersionCases.Name = "listViewTargetVersionCases";
            this.listViewTargetVersionCases.Size = new System.Drawing.Size(504, 110);
            this.listViewTargetVersionCases.TabIndex = 6;
            this.listViewTargetVersionCases.UseCompatibleStateImageBehavior = false;
            this.listViewTargetVersionCases.View = System.Windows.Forms.View.Details;
            this.listViewTargetVersionCases.SelectedIndexChanged += new System.EventHandler(this.listViewTargetVersionCases_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Part";
            this.columnHeader1.Width = 80;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Comparisson";
            this.columnHeader2.Width = 130;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Value";
            this.columnHeader3.Width = 190;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.comboBox3);
            this.panel3.Controls.Add(this.textBox1);
            this.panel3.Controls.Add(this.button1);
            this.panel3.Controls.Add(this.comboBox2);
            this.panel3.Controls.Add(this.comboBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 32);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(504, 32);
            this.panel3.TabIndex = 7;
            // 
            // comboBox3
            // 
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "Stable",
            "Alpha",
            "Beta",
            "Release Candidate"});
            this.comboBox3.Location = new System.Drawing.Point(200, 7);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(118, 21);
            this.comboBox3.TabIndex = 12;
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Location = new System.Drawing.Point(405, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Update Selected";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "Equal to",
            "Not Equal",
            "Greater than Equal",
            "Greater than",
            "Less than Equal",
            "Less than"});
            this.comboBox2.Location = new System.Drawing.Point(82, 7);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(112, 21);
            this.comboBox2.TabIndex = 9;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Major",
            "Minor",
            "Revision",
            "Release"});
            this.comboBox1.Location = new System.Drawing.Point(4, 7);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(72, 21);
            this.comboBox1.TabIndex = 9;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // TargetVersionDialogBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(504, 214);
            this.ControlBox = false;
            this.Controls.Add(this.listViewTargetVersionCases);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TargetVersionDialogBox";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "phpBB Version";
            this.Load += new System.EventHandler(this.TargetVersionDialogBox_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TargetVersionCases cases;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cases"></param>
        public void SetCases(TargetVersionCases cases)
        {
            // TODO: this
            /*if (entry.ChangeLog.Count == 0)
            {
                entry.ChangeLog.Add(new ModHistoryChangeLog(), "en");
            }
            //TODO: 
            //textBoxEntry.Text = entry.HistoryChanges.GetValue().Replace("\r","").Replace("\n", "\r\n");
            MODVersionMajor.Value = entry.Version.Major;
            MODVersionMinor.Value = entry.Version.Minor;
            MODVersionRevision.Value = entry.Version.Revision;
            MODVersionRelease.Text = entry.Version.Release.ToString();
            MODHistorydtp.Value = entry.Date;

            comboBoxLanguages.Items.Clear();
            comboBoxLanguages.Text = "";
            foreach (DictionaryEntry de in entry.ChangeLog)
            {
                comboBoxLanguages.Items.Add((string)de.Key);
                if (comboBoxLanguages.Items.Count == 1)
                {
                    comboBoxLanguages.SelectedIndex = 0;
                    listViewModChanges.Clear();
                    foreach (string change in ((ModHistoryChangeLog)de.Value))
                    {
                        listViewModChanges.Items.Add(change.Split('\n')[0]);
                    }
                }
            }*/
            this.textBoxTargetVersionPrimary.Text = cases.Primary;
            listViewTargetVersionCases.Items.Clear();
            foreach (TargetVersionCase tvc in cases)
            {
                string[] tempItem;
                if (tvc.Stage != VersionStage.Stable)
                {
                    tempItem = new string[] { tvc.Part.ToString(), tvc.Comparisson.ToString(), tvc.Stage.ToString() + " " + tvc.GetValue };
                }
                else
                {
                    tempItem = new string[] { tvc.Part.ToString(), tvc.Comparisson.ToString(), tvc.GetValue };
                }
                listViewTargetVersionCases.Items.Add(new ListViewItem(tempItem));
            }
            this.cases = cases;
        }

        /// <summary>
        /// 
        /// </summary>
        public TargetVersionCases Cases
        {
            get
            {
                // TODO:
                /*entry.Version.Major = (int)MODVersionMajor.Value;
                entry.Version.Minor = (int)MODVersionMinor.Value;
                entry.Version.Revision = (int)MODVersionRevision.Value;
                entry.Date = MODHistorydtp.Value;
                if (MODVersionRelease.Text.Length > 0)
                {
                    entry.Version.Release = MODVersionRelease.Text[0];
                }*/
                cases.Primary = textBoxTargetVersionPrimary.Text;
                return cases;
            }
            set
            {
                SetCases(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public delegate void TargetVersionDialogBoxSaveHandler(object sender, TargetVersionDialogBoxSaveEventArgs e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TargetVersionDialogBox_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add(TargetVersionPart.Major.ToString());
            comboBox1.Items.Add(TargetVersionPart.Minor.ToString());
            comboBox1.Items.Add(TargetVersionPart.Revision.ToString());
            comboBox1.Items.Add(TargetVersionPart.Release.ToString());

            comboBox2.Items.Clear();
            comboBox2.Items.Add(TargetVersionComparisson.EqualTo.ToString());
            comboBox2.Items.Add(TargetVersionComparisson.NotEqualTo.ToString());
            comboBox2.Items.Add(TargetVersionComparisson.LessThan.ToString());
            comboBox2.Items.Add(TargetVersionComparisson.GreaterThan.ToString());
            comboBox2.Items.Add(TargetVersionComparisson.LessThanEqual.ToString());
            comboBox2.Items.Add(TargetVersionComparisson.GreaterThanEqual.ToString());

            comboBox3.Items.Clear();
            comboBox3.Items.Add(VersionStage.Stable.ToString());
            comboBox3.Items.Add(VersionStage.Alpha.ToString());
            comboBox3.Items.Add(VersionStage.Beta.ToString());
            comboBox3.Items.Add(VersionStage.ReleaseCandidate.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            TargetVersionPart part = Phpbb.ModTeam.Tools.Mod.TargetVersionPartParse(comboBox1.Text);
            switch (part)
            {
                case TargetVersionPart.Release:
                    if (textBox1.Text.Length > 1 || !Regex.IsMatch(textBox1.Text, "^([A-Za-z]*)$"))
                    {
                        MessageBox.Show("Release character must be A-Z", "Invalid Release Version Part");
                        return;
                    }
                    break;
                default:
                    if (!Regex.IsMatch(textBox1.Text, "^([0-9]+)"))
                    {
                        MessageBox.Show(part.ToString() + " must be a number", "Invalid " + part.ToString() + " Version Part");
                        return;
                    }
                    break;
            }
            string textBoxValue = "";
            if (part == TargetVersionPart.Revision)
            {
                if (comboBox3.Text == VersionStage.Stable.ToString())
                {
                    textBoxValue = textBox1.Text;
                }
                else
                {
                    textBoxValue = comboBox3.Text + " " + textBox1.Text;
                }
            }
            else
            {
                textBoxValue = textBox1.Text;
            }
            string[] tempItem = { comboBox1.Text, comboBox2.Text, textBoxValue };
            listViewTargetVersionCases.Items.Add(new ListViewItem(tempItem));
        }

        private void listViewTargetVersionCases_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewTargetVersionCases.SelectedIndices.Count == 1)
            {
                int selectedIndex = listViewTargetVersionCases.SelectedIndices[0];
                comboBox1.SelectedIndex = (int)Phpbb.ModTeam.Tools.Mod.TargetVersionPartParse(listViewTargetVersionCases.Items[selectedIndex].SubItems[0].Text);
                comboBox2.SelectedIndex = (int)Phpbb.ModTeam.Tools.Mod.TargetVersionComparissonParse(listViewTargetVersionCases.Items[selectedIndex].SubItems[1].Text);

                if (Phpbb.ModTeam.Tools.Mod.TargetVersionPartParse(listViewTargetVersionCases.Items[selectedIndex].SubItems[0].Text) == TargetVersionPart.Revision)
                {
                    string[] parts = listViewTargetVersionCases.Items[selectedIndex].SubItems[2].Text.Split(new char[] { ' ' });
                    if (parts.Length == 1)
                    {
                        textBox1.Text = parts[0];
                    }
                    else if (parts.Length == 2)
                    {
                        textBox1.Text = parts[1];
                        if (parts[0] == VersionStage.Alpha.ToString())
                        {
                            comboBox3.SelectedIndex = 1;
                        }
                        else if (parts[0] == VersionStage.Beta.ToString())
                        {
                            comboBox3.SelectedIndex = 2;
                        }
                        else if (parts[0] == VersionStage.ReleaseCandidate.ToString())
                        {
                            comboBox3.SelectedIndex = 3;
                        }
                        else
                        {
                            comboBox3.SelectedIndex = 0;
                        }
                    }
                }
                else
                {
                    textBox1.Text = listViewTargetVersionCases.Items[selectedIndex].SubItems[2].Text;
                    comboBox3.SelectedIndex = 0;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listViewTargetVersionCases.SelectedIndices.Count == 1)
            {
                TargetVersionPart part = Phpbb.ModTeam.Tools.Mod.TargetVersionPartParse(comboBox1.Text);
                switch (part)
                {
                    case TargetVersionPart.Release:
                        if (textBox1.Text.Length > 1 || !Regex.IsMatch(textBox1.Text, "^([A-Za-z]*)$"))
                        {
                            MessageBox.Show("Release character must be A-Z", "Invalid Release Version Part");
                            return;
                        }
                        break;
                    default:
                        if (!Regex.IsMatch(textBox1.Text, "^([0-9]+)"))
                        {
                            MessageBox.Show(part.ToString() + " must be a number", "Invalid " + part.ToString() + " Version Part");
                            return;
                        }
                        break;
                }
                int selectedIndex = listViewTargetVersionCases.SelectedIndices[0];
                listViewTargetVersionCases.Items[selectedIndex].SubItems[0].Text = comboBox1.Text;
                listViewTargetVersionCases.Items[selectedIndex].SubItems[1].Text = comboBox2.Text;
                if (part == TargetVersionPart.Revision)
                {
                    if (comboBox3.Text == VersionStage.Stable.ToString())
                    {
                        listViewTargetVersionCases.Items[selectedIndex].SubItems[2].Text = textBox1.Text;
                    }
                    else
                    {
                        listViewTargetVersionCases.Items[selectedIndex].SubItems[2].Text = comboBox3.Text + " " + textBox1.Text;
                    }
                }
                else
                {
                    listViewTargetVersionCases.Items[selectedIndex].SubItems[2].Text = textBox1.Text;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listViewTargetVersionCases.SelectedIndices.Count == 1)
            {
                int selectedIndex = listViewTargetVersionCases.SelectedIndices[0];
                listViewTargetVersionCases.Items.RemoveAt(selectedIndex);
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            cases.Primary = textBoxTargetVersionPrimary.Text;
            cases.Clear();
            foreach (ListViewItem lvi in listViewTargetVersionCases.Items)
            {
                TargetVersionPart part = Phpbb.ModTeam.Tools.Mod.TargetVersionPartParse(lvi.SubItems[0].Text);
                switch (part)
                {
                    case TargetVersionPart.Release:
                        if (textBox1.Text.Length > 0)
                        {
                            cases.Add(new TargetVersionCase(
                                Phpbb.ModTeam.Tools.Mod.TargetVersionComparissonParse(lvi.SubItems[1].Text),
                                part,
                                lvi.SubItems[2].Text[0]));
                        }
                        else if (textBox1.Text.Length == 0)
                        {
                            TargetVersionCase tvc = new TargetVersionCase();
                            tvc.Comparisson = Phpbb.ModTeam.Tools.Mod.TargetVersionComparissonParse(lvi.SubItems[1].Text);
                            tvc.Part = part;
                            cases.Add(tvc);
                        }
                        break;
                    case TargetVersionPart.Revision:
                        TargetVersionCase tvcv = new TargetVersionCase();
                        string[] parts = lvi.SubItems[2].Text.Split(new char[] { ' ' });
                        tvcv.Comparisson = Phpbb.ModTeam.Tools.Mod.TargetVersionComparissonParse(lvi.SubItems[1].Text);
                        tvcv.Part = part;
                        if (parts.Length == 1)
                        {
                            tvcv.SetValueInt = int.Parse(parts[0]);
                        }
                        else if (parts.Length == 2)
                        {
                            tvcv.SetValueInt = int.Parse(parts[1]);
                            
                            if (parts[0] == VersionStage.Alpha.ToString())
                            {
                                tvcv.Stage = VersionStage.Alpha;
                            }
                            else if (parts[0] == VersionStage.Beta.ToString())
                            {
                                tvcv.Stage = VersionStage.Beta;
                            }
                            else if (parts[0] == VersionStage.ReleaseCandidate.ToString())
                            {
                                tvcv.Stage = VersionStage.ReleaseCandidate;
                            }
                            else
                            {
                                tvcv.Stage = VersionStage.Stable;
                            }
                        }
                        cases.Add(tvcv);
                        break;
                    default:
                        cases.Add(new TargetVersionCase(
                            Phpbb.ModTeam.Tools.Mod.TargetVersionComparissonParse(lvi.SubItems[1].Text),
                            part,
                            int.Parse(lvi.SubItems[2].Text)));
                        break;
                }
            }
            this.Hide();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == TargetVersionPart.Revision.ToString())
            {
                comboBox3.Enabled = true;
            }
            else
            {
                comboBox3.Enabled = false;
                comboBox3.Text = "";
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TargetVersionDialogBoxSaveEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public TargetVersionCases Cases;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cases"></param>
        public TargetVersionDialogBoxSaveEventArgs(TargetVersionCases cases)
        {
            this.Cases = cases;
        }
    }
}