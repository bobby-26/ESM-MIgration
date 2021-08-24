using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class VesselAccountsOrderFormGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                if (Request.QueryString["storeclass"].ToString() == "412")
                {
                    //MenuCrewBond.Title = "Requisition of Bond (" + PhoenixSecurityContext.CurrentSecurityContext.VesselName + ")";

                    lblLineItemAmount.Text = "Total Bond Cost (USD)(A)";
                }
                else if (Request.QueryString["storeclass"].ToString() == "411")
                {
                    //  MenuCrewBond.Title = "Requisition of Provisions (" + PhoenixSecurityContext.CurrentSecurityContext.VesselName + ")";
                    lblLineItemAmount.Text = "Total Provision Cost (USD)(A)";
                    lblForStock.Visible = false;
                    chkForStock.Visible = false;
                }
                txtSupplierId.Attributes.Add("style", "display:none");
                ViewState["ISSTOCKYN"] = "";
                ViewState["ORDERID"] = Request.QueryString["ORDERID"] == null ? null : Request.QueryString["ORDERID"];
                ViewState["DTKEY"] = Request.QueryString["DTKEY"] == null ? null : Request.QueryString["DTKEY"];
                ViewState["NEWPROCESS"] = Request.QueryString["NEWPROCESS"] == null ? "0" : Request.QueryString["NEWPROCESS"];
                ViewState["ISSTOCKYN"] = Request.QueryString["ISSTOCKYN"] == null ? null : Request.QueryString["ISSTOCKYN"];
                rblPaymentTerm.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 125);
                rblPaymentTerm.DataBind();
                ViewState["ACTIVE"] = "1";
                ViewState["REQNO"] = "";
                ddlPaidcurrency.VesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                txtOrderDate.Text = DateTime.Now.ToShortDateString();
                if (ViewState["ORDERID"] != null)
                {
                    EditBondProvision(new Guid(ViewState["ORDERID"].ToString()));
                    if (ViewState["NEWPROCESS"].ToString() == "0")
                    {
                        lblForStock.Visible = false;
                        chkForStock.Visible = false;
                    }
                    chkForStock.Enabled = false;
                }
                else
                {
                    if (ViewState["NEWPROCESS"].ToString() == "0")
                    {
                        lblForStock.Visible = false;
                        chkForStock.Visible = false;
                    }
                    else
                    {
                        lblForStock.Visible = true;
                        chkForStock.Visible = true;
                    }
                    int vesselcurrency = 0;
                    decimal Exchagerate = 0;

                    DataSet ds = PhoenixVesselAccountsCurrencyConfiguration.ListGetExchangeRate(PhoenixSecurityContext.CurrentSecurityContext.VesselID, null
                                                                                , DateTime.Parse(DateTime.Now.ToString()), ref Exchagerate, ref vesselcurrency, General.GetNullableInteger("1"));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlPaidcurrency.SelectedCurrency = ds.Tables[0].Rows[0]["FLDVESSELCURRENCYID"].ToString();
                        if (Request.QueryString["storeclass"].ToString() == "412")
                        {
                            //     MenuCrewBond.Title = "Requisition of Bond (" + PhoenixSecurityContext.CurrentSecurityContext.VesselName + ")";
                            lblLineItemAmount.Text = "Total Bond Cost (" + ds.Tables[0].Rows[0]["FLDCURRENCYCODE"].ToString() + ")(A)";
                        }
                        else if (Request.QueryString["storeclass"].ToString() == "411")
                        {
                            //   MenuCrewBond.Title = "Requisition of Provisions (" + PhoenixSecurityContext.CurrentSecurityContext.VesselName + ")";
                            lblLineItemAmount.Text = "Total Provision Cost (" + ds.Tables[0].Rows[0]["FLDCURRENCYCODE"].ToString() + ")(A)";
                        }
                        lblExchangeRate1USD.Text = "Exchange Rate (1 " + ds.Tables[0].Rows[0]["FLDCURRENCYCODE"].ToString() + " = )";
                        lbldeliverytotal.Text = "Charges after Discount (" + ds.Tables[0].Rows[0]["FLDCURRENCYCODE"].ToString() + ")(B)";
                        lblgrandtotal.Text = "Grand Total (" + ds.Tables[0].Rows[0]["FLDCURRENCYCODE"].ToString() + ")(A+B) ";
                    }
                    rblPaymentTerm.SelectedValue = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 125, "CRP");
                }
            }
            MainMenu();
            ResetMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlPaidcurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int vesselcurrency = 0;
            decimal Exchagerate = 0;
            if (General.GetNullableDateTime(txtReceivedDate.Text) == null)
            {
                ucError.Text = "Received Date is Required.";
                ucError.Visible = true;
                return;
            }
            DataSet ds = PhoenixVesselAccountsCurrencyConfiguration.ListGetExchangeRate(PhoenixSecurityContext.CurrentSecurityContext.VesselID, int.Parse(ddlPaidcurrency.SelectedCurrency)
                                                                        , DateTime.Parse(txtReceivedDate.Text), ref Exchagerate, ref vesselcurrency, General.GetNullableInteger("1"));

            if (ds.Tables[0].Rows.Count > 0)
            {//  txtExchangeRate.Text = ds.Tables[0].Rows[0]["FLDEXCHANGERATE"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ResetMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("General", "GENERAL");
        if (ViewState["ORDERID"] != null)
            toolbar.AddButton("Line Item", "LINEITEM");
        if (ViewState["ISSTOCKYN"] != null && ViewState["ISSTOCKYN"].ToString() == "0")
            toolbar.AddButton("Employee List", "EMPSTOCK");
        toolbar.AddButton("List", "LIST");
        MenuOrderForm.AccessRights = this.ViewState;
        MenuOrderForm.MenuList = toolbar.Show();
        MenuOrderForm.SelectedMenuIndex = 0;
    }
    protected void OrderForm_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("GENERAL"))
                Response.Redirect("../VesselAccounts/VesselAccountsOrderFormGeneral.aspx?storeclass=" + Request.QueryString["storeclass"].ToString() + "&orderid=" + ViewState["ORDERID"] + "&pageno=" + ViewState["PAGENUMBER"] + "&NewProcess=" + ViewState["NEWPROCESS"], false);
            else if (CommandName.ToUpper().Equals("LINEITEM"))
                Response.Redirect("../VesselAccounts/VesselAccountsOrderFormLineItem.aspx?storeclass=" + Request.QueryString["storeclass"].ToString() + "&orderid=" + ViewState["ORDERID"] + "&pageno=" + ViewState["PAGENUMBER"] + "&ISSTOCKYN=" + ViewState["ISSTOCKYN"].ToString() + "&NewProcess=" + ViewState["NEWPROCESS"], false);
            else if (CommandName.ToUpper().Equals("LIST"))
                Response.Redirect("../VesselAccounts/VesselAccountsOrderForm.aspx?storeclass=" + Request.QueryString["storeclass"].ToString(), false);
            else if (CommandName.ToUpper().Equals("EMPSTOCK"))
                Response.Redirect("../VesselAccounts/VesselAccountsEmployeeBondRequisitionLineItem.aspx?storeclass=" + Request.QueryString["storeclass"].ToString() + "&orderid=" + ViewState["ORDERID"] + "&pageno=" + ViewState["PAGENUMBER"] + "&ISSTOCKYN=" + ViewState["ISSTOCKYN"].ToString() + "&NewProcess=" + ViewState["NEWPROCESS"] + "&REQNO=" + ViewState["REQNO"], false);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCrewBond_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            string eta = (txtETATime.Text.Replace("AM", "").Replace("PM", "").Trim() == "00:00") ? "00:00" : txtETATime.Text.Substring(0, 2) + ":" + txtETATime.Text.Substring(2);
            string etb = (txtETBTime.Text.Replace("AM", "").Replace("PM", "").Trim() == "00:00") ? "00:00" : txtETBTime.Text.Substring(0, 2) + ":" + txtETATime.Text.Substring(2);
            string etd = (txtETDTime.Text.Replace("AM", "").Replace("PM", "").Trim() == "00:00") ? "00:00" : txtETDTime.Text.Substring(0, 2) + ":" + txtETATime.Text.Substring(2);

            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidOrder(Request.QueryString[0].ToString(), txtSupplierId.Text, txtOrderDate.Text, txtRoundOff.Text, txtDiscount.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["ORDERID"] == null)
                {
                    Guid Orderid = new Guid();
                    Guid dtkey = new Guid();
                    PhoenixVesselAccountsOrderForm.InsertOrderForm(PhoenixSecurityContext.CurrentSecurityContext.VesselID, int.Parse(Request.QueryString["storeclass"].ToString())
                        , DateTime.Parse(txtOrderDate.Text), new Guid(txtSupplierId.Text), General.GetNullableInteger(ddlCurrency.SelectedCurrency)
                        , General.GetNullableDecimal(txtExchangeRate.Text), General.GetNullableDecimal(txtTotalAmount.Text), General.GetNullableInteger(rblPaymentTerm.SelectedValue)
                        , General.GetNullableDecimal(txtDiscount.Text), General.GetNullableDateTime(txtETA.Text + " " + eta), General.GetNullableDateTime(txtETB.Text + (txtETB.Text != null ? " " + etb : ""))
                        , General.GetNullableDateTime(txtETD.Text + (txtETD.Text != null ? " " + etd : "")), General.GetNullableInteger(ucPort.SelectedValue), General.GetNullableDateTime(txtReceivedDate.Text)
                        , General.GetNullableInteger(ddlSeaPort.SelectedValue), General.GetNullableDecimal(txtDeliverycharges.Text), General.GetNullableDecimal(txtChargesDiscount.Text), ref Orderid, ref dtkey
                        , General.GetNullableInteger(txtForwarderId.Text)
                        , General.GetNullableInteger(chkForStock.Checked == true ? "1" : "0"));
                    ViewState["ORDERID"] = Orderid;
                    ViewState["DTKEY"] = dtkey;
                    ucstatus.Text = "Requisition Created.";
                    ucstatus.Visible = true;
                }
                else
                {
                    PhoenixVesselAccountsOrderForm.UpdateOrderForm(new Guid(ViewState["ORDERID"].ToString()), int.Parse(Request.QueryString["storeclass"].ToString())
                        , DateTime.Parse(txtOrderDate.Text), new Guid(txtSupplierId.Text), General.GetNullableInteger(ddlCurrency.SelectedCurrency)
                        , General.GetNullableDecimal(txtExchangeRate.Text), General.GetNullableDecimal(txtTotalAmount.Text), General.GetNullableInteger(rblPaymentTerm.SelectedValue)
                        , General.GetNullableDecimal(txtDiscount.Text), General.GetNullableDateTime(txtETA.Text + " " + eta), General.GetNullableDateTime(txtETB.Text + (txtETB.Text != null ? " " + etb : ""))
                        , General.GetNullableDateTime(txtETD.Text + (txtETD.Text != null ? " " + etd : "")), General.GetNullableInteger(ucPort.SelectedValue), General.GetNullableDateTime(txtReceivedDate.Text)
                        , General.GetNullableInteger(ddlSeaPort.SelectedValue), General.GetNullableDecimal(txtDeliverycharges.Text), General.GetNullableDecimal(txtChargesDiscount.Text)
                        , General.GetNullableInteger(txtForwarderId.Text), General.GetNullableInteger(ddlPaidcurrency.SelectedCurrency)
                        , General.GetNullableInteger(chkForStock.Checked == true ? "1" : "0")
                        );
                    ucstatus.Text = "Requisition Updated.";
                    ucstatus.Visible = true;
                }
                EditBondProvision(new Guid(ViewState["ORDERID"].ToString()));
                chkForStock.Enabled = false;
                MainMenu();
                ResetMenu();
            }
            else if (CommandName.ToUpper().Equals("SAVEREC"))
            {
                if (!IsValidOrder(Request.QueryString[0].ToString(), txtSupplierId.Text, txtOrderDate.Text, txtRoundOff.Text, txtDiscount.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                if (!IsValidReceivedDetails(txtReceivedDate.Text, ddlSeaPort.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsOrderForm.UpdateOrderForm(new Guid(ViewState["ORDERID"].ToString()), int.Parse(Request.QueryString["storeclass"].ToString())
                     , DateTime.Parse(txtOrderDate.Text), new Guid(txtSupplierId.Text), General.GetNullableInteger(ddlCurrency.SelectedCurrency)
                     , General.GetNullableDecimal(txtExchangeRate.Text), General.GetNullableDecimal(txtTotalAmount.Text), General.GetNullableInteger(rblPaymentTerm.SelectedValue)
                     , General.GetNullableDecimal(txtDiscount.Text), General.GetNullableDateTime(txtETA.Text + " " + eta), General.GetNullableDateTime(txtETB.Text + " " + etb)
                     , General.GetNullableDateTime(txtETD.Text + " " + etd), General.GetNullableInteger(ucPort.SelectedValue), General.GetNullableDateTime(txtReceivedDate.Text)
                     , General.GetNullableInteger(ddlSeaPort.SelectedValue), General.GetNullableDecimal(txtDeliverycharges.Text), General.GetNullableDecimal(txtChargesDiscount.Text)
                     , General.GetNullableInteger(txtForwarderId.Text), General.GetNullableInteger(ddlPaidcurrency.SelectedCurrency)
                     , General.GetNullableInteger(chkForStock.Checked == true ? "1" : "0")
                     );
                PhoenixVesselAccountsOrderForm.ConfirmOrderForm(new Guid(ViewState["ORDERID"].ToString()), DateTime.Parse(txtReceivedDate.Text)
                                                                , int.Parse(ddlSeaPort.SelectedValue), General.GetNullableInteger(chkForStock.Checked == true ? "1" : "0"));
                ucstatus.Text = "Requisition Received.";
                ucstatus.Visible = true;
                EditBondProvision(new Guid(ViewState["ORDERID"].ToString()));
                chkForStock.Enabled = false;
                ResetMenu();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EditBondProvision(Guid gOrderId)
    {
        DataTable dt = PhoenixVesselAccountsOrderForm.EditOrderForm(gOrderId, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            //if (Request.QueryString["storeclass"].ToString() == "412")
            //    MenuCrewBond.Title = "Requisition of Bond (" + dr["FLDVESSELNAME"].ToString() + ")";
            //else if (Request.QueryString["storeclass"].ToString() == "411")
            //    MenuCrewBond.Title = "Requisition of Provisions(" + dr["FLDVESSELNAME"].ToString() + ")";
            txtRefNo.Text = dr["FLDREFERENCENO"].ToString();
            txtSupplierCode.Text = dr["FLDCODE"].ToString();
            txtSupplierCode.ToolTip = dr["FLDCODE"].ToString();
            txtSupplierName.Text = dr["FLDNAME"].ToString();
            txtSupplierName.ToolTip = dr["FLDNAME"].ToString();
            txtSupplierId.Text = dr["FLDSUPPLIERCODE"].ToString();
            txtChargesDiscount.Text = dr["FLDDELIVERYDISCOUNT"].ToString();
            txtDeliverycharges.Text = dr["FLDDELIVERYCHARGES"].ToString();
            txtOrderDate.Text = dr["FLDORDERDATE"].ToString();
            ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
            ddlCurrency.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
            txtExchangeRate.Text = dr["FLDEXCHANGERATE"].ToString();
            txtTotalAmount.Text = dr["FLDAMOUNTPAID"].ToString();
            txtGrandTotal.Text = dr["FLDGRANDTOTAL"].ToString();
            txtLineItemAmount.Text = dr["FLDITEMTOTALUSD"].ToString();
            txtdeliverytotal.Text = dr["FLDTOTALDISCOUNT"].ToString();
            //PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 125, "CRP") == dr["FLDPAYMENTTERMID"].ToString() ? dr["FLDTOTALAMOUNT"].ToString()
            txtDiscount.Text = dr["FLDDISCOUNT"].ToString();
            txtRoundOff.Text = dr["FLDROUNDOFFAMOUNT"].ToString();
            ucPort.SelectedValue = dr["FLDEXPECTEDPORTID"].ToString();
            ucPort.Text = dr["FLDPORTNAME"].ToString();
            txtETA.Text = dr["FLDETA"].ToString();
            txtETATime.Text = String.Format("{0:HH:mm}", dr["FLDETA"]);
            txtETB.Text = dr["FLDETB"].ToString();
            txtETBTime.Text = String.Format("{0:HH:mm}", dr["FLDETB"]);
            txtETD.Text = dr["FLDETD"].ToString();
            txtETDTime.Text = String.Format("{0:HH:mm}", dr["FLDETD"]);
            ddlSeaPort.SelectedValue = dr["FLDRECEIVEDPORTID"].ToString();
            ddlSeaPort.Text = dr["FLDRECEIVEDPORTNAME"].ToString();
            txtReceivedDate.Text = dr["FLDRECEIVEDDATE"].ToString();
            ViewState["ACTIVE"] = dr["FLDACTIVEYN"].ToString();
            txtForwarderId.Text = dr["FLDPORTAGENTID"].ToString();
            txtForwarderCode.Text = dr["FLDPORTAGENTCODE"].ToString();
            txtForwarderName.Text = dr["FLDPORTAGENTNAME"].ToString();
            ddlPaidcurrency.SelectedCurrency = dr["FLDPAYMENTCURRENCY"].ToString();

            chkForStock.Checked = dr["FLDISSTOCKYN"].ToString() == "1" ? true : false;
            ViewState["ISSTOCKYN"] = dr["FLDISSTOCKYN"].ToString();
            ViewState["NEWPROCESS"] = dr["FLDNEWPROCESSYN"].ToString();
            ViewState["REQNO"] = dr["FLDREFERENCENO"].ToString();
            if (Request.QueryString["storeclass"].ToString() == "412")
            {
                lblLineItemAmount.Text = "Total Bond Cost (" + dr["FLDRPTCURRENCYCODE"].ToString() + ")(A)";
            }
            else if (Request.QueryString["storeclass"].ToString() == "411")
            {
                lblLineItemAmount.Text = "Total Provision Cost (" + dr["FLDRPTCURRENCYCODE"].ToString() + ")(A)";
            }
            lblExchangeRate1USD.Text = "Exchange Rate (1 " + dr["FLDRPTCURRENCYCODE"].ToString() + " = )";
            lbldeliverytotal.Text = "Charges after Discount (" + dr["FLDRPTCURRENCYCODE"].ToString() + ")(B)";
            lblgrandtotal.Text = "Grand Total (" + dr["FLDRPTCURRENCYCODE"].ToString() + ")(A+B) ";

            cmdForwarderAddress.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Purchase/PurchaseFormAddress.aspx?addresscode=" + txtForwarderId.Text + "');return false;");
            ListItem item = rblPaymentTerm.Items.FindByValue(dr["FLDPAYMENTTERMID"].ToString());
            if (item != null)
            {
                rblPaymentTerm.Items.FindByValue(dr["FLDPAYMENTTERMID"].ToString()).Selected = true;
                PaymentTerm();
            }
            MainMenu();
        }
    }
    protected void rblPaymentTerm_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        PaymentTerm();
    }
    protected void PaymentTerm()
    {
        if (rblPaymentTerm.SelectedValue == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 125, "CRP"))
        {
            ddlPaidcurrency.Enabled = false;
            txtTotalAmount.Enabled = false;
        }
        else
        {
            ddlPaidcurrency.Enabled = true;
            txtTotalAmount.Enabled = true;
        }
    }
    private bool IsValidOrder(string stocktype, string supplier, string orderdate, string roundoff, string discount)
    {
        string eta = (txtETATime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtETATime.Text.Substring(0, 2) + ":" + txtETATime.Text.Substring(2);
        string etb = (txtETBTime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtETBTime.Text.Substring(0, 2) + ":" + txtETATime.Text.Substring(2);
        string etd = (txtETDTime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtETDTime.Text.Substring(0, 2) + ":" + txtETATime.Text.Substring(2);
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate, resultDate1;
        if (!General.GetNullableDateTime(orderdate).HasValue)
        {
            ucError.ErrorMessage = "Order Date is required.";
        }
        else if (DateTime.TryParse(orderdate, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Order Date should be earlier than current date";
        }
        if (!General.GetNullableInteger(stocktype).HasValue)
        {
            ucError.ErrorMessage = "Stock Type is required.";
        }
        if (!General.GetNullableGuid(supplier).HasValue)
        {
            ucError.ErrorMessage = "Ship Chandler Name is required.";
        }
        if (General.GetNullableDecimal(discount).HasValue && General.GetNullableDecimal(discount).Value > 100)
        {
            ucError.ErrorMessage = "Discount should be between 0 and 100";
        }
        if (!General.GetNullableInteger(rblPaymentTerm.SelectedValue).HasValue)
        {
            ucError.ErrorMessage = "Payment Terms is Required.";
        }
        if (!General.GetNullableInteger(ucPort.SelectedValue).HasValue)
        {
            ucError.ErrorMessage = "Delivery Port is Required.";
        }
        if (!General.GetNullableDateTime(txtETA.Text).HasValue)
        {
            ucError.ErrorMessage = "ETA is Required.";
        }
        else if (DateTime.TryParse(orderdate, out resultDate1) && DateTime.Compare(resultDate1, DateTime.Parse(txtETA.Text)) > 0)
        {
            ucError.ErrorMessage = "ETA Should be later than Order Date.";
        }
        if (General.GetNullableDateTime(txtETB.Text + " " + etb).HasValue && General.GetNullableDateTime(txtETA.Text + " " + eta).HasValue
            && DateTime.TryParse(txtETB.Text + (txtETB.Text != null ? " " + etb : ""), out resultDate) && DateTime.Compare(General.GetNullableDateTime(txtETA.Text + (txtETA.Text != null ? " " + eta : "")).Value, resultDate) > 0)
        {
            ucError.ErrorMessage = "ETB should be later than ETA.";
        }
        if (General.GetNullableDateTime(txtETD.Text + " " + etd).HasValue && General.GetNullableDateTime(txtETB.Text + " " + etb).HasValue
            && DateTime.TryParse(txtETD.Text + (txtETD.Text != null ? " " + etd : ""), out resultDate) && DateTime.Compare(General.GetNullableDateTime(txtETB.Text + (txtETB.Text != null ? " " + etb : "")).Value, resultDate) > 0)
        {
            ucError.ErrorMessage = "ETD should be later than ETB.";
        }

        return (!ucError.IsError);
    }

    private void MainMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (Request.QueryString["r"] == null)
        {
            divMain.Visible = true;
            if (ViewState["ACTIVE"] == null || ViewState["ACTIVE"].ToString() == "1")
            {

                if (ViewState["ORDERID"] != null && ViewState["ORDERID"].ToString() != "")
                {
                    toolbar.AddImageLink("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKEY"] + "&mod="
                     + PhoenixModule.VESSELACCOUNTS + "'); return false;", "Attachment", "", "ATTACHMENT", ToolBarDirection.Right);

                    toolbar.AddButton("Receive", "SAVEREC", ToolBarDirection.Right);
                    toolbar.AddButton("Save", "UPDATE", ToolBarDirection.Right);


                }
                else
                {
                    toolbar.AddButton("Save", "UPDATE", ToolBarDirection.Right);
                }
                MenuCrewBond.AccessRights = this.ViewState;
                MenuCrewBond.MenuList = toolbar.Show();
                if (ViewState["ORDERID"] != null && ViewState["ORDERID"].ToString() != "") divSub.Visible = true;
                else divSub.Visible = false;
            }
        }
    }
    private bool IsValidReceivedDetails(string date, string Seaport)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Received Date is required.";
        }
        else if (DateTime.Parse(date) > DateTime.Today)
        {
            ucError.ErrorMessage = "Received Date should not be greater than Current Date";
        }
        if (!General.GetNullableInteger(Seaport).HasValue)
        {
            ucError.ErrorMessage = "Received Port is required.";
        }
        return (!ucError.IsError);
    }

    protected void cmdClearAddress_Click(object sender, ImageClickEventArgs e)
    {
        txtForwarderId.Text = "";
        txtForwarderName.Text = "";
        txtForwarderCode.Text = "";
    }
}
