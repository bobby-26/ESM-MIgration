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
public partial class CrewReportTravelCancellationandRefund : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
       
        PhoenixToolbar toolbargrid = new PhoenixToolbar();

        toolbargrid.AddFontAwesomeButton("../Crew/CrewReportTravelCancellationandRefund.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvTravelStatus')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("../Crew/CrewReportTravelCancellationandRefund.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

        MenuTravelStatus.AccessRights = this.ViewState;
        MenuTravelStatus.MenuList = toolbargrid.Show();

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
        MenuTravel.AccessRights = this.ViewState;
        MenuTravel.MenuList = toolbar.Show();

        ucPrincipal.AddressType = ((int)PhoenixAddressType.PRINCIPAL).ToString();

        if (!IsPostBack)
        {
            lstPurposeofTravel.DataSource = PhoenixCrewTravelRequest.ListTravelReason(null);
            lstPurposeofTravel.DataBind();
            lstPurposeofTravel.Items.Insert(0, new RadListBoxItem("--Select--", ""));

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTROW"] = null;

            //  ImgShowOrigin.Attributes.Add( "onclick",   "return showPickList('spnPickListOriginCity', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true);");

            //  ImgShowDestination.Attributes.Add( "onclick", "return showPickList('spnPickListDestinationCity', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true);");
            gvTravelStatus.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        //BindReport();
    }

    protected void MenuTravel_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        
        if (CommandName.ToUpper().Equals("SHOWREPORT"))
        {           
            ViewState["PAGENUMBER"] = 1;
            gvTravelStatus.CurrentPageIndex = 0;
            BindReport();
            gvTravelStatus.Rebind();
        }        
    }
    protected void MenuTravelStatus_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;

                ucVessel.SelectedVessel = "";
                txtOriginCity.Text = "";
                txtOriginId.Text = "";
                txtAgentName.Text = "";
                txtAgentNumber.Text = "";
                txtAgentId.Text = "";
                ucVesselType.SelectedVesseltype = "";
                txtDestinationCity.Text = "";
                txtDestinationId.Text = "";
                ucPrincipal.Text = "";
                ucPrincipal.SelectedValue = "";
                ucPool.SelectedPool = "";
                txtStartDate.Text = "";
                txtEndDate.Text = "";
                ucRank.selectedlist = "";
                txtInvoicefromdate.Text = "";
                txtInvoicetodate.Text = "";
                ucZone.selectedlist = "";
                lstCancelReason.selectedlist = "";
                lstPurposeofTravel.SelectedIndex = -1;
                txtpasengername.Text = "";
                txtinvoicenumber.Text = "";
                ddlRefundCancel.SelectedIndex = -1;
                chkShowArchive.Checked = false;
                ViewState["PAGENUMBER"] = 1;
                gvTravelStatus.CurrentPageIndex = 0;
                BindReport();
                gvTravelStatus.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;


        string[] alColumns = { "FLDROWNUMBER", "FLDPASSENGERNAME", "FLDRANKNAME", "FLDVESSELNAME", "FLDCANCELREFUND", "FLDORGIN", "FLDDESTINATION", "FLDAIRLINECODE", "FLDPONUMBER", "FLDINVOICENUMBER", "FLDINVOICEDATE", "FLDTRAVELDATE", "FLDBASIC", "FLDTOTALTAX", "FLDSTXCOLLECTED", "FLDDISCOUNT", "FLDCANCELLATIONCHARGE", "FLDTOTAL", "FLDTICKETNO", "FLDCANCELREASON" };
        string[] alCaptions = { "SI. No.", "Passenger Name", "Rank", "Vessel", "Cancel/Refund", "Origin", "Destination", "Airline", "Po Number", "Invoice Number", "Invoice Date", "Travel Date", "Basic", "Tax", "Service Tax", "Discount", "Cancel Charges", "Total Refund", "Ticket Number", "Cancel Reason" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in lstPurposeofTravel.Items)
        {
            if (item.Selected && item.Value != string.Empty)
            {
                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }
        }
        if (strlist.Length > 1)
        {
            strlist.Remove(strlist.Length - 1, 1);
        }
        
        DataTable dt = PhoenixCrewTravelRequest.SearchTravelCancellationandRefund(ucVessel.SelectedVessel, ucVesselType.SelectedVesseltype, ucRank.selectedlist
                                                                                , ucPool.SelectedPool, ucZone.selectedlist, txtpasengername.Text
                                                                                , General.GetNullableInteger(txtOriginId.Text), General.GetNullableInteger(txtDestinationId.Text)
                                                                                , General.GetNullableDateTime(txtStartDate.Text), General.GetNullableDateTime(txtEndDate.Text)
                                                                                , General.GetNullableDateTime(txtInvoicefromdate.Text), General.GetNullableDateTime(txtInvoicetodate.Text)
                                                                                , lstCancelReason.selectedlist, General.GetNullableInteger(ddlRefundCancel.SelectedValue)
                                                                                , (byte?)General.GetNullableInteger(chkShowArchive.Checked==true ? "1" : "")
                                                                                , General.GetNullableInteger(txtAgentId.Text), General.GetNullableInteger(ucPrincipal.SelectedValue)
                                                                                , txtinvoicenumber.Text, strlist.ToString()
                                                                                , sortexpression
                                                                                , sortdirection
                                                                                , 1
                                                                                , iRowCount
                                                                                , ref iRowCount
                                                                                , ref iTotalPageCount
                                                                                );
        General.ShowExcel("Cancel and Refund Report", dt, alColumns, alCaptions, null, null);
    }
    protected void BindReport()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROWNUMBER", "FLDPASSENGERNAME", "FLDRANKNAME", "FLDVESSELNAME", "FLDCANCELREFUND", "FLDORGIN", "FLDDESTINATION", "FLDAIRLINECODE", "FLDPONUMBER", "FLDINVOICENUMBER", "FLDINVOICEDATE", "FLDTRAVELDATE", "FLDBASIC", "FLDTOTALTAX", "FLDSTXCOLLECTED", "FLDDISCOUNT", "FLDCANCELLATIONCHARGE", "FLDTOTAL", "FLDTICKETNO", "FLDCANCELREASON" };
        string[] alCaptions = { "SI. No.", "Passenger Name", "Rank", "Vessel", "Cancel/Refund", "Origin", "Destination", "Airline", "Po Number", "Invoice Number", "Invoice Date", "Travel Date", "Basic", "Tax", "Service Tax", "Discount", "Cancel Charges", "Total Refund", "Ticket Number", "Cancel Reason" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in lstPurposeofTravel.Items)
        {
            if (item.Selected && item.Value != string.Empty)
            {
                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }
        }
        if (strlist.Length > 1)
        {
            strlist.Remove(strlist.Length - 1, 1);
        }
        
        DataTable dt = PhoenixCrewTravelRequest.SearchTravelCancellationandRefund(ucVessel.SelectedVessel, ucVesselType.SelectedVesseltype, ucRank.selectedlist
                                                                                , ucPool.SelectedPool, ucZone.selectedlist, txtpasengername.Text
                                                                                , General.GetNullableInteger(txtOriginId.Text), General.GetNullableInteger(txtDestinationId.Text)
                                                                                , General.GetNullableDateTime(txtStartDate.Text), General.GetNullableDateTime(txtEndDate.Text)
                                                                                , General.GetNullableDateTime(txtInvoicefromdate.Text), General.GetNullableDateTime(txtInvoicetodate.Text)
                                                                                , lstCancelReason.selectedlist, General.GetNullableInteger(ddlRefundCancel.SelectedValue)
                                                                                , (byte?)General.GetNullableInteger(chkShowArchive.Checked==true ? "1" : "")
                                                                                , General.GetNullableInteger(txtAgentId.Text), General.GetNullableInteger(ucPrincipal.SelectedValue)
                                                                                , txtinvoicenumber.Text, strlist.ToString()
                                                                                , sortexpression
                                                                                , sortdirection
                                                                                , (int)ViewState["PAGENUMBER"]
                                                                                , gvTravelStatus.PageSize
                                                                                , ref iRowCount
                                                                                , ref iTotalPageCount
                                                                                );
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvTravelStatus", "Cancel and Refund Report", alCaptions, alColumns, ds);
        
            gvTravelStatus.DataSource = dt;
        gvTravelStatus.VirtualItemCount = iRowCount;
        
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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

