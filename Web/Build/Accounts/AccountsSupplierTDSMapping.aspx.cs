using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class AccountsSupplierTDSMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
       
            PhoenixToolbar toolbar = new PhoenixToolbar();       

            if (Request.QueryString["Addresscode"] != null)
            {
                toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
                ViewState["Addresscode"] = Request.QueryString["Addresscode"].ToString();
                SupplierTDSMappingEdit();
            }
            else
            {
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            }
           MenuSupplierRegister.Title = "Suppier Register";
            MenuSupplierRegister.AccessRights = this.ViewState;
            MenuSupplierRegister.MenuList = toolbar.Show();           
      
    }

    protected void MenuSupplierRegister_TabStripCommand(object sender, EventArgs e)
    {
        string Script = "";
        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        Script += "fnReloadList('codehelp1', null, null);";
        Script += "</script>" + "\n";

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            //if (!IsValidCompany())
            //{
            //    ucError.Visible = true;
            //    return;
            //}

            try
            {
                if (ViewState["Addresscode"] != null)
                {
                    PhoenixAccountsTDSRegister.SupplierTDSMappingInsert(General.GetNullableInteger(ViewState["Addresscode"].ToString())
                                                                , General.GetNullableInteger(ddlTDSType.SelectedValue)
                                                                , General.GetNullableInteger(ddlWCTType.SelectedValue)
                                                                );
                    ucStatus.Text = "Supplier Configuration updated.";
                }
                
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
    }

    private void SupplierTDSMappingEdit()
    {
        DataTable dt = PhoenixAccountsTDSRegister.SupplierTDSMappingEdit(General.GetNullableInteger(ViewState["Addresscode"].ToString()));
        
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];

            txtSupplierName.Text = dr["FLDNAME"].ToString();
            txtSupplierCode.Text = dr["FLDCODE"].ToString();
            ddlTDSType.SelectedValue= dr["FLDTDSCONTRACTORTYPE"].ToString();
            ddlWCTType.SelectedValue = dr["FLDTDSONWCTTYPE"].ToString();
        }
    }   
}
