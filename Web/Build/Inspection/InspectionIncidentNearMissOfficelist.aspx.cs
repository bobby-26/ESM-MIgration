using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class InspectionIncidentNearMissOfficelist : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionIncidentAdd.aspx')", "Add New", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionIncidentNearMissOfficelist.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionIncidentFilter.aspx?&OFFICEYN=1')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvInspectionIncidentSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionIncidentNearMissOfficelist.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuIncidentSearch.AccessRights = this.ViewState;
            MenuIncidentSearch.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                VesselConfiguration();

                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }

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

                ViewState["Status"] = PhoenixCommonRegisters.GetHardCode(1, 168, "S1");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["SECTIONID"] = "";
                gvInspectionIncidentSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            toolbar = new PhoenixToolbar();
            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
            {
                toolbar.AddButton("Office Incident", "OFFICELIST", ToolBarDirection.Right);
            }
            toolbar.AddButton("Incident", "LIST", ToolBarDirection.Right);
            MenuIncidentGeneral.AccessRights = this.ViewState;
            MenuIncidentGeneral.MenuList = toolbar.Show();
            MenuIncidentGeneral.SelectedMenuIndex = 0;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void IncidentGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            Response.Redirect("../Inspection/InspectionIncidentFilter.aspx");
        }
        if (CommandName.ToUpper().Equals("LIST"))
        {
            Response.Redirect("../Inspection/InspectionIncidentNearMissList.aspx?callfrom=irecord", false);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? vesselid = null;

        string[] alColumns = { "FLDVESSELNAME", "FLDINCIDENTREFNO", "FLDINCIDENTCLASSIFICATION", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDCONSEQUENCECATEGORY", "FLDINCIDENTTITLE", "FLDREPORTEDDATE", "FLDINCIDENTDATE", "FLDSTATUSOFINCIDENTNAME" };
        string[] alCaptions = { "Vessel", "Reference Number", "Classification", "Category", "Subcategory", "Consequence Category", "Title", "Reported Date", "Incident Date", "Status" };

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

        NameValueCollection nvc = Filter.CurrentIncidentFilterCriteria;

        if (nvc != null)
            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel").ToString()) == null) ? null : General.GetNullableInteger(nvc.Get("ucVessel").ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        vesselid = 0;

        ds = PhoenixInspectionIncident.InspectionIncidentSearch(
              nvc != null ? General.GetNullableString(nvc.Get("txtRefNo")) : string.Empty
            , nvc != null ? General.GetNullableString(nvc.Get("txtTitle")) : string.Empty
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucStatus")) : General.GetNullableInteger(ViewState["Status"].ToString())
            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFromDate")) : null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtToDate")) : null
            , vesselid
            , sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucTechFleet")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucConsequenceCategory")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucPotentialCategory")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucActivity")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ddlIncidentNearmiss")) : null
            , nvc != null ? General.GetNullableGuid(nvc.Get("ucCategory")) : null
            , nvc != null ? General.GetNullableGuid(nvc.Get("ucSubCategory")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucVesselType")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucAddressType")) : null
            , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
            , 1
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucCompany")) : null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("ucReportedDateFrom")) : null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("ucReportedDateTo")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("chkContractedRelatedIncidentYN")) : null
            );

        Response.AddHeader("Content-Disposition", "attachment; filename=IncidentList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Incident List</h3></td>");
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
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            Filter.CurrentIncidentFilterCriteria = null;
            Rebind();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? vesselid = null;

        string[] alColumns = { "FLDVESSELNAME", "FLDINCIDENTREFNO", "FLDINCIDENTCLASSIFICATION", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDCONSEQUENCECATEGORY", "FLDINCIDENTTITLE", "FLDREPORTEDDATE", "FLDINCIDENTDATE", "FLDSTATUSOFINCIDENTNAME" };
        string[] alCaptions = { "Vessel", "Reference Number", "Classification", "Category", "Subcategory", "Consequence Category", "Title", "Reported Date", "Incident Date", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        NameValueCollection nvc = Filter.CurrentIncidentFilterCriteria;

        if (nvc != null)
            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel").ToString()) == null) ? null : General.GetNullableInteger(nvc.Get("ucVessel").ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        vesselid = 0;

        ds = PhoenixInspectionIncident.InspectionIncidentSearch(
              nvc != null ? General.GetNullableString(nvc.Get("txtRefNo")) : string.Empty
            , nvc != null ? General.GetNullableString(nvc.Get("txtTitle")) : string.Empty
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucStatus")) : General.GetNullableInteger(ViewState["Status"].ToString())
            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFromDate")) : null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtToDate")) : null
            , vesselid
            , sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvInspectionIncidentSearch.PageSize,
            ref iRowCount,
            ref iTotalPageCount
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucTechFleet")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucConsequenceCategory")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucPotentialCategory")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucActivity")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ddlIncidentNearmiss")) : null
            , nvc != null ? General.GetNullableGuid(nvc.Get("ucCategory")) : null
            , nvc != null ? General.GetNullableGuid(nvc.Get("ucSubCategory")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucVesselType")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucAddressType")) : null
            , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
            , 1
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucCompany")) : null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("ucReportedDateFrom")) : null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("ucReportedDateTo")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("chkContractedRelatedIncidentYN")) : null
            );

        General.SetPrintOptions("gvInspectionIncidentSearch", "Incident List", alCaptions, alColumns, ds);

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
            DataTable dt = ds.Tables[0];
            Filter.CurrentIncidentID = null;
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvInspectionIncidentSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                string lblStatusid = ((RadLabel)e.Item.FindControl("lblStatusid")).Text;
                string lblVesselID = ((RadLabel)e.Item.FindControl("lblVesselID")).Text;
                Filter.CurrentIncidentID = ((RadLabel)e.Item.FindControl("lblInspectionIncidentId")).Text;
                Filter.CurrentIncidentTab = "INCIDENTDETAILS";
                BindPageURL(e.Item.ItemIndex);
                //if (lblStatusid == PhoenixCommonRegisters.GetHardCode(1, 168, "S4")
                //    || lblStatusid == PhoenixCommonRegisters.GetHardCode(1, 168, "S5"))
                //    Filter.CurrentSelectedIncidentMenu = "ilog";
                //else
                //    Filter.CurrentSelectedIncidentMenu = null;

                Response.Redirect("../Inspection/InspectionIncidentList.aspx?callfrom=irecord" + "&Vesselid=" + lblVesselID, false);
            }
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
                SetRowSelection();
                //PhoenixInspectionIncident.CancelIncident(new Guid(inspectionincidentid));
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("CORRECTIVEACTIONS"))
            {
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
            }
            if (e.CommandName.ToUpper().Equals("INCIDENTREPORT"))
            {
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
            }
            if (e.CommandName.ToUpper().Equals("THIRDPARTYINCIDENTREPORT"))
            {
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
            }
            if (e.CommandName.ToUpper().Equals("DIRECTORCOMMENTS"))
            {
                string inspectionincidentid = ((RadLabel)e.Item.FindControl("lblInspectionIncidentId")).Text;
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
                Rebind();
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
                SetRowSelection();
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
            gvInspectionIncidentSearch.SelectedIndexes.Clear();
            foreach (GridDataItem item in gvInspectionIncidentSearch.Items)
            {
                if (item.GetDataKeyValue("FLDINSPECTIONINCIDENTID").ToString() == Filter.CurrentIncidentID)
                {
                    Filter.CurrentIncidentVesselID = (item["RefNo"].FindControl("lblVesselID") as RadLabel).Text;
                    ViewState["DTKEY"] = (item["RefNo"].FindControl("lbldtkey") as RadLabel).Text;
                    gvInspectionIncidentSearch.SelectedIndexes.Add(item.ItemIndex);
                    break;
                }
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
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;

            RadLabel lblInspectionIncidentId = (RadLabel)e.Item.FindControl("lblInspectionIncidentId");
            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null && lblInspectionIncidentId != null) cb.Attributes.Add("onclick", "openNewWindow('NAFA','', '" + Session["sitepath"] + "/Inspection/InspectionCancelReasonUpdate.aspx?REFERENCEID=" + lblInspectionIncidentId.Text + "&TYPE=2','medium'); return true;");
            if (!SessionUtil.CanAccess(this.ViewState, cb.CommandName)) cb.Visible = false;
            LinkButton cmdUnlockIncident = (LinkButton)e.Item.FindControl("cmdUnlockIncident");
            if (!SessionUtil.CanAccess(this.ViewState, cmdUnlockIncident.CommandName)) cmdUnlockIncident.Visible = false;

            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToString().ToUpper().Equals("OFFSHORE"))
            {
                //cmdClose
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
                    cmdClose.Visible = false;
                }
                cmdClose.Attributes.Add("onclick", "openNewWindow('NAFA','', '" + Session["sitepath"] + "/Inspection/InspectionIncidentCloseComment.aspx?REFERENCEID=" + lblInspectionIncidentId.Text + "&type=" + drv["FLDAPPROVALTYPE"].ToString() + "&user=" + drv["FLDQUALITYGENERALMANAGER"].ToString() + "," + drv["FLDQUALITYDIRECTOR"].ToString() + "," + drv["FLDQAMANAGER"].ToString() + "," + drv["FLDQUALITYINCHARGE"].ToString() + "','large'); return true;");
            }
            else
            {

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
                    cmdClose.Visible = false;
                }

                cmdClose.Attributes.Add("onclick", "openNewWindow('NAFA','', '" + Session["sitepath"] + "/Inspection/InspectionIncidentCloseComment.aspx?REFERENCEID=" + lblInspectionIncidentId.Text + "&type=" + drv["FLDAPPROVALTYPE"].ToString() + "&user=" + drv["FLDQUALITYGENERALMANAGER"].ToString() + "," + drv["FLDQUALITYDIRECTOR"].ToString() + "," + drv["FLDQAMANAGER"].ToString() + "," + drv["FLDQUALITYINCHARGE"].ToString() + "','large'); return true;");
                //cmdApprove.ImageUrl = drv["FLDSTATUSOFINCIDENT"].ToString().Equals(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 168, "S2")) ? Session["images"] + "/approve.png" : Session["images"] + "/spacer.gif";
                //cmdApprove.Enabled = drv["FLDSTATUSOFINCIDENT"].ToString().Equals(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 168, "S2")) ? true : false;
                //cmdApprove.Attributes.Add("onclick", "parent.Openpopup('approval', '', '../Common/CommonApproval.aspx?docid=" + drv["FLDINSPECTIONINCIDENTID"].ToString() + "&mod=" + PhoenixModule.QUALITY
                //        + "&type=" + drv["FLDAPPROVALTYPE"].ToString() + "&user=" + drv["FLDQUALITYGENERALMANAGER"].ToString() + "," + drv["FLDQUALITYDIRECTOR"].ToString() + "," + drv["FLDQAMANAGER"].ToString() + "," + drv["FLDQUALITYINCHARGE"].ToString() + "&incidenttype=" + drv["FLDINCIDENTCATEGORY"] + "');return false;");
                cmdClose.Visible = SessionUtil.CanAccess(this.ViewState, cmdClose.CommandName);
            }

            LinkButton cmdCar = (LinkButton)e.Item.FindControl("cmdCorrectiveActions");
            if (cmdCar != null)
            {
                cmdCar.Attributes.Add("onclick", "openNewWindow('car', '', '" + Session["sitepath"] + "/Inspection/InspectionIncidentActionsView.aspx?inspectionincidentid=" + drv["FLDINSPECTIONINCIDENTID"].ToString() + "');return true;");
                cmdCar.Visible = SessionUtil.CanAccess(this.ViewState, cmdCar.CommandName);
                //if (drv["FLDCARPARRECORDEDYN"].ToString() == "0")
                //    cmdCar.ImageUrl = Session["images"] + "/deficiency-noaction.png";
                HtmlGenericControl html = new HtmlGenericControl();

                if (drv["FLDCARPARRECORDEDYN"].ToString() == "0")
                {
                    cmdCar.Controls.Remove(html);
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-eye\"></i></span>";
                    cmdCar.Controls.Add(html);
                }
            }

            LinkButton cmdReport = (LinkButton)e.Item.FindControl("cmdReport");

            if (cmdReport != null)
            {
                cmdReport.Attributes.Add("onclick", "Openpopup('Report', '', '../Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=INCIDENTNEARMISSREPORT&inspectionincidentid=" + lblInspectionIncidentId.Text + "&vesselid=" + drv["FLDVESSELID"].ToString() + "&showmenu=0&showexcel=NO');return true;");
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
            LinkButton cmdDirectorComment = (LinkButton)e.Item.FindControl("imgDirectorComments");
            if (lblInspectionIncidentId != null) cmdDirectorComment.Attributes.Add("onclick", "openNewWindow('NAFA','', '" + Session["sitepath"] + "/Inspection/InspectionIncidentDirectorComment.aspx?REFERENCEID=" + lblInspectionIncidentId.Text + "','large'); return true;");
            if (drv["FLDSTATUSOFINCIDENT"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 168, "S2") && drv["FLDINCIDENTLOCKFORVESSELYN"].ToString() == "1")
            {
                if (cmdUnlockIncident != null)
                {
                    cmdUnlockIncident.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to Unlock this Investigation in Vessel?'); return false;");
                    cmdUnlockIncident.Visible = true;
                }
            }
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
    protected void gvInspectionIncidentSearch_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetRowSelection();
    }
    protected void Rebind()
    {
        gvInspectionIncidentSearch.SelectedIndexes.Clear();
        gvInspectionIncidentSearch.EditIndexes.Clear();
        gvInspectionIncidentSearch.DataSource = null;
        gvInspectionIncidentSearch.Rebind();
    }

    protected void gvInspectionIncidentSearch_PreRender(object sender, EventArgs e)
    {
        SetRowSelection();
    }
}

