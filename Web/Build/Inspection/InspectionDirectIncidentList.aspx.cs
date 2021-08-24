using System;
using System.Data;
using System.Web.UI;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionDirectIncidentList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridDataItem r in gvDirectIncident.Items)
        {

            Page.ClientScript.RegisterForEventValidation(gvDirectIncident.UniqueID, "Edit$" + r.RowIndex.ToString());

        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
            MenuRegistersInspectionGeneral.AccessRights = this.ViewState;
            // MenuRegistersInspectionGeneral.MenuList = toolbarmain.Show();
            MenuRegistersInspectionGeneral.Visible = false;
            //MenuRegistersInspectionGeneral.SetTrigger(pnlInspectionEntry);
            if (Request.QueryString["category"] != null)
            {
                ViewState["categoryid"] = Request.QueryString["category"].ToString();
                MenuRegistersInspectionGeneral.Title = "Crew Complaints";
            }
            else
            {
                ViewState["categoryid"] = "2";
                MenuRegistersInspectionGeneral.Title = "Open Reports";
            }
            if (!IsPostBack)
            {


                VesselConfiguration();

                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }

                DateTime now = DateTime.Now;
                string FromDate = now.Date.AddMonths(-6).ToShortDateString();
                string ToDate = DateTime.Now.ToShortDateString();
                string Status = PhoenixCommonRegisters.GetHardCode(1, 243, "UNC");

                ViewState["FROMDATE"] = FromDate.ToString();
                ViewState["TODATE"] = ToDate.ToString();
                ViewState["Status"] = Status.ToString();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                gvDirectIncident.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();            
            toolbar.AddFontAwesomeButton("../Inspection/InspectionDirectIncidentList.aspx?category=" + ViewState["categoryid"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDirectIncident')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");            
            if (ViewState["categoryid"].ToString() == "3")
            {
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionCrewComplaintsFilter.aspx')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            }
            else
            {
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionOpenReportsFilter.aspx')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            }
            toolbar.AddFontAwesomeButton("../Inspection/InspectionDirectIncidentList.aspx?category=" + ViewState["categoryid"].ToString(), "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegistersInspection.AccessRights = this.ViewState;
            MenuRegistersInspection.MenuList = toolbar.Show();
            // MenuRegistersInspection.SetTrigger(pnlInspectionEntry);     


        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? vesselid = -1;

        string[] alColumns = new string[] { };
        string[] alCaptions = new string[] { };
        string doctitile = "";

        if (ViewState["categoryid"].ToString() == "3")
        {
            alColumns = new string[]{ "FLDVESSELNAME", "FLDOPENREPORTREFNO", "FLDREPORTEDDATE", "FLDSUMMARY", "FLDREVIEWCATEGORYNAME", "FLDREMARKS",
                                     "FLDEVIDENCEREQUIREDYN", "FLDDUEDATE", "FLDCOMPLETIONDATE", "FLDSTATUS" ,"FLDACTIONTOBETAKEN","FLDCREWCLOSEOUTREMARKS"};
            alCaptions = new string[]{ "Vessel", "Reference Number", "Reported", "Details", "Crew Review Category", "Remarks",
                                      "Evidence Required", "Due", "Completion", "Status", "Action to be Taken", "Close out Remarks" };
            doctitile = "Crew Complaints";
        }
        else if (ViewState["categoryid"].ToString() == "2")
        {
            alColumns = new string[]{ "FLDVESSELNAME", "FLDOPENREPORTREFNO", "FLDREPORTEDDATE", "FLDSUMMARY", "FLDREVIEWCATEGORYNAME", "FLDREMARKS",
                                 "FLDEVIDENCEREQUIREDYN", "FLDDEPARTMENTNAME", "FLDDUEDATE", "FLDCOMPLETIONDATE", "FLDREFERENCENUMBER", "FLDSTATUSNAME" };
            alCaptions = new string[]{ "Vessel/Office", "Reference Number", "Reported", "Details", "Review Category", "Remarks",
                                  "Evidence Required", "Assigned to", "Due", "Completion", "Reference number", "Status" };
            doctitile = "Open Reports";
        }

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentOpenReportsFilter;
        if (nvc != null)
            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel").ToString()) == null) ? null : General.GetNullableInteger(nvc.Get("ucVessel").ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        DataSet ds = new DataSet();

        if (ViewState["categoryid"].ToString() == "3")
        {
            ds = PhoenixInspectionIncident.CrewComplaintsSearch(
                                                                General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : ViewState["FROMDATE"].ToString())
                                                                , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : ViewState["TODATE"].ToString())
                                                                , vesselid
                                                                , sortexpression
                                                                , sortdirection
                                                                , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                , gvDirectIncident.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , General.GetNullableInteger(nvc != null ? nvc["ucReviewCategory"] : null)
                                                                , General.GetNullableInteger(nvc != null ? nvc["ucReviewSubcategory"] : null)
                                                                , General.GetNullableInteger(nvc != null ? nvc["ucStatusofCC"] : ViewState["Status"].ToString())
                                                                , General.GetNullableInteger(nvc != null ? nvc["ucTechFleet"] : null)
                                                                , General.GetNullableInteger(nvc != null ? nvc["ucAddrOwner"] : null)
                                                                , General.GetNullableInteger(nvc != null ? nvc["ucVesselType"] : null)
                                                                , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                                                                );
        }
        else
        {
            ds = PhoenixInspectionIncident.DirectIncidentSearch(
                                                                        General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : ViewState["FROMDATE"].ToString())
                                                                        , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : ViewState["TODATE"].ToString())
                                                                        , vesselid
                                                                        , sortexpression
                                                                        , sortdirection
                                                                        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                        , gvDirectIncident.PageSize
                                                                        , ref iRowCount
                                                                        , ref iTotalPageCount
                                                                        , General.GetNullableInteger(nvc != null ? nvc["ucReviewCategory"] : null)
                                                                        , General.GetNullableInteger(nvc != null ? nvc["ucReviewSubcategory"] : null)
                                                                        , General.GetNullableInteger(nvc != null ? nvc["ddlStatus"] : ViewState["Status"].ToString())
                                                                        , int.Parse(ViewState["categoryid"].ToString())
                                                                        , General.GetNullableInteger(nvc != null ? nvc["ucTechFleet"] : null)
                                                                        , General.GetNullableInteger(nvc != null ? nvc["ucAddrOwner"] : null)
                                                                        , General.GetNullableInteger(nvc != null ? nvc["ucVesselType"] : null)
                                                                        , General.GetNullableInteger(nvc != null ? nvc["ucDept"] : null)
                                                                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                                                                        , General.GetNullableInteger(nvc != null ? nvc["ucCompany"] : null)
                                                                        );
        }

        General.SetPrintOptions("gvDirectIncident", doctitile, alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDirectIncident.DataSource = ds;

            if (ViewState["directincidentid"] == null)
            {
                ViewState["directincidentid"] = ds.Tables[0].Rows[0]["FLDDIRECTINCIDENTID"].ToString();
                // gvDirectIncident.SelectedIndex = 0;
            }

        }
        else
        {
            gvDirectIncident.DataSource = ds;
        }

        gvDirectIncident.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? vesselid = -1;

        string[] alColumns = new string[] { };
        string[] alCaptions = new string[] { };
        string doctitile = "";

        if (ViewState["categoryid"].ToString() == "3")
        {
            alColumns = new string[]{ "FLDVESSELNAME", "FLDOPENREPORTREFNO", "FLDREPORTEDDATE", "FLDSUMMARY", "FLDREVIEWCATEGORYNAME", "FLDREMARKS",
                                     "FLDEVIDENCEREQUIREDYN", "FLDDUEDATE", "FLDCOMPLETIONDATE", "FLDSTATUS","FLDACTIONTOBETAKEN","FLDCREWCLOSEOUTREMARKS" };
            alCaptions = new string[]{ "Vessel", "Reference Number", "Reported", "Details", "Crew Review Category", "Remarks",
                                      "Evidence Required", "Due", "Completion", "Status", "Action to be Taken", "Close out Remarks" };
            doctitile = "Crew Complaints";
        }
        else if (ViewState["categoryid"].ToString() == "2")
        {
            alColumns = new string[]{ "FLDVESSELNAME", "FLDOPENREPORTREFNO", "FLDREPORTEDDATE", "FLDSUMMARY", "FLDREVIEWCATEGORYNAME", "FLDREMARKS",
                                 "FLDEVIDENCEREQUIREDYN", "FLDDEPARTMENTNAME", "FLDDUEDATE", "FLDCOMPLETIONDATE", "FLDREFERENCENUMBER", "FLDSTATUS" };
            alCaptions = new string[]{ "Vessel/Office", "Reference Number", "Reported Date", "Details", "Review Category", "Remarks",
                                  "Evidence Required", "Assigned to", "Due", "Completion", "Reference number", "Status" };
            doctitile = "Open Reports";
        }

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentOpenReportsFilter;
        if (nvc != null)
            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel").ToString()) == null) ? null : General.GetNullableInteger(nvc.Get("ucVessel").ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        DataSet ds = new DataSet();
        if (ViewState["categoryid"].ToString() == "3")
        {
            ds = PhoenixInspectionIncident.CrewComplaintsSearch(
                                                                General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : ViewState["FROMDATE"].ToString())
                                                                , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : ViewState["TODATE"].ToString())
                                                                , vesselid
                                                                , sortexpression
                                                                , sortdirection
                                                                , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                , iRowCount
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , General.GetNullableInteger(nvc != null ? nvc["ucReviewCategory"] : null)
                                                                , General.GetNullableInteger(nvc != null ? nvc["ucReviewSubcategory"] : null)
                                                                , General.GetNullableInteger(nvc != null ? nvc["ucStatusofCC"] : ViewState["Status"].ToString())
                                                                , General.GetNullableInteger(nvc != null ? nvc["ucTechFleet"] : null)
                                                                , General.GetNullableInteger(nvc != null ? nvc["ucAddrOwner"] : null)
                                                                , General.GetNullableInteger(nvc != null ? nvc["ucVesselType"] : null)
                                                                , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                                                                );
        }
        else
        {
            ds = PhoenixInspectionIncident.DirectIncidentSearch(
                                                                        General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : ViewState["FROMDATE"].ToString())
                                                                        , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : ViewState["TODATE"].ToString())
                                                                        , vesselid
                                                                        , sortexpression
                                                                        , sortdirection
                                                                        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                        , iRowCount
                                                                        , ref iRowCount
                                                                        , ref iTotalPageCount
                                                                        , General.GetNullableInteger(nvc != null ? nvc["ucReviewCategory"] : null)
                                                                        , General.GetNullableInteger(nvc != null ? nvc["ucReviewSubcategory"] : null)
                                                                        , General.GetNullableInteger(nvc != null ? nvc["ddlStatus"] : ViewState["Status"].ToString())
                                                                        , int.Parse(ViewState["categoryid"].ToString())
                                                                        , General.GetNullableInteger(nvc != null ? nvc["ucTechFleet"] : null)
                                                                        , General.GetNullableInteger(nvc != null ? nvc["ucAddrOwner"] : null)
                                                                        , General.GetNullableInteger(nvc != null ? nvc["ucVesselType"] : null)
                                                                        , General.GetNullableInteger(nvc != null ? nvc["ucDept"] : null)
                                                                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                                                                        , General.GetNullableInteger(nvc != null ? nvc["ucCompany"] : null)
                                                                        );
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=OpenReportList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>" + doctitile + "</h3></td>");
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
    protected void MenuRegistersInspectionGeneral_TabStripCommand(object sender, EventArgs e)
    {

        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                if (ViewState["categoryid"].ToString() == "3")
                {
                    Response.Redirect("../Inspection/InspectionCrewComplaintsFilter.aspx");
                }
                else
                {
                    Response.Redirect("../Inspection/InspectionOpenReportsFilter.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RegistersInspection_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentOpenReportsFilter = null;
                gvDirectIncident.Rebind();
            }

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
            RadLabel lblDirectIncidentId = (RadLabel)gvDirectIncident.Items[rowindex].FindControl("lblDirectIncidentId");
            if (lblDirectIncidentId != null)
            {
                ViewState["directincidentid"] = lblDirectIncidentId.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

    }




    private bool IsValidInspection(string inspectiontypeid, string inspectiontypecategoryid, string inspectionname, string shortcode, string effectivedate, string externalaudittype, string officeyn, string companyid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (inspectiontypeid.Trim().Equals("Dummy") || inspectiontypeid.Trim().Equals(""))
            ucError.ErrorMessage = "Type is required.";
        if (inspectiontypecategoryid.Trim().Equals("Dummy") || inspectiontypecategoryid.Trim().Equals(""))
            ucError.ErrorMessage = "Category is required.";

        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvDirectIncident.Rebind();
    }

    protected void gvDirectIncident_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow
        //    && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
        //    && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        //{
        //    e.Row.TabIndex = -1;
        //    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvDirectIncident, "Edit$" + e.Row.RowIndex.ToString(), false);
        //}
    }


    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {

        gvDirectIncident.Rebind();

    }


    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvDirectIncident_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDirectIncident.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDirectIncident_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvDirectIncident_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;


            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                RadLabel lblDirectIncidentId = (RadLabel)e.Item.FindControl("lblDirectIncidentId");
                int nCurrentRow = e.Item.ItemIndex;
                BindPageURL(nCurrentRow);
                if (lblDirectIncidentId != null)
                    ViewState["directincidentid"] = lblDirectIncidentId.Text;
                if (ViewState["categoryid"].ToString() == "2")
                    Response.Redirect("../Inspection/InspectionDirectIncidentGeneral.aspx?directincidentid=" + ViewState["directincidentid"], false);
                else
                    Response.Redirect("../Inspection/InspectionCrewComplaintsEdit.aspx?directincidentid=" + ViewState["directincidentid"], false);
                /*ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?directincidentid=" + ViewState["directincidentid"];*/
            }
            if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                gvDirectIncident.Rebind();
                //int nCurrentRow = e.Item.ItemIndex;
                //BindPageURL(nCurrentRow);
            }
            if (e.CommandName.ToUpper().Equals("CANCELACTION"))
            {
                int nCurrentRow = e.Item.ItemIndex;
                BindPageURL(nCurrentRow);

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

    protected void gvDirectIncident_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblRemarks = (RadLabel)e.Item.FindControl("lblRemarks");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipRemarks");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (uct != null)
            {
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lblRemarks.ClientID;
            }

            RadLabel lblSummary = (RadLabel)e.Item.FindControl("lblSummaryFirstLine");
            UserControlToolTip uctt = (UserControlToolTip)e.Item.FindControl("ucToolTipSummary");

            if (uctt != null)
            {
                uctt.Position = ToolTipPosition.TopCenter;
                uctt.TargetControlId = lblSummary.ClientID;
            }

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselid");
            
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                HtmlGenericControl html = new HtmlGenericControl();
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                else
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-paperclip\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.QUALITY + "&type=OPENREPORT&cmdname=OPENREPORTUPLOAD&VESSELID=" + lblVesselid.Text + "'); return true;");
            }

            LinkButton cmdCancel = (LinkButton)e.Item.FindControl("cmdCancel");
            RadLabel lblDirectIncidentId = (RadLabel)e.Item.FindControl("lblDirectIncidentId");
            if (cmdCancel != null && lblDirectIncidentId != null)
            {
                if (ViewState["categoryid"].ToString() == "2")
                    cmdCancel.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Inspection/InspectionCancelReasonUpdate.aspx?REFERENCEID=" + lblDirectIncidentId.Text + "&TYPE=5','medium'); return true;");
                else if (ViewState["categoryid"].ToString() == "3")
                    cmdCancel.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Inspection/InspectionCancelReasonUpdate.aspx?REFERENCEID=" + lblDirectIncidentId.Text + "&TYPE=6','medium'); return true;");
            }

            if (ViewState["categoryid"] != null && ViewState["categoryid"].ToString() == "2")
            {
                gvDirectIncident.Columns[11].Visible = true;
                gvDirectIncident.Columns[8].Visible = true;
            }
            else
            {
                gvDirectIncident.Columns[11].Visible = false;
                gvDirectIncident.Columns[8].Visible = false;
            }

            if (cmdCancel != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName))
                    cmdCancel.Visible = false;
            }
        }
    }
}
