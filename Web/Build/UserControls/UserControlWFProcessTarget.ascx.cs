using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class UserControlWFProcessTarget : System.Web.UI.UserControl
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

    public DataTable ProcessTargetList
    {
        set
        {
            ddlProcessTarget.Items.Clear();
            ddlProcessTarget.DataSource = value;
            ddlProcessTarget.DataBind();
        }
    }

    private void bind()
    {
        ddlProcessTarget.DataSource = PhoenixWorkflow.ProcessTargetList(_ProcessId);
        ddlProcessTarget.DataBind();

        foreach (RadComboBoxItem item in ddlProcessTarget.Items)
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
            ddlProcessTarget.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlProcessTarget.AutoPostBack = true;
        }
    }


    public string SelectedProcessTarget
    {
        get
        {

            return ddlProcessTarget.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlProcessTarget.SelectedIndex = -1;
                ddlProcessTarget.ClearSelection();
                ddlProcessTarget.Text = string.Empty;
                return;
            }
            _selectedValue = General.GetNullableInteger(value);
            ddlProcessTarget.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlProcessTarget.Items)
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
            ddlProcessTarget.Enabled = value;
        }
    }


    protected void ddlProcessTarget_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
        if (_appenddatabounditems)
            ddlProcessTarget.Items.Insert(0, new RadComboBoxItem("--Select--", "-1"));
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlProcessTarget_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public Unit Width
    {
        get
        {
            return ddlProcessTarget.Width;
        }
        set
        {
            ddlProcessTarget.Width = value;
        }
    }

}