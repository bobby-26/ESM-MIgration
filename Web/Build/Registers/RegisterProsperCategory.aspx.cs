using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class Registers_RegisterProsperCategory : PhoenixBasePage
{

    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvprospercategory.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvprospercategory.UniqueID, "Edit$" + r.RowIndex.ToString());
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
            toolbar.AddFontAwesomeButton("../Registers/RegisterProsperCategory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvprospercategory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegisterProsperCategory.aspx", "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("../Registers/RegisterProsperCategory.aspx", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");
            MenuRegistersProsper.AccessRights = this.ViewState;
            MenuRegistersProsper.MenuList = toolbar.Show();
            //MenuRegistersProsper.SetTrigger(pnlprospercatgory);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["CODE"] = txtcategorycode.Text;
                ViewState["NAME"] = txtname.Text;

                gvprospercategory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        string[] alColumns = { "FLDCATEGORYCODE", "FLDCATEGORYNAME", "FLDMAXSCORE" };
        string[] alCaptions = { "Category Code", "Category Name", "Max Score" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixRegisterProsperCategory.ProsperCategorySearch(txtcategorycode.Text, txtname.Text, sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=ProsperCategory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Prosper Category</h3></td>");
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

    protected void RegistersProsper_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {

                ViewState["CODE"] = txtcategorycode.Text;
                ViewState["NAME"] = txtname.Text;

                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvprospercategory.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtcategorycode.Text = "";
                txtname.Text = "";
                ViewState["CODE"] = txtcategorycode.Text;
                ViewState["NAME"] = txtname.Text;
                BindData();
                gvprospercategory.Rebind();

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

        string[] alColumns = { "FLDCATEGORYCODE", "FLDCATEGORYNAME", "FLDMAXSCORE" };
        string[] alCaptions = { "Category Code", "Category Name", "Max Score" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegisterProsperCategory.ProsperCategorySearch(ViewState["CODE"].ToString(), ViewState["NAME"].ToString(), sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvprospercategory.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        General.SetPrintOptions("gvprospercategory", "Category", alCaptions, alColumns, ds);
        gvprospercategory.DataSource = ds;
        gvprospercategory.VirtualItemCount = iRowCount;



        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    protected void ddlmodulecode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;

        BindData();
        gvprospercategory.Rebind();
    }

    private bool IsValidInstallation(string categorycode, string categoryname)
    {
        ucError.HeaderMessage = "Please provide the following required information";


        if (categorycode.Trim().Equals(""))
            ucError.ErrorMessage = "Category Code is required.";

        if (categoryname.Trim().Equals(""))
            ucError.ErrorMessage = "Category Name is required.";


        return (!ucError.IsError);
    }

    protected void gvprospercategory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvprospercategory.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvprospercategory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {


                string categorycode = ((RadTextBox)e.Item.FindControl("txtCategoryCodeAdd")).Text.Trim();
                string categoryname = ((RadTextBox)e.Item.FindControl("txtCategoryNameAdd")).Text.Trim();
                int? maxscore = General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("txtmaxscoreAdd")).Text.Trim()); //Convert.ToDecimal();
                decimal? noinsscore = General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtnoinsscoreAdd")).Text.Trim()); //Convert.ToDecimal();


                if (!IsValidInstallation(categorycode, categoryname))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegisterProsperCategory.InsertProsperCategory(
                     categorycode
                     , categoryname
                     , maxscore
                     , noinsscore);

                BindData();
                gvprospercategory.Rebind();

            }
            if (e.CommandName.ToUpper() == "DELETE")
            {
                try
                {


                    Guid? categoryid = General.GetNullableGuid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDCATEGORYID"].ToString());

                    PhoenixRegisterProsperCategory.DeleteProsperCategory(categoryid);

                    BindData();
                    gvprospercategory.Rebind();
                }

                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;

                }
            }
            if (e.CommandName.ToUpper() == "UPDATE")
            {
                try
                {

                    string code = ((RadLabel)e.Item.FindControl("txtCategoryCode")).Text;
                    string name = ((RadTextBox)e.Item.FindControl("txtCategoryName")).Text;

                    Guid? categoryid = General.GetNullableGuid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDCATEGORYID"].ToString());
                    if (!IsValidInstallation(code, name))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixRegisterProsperCategory.UpdateProsperCategory(
                              categoryid
                             , code
                             , name
                             , General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("txtmaxscore")).Text)
                             , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtnoinsscore")).Text)
                             , General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("txtsortorderedit")).Text));


                    BindData();
                    gvprospercategory.Rebind();

                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
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

    protected void gvprospercategory_ItemDataBound(object sender, GridItemEventArgs e)
    {


        if (e.Item is GridDataItem)
        {

            Guid? categoryid = General.GetNullableGuid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDCATEGORYID"].ToString());

            LinkButton lb = (LinkButton)e.Item.FindControl("lnkcatogoryCode");
            if (lb != null && lb.Text == "VTE")
            {
                LinkButton map = (LinkButton)e.Item.FindControl("cmdMap");
                if (map != null)
                    map.Visible = false;
            }

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            LinkButton cmdMap = (LinkButton)e.Item.FindControl("cmdMap");
            if (cmdMap != null)
                cmdMap.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegisterProsperCategoryMapping.aspx?CATEGORYID=" + categoryid.ToString() + "');return false;");

        }
    }

    protected void gvprospercategory_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
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
