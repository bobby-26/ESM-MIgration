using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Owners;
using Telerik.Web.UI;

public partial class UserControlVesselByUserType : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private bool _vesselsonly = false;
    private bool _assignedVessels=false;
    private int _selectedValue=-1;
    private int _selecteduser = -1;
    private string _PlannedDays;
    private bool _appenditempresea;
    private bool _syncactivevesselsonly = false;
    
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
    public string PlannedDays
    {
        set 
        {
            _PlannedDays = value;
        }
    }
    public bool AssignedVessels
    {
        set
        {
            _assignedVessels = true;
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

    public string SyncActiveVesselsOnly
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                _syncactivevesselsonly = true;
            else
                _syncactivevesselsonly = false;
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
            if (value == string.Empty || value== "Dummy")
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

    public bool VesselsOnly
    {
        set 
        {
            _vesselsonly = value;
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
        if (PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() != "OWNER")
        {
            if (_syncactivevesselsonly)
            {
                if (_assignedVessels)
                {
                    if (_selecteduser == -1)
                        ddlVessel.DataSource = PhoenixRegistersVessel.ListActiveAssignedVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(Flag));
                    else
                        ddlVessel.DataSource = PhoenixRegistersVessel.ListActiveAssignedVessel(_selecteduser, General.GetNullableInteger(Flag));
                    ddlVessel.DataBind();
                }
                else
                {
                    int vessel = 0;
                    if (_vesselsonly == true)
                        vessel = 1;
                    else
                        vessel = 0;
                    if (General.GetNullableInteger(_PlannedDays) != null && General.GetNullableInteger(_PlannedDays) != 0)
                    {
                        ddlVessel.DataSource = PhoenixRegistersVessel.ListActiveVessel(General.GetNullableInteger(Flag), General.GetNullableString(Principal), vessel, string.Empty, null, null, null, General.GetNullableInteger(_PlannedDays));
                        ddlVessel.DataBind();
                    }
                    else
                    {
                        ddlVessel.DataSource = PhoenixRegistersVessel.ListActiveVessel(General.GetNullableInteger(Flag), General.GetNullableString(Principal), vessel);
                        ddlVessel.DataBind();
                    }
                }
            }
            else
            {
                if (_assignedVessels)
                {
                    if (_selecteduser == -1)
                        ddlVessel.DataSource = PhoenixRegistersVessel.ListAssignedVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(Flag));
                    else
                        ddlVessel.DataSource = PhoenixRegistersVessel.ListAssignedVessel(_selecteduser, General.GetNullableInteger(Flag));
                    ddlVessel.DataBind();
                }
                else
                {
                    int vessel = 0;
                    if (_vesselsonly == true)
                        vessel = 1;
                    else
                        vessel = 0;
                    if (General.GetNullableInteger(_PlannedDays) != null && General.GetNullableInteger(_PlannedDays) != 0)
                    {
                        ddlVessel.DataSource = PhoenixRegistersVessel.ListVessel(General.GetNullableInteger(Flag), General.GetNullableString(Principal), vessel, string.Empty, null, null, null, General.GetNullableInteger(_PlannedDays));
                        ddlVessel.DataBind();
                    }
                    else
                    {
                        ddlVessel.DataSource = PhoenixRegistersVessel.ListVessel(General.GetNullableInteger(Flag), General.GetNullableString(Principal), vessel);
                        ddlVessel.DataBind();
                    }
                }
            }
        }
        else
        {
            ddlVessel.DataSource = PhoenixOwnersVessel.ListAssignedVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            ddlVessel.DataBind();
        }

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
        if (_appenditempresea)
            ddlVessel.Items.Insert(1, new RadComboBoxItem("SIMS Pre-Sea", "0"));
    }

    public string AppendItemPreSea
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                _appenditempresea = true;
            else
                _appenditempresea = false;

        }
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
