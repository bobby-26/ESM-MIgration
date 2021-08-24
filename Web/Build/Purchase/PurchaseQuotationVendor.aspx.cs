using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class PurchaseQuotationVendor : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbarsave = new PhoenixToolbar();
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            txtVenderID.Attributes.Add("style", "visibility:hidden");
            //cmdHiddenPick.Attributes.Add("style", "visibility:hidden");
            btnPickVender.Visible = SessionUtil.CanAccess(this.ViewState, btnPickVender.CommandName);

            MenuVender.Title = "Quotation ( " + Filter.CurrentPurchaseFormNumberSelection + " )";
            if (Request.QueryString["orderid"] != null)
            {
                ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                if (Request.QueryString["quotationid"] != null)
                {
                    toolbarsave.AddButton("Contacts", "CONTACTS", ToolBarDirection.Right);
                    toolbarsave.AddButton("Attachment", "ATTACHMENT", ToolBarDirection.Right);
                    toolbarsave.AddButton("Comments", "COMMENTS", ToolBarDirection.Right);
                    toolbarsave.AddButton("Remarks", "REMARKS", ToolBarDirection.Right);
                    toolbarsave.AddButton("Delivery", "DELIVERYINSTRUCTION", ToolBarDirection.Right);
                }
            }
            toolbarsave.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbarsave.AddButton("New", "NEW", ToolBarDirection.Right);

            MenuVender.AccessRights = this.ViewState;
            MenuVender.MenuList = toolbarsave.Show();

            if (!IsPostBack)
            {

                short showcreditnote = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
                lblCreditNoteDisc.Visible = (showcreditnote == 1) ? true : false;
                txtDiscount.Visible = (showcreditnote == 1) ? true : false;
                cmdDiscount.Visible = (showcreditnote == 1) ? true : false;
                lblTotalDisc.Visible = (showcreditnote == 1) ? true : false;
                ucTotalDiscount.Visible = (showcreditnote == 1) ? true : false;
                lblUSDTotalDisc.Visible = (showcreditnote == 1) ? true : false;
                txtTotalDiscount.Visible = (showcreditnote == 1) ? true : false;
                txtusercode.Attributes.Add("style", "display:none");
                txtSentById.Attributes.Add("style", "visibility:hidden");
                txtRejectedById.Attributes.Add("style", "visibility:hidden");

                UCDeliveryTerms.QuickTypeCode = ((int)PhoenixQuickTypeCode.DELIVERYTERM).ToString();
                UCPaymentTerms.QuickTypeCode = ((int)PhoenixQuickTypeCode.PAYMENTTERM).ToString();
                ViewState["Lock"] = "N";
                ucCurrency.SelectedCurrency = PhoenixPurchaseOrderForm.DefaultCurrency;
                ViewState["SaveStatus"] = "New";

                BindVesselPrincipal();

                if (Request.QueryString["orderid"] != null)
                {
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                    if (Request.QueryString["quotationid"] != null)
                    {
                        ViewState["quotationid"] = Request.QueryString["quotationid"].ToString();
                        BindFields(ViewState["quotationid"].ToString());
                        ViewState["SaveStatus"] = "Edit";
                        BindPreferredby();
                    }
                    else
                    {
                        ViewState["quotationid"] = "";
                    }

                }
                else
                {
                    ViewState["orderid"] = 0;
                }

                ViewState["OwnerBudgetId"] = "";
                //cmdDiscount.Attributes.Add("onclick", "return openCreditNoteDiscount(); ");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindFields(string quotationid)
    {
        DataSet quotationdataset = new DataSet();
        quotationdataset = PhoenixPurchaseQuotation.EditQuotation(new Guid(quotationid));
        if (quotationdataset.Tables[0].Rows.Count > 0)
        {
            DataRow dr = quotationdataset.Tables[0].Rows[0];
            txtVenderCode.Text = dr["FLDVENDORCODE"].ToString();
            txtVenderID.Text = dr["FLDVENDORID"].ToString();
            txtVenderName.Text = dr["FLDNAME"].ToString();
            PhoenixPurchaseQuotation.VendorName = dr["FLDNAME"].ToString();
            txtQtnRefenceno.Text = dr["FLDVENDORQUOTATIONREF"].ToString();
            txtRejectedDate.Text = General.GetNullableDateTime(dr["FLDREJECTEDDATE"].ToString()) != null ? General.GetDateTimeToString(dr["FLDREJECTEDDATE"].ToString()) : "";
            txtRecivedDate.Text = General.GetNullableDateTime(dr["FLDRECEIVEDDATE"].ToString()) != null ? General.GetDateTimeToString(dr["FLDRECEIVEDDATE"].ToString()) : "";
            txtOrderDate.Text = General.GetNullableDateTime(dr["FLDORDERBEFOREDELIVERYDATE"].ToString())!=null?General.GetDateTimeToString(dr["FLDORDERBEFOREDELIVERYDATE"].ToString()):"";
            txtDeliveryTime.Text = dr["FLDDELIVERYTIME"].ToString();
            txtSentDate.Text = General.GetNullableDateTime(dr["FLDSENTDATE"].ToString()) != null ? General.GetDateTimeToString(dr["FLDSENTDATE"].ToString()) : "";
            //txtRemorks.Text = dr["FLDREMARKS"].ToString();
            ViewState["PURCHASEAPPROVEDBY"] = dr["FLDPURCHASEAPPROVEDBY"];
            //ViewState["PURCHASEAPPROVEDATE"] = dr["FLDPURCHASEAPPROVEDATE"];
            txtSentById.Text = dr["FLDSENTBY"].ToString();
            txtNameSentBy.Text = dr["FLDSENTBYNAME"].ToString();

            txtRejectedById.Text = dr["FLDREJECTEDBY"].ToString();
            txtNameRejectedBy.Text = dr["FLDREJECTEDBYNAME"].ToString();
            txtSupplierDiscount.Text = dr["FLDSUPPLIERDISCOUNT"].ToString();
            txtusername.Text = dr["FLDAPPROVALBYNAME"].ToString();
            txtusercode.Text = dr["FLDAPPROVALBY"].ToString();

            if (dr["FLDQUOTEDCURRENCYID"] != null && dr["FLDQUOTEDCURRENCYID"].ToString().Trim() != "")
                ucCurrency.SelectedCurrency = dr["FLDQUOTEDCURRENCYID"].ToString();
            else
                ucCurrency.SelectedCurrency = PhoenixPurchaseOrderForm.DefaultCurrency;
            ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
            txtDiscount.Text = String.Format("{0:##.0000000}", dr["FLDDISCOUNT"]);
            txtRecivedDate.Enabled = true;
            txtOrderDate.Enabled = true;
            txtExpirationDate.Text = General.GetNullableDateTime(dr["FLDEXPIRYDATE"].ToString()) != null ? General.GetDateTimeToString(dr["FLDEXPIRYDATE"].ToString()) : "";
            txtExpirationDate.Enabled = true;

            if (dr["FLDSENTDATE"].ToString().Equals(""))
            {
                txtRecivedDate.Enabled = false;
                txtOrderDate.Enabled = true;
            }

            if (!dr["FLDRECEIVEDDATE"].ToString().Equals(""))
            {
                txtRecivedDate.Enabled = false;
                //txtQtnRefenceno.Enabled = false;
                txtOrderDate.Enabled = false;


                txtRate.Text = String.Format("{0:###,###,###,##0.00}", dr["FLDQUOTEDPRICE"]);
                lblExchangeRate.Text = String.Format("{0:###,###,###,###,###.000000000}", dr["EXCHANGERATE"]);
                ucTotalDiscount.Text = String.Format("{0:###,###,###,###,###.00}", dr["FLDTOTALDISCOUNT"]);
                ucTotalAmount.Text = String.Format("{0:###,###,###,###,###.00}", dr["FLDTOTALAMOUNT"]);

                txtUsdPrice.Text = String.Format("{0:###,###,###,##0.00}", (decimal.Parse(dr["FLDUSDPRICE"].ToString())));
                txtTotalDiscount.Text = String.Format("{0:###,###,###,###.00}", (decimal.Parse(dr["FLDUSDDISCOUNT"].ToString())));
                txtTotalInUSD.Text = String.Format("{0:###,###,###,###.00}", (decimal.Parse(dr["TOTALUSDAMOUNT"].ToString())));


            }
            if (!dr["FLDORDEREDDATE"].ToString().Equals(""))
            {
                ViewState["Lock"] = "Y";
            }
            if (!dr["FLDEMAIL1"].ToString().Equals(""))
                lblEmailIds.Text = dr["FLDEMAIL1"].ToString() + ",";
            if (!dr["FLDEMAIL2"].ToString().Equals(""))
                lblEmailIds.Text = lblEmailIds.Text + dr["FLDEMAIL2"].ToString() + ",";
            //if (!dr["FLDSUPTEMAIL"].ToString().Equals(""))
            //    lblEmailIds.Text =lblEmailIds.Text+ dr["FLDSUPTEMAIL"].ToString() + ",";
            if (!dr["FLDSUPPLIEREMAIL"].ToString().Equals(""))
                lblEmailIds.Text = lblEmailIds.Text + dr["FLDSUPPLIEREMAIL"].ToString();


            txtDiscount.ReadOnly = "false";
            txtDeliveryTime.ReadOnly = "false";
            btnPickVender.Enabled = false;

            UCDeliveryTerms.SelectedQuick = dr["FLDVENDORDELIVERYTERMID"].ToString();
            UCPaymentTerms.SelectedQuick = dr["FLDVENDORPAYMENTTERMID"].ToString();
            ucModeOfTransport.SelectedQuick = dr["FLDMODEOFTRANSPORT"].ToString();

            if (dr["FLDISSELECTEDFORORDER"].ToString().ToUpper().Equals("TRUE"))
            {
                ucCurrency.Enabled = false;
                txtDeliveryTime.ReadOnly = "true";
                txtDiscount.ReadOnly = "true";
            }

            if (dr["FLDPURCHASEAPPROVEDBY"].ToString().Equals("1"))
            {
                ddlAccountDetails.Enabled = false;
            }

            DataTable dt = PhoenixPurchaseApprovedVesselAccount.VesselAccountSearch(
                new Guid(ViewState["quotationid"].ToString()),
                new Guid(ViewState["orderid"].ToString()));

            if (dt.Rows.Count > 0)
                ddlAccountDetails.SelectedValue = dt.Rows[0]["FLDVESSELACCOUNTID"].ToString();
            else
                ddlAccountDetails.SelectedValue = "";

            ddlType.SelectedHard = dr["FLDITEMTYPE"].ToString();


            if (dr["FLDISSELECTEDFORORDER"].ToString().ToUpper().Equals("TRUE"))
            {
                txtPartPaid.Text = String.Format("{0:##,###,###.00}", dr["FLDPARTPAYMENT"]);
                cmdPicPartPaid.Attributes.Add("onclick", "return showPickList('spnPicPartPaid', 'codehelp1', '', 'Purchase/PurchaseOrderPartPaid.aspx?orderid=" + ViewState["orderid"].ToString() + "&vendorid=" + dr["FLDVENDORID"].ToString() + "', true); ");
            }
            cmdDiscount.Attributes.Add("onclick", "return showPickList('spnDiscount', 'codehelp1', '', 'Purchase/PurchaseQuotationEsmDiscount.aspx?quotationid=" + ViewState["quotationid"].ToString() + "&discount=" + txtDiscount.Text + "', true); ");
            hdnprincipalId.Value = dr["FLDPRINCIPALID"].ToString();
        }
    }

    protected void gvTax_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();
        if (ViewState["SaveStatus"].ToString().Equals("New"))
        {
            ds = PhoenixPurchaseQuotation.QuotationTaxMapListFinalize(null, null);
        }
        else
        {
            ds = PhoenixPurchaseQuotation.QuotationTaxMapListFinalize(null, new Guid(ViewState["quotationid"].ToString()));
        }
         
        gvTax.DataSource = ds;

    }
    protected void gvTax_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                DeleteQuotationTax(new Guid(item.GetDataKeyValue("FLDQUOTATIONTAXMAPCODE").ToString())
                                     , new Guid(ViewState["quotationid"].ToString()));

                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }
    protected void gvTax_EditCommand(object sender, GridCommandEventArgs e)
    {

    }
    protected void gvTax_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem item = (GridFooterItem)e.Item;
                if (!IsValidTax(
                    ((RadTextBox)item.FindControl("txtDescriptionAdd")).Text.ToString().Trim(),
                    ((UserControlTaxType)item.FindControl("ucTaxTypeAdd")).TaxType.ToString(),
                    ((UserControlDecimal)item.FindControl("txtValueAdd")).Text.ToString(),
                     ViewState["quotationid"].ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertQuotationTax(
                                     new Guid(ViewState["quotationid"].ToString())
                                    , ((RadTextBox)item.FindControl("txtDescriptionAdd")).Text.ToString().Trim()
                                    , int.Parse(((UserControlTaxType)item.FindControl("ucTaxTypeAdd")).TaxType)
                                    , decimal.Parse(((UserControlDecimal)item.FindControl("txtValueAdd")).Text.ToString())
                                    , 0
                                    ,null
                                    , ((RadTextBox)item.FindControl("txtOwnerBudgetId")).Text.ToString().Trim()
                                    , General.GetNullableInteger(((RadTextBox)item.FindControl("txtBudgetId")).Text.ToString().Trim())
                                );

                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvTax_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            GridDataItem item = (GridDataItem)e.Item;

            if (!IsValidTax(
              ((RadTextBox)item.FindControl("txtDescriptionEdit")).Text.ToString().Trim(),
              ((UserControlTaxType)item.FindControl("ucTaxTypeEdit")).TaxType.ToString(),
              ((UserControlDecimal)item.FindControl("txtValueEdit")).Text.ToString()
              , ViewState["quotationid"].ToString()))
            {
                ucError.Visible = true;
                return;
            }

            UpdateQuotationTax(
                                 new Guid(item.GetDataKeyValue("FLDQUOTATIONTAXMAPCODE").ToString())
                                , new Guid(ViewState["quotationid"].ToString())
                                , ((RadTextBox)item.FindControl("txtDescriptionEdit")).Text.ToString().Trim()
                                , int.Parse(((UserControlTaxType)item.FindControl("ucTaxTypeEdit")).TaxType)
                                , decimal.Parse(((UserControlDecimal)item.FindControl("txtValueEdit")).Text.ToString())
                                , 0
                                ,null
                                , ((RadTextBox)item.FindControl("txtOwnerBudgetIdEdit")).Text.ToString().Trim()
                                    , General.GetNullableInteger(((RadTextBox)item.FindControl("txtBudgetIdEdit")).Text.ToString().Trim())
                            );
            String script = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTax_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            GridEditableItem item = (GridEditableItem)e.Item;
            LinkButton db = (LinkButton)item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
        }

        if (e.Item is GridEditableItem)
        {
            GridEditableItem item = (GridEditableItem)e.Item;
            LinkButton cmdEdit = (LinkButton)item.FindControl("cmdEdit");
            if (cmdEdit != null)
                cmdEdit.Visible = Request.QueryString["VIEWONLY"] == "Y" ? false : SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);
            LinkButton cmdSave = (LinkButton)item.FindControl("cmdSave");
            if (cmdSave != null)
                cmdSave.Visible = Request.QueryString["VIEWONLY"] == "Y" ? false : SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);
            LinkButton cmdCancel = (LinkButton)item.FindControl("cmdCancel");
            if (cmdCancel != null)
                cmdCancel.Visible = Request.QueryString["VIEWONLY"] == "Y" ? false : SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName);
            LinkButton cmdDelete = (LinkButton)item.FindControl("cmdDelete");
            if (cmdDelete != null)
                cmdDelete.Visible = Request.QueryString["VIEWONLY"] == "Y" ? false : SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);

            DataRowView drv = (DataRowView)item.DataItem;
            if (drv["FLDISGST"].ToString() == "1")
            {
                UserControlTaxType uctTaxType = (UserControlTaxType)item.FindControl("ucTaxTypeEdit");
                if (uctTaxType != null) uctTaxType.Enabled = false;
            }

            LinkButton ib1 = (LinkButton)item.FindControl("btnShowBudgetEdit");
            if (ib1 != null)
            {
                if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SPARE"))
                    ib1.Attributes.Add("onclick", "return showPickList('spnPickListMainBudgetEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + Filter.CurrentPurchaseVesselSelection + "&budgetdate=" + DateTime.Now.Date + "', true); ");
                else if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SERVICE"))
                    ib1.Attributes.Add("onclick", "return showPickList('spnPickListMainBudgetEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=107&hardtypecode=30&vesselid=" + Filter.CurrentPurchaseVesselSelection + "&budgetdate=" + DateTime.Now.Date + "', true); ");
                else
                    ib1.Attributes.Add("onclick", "return showPickList('spnPickListMainBudgetEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=105&hardtypecode=30&vesselid=" + Filter.CurrentPurchaseVesselSelection + "&budgetdate=" + DateTime.Now.Date + "', true); ");

                if (!SessionUtil.CanAccess(this.ViewState, ib1.CommandName))
                    ib1.Visible = false;
            }
            LinkButton ib2 = (LinkButton)item.FindControl("btnShowOwnerBudgetEdit");
            RadTextBox txtBudgetIdEdit = (RadTextBox)item.FindControl("txtBudgetIdEdit");
            if (ib2 != null)
            {
                if (General.GetNullableInteger(drv["FLDBUDGETID"].ToString()) != null)
                    ib2.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', 'Common/CommonPickListOwnerBudget.aspx?vesselid=" + Filter.CurrentPurchaseVesselSelection + "&budgetdate=" + DateTime.Now.Date + "&budgetid=" + drv["FLDBUDGETID"].ToString() + "&Ownerid=" + hdnprincipalId.Value + "', true); ");
                else
                    ib2.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', 'Common/CommonPickListOwnerBudget.aspx?vesselid=" + Filter.CurrentPurchaseVesselSelection + "&budgetdate=" + DateTime.Now.Date + "&Ownerid=" + hdnprincipalId.Value + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, ib2.CommandName))
                    ib2.Visible = false;
            }
        }

        if (e.Item is GridFooterItem)
        {
            GridFooterItem item = (GridFooterItem)e.Item;
            LinkButton cmdAdd = (LinkButton)item.FindControl("cmdAdd");
            if (cmdAdd != null)
                cmdAdd.Visible = Request.QueryString["VIEWONLY"] == "Y" ? false : SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);

            LinkButton ib1 = (LinkButton)item.FindControl("btnShowBudget");
            if (ib1 != null)
            {
                if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SPARE"))
                    ib1.Attributes.Add("onclick", "return showPickList('spnPickListMainBudget', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + Filter.CurrentPurchaseVesselSelection + "&budgetdate=" + DateTime.Now.Date + "', true); ");
                else if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SERVICE"))
                    ib1.Attributes.Add("onclick", "return showPickList('spnPickListMainBudget', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=107&hardtypecode=30&vesselid=" + Filter.CurrentPurchaseVesselSelection + "&budgetdate=" + DateTime.Now.Date + "', true); ");
                else
                    ib1.Attributes.Add("onclick", "return showPickList('spnPickListMainBudget', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=105&hardtypecode=30&vesselid=" + Filter.CurrentPurchaseVesselSelection + "&budgetdate=" + DateTime.Now.Date + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, ib1.CommandName))
                    ib1.Visible = false;
            }
            LinkButton ib2 = (LinkButton)item.FindControl("btnShowOwnerBudget");
            RadTextBox txtBudgetId = (RadTextBox)item.FindControl("txtBudgetId");
            if (ib2 != null && txtBudgetId != null)
            {
                ib2.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudget', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListOwnerBudget.aspx?vesselid=" + Filter.CurrentPurchaseVesselSelection + "&budgetdate=" + DateTime.Now.Date + "&budgetid=" + txtBudgetId.Text + "&Ownerid=" + hdnprincipalId.Value + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, ib2.CommandName))
                    ib2.Visible = false;
            }
        }
    }
    protected bool IsValidTax(string strDescription, string strValueType, string strValue, string quotationid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (strDescription.Trim() == string.Empty)
            ucError.ErrorMessage = "Description is required.";
        if (strValue.Trim() == string.Empty)
            ucError.ErrorMessage = "Value is required.";
        if (strValueType.Trim() == string.Empty)
            ucError.ErrorMessage = "Type is required.";
        if (General.GetNullableGuid(quotationid) == null)
            ucError.ErrorMessage = "Invalid Quotation .";


        return (!ucError.IsError);
    }
    
    protected void InsertQuotationTax(Guid uQuotationId, string strDescription, int iTaxType, decimal dValue, int iOffsetVessel, string strBudgetCode, string ownerbudgetcode, int? budgetid)
    {

        PhoenixPurchaseQuotation.QuotationTaxMapInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, uQuotationId, strDescription, iTaxType, dValue, iOffsetVessel, General.GetNullableString(strBudgetCode), 1, General.GetNullableGuid(ownerbudgetcode), budgetid);

    }

    protected void UpdateQuotationTax(Guid uTaxMapCode, Guid uQuotationId, string strDescription, int iTaxType, decimal dValue, int iOffsetVessel, string strBudgetCode, string ownerbudgetcode, int? budgetid)
    {

        PhoenixPurchaseQuotation.QuotationTaxMapUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, uTaxMapCode, uQuotationId, strDescription, iTaxType, dValue, iOffsetVessel, General.GetNullableString(strBudgetCode), General.GetNullableGuid(ownerbudgetcode), budgetid);
    }

    protected void DeleteQuotationTax(Guid uTaxMapCode, Guid uQuotationId)
    {
        PhoenixPurchaseQuotation.QuotationTaxMapDelete(uTaxMapCode, uQuotationId);
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    }

    protected void MenuVender_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["SaveStatus"].ToString().Equals("New"))
                {
                    if (!IsValidVender())
                    {
                        ucError.Visible = true;
                        return;
                    }
                    InsertOrderVender();
                }
                else if (ViewState["SaveStatus"].ToString().Equals("Edit"))
                {
                    UpdateOrderVender();
                }
                String script = String.Format(" closeTelerikWindow('code1','detail','t');");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            if (CommandName.ToUpper().Equals("NEW"))
            {
                ClearTextBox();

                DataSet ds = PhoenixPurchaseQuotation.QuotationTaxMapListFinalize(null, null);
                gvTax.DataSource = ds;
                DataTable dt = new DataTable();
                rptPreferredby.Visible = false;
                ViewState["SaveStatus"] = "New";
            }
            if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                String scriptpopup = String.Format("javascript:openNewWindow('codehelp1', '', 'Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.PURCHASE + "');");
                RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (CommandName.ToUpper().Equals("REMARKS"))
            {
                String scriptpopup = String.Format(
                    "javascript:openNewWindow('codehelp1', '', 'Purchase/PurchaseVendorDetail.aspx?QUOTATIONID=" + ViewState["quotationid"].ToString() +
                    "&editable=false');");
                RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (CommandName.ToUpper().Equals("COMMENTS"))
            {
                String scriptpopup = String.Format(
                    "javascript:openNewWindow('codehelp1', '', 'Purchase/PurchaseQuotationComments.aspx?QUOTATIONID=" + ViewState["quotationid"].ToString() + "&Dtkey="+ViewState["DTKEY"].ToString()+"');");
                RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (CommandName.ToUpper().Equals("DELIVERYINSTRUCTION"))
            {
                String scriptpopup = String.Format(
                    "javascript:openNewWindow('codehelp1', '', 'Purchase/PurchaseQuotationDeliveryInstruction.aspx?QUOTATIONID=" + ViewState["quotationid"].ToString() +
                    "&editable=true');");
                RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (CommandName.ToUpper().Equals("CONTACTS"))
            {
                String scriptpopup = String.Format("javascript:openNewWindow('codehelp1', '', 'Purchase/PurchaseQuotationContacts.aspx?quotationid=" + ViewState["quotationid"].ToString() + "');");
                RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ClearTextBox()
    {
        txtVenderCode.Text = "";
        txtVenderName.Text = "";
        txtVenderReference.Text = "";
        txtSentDate.Text = "";
        txtExpirationDate.Text = "";
        txtRejectedDate.Text = "";
        txtRecivedDate.Text = "";
        txtRecivedDate.Enabled = false;
        txtOrderDate.Enabled = false;
        txtExpirationDate.Enabled = true;
        txtRate.Text = "";
        txtOrderDate.Text = "";
        txtDiscount.Text = "";
        txtDeliveryTime.Text = "";
        txtUsdPrice.Text = "";
        txtTotalDiscount.Text = "";
        txtTotalInUSD.Text = "";
        ucTotalAmount.Text = "";
        ucTotalDiscount.Text = "";
        lblExchangeRate.Text = "";
        txtNameSentBy.Text = "";
        txtSentById.Text = "";
        txtRejectedById.Text = "";
        txtNameRejectedBy.Text = "";
        txtDiscount.ReadOnly = "true";
        txtDeliveryTime.ReadOnly = "true";
        btnPickVender.Enabled = true;
        ucCurrency.SelectedCurrency = PhoenixPurchaseOrderForm.DefaultCurrency;
        txtQtnRefenceno.Text = "";
        UCDeliveryTerms.SelectedQuick = "";
        UCPaymentTerms.SelectedQuick = "";
        ucModeOfTransport.SelectedQuick = "";
        txtSupplierDiscount.Text = "";
    }

    private void InsertOrderVender()
    {
        PhoenixPurchaseQuotation.InsertQuotation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableInteger(txtSentById.Text),
            Int32.Parse(txtVenderID.Text), new Guid(ViewState["orderid"].ToString()),
            General.GetNullableInteger(txtRejectedById.Text), General.GetNullableDateTime(txtRejectedDate.Text),
            General.GetNullableDateTime(txtSentDate.Text), General.GetNullableDateTime(txtRecivedDate.Text),
            General.GetNullableDateTime(txtExpirationDate.Text), General.GetNullableDecimal(txtRate.Text),
            General.GetNullableInteger(ucCurrency.SelectedCurrency), General.GetNullableDecimal(txtDiscount.Text),
            General.GetNullableInteger(txtDeliveryTime.Text), txtQtnRefenceno.Text,
            General.GetNullableDateTime(txtOrderDate.Text),
            General.GetNullableInteger(ucModeOfTransport.SelectedQuick),
            General.GetNullableInteger(ddlType.SelectedHard));
    }

    private void UpdateOrderVender()
    {
        PhoenixPurchaseQuotation.UpdateQuotation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            new Guid(ViewState["quotationid"].ToString()),
            General.GetNullableInteger(txtRejectedById.Text), General.GetNullableDateTime(txtRejectedDate.Text),
            General.GetNullableDateTime(txtSentDate.Text), General.GetNullableDateTime(txtRecivedDate.Text),
            General.GetNullableDateTime(txtExpirationDate.Text),
            General.GetNullableInteger(ucCurrency.SelectedCurrency), General.GetNullableDecimal(txtDiscount.Text),
            General.GetNullableDecimal(txtDeliveryTime.Text), txtQtnRefenceno.Text,
            General.GetNullableDateTime(txtOrderDate.Text),
            General.GetNullableInteger(ucModeOfTransport.SelectedQuick),
            General.GetNullableInteger(UCDeliveryTerms.SelectedQuick),
            General.GetNullableInteger(UCPaymentTerms.SelectedQuick),
            General.GetNullableDecimal(txtSupplierDiscount.Text),
            General.GetNullableString(lblEmailIds.Text),
            General.GetNullableInteger(ddlType.SelectedHard));

    }

    private bool IsValidVender()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtVenderID.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Vendor is required. Please Select ";
        if (General.GetNullableDateTime(txtExpirationDate.Text) != null && Convert.ToDateTime(txtExpirationDate.Text) <= Convert.ToDateTime(DateTime.Now.ToString()))
            ucError.ErrorMessage = "Expiry  date should be greater than today's date.";
        if (ViewState["orderid"].ToString() == "0")
            ucError.ErrorMessage = "Please select a form to assign a vendor ";
        return (!ucError.IsError);
    }

    protected void ddlAccountDetails_DataBound(object sender, EventArgs e)
    {
        ddlAccountDetails.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void ddlAccountDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["quotationid"] == null)
            {
                ucError.ErrorMessage = "Please Add a Vendor";
                ucError.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdMapUser_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (ViewState["quotationid"].ToString() == null || ViewState["quotationid"].ToString() == "")
            {
                ucError.ErrorMessage = "Please Add a Vendor";
                ucError.Visible = true;
                return;
            }
            if (General.GetNullableInteger(txtusercode.Text) == null)
            {
                ucError.ErrorMessage = "Approval By is required.";
                ucError.Visible = true;
                return;
            }
            int? approvalby = General.GetNullableInteger(txtusercode.Text);
            string stocktype = Filter.CurrentPurchaseStockType;

            PhoenixPurchaseQuotation.QuotaitionApprovalMap(new Guid(ViewState["orderid"].ToString()), new Guid(ViewState["quotationid"].ToString()), approvalby, stocktype);
            ucStatus.Text = "Successfully Mapped";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindPreferredby()
    {

        if (General.GetNullableInteger(txtVenderID.Text) != null && PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() != "OWNER")
        {
            DataSet ds = PhoenixRegistersAddressRelation.ListAddressOwnerMapping(
                            int.Parse(txtVenderID.Text));

            if (ds.Tables[0].Rows.Count > 0)
            {
                rptPreferredby.DataSource = ds.Tables[0];
                rptPreferredby.DataBind();
                rptPreferredby.Visible = true;
            }

        }
    }
    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        BindPreferredby();
    }
    private void BindVesselPrincipal()
    {
        if (Filter.CurrentPurchaseVesselSelection >0)
        {
            DataSet ds = PhoenixRegistersVessel.EditVessel(Filter.CurrentPurchaseVesselSelection);
            hdnprincipalId.Value = ds.Tables[0].Rows[0]["FLDPRINCIPAL"].ToString();          
        }      
    }

    
}
