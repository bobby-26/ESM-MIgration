using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;
using System.Globalization;
using SouthNests.Phoenix.Reports;
using System.Collections.Specialized;
using System.IO;
using SouthNests.Phoenix.Registers;
using System.Web.UI;

public partial class VesselAccountsCrewListRHNonComplianceList : PhoenixBasePage
{
    public decimal strTotalRest = 0.00m;
    public decimal strTotalWork = 0.00m;
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsCrewListRHNonComplianceList.aspx", "Export to PDF", "<i class=\"fas fa-file-pdf\"></i>", "PDF");
            
            MenuWorkHour.AccessRights = this.ViewState;
            MenuWorkHour.MenuList = toolbar.Show();


            if (!IsPostBack)
            {
                ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ddlVessel.bind();
                ddlVessel.Enabled = false;

                if (Request.QueryString["EmployeeId"] != null && Request.QueryString["EmployeeId"].ToString() != null)
                    ViewState["EmployeeId"] = Request.QueryString["EmployeeId"];
                else
                    ViewState["EmployeeId"] = null;

                if (Request.QueryString["RestHourStartId"] != null && Request.QueryString["RestHourStartId"].ToString() != null)
                    ViewState["RestHourStartId"] = Request.QueryString["RestHourStartId"];
                else
                    ViewState["RestHourStartId"] = null;

                if (Request.QueryString["RESTHOUREMPLOYEEID"] != null)
                    ViewState["RESTHOUREMPLOYEEID"] = Request.QueryString["RESTHOUREMPLOYEEID"].ToString();
                else
                    ViewState["RESTHOUREMPLOYEEID"] = null;

                BindMonth();
                BindLevel();
                ViewState["MONTH"] = DateTime.Today.Month.ToString();
                ViewState["YEAR"] = DateTime.Today.Year.ToString();
                ddlMonth.SelectedValue = ViewState["MONTH"].ToString() + "-" + ViewState["YEAR"].ToString();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindMonth()
    {
        DataSet dsMonth = PhoenixVesselAccountsRestHourReports.RestHourReportMonth(int.Parse(ViewState["EmployeeId"].ToString()),General.GetNullableGuid(ViewState["RESTHOUREMPLOYEEID"].ToString()));
        ddlMonth.DataValueField = "FLDMONTHYEARID";
        ddlMonth.DataTextField = "FLDMONTHNAME";
        ddlMonth.DataSource = dsMonth;
        ddlMonth.DataBind();
    }
    protected void ddlMonth_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        string Month = ddlMonth.SelectedValue.ToString();
        string[] SplitMonth = Month.Split(new Char[] { '-' });
        ViewState["MONTH"] = SplitMonth[0];
        ViewState["YEAR"] = SplitMonth[1];

        gvWorkCalender.Rebind();
        gvNC.Rebind();
    }
    protected void ShowReport()
    {
        NameValueCollection nvc = new NameValueCollection();
        nvc.Add("applicationcode", "7");
        nvc.Add("reportcode", "RESTHOURSRECORDNEW");
        nvc.Add("showmenu", "false");
        nvc.Add("showexcel", "no");
        nvc.Add("showword", "no");
        nvc.Add("vesselid", PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString());
        nvc.Add("month", ViewState["MONTH"].ToString());
        nvc.Add("year", ViewState["YEAR"].ToString());
        nvc.Add("employeeid", ViewState["EmployeeId"].ToString());
        nvc.Add("rhstartid", ViewState["RestHourStartId"].ToString());

        string[] _reportfile = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
        string _filename = "";

        DataSet ds = PhoenixSsrsReportsCommon.GetReportAndSubReportData(nvc, ref _reportfile, ref _filename);

        Session["PHOENIXREPORTPARAMETERS"] = nvc;

        PhoenixSSRSReportClass.ExportSSRSReport(_reportfile, ds, PhoenixSSRSReportClass.ExportFileFormat.PDF, "", ref _filename);

        string name = Path.GetFileName(_filename);
                
        string scriptpopup = String.Format("javascript:parent.openNewWindow('pdf','','" + Session["sitepath"] + "/Attachments/TEMP/" + name + "');");
        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

        //ifMoreInfo.Attributes["src"] = "~/Attachments/TEMP/" + name;
    }

    protected void gvWorkCalender_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        if (e.Item is GridDataItem)
        {
            DataSet ds = (DataSet)grid.DataSource;
            DataTable dtncdetails = ds.Tables[2];
            DataRowView drv = (DataRowView)e.Item.DataItem;
            GridDataItem item = (GridDataItem)e.Item;
            

            if (!string.IsNullOrEmpty(drv["FLDNATUREOFWORK"].ToString()) && drv["FLDNATUREOFWORK"].ToString() != "<P><CENTER></CENTER></P>")
            {
                item["NATUREOFWORK"].ToolTip = drv["FLDNATUREOFWORK"].ToString();
                RadToolTipManager1.TargetControls.Add(item["NATUREOFWORK"].ClientID, true);
            }
            strTotalRest = strTotalRest + (General.GetNullableDecimal(drv["FLDRESTHOURS"].ToString()) != null ? Decimal.Parse(drv["FLDRESTHOURS"].ToString()) : 0.00m);
            strTotalWork = strTotalWork + (General.GetNullableDecimal(drv["FLDWORKHOURS"].ToString()) != null ? Decimal.Parse(drv["FLDWORKHOURS"].ToString()) : 0.00m);

            foreach (GridColumn c in grid.Columns)
            {
                string t = c.UniqueName.Replace("FLD", "");
                if (General.GetNullableInteger(t) != null)
                {
                    int reportinghr = int.Parse(t);

                    DataRow[] dr = dtncdetails.Select("FLDDATE = '" + drv["FLDDATE"].ToString() + "' AND FLDREPORTINGHOUR = '" + (reportinghr+1) + "'");

                    if (dr.Length>0)
                    {
                        item[c.UniqueName].ToolTip = dr[0]["FLDNCTEXT"].ToString();
                        RadToolTipManager1.TargetControls.Add(item[c.UniqueName].ClientID, true);
                        ImageButton img = (ImageButton)item.FindControl("ImgFlag" + t);
                        if (img != null)
                            img.Visible = true;

                    }

                }
            }


        }
    }

    protected void gvWorkCalender_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        NameValueCollection nvc = new NameValueCollection();
        nvc.Add("applicationcode", "7");
        nvc.Add("reportcode", "RESTHOURSRECORDNEW");
        nvc.Add("showmenu", "false");
        nvc.Add("showexcel", "no");
        nvc.Add("showword", "no");
        nvc.Add("vesselid", PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString());
        nvc.Add("month", ViewState["MONTH"].ToString());
        nvc.Add("year", ViewState["YEAR"].ToString());
        nvc.Add("employeeid", ViewState["EmployeeId"].ToString());
        nvc.Add("rhstartid", ViewState["RestHourStartId"].ToString());

        DataSet ds = PhoenixVesselAccountsReports.RecordOfRestHoursNew(int.Parse(nvc.Get("vesselid").ToString()),
           int.Parse(nvc.Get("employeeid").ToString()), int.Parse(nvc.Get("month").ToString()), int.Parse(nvc.Get("year").ToString()), General.GetNullableGuid(nvc.Get("rhstartid").ToString()));

        gvWorkCalender.DataSource = ds;
        dt = ds.Tables[1];

        if (dt.Rows.Count > 0)
        {
            General.RadBindCheckBoxList(rdLevel, dt.Rows[0]["FLDNCLEVEL"].ToString());
            txtRemarks.Text = dt.Rows[0]["FLDREMARKS"].ToString();
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            lblShipNameValue.Text = dr["FLDVESSELNAME"].ToString();
            lblIMOValue.Text = dr["FLDIMONUMBER"].ToString();
            lblFlagValue.Text = dr["FLDFLAG"].ToString();
            lblMonthValue.Text = dr["FLDMONTH"].ToString();
            lblNameValue.Text = dr["FLDEMPLOYEENAME"].ToString();
            lblRankValue.Text = dr["FLDRANKNAME"].ToString();
            lblWatchValue.Text = dr["FLDWATCHKEEPERYN"].ToString();


            lblSeafarerName.Text = dr["FLDEMPLOYEESIGNATURE"].ToString();
            lblCO.Text = dr["FLDCENAME"].ToString();
            lblMasterName.Text = dr["FLDMASTERNAME"].ToString();
        }
    }
    protected void MenuWorkHour_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("PDF"))
            {
                ShowReport();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvNC_ItemDataBound(object sender, GridItemEventArgs e)
    {
        
    }

    protected void gvNC_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (dt.Columns.Count > 0)
            gvNC.DataSource = dt;
    }
    protected void BindLevel()
    {
        DataSet ds = PhoenixRegistersHardExtn.ListHardExtn(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 284, 0, null);
        rdLevel.DataSource = ds;
        rdLevel.DataBind();

        rdLevel.Enabled = false;
        txtRemarks.Enabled = false;

    }
}