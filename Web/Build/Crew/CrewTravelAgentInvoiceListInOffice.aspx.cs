using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Text;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewTravelAgentInvoiceListInOffice : PhoenixBasePage
{
    NameValueCollection nvc = new NameValueCollection();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);


            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewTravelAgentInvoiceListInOffice.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCCT')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Crew/CrewTravelAgentInvoiceListInOffice.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Crew/CrewTravelAgentInvoiceListInOffice.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");

            MenutravelList.AccessRights = this.ViewState;
            MenutravelList.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                txtAgentId.Attributes.Add("style", "display:none");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["INVOICENO"] = null;

                cmdShowAgent.Attributes.Add(
                                            "onclick"
                                            , "return showPickList('spnPickListAgent', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=135', true);");
                if (Filter.CurrentTravelInvoiceFilter != null)
                {
                    BindFilterData();
                }
                else
                {
                    ClearFilter();
                }


                gvCCT.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            if (ViewState["Bind"] != null)
            {
                BindData();
                gvCCT.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenutravelList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ClearFilter();
                ViewState["PAGENUMBER"] = 1;
                gvCCT.CurrentPageIndex = 0;
                BindData();
                gvCCT.Rebind();
            }
            else if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;

                ViewState["INVOICENO"] = null;
                SetFilter();

                if (!IsValidInvoiceDate())
                {
                    ClearFilter();
                    gvCCT.CurrentPageIndex = 0;
                    BindData();
                    gvCCT.Rebind();
                    ViewState["Bind"] = 1;
                    ucError.Visible = true;
                    return;
                }
                BindData();
                gvCCT.Rebind();
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

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDINVOICENUMBER", "FLDINVOICEDATE", "FLDNAME", "FLDPASSENGERNAME", "FLDDEPARTUREDATE", "FLDVESSELNAME", "FLDTICKETNO", "FLDTOTAL", "FLDSTATUSNAME", "FLDCREATEDDATE", "FLDACCOUNTVOUCHERNUMBER", "FLDPAIDDATE" };
        string[] alCaptions = { "Invoice No", "Invoice Date", "Agent", "Passenger", "Departure", "Vessel", "Ticket", "Total Charges", "Status", "Imported Date", "Accounts Voucher No", "Paid Date" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (Filter.CurrentTravelInvoiceFilter != null)
            nvc = Filter.CurrentTravelInvoiceFilter;

        ds = PhoenixCrewTravelInvoice.CrewTravelAgentInvoiceOfficeSearch(nvc != null ? nvc.Get("INVOICENO") : string.Empty
                , General.GetNullableString(nvc != null ? nvc.Get("txtInvoiceNumber") : string.Empty)
                , General.GetNullableDateTime(nvc != null ? nvc.Get("txtstartdate") : string.Empty)
                , General.GetNullableDateTime(nvc != null ? nvc.Get("txtenddate") : string.Empty)
                , General.GetNullableInteger(nvc != null ? nvc.Get("ddlvessel") : string.Empty)
                , General.GetNullableInteger(nvc != null ? nvc.Get("ucInvoiceStatus") : string.Empty)
                , General.GetNullableInteger(nvc != null ? nvc.Get("txtAgentId") : string.Empty)
                , General.GetNullableString(nvc != null ? nvc.Get("txtTicket") : string.Empty)
                , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDepDate") : string.Empty)
                , General.GetNullableString(nvc != null ? nvc.Get("txtName") : string.Empty)
                , null
                , sortexpression
                , sortdirection
                , (int)ViewState["PAGENUMBER"]
                , gvCCT.PageSize
                , ref iRowCount
                , ref iTotalPageCount);

        if (ds.Tables.Count > 0)
            General.ShowExcel("Travel Invoice", ds.Tables[0], alColumns, alCaptions, null, null);
        
    }

    protected void BindData()
    {

        try
        {
            if (ViewState["Bind"] != null)
                ViewState["Bind"] = null;

            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDINVOICENUMBER", "FLDINVOICEDATE", "FLDNAME", "FLDPASSENGERNAME", "FLDDEPARTUREDATE", "FLDVESSELNAME", "FLDTICKETNO", "FLDTOTAL", "FLDSTATUSNAME", "FLDCREATEDDATE", "FLDACCOUNTVOUCHERNUMBER", "FLDPAIDDATE" };
            string[] alCaptions = { "Invoice No", "Invoice Date", "Agent", "Passenger", "Departure", "Vessel", "Ticket", "Total Charges", "Status", "Imported Date", "Accounts Voucher No", "Paid Date" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            DataSet ds = new DataSet();

            if (Filter.CurrentTravelInvoiceFilter != null)
                nvc = Filter.CurrentTravelInvoiceFilter;

            ds = PhoenixCrewTravelInvoice.CrewTravelAgentInvoiceOfficeSearch(nvc != null ? nvc.Get("INVOICENO") : string.Empty
                , General.GetNullableString(nvc != null ? nvc.Get("txtInvoiceNumber") : string.Empty)
                , General.GetNullableDateTime(nvc != null ? nvc.Get("txtstartdate") : string.Empty)
                , General.GetNullableDateTime(nvc != null ? nvc.Get("txtenddate") : string.Empty)
                , General.GetNullableInteger(nvc != null ? nvc.Get("ddlvessel") : string.Empty)
                , General.GetNullableInteger(nvc != null ? nvc.Get("ucInvoiceStatus") : string.Empty)
                , General.GetNullableInteger(nvc != null ? nvc.Get("txtAgentId") : string.Empty)
                , General.GetNullableString(nvc != null ? nvc.Get("txtTicket") : string.Empty)
                , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDepDate") : string.Empty)
                , General.GetNullableString(nvc != null ? nvc.Get("txtName") : string.Empty)
                , null
                , sortexpression
                , sortdirection
                , (int)ViewState["PAGENUMBER"]
                , gvCCT.PageSize
                , ref iRowCount
                , ref iTotalPageCount);

            General.SetPrintOptions("gvCCT", "Travel Invoice", alCaptions, alColumns, ds);

            gvCCT.DataSource = ds;
            gvCCT.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidInvoiceDate()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(txtstartdate.Text) == null)
            ucError.ErrorMessage = "Invoice From date required";

        if (General.GetNullableDateTime(txtenddate.Text) == null)
            ucError.ErrorMessage = "Invoice To date required";

        if (General.GetNullableInteger(txtAgentId.Text) == null)
            ucError.ErrorMessage = "Agent is Required";

        return (!ucError.IsError);
    }
  
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvCCT.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindFilterData()
    {
        try
        {
            nvc = Filter.CurrentTravelInvoiceFilter;

            ddlvessel.SelectedVessel = nvc != null ? (nvc.Get("ddlvessel").ToUpper() == "DUMMY" ? "" : nvc.Get("ddlvessel")) : "DUMMY";
            txtstartdate.Text = nvc != null ? nvc.Get("txtstartdate") : "";
            txtenddate.Text = nvc != null ? nvc.Get("txtenddate") : "";
            txtInvoiceNumber.Text = nvc != null ? nvc.Get("txtInvoiceNumber") : "";
            txtAgentName.Text = nvc != null ? nvc.Get("txtAgentName") : "";
            txtAgentNumber.Text = nvc != null ? nvc.Get("txtAgentNumber") : "";
            txtAgentId.Text = nvc != null ? nvc.Get("txtAgentId") : "";
            txtTicket.Text = nvc != null ? nvc.Get("txtTicket") : "";
            txtDepDate.Text = nvc != null ? nvc.Get("txtDepDate") : "";
            txtName.Text = nvc != null ? nvc.Get("txtName") : "";
            ucInvoiceStatus.SelectedHard = nvc != null ? (nvc.Get("ucInvoiceStatus").ToUpper() == "DUMMY" ? "" : nvc.Get("ucInvoiceStatus")) : "DUMMY";

            ViewState["INVOICENO"] = nvc != null ? (nvc.Get("INVOICENO") == null ? null : nvc.Get("INVOICENO").ToString()) : null;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ClearFilter()
    {
        Filter.CurrentTravelInvoiceFilter = null;
        nvc.Clear();
        ddlvessel.SelectedVessel = "";
        txtstartdate.Text = "";
        txtenddate.Text = "";
        txtInvoiceNumber.Text = "";
        txtAgentId.Text = "";
        txtAgentName.Text = "";
        txtAgentNumber.Text = "";
        txtTicket.Text = "";
        txtDepDate.Text = "";
        txtName.Text = "";
        ucInvoiceStatus.SelectedHard = "";
        string pen = PhoenixCommonRegisters.GetHardCode(
                                              PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                              200, "PEN");
        ViewState["INVOICENO"] = null;
        ViewState["PAGENUMBER"] = 1;

    }
    private void SetFilter()
    {
        try
        {
            nvc.Clear();
            nvc.Add("ddlvessel", ddlvessel.SelectedVessel);
            nvc.Add("txtstartdate", txtstartdate.Text);
            nvc.Add("txtenddate", txtenddate.Text);
            nvc.Add("txtAgentId", txtAgentId.Text);
            nvc.Add("txtName", txtName.Text.Trim());
            nvc.Add("txtInvoiceNumber", txtInvoiceNumber.Text.Trim());
            nvc.Add("ucInvoiceStatus", ucInvoiceStatus.SelectedHard);
            nvc.Add("txtAgentName", txtAgentName.Text);
            nvc.Add("txtAgentNumber", txtAgentNumber.Text);
            nvc.Add("txtTicket", txtTicket.Text.Trim());
            nvc.Add("txtDepDate", txtDepDate.Text);
            nvc.Add("INVOICENO", ViewState["INVOICENO"] == null ? null : ViewState["INVOICENO"].ToString());

            Filter.CurrentTravelInvoiceFilter = nvc;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCCT_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCCT.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvCCT_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT")
                return;

           
            if (e.CommandName.ToUpper() == "INVOICESEARCH")
            {
                RadLabel lblInvNO = (RadLabel)e.Item.FindControl("lblInvNo");
                if (lblInvNO != null)
                    ViewState["INVOICENO"] = lblInvNO.Text;
                SetFilter();

                gvCCT.CurrentPageIndex = 0;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvCCT.Rebind();

            }
            if (e.CommandName.ToUpper() == "CLEARFILTER")
            {
                ViewState["INVOICENO"] = null;
                SetFilter();

                gvCCT.CurrentPageIndex = 0;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvCCT.Rebind();
            }

            if (e.CommandName.ToUpper() == "REPOST")
            {
                RadLabel lblInvId = (RadLabel)e.Item.FindControl("lblAgentInvoiceid");
                PhoenixCrewTravelInvoice.TravelInvoiceRePost(new Guid(lblInvId.Text), PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                
                BindData();
                gvCCT.Rebind();
            }
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

    protected void gvCCT_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if(e.Item is GridDataItem)
        {
            LinkButton Filter = (LinkButton)e.Item.FindControl("cmdFilter");
            LinkButton Clearfilter = (LinkButton)e.Item.FindControl("cmdClearFilter");
            RadLabel lblInvno = (RadLabel)e.Item.FindControl("lblInvNo");

            if (Filter != null && Clearfilter != null)
            {
                if (ViewState["INVOICENO"] != null)
                {
                    if (lblInvno != null && lblInvno.Text.Equals(ViewState["INVOICENO"].ToString()))
                    {
                        Filter.Visible = false;
                        Clearfilter.Visible = true;
                    }
                }
                else
                {
                    Filter.Visible = true;
                    Clearfilter.Visible = false;
                }
            }
            LinkButton cmdedit = (LinkButton)e.Item.FindControl("cmdedit");
            LinkButton lnkticket = (LinkButton)e.Item.FindControl("lnkTicket");
            RadLabel lblagentinvid = (RadLabel)e.Item.FindControl("lblAgentInvoiceid");

            if (cmdedit != null && lblagentinvid != null)
                cmdedit.Attributes.Add("onclick", "openNewWindow('Invoice','Invoice'," + "'" + Session["sitepath"] + "/Crew/CrewTravelAgentInvoiceGeneralInOffice.aspx?AGENTINVOICEID=" + lblagentinvid.Text + "'); return false;");

            if (lnkticket != null && lblagentinvid != null)
                lnkticket.Attributes.Add("onclick", "openNewWindow('Invoice','Invoice'," + "'" + Session["sitepath"] + "/Crew/CrewTravelAgentInvoiceGeneralInOffice.aspx?AGENTINVOICEID=" + lblagentinvid.Text + "'); return false;");

            LinkButton cmdReport = (LinkButton)e.Item.FindControl("cmdReport");
            if (cmdReport != null)
            {
                cmdReport.Attributes.Add("onclick", "openNewWindow('codehelp','','" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=4&reportcode=TRAVELINVOICE&showmenu=false&showexcel=no&AgentInvoiceId=" + lblagentinvid.Text.ToString() + "');return true;");
                cmdReport.Visible = SessionUtil.CanAccess(this.ViewState, cmdReport.CommandName);
            }            
        }
    }
    
}