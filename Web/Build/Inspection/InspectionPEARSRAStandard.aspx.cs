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

public partial class InspectionPEARSRAStandard : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionPEARSRAStandard.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvPEARSSTDRA')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionPEARSRAStandard.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuPEARSRA.AccessRights = this.ViewState;
            MenuPEARSRA.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ucConfirmApprove.Attributes.Add("style", "display:none");
                ucConfirmRevision.Attributes.Add("style", "display:none");
                btnstandardtemplateissue.Attributes.Add("style", "display:none");

                ViewState["txtRefNo"] = string.Empty;
                ViewState["txtActivity"] = string.Empty;
                ViewState["txtWorkSite"] = string.Empty;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["COMPANYID"] = "";


                NameValueCollection nvcCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvcCompany.Get("QMS") != null && nvcCompany.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvcCompany.Get("QMS");
                }

                if (InspectionFilter.CurrentPEARSRAFilter == null)
                {
                    SetFilter();
                }

                gvPEARSSTDRA.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
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

        string[] alColumns = { "FLDREFERENCENUMBER", "FLDCREATEDDATE", "FLDINTENDEDWORKDATE", "FLDTYPEOFACTIVITY", "FLDACTIVITYWORKSITE", "FLDREVISIONNUMBER", "FLDSTATUS" };
        string[] alCaptions = { "Ref. No", "Prepared", "Intended Work", "Type of Activity", "Activity Worksite", "Revision No", "Status" };

        NameValueCollection nvc = InspectionFilter.CurrentPEARSRAFilter;

        DataSet ds = PhoenixInspectionPEARSRiskAssessment.SearchStandardRiskAssessment(nvc.Get("txtRefNo") != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : General.GetNullableString(ViewState["txtRefNo"].ToString())
                        , nvc.Get("txtActivity") != null ? General.GetNullableString(nvc["txtActivity"].ToString()) : General.GetNullableString(ViewState["txtActivity"].ToString())
                        , nvc.Get("txtWorkSite") != null ? General.GetNullableString(nvc["txtWorkSite"].ToString()) : General.GetNullableString(ViewState["txtWorkSite"].ToString())
                        , sortdirection
                        , sortexpression
                        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                        , gvPEARSSTDRA.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);

        General.SetPrintOptions("gvPEARSSTDRA", "PEARS STANDARD RA", alCaptions, alColumns, ds);

        gvPEARSSTDRA.DataSource = ds;
        gvPEARSSTDRA.VirtualItemCount = iRowCount;
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
            string[] alColumns = { "FLDREFERENCENUMBER", "FLDCREATEDDATE", "FLDINTENDEDWORKDATE", "FLDTYPEOFACTIVITY", "FLDACTIVITYWORKSITE", "FLDREVISIONNUMBER", "FLDSTATUS" };
            string[] alCaptions = { "Ref. No", "Prepared", "Intended Work", "Type of Activity", "Activity Worksite", "Revision No", "Status" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = InspectionFilter.CurrentPEARSRAFilter;

            DataSet ds = PhoenixInspectionPEARSRiskAssessment.SearchStandardRiskAssessment(nvc.Get("txtRefNo") != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : General.GetNullableString(ViewState["txtRefNo"].ToString())
                       , nvc.Get("txtActivity") != null ? General.GetNullableString(nvc["txtActivity"].ToString()) : General.GetNullableString(ViewState["txtActivity"].ToString())
                       , nvc.Get("txtWorkSite") != null ? General.GetNullableString(nvc["txtWorkSite"].ToString()) : General.GetNullableString(ViewState["txtWorkSite"].ToString())
                       , sortdirection
                       , sortexpression
                       , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                       , iRowCount
                       , ref iRowCount
                       , ref iTotalPageCount);

            General.ShowExcel("PEARS STANDARD RA", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvPEARSSTDRA.SelectedIndexes.Clear();
        gvPEARSSTDRA.EditIndexes.Clear();
        gvPEARSSTDRA.DataSource = null;
        gvPEARSSTDRA.Rebind();
    }
    protected void gvPEARSSTDRA_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPEARSSTDRA.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvPEARSSTDRA_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            RadLabel lblRAid = (RadLabel)e.Item.FindControl("lblRAID");

            if (e.CommandName.ToUpper().Equals("EDITROW"))
            {
                Response.Redirect("../Inspection/InspectionPEARSRiskAssessmentDetails.aspx?RAID=" + lblRAid.Text + "&StandardRA=1", false);
            }

            if (e.CommandName.ToUpper().Equals("REVISION"))
            {
                ViewState["RAID"] = lblRAid.Text;
                RadWindowManager1.RadConfirm("Are you sure you want to revise this RA?", "ConfirmRevision", 320, 150, null, "Revision");
            }

            if (e.CommandName.ToUpper().Equals("STDISSUE"))
            {
                ViewState["RAID"] = lblRAid.Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Issue Standard Template for this RA?", "confirmissue", 320, 150, null, "issue");
            }

            if (e.CommandName == RadGrid.FilterCommandName)
            {
                Pair filterPair = (Pair)e.CommandArgument;
                ViewState["PAGENUMBER"] = 1;

                ViewState["txtRefNo"] = gvPEARSSTDRA.MasterTableView.GetColumn("FLDREFERENCENUMBER").CurrentFilterValue;
                ViewState["txtActivity"] = gvPEARSSTDRA.MasterTableView.GetColumn("FLDTYPEOFACTIVITY").CurrentFilterValue;
                SetFilter();
                Rebind();
            }

            if (e.CommandName.ToUpper().Equals("PEARS"))
            {
                string Tmpfilelocation = string.Empty; string[] reportfile = new string[0];

                if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
                {
                    string filename = "";
                    DataSet ds = new DataSet();

                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("applicationcode", "9");
                    nvc.Add("reportcode", "PEARSRA");
                    nvc.Add("CRITERIA", "");
                    Session["PHOENIXREPORTPARAMETERS"] = nvc;

                    ds = PhoenixInspectionPEARSRiskAssessment.ReportRiskAssessment(new Guid(lblRAid.Text));

                    Tmpfilelocation = System.Web.HttpContext.Current.Request.MapPath("~/");
                    filename = "PEARSNEW" + DateTime.Now.ToShortDateString().Replace("/", "_") + ".pdf";
                    Tmpfilelocation = HttpContext.Current.Server.MapPath("../Attachments/TEMP/" + filename);

                    PhoenixSsrsReportsCommon.getVersion();
                    PhoenixSsrsReportsCommon.getLogo();
                    PhoenixSSRSReportClass.ExportSSRSReport(reportfile, ds, PhoenixSSRSReportClass.ExportFileFormat.PDF, null, ref Tmpfilelocation);
                    Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + Tmpfilelocation + "&type=pdf");
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

    protected void gvPEARSSTDRA_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblRAid = (RadLabel)e.Item.FindControl("lblRAID");

            LinkButton imgApprove = (LinkButton)e.Item.FindControl("imgApprove");
            LinkButton imgrevision = (LinkButton)e.Item.FindControl("imgrevision");
            LinkButton imgCopyTemplate = (LinkButton)e.Item.FindControl("imgCopyTemplate");
            LinkButton cmdRevision = (LinkButton)e.Item.FindControl("cmdRevision");
            RadLabel lblStatus = (RadLabel)e.Item.FindControl("lblStatus");
            LinkButton cmdRAReport = (LinkButton)e.Item.FindControl("cmdRAReport");
            RadLabel lblRevisionNo = (RadLabel)e.Item.FindControl("lblRevisionNo");
            LinkButton imgSTD = (LinkButton)e.Item.FindControl("imgSTD");

            imgSTD.Visible = false;
            imgrevision.Visible = false;
            imgApprove.Visible = false;
            cmdRevision.Visible = false;

            if (lblRevisionNo.Text != "0")
            {
                cmdRevision.Visible = true;
            }

            if (lblStatus.Text == "2")
            {
                imgApprove.Visible = true;
            }
            if (lblStatus.Text == "3")
            {
                imgrevision.Visible = true;
            }
            if (lblStatus.Text == "5")
            {
                imgrevision.Visible = true;
                imgSTD.Visible = true;
                imgApprove.Visible = false;
            }
            if (lblStatus.Text == "6")
            {
                imgrevision.Visible = true;
                imgApprove.Visible = false;
            }
            if (lblStatus.Text == "7")
            {
                imgrevision.Visible = true;
                imgApprove.Visible = false;

            }
            if (lblStatus.Text == "8")
            {
                imgrevision.Visible = true;
                imgApprove.Visible = false;

            }

            if (imgApprove != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgApprove.CommandName)) imgApprove.Visible = false;
            }
            if (cmdRevision != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdRevision.CommandName)) cmdRevision.Visible = false;
            }
            if (cmdRAReport != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdRAReport.CommandName)) cmdRAReport.Visible = false;
            }
            if (imgCopyTemplate != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgCopyTemplate.CommandName)) imgCopyTemplate.Visible = false;
            }

            if (imgSTD != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgSTD.CommandName)) imgSTD.Visible = false;
            }


            if (imgApprove != null)
            {
                imgApprove.Attributes.Add("onclick", "openNewWindow('Approve', '', '" + Session["sitepath"] + "/Inspection/InspectionPEARSRAApprove.aspx?RAID=" + lblRAid.Text + "','medium'); return true;");
            }

            if (imgCopyTemplate != null)
            {
                imgCopyTemplate.Attributes.Add("onclick", "openNewWindow('Copy', '', '" + Session["sitepath"] + "/Inspection/InspectionPEARSRiskAssessmentDetails.aspx?RAID=" + lblRAid.Text + "&CopyType=1" + "'); return true;");
            }
            if (cmdRevision != null)
            {
                cmdRevision.Attributes.Add("onclick", "openNewWindow('RA-Revision', '', '" + Session["sitepath"] + "/Inspection/InspectionPEARSRARevision.aspx?RAID=" + lblRAid.Text + "'); return true;");
            }
        }
    }

    protected void ucConfirmApprove_Click(object sender, EventArgs e)
    {

    }
    protected void btnstandardtemplateissue_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["RAID"] != null && ViewState["RAID"].ToString() != "")
            {
                PhoenixInspectionPEARSRiskAssessment.IssueStandardTemplateRA(new Guid(ViewState["RAID"].ToString()));
                ucStatus.Text = "Issued Standard Template Successfully";
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void ucConfirmRevision_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["RAID"] != null && ViewState["RAID"].ToString() != "")
            {
                PhoenixInspectionPEARSRiskAssessment.CreateRiskAssessmentRevision(new Guid(ViewState["RAID"].ToString()));

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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    private void SetFilter()
    {
        NameValueCollection criteria = new NameValueCollection();

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            ViewState["VesselID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        }
        criteria.Add("txtRefNo", ViewState["txtRefNo"].ToString());
        criteria.Add("txtActivity", ViewState["txtActivity"].ToString());
        criteria.Add("txtWorkSite", ViewState["txtWorkSite"].ToString());

        InspectionFilter.CurrentPEARSRAFilter = criteria;
    }
    protected void MenuPEARSRA_TabStripCommand(object sender, EventArgs e)
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
                ViewState["PAGENUMBER"] = 1;
                Rebind();
                ViewState["txtRefNo"] = string.Empty;
                ViewState["txtActivity"] = string.Empty;
                ViewState["txtWorkSite"] = string.Empty;

                InspectionFilter.CurrentPEARSRAFilter.Clear();
                SetFilter();
                gvPEARSSTDRA.Rebind();
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