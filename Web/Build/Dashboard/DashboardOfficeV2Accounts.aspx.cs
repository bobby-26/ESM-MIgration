using System;
using System.Data;
using SouthNests.Phoenix.Dashboard;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class Dashboard_DashboardOfficeV2Accounts : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //ddlSubtype.DataBind();
            //ddlFund7days.DataBind();
            ViewState["ddlMonth"] = (DateTime.Today.Month ).ToString();
            ViewState["ddlMonthJV"] = (DateTime.Today.Month ).ToString();
            ViewState["ddlsoamonth"] = (DateTime.Today.Month).ToString();

            if (ViewState["ddlMonth"].ToString() == "1")
            {
                ddlYear.SelectedYear = DateTime.Today.Year - 1;
                ddlMonth.SelectedMonth = "12";
            }
            else
            {
                ddlYear.SelectedYear = DateTime.Today.Year;
                ddlMonth.SelectedMonth = (DateTime.Today.Month - 1).ToString();
            }

            if (ViewState["ddlMonthJV"].ToString() == "1")
            {
                ddlYearJV.SelectedYear = DateTime.Today.Year - 1;
                ddlMonthJV.SelectedMonth = "12";
            }
            else
            {
                ddlYearJV.SelectedYear = DateTime.Today.Year;
                ddlMonthJV.SelectedMonth = (DateTime.Today.Month - 1).ToString();
            }
            if (ViewState["ddlsoamonth"].ToString() == "1")
            {
                ddlsoayear.SelectedYear = DateTime.Today.Year - 1;
                ddlsoamonth.SelectedMonth = "12";
            }
            else
            {
                ddlsoayear.SelectedYear = DateTime.Today.Year;
                ddlsoamonth.SelectedMonth = (DateTime.Today.Month - 1).ToString();
            }
            //ddlMonthUnbill.SelectedMonth = (DateTime.Today.Month - 1).ToString();
            //ddlYearUnbill.SelectedYear = DateTime.Today.Year;
            
            idsoareport.Attributes["class"] = "active";
            tab1.Attributes["class"] = "tab-pane active";
           // Techtab1.Attributes["class"] = "tab-pane active";
            //  Directtab2.Attributes["class"] = "tab-pane active";
        }
        

        Page.MaintainScrollPositionOnPostBack = true;

    }
    protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.ToUpper() == "SOAOTHERS")
        {
            BindFundReceivedData();
            BindPhoneCard();
            BindOutStandingFundData();
            BindOutStandingTotal();
            BindAllotNYP();
            BindUnallFundRcpt();
        }
        if (e.Argument.ToUpper() == "AP")
        {
            BindGeneratePMV();
            BindInvoiceParApp();
            BindAdvanceAccChecking();
            BindInvoiceAccountChecking();
            BindInvoiceAdvPMV();
            BindPOAdvFollowUp();
            BindPOADPMVNotGen();
            BindRebatesFlw();
            BindSluggishInvoice();
        }
        if (e.Argument.ToUpper() == "REMITTANCE")
        {
            BindRMTNotGen();
            BindApprovedPMVRMTnotGen();
            BindApprovedAllotM();
            BindRemittanceCount();
        }
        if (e.Argument.ToUpper() == "FUNDSFLOW")
        {
            BindPendingFR();
            BindBankBalance();
            BindEstimatedFU();
            BindApprovedSupplPMVs();
        }
        if (e.Argument.ToUpper() == "CTMCASH")
        {
            BindLocalCP();
            BindCashUSD();
            BindCashSGD();
            BindCashMYR();
            BindCTMArrng();
            BindTravelAdv();
            BindArranCTMNOTRev();
        }
    }
    private void BindOpenDryPOs()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardOpenDrydockingPOsList();
        //gvOpenDryPOs.DataSource = ds.Tables[0];
    }
    private void BindAllotNYP()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardAllotmentNotYetProcessedList();
        //gvAllotNYP.DataSource = ds.Tables[0];
        //gvAllotNYP.DataBind();
    }
    private void BindFundReceivedData()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardFundReceivedList(General.GetNullableInteger(ddlFund7days.SelectedValue));
        //gvFundReceived.DataSource = ds.Tables[0];
        //gvFundReceived.DataBind();
    }
    private void BindUnbilledEntr()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardUnbilledEntriesList(General.GetNullableInteger(ddlYearUnbill.SelectedYear.ToString())
        //                                                                       , General.GetNullableInteger(ddlMonthUnbill.SelectedMonth)
        //                                                                        , null);
        //gvUnbilledEntr.DataSource = ds.Tables[0];
    }
    private void BindUnbilledEntrDDlBind()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardUnbilledEntriesList(General.GetNullableInteger(ddlMonthUnbill.SelectedMonth)
        //                                                                        , General.GetNullableInteger(ddlYearUnbill.SelectedYear.ToString())
        //                                                                        , null);
        //gvUnbilledEntr.DataSource = ds.Tables[0];
        //gvUnbilledEntr.DataBind();
    }
    private void BindOutStandingFundData()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardOutstandingVesselFundRequestdetails(General.GetNullableInteger(ddlSubtype.SelectedValue));
        //gvOutstandingFund.DataSource = ds.Tables[0];
        //gvOutstandingFund.DataBind();
    }
    private void BindSOAPendingList()
    {
        DataSet ds = PhoenixDashboardAccounts.DashboardSOAPublishPendingList(General.GetNullableInteger(ddlsoayear.SelectedYear.ToString())
                                                                                , General.GetNullableInteger(ddlsoamonth.SelectedMonth.ToString()));
        gvSOAPendingList.DataSource = ds.Tables[0];
    }
    private void BindSOAPendingListddlBind()
    {
        DataSet ds = PhoenixDashboardAccounts.DashboardSOAPublishPendingList(General.GetNullableInteger(ddlsoayear.SelectedYear.ToString())
                                                                                , General.GetNullableInteger(ddlsoamonth.SelectedMonth.ToString()));
        gvSOAPendingList.DataSource = ds.Tables[0];
        gvSOAPendingList.DataBind();
    }

    protected void gvPortage_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindPortage();
    }
    protected void gvMonthlyJV_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindMonthlyJV();
    }
    protected void gvSOAPendingList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindSOAPendingList();
    }

    //protected void gvOldpendingPOS_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    //{
    //    BindOldPendingPOs();
    //}
    private void BindInvoiceAdvPMV()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardInvoicePMVAgeinglist();
        //gvInvoiceAdvPMV.DataSource = ds.Tables[0];
        //gvInvoiceAdvPMV.DataBind();
    }
    private void BindInvoiceAccountChecking()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardInvoiceAccountCheckingcountlist();
        //gvInvoiceAccChecking.DataSource = ds.Tables[0];
        //gvInvoiceAccChecking.DataBind();
    }
    private void BindOutStandingTotal()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardOutstandingFundRequestTotalDetail();
        //gvOutstandingTotal.DataSource = ds.Tables[0];
        //gvOutstandingTotal.DataBind();
    }
    private void BindAdvanceAccChecking()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardPODepositAccountCheckingcountlist();
        //gvAdvanceAccChecking.DataSource = ds.Tables[0];
        //gvAdvanceAccChecking.DataBind();
    }
    private void BindGeneratePMV()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardGeneratePMVcountlist(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        //gvGeneratePMV.DataSource = ds.Tables[0];
        //gvGeneratePMV.DataBind();
    }
    private void BindInvoiceParApp()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardInvoicePMVPartiallyApprovedCountList();
        //gvInvoiceParApp.DataSource = ds.Tables[0];
        //gvInvoiceParApp.DataBind();
    }
    private void BindPhoneCard()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardPhoneCardsNotYetArrangedList();
        //gvPhoneCard.DataSource = ds.Tables[0];
        //gvPhoneCard.DataBind();
    }
    private void BindPortage()
    {
        DataSet ds = PhoenixDashboardAccounts.DashboardPortageBillNotYetFinalized(General.GetNullableInteger(ddlYear.SelectedYear.ToString())
                                                                                , General.GetNullableInteger(ddlMonth.SelectedMonth.ToString()));
        gvPortage.DataSource = ds.Tables[0];
    }
    private void BindPortageddlBind()
    {
        DataSet ds = PhoenixDashboardAccounts.DashboardPortageBillNotYetFinalized(General.GetNullableInteger(ddlYear.SelectedYear.ToString())
                                                                                , General.GetNullableInteger(ddlMonth.SelectedMonth.ToString()));
        gvPortage.DataSource = ds.Tables[0];
        gvPortage.DataBind();
    }
    private void BindRMTNotGen()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardRemittanceNotGenerated();
        //gvRMTNotGen.DataSource = ds.Tables[0];
        //gvRMTNotGen.DataBind();
    }
    private void BindApprovedPMVRMTnotGen()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardRemittanceNotGeneratedAgeingList();
        //gvApprovedPMVRMTnotGen.DataSource = ds.Tables[0];
        //gvApprovedPMVRMTnotGen.DataBind();
    }
    private void BindApprovedAllotM()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardRemittanceNotApprovedGeneratedAgeingList();
        //gvApprovedAllotM.DataSource = ds.Tables[0];
        //gvApprovedAllotM.DataBind();
    }
    private void BindRemittanceCount()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardRemittanceCountList();
        //gvRemittanceCount.DataSource = ds.Tables[0];
        //gvRemittanceCount.DataBind();
    }
    private void BindEstimatedFU()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardEstimatedFirmOutflowsList();
        //gvEstimatedFU.DataSource = ds.Tables[0];
        //gvEstimatedFU.DataBind();
    }
    private void BindApprovedSupplPMVs()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardApprovedSupplierPMVSList();
        //gvApprovedSupplPMVs.DataSource = ds.Tables[0];
        //gvApprovedSupplPMVs.DataBind();
    }
    private void BindCashUSD()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardCashBalancesList("USD");
        //gvCashUSD.DataSource = ds.Tables[0];
        //gvCashUSD.DataBind();
    }
    private void BindCashSGD()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardCashBalancesList("SGD");
        //gvCashSGD.DataSource = ds.Tables[0];
        //gvCashSGD.DataBind();
    }
    private void BindCashMYR()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardCashBalancesList("MYR");
        //gvCashMYR.DataSource = ds.Tables[0];
        //gvCashMYR.DataBind();
    }
    private void BindLocalCP()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardLocalClaimPostingList();
        //gvLocalCP.DataSource = ds.Tables[0];
        //gvLocalCP.DataBind();
    }
    private void BindCTMArrng()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = PhoenixDashboardAccounts.DashboardCTMtobeArrangedList(gvCTMArrng.CurrentPageIndex + 1,
            gvCTMArrng.PageSize,
            ref iRowCount,
            ref iTotalPageCount);
        gvCTMArrng.DataSource = ds.Tables[0];
        gvCTMArrng.DataBind();
    }
    private void BindTravelAdv()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardTravelAdvancestobePaidList();
        //gvTravelAdv.DataSource = ds.Tables[0];
        //gvTravelAdv.DataBind();
    }
    //protected void ddlSubtype_DataBound(object sender, EventArgs e)
    //{
    //    ddlSubtype.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    //}
    private void BindMonthlyJV()
    {
        DataSet ds = PhoenixDashboardAccounts.DashboardMonthlyJVNotYetPosted(General.GetNullableInteger(ddlYearJV.SelectedYear.ToString())
                                                                           , General.GetNullableInteger(ddlMonthJV.SelectedMonth.ToString()));
        gvMonthlyJV.DataSource = ds.Tables[0];
    }
    private void BindMonthlyJVddlBind()
    {
        DataSet ds = PhoenixDashboardAccounts.DashboardMonthlyJVNotYetPosted(General.GetNullableInteger(ddlYearJV.SelectedYear.ToString())
                                                                           , General.GetNullableInteger(ddlMonthJV.SelectedMonth.ToString()));
        gvMonthlyJV.DataSource = ds.Tables[0];
        gvMonthlyJV.DataBind();
    }
    private void BindOpenTechPOs()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardOpenTechnicalPOsList();
        //gvOpenTechPOs.DataSource = ds.Tables[0];
    }
    private void BindDirectPOs()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardOpenDirectPOsList();
        //gvDirectPOs.DataSource = ds.Tables[0];
    }
    private void BindArranCTMNOTRev()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = PhoenixDashboardAccounts.DashboardArrangedCTMNotYetReceivedList(gvArranCTMNOTRev.CurrentPageIndex + 1,
            gvArranCTMNOTRev.PageSize,
            ref iRowCount,
            ref iTotalPageCount);
        gvArranCTMNOTRev.DataSource = ds.Tables[0];
        gvArranCTMNOTRev.DataBind();
    }
    private void BindRebatesFlw()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardRebatesFollowupList();
        //gvRebatesFlw.DataSource = ds.Tables[0];
        //gvRebatesFlw.DataBind();
    }
    private void BindSluggishInvoice()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardSluggishInvoicesList();
        //gvSluggishInvoice.DataSource = ds.Tables[0];
        //gvSluggishInvoice.DataBind();
    }
    private void BindPOADPMVNotGen()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardPOAdvanceDepositPMVRMTCASNotGeneratedList();
        //gvPOADPMVNotGen.DataSource = ds.Tables[0];
        //gvPOADPMVNotGen.DataBind();
    }
    private void BindPOAdvFollowUp()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardPOAdvancesFollowupUSDList();
        //gvPOAdvFollowUp.DataSource = ds.Tables[0];
        //gvPOAdvFollowUp.DataBind();
    }
    private void BindUnallFundRcpt()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardUnallocatedFundsReceiptList();
        //gvUnallFundRcpt.DataSource = ds.Tables[0];
        //gvUnallFundRcpt.DataBind();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvSOAPendingList.Rebind();
        gvPortage.Rebind();
        gvMonthlyJV.Rebind();
    }
    protected void ddlMonthSOA_TextChangedEvent(object sender, EventArgs e)
    {
        BindSOAPendingListddlBind();
        //BindPortage();
        //BindMonthlyJV();
        idsoareport.Attributes["class"] = "active";
        tab1.Attributes["class"] = "tab-pane active";
      //  idsoaother.Attributes["class"] = "";
        tab2.Attributes["class"] = "tab-pane";
    }
    protected void ddlYearSOA_TextChangedEvent(object sender, EventArgs e)
    {
        BindSOAPendingListddlBind();
        //BindPortage();
        //BindMonthlyJV();
        idsoareport.Attributes["class"] = "active";
        tab1.Attributes["class"] = "tab-pane active";
      //  idsoaother.Attributes["class"] = "";
        tab2.Attributes["class"] = "tab-pane";
    }
    protected void ddlYear_TextChangedEvent(object sender, EventArgs e)
    {
        //BindSOAPendingList();
        BindPortageddlBind();
        //BindMonthlyJV();
        idsoareport.Attributes["class"] = "active";
        tab1.Attributes["class"] = "tab-pane active";
       // idsoaother.Attributes["class"] = "";
        tab2.Attributes["class"] = "tab-pane";
    }
    protected void ddlMonth_TextChangedEvent(object sender, EventArgs e)
    {
        //  BindSOAPendingList();
        BindPortageddlBind();
        //BindMonthlyJV();
        idsoareport.Attributes["class"] = "active";
        tab1.Attributes["class"] = "tab-pane active";
       // idsoaother.Attributes["class"] = "";
        tab2.Attributes["class"] = "tab-pane";
    }
    protected void ddlMonthJV_TextChangedEvent(object sender, EventArgs e)
    {
        //BindSOAPendingList();
        //BindPortage();
        BindMonthlyJVddlBind();
        idsoareport.Attributes["class"] = "active";
        tab1.Attributes["class"] = "tab-pane active";
      //  idsoaother.Attributes["class"] = "";
        tab2.Attributes["class"] = "tab-pane";
    }
    protected void ddlYearJV_TextChangedEvent(object sender, EventArgs e)
    {
        //BindSOAPendingList();
        //BindPortage();
        BindMonthlyJVddlBind();
        idsoareport.Attributes["class"] = "active";
        tab1.Attributes["class"] = "tab-pane active";
       // idsoaother.Attributes["class"] = "";
        tab2.Attributes["class"] = "tab-pane";
    }
    protected void ddlSubtype_TextChanged(object sender, EventArgs e)
    {
        BindOutStandingFundData();
      //  idsoaother.Attributes["class"] = "active";
        idsoareport.Attributes["class"] = "";
        tab2.Attributes["class"] = "tab-pane active";
        tab1.Attributes["class"] = "tab-pane";
    }
    protected void BtnCrew_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Dashboard/DashboardOfficeV2Crew.aspx", false);
    }
    protected void btnHSQEA_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Dashboard/DashboardOfficeV2.aspx", true);
    }
    protected void BtnTech_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Dashboard/DashboardOfficeV2.aspx?type=t", true);
    }
    protected void gvPortage_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblvesselid");
            string url = "";
            if (e.CommandName == "SENDMAIL")
            {
                if (lblvesselid != null)
                {
                    url = String.Format(
                       "javascript:parent.openNewWindow('codehelp1', 'Crew List', 'Accounts/AccountsPortageBillfollowupMail.aspx?vesselid=" + lblvesselid.Text + "&Month=" + ddlMonth.SelectedMonth.ToString() + "&Year=" + ddlYear.SelectedYear.ToString() + "');");

                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", url, true);
                }
            }
            if (e.CommandName == "HISTORY")
            {
                if (lblvesselid != null)
                {
                    url = String.Format(
                       "javascript:parent.openNewWindow('codehelp1', 'Crew List', 'Accounts/AccountsDashboardPortageBillViewHistory.aspx?vesselid=" + lblvesselid.Text + "&Month=" + ddlMonth.SelectedMonth.ToString() + "&Year=" + ddlYear.SelectedYear.ToString() + "');");

                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", url, true);
                }
            }
        }
    }
    protected void gvPortage_ItemDataBound(object sender, GridItemEventArgs e)
    {
    }
    protected void gvSOAPendingList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            LinkButton lnk2ndcount = (LinkButton)e.Item.FindControl("lnk2ndcount");
            RadLabel lbl2ndurl = (RadLabel)e.Item.FindControl("lbl2ndurl");

            LinkButton lnk1stcount = (LinkButton)e.Item.FindControl("lnk1stcount");
            RadLabel lbl1sturl = (RadLabel)e.Item.FindControl("lbl1sturl");

            LinkButton lnkpubcount = (LinkButton)e.Item.FindControl("lnkpubcount");
            RadLabel lblpuburl = (RadLabel)e.Item.FindControl("lblpuburl");

            if (lnk2ndcount != null)
            {
                lnk2ndcount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Ship - " + lblmeasure.Text + "','" + lbl2ndurl.Text + "&Month=" + ddlsoamonth.SelectedMonth.ToString() + "&Year=" + ddlsoayear.SelectedYear.ToString() + "'); return false;");
            }

            if (lnk1stcount != null)
            {
                lnk1stcount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lbl1sturl.Text + "&Month=" + ddlsoamonth.SelectedMonth.ToString() + "&Year=" + ddlsoayear.SelectedYear.ToString() + "'); return false;");
            }
            if (lnkpubcount != null)
            {
                lnkpubcount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblpuburl.Text + "&Month=" + ddlsoamonth.SelectedMonth.ToString() + "&Year=" + ddlsoayear.SelectedYear.ToString() + "'); return false;");
            }
        }
    }
    //protected void ddlFund7days_DataBound(object sender, EventArgs e)
    //{
    //    ddlFund7days.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    //}
    protected void ddlFund7days_TextChanged(object sender, EventArgs e)
    {
        BindFundReceivedData();
       // idsoaother.Attributes["class"] = "active";
        idsoareport.Attributes["class"] = "";
        tab2.Attributes["class"] = "tab-pane active";
        tab1.Attributes["class"] = "tab-pane";
    }
    protected void gvMonthlyJV_ItemDataBound(object sender, GridItemEventArgs e)
    {
        //if (e.Item is GridDataItem)
        //{
        //    DataRowView drv = (DataRowView)e.Item.DataItem;

        //    RadLabel lnkPBYN = (RadLabel)e.Item.FindControl("lnkPBYN");
        //    RadLabel lblPBYN = (RadLabel)e.Item.FindControl("lblPBYN");

        //    if (drv["FLDPORTAGEBILLID"].ToString() == "")
        //    {
        //        lnkPBYN.Visible = true;
        //        lblPBYN.Visible = false;
        //    }
        //    else
        //    {
        //        lnkPBYN.Visible = false;
        //        lblPBYN.Visible = true;
        //    }
        //}
    }

    protected void gvMonthlyJV_ItemCommand(object sender, GridCommandEventArgs e)
    {
        RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblvesselid");
        RadLabel lblPortageBillid = (RadLabel)e.Item.FindControl("lblPortageBillid");
        RadLabel lblOCCID = (RadLabel)e.Item.FindControl("lblOCCID"); //FLDOFFICECAPTAINCASHID
        RadLabel lblIMBID = (RadLabel)e.Item.FindControl("lblIMBID"); //FLDINTERNALMONTHLYBILLINGID
        RadLabel lblCCVMAPID = (RadLabel)e.Item.FindControl("lblCCVMAPID"); //FLDCOMMITTEDCOSTVOUCHERMAPID
        RadLabel lblvslAccid = (RadLabel)e.Item.FindControl("lblvslAccid");
        RadLabel lblenddate = (RadLabel)e.Item.FindControl("lblenddate");
        RadLabel lblstartdate = (RadLabel)e.Item.FindControl("lblenddate");

        string url = "";
        if (e.CommandName == "PORTAGEBILL")
        {
            LinkButton lnkPBYN = (LinkButton)e.Item.FindControl("lnkPBYN");            

            if (lnkPBYN != null && lblPortageBillid.Text!="")
            {            
                url = String.Format(
                   "javascript:parent.openNewWindow('codehelp1', 'Crew List', 'Accounts/AccountsOfficePortageBillListDashboard.aspx?vslid="+lblvesselid.Text + "&pbid="+ lblPortageBillid.Text+"');");

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", url, true);
            }
        }
        if (e.CommandName == "COMMITTEDCOST") 
        {
            LinkButton lnkCommitedYN = (LinkButton)e.Item.FindControl("lnkCommitedYN");
            if (lnkCommitedYN != null)
            {
                url = String.Format(
                   "javascript:parent.openNewWindow('codehelp1', 'Committed Cost', 'Accounts/AccountsDashboardReportCommittedCost.aspx?vslAcutid=" + lblvslAccid.Text + "&date=" + General.GetNullableDateTime(lblenddate.Text) + "');");

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", url, true);
            }
        }
        if (e.CommandName == "CAPTAINCASH")
        {
            LinkButton lnkCCYN = (LinkButton)e.Item.FindControl("lnkCCYN");

            if (lnkCCYN != null)
            {
                url = String.Format(
                   "javascript:parent.openNewWindow('codehelp1', 'Captain Cash', 'Accounts/AccountsDashboardVesselAccountsPettyCashGeneral.aspx?vslid=" + lblvesselid.Text + "&DATE=" + General.GetNullableDateTime(lblenddate.Text) + "&STARTdate=" + General.GetNullableDateTime(lblstartdate.Text) + "');");

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", url, true);
            }
        }
        if (e.CommandName == "STANDARDBILL")
        {
            LinkButton lnkStandardBill = (LinkButton)e.Item.FindControl("lnkStandardBill");
            if (lnkStandardBill != null)
            {
                url = String.Format(
                   "javascript:parent.openNewWindow('codehelp1', 'Standard Bill', 'Accounts/AccountsInternalMonthlyBillingLineItemDashboard.aspx?vslid=" + lblvesselid.Text + "&pbid=" + lblPortageBillid.Text + "&IMBID=" + lblIMBID.Text + "');");

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", url, true);
            }
        }
    }

    protected void gvFundReceived_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblvesselid");
            LinkButton lnkRefNO = (LinkButton)e.Item.FindControl("lnkRefNO");
            string url = "";

            if (e.CommandName == "FUNDRECV")
            {
                if (lnkRefNO == null)
                {
                    url = String.Format(
                       "javascript:parent.openNewWindow('codehelp1', 'Crew List', 'Accounts/DashboardAccountsOwnerDebitCreditNoteFundReceived.aspx?ReferenceNo=" + lnkRefNO.Text + "');");

                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", url, true);
                }
            }
        }
    }

    protected void gvOutstandingFund_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");
            RadLabel lblprincipalid = (RadLabel)e.Item.FindControl("lblprincipalid");

            RadLabel lnk33dayscount = (RadLabel)e.Item.FindControl("lnk33dayscount");
            RadLabel lbl30daysurl = (RadLabel)e.Item.FindControl("lbl30daysurl");

            RadLabel lnk90dayscount = (RadLabel)e.Item.FindControl("lnk90dayscount");
            RadLabel lbl90daysurl = (RadLabel)e.Item.FindControl("lbl90daysurl");

            RadLabel lnkgrt90count = (RadLabel)e.Item.FindControl("lnkgrt90count");
            RadLabel lblgrt90url = (RadLabel)e.Item.FindControl("lblpuburl");

            RadLabel lnktotalcount = (RadLabel)e.Item.FindControl("lnktotalcount");
            RadLabel lbltotalurl = (RadLabel)e.Item.FindControl("lbltotalurl");

            if (lnk33dayscount != null)
            {
                lnk33dayscount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office -" + lblmeasure.Text + "','" + lbl30daysurl.Text + "&principalid=" + lblprincipalid.Text + "'); return false;");
            }

            if (lnk90dayscount != null)
            {
                lnk90dayscount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lbl90daysurl.Text + "&principalid=" + lblprincipalid.Text + "'); return false;");
            }
            if (lnkgrt90count == null)
            {
                //  lnkgrt90count.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblgrt90url.Text + "&principalid=" + lblprincipalid.Text + "'); return false;");
            }
            if (lnktotalcount != null)
            {
                lnktotalcount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lbltotalurl.Text + "&principalid=" + lblprincipalid.Text + "'); return false;");
            }
        }
    }

    protected void gvPhoneCard_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblvesselid");

            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            LinkButton lnk3dayscount = (LinkButton)e.Item.FindControl("lnk3dayscount");
            RadLabel lbl3daysurl = (RadLabel)e.Item.FindControl("lbl3daysurl");

            LinkButton lnk7dayscount = (LinkButton)e.Item.FindControl("lnk7dayscount");
            RadLabel lbl7daysurl = (RadLabel)e.Item.FindControl("lbl7daysurl");

            LinkButton lnkgrtsevcount = (LinkButton)e.Item.FindControl("lnkgrtsevcount");
            RadLabel lblgrtsevappurl = (RadLabel)e.Item.FindControl("lblgrtsevappurl");

            LinkButton lnktotalcount = (LinkButton)e.Item.FindControl("lnktotalcount");
            RadLabel lbltotalurl = (RadLabel)e.Item.FindControl("lbltotalurl");

            if (lnk3dayscount != null)
            {
                lnk3dayscount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office -" + lblmeasure.Text + "','" + lbl3daysurl.Text + "?vslid=" + lblvesselid.Text + "'); return false;");
            }

            if (lnk7dayscount != null)
            {
                lnk7dayscount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lbl7daysurl.Text + "?vslid=" + lblvesselid.Text + "'); return false;");
            }
            if (lnkgrtsevcount != null)
            {
                lnkgrtsevcount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblgrtsevappurl.Text + "?vslid=" + lblvesselid.Text + "'); return false;");
            }
            if (lnktotalcount != null)
            {
                lnktotalcount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lbltotalurl.Text + "?vslid=" + lblvesselid.Text + "'); return false;");
            }
        }
    }

    protected void gvOutstandingTotal_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            RadLabel lnkcount = (RadLabel)e.Item.FindControl("lnkcount");
            RadLabel lblurl = (RadLabel)e.Item.FindControl("lblurl");

            if (lnkcount == null)
            {
                lnkcount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblurl.Text + "'); return false;");
            }
        }
    }

    protected void gvInvoiceParApp_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            RadLabel lnk3dayscount = (RadLabel)e.Item.FindControl("lnk3dayscount");
            RadLabel lbl3daysurl = (RadLabel)e.Item.FindControl("lbl3daysurl");

            RadLabel lnk7dayscount = (RadLabel)e.Item.FindControl("lnk7dayscount");
            RadLabel lbl7daysurl = (RadLabel)e.Item.FindControl("lbl7daysurl");

            RadLabel lnkgrtsevcount = (RadLabel)e.Item.FindControl("lnkgrtsevcount");
            RadLabel lblgrtsevappurl = (RadLabel)e.Item.FindControl("lblgrtsevappurl");

            RadLabel lnktotalcount = (RadLabel)e.Item.FindControl("lnktotalcount");
            RadLabel lbltotalurl = (RadLabel)e.Item.FindControl("lbltotalurl");

            if (lnk3dayscount != null)
            {
                lnk3dayscount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lbl3daysurl.Text + "'); return false;");
            }
            if (lnk7dayscount != null)
            {
                lnk7dayscount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lbl7daysurl.Text + "'); return false;");
            }
            if (lnkgrtsevcount != null)
            {
                lnkgrtsevcount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblgrtsevappurl.Text + "'); return false;");
            }
            if (lnktotalcount != null)
            {
                lnktotalcount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lbltotalurl.Text + "'); return false;");
            }
        }
    }

    protected void gvInvoiceAdvPMV_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            RadLabel lnkpendappcount = (RadLabel)e.Item.FindControl("lnkpendappcount");
            RadLabel lblpendappurl = (RadLabel)e.Item.FindControl("lblpendappurl");

            RadLabel lnkparappcount = (RadLabel)e.Item.FindControl("lnkparappcount");
            RadLabel lblparappurl = (RadLabel)e.Item.FindControl("lblparappurl");

            RadLabel lnkfullappcount = (RadLabel)e.Item.FindControl("lnkfullappcount");
            RadLabel lblfullappurl = (RadLabel)e.Item.FindControl("lblfullappurl");

            if (lnkpendappcount != null)
            {
                lnkpendappcount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblpendappurl.Text + "'); return false;");
            }
            if (lnkparappcount != null)
            {
                lnkparappcount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblparappurl.Text + "'); return false;");
            }
            if (lnkfullappcount != null)
            {
                lnkfullappcount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblfullappurl.Text + "'); return false;");
            }
        }
    }

    protected void gvInvoiceAccChecking_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            RadLabel lnkacccheckingcount = (RadLabel)e.Item.FindControl("lnkacccheckingcount");
            RadLabel lblacccheckingurl = (RadLabel)e.Item.FindControl("lblacccheckingurl");

            if (lnkacccheckingcount != null)
            {
                lnkacccheckingcount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblacccheckingurl.Text + "'); return false;");
            }
        }
    }

    protected void gvAdvanceAccChecking_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            RadLabel lnkpoadvancecount = (RadLabel)e.Item.FindControl("lnkpoadvancecount");
            RadLabel lblpoadvanceurl = (RadLabel)e.Item.FindControl("lblpoadvanceurl");

            RadLabel lnkdepositcount = (RadLabel)e.Item.FindControl("lnkdepositcount");
            RadLabel lbldepositurl = (RadLabel)e.Item.FindControl("lbldepositurl");

            if (lnkpoadvancecount != null)
            {
                lnkpoadvancecount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblpoadvanceurl.Text + "'); return false;");
            }

            if (lnkdepositcount != null)
            {
                lnkdepositcount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lbldepositurl.Text + "'); return false;");
            }
        }
    }

    protected void gvGeneratePMV_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            LinkButton lnkinvoicecount = (LinkButton)e.Item.FindControl("lnkinvoicecount");
            RadLabel lblinvoiceurl = (RadLabel)e.Item.FindControl("lblinvoiceurl");

            LinkButton lnkpoadvancecount = (LinkButton)e.Item.FindControl("lnkpoadvancecount");
            RadLabel lblpoadvanceurl = (RadLabel)e.Item.FindControl("lblpoadvanceurl");

            LinkButton lnkdepositcount = (LinkButton)e.Item.FindControl("lnkdepositcount");
            RadLabel lbldepositurl = (RadLabel)e.Item.FindControl("lbldepositurl");

            if (lnkinvoicecount != null)
            {
                lnkinvoicecount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblinvoiceurl.Text + "'); return false;");
            }

            if (lnkpoadvancecount != null)
            {
                lnkpoadvancecount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblpoadvanceurl.Text + "'); return false;");
            }

            if (lnkdepositcount != null)
            {
                lnkdepositcount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lbldepositurl.Text + "'); return false;");
            }
        }
    }

    protected void gvRMTNotGen_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            RadLabel lnkADVPMVCount = (RadLabel)e.Item.FindControl("lnkADVPMVCount");
            RadLabel lblADVPMVurl = (RadLabel)e.Item.FindControl("lblADVPMVurl");

            RadLabel lnkCTMPMVCount = (RadLabel)e.Item.FindControl("lnkCTMPMVCount");
            RadLabel lblCTMPMVurl = (RadLabel)e.Item.FindControl("lblCTMPMVurl");

            RadLabel lnkdepositPMVcount = (RadLabel)e.Item.FindControl("lnkdepositPMVcount");
            RadLabel lbldepositPMVurl = (RadLabel)e.Item.FindControl("lbldepositPMVurl");

            RadLabel lnkTotalcount = (RadLabel)e.Item.FindControl("lnkTotalcount");
            RadLabel lblTotalurl = (RadLabel)e.Item.FindControl("lblTotalurl");

            if (lnkADVPMVCount != null)
            {
                lnkADVPMVCount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblADVPMVurl.Text + "'); return false;");
            }

            if (lnkCTMPMVCount != null)
            {
                lnkCTMPMVCount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblCTMPMVurl.Text + "'); return false;");
            }

            if (lnkdepositPMVcount != null)
            {
                lnkdepositPMVcount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lbldepositPMVurl.Text + "'); return false;");
            }

            if (lnkTotalcount != null)
            {
                lnkTotalcount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblTotalurl.Text + "'); return false;");
            }
        }
    }

    protected void gvApprovedPMVRMTnotGen_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            RadLabel lnkThreeCount = (RadLabel)e.Item.FindControl("lnkThreeCount");
            RadLabel lblThreeurl = (RadLabel)e.Item.FindControl("lblThreeurl");

            RadLabel lnkThreeSevenCount = (RadLabel)e.Item.FindControl("lnkThreeSevenCount");
            RadLabel lblThreeSevenurl = (RadLabel)e.Item.FindControl("lblThreeSevenurl");

            RadLabel lnkdGTRSevenCount = (RadLabel)e.Item.FindControl("lnkdGTRSevenCount");
            RadLabel lblGTRSevenurl = (RadLabel)e.Item.FindControl("lblGTRSevenurl");

            //LinkButton lnkTotalcount = (LinkButton)e.Item.FindControl("lnkTotalcount");
            RadLabel lblTotalC = (RadLabel)e.Item.FindControl("lblTotalC");

            if (lnkThreeCount != null)
            {
                lnkThreeCount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblThreeurl.Text + "'); return false;");
            }

            if (lnkThreeSevenCount != null)
            {
                lnkThreeSevenCount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblThreeSevenurl.Text + "'); return false;");
            }

            if (lnkdGTRSevenCount != null)
            {
                lnkdGTRSevenCount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblGTRSevenurl.Text + "'); return false;");
            }

            //if (lnkTotalcount != null)
            //{
            //    lnkTotalcount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblTotalurl.Text + "'); return false;");
            //}
        }
    }

    protected void gvApprovedAllotM_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            RadLabel lnk03Count = (RadLabel)e.Item.FindControl("lnk03Count");
            RadLabel lbl03url = (RadLabel)e.Item.FindControl("lbl03url");

            RadLabel lnk37Count = (RadLabel)e.Item.FindControl("lnk37Count");
            RadLabel lbl37url = (RadLabel)e.Item.FindControl("lbl37url");

            RadLabel lnkGTR7Count = (RadLabel)e.Item.FindControl("lnkGTR7Count");
            RadLabel lblGTR7url = (RadLabel)e.Item.FindControl("lblGTR7url");

            //LinkButton lnkTotalcount = (LinkButton)e.Item.FindControl("lnkTotalcount");
            RadLabel lblTotalC = (RadLabel)e.Item.FindControl("lblTotalC");

            if (lnk03Count != null)
            {
                lnk03Count.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lbl03url.Text + "'); return false;");
            }

            if (lnk37Count != null)
            {
                lnk37Count.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lbl37url.Text + "'); return false;");
            }

            if (lnkGTR7Count != null)
            {
                lnkGTR7Count.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblGTR7url.Text + "'); return false;");
            }
        }
    }

    protected void gvRemittanceCount_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            RadLabel lnkRemittanceCount = (RadLabel)e.Item.FindControl("lnkRemittanceCount");
            RadLabel lblRemittanceurl = (RadLabel)e.Item.FindControl("lblRemittanceurl");

            RadLabel lnkVerifiedCount = (RadLabel)e.Item.FindControl("lnkVerifiedCount");
            RadLabel lblVerifiedurl = (RadLabel)e.Item.FindControl("lblVerifiedurl");

            RadLabel lnkPACount = (RadLabel)e.Item.FindControl("lnkPACount");
            RadLabel lblPAurl = (RadLabel)e.Item.FindControl("lblPAurl");

            RadLabel lnkFXACount = (RadLabel)e.Item.FindControl("lnkFXACount");
            RadLabel lblFXurl = (RadLabel)e.Item.FindControl("lblFXurl");

            RadLabel lnkPIBcount = (RadLabel)e.Item.FindControl("lnkPIBcount");
            RadLabel lblPIBurl = (RadLabel)e.Item.FindControl("lblPIBurl");

            RadLabel lnkFileNotCount = (RadLabel)e.Item.FindControl("lnkFileNotCount");
            RadLabel lblFileNoturl = (RadLabel)e.Item.FindControl("lblFileNoturl");

            RadLabel lnkFileCount = (RadLabel)e.Item.FindControl("lnkFileCount");
            RadLabel lblFileurl = (RadLabel)e.Item.FindControl("lblFileurl");
            //LinkButton lnkTotalcount = (LinkButton)e.Item.FindControl("lnkTotalcount");
            RadLabel lblTotalC = (RadLabel)e.Item.FindControl("lblTotalC");

            if (lnkRemittanceCount != null)
            {
                lnkRemittanceCount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblRemittanceurl.Text + "'); return false;");
            }

            if (lnkVerifiedCount != null)
            {
                lnkVerifiedCount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblVerifiedurl.Text + "'); return false;");
            }

            if (lnkPACount != null)
            {
                lnkPACount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblPAurl.Text + "'); return false;");
            }

            if (lnkFXACount != null)
            {
                lnkFXACount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblFXurl.Text + "'); return false;");
            }

            if (lnkPIBcount != null)
            {
                lnkPIBcount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblPIBurl.Text + "'); return false;");
            }

            if (lnkFileNotCount != null)
            {
                lnkFileNotCount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblFileNoturl.Text + "'); return false;");
            }

            if (lnkFileCount != null)
            {
                lnkFileCount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblFileurl.Text + "'); return false;");
            }
        }
    }

    private void BindPendingFR()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardPendingFundsRequestsList();
        //gvPendingFR.DataSource = ds.Tables[0];
        //gvPendingFR.DataBind();
    }
    private void BindBankBalance()
    {
        //DataSet ds = PhoenixDashboardAccounts.DashboardBankBalancesList();
        //gvBankBalance.DataSource = ds.Tables[0];
        //gvBankBalance.DataBind();
    }

    protected void gvPendingFR_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            RadLabel lnkOD2WKAmount = (RadLabel)e.Item.FindControl("lnkOD2WKAmount");
            RadLabel lblOD2WKurl = (RadLabel)e.Item.FindControl("lblOD2WKurl");

            RadLabel lnkOD1WKAmount = (RadLabel)e.Item.FindControl("lnkOD1WKAmount");
            RadLabel lblOD1WKurl = (RadLabel)e.Item.FindControl("lblOD1WKurl");

            RadLabel lnkThisWKAmount = (RadLabel)e.Item.FindControl("lnkThisWKAmount");
            RadLabel lblThisWKurl = (RadLabel)e.Item.FindControl("lblThisWKurl");

            RadLabel lnkNEXTWK = (RadLabel)e.Item.FindControl("lnkNEXTWK");
            RadLabel lblNEXTWKurl = (RadLabel)e.Item.FindControl("lblNEXTWKurl");

            RadLabel lblFollowingWK = (RadLabel)e.Item.FindControl("lblFollowingWK");
            RadLabel lnkFollowingWKurl = (RadLabel)e.Item.FindControl("lnkFollowingWKurl");

            if (lnkOD2WKAmount != null)
            {
                lnkOD2WKAmount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblOD2WKurl.Text + "'); return false;");
            }
            if (lnkOD1WKAmount != null)
            {
                lnkOD1WKAmount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblOD1WKurl.Text + "'); return false;");
            }
            if (lnkThisWKAmount != null)
            {
                lnkThisWKAmount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblThisWKurl.Text + "'); return false;");
            }
            if (lnkNEXTWK != null)
            {
                lnkNEXTWK.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblNEXTWKurl.Text + "'); return false;");
            }
            if (lblFollowingWK != null)
            {
                lblFollowingWK.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lnkFollowingWKurl.Text + "'); return false;");
            }
        }
    }

    protected void gvEstimatedFU_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblCName");

            RadLabel lnkCTMSum = (RadLabel)e.Item.FindControl("lnkCTMSum");
            RadLabel lblCTMurl = (RadLabel)e.Item.FindControl("lblCTMurl");

            RadLabel lnkPOAdv = (RadLabel)e.Item.FindControl("lnkPOAdv");
            RadLabel lblPOAdvurl = (RadLabel)e.Item.FindControl("lblPOAdvurl");

            RadLabel lnkDepositePMV = (RadLabel)e.Item.FindControl("lnkDepositePMV");
            RadLabel lblDepositeurl = (RadLabel)e.Item.FindControl("lblDepositeurl");

            RadLabel lnkUnderProc = (RadLabel)e.Item.FindControl("lnkUnderProc");
            RadLabel lblUnderProc = (RadLabel)e.Item.FindControl("lblUnderProc");

            RadLabel lnkTotalSum = (RadLabel)e.Item.FindControl("lnkTotalSum");
            RadLabel lblTotalurl = (RadLabel)e.Item.FindControl("lblTotalurl");

            if (lnkCTMSum != null)
            {
                lnkCTMSum.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblCTMurl.Text + "'); return false;");
            }
            if (lnkPOAdv != null)
            {
                lnkPOAdv.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblPOAdvurl.Text + "'); return false;");
            }
            if (lnkDepositePMV != null)
            {
                lnkDepositePMV.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblDepositeurl.Text + "'); return false;");
            }
            if (lnkUnderProc != null)
            {
                lnkUnderProc.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblUnderProc.Text + "'); return false;");
            }
            if (lnkTotalSum != null)
            {
                lnkTotalSum.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblTotalurl.Text + "'); return false;");
            }
        }
    }

    protected void gvApprovedSupplPMVs_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            RadLabel lnkLT1WK = (RadLabel)e.Item.FindControl("lnkLT1WK");
            RadLabel lblLT1WKurl = (RadLabel)e.Item.FindControl("lblLT1WKurl");

            RadLabel lnkLT2WK = (RadLabel)e.Item.FindControl("lnkLT2WK");
            RadLabel lblLT2WKurl = (RadLabel)e.Item.FindControl("lblLT2WKurl");

            RadLabel lnkLT4WK = (RadLabel)e.Item.FindControl("lnkLT4WK");
            RadLabel lblLT4WKurl = (RadLabel)e.Item.FindControl("lblLT4WKurl");

            RadLabel lnkGT4WK = (RadLabel)e.Item.FindControl("lnkGT4WK");
            RadLabel lblGT4WKurl = (RadLabel)e.Item.FindControl("lblGT4WKurl");

            RadLabel lnkTotalSum = (RadLabel)e.Item.FindControl("lnkTotalSum");
            RadLabel lblTotalurl = (RadLabel)e.Item.FindControl("lblTotalurl");

            if (lnkLT1WK != null)
            {
                lnkLT1WK.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblLT1WKurl.Text + "'); return false;");
            }
            if (lnkLT2WK != null)
            {
                lnkLT2WK.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblLT2WKurl.Text + "'); return false;");
            }
            if (lnkLT4WK != null)
            {
                lnkLT4WK.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblLT4WKurl.Text + "'); return false;");
            }
            if (lnkGT4WK != null)
            {
                lnkGT4WK.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblGT4WKurl.Text + "'); return false;");
            }
            if (lnkTotalSum != null)
            {
                lnkTotalSum.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office - " + lblmeasure.Text + "','" + lblTotalurl.Text + "'); return false;");
            }
        }
    }

    protected void gvLocalCP_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            //   RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            RadLabel lnkGT7Days = (RadLabel)e.Item.FindControl("lnkGT7Days");
            RadLabel lblGT7Days = (RadLabel)e.Item.FindControl("lblGT7Days");

            RadLabel lnkLS7Days = (RadLabel)e.Item.FindControl("lnkLS7Days");
            RadLabel lblLSDays = (RadLabel)e.Item.FindControl("lblLSDays");

            if (lnkGT7Days != null)
            {
                lnkGT7Days.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office -','" + lblGT7Days.Text + "'); return false;");
            }
            if (lnkLS7Days != null)
            {
                lnkLS7Days.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office -','" + lblLSDays.Text + "'); return false;");
            }
        }
    }

    protected void gvTravelAdv_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            RadLabel lnkPMVAppcount = (RadLabel)e.Item.FindControl("lnkPMVAppcount");
            RadLabel lblPMVAppsurl = (RadLabel)e.Item.FindControl("lblPMVAppsurl");

            RadLabel lnkCRGcount = (RadLabel)e.Item.FindControl("lnkCRGcount");
            RadLabel lblCRGsurl = (RadLabel)e.Item.FindControl("lblCRGsurl");

            RadLabel lnkgPPcount = (RadLabel)e.Item.FindControl("lnkgPPcount");
            RadLabel lblPPurl = (RadLabel)e.Item.FindControl("lblPPurl");

            if (lnkPMVAppcount != null)
            {
                lnkPMVAppcount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office -" + lblmeasure.Text + "','" + lblPMVAppsurl.Text + "'); return false;");
            }
            if (lnkCRGcount != null)
            {
                lnkCRGcount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office -" + lblmeasure.Text + "','" + lblCRGsurl.Text + "'); return false;");
            }
            if (lnkgPPcount != null)
            {
                lnkgPPcount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office -" + lblmeasure.Text + "','" + lblPPurl.Text + "'); return false;");
            }
        }
    }

    protected void gvCTMArrng_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton VesselName = (LinkButton)e.Item.FindControl("lblVesselName");
            RadLabel VesselID = (RadLabel)e.Item.FindControl("lblVesselID");
            RadLabel CaptainCashID = (RadLabel)e.Item.FindControl("lblCaptainCashID");

            string url = "";
            if (VesselName != null)
            {
                if (CaptainCashID != null)
                {
                    url = String.Format(
                       "javascript:parent.openNewWindow('codehelp1', 'Crew List', 'Accounts/AccountsDashboardCtmRequestArrangementGeneral.aspx?vesselid=" + VesselID.Text + "&CaptainCashID=" + CaptainCashID.Text + "');");

                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", url, true);
                }
            }
        }
    }

    protected void gvOpenTechPOs_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindOpenTechPOs();
    }

    protected void gvDirectPOs_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDirectPOs();
    }

    protected void gvOpenDryPOs_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindOpenDryPOs();
    }

    protected void gvUnbilledEntr_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindUnbilledEntr();
    }

    protected void ddlMonthUnbill_TextChangedEvent(object sender, EventArgs e)
    {
        //BindSOAPendingListddlBind();
        //BindPortage();
        //BindMonthlyJV();
        BindUnbilledEntrDDlBind();
        idsoareport.Attributes["class"] = "active";
        tab1.Attributes["class"] = "tab-pane active";
      //  idsoaother.Attributes["class"] = "";
        tab2.Attributes["class"] = "tab-pane";
    }

    protected void ddlYearUnbill_TextChangedEvent(object sender, EventArgs e)
    {
        //BindSOAPendingListddlBind();
        //BindPortage();
        //BindMonthlyJV();
        BindUnbilledEntrDDlBind();
        idsoareport.Attributes["class"] = "active";
        tab1.Attributes["class"] = "tab-pane active";
      //  idsoaother.Attributes["class"] = "";
        tab2.Attributes["class"] = "tab-pane";

    }

    protected void gvOpenDryPOs_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadLabel lblVesselO = (RadLabel)e.Item.FindControl("lblVesselO");

        RadLabel lnkFirstPO = (RadLabel)e.Item.FindControl("lnkFirstPO");
        RadLabel lblFirstPOurl = (RadLabel)e.Item.FindControl("lblFirstPOurl");

        if (lnkFirstPO != null)
        {
            lnkFirstPO.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office -" + lblVesselO.Text + "','" + lblFirstPOurl.Text + "'); return false;");
        }
    }

    protected void gvUnbilledEntr_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadLabel lblVessel = (RadLabel)e.Item.FindControl("lblVessel");

        RadLabel lnkCountt = (RadLabel)e.Item.FindControl("lnkCountt");
        RadLabel lblCounturl = (RadLabel)e.Item.FindControl("lblCounturl");

        if (lnkCountt != null)
        {
            lnkCountt.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office -" + lblVessel.Text + "','" + lblCounturl.Text + "'); return false;");
        }
    }

    protected void gvOpenTechPOs_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

        RadLabel lnk06count = (RadLabel)e.Item.FindControl("lnk06count");
        RadLabel lbl06url = (RadLabel)e.Item.FindControl("lbl06url");

        RadLabel lnk60count = (RadLabel)e.Item.FindControl("lnk60count");
        RadLabel lbl60url = (RadLabel)e.Item.FindControl("lbl60url");

        RadLabel lnk120count = (RadLabel)e.Item.FindControl("lnk120count");
        RadLabel lbl120url = (RadLabel)e.Item.FindControl("lbl120url");

        RadLabel lnkGT180count = (RadLabel)e.Item.FindControl("lnkGT180count");
        RadLabel lblGT180url = (RadLabel)e.Item.FindControl("lblGT180url");

        if (lnk06count != null)
        {
            lnk06count.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office -" + lblmeasure.Text + "','" + lbl06url.Text + "'); return false;");
        }
        if (lnk60count != null)
        {
            lnk60count.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office -" + lblmeasure.Text + "','" + lbl60url.Text + "'); return false;");
        }
        if (lnk120count != null)
        {
            lnk120count.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office -" + lblmeasure.Text + "','" + lbl120url.Text + "'); return false;");
        }
        if (lnkGT180count != null)
        {
            lnkGT180count.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office -" + lblmeasure.Text + "','" + lblGT180url.Text + "'); return false;");
        }
    }

    protected void gvDirectPOs_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

        RadLabel lnk06count = (RadLabel)e.Item.FindControl("lnk06count");
        RadLabel lbl06url = (RadLabel)e.Item.FindControl("lbl06url");

        RadLabel lnk60count = (RadLabel)e.Item.FindControl("lnk60count");
        RadLabel lbl60url = (RadLabel)e.Item.FindControl("lbl60url");

        RadLabel lnk120count = (RadLabel)e.Item.FindControl("lnk120count");
        RadLabel lbl120url = (RadLabel)e.Item.FindControl("lbl120url");

        RadLabel lnkGT180count = (RadLabel)e.Item.FindControl("lnkGT180count");
        RadLabel lblGT180url = (RadLabel)e.Item.FindControl("lblGT180url");

        if (lnk06count != null)
        {
            lnk06count.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office -" + lblmeasure.Text + "','" + lbl06url.Text + "'); return false;");
        }
        if (lnk60count != null)
        {
            lnk60count.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office -" + lblmeasure.Text + "','" + lbl60url.Text + "'); return false;");
        }
        if (lnk120count != null)
        {
            lnk120count.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office -" + lblmeasure.Text + "','" + lbl120url.Text + "'); return false;");
        }
        if (lnkGT180count != null)
        {
            lnkGT180count.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Office -" + lblmeasure.Text + "','" + lblGT180url.Text + "'); return false;");
        }
    }
}