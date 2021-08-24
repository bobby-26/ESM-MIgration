using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using System.Web;
using Telerik.Web.UI;

public partial class PurchaseReasonForRequisition : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            MenuFormDetail.Title = "Reason for Requisition     (  " + PhoenixPurchaseOrderForm.FormNumber + "     )";
            if (Request.QueryString["launchedfrom"] == null)
            {
                toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
                MenuFormDetail.AccessRights = this.ViewState;
                MenuFormDetail.MenuList = toolbarmain.Show();
            }
            if (!IsPostBack)
            {
                
                if ((Request.QueryString["orderid"] != null) && (Request.QueryString["orderid"] != ""))
                {
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                    ViewState["dtkey"] = string.Empty;
                    BindData(Request.QueryString["orderid"].ToString());
                }
                if (Request.QueryString["launchedfrom"] == null)
                {
                    if (!Filter.CurrentPurchaseVesselSendDateSelection.ToUpper().Equals("") && !(Filter.CurrentVesselConfiguration.Equals("0") || Filter.CurrentVesselConfiguration == null))
                    {
                        MenuFormDetail.Visible = false;
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

    private void BindData(string orderid)
    {
        DataSet ds = PhoenixPurchaseOrderForm.EditOrderForm(new Guid(orderid));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtReasonForReq.Text = dr["FLDUSERDEFTEXT"].ToString();
            ViewState["dtkey"] = dr["FLDDTKEY"].ToString();
        }
    }

    protected void MenuFormDetail_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if ((Request.QueryString["orderid"] != null) && (Request.QueryString["orderid"] != ""))
                {
                    PhoenixPurchaseOrderForm.UpdateOrderFormReason(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode, 
                        new Guid(ViewState["orderid"].ToString()), 
                        txtReasonForReq.Text);

                    ucStatus.Text = "Reason updated successfully.";
                    ucStatus.Visible = true;
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
