using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Common;

public partial class VesselAccountsOrderFormBondGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                if (Request.QueryString["storeclass"].ToString() == "412")
                    Title1.Text = "Requisition of Bond";
                else if (Request.QueryString["storeclass"].ToString() == "411")
                    Title1.Text = "Requisition of Provisions";
                lblhead.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;

                txtSupplierId.Attributes.Add("style", "display:none");
                ViewState["ORDERID"] = Request.QueryString["ORDERID"] == null ? null : Request.QueryString["ORDERID"];
                ViewState["DTKEY"] = Request.QueryString["DTKEY"] == null ? null : Request.QueryString["DTKEY"];
                rblPaymentTerm.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 125);
                rblPaymentTerm.DataBind();
                ViewState["ACTIVE"] = "1";
                if (ViewState["ORDERID"] != null)
                    EditBondProvision(new Guid(ViewState["ORDERID"].ToString()));

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
    private void ResetMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("General", "GENERAL");
        if (ViewState["ORDERID"] != null)
            toolbar.AddButton("Line Item", "LINEITEM");
        toolbar.AddButton("List", "LIST");
        MenuOrderForm.AccessRights = this.ViewState;
        MenuOrderForm.MenuList = toolbar.Show();
        MenuOrderForm.SelectedMenuIndex = 0;
    }
    protected void OrderForm_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("GENERAL"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsOrderFormBondGeneral.aspx?storeclass=" + Request.QueryString["storeclass"].ToString() + "&orderid=" + ViewState["ORDERID"] + "&pageno=" + ViewState["PAGENUMBER"], false);
            }
            else if (dce.CommandName.ToUpper().Equals("LINEITEM"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsOrderFormBondLineItem.aspx?storeclass=" + Request.QueryString["storeclass"].ToString() + "&orderid=" + ViewState["ORDERID"] + "&pageno=" + ViewState["PAGENUMBER"], false);
            }
            else if (dce.CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsOrderFormBond.aspx?storeclass=" + Request.QueryString["storeclass"].ToString(), false);
            }
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
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                string eta = (txtETATime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtETATime.Text;
                string etb = (txtETBTime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtETBTime.Text;
                string etd = (txtETDTime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtETDTime.Text;
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
                        , General.GetNullableDecimal(txtDiscount.Text), General.GetNullableDateTime(txtETA.Text + " " + eta), General.GetNullableDateTime(txtETB.Text + " " + etb)
                        , General.GetNullableDateTime(txtETD.Text + " " + etd), General.GetNullableInteger(ucPort.SelectedValue), General.GetNullableDateTime(txtReceivedDate.Text)
                        , General.GetNullableInteger(ddlSeaPort.SelectedValue), ref Orderid, ref dtkey);
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
                        , General.GetNullableDecimal(txtDiscount.Text), General.GetNullableDateTime(txtETA.Text + " " + eta), General.GetNullableDateTime(txtETB.Text + " " + etb)
                        , General.GetNullableDateTime(txtETD.Text + " " + etd), General.GetNullableInteger(ucPort.SelectedValue), General.GetNullableDateTime(txtReceivedDate.Text)
                        , General.GetNullableInteger(ddlSeaPort.SelectedValue)
                        );
                    ucstatus.Text = "Requisition Updated.";
                    ucstatus.Visible = true;
                }
                EditBondProvision(new Guid(ViewState["ORDERID"].ToString()));
                MainMenu();
                ResetMenu();
            }
            else if (dce.CommandName.ToUpper().Equals("SAVEREC"))
            {
                if (!IsValidReceivedDetails(txtReceivedDate.Text, ddlSeaPort.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsOrderForm.ConfirmOrderForm(new Guid(ViewState["ORDERID"].ToString()), DateTime.Parse(txtReceivedDate.Text), int.Parse(ddlSeaPort.SelectedValue));
                ucstatus.Text = "Requisition Received.";
                ucstatus.Visible = true;
                EditBondProvision(new Guid(ViewState["ORDERID"].ToString()));
                ResetMenu();

                // Response.Redirect("../VesselAccounts/VesselAccountsOrderFormBondGeneral.aspx?storeclass=" + Request.QueryString["storeclass"].ToString() + "&orderid=" + ViewState["ORDERID"] + "&pageno=" + ViewState["PAGENUMBER"], false);
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
            lblhead.Text = dr["FLDVESSELNAME"].ToString();
            txtRefNo.Text = dr["FLDREFERENCENO"].ToString();
            txtSupplierCode.Text = dr["FLDCODE"].ToString();
            txtSupplierName.Text = dr["FLDNAME"].ToString();
            txtSupplierId.Text = dr["FLDSUPPLIERCODE"].ToString();
            txtOrderDate.Text = dr["FLDORDERDATE"].ToString();
            //ddlStock.SelectedHard = dr["FLDSTOCKTYPE"].ToString();
            //ddlStock.Enabled = false;
            ddlCurrency.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
            txtExchangeRate.Text = dr["FLDEXCHANGERATE"].ToString();
            txtTotalAmount.Text = dr["FLDAMOUNTPAID"].ToString();
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
            ListItem item = rblPaymentTerm.Items.FindByValue(dr["FLDPAYMENTTERMID"].ToString());
            if (item != null)
                rblPaymentTerm.Items.FindByValue(dr["FLDPAYMENTTERMID"].ToString()).Selected = true;
            MainMenu();
        }
    }
    private bool IsValidOrder(string stocktype, string supplier, string orderdate, string roundoff, string discount)
    {
        string eta = (txtETATime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtETATime.Text;
        string etb = (txtETBTime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtETBTime.Text;
        string etd = (txtETDTime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtETDTime.Text;
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
        //if (General.GetNullableDecimal(roundoff).HasValue && General.GetNullableDecimal(roundoff).Value > 10)
        //{
        //    ucError.ErrorMessage = "Round off Amount should be between 0$ and 10$";
        //}
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
            && DateTime.TryParse(txtETB.Text + " " + etb, out resultDate) && DateTime.Compare(General.GetNullableDateTime(txtETA.Text + " " + eta).Value, resultDate) > 0)
        {
            ucError.ErrorMessage = "ETB should be later than ETA.";
        }
        if (General.GetNullableDateTime(txtETD.Text + " " + etd).HasValue && General.GetNullableDateTime(txtETB.Text + " " + etb).HasValue
            && DateTime.TryParse(txtETD.Text + " " + etd, out resultDate) && DateTime.Compare(General.GetNullableDateTime(txtETB.Text + " " + etb).Value, resultDate) > 0)
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
                toolbar.AddButton("Save", "SAVE");
                if (ViewState["ORDERID"] != null && ViewState["ORDERID"].ToString() != "")
                {
                    divSub.Visible = true;
                    toolbar.AddButton("Receive", "SAVEREC");
                    toolbar.AddImageLink("javascript:parent.Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKEY"] + "&mod="
                        + PhoenixModule.VESSELACCOUNTS + "'); return false;", "Attachment", "", "ATTACHMENT");
                }
                else
                    divSub.Visible = false;
                MenuCrewBond.AccessRights = this.ViewState;
                MenuCrewBond.MenuList = toolbar.Show();
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
}
