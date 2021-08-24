using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Dashboard;
using Telerik.Web.UI;
partial class DashboardRHWorkCalenderGeneral : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
				
				ViewState["SHIPCALENDARID"] = null;

                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                MenuWorkHour.AccessRights = this.ViewState;
                MenuWorkHour.MenuList = toolbar.Show();

                rbtnadvanceretard.SelectedValue = "0";
                if (Request.QueryString["shipresthourid"] != null)
                {
                    ViewState["SHIPCALENDARID"] = Request.QueryString["shipresthourid"];
                }
                BindLocalTime();
                BindDetails();
               
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuWorkHour_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            //DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

			if (CommandName.ToUpper().Equals("SAVE"))
			{

                if (!IsValidData())
                {
                    ucError.Visible = true;
                    return;
                }

                var date = txtNextDate.SelectedDate.Value;
                var time = txtNextDateTime.SelectedDate.Value;
                
                string d = date.Day + "-" + date.Month + "-" + date.Year + " " + time.Hour + ":" + time.Minute + ":" + time.Second;


                PhoenixDashboardTechnical.InsertShipMeanTime(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , Convert.ToDateTime(d)
                        , General.GetNullableInteger(rbnhourchange.SelectedValue) > 0 ? int.Parse(ddlMinutes.SelectedValue) : 0
                        , int.Parse(rbnhourchange.SelectedValue ?? "0")
                        , int.Parse(rbtnworkplace.SelectedValue ?? "0")
                        , int.Parse(rbtnadvanceretard.SelectedValue ?? "0")
                    );
                PhoenixDashboardTechnical.UpdateRestHourShipWorkCalendar(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                         Convert.ToInt64(ViewState["SHIPCALENDARID"]),
                        PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                        Convert.ToDateTime(d),
                        int.Parse(rbtnadvanceretard.SelectedValue ?? "0"),
                        int.Parse(rbnhourchange.SelectedValue ?? "0"),
                        0,
                        int.Parse(rbtnworkplace.SelectedValue ?? "0"),
                        General.GetNullableInteger(rbnhourchange.SelectedValue) > 0 ? decimal.Parse(ddlMinutes.SelectedValue) : 0,
                        decimal.Parse(txtShipMean.Text));

                ucStatus.Text = "Rest Hour Information Saved Successfully";
				ucStatus.Visible = true;
                string script = "parent.frames[1].$find(\"RadAjaxManager1\").ajaxRequest(\"SMT\");top.closeTelerikWindow('smt');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
			else
			{
                string script = "parent.frames[1].$find(\"RadAjaxManager1\").ajaxRequest(\"SMT\");top.closeTelerikWindow('smt');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }	
    private void BindLocalTime()
    {
        DataTable dt = PhoenixVesselAccountsRH.EditShipMeanTime(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        if (dt.Rows.Count > 0)
        {
            txtNextDate.SelectedDate = DateTime.Parse(dt.Rows[0]["FLDCURRENTTIME"].ToString());
            txtNextDateTime.SelectedDate = DateTime.Parse(dt.Rows[0]["FLDCURRENTTIME"].ToString());
            ucCalendar.LocalTime = dt.Rows[0]["FLDCURRENTTIME"].ToString();
        }
        else
        {
            ucCalendar.LocalTime = DateTime.Now.ToString();
        }

        int r1 = 10;
        int r2 = 10;

        DataSet ds = PhoenixVesselAccountsRH.RestHourShipWorkCalendarSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , null, null, 1, 50, ref r1, ref r2, txtNextDate.SelectedDate);

        if(ds.Tables[0].Rows.Count ==1)
        {
            ucCalendar.SelectedValue = ds.Tables[0].Rows[0]["FLDSHIPCALENDARID"].ToString();
            ucCalendar.Text = ds.Tables[0].Rows[0]["FLDDATE"].ToString();
            ViewState["SHIPCALENDARID"] = ds.Tables[0].Rows[0]["FLDSHIPCALENDARID"].ToString();
            ucCalendar.Enabled = false;
        }

    }
    private void BindDetails()
    {
        
        DataSet ds = PhoenixVesselAccountsRH.WorkCalenderCurrentWorkDayEdit(
                                                          Convert.ToInt64(ViewState["SHIPCALENDARID"])
                                                        , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                        , int.Parse(rbtnadvanceretard.SelectedValue == null ? "0" : rbtnadvanceretard.SelectedValue)
                                                        );
        ViewState["IDLSELECTEDVALUE"] = (rbtnadvanceretard.SelectedValue == null ? "0" : rbtnadvanceretard.SelectedValue);

		if (ds.Tables[0].Rows.Count > 0)
		{
            rbnhourchange.SelectedValue = ds.Tables[0].Rows[0]["FLDADVANCERETARD"].ToString();
            //if(ds.Tables[0].Rows[0]["FLDHALFHOURYN"].ToString()!="0" && ds.Tables[0].Rows[0]["FLDHALFHOURYN"].ToString()!="")
            rbtnadvanceretard.SelectedValue = ds.Tables[0].Rows[0]["FLDCLOCK"].ToString();
            rbtnworkplace.SelectedValue = ds.Tables[0].Rows[0]["FLDWORKPLACE"].ToString();

            
            if (General.GetNullableDecimal(ds.Tables[0].Rows[0]["FLDSHIPMEANTIME"].ToString()) != null && General.GetNullableDecimal(ds.Tables[0].Rows[0]["FLDSHIPMEANTIME"].ToString()) < 0)
                ddlShipMean.SelectedValue = "-1";
            else
                ddlShipMean.SelectedValue = "1";

            txtShipMean.Text = ds.Tables[0].Rows[0]["FLDSHIPMEANTIME"].ToString();
        }
        
    }
    private bool IsValidData()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtNextDate.SelectedDate == null)
            ucError.ErrorMessage = "Clock is required";

        if (General.GetNullableInteger(ucCalendar.SelectedValue) == null)
            ucError.ErrorMessage = "Ship Calendar is required";

        if (General.GetNullableInteger(rbnhourchange.SelectedValue) == null)
            ucError.ErrorMessage = "Advance/Retard is required";

        if (General.GetNullableDecimal(ddlMinutes.SelectedValue).HasValue && General.GetNullableDecimal(ddlMinutes.SelectedValue).Value < 0)
            ucError.ErrorMessage = "Advance/Retard by should be greater than zero.";

        if (General.GetNullableDecimal(ddlMinutes.SelectedValue)==null && General.GetNullableInteger(rbnhourchange.SelectedValue) > 0)
            ucError.ErrorMessage = "Advance/Retard by is required";
        return (!ucError.IsError);
    }


    protected void txtNextDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        ucCalendar.LocalTime = txtNextDate.SelectedDate.ToString();
        int r1 = 10;
        int r2 = 10;
        DataSet ds = PhoenixVesselAccountsRH.RestHourShipWorkCalendarSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , null, null, 1, 50, ref r1, ref r2, txtNextDate.SelectedDate);

        if (ds.Tables[0].Rows.Count == 1)
        {
            ucCalendar.SelectedValue = ds.Tables[0].Rows[0]["FLDSHIPCALENDARID"].ToString();
            ucCalendar.Text = ds.Tables[0].Rows[0]["FLDDATE"].ToString();
            ViewState["SHIPCALENDARID"] = ds.Tables[0].Rows[0]["FLDSHIPCALENDARID"].ToString();
            ucCalendar.Enabled = false;
            BindDetails();
        }
        else
        {
            ucCalendar.SelectedValue = "";
            ucCalendar.Text = "";
            ucCalendar.Enabled = true;
        }
    }

    protected void ucCalendar_TextChangedEvent(object sender, EventArgs e)
    {
        ViewState["SHIPCALENDARID"] = ucCalendar.SelectedValue;

        BindDetails();
    }
}

