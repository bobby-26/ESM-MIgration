using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using System.Web;
using Telerik.Web.UI;

public partial class VesselPositionEUMRVStockCheck : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbarvoyagelist = new PhoenixToolbar();
            toolbarvoyagelist.AddFontAwesomeButton("../VesselPosition/VesselPositionEUMRVStockCheck.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbarvoyagelist.AddFontAwesomeButton("javascript:CallPrint('gvTankSoundinglog')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarvoyagelist.AddFontAwesomeButton("../VesselPosition/VesselPositionEUMRVStockCheck.aspx", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbarvoyagelist.AddFontAwesomeButton("../VesselPosition/VesselPositionEUMRVStockCheck.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "RESET");
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
            {
                toolbarvoyagelist.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','"+Session["sitepath"]+"/VesselPosition/VesselPositionEUMRVStockCheckAdd.aspx')", "Add Tank Sounding Log", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            }
            MenuVoyageList.AccessRights = this.ViewState;
            MenuVoyageList.MenuList = toolbarvoyagelist.Show();

            if (!IsPostBack)
            {
                ViewState["VesselId"] = "";
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() != "OWNER")
                        UcVessel.Enabled = false;
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ViewState["VesselId"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvTankSoundinglog.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlVessel_Changed(object sender, EventArgs e)
    {
        ViewState["VesselId"] = UcVessel.SelectedVessel;
        Rebind();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();
        //SetPageNavigator();
    }

    private void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVESSELNAME", "FLDDATE", "FLDLOCATION", "FLDOCCASION", "FLDDRAFTF", "FLDDRAFTA", "FLDLIST", "FLDPORS" };
        string[] alCaptions = { "Vessel", "Date", "Location", "Occasion", "Draft F", "Draft A", "List", "List P/S" };

        string sortexpression;
        int? sortdirection = null;

        int? vesselid = null;
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        else
            vesselid = General.GetNullableInteger(UcVessel.SelectedVessel);

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixVesselPositionEUMRVStockCheck.StockcheckSearch(
            General.GetNullableDateTime(txtReportFrom.Text), 
            General.GetNullableDateTime(txtReportTo.Text),
            General.GetNullableString(ddlLocation.SelectedValue.ToString()),
            General.GetNullableString(ddlPS.SelectedValue.ToString()),
            sortexpression,sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvTankSoundinglog.PageSize, ref iRowCount, ref iTotalPageCount,
            General.GetNullableInteger(ViewState["VesselId"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=\"TankSoundingLog.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Request.Url.Scheme + "://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Tank Sounding Log</h3></td>");
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
   
    protected void VoyageList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            Rebind();
        }

        if (CommandName.ToUpper().Equals("RESET"))
        {
            ClearFilter();
        }
    }

    private void ClearFilter()
    {
        ViewState["PAGENUMBER"] = 1;
        ddlPS.SelectedValue = "";
        txtReportFrom.Text = "";
        txtReportTo.Text = "";
        ddlLocation.SelectedValue = "";
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
        {
            UcVessel.SelectedVessel = "";
        }
        Rebind();
    }


    protected void gvTankSoundinglog_Sorting(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
        Rebind();
    }

    protected void gvTankSoundinglog_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionEUMRVStockCheck.DeleteStockcheck(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblTanksoundinlogid")).Text));

                ucStatus.Text = "Deleted Successfully";

                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SEND"))
            {
                PhoenixVesselPositionEUMRVStockCheck.UpdateStockcheck(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
                General.GetNullableDateTime(((LinkButton)e.Item.FindControl("lnkSoundingDate")).Text),
                General.GetNullableString(((RadLabel)e.Item.FindControl("lblLocationActual")).Text),
                General.GetNullableString(((RadLabel)e.Item.FindControl("lblOccasion")).Text),
                General.GetNullableDecimal(((RadLabel)e.Item.FindControl("lblDraftF")).Text),
                General.GetNullableDecimal(((RadLabel)e.Item.FindControl("lblDraftA")).Text),
                General.GetNullableDecimal(((RadLabel)e.Item.FindControl("lblList")).Text),
                General.GetNullableString(((RadLabel)e.Item.FindControl("lblPS")).Text),
                General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblTanksoundinlogid")).Text),
                1
                );
                ucStatus.Text = "Report sent to office";
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("RESET"))
            {
                PhoenixVesselPositionEUMRVStockCheck.ResetTankSoundingLog(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblTanksoundinlogid")).Text));

                ucStatus.Text = "Reset Successfull";

                Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }        
    }

    protected void gvTankSoundinglog_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadTimePicker ReportTimepic = (((RadTimePicker)e.Item.FindControl("txtReportTime")));
            string ReportTime = ReportTimepic.SelectedTime != null ? ReportTimepic.SelectedTime.Value.ToString() : "";

            PhoenixVesselPositionEUMRVStockCheck.UpdateStockcheck(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
                General.GetNullableDateTime((((UserControlDate)e.Item.FindControl("txtReportDate")).Text + " " + ReportTime)),
                General.GetNullableString(((RadComboBox)e.Item.FindControl("ddlLocation")).SelectedValue),
                General.GetNullableString(((RadTextBox)e.Item.FindControl("txtOccasion")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtDraftf")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtDrafta")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtList")).Text),
                General.GetNullableString(((RadComboBox)e.Item.FindControl("ddlPS")).SelectedValue),
                General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblTanksoundinlogidEdit")).Text),
                null
                );

            ucStatus.Text = "Saved Successfully";
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvTankSoundinglog_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;

        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            RadComboBox ddlLocation = (RadComboBox)e.Item.FindControl("ddlLocation");
            if (ddlLocation != null)
            {
                ddlLocation.SelectedValue = drv["FLDLOCATION"].ToString();
            }
            RadComboBox ddlPS = (RadComboBox)e.Item.FindControl("ddlPS");
            if (ddlPS != null)
            {
                ddlPS.SelectedValue = drv["FLDPORS"].ToString();
            }

            LinkButton Detail = (LinkButton)e.Item.FindControl("cmdSounding");

            if (Detail != null)
            {
                if (General.GetNullableGuid(drv["FLDTANKSOUNDINGLOGID"].ToString()) != null)
                {
                    Detail.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','"+Session["sitepath"]+"/VesselPosition/VesselPositionTankSoundinglogDetail.aspx?SoundingLogId="
                    + drv["FLDTANKSOUNDINGLOGID"].ToString() + "'); return true;");
                }
            }
            LinkButton cmdSend = (LinkButton)e.Item.FindControl("cmdSend");
            if (cmdSend != null)
            {
                cmdSend.Visible = drv["FLDCONFIRMEDYN"].ToString() == "1" ? false : true;
            }
            LinkButton cmdReset = (LinkButton)e.Item.FindControl("cmdReset");
            if (cmdReset != null)
            {
                cmdReset.Visible = drv["FLDRESETFLAG"].ToString() == "0" ? false : true;
            }
        }
    }

 

    protected void gvTankSoundinglog_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTankSoundinglog.CurrentPageIndex + 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        int? vesselid = null;
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        else
            vesselid = General.GetNullableInteger(UcVessel.SelectedVessel);

        DataSet ds = new DataSet();

        ds = PhoenixVesselPositionEUMRVStockCheck.StockcheckSearch(
           General.GetNullableDateTime(txtReportFrom.Text),
           General.GetNullableDateTime(txtReportTo.Text),
           General.GetNullableString(ddlLocation.SelectedValue.ToString()),
           General.GetNullableString(ddlPS.SelectedValue.ToString()),
           sortexpression, sortdirection,
           int.Parse(ViewState["PAGENUMBER"].ToString()),
           gvTankSoundinglog.PageSize, ref iRowCount, ref iTotalPageCount,
           General.GetNullableInteger(ViewState["VesselId"].ToString()));


        string[] alColumns = { "FLDVESSELNAME", "FLDDATE", "FLDLOCATION", "FLDOCCASION", "FLDDRAFTF", "FLDDRAFTA", "FLDLIST", "FLDPORS" };
        string[] alCaptions = { "Vessel", "Date", "Location", "Occasion", "Draft F", "Draft A", "List˚", "List P/S" };

        General.SetPrintOptions("gvTankSoundinglog", "Tank Sounding Log", alCaptions, alColumns, ds);

        gvTankSoundinglog.DataSource = ds;
        gvTankSoundinglog.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void Rebind()
    {
        gvTankSoundinglog.SelectedIndexes.Clear();
        gvTankSoundinglog.EditIndexes.Clear();
        gvTankSoundinglog.DataSource = null;
        gvTankSoundinglog.Rebind();
    }
}
