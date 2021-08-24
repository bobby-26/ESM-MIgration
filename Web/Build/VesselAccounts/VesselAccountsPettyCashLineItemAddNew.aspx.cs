using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;

public partial class VesselAccountsPettyCashLineItemAddNew : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("List", "LIST");
            MenuPettyCash.AccessRights = this.ViewState;
            MenuPettyCash.MenuList = toolbarmain.Show();
            toolbarmain = new PhoenixToolbar();
            toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE");
            MenuPettyCash1.AccessRights = this.ViewState;
            MenuPettyCash1.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {

                ViewState["ID"] = Request.QueryString["id"];
                if (ViewState["ID"] != null && ViewState["ID"].ToString() != "")
                    EditPettyCash(new Guid(ViewState["ID"].ToString()));

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
        DataTable dt = PhoenixVesselAccountsCTMNew.EditCaptainPettyCash(pettycaseid, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        if (dt.Rows.Count > 0)
        {
            txtDateAdd.Text = dt.Rows[0]["FLDDATE"].ToString();
            txtPurposeAdd.Text = dt.Rows[0]["FLDPURPOSE"].ToString();
            txtAmountAdd.Text = dt.Rows[0]["FLDAMOUNT"].ToString();
            ddlSeaPortAdd.Text = dt.Rows[0]["FLDSEAPORTNAME"].ToString();
            ddlSeaPortAdd.SelectedValue = dt.Rows[0]["FLDSEAPORTID"].ToString();
            if (dt.Rows[0]["FLDPAYMENTRECEIPT"].ToString() == "0")
                rbPayment.Checked = true;
            else
                rbReceipts.Checked = true;
        }
    }
    protected void MenuPettyCash_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsPettyCashLineItemNew.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuPettyCash1_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                string seaport = ddlSeaPortAdd.SelectedValue;
                string date = txtDateAdd.Text;
                string purpose = txtPurposeAdd.Text;
                string amount = txtAmountAdd.Text;
                int paymentreceipt = rbPayment.Checked ? 0 : 1;
                if (!IsValidPettyCash(seaport, date, purpose, amount))
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["ID"] == null)
                {
                    PhoenixVesselAccountsCTMNew.InsertCaptainPettyCash(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Parse(date)
                        , int.Parse(seaport), purpose, decimal.Parse(amount), paymentreceipt, null, null);
                }
                else
                {
                    PhoenixVesselAccountsCTMNew.UpdateCaptainPettyCash(new Guid(ViewState["ID"].ToString()), DateTime.Parse(date), int.Parse(seaport), purpose, decimal.Parse(amount), paymentreceipt, null, null);
                }
                Response.Redirect("../VesselAccounts/VesselAccountsPettyCashLineItemNew.aspx", false);
            }
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