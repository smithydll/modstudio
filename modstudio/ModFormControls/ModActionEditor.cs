using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace ModFormControls
{
	/// <summary>
	/// Summary description for ModActionEditor.
	/// </summary>
	public class ModActionEditor : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel4;
		private ICSharpCode.TextEditor.TextEditorControl textEditorControlActionBody;
		private ICSharpCode.TextEditor.TextEditorControl textEditorControlComment;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ModActionEditor()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ModActionEditor));
			this.panel2 = new System.Windows.Forms.Panel();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel3 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel4 = new System.Windows.Forms.Panel();
			this.textEditorControlActionBody = new ICSharpCode.TextEditor.TextEditorControl();
			this.textEditorControlComment = new ICSharpCode.TextEditor.TextEditorControl();
			this.panel5 = new System.Windows.Forms.Panel();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel5.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.AliceBlue;
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel2.Controls.Add(this.textEditorControlActionBody);
			this.panel2.Controls.Add(this.splitter1);
			this.panel2.Controls.Add(this.panel3);
			this.panel2.Controls.Add(this.comboBox1);
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(472, 312);
			this.panel2.TabIndex = 6;
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter1.Location = new System.Drawing.Point(0, 207);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(470, 3);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.textEditorControlComment);
			this.panel3.Controls.Add(this.panel5);
			this.panel3.Controls.Add(this.label1);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 210);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(470, 100);
			this.panel3.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.SlateGray;
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(470, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Comment";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboBox1
			// 
			this.comboBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.comboBox1.Items.AddRange(new object[] {
														   "FIND",
														   "AFTER, ADD",
														   "BEFORE, ADD",
														   "REPLACE WITH",
														   "IN-LINE FIND",
														   "IN-LINE AFTER, ADD",
														   "IN-LINE BEFORE, ADD",
														   "IN-LINE REPLACE WITH"});
			this.comboBox1.Location = new System.Drawing.Point(0, 0);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(470, 26);
			this.comboBox1.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.SlateGray;
			this.panel1.Location = new System.Drawing.Point(4, 4);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(472, 312);
			this.panel1.TabIndex = 5;
			// 
			// panel4
			// 
			this.panel4.BackColor = System.Drawing.Color.Transparent;
			this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel4.Location = new System.Drawing.Point(0, 0);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(480, 320);
			this.panel4.TabIndex = 7;
			// 
			// textEditorControlActionBody
			// 
			this.textEditorControlActionBody.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textEditorControlActionBody.Encoding = ((System.Text.Encoding)(resources.GetObject("textEditorControlActionBody.Encoding")));
			this.textEditorControlActionBody.IsIconBarVisible = false;
			this.textEditorControlActionBody.Location = new System.Drawing.Point(0, 26);
			this.textEditorControlActionBody.Name = "textEditorControlActionBody";
			this.textEditorControlActionBody.ShowEOLMarkers = true;
			this.textEditorControlActionBody.ShowSpaces = true;
			this.textEditorControlActionBody.ShowTabs = true;
			this.textEditorControlActionBody.ShowVRuler = true;
			this.textEditorControlActionBody.Size = new System.Drawing.Size(470, 181);
			this.textEditorControlActionBody.TabIndex = 3;
			// 
			// textEditorControlComment
			// 
			this.textEditorControlComment.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textEditorControlComment.Encoding = ((System.Text.Encoding)(resources.GetObject("textEditorControlComment.Encoding")));
			this.textEditorControlComment.IsIconBarVisible = false;
			this.textEditorControlComment.Location = new System.Drawing.Point(0, 20);
			this.textEditorControlComment.Name = "textEditorControlComment";
			this.textEditorControlComment.ShowEOLMarkers = true;
			this.textEditorControlComment.ShowSpaces = true;
			this.textEditorControlComment.ShowTabs = true;
			this.textEditorControlComment.ShowVRuler = true;
			this.textEditorControlComment.Size = new System.Drawing.Size(382, 80);
			this.textEditorControlComment.TabIndex = 1;
			// 
			// panel5
			// 
			this.panel5.Controls.Add(this.button1);
			this.panel5.Controls.Add(this.button2);
			this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel5.Location = new System.Drawing.Point(382, 20);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(88, 80);
			this.panel5.TabIndex = 2;
			// 
			// button1
			// 
			this.button1.BackColor = System.Drawing.SystemColors.Control;
			this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Location = new System.Drawing.Point(0, 0);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(88, 40);
			this.button1.TabIndex = 0;
			this.button1.Text = "OK";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.BackColor = System.Drawing.SystemColors.Control;
			this.button2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button2.Location = new System.Drawing.Point(0, 40);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(88, 40);
			this.button2.TabIndex = 1;
			this.button2.Text = "Cancel";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// ModActionEditor
			// 
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel4);
			this.Name = "ModActionEditor";
			this.Size = new System.Drawing.Size(480, 320);
			this.Resize += new System.EventHandler(this.panel1_Resize);
			this.Leave += new System.EventHandler(this.ModActionEditor_Leave);
			this.panel2.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel5.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void panel1_Resize(object sender, System.EventArgs e)
		{
			panel1.Height = this.Height - 8;
			panel2.Height = this.Height - 8;
			panel1.Width = this.Width - 8;
			panel2.Width = this.Width - 8;
		}

		private void ModActionEditor_Leave(object sender, System.EventArgs e)
		{
			this.Dispose();
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			this.Dispose();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			ModActionEditor_Leave(sender, e);
		}
	}
}
