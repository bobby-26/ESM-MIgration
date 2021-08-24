using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlDesignation : System.Web.UI.UserControl
{
	public event EventHandler TextChangedEvent;
	private bool _appenddatabounditems;
    private int _selectedValue = -1;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
            bind();
		}
	}

	public DataSet DesignationList
	{
		set
		{
			ucDesignation.DataSource = value;
			ucDesignation.DataBind();
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

	public string CssClass
	{
		set
		{
			ucDesignation.CssClass = value;
		}
	}

	public string AutoPostBack
	{
		set
		{
			if (value.ToUpper().Equals("TRUE"))
				ucDesignation.AutoPostBack = true;
		}
	}

	protected void OnTextChangedEvent(EventArgs e)
	{
		if (TextChangedEvent != null)
			TextChangedEvent(this, e);
	}

	protected void ucDesignation_TextChanged(object sender, EventArgs e)
	{
		OnTextChangedEvent(e);
	}

	public string SelectedDesignation
	{
		get
		{
			return ucDesignation.SelectedValue;
		}
		set
		{
            if (value == string.Empty)
            {
                ucDesignation.SelectedIndex = -1;
                ucDesignation.ClearSelection();
                ucDesignation.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ucDesignation.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ucDesignation.Items)
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
        ucDesignation.Items.Clear();
        ucDesignation.DataSource = PhoenixRegistersDesignation.ListDesignation();
        ucDesignation.DataBind();
        foreach (RadComboBoxItem item in ucDesignation.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

	protected void ucDesignation_DataBound(object sender, EventArgs e)
	{
		if (_appenddatabounditems)
			ucDesignation.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
	}

    public Unit Width
    {
        get
        {
            return ucDesignation.Width;
        }
        set
        {
            ucDesignation.Width = value;
        }
    }
}
