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

public partial class DocumentManagementAdminDocumentRevisionSectionList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementAdminDocumentRevisionSectionList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
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
                ViewState["DOCUMENTREVISIONID"] = "";

                if (Request.QueryString["DOCUMENTID"] != null && Request.QueryString["DOCUMENTID"].ToString() != "")
                    ViewState["DOCUMENTID"] = Request.QueryString["DOCUMENTID"].ToString();
                else
                    ViewState["DOCUMENTID"] = "";

                if (Request.QueryString["REVISIONID"] != null && Request.QueryString["REVISIONID"].ToString() != "")
                    ViewState["DOCUMENTREVISIONID"] = Request.QueryString["REVISIONID"].ToString();
                else
                    ViewState["DOCUMENTREVISIONID"] = "";

                gvDocumentRevision.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                GetDocumentDetails();
            }

            MenuDocument.MenuList = toolbar.Show();
            MenuDocument.SelectedMenuIndex = 0;


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
                MenuDocument.Title = dr["FLDDOCUMENTNAME"].ToString() + " - " + "Section"; 
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

            if (CommandName.ToUpper().Equals("REVISION"))
            {                
                Response.Redirect("../DocumentManagement/DocumentManagementAdminDocumentRevisionList.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"].ToString() + "&REVISIONID=" + ViewState["DOCUMENTREVISIONID"].ToString());
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
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSECTIONDETAILS", "FLDSECTIONREVISIONDETAILS", "FLDAPPROVEDDATE", "FLDAPPROVEDBYNAME" };
        string[] alCaptions = { "Section", "Revision", "Approved Date", "Approved By" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixDocumentManagementDocument.DocumentRevisionSectionList(
                                                                 PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableGuid(ViewState["DOCUMENTID"].ToString())
                                                                , General.GetNullableGuid(ViewState["DOCUMENTREVISIONID"].ToString())
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
            txtDocumentRevision.Text = ds.Tables[0].Rows[0]["FLDDOCUMENTREVISIONDETAILS"].ToString();
            txtPublishedDate.Text = ds.Tables[0].Rows[0]["FLDPUBLISHEDDATE"].ToString();
            txtPublishedBy.Text = ds.Tables[0].Rows[0]["FLDPUBLISHEDBYNAME"].ToString();
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSECTIONDETAILS", "FLDSECTIONREVISIONDETAILS", "FLDAPPROVEDDATE", "FLDAPPROVEDBYNAME" };
        string[] alCaptions = { "Section", "Revision", "Approved Date", "Approved By" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixDocumentManagementDocument.DocumentRevisionSectionList(
                                                                 PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableGuid(ViewState["DOCUMENTID"].ToString())
                                                                , General.GetNullableGuid(ViewState["DOCUMENTREVISIONID"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , gvDocumentRevision.CurrentPageIndex + 1
                                                                , gvDocumentRevision.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=SectionRevisions.xls");
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

            //GridDataItem item = (GridDataItem)e.Item;
            //HyperLink hlnkAddedDate = (HyperLink)item.FindControl("hlnkAddedDate");

            //if (hlnkAddedDate != null)
            //{
            //    if (ViewState["FORMTYPE"] != null && General.GetNullableInteger(ViewState["FORMTYPE"].ToString()) == 0)
            //    {
            //        hlnkAddedDate.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp','','../DocumentManagement/DocumentManagementFormPreview.aspx?FORMID=" + dr["FLDFORMID"].ToString() + "&FORMREVISIONID=" + dr["FLDFORMREVISIONID"].ToString() + "'); return false;");
            //    }
            //    else
            //    {
            //        if (dr["FLDDTKEY"] != null && dr["FLDDTKEY"].ToString() != "")
            //        {
            //            DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(dr["FLDDTKEY"].ToString()));
            //            if (dt.Rows.Count > 0)
            //            {
            //                DataRow drRow = dt.Rows[0];
            //                hlnkAddedDate.NavigateUrl = Session["sitepath"] + "/attachments/" + drRow["FLDFILEPATH"].ToString();
            //            }
            //        }
            //    }
            //}
    }

    protected void gvDocumentRevision_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {

        BindPageURL(se.NewSelectedIndex);
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["REVISIONID"] = ((RadLabel)gvDocumentRevision.Items[rowindex].FindControl("lblRevisionId")).Text;
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
    }
}
