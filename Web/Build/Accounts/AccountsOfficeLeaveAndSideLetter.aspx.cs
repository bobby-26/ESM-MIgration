using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class AccountsOfficeLeaveAndSideLetter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            ViewState["vslid"] = Request.QueryString["vslid"];
            ViewState["pbid"] = Request.QueryString["pbid"]; 

            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            PhoenixToolbar toolbarBack = new PhoenixToolbar();
            toolbarBack.AddButton("Back", "VOUCHER",ToolBarDirection.Right);
            MenuPB.AccessRights = this.ViewState;
            MenuPB.MenuList = toolbarBack.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsOfficeLeaveAndSideLetter.aspx?vslid=" + ViewState["vslid"].ToString() + "&pbid=" + ViewState["pbid"].ToString() + "", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvLWBPANDSL')", "Print Grid", "icon_print.png", "PRINT");
            MenuOffice.AccessRights = this.ViewState;
            MenuOffice.MenuList = toolbar.Show();
          //  MenuOffice.SetTrigger(pnlVesselSupplierEntry);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["VESSELID"] = null;


            }

           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuPB_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string date = string.Empty;
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

         
            if (CommandName.ToUpper().Equals("VOUCHER"))
            {
                Response.Redirect("AccountsOfficeLeaveWagesAndSideLetter.aspx", true);
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
        BindData();
       
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDVESSELACCOUNTCODE", "FLDENDDATE", "FLDSUBACCOUNT", "FLDOWNERBUDGETCODE", "FLDSOURCE", "FLDCOMPONENTNAME", "FLDAMOUNT", "FLDVOUCHERNUMBER" };
        string[] alCaptions = { "File Number", "Employee Name", "Rank", "Vessel Account Code", "Portage Bill End Date", "Budget Code", "Owner Budget Code", "Source", "Component", "Amount (USD)", "Voucher Number" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixAccountsOfficePortageBill.OfficeLeaveWagesAndSideLetterSearch(int.Parse(ViewState["vslid"].ToString()), new Guid(ViewState["pbid"].ToString())
                , Int32.Parse(ViewState["PAGENUMBER"].ToString()), gvLWBPANDSL.PageSize
                , ref iRowCount
                , ref iTotalPageCount);

        General.SetPrintOptions("gvLWBPANDSL", "Side Letter And Leave Wages Performance Bonus", alCaptions, alColumns, ds);

      
        gvLWBPANDSL.DataSource = ds;
        gvLWBPANDSL.VirtualItemCount = iRowCount;


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void Rebind()
    {
        gvLWBPANDSL.SelectedIndexes.Clear();
        gvLWBPANDSL.EditIndexes.Clear();
        gvLWBPANDSL.DataSource = null;
        gvLWBPANDSL.Rebind();
    }
    protected void gvLWBPANDSL_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLWBPANDSL.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    //protected void gvLWBPANDSL_Sorting(object sender, GridViewSortEventArgs se)
    //{
    //    ViewState["SORTEXPRESSION"] = se.SortExpression;

    //    if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
    //        ViewState["SORTDIRECTION"] = 1;
    //    else
    //        ViewState["SORTDIRECTION"] = 0;

    //    BindData();
    //}

     protected void gvLWBPANDSL_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
    protected void gvLWBPANDSL_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridEditableItem)
        {
          ImageButton edit = (ImageButton)e.Item.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        ImageButton save = (ImageButton)e.Item.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        ImageButton cancel = (ImageButton)e.Item.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

       
        }
       
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDVESSELACCOUNTCODE", "FLDENDDATE", "FLDSUBACCOUNT", "FLDOWNERBUDGETCODE", "FLDSOURCE", "FLDCOMPONENTNAME", "FLDAMOUNT", "FLDVOUCHERNUMBER" };
        string[] alCaptions = { "File Number", "Employee Name", "Rank", "Vessel Account Code", "Portage Bill End Date", "Budget Code", "Owner Budget Code", "Source", "Component", "Amount (USD)", "Voucher Number" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsOfficePortageBill.OfficeLeaveWagesAndSideLetterSearch(int.Parse(ViewState["vslid"].ToString()), new Guid(ViewState["pbid"].ToString())
                , Int32.Parse(ViewState["PAGENUMBER"].ToString()), iRowCount
                , ref iRowCount
                , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=SideLetterAndLeaveWagesPerformanceBonus.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Side Letter And Leave Wages Performance Bonus</h3></td>");
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

  
    protected void MenuOffice_TabStripCommand(object sender, EventArgs e)
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

    //protected void cmdSort_Click(object sender, EventArgs e)
    //{
    //    ImageButton ib = (ImageButton)sender;
    //    ViewState["SORTEXPRESSION"] = ib.CommandName;
    //    ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
    //    BindData();
    //    SetPageNavigator();
    //}
}
