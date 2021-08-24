using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;


public partial class Inspection_InspectionTMSAMatrixCategoryRevisions : PhoenixBasePage
{
    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetRowSelection();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionTMSAMatrixCategoryRevisions.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvFormRevision')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            MenuRegistersCountry.AccessRights = this.ViewState;
            MenuRegistersCountry.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            //toolbar.AddButton("Category", "CATEGORY");
            toolbar.AddButton("Revision", "REVISION", ToolBarDirection.Right);
            toolbar.AddButton("Matrix", "FORM", ToolBarDirection.Right);


            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["REVISIONID"] = "";

                if (Request.QueryString["CATEGORYID"] != null && Request.QueryString["CATEGORYID"].ToString() != "")
                    ViewState["CATEGORYID"] = Request.QueryString["CATEGORYID"].ToString();
                else
                    ViewState["CATEGORYID"] = "";

                gvFormRevision.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            GetFormDetails();

            MenuDocument.AccessRights = this.ViewState;
            MenuDocument.MenuList = toolbar.Show();
            MenuDocument.SelectedMenuIndex = 0;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    private void GetFormDetails()
    {
        if (ViewState["CATEGORYID"] != null && ViewState["CATEGORYID"].ToString() != "")
        {
            DataSet ds = PhoenixInspectionTMSAMatrix.TMSAMatrixCategoryEdit(
                                                            new Guid(ViewState["CATEGORYID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                MenuDocument.Title = dr["FLDCATEGORYNAME"].ToString() + " - " + "Revisions";
            }
        }
    }

    protected void RegistersCountry_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentDMSDocumentFilter = null;
                ViewState["DOCUMENTID"] = "";
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void MenuDocument_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FORM"))
            {
                Response.Redirect("../Inspection/InspectionTMSAMatrixCategoryContentList.aspx?CATEGORYID=" + ViewState["CATEGORYID"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void MenuField_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowFieldExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCREATEDDATE", "FLDAUTHORNAME", "FLDREVISIONDETAILS", "FLDPUBLISHEDDATE", "FLDAPPROVEDBYNAME" };
        string[] alCaptions = { "Added Date", "Added By", "Revision", "Approved Date", "Approved By" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixInspectionTMSAMatrix.TMSACategoryRevisionSearch(
                                                                 PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableGuid(ViewState["CATEGORYID"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , gvFormRevision.CurrentPageIndex + 1
                                                                , gvFormRevision.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        General.SetPrintOptions("gvFormRevision", MenuDocument.Title, alCaptions, alColumns, ds);

        gvFormRevision.DataSource = ds;
        gvFormRevision.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (General.GetNullableGuid(ViewState["REVISIONID"].ToString()) == null)
            {
                ViewState["REVISIONID"] = ds.Tables[0].Rows[0]["FLDREVISIONID"].ToString();
                ViewState["APPROVEDYN"] = ds.Tables[0].Rows[0]["FLDAPPROVEDYN"].ToString();
            }
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDCREATEDDATE", "FLDAUTHORNAME", "FLDREVISIONDETAILS", "FLDPUBLISHEDDATE", "FLDAPPROVEDBYNAME" };
        string[] alCaptions = { "Added Date", "Added By", "Revision", "Approved Date", "Approved By" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionTMSAMatrix.TMSACategoryRevisionSearch(
                                                              PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , General.GetNullableGuid(ViewState["CATEGORYID"].ToString())
                                                            , sortexpression
                                                            , sortdirection
                                                            , (int)ViewState["PAGENUMBER"]
                                                            , iRowCount
                                                            , ref iRowCount
                                                            , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=FormVersions.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>" + MenuDocument.Title + "</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");

            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void gvFormRevision_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvFormRevision_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

            int nCurrentRow = e.Item.ItemIndex;

            if (e.CommandName.ToUpper().Equals("ROWCLICK") || e.CommandName.ToUpper().Equals("SELECT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                Guid documentid = new Guid(item.GetDataKeyValue("FLDREVISIONID").ToString());
                gvFormRevision.SelectedIndexes.Add(e.Item.ItemIndex);
                BindPageURL(e.Item.ItemIndex);
            }

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                BindPageURL(nCurrentRow);
            }
            else if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                BindPageURL(nCurrentRow);

                PhoenixDocumentManagementForm.FormRevisionApprove(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["FORMID"].ToString())
                    , new Guid(ViewState["REVISIONID"].ToString()));

                ucStatus.Text = "Revision is approved.";
                BindData();
                gvFormRevision.Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixDocumentManagementForm.FormRevisionDelete(
                      PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(((RadLabel)_gridView.Items[nCurrentRow].FindControl("lblRevisionId")).Text));

                ViewState["REVISIONID"] = "";
                gvFormRevision.Rebind();
                gvFormField.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("NEWREVISION"))
            {
                BindPageURL(nCurrentRow);
                Guid? formrevisionid = Guid.Empty;

                PhoenixDocumentManagementForm.FormRevisionInsert(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["FORMID"].ToString())
                    , General.GetNullableGuid(ViewState["REVISIONID"].ToString())
                    , ref formrevisionid);

                ViewState["APPROVEDYN"] = "0";
                ViewState["REVISIONID"] = formrevisionid.ToString();
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvFormRevision_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        gvFormRevision.Rebind();
    }

    protected void gvFormRevision_ItemDataBound(Object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmTelerik(event); return false;");
            }

            RadLabel lblVersionNumber = (RadLabel)e.Item.FindControl("lblVersionNumber");
            LinkButton cmgEditContent = (LinkButton)e.Item.FindControl("cmgEditContent");
            LinkButton cmdApprove = (LinkButton)e.Item.FindControl("cmdApprove");
            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
            RadLabel lblRevisionId = (RadLabel)e.Item.FindControl("lblRevisonId");

            if (lblVersionNumber != null && lblVersionNumber.Text != null && General.GetNullableString(lblVersionNumber.Text) != null)
            {
                if (cmgEditContent != null)
                    cmgEditContent.Visible = false;
                if (cmdApprove != null)
                    cmdApprove.Visible = false;
            }

            if (cmdApprove != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdApprove.CommandName))
                    cmdApprove.Visible = false;
            }

            DataRowView dr = (DataRowView)e.Item.DataItem;
            HyperLink hlnkAddedDate = (HyperLink)e.Item.FindControl("hlnkAddedDate");

            if (hlnkAddedDate != null)
            {
                hlnkAddedDate.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/Inspection/InspectionTMSAMatrixCategoryRevisionContentList.aspx?categoryid=" + ViewState["CATEGORYID"].ToString() + "&revisionid=" + dr["FLDREVISIONID"].ToString() + "'); return false;");
            }

            DataRowView dv = (DataRowView)e.Item.DataItem;
            LinkButton cmdCreate = (LinkButton)e.Item.FindControl("cmdCreate");

            //if (cmdCreate != null && dv["FLDLATESTREVISIONYN"].ToString() == "1" && ViewState["FORMTYPE"] != null && General.GetNullableInteger(ViewState["FORMTYPE"].ToString()) == 0)
            //    cmdCreate.Visible = true;

            LinkButton cmdMoreInfo = (LinkButton)e.Item.FindControl("cmdMoreInfo");
            if (cmdMoreInfo != null)
            {
                cmdMoreInfo.Attributes.Add("onclick", "parent.openNewWindow('codeHelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionTMSAMatrixCommentsRevisionView.aspx?categoryid=" + ViewState["CATEGORYID"].ToString() + "&revisionid=" + dr["FLDREVISIONID"].ToString() + "');return false;");
            }

        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {

            RadLabel lblRevisonId = ((RadLabel)gvFormRevision.Items[rowindex].FindControl("lblRevisionId"));
            if (lblRevisonId != null)
                ViewState["REVISIONID"] = lblRevisonId.Text;

            RadLabel lblApprovedYN = ((RadLabel)gvFormRevision.Items[rowindex].FindControl("lblApprovedYN"));
            if (lblApprovedYN != null)
                ViewState["APPROVEDYN"] = lblApprovedYN.Text;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidDocument(string documentname, string categoryid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(documentname) == null)
            ucError.ErrorMessage = "Document name is required.";

        if (General.GetNullableGuid(categoryid) == null)
            ucError.ErrorMessage = "Document category is required.";

        return (!ucError.IsError);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    private void SetRowSelection()
    {

        foreach (GridDataItem item in gvFormRevision.Items)
        {

            if (item.GetDataKeyValue("FLDREVISIONID").ToString().Equals(ViewState["REVISIONID"].ToString()))
            {
                gvFormRevision.SelectedIndexes.Add(item.ItemIndex);
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
    }


    private void BindFieldsData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDSERIALNUMBER", "FLDLABEL", "FLDBOLDSTATUS", "FLDTYPE", "FLDSIZE", "FLDHEIGHT", "FLDPRECISION", "FLDSCALE", "FLDMANDATORYSTATUS" };
            string[] alCaptions = { "Sort Order", "Label", "Bold Y/N", "Type", "Width", "Height", "Precision", "Scale", "Mandatory Y/N" };

            string sortexpression = (ViewState["SORTEXPRESSIONSUB"] == null) ? null : (ViewState["SORTEXPRESSIONSUB"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTIONSUB"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONSUB"].ToString());

            DataSet ds = PhoenixDocumentManagementForm.FormFieldSearch(
                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , General.GetNullableGuid(ViewState["FORMID"].ToString())
                                                            , General.GetNullableGuid(ViewState["REVISIONID"].ToString())
                                                            , sortexpression
                                                            , sortdirection
                                                            , gvFormField.CurrentPageIndex + 1
                                                            , gvFormField.PageSize
                                                            , ref iRowCount
                                                            , ref iTotalPageCount);

            General.SetPrintOptions("gvFormField", "Form Fields", alCaptions, alColumns, ds);

            gvFormField.DataSource = ds;
            gvFormField.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNTSUB"] = iRowCount;
            ViewState["TOTALPAGECOUNTSUB"] = iTotalPageCount;
            if (General.GetNullableGuid(ViewState["FIELDID"].ToString()) == null)
            {
                ViewState["FIELDID"] = ds.Tables[0].Rows[0]["FLDFIELDID"].ToString();
            }
            if (ViewState["APPROVEDYN"] != null && ViewState["APPROVEDYN"].ToString() == "1" && gvFormField.Columns.Count > 8)
                gvFormField.Columns[9].Visible = false;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowFieldExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSERIALNUMBER", "FLDLABEL", "FLDBOLDSTATUS", "FLDTYPE", "FLDSIZE", "FLDHEIGHT", "FLDPRECISION", "FLDSCALE", "FLDMANDATORYSTATUS" };
        string[] alCaptions = { "Sort Order", "Label", "Bold Y/N", "Type", "Width", "Height", "Precision", "Scale", "Mandatory Y/N" };

        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSIONSUB"] == null) ? null : (ViewState["SORTEXPRESSIONSUB"].ToString());
        if (ViewState["SORTDIRECTIONSUB"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONSUB"].ToString());

        if (ViewState["SORTDIRECTIONSUB"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONSUB"].ToString());
        if (ViewState["ROWCOUNTSUB"] == null || Int32.Parse(ViewState["ROWCOUNTSUB"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNTSUB"].ToString());

        DataSet ds = PhoenixDocumentManagementForm.FormFieldSearch(
                                                   PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                   , General.GetNullableGuid(ViewState["FORMID"].ToString())
                                                   , General.GetNullableGuid(ViewState["REVISIONID"].ToString())
                                                   , sortexpression
                                                   , sortdirection
                                                   , (int)ViewState["PAGENUMBERSUB"]
                                                   , iRowCount
                                                   , ref iRowCount
                                                   , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=FieldList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Field List</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    private void GetCurrentField(int rowindex)
    {
        try
        {
            ViewState["FIELDID"] = ((RadLabel)gvFormField.Items[rowindex].FindControl("lblFieldId")).Text;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFormField_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

            if (e.CommandName.ToUpper().Equals("CHANGEPAGESIZE"))
                return;

            int nCurrentRow = e.Item.ItemIndex;

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                GetCurrentField(nCurrentRow);
            }
            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                GetCurrentField(nCurrentRow);
            }
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem eFooteritem = (GridFooterItem)e.Item;

                if (!IsValidField(
                    ((UserControlMaskNumber)eFooteritem.FindControl("ucSortOrderAdd")).Text,
                    ((DropDownList)eFooteritem.FindControl("ddlTypeAdd")).SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }

                if (General.GetNullableGuid(ViewState["REVISIONID"].ToString()) == null)
                {
                    Guid? formrevisionid = Guid.Empty;
                    PhoenixDocumentManagementForm.FormRevisionInsert(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(ViewState["FORMID"].ToString())
                        , General.GetNullableGuid(ViewState["REVISIONID"].ToString())
                        , ref formrevisionid);

                    ViewState["APPROVEDYN"] = "0";
                    ViewState["REVISIONID"] = formrevisionid.ToString();
                    BindData();
                }

                PhoenixDocumentManagementForm.FormFieldInsert(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["FORMID"].ToString())
                    , new Guid(ViewState["REVISIONID"].ToString())
                    , ((RadTextBox)eFooteritem.FindControl("txtLabelAdd")).Text
                    , ((DropDownList)eFooteritem.FindControl("ddlTypeAdd")).SelectedValue
                    , General.GetNullableString(((RadTextBox)eFooteritem.FindControl("txtLabelAdd")).Text)
                    , General.GetNullableString(((UserControlMaskNumber)eFooteritem.FindControl("ucMaxSizeAdd")).Text)
                    , (((CheckBox)eFooteritem.FindControl("chkMandatoryAdd")).Checked == true) ? 1 : 0
                    , null
                    , General.GetNullableInteger(((UserControlMaskNumber)eFooteritem.FindControl("ucColumnsAdd")).Text)
                    , General.GetNullableInteger(((UserControlMaskNumber)eFooteritem.FindControl("ucRowsAdd")).Text)
                    , General.GetNullableInteger(((UserControlMaskNumber)eFooteritem.FindControl("ucPrecisionAdd")).Text)
                    , General.GetNullableInteger(((UserControlMaskNumber)eFooteritem.FindControl("ucScaleAdd")).Text)
                    , (((CheckBox)eFooteritem.FindControl("chkBoldAdd")).Checked == true) ? 1 : 0
                    , Decimal.Parse(((UserControlMaskNumber)eFooteritem.FindControl("ucSortOrderAdd")).Text)
                    , null
                    , General.GetNullableInteger(((UserControlMaskNumber)eFooteritem.FindControl("ucHeightAdd")).Text)
                    );

                ViewState["FIELDID"] = "";
                BindFieldsData();

                //ucStatus.Text = "Field is added.";
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixDocumentManagementForm.FormFieldDelete(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(((Label)_gridView.Items[nCurrentRow].FindControl("lblFieldId")).Text));

                ViewState["FIELDID"] = "";
                BindFieldsData();
                //ucStatus.Text = "Field is deleted.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvFormField_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            db.Attributes.Add("onclick", "return fnConfirmTelerik(event)");
            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdEdit1");
            DropDownList ddlTypeEdit = (DropDownList)e.Item.FindControl("ddlTypeEdit");
            DataRowView dv = (DataRowView)e.Item.DataItem;

            if (ddlTypeEdit != null)
            {
                DataSet ds = PhoenixDocumentManagementForm.FormFieldTypeList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlTypeEdit.DataSource = ds;
                    ddlTypeEdit.DataTextField = "FLDFIELDTYPE";
                    ddlTypeEdit.DataValueField = "FLDFIELDTYPE";
                    ddlTypeEdit.DataBind();
                    ddlTypeEdit.Items.Insert(0, new ListItem("--Select--", "Dummy"));

                    if (dv != null)
                        ddlTypeEdit.SelectedValue = dv["FLDTYPE"].ToString();
                }
            }

            LinkButton imgChoice = (LinkButton)e.Item.FindControl("imgChoice");
            RadLabel lblFormId = (RadLabel)e.Item.FindControl("lblFormId");
            RadLabel lblFieldId = (RadLabel)e.Item.FindControl("lblFieldId");

            if (dv["FLDTYPE"].ToString() != "" && (dv["FLDTYPE"].ToString() == "DROPDOWN" || dv["FLDTYPE"].ToString() == "MULTIPLE CHOICE" || dv["FLDTYPE"].ToString() == "TABLE"))
            {
                if (imgChoice != null)
                {
                    imgChoice.Visible = true;
                    imgChoice.Attributes.Add("onclick", "javascript:parent.Openpopup('Filter','','../DocumentManagement/DocumentManagementFormFieldChoice.aspx?FORMID=" + lblFormId.Text + "&FIELDID=" + lblFieldId.Text + "&REVISIONID=" + ViewState["REVISIONID"].ToString() + "'); return false;");

                    if (!SessionUtil.CanAccess(this.ViewState, imgChoice.CommandName))
                        imgChoice.Visible = false;
                }
            }

        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            DropDownList ddlTypeAdd = (DropDownList)e.Item.FindControl("ddlTypeAdd");
            if (ddlTypeAdd != null)
            {
                DataSet ds = PhoenixDocumentManagementForm.FormFieldTypeList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlTypeAdd.DataSource = ds;
                    ddlTypeAdd.DataTextField = "FLDFIELDTYPE";
                    ddlTypeAdd.DataValueField = "FLDFIELDTYPE";
                    ddlTypeAdd.DataBind();
                    ddlTypeAdd.Items.Insert(0, new ListItem("--Select--", "Dummy"));
                }
            }
        }
    }

    protected void gvFormField_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = e.NewEditIndex;
            _gridView.SelectedIndex = e.NewEditIndex;

            BindFieldsData();

            DropDownList ddlTypeEdit = (DropDownList)_gridView.Rows[e.NewEditIndex].FindControl("ddlTypeEdit");
            ddlTypeEdit_Changed(ddlTypeEdit, new EventArgs());
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFormField_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (!IsValidField(
                    ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucSortOrderEdit")).Text,
                    ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlTypeEdit")).SelectedValue))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixDocumentManagementForm.FormFieldUpdate(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , new Guid(ViewState["FORMID"].ToString())
                , new Guid(ViewState["REVISIONID"].ToString())
                , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtLabelEdit")).Text
                , ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlTypeEdit")).SelectedValue
                , General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtLabelEdit")).Text)
                , General.GetNullableString(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucMaxSizeEdit")).Text)
                , (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkMandatoryEdit")).Checked == true) ? 1 : 0
                , null
                , General.GetNullableInteger(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucColumnsEdit")).Text)
                , General.GetNullableInteger(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucRowsEdit")).Text)
                , General.GetNullableInteger(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucPrecisionEdit")).Text)
                , General.GetNullableInteger(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucScaleEdit")).Text)
                , (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkBoldEdit")).Checked == true) ? 1 : 0
                , Decimal.Parse(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucSortOrderEdit")).Text)
                , new Guid(ViewState["FIELDID"].ToString())
                , General.GetNullableInteger(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucHeightEdit")).Text));


            //ucStatus.Text = "Field details updated.";
            _gridView.EditIndex = -1;
            BindFieldsData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFormField_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            BindFieldsData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void ddlTypeAdd_Changed(object sender, EventArgs e)
    {
        DropDownList ddlTypeAdd = (DropDownList)sender;

        GridViewRow gr = (GridViewRow)ddlTypeAdd.Parent.Parent;

        UserControlMaskNumber ucMaxSizeAdd = (UserControlMaskNumber)gr.FindControl("ucMaxSizeAdd");
        UserControlMaskNumber ucPrecisionAdd = (UserControlMaskNumber)gr.FindControl("ucPrecisionAdd");
        UserControlMaskNumber ucScaleAdd = (UserControlMaskNumber)gr.FindControl("ucScaleAdd");
        CheckBox chkMandatoryAdd = (CheckBox)gr.FindControl("chkMandatoryAdd");
        CheckBox chkBoldAdd = (CheckBox)gr.FindControl("chkBoldAdd");
        TextBox txtLabelAdd = (TextBox)gr.FindControl("txtLabelAdd");
        TextBox txtDefaultValueAdd = (TextBox)gr.FindControl("txtDefaultValueAdd");
        UserControlMaskNumber ucRowsAdd = (UserControlMaskNumber)gr.FindControl("ucRowsAdd");
        UserControlMaskNumber ucColumnsAdd = (UserControlMaskNumber)gr.FindControl("ucColumnsAdd");

        if (General.GetNullableString(ddlTypeAdd.SelectedValue) == "CAPTION" || General.GetNullableString(ddlTypeAdd.SelectedValue) == "HEADLINE NORMAL" || General.GetNullableString(ddlTypeAdd.SelectedValue) == "NOTE")
        {
            ucMaxSizeAdd.Visible = false;
            ucPrecisionAdd.Visible = false;
            ucScaleAdd.Visible = false;
            chkMandatoryAdd.Visible = false;
            txtDefaultValueAdd.Visible = false;
            ucRowsAdd.Visible = false;
            ucColumnsAdd.Visible = false;
        }
        else if (General.GetNullableString(ddlTypeAdd.SelectedValue) == "NUMBER")
        {
            ucScaleAdd.Visible = false;
            ucRowsAdd.Visible = false;
            ucColumnsAdd.Visible = false;
        }
        else if (General.GetNullableString(ddlTypeAdd.SelectedValue) == "PHONE" || General.GetNullableString(ddlTypeAdd.SelectedValue) == "TEXTBOX" || General.GetNullableString(ddlTypeAdd.SelectedValue) == "PARAGRAPH TEXT")
        {
            ucPrecisionAdd.Visible = false;
            ucScaleAdd.Visible = false;
            ucRowsAdd.Visible = false;
            ucColumnsAdd.Visible = false;
            if (General.GetNullableString(ddlTypeAdd.SelectedValue) == "PARAGRAPH TEXT")
                ucColumnsAdd.Visible = true;
        }
        else if (General.GetNullableString(ddlTypeAdd.SelectedValue) == "CHECKBOX" || General.GetNullableString(ddlTypeAdd.SelectedValue) == "DATE")
        {
            ucMaxSizeAdd.Visible = false;
            ucPrecisionAdd.Visible = false;
            ucScaleAdd.Visible = false;
            ucRowsAdd.Visible = false;
            ucColumnsAdd.Visible = false;
        }
        else if (General.GetNullableString(ddlTypeAdd.SelectedValue) == "SEPERATOR")
        {
            ucMaxSizeAdd.Visible = false;
            ucPrecisionAdd.Visible = false;
            ucScaleAdd.Visible = false;
            chkMandatoryAdd.Visible = false;
            txtLabelAdd.Visible = false;
            chkBoldAdd.Visible = false;
            txtDefaultValueAdd.Visible = false;
            ucRowsAdd.Visible = false;
            ucColumnsAdd.Visible = false;
        }
        else if (General.GetNullableString(ddlTypeAdd.SelectedValue) == "PRICE")
        {
            ucRowsAdd.Visible = false;
            ucColumnsAdd.Visible = false;
        }
        else if (General.GetNullableString(ddlTypeAdd.SelectedValue) == "DROPDOWN" || General.GetNullableString(ddlTypeAdd.SelectedValue) == "MULTIPLE CHOICE")
        {
            ucPrecisionAdd.Visible = false;
            ucScaleAdd.Visible = false;
            chkBoldAdd.Visible = false;

            ucRowsAdd.Visible = false;
            ucColumnsAdd.Visible = false;
        }
    }

    protected void ddlTypeEdit_Changed(object sender, EventArgs e)
    {
        DropDownList ddlTypeEdit = (DropDownList)sender;
        GridViewRow gr = (GridViewRow)ddlTypeEdit.Parent.Parent;

        UserControlMaskNumber ucMaxSizeAdd = (UserControlMaskNumber)gr.FindControl("ucMaxSizeEdit");
        UserControlMaskNumber ucPrecisionAdd = (UserControlMaskNumber)gr.FindControl("ucPrecisionEdit");
        UserControlMaskNumber ucScaleAdd = (UserControlMaskNumber)gr.FindControl("ucScaleEdit");
        CheckBox chkMandatoryAdd = (CheckBox)gr.FindControl("chkMandatoryEdit");
        CheckBox chkBoldAdd = (CheckBox)gr.FindControl("chkBoldEdit");
        TextBox txtLabelAdd = (TextBox)gr.FindControl("txtLabelEdit");
        TextBox txtDefaultValueAdd = (TextBox)gr.FindControl("txtDefaultValueEdit");

        UserControlMaskNumber ucRowsAdd = (UserControlMaskNumber)gr.FindControl("ucRowsEdit");
        UserControlMaskNumber ucColumnsAdd = (UserControlMaskNumber)gr.FindControl("ucColumnsEdit");

        if (General.GetNullableString(ddlTypeEdit.SelectedValue) == "CAPTION" || General.GetNullableString(ddlTypeEdit.SelectedValue) == "HEADLINE NORMAL" || General.GetNullableString(ddlTypeEdit.SelectedValue) == "NOTE")
        {
            ucMaxSizeAdd.Visible = false;
            ucPrecisionAdd.Visible = false;
            ucScaleAdd.Visible = false;
            chkMandatoryAdd.Visible = false;
            txtDefaultValueAdd.Visible = false;
            ucRowsAdd.Visible = false;
            ucColumnsAdd.Visible = false;
        }
        else if (General.GetNullableString(ddlTypeEdit.SelectedValue) == "NUMBER")
        {
            ucScaleAdd.Visible = false;
            ucRowsAdd.Visible = false;
            ucColumnsAdd.Visible = false;
        }
        else if (General.GetNullableString(ddlTypeEdit.SelectedValue) == "PHONE" || General.GetNullableString(ddlTypeEdit.SelectedValue) == "TEXTBOX" || General.GetNullableString(ddlTypeEdit.SelectedValue) == "PARAGRAPH TEXT")
        {
            ucPrecisionAdd.Visible = false;
            ucScaleAdd.Visible = false;
            ucRowsAdd.Visible = false;
            ucColumnsAdd.Visible = false;
            if (General.GetNullableString(ddlTypeEdit.SelectedValue) == "PARAGRAPH TEXT")
                ucColumnsAdd.Visible = true;
        }
        else if (General.GetNullableString(ddlTypeEdit.SelectedValue) == "CHECKBOX" || General.GetNullableString(ddlTypeEdit.SelectedValue) == "DATE")
        {
            ucMaxSizeAdd.Visible = false;
            ucPrecisionAdd.Visible = false;
            ucScaleAdd.Visible = false;
            ucRowsAdd.Visible = false;
            ucColumnsAdd.Visible = false;
        }
        else if (General.GetNullableString(ddlTypeEdit.SelectedValue) == "SEPERATOR")
        {
            ucMaxSizeAdd.Visible = false;
            ucPrecisionAdd.Visible = false;
            ucScaleAdd.Visible = false;
            chkMandatoryAdd.Visible = false;
            txtLabelAdd.Visible = false;
            chkBoldAdd.Visible = false;
            txtDefaultValueAdd.Visible = false;
            ucRowsAdd.Visible = false;
            ucColumnsAdd.Visible = false;
        }
        else if (General.GetNullableString(ddlTypeEdit.SelectedValue) == "PRICE")
        {

        }
        else if (General.GetNullableString(ddlTypeEdit.SelectedValue) == "DROPDOWN" || General.GetNullableString(ddlTypeEdit.SelectedValue) == "MULTIPLE CHOICE")
        {
            ucPrecisionAdd.Visible = false;
            ucScaleAdd.Visible = false;
            chkBoldAdd.Visible = false;
            ucRowsAdd.Visible = false;
            ucColumnsAdd.Visible = false;
        }
    }

    private bool IsValidField(string sortorder, string type)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDecimal(sortorder) == null)
            ucError.ErrorMessage = "Sort Order is required.";

        if (General.GetNullableString(type) == null)
            ucError.ErrorMessage = "Type is required.";

        return (!ucError.IsError);
    }


    private void SetFieldRowSelection()
    {

        foreach (GridDataItem item in gvFormField.Items)
        {

            if (item.GetDataKeyValue("FLDFIELDID").ToString().Equals(ViewState["FIELDID"].ToString()))
            {
                gvFormField.SelectedIndexes.Add(item.ItemIndex);
            }
        }
    }
}