using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;

public partial class Registers_Historypopup : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbargrid      = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionDrillHistorypopup.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvDrillHistorypopuplist')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        Tabstripdrillhistoy.MenuList    = toolbargrid.Show();

        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
           
            
            ViewState["DRILLSCHEDULEID"]        = Request.QueryString["drillscheduleid"].ToString();

            gvDrillHistorypopuplist.PageSize    = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount       = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDDRILLNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDSCENARIO", "FLDDESCRIPTION", "FLDREMARKS", "FLDDONEDATE" ,"FLDREASON"};
        string[] alCaptions = { "Vessel", "Name", "Interval", "Interval type", "Scenario", "Description", "Remarks", "Last done date" , "Reason for no attachments" };





        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount       = 10;
        else
                    iRowCount       = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        Guid?       drillscheduleid = General.GetNullableGuid(ViewState["DRILLSCHEDULEID"].ToString());
        DataTable   dt              = PhoenixInspectionDrillHistory.drillhistorylistpopup(drillscheduleid,
                                               gvDrillHistorypopuplist.CurrentPageIndex + 1,
                                               gvDrillHistorypopuplist.PageSize, 
                                               ref iRowCount
                                               );

        Response.AddHeader("Content-Disposition", "attachment; filename=Drill History Across Vessels.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Drill History</h3></td>");
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
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void drillhistory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce         = (RadToolBarEventArgs)e;
            string              CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible      = true;

        }
    
    }
    public void gvDrillHistorypopuplist_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                GridDataItem    item            = e.Item as GridDataItem;
                Guid?           drillscheduleid = General.GetNullableGuid(item.GetDataKeyValue("FLDDRILLSCHEDULEID").ToString());
                DataTable       dt              = PhoenixInspectionDrillSchedule.getflddtkey(drillscheduleid);
                Guid?           flddtkey        = General.GetNullableGuid(dt.Rows[0]["FLDDTKEY"].ToString());

                LinkButton      db              = ((LinkButton)item.FindControl("btnattachments"));
                if (db != null)
                {
                    db.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + flddtkey + "&mod="
                            + PhoenixModule.QUALITY +"&u=n"+ "'); return false;");
                }

                if (General.GetNullableString(dt.Rows[0]["FLDREASON"].ToString()) != null)
                { db.Visible = false; }

            
        }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible      = true;

        }


    }

    protected void gvDrillHistorypopuplist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int         iRowCount       = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDDRILLNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDSCENARIO", "FLDDESCRIPTION", "FLDREMARKS", "FLDDONEDATE" };
        string[] alCaptions = { "Vessel", "Name", "Interval", "Interval type", "Scenario", "Description", "Remarks", "Last done date" };


        Guid?       drillscheduleid = General.GetNullableGuid(ViewState["DRILLSCHEDULEID"].ToString());

        DataTable   dt              = PhoenixInspectionDrillHistory.drillhistorylistpopup(drillscheduleid,
                                               gvDrillHistorypopuplist.CurrentPageIndex + 1,
                                               gvDrillHistorypopuplist.PageSize, 
                                               ref iRowCount
                                               );




        gvDrillHistorypopuplist.DataSource          = dt;
        gvDrillHistorypopuplist.VirtualItemCount    = iRowCount;

        DataSet ds = dt.DataSet;

        General.SetPrintOptions("gvDrillHistorypopuplist", "Drill History Across Vessels", alCaptions, alColumns, ds);
    }

}