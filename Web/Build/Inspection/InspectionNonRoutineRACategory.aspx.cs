using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionNonRoutineRACategory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionNonRoutineRACategory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvNonRoutineRACategory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionNonRoutineRACategory.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionNonRoutineRACategory.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionNonRoutineRACategoryAdd.aspx')", "Add New", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuNonRoutineRACategory.AccessRights = this.ViewState;
            MenuNonRoutineRACategory.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvNonRoutineRACategory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                
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

        string[] alColumns = {  "FLDSHORTCODE","FLDNAME", "FLDACTIVE" };
        string[] alCaptions = {  "Code", "Name","Active Y/N" };
        string sortexpression;
        int? sortdirection = null;
        DataSet ds = new DataSet();
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

         ds = PhoenixInspectionNonRoutineRACategory.NonRoutineRACategorySearch(General.GetNullableString(txtShortcode.Text), General.GetNullableString(txtName.Text)
                                                            , cbActive.Checked.Equals(true) ? 1 : 0
                                                            , sortexpression
                                                            , sortdirection
                                                            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                            , gvNonRoutineRACategory.PageSize
                                                            , ref iRowCount
                                                            , ref iTotalPageCount
                                                            );
        Response.AddHeader("Content-Disposition", "attachment; filename=Category.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Category</h3></td>");
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
    protected void NonRoutineRACategory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvNonRoutineRACategory.Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtShortcode.Text = "";
                txtName.Text = "";
                cbActive.Checked = false;
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
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
    protected void Rebind()
    {
        gvNonRoutineRACategory.SelectedIndexes.Clear();
        gvNonRoutineRACategory.EditIndexes.Clear();
        gvNonRoutineRACategory.DataSource = null;
        gvNonRoutineRACategory.Rebind();
    }
    private void BindData()
    {
        string[] alColumns = { "FLDSHORTCODE", "FLDNAME", "FLDACTIVE" };
        string[] alCaptions = { "Code", "Name", "Active Y/N" };

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixInspectionNonRoutineRACategory.NonRoutineRACategorySearch(General.GetNullableString(txtShortcode.Text), General.GetNullableString(txtName.Text)
                                                           , cbActive.Checked.Equals(true) ? 1 : 0
                                                           , sortexpression
                                                           , sortdirection
                                                           , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                           , gvNonRoutineRACategory.PageSize
                                                           , ref iRowCount
                                                           , ref iTotalPageCount
                                                           );

        General.SetPrintOptions("gvNonRoutineRACategory", "Category", alCaptions, alColumns, ds);

        gvNonRoutineRACategory.DataSource = ds;
        gvNonRoutineRACategory.VirtualItemCount = iRowCount;

    }
    protected void gvRAActivity_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        gvNonRoutineRACategory.Rebind();
    }
    private void DeleteNonRoutineRACategory(int categoryid)
    {
        PhoenixInspectionNonRoutineRACategory.DeleteNonRoutineRACategory(categoryid);
    }
    protected void gvNonRoutineRACategory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvNonRoutineRACategory.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvNonRoutineRACategory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
               PhoenixInspectionNonRoutineRACategory.DeleteNonRoutineRACategory(Int32.Parse(((RadLabel)e.Item.FindControl("lblCategoryId")).Text));
                Rebind();
            }

            if (e.CommandName == "Page")
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

    protected void gvNonRoutineRACategory_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            RadLabel lblCategoryId = (RadLabel)e.Item.FindControl("lblCategoryId");

            if (eb != null)
                eb.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionNonROutineRACategoryAdd.aspx?CATEGORYID=" + lblCategoryId.Text + "'); return true;");
            
        }
    }

    protected void gvNonRoutineRACategory_SortCommand(object sender, GridSortCommandEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "1")
            ViewState["SORTDIRECTION"] = 0;
        else
            ViewState["SORTDIRECTION"] = 1;

       Rebind();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvNonRoutineRACategory.Rebind();
    }
}