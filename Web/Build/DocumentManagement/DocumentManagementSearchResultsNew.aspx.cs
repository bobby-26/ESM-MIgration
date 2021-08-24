using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Integration;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class DocumentManagement_DocumentManagementSearchResultsNew : PhoenixBasePage
{ 
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            MenuDocumentList.AccessRights = this.ViewState;
            MenuDocumentList.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["DOCUMENTID"] = "";

                ViewState["CURRENTINDEXSECTION"] = 1;
                ViewState["SECTIONID"] = "";

                ViewState["CURRENTINDEXFORM"] = 1;
                ViewState["FORMID"] = "";

                ViewState["CURRENTINDEXRISKASSESSMENT"] = 1;
                ViewState["PROCESSID"] = "";

                ViewState["CURRENTINDEXJOBHAZARD"] = 1;
                ViewState["JOBHAZARDID"] = "";

                if (Request.QueryString["keyword"] != null && Request.QueryString["keyword"].ToString() != "")
                    ViewState["keyword"] = Request.QueryString["keyword"].ToString();
                else
                    ViewState["keyword"] = "";

                lblSectionTitle.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Section View','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementSectionView.aspx?DMSYN=1&keyword=" + ViewState["keyword"] + "',false,900,600);return false;");
                lblFormTitle.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Form View','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementFormView.aspx?DMSYN=1&keyword=" + ViewState["keyword"] + "',false,900,600);return false;");
                lblRiskAssessmentTitle.Attributes.Add("onclick", "openNewWindow('Risk Assessment View', '','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementRAView.aspx?DMSYN=1&keyword=" + ViewState["keyword"] + "',false,900,600);return false;");
                lblJobHazardTitle.Attributes.Add("onclick", "openNewWindow('codehelp1', 'JHA View','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementJHAView.aspx?DMSYN=1&keyword=" + ViewState["keyword"] + "',false,900,600);return false;");

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvSectionMatches_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindSectionMatches();
    }

    protected void gvJobHazard_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindJobHazard();
    }

    protected void gvFormMatch_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindFormMatches();
    }

    protected void gvRiskAssessment_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindRiskAssessment();
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }


    // ----- SECTION ----- //

    private void BindSectionMatches()
    {
        int iRowCount = 0;

        DataSet ds = new DataSet();

        int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        ds = PhoenixDocumentManagementDocument.KeyWordSearchResultInNewSection(
                                                                  General.GetNullableString(ViewState["keyword"].ToString())
                                                                , ref iRowCount
                                                                , companyid
                                                                );


        gvSectionMatches.DataSource = ds;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (General.GetNullableGuid(ViewState["SECTIONID"].ToString()) == null)
            {
                lblSectionTitle.Text = ds.Tables[0].Rows[0]["FLDSEARCHRESULT"].ToString();
                ViewState["SECTIONID"] = ds.Tables[0].Rows[0]["FLDSECTIONID"].ToString();
            }
        }
    }

    protected void gvSectionMatches_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

            if (e.CommandName.ToUpper().Equals("CHANGEPAGESIZE"))
                return;

            int nCurrentRow = e.Item.ItemIndex;

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                RadLabel lblSectionId = ((RadLabel)gvSectionMatches.Items[nCurrentRow].FindControl("lblSectionId"));
                if (lblSectionId != null)
                    ViewState["SECTIONID"] = lblSectionId.Text;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvSectionMatches_RowDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            UserControlToolTip ucDocumentName = (UserControlToolTip)e.Item.FindControl("ucDocumentName");
            RadLabel lblDocumentName = (RadLabel)e.Item.FindControl("lblDocumentName");
            if (lblDocumentName != null)
            {
                //lblDocumentName.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucDocumentName.ToolTip + "', 'visible');");
                //lblDocumentName.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucDocumentName.ToolTip + "', 'hidden');");
                ucDocumentName.Position = ToolTipPosition.TopCenter;
                ucDocumentName.TargetControlId = lblDocumentName.ClientID;
            }

            UserControlToolTip ucSectionName = (UserControlToolTip)e.Item.FindControl("ucSectionName");
            LinkButton lnkSectionName = (LinkButton)e.Item.FindControl("lnkSectionName");
            if (lnkSectionName != null)
            {
                //lnkSectionName.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucSectionName.ToolTip + "', 'visible');");
                //lnkSectionName.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucSectionName.ToolTip + "', 'hidden');");
                ucSectionName.Position = ToolTipPosition.TopCenter;
                ucSectionName.TargetControlId = lnkSectionName.ClientID;
            }
            //string onclickformevent = @"javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementLinkedForms.aspx?sectionid=" + dr["FLDSECTIONID"].ToString() + @"&revisionid=" + dr["FLDREVISONID"].ToString() + @"'); return false;""";

            RadLabel lblSectionId = (RadLabel)e.Item.FindControl("lblSectionId");
            LinkButton lnkReadUnreadSec = (LinkButton)e.Item.FindControl("lnkReadUnreadSec");
            if (lnkReadUnreadSec != null)
            {
                SessionUtil.CanAccess(this.ViewState, lnkReadUnreadSec.CommandName);
                lnkReadUnreadSec.Attributes.Add("onclick", "openNewWindow('ReadUnread', '" + lnkSectionName.Text + " - Read/Unread User List', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionReadUnreadList.aspx?SECTIONID=" + lblSectionId.Text + "');return false;");
            }
            LinkButton cmdForms = (LinkButton)e.Item.FindControl("cmdForms");
            DataRowView dv = (DataRowView)e.Item.DataItem;

            if (dv["FLDFORMSPOSTERCHECKLISTIDS"].ToString() == null || dv["FLDFORMSPOSTERCHECKLISTIDS"].ToString() == string.Empty)
            {
                if (cmdForms != null)
                    cmdForms.Visible = false;
            }
            if (cmdForms != null)
            {
                SessionUtil.CanAccess(this.ViewState, cmdForms.CommandName);
                cmdForms.Attributes.Add("onclick", "openNewWindow('Forms', 'Forms and Checklist', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementLinkedForms.aspx?sectionid=" + lblSectionId.Text + "',false,500,300);return false;");
            }

            if (lnkSectionName != null)
            {
                lnkSectionName.Visible = SessionUtil.CanAccess(this.ViewState, lnkSectionName.CommandName);
                lnkSectionName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?callfrom=documenttree&SECTIONID=" + lblSectionId.Text + "',false,900,600);return false;");
            }


        }
    }

    protected void gvSectionMatches_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {

        Label lblSectionId = ((Label)gvSectionMatches.Items[se.NewSelectedIndex].FindControl("lblSectionId"));
        if (lblSectionId != null)
            ViewState["SECTIONID"] = lblSectionId.Text;
    }

    // ----- FORM ----- //

    private void BindFormMatches()
    {
        try
        {
            int iRowCount = 0;

            int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

            DataSet ds = PhoenixDocumentManagementDocument.KeyWordSearchResultInNewForm(
                                                                  General.GetNullableString(ViewState["keyword"].ToString())
                                                                , ref iRowCount
                                                                , companyid
                                                                );


            gvFormMatch.DataSource = ds;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (General.GetNullableGuid(ViewState["FORMID"].ToString()) == null)
                {
                    lblFormTitle.Text = ds.Tables[0].Rows[0]["FLDSEARCHRESULT"].ToString();
                    ViewState["FORMID"] = ds.Tables[0].Rows[0]["FLDFORMID"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFormMatch_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

            if (e.CommandName.ToUpper().Equals("CHANGEPAGESIZE"))
                return;

            int nCurrentRow = e.Item.ItemIndex;

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                RadLabel lblFormId = ((RadLabel)gvFormMatch.Items[nCurrentRow].FindControl("lblFormId"));
                if (lblFormId != null)
                    ViewState["FORMID"] = lblFormId.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Please make the required correction";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvFormMatch_RowDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            UserControlToolTip ucFormName = (UserControlToolTip)e.Item.FindControl("ucFormName");
            LinkButton lnkFormName = (LinkButton)e.Item.FindControl("lnkFormName");
            RadLabel lblFormId = (RadLabel)e.Item.FindControl("lblFormId");

            RadLabel lbltype = (RadLabel)e.Item.FindControl("lbltype");
            LinkButton lblName = (LinkButton)e.Item.FindControl("lnkFormName");
            if (lnkFormName != null)
            {
                //lnkFormName.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucFormName.ToolTip + "', 'visible');");
                //lnkFormName.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucFormName.ToolTip + "', 'hidden');");
                ucFormName.Position = ToolTipPosition.TopCenter;
                ucFormName.TargetControlId = lnkFormName.ClientID;

                if (lblFormId != null)
                {

                    if ((lbltype.Text == "5") && (lblName != null))
                    {
                        lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + lblFormId.Text + "&FORMREVISIONID=" + drv["FLDFORMREVISIONID"].ToString() + "',false,700,700);return false;");
                    }
                    if ((lbltype.Text == "6") && (lblName != null))
                    {
                       lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Common/download.aspx?formid=" + lblFormId.Text + "',false,700,700);return false;");
                    }
                }
            }

            LinkButton cmdDoc = (LinkButton)e.Item.FindControl("cmdDocuments");
            if (cmdDoc != null)
            {
                cmdDoc.Attributes.Add("onclick", "javascript:return openNewWindow('Document', '', 'DocumentManagement/DocumentManagementDocumentLinked.aspx?FORMID=" + lblFormId.Text + "',false,550,300); return false;");
            }

        }
    }

    protected void gvFormMatch_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        RadLabel lblFormId = ((RadLabel)gvFormMatch.Items[se.NewSelectedIndex].FindControl("lblFormId"));
        if (lblFormId != null)
            ViewState["FORMID"] = lblFormId.Text;
    }
    protected string GetParentIframeURL(string referenceid)
    {
        string strURL = "";
        int type = 0;
        DataSet ds = PhoenixDocumentManagementDocument.GetNewSelectedeTreeNodeType(General.GetNullableGuid(referenceid), ref type);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            strURL = dr["FLDURL"].ToString();
        }
        return strURL;
    }


    // ----- RA ----- //

    private void BindRiskAssessment()
    {
        try
        {
            int iRowCount = 0;

            int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

            DataSet ds = PhoenixDocumentManagementDocument.KeyWordSearchResultInNewRA(
                                                                  General.GetNullableString(ViewState["keyword"].ToString())
                                                                , ref iRowCount
                                                                , companyid
                                                                );

            gvRiskAssessment.DataSource = ds;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (General.GetNullableGuid(ViewState["PROCESSID"].ToString()) == null)
                {
                    lblRiskAssessmentTitle.Text = ds.Tables[0].Rows[0]["FLDSEARCHRESULT"].ToString();
                    ViewState["PROCESSID"] = ds.Tables[0].Rows[0]["FLDPROCESSID"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvRiskAssessment_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

            if (e.CommandName.ToUpper().Equals("CHANGEPAGESIZE"))
                return;

            int nCurrentRow = e.Item.ItemIndex;

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                RadLabel lblProcessId = ((RadLabel)gvRiskAssessment.Items[nCurrentRow].FindControl("lblProcessId"));
                if (lblProcessId != null)
                    ViewState["PROCESSID"] = lblProcessId.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Please make the required correction";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvRiskAssessment_RowDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            UserControlToolTip ucActivityCondition = (UserControlToolTip)e.Item.FindControl("ucActivityCondition");
            RadLabel lblActivityCondition = (RadLabel)e.Item.FindControl("lblActivityCondition");
            if (lblActivityCondition != null)
            {
                //lblActivityCondition.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucActivityCondition.ToolTip + "', 'visible');");
                //lblActivityCondition.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucActivityCondition.ToolTip + "', 'hidden');");
                ucActivityCondition.Position = ToolTipPosition.TopCenter;
                ucActivityCondition.TargetControlId = lblActivityCondition.ClientID;
            }

            UserControlToolTip ucProcessName = (UserControlToolTip)e.Item.FindControl("ucProcessName");
            RadLabel lblProcessName = (RadLabel)e.Item.FindControl("lblProcessName");
            if (lblProcessName != null)
            {
                //lblProcessName.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucProcessName.ToolTip + "', 'visible');");
                //lblProcessName.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucProcessName.ToolTip + "', 'hidden');");
                ucProcessName.Position = ToolTipPosition.TopCenter;
                ucProcessName.TargetControlId = lblProcessName.ClientID;
            }

            LinkButton lnkRefNo = (LinkButton)e.Item.FindControl("lnkRefNo");
            RadLabel lblProcessId = (RadLabel)e.Item.FindControl("lblProcessId");
            if (lnkRefNo != null)
            {
                lnkRefNo.Visible = SessionUtil.CanAccess(this.ViewState, lnkRefNo.CommandName);
                lnkRefNo.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Inspection/InspectionRAProcessExtn.aspx?DMSYN=1&processid=" + lblProcessId.Text + "',false,900,600);return false;");
            }
        }
    }

    protected void gvRiskAssessment_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        RadLabel lblProcessId = ((RadLabel)gvRiskAssessment.Items[se.NewSelectedIndex].FindControl("lblProcessId"));
        if (lblProcessId != null)
            ViewState["PROCESSID"] = lblProcessId.Text;
    }

    // ----- JHA ----- //

    private void BindJobHazard()
    {
        try
        {
            int iRowCount = 0;

            int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

            DataSet ds = PhoenixDocumentManagementDocument.KeyWordSearchResultInNewJHA(
                                                                  General.GetNullableString(ViewState["keyword"].ToString())
                                                                , ref iRowCount
                                                                , companyid
                                                                );

            gvJobHazard.DataSource = ds;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (General.GetNullableGuid(ViewState["JOBHAZARDID"].ToString()) == null)
                {
                    lblJobHazardTitle.Text = ds.Tables[0].Rows[0]["FLDSEARCHRESULT"].ToString();
                    ViewState["JOBHAZARDID"] = ds.Tables[0].Rows[0]["FLDJOBHAZARDID"].ToString();
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvJobHazard_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

            if (e.CommandName.ToUpper().Equals("CHANGEPAGESIZE"))
                return;

            int nCurrentRow = e.Item.ItemIndex;

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {

                RadLabel lblJobHazardId = ((RadLabel)gvJobHazard.Items[nCurrentRow].FindControl("lblJobHazardId"));
                if (lblJobHazardId != null)
                    ViewState["JOBHAZARDID"] = lblJobHazardId.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Please make the required correction";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvJobHazard_RowDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            UserControlToolTip ucJob = (UserControlToolTip)e.Item.FindControl("ucJob");
            RadLabel lblJob = (RadLabel)e.Item.FindControl("lblJob");
            if (lblJob != null)
            {
                ucJob.Position = ToolTipPosition.TopCenter;
                ucJob.TargetControlId = lblJob.ClientID;
            }

            LinkButton lnkRefNo = (LinkButton)e.Item.FindControl("lnkRefNo");
            RadLabel lblJobHazardId = (RadLabel)e.Item.FindControl("lblJobHazardId");
            if (lnkRefNo != null)
            {
                lnkRefNo.Visible = SessionUtil.CanAccess(this.ViewState, lnkRefNo.CommandName);
                lnkRefNo.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Inspection/InspectionRAJobHazardAnalysisExtn.aspx?DMSYN=1&jobhazardid=" + lblJobHazardId.Text + "',false,900,600);return false;");
            }
        }

    }
    protected void gvJobHazard_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        RadLabel lblJobHazardId = ((RadLabel)gvJobHazard.Items[se.NewSelectedIndex].FindControl("lblJobHazardId"));
        if (lblJobHazardId != null)
            ViewState["JOBHAZARDID"] = lblJobHazardId.Text;
    }
}
