using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using System.Collections.Specialized;
using System;

public partial class DocumentManagementDocumentSectionRevisonList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../DocumentManagement/DocumentManagementDocumentSectionRevisonList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvDocumentSectionRevison')", "Print Grid", "icon_print.png", "PRINT");
            //toolbar.AddImageLink("javascript:Openpopup('Filter','','DocumentManagementDocumentFilter.aspx'); return false;", "Filter", "search.png", "FIND");
            //toolbar.AddImageButton("../DocumentManagement/DocumentManagementDocumentSectionRevison.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            MenuRegistersCountry.AccessRights = this.ViewState;
            MenuRegistersCountry.MenuList = toolbar.Show();

            if (!IsPostBack)
            {               
                toolbar = new PhoenixToolbar();
                toolbar.AddButton("Close", "CLOSE");
                //MenuDocument.AccessRights = this.ViewState;
                //MenuDocument.MenuList = toolbar.Show();  
                ucTitle.ShowMenu = "false";
                    
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["DOCUMENTID"] = "";

                if (Request.QueryString["DOCUMENTID"] != null && Request.QueryString["DOCUMENTID"].ToString() != "")
                    ViewState["DOCUMENTID"] = Request.QueryString["DOCUMENTID"].ToString();
                else
                    ViewState["DOCUMENTID"] = "";

                if (Request.QueryString["SECTIONID"] != null && Request.QueryString["SECTIONID"].ToString() != "")
                    ViewState["SECTIONID"] = Request.QueryString["SECTIONID"].ToString();
                else
                    ViewState["SECTIONID"] = "";

                if (Request.QueryString["REVISONID"] != null && Request.QueryString["REVISONID"].ToString() != "")
                    ViewState["REVISONID"] = Request.QueryString["REVISONID"].ToString();
                else
                    ViewState["REVISONID"] = "";

                ifMoreInfo.Visible = false;
                GetDocumentAndSectionDetails();             
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

    private void GetDocumentAndSectionDetails()
    {
        if (ViewState["SECTIONID"] != null && ViewState["SECTIONID"].ToString() != "")
        {
            DataSet ds = PhoenixDocumentManagementDocumentSection.DocumentSectionEdit(new Guid(ViewState["SECTIONID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ucTitle.Text = dr["FLDDOCUMENTNAME"].ToString();
                ucTitle.Text = ucTitle.Text + " /" + dr["FLDSECTIONDETAILS"].ToString();
                ucTitle.Text = ucTitle.Text + " - Revisions";
            }
        }
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

        NameValueCollection nvc = Filter.CurrentDMSDocumentFilter;

        string documentid = (nvc != null && nvc.Get("ddlDocument") != null) ? nvc.Get("ddlDocument").ToString() : "";
        string sectionid = (nvc != null && nvc.Get("ddlSection") != null) ? nvc.Get("ddlSection").ToString() : "";
        string revisonid = (nvc != null && nvc.Get("ddlRevison") != null) ? nvc.Get("ddlRevison").ToString() : "";
       
         if (General.GetNullableGuid(revisonid) != null)
            ViewState["REVISONID"] = "";

        ds = PhoenixDocumentManagementDocumentSectionRevison.DocumentSectionRevisonSearch(
                                                                  PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableGuid(documentid)
                                                                , General.GetNullableGuid(sectionid)
                                                                , General.GetNullableGuid(revisonid)
                                                                , null                                                                
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , iRowCount
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=DocumentSectionRevisions.xls");
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

    protected void RegistersCountry_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvDocumentSectionRevison.EditIndex = -1;
                gvDocumentSectionRevison.SelectedIndex = -1;
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
            if (dce.CommandName.ToUpper().Equals("CLOSE"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementDocumentSectionList.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + ViewState["SECTIONID"].ToString());
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

        NameValueCollection nvc = Filter.CurrentDMSDocumentFilter;

        string documentid = (nvc != null && nvc.Get("ddlDocument") != null) ? nvc.Get("ddlDocument").ToString() : ViewState["DOCUMENTID"].ToString();
        string sectionid = (nvc != null && nvc.Get("ddlSection") != null) ? nvc.Get("ddlSection").ToString() : ViewState["SECTIONID"].ToString();
        string revisonid = (nvc != null && nvc.Get("ddlRevison") != null) ? nvc.Get("ddlRevison").ToString() : "";

        if (General.GetNullableGuid(revisonid) != null)
            ViewState["REVISONID"] = "";

        ds = PhoenixDocumentManagementDocumentSectionRevison.DocumentSectionRevisonSearch(
                                                                  PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableGuid(documentid)
                                                                , General.GetNullableGuid(sectionid)
                                                                , General.GetNullableGuid(revisonid)
                                                                , null
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , General.ShowRecords(null)
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        General.SetPrintOptions("gvDocumentSectionRevison", ucTitle.Text, alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDocumentSectionRevison.DataSource = ds;
            gvDocumentSectionRevison.DataBind();
            if (General.GetNullableGuid(ViewState["REVISONID"].ToString()) == null)
            {
                ViewState["REVISONID"] = ds.Tables[0].Rows[0]["FLDREVISONID"].ToString();
                ViewState["DOCUMENTID"] = ds.Tables[0].Rows[0]["FLDDOCUMENTID"].ToString();
                gvDocumentSectionRevison.SelectedIndex = 0;
                ifMoreInfo.Attributes["src"] = "../DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?REVISONID=" + ViewState["REVISONID"].ToString();
            }
            SetRowSelection();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvDocumentSectionRevison);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvDocumentSectionRevison_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();

    }

    protected void gvDocumentSectionRevison_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvDocumentSectionRevison.EditIndex = -1;
        gvDocumentSectionRevison.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvDocumentSectionRevison_RowCommand(object sender, GridViewCommandEventArgs e)
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
            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                BindPageURL(nCurrentRow);
            }
            //else if (e.CommandName.ToUpper().Equals("EDITCONTENT"))
            //{
            //    BindPageURL(nCurrentRow);
            //    Response.Redirect("../DocumentManagement/DocumentManagementDocumentSectionContentGeneral.aspx?callfrom=revison&DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + ViewState["SECTIONID"].ToString() + "&REVISONID=" + ViewState["REVISONID"].ToString());
            //}
            else if (e.CommandName.ToUpper().Equals("VIEWCONTENT"))
            {
                BindPageURL(nCurrentRow);
                Response.Redirect("../DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?callfrom=revison&DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + ViewState["SECTIONID"].ToString() + "&REVISONID=" + ViewState["REVISONID"].ToString());
            }
            else if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                BindPageURL(nCurrentRow);
                PhoenixDocumentManagementDocumentSectionRevison.DocumentSectionRevisonApprove(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["SECTIONID"].ToString())
                    , new Guid(ViewState["DOCUMENTID"].ToString())
                    , new Guid(ViewState["REVISONID"].ToString())
                    );
                ucStatus.Text = "Revison is approved.";
                BindData();

                String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
            } 
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteDocumentSectionRevision((((Label)_gridView.Rows[nCurrentRow].FindControl("lblRevisonId")).Text));
                ViewState["REVISONID"] = "";

                String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
            }

            SetPageNavigator();            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvDocumentSectionRevison_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvDocumentSectionRevison_RowEditing(object sender, GridViewEditEventArgs de)
    {        
        BindData();        
        SetPageNavigator();
    }

    protected void gvDocumentSectionRevison_RowUpdating(object sender, GridViewUpdateEventArgs e)
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

    protected void gvDocumentSectionRevison_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            Label lblSectionId = (Label)e.Row.FindControl("lblSectionId");
            Label lblRevisonId = (Label)e.Row.FindControl("lblRevisonId");
            Label lblDocumentId = (Label)e.Row.FindControl("lblDocumentId");
            
            if (lblVersionNumber != null && lblVersionNumber.Text != null && General.GetNullableString(lblVersionNumber.Text) != null)
            {
                if (cmgEditContent != null)
                    cmgEditContent.Enabled = false;
                if (cmdApprove != null)
                    cmdApprove.Enabled = false;
            }

            if (cmgEditContent != null && lblSectionId != null)
                cmgEditContent.Attributes.Add("onclick", "javascript:parent.Openpopup('Filter','','../DocumentManagement/DocumentManagementDocumentSectionContentGeneral.aspx?DOCUMENTID=" + lblDocumentId.Text + "&SECTIONID=" + lblSectionId.Text + "&REVISONID=" + lblRevisonId.Text + "'); return false;");
            
            if (cmgEditContent != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmgEditContent.CommandName))
                    cmgEditContent.Visible = false;
            }
            if (cmdApprove != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdApprove.CommandName))
                    cmdApprove.Visible = false;
            }

            DataRowView dr = (DataRowView)e.Row.DataItem;
            LinkButton lnkAddedDate = (LinkButton)e.Row.FindControl("lnkAddedDate");

            if (lnkAddedDate != null)
                lnkAddedDate.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp','','../DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + dr["FLDDOCUMENTID"].ToString() + "&SECTIONID=" + dr["FLDSECTIONID"].ToString() + "&REVISONID=" + dr["FLDREVISONID"].ToString() + "'); return false;");

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

    protected void gvDocumentSectionRevison_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        gvDocumentSectionRevison.SelectedIndex = se.NewSelectedIndex;
        BindPageURL(se.NewSelectedIndex);
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["REVISONID"] = ((Label)gvDocumentSectionRevison.Rows[rowindex].FindControl("lblRevisonId")).Text;
            ViewState["DOCUMENTID"] = ((Label)gvDocumentSectionRevison.Rows[rowindex].FindControl("lblDocumentId")).Text;
            //Filter.CurrentSelectedBulkOrderId = ((Label)gvDocumentSectionRevison.Rows[rowindex].FindControl("lblOrderId")).Text;    
            ifMoreInfo.Attributes["src"] = "../DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?REVISONID=" + ViewState["REVISONID"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvDocumentSectionRevison.EditIndex = -1;
        gvDocumentSectionRevison.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvDocumentSectionRevison.EditIndex = -1;
        gvDocumentSectionRevison.SelectedIndex = -1;
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
        gvDocumentSectionRevison.SelectedIndex = -1;
        gvDocumentSectionRevison.EditIndex = -1;
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
        gvDocumentSectionRevison.SelectedIndex = -1;

        for (int i = 0; i < gvDocumentSectionRevison.Rows.Count; i++)
        {
            if (gvDocumentSectionRevison.DataKeys[i].Value.ToString().Equals(ViewState["REVISONID"].ToString()))
            {
                gvDocumentSectionRevison.SelectedIndex = i;
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvDocumentSectionRevison.EditIndex = -1;
        gvDocumentSectionRevison.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }
}
