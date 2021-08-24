using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using SouthNests.Phoenix.Framework;

public partial class UserControlVesselTypeList : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
	public event EventHandler TextChangedEvent;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lstVesselType.DataSource = PhoenixRegistersVesselType.ListVesselType(0);
            lstVesselType.DataBind();
        }
    }

    public DataSet VesselTypeList
    {
        set
        {
            lstVesselType.Items.Clear();
            lstVesselType.DataSource = value;
            lstVesselType.DataBind();
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
            divVesselTypeList.Attributes["class"] = value;
            lstVesselType.CssClass = value;
        }
    }


    public bool AutoPostBack
    {
        set
        {
            lstVesselType.AutoPostBack = value;
        }
    }
    public bool Enabled
    {
        set
        {
            lstVesselType.Enabled = value;
        }
    }
    public string SelectedVesseltype
    {
        get
        {
            StringBuilder strlist = new StringBuilder();
            foreach (RadListBoxItem item in lstVesselType.Items)
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
            if (value == "")
                lstVesselType.SelectedIndex = -1;

            if (value != null && value != "" && value != "0" && (!value.Contains(",")))
                lstVesselType.SelectedValue = value;

            string strlist = "," + value + ",";
            foreach (RadListBoxItem item in lstVesselType.Items)
            {
                item.Checked = strlist.Contains("," + item.Value + ",") ? true : false;
            }            
        }
    }
    public string SelectedVesselTypeName
    {
        get
        {
            return lstVesselType.SelectedItem.Text;
        }
    }

    public string SelectedVesselTypeValue
    {
        get
        {

            return lstVesselType.SelectedValue;
        }
        set
        {
            if (value == "" || General.GetNullableString(value) == null)
            {
                lstVesselType.ClearChecked();
                lstVesselType.ClearSelection();
                lstVesselType.SelectedIndex = -1;
            }

            if (value != null && value != "" && value != "0")
                lstVesselType.SelectedValue = value;
        }
    }
    protected void lstVesselType_DataBound(object sender, EventArgs e)
    {
       
    }
    protected void OnTextChangedEvent(EventArgs e)
	{
		if (TextChangedEvent != null)
			TextChangedEvent(this, e);
	}

	protected void lstVesselType_TextChanged(object sender, EventArgs e)
	{
		OnTextChangedEvent(e);
	}
    public Unit Width
    {
        set
        {
            lstVesselType.Width = Unit.Parse(value.ToString());
            divVesselTypeList.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height: 80px; width:" + value);
        }
        get
        {
            return lstVesselType.Width;
        }
    }   
}
