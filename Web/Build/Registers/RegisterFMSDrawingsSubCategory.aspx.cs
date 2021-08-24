﻿using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegisterFMSDrawingsSubCategory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegisterFMSDrawingsSubCategory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvFMSSubCategory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegisterFMSDrawingsSubCategoryAdd.aspx?categoryid=" + ddlCategory.SelectedValue + "')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            toolbar.AddFontAwesomeButton("../Registers/RegisterFMSDrawingsSubCategory.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuFMSSubCategory.AccessRights = this.ViewState;
            MenuFMSSubCategory.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
                BindFMSCategory();
            }
            //BindData();
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

        string[] alColumns = { "FLDSUBCATEGORYCODE", "FLDSUBCATEGORYNAME" };
        string[] alCaptions = { "Code", "SubCategory" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegisterFMSDrawing.FMSDrawingSubCategorySearch(General.GetNullableGuid(ddlCategory.SelectedValue), sortexpression, sortdirection,
            1,
           iRowCount,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=FMSSubCategory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>FMS SubCategory</h3></td>");
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

    protected void FMSSubCategory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {

                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvFMSSubCategory.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {

                ViewState["PAGENUMBER"] = 1;
                ddlCategory.ClearSelection();
                BindData();
                gvFMSSubCategory.Rebind();

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
        string[] alColumns = { "FLDSUBCATEGORYCODE", "FLDSUBCATEGORYNAME" };
        string[] alCaptions = { "Code", "SubCategory" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixRegisterFMSDrawing.FMSDrawingSubCategorySearch(General.GetNullableGuid(ddlCategory.SelectedValue), sortexpression, sortdirection,
           (int)ViewState["PAGENUMBER"],
           gvFMSSubCategory.PageSize,
           ref iRowCount,
           ref iTotalPageCount);


        General.SetPrintOptions("gvFMSSubCategory", "FMS Drawing Subcategory", alCaptions, alColumns, ds);

        gvFMSSubCategory.DataSource = ds;
        gvFMSSubCategory.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvFMSSubCategory_Sorting(object sender, GridSortCommandEventArgs se)
    {

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
    protected void gvFMSSubCategory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegisterFMSDrawing.FMSDrawingSubCategoryDelete(new Guid(((RadLabel)e.Item.FindControl("lblSubCategoryId")).Text.ToString()));
                BindData();
                gvFMSSubCategory.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("PAGE"))
                ViewState["PAGENUMBER"] = null;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvFMSSubCategory_ItemDataBound(Object sender, GridItemEventArgs e)
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

            RadLabel lblSubCategory = (RadLabel)e.Item.FindControl("lblSubCategory");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("SubCategory");
            if (uct != null)
            {
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lblSubCategory.ClientID;
            }

            if (eb != null)
            {
                eb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegisterFMSDrawingsSubCategoryAdd.aspx?subcategoryid=" + (((RadLabel)e.Item.FindControl("lblSubCategoryId")).Text.ToString()) + "');return false;");
            }
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");

            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            RadComboBox category = (RadComboBox)e.Item.FindControl("ddlCategoryEdit");
            RadLabel categoryid = (RadLabel)e.Item.FindControl("lblCategoryIdEdit");
            if (category != null && categoryid != null)
            {
                category.DataSource = PhoenixRegisterFMSDrawing.FMSDrawingCategoryList();
                category.DataBind();
                category.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                category.SelectedValue = categoryid.Text.ToString();
            }

            LinkButton cmdDisVessel = (LinkButton)e.Item.FindControl("cmdMapVesselType");
            if (cmdDisVessel != null)
            {
                cmdDisVessel.Visible = SessionUtil.CanAccess(this.ViewState, cmdDisVessel.CommandName);
                cmdDisVessel.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'Map Vessel Type', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementFMSDrawingVesselTypeAdd.aspx?drawingcategoryid=" + (((RadLabel)e.Item.FindControl("lblSubCategoryId")).Text.ToString()) + "');return true;");
            }

            LinkButton cmdMapVessel = (LinkButton)e.Item.FindControl("cmdMapVessel");
            if (cmdMapVessel != null)
            {
                cmdMapVessel.Visible = SessionUtil.CanAccess(this.ViewState, cmdMapVessel.CommandName);
                cmdMapVessel.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'Exclude Vessels', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementFMSDrawingVesselsAdd.aspx?drawingcategoryid=" + (((RadLabel)e.Item.FindControl("lblSubCategoryId")).Text.ToString()) + "');return true;");
            }
        }
    }
    private bool IsValidCountry(string category, string shortcode, string subcategory)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(category) == null)
            ucError.ErrorMessage = "Category is required.";

        if (shortcode.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (subcategory.Trim().Equals(""))
            ucError.ErrorMessage = "SubCategory is required.";

        return (!ucError.IsError);
    }

    protected void BindFMSCategory()
    {
        ddlCategory.DataSource = PhoenixRegisterFMSDrawing.FMSDrawingCategoryList();
        ddlCategory.DataTextField = "FLDDRAWINGCATEGORY";
        ddlCategory.DataValueField = "FLDFMSDRAWINGCATEGORY";
        ddlCategory.DataBind();
    }
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void gvFMSSubCategory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFMSSubCategory.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
