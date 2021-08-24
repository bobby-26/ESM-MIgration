using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;
using SouthNests.Phoenix.Accounts;
public partial class VesselAccountsCashRequestSupplier : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuCTM.AccessRights = this.ViewState;
            MenuCTM.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["REQUESTID"] = Request.QueryString["REQUESTID"];
                if (ViewState["REQUESTID"] != null)
                    EditCashAdvanceRequest(new Guid(ViewState["REQUESTID"].ToString()));
                //   txtSupplierId.Attributes.Add("style", "display:none");
                ucVesselSupplier.vessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EditCashAdvanceRequest(Guid RequestId)
    {
        DataTable dt =PhoenixVesselAccountsCashAdvanceRequest.EditCashAdvanceRequest(RequestId);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ////txtSupplierCode.Text = dr["FLDSUPPLIERSHORTNAME"].ToString();
            ////txtSupplierCode.ToolTip = dr["FLDSUPPLIERSHORTNAME"].ToString();
            ////txtSupplierName.Text = dr["FLDSUPPLIERNAME"].ToString();
            ucVesselSupplier.Text = dr["FLDSUPPLIERNAME"].ToString();
           ucVesselSupplier.SelectedValue= dr["FLDSUPPLIERCODE"].ToString();
            txtDate.Text = dr["FLDDATE"].ToString();
            ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
            ucPort.SelectedValue = dr["FLDSEAPORTID"].ToString();
            ucPort.Text = dr["FLDSEAPORTNAME"].ToString();
            txtETA.Text = dr["FLDETA"].ToString();
            txtETD.Text = dr["FLDETD"].ToString();
        }
    }
    protected void MenuCTM_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string port = ucPort.SelectedValue;
                string eta = txtETA.Text;
                string etd = txtETD.Text;
                string supplier = ucVesselSupplier.SelectedValue;
                if (!IsValidCashresquest(port, eta, etd, supplier))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsCashAdvanceRequest.UpdateCashAdvanceRequestsUPPLIER(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                     , General.GetNullableInteger(ucPort.SelectedValue), General.GetNullableGuid(ucVesselSupplier.SelectedValue)
                      , General.GetNullableDateTime(txtETA.Text), General.GetNullableDateTime(txtETD.Text), new Guid(ViewState["REQUESTID"].ToString()));
                ucStatus.Text = "succesfully saved.";

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }        
    }
    private bool IsValidCashresquest(string port, string eta, string etd, string portagent)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
     
        if (!General.GetNullableDateTime(eta).HasValue)
        {
            ucError.ErrorMessage = "ETA is required.";
        }
        else if (DateTime.TryParse(txtDate.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Parse(eta)) > 0)
        {
            ucError.ErrorMessage = "ETA Should be later than Request Date.";
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
        return (!ucError.IsError);
    }

}