using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlRank : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    public event EventHandler TextChangedEvent;
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet RankList
    {
        set
        {
            ddlRank.Items.Clear();
            ddlRank.DataSource = value;
            ddlRank.DataBind();
        }
    }
	public bool Enabled
	{
		set
		{
			ddlRank.Enabled = value;
		}
	}
    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;
        }
    }

    public string CssClass
    {
        set
        {
            ddlRank.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlRank.AutoPostBack = value;
        }
    }
    public string DataBoundItemName
    {
        set;
        get;
    }
    public string SelectedRank
    {
        get
        {
            return ddlRank.SelectedValue;
        }
        set
        {
            ddlRank.Text = "";
            ddlRank.SelectedIndex = -1;

            if (value == string.Empty || General.GetNullableInteger(value) == null)
            {
                ddlRank.SelectedIndex = -1;
                ddlRank.ClearSelection();
                ddlRank.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            foreach (RadComboBoxItem item in ddlRank.Items)
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

            return (int.Parse(ddlRank.SelectedValue));
        }
        set
        {
            _selectedValue = value;
            ddlRank.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlRank.Items)
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
        ddlRank.DataSource = PhoenixRegistersRank.ListRank();
        ddlRank.DataBind();
        foreach (RadComboBoxItem item in ddlRank.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlRank_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlRank_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlRank.Items.Insert(0, new RadComboBoxItem((!string.IsNullOrEmpty(DataBoundItemName) ? DataBoundItemName : "--Select--"), "Dummy"));        
    }

    public string Width
    {
        get
        {
            return ddlRank.Width.ToString();
        }
        set
        {
            ddlRank.Width = Unit.Parse(value);
        }
    }
    public override void Focus()
    {                
        ddlRank.Focus();        
    }
}
