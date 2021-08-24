using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;
using SouthNests.Phoenix.Common;

public partial class VesselAccountsCTMGeneralNew : PhoenixBasePage
{
    string VesselCash, Cashadvance, FoodAllowance;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // toolbarmain = new PhoenixToolbar();
            //mainmenu("");
            VesselCash = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 268, "VSL");
            FoodAllowance = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 268, "FAL");
            Cashadvance = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 268, "CAA");

            if (!IsPostBack)
            {
                ViewState["ACTIVEYN"] = null;
                ViewState["CTMID"] = Request.QueryString["CTMID"];
                ViewState["TYPE"] = "";
                txtDate.Text = DateTime.Now.ToString();

                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                {
                    ucPurposeHard.SelectedHard = VesselCash;
                    ucPurposeHard.Enabled = false;
                }
                ucVesselSupplier.vessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                ddlCurrency.VesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                if (ViewState["CTMID"] != null && ViewState["CTMID"].ToString() != string.Empty)
                    EditCTM(new Guid(ViewState["CTMID"].ToString()));
                else
                {
                    DataSet ds = PhoenixRegistersVessel.EditVessel(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                    if (ds.Tables[0].Rows.Count > 0)
                        lblhead.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
                    int vesselcurrency = 0;
                    decimal Exchagerate = 0;
                    DataSet ds1 = PhoenixVesselAccountsCurrencyConfiguration.ListGetExchangeRate(PhoenixSecurityContext.CurrentSecurityContext.VesselID, null
                                                                                , DateTime.Parse(DateTime.Now.ToString()), ref Exchagerate, ref vesselcurrency, General.GetNullableInteger("1"));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        txtExchangeRate.Text = ds1.Tables[0].Rows[0]["FLDEXCHANGERATE"].ToString();
                        ddlCurrency.SelectedCurrency = ds1.Tables[0].Rows[0]["FLDVESSELCURRENCYID"].ToString();
                    }
                }
                lblArrangedAmount.Visible = false;
                txtAmountArranged.Visible = false;
                lblOfficeRemarks.Visible = false;
                txtRemarks.Visible = false;
            }
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            if (ViewState["TYPE"].ToString() == VesselCash)
            {
                toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("General", "GENERAL");
                toolbarmain.AddButton("BOW", "BOW");
                toolbarmain.AddButton("CTM", "CTMCAL");
                toolbarmain.AddButton("Denomination", "DENOMINATOIN");
                toolbarmain.AddButton("List", "LIST");
            }
            else if (ViewState["TYPE"].ToString() == FoodAllowance || ViewState["TYPE"].ToString() == Cashadvance)
            {
                toolbarmain = new PhoenixToolbar();

                toolbarmain.AddButton("General", "GENERAL");
                toolbarmain.AddButton("Crew List", "CREW");
                toolbarmain.AddButton("List", "LIST");

            }
            else
            {
                toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("General", "GENERAL");
                toolbarmain.AddButton("List", "LIST");

            }
            MenuCTMMain.AccessRights = this.ViewState;
            MenuCTMMain.MenuList = toolbarmain.Show();
            MenuCTMMain.SelectedMenuIndex = 0;
            MENU();
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
            if (General.GetNullableDateTime(txtDate.Text) == null)
            {
                ucError.Text = "Expenses On is Required.";
                ucError.Visible = true;
                return;
            }
            DataSet ds = PhoenixVesselAccountsCurrencyConfiguration.ListGetExchangeRate(PhoenixSecurityContext.CurrentSecurityContext.VesselID, int.Parse(ddlCurrency.SelectedCurrency)
                                                                        , DateTime.Parse(txtDate.Text), ref Exchagerate, ref vesselcurrency, General.GetNullableInteger("1"));

            if (ds.Tables[0].Rows.Count > 0)
                txtExchangeRate.Text = ds.Tables[0].Rows[0]["FLDEXCHANGERATE"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCTMMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (ViewState["CTMID"] == null || ViewState["CTMID"].ToString() == "")
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMGeneral.aspx";
            }
            else if (CommandName.ToUpper().Equals("GENERAL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMGeneral.aspx";
            }
            else if (CommandName.ToUpper().Equals("BOW"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMBOW.aspx";
            }
            else if (CommandName.ToUpper().Equals("DENOMINATOIN"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMDenomination.aspx";
            }
            else if (CommandName.ToUpper().Equals("CTMCAL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMCalculation.aspx";
            }
            else if (CommandName.ToUpper().Equals("CREW"))
            {
                if (ucPurposeHard.SelectedHard == Cashadvance)
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMCashAdvance.aspx";
                else
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMFoodAllowance.aspx";
            }
            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsCTM.aspx", false);
            }
            else
                Response.Redirect(ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + "?CTMID=" + ViewState["CTMID"] + "&a=" + ViewState["ACTIVEYN"], false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCTM_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE") || CommandName.ToUpper().Equals("SENDTOOFFICE"))
            {
                string date = txtDate.Text;
                string port = ddlPort.SelectedValue;
                string eta = txtETA.Text;
                string etd = txtETD.Text;
                string supplier = ucVesselSupplier.SelectedValue;
                string amount = txtAmount.Text;
                if (!IsValidCTM(date, port, eta, etd, supplier))
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["CTMID"] == null || ViewState["CTMID"].ToString() == string.Empty)
                {
                    Guid CTMID = new Guid();
                    PhoenixVesselAccountsCTM.InsertCTMRequest(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , DateTime.Parse(date), int.Parse(port), DateTime.Parse(txtETA.Text), DateTime.Parse(txtETD.Text)
                        , new Guid(supplier), decimal.Parse("0"), General.GetNullableInteger(ddlCurrency.SelectedCurrency), General.GetNullableDecimal(txtExchangeRate.Text), txtRemarks.Text, ref CTMID, int.Parse(ucPurposeHard.SelectedHard));
                    ViewState["CTMID"] = CTMID.ToString();
                    ucStatus.Text = "Request Created.";
                }
                else
                {
                    PhoenixVesselAccountsCTM.UpdateCTMRequest(new Guid(ViewState["CTMID"].ToString())
                       , DateTime.Parse(date), int.Parse(port), DateTime.Parse(txtETA.Text), DateTime.Parse(txtETD.Text)
                       , new Guid(supplier), decimal.Parse(amount), General.GetNullableInteger(ddlCurrency.SelectedCurrency), General.GetNullableDecimal(txtExchangeRate.Text)
                       , byte.Parse(CommandName.ToUpper().Equals("SAVE") ? "0" : "1"), General.GetNullableDecimal(txtAmountArranged.Text)
                       , txtRemarks.Text, General.GetNullableDateTime(txtReceivedDate.Text), General.GetNullableDecimal(txtReceivedAmount.Text));
                    ucStatus.Text = "Request Updated.";
                }
                EditCTM(new Guid(ViewState["CTMID"].ToString()));
            }
            else if (CommandName.ToUpper().Equals("SAVEREC"))
            {
                if (!IsValidCTMReceive(txtReceivedDate.Text, txtReceivedAmount.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsCTM.ReceiveCTMRequest(new Guid(ViewState["CTMID"].ToString()), DateTime.Parse(txtReceivedDate.Text), decimal.Parse(txtReceivedAmount.Text), null);
                ucStatus.Text = "Status Updated.";
                EditCTM(new Guid(ViewState["CTMID"].ToString()));
            }
            MENU();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void mainmenu(string test)
    {
        //PhoenixToolbar toolbarmain = new PhoenixToolbar();

        //if (ViewState["TYPE"] == VesselCash)
        //{
        //    toolbarmain = new PhoenixToolbar();
        //    toolbarmain.AddButton("General", "GENERAL");
        //    toolbarmain.AddButton("BOW", "BOW");
        //    toolbarmain.AddButton("CTM", "CTMCAL");
        //    toolbarmain.AddButton("Denomination", "DENOMINATOIN");
        //    toolbarmain.AddButton("List", "LIST");
        //    MenuCTMMain.AccessRights = this.ViewState;
        //    MenuCTMMain.MenuList = toolbarmain.Show();
        //    MenuCTMMain.SelectedMenuIndex = 0;
        //}
        //else if (ViewState["TYPE"] == FoodAllowance || ViewState["TYPE"] == Cashadvance)
        //{
        //    toolbarmain = new PhoenixToolbar();

        //    toolbarmain.AddButton("General", "GENERAL");
        //    toolbarmain.AddButton("Crew List", "CREW");
        //    toolbarmain.AddButton("List", "LIST");
        //    MenuCTMMain.AccessRights = this.ViewState;
        //    MenuCTMMain.MenuList = toolbarmain.Show();
        //    MenuCTMMain.SelectedMenuIndex = 0;
        //}
        //else
        //{
        //    toolbarmain = new PhoenixToolbar();
        //    toolbarmain.AddButton("General", "GENERAL");
        //    toolbarmain.AddButton("List", "LIST");
        //    MenuCTMMain.AccessRights = this.ViewState;
        //    MenuCTMMain.MenuList = toolbarmain.Show();
        //    MenuCTMMain.SelectedMenuIndex = 0;
        //}


    }
    private void MENU()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (ViewState["ACTIVEYN"] == null || ViewState["ACTIVEYN"].ToString() == "1")
        {

            if (ViewState["CTMID"] != null && ViewState["CTMID"].ToString() != "")
            {
                divSub.Visible = true;
                toolbar.AddButton("Received", "SAVEREC", ToolBarDirection.Right);
            }
            else
            {
                divSub.Visible = false;
            }
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                toolbar.AddButton("Send to Office", "SENDTOOFFICE", ToolBarDirection.Right);
            }
            MenuCTM.AccessRights = this.ViewState;
            MenuCTM.MenuList = toolbar.Show();
        }
        else
        {
            MenuCTM.Visible = false;
        }
    }
    private void EditCTM(Guid gCTMId)
    {
        DataTable dt = PhoenixVesselAccountsCTM.EditCTMRequest(gCTMId);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            lblhead.Text = dr["FLDVESSELNAME"].ToString();
            txtDate.Text = dr["FLDDATE"].ToString();
            ucPurposeHard.SelectedHard = dr["FLDCTMPURPOSE"].ToString();
            ddlPort.SelectedValue = dr["FLDSEAPORTID"].ToString();
            ddlPort.Text = dr["FLDPORTNAME"].ToString();
            txtETA.Text = dr["FLDETA"].ToString();
            txtETD.Text = dr["FLDETD"].ToString();
            //txtSupplierCode.Text = dr["FLDCODE"].ToString();
            ucVesselSupplier.Text = dr["FLDNAME"].ToString();
            ucVesselSupplier.SelectedValue = dr["FLDSUPPLIERCODE"].ToString();
            ddlCurrency.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
            txtAmount.Text = dr["FLDAMOUNT"].ToString();
            txtAmount.ReadOnly = true;
            txtAmountArranged.Text = dr["FLDAMOUNTARRANGED"].ToString();
            txtAmountArranged.ReadOnly = false;
            txtAmountArranged.CssClass = "input";
            txtExchangeRate.Text = dr["FLDEXCHANGERATE"].ToString();
            txtRemarks.Text = dr["FLDOFFICEREMARKS"].ToString();
            txtRemarks.ReadOnly = false;
            txtRemarks.CssClass = "input";
            txtReceivedAmount.Text = dr["FLDRECEIVEDAMOUNT"].ToString();
            txtReceivedDate.Text = dr["FLDRECEIVEDDATE"].ToString();
            ViewState["ACTIVEYN"] = dr["FLDACTIVEYN"].ToString();
            ViewState["TYPE"] = dr["FLDCTMPURPOSE"].ToString();
        }

    }
    private bool IsValidCTM(string date, string port, string eta, string etd, string portagent)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableInteger(ucPurposeHard.SelectedHard).HasValue)
        {
            ucError.ErrorMessage = "CTM Purpose is required.";
        }
        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Date is required.";
        }
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Date should be earlier than current date";
        }
        if (!General.GetNullableDateTime(eta).HasValue)
        {
            ucError.ErrorMessage = "ETA is required.";
        }
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Parse(eta)) > 0)
        {
            ucError.ErrorMessage = "ETA Should be later than CTM Date.";
        }
        if (!General.GetNullableDateTime(etd).HasValue)
        {
            ucError.ErrorMessage = "ETD is required.";
        }
        else if (DateTime.TryParse(eta, out resultDate) && DateTime.Compare(resultDate, DateTime.Parse(etd)) > 0)
        {
            ucError.ErrorMessage = "ETD Should be later than ETA";
        }
        if (!General.GetNullableInteger(port).HasValue)
        {
            ucError.ErrorMessage = "Port is required.";
        }
        if (!General.GetNullableGuid(portagent).HasValue)
        {
            ucError.ErrorMessage = "Port Agent is required.";
        }
        if (!General.GetNullableInteger(ddlCurrency.SelectedCurrency).HasValue)
        {
            ucError.ErrorMessage = "Requested Currency is required.";
        }
        return (!ucError.IsError);
    }
    private bool IsValidCTMReceive(string date, string amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableDecimal(amount).HasValue)
        {
            ucError.ErrorMessage = "Received Amount is required.";
        }
        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Received Date is required.";
        }
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Received Date should be earlier than current date";
        }
        return (!ucError.IsError);
    }

    protected void ucPurposeHard_TextChangedEvent(object sender, EventArgs e)
    {

    }
}