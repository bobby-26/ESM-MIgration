using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControlWFActivityType : System.Web.UI.UserControl
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
            ddlActivitytype.DataSource = value;
            ddlActivitytype.DataBind();
        }
    }
    private void bind()
    {
        ddlActivitytype.DataSource = PhoenixWorkForm.ActivityTypeList();       
        ddlActivitytype.DataBind();

        foreach (RadComboBoxItem item in ddlActivitytype.Items)
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
            ddlActivitytype.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlActivitytype.AutoPostBack = true;
        }
    }

    public string SelectedActivityType
    {
        get
        {

            return ddlActivitytype.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlActivitytype.SelectedIndex = -1;
                ddlActivitytype.ClearSelection();
                ddlActivitytype.Text = string.Empty;
                return;
            }
            _selectedValue = General.GetNullableGuid(value);
            ddlActivitytype.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlActivitytype.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    protected void ddlActivitytype_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlActivitytype.Items.Insert(0, new RadComboBoxItem("--Select--", "-1"));
    }

    public Unit Width
    {
        get
        {
            return ddlActivitytype.Width;
        }
        set
        {
            ddlActivitytype.Width = value;
        }
    }
}