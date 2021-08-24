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


public partial class DocumentManagementDocumentByCategoryListNew : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementDocumentByCategoryListNew.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDocumentByCategory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            MenuDocumentByCategory.AccessRights = this.ViewState;
            MenuDocumentByCategory.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            MenuDocument.AccessRights = this.ViewState;


            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["FILEID"] = "";

                if (Request.QueryString["CATEGORYID"] != null && Request.QueryString["CATEGORYID"].ToString() != "")
                    ViewState["CATEGORYID"] = Request.QueryString["CATEGORYID"].ToString();
                else
                    ViewState["CATEGORYID"] = "";

                GetCategoryName();
                GetDocumentDetails();

                gvDocumentByCategory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();

            MenuDocument.MenuList = toolbar1.Show();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void MenuDocumentByCategory_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
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

    private void GetCategoryName()
    {
        if (ViewState["CATEGORYID"] != null && General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) != null)
        {
            DataSet ds = PhoenixDocumentManagementCategory.DocumentCategoryEdit(new Guid(ViewState["CATEGORYID"].ToString()));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                MenuDocument.Title = MenuDocument.Title + " - " + ds.Tables[0].Rows[0]["FLDCATEGORYNUMBER"].ToString() + " " + ds.Tables[0].Rows[0]["FLDCATEGORYNAME"].ToString();
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
                gvDocumentByCategory.CurrentPageIndex = 0;
                gvDocumentByCategory.Rebind();
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

    protected void gvDocumentByCategory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }


    private void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCATEGORYNAME", "FLDFILENAME", "FLDREVISIONNUMBER", "FLDPURPOSE" };
        string[] alCaptions = { "Category", "File Name", "Revision", "Remarks" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixDocumentManagementDocument.ListDocumentByCategory(
                                                                 PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableGuid(ViewState["CATEGORYID"].ToString())
                                                                , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                , sortexpression
                                                                , sortdirection
                                                                , gvDocumentByCategory.CurrentPageIndex + 1
                                                                , gvDocumentByCategory.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);



        DataTable dt = ds.Tables[0];

        Response.AddHeader("Content-Disposition", "attachment; filename=DocumentByCategory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Document By Category</h3></td>");
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCATEGORYNAME", "FLDFILENAME", "FLDREVISIONNUMBER", "FLDPURPOSE" };
        string[] alCaptions = { "Category", "File Name", "Revision", "Remarks" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixDocumentManagementDocument.ListDocumentByCategory(
                                                                 PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableGuid(ViewState["CATEGORYID"].ToString())
                                                                , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                , sortexpression
                                                                , sortdirection
                                                                , gvDocumentByCategory.CurrentPageIndex + 1
                                                                , gvDocumentByCategory.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        General.SetPrintOptions("gvDocumentByCategory", MenuDocument.Title, alCaptions, alColumns, ds);

        gvDocumentByCategory.DataSource = ds;
        gvDocumentByCategory.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (General.GetNullableGuid(ViewState["FILEID"].ToString()) == null)
            {
                ViewState["FILEID"] = ds.Tables[0].Rows[0]["FLDFILEID"].ToString();
            }
            SetRowSelection();
        }
    }


    protected void gvDocumentByCategory_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

            int nCurrentRow = e.Item.ItemIndex;

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                BindPageURL(nCurrentRow);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvDocumentByCategory_ItemDataBound(Object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)item.DataItem;

            ImageButton del = (ImageButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            RadLabel lblType = (RadLabel)e.Item.FindControl("lblType");
            LinkButton cmdDoc = (LinkButton)e.Item.FindControl("cmdDocuments");
            LinkButton lnkReadUnread = (LinkButton)e.Item.FindControl("lnkReadUnread");
            LinkButton cmdForms = (LinkButton)e.Item.FindControl("cmdForms");
            LinkButton lnkFileName = (LinkButton)e.Item.FindControl("lnkFileName");

            RadLabel lblFileId = (RadLabel)e.Item.FindControl("lblFileId");

            if (lblType != null)
            {
                if (lblType.Text == "2")
                {
                    //Document
                    cmdDoc.Visible = false;
                    cmdForms.Visible = true;
                    lnkReadUnread.Visible = true;
                    lnkFileName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentGeneralNew.aspx?showall=0&DOCUMENTID=" + lblFileId.Text + "');return false;");
                }
                else if (lblType.Text == "3")
                {
                    //Document Section
                    cmdDoc.Visible = false;
                    cmdForms.Visible = true;
                    lnkReadUnread.Visible = true;
                    lnkFileName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionNewContentView.aspx?callfrom=documenttree&SECTIONID=" + lblFileId.Text + "');return false;");
                }
                else if (lblType.Text == "5" || lblType.Text == "6")
                {
                    //Forms
                    cmdDoc.Visible = true;
                    cmdForms.Visible = false;
                    lnkReadUnread.Visible = false;

                    if (lblType.Text == "5")
                    {
                        lnkFileName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + lblFileId.Text + "&FORMREVISIONID=" + drv["FLDFORMREVISIONID"].ToString() + "');return false;");
                    }
                    else
                    {
                        lnkFileName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Common/download.aspx?formid=" + lblFileId.Text + "');return false;");
                    }

                }
                else
                {
                    cmdDoc.Visible = false;
                    cmdForms.Visible = false;
                    lnkReadUnread.Visible = false;
                }
            }

            SectionMergeRows(gvDocumentByCategory);

            RadLabel lblDocumentId = (RadLabel)e.Item.FindControl("lblFileId");
            LinkButton lnkDocumentName = (LinkButton)e.Item.FindControl("lnkFileName");
            if (lnkReadUnread != null)
            {
                SessionUtil.CanAccess(this.ViewState, lnkReadUnread.CommandName);
                lnkReadUnread.Attributes.Add("onclick", "openNewWindow('ReadUnread', '" + lnkDocumentName.Text + " - Read/Unread User List', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionReadUnreadList.aspx?DOCUMENTID=" + lblDocumentId.Text + "');return false;");
            }
            RadLabel lblFormid = (RadLabel)e.Item.FindControl("lblFileId");

            if (cmdDoc != null)
            {
                cmdDoc.Attributes.Add("onclick", "javascript:return openNewWindow('Document', '', 'DocumentManagement/DocumentManagementDocumentLinked.aspx?FORMID=" + lblFormid.Text + "',false,550,300); return false;");
            }

            RadLabel lblSectionid = (RadLabel)e.Item.FindControl("lblFileId");
            if (cmdForms != null)
            {
                SessionUtil.CanAccess(this.ViewState, cmdForms.CommandName);
                if (lblType.Text == "2")
                {
                    cmdForms.Attributes.Add("onclick", "openNewWindow('Forms', 'Forms and Checklist', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentLinkedForms.aspx?DOCUMENTID=" + lblDocumentId.Text + "',false,500,300);return false;");
                }
                else
                {
                    cmdForms.Attributes.Add("onclick", "openNewWindow('Forms', 'Forms and Checklist', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementLinkedForms.aspx?sectionid=" + lblSectionid.Text + "',false,500,300);return false;");
                }
            }
        }
        if (e.Item is GridFooterItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
    protected void gvDocumentByCategory_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        BindPageURL(se.NewSelectedIndex);
    }

    public static void SectionMergeRows(RadGrid gridView)
    {
        for (int rowIndex = gridView.Items.Count - 2; rowIndex >= 0; rowIndex--)
        {
            GridDataItem row = gridView.Items[rowIndex];
            GridDataItem previousRow = gridView.Items[rowIndex + 1];

            string currentCategoryName = ((RadLabel)gridView.Items[rowIndex].FindControl("lblcategoryName")).Text;
            string previousCategoryName = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblcategoryName")).Text;

            string currentDocumentName = ((LinkButton)gridView.Items[rowIndex].FindControl("lnkFileName")).Text;
            string previousDocumentName = ((LinkButton)gridView.Items[rowIndex + 1].FindControl("lnkFileName")).Text;

            if (currentCategoryName == previousCategoryName && currentDocumentName != previousDocumentName)
            {
                row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                    previousRow.Cells[1].RowSpan + 1;
                previousRow.Cells[1].Visible = false;
            }
            else if (currentCategoryName == previousCategoryName && currentDocumentName == previousDocumentName)
            {
                row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                                           previousRow.Cells[1].RowSpan + 1;
                previousRow.Cells[1].Visible = false;

                row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                     previousRow.Cells[2].RowSpan + 1;
                previousRow.Cells[2].Visible = false;
            }
            else if (currentCategoryName == previousCategoryName && currentDocumentName == previousDocumentName)
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

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["FILEID"] = ((RadLabel)gvDocumentByCategory.Items[rowindex].FindControl("lblFileId")).Text;
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
        foreach (GridDataItem item in gvDocumentByCategory.Items)
        {

            if (item.GetDataKeyValue("FLDFILEID").ToString().Equals(ViewState["FILEID"].ToString()))
            {
                gvDocumentByCategory.SelectedIndexes.Add(item.ItemIndex);
                //ViewState["DOCUMENTID"] = item.GetDataKeyValue("FLDDOCUMENTID").ToString();
            }
        }
    }

    protected string GetParentIframeURL(string referenceid)
    {
        string strURL = "";
        int type = 0;
        DataSet ds = PhoenixDocumentManagementDocument.GetNewSelectedeTreeNodeType(General.GetNullableGuid(referenceid), ref type);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            strURL = dr["FLDURL"].ToString();
        }

        return strURL;
    }
}
