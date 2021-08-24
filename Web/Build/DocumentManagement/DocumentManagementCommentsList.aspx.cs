using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.DocumentManagement;
using System.Text;
using Telerik.Web.UI;

public partial class DocumentManagementCommentsList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementCommentsList.aspx", "Add New", "<i class=\"fa fa-plus\"></i>", "ADD");
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementCommentsList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvComments')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementCommentsList.aspx", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementCommentsList.aspx", "Find Section", "<i class=\"fa fa-search-minus\"></i>", "SECTIONSEARCH");
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementCommentsList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "RESET");
            //toolbar.AddImageButton("../DocumentManagement/DocumentManagementOfficeRemarks.aspx?COMMENTSIDLIST=" + Filter.CurrentSelectedComments, "Bulk Review", "checklist.png", "BULKREVIEW");
            toolbar.AddFontAwesomeButton("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementBulkReviewOfficeRemarks.aspx?')", "Bulk Office Comments", "<i class=\"fa fa-comments\"></i>", "BULKOFFICECOMMENTS");
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementCommentsList.aspx", "Bulk Review", "<i class=\"fa fa-check-square\"></i>", "BULKREVIEW");
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementCommentsList.aspx", "Bulk Archive", "<i class=\"fa fa-download\"></i>", "BULKARCHIVE");

            MenuComments.AccessRights = this.ViewState;
            MenuComments.MenuList = toolbar.Show();


            if (!IsPostBack)
            {

                if (Filter.CurrentSelectedComments != null)
                    Filter.CurrentSelectedComments = null;

                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                confirmarchive.Attributes.Add("style", "display:none;");
                confirmReview.Attributes.Add("style", "display:none;");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["DMSCOMMENTSID"] = "";
                ViewState["SELECTEDCOMMENTS"] = "";

                int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
                btnShowDocuments.Attributes.Add("onclick", "return showPickList('spnPickListDocument', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListDocumentManagementTreeForAllNodes.aspx?iframignore=true&companyid=" + companyid + "', true); ");

                gvComments.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //if (Filter.CurrentSelectedComments != null && Filter.CurrentSelectedComments.Count > 0 && ViewState["SELECTEDCOMMENTS"].ToString() == "Search")
            //{
            //    BindSelectedSection();
            //}

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuComments_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("ADD"))
        {
            Response.Redirect("../DocumentManagement/DocumentManagementDocument.aspx?viewonly=1", false);
        }

        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            gvComments.CurrentPageIndex = 0;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvComments.Rebind();
        }

        if (CommandName.ToUpper().Equals("SECTIONSEARCH"))
        {
            gvComments.CurrentPageIndex = 0;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            //BindData();            
            if (Filter.CurrentSelectedComments != null && Filter.CurrentSelectedComments.Count == 1)
            {
                BindSelectedSection();
                ViewState["SELECTEDCOMMENTS"] = "Search";
                gvComments.Rebind();
            }
            if (Filter.CurrentSelectedComments != null && Filter.CurrentSelectedComments.Count > 1)
            {
                ucError.ErrorMessage = "Please select single checkbox to find multiple comments for the single section.";
                Filter.CurrentSelectedComments = null;
                ucError.Visible = true;
                return;
            }
        }

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }

        if (CommandName.ToUpper().Equals("RESET"))
        {
            ClearFilter();
            gvComments.Rebind();
        }
        if (CommandName.ToUpper().Equals("BULKARCHIVE"))
        {
            RadWindowManager1.RadConfirm("Are you sure you want to archive all the selected comments.? Y/N", "confirmarchive", 320, 150, null, "Confirm");
        }
        if (CommandName.ToUpper().Equals("BULKREVIEW"))
        {
            RadWindowManager2.RadConfirm("Are you sure you want to review all the selected comments.? Y/N", "confirmReview", 320, 150, null, "Confirm");
        }

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            //ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvComments.Rebind();
            if (Filter.CurrentSelectedComments != null && ViewState["SELECTEDCOMMENTS"].ToString() == "Search")
            {
                BindSelectedSection();
                //Filter.CurrentSelectedComments = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ClearFilter()
    {
        txtDocumentId.Text = "";
        txtDocumentName.Text = "";
        ucCommentFromDate.Text = "";
        ucCommentToDate.Text = "";
        ucArchivedFromDate.Text = "";
        ucArchivedToDate.Text = "";
        chkArchivedYN.Checked = false;
        chknochangesrequired.Checked = false;
        chkacceptedyn.Checked = false;
        chkcompletedyn.Checked = false;

        txtArchivedBy.Text = "";
        txtArchivedByDesignation.Text = "";
        txtArchivedByName.Text = "";
        txtArchivedByEmailHidden.Text = "";

        txtPostedBy.Text = "";
        txtPostedByName.Text = "";
        txtPostedByDesignation.Text = "";
        txtPostedByEmailHidden.Text = "";

        Filter.CurrentSelectedComments = null;
        ViewState["SELECTEDCOMMENTS"] = "";
        gvComments.CurrentPageIndex = 0;


        SetRowSelection();
        BindData();
        if (Filter.CurrentSelectedComments != null && ViewState["SELECTEDCOMMENTS"].ToString() == "Search")
        {
            BindSelectedSection();
            //Filter.CurrentSelectedComments = null;
        }
    }

    private void BindSelectedSection()
    {
        if (Filter.CurrentSelectedComments != null)
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            // int TotalPageCount = 0;
            int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            ArrayList selectedcomments = (ArrayList)Filter.CurrentSelectedComments;
            if (selectedcomments != null && selectedcomments.Count > 0)
            {
                foreach (Guid commentid in selectedcomments)
                {
                    DataSet ds = PhoenixDocumentManagementDocument.DMSCommentForSelectedSearch(
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                new Guid(commentid.ToString()),
                                                                null,
                                                                null,
                                                                gvComments.CurrentPageIndex + 1,
                                                                gvComments.PageSize,
                                                                ref iRowCount,
                                                                ref iTotalPageCount,
                                                                companyid
                                             );
                    gvComments.DataSource = ds;
                    ViewState["ROWCOUNT"] = iRowCount;
                    ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (General.GetNullableGuid(ViewState["DMSCOMMENTSID"].ToString()) == null)
                        {
                            ViewState["DMSCOMMENTSID"] = ds.Tables[0].Rows[0]["FLDDMSCOMMENTSID"].ToString();
                        }
                    }
                }
            }
        }
    }

    protected void gvComments_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
        if (Filter.CurrentSelectedComments != null && Filter.CurrentSelectedComments.Count > 0 && ViewState["SELECTEDCOMMENTS"].ToString() == "Search")
        {
            BindSelectedSection();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCREATEDDATE", "FLDREFERENCENAME", "FLDCOMMENTS", "FLDCOMMMENTSOURCE", "FLDCREATEDBYNAME", "FLDACCEPTEDYNSTATUS", "FLDDUEDATE", "FLDOFFICEREMARKS", "FLDREVIEWEDBY", "FLDREVIEWEDDATE", "FLDCOMPLETIONDATE", "FLDARCHIVEDBYNAME", "FLDARCHIVEDDATE" };
        string[] alCaptions = { "Date", "File", "Comment", "Source", "Posted By", "Accepted", "Due", "Office Remarks", "Reviewed By", "Reviewed On", "Completed On", "Archived By", "Archived On" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        DataSet ds = PhoenixDocumentManagementDocument.DMSCommentSearch(
                                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                    General.GetNullableGuid(txtDocumentId.Text),
                                                                    General.GetNullableDateTime(ucCommentFromDate.Text),
                                                                    General.GetNullableDateTime(ucCommentToDate.Text),
                                                                    General.GetNullableDateTime(ucArchivedFromDate.Text),
                                                                    General.GetNullableDateTime(ucArchivedToDate.Text),
                                                                    (chkArchivedYN.Checked == true ? 1 : 0),
                                                                    General.GetNullableInteger(txtPostedBy.Text),
                                                                    General.GetNullableInteger(txtArchivedBy.Text),
                                                                    sortexpression,
                                                                    sortdirection,
                                                                    gvComments.CurrentPageIndex + 1,
                                                                    gvComments.PageSize,
                                                                    ref iRowCount,
                                                                    ref iTotalPageCount,
                                                                    companyid,
                                                                    (chkacceptedyn.Checked == true ? 1 : 0),
                                                                    (chkcompletedyn.Checked == true ? 1 : 0),
                                                                    (chknochangesrequired.Checked == true ? 1 : 0),
                                                                    null
                                                                    );

        General.SetPrintOptions("gvComments", "Comments", alCaptions, alColumns, ds);
        gvComments.DataSource = ds;
        gvComments.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (General.GetNullableGuid(ViewState["DMSCOMMENTSID"].ToString()) == null)
            {
                ViewState["DMSCOMMENTSID"] = ds.Tables[0].Rows[0]["FLDDMSCOMMENTSID"].ToString();
            }

        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        string[] alColumns = { "FLDCREATEDDATE", "FLDREFERENCENAME", "FLDCOMMENTS", "FLDCOMMMENTSOURCE", "FLDCREATEDBYNAME", "FLDACCEPTEDYNSTATUS", "FLDDUEDATE", "FLDOFFICEREMARKS", "FLDREVIEWEDBY", "FLDREVIEWEDDATE", "FLDCOMPLETIONDATE", "FLDARCHIVEDBYNAME", "FLDARCHIVEDDATE" };
        string[] alCaptions = { "Date", "File", "Comment", "Source", "Posted By", "Accepted", "Due", "Office Remarks", "Reviewed By", "Reviewed On", "Completed On", "Archived By", "Archived On" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        DataSet ds = PhoenixDocumentManagementDocument.DMSCommentSearch(
                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            General.GetNullableGuid(txtDocumentId.Text),
                                                            General.GetNullableDateTime(ucCommentFromDate.Text),
                                                            General.GetNullableDateTime(ucCommentToDate.Text),
                                                            General.GetNullableDateTime(ucArchivedFromDate.Text),
                                                            General.GetNullableDateTime(ucArchivedToDate.Text),
                                                            (chkArchivedYN.Checked == true ? 1 : 0),
                                                            General.GetNullableInteger(txtPostedBy.Text),
                                                            General.GetNullableInteger(txtArchivedBy.Text),
                                                            sortexpression,
                                                            sortdirection,
                                                            (int)ViewState["PAGENUMBER"],
                                                            iRowCount,
                                                            ref iRowCount,
                                                            ref iTotalPageCount,
                                                            companyid,
                                                            (chkacceptedyn.Checked == true ? 1 : 0),
                                                            (chkcompletedyn.Checked == true ? 1 : 0),
                                                            (chknochangesrequired.Checked == true ? 1 : 0),
                                                             null
                                                            );

        DataTable dt = ds.Tables[0];

        Response.AddHeader("Content-Disposition", "attachment; filename=CommentsList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("Windows-1252");
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Comments</h3></td>");
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
        foreach (DataRow dr in dt.Rows)
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

    protected void gvComments_ItemDataBound(Object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblReferenceId = (RadLabel)e.Item.FindControl("lblReferenceId");
            RadLabel lblCommentId = (RadLabel)e.Item.FindControl("lblCommentId");
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;

                eb.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementCommentsEdit.aspx?COMMENTID=" + lblCommentId.Text + "&REFERENCEID=" + lblReferenceId.Text + "'); return true;");
            }

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, sb.CommandName))
                    sb.Visible = false;
            }

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cb.CommandName))
                    cb.Visible = false;
            }

            if (!e.Item.IsInEditMode)
            {
                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null)
                {
                    del.Attributes.Add("onclick", "return fnConfirmTelerik(event); return false;");
                }
            }

            UserControlToolTip ucRefName = (UserControlToolTip)e.Item.FindControl("ucRefName");
            LinkButton lnkRefName = (LinkButton)e.Item.FindControl("lnkRefName");

            if (lnkRefName != null)
            {
                //lnkRefName.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucRefName.ToolTip + "', 'visible');");
                //lnkRefName.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucRefName.ToolTip + "', 'hidden');");
                ucRefName.Position = ToolTipPosition.TopCenter;
                ucRefName.TargetControlId = lnkRefName.ClientID;
            }

            UserControlToolTip ucOfficeRemarks = (UserControlToolTip)e.Item.FindControl("ucOfficeRemarks");
            RadLabel lblOfficeRemarks = (RadLabel)e.Item.FindControl("lblOfficeRemarks");
            if (lblOfficeRemarks != null)
            {
                //lblOfficeRemarks.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucOfficeRemarks.ToolTip + "', 'visible');");
                //lblOfficeRemarks.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucOfficeRemarks.ToolTip + "', 'hidden');");
                ucOfficeRemarks.Position = ToolTipPosition.TopCenter;
                ucOfficeRemarks.TargetControlId = lblOfficeRemarks.ClientID;
            }

            UserControlToolTip ucComments = (UserControlToolTip)e.Item.FindControl("ucComments");
            LinkButton lnkComments = (LinkButton)e.Item.FindControl("lnkComments");

            if (lnkComments != null)
            {
                ucComments.Position = ToolTipPosition.TopCenter;
                ucComments.TargetControlId = lnkComments.ClientID;
                lnkComments.Attributes.Add("onclick", "javascript:parent.Openpopup('Filter','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementCommentsGeneral.aspx?COMMENTSID=" + lblCommentId.Text + "'); return false;");
            }

            DataRowView dv = (DataRowView)e.Item.DataItem;
            LinkButton cmdArchive = (LinkButton)e.Item.FindControl("cmdArchive");
            RadCheckBox chkSelect = (RadCheckBox)e.Item.FindControl("chkSelect");
            LinkButton cmdReview = (LinkButton)e.Item.FindControl("cmdReview");

            if (cmdArchive != null && dv["FLDARCHIVEDYN"].ToString() == "1")
            {
                cmdArchive.Visible = false;
                chkSelect.Visible = false;
            }


            if (cmdArchive != null && !SessionUtil.CanAccess(this.ViewState, cmdArchive.CommandName))
                cmdArchive.Visible = false;

            if (cmdReview != null && !SessionUtil.CanAccess(this.ViewState, cmdReview.CommandName))
                cmdReview.Visible = false;

            if (!e.Item.IsInEditMode)
            {
                if (cmdArchive != null)
                {
                    cmdArchive.Attributes.Add("onclick", "return fnConfirmTelerik(event,'Do you want to archive this comment.'); return false;");
                }

                if (cmdReview != null)
                {
                    cmdReview.Attributes.Add("onclick", "return fnConfirmTelerik(event,'Do you want to review this comment.'); return false;");
                }
            }

            //if (cmdReview != null)
            //{
            //    cmdReview.Attributes.Add("onclick", "javascript:parent.Openpopup('NAFA','','../DocumentManagement/DocumentManagementOfficeRemarks.aspx?VIEW=review&COMMENTID=" + lblCommentId.Text + "&REFERENCEID=" + lblReferenceId.Text + "'); return true;");
            //}

            RadLabel txtCommentsId = (RadLabel)e.Item.FindControl("lblCommentId");
            LinkButton cmdMoreInfo = (LinkButton)e.Item.FindControl("cmdMoreInfo");
            if (cmdMoreInfo != null)
            {
                cmdMoreInfo.Attributes.Add("onclick", "parent.openNewWindow('codeHelp1', '', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementCommentsMoreInfo.aspx?CommentsId=" + txtCommentsId.Text + "');return false;");
            }
        }
    }

    protected void gvComments_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        gvComments.Rebind();

    }

    //protected void gvComments_RowEditing(object sender, GridViewEditEventArgs de)
    //{

    //    GridView _gridView = (GridView)sender;

    //    _gridView.EditIndex = de.NewEditIndex;
    //    _gridView.SelectedIndex = de.NewEditIndex;

    //    BindData();
    //    if (Filter.CurrentSelectedComments != null && ViewState["SELECTEDCOMMENTS"].ToString() == "Search")
    //    {
    //        BindSelectedSection();
    //        //Filter.CurrentSelectedComments = null;
    //    }
    //    //((CheckBox)_gridView.Rows[de.NewEditIndex].FindControl("chkAcceptanceYNEdit")).Focus();
    //}

    protected void gvComments_ItemCommand(object sender, GridCommandEventArgs e)
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

            if (e.CommandName.ToUpper().Equals("ROWCLICK"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                Guid documentid = new Guid(item.GetDataKeyValue("FLDDMSCOMMENTSID").ToString());
                ////gvDocument.SelectedIndexes.Add(e.Item.ItemIndex);
                //BindPageURL(e.Item.ItemIndex);
            }

            if (e.CommandName.ToUpper().Equals("ARCHIVE"))
            {
                BindPageURL(nCurrentRow);
                RadLabel lblCommentId = ((RadLabel)gvComments.Items[nCurrentRow].FindControl("lblCommentId"));

                PhoenixDocumentManagementDocument.DMSCommentArchive(
                                         PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                         , new Guid(lblCommentId.Text)
                                         , 1
                                         );
                ucStatus.Text = "Comment is archived.";
                BindData();
                if (Filter.CurrentSelectedComments != null && ViewState["SELECTEDCOMMENTS"].ToString() == "Search")
                {
                    BindSelectedSection();
                    //Filter.CurrentSelectedComments = null;
                }
                gvComments.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                RadLabel lblCommentId = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblCommentId");
                PhoenixDocumentManagementDocument.CommentDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(lblCommentId.Text));
                ViewState["DMSCOMMENTSID"] = "";
                ucStatus.Text = "Comment is deleted.";
                BindData();
                if (Filter.CurrentSelectedComments != null && ViewState["SELECTEDCOMMENTS"].ToString() == "Search")
                {
                    BindSelectedSection();
                    //Filter.CurrentSelectedComments = null;
                }
                gvComments.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                BindPageURL(nCurrentRow);

            }
            else if (e.CommandName.ToUpper().Equals("REVIEW"))
            {
                BindPageURL(nCurrentRow);
                RadLabel lblCommentId = ((RadLabel)gvComments.Items[nCurrentRow].FindControl("lblCommentId"));

                PhoenixDocumentManagementDocument.DMSCommentReviewSinglepdate(
                                         PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                         , new Guid(lblCommentId.Text)
                                         );
                ucStatus.Text = "Comment is reviewed.";
                BindData();
                if (Filter.CurrentSelectedComments != null && ViewState["SELECTEDCOMMENTS"].ToString() == "Search")
                {
                    BindSelectedSection();
                    //Filter.CurrentSelectedComments = null;
                }
                gvComments.Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("FILEEDIT"))
            {
                RadLabel lblsectionno = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblsectionno");
                RadLabel lblDocumentId = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblDocumentId");
                RadLabel lblformid = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblformid");
                RadLabel lblformno = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblformno");
                RadLabel lblsectionid = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblsectionid");
                RadLabel lblAcceptanceYN = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblAcceptanceYN");
                RadLabel lblReviewedByLink = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblReviewedBy");
                RadLabel lblcategoryid = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblcategoryid");

                if (lblAcceptanceYN.Text == "Yes" && (lblReviewedByLink.Text != ""))
                {
                    if (lblsectionid.Text != "")
                    {
                        Response.Redirect("../DocumentManagement/DocumentManagementAdminDocumentSectionList.aspx?DOCUMENTID=" + lblDocumentId.Text + "&SECTIONNO=" + lblsectionno.Text.Trim(), false);
                    }
                    if ((lblsectionid.Text == "") && (lblDocumentId.Text != "") && (lblformid.Text == "") && (lblformno.Text == ""))
                    {
                        Response.Redirect("../DocumentManagement/DocumentManagementAdminDocumentList.aspx?DOCUMENTID=" + lblDocumentId.Text, false);
                    }
                    if (lblformno.Text != "")
                    {
                        Response.Redirect("../DocumentManagement/DocumentManagementFormList.aspx?FORMNO=" + lblformno.Text.Trim(), false);
                    }
                    if (lblcategoryid.Text != "")
                    {
                        Response.Redirect("../DocumentManagement/DocumentManagementFormList.aspx?CATEGORYID=" + lblcategoryid.Text.Trim(), false);
                    }
                }
                else
                {
                    ucError.ErrorMessage = "Document cannot be Revised before it is Accepted and Reviewed";
                    ucError.Visible = true;
                }
            }
            //BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComments_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem item = (GridEditableItem)e.Item;
            int nCurrentRow = e.Item.ItemIndex;
            DateTime resultDate;

            RadLabel lblCommentId = (RadLabel)item.FindControl("lblCommentId");
            RadLabel lblReferenceId = (RadLabel)item.FindControl("lblReferenceId");
            UserControlDate duedate = ((UserControlDate)item.FindControl("ucDueDateEdit"));

            if (DateTime.TryParse(duedate.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) < 0)
            {
                ucError.ErrorMessage = "Due Date should be later than current date.";
                ucError.Visible = true;
                return;
            }

            PhoenixDocumentManagementDocument.DMSCommentUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                  General.GetNullableGuid(((RadLabel)item.FindControl("lblReferenceId")).Text)
                , General.GetNullableGuid(((RadLabel)item.FindControl("lblCommentId")).Text)
                , (((CheckBox)item.FindControl("chkAcceptanceYNEdit")).Checked) ? 1 : 0
                , General.GetNullableDateTime(((UserControlDate)item.FindControl("ucDueDateEdit")).Text)
                , General.GetNullableString(((RadTextBox)item.FindControl("txtOfficeRemarksEdit")).Text)
                );

            ucStatus.Text = "Comments details updated.";
            gvComments.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvComments_SelectedIndexChanging(object sender, GridSelectCommandEventArgs se)
    //{
    //    GridDataItem item = (GridDataItem)se.Item;
    //    int currentrow = item.ItemIndex;
    //    RadCheckBox cbSelected = (RadCheckBox)gvComments.Items[currentrow].FindControl("chkSelect");
    //    RadLabel lblCommentId = (RadLabel)gvComments.Items[currentrow].FindControl("lblCommentId");
    //    ViewState["DMSCOMMENTSID"] = lblCommentId.Text;
    //    SetRowSelectionForSearch();
    //    for (int i = 0; i < gvComments.Items.Count; i++)
    //    {
    //        if (i == currentrow)
    //        {
    //            CheckBox cb = (CheckBox)gvComments.Items[currentrow].FindControl("chkSelect");
    //            cb.Checked = false;
    //        }
    //    }
    //}

    private void SetRowSelectionForSearch()
    {

        foreach (GridDataItem item in gvComments.Items)
        {
            if (item.GetDataKeyValue("FLDDMSCOMMENTSID").ToString().Equals(ViewState["DMSCOMMENTSID"].ToString()))
            {

                ArrayList selectedvalue = new ArrayList();
                CheckBox cb = (CheckBox)gvComments.Items[item.ItemIndex].FindControl("chkSelect");
                cb.Checked = true;
                if (cb.Checked == true)
                {
                    if (!selectedvalue.Contains(new Guid(ViewState["DMSCOMMENTSID"].ToString())))
                        selectedvalue.Add(new Guid(ViewState["DMSCOMMENTSID"].ToString()));
                }
                else
                    selectedvalue.Remove(new Guid(ViewState["DMSCOMMENTSID"].ToString()));

                if (selectedvalue != null && selectedvalue.Count > 0)
                    Filter.CurrentSelectedComments = selectedvalue;

            }
        }
    }

    private void SetRowSelection()
    {

        foreach (GridDataItem item in gvComments.Items)
        {
            if (item.GetDataKeyValue("FLDDMSCOMMENTSID").ToString().Equals(ViewState["DMSCOMMENTSID"].ToString()))
            {
                gvComments.SelectedIndexes.Add(item.ItemIndex);
            }
        }
    }
    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblCommentId = (RadLabel)gvComments.Items[rowindex].FindControl("lblCommentId");
            if (lblCommentId != null)
            {
                ViewState["DMSCOMMENTSID"] = lblCommentId.Text;
            }

            SetRowSelection();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void confirmarchive_Click(object sender, EventArgs e)
    {
        try
        {
            if (Filter.CurrentSelectedComments != null)
            {
                ArrayList selectedcomments = (ArrayList)Filter.CurrentSelectedComments;
                if (selectedcomments != null && selectedcomments.Count > 0)
                {
                    foreach (Guid commentid in selectedcomments)
                    {
                        PhoenixDocumentManagementDocument.DMSCommentArchive(
                                                 PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                 , new Guid(commentid.ToString())
                                                 , 1
                                                 );
                    }
                }
            }
            //Filter.CurrentSelectedComments = null;
            ucStatus.Text = "Comments are archived.";
            BindData();
            if (Filter.CurrentSelectedComments != null && ViewState["SELECTEDCOMMENTS"].ToString() == "Search")
            {
                BindSelectedSection();
                //Filter.CurrentSelectedComments = null;
            }
            gvComments.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void confirmReview_Click(object sender, EventArgs e)
    {
        try
        {

            if (Filter.CurrentSelectedComments != null)
            {
                ArrayList selectedcomments = (ArrayList)Filter.CurrentSelectedComments;
                if (selectedcomments != null && selectedcomments.Count > 0)
                {
                    foreach (Guid commentid in selectedcomments)
                    {
                        PhoenixDocumentManagementDocument.DMSCommentReviewSinglepdate(
                                                 PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                 , new Guid(commentid.ToString())
                                                 );
                    }
                }
            }
            //Filter.CurrentSelectedComments = null;
            ucStatus.Text = "Comments are reviewed.";
            BindData();
            if (Filter.CurrentSelectedComments != null && ViewState["SELECTEDCOMMENTS"].ToString() == "Search")
            {
                BindSelectedSection();
                //Filter.CurrentSelectedComments = null;
            }
            gvComments.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }


    protected void CheckAll(Object sender, EventArgs e)
    {
        RadCheckBox headerCheckBox = (sender as RadCheckBox);
        ArrayList SelectedForms = new ArrayList();
        Guid index = new Guid();

        foreach (GridDataItem dataItem in gvComments.MasterTableView.Items)
        {
            bool result = false;
            if (headerCheckBox.Checked == true)
            {
                if (dataItem.GetDataKeyValue("FLDDMSCOMMENTSID").ToString() != "")
                {
                    index = new Guid(dataItem.GetDataKeyValue("FLDDMSCOMMENTSID").ToString());
                    (dataItem.FindControl("chkSelect") as RadCheckBox).Checked = true;
                    result = true;
                }
                //dataItem.Selected = true;
            }
            else
            {
                (dataItem.FindControl("chkSelect") as RadCheckBox).Checked = false;
                Filter.CurrentSelectedComments = null;
            }

            // Check in the Session
            if (Filter.CurrentSelectedComments != null)
                SelectedForms = (ArrayList)Filter.CurrentSelectedComments;
            if (result)
            {
                if (!SelectedForms.Contains(index))
                    SelectedForms.Add(index);
            }
            else
                SelectedForms.Remove(index);
        }
        if (SelectedForms != null && SelectedForms.Count > 0)
            Filter.CurrentSelectedComments = SelectedForms;
    }

    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        ArrayList SelectedSections = new ArrayList();
        Guid index = new Guid();


        foreach (GridDataItem item in gvComments.Items)
        {
            bool result = false;

            if (item.GetDataKeyValue("FLDDMSCOMMENTSID").ToString() != "")
            {
                index = new Guid(item.GetDataKeyValue("FLDDMSCOMMENTSID").ToString());

                if (((RadCheckBox)(item.FindControl("chkSelect"))).Checked == true)
                {
                    result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
                }

                // Check in the Session
                if (Filter.CurrentSelectedComments != null)
                    SelectedSections = (ArrayList)Filter.CurrentSelectedComments;
                if (result)
                {
                    if (!SelectedSections.Contains(index))
                        SelectedSections.Add(index);
                }
                else
                    SelectedSections.Remove(index);
            }
        }
        if (SelectedSections != null && SelectedSections.Count > 0)
            Filter.CurrentSelectedComments = SelectedSections;
    }

    private void GetSelectedPvs()
    {
        if (Filter.CurrentSelectedComments != null)
        {
            ArrayList SelectedSections = (ArrayList)Filter.CurrentSelectedComments;
            Guid index = new Guid();
            if (SelectedSections != null && SelectedSections.Count > 0)
            {
                foreach (GridDataItem row in gvComments.Items)
                {
                    //CheckBox ChkPlan = (CheckBox)row["ClientSelectColumn"].Controls[0];
                    //CheckBox chk = (CheckBox)(row.Cells[0].FindControl("chkSelect"));
                    RadCheckBox chk = (RadCheckBox)(row.Cells[0].FindControl("chkSelect"));
                    index = new Guid(row.GetDataKeyValue("FLDDMSCOMMENTSID").ToString());
                    if (SelectedSections.Contains(index))
                    {
                        RadCheckBox myCheckBox = (RadCheckBox)row.FindControl("chkSelect");
                        myCheckBox.Checked = true;
                    }
                }
            }
        }

    }

}
