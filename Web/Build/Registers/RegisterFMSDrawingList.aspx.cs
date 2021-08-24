using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegisterFMSDrawingList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegisterFMSDrawingList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvFMSCategory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegisterFMSDrawingList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegisterFMSDrawingList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegisterFMSDrawingsCategoryAdd.aspx?" + "')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

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

        Response.AddHeader("Content-Disposition", "attachment; filename=FMSCategory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='3' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Drawing Category</h3></td>");
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
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem footerItem = (GridFooterItem)gvFMSCategory.MasterTableView.GetItems(GridItemType.Footer)[0];
                string scode = ((RadTextBox)footerItem.FindControl("txtOrderAdd")).Text;
                string code = ((RadTextBox)footerItem.FindControl("txtDrawingCodeAdd")).Text;
                string name = ((RadTextBox)footerItem.FindControl("txDrawingsNameAdd")).Text;
                if (!IsValidDrawing(scode, name, code))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegisterFMSDrawing.FMSDrawingCategoryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , null
                    , int.Parse(((RadTextBox)footerItem.FindControl("txtOrderAdd")).Text)
                    , ((RadTextBox)footerItem.FindControl("txtDrawingCodeAdd")).Text
                    , ((RadTextBox)footerItem.FindControl("txDrawingsNameAdd")).Text);

                Rebind();
                ((RadTextBox)footerItem.FindControl("txDrawingsNameAdd")).Focus();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["DrawingID"] = ((RadLabel)e.Item.FindControl("lblDrawingsID")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");


                //BindData();
                //gvFMSCategory.Rebind();
                return;
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
    protected void gvFMSCategory_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (!IsValidDrawing(((RadTextBox)e.Item.FindControl("txtOrderEdit")).Text
                    , ((RadTextBox)e.Item.FindControl("txtDrawingCodeEdit")).Text
                    , ((RadTextBox)e.Item.FindControl("txtDrawingsNameEdit")).Text
                  ))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixRegisterFMSDrawing.FMSDrawingCategoryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDrawingsIDEdit")).Text)
                    , int.Parse(((RadTextBox)e.Item.FindControl("txtOrderEdit")).Text)
                    , ((RadTextBox)e.Item.FindControl("txtDrawingCodeEdit")).Text
                    , ((RadTextBox)e.Item.FindControl("txtDrawingsNameEdit")).Text);
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvFMSCategory.SelectedIndexes.Clear();
        gvFMSCategory.EditIndexes.Clear();
        gvFMSCategory.DataSource = null;
        gvFMSCategory.Rebind();
    }
    private bool IsValidDrawing(string order, string code, string name)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvFMSCategory;

        if (order.Trim().Equals(""))
            ucError.ErrorMessage = "Order is required.";

        if (code.Trim().Equals(""))
            ucError.ErrorMessage = "Short Code is required.";

        if (name.Trim().Equals(""))
            ucError.ErrorMessage = "Drawing Name is required.";

        return (!ucError.IsError);
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

            //if (eb != null)
            //{
            //    eb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegisterFMSDrawingsCategoryAdd.aspx?categoryid=" + (((RadLabel)e.Item.FindControl("lblCategoryId")).Text.ToString()) + "');return false;");
            //}

            RadLabel lblDrawingsID = (RadLabel)e.Item.FindControl("lblDrawingsID");
            LinkButton lnkDrawingsName = (LinkButton)e.Item.FindControl("lnkDrawingsName");
            if (lnkDrawingsName != null)
            {
                lnkDrawingsName.Visible = SessionUtil.CanAccess(this.ViewState, lnkDrawingsName.CommandName);
                lnkDrawingsName.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'FMS', '" + Session["sitepath"] + "/Registers/RegisterFMSDrawingsCategoryAdd.aspx?subcategoryid=" + lblDrawingsID.Text + "');return true;");
            }


            //if ((lblorderid != null) && (lblorderid.Text == "0"))
            //{
            //    del.Visible = false;
            //    eb.Visible = false;
            //}
            //LinkButton cmdDisVessel = (LinkButton)e.Item.FindControl("cmdMapVesselType");
            //if (cmdDisVessel != null)
            //{
            //    cmdDisVessel.Visible = SessionUtil.CanAccess(this.ViewState, cmdDisVessel.CommandName);
            //    cmdDisVessel.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'FMS', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementFMSDrawingVesselTypeAdd.aspx?drawingcategoryid=" + (((RadLabel)e.Item.FindControl("lblDrawingsID")).Text.ToString()) + "');return true;");
            //}

            //LinkButton cmdMapVessel = (LinkButton)e.Item.FindControl("cmdMapVessel");
            //if (cmdMapVessel != null)
            //{
            //    cmdMapVessel.Visible = SessionUtil.CanAccess(this.ViewState, cmdMapVessel.CommandName);
            //    cmdMapVessel.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'FMS', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementFMSDrawingVesselsAdd.aspx?drawingcategoryid=" + (((RadLabel)e.Item.FindControl("lblDrawingsID")).Text.ToString()) + "');return true;");
            //}
        }
        if (e.Item is GridEditableItem)
        {
            //RadTextBox txt = (RadTextBox)e.Item.FindControl("txtCertificatesCodeEdit");
            //if (txtCertificatesCodeEdit != null)
            //{
            //    ((RadTextBox)e.Item.FindControl("txtCertificatesCodeEdit")).Focus();
            //}
            ////((UserControlDepartment)e.Item.FindControl("ucDepartmentEdit")).Focus();
            //RadComboBox ddlCertificateCategoryEdit = (RadComboBox)e.Item.FindControl("ddlCertificateCategoryEdit");
            //RadLabel lblCertificateCategoryId = (RadLabel)e.Item.FindControl("lblCertificateCategoryId");
            //if (ddlCertificateCategoryEdit != null && lblCertificateCategoryId != null)
            //{
            //    ddlCertificateCategoryEdit.SelectedValue = lblCertificateCategoryId.Text.Trim();
            //}
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            //RadComboBox ddlCertificateCategoryAdd = (RadComboBox)e.Item.FindControl("ddlCertificateCategoryAdd");
            //RadComboBox ddlSurveyTemplateAdd = (RadComboBox)e.Item.FindControl("ddlSurveyTemplateAdd");


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

    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixRegisterFMSDrawing.FMSDrawingCategoryDelete(new Guid(ViewState["DrawingID"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}