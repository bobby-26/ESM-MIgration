using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using SouthNests.Phoenix.CrewReports;
using System.Data;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CrewReportsCrewChangeFormat3 : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            toolbar.AddButton("Compose Mail", "MAIL", ToolBarDirection.Right);

            MenuReportsFilter.MenuList = toolbar.Show();
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportsCrewChangeFormat3.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportsCrewChangeFormat3.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.MenuList = toolbar1.Show();
            PhoenixToolbar toolbar2 = new PhoenixToolbar();
            toolbar2.AddButton("Format3", "FORMAT3", ToolBarDirection.Right);
            toolbar2.AddButton("Format2", "FORMAT2", ToolBarDirection.Right);
            toolbar2.AddButton("Format1", "FORMAT1", ToolBarDirection.Right);
            MenuReport.AccessRights = this.ViewState;
            MenuReport.MenuList = toolbar2.Show();
            MenuReport.SelectedMenuIndex = 0;
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SHOWREPORT"] = null;
                rblFormats.SelectedIndex = 2;
                ucDate1.Text = DateTime.Now.ToShortDateString();
                NameValueCollection nvc = Filter.CurrentCrewChangeList;
                if (nvc != null)
                {
                    ddlVessel.SelectedValue = nvc.Get("ddlvessel");
                    ucDate.Text = nvc.Get("ucDate");
                    ucDate1.Text = nvc.Get("ucDate1");
                    ddlUnion.SelectedValue = nvc.Get("ddlUnion");

                    DataSet ds = new DataSet();
                    ds = PhoenixCrewReportsCrewChange.GetJSUVessels(int.Parse(nvc.Get("ddlUnion").ToString()));
                    ddlVessel.DataTextField = "FLDVESSELNAME";
                    ddlVessel.DataValueField = "FLDVESSELID";
                    ddlVessel.DataSource = ds;
                    ddlVessel.DataBind();
                    ddlVessel.Items.Insert(0, new RadComboBoxItem("--Select--", "0"));
                }
                DataSet ds1 = new DataSet();
                ds1 = PhoenixCrewReportsCrewChange.GetUnions();
                ddlUnion.DataTextField = "FLDNAME";
                ddlUnion.DataValueField = "FLDADDRESSCODE";
                ddlUnion.DataSource = ds1;
                ddlUnion.DataBind();
                ddlUnion.Items.Insert(0, new RadComboBoxItem("--Select--", "0"));
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuReport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FORMAT1"))
            {
                Response.Redirect("CrewReportsCrewChange.aspx", true);
                MenuReport.SelectedMenuIndex = 1;
            }
            if (CommandName.ToUpper().Equals("FORMAT2"))
            {
                Response.Redirect("CrewReportsCrewChangeFormat2.aspx", true);
                MenuReport.SelectedMenuIndex = 0;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlUnion_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        NameValueCollection nvc = Filter.CurrentCrewChangeList;

        if (nvc != null)
            ds = PhoenixCrewReportsCrewChange.GetJSUVessels(int.Parse(nvc.Get("ddlUnion").ToString()));
        else
            ds = PhoenixCrewReportsCrewChange.GetJSUVessels(int.Parse(ddlUnion.SelectedValue));

        ddlVessel.DataTextField = "FLDVESSELNAME";
        ddlVessel.DataValueField = "FLDVESSELID";
        ddlVessel.DataSource = ds;
        ddlVessel.DataBind();
        ddlVessel.Items.Insert(0, new RadComboBoxItem("--Select--", "0"));
    }
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["SHOWREPORT"] = null;
                ddlVessel.SelectedIndex = 0;
                ddlUnion.SelectedIndex = 0;
                ucDate.Text = "";
                ucDate1.Text = DateTime.Now.ToShortDateString();

                Filter.CurrentCrewChangeList = null;

                ShowReport();
                gvCrew.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter(ddlVessel.SelectedValue, ucDate.Text, ucDate1.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    NameValueCollection criteria = new NameValueCollection();
                    criteria.Add("ddlvessel", ddlVessel.SelectedValue);
                    criteria.Add("ucDate", ucDate.Text);
                    criteria.Add("ucDate1", ucDate1.Text);
                    criteria.Add("Format", rblFormats.SelectedIndex.ToString());
                    criteria.Add("ddlUnion", ddlUnion.SelectedValue);
                    Filter.CurrentCrewChangeList = criteria;
                    ViewState["PAGENUMBER"] = 1;
                    ShowReport();
                    gvCrew.Rebind();
                }
            }
            if (CommandName.ToUpper().Equals("MAIL"))
            {
                if (!IsValidFilter(ddlVessel.SelectedValue, ucDate.Text, ucDate1.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    Response.Redirect("CrewEmail.aspx?itf=crewchange&vesselid=" + ddlVessel.SelectedValue + "&fromdate=" + ucDate.Text + "&todate=" + ucDate1.Text);
                }
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

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDOFFSIGNERNAME", "FLDSIGNOFFDATE", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDNATIONALITY", "FLDDATEOFBIRTH", "FLDPASSPORTNO", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDSIGNONDATE", "FLDPOJ" };
        string[] alCaptions = { "Off Signer Name", "Sign Off Date", "Name", "Rank", "Nationality", "Birth Date", "Passport No.", "POI", "DOI", "DOE", "Sign On", "POJ" };
        string[] FilterCaptions = { "Vessel" };
        string[] FilterColumns = { "FLDVESSELNAME" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewReportsCrewChange.CrewChangeList(
                           General.GetNullableInteger(ddlVessel.SelectedValue),
                           General.GetNullableDateTime(ucDate.Text),
                           General.GetNullableDateTime(ucDate1.Text),
                           sortexpression, sortdirection,
                           Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                           iRowCount,
                           ref iRowCount,
                           ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=ChangedCrewList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center></center></h5></td></tr>");
        General.ShowFilterCriteriaInExcel(ds, FilterCaptions, FilterColumns);
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
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
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    private void ShowReport()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDNATIONALITY", "FLDDATEOFBIRTH", "FLDPASSPORTNO", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDSIGNONDATE", "FLDPOJ" };
        string[] alCaptions = { "Name", "Rank", "Nationality", "Birth Date", "Passport No.", "POI", "DOI", "DOE", "Sign On", "POJ" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCrewReportsCrewChange.CrewChangeList(
                    General.GetNullableInteger(ddlVessel.SelectedValue),
                    General.GetNullableDateTime(ucDate.Text),
                    General.GetNullableDateTime(ucDate1.Text),
                    sortexpression, sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvCrew.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);


        General.SetPrintOptions("gvCrew", "Crew Change List", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            lbr.Attributes.Add("onclick", "javascript:parent.Openpopup('Crew','','../Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");

            RadLabel empid1 = (RadLabel)e.Item.FindControl("lblSignOffEmpNo");
            LinkButton lbr1 = (LinkButton)e.Item.FindControl("lnkOffSigner");
            lbr1.Attributes.Add("onclick", "javascript:parent.Openpopup('Crew','','../Crew/CrewPersonalGeneral.aspx?empid=" + empid1.Text + "'); return false;");

        }

    }
    protected void gvCrew_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

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

    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;

        ShowReport();
    }
    public bool IsValidFilter(string vessel, string fromdate, string todate)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";

        if (vessel.Equals("") || vessel.Equals("Dummy") || vessel.Equals("0"))
        {
            ucError.ErrorMessage = "Select Vessel";
        }
        if (string.IsNullOrEmpty(fromdate))
        {
            ucError.ErrorMessage = "From Date is required";
        }
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Date should be earlier than current date";
        }

        if (string.IsNullOrEmpty(todate))
        {
            ucError.ErrorMessage = "To Date is required";
        }
        else if (!string.IsNullOrEmpty(fromdate)
            && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "To Date should be later than 'From Date'";
        }
        return (!ucError.IsError);
    }
    public void rblFormats_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblFormats.SelectedIndex == 0)
        {
            Response.Redirect("../Crew/CrewReportsCrewChange.aspx");
        }

        else if (rblFormats.SelectedIndex == 1)
        {
            Response.Redirect("../Crew/CrewReportsCrewChangeFormat2.aspx");
        }

        else
        {
            Response.Redirect("../Crew/CrewReportsCrewChangeFormat3.aspx");
        }
    }
}
