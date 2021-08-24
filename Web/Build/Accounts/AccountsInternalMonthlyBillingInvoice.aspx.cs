using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using Telerik.Web.UI;


public partial class AccountsInternalMonthlyBillingInvoice : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvMonthlyBillingInvoice.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvMonthlyBillingInvoice.UniqueID, "Edit$" + r.RowIndex.ToString());
    //        }
    //    }
    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddImageButton("../Accounts/AccountsInternalMonthlyBillingInvoice.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvMonthlyBillingInvoice')", "Print Grid", "icon_print.png", "PRINT");
        MenuCrew.AccessRights = this.ViewState;
        MenuCrew.MenuList = toolbar.Show();
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Vessels", "VESSEL");
        toolbar.AddButton("Line Items", "LINEITEM");
        toolbar.AddButton("Invoices", "INVOICE");
        MenuBudgetTab.AccessRights = this.ViewState;
        MenuBudgetTab.MenuList = toolbar.Show();

        MenuBudgetTab.SelectedMenuIndex = 2;

        if (!IsPostBack)
        {
            ucConfirm.Visible = false;
            

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            gvMonthlyBillingInvoice.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);


            if (Request.QueryString["PORTAGEBILLID"] != null && Request.QueryString["PORTAGEBILLID"].ToString() != "")
                ViewState["PORTAGEBILLID"] = Request.QueryString["PORTAGEBILLID"].ToString();
            else
                ViewState["PORTAGEBILLID"] = "";

            ViewState["VesselId"] = Request.QueryString["vesselId"];
            ViewState["StartDate"] = Request.QueryString["startDate"];
            ViewState["EndDate"] = Request.QueryString["endDate"];
        }
        BindData();
        BindVisitData();
        
    }

    protected void BudgetTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("LINEITEM"))
        {
            Response.Redirect("../Accounts/AccountsInternalMonthlyBillingLineItem.aspx?PORTAGEBILLID=" + ViewState["PORTAGEBILLID"] +"&vesselID=" + ViewState["VesselId"], false);
        }
        if (CommandName.ToUpper().Equals("VESSEL"))
        {
            Response.Redirect("../Accounts/AccountsInternalMonthlyBilling.aspx?PORTAGEBILLID=" + ViewState["PORTAGEBILLID"], false);
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSERIALNUMBER", "FLDBILLINGITEMDESCRIPTION", "FLDTOTALAMOUNT", "FLDVOUCHERNUMBER", "FLDVOUCHERDATE" };
        string[] alCaptions = {"SNo", "Billing Item", "Amount", "Voucher Number", "Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixAccountsInternalBilling.InvoiceSearch(
                                                           General.GetNullableGuid(ViewState["PORTAGEBILLID"].ToString())                                                           
                                                           , sortexpression
                                                           , sortdirection
                                                           , (int)ViewState["PAGENUMBER"]
                                                           , gvMonthlyBillingInvoice.PageSize
                                                           , ref iRowCount
                                                           , ref iTotalPageCount);

        General.SetPrintOptions("gvMonthlyBillingInvoice", "Voucher List", alCaptions, alColumns, ds);

        gvMonthlyBillingInvoice.DataSource = ds;
        gvMonthlyBillingInvoice.VirtualItemCount = iRowCount;


        if (ds.Tables[0].Rows.Count > 0)
        {
          

            if (ViewState["MONTHLYBILLINGITEMID"] == null)
            {
                ViewState["MONTHLYBILLINGITEMID"] = ds.Tables[0].Rows[0]["FLDMONTHLYBILLINGITEMID"].ToString();
            }
            //SetRowSelection();
        }
   
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvMonthlyBillingInvoice_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMonthlyBillingInvoice.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvVisit_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVisit.CurrentPageIndex + 1;
            BindVisitData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindVisitData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = PhoenixAccountsInternalBilling.InvoiceVisitSearch(Int32.Parse(ViewState["VesselId"].ToString())
                                                           , Convert.ToDateTime(ViewState["StartDate"].ToString())
                                                           , Convert.ToDateTime(ViewState["EndDate"].ToString())
                                                           , (int)ViewState["PAGENUMBER"]
                                                           , gvVisit.PageSize
                                                           , ref iRowCount
                                                           , ref iTotalPageCount
                                                           , General.GetNullableGuid(ViewState["PORTAGEBILLID"].ToString()));
        gvVisit.DataSource = ds;
        gvVisit.VirtualItemCount = iRowCount;



        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    gvVisit.DataSource = ds;
        //    gvVisit.DataBind();
        //}
      
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
   
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression;
        int? sortdirection = null;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSERIALNUMBER", "FLDBILLINGITEMDESCRIPTION", "FLDTOTALAMOUNT", "FLDVOUCHERNUMBER", "FLDVOUCHERDATE" };
        string[] alCaptions = { "SNo", "Billing Item", "Amount", "Voucher Number", "Date" };

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsInternalBilling.InvoiceSearch(
                                                        General.GetNullableGuid(ViewState["PORTAGEBILLID"].ToString())    
                                                      , sortexpression
                                                      , sortdirection
                                                      , (int)ViewState["PAGENUMBER"]
                                                      , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                      , ref iRowCount
                                                      , ref iTotalPageCount);

        General.ShowExcel("Voucher List", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void MenuCrew_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    protected void gvVisit_ItemDataBound(object sender, GridItemEventArgs e)
    {
    }
    //protected void gvVisit_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //}
    protected void gvMonthlyBillingInvoice_ItemDataBound(object sender, GridItemEventArgs e)
    {
    }
    //protected void gvMonthlyBillingInvoice_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{

    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        if (ViewState["SORTEXPRESSION"] != null)
    //        {
    //            HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //            if (img != null)
    //            {
    //                if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                    img.Src = Session["images"] + "/arrowUp.png";
    //                else
    //                    img.Src = Session["images"] + "/arrowDown.png";

    //                img.Visible = true;
    //            }
    //        }
    //    }        
    //}
    protected void Rebind()
    {
        gvMonthlyBillingInvoice.SelectedIndexes.Clear();
        gvMonthlyBillingInvoice.EditIndexes.Clear();
        gvMonthlyBillingInvoice.DataSource = null;
        gvMonthlyBillingInvoice.Rebind();
    }
    protected void gvVisit_ItemCommand(object sender, GridCommandEventArgs e)
    {
    }
    protected void gvMonthlyBillingInvoice_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                RadLabel lblMonthlyBillingEmployeeId = ((RadLabel)e.Item.FindControl("lblMonthlyBillingEmployeeId"));
                if (lblMonthlyBillingEmployeeId != null)
                    ViewState["MONTHLYBILLINGITEMID"] = lblMonthlyBillingEmployeeId.Text;
                else
                    ViewState["MONTHLYBILLINGITEMID"] = "";

                if (ViewState["PORTAGEBILLID"] != null && ViewState["PORTAGEBILLID"].ToString() != "" && ViewState["MONTHLYBILLINGITEMID"] != null && ViewState["MONTHLYBILLINGITEMID"].ToString() != "")
                {
                    ucConfirm.Visible = true;
                    ucConfirm.Text = "Do you want to delete this Crew '" + ViewState["EMPLOYEENAME"] + "' from this 'Billing Item'?";
                    return;
                }
                Rebind();

            }

            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                int iRowno = e.Item.ItemIndex;
               // int iRowno = int.Parse(e.CommandArgument.ToString());
                 BindCurentValue(iRowno);
                Rebind();

            }


        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    //protected void gvMonthlyBillingInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    if (e.CommandName.ToUpper().Equals("SORT"))
    //        return;
    //    int iRowno = int.Parse(e.CommandArgument.ToString());

    //    if (e.CommandName.ToUpper().Equals("SELECT"))
    //    {
    //        BindCurentValue(iRowno);
    //    }
    //    if (e.CommandName.ToUpper().Equals("DELETE"))
    //    {
    //        try
    //        {
    //            Label lblMonthlyBillingEmployeeId = ((Label)gvMonthlyBillingInvoice.Rows[iRowno].FindControl("lblMonthlyBillingEmployeeId"));
    //            if (lblMonthlyBillingEmployeeId != null)
    //                ViewState["MONTHLYBILLINGITEMID"] = lblMonthlyBillingEmployeeId.Text;
    //            else
    //                ViewState["MONTHLYBILLINGITEMID"] = "";               

    //            if (ViewState["PORTAGEBILLID"] != null && ViewState["PORTAGEBILLID"].ToString() != "" && ViewState["MONTHLYBILLINGITEMID"] != null && ViewState["MONTHLYBILLINGITEMID"].ToString() != "" )
    //            {
    //                ucConfirm.Visible = true;
    //                ucConfirm.Text = "Do you want to delete this Crew '" + ViewState["EMPLOYEENAME"] + "' from this 'Billing Item'?";
    //                return;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            ucError.ErrorMessage = ex.Message;
    //            ucError.Visible = true;
    //            return;
    //        }
    //    }
    //}

    //protected void gvMonthlyBillingInvoice_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    GridView _gridView = (GridView)sender;

    //    _gridView.EditIndex = de.NewEditIndex;
    //    _gridView.SelectedIndex = de.NewEditIndex;

    //    BindData();
    //    SetPageNavigator();
    //   }

    //protected void gvMonthlyBillingInvoice_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;

    //        gvMonthlyBillingInvoice.EditIndex = -1;
    //        BindData();

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvMonthlyBillingInvoice_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    BindData();
    //}

    //protected void gvMonthlyBillingInvoice_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    BindData();
    //    SetPageNavigator();
    //}
 
    protected void gvMonthlyBillingInvoice_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvMonthlyBillingInvoice.SelectedIndexes.Add(e.NewSelectedIndex);
        BindCurentValue(e.NewSelectedIndex);

    }
 
    protected void gvMonthlyBillingInvoice_SortCommand(object sender, GridSortCommandEventArgs e)
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
    private void BindCurentValue(int rowindex)
    {
        try
        {
            GridDataItem item = (GridDataItem)gvMonthlyBillingInvoice.Items[rowindex];
            RadLabel lblMonthlyBillingItemId = ((RadLabel)gvMonthlyBillingInvoice.Items[rowindex].FindControl("lblMonthlyBillingItemId"));
            if (lblMonthlyBillingItemId != null)
                ViewState["MONTHLYBILLINGITEMID"] = lblMonthlyBillingItemId.Text;
            else
                ViewState["MONTHLYBILLINGITEMID"] = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {

            UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            if (ucCM.confirmboxvalue == 1)
            {
                //PhoenixAccountsInternalBilling.BillingItemEmployeeListUpdate(
                //                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                //                                               , new Guid(ViewState["PORTAGEBILLID"].ToString())
                //                                               , new Guid(ViewState["MONTHLYBILLINGITEMID"].ToString())
                //                                               , new Guid(ViewState["MONTHLYBILLINGITEMID"].ToString()));
                //ViewState["MONTHLYBILLINGITEMID"] = null;
                //BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    //protected void cmdSort_Click(object sender, EventArgs e)
    //{
    //    ImageButton ib = (ImageButton)sender;
    //    try
    //    {
    //        ViewState["SORTEXPRESSION"] = ib.CommandName;
    //        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
    //        BindData();
    //        SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

  
}
