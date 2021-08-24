using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;
using System.Web;

public partial class Crew_CrewReportOwnerwiseSearch : PhoenixBasePage
{
 
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           
         
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportOwnerwiseSearch.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportOwnerwiseSearch.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            PhoenixToolbar toolbar2 = new PhoenixToolbar();
            toolbar2.AddFontAwesomeButton("../Crew/CrewReportOwnerwiseSearch.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel1");
            toolbar2.AddFontAwesomeButton("javascript:CallPrint('gvCrew1')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT1");
            toolbar2.AddFontAwesomeButton("../Crew/CrewReportOwnerwiseSearch.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuShowExcel1.AccessRights = this.ViewState;
            MenuShowExcel1.MenuList = toolbar2.Show();

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar3 = new PhoenixToolbar();
            toolbar3.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);

            ShowReports.AccessRights = this.ViewState;
            ShowReports.MenuList = toolbar3.Show();
            
            

            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Hand On Leave", "HANDONLEAVE", ToolBarDirection.Right);
                toolbar.AddButton("Planner", "PLANNER", ToolBarDirection.Right);

                MenuReportsFilter.AccessRights = this.ViewState;
                MenuReportsFilter.MenuList = toolbar.Show();
                MenuReportsFilter.SelectedMenuIndex = 1;

                Default.Visible = true;
                HandOnLeave.Visible = false;
                

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBER1"] = 1;
                ViewState["SORTEXPRESSION1"] = null;
                ViewState["SORTDIRECTION1"] = null;
                
                gvCrew1.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
                ViewState["PAGENUMBER"] = 1;
                ViewState["PAGENUMBER1"] = 1;
                ucPrincipal.SelectedAddress = "";
                ucVesselType.SelectedVesselTypeValue = "";
                ucRank.SelectedRankValue = "";
                ucBatch.SelectedBatch = "";
                ucDateFrom.Text = "";
                ucDateTo.Text = DateTime.Now.ToShortDateString();
                ShowReport();
                Rebind();
             }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvCrew1.SelectedIndexes.Clear();
        gvCrew1.EditIndexes.Clear();
        gvCrew1.DataSource = null;
        gvCrew1.Rebind();
        gvCrew.SelectedIndexes.Clear();
        gvCrew.EditIndexes.Clear();
        gvCrew.DataSource = null;
        gvCrew.Rebind();       
    }
    protected void CrewShowExcel1_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL1"))
            {
                ShowExcel1();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["SHOWREPORT"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["PAGENUMBER1"] = 1; 
                ucPrincipal.SelectedAddress = "";
                ucVesselType.SelectedVesselTypeValue = "";
                ucRank.SelectedRankValue = "";
                ucBatch.SelectedBatch = "";
                ucDateFrom.Text = "";
                ucDateTo.Text = DateTime.Now.ToShortDateString();
                lstPool.SelectedPool = "";
                lstZone.SelectedZoneValue = "";
                ShowReport1();
                Rebind();
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

                

            if (CommandName.ToUpper().Equals("PLANNER"))
            {
                Default.Visible = true;
                HandOnLeave.Visible = false;

                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Hand On Leave", "HANDONLEAVE", ToolBarDirection.Right);
                toolbar.AddButton("Planner", "PLANNER", ToolBarDirection.Right);

                MenuReportsFilter.AccessRights = this.ViewState;
                MenuReportsFilter.MenuList = toolbar.Show();
                MenuReportsFilter.SelectedMenuIndex = 1;
                           
                gvCrew.CurrentPageIndex = 0;
                gvCrew.Rebind();
                ShowReport();
          
                
            }
            else if (CommandName.ToUpper().Equals("HANDONLEAVE"))
            {
                
                Default.Visible = false;
                HandOnLeave.Visible = true;

                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Hand On Leave", "HANDONLEAVE", ToolBarDirection.Right);
                toolbar.AddButton("Planner", "PLANNER", ToolBarDirection.Right);

                MenuReportsFilter.AccessRights = this.ViewState;
                MenuReportsFilter.MenuList = toolbar.Show();
                MenuReportsFilter.SelectedMenuIndex = 0;

                int page = gvCrew1.CurrentPageIndex;
              
                gvCrew1.CurrentPageIndex = 0;

                gvCrew1.Rebind();
                ShowReport1();  
            }
                      
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuShowReport_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SHOWREPORT"))
        {
            

            if (!IsValidFilter(ucPrincipal.SelectedAddress.ToString()))
            {
                ucError.Visible = true;
                return;
            }
            else
            {
                
                ViewState["PAGENUMBER"] = 1;

                Rebind();
                
            }
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDBATCH", "FLDVESSELNAME", "FLDRELIEFDUEDATE", "FLDRELIEVER", "FLDRELIEVERREMARKS" };
        string[] alCaptions = { "Name", "Rank", "Batch", "Vessel", "Relief Due Date", "Reliever", "Remarks" };
        string[] FilterColumns = { "FLDSELECTEDOWNER", "FLDSELECTEDVESSELTYPE", "FLDSELECTEDBATCH", "FLDSELECTEDRANK" };
        string[] FilterCaptions = { "Prinicpal", "Vessel Type", "Batch", "Rank" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewReportOwnerwiseSearch.CrewOwnerwiseSearch(
                (ucPrincipal.SelectedAddress.ToString()) == "Dummy" ? null : General.GetNullableInteger(ucPrincipal.SelectedAddress.ToString()),
                1,
                (ucRank.selectedlist.ToString()) == "Dummy" ? null : General.GetNullableString(ucRank.selectedlist.ToString()),
                (ucBatch.SelectedBatch.ToString()) == "Dummy" ? null : General.GetNullableInteger(ucBatch.SelectedBatch),
                (ucVesselType.SelectedVesseltype.ToString()) == "Dummy" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                General.GetNullableDateTime(ucDateFrom.Text), General.GetNullableDateTime(ucDateTo.Text),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount,
                lstPool.SelectedPool,
                lstZone.selectedlist);


        Response.AddHeader("Content-Disposition", "attachment; filename=CrewPrincipalwisePlanner.xls");
        Response.ContentType = "application/msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Crew Principalwise Planner</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        General.ShowFilterCriteriaInExcel(ds, FilterCaptions, FilterColumns);
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
    protected void ShowExcel1()
    {
        int iRowCount1 = 0;
        int iTotalPageCount1 = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDFIRSTNAME", "FLDLASTNAME", "FLDRANKCODE", "FLDBATCH", "FLDVESSELNAME", "FLDSIGNOFFDATE", "FLDZONE", "FLDREMARKS" };
        string[] alCaptions = { "First Name", "Surname", "Rank", "Batch", "Last Vessel", "Signed Off", "Zone", "Remarks" };
        string[] FilterColumns = { "FLDSELECTEDOWNER", "FLDSELECTEDVESSELTYPE", "FLDSELECTEDBATCH", "FLDSELECTEDRANK" };
        string[] FilterCaptions = { "Prinicpal", "Vessel Type", "Batch", "Rank" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION1"] == null) ? null : (ViewState["SORTEXPRESSION1"].ToString());
        if (ViewState["SORTDIRECTION1"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION1"].ToString());
        if (ViewState["ROWCOUNT1"] == null || Int32.Parse(ViewState["ROWCOUNT1"].ToString()) == 0)
            iRowCount1 = 10;
        else
            iRowCount1 = Int32.Parse(ViewState["ROWCOUNT1"].ToString());

        ds = PhoenixCrewReportOwnerwiseSearch.CrewOwnerwiseSearch(
                (ucPrincipal.SelectedAddress.ToString()) == "Dummy" ? null : General.GetNullableInteger(ucPrincipal.SelectedAddress.ToString()),
                2,
                (ucRank.selectedlist.ToString()) == "Dummy" ? null : General.GetNullableString(ucRank.selectedlist.ToString()),
                (ucBatch.SelectedBatch.ToString()) == "Dummy" ? null : General.GetNullableInteger(ucBatch.SelectedBatch),
                (ucVesselType.SelectedVesseltype.ToString()) == "Dummy" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                General.GetNullableDateTime(ucDateFrom.Text), General.GetNullableDateTime(ucDateTo.Text),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER1"].ToString()),
                iRowCount1,
                ref iRowCount1,
                ref iTotalPageCount1,
                lstZone.selectedlist,
                lstPool.SelectedPool);



        Response.AddHeader("Content-Disposition", "attachment; filename=CrewPrincipalwisePlanner-ExHandOnLeave.xls");
        Response.ContentType = "application/msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Crew Ownerwise Planner-Ex Hand On Leave</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        General.ShowFilterCriteriaInExcel(ds, FilterCaptions, FilterColumns);
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

        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDBATCH", "FLDVESSELNAME", "FLDRELIEFDUEDATE", "FLDRELIEVER", "FLDRELIEVERREMARKS" };
        string[] alCaptions = { "Name", "Rank", "Batch", "Vessel Name", "Relief Due Date", "Reliever", "Remarks" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        ds = PhoenixCrewReportOwnerwiseSearch.CrewOwnerwiseSearch(
            (ucPrincipal.SelectedAddress.ToString()) == "Dummy" ? null : General.GetNullableInteger(ucPrincipal.SelectedAddress.ToString()),
            1,
            (ucRank.selectedlist.ToString()) == "," ? null : General.GetNullableString(ucRank.selectedlist.ToString()),
            (ucBatch.SelectedBatch.ToString()) == "Dummy" ? null : General.GetNullableInteger(ucBatch.SelectedBatch),
            (ucVesselType.SelectedVesseltype.ToString()) == "Dummy" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
            General.GetNullableDateTime(ucDateFrom.Text), General.GetNullableDateTime(ucDateTo.Text),
            sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvCrew.PageSize,
            ref iRowCount,
            ref iTotalPageCount,
            lstZone.selectedlist,
            lstPool.SelectedPool);

        General.SetPrintOptions("gvCrew", "Crew Ownerwise Planner", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
     
    }
    private void ShowReport1()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDRANKCODE", "FLDBATCH", "FLDLASTNAME", "FLDFIRSTNAME", "FLDVESSELNAME", "FLDSIGNOFFDATE", "FLDZONE", "FLDREMARKS" };
        string[] alCaptions = { "Rank", "Batch", "Surname", "First Name", "Last Vessel Name", "Signed Off", "Zone", "Remarks" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        ds = PhoenixCrewReportOwnerwiseSearch.CrewOwnerwiseSearch(
            (ucPrincipal.SelectedAddress.ToString()) == "Dummy" ? null : General.GetNullableInteger(ucPrincipal.SelectedAddress.ToString()),
            2,
            (ucRank.selectedlist.ToString()) == "," ? null : General.GetNullableString(ucRank.selectedlist.ToString()),
            (ucBatch.SelectedBatch.ToString()) == "Dummy" ? null : General.GetNullableInteger(ucBatch.SelectedBatch),
            (ucVesselType.SelectedVesseltype.ToString()) == "Dummy" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
            General.GetNullableDateTime(ucDateFrom.Text), General.GetNullableDateTime(ucDateTo.Text),
            sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER1"].ToString()),
            gvCrew1.PageSize,
            ref iRowCount,
            ref iTotalPageCount,
            lstPool.SelectedPool,
            lstZone.selectedlist
            );


        General.SetPrintOptions("gvCrew1", "Crew Ownerwise Planner-Ex Hand On Leave", alCaptions, alColumns, ds);

        gvCrew1.DataSource = ds;
        gvCrew1.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT1"] = iRowCount;
        ViewState["TOTALPAGECOUNT1"] = iTotalPageCount;
       
    }
    
    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

                RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
                LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");

                RadLabel relieverid = (RadLabel)e.Item.FindControl("lblOnsignerID");
                RadLabel reliever = (RadLabel)e.Item.FindControl("lblRelieverName");
                LinkButton relivername = (LinkButton)e.Item.FindControl("lnkRelieverName");
                if (relieverid.Text.Equals(""))
                {
                    reliever.Visible = true;
                    relivername.Visible = false;
                }
                relivername.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + relieverid.Text + "'); return false;");
            }
        
    }
    protected void gvCrew1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

                RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
                LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");

            }
        
    }
    public bool IsValidFilter(string principal)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (principal.Equals("") || principal.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Principal is Required";
        }

        return (!ucError.IsError);

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

    protected void gvCrew1_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvCrew1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER1"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }


    protected void gvCrew1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER1"] = ViewState["PAGENUMBER1"] != null ? ViewState["PAGENUMBER1"] : gvCrew1.CurrentPageIndex + 1;

        ShowReport1();
    }
}
