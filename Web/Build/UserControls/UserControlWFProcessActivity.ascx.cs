using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControlWFProcessActivity : System.Web.UI.UserControl
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

    public DataTable ProcessActivityList
    {
        set
        {
            ddlProcessActivity.Items.Clear();
            ddlProcessActivity.DataSource = value;
            ddlProcessActivity.DataBind();
        }
    }

    private void bind()
    {
        ddlProcessActivity.DataSource = PhoenixWorkflow.ProcessActivityList(_ProcessId);
        ddlProcessActivity.DataBind();

        foreach (RadComboBoxItem item in ddlProcessActivity.Items)
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
            ddlProcessActivity.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlProcessActivity.AutoPostBack = true;
        }
    }


    public string SelectedProcessActivity
    {
        get
        {

            return ddlProcessActivity.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlProcessActivity.SelectedIndex = -1;
                ddlProcessActivity.ClearSelection();
                ddlProcessActivity.Text = string.Empty;
                return;
            }
            _selectedValue = General.GetNullableGuid(value);
            ddlProcessActivity.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlProcessActivity.Items)
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
            ddlProcessActivity.Enabled = value;
        }
    }


    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }


    protected void ddlProcessActivity_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
       
            if (_appenddatabounditems)
                ddlProcessActivity.Items.Insert(0, new RadComboBoxItem("--Select--", "-1"));
        
    }

    protected void ddlProcessActivity_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public Unit Width
    {
        get
        {
            return ddlProcessActivity.Width;
        }
        set
        {
            ddlProcessActivity.Width = value;
        }
    }


}