using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;

public partial class DocumentManagementFormUploadRevisionList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvFormRevision.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvFormRevision.UniqueID, "Select$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddImageButton("../DocumentManagement/DocumentManagementFormRevisionList.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbar.AddImageLink("javascript:CallPrint('gvFormRevision')", "Print Grid", "icon_print.png", "PRINT");

                MenuRegistersCountry.AccessRights = this.ViewState;
                MenuRegistersCountry.MenuList = toolbar.Show();

                toolbar = new PhoenixToolbar();
                toolbar.AddImageButton("../DocumentManagement/DocumentManagementFormRevisionList.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbar.AddImageLink("javascript:CallPrint('gvFormField')", "Print Grid", "icon_print.png", "PRINT");
                //MenuField.AccessRights = this.ViewState;
                //MenuField.MenuList = toolbar.Show();

                toolbar = new PhoenixToolbar();
                toolbar.AddButton("Category", "CATEGORY");
                toolbar.AddButton("Form", "FORM");
                toolbar.AddButton("Revision", "REVISION");

                MenuDocument.AccessRights = this.ViewState;
                //MenuDocument.MenuList = toolbar.Show();
                //MenuDocument.SelectedMenuIndex = 2;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["PAGENUMBERSUB"] = 1;
                ViewState["SORTEXPRESSIONSUB"] = null;
                ViewState["SORTDIRECTIONSUB"] = null;
                ViewState["CURRENTINDEXSUB"] = 1;

                ViewState["REVISIONID"] = "";
                ViewState["FIELDID"] = "";
                ViewState["APPROVEDYN"] = "";

                if (Request.QueryString["FORMID"] != null && Request.QueryString["FORMID"].ToString() != "")
                    ViewState["FORMID"] = Request.QueryString["FORMID"].ToString();
                else
                    ViewState["FORMID"] = "";

                if (Request.QueryString["CATEGORYID"] != null && Request.QueryString["CATEGORYID"].ToString() != "")
                    ViewState["CATEGORYID"] = Request.QueryString["CATEGORYID"].ToString();
                else
                    ViewState["CATEGORYID"] = "";

                GetFormDetails();
            }
            BindData();            
            SetPageNavigator();            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindData();
        SetPageNavigator();
    }

    private void GetFormDetails()
    {
        if (ViewState["FORMID"] != null && ViewState["FORMID"].ToString() != "")
        {
            DataSet ds = PhoenixDocumentManagementForm.FormEdit(
                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , new Guid(ViewState["FORMID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ucTitle.Text = dr["FLDCAPTION"].ToString() + " - " + "Revisions";
            }
        }
    }

    protected void RegistersCountry_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvFormRevision.EditIndex = -1;
                gvFormRevision.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }
            else if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentDMSDocumentFilter = null;
                ViewState["DOCUMENTID"] = "";
                BindData();
                SetPageNavigator();
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
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("CATEGORY"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFormCategoryList.aspx?CATEGORYID=" + ViewState["CATEGORYID"].ToString());
            }
            else if (dce.CommandName.ToUpper().Equals("FORM"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFormList.aspx?FORMID=" + ViewState["FORMID"] + "&CATEGORYID=" + ViewState["CATEGORYID"].ToString());
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
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                
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

        string[] alColumns = { "FLDCREATEDDATE", "FLDAUTHORNAME", "FLDREVISIONDETAILS", "FLDAPPROVEDDATE", "FLDAPPROVEDBYNAME" };
        string[] alCaptions = { "Added Date", "Added By", "Revision", "Approved Date", "Approved By" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixDocumentManagementForm.FormRevisionSearch(
                                                                 PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableGuid(ViewState["FORMID"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , General.ShowRecords(null)
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        General.SetPrintOptions("gvFormRevision", ucTitle.Text, alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvFormRevision.DataSource = ds;
            gvFormRevision.DataBind();
            if (General.GetNullableGuid(ViewState["REVISIONID"].ToString()) == null)
            {
                ViewState["REVISIONID"] = ds.Tables[0].Rows[0]["FLDFORMREVISIONID"].ToString();
                ViewState["APPROVEDYN"] = ds.Tables[0].Rows[0]["FLDAPPROVEDYN"].ToString();
                gvFormRevision.SelectedIndex = 0;
            }
            SetRowSelection();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvFormRevision);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDCREATEDDATE", "FLDAUTHORNAME", "FLDREVISIONDETAILS", "FLDAPPROVEDDATE", "FLDAPPROVEDBYNAME" };
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

        ds = PhoenixDocumentManagementForm.FormRevisionSearch(
                                                              PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , General.GetNullableGuid(ViewState["FORMID"].ToString())
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
        Response.Write("<td><h3>" + ucTitle.Text + "</h3></td>");
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

    protected void gvFormRevision_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();

    }

    protected void gvFormRevision_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvFormRevision.EditIndex = -1;
        gvFormRevision.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvFormRevision_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

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
                    , new Guid(ViewState["REVISIONID"].ToString()))
                    ;

                ucStatus.Text = "Revision is approved.";
                BindData();

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixDocumentManagementForm.FormRevisionDelete(
                      PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblRevisionId")).Text));

                ViewState["REVISIONID"] = "";
                BindData();
                //BindFieldsData();
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
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvFormRevision_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvFormRevision, "Select$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void gvFormRevision_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvFormRevision_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvFormRevision_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            _gridView.EditIndex = -1;
            BindData();

            SetPageNavigator();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFormRevision_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }
            }

            Label lblVersionNumber = (Label)e.Row.FindControl("lblVersionNumber");
            ImageButton cmgEditContent = (ImageButton)e.Row.FindControl("cmgEditContent");
            ImageButton cmdApprove = (ImageButton)e.Row.FindControl("cmdApprove");
            ImageButton cmdDelete = (ImageButton)e.Row.FindControl("cmdDelete");
            Label lblRevisionId = (Label)e.Row.FindControl("lblRevisonId");

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

            DataRowView dr = (DataRowView)e.Row.DataItem;
            LinkButton lnkAddedDate = (LinkButton)e.Row.FindControl("lnkAddedDate");

            //if (lnkAddedDate != null)
            //    lnkAddedDate.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp','','../DocumentManagement/DocumentManagementFormPreview.aspx?FORMID=" + dr["FLDFORMID"].ToString() + "&FORMREVISIONID=" + dr["FLDFORMREVISIONID"].ToString() + "'); return false;");
            
            HyperLink lnkfilename = (HyperLink)e.Row.FindControl("lnkfilename");
            HyperLink hlnkDraftName = (HyperLink)e.Row.FindControl("hlnkDraftName");

            if (dr["FLDDTKEY"] != null && General.GetNullableGuid(dr["FLDDTKEY"].ToString()) != null)
            {
                DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(dr["FLDDTKEY"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    DataRow drRow = dt.Rows[0];
                    if (lnkfilename != null)
                        lnkfilename.NavigateUrl = "../Common/download.aspx?dtkey=" + dr["FLDDTKEY"].ToString();

                    //lnkfilename.NavigateUrl = Session["sitepath"] + "/attachments/" + drRow["FLDFILEPATH"].ToString();
                }
            }            

            DataRowView dv = (DataRowView)e.Row.DataItem;
            ImageButton cmdCreate = (ImageButton)e.Row.FindControl("cmdCreate");
            if (cmdCreate != null && dv["FLDLATESTREVISIONYN"].ToString() == "1")
                cmdCreate.Visible = true;

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    protected void gvFormRevision_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        gvFormRevision.SelectedIndex = se.NewSelectedIndex;
        BindPageURL(se.NewSelectedIndex);
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["REVISIONID"] = ((Label)gvFormRevision.Rows[rowindex].FindControl("lblRevisionId")).Text;
            ViewState["APPROVEDYN"] = ((Label)gvFormRevision.Rows[rowindex].FindControl("lblApprovedYN")).Text;
            //ViewState["FIELDID"] = "";
            //BindFieldsData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvFormRevision.EditIndex = -1;
        gvFormRevision.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvFormRevision.EditIndex = -1;
        gvFormRevision.SelectedIndex = -1;
        int result;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindData();
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvFormRevision.SelectedIndex = -1;
        gvFormRevision.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }    

    private void DeleteDocumentSectionRevision(string documentid)
    {
        PhoenixDocumentManagementDocumentSectionRevison.DocumentSectionRevisonDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(documentid));
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

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    private void SetRowSelection()
    {
        gvFormRevision.SelectedIndex = -1;

        for (int i = 0; i < gvFormRevision.Rows.Count; i++)
        {
            if (gvFormRevision.DataKeys[i].Value.ToString().Equals(ViewState["REVISIONID"].ToString()))
            {
                gvFormRevision.SelectedIndex = i;
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvFormRevision.EditIndex = -1;
        gvFormRevision.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
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
    
}
