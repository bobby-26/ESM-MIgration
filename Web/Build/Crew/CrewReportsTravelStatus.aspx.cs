using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using SouthNests.Phoenix.CrewCommon;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class CrewReportsTravelStatus : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            txtOriginId.Attributes.Add("style", "visibility:hidden");
            txtDestinationId.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolbar = new PhoenixToolbar();            
            toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
            
            MenuTravel.AccessRights = this.ViewState;
            MenuTravel.MenuList = toolbar.Show();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            
            toolbargrid.AddFontAwesomeButton("../Crew/CrewReportsTravelStatus.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvTravelStatus')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewReportsTravelStatus.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
                        
            MenuTravelStatus.AccessRights = this.ViewState;
            MenuTravelStatus.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTROW"] = null;
                ViewState["REQUESTID"] = null;
                ViewState["CANCELREQUISITION"] = null;                
                //  ImgShowOrigin.Attributes.Add("onclick","return showPickList('spnPickListOriginCity', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true);");
                //  
                //  ImgShowDestination.Attributes.Add( "onclick", "return showPickList('spnPickListDestinationCity', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true);");

                if (Filter.CurrentTravelStatusFilter != null)
                {
                    NameValueCollection nvc = Filter.CurrentTravelStatusFilter;

                    txtOriginId.Text = nvc.Get("txtOriginId").ToString();
                    txtDestinationId.Text = nvc.Get("txtDestinationId").ToString();
                }
                else
                {
                    NameValueCollection criteria = new NameValueCollection();
                    criteria.Clear();
                    criteria.Add("txtOriginId", txtOriginId.Text);
                    criteria.Add("txtDestinationId", txtDestinationId.Text);
                    Filter.CurrentTravelStatusFilter = criteria;
                }
                gvTravelStatus.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            if (IsPostBack)
            {
                NameValueCollection criteria = new NameValueCollection();

                criteria.Clear();

                criteria.Add("txtOriginId", txtOriginId.Text);
                criteria.Add("txtDestinationId", txtDestinationId.Text);
                Filter.CurrentTravelStatusFilter = criteria;


               // ImgShowOrigin.Attributes.Add(
               //                            "onclick",
               //                            "return showPickList('spnPickListOriginCity', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true);");
               //
               // ImgShowDestination.Attributes.Add(
               //                            "onclick",
               //                            "return showPickList('spnPickListDestinationCity', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true);");
            }
           //   BindReport();
           //   SetPageNavigator();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindReport()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDREQUISITIONNO", "FLDEMPLOYEECODE", "FLDNAME", "FLDRANKNAME", "FLDBATCH", "FLDVESSELNAME", "FLDPORTNAME", "FLDORIGINNAME", "FLDDESTINATIONNAME", "FLDTRAVELDATE", "FLDARRIVALDATE", "FLDTICKETNO", "FLDAMOUNT", "FLDSTATUS" };
            string[] alCaptions = { "Request No.", "File No.", "Name", "Rank", "Batch", "Vessel Name", "Crew Change Port", "Origin", "Destination", "Depature Date", "Arrival Date", "Ticket No", "Total Airfare", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewTravelRequest.SearchTravelStatus((ucVessel.SelectedVessel) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel)
                , (ucVesselType.SelectedVesseltype) == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype)
                , (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool)
                , (ucManager.SelectedAddress) == "," ? null : General.GetNullableString(ucManager.SelectedAddress)
                , (ucPrincipal.SelectedAddress) == "," ? null : General.GetNullableString(ucPrincipal.SelectedAddress)
                , (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                , General.GetNullableInteger(ucpurpose.SelectedReason)
                , General.GetNullableInteger(ddlTravelStatus.SelectedValue)
                , General.GetNullableDateTime(txtStartDate.Text)
                , General.GetNullableDateTime(txtEndDate.Text)
                , General.GetNullableInteger(txtOriginId.Text)
                , General.GetNullableInteger(txtDestinationId.Text)
                , General.GetNullableInteger(ddlofficetravelyn.SelectedValue)
                , null
                , General.GetNullableInteger(ucport.SelectedSeaport)
                , sortexpression, sortdirection
                , (int)ViewState["PAGENUMBER"]
                , gvTravelStatus.PageSize
                , ref iRowCount
                , ref iTotalPageCount);
            General.SetPrintOptions("gvTravelStatus", "Travel Status", alCaptions, alColumns, ds);
            // General.GetNullableInteger(nvc != null ? nvc.Get("chkCanceledEmployees") : "0");
                        
                 gvTravelStatus.DataSource = ds.Tables[0];
                gvTravelStatus.VirtualItemCount = iRowCount;                
                 if (ViewState["TRAVELREQUESTID"] == null)
                     ViewState["TRAVELREQUESTID"] = ds.Tables[0].Rows[0]["FLDREQUESTID"].ToString();
                 ViewState["TRAVELID"] = ds.Tables[0].Rows[0]["FLDTRAVELID"].ToString();
             
             ViewState["CANCELREQUISITION"] = "0";   
             ViewState["ROWCOUNT"] = iRowCount;
             ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

          //  if (ds.Tables[0].Rows.Count > 0)
          //  {
          //      gvTravelStatus.DataSource = ds;
          //      gvTravelStatus.VirtualItemCount = iRowCount;
          //
          //      if (ViewState["TRAVELREQUESTID"] == null)
          //          ViewState["TRAVELREQUESTID"] = gvTravelStatus.MasterTableView.Items[0].GetDataKeyValue("FLDREQUESTID").ToString();
          //      ViewState["TRAVELID"] = ds.Tables[0].Rows[0]["FLDTRAVELID"].ToString();
          //  }
          //  ViewState["CANCELREQUISITION"] = "0";
          //
          //  ViewState["ROWCOUNT"] = iRowCount;
          //  ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuTravel_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        
        if (CommandName.ToUpper().Equals("SHOWREPORT"))
        {
            if (!IsValidDetails())
            {
                ucError.Visible = true;
                return;
            }

            ViewState["PAGENUMBER"] = 1;
            gvTravelStatus.CurrentPageIndex = 0;

            BindReport();
            gvTravelStatus.Rebind();
        }
    }
    private bool IsValidDetails()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (General.GetNullableDateTime(txtStartDate.Text) == null)
            ucError.ErrorMessage = "From date required";
        if (General.GetNullableDateTime(txtEndDate.Text) == null)
            ucError.ErrorMessage = "To date required";
        return (!ucError.IsError);
    }
    protected void MenuTravelStatus_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDREQUISITIONNO", "FLDEMPLOYEECODE", "FLDNAME", "FLDRANKNAME", "FLDBATCH", "FLDVESSELNAME", "FLDPORTNAME", "FLDORIGINNAME", "FLDDESTINATIONNAME", "FLDTRAVELDATE", "FLDARRIVALDATE", "FLDTICKETNO", "FLDAMOUNT", "FLDSTATUS" };
                string[] alCaptions = { "Request No.", "File No.", "Name", "Rank", "Batch", "Vessel Name", "Crew Change Port", "Origin", "Destination", "Depature Date", "Arrival Date", "Ticket No", "Total Airfare", "Status" };

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int sortdirection;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                else
                    sortdirection = 0;
                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataSet ds = PhoenixCrewTravelRequest.SearchTravelStatus((ucVessel.SelectedVessel) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel)
                      , (ucVesselType.SelectedVesseltype) == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype)
                      , (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool)
                      , (ucManager.SelectedAddress) == "," ? null : General.GetNullableString(ucManager.SelectedAddress)
                      , (ucPrincipal.SelectedAddress) == "," ? null : General.GetNullableString(ucPrincipal.SelectedAddress)
                      , (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                      , General.GetNullableInteger(ucpurpose.SelectedReason)
                      , General.GetNullableInteger(ddlTravelStatus.SelectedValue)
                      , General.GetNullableDateTime(txtStartDate.Text)
                      , General.GetNullableDateTime(txtEndDate.Text)
                      , General.GetNullableInteger(txtOriginId.Text)
                      , General.GetNullableInteger(txtDestinationId.Text)
                      , General.GetNullableInteger(ddlofficetravelyn.SelectedValue)
                      , null
                      , General.GetNullableInteger(ucport.SelectedSeaport)
                      , sortexpression, sortdirection
                      , 1
                      , iRowCount
                      , ref iRowCount
                      , ref iTotalPageCount);

                General.ShowExcel("Travel Details", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["CANCELREQUISITION"] = 0;
                Filter.CurrentTravelStatusFilter = null;
                ucPool.SelectedPool = "";
                ucport.SelectedSeaport = "";
                ucPrincipal.SelectedAddress = "";
                ucRank.selectedlist = "";
                ucManager.SelectedAddress = "";
                ucpurpose.SelectedReason = "";
                ddlTravelStatus.SelectedValue = "";
                ucVessel.SelectedVessel = "";
                ucVesselType.SelectedVesseltype = "";
                txtOriginId.Text = "";
                txtDestinationId.Text = "";
                txtStartDate.Text = "";
                txtEndDate.Text = "";
                txtDestinationId.Text = "";
                txtOriginCity.Text = "";
                txtDestinationCity.Text = "";                
                ddlofficetravelyn.SelectedValue = "DUMMY";
                gvTravelStatus.CurrentPageIndex = 0;
                ViewState["PAGENUMBER"] = 1;
                BindReport();
                gvTravelStatus.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }   
    protected void gvTravelStatus_ItemDataBound(object sender, GridItemEventArgs e)
    {
    }
    protected void gvTravelStatus_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().ToString().Equals("PAGE"))
            ViewState["PAGENUMBER"] = null;
    }
    protected void gvTravelStatus_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTravelStatus.CurrentPageIndex + 1;
            BindReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
