using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselPosition;
using Telerik.Web.UI;

public partial class VesselPositionESIRegister : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            //if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() != "OWNER")
            //    tblSearch.Visible = false;
            //else
            //    tblSearch.Visible = true;

            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbartap = new PhoenixToolbar();
            toolbartap.AddButton("ESI", "ESI");
            toolbartap.AddButton("Quarterly EEOI", "EEOI");
            toolbartap.AddButton("BDN", "BDN");
            toolbartap.AddButton("Chart", "CHART");
            MenuTab.AccessRights = this.ViewState;
            MenuTab.MenuList = toolbartap.Show();
            MenuTab.SelectedMenuIndex = 0;

            PhoenixToolbar toolbarnoonreportlist = new PhoenixToolbar();
            toolbarnoonreportlist.AddFontAwesomeButton("../VesselPosition/VesselPositionESIRegister.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbarnoonreportlist.AddFontAwesomeButton("javascript:CallPrint('gvESI')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarnoonreportlist.AddFontAwesomeButton("../VesselPosition/VesselPositionESIRegister.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "RESET");
            MenuNoonReportList.AccessRights = this.ViewState;
            MenuNoonReportList.MenuList = toolbarnoonreportlist.Show();

            if (!IsPostBack)
            {
                ViewState["VesselId"] = "";

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ucVEssel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ViewState["VesselId"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVEssel.Enabled = false;
                }
                else
                {
                    ucVEssel.Enabled = true;
                }

                gvESI.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        ViewState["VesselId"] = ucVEssel.SelectedVessel;
        Rebind();
    }
    protected void MenuTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("BDN"))
        {
            Response.Redirect("../VesselPosition/VesselPositionBunkerReceiptList.aspx");
        }

        if (CommandName.ToUpper().Equals("CHART"))
        {
            Response.Redirect("../VesselPosition/VesselPositionESIChart.aspx");
        }
        if (CommandName.ToUpper().Equals("EEOI"))
        {
            Response.Redirect("../VesselPosition/vesselpositionyeartodatequaterreport.aspx");
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }

    protected void NoonReportList_TabStripCommand(object sender, EventArgs e)
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
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
            {
                ucVEssel.SelectedVessel = "";
                ViewState["VesselId"] = "";
            }
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
    }

    protected void gvESI_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0 && PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() != "OWNER")
                gvESI.Columns[0].Visible = false;

            if (e.Item is GridEditableItem)
            {
                RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkPublishESI");
                
                if (cb != null)
                {
                    if (drv["FLDPUBLISHFLAG"].ToString() == "1")
                        cb.Visible = true;
                    else
                        cb.Visible = false;

                    if (drv["FLDPUBLISHFLAG"].ToString() == "1" && drv["FLDPUBLISHESIYN"].ToString() == "1")
                    {
                        cb.Checked = true;
                        e.Item.CssClass = "datagrid_selectedstyle";

                        UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("TtipPublish");
                        if (uct != null)
                        {
                            uct.Position = ToolTipPosition.TopCenter;
                            uct.TargetControlId = cb.ClientID;
                        }
                    }
                    else
                    {
                        UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("TtipPublish");
                        if (uct != null)
                        {
                            uct.Text = "Publish";
                            uct.Position = ToolTipPosition.TopCenter;
                            uct.TargetControlId = cb.ClientID;
                        }
                    }

                   
                }
               

                LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (cmdEdit != null)
                {
                    cmdEdit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','"+Session["sitepath"]+"/VesselPosition/VesselPositionESICalculationBreakup.aspx?vesselid="
                           + drv["FLDVESSELID"].ToString() + "&voyageid=" + drv["FLDVOYAGEID"].ToString() + "&noonreportid=" + drv["FLDNOONREPORTID"].ToString() + "'); return false;");
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns;
        string[] alCaptions;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() != "OWNER")
        {
            alColumns = new string[12] { "FLDNOONREPORTDATE", "FLDCO2EMISSION", "FLDCO2INDEX", "FLDEEOI", "FLDSOXEMISSION", "FLDSOXINDEX", "FLDSOXINDICATOR", "FLDNOXEMISSION", "FLDNOXINDEX", "FLDNOXINDICATOR", "FLDSOX", "FLDESI" };
            alCaptions = new string[12] { "Date", "CO2 Emission(mT)", "CO2 Index (kg/nm)", "EEOI_CO2(g/nm-t)", "SOx Emission(mT)", "SOx Index(g/nm)", "EEOI_SOx (mg/nm-t)", "NOx Emission(mT)", "NOx Index (g/nm)", "EEOI_NOx (mg/nm-t)", "ESI_SOx", "Overall ESI" };
        }
        else
        {
            alColumns = new string[13] { "FLDVESSELNAME", "FLDNOONREPORTDATE", "FLDCO2EMISSION", "FLDCO2INDEX", "FLDEEOI", "FLDSOXEMISSION", "FLDSOXINDEX", "FLDSOXINDICATOR", "FLDNOXEMISSION", "FLDNOXINDEX", "FLDNOXINDICATOR", "FLDSOX", "FLDESI" };
            alCaptions = new string[13] { "Vessel", "Date", "CO2 Emission(mT)", "CO2 Index (kg/nm)", "EEOI_CO2 (g/nm-t)", "SOx Emission(mT)", "SOx Index (g/nm)", "EEOI_SOx (mg/nm-t)", "NOx Emission(mT)", "NOx Index (g/nm)", "EEOI_NOx (mg/nm-t)", "ESI_SOx", "Overall ESI" };
        }

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixVesselPositionNoonReport.ESISearch(
             General.GetNullableInteger(ViewState["VesselId"].ToString()),
            sortexpression,
            sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvESI.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"ESIScores.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Request.Url.Scheme + "://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>ESI Scores</h3></td>");
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

    protected void gvESI_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkPublishESI");

               if(cb.Checked == true)
                {
                    PhoenixVesselPositionNoonReport.ESIPublishUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableInteger( ucVEssel.SelectedVessel)!=null? ucVEssel.SelectedValue : PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , new Guid(((RadLabel)e.Item.FindControl("lblESIId")).Text)
                        , 1);
                }
                else
                {
                    PhoenixVesselPositionNoonReport.ESIPublishUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableInteger(ucVEssel.SelectedVessel) != null ? ucVEssel.SelectedValue : PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , new Guid(((RadLabel)e.Item.FindControl("lblESIId")).Text)
                        , 0);
                }

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


    protected void gvESI_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvESI.CurrentPageIndex + 1;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns;
        string[] alCaptions;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() != "OWNER")
        {
            alColumns = new string[12] { "FLDNOONREPORTDATE", "FLDCO2EMISSION", "FLDCO2INDEX", "FLDEEOI", "FLDSOXEMISSION", "FLDSOXINDEX", "FLDSOXINDICATOR", "FLDNOXEMISSION", "FLDNOXINDEX", "FLDNOXINDICATOR", "FLDSOX", "FLDESI" };
            alCaptions = new string[12] { "Date", "CO2 Emission(mT)", "CO2 Index (kg/nm)", "EEOI_CO2(g/nm-t)", "SOx Emission(mT)", "SOx Index(g/nm)", "EEOI_SOx (mg/nm-t)", "NOx Emission (mT)", "NOx Index (g/nm)", "EEOI_NOx (mg/nm-t)", "ESI_SOx", "Overall ESI" };
        }
        else
        {
            alColumns = new string[13] { "FLDVESSELNAME", "FLDNOONREPORTDATE", "FLDCO2EMISSION", "FLDCO2INDEX", "FLDEEOI", "FLDSOXEMISSION", "FLDSOXINDEX", "FLDSOXINDICATOR", "FLDNOXEMISSION", "FLDNOXINDEX", "FLDNOXINDICATOR", "FLDSOX", "FLDESI" };
            alCaptions = new string[13] { "Vessel", "Date", "CO2 Emission (mT)", "CO2 Index (kg/nm)", "EEOI_CO2 (g/nm-t)", "SOx Emission (mT)", "SOx Index (g/nm)", "EEOI_SOx (mg/nm-t)", "NOx Emission (mT)", "NOx Index (g/nm)", "EEOI_NOx (mg/nm-t)", "ESI_SOx", "Overall ESI" };
        }

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixVesselPositionNoonReport.ESISearch(
            General.GetNullableInteger(ViewState["VesselId"].ToString())
            , sortexpression,
            sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvESI.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvESI", "ESI Scores", alCaptions, alColumns, ds);


        gvESI.DataSource = ds;
        gvESI.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void Rebind()
    {
        gvESI.SelectedIndexes.Clear();
        gvESI.EditIndexes.Clear();
        gvESI.DataSource = null;
        gvESI.Rebind();
    }
}
