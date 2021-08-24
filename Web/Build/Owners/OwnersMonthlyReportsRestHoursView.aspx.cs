using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Reports;
using System.IO;
using Telerik.Web.UI;
using System.Web.UI;

public partial class Owners_OwnersMonthlyReportsRestHoursView : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            ViewState["REPORTPAGEURL"] = "../Reports/ReportsViewWithSubReport.aspx";

            if (!IsPostBack)
            {
                //lnkExpenses.Attributes.Add("onclick", "javascript:openNewWindow('code1', 'Vessel Operating Expenses', '" + Session["sitepath"] + "/Owners/OwnersMonthlyReportsRestHours.aspx',500,800);");

                //ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
                //BindYear();
            }
            //BindToolbar();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void BindYear()
    //{
    //    for (int i = (DateTime.Today.Year - 4); i <= (DateTime.Today.Year); i++)
    //    {
    //        ddlYear.Items.Add(i.ToString());
    //    }
    //    ddlYear.DataBind();
    //}
    //protected void BindToolbar()
    //{
    //    DataTable dt = PhoenixVesselAccountsRH.MonthlyReviewCREWEdit(General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
    //    PhoenixToolbar toolbar = new PhoenixToolbar();
    //    toolbar.AddFontAwesomeButton("../Owners/OwnersMonthlyReportsRestHours.aspx", "Export to PDF", "<i class=\"fas fa-file-pdf\"></i>", "PDF");
    //    //toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);


    //    if (dt.Rows.Count > 0)
    //    {
    //        if (dt.Rows[0]["FLDHODREVIEWYN"].ToString() == "1" && dt.Rows[0]["FLDLOGINUSERRANK"].ToString() == "1" && dt.Rows[0]["FLDMASTERREVEIWEDYN"].ToString() == "0")
    //            toolbar.AddButton("Master Review", "MASTERREVIEW", ToolBarDirection.Right);
    //        else if (dt.Rows[0]["FLDHODREVIEWYN"].ToString() == "1" && dt.Rows[0]["FLDLOGINUSERRANK"].ToString() == "1" && dt.Rows[0]["FLDMASTERREVEIWEDYN"].ToString() == "1")
    //            toolbar.AddButton("Unlock", "UNLOCK", ToolBarDirection.Right);
    //        else if (dt.Rows[0]["FLDHODREVIEWYN"].ToString() == "1" && dt.Rows[0]["FLDLOGINUSERRANK"].ToString() == "0" && dt.Rows[0]["FLDMASTERREVEIWEDYN"].ToString() == "1")
    //            toolbar.AddButton("Office Review", "OFFICEREVIEW", ToolBarDirection.Right);
    //        else if (dt.Rows[0]["FLDHODREVIEWYN"].ToString() == "1" && dt.Rows[0]["FLDLOGINUSERRANK"].ToString() == "0" && dt.Rows[0]["FLDOFFICEREVIEWEDYN"].ToString() == "1")
    //            toolbar.AddButton("Unlock", "UNLOCK", ToolBarDirection.Right);
    //    }

    //    MenuReportNonCompliance.AccessRights = this.ViewState;
    //    MenuReportNonCompliance.MenuList = toolbar.Show();
    //}
    protected void MenuReportNonCompliance_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("MASTERREVIEW"))
            {
                ViewState["CODE"] = "MASTERREVIEW";
                RadWindowManager1.RadConfirm("Are you sure you want to Review this month Work Hours?", "ConfirmVerify", 320, 150, null, "Confirm Review");
                return;
            }
            else if (CommandName.ToUpper().Equals("OFFICEREVIEW"))
            {
                ViewState["CODE"] = "OFFICEREVIEW";
                RadWindowManager1.RadConfirm("Are you sure you want to Review this month Work Hours?", "ConfirmVerify", 320, 150, null, "Confirm Review");
                return;
            }
            else if (CommandName.ToUpper().Equals("UNLOCK"))
            {
                ViewState["CODE"] = "UNLOCK";
                RadWindowManager1.RadConfirm("Are you sure you want to Unlock this month Work Hours?", "ConfirmVerify", 320, 150, null, "Confirm Unlock");
                return;
            }
            else if (CommandName.ToUpper().Equals("PDF"))
            {
                //ShowReport();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void ddlYear_DataBound(object sender, EventArgs e)
    //{
    //    ddlYear.Items.Sort();
    //}
    protected void ucConfirmWorkHours_Click(object sender, EventArgs e)
    {
        try
        {
            //if (ViewState["CODE"].ToString() == "MASTERREVIEW")       //seafarer
            //{
            //    PhoenixVesselAccountsRH.MonthlyReviewByMaster(int.Parse(ddlMonth.SelectedValue), int.Parse(ddlYear.SelectedItem.Text));
            //    RadWindowManager1.RadAlert("Master Reviewed Successfully.", 320, 150, null, "");
            //}
            //if (ViewState["CODE"].ToString() == "OFFICEREVIEW")  //hod
            //{
            //    PhoenixVesselAccountsRH.MonthlyReviewByOffice(int.Parse(ddlMonth.SelectedValue), int.Parse(ddlYear.SelectedItem.Text));

            //    RadWindowManager1.RadAlert("Office Reviewed Successfully.", 320, 150, null, "");
            //}
            //if (ViewState["CODE"].ToString() == "UNLOCK")  //master
            //{
            //    PhoenixVesselAccountsRH.MonthlyReviewUnlock(int.Parse(ddlMonth.SelectedValue), int.Parse(ddlYear.SelectedItem.Text));

            //    RadWindowManager1.RadAlert("Crew Work Hours Unlocked for the month. HOD can unlock for a seafarer.", 320, 150, null, "");
            //}

            //BindToolbar();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    //protected void ddlMonth_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    //{
    //    gvNC.Rebind();
    //}

    //protected void ddlYear_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    //{
    //    gvNC.Rebind();
    //}
    //protected void ShowReport()
    //{
    //    NameValueCollection nvc = new NameValueCollection();
    //    nvc.Add("applicationcode", "7");
    //    nvc.Add("reportcode", "RESTHOURSNCSUMMARYNEW");
    //    nvc.Add("showmenu", "false");
    //    nvc.Add("showexcel", "no");
    //    nvc.Add("showword", "no");
    //    nvc.Add("vesselid", PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString());
    //    nvc.Add("month", ddlMonth.SelectedValue);
    //    nvc.Add("year", ddlYear.SelectedItem.Text);

    //    string _reportfile = "";
    //    string _filename = "";

    //    string[] _reportfiles = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
    //    DataTable dt = PhoenixSsrsReportsCommon.GetReportData(nvc, ref _reportfile, ref _filename);

    //    DataSet ds = new DataSet();
    //    ds.Tables.Add(dt.Copy());
    //    _reportfiles[0] = _reportfile;
    //    Session["PHOENIXREPORTPARAMETERS"] = nvc;

    //    PhoenixSSRSReportClass.ExportSSRSReport(_reportfiles, ds, PhoenixSSRSReportClass.ExportFileFormat.PDF, "", ref _filename);

    //    string name = Path.GetFileName(_filename);

    //    string scriptpopup = String.Format("javascript:parent.openNewWindow('pdf','','" + Session["sitepath"] + "/Attachments/TEMP/" + name + "');");
    //    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

    //    //ifMoreInfo.Attributes["src"] = "~/Attachments/TEMP/" + name;
    //}


    protected void gvNC_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        NameValueCollection nvc = new NameValueCollection();
        nvc.Add("applicationcode", "7");
        nvc.Add("reportcode", "RESTHOURSNCSUMMARYNEW");
        nvc.Add("showmenu", "false");
        nvc.Add("showexcel", "no");
        nvc.Add("showword", "no");
        nvc.Add("vesselid", PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString());
        //nvc.Add("month", ddlMonth.SelectedValue);
        //nvc.Add("year", ddlYear.SelectedItem.Text);

        DataSet ds = PhoenixVesselAccountsReports.RestHoursNonComplianceAnalysisCrewNew(int.Parse(nvc.Get("vesselid").ToString()),
         General.GetNullableDateTime(Filter.SelectedOwnersReportDate));

        gvNC.DataSource = ds;

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            //lblShipNameValue.Text = dr["FLDVESSELNAME"].ToString();
            //lblIMOValue.Text = dr["FLDIMONUMBER"].ToString();
            //lblFlagValue.Text = dr["FLDFLAG"].ToString();
            //lblMonthValue.Text = dr["FLDMONTH"].ToString();
            lblCO.Text = dr["FLDCONAME"].ToString();
            lblCE.Text = dr["FLDCENAME"].ToString();
            lblMasterName.Text = dr["FLDMASTERNAME"].ToString();

        }

    }

    protected void gvNC_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)item.DataItem;

            if (General.GetNullableInteger(drv["FLDNOOFDAYSWITHNC"].ToString()) != null)
            {
                int n = int.Parse(drv["FLDNOOFDAYSWITHNC"].ToString());
                item["NCCOUNT"].BackColor = n == 0 ? System.Drawing.Color.Green : (n > 0 && n <= 2 ? System.Drawing.Color.Yellow : System.Drawing.Color.Red);
            }

        }
    }
}