using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using System.Text;
using SouthNests.Phoenix.PhoenixCrewQuickReports;
using Telerik.Web.UI;
using System.Web;

public partial class Crew_CrewReportsOverlappingDays : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportsOverlappingDays.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportsOverlappingDays.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();
            
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SHOWREPORT"] = null;
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            ViewState["OWNER"] = "";        
            //  ShowReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //   protected void OnDataBound(object sender, EventArgs e)
    //   {
    //       GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
    //       TableHeaderCell cell = new TableHeaderCell();
    //       cell.HorizontalAlign = HorizontalAlign.Center;
    //       cell.Text = "Overlap Report Principal: " + ViewState["OWNER"].ToString();
    //       cell.ColumnSpan = 12;
    //       row.Controls.Add(cell);
    //       gvCrew.HeaderRow.Parent.Controls.AddAt(0, row);
    //   }
    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            gvCrew.MasterTableView.ColumnGroups.FindGroupByName("Owner").HeaderText = "Overlap Report Principal: " + ViewState["OWNER"].ToString();
            //cell.Text = "Overlap Report Principal: " + ViewState["OWNER"].ToString();
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
                Response.Redirect("../Crew/CrewReportsOverlappingDays.aspx");
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
                if (!IsValidDetails())
                {
                    ucError.Visible = true;
                    return;
                }              
                ShowReport();
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
        string[] alColumns = {  "FLDVESSELNAME", "FLDVESSELTYPE", "FLDEMPLOYEENAME", "FLDRANKNAME","FLDDOB", "FLDAGE", "FLDNATIONALITY", "FLDSIGNONDATE", "FLDSIGNOFFDATE", "FLDOVERLAPDAYS", "FLDPORTNAME", "FLDREMARKS" };
        string[] alCaptions = {  "Vessel Name", "Type", "Name","Rank", "DOB", "Age", "Nationality", "Sign-On Date", "Sign-Off Date", "No.of Days", "Port", "Remark" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewQuickReports.CrewQuickOverlap(General.GetNullableInteger(ucAddrOwner.SelectedAddress)
                                                    , General.GetNullableInteger(ddlrank.SelectedRank)
                                                    , sortexpression
                                                    , sortdirection
                                                    , 1
                                                    , iRowCount
                                                    , ref iRowCount
                                                    , ref iTotalPageCount
                                                  );

        Response.AddHeader("Content-Disposition", "attachment; filename=SeafarersOverlapReport.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");      
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h3><center>Overlap Report Owner: " + ViewState["OWNER"].ToString() + " </center></h3></td></tr>");
        ;
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");

        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td style='font-family:Arial; font-size:10px;' width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td style='font-family:Arial; font-size:10px;' align='left'>");
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
        ViewState["SHOWREPORT"] = 1;
        //  divTab1.Visible = true;
      
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDVESSELNAME", "FLDVESSELTYPE", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDDOB", "FLDAGE", "FLDNATIONALITY", "FLDSIGNONDATE", "FLDSIGNOFFDATE", "FLDOVERLAPDAYS", "FLDPORTNAME", "FLDREMARKS" };
        string[] alCaptions = { "Vessel Name", "Type", "Name", "Rank", "DOB", "Age", "Nationality", "Sign-On Date", "Sign-Off Date", "No.of Days", "Port", "Remark" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        

        ds = PhoenixCrewQuickReports.CrewQuickOverlap(General.GetNullableInteger(ucAddrOwner.SelectedAddress)
                                                     , General.GetNullableInteger(ddlrank.SelectedRank)
                                                      , sortexpression, sortdirection,
                                                      Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                      gvCrew.PageSize,
                                                      ref iRowCount,
                                                      ref iTotalPageCount
                                                    );

        General.SetPrintOptions("gvCrew", "Overlap Report Owner: " + ViewState["OWNER"].ToString() , alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        if(iRowCount>0)
            ViewState["OWNER"] = ds.Tables[0].Rows[0]["FLDPRINCIPALNAME"].ToString();
        else
            ViewState["OWNER"] = "";
    }

    private bool IsValidDetails()
    {

        ucError.HeaderMessage = "Please provide the following required information";
        if (General.GetNullableInteger(ucAddrOwner.SelectedAddress) == null)
            ucError.ErrorMessage = "Select the owner";    
        return (!ucError.IsError);
    }


    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("PAGE"))
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
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;
            ShowReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}