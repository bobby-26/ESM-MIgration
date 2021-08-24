using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.OwnerBudget;
using Telerik.Web.UI;
public partial class UserControlOfficerType : System.Web.UI.UserControl
{
    string officertype = null;
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

    public string OfficerType
    {
        get
        {
            return officertype;
        }
        set
        {
            officertype = value;
        }
    }
    public DataSet OfficerTypeList
    {
        set
        {
            ddlOfficerType.DataBind();
            ddlOfficerType.Items.Clear();
            ddlOfficerType.DataSource = value;
            ddlOfficerType.DataBind();
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
            ddlOfficerType.CssClass = value;
        }
    }


    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlOfficerType.AutoPostBack = true;
        }
    }


    public string SelectedOfficerType
    {
        get
        {

            return ddlOfficerType.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlOfficerType.SelectedIndex = -1;
                ddlOfficerType.ClearSelection();
                ddlOfficerType.Text = string.Empty;
                return;
            }
            _selectedValue = value;
            ddlOfficerType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlOfficerType.Items)
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
                ddlOfficerType.Enabled = true;
            else
                ddlOfficerType.Enabled = false;
        }
    }
    public void bind()
    {
        ddlOfficerType.DataSource = PhoenixOwnerBudget.OwnerBudgetOfficerTypeList(General.GetNullableInteger(officertype));
        ddlOfficerType.DataBind();
        foreach (RadComboBoxItem item in ddlOfficerType.Items)
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
            return ddlOfficerType.SelectedValue.ToString();
        }
        set
        {
            _selectedValue = value;
            ddlOfficerType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlOfficerType.Items)
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

    protected void ddlOfficerType_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlOfficerType_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlOfficerType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
