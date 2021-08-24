using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControlCrewContract : System.Web.UI.UserControl
{
    int? EmployeeId = null;
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }
    public DataSet ContractList
    {
        set
        {
            ddlContract.DataSource = value;
            ddlContract.DataBind();
        }
    }

    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;
        }
    }

    public string CssClass
    {
        set
        {
            ddlContract.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlContract.AutoPostBack = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlContract.Enabled = value;
        }
    }

    public string employeeId
    {
        get
        {
            return EmployeeId.ToString();
        }
        set
        {
            EmployeeId = General.GetNullableInteger(value);
        }
    }
    protected void ddlContract_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlContract.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void ddlContract_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }
    public string SelectedContract
    {
        get
        {
            return ddlContract.SelectedValue;
        }
        set
        {
            ddlContract.SelectedIndex = -1;
            ddlContract.Text = "";
            ddlContract.ClearSelection();

            if (value == string.Empty || General.GetNullableGuid(value) == null)
            {
                ddlContract.Text = string.Empty;
                return;
            }
            _selectedValue = value;

            foreach (RadComboBoxItem item in ddlContract.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public string Text
    {
        get
        {
            return ddlContract.Text;
        }
        set
        {
            ddlContract.Text = value;
        }
    }

    public Unit Width
    {
        get
        {
            return ddlContract.Width;
        }
        set
        {
            ddlContract.Width = value;
        }
    }

    private void bind()
    {
        ddlContract.DataSource = PhoenixCrewManagement.CrewContractList(int.Parse(employeeId));
        ddlContract.DataBind();
        foreach (RadComboBoxItem item in ddlContract.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
}