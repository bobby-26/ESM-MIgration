using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.DocumentManagement;
using System.Collections.Specialized;
using SouthNests.Phoenix.Reports;
using System.Web;

public partial class DocumentManagementRAJobHazardAnalysisList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementRAJobHazardAnalysisList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "SEARCH");
        toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementRAJobHazardAnalysisList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuJobHazardAnalysis.AccessRights = this.ViewState;
        MenuJobHazardAnalysis.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            BindStatus();

            ViewState["REFNO"] = "";
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["NEWDMSYN"] = 0;

            if (Request.QueryString["NEWDMSYN"] != null)
                ViewState["NEWDMSYN"] = Request.QueryString["NEWDMSYN"].ToString();

            lblStatus.Visible = false;
            ddlStatus.SelectedValue = "3";
            ddlStatus.Visible = false;
            gvRiskAssessmentJobHazardAnalysis.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            BindCategory();
        }
        if (ViewState["NEWDMSYN"].ToString().Equals("1"))
        {
            rblOldNew.SelectedValue = "0";
        }
        else
        {
            rblOldNew.SelectedValue = "1";

        }
        rblOldNew.Enabled = false;
        rblOldNew.Visible = false;
    }
    private void BindStatus()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("FLDSTATUSID", typeof(string));
        dt.Columns.Add("FLDSTATUSNAME", typeof(string));
        dt.Rows.Add("1", "Draft");
        dt.Rows.Add("2", "Approved");
        dt.Rows.Add("3", "Issued");
        dt.Rows.Add("8", "Preparation");
        dt.Rows.Add("4", "Pending approval");
        dt.Rows.Add("5", "Approved for use");
        dt.Rows.Add("6", "Not Approved");
        //dt.Rows.Add("7", "Completed");                

        ddlStatus.DataSource = dt;
        ddlStatus.DataTextField = "FLDSTATUSNAME";
        ddlStatus.DataValueField = "FLDSTATUSID";
        ddlStatus.DataBind();
        ddlStatus.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDHAZARDNUMBER", "FLDCATEGORYNAME", "FLDSUBCATEGORY", "FLDJOB", "FLDSTATUS", "FLDREVISIONNO", "FLDISSUEDDATE" };
        string[] alCaptions = { "Hazard Number", "Type", "Category", "Job", "Status", "Revision No", "Issued Date" };

        int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
        DataSet ds = new DataSet();

        if (rblOldNew.SelectedValue == "1")
        {
            ds = PhoenixDocumentManagementDocument.RiskAssessmentJobHazardSearch(
                    General.GetNullableString(txtHazardNo.Text),
                    General.GetNullableInteger(null),
                    General.GetNullableInteger(ddlCategory.SelectedValue),
                    General.GetNullableInteger(ddlStatus.SelectedValue),
                    General.GetNullableGuid(ViewState["REFNO"].ToString()),
                    null, null,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvRiskAssessmentJobHazardAnalysis.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount,
                    General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
                    companyid
                    );
        }
        else
        {
            ds = PhoenixDocumentManagementDocument.NewRiskAssessmentJobHazardSearch(
                   General.GetNullableString(txtHazardNo.Text),
                   General.GetNullableInteger(ddlCategory.SelectedValue),
                   null, null,
                   Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                   gvRiskAssessmentJobHazardAnalysis.PageSize,
                   ref iRowCount,
                   ref iTotalPageCount,
                   General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
                   companyid,null);
        }

        General.SetPrintOptions("gvRiskAssessmentJobHazardAnalysis", "Job Hazard Analysis", alCaptions, alColumns, ds);

        gvRiskAssessmentJobHazardAnalysis.DataSource = ds;
        gvRiskAssessmentJobHazardAnalysis.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;

    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDHAZARDNUMBER", "FLDCATEGORYNAME", "FLDSUBCATEGORY", "FLDJOB", "FLDSTATUS", "FLDREVISIONNO", "FLDISSUEDDATE" };
            string[] alCaptions = { "Hazard Number", "Type", "Category", "Job", "Status", "Revision No", "Issued Date" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

            DataSet ds = new DataSet();

            if (rblOldNew.SelectedValue == "1")
            {
                ds = PhoenixDocumentManagementDocument.RiskAssessmentJobHazardSearch(
                   General.GetNullableString(txtHazardNo.Text),
                   General.GetNullableInteger(null),
                   General.GetNullableInteger(ddlCategory.SelectedValue),
                   General.GetNullableInteger(ddlStatus.SelectedValue),
                   General.GetNullableGuid(ViewState["REFNO"].ToString()),
                   null, null,
                   Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                   iRowCount,
                   ref iRowCount,
                   ref iTotalPageCount,
                   General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()));
            }
            else
            {

                ds = PhoenixDocumentManagementDocument.NewRiskAssessmentJobHazardSearch(
                   General.GetNullableString(txtHazardNo.Text),
                   General.GetNullableInteger(ddlCategory.SelectedValue),
                   null, null,
                   Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                   iRowCount,
                   ref iRowCount,
                   ref iTotalPageCount,
                   General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
                   companyid,
                   null);
            }
            General.ShowExcel("Job Hazard Analysis", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvRiskAssessmentJobHazardAnalysis.SelectedIndexes.Clear();
        gvRiskAssessmentJobHazardAnalysis.EditIndexes.Clear();
        gvRiskAssessmentJobHazardAnalysis.DataSource = null;
        gvRiskAssessmentJobHazardAnalysis.Rebind();
    }

    protected void MenuJobHazardAnalysis_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("ADD"))
        {
            Response.Redirect("../Inspection/InspectionRAJobHazardAnalysis.aspx?status=", false);
        }
        else if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (CommandName.ToUpper().Equals("SEARCH"))
        {
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            BindStatus();           
            ViewState["REFNO"] = "";
            txtHazardNo.Text = "";
            ddlStatus.SelectedValue = "3";
            BindCategory();
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
    }

    protected void gvRiskAssessmentJobHazardAnalysis_RowDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lbljobhazardid = (RadLabel)e.Item.FindControl("lblJobHazardid");
            ImageButton jobhzd = (ImageButton)e.Item.FindControl("cmdJobHazard");
            if (jobhzd != null)
            {
                jobhzd.Visible = SessionUtil.CanAccess(this.ViewState, jobhzd.CommandName);
                if (rblOldNew.SelectedValue == "1")
                {
                    jobhzd.Attributes.Add("onclick", "javascript:parent.Openpopup('JobHazard','','../Reports/ReportsView.aspx?applicationcode=9&reportcode=JOBHAZARD&showmenu=0&showword=NO&showexcel=NO&jobhazardid=" + lbljobhazardid.Text + "'); return false;");
                }

            }
        }

    }

    protected void gvRiskAssessmentJobHazardAnalysis_RowCommand(object sender, GridCommandEventArgs gce)
    {
        try
        {
            string Tmpfilelocation = string.Empty; string[] reportfile = new string[0];

            if (gce.CommandName.ToUpper().Equals("JOBHAZARD"))
            {
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblJobHazardid");
                DataSet ds = PhoenixInspectionRiskAssessmentJobHazardExtn.InspectionNewJHAReportExtn(new Guid(lbl.Text));
                if (rblOldNew.SelectedValue != "1")
                {
                    if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
                    {

                        NameValueCollection nvc = new NameValueCollection();
                        nvc.Add("applicationcode", "9");
                        nvc.Add("reportcode", "JOBHAZARDNEW");
                        nvc.Add("CRITERIA", "");
                        Session["PHOENIXREPORTPARAMETERS"] = nvc;

                        Tmpfilelocation = System.Web.HttpContext.Current.Request.MapPath("~/");
                        string filename = "JOBHAZARDNEW_" + DateTime.Now.ToShortDateString().Replace("/", "_") + ".pdf";
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

protected void gvRiskAssessmentJobHazardAnalysis_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
{
    try
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRiskAssessmentJobHazardAnalysis.CurrentPageIndex + 1;
        BindData();
    }
    catch (Exception ex)
    {
        ucError.ErrorMessage = ex.Message;
        ucError.Visible = true;
    }
}

protected void gvRiskAssessmentJobHazardAnalysis_RowDeleting(object sender, GridCommandEventArgs e)
{

}



protected void BindCategory()
{

    if (rblOldNew.SelectedValue == "1")
    {
        DataSet ds = PhoenixDocumentManagementDocument.ListRiskAssessmentActivity(General.GetNullableInteger(null),
            General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString()));
        ddlCategory.DataTextField = "FLDNAME";
        ddlCategory.DataValueField = "FLDACTIVITYID";
        ddlCategory.Items.Clear();
        ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        ddlCategory.DataSource = ds.Tables[0];
        ddlCategory.DataBind();

    }
    else
    {
        DataTable ds = PhoenixInspectionRiskAssessmentActivityExtn.ListRiskAssessmentElement();
        ddlCategory.DataTextField = "FLDNAME";
        ddlCategory.DataValueField = "FLDCATEGORYID";
        ddlCategory.Items.Clear();
        ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        ddlCategory.DataSource = ds;
        ddlCategory.DataBind();
    }


}

protected void ucType_SelectedIndexChanged(object sender, EventArgs e)
{
    BindCategory();
}

protected void ddlCategory_Changed(object sender, EventArgs e)
{
    ViewState["PAGENUMBER"] = 1;
    Rebind();
}

protected void ddlStatus_Changed(object sender, EventArgs e)
{

}


protected void rblOldNew_SelectedIndexChanged(object sender, EventArgs e)
{
    if (rblOldNew.SelectedValue == "1")
    {
        ViewState["PAGENUMBER"] = 1;
        txtHazardNo.Text = "";
        rblOldNew.SelectedValue = "1";
        BindCategory();
        Rebind();
    }
    else
    {
        ViewState["PAGENUMBER"] = 1;
        txtHazardNo.Text = "";
        rblOldNew.SelectedValue = "0";
        BindCategory();
        Rebind();
    }
}
}
