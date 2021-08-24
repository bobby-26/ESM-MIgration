using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class VesselAccountsPhoneCardRequestionGeneralNew : PhoenixBasePage
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
                ViewState["ARRANGEYN"] = Request.QueryString["ARRANGEYN"] == null ? null : Request.QueryString["ARRANGEYN"];
                if (Request.QueryString["ARRANGEYN"] == "1")
                {
                    MenuPhonReq.Title = "Arrange Phone Cards";
                }
                txtVesselName.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
                if (ViewState["REQUESTID"] != null)
                    EditPhoneRequisition(new Guid(ViewState["REQUESTID"].ToString()));
            }
            ResetMenu();
            MainMenu();
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
        toolbar.AddButton("List", "LIST");
        toolbar.AddButton("General", "GENERAL");
       
        MenuOrderForm.AccessRights = this.ViewState;
        MenuOrderForm.MenuList = toolbar.Show();
        MenuOrderForm.SelectedMenuIndex = 1;
    }

    protected void MenuPhonReq_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidRequest(txtRequestDate.Text, "1"))
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["REQUESTID"] == null)
                {
                    Guid Requestid = new Guid();
                    PhoenixVesselAccountsPhoneCardRequisition.InsertPhoneCradRequest(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                        DateTime.Parse(txtRequestDate.Text), null, ref Requestid);
                    ViewState["REQUESTID"] = Requestid;
                }
                else
                {
                    PhoenixVesselAccountsPhoneCardRequisition.UpdatePhoneCradRequest(new Guid(ViewState["REQUESTID"].ToString())
                        , DateTime.Parse(txtRequestDate.Text), null);
                }
                Response.Redirect("../VesselAccounts/VesselAccountsPhoneCardRequisitionLineItemNew.aspx?requestid=" + ViewState["REQUESTID"] + "&arrangeyn=" + ViewState["ARRANGEYN"], false);
               
            }
            else if (CommandName.ToUpper().Equals("NEW"))
            {
                ViewState["REQUESTID"] = null;
                ViewState["ACTIVE"] = "1";
                MainMenu();
                txtVesselName.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
                txtRefNo.Text = string.Empty;
                txtRequestDate.Text = string.Empty;
                txtRequestDate.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void OrderForm_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsPhoneCardRequestionGeneralNew.aspx?requestid=" + ViewState["REQUESTID"] + "&arrangeyn=" + ViewState["ARRANGEYN"], false);
            }          
            else if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsPhoneCardRequisitionNew.aspx", false);
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

                if (ViewState["ALLOWEDIT"] != null)
                {
                    if (ViewState["ALLOWEDIT"].ToString() == "false")
                    {
                        txtRequestDate.Enabled = false;
                    }
                }
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
            if (!Filter.CurrentMenuCodeSelection.Contains("VAC-PBL-APC"))
            {
                if (ViewState["ACTIVE"].ToString() != "0")
                    toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
                MenuPhonReq.AccessRights = this.ViewState;
                MenuPhonReq.MenuList = toolbar.Show();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}


