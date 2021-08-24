using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;

public partial class PurchaseDeliveryTemp : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                toolbar.AddButton("Save", "SAVE");
                MenuDelivery.AccessRights = this.ViewState;
                MenuDelivery.MenuList = toolbar.Show();
            }

            if (!IsPostBack)
            {
                if (Request.QueryString["deliveryid"] != null)
                {
                    ViewState["deliveryid"] = Request.QueryString["deliveryid"].ToString();
                    BindFields(ViewState["deliveryid"].ToString());
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindFields(string deliveryid)
    {
        DataSet ds = PhoenixPurchaseDelivery.EditdeliveryTemp(new Guid(deliveryid));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtFormNo.Text = dr["FLDFORMNO"].ToString();
            txtForwarder.Text = dr["FLDFORWARDER"].ToString();
            txtIsDelivered.Text = dr["FLDDELIVERED"].ToString().Equals("1") ? "Y" : "N";
            txtSupplier.Text = dr["FLDVENDOR"].ToString();
            txtDGR.Text = dr["FLDISDGR"].ToString().Equals("1") ? "Y" : "N";
            txtCurrency.Text = dr["FLDCURRENCY"].ToString();
            txtShortNote.Text = dr["FLDSHORTNOTE"].ToString();
            txtMawb.Text = dr["FLDMAWB"].ToString();
            txtHawb.Text = dr["FLDHAWB"].ToString();
            txtNoOfPackages.Text = dr["FLDNUMBEROFPACKAGES"].ToString();
            ucTotalWeight.Text = String.Format("{0:#,###,##0.00}", dr["FLDTOTALWEIGHT"]);
            txtAmount.Text = String.Format("{0:#,##0}", dr["FLDVALUE"]);
            txtLocation.Text = dr["FLDLOCATION"].ToString();
            txtOrigin.Text = dr["FLDORIGIN"].ToString();
            txtShipmentMode.Text = dr["FLDSHIPMENTMODE"].ToString();
            txtShortNote.Text = dr["FLDSHORTNOTE"].ToString();
            txtReceivedForwarder.Text = General.GetDateTimeToString(dr["FLDFORWARDERRECEIVEDDATE"].ToString());
            txtVesselName.Text = dr["FLDVESSELNAME"].ToString();
            txtError.Text = dr["FLDERROR"].ToString();
            lblDeliveryTempId.Text = dr["FLDDELIVERYTEMPID"].ToString();
        }
    }

    protected void MenuDelivery_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidDelivery())
                {
                    ucError.Visible = true;
                    return;
                }
                
                UpdateDelivery();

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

    private void UpdateDelivery()
    {
        PhoenixPurchaseDelivery.UpdateDeliveryExcel(
            new Guid(lblDeliveryTempId.Text),
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            txtVesselName.Text, txtFormNo.Text,
            General.GetNullableInteger(txtNoOfPackages.Text),
            General.GetNullableDecimal(ucTotalWeight.Text),
            txtForwarder.Text,
            General.GetNullableDateTime(txtReceivedForwarder.Text),
            txtShipmentMode.Text,
            txtSupplier.Text,
            txtCurrency.Text,
            General.GetNullableDecimal(txtAmount.Text),
            txtDGR.Text.ToUpper().Equals("Y") ? 1 : 0,
            txtShortNote.Text, 
            txtMawb.Text, 
            txtHawb.Text, 
            txtOrigin.Text, 
            txtLocation.Text,
            txtIsDelivered.Text.ToUpper().Equals("Y") ? 1 : 0);
    }

    private bool IsValidDelivery()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtForwarder.Text.Equals(""))
            ucError.ErrorMessage = "Forwarder is mandatory.";

        if (txtVesselName.Text.Equals(""))
            ucError.ErrorMessage = "Vessel Name is mandatory";

        if (txtFormNo.Text.Equals(""))
            ucError.ErrorMessage = "Form No. is mandatory";

        return (!ucError.IsError);
    }
}
