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

public partial class UserControlImmediateSubCause : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = string.Empty;
    string maincauseid = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet SubCauseList
    {
        set
        {
            ddlImmediateSubCause.Items.Clear();
            ddlImmediateSubCause.DataSource = value;
            ddlImmediateSubCause.DataBind();
        }
    }

    public string CssClass
    {
        set
        {
            ddlImmediateSubCause.CssClass = value;
        }
    }

    public string Width
    {
        get
        {
            return ddlImmediateSubCause.Width.ToString();
        }
        set
        {
            ddlImmediateSubCause.Width = Unit.Parse(value);
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlImmediateSubCause.AutoPostBack = true;
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

    public string SelectedSubCause
    {
        get
        {

            return ddlImmediateSubCause.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlImmediateSubCause.SelectedIndex = -1;
                ddlImmediateSubCause.ClearSelection();
                ddlImmediateSubCause.Text = string.Empty;
                return;
            }
            _selectedValue = value.ToString();
            ddlImmediateSubCause.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlImmediateSubCause.Items)
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
            return ddlImmediateSubCause.SelectedValue;
        }
        set
        {
            _selectedValue = value;
            ddlImmediateSubCause.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlImmediateSubCause.Items)
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
        ddlImmediateSubCause.SelectedIndex = -1;
        ddlImmediateSubCause.DataSource = PhoenixInspectionImmediateSubCause.ListSubCause(PhoenixSecurityContext.CurrentSecurityContext.UserCode,General.GetNullableGuid(maincauseid));
        ddlImmediateSubCause.DataBind();
        foreach (RadComboBoxItem item in ddlImmediateSubCause.Items)
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

    protected void ddlImmediateSubCause_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlImmediateSubCause_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlImmediateSubCause.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
