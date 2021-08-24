using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;

public partial class Registers_Individualdrillreport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {


        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionDrillIndividualReport.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvindvdrillreport')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        Tabstripdrillreportmenu.MenuList = toolbargrid.Show();
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
           

            ViewState["PAGENUMBER"] = 1;
            ViewState["VESSELID"] = Request.QueryString["vessel"];
            ViewState["DRILLID"] = Request.QueryString["drillid"];
            ViewState["YEAR"] = Request.QueryString["year"];
            ViewState["MONTH"] = Request.QueryString["month"];
            ViewState["SCENARIO"] = Request.QueryString["scenario"];
            gvindvdrillreport.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }
    }

    protected void drillreport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
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

        string[] alColumns = { "FLDVESSELNAME", "FLDDRILLNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDSCENARIO", "FLDDESCRIPTION", "FLDREMARKS", "FLDLASTDONEDATE","FLDREASON" };
        string[] alCaptions = { "Vessel", "Name", "Interval", "Interval type", "Scenario", "Description", "Remarks", "Last done date","Reason for no attachments" };

        int rowcount = 0;
        int? vesselid = General.GetNullableInteger(ViewState["VESSELID"].ToString());
        string scenario = General.GetNullableString(ViewState["SCENARIO"].ToString());
        Guid? drillid = General.GetNullableGuid(ViewState["DRILLID"].ToString());
        int? year = General.GetNullableInteger(ViewState["YEAR"].ToString());
        int? month = General.GetNullableInteger(ViewState["MONTH"].ToString());
        DataTable dt = PhoenixInspectionDrillLog.individualdrillreport(vesselid, scenario, drillid, year, month, ref rowcount, gvindvdrillreport.CurrentPageIndex + 1, gvindvdrillreport.PageSize);

        Response.AddHeader("Content-Disposition", "attachment; filename=Individual Drill Report.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Individual Drill Report</h3></td>");
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
        foreach (DataRow dr in dt.Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                if (alColumns[i] == "FLDLASTDONEDATE")
                {
                    Response.Write(General.GetDateTimeToString(dr[alColumns[i]]));
                }
                else
                {
                    Response.Write(dr[alColumns[i]]);
                }
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void gvindvdrillreport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int rowcount = 0;
        int? vesselid = General.GetNullableInteger(ViewState["VESSELID"].ToString());
        string scenario = General.GetNullableString(ViewState["SCENARIO"].ToString());
        Guid? drillid = General.GetNullableGuid(ViewState["DRILLID"].ToString());
        int? year = General.GetNullableInteger(ViewState["YEAR"].ToString());
        int? month = General.GetNullableInteger(ViewState["MONTH"].ToString());
        DataTable dt = PhoenixInspectionDrillLog.individualdrillreport(vesselid, scenario, drillid, year, month, ref rowcount, gvindvdrillreport.CurrentPageIndex + 1, gvindvdrillreport.PageSize);

        gvindvdrillreport.DataSource = dt;
        gvindvdrillreport.VirtualItemCount = rowcount;
        string[] alColumns = { "FLDVESSELNAME", "FLDDRILLNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDSCENARIO", "FLDDESCRIPTION", "FLDREMARKS", "FLDLASTDONEDATE", "FLDREASON" };
        string[] alCaptions = { "Vessel", "Name", "Interval", "Interval type", "Scenario", "Description", "Remarks", "Last done date", "Reason for no attachments" };

        DataSet ds = dt.DataSet;

        General.SetPrintOptions("gvindvdrillreport", "Drill Individual Report", alCaptions, alColumns, ds);

    }
    public void gvindvdrillreport_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? drillscheduleid = General.GetNullableGuid(item.GetDataKeyValue("FLDDRILLSCHEDULEID").ToString());
                DataTable dt = PhoenixInspectionDrillSchedule.getflddtkey(drillscheduleid);
                Guid? flddtkey = General.GetNullableGuid(dt.Rows[0]["FLDDTKEY"].ToString());

                LinkButton db = ((LinkButton)item.FindControl("btnattachments"));
                if (db != null)
                {
                    db.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + flddtkey + "&mod="
                            + PhoenixModule.QUALITY +"&u=n" +"'); return false;");
                }
                if (General.GetNullableString(dt.Rows[0]["FLDREASON"].ToString()) != null)
                { db.Visible = false; }
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }
}