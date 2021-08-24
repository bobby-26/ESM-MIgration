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

public partial class DocumentManagementDocumentByCategoryList : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvDocumentByCategory.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvDocumentByCategory.UniqueID, "Select$" + r.RowIndex.ToString());
    //        }
    //    }
    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementDocumentByCategoryList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
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
                //ShowExcel();
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
                                                                ,PhoenixSecurityContext.CurrentSecurityContext.CompanyID
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

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                int nCurrentRow = e.Item.ItemIndex;
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

            SectionMergeRows(gvDocumentByCategory);

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
        DataSet ds = PhoenixDocumentManagementDocument.GetSelectedeTreeNodeType(General.GetNullableGuid(referenceid), ref type);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            strURL = dr["FLDURL"].ToString();
        }

        return strURL;
    }
}
