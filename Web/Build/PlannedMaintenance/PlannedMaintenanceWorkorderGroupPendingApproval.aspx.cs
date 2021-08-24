using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using System.Web.UI.WebControls;

public partial class PlannedMaintenanceWorkorderGroupPendingApproval : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            if (!IsPostBack)
            {
                ViewState["CurrentWorkorderId"] = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDJOBCATEGORY", "FLDPLANNINGDUEDATE", "FLDDURATION", "FLDDISCIPLINENAME", "FLDSTATUS" };
            string[] alCaptions = { "Work Order No.", "Category", "Planned Date", "Duration", "Assigned To", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds;

            ds = PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderGroupApprovalPendingList(null, null, null,
                                        sortexpression, sortdirection,
                                        gvWorkorderList.CurrentPageIndex + 1, gvWorkorderList.PageSize, ref iRowCount, ref iTotalPageCount
                                        );

            General.SetPrintOptions("gvWorkorderList", "Work Order", alCaptions, alColumns, ds);

            gvWorkorderList.DataSource = ds;
            gvWorkorderList.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvJhaList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvWorkorderList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                ViewState["CurrentWorkorderId"] = ((RadLabel)e.Item.FindControl("lblWorkorderId")).Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderGroupApprovalUpdate(new Guid(ViewState["CurrentWorkorderId"].ToString()),1,txtRemarks.Text);
    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderGroupApprovalUpdate(new Guid(ViewState["CurrentWorkorderId"].ToString()), 0, txtRemarks.Text);
        }
    }

    protected void gvWorkorderList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if(e.Item is GridDataItem)
        {
            LinkButton approve = (LinkButton)e.Item.FindControl("lnkApprove");
            if (approve != null)
            {
                approve.Visible = SessionUtil.CanAccess(this.ViewState, approve.CommandName);
                approve.Attributes.Add("onclick", "showDialog();");
                
            }
        }
    }
}
