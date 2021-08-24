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
public partial class UserControlWorkPermitByCompany : System.Web.UI.UserControl
{
    int _categorynumber;
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public string CategoryNumber
    {
        get
        {
            return _categorynumber.ToString();
        }
        set
        {
            _categorynumber = int.Parse(value);
        }
    }

    public string Company
    {
        get;
        set;
    }

    public DataSet WorkPermitList
    {
        set
        {
            ddlWorkPermit.Items.Clear();
            ddlWorkPermit.DataSource = value;
            ddlWorkPermit.DataBind();
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
            ddlWorkPermit.CssClass = value;
        }
    }

    public string Width
    {
        get
        {
            return ddlWorkPermit.Width.ToString();
        }
        set
        {
            ddlWorkPermit.Width = Unit.Parse(value);
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlWorkPermit.AutoPostBack = true;
        }
    }


    public string SelectedWorkPermit
    {
        get
        {

            return ddlWorkPermit.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlWorkPermit.SelectedIndex = -1;
                ddlWorkPermit.ClearSelection();
                ddlWorkPermit.Text = string.Empty;
                return;
            }
            _selectedValue = value.ToString();
            ddlWorkPermit.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlWorkPermit.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public void bind()
    {
        ddlWorkPermit.DataSource = PhoenixInspectionDailyWorkPlanActivity.WorkPermitListByCompany(_categorynumber, General.GetNullableInteger(Company));
        ddlWorkPermit.DataBind();
        foreach (RadComboBoxItem item in ddlWorkPermit.Items)
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

            return ddlWorkPermit.SelectedValue;
        }
        set
        {
            _selectedValue = value.ToString();
            ddlWorkPermit.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlWorkPermit.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }


    public string SelectedText
    {
        get
        {

            return ddlWorkPermit.SelectedValue;
        }
        set
        {
            value = ddlWorkPermit.SelectedValue;
        }
    }
    public bool Enabled
    {
        set
        {
            ddlWorkPermit.Enabled = value;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlWorkPermit_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlWorkPermit_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlWorkPermit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }    
}
