using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using System.Text;
using System.Web.UI;
using Telerik.Web.UI;

public partial class PurchaseStrategySheet : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseStrategySheet.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvFormDetails')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseStrategySheet.aspx", "Search", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseStrategySheet.aspx", "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                if (Request.QueryString["orderid"] != null)
                {
                    ViewState["orderid"] = Request.QueryString["orderid"];
                }
                else
                    ViewState["orderid"] = "";

                ViewState["SelectedFleetList"] = "";
                ViewState["SelectedVesselList"] = "";
                ViewState["SelectedGroupList"] = "";
                BindVesselGroupList();
                BindVesselFleetList();
                BindVesselList();
                NameValueCollection nvc = Filter.CurrentSelectedPurchaseStrategySheet;
                if (nvc != null)
                {
                    ucFromDate.Text = nvc.Get("ucFromDate");
                    ucToDate.Text = nvc.Get("ucToDate");

                    string strgrouplist = nvc.Get("chkGroupList");
                    if (strgrouplist.Length > 0 && strgrouplist.ToCharArray()[0].ToString() != ",")
                        strgrouplist = "," + strgrouplist;

                    if (strgrouplist.Length > 0 && strgrouplist.ToCharArray()[strgrouplist.Length - 1].ToString() != ",")
                        strgrouplist = strgrouplist + ",";

                    ViewState["SelectedGroupList"] = strgrouplist;

                    foreach (ButtonListItem item in chkGroupList.Items)
                    {
                        if (strgrouplist.Contains("," + item.Value))
                        {
                            item.Selected = true;
                        }
                    }

                    string strfleetlist = nvc.Get("chkFleetList");
                    if (strfleetlist.Length > 0 && strfleetlist.ToCharArray()[0].ToString() != ",")
                        strfleetlist = "," + strfleetlist;

                    if (strfleetlist.Length > 0 && strfleetlist.ToCharArray()[strfleetlist.Length - 1].ToString() != ",")
                        strfleetlist = strfleetlist + ",";

                    ViewState["SelectedFleetList"] = strfleetlist;

                    foreach (ButtonListItem item in chkFleetList.Items)
                    {
                        if (strfleetlist.Contains(","+item.Value))
                        {
                            item.Selected = true;
                        }
                    }
                    string strvessellist = nvc.Get("chkVesselList");
                    
                    if (strvessellist.Length > 0 && strvessellist.ToCharArray()[0].ToString() != ",")
                        strvessellist = "," + strvessellist;

                    if (strvessellist.Length > 0 && strvessellist.ToCharArray()[strvessellist.Length-1].ToString() != ",")
                        strvessellist = strvessellist+",";

                    ViewState["SelectedVesselList"] = strvessellist;
                    foreach (ButtonListItem item in chkVesselList.Items)
                    {
                        if (strvessellist.Contains(","+item.Value+","))
                        {
                            item.Selected = true;
                        }
                    }
                    if (nvc.Get("cbShowNonBlank").Equals("1"))
                        cbShowNonBlank.Checked = true;

                    if (nvc.Get("rbDatesValue") == "")
                        rbDates.SelectedIndex = 0;
                    else
                        rbDates.SelectedValue = nvc.Get("rbDatesValue").ToString();


                    txtDays.Text = nvc.Get("txtDays").ToString();

                    ddlStockType.SelectedValue = nvc.Get("ddlstocktype").ToString();

                    if (nvc.Get("txtForwaderId") != "")
                    {
                        txtVenderID.Text = nvc.Get("txtForwaderId").ToString();
                        txtVenderCode.Text = nvc.Get("txtSupplierCode").ToString();
                        txtVenderName.Text = nvc.Get("txtSupplierName").ToString();
                    }
                }

                PhoenixToolbar toolbar = new PhoenixToolbar();

                toolbar.AddButton("Comments", "COMMENTS",ToolBarDirection.Right);

                MenuOrderFormMain.AccessRights = this.ViewState;
                MenuOrderFormMain.MenuList = toolbar.Show();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    
    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("COMMENTS"))
            {
                Response.Redirect("../Purchase/PurchaseComments.aspx");
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
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDFORMNO", "FLDSTOCKTYPE", "FLDTITLE", "FLDIMPORTEDDATE","FLDACTIVEDATE", "FLDSENTDATE", "FLDDAYDIFFRECDANDACTIVE", "FLDRECEIVEDDATE", "FLDDAYDIFFRECDANDSENT", "FLDPURCHASEAPPROVEDATE", "FLDDAYDIFFRECDANDAPPROVED", "FLDORDEREDDATE", "FLDDAYDIFFORDERANDAPPROVED", "FLDFORWARDERNAME", "FLDVENDORDELIVERYDATE", "FLDSEAPORTNAME", "FLDPRICE", "FLDREMARKS" };
        string[] alCaptions = { "Req. No.", "Stock Type", "Form Title", "Received On Phoenix", "Active On Phoenix", "Sent for Quote", "Days", "Quotation Received", "Days", "Approved Date", "Days", "PO Issued", "Days", "Forwarder", "Received Onboard", "Delivery Port", "Total Amount (USD)", "Remarks" };
        
        int? rbDatesValue = null;
        int? isnonblank = null;
        int? sentforquote = null;
        int? qtnrecd = null;
        int? poapprd = null;
        int? poissued = null;
        int? recdonboard = null;
        int? sentforquotedays = null;
        int? qtnrecddays = null;
        int? poapprddays = null;
        int? poisseddays = null;

        isnonblank = cbShowNonBlank.Checked == true ? 1 : 0;
        rbDatesValue = General.GetNullableInteger(rbDates.SelectedValue);

        if (rbDatesValue != null)
        {
            switch (rbDatesValue)
            {
                case 1:
                    sentforquote = isnonblank;
                    sentforquotedays = General.GetNullableInteger(txtDays.Text);
                    break;
                case 2:
                    sentforquote = 1;
                    qtnrecd = isnonblank;
                    qtnrecddays = General.GetNullableInteger(txtDays.Text);
                    break;
                case 3:
                    sentforquote = 1;
                    qtnrecd = 1;
                    poapprd = isnonblank;
                    poapprddays = General.GetNullableInteger(txtDays.Text);
                    break;
                case 4:
                    sentforquote = 1;
                    qtnrecd = 1;
                    poapprd = 1;
                    poissued = isnonblank;
                    poisseddays = General.GetNullableInteger(txtDays.Text);
                    break;
                case 5:
                    sentforquote = 1;
                    qtnrecd = 1;
                    poapprd = 1;
                    poissued = 1;
                    recdonboard = isnonblank;
                    break;
            }
        }

        ds = PhoenixPurchaseOrderForm.PurchaseStrategySheetList(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , ViewState["SelectedVesselList"].ToString() // vesselid
            , General.GetNullableDateTime(ucFromDate.Text) // created from
            , General.GetNullableDateTime(ucToDate.Text) // created to
            , sentforquote // send date nb
            , qtnrecd // qtn recd nb
            , poapprd // apprd nb
            , poissued // po issued nb
            , recdonboard // recd nb
            , sentforquotedays // qtn sent vs req recd
            , qtnrecddays // qtn recd vs sent
            , poapprddays // apprd vs qtn recd
            , poisseddays // po issued vs apprd
            , General.GetNullableString(ddlStockType.SelectedValue)
            , General.GetNullableInteger(txtVenderID.Text)); 

        Response.AddHeader("Content-Disposition", "attachment; filename=PurchaseStrategySheet.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>Purchase Strategy Sheet" + "</center></h3></td>");
        //Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
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

    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                if (!IsValidFilter())
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ViewState["PAGENUMBER"] = 1;
                    BindData();
                    gvFormDetails.Rebind();
                }
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                foreach (ButtonListItem item in chkVesselList.Items)
                {
                    item.Selected = false;
                }
                foreach (ButtonListItem item in chkFleetList.Items)
                {
                   item.Selected = false;
                }
                foreach (ButtonListItem item in chkGroupList.Items)
                {
                    item.Selected = false;
                }
                ucFromDate.Text = "";
                ucToDate.Text = "";
                cbShowNonBlank.Checked = false;
                rbDates.SelectedIndex = 0;
                txtDays.Text = "";
                ddlStockType.SelectedValue = "";
                txtVenderID.Text = "";
                txtVenderCode.Text = "";
                txtVenderName.Text = "";
                ViewState["SelectedFleetList"] = "";
                ViewState["SelectedVesselList"] = "";
                ViewState["SelectedGroupList"] = "";
                BindData();
                gvFormDetails.Rebind();
                Filter.CurrentSelectedPurchaseStrategySheet = null;
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
        string[] alColumns = { "FLDFORMNO", "FLDSTOCKTYPE", "FLDTITLE", "FLDIMPORTEDDATE","FLDACTIVEDATE", "FLDSENTDATE", "FLDDAYDIFFRECDANDACTIVE", "FLDRECEIVEDDATE", "FLDDAYDIFFRECDANDSENT", "FLDPURCHASEAPPROVEDATE", "FLDDAYDIFFRECDANDAPPROVED", "FLDORDEREDDATE", "FLDDAYDIFFORDERANDAPPROVED", "FLDFORWARDERNAME", "FLDVENDORDELIVERYDATE", "FLDSEAPORTNAME", "FLDPRICE", "FLDREMARKS"  };
        string[] alCaptions = { "Req. No.", "Stock Type", "Form Title", "Received On Phoenix","Active on Phoenix", "Sent for Quote", "Days", "Quotation Received", "Days", "Approved Date", "Days", "PO Issued", "Days", "Forwarder", "Received Onboard", "Delivery Port", "Total Amount (USD)","Remarks" };
        
        DataSet ds = new DataSet();

        int? rbDatesValue = null;
        int? isnonblank = null;
        int? sentforquote = null;
        int? qtnrecd = null;
        int? poapprd = null;
        int? poissued = null;
        int? recdonboard = null;
        int? sentforquotedays = null;
        int? qtnrecddays = null;
        int? poapprddays = null;
        int? poisseddays = null;

        isnonblank = cbShowNonBlank.Checked == true ? 1 : 0;
        rbDatesValue = General.GetNullableInteger(rbDates.SelectedValue);

        if (rbDatesValue != null)
        {
            switch (rbDatesValue)
            {
                case 1:
                    sentforquote = isnonblank;
                    sentforquotedays = General.GetNullableInteger(txtDays.Text);
                    break;
                case 2:
                    sentforquote = 1;
                    qtnrecd = isnonblank;
                    qtnrecddays = General.GetNullableInteger(txtDays.Text);
                    break;
                case 3:
                    sentforquote = 1;
                    qtnrecd = 1;
                    poapprd = isnonblank;
                    poapprddays = General.GetNullableInteger(txtDays.Text);
                    break;
                case 4:
                    sentforquote = 1;
                    qtnrecd = 1;
                    poapprd = 1;
                    poissued = isnonblank;
                    poisseddays = General.GetNullableInteger(txtDays.Text);
                    break;
                case 5:
                    sentforquote = 1;
                    qtnrecd = 1;
                    poapprd = 1;
                    poissued = 1;
                    recdonboard = isnonblank;
                    break;
            }
        }

        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();
        criteria.Add("ucFromDate", ucFromDate.Text);
        criteria.Add("ucToDate", ucToDate.Text);
        criteria.Add("chkFleetList", ViewState["SelectedFleetList"].ToString() == null ? "" : ViewState["SelectedFleetList"].ToString());
        criteria.Add("chkVesselList", ViewState["SelectedVesselList"].ToString());
        criteria.Add("cbShowNonBlank", isnonblank.ToString());
        criteria.Add("rbDatesValue", rbDates.SelectedValue);
        criteria.Add("txtDays", txtDays.Text);
        criteria.Add("ddlstocktype",ddlStockType.SelectedValue);
        criteria.Add("txtForwaderId",txtVenderID.Text);
        criteria.Add("txtSupplierCode", txtVenderCode.Text);
        criteria.Add("txtSupplierName", txtVenderName.Text);
        criteria.Add("chkGroupList", ViewState["SelectedGroupList"].ToString() == null ? "" : ViewState["SelectedGroupList"].ToString());

        Filter.CurrentSelectedPurchaseStrategySheet = criteria;

        ds = PhoenixPurchaseOrderForm.PurchaseStrategySheetList(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , ViewState["SelectedVesselList"].ToString() // vesselid
            , General.GetNullableDateTime(ucFromDate.Text) // created from
            , General.GetNullableDateTime(ucToDate.Text) // created to
            , sentforquote // send date nb
            , qtnrecd // qtn recd nb
            , poapprd // apprd nb
            , poissued // po issued nb
            , recdonboard // recd nb
            , sentforquotedays // qtn sent vs req recd
            , qtnrecddays // qtn recd vs sent
            , poapprddays // apprd vs qtn recd
            , poisseddays // po issued vs apprd
            , General.GetNullableString(ddlStockType.SelectedValue)
            , General.GetNullableInteger(txtVenderID.Text)); 

            gvFormDetails.DataSource = ds;


        DataTable dtReport = new DataTable();

        dtReport = PhoenixPurchaseOrderForm.PurchasePerformanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , ViewState["SelectedVesselList"].ToString() // vesselid
            , General.GetNullableDateTime(ucFromDate.Text) // created from
            , General.GetNullableDateTime(ucToDate.Text) // created to
            , sentforquote // send date nb
            , qtnrecd // qtn recd nb
            , poapprd // apprd nb
            , poissued // po issued nb
            , recdonboard // recd nb
            , sentforquotedays // qtn sent vs req recd
            , qtnrecddays // qtn recd vs sent
            , poapprddays // apprd vs qtn recd
            , poisseddays // po issued vs apprd
            , General.GetNullableString(ddlStockType.SelectedValue)
            , General.GetNullableInteger(txtVenderID.Text));

        if (dtReport.Rows.Count > 0)
        {
            txtNewCreated.Text = dtReport.Rows[0]["NEWCREATED"].ToString();
            txtOldpending.Text = dtReport.Rows[0]["OLDPENDING"].ToString();
            txtTotalPending.Text = dtReport.Rows[0]["TOTALPENDING"].ToString();
            txtTotalCleared.Text = dtReport.Rows[0]["TOTALCLEARED"].ToString();
        }

        General.SetPrintOptions("gvFormDetails", "Purchase Strategy Sheet", alCaptions, alColumns, ds);
    }

    private void BindVesselList()
    {
        DataSet ds = PhoenixRegistersVessel.ListVessel();
        chkVesselList.DataSource = ds;
        chkVesselList.DataBind();
    }

    private void BindVesselFleetList()
    {
        DataSet ds = PhoenixRegistersFleet.ListFleet();
        chkFleetList.DataSource = ds;
        chkFleetList.DataBind();
    }
    private void BindVesselGroupList()
    {
        DataSet ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 246); //246 Vessel Group hard type
        chkGroupList.DataSource = ds;
        chkGroupList.DataBind();
    }
    protected void ChkGroupList_Changed(object sender, EventArgs e)
    {
        StringBuilder strgrouplist = new StringBuilder();
        ViewState["SelectedFleetList"] = "";
        ViewState["SelectedVesselList"] = "";

        foreach (ButtonListItem item in chkFleetList.Items)
        {
            item.Selected = false;
        }

        foreach (ButtonListItem item in chkGroupList.Items)
        {
            if (item.Selected == true)
            {
                strgrouplist.Append(item.Value.ToString());
                strgrouplist.Append(",");
            }
        }
        if (strgrouplist.Length > 1)
        {
            strgrouplist.Remove(strgrouplist.Length - 1, 1);
        }
        if (strgrouplist.ToString().Contains("Dummy"))
        {
            strgrouplist = new StringBuilder();
            strgrouplist.Append("0");
        }
        if (strgrouplist.ToString() == null || strgrouplist.ToString() == "")
            strgrouplist.Append("-1");

        ViewState["SelectedGroupList"] = strgrouplist;

        DataSet ds = PhoenixRegistersVesselGroupMapping.VesselListByGroup(strgrouplist.ToString() == "0" ? null : strgrouplist.ToString());

        foreach (ButtonListItem item in chkVesselList.Items)
        {
            item.Selected = false;
        }
        if (ds.Tables[0].Rows.Count > 0)
        {
            string vesselid = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                vesselid = dr["FLDVESSELID"].ToString();
                foreach (ButtonListItem item in chkVesselList.Items)
                {
                    if (item.Value == vesselid && !item.Selected)
                    {
                        item.Selected = true;
                        ViewState["SelectedVesselList"] = ViewState["SelectedVesselList"].ToString() + ',' + item.Value.ToString();
                        break;
                    }
                }
            }
        }
    }
    protected void chkFleetList_Changed(object sender, EventArgs e)
    {
        StringBuilder strfleetlist = new StringBuilder();

        ViewState["SelectedVesselList"] = "";
        ViewState["SelectedGroupList"] = "";

        foreach (ButtonListItem item in chkGroupList.Items)
        {
            item.Selected = false;
        }

        foreach (ButtonListItem item in chkFleetList.Items)
        {
            if (item.Selected == true)
            {
                strfleetlist.Append(item.Value.ToString());
                strfleetlist.Append(",");
            }
        }
        if (strfleetlist.Length > 1)
        {
            strfleetlist.Remove(strfleetlist.Length - 1, 1);
        }
        if (strfleetlist.ToString().Contains("Dummy"))
        {
            strfleetlist = new StringBuilder();
            strfleetlist.Append("0");
        }
        if (strfleetlist.ToString() == null || strfleetlist.ToString() == "")
            strfleetlist.Append("-1");

        ViewState["SelectedFleetList"] = strfleetlist;

        DataSet ds = PhoenixRegistersVessel.ListFleetWiseVessel(null, null, null, null, strfleetlist.ToString() == "0" ? null : strfleetlist.ToString());

        foreach (ButtonListItem item in chkVesselList.Items)
        {
            item.Selected = false;
        }
        if (ds.Tables[0].Rows.Count > 0)
        {
            string vesselid = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                vesselid = dr["FLDVESSELID"].ToString();
                foreach (ButtonListItem item in chkVesselList.Items)
                {
                    if (item.Value == vesselid && !item.Selected)
                    {
                        item.Selected = true;
                        ViewState["SelectedVesselList"] = ViewState["SelectedVesselList"].ToString() + ',' + item.Value.ToString();
                        break;
                    }
                }
            }
        }
    }

    protected void chkVesselList_Changed(object sender, EventArgs e)
    {
        ViewState["SelectedVesselList"] = "";
        foreach (ButtonListItem item in chkVesselList.Items)
        {
            if (item.Selected == true && !ViewState["SelectedVesselList"].ToString().Contains("," + item.Value.ToString() + ","))
            {
                ViewState["SelectedVesselList"] = ViewState["SelectedVesselList"].ToString() + ',' + item.Value.ToString();
            }
            if (item.Selected == false && ViewState["SelectedVesselList"].ToString().Contains("," + item.Value.ToString() + ","))
            {
                ViewState["SelectedVesselList"] = ViewState["SelectedVesselList"].ToString().Replace("," + item.Value.ToString() + ",", ",");
            }
        }
    }

    public bool IsValidFilter()
    {
        DateTime resultdate;

        ucError.HeaderMessage = "Please provide the following required information";

        
        if (DateTime.TryParse(ucFromDate.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Date should be earlier than current date";
        }

        if (!string.IsNullOrEmpty(ucFromDate.Text) && string.IsNullOrEmpty(ucToDate.Text))
        {
            ucError.ErrorMessage = "To Date is Required.";
        }
        if (!string.IsNullOrEmpty(ucFromDate.Text)
            && DateTime.TryParse(ucToDate.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(ucFromDate.Text)) < 0)
        {
            ucError.ErrorMessage = "To Date should be later than 'From Date'";
        }

        if (ViewState["SelectedVesselList"].ToString().Equals(""))
            ucError.ErrorMessage = "Select atleast one Vessel.";

        return (!ucError.IsError);

    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }

   

    protected void cmdClear_Click(object sender, ImageClickEventArgs e)
    {
        txtVenderCode.Text = "";
        txtVenderID.Text = "";
        txtVenderName.Text = "";
    }

    protected void gvFormDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvFormDetails_ItemCommand(object sender, GridCommandEventArgs e)
    {
        GridDataItem item = (GridDataItem)e.Item;
        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            RadLabel lblVesselId = (RadLabel)item.FindControl("lblVesselId");
            LinkButton lnkFormNumberName = (LinkButton)item.FindControl("lnkFormNumberName");
            RadLabel lblFormTitle = (RadLabel)item.FindControl("lblFormTitle");
            RadLabel lblOrderid = (RadLabel)item.FindControl("lblOrderId");

            string subject = lnkFormNumberName.Text + " - " + lblFormTitle.Text;

            Response.Redirect("../Purchase/PurchaseComments.aspx?vesselid=" + lblVesselId.Text + "&subject=" + subject + "&orderid=" + lblOrderid.Text);
        }

        if (e.CommandName.ToUpper().Equals("FORM"))
        {
            RadLabel lblOrderId = ((RadLabel)item.FindControl("lblOrderId"));
            LinkButton lnkFormNumber = ((LinkButton)item.FindControl("lnkFormNumberName"));
            RadLabel lblStockType = ((RadLabel)item.FindControl("lblStockType"));
            RadLabel lblVesselId = ((RadLabel)item.FindControl("lblVesselId"));
            RadLabel lblVesselName = ((RadLabel)item.FindControl("lblVesselName"));

            NameValueCollection criteria = new NameValueCollection();

            PhoenixSecurityContext.CurrentSecurityContext.VesselID = int.Parse(lblVesselId.Text);
            Filter.CurrentPurchaseVesselSelection = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            PhoenixSecurityContext.CurrentSecurityContext.VesselName = lblVesselName.Text;

            string Script = "";
            Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
            Script += "setVessel();";
            Script += "</script>" + "\n";

            DataSet ds = PhoenixGeneralSettings.GetUserOptions(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            PhoenixGeneralSettings.CurrentGeneralSetting = new GeneralSetting(ds);

            RadScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", Script, false);

            criteria.Clear();
            criteria.Add("ucVessel", lblVesselId.Text);
            criteria.Add("ddlStockType", lblStockType.Text);
            criteria.Add("txtNumber", lnkFormNumber.Text);
            criteria.Add("txtTitle", "");
            criteria.Add("txtVendorid", "");
            criteria.Add("txtDeliveryLocationId", "");
            criteria.Add("txtBudgetId", "");
            criteria.Add("txtBudgetgroupId", "");
            criteria.Add("ucFinacialYear", "");
            criteria.Add("ucFormState", "");
            criteria.Add("ucApproval", "");
            criteria.Add("UCrecieptCondition", "");
            criteria.Add("UCPeority", "");
            criteria.Add("ucFormStatus", "");
            criteria.Add("ucFormType", "");
            criteria.Add("ucComponentclass", "");
            criteria.Add("txtMakerReference", "");
            criteria.Add("txtOrderedDate", "");
            criteria.Add("txtOrderedToDate", "");
            criteria.Add("txtCreatedDate", "");
            criteria.Add("txtCreatedToDate", "");
            criteria.Add("txtApprovedDate", "");
            criteria.Add("txtApprovedToDate", "");
            Filter.CurrentOrderFormFilterCriteria = criteria;

            LinkButton lnkform = (LinkButton)item.FindControl("lnkStockType");

            Response.Redirect("../Purchase/PurchaseForm.aspx?&orderid=" + lblOrderId.Text + "&launchedfrom=ANALYSIS");
        }
    }

    protected void gvFormDetails_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;

            RadLabel lblQtnSentVsReqRecd = (RadLabel)item.FindControl("lblQtnSentVsReqRecd");
            Int64 result = 0;

            if (lblQtnSentVsReqRecd != null && Int64.TryParse(lblQtnSentVsReqRecd.Text, out result))
            {
                item.Cells[5].ForeColor = (result > 2) ? System.Drawing.Color.Red : System.Drawing.Color.Black;
            }

            RadLabel lblQtnRecdVsSent = (RadLabel)item.FindControl("lblQtnRecdVsSent");
            if (lblQtnRecdVsSent != null && Int64.TryParse(lblQtnRecdVsSent.Text, out result))
            {
                item.Cells[7].ForeColor = (result > 2) ? System.Drawing.Color.Red : System.Drawing.Color.Black;
            }

            RadLabel lblApprdVsQtnRecd = (RadLabel)item.FindControl("lblApprdVsQtnRecd");
            if (lblApprdVsQtnRecd != null && Int64.TryParse(lblApprdVsQtnRecd.Text, out result))
            {
                item.Cells[9].ForeColor = (result > 2) ? System.Drawing.Color.Red : System.Drawing.Color.Black;
            }

            RadLabel lblPOIssuedVsApprd = (RadLabel)item.FindControl("lblPOIssuedVsApprd");
            if (lblPOIssuedVsApprd != null && Int64.TryParse(lblPOIssuedVsApprd.Text, out result))
            {
                item.Cells[11].ForeColor = (result > 2) ? System.Drawing.Color.Red : System.Drawing.Color.Black;
            }
        }
    }

    protected void gvFormDetails_SortCommand(object sender, GridSortCommandEventArgs e)
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
    }
}
