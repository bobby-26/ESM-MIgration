using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Web.UI;

[ValidationPropertyAttribute("SelectedValue")]
public partial class UserControlMultiColumnCity : System.Web.UI.UserControl
{
    private int _ItemPerRequest = 50;
    private bool _appenddatabounditems;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }
    protected void RadMCCity_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
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

        ds = PhoenixRegistersCity.CitySearch(e.Text, null, null, null, null, pageNumber, ItemsPerRequest, ref iRowCount, ref iTotalPageCount);
        if (ds.Tables.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            count = dt.Rows.Count;
            e.EndOfItems = (itemOffset + count) == iRowCount;
            foreach (DataRow dr in dt.Rows)
            {
                ddl.Items.Add(new RadComboBoxItem(dr["FLDCITYNAME"].ToString(), dr["FLDCITYID"].ToString()));
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
            return RadMCCity.Enabled;
        }
        set
        {
            RadMCCity.Enabled = value;
        }
    }
    public Unit Width
    {
        get
        {
            return RadMCCity.Width;
        }
        set
        {
            RadMCCity.Width = value;
        }
    }
    public string CssClass
    {
        set
        {
            RadMCCity.CssClass = value;
        }
    }
    public string SelectedValue
    {
        get
        {
            return ViewState[RadMCCity.ClientID] != null ? ViewState[RadMCCity.ClientID].ToString() : "";
        }
        set
        {
            ViewState[RadMCCity.ClientID] = value;
        }

    }
    public string Text
    {
        get
        {
            return RadMCCity.Text;
        }
        set
        {
            RadMCCity.Text = value;
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

    protected void RadMCCity_TextChanged(object sender, EventArgs e)
    {
        SelectedValue = RadMCCity.SelectedValue;
    }
    
}
