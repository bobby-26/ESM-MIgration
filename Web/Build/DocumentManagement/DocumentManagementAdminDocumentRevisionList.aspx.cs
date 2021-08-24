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

public partial class DocumentManagementAdminDocumentRevisionList : PhoenixBasePage
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
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementAdminDocumentRevisionList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDocumentRevision')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            MenuDocumentRevision.AccessRights = this.ViewState;
            MenuDocumentRevision.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Section", "SECTION", ToolBarDirection.Right);
            toolbar.AddButton("Revision", "REVISION", ToolBarDirection.Right);

            MenuDocument.AccessRights = this.ViewState;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["REVISIONID"] = "";

                if (Request.QueryString["DOCUMENTID"] != null && Request.QueryString["DOCUMENTID"].ToString() != "")
                    ViewState["DOCUMENTID"] = Request.QueryString["DOCUMENTID"].ToString();
                else
                    ViewState["DOCUMENTID"] = "";

                gvDocumentRevision.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                GetDocumentDetails();
            }

            MenuDocument.MenuList = toolbar.Show();
            MenuDocument.SelectedMenuIndex = 1;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    private void GetDocumentDetails()
    {
        if (ViewState["DOCUMENTID"] != null && ViewState["DOCUMENTID"].ToString() != "")
        {
            DataSet ds = PhoenixDocumentManagementDocument.DocumentEdit(
                                                             new Guid(ViewState["DOCUMENTID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                MenuDocument.Title = dr["FLDDOCUMENTNAME"].ToString() + " - " + "Revisions";
            }
        }
    }

    protected void MenuDocumentRevision_TabStripCommand(object sender, EventArgs e)
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
                Response.Redirect("../DocumentManagement/DocumentManagementAdminDocumentRevisionSectionList.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"].ToString() + "&REVISIONID=" + ViewState["REVISIONID"].ToString());
            }
            else if (CommandName.ToUpper().Equals("FORM"))
            {
                //Response.Redirect("../DocumentManagement/DocumentManagementFormList.aspx?FORMID=" + ViewState["FORMID"] + "&CATEGORYID=" + ViewState["CATEGORYID"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvDocumentRevision_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
        GetDocumentDetails();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDREVISIONDETAILS", "FLDPUBLISHEDDATE", "FLDPUBLISHEDBYNAME" };
        string[] alCaptions = { "Revision", "Published", "Published By" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixDocumentManagementDocument.DocumentRevisionSearch(
                                                                 PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableGuid(ViewState["DOCUMENTID"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , gvDocumentRevision.CurrentPageIndex + 1
                                                                , gvDocumentRevision.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        General.SetPrintOptions("gvDocumentRevision", MenuDocument.Title, alCaptions, alColumns, ds);
        gvDocumentRevision.DataSource = ds;
        gvDocumentRevision.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (General.GetNullableGuid(ViewState["REVISIONID"].ToString()) == null)
            {
                ViewState["REVISIONID"] = ds.Tables[0].Rows[0]["FLDREVISIONID"].ToString();
            }
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDREVISIONDETAILS", "FLDPUBLISHEDDATE", "FLDPUBLISHEDBYNAME" };
        string[] alCaptions = { "Revision", "Published", "Published By" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixDocumentManagementDocument.DocumentRevisionSearch(
                                                                 PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableGuid(ViewState["DOCUMENTID"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , gvDocumentRevision.CurrentPageIndex + 1
                                                                , gvDocumentRevision.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=DocumentRevisions.xls");
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

    protected void gvDocumentRevision_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

            if (e.CommandName.ToUpper().Equals("CHANGEPAGESIZE"))
                return;

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                BindPageURL(e.Item.ItemIndex);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvDocumentRevision_ItemDataBound(Object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {

            GridDataItem item = (GridDataItem)e.Item;
            DataRowView dr = (DataRowView)item.DataItem;

            LinkButton hlnkAddedDate = (LinkButton)item.FindControl("hlnkAddedDate");

            if (hlnkAddedDate != null)
            {
                if (ViewState["FORMTYPE"] != null && General.GetNullableInteger(ViewState["FORMTYPE"].ToString()) == 0)
                {
                    hlnkAddedDate.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp','','../DocumentManagement/DocumentManagementFormPreview.aspx?FORMID=" + dr["FLDFORMID"].ToString() + "&FORMREVISIONID=" + dr["FLDFORMREVISIONID"].ToString() + "'); return false;");
                }
                else
                {
                    if (dr["FLDDTKEY"] != null && dr["FLDDTKEY"].ToString() != "")
                    {
                        DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(dr["FLDDTKEY"].ToString()));
                        if (dt.Rows.Count > 0)
                        {
                            DataRow drRow = dt.Rows[0];
                            // hlnkAddedDate.NavigateUrl = Session["sitepath"] + "/attachments/" + drRow["FLDFILEPATH"].ToString();
                        }
                    }
                }
            }
        }
        if (e.Item is GridFooterItem)
        {
            GridFooterItem item = (GridFooterItem)e.Item;
            ImageButton db = (ImageButton)item.FindControl("cmdAdd");
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
            ViewState["REVISIONID"] = ((RadLabel)gvDocumentRevision.Items[rowindex].FindControl("lblRevisionId")).Text;
            SetRowSelection();
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
        foreach (GridDataItem item in gvDocumentRevision.Items)
        {

            if (item.GetDataKeyValue("FLDREVISIONID").ToString().Equals(ViewState["REVISIONID"].ToString()))
            {
                gvDocumentRevision.SelectedIndexes.Add(item.ItemIndex);
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
    }

}
