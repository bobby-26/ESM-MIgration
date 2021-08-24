using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class Registers_RegisterReApprovalReasonSubCategory : PhoenixBasePage
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegisterReApprovalReasonSubCategory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSubCategory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersSubCategory.AccessRights = this.ViewState;
            MenuRegistersSubCategory.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvSubCategory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
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
        gvSubCategory.Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDACTIVEYNSTATUS" };
        string[] alCaptions = { "Category Name", "Sub Category Name", "Active Y/N" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegisterReApprovalReasonSubCategory.SubCategorySearch(
            sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvSubCategory.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=ReEmploymentSubCategory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>ReEmployment SubCategory</h3></td>");
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

    protected void MenuRegistersSubCategory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvSubCategory.Rebind();
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDACTIVEYNSTATUS" };
        string[] alCaptions = { "Category Name", "Sub Category Name", "Active Y/N" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixRegisterReApprovalReasonSubCategory.SubCategorySearch(
            sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvSubCategory.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvSubCategory", "ReEmployment SubCategory", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvSubCategory.DataSource = ds;
            gvSubCategory.VirtualItemCount = iRowCount;
        }
        else
        {
            gvSubCategory.DataSource = "";
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvSubCategory.Rebind();
    }
    

    private void InsertSubCategory(int CategoryCode, int SubCategoryCode, int Activeyn)
    {
        PhoenixRegisterReApprovalReasonSubCategory.InsertSubCategory(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
             CategoryCode
             , SubCategoryCode
             , Activeyn);
    }

    private void UpdateSubCategory(int CategoryCode, Guid DtKey,int Activeyn, int SubCategoryid)
    {
        PhoenixRegisterReApprovalReasonSubCategory.UpdateSubCategory(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            CategoryCode,DtKey, Activeyn, SubCategoryid);
    }

    private bool IsValidSubCategory(string CategoryCode, string SubCategoryName)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(CategoryCode) == null)
            ucError.ErrorMessage = "Category is required.";

        if (SubCategoryName.Trim() == null)
            ucError.ErrorMessage = "Sub Category Name is required.";

        return (!ucError.IsError);
    }

    private void DeleteSubCategory(string SubCategoryid)
    {
        PhoenixRegisterReApprovalReasonSubCategory.DeleteSubCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(SubCategoryid));
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

    protected void gvSubCategory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidSubCategory(((RadComboBox)e.Item.FindControl("ddlCategoryAdd")).SelectedValue
                 , ((RadComboBox)e.Item.FindControl("ddlSubCategoryAdd")).SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertSubCategory(
                    Int32.Parse(((RadComboBox)e.Item.FindControl("ddlCategoryAdd")).SelectedValue)
                    , Int32.Parse(((RadComboBox)e.Item.FindControl("ddlSubCategoryAdd")).SelectedValue)
                    , (((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked==true) ? 1 : 0
                );

                //((RadCheckBox)e.Item.FindControl("ddlCategoryAdd")).Focus();
                BindData();
                gvSubCategory.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteSubCategory(((RadLabel)e.Item.FindControl("lblDtKey")).Text);
                BindData();
                gvSubCategory.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidSubCategory(((RadComboBox)e.Item.FindControl("ddlCategoryEdit")).SelectedValue
               , ((RadComboBox)e.Item.FindControl("ddlSubCategoryEdit")).SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateSubCategory(
                         Int32.Parse(((RadComboBox)e.Item.FindControl("ddlCategoryEdit")).SelectedValue)
                         , new Guid(((RadLabel)e.Item.FindControl("lblDtKeyEdit")).Text)
                         , (((RadCheckBox)e.Item.FindControl("chkActiveYNEdit")).Checked==true) ? 1 : 0
                          , Int32.Parse(((RadComboBox)e.Item.FindControl("ddlSubCategoryEdit")).SelectedValue)
                     );
                BindData();
                gvSubCategory.Rebind();
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

    protected void gvSubCategory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSubCategory.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvSubCategory_ItemDataBound(object sender, GridItemEventArgs e)
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

            RadComboBox ddlCategoryEdit = (RadComboBox)e.Item.FindControl("ddlCategoryEdit");
            RadComboBox ddlSubCategoryEdit = (RadComboBox)e.Item.FindControl("ddlSubCategoryEdit");
            RadLabel lblSubCategoryIdEdit = (RadLabel)e.Item.FindControl("lblSubCategoryIdEdit");
            RadLabel lblCategoryIDEdit = (RadLabel)e.Item.FindControl("lblCategoryIDEdit");

            if (ddlCategoryEdit != null && lblCategoryIDEdit != null) ddlCategoryEdit.SelectedValue = lblCategoryIDEdit.Text.Trim();
            if (ddlSubCategoryEdit != null && lblSubCategoryIdEdit != null) ddlSubCategoryEdit.SelectedValue = lblSubCategoryIdEdit.Text.Trim();
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

    protected void gvSubCategory_SortCommand(object sender, GridSortCommandEventArgs e)
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
