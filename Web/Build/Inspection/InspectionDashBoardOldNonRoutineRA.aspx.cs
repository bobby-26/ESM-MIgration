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


public partial class InspectionDashBoardOldNonRoutineRA : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionDashBoardOldNonRoutineRA.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessment')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionDashBoardOldNonRoutineRA.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        //toolbar.AddFontAwesomeButton("../Inspection/InspectionDashBoardOldNonRoutineRA.aspx", "New Template", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

        MenuGeneric.AccessRights = this.ViewState;
        MenuGeneric.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ucConfirmApprove.Attributes.Add("style", "display:none");
            ucConfirmIssue.Attributes.Add("style", "display:none");
            ucConfirmRevision.Attributes.Add("style", "display:none");

            ViewState["REFID"] = "";
            ViewState["RAID"] = "";
            ViewState["PAGENUMBER"] = 1;
            ViewState["ROWCOUNT"] = "0";
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["TYPE"] = "0";
            ViewState["COMPANYID"] = "";
            ViewState["txtRefNo"] = string.Empty;
            ViewState["VesselID"] = string.Empty;
            ViewState["ddlRAType"] = string.Empty;
            ViewState["JobActivity"] = string.Empty;
            ViewState["FDATE"] = string.Empty;
            ViewState["TDATE"] = string.Empty;
            ViewState["Code"] = string.Empty;

            NameValueCollection nvcCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvcCompany.Get("QMS") != null && nvcCompany.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvcCompany.Get("QMS");
            }

            ViewState["VESSEL"] = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["vslid"]))
            {
                ViewState["VESSEL"] = Request.QueryString["vslid"];
            }

            if (Request.QueryString["STATUS"] != null && Request.QueryString["STATUS"].ToString() != string.Empty)
                ViewState["Code"] = Request.QueryString["STATUS"].ToString();

            gvRiskAssessment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDTYPE", "FLDACTIVITYCONDITIONS", "FLDREVISIONNO", "FLDSTATUSNAME", "FLDAPPROVEDBY" };
            string[] alCaptions = { "Ref Number", "Vessel", "Prepared", "Type", "Activity", "Revision No", "Status", "Approved By" };

            NameValueCollection Dashboardnvc = FilterDashboard.OfficeDashboardFilterCriteria;
            NameValueCollection nvc = InspectionFilter.CurrentDashboardNonRoutineRAFilter;

            DataSet ds = PhoenixInspectionRiskAssessmentGeneric.DashboardOldNonRoutineRASearch(
                        nvc != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : General.GetNullableString(ViewState["txtRefNo"].ToString())
                        , gvRiskAssessment.CurrentPageIndex + 1
                        , gvRiskAssessment.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount
                        , General.GetNullableString(ViewState["Code"].ToString())
                        , sortexpression, sortdirection
                        , nvc != null ? General.GetNullableDateTime(nvc["FromDate"]) : General.GetNullableDateTime(ViewState["FDATE"].ToString())
                        , nvc != null ? General.GetNullableDateTime(nvc["ToDate"]) : General.GetNullableDateTime(ViewState["TDATE"].ToString())                        
                        , General.GetNullableString(ViewState["VESSEL"].ToString() == string.Empty ? (nvc != null ? nvc["ucVessel"] : ViewState["VesselID"].ToString()) : ViewState["VESSEL"].ToString())
                        , nvc != null ? General.GetNullableString(nvc["ddlRAType"]) : General.GetNullableString(ViewState["ddlRAType"].ToString())
                        , nvc != null ? General.GetNullableString(nvc["JobActivity"]) : General.GetNullableString(ViewState["JobActivity"].ToString())
                        ,PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

            General.SetPrintOptions("gvRiskAssessment", "Non Routine RA", alCaptions, alColumns, ds);

            gvRiskAssessment.DataSource = ds;
            gvRiskAssessment.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

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
            string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDTYPE", "FLDACTIVITYCONDITIONS", "FLDREVISIONNO", "FLDSTATUSNAME", "FLDAPPROVEDBY" };
            string[] alCaptions = { "Ref Number", "Vessel", "Prepared", "Type", "Activity", "Revision No", "Status", "Approved By" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection Dashboardnvc = FilterDashboard.OfficeDashboardFilterCriteria;
            NameValueCollection nvc = InspectionFilter.CurrentDashboardNonRoutineRAFilter;

            DataSet ds = PhoenixInspectionRiskAssessmentGeneric.DashboardOldNonRoutineRASearch(nvc != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : General.GetNullableString(ViewState["txtRefNo"].ToString())
                        , 1
                        , iRowCount
                        , ref iRowCount
                        , ref iTotalPageCount
                        , General.GetNullableString(ViewState["Code"].ToString())
                        , sortexpression, sortdirection
                        , nvc != null ? General.GetNullableDateTime(nvc["FromDate"]) : General.GetNullableDateTime(ViewState["FDATE"].ToString())
                        , nvc != null ? General.GetNullableDateTime(nvc["ToDate"]) : General.GetNullableDateTime(ViewState["TDATE"].ToString())
                        , General.GetNullableString(ViewState["VESSEL"].ToString() == string.Empty ? (nvc != null ? nvc["ucVessel"] : ViewState["VesselID"].ToString()) : ViewState["VESSEL"].ToString())
                        , nvc != null ? General.GetNullableString(nvc["ddlRAType"]) : General.GetNullableString(ViewState["ddlRAType"].ToString())
                        , nvc != null ? General.GetNullableString(nvc["JobActivity"]) : General.GetNullableString(ViewState["JobActivity"].ToString())
                        , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
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

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }

        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            ViewState["txtRefNo"] = string.Empty;
            ViewState["VesselID"] = string.Empty;
            ViewState["ddlRAType"] = string.Empty;
            ViewState["JobActivity"] = string.Empty;
            ViewState["FDATE"] = string.Empty;
            ViewState["TDATE"] = string.Empty;

            ViewState["PAGENUMBER"] = 1;

            InspectionFilter.CurrentDashboardNonRoutineRAFilter.Clear();
            SetFilter();
            Rebind();
        }
    }

    protected void gvRiskAssessment_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                RadLabel lblGenericID = (RadLabel)e.Item.FindControl("lblRiskAssessmentGenericID");
                RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselid");
                RadLabel lblInstallcode = (RadLabel)e.Item.FindControl("lblInstallcode");
                RadLabel lbltypeid = (RadLabel)e.Item.FindControl("lblTypeid");

                LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
                if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

                LinkButton imgApprove = (LinkButton)e.Item.FindControl("imgApprove");
                LinkButton imgIssue = (LinkButton)e.Item.FindControl("imgIssue");
                LinkButton imgrevision = (LinkButton)e.Item.FindControl("imgrevision");
                LinkButton imgCopyTemplate = (LinkButton)e.Item.FindControl("imgCopyTemplate");
                LinkButton cmdRevision = (LinkButton)e.Item.FindControl("cmdRevision");
                RadLabel lblStatusid = (RadLabel)e.Item.FindControl("lblStatus");
                LinkButton imgProposeTemplate = (LinkButton)e.Item.FindControl("imgProposeTemplate");
                RadLabel lblIsCreatedByOffice = (RadLabel)e.Item.FindControl("lblIsCreatedByOffice");

                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                {
                    imgProposeTemplate.Visible = true;
                }
                else
                {
                    imgProposeTemplate.Visible = false;
                }

                if (lblInstallcode != null)
                {
                    if (General.GetNullableInteger(lblInstallcode.Text) != null && int.Parse(lblInstallcode.Text) > 0) // when RA is approved in vessel.
                    {
                        if (imgApprove != null)
                        {
                            imgApprove.ToolTip = "Emergency Override";

                            if (lbltypeid.Text == "1")
                            {
                                imgApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApprovalRemarks.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=1','medium'); return true;");
                            }
                            if (lbltypeid.Text == "2")
                            {
                                imgApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApprovalRemarks.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=2','medium'); return true;");
                            }
                            if (lbltypeid.Text == "3")
                            {
                                imgApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApprovalRemarks.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=3','medium'); return true;");
                            }
                            if (lbltypeid.Text == "4")
                            {
                                imgApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApprovalRemarks.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=5','medium'); return true;");
                            }
                        }
                    }
                    else if (General.GetNullableInteger(lblInstallcode.Text) != null && int.Parse(lblInstallcode.Text) == 0) // when vessel created RA approved in office.
                    {
                        if (lblIsCreatedByOffice != null && lblIsCreatedByOffice.Text != "" && int.Parse(lblIsCreatedByOffice.Text) > 0)
                        {
                            if (imgApprove != null)
                            {
                                if (lbltypeid.Text == "1")
                                {
                                    imgApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApproval.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=1','large'); return true;");
                                }
                                if (lbltypeid.Text == "2")
                                {
                                    imgApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApproval.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=2','large'); return true;");
                                }
                                if (lbltypeid.Text == "3")
                                {
                                    imgApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApproval.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=3','large'); return true;");
                                }
                                if (lbltypeid.Text == "4")
                                {
                                    imgApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionMainFleetRAApproval.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=5','large'); return true;");
                                }
                            }
                        }
                    }
                }

                if (imgCopyTemplate != null)
                {
                    if (lbltypeid.Text == "1")
                    {
                        imgCopyTemplate.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAGeneric.aspx?genericid=" + lblGenericID.Text + "&CopyType=1&showall=1" + "'); return true;");
                    }
                    if (lbltypeid.Text == "2")
                    {
                        imgCopyTemplate.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRANavigation.aspx?navigationid=" + lblGenericID.Text + "&CopyType=1&showall=1" + "'); return true;");
                    }
                    if (lbltypeid.Text == "3")
                    {
                        imgCopyTemplate.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAMachinery.aspx?machineryid=" + lblGenericID.Text + "&CopyType=1&showall=1" + "'); return true;");
                    }
                    if (lbltypeid.Text == "4")
                    {
                        imgCopyTemplate.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRACargo.aspx?genericid=" + lblGenericID.Text + "&CopyType=1&showall=1" + "'); return true;");
                    }
                }

                if (imgProposeTemplate != null)
                {
                    if (lbltypeid.Text == "1")
                    {
                        imgProposeTemplate.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAGeneric.aspx?genericid=" + lblGenericID.Text + "&CopyType=2&showall=1" + "'); return true;");
                    }
                    if (lbltypeid.Text == "2")
                    {
                        imgProposeTemplate.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRANavigation.aspx?navigationid=" + lblGenericID.Text + "&CopyType=2&showall=1" + "'); return true;");
                    }
                    if (lbltypeid.Text == "3")
                    {
                        imgProposeTemplate.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAMachinery.aspx?machineryid=" + lblGenericID.Text + "&CopyType=2&showall=1" + "'); return true;");
                    }
                    if (lbltypeid.Text == "4")
                    {
                        imgProposeTemplate.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRACargo.aspx?genericid=" + lblGenericID.Text + "&CopyType=2&showall=1" + "'); return true;");
                    }
                }

                //imgCopy.Visible = false;
                imgrevision.Visible = false;
                imgApprove.Visible = false;
                imgIssue.Visible = false;
                imgCopyTemplate.Visible = false;

                LinkButton imgComparison = (LinkButton)e.Item.FindControl("imgComparison");
                imgComparison.Visible = false;
                Image imgFlag = e.Item.FindControl("imgFlag") as Image;
                if (imgFlag != null && drv["FLDINTENDEDWORKDATEDUEYN"].ToString().Equals("3"))
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/" + "red-symbol.png";
                    imgFlag.ToolTip = "Overdue";
                }
                else if (imgFlag != null && drv["FLDINTENDEDWORKDATEDUEYN"].ToString().Equals("2"))
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/" + "yellow-symbol.png";
                    imgFlag.ToolTip = "Due within 2 Weeks";
                }
                else
                {
                    if (imgFlag != null) imgFlag.Visible = false;
                }

                Image imgOfficeFlag = (Image)e.Item.FindControl("imgOfficeFlag");

                if (drv["FLDCOPIEDFROMSTANDARDTEMPLATE"].ToString() == "1")
                {
                    imgOfficeFlag.Visible = true;
                    imgOfficeFlag.ImageUrl = Session["images"] + "/" + "green.png";
                    imgOfficeFlag.ToolTip = "Copied from Standard Template";

                    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                    {
                        imgComparison.Visible = true;
                    }
                }

                if (imgComparison != null)
                {
                    if (lbltypeid.Text == "1")
                    {
                        imgComparison.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAGenericComparison.aspx?genericid=" + lblGenericID.Text + "'); return true;");
                    }

                    if (lbltypeid.Text == "2")
                    {
                        imgComparison.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRANavigationComparison.aspx?navigationid=" + lblGenericID.Text + "'); return true;");
                    }

                    if (lbltypeid.Text == "3")
                    {
                        imgComparison.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAMachineryComparison.aspx?machneryid=" + lblGenericID.Text + "'); return true;");
                    }

                    if (lbltypeid.Text == "4")
                    {
                        imgComparison.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRACargoComparison.aspx?cargoid=" + lblGenericID.Text + "'); return true;");
                    }
                }

                cmdRevision.Visible = false;
                if (cmdRevision != null && lblVesselid.Text == "0")
                {
                    cmdRevision.Visible = true;
                }

                if (lblStatusid.Text == "1")
                {
                    //imgCopy.Visible = false;
                    imgrevision.Visible = false;
                    imgApprove.Visible = true;
                    imgIssue.Visible = false;
                    imgCopyTemplate.Visible = false;
                }

                else if (lblStatusid.Text == "4")
                {
                    //imgCopy.Visible = false;
                    imgrevision.Visible = false;
                    imgApprove.Visible = true;
                    imgIssue.Visible = false;
                    imgCopyTemplate.Visible = false;
                    if (lblVesselid.Text == "0")
                    {
                        imgIssue.Visible = true;
                        imgApprove.Visible = false;
                    }
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
                    imgCopyTemplate.Visible = true;
                }

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    if (imgCopyTemplate != null) imgCopyTemplate.ToolTip = "Use as RA";
                }
                else
                {
                    if (imgCopyTemplate != null) imgCopyTemplate.ToolTip = "Copy RA";
                }

                if (lblVesselid != null && lblVesselid.Text == "0")
                {
                    if (imgApprove != null) imgApprove.ToolTip = "Request for Approval";
                }
                else
                {
                    if (imgApprove != null) imgApprove.ToolTip = "Approve/Reject";
                }

                if ((imgIssue != null) && (lblVesselid.Text == "0"))
                {
                    if (lbltypeid.Text == "1")
                    {
                        imgIssue.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAMainFleetOfficeApprovalRemarks.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=1','medium'); return true;");
                    }
                    if (lbltypeid.Text == "2")
                    {
                        imgIssue.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAMainFleetOfficeApprovalRemarks.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=2','medium'); return true;");
                    }
                    if (lbltypeid.Text == "3")
                    {
                        imgIssue.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAMainFleetOfficeApprovalRemarks.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=3','medium'); return true;");
                    }
                    if (lbltypeid.Text == "4")
                    {
                        imgIssue.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRAMainFleetOfficeApprovalRemarks.aspx?RATEMPLATEID=" + lblGenericID.Text + "&TYPE=5','medium'); return true;");
                    }
                }

                LinkButton cmdRAGeneric = (LinkButton)e.Item.FindControl("cmdRAGeneric");
                if (cmdRAGeneric != null)
                {
                    cmdRAGeneric.Visible = SessionUtil.CanAccess(this.ViewState, cmdRAGeneric.CommandName);
                    if (lbltypeid.Text == "1")
                    {
                        cmdRAGeneric.Attributes.Add("onclick", "javascript:openNewWindow('RAGeneric', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAGENERIC&genericid=" + lblGenericID.Text + "&showmenu=0&showexcel=NO');return true;");
                    }
                    if (lbltypeid.Text == "2")
                    {
                        cmdRAGeneric.Attributes.Add("onclick", "javascript:openNewWindow('RAGeneric', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RANAVIGATION&navigationid=" + lblGenericID.Text + "&showmenu=0&showexcel=NO');return true;");
                    }
                    if (lbltypeid.Text == "3")
                    {
                        cmdRAGeneric.Attributes.Add("onclick", "javascript:openNewWindow('RAGeneric', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + lblGenericID.Text + "&showmenu=0&showexcel=NO');return true;");
                    }
                    if (lbltypeid.Text == "4")
                    {
                        cmdRAGeneric.Attributes.Add("onclick", "javascript:openNewWindow('RAGeneric', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RACARGO&genericid=" + lblGenericID.Text + "&showmenu=0&showexcel=NO');return true;");
                    }
                }

                if (cmdRevision != null)
                {
                    if (lbltypeid.Text == "1")
                    {
                        cmdRevision.Attributes.Add("onclick", "javascript:openNewWindow('RAGenericRevision', '', '" + Session["sitepath"] + "/Inspection/InspectionRAGenericRevisionList.aspx?genericid=" + lblGenericID.Text + "');return true;");
                    }
                    if (lbltypeid.Text == "2")
                    {
                        cmdRevision.Attributes.Add("onclick", "javascript:openNewWindow('RAGenericRevision', '', '" + Session["sitepath"] + "/Inspection/InspectionRANavigationRevisionList.aspx?navigationid=" + lblGenericID.Text + "');return true;");
                    }
                    if (lbltypeid.Text == "3")
                    {
                        cmdRevision.Attributes.Add("onclick", "javascript:openNewWindow('RAGenericRevision', '', '" + Session["sitepath"] + "/Inspection/InspectionRAMachineryRevisionList.aspx?machineryid=" + lblGenericID.Text + "');return true;");
                    }
                    if (lbltypeid.Text == "4")
                    {
                        cmdRevision.Attributes.Add("onclick", "javascript:openNewWindow('RAGenericRevision', '', '" + Session["sitepath"] + "/Inspection/InspectionRACargoRevisionList.aspx?genericid=" + lblGenericID.Text + "');return true;");
                    }
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
                //if (imgrevision != null)
                //{
                //    if (!SessionUtil.CanAccess(this.ViewState, imgrevision.CommandName)) imgrevision.Visible = false;
                //}
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
                if (imgProposeTemplate != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, imgProposeTemplate.CommandName)) imgProposeTemplate.Visible = false;
                }

                RadLabel lbl = (RadLabel)e.Item.FindControl("lblRiskAssessmentGenericID");
                RadLabel lblstatus = (RadLabel)e.Item.FindControl("lblStatus");

                if (ed !=null)
                {
                    if (lbltypeid.Text == "1")
                    {
                        ed.Attributes.Add("onclick", "javascript:openNewWindow('RAGeneric', '', '" + Session["sitepath"] + "/Inspection/InspectionRAGeneric.aspx?showall=1&Dashboardyn=1&genericid=" + lbl.Text + "&status=" + lblstatus.Text+"');return true;");
                    }
                    if (lbltypeid.Text == "2")
                    {
                        ed.Attributes.Add("onclick", "javascript:openNewWindow('RANavigation', '', '" + Session["sitepath"] + "/Inspection/InspectionRANavigation.aspx?showall=1&Dashboardyn=1&navigationid=" + lbl.Text + "&status=" + lblstatus.Text + "');return true;");
                    }
                    if (lbltypeid.Text == "3")
                    {
                        ed.Attributes.Add("onclick", "javascript:openNewWindow('RAMachinery', '', '" + Session["sitepath"] + "/Inspection/InspectionRAMachinery.aspx?showall=1&Dashboardyn=1&machineryid=" + lbl.Text + "&status=" + lblstatus.Text + "');return true;");
                    }
                    if (lbltypeid.Text == "4")
                    {
                        ed.Attributes.Add("onclick", "javascript:openNewWindow('RACargo', '', '" + Session["sitepath"] + "/Inspection/InspectionRACargo.aspx?showall=1&Dashboardyn=1&genericid=" + lbl.Text + "&status=" + lblstatus.Text + "');return true;");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
            return;
        }
    }

    protected void gvRiskAssessment_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        try
        {
            if (gce.Item is GridEditableItem)
            {
                RadGrid _gridView = (RadGrid)sender;
                int nRow = gce.Item.ItemIndex;

                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentGenericID");
                RadLabel lblstatus = (RadLabel)gce.Item.FindControl("lblStatus");
                RadLabel lblInstallcode = (RadLabel)gce.Item.FindControl("lblInstallcode");
                RadLabel lblIsCreatedByOffice = (RadLabel)gce.Item.FindControl("lblIsCreatedByOffice");
                RadLabel lbltypeid = (RadLabel)gce.Item.FindControl("lblTypeid");
                RadLabel lblvesselid = (RadLabel)gce.Item.FindControl("lblvesselid");

                if (gce.CommandName.ToUpper().Equals("EDITROW"))
                {
                    RadLabel lbltypeid1 = (RadLabel)gce.Item.FindControl("lblTypeid");
                    ViewState["RAID"] = lbl.Text;
                    ViewState["GENERICID"] = lbl.Text;
                    ViewState["STATUS"] = lblstatus.Text;
                    ViewState["INSTALLCODE"] = lblInstallcode.Text;
                    ViewState["TYPEID"] = lbltypeid1.Text;

                    BindPageURL(nRow);
                    //SetRowSelection();
                    

                }
                if (gce.CommandName.ToUpper().Equals("APPROVE"))
                {
                    BindPageURL(nRow);
                    // SetRowSelection();
                    if (lblInstallcode != null && lblInstallcode.Text == "0" && lblvesselid.Text == "0")
                    {
                        if (lbltypeid.Text == "1")
                        {
                            PhoenixInspectionRiskAssessmentGeneric.MainFleetApproveGeneric(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                           new Guid(lbl.Text));
                            ucStatus.Text = "Approved Successfully";
                        }
                        if (lbltypeid.Text == "2")
                        {
                            PhoenixInspectionRiskAssessmentNavigation.MainFleetApproveNavigation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                           new Guid(lbl.Text));
                            ucStatus.Text = "Approved Successfully";
                        }
                        if (lbltypeid.Text == "3")
                        {
                            PhoenixInspectionRiskAssessmentMachinery.MainFleetApproveMachinery(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                           new Guid(lbl.Text));
                            ucStatus.Text = "Approved Successfully";
                        }
                        if (lbltypeid.Text == "4")
                        {
                            PhoenixInspectionRiskAssessmentCargo.MainFleetApproveCargo(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                           new Guid(lbl.Text));
                            ucStatus.Text = "Approved Successfully";
                        }
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

        if (ViewState["VESSEL"].ToString()!= string.Empty)
        {
            ViewState["VesselID"] = ViewState["VESSEL"].ToString();
        }

        criteria.Add("txtRefNo", ViewState["txtRefNo"].ToString());
        criteria.Add("ucVessel", ViewState["VesselID"].ToString());
        criteria.Add("ddlRAType", ViewState["ddlRAType"].ToString());
        criteria.Add("JobActivity", ViewState["JobActivity"].ToString());
        criteria.Add("FromDate", ViewState["FDATE"].ToString());
        criteria.Add("ToDate", ViewState["TDATE"].ToString());

        InspectionFilter.CurrentDashboardNonRoutineRAFilter = criteria;
    }
    protected void btnConfirmApprove_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["GENERICID"] != null && ViewState["GENERICID"].ToString() != "")
            {
                if (ViewState["TYPE"].ToString() == "1")
                {
                    PhoenixInspectionRiskAssessmentGeneric.ApproveGeneric(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["GENERICID"].ToString()));
                }
                if (ViewState["TYPE"].ToString() == "2")
                {
                    PhoenixInspectionRiskAssessmentNavigation.ApproveNavigation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["GENERICID"].ToString()));
                }
                if (ViewState["TYPE"].ToString() == "3")
                {
                    PhoenixInspectionRiskAssessmentMachinery.ApproveMachinery(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["GENERICID"].ToString()));
                }
                if (ViewState["TYPE"].ToString() == "4")
                {
                    PhoenixInspectionRiskAssessmentCargo.ApproveCargo(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["GENERICID"].ToString()));
                }

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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    private void BindPageURL(int rowindex)
    {
        try
        {
            //RadLabel lblRiskAssessmentGenericID = (RadLabel)gvRiskAssessment.Items[rowindex].FindControl("lblRiskAssessmentGenericID");
            //if (lblRiskAssessmentGenericID != null)
            //{
            //    Filter.CurrentSelectedGenericRA = lblRiskAssessmentGenericID.Text;
            //}
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
            if (ViewState["GENERICID"] != null && ViewState["GENERICID"].ToString() != "")
            {
                if (ViewState["TYPE"].ToString() == "1")
                {
                    PhoenixInspectionRiskAssessmentGeneric.IssueGeneric(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["GENERICID"].ToString()));
                }
                if (ViewState["TYPE"].ToString() == "2")
                {
                    PhoenixInspectionRiskAssessmentNavigation.IssueNavigation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["GENERICID"].ToString()));
                }
                if (ViewState["TYPE"].ToString() == "3")
                {
                    PhoenixInspectionRiskAssessmentMachinery.IssueMachinery(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["GENERICID"].ToString()));
                }
                if (ViewState["TYPE"].ToString() == "4")
                {
                    PhoenixInspectionRiskAssessmentCargo.IssueCargo(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["GENERICID"].ToString()));
                }
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
    protected void btnConfirmRevision_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["GENERICID"] != null && ViewState["GENERICID"].ToString() != "")
            {
                if (ViewState["TYPE"].ToString() == "1")
                {
                    PhoenixInspectionRiskAssessmentGeneric.ReviseGeneric(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                    new Guid(ViewState["GENERICID"].ToString()));
                }
                if (ViewState["TYPE"].ToString() == "2")
                {
                    PhoenixInspectionRiskAssessmentNavigation.ReviseNavigation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                    new Guid(ViewState["GENERICID"].ToString()));
                }
                if (ViewState["TYPE"].ToString() == "3")
                {
                    PhoenixInspectionRiskAssessmentMachinery.ReviseMachinery(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                    new Guid(ViewState["GENERICID"].ToString()));
                }
                if (ViewState["TYPE"].ToString() == "4")
                {
                    PhoenixInspectionRiskAssessmentCargo.ReviseCargo(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
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
        dt = PhoenixRegistersVessel.ListOwnerAssignedVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(null), General.GetNullableInteger(ViewState["COMPANYID"].ToString()), General.GetNullableInteger(null), 1);
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

        if (ViewState["VESSEL"].ToString() != string.Empty)
        {
            ViewState["VesselID"] = ViewState["VESSEL"].ToString();
            ucVessel.Enabled = false;
        }

        if (ViewState["VesselID"] != null && ViewState["VesselID"].ToString() != "")
        {
            if (dt.Tables[0].Rows.Contains(ViewState["VesselID"].ToString()))
            {
                ucVessel.SelectedValue = ViewState["VesselID"].ToString();
            }
        }

    }
    protected void ddlRAType_DataBinding(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();

        dt = PhoenixInspectionRiskAssessmentActivity.ListNonRoutineRiskAssessmentType();
        RadComboBox ddlRAType = sender as RadComboBox;
        ddlRAType.DataSource = dt;
        ddlRAType.DataTextField = "FLDNAME";
        ddlRAType.DataValueField = "FLDCODE";
        ddlRAType.Items.Insert(0, new RadComboBoxItem("--Select--", ""));

    }

    protected void ddlRAType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDTYPECODE").CurrentFilterValue = e.Value;
        ViewState["ddlRAType"] = e.Value;

        SetFilter();
        ViewState["PAGENUMBER"] = 1;
        Rebind();

    }
    private void SetRowSelection()
    {
        try
        {
            //gvRiskAssessment.SelectedIndexes.Clear();
            //for (int i = 0; i < gvRiskAssessment.Items.Count; i++)
            //{
            //    if (gvRiskAssessment.MasterTableView.Items[i].GetDataKeyValue("FLDRISKASSESSMENTID").ToString().Equals(Filter.CurrentSelectedGenericRA.ToString()))
            //    {
            //        gvRiskAssessment.MasterTableView.Items[i].Selected = true;
            //    }
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
    }
}
