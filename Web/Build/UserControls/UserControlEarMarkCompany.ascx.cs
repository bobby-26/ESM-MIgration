using System;
using System.Data;
using System.Web.UI.WebControls;
//using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;
public partial class UserControlEarMarkCompany : System.Web.UI.UserControl
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

    public DataSet CompanyList
    {
        set
        {
            ddlCompany.DataSource = value;
            ddlCompany.DataBind();
        }
    }
    public string CssClass
    {
        set
        {
            ddlCompany.CssClass = value;
        }
    }
    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlCompany.AutoPostBack = value;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlCompany_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedCompany
    {
        get
        {
            return ddlCompany.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlCompany.SelectedIndex = -1;
                ddlCompany.ClearSelection();
                ddlCompany.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlCompany.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlCompany.Items)
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
        ddlCompany.DataSource = PhoenixAccountsInvoice.InvoiceEarMarkCompanyList() ;
        ddlCompany.DataBind();
        foreach (RadComboBoxItem item in ddlCompany.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    public bool Readonly
    {
        get
        {
            return ddlCompany.Enabled;
        }
        set
        {
            ddlCompany.Enabled = value;
        }
    }
    public string SelectedCompanyName
    {
        get
        {
            return ddlCompany.SelectedItem.Text;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlCompany.Enabled = value;
        }
    }

    protected void ddlCompany_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlCompany.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
