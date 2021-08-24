using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

[ValidationPropertyAttribute("SelectedValue")]
public partial class UserControlMultiColumnInspection : UserControl
{
    private bool _appenddatabounditems;
    private int _ItemPerRequest = 50;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
        }
    }
    public bool Enabled
    {
        get
        {
            return gvMulticolumn.Enabled;
        }
        set
        {
            gvMulticolumn.Enabled = value;
        }
    }
    public Unit Width
    {
        get
        {
            return gvMulticolumn.Width;
        }
        set
        {
            gvMulticolumn.Width = value;

        }
    }
    public string CssClass
    {
        set
        {
            gvMulticolumn.CssClass = value;
        }
    }
    public string SelectedValue
    {
        get
        {
            return ViewState[gvMulticolumn.ClientID] != null ? ViewState[gvMulticolumn.ClientID].ToString() : "";
        }
        set
        {
            ViewState[gvMulticolumn.ClientID] = value;
        }

    }
    public string Text
    {
        get
        {
            return gvMulticolumn.Text;
        }
        set
        {
            gvMulticolumn.Text = value;
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
    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;
        }
    }
    protected void gvMulticolumn_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
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

        ds = PhoenixInspectionAuditSchedule.AuditScheduleMultiColumnSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , e.Text
                            , null
                            , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                            , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString())
                            , null
                            , null, null
                            , pageNumber
                            , ItemsPerRequest
                            , ref iRowCount
                            , ref iTotalPageCount);

        if (ds.Tables.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            count = dt.Rows.Count;
            e.EndOfItems = (itemOffset + count) == iRowCount;
            foreach (DataRow dr in dt.Rows)
            {
                ddl.Items.Add(new RadComboBoxItem(dr["FLDAUDITSCHEDULENAME"].ToString(), dr["FLDREVIEWSCHEDULEID"].ToString()));
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

    protected void gvMulticolumn_TextChanged(object sender, EventArgs e)
    {
        SelectedValue = gvMulticolumn.SelectedValue;
    }
}
