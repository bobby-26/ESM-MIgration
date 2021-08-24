using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlCertificate : System.Web.UI.UserControl
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

    public DataSet CountryList
    {
        set
        {
            ddlCertificate.DataSource = value;
            ddlCertificate.DataBind();
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
            ddlCertificate.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlCertificate.AutoPostBack = true;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlCertificate_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedCountry
    {
        get
        {
            return ddlCertificate.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlCertificate.SelectedIndex = -1;
                ddlCertificate.ClearSelection();
                ddlCertificate.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlCertificate.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlCertificate.Items)
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
        ddlCertificate.DataSource = PhoenixRegistersCertificates.ListCertificates();
        ddlCertificate.DataBind();
        foreach (RadComboBoxItem item in ddlCertificate.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlCertificate_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlCertificate.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    public string Width
    {
        get
        {
            return ddlCertificate.Width.ToString();
        }
        set
        {
            ddlCertificate.Width = Unit.Parse(value);
        }
    }
}
