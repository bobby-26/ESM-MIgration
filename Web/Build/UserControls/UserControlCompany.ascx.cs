using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class UserControlCompany : System.Web.UI.UserControl
{

    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    private string _Userid;

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
    public string UserId
    {
        set
        {
            _Userid = value;
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
    public void bind()
    {
        if (!string.IsNullOrEmpty(_Userid))
        {
            ddlCompany.DataSource = PhoenixRegistersCompany.ListCompany(General.GetNullableInteger(_Userid));
            ddlCompany.DataBind();
        }
        else
        {
            ddlCompany.DataSource = PhoenixRegistersCompany.ListCompany();
            ddlCompany.DataBind();
        }
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

    public bool ShowToggleImage
    {
        set
        {
            ddlCompany.ShowToggleImage = value;
        }
    }

    public bool ShowDropDownOnTextboxClick
    {
        set
        {
            ddlCompany.ShowDropDownOnTextboxClick = value;
        }
    }


    public bool Enabled
    {
        set
        {
            ddlCompany.Enabled = value;
        }
    }
    public string Width
    {
        get
        {
            return ddlCompany.Width.ToString();
        }
        set
        {
            ddlCompany.Width = Unit.Parse(value);
        }
    }

    protected void ddlCompany_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlCompany.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
