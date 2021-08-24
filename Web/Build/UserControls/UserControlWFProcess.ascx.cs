using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class UserControlWFProcess : System.Web.UI.UserControl
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

    public DataTable ProcessList
    {
        set
        {
            ddlProcess.DataSource = value;
            ddlProcess.DataBind();
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

        ddlProcess.DataSource = PhoenixWorkflow.ProcessList();
        ddlProcess.DataBind();
        foreach (RadComboBoxItem item in ddlProcess.Items)
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
            ddlProcess.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlProcess.AutoPostBack = true;
        }
    }

    public string SelectedProcess
    {
        get
        {
            return ddlProcess.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlProcess.SelectedIndex = -1;
                ddlProcess.ClearSelection();
                ddlProcess.Text = string.Empty;
                return;
            }
            _selectedValue = General.GetNullableGuid(value);
            ddlProcess.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlProcess.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }


    protected void ddlProcess_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlProcess.Items.Insert(0, new RadComboBoxItem("--Select--", "-1"));
    }

    public Unit Width
    {
        get
        {
            return ddlProcess.Width;
        }
        set
        {
            ddlProcess.Width = value;
        }
    }
}