using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI;

public partial class PlannedMaintenanceWorkorderApprovalPendingList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkorderApprovalPendingList.aspx?" + Request.QueryString.ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarmain.AddFontAwesomeButton("javascript:CallPrint('gvWorkorderList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            if (!IsPostBack)
            {
                ViewState["CurrentWorkorderId"] = string.Empty;
                ViewState["VerificationType"] = string.Empty;
                ViewState["vslid"] = string.Empty;
                if (Request.QueryString["VerifyType"] != null)
                    ViewState["VerificationType"] = Request.QueryString["VerifyType"].ToString();                
                gvWorkorderList.PageSize = General.ShowRecords(null);
            }
            MenuWorkOrder.Title = "Supt Verification";
            if (ViewState["VerificationType"].ToString() == "1")
                MenuWorkOrder.Title = "Vessel Verification";
            MenuWorkOrder.AccessRights = this.ViewState;
            MenuWorkOrder.MenuList = toolbarmain.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            Guid? groupId = Guid.Empty;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDVESSELNAME", "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDPLANNINGDUEDATE", "FLDDISCIPLINENAME", "FLDVESSELVERIFICATIONRANK", "FLDVESSELVERIFICATION", "FLDVESSELVERIFYREMARKS", "FLDOFFICEVERIFICATION", "FLDOFFICEVERIFYREMARKS" };
                string[] alCaptions = { "Vessel", "Work Order No.", "Title", "Planned Date", "Assigned To", "Vessel Verification Rank", "Vessel Verification", "Vessel Remarks", "Supnt Verification", "Supnt Remarks" };

                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                int? vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                if (vesselid.Value == 0)
                    vesselid = null;
                DataSet ds = PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderVerificationPendingSearch(int.Parse(ViewState["VerificationType"].ToString())
                                            , sortexpression, sortdirection
                                            , 1, iRowCount, ref iRowCount, ref iTotalPageCount
                                            , vesselid
                                            );
                string heading = "Supt Verification";
                if (ViewState["VerificationType"].ToString() == "1")
                    heading = "Vessel Verification";
                General.ShowExcel(heading, ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }               
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvWorkorderList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDVESSELNAME", "FLDWORKORDERNUMBER","FLDWORKORDERNAME", "FLDPLANNINGDUEDATE", "FLDDISCIPLINENAME", "FLDVESSELVERIFICATIONRANK", "FLDVESSELVERIFICATION", "FLDVESSELVERIFYREMARKS", "FLDOFFICEVERIFICATION", "FLDOFFICEVERIFYREMARKS" };
            string[] alCaptions = { "Vessel","Work Order No.","Title", "Planned Date", "Assigned To","Vessel Verification Rank", "Vessel Verification", "Vessel Remarks", "Supnt Verification", "Supnt Remarks" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            int? vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            if (vesselid.Value == 0)
                vesselid = null;
            DataSet ds = PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderVerificationPendingSearch(int.Parse(ViewState["VerificationType"].ToString())
                                        , sortexpression, sortdirection
                                        , gvWorkorderList.CurrentPageIndex + 1, gvWorkorderList.PageSize, ref iRowCount, ref iTotalPageCount
                                        , vesselid
                                        );
            string heading = "Supt Verification";
            if (ViewState["VerificationType"].ToString() == "1")
                heading = "Vessel Verification";
            General.SetPrintOptions("gvWorkorderList", heading, alCaptions, alColumns, ds);

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

    protected void gvWorkorderList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                ViewState["CurrentWorkorderId"] = ((RadLabel)e.Item.FindControl("lblWorkorderId")).Text;
                ViewState["vslid"] = ((RadLabel)e.Item.FindControl("lblVesselid")).Text;
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
        if (Page.IsValid)
        {
            try
            {
                PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderVerificationUpdate(new Guid(ViewState["CurrentWorkorderId"].ToString())
                                                                            , 1, txtRemarks.Text
                                                                            , int.Parse(ViewState["VerificationType"].ToString())
                                                                            , int.Parse(ViewState["vslid"].ToString()));

                txtRemarks.Text = "";
                string script = "function f(){CloseModelWindow(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            catch (Exception ex)
            {
                RequiredFieldValidator Validator = new RequiredFieldValidator();
                Validator.ErrorMessage = ex.Message;
                Validator.IsValid = false;
                Validator.Visible = false;
                Validator.ValidationGroup = "WorkOrderDetail";
                Page.Form.Controls.Add(Validator);
                //ucError.ErrorMessage = ex.Message;
                //ucError.Visible = true;
            }
        }
    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderVerificationUpdate(new Guid(ViewState["CurrentWorkorderId"].ToString())
                                                                       , 0, txtRemarks.Text
                                                                   , int.Parse(ViewState["VerificationType"].ToString())
                                                                   , int.Parse(ViewState["vslid"].ToString()));

                txtRemarks.Text = "";

                string script = "function f(){CloseModelWindow(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            catch(Exception ex)
            {
                RequiredFieldValidator Validator = new RequiredFieldValidator
                {
                    ErrorMessage = ex.Message,
                    IsValid = false,
                    Visible = false,
                    ValidationGroup = "WorkOrderDetail"
                };
                Page.Form.Controls.Add(Validator);
            }
        }
    }

    protected void gvWorkorderList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
            LinkButton approve = (LinkButton)e.Item.FindControl("lnkApprove");
            if (approve != null)
            {
                approve.Visible = SessionUtil.CanAccess(this.ViewState, approve.CommandName);
                approve.Attributes.Add("onclick", "showDialog();");
                
            }
            LinkButton lnkWo = ((LinkButton)e.Item.FindControl("lblWO"));
            if (lnkWo != null && drv != null)
            {
                lnkWo.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Work Orders','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderGroupList.aspx?wono=" + drv["FLDWORKORDERNUMBER"].ToString() + "'); return false;");
            }
        }
    }
}
