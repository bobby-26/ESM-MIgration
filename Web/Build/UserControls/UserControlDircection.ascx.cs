using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlDirection : System.Web.UI.UserControl
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

    public DataSet DirectionList
    {
        set
        {
            ddlDirection.DataBind();
            ddlDirection.Items.Clear();
            ddlDirection.DataSource = value;
            ddlDirection.DataBind();
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
            ddlDirection.CssClass = value;
        }
    }


    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlDirection.AutoPostBack = true;
        }
    }


    public string SelectedDirection
    {
        get
        {

            return ddlDirection.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlDirection.SelectedIndex = -1;
                ddlDirection.ClearSelection();
                ddlDirection.Text = string.Empty;
                return;
            }
            _selectedValue = value;
            ddlDirection.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlDirection.Items)
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
                ddlDirection.Enabled = true;
            else
                ddlDirection.Enabled = false;
        }
    }
    public void bind()
    {
        ddlDirection.DataSource = PhoenixRegistersDirection.ListDirection();
        ddlDirection.DataBind();
        foreach (RadComboBoxItem item in ddlDirection.Items)
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
            return ddlDirection.SelectedValue.ToString();
        }
        set
        {
            _selectedValue = value;
            ddlDirection.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlDirection.Items)
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

    protected void ddlDirection_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlDirection_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlDirection.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
