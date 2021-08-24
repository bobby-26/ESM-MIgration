using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;

public partial class Registers_DrillIndividualHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionDrillIndividualHistory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvDrillHistorylist')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionDrillIndividualHistory.aspx", "Search", "<i class=\"fa fa-search\"></i>", "SEARCH");

        Tabstripdrillhistorymenu.MenuList = toolbargrid.Show();
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
           
            ViewState["VESSELID"] = Request.QueryString["vesselid"];
            ViewState["DRILLSCHEDULEID"] = Request.QueryString["drillscheduleid"];
            gvDrillHistorylist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            
        }
    }


    protected void ShowExcel()
    {
        int         iRowCount       = 0;
        int         iTotalPageCount = 0;
        string[]    alColumns       = {  "FLDDRILLNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDSCENARIO","FLDDESCRIPTION" , "FLDDRILLLASTDONEDATE" ,"FLDREMARKS","FLDREASON"};
        string[]    alCaptions      = {  "Name", "Interval", "Interval type", "Scenario","Description",   "Done Date" ,"Remarks", "Reason for no attachments" };





        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount       = 10;
        else
                    iRowCount       = Int32.Parse(ViewState["ROWCOUNT"].ToString());

      
        DateTime?   fromdate        = General.GetNullableDateTime(tbfromdateentry.Text);
        DateTime?   todate          = General.GetNullableDateTime(tbtodateentry.Text);

        int?        vesselid        = General.GetNullableInteger(ViewState["VESSELID"].ToString());
        Guid?       drillscheduleid = General.GetNullableGuid(ViewState["DRILLSCHEDULEID"].ToString());

        string      vesselname      = string.Empty;
        DataTable   dt              = PhoenixInspectionDrillHistory.drillindividualhistorylist(vesselid, drillscheduleid,
                                                gvDrillHistorylist.CurrentPageIndex + 1,
                                                gvDrillHistorylist.PageSize, fromdate, todate,
                                                ref iRowCount,
                                                ref iTotalPageCount,
                                                ref vesselname);

        Response.AddHeader("Content-Disposition", "attachment; filename=Drill Individual History.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Drill Individual History</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("<td> Vessel Name = '" + vesselname + "'</td>");
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
                if (alColumns[i] == "FLDDRILLLASTDONEDATE")
                {
                    Response.Write(General.GetDateTimeToString(dr[alColumns[i]]));
                }
                else {
                    Response.Write(dr[alColumns[i]]);
                }
               
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void drillhistorymenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs         dce         = (RadToolBarEventArgs)e;
            string                      CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (CommandName.ToUpper().Equals("SEARCH"))
            {

                gvDrillHistorylist.Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage    = ex.Message;
            ucError.Visible         = true;

        }

    }
    public void gvDrillHistorylist_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                GridDataItem        item            = e.Item as GridDataItem;
                Guid?               drillscheduleid = General.GetNullableGuid(item.GetDataKeyValue("FLDDRILLSCHEDULEID").ToString());
                DataTable           dt              = PhoenixInspectionDrillSchedule.getflddtkey(drillscheduleid);
                Guid?               flddtkey        = General.GetNullableGuid(dt.Rows[0]["FLDDTKEY"].ToString());

                LinkButton          db              = ((LinkButton)item.FindControl("btnattachments"));
                if (db != null)
                {
                    db.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + flddtkey + "&mod="
                             + PhoenixModule.QUALITY + "&u=n" + "'); return false;");
                }
                if (General.GetNullableString(dt.Rows[0]["FLDREASON"].ToString()) !=null)
                { db.Visible = false; }

            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage    = ex.Message;
            ucError.Visible         = true;

        }


    }
    protected void datechange(object sender, EventArgs e)
    {
        gvDrillHistorylist.Rebind();

    }
    protected void gvDrillHistorylist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int         iRowCount       = 0;
        int         iTotalPageCount = 0;
        string      vesselname      = string.Empty;
        DateTime?   fromdate        = General.GetNullableDateTime(tbfromdateentry.Text);
        DateTime?   todate          = General.GetNullableDateTime(tbtodateentry.Text);

        int?        vesselid        = General.GetNullableInteger(ViewState["VESSELID"].ToString());
        Guid?       drillscheduleid = General.GetNullableGuid(ViewState["DRILLSCHEDULEID"].ToString());

        DataTable   dt              = PhoenixInspectionDrillHistory.drillindividualhistorylist(vesselid, drillscheduleid,
                                                gvDrillHistorylist.CurrentPageIndex + 1,
                                                gvDrillHistorylist.PageSize, fromdate,todate,
                                                ref iRowCount,
                                                ref iTotalPageCount,
                                                ref vesselname);

        DataSet ds = dt.DataSet;

        string[] alColumns = { "FLDDRILLNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDSCENARIO", "FLDDESCRIPTION", "FLDDRILLLASTDONEDATE", "FLDREMARKS", "FLDREASON" };
        string[] alCaptions = { "Name", "Interval", "Interval type", "Scenario", "Description", "Done Date", "Remarks", "Reason for no attachments" };

        General.SetPrintOptions("gvDrillHistorylist", "Drill Individual History ", alCaptions, alColumns, ds);

        gvDrillHistorylist.DataSource       = dt;
        gvDrillHistorylist.VirtualItemCount = iRowCount;
       
    }
}