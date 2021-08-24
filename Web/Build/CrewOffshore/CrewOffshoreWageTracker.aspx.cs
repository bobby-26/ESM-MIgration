using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using Telerik.Web.UI;
public partial class CrewOffshoreWageTracker : PhoenixBasePage
{
    public int TravelDaysTotal = 0;
    public decimal TravelWagesTotal = 0;
    public int OnboardDaysTotal = 0;
    public decimal OnboardWagesTotal = 0;
    public int SignoffTravelDaysTotal = 0;
    public decimal TravelAllowanceTotal = 0;
    public decimal ReimbursementsTotal = 0;
    public decimal WagesTotal = 0;
    //public decimal WagesTotalrc = 0;
    public decimal DeductionsTotal = 0;
    public decimal DPAllowanceTotal = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            //ucConfirm.Visible = false;
            confirm.Attributes.Add("style", "display:none");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreWageTracker.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvWageTracker')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreWageTracker.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuOffshoreWageTracker.AccessRights = this.ViewState;
            MenuOffshoreWageTracker.MenuList = toolbar.Show();


            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Unlock", "UNLOCK", ToolBarDirection.Right);
            toolbar.AddButton("Lock", "LOCK",ToolBarDirection.Right);
            
            CrewTrainingMenu.AccessRights = this.ViewState;
            CrewTrainingMenu.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Wage Tracker Summary", "SUMMARY");
            toolbar.AddButton("Wage Tracker", "WAGETRACKER");
            CrewMenuGeneral.AccessRights = this.ViewState;
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                CrewMenuGeneral.MenuList = toolbar.Show();
                CrewMenuGeneral.SelectedMenuIndex = 1;
            }

            if (!IsPostBack)
            {
                ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
                BindYear();
                BindDate();

                ViewState["lockunlock"] = "";
                ViewState["vesselid"] = "";
                ViewState["month"] = "";
                ViewState["year"] = "";
                ViewState["asondate"] = "";
                if (Request.QueryString["month"] != null && Request.QueryString["month"].ToString() != "")
                {
                    ViewState["month"] = Request.QueryString["month"].ToString();
                    ddlMonth.SelectedValue = ViewState["month"].ToString();
                }
                if (Request.QueryString["year"] != null && Request.QueryString["year"].ToString() != "")
                {
                    ViewState["year"] = Request.QueryString["year"].ToString();
                    ddlYear.SelectedValue = ViewState["year"].ToString();
                }
                if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                {
                    ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();
                    UcVessel.SelectedVessel = ViewState["vesselid"].ToString();
                    UcVessel.bind();
                }
                if (Request.QueryString["fromdate"] != null && Request.QueryString["fromdate"].ToString() != "")
                {
                    ViewState["fromdate"] = Request.QueryString["fromdate"].ToString();
                    txtfromdate.Text = ViewState["fromdate"].ToString();
                }
                if (Request.QueryString["todate"] != null && Request.QueryString["todate"].ToString() != "")
                {
                    ViewState["todate"] = Request.QueryString["todate"].ToString();
                    txtAsOnDate.Text = ViewState["todate"].ToString();
                }
               
                //    if (Request.QueryString["wagetrackerid"] != null && Request.QueryString["wagetrackerid"].ToString() != "")
                //{
                //    ListPortageBill();
                //    ViewState["wagetrackerid"] = Request.QueryString["wagetrackerid"].ToString();
                //    ddlHistory.SelectedValue = ViewState["wagetrackerid"].ToString();
                //}
               

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                {
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    UcVessel.Enabled = false;
                    UcVessel.bind();
                }
                BindDate();
                DataSet ds = PhoenixCrewOffshoreWageTracker.ListWageTrackerPeriod(General.GetNullableInteger(UcVessel.SelectedVessel)
               , General.GetNullableDateTime(txtfromdate.Text), General.GetNullableDateTime(txtAsOnDate.Text), General.GetNullableGuid(ddlHistory.SelectedValue));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (txtfromdate.Text == "" || txtAsOnDate.Text == "")
                    {

                        txtfromdate.Text = ds.Tables[0].Rows[0]["FLDSTARTDATE"].ToString();
                        txtAsOnDate.Text = ds.Tables[0].Rows[0]["FLDENDDATE"].ToString();
                    }
                }
                ListPortageBill();
                BindDate();

                //ucCommonToolTip.Screen = "CrewOffshore/CrewOffshoreWageTrackerTotal.aspx?vlsid=" + UcVessel.SelectedVessel + "&date=" + txtfromdate.Text + "~" + txtAsOnDate.Text;
            }
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("javascript:openNewWindow('chml','','"+Session["sitepath"]+"/CrewOffshore/CrewOffshoreWageTrackerTotal.aspx?vesselid=" + UcVessel.SelectedVessel + "&month=" + ddlMonth.SelectedValue
                                         + "&year=" + ddlYear.SelectedValue + "&date=" + txtfromdate.Text + "~" + txtAsOnDate.Text + "'); return false;", "Detail", "<i class=\"fas fa-clipboard-list\"></i>", "DETAIL");
            //toolbar1.AddImageButton("../CrewOffshore/CrewOffshoreWageTrackerTotal.aspx", "Export to Excel", "icon_xls.png", "DETAIL");
            btndetail.AccessRights = this.ViewState;
            btndetail.MenuList = toolbar1.Show();
            //BindData();
            //RecalcluateTotal();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ImgPOPickList_Click(object sender, EventArgs e)
    {



    }

    protected void CrewMenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SUMMARY"))
            {
                Response.Redirect("../CrewOffshore/CrewOffshoreWageTrackerSummary.aspx?vesselid=" + UcVessel.SelectedVessel
                                         + "&month=" + ddlMonth.SelectedValue
                                         + "&year=" + ddlYear.SelectedValue
                                        + "&fromdate=" + txtfromdate.Text
                                        + "&todate=" + txtAsOnDate.Text
                                        + "&wagetrackerid=" + ddlHistory.SelectedValue);
                CrewTrainingMenu.SelectedMenuIndex = 0;
            }
            else if (CommandName.ToUpper().Equals("WAGETRACKER"))
            {
                Response.Redirect("../CrewOffshore/CrewOffshoreWageTracker.aspx?vesselid=" + UcVessel.SelectedVessel
                                             + "&month=" + ddlMonth.SelectedValue
                                             + "&year=" + ddlYear.SelectedValue
                                          + "&fromdate=" + txtfromdate.Text
                                        + "&todate=" + txtAsOnDate.Text
                                        + "&wagetrackerid=" + ddlHistory.SelectedValue);
                CrewTrainingMenu.SelectedMenuIndex = 1;

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void CrewTrainingMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("LOCK"))
            {
                //Response.Redirect("../CrewOffshore/CrewOffshoreTrainingMatrixHistoryList.aspx?matrixid=" + ViewState["MATRIXID"].ToString());
                ViewState["lockunlock"] = 1;
                //ucConfirm.Visible = true;
                //ucConfirm.Text = "You will not be able to sign on/off a person in this month after the wage tracker is locked. Are you sure to continue?";
                //return;
                RadWindowManager1.RadConfirm("You will not be able to sign on/off a person in this month after the wage tracker is locked. Are you sure to continue?", "confirm", 320, 150, null, "Confirm");
            }
            else if (CommandName.ToUpper().Equals("UNLOCK"))
            {
                ViewState["lockunlock"] = 2;
                //ucConfirm.Visible = true;
                //ucConfirm.Text = "Are you sure to unlock the wage tracker for the selected month?";
                //return;
                RadWindowManager1.RadConfirm("Are you sure to unlock the wage tracker for the selected month?", "confirm", 320, 150, null, "Confirm");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            //UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            //if (ucCM.confirmboxvalue == 1)
            //{
                if (ViewState["lockunlock"].ToString().Equals("1"))
                {
                    Guid? newid = null;

                    PhoenixCrewOffshoreWageTracker.WageTrackerInsert(General.GetNullableInteger(UcVessel.SelectedVessel)
                            , General.GetNullableDateTime(txtfromdate.Text), General.GetNullableDateTime(txtAsOnDate.Text)
                            , General.GetNullableDecimal(txtMonthlyBudget.Text), General.GetNullableDecimal(txtBudget.Text)
                            , General.GetNullableDecimal(txtActual.Text), General.GetNullableDecimal(txtVariance.Text)
                            , TravelWagesTotal, OnboardDaysTotal, OnboardWagesTotal, SignoffTravelDaysTotal
                            , TravelAllowanceTotal, ReimbursementsTotal, WagesTotal, ref newid, DeductionsTotal, DPAllowanceTotal
                            , General.GetNullableDateTime(txtAsOnDate.Text)
                            ,General.GetNullableInteger(lblcurrency.Text)
                            );

                    foreach (GridDataItem gvr in gvWageTracker.Items)
                    {
                        RadLabel lblSignonoffID = (RadLabel)gvr.FindControl("lblSignonoffID");
                        RadLabel lblBudgetedWage = (RadLabel)gvr.FindControl("lblBudgetedWage");
                        RadLabel lblTravelDays = (RadLabel)gvr.FindControl("lblTravelDays");
                        RadLabel lblTravelWages = (RadLabel)gvr.FindControl("lblTravelWages");
                        RadLabel lblOnboardDays = (RadLabel)gvr.FindControl("lblOnboardDays");
                        RadLabel lblOnboardWages = (RadLabel)gvr.FindControl("lblOnboardWages");
                        RadLabel lblSignoffTravelDays = (RadLabel)gvr.FindControl("lblSignoffTravelDays");
                        RadLabel lblTravelAllowance = (RadLabel)gvr.FindControl("lblTravelAllowance");
                        RadLabel lblReimbursements = (RadLabel)gvr.FindControl("lblReimbursements");
                        RadLabel lblTotalWages = (RadLabel)gvr.FindControl("lblTotalWages");
                        RadLabel lblRemarks = (RadLabel)gvr.FindControl("lblRemarks");
                        RadLabel lblDeductions = (RadLabel)gvr.FindControl("lblDeductions");
                        RadLabel lblDPAllowance = (RadLabel)gvr.FindControl("lblDPAllowance");
                        RadLabel lblActualWages = (RadLabel)gvr.FindControl("lblActualWages");
                        RadLabel lblDailyDPAllowance = (RadLabel)gvr.FindControl("lblDailyDPAllowance");
                        RadLabel lblTravelEndDate = (RadLabel)gvr.FindControl("lblTravelEndDate");
                        RadLabel lblcurrencyid = (RadLabel)gvr.FindControl("lblcurrencyid");
                        

                        PhoenixCrewOffshoreWageTracker.WageTrackerEmployeeInsert(newid
                                                            , General.GetNullableInteger(lblSignonoffID.Text)
                                                            , General.GetNullableDecimal(lblBudgetedWage.Text)
                                                            , General.GetNullableInteger(lblTravelDays.Text)
                                                            , General.GetNullableDecimal(lblTravelWages.Text)
                                                            , General.GetNullableInteger(lblOnboardDays.Text)
                                                            , General.GetNullableDecimal(lblOnboardWages.Text)
                                                            , General.GetNullableDecimal(lblTravelAllowance.Text)
                                                            , General.GetNullableDecimal(lblReimbursements.Text)
                                                            , General.GetNullableDecimal(lblTotalWages.Text)
                                                            , General.GetNullableString(lblRemarks.Text)
                                                            , General.GetNullableDecimal(lblDeductions.Text)
                                                            , General.GetNullableDecimal(lblDPAllowance.Text)
                                                            , General.GetNullableInteger(UcVessel.SelectedVessel)
                                                            , General.GetNullableInteger(lblActualWages.Text)
                                                            , General.GetNullableInteger(lblDailyDPAllowance.Text)
                                                            , General.GetNullableInteger(lblSignoffTravelDays.Text)
                                                            , General.GetNullableDateTime(lblTravelEndDate.Text)
                                                            , General.GetNullableDateTime(txtfromdate.Text)
                                                            , General.GetNullableDateTime(txtAsOnDate.Text)
                                                            , General.GetNullableInteger(lblcurrencyid.Text)
                                                            );
                    }

                    PhoenixCrewOffshoreWageTracker.EarningDeductionLock(General.GetNullableInteger(UcVessel.SelectedVessel)
                            , General.GetNullableDateTime(txtAsOnDate.Text), newid
                            , General.GetNullableDateTime(txtfromdate.Text)
                            , General.GetNullableDateTime(txtAsOnDate.Text));

                    ucStatus.Text = "Wage tracker is locked Successfully.";
                    ListPortageBill();
                }

                if (ViewState["lockunlock"].ToString().Equals("2"))
                {
                    RadLabel lblWageTrackerID = (RadLabel)gvWageTracker.Items[0].FindControl("lblWageTrackerID");
                    string wagetrackerid = "";
                    if (lblWageTrackerID != null)
                        wagetrackerid = lblWageTrackerID.Text;

                    PhoenixCrewOffshoreWageTracker.EarningDeductionUnLock(General.GetNullableInteger(UcVessel.SelectedVessel)
                        , General.GetNullableGuid(ddlHistory.SelectedValue));

                    PhoenixCrewOffshoreWageTracker.DeleteWageTracker(General.GetNullableGuid(ddlHistory.SelectedValue));

                    ucStatus.Text = "Wage tracker is unlocked Successfully.";
                    ListPortageBill();

                }

                TravelDaysTotal = 0;
                TravelWagesTotal = 0;
                OnboardDaysTotal = 0;
                OnboardWagesTotal = 0;
                SignoffTravelDaysTotal = 0;
                TravelAllowanceTotal = 0;
                ReimbursementsTotal = 0;
                WagesTotal = 0;
                //WagesTotalrc = 0;
                DeductionsTotal = 0;
                DPAllowanceTotal = 0;
                BindData();
                RecalcluateTotal();
          //  }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindYear()
    {
        for (int i = (DateTime.Today.Year - 4); i <= (DateTime.Today.Year); i++)
        {
            RadComboBoxItem li = new RadComboBoxItem(i.ToString(), i.ToString());
            ddlYear.Items.Add(li);
        }
        ddlYear.DataBind();
        ddlYear.SelectedValue = DateTime.Today.Year.ToString();
    }
    protected void BindDate()
    {
        DateTime fromdate = new DateTime();
        fromdate = Convert.ToDateTime("01/" + ddlMonth.SelectedValue + "/" + ddlYear.SelectedValue);
        txtfromdate.Text = "01/" + ddlMonth.SelectedValue + "/" + ddlYear.SelectedValue;

        if (ddlMonth.SelectedValue == DateTime.Today.Month.ToString() && ddlYear.SelectedValue == DateTime.Today.Year.ToString())
            txtAsOnDate.Text = DateTime.Today.ToString();
        else
        {
            txtAsOnDate.Text = LastDayOfMonthFromDateTime(fromdate);
        }
    }
    protected void ddlHistory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreWageTracker.ListWageTrackerPeriod(General.GetNullableInteger(UcVessel.SelectedVessel)
            , General.GetNullableDateTime(txtfromdate.Text), General.GetNullableDateTime(txtAsOnDate.Text), General.GetNullableGuid(ddlHistory.SelectedValue));

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlMonth.SelectedValue = ds.Tables[0].Rows[0]["FLDMONTH"].ToString();
                ddlYear.SelectedValue = ds.Tables[0].Rows[0]["FLDYEAR"].ToString();
                BindDate();
                //txtfromdate.Text = ds.Tables[0].Rows[0]["FLDSTARTDATE"].ToString();
                //txtAsOnDate.Text = ds.Tables[0].Rows[0]["FLDENDDATE"].ToString();
            }

            BindData();
            RecalcluateTotal();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDate();
        BindData();
        RecalcluateTotal();
        ddlHistory.SelectedValue = "";
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDate();
        BindData();
        RecalcluateTotal();
        ddlHistory.SelectedValue = "";

    }
    //protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    BindDate();
    //    BindData();
    //    RecalcluateTotal();
    //}
    //protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    BindDate();
    //    BindData();
    //    RecalcluateTotal();
    //}
    private string LastDayOfMonthFromDateTime(DateTime dateTime)
    {
        DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
        return firstDayOfTheMonth.AddMonths(1).AddDays(-1).ToString();
    }
    private string FirstDayOfMonthFromDateTime(DateTime dateTime)
    {
        DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
        return firstDayOfTheMonth.ToString();
    }
    protected void txtAsOnDate_TextChanged(object sender, EventArgs e)
    {
        UserControlDate txtAsOnDate = (UserControlDate)sender;
        DateTime dt = Convert.ToDateTime(txtAsOnDate.Text);
        ddlMonth.SelectedValue = dt.Month.ToString();
        ddlYear.SelectedValue = dt.Year.ToString();
        BindData();
    }
    protected void txtAsfromOnDate_TextChanged(object sender, EventArgs e)
    {
        //    UserControlDate txtAsOnDate = (UserControlDate)sender;
        //    DateTime dt = Convert.ToDateTime(txtAsOnDate.Text);
        //    ddlMonth.SelectedValue = dt.Month.ToString();
        //    ddlYear.SelectedValue = dt.Year.ToString();
        BindData();
    }
    //private bool IsValidEarningDeduction(string tomonth, string toyear, string frommonth, string fromyear)
    //{
    //    ucError.HeaderMessage = "Please provide the following required information";
    //    if (tomonth == null || tomonth.ToString() == "" || toyear == null || toyear.ToString() == "")
    //    {
    //        ucError.ErrorMessage = "To Date is required.";
    //    }
    //    if (frommonth == null || frommonth.ToString() == "" || fromyear == null || fromyear.ToString() == "")
    //    {
    //        ucError.ErrorMessage = "From Date is required.";
    //    }
    //    if (frommonth.ToString() != tomonth.ToString() && fromyear.ToString() == toyear.ToString())
    //    {
    //        ucError.ErrorMessage = "From Date month and To date moth should be the same";
    //    }
    //    return (!ucError.IsError);
    //}

    protected void MenuOffshoreWageTracker_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (CommandName.ToUpper().Equals("FIND"))
            {
                TravelDaysTotal = 0;
                TravelWagesTotal = 0;
                OnboardDaysTotal = 0;
                OnboardWagesTotal = 0;
                SignoffTravelDaysTotal = 0;
                TravelAllowanceTotal = 0;
                ReimbursementsTotal = 0;
                WagesTotal = 0;
                //WagesTotalrc = 0;
                DeductionsTotal = 0;
                DPAllowanceTotal = 0;
                BindData();
                RecalcluateTotal();
                gvWageTracker.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        string[] alColumns = {"FLDEMPLOYEENAME", "FLDEMAIL","FLDRANKNAME","FLDCURRENCYCODE", "FLDBUDGETEDWAGE", "FLDACTUALWAGE","FLDDAILYDPALLOWANCE",
                                       "FLDCONTRACTCOMMENCEMENTDATE", "FLDCONTRACTCANCELDATE", "FLDSIGNONDATE", "FLDSIGNOFFDATE" ,
                                       "FLDDAYSFORTRAVEL", "FLDWAGESFORTRAVEL", "FLDDAYSONBOARD", "FLDWAGESONBOARD", "FLDESTIMATEDTRAVELENDDATE","FLDTRAVELDAYSSIGNOFF", "FLDTRAVELALLOWANCE", "FLDDPALLOWANCE",
                                       "FLDREIMBURSEMENTS", "FLDDEDUCTIONS", "FLDTOTALWAGE", "FLDREMARKS" };
        string[] alCaptions = {"Name", "E.mail ID",  "Rank","Currency", "Budgeted Daily rate", "Actual Daily rate", "Daily DP Allowance", "Contract Start Date",
                                        "Contract Cancellation Date", "Date Signed on", "Date Signed off",  "Days for Travel",
                                        "Wages for Travel", "Days Onboard", "Wages Onboard","Estimated Travel End Date", "Travel Days for sign off", "Travel Allowance", "Total DP Allowance","Earnings",
                                        "Deductions", "Total Wages", "Remarks"};

        DataSet ds = PhoenixCrewOffshoreWageTracker.ListWageTracker(General.GetNullableInteger(UcVessel.SelectedVessel)
        , General.GetNullableDateTime(txtfromdate.Text), General.GetNullableDateTime(txtAsOnDate.Text), General.GetNullableGuid(ddlHistory.SelectedValue));

        DataTable dt = ds.Tables[0];

        //General.ShowFilterCriteriaInExcel(ds, filtercaptions, filtercolumns);
        //General.ShowExcel("Vessel Wage Tracker", dt, alColumns, alCaptions, null, null);

        Response.AddHeader("Content-Disposition", "attachment; filename=WageTracker.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td></td><td><b>Vessel</b></td><td>" + dt.Rows[0]["FLDVESSELNAME"].ToString() + "</td><td></td>");
        Response.Write("<td><b>Month</b></td><td>" + dt.Rows[0]["FLDMONTHNAME"].ToString() + "</td>");
        Response.Write("<td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>");
        Response.Write("</tr>");
        Response.Write("<tr><td></td><td><b>Monthly Budget</b></td><td align='left'>" + dt.Rows[0]["FLDMONTHLYBUDGET"].ToString() + "</td><td></td>");
        Response.Write("<td><b>Year</b></td><td align='left'>" + dt.Rows[0]["FLDYEAR"].ToString() + "</td><td></td>");
        Response.Write("<td><b>Actual</b></td><td align='left'>" + dt.Rows[0]["FLDACTUALTOTAL"].ToString() + "</td>");
        Response.Write("<td><b>Budget</b></td><td align='left'>" + dt.Rows[0]["FLDAVAILABLEBUDGET"].ToString() + "</td>");
        Response.Write("<td><b>Variance</b></td><td align='left'>" + dt.Rows[0]["FLDVARIANCE"].ToString() + "</td>");
        Response.Write("<td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");

        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
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
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        //  Response.Write("<tr>");
        // Response.Write("<td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>");
        //    Response.Write("<td><b>" + TravelWagesTotal .ToString() + "</b></td>");
        //    Response.Write("<td><b>" + OnboardDaysTotal.ToString() + "</b></td>");
        //    Response.Write("<td><b>" + OnboardWagesTotal.ToString() + "</b></td>");
        //    Response.Write("<td></td>");
        //    Response.Write("<td><b>" + SignoffTravelDaysTotal.ToString() + "</b></td>");
        //    Response.Write("<td><b>" + TravelAllowanceTotal.ToString() + "</b></td>");
        //    Response.Write("<td><b>" + DPAllowanceTotal.ToString() + "</b></td>");
        //    Response.Write("<td><b>" + ReimbursementsTotal.ToString() + "</b></td>");
        //    Response.Write("<td><b>" + DeductionsTotal.ToString() + "</b></td>");
        //    Response.Write("<td><b>" + WagesTotal.ToString() + "</b></td>");
        //Response.Write("<td></td></tr>");
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        TravelDaysTotal = 0;
        TravelWagesTotal = 0;
        OnboardDaysTotal = 0;
        OnboardWagesTotal = 0;
        SignoffTravelDaysTotal = 0;
        TravelAllowanceTotal = 0;
        ReimbursementsTotal = 0;
        WagesTotal = 0;
        //WagesTotalrc = 0;
        DeductionsTotal = 0;
        DPAllowanceTotal = 0;
        ListPortageBill();
        BindData();
        RecalcluateTotal();
    }

    public void BindData()
    {
        string[] alColumns = { "FLDEMPLOYEENAME", "FLDEMAIL","FLDRANKNAME","FLDCURRENCYCODE", "FLDBUDGETEDWAGE", "FLDACTUALWAGE","FLDDAILYDPALLOWANCE",
                               "FLDCONTRACTCOMMENCEMENTDATE", "FLDCONTRACTCANCELDATE", "FLDSIGNONDATE", "FLDSIGNOFFDATE" ,
                               "FLDDAYSFORTRAVEL", "FLDWAGESFORTRAVEL", "FLDDAYSONBOARD", "FLDWAGESONBOARD", "FLDESTIMATEDTRAVELENDDATE","FLDTRAVELDAYSSIGNOFF", "FLDTRAVELALLOWANCE", "FLDDPALLOWANCE",
                               "FLDREIMBURSEMENTS", "FLDDEDUCTIONS", "FLDTOTALWAGE", "FLDREMARKS" };
        string[] alCaptions = {  "Name", "E.mail ID",  "Rank","Currency", "Budgeted Daily rate", "Actual Daily rate", "Daily DP Allowance", "Contract Start Date",
                                "Contract Cancellation Date", "Date Signed on", "Date Signed off",  "Days for Travel",
                                "Wages for Travel", "Days Onboard", "Wages Onboard","Estimated Travel End Date", "Travel Days for sign off", "Travel Allowance", "Total DP Allowance","Earnings",
                                "Deductions", "Total Wages", "Remarks"};
        try
        {
            
            DataSet ds = PhoenixCrewOffshoreWageTracker.ListWageTracker(General.GetNullableInteger(UcVessel.SelectedVessel)
                , General.GetNullableDateTime(txtfromdate.Text), General.GetNullableDateTime(txtAsOnDate.Text), General.GetNullableGuid(ddlHistory.SelectedValue));
            //, General.GetNullableInteger(ddlMonth.SelectedValue), General.GetNullableInteger(ddlYear.SelectedValue), General.GetNullableDateTime(txtAsOnDate.Text));

            BindBudget(ds);

            General.SetPrintOptions("gvWageTracker", "Vessel Wage Tracker", alCaptions, alColumns, ds);
            gvWageTracker.DataSource = ds.Tables[0];
           
            lblcurrency.Text = ds.Tables[0].Rows[0]["FLDTOTALWAGESINRPTCURRENCY"].ToString();
            //lblTotalWagesrptHeader.Text = "Total Wages in" + "(" + ds.Tables[0].Rows[0]["FLDREPORTINGCURRENCY"].ToString() + ")";
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindBudget(DataSet ds)
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtMonthlyBudget.Text = dr["FLDMONTHLYBUDGET"].ToString();
            txtActual.Text = dr["FLDACTUALTOTAL"].ToString();
            txtBudget.Text = dr["FLDAVAILABLEBUDGET"].ToString();
            txtVariance.Text = dr["FLDVARIANCE"].ToString();
        }
        else
        {
            txtMonthlyBudget.Text = "";
            txtActual.Text = "";
            txtBudget.Text = "";
            txtVariance.Text = "";
        }
    }
    //protected void InsertWagetracker()
    //{
    //    Guid? newid = null;

    //    if (UcVessel.SelectedVessel != null && UcVessel.SelectedVessel.ToUpper() != "DUMMY" && UcVessel.SelectedVessel != "" && ViewState["WAGETRACKERID"].ToString() == "")
    //    {
    //        if (ddlMonth.SelectedValue != "" && ddlMonth.SelectedValue != null)
    //        {
    //            if (ddlYear.SelectedValue != "" && ddlYear.SelectedValue != null)
    //            {
    //                PhoenixCrewOffshoreWageTracker.WageTrackerInsert(General.GetNullableInteger(UcVessel.SelectedVessel)
    //                        , General.GetNullableInteger(ddlMonth.SelectedValue), General.GetNullableInteger(ddlYear.SelectedValue)
    //                        , General.GetNullableDecimal(txtMonthlyBudget.Text), General.GetNullableDecimal(txtBudget.Text)
    //                        , General.GetNullableDecimal(txtActual.Text), General.GetNullableDecimal(txtVariance.Text)
    //                        , TravelWagesTotal, OnboardDaysTotal, OnboardWagesTotal, SignoffTravelDaysTotal
    //                        , TravelAllowanceTotal, ReimbursementsTotal, WagesTotal, ref newid, DeductionsTotal, DPAllowanceTotal, 0);

    //                ViewState["WAGETRACKERID"] = newid;

    //                //RadLabel lblSignonoffID = (RadLabel)gvr.FindControl("lblSignonoffID");
    //                //RadLabel lblBudgetedWage = (RadLabel)gvr.FindControl("lblBudgetedWage");
    //                //RadLabel lblTravelDays = (RadLabel)gvr.FindControl("lblTravelDays");
    //                //RadLabel lblTravelWages = (RadLabel)gvr.FindControl("lblTravelWages");
    //                //RadLabel lblOnboardDays = (RadLabel)gvr.FindControl("lblOnboardDays");
    //                //RadLabel lblOnboardWages = (RadLabel)gvr.FindControl("lblOnboardWages");
    //                //RadLabel lblSignoffTravelDays = (RadLabel)gvr.FindControl("lblSignoffTravelDays");
    //                //RadLabel lblTravelAllowance = (RadLabel)gvr.FindControl("lblTravelAllowance");
    //                //RadLabel lblReimbursements = (RadLabel)gvr.FindControl("lblReimbursements");
    //                //RadLabel lblTotalWages = (RadLabel)gvr.FindControl("lblTotalWages");
    //                //RadLabel lblRemarks = (RadLabel)gvr.FindControl("lblRemarks");
    //                //RadLabel lblDeductions = (RadLabel)gvr.FindControl("lblDeductions");
    //                //RadLabel lblDPAllowance = (RadLabel)gvr.FindControl("lblDPAllowance");

    //                PhoenixCrewOffshoreWageTracker.WageTrackerEmployeeInsert(newid                                                          
    //                                                    , General.GetNullableDateTime(txtAsOnDate.Text)
    //                                                    , General.GetNullableInteger(UcVessel.SelectedVessel)
    //                                                    , General.GetNullableInteger(ddlMonth.SelectedValue)
    //                                                    , General.GetNullableInteger(ddlYear.SelectedValue)
    //                                                    );

    //                PhoenixCrewOffshoreWageTracker.WageTrackerEmployeeBulkUpdate(newid
    //                                                    , General.GetNullableDateTime(txtAsOnDate.Text)
    //                                                    , General.GetNullableInteger(UcVessel.SelectedVessel)
    //                                                    , General.GetNullableInteger(ddlMonth.SelectedValue)
    //                                                    , General.GetNullableInteger(ddlYear.SelectedValue)
    //                                                    );
    //               // RecalcluateTotal();
    //            }
    //        }
    //    }
    //}

    protected void RecalcluateTotal()
    {
        TravelDaysTotal = 0;
        TravelWagesTotal = 0;
        OnboardDaysTotal = 0;
        OnboardWagesTotal = 0;
        SignoffTravelDaysTotal = 0;
        TravelAllowanceTotal = 0;
        ReimbursementsTotal = 0;
        WagesTotal = 0;
        //WagesTotalrc = 0;
        DeductionsTotal = 0;
        DPAllowanceTotal = 0;
       // foreach (GridDataItem gvr in gvWageTracker.Items)
       for(int i=0;i<=gvWageTracker.Items.Count-1;i++)
        {
            RadLabel lblTravelDays = (RadLabel)gvWageTracker.Items[i].FindControl("lblTravelDays");
            if (lblTravelDays != null)
            {
                if (lblTravelDays.Text != string.Empty)
                    TravelDaysTotal = TravelDaysTotal + int.Parse(lblTravelDays.Text);
            }

            RadLabel lblTravelWages = (RadLabel)gvWageTracker.Items[i].FindControl("lblTravelWages");
            if (lblTravelWages != null)
            {
                if (lblTravelWages.Text != string.Empty)
                    TravelWagesTotal = TravelWagesTotal + Convert.ToDecimal(lblTravelWages.Text);
            }

            RadLabel lblOnboardDays = (RadLabel)gvWageTracker.Items[i].FindControl("lblOnboardDays");
            if (lblOnboardDays != null)
            {
                if (lblOnboardDays.Text != string.Empty)
                    OnboardDaysTotal = OnboardDaysTotal + int.Parse(lblOnboardDays.Text);
            }

            RadLabel lblOnboardWages = (RadLabel)gvWageTracker.Items[i].FindControl("lblOnboardWages");
            if (lblOnboardWages != null)
            {
                if (lblOnboardWages.Text != string.Empty)
                    OnboardWagesTotal = OnboardWagesTotal + Convert.ToDecimal(lblOnboardWages.Text);
            }

            RadLabel lblSignoffTravelDays = (RadLabel)gvWageTracker.Items[i].FindControl("lblSignoffTravelDays");
            if (lblSignoffTravelDays != null)
            {
                if (lblSignoffTravelDays.Text != string.Empty)
                    SignoffTravelDaysTotal = SignoffTravelDaysTotal + int.Parse(lblSignoffTravelDays.Text);
            }

            RadLabel lblTravelAllowance = (RadLabel)gvWageTracker.Items[i].FindControl("lblTravelAllowance");
            if (lblTravelAllowance != null)
            {
                if (lblTravelAllowance.Text != string.Empty)
                    TravelAllowanceTotal = TravelAllowanceTotal + Convert.ToDecimal(lblTravelAllowance.Text);
            }

            RadLabel lblReimbursements = (RadLabel)gvWageTracker.Items[i].FindControl("lblReimbursements");
            if (lblReimbursements != null)
            {
                if (lblReimbursements.Text != string.Empty)
                    ReimbursementsTotal = ReimbursementsTotal + Convert.ToDecimal(lblReimbursements.Text);
            }

            RadLabel lblDeductions = (RadLabel)gvWageTracker.Items[i].FindControl("lblDeductions");
            if (lblDeductions != null)
            {
                if (lblDeductions.Text != string.Empty)
                    DeductionsTotal = DeductionsTotal + Convert.ToDecimal(lblDeductions.Text);
            }

            RadLabel lblTotalWages = (RadLabel)gvWageTracker.Items[i].FindControl("lblTotalWages");
            if (lblTotalWages != null)
            {
                if (lblTotalWages.Text != string.Empty)
                    WagesTotal = WagesTotal + Convert.ToDecimal(lblTotalWages.Text);
            }
            //RadLabel lblTotalWagesrpt = (RadLabel)gvr.FindControl("lblTotalWagesrpt");
            //if (lblTotalWagesrpt != null)
            //{
            //    if (lblTotalWagesrpt.Text != string.Empty)
            //        WagesTotalrc = WagesTotalrc + Convert.ToDecimal(lblTotalWagesrpt.Text);
            //}
            
            RadLabel lblDPAllowance = (RadLabel)gvWageTracker.Items[i].FindControl("lblDPAllowance");
            if (lblDPAllowance != null)
            {
                if (lblDPAllowance.Text != string.Empty)
                    DPAllowanceTotal = DPAllowanceTotal + Convert.ToDecimal(lblDPAllowance.Text);
            }

            UserControlMaskNumber txtDPAllowance = (UserControlMaskNumber)gvWageTracker.Items[i].FindControl("txtDPAllowance");
            if (txtDPAllowance != null)
            {
                if (txtDPAllowance.Text != string.Empty)
                    DPAllowanceTotal = DPAllowanceTotal + Convert.ToDecimal(txtDPAllowance.Text);
            }
        }
    }

  
    private void ListPortageBill()
    {
        DataTable dt = PhoenixCrewOffshoreWageTracker.ListVesselWageTrackerHistory(General.GetNullableInteger(UcVessel.SelectedVessel));
        //DataTable dt = PhoenixVesselAccountsPortageBill.ListVesselPortageBillHistory(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        ddlHistory.DataSource = dt;
        ddlHistory.DataBind();
        ddlHistory.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
        //if (dt.Rows.Count > 0)
        //{
        //    txtAsOnDate.Text = General.GetNullableDateTime(dt.Rows[0]["FLDPBCLOSGINDATE"].ToString()).Value.AddDays(1).ToString();
        //}
    }


    protected void gvWageTracker_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        TravelDaysTotal = 0;
        TravelWagesTotal = 0;
        OnboardDaysTotal = 0;
        OnboardWagesTotal = 0;
        SignoffTravelDaysTotal = 0;
        TravelAllowanceTotal = 0;
        ReimbursementsTotal = 0;
        WagesTotal = 0;
        //WagesTotalrc = 0;
        DeductionsTotal = 0;
        DPAllowanceTotal = 0;
        BindData();
        RecalcluateTotal();

    }

    protected void gvWageTracker_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {

                RadLabel txtwagetrackeremployeeid = (RadLabel)e.Item.FindControl("txtwagetrackeremployeeid");
                UserControlMaskNumber txtActualWages = (UserControlMaskNumber)e.Item.FindControl("txtActualWages");
                RadLabel txtSignonoffID = (RadLabel)e.Item.FindControl("txtSignonoffID");
                RadLabel lblBudgetedWage = (RadLabel)e.Item.FindControl("lblBudgetedWage");
                RadLabel lblTravelDays = (RadLabel)e.Item.FindControl("lblTravelDays");
                RadLabel lblTravelWages = (RadLabel)e.Item.FindControl("lblTravelWages");
                RadLabel lblOnboardDays = (RadLabel)e.Item.FindControl("lblOnboardDays");
                RadLabel lblOnboardWages = (RadLabel)e.Item.FindControl("lblOnboardWages");
                RadLabel lblSignoffTravelDays = (RadLabel)e.Item.FindControl("lblSignoffTravelDays");
                RadLabel lblTravelAllowance = (RadLabel)e.Item.FindControl("lblTravelAllowance");
                RadLabel lblReimbursements = (RadLabel)e.Item.FindControl("lblReimbursements");
                RadLabel lblTotalWages = (RadLabel)e.Item.FindControl("lblTotalWages");
                RadLabel lblRemarks = (RadLabel)e.Item.FindControl("lblRemarks");
                RadLabel lblDeductions = (RadLabel)e.Item.FindControl("lblDeductions");
                UserControlMaskNumber txtDPAllowance = (UserControlMaskNumber)e.Item.FindControl("txtDPAllowance");
                UserControlDate ucTravelEndDateEdit = (UserControlDate)e.Item.FindControl("ucTravelEndDateEdit");

                RadLabel lblcrewplanidedit = (RadLabel)e.Item.FindControl("lblcrewplanidedit");
                RadLabel lblActualWagesEdit = (RadLabel)e.Item.FindControl("lblActualWagesEdit");
                RadLabel lblWageTrackerExtnIDEdit = (RadLabel)e.Item.FindControl("lblWageTrackerExtnIDEdit");

                string actualwage = "";
                if (lblcrewplanidedit != null && lblcrewplanidedit.Text != "")
                {
                    if (lblActualWagesEdit != null)
                        actualwage = lblActualWagesEdit.Text;
                }
                else
                {
                    if (txtActualWages != null)
                        actualwage = txtActualWages.Text;
                }

                if (lblWageTrackerExtnIDEdit != null && lblWageTrackerExtnIDEdit.Text != "")
                {
                    PhoenixCrewOffshoreWageTracker.WageTrackerEmployeeExtnUpdate(
                                                         General.GetNullableGuid(lblWageTrackerExtnIDEdit.Text)
                                                        , General.GetNullableInteger(txtSignonoffID.Text)
                                                        , General.GetNullableInteger(UcVessel.SelectedVessel)
                                                        , null
                                                        , null
                                                        , General.GetNullableDateTime(txtAsOnDate.Text)
                                                        , General.GetNullableGuid(lblcrewplanidedit.Text)
                                                        , General.GetNullableInteger(actualwage)
                                                        , General.GetNullableDecimal(txtDPAllowance.Text)
                                                        , General.GetNullableString(lblRemarks.Text)
                                                        , General.GetNullableDateTime(ucTravelEndDateEdit.Text)
                                                        , General.GetNullableDateTime(txtfromdate.Text)

                                                        );
                }
                else
                {
                    PhoenixCrewOffshoreWageTracker.WageTrackerEmployeeExtnInsert(
                                                         General.GetNullableInteger(txtSignonoffID.Text)
                                                        , General.GetNullableInteger(UcVessel.SelectedVessel)
                                                        , null
                                                        , null
                                                        , General.GetNullableDateTime(txtAsOnDate.Text)
                                                        , General.GetNullableGuid(lblcrewplanidedit.Text)
                                                        , General.GetNullableInteger(actualwage)
                                                        , General.GetNullableDecimal(txtDPAllowance.Text)
                                                        , General.GetNullableString(lblRemarks.Text)
                                                        , General.GetNullableDateTime(ucTravelEndDateEdit.Text)
                                                        , General.GetNullableDateTime(txtfromdate.Text)

                                                        );
                }

                BindData();
                RecalcluateTotal();
                gvWageTracker.Rebind();
                ucStatus.Text = "Wage tracker updated.";

            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWageTracker_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView dr1 = (DataRowView)e.Item.DataItem;
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            RadLabel lblcrewplanidedit = (RadLabel)e.Item.FindControl("lblcrewplanidedit");
            RadLabel lblActualWagesEdit = (RadLabel)e.Item.FindControl("lblActualWagesEdit");
            UserControlMaskNumber txtDPAllowance = (UserControlMaskNumber)e.Item.FindControl("txtDPAllowance");
            UserControlMaskNumber txtActualWages = (UserControlMaskNumber)e.Item.FindControl("txtActualWages");
            RadLabel lblWageTrackerID = (RadLabel)e.Item.FindControl("lblWageTrackerID");

            if (lblWageTrackerID != null && lblWageTrackerID.Text != "")
            {
                ViewState["WAGETRACKERID"] = lblWageTrackerID.Text;
            }
            else
            {
                ViewState["WAGETRACKERID"] = "";
            }

            if (lblcrewplanidedit != null && lblcrewplanidedit.Text != "")
            {
                if (txtActualWages != null)
                    txtActualWages.Visible = false;
                if (lblActualWagesEdit != null)
                    lblActualWagesEdit.Visible = true;
            }
            else
            {
                if (txtActualWages != null)
                    txtActualWages.Visible = true;
                if (lblActualWagesEdit != null)
                    lblActualWagesEdit.Visible = false;
            }
        }
        RecalcluateTotal();
    }
}
