using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsRHWorkCalendarShip : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            ///toolbarmain.AddButton("OPA90", "OPA", ToolBarDirection.Right);
            toolbarmain.AddButton("Calendar", "WORKCALENDER", ToolBarDirection.Left);
            toolbarmain.AddButton("OPA90", "OPA", ToolBarDirection.Left);
            toolbarmain.AddButton("Crew", "CREW", ToolBarDirection.Left);
            
            MenuRHGeneral.AccessRights = this.ViewState;
            MenuRHGeneral.MenuList = toolbarmain.Show();
            MenuRHGeneral.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                gvWorkCalender.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                BindYear();
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsRHWorkCalendarShip.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvWorkCalendar')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            //toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsRHWorkCalendarShip.aspx", "Add One Month", "<i class=\"fas fa-calendar-alt\"></i>", "BULKADD");
            //toolbar.AddFontAwesomeButton("javascript:parent.Openpopup('MoreInfo','','VesselAccountsRHWorkCalenderGeneral.aspx? '); return false;", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuWorkHour.AccessRights = this.ViewState;
            MenuWorkHour.MenuList = toolbar.Show();
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
                Response.Redirect("../VesselAccounts/VesselAccountsRHConfiguration.aspx", false);
            }
            else if (CommandName.ToUpper().Equals("WORKCALENDER"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsRHWorkCalendarShip.aspx", false);
            }
            else if (CommandName.ToUpper().Equals("DESIGNATION"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsRHDesignation.aspx", false);
            }
            else if (CommandName.ToUpper().Equals("OPA"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsRHOPARequirement.aspx", false);
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string[] alColumns = { "FLDDATE", "FLDHOURS", "FLDCLOCKNAME", "FLDADVANCERETARDNAME", "FLDWORKPLACENAME" };
            string[] alCaptions = { "Date", "Hours", "IDL [E-W (or) W-E (or) NONE]", "Advance/Retard/Reset", "Work At" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            ds = PhoenixVesselAccountsRH.RestHourShipWorkCalendarSearch
                                    (PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                            General.GetNullableInteger(ddlYear.SelectedValue), General.GetNullableInteger(ddlmonth.SelectedValue),
                                            (int)ViewState["PAGENUMBER"],
                                            iRowCount,
                                            ref iRowCount,
                                            ref iTotalPageCount
                                    );

            Response.AddHeader("Content-Disposition", "attachment; filename=Calendar.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Calendar</h3></td>");
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindYear()
    {
        for (int i = 2012; i <= DateTime.Now.Year; i++)
        {
            ddlYear.Items.Add(i.ToString());
        }
        ddlYear.DataBind();
        ddlmonth.SelectedValue = DateTime.Now.Month.ToString();
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
            if (CommandName.ToUpper().Equals("BULKADD"))
            {
                PhoenixVesselAccountsRH.ShipCalendarBulkInsert(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDDATE", "FLDHOURS", "FLDCLOCKNAME", "FLDADVANCERETARDNAME", "FLDWORKPLACENAME" };
            string[] alCaptions = { "Date", "Hours", "IDL [E-W (or) W-E (or) NONE]", "Advance/Retard/Reset", "Work At" };

            DataSet ds = PhoenixVesselAccountsRH.RestHourShipWorkCalendarSearch
                                    (PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                            General.GetNullableInteger(ddlYear.SelectedItem.Text), General.GetNullableInteger(ddlmonth.SelectedValue),
                                            (int)ViewState["PAGENUMBER"],
                                            gvWorkCalender.PageSize,
                                            ref iRowCount,
                                            ref iTotalPageCount
                                    );

            General.SetPrintOptions("gvWorkCalendar", "Calendar", alCaptions, alColumns, ds);

            gvWorkCalender.DataSource = ds;
            gvWorkCalender.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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
                LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
                if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

                RadLabel CalenderId = (RadLabel)e.Item.FindControl("lblShipCalendarId");

                eb.Attributes.Add("onclick", "openNewWindow('code1', '', '" + Session["sitepath"] + "/VesselAccounts/VesselAccountsRHWorkCalenderGeneral.aspx?shipresthourid=" + CalenderId.Text + "');return true;");
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
    }

    protected void gvWorkCalender_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void ddlYear_DataBound(object sender, EventArgs e)
    {
        ddlYear.Items.Sort();
    }

    protected void cmdHiddenSubmit_Click1(object sender, EventArgs e)
    {
        Rebind();
    }
}
