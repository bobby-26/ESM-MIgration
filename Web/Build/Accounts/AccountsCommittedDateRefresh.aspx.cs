using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class AccountsCommittedDateRefresh : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        cmdHiddenSubmit.Attributes.Add("style", "display:none;");

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuAdvancePayment.AccessRights = this.ViewState;
        MenuAdvancePayment.Title = "Change Committed Date";
        MenuAdvancePayment.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["ORDERID"] = "";
            ViewState["VESSELID"] = "";

            if (Request.QueryString["ORDERID"] != null && Request.QueryString["ORDERID"] != string.Empty)
            {
                ViewState["ORDERID"] = Request.QueryString["ORDERID"].ToString();

            }
            if (Request.QueryString["VESSELID"] != null && Request.QueryString["VESSELID"] != string.Empty)
            {
                ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();

            }
            ViewState["BudgetCode"] = "0";
        }
       // BindOwnerBudgetCode();
    }

    protected void MenuAdvancePayment_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidExclude())
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountsCommittedCosts.CommittedBudgetDateRefresh(General.GetNullableGuid(ViewState["ORDERID"].ToString())
                                                                        , General.GetNullableDateTime(ucEffectiveDate.Text)
                                                                        , General.GetNullableDateTime(ucCommittedDate.Text)
                                                                        , General.GetNullableString(txtReason.Text)
                                                                        );

                ucStatus.Text = "Committed Date is changed successfully.";
                String scriptupdate = String.Format("javascript:fnReloadList('codehelp1','yes','yes');");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", scriptupdate, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidExclude()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(ucEffectiveDate.Text) == null)
            ucError.ErrorMessage = "Effective Date is required.";

        if (General.GetNullableDateTime(ucCommittedDate.Text) == null)
            ucError.ErrorMessage = "New Committed Date is required.";

        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }
   

   
}

