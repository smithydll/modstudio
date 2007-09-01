/***************************************************************************
 *                             LanguageEditor.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: LanguageEditor.cs,v 1.2 2007-09-01 13:52:37 smithydll Exp $
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
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace ModStudio
{
	/// <summary>
	/// Summary description for LanguageEditor.
	/// </summary>
	public class LanguageEditor : System.Windows.Forms.Form
	{
		private System.Windows.Forms.DataGrid dataGrid1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// 
		/// </summary>
		public LanguageEditor()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(LanguageEditor));
			this.dataGrid1 = new System.Windows.Forms.DataGrid();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
			this.SuspendLayout();
			// 
			// dataGrid1
			// 
			this.dataGrid1.DataMember = "";
			this.dataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGrid1.FlatMode = true;
			this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGrid1.Location = new System.Drawing.Point(0, 0);
			this.dataGrid1.Name = "dataGrid1";
			this.dataGrid1.PreferredColumnWidth = 300;
			this.dataGrid1.Size = new System.Drawing.Size(792, 566);
			this.dataGrid1.TabIndex = 2;
			this.dataGrid1.CurrentCellChanged += new System.EventHandler(this.dataGrid1_CurrentCellChanged);
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.FileName = "untitled";
			this.saveFileDialog1.Filter = "phpBB2 MOD Files (*.mod,*.txt)|*.mod;*.txt|MODX Format (*.xml)|*.xml";
			this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
			// 
			// LanguageEditor
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(792, 566);
			this.Controls.Add(this.dataGrid1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "LanguageEditor";
			this.Text = "LanguageEditor";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Load += new System.EventHandler(this.LanguageEditor_Load);
			this.TextChanged += new System.EventHandler(this.LanguageEditor_TextChanged);
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		// Variables
		System.Data.DataTable data = new DataTable("data");

		private void LanguageEditor_Load(object sender, System.EventArgs e)
		{
			SetUnmodified();
		}

		/// <summary>
		/// 
		/// </summary>
		public void SaveFile()
		{
			if (this.Text.EndsWith("*")) // the file has been modified
			{
				if (this.Text.StartsWith("untitled")) // hasn't been saved before
				{
					SaveFileAs();
				}
				else
				{
					save(this.Text.Substring(0, this.Text.Length - 1));
					SetUnmodified();
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void SaveFileAs()
		{
			saveFileDialog1.ShowDialog(this);
		}

		private void save(string fileName)
		{
			string[] LanguageFile = OpenTextFile(fileName).Replace("\r\n", "\n").Split('\n');
			string newvars = "";
			bool existingvar = false;
			foreach (DataRow r in data.Rows)
			{
				existingvar = false;
				string[] var = r["variable"].ToString().Split('.');
				string needle = "$lang";
				foreach (string s in var)
				{
					needle += string.Format("['{0}']", s);
				}
				for (int i = 0; i < LanguageFile.Length; i++)
				{
					if (LanguageFile[i].Replace(needle, "") != LanguageFile[i])
					{
						LanguageFile[i] = Regex.Replace(LanguageFile[i], "\\=([ ]+?)\\'(.*?)\\'\\;", string.Format("=$1'{0}';", r["value"].ToString().Replace("'", "\'")));
						existingvar = true;
					}
				}
				if (!existingvar)
				{
					newvars += string.Format("{0} = '{1}';\n", needle, r["value"].ToString().Replace("'", "\\'"));
				}
			}
			string savefile = "";
			for (int i = 0; i < LanguageFile.Length; i++)
			{
				if (i < LanguageFile.Length - 1)
				{
					if (LanguageFile[(i + 1)] == "// That's all, Folks!" && newvars != "")
					{
						savefile += "\n" + newvars + "\n";
					}
				}
				if (i < LanguageFile.Length - 1)
				{
					savefile += LanguageFile[i] + "\n";
				}
				else
				{
					savefile += LanguageFile[i];
				}
			}
			SaveTextFile(savefile, fileName);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		public void Open(string fileName)
		{
			data = new DataTable("data");
			data.Columns.Add("line", System.Type.GetType("System.Int32"));
			data.Columns.Add("variable", System.Type.GetType("System.String"));
			data.Columns.Add("value", System.Type.GetType("System.String"));
			dataGrid1.DataSource = data;
			DataGridTableStyle thisTableStyle = new DataGridTableStyle();
			thisTableStyle.MappingName = "data";
			thisTableStyle.AllowSorting = false;
			dataGrid1.TableStyles.Add(thisTableStyle);
			thisTableStyle.GridColumnStyles[0].ReadOnly = true;
			thisTableStyle.GridColumnStyles[0].Width = 48;
			thisTableStyle.GridColumnStyles[1].Width = 120;
			thisTableStyle.GridColumnStyles[2].Width = 320;
			string[] LanguageFile = OpenTextFile(fileName).Replace("\r\n", "\n").Split('\n');
			Int32 lineNumber = 0;
			foreach (string Line in LanguageFile)
			{
				string Line2;
				Line2 = Regex.Replace(Line, "^(.*?)\\;([ ]*)\\/\\/(.*?)$", "$1;");
				string[] linedata = Regex.Split(Line2, "(\\'\\]([ ]*)\\=([ ]*))\\'");//Line.Split('=');
				char[] TrimChars = {' ', '\t', '\'', ';'};
				char[] TrimChars2 = {'\t', '\'', ';'};
				if (Line.StartsWith("$lang"))
				{
					try
					{
						linedata[0] = linedata[0].Replace("$lang['","").Replace("']['", ".").Replace("']", "").Trim(TrimChars);
						linedata[1] = linedata[4].Trim(TrimChars2).Replace("\'", "'");
						string[] cols = {lineNumber.ToString(), linedata[0], linedata[1]};
						data.Rows.Add(cols);
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.ToString());
						Console.WriteLine(Line);
					}
				}
				lineNumber++;
			}
		}

		private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			save(saveFileDialog1.FileName);
			this.Text = saveFileDialog1.FileName;
			SetUnmodified();
		}

		/// <summary>
		/// 
		/// </summary>
		public void SetModified()
		{
			if (!this.Text.EndsWith("*"))
			{
				this.Text += "*";
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void SetUnmodified()
		{
			if (this.Text.EndsWith("*"))
			{
				this.Text = this.Text.Substring(0, this.Text.Length - 1);
			}
		}

		private void LanguageEditor_TextChanged(object sender, System.EventArgs e)
		{
			//SetModified();
		}

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

		private void dataGrid1_CurrentCellChanged(object sender, System.EventArgs e)
		{
			SetModified();
		}
	}
}
