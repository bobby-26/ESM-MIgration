using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Telerik.Web.UI;
using Telerik.Web.UI.Calendar.View;

public partial class PlannedMaintenance_PlannedMaintenanceDailyWorkPlan : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceDailyWorkPlan.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDailyWorkPlan')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:$modalWindow.modalWindowUrl = '../PlannedMaintenance/PlannedMaintenanceDailyWorkPlanAdd.aspx';showDialog('Add');", "Add Daily Work Plan", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        //toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceDailyWorkPlan.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
        //toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceDailyWorkPlan.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

        MenuDailyWorkPlan.AccessRights = this.ViewState;
        MenuDailyWorkPlan.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            //DateTime startDate = ((Telerik.Web.UI.Calendar.View.MonthView)txtDate.Calendar.CalendarView).MonthStartDate;
            //DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.ListShipCalendar(PhoenixSecurityContext.CurrentSecurityContext.VesselID, startDate.Year, startDate.Month);
            //Session["SHIPCAL"] = dt;
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvDailyWorkPlan.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);            
        }
    }
    //protected override void OnPreRender(EventArgs e)
    //{
    //    base.OnPreRender(e);

    //    //startDate = ((MonthView)txtDate.Calendar.CalendarView).MonthStartDate;        
    //}
    protected void MenuDailyWorkPlan_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("CLEAR"))
            {

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

        string[] alColumns = { "FLDPLANNO", "FLDVOYAGENUMBER", "FLDDATE", "FLDSTATUS" };
        string[] alCaptions = { "Daily Work Plan No.", "Voyage Plan No.", "Date", "Status" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.Search(PhoenixSecurityContext.CurrentSecurityContext.VesselID, 1
                                         , sortexpression, sortdirection
                                         , 1
                                         , iRowCount
                                         , ref iRowCount
                                         , ref iTotalPageCount);

        General.ShowExcel("Daily Work Plan", dt, alColumns, alCaptions, null, null);
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDPLANNO", "FLDVOYAGENUMBER", "FLDDATE", "FLDSTATUS" };
            string[] alCaptions = { "Daily Work Plan No.", "Voyage Plan No.", "Date", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.Search(PhoenixSecurityContext.CurrentSecurityContext.VesselID, 1
                                         , sortexpression, sortdirection
                                         , int.Parse(ViewState["PAGENUMBER"].ToString())
                                         , gvDailyWorkPlan.PageSize
                                         , ref iRowCount
                                         , ref iTotalPageCount);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvDailyWorkPlan", "Daily Work Plan", alCaptions, alColumns, ds);

            gvDailyWorkPlan.DataSource = dt;
            gvDailyWorkPlan.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDailyWorkPlan_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "PAGE")
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName.ToUpper() == "ADD")
            {
                string No = ((RadTextBox)e.Item.FindControl("txtPlanNumberAdd")).Text;
                string date = ((UserControlDate)e.Item.FindControl("txtDateAdd")).Text;

                if (!IsValidDailyWorkPlan(Guid.NewGuid().ToString(), date))
                {
                    ucError.Visible = true;
                    return;
                }
                //PhoenixPlannedMaintenanceDailyWorkPlan.Insert(PhoenixSecurityContext.CurrentSecurityContext.VesselID                    
                //    , null
                //    , General.GetNullableDateTime(date).Value);                    
                BindData();
                gvDailyWorkPlan.Rebind();
            }
            else if (e.CommandName.ToUpper() == "UPDATE")
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                string id = drv["FLDDAILYWORKPLANID"].ToString();
                string No = ((RadTextBox)e.Item.FindControl("txtPlanNumber")).Text;
                string date = ((UserControlDate)e.Item.FindControl("txtDate")).Text;

                if (!IsValidDailyWorkPlan(Guid.NewGuid().ToString(),date))
                {
                    ucError.Visible = true;
                    e.Canceled = true;
                    return;
                }
                //PhoenixPlannedMaintenanceDailyWorkPlan.Update(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID                     
                //     , null
                //     , General.GetNullableDateTime(date).Value);
                BindData();
                gvDailyWorkPlan.Rebind();
            }
            else if (e.CommandName.ToUpper() == "EDITR")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDDAILYWORKPLANID").ToString();
                string script = "$modalWindow.modalWindowUrl = '../PlannedMaintenance/PlannedMaintenanceDailyWorkPlanAdd.aspx?id=" + id + "';showDialog('Edit');";                
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            else if (e.CommandName.ToUpper() == "DELETE")
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                string id = drv["FLDPLANID"].ToString();
                BindData();
                gvDailyWorkPlan.Rebind();
            }
            else if (e.CommandName.ToUpper() == "SELECT")
            {
                string planid = e.CommandArgument.ToString();
                Response.Redirect("~/PlannedMaintenance/PlannedMaintenanceDailyWorkPlanDetail.aspx?p=" + planid);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDailyWorkPlan_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
            }
            LinkButton vp = (LinkButton)e.Item.FindControl("lnkVoyagePlan");
            if (vp != null)
            {                
                vp.Attributes.Add("onclick", "javascript:top.openNewWindow('vp','Voyage Plan','PlannedMaintenance/PlannedMaintenanceVoyagePlanDetail.aspx?p=" + drv["FLDVOYAGEPLANID"].ToString() + "'); return false;");
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDailyWorkPlan_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDailyWorkPlan.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDailyWorkPlan_SortCommand(object sender, GridSortCommandEventArgs e)
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
    private bool IsValidDailyWorkPlan(string voyage, string date)
    {
        ucError.HeaderMessage = "Please provide the following required information";       

        if (General.GetNullableGuid(voyage) == null)
            ucError.ErrorMessage = "Voyage Plan No. is required.";


        if (General.GetNullableDateTime(date) == null)
            ucError.ErrorMessage = "Date is required.";

        return (!ucError.IsError);
    }
    //protected void btnCreate_Click(object sender, EventArgs e)
    //{
    //    if (Page.IsValid)
    //    {
    //        try
    //        {
    //            var args = lblPlanId.Text;
    //            var array = args.Split(',');
    //            var id = array[0];
    //            var cmd = array[1];
    //            if (General.GetNullableGuid(id) == null)
    //            {
    //                PhoenixPlannedMaintenanceDailyWorkPlan.Insert(PhoenixSecurityContext.CurrentSecurityContext.VesselID
    //                    , null
    //                    , txtDate.SelectedDate.Value
    //                    , int.Parse(rblVesselStatus.SelectedValue)
    //                    , tpChangeTime.SelectedDate);
    //            }
    //            else
    //            {
    //                DateTime? changeTime = null;
    //                if (tpChangeTime.SelectedTime != null)
    //                {
    //                    changeTime = txtDate.SelectedDate.Value.AddMinutes(tpChangeTime.SelectedTime.Value.TotalMinutes);
    //                }
    //                PhoenixPlannedMaintenanceDailyWorkPlan.Update(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID
    //                , null
    //                , txtDate.SelectedDate.Value
    //                , int.Parse(rblVesselStatus.SelectedValue)
    //                , changeTime);
    //            }
    //            gvDailyWorkPlan.Rebind();
    //            string script = "function f(){CloseModelWindow(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
    //            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    //            lblPlanId.Text = string.Empty;
    //            txtNo.Text = string.Empty;
    //            txtDate.SelectedDate = null;
    //            btnCreate.Text = "Create";
    //        }
    //        catch (Exception ex)
    //        {
    //            RequiredFieldValidator Validator = new RequiredFieldValidator();
    //            Validator.ErrorMessage = "* " + ex.Message;
    //            //Validator.ValidationGroup = "Group1";
    //            Validator.IsValid = false;
    //            Validator.Visible = false;
    //            Page.Form.Controls.Add(Validator);
    //        }
    //    }
    //}
    //protected void RadAjaxPanel2_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
    //{        
    //    var args = e.Argument;
    //    var array = args.Split(',');
    //    var id = array[0];
    //    var cmd = array[1];
    //    if (cmd.ToUpper() == "ADD")
    //    {
    //        lblPlanId.Text = string.Empty;
    //        txtNo.Text = string.Empty;
    //        txtDate.SelectedDate = null;
    //        btnCreate.Text = "Create";
    //    }
    //    else if (cmd.ToUpper() == "EDIT")
    //    {
    //        DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.Edit(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
    //        if (dt.Rows.Count > 0)
    //        {
    //            DataRow dr = dt.Rows[0];
    //            lblPlanId.Text = e.Argument;
    //            txtNo.Text = dr["FLDPLANNO"].ToString();
    //            txtDate.SelectedDate = General.GetNullableDateTime(dr["FLDDATE"].ToString());
    //            rblVesselStatus.SelectedValue = dr["FLDVESSELSTATUS"].ToString();
    //            tpChangeTime.SelectedDate = General.GetNullableDateTime(dr["FLDCHANGETIME"].ToString());
    //            btnCreate.Text = "Save";
    //        }
    //    }
    //}

    //protected void rblVesselStatus_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (rblVesselStatus.SelectedValue == "3")
    //        RequiredFieldValidatorChangeTime.Enabled = true;
    //    else
    //    {
    //        tpChangeTime.SelectedDate = null;
    //        RequiredFieldValidatorChangeTime.Enabled = false;
    //    }
    //}    
    //protected void RadCalendar1_DefaultViewChanged(object sender, Telerik.Web.UI.Calendar.DefaultViewChangedEventArgs e)
    //{
    //    RadCalendar c = (RadCalendar)sender;
    //    startDate = ((Telerik.Web.UI.Calendar.View.MonthView)c.CalendarView).MonthStartDate;
    //    DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.ListShipCalendar(PhoenixSecurityContext.CurrentSecurityContext.VesselID, startDate.Year, startDate.Month);
    //    Session["SHIPCAL"] = dt;
    //}
    //private int i = 0;
    //protected void RadCalendar1_DayRender(object sender, Telerik.Web.UI.Calendar.DayRenderEventArgs e)
    //{
    //    DateTime CurrentDate = e.Day.Date;        
    //    if(CurrentDate >= startDate)
    //    {
    //        DataTable dt = (DataTable)Session["SHIPCAL"];
    //        if (dt != null && i < dt.Rows.Count)
    //        {
    //            DataRow dr = dt.Rows[i];
    //            e.Cell.Text= dr["FLDDAY"].ToString() + "-" + dr["FLDCLOCKNAME"].ToString();
    //            Label label = new Label();
    //            label.Text = dr["FLDDAY"].ToString() + "-" + dr["FLDCLOCKNAME"].ToString();
    //            e.Cell.Controls.Add(label);
    //            e.Cell.Attributes["data-day"] = dr["FLDDAY"].ToString() + "-" + dr["FLDCLOCKNAME"].ToString();
    //            i++;
    //        }
    //        else
    //        {
    //            RadCalendarDay calendarDay = new RadCalendarDay();
    //            calendarDay.Date = e.Day.Date;
    //            calendarDay.IsSelectable = false;
    //            //txtDate.Calendar.SpecialDays.Add(calendarDay);
    //        }
    //    }  
    //    else
    //    {
    //        RadCalendarDay calendarDay = new RadCalendarDay();
    //        calendarDay.Date = e.Day.Date;
    //        calendarDay.IsSelectable = false;
    //        //txtDate.Calendar.SpecialDays.Add(calendarDay);
    //    }     
    //}
}