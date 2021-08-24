using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;

public partial class Inspection_InspectionVesselDrillOverDue : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionVesselDrillOverDue.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvDrillvsVessels')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        TabstripMenu.MenuList = toolbargrid.Show();
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
           
          

            gvDrillvsVessels.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDDRILLNAME",  "FLDDRILLDUEDATE", "DUEIN", };
        string[] alCaptions = { "Drill", "Due on", "Overdue by" };


        int vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        DataTable dt = PhoenixInspectionDrillSummary.OverdueDrills(gvDrillvsVessels.CurrentPageIndex + 1,
                                                gvDrillvsVessels.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount
                                                , vesselid);

        gvDrillvsVessels.DataSource = dt;
        gvDrillvsVessels.VirtualItemCount = iRowCount;


        Response.AddHeader("Content-Disposition", "attachment; filename=OverDue Drills.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Over Due Drills</h3></td>");
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

    protected void drillvsvessels_TabStripMenuCommand(object sender, EventArgs e)
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
    protected void gvDrillvsVessels_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

      
       
       
        int vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        DataTable dt = PhoenixInspectionDrillSummary.OverdueDrills(gvDrillvsVessels.CurrentPageIndex + 1,
                                                gvDrillvsVessels.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount
                                                , vesselid);

        gvDrillvsVessels.DataSource = dt;
        gvDrillvsVessels.VirtualItemCount = iRowCount;

        DataSet ds = dt.DataSet;
        string[] alColumns = { "FLDDRILLNAME", "FLDDRILLDUEDATE", "DUEIN", };
        string[] alCaptions = { "Drill", "Due on", "Overdue by" };

        General.SetPrintOptions("gvDrillvsVessels", "Over Due Drills", alCaptions, alColumns, ds);

    }

    protected void gvDrillvsVessels_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? drillscheduleid = General.GetNullableGuid(item.GetDataKeyValue("FLDDRILLSCHEDULEID").ToString());

                HtmlAnchor drillname = (HtmlAnchor)item.FindControl("Radlblduein");

                drillname.Attributes.Add("onclick", "javascript:parent.openNewWindow('filter','Drill Report','Inspection/InspectionDrillScheduleReport.aspx?drillscheduleid=" + drillscheduleid +"&l=d"+ "');return false");

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            gvDrillvsVessels.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}