using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using Telerik.Web.UI;

public partial class DocumentManagementSearchResults : PhoenixBasePage
{

    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvSearchResults.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvSearchResults.UniqueID, "Select$" + r.RowIndex.ToString());
    //        }
    //    }
    //    foreach (GridViewRow r in gvSectionMatches.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvSectionMatches.UniqueID, "Select$" + r.RowIndex.ToString());
    //        }
    //    }
    //    foreach (GridViewRow r in gvFormMatch.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvFormMatch.UniqueID, "Select$" + r.RowIndex.ToString());
    //        }
    //    }
    //    foreach (GridViewRow r in gvJobHazard.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvJobHazard.UniqueID, "Select$" + r.RowIndex.ToString());
    //        }
    //    }
    //    foreach (GridViewRow r in gvRiskAssessment.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvRiskAssessment.UniqueID, "Select$" + r.RowIndex.ToString());
    //        }
    //    }
    //    base.Render(writer);
    //}

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
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["DOCUMENTID"] = "";

                ViewState["PAGENUMBERSECTION"] = 1;
                ViewState["SORTEXPRESSIONSECTION"] = null;
                ViewState["SORTDIRECTIONSECTION"] = null;
                ViewState["CURRENTINDEXSECTION"] = 1;
                ViewState["SECTIONID"] = "";

                ViewState["PAGENUMBERFORM"] = 1;
                ViewState["SORTEXPRESSIONFORM"] = null;
                ViewState["SORTDIRECTIONFORM"] = null;
                ViewState["CURRENTINDEXFORM"] = 1;
                ViewState["FORMID"] = "";

                ViewState["PAGENUMBERRISKASSESSMENT"] = 1;
                ViewState["SORTEXPRESSIONRISKASSESSMENT"] = null;
                ViewState["SORTDIRECTIONRISKASSESSMENT"] = null;
                ViewState["CURRENTINDEXRISKASSESSMENT"] = 1;
                ViewState["PROCESSID"] = "";

                ViewState["PAGENUMBERJOBHAZARD"] = 1;
                ViewState["SORTEXPRESSIONJOBHAZARD"] = null;
                ViewState["SORTDIRECTIONJOBHAZARD"] = null;
                ViewState["CURRENTINDEXJOBHAZARD"] = 1;
                ViewState["JOBHAZARDID"] = "";

                if (Request.QueryString["keyword"] != null && Request.QueryString["keyword"].ToString() != "")
                    ViewState["keyword"] = Request.QueryString["keyword"].ToString();
                else
                    ViewState["keyword"] = "";

                gvSearchResults.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvSectionMatches.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvFormMatch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvRiskAssessment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvJobHazard.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
            //BindData();
            //BindSectionMatches();
            //BindFormMatches();
            //BindRiskAssessment();
            //BindJobHazard();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }


    protected void gvSearchResults_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
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


    // ----- DOCUMENT ----- //

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        ds = PhoenixDocumentManagementDocument.KeyWordSearchResult(
                                                                  General.GetNullableString(ViewState["keyword"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , gvSearchResults.CurrentPageIndex + 1
                                                                , gvSearchResults.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , companyid
                                                                );
        gvSearchResults.DataSource = ds;
        gvSearchResults.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;


        if (ds.Tables[0].Rows.Count > 0)
        {

            if (General.GetNullableGuid(ViewState["DOCUMENTID"].ToString()) == null)
            {
                lblDocumentTitle.Text = ds.Tables[0].Rows[0]["FLDSEARCHRESULT"].ToString();
                ViewState["DOCUMENTID"] = ds.Tables[0].Rows[0]["FLDDOCUMENTID"].ToString();
            }
            SetRowSelection();
        }
    }

    protected void gvSearchResults_RowCommand(object sender, GridCommandEventArgs e)
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
                BindPageURL(nCurrentRow);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvSearchResults_ItemDataBound(Object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)item.DataItem;

            ImageButton del = (ImageButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            if (!e.Item.ItemType.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.ItemType.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }
            }
        }
        if (e.Item is GridFooterItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    protected void gvSearchResults_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        BindPageURL(se.NewSelectedIndex);
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["DOCUMENTID"] = ((Label)gvSearchResults.Items[rowindex].FindControl("lblDocumentId")).Text;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    private void SetRowSelection()
    {

        foreach (GridDataItem item in gvSearchResults.Items)
        {

            if (item.GetDataKeyValue("FLDDOCUMENTID").ToString().Equals(ViewState["DOCUMENTID"].ToString()))
            {
                gvSearchResults.SelectedIndexes.Add(item.ItemIndex);
                //ViewState["DOCUMENTID"] = item.GetDataKeyValue("FLDDOCUMENTID").ToString();
            }
        }
    }

    // ----- SECTION ----- //

    private void BindSectionMatches()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSIONSECTION"] == null) ? null : (ViewState["SORTEXPRESSIONSECTION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTIONSECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONSECTION"].ToString());

        DataSet ds = new DataSet();

        int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        ds = PhoenixDocumentManagementDocument.KeyWordSearchResultInSection(
                                                                  General.GetNullableString(ViewState["keyword"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , gvSectionMatches.CurrentPageIndex + 1
                                                                , gvSectionMatches.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , companyid
                                                                );


        gvSectionMatches.DataSource = ds;
        gvSectionMatches.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNTSECTION"] = iRowCount;
        ViewState["TOTALPAGECOUNTSECTION"] = iTotalPageCount;

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
                Label lblSectionId = ((Label)gvSectionMatches.Items[nCurrentRow].FindControl("lblSectionId"));
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
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSIONFORM"] == null) ? null : (ViewState["SORTEXPRESSIONFORM"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTIONFORM"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONFORM"].ToString());

            int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

            DataSet ds = PhoenixDocumentManagementDocument.KeyWordSearchResultInForm(
                                                                  General.GetNullableString(ViewState["keyword"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , gvFormMatch.CurrentPageIndex + 1
                                                                , gvFormMatch.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , companyid
                                                                );


            gvFormMatch.DataSource = ds;
            gvFormMatch.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNTFORM"] = iRowCount;
            ViewState["TOTALPAGECOUNTFORM"] = iTotalPageCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (General.GetNullableGuid(ViewState["FORMID"].ToString()) == null)
                {
                    lblFormTitle.Text = ds.Tables[0].Rows[0]["FLDSEARCHRESULT"].ToString();
                    ViewState["FORMID"] = ds.Tables[0].Rows[0]["FLDFORMID"].ToString();
                }
                //SetRowSelection();
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
            UserControlToolTip ucFormName = (UserControlToolTip)e.Item.FindControl("ucFormName");
            LinkButton lnkFormName = (LinkButton)e.Item.FindControl("lnkFormName");
            if (lnkFormName != null)
            {
                //lnkFormName.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucFormName.ToolTip + "', 'visible');");
                //lnkFormName.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucFormName.ToolTip + "', 'hidden');");
                ucFormName.Position = ToolTipPosition.TopCenter;
                ucFormName.TargetControlId = lnkFormName.ClientID;
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
        DataSet ds = PhoenixDocumentManagementDocument.GetSelectedeTreeNodeType(General.GetNullableGuid(referenceid), ref type);
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
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSIONRISKASSESSMENT"] == null) ? null : (ViewState["SORTEXPRESSIONRISKASSESSMENT"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTIONRISKASSESSMENT"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONRISKASSESSMENT"].ToString());

            int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

            DataSet ds = PhoenixDocumentManagementDocument.KeyWordSearchResultInProcessRA(
                                                                  General.GetNullableString(ViewState["keyword"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , gvRiskAssessment.CurrentPageIndex + 1
                                                                , gvRiskAssessment.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , companyid
                                                                );

            gvRiskAssessment.DataSource = ds;
            gvRiskAssessment.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNTRISKASSESSMENT"] = iRowCount;
            ViewState["TOTALPAGECOUNTRISKASSESSMENT"] = iTotalPageCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (General.GetNullableGuid(ViewState["PROCESSID"].ToString()) == null)
                {
                    lblRiskAssessmentTitle.Text = ds.Tables[0].Rows[0]["FLDSEARCHRESULT"].ToString();
                    ViewState["PROCESSID"] = ds.Tables[0].Rows[0]["FLDPROCESSID"].ToString();
                }
                //SetRowSelection();
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

        if (e.Item is GridDataItem)
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
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSIONJOBHAZARD"] == null) ? null : (ViewState["SORTEXPRESSIONJOBHAZARD"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTIONJOBHAZARD"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONJOBHAZARD"].ToString());

            int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

            DataSet ds = PhoenixDocumentManagementDocument.KeyWordSearchResultInJHA(
                                                                  General.GetNullableString(ViewState["keyword"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , gvJobHazard.CurrentPageIndex + 1
                                                                , gvJobHazard.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , companyid
                                                                );

            gvJobHazard.DataSource = ds;
            gvJobHazard.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNTJOBHAZARD"] = iRowCount;
            ViewState["TOTALPAGECOUNTJOBHAZARD"] = iTotalPageCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (General.GetNullableGuid(ViewState["JOBHAZARDID"].ToString()) == null)
                {
                    lblJobHazardTitle.Text = ds.Tables[0].Rows[0]["FLDSEARCHRESULT"].ToString();
                    ViewState["JOBHAZARDID"] = ds.Tables[0].Rows[0]["FLDJOBHAZARDID"].ToString();
                }
                //SetRowSelection();
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

        if (e.Item is GridDataItem)
        {
            UserControlToolTip ucJob = (UserControlToolTip)e.Item.FindControl("ucJob");
            RadLabel lblJob = (RadLabel)e.Item.FindControl("lblJob");
            if (lblJob != null)
            {
                //lblJob.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucJob.ToolTip + "', 'visible');");
                //lblJob.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucJob.ToolTip + "', 'hidden');");
                ucJob.Position = ToolTipPosition.TopCenter;
                ucJob.TargetControlId = lblJob.ClientID;
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
