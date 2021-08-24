using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersVesselSupplierList : PhoenixBasePage
{
    private const string SCRIPT_DOFOCUS = @"window.setTimeout('DoFocus()', 1);
            function DoFocus()
            {
                try {
                    document.getElementById('REQUEST_LASTFOCUS').focus();
                } catch (ex) {}
            }";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            HookOnFocus(this.Page as Control);
            if (Request.QueryString["saveyn"] == null)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
                MenuOfficeMain.AccessRights = this.ViewState;
                MenuOfficeMain.MenuList = toolbar.Show();
            }
            if (!IsPostBack)
            {
                
                RadTextBox ucadd = (RadTextBox)ucAddress.FindControl("txtName");
                ucadd.Focus();
                Page.ClientScript.RegisterStartupScript(
                typeof(RegistersVesselSupplierList),
                "ScriptDoFocus",
                SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
                true);

                if (Request.QueryString["SUPPLIERCODE"] != null)
                {
                    ViewState["SUPPLIERCODE"] = Request.QueryString["SUPPLIERCODE"];
                    SupplierEdit();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void HookOnFocus(Control CurrentControl)
    {
        if ((CurrentControl is TextBox) ||
            (CurrentControl is DropDownList))

            (CurrentControl as WebControl).Attributes.Add(
               "onfocus", "try{document.getElementById('__LASTFOCUS').value=this.id} catch(e) {}");
        if (CurrentControl.HasControls())

            foreach (Control CurrentChildControl in CurrentControl.Controls)
                HookOnFocus(CurrentChildControl);
    }   
    protected void OfficeMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            UserControlPhoneNumber phoneno = (UserControlPhoneNumber)ucAddress.FindControl("txtPhone2");
            string scriptRefreshDontClose = "";
            scriptRefreshDontClose += "<script language='javaScript' id='AddressAddNew'>" + "\n";
            scriptRefreshDontClose += "fnReloadList('AddAddress',null,'keepopen');";
            scriptRefreshDontClose += "</script>" + "\n";

            if (CommandName.ToUpper().Equals("NEW"))
            {
                ViewState["SUPPLIERCODE"] = null;
                Reset();
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!ucAddress.IsValidAddress())
                {
                    ucError.ErrorMessage = ucAddress.ErrorMessage;
                    ucError.Visible = true;
                    return;
                }
                if (!General.IsvalidEmail(ucAddress.Email1))
                {
                    ucError.ErrorMessage = "Please enter valid e-mail1";
                    ucError.Visible = true;
                    return;
                }
                
                if (!phoneno.IsValidPhoneNumber())
                {
                    ucError.ErrorMessage = "Enter area code for phone number";
                    ucError.Visible = true;
                    return;
                }
                else
                {                                      
                    
                    if (ViewState["SUPPLIERCODE"] == null)
                    {

                        DataTable dt = PhoenixRegistersVesselSupplier.InsertVesselSupplier
                       (PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                        ucAddress.Name,
                        General.GetNullableString(ucAddress.Address1),
                        General.GetNullableString(ucAddress.Address2),
                        General.GetNullableString(ucAddress.Address3),
                        General.GetNullableString(ucAddress.Address4),                        
                        General.GetNullableInteger(ucAddress.Country),
                        General.GetNullableInteger(ucAddress.State),
                        General.GetNullableInteger(ucAddress.City),
                        General.GetNullableString(ucAddress.PostalCode),
                        General.GetNullableString(ucAddress.Phone1),
                        General.GetNullableString(ucAddress.Phone2),
                        General.GetNullableString(ucAddress.Fax1),
                        General.GetNullableString(ucAddress.Fax2),
                        ucAddress.Email1,
                        General.GetNullableString(ucAddress.Email2),
                        General.GetNullableString(ucAddress.Url), General.GetNullableInteger(ucAddress.Status),                        
                        General.GetNullableString(ucAddress.Attention),
                        General.GetNullableString(ucAddress.InCharge),                       
                        General.GetNullableInteger(ucAddress.QAGrading)
                        );
                        ViewState["SUPPLIERCODE"] = dt.Rows[0][0].ToString();
                        SupplierEdit();
                        ucStatus.Text = "Supplier Information saved";
                    }
                    else if (ViewState["SUPPLIERCODE"] != null)
                    {

                        PhoenixRegistersVesselSupplier.UpdateVesselSupplier(new Guid(ViewState["SUPPLIERCODE"].ToString()),
                        ucAddress.Name,
                        General.GetNullableString(ucAddress.Address1),
                        General.GetNullableString(ucAddress.Address2),
                        General.GetNullableString(ucAddress.Address3),
                        General.GetNullableString(ucAddress.Address4),
                        General.GetNullableInteger(ucAddress.Country),
                        General.GetNullableInteger(ucAddress.State),
                        General.GetNullableInteger(ucAddress.City),
                        General.GetNullableString(ucAddress.PostalCode),
                        General.GetNullableString(ucAddress.Phone1),
                        General.GetNullableString(ucAddress.Phone2),
                        General.GetNullableString(ucAddress.Fax1),
                        General.GetNullableString(ucAddress.Fax2),
                        ucAddress.Email1,
                        General.GetNullableString(ucAddress.Email2),
                        General.GetNullableString(ucAddress.Url), General.GetNullableInteger(ucAddress.Status),
                        General.GetNullableString(ucAddress.Attention),
                        General.GetNullableString(ucAddress.InCharge),
                        General.GetNullableInteger(ucAddress.QAGrading)
                        );
                        SupplierEdit();                        
                        ucStatus.Text = "Supplier Information updated";
                    }
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "AddressAddNew", scriptRefreshDontClose, false);                    
                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }   
    private void Reset()
    {
        ViewState["SUPPLIERCODE"] = null;
        ucAddress.Name = "";
        ucAddress.Address1 = "";
        ucAddress.Address2 = "";
        ucAddress.Address3 = "";
        ucAddress.Country = null;
        ucAddress.State = null;
        ucAddress.City = "";
        ucAddress.PostalCode = "";
        ucAddress.Phone1 = "";
        ucAddress.Phone2 = "";
        ucAddress.Fax1 = "";
        ucAddress.Fax2 = "";
        ucAddress.Email1 = "";
        ucAddress.Email2 = "";
        ucAddress.Url = "";
        ucAddress.QAGrading = null;       
        ucAddress.Attention = "";
        ucAddress.InCharge = "";
        ucAddress.code = "";
    }
    protected void SupplierEdit()
    {
        try
        {
            UserControlPhoneNumber ucaddphoneno = (UserControlPhoneNumber)ucAddress.FindControl("txtPhone1");
            DataTable dt = PhoenixRegistersVesselSupplier.EditVesselSupplier(new Guid(ViewState["SUPPLIERCODE"].ToString()));

            if (dt.Rows.Count > 0)
            {
                DataRow draddress = dt.Rows[0];
                ucAddress.Name = draddress["FLDNAME"].ToString();
                ucAddress.Address1 = draddress["FLDADDRESS1"].ToString();
                ucAddress.Address2 = draddress["FLDADDRESS2"].ToString();
                ucAddress.Address3 = draddress["FLDADDRESS3"].ToString();
                ucAddress.Address4 = draddress["FLDADDRESS4"].ToString();
                ucAddress.Country = draddress["FLDCOUNTRYCODE"].ToString();
                ucAddress.QAGrading = draddress["FLDQAGRADING"].ToString();
                ucAddress.State = draddress["FLDSTATECODE"].ToString();
                ucAddress.City = draddress["FLDCITYID"].ToString();
                ucAddress.PostalCode = draddress["FLDPOSTALCODE"].ToString();
                ucaddphoneno.ISDCode = draddress["FLDISDCODE"].ToString();
                ucAddress.Phone1 = draddress["FLDPHONE1"].ToString();
                ucAddress.Phone2 = draddress["FLDPHONE2"].ToString();
                ucAddress.Email1 = draddress["FLDEMAIL1"].ToString();
                ucAddress.Email2 = draddress["FLDEMAIL2"].ToString();
                ucAddress.Fax1 = draddress["FLDFAX1"].ToString();
                ucAddress.Fax2 = draddress["FLDFAX2"].ToString();
                ucAddress.Url = draddress["FLDURL"].ToString();
                ucAddress.code = draddress["FLDCODE"].ToString();
                ucAddress.Attention = draddress["FLDATTENTION"].ToString();
                ucAddress.InCharge = draddress["FLDINCHARGE"].ToString();
                ucAddress.Status = draddress["FLDSTATUS"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
