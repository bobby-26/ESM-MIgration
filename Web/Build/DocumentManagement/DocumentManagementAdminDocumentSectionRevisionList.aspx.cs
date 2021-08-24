using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using System.Collections.Specialized;
using System;
using Telerik.Web.UI;

public partial class DocumentManagementAdminDocumentSectionRevisionList : PhoenixBasePage
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
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementAdminDocumentSectionRevisionList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDocumentSectionRevison')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            if (Request.QueryString["DOCUMENTID"] != null && Request.QueryString["SECTIONID"] != null)
            {
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','DocumentManagement/DocumentManagementDocumentSectionRevisionEdit.aspx?documentid=" + Request.QueryString["DOCUMENTID"].ToString() + "&sectionid=" + Request.QueryString["SECTIONID"].ToString() + "'); return false;", "Modify", "<i class=\"fas fa-receipt\"></i>", "MODIFY");
            }
            else
            {
                toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementDocumentSectionRevisionEdit.aspx", "Modify", "<i class=\"fas fa-receipt\"></i>", "MODIFY");
            }
            //toolbar.AddImageLink("javascript:Openpopup('Filter','','DocumentManagementDocumentFilter.aspx'); return false;", "Filter", "search.png", "FIND");
            //toolbar.AddImageButton("../DocumentManagement/DocumentManagementDocumentSectionRevison.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            MenuRegistersCountry.AccessRights = this.ViewState;
            MenuRegistersCountry.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();

            toolbar.AddButton("Revision", "REVISION", ToolBarDirection.Right);
            toolbar.AddButton("Section", "SECTION", ToolBarDirection.Right);
            toolbar.AddButton("Document", "DOCUMENT", ToolBarDirection.Right);


            MenuDocument.AccessRights = this.ViewState;

            if (!IsPostBack)
            {
                //ucConfirm.Visible = false;
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
 
                //ifMoreInfo.Visible = false;

                gvDocumentSectionRevison.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            GetDocumentAndSectionDetails();

            MenuDocument.MenuList = toolbar.Show();
            MenuDocument.SelectedMenuIndex = 0;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    private void GetDocumentAndSectionDetails()
    {
        if (ViewState["SECTIONID"] != null && ViewState["SECTIONID"].ToString() != "")
        {
            DataSet ds = PhoenixDocumentManagementDocumentSection.DocumentSectionEdit(new Guid(ViewState["SECTIONID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                MenuDocument.Title = dr["FLDDOCUMENTNAME"].ToString();
                MenuDocument.Title = MenuDocument.Title + " /" + dr["FLDSECTIONDETAILS"].ToString();
            }
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDCREATEDDATE", "FLDAUTHORNAME", "FLDREVISIONDETAILS", "FLDAPPROVEDDATE", "FLDAPPROVEDBYNAME", "FLDDRAFTREVISION" };
        string[] alCaptions = { "Added Date", "Added By", "Revision", "Approved", "Approved By", "Draft" };

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
                                                                , iRowCount
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=DocumentSectionRevisons.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>" + MenuDocument.Title + " - Revisions" + "</h3></td>");
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

            if (CommandName.ToUpper().Equals("SECTION"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementAdminDocumentSectionList.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + ViewState["SECTIONID"].ToString());
            }
            else if (CommandName.ToUpper().Equals("DOCUMENT"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementAdminDocumentList.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + ViewState["SECTIONID"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvDocumentSectionRevison_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
        GetDocumentAndSectionDetails();
    }


    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCREATEDDATE", "FLDAUTHORNAME", "FLDREVISIONDETAILS", "FLDAPPROVEDDATE", "FLDAPPROVEDBYNAME", "FLDDRAFTREVISION" };
        string[] alCaptions = { "Added Date", "Added By", "Revision", "Approved", "Approved By", "Draft" };

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
                                                                , gvDocumentSectionRevison.CurrentPageIndex + 1
                                                                , gvDocumentSectionRevison.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        General.SetPrintOptions("gvDocumentSectionRevison", MenuDocument.Title + " - Revisions", alCaptions, alColumns, ds);
        gvDocumentSectionRevison.DataSource = ds;
        gvDocumentSectionRevison.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (General.GetNullableGuid(ViewState["REVISONID"].ToString()) == null)
            {
                ViewState["REVISONID"] = ds.Tables[0].Rows[0]["FLDREVISONID"].ToString();
                ViewState["DOCUMENTID"] = ds.Tables[0].Rows[0]["FLDDOCUMENTID"].ToString();
            }
        }

    }


    protected void gvDocumentSectionRevison_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

            if (e.CommandName.ToUpper().Equals("CHANGEPAGESIZE"))
                return;

            int nCurrentRow = e.Item.ItemIndex;

            if (e.CommandName.ToUpper().Equals("ROWCLICK") || e.CommandName.ToUpper().Equals("SELECT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                Guid documentid = new Guid(item.GetDataKeyValue("FLDSECTIONID").ToString());
                gvDocumentSectionRevison.SelectedIndexes.Add(e.Item.ItemIndex);
                BindPageURL(e.Item.ItemIndex);
            }

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                BindPageURL(nCurrentRow);
            }
            else if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                BindPageURL(nCurrentRow);

                RadLabel lblRevisionStatus = ((RadLabel)eeditedItem.FindControl("lblRevisionStatus"));

                //if (lblRevisionStatus.Text != "3")
                //{
                //    ucConfirm.Visible = true;
                //    ucConfirm.Text = "Are you sure you want to Approve this draft.? Y/N";
                //    return;
                //}
                PhoenixDocumentManagementDocumentSectionRevison.DocumentSectionRevisonApprove(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["SECTIONID"].ToString())
                    , new Guid(ViewState["DOCUMENTID"].ToString())
                    , new Guid(ViewState["REVISONID"].ToString())
                    );
                ucStatus.Text = "Revision is approved.";
                BindData();
                gvDocumentSectionRevison.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteDocumentSectionRevision((((RadLabel)eeditedItem.FindControl("lblRevisonId")).Text));
                ViewState["REVISONID"] = "";
                ucStatus.Text = "Section is deleted.";
                BindData();
                gvDocumentSectionRevison.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvDocumentSectionRevison_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        gvDocumentSectionRevison.Rebind();
    }

    protected void gvDocumentSectionRevison_ItemDataBound(Object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)item.DataItem;

            LinkButton del = (LinkButton)item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton db = (LinkButton)item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmTelerik(event); return false;");
            }


            RadLabel lblVersionNumber = (RadLabel)item.FindControl("lblVersionNumber");
            LinkButton cmgEditContent = (LinkButton)item.FindControl("cmgEditContent");
            LinkButton cmdApprove = (LinkButton)item.FindControl("cmdApprove");
            RadLabel lblSectionId = (RadLabel)item.FindControl("lblSectionId");
            RadLabel lblRevisonId = (RadLabel)item.FindControl("lblRevisonId");
            RadLabel lblDocumentId = (RadLabel)item.FindControl("lblDocumentId");
            RadLabel lblRevisionStatus = (RadLabel)item.FindControl("lblRevisionStatus");

            if (lblVersionNumber != null && lblVersionNumber.Text != null && General.GetNullableString(lblVersionNumber.Text) != null)
            {
                if (cmgEditContent != null)
                    cmgEditContent.Visible = false;
                if (cmdApprove != null)
                    cmdApprove.Visible = false;
            }

            if (cmgEditContent != null && lblSectionId != null && lblRevisionStatus.Text != "3")
                cmgEditContent.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filter','','DocumentManagement/DocumentManagementDocumentSectionContentGeneral.aspx?DOCUMENTID=" + lblDocumentId.Text + "&SECTIONID=" + lblSectionId.Text + "&REVISONID=" + lblRevisonId.Text + "'); return false;");

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

            //if (lblRevisionStatus.Text != "3")
            //{
            //    ucConfirm.Visible = true;
            //    ucConfirm.Text = "Are you sure you want to Approve this draft.? Y/N";
            //    return;
            //}

            if (cmdApprove != null)
            {
                if (lblRevisionStatus.Text != "3")
                {
                    cmdApprove.Attributes.Add("onclick", "return fnConfirmTelerik(event,'Are you sure you want to Approve this draft.? Y/N'); return false;");
                }
            }

            DataRowView dr = (DataRowView)item.DataItem;
            LinkButton lnkAddedDate = (LinkButton)item.FindControl("lnkAddedDate");

            if (lnkAddedDate != null)
                lnkAddedDate.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp','','DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + dr["FLDDOCUMENTID"].ToString() + "&SECTIONID=" + dr["FLDSECTIONID"].ToString() + "&REVISONID=" + dr["FLDREVISONID"].ToString() + "'); return false;");

        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {

            RadLabel lblRevisonId = ((RadLabel)gvDocumentSectionRevison.Items[rowindex].FindControl("lblRevisonId"));
            if (lblRevisonId != null)
                ViewState["REVISONID"] = lblRevisonId.Text;

            RadLabel lblDocumentId = ((RadLabel)gvDocumentSectionRevison.Items[rowindex].FindControl("lblDocumentId"));
            if (lblDocumentId != null)
                ViewState["DOCUMENTID"] = lblDocumentId.Text;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

 
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    private void SetRowSelection()
    {

        foreach (GridDataItem item in gvDocumentSectionRevison.Items)
        {

            if (item.GetDataKeyValue("FLDREVISONID").ToString().Equals(ViewState["REVISONID"].ToString()))
            {
                gvDocumentSectionRevison.SelectedIndexes.Add(item.ItemIndex);
            }
        }

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
    }

    //protected void ucConfirm_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

    //        if (ucCM.confirmboxvalue == 1)
    //        {
    //            PhoenixDocumentManagementDocumentSectionRevison.DocumentSectionRevisonApprove(
    //                PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                , new Guid(ViewState["SECTIONID"].ToString())
    //                , new Guid(ViewState["DOCUMENTID"].ToString())
    //                , new Guid(ViewState["REVISONID"].ToString())
    //                );
    //            ucStatus.Text = "Revision is approved.";
    //            BindData();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //        return;
    //    }
    //}
}
