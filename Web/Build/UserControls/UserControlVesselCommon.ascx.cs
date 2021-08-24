using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Common;


public partial class UserControlVesselCommon : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private bool _vesselsonly = false;
    private bool _assignedVessels = false;
    private int _selectedValue = -1;
    private int _selecteduser = -1;
    private bool _activevesselsonly = false;
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
            ddlVessel.Text = "";
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

    public bool VesselsOnly
    {
        set
        {
            _vesselsonly = value;
        }
    }

    public bool ActiveVesselsOnly
    {
        set
        {
            _activevesselsonly = value;
        }
    }

    public bool SyncActiveVesselsOnly
    {
        set
        {
            _syncactivevesselsonly = value;
        }
    }
    public PhoenixVesselEntityType? Entitytype
    {
        get;
        set;  
    }

    public PhoenixVesselManagementType? ManagementType
    {
        get;
        set;
    }

    public bool Enabled
    {
        set
        {
            ddlVessel.Enabled = value;
        }
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
        string vessel = "0";
        string activevesselsonly = "";
        string syncactivevesselsonly = "";
        string assignedVessels = "0";

        if (_vesselsonly == true)
            vessel = "1";

        if (_activevesselsonly == true)
            activevesselsonly = "1";

        if (_syncactivevesselsonly == true)
            syncactivevesselsonly = "1";

        if (_assignedVessels == true)
            assignedVessels = "1";
        

        ddlVessel.DataSource = PhoenixRegistersVessel.VesselListCommon(General.GetNullableByte(vessel)
                                                                        , General.GetNullableByte(activevesselsonly)
                                                                        , General.GetNullableByte(syncactivevesselsonly)
                                                                        , General.GetNullableByte(assignedVessels)
                                                                        , Entitytype
                                                                        , ManagementType);

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
    public Unit Height
    {
        get
        {
            return ddlVessel.Height;
        }
        set
        {
            ddlVessel.Height = value;
        }
    }
}