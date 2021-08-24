using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionRANavigationList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRANavigationList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessmentNavigation')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRANavigationList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRANavigationList.aspx", "New Template", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        MenuNavigation.AccessRights = this.ViewState;
        MenuNavigation.MenuList = toolbar.Show();

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);

        MenuNavigationMain.AccessRights = this.ViewState;
        MenuNavigationMain.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["REFID"] = "";
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvRiskAssessmentNavigation.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            ViewState["COMPANYID"] = "";
            NameValueCollection nvcCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvcCompany.Get("QMS") != null && nvcCompany.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvcCompany.Get("QMS");
            }
            if (Request.QueryString["filter"] != null)
            {
                if (Request.QueryString["filter"].ToString() == "0")
                {
                    Filter.CurrentNavigationRAFilter = null;
                }
            }
            //if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
            //{

            //MenuNavigationMain.SelectedMenuIndex = 1;
            //}
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string vesselid = "";
        string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDTYPE", "FLDACTIVITYCONDITIONS", "FLDINTENDEDWORKDATE", "FLDREVISIONNO", "FLDSTATUSNAME", "FLDAPPROVEDBY" };
        string[] alCaptions = { "Ref Number", "Vessel", "Prepared", "Type", "Activity / Conditions", "Intended Work", "Revision No", "Status", "Approved By" };

        NameValueCollection nvc = Filter.CurrentNavigationRAFilter;
        if (Filter.CurrentNavigationRAFilter == null)
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

        DataSet ds = PhoenixInspectionRiskAssessmentNavigation.RiskAssessmentNavigationSearch(
                     nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkFrom"]) : null
                    , nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkTo"]) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ucVessel"]) : General.GetNullableInteger(vesselid)
                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                    , gvRiskAssessmentNavigation.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount
                    , General.GetNullableGuid(ViewState["REFID"].ToString())
                    , nvc != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ddlStatus"]) : null
                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                    , nvc != null ? General.GetNullableString(nvc["txtActivityConditions"]) : null
                    , nvc != null ? General.GetNullableDateTime(nvc["ucDatePreparedFrom"]) : null
                    , nvc != null ? General.GetNullableDateTime(nvc["ucDatePreparedTo"]) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ucTechFleet"]) : null
                    );

        General.SetPrintOptions("gvRiskAssessmentNavigation", "Navigation", alCaptions, alColumns, ds);

        gvRiskAssessmentNavigation.DataSource = ds;
        gvRiskAssessmentNavigation.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Filter.CurrentSelectedNavigationRA == null)
            {
                Filter.CurrentSelectedNavigationRA = ds.Tables[0].Rows[0]["FLDRISKASSESSMENTNAVIGATIONID"].ToString();
                gvRiskAssessmentNavigation.SelectedIndexes.Clear();
            }
            SetRowSelection();
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string vesselid = "";
            string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDTYPE", "FLDACTIVITYCONDITIONS", "FLDINTENDEDWORKDATE", "FLDREVISIONNO", "FLDSTATUSNAME", "FLDAPPROVEDBY" };
            string[] alCaptions = { "Ref Number", "Vessel", "Prepared", "Type", "Activity / Conditions", "Intended Work", "Revision No", "Status", "Approved By" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentNavigationRAFilter;
            if (Filter.CurrentNavigationRAFilter == null)
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

            DataSet ds = PhoenixInspectionRiskAssessmentNavigation.RiskAssessmentNavigationSearch(
                         nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkFrom"]) : null
                        , nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkTo"]) : null
                        , General.GetNullableInteger(vesselid)
                        , 1
                        , iRowCount
                        , ref iRowCount
                        , ref iTotalPageCount
                        , General.GetNullableGuid(ViewState["REFID"].ToString())
                        , nvc != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : null
                        , nvc != null ? General.GetNullableInteger(nvc["ddlStatus"]) : null
                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                        , nvc != null ? General.GetNullableString(nvc["txtActivityConditions"]) : null
                        , nvc != null ? General.GetNullableDateTime(nvc["ucDatePreparedFrom"]) : null
                        , nvc != null ? General.GetNullableDateTime(nvc["ucDatePreparedTo"]) : null
                        , nvc != null ? General.GetNullableInteger(nvc["ucTechFleet"]) : null
                        );

            General.ShowExcel("Navigation", ds.Tables[0], alColumns, alCaptions, null, null);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuNavigation_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("ADD"))
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
            {
                Response.Redirect("../Inspection/InspectionRANavigationDetails.aspx?status=", false);
            }
            else
            {
                Response.Redirect("../Inspection/InspectionRANavigation.aspx?status=", false);
            }
        }
        else if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            Filter.CurrentNavigationRAFilter = null;
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
    }
    protected void gvRiskAssessmentNavigation_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            //if (!e.Item.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.RowState.Equals(DataControlRowState.Edit))
            //{
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblNavigationID = (RadLabel)e.Item.FindControl("lblRiskAssessmentNavigationID");
            RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselid");
            RadLabel lblInstallcode = (RadLabel)e.Item.FindControl("lblInstallcode");
            RadLabel lblIsCreatedByOffice = (RadLabel)e.Item.FindControl("lblIsCreatedByOffice");

            //ImageButton imgCopy = (ImageButton)e.Item.FindControl("imgCopy");

            //imgCopy.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRATemplateMapping.aspx?RISKASSESSMENTID=" + lblGenericID.Text + "&TYPE=1');return false;");


            //if (imgCopy != null) imgCopy.Visible = SessionUtil.CanAccess(this.ViewState, imgCopy.CommandName);


            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            LinkButton imgApprove = (LinkButton)e.Item.FindControl("imgApprove");
            LinkButton imgIssue = (LinkButton)e.Item.FindControl("imgIssue");
            LinkButton imgrevision = (LinkButton)e.Item.FindControl("imgrevision");
            LinkButton imgCopyTemplate = (LinkButton)e.Item.FindControl("imgCopyTemplate");
            LinkButton cmdRevision = (LinkButton)e.Item.FindControl("cmdRevision");
            RadLabel lblStatusid = (RadLabel)e.Item.FindControl("lblStatus");

            if (lblInstallcode != null)
            {
                if (General.GetNullableInteger(lblInstallcode.Text) != null && int.Parse(lblInstallcode.Text) > 0) // when RA is approved in vessel.
                {
                    if (imgApprove != null)
                    {
                        imgApprove.ToolTip = "Emergency Override";
                        imgApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionRAApprovalRemarks.aspx?RATEMPLATEID=" + lblNavigationID.Text + "&TYPE=2','medium'); return true;");
                    }
                }
                else if (General.GetNullableInteger(lblInstallcode.Text) != null && int.Parse(lblInstallcode.Text) == 0) // when vessel created RA approved in office.
                {
                    if (lblIsCreatedByOffice != null && lblIsCreatedByOffice.Text != "" && int.Parse(lblIsCreatedByOffice.Text) > 0)
                    {
                        if (imgApprove != null)
                        {
                            imgApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionVesselRAApproval.aspx?RATEMPLATEID=" + lblNavigationID.Text + "&TYPE=2','large'); return true;");
                        }
                    }
                }
            }

            if (imgIssue != null)
            {
                imgIssue.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionRAOfficeApprovalRemarks.aspx?RATEMPLATEID=" + lblNavigationID.Text + "&TYPE=2','medium'); return true;");
            }
            //imgCopy.Visible = false;
            imgrevision.Visible = false;
            imgApprove.Visible = false;
            imgIssue.Visible = false;
            imgCopyTemplate.Visible = false;

            if (lblIsCreatedByOffice != null && lblIsCreatedByOffice.Text == "0")
            {
                if (cmdRevision != null) cmdRevision.Visible = true;
            }
            else
            {
                if (cmdRevision != null) cmdRevision.Visible = false;
            }

            if (lblStatusid.Text == "1")
            {
                //imgCopy.Visible = false;
                imgrevision.Visible = false;
                imgApprove.Visible = true;
                imgIssue.Visible = false;
                imgCopyTemplate.Visible = false;
            }
            else if (lblStatusid.Text == "2")
            {
                //imgCopy.Visible = false;
                imgrevision.Visible = false;
                imgApprove.Visible = false;
                imgIssue.Visible = true;
                imgCopyTemplate.Visible = false;
            }
            else if (lblStatusid.Text == "3")
            {
                //imgCopy.Visible = true;
                if (lblVesselid != null && lblVesselid.Text == "0" && drv["FLDACTIVEYN"].ToString() == "1" && PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                    imgrevision.Visible = true;
                else
                    imgrevision.Visible = false;
                imgApprove.Visible = false;
                imgIssue.Visible = false;
                imgCopyTemplate.Visible = true;
            }
            else if (lblStatusid.Text == "4")
            {
                //imgCopy.Visible = false;
                imgrevision.Visible = false;
                imgApprove.Visible = true;
                imgIssue.Visible = false;
                imgCopyTemplate.Visible = false;
            }
            else if (lblStatusid.Text == "5")
            {
                //imgCopy.Visible = true;
                if (lblVesselid != null && lblVesselid.Text == "0" && drv["FLDACTIVEYN"].ToString() == "1" && PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                    imgrevision.Visible = true;
                else
                    imgrevision.Visible = false;
                imgApprove.Visible = false;
                imgIssue.Visible = false;
                imgCopyTemplate.Visible = true;
            }
            else if (lblStatusid.Text == "6")
            {
                //imgCopy.Visible = true;
                if (lblVesselid != null && lblVesselid.Text == "0" && drv["FLDACTIVEYN"].ToString() == "1" && PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                    imgrevision.Visible = true;
                else
                    imgrevision.Visible = false;
                imgApprove.Visible = false;
                imgIssue.Visible = false;
                imgCopyTemplate.Visible = false;
            }

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                if (imgCopyTemplate != null) imgCopyTemplate.ToolTip = "Use RA";
            }
            else
            {
                if (imgCopyTemplate != null) imgCopyTemplate.ToolTip = "Copy RA";
            }

            if (lblVesselid != null && lblVesselid.Text == "0")
            {
                if (imgApprove != null) imgApprove.ToolTip = "Approve";
            }
            else
            {
                if (imgApprove != null) imgApprove.ToolTip = "Approve/Disapprove";
            }

            LinkButton cmdRANavigation = (LinkButton)e.Item.FindControl("cmdRANavigation");
            if (cmdRANavigation != null)
            {
                cmdRANavigation.Visible = SessionUtil.CanAccess(this.ViewState, cmdRANavigation.CommandName);
                //cmdRANavigation.Visible = false;

                //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                //{
                //    cmdRANavigation.Attributes.Add("onclick", "openNewWindow('RANavigation', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=9&reportcode=RANAVIGATION&navigationid=" + lblNavigationID.Text + "&showmenu=0&showword=NO&showexcel=NO&showpdf=NO&showprint=YES');return false;");
                //}
                //else
                //{
                cmdRANavigation.Attributes.Add("onclick", "openNewWindow('RANavigation', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RANAVIGATION&navigationid=" + lblNavigationID.Text + "&showmenu=0&showexcel=NO');return true;");
                //}
            }

            if (cmdRevision != null)
            {
                cmdRevision.Attributes.Add("onclick", "openNewWindow('RANavigationRevision', '', '" + Session["sitepath"] + "/Inspection/InspectionRANavigationRevisionList.aspx?navigationid=" + lblNavigationID.Text + "');return true;");
            }

            RadLabel lblWorkActivity = (RadLabel)e.Item.FindControl("lblWorkActivity");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucWorkActivity");
            if (uct != null)
            {
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lblWorkActivity.ClientID;
            }

            RadLabel lblApprovedBy = (RadLabel)e.Item.FindControl("lblApprovedBy");
            UserControlToolTip ucApprovedBy = (UserControlToolTip)e.Item.FindControl("ucApprovedBy");
            if (ucApprovedBy != null)
            {
                ucApprovedBy.Position = ToolTipPosition.TopCenter;
                ucApprovedBy.TargetControlId = lblApprovedBy.ClientID;
            }

            if (imgApprove != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgApprove.CommandName)) imgApprove.Visible = false;
            }
            if (imgIssue != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgIssue.CommandName)) imgIssue.Visible = false;
            }
            if (imgrevision != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgrevision.CommandName)) imgrevision.Visible = false;
            }
            if (cmdRevision != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdRevision.CommandName)) cmdRevision.Visible = false;
            }
            if (cmdRANavigation != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdRANavigation.CommandName)) cmdRANavigation.Visible = false;
            }
            if (imgCopyTemplate != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgCopyTemplate.CommandName)) imgCopyTemplate.Visible = false;
            }
            //}
        }
    }

    protected void gvRiskAssessmentNavigation_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        try
        {
            if (gce.CommandName.ToUpper().Equals("EDITROW"))
            {
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentNavigationID");
                RadLabel lblstatus = (RadLabel)gce.Item.FindControl("lblStatus");
                BindPageURL(gce.Item.ItemIndex);
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                {
                    Response.Redirect("../Inspection/InspectionRANavigationDetails.aspx?navigationid=" + lbl.Text + "&status=" + lblstatus.Text, false);
                }
                else
                {
                    Response.Redirect("../Inspection/InspectionRANavigation.aspx?navigationid=" + lbl.Text + "&status=" + lblstatus.Text, false);
                }
                Rebind();
            }
            if (gce.CommandName.ToUpper().Equals("APPROVE"))
            {
                RadLabel lblInstallcode = (RadLabel)gce.Item.FindControl("lblInstallcode");
                RadLabel lblIsCreatedByOffice = (RadLabel)gce.Item.FindControl("lblIsCreatedByOffice");
                BindPageURL(gce.Item.ItemIndex);
                if (lblInstallcode != null && lblInstallcode.Text == "0")
                {
                    //if (lblIsCreatedByOffice != null && int.Parse(lblIsCreatedByOffice.Text) > 0)
                    //{
                    //    ViewState["NAVIGATIONID"] = lbl.Text;
                    //    ucConfirmApprove.Visible = true;
                    //    ucConfirmApprove.Text = "Once the RA is approved, It cannot be edited. Are you sure to continue?";
                    //    return;
                    //}
                    if (lblIsCreatedByOffice != null && int.Parse(lblIsCreatedByOffice.Text) == 0)
                    {
                        RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentNavigationID");
                        PhoenixInspectionRiskAssessmentNavigation.ApproveNavigation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            new Guid(lbl.Text));
                        ucStatus.Text = "Approved Successfully";
                    }
                }
                Rebind();
            }
            if (gce.CommandName.ToUpper().Equals("ISSUE"))
            {
                BindPageURL(gce.Item.ItemIndex);
                Rebind();
                //ViewState["NAVIGATIONID"] = lbl.Text;
                //ucConfirmIssue.Visible = true;
                //ucConfirmIssue.Text = "Once the RA is issued, It cannot be edited. Are you sure to continue?";
                //return;	                
            }
            if (gce.CommandName.ToUpper().Equals("VIEWREVISION"))
            {
                BindPageURL(gce.Item.ItemIndex);
                Rebind();
            }
            if (gce.CommandName.ToUpper().Equals("REVISION"))
            {
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentNavigationID");
                BindPageURL(gce.Item.ItemIndex);
                ViewState["NAVIGATIONID"] = lbl.Text;
                RadWindowManager1.RadConfirm("Are you sure you want to revise this RA.", "ConfirmRevision", 320, 150, null, "Confirm");
                return;
            }
            if (gce.CommandName.ToUpper().Equals("COPYTEMPLATE"))
            {
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentNavigationID");
                BindPageURL(gce.Item.ItemIndex);
                ViewState["NAVIGATIONID"] = lbl.Text;
                RadWindowManager1.RadConfirm("Are you sure to copy the template?", "Confirm", 320, 150, null, "Confirm");
                return;
            }
            if (gce.CommandName.ToUpper().Equals("RANAVIGATION"))
            {
                BindPageURL(gce.Item.ItemIndex);
                Rebind();
            }
            if (gce.CommandName == "Page")
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblRiskAssessmentNavigationID = (RadLabel)gvRiskAssessmentNavigation.Items[rowindex].FindControl("lblRiskAssessmentNavigationID");
            if (lblRiskAssessmentNavigationID != null)
            {
                Filter.CurrentSelectedNavigationRA = lblRiskAssessmentNavigationID.Text;
            }
            SetRowSelection();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
        gvRiskAssessmentNavigation.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvRiskAssessmentNavigation.Items)
        {
            if (item.GetDataKeyValue("FLDRISKASSESSMENTNAVIGATIONID").ToString() == Filter.CurrentSelectedNavigationRA)
            {
                gvRiskAssessmentNavigation.SelectedIndexes.Add(item.ItemIndex);
                break;
            }
        }
    }
    protected void MenuNavigationMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        //if (dce.CommandName.ToUpper().Equals("GENERIC"))
        //{
        //    Response.Redirect("../Inspection/InspectionRAGenericList.aspx?filter=0", false);
        //}
        //if (dce.CommandName.ToUpper().Equals("MACHINERY"))
        //{
        //    Response.Redirect("../Inspection/InspectionRAMachineryList.aspx?filter=0", false);
        //}
        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            Response.Redirect("../Inspection/InspectionNonRoutineRAFilter.aspx", false);
        }
    }
    protected void gvRiskAssessmentNavigation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRiskAssessmentNavigation.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        ViewState["PAGENUMBER"] = 1;
        gvRiskAssessmentNavigation.SelectedIndexes.Clear();
        gvRiskAssessmentNavigation.EditIndexes.Clear();
        gvRiskAssessmentNavigation.DataSource = null;
        gvRiskAssessmentNavigation.Rebind();
    }

    protected void ucConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["NAVIGATIONID"] != null && ViewState["NAVIGATIONID"].ToString() != "")
            {
                PhoenixInspectionRiskAssessmentNavigation.CopyNavigation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["NAVIGATIONID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                ucStatus.Text = "Copied Successfully";
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirmApprove_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["NAVIGATIONID"] != null && ViewState["NAVIGATIONID"].ToString() != "")
            {
                PhoenixInspectionRiskAssessmentNavigation.ApproveNavigation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["NAVIGATIONID"].ToString()));
                ucStatus.Text = "Approved Successfully";
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirmIssue_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["NAVIGATIONID"] != null && ViewState["NAVIGATIONID"].ToString() != "")
            {
                PhoenixInspectionRiskAssessmentNavigation.IssueNavigation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["NAVIGATIONID"].ToString()));
                ucStatus.Text = "Issued Successfully";
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirmRevision_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["NAVIGATIONID"] != null && ViewState["NAVIGATIONID"].ToString() != "")
                {
                    PhoenixInspectionRiskAssessmentNavigation.ReviseNavigation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["NAVIGATIONID"].ToString()));
                    ucStatus.Text = "RA is revised.";
                    Rebind();
                }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetRowSelection();
    }
}
