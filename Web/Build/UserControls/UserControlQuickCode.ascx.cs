using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlQuickCode : System.Web.UI.UserControl
{
    int _quicktype;
    private int _selectedValue = -1;
    private bool _appenddatabounditems;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            bind();

        }
    }

    public string QuickType
    {
        get
        {
            return _quicktype.ToString();
        }
        set
        {
            _quicktype = Int32.Parse(value);
        }
    }


    public DataSet PriceClassList
    {
        set
        {
            ddlPriceClass.DataBind();
            ddlPriceClass.Items.Clear();
            ddlPriceClass.DataSource = value;
            ddlPriceClass.DataBind();
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
            ddlPriceClass.CssClass = value;
        }
    }


    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlPriceClass.AutoPostBack = true;
        }
    }


    public string SelectedPriceClass
    {
        get
        {

            return ddlPriceClass.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlPriceClass.SelectedIndex = -1;
                ddlPriceClass.ClearSelection();
                ddlPriceClass.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlPriceClass.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlPriceClass.Items)
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
        ddlPriceClass.DataSource = PhoenixRegistersQuick.ListQuick(1, _quicktype);
        ddlPriceClass.DataBind();
        foreach (RadComboBoxItem item in ddlPriceClass.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    protected void ddlPriceClass_DataBound(object sender, EventArgs e) 
    {
        if (_appenddatabounditems)
            ddlPriceClass.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
