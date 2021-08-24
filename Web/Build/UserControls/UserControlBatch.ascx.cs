using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlBatch : System.Web.UI.UserControl
{
	public event EventHandler TextChangedEvent;
	private bool _appenddatabounditems;
	private int _selectedValue = -1;
	string courseid;
	string batchstatus;
	private bool isoutside;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			bind();
		}
	}

	public DataSet BatchList
	{
		set
		{
			ddlBatch.Items.Clear();
			ddlBatch.DataSource = value;
			ddlBatch.DataBind();
		}
	}
	public bool Enabled
	{
		set
		{
			ddlBatch.Enabled = value;
		}
	}
	public string CssClass
	{
		set
		{
			ddlBatch.CssClass = value;
		}
	}
	public string CourseId
	{
		get
		{
			return courseid;
		}
		set
		{
			courseid =value;
		}
	}

	public string BatchStatus
	{
		get
		{
			return batchstatus;
		}
		set
		{
			batchstatus = value;
		}
	}
	public string AppendDataBoundItems
	{
		set
		{
			if (value.ToUpper().Equals("TRUE"))
				_appenddatabounditems = true;
			else
				_appenddatabounditems = false;
		}
	}
	public string IsOutside
	{
		set
		{
			if (value.ToUpper().Equals("TRUE"))
				isoutside = true;
			else
				isoutside = false;
		}
	}
	public string AutoPostBack
	{
		set
		{
			if (value.ToUpper().Equals("TRUE"))
				ddlBatch.AutoPostBack = true;
		}
	}
	public void OnTextChangedEvent(EventArgs e)
	{
		if (TextChangedEvent != null)
			TextChangedEvent(this, e);
	}
	public void ddlBatch_TextChanged(object sender, EventArgs e)
	{
		OnTextChangedEvent(e);
	}

	public string SelectedBatch
	{
		get
		{
			return ddlBatch.SelectedValue;
		}
		set
		{
			if (value == string.Empty)
			{
				ddlBatch.SelectedIndex = -1;
                ddlBatch.ClearSelection();
                ddlBatch.Text = string.Empty;
				return;
			}
			_selectedValue = Int32.Parse(value);
			ddlBatch.SelectedIndex = -1;
			foreach (RadComboBoxItem item in ddlBatch.Items)
			{
				if (item.Value == _selectedValue.ToString())
				{
					item.Selected = true;
					break;
				}
			}
		}
	}

	public int SelectedValue
	{
		get
		{
			return (int.Parse(ddlBatch.SelectedValue));
		}
		set
		{
			_selectedValue = value;
			ddlBatch.SelectedIndex = -1;
			foreach (RadComboBoxItem item in ddlBatch.Items)
			{
				if (item.Value == _selectedValue.ToString())
				{
					item.Selected = true;
					break;
				}
			}
		}
	}
	private void bind()
	{
		ddlBatch.DataSource = PhoenixRegistersBatch.ListBatch(General.GetNullableInteger(CourseId),General.GetNullableInteger(batchstatus),0);
		ddlBatch.DataBind();
		foreach (RadComboBoxItem item in ddlBatch.Items)
		{
			if (item.Value == _selectedValue.ToString())
			{
				item.Selected = true;
				break;
			}
		}
	}

	protected void ddlBatch_DataBound(object sender, EventArgs e)
	{
        if (_appenddatabounditems)
            ddlBatch.Items.Insert(0, new RadComboBoxItem("--Select the Batch--", "Dummy"));
        if (!isoutside)
            ddlBatch.Items.Insert(1, new RadComboBoxItem("non-SIMS", "0"));
    }
}
