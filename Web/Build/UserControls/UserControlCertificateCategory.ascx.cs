using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlCertificateCategory : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet CertificateCategoryList
    {
        set
        {
            ddlCertificateCategory.DataSource = value;
            ddlCertificateCategory.DataBind();
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
            ddlCertificateCategory.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlCertificateCategory.AutoPostBack = true;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlCertificateCategory_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedCategory
    {
        get
        {
            return ddlCertificateCategory.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlCertificateCategory.SelectedIndex = -1;
                ddlCertificateCategory.ClearSelection();
                ddlCertificateCategory.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlCertificateCategory.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlCertificateCategory.Items)
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
        ddlCertificateCategory.DataSource = PhoenixRegistersVesselSurvey.CertificateCategoryList();
        ddlCertificateCategory.DataBind();
        foreach (RadComboBoxItem item in ddlCertificateCategory.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlCertificateCategory_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlCertificateCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    public string Width
    {
        get
        {
            return ddlCertificateCategory.Width.ToString();
        }
        set
        {
            ddlCertificateCategory.Width = Unit.Parse(value);
        }
    }
}