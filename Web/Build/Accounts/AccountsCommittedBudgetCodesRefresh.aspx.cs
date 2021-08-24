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
public partial class AccountsCommittedBudgetCodesRefresh : PhoenixBasePage
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
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuAdvancePayment.AccessRights = this.ViewState;
        MenuAdvancePayment.Title = "Refresh Budget Codes";
        MenuAdvancePayment.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["ORDERID"] = "";
            ViewState["VESSELID"] = "";
            ViewState["ACCOUNTID"] = "";

            if (Request.QueryString["ORDERID"] != null && Request.QueryString["ORDERID"] != string.Empty)
            {
                ViewState["ORDERID"] = Request.QueryString["ORDERID"].ToString();

            }
            if (Request.QueryString["VESSELID"] != null && Request.QueryString["VESSELID"] != string.Empty)
            {
                ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();

            }

            if (General.GetNullableInteger(Request.QueryString["ACCOUNTID"].ToString()) != null)
            {
                ViewState["ACCOUNTID"] = Request.QueryString["ACCOUNTID"].ToString();
                Getprincipal(Convert.ToInt32(ViewState["ACCOUNTID"].ToString()));

            }
            ViewState["BudgetCode"] = "0";
            BindProjectCode();
        }
        BindOwnerBudgetCode();
        
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

                PhoenixAccountsCommittedCosts.CommittedBudgetCodeRefresh(General.GetNullableGuid(ViewState["ORDERID"].ToString())
                                                                        , int.Parse(ucBudgetCode.SelectedBudgetCode)
                                                                        , General.GetNullableGuid(ucOwnerBudgetCode.SelectedValue)
                                                                        , General.GetNullableDateTime(ucEffectiveDate.Text)
                                                                        , General.GetNullableInteger(ucProjectcode.SelectedProjectCode)
                                                                        );

                ucStatus.Text = "Budget Codes changed successfully.";
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

        if (General.GetNullableInteger(ucBudgetCode.SelectedBudgetCode) == null)
            ucError.ErrorMessage = "Budget Code is required.";

        if (General.GetNullableGuid(ucOwnerBudgetCode.SelectedValue) == null)
            ucError.ErrorMessage = "Owner Budget Code is required.";

        //if (General.GetNullableInteger(ucProjectcode.SelectedProjectCode) == null)
        //    ucError.ErrorMessage = "Project Code is required.";

        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }
    protected void ucBudgetCode_Changed(object sender, EventArgs e)
    {
        ucOwnerBudgetCode.SelectedValue = "";
        ucOwnerBudgetCode.Text = "";

        ViewState["BudgetCode"] = ucBudgetCode.SelectedBudgetCode;
        BindOwnerBudgetCode();
        BindProjectCode();
    }


    private void BindProjectCode()
    {

        try
        {
            ucProjectcode.bind(General.GetNullableInteger(ViewState["ACCOUNTID"].ToString())
                        ,General.GetNullableInteger(ViewState["BudgetCode"].ToString()));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }
    private void BindOwnerBudgetCode()
    {
        if (Convert.ToString(ViewState["PRINCIPALID"]) != "" && ViewState["BudgetCode"].ToString() != "")
        {
            //ucOwnerBudgetCode.VesselId = ViewState["VesselId"].ToString();
            ucOwnerBudgetCode.OwnerId = ViewState["PRINCIPALID"].ToString();
            ucOwnerBudgetCode.BudgetId = ViewState["BudgetCode"].ToString();

            int iRowCount = 0;
            int iTotalPageCount = 0;
            int? iownerid = 0;
            DataSet ds1 = PhoenixCommonRegisters.InternalBillingOwnerBudgetCodeSearch(null
                                                                                     , null
                                                                                     , General.GetNullableInteger(Convert.ToString(ViewState["PRINCIPALID"]))
                                                                                     , null
                                                                                     , General.GetNullableInteger(ViewState["BudgetCode"].ToString())
                                                                                     , null, null
                                                                                     , 1
                                                                                     , General.ShowRecords(null)
                                                                                     , ref iRowCount
                                                                                     , ref iTotalPageCount
                                                                                     , ref iownerid);

            if (ds1.Tables[0].Rows.Count > 0)
                ViewState["OwnerBudgetCode"] = "1";
            else
                ViewState["OwnerBudgetCode"] = "";
            if (ds1.Tables[0].Rows.Count == 1)
            {
                DataRow dr = ds1.Tables[0].Rows[0];
                ucOwnerBudgetCode.SelectedValue = dr["FLDOWNERBUDGETCODEID"].ToString();
                ucOwnerBudgetCode.Text = dr["FLDOWNERBUDGETCODE"].ToString();
            }
        }
    }

    public void Getprincipal(int accountid)
    {
        try
        {
            DataSet ds = null;
            ds = PhoenixRegistersAccount.EditAccount(accountid);
            DataRow dr = ds.Tables[0].Rows[0];
            ViewState["PRINCIPALID"] = Convert.ToString(dr["FLDPRINCIPALID"]);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

