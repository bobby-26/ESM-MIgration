using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;

using Telerik.Web.UI;
public partial class CrewOffshoreDMRPMSOverdueDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreDMRPMSOverdueDetails.aspx?type=" + Request.QueryString["type"] + "&vesselid=" + Request.QueryString["vesselid"] + "&reportdate=" + Request.QueryString["reportdate"], "Export to Excel", "icon_xls.png", "Excel");
            //MenuShowExcel.AccessRights = this.ViewState;
            //MenuShowExcel.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvDMRPMSOverdue.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                BindData();
                
            }
          
            //SetPageNavigator();

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
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("EXCEL"))
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

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDPLANNINGDUEDATE" };
        string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Due Date" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreDMRPMSOverdueList(General.GetNullableInteger(Request.QueryString["type"].ToString()),
                                                                        General.GetNullableInteger(Request.QueryString["vesselid"].ToString()),
                                                                        General.GetNullableDateTime(Request.QueryString["reportdate"].ToString()),
                                                                        gvDMRPMSOverdue.CurrentPageIndex+1,
                                                                        gvDMRPMSOverdue.PageSize,
                                                                        ref iRowCount,
                                                                        ref iTotalPageCount);

        General.ShowExcel("PMS Overdue", ds.Tables[0], alColumns, alCaptions, 1, null);
        //Response.AddHeader("Content-Disposition", "attachment; filename=PMSOverdue.xls");
        //Response.ContentType = "application/vnd.msexcel";
        //Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        //Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>SHIP MANAGEMENT</center></h5></td></tr>");
        //Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>PMS Overdue</center></h5></td></tr>");
        //Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        //Response.Write("</tr>");
        //Response.Write("</TABLE>");
        //Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        //Response.Write("<tr>");
        //for (int i = 0; i < alCaptions.Length; i++)
        //{
        //    Response.Write("<td style='font-family:Arial; font-size:10px;' width='20%'>");
        //    Response.Write("<b><center>" + alCaptions[i] + "</center></b>");
        //    Response.Write("</td>");
        //}
        //Response.Write("</tr>");
        //foreach (DataRow dr in ds.Tables[0].Rows)
        //{
        //    Response.Write("<tr>");
        //    for (int i = 0; i < alColumns.Length; i++)
        //    {
        //        Response.Write("<td style='font-family:Arial; font-size:10px;' align='left'>");
        //        Response.Write("<center>" + dr[alColumns[i]] + "</center>");
        //        Response.Write("</td>");
        //    }
        //    Response.Write("</tr>");
        //}
        //Response.Write("</TABLE>");
        //Response.End();
    }

    private void BindData()
    {
        ViewState["SHOWREPORT"] = 1;

        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreDMRPMSOverdueList(General.GetNullableInteger(Request.QueryString["type"].ToString()),
                                                                        General.GetNullableInteger(Request.QueryString["vesselid"].ToString()),
                                                                        General.GetNullableDateTime(Request.QueryString["reportdate"].ToString()),
                                                                        gvDMRPMSOverdue.CurrentPageIndex+1,
                                                                        gvDMRPMSOverdue.PageSize,
                                                                        ref iRowCount,
                                                                        ref iTotalPageCount);
        gvDMRPMSOverdue.DataSource = ds;
        //gvDMRPMSOverdue.DataBind();

        gvDMRPMSOverdue.VirtualItemCount = iRowCount;

       
        ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    gvDMRPMSOverdue.EditIndex = -1;
    //    gvDMRPMSOverdue.SelectedIndex = -1;
    //    int result;
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
    //    gvDMRPMSOverdue.SelectedIndex = -1;
    //    gvDMRPMSOverdue.EditIndex = -1;
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
    //        return true;

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
    //protected void gvDMRPMSOverdue_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {

    //        LinkButton lnkWONumber = (LinkButton)e.Row.FindControl("lnkWONumber");
    //        RadLabel lblWorkOrderId = (RadLabel)e.Row.FindControl("lblWorkOrderId");

    //        if (lnkWONumber != null && lblWorkOrderId != null)
    //            lnkWONumber.Attributes.Add("onclick", "Openpopup('MoreInfo', '', '../PlannedMaintenance/PlannedMaintenanceWorkOrderGeneral.aspx?WORKORDERID=" + lblWorkOrderId.Text + "'); return false;");

    //    }
    //}

    protected void gvDMRPMSOverdue_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

            LinkButton lnkWONumber = (LinkButton)e.Item.FindControl("lnkWONumber");
            RadLabel lblWorkOrderId = (RadLabel)e.Item.FindControl("lblWorkOrderId");

            if (lnkWONumber != null && lblWorkOrderId != null)
            {
                //    lnkWONumber.Attributes.Add("onclick", "Openpopup('MoreInfo', '', '../PlannedMaintenance/PlannedMaintenanceWorkOrderGeneral.aspx?WORKORDERID=" + lblWorkOrderId.Text + "'); return false;");
                lnkWONumber.Attributes.Add("onclick", "javascript:openNewWindow('jobaudit','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderGeneral.aspx?WORKORDERID=" + lblWorkOrderId.Text + "'); return false;");
            }
        }
    }

    protected void gvDMRPMSOverdue_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvDMRPMSOverdue_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.ExportToExcelCommandName)
        {
            gvDMRPMSOverdue.ExportSettings.IgnorePaging = true;
            ShowExcel();
        }
    }
}
