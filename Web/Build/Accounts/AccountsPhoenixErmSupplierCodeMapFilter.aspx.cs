using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using System.Text;
using Telerik.Web.UI;
public partial class AccountsPhoenixErmSupplierCodeMapFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
                    
        }
        ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=183,138,334,131,132', true); ");
        txtVendorId.Attributes.Add("style", "visibility:hidden");
    }
    
    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string addresstype = null;
        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        NameValueCollection criteria = new NameValueCollection();

        if (CommandName.ToUpper().Equals("GO"))
        {
            if (ddlSupplierType.SelectedHard.ToUpper() != "DUMMY")
            {
                if (ddlSupplierType.SelectedHard.ToUpper() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "SPI"))
                {
                    addresstype = ",130,131,";
                }
                else if (ddlSupplierType.SelectedHard.ToUpper() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "MDL"))
                {
                    addresstype = ",183,";
                }
                else if (ddlSupplierType.SelectedHard.ToUpper() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "PCD")
                || ddlSupplierType.SelectedHard.ToUpper() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "ROL"))
                {
                    addresstype = ",132,";
                }
                else if (ddlSupplierType.SelectedHard.ToUpper() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59, "AVN"))
                {
                    addresstype = ",135,";
                }                
            }
            criteria.Clear();
            criteria.Add("PhoenixSupplierId", txtVendorId.Text.Trim());
            criteria.Add("AddressType", addresstype);

            Filter.CurrentPhoenixErmSupplierCodeMapSelection = criteria;
            Response.Redirect("../Accounts/AccountsPhoenixErmSupplierCodeMap.aspx", false);
        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
            Filter.CurrentPhoenixErmSupplierCodeMapSelection = criteria;
            Response.Redirect("../Accounts/AccountsPhoenixErmSupplierCodeMap.aspx", false);
        }
    }       
}
