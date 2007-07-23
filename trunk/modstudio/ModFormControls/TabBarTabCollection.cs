using System;
using System.Collections;

namespace ModFormControls
{
	/// <summary>
	/// Summary description for TabBarTabCollection.
	/// </summary>
	public class TabBarTabCollection : System.Collections.ICollection
	{
		ArrayList tabs = new ArrayList();

		public TabBarTabCollection()
		{
			
		}

		#region ICollection Members

		public bool IsSynchronized
		{
			get
			{
				return tabs.IsSynchronized;
			}
		}

		public int Count
		{
			get
			{
				return tabs.Count;
			}
		}

		public void CopyTo(Array array, int index)
		{
			tabs.CopyTo(array, index);
		}

		public object SyncRoot
		{
			get
			{
				return tabs.SyncRoot;
			}
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return tabs.GetEnumerator();
		}

		#endregion

		#region IList Members

		public bool IsReadOnly
		{
			get
			{
				return tabs.IsReadOnly;
			}
		}

		public TabBarTab this[int index]
		{
			get
			{
				return (TabBarTab)tabs[index];
			}
			set
			{
				tabs[index] = value;
				this.Modified(this, new EventArgs());
			}
		}

		public void RemoveAt(int index)
		{
			tabs.RemoveAt(index);
			this.Modified(this, new EventArgs());
		}

		public void Insert(int index, TabBarTab value)
		{
			tabs.Insert(index, value);
			//this.Modified(this, new EventArgs());
		}

		public void Remove(TabBarTab value)
		{
			tabs.Remove(value);
			//this.Modified(this, new EventArgs());
		}

		public bool Contains(TabBarTab value)
		{
			return tabs.Contains(value);
		}

		public void Clear()
		{
			foreach (TabBarTab tab in tabs)
			{
				tab.Dispose();
			}
			tabs.Clear();
			//tabs = null;
			//tabs = new ArrayList();
			//this.Modified(this, new EventArgs());
		}

		public int IndexOf(TabBarTab value)
		{
			return tabs.IndexOf(value);
		}

		public int Add(TabBarTab value)
		{
			int temp = tabs.Add(value);
			//this.Modified(this, new EventArgs());
			return temp;
		}

		public bool IsFixedSize
		{
			get
			{
				return tabs.IsFixedSize;
			}
		}

		#endregion

		public event EventHandler Modified;
	}
}
