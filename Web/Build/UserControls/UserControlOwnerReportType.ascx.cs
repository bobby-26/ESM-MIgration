using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;
public partial class UserControlOwnerReportType : System.Web.UI.UserControl
{

    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;

    private bool? _active = false;
    private string _selectedValue = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                bind();
            }
            catch { }
        }

    }

    public bool ActiveCurrency
    {
        set
        {
            _active = value;
        }
    }

    public DataTable ReportTypeList
    {
        set
        {
            ddlReportType.DataSource = value;
            ddlReportType.DataBind();
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
            ddlReportType.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlReportType.AutoPostBack = value;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlReportType_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedReportType
    {
        get
        {
            return ddlReportType.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlReportType.SelectedIndex = -1;
                ddlReportType.ClearSelection();
                ddlReportType.Text = string.Empty;
                return;
            }
            _selectedValue = value.ToString();
            ddlReportType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlReportType.Items)
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
        ddlReportType.DataSource = PhoenixAccountsOwnerReportDisplay.SOACheckingReportTypeList();        
        ddlReportType.DataBind();
        foreach (RadComboBoxItem item in ddlReportType.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlReportType_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlReportType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    public bool Enabled
    {
        set
        {
            ddlReportType.Enabled = value;
        }
    }

    public Unit Width
    {
        get
        {
            return ddlReportType.Width;
        }
        set
        {
            ddlReportType.Width = value;
        }
    }


    public string SelectedReportTypeText
    {
        get
        {
            return ddlReportType.Text.Trim();
        }
        set
        {
            if (value == string.Empty)
            {
                ddlReportType.SelectedIndex = -1;
                return;
            }
            _selectedValue = value.ToString();
            ddlReportType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlReportType.Items)
            {
                if (item.Text == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

}
