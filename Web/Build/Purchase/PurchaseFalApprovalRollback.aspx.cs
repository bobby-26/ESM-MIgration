using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Purchase;
using Telerik.Web.UI;

public partial class Purchase_PurchaseFalApprovalRollback : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("Save", "Save", ToolBarDirection.Right);
        Menu.MenuList = toolbargrid.Show();
        if (!Page.IsPostBack)
        { 
        Guid? Approvalid = General.GetNullableGuid(Request.QueryString["approvalid"]);

        radcblevel.DataSource = PhoenixPurchaseFalApprovalLevel.FalApprovedList(Approvalid);
        radcblevel.DataValueField = "FLDLEVELID";
        radcblevel.DataTextField = "FLDLEVELNAME";
        radcblevel.DataBind();
        }

    }

    protected void Menu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                Guid? ApprovalId = General.GetNullableGuid(Request.QueryString["approvalid"]);
                Guid? RollbackLevelID = General.GetNullableGuid(radcblevel.SelectedValue);
                string Reason = General.GetNullableString(radtbreson.Text);
                if (!IsValidRollback(RollbackLevelID, Reason))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPurchaseFalApprovalLevel.FalRollback(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ApprovalId, RollbackLevelID, Reason);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "closeTelerikWindow('Filters', 'approval');", true); 
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }


    }

    private bool IsValidRollback(Guid? RollbackLevelID,string  Reason)
    {

        ucError.HeaderMessage = "Provide the following required information";
        if(RollbackLevelID == null)
            ucError.ErrorMessage = "Rollback Level.";
        if (Reason == null)
            ucError.ErrorMessage = "Reason .";


        return (!ucError.IsError);
    }
}