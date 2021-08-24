using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class VesselAccountsReportAllotment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsReportAllotment.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            MenuReportAllotment.AccessRights = this.ViewState;
            MenuReportAllotment.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["REPORTPAGEURL"] = "../Reports/ReportsView.aspx";
                ddlMonth.SelectedMonth = DateTime.Today.Month.ToString();
                ddlYear.SelectedYear = DateTime.Today.Year;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

 

    protected void MenuReportAllotment_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                string selecteddate = DateTime.Today.Day.ToString() + "/" + ddlMonth.SelectedMonth.ToString() + "/" + ddlYear.SelectedYear.ToString();

                if (IsValidDates(selecteddate))
                {
                    ifMoreInfo.Attributes["src"] = ViewState["REPORTPAGEURL"].ToString() + "?applicationcode=7&reportcode=ALLOTMENT&showmenu=false&showexcel=no&showword=no&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "&month=" + ddlMonth.SelectedMonth + "&year=" + ddlYear.SelectedYear + "&type=" + ddlAllotmentType.SelectedValue;

                }
                else
                {
                    ucError.Visible = true;
                    ifMoreInfo.Attributes["src"] = "";
                }
                ScriptManager.RegisterStartupScript(this, typeof(Page), "resizeScript", "resize();", true);
            }

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                string[] alColumns = { "FLDFILENUMBER", "FLDEMPLOYEENAME", "FLDRANK", "FLDACCOUNTNUMBER", "FLDBANKNAME", "FLDBANKSWIFTCODE", "FLDFULLADDRESS", "FLDBENEFICIARY", "FLDAMOUNT", "FLDREMARKS", "FLDSIGNATURE" };
                string[] alCaptions = { "File No", "Employee Name", "Rank", "Account No", "Bank Name", "Swift Code (Seafarer Bank)", "Bank Address", "Beneficiary", "Amount(USD)", "Remarks", "Signature" };

                DataTable dt = PhoenixVesselAccountsReports.ReportAllotment(PhoenixSecurityContext.CurrentSecurityContext.VesselID, byte.Parse(ddlMonth.SelectedMonth), ddlYear.SelectedYear, byte.Parse(ddlAllotmentType.SelectedValue));

                ShowExcel(ddlAllotmentType.SelectedItem.Text, dt, alColumns, alCaptions, null, null);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidDates(string selecteddate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultdate;
        if (ddlAllotmentType.SelectedValue=="")
        {
            ucError.ErrorMessage = "Allotment Type is Required";
        }

        if (DateTime.TryParse(selecteddate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(DateTime.Now.ToLongDateString())) > 0)
        {
            ucError.ErrorMessage = "Month should be earlier or Equal to Current Month";
        }
        return (!ucError.IsError);
    }

    public static void ShowExcel(string strHeading, DataTable dt, string[] alColumns, string[] alCaptions, int? strSortDirection, string strSortExpression)
    {

        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + (string.IsNullOrEmpty(strHeading) ? "Attachment" : strHeading.Replace(" ", "_")) + ".xls");
        HttpContext.Current.Response.ContentType = "application/vnd.msexcel";
        HttpContext.Current.Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        HttpContext.Current.Response.Write("<tr>");
        HttpContext.Current.Response.Write("<td><img src='http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        HttpContext.Current.Response.Write("<td colspan='" + (alColumns.Length - 1).ToString() + "'><h3>" + strHeading + "</h3></td>");
        HttpContext.Current.Response.Write("</tr>");
        HttpContext.Current.Response.Write("</TABLE>");
        HttpContext.Current.Response.Write("<br />");
        HttpContext.Current.Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        HttpContext.Current.Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            HttpContext.Current.Response.Write("<td width='20%'>");
            HttpContext.Current.Response.Write("<b>" + alCaptions[i] + "</b>");
            HttpContext.Current.Response.Write("</td>");
        }
        HttpContext.Current.Response.Write("</tr>");
        foreach (DataRow dr in dt.Rows)
        {
            HttpContext.Current.Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                HttpContext.Current.Response.Write("<td>");
                HttpContext.Current.Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i].ToString()]);
                HttpContext.Current.Response.Write("</td>");
            }
            HttpContext.Current.Response.Write("</tr>");
        }
        HttpContext.Current.Response.Write("</TABLE>");
        HttpContext.Current.Response.Write("<br/>");
        HttpContext.Current.Response.Write("<br/>");
        HttpContext.Current.Response.Write(ConfigurationManager.AppSettings["softwarename"].ToString() + "/" + PhoenixSecurityContext.CurrentSecurityContext.UserName + "/" + DateTime.Now.ToString());
        HttpContext.Current.Response.End();
    }
}
