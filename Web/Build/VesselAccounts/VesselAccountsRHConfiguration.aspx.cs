using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Text;
using System.Web.UI;
using Telerik.Web.UI;

public partial class VesselAccountsRHConfiguration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Calendar", "WORKCALENDER",ToolBarDirection.Left);
            toolbarmain.AddButton("Crew", "CREW", ToolBarDirection.Left);
            MenuRHGeneral.AccessRights = this.ViewState;
            MenuRHGeneral.MenuList = toolbarmain.Show();
            MenuRHGeneral.SelectedMenuIndex = 1;
            
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
			    ViewState["RESTHOURSTARTID"] = null;
                gvCrewList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RHGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CREW"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsRHConfiguration.aspx");
            }
            else if (CommandName.ToUpper().Equals("WORKCALENDER"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsRHWorkCalendarShip.aspx");
            }
            else if (CommandName.ToUpper().Equals("DESIGNATION"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsRHDesignation.aspx");
            }
            else if (CommandName.ToUpper().Equals("OPA"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsRHOPARequirement.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowError()
    {
        ucError.HeaderMessage = "Navigation Error";
        ucError.ErrorMessage = "Please select a Seafarer and then Navigate to other Tabs";
        ucError.Visible = true;
    }
    protected void CrewList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
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
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();
        string[] alColumns = { "FLDROWNUMBER", "FLDNAME", "FLDRANKNAME", "FLDSIGNONDATE", "FLDRELIEFDUEDATE", "FLDSTARTDATE"};
        string[] alCaptions = { "Sr.No", "Name", "Rank", "SignOn Date", "Releif Due Date", "Start Date" };

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

        DataSet ds = PhoenixVesselAccountsRH.SearchRestHourStart(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                            , int.Parse(General.GetNullableInteger(ddlactive.SelectedValue)==null ? "1": ddlactive.SelectedValue)
                                            , sortexpression, sortdirection
                                            , (int)ViewState["PAGENUMBER"]
                                            , iRowCount
                                            , ref iRowCount
                                            , ref iTotalPageCount,null);


        Response.AddHeader("Content-Disposition", "attachment; filename=RestHourCrewList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>CrewList</h3></td>");
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
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROWNUMBER", "FLDNAME", "FLDRANKNAME", "FLDSIGNONDATE", "FLDRELIEFDUEDATE", "FLDSTARTDATE" };
        string[] alCaptions = { "Sr.No", "Name", "Rank", "SignOn Date", "Releif Due Date", "Start Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        NameValueCollection nvc = Filter.CurrentCrewListSelection;

         DataSet ds = PhoenixVesselAccountsRH.SearchRestHourStart(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                            , int.Parse(General.GetNullableInteger(ddlactive.SelectedValue)==null ? "1": ddlactive.SelectedValue)
                                            , sortexpression, sortdirection
                                            , (int)ViewState["PAGENUMBER"]
                                            , gvCrewList.PageSize
                                            , ref iRowCount
                                            , ref iTotalPageCount,null);

        General.SetPrintOptions("gvCrewList", "Crew List", alCaptions, alColumns, ds);

        gvCrewList.DataSource = ds;
        gvCrewList.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["FLDALLOWYN"].ToString() == "0")
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelpactivity','','VesselAccounts/VesselAccountsRHEmployeeList.aspx')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelpactivity','','VesselAccounts/VesselAccountsCrewRHCR6DReport.aspx')", "CR6D Report", "<i class=\"fas fa-chart-bar\"></i>", "CR6DREPORT");
                MenuCrewList.AccessRights = this.ViewState;
                MenuCrewList.MenuList = toolbar.Show();
                MenuCrewList.Visible = true;
            }
        }
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["FLDALLOWYN"].ToString() == "1")
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelpactivity','','VesselAccounts/VesselAccountsRHEmployeeList.aspx')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelpactivity','','VesselAccounts/VesselAccountsCrewRHCR6DReport.aspx')", "CR6D Report", "<i class=\"fas fa-chart-bar\"></i>", "CR6DREPORT");
                MenuCrewList.AccessRights = this.ViewState;
                MenuCrewList.MenuList = toolbar.Show();
                MenuCrewList.Visible = false;
                gvCrewList.ShowFooter = true;
            }
        }

        if (ds.Tables[0].Rows.Count == 0)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelpactivity','','VesselAccounts/VesselAccountsRHEmployeeList.aspx')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuCrewList.AccessRights = this.ViewState;
            MenuCrewList.MenuList = toolbar.Show();
            MenuCrewList.Visible = true;
            gvCrewList.ShowFooter = true;
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;        
    }

    protected void gvCrewList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                DataRowView  drv = (DataRowView)e.Item.DataItem;
                ImageButton ib1 = (ImageButton)e.Item.FindControl("cmdShowCalender");

                if (ib1 != null)
                    ib1.Attributes.Add("onclick", "return showPickList('spnPickListCalendar', 'codehelp1', '', '../Common/CommonPickListRHShipWorkCalendar.aspx',true);");

                if (ddlactive.SelectedValue == "0")
                {
                    LinkButton ap = (LinkButton)e.Item.FindControl("cmdReset");
                    if (ap != null)
                    {
                        ap.Visible = false;
                    }
                }
                RadComboBox ddl = (RadComboBox)e.Item.FindControl("ddlWatchKeeping");
                if (ddl != null)
                {
                    ddl.DataSource = PhoenixVesselAccountsRH.RHWatchKeeperTime(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                    ddl.DataBind();
                    
                    ddl.SelectedValue = drv["FLDWATCHKEEPINGID"].ToString();
                }
                if (drv["FLDALLOWYN"].ToString() == "1")
                {
                    gvCrewList.ShowFooter = true;
                    LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
                    if(cmdEdit != null)
                    {
                        cmdEdit.Visible = true;
                    }
                }
                RadComboBox ddlusername = (RadComboBox)e.Item.FindControl("ddlUserNameEdit");
                if (ddlusername != null && drv != null)
                {
                    ddlusername.DataSource = PhoenixVesselAccountsRH.ListRestHourUserName(
                                                                    PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                                    Convert.ToInt32(General.GetNullableInteger(drv["FLDRANKID"].ToString())),
                                                                    Convert.ToInt32(General.GetNullableInteger(drv["FLDEMPLOYEEID"].ToString())),
                                                                    int.Parse(General.GetNullableInteger(ddlactive.SelectedValue) == null ? "1" : ddlactive.SelectedValue)
                                                                    );
                    ddlusername.DataBind();
                    ddlusername.SelectedValue = drv["FLDUSERCODE"].ToString();
                }

                RadLabel EmployeeId = (RadLabel)e.Item.FindControl("lblEmpId");
                RadLabel RankId = (RadLabel)e.Item.FindControl("lblrankid");
                RadLabel EmployeeName = (RadLabel)e.Item.FindControl("lnkName");
                RadLabel RankName = (RadLabel)e.Item.FindControl("lblRankName");
                RadLabel RestHourEmployeeId = (RadLabel)e.Item.FindControl("lblrhempid");
                RadLabel RestHourStartId = (RadLabel)e.Item.FindControl("lblRHstartid");

                LinkButton cmdWorkHours = (LinkButton)e.Item.FindControl("cmdWorkHours");
                if (cmdWorkHours != null)
                {
                    cmdWorkHours.Visible = SessionUtil.CanAccess(this.ViewState, cmdWorkHours.CommandName);
                    cmdWorkHours.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelpactivity', '', '" + Session["sitepath"] + "/VesselAccounts/VesselAccountsRHDesignation.aspx?EmployeeId=" + EmployeeId.Text + "&RankId=" + RankId.Text + 
                        "&EmployeeName=" + EmployeeName.Text + "&RankName=" + RankName.Text + "&RestHourEmployeeId=" + RestHourEmployeeId.Text + "');return true;");
                }
                if (ddlactive.SelectedValue == "0" && PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                {
                    LinkButton Attendance = (LinkButton)e.Item.FindControl("cmdCrewAttendance");
                    if (Attendance != null)
                    {
                        Attendance.Visible = false;
                    }
                    LinkButton DefaultHours = (LinkButton)e.Item.FindControl("cmdWorkHours");
                    if (DefaultHours != null)
                    {
                        DefaultHours.Visible = false;
                    }
                }
                if (ddlactive.SelectedValue == "0")
                {
                    ImageButton ap = (ImageButton)e.Item.FindControl("cmdRankReset");
                    if (ap != null)
                    {
                        ap.Visible = false;
                    }
                }

                LinkButton cmdReport = (LinkButton)e.Item.FindControl("cmdReport");

                if (cmdReport != null)
                {
                    cmdReport.Attributes.Add("onclick", "openNewWindow('Report', '', '" + Session["sitepath"] + "/VesselAccounts/VesselAccountsCrewListRHNonComplianceList.aspx?EmployeeId=" + EmployeeId.Text + "&RestHourStartId=" + RestHourStartId.Text + "&RestHourEmployeeId=" + RestHourEmployeeId.Text + "');return true;");
                }

                if (cmdReport != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, cmdReport.CommandName)) cmdReport.Visible = false;
                }

                LinkButton cmdCR6CReport = (LinkButton)e.Item.FindControl("cmdCR6CReport");

                if (cmdCR6CReport != null)
                {
                    cmdCR6CReport.Attributes.Add("onclick", "openNewWindow('Report', '', '" + Session["sitepath"] + "/VesselAccounts/VesselAccountsCrewListRHCR6CReport.aspx?EmployeeId=" + EmployeeId.Text + "&RestHourStartId=" + RestHourStartId.Text + "&RestHourEmployeeId=" + RestHourEmployeeId.Text + "');return true;");
                }

                if (cmdCR6CReport != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, cmdCR6CReport.CommandName)) cmdCR6CReport.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidList(((RadTextBox)e.Item.FindControl("txtEmpNameAdd")).Text,
                                    ((UserControlRank)e.Item.FindControl("ucRankAdd")).SelectedRank,
                                    ((UserControlDate)e.Item.FindControl("ucSignonDateAdd")).Text,
                                    ((UserControlDate)e.Item.FindControl("ucReliefDueDateAdd")).Text))									
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixVesselAccountsRH.InsertRestHourEmployee(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                         null,
                        Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.VesselID),
                         ((RadTextBox)e.Item.FindControl("txtEmpNameAdd")).Text,
                        General.GetNullableInteger(((UserControlRank)e.Item.FindControl("ucRankAdd")).SelectedRank),
                        General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucSignonDateAdd")).Text),
                        General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucReliefDueDateAdd")).Text),
                        1,
						null,
                        General.GetNullableString(((RadTextBox)e.Item.FindControl("txtFileNoAdd")).Text));
                Rebind();
                ucStatus.Text = "Crew Information Added.";
            }
            else if (e.CommandName.ToUpper().Equals("MAP"))
            {
                PhoenixVesselAccountsRH.RHEmployeeUsernameMap(int.Parse(((RadLabel)e.Item.FindControl("lblEmpId")).Text)
                            , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblRHstartid")).Text));
            }
            else if (e.CommandName.ToUpper().Equals("RESET"))
            {
                ViewState["RESTHOURSTARTID"] = ((RadLabel)e.Item.FindControl("lblRHstartid")).Text;
                RadWindowManager1.RadConfirm("1. If the Seafarer is Promoted, then Seafarer rank will be changed to the Promoted Rank and old record will not be available. <br/> Are you sure to refresh the crew list ?", "ConfirmRankReset", 320, 150, null, "Confirm");
                return;
            }
            else if (e.CommandName.ToUpper().Equals("WORKHOURS"))
            {
                e.Item.Selected = true;
            }
            else if (e.CommandName.ToUpper().Equals("CREWATTENDANCE"))
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
                {
                    e.Item.Selected = true;
                    LinkButton cmdCrewAttendance = (LinkButton)e.Item.FindControl("cmdCrewAttendance");
                    if (cmdCrewAttendance != null)
                    {
                        cmdCrewAttendance.Visible = SessionUtil.CanAccess(this.ViewState, cmdCrewAttendance.CommandName);
                        RadLabel EmpId = ((RadLabel)e.Item.FindControl("lblEmpId"));
                        RadLabel RHStartId = ((RadLabel)e.Item.FindControl("lblRHstartid"));
                        RadLabel ShipCalendarId = ((RadLabel)e.Item.FindControl("lblShipCalendarId"));
                        RadLabel RHEmpId = ((RadLabel)e.Item.FindControl("lblrhempid"));

                        PhoenixVesselAccountsRH.InsertRestHourWorkCalenderAutomatic(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                               new Guid(RHStartId.Text),
                               PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                               int.Parse(EmpId.Text),
                               int.Parse(ShipCalendarId.Text)
                               );

                        string scriptpopup = String.Format("javascript:parent.openNewWindow('code1','','" + Session["sitepath"] + "/VesselAccounts/VesselAccountsRHWorkCalender.aspx?EMPID=" + EmpId.Text + "&RESTHOURSTARTID=" + RHStartId.Text + "&MODELYN=Y&SHIPSTARTCALENDARID=" + ShipCalendarId.Text + "&RESTHOUREMPLOYEEID=" + RHEmpId.Text + "');");
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

                        //Response.Redirect("../VesselAccounts/VesselAccountsRHWorkCalender.aspx?EMPID=" + EmpId.Text + "&RESTHOURSTARTID=" + RHStartId.Text + "&SHIPSTARTCALENDARID=" + ShipCalendarId.Text, false);
                    }
                }
                else
                {
                    RadLabel EmpId = ((RadLabel)e.Item.FindControl("lblEmpId"));
                    RadLabel RHStartId = ((RadLabel)e.Item.FindControl("lblRHstartid"));
                    RadLabel ShipCalendarId = ((RadLabel)e.Item.FindControl("lblShipCalendarId"));
                    RadLabel RHEmpId = ((RadLabel)e.Item.FindControl("lblrhempid"));

                    if (General.GetNullableGuid(RHStartId.Text) != null)
                    {

                        string scriptpopup = String.Format("javascript:parent.openNewWindow('code1','','" + Session["sitepath"] + "/VesselAccounts/VesselAccountsRHWorkCalender.aspx?EMPID=" + EmpId.Text + "&RESTHOURSTARTID=" + RHStartId.Text + "&MODELYN=Y&SHIPSTARTCALENDARID=" + ShipCalendarId.Text + "&RESTHOUREMPLOYEEID=" + RHEmpId.Text + "');");
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

                        //Response.Redirect("../VesselAccounts/VesselAccountsRHWorkCalender.aspx?EMPID=" + EmpId.Text + "&RESTHOURSTARTID=" + RHStartId.Text + "&SHIPSTARTCALENDARID=" + ShipCalendarId.Text, false);
                    }
                }
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
    private bool IsValidList(string empname, string rankid, string signondate, string reliefduedate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(empname) == null)
            ucError.ErrorMessage = "Name is required.";

        if (General.GetNullableInteger(rankid) == null)
            ucError.ErrorMessage = "Rank is required.";

        if (General.GetNullableDateTime(reliefduedate) == null)
            ucError.ErrorMessage = "Relief Due Date is required.";

        if (General.GetNullableDateTime(signondate) == null)
            ucError.ErrorMessage = "Sign On Date is required.";

        return (!ucError.IsError);
    }

    private bool IsValidStart(string startdate, string ActiveYN)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
        if (General.GetNullableDateTime(startdate) == null)
            ucError.ErrorMessage = "Start Date is required.";

        //if (General.GetNullableInteger(ActiveYN) == 1)
        //{
        //    if (string.IsNullOrEmpty(usercode) || usercode.ToUpper().ToString() == "DUMMY")
        //        ucError.ErrorMessage = "User Name is required.";
        //}

        return (!ucError.IsError);
    }  
    protected void ddlactive_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Rebind();       
    }

    protected void gvCrewList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewList.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvCrewList.EditIndexes.Clear();
        gvCrewList.SelectedIndexes.Clear();
        gvCrewList.DataSource = null;
        gvCrewList.Rebind();
    }

    protected void gvCrewList_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void gvCrewList_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string ActiveYN = ((RadCheckBox)e.Item.FindControl("chkactiveyn")).Checked.Equals(true) ? "1" : "0";

            if (!IsValidStart(((RadTextBox)e.Item.FindControl("ucStartDate")).Text, ActiveYN))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixVesselAccountsRH.InsertRestHourStart(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblRHstartid")).Text),
                        PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                         Convert.ToInt32(((RadLabel)e.Item.FindControl("lblEmpId")).Text),
                         Convert.ToDateTime(((RadTextBox)e.Item.FindControl("ucStartDate")).Text),
                         int.Parse(((RadTextBox)e.Item.FindControl("txtShipCalendarId")).Text),
                         int.Parse(((RadComboBox)e.Item.FindControl("ddlWatchKeeping")).SelectedValue),
                         int.Parse(((RadCheckBox)e.Item.FindControl("chkactiveyn")).Checked.Equals(true)? "1" : "0"),
                         General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlUserNameEdit")).SelectedValue),
                         General.GetNullableString(((RadTextBox)e.Item.FindControl("txtFileNo")).Text));

            ucStatus.Text = "Crew Information Updated";

            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }

    protected void ucConfirmRankReset_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixVesselAccountsRH.RefreshRestHourEmployee(General.GetNullableGuid(ViewState["RESTHOURSTARTID"].ToString()));
            Rebind();
            ucStatus.Text = "Crew List refreshed.";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
