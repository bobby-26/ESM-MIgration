using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlProductType : System.Web.UI.UserControl
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

    public DataSet ProductTypeList
    {
        set
        {
            ddlProductType.DataBind();
            ddlProductType.Items.Clear();
            ddlProductType.DataSource = value;
            ddlProductType.DataBind();
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
            ddlProductType.CssClass = value;
        }
    }


    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlProductType.AutoPostBack = true;
        }
    }


    public string SelectedProductType
    {
        get
        {

            return ddlProductType.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlProductType.SelectedIndex = -1;
                ddlProductType.ClearSelection();
                ddlProductType.Text = string.Empty;
                return;
            }
            _selectedValue = value;
            ddlProductType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlProductType.Items)
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
                ddlProductType.Enabled = true;
            else
                ddlProductType.Enabled = false;
        }
    }
    public void bind()
    {
        ddlProductType.DataSource = PhoenixRegistersDMRProductType.ListProductType();
        ddlProductType.DataBind();
        foreach (RadComboBoxItem item in ddlProductType.Items)
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
            return ddlProductType.SelectedValue.ToString();
        }
        set
        {
            _selectedValue = value;
            ddlProductType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlProductType.Items)
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

    protected void ddlProductType_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlProductType_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlProductType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
