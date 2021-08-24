using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;
using System.Web.UI.WebControls;
using System.Web.UI;
using SouthNests.Phoenix.Registers;

public partial class Registers_DrillsvsVesselList : PhoenixBasePage
{
    

  
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionDrillsvsVesselList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvDrillvsVessels')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionDrillHistory.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        TabstripMenu.MenuList = toolbargrid.Show();
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
           
            ViewState["PAGENUMBER"] = 1;
            ViewState["TYPE"] = Request.QueryString["type"];
            ViewState["DUE"] = Request.QueryString["i"];
            ViewState["DUE1"] = Request.QueryString["j"];
            ViewState["DRILLID"] = Request.QueryString["drillid"];
            ViewState["A"] = Request.QueryString["a"];
            gvDrillvsVessels.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            ViewState["DRILL"] = "";
            ViewState["VESSELID"] = "";
            ViewState["DUEIN"] = "";
            ViewState["FROMDATE"] = "";
            ViewState["TODATE"] = "";

            if (ViewState["TYPE"].ToString() == "Mandatory")
            {
                DataTable dt = PhoenixRegisterDrill.drilleditlist(General.GetNullableGuid(ViewState["DRILLID"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    ViewState["DRILL"] = dt.Rows[0]["FLDDRILLNAME"].ToString();
                }
               
            }
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDDRILLNAME", "FLDVESSELNAME", "FLDDRILLDUEDATE", "DUEIN", };
        string[] alCaptions = {  "Drill", "Vessel ", "Due on", "Overdue by" };





        Guid? drillid = General.GetNullableGuid(ViewState["DRILLID"].ToString());
        string type = General.GetNullableString(ViewState["TYPE"].ToString());
        int? due = General.GetNullableInteger(ViewState["DUE"].ToString());
        int? due1 = General.GetNullableInteger(ViewState["DUE1"].ToString());
        int? vesselid = General.GetNullableInteger(ViewState["VESSELID"].ToString());
        DateTime? FromDate = General.GetNullableDateTime(ViewState["FROMDATE"].ToString());
        DateTime? ToDate = General.GetNullableDateTime(ViewState["TODATE"].ToString());
        int? DueIn = General.GetNullableInteger(ViewState["DUEIN"].ToString());
        string drill = General.GetNullableString(ViewState["DRILL"].ToString());
        int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        DataTable dt = PhoenixInspectionDrillSummary.DrillvsVessellist(gvDrillvsVessels.CurrentPageIndex + 1,
                                                gvDrillvsVessels.PageSize, type,
                                                ref iRowCount,
                                                ref iTotalPageCount
                                                , rowusercode
                                                , drill
                                                , vesselid
                                                , FromDate
                                                , ToDate
                                                , DueIn);

        Response.AddHeader("Content-Disposition", "attachment; filename=Overdue Drills.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Drill Vs Vessel list</h3></td>");
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

        Guid? drillid = General.GetNullableGuid(ViewState["DRILLID"].ToString());
        string type = General.GetNullableString(ViewState["TYPE"].ToString());
        int? due = General.GetNullableInteger(ViewState["DUE"].ToString());
        int? due1 = General.GetNullableInteger(ViewState["DUE1"].ToString());
        int? vesselid = General.GetNullableInteger(ViewState["VESSELID"].ToString());
        DateTime? FromDate = General.GetNullableDateTime(ViewState["FROMDATE"].ToString());
        DateTime? ToDate = General.GetNullableDateTime(ViewState["TODATE"].ToString());
        int? DueIn = General.GetNullableInteger(ViewState["DUEIN"].ToString());
        string drill = General.GetNullableString(ViewState["DRILL"].ToString());
        int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        DataTable dt = PhoenixInspectionDrillSummary.DrillvsVessellist(gvDrillvsVessels.CurrentPageIndex + 1,
                                                gvDrillvsVessels.PageSize,type,
                                                ref iRowCount,
                                                ref iTotalPageCount
                                                , rowusercode
                                                , drill
                                                ,vesselid
                                                ,FromDate
                                                ,ToDate
                                                ,DueIn);

        gvDrillvsVessels.DataSource = dt;
        gvDrillvsVessels.VirtualItemCount = iRowCount;

        DataSet ds = dt.DataSet;
        string[] alColumns = { "FLDDRILLNAME", "FLDVESSELNAME", "FLDDRILLDUEDATE", "DUEIN", };
        string[] alCaptions = { "Drill", "Vessel ", "Due on", "Overdue by" };

        General.SetPrintOptions("gvDrillvsVessels", "Overdue Drills", alCaptions, alColumns, ds);

    }

    protected void gvDrillvsVessels_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? drillscheduleid = General.GetNullableGuid(item.GetDataKeyValue("FLDDRILLSCHEDULEID").ToString());
                LinkButton vesselname = (LinkButton)item.FindControl("RadlblVesselName");
                vesselname.Attributes.Add("onclick", "javascript:top.openNewWindow('maint','Details','Dashboard/DashboardVesselDetails.aspx?vesselid=" + DataBinder.Eval(e.Item.DataItem, "FLDVESSELID") + "');return false");

            }
            if (e.Item is GridFilteringItem)
            {
                GridFilteringItem item = e.Item as GridFilteringItem;
                RadTextBox DrillName = (RadTextBox)e.Item.FindControl("radtbdrill");
                if (DrillName != null)
                {
                    DrillName.Text = ViewState["DRILL"].ToString();
                }
                RadDropDownList Type = (RadDropDownList)e.Item.FindControl("radtype");
                if (Type != null)
                {
                    Type.SelectedValue = ViewState["TYPE"].ToString();
                }
                UserControlVesselCommon vessel = (UserControlVesselCommon)e.Item.FindControl("ddlvessellist");
                if (vessel != null && ViewState["VESSELID"] != null)
                {
                    vessel.SelectedVessel = ViewState["VESSELID"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }


    protected void gvDrillvsVessels_ItemCommand(object sender, GridCommandEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        if (e.CommandName == RadGrid.FilterCommandName)
        {
           
            
            ViewState["DUEIN"] = gvDrillvsVessels.MasterTableView.GetColumn("DUEIN").CurrentFilterValue;

            

            ViewState["DRILL"] = ((RadTextBox)(e.Item.FindControl("radtbdrill"))).Text;
           
            string daterange = grid.MasterTableView.GetColumn("FLDDATE").CurrentFilterValue;
            string drillfilters = grid.MasterTableView.GetColumn("FLDDRILLNAME").CurrentFilterValue;
            if (daterange != "")
            {
                ViewState["FROMDATE"] = daterange.Split('~')[0];
                ViewState["TODATE"] = daterange.Split('~')[1];
            }

            if (drillfilters != "")
            {
                ViewState["DRILL"] = drillfilters.Split('~')[0];
                ViewState["TYPE"] = drillfilters.Split('~')[1];
            }
            gvDrillvsVessels.Rebind();

        }
    }



    protected void ddlvessellist_TextChangedEvent(object sender, EventArgs e)
    {
        UserControlVesselCommon vessel = (UserControlVesselCommon)sender;

        ViewState["VESSELID"] = vessel.SelectedVessel;
        gvDrillvsVessels.Rebind(); 
    }
}