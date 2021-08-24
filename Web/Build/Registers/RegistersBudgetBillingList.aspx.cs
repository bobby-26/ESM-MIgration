using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersBudgetBillingList : PhoenixBasePage
{
    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetRowSelection();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();

        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            gvBudgetBillingList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        toolbar.AddFontAwesomeButton("../Registers/RegistersBudgetBillingList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvBudgetBillingList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        MenuRegistersBudgetBillingList.AccessRights = this.ViewState;
        MenuRegistersBudgetBillingList.MenuList = toolbar.Show();
    }

    protected void RegistersBudgetBillingList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            Rebind();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDBILLINGITEMDESCRIPTION", "FLDFREQUENCY", "FLDBILLINGBASIS", "FLDBILLINGUNIT", "FLDVESSELBUDGETCODE", "FLDCREDITACCOUNT" };
        string[] alCaptions = { "Billing Item Description", "Frequency", "Billing Basis", "Billing Unit", "Vessel Budget Code", "Credit Account" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersBudgetBilling.BudgetBillingSearch(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , null
            , sortexpression
            , sortdirection
            , (int)ViewState["PAGENUMBER"]
            , gvBudgetBillingList.PageSize
            , ref iRowCount
            , ref iTotalPageCount);

        General.SetPrintOptions("gvBudgetBillingList", "Standard Billing Item List", alCaptions, alColumns, ds);


        gvBudgetBillingList.DataSource = ds;
        gvBudgetBillingList.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        if (ViewState["BUDGETBILLINGID"] == null)
        {
            ViewState["BUDGETBILLINGID"] = ds.Tables[0].Rows[0]["FLDBUDGETBILLINGID"].ToString();
        }

        ifMoreInfo.Attributes["src"] = "../Registers/RegistersBudgetBilling.aspx?BUDGETBILLINGID=" + ViewState["BUDGETBILLINGID"];
        SetRowSelection();

    }
    protected void gvBudgetBillingList_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBudgetBillingList.CurrentPageIndex + 1;
            BindData();
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

        string[] alColumns = { "FLDBILLINGITEMDESCRIPTION", "FLDFREQUENCY", "FLDBILLINGBASIS", "FLDBILLINGUNIT", "FLDVESSELBUDGETCODE", "FLDCREDITACCOUNT" };
        string[] alCaptions = { "Billing Item Description", "Frequency", "Billing Basis", "Billing Unit", "Vessel Budget Code", "Credit Account" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        ds = PhoenixRegistersBudgetBilling.BudgetBillingSearch(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , null
            , sortexpression
            , sortdirection
            , 1
            , iRowCount
            , ref iRowCount
            , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=StandardBillingItemList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Standard Billing Item List</h3></td>");
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

    private void SetRowSelection()
    {
        if (ViewState["BUDGETBILLINGID"] != null)
        {
            gvBudgetBillingList.SelectedIndexes.Clear();
            foreach (GridDataItem item in gvBudgetBillingList.Items)
            {
                if (item.GetDataKeyValue("FLDBUDGETBILLINGID").ToString().Equals(ViewState["BUDGETBILLINGID"].ToString()))
                {
                    gvBudgetBillingList.SelectedIndexes.Add(item.ItemIndex);
                }
            }
        }
    }

    protected void gvBudgetBillingList_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void Rebind()
    {
        gvBudgetBillingList.SelectedIndexes.Clear();
        gvBudgetBillingList.EditIndexes.Clear();
        gvBudgetBillingList.DataSource = null;
        gvBudgetBillingList.Rebind();
    }


    protected void gvBudgetBillingList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                ViewState["BUDGETBILLINGID"] = ((RadLabel)e.Item.FindControl("lblBudgetBillingId")).Text;
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteBudgetBilling(new Guid(((RadLabel)e.Item.FindControl("lblBudgetBillingId")).Text));
                ViewState["BUDGETBILLINGID"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBudgetBillingList_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        Rebind();
    }

    protected void gvBudgetBillingList_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["BUDGETBILLINGID"] = null;
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void DeleteBudgetBilling(Guid BudgetBillingid)
    {
        PhoenixRegistersBudgetBilling.DeleteBudgetBilling(PhoenixSecurityContext.CurrentSecurityContext.UserCode, BudgetBillingid);
        Rebind();
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void gvBudgetBillingList_SelectedIndexChanged(object sender, GridViewSelectEventArgs e)
    {
        gvBudgetBillingList.SelectedIndexes.Add(e.NewSelectedIndex);
        ViewState["BUDGETBILLINGID"] = ((LinkButton)gvBudgetBillingList.Items[e.NewSelectedIndex].FindControl("lblBudgetBillingId")).CommandArgument;
        Rebind();
    }

}
