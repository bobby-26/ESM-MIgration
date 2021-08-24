using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControls_UserControlVesselCommonCheckBoxList : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private bool _vesselsonly = false;
    private bool _assignedVessels = false;
    private string _selectedValue;
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
            lstVessel.Items.Clear();
            lstVessel.DataSource = value;
            lstVessel.DataBind();
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
    {        set
        {
            divVesselList.Attributes["class"] = value;
            lstVessel.CssClass = value;            
        }
        get { return lstVessel.CssClass; }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                lstVessel.AutoPostBack = true;
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
    public PhoenixVesselEntityType Entitytype
    {
        get;
        set;
    }

    public PhoenixVesselManagementType ManagementType
    {
        get;
        set;
    }
    public Unit Width
    {
        get
        {
            return lstVessel.Width;
        }
        set
        {
            lstVessel.Width = value;

        }
    }
    public Unit Height
    {
        get
        {
            return lstVessel.Height;
        }
        set
        {
            lstVessel.Height = value;
        }
    }

    public bool Enabled
    {
        set
        {
            lstVessel.Enabled = value;
        }
    }
    public string SelectedVesselValue
    {
        get
        {
            return lstVessel.SelectedValue;
        }
        set
        {
            if (value == "")
                lstVessel.SelectedIndex = -1;
            if (value != null && value != "" && value != "0")
                lstVessel.SelectedValue = value;
        }
    }

    public string SelectedValue
    {
        get
        {
            return lstVessel.SelectedValue;
        }
        set
        {
            if (value == string.Empty || value == "Dummy")
            {
                //lstVessel.SelectedIndex = -1;
                lstVessel.ClearSelection();
                //ddlVessel.Text = string.Empty;
                return;
            }
            _selectedValue = value;
            lstVessel.SelectedIndex = -1;
            foreach (RadListBoxItem item in lstVessel.Items)
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
        lstVessel.DataSource = PhoenixRegistersVessel.VesselListCommon(General.GetNullableByte(vessel)
                                                                        , General.GetNullableByte(activevesselsonly)
                                                                        , General.GetNullableByte(syncactivevesselsonly)
                                                                        , General.GetNullableByte(assignedVessels)
                                                                        , PhoenixVesselEntityType.VSL
                                                                        , PhoenixVesselManagementType.FUL);

        lstVessel.DataBind();

        //foreach (RadListBoxItem item in lstVessel.Items)
        //{
        //    if (item.Value == _selectedValue.ToString())
        //    {
        //        item.Selected = true;
        //        break;
        //    }
        //}

    }
    public string SelectedVesselName
    {
        get
        {
            return lstVessel.SelectedItem.Text;
        }
    }

    public string SelectedVessel
    {
        get
        {
            StringBuilder strlist = new StringBuilder();
            foreach (RadListBoxItem item in lstVessel.CheckedItems)
            {
                if (item.Checked && item.Value.Trim() != "")
                {

                    strlist.Append(item.Value.ToString());
                    strlist.Append(",");
                }

            }
            if (strlist.Length > 1)
            {
                strlist.Remove(strlist.Length - 1, 1);
            }
            return strlist.ToString();
        }
        set
        {
            _selectedValue = value;
            string strlist = "," + _selectedValue + ",";
            foreach (RadListBoxItem item in lstVessel.Items)
            {
                item.Checked = strlist.Contains("," + item.Value + ",") ? true : false;
            }
        }
    }

    protected void lstVessel_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
        {
            //RadListBoxItem li = new RadListBoxItem("--Check All--", "0");
            //li.Attributes.Add("onclick", "checkUnchekAll(this);");
            //lstVessel.Items.Insert(0, li);
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }
    protected void lstVessel_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }
}