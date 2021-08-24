using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using Telerik.Web.UI;

public partial class CrewOffshoreWageTrackerSummary : PhoenixBasePage
{
    public int TravelDaysTotal = 0;
    public decimal TravelWagesTotal = 0;
    public int OnboardDaysTotal = 0;
    public decimal OnboardWagesTotal = 0;
    public int SignoffTravelDaysTotal = 0;
    public decimal TravelAllowanceTotal = 0;
    public decimal ReimbursementsTotal = 0;
    public decimal WagesTotal = 0;
    public decimal DeductionsTotal = 0;
    public decimal DPAllowanceTotal = 0;
    public decimal TotalAvailableBudegt = 0;
    public decimal TotalActualWageTotal = 0;
    public decimal TotalVaraiance = 0;
    public decimal TotalVaraiancePercentage = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreWageTrackerSummary.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvWageTracker')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreWageTrackerSummary.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuOffshoreWageTracker.AccessRights = this.ViewState;
            MenuOffshoreWageTracker.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Wage Tracker Summary", "SUMMARY");
            toolbar.AddButton("Wage Tracker", "WAGETRACKER");
            
          
            CrewTrainingMenu.AccessRights = this.ViewState;
            CrewTrainingMenu.MenuList = toolbar.Show();
            CrewTrainingMenu.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {

                ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
                BindYear();
                BindDate();
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
                //if (Request.QueryString["wagetrackerid"] != null && Request.QueryString["wagetrackerid"].ToString() != "")
                //{
                //    ViewState["wagetrackerid"] = Request.QueryString["wagetrackerid"].ToString();
                //    ddlHistory.SelectedValue = ViewState["wagetrackerid"].ToString();
                //}
                if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                {
                    ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();
                    UcVessel.SelectedVessel = ViewState["vesselid"].ToString();
                    UcVessel.bind();
                }
                ViewState["lockunlock"] = "";

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    UcVessel.Enabled = false;
                    UcVessel.bind();
                }
                BindDate();
                DataSet ds = PhoenixCrewOffshoreWageTracker.ListWageTrackerPeriod(General.GetNullableInteger(UcVessel.SelectedVessel)
               , General.GetNullableDateTime(txtfromdate.Text), General.GetNullableDateTime(txtAsOnDate.Text), null);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (txtfromdate.Text == "" || txtAsOnDate.Text == "")
                    {
                        txtfromdate.Text = ds.Tables[0].Rows[0]["FLDSTARTDATE"].ToString();
                        txtAsOnDate.Text = ds.Tables[0].Rows[0]["FLDENDDATE"].ToString();
                    }
                }
                //ListPortageBill();
                BindDate();
            }

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //private void ListPortageBill()
    //{
    //    DataTable dt = PhoenixCrewOffshoreWageTracker.ListVesselWageTrackerHistory(General.GetNullableInteger(UcVessel.SelectedVessel));
    //    //DataTable dt = PhoenixVesselAccountsPortageBill.ListVesselPortageBillHistory(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
    //    ddlHistory.DataSource = dt;
    //    ddlHistory.DataBind();
    //    ddlHistory.Items.Insert(0, new ListItem("--Select--", ""));
    //    //if (dt.Rows.Count > 0)
    //    //{
    //    //    txtAsOnDate.Text = General.GetNullableDateTime(dt.Rows[0]["FLDPBCLOSGINDATE"].ToString()).Value.AddDays(1).ToString();
    //    //}
    //}

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
    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDate();
        BindData();
        gvWageTracker.Rebind();
        //ddlHistory.SelectedValue = "";
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindDate();
            BindData();
            gvWageTracker.Rebind();
            //ddlHistory.SelectedValue = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void txtAsOnDate_TextChanged(object sender, EventArgs e)
    {
        UserControlDate txtAsOnDate = (UserControlDate)sender;
        DateTime dt = Convert.ToDateTime(txtAsOnDate.Text);
        ddlMonth.SelectedValue = dt.Month.ToString();
        ddlYear.SelectedValue = dt.Year.ToString();
        BindData();
        gvWageTracker.Rebind();
    }
    private string LastDayOfMonthFromDateTime(DateTime dateTime)
    {
        DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
        return firstDayOfTheMonth.AddMonths(1).AddDays(-1).ToString();
    }

    protected void CrewTrainingMenu_TabStripCommand(object sender, EventArgs e)
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
                                        );
                CrewTrainingMenu.SelectedMenuIndex = 0;
            }
            else if (CommandName.ToUpper().Equals("WAGETRACKER"))
            {

                Response.Redirect("../CrewOffshore/CrewOffshoreWageTracker.aspx?vesselid=" + UcVessel.SelectedVessel
                                        + "&month=" + ddlMonth.SelectedValue
                                        + "&year=" + ddlYear.SelectedValue
                                        + "&fromdate=" + txtfromdate.Text
                                        + "&todate=" + txtAsOnDate.Text
                                        );
                CrewTrainingMenu.SelectedMenuIndex = 1;

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


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
                DeductionsTotal = 0;
                DPAllowanceTotal = 0;
                BindData();
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
        string[] alColumns = { "FLDVESSELNAME", "FLDCURRENCYCODE", "FLDAVAILABLEBUDGET", "FLDACTUALWAGETOTAL", "FLDVARIANCE", "FLDVARIANCEPERCENTAGE" };
        string[] alCaptions = { "Vessel Name", "Currency", "Available Budget", "Actual Wage", "Variance", "Variance Percentage" };

        string[] filtercolumns = { "FLDVESSELNAME", "FLDMONTH", "FLDYEAR", "FLDTODATE" };
        string[] filtercaptions = { "Vessel", "Report for the Month of ", "Year ", "Date:" };

        DataSet ds = PhoenixCrewOffshoreWageTracker.ListWageTrackerSummary(General.GetNullableInteger(UcVessel.SelectedVessel)
               , General.GetNullableDateTime(txtfromdate.Text), General.GetNullableDateTime(txtAsOnDate.Text), null);

        DataTable dt = ds.Tables[0];

        General.ShowFilterCriteriaInExcel(ds, filtercaptions, filtercolumns);
        General.ShowExcel("Wage Tracker Summary", dt, alColumns, alCaptions, null, null);
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
        DeductionsTotal = 0;
        DPAllowanceTotal = 0;
        //ListPortageBill();
        BindData();
        gvWageTracker.Rebind();
    }

    public void BindData()
    {
        string[] alColumns = { "FLDVESSELNAME", "FLDCURRENCYCODE", "FLDAVAILABLEBUDGETINRPC", "FLDACTUALWAGETOTALRPC", "FLDTOTALVARIANCERPC", "FLDVARIANCEPERCENTAGE" };
        string[] alCaptions = { "Vessel Name", "Currency", "Available Budget", "Actual Wage", "Variance", "Variance Percentage" };

        try
        {
            DataSet ds = PhoenixCrewOffshoreWageTracker.ListWageTrackerSummary(General.GetNullableInteger(UcVessel.SelectedVessel)
                , General.GetNullableDateTime(txtfromdate.Text), General.GetNullableDateTime(txtAsOnDate.Text), null);

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                Response.Redirect("../CrewOffshore/CrewOffshoreWageTracker.aspx?vesselid=" + UcVessel.SelectedVessel
                                        + "&month=" + ddlMonth.SelectedValue
                                        + "&year=" + ddlYear.SelectedValue
                                        + "&fromdate=" + txtfromdate.Text
                                        + "&todate=" + txtAsOnDate.Text
                                        );
                CrewTrainingMenu.SelectedMenuIndex = 1;
                return;
            }
            General.SetPrintOptions("gvWageTracker", "Wage Tracker Summary", alCaptions, alColumns, ds);
            gvWageTracker.DataSource = ds.Tables[0];

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
  
     


    protected void gvWageTracker_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvWageTracker_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
          
            RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblvesselid");

            if (e.CommandName.ToUpper() == "WAGETRACKER")
            {
                Response.Redirect("../CrewOffshore/CrewOffshoreWageTracker.aspx?vesselid=" + lblVesselid.Text
                                        + "&month=" + ddlMonth.SelectedValue
                                        + "&year=" + ddlYear.SelectedValue
                                        + "&fromdate=" + txtfromdate.Text
                                        + "&todate=" + txtAsOnDate.Text
                                        );

            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
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
            LinkButton cmdWageTracker = (LinkButton)e.Item.FindControl("cmdWageTracker");
            if (cmdWageTracker != null)
            {
                //cmdWageTracker.Attributes.Add("onclick", "parent.Openpopup('chml', '', '../CrewOffshore/CrewOffshoreWageTracker.aspx?vesselid=" + UcVessel.SelectedVessel
                //+ "&month=" + ddlMonth.SelectedValue
                //+ "&year=" + ddlYear.SelectedValue
                //+ "&asondate=" + txtAsOnDate.Text+"');return false;");
                //Response.Redirect("../CrewOffshore/CrewOffshoreWageTracker.aspx?vesselid=" + UcVessel.SelectedVessel
                //+ "&month=" + ddlMonth.SelectedValue
                //+ "&year=" + ddlYear.SelectedValue
                //+ "&asondate=" + txtAsOnDate.Text);
            }
            if (dr1.Row["FLDTOTALAVAILABLEBUDGET"].ToString() != "")
                TotalAvailableBudegt = Convert.ToDecimal(dr1.Row["FLDTOTALAVAILABLEBUDGET"].ToString());
            else
                TotalAvailableBudegt = 0;
            if (dr1.Row["FLDTOTALACTUALWAGETOTAL"].ToString() != "")
                TotalActualWageTotal = Convert.ToDecimal(dr1.Row["FLDTOTALACTUALWAGETOTAL"].ToString());
            else
                TotalActualWageTotal = 0;
            if (dr1.Row["FLDTOTALVARIANCE"].ToString() != "")
                TotalVaraiance = Convert.ToDecimal(dr1.Row["FLDTOTALVARIANCE"].ToString());
            else
                TotalVaraiance = 0;
            if (dr1.Row["FLDTOTALVARIANCEPERCENTAGE"].ToString() != "")
                TotalVaraiancePercentage = Convert.ToDecimal(dr1.Row["FLDTOTALVARIANCEPERCENTAGE"].ToString());
            else
                TotalVaraiancePercentage = 0;


        }
    }
}
