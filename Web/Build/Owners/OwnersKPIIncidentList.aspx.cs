using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Owners;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class OwnersKPIIncidentList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Owners/OwnersKPIIncidentList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvInspectionIncidentSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            //toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','', '" + Session["sitepath"] + "/Inspection/InspectionDashboardOfficeIncidentNearMissListFilter.aspx')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            //toolbar.AddFontAwesomeButton("../Inspection/InspectionDashboardOfficeIncidentNearMissList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuIncidentSearch.AccessRights = this.ViewState;
            MenuIncidentSearch.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                VesselConfiguration();

                InspectionFilter.CurrentIncidentnearmissDashboardFilter = null;

                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }

                string Status = PhoenixCommonRegisters.GetHardCode(1, 168, "S1");

                DateTime now = DateTime.Now;
                string FromDate = now.Date.AddMonths(-6).ToShortDateString();
                string ToDate = DateTime.Now.ToShortDateString();

                ViewState["FROMDATE"] = FromDate.ToString();
                ViewState["TODATE"] = ToDate.ToString();
                ViewState["Status"] = Status.ToString();

                Filter.CurrentSelectedIncidentMenu = null;

                if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"] != string.Empty)
                {
                    if (Request.QueryString["callfrom"] == "ilog")
                    {
                        Filter.CurrentIncidentID = null;
                        Filter.CurrentSelectedIncidentMenu = "ilog";
                    }
                    else if (Request.QueryString["callfrom"] == "irecord")
                    {
                        Filter.CurrentIncidentID = null;
                        Filter.CurrentSelectedIncidentMenu = null;
                    }
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["SECTIONID"] = "";
                ViewState["CODE"] = "";
                ViewState["VESSELID"] = "";

                if (!string.IsNullOrEmpty(Request.QueryString["code"]))
                {
                    ViewState["CODE"] = Request.QueryString["code"];
                }

                if (!string.IsNullOrEmpty(Request.QueryString["vslid"]))
                {
                    ViewState["VESSELID"] = Request.QueryString["vslid"];
                }
                gvInspectionIncidentSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void Rebind()
    {
        gvInspectionIncidentSearch.SelectedIndexes.Clear();
        gvInspectionIncidentSearch.EditIndexes.Clear();
        gvInspectionIncidentSearch.DataSource = null;
        gvInspectionIncidentSearch.Rebind();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDINCIDENTREFNO", "FLDINCIDENTCLASSIFICATION", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDCONSEQUENCECATEGORY", "FLDINCIDENTTITLE", "FLDREPORTEDDATE", "FLDINCIDENTDATE", "FLDSTATUSOFINCIDENTNAME" };
        string[] alCaptions = { "Vessel", "Ref. No", "Type", "Category", "Subcategory", "Consequence Category", "Title", "Reported", "Incident", "Status" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = new DataSet();

        NameValueCollection nvc = FilterDashboard.OfficeDashboardFilterCriteria;
        NameValueCollection Dashboardnvc = InspectionFilter.CurrentIncidentnearmissDashboardFilter;

        ds = PhoenixOwnerReportQuality.OwnerReportKPIIncidentList(General.GetNullableInteger(Filter.SelectedOwnersReportVessel)
            , General.GetNullableDateTime(Filter.SelectedOwnersReportDate)
            , General.GetNullableString(ViewState["CODE"].ToString())
            , sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Accident/NearMissList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Accident/Near Miss List</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void IncidentSearch_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {

            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                InspectionFilter.CurrentIncidentnearmissDashboardFilter = null;
                ViewState["PAGENUMBER"] = 1;
                Rebind();
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

            string[] alColumns = { "FLDVESSELNAME", "FLDINCIDENTREFNO", "FLDINCIDENTCLASSIFICATION", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDCONSEQUENCECATEGORY", "FLDINCIDENTTITLE", "FLDREPORTEDDATE", "FLDINCIDENTDATE", "FLDSTATUSOFINCIDENTNAME" };
            string[] alCaptions = { "Vessel", "Ref. No", "Type", "Category", "Subcategory", "Consequence Category", "Title", "Reported", "Incident", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = new DataSet();

            NameValueCollection Dashboardnvc = InspectionFilter.CurrentIncidentnearmissDashboardFilter;


            ds = PhoenixOwnerReportQuality.OwnerReportKPIIncidentList(General.GetNullableInteger(Filter.SelectedOwnersReportVessel)
            , General.GetNullableDateTime(Filter.SelectedOwnersReportDate)
            , General.GetNullableString(ViewState["CODE"].ToString())
                                                                    , sortexpression, sortdirection,
                                                                    (int)ViewState["PAGENUMBER"],
                                                                    gvInspectionIncidentSearch.PageSize,
                                                                    ref iRowCount,
                                                                    ref iTotalPageCount);

            General.SetPrintOptions("gvInspectionIncidentSearch", "Accident/Near Miss List", alCaptions, alColumns, ds);

            gvInspectionIncidentSearch.DataSource = ds;
            gvInspectionIncidentSearch.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Filter.CurrentIncidentID == null)
                {
                    gvInspectionIncidentSearch.SelectedIndexes.Clear();
                    Filter.CurrentIncidentVesselID = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                    Filter.CurrentIncidentID = ds.Tables[0].Rows[0]["FLDINSPECTIONINCIDENTID"].ToString();
                    ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                }
                SetRowSelection();
            }
            else
            {
                ds.Tables[0].Rows.Clear();
                Filter.CurrentIncidentID = null;
            }
            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void gvInspectionIncidentSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string inspectionincidentid = ((RadLabel)e.Item.FindControl("lblInspectionIncidentId")).Text;
                Filter.CurrentIncidentID = null;
                BindPageURL(e.Item.ItemIndex);
                DeleteInspectionIncident(inspectionincidentid);
            }
            if (e.CommandName.ToUpper().Equals("CANCELINCIDENT"))
            {
                string inspectionincidentid = ((RadLabel)e.Item.FindControl("lblInspectionIncidentId")).Text;
                BindPageURL(e.Item.ItemIndex);
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("CORRECTIVEACTIONS"))
            {
                string inspectionincidentid = ((RadLabel)e.Item.FindControl("lblInspectionIncidentId")).Text;
                Response.Redirect("../Inspection/InspectionIncidentActionsView.aspx?inspectionincidentid=" + inspectionincidentid, false);
                BindPageURL(e.Item.ItemIndex);
            }
            if (e.CommandName.ToUpper().Equals("INCIDENTREPORT"))
            {
                BindPageURL(e.Item.ItemIndex);
            }
            if (e.CommandName.ToUpper().Equals("THIRDPARTYINCIDENTREPORT"))
            {
                BindPageURL(e.Item.ItemIndex);
            }
            if (e.CommandName.ToUpper().Equals("DIRECTORCOMMENTS"))
            {
                string inspectionincidentid = ((RadLabel)e.Item.FindControl("lblInspectionIncidentId")).Text;
                BindPageURL(e.Item.ItemIndex);
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                Filter.CurrentIncidentID = ((RadLabel)e.Item.FindControl("lblInspectionIncidentId")).Text;
                Filter.CurrentIncidentVesselID = ((RadLabel)e.Item.FindControl("lblVesselID")).Text;
            }
            if (e.CommandName.ToUpper().Equals("UNLOCK"))
            {
                if (Filter.CurrentIncidentID != null)
                {
                    PhoenixInspectionIncident.IncidentVesselUnlock(
                                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , new Guid(Filter.CurrentIncidentID)
                                                                    , 0
                                                             );
                    ucStatus.Text = "Investigation Unlocked in Vessel.";
                }
                BindPageURL(e.Item.ItemIndex);
                Rebind();
            }
            if (e.CommandName == "Page")
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
    private void SetRowSelection()
    {
        try
        {
            //gvInspectionIncidentSearch.SelectedIndexes.Clear();
            //foreach (GridDataItem item in gvInspectionIncidentSearch.Items)
            //{
            //    if (item.GetDataKeyValue("FLDINSPECTIONINCIDENTID").ToString() == Filter.CurrentIncidentID)
            //    {
            //        Filter.CurrentIncidentVesselID = (item["RefNo"].FindControl("lblVesselID") as RadLabel).Text;
            //        ViewState["DTKEY"] = (item["RefNo"].FindControl("lbldtkey") as RadLabel).Text;
            //        gvInspectionIncidentSearch.SelectedIndexes.Add(item.ItemIndex);
            //        break;
            //    }
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindPageURL(int rowindex)
    {
        try
        {
            Filter.CurrentIncidentID = ((RadLabel)gvInspectionIncidentSearch.Items[rowindex].FindControl("lblInspectionIncidentId")).Text;
            ViewState["DTKEY"] = ((RadLabel)gvInspectionIncidentSearch.Items[rowindex].FindControl("lbldtkey")).Text;
            Filter.CurrentIncidentVesselID = ((RadLabel)gvInspectionIncidentSearch.Items[rowindex].FindControl("lblVesselID")).Text;
            SetRowSelection();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void DeleteInspectionIncident(string inspectionincidentid)
    {
        PhoenixInspectionIncident.DeleteInspectionIncident(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            new Guid(inspectionincidentid));
    }
    protected void gvInspectionIncidentSearch_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                LinkButton Communication = (LinkButton)e.Item.FindControl("lnkCommunication");
                RadLabel lblVesselsid = (RadLabel)e.Item.FindControl("lblVesselID");
                RadLabel lblInspectionIncidentId = (RadLabel)e.Item.FindControl("lblInspectionIncidentId");
                if (Communication != null)
                {
                    Communication.Visible = SessionUtil.CanAccess(this.ViewState, Communication.CommandName);
                    Communication.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonCommunication.aspx?Type=INCIDENT" + "&Referenceid=" + lblInspectionIncidentId.Text + "&Vesselid=" + lblVesselsid.Text + "','large'); return true;");
                }
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;

                LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
                if (cb != null && lblInspectionIncidentId != null) cb.Attributes.Add("onclick", "openNewWindow('NAFA','', '" + Session["sitepath"] + "/Inspection/InspectionCancelReasonUpdate.aspx?DASHBOARDYN=1&REFERENCEID=" + lblInspectionIncidentId.Text + "&TYPE=2','medium'); return true;");
                if (!SessionUtil.CanAccess(this.ViewState, cb.CommandName)) cb.Visible = false;
                LinkButton cmdUnlockIncident = (LinkButton)e.Item.FindControl("cmdUnlockIncident");
                if (!SessionUtil.CanAccess(this.ViewState, cmdUnlockIncident.CommandName)) cmdUnlockIncident.Visible = false;

                DataRowView drv = (DataRowView)e.Item.DataItem;
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToString().ToUpper().Equals("OFFSHORE"))
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    LinkButton cmdClose = (LinkButton)e.Item.FindControl("cmdApprove");
                    if (drv["FLDSTATUSOFINCIDENT"].ToString().Equals(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 168, "S3"))
                        || drv["FLDSTATUSOFINCIDENT"].ToString().Equals(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 168, "S4")))
                    {
                        //cmdClose.ImageUrl = Session["images"] + "/approve.png";
                        cmdClose.Visible = SessionUtil.CanAccess(this.ViewState, cmdClose.CommandName);
                        if (cmdClose.Visible == true)
                        {
                            cmdClose.Visible = true;
                        }
                    }
                    else
                    {
                        //cmdClose.ImageUrl = Session["images"] + "/spacer.gif";
                        cmdClose.Visible = false;
                    }

                    cmdClose.Attributes.Add("onclick", "openNewWindow('NAFA','', '" + Session["sitepath"] + "/Inspection/InspectionIncidentCloseComment.aspx?DASHBOARDYN=1&REFERENCEID=" + lblInspectionIncidentId.Text + "&type=" + drv["FLDAPPROVALTYPE"].ToString() + "&user=" + drv["FLDQUALITYGENERALMANAGER"].ToString() + "," + drv["FLDQUALITYDIRECTOR"].ToString() + "," + drv["FLDQAMANAGER"].ToString() + "," + drv["FLDQUALITYINCHARGE"].ToString() + "','large'); return true;");
                }
                else
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    LinkButton cmdClose = (LinkButton)e.Item.FindControl("cmdApprove");
                    if (drv["FLDSTATUSOFINCIDENT"].ToString().Equals(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 168, "S3"))
                      || drv["FLDSTATUSOFINCIDENT"].ToString().Equals(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 168, "S4")))
                    {
                        //cmdClose.ImageUrl = Session["images"] + "/approve.png";
                        cmdClose.Visible = SessionUtil.CanAccess(this.ViewState, cmdClose.CommandName);
                        if (cmdClose.Visible == true)
                        {
                            cmdClose.Visible = true;
                        }
                    }
                    else
                    {
                        //cmdClose.ImageUrl = Session["images"] + "/spacer.gif";
                        cmdClose.Visible = false;
                    }
                    cmdClose.Attributes.Add("onclick", "openNewWindow('NAFA','', '" + Session["sitepath"] + "/Inspection/InspectionIncidentCloseComment.aspx?DASHBOARDYN=1&REFERENCEID=" + lblInspectionIncidentId.Text + "&type=" + drv["FLDAPPROVALTYPE"].ToString() + "&user=" + drv["FLDQUALITYGENERALMANAGER"].ToString() + "," + drv["FLDQUALITYDIRECTOR"].ToString() + "," + drv["FLDQAMANAGER"].ToString() + "," + drv["FLDQUALITYINCHARGE"].ToString() + "','large'); return true;");
                }

                //LinkButton cmdCar = (LinkButton)e.Item.FindControl("cmdCorrectiveActions");
                //if (cmdCar != null)
                //{
                //    cmdCar.Visible = SessionUtil.CanAccess(this.ViewState, cmdCar.CommandName);
                //    HtmlGenericControl html = new HtmlGenericControl();

                //    if (drv["FLDCARPARRECORDEDYN"].ToString() == "0")
                //    {
                //        cmdCar.Controls.Remove(html);
                //        html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-eye\"></i></span>";
                //        cmdCar.Controls.Add(html);
                //    }
                //}

                LinkButton cmdReport = (LinkButton)e.Item.FindControl("cmdReport");

                if (cmdReport != null)
                {
                    cmdReport.Attributes.Add("onclick", "openNewWindow('Report', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=INCIDENTNEARMISSREPORT&inspectionincidentid=" + lblInspectionIncidentId.Text + "&vesselid=" + drv["FLDVESSELID"].ToString() + "&showmenu=0&showexcel=NO');return true;");
                }

                if (cmdReport != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, cmdReport.CommandName)) cmdReport.Visible = false;
                }

                LinkButton cmdThirdPartyReport = (LinkButton)e.Item.FindControl("cmdThirdPartyReport");
                if (cmdThirdPartyReport != null)
                {
                    cmdThirdPartyReport.Attributes.Add("onclick", "openNewWindow('Report', '', '" + Session["sitepath"] + "/Inspection/InspectionIncidentReport.aspx?INCIDENTID=" + lblInspectionIncidentId.Text + "&VESSELID=" + drv["FLDVESSELID"].ToString() + "');return true;");
                }

                if (cmdThirdPartyReport != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, cmdThirdPartyReport.CommandName)) cmdThirdPartyReport.Visible = false;
                }

                RadLabel lblTitle = (RadLabel)e.Item.FindControl("lblTitle");
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucIncidentTitle");
                if (uct != null)
                {
                    uct.Position = ToolTipPosition.TopCenter;
                    uct.TargetControlId = lblTitle.ClientID;
                }

                LinkButton cmdDirectorComment = (LinkButton)e.Item.FindControl("imgDirectorComments");
                if (lblInspectionIncidentId != null) cmdDirectorComment.Attributes.Add("onclick", "openNewWindow('NAFA','', '" + Session["sitepath"] + "/Inspection/InspectionIncidentDirectorComment.aspx?DASHBOARDYN=1&REFERENCEID=" + lblInspectionIncidentId.Text + "','large'); return true;");
                if (drv["FLDSTATUSOFINCIDENT"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 168, "S2") && drv["FLDINCIDENTLOCKFORVESSELYN"].ToString() == "1")
                {
                    if (cmdUnlockIncident != null)
                    {
                        cmdUnlockIncident.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to Unlock this Investigation in Vessel?'); return false;");
                        cmdUnlockIncident.Visible = true;
                    }
                }

                LinkButton lnkRefno = (LinkButton)e.Item.FindControl("lnkIncidentRefNo");
                if (lnkRefno != null)
                {


                    lnkRefno.Visible = SessionUtil.CanAccess(this.ViewState, lnkRefno.CommandName);

                    lnkRefno.Attributes.Add("onclick", "javascript:openNewWindow('code1', '', '" + Session["sitepath"] + "/Inspection/InspectionIncidentList.aspx?DashboardYN=1&IncidentId=" + lblInspectionIncidentId.Text + "');");

                }

                LinkButton lnkVessel = (LinkButton)e.Item.FindControl("lnkVessel");
                RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselid");
                if (lnkVessel != null)
                {
                    lnkVessel.Attributes.Add("onclick", "javascript:top.openNewWindow('maint','Details','Dashboard/DashboardVesselDetails.aspx?vesselid=" + lblVesselid.Text + "');");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }
    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }
    protected void gvInspectionIncidentSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInspectionIncidentSearch.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvInspectionIncidentSearch_SortCommand1(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetRowSelection();
    }
}
