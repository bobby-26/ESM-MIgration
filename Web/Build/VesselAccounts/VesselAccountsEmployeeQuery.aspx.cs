using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Web.UI;

public partial class VesselAccountsEmployeeQuery : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsEmployeeQuery.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelpactivity','','VesselAccounts/VesselAccountsCrewRHCR6DReport.aspx')", "CR6D Report", "<i class=\"fas fa-chart-bar\"></i>", "CR6DREPORT");
            CrewQueryMenu.AccessRights = this.ViewState;
            CrewQueryMenu.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void CrewQueryMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDROWNUMBER", "FLDFILENO", "FLDNAME", "FLDSIGNONRANKCODE", "FLDPASSPORTNO", "FLDSEAMANBOOKNO", "FLDSIGNONDATE", "FLDRELIEFDUEDATE" };
                string[] alCaptions = { "S.No.", "File No.", "Name", "Rank", "Passport", "CDC No.", "Sign on", "Relief Due" };

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int sortdirection;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                else
                    sortdirection = 0;
                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataTable dt = PhoenixCommonVesselAccounts.SearchVesseEmployee(PhoenixSecurityContext.CurrentSecurityContext.VesselID, null, null, null
                                                                                       , sortexpression, sortdirection
                                                                                       , 1, iRowCount
                                                                                       , ref iRowCount, ref iTotalPageCount);
                General.ShowExcel("Crew List", dt, alColumns, alCaptions, sortdirection, sortexpression);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    public void BindData()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDROWNUMBER", "FLDFILENO", "FLDNAME", "FLDSIGNONRANKCODE", "FLDPASSPORTNO", "FLDSEAMANBOOKNO", "FLDSIGNONDATE", "FLDRELIEFDUEDATE" };
        string[] alCaptions = { "S.No.", "File No.", "Name", "Rank", "Passport", "CDC No.", "Sign on", "Relief Due" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {

            DataTable dt = PhoenixCommonVesselAccounts.SearchVesseEmployee(PhoenixSecurityContext.CurrentSecurityContext.VesselID, null, null, null
                                                                       , sortexpression, sortdirection
                                                                       , gvCrewSearch.CurrentPageIndex + 1, gvCrewSearch.PageSize
                                                                       , ref iRowCount, ref iTotalPageCount);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvCrewSearch", "Crew List", alCaptions, alColumns, ds);
            gvCrewSearch.DataSource = ds;
            gvCrewSearch.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvCrewSearch_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;
            RadGrid _gridView = (RadGrid)sender;
            if (e.CommandName.ToUpper() == "EDIT")
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                Filter.CurrentVesselCrewSelection = ((RadLabel)eeditedItem.FindControl("lblEmployeeid")).Text;
                Response.Redirect("..\\VesselAccounts\\VesselAccountsEmployeeGeneral.aspx", false);
            }
            else if (e.CommandName.ToUpper().Equals("CREWATTENDANCE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
                {
                    e.Item.Selected = true;
                    LinkButton cmdCrewAttendance = (LinkButton)e.Item.FindControl("cmdCrewAttendance");
                    cmdCrewAttendance.Visible = SessionUtil.CanAccess(this.ViewState, cmdCrewAttendance.CommandName);
                    RadLabel EmpId = ((RadLabel)e.Item.FindControl("lblEmployeeid"));
                    RadLabel RHStartId = ((RadLabel)e.Item.FindControl("lblRHstartid"));
                    RadLabel ShipCalendarId = ((RadLabel)e.Item.FindControl("lblShipCalendarId"));
                    RadLabel RHEmpId = ((RadLabel)e.Item.FindControl("lblrhempid"));

                    if (General.GetNullableGuid(RHStartId.Text) != null)
                    {
                        PhoenixVesselAccountsRH.InsertRestHourWorkCalenderAutomatic(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                               new Guid(RHStartId.Text),
                               PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                               int.Parse(EmpId.Text),
                               int.Parse(ShipCalendarId.Text)
                               );

                        string scriptpopup = String.Format("javascript:parent.openNewWindow('code1','','" + Session["sitepath"] + "/VesselAccounts/VesselAccountsRHWorkCalender.aspx?EMPID=" + EmpId.Text + "&RESTHOURSTARTID=" + RHStartId.Text + "&MODELYN=Y&SHIPSTARTCALENDARID=" + ShipCalendarId.Text + "&RESTHOUREMPLOYEEID=" + RHEmpId.Text + "');");
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

                        //Response.Redirect("../VesselAccounts/VesselAccountsRHWorkCalender.aspx?EMPID=" + EmpId.Text + "&RESTHOURSTARTID=" + RHStartId.Text + "&MODELYN=Y&SHIPSTARTCALENDARID=" + ShipCalendarId.Text, false);
                    }
                        

                }
                else
                {
                    RadLabel EmpId = ((RadLabel)e.Item.FindControl("lblEmployeeid"));
                    RadLabel RHStartId = ((RadLabel)e.Item.FindControl("lblRHstartid"));
                    RadLabel ShipCalendarId = ((RadLabel)e.Item.FindControl("lblShipCalendarId"));
                    RadLabel RHEmpId = ((RadLabel)e.Item.FindControl("lblrhempid"));

                    if (General.GetNullableGuid(RHStartId.Text) != null)
                    {

                        string scriptpopup = String.Format("javascript:parent.openNewWindow('code1','','" + Session["sitepath"] + "/VesselAccounts/VesselAccountsRHWorkCalender.aspx?EMPID=" + EmpId.Text + "&RESTHOURSTARTID=" + RHStartId.Text + "&MODELYN=Y&SHIPSTARTCALENDARID=" + ShipCalendarId.Text + "&RESTHOUREMPLOYEEID=" + RHEmpId.Text + "');");
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

                        //Response.Redirect("../VesselAccounts/VesselAccountsRHWorkCalender.aspx?EMPID=" + EmpId.Text + "&RESTHOURSTARTID=" + RHStartId.Text + "&MODELYN=Y&SHIPSTARTCALENDARID=" + ShipCalendarId.Text, false);
                    }
                }

            }else if (e.CommandName.ToUpper().Equals("SIGNOFF"))
            {
                RadLabel RHStartId = ((RadLabel)e.Item.FindControl("lblRHstartid"));
                if (RHStartId != null && General.GetNullableGuid(RHStartId.Text) != null)
                {
                    PhoenixVesselAccountsRH.AllowSignOff(new Guid(RHStartId.Text));
                    
                    ucStatus.Show("Now Seafarer can Verify & Review his work hours");
                    gvCrewSearch.Rebind();
                }
                    
            }
            else if (e.CommandName.ToUpper().Equals("SIGNON"))
            {
                RadLabel RHStartId = ((RadLabel)e.Item.FindControl("lblRHstartid"));
                if (RHStartId != null && General.GetNullableGuid(RHStartId.Text) != null)
                {
                    PhoenixVesselAccountsRH.AllowSignOff(new Guid(RHStartId.Text));

                    ucStatus.Show("Now Seafarer cannot Review his work hours before month end");
                    gvCrewSearch.Rebind();
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {
        
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            GridDataItem item = (GridDataItem)e.Item;
            LinkButton sg = (LinkButton)item.FindControl("cmdGenContract");
            LinkButton crw = (LinkButton)item.FindControl("lnkEployeeName");
            if (sg != null)
            {
                sg.Visible = SessionUtil.CanAccess(this.ViewState, sg.CommandName);
                sg.Attributes.Add("onclick", "openNewWindow('chml', 'Contract', '" + Session["sitepath"] + "/VesselAccounts/VesselAccountsReportContract.aspx?EmpId=" + drv["FLDEMPLOYEEID"].ToString() + "&SignonoffId=" + drv["FLDSIGNONOFFID"].ToString() + "');return false;");
            }
            LinkButton cmdWorkHours = (LinkButton)e.Item.FindControl("cmdWorkHours");
            HtmlGenericControl workhourIcon = (HtmlGenericControl)e.Item.FindControl("workhourIcon");

            if (cmdWorkHours != null)
            {
                cmdWorkHours.Visible = SessionUtil.CanAccess(this.ViewState, cmdWorkHours.CommandName);
                if (General.GetNullableGuid(drv["FLDRESTHOUREMPLOYEEID"].ToString()) != null)
                    cmdWorkHours.Attributes.Add("onclick", "javascript:$modalWindow.modalWindowUrl = '../VesselAccounts/VesselAccountsRHDesignation.aspx?EmployeeId=" + drv["FLDEMPLOYEEID"].ToString() + "&RankId=" + drv["FLDSIGNONRANKID"].ToString() +
                        "&EmployeeName=" + drv["FLDNAME"].ToString() + "&RankName=" + drv["FLDSIGNONRANKNAME"].ToString() + "&MODELYN=Y&RestHourEmployeeId=" + drv["FLDRESTHOUREMPLOYEEID"].ToString() + "';showDialog('Default Work Hours');");

                if (drv["FLDISCONFIGURED"].ToString() == "0")
                {                    
                    workhourIcon.Style.Add("color", "red");
                    cmdWorkHours.ToolTip = "Schedule Work Hours";
                }
            }
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                {
                    crw.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&launchedfrom=offshore'); return false;");
                }
                else
                {
                    crw.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "'); return false;");
                }
            }

            LinkButton cmdReport = (LinkButton)e.Item.FindControl("cmdReport");

            if (cmdReport != null)
            {
                cmdReport.Attributes.Add("onclick", "openNewWindow('Report', '', '" + Session["sitepath"] + "/VesselAccounts/VesselAccountsCrewListRHNonComplianceList.aspx?EmployeeId=" + drv["FLDEMPLOYEEID"].ToString() + "&RestHourStartId=" + drv["FLDRESTHOURSTARTID"].ToString() + "&RestHourEmployeeId=" + drv["FLDRESTHOUREMPLOYEEID"].ToString() + "');return true;");
            }

            if (cmdReport != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdReport.CommandName)) cmdReport.Visible = false;
            }
            LinkButton cmdCR6CReport = (LinkButton)e.Item.FindControl("cmdCR6CReport");

            if (cmdCR6CReport != null)
            {
                cmdCR6CReport.Attributes.Add("onclick", "openNewWindow('Report', '', '" + Session["sitepath"] + "/VesselAccounts/VesselAccountsCrewListRHCR6CReport.aspx?EmployeeId=" + drv["FLDEMPLOYEEID"].ToString() + "&RestHourStartId=" + drv["FLDRESTHOURSTARTID"].ToString() + "&RestHourEmployeeId=" + drv["FLDRESTHOUREMPLOYEEID"].ToString() + "');return true;");
            }

            if (cmdCR6CReport != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdCR6CReport.CommandName)) cmdCR6CReport.Visible = false;
            }

            LinkButton SignOff = (LinkButton)e.Item.FindControl("cmdSignOff");
            if (SignOff != null && (drv["FLDLOGINRANKID"].ToString() != "1" || drv["FLDSIGNOFFDUE"].ToString() == "1"))
            {
                SignOff.Visible = false;
            }
            else if (SignOff != null)
            {
                SignOff.Visible = SessionUtil.CanAccess(this.ViewState, SignOff.CommandName);
            }
            LinkButton SignOn = (LinkButton)e.Item.FindControl("cmdSignOn");
            if (SignOn != null && (drv["FLDLOGINRANKID"].ToString() != "1" || drv["FLDSIGNOFFDUE"].ToString() == "0"))
            {
                SignOn.Visible = false;
            }
            else if (SignOn != null)
            {
                SignOn.Visible = SessionUtil.CanAccess(this.ViewState, SignOn.CommandName);
            }

        }
    }
  
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvCrewSearch.Rebind();

    }
}
