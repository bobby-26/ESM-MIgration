using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;



public partial class Accounts_AccountsCompanyFinancialStatement : PhoenixBasePage
{
   

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindCompany();            
        }
         RadWindowManager1.Visible = false;
        //ucConfirmMessage.Visible = false;

        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("Excluded Vouchers", "EXCLUDED", ToolBarDirection.Right);
        toolbar1.AddButton("Recalculate", "RECALCULATE", ToolBarDirection.Right);
        toolbar1.AddButton("Reset", "RESET", ToolBarDirection.Right);
        MenuFinancialYearStatement.Title = "Company Financial Statement";
        MenuFinancialYearStatement.AccessRights = this.ViewState;
        MenuFinancialYearStatement.MenuList = toolbar1.Show();     

    }

    private void BindCompany()
    {
        ddlCompany.DataTextField = "FLDCOMPANYNAME";
        ddlCompany.DataValueField = "FLDCOMPANYID";

        ddlCompany.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearCompanyList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string customerId = (e.Item.FindControl("hfCustomerId") as HiddenField).Value;
            Repeater rptOrders = e.Item.FindControl("rptOrders") as Repeater;
            rptOrders.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearTrailBalance(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue,int.Parse(ddlMonth.SelectedValue), 1, int.Parse(customerId));
            rptOrders.DataBind();
        }
    }
    
    protected void rptOrders_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string customerId = (e.Item.FindControl("hfCustomerId") as HiddenField).Value;            
            Repeater rptOrders = e.Item.FindControl("rptOrders1") as Repeater;
            rptOrders.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearTrailBalance(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), 2, int.Parse(customerId));
            rptOrders.DataBind();
            
        }
    }

    protected void rptOrders1_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string customerId = (e.Item.FindControl("hfCustomerId") as HiddenField).Value;
            Repeater rptOrders = e.Item.FindControl("rptTBlevel4") as Repeater;
            rptOrders.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearTrailBalance(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), 3, int.Parse(customerId));
            rptOrders.DataBind();

        }
    }


    protected void rptPnLBalancesheet_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string customerId = (e.Item.FindControl("hfCustomerId") as HiddenField).Value;
            Repeater rptPnLBalancesheet2nd = e.Item.FindControl("rptPnLBalancesheet2nd") as Repeater;
            if (ddlFormat.SelectedValue.Equals("Y"))
            {
                if (ddlReport.SelectedValue.Equals("B"))
                {
                    rptPnLBalancesheet2nd.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearBalanceSheet(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), 1, int.Parse(customerId));
                    rptPnLBalancesheet2nd.DataBind();
                }
                else
                {
                    rptPnLBalancesheet2nd.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearProfitAndLoss(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), 1, int.Parse(customerId));
                    rptPnLBalancesheet2nd.DataBind();
                }

            }
        }
    }

    protected void rptPnLBalancesheet2nd_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string customerId = (e.Item.FindControl("hfCustomerId") as HiddenField).Value;
            Repeater rptPnLBalancesheet3rd = e.Item.FindControl("rptPnLBalancesheet3rd") as Repeater;
            if (ddlFormat.SelectedValue.Equals("Y"))
            {
                if (ddlReport.SelectedValue.Equals("B"))
                {
                    rptPnLBalancesheet3rd.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearBalanceSheet(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), 2, int.Parse(customerId));
                    rptPnLBalancesheet3rd.DataBind();
                }
                else
                {
                    rptPnLBalancesheet3rd.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearProfitAndLoss(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), 2, int.Parse(customerId));
                    rptPnLBalancesheet3rd.DataBind();
                }
            }

        }
    }

    protected void rptPnLBalancesheet3rd_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string customerId = (e.Item.FindControl("hfCustomerId") as HiddenField).Value;
            Repeater rptPnLBalancesheet3rd = e.Item.FindControl("rptPnLBalancesheet4th") as Repeater;
            if (ddlFormat.SelectedValue.Equals("Y"))
            {
                if (ddlReport.SelectedValue.Equals("B"))
                {
                    rptPnLBalancesheet3rd.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearBalanceSheet(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), 3, int.Parse(customerId));
                    rptPnLBalancesheet3rd.DataBind();
                }
                else
                {
                    rptPnLBalancesheet3rd.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearProfitAndLoss(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), 3, int.Parse(customerId));
                    rptPnLBalancesheet3rd.DataBind();
                }
            }

        }
    }




    protected void rptTrailBalance_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string customerId = (e.Item.FindControl("hfCustomerId") as HiddenField).Value;
            Repeater rptTrailBalance = e.Item.FindControl("rptTrailBalance2nd") as Repeater;
            if (ViewState["FSList"] != null)
            {
                DataTable dtTB = (DataTable)ViewState["FSList"];
                DataTable dtTBM = dtTB.Clone();

                DataRow[] drR = dtTB.Select("FLDSORTLEVEL=2 AND FLDPARENTID="+customerId);

                foreach (DataRow dr in drR)
                {
                    dtTBM.ImportRow(dr);
                }

                rptTrailBalance.DataSource = dtTBM;
                rptTrailBalance.DataBind();
            }
            //rptTrailBalance.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearTrailBalanceMonthwise(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.Value, int.Parse(ddlMonth.SelectedValue), 1, int.Parse(customerId));                
            //rptTrailBalance.DataBind();
        }
    }

    protected void rptTrailBalance2nd_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string customerId = (e.Item.FindControl("hfCustomerId") as HiddenField).Value;
            Repeater rptTrailBalance = e.Item.FindControl("rptTrailBalance3rd") as Repeater;
            if (ViewState["FSList"] != null)
            {
                DataTable dtTB = (DataTable)ViewState["FSList"];
                DataTable dtTBM = dtTB.Clone();

                DataRow[] drR = dtTB.Select("FLDSORTLEVEL=3 AND FLDPARENTID="+customerId);

                foreach (DataRow dr in drR)
                {
                    dtTBM.ImportRow(dr);
                }

                rptTrailBalance.DataSource = dtTBM;
                rptTrailBalance.DataBind();
            }
            //rptTrailBalance.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearTrailBalanceMonthwise(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.Value, int.Parse(ddlMonth.SelectedValue),2, int.Parse(customerId));                
            //rptTrailBalance.DataBind();

        }
    }

    protected void rptTrailBalance3rd_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string customerId = (e.Item.FindControl("hfCustomerId") as HiddenField).Value;
            Repeater rptTrailBalance = e.Item.FindControl("rptTrailBalance4th") as Repeater;
            if (ViewState["FSList"] != null)
            {
                DataTable dtTB = (DataTable)ViewState["FSList"];
                DataTable dtTBM = dtTB.Clone();

                DataRow[] drR = dtTB.Select("FLDSORTLEVEL=4 AND FLDPARENTID ="+customerId);

                foreach (DataRow dr in drR)
                {
                    dtTBM.ImportRow(dr);
                }

                rptTrailBalance.DataSource = dtTBM;
                rptTrailBalance.DataBind();
            }
            //rptTrailBalance.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearTrailBalanceMonthwise(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.Value, int.Parse(ddlMonth.SelectedValue), 3, int.Parse(customerId));
            //rptTrailBalance.DataBind();

        }
    }



    protected void MenuFinancialYearStatement_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("RECALCULATE"))
            {         

                ControlVisibility();
                if (!IsValidData())
                {
                    ucError.Visible = true;
                    return;
                }

                DataSet dtCur = PhoenixRegistersCompany.EditCompany(int.Parse(ddlCompany.SelectedValue));
                if (dtCur.Tables.Count > 0)
                {
                    if (dtCur.Tables[0].Rows.Count > 0)
                    {
                        int currency = 0;
                        if (ddlCurrency.SelectedValue == "B")
                        {
                            currency = int.Parse(dtCur.Tables[0].Rows[0]["FLDBASECURRENCY"].ToString());                            
                        }
                        else
                        {
                            currency = int.Parse(dtCur.Tables[0].Rows[0]["FLDREPORTINGCURRENCY"].ToString());
                        }
                        DataSet dts = PhoenixRegistersCurrency.EditCurrency(1,currency);
                        if (dts.Tables.Count > 0)
                        {
                            lblCurrencycode.Text = dts.Tables[0].Rows[0]["FLDCURRENCYCODE"].ToString();
                        }
                         
                    }
                }

                

                DataTable dt = null;
                dt = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearOpeningExistYN(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), int.Parse(ddlMonth.SelectedValue));

                
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() == "1")
                        {
                        //ucConfirmMessage.Visible = true;
                        //ucConfirmMessage.Text = "Opening Balances has not been transferred for the Selected Financial Year.Please confirm if you still want to see the report.";
                        //return;
                        RadWindowManager1.Visible = true;
                        RadWindowManager1.RadConfirm("Opening Balances has not been transferred for the Selected Financial Year.Please confirm if you still want to see the report.", "confirm", 400, 250, null, "Confirm");
                        return;
                    }
                    }
                

                dt = null;
                dt = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearOutOfBalanceExistYN(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), int.Parse(ddlMonth.SelectedValue));                

                
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() == "1")
                        {
                            //ucConfirmMessage.Visible = true;
                            //ucConfirmMessage.Text = "Some vouchers are not yet Balanced.Please see Excluded vouchers.Please confirm if you still want to see the Report.";
                            //return;
                        RadWindowManager1.Visible = true;
                        RadWindowManager1.RadConfirm("Some vouchers are not yet Balanced.Please see Excluded vouchers.Please confirm if you still want to see the Report.", "confirm", 400, 250, null, "Confirm");
                        return;
                    }
                    }

                    lblAccuratedate.Text = System.DateTime.UtcNow.ToString();

                if (ddlReport.SelectedValue.Equals("T") && ddlFormat.SelectedValue.Equals("Y"))
                {
                    rptCustomers.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearTrailBalance(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), 0, null);
                    rptCustomers.DataBind();
                }
                if (ddlReport.SelectedValue.Equals("T") && ddlFormat.SelectedValue.Equals("M"))
                {
                    DataSet ds = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearTrailBalanceMonthwise(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), 0, null);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ViewState["FSList"] = ds.Tables[0];
                        }
                    }
                    if (ViewState["FSList"] != null)
                    {
                        DataTable dtTB = (DataTable)ViewState["FSList"];
                        DataTable dtTBM = dtTB.Clone();

                        DataRow[] drR = dtTB.Select("FLDSORTLEVEL=1 AND FLDPARENTID IS NULL");

                        foreach (DataRow dr in drR)
                        {
                            dtTBM.ImportRow(dr);
                        }

                        rptTrailBalance.DataSource = dtTBM;
                        rptTrailBalance.DataBind();                        
                    }                    
                }
                if (ddlReport.SelectedValue.Equals("B") && ddlFormat.SelectedValue.Equals("Y"))
                {
                    rptPnLBalancesheet.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearBalanceSheet(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), 0, null);
                    rptPnLBalancesheet.DataBind();
                }
                if (ddlReport.SelectedValue.Equals("B") && ddlFormat.SelectedValue.Equals("M"))
                {
                    DataSet ds = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearBalanceSheetMonthwise(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), 0, null);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ViewState["FSList"] = ds.Tables[0];
                        }
                    }
                    if (ViewState["FSList"] != null)
                    {
                        DataTable dtTB = (DataTable)ViewState["FSList"];
                        DataTable dtTBM = dtTB.Clone();

                        DataRow[] drR = dtTB.Select("FLDSORTLEVEL=1 AND FLDPARENTID IS NULL");

                        foreach (DataRow dr in drR)
                        {
                            dtTBM.ImportRow(dr);
                        }

                        rptMnthPnLBS.DataSource = dtTBM;
                        rptMnthPnLBS.DataBind();
                    }
                    //rptMnthPnLBS.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearBalanceSheetMonthwise(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.Value, int.Parse(ddlMonth.SelectedValue), 0, null);
                    //rptMnthPnLBS.DataBind();
                }
                if (ddlReport.SelectedValue.Equals("P") && ddlFormat.SelectedValue.Equals("Y"))
                {
                    rptPnLBalancesheet.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearProfitAndLoss(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), 0, null);
                    rptPnLBalancesheet.DataBind();
                }
                if (ddlReport.SelectedValue.Equals("P") && ddlFormat.SelectedValue.Equals("M"))
                {
                    rptMnthPnLBS.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearProfitAndLossMonthwise(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), 0, null);
                    rptMnthPnLBS.DataBind();
                }
            }
            if (CommandName.ToUpper().Equals("RESET"))
            {
                Reset();
                ControlVisibility();
            }
            if (CommandName.ToUpper().Equals("EXCLUDED"))
            {
                ControlVisibility();
                if (!IsValidData())
                {
                    ucError.Visible = true;
                    return;
                }
                
                gvExcludedVouchers.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearExcludedVouchers(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), int.Parse(ddlMonth.SelectedValue));
                gvExcludedVouchers.DataBind();
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected bool IsValidData()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ddlCompany.SelectedValue == "Dummy")
            ucError.ErrorMessage = "Company is required";
        if (ucFinancialYear.SelectedQuick == "Dummy")
            ucError.ErrorMessage = "Financial Year is required";
        if (ddlMonth.SelectedValue == "Dummy")
            ucError.ErrorMessage = "Period is required";

        return (!ucError.IsError);
    }

    protected void rptMnthPnLBS_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string customerId = (e.Item.FindControl("hfCustomerId") as HiddenField).Value;
            Repeater rptProfitNLoss = e.Item.FindControl("rptMnthPnLBS2nd") as Repeater;
            if (ddlReport.SelectedValue.Equals("B"))
            {
                if (ViewState["FSList"] != null)
                {
                    DataTable dtTB = (DataTable)ViewState["FSList"];
                    DataTable dtTBM = dtTB.Clone();

                    DataRow[] drR = dtTB.Select("FLDSORTLEVEL=2 AND FLDPARENTID=" + customerId);

                    foreach (DataRow dr in drR)
                    {
                        dtTBM.ImportRow(dr);
                    }                   

                    rptProfitNLoss.DataSource = dtTBM;
                    rptProfitNLoss.DataBind();                   
                }
                //rptProfitNLoss.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearBalanceSheetMonthwise(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.Value, int.Parse(ddlMonth.SelectedValue), 1, int.Parse(customerId));
                //rptProfitNLoss.DataBind();
            }
            else
            {
                rptProfitNLoss.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearProfitAndLossMonthwise(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), 1, int.Parse(customerId));
                rptProfitNLoss.DataBind();
            }
            
        }
    }

    protected void rptMnthPnLBS2nd_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string customerId = (e.Item.FindControl("hfCustomerId") as HiddenField).Value;
            Repeater rptProfitNLoss = e.Item.FindControl("rptMnthPnLBS3rd") as Repeater;
            if (ddlReport.SelectedValue.Equals("B"))
            {
                if (ViewState["FSList"] != null)
                {
                    DataTable dtTB = (DataTable)ViewState["FSList"];
                    DataTable dtTBM = dtTB.Clone();

                    DataRow[] drR = dtTB.Select("FLDSORTLEVEL=3 AND FLDPARENTID=" + customerId);

                    foreach (DataRow dr in drR)
                    {
                        dtTBM.ImportRow(dr);
                    }

                    rptProfitNLoss.DataSource = dtTBM;
                    rptProfitNLoss.DataBind();
                }
                //rptProfitNLoss.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearBalanceSheetMonthwise(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.Value, int.Parse(ddlMonth.SelectedValue), 2, int.Parse(customerId));
                //rptProfitNLoss.DataBind();
            }
            else
            {
                rptProfitNLoss.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearProfitAndLossMonthwise(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), 2, int.Parse(customerId));
                rptProfitNLoss.DataBind();
            }
            

        }
    }

    protected void rptMnthPnLBS3rd_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string customerId = (e.Item.FindControl("hfCustomerId") as HiddenField).Value;
            Repeater rptProfitNLoss = e.Item.FindControl("rptMnthPnLBS4th") as Repeater;
            if (ddlReport.SelectedValue.Equals("B"))
            {
                if (ViewState["FSList"] != null)
                {
                    DataTable dtTB = (DataTable)ViewState["FSList"];
                    DataTable dtTBM = dtTB.Clone();

                    DataRow[] drR = dtTB.Select("FLDSORTLEVEL=4 AND FLDPARENTID =" + customerId);

                    foreach (DataRow dr in drR)
                    {
                        dtTBM.ImportRow(dr);
                    }

                    rptProfitNLoss.DataSource = dtTBM;
                    rptProfitNLoss.DataBind();
                }
                //rptProfitNLoss.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearBalanceSheetMonthwise(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.Value, int.Parse(ddlMonth.SelectedValue), 3, int.Parse(customerId));
                //rptProfitNLoss.DataBind();
            }
            else
            {
                rptProfitNLoss.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearProfitAndLossMonthwise(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), 3, int.Parse(customerId));
                rptProfitNLoss.DataBind();
            }


        }
    }




    private void ControlVisibility()
    {
        rptCustomers.DataSource = null;
        rptCustomers.DataBind();

        rptPnLBalancesheet.DataSource = null;
        rptPnLBalancesheet.DataBind();

        rptTrailBalance.DataSource = null;
        rptTrailBalance.DataBind();

        rptMnthPnLBS.DataSource = null;
        rptMnthPnLBS.DataBind();

        gvExcludedVouchers.DataSource = null;
        gvExcludedVouchers.DataBind();
    }

    private void Reset()
    {
        ddlCompany.SelectedIndex = 0;
        ucFinancialYear.SelectedText = "--Select--";
        ddlReport.SelectedIndex = 0;
        ddlMonth.SelectedIndex = 0;
        ddlFormat.SelectedIndex = 0;
        ddlCurrency.SelectedIndex = 0;
        lblAccuratedate.Text = string.Empty;
        ddlMonth.DataSource = null;
        ddlMonth.DataBind();
        lblCurrencycode.Text = "";
        ViewState["FSList"] = null;
    }

    protected void OnAction_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
            if (ucCM.confirmboxvalue == 1)
            {
                lblAccuratedate.Text = System.DateTime.UtcNow.ToString();
                if (ddlReport.SelectedValue.Equals("T") && ddlFormat.SelectedValue.Equals("Y"))
                {
                    rptCustomers.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearTrailBalance(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), 0, null);
                    rptCustomers.DataBind();
                }
                if (ddlReport.SelectedValue.Equals("T") && ddlFormat.SelectedValue.Equals("M"))
                {
                    DataSet ds = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearTrailBalanceMonthwise(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), 0, null);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ViewState["FSList"] = ds.Tables[0];
                        }
                    }
                    if (ViewState["FSList"] != null)
                    {
                        DataTable dtTB = (DataTable)ViewState["FSList"];
                        DataTable dtTBM = dtTB.Clone();

                        DataRow[] drR = dtTB.Select("FLDSORTLEVEL=1");

                        foreach (DataRow dr in drR)
                        {
                            dtTBM.ImportRow(dr);
                        }

                        rptTrailBalance.DataSource = dtTBM;
                        rptTrailBalance.DataBind();
                    }
                    //rptTrailBalance.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearTrailBalanceMonthwise(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.Value, int.Parse(ddlMonth.SelectedValue), 0, null);
                    //rptTrailBalance.DataBind();
                }
                if (ddlReport.SelectedValue.Equals("B") && ddlFormat.SelectedValue.Equals("Y"))
                {
                    rptPnLBalancesheet.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearBalanceSheet(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), 0, null);
                    rptPnLBalancesheet.DataBind();
                }
                if (ddlReport.SelectedValue.Equals("B") && ddlFormat.SelectedValue.Equals("M"))
                {
                    DataSet ds = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearBalanceSheetMonthwise(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), 0, null);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ViewState["FSList"] = ds.Tables[0];
                        }
                    }
                    if (ViewState["FSList"] != null)
                    {
                        DataTable dtTB = (DataTable)ViewState["FSList"];
                        DataTable dtTBM = dtTB.Clone();

                        DataRow[] drR = dtTB.Select("FLDSORTLEVEL=1 AND FLDPARENTID IS NULL");

                        foreach (DataRow dr in drR)
                        {
                            dtTBM.ImportRow(dr);
                        }

                        rptMnthPnLBS.DataSource = dtTBM;
                        rptMnthPnLBS.DataBind();
                    }
                    //rptMnthPnLBS.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearBalanceSheetMonthwise(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.Value, int.Parse(ddlMonth.SelectedValue), 0, null);
                    //rptMnthPnLBS.DataBind();
                }
                if (ddlReport.SelectedValue.Equals("P") && ddlFormat.SelectedValue.Equals("Y"))
                {
                    rptPnLBalancesheet.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearProfitAndLoss(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), 0, null);
                    rptPnLBalancesheet.DataBind();
                }
                if (ddlReport.SelectedValue.Equals("P") && ddlFormat.SelectedValue.Equals("M"))
                {
                    rptMnthPnLBS.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearProfitAndLossMonthwise(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), 0, null);
                    rptMnthPnLBS.DataBind();
                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ShowExcel(int Level)
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        //int iTotalPageCount = 0;
        string[] alCaptions = null;
        string[] alColumns = null;

        if (ddlReport.SelectedValue.Equals("T") && ddlFormat.SelectedValue.Equals("Y"))
        {
            alCaptions = new string[] {"Account","Opening Balance","YTD ("+lblCurrencycode.Text+")","Closing Balance"};

            alColumns = new string[] {"FLDDESCRIPTION","FLDOPENINGBALANCE","FLDBALANCE","FLDCLOSINGBALANCE"};
        }

        if (ddlReport.SelectedValue.Equals("T") && ddlFormat.SelectedValue.Equals("M"))
        {
            alCaptions = new string[] { "Account", "Opening Balance", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", "Closing Balance" };

            alColumns = new string[] { "FLDDESCRIPTION", "FLDOPENINGBALANCE", "FLDJANUARY", "FLDFEBRUARY", "FLDMARCH", "FLDAPRIL", "FLDMAY", "FLDJUNE", "FLDJULY", "FLDAUGUST", "FLDSEPTEMBER", "FLDOCTOBER", "FLDNOVEMBER", "FLDDECEMBER", "FLDCLOSINGBALANCE" };
        }

        if (!ddlReport.SelectedValue.Equals("T") && ddlFormat.SelectedValue.Equals("Y"))
        {
            alCaptions = new string[] { "Account", "YTD ("+lblCurrencycode.Text+")" };

            alColumns = new string[] { "FLDDESCRIPTION", "FLDBALANCE"};
        }

        if (!ddlReport.SelectedValue.Equals("T") && ddlFormat.SelectedValue.Equals("M"))
        {
            alCaptions = new string[] { "Account", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            alColumns = new string[] { "FLDDESCRIPTION",  "FLDJANUARY", "FLDFEBRUARY", "FLDMARCH", "FLDAPRIL", "FLDMAY", "FLDJUNE", "FLDJULY", "FLDAUGUST", "FLDSEPTEMBER", "FLDOCTOBER", "FLDNOVEMBER", "FLDDECEMBER" };
        }

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (ddlReport.SelectedValue.Equals("T") && ddlFormat.SelectedValue.Equals("Y"))
            ds = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearTrailBalance(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), Level, null);            
        if (ddlReport.SelectedValue.Equals("T") && ddlFormat.SelectedValue.Equals("M"))
            ds = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearTrailBalanceMonthwise(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), Level, null);
        if (ddlReport.SelectedValue.Equals("B") && ddlFormat.SelectedValue.Equals("Y"))
            ds = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearBalanceSheet(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), Level, null);            
        if (ddlReport.SelectedValue.Equals("B") && ddlFormat.SelectedValue.Equals("M"))
            ds = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearBalanceSheetMonthwise (int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), Level, null);
        if (ddlReport.SelectedValue.Equals("P") && ddlFormat.SelectedValue.Equals("Y"))
            ds = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearProfitAndLoss(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), Level, null);
        if (ddlReport.SelectedValue.Equals("P") && ddlFormat.SelectedValue.Equals("M"))
            ds = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearProfitAndLossMonthwise(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick), ddlCurrency.SelectedValue, int.Parse(ddlMonth.SelectedValue), Level, null);

        //NameValueCollection nvc = Filter.CurrentInvoiceSelection;
        //ds = PhoenixAccountsVoucher.VoucherSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, 74
        //                                           , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherNumberSearch")) : string.Empty
        //                                           , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherFromdateSearch")) : null
        //                                           , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherTodateSearch")) : null
        //                                           , null
        //                                           , nvc != null ? General.GetNullableInteger(nvc.Get("ucInvoiceStatus").ToString()) : null
        //                                           , nvc != null ? General.GetNullableString(nvc.Get("txtReferenceNumberSearch")) : string.Empty
        //                                           , null
        //                                           , sortexpression, sortdirection
        //                                           , (int)ViewState["PAGENUMBER"]
        //                                           , iRowCount
        //                                           , ref iRowCount, ref iTotalPageCount
        //                                           , nvc != null ? General.GetNullableString(nvc.Get("txtLongDescription")) : string.Empty
        //                                           );



        Response.AddHeader("Content-Disposition", "attachment; filename=CompanyFinancialStatement.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Company Financial Statement</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void imgExcel_OnClientClick(object sender, EventArgs e)
    {
        if (!IsValidData())
        {
            ucError.Visible = true;
            return;
        } 
        ShowExcel(0);
    }

    protected void imgExcel2nd_OnClientClick(object sender, EventArgs e)
    {
        if (!IsValidData())
        {
            ucError.Visible = true;
            return;
        }
        ShowExcel(1);
    }

    protected void imgExcel3rd_OnClientClick(object sender, EventArgs e)
    {
        if (!IsValidData())
        {
            ucError.Visible = true;
            return;
        }
        ShowExcel(2);
    }

    protected void imgExcel4th_OnClientClick(object sender, EventArgs e)
    {
        if (!IsValidData())
        {
            ucError.Visible = true;
            return;
        }
        ShowExcel(3);
    }

    protected void ucFinancialYear_OnTextChangedEvent(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        if (ddlCompany.SelectedIndex > 0 && General.GetNullableInteger(ucFinancialYear.SelectedQuick) != null)
        {
            ds = PhoenixAccountsPeriodLock.PeriodLockSearch(int.Parse(ddlCompany.SelectedValue), int.Parse(ucFinancialYear.SelectedQuick));
            ddlMonth.DataTextField = "FLDHARDNAME";
            ddlMonth.DataValueField = "FLDHARDCODE";
            ddlMonth.DataSource = ds;
            ddlMonth.DataBind();
            ddlMonth.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        }
        else
        {
            ddlMonth.DataSource = null;
            ddlMonth.DataBind();
        }
       
    }

}
