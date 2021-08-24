using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class UserControlWFProcessState : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
     int? _selectedValue;
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

    public DataTable ProcessStateList
    {
        set
        {
            ddlProcessState.Items.Clear();
            ddlProcessState.DataSource = value;
            ddlProcessState.DataBind();
        }
    }

    private void bind()
    {
        ddlProcessState.DataSource = PhoenixWorkflow.ProcessStateList(_ProcessId);
        ddlProcessState.DataBind();

        foreach (RadComboBoxItem item in ddlProcessState.Items)
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
            ddlProcessState.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlProcessState.AutoPostBack = true;
        }
    }


    public string SelectedProcessState
    {
        get
        {
            return ddlProcessState.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlProcessState.SelectedIndex = -1;
                ddlProcessState.ClearSelection();
                ddlProcessState.Text = string.Empty;
                return;
            }
            _selectedValue = General.GetNullableInteger(value);
            ddlProcessState.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlProcessState.Items)
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
            ddlProcessState.Enabled = value;
        }
    }




    protected void ddlProcessState_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
        if (_appenddatabounditems)
            ddlProcessState.Items.Insert(0, new RadComboBoxItem("--Select--", "-1"));
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlProcessState_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public Unit Width
    {
        get
        {
            return ddlProcessState.Width;
        }
        set
        {
            ddlProcessState.Width = value;
        }
    }

   

}