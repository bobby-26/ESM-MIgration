using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.DocumentManagement;
using System.Collections.Specialized;
using System.Web;
using SouthNests.Phoenix.Reports;
using System.Web.UI.HtmlControls;

public partial class DocumentManagementRAProcessListNew : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementRAProcessListNew.aspx", "Find", "<i class=\"fas fa-search\"></i>", "SEARCH");
        toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementRAProcessListNew.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuProcess.AccessRights = this.ViewState;
        MenuProcess.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["REFNO"] = "";
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["NEWDMSYN"] = 0;
            ucVessel.bind();
            txtRefNo.Text = "";
            ucVessel.SelectedVessel = string.Empty;
            lbltypes.Visible = false;
            ddlType.Visible = false;
            txtActivity.Visible = false;
            lblactivityconditions.Visible = false;

            BindType();
            BindCategory();

            int installcode = PhoenixSecurityContext.CurrentSecurityContext.InstallCode;

            if (installcode > 0)
            {
                ucVessel.Enabled = false;
                ucVessel.SelectedValue = installcode;
            }

            gvRiskAssessmentProcess.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alColumns = { "FLDNUMBER", "FLDDATE", "FLDTYPE", "FLDPROCESSNAME", "FLDJOBACTIVITY", "FLDSTATUSNAME", "FLDREVISIONNO", "FLDISSUEDDATE", "FLDEDITEDBY" };
        string[] alCaptions = { "Number", "Date", "Type", "Category", " Process", "Status", "Rev No", "Issued Date", "Edited By" };

        int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        DataSet ds = new DataSet();
        if (rblRAType.SelectedValue == "0")
        {
            ds = PhoenixDocumentManagementDocument.InspectionRoutineRASearchExtn(
                    int.Parse(ViewState["PAGENUMBER"].ToString())
                   , gvRiskAssessmentProcess.PageSize
                   , ref iRowCount
                   , ref iTotalPageCount
                   , sortexpression
                   , sortdirection
                   , General.GetNullableString(txtRefNo.Text)
                   , companyid
                   , General.GetNullableInteger(ucVessel.SelectedVessel)
                   , General.GetNullableInteger(ddlType.SelectedValue.ToString())
                   , General.GetNullableString(txtActivity.Text)
                   , General.GetNullableInteger(ddlCategory.SelectedValue)
                   );
            gvRiskAssessmentProcess.DataSource = ds;
            gvRiskAssessmentProcess.Columns[2].Visible = false;
            gvRiskAssessmentProcess.Columns[3].HeaderText = "Process";
            gvRiskAssessmentProcess.Columns[4].HeaderText = "Activity";
            gvRiskAssessmentProcess.Columns[5].Visible = false;
            gvRiskAssessmentProcess.Columns[6].Visible = false;
            gvRiskAssessmentProcess.Columns[10].Visible = true;
            gvRiskAssessmentProcess.Columns[11].Visible = true;
            gvRiskAssessmentProcess.Columns[12].Visible = true;
            gvRiskAssessmentProcess.Columns[13].Visible = true;
            gvRiskAssessmentProcess.Columns[14].Visible = true;
            gvRiskAssessmentProcess.Columns[15].Visible = true;
        }
        else
        {
            ds = PhoenixDocumentManagementDocument.InspectionNonRoutineRASearchExtn(
                     int.Parse(ViewState["PAGENUMBER"].ToString())
                    , gvRiskAssessmentProcess.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount
                    , sortexpression
                    , sortdirection
                    , General.GetNullableString(txtRefNo.Text)
                    , companyid
                    , General.GetNullableInteger(ucVessel.SelectedVessel)
                    , General.GetNullableInteger(ddlType.SelectedValue.ToString())
                    , General.GetNullableString(txtActivity.Text)
                    );
            gvRiskAssessmentProcess.DataSource = ds;
            gvRiskAssessmentProcess.Columns[3].HeaderText = "Type";
            gvRiskAssessmentProcess.Columns[4].HeaderText = "Activity / Conditions";
            gvRiskAssessmentProcess.Columns[10].Visible = false;
            gvRiskAssessmentProcess.Columns[11].Visible = false;
            gvRiskAssessmentProcess.Columns[12].Visible = false;
            gvRiskAssessmentProcess.Columns[13].Visible = false;
            gvRiskAssessmentProcess.Columns[14].Visible = false;
            gvRiskAssessmentProcess.Columns[15].Visible = false;
            gvRiskAssessmentProcess.Columns[5].Visible = true;
            gvRiskAssessmentProcess.Columns[6].Visible = true;
        }


        gvRiskAssessmentProcess.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void Rebind()
    {
        gvRiskAssessmentProcess.SelectedIndexes.Clear();
        gvRiskAssessmentProcess.EditIndexes.Clear();
        gvRiskAssessmentProcess.DataSource = null;
        gvRiskAssessmentProcess.Rebind();
    }

    protected void BindCategory()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentActivityExtn.ListRiskAssessmentActivity(null);
        ddlCategory.Items.Clear();
        ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        ddlCategory.DataSource = ds.Tables[0];
        ddlCategory.DataBind();
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

            string[] alColumns = { "FLDNUMBER", "FLDDATE", "FLDTYPE", "FLDPROCESSNAME", "FLDJOBACTIVITY", "FLDSTATUSNAME", "FLDREVISIONNO", "FLDISSUEDDATE", "FLDEDITEDBY" };
            string[] alCaptions = { "Number", "Date", "Type", "Category", " Process", "Status", "Rev No", "Issued Date", "Edited By" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());



            int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

            DataSet ds = new DataSet();
            if (rblRAType.SelectedValue == "1")
            {
                ds = PhoenixDocumentManagementDocument.InspectionRoutineRASearchExtn(
                        int.Parse(ViewState["PAGENUMBER"].ToString())
                       , gvRiskAssessmentProcess.PageSize
                       , ref iRowCount
                       , ref iTotalPageCount
                       , sortexpression
                       , sortdirection
                       , General.GetNullableString(txtRefNo.Text)
                       , companyid
                       , General.GetNullableInteger(ucVessel.SelectedVessel)
                       , General.GetNullableInteger(ddlType.SelectedValue.ToString())
                       , General.GetNullableString(txtActivity.Text)
                       , General.GetNullableInteger(ddlCategory.SelectedValue)
                       );
            }
            else
            {
                ds = PhoenixDocumentManagementDocument.InspectionNonRoutineRASearchExtn(
                         int.Parse(ViewState["PAGENUMBER"].ToString())
                        , gvRiskAssessmentProcess.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount
                        , sortexpression
                        , sortdirection
                        , General.GetNullableString(txtRefNo.Text)
                        , companyid
                        , General.GetNullableInteger(ucVessel.SelectedVessel)
                        , General.GetNullableInteger(ddlType.SelectedValue.ToString())
                        , General.GetNullableString(txtActivity.Text)
                        );
            }

            General.ShowExcel("Risk Assessment-Process", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvRiskAssessmentProcess_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRiskAssessmentProcess.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuProcess_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            if (rblRAType.SelectedValue == "1")
            {
                rblRAType.SelectedValue = "1";
                ShowExcel();
            }
            else
            {
                rblRAType.SelectedValue = "0";
                ShowExcel();
            }
        }
        else if (CommandName.ToUpper().Equals("SEARCH"))
        {
            if (rblRAType.SelectedValue == "1")
            {
                rblRAType.SelectedValue = "1";
                gvRiskAssessmentProcess.Rebind();
            }
            else
            {
                rblRAType.SelectedValue = "0";
                gvRiskAssessmentProcess.Rebind();
            }
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            ViewState["PAGENUMBER"] = 1;
            txtRefNo.Text = "";
            txtActivity.Text = "";
            ddlType.ClearSelection();
            ddlCategory.ClearSelection();
            ucVessel.SelectedVessel = string.Empty;
            BindData();
            gvRiskAssessmentProcess.Rebind();
        }
    }

    protected void ucType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvRiskAssessmentProcess.Rebind();
    }

    protected void ucVessel_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvRiskAssessmentProcess.Rebind();
    }

    protected void rblRAType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblRAType.SelectedValue == "1")
        {
            ViewState["PAGENUMBER"] = 1;
            txtRefNo.Text = "";
            lbltypes.Visible = true;
            ddlType.Visible = true;
            ucVessel.SelectedVessel = string.Empty;
            rblRAType.SelectedValue = "1";
            txtActivity.Visible = true;
            lblactivityconditions.Visible = true;
            //BindData();
            gvRiskAssessmentProcess.Rebind();
            lblCategory.Visible = false;
            ddlCategory.Visible = false;
        }
        else
        {
            ViewState["PAGENUMBER"] = 1;
            txtRefNo.Text = "";
            lbltypes.Visible = false;
            ddlType.Visible = false;
            ddlType.ClearSelection();
            ucVessel.SelectedVessel = string.Empty;
            rblRAType.SelectedValue = "0";
            txtActivity.Visible = false;
            lblactivityconditions.Visible = false;
            //BindData();
            gvRiskAssessmentProcess.Rebind();
            lblCategory.Visible = true;
            ddlCategory.Visible = true;
        }
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvRiskAssessmentProcess.Rebind();
    }
    protected void BindType()
    {
        DataTable dt = new DataTable();
        dt = PhoenixInspectionRiskAssessmentActivityExtn.ListNonRoutineRiskAssessmentType();
        ddlType.DataSource = dt;
        ddlType.DataTextField = "FLDNAME";
        ddlType.DataValueField = "FLDCATEGORYID";
        ddlType.DataBind();
        ddlType.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    }
    protected void gvRiskAssessmentProcess_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("REPORT"))
            {
                string Tmpfilelocation = string.Empty; string[] reportfile = new string[0];

                RadLabel lbl = (RadLabel)e.Item.FindControl("lblRiskAssessmentProcessID");
                RadLabel lbltypeid = (RadLabel)e.Item.FindControl("lblTypeid");

                if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
                {
                    string filename = "";
                    DataSet ds;
                    if (rblRAType.SelectedValue.Equals("0"))
                    {
                        NameValueCollection nvc = new NameValueCollection();
                        nvc.Add("applicationcode", "9");
                        nvc.Add("reportcode", "RAPROCESSNEW");
                        nvc.Add("CRITERIA", "");
                        Session["PHOENIXREPORTPARAMETERS"] = nvc;

                        ds = PhoenixInspectionRiskAssessmentProcessExtn.InspectionNewProcessRAReportExtn(new Guid(lbl.Text));

                        Tmpfilelocation = System.Web.HttpContext.Current.Request.MapPath("~/");
                        filename = "PROCESSNEW_" + DateTime.Now.ToShortDateString().Replace("/", "_") + ".pdf";
                        Tmpfilelocation = HttpContext.Current.Server.MapPath("../Attachments/TEMP/" + filename);

                        PhoenixSsrsReportsCommon.getVersion();
                        PhoenixSsrsReportsCommon.getLogo();
                        PhoenixSSRSReportClass.ExportSSRSReport(reportfile, ds, PhoenixSSRSReportClass.ExportFileFormat.PDF, null, ref Tmpfilelocation);
                        Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + Tmpfilelocation + "&type=pdf");
                    }
                    else
                    {
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

    protected void gvRiskAssessmentProcess_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblProcessID = (RadLabel)e.Item.FindControl("lblRiskAssessmentProcessID");
            RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselid");
            RadLabel lblJobHazardid = (RadLabel)e.Item.FindControl("lblRiskAssessmentProcessID");
            LinkButton lnkJobActivity = (LinkButton)e.Item.FindControl("lnkJobActivity");

            if (lnkJobActivity != null)
            {
                lnkJobActivity.Visible = SessionUtil.CanAccess(this.ViewState, lnkJobActivity.CommandName);
                if (rblRAType.SelectedValue == "0")
                    lnkJobActivity.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Inspection/InspectionRAProcessExtn.aspx?DMSYN=1&processid=" + lblProcessID.Text + "',false,900,600);return false;");
            }

            LinkButton lnkHealth = (LinkButton)e.Item.FindControl("lnkHealth");

            if (lnkHealth != null)
            {
                HtmlGenericControl html = new HtmlGenericControl();

                if (drv["FLDHSYN"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    lnkHealth.Controls.Add(html);
                    lnkHealth.ToolTip = "";
                    lnkHealth.Enabled = false;
                }
                else
                {
                    lnkHealth.Visible = SessionUtil.CanAccess(this.ViewState, lnkHealth.CommandName);
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-Health\"></i></span>";
                    lnkHealth.Controls.Add(html);
                    lnkHealth.ToolTip = "Health and Safety Hazard";
                    lnkHealth.Enabled = true;
                    lnkHealth.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Health and Safety Hazard', '" + Session["sitepath"] + "/Inspection/InspectionJHAHazardsWithIcons.aspx?JHAYN=0&TYPE=1&JHAID=" + lblJobHazardid.Text + "',false,500,400); return false;");
                }
            }

            LinkButton lnkenv = (LinkButton)e.Item.FindControl("lnkenv");
            if (lnkenv != null)
            {
                HtmlGenericControl html = new HtmlGenericControl();

                if (drv["FLDENVYN"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    lnkenv.Controls.Add(html);
                    lnkenv.ToolTip = "";
                    lnkenv.Enabled = false;
                }
                else
                {
                    lnkenv.Visible = SessionUtil.CanAccess(this.ViewState, lnkenv.CommandName);
                    lnkenv.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Environmental Hazard', '" + Session["sitepath"] + "/Inspection/InspectionJHAHazardsWithIcons.aspx?JHAYN=0&TYPE=2&JHAID=" + lblJobHazardid.Text + "',false,500,400); return false;");
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-Environmental\"></i></span>";
                    lnkenv.Controls.Add(html);
                    lnkenv.ToolTip = "Environmental Hazard";
                    lnkenv.Enabled = false;
                }
            }

            LinkButton lnkPPE = (LinkButton)e.Item.FindControl("lnkPPE");
            if (lnkPPE != null)
            {
                HtmlGenericControl html = new HtmlGenericControl();
                if (drv["FLDPPEYN"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    lnkPPE.Controls.Add(html);
                    lnkPPE.ToolTip = "";
                    lnkPPE.Enabled = false;
                }
                else
                {
                    lnkPPE.Visible = SessionUtil.CanAccess(this.ViewState, lnkPPE.CommandName);
                    lnkPPE.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Recommended PPE', '" + Session["sitepath"] + "/Inspection/InspectionJHAPPEWithIcons.aspx?JHAYN=0&JHAID=" + lblJobHazardid.Text + "',false,500,400); return true;");
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-PPE\"></i></span>";
                    lnkPPE.Controls.Add(html);
                    lnkPPE.ToolTip = "Recommended PPE";
                    lnkPPE.Enabled = false;
                }
            }

            LinkButton lnkcomponent = (LinkButton)e.Item.FindControl("lnkcomponent");
            if (lnkcomponent != null)
            {
                HtmlGenericControl html = new HtmlGenericControl();
                if (drv["FLDEQUIPMENTYN"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    lnkcomponent.Controls.Add(html);
                    lnkcomponent.ToolTip = "";
                    lnkcomponent.Enabled = false;
                }
                else
                {
                    lnkcomponent.Visible = SessionUtil.CanAccess(this.ViewState, lnkcomponent.CommandName);
                    lnkcomponent.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Equipment', '" + Session["sitepath"] + "/Inspection/InspectionJHAMappedComponents.aspx?JHAYN=0&JHAID=" + lblJobHazardid.Text + "',false,500,400); return true;");
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-PMS\"></i></span>";
                    lnkcomponent.Controls.Add(html);
                    lnkcomponent.ToolTip = "Equipment";
                    lnkcomponent.Enabled = false;
                }
            }

            LinkButton lnkprocedure = (LinkButton)e.Item.FindControl("lnkprocedure");
            if (lnkprocedure != null)
            {
                HtmlGenericControl html = new HtmlGenericControl();
                if (drv["FLDPROCEDUREYN"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    lnkprocedure.Controls.Add(html);
                    lnkprocedure.ToolTip = "";
                    lnkprocedure.Enabled = false;
                }
                else
                {
                    lnkprocedure.Visible = SessionUtil.CanAccess(this.ViewState, lnkprocedure.CommandName);
                    lnkprocedure.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Procedure', '" + Session["sitepath"] + "/Inspection/InspectionJHAMappedProcedure.aspx?JHAYN=0&JHAID=" + lblJobHazardid.Text + "',false,500,400); return true;");
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-process\"></i></span>";
                    lnkprocedure.Controls.Add(html);
                    lnkprocedure.ToolTip = "Procedure";
                    lnkprocedure.Enabled = false;
                }
            }

            LinkButton lnkforms = (LinkButton)e.Item.FindControl("lnkforms");
            if (lnkforms != null)
            {
                HtmlGenericControl html = new HtmlGenericControl();
                if (drv["FLDFORMSANDCHECKLISTYN"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    lnkforms.Controls.Add(html);
                    lnkforms.ToolTip = "";
                    lnkforms.Enabled = false;
                }
                else
                {
                    lnkforms.Visible = SessionUtil.CanAccess(this.ViewState, lnkforms.CommandName);
                    lnkforms.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Forms and Checklist', '" + Session["sitepath"] + "/Inspection/InspectionJHAMappedForms.aspx?JHAYN=0&JHAID=" + lblJobHazardid.Text + "',false,500,400); return true;");
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-file-contract-af\"></i></span>";
                    lnkforms.Controls.Add(html);
                    lnkforms.ToolTip = "Forms and Checklist";
                    lnkforms.Enabled = false;
                }
            }

            LinkButton lnkWorkPermits = (LinkButton)e.Item.FindControl("lnkWorkPermits");
            if (lnkWorkPermits != null)
            {
                HtmlGenericControl html = new HtmlGenericControl();
                if (drv["FLDWORKPERMITYN"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    lnkWorkPermits.Controls.Add(html);
                    lnkWorkPermits.ToolTip = "";
                    lnkWorkPermits.Enabled = false;
                }
                else
                {
                    lnkWorkPermits.Visible = SessionUtil.CanAccess(this.ViewState, lnkWorkPermits.CommandName);
                    lnkWorkPermits.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Work Permit', '" + Session["sitepath"] + "/Inspection/InspectionJHAWorkPermitWithIcons.aspx?JHAYN=0&JHAID=" + lblJobHazardid.Text + "',false,500,400); return true;");
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-administration\"></i></span>";
                    lnkWorkPermits.Controls.Add(html);
                    lnkWorkPermits.ToolTip = "Work Permit";
                    lnkWorkPermits.Enabled = false;
                }

            }

            LinkButton lnkEPSS = (LinkButton)e.Item.FindControl("lnkEPSS");
            if (lnkEPSS != null)
            {
                HtmlGenericControl html = new HtmlGenericControl();
                if (drv["FLDEPSSYN"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"empty\"></i></span>";
                    lnkEPSS.Controls.Add(html);
                    lnkEPSS.ToolTip = "";
                    lnkEPSS.Enabled = false;
                }
                else
                {
                    lnkEPSS.Visible = SessionUtil.CanAccess(this.ViewState, lnkEPSS.CommandName);
                    lnkEPSS.Attributes.Add("onclick", "openNewWindow('codehelp1', 'EPSS', '" + Session["sitepath"] + "/Inspection/InspectionJHAEPSSList.aspx?JHAYN=0&JHAID=" + lblJobHazardid.Text + "',false,500,400); return true;");
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-Elog\"></i></span>";
                    lnkEPSS.Controls.Add(html);
                    lnkEPSS.ToolTip = "EPSS";
                    lnkEPSS.Enabled = false;
                }
            }

            RadLabel lblHS = (RadLabel)e.Item.FindControl("lblHealth");
            RadLabel lblENV = (RadLabel)e.Item.FindControl("lblEnvironmental");
            RadLabel lblECO = (RadLabel)e.Item.FindControl("lblEcononmic");
            RadLabel lblWCS = (RadLabel)e.Item.FindControl("lblWorstCase");

            DataRowView dv = (DataRowView)e.Item.DataItem;

            decimal minscore = 0, maxscore = 0;

            if (!string.IsNullOrEmpty(dv["FLDMINSCORE"].ToString()))
                minscore = decimal.Parse(dv["FLDMINSCORE"].ToString());

            if (!string.IsNullOrEmpty(dv["FLDMAXSCORE"].ToString()))
                maxscore = decimal.Parse(dv["FLDMAXSCORE"].ToString());

            if (lblHS != null)
            {
                lblHS.Attributes.Add("style", "font-weight:bold;");
                if (!string.IsNullOrEmpty(lblHS.Text))
                {
                    if (decimal.Parse(lblHS.Text) <= minscore)
                        lblHS.BackColor = System.Drawing.Color.Lime;
                    else if (decimal.Parse(lblHS.Text) > minscore && decimal.Parse(lblHS.Text) <= maxscore)
                        lblHS.BackColor = System.Drawing.Color.Yellow;
                    else if (decimal.Parse(lblHS.Text) > maxscore)
                        lblHS.BackColor = System.Drawing.Color.Red;
                }
            }

            if (lblENV != null)
            {
                lblENV.Attributes.Add("style", "font-weight:bold;");
                if (!string.IsNullOrEmpty(lblENV.Text))
                {
                    if (decimal.Parse(lblENV.Text) <= minscore)
                        lblENV.BackColor = System.Drawing.Color.Lime;
                    else if (decimal.Parse(lblENV.Text) > minscore && decimal.Parse(lblENV.Text) <= maxscore)
                        lblENV.BackColor = System.Drawing.Color.Yellow;
                    else if (decimal.Parse(lblENV.Text) > maxscore)
                        lblENV.BackColor = System.Drawing.Color.Red;
                }
            }

            if (lblECO != null)
            {
                lblECO.Attributes.Add("style", "font-weight:bold;");
                if (!string.IsNullOrEmpty(lblECO.Text))
                {
                    if (decimal.Parse(lblECO.Text) <= minscore)
                        lblECO.BackColor = System.Drawing.Color.Lime;
                    else if (decimal.Parse(lblECO.Text) > minscore && decimal.Parse(lblECO.Text) <= maxscore)
                        lblECO.BackColor = System.Drawing.Color.Yellow;
                    else if (decimal.Parse(lblECO.Text) > maxscore)
                        lblECO.BackColor = System.Drawing.Color.Red;
                }
            }

            if (lblWCS != null)
            {
                lblWCS.Attributes.Add("style", "font-weight:bold;");
                if (!string.IsNullOrEmpty(lblWCS.Text))
                {
                    if (decimal.Parse(lblWCS.Text) <= minscore)
                        lblWCS.BackColor = System.Drawing.Color.Lime;
                    else if (decimal.Parse(lblWCS.Text) > minscore && decimal.Parse(lblWCS.Text) <= maxscore)
                        lblWCS.BackColor = System.Drawing.Color.Yellow;
                    else if (decimal.Parse(lblWCS.Text) > maxscore)
                        lblWCS.BackColor = System.Drawing.Color.Red;
                }
            }

        }

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
}