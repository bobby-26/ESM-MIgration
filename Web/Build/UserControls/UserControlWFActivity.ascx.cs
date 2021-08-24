using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControlWFActivity : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    Guid? _selectedValue;
    Guid? _ActivityTypeId;
    public event EventHandler TextChangedEvent;
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            bind();
        }

    }


    public string ActivityTypeId
    {
        get
        {
            return _ActivityTypeId.ToString();

        }
        set
        {
            _ActivityTypeId = General.GetNullableGuid(value);
        }
    }



    public DataTable ActionTypeList
    {
        set
        {
            ddlActivity.Items.Clear();
            ddlActivity.DataSource = value;
            ddlActivity.DataBind();
        }
    }

    private void bind()
    {
        ddlActivity.DataSource = PhoenixWorkflow.WORKFLOWACTIVITYLIST(_ActivityTypeId);
        ddlActivity.DataBind();

        foreach (RadComboBoxItem item in ddlActivity.Items)
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
            ddlActivity.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlActivity.AutoPostBack = true;
        }
    }

    public string SelectedActivity
    {
        get
        {

            return ddlActivity.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlActivity.SelectedIndex = -1;
                ddlActivity.ClearSelection();
                ddlActivity.Text = string.Empty;
                return;
            }
            _selectedValue = General.GetNullableGuid(value);
            ddlActivity.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlActivity.Items)
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
            ddlActivity.Enabled = value;
        }
    }


    protected void ddlActivity_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
        if (_appenddatabounditems)
            ddlActivity.Items.Insert(0, new RadComboBoxItem("--select--", "-1"));
    }


    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlActivity_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public Unit Width
    {
        get
        {
            return ddlActivity.Width;
        }
        set
        {
            ddlActivity.Width = value;
        }
    }
}