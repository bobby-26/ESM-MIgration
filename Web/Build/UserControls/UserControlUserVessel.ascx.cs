using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControlUserVessel : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet VesselList
    {
        set
        {
            ddlVessel.Items.Clear();
            ddlVessel.DataSource = value;
            ddlVessel.DataBind();
        }
    }

    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;          
        }
    }

    public bool Enabled
    {
        set
        {
            ddlVessel.Enabled = value;
        }
    }

    public string CssClass
    {
        set
        {
            ddlVessel.CssClass = value;
        }
    }


    public bool AutoPostBack
    {
        set
        {            
                ddlVessel.AutoPostBack = value;
        }
    }

    public string SelectedVessel
    {
        get
        {
            return ddlVessel.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlVessel.SelectedIndex = -1;
                ddlVessel.ClearSelection();
                ddlVessel.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlVessel.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlVessel.Items)
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
        ddlVessel.DataSource = PhoenixRegistersVessel.ListUserAssignedVessel();
        ddlVessel.DataBind();
        foreach (RadComboBoxItem item in ddlVessel.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    protected void ddlVessel_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlVessel.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlVessel_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string Width
    {
        get
        {
            return ddlVessel.Width.ToString();
        }
        set
        {
            ddlVessel.Width = Unit.Parse(value);
        }
    }
}
