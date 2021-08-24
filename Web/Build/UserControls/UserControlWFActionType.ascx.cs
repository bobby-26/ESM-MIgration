using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControlWFActionType : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    Guid? _selectedValue;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataTable ActionTypeList
    {
        set
        {
            ddlActiontype.DataSource = value;
            ddlActiontype.DataBind();
        }
    }
    private void bind()
    {
        ddlActiontype.DataSource = PhoenixWorkForm.ActionTypeList();        
        ddlActiontype.DataBind();

        foreach (RadComboBoxItem item in ddlActiontype.Items)
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
            ddlActiontype.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlActiontype.AutoPostBack = true;
        }
    }

    public string SelectedActionType
    {
        get
        {

            return ddlActiontype.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlActiontype.SelectedIndex = -1;
                ddlActiontype.ClearSelection();
                ddlActiontype.Text = string.Empty;
                return;
            }
            _selectedValue = General.GetNullableGuid(value);
            ddlActiontype.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlActiontype.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    protected void ddlActiontype_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlActiontype.Items.Insert(0, new RadComboBoxItem("--Select--", "-1"));
    }

    public Unit Width
    {
        get
        {
            return ddlActiontype.Width;
        }
        set
        {
            ddlActiontype.Width = value;
        }
    }
}