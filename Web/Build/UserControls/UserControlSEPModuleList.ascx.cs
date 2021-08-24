using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;
using Telerik.Web.UI;
public partial class UserControlSepModuleList : System.Web.UI.UserControl
{
    string module_name = null;
    int? module_id = null;

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

    public DataTable ModuleList
    {
        set
        {
            ddlModuleList.DataSource = value;
            ddlModuleList.DataBind();
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
            ddlModuleList.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlModuleList.AutoPostBack = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlModuleList.Enabled = value;
        }
    }

    public string ModuleName
    {
        get
        {
            return ddlModuleList.SelectedItem.Text;
        }
        set
        {
            module_name = null;
        }
    }

    public string ModuleID
    {
        get
        {
            return module_id.ToString();
        }
        set
        {
            module_id = General.GetNullableInteger(value);
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlModuleList_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedValue
    {
        get
        {
            return ddlModuleList.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlModuleList.SelectedIndex = -1;
                ddlModuleList.ClearSelection();
                ddlModuleList.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlModuleList.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlModuleList.Items)
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
        ddlModuleList.DataSource = PhoenixDefectTracker.ModuleList(module_id == null ? null : module_id
             , module_name == null ? null : module_name);
        ddlModuleList.DataBind();
        foreach (RadComboBoxItem item in ddlModuleList.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlModuleList_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlModuleList.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
