using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using System.Text;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using Telerik.Web.UI;
public partial class UserControlVesselCheckBoxList : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private bool _vesselsonly = false;
    private bool _assignedVessels = false;
    private bool _activevessels = true;
    protected override void Render(HtmlTextWriter writer)
    {
        if (_appenddatabounditems && lstVessel.Items.Count > 0)
        {
            lstVessel.Items[0].Attributes["onclick"] = "checkUnchekAll(this);";
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }
    public void bind()
    {
        int vessel = 0, activevessel = 0;

        if (_vesselsonly == true)
            vessel = 1;
        else
            vessel = 0;
        if (_activevessels == true)
            activevessel = 1;
        lstVessel.DataSource = PhoenixRegistersVessel.ListCommonVessels(General.GetNullableInteger(Flag)
                                                                        , null
                                                                        , null
                                                                        , null
                                                                        , vessel
                                                                        , General.GetNullableString(Principal)
                                                                        , null
                                                                        , null
                                                                        , activevessel
                                                                        , General.GetNullableString(EntityType));
        lstVessel.DataBind();
    }
    public bool VesselsOnly
    {
        set
        {
            _vesselsonly = value;
        }
    }
    public bool AssignedVessel
    {
        set
        {
            _assignedVessels = value;
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
          
            divVesselList.Attributes["class"] = value;
            lstVessel.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            lstVessel.AutoPostBack = value;
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
    public string SelectedVessel
    {
        get
        {
            StringBuilder strlist = new StringBuilder();
            foreach (RadListBoxItem item in lstVessel.Items)
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
            string strlist = "," + value + ",";
            foreach (RadListBoxItem item in lstVessel.Items)
            {
                item.Checked = strlist.Contains("," + item.Value + ",") ? true : false;
            }
        }
    }
    public Unit Width
    {
        set
        {
            lstVessel.Width = Unit.Parse(value.ToString());
            divVesselList.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height: 80px; width:" + value);
        }
        get
        {
            return lstVessel.Width;
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

    protected void lstVessel_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
        {
            //RadListBoxItem li = new RadListBoxItem("--Check All--", "0");
            //li.Attributes.Add("onclick", "checkUnchekAll(this);");
            //lstVessel.Items.Insert(0, li);
        }
    }
    public bool ActiveVessels
    {
        set
        {
            _activevessels = true;
        }
    }
    public string EntityType
    {
        get;
        set;
    }
}
