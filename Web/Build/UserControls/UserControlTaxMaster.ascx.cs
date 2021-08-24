using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlTaxMaster : System.Web.UI.UserControl
{

    private bool _appenddatabounditems;
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet TaxList
    {
        set
        {
            ddlTaxMaster.Items.Clear();
            ddlTaxMaster.DataSource = value;
            ddlTaxMaster.DataBind();
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
            ddlTaxMaster.CssClass = value;
        }
    }


    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlTaxMaster.AutoPostBack = true;
        }
    }

    public string SelectedTax
    {
        get
        {

            return ddlTaxMaster.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlTaxMaster.SelectedIndex = -1;
                ddlTaxMaster.ClearSelection();
                ddlTaxMaster.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlTaxMaster.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlTaxMaster.Items)
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

            return (int.Parse(ddlTaxMaster.SelectedValue));
        }
        set
        {
            _selectedValue = value;
            ddlTaxMaster.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlTaxMaster.Items)
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
        ddlTaxMaster.DataSource = PhoenixRegistersTax.TaxMasterList();
        ddlTaxMaster.DataBind();
        foreach (RadComboBoxItem item in ddlTaxMaster.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    protected void ddlTaxMaster_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlTaxMaster.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
