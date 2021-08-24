using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using Telerik.Web.UI;
public partial class UserControlOwnersVessel : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    private int _selecteduser = -1;

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

    public int Usercode
    {
        set
        {
            _selecteduser = value;
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
            ddlVessel.CssClass = value;
        }
        get { return ddlVessel.CssClass; }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlVessel.AutoPostBack = true;
        }
    }

    public string SelectedVesselName
    {
        get
        {
            return ddlVessel.SelectedItem.Text;
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
            if (value == string.Empty || value == "Dummy")
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

    public bool Enabled
    {
        set
        {
            ddlVessel.Enabled = value;
        }
    }
    public string Flag
    {
        get;
        set;
    }

    public string Principal
    {
        get;
        set;
    }
    public int SelectedValue
    {
        get
        {
            return (int.Parse(ddlVessel.SelectedValue));
        }
        set
        {
            _selectedValue = value;
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
    public void bind()
    {

        ddlVessel.DataSource = PhoenixOwnersVessel.ListAssignedVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
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
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlVessel_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlVessel_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlVessel.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    public Unit Width
    {
        get
        {
            return ddlVessel.Width;
        }
        set
        {
            ddlVessel.Width = value;

        }
    }
}
