using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControlWFAction : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    Guid? _selectedValue;
    Guid? _ActionTypeId;
    public event EventHandler TextChangedEvent;
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            bind();
        }
    }


    public string ActionTypeId
    {
        get
        {
            return _ActionTypeId.ToString();

        }
        set
        {
            _ActionTypeId = General.GetNullableGuid(value);
        }
    }



    public DataTable ActionTypeList
    {
        set
        {
            ddlAction.Items.Clear();
            ddlAction.DataSource = value;
            ddlAction.DataBind();
        }
    }


    private void bind()
    {
        ddlAction.DataSource = PhoenixWorkflow.WORKFLOWACTIONLIST(_ActionTypeId);
        ddlAction.DataBind();

        foreach (RadComboBoxItem item in ddlAction.Items)
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
            ddlAction.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlAction.AutoPostBack = true;
        }
    }

    public string SelectedAction
    {
        get
        {

            return ddlAction.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlAction.SelectedIndex = -1;
                ddlAction.ClearSelection();
                ddlAction.Text = string.Empty;
                return;
            }
            _selectedValue = General.GetNullableGuid(value);
            ddlAction.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlAction.Items)
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
            ddlAction.Enabled = value;
        }
    }

    protected void ddlAction_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
        if (_appenddatabounditems)
            ddlAction.Items.Insert(0, new RadComboBoxItem("--Select--", "-1"));
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }
    protected void ddlAction_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public Unit Width
    {
        get
        {
            return ddlAction.Width;
        }
        set
        {
            ddlAction.Width = value;
        }
    }
}