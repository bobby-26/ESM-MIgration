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

public partial class UserControlBasicSubCause : System.Web.UI.UserControl
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
            ddlBasicSubCause.Items.Clear();
            ddlBasicSubCause.DataSource = value;
            ddlBasicSubCause.DataBind();
        }
    }

    public string CssClass
    {
        set
        {
            ddlBasicSubCause.CssClass = value;
        }
    }

    public string Width
    {
        get
        {
            return ddlBasicSubCause.Width.ToString();
        }
        set
        {
            ddlBasicSubCause.Width = Unit.Parse(value);
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlBasicSubCause.AutoPostBack = true;
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

            return ddlBasicSubCause.SelectedValue;
        }
        set
        {
            ddlBasicSubCause.SelectedIndex = -1;
            if (value == string.Empty || General.GetNullableInteger(value) == null)
            {
                ddlBasicSubCause.SelectedIndex = -1;
                ddlBasicSubCause.ClearSelection();
                ddlBasicSubCause.Text = string.Empty;
                return;
            }
            _selectedValue = value.ToString();
            foreach (RadComboBoxItem item in ddlBasicSubCause.Items)
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
            return ddlBasicSubCause.SelectedValue;
        }
        set
        {
            _selectedValue = value;
            ddlBasicSubCause.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlBasicSubCause.Items)
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
        ddlBasicSubCause.SelectedIndex = -1;
        ddlBasicSubCause.DataSource = PhoenixInspectionBasicSubCause.ListSubCause(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(maincauseid));
        ddlBasicSubCause.DataBind();
        foreach (RadComboBoxItem item in ddlBasicSubCause.Items)
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

    protected void ddlBasicSubCause_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlBasicSubCause_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlBasicSubCause.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
