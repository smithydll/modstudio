/***************************************************************************
 *                           OpenActionDialogBox.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: NewFileDialogBox.cs,v 1.1 2007-09-01 13:52:38 smithydll Exp $
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
    public partial class NewFileDialogBox : System.Windows.Forms.Form
    {
        /// <summary>
        /// 
        /// </summary>
        public NewFileDialogBox()
        {
            InitializeComponent();
        }

        private void NewFileDialogBox_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("", 200);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void NewFileDialogBoxSaveNewHandler(object sender, NewFileDialogBoxSaveNewEventArgs e);

        string lastFileName = "";
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                if (lastFileName != "")
                {
                    textBox1.Text = lastFileName;
                    lastFileName = "";
                }
                textBox1.Enabled = true;
                switch (listView1.SelectedItems[0].Text.ToLower())
                {
                    case "modx file":
                        textBox1.Text = Studio.CleanFileExtension(textBox1.Text) + ".xml";
                        break;
                    case "text template file":
                        textBox1.Text = Studio.CleanFileExtension(textBox1.Text) + ".mod";
                        break;
                    case "gpl v2 license":
                        lastFileName = (lastFileName == "") ? textBox1.Text : lastFileName;
                        textBox1.Text = "license.txt";
                        textBox1.Enabled = false;
                        break;
                    case "php file":
                        textBox1.Text = Studio.CleanFileExtension(textBox1.Text) + ".php";
                        break;
                    case "html file":
                        textBox1.Text = Studio.CleanFileExtension(textBox1.Text) + ".html";
                        break;
                    case "template file":
                        // TODO: change file extension based on phpBB version
                        textBox1.Text = Studio.CleanFileExtension(textBox1.Text) + ".tpl";
                        break;
                    case "css file":
                        textBox1.Text = Studio.CleanFileExtension(textBox1.Text) + ".css";
                        break;
                    case "text file":
                        textBox1.Text = Studio.CleanFileExtension(textBox1.Text) + ".txt";
                        break;
                    case "xslt file":
                        textBox1.Text = Studio.CleanFileExtension(textBox1.Text) + ".xsl";
                        break;
                    case "prosilver xslt file":
                        lastFileName = (lastFileName == "") ? textBox1.Text : lastFileName;
                        textBox1.Text = "modx.prosilver.en.xsl";
                        textBox1.Enabled = false;
                        break;
                    case "subsilver xslt file":
                        lastFileName = (lastFileName == "") ? textBox1.Text : lastFileName;
                        textBox1.Text = "modx.subsilver.en.xsl";
                        textBox1.Enabled = false;
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FileName
        {
            get
            {
                return textBox1.Text;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ModStudioFileTypes FileType
        {
            get
            {
                if (listView1.SelectedItems.Count == 1)
                {
                    switch (listView1.SelectedItems[0].Text.ToLower())
                    {
                        case "modx file":
                            return ModStudioFileTypes.ModxMod;
                        case "gpl v2 license":
                            return ModStudioFileTypes.Gpl2License;
                        case "php file":
                            return ModStudioFileTypes.PhpFile;
                        case "html file":
                            return ModStudioFileTypes.HtmlFile;
                        case "template file":
                            return ModStudioFileTypes.TemplateFile;
                        case "css file":
                            return ModStudioFileTypes.CascadingStyleSheet;
                        case "text file":
                            return ModStudioFileTypes.TextFile;
                        case "xslt file":
                            return ModStudioFileTypes.XsltFile;
                        case "prosilver xslt file":
                            return ModStudioFileTypes.ProsilverXslt;
                        case "subsilver xslt file":
                            return ModStudioFileTypes.SubsilverXslt;
                    }
                }
                return ModStudioFileTypes.None;
            }
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class NewFileDialogBoxSaveNewEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public string FileName;

        /// <summary>
        /// 
        /// </summary>
        public ModStudioFileTypes FileType;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileType"></param>
        public NewFileDialogBoxSaveNewEventArgs(string fileName, ModStudioFileTypes fileType)
        {
            this.FileName = fileName;
            this.FileType = fileType;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ModStudioFileTypes
    {
        /// <summary>
        /// 
        /// </summary>
        None,
        /// <summary>
        /// 
        /// </summary>
        ModxMod,
        /// <summary>
        /// 
        /// </summary>
        TextMod,
        /// <summary>
        /// 
        /// </summary>
        LanguageFile,
        /// <summary>
        /// 
        /// </summary>
        PhpFile,
        /// <summary>
        /// 
        /// </summary>
        TemplateFile,
        /// <summary>
        /// 
        /// </summary>
        HtmlFile,
        /// <summary>
        /// 
        /// </summary>
        TextFile,
        /// <summary>
        /// 
        /// </summary>
        Gpl2License,
        /// <summary>
        /// 
        /// </summary>
        CascadingStyleSheet,
        /// <summary>
        /// 
        /// </summary>
        ImageSet,
        /// <summary>
        /// 
        /// </summary>
        XsltFile,
        /// <summary>
        /// 
        /// </summary>
        SqlFile,
        /// <summary>
        /// 
        /// </summary>
        ProsilverXslt,
        /// <summary>
        /// 
        /// </summary>
        SubsilverXslt,
        /// <summary>
        /// 
        /// </summary>
        HtaccessFile,
        /// <summary>
        /// 
        /// </summary>
        TemplateConfigFile,
        /// <summary>
        /// 
        /// </summary>
        JavascriptFile,
        /// <summary>
        /// 
        /// </summary>
        SmileyFile
    }
}
