using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using System.Data;
using SouthNests.Phoenix.Purchase;
using System.Web.UI;
using System.Text;
using Telerik.Web.UI;
public partial class PurchaseRemainingBudget : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            
            
            
            if (!IsPostBack)
            {
                ViewState["Title"] = "Approve";
                ViewState["BudgetExceeded"] = "0";
                if (!string.IsNullOrEmpty(Request.QueryString["quotationid"]))
                    ViewState["quotationid"] = Request.QueryString["quotationid"].ToString();

                if (!string.IsNullOrEmpty(Request.QueryString["type"]))
                    ViewState["type"] = Request.QueryString["type"].ToString();

                if (!string.IsNullOrEmpty(Request.QueryString["user"]))
                    ViewState["user"] = Request.QueryString["user"].ToString();

                if (!string.IsNullOrEmpty(Request.QueryString["currentorder"]))
                    ViewState["currentorder"] = Request.QueryString["currentorder"].ToString();

                if (!string.IsNullOrEmpty(Request.QueryString["directapprovalyn"]))
                    ViewState["directapprovalyn"] = Request.QueryString["directapprovalyn"].ToString();

                if (!string.IsNullOrEmpty(Request.QueryString["stocktype"]))
                    Filter.CurrentPurchaseStockType = Request.QueryString["stocktype"].ToString();

                if (!string.IsNullOrEmpty(Request.QueryString["vesselid"]))
                    Filter.CurrentPurchaseVesselSelection = int.Parse(Request.QueryString["vesselid"].ToString());

                GetAccount();

                if (ViewState["budgetid"]!=null && General.GetNullableInteger(ViewState["budgetid"].ToString())==null)
                {
                    ucError.ErrorMessage = "Budget codes not updated in Order Form.";
                    ucError.Visible = true;
                    return;
                }

                Edit();
            }
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            MenuFormFilter.Title = ViewState["Title"].ToString();
            toolbarmain.AddButton("Approve", "APPROVE", ToolBarDirection.Right);
            MenuFormFilter.MenuList = toolbarmain.Show();
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
        if (ViewState["quotationid"] != null && ViewState["quotationid"].ToString() != "")
        {
            dt = SouthNests.Phoenix.Purchase.PurchaseRemainingBudget.GetAccountid(new Guid(ViewState["quotationid"].ToString()));
            if (dt.Rows.Count>0)
            {
                ViewState["accountid"] = dt.Rows[0]["FLDACCOUNTID"].ToString();
                ViewState["budgetid"] = dt.Rows[0]["FLDBUDGETID"].ToString();
            }
        }
            
    }
    private void Edit()
    {
        if(General.GetNullableInteger(ViewState["budgetid"].ToString())!=null&& General.GetNullableInteger(ViewState["accountid"].ToString())!=null)
        {
            DataSet ds = SouthNests.Phoenix.Purchase.PurchaseRemainingBudget.RemainigBudget(int.Parse(ViewState["budgetid"].ToString()), int.Parse(ViewState["accountid"].ToString()), General.GetNullableDecimal(ViewState["currentorder"].ToString()));

            DataRow dr = ds.Tables[0].Rows[0];
            ViewState["Title"]= dr["FLDBUDGETGROUP"].ToString() + ": " + dr["FLDSUBACCOUNT"].ToString() + " " + dr["FLDSUBACCOUNTDESC"].ToString();
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
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("APPROVE"))
            {
                
             //if(ViewState["directapprovalyn"]!=null&& ViewState["directapprovalyn"].ToString() != ""&& ViewState["quotationid"]!=null&& ViewState["type"]!=null&& ViewState["user"]!=null)
             //   {
             //       if(ViewState["directapprovalyn"].ToString()=="Y")
             //       {
                        //DataTable dt1 = new DataTable();
                        //dt1 = PhoenixCommonApproval.InsertApprovalRecord(ViewState["quotationid"].ToString(), int.Parse(ViewState["type"].ToString()), General.GetNullableInteger(""), int.Parse("187")
                        //    , null, ViewState["user"].ToString(), General.GetNullableInteger(Filter.CurrentPurchaseVesselSelection.ToString()));
                        //string strApprovalID = dt1.Rows[0]["FLDAPPROVALID"].ToString();
                        PhoenixCommonPurchase.UpdateQuotationApproval(new Guid(ViewState["quotationid"].ToString()), int.Parse(ViewState["BudgetExceeded"].ToString()));
                        ucStatus.Text = "Approved";
                        ucStatus.Visible = true;
                        if (Session["POQAPPROVE"] != null && ((DataSet)Session["POQAPPROVE"]).Tables.Count > 0)
                        {
                            DataSet ds = (DataSet)Session["POQAPPROVE"];
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string emailbodytext = PrepareApprovalText(ds.Tables[0], 0);
                                DataRow dr = ds.Tables[0].Rows[0];
                                PhoenixMail.SendMail(dr["FLDEMAIL"].ToString().Replace(";", ",").TrimEnd(','),
                                    dr["FLDFROMEMAIL"].ToString().Replace(";", ",").TrimEnd(','),
                                    null,
                                    dr["FLDSUBJECT"].ToString() + "     " + dr["FLDFORMNO"].ToString(),
                                    emailbodytext,
                                    true,
                                    System.Net.Mail.MailPriority.Normal,
                                    "",
                                    null,
                                    null);
                            }
                            Session["POQAPPROVE"] = null;
                        }

                        RadScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "CloseWindow('Filter');fnReloadList('approval','filterandsearch',null);", true);
                    //}
                    //else
                    //{
                    //    //String scriptpopup = String.Format("javascript:Openpopup('approval', '', '../Common/CommonApproval.aspx?docid=" + ViewState["quotationid"].ToString() + "&mod=" + PhoenixModule.PURCHASE + "&type=" + ViewState["type"].ToString() + "&user=" + ViewState["user"].ToString()+ "');");
                    //    //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

                    //    Response.Redirect("../Common/CommonApproval.aspx?docid=" + ViewState["quotationid"].ToString() + "&mod=" + PhoenixModule.PURCHASE + "&type=" + ViewState["type"].ToString() + "&user=" + ViewState["user"].ToString());
                    //}
                //}
                
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
        sbemailbody.AppendLine("For and on behalf of Executive Ship Management Pte Ltd.");
        sbemailbody.AppendLine();

        return sbemailbody.ToString();

    }
}
