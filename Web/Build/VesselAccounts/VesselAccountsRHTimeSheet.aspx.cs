﻿using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsRHTimeSheet : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbartimesheet = new PhoenixToolbar();
            toolbartimesheet.AddButton("NC", "NC", ToolBarDirection.Right);
            toolbartimesheet.AddButton("Exception", "EXCEPTION", ToolBarDirection.Right);
            toolbartimesheet.AddButton("Timesheet", "TIMESHEET", ToolBarDirection.Right);
            MenuTimesheet.MenuList = toolbartimesheet.Show();
            MenuTimesheet.SelectedMenuIndex = 2;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                gvTimeSheet.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsRHTimeSheet.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvTimeSheet')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsRHTimeSheet.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuWorkHour.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuTimesheet_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("TIMESHEET"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsRHTimeSheet.aspx", false);
            }
            if (CommandName.ToUpper().Equals("EXCEPTION"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsRHTimeSheetException.aspx", false);
            }
            if (CommandName.ToUpper().Equals("NC"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsRHTimeSheetNCIn2Days.aspx", false);
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

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROWNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDNOOFPERSON" };
        string[] alCaptions = { "S.No ", "Vessel Name", "Date", "No of Persons" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        ds = PhoenixVesselAccountsRH.Timesheetseach(General.GetNullableInteger(ucvessel.SelectedVessel),
                 1,
                 iRowCount,
                 ref iRowCount,
                 ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=TimeSheet.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Time Sheet</h3></td>");
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

    protected void MenuWorkHour_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROWNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDNOOFPERSON" };
        string[] alCaptions = { "S.No ", "Vessel Name", "Date", "No of Persons" };

        DataSet ds = new DataSet();

        ds = PhoenixVesselAccountsRH.Timesheetseach(General.GetNullableInteger(ucvessel.SelectedVessel),
           (int)ViewState["PAGENUMBER"],
           gvTimeSheet.PageSize,
           ref iRowCount,
           ref iTotalPageCount);

        General.SetPrintOptions("gvTimeSheet", "Time Sheet", alCaptions, alColumns, ds);

        gvTimeSheet.DataSource = ds;
        gvTimeSheet.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            txttotalresvessel.Text = ds.Tables[0].Rows[0]["TOTALVESSELCOUNT"].ToString();
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void gvTimeSheet_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTimeSheet.CurrentPageIndex + 1;
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
        gvTimeSheet.EditIndexes.Clear();
        gvTimeSheet.SelectedIndexes.Clear();
        gvTimeSheet.DataSource = null;
        gvTimeSheet.Rebind();
    }

    protected void gvTimeSheet_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
}
