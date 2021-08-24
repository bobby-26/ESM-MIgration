using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
public partial class UserControlMultiColumnPMSRA : System.Web.UI.UserControl
{
    private string _vesselid;
    private string _status;
    private int _ItemPerRequest = 50;
    public event EventHandler TextChangedEvent;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }
    protected void gvMulticolumn_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
    {
        NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
        if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
        {
            ViewState["COMPANYID"] = nvc.Get("QMS");
        }

        RadComboBox ddl = (RadComboBox)sender;
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int pageNumber = 1;
        int itemOffset = e.NumberOfItems;
        int count = 0;
        if (e.NumberOfItems > 0)
        {
            pageNumber = ((int)Math.Ceiling((decimal)itemOffset / ItemsPerRequest)) + 1;
        }

        dt = PhoenixInspectionDailyWorkPlanActivity.DailyWorkRiskAssessmentMachineryList(General.GetNullableInteger(_vesselid)
                                                         , General.GetNullableString(_status)
                                                         , pageNumber
                                                         , ItemsPerRequest
                                                         , ref iRowCount
                                                         , ref iTotalPageCount
                                                         , null
                                                         , null
                                                         , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        count = dt.Rows.Count;
        e.EndOfItems = (itemOffset + count) == iRowCount;
        foreach (DataRow dr in dt.Rows)
        {
            ddl.Items.Add(new RadComboBoxItem(dr["FLDREFNO"].ToString(), dr["FLDRISKASSESSMENTID"].ToString()));
        }

        ddl.DataSource = dt;
        ddl.DataBind();
        string message = string.Empty;
        if (iRowCount <= 0)
            message = "No matches";
        else
            message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", (itemOffset + count), iRowCount);
        e.Message = message;

    }
    public string Vessel
    {
        set
        {
            _vesselid = value;
        }

    }
    public string Status
    {
        set
        {
            _status = value;
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
    public Unit DropDownWidth
    {
        set
        {
            gvMulticolumn.DropDownWidth = value;
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
            return gvMulticolumn.SelectedValue;
        }
        set
        {
            gvMulticolumn.SelectedValue = value;
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

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void gvMulticolumn_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }
    public ExpandDirection ExpandDirection
    {
        set
        {
            gvMulticolumn.ExpandDirection = (RadComboBoxExpandDirection)value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                gvMulticolumn.AutoPostBack = true;
        }
    }
}