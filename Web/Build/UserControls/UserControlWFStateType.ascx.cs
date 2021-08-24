using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class UserControlWFStateType : System.Web.UI.UserControl
{

    private bool _appenddatabounditems;
    int? _selectedValue;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataTable StateTypeList
    {
        set
        {
            ddlStateType.DataSource = value;
            ddlStateType.DataBind();
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

    private void bind()
    {

        ddlStateType.DataSource = PhoenixWorkForm.StateTypeList();
        ddlStateType.DataBind();    
        foreach (RadComboBoxItem item in ddlStateType.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }


    public string CssClass
    {
        set
        {
            ddlStateType.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlStateType.AutoPostBack = true;
        }
    }


    public string SelectedStateType
    {     
        get
        {

            return ddlStateType.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlStateType.SelectedIndex = -1;
                ddlStateType.ClearSelection();
                ddlStateType.Text = string.Empty;
                return;
            }
            _selectedValue = General.GetNullableInteger(value);
            ddlStateType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlStateType.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    protected void ddlStateType_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlStateType.Items.Insert(0, new RadComboBoxItem("--Select--", "-1"));
    }

    public Unit Width
    {
        get
        {
            return ddlStateType.Width;
        }
        set
        {
            ddlStateType.Width = value;
        }
    }
}