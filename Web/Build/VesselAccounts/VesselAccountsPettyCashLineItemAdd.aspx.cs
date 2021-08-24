using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class VesselAccountsPettyCashLineItemAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            
            if (!IsPostBack)
            {
                ViewState["ID"] = Request.QueryString["id"];
                ddlCurrency.VesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                {
                    txtExchangeRate.CssClass = "readonlytextbox txtNumber";
                    txtExchangeRate.Enabled = false;
                    txtExchangeRate.ReadOnly = true;
                }

                if (ViewState["ID"] != null && ViewState["ID"].ToString() != "")
                    EditPettyCash(new Guid(ViewState["ID"].ToString()));
                else
                {
                    int vesselcurrency = 0;
                    decimal Exchagerate = 0;

                    DataSet ds = PhoenixVesselAccountsCurrencyConfiguration.ListGetExchangeRate(PhoenixSecurityContext.CurrentSecurityContext.VesselID, null
                                                                                , DateTime.Parse(DateTime.Now.ToString()), ref Exchagerate, ref vesselcurrency, General.GetNullableInteger("1"));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtExchangeRate.Text = ds.Tables[0].Rows[0]["FLDEXCHANGERATE"].ToString();
                        ddlCurrency.SelectedCurrency = ds.Tables[0].Rows[0]["FLDVESSELCURRENCYID"].ToString();
                    }
                }
            }
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("List", "LIST", ToolBarDirection.Right);
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuPettyCash1.AccessRights = this.ViewState;
            MenuPettyCash1.MenuList = toolbarmain.Show();


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int vesselcurrency = 0;
            decimal Exchagerate = 0;
            if (General.GetNullableDateTime(txtDateAdd.Text) == null)
            {
                ucError.Text = "Expenses On is Required.";
                ucError.Visible = true;
                return;
            }
            DataSet ds = PhoenixVesselAccountsCurrencyConfiguration.ListGetExchangeRate(PhoenixSecurityContext.CurrentSecurityContext.VesselID, int.Parse(ddlCurrency.SelectedCurrency)
                                                                        , DateTime.Parse(txtDateAdd.Text), ref Exchagerate, ref vesselcurrency, General.GetNullableInteger("1"));

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtExchangeRate.Text = ds.Tables[0].Rows[0]["FLDEXCHANGERATE"].ToString();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EditPettyCash(Guid pettycaseid)
    {
        DataTable dt = PhoenixVesselAccountsCTM.EditCaptainPettyCash(pettycaseid, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        if (dt.Rows.Count > 0)
        {
            txtDateAdd.Text = dt.Rows[0]["FLDDATE"].ToString();
            txtPurposeAdd.Text = dt.Rows[0]["FLDPURPOSE"].ToString();
            txtAmountAdd.Text = dt.Rows[0]["FLDBASEAMOUNT"].ToString();
            ddlSeaPortAdd.Text = dt.Rows[0]["FLDSEAPORTNAME"].ToString();
            ddlSeaPortAdd.SelectedValue = dt.Rows[0]["FLDSEAPORTID"].ToString();
            ddlCurrency.SelectedCurrency = dt.Rows[0]["FLDBASECURRENCY"].ToString();
            txtExchangeRate.Text = dt.Rows[0]["FLDEXCHANGERATE"].ToString();
            txtvesselcurrency.Text = dt.Rows[0]["FLDVESSELCURRENCYCODE"].ToString();
            txtVCAmount.Text = dt.Rows[0]["FLDAMOUNT"].ToString();
            rbPayment.SelectedValue = dt.Rows[0]["FLDPAYMENTRECEIPT"].ToString();
        }
    }
  

    protected void MenuPettyCash1_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string seaport = ddlSeaPortAdd.SelectedValue;
                string date = txtDateAdd.Text;
                string purpose = txtPurposeAdd.Text;
                string amount = txtAmountAdd.Text;

                int paymentreceipt = int.Parse(rbPayment.SelectedValue);
                if (!IsValidPettyCash(seaport, date, purpose, amount))
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["ID"] == null)
                {
                    PhoenixVesselAccountsCTM.InsertCaptainPettyCash(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Parse(date)
                        , int.Parse(seaport), purpose, General.GetNullableDecimal(txtVCAmount.Text), paymentreceipt, null, null, int.Parse(ddlCurrency.SelectedCurrency), decimal.Parse(amount));
                }
                else
                {
                    PhoenixVesselAccountsCTM.UpdateCaptainPettyCash(new Guid(ViewState["ID"].ToString()), DateTime.Parse(date), int.Parse(seaport), purpose, General.GetNullableDecimal(txtVCAmount.Text), paymentreceipt, null, null
                        , int.Parse(ddlCurrency.SelectedCurrency), decimal.Parse(amount));
                }
                Response.Redirect("../VesselAccounts/VesselAccountsPettyCashLineItem.aspx", false);
            }
            if (CommandName.ToUpper().Equals("LIST"))
                Response.Redirect("../VesselAccounts/VesselAccountsPettyCashLineItem.aspx", false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidPettyCash(string seaport, string date, string purpose, string amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableInteger(seaport).HasValue)
        {
            ucError.ErrorMessage = "Port is required.";
        }

        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Expenses On is required.";
        }
        if (!General.GetNullableInteger(ddlCurrency.SelectedCurrency).HasValue)
        {
            ucError.ErrorMessage = "Currency is required.";
        }
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Expenses On should be earlier than current date";
        }
        if (string.IsNullOrEmpty(purpose))
            ucError.ErrorMessage = "Purpose is required.";

        if (!General.GetNullableDecimal(amount).HasValue)
        {
            ucError.ErrorMessage = "Amount is required.";
        }

        return (!ucError.IsError);
    }

}