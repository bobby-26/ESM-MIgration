using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsInvoiceDirectPurchaseOrderLineItem : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsInvoiceDirectPurchaseOrderLineItem.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvOrderLine')", "Print Grid", "icon_print.png", "PRINT");
            MenuRegistersStockItem.AccessRights = this.ViewState;
            MenuRegistersStockItem.MenuList = toolbargrid.Show();

            PhoenixToolbar toolgrid = new PhoenixToolbar();
            toolgrid.AddImageButton("../Accounts/AccountsInvoiceDirectPurchaseOrderLineItem.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolgrid.AddImageLink("javascript:CallPrint('gvTax')", "Print Grid", "icon_print.png", "PRINT");
            AdditionalChargeItem.AccessRights = this.ViewState;
            AdditionalChargeItem.MenuList = toolgrid.Show();

            if (!IsPostBack)
            {
                ViewState["VESSELID"] = Request.QueryString["vslid"];
                ViewState["ORDERID"] = Request.QueryString["ORDERID"].ToString();
                ViewState["INVOICECODE"] = Request.QueryString["qinvoicecode"];

                EditOrder();

                ViewState["PAGENUMBER"] = 1;
                ViewState["PAGENUMBERTax"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                DataTable dt = new DataTable();
                dt = PhoenixAccountsInvoice.orderformdirectpolist(General.GetNullableGuid(ViewState["ORDERID"].ToString()));
                //if (dt.Rows.Count > 0)
                //    ViewState["ISPNI"] = dt.Rows[0]["FLDISPNI"].ToString();
                //else
                ViewState["ISPNI"] = null;
                gvOrderLine.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvTax.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
            //BindData();
            //BindDataTax();
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

        string[] alColumns = { "FLDPARTNAME", "FLDSUBACCOUNT", "FLDAMOUNT", "FLDPODISCOUNT", "FLDTOTALAMOUNT", "FLDVESSELDISCOUNT", "FLDTOTALVESSELAMOUNT" };
        string[] alCaptions = { "Item Name", "Budget Code", "Amount", "Discount", "Total Amount", "Vessel Discount", "Total Vessel Amount" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsPOStaging.OrderLineStagingSearchDirectPO(new Guid(ViewState["ORDERID"].ToString()), sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
           iRowCount,
           ref iRowCount,
           ref iTotalPageCount);
        General.ShowExcel("Line Items", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }
    protected void ShowExcelAdditionalChargeItem()
    {
        int iTaxRowCount = 0;
        int iTaxTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = {"FLDDISCRIPTION", "FLDTAXTYPENAME", "FLDSUBACCOUNT", "FLDVALUE", "FLDPODISCOUNT",
                                 "FLDAMOUNT","FLDTOTALAMOUNT", "FLDVESSELDISCOUNT", "FLDTOTALVESSELAMOUNT" };

        string[] alCaptions = {"Charge Description", "Type ","Budget Code", "Value", "Discount", "Amount",
                                 "Total Amount", "Vessel Discount", "Total Vessel Amount" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["TAXROWCOUNT"] == null || Int32.Parse(ViewState["TAXROWCOUNT"].ToString()) == 0)
            iTaxRowCount = 10;
        else
            iTaxRowCount = Int32.Parse(ViewState["TAXROWCOUNT"].ToString());

        ds = PhoenixAccountsPOStaging.OrderTaxStagingSearchDirectPO(new Guid(ViewState["ORDERID"].ToString()), sortexpression, sortdirection, 1, 10
            , ref iTaxRowCount, ref iTaxTotalPageCount);
        General.ShowExcel("Additional Charge Items", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }
    private void EditOrder()
    {
        if (ViewState["ORDERID"] != null && ViewState["ORDERID"].ToString() != string.Empty)
        {
            DataTable dt = PhoenixAccountsInvoice.InvoiceDirectPOEdit(new Guid(ViewState["ORDERID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                ViewState["APPROVALTYPE"] = dt.Rows[0]["FLDAPPROVALTYPE"].ToString();
                ViewState["TECHDIRECTOR"] = dt.Rows[0]["FLDTECHDIRECTOR"].ToString();
                ViewState["FLEETMANAGER"] = dt.Rows[0]["FLDFLEETMANAGER"].ToString();
                ViewState["SUPT"] = dt.Rows[0]["FLDSUPT"].ToString();

                DataSet ds = PhoenixRegistersAccount.EditAccount(int.Parse(dt.Rows[0]["FLDACCOUNTID"].ToString()));
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ViewState["Ownerid"] = ds.Tables[0].Rows[0]["FLDPRINCIPALID"].ToString();
                    }
                }
                PhoenixToolbar toolbar = new PhoenixToolbar();
                if (dt.Rows[0]["FLDAPPROVALSTATUS"].ToString() == "0")
                {
                    toolbar.AddButton("Approve", "APPROVE", ToolBarDirection.Right);
                    MenuDirectPO.AccessRights = this.ViewState;
                    MenuDirectPO.Visible = true;
                    MenuDirectPO.Title = "Line Item";
                    MenuDirectPO.MenuList = toolbar.Show();
                }
                else
                {
                    MenuDirectPO.Visible = false;
                }
            }
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        try
        {
            string[] alColumns = { "FLDPARTNAME", "FLDSUBACCOUNT", "FLDAMOUNT", "FLDPODISCOUNT", "FLDTOTALAMOUNT", "FLDVESSELDISCOUNT", "FLDTOTALVESSELAMOUNT" };
            string[] alCaptions = { "Item Name", "Budget Code", "Amount", "Discount", "Total Amount", "Vessel Discount", "Total Vessel Amount" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            string ispni;
            DataTable dt1 = PhoenixAccountsInvoice.InvoiceDirectPOEdit(new Guid(ViewState["ORDERID"].ToString()));
            if (dt1.Rows.Count > 0)
                ispni = dt1.Rows[0]["FLDPNIYN"].ToString();
            else
                ispni = "";

            DataSet ds = PhoenixAccountsPOStaging.OrderLineStagingSearchDirectPO(new Guid(ViewState["ORDERID"].ToString()), sortexpression, sortdirection, int.Parse(ViewState["PAGENUMBER"].ToString()),
                gvOrderLine.PageSize,
                ref iRowCount,
                ref iTotalPageCount);


            gvOrderLine.DataSource = ds;
            gvOrderLine.VirtualItemCount = iRowCount;


            if (ispni == "1")
            {
                gvOrderLine.Columns[3].Visible = true;
                gvOrderLine.Columns[4].Visible = true;
            }
            else
            {
                gvOrderLine.Columns[3].Visible = false;
                gvOrderLine.Columns[4].Visible = false;
            }
            short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
            gvOrderLine.Columns[5].Visible = (showcreditnotedisc == 1) ? true : false;
            gvOrderLine.Columns[6].Visible = (showcreditnotedisc == 1) ? true : false;
            gvOrderLine.Columns[7].Visible = (showcreditnotedisc == 1) ? true : false;
            gvOrderLine.Columns[8].Visible = (showcreditnotedisc == 1) ? true : false;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            General.SetPrintOptions("gvOrderLine", "LINE ITEMS", alCaptions, alColumns, ds);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void RegistersStockItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("APPROVE"))
            {
                int budgetedYN = 0;
                String scriptpopup = "";
                DataTable dt = PhoenixAccountsPOStaging.DirectPOAccountDeatils(new Guid(ViewState["ORDERID"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    budgetedYN = int.Parse(dt.Rows[0]["FLDISBUDGETED"].ToString());
                }

                if (budgetedYN == 1)
                {
                    scriptpopup = String.Format(
                           "javascript:parent.Openpopup('codehelp1', '', '../Accounts/AccountsDirectPORemainingBudget.aspx?orderID=" + ViewState["ORDERID"].ToString() + "');");
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
                else
                {
                    PhoenixAccountsPOStaging.DirectPOApprove(new Guid(ViewState["ORDERID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                    ucStatus.Text = "Purchase order is approved";
                    ucStatus.Visible = true;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void AdditionalChargeItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelAdditionalChargeItem();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindDataTax()
    {
        int iTaxRowCount = 0;
        int iTaxTotalPageCount = 0;

        string[] alColumns = {"FLDDISCRIPTION", "FLDTAXTYPENAME", "FLDSUBACCOUNT", "FLDVALUE", "FLDPODISCOUNT",
                                 "FLDAMOUNT","FLDTOTALAMOUNT", "FLDVESSELDISCOUNT", "FLDTOTALVESSELAMOUNT" };

        string[] alCaptions = {"Charge Description", "Type ","Budget Code", "Value", "Discount", "Amount",
                                 "Total Amount", "Vessel Discount", "Total Vessel Amount" };

        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsPOStaging.OrderTaxStagingSearchDirectPO(new Guid(ViewState["ORDERID"].ToString()), sortexpression, sortdirection, 1, 10
            , ref iTaxRowCount, ref iTaxTotalPageCount);

        gvTax.DataSource = ds;
        gvTax.VirtualItemCount = iTaxRowCount;

        short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
        //gvTax.Columns[4].Visible = (showcreditnotedisc == 1) ? true : false;
        gvTax.Columns[6].Visible = (showcreditnotedisc == 1) ? true : false;
        gvTax.Columns[7].Visible = (showcreditnotedisc == 1) ? true : false;
        gvTax.Columns[8].Visible = (showcreditnotedisc == 1) ? true : false;
        gvTax.Columns[9].Visible = (showcreditnotedisc == 1) ? true : false;
        gvTax.Columns[10].Visible = (showcreditnotedisc == 1) ? true : false;

        ViewState["TAXROWCOUNT"] = iTaxRowCount;
        ViewState["TAXTOTALPAGECOUNT"] = iTaxTotalPageCount;
        General.SetPrintOptions("gvTax", "ADDITIONAL CHARGE ITEM", alCaptions, alColumns, ds);
    }
    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {

    }
    protected void gvTax_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdEdit");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            ImageButton de = (ImageButton)e.Item.FindControl("cmdDelete");
            if (de != null)
            {
                de.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                de.Visible = SessionUtil.CanAccess(this.ViewState, de.CommandName);
                if (drv["FLDTAXTYPE"].ToString() == "2") de.Visible = false;
            }
            UserControlDecimal number = (UserControlDecimal)e.Item.FindControl("txtAmountEdit");
            //if (number != null && drv["FLDTAXTYPE"].ToString() == "1")
            //{
            //    number.CssClass = "readonlytextbox";
            //    number.ReadOnly = true;
            //}
            //else if (number != null)
            //{
            //    number.CssClass = "input_mandatory";
            //    number.ReadOnly = false;
            //}
            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowBudgetEdit");
            if (ib1 != null && ViewState["VESSELID"] != null)
            {

                ib1.Attributes.Add("onclick", "return showPickList('spnPickListTaxBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=102&hardtypecode=30&vesselid=" + ViewState["VESSELID"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
                ib1.Visible = SessionUtil.CanAccess(this.ViewState, ib1.CommandName);
                //if (drv["FLDTAXTYPE"].ToString() == "2")
                //{
                //    HtmlGenericControl spn = (HtmlGenericControl)e.Row.FindControl("spnPickListTaxBudgetEdit");
                //    spn.Visible = false;
                //}
            }

            RadTextBox tb = (RadTextBox)e.Item.FindControl("txtOwnerBudgetNameEdit");
            if (tb != null) tb.Attributes.Add("style", "visibility:hidden");
            tb = (RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit");
            if (tb != null) tb.Attributes.Add("style", "visibility:hidden");
            tb = (RadTextBox)e.Item.FindControl("txtOwnerBudgetgroupIdEdit");
            if (tb != null) tb.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtBudgetIdEdit = (RadTextBox)e.Item.FindControl("txtBudgetIdEdit");
            ImageButton imgShowOwnerBudgetEdit = (ImageButton)e.Item.FindControl("imgShowOwnerBudgetEdit");
            if (imgShowOwnerBudgetEdit != null && txtBudgetIdEdit != null)
            {
                imgShowOwnerBudgetEdit.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetCodeTaxEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=&Ownerid=" + ViewState["Ownerid"] + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetIdEdit.Text + "', true); ");          //+ "&budgetid=" + txtBudgetIdEdit.Text       
                imgShowOwnerBudgetEdit.Visible = SessionUtil.CanAccess(this.ViewState, imgShowOwnerBudgetEdit.CommandName);
            }
        }
        if (e.Item is GridFooterItem)
        {
            ImageButton ad = (ImageButton)e.Item.FindControl("cmdAdd");
            if (ad != null) ad.Visible = SessionUtil.CanAccess(this.ViewState, ad.CommandName);
            ImageButton ib = (ImageButton)e.Item.FindControl("btnShowBudgetAdd");
            if (ib != null)
            {
                ib.Attributes.Add("onclick", "return showPickList('spnPickListTaxBudgetAdd', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=102&hardtypecode=30&vesselid=" + ViewState["VESSELID"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
                ib.Visible = SessionUtil.CanAccess(this.ViewState, ib.CommandName);
            }

            RadTextBox tb = (RadTextBox)e.Item.FindControl("txtOwnerBudgetName");
            if (tb != null) tb.Attributes.Add("style", "visibility:hidden");
            tb = (RadTextBox)e.Item.FindControl("txtOwnerBudgetId");
            if (tb != null) tb.Attributes.Add("style", "visibility:hidden");
            tb = (RadTextBox)e.Item.FindControl("txtOwnerBudgetgroupId");
            if (tb != null) tb.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtBudgetIdAdd = (RadTextBox)e.Item.FindControl("txtBudgetIdAdd");
            ImageButton imgShowOwnerBudget = (ImageButton)e.Item.FindControl("imgShowOwnerBudget");
            if (imgShowOwnerBudget != null && txtBudgetIdAdd.Text != null)
            {
                imgShowOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetCodeTax', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=&Ownerid=" + ViewState["Ownerid"] + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetIdAdd.Text + "', true); ");          //+ "&budgetid=" + txtBudgetIdEdit.Text       
                imgShowOwnerBudget.Visible = SessionUtil.CanAccess(this.ViewState, imgShowOwnerBudget.CommandName);
            }
        }
    }
    protected void gvTax_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBERTax"] = null;
        }
        if (e.CommandName.ToUpper().Equals("ADD"))
        {
            try
            {
                string description = ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text;
                string budgetcode = ((RadTextBox)e.Item.FindControl("txtBudgetIdAdd")).Text;
                string amount = ((UserControlDecimal)e.Item.FindControl("txtValueAdd")).Text;
                string discount = ((UserControlDecimal)e.Item.FindControl("txtDiscountAdd")).Text;
                string taxtype = ((UserControlTaxType)e.Item.FindControl("ucTaxTypeAdd")).TaxType;
                string OwnerBudgetId = ((RadTextBox)e.Item.FindControl("txtOwnerBudgetId")).Text;

                if (!IsValidTaxAdditonal(description, budgetcode, amount))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsInvoice.InvoiceDirectPOTaxInsert(new Guid(ViewState["ORDERID"].ToString()), description, int.Parse(taxtype), decimal.Parse(amount), General.GetNullableInteger(budgetcode)
                    , General.GetNullableDecimal(discount), null, General.GetNullableGuid(OwnerBudgetId));
                EditOrder();
                //BindDataTax();
                gvTax.Rebind();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            try
            {
                Guid id = Guid.Parse(((RadLabel)e.Item.FindControl("lblordertaxcode")).Text);
                PhoenixAccountsInvoice.InvoiceDirectPOTaxDelete(id);
                EditOrder();
                //BindDataTax();
                gvTax.Rebind();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
        //RebindgvTax();
    }

    protected void gvTax_RowUpdating(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            Guid id = new Guid(((RadLabel)e.Item.FindControl("lblordertaxcode")).Text);
            string description = ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text;
            string budgetcode = ((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text;
            string value = ((UserControlDecimal)e.Item.FindControl("txtValueEdit")).Text;
            string discount = ((UserControlDecimal)e.Item.FindControl("txtDiscountEdit")).Text;
            string taxtype = ((UserControlTaxType)e.Item.FindControl("ucTaxTypeEdit")).TaxType;
            string isgst = ((RadLabel)e.Item.FindControl("lblIsGST")).Text;
            string OwnerBudgetId = ((RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit")).Text;

            if (!IsValidTaxAdditonal(description, (taxtype == "2" ? "1" : budgetcode), value))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixAccountsInvoice.InvoiceDirectPOTaxUpdate(id, description, int.Parse(taxtype), decimal.Parse(value), General.GetNullableInteger(budgetcode)
                , General.GetNullableDecimal(discount), (byte?)General.GetNullableInteger(isgst), General.GetNullableGuid(OwnerBudgetId));

            EditOrder();
            //BindDataTax();
            gvTax.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOrderLine_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        if (e.CommandName.ToUpper().Equals("ADD"))
        {
            try
            {
                string budgetcode = ((RadTextBox)e.Item.FindControl("txtBudgetIdAdd")).Text;
                string amount = ((UserControlDecimal)e.Item.FindControl("txtAmountAdd")).Text;
                string discount = ((UserControlDecimal)e.Item.FindControl("txtDiscountAdd")).Text;
                string partname = ((RadTextBox)e.Item.FindControl("txtPartNameAdd")).Text;
                string OwnerBudgetId = ((RadTextBox)e.Item.FindControl("txtOwnerBudgetId")).Text;

                string medicalcaseid = ((RadComboBox)e.Item.FindControl("ddlpnimedicalcaseadd")).SelectedValue;
                RadComboBox ddlpnitypeadd = (RadComboBox)e.Item.FindControl("ddlpnitypeadd");
                string pnitype = ddlpnitypeadd.Text;
                string pnitypevalid = ((RadComboBox)e.Item.FindControl("ddlpnitypeadd")).SelectedValue;
                string ispni = (((RadComboBox)e.Item.FindControl("ddlpnimedicalcaseadd")).SelectedIndex <= 1) ? "0" : "1";
                string signeryn = ((RadComboBox)e.Item.FindControl("ddlpnitypeadd")).SelectedValue;

                int medical = ((RadComboBox)e.Item.FindControl("ddlpnimedicalcaseadd")).SelectedIndex;

                if (signeryn == "")
                    signeryn = "onsigner";

                if (pnitype.ToUpper() == "--SELECT--")
                    pnitype = "";

                if (!IsValidLineItem(budgetcode, amount, medicalcaseid, pnitypevalid, medical))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsPOStaging.OrderLineStagingInsert(new Guid(ViewState["ORDERID"].ToString()), partname, int.Parse(budgetcode), decimal.Parse(amount), General.GetNullableDecimal(discount), General.GetNullableGuid(OwnerBudgetId), General.GetNullableInteger(ispni), General.GetNullableGuid(medicalcaseid), pnitype, General.GetNullableInteger(signeryn.Substring(0, 1)));
                EditOrder();
                gvOrderLine.Rebind();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            Guid id = Guid.Parse(((RadLabel)e.Item.FindControl("lblorderlineitem")).Text);
            PhoenixAccountsPOStaging.OrderLineStagingDelete(id, new Guid(ViewState["ORDERID"].ToString()));
            EditOrder();
            gvOrderLine.Rebind();
        }
        //Rebind();

    }

    protected void gvOrderLine_RowUpdating(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            string budgetcode = ((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text;
            string amount = ((UserControlDecimal)e.Item.FindControl("txtAmountEdit")).Text;
            string discount = ((UserControlDecimal)e.Item.FindControl("txtDiscountEdit")).Text;
            string partname = ((RadTextBox)e.Item.FindControl("txtPartNameEdit")).Text;
            string OwnerBudgetId = ((RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit")).Text;
            // string ispni = ((RadDropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlpniedit")).SelectedValue;
            string medicalcaseid = ((RadComboBox)e.Item.FindControl("ddlpnimedicalcaseedit")).SelectedValue;
            string pnitype = ((RadLabel)e.Item.FindControl("txtpnitypeedit")).Text;
            //string pnitypevalid = ((RadDropDownList)_gridView.FooterRow.FindControl("ddlpnitypeadd")).SelectedValue;
            string orderid = ((RadLabel)e.Item.FindControl("lblorderlineitem")).Text;

            string ispni = (((RadComboBox)e.Item.FindControl("ddlpnimedicalcaseedit")).SelectedIndex <= 1) ? "0" : "1";
            int medical = 1;

            if (!IsValidLineItem(budgetcode, amount, medicalcaseid, pnitype, medical))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixAccountsPOStaging.OrderLineStagingUpdate(new Guid(orderid), new Guid(ViewState["ORDERID"].ToString()), partname, int.Parse(budgetcode), decimal.Parse(amount), General.GetNullableDecimal(discount), General.GetNullableGuid(OwnerBudgetId), General.GetNullableInteger(ispni), General.GetNullableGuid(medicalcaseid), pnitype);
            EditOrder();
            gvOrderLine.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvOrderLine_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)item.DataItem;

            ImageButton db = (ImageButton)e.Item.FindControl("cmdEdit");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            ImageButton de = (ImageButton)e.Item.FindControl("cmdDelete");
            if (de != null)
            {
                de.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                de.Visible = SessionUtil.CanAccess(this.ViewState, de.CommandName);
            }
            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowBudgetEdit");
            if (ib1 != null)
            {
                ib1.Attributes.Add("onclick", "return showPickList('spnPickListBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=102&hardtypecode=30&vesselid=" + ViewState["VESSELID"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
                ib1.Visible = SessionUtil.CanAccess(this.ViewState, ib1.CommandName);
            }

            RadTextBox tb = (RadTextBox)e.Item.FindControl("txtOwnerBudgetNameEdit");
            if (tb != null) tb.Attributes.Add("style", "visibility:hidden");
            tb = (RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit");
            if (tb != null) tb.Attributes.Add("style", "visibility:hidden");
            tb = (RadTextBox)e.Item.FindControl("txtOwnerBudgetgroupIdEdit");
            if (tb != null) tb.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtBudgetIdEdit = (RadTextBox)e.Item.FindControl("txtBudgetIdEdit");
            ImageButton imgShowOwnerBudgetEdit = (ImageButton)e.Item.FindControl("imgShowOwnerBudgetEdit");
            if (imgShowOwnerBudgetEdit != null && txtBudgetIdEdit != null)
            {
                imgShowOwnerBudgetEdit.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetCodeEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=&Ownerid=" + ViewState["Ownerid"] + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetIdEdit.Text + "', true); ");          //+ "&budgetid=" + txtBudgetIdEdit.Text       
                imgShowOwnerBudgetEdit.Visible = SessionUtil.CanAccess(this.ViewState, imgShowOwnerBudgetEdit.CommandName);
            }

            RadTextBox txtpni = (RadTextBox)e.Item.FindControl("txtpni");
            RadComboBox ddlpni = (RadComboBox)e.Item.FindControl("ddlpni");
            if (txtpni != null && ddlpni != null)
                ddlpni.SelectedValue = txtpni.Text;

            RadTextBox txtpniedit = (RadTextBox)e.Item.FindControl("txtpniedit");
            RadComboBox ddlpniedit = (RadComboBox)e.Item.FindControl("ddlpniedit");
            if (txtpniedit != null && ddlpniedit != null)
            {
                ddlpniedit.SelectedValue = txtpniedit.Text;
                if (ViewState["ISPNI"] != null && ViewState["ISPNI"].ToString() == "0")
                    ddlpniedit.Enabled = false;
            }
            RadLabel txtmedicalcaseedit = (RadLabel)e.Item.FindControl("txtmedicalcaseedit");
            RadComboBox ddlpnimedicalcaseedit = (RadComboBox)e.Item.FindControl("ddlpnimedicalcaseedit");

            if (ddlpnimedicalcaseedit != null)
            {
                DataSet ds = PhoenixAccountsPNIIntergeration.MedicalPNIPoMappingList(new Guid(Request.QueryString["ORDERID"].ToString()));
                ddlpnimedicalcaseedit.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
                ddlpnimedicalcaseedit.Items.Insert(1, new RadComboBoxItem("Not Applicable", ""));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlpnimedicalcaseedit.DataSource = ds.Tables[0];

                    ddlpnimedicalcaseedit.DataTextField = "FLDMEDICALREFNO";
                    ddlpnimedicalcaseedit.DataValueField = "FLDPNIMEDICALCASEID";
                    ddlpnimedicalcaseedit.DataBind();
                    ddlpnimedicalcaseedit.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
                    ddlpnimedicalcaseedit.Items.Insert(1, new RadComboBoxItem("Not Applicable", ""));
                }
                //string pnimedicalcaseedit = drv["FLDREFERENCENO"].ToString(); ;
                ddlpnimedicalcaseedit.SelectedValue = drv["FLDPNIID"].ToString();
            }

            RadLabel txtpnitypeedit = (RadLabel)e.Item.FindControl("txtpnitypeedit");
            RadComboBoxItem ddlpnitypeedit = (RadComboBoxItem)e.Item.FindControl("ddlpnitypeedit");
            //if(ddlpnitypeedit != null)
            //{
            //    DataTable dt = PhoenixAccountsPNIIntergeration.PniTypeList();
            //    if(dt.Rows.Count>0)
            //    {
            //        ddlpnitypeedit.DataSource = dt;

            //        ddlpnitypeedit.DataTextField = "FLDTYPE";
            //        ddlpnitypeedit.DataValueField = "FLDSIGNERYN";
            //        ddlpnitypeedit.DataBind();
            //        ddlpnitypeedit.Items.Insert(0, new ListItem("--Select--", ""));
            //    }
            //    ddlpnitypeedit.SelectedValue = txtpnitypeedit.Text;

            //}
        }
        if (e.Item is GridFooterItem)
        {
            ImageButton ad = (ImageButton)e.Item.FindControl("cmdAdd");
            if (ad != null) ad.Visible = SessionUtil.CanAccess(this.ViewState, ad.CommandName);
            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowBudgetAdd");
            if (ib1 != null)
            {
                ib1.Attributes.Add("onclick", "return showPickList('spnPickListBudgetAdd', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=102&hardtypecode=30&vesselid=" + ViewState["VESSELID"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
                ib1.Visible = SessionUtil.CanAccess(this.ViewState, ib1.CommandName);
            }

            RadTextBox tb = (RadTextBox)e.Item.FindControl("txtOwnerBudgetName");
            if (tb != null) tb.Attributes.Add("style", "visibility:hidden");
            tb = (RadTextBox)e.Item.FindControl("txtOwnerBudgetId");
            if (tb != null) tb.Attributes.Add("style", "visibility:hidden");
            tb = (RadTextBox)e.Item.FindControl("txtOwnerBudgetgroupId");
            if (tb != null) tb.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtBudgetIdAdd = (RadTextBox)e.Item.FindControl("txtBudgetIdAdd");
            ImageButton imgShowOwnerBudget = (ImageButton)e.Item.FindControl("imgShowOwnerBudget");
            if (imgShowOwnerBudget != null && txtBudgetIdAdd.Text != null)
            {
                imgShowOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetCode', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=&Ownerid=" + ViewState["Ownerid"] + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetIdAdd.Text + "', true); ");          //+ "&budgetid=" + txtBudgetIdEdit.Text       
                imgShowOwnerBudget.Visible = SessionUtil.CanAccess(this.ViewState, imgShowOwnerBudget.CommandName);
            }
            //RadDropDownList ddlpniadd = (RadDropDownList)e.Row.FindControl("ddlpniadd");
            //if (ViewState["ISPNI"] != null && ViewState["ISPNI"].ToString() == "0")
            //    ddlpniadd.Enabled = false;

            RadComboBox ddlpnimedicalcaseadd = (RadComboBox)e.Item.FindControl("ddlpnimedicalcaseadd");
            if (ddlpnimedicalcaseadd != null)
            {
                DataSet ds = PhoenixAccountsPNIIntergeration.MedicalPNIPoMappingList(new Guid(Request.QueryString["ORDERID"].ToString()));


                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    ddlpnimedicalcaseadd.DataSource = dt;

                    ddlpnimedicalcaseadd.DataTextField = "FLDMEDICALREFNO";
                    ddlpnimedicalcaseadd.DataValueField = "FLDPNIMEDICALCASEID";
                    ddlpnimedicalcaseadd.DataBind();
                    ddlpnimedicalcaseadd.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
                    ddlpnimedicalcaseadd.Items.Insert(1, new RadComboBoxItem("Not Applicable", "Not Applicable"));

                }
            }

            RadComboBox ddlpnitypeadd = (RadComboBox)e.Item.FindControl("ddlpnitypeadd");
            if (ddlpnitypeadd != null)
            {
                DataTable dt = PhoenixAccountsPNIIntergeration.PniTypeList();
                if (dt.Rows.Count > 0)
                {
                    ddlpnitypeadd.DataSource = dt;

                    ddlpnitypeadd.DataTextField = "FLDTYPE";
                    ddlpnitypeadd.DataValueField = "FLDSIGNERYN";
                    ddlpnitypeadd.DataBind();
                    ddlpnitypeadd.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
                }

            }

        }
    }
    protected void gvOrderLine_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOrderLine.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvTax_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBERTax"] = ViewState["PAGENUMBERTax"] != null ? ViewState["PAGENUMBERTax"] : gvTax.CurrentPageIndex + 1;
            BindDataTax();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected bool IsValidLineItem(string budgetid, string amount, string medicalcase, string type, int medical)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (amount.Trim() == string.Empty)
            ucError.ErrorMessage = "Amount is required.";
        if (budgetid.Trim() == string.Empty)
            ucError.ErrorMessage = "Budget code is required.";
        if (General.GetNullableGuid(medicalcase) != null && type.Trim() == string.Empty)
            ucError.ErrorMessage = "Type is required.";
        if (medical == 0)
            ucError.ErrorMessage = "Select Medical case or Not applicable.";
        //if (medicalcase.Trim() == string.Empty && (type.Trim() != string.Empty || type ='--OFF SIGNER--' || type== '--ON SIGNER--'))
        //    ucError.ErrorMessage = "Medical case is required."; 

        return (!ucError.IsError);
    }
    protected bool IsValidTaxAdditonal(string description, string budgetid, string amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (description.Trim() == string.Empty)
            ucError.ErrorMessage = "Description is required.";
        if (budgetid.Trim() == string.Empty)
            ucError.ErrorMessage = "Budget code is required.";
        if (amount.Trim() == string.Empty)
            ucError.ErrorMessage = "Amount is required.";

        return (!ucError.IsError);
    }
    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }

    }
    protected void RebindgvTax()
    {
        gvTax.SelectedIndexes.Clear();
        gvTax.EditIndexes.Clear();
        gvTax.DataSource = null;
        gvTax.Rebind();
    }
    protected void Rebind()
    {
        gvOrderLine.SelectedIndexes.Clear();
        gvOrderLine.EditIndexes.Clear();
        gvOrderLine.DataSource = null;
        gvOrderLine.Rebind();
    }
}

