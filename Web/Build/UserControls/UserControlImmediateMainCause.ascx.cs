using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class UserControlImmediateMainCause : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = string.Empty;
    Guid? maincauseid = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet MainCauseList
    {
        set
        {
            ddlImmediateMainCause.Items.Clear();
            ddlImmediateMainCause.DataSource = value;
            ddlImmediateMainCause.DataBind();
        }
    }    

    public string CssClass
    {
        set
        {
            ddlImmediateMainCause.CssClass = value;
        }
    }

    public string Width
    {
        get
        {
            return ddlImmediateMainCause.Width.ToString();
        }
        set
        {
            ddlImmediateMainCause.Width = Unit.Parse(value);
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlImmediateMainCause.AutoPostBack = true;
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

    public string SelectedMainCause
    {
        get
        {

            return ddlImmediateMainCause.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlImmediateMainCause.SelectedIndex = -1;
                ddlImmediateMainCause.ClearSelection();
                ddlImmediateMainCause.Text = string.Empty;
                return;
            }
            _selectedValue = value.ToString();
            ddlImmediateMainCause.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlImmediateMainCause.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public string SelectedValue
    {
        get
        {
            return ddlImmediateMainCause.SelectedValue;
        }
        set
        {
            _selectedValue = value;
            ddlImmediateMainCause.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlImmediateMainCause.Items)
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
        ddlImmediateMainCause.SelectedIndex = -1;
        ddlImmediateMainCause.DataSource = PhoenixInspectionImmediateMainCause.ListMainCause(PhoenixSecurityContext.CurrentSecurityContext.UserCode, maincauseid);
        ddlImmediateMainCause.DataBind();
        foreach (RadComboBoxItem item in ddlImmediateMainCause.Items)
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

    protected void ddlImmediateMainCause_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlImmediateMainCause_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlImmediateMainCause.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
