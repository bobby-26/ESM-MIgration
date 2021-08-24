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
using System.Collections.Generic;
using System.IO;
using System.Text;
using Telerik.Web.UI;
public partial class CrewOffshoreDMRMonthlyReportList : PhoenixBasePage
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
            toolbarreportlist.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreDMRMonthlyReportList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbarreportlist.AddFontAwesomeButton("javascript:CallPrint('gvReport')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarreportlist.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreDMRMonthlyReportList.aspx", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                toolbarreportlist.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreDMRMonthlyReportList.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

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
                if(Request.QueryString["vesselid"]!=null)
                {
                    ucVessel.SelectedVessel = Request.QueryString["vesselid"].ToString();                    
                    ucVessel.Enabled = false;
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvReport.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                BindData();
                //SetPageNavigator();
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
        }
        if (CommandName.ToUpper().Equals("ADD"))
        {
            Session["MONTHLYREPORTID"] = null;
            Response.Redirect("CrewOffshoreDMRMonthlyReport.aspx", false);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns;
        string[] alCaptions;

        alColumns = new string[2] { "FLDVESSELNAME", "FLDDATE" };
        alCaptions = new string[2] { "Vessel Name", "Report Date" };

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
        ds = PhoenixCrewOffshoreDMRMonthlyReport.MonthlyReportSearch(
            PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 ? PhoenixSecurityContext.CurrentSecurityContext.VesselID : General.GetNullableInteger(ucVessel.SelectedVessel),
            sortexpression,
            sortdirection,
            (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"MonthlyReport.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns;
        string[] alCaptions;

        alColumns = new string[2] { "FLDVESSELNAME", "FLDDATE" };
        alCaptions = new string[2] { "Vessel Name", "Report Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        DataSet ds;
        if (Request.QueryString["vesselid"] != null)
        {
            ds = PhoenixCrewOffshoreDMRMonthlyReport.MonthlyReportSearch(
                      General.GetNullableInteger(Request.QueryString["vesselid"].ToString()),
                      sortexpression,
                      sortdirection,
                      gvReport.CurrentPageIndex + 1,
                     gvReport.PageSize,
                      ref iRowCount,
                      ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixCrewOffshoreDMRMonthlyReport.MonthlyReportSearch(
           PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 ? PhoenixSecurityContext.CurrentSecurityContext.VesselID : General.GetNullableInteger(ucVessel.SelectedVessel),
           sortexpression,
           sortdirection,
           gvReport.CurrentPageIndex + 1,
          gvReport.PageSize,
           ref iRowCount,
           ref iTotalPageCount);
        }

        General.SetPrintOptions("gvReport", "Monthly Report", alCaptions, alColumns, ds);

        gvReport.DataSource = ds;
        //gvReport.DataBind();
        gvReport.VirtualItemCount = iRowCount;


        SetRowSelection();


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void SetRowSelection()
    {
        // gvReport.sele.SelectedIndex = -1;



        if (Session["MONTHLYREPORTID"] != null)
        {
            foreach (GridDataItem item in gvReport.MasterTableView.Items)
            {
                if (item.GetDataKeyValue("FLDMONTHLYREPORTID").ToString().Equals(Session["MONTHLYREPORTID"].ToString()))
                {
                    gvReport.MasterTableView.ClearSelectedItems();

                }
                else
                {
                    //gvReport.SelectedIndex = 0;
                    Session["NOONREPORTID"] = item.GetDataKeyValue("FLDMONTHLYREPORTID").ToString();
                }
            }

        }

    }

    //protected void gvReport_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
    //        {
    //            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
    //            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //        }

    //        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
    //        {
    //            gvReport.Columns[0].Visible = false;
    //        }
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            string ReportID = ((RadLabel)e.Row.FindControl("lblReportID")).Text;                
    //            ImageButton vr = (ImageButton)e.Row.FindControl("cmdDatewiseReport");
    //            if (vr != null)
    //            {
    //                vr.Visible = SessionUtil.CanAccess(this.ViewState, vr.CommandName);                    
    //                vr.Attributes.Add("onclick", "Openpopup('DatewiseDMRMonthlyReport', '', '../Reports/ReportsView.aspx?applicationcode=11&reportcode=DATEWISEREPORT&MonthlyReportID=" + ReportID +"&showmenu=0&showword=no&showexcel=no');return false;");
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    protected void gvReport_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

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
    //            BindData();
    //        }
    //        if (e.CommandName.ToUpper() == "EDIT")
    //        {
    //            string ReportID = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblReportID")).Text;
    //            Session["MONTHLYREPORTID"] = ReportID;
    //            Response.Redirect("CrewOffshoreDMRMonthlyReport.aspx?ReportID=" + ReportID, false);
    //        }
    //        if (e.CommandName.ToUpper().Equals("DELETE"))
    //        {
    //            string ReportID = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblReportID")).Text;
    //            PhoenixCrewOffshoreDMRMonthlyReport.MonthlyReportDelete(new Guid(ReportID));
    //            BindData();
    //        }
    //        if (e.CommandName.ToUpper().Equals("DATEWISEREPORTEXCEL"))
    //        {
    //            string VesselName, Month;
    //            int Year;
    //            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)                
    //                VesselName = PhoenixSecurityContext.CurrentSecurityContext.VesselName;                
    //            else
    //                VesselName = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblVesselName")).Text;
    //            DateTime Date = DateTime.Parse(((LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkReportID")).Text);

    //            Month = Date.ToString("MMMM");
    //            Year  = Date.Year;

    //            string ReportID = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblReportID")).Text;
    //            DataTable dt = PhoenixCrewOffshoreDMRMonthlyReport.DatewiseDMRMonthlyReport(new Guid(ReportID));
    //            DataTable dtPvt = PivotTable(dt);
    //            string filename = HttpContext.Current.Server.MapPath("../Attachments/CrewOffshore/DMRMonthlyReport-" + Month +'-'+ Year +'-'+ VesselName + ".xls");
    //            ExportToExcel(dtPvt, filename);
    //            String scriptpopup = String.Format("window.open('" + Session["sitepath"] + "/Attachments/CrewOffshore/DMRMonthlyReport-" + Month + '-' + Year + '-' + VesselName + ".xls');");                
    //            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);                
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void gvReport_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }

    protected void gvReport_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        //SetPageNavigator();
    }

    protected void gvReport_RowCreated(object sender, GridViewRowEventArgs e)
    {
    }

    //protected void gvReport_Sorting(object sender, GridViewSortEventArgs se)
    //{
    //    gvReport.EditIndex = -1;
    //    gvReport.SelectedIndex = -1;
    //    ViewState["SORTEXPRESSION"] = se.SortExpression;

    //    if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
    //        ViewState["SORTDIRECTION"] = 1;
    //    else
    //        ViewState["SORTDIRECTION"] = 0;

    //    BindData();
    //}

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    int result;
    //    gvReport.SelectedIndex = -1;
    //    gvReport.EditIndex = -1;
    //    if (Int32.TryParse(txtnopage.Text, out result))
    //    {
    //        ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //        if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


    //        if (0 >= Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = 1;

    //        if ((int)ViewState["PAGENUMBER"] == 0)
    //            ViewState["PAGENUMBER"] = 1;

    //        txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //    }
    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    gvReport.SelectedIndex = -1;
    //    gvReport.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //    else
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //    BindData();
    //    SetPageNavigator();
    //}

    //private void SetPageNavigator()
    //{
    //    cmdPrevious.Enabled = IsPreviousEnabled();
    //    cmdNext.Enabled = IsNextEnabled();
    //    lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //    lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //}

    //private Boolean IsPreviousEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //private Boolean IsNextEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    //protected void cmdSearch_Click(object sender, EventArgs e)
    //{
    //    gvReport.SelectedIndex = -1;
    //    gvReport.EditIndex = -1;
    //    ViewState["PAGENUMBER"] = 1;
    //    BindData();
    //    SetPageNavigator();
    //}

    //private void ShowNoRecordsFound(DataTable dt, GridView gv)
    //{
    //    dt.Rows.Add(dt.NewRow());
    //    gv.DataSource = dt;
    //    gv.DataBind();

    //    int colcount = gv.Columns.Count;
    //    gv.Rows[0].Cells.Clear();
    //    gv.Rows[0].Cells.Add(new TableCell());
    //    gv.Rows[0].Cells[0].ColumnSpan = colcount;
    //    gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    //    gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
    //    gv.Rows[0].Cells[0].Font.Bold = true;
    //    gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    //    gv.Rows[0].Attributes["onclick"] = "";
    //}

    public DataTable PivotTable(DataTable source)
    {
        DataTable dest = new DataTable("Pivoted" + source.TableName);
        List<string> DateList = new List<string> { };
        dest.Columns.Add(" ");
        foreach (DataRow r in source.Rows)
        {
            if (!DateList.Contains(r[0].ToString()))
            {
                dest.Columns.Add(r[0].ToString()); // assign each row the Productname (r[0])            
                DateList.Add(r[0].ToString());
            }
        }

        List<string> ReferenceValue = new List<string> { };
        string strReference = "";
        int cnt = 0;
        for (int i = 0; i < source.Rows.Count - 1; i++)
        {
            strReference = source.Rows[i]["FLDREFERNCE"].ToString();
            if (!ReferenceValue.Contains(strReference))
            {
                dest.Rows.Add(dest.NewRow());
                ReferenceValue.Add(strReference);
                dest.Rows[cnt][0] = strReference;
                cnt++;
            }
        }

        string desRowCell;
        string desColCell;
        string souRowCell;
        string souColCell;
        for (int r = 0; r < dest.Rows.Count; r++)
        {
            desRowCell = dest.Rows[r][0].ToString();
            for (int c = 0; c < dest.Columns.Count; c++)
            {
                desColCell = dest.Columns[c].ToString();
                for (int i = 0; i < source.Rows.Count; i++)
                {
                    souRowCell = source.Rows[i]["FLDREFERNCE"].ToString();
                    souColCell = source.Rows[i]["FLDDATE"].ToString();
                    if (souRowCell == desRowCell && (souColCell == desColCell))
                    {
                        dest.Rows[r][c] = source.Rows[i]["FLDVALUE"];
                    }
                }

            }
        }
        dest.AcceptChanges();
        return dest;
    }

    private void ExportToExcel(DataTable table, string filePath)
    {
        StreamWriter sw = new StreamWriter(filePath, false);
        sw.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        sw.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        sw.Write("<BR><BR><BR>");
        sw.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
        int columnscount = table.Columns.Count;

        for (int j = 0; j < columnscount; j++)
        {
            sw.Write("<Td>");
            sw.Write("<B>");
            sw.Write(table.Columns[j].ToString());
            sw.Write("</B>");
            sw.Write("</Td>");
        }
        sw.Write("</TR>");
        foreach (DataRow row in table.Rows)
        {
            sw.Write("<TR>");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                sw.Write("<Td>");
                sw.Write(row[i].ToString());
                sw.Write("</Td>");
            }
            sw.Write("</TR>");
        }
        sw.Write("</Table>");
        sw.Write("</font>");
        sw.Close();
    }










    protected void gvReport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvReport_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
            {
                gvReport.Columns[0].Visible = false;
            }
            if (e.Item is GridDataItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                string ReportID = ((RadLabel)e.Item.FindControl("lblReportID")).Text;
                LinkButton vr = (LinkButton)e.Item.FindControl("cmdDatewiseReport");
                if (vr != null)
                {
                    vr.Visible = SessionUtil.CanAccess(this.ViewState, vr.CommandName);
                    vr.Attributes.Add("onclick", "openNewWindow('DatewiseDMRMonthlyReport', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=11&reportcode=DATEWISEREPORT&MonthlyReportID=" + ReportID + "&showmenu=0&showword=no&showexcel=no');return false;");
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvReport_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;


            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                //_gridView.EditIndex = -1;
                BindData();
            }
            if (e.CommandName.ToUpper() == "EDIT")
            {
                string ReportID = ((RadLabel)e.Item.FindControl("lblReportID")).Text;
                Session["MONTHLYREPORTID"] = ReportID;
                Response.Redirect("CrewOffshoreDMRMonthlyReport.aspx?ReportID=" + ReportID, false);
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string ReportID = ((RadLabel)e.Item.FindControl("lblReportID")).Text;
                PhoenixCrewOffshoreDMRMonthlyReport.MonthlyReportDelete(new Guid(ReportID));
                BindData();
            }
            if (e.CommandName.ToUpper().Equals("DATEWISEREPORTEXCEL"))
            {
                string VesselName, Month;
                int Year;
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                    VesselName = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
                else
                    VesselName = ((RadLabel)e.Item.FindControl("lblVesselName")).Text;
                DateTime Date = DateTime.Parse(((LinkButton)e.Item.FindControl("lnkReportID")).Text);

                Month = Date.ToString("MMMM");
                Year = Date.Year;

                string ReportID = ((RadLabel)e.Item.FindControl("lblReportID")).Text;
                DataTable dt = PhoenixCrewOffshoreDMRMonthlyReport.DatewiseDMRMonthlyReport(new Guid(ReportID));
                DataTable dtPvt = PivotTable(dt);
                string filename = HttpContext.Current.Server.MapPath("../Attachments/Temp/DMRMonthlyReport-" + Month + '-' + Year + '-' + VesselName + ".xls");
                ExportToExcel(dtPvt, filename);
                String scriptpopup = String.Format("window.open('" + Session["sitepath"] + "/Attachments/Temp/DMRMonthlyReport-" + Month + '-' + Year + '-' + VesselName + ".xls');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucVessel_TextChangedEvent(object sender, EventArgs e)
    {
        gvReport.Rebind();
    }
}
