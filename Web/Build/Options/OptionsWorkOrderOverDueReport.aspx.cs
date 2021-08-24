using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;

public partial class OptionsWorkOrderOverDueReport : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        if (!IsPostBack)
            BindData();
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT");
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();

            toolbar1.AddImageButton("../Options/OptionsWorkOrderOverDueReport.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar1.AddImageLink("javascript:CallPrint('gvWorkOrder')", "Print Grid", "icon_print.png", "PRINT");
            MenuCrewList.AccessRights = this.ViewState;
            MenuCrewList.MenuList = toolbar1.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

            }            
            BindData();            
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
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter(ddlVessel.SelectedVessel))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {                  
                    ViewState["PAGENUMBER"] = 1;
                    BindData();                    
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidFilter(string vessellist)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (vessellist.Equals("") || vessellist.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Select Vessel";
        }
       
        return (!ucError.IsError);
    }

    protected void CrewList_TabStripCommand(object sender, EventArgs e)
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
        try
        {

            string[] alColumns = { "FLDVESSELNAME", "FLDNOOFWO", "FLDNOOFDUE", "FLDPERCENT", "FLDNOOFCRITIACLDUE", "FLDNOOFDUEEXCEED30DAYS" };
            string[] alCaptions = { "Vessel Name", "No Of Workorder", "No Of Overdue", "Percentage(%)", "Overdue Critical Jobs", "Overdue > 30 days" };
            
            DataSet ds = PhoenixPlannedMaintenanceWorkOrderReport.ReportOverDue(null, null, null, ddlVessel.SelectedVessel);

            General.ShowExcel("Overdue Report", ds.Tables[0], alColumns, alCaptions, null, string.Empty);

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
            string[] alColumns = { "FLDVESSELNAME", "FLDNOOFWO", "FLDNOOFDUE", "FLDPERCENT", "FLDNOOFCRITIACLDUE", "FLDNOOFDUEEXCEED30DAYS" };
            string[] alCaptions = { "Vessel Name", "No Of Workorder", "No Of Overdue", "Percentage(%)", "Overdue Critical Jobs", "Overdue > 30 days" };

            DataSet ds = PhoenixPlannedMaintenanceWorkOrderReport.ReportOverDue(null, null, null, ddlVessel.SelectedVessel);

            General.SetPrintOptions("gvWorkOrder", "Overdue Report", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvWorkOrder.DataSource = ds;
                gvWorkOrder.DataBind();
               
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvWorkOrder);                
            }
         
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();

            int colcount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = colcount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
            gv.Rows[0].Attributes["onclick"] = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
}
