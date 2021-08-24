using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using Telerik.Web.UI;
public partial class Crew_CrewReportSignoffFeedBackAnalysis : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar1 = new PhoenixToolbar();

        toolbar1.AddFontAwesomeButton("../Crew/CrewReportSignoffFeedBackAnalysis.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvFeedBackQst')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        MenuShowExcel.AccessRights = this.ViewState;
        MenuShowExcel.MenuList = toolbar1.Show();
        if (!IsPostBack)
        {
            ViewState["empid"] = string.IsNullOrEmpty(Request.QueryString["empid"]) ? "" : Request.QueryString["empid"];
            ViewState["SignOnOffId"] = string.IsNullOrEmpty(Request.QueryString["SignOnOffId"]) ? "" : Request.QueryString["SignOnOffId"];
        }   //  BindData(   string.IsNullOrEmpty(Request.QueryString["empid"]) ? "" : Request.QueryString["empid"], string.IsNullOrEmpty(Request.QueryString["SignOnOffId"]) ? "" : Request.QueryString["SignOnOffId"] );
    }

    private void BindData(string Employeeid, string SignonoffId)
    {
        try
        {
            string[] alColumns = { "FLDQUESTIONNAME", "FLDOPTIONNAME", "FLDCOMMENTS" };
            string[] alCaptions = { "Feed Back Questions", "Answer", "Comments" };
            DataSet ds = new DataSet();
            ds = PhoenixCrewReportSignoffFeedBack.SignoffFeedBackAnalysisSearch(General.GetNullableInteger(Employeeid), General.GetNullableInteger(SignonoffId));
            General.SetPrintOptions("gvFeedBackQst", "Crew Sign Off Feed Back Analysis Report", alCaptions, alColumns, ds);

            gvFeedBackQst.DataSource = ds;

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtName.Text = ds.Tables[0].Rows[0]["FLDNAME"].ToString();
                txtRank.Text = ds.Tables[0].Rows[0]["FLDRANKNAME"].ToString();
                txtSignonDate.Text = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDSIGNONDATE"].ToString());
                txtVessel.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
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
            DataSet ds = new DataSet();
            string[] alColumns = { "FLDQUESTIONNAME", "FLDOPTIONNAME", "FLDCOMMENTS" };
            string[] alCaptions = { "Feed Back Questions", "Answer", "Comments" };
            ds = PhoenixCrewReportSignoffFeedBack.SignoffFeedBackAnalysisSearch(
                General.GetNullableInteger(string.IsNullOrEmpty(ViewState["empid"].ToString()) ? "" : ViewState["empid"].ToString())
                , General.GetNullableInteger(string.IsNullOrEmpty(ViewState["SignOnOffId"].ToString()) ? "" : ViewState["SignOnOffId"].ToString()));
            string sEmployee = "";
            string sRank = "";
            string sSignonDate = "";
            string sVessel = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                sEmployee = ds.Tables[0].Rows[0]["FLDNAME"].ToString();
                sRank = ds.Tables[0].Rows[0]["FLDRANKNAME"].ToString();
                sSignonDate = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDSIGNONDATE"].ToString());
                sVessel = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
            }
            Response.AddHeader("Content-Disposition", "attachment; filename=Crew_Sign_Off_FeedBackAnalysis.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
            Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Sign Off FeedBack Analysis Report</center></h5></td></tr>");
            string shtml = "<tr>";
            shtml += "<td colspan='" + (alColumns.Length).ToString() + "'>";
            shtml += "<table><tr><td><h5><left>Emp Name : " + sEmployee + "</left></h5></td>";
            shtml += "<td><h5><left>Rank :" + sRank + "</left></h5></td></tr></table>";
            shtml += "</td>";
            shtml += "</tr>";
            shtml += "<tr>";
            shtml += "<td colspan='" + (alColumns.Length).ToString() + "'>";
            shtml += "<table><tr><td><h5><left>Sign On Date : " + sSignonDate + "</left></h5></td>";
            shtml += "<td><h5><left> Vessel : " + sVessel + "</left></h5></td></tr></table>";
            shtml += "</td>";
            shtml += "</tr>";
            Response.Write(shtml);
            //Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Emp Name : " + sEmployee + " Rank :" + sRank + "</center></h5></td></tr>");
            //Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Sign On Date : " + sSignonDate + " Vessel :" + sVessel + "</center></h5></td></tr>");
            Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + DateTime.Now.ToShortDateString() + "</td></tr>");
            Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
            Response.Write("</tr>");
            Response.Write("</TABLE>");
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
                    Response.Write("<td align='left'>");
                    Response.Write(Regex.Replace(dr[alColumns[i]].ToString(), @"[^\u0000-\u007F]", string.Empty));
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
            }
            Response.Write("</TABLE>");
            Response.End();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    protected void gvFeedBackQst_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData(string.IsNullOrEmpty(Request.QueryString["empid"]) ? "" : Request.QueryString["empid"], string.IsNullOrEmpty(Request.QueryString["SignOnOffId"]) ? "" : Request.QueryString["SignOnOffId"]);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFeedBackQst_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            RadLabel a = (RadLabel)e.Item.FindControl("lblsno");
            a.Text = (e.Item.ItemIndex + 1).ToString();
        }
    }
}