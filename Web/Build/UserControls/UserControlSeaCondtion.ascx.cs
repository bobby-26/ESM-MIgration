using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlSeaCondtion : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet SeaConditionList
    {
        set
        {
            ddlSeaCondition.DataBind();
            ddlSeaCondition.Items.Clear();
            ddlSeaCondition.DataSource = value;
            ddlSeaCondition.DataBind();
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
            ddlSeaCondition.CssClass = value;
        }
    }


    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlSeaCondition.AutoPostBack = true;
        }
    }


    public string SelectedSeaCondition
    {
        get
        {

            return ddlSeaCondition.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlSeaCondition.SelectedIndex = -1;
                ddlSeaCondition.ClearSelection();
                ddlSeaCondition.Text = string.Empty;
                return;
            }
            _selectedValue = value;
            ddlSeaCondition.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlSeaCondition.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }
    public string Enabled
    {
        set
        {

            if (value.ToUpper().Equals("TRUE"))
                ddlSeaCondition.Enabled = true;
            else
                ddlSeaCondition.Enabled = false;
        }
    }
    public void bind()
    {
        ddlSeaCondition.DataSource = PhoenixRegistersSeaCondition.ListSeaCondition();
        ddlSeaCondition.DataBind();
        foreach (RadComboBoxItem item in ddlSeaCondition.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }


    public string SelectedValue
    {
        get
        {
            return ddlSeaCondition.SelectedValue.ToString();
        }
        set
        {
            _selectedValue = value;
            ddlSeaCondition.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlSeaCondition.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlSeaCondition_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlSeaCondition_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlSeaCondition.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
