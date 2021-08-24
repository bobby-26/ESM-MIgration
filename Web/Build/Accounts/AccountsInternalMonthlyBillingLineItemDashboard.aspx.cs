using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Accounts_AccountsInternalMonthlyBillingLineItemDashboard : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            ViewState["PORTAGEBILLID"] = "";
            ViewState["VesselId"] = "";
            ViewState["INTERNALMONTHLYBILLINGID"] = "";

            if (Request.QueryString["pbid"] != null && Request.QueryString["pbid"].ToString() != "")
                ViewState["PORTAGEBILLID"] = Request.QueryString["pbid"].ToString();

            if (Request.QueryString["vslid"] != null && Request.QueryString["vslid"].ToString() != "")
                ViewState["VesselId"] = Request.QueryString["vslid"].ToString();

            if (Request.QueryString["IMBID"] != null && Request.QueryString["IMBID"].ToString() != "")
                ViewState["INTERNALMONTHLYBILLINGID"] = Request.QueryString["IMBID"].ToString();

            //if (Request.QueryString["PORTAGEBILLID"] != null && Request.QueryString["PORTAGEBILLID"].ToString() != "")
            //    ViewState["PORTAGEBILLID"] = Request.QueryString["PORTAGEBILLID"].ToString();
            //else
            //if (Request.QueryString["internalmonthlybillingid"] != null && Request.QueryString["internalmonthlybillingid"].ToString() != "")
            //    ViewState["INTERNALMONTHLYBILLINGID"] = Request.QueryString["internalmonthlybillingid"].ToString();
            //else
            //    ViewState["INTERNALMONTHLYBILLINGID"] = "";
            //   ViewState["VesselId"] = Request.QueryString["vesselId"];
            //ucConfirm.Visible = false;
            //ucConfirmPost.Visible = false;

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            ViewState["PAGENUMBERSUB"] = 1;
            ViewState["SORTEXPRESSIONSUB"] = null;
            ViewState["SORTDIRECTIONSUB"] = null;

            ViewState["PAGENUMBERIT"] = 1;
            ViewState["PAGENUMBERSUP"] = 1;
            ViewState["Visit"] = null;
            gvMonthlyBillingCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvMonthlyBillingItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Accounts/AccountsInternalMonthlyBillingLineItem.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvMonthlyBillingCrew')", "Print Grid", "icon_print.png", "PRINT");
     //   toolbar.AddImageLink("javascript:openNewWindow('MoreInfo','','" + Session["sitepath"] + "/Accounts/AccountsInternalMonthlyBillingDeletedEmployeeList.aspx?PORTAGEBILLID=" + ViewState["PORTAGEBILLID"] + "'); return false;", "Add", "add.png", "ADD");
        MenuCrew.AccessRights = this.ViewState;
        MenuCrew.MenuList = toolbar.Show();
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");

        toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Accounts/AccountsInternalMonthlyBillingLineItem.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvMonthlyBillingItem')", "Print Grid", "icon_print.png", "PRINT");
        toolbar.AddImageButton("../Accounts/AccountsInternalMonthlyBillingLineItem.aspx", "Refresh", "refresh.png", "REFRESH");

        MenuBillingItem.AccessRights = this.ViewState;
        MenuBillingItem.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Vessels", "VESSEL");
        toolbar.AddButton("Line Items", "LINEITEM");
        toolbar.AddButton("Invoices", "INVOICE");
        //toolbar.AddButton("As Incurred Billings", "INCURREDBILLINGS");

        MenuBudgetTab.AccessRights = this.ViewState;
        MenuBudgetTab.MenuList = toolbar.Show();
        MenuBudgetTab.SelectedMenuIndex = 1;

    }

    protected void BudgetTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("VESSEL"))
        {
            Response.Redirect("../Accounts/AccountsInternalMonthlyBilling.aspx?PORTAGEBILLID=" + ViewState["PORTAGEBILLID"], false);
        }
        if (CommandName.ToUpper().Equals("INVOICE"))
        {
            Response.Redirect("../Accounts/AccountsInternalMonthlyBillingInvoice.aspx?PORTAGEBILLID=" + ViewState["PORTAGEBILLID"] + "&vesselId=" + ViewState["VesselId"].ToString() + "&startDate=" + txtPortagebillStartDate.Text + "&endDate=" + txtPortagebillEndDate.Text, false);
        }
    }

    private void BindPortageBillData()
    {
        if (ViewState["PORTAGEBILLID"] != null && ViewState["PORTAGEBILLID"].ToString() != "")
        {
            DataSet ds = new DataSet();
            ds = PhoenixAccountsInternalBilling.PortageBillEdit(new Guid(ViewState["PORTAGEBILLID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtPortagebillStatus.Text = ds.Tables[0].Rows[0]["FLDPORTAGEBILLSTATUS"].ToString();
                txtPortagebillStartDate.Text = ds.Tables[0].Rows[0]["FLDFROMDATE"].ToString();
                txtPortagebillEndDate.Text = ds.Tables[0].Rows[0]["FLDTODATE"].ToString();
            }
        }
    }

    private void BindCrewData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSERIALNUMBER", "FLDRANKCODE", "FLDEMPLOYEENAME", "FLDSIGNONDATE", "FLDSIGNOFFDATE" };
        string[] alCaptions = { "Serial Number", "Rank", "Name", "Sign On Date", "Sign Off Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixAccountsInternalBilling.EmployeeSearch(
                                                            General.GetNullableGuid(ViewState["PORTAGEBILLID"].ToString())
                                                           , sortexpression
                                                           , sortdirection
                                                           , (int)ViewState["PAGENUMBER"]
                                                           , gvMonthlyBillingCrew.PageSize
                                                           , ref iRowCount
                                                           , ref iTotalPageCount);

        General.SetPrintOptions("gvMonthlyBillingCrew", "Crew List", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["MONTHLYBILLINGEMPLOYEEID"] == null)
            {
                ViewState["MONTHLYBILLINGEMPLOYEEID"] = ds.Tables[0].Rows[0]["FLDMONTHLYBILLINGEMPLOYEEID"].ToString();
                ViewState["POSTEDYN"] = ds.Tables[0].Rows[0]["FLDPOSTEDYN"].ToString();
                if (ViewState["POSTEDYN"] != null && ViewState["POSTEDYN"].ToString() == "1")
                    gvMonthlyBillingCrew.Columns[5].Visible = false;
            }
        }
        gvMonthlyBillingCrew.DataSource = ds;
        gvMonthlyBillingCrew.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void BindBillingItemData()
    {
        int iRowCountBilling = 0;
        int iTotalPageCountBilling = 0;

        string[] alColumns = { "FLDBILLINGITEMDESCRIPTION", "FLDVESSELBUDGETCODE", "FLDOWNERBUDGETCODE", "FLDBILLINGUNITNAME", "FLDBILLINGITEMRATE", "FLDTOTALAMOUNT" };
        string[] alCaptions = { "Billing Item", "Vessel Budget Code", "Owner Budget Code", "Unit Name", "Rate", "Amount" };

        string sortexpression = (ViewState["SORTEXPRESSIONSUB"] == null) ? null : (ViewState["SORTEXPRESSIONSUB"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTIONSUB"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONSUB"].ToString());

        if (ViewState["ROWCOUNTSUB"] == null || Int32.Parse(ViewState["ROWCOUNTSUB"].ToString()) == 0)
            iRowCountBilling = 10;
        else
            iRowCountBilling = Int32.Parse(ViewState["ROWCOUNTSUB"].ToString());

        DataSet ds = PhoenixAccountsInternalBilling.BillingItemSearch(
                                                                 General.GetNullableGuid(ViewState["PORTAGEBILLID"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBERSUB"]
                                                                , gvMonthlyBillingItem.PageSize
                                                                , ref iRowCountBilling
                                                                , ref iTotalPageCountBilling);

        General.SetPrintOptions("gvMonthlyBillingItem", "Billing Item List", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {

            if (ViewState["MONTHLYBILLINGITEMID"] == null)
            {
                ViewState["MONTHLYBILLINGITEMID"] = ds.Tables[0].Rows[0]["FLDMONTHLYBILLINGITEMID"].ToString();
                ViewState["VESSELBUDGETALLOCATIONID"] = ds.Tables[0].Rows[0]["FLDVESSELBUDGETALLOCATIONID"].ToString();
                ViewState["BUDGETBILLINGID"] = ds.Tables[0].Rows[0]["FLDBUDGETBILLINGID"].ToString();
                ViewState["COMPANYID"] = ds.Tables[0].Rows[0]["FLDCOMPANYID"].ToString();
                ViewState["POSTED"] = ds.Tables[0].Rows[0]["FLDISPOSTED"].ToString();
                string str = ds.Tables[0].Rows[0]["FLDPOSTALLVISIBLEYN"].ToString();
                if (str != null && str.ToString() == "1")
                    gvMonthlyBillingItem.Columns[7].Visible = false;
            }
        }

        gvMonthlyBillingItem.DataSource = ds;
        gvMonthlyBillingItem.VirtualItemCount = iRowCountBilling;
        ViewState["ROWCOUNTSUB"] = iRowCountBilling;
        ViewState["TOTALPAGECOUNTSUB"] = iTotalPageCountBilling;
    }

    protected void ShowCrewExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression;
        int? sortdirection = null;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSERIALNUMBER", "FLDRANKCODE", "FLDEMPLOYEENAME", "FLDSIGNONDATE", "FLDSIGNOFFDATE" };
        string[] alCaptions = { "Serial Number", "Rank", "Name", "Sign On Date", "Sign Off Date" };

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsInternalBilling.EmployeeSearch(
                                                      General.GetNullableGuid(ViewState["PORTAGEBILLID"].ToString())
                                                      , sortexpression
                                                      , sortdirection
                                                      , (int)ViewState["PAGENUMBER"]
                                                      , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                      , ref iRowCount
                                                      , ref iTotalPageCount);

        General.ShowExcel("Crew List", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void ShowBillingItemExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression;
        int? sortdirection = null;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDBILLINGITEMDESCRIPTION", "FLDVESSELBUDGETCODE", "FLDOWNERBUDGETCODE", "FLDBILLINGUNITNAME", "FLDBILLINGITEMRATE", "FLDTOTALAMOUNT" };
        string[] alCaptions = { "Billing Item", "Vessel Budget Code", "Owner Budget Code", "Unit Name", "Rate", "Amount" };

        sortexpression = (ViewState["SORTEXPRESSIONSUB"] == null) ? null : (ViewState["SORTEXPRESSIONSUB"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONSUB"].ToString());

        if (ViewState["ROWCOUNTSUB"] == null || Int32.Parse(ViewState["ROWCOUNTSUB"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNTSUB"].ToString());

        ds = PhoenixAccountsInternalBilling.BillingItemSearch(
                                                              General.GetNullableGuid(ViewState["PORTAGEBILLID"].ToString())
                                                             , sortexpression
                                                             , sortdirection
                                                             , (int)ViewState["PAGENUMBERSUB"]
                                                             , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                             , ref iRowCount
                                                             , ref iTotalPageCount);

        General.ShowExcel("Billing Item List", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void MenuCrew_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowCrewExcel();
        }
    }

    protected void MenuBillingItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowBillingItemExcel();
            }
            if (CommandName.ToUpper().Equals("REFRESH"))
            {
                if (General.GetNullableGuid(ViewState["INTERNALMONTHLYBILLINGID"].ToString()) != null)
                {
                    PhoenixAccountsInternalBilling.MonthlyBillingItemInsert(
                                                      PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , new Guid(ViewState["PORTAGEBILLID"].ToString())
                                                    , new Guid(ViewState["INTERNALMONTHLYBILLINGID"].ToString()));

                    ucStatus.Text = "Billing Item information is refreshed.";
                    ViewState["MONTHLYBILLINGITEMID"] = null;
                    Rebind();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvMonthlyBillingCrew_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
        }
    }

    protected void gvMonthlyBillingCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            int iRowno = e.Item.ItemIndex;
            BindCurentCrewValue(iRowno);
            RebindCrew();
        }
        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            try
            {
                GridDataItem item = (GridDataItem)e.Item;
                int iRowno = e.Item.ItemIndex;
                BindCurentCrewValue(iRowno);
                PhoenixAccountsInternalBilling.EmployeeDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                     , new Guid(ViewState["PORTAGEBILLID"].ToString())
                                                                     , new Guid(ViewState["MONTHLYBILLINGEMPLOYEEID"].ToString()));
                ViewState["MONTHLYBILLINGEMPLOYEEID"] = null;
                RebindCrew();
            }

            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
            // try
            // {
            //     GridDataItem item = (GridDataItem)e.Item;
            //     int iRowno = e.Item.ItemIndex;
            //     BindCurentCrewValue(iRowno);
            //     //if (ViewState["PORTAGEBILLID"] != null && ViewState["PORTAGEBILLID"].ToString() != "" && ViewState["MONTHLYBILLINGEMPLOYEEID"] != null && ViewState["MONTHLYBILLINGEMPLOYEEID"].ToString() != "")
            //     //{
            //
            //         //ucConfirm.Text = "Do you want to delete this Crew '" + ViewState["EMPLOYEENAME"] + "' from the all the 'Billing Items'?";
            //         RadWindowManager1.RadConfirm("Do you want to delete this Crew from the all the 'Billing Items?", "DeleteRecord", 320, 150, null, "Delete");
            //         //return;
            //     //}
            //    // RebindCrew();
            // }
            // catch (Exception ex)
            // {
            //     ucError.ErrorMessage = ex.Message;
            //     ucError.Visible = true;
            //     return;
            // }
        }
    }

    protected void gvMonthlyBillingCrew_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvMonthlyBillingCrew.SelectedIndexes.Add(e.NewSelectedIndex);
        BindCurentCrewValue(e.NewSelectedIndex);

    }


    protected void gvMonthlyBillingItem_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            string posted = "0";
            RadLabel lblMonthlyBillingItemId = (RadLabel)e.Item.FindControl("lblMonthlyBillingItemId");
            RadLabel lblPortageBillId = (RadLabel)e.Item.FindControl("lblPortageBillId");
            RadLabel lblIsPosted = ((RadLabel)e.Item.FindControl("lblIsPosted"));
            LinkButton lnkBillingItem = (LinkButton)e.Item.FindControl("lnkBillingItem");
            if (lblIsPosted != null)
                posted = lblIsPosted.Text;
            if (lnkBillingItem != null)
            {
                /*if (!SessionUtil.CanAccess(this.ViewState, lnkBillingItem.CommandName)) lnkBillingItem.Visible = false;
                if (lblMonthlyBillingItemId != null && lblPortageBillId != null && lblIsPosted != null)                                    
                    lnkBillingItem.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', 'AccountsInternalMonthlyBillingEmployeeList.aspx?PORTAGEBILLID=" + lblPortageBillId.Text + "&MONTHLYBILLINGITEMID=" + lblMonthlyBillingItemId.Text + "&POSTED=" + lblIsPosted.Text + "');return false;");               
                 */
            }
            ImageButton cmdPost = (ImageButton)e.Item.FindControl("cmdPost");
            if (cmdPost != null)
            {
                if (posted == "1")
                    cmdPost.Visible = false;
                if (!SessionUtil.CanAccess(this.ViewState, cmdPost.CommandName))
                    cmdPost.Visible = false;
            }
        }
    }

    protected void gvMonthlyBillingItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            try
            {
                GridDataItem item = (GridDataItem)e.Item;
                int iRowno = e.Item.ItemIndex;
                BindCurentItemValue(iRowno);
                Response.Redirect("../Accounts/AccountsInternalMonthlyBillingEmployeeList.aspx?PORTAGEBILLID=" + ViewState["PORTAGEBILLID"].ToString() + "&MONTHLYBILLINGITEMID=" + ViewState["MONTHLYBILLINGITEMID"].ToString() + "&POSTED=" + ViewState["POSTED"] + "&internalmonthlybillingid=" + ViewState["INTERNALMONTHLYBILLINGID"] + "&vesselId=" + ViewState["VesselId"].ToString(), false);
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
        }
        if (e.CommandName.ToUpper().Equals("SAVE"))
        {
            try
            {
                GridDataItem item = (GridDataItem)e.Item;
                int iRowno = e.Item.ItemIndex;
                BindCurentItemValue(iRowno);
                PhoenixAccountsInternalBilling.RefreshTotalAmount(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , new Guid(ViewState["PORTAGEBILLID"].ToString())
                                                                    , new Guid(ViewState["MONTHLYBILLINGITEMID"].ToString())
                                                                    , decimal.Parse(((RadTextBox)e.Item.FindControl("txtBillingItemApportionment")).Text));

                Rebind();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
        }
        if (e.CommandName.ToUpper().Equals("POST"))
        {
            try
            {
                GridDataItem item = (GridDataItem)e.Item;
                int iRowno = e.Item.ItemIndex;
                if (ViewState["PORTAGEBILLID"] != null && ViewState["PORTAGEBILLID"].ToString() != "")
                {
                    BindCurentItemValue(iRowno);
                    ViewState["POSTALL"] = 0;
                    //ucConfirmPost.Visible = true;
                    //ucConfirmPost.Text = "Do you want to post the voucher for this 'Billing Item'?";
                    RadWindowManager1.RadConfirm("Do you want to post the voucher for this 'Billing Item'?", "PostRecord", 320, 150, null, "Delete");
                    return;
                }

            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
        }
        if (e.CommandName.ToUpper().Equals("POSTALL"))
        {
            ViewState["POSTALL"] = "1";
            if (ViewState["PORTAGEBILLID"] != null && ViewState["PORTAGEBILLID"].ToString() != "")
            {
                //ucConfirmPost.Visible = true;
                //ucConfirmPost.Text = "Do you want to post the voucher for all the 'Billing Items'.?";
                RadWindowManager1.RadConfirm("Do you want to post the voucher for all the 'Billing Item'?", "PostRecord", 320, 150, null, "Post");

                return;
            }
            //cmdPostAll
        }

        //  gvMonthlyBillingItem.EditIndex = -1;
        Rebind();
    }

    protected void gvMonthlyBillingItem_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvMonthlyBillingItem.SelectedIndexes.Add(e.NewSelectedIndex);
        BindCurentItemValue(e.NewSelectedIndex);

    }

    private void BindCurentCrewValue(int rowindex)
    {
        try
        {
            RadLabel lblMonthlyBillingEmployeeId = ((RadLabel)gvMonthlyBillingCrew.Items[rowindex].FindControl("lblMonthlyBillingEmployeeId"));
            RadLabel lblName = ((RadLabel)gvMonthlyBillingCrew.Items[rowindex].FindControl("lblName"));
            RadLabel lblPostedYN = ((RadLabel)gvMonthlyBillingCrew.Items[rowindex].FindControl("lblPostedYN"));

            if (lblMonthlyBillingEmployeeId != null)
                ViewState["MONTHLYBILLINGEMPLOYEEID"] = lblMonthlyBillingEmployeeId.Text;
            else
                ViewState["MONTHLYBILLINGEMPLOYEEID"] = "";
            if (lblName != null)
                ViewState["EMPLOYEENAME"] = lblName.Text;
            if (lblPostedYN != null)
                ViewState["POSTEDYN"] = lblPostedYN.Text;
            if (ViewState["POSTEDYN"] != null && ViewState["POSTEDYN"].ToString() == "1")
                gvMonthlyBillingCrew.Columns[5].Visible = false;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindCurentItemValue(int rowindex)
    {
        try
        {
            RadLabel lblMonthlyBillingItemId = ((RadLabel)gvMonthlyBillingItem.Items[rowindex].FindControl("lblMonthlyBillingItemId"));
            RadLabel lblBudgetBillingId = ((RadLabel)gvMonthlyBillingItem.Items[rowindex].FindControl("lblBudgetBillingId"));
            RadLabel lblVesselBudgetAllocationId = ((RadLabel)gvMonthlyBillingItem.Items[rowindex].FindControl("lblVesselBudgetAllocationId"));
            RadLabel lblIsPosted = ((RadLabel)gvMonthlyBillingItem.Items[rowindex].FindControl("lblIsPosted"));
            RadLabel lblCompanyId = ((RadLabel)gvMonthlyBillingItem.Items[rowindex].FindControl("lblCompanyId"));
            RadLabel lblPostAllVisibleYN = ((RadLabel)gvMonthlyBillingItem.Items[rowindex].FindControl("lblPostAllVisibleYN"));

            if (lblMonthlyBillingItemId != null)
            {
                ViewState["VisitID"] = null;
                ViewState["MONTHLYBILLINGITEMID"] = lblMonthlyBillingItemId.Text;
                ViewState["Visit"] = null;
            }
            if (lblBudgetBillingId != null)
                ViewState["BUDGETBILLINGID"] = lblBudgetBillingId.Text;
            if (lblVesselBudgetAllocationId != null)
                ViewState["VESSELBUDGETALLOCATIONID"] = lblVesselBudgetAllocationId.Text;
            else
                ViewState["VESSELBUDGETALLOCATIONID"] = "";
            if (lblIsPosted != null)
                ViewState["POSTED"] = lblIsPosted.Text;
            if (lblCompanyId != null)
                ViewState["COMPANYID"] = lblCompanyId.Text;
            if (lblPostAllVisibleYN != null && lblPostAllVisibleYN.Text == "1")
                gvMonthlyBillingItem.Columns[6].Visible = false;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    // protected void ucConfirmDelete_Click(object sender, EventArgs e)
    // {
    //     try
    //     {
    //         PhoenixAccountsInternalBilling.EmployeeDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                                                              , new Guid(ViewState["PORTAGEBILLID"].ToString())
    //                                                              , new Guid(ViewState["MONTHLYBILLINGEMPLOYEEID"].ToString()));
    //         ViewState["MONTHLYBILLINGEMPLOYEEID"] = null;
    //         RebindCrew();
    //     }
    //
    //     catch (Exception ex)
    //     {
    //         ucError.ErrorMessage = ex.Message;
    //         ucError.Visible = true;
    //         return;
    //     }
    // }

    protected void btnConfirmPost_Click(object sender, EventArgs e)
    {
        try
        {

            string voucherdate = "";
            int periodlockedyn = 0;

            if (ViewState["POSTALL"].ToString() == "1")
            {
                FinancialYearCheck(txtPortagebillEndDate.Text.Trim(), ViewState["COMPANYID"].ToString(), 1, null, ref periodlockedyn);

                /* If the Period is locked, will open a popup to capture the voucher 
                    date for posting other wise will go directly for posting. */

                if (periodlockedyn != 2)
                {
                    PhoenixAccountsInternalBilling.VoucherPostAll(new Guid(ViewState["PORTAGEBILLID"].ToString()), General.GetNullableDateTime(voucherdate));
                }


                ViewState["POSTALL"] = "0";
            }
            else
            {
                FinancialYearCheck(txtPortagebillEndDate.Text.Trim(), ViewState["COMPANYID"].ToString(), 0, ViewState["BUDGETBILLINGID"].ToString(), ref periodlockedyn);

                if (periodlockedyn != 2)
                {
                    if (ViewState["Visit"] == null)
                    {
                        PhoenixAccountsInternalBilling.VoucherPost(
                                                                   PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                  , new Guid(ViewState["PORTAGEBILLID"].ToString())
                                                                  , new Guid(ViewState["MONTHLYBILLINGITEMID"].ToString())
                                                                  , new Guid(ViewState["BUDGETBILLINGID"].ToString())
                                                                  , new Guid(ViewState["VESSELBUDGETALLOCATIONID"].ToString())
                                                                  , General.GetNullableDateTime(voucherdate));

                        ViewState["POSTED"] = 1;
                    }
                    else
                    {
                        Guid iVoucherDtKey = new Guid();
                        int iVisitType = 0;
                        PhoenixAccountsInternalBilling.VoucherVisitPost(
                                                                   PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                  , new Guid(ViewState["PORTAGEBILLID"].ToString())
                                                                  , new Guid(ViewState["VisitID"].ToString())
                                                                  , new Guid(ViewState["VESSELBUDGETALLOCATIONID"].ToString())
                                                                  , Convert.ToDecimal(ViewState["Amount"].ToString())
                                                                  , General.GetNullableDateTime(voucherdate)
                                                                  , ref iVoucherDtKey
                                                                  , ref iVisitType);

                        if (iVoucherDtKey.ToString() != "00000000-0000-0000-0000-000000000000")
                            CreateReport(new Guid(ViewState["VisitID"].ToString()), iVoucherDtKey, iVisitType);
                        ViewState["POSTED"] = 1;
                    }
                }
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void FinancialYearCheck(string date, string companyid, int postall, string budgetbillingid, ref int periodlockedyn)
    {
        periodlockedyn = 0;
        DateTime temp = Convert.ToDateTime(date);
        PhoenixAccountsInternalBilling.FinancialYearCheck(
                                                            temp
                                                          , int.Parse(companyid)
                                                          , ref periodlockedyn);
        if (periodlockedyn == 2)
        {
            String scriptinsert = String.Format("javascript:parent.Openpopup('codehelp1','','../Accounts/AccountsInternalMonthlyBillingLineItemGeneral.aspx?portagebillid=" + ViewState["PORTAGEBILLID"] + "&budgetbillingid=" + General.GetNullableGuid(budgetbillingid) + "&postall=" + postall + "',null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
            return;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        RebindCrew();
        Rebind();
    }

    private void CreateReport(Guid visitID, Guid VoucherDtKey, int visitType)
    {
        Guid fileDtKey = new Guid();
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();

        if (visitType == 1)
        {
            ds = PhoenixAccountsInternalBilling.ItVisitReport(visitID, VoucherDtKey, PhoenixSecurityContext.CurrentSecurityContext.UserCode, ref fileDtKey);

            if (ds.Tables[0].Rows.Count > 0)
            {
                AddAttachment(ds, fileDtKey, visitType);
            }

            ds1 = PhoenixAccountsInternalBilling.ItVisitLineItemReport(visitID, VoucherDtKey, PhoenixSecurityContext.CurrentSecurityContext.UserCode, ref fileDtKey);

            if (ds1.Tables[0].Rows.Count > 0)
            {
                AddAttachmentLineItem(ds1, fileDtKey, visitType);
            }
        }
        else if (visitType == 2)
        {
            ds = PhoenixAccountsInternalBilling.SupVisitReport(visitID,
                                                                int.Parse(ViewState["SUPBUDGETEDDAYS"].ToString()),
                                                                int.Parse(ViewState["SUPCHARGEDDAYS"].ToString()),
                                                                int.Parse(ViewState["SUPCHARGINGDAYS"].ToString()),
                                                                decimal.Parse(ViewState["SUPTOTALAMOUNT"].ToString()),
                                                                decimal.Parse(ViewState["SUPBILLINGITEMRATE"].ToString()),
                                                                VoucherDtKey,
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                ref fileDtKey);
            if (ds.Tables[0].Rows.Count > 0)
            {
                AddAttachment(ds, fileDtKey, visitType);
            }
        }
    }

    private string AddAttachment(DataSet ds, Guid fileDtKey, int visitType)
    {
        FontFactory.RegisterDirectories();
        Font fontNormal = new Font(FontFactory.GetFont("Arial", 12, Font.NORMAL));


        //string path = HttpContext.Current.Request.MapPath("~/Attachments/Accounts/");
        string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/ACCOUNTS/";
        string filefullpath = path + fileDtKey + ".pdf";

        if (visitType == 1)
            ConvertToPdf(HtmlTableDataContent(ds), filefullpath);

        else if (visitType == 2)
            ConvertToPdf(HtmlTableDataContentSup(ds), filefullpath);

        return filefullpath;
    }

    private string AddAttachmentLineItem(DataSet ds, Guid fileDtKey, int visitType)
    {
        FontFactory.RegisterDirectories();
        Font fontNormal = new Font(FontFactory.GetFont("Arial", 12, Font.NORMAL));


        //string path = HttpContext.Current.Request.MapPath("~/Attachments/Accounts/");
        string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/ACCOUNTS/";
        string filefullpath = path + fileDtKey + ".pdf";

        if (visitType == 1)
            ConvertToPdf(HtmlTableDataContentLineItem(ds), filefullpath);

        return filefullpath;
    }


    private string HtmlTableDataContent(DataSet ds)
    {
        StringBuilder sbHtmlContent = new StringBuilder();
        sbHtmlContent.Append("<div style='font-size: 10px'>");
        sbHtmlContent.Append("<div align='left'>");
        sbHtmlContent.Append("<table ID='tbl1'>");

        sbHtmlContent.Append("</table>");
        sbHtmlContent.Append("</div>");
        sbHtmlContent.Append("<br/>");

        sbHtmlContent.Append("<div align='right'>");
        sbHtmlContent.Append("<b>Date:</b>");
        sbHtmlContent.Append(General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDVOUCHERDATE"].ToString()));
        sbHtmlContent.Append("</div>");

        sbHtmlContent.Append("<div align='left'>");
        sbHtmlContent.Append("<b>TO: CAPTAIN AND OWNERS OF </b>");
        sbHtmlContent.Append("<i>" + ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString() + "</i>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<b>WE HAVE DEBITED YOUR ACCOUNT WITH US FOR THE FOLLOWING:</b>");
        sbHtmlContent.Append("</div>");
        sbHtmlContent.Append("<br/>");

        sbHtmlContent.Append("<table ID='tbl1' border='1'>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td colspan='4'  align='center' style=\"width:6000px\">");
        sbHtmlContent.Append("<b>PARTICULARS</b>");
        sbHtmlContent.Append("</td>");
        if (ds.Tables[0].Rows[0]["FLDLUMPSUM"].ToString() != "1")
        {
            sbHtmlContent.Append("<td align='center' style=\"width:1000px\">");
            sbHtmlContent.Append("<b>No of Hours</b>");
            sbHtmlContent.Append("</td>");

            sbHtmlContent.Append("<td align='center' style=\"width:1000px\">");
            sbHtmlContent.Append("<b>Rate / Hours</b>");
            sbHtmlContent.Append("<br/>");
            sbHtmlContent.Append("<i>(" + ds.Tables[0].Rows[0]["FLDCURRENCYNAME"].ToString() + ")</i>");
            sbHtmlContent.Append("</td>");
        }
        sbHtmlContent.Append("<td align='center' style=\"width:2000px\">");
        sbHtmlContent.Append("<b>AMOUNT</b>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<i>(" + ds.Tables[0].Rows[0]["FLDCURRENCYNAME"].ToString() + ")</i>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");

        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td colspan='4' style=\"font-size:9px;\" >");
        sbHtmlContent.Append("BEING IT RELATED SERVICES PROVIDED AS PER ATTACHED REPORT AND TIMINGS MENTIONED AS BELOW:");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");

        sbHtmlContent.Append("<table ID='tbl2' width='100%' border='0'>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td colspan='20' style=\"font-size:9px;\" >");
        sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDEMPLOYEENAME"].ToString());
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDFROMDATE"].ToString() + " - " + ds.Tables[0].Rows[0]["FLDTODATE"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td style=\"font-size:9px;\" >");
        if (ds.Tables[0].Rows[0]["FLDLUMPSUM"].ToString() != "1")
        {
            sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDNOOFHOURS"].ToString());
        }
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");

        if (ds.Tables[0].Rows[0]["FLDLUMPSUM"].ToString() != "1")
        {
            sbHtmlContent.Append("<tr>");
            sbHtmlContent.Append("<td colspan='20' style=\"font-size:9px;\" >");
            sbHtmlContent.Append("Travel Hours:");
            sbHtmlContent.Append("</td>");
            sbHtmlContent.Append("<td style=\"font-size:9px;\" >");
            sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDTRAVELHOURS"].ToString());
            sbHtmlContent.Append("<br/>");
            sbHtmlContent.Append("</td>");
            sbHtmlContent.Append("</tr>");

            sbHtmlContent.Append("<tr>");
            sbHtmlContent.Append("<td colspan='20' style=\"font-size:9px;\" >");
            sbHtmlContent.Append("Total No of Hours :");
            sbHtmlContent.Append("</td>");
            sbHtmlContent.Append("<td style=\"font-size:9px;\" >");
            sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDTOTALNOOFHOURS"].ToString());
            sbHtmlContent.Append("<br/>");
            sbHtmlContent.Append("</td>");
            sbHtmlContent.Append("</tr>");
        }
        sbHtmlContent.Append("</table>");

        sbHtmlContent.Append("</td>");
        if (ds.Tables[0].Rows[0]["FLDLUMPSUM"].ToString() != "1")
        {
            sbHtmlContent.Append("<td style=\"font-size:9px;\">");
            sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDTOTALNOOFHOURS"].ToString());
            sbHtmlContent.Append("</td>");
            sbHtmlContent.Append("<td style=\"font-size:9px;\">");
            sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDBILLINGITEMRATE"].ToString());
            sbHtmlContent.Append("</td>");
        }
        sbHtmlContent.Append("<td style=\"font-size:9px;\">");
        sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDTOTALAMOUNT"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        if (ds.Tables[0].Rows[0]["FLDLUMPSUM"].ToString() != "1")
        {
            sbHtmlContent.Append("<tr>");
            sbHtmlContent.Append("<td colspan='4'>");
            sbHtmlContent.Append("");
            sbHtmlContent.Append("</td>");
            sbHtmlContent.Append("<td colspan='2' style=\"font-size:9px;\">");
            sbHtmlContent.Append("<b>");
            sbHtmlContent.Append("TOTAL AMOUNT");
            sbHtmlContent.Append("</b>");
            sbHtmlContent.Append("</td>");
            sbHtmlContent.Append("<td style=\"font-size:9px;\">");
            sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDTOTALAMOUNT"].ToString());
            sbHtmlContent.Append("</td>");
            sbHtmlContent.Append("</tr>");
        }
        else
        {
            sbHtmlContent.Append("<tr>");
            sbHtmlContent.Append("<td colspan='4'style=\"font-size:9px;\" align=\"right\">");
            sbHtmlContent.Append("<b>");
            sbHtmlContent.Append("TOTAL AMOUNT");
            sbHtmlContent.Append("</b>");
            sbHtmlContent.Append("</td>");
            sbHtmlContent.Append("<td style=\"font-size:9px;\">");
            sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDTOTALAMOUNT"].ToString());
            sbHtmlContent.Append("</td>");
            sbHtmlContent.Append("</tr>");

        }

        sbHtmlContent.Append("</table>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<div align='left'>");
        sbHtmlContent.Append("FINANCE DIRECTOR ");
        sbHtmlContent.Append("</div>");
        sbHtmlContent.Append("</div>");
        return sbHtmlContent.ToString();
    }

    private string HtmlTableDataContentLineItem(DataSet ds)
    {
        StringBuilder sbHtmlContent = new StringBuilder();

        string[] alColumns = { "FLDSNO", "FLDDESCRIPTION", "FLDCOMPLETED", "FLDHARDNAME" };
        string[] alCaptions = { "S.No.", "Description", "Completed Status", "Grading" };

        DataTable dt = ds.Tables[1];
        sbHtmlContent.Append("<div style='font-size: 10px'>");
        sbHtmlContent.Append("<div align='left'>");

        sbHtmlContent.Append("</div>");
        sbHtmlContent.Append("<br/>");


        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<table ID='tbl1'>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td>");
        sbHtmlContent.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
        sbHtmlContent.Append("Vessel Name:");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td>");
        sbHtmlContent.Append("<i>" + ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString() + " - " + ds.Tables[0].Rows[0]["FLDACCOUNTDESCRIPTION"].ToString() + "</i>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");

        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td>");
        sbHtmlContent.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
        sbHtmlContent.Append("Ship IT:");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td>");
        sbHtmlContent.Append("<i>" + ds.Tables[0].Rows[0]["FLDEMPLOYEENAME"].ToString() + "</i>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");

        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td>");
        sbHtmlContent.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
        sbHtmlContent.Append("Date & Time of Boarding:");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td>");
        sbHtmlContent.Append("<i>" + ds.Tables[0].Rows[0]["FLDFROMDATE"].ToString() + "</i>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");

        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td>");
        sbHtmlContent.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
        sbHtmlContent.Append("Date & Time of Sign Off:");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td>");
        sbHtmlContent.Append("<i>" + ds.Tables[0].Rows[0]["FLDTODATE"].ToString() + "</i>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");

        sbHtmlContent.Append("</table>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");

        sbHtmlContent.Append("<table ID='tbl2' border='1'>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td align='center' >");
        sbHtmlContent.Append("<b>S.No.</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td align='center' colspan='2'>");
        sbHtmlContent.Append("<b>Description</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td align='center'>");
        sbHtmlContent.Append("<b>Completed Status</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td align='center' >");
        sbHtmlContent.Append("<b>Grading</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");

        foreach (DataRow dr in dt.Rows)
        {
            sbHtmlContent.Append("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                if (alColumns[i] == "FLDSNO")
                {
                    sbHtmlContent.Append("<td style=\"font-size:9px;\" >");
                    sbHtmlContent.Append(dr[alColumns[i]].ToString());
                    sbHtmlContent.Append("</td>");
                }

                else if (alColumns[i] == "FLDDESCRIPTION")
                {
                    sbHtmlContent.Append("<td  colspan='2' style=\"font-size:9px;\" >");
                    sbHtmlContent.Append(dr[alColumns[i]].ToString());
                    sbHtmlContent.Append("</td>");
                }
                else if (alColumns[i] == "FLDCOMPLETED")
                {
                    sbHtmlContent.Append("<td style=\"font-size:9px;\" >");
                    sbHtmlContent.Append(dr[alColumns[i]].ToString());
                    sbHtmlContent.Append("</td>");
                }
                else if (alColumns[i] == "FLDHARDNAME")
                {
                    sbHtmlContent.Append("<td style=\"font-size:9px;\" >");
                    sbHtmlContent.Append(dr[alColumns[i]].ToString());
                    sbHtmlContent.Append("</td>");
                }
            }
            sbHtmlContent.Append("</tr>");
        }

        sbHtmlContent.Append("</table>");
        sbHtmlContent.Append("<br/>");

        sbHtmlContent.Append("<table ID='tbl3'>");

        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td >");
        sbHtmlContent.Append("<b>Acknowledged By, </b> ");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<b>Master</b> ");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<b>" + ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString() + "</b> ");
        sbHtmlContent.Append("</td >");
        sbHtmlContent.Append("<td colspan = '2'>");
        sbHtmlContent.Append("</td >");
        sbHtmlContent.Append("<td >");
        sbHtmlContent.Append("<b>Task Done By</b> ");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<b>" + ds.Tables[0].Rows[0]["FLDEMPLOYEEID"].ToString() + "</b> ");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<b>Ship IT Support‎</b> ");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<b>ESM Singapore</b> ");

        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("</table>");

        sbHtmlContent.Append("</div>");
        sbHtmlContent.Append("</div>");

        return sbHtmlContent.ToString();
    }

    private string HtmlTableDataContentSup(DataSet ds)
    {
        StringBuilder sbHtmlContent = new StringBuilder();

        sbHtmlContent.Append("<div style='font-size: 10px'>");
        sbHtmlContent.Append("<div align='left'>");

        sbHtmlContent.Append("</div>");
        sbHtmlContent.Append("<br/>");

        sbHtmlContent.Append("<div align='right'>");
        sbHtmlContent.Append("<b>Date:</b>");
        sbHtmlContent.Append(General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDVOUCHERDATE"].ToString()));
        sbHtmlContent.Append("</div>");

        sbHtmlContent.Append("<div align='left'>");
        sbHtmlContent.Append("<b>TO: CAPTAIN AND OWNERS OF </b>");
        sbHtmlContent.Append("<i>" + ds.Tables[1].Rows[0]["FLDVESSELNAME"].ToString() + "</i>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<b>WE HAVE DEBITED YOUR ACCOUNT WITH US UNDER THE HEAD</b>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<b>OWN SUPT.TRAVELLING FOR THE FOLLOWING: </b>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("</div>");

        sbHtmlContent.Append("<table ID='tbl11' border='1'>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td align='center' colspan='4' style=\"width:6000px\">");
        sbHtmlContent.Append("<b>PARTICULARS</b>");
        sbHtmlContent.Append("</td>");

        sbHtmlContent.Append("<td align='center' style=\"width:1000px\">");
        sbHtmlContent.Append("<b>No of Days</b>");
        sbHtmlContent.Append("</td>");

        sbHtmlContent.Append("<td align='center' style=\"width:1000px\">");
        sbHtmlContent.Append("<b>Rate / Hours</b>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<i>(" + ds.Tables[1].Rows[0]["FLDCURRENCYNAME"].ToString() + ")</i>");
        sbHtmlContent.Append("</td>");

        sbHtmlContent.Append("<td align='center' style=\"width:2000px\">");
        sbHtmlContent.Append("<b>AMOUNT</b>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<i>(" + ds.Tables[1].Rows[0]["FLDCURRENCYNAME"].ToString() + ")</i>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");

        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td colspan='4' style=\"font-size:9px;\" >");
        sbHtmlContent.Append("BEING SUPT. ATTENDANCE FEE INCURRED FOR THE VESSEL DURING THESE PERIODS.");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");

        sbHtmlContent.Append("<table ID='tbl2' width='100%' border='0'");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td colspan='20' style=\"width:9000px;font-size:9px;\" >");
        sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDEMPLOYEENAME"].ToString());
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDFROMDATE"].ToString() + " - " + ds.Tables[0].Rows[0]["FLDTODATE"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td style=\"width:1000px;font-size:9px;\">");
        sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDTOTALDAYS"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");

        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td colspan='20' style=\"width:9000px;font-size:9px;\">");
        sbHtmlContent.Append("Less: Yearly Attendance as per agreement");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td style=\"width:1000px;font-size:9px;\">");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDBUDGETEDDAYS"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");

        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td colspan='20' style=\"width:9000px;font-size:9px;\">");
        sbHtmlContent.Append("Less:  Charged Voucher Register – Voucher Date");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td style=\"width:1000px;font-size:9px;\">");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDCHARGEDDAYS"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");

        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td colspan='20' style=\"width:9000px;font-size:9px;\">");
        sbHtmlContent.Append("Total Number of days Billed");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td style='border-top' style=\"width:1000px;font-size:9px;\">");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDCHARGINGDAYS"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("</table>");

        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td style=\"font-size:9px;\" >");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDCHARGINGDAYS"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td style=\"font-size:9px;\" >");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDBILLINGITEMRATE"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td style=\"font-size:9px;\" >");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDTOTALAMOUNT"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");

        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td colspan='4' >");
        sbHtmlContent.Append("");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td colspan='2' style=\"font-size:9px;\" >");
        sbHtmlContent.Append("<b>");
        sbHtmlContent.Append("TOTAL AMOUNT");
        sbHtmlContent.Append("</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td  style=\"font-size:9px;\" >");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDTOTALAMOUNT"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");

        sbHtmlContent.Append("</table>");
        sbHtmlContent.Append("<div align='left'>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("FINANCE DIRECTOR ");
        sbHtmlContent.Append("</div>");
        sbHtmlContent.Append("</div>");

        return sbHtmlContent.ToString();
    }

    public void ConvertToPdf(string HTMLString, string fileLocation)
    {
        //Document document = new Document(PageSize.A4, 50, 50, 25, 25);
        Document document = new Document(new Rectangle(595f, 842f));
        document.SetMargins(36f, 36f, 36f, 0f);

        PdfWriter.GetInstance(document, new FileStream(fileLocation, FileMode.Create));
        document.Open();

        StyleSheet styles = new StyleSheet();

        ArrayList htmlarraylist = (ArrayList)iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(HTMLString), styles);
        //List<IElement> htmlarraylist = HTMLWorker.ParseToList(new StringReader(HTMLString), null);

        for (int k = 0; k < htmlarraylist.Count; k++)
        {
            document.Add((IElement)htmlarraylist[k]);
        }

        document.Close();
    }
    protected void gvMonthlyBillingItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMonthlyBillingItem.CurrentPageIndex + 1;
            BindBillingItemData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMonthlyBillingCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMonthlyBillingCrew.CurrentPageIndex + 1;
            BindCrewData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvMonthlyBillingItem.SelectedIndexes.Clear();
        gvMonthlyBillingItem.EditIndexes.Clear();
        gvMonthlyBillingItem.DataSource = null;
        gvMonthlyBillingItem.Rebind();
    }
    protected void RebindCrew()
    {
        gvMonthlyBillingCrew.SelectedIndexes.Clear();
        gvMonthlyBillingCrew.EditIndexes.Clear();
        gvMonthlyBillingCrew.DataSource = null;
        gvMonthlyBillingCrew.Rebind();
    }
}
