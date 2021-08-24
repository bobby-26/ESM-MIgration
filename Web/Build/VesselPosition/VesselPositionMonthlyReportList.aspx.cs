﻿using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselPosition;
using Telerik.Web.UI;

public partial class VesselPositionMonthlyReportList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbarnoonreportlist = new PhoenixToolbar();
            if (Request.QueryString["vesselid"] != null)
            {
                toolbarnoonreportlist.AddFontAwesomeButton("../VesselPosition/VesselPositionMonthlyReportList.aspx?vesselid=" + Request.QueryString["vesselid"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
                toolbarnoonreportlist.AddFontAwesomeButton("javascript:CallPrint('gvMonthlyReport')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
                toolbarnoonreportlist.AddFontAwesomeButton("../VesselPosition/VesselPositionMonthlyReportList.aspx?vesselid=" + Request.QueryString["vesselid"], "Find", "<i class=\"fas fa-search\"></i>", "FIND");
                toolbarnoonreportlist.AddFontAwesomeButton("../VesselPosition/VesselPositionMonthlyReportList.aspx?vesselid=" + Request.QueryString["vesselid"], "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "RESET");
            }
            else
            {
                toolbarnoonreportlist.AddFontAwesomeButton("../VesselPosition/VesselPositionMonthlyReportList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
                toolbarnoonreportlist.AddFontAwesomeButton("javascript:CallPrint('gvMonthlyReport')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
                toolbarnoonreportlist.AddFontAwesomeButton("../VesselPosition/VesselPositionMonthlyReportList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
                toolbarnoonreportlist.AddFontAwesomeButton("../VesselPosition/VesselPositionMonthlyReportList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "RESET");
            }
            

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                toolbarnoonreportlist.AddFontAwesomeButton("../VesselPosition/VesselPositionMonthlyReport.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            
            MenuMonthlyReportList.AccessRights = this.ViewState;
            MenuMonthlyReportList.MenuList = toolbarnoonreportlist.Show();


            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["VesselId"] = "";
                ViewState["OVERDUE"] = Request.QueryString["overdue"] != null ? "1" : "";
                ViewState["REVIEW"] = Request.QueryString["review"] != null ? "1" : "";
                //BindData();
                BindVesselFleetList();

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                {
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ViewState["VesselId"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    DataSet ds = PhoenixRegistersVessel.EditVesselCrewRelatedInfo(Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()));
                    DataTable dt = ds.Tables[0];
                    DataRow dr = dt.Rows[0];
                    ddlFleet.SelectedValue = dr["FLDTECHFLEET"].ToString();
                    if (PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() != "OWNER")
                    {
                        UcVessel.Enabled = false;
                        ddlFleet.Enabled = false;
                    }
                }
                else if (Request.QueryString["vesselid"] != null)
                {
                    UcVessel.SelectedVessel = Request.QueryString["vesselid"].ToString();
                    UcVessel.Enabled = false;
                    ddlFleet.Enabled = false;
                }
                gvMonthlyReport.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    protected void VoyageListList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("ADD"))
        {
            Response.Redirect("VesselPositionMonthlyReport.aspx", false);
        }
        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            Rebind();
        }
        if (CommandName.ToUpper().Equals("RESET"))
        {
            ClearFilter();
            Rebind();
        }
    }
    protected void ddlVessel_Changed(object sender, EventArgs e)
    {
        ViewState["VesselId"] = UcVessel.SelectedVessel;
        if (General.GetNullableInteger(UcVessel.SelectedVessel) != null)
        {
            DataSet ds = PhoenixRegistersVessel.EditVesselCrewRelatedInfo(Convert.ToInt32(UcVessel.SelectedVessel));
            DataTable dt = ds.Tables[0];
            DataRow dr = dt.Rows[0];
            ddlFleet.SelectedValue = dr["FLDTECHFLEET"].ToString();
        }
        else
        {
            ddlFleet.SelectedIndex = 0;
        }
        Rebind();
    }
    private void ClearFilter()
    {
        UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0 ? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() : (Request.QueryString["vesselid"] != null ? Request.QueryString["vesselid"].ToString() : "");
        DataSet ds = PhoenixRegistersVessel.EditVesselCrewRelatedInfo(Convert.ToInt32(UcVessel.SelectedVessel));
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            string FleetID = dr["FLDTECHFLEET"].ToString();
            if (FleetID != string.Empty && General.GetNullableInteger(FleetID) != null)
                ddlFleet.SelectedValue = FleetID ;
        }
        else
        {
            ddlFleet.SelectedValue = "Dummy";
        }

        Session["MONTHLYREPORTID"] = null;
        ViewState["PAGENUMBER"] = 1;
    }
    protected void gvMonthlyReport_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }    
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDREPORTMONTHANDYEAR", "FLDVESSELNAME", "FLDTOTALSTEAMINGTIME", "FLDTOTALMANOEVERINGTIME", "FLDTOTALAVGBHP", "FLDTOTALDEVIATIONORDELAY", "FLDTOTALMESTOPPAGE", "FLDTOTALAVGSPEED", "FLDTOTALAVGFOCONSUMPTIONPERDAY" };
        string[] alCaptions = { "Month", "Vessel", "Toatal Steaming Time", "Total Manoeuvering Time", "Avg BHP", "Deviation or Delay", "M/E Stoppage", "Avg Speed", "Avg FO Cons" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? isoverdue = General.GetNullableInteger(ViewState["OVERDUE"].ToString());
        int? isreview = General.GetNullableInteger(ViewState["REVIEW"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = new DataSet();
        ds = PhoenixVesselPositionMonthlyReport.MonthlyReportSearch(General.GetNullableInteger(UcVessel.SelectedVessel),
                                                General.GetNullableInteger(ddlFleet.SelectedValue),                                  
                                                sortexpression,
                                                sortdirection,
                                                (int)ViewState["PAGENUMBER"],
                                                gvMonthlyReport.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount,
                                                isoverdue,
                                                isreview
                                              );

        Response.AddHeader("Content-Disposition", "attachment; filename=\"MontlyReport.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Request.Url.Scheme + "://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Monthly Report</h3></td>");
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

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }


    protected void gvMonthlyReport_RowCommand(object sender, GridCommandEventArgs e)
    {

        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper() == "EDIT")
            {
                string MonthlyReportID = ((RadLabel)e.Item.FindControl("lblMonthlyReportID")).Text;
                Session["MONTHLYREPORTID"] = MonthlyReportID;
                if (Request.QueryString["vesselid"] != null)
                    Response.Redirect("VesselPositionMonthlyReport.aspx?MonthlyReportID=" + MonthlyReportID + "&vesselid=" + Request.QueryString["vesselid"], false);
                else
                    Response.Redirect("VesselPositionMonthlyReport.aspx?MonthlyReportID=" + MonthlyReportID, false);
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionMonthlyReport.DeleteMonthlyReport((PhoenixSecurityContext.CurrentSecurityContext.UserCode), General.GetNullableGuid( ((RadLabel)e.Item.FindControl("lblMonthlyReportID")).Text)       );
                Rebind();
            }
            else if (e.CommandName == "Page")
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
    private void BindVesselFleetList()
    {
        DataSet ds = PhoenixRegistersFleet.ListFleet();
        ddlFleet.Items.Add("select");
        ddlFleet.DataSource = ds;
        ddlFleet.DataTextField = "FLDFLEETDESCRIPTION";
        ddlFleet.DataValueField = "FLDFLEETID";
        ddlFleet.DataBind();
        ddlFleet.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void gvMonthlyReport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMonthlyReport.CurrentPageIndex + 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDREPORTMONTHANDYEAR", "FLDVESSELNAME", "FLDTOTALSTEAMINGTIME", "FLDTOTALMANOEVERINGTIME", "FLDTOTALAVGBHP", "FLDTOTALDEVIATIONORDELAY", "FLDTOTALMESTOPPAGE", "FLDTOTALAVGSPEED", "FLDTOTALAVGFOCONSUMPTIONPERDAY" };
        string[] alCaptions = { "Month", "Vessel", "Toatal Steaming Time", "Total Manoeuvering Time", "Avg BHP", "Deviation or Delay", "M/E Stoppage", "Avg Speed", "Avg FO Cons" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        int? isoverdue = General.GetNullableInteger(ViewState["OVERDUE"].ToString());
        int? isreview = General.GetNullableInteger(ViewState["REVIEW"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        ds = PhoenixVesselPositionMonthlyReport.MonthlyReportSearch(General.GetNullableInteger(UcVessel.SelectedVessel),
                                                                    General.GetNullableInteger(ddlFleet.SelectedValue),
                                                                    sortexpression,
                                                                    sortdirection,
                                                                    (int)ViewState["PAGENUMBER"],
                                                                    gvMonthlyReport.PageSize,
                                                                    ref iRowCount,
                                                                    ref iTotalPageCount,
                                                                    isoverdue,
                                                                    isreview
                                                                  );

        General.SetPrintOptions("gvMonthlyReport", "Monthly Report", alCaptions, alColumns, ds);

        gvMonthlyReport.DataSource = ds;
        gvMonthlyReport.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void Rebind()
    {
        gvMonthlyReport.SelectedIndexes.Clear();
        gvMonthlyReport.EditIndexes.Clear();
        gvMonthlyReport.DataSource = null;
        gvMonthlyReport.Rebind();
    }

    protected void gvMonthlyReport_PreRender(object sender, EventArgs e)
    {
        if (Session["MONTHLYREPORTID"] != null)
        {
            foreach (GridItem item in gvMonthlyReport.MasterTableView.Items)
            {
                if (item is GridDataItem)
                {
                    GridDataItem dataItem = (GridDataItem)item;
                    if (Session["MONTHLYREPORTID"].Equals(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["FLDVESSELMONTHLYREPORTID"].ToString()))
                    {
                        dataItem.Selected = true;
                        break;
                    }
                }
            }
        }
    }
}
