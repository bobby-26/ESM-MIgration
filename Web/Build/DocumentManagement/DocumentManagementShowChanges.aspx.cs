using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class DocumentManagementShowChanges : PhoenixBasePage
{

    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvSectionChanges.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvSectionChanges.UniqueID, "Select$" + r.RowIndex.ToString());
    //        }
    //    }
    //    foreach (GridViewRow r in gvFormChanges.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvFormChanges.UniqueID, "Select$" + r.RowIndex.ToString());
    //        }
    //    }
    //    foreach (GridViewRow r in gvJobHazard.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvJobHazard.UniqueID, "Select$" + r.RowIndex.ToString());
    //        }
    //    }
    //    foreach (GridViewRow r in gvProcessRA.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvProcessRA.UniqueID, "Select$" + r.RowIndex.ToString());
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
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementShowChanges.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementShowChanges.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuDocument.AccessRights = this.ViewState;
            MenuDocument.MenuList = toolbar.Show();


            if (!IsPostBack)
            {
                DateTime now = DateTime.Now;

                ucFromDate.Text = now.Date.AddMonths(-6).ToShortDateString();
                ucToDate.Text = DateTime.Today.ToString();

                ViewState["PAGENUMBERSECTION"] = 1;
                ViewState["SORTEXPRESSIONSECTION"] = null;
                ViewState["SORTDIRECTIONSECTION"] = null;
                ViewState["CURRENTINDEXSECTION"] = 1;
                ViewState["SECTIONREVISIONID"] = "";

                ViewState["PAGENUMBERFORM"] = 1;
                ViewState["SORTEXPRESSIONFORM"] = null;
                ViewState["SORTDIRECTIONFORM"] = null;
                ViewState["CURRENTINDEXFORM"] = 1;
                ViewState["FORMREVISIONID"] = "";

                ViewState["PAGENUMBERRISKASSESSMENT"] = 1;
                ViewState["SORTEXPRESSIONRISKASSESSMENT"] = null;
                ViewState["SORTDIRECTIONRISKASSESSMENT"] = null;
                ViewState["CURRENTINDEXRISKASSESSMENT"] = 1;
                ViewState["PROCESSIDRAID"] = "";

                ViewState["PAGENUMBERJOBHAZARD"] = 1;
                ViewState["SORTEXPRESSIONJOBHAZARD"] = null;
                ViewState["SORTDIRECTIONJOBHAZARD"] = null;
                ViewState["CURRENTINDEXJOBHAZARD"] = 1;
                ViewState["JOBHAZARDID"] = "";
                ViewState["VESSELID"] = "0";

                //VesselConfiguration();
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.InstallCode.ToString();
                //SetVessel();
                //BindVesselList();
                ddlVessel.Company = PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString();
                ddlVessel.bind();
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ddlVessel.Enabled = false;
                }
                else
                    ddlVessel.SelectedVessel = ViewState["VESSELID"].ToString();

                gvSectionChanges.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvFormChanges.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvJobHazard.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvProcessRA.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }

            //BindSectionChanges();

            //BindFormChanges();

            //BindJobHazardChanges();

            //BindProcessRAChanges();

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    // private void BindVesselList()
    //{
    //    DataSet ds = PhoenixRegistersVessel.ListAllVessel();        
    //    ddlVessel.DataSource = ds;
    //    ddlVessel.DataTextField = "FLDVESSELNAME";
    //    ddlVessel.DataValueField = "FLDVESSELID";
    //    ddlVessel.DataBind();
    //    if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
    //    {
    //        ddlVessel.SelectedValue = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
    //        ddlVessel.Enabled = false;            
    //    }
    // }


    protected void MenuDocument_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucFromDate.Text = "";
                ucToDate.Text = "";
                DateTime now = DateTime.Now;
                ucFromDate.Text = now.Date.AddMonths(-6).ToShortDateString();
                
                lblSectionTitle.Text = "0 changes in documents / sections";
                lblFormTitle.Text = "0 changes in forms / posters / books";                
                lblJobHazardTitle.Text = "0 changes in JHA's";
                lblRiskAssessmentTitle.Text = "0 changes in RA's";
                BindSectionChanges();
                BindFormChanges();
                BindJobHazardChanges();
                BindProcessRAChanges();
                gvSectionChanges.Rebind();
                gvFormChanges.Rebind();
                gvJobHazard.Rebind();
                gvProcessRA.Rebind();

            }
            else if (CommandName.ToUpper().Equals("FIND"))
            {
                if (!IsValidInput())
                {
                    ucError.Visible = true;
                    return;
                }

                lblSectionTitle.Text = "0 changes in documents / sections";
                lblFormTitle.Text = "0 changes in forms / posters / books";
                lblJobHazardTitle.Text = "0 changes in JHA's";
                lblRiskAssessmentTitle.Text = "0 changes in RA's";

                BindSectionChanges();
                BindFormChanges();
                BindJobHazardChanges();
                BindProcessRAChanges();
                gvSectionChanges.Rebind();
                gvFormChanges.Rebind();
                gvJobHazard.Rebind();
                gvProcessRA.Rebind();

            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    private bool IsValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if(General.GetNullableDateTime(ucFromDate.Text) == null)
            ucError.ErrorMessage = "From Date is required.";

        if (General.GetNullableDateTime(ucToDate.Text) != null)
        {
            if (General.GetNullableDateTime(ucToDate.Text) < General.GetNullableDateTime(ucFromDate.Text))
                ucError.ErrorMessage = "To Date should be later than From Date.";
        }
        else
        {
            if (General.GetNullableDateTime(ucFromDate.Text) != null)
                ucToDate.Text = DateTime.Today.ToString();
        }

        return (!ucError.IsError);
    }


    protected void gvSectionChanges_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindSectionChanges();

        BindFormChanges();

        BindJobHazardChanges();

        BindProcessRAChanges();
    }

    protected void gvFormChanges_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindSectionChanges();

        BindFormChanges();

        BindJobHazardChanges();

        BindProcessRAChanges();
    }

    protected void gvJobHazard_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindSectionChanges();

        BindFormChanges();

        BindJobHazardChanges();

        BindProcessRAChanges();
    }

    protected void gvProcessRA_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindSectionChanges();

        BindFormChanges();

        BindJobHazardChanges();

        BindProcessRAChanges();
    }

    // ----- SECTION ----- //

    private void BindSectionChanges()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSIONSECTION"] == null) ? null : (ViewState["SORTEXPRESSIONSECTION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTIONSECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONSECTION"].ToString());

        //SetVessel();

        DataSet ds = new DataSet();

        int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        ds = PhoenixDocumentManagementRevision.DocumentRevisonByDateSearch(
                                                                  PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , null
                                                                , General.GetNullableDateTime(ucFromDate.Text)
                                                                , General.GetNullableDateTime(ucToDate.Text)
                                                                , General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                                , sortexpression
                                                                , sortdirection
                                                                , gvSectionChanges.CurrentPageIndex + 1
                                                                , gvSectionChanges.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , companyid
                                                                );


        gvSectionChanges.DataSource = ds;
        gvSectionChanges.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNTSECTION"] = iRowCount;
        ViewState["TOTALPAGECOUNTSECTION"] = iTotalPageCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (General.GetNullableGuid(ViewState["SECTIONREVISIONID"].ToString()) == null)
            {                
                ViewState["SECTIONREVISIONID"] = ds.Tables[0].Rows[0]["FLDSECTIONREVISIONID"].ToString();
                //gvSectionChanges.SelectedIndex = 0;
            }
            lblSectionTitle.Text = ds.Tables[0].Rows[0]["FLDSEARCHRESULT"].ToString();
        }

    }

    protected void gvSectionChanges_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("CHANGEPAGESIZE"))
                return;

            int nCurrentRow = e.Item.ItemIndex;

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                Label lblSectionRevisionId = ((Label)gvSectionChanges.Items[nCurrentRow].FindControl("lblSectionRevisionId"));
                if (lblSectionRevisionId != null)
                    ViewState["SECTIONREVISIONID"] = lblSectionRevisionId.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvSectionChanges_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {

            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)item.DataItem;

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
            RadLabel lblSectionName = (RadLabel)e.Item.FindControl("lblSectionName");
            if (lblSectionName != null)
            {
                //lblSectionName.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucSectionName.ToolTip + "', 'visible');");
                //lblSectionName.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucSectionName.ToolTip + "', 'hidden');");
                ucSectionName.Position = ToolTipPosition.TopCenter;
                ucSectionName.TargetControlId = lblSectionName.ClientID;
            }

            DataRowView dr = (DataRowView)e.Item.DataItem;

            LinkButton lnkSectionRevision = (LinkButton)e.Item.FindControl("lnkSectionRevision");
            if (lnkSectionRevision != null)
                lnkSectionRevision.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filter','','DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + dr["FLDDOCUMENTID"].ToString() + "&SECTIONID=" + dr["FLDSECTIONID"].ToString() + "&REVISONID=" + dr["FLDSECTIONREVISIONID"].ToString() + "'); return false;");

            GridDecorator.SectionMergeRows(gvSectionChanges, e);
        }
    }



    // ----- FORM ----- //

    private void BindFormChanges()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSIONFORM"] == null) ? null : (ViewState["SORTEXPRESSIONFORM"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTIONFORM"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONFORM"].ToString());

            //SetVessel();

            DataSet ds = new DataSet();

            int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

            ds = PhoenixDocumentManagementRevision.FormRevisonByDateSearch(
                                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                  , null
                                                                  , General.GetNullableDateTime(ucFromDate.Text)
                                                                  , General.GetNullableDateTime(ucToDate.Text)
                                                                  , int.Parse(ddlVessel.SelectedVessel)
                                                                  , sortexpression
                                                                  , sortdirection
                                                                  , gvFormChanges.CurrentPageIndex + 1
                                                                  , gvFormChanges.PageSize
                                                                  , ref iRowCount
                                                                  , ref iTotalPageCount
                                                                  , companyid 
                                                                  );

            gvFormChanges.DataSource = ds;
            gvFormChanges.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNTFORM"] = iRowCount;
            ViewState["TOTALPAGECOUNTFORM"] = iTotalPageCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (General.GetNullableGuid(ViewState["FORMREVISIONID"].ToString()) == null)
                {                    
                    ViewState["FORMREVISIONID"] = ds.Tables[0].Rows[0]["FLDFORMREVISIONID"].ToString();
                    //gvFormChanges.SelectedIndex = 0;
                }
                lblFormTitle.Text = ds.Tables[0].Rows[0]["FLDSEARCHRESULT"].ToString();
                //SetRowSelection();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFormChanges_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("CHANGEPAGESIZE"))
                return;

            int nCurrentRow = e.Item.ItemIndex;

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {

                RadLabel lblRevisionId = ((RadLabel)gvFormChanges.Items[nCurrentRow].FindControl("lblRevisionId"));
                if (lblRevisionId != null)
                    ViewState["FORMREVISIONID"] = lblRevisionId.Text;
            }

        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Please make the required correction";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvFormChanges_OnItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView dr = (DataRowView)item.DataItem;
            HyperLink hlnkRevison = (HyperLink)e.Item.FindControl("hlnkRevison");

            if (dr["FLDTYPE"] != null && dr["FLDTYPE"].ToString() == "1" && General.GetNullableGuid(dr["FLDDTKEY"].ToString()) != null)
            {
                DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(dr["FLDDTKEY"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    DataRow drRow = dt.Rows[0];
                    if (hlnkRevison != null)
                        hlnkRevison.NavigateUrl = "../Common/download.aspx?dtkey=" + drRow["FLDDTKEY"].ToString();

                }
            }

            if (dr["FLDTYPE"] != null && dr["FLDTYPE"].ToString() == "0")
            {
                hlnkRevison.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FormId=" + dr["FLDFORMID"].ToString() + "&FORMTYPE=DMSForm&FormName=" + dr["FLDFORMDESIGNNAME"].ToString() + "&FORMREVISIONID=" + dr["FLDFORMREVISIONID"].ToString() + "'); return false;");
            }

            UserControlToolTip ucFormName = (UserControlToolTip)e.Item.FindControl("ucFormName");
            RadLabel lblFormName = (RadLabel)e.Item.FindControl("lblFormName");
            if (lblFormName != null)
            {
                //lblFormName.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucFormName.ToolTip + "', 'visible');");
                //lblFormName.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucFormName.ToolTip + "', 'hidden');");
                ucFormName.Position = ToolTipPosition.TopCenter;
                ucFormName.TargetControlId = lblFormName.ClientID;
            }

            GridDecorator.FormMergeRows(gvFormChanges, e);
        }
    }




    // ----- JHA ----- //

    private void BindJobHazardChanges()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSIONJOBHAZARD"] == null) ? null : (ViewState["SORTEXPRESSIONJOBHAZARD"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTIONJOBHAZARD"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONJOBHAZARD"].ToString());

            //SetVessel();

            int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

            DataSet ds = new DataSet();

            ds = PhoenixDocumentManagementRevision.JHARevisonByDateSearch(
                                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                  , General.GetNullableDateTime(ucFromDate.Text)
                                                                  , General.GetNullableDateTime(ucToDate.Text)
                                                                  , int.Parse(ddlVessel.SelectedVessel)
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
                    ViewState["JOBHAZARDID"] = ds.Tables[0].Rows[0]["FLDJOBHAZARDID"].ToString();
                    //gvJobHazard.SelectedIndex = 0;
                }
                lblJobHazardTitle.Text = ds.Tables[0].Rows[0]["FLDSEARCHRESULT"].ToString();
                //SetRowSelection();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvJobHazard_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;


            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("CHANGEPAGESIZE"))
                return;

            int nCurrentRow = e.Item.ItemIndex;

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {

                Label lblJobHazardId = ((Label)gvJobHazard.Items[nCurrentRow].FindControl("lblJobHazardId"));
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

    protected void gvJobHazard_OnItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {

            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)item.DataItem;

            RadLabel lblJobHazardId = (RadLabel)e.Item.FindControl("lblJobHazardId");
            LinkButton lnkRevision = (LinkButton)e.Item.FindControl("lnkRevision");
            if (lnkRevision != null && lblJobHazardId != null)
            {
                lnkRevision.Visible = SessionUtil.CanAccess(this.ViewState, lnkRevision.CommandName);
                lnkRevision.Attributes.Add("onclick", "openNewWindow('JobHazard', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=9&reportcode=JOBHAZARD&jobhazardid=" + lblJobHazardId.Text + "&showmenu=0&showword=NO&showexcel=NO');return false;");
            }

            UserControlToolTip ucJob = (UserControlToolTip)e.Item.FindControl("ucJob");
            RadLabel lblJob = (RadLabel)e.Item.FindControl("lblJob");
            if (lblJob != null)
            {
                //lblJob.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucJob.ToolTip + "', 'visible');");
                //lblJob.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucJob.ToolTip + "', 'hidden');");
                ucJob.Position = ToolTipPosition.TopCenter;
                ucJob.TargetControlId = lblJob.ClientID;

            }

            GridDecorator.RAMergeRows(gvJobHazard, e);
        }
    }



    // ----- RA ----- //

    private void BindProcessRAChanges()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSIONRISKASSESSMENT"] == null) ? null : (ViewState["SORTEXPRESSIONRISKASSESSMENT"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTIONRISKASSESSMENT"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONRISKASSESSMENT"].ToString());
            
            //SetVessel();

            DataSet ds = new DataSet();

            int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

            ds = PhoenixDocumentManagementRevision.ProcessRARevisonByDateSearch(
                                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                  , General.GetNullableDateTime(ucFromDate.Text)
                                                                  , General.GetNullableDateTime(ucToDate.Text)
                                                                  , int.Parse(ddlVessel.SelectedVessel)
                                                                  , sortexpression
                                                                  , sortdirection
                                                                  , gvProcessRA.CurrentPageIndex + 1
                                                                  , gvProcessRA.PageSize
                                                                  , ref iRowCount
                                                                  , ref iTotalPageCount
                                                                  , companyid
                                                                  );


            gvProcessRA.DataSource = ds;
            gvProcessRA.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNTRISKASSESSMENT"] = iRowCount;
            ViewState["TOTALPAGECOUNTRISKASSESSMENT"] = iTotalPageCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (General.GetNullableGuid(ViewState["PROCESSIDRAID"].ToString()) == null)
                {                    
                    ViewState["PROCESSIDRAID"] = ds.Tables[0].Rows[0]["FLDRISKASSESSMENTPROCESSID"].ToString();
                    //gvProcessRA.SelectedIndex = 0;
                }
                lblRiskAssessmentTitle.Text = ds.Tables[0].Rows[0]["FLDSEARCHRESULT"].ToString();
                //SetRowSelection();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvProcessRA_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("CHANGEPAGESIZE"))
                return;

            int nCurrentRow = e.Item.ItemIndex;

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {

                RadLabel lblProcessId = ((RadLabel)gvProcessRA.Items[nCurrentRow].FindControl("lblProcessId"));
                if (lblProcessId != null)
                    ViewState["PROCESSIDRAID"] = lblProcessId.Text;
            }

        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Please make the required correction";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvProcessRA_OnItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            UserControlToolTip ucProcessName = (UserControlToolTip)e.Item.FindControl("ucProcessName");
            RadLabel lblProcessName = (RadLabel)e.Item.FindControl("lblProcessName");
            if (lblProcessName != null)
            {
                //lblProcessName.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucProcessName.ToolTip + "', 'visible');");
                //lblProcessName.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucProcessName.ToolTip + "', 'hidden');");
                ucProcessName.Position = ToolTipPosition.TopCenter;
                ucProcessName.TargetControlId = lblProcessName.ClientID;
            }

            LinkButton lnkRevision = (LinkButton)e.Item.FindControl("lnkRevision");
            RadLabel lblProcessId = (RadLabel)e.Item.FindControl("lblProcessId");
            DataRowView dv = (DataRowView)e.Item.DataItem;
            string ratype = dv["FLDRATYPE"].ToString();

            if (lnkRevision != null && lblProcessId != null)
            {
                if (ratype == "8")
                {
                    //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    //{
                    //    lnkRevision.Attributes.Add("onclick", "Openpopup('RAProcess', '', '../Reports/ReportsView.aspx?applicationcode=9&reportcode=RAPROCESS&processid=" + lblProcessId.Text + "&showmenu=0&showword=NO&showexcel=NO&showpdf=NO&showprint=YES');return false;");
                    //}
                    //else
                    {
                        lnkRevision.Attributes.Add("onclick", "openNewWindow('RAProcess', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=9&reportcode=RAPROCESS&processid=" + lblProcessId.Text + "&showmenu=0&showword=NO&showexcel=NO');return false;");
                    }
                }
                else if (ratype == "11")
                {
                    //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    //{
                    //    lnkRevision.Attributes.Add("onclick", "Openpopup('RAGeneric', '', '../Reports/ReportsView.aspx?applicationcode=9&reportcode=RAGENERIC&genericid=" + lblProcessId.Text + "&showmenu=0&showword=NO&showexcel=NO&showpdf=NO&showprint=YES');return false;");
                    //}
                    //else
                    {
                        lnkRevision.Attributes.Add("onclick", "openNewWindow('RAGeneric', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAGENERIC&genericid=" + lblProcessId.Text + "&showmenu=0&showexcel=NO');return false;");
                    }
                }
                else if (ratype == "12")
                {
                    //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    //{
                    //    lnkRevision.Attributes.Add("onclick", "Openpopup('RAMachinery', '', '../Reports/ReportsView.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + lblProcessId.Text + "&showmenu=0&showword=NO&showexcel=NO&showpdf=NO&showprint=YES');return false;");
                    //}
                    //else
                    {
                        lnkRevision.Attributes.Add("onclick", "openNewWindow('RAMachinery', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + lblProcessId.Text + "&showmenu=0&showexcel=NO');return false;");
                    }
                }
                else if (ratype == "13")
                {
                    //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    //{
                    //    lnkRevision.Attributes.Add("onclick", "Openpopup('RANavigation', '', '../Reports/ReportsView.aspx?applicationcode=9&reportcode=RANAVIGATION&navigationid=" + lblProcessId.Text + "&showmenu=0&showword=NO&showexcel=NO&showpdf=NO&showprint=YES');return false;");
                    //}
                    //else
                    {
                        lnkRevision.Attributes.Add("onclick", "openNewWindow('RANavigation', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RANAVIGATION&navigationid=" + lblProcessId.Text + "&showmenu=0&showexcel=NO');return false;");
                    }
                }
                lnkRevision.Visible = SessionUtil.CanAccess(this.ViewState, lnkRevision.CommandName);
            }

            GridDecorator.RAMergeRows(gvProcessRA, e);
        }
    }



    public class GridDecorator
    {
        public static void SectionMergeRows(RadGrid gridView, GridItemEventArgs e)
        {
            for (int rowIndex = gridView.Items.Count - 2; rowIndex >= 0; rowIndex--)
            {
                //int index = e.Item.ItemIndex;
                GridDataItem row = gridView.Items[rowIndex];
                GridDataItem previousRow = gridView.Items[rowIndex + 1];

                string currentCategoryName = ((RadLabel)gridView.Items[rowIndex].FindControl("lblcategoryName")).Text;
                string previousCategoryName = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblcategoryName")).Text;

                string currentDocumentName = ((RadLabel)gridView.Items[rowIndex].FindControl("lblDocumentName")).Text;
                string previousDocumentName = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblDocumentName")).Text;

                string currentDocumentRevision = ((RadLabel)gridView.Items[rowIndex].FindControl("lblDocumentRevision")).Text;
                string previousDocumentRevision = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblDocumentRevision")).Text;

                if (currentCategoryName == previousCategoryName && currentDocumentName != previousDocumentName && currentDocumentRevision != previousDocumentRevision)
                {
                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                        previousRow.Cells[1].RowSpan + 1;
                    previousRow.Cells[1].Visible = false;
                }
                else if (currentCategoryName == previousCategoryName && currentDocumentName == previousDocumentName && currentDocumentRevision != previousDocumentRevision)
                {
                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                                               previousRow.Cells[1].RowSpan + 1;
                    previousRow.Cells[1].Visible = false;

                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                         previousRow.Cells[2].RowSpan + 1;
                    previousRow.Cells[2].Visible = false;
                }
                else if (currentCategoryName == previousCategoryName && currentDocumentName == previousDocumentName && currentDocumentRevision == previousDocumentRevision)
                {
                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                           previousRow.Cells[1].RowSpan + 1;
                    previousRow.Cells[1].Visible = false;

                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                         previousRow.Cells[2].RowSpan + 1;
                    previousRow.Cells[2].Visible = false;

                    row.Cells[3].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
                                     previousRow.Cells[3].RowSpan + 1;
                    previousRow.Cells[3].Visible = false;
                }
            }
        }

        public static void FormMergeRows(RadGrid gridView, GridItemEventArgs e)
        {
            for (int rowIndex = gridView.Items.Count - 2; rowIndex >= 0; rowIndex--)
            {
                int index = e.Item.ItemIndex;
                GridDataItem row = gridView.Items[rowIndex];
                GridDataItem previousRow = gridView.Items[rowIndex + 1];

                string currentCategoryName = ((RadLabel)gridView.Items[rowIndex].FindControl("lblCategoryName")).Text;
                string previousCategoryName = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblCategoryName")).Text;

                string currentSubCategoryName = ((RadLabel)gridView.Items[rowIndex].FindControl("lblSubCategoryName")).Text;
                string previousSubCategoryName = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblSubCategoryName")).Text;

                if (currentCategoryName == previousCategoryName && currentSubCategoryName != previousSubCategoryName)
                {
                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                           previousRow.Cells[1].RowSpan + 1;
                    previousRow.Cells[1].Visible = false;
                }
                else if (currentCategoryName == previousCategoryName && currentSubCategoryName == previousSubCategoryName)
                {
                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                           previousRow.Cells[1].RowSpan + 1;
                    previousRow.Cells[1].Visible = false;

                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                         previousRow.Cells[2].RowSpan + 1;
                    previousRow.Cells[2].Visible = false;
                }
            }
        }

        public static void RAMergeRows(RadGrid gridView, GridItemEventArgs e)
        {
            for (int rowIndex = gridView.Items.Count - 2; rowIndex >= 0; rowIndex--)
            {
                int index = e.Item.ItemIndex;
                GridDataItem row = gridView.Items[rowIndex];
                GridDataItem previousRow = gridView.Items[rowIndex + 1];

                string currentCategoryName = ((RadLabel)gridView.Items[rowIndex].FindControl("lblCategoryName")).Text;
                string previousCategoryName = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblCategoryName")).Text;

                string currentTypeName = ((RadLabel)gridView.Items[rowIndex].FindControl("lblTypeName")).Text;                
                string previousTypeName = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblTypeName")).Text;

                if (currentTypeName == previousTypeName && currentCategoryName != previousCategoryName)
                {
                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                        previousRow.Cells[1].RowSpan + 1;
                    previousRow.Cells[1].Visible = false;
                }
                else if (currentTypeName == previousTypeName && currentCategoryName == previousCategoryName)
                {
                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                           previousRow.Cells[1].RowSpan + 1;
                    previousRow.Cells[1].Visible = false;

                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                         previousRow.Cells[2].RowSpan + 1;
                    previousRow.Cells[2].Visible = false;
                }
            }
        }
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

    protected void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixDocumentManagementDocument.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    //protected void SetVessel()
    //{
    //    if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
    //        ViewState["VESSELID"] = int.Parse(Filter.CurrentVesselConfiguration.ToString());

    //    if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
    //        ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
    //}
       
}
