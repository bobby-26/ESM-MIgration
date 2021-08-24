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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Inspection_InspectionRegulationClosedVessel : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelpactivity','Filter','Inspection/InspectionRegulationClosedVesselFilter.aspx')", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionRegulationClosedVessel.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvRegulationCompletedVessel')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        gvTabStrip.AccessRights = this.ViewState;
        gvTabStrip.MenuList = toolbargrid.Show();
        if (IsPostBack == false)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            gvRegulationCompletedVessel.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            ViewState["COMPANYID"] = "";
            NameValueCollection nvcCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvcCompany.Get("QMS") != null && nvcCompany.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvcCompany.Get("QMS");
 
            }

        }
    }
    //public DataSet GetRegulationCompletedVesselList(ref int iRowCount, ref int iTotalPageCount)
    //{
    //    int usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
    //    int? complianceStatus = null;
    //    if (Filter.CurrentRegulationClosedVesselFilterCriteria != null)
    //    {
    //        complianceStatus = Convert.ToInt32(Filter.CurrentRegulationClosedVesselFilterCriteria["chkComplianceStatusAdd"]);
    //    }
    //    return PhoenixInspectionNewRegulation.RegulationCompletedVessel(usercode, (int)ViewState["PAGENUMBER"], gvRegulationCompletedVessel.PageSize, ref iRowCount, ref iTotalPageCount, complianceStatus);
    //}

    public DataSet GetRegulationCompletedVesselList(ref int iRowCount, ref int iTotalPageCount)
    {
        int usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        int? complianceStatus = null;
        if (Filter.CurrentRegulationClosedVesselFilterCriteria != null)
        {
            complianceStatus = Convert.ToInt32(Filter.CurrentRegulationClosedVesselFilterCriteria["chkComplianceStatusAdd"]);
        }
        return PhoenixInspectionNewRegulationActionPlan.RegulationCompletedVessel(usercode, (int)ViewState["PAGENUMBER"], gvRegulationCompletedVessel.PageSize, ref iRowCount, ref iTotalPageCount, complianceStatus, General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvRegulationCompletedVessel.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
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

    public void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alCaptions = getAlCaptions();
        string[] alColumns = getAlColumns();
        DataSet ds = new DataSet();
        ds = GetRegulationCompletedVesselList(ref iRowCount, ref iTotalPageCount);
        gvRegulationCompletedVessel.DataSource = ds;
        gvRegulationCompletedVessel.VirtualItemCount = iRowCount;
        PrintReport(ds);
    }
    private string[] getAlColumns()
    {
        string[] alColumns = { "FLDTITLE", "FLDACTIONTOBETAKEN", "FLDVESSELNAME", "FLDTARGETDATE", "FLDCOMPLETIONDATE", "FLDCLOSEDDATE", "FLDCLOSEDSTATUS" };
        return alColumns;
    }
    private string[] getAlCaptions()
    {
        string[] alCaptions = { "Regulation", "Task", "Vessel","Target Date", "Completed Date", "Closed Date", "Status" };
        return alCaptions;
    }
    private void PrintReport(DataSet ds)
    {
        string[] alColumns = getAlColumns();
        string[] alCaptions = getAlCaptions();
        General.SetPrintOptions("gvRegulationCompletedVessel", "Regulation Closed on Vessel", alCaptions, alColumns, ds);
    }
    private void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alColumns = getAlColumns();
        string[] alCaptions = getAlCaptions();
        string sortexpression = null;
        int? sortdirection = null;

        ds = GetRegulationCompletedVesselList(ref iRowCount, ref iTotalPageCount);
        General.ShowExcel("Regulation Closed on Vessel", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void gvRegulationCompletedVessel_NeedDataSource(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRegulationCompletedVessel.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvRegulationCompletedVessel_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel dtKey = new RadLabel();
            GridDataItem item = (GridDataItem)e.Item;
            dtKey = (RadLabel)item.FindControl("lbldtkey");
            LinkButton atbtn = (LinkButton)e.Item.FindControl("lnkAttachment");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (atbtn != null)
            {
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                    atbtn.Controls.Add(html);
                }
                atbtn.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Inspection/InspectionRegulationAttachment.aspx?dtkey=" + dtKey.Text + "&mod=Inspection&u=n'); return false;");
            }

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdEdit");

            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'RegulationSummary', '" + Session["sitepath"] + "/Inspection/InspectionMOCShipBoardTaskDetails.aspx?MOCActionplanid=" + ((RadLabel)e.Item.FindControl("lblId")).Text + "&Vesselid=" + ((RadLabel)e.Item.FindControl("lblVesselid")).Text +"&Regulation=Y');return false;");
            }

        }
    }
    protected void gvRegulationCompletedVessel_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvRegulationCompletedVessel_PreRender(object sender, EventArgs e)
    {
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
        {
            gvRegulationCompletedVessel.MasterTableView.GetColumn("Vessel").Visible = false;
        }
    }
}