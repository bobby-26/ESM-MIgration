using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegisterFMSDrawingCategory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegisterFMSDrawingCategory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvFMSCategory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegisterFMSDrawingCategory.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegisterFMSDrawingCategory.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegisterFMSDrawingCategoryAdd.aspx?" + "')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            
            MenuMOCCategory.AccessRights = this.ViewState;
            MenuMOCCategory.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvFMSCategory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDDRAWINGCODE", "FLDFMSDRAWINGCATEGORY" };
        string[] alCaptions = { "Code", "Category" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegisterFMSDrawing.FMSDrawingCategorySearch(General.GetNullableString(txtShortCode.Text)
                                                                    , General.GetNullableString(txtCategory.Text)
                                                                    , sortexpression, sortdirection
                                                                    , 1
                                                                    , iRowCount
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=MOCCategory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='3' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>MOC Category</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='3' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='100%'>");
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

    protected void MenuMOCCategory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {

                ViewState["PAGENUMBER"] = 1;
                gvFMSCategory.CurrentPageIndex = 0;
                BindData();
                gvFMSCategory.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                txtShortCode.Text = "";
                txtCategory.Text = "";
                BindData();
                gvFMSCategory.Rebind();

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
        string[] alColumns = { "FLDDRAWINGCODE", "FLDDRAWINGCATEGORY" };
        string[] alCaptions = { "Code", "Category" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixRegisterFMSDrawing.FMSDrawingCategorySearch(General.GetNullableString(txtShortCode.Text)
                                                                    , General.GetNullableString(txtCategory.Text)
                                                                    , sortexpression, sortdirection
                                                                    , (int)ViewState["PAGENUMBER"]
                                                                    , gvFMSCategory.PageSize
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount);

        General.SetPrintOptions("gvFMSCategory", "FMS Drawing Category", alCaptions, alColumns, ds);


        gvFMSCategory.DataSource = ds;
        gvFMSCategory.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvFMSCategory_Sorting(object sender, GridSortCommandEventArgs se)
    {

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }


    protected void gvFMSCategory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegisterFMSDrawing.FMSDrawingCategoryDelete(new Guid(((RadLabel)e.Item.FindControl("lblCategoryId")).Text.ToString()));
                BindData();
                gvFMSCategory.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("PAGE"))
            {
                ViewState["PAGENUMBER"] = null;
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvFMSCategory.Rebind();

    }

    protected void gvFMSCategory_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);


            if (del != null)
            {
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            if (eb != null)
            {
                eb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegisterFMSDrawingCategoryAdd.aspx?categoryid=" + (((RadLabel)e.Item.FindControl("lblCategoryId")).Text.ToString()) + "');return false;");  
            }
        }
    }
    private bool IsValidMOCCategory(string shortcode, string category)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (shortcode.Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (category.Trim().Equals(""))
            ucError.ErrorMessage = "Category is required.";

        return (!ucError.IsError);
    }

    //private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    //{
    //    int nextRow = 0;
    //    GridView _gridView = (GridView)sender;

    //    if (e.Row.RowType == DataControlRowType.DataRow
    //        && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
    //        || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
    //    {
    //        int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

    //        String script = "var keyValue = SelectSibling(event); ";
    //        script += " if(keyValue == 38) {";  //Up Arrow
    //        nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

    //        script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
    //        script += "}";

    //        script += " if(keyValue == 40) {";  //Down Arrow
    //        nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

    //        script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
    //        script += "}";
    //        script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
    //        e.Row.Attributes["onkeydown"] = script;
    //    }

    //}

    protected void gvFMSCategory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFMSCategory.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvFMSCategory.Rebind();
    }
}