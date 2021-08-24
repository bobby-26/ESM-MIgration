using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Reports;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using Telerik.Web.UI;

public partial class AccountsVesselVisitMonthlybilling : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Vessel Visit", "VISIT", ToolBarDirection.Right);
            MenuTravelClaimMain.AccessRights = this.ViewState;
            MenuTravelClaimMain.MenuList = toolbar.Show();

            //MenuTravelClaim.SelectedMenuIndex = 2;

            if (!IsPostBack)
            {
                ViewState["visitid"] = Request.QueryString["visitid"];
                ViewState["VisitType"] = Request.QueryString["VisitType"];
                BindData();
            }
           //


            txtBulkBudgetName.Attributes.Add("style", "visibility:hidden;");
            txtBulkBudgetId.Attributes.Add("style", "visibility:hidden;");
            txtBulkBudgetgroupId.Attributes.Add("style", "visibility:hidden;");
            txtBulkOwnerBudgetName.Attributes.Add("style", "visibility:hidden;");
            txtBulkOwnerBudgetId.Attributes.Add("style", "visibility:hidden;");
            txtBulkOwnerBudgetgroupId.Attributes.Add("style", "visibility:hidden;");
        }
       

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
       

    }
    private void BindData()
    {
        DataSet ds = PhoenixAccountsInternalBilling.VesselvisitMonthlyBillingEdit(new Guid(ViewState["visitid"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            lblFormno.Text = dr["FLDFORMNUMBER"].ToString();
            lblClaimstatus.Text = dr["FLDCLAIMSTATUS"].ToString(); ;
            lblEmpId.Text = dr["FLDEMPLOYEECODE"].ToString();
            lblEmpname.Text = dr["FLDEMPLOYEENAME"].ToString();
            lblFromdate.Text = dr["FLDFROMDATE"].ToString();
            lblTodate.Text = dr["FLDTODATE"].ToString();
            lblBudgeteddays.Text = dr["FLDBUDGETEDDAYS"].ToString();
            lblCurrentvisit.Text = dr["FLDCURRENTVISIT"].ToString();
            lbltotalinclucurrentdays.Text = dr["FLDTOTALCURRENTVISITDAYS"].ToString();
            lblChargeddays.Text = dr["FLDCHARGEDDAYS"].ToString();
            lblchargingdays.Text = dr["FLDCHARGINGDAYS"].ToString();
            lblBillingunit.Text = dr["FLDBILLINGUNITNAME"].ToString();
            lblAmount.Text = dr["FLDTOTALAMOUNT"].ToString();
            txtRate.Text = dr["FLDBILLINGITEMRATE"].ToString();
            lblVoouchernumber.Text = dr["FLDVOUCHERNUMBER"].ToString();
            if (dr["FLDVOUCHERDATE"].ToString() != "")
            {
                txtPostdate.Text = dr["FLDVOUCHERDATE"].ToString();
            }
            else
            {
                txtPostdate.Text = DateTime.Today.ToString();
            }
            txtBulkOwnerBudgetCode.Text = dr["FLDOWNERBUDGETCODE"].ToString();
            txtBulkBudgetCode.Text = dr["FLDVESSELBUDGETCODE"].ToString();
            txtBulkOwnerBudgetId.Text = dr["FLDOWNERBUDGETID"].ToString();
            txtBulkBudgetId.Text = dr["FLDVESSELBUDGETID"].ToString();
            lblVesselaccount.Text = dr["FLDACCOUNTDESCRIPTION"].ToString();
            lblFinyear.Text = dr["FLDFINYEAR"].ToString();
            ddlBudgetedVisit.SelectedValue = dr["FLDBUDGETEDVISIT"].ToString();

            if (dr["FLDBUDGETEDVISIT"].ToString() != "1")
            {
                lblBudgeteddays.Text = "NA";
                lbltotalinclucurrentdays.Text = "NA";
                lblChargeddays.Text = "NA";
            }
         
            ViewState["VesselId"] = dr["FLDVESSELID"].ToString();
            ViewState["Vesselbudgetallocationid"] = dr["FLDVESSELBUDGETALLOCATIONID"].ToString();
            ViewState["DTKey"] = dr["FLDVOUCHERDTKEY"].ToString();
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            if (dr["FLDISPOSTED"].ToString() != "1")
            {
                toolbar1.AddButton("Post", "POST", ToolBarDirection.Right);
                toolbar1.AddButton("Save", "SAVE", ToolBarDirection.Right);
                MenuTravelclaim.AccessRights = this.ViewState;
                MenuTravelclaim.MenuList = toolbar1.Show();
                cmdAtt.Visible = false;
            }
            else
            {
                toolbar1.AddButton("Repost", "REPOST", ToolBarDirection.Right);
                MenuTravelclaim.AccessRights = this.ViewState;
                MenuTravelclaim.MenuList = toolbar1.Show();
            }

            Getprincipal(int.Parse(dr["FLDACCOUNTID"].ToString()));

            btnShowBulkBudget.Attributes.Add("onclick", "return showPickList('spnPickListBulkBudget', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + ViewState["VesselId"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
            btnShowBulkOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListBulkOwnerBudget', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=true&vesselid=" + null + "&Ownerid=" + ViewState["PRINCIPAL"] + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBulkBudgetId.Text + "', true); ");
            cmdAtt.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKey"].ToString() + "&mod=" + PhoenixModule.ACCOUNTS + " &U=1 '); return false;");

        }
    }

    public void Getprincipal(int accountid)
    {
        try
        {
            DataSet ds = null;
            ds = PhoenixRegistersAccount.EditAccount(accountid);
            DataRow dr = ds.Tables[0].Rows[0];
            ViewState["PRINCIPAL"] = Convert.ToString(dr["FLDPRINCIPALID"]);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuTravelClaimMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("VISIT"))
            {
                Response.Redirect("../Accounts/AccountsVesselVisitITSuperintendentVisitRegister.aspx?VisitId=" + ViewState["visitid"].ToString() + "&VisitType=" + ViewState["VisitType"]);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuTravelClaim_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidLineItem())
                {
                    string errormessage = "";
                    errormessage = ucError.ErrorMessage;
                    errormessage = errormessage.Replace("<font color='#ff0000'>", "").Replace("</font>", "");
                    RadWindowManager1.RadAlert(errormessage, 400, 150, "Message", null);
                    return;
                }

                PhoenixAccountsInternalBilling.MonthliBillingSupVisitUpdate(new Guid(ViewState["visitid"].ToString())
                     , General.GetNullableGuid(txtBulkOwnerBudgetId.Text)
                     , int.Parse(txtBulkBudgetId.Text)
                     , txtRate.Text
                     , ddlBudgetedVisit.SelectedValue);

                ucStatus.Text = "Monthly billing updated successfully.";
            }
            if (CommandName.ToUpper().Equals("POST"))
            {
                Guid VoucherDtKey = Guid.Empty;
                PhoenixAccountsInternalBilling.VesselVisitMonthlybillPost(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["visitid"].ToString())
                    //, new Guid(ViewState["Vesselbudgetallocationid"].ToString())
                    , General.GetNullableGuid(ViewState["Vesselbudgetallocationid"].ToString())
                    , Decimal.Parse(lblAmount.Text)
                    , DateTime.Parse(txtPostdate.Text)
                    , ref VoucherDtKey
                    );

                AttachPD(VoucherDtKey, 1); //Attach Supporting Document

                ucStatus.Text = "Monthly billing posted successfully.";
            }

            if (CommandName.ToUpper().Equals("REPOST"))
            {
                PhoenixAccountsInternalBilling.VesselVisitMonthlybillRePost(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["visitid"].ToString()));

                ucStatus.Text = "Monthly billing reposted successfully.";
            }
            Response.Redirect("../Accounts/AccountsVesselVisitMonthlybilling.aspx?VisitId=" + ViewState["visitid"].ToString() + "&VisitType=" + ViewState["VisitType"].ToString(), false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void AttachPD(Guid Dtkey, int arr)
    {
        string Tmpfilelocation = string.Empty; string[] reportfile = new string[arr];
        DataSet ds = new DataSet();



        ds = PhoenixAccountsInternalBilling.MonthlyBillingReport(new Guid(ViewState["visitid"].ToString()));

        if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
        {

            if (ddlBudgetedVisit.SelectedValue != "1")
            {
                //NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
                NameValueCollection nvc = new NameValueCollection();
                //nvc.Remove("applicationcode");
                nvc.Add("applicationcode", "5");
                //nvc.Remove("reportcode");
                nvc.Add("reportcode", "MONTHLYBILLING");
                nvc.Add("CRITERIA", "");
                Session["PHOENIXREPORTPARAMETERS"] = nvc;
            }
            else
            {
                //NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
                NameValueCollection nvc = new NameValueCollection();
                //nvc.Remove("applicationcode");
                nvc.Add("applicationcode", "5");
                //nvc.Remove("reportcode");
                nvc.Add("reportcode", "MONTHLYBILLINGBUDGETED");
                nvc.Add("CRITERIA", "");
                Session["PHOENIXREPORTPARAMETERS"] = nvc;
            }

            //  Tmpfilelocation = System.Web.HttpContext.Current.Request.MapPath("~/");
            string fileid = Dtkey.ToString();
            string filename = fileid + ".pdf";
            Tmpfilelocation = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/Accounts/" + filename;
            //Tmpfilelocation = HttpContext.Current.Server.MapPath("../Attachments/Accounts/" +filename);

            PhoenixSsrsReportsCommon.getVersion();
            PhoenixSsrsReportsCommon.getLogo();
            PhoenixSSRSReportClass.ExportSSRSReport(reportfile, ds, PhoenixSSRSReportClass.ExportFileFormat.PDF, Tmpfilelocation, ref Tmpfilelocation);
        }
        else
        {
            if (ddlBudgetedVisit.SelectedValue != "1")
                reportfile[0] = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountsMonthlyBillingReport.rpt");
            else
                reportfile[0] = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountsMonthlyBillingReportBudgeted.rpt");

            //  Tmpfilelocation = System.Web.HttpContext.Current.Request.MapPath("~/");
            string fileid = Dtkey.ToString();
            string filename = fileid + ".pdf";
            Tmpfilelocation = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/Accounts/" + filename;
            //Tmpfilelocation = Tmpfilelocation + "Attachments/Accounts/" + filename;
            PhoenixReportClass.ExportReport(reportfile, Tmpfilelocation, ds);
        }
    }

    public bool IsValidLineItem()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        ucError.clear = "";
        
        if (txtRate.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Rate is required.";

        return (!ucError.IsError);
    }
    protected void txtBulkBudgetIdClick(object sender, EventArgs e)
    {
        txtBulkOwnerBudgetgroupId.Text = "";
        txtBulkOwnerBudgetId.Text = "";
        txtBulkOwnerBudgetName.Text = "";
        txtBulkOwnerBudgetCode.Text = "";
    }

}
