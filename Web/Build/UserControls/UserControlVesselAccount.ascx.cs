using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class UserControlVesselAccount : System.Web.UI.UserControl
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
    public DataSet VesselAccountList
    {
        set
        {
            ddlVesselAccount.DataSource = value;
            ddlVesselAccount.DataBind();
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
            ddlVesselAccount.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlVesselAccount.AutoPostBack = value;
        }
    }
    public bool Enabled
    {
        set
        {
            ddlVesselAccount.Enabled = value;
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlVesselAccount_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedVesselAccount
    {
        get
        {
            return ddlVesselAccount.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlVesselAccount.SelectedIndex = -1;
                ddlVesselAccount.ClearSelection();
                ddlVesselAccount.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlVesselAccount.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlVesselAccount.Items)
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
        ddlVesselAccount.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(null, 1);
        ddlVesselAccount.DataBind();
        foreach (RadComboBoxItem item in ddlVesselAccount.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlVesselAccount_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlVesselAccount.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
