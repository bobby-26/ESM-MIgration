using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Text;
using Telerik.Web.UI;
public partial class CrewTravelAgentInvoiceExceptionList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewTravelAgentInvoiceExceptionList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCCT')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Crew/CrewTravelAgentInvoiceExceptionList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenutravelList.AccessRights = this.ViewState;
            MenutravelList.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);
                cmdHiddenSubmit.Attributes.Add("style", "display:none");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvCCT.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;

                gvCCT.CurrentPageIndex = 0;
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

    protected void chkarchievedyn_OnCheckedChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;

        gvCCT.CurrentPageIndex = 0;
        BindData();
        gvCCT.Rebind();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDINVOICENUMBER", "FLDINVOICEDATE", "FLDPNRNO", "FLDPASSENGERNAME", "FLDDEPARTUREDATE", "FLDVESSELNAME", "FLDTICKETNO", "FLDTOTAL", "FLDSTATUSNAME", "FLDCREATEDDATE" };
        string[] alCaptions = { "Invoice Number", "Invoice Date", "PNR", "Passenger Name", "Dep Date", "Vessel", "Ticket", "Total Charges", "Status", "Imported Date" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewTravelInvoice.CrewTravelAgentInvoiceExceptionSearch(
                    Convert.ToInt32(chkarchievedyn.Checked== true ? "1" : "0")
                    , ViewState["INVOICENO"] == null ? null : ViewState["INVOICENO"].ToString()
                    , General.GetNullableDateTime(txtstartdate.Text)
                    , General.GetNullableDateTime(txtenddate.Text)
                    , General.GetNullableString(txtvessel.Text.Trim())
                    , null
                    , General.GetNullableString(txtRequestNo.Text.Trim())
                    , General.GetNullableString(txtPassengerName.Text.Trim())
                    , General.GetNullableString(txtTicketNo.Text.Trim())
                    , sortexpression, sortdirection
                    , (int)ViewState["PAGENUMBER"]
                    , gvCCT.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount);

        if (ds.Tables.Count > 0)
            General.ShowExcel("Travel Invoice Exception List", ds.Tables[0], alColumns, alCaptions, null, null);
    }
    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDINVOICENUMBER", "FLDINVOICEDATE", "FLDPNRNO", "FLDPASSENGERNAME", "FLDDEPARTUREDATE", "FLDVESSELNAME", "FLDTICKETNO", "FLDTOTAL", "FLDSTATUSNAME", "FLDCREATEDDATE" };
            string[] alCaptions = { "Invoice Number", "Invoice Date", "PNR", "Passenger Name", "Dep Date", "Vessel", "Ticket", "Total Charges", "Status", "Imported Date" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            DataSet ds = new DataSet();

            ds = PhoenixCrewTravelInvoice.CrewTravelAgentInvoiceExceptionSearch(
                    int.Parse(chkarchievedyn.Checked == true ? "1" : "0")
                    , ViewState["INVOICENO"] == null ? null : ViewState["INVOICENO"].ToString()
                    , General.GetNullableDateTime(txtstartdate.Text)
                    , General.GetNullableDateTime(txtenddate.Text)
                    , General.GetNullableString(txtvessel.Text.Trim())
                    , null
                    , General.GetNullableString(txtRequestNo.Text.Trim())
                    , General.GetNullableString(txtPassengerName.Text.Trim())
                    , General.GetNullableString(txtTicketNo.Text.Trim())
                    , sortexpression, sortdirection
                    , (int)ViewState["PAGENUMBER"]
                    , gvCCT.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount);

            General.SetPrintOptions("gvCCT", "Travel Invoice Exception List", alCaptions, alColumns, ds);

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

    private void updateArchievestatus(Guid agentinvoiceid, int isarchieved)
    {
        PhoenixCrewTravelInvoice.AgentInvoicearchieveUpdate(agentinvoiceid, isarchieved);
    }


    protected void gvCCT_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            LinkButton lnkticket = (LinkButton)e.Item.FindControl("lnkTicket");
            RadLabel lblagentinvid = (RadLabel)e.Item.FindControl("lblAgentInvoiceid");

            if (lnkticket != null && lblagentinvid != null)
                lnkticket.Attributes.Add("onclick", "openNewWindow('Invoice','Invoice'," + "'" + Session["sitepath"] + "/Crew/CrewTravelAgentInvoiceGeneral.aspx?INVOICEID=" + lblagentinvid.Text + " &INVOICEBULK=0'); return false;");

        }
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

}
