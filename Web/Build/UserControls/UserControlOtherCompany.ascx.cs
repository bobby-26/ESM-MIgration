using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class UserControlOtherCompany : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int? showfieldoffice = null;
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public bool ShowFieldOffice
    {
        set
        {
            if (value == true)
                showfieldoffice = null;
            else
                showfieldoffice = 0;
        }
    }

    public DataSet OtherCompanyList
    {
        set
        {
            ddlOtherCompany.DataSource = value;
            ddlOtherCompany.DataBind();
        }
    }

    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;
        }
    }

    public string CssClass
    {
        set
        {
            ddlOtherCompany.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
             ddlOtherCompany.AutoPostBack = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlOtherCompany.Enabled = value;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlOtherCompany_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedOtherCompany
    {
        get
        {
            return ddlOtherCompany.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlOtherCompany.SelectedIndex = -1;
                ddlOtherCompany.ClearSelection();
                ddlOtherCompany.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlOtherCompany.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlOtherCompany.Items)
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
        ddlOtherCompany.DataSource = PhoenixRegistersOtherCompany.ListOtherCompany(showfieldoffice);
        ddlOtherCompany.DataBind();
        foreach (RadComboBoxItem item in ddlOtherCompany.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    protected void ddlOtherCompany_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlOtherCompany.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    public Unit Width
    {
        get
        {
            return ddlOtherCompany.Width;
        }
        set
        {
            ddlOtherCompany.Width = value;
        }
    }
}
