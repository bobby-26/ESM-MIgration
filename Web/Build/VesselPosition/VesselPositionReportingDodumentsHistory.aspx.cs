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
using SouthNests.Phoenix.Common;

public partial class VesselPositionReportingDodumentsHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Daily Report", "DAILYREPORT");
            toolbarmain.AddButton("History", "HISTORY");
            MenuTab.AccessRights = this.ViewState;
            MenuTab.MenuList = toolbarmain.Show();
            MenuTab.SelectedMenuIndex = 1;

            PhoenixToolbar toolbarvoyagelist = new PhoenixToolbar();
            toolbarvoyagelist.AddFontAwesomeButton("../VesselPosition/VesselPositionReportingDodumentsHistory.aspx", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbarvoyagelist.AddFontAwesomeButton("../VesselPosition/VesselPositionReportingDodumentsHistory.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "RESET");
            toolbarvoyagelist.AddFontAwesomeButton("../VesselPosition/VesselPositionReportingDodumentsHistory.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            
            MenuVoyageList.AccessRights = this.ViewState;
            MenuVoyageList.MenuList = toolbarvoyagelist.Show();

            if (!IsPostBack)
            {
                for (int i = 2005; i <= DateTime.Now.Year; i++)
                {
                    ddlYear.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
                }
                ddlYear.Items.Insert(0, new RadComboBoxItem("--Select--", ""));

                BindVesselFleetList();

                UcVessel.bind();
                UcVessel.DataBind();
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    UcVessel.Enabled = false;
                    ddlFleet.Enabled = false;
                }
                
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvSTSOperation.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindVesselFleetList()
    {
        DataSet ds = PhoenixRegistersFleet.ListFleet();

        ddlFleet.DataSource = ds;
        ddlFleet.DataTextField = "FLDFLEETDESCRIPTION";
        ddlFleet.DataValueField = "FLDFLEETID";
        ddlFleet.DataBind();
        ddlFleet.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void ddlVessel_Changed(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }
 
    protected void VoyageList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }

        else if (CommandName.ToUpper().Equals("RESET"))
        {
            ViewState["PAGENUMBER"] = 1;
            ClearFilter();
        }
        else if (CommandName.ToUpper().Equals("ADD"))
        {
            populatereportingyear();
        }
    }
    private void populatereportingyear()
    {
        PhoenixVesselPositionReportingdocumentsHistory.ReportingdocumentsHistoryInsert(General.GetNullableInteger(UcVessel.SelectedVessel),1);
        Rebind();
    }
    private void ClearFilter()
    {
        ViewState["PAGENUMBER"] = 1;
        ddlYear.SelectedIndex = 0;       
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
        {
            UcVessel.SelectedVessel = "";
            ddlFleet.SelectedIndex = 0;
        }
        Rebind();
    }
    protected void gvSTSOperation_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSTSOperation.CurrentPageIndex + 1;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixVesselPositionReportingdocumentsHistory.ReportingdocumentsHistorySearch(
        General.GetNullableInteger(UcVessel.SelectedVessel),
           General.GetNullableInteger(ddlFleet.SelectedValue),
           General.GetNullableInteger(ddlYear.SelectedValue),
           1,//IMO DCS DOCUMENTS
           int.Parse(ViewState["PAGENUMBER"].ToString()),
           gvSTSOperation.PageSize, ref iRowCount, ref iTotalPageCount
           );


        //string[] alColumns = { "FLDVESSELNAME", "FLDDATE", "FLDREPORTTYPE", "FLDOPERATIONTYPE", "FLDCAGOQTY", "FLDSTSOPERATION" };
        //string[] alCaptions = { "Vessel", "Date", "Report Type", "Operation Type", "Cargo Qty", "STS Y/N" };

        //General.SetPrintOptions("gvSTSOperation", "STS Operation", alCaptions, alColumns, ds);

        gvSTSOperation.DataSource = ds;
        gvSTSOperation.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void Rebind()
    {
        gvSTSOperation.SelectedIndexes.Clear();
        gvSTSOperation.EditIndexes.Clear();
        gvSTSOperation.DataSource = null;
        gvSTSOperation.Rebind();
    }
    protected void gvSTSOperation_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvSTSOperation_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid? activeyn = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblHistoryid")).Text);
                PhoenixVesselPositionReportingdocumentsHistory.ReportingdocumentsHistoryDelete(activeyn);
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

    protected void gvSTSOperation_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridEditableItem)
        {
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" ><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.VESSELPOSITION + "'); return false;");
            }
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }
    }

    protected void MenuTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("DAILYREPORT"))
        {
            Response.Redirect("../VesselPosition/VesselPositionIMODCSReport.aspx");
        }

    }
}
