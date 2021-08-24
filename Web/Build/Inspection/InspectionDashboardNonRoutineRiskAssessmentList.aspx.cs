using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Reports;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionDashboardNonRoutineRiskAssessmentList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionDashboardNonRoutineRiskAssessmentList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessment')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionDashboardNonRoutineRiskAssessmentList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
//        toolbar.AddFontAwesomeButton("../Inspection/InspectionDashboardNonRoutineRiskAssessmentList.aspx", "New Template", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

        MenuGeneric.AccessRights = this.ViewState;
        MenuGeneric.MenuList = toolbar.Show();

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Job Hazard Analysis", "JOBHAZARDANALYSIS");
        toolbarmain.AddButton("Process RA", "PROCESSRA");
        toolbarmain.AddButton("Standard Templates", "STANDARDTEMPLATES");
        toolbarmain.AddButton("Non Routine RA", "NONROUTINERA");
        MenuARSubTab.AccessRights = this.ViewState;
        MenuARSubTab.MenuList = toolbarmain.Show();

        MenuARSubTab.SelectedMenuIndex = 3;

        if (!IsPostBack)
        {
            ucConfirmApprove.Attributes.Add("style", "display:none");
            ucConfirmRevision.Attributes.Add("style", "display:none");
            btncancel.Attributes.Add("style", "display:none");
            btnnotused.Attributes.Add("style", "display:none");
            btnstandardtemplateissue.Attributes.Add("style", "display:none");

            ViewState["REFID"] = "";
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["TYPE"] = "0";
            ViewState["COMPANYID"] = "";
            ViewState["txtRefNo"] = string.Empty;
            ViewState["VesselID"] = string.Empty;
            ViewState["ddlStatus"] = string.Empty;
            ViewState["ddlRAType"] = string.Empty;
            ViewState["JobActivity"] = string.Empty;
            ViewState["FDATE"] = string.Empty;
            ViewState["TDATE"] = string.Empty;

            NameValueCollection nvcCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvcCompany.Get("QMS") != null && nvcCompany.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvcCompany.Get("QMS");
            }

            gvRiskAssessment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void MenuARSubTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("JOBHAZARDANALYSIS"))
        {
            Response.Redirect("../Inspection/InspectionRDashboardAJobHazardAnalysisList.aspx");
        }
        if (CommandName.ToUpper().Equals("STANDARDTEMPLATES"))
        {
            Response.Redirect("../Inspection/InspectionStandardTemplateNonRoutineRA.aspx");
        }
        if (CommandName.ToUpper().Equals("PROCESSRA"))
        {
            Response.Redirect("../Inspection/InspectionDashboardMainFleetRAProcessListExtn.aspx");
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        //string vesselid = "";
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE","FLDINTENDEDWORKDATE", "FLDTYPE", "FLDACTIVITYCONDITIONS", "FLDCOMPLETIONDATE", "FLDTASKSTATUS", "FLDREVISIONNO", "FLDSTATUSNAME"};
        string[] alCaptions = { "Ref. No", "Vessel", "Prepared","Intended Work", "Type", "Activity / Conditions", "Target Date for completion", "Task completed YN", "Revision No", "Status" };

        NameValueCollection nvc = InspectionFilter.CurrentNonRoutineRAFilter;

        DataSet ds = PhoenixInspectionRiskAssessmentGenericExtn.PhoenixInspectionNonRoutineRiskAssessmentDashboardSearch(
                     nvc.Get("txtRefNo") != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : General.GetNullableString(ViewState["txtRefNo"].ToString())
                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                    , gvRiskAssessment.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount
                    , sortexpression, sortdirection
                    ,4
                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                    , nvc.Get("FDATE") != null ? General.GetNullableDateTime(nvc["FDATE"]) : General.GetNullableDateTime(ViewState["FDATE"].ToString())
                    , nvc.Get("TDATE") != null ? General.GetNullableDateTime(nvc["TDATE"]) : General.GetNullableDateTime(ViewState["TDATE"].ToString())
                    , nvc.Get("ucVessel") != null ? General.GetNullableInteger(nvc["ucVessel"]) : General.GetNullableInteger(ViewState["VesselID"].ToString())
                    , null
                    , null
                    , nvc.Get("ddlRAType") != null ? General.GetNullableInteger(nvc["ddlRAType"]) : General.GetNullableInteger(ViewState["ddlRAType"].ToString())
                    , nvc.Get("JobActivity") != null ? General.GetNullableString(nvc["JobActivity"]) : General.GetNullableString(ViewState["JobActivity"].ToString())
                    );

        General.SetPrintOptions("gvRiskAssessment", "Non Routine RA", alCaptions, alColumns, ds);

        gvRiskAssessment.DataSource = ds;
        gvRiskAssessment.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Filter.CurrentSelectedGenericRA == null)
            {
                Filter.CurrentSelectedGenericRA = ds.Tables[0].Rows[0]["FLDRISKASSESSMENTID"].ToString();
                gvRiskAssessment.SelectedIndexes.Clear();
            }
            SetRowSelection();
        }

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDINTENDEDWORKDATE", "FLDTYPE", "FLDACTIVITYCONDITIONS", "FLDCOMPLETIONDATE", "FLDTASKSTATUS", "FLDREVISIONNO", "FLDSTATUSNAME" };
            string[] alCaptions = { "Ref. No", "Vessel", "Prepared", "Intended Work", "Type", "Activity / Conditions", "Target Date for completion", "Task completed YN", "Revision No", "Status" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = InspectionFilter.CurrentNonRoutineRAFilter;

            DataSet ds = PhoenixInspectionRiskAssessmentGenericExtn.PhoenixInspectionNonRoutineRiskAssessmentDashboardSearch(
                    nvc != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : null
                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                    , iRowCount
                    , ref iRowCount
                    , ref iTotalPageCount
                    , sortexpression, sortdirection
                   , 4
                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                    , nvc.Get("FDATE") != null ? General.GetNullableDateTime(nvc["FDATE"]) : General.GetNullableDateTime(ViewState["FDATE"].ToString())
                    , nvc.Get("TDATE") != null ? General.GetNullableDateTime(nvc["TDATE"]) : General.GetNullableDateTime(ViewState["TDATE"].ToString())
                    , nvc.Get("ucVessel") != null ? General.GetNullableInteger(nvc["ucVessel"]) : General.GetNullableInteger(ViewState["VesselID"].ToString())
                    , null
                    , null
                    , nvc.Get("ddlRAType") != null ? General.GetNullableInteger(nvc["ddlRAType"]) : General.GetNullableInteger(ViewState["ddlRAType"].ToString())
                    , nvc.Get("JobActivity") != null ? General.GetNullableString(nvc["JobActivity"]) : General.GetNullableString(ViewState["JobActivity"].ToString())
                    );

            General.ShowExcel("Non Routine RA", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvRiskAssessment.SelectedIndexes.Clear();
        gvRiskAssessment.EditIndexes.Clear();
        gvRiskAssessment.DataSource = null;
        gvRiskAssessment.Rebind();
    }
    protected void MenuGeneric_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("ADD"))
        {
            Response.Redirect("../Inspection/InspectionNewRASelection.aspx?", false);
        }

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }

        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            ViewState["txtRefNo"] = string.Empty;
            ViewState["VesselID"] = string.Empty;
            ViewState["ddlStatus"] = string.Empty;
            ViewState["ddlRAType"] = string.Empty;
            ViewState["JobActivity"] = string.Empty;
            ViewState["FDATE"] = string.Empty;
            ViewState["TDATE"] = string.Empty;

            ViewState["PAGENUMBER"] = 1;

            InspectionFilter.CurrentNonRoutineRAFilter.Clear();
            SetFilter();
            Rebind();
            gvRiskAssessment.Rebind();
        }
    }

    protected void gvRiskAssessment_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblGenericID = (RadLabel)e.Item.FindControl("lblRiskAssessmentGenericID");
           RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselid");
           RadLabel lblInstallcode = (RadLabel)e.Item.FindControl("lblInstallcode");
            RadLabel lbltypeid = (RadLabel)e.Item.FindControl("lblTypeid");

            LinkButton imgApprove = (LinkButton)e.Item.FindControl("imgApprove");
            LinkButton imgrevision = (LinkButton)e.Item.FindControl("imgrevision");
            LinkButton imgCopyTemplate = (LinkButton)e.Item.FindControl("imgCopyTemplate");
            LinkButton cmdRevision = (LinkButton)e.Item.FindControl("cmdRevision");
            RadLabel lblStatusid = (RadLabel)e.Item.FindControl("lblStatus");
            RadLabel lblIsCreatedByOffice = (RadLabel)e.Item.FindControl("lblIsCreatedByOffice");
            LinkButton lnkstissue = (LinkButton)e.Item.FindControl("lnkstissue");
            LinkButton imgcancel = (LinkButton)e.Item.FindControl("imgcancel");
            LinkButton imgnotused = (LinkButton)e.Item.FindControl("imgnotused");

            if (lblInstallcode != null)
            {
                if (General.GetNullableInteger(lblInstallcode.Text) != null && int.Parse(lblInstallcode.Text) > 0) // when RA is approved in vessel.
                {
                    if (imgApprove != null)
                    {
                        imgApprove.ToolTip = "Emergency Override";

                        if (lbltypeid.Text == "1")
                        {
                            imgApprove.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApprovalRemarksExtn.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=1','medium'); return true;");
                        }
                        if (lbltypeid.Text == "2")
                        {
                            imgApprove.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApprovalRemarksExtn.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=2','medium'); return true;");
                        }
                        if (lbltypeid.Text == "3")
                        {
                            imgApprove.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApprovalRemarksExtn.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=3','medium'); return true;");
                        }
                        if (lbltypeid.Text == "4")
                        {
                            imgApprove.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApprovalRemarksExtn.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=5','medium'); return true;");
                        }
                    }
                }
                else if (General.GetNullableInteger(lblInstallcode.Text) != null && int.Parse(lblInstallcode.Text) == 0) // when vessel created RA approved in office.
                {
                    if (lblVesselid.Text == "0")
                    {
                        if (lbltypeid.Text == "1")
                        {
                            imgApprove.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAMainFleetOfficeApprovalRemarksExtn.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=1','medium'); return true;");
                        }
                        if (lbltypeid.Text == "2")
                        {
                            imgApprove.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAMainFleetOfficeApprovalRemarksExtn.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=2','medium'); return true;");
                        }
                        if (lbltypeid.Text == "3")
                        {
                            imgApprove.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAMainFleetOfficeApprovalRemarksExtn.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=3','medium'); return true;");
                        }
                        if (lbltypeid.Text == "4")
                        {
                            imgApprove.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAMainFleetOfficeApprovalRemarksExtn.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=5','medium'); return true;");
                        }
                    }

                    else if (lblIsCreatedByOffice != null && lblIsCreatedByOffice.Text != "" && int.Parse(lblIsCreatedByOffice.Text) > 0)
                    {
                        if (imgApprove != null)
                        {
                            if (lbltypeid.Text == "1")
                            {
                                imgApprove.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApprovalExtn.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=1','large'); return true;");
                            }
                            if (lbltypeid.Text == "2")
                            {
                                imgApprove.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApprovalExtn.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=2','large'); return true;");
                            }
                            if (lbltypeid.Text == "3")
                            {
                                imgApprove.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApprovalExtn.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=3','large'); return true;");
                            }
                            if (lbltypeid.Text == "4")
                            {
                                imgApprove.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApprovalExtn.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=5','large'); return true;");
                            }
                        }
                    }
                }
            }

            if (imgCopyTemplate != null)
            {
                if (lbltypeid.Text == "1")
                {
                    imgCopyTemplate.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAGenericExtn.aspx?genericid=" + lblGenericID.Text + "&CopyType=1&showall=1" + "'); return true;");
                }
                if (lbltypeid.Text == "2")
                {
                    imgCopyTemplate.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRANavigationExtn.aspx?navigationid=" + lblGenericID.Text + "&CopyType=1&showall=1" + "'); return true;");
                }
                if (lbltypeid.Text == "3")
                {
                    imgCopyTemplate.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAMachineryExtn.aspx?machineryid=" + lblGenericID.Text + "&CopyType=1&showall=1" + "'); return true;");
                }
                if (lbltypeid.Text == "4")
                {
                    imgCopyTemplate.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRACargoExtn.aspx?genericid=" + lblGenericID.Text + "&CopyType=1&showall=1" + "'); return true;");
                }
            }

            imgrevision.Visible = false;
            imgApprove.Visible = false;
            lnkstissue.Visible = false;

            cmdRevision.Visible = false;
            if (cmdRevision != null && lblVesselid.Text == "0")
            {
                cmdRevision.Visible = true;
            }

            if (lblStatusid.Text == "1")
            {
                imgrevision.Visible = false;
                //imgCopyTemplate.Visible = false;
            }

            if (lblStatusid.Text == "10")
            {
                imgcancel.Visible = false;
                imgnotused.Visible = false;
            }


            if (lblStatusid.Text == "11")
            {
                imgcancel.Visible = false;
                imgnotused.Visible = false;
            }

            else if (lblStatusid.Text == "4")
            {
                imgrevision.Visible = false;
                imgApprove.Visible = true;
                ////imgIssue.Visible = false;
                //imgCopyTemplate.Visible = false;
            }

            else if (lblStatusid.Text == "5")
            {
                if (lblVesselid != null && lblVesselid.Text == "0" && drv["FLDACTIVEYN"].ToString() == "1" && PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                    imgrevision.Visible = true;
                else
                    imgrevision.Visible = false;

                if (lblVesselid != null && lblVesselid.Text == "0" && PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                    lnkstissue.Visible = true;
                else
                    lnkstissue.Visible = false;

                imgApprove.Visible = false;
                //imgIssue.Visible = false;
                //imgCopyTemplate.Visible = true;
            }

            else if (lblStatusid.Text == "6")
            {
                if (lblVesselid != null && lblVesselid.Text == "0" && drv["FLDACTIVEYN"].ToString() == "1" && PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                    imgrevision.Visible = true;
                else
                    imgrevision.Visible = false;
                imgApprove.Visible = false;
                //imgIssue.Visible = false;
                //imgCopyTemplate.Visible = true;
            }

            else if (lblStatusid.Text == "7")
            {
                if (lblVesselid != null && lblVesselid.Text == "0" && PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                    lnkstissue.Visible = true;
                else
                    lnkstissue.Visible = false;
            }

            //if (lblVesselid != null && lblVesselid.Text == "0")
            //{
            //    if (imgApprove != null) imgApprove.ToolTip = "Request for Approval";
            //}
            //else
            //{
            //    if (imgApprove != null) imgApprove.ToolTip = "Approve/Reject";
            //}

            LinkButton cmdRAGeneric = (LinkButton)e.Item.FindControl("cmdRAGeneric");
            if (cmdRAGeneric != null)
            {
                cmdRAGeneric.Visible = SessionUtil.CanAccess(this.ViewState, cmdRAGeneric.CommandName);                
            }

            if (cmdRevision != null)
            {
                if (lbltypeid.Text == "1")
                {
                    cmdRevision.Attributes.Add("onclick", "openNewWindow('RAGenericRevision', '', '" + Session["sitepath"] + "/Inspection/InspectionRAGenericRevisionListExtn.aspx?genericid=" + lblGenericID.Text + "');return true;");
                }
                if (lbltypeid.Text == "2")
                {
                    cmdRevision.Attributes.Add("onclick", "openNewWindow('RAGenericRevision', '', '" + Session["sitepath"] + "/Inspection/InspectionRANavigationRevisionListExtn.aspx?navigationid=" + lblGenericID.Text + "');return true;");
                }
                if (lbltypeid.Text == "3")
                {
                    cmdRevision.Attributes.Add("onclick", "openNewWindow('RAGenericRevision', '', '" + Session["sitepath"] + "/Inspection/InspectionRAMachineryRevisionListExtn.aspx?machineryid=" + lblGenericID.Text + "');return true;");
                }
                if (lbltypeid.Text == "4")
                {
                    cmdRevision.Attributes.Add("onclick", "openNewWindow('RAGenericRevision', '', '" + Session["sitepath"] + "/Inspection/InspectionRACargoRevisionListExtn.aspx?genericid=" + lblGenericID.Text + "');return true;");
                }
            }

            LinkButton cmdShowPendingTask = (LinkButton)e.Item.FindControl("cmdShowPendingTask");
            if ((cmdShowPendingTask != null) && (lblStatusid.Text == "5") || (lblStatusid.Text == "7"))
            {
                cmdShowPendingTask.Visible = true;
                cmdShowPendingTask.Visible = SessionUtil.CanAccess(this.ViewState, cmdShowPendingTask.CommandName);
                cmdShowPendingTask.Attributes.Add("onclick", "openNewWindow('PendingTask', '', '" + Session["sitepath"] + "/Inspection/InspectionNonRoutineRATask.aspx?RAID=" + lblGenericID.Text + "');return true;");
            }

            LinkButton lnkComplete = (LinkButton)e.Item.FindControl("lnkComplete");            
            if (lnkComplete != null)
            {
                if (lblStatusid.Text == "5")
                {
                    lnkComplete.Visible = true;
                    lnkComplete.Visible = SessionUtil.CanAccess(this.ViewState, lnkComplete.CommandName);
                    lnkComplete.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRiskAssessmentVerification.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=" + lbltypeid.Text + "');return true;");
                }
                else
                    lnkComplete.Visible = false;
            }
           
            if (imgApprove != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgApprove.CommandName)) imgApprove.Visible = false;
            }
            if (cmdRevision != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdRevision.CommandName)) cmdRevision.Visible = false;
            }
            if (cmdRAGeneric != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdRAGeneric.CommandName)) cmdRAGeneric.Visible = false;
            }
            if (imgCopyTemplate != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgCopyTemplate.CommandName)) imgCopyTemplate.Visible = false;
            }

            if (lnkstissue != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, lnkstissue.CommandName)) lnkstissue.Visible = false;
            }

            if (imgcancel != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgcancel.CommandName)) imgcancel.Visible = false;
            }

            if (imgnotused != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgnotused.CommandName)) imgnotused.Visible = false;
            }
        }
    }

    protected void gvRiskAssessment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRiskAssessment.CurrentPageIndex + 1;            
            SetFilter();
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRiskAssessment_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        try
        {
            if (gce.CommandName.ToUpper().Equals("EDITROW"))
            {
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentGenericID");
                RadLabel lblstatus = (RadLabel)gce.Item.FindControl("lblStatus");
                RadLabel lbltypeid = (RadLabel)gce.Item.FindControl("lblTypeid");

                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
                if (lbltypeid.Text == "1")
                {
                    Response.Redirect("../Inspection/InspectionRAGenericExtn.aspx?showall=1&genericid=" + lbl.Text + "&status=" + lblstatus.Text + "&HSEQADashboardYN=1", false);
                }
                if (lbltypeid.Text == "2")
                {
                    Response.Redirect("../Inspection/InspectionRANavigationExtn.aspx?showall=1&navigationid=" + lbl.Text + "&status=" + lblstatus.Text + "&HSEQADashboardYN=1", false);
                }
                if (lbltypeid.Text == "3")
                {
                    Response.Redirect("../Inspection/InspectionRAMachineryExtn.aspx?showall=1&machineryid=" + lbl.Text + "&status=" + lblstatus.Text + "&HSEQADashboardYN=1", false);
                }
                if (lbltypeid.Text == "4")
                {
                    Response.Redirect("../Inspection/InspectionRACargoExtn.aspx?showall=1&genericid=" + lbl.Text + "&status=" + lblstatus.Text + "&HSEQADashboardYN=1", false);
                }

            }

            if (gce.CommandName.ToUpper().Equals("STISSUE"))
            {
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentGenericID");
                RadLabel lbltypeid = (RadLabel)gce.Item.FindControl("lblTypeid");

                ViewState["GENERICID"] = lbl.Text;
                ViewState["TYPE"] = lbltypeid.Text;

                RadWindowManager1.RadConfirm("Are you sure you want to issue as standard templete?", "confirmissue", 320, 150, null, "Issue as Standard Template");
            }

            if (gce.CommandName.ToUpper().Equals("CANCEL"))
            {
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentGenericID");
                RadLabel lbltypeid = (RadLabel)gce.Item.FindControl("lblTypeid");
                ViewState["GENERICID"] = lbl.Text;
                ViewState["TYPE"] = lbltypeid.Text;

                RadWindowManager1.RadConfirm("Are you sure you want to cancel this RA?", "confirmcancel", 320, 150, null, "Cancel");
            }

            if (gce.CommandName.ToUpper().Equals("NOTUSE"))
            {
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentGenericID");
                RadLabel lbltypeid = (RadLabel)gce.Item.FindControl("lblTypeid");

                ViewState["GENERICID"] = lbl.Text;
                ViewState["TYPE"] = lbltypeid.Text;

                RadWindowManager1.RadConfirm("Are you sure you want to this RA is not required?", "confirmnotused", 320, 150, null, "Not Use");
            }
            if (gce.CommandName.ToUpper().Equals("ISSUE"))
            {
                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
            }
            if (gce.CommandName.ToUpper().Equals("REVISION"))
            {
                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentGenericID");
                RadLabel lbltypeid = (RadLabel)gce.Item.FindControl("lblTypeid");

                ViewState["GENERICID"] = lbl.Text;
                ViewState["TYPE"] = lbltypeid.Text;

                RadWindowManager1.RadConfirm("Are you sure you want to revise this RA?", "ConfirmRevision", 320, 150, null, "ConfirmRevision");
            }
            if (gce.CommandName.ToUpper().Equals("VIEWREVISION"))
            {
                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
            }
            if (gce.CommandName.ToUpper().Equals("RAGENERIC"))
            {
                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
            }

            if (gce.CommandName.ToUpper().Equals("RAGENERIC"))
            {
                string Tmpfilelocation = string.Empty; string[] reportfile = new string[0];

                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentGenericID");
                RadLabel lbltypeid = (RadLabel)gce.Item.FindControl("lblTypeid");                               

                if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
                {
                    string filename = "";
                    DataSet ds;

                    if (lbltypeid.Text.Equals("1"))
                    {
                        NameValueCollection nvc = new NameValueCollection();
                        nvc.Add("applicationcode", "9");
                        nvc.Add("reportcode", "RAGENERICNEW");
                        nvc.Add("CRITERIA", "");
                        Session["PHOENIXREPORTPARAMETERS"] = nvc;

                        ds = PhoenixInspectionRiskAssessmentGenericExtn.InspectionNewGenericRAReportExtn(new Guid(lbl.Text));

                        Tmpfilelocation = System.Web.HttpContext.Current.Request.MapPath("~/");
                        filename = "GENERICNEW_" + DateTime.Now.ToShortDateString().Replace("/", "_") + ".pdf";
                        Tmpfilelocation = HttpContext.Current.Server.MapPath("../Attachments/TEMP/" + filename);

                        PhoenixSsrsReportsCommon.getVersion();
                        PhoenixSsrsReportsCommon.getLogo();
                        PhoenixSSRSReportClass.ExportSSRSReport(reportfile, ds, PhoenixSSRSReportClass.ExportFileFormat.PDF, null, ref Tmpfilelocation);
                        Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + Tmpfilelocation + "&type=pdf");                        
                    }

                    if (lbltypeid.Text.Equals("2"))
                    {
                        NameValueCollection nvc = new NameValueCollection();
                        nvc.Add("applicationcode", "9");
                        nvc.Add("reportcode", "RANAVIGATIONNEW");
                        nvc.Add("CRITERIA", "");
                        Session["PHOENIXREPORTPARAMETERS"] = nvc;

                        ds = PhoenixInspectionRiskAssessmentGenericExtn.InspectionNewNavigationRAReportExtn(new Guid(lbl.Text));
                        filename = "NAVIGATIONNEW_" + DateTime.Now.ToShortDateString().Replace("/", "_") + ".pdf";
                        Tmpfilelocation = HttpContext.Current.Server.MapPath("../Attachments/TEMP/" + filename);

                        PhoenixSsrsReportsCommon.getVersion();
                        PhoenixSsrsReportsCommon.getLogo();
                        PhoenixSSRSReportClass.ExportSSRSReport(reportfile, ds, PhoenixSSRSReportClass.ExportFileFormat.PDF, null, ref Tmpfilelocation);
                        Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + Tmpfilelocation + "&type=pdf");
                    }

                    if (lbltypeid.Text.Equals("3"))
                    {
                        NameValueCollection nvc = new NameValueCollection();
                        nvc.Add("applicationcode", "9");
                        nvc.Add("reportcode", "RAMACHINERYNEW");
                        nvc.Add("CRITERIA", "");
                        Session["PHOENIXREPORTPARAMETERS"] = nvc;

                        ds = PhoenixInspectionRiskAssessmentGenericExtn.InspectionNewMachineryRAReportExtn(new Guid(lbl.Text));
                        filename = "MACHINERYNEW_" + DateTime.Now.ToShortDateString().Replace("/", "_") + ".pdf";
                        Tmpfilelocation = HttpContext.Current.Server.MapPath("../Attachments/TEMP/" + filename);

                        PhoenixSsrsReportsCommon.getVersion();
                        PhoenixSsrsReportsCommon.getLogo();
                        PhoenixSSRSReportClass.ExportSSRSReport(reportfile, ds, PhoenixSSRSReportClass.ExportFileFormat.PDF, null, ref Tmpfilelocation);
                        Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + Tmpfilelocation + "&type=pdf");
                    }

                    if (lbltypeid.Text.Equals("4"))
                    {
                        NameValueCollection nvc = new NameValueCollection();
                        nvc.Add("applicationcode", "9");
                        nvc.Add("reportcode", "RACARGONEW");
                        nvc.Add("CRITERIA", "");
                        Session["PHOENIXREPORTPARAMETERS"] = nvc;

                        ds = PhoenixInspectionRiskAssessmentGenericExtn.InspectionNewCargoRAReportExtn(new Guid(lbl.Text));
                        filename = "CARGONEW_" + DateTime.Now.ToShortDateString().Replace("/", "_") + ".pdf";
                        Tmpfilelocation = HttpContext.Current.Server.MapPath("../Attachments/TEMP/" + filename);

                        PhoenixSsrsReportsCommon.getVersion();
                        PhoenixSsrsReportsCommon.getLogo();
                        PhoenixSSRSReportClass.ExportSSRSReport(reportfile, ds, PhoenixSSRSReportClass.ExportFileFormat.PDF, null, ref Tmpfilelocation);
                        Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + Tmpfilelocation + "&type=pdf");
                    }
                }
            }

            if (gce.CommandName == RadGrid.FilterCommandName)
            {
                Pair filterPair = (Pair)gce.CommandArgument;
                ViewState["PAGENUMBER"] = 1;

                string daterange = gvRiskAssessment.MasterTableView.GetColumn("FLDINTENDEDWORKDATE").CurrentFilterValue.ToString();
                if (daterange != string.Empty)
                {
                    ViewState["FDATE"] = daterange.Split('~')[0];
                    ViewState["TDATE"] = daterange.Split('~')[1];
                }

                ViewState["txtRefNo"] = gvRiskAssessment.MasterTableView.GetColumn("FLDNUMBER").CurrentFilterValue;
                ViewState["JobActivity"] = gvRiskAssessment.MasterTableView.GetColumn("FLDACTIVITYCONDITIONS").CurrentFilterValue;
                SetFilter();
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

    private void SetFilter()
    {
        NameValueCollection criteria = new NameValueCollection();

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            ViewState["VesselID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        }

        criteria.Add("txtRefNo", ViewState["txtRefNo"].ToString());
        criteria.Add("ucVessel", ViewState["VesselID"].ToString());
        criteria.Add("ddlStatus", ViewState["ddlStatus"].ToString());
        criteria.Add("ddlRAType", ViewState["ddlRAType"].ToString());
        criteria.Add("JobActivity", ViewState["JobActivity"].ToString());
        criteria.Add("FromDate", ViewState["FDATE"].ToString());
        criteria.Add("ToDate", ViewState["TDATE"].ToString());

       InspectionFilter.CurrentNonRoutineRAFilter = criteria;
    }
    protected void btnConfirmApprove_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            if (ucCM.confirmboxvalue == 1)
            {
                if (ViewState["GENERICID"] != null && ViewState["GENERICID"].ToString() != "")
                {
                    if (ViewState["TYPE"].ToString() == "1")
                    {
                        PhoenixInspectionRiskAssessmentGenericExtn.ApproveGeneric(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["GENERICID"].ToString()));
                    }
                    if (ViewState["TYPE"].ToString() == "2")
                    {
                        PhoenixInspectionRiskAssessmentNavigationExtn.ApproveNavigation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["GENERICID"].ToString()));
                    }
                    if (ViewState["TYPE"].ToString() == "3")
                    {
                        PhoenixInspectionRiskAssessmentMachineryExtn.ApproveMachinery(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["GENERICID"].ToString()));
                    }
                    if (ViewState["TYPE"].ToString() == "4")
                    {
                        PhoenixInspectionRiskAssessmentCargoExtn.ApproveCargo(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["GENERICID"].ToString()));
                    }

                    ucStatus.Text = "Approved Successfully";
                    Rebind();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void btnConfirmIssue_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            if (ucCM.confirmboxvalue == 1)
            {
                if (ViewState["GENERICID"] != null && ViewState["GENERICID"].ToString() != "")
                {
                    if (ViewState["TYPE"].ToString() == "1")
                    {
                        PhoenixInspectionRiskAssessmentGenericExtn.IssueGeneric(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["GENERICID"].ToString()));
                    }
                    if (ViewState["TYPE"].ToString() == "2")
                    {
                        PhoenixInspectionRiskAssessmentNavigationExtn.IssueNavigation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["GENERICID"].ToString()));
                    }
                    if (ViewState["TYPE"].ToString() == "3")
                    {
                        PhoenixInspectionRiskAssessmentMachineryExtn.IssueMachinery(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["GENERICID"].ToString()));
                    }
                    if (ViewState["TYPE"].ToString() == "4")
                    {
                        PhoenixInspectionRiskAssessmentCargoExtn.IssueCargo(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["GENERICID"].ToString()));
                    }
                    ucStatus.Text = "Issued Successfully";
                    Rebind();
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
        Rebind();
    }
    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblRiskAssessmentGenericID = (RadLabel)gvRiskAssessment.Items[rowindex].FindControl("lblRiskAssessmentGenericID");
            if (lblRiskAssessmentGenericID != null)
            {
                Filter.CurrentSelectedGenericRA = lblRiskAssessmentGenericID.Text;
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
        gvRiskAssessment.SelectedIndexes.Clear();
        for (int i = 0; i < gvRiskAssessment.Items.Count; i++)
        {
            if (gvRiskAssessment.MasterTableView.Items[i].GetDataKeyValue("FLDRISKASSESSMENTID").ToString().Equals(Filter.CurrentSelectedGenericRA.ToString()))
            {
                gvRiskAssessment.MasterTableView.Items[i].Selected = true;
            }
        }
    }
    public void btnConfirmRevision_Click(object sender, EventArgs e)
    {
        try
        {
            
                if (ViewState["GENERICID"] != null && ViewState["GENERICID"].ToString() != "")
                {
                    if (ViewState["TYPE"].ToString() == "1")
                    {
                        PhoenixInspectionRiskAssessmentGenericExtn.ReviseGeneric(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                        new Guid(ViewState["GENERICID"].ToString()));
                    }
                    if (ViewState["TYPE"].ToString() == "2")
                    {
                        PhoenixInspectionRiskAssessmentNavigationExtn.ReviseNavigation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                        new Guid(ViewState["GENERICID"].ToString()));
                    }
                    if (ViewState["TYPE"].ToString() == "3")
                    {
                        PhoenixInspectionRiskAssessmentMachineryExtn.ReviseMachinery(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                        new Guid(ViewState["GENERICID"].ToString()));
                    }
                    if (ViewState["TYPE"].ToString() == "4")
                    {
                        PhoenixInspectionRiskAssessmentCargoExtn.ReviseCargo(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                        new Guid(ViewState["GENERICID"].ToString()));
                    }
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

    protected void ucVessel_DataBinding(object sender, EventArgs e)
    {
        DataSet dt = new DataSet();
        dt = PhoenixRegistersVessel.ListOwnerAssignedVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(null), General.GetNullableInteger(ViewState["COMPANYID"].ToString()), General.GetNullableInteger(null), 0);
        RadComboBox ucVessel = sender as RadComboBox;
        ucVessel.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
        ucVessel.DataSource = dt;

        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Tables[0].Columns["FLDVESSELID"];
        dt.Tables[0].PrimaryKey = keyColumns;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            ucVessel.Enabled = false;
            ViewState["VesselID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        }

        if (ViewState["VesselID"] != null && ViewState["VesselID"].ToString() != "")
        {
            if (dt.Tables[0].Rows.Contains(ViewState["VesselID"].ToString()))
            {
                ucVessel.SelectedValue = ViewState["VesselID"].ToString();
            }
        }

    }

    protected void ddlStatus_DataBinding(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();

        dt = PhoenixInspectionRiskAssessmentActivityExtn.ListNonRoutineRiskAssessmentStatus();
        RadComboBox ddlStatus = sender as RadComboBox;
        ddlStatus.DataSource = dt;
        ddlStatus.DataTextField = "FLDSTATUSNAME";
        ddlStatus.DataValueField = "FLDSTATUSID";
        ddlStatus.Items.Insert(0, new RadComboBoxItem("Select", string.Empty));
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDSTATUSID").CurrentFilterValue = e.Value;
        ViewState["ddlStatus"] = e.Value;

        SetFilter();
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    protected void ddlRAType_DataBinding(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();

        dt = PhoenixInspectionRiskAssessmentActivityExtn.ListNonRoutineRiskAssessmentType();

        RadComboBox ddlRAType = sender as RadComboBox;
        ddlRAType.DataSource = dt;
        ddlRAType.DataTextField = "FLDNAME";
        ddlRAType.DataValueField = "FLDCATEGORYID";
        ddlRAType.Items.Insert(0, new RadComboBoxItem("All", string.Empty));

    }

    protected void ddlRAType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDTYPE").CurrentFilterValue = e.Value;
        ViewState["ddlRAType"] = e.Value;

        SetFilter();
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }


    protected void btnstandardtemplateissue_Click(object sender, EventArgs e)
    {
        if (ViewState["GENERICID"] != null && ViewState["GENERICID"].ToString() != "")
        {
            PhoenixInspectionRiskAssessmentGenericExtn.MainFleetStandardIssue(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                  new Guid(ViewState["GENERICID"].ToString()), General.GetNullableInteger(ViewState["TYPE"].ToString()));
            ucStatus.Text = "Standard Template Issue Successfully";
            Rebind();
        }
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        if (ViewState["GENERICID"] != null && ViewState["GENERICID"].ToString() != "")
        {
            PhoenixInspectionRiskAssessmentGenericExtn.RACancel(new Guid(ViewState["GENERICID"].ToString()), General.GetNullableInteger(ViewState["TYPE"].ToString()));
            ucStatus.Text = "Cancelled Successfully";
            Rebind();
        }
    }

    protected void btnotused_Click(object sender, EventArgs e)
    {
        if (ViewState["GENERICID"] != null && ViewState["GENERICID"].ToString() != "")
        {
            PhoenixInspectionRiskAssessmentGenericExtn.RANotuse(new Guid(ViewState["GENERICID"].ToString()), General.GetNullableInteger(ViewState["TYPE"].ToString()));
            ucStatus.Text = "Updated Successfully";
            Rebind();
        }
    }

    protected void ucVessel_DataBinding_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDVESSELID").CurrentFilterValue = e.Value;
        ViewState["VesselID"] = e.Value;

        SetFilter();
        ViewState["PAGENUMBER"] = 1;
        Rebind();
        gvRiskAssessment.Rebind();
    }
}
