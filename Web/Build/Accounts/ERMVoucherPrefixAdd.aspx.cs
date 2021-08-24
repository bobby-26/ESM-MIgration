using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class ERMVoucherPrefixAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuBudget.Title = "ERM Voucher Prefix";
        MenuBudget.AccessRights = this.ViewState;
        MenuBudget.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            if ((Request.QueryString["PrefixId"].ToString().Trim() != "" ? Request.QueryString["PrefixId"].ToString() : null) != null)
                BudgetEdit();
        }
    }

    protected void BudgetEdit()
    {
        if ((Request.QueryString["PrefixId"].ToString().Trim() != "" ? Request.QueryString["PrefixId"].ToString() : null) != null)
        {
            string prefixid = Request.QueryString["PrefixId"].ToString();
            DataSet dsBudget = PhoenixAccountsERMVesselAccruedExpensess.ERMVoucherPrefixEdit(General.GetNullableGuid(Request.QueryString["PrefixId"].ToString()));

            if (dsBudget.Tables.Count > 0)
            {
                DataRow drBudget = dsBudget.Tables[0].Rows[0];
                txtcompany.Text = drBudget["FLDCOMPANY"].ToString();
                txtDatabase1.Text = drBudget["FLDDATABASE"].ToString();
                txtERM.Text = drBudget["FLDERM"].ToString();
                txtPhoenix.Text = drBudget["FLDPHOENIX"].ToString();
                txtPhoenixTRN.Text = drBudget["FLDPHOENIXTRN"].ToString();
                txtXAccess.Text = drBudget["FLDXACCESS"].ToString();
                txtXTime.Text = drBudget["FLDZTIME"].ToString();
                txtZUTime.Text = drBudget["FLDZUTIME"].ToString();
                txtZID.Text = drBudget["FLDZID"].ToString();
                txtXtypeTRN.Text = drBudget["FLDXTYPETRN"].ToString();
                txtXTRN.Text = drBudget["FLDXTRN"].ToString();
                txtXAction.Text = drBudget["FLDXACTION"].ToString();
                txtXDescription.Text = drBudget["FLDXDESC"].ToString();
                txtXNumber.Text = drBudget["FLDXNUM"].ToString();
                txtXInc.Text = drBudget["FLDXINC"].ToString();
                txtZActive.Text = drBudget["FLDZACTIVE"].ToString();
                txtColumn1.Text = drBudget["FLDCOLUMN1"].ToString();
                txtDatabase2.Text = drBudget["FLDDATABASE2"].ToString();

            }
        }
    }

    protected void MenuMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            string Script = "";
            Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
            Script += "fnReloadList('codehelp1');";
            Script += "</script>" + "\n";

            NameValueCollection criteria = new NameValueCollection();

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if ((Request.QueryString["PrefixId"].ToString().Trim() != "" ? Request.QueryString["PrefixId"].ToString() : null) == null)
                {
                    PhoenixAccountsERMVesselAccruedExpensess.ERMVoucherPrefixInsert(txtcompany.Text.Trim()
                                                                             , txtDatabase1.Text.Trim()
                                                                             , txtERM.Text.Trim()
                                                                             , txtPhoenix.Text.Trim()
                                                                             , txtPhoenixTRN.Text.Trim()
                                                                             , txtXAccess.Text.Trim()
                                                                             , txtXTime.Text.Trim()
                                                                             , txtZUTime.Text.Trim()
                                                                             , txtZID.Text.Trim()
                                                                             , txtXtypeTRN.Text.Trim()
                                                                             , txtXTRN.Text.Trim()
                                                                             , txtXAction.Text.Trim()
                                                                             , txtXDescription.Text.Trim()
                                                                             , txtXNumber.Text.Trim()
                                                                             , txtXInc.Text.Trim()
                                                                             , txtZActive.Text.Trim()
                                                                             , txtColumn1.Text.Trim()
                                                                             , txtDatabase2.Text.Trim()

                                                                        );
                }
                else
                {
                    PhoenixAccountsERMVesselAccruedExpensess.ERMVoucherPrefixUpdate(txtcompany.Text.Trim()
                                                                             , txtDatabase1.Text.Trim()
                                                                             , txtERM.Text.Trim()
                                                                             , txtPhoenix.Text.Trim()
                                                                             , txtPhoenixTRN.Text.Trim()
                                                                             , txtXAccess.Text.Trim()
                                                                             , txtXTime.Text.Trim()
                                                                             , txtZUTime.Text.Trim()
                                                                             , txtZID.Text.Trim()
                                                                             , txtXtypeTRN.Text.Trim()
                                                                             , txtXTRN.Text.Trim()
                                                                             , txtXAction.Text.Trim()
                                                                             , txtXDescription.Text.Trim()
                                                                             , txtXNumber.Text.Trim()
                                                                             , txtXInc.Text.Trim()
                                                                             , txtZActive.Text.Trim()
                                                                             , txtColumn1.Text.Trim()
                                                                             , txtDatabase2.Text.Trim()
                                                                             , General.GetNullableGuid(Request.QueryString["PrefixId"].ToString())

                                                                        );
                }
                ucStatus.Text = "Record Saved Successfully.";
                ucStatus.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

        String script = "javascript:fnReloadList('codehelp1');";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

    }
}



