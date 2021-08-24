using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;
public partial class UserControlOwnerSubReportType : System.Web.UI.UserControl
{

    string reporttypecode;
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;

    private bool? _active = false;
    private int _selectedValue = -1;

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

    public string ReportCode
    {
        get
        {
            return reporttypecode.ToString();
        }
        set
        {
            reporttypecode = value;
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
            ddlSubReportType.DataSource = value;
            ddlSubReportType.DataBind();
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
            ddlSubReportType.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlSubReportType.AutoPostBack = value;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlSubReportType_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedReportType
    {
        get
        {
            return ddlSubReportType.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlSubReportType.SelectedIndex = -1;
                ddlSubReportType.ClearSelection();
                ddlSubReportType.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlSubReportType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlSubReportType.Items)
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
        ddlSubReportType.DataSource = PhoenixAccountsOwnerReportDisplay.SOACheckingSubReportTypeList(ReportCode);            
        ddlSubReportType.DataBind();
        foreach (RadComboBoxItem item in ddlSubReportType.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlSubReportType_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlSubReportType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    public bool Enabled
    {
        set
        {
            ddlSubReportType.Enabled = value;
        }
    }

    public Unit Width
    {
        get
        {
            return ddlSubReportType.Width;
        }
        set
        {
            ddlSubReportType.Width = value;
        }
    }

    public string SelectedReportTypeText
    {
        get
        {
            return ddlSubReportType.Text.Trim();
        }
        set
        {
            if (value == string.Empty)
            {
                ddlSubReportType.SelectedIndex = -1;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlSubReportType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlSubReportType.Items)
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
