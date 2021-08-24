using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;
using System.Data;

public partial class UserControlGlobalWageComponent : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private Guid _selectedValue;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet ComponentList
    {
        set
        {
            ddlGWC.Items.Clear();
            ddlGWC.DataSource = value;
            ddlGWC.DataBind();
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
            ddlGWC.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlGWC.AutoPostBack = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlGWC.Enabled = value;
        }
    }

    public string SelectedComponent
    {
        get
        {
            return ddlGWC.SelectedValue;
        }
        set
        {
            ddlGWC.SelectedIndex = -1;
            ddlGWC.Text = "";
            ddlGWC.ClearSelection();

            if (value == string.Empty || General.GetNullableGuid(value) == null)
            {
                ddlGWC.Text = string.Empty;
                return;
            }
            _selectedValue = new Guid(value);

            foreach (RadComboBoxItem item in ddlGWC.Items)
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
        ddlGWC.DataSource = PhoenixRegisterGlobalWageComponent.GloabalWageComponentList(1);
        ddlGWC.DataBind();
        foreach (RadComboBoxItem item in ddlGWC.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    protected void ddlGWC_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlGWC.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }
    protected void ddlGWC_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public Unit Width
    {
        get
        {
            return ddlGWC.Width;
        }
        set
        {
            ddlGWC.Width = value;
        }
    }
}