using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Document;
using Telerik.Web.UI;
public partial class UserControlVesselTables : System.Web.UI.UserControl
{
    public delegate void TextChangedDelegate(object o, EventArgs e);
    public event TextChangedDelegate TextChangedEvent;

    private bool _appenddatabounditems;
    private int _selectedValue = -3;
    private byte? audityn = 1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public bool Enabled
    {
        set
        {
            ddlTables.Enabled = value;
        }
    }

    public DataTable TableList
    {
        set
        {
            ddlTables.Items.Clear();
            ddlTables.DataSource = value;
            ddlTables.DataBind();
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
            ddlTables.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlTables.AutoPostBack = value;
        }
    }
    public bool AuditTableYn
    {
        set
        {
            if (value)
            {
                audityn = 1;
            }
            else
            {
                audityn = 0;
            }
        }        
    }
    public bool AppendOwnerCharterer
    {
        set;
        get;        
    }
    public Unit Width
    {
        set
        {
            ddlTables.Width = value;
        }
    }

    public string SelectedTableName
    {
        get
        {
            //return ddlTables.SelectedItem.Text;
            return General.GetNullableString(ddlTables.Text);
        }
    }

    public string SelectedTable
    {
        get
        {
            return ddlTables.SelectedValue;
        }
        set
        {
            ddlTables.SelectedIndex = -1;
            if (value == string.Empty)
            {
                ddlTables.ClearSelection();
                ddlTables.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlTables.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlTables.Items)
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
        ddlTables.DataSource = PhoenixDocumentsTables.ListTables(PhoenixSecurityContext.CurrentSecurityContext.UserCode, audityn);
        ddlTables.DataBind();

        foreach (RadComboBoxItem item in ddlTables.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlTables_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlTables.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void OnUserControlVesselTablesTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlTables_TextChanged(object sender, EventArgs e)
    {
        OnUserControlVesselTablesTextChangedEvent(e);
    }

}