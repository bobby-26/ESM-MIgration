using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using System.Text;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class UserControlVesselList : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
	private bool _vesselsonly = false;
    private bool _assignedVessels = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }
    public void bind()
    {
        if (_assignedVessels)
        {
            lstVessel.DataSource = PhoenixRegistersVessel.ListAssignedVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(Flag));
            lstVessel.DataBind();
        }
        else
        {
            int vessel = 0;
            if (_vesselsonly == true)
                vessel = 1;
            else
                vessel = 0;
            lstVessel.DataSource = PhoenixRegistersVessel.ListVessel(General.GetNullableInteger(Flag), General.GetNullableString(Principal), vessel);
            lstVessel.DataBind();
        }
    }
	public bool VesselsOnly
	{
		set
		{
			_vesselsonly = value;
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
                if (item.Checked)
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
            if (value == "" || General.GetNullableString(value) == null)
            {
                lstVessel.ClearChecked();
                lstVessel.ClearSelection();
                lstVessel.SelectedIndex = -1;

            }

            if (value != null && value != "" && value != "0" && (!value.Contains(",")))
                lstVessel.SelectedValue = value;

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

    public Unit Height
    {
        set
        {
            lstVessel.Height = Unit.Parse(value.ToString());
            divVesselList.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height:" + value + ";width:" + Width);

        }
        get
        {
            return lstVessel.Height;
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
            lstVessel.Items.Insert(0, new RadListBoxItem("--Select--", "Dummy"));
    }
}
