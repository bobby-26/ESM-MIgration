using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;


public partial class PurchaseForm : PhoenixBasePage
{
    string vesselname;

    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetRowSelection();
        SetTabHighlight();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                if (Request.QueryString["dfilter"] != null)
                {
                    Filter.CurrentPurchaseDashboardCode = Request.QueryString["dfilter"].ToString();
                    Filter.CurrentPurchaseOwnerReportCode = null;
                }
                if (Request.QueryString["mfilter"] != null)
                {
                    Filter.CurrentPurchaseOwnerReportCode = Request.QueryString["mfilter"].ToString();
                    Filter.CurrentPurchaseDashboardCode = null;
                }
                else
                {
                    Filter.SelectedOwnersReportVessel = "0";
                }

                VesselConfiguration();
                Filter.CurrentPurchaseOrderIdSelection = null;
                rgvForm.PageSize = General.ShowRecords(rgvForm.PageSize);
                if (Request.QueryString["pageno"] != null && General.GetNullableInteger(Request.QueryString["pageno"].ToString()) != null)
                    rgvForm.CurrentPageIndex = int.Parse(Request.QueryString["pageno"].ToString());



                if (Request.QueryString["vslid"] != null)
                    ViewState["dvslid"] = Request.QueryString["vslid"].ToString();
                else
                    ViewState["dvslid"] = "";
            }

            if (Request.QueryString["launchedfrom"] != null)
            {
                if (Request.QueryString["launchedfrom"].ToString().ToUpper() == "ACCOUNTS")
                {
                    Session["launchedfrom"] = "ACCOUNTS";
                }
                else if (Request.QueryString["launchedfrom"].ToString().ToUpper() == "PURCHASE")
                {
                    Session["launchedfrom"] = "PURCHASE";
                }
                else if (Request.QueryString["launchedfrom"].ToString().ToUpper() == "ANALYSIS")
                {
                    Session["launchedfrom"] = "ANALYSIS";
                }
            }

            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            copy.Attributes.Add("style", "display:none");
            lblorderId.Attributes.Add("style", "display:none");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            if (Request.QueryString["orderid"] != null)
            {
                if (Filter.CurrentVesselConfiguration == null || Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                {
                    toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseForm.aspx?orderid=" + Request.QueryString["orderid"].ToString(), "Export to Excel with Amount", "<i class=\"fas fa-file-excel\"></i>", "EXDETAILS");
                }
                toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseForm.aspx?orderid=" + Request.QueryString["orderid"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            }
            else
            {
                if (Filter.CurrentVesselConfiguration == null || Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                {
                    toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseForm.aspx", "Export to Excel with Amount", "<i class=\"fas fa-file-excel\"></i>", "EXDETAILS");
                }
                toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseForm.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            }
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('rgvForm')", "Print", "<i class=\"fas fa-print\"></i>", "PRINT");

            if (Session["launchedfrom"] != null && Session["launchedfrom"].ToString().ToUpper() == "PURCHASE")
            {
                toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseFormFilter.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
                //toolbargrid.AddImageButton("", "Clear Filter", "clear-filter.png", "CLEAR");
            }
            else if (Session["launchedfrom"] == null)
            {
                toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseFormFilter.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
                //toolbargrid.AddImageButton("", "Clear Filter", "clear-filter.png", "CLEAR");
            }

            if (Request.QueryString["orderid"] != null)
            {
                toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseForm.aspx?orderid=" + Request.QueryString["orderid"].ToString(), "Copy Requisition", "<i class=\"fas fa-copy\"></i>", "COPY");
            }
            else
            {
                toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseForm.aspx", "Copy Requisition", "<i class=\"fas fa-copy\"></i>", "COPY");
            }
            //toolbargrid.AddImageButton("../Purchase/PurchaseForm.aspx", "Show/Hide Form", "annexure.png", "SHOW", ToolBarDirection.Left);
            //toolbargrid.AddImageLink("javascript:Openpopup('Filter','','../Purchase/PurchaseFormCopy.aspx?orderid='+ document.getElementById('" + lblorderId.UniqueID + "').value );return false;", "Copy", "Copy.png", "");
            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
            //MenuOrderForm.SetTrigger(pnlOrderForm);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Vessel Form", "FORM", ToolBarDirection.Left); //Bugid 39009
            toolbarmain.AddButton("Details", "DETAILS", ToolBarDirection.Left);
            toolbarmain.AddButton("Reason", "REASON", ToolBarDirection.Left);
            toolbarmain.AddButton("Attachment", "ATTACHMENT", ToolBarDirection.Left);
            toolbarmain.AddButton("History", "HISTORY", ToolBarDirection.Left);
            toolbarmain.AddButton("Line Items", "LINEITEM", ToolBarDirection.Left);
            if (Filter.CurrentVesselConfiguration == null || Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                toolbarmain.AddButton("Vendor Quotations", "QUOTATIONS", ToolBarDirection.Left); //Bugid 39009
            //toolbarmain.AddButton("Delivery", "DELIVERY");
            toolbarmain.AddButton("Received", "RECEIVED", ToolBarDirection.Left);
            if (Filter.CurrentVesselConfiguration == null || Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                toolbarmain.AddButton("Comments", "COMMENTS", ToolBarDirection.Left);
            if (Session["launchedfrom"] != null && Session["launchedfrom"].ToString().ToUpper() == "ANALYSIS")
                toolbarmain.AddButton("Purchase Analysis", "BACK", ToolBarDirection.Right);
            //if (Filter.CurrentPurchaseDashboardCode != null && PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentOrderFormFilterCriteria == null)
            //    toolbarmain.AddButton("Dashboard", "DASHBOARD", ToolBarDirection.Right);

            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                if (PhoenixGeneralSettings.CurrentGeneralSetting == null)
                    Response.Redirect("PhoenixLogout.aspx");
                ucConfirm.Visible = false;
                //VesselConfiguration();
                Session["New"] = "N";

                //MenuOrderFormMain.SetTrigger(pnlOrderForm);
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["orderid"] = null;
                ViewState["quotationid"] = null;
                ViewState["PAGEURL"] = null;

                if (Request.QueryString["orderid"] != null)
                {
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseFormGeneral.aspx?orderid=" + Request.QueryString["orderid"].ToString();
                    //generalPane.ContentUrl = "../Purchase/PurchaseFormGeneral.aspx?orderid=" + Request.QueryString["orderid"].ToString();
                    lblorderId.Text = ViewState["orderid"].ToString();
                }

                if (Request.QueryString["quotationid"] != null)
                {
                    ViewState["quotationid"] = Request.QueryString["quotationid"].ToString();
                }

                MenuOrderFormMain.SelectedMenuIndex = 0;
                //if (Filter.currentPurchaseIframeStatus != null && Filter.currentPurchaseIframeStatus == "HIDE")
                //{
                //    ifMoreInfo.Visible = false;
                //}
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
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FORM"))
            {
                ViewState["PAGEURL"] = "../Purchase/PurchaseFormGeneral.aspx";
                //lnkShowHide.Text = "Show/Hide Form";
            }
            else if (CommandName.ToUpper().Equals("VENDORS"))
            {
                ViewState["PAGEURL"] = "../Purchase/PurchaseQuotedVendorList.aspx";
            }
            else if (CommandName.ToUpper().Equals("DETAILS"))
            {
                ViewState["PAGEURL"] = "../Purchase/PurchaseFormDetail.aspx";
                //lnkShowHide.Text = "Show/Hide Details";
            }
            else if (CommandName.ToUpper().Equals("REASON"))
            {
                ViewState["PAGEURL"] = "../Purchase/PurchaseReasonForRequisition.aspx";
                //lnkShowHide.Text = "Show/Hide Reason";
            }
            else if (CommandName.ToUpper().Equals("HISTORY"))
            {
                ViewState["PAGEURL"] = "../Purchase/PurchaseFormTransactionHistory.aspx";
                //lnkShowHide.Text = "Show/Hide History";
            }
            else if (CommandName.ToUpper().Equals("LINEITEM"))
            {
                if (ViewState["orderid"] == null)
                    Response.Redirect("../Purchase/PurchaseFormItemDetails.aspx?pageno=" + rgvForm.CurrentPageIndex);
                else
                    Response.Redirect("../Purchase/PurchaseFormItemDetails.aspx?orderid=" + ViewState["orderid"].ToString() + "&pageno=" + rgvForm.CurrentPageIndex + "&COMPONENTID=" + ViewState["COMPONENTID"]);
            }
            else if (CommandName.ToUpper().Equals("QUOTATIONS"))
            {
                if (Request.QueryString["orderid"] != null && Request.QueryString["orderid"].ToString() != ViewState["orderid"].ToString())
                {
                    ViewState["quotationid"] = null;
                }

                if (ViewState["orderid"] != null)
                {
                    if (ViewState["quotationid"] != null)
                        Response.Redirect("../Purchase/PurchaseQuotationVendorDetail.aspx?quotationid=" + ViewState["quotationid"].ToString() + "&orderid=" + ViewState["orderid"].ToString() + "&pageno=" + rgvForm.CurrentPageIndex.ToString());
                    else
                        Response.Redirect("../Purchase/PurchaseQuotationVendorDetail.aspx?orderid=" + ViewState["orderid"].ToString() + "&pageno=" + rgvForm.CurrentPageIndex.ToString());
                }
                else
                {
                    Response.Redirect("../Purchase/PurchaseQuotationVendorDetail.aspx?pageno=" + rgvForm.CurrentPageIndex.ToString());
                }
            }
            else if (CommandName.ToUpper().Equals("DELIVERY"))
            {
                DataSet ds = new DataSet();
                ds = PhoenixPurchaseOrderForm.EditOrderForm(new Guid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection);

                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["FLDFORMTYPE"].ToString() == "60" && ds.Tables[0].Rows[0]["FLDPURCHASEAPPROVEDBY"].ToString() != "")
                {
                    if (ViewState["orderid"] == null)
                        Response.Redirect("../Purchase/PurchaseDeliveryDetail.aspx");
                    else
                        Response.Redirect("../Purchase/PurchaseDeliveryDetail.aspx?orderid=" + ViewState["orderid"].ToString() + "&pageno=" + rgvForm.CurrentPageIndex);
                }
                else
                {
                    ucError.ErrorMessage = "You cannot deliver the items, before processing the Purchase Order";
                    ucError.Visible = true;
                    return;
                }
            }
            else if (CommandName.ToUpper().Equals("RECEIVED"))
            {
                DataSet ds = new DataSet();
                ds = PhoenixPurchaseOrderForm.EditOrderForm(new Guid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection);
                if (ds.Tables[0].Rows.Count > 0 && (ds.Tables[0].Rows[0]["FLDSTOCKCLASSID"].ToString() == "411" || ds.Tables[0].Rows[0]["FLDSTOCKCLASSID"].ToString() == "412"))
                {
                    ucError.ErrorMessage = "You cannot receive the items for the store type Bond and Provision";
                    ucError.Visible = true;
                    return;
                }
                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["FLDPURCHASEAPPROVEDBY"].ToString() != "" && ds.Tables[0].Rows[0]["FLDORDEREDDATE"].ToString() != "")
                {
                    if (ViewState["orderid"] == null)
                        Response.Redirect("../Purchase/PurchaseFormReceivedItem.aspx?pageno=" + rgvForm.CurrentPageIndex);
                    else
                        Response.Redirect("../Purchase/PurchaseFormReceivedItem.aspx?orderid=" + ViewState["orderid"].ToString() + "&pageno=" + rgvForm.CurrentPageIndex);
                }
                else
                {
                    ucError.ErrorMessage = "You cannot receive the items, Before processing the Purchase Order";
                    ucError.Visible = true;
                    return;
                }
            }
            else if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                //ViewState["PAGEURL"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.PURCHASE;

                Response.Redirect("../Purchase/PurchaseAttachments.aspx?orderid=" + ViewState["orderid"].ToString() + "&DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.PURCHASE + "&pageno=" + rgvForm.CurrentPageIndex);
            }


            if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.PURCHASE;
                //generalPane.ContentUrl= "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.PURCHASE;
            }
            else
            {
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?orderid=" + ViewState["orderid"];
                //generalPane.ContentUrl= ViewState["PAGEURL"].ToString() + "?orderid=" + ViewState["orderid"];
            }

            if (CommandName.ToUpper().Equals("COMMENTS"))
            {
                if (ViewState["orderid"] == null)
                    Response.Redirect("../Purchase/PurchaseOrderFormComments.aspx?pageno=" + rgvForm.CurrentPageIndex);
                else
                    Response.Redirect("../Purchase/PurchaseOrderFormComments.aspx?orderid=" + ViewState["orderid"].ToString() + "&pageno=" + rgvForm.CurrentPageIndex);
            }
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Purchase/PurchaseStrategySheet.aspx?orderid=" + rgvForm.CurrentPageIndex.ToString());
            }
            if (CommandName.ToUpper().Equals("DASHBOARD"))
            {
                Response.Redirect("../Dashboard/DashboardVessel.aspx");
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
        int vesselid = -1;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDVESSELNAME","FLDFORMNO", "FLDTITLE", "FLDVENDORNAME", "FLDFORMTYPENAME", "FLDFORMSTATUSNAME", "FLDSUBACCOUNT", "FLDVENDORDELIVERYDATE", "FLDSTOCKTYPE", "FLDSTOCKCLASS", "FLDSTATUS", "FLDPOSTATUS", "FLDVESSELCODE", "FLDCURRENCY", "FLDBUDGETNAME", "FLDREASONFORREQUISITIONNAME", "FLDCREATEDDATE", "FLDPURCHASEAPPROVEDATE", "FLDORDEREDDATE" };
        string[] alCaptions = { "Vessel","Number", "Form Title", "Vendor", "Form Type", "Form Status", "Budget Code", "Received date", "Type", "Component Class</br>/Store Type", "Requisition Status", "PO Status", "Vessel", "Currency", "Budget", "Reason for Requisition", "Created Date", "Approved Date", "Ordered Date" };


        //string[] alColumns;
        //string[] alCaptions;
        //DataSet ds = new DataSet();

        //if (Filter.CurrentOrderFormFilterBySpareGroup !=null)
        //{
        //    alColumns = new string[] { "FLDFORMNO", "FLDTITLE", "FLDVESSELNAME", "FLDVENDORNAME", "FLDFORMTYPENAME", "FLDFORMSTATUSNAME", "FLDSUBACCOUNT", "FLDPURCHASEAPPROVEDATE", "FLDORDEREDDATE", "FLDVENDORDELIVERYDATE", "FLDCOMMITTEDUSD", "FLDSTOCKTYPE", "FLDSTATUS", "FLDPOSTATUS", "FLDREASONFORREQUISITIONNAME" };
        //  alCaptions = new string[] { "Number", "Form Title","Vessel","Vendor", "Form Type", "Form Status", "Budget Code", "Approved Date", "Ordered Date", "Received date", "Committed (USD)", "Type", "Requisition Status", "PO Status", "Reason for Requisition" };
        //}
        //else
        //{
        //  alColumns = new string[] { "FLDFORMNO", "FLDTITLE", "FLDVENDORNAME", "FLDFORMTYPENAME", "FLDFORMSTATUSNAME", "FLDSUBACCOUNT", "FLDOWNERBUDGETGROUP",  "FLDVENDORDELIVERYDATE", "FLDSTOCKTYPE", "FLDSTOCKCLASS", "FLDSTATUS","FLDPOSTATUS", "FLDVESSELCODE", "FLDCURRENCY", "FLDBUDGETNAME", "FLDREASONFORREQUISITIONNAME", "FLDCREATEDDATE", "FLDPURCHASEAPPROVEDATE", "FLDORDEREDDATE" };
        //  alCaptions= new string[] { "Number", "Form Title", "Vendor", "Form Type", "Form Status","Budget Code","Owner Budget Code","Received date","Type","Component Class</br>/Store Type","Requisition Status","PO Status","Vessel", "Currency", "Budget","Reason for Requisition","Created Date","Approved Date","Ordered Date" };
        //}
        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        if (Filter.CurrentPurchaseDashboardCode != null && PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentOrderFormFilterCriteria == null)
        {
            ds = PhoenixPurchaseOrderForm.OrderFormSearchDashboard(PhoenixSecurityContext.CurrentSecurityContext.VesselID, Filter.CurrentPurchaseDashboardCode
                                , sortexpression
                                , sortdirection
                                , rgvForm.CurrentPageIndex + 1
                                , rgvForm.VirtualItemCount
                                , ref iRowCount
                                , ref iTotalPageCount);
        }
        else if (Filter.CurrentPurchaseOwnerReportCode != null && int.Parse( Filter.SelectedOwnersReportVessel) > 0 && Filter.CurrentOrderFormFilterCriteria == null)
        {
            ds = PhoenixPurchaseOrderForm.OrderFormSearchOwnerReport(int.Parse(Filter.SelectedOwnersReportVessel), Filter.CurrentPurchaseOwnerReportCode
                                , DateTime.Parse(Filter.SelectedOwnersReportDate)
                                , sortexpression
                                , sortdirection
                                , rgvForm.CurrentPageIndex + 1
                                , rgvForm.VirtualItemCount
                                , ref iRowCount
                                , ref iTotalPageCount);
        }
        else if (Filter.CurrentPurchaseDashboardCode != null && PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
        {

            //NameValueCollection nvc = Filter.CurrentOrderFormFilterCriteria;
            //if (Filter.CurrentOrderFormFilterBySpareGroup != null)
            //{
            //    ds = PhoenixPurchaseOrderForm.OrderFormSearchByGroup(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            //          , General.GetNullableString(nvc.Get("ddlRequisitionStatus").ToString())
            //          , General.GetNullableString(nvc.Get("txtSpareGroupCode").ToString())
            //          , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], General.ShowRecords(null),
            //              ref iRowCount, ref iTotalPageCount);
            //}
            //else
            //{
            //vesselid = (General.GetNullableInteger(nvc.Get("ucVessel").ToString()) == null) ? -1 : int.Parse(nvc.Get("ucVessel").ToString());
            NameValueCollection nvc = new NameValueCollection();        //= FilterDashboard.OfficeDashboardFilterCriteria;
            nvc["VesselList"] = string.Empty;
            nvc["FleetList"] = string.Empty;
            nvc["Owner"] = string.Empty;
            nvc["VesselTypeList"] = string.Empty;
            nvc["RankList"] = string.Empty;

            if (General.GetNullableString(ViewState["dvslid"].ToString()) != null)
                nvc["VesselList"] = "," + ViewState["dvslid"].ToString() + ",";



            if (Filter.CurrentPurchaseDashboardCode == "POAWVR")
            {
                ds = PhoenixPurchaseFormDashBoard.DashBoardOrderFormSearchPOAWVR(General.GetNullableString(nvc["VesselList"].ToString()),
                       General.GetNullableString(nvc["VesselTypeList"].ToString()), General.GetNullableString(nvc["FleetList"].ToString()), General.GetNullableInteger(nvc["Owner"].ToString()),
                       sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                       ref iRowCount, ref iTotalPageCount);
            }
            else if (Filter.CurrentPurchaseDashboardCode == "RFQAP")
            {
                ds = PhoenixPurchaseFormDashBoard.DashBoardOrderFormSearchRFQAP(General.GetNullableString(nvc["VesselList"].ToString()),
                       General.GetNullableString(nvc["VesselTypeList"].ToString()), General.GetNullableString(nvc["FleetList"].ToString()), General.GetNullableInteger(nvc["Owner"].ToString()),
                       sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                       ref iRowCount, ref iTotalPageCount);
            }
            else if (Filter.CurrentPurchaseDashboardCode == "AQPPO")
            {
                ds = PhoenixPurchaseFormDashBoard.DashBoardOrderFormSearchAQPPO(General.GetNullableString(nvc["VesselList"].ToString()),
                       General.GetNullableString(nvc["VesselTypeList"].ToString()), General.GetNullableString(nvc["FleetList"].ToString()), General.GetNullableInteger(nvc["Owner"].ToString()),
                       sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                       ref iRowCount, ref iTotalPageCount);
            }
            else if (Filter.CurrentPurchaseDashboardCode == "POND")
            {
                ds = PhoenixPurchaseFormDashBoard.DashBoardOrderFormSearchPOND(General.GetNullableString(nvc["VesselList"].ToString()),
                       General.GetNullableString(nvc["VesselTypeList"].ToString()), General.GetNullableString(nvc["FleetList"].ToString()), General.GetNullableInteger(nvc["Owner"].ToString()),
                       sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                       ref iRowCount, ref iTotalPageCount);
            }
            else if (Filter.CurrentPurchaseDashboardCode == "QAA5K")
            {
                ds = PhoenixPurchaseFormDashBoard.DashBoardOrderFormSearchQAA5K(General.GetNullableString(nvc["VesselList"].ToString()),
                       General.GetNullableString(nvc["VesselTypeList"].ToString()), General.GetNullableString(nvc["FleetList"].ToString()), General.GetNullableInteger(nvc["Owner"].ToString()),
                       sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                       ref iRowCount, ref iTotalPageCount);
            }
            else if (Filter.CurrentPurchaseDashboardCode == "QAA10K")
            {
                ds = PhoenixPurchaseFormDashBoard.DashBoardOrderFormSearchQAA10K(General.GetNullableString(nvc["VesselList"].ToString()),
                       General.GetNullableString(nvc["VesselTypeList"].ToString()), General.GetNullableString(nvc["FleetList"].ToString()), General.GetNullableInteger(nvc["Owner"].ToString()),
                       sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                       ref iRowCount, ref iTotalPageCount);
            }
            else if (Filter.CurrentPurchaseDashboardCode == "QTNR")
            {
                ds = PhoenixPurchaseFormDashBoard.DashBoardOrderFormSearchQTNR(General.GetNullableString(nvc["VesselList"].ToString()),
                       General.GetNullableString(nvc["VesselTypeList"].ToString()), General.GetNullableString(nvc["FleetList"].ToString()), General.GetNullableInteger(nvc["Owner"].ToString()),
                       sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                       ref iRowCount, ref iTotalPageCount);
            }
            else if (Filter.CurrentPurchaseDashboardCode == "REQV2")
            {
                ds = PhoenixPurchaseFormDashBoard.DashBoardOrderFormSearchREQV2(General.GetNullableString(nvc["VesselList"].ToString()),
                       General.GetNullableString(nvc["VesselTypeList"].ToString()), General.GetNullableString(nvc["FleetList"].ToString()), General.GetNullableInteger(nvc["Owner"].ToString()),
                       sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                       ref iRowCount, ref iTotalPageCount);
            }
            else if (Filter.CurrentPurchaseDashboardCode == "INVV2")
            {
                ds = PhoenixPurchaseFormDashBoard.DashBoardOrderFormSearchINVV2(General.GetNullableString(nvc["VesselList"].ToString()),
                       General.GetNullableString(nvc["VesselTypeList"].ToString()), General.GetNullableString(nvc["FleetList"].ToString()), General.GetNullableInteger(nvc["Owner"].ToString()),
                       sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                       ref iRowCount, ref iTotalPageCount);
            }
            else if (Filter.CurrentPurchaseDashboardCode == "SUPV2")
            {
                ds = PhoenixPurchaseFormDashBoard.DashBoardOrderFormSearchSUPV2(General.GetNullableString(nvc["VesselList"].ToString()),
                       General.GetNullableString(nvc["VesselTypeList"].ToString()), General.GetNullableString(nvc["FleetList"].ToString()), General.GetNullableInteger(nvc["Owner"].ToString()),
                       sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                       ref iRowCount, ref iTotalPageCount);
            }
            else if (Filter.CurrentPurchaseDashboardCode == "FLTV2")
            {
                ds = PhoenixPurchaseFormDashBoard.DashBoardOrderFormSearchFLTV2(General.GetNullableString(nvc["VesselList"].ToString()),
                       General.GetNullableString(nvc["VesselTypeList"].ToString()), General.GetNullableString(nvc["FleetList"].ToString()), General.GetNullableInteger(nvc["Owner"].ToString()),
                       sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                       ref iRowCount, ref iTotalPageCount);
            }
            else if (Filter.CurrentPurchaseDashboardCode == "TDRV2")
            {
                ds = PhoenixPurchaseFormDashBoard.DashBoardOrderFormSearchTDRV2(General.GetNullableString(nvc["VesselList"].ToString()),
                       General.GetNullableString(nvc["VesselTypeList"].ToString()), General.GetNullableString(nvc["FleetList"].ToString()), General.GetNullableInteger(nvc["Owner"].ToString()),
                       sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                       ref iRowCount, ref iTotalPageCount);
            }
            else
            {
                if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
                    vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                    vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

                ds = PhoenixPurchaseOrderForm.OrderFormSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, vesselid, null,
                  null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,
                  sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                      ref iRowCount, ref iTotalPageCount, null);
            }
        }
        else if (Filter.CurrentOrderFormFilterCriteria != null)
        {
            NameValueCollection nvc = Filter.CurrentOrderFormFilterCriteria;

            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel").ToString()) == null) ? -1 : int.Parse(nvc.Get("ucVessel").ToString());
            if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
                vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

            ds = PhoenixPurchaseOrderForm.OrderFormSearchForXL(PhoenixSecurityContext.CurrentSecurityContext.UserCode, vesselid, nvc.Get("txtNumber").ToString(),
                   nvc.Get("txtTitle").ToString()
                   , nvc.Get("txtVendorid") != null ? General.GetNullableInteger(nvc.Get("txtVendorid").ToString()) : null
                   , nvc.Get("txtDeliveryLocationId") != null ? General.GetNullableInteger(nvc.Get("txtDeliveryLocationId").ToString()) : null
                   , nvc.Get("txtBudgetId") != null ? General.GetNullableInteger(nvc.Get("txtBudgetId").ToString()) : null
                   , nvc.Get("ucFinacialYear") != null ? General.GetNullableInteger(nvc.Get("ucFinacialYear").ToString()) : null
                   , nvc.Get("UCPeority") != null ? General.GetNullableInteger(nvc.Get("UCPeority").ToString()) : null
                   , nvc.Get("UCrecieptCondition") != null ? General.GetNullableInteger(nvc.Get("UCrecieptCondition").ToString()) : null
                   , nvc.Get("ucFormType") != null ? General.GetNullableInteger(nvc.Get("ucFormType").ToString()).ToString() : null
                   , nvc.Get("ucFormStatus") != null ? General.GetNullableInteger(nvc.Get("ucFormStatus").ToString()).ToString() : null
                   , nvc.Get("ucFormState") != null ? General.GetNullableInteger(nvc.Get("ucFormState").ToString()) : null
                   , nvc.Get("ucApproval") != null ? General.GetNullableInteger(nvc.Get("ucApproval").ToString()) : null
                   , nvc.Get("ddlStockType") != null ? General.GetNullableString(nvc.Get("ddlStockType").ToString()) : null
                   , nvc.Get("ucComponentclass") != null ? General.GetNullableInteger(nvc.Get("ucComponentclass").ToString()) : null
                   , nvc.Get("txtMakerReference") != null ? General.GetNullableString(nvc.Get("txtMakerReference").ToString()) : null
                   , nvc.Get("txtCreatedDate") != null ? General.GetNullableDateTime(nvc.Get("txtCreatedDate").ToString()) : null
                   , nvc.Get("txtCreatedToDate") != null ? General.GetNullableDateTime(nvc.Get("txtCreatedToDate").ToString()) : null
                   , nvc.Get("txtApprovedDate") != null ? General.GetNullableDateTime(nvc.Get("txtApprovedDate").ToString()) : null
                   , nvc.Get("txtApprovedToDate") != null ? General.GetNullableDateTime(nvc.Get("txtApprovedToDate").ToString()) : null
                   , nvc.Get("txtOrderedDate") != null ? General.GetNullableDateTime(nvc.Get("txtOrderedDate").ToString()) : null
                   , nvc.Get("txtOrderedToDate") != null ? General.GetNullableDateTime(nvc.Get("txtOrderedToDate").ToString()) : null
                   , sortexpression, sortdirection, 1, iRowCount
                       , ref iRowCount, ref iTotalPageCount
                   , nvc.Get("ddlDepartment") != null ? General.GetNullableInteger(nvc.Get("ddlDepartment").ToString()) : null
                   );
            //}


        }
        else
        {
            if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
                vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            ds = PhoenixPurchaseOrderForm.OrderFormSearchForXL(PhoenixSecurityContext.CurrentSecurityContext.UserCode, vesselid, null,
                null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,
                sortexpression, sortdirection, 1, iRowCount,
                    ref iRowCount, ref iTotalPageCount, null);

        }
        Response.AddHeader("Content-Disposition", "attachment; filename=OrderForm" + ".xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>Purchase order form  " + vesselname + "</center></h3></td>");
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
                if (alCaptions[i].ToUpper().Equals("FLDCREATEDDATE") || alCaptions[i].ToUpper().Equals("FLDPURCHASEAPPROVEDATE") || alCaptions[i].ToUpper().Equals("FLDORDEREDDATE"))
                    Response.Write(General.GetDateTimeToString(dr[alColumns[i]]));
                else
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
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("COPY"))
            {
                RadWindowManager1.RadConfirm("Do you want to copy Form " + PhoenixPurchaseOrderForm.FormNumber + " ? ", "copyrequisition", 300, 100, null, "Copy Requisition");
                //ucConfirm.Visible = true;
                //ucConfirm.Text = " Do you want to copy Form " + PhoenixPurchaseOrderForm.FormNumber +"? ";

            }
            if (CommandName.ToUpper().Equals("EXDETAILS"))
            {
                ShowExcelAmount();
            }

            //if (CommandName.ToUpper().Equals("SHOW"))
            //{
            //    if (ifMoreInfo.Visible)
            //    {
            //        ifMoreInfo.Visible = false;
            //        Filter.currentPurchaseIframeStatus = "HIDE";
            //    }
            //    else
            //    {
            //        ifMoreInfo.Visible = true;
            //        Filter.currentPurchaseIframeStatus = "SHOW";
            //    }
            //}

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcelAmount()
    {

        int vesselid = -1;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDFORMNO", "FLDTITLE", "FLDVENDORNAME", "FLDFORMTYPE", "FLDFORMSTATUS", "FLDSTOCKTYPE", "FLDCURRENCYCODE", "FLDSUBACCOUNT", "FLDACTUALTOTAL", "FLDAMOUNTUSD", "FLDINVOICEAMOUNT", "FLDINVOICESUPPLIERREFERENCE", "FLDINVOICESTATUS" };
        string[] alCaptions = { "Number", "Form Title", "Vendor", "Form Type", "Form Status", "Type", "Currency", "Budget Code", "Approved Amount<br/>(Quoted Currency)", "Approved Amount<br/>(USD)", "Invoice Amount", "Invoice No.", "Invoice Status" };
        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        //if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
        //    iRowCount = 10;
        //else
        //    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        if (Filter.CurrentOrderFormFilterCriteria != null)
        {

            NameValueCollection nvc = Filter.CurrentOrderFormFilterCriteria;

            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel").ToString()) == null) ? -1 : int.Parse(nvc.Get("ucVessel").ToString());

            if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
                vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());


            ds = PhoenixPurchaseOrderForm.OrderFormSearchByAmount(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode, vesselid,
                nvc.Get("txtTitle").ToString(),
                nvc.Get("txtNumber").ToString(),
                nvc.Get("txtVendorid") != null ? General.GetNullableInteger(nvc.Get("txtVendorid").ToString()) : null,
                 nvc.Get("txtBudgetId") != null ? General.GetNullableInteger(nvc.Get("txtBudgetId").ToString()) : null,
                  nvc.Get("ddlStockType") != null ? General.GetNullableString(nvc.Get("ddlStockType").ToString()) : null,
                  nvc.Get("txtCreatedDate") != null ? General.GetNullableDateTime(nvc.Get("txtCreatedDate").ToString()) : null,
                  nvc.Get("txtCreatedToDate") != null ? General.GetNullableDateTime(nvc.Get("txtCreatedToDate").ToString()) : null,
                  nvc.Get("txtApprovedDate") != null ? General.GetNullableDateTime(nvc.Get("txtApprovedDate").ToString()) : null,
                  nvc.Get("txtApprovedToDate") != null ? General.GetNullableDateTime(nvc.Get("txtApprovedToDate").ToString()) : null,
                  nvc.Get("txtOrderedDate") != null ? General.GetNullableDateTime(nvc.Get("txtOrderedDate").ToString()) : null,
                  nvc.Get("txtOrderedToDate") != null ? General.GetNullableDateTime(nvc.Get("txtOrderedToDate").ToString()) : null
                    );
        }
        else
        {
            if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
                vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            ds = PhoenixPurchaseOrderForm.OrderFormSearchByAmount(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode, vesselid, null,
               null, null, null, null, null, null, null, null, null, null);



        }
        Response.AddHeader("Content-Disposition", "attachment; filename=OrderFormAmount - " + vesselname + ".xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>Purchase order form - " + vesselname + "</center></h3></td>");
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

    protected void CopyForm_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
            if (ucCM.confirmboxvalue == 1)
            {
                GetDocumentNumber();
                PhoenixPurchaseOrderForm.CopytOrderFormRequisition(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                              , new Guid(ViewState["orderid"].ToString())
                              , Filter.CurrentPurchaseVesselSelection
                              , Int32.Parse(ViewState["DocumentNumber"].ToString())
                              , Filter.CurrentPurchaseStockType);
                rgvForm.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetRowSelection()
    {
        rgvForm.SelectedIndexes.Clear();
        foreach (GridDataItem item in rgvForm.Items)
        {

            if (item.GetDataKeyValue("FLDORDERID").ToString() == ViewState["orderid"].ToString())
            {
                rgvForm.SelectedIndexes.Add(item.ItemIndex);
                PhoenixPurchaseOrderForm.FormNumber = item.GetDataKeyValue("FLDFORMNO").ToString();
                Filter.CurrentPurchaseVesselSelection = int.Parse(item.GetDataKeyValue("FLDVESSELID").ToString());
                Filter.CurrentPurchaseStockType = item.GetDataKeyValue("FLDSTOCKTYPE").ToString();
                ViewState["DTKEY"] = (item["Budget"].FindControl("lbldtkey") as Label).Text;
                Filter.CurrentPurchaseStockClass = (item["Budget"].FindControl("lblStockId") as Label).Text;
                BindSendDate();
            }

        }
    }

    private void BindSendDate()
    {
        DataSet ds = new DataSet();
        ds = PhoenixPurchaseOrderForm.OrderFormCheckSendToOffice(int.Parse(Filter.CurrentPurchaseVesselSelection.ToString()), new Guid(ViewState["orderid"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentPurchaseVesselSendDateSelection = ds.Tables[0].Rows[0]["FLDAUDITTRANSFERDATE"].ToString();
        else
            Filter.CurrentPurchaseVesselSendDateSelection = "";
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        rgvForm.Rebind();
        if (Session["New"].ToString() == "Y")
        {
            Session["New"] = "N";
            BindPageURL(0);
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            GridDataItem item = (GridDataItem)rgvForm.Items[rowindex];

            ViewState["orderid"] = item.GetDataKeyValue("FLDORDERID");
            ViewState["DTKEY"] = (item["Budget"].FindControl("lbldtkey") as Label).Text;
            Filter.CurrentPurchaseVesselSelection = int.Parse(item.GetDataKeyValue("FLDVESSELID").ToString());

            PhoenixPurchaseOrderForm.FormNumber = item.GetDataKeyValue("FLDFORMNO").ToString();
            lblorderId.Text = ViewState["orderid"].ToString();

            if (ViewState["PAGEURL"].ToString().Contains("CommonFileAttachment.aspx"))
            {
                ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.PURCHASE;
                //generalPane.ContentUrl= "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.PURCHASE;
            }
            else
            {
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?orderid=" + ViewState["orderid"];
                //generalPane.ContentUrl= ViewState["PAGEURL"].ToString() + "?orderid=" + ViewState["orderid"];
            }
            SetRowSelection();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void lnkShowHide_Click(object sender, EventArgs e)
    {
        //if (ifMoreInfo.Visible)
        //{
        //    ifMoreInfo.Visible = false;
        //    Filter.currentPurchaseIframeStatus = "HIDE";
        //}
        //else
        //{
        //    ifMoreInfo.Visible = true;
        //    Filter.currentPurchaseIframeStatus = "SHOW";
        //}
    }

    protected void SetTabHighlight()
    {
        try
        {

            if (ViewState["PAGEURL"].ToString().Trim().Contains("PurchaseFormGeneral.aspx"))
            {
                MenuOrderFormMain.SelectedMenuIndex = 0;
            }
            else if (ViewState["PAGEURL"].ToString().Trim().Contains("PurchaseFormItemDetails.aspx"))
            {
                MenuOrderFormMain.SelectedMenuIndex = 1;
            }
            else if (ViewState["PAGEURL"].ToString().Trim().Contains("PurchaseFormDetail.aspx"))
            {
                MenuOrderFormMain.SelectedMenuIndex = 1;
            }
            else if (ViewState["PAGEURL"].ToString().Trim().Contains("PurchaseReasonForRequisition.aspx"))
            {
                MenuOrderFormMain.SelectedMenuIndex = 2;
            }
            else if (ViewState["PAGEURL"].ToString().Trim().Contains("PurchaseFormTransactionHistory.aspx"))
            {
                MenuOrderFormMain.SelectedMenuIndex = 4;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixCommonPurchase.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    private void GetDocumentNumber()
    {
        DataTable dt = PhoenixPurchaseOrderForm.GetDocumentNumber();
        if (dt.Rows.Count > 0)
        {
            ViewState["DocumentNumber"] = dt.Rows[0]["FLDDOCUMENTTYPEID"].ToString();
        }
        else
        {
            ViewState["DocumentNumber"] = "0";
        }
    }

    protected void rgvForm_ItemCommand(object sender, GridCommandEventArgs e)
    {
        rgvForm.SelectedIndexes.Clear();
        string name = e.CommandName;
        if (e.CommandName == RadGrid.PageCommandName)
        {
            Filter.CurrentPurchaseOrderIdSelection = null;
            ViewState["orderid"] = null;
        }
        if (e.CommandName.ToUpper().Equals("ROWCLICK") || e.CommandName.ToUpper().Equals("SELECT"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            Guid orderid = new Guid(item.GetDataKeyValue("FLDORDERID").ToString());
            int vesselid = int.Parse(item.GetDataKeyValue("FLDVESSELID").ToString());
            string stocktype = item.GetDataKeyValue("FLDSTOCKTYPE").ToString();

            PhoenixPurchaseOrderForm.FormNumber = item.GetDataKeyValue("FLDFORMNO").ToString();
            Filter.CurrentPurchaseStockType = stocktype;
            Filter.CurrentPurchaseOrderIdSelection = item.GetDataKeyValue("FLDORDERID").ToString();
            rgvForm.SelectedIndexes.Add(e.Item.ItemIndex);
            BindPageURL(e.Item.ItemIndex);
        }


    }
    protected void rgvForm_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {


            int iRowCount = 0;
            int iTotalPageCount = 0;
            int vesselid = -1;
            string[] alColumns = { "FLDFORMNO", "FLDTITLE", "FLDVENDORNAME", "FLDFORMTYPENAME", "FLDFORMSTATUSNAME", "FLDSUBACCOUNT", "FLDORDEREDDATE", "FLDVENDORDELIVERYDATE", "FLDSTOCKTYPE", "FLDSTOCKCLASS", "FLDSTATUS", "FLDVESSELCODE", "FLDCURRENCY", "FLDBUDGETNAME", "FLDREASONFORREQUISITIONNAME" };
            string[] alCaptions = { "Number", "Form Title", "Vendor", "Form Type", "Form Status", "Budget Code", "Ordered date", "Received date", "Type", "Component Class</br>/Store Type", "Status", "Vessel", "Currency", "Budget", "Reason for Requisition" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());


            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = new DataSet();

            if (Filter.CurrentPurchaseDashboardCode != null && PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentOrderFormFilterCriteria == null)
            {
                ds = PhoenixPurchaseOrderForm.OrderFormSearchDashboard(PhoenixSecurityContext.CurrentSecurityContext.VesselID, Filter.CurrentPurchaseDashboardCode
                                    , sortexpression
                                    , sortdirection
                                    , rgvForm.CurrentPageIndex + 1
                                    , rgvForm.PageSize
                                    , ref iRowCount
                                    , ref iTotalPageCount);
            }
            else if (Filter.CurrentPurchaseOwnerReportCode != null && int.Parse(Filter.SelectedOwnersReportVessel) > 0 && Filter.CurrentOrderFormFilterCriteria == null)
            {
                ds = PhoenixPurchaseOrderForm.OrderFormSearchOwnerReport(int.Parse(Filter.SelectedOwnersReportVessel), Filter.CurrentPurchaseOwnerReportCode
                                    , DateTime.Parse(Filter.SelectedOwnersReportDate)
                                    , sortexpression
                                    , sortdirection
                                    , rgvForm.CurrentPageIndex + 1
                                    , rgvForm.PageSize
                                    , ref iRowCount
                                    , ref iTotalPageCount);
            }
            else if (Filter.CurrentPurchaseDashboardCode != null && PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
            {
                NameValueCollection nvc = new NameValueCollection();        //= FilterDashboard.OfficeDashboardFilterCriteria;
                nvc["VesselList"] = string.Empty;
                nvc["FleetList"] = string.Empty;
                nvc["Owner"] = string.Empty;
                nvc["VesselTypeList"] = string.Empty;
                nvc["RankList"] = string.Empty;
                //if (nvc == null)
                //{
                //    nvc = new NameValueCollection();

                //}

                if (General.GetNullableString(ViewState["dvslid"].ToString()) != null)
                    nvc["VesselList"] = "," + ViewState["dvslid"].ToString() + ",";



                if (Filter.CurrentPurchaseDashboardCode == "POAWVR")
                {
                    ds = PhoenixPurchaseFormDashBoard.DashBoardOrderFormSearchPOAWVR(General.GetNullableString(nvc["VesselList"].ToString()),
                           General.GetNullableString(nvc["VesselTypeList"].ToString()), General.GetNullableString(nvc["FleetList"].ToString()), General.GetNullableInteger(nvc["Owner"].ToString()),
                           sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                           ref iRowCount, ref iTotalPageCount);
                }
                else if (Filter.CurrentPurchaseDashboardCode == "RFQAP")
                {
                    ds = PhoenixPurchaseFormDashBoard.DashBoardOrderFormSearchRFQAP(General.GetNullableString(nvc["VesselList"].ToString()),
                           General.GetNullableString(nvc["VesselTypeList"].ToString()), General.GetNullableString(nvc["FleetList"].ToString()), General.GetNullableInteger(nvc["Owner"].ToString()),
                           sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                           ref iRowCount, ref iTotalPageCount);
                }
                else if (Filter.CurrentPurchaseDashboardCode == "AQPPO")
                {
                    ds = PhoenixPurchaseFormDashBoard.DashBoardOrderFormSearchAQPPO(General.GetNullableString(nvc["VesselList"].ToString()),
                           General.GetNullableString(nvc["VesselTypeList"].ToString()), General.GetNullableString(nvc["FleetList"].ToString()), General.GetNullableInteger(nvc["Owner"].ToString()),
                           sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                           ref iRowCount, ref iTotalPageCount);
                }
                else if (Filter.CurrentPurchaseDashboardCode == "POND")
                {
                    ds = PhoenixPurchaseFormDashBoard.DashBoardOrderFormSearchPOND(General.GetNullableString(nvc["VesselList"].ToString()),
                           General.GetNullableString(nvc["VesselTypeList"].ToString()), General.GetNullableString(nvc["FleetList"].ToString()), General.GetNullableInteger(nvc["Owner"].ToString()),
                           sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                           ref iRowCount, ref iTotalPageCount);
                }
                else if (Filter.CurrentPurchaseDashboardCode == "QAA5K")
                {
                    ds = PhoenixPurchaseFormDashBoard.DashBoardOrderFormSearchQAA5K(General.GetNullableString(nvc["VesselList"].ToString()),
                           General.GetNullableString(nvc["VesselTypeList"].ToString()), General.GetNullableString(nvc["FleetList"].ToString()), General.GetNullableInteger(nvc["Owner"].ToString()),
                           sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                           ref iRowCount, ref iTotalPageCount);
                }
                else if (Filter.CurrentPurchaseDashboardCode == "QAA10K")
                {
                    ds = PhoenixPurchaseFormDashBoard.DashBoardOrderFormSearchQAA10K(General.GetNullableString(nvc["VesselList"].ToString()),
                           General.GetNullableString(nvc["VesselTypeList"].ToString()), General.GetNullableString(nvc["FleetList"].ToString()), General.GetNullableInteger(nvc["Owner"].ToString()),
                           sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                           ref iRowCount, ref iTotalPageCount);
                }
                else if (Filter.CurrentPurchaseDashboardCode == "QTNR")
                {
                    ds = PhoenixPurchaseFormDashBoard.DashBoardOrderFormSearchQTNR(General.GetNullableString(nvc["VesselList"].ToString()),
                           General.GetNullableString(nvc["VesselTypeList"].ToString()), General.GetNullableString(nvc["FleetList"].ToString()), General.GetNullableInteger(nvc["Owner"].ToString()),
                           sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                           ref iRowCount, ref iTotalPageCount);
                }
                else if (Filter.CurrentPurchaseDashboardCode == "REQV2")
                {
                    ds = PhoenixPurchaseFormDashBoard.DashBoardOrderFormSearchREQV2(General.GetNullableString(nvc["VesselList"].ToString()),
                           General.GetNullableString(nvc["VesselTypeList"].ToString()), General.GetNullableString(nvc["FleetList"].ToString()), General.GetNullableInteger(nvc["Owner"].ToString()),
                           sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                           ref iRowCount, ref iTotalPageCount);
                }
                else if (Filter.CurrentPurchaseDashboardCode == "INVV2")
                {
                    ds = PhoenixPurchaseFormDashBoard.DashBoardOrderFormSearchINVV2(General.GetNullableString(nvc["VesselList"].ToString()),
                           General.GetNullableString(nvc["VesselTypeList"].ToString()), General.GetNullableString(nvc["FleetList"].ToString()), General.GetNullableInteger(nvc["Owner"].ToString()),
                           sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                           ref iRowCount, ref iTotalPageCount);
                }
                else if (Filter.CurrentPurchaseDashboardCode == "SUPV2")
                {
                    ds = PhoenixPurchaseFormDashBoard.DashBoardOrderFormSearchSUPV2(General.GetNullableString(nvc["VesselList"].ToString()),
                           General.GetNullableString(nvc["VesselTypeList"].ToString()), General.GetNullableString(nvc["FleetList"].ToString()), General.GetNullableInteger(nvc["Owner"].ToString()),
                           sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                           ref iRowCount, ref iTotalPageCount);
                }
                else if (Filter.CurrentPurchaseDashboardCode == "FLTV2")
                {
                    ds = PhoenixPurchaseFormDashBoard.DashBoardOrderFormSearchFLTV2(General.GetNullableString(nvc["VesselList"].ToString()),
                           General.GetNullableString(nvc["VesselTypeList"].ToString()), General.GetNullableString(nvc["FleetList"].ToString()), General.GetNullableInteger(nvc["Owner"].ToString()),
                           sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                           ref iRowCount, ref iTotalPageCount);
                }
                else if (Filter.CurrentPurchaseDashboardCode == "TDRV2")
                {
                    ds = PhoenixPurchaseFormDashBoard.DashBoardOrderFormSearchTDRV2(General.GetNullableString(nvc["VesselList"].ToString()),
                           General.GetNullableString(nvc["VesselTypeList"].ToString()), General.GetNullableString(nvc["FleetList"].ToString()), General.GetNullableInteger(nvc["Owner"].ToString()),
                           sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                           ref iRowCount, ref iTotalPageCount);
                }
                else
                {
                    if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
                        vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

                    if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                        vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

                    ds = PhoenixPurchaseOrderForm.OrderFormSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, vesselid, null,
                      null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,
                      sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                          ref iRowCount, ref iTotalPageCount, null);
                }
            }
            else if (Filter.CurrentOrderFormFilterCriteria != null)
            {
                NameValueCollection nvc = Filter.CurrentOrderFormFilterCriteria;

                vesselid = (General.GetNullableInteger(nvc.Get("ucVessel").ToString()) == null) ? -1 : int.Parse(nvc.Get("ucVessel").ToString());
                if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
                    vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

                ds = PhoenixPurchaseOrderForm.OrderFormSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, vesselid, nvc.Get("txtNumber").ToString(),
                    nvc.Get("txtTitle") != null ? nvc.Get("txtTitle").ToString() : string.Empty,
                    nvc.Get("txtVendorid") != null ? General.GetNullableInteger(nvc.Get("txtVendorid").ToString()) : null,
                    nvc.Get("txtDeliveryLocationId") != null ? General.GetNullableInteger(nvc.Get("txtDeliveryLocationId").ToString()) : null,
                    nvc.Get("txtBudgetId") != null ? General.GetNullableInteger(nvc.Get("txtBudgetId").ToString()) : null,
                    nvc.Get("ucFinacialYear") != null ? General.GetNullableInteger(nvc.Get("ucFinacialYear").ToString()) : null,
                    nvc.Get("UCPeority") != null ? General.GetNullableInteger(nvc.Get("UCPeority").ToString()) : null,
                    nvc.Get("UCrecieptCondition") != null ? General.GetNullableInteger(nvc.Get("UCrecieptCondition").ToString()) : null,
                    nvc.Get("ucFormType") != null ? General.GetNullableInteger(nvc.Get("ucFormType").ToString()).ToString() : null,
                    nvc.Get("ucFormStatus") != null ? General.GetNullableInteger(nvc.Get("ucFormStatus").ToString()).ToString() : null,
                    nvc.Get("ucFormState") != null ? General.GetNullableInteger(nvc.Get("ucFormState").ToString()) : null,
                    nvc.Get("ucApproval") != null ? General.GetNullableInteger(nvc.Get("ucApproval").ToString()) : null,
                    nvc.Get("ddlStockType") != null ? General.GetNullableString(nvc.Get("ddlStockType").ToString()) : null,
                    nvc.Get("ucComponentclass") != null ? General.GetNullableInteger(nvc.Get("ucComponentclass").ToString()) : null,
                    nvc.Get("txtMakerReference") != null ? General.GetNullableString(nvc.Get("txtMakerReference").ToString()) : null,
                    nvc.Get("txtCreatedDate") != null ? General.GetNullableDateTime(nvc.Get("txtCreatedDate").ToString()) : null,
                    nvc.Get("txtCreatedToDate") != null ? General.GetNullableDateTime(nvc.Get("txtCreatedToDate").ToString()) : null,
                    nvc.Get("txtApprovedDate") != null ? General.GetNullableDateTime(nvc.Get("txtApprovedDate").ToString()) : null,
                    nvc.Get("txtApprovedToDate") != null ? General.GetNullableDateTime(nvc.Get("txtApprovedToDate").ToString()) : null,
                    nvc.Get("txtOrderedDate") != null ? General.GetNullableDateTime(nvc.Get("txtOrderedDate").ToString()) : null,
                    nvc.Get("txtOrderedToDate") != null ? General.GetNullableDateTime(nvc.Get("txtOrderedToDate").ToString()) : null,
                      sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                          ref iRowCount, ref iTotalPageCount,
                    nvc.Get("ddlDepartment") != null ? General.GetNullableInteger(nvc.Get("ddlDepartment").ToString()) : null,
                    nvc.Get("ddlReqStatus") != null ? General.GetNullableString(nvc.Get("ddlReqStatus").ToString()) : null,
                    nvc.Get("ucReason4Requisition") != null ? General.GetNullableInteger(nvc.Get("ucReason4Requisition").ToString()) : null);
                //}


            }
            else
            {
                if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
                    vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                    vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

                ds = PhoenixPurchaseOrderForm.OrderFormSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, vesselid, null,
                  null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,
                  sortexpression, sortdirection, rgvForm.CurrentPageIndex + 1, rgvForm.PageSize,
                      ref iRowCount, ref iTotalPageCount, null);

            }

            General.SetPrintOptions("rgvForm", "Order Form List -  " + vesselname, alCaptions, alColumns, ds);

            rgvForm.DataSource = ds;
            rgvForm.VirtualItemCount = iRowCount;
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                vesselname = PhoenixSecurityContext.CurrentSecurityContext.VesselName; // dr["FLDVESSELNAME"].ToString();

                if (Filter.CurrentPurchaseOrderIdSelection != null)
                    ViewState["orderid"] = Filter.CurrentPurchaseOrderIdSelection.ToString();

                if (Filter.CurrentPurchaseOrderComponentIDSelection != null)
                    ViewState["COMPONENTID"] = Filter.CurrentPurchaseOrderComponentIDSelection.ToString();

                if (ViewState["orderid"] == null)
                {
                    Filter.CurrentPurchaseVesselSelection = int.Parse(dr["FLDVESSELID"].ToString());
                    ViewState["orderid"] = dr["FLDORDERID"].ToString();
                    ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
                    ViewState["COMPONENTID"] = dr["FLDCOMPONENTID"].ToString();
                    lblorderId.Text = ViewState["orderid"].ToString();
                    MenuOrderFormMain.Visible = true;
                }
                if (ViewState["PAGEURL"] == null)
                {
                    ViewState["PAGEURL"] = "../Purchase/PurchaseFormGeneral.aspx";
                }

                if (ViewState["PAGEURL"].ToString().Contains("CommonFileAttachment.aspx"))
                {
                    ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.PURCHASE;
                    //generalPane.ContentUrl= "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.PURCHASE;
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?orderid=" + ViewState["orderid"].ToString();
                    //generalPane.ContentUrl = ViewState["PAGEURL"].ToString() + "?orderid=" + ViewState["orderid"].ToString();
                }

            }
            else
            {

                ViewState["PAGEURL"] = "PurchaseFormType.aspx";
                ifMoreInfo.Attributes["src"] = "PurchaseFormType.aspx";
                //generalPane.ContentUrl = "PurchaseFormType.aspx";
                MenuOrderFormMain.Visible = false;


            }

            //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            //{
            //    gvFormDetails.Columns[10].Visible = false;
            //}
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            SetTabHighlight();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void rgvForm_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)e.Item.DataItem;
            Image img = item["FORMNO"].FindControl("imgPriority") as Image;


            if (General.GetNullableInteger(drv["PRIORITYFLAG"].ToString()) > 0)
                img.Visible = true;
            else
                img.Visible = false;


            LinkButton Imgcopy = item["Action"].FindControl("cmdCopy") as LinkButton;

            if (Imgcopy != null && General.GetNullableString(drv["FLDSTOCKTYPE"].ToString()) != null)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode.ToString().Equals("0"))
                    Imgcopy.Visible = SessionUtil.CanAccess(this.ViewState, Imgcopy.CommandName);
                else
                    Imgcopy.Visible = false;
            }
            if (Imgcopy != null && General.GetNullableGuid(drv["FLDORDERID"].ToString()) != null && General.GetNullableInteger(drv["FLDVESSELID"].ToString()) != null && General.GetNullableString(drv["FLDSTOCKTYPE"].ToString()) != null)
                Imgcopy.Attributes.Add("onclick", "openNewWindow('MoreInfo', 'Copy Requisition', 'Purchase/PurchaseFormCopyStoreRequisition.aspx?ORDERID=" + drv["FLDORDERID"].ToString() + "&VESSELID=" + drv["FLDVESSELID"].ToString() + "&STOCKTYPE=" + drv["FLDSTOCKTYPE"].ToString().ToUpper() + "&FORMNO=" + drv["FLDFORMNO"].ToString() + "'); return false;");

            LinkButton vr = item["Action"].FindControl("cmdVesselReceipt") as LinkButton;
            if (vr != null && General.GetNullableGuid(drv["FLDORDERID"].ToString()) != null)
            {
                vr.Visible = SessionUtil.CanAccess(this.ViewState, vr.CommandName);
                vr.Attributes.Add("onclick", "openNewWindow('VesselReceipt', 'Vessel Receipt', 'Reports/ReportsView.aspx?applicationcode=3&reportcode=VESSELRECEIPT&orderid=" + drv["FLDORDERID"].ToString() + "&showmenu=0&showword=no&showexcel=no');return false;");
            }


            LinkButton Rtn = item["Action"].FindControl("cmdReturn") as LinkButton;

            int? Status = General.GetNullableInteger(((RadLabel)item.FindControl("lblStatusId")).Text);
            Guid? Orderid = General.GetNullableGuid(item.GetDataKeyValue("FLDORDERID").ToString());
            int? vessel = General.GetNullableInteger(item.GetDataKeyValue("FLDVESSELID").ToString());
            string StockType = General.GetNullableString(((RadLabel)item.FindControl("lblType")).Text);

            if (Rtn != null && Status == 55 && (StockType == "SPARE" || StockType == "STORE"))
            {
                Rtn.Visible = SessionUtil.CanAccess(this.ViewState, Rtn.CommandName);
                Rtn.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','GRN','Purchase/PurchaseGoodsReturnLineAdd.aspx?ORDERID=" + Orderid + "&VESSELID=" + vessel + "');return false");
            }
            else
            {
                Rtn.Visible = false;
            }

        }

    }
    protected void rgvForm_UpdateCommand(object sender, GridCommandEventArgs e)
    {
    }
    protected void rgvForm_PreRender(object sender, EventArgs e)
    {
    }
    protected void rgvForm_InsertCommand(object sender, GridCommandEventArgs e)
    {
    }
    protected void rgvForm_DeleteCommand(object sender, GridCommandEventArgs e)
    {
    }
    protected void rgvForm_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
        rgvForm.Rebind();
    }

    protected void copy_Click(object sender, EventArgs e)
    {
        GetDocumentNumber();
        PhoenixPurchaseOrderForm.CopytOrderFormRequisition(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                      , new Guid(ViewState["orderid"].ToString())
                      , Filter.CurrentPurchaseVesselSelection
                      , Int32.Parse(ViewState["DocumentNumber"].ToString())
                      , Filter.CurrentPurchaseStockType);
        rgvForm.Rebind();
    }
}
