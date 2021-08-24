using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionTaskSubCategory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionTaskSubCategory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCountry')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersCountry.AccessRights = this.ViewState;
            MenuRegistersCountry.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

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
        LinkButton ib = (LinkButton)sender;
        Rebind();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME" };
        string[] alCaptions = { "Category", "Subcategory" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionTaskCategory.InspectionTaskSubCategorySearch(sortexpression, sortdirection,
            1,
          iRowCount,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=InspectionTaskSubCategory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Task Subcategory</h3></td>");
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
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Rebind();
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
        gvCountry.SelectedIndexes.Clear();
        gvCountry.EditIndexes.Clear();
        gvCountry.DataSource = null;
        gvCountry.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME" };
        string[] alCaptions = { "Category", "Subcategory" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixInspectionTaskCategory.InspectionTaskSubCategorySearch(sortexpression, sortdirection,
            gvCountry.CurrentPageIndex + 1,
          gvCountry.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvCountry", "Task Subcategory", alCaptions, alColumns, ds);

        gvCountry.DataSource = ds;
        gvCountry.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void gvCountry_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridFooterItem)
            {
                if (e.CommandName.ToUpper().Equals("ADD"))
                {
                    if (!IsValidCountry(((RadComboBox)e.Item.FindControl("ddlCategoryAdd")).SelectedValue.ToString(),
                        ((RadTextBox)e.Item.FindControl("txtSubCategoryAdd")).Text,
                        ((TextBox)e.Item.FindControl("txtDescriptionAdd")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixInspectionTaskCategory.InspectionTaskSubCategoryInsert(((RadTextBox)e.Item.FindControl("txtSubCategoryAdd")).Text,
                                                                                new Guid(((RadComboBox)e.Item.FindControl("ddlCategoryAdd")).SelectedValue.ToString()),
                                                                                ((TextBox)e.Item.FindControl("txtDescriptionAdd")).Text);

                    ((RadTextBox)e.Item.FindControl("txtSubCategoryAdd")).Focus();
                    Rebind();
                }
            }
            if (e.Item is GridEditableItem)
            {
                if (e.CommandName.ToUpper().Equals("UPDATE"))
                {
                    if (!IsValidCountry(((RadComboBox)e.Item.FindControl("ddlCategoryEdit")).SelectedValue.ToString(),
                        ((RadTextBox)e.Item.FindControl("txtSubCategoryEdit")).Text,
                        ((TextBox)e.Item.FindControl("txtDescriptionEdit")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixInspectionTaskCategory.InspectionTaskSubCategoryUpdate(new Guid(((RadLabel)e.Item.FindControl("lblSubCategoryIdEdit")).Text.ToString())
                                                                                , new Guid(((RadComboBox)e.Item.FindControl("ddlCategoryEdit")).SelectedValue.ToString())
                                                                                , ((RadTextBox)e.Item.FindControl("txtSubCategoryEdit")).Text
                                                                                , ((TextBox)e.Item.FindControl("txtDescriptionEdit")).Text);
                    Rebind();
                }

                else if (e.CommandName.ToUpper().Equals("DELETE"))
                {
                    PhoenixInspectionTaskCategory.InspectionTaskSubCategoryDelete(new Guid(((RadLabel)e.Item.FindControl("lblSubCategoryId")).Text.ToString()));
                    Rebind();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvCountry_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            RadComboBox category = (RadComboBox)e.Item.FindControl("ddlCategoryEdit");
            RadLabel categoryid = (RadLabel)e.Item.FindControl("lblCategoryIdEdit");
            if (category != null && categoryid != null)
            {
                category.DataSource = PhoenixInspectionTaskCategory.InspectionTaskCategoryList();
                category.DataBind();
                // category.Items.Insert(0, new ListItem("--Select--","Dummy"));
                category.SelectedValue = categoryid.Text.ToString();
            }
        }

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            RadComboBox category = (RadComboBox)e.Item.FindControl("ddlCategoryAdd");
            if (category != null)
            {
                category.DataSource = PhoenixInspectionTaskCategory.InspectionTaskCategoryList();
                category.DataBind();
                //  category.Items.Insert(0, new ListItem("--Select--", "Dummy"));
            }
        }
    }

    private bool IsValidCountry(string category, string subcategory, string Description)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(category) == null)
            ucError.ErrorMessage = "Category is required.";

        if (subcategory.Trim().Equals(""))
            ucError.ErrorMessage = "Subcategory is required.";

        //if (Description.Trim().Equals(""))
        //    ucError.ErrorMessage = "Description is required.";

        return (!ucError.IsError);
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

    protected void gvCountry_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
