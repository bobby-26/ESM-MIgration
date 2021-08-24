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
public partial class CrewOffshoreDMRMonthlyDPOperations : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("List", "MONTHLYREPORTLIST");
            toolbar.AddButton("Status", "MONTHLYREPORT");
            toolbar.AddButton("Engine", "CONSUMPTION");
            toolbar.AddButton("DP Summary", "DP");



            MenuReportTap.AccessRights = this.ViewState;
            MenuReportTap.MenuList = toolbar.Show();
            MenuReportTap.SelectedMenuIndex = 3;

            if (Request.QueryString["Source"] != null && Request.QueryString["Source"].ToString() == "midnightreport")
            {
                MenuReportTap.Visible = false;
            }

            if (!IsPostBack)
            {

                ddlYear.Items.Clear();
                ddlYear.Items.Add(new DropDownListItem("--Select--", ""));
                for (int i = 2005; i <= DateTime.Now.Year; i++)
                {
                    ddlYear.Items.Add((new DropDownListItem(i.ToString(), i.ToString())));
                }

                ddlYear.SelectedValue = DateTime.Now.Year.ToString();
                ddlMonth.SelectedValue = DateTime.Now.Month.ToString();

                ViewState["YEAR"] = DateTime.Now.Year;
                ViewState["MONTH"] = DateTime.Now.Month;
                ucVessel.bind();
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }
                else
                {
                    ViewState["VESSELID"] = "";
                    ucVessel.Enabled = true;
                }
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                EditMonthlyReport();
                gvDPOperation.PageSize = 50;
            }
            DataSet ds = PhoenixCrewOffshoreDMRMonthlyReport.MonthlyReportByVessel(General.GetNullableInteger(ucVessel.SelectedVessel), General.GetNullableInteger(ddlMonth.SelectedValue), General.GetNullableInteger(ddlYear.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                ViewState["MONTH"] = ds.Tables[0].Rows[0]["FLDMONTH"].ToString();
                ViewState["YEAR"] = ds.Tables[0].Rows[0]["FLDYEAR"].ToString();
                Session["MONTHLYREPORTID"] = ds.Tables[0].Rows[0]["FLDMONTHLYREPORTID"].ToString();
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private void EditMonthlyReport()
    {
        DataSet ds = PhoenixCrewOffshoreDMRMonthlyReport.MonthlyReportEdit(new Guid(Session["MONTHLYREPORTID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
            ucVessel.SelectedVessel = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
            ddlYear.SelectedValue = ds.Tables[0].Rows[0]["FLDYEAR"].ToString();
            ddlMonth.SelectedValue = ds.Tables[0].Rows[0]["FLDMONTH"].ToString();            
            ViewState["MONTH"] = ds.Tables[0].Rows[0]["FLDMONTH"].ToString();
            ViewState["YEAR"] = ds.Tables[0].Rows[0]["FLDYEAR"].ToString();            
            ddlYear.Enabled = PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 ? false : true;
            ddlMonth.Enabled = PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 ? false : true;
        }
    }

    private void BindData()
    {
        string[] alColumns;
        string[] alCaptions;

        alColumns = new string[9] { "FLDREPORTDATE", "FLDVESSELNAME", "FLDVOYAGENO", "FLDVESSELSTATUS", "FLDSEAPORTNAME", "FLDETADATE", "FLDETDDATE", "FLDNEXTPLANNEDACTIVITY", "FLDDPDAYYN" };
        alCaptions = new string[9] { "Report Date", "Vessel Name", "Charter Id", "Status", "Port / Location", "ETA", "ETD", "Next Planned Activity","DP Day(Y/N)" };
        
        DataSet ds = PhoenixCrewOffshoreDMRMonthlyReport.MonthlyReportDPSummary(General.GetNullableInteger(ddlMonth.SelectedValue),
            General.GetNullableInteger(ddlYear.SelectedValue),
            General.GetNullableInteger(ucVessel.SelectedVessel));

        General.SetPrintOptions("gvDPOperation", "MidNight Report", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDPOperation.DataSource = ds;
            gvDPOperation.DataBind();
            txtNoOfDays.Text = ds.Tables[0].Rows[0]["FLDDAYSCOUNT"].ToString();
            txtNoOfHours.Text = ds.Tables[0].Rows[0]["FLDHOURSCOUNT"].ToString();
        }
        else
        {
            gvDPOperation.DataSource = ds;
            gvDPOperation.DataBind();
            txtNoOfDays.Text = "";
            txtNoOfHours.Text = "";
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void ReportTapp_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("MONTHLYREPORTLIST"))
        {
            Response.Redirect("CrewOffshoreDMRMonthlyReportList.aspx", false);
        }
        if (CommandName.ToUpper().Equals("CONSUMPTION"))
        {
            Response.Redirect("CrewOffshoreDMRMonthlyConsReport.aspx", false);
        }
        if (CommandName.ToUpper().Equals("ACTIVITY"))
        {
            Response.Redirect("CrewOffshoreDMRMonthlyReport.aspx", false);
        }
    }

    protected void DPOperation_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
    }

    protected void ddlMonth_TextChangedEvent(object sender, EventArgs e)
    {
        ViewState["MONTH"] = ddlMonth.SelectedValue;
        ViewState["YEAR"] = ddlYear.SelectedValue;
        //BindData();
    }

    protected void gvDPOperation_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridViewRow gv = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);

            TableCell tb1 = new TableCell();
            TableCell tb2 = new TableCell();
            TableCell tb3 = new TableCell();
            TableCell tb4 = new TableCell();
            TableCell tb5 = new TableCell();
            TableCell tb6 = new TableCell();

            tb1.ColumnSpan = 1;
            tb2.ColumnSpan = 3;
            tb3.ColumnSpan = 3;
            tb4.ColumnSpan = 3;
            tb5.ColumnSpan = 1;
            tb6.ColumnSpan = 1;

            tb1.Text = "";
            tb2.Text = "1st Operation";
            tb3.Text = "2nd Operation";
            tb4.Text = "3rd Operation";
            tb5.Text = "";
            tb6.Text = "";

            tb1.Attributes.Add("style", "text-align:center;");
            tb2.Attributes.Add("style", "text-align:center;");
            tb3.Attributes.Add("style", "text-align:center;");
            tb4.Attributes.Add("style", "text-align:center;");
            tb5.Attributes.Add("style", "text-align:center;");
            tb6.Attributes.Add("style", "text-align:center;");

            gv.Cells.Add(tb1);
            gv.Cells.Add(tb2);
            gv.Cells.Add(tb3);
            gv.Cells.Add(tb4);
            gv.Cells.Add(tb5);
            gv.Cells.Add(tb6);

            gvDPOperation.Controls[0].Controls.AddAt(0, gv);
        }
    }

    protected void ucVessel_OnTextChangedEvent(object sender, EventArgs e)
    {
        ViewState["MONTH"] = ddlMonth.SelectedValue;
        ViewState["YEAR"] = ddlYear.SelectedValue;
        ViewState["VESSELID"] = ucVessel.SelectedVessel;
    }

    protected void gvDPOperation_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
