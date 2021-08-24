using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewTravelAgentInvoiceExceptionListInOffice : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewTravelAgentInvoiceExceptionListInOffice.aspx?e=3", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCCT')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Crew/CrewTravelAgentInvoiceExceptionListInOffice.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Crew/CrewTravelAgentInvoiceExceptionListInOffice.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");

            MenutravelList.AccessRights = this.ViewState;
            MenutravelList.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                txtAgentId.Attributes.Add("style", "visibility:hidden");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                cmdShowAgent.Attributes.Add("onclick", "return showPickList('spnPickListAgent', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=135', true);");

                if (Filter.CurrentTravelInvoiceExceptionFilter != null)
                {
                    NameValueCollection nvc = Filter.CurrentTravelInvoiceExceptionFilter;

                    txtAgentId.Text = nvc.Get("txtAgentId").ToString();
                    txtAgentName.Text = nvc.Get("txtAgentName");
                    txtAgentNumber.Text = nvc.Get("txtAgentNumber");
                    ddlvessel.SelectedVessel = nvc.Get("ddlvessel");
                    txtName.Text = nvc.Get("txtName");
                    txtInvoiceNumber.Text = nvc.Get("txtInvoiceNumber");
                    txtstartdate.Text = nvc.Get("txtstartdate");
                    txtenddate.Text = nvc.Get("txtenddate");
                    chkarchievedyn.Checked = (nvc.Get("chkarchievedyn") == "1" ? true : false);
                    ViewState["PAGENUMBER"] = General.GetNullableInteger(nvc.Get("PAGENUMBER").ToString());
                    ViewState["SORTEXPRESSION"] = nvc.Get("SORTEXPRESSION").ToString();
                    ViewState["SORTDIRECTION"] = General.GetNullableInteger(nvc.Get("SORTDIRECTION").ToString());
                }
                else
                {
                    NameValueCollection criteria = new NameValueCollection();
                    criteria.Clear();
                    criteria.Add("txtAgentId", txtAgentId.Text);
                    criteria.Add("txtAgentName", txtAgentName.Text);
                    criteria.Add("txtAgentNumber", txtAgentNumber.Text);
                    criteria.Add("ddlvessel", ddlvessel.SelectedVessel);
                    criteria.Add("txtName", txtName.Text);
                    criteria.Add("txtInvoiceNumber", txtInvoiceNumber.Text);
                    criteria.Add("txtstartdate", txtstartdate.Text);
                    criteria.Add("txtenddate", txtenddate.Text);
                    criteria.Add("chkarchievedyn", (chkarchievedyn.Checked == true) ? "1" : "0");
                    criteria.Add("PAGENUMBER", ViewState["PAGENUMBER"] == null ? "1" : ViewState["PAGENUMBER"].ToString());
                    criteria.Add("SORTEXPRESSION", ViewState["SORTEXPRESSION"] == null ? "" : ViewState["SORTEXPRESSION"].ToString());
                    criteria.Add("SORTDIRECTION", ViewState["SORTDIRECTION"] == null ? "" : ViewState["SORTDIRECTION"].ToString());

                    Filter.CurrentTravelInvoiceExceptionFilter = criteria;
                }
                gvCCT.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();
            cmdShowAgent.Attributes.Add("onclick", "return showPickList('spnPickListAgent', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=135', true);");
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
                ViewState["PAGENUMBER"] = 1;
                clearAll();
                gvCCT.CurrentPageIndex = 0;
                BindData();
                gvCCT.Rebind();
            }
            else if (CommandName.ToUpper().Equals("FIND"))
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("txtAgentId", txtAgentId.Text);
                criteria.Add("txtAgentName", txtAgentName.Text);
                criteria.Add("txtAgentNumber", txtAgentNumber.Text);
                criteria.Add("ddlvessel", ddlvessel.SelectedVessel);
                criteria.Add("txtName", txtName.Text);
                criteria.Add("txtInvoiceNumber", txtInvoiceNumber.Text);
                criteria.Add("txtstartdate", txtstartdate.Text);
                criteria.Add("txtenddate", txtenddate.Text);
                criteria.Add("chkarchievedyn", (chkarchievedyn.Checked == true) ? "1" : "0");
                criteria.Add("PAGENUMBER", "1");
                criteria.Add("SORTEXPRESSION", ViewState["SORTEXPRESSION"] == null ? "" : ViewState["SORTEXPRESSION"].ToString());
                criteria.Add("SORTDIRECTION", ViewState["SORTDIRECTION"] == null ? "" : ViewState["SORTDIRECTION"].ToString());
                ViewState["PAGENUMBER"] = 1;
                Filter.CurrentTravelInvoiceExceptionFilter = criteria;
                if (!IsValidTravelDate())
                {
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
    private bool IsValidTravelDate()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(txtstartdate.Text) == null)
            ucError.ErrorMessage = "Travel start date required.";

        if (General.GetNullableDateTime(txtenddate.Text) == null)
            ucError.ErrorMessage = "Travel end date required.";

        return (!ucError.IsError);
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

        ds = PhoenixCrewTravelInvoice.CrewTravelAgentInvoiceExceptionOfficeSearch(
                int.Parse(chkarchievedyn.Checked == true ? "1" : "0")
                , General.GetNullableString(txtInvoiceNumber.Text)
                , General.GetNullableDateTime(txtstartdate.Text)
                , General.GetNullableDateTime(txtenddate.Text)
                , General.GetNullableInteger(ddlvessel.SelectedVessel)
                , General.GetNullableInteger(txtAgentId.Text)
                , General.GetNullableString(txtTicketNo.Text)
                , General.GetNullableString(txtName.Text)
                , sortexpression, sortdirection
                , (int)ViewState["PAGENUMBER"]
                , gvCCT.PageSize
                , ref iRowCount
                , ref iTotalPageCount
                                                                                    );
    
        General.ShowExcel("Travel Invoice Exception List", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

      
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

            NameValueCollection nvc = Filter.CurrentTravelInvoiceExceptionFilter;

            ds = PhoenixCrewTravelInvoice.CrewTravelAgentInvoiceExceptionOfficeSearch(
                int.Parse(chkarchievedyn.Checked == true ? "1" : "0")
              , General.GetNullableString(txtInvoiceNumber.Text)
              , General.GetNullableDateTime(txtstartdate.Text)
              , General.GetNullableDateTime(txtenddate.Text)
              , General.GetNullableInteger(ddlvessel.SelectedVessel)
              , General.GetNullableInteger(txtAgentId.Text)
              , General.GetNullableString(txtTicketNo.Text)
              , General.GetNullableString(txtName.Text)
              , General.GetNullableString(nvc.Get("SORTEXPRESSION").ToString())
              , General.GetNullableInteger(nvc.Get("SORTDIRECTION").ToString())
              , (int)ViewState["PAGENUMBER"]
              , gvCCT.PageSize
              , ref iRowCount
              , ref iTotalPageCount);

            General.SetPrintOptions("gvCCT", "Travel Invoice Exception List", alCaptions, alColumns, ds);

            gvCCT.DataSource = ds;
            gvCCT.VirtualItemCount = iRowCount;

            if (ds.Tables[1].Rows.Count > 0)
                ddlvessel.SelectedVessel = ds.Tables[1].Rows[0]["FLDSELECTEDVESSEL"].ToString();
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
    private void clearAll()
    {
        ddlvessel.SelectedVessel = "";
        txtstartdate.Text = "";
        txtenddate.Text = "";
        txtInvoiceNumber.Text = "";
        txtAgentName.Text = "";
        txtAgentNumber.Text = "";
        txtAgentId.Text = "";
        txtName.Text = "";
        chkarchievedyn.Checked = false;

        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();
        criteria.Add("txtAgentId", "");
        criteria.Add("txtAgentName", "");
        criteria.Add("txtAgentNumber", "");
        criteria.Add("ddlvessel", "");
        criteria.Add("txtName", "");
        criteria.Add("txtInvoiceNumber", "");
        criteria.Add("txtstartdate", "");
        criteria.Add("txtenddate", "");
        criteria.Add("chkarchievedyn", "0");
        criteria.Add("PAGENUMBER", "1");
        criteria.Add("SORTEXPRESSION", "");
        criteria.Add("SORTDIRECTION", "");
        ViewState["PAGENUMBER"] = 1;
        Filter.CurrentTravelInvoiceExceptionFilter = criteria;
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

    protected void chkarchievedyn_OnCheckedChanged(object sender, EventArgs e)
    {
        Filter.CurrentTravelInvoiceExceptionFilter.Set("chkarchievedyn", (chkarchievedyn.Checked == true) ? "1" : "0");
        BindData();
        gvCCT.Rebind();
    }

    protected void gvCCT_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT")
                return;

            else if (e.CommandName.ToUpper() == "ARCHIEVE")
            {
                RadLabel lblagentinvoiceid = (RadLabel)e.Item.FindControl("lblagentinvoiceid");
                updateArchievestatus(new Guid(lblagentinvoiceid.Text), 1);
                BindData();
                gvCCT.Rebind();
            }
            else if (e.CommandName.ToUpper() == "DEARCHIEVE")
            {
                RadLabel lblagentinvoiceid = (RadLabel)e.Item.FindControl("lblagentinvoiceid");
                updateArchievestatus(new Guid(lblagentinvoiceid.Text), 0);
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
        if (e.Item is GridEditableItem)
        {
            LinkButton archieve = (LinkButton)e.Item.FindControl("cmdarchieve");
            LinkButton dearchieve = (LinkButton)e.Item.FindControl("cmddearchieve");
            RadLabel fldisarchivedyn = (RadLabel)e.Item.FindControl("lblarchievedyn");

            if (archieve != null && dearchieve != null && fldisarchivedyn != null)
            {
                if (fldisarchivedyn.Text.Equals("1"))
                {
                    archieve.Visible = false;
                    dearchieve.Visible = true;
                }
            }
            else
            {
                archieve.Visible = true;
                dearchieve.Visible = false;
            }
            LinkButton lnkticket = (LinkButton)e.Item.FindControl("lnkTicket");
            RadLabel lblagentinvid = (RadLabel)e.Item.FindControl("lblagentinvoiceid");

            if (lnkticket != null && lblagentinvid != null)
                lnkticket.Attributes.Add("onclick", "javascript:openNewWindow('Invoice','Invoice'," + "'" + Session["sitepath"] + "/Crew/CrewTravelAgentInvoiceGeneralInOffice.aspx?AGENTINVOICEID=" + lblagentinvid.Text + "&fromexception=1'); return false;");
            if (archieve != null) archieve.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to archive'); return false;");
            if (dearchieve != null) dearchieve.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to de-archive'); return false;");

        }
    }

    protected void gvCCT_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCCT.CurrentPageIndex + 1;
        BindData();
    }
}
