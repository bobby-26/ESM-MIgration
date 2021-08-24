using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using SouthNests.Phoenix.DocumentManagement;

public partial class Registers_RegistersCrewApprovalCategory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersCrewApprovalCategory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewApprovalCategory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCrewApprovalCategory.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCrewApprovalCategory.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            // toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionMOCTypeofChangeCategoryAdd.aspx?" + "')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

            MenuCrewCategory.AccessRights = this.ViewState;
            MenuCrewCategory.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["CATEGORYID"] = "";
                gvCrewApprovalCategory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvCrewApprovalCategory.Rebind();
    }

    protected void MenuCrewCategory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;

                gvCrewApprovalCategory.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtShortCode.Text = "";
                txtCategory.Text = "";
                ViewState["PAGENUMBER"] = 1;
               
                gvCrewApprovalCategory.Rebind();
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
        string[] alColumns = { "FLDAPPROVALCATEGORYCODE", "FLDAPPROVALCATEGORYNAME" };
        string[] alCaptions = { "Code", "Category" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixRegistersCrewApprovalCategory.CrewCategorySearch(General.GetNullableString(txtShortCode.Text)
                                                                    , General.GetNullableString(txtCategory.Text)
                                                                    , sortexpression, sortdirection
                                                                    , (int)ViewState["PAGENUMBER"]
                                                                    , gvCrewApprovalCategory.PageSize
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=CrewApprovalCategory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Department</h3></td>");
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

    protected void gvCrewApprovalCategory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "ADD")
            {
                RadTextBox txtShortCodeAdd = (RadTextBox)e.Item.FindControl("txtShortCodeAdd");
                RadTextBox txtCategorynameAdd = (RadTextBox)e.Item.FindControl("txtCategorynameAdd");

                PhoenixRegistersCrewApprovalCategory.CrewApprovalCategoryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , txtCategorynameAdd.Text
                                                                                , txtShortCodeAdd.Text);

                gvCrewApprovalCategory.Rebind();

            }
            if (e.CommandName.ToUpper() == "ADDSUB")
            {
                GridDataItem Item = (GridDataItem)e.Item;
                ViewState["CATEGORYID"] = Item.GetDataKeyValue("FLDAPPROVALCATEGORYID").ToString();
                Response.Redirect("../Registers/RegisterCrewApprovalSubCategory.aspx?CATEGORYID=" + ViewState["CATEGORYID"].ToString());
            }

            if (e.CommandName.ToUpper() == "DELETE")
            {
                PhoenixRegistersCrewApprovalCategory.DeleteCatageory(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblCategoryid")).Text));
                gvCrewApprovalCategory.Rebind();
               
            }
            if (e.CommandName.ToUpper() == "SAVE")
            {
                PhoenixRegistersCrewApprovalCategory.UpdateCatageory(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblCategoryid")).Text)
                                                    , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtCategoryEdit")).Text)
                                                    , General.GetNullableString(((RadLabel)e.Item.FindControl("lblShortCode")).Text));
                gvCrewApprovalCategory.Rebind();
               

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

    protected void gvCrewApprovalCategory_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }

    protected void gvCrewApprovalCategory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewApprovalCategory.CurrentPageIndex + 1;
            BindData();
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
        string[] alColumns = { "FLDAPPROVALCATEGORYCODE", "FLDAPPROVALCATEGORYNAME" };
        string[] alCaptions = { "Code", "Category" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixRegistersCrewApprovalCategory.CrewCategorySearch(General.GetNullableString(txtShortCode.Text)
                                                                    , General.GetNullableString(txtCategory.Text)
                                                                    , sortexpression, sortdirection
                                                                    , (int)ViewState["PAGENUMBER"]
                                                                    , gvCrewApprovalCategory.PageSize
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount);

        General.SetPrintOptions("gvCrewApprovalCategory", "Crew Approval Category", alCaptions, alColumns, ds);


        gvCrewApprovalCategory.DataSource = ds;
        gvCrewApprovalCategory.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
}