using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using System.Collections.Specialized;

public partial class DocumentManagementDocumentSectionList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvDocument.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvDocument.UniqueID, "Select$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../DocumentManagement/DocumentManagementDocumentSectionList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvDocument')", "Print Grid", "icon_print.png", "PRINT");            
            //toolbar.AddImageLink("javascript:Openpopup('Filter','','DocumentManagementDocumentFilter.aspx'); return false;", "Filter", "search.png", "FIND");
            //toolbar.AddImageButton("../DocumentManagement/DocumentManagementDocumentSectionList.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            MenuRegistersCountry.AccessRights = this.ViewState;
            MenuRegistersCountry.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                toolbar = new PhoenixToolbar();
                toolbar.AddButton("Document", "DOCUMENT");
                toolbar.AddButton("Section", "SECTION");

                //MenuDocument.AccessRights = this.ViewState;
                //MenuDocument.MenuList = toolbar.Show();
                //MenuDocument.SelectedMenuIndex = 1;

                if (Request.QueryString["DOCUMENTID"] != null && Request.QueryString["DOCUMENTID"].ToString() != "")
                {
                    ViewState["DOCUMENTID"] = Request.QueryString["DOCUMENTID"].ToString();
                    GetDocumentName();
                }
                else
                    ViewState["DOCUMENTID"] = "";

                if (Request.QueryString["SECTIONID"] != null && Request.QueryString["SECTIONID"].ToString() != "")
                    ViewState["SECTIONID"] = Request.QueryString["SECTIONID"].ToString();
                else
                    ViewState["SECTIONID"] = "";

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;                
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

    protected void RegistersCountry_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvDocument.EditIndex = -1;
                gvDocument.SelectedIndex = -1;
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
            if (dce.CommandName.ToUpper().Equals("DOCUMENT"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementAdminDocumentList.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"]);
            }           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    private void GetDocumentName()
    {
        if (ViewState["DOCUMENTID"] != null && ViewState["DOCUMENTID"].ToString() != "")
        {
            DataSet ds = PhoenixDocumentManagementDocument.DocumentEdit(new Guid(ViewState["DOCUMENTID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ucTitle.Text = dr["FLDDOCUMENTNAME"].ToString();
            }
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSECTIONNUMBER", "FLDSECTIONNAME", "FLDREVISIONDETAILS", "FLDACTIVESTATUS" };
        string[] alCaptions = { "Section", "Name", "Revision","Active Y/N" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
       
        NameValueCollection nvc = Filter.CurrentDMSDocumentFilter;

        string documentid = (nvc != null && nvc.Get("ddlDocument") != null) ? nvc.Get("ddlDocument").ToString() : "";

        if (General.GetNullableGuid(documentid) == null)
            documentid = ViewState["DOCUMENTID"].ToString();       

        ds = PhoenixDocumentManagementDocumentSection.DocumentSectionSearch(
                                                                  PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableGuid(documentid)
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , General.ShowRecords(null)
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , null );


        General.SetPrintOptions("gvDocument", ucTitle.Text + " - Sections", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDocument.DataSource = ds;
            gvDocument.DataBind();
            if (General.GetNullableGuid(ViewState["SECTIONID"].ToString()) == null)
            {
                ViewState["SECTIONID"] = ds.Tables[0].Rows[0]["FLDSECTIONID"].ToString();
                gvDocument.SelectedIndex = 0;
            }
            SetRowSelection();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvDocument);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSECTIONNUMBER", "FLDSECTIONNAME", "FLDREVISIONDETAILS", "FLDACTIVESTATUS" };
        string[] alCaptions = { "Section", "Name", "Revision", "Active Y/N" };

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

        if (General.GetNullableGuid(documentid) == null)
            documentid = ViewState["DOCUMENTID"].ToString();

        ds = PhoenixDocumentManagementDocumentSection.DocumentSectionSearch(
                                                                  PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableGuid(documentid)
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , General.ShowRecords(null)
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , null);

        Response.AddHeader("Content-Disposition", "attachment; filename=DocumentSection.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>" + ucTitle.Text + " - Sections" + "</h3></td>");
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

    protected void gvDocument_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();

    }

    protected void gvDocument_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvDocument.EditIndex = -1;
        gvDocument.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }    

    protected void gvDocument_RowCommand(object sender, GridViewCommandEventArgs e)
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
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidDocument(
                    ((TextBox)_gridView.FooterRow.FindControl("txtSectionNameAdd")).Text,        
                    ((TextBox)_gridView.FooterRow.FindControl("txtSectionNumberAdd")).Text,                   
                     ViewState["DOCUMENTID"].ToString())
                    )
                {
                    ucError.Visible = true;
                    return;
                }
                InsertDocumentSection(
                    ((TextBox)_gridView.FooterRow.FindControl("txtSectionNameAdd")).Text                   
                    , ViewState["DOCUMENTID"].ToString()                    
                    , (((CheckBox)_gridView.FooterRow.FindControl("chkActiveYNAdd")).Checked) ? 1 : 0
                    , ((TextBox)_gridView.FooterRow.FindControl("txtSectionNumberAdd")).Text
                    , ((TextBox)_gridView.FooterRow.FindControl("txtVersionNumberAdd")).Text
                );

                ucStatus.Text = "Section is added.";
                ViewState["SECTIONID"] = "";
                BindData();

                String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteDocumentSection((((Label)_gridView.Rows[nCurrentRow].FindControl("lblSectionId")).Text));
                ucStatus.Text = "Section is deleted.";
                ViewState["SECTIONID"] = "";                
                BindData();

                String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
            }
            else if (e.CommandName.ToUpper().Equals("EDITDOCUMENT"))
            {
                BindPageURL(nCurrentRow);
                Response.Redirect("../DocumentManagement/DocumentManagementDocumentSectionContentGeneral.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + ViewState["SECTIONID"].ToString());
            }
            else if (e.CommandName.ToUpper().Equals("VIEWDOCUMENT"))
            {
                BindPageURL(nCurrentRow);
                Response.Redirect("../DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + ViewState["SECTIONID"].ToString());
            }
            else if (e.CommandName.ToUpper().Equals("VIEWREVISON"))
            {
                BindPageURL(nCurrentRow);
                Response.Redirect("../DocumentManagement/DocumentManagementDocumentSectionRevisonList.aspx?call=treeview&DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + ViewState["SECTIONID"].ToString());
            }

            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvDocument_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvDocument, "Select$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void gvDocument_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvDocument_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
        ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtSectionNameEdit")).Focus();
        SetPageNavigator();

    }

    protected void gvDocument_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (!IsValidDocument(
                     ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSectionNameEdit")).Text,
                     ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSectionNumberEdit")).Text,
                     ViewState["DOCUMENTID"].ToString())
                )

            {
                ucError.Visible = true;
                return;
            }
            UpdateDocumentSection(
                  ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSectionNameEdit")).Text
                , ViewState["DOCUMENTID"].ToString()
                , ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSectionIdEdit")).Text
                , (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkActiveYNEdit")).Checked) ? 1 : 0
                , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSectionNumberEdit")).Text
             );

            ucStatus.Text = "Section details updated.";

            _gridView.EditIndex = -1;
            BindData();

            SetPageNavigator();

            String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDocument_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

            Label lblParentSectionYN = (Label)e.Row.FindControl("lblParentSectionYN");
            if (lblParentSectionYN != null && lblParentSectionYN.Text.ToString() == "1")
            {
                HtmlImage imgSection = (HtmlImage)e.Row.FindControl("imgSection");
                if (imgSection != null)
                    imgSection.Visible = true;
            }

            ImageButton cmdSubSection = (ImageButton)e.Row.FindControl("cmdSubSection");
            ImageButton cmgEditContent = (ImageButton)e.Row.FindControl("cmgEditContent");
            Label lblSectionId = (Label)e.Row.FindControl("lblSectionId");

            if (cmdSubSection != null && lblSectionId != null)
                cmdSubSection.Attributes.Add("onclick", "javascript:parent.Openpopup('Filter','','../DocumentManagement/DocumentManagementDocumentSubSectionAdd.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + lblSectionId.Text + "','medium'); return false;");

            if (cmgEditContent != null && lblSectionId != null)
                cmgEditContent.Attributes.Add("onclick", "javascript:parent.Openpopup('Filter','','../DocumentManagement/DocumentManagementDocumentSectionContentGeneral.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + lblSectionId.Text + "'); return false;");

            if (cmdSubSection != null && !SessionUtil.CanAccess(this.ViewState, cmdSubSection.CommandName))
                cmdSubSection.Visible = false;
            if (cmgEditContent != null && !SessionUtil.CanAccess(this.ViewState, cmgEditContent.CommandName))
                cmgEditContent.Visible = false;

            LinkButton lnkSectionName = (LinkButton)e.Row.FindControl("lnkSectionName");

            if (lnkSectionName != null)
                lnkSectionName.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp','','../DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + lblSectionId.Text + "'); return false;");

            DropDownList ddlRevisionStatusEdit = (DropDownList)e.Row.FindControl("ddlRevisionStatusEdit");
            DataRowView dv = (DataRowView)e.Row.DataItem;

            if (ddlRevisionStatusEdit != null)
            {
                if (dv["FLDREVISIONSTATUS"] != null && General.GetNullableInteger(dv["FLDREVISIONSTATUS"].ToString()) == 4)
                {
                    ddlRevisionStatusEdit.Visible = false;
                }
                else
                { 
                    ddlRevisionStatusEdit.SelectedValue = dv["FLDDRAFTREVISIONSTATUS"].ToString();
                }
            }
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

    protected void gvDocument_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        gvDocument.SelectedIndex = se.NewSelectedIndex;
        BindPageURL(se.NewSelectedIndex);
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["SECTIONID"] = ((Label)gvDocument.Rows[rowindex].FindControl("lblSectionId")).Text;
            //Filter.CurrentSelectedBulkOrderId = ((Label)gvDocument.Rows[rowindex].FindControl("lblOrderId")).Text;            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvDocument.EditIndex = -1;
        gvDocument.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvDocument.EditIndex = -1;
        gvDocument.SelectedIndex = -1;
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
        gvDocument.SelectedIndex = -1;
        gvDocument.EditIndex = -1;
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

    private void InsertDocumentSection(string sectionname, string documentid, int? activyn, string sectionnumber, string newrevisionnumber)
    {
        PhoenixDocumentManagementDocumentSection.DocumentSectionInsert(
              PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , sectionname
            , null
            , null
            , new Guid(documentid)            
            , activyn
            , sectionnumber
            , newrevisionnumber);
    }

    private void UpdateDocumentSection(string sectionname, string documentid, string sectionid, int? activyn, string sectionnumber)
    {

        //PhoenixDocumentManagementDocumentSection.DocumentSectionUpdate(
        //      PhoenixSecurityContext.CurrentSecurityContext.UserCode
        //    , sectionname            
        //    , new Guid(documentid)
        //    , new Guid(sectionid)            
        //    , activyn
        //    , sectionnumber            
        //    );
    }

    private void DeleteDocumentSection(string sectionid)
    {
        PhoenixDocumentManagementDocumentSection.DocumentSectionDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(sectionid));
    }

    private bool IsValidDocument(string sectionname, string sectionnumber, string documentid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(documentid) == null)
            ucError.ErrorMessage = "Document is not selected.";

        if (General.GetNullableString(sectionnumber) == null)
            ucError.ErrorMessage = "Section number is required."; 

        if (General.GetNullableString(sectionname) == null)
            ucError.ErrorMessage = "Section name is required.";       

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
    
    private void SetRowSelection()
    {
        gvDocument.SelectedIndex = -1;

        for (int i = 0; i < gvDocument.Rows.Count; i++)
        {
            if (gvDocument.DataKeys[i].Value.ToString().Equals(ViewState["SECTIONID"].ToString()))
            {
                gvDocument.SelectedIndex = i;
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvDocument.EditIndex = -1;
        gvDocument.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }    

}
