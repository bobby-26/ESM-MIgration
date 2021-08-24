using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class PurchaseFormConvert : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        MenuStockItemGeneral.Title = "Convert ( " + Filter.CurrentPurchaseFormNumberSelection + " )";
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuStockItemGeneral.MenuList = toolbarmain.Show();        
        if (!IsPostBack)
        {
            rdoListFormType.AppendDataBoundItems = true;
            rdoListFormType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.FORMTYPE));
            rdoListFormType.DataBind();
            rdoListFormStatus.AppendDataBoundItems = true;
            if (Filter.CurrentVesselConfiguration.Equals("0") || Filter.CurrentVesselConfiguration == null)
            {
                rdoListFormStatus.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.FORMSTATUS));
            }
            else
            {
                rdoListFormType.Enabled = false;
                rdoListFormStatus.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.FORMSTATUS), 0, "ACT,PKD");
            }
            rdoListFormStatus.DataBind();
        }
        if (!IsPostBack)
        {
            //Title1.Text = "Convert     (  " + PhoenixPurchaseOrderForm.FormNumber + "     )";
            if (Request.QueryString["orderid"] != null)
            {
                ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                BindData();
            }

        }
    }

    private void BindData()
    {

        DataSet ds = new DataSet();
        ds = PhoenixPurchaseOrderForm.EditOrderForm(new Guid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            rdoListFormType.SelectedValue = dr["FLDFORMTYPE"].ToString();
            rdoListFormStatus.SelectedValue = dr["FLDFORMSTATUS"].ToString();           
        }
    }
    protected void InventoryStockItemGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string attachmentflag;
                DataTable dt = PhoenixCommonPurchase.AdavancePurchasePOCancelValidation(General.GetNullableGuid(ViewState["orderid"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    attachmentflag = dt.Rows[0]["FLDFLAG"].ToString();
                    if (attachmentflag == "1")
                    {
                        ucError.ErrorMessage = "Supplier Document Attachment is mandatory to Cancel PO which is having Adavance amount";
                        ucError.Visible = true;
                        return;
                    }
                    else if (attachmentflag == "3")
                    {
                        ucError.ErrorMessage = "Unable to Cancel PO,check with Accounts if the advance has been made";
                        ucError.Visible = true;
                        return;
                    }
                    else if (attachmentflag == "2")
                    {

                        DataTable dtV = PhoenixCommonPurchase.AdvancePurchasePOGeneralValidation(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["orderid"].ToString()), Int32.Parse(rdoListFormStatus.SelectedValue.ToString()), Int32.Parse(rdoListFormType.SelectedValue.ToString()));
                        if (dt.Rows.Count > 0)
                        {
                            ucConfirmMsg.Visible = true;
                            ucConfirmMsg.Text = "Advance Payment already been made,are you sure this PO is to be cancelled?";
                            return;
                        }

                    }
                    else
                        UpdateFormStatus();
                }

            }

            if (CommandName.ToUpper().Equals("CANCEL"))
            {
                if (ViewState["orderid"] != null)
                {
                    Response.Redirect("../Purchase/PurchaseFormGeneral.aspx?orderid=" + ViewState["orderid"].ToString());
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void UpdateFormStatus()
    {
        try
        {
            if (!IsValidForm())
            {
                ucError.Visible = true;
                return;
            }

            PhoenixPurchaseOrderForm.ConvertOrderForm(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["orderid"].ToString()), Int32.Parse(rdoListFormStatus.SelectedValue.ToString()), Int32.Parse(rdoListFormType.SelectedValue.ToString()));
            InsertOrderFormHistory();
            String script = String.Format("javascript:parent.fnReloadList('code1');");
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
       
    }
    private void InsertOrderFormHistory()
    {
        PhoenixPurchaseOrderForm.InsertOrderFormHistory(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection);
    }
    private bool IsValidForm()
    {  

        return (!ucError.IsError);
    }

    protected void CheckMapping_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
            if (ucCM.confirmboxvalue == 1)
            {
                if (General.GetNullableGuid(ViewState["orderid"].ToString()) != null)
                {
                    PhoenixCommonPurchase.AdavancePurchasePOCreditNoteInsert(General.GetNullableGuid(ViewState["orderid"].ToString()));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
