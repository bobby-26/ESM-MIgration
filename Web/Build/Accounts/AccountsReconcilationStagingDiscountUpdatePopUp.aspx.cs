using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using Telerik.Web.UI;


public partial class AccountsReconcilationStagingDiscountUpdatePopUp : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    try
    //    {                        
    //        base.Render(writer);
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucConfirm.Attributes.Add("style", "display:none;");
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Update", "UPDATE", ToolBarDirection.Right);
            //toolbar.AddButton("Close", "CLOSE");  
            MenuStaging.Title = "Update All Cr Note Discount";
            MenuStaging.AccessRights = this.ViewState;

            MenuStaging.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                // ucConfirm.Attributes.Add("style", "display:none");
                //ucConfirm.Visible = false;                
                if (Request.QueryString["ORDERID"] != null && Request.QueryString["ORDERID"] != string.Empty)
                    ViewState["ORDERID"] = Request.QueryString["ORDERID"];
                else
                    ViewState["ORDERID"] = "";
                BindPOInfo();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindPOInfo()
    {
        DataSet dsVendor = PhoenixAccountsPOStaging.POStagingEdit(new Guid(ViewState["ORDERID"].ToString()));
        if (dsVendor.Tables[0].Rows.Count > 0)
        {
            DataRow dr = dsVendor.Tables[0].Rows[0];
            txtPONumber.Text = dr["FLDFORMNO"].ToString();
            txtVessel.Text = dr["FLDVESSELNAME"].ToString();
            txtInvoiceNumber.Text = dr["FLDINVOICENUMBER"].ToString();
        }
    }
    protected void MenuStaging_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("UPDATE"))
        {
            try
            {
                if (!IsValidDiscountPercentage(txtDiscountPercentage.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                RadWindowManager1.RadConfirm("Do you want to update the" + '"' + "New Cr Note Discount for ALL the line items" + '"' + "?.", "ucConfirm", 320, 150, null, "Confirm Message");
                //return;
                //ucConfirm.Visible = true;
                //ucConfirm.Text = "Do you want to update the ''New Cr Note Discount for ALL the line items''?.";
                //return;                 
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
    }
    //protected void ucConfirm_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

    //        if (ucCM.confirmboxvalue == 1)
    //        {
    //            int status = 1;
    //            UpdateDiscountForLineItems(ViewState["ORDERID"].ToString(), txtDiscountPercentage.Text, ref status);
    //            //If the changes are already approved or changes are made for the first time status value will be 0 otherwise 1
    //            if (status == 0)
    //                UpdateApprovalStatus(ViewState["ORDERID"].ToString());
    //            string Script = "";
    //            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
    //            Script += " fnReloadList();";
    //            Script += "</script>" + "\n";

    //            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script, true);
    //            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null);", true);
    //            ucStatus.Text = "New Cr Note Discount is updated for all the Line Items.";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //        return;
    //    }
    //}
    protected void ucConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            int status = 1;
            UpdateDiscountForLineItems(ViewState["ORDERID"].ToString(), txtDiscountPercentage.Text, ref status);

            if (status == 0)
                UpdateApprovalStatus(ViewState["ORDERID"].ToString());
            ucStatus.Text = "New Cr Note Discount is updated for all the Line Items.";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void UpdateDiscountForLineItems(string orderid, string discount, ref int status)
    {
        try
        {
            PhoenixAccountsPOStaging.OrderLineStagingUpdateDiscountForLineItems(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(orderid), General.GetNullableDecimal(discount), ref status);
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void UpdateApprovalStatus(string orderid)
    {
        PhoenixAccountsPOStaging.POStagingApprovalStatusUpdate(new Guid(orderid), PhoenixSecurityContext.CurrentSecurityContext.UserCode);
    }
    protected bool IsValidDiscountPercentage(string discountpercentage)
    {
        if (General.GetNullableDecimal(discountpercentage) == null)
            ucError.Text = "Discount Percentage is required.";
        if (General.GetNullableDecimal(discountpercentage) != null && General.GetNullableDecimal(discountpercentage) > 100)
            ucError.Text = "Discount Percentage should be less than or equal to 100.";

        return (!ucError.IsError);
    }
}
