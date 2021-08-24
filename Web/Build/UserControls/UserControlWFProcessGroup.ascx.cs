using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class UserControlWFProcessGroup : System.Web.UI.UserControl
{

    private bool _appenddatabounditems;
    Guid? _selectedValue;
    Guid? _ProcessId;
    public event EventHandler TextChangedEvent;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }


    public string ProcessId
    {
        get
        {
            return _ProcessId.ToString();

        }
        set
        {
            _ProcessId = General.GetNullableGuid(value);
        }
    }

    public DataTable ProcessGroupList
    {
        set
        {
            ddlProcessGroup.Items.Clear();
            ddlProcessGroup.DataSource = value;
            ddlProcessGroup.DataBind();
        }
    }


    private void bind()
    {
        ddlProcessGroup.DataSource = PhoenixWorkflow.ProcessGroupList(_ProcessId);
        ddlProcessGroup.DataBind();

        foreach (RadComboBoxItem item in ddlProcessGroup.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
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
            ddlProcessGroup.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlProcessGroup.AutoPostBack = true;
        }
    }

    public string SelectedProcessGroup
    {
        get
        {

            return ddlProcessGroup.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlProcessGroup.SelectedIndex = -1;
                ddlProcessGroup.ClearSelection();
                ddlProcessGroup.Text = string.Empty;
                return;
            }
            _selectedValue = General.GetNullableGuid(value);
            ddlProcessGroup.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlProcessGroup.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public bool Enabled
    {
        set
        {
            ddlProcessGroup.Enabled = value;
        }
    }


    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlProcessGroup_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
        if (_appenddatabounditems)
            ddlProcessGroup.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void ddlProcessGroup_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public Unit Width
    {
        get
        {
            return ddlProcessGroup.Width;
        }
        set
        {
            ddlProcessGroup.Width = value;
        }
    }


}