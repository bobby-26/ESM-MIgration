using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionRAMachineryList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRAMachineryList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessmentMachinery')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRAMachineryList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRAMachineryList.aspx", "New Template", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        MenuMachinery.AccessRights = this.ViewState;
        MenuMachinery.MenuList = toolbar.Show();

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);

        MenuMachineryMain.AccessRights = this.ViewState;
        MenuMachineryMain.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["REFID"] = "";
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvRiskAssessmentMachinery.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

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
                    Filter.CurrentMachineryRAFilter = null;
                }
            }
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string vesselid = "";

        string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDTYPE", "FLDACTIVITYCONDITIONS", "FLDINTENDEDWORKDATE", "FLDREVISIONNO", "FLDSTATUSNAME", "FLDAPPROVEDBY" };
        string[] alCaptions = { "Ref Number", "Vessel", "Prepared", "Type", "Activity / Conditions", "Intended Work", "Revision No", "Status", "Approved By" };

        NameValueCollection nvc = Filter.CurrentMachineryRAFilter;
        if (Filter.CurrentMachineryRAFilter == null)
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

        DataSet ds = PhoenixInspectionRiskAssessmentMachinery.RiskAssessmentMachinerySearch(
                     nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkFrom"]) : null
                    , nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkTo"]) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ucVessel"]) : General.GetNullableInteger(vesselid)
                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                    , gvRiskAssessmentMachinery.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount
                    , General.GetNullableGuid(ViewState["REFID"].ToString())
                    , nvc != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ddlStatus"]) : null
                    , null
                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                    , nvc != null ? General.GetNullableString(nvc["txtActivityConditions"]) : null
                    , nvc != null ? General.GetNullableDateTime(nvc["ucDatePreparedFrom"]) : null
                    , nvc != null ? General.GetNullableDateTime(nvc["ucDatePreparedTo"]) : null
                   , nvc != null ? General.GetNullableInteger(nvc["ucTechFleet"]) : null
                    );

        General.SetPrintOptions("gvRiskAssessmentMachinery", "Machinery", alCaptions, alColumns, ds);

        gvRiskAssessmentMachinery.DataSource = ds;
        gvRiskAssessmentMachinery.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Filter.CurrentSelectedMachineryRA == null)
            {
                Filter.CurrentSelectedMachineryRA = ds.Tables[0].Rows[0]["FLDRISKASSESSMENTMACHINERYID"].ToString();
                gvRiskAssessmentMachinery.SelectedIndexes.Clear();
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

            NameValueCollection nvc = Filter.CurrentMachineryRAFilter;
            if (Filter.CurrentMachineryRAFilter == null)
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

            DataSet ds = PhoenixInspectionRiskAssessmentMachinery.RiskAssessmentMachinerySearch(
                     nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkFrom"]) : null
                    , nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkTo"]) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ucVessel"]) : General.GetNullableInteger(vesselid)
                    , 1
                    , iRowCount
                    , ref iRowCount
                    , ref iTotalPageCount
                    , General.GetNullableGuid(ViewState["REFID"].ToString())
                    , nvc != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ddlStatus"]) : null
                    , null
                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                    , nvc != null ? General.GetNullableString(nvc["txtActivityConditions"]) : null
                    , nvc != null ? General.GetNullableDateTime(nvc["ucDatePreparedFrom"]) : null
                    , nvc != null ? General.GetNullableDateTime(nvc["ucDatePreparedTo"]) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ucTechFleet"]) : null
                    );

            General.ShowExcel("Machinery", ds.Tables[0], alColumns, alCaptions, null, null);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuMachinery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("ADD"))
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
            {
                Response.Redirect("../Inspection/InspectionRAMachineryDetails.aspx?status=", false);
            }
            else
            {
                Response.Redirect("../Inspection/InspectionRAMachinery.aspx?status=", false);
            }
        }
        else if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            Filter.CurrentMachineryRAFilter = null;
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
    }
    protected void gvRiskAssessmentMachinery_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblMachineryID = (RadLabel)e.Item.FindControl("lblRiskAssessmentMachineryID");
            RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselid");
            RadLabel lblInstallcode = (RadLabel)e.Item.FindControl("lblInstallcode");
            RadLabel lblIsCreatedByOffice = (RadLabel)e.Item.FindControl("lblIsCreatedByOffice");

            //LinkButton imgCopy = (LinkButton)e.Item.FindControl("imgCopy");

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
                        imgApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionRAApprovalRemarks.aspx?RATEMPLATEID=" + lblMachineryID.Text + "&TYPE=3','medium'); return true;");
                    }
                }
                else if (General.GetNullableInteger(lblInstallcode.Text) != null && int.Parse(lblInstallcode.Text) == 0) // when vessel created RA approved in office.
                {
                    if (lblIsCreatedByOffice != null && lblIsCreatedByOffice.Text != "" && int.Parse(lblIsCreatedByOffice.Text) > 0)
                    {
                        if (imgApprove != null)
                        {
                            imgApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionVesselRAApproval.aspx?RATEMPLATEID=" + lblMachineryID.Text + "&TYPE=3','large'); return true;");
                        }
                    }
                }
            }

            if (imgIssue != null)
            {
                imgIssue.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionRAOfficeApprovalRemarks.aspx?RATEMPLATEID=" + lblMachineryID.Text + "&TYPE=3','medium'); return true;");
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

            LinkButton cmdRAMachinery = (LinkButton)e.Item.FindControl("cmdRAMachinery");
            if (cmdRAMachinery != null)
            {
                cmdRAMachinery.Visible = SessionUtil.CanAccess(this.ViewState, cmdRAMachinery.CommandName);
                cmdRAMachinery.Attributes.Add("onclick", "openNewWindow('RAMachinery', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + lblMachineryID.Text + "&showmenu=0&showexcel=NO');return true;");
            }

            if (cmdRevision != null)
            {
                cmdRevision.Attributes.Add("onclick", "openNewWindow('RAMachineryRevision', '', '" + Session["sitepath"] + "/Inspection/InspectionRAMachineryRevisionList.aspx?machineryid=" + lblMachineryID.Text + "');return true;");
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
            if (cmdRAMachinery != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdRAMachinery.CommandName)) cmdRAMachinery.Visible = false;
            }
            if (imgCopyTemplate != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgCopyTemplate.CommandName)) imgCopyTemplate.Visible = false;
            }
        }
    }

    protected void gvRiskAssessmentMachinery_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        try
        {
            if (gce.CommandName.ToUpper().Equals("EDITROW"))
            {
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentMachineryID");
                RadLabel lblstatus = (RadLabel)gce.Item.FindControl("lblStatus");
                BindPageURL(gce.Item.ItemIndex);
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                {
                    Response.Redirect("../Inspection/InspectionRAMachineryDetails.aspx?machineryid=" + lbl.Text + "&status=" + lblstatus.Text, false);
                }
                else
                {
                    Response.Redirect("../Inspection/InspectionRAMachinery.aspx?machineryid=" + lbl.Text + "&status=" + lblstatus.Text, false);
                }
                Rebind();
            }
            if (gce.CommandName.ToUpper().Equals("APPROVE"))
            {
                RadLabel lblInstallcode = (RadLabel)gce.Item.FindControl("lblInstallcode");
                RadLabel lblIsCreatedByOffice = (RadLabel)gce.Item.FindControl("lblIsCreatedByOffice");
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentMachineryID");

                BindPageURL(gce.Item.ItemIndex);
                if (lblInstallcode != null && lblInstallcode.Text == "0")
                {
                    //if (lblIsCreatedByOffice != null && int.Parse(lblIsCreatedByOffice.Text) > 0)
                    //{
                    //    ViewState["MACHINERYID"] = lbl.Text;
                    //    ucConfirmApprove.Visible = true;
                    //    ucConfirmApprove.Text = "Once the RA is approved, It cannot be edited. Are you sure to continue?";
                    //    return;
                    //}
                    if (lblIsCreatedByOffice != null && int.Parse(lblIsCreatedByOffice.Text) == 0)
                    {
                        PhoenixInspectionRiskAssessmentMachinery.ApproveMachinery(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
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
                //ViewState["MACHINERYID"] = lbl.Text;
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
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentMachineryID");
                BindPageURL(gce.Item.ItemIndex);
                ViewState["MACHINERYID"] = lbl.Text;
                RadWindowManager1.RadConfirm("Are you sure you want to revise this RA.", "ConfirmRevision", 320, 150, null, "Confirm");
                return;
            }
            if (gce.CommandName.ToUpper().Equals("COPYTEMPLATE"))
            {
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentMachineryID");
                BindPageURL(gce.Item.ItemIndex);
                ViewState["MACHINERYID"] = lbl.Text;
                RadWindowManager1.RadConfirm("Are you sure to copy the template?", "Confirm", 320, 150, null, "Confirm");
                return;
            }
            if (gce.CommandName.ToUpper().Equals("RAMACHINERY"))
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
            RadLabel lblRiskAssessmentMachineryID = (RadLabel)gvRiskAssessmentMachinery.Items[rowindex].FindControl("lblRiskAssessmentMachineryID");
            if (lblRiskAssessmentMachineryID != null)
            {
                Filter.CurrentSelectedMachineryRA = lblRiskAssessmentMachineryID.Text;
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
        gvRiskAssessmentMachinery.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvRiskAssessmentMachinery.Items)
        {
            if (item.GetDataKeyValue("FLDRISKASSESSMENTMACHINERYID").ToString() == Filter.CurrentSelectedMachineryRA)
            {
                gvRiskAssessmentMachinery.SelectedIndexes.Add(item.ItemIndex);
                break;
            }
        }
    }
    protected void MenuMachineryMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        //if (dce.CommandName.ToUpper().Equals("GENERIC"))
        //{
        //    Response.Redirect("../Inspection/InspectionRAGenericList.aspx?filter=0", false);
        //}
        //if (dce.CommandName.ToUpper().Equals("NAVIGATION"))
        //{
        //    Response.Redirect("../Inspection/InspectionRANavigationList.aspx?filter=0", false);
        //}
        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            Response.Redirect("../Inspection/InspectionNonRoutineRAFilter.aspx", false);
        }
    }

    protected void gvRiskAssessmentMachinery_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRiskAssessmentMachinery.CurrentPageIndex + 1;
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
        gvRiskAssessmentMachinery.SelectedIndexes.Clear();
        gvRiskAssessmentMachinery.EditIndexes.Clear();
        gvRiskAssessmentMachinery.DataSource = null;
        gvRiskAssessmentMachinery.Rebind();
    }
    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetRowSelection();
    }

    protected void ucConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["MACHINERYID"] != null && ViewState["MACHINERYID"].ToString() != "")
            {
                PhoenixInspectionRiskAssessmentMachinery.CopyMachinery(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["MACHINERYID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
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
            if (ViewState["MACHINERYID"] != null && ViewState["MACHINERYID"].ToString() != "")
            {
                PhoenixInspectionRiskAssessmentMachinery.ApproveMachinery(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["MACHINERYID"].ToString()));
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
            if (ViewState["MACHINERYID"] != null && ViewState["MACHINERYID"].ToString() != "")
            {
                PhoenixInspectionRiskAssessmentMachinery.IssueMachinery(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["MACHINERYID"].ToString()));
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
            if (ViewState["MACHINERYID"] != null && ViewState["MACHINERYID"].ToString() != "")
            {
                PhoenixInspectionRiskAssessmentMachinery.ReviseMachinery(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["MACHINERYID"].ToString()));
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
}
