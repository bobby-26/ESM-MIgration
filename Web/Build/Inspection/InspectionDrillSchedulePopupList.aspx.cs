using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;


public partial class Inspection_InspectionDrillSchedulePopupList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionDrillSchedulePopupList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvDrillSchedulelist')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        TabstripMenu.MenuList = toolbargrid.Show();
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
          
            ViewState["DRILLSCHEDULEID"] = Request.QueryString["drillscheduleid"].ToString();
            gvDrillSchedulelist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

     
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        
        string[] alColumns = { "FLDDRILLNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDFIXEDORVARIABLE", "FLDTYPE", "DUEDATE", "LASTDONEDATE" };
        string[] alCaptions = { "Name", "Interval", "Interval Type", "Fixed/Variable", "Type", "Due Date", "Last done Date" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        Guid? drillscheduleid = General.GetNullableGuid(ViewState["DRILLSCHEDULEID"].ToString());
        int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        DataSet ds = PhoenixInspectionDrillSchedule.drillschedulepopupsearch(rowusercode,drillscheduleid,
                                                gvDrillSchedulelist.CurrentPageIndex + 1,
                                                gvDrillSchedulelist.PageSize,
                                                ref iRowCount
                                                );
        Response.AddHeader("Content-Disposition", "attachment; filename=Drill Schedule.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Drill Schedule</h3></td>");
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

    protected void drillschedule_TabStripMenuCommand(object sender, EventArgs e)
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
    public void gvDrillSchedulelist_ItemDataBound(object sender, GridItemEventArgs e)
    {
    }
    
    protected void gvDrillSchedulelist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        

        string[] alColumns = { "FLDDRILLNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDFIXEDORVARIABLE", "FLDTYPE", "DUEDATE", "LASTDONEDATE" };
        string[] alCaptions = { "Name", "Interval", "Interval Type", "Fixed/Variable", "Type", "Due Date", "Last done Date" };

        


        Guid? drillscheduleid = General.GetNullableGuid(ViewState["DRILLSCHEDULEID"].ToString());
        int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        DataSet ds = PhoenixInspectionDrillSchedule.drillschedulepopupsearch( rowusercode,drillscheduleid,
                                                gvDrillSchedulelist.CurrentPageIndex + 1,
                                                gvDrillSchedulelist.PageSize,
                                                ref iRowCount
                                                );


        General.SetPrintOptions("gvDrillSchedulelist", "Drill Schedule list", alCaptions, alColumns, ds);

        gvDrillSchedulelist.DataSource = ds.Tables[0];
        gvDrillSchedulelist.VirtualItemCount = iRowCount;
    }

}