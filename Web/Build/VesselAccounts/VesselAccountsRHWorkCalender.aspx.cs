using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class VesselAccountsRHWorkCalender : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            //toolbarmain.AddButton("Attendance", "WORKCALENDER", ToolBarDirection.Right);
            //toolbarmain.AddButton("Crew List", "CREW", ToolBarDirection.Right);
            MenuRHGeneral.AccessRights = this.ViewState;
            MenuRHGeneral.MenuList = toolbarmain.Show();
            MenuRHGeneral.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["EMPID"] = null;
                ViewState["RHSTARTID"] = null;
                ViewState["NCEXISTSYN"] = "";
                gvWorkCalender.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                cmdHiddenSubmit.Attributes.Add("style", "display:none");

                if (Request.QueryString["EMPID"] != null)
                    ViewState["EMPID"] = Request.QueryString["EMPID"].ToString();

                if (Request.QueryString["RESTHOURSTARTID"] != null)
                    ViewState["RHSTARTID"] = Request.QueryString["RESTHOURSTARTID"].ToString();


                if (Request.QueryString["SHIPSTARTCALENDARID"] != null)
                    ViewState["SHIPSTARTCALENDARID"] = Request.QueryString["SHIPSTARTCALENDARID"].ToString();

                if (Request.QueryString["RESTHOUREMPLOYEEID"] != null)
                    ViewState["RESTHOUREMPLOYEEID"] = Request.QueryString["RESTHOUREMPLOYEEID"].ToString();

                if (ViewState["RHSTARTID"] != null)
                    BindDetails();
                BindLevel();
                BindMonth();

                if(Request.QueryString["DCODE"]!=null && (Request.QueryString["DCODE"].ToString() == "RHWS" || Request.QueryString["DCODE"].ToString() == "RHWH" || Request.QueryString["DCODE"].ToString() == "RHWM"))
                {
                    var previousmonth = DateTime.Now.AddMonths(-1);


                    ViewState["MONTH"] = previousmonth.Month.ToString();
                    ViewState["YEAR"] = previousmonth.Year.ToString();
                }
                else
                {
                    ViewState["MONTH"] = DateTime.Today.Month.ToString();
                    ViewState["YEAR"] = DateTime.Today.Year.ToString();
                }


                
                ddlMonth.SelectedValue = ViewState["MONTH"].ToString() + "-" + ViewState["YEAR"].ToString();

            }

            BindUnlockedStatus();
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsRHWorkCalender.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvWorkCalender')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            if (SessionUtil.CanAccess(this.ViewState, "RECONCILE"))
            {
                toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsRHWorkCalender.aspx", "Reconcile Calendar", "<i class=\"fas fa-calendar-alt\"></i>", "RECONCILE");
            }
            MenuWorkHour.AccessRights = this.ViewState;
            MenuWorkHour.MenuList = toolbar.Show();


            BindToolbar();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindToolbar()
    {
        DataSet ds = PhoenixVesselAccountsRH.GetWRHAccess(int.Parse(ViewState["EMPID"].ToString()));
        DataSet ds1 = PhoenixVesselAccountsRH.RHUnlockedStatusBind(
                                          int.Parse(ViewState["EMPID"].ToString())
                                           , int.Parse(ViewState["MONTH"].ToString())
                                           , int.Parse(ViewState["YEAR"].ToString())
                                           );


        if(ds.Tables[0].Rows.Count>0 && ds1.Tables[0].Rows.Count > 0)
        {
            PhoenixToolbar toolbarLock = new PhoenixToolbar();

            if(ds1.Tables[0].Rows[0]["FLDSEAFARERLOCKEDYN"].ToString() == "0" && ds1.Tables[0].Rows[0]["FLDALLDAYSVERIFIEDYN"].ToString()=="1")
            {
                if(ds.Tables[0].Rows[0]["FLDSEAFARERYN"].ToString() == "1")
                    toolbarLock.AddButton("Review by Seafarer", "SEAFARER", ToolBarDirection.Right);
            }
            else if(ds1.Tables[0].Rows[0]["FLDSEAFARERLOCKEDYN"].ToString() == "1" && ds1.Tables[0].Rows[0]["FLDHODLOCKEDYN"].ToString() == "0")
            {
                if (ds.Tables[0].Rows[0]["FLDSEAFARERYN"].ToString() == "1")
                {
                    toolbarLock.AddButton("Unlock", "UNLOCK", ToolBarDirection.Right);
                    rdLevel.Enabled = false;
                    txtRemarks.Enabled = false;
                }
                    
                else if (ds.Tables[0].Rows[0]["FLDHODYN"].ToString() == "1")
                    toolbarLock.AddButton("Review by HOD", "HOD", ToolBarDirection.Right);
            }
            else if (ds1.Tables[0].Rows[0]["FLDSEAFARERLOCKEDYN"].ToString() == "1" && ds1.Tables[0].Rows[0]["FLDHODLOCKEDYN"].ToString() == "1" && ds1.Tables[0].Rows[0]["FLDMASTERLOCKEDYN"].ToString() =="0")
            {
                if (ds.Tables[0].Rows[0]["FLDHODYN"].ToString() == "1")
                {
                    toolbarLock.AddButton("Unlock", "UNLOCK", ToolBarDirection.Right);
                    rdLevel.Enabled = false;
                    txtRemarks.Enabled = false;
                }
                else if (ds.Tables[0].Rows[0]["FLDMASTERYN"].ToString() == "1")
                    toolbarLock.AddButton("Review by Master", "MASTER", ToolBarDirection.Right);
            }
            else if (ds1.Tables[0].Rows[0]["FLDSEAFARERLOCKEDYN"].ToString() == "1" && ds1.Tables[0].Rows[0]["FLDHODLOCKEDYN"].ToString() == "1" && ds1.Tables[0].Rows[0]["FLDMASTERLOCKEDYN"].ToString() == "1" && ds1.Tables[0].Rows[0]["FLDOFFICER1LOCKEDYN"].ToString() =="0")
            {
                if (ds.Tables[0].Rows[0]["FLDMASTERYN"].ToString() == "1")
                {
                    toolbarLock.AddButton("Unlock", "UNLOCK", ToolBarDirection.Right);
                    rdLevel.Enabled = false;
                    txtRemarks.Enabled = false;
                }
                else if (ds.Tables[0].Rows[0]["FLDOFFICEYN"].ToString() == "1")
                    toolbarLock.AddButton("Review by Office", "OFFICE", ToolBarDirection.Right);
            }

            MenuRHUnlock.AccessRights = this.ViewState;
            MenuRHUnlock.MenuList = toolbarLock.Show();
            MenuRHUnlock.SelectedMenuIndex = 0;
        }


    }


    private void GetUnlockAccess()
    {
        try
        {
            DataSet ds = PhoenixVesselAccountsRH.RHUnlockAccess(int.Parse(ViewState["EMPID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ViewState["ACCESSRANK"] = dr["FLDRANKID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindDetails()
    {
        DataSet ds = PhoenixVesselAccountsRH.RHStartHourEdit(new Guid(ViewState["RHSTARTID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtEmpName.Text = dr["FLDNAME"].ToString();
            txtStartDate.Text = string.Format("{0:dd/MMM/yyyy}", dr["FLDSTARTDATE"].ToString());
            ucRank.SelectedRank = dr["FLDRANKID"].ToString();
        }
    }
    private void BindUnlockedStatus()
    {
        DataSet ds = PhoenixVesselAccountsRH.RHUnlockedStatusBind(
           int.Parse(ViewState["EMPID"].ToString())
            , int.Parse(ViewState["MONTH"].ToString())
            , int.Parse(ViewState["YEAR"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            RadtxtUnlockStatus.Text = dr["FLDUNLOCKEDSTATUS"].ToString();
            txtRemarks.Text = dr["FLDHODREMARKS"].ToString();

            if (dr["FLDNCEXISTSYN"].ToString() == "0")
                rdLevel.Enabled = false;
            else
                rdLevel.Enabled = true;
            ViewState["NCEXISTSYN"] = dr["FLDNCEXISTSYN"].ToString();
            General.RadBindCheckBoxList(rdLevel, dr["FLDNCLEVEL"].ToString());

        }
    }
    private void UnlockStatusUpdate()
    {
        RadWindowManager1.RadConfirm("Are you sure you want to unlock this month to edit Work Hours?", "ConfirmUnlock", 320, 150, null, "ConfirmUnlock");
        return;
    }
    protected void BindMonth()
    {
        DataSet dsMonth = PhoenixVesselAccountsRestHourReports.RestHourReportMonth(int.Parse(ViewState["EMPID"].ToString()), General.GetNullableGuid(ViewState["RESTHOUREMPLOYEEID"].ToString()));
        ddlMonth.DataValueField = "FLDMONTHYEARID";
        ddlMonth.DataTextField = "FLDMONTHNAME";
        ddlMonth.DataSource = dsMonth;
        ddlMonth.DataBind();
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string[] alColumns = { "FLDNATUREOFWORKTEXT", "FLDSYSTEMCAUSESTEXT", "FLDCORRECTIVEACTIONTEXT", "FLDREPORTINGDAY", "FLDDATE", "FLDHOURS", "FLDCLOCKNAME", "FLDTOTALHOURS", "FLDRESTHOURS", "FLDNOOFCOMPLIANCES" };
            string[] alCaptions = { "Reasons For NC", "System Causes", "Corrective Action", "Reporting day", "Date", "Hours", "IDL", "Work Hours", "Rest Hours", "No.of Non Compliance" };


            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            ds = PhoenixVesselAccountsRH.SearchRHWorkCalender
                                   (
                                           int.Parse(ViewState["EMPID"].ToString()),
                                           PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                           new Guid(ViewState["RHSTARTID"].ToString()),
                                           (int)ViewState["PAGENUMBER"],
                                           General.ShowRecords(null),
                                           ref iRowCount,
                                           ref iTotalPageCount,
                                           int.Parse(ViewState["MONTH"].ToString()),
                                           int.Parse(ViewState["YEAR"].ToString())
                                   );

            Response.AddHeader("Content-Disposition", "attachment; filename=Attendance.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Attendance</h3></td>");
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
                if (Request.QueryString["launchfrom"] != null)
                    Response.Redirect("../CrewOffshore/CrewOffshoreCrewList.aspx");
                else
                {
                    if (Request.QueryString["MODELYN"] != null && Request.QueryString["MODELYN"].ToString() == "Y")
                        Response.Redirect("../VesselAccounts/VesselAccountsEmployeeQuery.aspx");
                    else
                        Response.Redirect("../VesselAccounts/VesselAccountsRHConfiguration.aspx?RESTHOURSTARTID=" + ViewState["RHSTARTID"].ToString());
                }
            }
            else if (CommandName.ToUpper().Equals("WORKCALENDER"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsRHWorkCalender.aspx?EMPID=" + ViewState["EMPID"].ToString() + "&RESTHOURSTARTID=" + ViewState["RHSTARTID"].ToString() + "&SHIPSTARTCALENDARID=" + ViewState["SHIPSTARTCALENDARID"].ToString());
            }
            else if (CommandName.ToUpper().Equals("TIMESHEET"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsRHTimeSheet.aspx?EMPID=" + ViewState["EMPID"].ToString() + "&RESTHOURSTARTID=" + ViewState["RHSTARTID"].ToString() + "&SHIPSTARTCALENDARID=" + ViewState["SHIPSTARTCALENDARID"].ToString());
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

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("ADD"))
            {
                AddWorkDay();
            }
            else if (CommandName.ToUpper().Equals("RECONCILE"))
            {
                RadWindowManager1.RadConfirm("Are you sure you want to Reconcile the Work Hours?", "ConfirmReconcile", 320, 150, null, "ConfirmReconcile");
                return;
            }
            else if (CommandName.ToUpper().Equals("SEAFARER"))
            {
                ViewState["VERIFIEDBY"] = 1;
                RadWindowManager1.RadConfirm("Are you sure you want to Verify this month Work Hours?", "ConfirmVerify", 320, 150, null, "Confirm Review");
                return;
            }
            else if (CommandName.ToUpper().Equals("HOD"))
            {
                ViewState["VERIFIEDBY"] = 2;
                RadWindowManager1.RadConfirm("Are you sure you want to Verify this month Work Hours?", "ConfirmVerify", 320, 150, null, "Confirm Review");
                return;
            }
            else if (CommandName.ToUpper().Equals("MASTER"))
            {
                ViewState["VERIFIEDBY"] = 3;
                RadWindowManager1.RadConfirm("Are you sure you want to Verify this month Work Hours?", "ConfirmVerify", 320, 150, null, "Confirm Review");
                return;
            }
            else if (CommandName.ToUpper().Equals("OFFICE"))
            {
                ViewState["VERIFIEDBY"] = 4;
                RadWindowManager1.RadConfirm("Are you sure you want to Verify this month Work Hours?", "ConfirmVerify", 320, 150, null, "Confirm Review");
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void AddWorkDay()
    {
        try
        {
            Guid? resthourcalendarid = null;
            int? reportingday = null;
            decimal? hours = null;

            PhoenixVesselAccountsRH.InsertRestHourWorkCalender(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                           new Guid(ViewState["RHSTARTID"].ToString()),
                           PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                           int.Parse(ViewState["EMPID"].ToString()),
                           int.Parse(ViewState["SHIPSTARTCALENDARID"].ToString()),
                           ref resthourcalendarid,
                           ref reportingday,
                           ref hours);

            PhoenixVesselAccountsRH.InsertRestHourWorkGenerate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                   new Guid(ViewState["RHSTARTID"].ToString()),
                   resthourcalendarid,
                   PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                   int.Parse(ViewState["EMPID"].ToString()),
                   reportingday,
                   24);
            //ucStatus.Text = "Work hours for today generated.";
            RadWindowManager1.RadAlert("Work hours generated successfully.", 320, 150, null, "");
            Rebind();
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDNATUREOFWORKTEXT", "FLDSYSTEMCAUSESTEXT", "FLDCORRECTIVEACTIONTEXT", "FLDREPORTINGDAY", "FLDDATE", "FLDHOURS", "FLDCLOCKNAME", "FLDTOTALHOURS", "FLDRESTHOURS", "FLDNOOFCOMPLIANCES" };
            string[] alCaptions = { "Reasons For NC", "System Causes", "Corrective Action", "Reporting day", "Date", "Hours", "IDL", "Work Hours", "Rest Hours", "No.of Non Compliance" };

            DataSet ds = PhoenixVesselAccountsRH.SearchRHWorkCalender
                                    (
                                            int.Parse(ViewState["EMPID"].ToString()),
                                            PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                            new Guid(ViewState["RHSTARTID"].ToString()),
                                            (int)ViewState["PAGENUMBER"],
                                            gvWorkCalender.PageSize,
                                            ref iRowCount,
                                            ref iTotalPageCount,
                                            int.Parse(ViewState["MONTH"].ToString()),
                                            int.Parse(ViewState["YEAR"].ToString())
                                    );

            General.SetPrintOptions("gvWorkCalender", "Attendance", alCaptions, alColumns, ds);

            gvWorkCalender.DataSource = ds;
            gvWorkCalender.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtNOdays.Text = ds.Tables[0].Rows[0]["FLDNOOFDAYS"].ToString();
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            BindUnlockedStatus();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvWorkCalender_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                DataRowView drv = (DataRowView)item.DataItem;

                ImageButton sb = (ImageButton)e.Item.FindControl("cmdWorkingHourAdd");
                if (sb != null)
                {
                    sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);
                    if (drv["FLDISCONFIRMED"].ToString() == "1")
                    {
                        sb.ImageUrl = Session["images"] + "/45.png";
                        sb.ToolTip = "Work Hours Verified";
                    }
                        
                }

             
                int? Validate = 1;
                RadLabel PendingActivities = (RadLabel)e.Item.FindControl("lblpendingactivity");
                Validate = General.GetNullableInteger(PendingActivities.Text);




                RadLabel CalenderId = (RadLabel)e.Item.FindControl("lblCalenderId");
                RadLabel reportingDay = (RadLabel)e.Item.FindControl("lblReportingDay");
                RadLabel ShipCalendarid = (RadLabel)e.Item.FindControl("lblShipCalenderId");
                RadLabel NoCompliance = (RadLabel)e.Item.FindControl("lblnoofNonCompliance");
                RadLabel MonthId = (RadLabel)e.Item.FindControl("lblMonthId");
                RadLabel Year = (RadLabel)e.Item.FindControl("lblYear");
                if (Validate == 0 || drv["FLDISCONFIRMED"].ToString() == "1")
                {
                    if (CalenderId != null && sb != null && ViewState["EMPID"] != null && ViewState["RHSTARTID"] != null && ShipCalendarid != null && drv["FLDDRAGTOSELECTYN"].ToString() != "Y")
                        sb.Attributes.Add("onclick", "openNewWindow('MoreInfo', '', '" + Session["sitepath"] + "/VesselAccounts/VesselAccountsRHWorkHourRecordDragToSelect.aspx?CalenderId=" + CalenderId.Text + "&EMPID=" + ViewState["EMPID"].ToString() + "&RHStartId=" + ViewState["RHSTARTID"].ToString() + "&SHIPCALENDERID=" + ShipCalendarid.Text + "&NOOFCOMPLIANCES=" + NoCompliance.Text + "&MONTHID=" + MonthId.Text + "&YEAR=" + Year.Text + "'); return false;");
                    else if (CalenderId != null && sb != null && ViewState["EMPID"] != null && ViewState["RHSTARTID"] != null && ShipCalendarid != null && drv["FLDDRAGTOSELECTYN"].ToString() == "Y")
                        sb.Attributes.Add("onclick", "openNewWindow('MoreInfo', '', '" + Session["sitepath"] + "/VesselAccounts/VesselAccountsRHWorkHourRecordDragToSelect.aspx?CalenderId=" + CalenderId.Text + "&EMPID=" + ViewState["EMPID"].ToString() + "&RHStartId=" + ViewState["RHSTARTID"].ToString() + "&SHIPCALENDERID=" + ShipCalendarid.Text + "&NOOFCOMPLIANCES=" + NoCompliance.Text + "&MONTHID=" + MonthId.Text + "&YEAR=" + Year.Text + "'); return false;");
                }
                
                UserControlToolTip ucNW = (UserControlToolTip)e.Item.FindControl("ucToolTipNW");
                UserControlToolTip ucNC = (UserControlToolTip)e.Item.FindControl("ucToolTipNC");

                //UserControlToolTip ucToolTipReason = (UserControlToolTip)e.Item.FindControl("ucToolTipReason");
                //RadLabel lblReasons = (RadLabel)e.Item.FindControl("lblReasons");
                //if (lblReasons != null)
                //{
                //    ucToolTipReason.Position = ToolTipPosition.TopCenter;
                //    ucToolTipReason.TargetControlId = lblReasons.ClientID;
                //}

                //UserControlToolTip ucToolTipSystemcauses = (UserControlToolTip)e.Item.FindControl("ucToolTipSystemcauses");
                //RadLabel lblSystemCauses = (RadLabel)e.Item.FindControl("lblSystemCauses");
                //if (lblSystemCauses != null)
                //{
                //    ucToolTipSystemcauses.Position = ToolTipPosition.TopCenter;
                //    ucToolTipSystemcauses.TargetControlId = lblSystemCauses.ClientID;
                //}
                
                //UserControlToolTip ucToolTipCorrectiveAction = (UserControlToolTip)e.Item.FindControl("ucToolTipCorrectiveAction");
                //RadLabel lblCorrectiveAction = (RadLabel)e.Item.FindControl("lblCorrectiveAction");
                //if(lblCorrectiveAction != null)
                //{
                //    ucToolTipCorrectiveAction.Position = ToolTipPosition.TopCenter;
                //    ucToolTipCorrectiveAction.TargetControlId = lblCorrectiveAction.ClientID;
                //}

                //DataRowView drv = (DataRowView)e.Item.DataItem;
                if (drv != null)
                {
                    ImageButton imgFlagNW = (ImageButton)e.Item.FindControl("ImgFlagNW");
                    imgFlagNW.Visible = false;
                    imgFlagNW.Attributes.Add("onclick", "openNewWindow('nature', '', '" + Session["sitepath"] + "/VesselAccounts/VesselAccountsRHNatureOfWork.aspx?CalenderId=" + CalenderId.Text + "&EMPID=" + ViewState["EMPID"].ToString() + "&RHStartId=" + ViewState["RHSTARTID"].ToString() + "'); return false;");
                    imgFlagNW.ToolTip = drv["FLDNATUREOFWORKTEXT"].ToString().TrimEnd(',');
                    if (imgFlagNW != null)
                    {
                        ucNW.Position = ToolTipPosition.TopCenter;
                        ucNW.TargetControlId = imgFlagNW.ClientID;
                    }
                    if (NoCompliance != null && !string.IsNullOrEmpty(NoCompliance.Text) && int.Parse(NoCompliance.Text) > 0)
                    {
                        ImageButton imgFlag = (ImageButton)e.Item.FindControl("ImgFlag");
                        imgFlag.Visible = true;
                        imgFlag.Attributes.Add("onclick", "openNewWindow('ncwin', '', '" + Session["sitepath"] + "/VesselAccounts/VesselAccountsRHWorkCalenderRemarks.aspx?CalenderId=" + CalenderId.Text + "&EMPID=" + ViewState["EMPID"].ToString() + "&RHStartId=" + ViewState["RHSTARTID"].ToString() + "'); return true;");
                        imgFlag.ToolTip = drv["FLDNCTEXT"].ToString().TrimEnd(',');
                        if (imgFlag != null)
                        {
                            ucNC.Position = ToolTipPosition.TopCenter;
                            ucNC.TargetControlId = imgFlag.ClientID;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvWorkCalender_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvWorkCalender.CurrentPageIndex + 1;
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
        gvWorkCalender.EditIndexes.Clear();
        gvWorkCalender.SelectedIndexes.Clear();
        gvWorkCalender.DataSource = null;
        gvWorkCalender.Rebind();
        BindUnlockedStatus();
    }

    protected void gvWorkCalender_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        if (e.CommandName == "Add")
        {
            ucError.ErrorMessage = "Your records cannot be verified as there are activities / work orders in progress. Please contact your Head of Department or the Master.";
            ucError.Visible = true;
        }
    }

    protected void ucConfirmReconcile_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixVesselAccountsRH.InsertRestHourMissingCalendarWorkInsert(new Guid(ViewState["RHSTARTID"].ToString()));
            RadWindowManager1.RadAlert("Reconciled Successfully.", 320, 150, null, "");
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        string Month = ddlMonth.SelectedValue.ToString();
        string[] SplitMonth = Month.Split(new Char[] { '-' });
        ViewState["MONTH"] = SplitMonth[0];
        ViewState["YEAR"] = SplitMonth[1];
        Rebind();
        BindToolbar();
    }

    protected void ucConfirmWorkHours_Click(object sender, EventArgs e)
    {
        try
        {
            string level = ",";

            foreach (string c in rdLevel.SelectedValues)
            {
                level = level + c + ',';
            }
            if (level == ",")
                level = string.Empty;

            if (ViewState["NCEXISTSYN"].ToString() == "1" && General.GetNullableString(level)==null)
            {
                ucError.ErrorMessage = "NC Level required.";
                ucError.Visible = true;
                return;
            }

            if (ViewState["VERIFIEDBY"].ToString() == "1")       //seafarer
            {
                PhoenixVesselAccountsRH.WorkHoursReviewedUpdate(int.Parse(ViewState["EMPID"].ToString())
                    , int.Parse(ViewState["MONTH"].ToString())
                    , int.Parse(ViewState["YEAR"].ToString())
                    , int.Parse(ViewState["VERIFIEDBY"].ToString()));
                RadWindowManager1.RadAlert("Seafarer Verified Calendar work and rest hours.", 320, 150, null, "");
            }
            if (ViewState["VERIFIEDBY"].ToString() == "2")  //hod
            {
                PhoenixVesselAccountsRH.WorkHoursReviewedUpdate(int.Parse(ViewState["EMPID"].ToString())
                    , int.Parse(ViewState["MONTH"].ToString())
                    , int.Parse(ViewState["YEAR"].ToString())
                    , int.Parse(ViewState["VERIFIEDBY"].ToString()));
                RadWindowManager1.RadAlert("HOD Reviewed Calendar work and rest hours.", 320, 150, null, "");
            }
            if (ViewState["VERIFIEDBY"].ToString() == "3")  //master
            {
                PhoenixVesselAccountsRH.WorkHoursReviewedUpdate(int.Parse(ViewState["EMPID"].ToString())
                    , int.Parse(ViewState["MONTH"].ToString())
                    , int.Parse(ViewState["YEAR"].ToString())
                    , int.Parse(ViewState["VERIFIEDBY"].ToString()));
                RadWindowManager1.RadAlert("Master Reviewed Calendar work and rest hours.", 320, 150, null, "");
            }
            if (ViewState["VERIFIEDBY"].ToString() == "4")  //office
            {
                PhoenixVesselAccountsRH.WorkHoursReviewedUpdate(int.Parse(ViewState["EMPID"].ToString())
                    , int.Parse(ViewState["MONTH"].ToString())
                    , int.Parse(ViewState["YEAR"].ToString())
                    , int.Parse(ViewState["VERIFIEDBY"].ToString()));
                RadWindowManager1.RadAlert("Office Reviewed Calendar work and rest hours.", 320, 150, null, "");
            }


            
            
                

            PhoenixVesselAccountsRH.WorkHoursReviewedRemarksUpdate(int.Parse(ViewState["EMPID"].ToString())
                    , int.Parse(ViewState["MONTH"].ToString())
                    , int.Parse(ViewState["YEAR"].ToString())
                    , General.GetNullableString(level)
                    , General.GetNullableString(txtRemarks.Text)
                    );

            Rebind();
            BindToolbar();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuRHUnlock_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("UNLOCK"))
            {
                UnlockStatusUpdate();
            }
            else if (CommandName.ToUpper().Equals("SEAFARER"))
            {
                                
                ViewState["VERIFIEDBY"] = 1;
                RadWindowManager1.RadConfirm("Are you sure you want to Verify this month Work Hours?", "ConfirmVerify", 320, 150, null, "ConfirmVerify");
                return;
            }
            else if (CommandName.ToUpper().Equals("HOD"))
            {
                ViewState["VERIFIEDBY"] = 2;
                RadWindowManager1.RadConfirm("Are you sure you want to Verify this month Work Hours?", "ConfirmVerify", 320, 150, null, "ConfirmVerify");
                return;
            }
            else if (CommandName.ToUpper().Equals("MASTER"))
            {
                ViewState["VERIFIEDBY"] = 3;
                RadWindowManager1.RadConfirm("Are you sure you want to Verify this month Work Hours?", "ConfirmVerify", 320, 150, null, "ConfirmVerify");
                return;
            }
            else if (CommandName.ToUpper().Equals("OFFICE"))
            {
                ViewState["VERIFIEDBY"] = 4;
                RadWindowManager1.RadConfirm("Are you sure you want to Verify this month Work Hours?", "ConfirmVerify", 320, 150, null, "ConfirmVerify");
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirmUnlock_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixVesselAccountsRH.WRHUnlockStatusUpdate(int.Parse(ViewState["EMPID"].ToString())
                    , int.Parse(ViewState["MONTH"].ToString())
                    , int.Parse(ViewState["YEAR"].ToString())
                    );  
            RadWindowManager1.RadAlert("Seafarer work hours unlocked to edit and verify.", 320, 150, null, "");
            Rebind();
            BindToolbar();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmddashboardactivityvalidation_Click(object sender, EventArgs e)
    {
        ucError.ErrorMessage = "Your records cannot be verified as there are activities / work orders in progress. Please contact your Head of Department or the Master.";
        ucError.Visible = true;
        
    }
    protected void BindLevel()
    {
        DataSet ds = PhoenixRegistersHardExtn.ListHardExtn(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 284, 0, null);
        rdLevel.DataSource = ds;
        rdLevel.DataBind();

    }
}
