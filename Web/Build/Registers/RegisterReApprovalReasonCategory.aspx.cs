using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class Registers_RegisterReApprovalReasonCategory : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegisterReApprovalReasonCategory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCategory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            
            MenuRegistersCategory.AccessRights = this.ViewState;
            MenuRegistersCategory.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCategory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        string[] alColumns = { "FLDCATEGORYNAME", "FLDACTIVEYNSTATUS" };
        string[] alCaptions = { "Name", "Active Y/N" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegisterReApprovalReasonCategory.CategorySearch(
            sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvCategory.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=ReEmploymentCategory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>ReEmployment Category</h3></td>");
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

    protected void MenuRegistersCategory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCATEGORYNAME", "FLDACTIVEYNSTATUS" };
        string[] alCaptions = { "Name", "Active Y/N" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixRegisterReApprovalReasonCategory.CategorySearch(
            sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvCategory.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvCategory", "ReEmployment Category", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCategory.DataSource = ds;
            gvCategory.VirtualItemCount = iRowCount;
        }
        else
        {
            gvCategory.DataSource = "";
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {        
        BindData();
        gvCategory.Rebind();
    }
    

    private void InsertCategory(int Activeyn, string CategoryName)
    {
        PhoenixRegisterReApprovalReasonCategory.InsertCategory(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
             Activeyn
             , CategoryName);
    }

    private void UpdateCategory(int Categoryid, string Activeyn, string CategoryName)
    {
        PhoenixRegisterReApprovalReasonCategory.UpdateCategory(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , Categoryid
            ,General.GetNullableInteger(Activeyn)
           ,CategoryName);

        //ucStatus.Text = "Flag information updated";        
    }

    private bool IsValidCategory(string CategoryName)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (CategoryName == string.Empty)
            ucError.ErrorMessage = "Category Name is required.";

        return (!ucError.IsError);
    }

    private void DeleteCategory(int Categoryid)
    {
        PhoenixRegisterReApprovalReasonCategory.DeleteCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Categoryid);
    }

    
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }

    }

    protected void gvCategory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
           
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidCategory(((RadTextBox)e.Item.FindControl("txtCategoryFooter")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertCategory((((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked==true) ? 1 : 0
                    , ((RadTextBox)e.Item.FindControl("txtCategoryFooter")).Text
                   );

                //((RadTextBox)_gridView.FooterRow.FindControl("txtCategoryFooter")).Focus();
                BindData();
                gvCategory.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteCategory(Int32.Parse(((RadLabel)e.Item.FindControl("lblCategoryId")).Text));
                BindData();
                gvCategory.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                Session["FLAGID"] = ((RadLabel)e.Item.FindControl("lblCategoryId")).Text;
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidCategory(((RadTextBox)e.Item.FindControl("txtCategory")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateCategory(
                         Int32.Parse(((RadLabel)e.Item.FindControl("lblCategoryIdEdit")).Text)
                          , (((RadCheckBox)e.Item.FindControl("chkActiveYNEdit")).Checked==true) ? "1" : "0"
                         , ((RadTextBox)e.Item.FindControl("txtCategory")).Text
                         );
                BindData();
                gvCategory.Rebind();
            }
            else if (e.CommandName == "Page")
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

    protected void gvCategory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCategory.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvCategory_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    protected void gvCategory_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
}
