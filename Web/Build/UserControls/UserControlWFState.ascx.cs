using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControlWFState : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    int? _selectedValue;
    int? _StateTypeId;
    public event EventHandler TextChangedEvent;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }

    }

    public string STATETYPEID
    {
        get
        {
            return _StateTypeId.ToString();
                
        }
        set
        {
            _StateTypeId = General.GetNullableInteger(value);
        }
    }



    public DataTable StateTypeList
    {
        set
        {           
            ddlState.Items.Clear();
            ddlState.DataSource = value;
            ddlState.DataBind();
        }
    }

    private void bind()
    {
        ddlState.DataSource = PhoenixWorkflow.WORKFLOWSTATELIST(_StateTypeId);
        ddlState.DataBind();

        foreach (RadComboBoxItem item in ddlState.Items)
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
            ddlState.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlState.AutoPostBack = true;
        }
    }


    public string SelectedState
    {
        get
        {

            return ddlState.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlState.SelectedIndex = -1;
                ddlState.ClearSelection();
                ddlState.Text = string.Empty;
                return;
            }
            _selectedValue = General.GetNullableInteger(value);
            ddlState.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlState.Items)
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
            ddlState.Enabled = value;
        }
    }


   
    protected void ddlState_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlState.Items.Insert(0, new RadComboBoxItem("--Select--", "-1"));
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }


    protected void ddlState_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public Unit Width
    {
        get
        {
            return ddlState.Width;
        }
        set
        {
            ddlState.Width = value;
        }
    }

}