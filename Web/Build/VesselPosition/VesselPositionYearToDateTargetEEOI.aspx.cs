using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselPosition;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Linq;
using OfficeOpenXml.Drawing;
using System.Web;
using Telerik.Web.UI;

public partial class VesselPositionYearToDateTargetEEOI : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            UserAccessRights.SetUserAccess(Page.Controls, PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionYearToDateTargetEEOI.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMenuYeartodatequaterreport')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            MenuYeartodatequaterreport.AccessRights = this.ViewState;
            MenuYeartodatequaterreport.MenuList = toolbar.Show();
        
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;

                for (int i = 2005; i <= DateTime.Now.Year; i++)
                {
                    ddlYear.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
                }
                ddlYear.Items.Insert(0, new RadComboBoxItem("--Select--", ""));

                ddlYear.SelectedIndex = 0;

                if (Request.QueryString["VesselId"] != null && PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                {
                    UcVessel.SelectedVessel = Request.QueryString["VesselId"].ToString();
                }
                else if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    UcVessel.Enabled = false;
                }

                UcVessel.DataBind();
                UcVessel.bind();

                gvMenuYeartodatequaterreport.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDEEOIYEAR", "FLDTARGETEEOI" };
        string[] alCaptions = { "Vessel Name", "Year", "Target EEOI" };

        string vesselid;
        vesselid = UcVessel.SelectedVessel;

        DataSet ds = PhoenixVesselPositionYearToDateQuaterReport.YearToDateTargetEEOISearch(
             General.GetNullableInteger(vesselid), int.Parse(ViewState["PAGENUMBER"].ToString()),
             gvMenuYeartodatequaterreport.PageSize, ref iRowCount, ref iTotalPageCount, General.GetNullableInteger(ddlYear.SelectedValue));

        Response.AddHeader("Content-Disposition", "attachment; filename=TargetEEOI.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Request.Url.Scheme + "://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Target EEOI</h3></td>");
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

    protected void MenuNRRangeConfig_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("RESET"))
        {
            ClearFilter();
        }
    }

    private void ClearFilter()
    {
        //txtAgentName.Text = "";
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
            UcVessel.SelectedVessel = "";
        Rebind();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }

    protected void gvMenuYeartodatequaterreport_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                PhoenixVesselPositionYearToDateQuaterReport.InsertYearToDateTargetEEOIReport(General.GetNullableInteger(UcVessel.SelectedVessel),
                    General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlYear")).SelectedValue),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtTargetEEOI")).Text));
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionYearToDateQuaterReport.DeleteYearToDateTagetEEOIReport(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblTargetEEOIId")).Text));
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("Page"))
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

    protected void gvMenuYeartodatequaterreport_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {

            Guid? EEOIid;

            EEOIid = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblTargetEEOIIdEdit")).Text);


            if (EEOIid != null)
            {
                PhoenixVesselPositionYearToDateQuaterReport.UpdateYearToDateTargetEEOIReport(EEOIid,
                    General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlYearEdit")).SelectedValue),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtTargetEEOIEdit")).Text));
            }
            Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMenuYeartodatequaterreport_ItemDataBound(Object sender, GridItemEventArgs e)
    {
 
        DataRowView drv = (DataRowView)e.Item.DataItem;

        if(e.Item is GridEditableItem)
        {
            RadComboBox ddlYear = (RadComboBox)e.Item.FindControl("ddlYearEdit");
            if (ddlYear != null)
            {
                for (int i = 2005; i <= DateTime.Now.Year; i++)
                {
                    ddlYear.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
                }
                ddlYear.Items.Insert(0, new RadComboBoxItem("--Select--", ""));

                ddlYear.SelectedValue = drv["FLDEEOIYEAR"].ToString();
            }

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            }
        }
        if (e.Item is GridFooterItem)
        {
            RadComboBox ddlYear = (RadComboBox)e.Item.FindControl("ddlYear");
            if (ddlYear != null)
            {
                for (int i = 2005; i <= DateTime.Now.Year; i++)
                {
                    ddlYear.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
                }
                ddlYear.Items.Insert(0, new RadComboBoxItem("--Select--", ""));

                ddlYear.SelectedValue = DateTime.Now.Year.ToString();
            }
        }
    }

    protected void gvMenuYeartodatequaterreport_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMenuYeartodatequaterreport.CurrentPageIndex + 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDEEOIYEAR", "FLDTARGETEEOI" };
        string[] alCaptions = { "Vessel Name", "Year", "Target EEOI" };

        string vesselid;
        vesselid = UcVessel.SelectedVessel;

        DataSet ds = PhoenixVesselPositionYearToDateQuaterReport.YearToDateTargetEEOISearch(
            General.GetNullableInteger(vesselid), int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvMenuYeartodatequaterreport.PageSize, ref iRowCount, ref iTotalPageCount, General.GetNullableInteger(ddlYear.SelectedValue));

        General.SetPrintOptions("gvMenuYeartodatequaterreport", "Target EEOI", alCaptions, alColumns, ds);


            gvMenuYeartodatequaterreport.DataSource = ds;
        gvMenuYeartodatequaterreport.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void Rebind()
    {
        gvMenuYeartodatequaterreport.SelectedIndexes.Clear();
        gvMenuYeartodatequaterreport.EditIndexes.Clear();
        gvMenuYeartodatequaterreport.DataSource = null;
        gvMenuYeartodatequaterreport.Rebind();
    }


    protected void UcVessel_TextChangedEvent(object sender, EventArgs e)
    {
        Rebind();
    }

    protected void ddlYear_TextChanged(object sender, EventArgs e)
    {
        Rebind();
    }
}
