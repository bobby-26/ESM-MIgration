using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class CommonBudgetBillingByVessel : PhoenixBasePage
{


    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        PhoenixToolbar Subtoolbar = new PhoenixToolbar();

        toolbar.AddFontAwesomeButton("../Common/CommonBudgetBillingByVessel.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvVesselAllocation')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        MenuCommonBudgetGroupAllocation.AccessRights = this.ViewState;
        MenuCommonBudgetGroupAllocation.MenuList = toolbar.Show();
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
        toolbar = new PhoenixToolbar();
        Subtoolbar = new PhoenixToolbar();
        toolbar.AddButton("Budget Allocation", "BUDGETALLOCATION", ToolBarDirection.Right);
        toolbar.AddButton("Budget Breakdown", "BUDGETBREAKDOWN", ToolBarDirection.Right);
        Subtoolbar.AddButton("Budget Billing Items ", "BUDGETBILLINGITEM", ToolBarDirection.Right);
        Subtoolbar.AddButton("Owner Budget Allocation ", "OWNERBILLINGALLOCATION", ToolBarDirection.Right);
        MenuBudgetTab.AccessRights = this.ViewState;
        MenuBudgetTab.MenuList = toolbar.Show();
        subMenuBudgetTab.AccessRights = this.ViewState;
        subMenuBudgetTab.MenuList = Subtoolbar.Show();
        MenuBudgetTab.SelectedMenuIndex = 1;
        subMenuBudgetTab.SelectedMenuIndex = 0;

        if (!IsPostBack)
        {




            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["VESSELBUDGETID"] = null;
            ViewState["VESSELBUDGETID"] = "";
            ViewState["ACCOUNTID"] = "";
            ViewState["OWNERID"] = "";

            if (Request.QueryString["VESSELBUDGETALLOCATIONID"] != null && Request.QueryString["VESSELBUDGETALLOCATIONID"].ToString() != "")
                ViewState["VESSELBUDGETALLOCATIONID"] = Request.QueryString["VESSELBUDGETALLOCATIONID"].ToString();
            else
                ViewState["VESSELBUDGETALLOCATIONID"] = "";
            if (Request.QueryString["VESSELID"] != null && Request.QueryString["VESSELID"].ToString() != "")
                ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
            if (Request.QueryString["vesselaccountid"] != null && Request.QueryString["vesselaccountid"].ToString() != "")
                ViewState["VESSELACCOUNTID"] = Request.QueryString["VESSELACCOUNTID"].ToString();
            ViewState["VESSELBUDGETID"] = "";
            ViewState["OWNER"] = "";
            if (Request.QueryString["accountid"] != null)
                ViewState["ACCOUNTID"] = Request.QueryString["accountid"].ToString();
            BindFinancialYearDetails();
            Bindaccount();
        }
    }

    protected void BudgetTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("BUDGETWORKINGS"))
        {
            //Response.Redirect("../Common/CommonBudgetVesselList.aspx", false);
        }
        if (CommandName.ToUpper().Equals("OWNERBILLINGALLOCATION"))
        {
            Response.Redirect("../Common/CommonBudgetOwnerGroupAllocation.aspx?VESSELBUDGETALLOCATIONID=" + ViewState["VESSELBUDGETALLOCATIONID"] + "&VESSELID=" + ViewState["VESSELID"] + "&vesselaccountid=" + ViewState["VESSELACCOUNTID"], false);
        }
        if (CommandName.ToUpper().Equals("BUDGETALLOCATION"))
        {
            Response.Redirect("../Common/CommonBudgetGroupAllocation.aspx?vesselid=" + ViewState["VESSELID"] + "&vesselaccountid=" + ViewState["VESSELACCOUNTID"], false);
        }
    }

    private void BindFinancialYearDetails()
    {
        if (ViewState["VESSELID"] != null && ViewState["VESSELID"].ToString() != "")
        {
            DataSet dsVessel = PhoenixCommonBudgetGroupAllocation.EditVesselBudgetAllocation(General.GetNullableGuid(ViewState["VESSELBUDGETALLOCATIONID"].ToString()));
            if (dsVessel.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsVessel.Tables[0].Rows[0];
                ucFinancialYear.SelectedQuick = dr["FLDFINANCIALYEAR"].ToString();
                txtEffectiveDate.Text = dr["FLDEFFECTIVEDATE"].ToString();
                txtVesselCode.Text = dr["FLDVESSELNAME"].ToString();
                ViewState["OWNER"] = dr["FLDOWNERID"].ToString();
            }

        }
    }
    private void Bindaccount()
    {
        if (ViewState["ACCOUNTID"].ToString() != "")
        {
            DataSet ds = PhoenixRegistersAccount.EditAccount(int.Parse(ViewState["ACCOUNTID"].ToString()));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["OWNERID"] = ds.Tables[0].Rows[0]["FLDPRINCIPALID"].ToString();
                }
            }
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDBILLINGITEMDESCRIPTION", "FLDFREQUENCYNAME", "FLDBILLINGBASISNAME", "FLDBILLINGUNITNAME", "FLDVESSELBUDGETCODE", "FLDAMOUNT", "FLDOWNERBUDGETCODE", "FLDCOMPANYSHORTCODE", "FLDBUDGETEDQUANTITY", "FLDCURRENCY" };
        string[] alCaptions = { "Billing Item", "Frequency", "Billing Basis", "Billing Unit", "Vessel Budget Code", "Budget Amount", "Owner Budget Code", "Company", "Budgeted Quantity", "Currency" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixCommonBudgetBillingByVessel.BudgetBillingByVesselSearch(
                                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , null
                                                                            , null
                                                                            , sortexpression
                                                                            , sortdirection
                                                                            , (int)ViewState["PAGENUMBER"]
                                                                            , General.ShowRecords(null)
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount
                                                                            , General.GetNullableGuid(ViewState["VESSELBUDGETALLOCATIONID"].ToString()));

        General.SetPrintOptions("gvVesselAllocation", "Budget Billing Items by Vessel", alCaptions, alColumns, ds);


        gvVesselAllocation.DataSource = ds;


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

        string[] alColumns = { "FLDBILLINGITEMDESCRIPTION", "FLDFREQUENCYNAME", "FLDBILLINGBASISNAME", "FLDBILLINGUNITNAME", "FLDVESSELBUDGETCODE", "FLDAMOUNT", "FLDOWNERBUDGETCODE", "FLDCOMPANYSHORTCODE", "FLDBUDGETEDQUANTITY", "FLDCURRENCY" };
        string[] alCaptions = { "Billing Item", "Frequency", "Billing Basis", "Billing Unit", "Vessel Budget Code", "Budget Amount", "Owner Budget Code", "Company", "Budgeted Quantity", "Currency" };

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCommonBudgetBillingByVessel.BudgetBillingByVesselSearch(
                                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , null
                                                                            , null
                                                                            , sortexpression
                                                                            , sortdirection
                                                                            , (int)ViewState["PAGENUMBER"]
                                                                            , iRowCount
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount
                                                                            , General.GetNullableGuid(ViewState["VESSELBUDGETALLOCATIONID"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=BudgetBillingItemsByVessel.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Budget Billing Items by Vessel</h3></td>");
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

    protected void gvVesselAllocation_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Item.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);

            RadTextBox tb1 = (RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtOwnerBudgetgroupIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtOwnerBudgetNameEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");

            LinkButton ibtnShowOwnerBudgetEdit = (LinkButton)e.Item.FindControl("ibtnShowOwnerBudgetEdit");

            UserControlBudgetBillingItem ucBudgetBillingItemEdit = (UserControlBudgetBillingItem)e.Item.FindControl("ucBudgetBillingItemEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucBudgetBillingItemEdit != null && drv["FLDBUDGETBILLINGID"] != null)
            {
                ucBudgetBillingItemEdit.BillingItemsList = PhoenixRegistersBudgetBilling.BudgetBillingItemList();
                ucBudgetBillingItemEdit.SelectedBillingItem = drv["FLDBUDGETBILLINGID"].ToString();
            }

            if (ibtnShowOwnerBudgetEdit != null)
            {
                ibtnShowOwnerBudgetEdit.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=true&ownerid=" + ViewState["OWNERID"].ToString() + "&budgetid=" + drv["FLDVESSELBUDGETID"].ToString() + "', true); ");          //+ "&budgetid=" + lblbudgetid.Text       
                if (!SessionUtil.CanAccess(this.ViewState, ibtnShowOwnerBudgetEdit.CommandName)) ibtnShowOwnerBudgetEdit.Visible = false;
            }

            UserControlCompany ucCompanyEdit = (UserControlCompany)e.Item.FindControl("ucCompanyEdit");
            DataRowView drvcmp = (DataRowView)e.Item.DataItem;
            if (ucCompanyEdit != null)
            {
                ucCompanyEdit.CompanyList = PhoenixRegistersCompany.ListCompany();
                ucCompanyEdit.SelectedCompany = drvcmp["FLDCOMPANYID"].ToString();
            }

            UserControlCurrency ddlCurrencyCodeEdit = (UserControlCurrency)e.Item.FindControl("ddlCurrencyCodeEdit");
            DataRowView drvCur = (DataRowView)e.Item.DataItem;
            if (ddlCurrencyCodeEdit != null && drv["FLDCURRENCYID"] != null)
            {
                ddlCurrencyCodeEdit.SelectedCurrency = drv["FLDCURRENCYID"].ToString();
            }

            RadComboBox ddlVisitTypeEdit = (RadComboBox)e.Item.FindControl("ddlVisitTypeEdit");

            if (ddlVisitTypeEdit != null && drv["FLDVISITTYPE"] != null && drv["FLDVISITTYPE"].ToString() != string.Empty)
            {
                ddlVisitTypeEdit.SelectedValue = drv["FLDVISITTYPE"].ToString();
            }


        }
        if (e.Item is GridFooterItem)
        {
            RadTextBox tb1 = (RadTextBox)e.Item.FindControl("txtOwnerBudgetIdAdd");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtOwnerBudgetgroupIdAdd");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtOwnerBudgetNameAdd");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");

            UserControlBudgetBillingItem ucBudgetBillingItemAdd = (UserControlBudgetBillingItem)e.Item.FindControl("ucBudgetBillingItemAdd");
            UserControlCompany ucCompanyAdd = (UserControlCompany)e.Item.FindControl("ucCompanyAdd");
            LinkButton ibtnShowOwnerBudgetAdd = (LinkButton)e.Item.FindControl("ibtnShowOwnerBudgetAdd");
            UserControlCurrency ddlCurrencyCodeAdd = (UserControlCurrency)e.Item.FindControl("ddlCurrencyCodeAdd");
            DataRowView drvCur = (DataRowView)e.Item.DataItem;

            if (ucBudgetBillingItemAdd != null)
                ucBudgetBillingItemAdd.BillingItemsList = PhoenixRegistersBudgetBilling.BudgetBillingItemList();

            if (ibtnShowOwnerBudgetAdd != null)
            {
                ibtnShowOwnerBudgetAdd.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetAdd', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=true&ownerid=" + ViewState["OWNERID"].ToString() + "&budgetid=" + ucBudgetBillingItemAdd.SelectedBillingItem + "', true); ");          //+ "&budgetid=" + lblbudgetid
                if (!SessionUtil.CanAccess(this.ViewState, ibtnShowOwnerBudgetAdd.CommandName)) ibtnShowOwnerBudgetAdd.Visible = false;
            }

            if (ucCompanyAdd != null)
            {
                ucCompanyAdd.CompanyList = PhoenixRegistersCompany.ListCompany();
                ucCompanyAdd.SelectedCompany = "12";
            }
        }
    }

    protected void gvVesselAllocation_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            RadLabel lblBudgetBillingByVesselId = (RadLabel)e.Item.FindControl("lblBudgetBillingByVesselId");
            if (lblBudgetBillingByVesselId != null)
                ViewState["BUDGETBILLINGBYVESSELID"] = lblBudgetBillingByVesselId.Text;
        }
        else if (e.CommandName.ToUpper().Equals("ADD"))
        {
            try
            {
                UserControlBudgetBillingItem ucBudgetBillingItemAdd = (UserControlBudgetBillingItem)e.Item.FindControl("ucBudgetBillingItemAdd");
                UserControlDecimal txtAmountAdd = (UserControlDecimal)e.Item.FindControl("txtAmountAdd");
                RadTextBox txtOwnerBudgetIdAdd = (RadTextBox)e.Item.FindControl("txtOwnerBudgetIdAdd");
                UserControlCompany ucCompanyAdd = (UserControlCompany)e.Item.FindControl("ucCompanyAdd");
                UserControlDecimal txtBudgetedQuantityAdd = (UserControlDecimal)e.Item.FindControl("txtBudgetedQuantityAdd");
                UserControlCurrency ddlCurrencyCodeAdd = (UserControlCurrency)e.Item.FindControl("ddlCurrencyCodeAdd");
                RadComboBox ddlVisitTypeAdd = (RadComboBox)e.Item.FindControl("ddlVisitTypeAdd");

                if (!IsValidBudgetBillingItem(ucBudgetBillingItemAdd.SelectedBillingItem
                                                , txtAmountAdd.Text
                                                , txtOwnerBudgetIdAdd.Text
                                                , ucCompanyAdd.SelectedCompany
                                                , ddlCurrencyCodeAdd.SelectedCurrency))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCommonBudgetBillingByVessel.InsertBudgetBillingByVessel(
                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                        , int.Parse(ViewState["VESSELID"].ToString())
                                                        , new Guid(ucBudgetBillingItemAdd.SelectedBillingItem)
                                                        , new Guid(ViewState["VESSELBUDGETALLOCATIONID"].ToString())
                                                        , General.GetNullableGuid(txtOwnerBudgetIdAdd.Text)
                                                        , General.GetNullableDecimal(txtAmountAdd.Text)
                                                        , General.GetNullableInteger(ucCompanyAdd.SelectedCompany)
                                                        , Convert.ToDecimal(txtBudgetedQuantityAdd.Text)
                                                        , Int32.Parse(ddlCurrencyCodeAdd.SelectedCurrency)
                                                        , General.GetNullableInteger(ddlVisitTypeAdd.SelectedValue));
                ViewState["BUDGETBILLINGBYVESSELID"] = null;
                BindData();
                gvVesselAllocation.Rebind();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
        }
        else if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            RadLabel lblBudgetBillingByVesselId = (RadLabel)e.Item.FindControl("lblBudgetBillingByVesselId");
            if (lblBudgetBillingByVesselId != null)
            {
                PhoenixCommonBudgetBillingByVessel.DeleteBudgetBillingByVessel(
                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                        , new Guid(lblBudgetBillingByVesselId.Text)
                                                        );
                ViewState["BUDGETBILLINGBYVESSELID"] = null;
                BindData();
            }
        }
    }



    protected void gvVesselAllocation_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            UserControlBudgetBillingItem ucBudgetBillingItemEdit = (UserControlBudgetBillingItem)e.Item.FindControl("ucBudgetBillingItemEdit");
            UserControlDecimal txtAmountEdit = (UserControlDecimal)e.Item.FindControl("txtAmountEdit");
            RadTextBox txtOwnerBudgetIdEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit");
            UserControlCompany ucCompanyEdit = (UserControlCompany)e.Item.FindControl("ucCompanyEdit");
            UserControlDecimal txtBudgetedQuantityEdit = (UserControlDecimal)e.Item.FindControl("txtBudgetedQuantityEdit");
            UserControlCurrency ddlCurrencyCodeEdit = (UserControlCurrency)e.Item.FindControl("ddlCurrencyCodeEdit");
            RadComboBox ddlVisitTypeEdit = (RadComboBox)e.Item.FindControl("ddlVisitTypeEdit");

            if (!IsValidBudgetBillingItem(ucBudgetBillingItemEdit.SelectedBillingItem
                                            , txtAmountEdit.Text
                                            , txtOwnerBudgetIdEdit.Text
                                            , ucCompanyEdit.SelectedCompany
                                            , ddlCurrencyCodeEdit.SelectedCurrency))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCommonBudgetBillingByVessel.UpdateBudgetBillingByVessel(
                                                       PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                       , int.Parse(ViewState["VESSELID"].ToString())
                                                       , new Guid(ViewState["BUDGETBILLINGBYVESSELID"].ToString())
                                                       , new Guid(ucBudgetBillingItemEdit.SelectedBillingItem)
                                                       , General.GetNullableGuid(txtOwnerBudgetIdEdit.Text)
                                                       , General.GetNullableDecimal(txtAmountEdit.Text)
                                                       , General.GetNullableInteger(ucCompanyEdit.SelectedCompany)
                                                       , Convert.ToDecimal(txtBudgetedQuantityEdit.Text)
                                                       , Int32.Parse(ddlCurrencyCodeEdit.SelectedCurrency)
                                                       , General.GetNullableInteger(ddlVisitTypeEdit.SelectedValue));
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvVesselAllocation_DeleteCommand(object sender, GridCommandEventArgs de)
    {
        BindData();
    }

    protected void gvVesselAllocation_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {


    }

    protected void gvVesselAllocation_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }



    protected void AddBillingItem_Changed(object sender, EventArgs e)
    {
        UserControlBudgetBillingItem ucBudgetBillingItem = (UserControlBudgetBillingItem)sender;
        GridFooterItem row = (GridFooterItem)ucBudgetBillingItem.Parent.Parent;
        LinkButton ibtnShowOwnerBudgetAdd = (LinkButton)row.Cells[7].FindControl("ibtnShowOwnerBudgetAdd");

        if (ucBudgetBillingItem.SelectedBillingItem.ToUpper() != "DUMMY")
        {
            DataSet ds = PhoenixRegistersBudgetBilling.EditBudgetBilling(new Guid(ucBudgetBillingItem.SelectedBillingItem));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ViewState["VESSELBUDGETID"] = dr["FLDVESSELBUDGETID"].ToString();
                if (ibtnShowOwnerBudgetAdd != null)
                    ibtnShowOwnerBudgetAdd.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetAdd', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=true&ownerid=" + ViewState["OWNERID"].ToString() + "&budgetid=" + ViewState["VESSELBUDGETID"] + "', true); ");
            }
        }
    }

    protected void EditBillingItem_Changed(object sender, EventArgs e)
    {
        UserControlBudgetBillingItem ucBudgetBillingItem = (UserControlBudgetBillingItem)sender;
        GridViewRow row = (GridViewRow)ucBudgetBillingItem.Parent.Parent;
        ImageButton ibtnShowOwnerBudgetEdit = (ImageButton)row.Cells[7].FindControl("ibtnShowOwnerBudgetEdit");

        if (ucBudgetBillingItem.SelectedBillingItem.ToUpper() != "DUMMY")
        {
            DataSet ds = PhoenixRegistersBudgetBilling.EditBudgetBilling(new Guid(ucBudgetBillingItem.SelectedBillingItem));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ViewState["VESSELBUDGETID"] = dr["FLDVESSELBUDGETID"].ToString();
                if (ibtnShowOwnerBudgetEdit != null)
                    ibtnShowOwnerBudgetEdit.Attributes.Add("onclick", "return showPickList('ibtnShowOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=true&ownerid=" + ViewState["OWNERID"].ToString() + "&budgetid=" + ViewState["VESSELBUDGETID"] + "', true); ");
            }
        }
    }

    private bool IsValidBudgetBillingItem(string billingitemid, string amount, string ownerbudgetid, string companyid, string currency)
    {
        ucError.HeaderMessage = "Please provide the following required information.";
        int mappedyn = 0;

        if (General.GetNullableGuid(billingitemid) == null)
            ucError.ErrorMessage = "Billing Item is required.";
        if (General.GetNullableDecimal(amount) == null)
            ucError.ErrorMessage = "Amount is required.";
        if (General.GetNullableInteger(companyid) == null)
            ucError.ErrorMessage = "Company is required.";
        if (currency.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Currency is required.";
        if (General.GetNullableInteger(ViewState["OWNER"].ToString()) != null && General.GetNullableInteger(ViewState["VESSELBUDGETID"].ToString()) != null)
        {
            DataSet dsmap = PhoenixCommonBudgetBillingByVessel.OwnerBudgetMapCheck(int.Parse(ViewState["OWNER"].ToString()), int.Parse(ViewState["VESSELBUDGETID"].ToString()), ref mappedyn);

            if (mappedyn == 1)
            {
                if (General.GetNullableGuid(ownerbudgetid) == null)
                    ucError.ErrorMessage = "Owner Budget Code is required.";
            }
        }

        return (!ucError.IsError);
    }



    protected void gvVesselAllocation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVesselAllocation.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
