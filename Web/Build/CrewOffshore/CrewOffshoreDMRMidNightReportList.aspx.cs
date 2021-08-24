using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewOffshore;

using Telerik.Web.UI;



public partial class CrewOffshoreDMRMidNightReportList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarreporttap = new PhoenixToolbar();
            toolbarreporttap.AddButton("List", "REPORTLIST");
            MenuReportTab.AccessRights = this.ViewState;
            MenuReportTab.MenuList = toolbarreporttap.Show();
            MenuReportTab.SelectedMenuIndex = 0;

            PhoenixToolbar toolbarreportlist = new PhoenixToolbar();
            toolbarreportlist.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreDMRMidNightReportList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbarreportlist.AddFontAwesomeButton("javascript:CallPrint('gvReport')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarreportlist.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreDMRMidNightReportList.aspx", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
            {
                Session["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                toolbarreportlist.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreDMRMidNightReportList.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            }

            MenuReportList.AccessRights = this.ViewState;
            MenuReportList.MenuList = toolbarreportlist.Show();

            if (!IsPostBack)
            {


                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }
                else
                {
                    if (Request.QueryString["VesselId"] != null && Request.QueryString["VesselId"].ToString() != string.Empty)
                    {
                        ucVessel.SelectedVessel = Request.QueryString["VesselId"].ToString();
                        ViewState["VESSELID"] = Request.QueryString["VesselId"].ToString();
                    }
                }
                if (Request.QueryString["PageNumber"] != null && Request.QueryString["PageNumber"].ToString() != string.Empty)
                    ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["PageNumber"].ToString());
                else
                    ViewState["PAGENUMBER"] = 1;

                gvReport.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ucVessel.bind();


            }
            if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
            {
                lblFromDate.Visible = false;
                lblToDate.Visible = false;
                ucFromDate.Visible = false;
                ucToDate.Visible = false;
            }
            else
            {
                lblFromDate.Visible = true;
                lblToDate.Visible = true;
                ucFromDate.Visible = true;
                ucToDate.Visible = true;
            }

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                gvReport.Columns[12].Visible = false;
            }

            // BindData();
            //  SetPageNavigator();           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvReport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {

            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvReport_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.ExportToExcelCommandName)
        {
            gvReport.ExportSettings.Excel.Format = GridExcelExportFormat.Biff;
            gvReport.ExportSettings.IgnorePaging = true;
            gvReport.ExportSettings.ExportOnlyData = true;
            gvReport.ExportSettings.OpenInNewWindow = true;
        }
        if (e.CommandName == RadGrid.RebindGridCommandName)
        {
            gvReport.EditIndexes.Clear();
            gvReport.SelectedIndexes.Clear();
            gvReport.Rebind();
        }
        if (e.CommandName.ToUpper() == "EDIT")
        {
            GridDataItem item = (GridDataItem)e.Item;
            RadLabel lblVoyageId = (RadLabel)item.FindControl("lblVoyageId");
            Filter.CurrentVPRSVoyageSelection = lblVoyageId.Text;

            string ReportID = ((RadLabel)item.FindControl("lblReportID")).Text;
            string VesselID = ((RadLabel)item.FindControl("lblVesselID")).Text;
            string ReportDate = ((LinkButton)item.FindControl("lnkReportID")).Text;
            Session["MIDNIGHTREPORTID"] = ReportID;
            Session["VESSELID"] = VesselID;
            Session["MIDNIGHTREPORTDATE"] = ReportDate;
            Response.Redirect("CrewOffshoreDMRMidNightReport.aspx?ReportID=" + ReportID + "&VesselId=" + ucVessel.SelectedVessel.ToString() + "&PageNumber=" + ViewState["PAGENUMBER"].ToString(), false);

        }
        if (e.CommandName == "InitInsert")
        {
            Response.Redirect("CrewOffshoreDMRMidNightReport.aspx", false);

        }
        if (e.CommandName == "ExportToExcel")
        {
            gvReport.ExportSettings.ExportOnlyData = true;
            gvReport.ExportSettings.IgnorePaging = true;
            //gvReport.ExportSettings.OpenInNewWindow = true;
            //gvReport.ExportSettings.FileName = @"C:\jasInv.xlxs";
            //gvReport.MasterTableView.ExportToExcel();
            ShowExcel();
        }
        if (e.CommandName == "ExportToCsv")
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script language='javascript'>");
            sb.Append(@"example();");
            sb.Append(@"</script>");
            //  string str=phoenix
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "CallPrint('gvReport')", sb.ToString(), false);
        }
    }

    protected void gvReport_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                // GridDataItem item = (GridDataItem)e.Item;
                RadImageButton db = (RadImageButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    string FLDCANCELFLAG = DataBinder.Eval(e.Item.DataItem, "FLDCANCELFLAG").ToString();
                    db.Visible = FLDCANCELFLAG.ToString() == "1" ? true : false;
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                {
                    gvReport.Columns[1].Visible = false;
                }

                RadLabel lblPlannedActvity1Name = (RadLabel)e.Item.FindControl("lblPlannedActvity1Name");
                UserControlToolTip ucPlannedActvity1Name = (UserControlToolTip)e.Item.FindControl("ucPlannedActvity1Name");
                if (lblPlannedActvity1Name != null && ucPlannedActvity1Name != null)
                {
                    lblPlannedActvity1Name.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucPlannedActvity1Name.ToolTip + "', 'visible');");
                    lblPlannedActvity1Name.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucPlannedActvity1Name.ToolTip + "', 'hidden');");
                }

                RadLabel lblPlannedActvity2Name = (RadLabel)e.Item.FindControl("lblPlannedActvity2Name");
                UserControlToolTip ucPlannedActvity2Name = (UserControlToolTip)e.Item.FindControl("ucPlannedActvity2Name");
                if (lblPlannedActvity2Name != null && ucPlannedActvity2Name != null)
                {
                    lblPlannedActvity2Name.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucPlannedActvity2Name.ToolTip + "', 'visible');");
                    lblPlannedActvity2Name.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucPlannedActvity2Name.ToolTip + "', 'hidden');");
                }

                RadLabel lblPlannedActvity3Name = (RadLabel)e.Item.FindControl("lblPlannedActvity3Name");
                UserControlToolTip ucPlannedActvity3Name = (UserControlToolTip)e.Item.FindControl("ucPlannedActvity3Name");
                if (lblPlannedActvity3Name != null && ucPlannedActvity3Name != null)
                {
                    lblPlannedActvity3Name.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucPlannedActvity3Name.ToolTip + "', 'visible');");
                    lblPlannedActvity3Name.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucPlannedActvity3Name.ToolTip + "', 'hidden');");
                }
                RadImageAndTextTile imgFlag = (RadImageAndTextTile)e.Item.FindControl("imgFlag");
                if (imgFlag != null)
                {
                    string FLDCONFIRMEDYN = DataBinder.Eval(e.Item.DataItem, "FLDCONFIRMEDYN").ToString();
                    if (FLDCONFIRMEDYN.ToString() == "0")
                    {
                        imgFlag.ImageUrl = Session["images"] + "/de-select.png";
                        imgFlag.ToolTip = "Report not sent to Office";
                    }
                    if (FLDCONFIRMEDYN.ToString() == "1")
                    {
                        imgFlag.ImageUrl = Session["images"] + "/select.png";
                        imgFlag.ToolTip = "Report sent to Office";
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


    protected void ReportList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            BindData();
            gvReport.Rebind();
        }
        if (CommandName.ToUpper().Equals("ADD"))
        {
            Session["MIDNIGHTREPORTID"] = null;
            Response.Redirect("CrewOffshoreDMRMidNightReport.aspx", false);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns;
        string[] alCaptions;

        alColumns = new string[11] { "FLDREPORTDATE", "FLDVESSELNAME", "FLDMASTER", "FLDCENAME", "FLDVOYAGENO", "FLDVESSELSTATUS", "FLDSEAPORTNAME", "FLDETADATE", "FLDETDDATE", "FLDNEXTPLANNEDACTIVITY1", "FLDNEXTPLANNEDACTIVITY2" };
        alCaptions = new string[11] { "Report Date", "Vessel Name", "Master", "CE", "Charter Id", "Status", "Port / Location", "ETA", "ETD", "Look Ahead (Day 1)", "Look Ahead (Day 2)" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = new DataSet();
        ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportSearch(
            PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 ? PhoenixSecurityContext.CurrentSecurityContext.VesselID : General.GetNullableInteger(ucVessel.SelectedVessel),
            General.GetNullableDateTime(ucFromDate.Text),
            General.GetNullableDateTime(ucToDate.Text),
            sortexpression,
            sortdirection,
            gvReport.CurrentPageIndex + 1,
            gvReport.PageSize,
            ref iRowCount,
            ref iTotalPageCount);



        Response.AddHeader("Content-Disposition", "attachment; filename=\"MidNightReport.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>MidNight Report</h3></td>");
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();


        ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportSearch(
                     PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 ? PhoenixSecurityContext.CurrentSecurityContext.VesselID : General.GetNullableInteger(ucVessel.SelectedVessel),
                     General.GetNullableDateTime(ucFromDate.Text),
                     General.GetNullableDateTime(ucToDate.Text),
                     sortexpression,
                     sortdirection,
                     gvReport.CurrentPageIndex + 1,
                     gvReport.PageSize,
                     ref iRowCount,
                     ref iTotalPageCount);

        string[] alColumns = new string[11] { "FLDREPORTDATE", "FLDVESSELNAME", "FLDMASTER", "FLDCENAME", "FLDVOYAGENO", "FLDVESSELSTATUS", "FLDSEAPORTNAME", "FLDETADATE", "FLDETDDATE", "FLDNEXTPLANNEDACTIVITY1", "FLDNEXTPLANNEDACTIVITY2" };
        string[] alCaptions = new string[11] { "Report Date", "Vessel Name", "Master", "CE", "Charter Id", "Status", "Port / Location", "ETA", "ETD", "Look Ahead (Day 1)", "Look Ahead (Day 2)" };

        General.SetPrintOptions("gvReport", "Mid Night Report", alCaptions, alColumns, ds);

        gvReport.DataSource = ds;
        gvReport.VirtualItemCount = iRowCount;

        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            gvReport.Columns[0].Visible = false;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    //private void SetRowSelection()
    //{
    //    gvReport.SelectedIndex = -1;

    //    if (Session["MIDNIGHTREPORTID"] != null)
    //    {
    //        for (int i = 0; i < gvReport.Rows.Count; i++)
    //        {
    //            if (gvReport.DataKeys[i].Value.ToString().Equals(Session["MIDNIGHTREPORTID"].ToString()))
    //            {
    //                gvReport.SelectedIndex = i;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        gvReport.SelectedIndex = 0;
    //        Session["NOONREPORTID"] = gvReport.DataKeys[0].Value.ToString();
    //    }
    //}

    //protected void gvReport_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        DataRowView drv = (DataRowView)e.Row.DataItem;
    //        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
    //        {
    //            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");                
    //            if (db != null)
    //            {
    //                db.Visible = drv["FLDCANCELFLAG"].ToString() == "1" ? true : false;
    //                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //            }
    //        }      

    //        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
    //        {
    //            gvReport.Columns[1].Visible = false;
    //        }


    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            Label lblPlannedActvity1Name = (Label)e.Row.FindControl("lblPlannedActvity1Name");
    //            UserControlToolTip ucPlannedActvity1Name = (UserControlToolTip)e.Row.FindControl("ucPlannedActvity1Name");
    //            if (lblPlannedActvity1Name != null && ucPlannedActvity1Name != null)
    //            {
    //                lblPlannedActvity1Name.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucPlannedActvity1Name.ToolTip + "', 'visible');");
    //                lblPlannedActvity1Name.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucPlannedActvity1Name.ToolTip + "', 'hidden');");
    //            } 

    //            Label lblPlannedActvity2Name = (Label)e.Row.FindControl("lblPlannedActvity2Name");
    //            UserControlToolTip ucPlannedActvity2Name = (UserControlToolTip)e.Row.FindControl("ucPlannedActvity2Name");
    //            if (lblPlannedActvity2Name != null && ucPlannedActvity2Name != null)
    //            {
    //                lblPlannedActvity2Name.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucPlannedActvity2Name.ToolTip + "', 'visible');");
    //                lblPlannedActvity2Name.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucPlannedActvity2Name.ToolTip + "', 'hidden');");
    //            }

    //            Label lblPlannedActvity3Name = (Label)e.Row.FindControl("lblPlannedActvity3Name");
    //            UserControlToolTip ucPlannedActvity3Name = (UserControlToolTip)e.Row.FindControl("ucPlannedActvity3Name");
    //            if (lblPlannedActvity3Name != null && ucPlannedActvity3Name != null)
    //            {
    //                lblPlannedActvity3Name.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucPlannedActvity3Name.ToolTip + "', 'visible');");
    //                lblPlannedActvity3Name.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucPlannedActvity3Name.ToolTip + "', 'hidden');");
    //            }
    //            Image imgFlag = (Image)e.Row.FindControl("imgFlag");
    //            if (imgFlag != null)
    //            {
    //                if(drv["FLDCONFIRMEDYN"].ToString() == "0")
    //                {
    //                    imgFlag.ImageUrl = Session["images"] + "/de-select.png";
    //                    imgFlag.ToolTip = "Report not sent to Office";
    //                }
    //                if (drv["FLDCONFIRMEDYN"].ToString() == "1")
    //                {
    //                    imgFlag.ImageUrl = Session["images"] + "/select.png";
    //                    imgFlag.ToolTip = "Report sent to Office";
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}



    //protected void gvReport_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;

    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //        if (e.CommandName.ToUpper().Equals("UPDATE"))
    //        {
    //            _gridView.EditIndex = -1;
    //            // BindData();
    //        }
    //        if (e.CommandName.ToUpper() == "EDIT")
    //        {
    //            string ReportID = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblReportID")).Text;
    //            string VesselID = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselID")).Text;
    //            string ReportDate = ((LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkReportID")).Text;
    //            Session["MIDNIGHTREPORTID"] = ReportID;
    //            Session["VESSELID"] = VesselID;
    //            Session["MIDNIGHTREPORTDATE"] = ReportDate;
    //            Response.Redirect("CrewOffshoreDMRMidNightReport.aspx?ReportID=" + ReportID + "&VesselId=" + ucVessel.SelectedVessel.ToString() + "&PageNumber=" + ViewState["PAGENUMBER"].ToString(), false);
    //        }
    //        if (e.CommandName.ToUpper().Equals("DELETE"))
    //        {
    //            string ReportID = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblReportID")).Text;
    //            PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ReportID));
    //            // BindData();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

}