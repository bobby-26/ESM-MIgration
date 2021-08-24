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

public partial class VesselAccountsRHCrewList : PhoenixBasePage
{
    public string vesselname;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsRHCrewList.aspx" + (Request.QueryString["access"] != null ? "?access=1" : string.Empty), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            MenuCrewList.AccessRights = this.ViewState;
            MenuCrewList.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Attendance", "WORKCALENDER", ToolBarDirection.Right);
                toolbarmain.AddButton("Crew List", "CREW", ToolBarDirection.Right);

                MenuRHGeneral.AccessRights = this.ViewState;
                MenuRHGeneral.MenuList = toolbarmain.Show();
                MenuRHGeneral.SelectedMenuIndex = 1;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["RHSTARTID"] = null;
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

            if (ViewState["EMPID"] == null || ViewState["RHSTARTID"] == null)
            {
                //Response.Redirect("../VesselAccounts/VesselAccountsRHCrewList.aspx");
                ShowError();
                return;
            }
            if (CommandName.ToUpper().Equals("CREW"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsRHCrewList.aspx", false);
            }
            else if (CommandName.ToUpper().Equals("WORKCALENDER"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsRHWorkCalender.aspx?EMPID=" + ViewState["EMPID"].ToString() + "&RESTHOURSTARTID=" + ViewState["RHSTARTID"].ToString() + "&SHIPSTARTCALENDARID=" + ViewState["SHIPCALENDARID"].ToString(), false);
            }
            else if (CommandName.ToUpper().Equals("TIMESHEET"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsRHTimeSheet.aspx?EMPID=" + ViewState["EMPID"].ToString() + "&RESTHOURSTARTID=" + ViewState["RHSTARTID"].ToString() + "&SHIPSTARTCALENDARID=" + ViewState["SHIPCALENDARID"].ToString(), false);
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
        ucError.ErrorMessage = "Please select a Seafarer with start date and then Navigate to other Tabs";
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
        string[] alColumns = { "FLDROWNUMBER", "FLDNAME", "FLDRANKNAME", "FLDSIGNONDATE", "FLDRELIEFDUEDATE", "FLDSTARTDATE" };
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

        DataSet ds = PhoenixVesselAccountsRH.SearchRestHourStart(PhoenixSecurityContext.CurrentSecurityContext.VesselID, 1, sortexpression, sortdirection
                                            , 1
                                            , iRowCount
                                            , ref iRowCount
                                            , ref iTotalPageCount, 1);


        Response.AddHeader("Content-Disposition", "attachment; filename=Crew List.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Crew List</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
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

        DataSet ds = PhoenixVesselAccountsRH.SearchRestHourStart(PhoenixSecurityContext.CurrentSecurityContext.VesselID, 1, sortexpression, sortdirection
                                            , (int)ViewState["PAGENUMBER"]
                                            , gvCrewList.PageSize
                                            , ref iRowCount
                                            , ref iTotalPageCount, 1);

        General.SetPrintOptions("gvCrewList", "Crew List", alCaptions, alColumns, ds);

        gvCrewList.DataSource = ds;
        gvCrewList.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvCrewList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToString().ToUpper() == "SELECT")
        {
            try
            {
                string employeeid = ((RadLabel)e.Item.FindControl("lblEmpId")).Text;
                string rhstartid = ((RadLabel)e.Item.FindControl("lblRHstartid")).Text;
                string signondate = ((RadLabel)e.Item.FindControl("lblSignondate")).Text;
                string ShipCalendarId = ((RadLabel)e.Item.FindControl("lblShipCalendarId")).Text;
                string StartId = ((RadLabel)e.Item.FindControl("lblRHstartid")).Text;

                ViewState["EMPID"] = employeeid.ToString();
                ViewState["RHSTARTID"] = rhstartid.ToString() == "" ? null : rhstartid.ToString();
                ViewState["SHIPCALENDARID"] = ShipCalendarId.ToString();

                Response.Redirect("../VesselAccounts/VesselAccountsRHWorkCalender.aspx?EMPID=" + ViewState["EMPID"].ToString() + "&RESTHOURSTARTID=" + ViewState["RHSTARTID"].ToString() + "&SHIPSTARTCALENDARID=" + ViewState["SHIPCALENDARID"].ToString(), false);
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvCrewList_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
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
}
