using System;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Web.UI;

[ValidationPropertyAttribute("SelectedValue")]
public partial class UserControlMultiColumnPort : System.Web.UI.UserControl
{
    private int? EUyn;
    //public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _ItemPerRequest = 50;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }
    protected void RadMCPort_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        RadComboBox ddl = (RadComboBox)sender;
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int pageNumber = 1;
        int itemOffset = e.NumberOfItems;
        int count = 0;
        DataSet ds = new DataSet();
        if (e.NumberOfItems > 0)
        {
            pageNumber = ((int)Math.Ceiling((decimal)itemOffset / ItemsPerRequest)) + 1;
        }

        if (EUyn == 1)
        {
            ds = PhoenixCommonRegisters.EUSeaportSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null, e.Text,
                            General.GetNullableInteger(null), null, null, null,
                            pageNumber,
                            ItemsPerRequest,
                            ref iRowCount,
                            ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixCommonRegisters.SeaportSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null, e.Text,
                            General.GetNullableInteger(null), null, null, null,
                            pageNumber,
                            ItemsPerRequest,
                            ref iRowCount,
                            ref iTotalPageCount);
        }
        if (ds.Tables.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            count = dt.Rows.Count;
            e.EndOfItems = (itemOffset + count) == iRowCount;
            foreach (DataRow dr in dt.Rows)
            {
                ddl.Items.Add(new RadComboBoxItem(dr["FLDSEAPORTNAME"].ToString(), dr["FLDSEAPORTID"].ToString()));
            }
        }
        ddl.DataSource = ds;
        ddl.DataBind();
        ddl.SelectedValue = SelectedValue;
        string message = string.Empty;
        if (iRowCount <= 0)
            message = "No matches";
        else
            message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", (itemOffset + count), iRowCount);
        e.Message = message;
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
        get
        {
            return RadMCPort.Enabled;
        }
        set
        {
            RadMCPort.Enabled = value;
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
    public int ItemsPerRequest
    {
        get
        {
            return _ItemPerRequest;
        }
        set
        {
            _ItemPerRequest = value;
        }
    }
    public int? IsEUPort
    {
        set
        {
            EUyn = value;
        }
        get
        {
            return EUyn;
        }
    }

    protected void RadMCPort_TextChanged(object sender, EventArgs e)
    {
        SelectedValue = RadMCPort.SelectedValue;
    }
}


