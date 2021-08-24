using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControlWFGroup : System.Web.UI.UserControl
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

    public DataTable GroupList
    {
        set
        {
            ddlGroup.DataSource = value;
            ddlGroup.DataBind();
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

        ddlGroup.DataSource =  PhoenixWorkForm.GroupList();
        ddlGroup.DataBind();
        foreach (RadComboBoxItem item in ddlGroup.Items)
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
            ddlGroup.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlGroup.AutoPostBack = true;
        }
    }


    public string SelectedGroup
    {
        get
        {
            return ddlGroup.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlGroup.SelectedIndex = -1;
                ddlGroup.ClearSelection();
                ddlGroup.Text = string.Empty;
                return;
            }
            _selectedValue = General.GetNullableGuid(value);
            ddlGroup.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlGroup.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }


    protected void ddlGroup_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlGroup.Items.Insert(0, new RadComboBoxItem("--Select--", "-1"));
    }
    public Unit Width
    {
        get
        {
            return ddlGroup.Width;
        }
        set
        {
            ddlGroup.Width = value;
        }
    }


}