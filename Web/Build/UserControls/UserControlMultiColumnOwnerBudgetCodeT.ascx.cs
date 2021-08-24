using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControlMultiColumnOwnerBudgetCodeT : UserControl
{
    private bool _appenddatabounditems;
    private int _ItemPerRequest = 50;
    int vesselid;
    int budgetid;
    int ownerid;
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
    public string VesselId
    {
        get
        {
            return vesselid.ToString();
        }
        set
        {
            vesselid = Int32.Parse(value);
        }
    }
    public string BudgetId
    {
        get
        {
            return budgetid.ToString();
        }
        set
        {
            budgetid = Int32.Parse(value);
        }
    }
    public string OwnerId
    {
        get
        {
            return ownerid.ToString();
        }
        set
        {
            ownerid = Int32.Parse(value);
        }
    }
    protected void gvMulticolumn_TextChanged(object sender, EventArgs e)
    {
        SelectedValue = gvMulticolumn.SelectedValue;
    }
    protected void gvMulticolumn_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {

        RadComboBox ddl = (RadComboBox)sender;
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? iownerid = 0;
        int pageNumber = 1;
        int itemOffset = e.NumberOfItems;
        int count = 0;
        DataSet ds = new DataSet();
        if (e.NumberOfItems > 0)
        {
            pageNumber = ((int)Math.Ceiling((decimal)itemOffset / ItemsPerRequest)) + 1;
        }

        //ds = PhoenixInspectionIncident.IncidentsMultiColumnSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
        //                        , e.Text
        //                        , null
        //                        , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
        //                        , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString())
        //                        , null
        //                        , null, null
        //                        , pageNumber
        //                        , ItemsPerRequest
        //                        , ref iRowCount
        //                        , ref iTotalPageCount);
        ds = PhoenixCommonRegisters.InternalBillingOwnerBudgetCodeSearch(
                   e.Text
                 , null
                 , General.GetNullableInteger(ownerid.ToString()), General.GetNullableInteger(VesselId.ToString())
                 , General.GetNullableInteger(BudgetId.ToString())
                 , null, null
                 , pageNumber
                 , ItemsPerRequest
                 , ref iRowCount
                 , ref iTotalPageCount
                 , ref iownerid);

        if (ds.Tables.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            count = dt.Rows.Count;
            e.EndOfItems = (itemOffset + count) == iRowCount;
            foreach (DataRow dr in dt.Rows)
            {
                ddl.Items.Add(new RadComboBoxItem(dr["FLDOWNERBUDGETCODE"].ToString(), dr["FLDOWNERBUDGETCODEID"].ToString()));
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
}