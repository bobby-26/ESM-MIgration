using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using System.Data;
using SouthNests.Phoenix.Accounts;
using System.Web.UI;
using System.Text;

public partial class AccountsDirectPORemainingBudget : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Approve", "APPROVE");
            MenuFormFilter.MenuList = toolbarmain.Show();
            
            
            if (!IsPostBack)
            {

                ViewState["BudgetExceeded"] = "0";

                if (!string.IsNullOrEmpty(Request.QueryString["orderID"]))
                    ViewState["orderID"] = Request.QueryString["orderID"].ToString();

                //if (!string.IsNullOrEmpty(Request.QueryString["type"]))
                //    ViewState["type"] = Request.QueryString["type"].ToString();

                //if (!string.IsNullOrEmpty(Request.QueryString["user"]))
                //    ViewState["user"] = Request.QueryString["user"].ToString();

                //if (!string.IsNullOrEmpty(Request.QueryString["currentorder"]))
                //    ViewState["currentorder"] = Request.QueryString["currentorder"].ToString();

                //if (!string.IsNullOrEmpty(Request.QueryString["directapprovalyn"]))
                //    ViewState["directapprovalyn"] = Request.QueryString["directapprovalyn"].ToString();

                GetAccount();

                if (ViewState["budgetid"]!=null && General.GetNullableInteger(ViewState["budgetid"].ToString())==null)
                {
                    ucError.ErrorMessage = "Budget codes not updated in Order Form.";
                    ucError.Visible = true;
                    return;
                }

                Edit();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void GetAccount()
    {
        DataTable dt = new DataTable();
        if (ViewState["orderID"] != null && ViewState["orderID"].ToString() != "")
        {
            dt = PhoenixAccountsPOStaging.DirectPOAccountDeatils(new Guid(ViewState["orderID"].ToString()));
            if (dt.Rows.Count>0)
            {
                ViewState["accountid"] = dt.Rows[0]["FLDACCOUNTID"].ToString();
                ViewState["budgetid"] = dt.Rows[0]["FLDBUDGETID"].ToString();
                ViewState["currentorder"] = dt.Rows[0]["FLDTOTALAMOUNT"].ToString();
            }
        }
            
    }
    private void Edit()
    {
        if(General.GetNullableInteger(ViewState["budgetid"].ToString())!=null&& General.GetNullableInteger(ViewState["accountid"].ToString())!=null)
        {
            DataSet ds = SouthNests.Phoenix.Purchase.PurchaseRemainingBudget.RemainigBudgetForDirectPO(int.Parse(ViewState["budgetid"].ToString()), int.Parse(ViewState["accountid"].ToString()), General.GetNullableDecimal(ViewState["currentorder"].ToString()));

            DataRow dr = ds.Tables[0].Rows[0];
            Title1.Text = dr["FLDBUDGETGROUP"].ToString() + ": " + dr["FLDSUBACCOUNT"].ToString() + " " + dr["FLDSUBACCOUNTDESC"].ToString();
            lblBudgetGroup.Text = dr["FLDBUDGETGROUP"].ToString()+": "+ dr["FLDSUBACCOUNT"].ToString()+" "+dr["FLDSUBACCOUNTDESC"].ToString();
            lblVarianceDesc.Text = dr["FLDVARIANCEDESC"].ToString();

            if (General.GetNullableDecimal(dr["FLDVARIANCE"].ToString()) < 0)
            {
                lblVariance.Text = "(" + string.Format("{0:N}", General.GetNullableDecimal(dr["FLDVARIANCE"].ToString()) * -1) + ")";
                lblVariance.Attributes.Add("style", "color:red");
            }
            else
            {
                lblVariance.Text = string.Format("{0:N}", dr["FLDVARIANCE"]);
            }
                
            

            lblBudgetDesc.Text= dr["FLDMONTHLYBUDGETDESC"].ToString();
            if (General.GetNullableDecimal(dr["FLDMONTHLYBUDGET"].ToString()) < 0)
            {
                lblBudget.Text = "(" + string.Format("{0:N}", General.GetNullableDecimal(dr["FLDMONTHLYBUDGET"].ToString()) * -1) + ")";
                lblBudget.Attributes.Add("style", "color:red");
            }
            else
            {
                lblBudget.Text = string.Format("{0:N}", dr["FLDMONTHLYBUDGET"]);
            }
            

            lblChaCommittedDesc.Text= dr["FLDCOMMITTEDCHANGESDESC"].ToString();
            if (General.GetNullableDecimal(dr["FLDCOMMITTEDCHANGES"].ToString()) < 0)
            {
                lblChaCommitted.Text = "(" + string.Format("{0:N}", General.GetNullableDecimal(dr["FLDCOMMITTEDCHANGES"].ToString()) * -1) + ")";
                lblChaCommitted.Attributes.Add("style", "color:red");
            }
            else
            {
                lblChaCommitted.Text = string.Format("{0:N}", dr["FLDCOMMITTEDCHANGES"]);
            }

            lblActualDesc.Text= dr["FLDACTUALCHARGEDDESC"].ToString();

            if (General.GetNullableDecimal(dr["FLDACTUALCHARGED"].ToString()) < 0)
            {
                lblActual.Text = "(" + string.Format("{0:N}", General.GetNullableDecimal(dr["FLDACTUALCHARGED"].ToString()) * -1) + ")";
                lblActual.Attributes.Add("style", "color:red");
            }
            else
            {
                lblActual.Text = string.Format("{0:N}", dr["FLDACTUALCHARGED"]);
            }

            lblCurrentDesc.Text= dr["FLDCURRENTORDERDESC"].ToString();

            if (General.GetNullableDecimal(dr["FLDCURRENTORDERAMOUNT"].ToString()) < 0)
            {
                lblCurrent.Text = "(" + string.Format("{0:N}", General.GetNullableDecimal(dr["FLDCURRENTORDERAMOUNT"].ToString()) * -1) + ")";
                lblCurrent.Attributes.Add("style", "color:red");
            }
            else
            {
                lblCurrent.Text = string.Format("{0:N}", dr["FLDCURRENTORDERAMOUNT"]);
            }


            lblAppNotOrderDesc.Text= dr["FLDAPPNOTORDERDDESC"].ToString();

            if (General.GetNullableDecimal(dr["FLDAPPNOTORDERD"].ToString()) < 0)
            {
                lblAppNotOrder.Text = "(" + string.Format("{0:N}", General.GetNullableDecimal(dr["FLDAPPNOTORDERD"].ToString()) * -1) + ")";
                lblAppNotOrder.Attributes.Add("style", "color:red");
            }
            else
            {
                lblAppNotOrder.Text = string.Format("{0:N}", dr["FLDAPPNOTORDERD"]);
            }
            

            lblMonthlyRemainigDesc.Text = dr["FLDMONTHLYREMAININGDESC"].ToString();
            if (General.GetNullableDecimal(dr["FLDMONTHLYREMAINING"].ToString()) < 0)
            {
                lblMonthlyRemainig.Text = "(" + string.Format("{0:N}", General.GetNullableDecimal(dr["FLDMONTHLYREMAINING"].ToString()) * -1) + ")";
                lblMonthlyRemainig.Attributes.Add("style", "color:red");
            }
            else
            {
                lblMonthlyRemainig.Text = string.Format("{0:N}", dr["FLDMONTHLYREMAINING"]);
            }


            lblYTDRemainigDesc.Text= dr["FLDYTDREMAININGDESC"].ToString();
            if (General.GetNullableDecimal(dr["FLDYTDREMAINING"].ToString()) < 0)
            {
                lblYTDRemainig.Text = "(" + string.Format("{0:N}", General.GetNullableDecimal(dr["FLDYTDREMAINING"].ToString()) * -1) + ")";
                lblYTDRemainig.Attributes.Add("style", "color:red");
                ViewState["BudgetExceeded"] = "1";
            }
            else
            {
                lblYTDRemainig.Text = string.Format("{0:N}", dr["FLDYTDREMAINING"]);
            }

            DataSet ds1 = SouthNests.Phoenix.Purchase.PurchaseRemainingBudget.TechRemainigBudget(int.Parse(ViewState["accountid"].ToString()), General.GetNullableDecimal(ViewState["currentorder"].ToString()));

            dr = ds1.Tables[0].Rows[0];

            lblTechMonthlyRemainigDesc.Text = dr["FLDMONTHLYREMAININGDESC"].ToString();
            if (General.GetNullableDecimal(dr["FLDMONTHLYREMAINING"].ToString()) < 0)
            {
                lblTechMonthlyRemainig.Text = "(" + string.Format("{0:N}", General.GetNullableDecimal(dr["FLDMONTHLYREMAINING"].ToString())*-1) + ")";
                lblTechMonthlyRemainig.Attributes.Add("style", "color:red");
            }
            else
            {
                lblTechMonthlyRemainig.Text = string.Format("{0:N}", dr["FLDMONTHLYREMAINING"]);
            }

            lblTechYTDRemainigDesc.Text = dr["FLDYTDREMAININGDESC"].ToString();

            if (General.GetNullableDecimal(dr["FLDYTDREMAINING"].ToString()) < 0)
            {
                lblTechYTDRemainig.Text = "(" + string.Format("{0:N}", General.GetNullableDecimal(dr["FLDYTDREMAINING"].ToString()) * -1) + ")";
                lblTechYTDRemainig.Attributes.Add("style", "color:red");
            }
            else
            {
                lblTechYTDRemainig.Text = string.Format("{0:N}", dr["FLDYTDREMAINING"]);
            }
        }
        
    }
    protected void MenuFormFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("APPROVE"))
            {
                PhoenixAccountsPOStaging.DirectPOApprove(new Guid(ViewState["orderID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            int.Parse(ViewState["BudgetExceeded"].ToString()));
                ucStatus.Text = "Purchase order is approved";
                ucStatus.Visible = true;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('approval','null',null);", true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected string PrepareApprovalText(DataTable dt, int approved)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataRow dr = dt.Rows[0];
        sbemailbody.Append("<pre>");
        sbemailbody.AppendLine();
        sbemailbody.Append("Dear Purchaser");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        if (approved == 1)
            sbemailbody.AppendLine("Purchase approval is cancelled.");
        else
            sbemailbody.AppendLine("Purchase order is approved");

        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Thank you,");
        sbemailbody.AppendLine();
        sbemailbody.Append(dr["FLDAPPROVEDBY"].ToString());
        sbemailbody.AppendLine();
       
        return sbemailbody.ToString();

    }
}
