using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionRAProcessList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ViewState["REFNO"] = "";
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            ViewState["COMPANYID"] = "";
            NameValueCollection nvcCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvcCompany.Get("QMS") != null && nvcCompany.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvcCompany.Get("QMS");
            }

            BindCategory();

            NameValueCollection nvc = new NameValueCollection();
            nvc = Filter.CurrentProcessRAFilter;

            if (nvc != null)
            {
                txtHazardNo.Text = nvc.Get("txtHazardNo");
                ddlCategory.SelectedValue = nvc.Get("ddlCategory");
                ddlStatus.SelectedValue = nvc.Get("ddlStatus");
            }
            gvRiskAssessmentProcess.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRAProcessList.aspx", "New Template", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRAProcessList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessmentProcess')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRAProcessList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "SEARCH");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionRAProcessList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuProcess.AccessRights = this.ViewState;

        if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToString() != "")
        {
            lblStatus.Visible = false;
            ddlStatus.SelectedValue = "3";
            ddlStatus.Visible = false;
            ViewState["callfrom"] = Request.QueryString["callfrom"].ToString();
            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRAProcessList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRAProcessList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        }
        else
        {
            ViewState["callfrom"] = "";
        }
        MenuProcess.MenuList = toolbar.Show();
    }

    protected void BindCategory()
    {
        DataSet ds = new DataSet();

        //ds = PhoenixInspectionRiskAssessmentActivity.RiskAssessmentActivityByCategory(5);
        ds = PhoenixInspectionRiskAssessmentActivity.ListRiskAssessmentActivity(5, General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            ddlCategory.DataSource = ds.Tables[0];
            ddlCategory.DataBind();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNUMBER", "FLDDATE", "FLDTYPE", "FLDPROCESSNAME", "FLDJOBACTIVITY", "FLDSTATUSNAME", "FLDREVISIONNO", "FLDISSUEDDATE" };
        string[] alCaptions = { "Number", "Date", "Type", "Category", " Process", "Status", "Rev No", "Issued Date" };

        DataSet ds = PhoenixInspectionRiskAssessmentProcess.InspectionRiskAssessmentProcessSearch(
                                                                                General.GetNullableString(txtHazardNo.Text),
                                                                                General.GetNullableInteger(ddlCategory.SelectedValue),
                                                                                General.GetNullableInteger(ddlStatus.SelectedValue),
                                                                                General.GetNullableGuid(ViewState["REFNO"].ToString()),
                                                                                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                                                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                                gvRiskAssessmentProcess.PageSize,
                                                                                ref iRowCount,
                                                                                ref iTotalPageCount,
                                                                                General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        General.SetPrintOptions("gvRiskAssessmentProcess", "Risk Assessment-Process", alCaptions, alColumns, ds);

        gvRiskAssessmentProcess.DataSource = ds;
        gvRiskAssessmentProcess.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Filter.CurrentSelectedProcessRA == null)
            {
                Filter.CurrentSelectedProcessRA = ds.Tables[0].Rows[0]["FLDRISKASSESSMENTPROCESSID"].ToString();
                gvRiskAssessmentProcess.SelectedIndexes.Clear();
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
            string[] alColumns = { "FLDNUMBER", "FLDDATE", "FLDTYPE", "FLDPROCESSNAME", "FLDJOBACTIVITY", "FLDSTATUSNAME", "FLDREVISIONNO", "FLDISSUEDDATE" };
            string[] alCaptions = { "Number", "Date", "Type", "Category", " Process", "Status", "Rev No", "Issued Date" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixInspectionRiskAssessmentProcess.InspectionRiskAssessmentProcessSearch(
                                                                                General.GetNullableString(txtHazardNo.Text),
                                                                                General.GetNullableInteger(ddlCategory.SelectedValue),
                                                                                General.GetNullableInteger(ddlStatus.SelectedValue),
                                                                                General.GetNullableGuid(ViewState["REFNO"].ToString()),
                                                                                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                                                1,
                                                                                iRowCount,
                                                                                ref iRowCount,
                                                                                ref iTotalPageCount,
                                                                                General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

            General.ShowExcel("Risk Assessment-Process", ds.Tables[0], alColumns, alCaptions, null, null);

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

        if (CommandName.ToUpper().Equals("ADD"))
        {
            Response.Redirect("../Inspection/InspectionRAProcess.aspx?status=", false);
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
            Filter.CurrentProcessRAFilter = criteria;
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            txtHazardNo.Text = "";
            ddlStatus.SelectedIndex = 0;
            ddlCategory.SelectedIndex = 0;
            Filter.CurrentProcessRAFilter = null;
            Rebind();
        }
    }
    protected void gvRiskAssessmentProcess_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            //if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            //{
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblProcessID = (RadLabel)e.Item.FindControl("lblRiskAssessmentProcessID");
            //ImageButton imgPublish = (ImageButton)e.Row.FindControl("imgPublish");
            //imgPublish.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'InspectionRATemplateMapping.aspx?RISKASSESSMENTID=" + lblProcessID.Text + "&TYPE=4');return false;");                

            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            LinkButton cmdRAProcess = (LinkButton)e.Item.FindControl("cmdRAProcess");
            if (cmdRAProcess != null)
            {
                cmdRAProcess.Visible = SessionUtil.CanAccess(this.ViewState, cmdRAProcess.CommandName);
                cmdRAProcess.Attributes.Add("onclick", "openNewWindow('RAProcess', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=9&reportcode=RAPROCESS&processid=" + lblProcessID.Text + "&showmenu=0&showword=NO&showexcel=NO');return true;");
            }
            LinkButton imgrev = (LinkButton)e.Item.FindControl("imgrevision");
            if (imgrev != null) imgrev.Visible = SessionUtil.CanAccess(this.ViewState, imgrev.CommandName);

            RadLabel lblStatus = (RadLabel)e.Item.FindControl("lblStatus");
            RadLabel lblStatusID = (RadLabel)e.Item.FindControl("lblStatusID");
            RadLabel lblactiveyn = (RadLabel)e.Item.FindControl("lblActiveyn");
            RadLabel lblInstallcode = (RadLabel)e.Item.FindControl("lblInstallcode");
            LinkButton imgApprove = (LinkButton)e.Item.FindControl("imgApprove");
            LinkButton imgIssue = (LinkButton)e.Item.FindControl("imgIssue");
            LinkButton imgOfficeCopy = (LinkButton)e.Item.FindControl("imgOfficeCopy");

            LinkButton cmdRevisions = (LinkButton)e.Item.FindControl("cmdRevisions");
            RadLabel lblReferencid = (RadLabel)e.Item.FindControl("lblReferencid");

            if (cmdRevisions != null)
                cmdRevisions.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAProcessRevisionList.aspx?referenceid=" + lblReferencid.Text + "'); return true;");

            if (lblInstallcode != null && General.GetNullableInteger(lblInstallcode.Text) != null && int.Parse(lblInstallcode.Text) > 0)
            {
                if (imgApprove != null)
                    imgApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionRAApprovalRemarks.aspx?RATEMPLATEID=" + lblProcessID.Text + "&TYPE=4','medium'); return true;");
            }
            if (imgOfficeCopy != null)
                imgOfficeCopy.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionCopyJHARA.aspx?RAPROCESSID=" + lblProcessID.Text + "&TYPE=RAPROCESS','medium'); return true;");

            if (lblStatusID.Text == "1")
            {
                imgApprove.Visible = true;
                imgIssue.Visible = false;
            }

            else if (lblStatusID.Text == "2")
            {
                imgApprove.Visible = false;
                imgIssue.Visible = true;
            }
            else if (lblStatusID.Text == "3")
            {
                imgApprove.Visible = false;
                imgIssue.Visible = false;
            }

            if (lblStatus.Text.ToUpper() == "DRAFT" || lblStatus.Text.ToUpper() == "APPROVED" || lblactiveyn.Text == "0")
                if (imgrev != null) imgrev.Visible = false;

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                if (imgrev != null) imgrev.Visible = false;

            LinkButton imgCopy = (LinkButton)e.Item.FindControl("imgCopy");
            if (imgCopy != null) imgCopy.Visible = SessionUtil.CanAccess(this.ViewState, imgCopy.CommandName);

            if (lblStatus.Text.ToUpper() == "DRAFT" || lblStatus.Text.ToUpper() == "APPROVED")
                if (imgCopy != null) imgCopy.Visible = false;

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                if (imgCopy != null) imgCopy.Visible = false;
                if (imgOfficeCopy != null) imgOfficeCopy.Visible = false;
            }

            //ImageButton imgClearFilter = (ImageButton)e.Row.FindControl("imgClearFilter");
            //if (ViewState["REFNO"].ToString() != "")                
            //    imgClearFilter.Visible = true;                
            //else                
            //    imgClearFilter.Visible = false;

            //if (lblStatusID != null && lblStatusID.Text == "3" )
            //    imgPublish.Visible = true;

            if (imgrev != null && !SessionUtil.CanAccess(this.ViewState, imgrev.CommandName))
                imgrev.Visible = false;
            //if (imgPublish != null && !SessionUtil.CanAccess(this.ViewState, imgPublish.CommandName))
            //    imgPublish.Visible = false;

            ImageButton imgFilter = (ImageButton)e.Item.FindControl("imgFilter");
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                if (imgFilter != null) imgFilter.Visible = false;

            //if (imgPublish != null) imgPublish.Visible = false;

            if (imgApprove != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgApprove.CommandName)) imgApprove.Visible = false;
            }
            if (imgIssue != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgIssue.CommandName)) imgIssue.Visible = false;
            }
            if (imgrev != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgrev.CommandName)) imgrev.Visible = false;
            }
            if (imgFilter != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgFilter.CommandName)) imgFilter.Visible = false;
            }
            //if (imgClearFilter != null)
            //{
            //    if (!SessionUtil.CanAccess(this.ViewState, imgClearFilter.CommandName)) imgClearFilter.Visible = false;
            //}
            if (cmdRAProcess != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdRAProcess.CommandName)) cmdRAProcess.Visible = false;
            }
            if (imgCopy != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgCopy.CommandName)) imgCopy.Visible = false;
            }
            if (cmdRevisions != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdRevisions.CommandName)) cmdRevisions.Visible = false;
            }
            if (imgOfficeCopy != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgOfficeCopy.CommandName)) imgOfficeCopy.Visible = false;
            }
            //}
        }
    }

    protected void gvRiskAssessmentProcess_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        try
        {
            if (gce.CommandName.ToUpper().Equals("EDITROW"))
            {
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentProcessID");
                RadLabel lblstatus = (RadLabel)gce.Item.FindControl("lblStatusID");
                BindPageURL(gce.Item.ItemIndex);
                Response.Redirect("../Inspection/InspectionRAProcess.aspx?processid=" + lbl.Text + "&status=" + lblstatus.Text, false);
            }
            if (gce.CommandName.ToUpper().Equals("APPROVE"))
            {
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentProcessID");
                BindPageURL(gce.Item.ItemIndex);
                RadLabel lblInstallcode = (RadLabel)gce.Item.FindControl("lblInstallcode");
                if (lblInstallcode != null && lblInstallcode.Text == "0")
                {
                    PhoenixInspectionRiskAssessmentProcess.UpdateRiskAssessmentProcessApproval(
                                                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , new Guid(lbl.Text)
                                                                                    , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , null
                                                                                    , 2);
                    ucStatus.Text = "Approved Successfully";
                }
            }
            if (gce.CommandName.ToUpper().Equals("ISSUE"))
            {
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentProcessID");
                BindPageURL(gce.Item.ItemIndex);
                ViewState["PROCESSID"] = lbl.Text;
                RadWindowManager1.RadConfirm("RA cannot be edited after it is issued. Are you sure to continue?", "ConfirmIssue", 320, 150, null, "Confirm");
                return;
            }
            if (gce.CommandName.ToUpper().Equals("REVISION"))
            {
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentProcessID");
                BindPageURL(gce.Item.ItemIndex);
                ViewState["PROCESSID"] = lbl.Text;
                RadWindowManager1.RadConfirm("Are you sure you want to revise this RA.?", "ConfirmRevision", 320, 150, null, "Confirm");
                return;
            }
            if (gce.CommandName.ToUpper().Equals("FILTER"))
            {
                BindPageURL(gce.Item.ItemIndex);
                RadLabel lblrefno = (RadLabel)gce.Item.FindControl("lblReferencid");
                ViewState["REFNO"] = lblrefno.Text;
            }
            if (gce.CommandName.ToUpper().Equals("CLEARFILTER"))
            {
                BindPageURL(gce.Item.ItemIndex);
                ViewState["REFNO"] = "";
            }
            if (gce.CommandName.ToUpper().Equals("COPY"))
            {
                RadLabel lbl = (RadLabel)gce.Item.FindControl("lblRiskAssessmentProcessID");
                BindPageURL(gce.Item.ItemIndex);
                ViewState["PROCESSID"] = lbl.Text;
                RadWindowManager1.RadConfirm("Are you sure to copy the template.?", "Confirm", 320, 150, null, "Confirm");
                return;
            }
            if (gce.CommandName.ToUpper().Equals("RAPROCESS"))
            {
                BindPageURL(gce.Item.ItemIndex);
            }
            if (gce.CommandName.ToUpper().Equals("VIEWREVISION"))
            {
                BindPageURL(gce.Item.ItemIndex);
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
    protected void ucType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Rebind();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    protected void ddlCategory_Changed(object sender, EventArgs e)
    {
        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();
        criteria.Add("txtHazardNo", txtHazardNo.Text.Trim());
        criteria.Add("ddlCategory", ddlCategory.SelectedValue);
        criteria.Add("ddlStatus", ddlStatus.SelectedValue);
        Filter.CurrentProcessRAFilter = criteria;
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    protected void ddlStatus_Changed(object sender, EventArgs e)
    {
        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();
        criteria.Add("txtHazardNo", txtHazardNo.Text.Trim());
        criteria.Add("ddlCategory", ddlCategory.SelectedValue);
        criteria.Add("ddlStatus", ddlStatus.SelectedValue);
        Filter.CurrentProcessRAFilter = criteria;
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblRiskAssessmentProcessID = (RadLabel)gvRiskAssessmentProcess.Items[rowindex].FindControl("lblRiskAssessmentProcessID");
            if (lblRiskAssessmentProcessID != null)
            {
                Filter.CurrentSelectedProcessRA = lblRiskAssessmentProcessID.Text;
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
        try
        {
            gvRiskAssessmentProcess.SelectedIndexes.Clear();
            foreach (GridDataItem item in gvRiskAssessmentProcess.Items)
            {
                if (item.GetDataKeyValue("FLDRISKASSESSMENTPROCESSID").ToString() == Filter.CurrentSelectedProcessRA)
                {
                    gvRiskAssessmentProcess.SelectedIndexes.Add(item.ItemIndex);
                    break;
                }
            }
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
    protected void Rebind()
    {
        gvRiskAssessmentProcess.SelectedIndexes.Clear();
        gvRiskAssessmentProcess.EditIndexes.Clear();
        gvRiskAssessmentProcess.DataSource = null;
        gvRiskAssessmentProcess.Rebind();
    }
    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetRowSelection();
    }

    protected void ucConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["PROCESSID"] != null && ViewState["PROCESSID"].ToString() != "")
            {
                PhoenixInspectionRiskAssessmentProcess.CopyRiskAssessmentProcess(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                   new Guid(ViewState["PROCESSID"].ToString()),
                   PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                ucStatus.Text = "RA is copied.";
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
            if (ViewState["PROCESSID"] != null && ViewState["PROCESSID"].ToString() != "")
            {
                PhoenixInspectionRiskAssessmentProcess.UpdateRiskAssessmentProcessApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                              , new Guid(ViewState["PROCESSID"].ToString())
                                                                              , null
                                                                              , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                              , 3);
                ucStatus.Text = "RA is issued successfully";
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
            if (ViewState["PROCESSID"] != null && ViewState["PROCESSID"].ToString() != "")
            {
                PhoenixInspectionRiskAssessmentProcess.UpdateRiskAssessmentProcessRevision(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                new Guid(ViewState["PROCESSID"].ToString()));
                ucStatus.Text = "RA is revised.";
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
