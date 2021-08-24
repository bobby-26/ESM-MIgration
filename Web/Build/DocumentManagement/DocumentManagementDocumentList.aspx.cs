using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using System.Collections.Specialized;

public partial class DocumentManagementDocumentList : PhoenixBasePage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../DocumentManagement/DocumentManagementDocumentList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvDocument')", "Print Grid", "icon_print.png", "PRINT");            
            //toolbar.AddImageLink("javascript:Openpopup('Filter','','DocumentManagementDocumentFilter.aspx'); return false;", "Filter", "search.png", "FIND");
            //toolbar.AddImageButton("../DocumentManagement/DocumentManagementDocumentList.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            MenuRegistersCountry.AccessRights = this.ViewState;
            MenuRegistersCountry.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                toolbar = new PhoenixToolbar();
                toolbar.AddButton("Document", "DOCUMENT");
                toolbar.AddButton("Section", "SECTION");
                //toolbar.AddButton("Content", "CONTENT");                

                //MenuDocument.AccessRights = this.ViewState;
                //MenuDocument.MenuList = toolbar.Show();
                //MenuDocument.SelectedMenuIndex = 0;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["DOCUMENTID"] = "";

                if (Request.QueryString["DOCUMENTID"] != null && Request.QueryString["DOCUMENTID"].ToString() != "")
                    ViewState["DOCUMENTID"] = Request.QueryString["DOCUMENTID"].ToString();
                else
                    ViewState["DOCUMENTID"] = "";

                if (Request.QueryString["CATEGORYID"] != null && Request.QueryString["CATEGORYID"].ToString() != "")
                    ViewState["CATEGORYID"] = Request.QueryString["CATEGORYID"].ToString();
                else
                    ViewState["CATEGORYID"] = "";

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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        //int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSERIALNUMBER", "FLDDOCUMENTNAME", "FLDCATEGORYNAME", "FLDACTIVESTATUS", "FLDDATE", "FLDAUTHORNAME", "FLDREVISIONDETAILS", "FLDPUBLISHEDYN" };
        string[] alCaptions = { "S.No", "Name", "Category", "Active Y/N", "Added Date", "Added By", "Revision", "PublishedYN" };

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

        if (General.GetNullableGuid(documentid) != null)
            ViewState["DOCUMENTID"] = "";

        //ds = PhoenixDocumentManagementDocument.DocumentSearch(
        //                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
        //                                                        , General.GetNullableGuid(documentid)
        //                                                        , null
        //                                                        , null
        //                                                        , null
        //                                                        , General.GetNullableGuid(ViewState["CATEGORYID"].ToString())
        //                                                        , sortexpression
        //                                                        , sortdirection
        //                                                        , (int)ViewState["PAGENUMBER"]
        //                                                        , iRowCount
        //                                                        , ref iRowCount
        //                                                        , ref iTotalPageCount);    

        Response.AddHeader("Content-Disposition", "attachment; filename=Documents.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Documents</h3></td>");
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
            if (dce.CommandName.ToUpper().Equals("SECTION"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementDocumentSectionList.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"]);
            }
            else if (dce.CommandName.ToUpper().Equals("CONTENT"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"]);
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

        string[] alColumns = { "FLDSERIALNUMBER", "FLDDOCUMENTNAME", "FLDCATEGORYNAME", "FLDACTIVESTATUS", "FLDDATE", "FLDAUTHORNAME", "FLDREVISIONDETAILS", "FLDPUBLISHEDYN" };
        string[] alCaptions = { "S.No", "Name", "Category", "Active Y/N", "Added Date", "Added By", "Revision", "PublishedYN" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
       
        NameValueCollection nvc = Filter.CurrentDMSDocumentFilter;

        string documentid = (nvc != null && nvc.Get("ddlDocument") != null) ? nvc.Get("ddlDocument").ToString() : "";

        if (General.GetNullableGuid(documentid) != null)
            ViewState["DOCUMENTID"] = "";

        //ds = PhoenixDocumentManagementDocument.DocumentSearch(
        //                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
        //                                                        , General.GetNullableGuid(documentid)
        //                                                        , null
        //                                                        , null
        //                                                        , null
        //                                                        , General.GetNullableGuid(ViewState["CATEGORYID"].ToString())
        //                                                        , sortexpression
        //                                                        , sortdirection
        //                                                        , (int)ViewState["PAGENUMBER"]
        //                                                        , General.ShowRecords(null)
        //                                                        , ref iRowCount
        //                                                        , ref iTotalPageCount);     
      
        General.SetPrintOptions("gvDocument", "Documents", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDocument.DataSource = ds;
            gvDocument.DataBind();
            if (General.GetNullableGuid(ViewState["DOCUMENTID"].ToString()) == null)
            {
                ViewState["DOCUMENTID"] = ds.Tables[0].Rows[0]["FLDDOCUMENTID"].ToString();
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
            else  if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                BindPageURL(nCurrentRow);                
            }
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidDocument(
                    ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtSequenceNumberAdd")).Text
                     , ((TextBox)_gridView.FooterRow.FindControl("txtDocumentNameAdd")).Text
                     , ((TextBox)_gridView.FooterRow.FindControl("txtCategoryidAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertDocument(
                    ((TextBox)_gridView.FooterRow.FindControl("txtDocumentNameAdd")).Text
                    , ((TextBox)_gridView.FooterRow.FindControl("txtCategoryidAdd")).Text
                    , (((CheckBox)_gridView.FooterRow.FindControl("chkActiveYNAdd")).Checked) ? 1 : 0
                    , 0
                    , int.Parse(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtSequenceNumberAdd")).Text)
                );
                ucStatus.Text = "Document is added.";
                ViewState["DOCUMENTID"] = "";  
                BindData();

                String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
            }
            else if (e.CommandName.ToUpper().Equals("APPROVE"))
            {

                Label lblDocumentId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDocumentId"));
                Label lblRevisionId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRevisionId"));
                Guid? revisionid = General.GetNullableGuid(lblRevisionId.Text);
                Guid? newrevisionid = null;
                string strRevisionId = "";

                PhoenixDocumentManagementDocument.DocumentRevisionInsert(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(lblDocumentId.Text)
                    , revisionid
                    , ref newrevisionid
                    );

                if (revisionid != null)
                    strRevisionId = revisionid.ToString();

                if (newrevisionid != null)
                    strRevisionId = newrevisionid.ToString();

                if (General.GetNullableGuid(strRevisionId) != null)
                {
                    //PhoenixDocumentManagementDocument.DocumentRevisionApprove(
                    //    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    //    , new Guid(lblDocumentId.Text)
                    //    , new Guid(strRevisionId)
                    //    );
                }
                ucStatus.Text = "Document is approved and published.";
                BindData();

                String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteDocument((((Label)_gridView.Rows[nCurrentRow].FindControl("lblDocumentId")).Text));
                ViewState["DOCUMENTID"] = "";
                ucStatus.Text = "Document is deleted.";
                BindData();

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

    protected void gvDocument_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvDocument_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvDocument_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (!IsValidDocument(
                      ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtSequenceNumberEdit")).Text,
                      ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDocumentNameEdit")).Text,
                      ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCategoryidEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }
            UpdateDocument(
                  ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDocumentNameEdit")).Text
                , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCategoryidEdit")).Text
                , (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkActiveYNEdit")).Checked) ? 1 : 0
                , ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDocumentIdEdit")).Text
                , 0
                , int.Parse(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtSequenceNumberEdit")).Text)
             );

                ucStatus.Text = "Document details updated.";
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

            DropDownList ddlDocumentCategoryEdit = (DropDownList)e.Row.FindControl("ddlDocumentCategoryEdit");
            DataRowView dv = (DataRowView)e.Row.DataItem;
            if (ddlDocumentCategoryEdit != null)
            {
                ddlDocumentCategoryEdit.Items.Clear();
                ddlDocumentCategoryEdit.DataSource = PhoenixDocumentManagementCategory.ListDocumentCategory();
                ddlDocumentCategoryEdit.DataTextField = "FLDCATEGORYNAME";
                ddlDocumentCategoryEdit.DataValueField = "FLDCATEGORYID";
                ddlDocumentCategoryEdit.DataBind();

                if (dv != null)
                    ddlDocumentCategoryEdit.SelectedValue = dv["FLDCATEGORYID"].ToString();
            }

            Label lblDocumentId = (Label)e.Row.FindControl("lblDocumentId");

            ImageButton cmgViewContent = (ImageButton)e.Row.FindControl("cmgViewContent");
            ImageButton cmdApprove = (ImageButton)e.Row.FindControl("cmdApprove");
            ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
            ImageButton cmdDelete = (ImageButton)e.Row.FindControl("cmdDelete");

            if (cmgViewContent != null && lblDocumentId != null)
                cmgViewContent.Attributes.Add("onclick", "javascript:parent.Openpopup('Filter','','../DocumentManagement/DocumentManagementDocumentGeneral.aspx?DOCUMENTID=" + lblDocumentId.Text + "'); return false;");           
            
            if (cmgViewContent != null && !SessionUtil.CanAccess(this.ViewState, cmgViewContent.CommandName))
                cmgViewContent.Visible = false;
            if (cmdApprove != null && !SessionUtil.CanAccess(this.ViewState, cmdApprove.CommandName))
                cmdApprove.Visible = false;
            if (cmdEdit != null && !SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName))
                cmdEdit.Visible = false;
            if (cmdDelete != null && !SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName))
                cmdDelete.Visible = false;

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            DropDownList ddlDocumentCategoryAdd = (DropDownList)e.Row.FindControl("ddlDocumentCategoryAdd");
            if (ddlDocumentCategoryAdd != null)
            {
                ddlDocumentCategoryAdd.DataSource = PhoenixDocumentManagementCategory.ListDocumentCategory();
                ddlDocumentCategoryAdd.DataTextField = "FLDCATEGORYNAME";
                ddlDocumentCategoryAdd.DataValueField = "FLDCATEGORYID";
                ddlDocumentCategoryAdd.DataBind();
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
            ViewState["DOCUMENTID"] = ((Label)gvDocument.Rows[rowindex].FindControl("lblDocumentId")).Text;
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

    private void InsertDocument(string documentname, string categorid, int? activyn, int? documenttype, int serialnumber)
    {
        //PhoenixDocumentManagementDocument.DocumentInsert(
        //    PhoenixSecurityContext.CurrentSecurityContext.UserCode
        //    , documentname
        //    , new Guid(categorid)
        //    , activyn
        //    , documenttype
        //    , serialnumber);
    }

    private void UpdateDocument(string documentname, string categorid, int? activyn, string documentid, int? documenttype, int serialnumber)
    {

        //PhoenixDocumentManagementDocument.DocumentUpdate(
        //    PhoenixSecurityContext.CurrentSecurityContext.UserCode
        //    , documentname
        //    , new Guid(categorid)
        //    , activyn
        //    , new Guid(documentid)
        //    , documenttype
        //    , serialnumber);
    }

    private void DeleteDocument(string documentid)
    {
        PhoenixDocumentManagementDocument.DocumentDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(documentid));
    }

    private bool IsValidDocument(string serialnumber, string documentname, string categoryid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(serialnumber) == null)
            ucError.ErrorMessage = "Serial Number is required.";

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
        gvDocument.SelectedIndex = -1;

        for (int i = 0; i < gvDocument.Rows.Count; i++) 
        {
            if (gvDocument.DataKeys[i].Value.ToString().Equals(ViewState["DOCUMENTID"].ToString()))
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
}
