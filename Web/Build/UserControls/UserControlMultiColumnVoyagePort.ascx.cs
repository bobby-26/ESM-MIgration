using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using Telerik.Web.UI;

public partial class UserControlMultiColumnVoyagePort : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems = true;
    private string _selectedValue = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }        
    }

    public bool Enabled
    {
        get
        {
            return RadMCPort.Enabled;
        }
        set
        {
            RadMCPort.Enabled = value;
        }
    }
    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                RadMCPort.AutoPostBack = true;
        }
    }
    public Unit Width
    {
        get
        {
            return RadMCPort.Width;
        }
        set
        {
            RadMCPort.Width = value;
        }
    }

    public string CssClass
    {
        set
        {
            RadMCPort.CssClass = value;
        }
    }

    public string SelectedValue
    {
        get
        {
            return ViewState[RadMCPort.ClientID] != null ? ViewState[RadMCPort.ClientID].ToString() : "";
        }
        set
        {
            ViewState[RadMCPort.ClientID] = value;

        }
    }

    public string SelectedPortCallValue
    {
        get
        {
            return ViewState["PORTCALLID"] != null ? ViewState["PORTCALLID"].ToString() : "";
        }
        set
        {
            ViewState["PORTCALLID"] = value;
        }
    }

    public string Text
    {
        get
        {
            return RadMCPort.Text;
        }
        set
        {
            RadMCPort.Text = value;
        }
    }

    public string VoyageId
    {
        get
        {
            return ViewState["VOYAGEID"] != null ? ViewState["VOYAGEID"].ToString() : "";
        }
        set
        {
            ViewState["VOYAGEID"] = value;
        }
    }

    public string VesselId
    {
        get
        {
            return ViewState["VESSELID"] != null ? ViewState["VESSELID"].ToString() : "";
        }
        set
        {
            ViewState["VESSELID"] = value;
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void RadMCPort_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);

        SelectedPortCallValue = RadMCPort.SelectedValue;

            foreach (DataRow dr in ((DataTable)ViewState["DataTable"]).Rows)
            {
                if (General.GetNullableGuid(dr["FLDPORTCALLID"].ToString()) == General.GetNullableGuid(SelectedPortCallValue))
                {
                    SelectedValue = dr["FLDSEAPORTID"].ToString();
                }
            }
    }
    public string AppendDataBoundItems
    {
        set
        {
            if (value.ToUpper().Equals("FALSE"))
                _appenddatabounditems = false;
            else
                _appenddatabounditems = true;
        }
    }
    public void bind()
    {
        DataSet ds = PhoenixVesselPositionVoyageLoadDetails.ListVoyagePortDetails(
                    General.GetNullableInteger(ViewState["VESSELID"] != null ? ViewState["VESSELID"].ToString() : "")
                   , General.GetNullableGuid(ViewState["VOYAGEID"] != null ? ViewState["VOYAGEID"].ToString() : ""));

        RadMCPort.DataSource = ds;
        RadMCPort.DataBind();

        ViewState["DataTable"] = ds.Tables[0];
        foreach (RadComboBoxItem item in RadMCPort.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void RadMCPort_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            RadMCPort.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
