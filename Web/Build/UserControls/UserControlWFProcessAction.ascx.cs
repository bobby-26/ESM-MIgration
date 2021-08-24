using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class UserControlWFProcessAction : System.Web.UI.UserControl
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

    public DataTable ProcessActionList
    {
        set
        {
            ddlProcessAction.Items.Clear();
            ddlProcessAction.DataSource = value;
            ddlProcessAction.DataBind();
        }
    }

    private void bind()
    {
        ddlProcessAction.DataSource = PhoenixWorkflow.ProcessActionList(_ProcessId);
        ddlProcessAction.DataBind();

        foreach (RadComboBoxItem item in ddlProcessAction.Items)
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
            ddlProcessAction.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlProcessAction.AutoPostBack = true;
        }
    }

    public string SelectedProcessAction
    {
        get
        {

            return ddlProcessAction.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlProcessAction.SelectedIndex = -1;
                ddlProcessAction.ClearSelection();
                ddlProcessAction.Text = string.Empty;
                return;
            }
            _selectedValue = General.GetNullableGuid(value);
            ddlProcessAction.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlProcessAction.Items)
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
            ddlProcessAction.Enabled = value;
        }
    }




    protected void ddlProcessAction_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
       if (_appenddatabounditems)
           ddlProcessAction.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }


    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlProcessAction_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public Unit Width
    {
        get
        {
            return ddlProcessAction.Width;
        }
        set
        {
            ddlProcessAction.Width = value;
        }
    }

}