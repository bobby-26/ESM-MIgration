using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class CommonBudgetCodeAllocation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["VESSELBUDGETID"] = null;
            ViewState["BUDGETALLOCATIONID"] = null;
            ViewState["UCAPPORTIONMETHOD"] = null;

            if (Request.QueryString["vesselaccountid"] != null)
                ViewState["VESSELACCOUNTID"] = Request.QueryString["vesselaccountid"].ToString();
        }

        
        toolbar.AddFontAwesomeButton("../Common/CommonBudgetCodeAllocation.aspx?vesselaccountid=" + Request.QueryString["vesselaccountid"].ToString() + "&budgetgroupallocationid=" + Request.QueryString["budgetgroupallocationid"].ToString() + "&vesselbudgetallocationid=" + Request.QueryString["vesselbudgetallocationid"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvBudgetCodeAllocation')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        MenuCommonBudgetGroupAllocation.AccessRights = this.ViewState;
        MenuCommonBudgetGroupAllocation.MenuList = toolbar.Show();
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

   
    }


    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSUBACCOUNT", "FLDDESCRIPTION", "FLDBUDGETAMOUNT", "FLDALLOWANCE", "FLDAPPORTIONMENTMETHODNAME" };
        string[] alCaptions = { "Budget Group", "Description", "Budget Amount", "Allowance", "Apportionment" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCommonBudgetCodeAllocation.BudgetCodeAllocationSearch(
           Request.QueryString["vesselaccountid"] != null ? General.GetNullableInteger(Request.QueryString["vesselaccountid"].ToString()) : null
           , Request.QueryString["budgetgroupallocationid"] != null ? General.GetNullableInteger(Request.QueryString["budgetgroupallocationid"].ToString()) : null
           , Request.QueryString["vesselbudgetallocationid"] != null ? General.GetNullableGuid(Request.QueryString["vesselbudgetallocationid"].ToString()) : null
           , (int)ViewState["PAGENUMBER"]
           , iRowCount
           , ref iRowCount
           , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=BudgetCodeAllocation.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Budget Group Allocation</h3></td>");
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

    protected void CommonBudgetGroupAllocation_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
        PeriodRebind();
    }

    private void BindBudgetCode()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSUBACCOUNT", "FLDDESCRIPTION", "FLDBUDGETAMOUNT", "FLDALLOWANCE", "FLDAPPORTIONMENTMETHODNAME" };
        string[] alCaptions = { "Budget Group", "Description", "Budget Amount", "Allowance", "Apportionment" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCommonBudgetCodeAllocation.BudgetCodeAllocationSearch(
            Request.QueryString["vesselaccountid"] != null ? General.GetNullableInteger(Request.QueryString["vesselaccountid"].ToString()) : null
            , Request.QueryString["budgetgroupallocationid"] != null ? General.GetNullableInteger(Request.QueryString["budgetgroupallocationid"].ToString()):null
            , Request.QueryString["vesselbudgetallocationid"] != null ? General.GetNullableGuid(Request.QueryString["vesselbudgetallocationid"].ToString()) : null
            , (int)ViewState["PAGENUMBER"]
            , gvBudgetCodeAllocation.PageSize
            , ref iRowCount
            , ref iTotalPageCount);

        General.SetPrintOptions("gvBudgetCodeAllocation", "Budget Code Allocation", alCaptions, alColumns, ds);

        gvBudgetCodeAllocation.DataSource = ds;
        gvBudgetCodeAllocation.VirtualItemCount = iRowCount;

        if (!IsPostBack)
        {
            //ViewState["BUDGETCODEALLOCATIONID"] = ds.Tables[0].Rows[0]["FLDBUDGETCODEALLOCATIONID"].ToString();


            //txtBudgetAmount.Text = ds.Tables[0].Rows[0]["FLDBUDGETGROUPAMOUNT"].ToString();
            //txtBudgetGroup.Text = ds.Tables[0].Rows[0]["FLDBUDGETGROUPNAME"].ToString();
            //txtFinancialYear.Text = ds.Tables[0].Rows[0]["FLDFINANCIALYEARQUICK"].ToString();
            //txtAccountCode.Text = ds.Tables[0].Rows[0]["FLDACCOUNTCODE"].ToString();


        }
        SetRowSelectionOfBudgetCode();
        //}
        //  else
        //  {
        //      DataTable dt = ds.Tables[0];
        //      //ShowNoRecordsFound(dt, gvBudgetCodeAllocation);

        ////      gvBudgetCodeAllocation.SelectedIndex = -1;
        // //     gvBudgetCodeAllocation.EditIndex = -1;
        //  }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void SetRowSelectionOfBudgetCode()
    {
        gvBudgetCodeAllocation.SelectedIndexes.Clear();

        for (int i = 0; i < gvBudgetCodeAllocation.Items.Count; i++)
        {
            if (gvBudgetCodeAllocation.MasterTableView.DataKeyValues[i].ToString().Equals(ViewState["BUDGETCODEALLOCATIONID"].ToString()))
            {
                gvBudgetCodeAllocation.MasterTableView.Items[i].Selected = true;
            }
        }
    }
    private void BindBudgetPeriod()
    {
        DataSet ds = PhoenixCommonBudgetCodeAllocation.BudgetCodePeriodAllocationSearch(
            Request.QueryString["vesselaccountid"] != null ? General.GetNullableInteger(Request.QueryString["vesselaccountid"].ToString()) : null
             , General.GetNullableInteger(((ViewState["BUDGETCODEALLOCATIONID"] == null) || (ViewState["BUDGETCODEALLOCATIONID"].ToString().Equals(""))) ? "0" : ViewState["BUDGETCODEALLOCATIONID"].ToString())
             );

     
        gvBudgetPeriodAllocation.DataSource = ds;
     
    }
    protected void gvBudgetCodeAllocation_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                int ibudgetcodeallocationout = 0;
                if (!IsValidBudget(
                                    ((UserControlMaskNumber)e.Item.FindControl("ucBudgetAmount")).Text
                                    , ((UserControlMaskNumber)e.Item.FindControl("ucAllowanceAmount")).Text
                                    , ((UserControlHard)e.Item.FindControl("ucApportionmentMethod")).SelectedHard
                                    ))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCommonBudgetCodeAllocation.BudgetCodeAllocationUpdate(
                         int.Parse(((RadLabel)e.Item.FindControl("lblBudgetCodeAllocationId")).Text)
                        //int ? taskid = General.GetNullableInteger(eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDTASKID"].ToString());
                        , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucBudgetAmount")).Text)
                        , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAllowanceAmount")).Text)
                        , ViewState["UCAPPORTIONMETHOD"]==null? int.Parse(((UserControlHard)e.Item.FindControl("ucApportionmentMethod")).SelectedHard): int.Parse(ViewState["UCAPPORTIONMETHOD"].ToString())
                        , ref ibudgetcodeallocationout
                        );


                ViewState["BUDGETCODEALLOCATIONID"] = ibudgetcodeallocationout.ToString();
                Rebind();


             //   string script = "javascript:fnReloadList('codehelp1');";
             //  RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);


            }

            else if (e.CommandName.ToString().ToUpper() == "SELECT")
            {
                //ViewState["RowIndex"] = e.Item.RowIndex;
                ViewState["BUDGETCODEALLOCATIONID"] = ((RadLabel)e.Item.FindControl("lblBudgetGroupAllocationId")).Text;
                //BindFEData();
                PeriodRebind();

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
        gvBudgetCodeAllocation.SelectedIndexes.Clear();
        gvBudgetCodeAllocation.EditIndexes.Clear();
        gvBudgetCodeAllocation.DataSource = null;
        gvBudgetCodeAllocation.Rebind();
    }
    protected void PeriodRebind()
    {
        gvBudgetPeriodAllocation.SelectedIndexes.Clear();
        gvBudgetPeriodAllocation.EditIndexes.Clear();
        gvBudgetPeriodAllocation.DataSource = null;
        gvBudgetPeriodAllocation.Rebind();
    }
    protected void gvBudgetCodeAllocation_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdEdit");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);

            LinkButton ibSave = (LinkButton)e.Item.FindControl("cmdSave");
            if (ibSave != null) ibSave.Visible = SessionUtil.CanAccess(this.ViewState, ibSave.CommandName);

            UserControlHard ucApportionment = (UserControlHard)e.Item.FindControl("ucApportionmentMethod");

            if (e.Item.IsInEditMode && string.IsNullOrEmpty(ucApportionment.SelectedHard))
            {
                if (ucApportionment != null)
                {
                    ucApportionment.HardTypeCode = "106";
                    ucApportionment.bind();
                    ucApportionment.SelectedHard = ((RadLabel)e.Item.FindControl("RadLabel1")).Text;
                }
            }

        }
    }

    private bool IsValidBudget(string budgetamount, string allowanceamount, string apportionment)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDecimal(budgetamount) == null)
            ucError.ErrorMessage = "Budget Amount is required.";

        if (General.GetNullableDecimal(allowanceamount) == null)
            ucError.ErrorMessage = "Allowance Amount is required.";

        if (General.GetNullableInteger(apportionment) == null)
            ucError.ErrorMessage = "Apportionment method is required.";

        return (!ucError.IsError);
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void gvBudgetPeriodAllocation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBudgetPeriodAllocation.CurrentPageIndex + 1;
            BindBudgetPeriod();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBudgetCodeAllocation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBudgetCodeAllocation.CurrentPageIndex + 1;
        BindBudgetCode();
    }

    protected void ucApportionmentMethod_TextChangedEvent(object sender, EventArgs e)
    {
        UserControlHard ddl = (UserControlHard)sender;
        GridDataItem row = (GridDataItem)ddl.Parent.Parent;

        UserControlHard uc = row.FindControl("ucApportionmentMethod") as UserControlHard;

        ViewState["UCAPPORTIONMETHOD"] = uc.SelectedHard;
    }
}
