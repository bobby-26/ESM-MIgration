using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using System.Web.UI.WebControls;

public partial class PlannedMaintenanceGlobalRoutineWorkorderDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            ViewState["ROUTINEWOID"] = Request.QueryString["ROUTINEWOID"];
            ViewState["Title"] = Request.QueryString["Title"];
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvWorkorderDetails.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddFontAwesomeButton("javascript:openNewWindow('List','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceGlobalComponentJob.aspx?ROUTINEWOID=" + ViewState["ROUTINEWOID"].ToString() + "');", "Global Component", "<i class=\"fas fa-plus-circle\"></i>", "CREATE");
        MenuRoutineWorkorderDetails.Title = ViewState["Title"].ToString();
        MenuRoutineWorkorderDetails.AccessRights = this.ViewState;
        MenuRoutineWorkorderDetails.MenuList = toolbar.Show();
    }

    protected void gvWorkorderDetails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvWorkorderDetails.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixPlannedMaintenanceGlobalRoutineWorkorder.GlobalRoutineWorkorderDetailssearch(
                    General.GetNullableGuid(ViewState["ROUTINEWOID"].ToString()),
                    null, null,
                  sortexpression, sortdirection,
                  gvWorkorderDetails.CurrentPageIndex + 1,
                  gvWorkorderDetails.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

            gvWorkorderDetails.DataSource = ds;
            gvWorkorderDetails.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvWorkorderDetails.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixPlannedMaintenanceGlobalRoutineWorkorder.GlobalRoutineWorkorderDetailsDelete(General.GetNullableGuid(ViewState["DETAILID"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvWorkorderDetails.SelectedIndexes.Clear();
        gvWorkorderDetails.EditIndexes.Clear();
        gvWorkorderDetails.DataSource = null;
        gvWorkorderDetails.Rebind();
    }


    protected void MenuRoutineWorkorderDetails_TabStripCommand(object sender, EventArgs e)
    {

    }

    protected void gvWorkorderDetails_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["DETAILID"] = ((RadLabel)e.Item.FindControl("lblDetailId")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}