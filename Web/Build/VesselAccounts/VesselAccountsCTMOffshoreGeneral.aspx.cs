using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
public partial class VesselAccountsCTMOffshoreGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("General", "GENERAL");
            toolbarmain.AddButton("List", "LIST");
            MenuCTMMain.AccessRights = this.ViewState;
            MenuCTMMain.MenuList = toolbarmain.Show();
            MenuCTMMain.SelectedMenuIndex = 0;
            if (!IsPostBack)
            {
                toolbarmain = new PhoenixToolbar();
                ViewState["ACTIVEYN"] = null;
                ViewState["CTMID"] = Request.QueryString["CTMID"];
                txtDate.Text = DateTime.Now.ToString();
                if (ViewState["CTMID"] != null && ViewState["CTMID"].ToString() != string.Empty)
                    EditCTM(new Guid(ViewState["CTMID"].ToString()));
                else
                {
                    DataSet ds = PhoenixRegistersVessel.EditVessel(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                    if (ds.Tables[0].Rows.Count > 0)
                        lblhead.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
                }
                lblArrangedAmount.Visible = false;
                txtAmountArranged.Visible = false;
                txtSupplierId.Attributes.Add("style", "display:none");
             
            }   MENU();
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
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("LIST"))
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
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE") || dce.CommandName.ToUpper().Equals("SENDTOOFFICE"))
            {
                string date = txtDate.Text;
                string port = ddlPort.SelectedValue;
                string eta = txtETA.Text;
                string etd = txtETD.Text;
                string supplier = txtSupplierId.Text;
                string amount = txtAmount.Text;
                if (!IsValidCTM(amount, txtRemarks.Text, date, port, eta, etd, supplier))
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["CTMID"] == null || ViewState["CTMID"].ToString() == string.Empty)
                {
                    Guid CTMID = new Guid();
                    PhoenixVesselAccountsCTM.InsertCTMRequest(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , DateTime.Parse(date), int.Parse(port), DateTime.Parse(txtETA.Text), DateTime.Parse(txtETD.Text)
                        , new Guid(supplier), decimal.Parse(amount), null, null, txtRemarks.Text.Trim(), ref CTMID);
                    ViewState["CTMID"] = CTMID.ToString();
                    ucStatus.Text = "Request Created.";
                }
                else
                {
                    PhoenixVesselAccountsCTM.UpdateCTMRequest(new Guid(ViewState["CTMID"].ToString())
                       , DateTime.Parse(date), int.Parse(port), DateTime.Parse(txtETA.Text), DateTime.Parse(txtETD.Text)
                       , new Guid(supplier), decimal.Parse(amount), null, null
                       , byte.Parse(dce.CommandName.ToUpper().Equals("SAVE") ? "0" : "1"), General.GetNullableDecimal(txtAmountArranged.Text)
                       , txtRemarks.Text.Trim(), General.GetNullableDateTime(txtReceivedDate.Text), General.GetNullableDecimal(txtReceivedAmount.Text));
                    ucStatus.Text = "Request Updated.";
                }
                EditCTM(new Guid(ViewState["CTMID"].ToString()));

            }
            else if (dce.CommandName.ToUpper().Equals("SAVEREC"))
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
    private void MENU()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (ViewState["ACTIVEYN"] == null || ViewState["ACTIVEYN"].ToString() == "1")
        {
            toolbar.AddButton("Save", "SAVE");
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                toolbar.AddButton("Send to Office", "SENDTOOFFICE");
            }
            if (ViewState["CTMID"] != null && ViewState["CTMID"].ToString() != "")
            {
                divSub.Visible = true;
                toolbar.AddButton("Received", "SAVEREC");
            }
            else
            {
                divSub.Visible = false;
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
            ddlPort.SelectedValue = dr["FLDSEAPORTID"].ToString();
            ddlPort.Text = dr["FLDPORTNAME"].ToString();
            txtETA.Text = dr["FLDETA"].ToString();
            txtETD.Text = dr["FLDETD"].ToString();
            txtSupplierCode.Text = dr["FLDCODE"].ToString();
            txtSupplierName.Text = dr["FLDNAME"].ToString();
            txtSupplierId.Text = dr["FLDSUPPLIERCODE"].ToString();
            txtAmount.Text = dr["FLDAMOUNT"].ToString();
            txtAmount.ReadOnly = true;
            txtAmountArranged.Text = dr["FLDAMOUNTARRANGED"].ToString();
            txtAmountArranged.ReadOnly = false;
            txtAmountArranged.CssClass = "input";
            txtRemarks.Text = dr["FLDOFFICEREMARKS"].ToString();
            txtRemarks.ReadOnly = false;
            txtRemarks.CssClass = "input";
            txtReceivedAmount.Text = dr["FLDRECEIVEDAMOUNT"].ToString();
            txtReceivedDate.Text = dr["FLDRECEIVEDDATE"].ToString();
            ViewState["ACTIVEYN"] = dr["FLDACTIVEYN"].ToString();
            txtRemarks.Text = dr["FLDPURPOSE"].ToString();
        }
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
    private bool IsValidCTM(string amount, string reason, string date, string port, string eta, string etd, string portagent)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Date is required.";
        }
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Date should be earlier than current date";
        }
        if (!General.GetNullableInteger(port).HasValue)
        {
            ucError.ErrorMessage = "Port is required.";
        }
        if (!General.GetNullableGuid(portagent).HasValue)
        {
            ucError.ErrorMessage = "Port Agent is required.";
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
        if (General.GetNullableDecimal(amount) == null)
        {
            ucError.ErrorMessage = "Requested amount is required.";
        }
        if (General.GetNullableString(reason) == null)
        {
            ucError.ErrorMessage = "Reason for request is required.";
        }
        return (!ucError.IsError);
    }
}