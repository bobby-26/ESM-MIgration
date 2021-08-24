using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Dashboard_DashboardRestHourDuty : PhoenixBasePage
{
    double currenttime = 24.0;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
       
        if (!IsPostBack)
        {
            ViewState["DFT"] = DefaultTime();
            ViewState["DUTY"] = Request.QueryString["d"];
            if(ViewState["DUTY"].ToString().ToUpper()=="OFF")
            {
                lblStartTime.Text = "End Time";
                ddlPersonel.Enabled = false;
                ddlPersonel.SelectedValue = Request.QueryString["e"];
            }
            else
            {
                ddlPersonel.Enabled = true;
            }
            PopuldateElement();            
        }
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (ViewState["DUTY"].ToString().ToUpper() == "ON")
            toolbar.AddButton("Add", "ON", ToolBarDirection.Right);
        else
            toolbar.AddButton("Off Duty", "OFF", ToolBarDirection.Right);
        MenuRestHour.AccessRights = this.ViewState;
        MenuRestHour.MenuList = toolbar.Show();
    }
    protected void MenuRestHour_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("ON"))
            {
                if (!IsValidRestHour())
                {
                    ucError.Visible = true;
                    return;
                }

                DateTime localtime = Session["localtime"] != null ? DateTime.Parse(Session["localtime"].ToString()) : DateTime.Now;

                PhoenixDashboardTechnical.DashboardAddOnDuty(PhoenixSecurityContext.CurrentSecurityContext.VesselID, int.Parse(ddlPersonel.SelectedValue)
                    , localtime, decimal.Parse(ddlStartTime.SelectedValue));
                string script = "parent.frames[1].$find(\"RadAjaxManager1\").ajaxRequest(\"WRH\");top.closeTelerikWindow('wo');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            else if (CommandName.ToUpper().Equals("OFF"))
            {
                if (!IsValidRestHour())
                {
                    ucError.Visible = true;
                    return;
                }
                DateTime localtime = Session["localtime"] != null ? DateTime.Parse(Session["localtime"].ToString()) : DateTime.Now;
                PhoenixDashboardTechnical.DashboardOffHire(PhoenixSecurityContext.CurrentSecurityContext.VesselID, int.Parse(ddlPersonel.SelectedValue)
                    , localtime, decimal.Parse(ddlStartTime.SelectedValue));
                string refreshwindow = Request.QueryString["refwin"];
                string script = string.Empty;
                if (string.IsNullOrEmpty(refreshwindow))
                {
                    script = "parent.frames[1].$find(\"RadAjaxManager1\").ajaxRequest(\"WRH\");top.closeTelerikWindow('wo');";
                }
                else
                {
                    script = "top.closeTelerikWindow('wo','" + refreshwindow + "');";
                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected List<String> BindDuration()
    {
        List<String> duration = new List<String>();
        int j = 0;
        for (var i = 0.0; i <= currenttime; i += .5)
        {
            duration.Add(i.ToString());
            //duration[j] = i.ToString();
            //j++;
        }
        return duration;// new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24" };
    }
    private void PopuldateElement()
    {
        DataTable crewList = GetCrewList();

        ddlStartTime.DataSource = BindDuration();
        ddlStartTime.DataBind();

        ddlPersonel.DataSource = crewList;
        ddlPersonel.DataBind();
    }
    private DataTable GetCrewList()
    {
        return PhoenixVesselAccountsEmployee.ListVesselCrew(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableDateTime(string.Empty));
    }
    private bool IsValidRestHour()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDecimal(ddlStartTime.SelectedValue) == null)
            ucError.ErrorMessage = "Start Time is required.";


        if (General.GetNullableInteger(ddlPersonel.SelectedValue) == null)
            ucError.ErrorMessage = "Personel is required.";

        return (!ucError.IsError);
    }

    protected void ddlStartTime_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
        //set custom attributes from the datasource:  
        string time = e.Item.Text;
        if (time.IndexOf(".") >= 0)
        {
            time = time.Replace(".5", "30");
        }
        e.Item.Text = PadZero(time);
        if(ViewState["DFT"].ToString() == e.Item.Text)
        {
            e.Item.Selected = true;
        }
    }
    protected string PadZero(string padstring)
    {
        if (padstring.Length == 1)
        {
            padstring = padstring.PadLeft(2, '0');
        }
        if (padstring.Length == 2)
        {
            padstring = padstring.PadRight(4, '0');
        }
        if (padstring.Length == 3)
        {
            padstring = padstring.PadLeft(4, '0');
        }
        return padstring;
    }
    private string DefaultTime()
    {
        string defaulttime = string.Empty;
        DataTable dt;
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            dt = PhoenixCommonDashboard.VesselOfficeDetailsEdit(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        else
            dt = PhoenixCommonDashboard.FetchVesselTime(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        if(dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            DateTime localtime = General.GetNullableDateTime(dr["FLDLOCALTIME"].ToString()) != null ? DateTime.Parse(dr["FLDLOCALTIME"].ToString()) : DateTime.Now;
            int min = localtime.Minute;
            int x = min - (min % 30);
            defaulttime = PadZero(localtime.Hour + "" + x + (x.ToString().Length > 1 ? "" : "0"));

            currenttime = localtime.Hour + (min / 60);

        }
        return defaulttime;
    }
}