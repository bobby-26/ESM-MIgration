using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;

public partial class VesselAccountsPhoneCardRequisitionGeneral :  PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["REQUESTID"] = Request.QueryString["REQUESTID"];
                ViewState["ALLOWEDIT"] = Request.QueryString["ALLOWEDIT"];
                ViewState["ACTIVE"] = "1";
                txtVesselName.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
                if (ViewState["REQUESTID"] != null)
                    EditPhoneRequisition(new Guid(ViewState["REQUESTID"].ToString()));
              
                //txtSupplierId.Attributes.Add("style", "visibility:hidden");
            }  MainMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuPhonReq_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidRequest(txtRequestDate.Text,"1"))
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["REQUESTID"] == null)
                {
                    PhoenixVesselAccountsPhoneCardRequisition.InsertPhoneCradRequest(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                        DateTime.Parse(txtRequestDate.Text), null);
                }
                else
                {
                    PhoenixVesselAccountsPhoneCardRequisition.UpdatePhoneCradRequest(new Guid(ViewState["REQUESTID"].ToString())
                        , DateTime.Parse(txtRequestDate.Text), null);
                    EditPhoneRequisition(new Guid(ViewState["REQUESTID"].ToString()));
                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
            }
            else if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                ViewState["REQUESTID"] = null;
                ViewState["ACTIVE"] = "1";
                MainMenu();
                txtVesselName.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
                txtRefNo.Text = string.Empty;
                txtRequestDate.Text = string.Empty;
                //txtSupplierId.Text = string.Empty;
                //txtSupplierCode.Text = string.Empty;
                //txtSupplierName.Text = string.Empty;
                txtRequestDate.Enabled = true;
                //txtSupplierId.Enabled = true;
                //txtSupplierCode.Enabled = true;
                //txtSupplierName.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void EditPhoneRequisition(Guid gRequestId)
    {
        try
        {
            DataTable dt = PhoenixVesselAccountsPhoneCardRequisition.EditPhoneCradRequest(gRequestId);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtVesselName.Text = dr["FLDVESSELNAME"].ToString();
                txtRefNo.Text = dr["FLDREFERENCENO"].ToString();
                txtRequestDate.Text = dr["FLDREQUESTDATE"].ToString();
                //txtSupplierId.Text = dr["FLDSUPPLIERCODE"].ToString();
                //txtSupplierCode.Text = dr["FLDCODE"].ToString();
                //txtSupplierName.Text = dr["FLDNAME"].ToString();

                if (ViewState["ALLOWEDIT"] != null)
                {
                    if (ViewState["ALLOWEDIT"].ToString() == "false")
                    {
                        txtRequestDate.Enabled = false;
                        //txtSupplierId.Enabled = false;
                        //txtSupplierCode.Enabled = false;
                        //txtSupplierName.Enabled = false;
                    }
                }
                MainMenu();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidRequest(string requestdate, string supplier)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableDateTime(requestdate).HasValue)
        {
            ucError.ErrorMessage = "Request Date is required.";
        }
        else if (DateTime.TryParse(requestdate, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Request Date should be earlier than current date";
        }
        if (String.IsNullOrEmpty(supplier))
        {
            ucError.ErrorMessage = "Ship Chandler is required.";
        }

        return (!ucError.IsError);
    }

    private void MainMenu()
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("New", "NEW");
            if (ViewState["ACTIVE"].ToString() != "0")
                toolbar.AddButton("Save", "SAVE");
            MenuPhonReq.AccessRights = this.ViewState;
            MenuPhonReq.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
