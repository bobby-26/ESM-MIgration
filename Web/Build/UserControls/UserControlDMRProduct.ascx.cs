using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Reflection;
using System.Drawing;
using Telerik.Web.UI;
public partial class UserControlDMRProduct : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = "";
    string csvShortName = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();         
        }   
        colorManipulation();
    }

    public DataSet ProductList
    {
        set
        {
            ddlProduct.DataBind();
            ddlProduct.Items.Clear();
            ddlProduct.DataSource = value;
            ddlProduct.DataBind();
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
            ddlProduct.CssClass = value;
        }
    }


    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlProduct.AutoPostBack = true;
        }
    }


    public string SelectedProduct
    {
        get
        {

            return ddlProduct.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlProduct.SelectedIndex = -1;
                ddlProduct.ClearSelection();
                ddlProduct.Text = string.Empty;
                return;
            }
            _selectedValue = value;
            ddlProduct.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlProduct.Items)
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
                ddlProduct.Enabled = true;
            else
                ddlProduct.Enabled = false;
        }
    }
    public void bind()
    {
        ddlProduct.DataSource = PhoenixRegistersDMROilType.ListProduct(General.GetNullableGuid(csvShortName));   
        ddlProduct.DataBind();

        DataSet ds = PhoenixRegistersDMROilType.ListProduct(General.GetNullableGuid(csvShortName));
       
        int i = 0;
        foreach (RadComboBoxItem item in ddlProduct.Items)
        {
            if (item.Value != "Dummy")
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    ddlProduct.BackColor = Color.FromName(ds.Tables[0].Rows[i]["FLDCOLOR"].ToString()); //.FromName("##666699");
                    break;
                }
                i = i + 1;
            }
        }
        colorManipulation();
    }

    private void colorManipulation()
    {
        DataSet ds = PhoenixRegistersDMROilType.ListProduct(General.GetNullableGuid(csvShortName));
        int i = 0;
        foreach (RadComboBoxItem item in ddlProduct.Items)
        {
            if (i < ds.Tables[0].Rows.Count)
            {
                if (item.Value != "Dummy")
                {
                    if (ds.Tables[0].Rows[i]["FLDOILTYPECODE"].ToString() == item.Value.ToString())
                    {
                        item.Attributes.Add("style", "background-color:" + ds.Tables[0].Rows[i]["FLDCOLOR"].ToString());
                    }
                    i = i + 1;
                }
            }
        }

    }

    public string SelectedValue
    {
        get
        {
            return ddlProduct.SelectedValue.ToString();
        }
        set
        {
            _selectedValue = value;
            ddlProduct.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlProduct.Items)
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

    protected void ddlProduct_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlProduct_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
        {
            ddlProduct.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            ddlProduct.BackColor = Color.White;
        }
        else
        {
            ddlProduct.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            ddlProduct.BackColor = Color.White;
        }
    }
    protected void ddlProduct_SelectedChanged(object sender, EventArgs e)
    {

        ddlProduct.BackColor = Color.FromName("#66FF33");
        bind();
        //ddlProduct.Items.FindByValue(ddlProduct.SelectedValue).Selected = true;
        ddlProduct.Items.FindItemByValue(ddlProduct.SelectedValue).Selected = true;     //Telerik Control
    }

    public string ShortNameFilter
    {
        set { csvShortName = value; }
    }
}
