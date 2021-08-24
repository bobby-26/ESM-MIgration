using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Inspection_InspectionRegulationVesselSummary : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionRegulationReport.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvRegulation')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        MenuDivRegulation.AccessRights = this.ViewState;
        MenuDivRegulation.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
            ViewState["ISTREENODECLICK"] = false;
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

            gvRegulation.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvRegulation.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRegulation_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.ExportToExcelCommandName)
        {
            ShowExcel();
        }
    }

    protected void gvRegulation_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel regulationDtKey = new RadLabel();
            GridDataItem item = (GridDataItem)e.Item;
            regulationDtKey = (RadLabel)item.FindControl("lblRegulationDtkey");
            RadLabel vesselDtKey = (RadLabel)item.FindControl("lblVesselDtkey");
            //LinkButton vesselAttachmentbtn = (LinkButton)e.Item.FindControl("btnAttachment");
            LinkButton regulationAttachment = (LinkButton)e.Item.FindControl("lnkRegulationAttachment");
            if (regulationAttachment != null)
            {
                regulationAttachment.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Inspection/InspectionRegulationAttachment.aspx?dtkey=" + regulationDtKey.Text + "&mod=Inspection&u=n'); return false;");
            }
            //if (vesselAttachmentbtn != null)
            //{
            //    vesselAttachmentbtn.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + vesselDtKey.Text + "&mod=QUALITY&u=n'); return false;");
            //}

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdEdit");
            RadLabel lblRegulationId = (RadLabel)e.Item.FindControl("lblRegulationID");
            if (db1 != null)
            {
                string script = string.Format("openNewWindow('codehelpactivity', 'RegulationSummary', '{0}/Inspection/InspectionRegulationVesselSummaryUpdate.aspx?RegulationId={1}&VesselDtkey={2}'); return true;", Session["sitepath"], lblRegulationId.Text, vesselDtKey.Text);
                //db1.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'RegulationSummary', '" + Session["sitepath"] + "/Inspection/InspectionRegulationVesselSummaryUpdate.aspx?RegulationId=" + lblRegulationId.Text + "');return true;");
                db1.Attributes.Add("onclick", script);
            }
        }
    }

    protected void ShowExcel()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDTITLE", "FLDDESCRIPTION", "FLDISSUEDATE", "FLDISSUEDBYNAME", "FLDACTIONREQUIRED", "FLDDUEDATE", "FLDCLOSEDDATE" };
        string[] alCaptions = { "Title", "Description", "Issue Date", "Issued by", "Action Required", "Due Date", "Date Closed" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        int? vesselId = null;
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
        {
            vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        }
        DataSet ds = PhoenixInspectionNewRegulation.RegulationListbyvessel(gvRegulation.CurrentPageIndex + 1, gvRegulation.PageSize, ref iRowCount, ref iTotalPageCount, vesselId);

        General.ShowExcel("Regulation Rule by Vessel", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }
    public void BindData()
    {
        string[] alColumns = { "FLDTITLE", "FLDDESCRIPTION", "FLDISSUEDATE", "FLDISSUEDBYNAME", "FLDACTIONREQUIRED", "FLDDUEDATE", "FLDCLOSEDDATE" };
        string[] alCaptions = { "Title", "Description", "Issue Date", "Issued by", "Action Required", "Due Date", "Date Closed" };

        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? vesselId = null;
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
        {
            vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        }
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRegulation.CurrentPageIndex + 1;
        DataSet ds = PhoenixInspectionNewRegulation.RegulationListbyvessel(gvRegulation.CurrentPageIndex + 1, gvRegulation.PageSize, ref iRowCount, ref iTotalPageCount, vesselId);
        General.SetPrintOptions("gvRegulation", "Regulation Rule by Vessel", alCaptions, alColumns, ds);

        gvRegulation.DataSource = ds;
        gvRegulation.VirtualItemCount = iRowCount;
    }
    protected void gvRegulation_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void plan_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                //BindData();
                gvRegulation.Rebind();
            }
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
}