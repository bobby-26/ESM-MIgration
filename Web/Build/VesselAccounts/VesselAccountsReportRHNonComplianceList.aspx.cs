using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class VesselAccountsReportRHNonComplianceList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();
            else
                ViewState["vesselid"] = null;

            if (Request.QueryString["month"] != null && Request.QueryString["month"].ToString() != null)
                ViewState["month"] = Request.QueryString["month"];
            else
                ViewState["month"] = null;

            if (Request.QueryString["year"] != null && Request.QueryString["year"].ToString() != null)
                ViewState["year"] = Request.QueryString["year"];
            else
                ViewState["year"] = null;

            if (Request.QueryString["Report"] != null && Request.QueryString["Report"].ToString() != null)
                ViewState["Report"] = Request.QueryString["Report"];
        }
    }

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDSIGNONDATE", "FLDSIGNOFFDATE" };
        string[] alCaptions = { "Employee Name", "Rank Name", "Sign-on Date", "Sign-off Date" };

        ds = PhoenixVesselAccountsRH.RestHourEmployeeMonthWiseList(int.Parse(ViewState["vesselid"].ToString())
            , int.Parse(ViewState["month"].ToString())
            , int.Parse(ViewState["year"].ToString())
            );

        Response.AddHeader("Content-Disposition", "attachment; filename=RestHourCrewList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Work and Rest Hour Month Wise Crewlist</h3></td>");
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
        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDSIGNONDATE", "FLDSIGNOFFDATE" };
        string[] alCaptions = { "Employee Name", "Rank Name", "Sign-on Date", "Sign-off Date" };

        DataSet ds = PhoenixVesselAccountsRH.RestHourEmployeeMonthWiseList(int.Parse(ViewState["vesselid"].ToString())
            , int.Parse(ViewState["month"].ToString())
            , int.Parse(ViewState["year"].ToString())
            );

        General.SetPrintOptions("gvPreSeaExam", "Presea Block", alCaptions, alColumns, ds);

        gvRestHourNonComplianceList.DataSource = ds;
    }

    protected void RestHoursNonComplianceList_TabStripCommand(object sender, EventArgs e)
    {
    }

    protected void gvRestHourNonComplianceList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        RadLabel lblEmployeeId = ((RadLabel)e.Item.FindControl("txtSequenceNumberEdit"));

        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            BindPageURL(e.Item.ItemIndex);
            if (ViewState["Report"].ToString() == "RESTHOURSNC")
            {
                Response.Redirect("../Reports/ReportsView.aspx?applicationcode=7&reportcode=RESTHOURSNC&showmenu=false&showexcel=no&showword=no&VESSELID="
                     + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()
                     + "&month=" + int.Parse(ViewState["month"].ToString())
                     + "&year=" + int.Parse(ViewState["year"].ToString())
                     + "&employeeid=" + int.Parse(ViewState["EMPLOYEEID"].ToString())
                     + "&rhstartid="+ ViewState["RHSTARTID"].ToString());
            }
            else if (ViewState["Report"].ToString() == "RESTHOURSRECORD")
            {
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=7&reportcode=RESTHOURSRECORD&showmenu=false&showexcel=no&showword=no&VESSELID="
                     + int.Parse(ViewState["vesselid"].ToString())
                     + "&month=" + int.Parse(ViewState["month"].ToString())
                     + "&year=" + int.Parse(ViewState["year"].ToString())
                     + "&employeeid=" + int.Parse(ViewState["EMPLOYEEID"].ToString())
                     + "&rhstartid=" + ViewState["RHSTARTID"].ToString());
            }
        }
    }
    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblEmployeeId = ((RadLabel)gvRestHourNonComplianceList.Items[rowindex].FindControl("lblEmployeeID"));
            if (lblEmployeeId != null)
                ViewState["EMPLOYEEID"] = lblEmployeeId.Text;
            RadLabel lblRHStartId = ((RadLabel)gvRestHourNonComplianceList.Items[rowindex].FindControl("lblRHStartid"));
            if(lblRHStartId != null && lblRHStartId.Text != "")
                ViewState["RHSTARTID"] = lblRHStartId.Text;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvRestHourNonComplianceList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
}
