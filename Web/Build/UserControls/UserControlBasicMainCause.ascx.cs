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

public partial class UserControlBasicMainCause : System.Web.UI.UserControl
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
            ddlBasicMainCause.Items.Clear();
            ddlBasicMainCause.DataSource = value;
            ddlBasicMainCause.DataBind();
        }
    }

    public string CssClass
    {
        set
        {
            ddlBasicMainCause.CssClass = value;
        }
    }

    public string Width
    {
        get
        {
            return ddlBasicMainCause.Width.ToString();
        }
        set
        {
            ddlBasicMainCause.Width = Unit.Parse(value);
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlBasicMainCause.AutoPostBack = true;
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

            return ddlBasicMainCause.SelectedValue;
        }
        set
        {
            ddlBasicMainCause.SelectedIndex = -1;

            if (value == string.Empty|| General.GetNullableInteger(value) == null)
            {
                ddlBasicMainCause.SelectedIndex = -1;
                ddlBasicMainCause.ClearSelection();
                ddlBasicMainCause.Text = string.Empty;
                return;
            }
            _selectedValue = value.ToString();
            foreach (RadComboBoxItem item in ddlBasicMainCause.Items)
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
            return ddlBasicMainCause.SelectedValue;
        }
        set
        {
            _selectedValue = value;
            ddlBasicMainCause.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlBasicMainCause.Items)
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
        ddlBasicMainCause.SelectedIndex = -1;
        ddlBasicMainCause.DataSource = PhoenixInspectionBasicMainCause.ListMainCause(PhoenixSecurityContext.CurrentSecurityContext.UserCode, maincauseid);
        ddlBasicMainCause.DataBind();
        foreach (RadComboBoxItem item in ddlBasicMainCause.Items)
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

    protected void ddlBasicMainCause_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlBasicMainCause_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlBasicMainCause.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
