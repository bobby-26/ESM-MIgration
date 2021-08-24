using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;

public partial class PurchaseQuotationCompareSplit : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            toolbarmain.AddButton("Save", "SAVE");
            MenuStockItemGeneral.AccessRights = this.ViewState;   
            MenuStockItemGeneral.MenuList = toolbarmain.Show();          
            txtContractorId.Attributes.Add("style", "visibility:hidden");
            txtQuotationId.Attributes.Add("style", "visibility:hidden");
            txtOrderId.Attributes.Add("style", "visibility:hidden");
            txtOrderName.Attributes.Add("onkeydown", "return false;");
            txtOrderNumber.Attributes.Add("onkeydown", "return false;");
            if (!IsPostBack)
            {
                if (Request.QueryString["orderid"] != null)
                {
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                }
                if (Request.QueryString["orderline"] != null)
                {
                    ViewState["orderline"] = Request.QueryString["orderline"].ToString();
                }
                if (Request.QueryString["quotationid"] != null)
                {
                    ViewState["quotationid"] = Request.QueryString["quotationid"].ToString();
                }
                rdoFormType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.FORMTYPE));
                rdoFormType.DataBind();
                GetDocumentNumber();
                rdoFormType.SelectedValue = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.FORMTYPE), "RQ");
                string formtype = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.FORMTYPE), "RQ");

                if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SPARE"))
                {
                    imgOrder.Attributes.Add("onclick", "return showPickList('spnPickListOrder', 'codehelp1', '', '../Common/CommonPickListOrder.aspx?VESSELID=" + Filter.CurrentPurchaseVesselSelection + "&STOCKTYPE=SPARE', false);");                   
                }
                else
                {
                    imgOrder.Attributes.Add("onclick", "return showPickList('spnPickListOrder', 'codehelp1', '', '../Common/CommonPickListOrder.aspx?VESSELID=" + Filter.CurrentPurchaseVesselSelection + "&STOCKTYPE=STORE', false);");
                }
                btnContractPickList.OnClientClick = "return showPickList('spnPickListContract', 'codehelp1', '', '../Common/CommonPickListAddressQuotation.aspx?orderid=" + ViewState["orderid"].ToString() + "', true);";

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   
    private void GetDocumentNumber()
    {
        DataTable dt = PhoenixPurchaseOrderForm.GetDocumentNumber();
       if (dt.Rows.Count > 0)
       {
           ViewState["DocumentNumber"] = dt.Rows[0]["FLDDOCUMENTTYPEID"].ToString();
       }
       else
       {
           ViewState["DocumentNumber"] = "0";
       }
    }

 
    protected void InventoryStockItemGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                SplitOrderForm();

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
    private void SplitOrderForm()
    {
        if (!IsValidForm())
        {
            ucError.Visible = true;
            return;
        }
        string quotationid = "";
        if (txtQuotationId.Text.Equals(""))
            quotationid = ViewState["quotationid"].ToString();
        else
            quotationid = txtQuotationId.Text;
        PhoenixPurchaseOrderLine.SplitOrderLine(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["orderid"].ToString())
          , ViewState["orderline"].ToString(), General.GetNullableInteger(rdoFormType.SelectedValue.ToString())
            , General.GetNullableInteger(rblCreation.SelectedValue), General.GetNullableInteger(txtContractorId.Text),
            General.GetNullableGuid(txtOrderId.Text), General.GetNullableGuid(quotationid));
        //String script = String.Format("javascript:parent.fnReloadList('code1');");
        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

        PhoenixPurchaseOrderForm.InsertOrderFormHistory(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, 
            General.GetNullableGuid(ViewState["orderid"].ToString()), 
            Filter.CurrentPurchaseVesselSelection);
       
    }

    private bool IsValidForm()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if((rblCreation.SelectedValue =="2") && txtOrderId.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Orderno is required.";
        return (!ucError.IsError);
    }
    protected void rblCreation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblCreation.SelectedValue == "2")
        {
            spnPickListOrder.Visible = true;
            lblordernumber.Visible = true;  
        }
        else
        {
            spnPickListOrder.Visible = false;
            lblordernumber.Visible = false; 

        }
    }
}
