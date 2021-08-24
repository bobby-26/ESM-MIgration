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

public partial class InspectionPEARSRiskAssessmentList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionPEARSRiskAssessmentList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvPEARSRA')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionPEARSRiskAssessmentList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionPEARSRiskAssessmentDetails.aspx", "New Template", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

            MenuPEARSRA.AccessRights = this.ViewState;
            MenuPEARSRA.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ucConfirmApprove.Attributes.Add("style", "display:none");
                ucConfirmRevision.Attributes.Add("style", "display:none");
                btncancel.Attributes.Add("style", "display:none");
                btnnotused.Attributes.Add("style", "display:none");
                btnstandardtemplateissue.Attributes.Add("style", "display:none");

                ViewState["txtRefNo"] = string.Empty;
                ViewState["VesselID"] = string.Empty;
                ViewState["ddlStatus"] = string.Empty;
                ViewState["FDATE"] = string.Empty;
                ViewState["TDATE"] = string.Empty;

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

                gvPEARSRA.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        string[] alColumns = { "FLDREFERENCENUMBER", "FLDVESSELNAME", "FLDCREATEDDATE", "FLDINTENDEDWORKDATE", "FLDTYPEOFACTIVITY", "FLDACTIVITYWORKSITE", "FLDREVISIONNUMBER", "FLDSTATUS" };
        string[] alCaptions = { "Ref. No", "Vessel", "Prepared", "Intended Work", "Type of Activity", "Activity Worksite", "Revision No", "Status" };

        NameValueCollection nvc = InspectionFilter.CurrentPEARSRAFilter;

        DataSet ds = PhoenixInspectionPEARSRiskAssessment.SearchRiskAssessment(nvc.Get("ddlVessel") != null ? General.GetNullableInteger(nvc["ddlVessel"].ToString()) : General.GetNullableInteger(ViewState["VesselID"].ToString())
                       , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                       , nvc.Get("FromDate") != null ? General.GetNullableDateTime(nvc["FromDate"].ToString()) : General.GetNullableDateTime(ViewState["FDATE"].ToString())
                       , nvc.Get("ToDate") != null ? General.GetNullableDateTime(nvc["ToDate"].ToString()) : General.GetNullableDateTime(ViewState["TDATE"].ToString())
                       , nvc.Get("txtRefNo") != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : General.GetNullableString(ViewState["txtRefNo"].ToString())
                       , nvc.Get("ddlStatus") != null ? General.GetNullableInteger(nvc["ddlStatus"].ToString()) : General.GetNullableInteger(ViewState["ddlStatus"].ToString())
                       , sortdirection
                       , sortexpression
                       , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                       , gvPEARSRA.PageSize
                       , ref iRowCount
                       , ref iTotalPageCount);

        General.SetPrintOptions("gvPEARSRA", "PEARS RA", alCaptions, alColumns, ds);

        gvPEARSRA.DataSource = ds;
        gvPEARSRA.VirtualItemCount = iRowCount;
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
            string[] alColumns = { "FLDREFERENCENUMBER", "FLDVESSELNAME", "FLDCREATEDDATE", "FLDINTENDEDWORKDATE", "FLDTYPEOFACTIVITY", "FLDACTIVITYWORKSITE", "FLDREVISIONNUMBER", "FLDSTATUS" };
            string[] alCaptions = { "Ref. No", "Vessel", "Prepared", "Intended Work", "Type of Activity", "Activity Worksite", "Revision No", "Status" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = InspectionFilter.CurrentPEARSRAFilter;

            DataSet ds = PhoenixInspectionPEARSRiskAssessment.SearchRiskAssessment(nvc.Get("ddlVessel") != null ? General.GetNullableInteger(nvc["ddlVessel"].ToString()) : General.GetNullableInteger(ViewState["VesselID"].ToString())
                       , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                       , nvc.Get("FromDate") != null ? General.GetNullableDateTime(nvc["FromDate"].ToString()) : General.GetNullableDateTime(ViewState["FDATE"].ToString())
                       , nvc.Get("ToDate") != null ? General.GetNullableDateTime(nvc["ToDate"].ToString()) : General.GetNullableDateTime(ViewState["TDATE"].ToString())
                       , nvc.Get("txtRefNo") != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : General.GetNullableString(ViewState["txtRefNo"].ToString())
                       , nvc.Get("ddlStatus") != null ? General.GetNullableInteger(nvc["ddlStatus"].ToString()) : General.GetNullableInteger(ViewState["ddlStatus"].ToString())
                       , sortdirection
                       , sortexpression
                       , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                       , gvPEARSRA.PageSize
                       , ref iRowCount
                       , ref iTotalPageCount);

            General.ShowExcel("PEARS RA", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvPEARSRA.SelectedIndexes.Clear();
        gvPEARSRA.EditIndexes.Clear();
        gvPEARSRA.DataSource = null;
        gvPEARSRA.Rebind();
    }
    protected void MenuPEARSRA_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("ADD"))
            {
                Response.Redirect("../Inspection/InspectionPEARSRiskAssessmentDetails.aspx?", false);
            }

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
                ViewState["txtRefNo"] = string.Empty;
                ViewState["VesselID"] = string.Empty;
                ViewState["ddlStatus"] = string.Empty;
                ViewState["FDATE"] = string.Empty;
                ViewState["TDATE"] = string.Empty;

                InspectionFilter.CurrentPEARSRAFilter.Clear();
                SetFilter();
                gvPEARSRA.Rebind();
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

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            ViewState["VesselID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        }

        criteria.Add("txtRefNo", ViewState["txtRefNo"].ToString());
        criteria.Add("ddlVessel", ViewState["VesselID"].ToString());
        criteria.Add("ddlStatus", ViewState["ddlStatus"].ToString());
        criteria.Add("FromDate", ViewState["FDATE"].ToString());
        criteria.Add("ToDate", ViewState["TDATE"].ToString());

        InspectionFilter.CurrentPEARSRAFilter = criteria;
    }
    protected void gvPEARSRA_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPEARSRA.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvPEARSRA_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            RadLabel lblRAid = (RadLabel)e.Item.FindControl("lblRAID");

            if (e.CommandName.ToUpper().Equals("EDITROW"))
            {
                Response.Redirect("../Inspection/InspectionPEARSRiskAssessmentDetails.aspx?RAID=" + lblRAid.Text, false);
            }
            if (e.CommandName.ToUpper().Equals("CANCEL"))
            {
                ViewState["RAID"] = lblRAid.Text;
                RadWindowManager1.RadConfirm("Are you sure you want to cancel this RA?", "confirmcancel", 320, 150, null, "Cancel");
            }

            if (e.CommandName.ToUpper().Equals("NOTUSE"))
            {
                ViewState["RAID"] = lblRAid.Text;
                RadWindowManager1.RadConfirm("Are you sure your not required this RA?", "confirmnotused", 320, 150, null, "Not Use");
            }
            if (e.CommandName.ToUpper().Equals("STDISSUE"))
            {
                ViewState["RAID"] = lblRAid.Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Issue Standard Template for this RA?", "confirmissue", 320, 150, null, "issue");
            }
            if (e.CommandName.ToUpper().Equals("REVISION"))
            {
                ViewState["RAID"] = lblRAid.Text;
                RadWindowManager1.RadConfirm("Are you sure you want to revise this RA?", "ConfirmRevision", 320, 150, null, "Revision");
            }
            if (e.CommandName == RadGrid.FilterCommandName)
            {
                Pair filterPair = (Pair)e.CommandArgument;
                ViewState["PAGENUMBER"] = 1;

                string daterange = gvPEARSRA.MasterTableView.GetColumn("FLDINTENDEDWORKDATE").CurrentFilterValue.ToString();
                if (daterange != string.Empty)
                {
                    ViewState["FDATE"] = daterange.Split('~')[0];
                    ViewState["TDATE"] = daterange.Split('~')[1];
                }

                ViewState["txtRefNo"] = gvPEARSRA.MasterTableView.GetColumn("FLDREFERENCENUMBER").CurrentFilterValue;
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

                    PhoenixReportsInspection.LoadImage(ds);

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

    protected void gvPEARSRA_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblRAid = (RadLabel)e.Item.FindControl("lblRAID");
            RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselid");

            LinkButton imgApprove = (LinkButton)e.Item.FindControl("imgApprove");
            LinkButton imgrevision = (LinkButton)e.Item.FindControl("imgrevision");
            LinkButton imgCopyTemplate = (LinkButton)e.Item.FindControl("imgCopyTemplate");
            LinkButton cmdRevision = (LinkButton)e.Item.FindControl("cmdRevision");
            RadLabel lblStatus = (RadLabel)e.Item.FindControl("lblStatus");
            LinkButton imgSTD = (LinkButton)e.Item.FindControl("imgSTD");
            LinkButton imgcancel = (LinkButton)e.Item.FindControl("imgcancel");
            LinkButton imgnotused = (LinkButton)e.Item.FindControl("imgnotused");
            LinkButton cmdRAReport = (LinkButton)e.Item.FindControl("cmdRAReport");
            RadLabel lblRevisionNo = (RadLabel)e.Item.FindControl("lblRevisionNo");

            imgrevision.Visible = false;
            imgApprove.Visible = false;
            imgSTD.Visible = false;
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
            //if (lblStatus.Text == "4")
            //{
            //    cmdRevision.Visible = true;
            //}
            if (lblStatus.Text == "5")
            {
                imgApprove.Visible = false;
                imgSTD.Visible = true;
            }
            if (lblStatus.Text == "7")
            {
                imgApprove.Visible = false;
                imgcancel.Visible = false;
                imgnotused.Visible = false;
            }
            if (lblStatus.Text == "8")
            {
                imgApprove.Visible = false;
                imgcancel.Visible = false;
                imgnotused.Visible = false;
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

            if (imgcancel != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgcancel.CommandName)) imgcancel.Visible = false;
            }

            if (imgnotused != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgnotused.CommandName)) imgnotused.Visible = false;
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
                cmdRevision.Attributes.Add("onclick", "openNewWindow('Revision', '', '" + Session["sitepath"] + "/Inspection/InspectionPEARSRARevision.aspx?RAID=" + lblRAid.Text + "'); return false;");
            }

        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
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

    protected void btncancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["RAID"] != null && ViewState["RAID"].ToString() != "")
            {
                PhoenixInspectionPEARSRiskAssessment.CancelRiskAssessment(new Guid(ViewState["RAID"].ToString()));
                ucStatus.Text = "Cancelled Successfully";
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

    protected void btnnotused_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["RAID"] != null && ViewState["RAID"].ToString() != "")
            {
                PhoenixInspectionPEARSRiskAssessment.NotUseRiskAssessment(new Guid(ViewState["RAID"].ToString()));
                ucStatus.Text = "Updated Successfully";
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

                ucStatus.Text = "RA is revised successfully.";
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

    protected void ddlVessel_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDVESSELID").CurrentFilterValue = e.Value;
        ViewState["VesselID"] = e.Value;

        SetFilter();
        ViewState["PAGENUMBER"] = 1;
        Rebind();
        gvPEARSRA.Rebind();
    }

    protected void ddlVessel_DataBinding(object sender, EventArgs e)
    {
        DataSet dt = new DataSet();
        dt = PhoenixRegistersVessel.ListOwnerAssignedVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(null), General.GetNullableInteger(ViewState["COMPANYID"].ToString()), General.GetNullableInteger(null), 0);
        RadComboBox ddlVessel = sender as RadComboBox;
        ddlVessel.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
        ddlVessel.DataSource = dt;

        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Tables[0].Columns["FLDVESSELID"];
        dt.Tables[0].PrimaryKey = keyColumns;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            ddlVessel.Enabled = false;
            ViewState["VesselID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        }

        if (ViewState["VesselID"] != null && ViewState["VesselID"].ToString() != "")
        {
            if (dt.Tables[0].Rows.Contains(ViewState["VesselID"].ToString()))
            {
                ddlVessel.SelectedValue = ViewState["VesselID"].ToString();
            }
        }
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

    protected void ddlStatus_DataBinding(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();

        ds = PhoenixInspectionPEARSRiskAssessment.ListRAStatus();
        RadComboBox ddlStatus = sender as RadComboBox;
        ddlStatus.DataSource = ds;
        ddlStatus.DataTextField = "FLDSTATUSNAME";
        ddlStatus.DataValueField = "FLDSTATUSID";
        ddlStatus.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    }
}