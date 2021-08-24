using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PurchaseFalApproveLimitEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuPurchaseFalApproveLimitEdit.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            Guid? Id = General.GetNullableGuid(Request.QueryString["APPROVALLIMITID"].ToString());
            DataTable dT;
            dT = PhoenixPurchaseFalApprove.PurchaseFalApproveLimitEdit(Id);
            if (dT.Rows.Count > 0)
            {
                DataRow dr = dT.Rows[0];
                lblLevel.Text = dr["FLDAPPROVALLIMIT"].ToString();
                UcRules.SelectedPurchaseFalRules = dr["FLDAPPROVALRULEID"].ToString();             
                txtName.Text = dr["FLDLEVELNAME"].ToString();
                txtApprove.Text = dr["FLDAPPROVALID"].ToString();
            }
        }
    }

    protected void MenuPurchaseFalApproveLimitEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            decimal? Limit = General.GetNullableDecimal(lblLevel.Text);                      
            Guid? Rule = General.GetNullableGuid(UcRules.SelectedPurchaseFalRules);
            Guid? Approval = General.GetNullableGuid(txtApprove.Text);

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidApproveLimit(Limit, Rule))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixPurchaseFalApprove.PurchaseFalApproveLimitUpdate(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Limit
                    , Rule
                    , Approval
                    , General.GetNullableGuid(Request.QueryString["APPROVALLIMITID"].ToString())

                    );

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                          "BookMarkScript", "fnReloadList('Add', 'ifMoreInfo', null);", true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }

    private bool IsValidApproveLimit(decimal? Limit, Guid? Rule)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((Limit) == null)
            ucError.ErrorMessage = "Approval Limit is required.";

        if (Rule == null)
            ucError.ErrorMessage = "Rule is required.";

        return (!ucError.IsError);
    }

}

