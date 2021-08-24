using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControlWFTarget : System.Web.UI.UserControl
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

    public DataTable TargetList
    {
        set
        {
            ddlTarget.DataSource = value;
            ddlTarget.DataBind();
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

        ddlTarget.DataSource = PhoenixWorkForm.TargetList();
        ddlTarget.DataBind();
        foreach (RadComboBoxItem item in ddlTarget.Items)
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
            ddlTarget.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlTarget.AutoPostBack = true;
        }
    }

    public string SelectedTarget
    {
        get
        {

            return ddlTarget.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlTarget.SelectedIndex = -1;
                ddlTarget.ClearSelection();
                ddlTarget.Text = string.Empty;
                return;
            }
            _selectedValue = General.GetNullableInteger(value);
            ddlTarget.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlTarget.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    protected void ddlTarget_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlTarget.Items.Insert(0, new RadComboBoxItem("--Select--", "-1"));
    }
    public Unit Width
    {
        get
        {
            return ddlTarget.Width;
        }
        set
        {
            ddlTarget.Width = value;
        }
    }

}