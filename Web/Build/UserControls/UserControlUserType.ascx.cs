using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlUserType : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    private string _selectedValue = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            bind();
    }

    public DataSet UserTypeList
    {
        set
        {
            ddlUserType.Items.Clear();
            ddlUserType.DataSource = value;
            ddlUserType.DataBind();
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
            ddlUserType.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlUserType.AutoPostBack = true;
        }
    }

    public string SelectedUserType
    {
        get
        {

            return ddlUserType.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlUserType.SelectedIndex = -1;
                ddlUserType.ClearSelection();
                ddlUserType.Text = string.Empty;
                return;
            }
            _selectedValue = value;
            ddlUserType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlUserType.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }


    private void bind()
    {
        ddlUserType.DataSource = PhoenixUser.ListUserType(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        ddlUserType.DataBind();
        foreach (RadComboBoxItem item in ddlUserType.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    public int SelectedIndex
    {
        get
        {
            return ddlUserType.SelectedIndex;
        }
        set
        {
            ddlUserType.SelectedIndex = value;
        }
    }

    protected void ddlUserType_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlUserType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
