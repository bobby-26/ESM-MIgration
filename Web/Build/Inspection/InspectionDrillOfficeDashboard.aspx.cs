using System;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;

public partial class Registers_DrillOfficeDashboard : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionDrillOfficeDashboard.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvDrillofiicedashboard')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        Tabstripdrillofficedashboardmenu.MenuList = toolbargrid.Show();


        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
          
            ViewState["PAGENUMBER"] = 1;

            gvDrillofiicedashboard.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDDRILLNAME", "FLDOVERDUE", };
        string[] alCaptions = { "Mandatory Drills", "Over Due" };





        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        int vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;


        DataTable dt = PhoenixInspectionDrillSummary.drillofficedashboardlist(gvDrillofiicedashboard.CurrentPageIndex + 1,
                                                 gvDrillofiicedashboard.PageSize,
                                                 ref iRowCount,
                                                 ref iTotalPageCount
                                                  , vesselid
                                                 );



        Response.AddHeader("Content-Disposition", "attachment; filename=Drill-Reminder.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Drill-Reminder</h3></td>");
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
    protected void drilljobregistermenu_TabStripCommand(object sender, EventArgs e)
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



    protected void gvDrillofiicedashboard_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        int vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        string[] alColumns = { "FLDDRILLNAME", "FLDOVERDUE" };
        string[] alCaptions = { "Mandatory Drills", "Over Due" };
        DataTable dt = PhoenixInspectionDrillSummary.drillofficedashboardlist(gvDrillofiicedashboard.CurrentPageIndex + 1,
                                                gvDrillofiicedashboard.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount
                                               , vesselid
                                                );

        gvDrillofiicedashboard.DataSource = dt;
        gvDrillofiicedashboard.VirtualItemCount = iRowCount;

        DataSet ds = dt.DataSet;
        General.SetPrintOptions("gvDrillofiicedashboard", "Drill-Reminder", alCaptions, alColumns, ds);



    }

    public void gvDrillofiicedashboard_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {

                GridDataItem item = (GridDataItem)e.Item;
                string type = DataBinder.Eval(e.Item.DataItem, "FLDTYPE").ToString();
                Guid? drillid = General.GetNullableGuid(item.GetDataKeyValue("FLDDRILLID").ToString());
                HtmlAnchor mandatoryoverdue = (HtmlAnchor)item.FindControl("overdueanchor");

                mandatoryoverdue.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Due Drills and Vessels','Inspection/InspectionDrillsvsVesselList.aspx?drillid=" + drillid + "&i=-1" + "&j=-1500" + "&type=" + type + "');return false");


            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}