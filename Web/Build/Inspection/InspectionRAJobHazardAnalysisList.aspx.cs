using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class InspectionRAJobHazardAnalysisList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            if (!IsPostBack)
            {
                BindStatus();

                ViewState["COMPANYID"] = "";
                NameValueCollection nvcCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvcCompany.Get("QMS") != null && nvcCompany.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvcCompany.Get("QMS");
                    ucVessel.Company = nvcCompany.Get("QMS");
                    ucVessel.bind();
                }
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }

                ViewState["REFNO"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                BindCategory();

                NameValueCollection nvc = new NameValueCollection();
                nvc = Filter.CurrentJHAFilter;

                if (nvc != null)
                {
                    txtHazardNo.Text = nvc.Get("txtHazardNo");
                    ddlCategory.SelectedValue = nvc.Get("ddlCategory");
                    ddlStatus.SelectedValue = nvc.Get("ddlStatus");
                }
                gvRiskAssessmentJobHazardAnalysis.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRAJobHazardAnalysisList.aspx", "New Template", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRAJobHazardAnalysisList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessmentJobHazardAnalysis')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRAJobHazardAnalysisList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRAJobHazardAnalysisList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToString() != "")
            {
                lblStatus.Visible = false;
                ddlStatus.SelectedValue = "3";
                ddlStatus.Visible = false;
                ViewState["callfrom"] = Request.QueryString["callfrom"].ToString();
                //ucTitle.ShowMenu = "false";
                toolbar = new PhoenixToolbar();
                toolbar.AddFontAwesomeButton("../Inspection/InspectionRAJobHazardAnalysisList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "SEARCH");
                toolbar.AddFontAwesomeButton("../Inspection/InspectionRAJobHazardAnalysisList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            }
            else
            {
                ViewState["callfrom"] = "";
                //ucTitle.ShowMenu = "true";
            }
            MenuJobHazardAnalysis.AccessRights = this.ViewState;
            MenuJobHazardAnalysis.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

        string[] alColumns = { "FLDHAZARDNUMBER", "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORY", "FLDJOB", "FLDSTATUS", "FLDREVISIONNO", "FLDISSUEDDATE" };
        string[] alCaptions = { "Hazard Number", "Vessel Name", "Type", "Category", "Job", "Status", "Revision No", "Issued Date" };

        DataSet ds = PhoenixInspectionRiskAssessmentJobHazard.RiskAssessmentJobHazardSearch(
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
                    General.GetNullableString(txtJob.Text),
                    General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        General.SetPrintOptions("gvRiskAssessmentJobHazardAnalysis", "Job Hazard Analysis", alCaptions, alColumns, ds);

        gvRiskAssessmentJobHazardAnalysis.DataSource = ds;
        gvRiskAssessmentJobHazardAnalysis.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Filter.CurrentSelectedJHA == null)
            {
                Filter.CurrentSelectedJHA = ds.Tables[0].Rows[0]["FLDJOBHAZARDID"].ToString();
                gvRiskAssessmentJobHazardAnalysis.SelectedIndexes.Clear();
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
            string[] alColumns = { "FLDHAZARDNUMBER", "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORY", "FLDJOB", "FLDSTATUS", "FLDREVISIONNO", "FLDISSUEDDATE" };
            string[] alCaptions = { "Hazard Number", "Vessel Name", "Type", "Category", "Job", "Status", "Revision No", "Issued Date" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixInspectionRiskAssessmentJobHazard.RiskAssessmentJobHazardSearch(
                    General.GetNullableString(txtHazardNo.Text),
                    General.GetNullableInteger(null),
                    General.GetNullableInteger(ddlCategory.SelectedValue),
                    General.GetNullableInteger(ddlStatus.SelectedValue),
                    General.GetNullableGuid(ViewState["REFNO"].ToString()),
                    null, null,
                    (int)ViewState["PAGENUMBER"],
                    General.ShowRecords(null),
                    ref iRowCount,
                    ref iTotalPageCount,
                    General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
                    General.GetNullableString(txtJob.Text),
                    General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

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
            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("txtHazardNo", txtHazardNo.Text.Trim());
            criteria.Add("ddlCategory", ddlCategory.SelectedValue);
            criteria.Add("ddlStatus", ddlStatus.SelectedValue);
            Filter.CurrentJHAFilter = criteria;
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            txtHazardNo.Text = "";
            ddlCategory.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
            txtJob.Text = "";

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ucVessel.Enabled = false;
            }
            else
            {
                ucVessel.Enabled = true;
                ucVessel.SelectedVessel = "";
            }

            //ucType.SelectedCategory = "";
            Filter.CurrentJHAFilter = null;
            ViewState["PAGENUMBER"] = 1;
            BindCategory();
            Rebind();
        }
    }
    protected void gvRiskAssessmentJobHazardAnalysis_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblInstallcode = (RadLabel)e.Item.FindControl("lblInstallcode");
            RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselid");
            RadLabel lblJobHazardid = (RadLabel)e.Item.FindControl("lblJobHazardid");

            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            LinkButton imgdelete = (LinkButton)e.Item.FindControl("cmdDelete");
            imgdelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                imgdelete.Visible = false;

            LinkButton imgrevision = (LinkButton)e.Item.FindControl("imgrevision");
            if (imgrevision != null) imgrevision.Visible = SessionUtil.CanAccess(this.ViewState, imgrevision.CommandName);

            RadLabel lblStatus = (RadLabel)e.Item.FindControl("lblStatus");
            RadLabel lblStatusid = (RadLabel)e.Item.FindControl("lblStatusID");
            RadLabel lblactiveyn = (RadLabel)e.Item.FindControl("lblActiveyn");
            if (lblStatus.Text.ToUpper() == "DRAFT" || lblStatus.Text.ToUpper() == "APPROVED" || lblactiveyn.Text == "0")
            {
                imgrevision.Visible = false;
            }
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                imgrevision.Visible = false;

            RadLabel lbljobhazardid = (RadLabel)e.Item.FindControl("lblJobHazardid");
            LinkButton imgAnalysis = (LinkButton)e.Item.FindControl("imgAnalysis");
            if (imgAnalysis != null)
            {
                imgAnalysis.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAJobHazardAnalysisReview.aspx?jobhazardid=" + lbljobhazardid.Text + "'); return false;");
            }

            LinkButton jobhzd = (LinkButton)e.Item.FindControl("cmdJobHazard");
            if (jobhzd != null)
            {
                jobhzd.Visible = SessionUtil.CanAccess(this.ViewState, jobhzd.CommandName);
                jobhzd.Attributes.Add("onclick", "openNewWindow('JobHazard', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=9&reportcode=JOBHAZARD&jobhazardid=" + lbljobhazardid.Text + "&showmenu=0&showword=NO&showexcel=NO');return true;");
            }

            LinkButton cmdRevisions = (LinkButton)e.Item.FindControl("cmdRevisions");
            LinkButton imgCopy = (LinkButton)e.Item.FindControl("imgCopy");
            LinkButton imgOfficeCopy = (LinkButton)e.Item.FindControl("imgOfficeCopy");

            RadLabel lblReferencid = (RadLabel)e.Item.FindControl("lblReferencid");
            LinkButton imgCopyTemplate = (LinkButton)e.Item.FindControl("imgCopyTemplate");

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                imgCopy.Visible = false;
                imgdelete.Visible = false;
                imgOfficeCopy.Visible = false;
            }

            if (cmdRevisions != null)
                cmdRevisions.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAJobHazardAnalysisRevisionList.aspx?referenceid=" + lblReferencid.Text + "'); return true;");

            if (imgOfficeCopy != null)
                imgOfficeCopy.Attributes.Add("onclick", "openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionCopyJHARA.aspx?JOBHAZARDID=" + lbljobhazardid.Text + "&Type=JHA','medium'); return true;");
            if (imgrevision != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgrevision.CommandName)) imgrevision.Visible = false;
            }
            if (jobhzd != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, jobhzd.CommandName)) jobhzd.Visible = false;
            }
            if (cmdRevisions != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdRevisions.CommandName)) cmdRevisions.Visible = false;
            }
            if (imgCopy != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgCopy.CommandName)) imgCopy.Visible = false;
            }
            if (imgdelete != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgdelete.CommandName)) imgdelete.Visible = false;
            }
            if (imgOfficeCopy != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgOfficeCopy.CommandName)) imgOfficeCopy.Visible = false;
            }


            imgrevision.Visible = false;
            imgCopy.Visible = false;

            if (lblVesselid != null && lblVesselid.Text == "0")
            {
                if (cmdRevisions != null) cmdRevisions.Visible = true;
            }
            else
            {
                if (cmdRevisions != null) cmdRevisions.Visible = false;
            }

            if (lblStatusid.Text == "1")
            {
                imgrevision.Visible = false;
                imgCopy.Visible = false;
            }
            else if (lblStatusid.Text == "2")
            {
                imgrevision.Visible = false;
                imgCopy.Visible = false;
            }
            else if (lblStatusid.Text == "3")
            {
                if (lblVesselid != null && lblVesselid.Text == "0" && drv["FLDACTIVEYN"].ToString() == "1" && PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                    imgrevision.Visible = true;
                else
                    imgrevision.Visible = false;

                imgCopy.Visible = true;
            }
            else if (lblStatusid.Text == "4")
            {
                imgrevision.Visible = false;
                imgCopy.Visible = false;
            }
            else if (lblStatusid.Text == "5")
            {
                if (lblVesselid != null && lblVesselid.Text == "0" && drv["FLDACTIVEYN"].ToString() == "1" && PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                    imgrevision.Visible = true;
                else
                    imgrevision.Visible = false;
                imgCopy.Visible = true;
            }
            else if (lblStatusid.Text == "6")
            {
                if (lblVesselid != null && lblVesselid.Text == "0" && drv["FLDACTIVEYN"].ToString() == "1" && PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                    imgrevision.Visible = true;
                else
                    imgrevision.Visible = false;
                imgCopy.Visible = false;
            }
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                if (imgCopy != null) imgCopy.ToolTip = "Use JHA";
            }
            else
            {
                if (imgCopy != null) imgCopy.ToolTip = "Copy JHA";
            }
            if (imgCopy != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgCopy.CommandName)) imgCopy.Visible = false;
            }
        }
    }

    protected void gvRiskAssessmentJobHazardAnalysis_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        try
        {
            if (gce.CommandName.ToUpper().Equals("EDITROW"))
            {
                BindPageURL(gce.Item.ItemIndex);
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblJobHazardid");
                RadLabel lblstatus = (RadLabel)gce.Item.FindControl("lblStatusID");
                Response.Redirect("../Inspection/InspectionRAJobHazardAnalysis.aspx?jobhazardid=" + lbl.Text + "&status=" + lblstatus.Text, false);
                Rebind();
            }

            if (gce.CommandName.ToUpper().Equals("REVISION"))
            {
                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
                RadWindowManager1.RadConfirm("Are you sure you want to revise this JHA.?", "ConfirmReviseJHA", 320, 150, null, "Confirm");
                return;
            }
            if (gce.CommandName.ToUpper().Equals("FILTER"))
            {
                BindPageURL(gce.Item.ItemIndex);
                RadLabel lbljobhazardid = (RadLabel)gce.Item.FindControl("lblJobHazardid");
                RadLabel lblrefno = (RadLabel)gce.Item.FindControl("lblReferencid");
                ViewState["REFNO"] = lblrefno.Text;
                Rebind();
            }
            if (gce.CommandName.ToUpper().Equals("CLEARFILTER"))
            {
                BindPageURL(gce.Item.ItemIndex);
                ViewState["REFNO"] = "";
                Rebind();
            }
            if (gce.CommandName.ToUpper().Equals("JOBHAZARDREPORT"))
            {
                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
            }
            if (gce.CommandName.ToUpper().Equals("VIEWREVISION"))
            {
                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
            }
            if (gce.CommandName.ToUpper().Equals("COPY"))
            {
                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
                RadWindowManager1.RadConfirm("Are you sure you want to copy this JHA.?", "ConfirmCopyJHA", 320, 150, null, "Confirm");
                return;
            }
            if (gce.CommandName.ToUpper().Equals("OFFICECOPY"))
            {
                BindPageURL(gce.Item.ItemIndex);
                SetRowSelection();
            }
            if (gce.CommandName.ToUpper().Equals("APPROVE"))
            {
                BindPageURL(gce.Item.ItemIndex);
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblJobHazardid");
                RadLabel lblInstallcode = (RadLabel)gce.Item.FindControl("lblInstallcode");
                RadLabel lblVesselid = (RadLabel)gce.Item.FindControl("lblVesselid");
                if (lblInstallcode != null && lblInstallcode.Text == "0")
                {
                    if (lblVesselid != null && int.Parse(lblVesselid.Text) == 0)
                    {
                        PhoenixInspectionRiskAssessmentJobHazard.UpdateRiskAssessmentJobHazardApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                                        new Guid(lbl.Text), PhoenixSecurityContext.CurrentSecurityContext.UserCode, null, 2);
                        ucStatus.Text = "Approved Successfully";
                    }
                }
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

    protected void BindCategory()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentActivity.ListRiskAssessmentActivity(General.GetNullableInteger(null), General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            ddlCategory.DataSource = ds.Tables[0];
            ddlCategory.DataBind();
        }
    }

    protected void ucType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindCategory();
    }

    protected void ddlCategory_Changed(object sender, EventArgs e)
    {
        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();
        criteria.Add("txtHazardNo", txtHazardNo.Text.Trim());
        criteria.Add("ddlCategory", ddlCategory.SelectedValue);
        criteria.Add("ddlStatus", ddlStatus.SelectedValue);
        Filter.CurrentJHAFilter = criteria;
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    protected void ddlStatus_Changed(object sender, EventArgs e)
    {

    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblJobHazardid = (RadLabel)gvRiskAssessmentJobHazardAnalysis.Items[rowindex].FindControl("lblJobHazardid");
            if (lblJobHazardid != null)
            {
                Filter.CurrentSelectedJHA = lblJobHazardid.Text;
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
        gvRiskAssessmentJobHazardAnalysis.SelectedIndexes.Clear();
        for (int i = 0; i < gvRiskAssessmentJobHazardAnalysis.Items.Count; i++)
        {
            if (gvRiskAssessmentJobHazardAnalysis.MasterTableView.Items[i].GetDataKeyValue("FLDJOBHAZARDID").ToString().Equals(Filter.CurrentSelectedJHA.ToString()))
            {
                gvRiskAssessmentJobHazardAnalysis.MasterTableView.Items[i].Selected = true;
            }
        }
    }
    protected void gvRiskAssessmentJobHazardAnalysis_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadLabel lbljobhazardid = ((RadLabel)e.Item.FindControl("lblJobHazardid"));
            PhoenixInspectionRiskAssessmentJobHazard.DeleteRiskAssessmentJobHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(lbljobhazardid.Text));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

    protected void ucConfirmCopy_Click(object sender, EventArgs e)
    {
        try
        {
            string jobhazardid = Filter.CurrentSelectedJHA;
            if (jobhazardid != null && jobhazardid != "" && General.GetNullableGuid(jobhazardid) != null)
            {
                PhoenixInspectionRiskAssessmentJobHazard.RiskAssessmentJobHazardCopy(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , new Guid(jobhazardid)
                                                                                    , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()));
                ucStatus.Text = "JHA is copied successfully.";
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

    protected void ucConfirmRevision_Click(object sender, EventArgs e)
    {
        try
        {
            string jobhazardid = Filter.CurrentSelectedJHA;
            if (jobhazardid != null && jobhazardid != "" && General.GetNullableGuid(jobhazardid) != null)
            {
                PhoenixInspectionRiskAssessmentJobHazard.UpdateRiskAssessmentJobHazardRevision(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(jobhazardid));

                Rebind();
                ucStatus.Text = "JHA is revised.";
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
