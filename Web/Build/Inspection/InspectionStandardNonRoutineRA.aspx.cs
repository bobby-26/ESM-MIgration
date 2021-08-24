using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using Telerik.Web.UI;
using System.Web;
using SouthNests.Phoenix.Reports;

public partial class InspectionStandardNonRoutineRA : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionStandardNonRoutineRA.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessment')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        //toolbar.AddFontAwesomeButton("../Inspection/InspectionStandardNonRoutineRA.aspx", "Find", "<i class=\"fas fa-search\"></i>", "SEARCH");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionStandardNonRoutineRA.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

        MenuGeneric.AccessRights = this.ViewState;
        MenuGeneric.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ucConfirmApprove.Attributes.Add("style", "display:none");
            ucConfirmRevision.Attributes.Add("style", "display:none");

            ViewState["REFID"] = "";
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["TYPE"] = "0";
            ViewState["COMPANYID"] = "";

            ViewState["txtRefNo"] = string.Empty;
            ViewState["txtActivity"] = string.Empty;
            ViewState["ddlRAType"] = string.Empty;
            ViewState["ddlCategory"] = string.Empty;
            ViewState["txtActivityCondition"] = string.Empty;

            gvRiskAssessment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            NameValueCollection nvcCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvcCompany.Get("QMS") != null && nvcCompany.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvcCompany.Get("QMS");
            }

            if (Filter.CurrentStandardTemplateRAFilter == null)
            {
                SetFilter();
            }

        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string vesselid = "";
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alColumns = { "FLDNUMBER", "FLDDATE", "FLDTYPE", "FLDACTIVITYCONDITIONS", "FLDREVISIONNO" };
        string[] alCaptions = { "Ref. No", "Prepared", "Type", "Activity / Conditions", "Revision No" };

        NameValueCollection nvc = Filter.CurrentStandardTemplateRAFilter;

        if (Filter.CurrentStandardTemplateRAFilter == null)
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

        if (nvc != null && General.GetNullableInteger(nvc["ucVessel"]) == null)
        {
            nvc["ucVessel"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        }

        DataSet ds = PhoenixInspectionRiskAssessmentGenericExtn.NonRoutineStandardRiskAssessmentSearch(
                     nvc.Get("txtRefNo") != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : General.GetNullableString(ViewState["txtRefNo"].ToString())
                    , gvRiskAssessment.CurrentPageIndex + 1
                    , gvRiskAssessment.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount
                    , sortexpression, sortdirection
                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                    , General.GetNullableInteger(null)
                    , nvc.Get("ddlRAType") != null ? General.GetNullableInteger(nvc["ddlRAType"]) : General.GetNullableInteger(ViewState["ddlRAType"].ToString())
                    , nvc.Get("txtActivityCondition") != null ? General.GetNullableString(nvc["txtActivityCondition"].ToString()) : General.GetNullableString(ViewState["txtActivityCondition"].ToString())
                    );

        General.SetPrintOptions("gvRiskAssessment", "Standard Template", alCaptions, alColumns, ds);

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
            string vesselid = "";
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            string[] alColumns = { "FLDNUMBER", "FLDDATE", "FLDTYPE", "FLDACTIVITYCONDITIONS", "FLDREVISIONNO" };
            string[] alCaptions = { "Ref. No", "Prepared", "Type", "Activity / Conditions", "Revision No" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentStandardTemplateRAFilter;

            if (Filter.CurrentStandardTemplateRAFilter == null)
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

            if (nvc != null && General.GetNullableInteger(nvc["ucVessel"]) == null)
            {
                nvc["ucVessel"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            }

            DataSet ds = PhoenixInspectionRiskAssessmentGenericExtn.NonRoutineStandardRiskAssessmentSearch(
                         nvc.Get("txtRefNo") != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : General.GetNullableString(ViewState["txtRefNo"].ToString())
                        , 1
                        , iRowCount
                        , ref iRowCount
                        , ref iTotalPageCount
                        , sortexpression, sortdirection
                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                        , General.GetNullableInteger(null)
                        , nvc.Get("ddlRAType") != null ? General.GetNullableInteger(nvc["ddlRAType"]) : General.GetNullableInteger(ViewState["ddlRAType"].ToString())
                        , nvc.Get("txtActivityCondition") != null ? General.GetNullableString(nvc["txtActivityCondition"].ToString()) : General.GetNullableString(ViewState["txtActivityCondition"].ToString())
                        );

            General.ShowExcel("Standard Template", ds.Tables[0], alColumns, alCaptions, null, null);

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
            ClearGridFilter(gvRiskAssessment.MasterTableView.GetColumn("FLDNUMBER"));

            ViewState["txtActivity"] = string.Empty;
            ClearGridFilter(gvRiskAssessment.MasterTableView.GetColumn("FLDACTIVITYNAME"));

            ViewState["ddlRAType"] = string.Empty;
            ClearGridFilter(gvRiskAssessment.MasterTableView.GetColumn("FLDTYPE"));

            ViewState["txtActivityCondition"] = string.Empty;
            ClearGridFilter(gvRiskAssessment.MasterTableView.GetColumn("FLDACTIVITYCONDITIONS"));

            ViewState["PAGENUMBER"] = 1;

            Filter.CurrentStandardTemplateRAFilter.Clear();
            SetFilter();
            Rebind();
        }
    }

    private void ClearGridFilter(GridColumn column)
    {
        column.ListOfFilterValues = null;
        column.CurrentFilterFunction = GridKnownFunction.NoFilter;
        column.CurrentFilterValue = string.Empty;
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

            LinkButton imgrevision = (LinkButton)e.Item.FindControl("imgrevision");
            LinkButton imgCopyTemplate = (LinkButton)e.Item.FindControl("imgCopyTemplate");
            LinkButton cmdRevision = (LinkButton)e.Item.FindControl("cmdRevision");
            RadLabel lblStatusid = (RadLabel)e.Item.FindControl("lblStatus");
            RadLabel lblIsCreatedByOffice = (RadLabel)e.Item.FindControl("lblIsCreatedByOffice");

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

            cmdRevision.Visible = false;
            if (cmdRevision != null && lblVesselid.Text == "0")
            {
                cmdRevision.Visible = true;
            }

            if (lblStatusid.Text == "9")
            {
                if (lblVesselid != null && lblVesselid.Text == "0" && drv["FLDACTIVEYN"].ToString() == "1" && PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                    imgrevision.Visible = true;
                else
                    imgrevision.Visible = false;

                imgCopyTemplate.Visible = true;
            }


            if (lblStatusid.Text == "1")
            {
                imgrevision.Visible = false;

                imgCopyTemplate.Visible = false;
            }

            else if (lblStatusid.Text == "4")
            {
                imgrevision.Visible = false;
                imgCopyTemplate.Visible = false;
            }
            else if (lblStatusid.Text == "5")
            {
                if (lblVesselid != null && lblVesselid.Text == "0" && drv["FLDACTIVEYN"].ToString() == "1" && PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                    imgrevision.Visible = true;
                else
                    imgrevision.Visible = false;

                imgCopyTemplate.Visible = true;
            }

            else if (lblStatusid.Text == "6")
            {
                if (lblVesselid != null && lblVesselid.Text == "0" && drv["FLDACTIVEYN"].ToString() == "1" && PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                    imgrevision.Visible = true;
                else
                    imgrevision.Visible = false;
                imgCopyTemplate.Visible = true;
            }

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
        }
    }

    protected void gvRiskAssessment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRiskAssessment.CurrentPageIndex + 1;
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
                RadLabel lblInstallcode = (RadLabel)gce.Item.FindControl("lblInstallcode");
                RadLabel lblIsCreatedByOffice = (RadLabel)gce.Item.FindControl("lblIsCreatedByOffice");
                RadLabel lbltypeid = (RadLabel)gce.Item.FindControl("lblTypeid");
                RadLabel lblvesselid = (RadLabel)gce.Item.FindControl("lblvesselid");

                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
                if (lbltypeid.Text == "1")
                {
                    Response.Redirect("../Inspection/InspectionRAGenericExtn.aspx?StandardRA=1&showall=1&genericid=" + lbl.Text + "&status=" + lblstatus.Text, false);
                }
                if (lbltypeid.Text == "2")
                {
                    Response.Redirect("../Inspection/InspectionRANavigationExtn.aspx?StandardRA=1&showall=1&navigationid=" + lbl.Text + "&status=" + lblstatus.Text, false);
                }
                if (lbltypeid.Text == "3")
                {
                    Response.Redirect("../Inspection/InspectionRAMachineryExtn.aspx?StandardRA=1&showall=1&machineryid=" + lbl.Text + "&status=" + lblstatus.Text, false);
                }
                if (lbltypeid.Text == "4")
                {
                    Response.Redirect("../Inspection/InspectionRACargoExtn.aspx?StandardRA=1&showall=1&genericid=" + lbl.Text + "&status=" + lblstatus.Text, false);
                }
            }

            if (gce.CommandName.ToUpper().Equals("STISSUE"))
            {
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentGenericID");
                RadLabel lblstatus = (RadLabel)gce.Item.FindControl("lblStatus");
                RadLabel lblInstallcode = (RadLabel)gce.Item.FindControl("lblInstallcode");
                RadLabel lblIsCreatedByOffice = (RadLabel)gce.Item.FindControl("lblIsCreatedByOffice");
                RadLabel lbltypeid = (RadLabel)gce.Item.FindControl("lblTypeid");
                RadLabel lblvesselid = (RadLabel)gce.Item.FindControl("lblvesselid");

                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
                if (lblInstallcode != null && lblInstallcode.Text == "0" && lblvesselid.Text == "0")
                {
                    PhoenixInspectionRiskAssessmentGenericExtn.MainFleetStandardIssue(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(lbl.Text), General.GetNullableInteger(lbltypeid.Text));
                    ucStatus.Text = "Standard Template Issue Successfully";
                }
            }
            if (gce.CommandName.ToUpper().Equals("ISSUE"))
            {
                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
            }
            if (gce.CommandName.ToUpper().Equals("REVISION"))
            {
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentGenericID");
                RadLabel lblstatus = (RadLabel)gce.Item.FindControl("lblStatus");
                RadLabel lblInstallcode = (RadLabel)gce.Item.FindControl("lblInstallcode");
                RadLabel lblIsCreatedByOffice = (RadLabel)gce.Item.FindControl("lblIsCreatedByOffice");
                RadLabel lbltypeid = (RadLabel)gce.Item.FindControl("lblTypeid");
                RadLabel lblvesselid = (RadLabel)gce.Item.FindControl("lblvesselid");

                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
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

            if (gce.CommandName == RadGrid.FilterCommandName)
            {
                Pair filterPair = (Pair)gce.CommandArgument;
                ViewState["PAGENUMBER"] = 1;

                ViewState["txtRefNo"] = gvRiskAssessment.MasterTableView.GetColumn("FLDNUMBER").CurrentFilterValue;
                ViewState["txtActivityCondition"] = gvRiskAssessment.MasterTableView.GetColumn("FLDACTIVITYCONDITIONS").CurrentFilterValue;

                SetFilter();
                gvRiskAssessment.Rebind();
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

            if (gce.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

    private void SetFilter()
    {
        NameValueCollection criteria = new NameValueCollection();

        criteria.Add("txtRefNo", ViewState["txtRefNo"].ToString());
        criteria.Add("txtActivity", ViewState["txtActivity"].ToString());
        criteria.Add("ddlRAType", ViewState["ddlRAType"].ToString());
        criteria.Add("ddlCategory", ViewState["ddlCategory"].ToString());
        criteria.Add("txtActivityCondition", ViewState["txtActivityCondition"].ToString());

        Filter.CurrentStandardTemplateRAFilter = criteria;
        gvRiskAssessment.Rebind();
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

    protected void ddlCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDACTIVITYNAME").CurrentFilterValue = e.Value;
        ViewState["ddlCategory"] = e.Value;

        SetFilter();
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    protected void ddlCategory_DataBinding(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();

        ds = PhoenixInspectionRiskAssessmentActivityExtn.ListRiskAssessmentActivity(null);

        if (ds.Tables[0].Rows.Count > 0)
        {
            RadComboBox ddlCategory = sender as RadComboBox;
            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new RadComboBoxItem("All", "Dummy"));
            ddlCategory.DataSource = ds.Tables[0];
            ddlCategory.DataTextField = "FLDNAME";
            ddlCategory.DataValueField = "FLDACTIVITYID";
        }
    }
}