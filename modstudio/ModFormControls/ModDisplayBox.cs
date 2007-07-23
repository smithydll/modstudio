/***************************************************************************
 *                              ModDisplayBox.cs
 *                            -------------------
 *   begin                : Wednesday, Jun 29, 2005
 *   copyright            : (C) 2005 smithy_dll
 *   email                : smithydll@users.sourceforge.net
 *
 *   $Id: ModDisplayBox.cs,v 1.15 2007-07-23 09:03:41 smithydll Exp $
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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Phpbb.ModTeam.Tools;
using Phpbb.ModTeam.Tools.DataStructures;

namespace ModFormControls
{
	/// <summary>
	/// Summary description for ModDisplayBox.
	/// </summary>
	public class ModDisplayBox : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ModDisplayBox()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			this.SetStyle(ControlStyles.Selectable, true);
			this.UpdateStyles();
			this.CreateControl();
			IntPtr handle = this.Handle;
			Console.WriteLine(this.CanFocus + ":f");
			Console.WriteLine(this.CanSelect);
			Console.WriteLine(this.Enabled);
			Console.WriteLine(this.Visible);
			Console.WriteLine(this.Handle);
			//this.CanFocus = true;
			//this.CanSelect = true;

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
			// 
			// ModDisplayBox
			// 
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.AliceBlue;
			this.Name = "ModDisplayBox";
			this.Size = new System.Drawing.Size(480, 320);
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ModDisplayBox_KeyPress);
			this.Load += new System.EventHandler(this.ModDisplayBox_Load);
			this.Enter += new System.EventHandler(this.ModDisplayBox_Enter);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ModDisplayBox_KeyDown);

		}
		#endregion

		private ModActions Actions;
		private ModActionItemCollection ActionItems;
		private int selectedIndex = 0;
		const int scrollBarWidth = 17;
		const int itemHeight = 60;
		const int itemRealHeight = itemHeight + 5;

		/// <summary>
		/// 
		/// </summary>
		public ModActions ModActions
		{
			set
			{
				Actions = value;

				// Lets dispose of any existing action objects
				if (ActionItems != null)
				{
					foreach (ModActionItem mai in ActionItems)
					{
						mai.Dispose();
					}
					ActionItems = null;
				}
				// if we assigned a non-null object
				if (Actions != null)
				{
					// create a new array of the right length
					ActionItems = new ModActionItemCollection();

					//this.ModDisplayPanel.SuspendLayout();
					this.SuspendLayout();
					//ModDisplayPanel.Height = ActionItems.Count * itemRealHeight;
					for (int i = 0; i < Actions.Count; i++)
					{
						ModActionItem tempActionItem = new ModActionItem();
						tempActionItem.TabIndex = i + 2;
						tempActionItem.Index = i;
						tempActionItem.ItemClick += new ModActionItem.ActionItemClickHandler(this.ActionItems_SelectedIndexChanged);
						tempActionItem.ItemDoubleClick += new ModActionItem.ActionItemClickHandler(this.ActionItmes_ItemDoubleClick);
						tempActionItem.ItemSwitch += new ModFormControls.ModActionItem.ActionItemSwitchHandler(this.ActionItmes_ItemSwitch);
						tempActionItem.ItemEnter += new ModFormControls.ModActionItem.ActionItemEnterHandler(this.ActionItems_ItemEnter);
						
						ActionItems.Add(tempActionItem);
						this.Controls.Add(tempActionItem);
					}
					//this.ModDisplayPanel.Height = Actions.Count * 100;
					UpdateLayout();
					//this.ModDisplayPanel.ResumeLayout(false);
					this.ResumeLayout(false);
				}
			}
		}

		public event ModActionItem.ActionItemClickHandler ItemDoubleClick;
		public event EventHandler SelectedIndexChanged;

		private void ActionItems_SelectedIndexChanged(object sender, ActionItemClickEventArgs e)
		{
			SelectedIndex = e.Index;
			//UpdateColours();
			this.SelectedIndexChanged(this, new EventArgs());
		}

		public void ActionItmes_ItemDoubleClick(object sender, ActionItemClickEventArgs e)
		{
			this.ItemDoubleClick(this, e);
		}

		public void UpdateColours()
		{
			//this.ModDisplayPanel.SuspendLayout();
			this.SuspendLayout();
			if (Actions != null)
			{
				for (int i = 0; i < ActionItems.Count; i++)
				{
					ModActionItem mai = ActionItems[i];
					mai.SuspendLayout();

					mai.BackColor = GetColour(i);

					mai.ResumeLayout();
					ActionItems[i].Refresh();
				}
			}
			//this.ModDisplayPanel.ResumeLayout();
			this.ResumeLayout();
		}

		public void UpdateSize()
		{
			//this.ModDisplayPanel.SuspendLayout();
			this.SuspendLayout();
			if (Actions != null)
			{
				for (int i = 0; i < ActionItems.Count; i++)
				{
					ActionItems[i].SuspendLayout();
					ActionItems[i].Height = itemHeight;
					ActionItems[i].Visible = true;
					//ActionItems[i].Location = new Point(0,0);

					switch (Actions[i].Type)
					{
						case "SQL":
						case "COPY":
						case "SAVE/CLOSE ALL FILES":
						case "OPEN":
						case "DIY INSTRUCTIONS":
							ActionItems[i].Width = this.Width - 20 - 16;
							break;
						case "FIND":
							ActionItems[i].Width = this.Width - 40 - 16;
							break;
						case "IN-LINE FIND":
						case "REPLACE WITH":
						case "AFTER, ADD":
						case "BEFORE, ADD":
						case "INCREMENT":
							ActionItems[i].Width = this.Width - 60 - 16;
							break;
						case "IN-LINE REPLACE WITH":
						case "IN-LINE AFTER, ADD":
						case "IN-LINE BEFORE, ADD":
						case "IN-LINE INCREMENT":
							ActionItems[i].Width = this.Width - 80 - 16;
							break;

					}
					ActionItems[i].Refresh();
				}
			}
			//this.ModDisplayPanel.ResumeLayout();
			this.ResumeLayout();
		}

		public void UpdateText()
		{
			if (Actions != null)
			{

				//ModDisplayPanel.ScrollControlIntoView(ActionItems[0]);
				for (int i = 0; i < ActionItems.Count; i++)
				{
					ActionItems[i].SuspendLayout();
					ActionItems[i].ActionTitle = Actions[i].Type;
					ActionItems[i].ActionBody = GetFirstLines(Actions[i].Body);
					ActionItems[i].ResumeLayout();
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void UpdateLayout()
		{
			//this.ModDisplayPanel.SuspendLayout();
			this.SuspendLayout();
			int offset = 0;
			Point scrollPosn = new Point(0,0);
			if (Actions != null)
			{

				if (ActionItems.Count > 0)
				{
					scrollPosn = this.AutoScrollPosition;
					this.AutoScrollPosition = new Point(0,0);
				}
				for (int i = 0; i < ActionItems.Count; i++)
				{
					ModActionItem mai = ActionItems[i];
					mai.SuspendLayout();

					mai.Height = itemHeight;
					mai.Visible = true;
					int indent = GetIndent(i);
					mai.Width = this.Width - 20 - indent - scrollBarWidth;
					mai.Location = new Point(10 + indent, i * itemRealHeight + 10 + offset);
					mai.BackColor = GetColour(i);

					ActionItems[i].ActionTitle = Actions[i].Type;
					ActionItems[i].ActionBody = GetFirstLines(Actions[i].Body);

					/*if (selectedIndex == i)
					{
						ModDisplayPanel.ScrollControlIntoView(ActionItems[i]);
					}*/

					mai.ResumeLayout();

					ActionItems[i].Refresh();
				}

				this.AutoScrollPosition = scrollPosn;
				this.ScrollControlIntoView(ActionItems[selectedIndex]);
			}
			//this.ModDisplayPanel.ResumeLayout();
			this.ResumeLayout();
		}

		private void ModDisplayBox_Enter(object sender, System.EventArgs e)
		{
			this.Focus();
		}

		public int SelectedIndex
		{
			set
			{
				if (ActionItems != null)
				{
					if (value < ActionItems.Count)
					{
						selectedIndex = value;
						UpdateColours();
						this.ScrollControlIntoView(ActionItems[value]);
						this.SelectedIndexChanged(this, new EventArgs());
						
						if (selectedIndex >= 0)
						{
							//ActionItems[selectedIndex].Index = selectedIndex;
							ActionItems[selectedIndex].Focus();
						}
					}
					else
					{
						// error
					}
				}
			}
			get
			{
				return selectedIndex;
			}
		}

		/// <summary>
		/// Return the indent
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		private int GetIndent(int index)
		{
			string type;
			try
			{
				type = Actions[index].Type;
			}
			catch
			{
				return 0;
			}
			switch (type)
			{
				case "SQL":
				case "COPY":
				case "SAVE/CLOSE ALL FILES":
				case "DIY INSTRUCTIONS":
				case "OPEN":
					return 0;
				case "FIND":
					return 20;
				case "IN-LINE FIND":
				case "REPLACE WITH":
				case "AFTER, ADD":
				case "BEFORE, ADD":
				case "INCREMENT":
					return 40;
				case "IN-LINE REPLACE WITH":
				case "IN-LINE AFTER, ADD":
				case "IN-LINE BEFORE, ADD":
				case "IN-LINE INCREMENT":
					return 60;
			}
			return 0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		private Color GetColour(int index)
		{
			Color error = Color.LightPink;

			if (selectedIndex == index)
			{
				return Color.GhostWhite;
			}

			string type = "";
			string next = "";

			try
			{
				type = Actions[index].Type;
				if (index + 1 < ActionItems.Count)
				{
					next = Actions[index + 1].Type;
				}
			}
			catch
			{
				return error;
			}

			switch (type)
			{
				case "SQL":
				case "COPY":
				case "SAVE/CLOSE ALL FILES":
				case "DIY INSTRUCTIONS":
					return Color.LightCyan;
				case "OPEN":
					if (next != "FIND") return error;
					return Color.LightYellow;
				case "FIND":
					if (next != "AFTER, ADD" && next != "BEFORE, ADD" && next != "REPLACE WITH" && next != "IN-LINE FIND") return error;
					return Color.LightGreen;
				case "IN-LINE FIND":
					return Color.PaleGreen;
				case "REPLACE WITH":
				case "AFTER, ADD":
				case "BEFORE, ADD":
				case "INCREMENT":
					return Color.LightGoldenrodYellow;
				case "IN-LINE REPLACE WITH":
				case "IN-LINE AFTER, ADD":
				case "IN-LINE BEFORE, ADD":
				case "IN-LINE INCREMENT":
					return Color.LemonChiffon;
			}
			return error;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public void Insert(int index, ModAction value)
		{
			Actions.Insert(index, value);
			// TODO: insert new ModActionItems
			ModActionItem tempActionItem = new ModActionItem();

			tempActionItem.ItemClick += new ModActionItem.ActionItemClickHandler(this.ActionItems_SelectedIndexChanged);
			tempActionItem.ItemDoubleClick += new ModActionItem.ActionItemClickHandler(this.ActionItmes_ItemDoubleClick);
			tempActionItem.ItemSwitch += new ModFormControls.ModActionItem.ActionItemSwitchHandler(this.ActionItmes_ItemSwitch);
			tempActionItem.ItemEnter += new ModFormControls.ModActionItem.ActionItemEnterHandler(this.ActionItems_ItemEnter);

			ActionItems.Insert(index, tempActionItem);
			this.Controls.Add(tempActionItem);

			for (int i = index; i < ActionItems.Count; i++)
			{
				ActionItems[i].TabIndex = i + 2;
				ActionItems[i].Index = i;
			}

			UpdateLayout();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			Actions.RemoveAt(index);
			this.Controls.Remove(ActionItems[index]);
			ActionItems[index].Dispose();
			ActionItems.RemoveAt(index);

			for (int i = index; i < ActionItems.Count; i++)
			{
				ActionItems[i].TabIndex = i + 2;
				ActionItems[i].Index = i;
			}

			UpdateLayout();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetFirstLines(string value)
		{
			if (value.Split('\n').Length <= 2)
			{
				return value;
			}
			string retValue = "";
			string[] splitValue = value.Split('\n');
			for (int i = 0; i < 2; i++)
			{
				retValue += splitValue[i] + "\r\n";
			}
			return retValue;
		}

		private void ModDisplayBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
		}

		private void ModDisplayBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			Console.WriteLine(e.Modifiers.ToString() + " + " + e.KeyCode.ToString());
			if (e.Modifiers == Keys.Control && e.KeyCode == Keys.C)
			{
				Clipboard.SetDataObject(Actions[selectedIndex].Body, true);
			}
			Console.WriteLine(this.SelectedIndex);
		}

		private void ModDisplayBox_Load(object sender, System.EventArgs e)
		{
			this.SetStyle(ControlStyles.Selectable, true);
			this.UpdateStyles();
			InitializeComponent();
			this.CreateControl();
			IntPtr handle = this.Handle;
			Console.WriteLine("-");
			Console.WriteLine(this.CanFocus + ":f");
			Console.WriteLine(this.CanSelect);
			Console.WriteLine(this.Enabled);
			Console.WriteLine(this.Visible);
			Console.WriteLine(this.Handle);
		}

		int ipp = 0;
		private void ActionItmes_ItemSwitch(object snder, ActionItemSwitchEventArgs e)
		{
			ipp++;
			Console.WriteLine("boo:" + ipp);
			switch (e.SwitchTo)
			{
				case SwitchTo.Next:
					if (selectedIndex < Actions.Count)
					{
						this.SelectedIndex += 1;
					}
					break;
				case SwitchTo.Previous:
					if (selectedIndex > 0)
					{
						this.SelectedIndex -= 1;
					}
					break;
			}
		}

		protected override bool IsInputKey(Keys keyData)
		{
			if (keyData == Keys.Up || keyData == Keys.Down)
			{
				//return true;
			}
			return base.IsInputKey (keyData);
		}

		private void ActionItems_ItemEnter(object sender, ActionItemEnterEventArgs e)
		{
			this.ItemDoubleClick(this, new ActionItemClickEventArgs(e.Index));
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class ModActionItemCollection : System.Collections.IEnumerable, System.Collections.ICollection
	{
		private ArrayList ModActionItems;

		/// <summary>
		/// 
		/// </summary>
		public ModActionItemCollection()
		{
			ModActionItems = new ArrayList();
		}

		/// <summary>
		/// 
		/// </summary>
		public ModActionItem this[int index]
		{
			get
			{
				return (ModActionItem)ModActionItems[index];
			}
			set
			{
				ModActionItems[index] = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		public void Add(ModActionItem value)
		{
			ModActionItems.Add(value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public void Insert(int index, ModActionItem value)
		{
			ModActionItems.Insert(index, value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			ModActionItems.RemoveAt(index);
		}

		#region IEnumerable Members

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return ModActionItems.GetEnumerator();
		}

		#endregion

		#region ICollection Members

		/// <summary>
		/// 
		/// </summary>
		public bool IsSynchronized
		{
			get
			{
				return ModActionItems.IsSynchronized;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int Count
		{
			get
			{
				return ModActionItems.Count;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		public void CopyTo(Array array, int index)
		{
			ModActionItems.CopyTo(array, index);
		}

		/// <summary>
		/// 
		/// </summary>
		public object SyncRoot
		{
			get
			{
				return ModActionItems.SyncRoot;
			}
		}

		#endregion
	}
}
