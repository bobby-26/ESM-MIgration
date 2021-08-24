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
using SouthNests.Phoenix.Portal;
using Telerik.Web.UI;
public partial class CrewTravelDetailsForAgent : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewTravelDetailsForAgent.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvTravelDetails')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewTravelDetailsForAgent.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewTravelDetailsForAgent.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenutravelList.AccessRights = this.ViewState;
            MenutravelList.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                txtOriginId.Attributes.Add("style", "display:none");
                txtDestinationId.Attributes.Add("style", "display:none");

                gvTravelDetails.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            if (IsPostBack)
            {
                NameValueCollection criteria = new NameValueCollection();

                criteria.Clear();

                criteria.Add("txtOriginId", txtOriginId.Text);
                criteria.Add("txtDestinationId", txtDestinationId.Text);
                Filter.CurrentTravelStatusFilter = criteria;


                ImgShowOrigin.Attributes.Add("onclick",
                                           "return showPickList('spnPickListOriginCity', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListCity.aspx', true);");

                ImgShowDestination.Attributes.Add("onclick",
                                           "return showPickList('spnPickListDestinationCity', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListCity.aspx', true);");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            string[] alColumns = { "FLDREQUISITIONNO", "FLDNAME", "FLDVESSELNAME", "FLDORIGINNAME", "FLDDESTINATIONNAME", "FLDDEPARTUREDATE", "FLDARRIVALDATE", "FLDTICKETNO", "FLDAMOUNT", "FLDTAX", "FLDTOTALAMOUNT" };
            string[] alCaptions = { "Request No.", "Name", "Vessel Name", "Origin", "Destination", "Departure Date", "Arrival Date", "Ticket No.", "Amount", "Tax", "Total Amount" };

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixPortalTravelQuote.TravelDetailsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableString(txtReqNo.Text)
                , General.GetNullableInteger(ddlVessel.SelectedVessel)
                , General.GetNullableString(txtName.Text)
                , General.GetNullableString(txtTicketNo.Text)
                , General.GetNullableDateTime(txtStartDate.Text)
                , General.GetNullableDateTime(txtEndDate.Text)
                , General.GetNullableInteger(txtOriginId.Text)
                , General.GetNullableInteger(txtDestinationId.Text)
                , sortexpression, sortdirection
                , (int)ViewState["PAGENUMBER"]
                , gvTravelDetails.PageSize
                , ref iRowCount
                , ref iTotalPageCount);

            gvTravelDetails.DataSource = ds;
            gvTravelDetails.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            General.SetPrintOptions("gvTravelDetails", "Travel Details", alCaptions, alColumns, ds);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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

    protected void MenutravelList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                if (!IsValidDetails())
                {
                    ucError.Visible = true;
                    return;
                }

                ViewState["PAGENUMBER"] = 1;

                gvTravelDetails.CurrentPageIndex = 0;
                BindData();
                gvTravelDetails.Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentTravelStatusFilter = null;
                txtReqNo.Text = "";
                ddlVessel.SelectedVessel = "";
                txtStartDate.Text = "";
                txtEndDate.Text = "";
                txtName.Text = "";
                txtTicketNo.Text = "";
                txtOriginId.Text = "";
                txtDestinationId.Text = "";
                txtOriginCity.Text = "";
                txtDestinationCity.Text = "";

                ViewState["PAGENUMBER"] = 1;
                gvTravelDetails.CurrentPageIndex = 0;
                BindData();
                gvTravelDetails.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                DataSet ds = new DataSet();
                string[] alColumns = { "FLDREQUISITIONNO", "FLDNAME", "FLDVESSELNAME", "FLDORIGINNAME", "FLDDESTINATIONNAME", "FLDDEPARTUREDATE", "FLDARRIVALDATE", "FLDTICKETNO", "FLDAMOUNT", "FLDTAX", "FLDTOTALAMOUNT" };
                string[] alCaptions = { "Request No.", "Name", "Vessel Name", "Origin", "Destination", "Departure Date", "Arrival Date", "Ticket No.", "Amount", "Tax", "Total Amount" };

                string sortexpression;
                int? sortdirection = null;
                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                ds = PhoenixPortalTravelQuote.TravelDetailsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                   , General.GetNullableString(txtReqNo.Text)
                   , General.GetNullableInteger(ddlVessel.SelectedVessel)
                   , General.GetNullableString(txtName.Text)
                   , General.GetNullableString(txtTicketNo.Text)
                   , General.GetNullableDateTime(txtStartDate.Text)
                   , General.GetNullableDateTime(txtEndDate.Text)
                   , General.GetNullableInteger(txtOriginId.Text)
                   , General.GetNullableInteger(txtDestinationId.Text)
                   , sortexpression, sortdirection
                   , (int)ViewState["PAGENUMBER"]
                   , gvTravelDetails.PageSize
                   , ref iRowCount
                   , ref iTotalPageCount);

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Travel Details", ds.Tables[0], alColumns, alCaptions, null, "");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravelDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTravelDetails.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvTravelDetails_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
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

    protected void gvTravelDetails_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

        }
    }

}
