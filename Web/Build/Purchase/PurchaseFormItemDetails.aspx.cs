using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Configuration;
using SouthNests.Phoenix.Registers;
using System.Text;

public partial class PurchaseFormItemDetails : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        //foreach (GridViewRow r in gvLineItem.Rows)
        //{
        //    if (r.RowType == DataControlRowType.DataRow )
        //    {
        //        Page.ClientScript.RegisterForEventValidation(gvLineItem.UniqueID, "Edit$" + r.RowIndex.ToString());
        //    }
        //}
        base.Render(writer);
    }
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
                //rgvLine.PageSize = General.ShowRecords(10);
                //rgvLine.PageSize = 10;
                
                ViewState["COMPONENTID"] = "";
                if (Request.QueryString["orderid"] != null)
                {
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                }
                else
                {
                    ViewState["orderid"] = "0";
                }
                //if (Request.QueryString["COMPONENTID"] != null)
                //{
                //    ViewState["COMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();
                //}

                if (Filter.CurrentPurchaseOrderComponentIDSelection != null && General.GetNullableGuid(Filter.CurrentPurchaseOrderComponentIDSelection.ToString()) != null)
                    ViewState["COMPONENTID"] = Filter.CurrentPurchaseOrderComponentIDSelection.ToString();

                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Vessel Form", "FORMS",ToolBarDirection.Left);
                toolbarmain.AddButton("Line Items", "GENERAL",ToolBarDirection.Left);
                toolbarmain.AddButton("Details", "DETAILS",ToolBarDirection.Left);
                toolbarmain.AddButton("Attachment", "ATTACHMENT",ToolBarDirection.Left);
                if (Filter.CurrentVesselConfiguration == null || Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                    toolbarmain.AddButton("Vendor Quotations", "QUOTATION",ToolBarDirection.Left);
                
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
                MenuLineItem.AccessRights = this.ViewState; 
                MenuLineItem.MenuList = toolbarmain.Show();
                MenuLineItem.SelectedMenuIndex = 1;
                

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["pageno"] = Request.QueryString["pageno"].ToString();

                if (Request.QueryString["orderid"] != null)
                {
                   ifMoreInfo.Attributes["src"] = "PurchaseFormItem.aspx?orderid=" + Request.QueryString["orderid"].ToString();                  
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = "PurchaseFormItem.aspx";                  
                }
                //MenuLineItem.Title = "Line Items ( " + PhoenixPurchaseOrderForm.FormNumber + " )";
            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            if (!Filter.CurrentPurchaseVesselSendDateSelection.ToUpper().Equals("") && !(Filter.CurrentVesselConfiguration.Equals("0") || Filter.CurrentVesselConfiguration == null))
            {
                toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseFormItemDetails.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
                toolbargrid.AddFontAwesomeButton("javascript:CallPrint('rgvLine')", "Print", "<i class=\"fas fa-print\"></i>", "Print");
                toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Report','','Reports/ReportsView.aspx?applicationcode=3&reportcode=REQUISTIONFORM&orderid=" + Request.QueryString["orderid"].ToString() + "');return false;", "Requistion Form", "<i class=\"fas fa-file-alt\"></i>", "REQUISTION");
            }
            else
            {
                toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseFormItemDetails.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
                toolbargrid.AddFontAwesomeButton("javascript:CallPrint('rgvLine')", "Print", "<i class=\"fas fa-print\"></i>", "Print");
                if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SERVICE"))
                    toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter_1','','Purchase/PurchaseOrderLineItemSelectService.aspx?orderid=" + Request.QueryString["orderid"].ToString() + "&COMPONENTID=" + ViewState["COMPONENTID"] + "&vesselid=" + Filter.CurrentPurchaseVesselSelection + "');return false;", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
                else
                    toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter_1','','Purchase/PurchaseOrderLineItemSpareSelect.aspx?orderid=" + Request.QueryString["orderid"].ToString() + "&COMPONENTID=" + ViewState["COMPONENTID"] + "&vesselid=" + Filter.CurrentPurchaseVesselSelection + "');return false;", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
                //toolbargrid.AddImageButton("../Purchase/PurchaseFormItemDetails.aspx", "Delete", "te_del.png", "BulkDelete");
                toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Report','','Reports/ReportsView.aspx?applicationcode=3&reportcode=REQUISTIONFORM&orderid=" + Request.QueryString["orderid"].ToString() + "');return false;", "Requisition Form", "<i class=\"fas fa-file-alt\"></i>", "REQUISTION");
            }

            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter_1','','Purchase/PurchaseOrderLineItemBulkSave.aspx?orderid=" + ViewState["orderid"].ToString() + "');return false;", "Save", "<i class=\"fas fa-database\"></i>", "BULKSAVE");
            toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseFormItemDetails.aspx?orderid=" + Request.QueryString["orderid"].ToString(), "Verify", "<i class=\"fas fa-check\"></i>", "VERIFY");
			toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Split','','Purchase/PurchaseRequisitionSplitByQuantity.aspx?orderid=" + ViewState["orderid"].ToString() + "','true');return false;", "Split", "<i class=\"fas fa-sitemap\"></i>", "SPLIT");
            //if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SPARE") || Filter.CurrentPurchaseStockType.ToUpper().Equals("SERVICE"))
            //{
            //    toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Stock','','Purchase/PurchaseOrderLineItemROBWarehouse.aspx?orderid=" + ViewState["orderid"].ToString() + "','true');return false;", "ROB at Warehouse", "<i class=\"fa fa-cubes\"></i>", "WROB");
            //}
            MenuOrderLineItem.AccessRights = this.ViewState;
            MenuOrderLineItem.MenuList = toolbargrid.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuLineItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                ViewState["PAGEURL"] = "../Purchase/PurchaseFormItem.aspx";
            }
            else if (CommandName.ToUpper().Equals("DETAILS"))
            {
                ViewState["PAGEURL"] = "../Purchase/PurchaseFormItemComment.aspx";
            }
            else if (CommandName.ToUpper().Equals("FORMS"))
            {
                if (ViewState["orderid"].ToString() == "0")
                    Response.Redirect("../Purchase/PurchaseForm.aspx?pageno=" + ViewState["pageno"].ToString());
                else
                    Response.Redirect("../Purchase/PurchaseForm.aspx?orderid=" + ViewState["orderid"].ToString() + "&pageno=" + ViewState["pageno"].ToString());
            }
            else if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                ViewState["PAGEURL"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.PURCHASE;
            }

            if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString();
            }
            else if (CommandName.ToUpper().Equals("SPLIT"))
            {
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString();
            }
            else if (CommandName.ToUpper().Equals("QUOTATION"))
            {
                if (ViewState["orderid"] == null)
                    Response.Redirect("../Purchase/PurchaseQuotationVendorDetail.aspx?pageno=" + ViewState["pageno"]);
                else
                    Response.Redirect("../Purchase/PurchaseQuotationVendorDetail.aspx?orderid=" + ViewState["orderid"].ToString() + "&pageno=" + ViewState["pageno"]);
            }
            else 
            {
                if (ViewState["orderlineid"] != null)
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?orderid=" + ViewState["orderid"] + "&orderlineid=" + ViewState["orderlineid"].ToString();
                else
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() +"?orderid=" +  ViewState["orderid"].ToString();
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

        string[] alColumns;
        string[] alCaptions;

        if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SPARE") || Filter.CurrentPurchaseStockType.ToUpper().Equals("SERVICE"))
        {
            alColumns = new string[16] {"FLDSERIALNO", "FLDPARTNUMBER","FLDMAKER", "FLDMAKERREFERENCE","FLDDRAWINGNO", "FLDNAME","FLDNOTES", "FLDROBQUANTITY", "FLDREQUESTEDQUANTITY",
                                 "FLDUNITNAME", "FLDSPLITFORMNO", "FLDORDEREDQUANTITY", "FLDSUBACCOUNT", "FLDOWNERACCOUNT", "FLDQUICKNAME", "FLDCHKREMARKS" };
            alCaptions = new string[16] {"S. No.", "Number","Maker", "Maker Reference","Drawing No", "Name", "Details", "ROB", "Requested Qty",
                                 "Unit", "From Order", "Order Qty", "Budget Code", "Owner Budget Code", "Receipt Status", "Received Remarks" };
        }
        else
        {
            alColumns = new string[15] {"FLDSERIALNO", "FLDPARTNUMBER","FLDMAKER", "FLDMAKERREFERENCE", "FLDNAME", "FLDNOTES","FLDROBQUANTITY", "FLDREQUESTEDQUANTITY",
                                 "FLDUNITNAME", "FLDSPLITFORMNO", "FLDORDEREDQUANTITY", "FLDSUBACCOUNT", "FLDOWNERACCOUNT", "FLDQUICKNAME", "FLDCHKREMARKS" };
            alCaptions = new string[15] {"S. No.", "Number", "Maker", "Product Code", "Name", "Details", "ROB", "Requested Qty",
                                 "Unit", "From Order", "Order Qty", "Budget Code", "Owner Budget Code", "Receipt Status", "Received Remarks" };
        }

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

       
        ds = PhoenixPurchaseOrderLine.OrderLineSearch(
            General.GetNullableGuid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection, 
            sortexpression, sortdirection, 1, iRowCount, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=FormLineItem - " + PhoenixPurchaseOrderForm.FormNumber + ".xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Line Item Details</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
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
                Response.Write((alColumns[i] == "FLDNOTES") ? RemoveHTMLTags(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                //Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void MenuOrderLineItem_TabStripCommand(object sender, EventArgs e)
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
            if (CommandName.ToUpper().Equals("BULKDELETE"))
            {
                BulkDelete();               
            }
            if (CommandName.ToUpper().Equals("VERIFY"))
            {
                try
                {
                    UpdateOrderFormStatus();
                    RateContractVendorQuotationInsert();
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void UpdateOrderFormStatus()
    {
        PhoenixPurchaseOrderLine.StatusUpdateOrderForm(new Guid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection);
    }
    private void RateContractVendorQuotationInsert()
    { string vendorlist = "";
        PhoenixPurchaseOrderLine.RateContractVendorQuotationInsert(new Guid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection,ref vendorlist);
        if (vendorlist != "")
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = PhoenixPurchaseQuotation.QuotationSearch(General.GetNullableGuid(ViewState["orderid"].ToString()), null, null, 1,100, ref iRowCount, ref iTotalPageCount);
            DataTable dt = ds.Tables[0];
            vendorlist = ",";
            for (int i = 0; i < dt.Rows.Count; i ++)
            {
                vendorlist = vendorlist + dt.Rows[i]["FLDQUOTATIONID"].ToString() + ",";

            }
            SendForQuotation(vendorlist);
        }
    }
    private void SendForQuotation(string selectedvendors)
    {
        string emailid;
        try
        {
            
            if (selectedvendors.Length <= 1)
                selectedvendors = null;
            DataSet dsvendor = PhoenixPurchaseQuotation.ListQuotationToSend(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["orderid"].ToString()), selectedvendors, Int64.Parse(DateTime.Now.ToString("yyyyMMddhhmm")));
            if (dsvendor.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in dsvendor.Tables[0].Rows)
                {
                    string emailbodytext = "";
                    string supemail = "";
                    if (dr["FLDEMAIL1"].ToString().Contains(";"))
                        emailid = dr["FLDEMAIL1"].ToString().Replace(";", ",");
                    else
                        emailid = dr["FLDEMAIL1"].ToString();

                    if (!dr["FLDEMAIL2"].ToString().Equals(""))
                    {
                        emailid = emailid + "," + dr["FLDEMAIL2"].ToString().Replace(";", ",");
                    }
                    /*  Bug Id: 9143 - No need to cc the Supdt. mail id while sending RFQ..
                    if (dr["FLDSUPTEMAIL"].ToString().Equals("") || dr["FLDSUPPLIEREMAIL"].ToString().Equals(""))
                        supemail = dr["FLDSUPTEMAIL"].ToString() + dr["FLDSUPPLIEREMAIL"].ToString();
                    else
                        supemail = dr["FLDSUPTEMAIL"].ToString() + "," + dr["FLDSUPPLIEREMAIL"].ToString();
                    */
                    // Bug Id: 9143..

                    /* Bug Id: 9901 - Again the users want to cc the supdt
                    */
                    if (dr["FLDSUPTEMAIL"].ToString().Equals("") || dr["FLDSUPPLIEREMAIL"].ToString().Equals(""))
                        supemail = dr["FLDSUPTEMAIL"].ToString() + dr["FLDSUPPLIEREMAIL"].ToString();
                    else
                        supemail = dr["FLDSUPTEMAIL"].ToString() + "," + dr["FLDSUPPLIEREMAIL"].ToString();
                    // Bug Id: 9901..
                    supemail = dr["FLDSUPPLIEREMAIL"].ToString();

                    DataSet dscontact;
                    dscontact = PhoenixPurchaseQuotation.QuotationContactsGetEmail(General.GetNullableInteger(dr["FLDVENDORID"].ToString()), Filter.CurrentPurchaseStockType.ToString(), Filter.CurrentPurchaseVesselSelection);
                    if (!dscontact.Tables[0].Rows[0]["FLDEMALTO"].ToString().Trim().Equals(""))
                    {
                        emailid = emailid + dscontact.Tables[0].Rows[0]["FLDEMALTO"].ToString().Trim();
                    }
                    if (!dscontact.Tables[0].Rows[0]["FLDEMALCC"].ToString().Trim().Equals(""))
                    {
                        supemail = supemail + dscontact.Tables[0].Rows[0]["FLDEMALCC"].ToString().Trim();
                    }

                    try
                    {
                        if (dr["FLDRFQPREFERENCE"].ToString().Equals("WEB"))
                        {
                            PhoenixPurchaseQuotation.InsertWebSession(new Guid(dr["FLDQUOTATIONID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode, "RFQ", PhoenixPurchaseOrderForm.FormNumber);
                            emailbodytext = PrepareEmailBodyText(new Guid(dr["FLDQUOTATIONID"].ToString()), dr["FLDFORMNO"].ToString(), dr["FLDFROMEMAIL"].ToString());
                            PhoenixCommoneProcessing.PrepareEmailMessage(
                                emailid, "RFQ", new Guid(dr["FLDQUOTATIONID"].ToString()),
                                "", supemail.Equals("") ? dr["FLDFROMEMAIL"].ToString() : supemail + "," + dr["FLDFROMEMAIL"].ToString(),
                                dr["FLDNAME"].ToString(), dr["FLDVESSELNAME"].ToString() + " - RFQ for " + dr["FLDFORMNO"].ToString() + "" + (dr["FLDTITLE"].ToString() == "" ? "" : "-") + dr["FLDTITLE"].ToString() + "" + (dr["FLDSEAPORTNAME"].ToString() == "" ? "" : "-") + dr["FLDSEAPORTNAME"].ToString(), emailbodytext, "", "");
                            PhoenixPurchaseQuotation.UpdateQuotation(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["orderid"].ToString()), new Guid(dr["FLDQUOTATIONID"].ToString()));

                        }
                        else
                        {
                            PhoenixPurchaseQuotation.QuotationRFQExcelInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(dr["FLDORDERID"].ToString()), new Guid(dr["FLDQUOTATIONID"].ToString()), int.Parse(dr["FLDVENDORID"].ToString()));
                        }
                       // ucConfirm.ErrorMessage = "Email sent to " + dr["FLDNAME"].ToString() + "\n";
                    }
                    catch (Exception)
                    {
                       // ucConfirm.ErrorMessage = ex.Message + " for  " + dr["FLDNAME"].ToString() + "\n";
                    }
                }
                InsertOrderFormHistory();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void InsertOrderFormHistory()
    {
        PhoenixPurchaseOrderForm.InsertOrderFormHistory(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection);
    }
    protected string PrepareEmailBodyText(Guid quotationid, string orderformnumber, string frommailid)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataSet ds = PhoenixPurchaseQuotation.GetQuotationDataForEmailBody(PhoenixSecurityContext.CurrentSecurityContext.UserCode, quotationid, "RFQ");
        DataRow dr = ds.Tables[0].Rows[0];

        sbemailbody.Append("This is an automated message. DO NOT REPLY to " + ConfigurationManager.AppSettings.Get("FromMail").ToString() + ". Kindly use the \"reply all\" button if you are responding to this message.");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.Append(dr["FLDVENDORNAME"].ToString() + "             " + orderformnumber);
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.Append("Dear Sir");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine(dr["FLDCOMPANYNAME"].ToString() + " hereby requests you to provide your BEST quotation for the following items to be delivered to our vessel");
        sbemailbody.AppendLine(dr["FLDVESSELNAME"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.Append("Request your IT department to kindly allow access to this URL for submitting quotes.");
        sbemailbody.AppendLine();
        sbemailbody.Append("Please click on the link below and key in the relevant fields indicated. If the link is wrapped, please copy and paste it on the address bar of your browser");
        sbemailbody.AppendLine();
        string url = "\"<" + Session["sitepath"] + "/" + dr["URL"].ToString();
        if (Filter.CurrentPurchaseStockType == null || Filter.CurrentPurchaseStockType.Equals(string.Empty))
            sbemailbody.AppendLine(url + "?SESSIONID=" + dr["SESSIONID"].ToString());
        else
            sbemailbody.AppendLine(url + "?SESSIONID=" + dr["SESSIONID"].ToString() + "&STOCKTYPE=" + Filter.CurrentPurchaseStockType + ">\"");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();

        if (dr["FLDEXPIRYDATE"].ToString() != "")
        {
            sbemailbody.AppendLine("We request you to submit your bid by");
            sbemailbody.Append(dr["FLDEXPIRYDATE"].ToString());
            sbemailbody.Append(", failing which your offer will NOT be accepted. If you wish to decline to bid, please advise us by email with your reasons for declining.");
        }
        else
        {
            sbemailbody.AppendLine("We request you to submit your bid, failing which your offer will NOT be accepted. If you wish to decline to bid, please advise us by email with your reasons for declining.");
        }

        sbemailbody.AppendLine();
        sbemailbody.Append("Note: In our continual effort to keep correct records of your address and contact information, we appreciate your time to verify and correct it where necessary. Please click on the link below to view/correct the address.");
        sbemailbody.AppendLine();

        DataSet dsvendorid = PhoenixPurchaseQuotation.EditQuotation(quotationid);
        DataRow drvendorid = dsvendorid.Tables[0].Rows[0];
        string vendorid = drvendorid["FLDVENDORID"].ToString();

        DataSet dsaddress = PhoenixRegistersAddress.EditAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(vendorid));
        DataRow draddress = dsaddress.Tables[0].Rows[0];

        sbemailbody.AppendLine("\n" + "\"<" + Session["sitepath"] + "/Purchase/PurchaseVendorAddressEdit.aspx?sessionid=" + draddress["FLDDTKEY"].ToString() + ">\"");

        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Thank you,");
        sbemailbody.AppendLine();
        sbemailbody.Append(dr["FLDUSERNAME"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("For and on behalf of " + dr["FLDCOMPANYNAME"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("(As Agents for Owners)");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Contact: " + frommailid);
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("This is an automated message.");
        sbemailbody.AppendLine("If you need personal attention, use \"reply all\" to get your communication across to an email id that is monitored.");
        sbemailbody.AppendLine("Please note " + ConfigurationManager.AppSettings.Get("FromMail").ToString() + " is NOT monitored.");

        return sbemailbody.ToString();

    }
    private void BulkDelete()
    {
    }    
    private void UpdateOrderQuantity(string orderlineid, string orderquantity,string budgetid,string ownerbudgetid)
    {
        try
        {
            PhoenixPurchaseOrderLine.UpdateOrderQuantity(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode, 
                General.GetNullableGuid(orderlineid), 
                General.GetNullableDecimal(orderquantity), 
                General.GetNullableInteger(budgetid), 
                General.GetNullableGuid(ownerbudgetid));
        }
        catch (Exception e)
        {
            ucError.ErrorMessage = e.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            rgvLine.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetTabHighlight()
    {
        if (ViewState["PAGEURL"].ToString().Contains("PurchaseFormItem.aspx"))
        {
            MenuLineItem.SelectedMenuIndex = 1;
        }
        if (ViewState["PAGEURL"].ToString().Contains("PurchaseFormItemComment.aspx"))
        {
            MenuLineItem.SelectedMenuIndex = 2;
        }
    }

    private void SetRowSelection()
    {
        rgvLine.SelectedIndexes.Clear();
        foreach(GridDataItem item in rgvLine.Items)
        {
            if (item.GetDataKeyValue("FLDORDERLINEID").ToString().Equals(ViewState["orderlineid"].ToString()))
            {
                rgvLine.SelectedIndexes.Add(item.ItemIndex);
                PhoenixPurchaseOrderLine.OrderLinePartNumber = (item["NUMBER"].FindControl("lblNumber") as RadLabel).Text;

                ViewState["DTKEY"] = item.GetDataKeyValue("FLDDTKEY").ToString();
            }
        }
    }

    protected void CheckBoxClicked(object sender, EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        //int nCurrentRow = Int32.Parse(cb.Text);
    }
    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }

    }
    public static string RemoveHTMLTags(string source)
    {
        HtmlGenericControl htmlDiv = new HtmlGenericControl("div");
        htmlDiv.InnerHtml = source;
        String plainText = htmlDiv.InnerText;

        return plainText;
    }

    protected void rgvLine_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //if (e.CommandName.ToUpper().Equals("SORT"))
            //    return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                int nCurrentRow = item.ItemIndex;
                PhoenixPurchaseOrderLine.DeleteOrderLine(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(item.GetDataKeyValue("FLDORDERLINEID").ToString()));
                ViewState["orderlineid"] = null;
            }

            if (e.CommandName.ToUpper().Equals("ROWCLICK") || e.CommandName.ToUpper().Equals("SELECT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                int nCurrentRow = item.ItemIndex;
                PhoenixPurchaseOrderLine.OrderLinePartNumber = (item["NUMBER"].FindControl("lblNumber") as RadLabel).Text;


                ViewState["orderlineid"] = item.GetDataKeyValue("FLDORDERLINEID").ToString();
                ViewState["DTKEY"] = item.GetDataKeyValue("FLDDTKEY").ToString();

                if (ViewState["PAGEURL"] != null && ViewState["PAGEURL"].ToString().Contains("CommonFileAttachment.aspx"))
                {
                    ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.PURCHASE;
                }
                else
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?orderid=" + ViewState["orderid"].ToString() + "&orderlineid=" + ViewState["orderlineid"].ToString();
            }
            else if (e.CommandName.ToUpper().Equals("SPARESEARCH"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                int nCurrentRow = item.ItemIndex;
                rgvLine.SelectedIndexes.Add(nCurrentRow);

                string lblRID = ((RadLabel)e.Item.FindControl("lblNumber")).Text;
                string lblVesselid = item.GetDataKeyValue("FLDVESSELID").ToString();
                //if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SPARE"))

                string scriptPopup = string.Format("javascript:openNewWindow('codehelp1','','Purchase/PurchaseSpareItemSearchAcrossVessel.aspx?ItemNo=" + lblRID + "&Vesselid=" + lblVesselid+ "');");
                RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptPopup, true);
            }
            //else if (e.CommandName.ToUpper().Equals("EDIT"))
            //{
            //    rgvLine.SelectedIndexes.Clear();
            //    PhoenixPurchaseOrderLine.OrderLinePartNumber = (item["NUMBER"].FindControl("lblNumber") as RadLabel).Text;

            //    ViewState["orderlineid"] = item.GetDataKeyValue("FLDORDERLINEID").ToString();
            //    ViewState["DTKEY"] = item.GetDataKeyValue("FLDDTKEY").ToString();

            //    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?orderid=" + ViewState["orderid"].ToString() + "&orderlineid=" + ViewState["orderlineid"].ToString();

            //    rgvLine.SelectedIndexes.Add(e.Item.ItemIndex);
            //}
            if (e.CommandName.ToUpper().Equals("REMARKS"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                int nCurrentRow = item.ItemIndex;
                rgvLine.SelectedIndexes.Add(nCurrentRow);
                string dtkey = item.GetDataKeyValue("FLDDTKEY").ToString();
                string lblVesselid = item.GetDataKeyValue("FLDVESSELID").ToString();
                //if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SPARE"))

                string scriptPopup = string.Format("javascript:parent.Openpopup('codehelp1','','../Common/CommonRemarks.aspx?dtkey=" + dtkey + "&Applncode=3');");
                RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptPopup, true);

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void rgvLine_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns;
        string[] alCaptions;

        if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SPARE") || Filter.CurrentPurchaseStockType.ToUpper().Equals("SERVICE"))
        {
            alColumns = new string[14] {"FLDSERIALNO", "FLDPARTNUMBER", "FLDMAKERREFERENCE","FLDDRAWINGNO", "FLDNAME", "FLDROBQUANTITY", "FLDREQUESTEDQUANTITY",
                                 "FLDUNITNAME", "FLDSPLITFORMNO", "FLDORDEREDQUANTITY", "FLDSUBACCOUNT", "FLDOWNERACCOUNT", "FLDQUICKNAME", "FLDCHKREMARKS" };
            alCaptions = new string[14] {"S. No.", "Number", "Maker Reference","Drawing No", "Name", "ROB", "Requested Qty",
                                 "Unit", "From Order", "Order Qty", "Budget Code", "Owner Budget Code", "Receipt Status", "Received Remarks" };
        }
        else
        {
            alColumns = new string[13] {"FLDSERIALNO", "FLDPARTNUMBER", "FLDMAKERREFERENCE", "FLDNAME", "FLDROBQUANTITY", "FLDREQUESTEDQUANTITY",
                                 "FLDUNITNAME", "FLDSPLITFORMNO", "FLDORDEREDQUANTITY", "FLDSUBACCOUNT", "FLDOWNERACCOUNT", "FLDQUICKNAME", "FLDCHKREMARKS" };
            alCaptions = new string[13] {"S. No.", "Number", "Product Code", "Name", "ROB", "Requested Qty",
                                 "Unit", "From Order", "Order Qty", "Budget Code", "Owner Budget Code", "Receipt Status", "Received Remarks" };
        }

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPurchaseOrderLine.OrderLineSearch(
            General.GetNullableGuid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection,
            sortexpression, sortdirection, rgvLine.CurrentPageIndex+1,
            rgvLine.PageSize, ref iRowCount, ref iTotalPageCount);

        
        if (ds.Tables[0].Rows.Count > 0)
        {
            
            if (ViewState["orderlineid"] == null)
            {
                ViewState["orderlineid"] = ds.Tables[0].Rows[0]["FLDORDERLINEID"].ToString();
                ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                rgvLine.SelectedIndexes.Add(0);

            }

            if (ViewState["PAGEURL"] == null)
            {
                ViewState["PAGEURL"] = "../Purchase/PurchaseFormItem.aspx";
            }

            if (ViewState["PAGEURL"].ToString().Contains("CommonFileAttachment.aspx"))
            {
                ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.PURCHASE;
            }
            else
            {
                if (ViewState["orderlineid"] != null)
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?orderid=" + ViewState["orderid"] + "&orderlineid=" + ViewState["orderlineid"].ToString();
                else
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?orderid=" + ViewState["orderid"].ToString();
            }
          
        }
        else
        {
            ViewState["PAGEURL"] = "../Purchase/PurchaseFormItem.aspx";
        }
        rgvLine.DataSource = ds;
        rgvLine.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("rgvLine", "Order Line Item - " + PhoenixPurchaseOrderForm.FormNumber, alCaptions, alColumns, ds);
    }
    protected void rgvLine_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            GridHeaderItem item = (GridHeaderItem)e.Item;
            if (Filter.CurrentPurchaseStockType == "STORE")
                item["MAKERREF"].Text = "Product Code";
        }
        if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            GridEditableItem item = e.Item as GridEditableItem;
            UserControlNumber txtOrderQtyEdit = (UserControlNumber)item["ORDERQTY"].FindControl("txtOrderQtyEdit");
            if (txtOrderQtyEdit != null)
                txtOrderQtyEdit.Text = drv["FLDORDEREDQUANTITY"].ToString();
        }
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            LinkButton cmddelete = (LinkButton)item["Action"].FindControl("cmdDelete");
            if (cmddelete != null)
            {
                cmddelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                cmddelete.Visible = SessionUtil.CanAccess(this.ViewState, cmddelete.CommandName);
            }
            LinkButton db = (LinkButton)item["Action"].FindControl("cmdEdit");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }



            RadLabel lblcomponentName = (RadLabel)item["NAME"].FindControl("lblComponentName");
            RadLabel lblcomponentid = (RadLabel)item["NAME"].FindControl("lblComponentId");
            
            if (Filter.CurrentPurchaseStockType.Equals("STORE"))
            {
                lblcomponentName.Visible = false;
            }
            RadLabel lblpartid = (RadLabel)item["NAME"].FindControl("lblPartId");
            LinkButton img = (LinkButton)item["ACTION"].FindControl("cmdDetail");
            RadLabel lblisitemdetails = (RadLabel)item["NAME"].FindControl("lblIsItemDetails");
            if (img != null)
            {
                if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SERVICE"))
                    img.Attributes.Add("onclick", "javascript:return openNewWindow('Component', '', 'Purchase/PurchaseServiceDetail.aspx?WORKORDERID=" + lblcomponentid.Text + "&VESSELID=" + item.GetDataKeyValue("FLDVESSELID") + "&STOCKTYPE=" + Filter.CurrentPurchaseStockType.ToString() + "'); return false;");
                else
                    img.Attributes.Add("onclick", "javascript:return openNewWindow('Component', '', 'Purchase/PurchaseSpareItemDetail.aspx?SPAREITEMID=" + item.GetDataKeyValue("FLDPARTID").ToString() + "&VESSELID=" + item.GetDataKeyValue("FLDVESSELID") + "&STOCKTYPE=" + Filter.CurrentPurchaseStockType.ToString() + "'); return false;");
                img.Visible = lblisitemdetails.Text.ToUpper().Equals("1") ? true : false;

            }

            RadLabel lblIsdefault = (RadLabel)item["NAME"].FindControl("lblIsFormNotes");
            ImageButton imgFlag = (ImageButton)item["ACTION"].FindControl("imgFlag");
            if (imgFlag != null)
                imgFlag.ImageUrl = !lblIsdefault.Text.ToUpper().Equals("") ? Session["images"] + "/detail-flag.png" : Session["images"] + "/spacer.gif";

            RadLabel lblIsContract = (RadLabel)item["NAME"].FindControl("lblIsContract");
            ImageButton imgContractFlag = (ImageButton)item["ACTION"].FindControl("imgContractFlag");
            if (imgContractFlag != null)
                imgContractFlag.ImageUrl = lblIsContract.Text.ToUpper().Equals("1") ? Session["images"] + "/contract-exist.png" : Session["images"] + "/spacer.gif";

            RadLabel lb2 = (RadLabel)item["RREMARKS"].FindControl("lblChkRemarks");
            LinkButton recimg = (LinkButton)item["RREMARKS"].FindControl("imgReceiptRemarks");
            if (recimg != null)
            {
                recimg.Attributes.Add("onclick", "javascript:showMoreInformation(ev, 'PurchaseFormItemReceiptRemarks.aspx?orderlineid=" + item.GetDataKeyValue("FLDORDERLINEID").ToString() + "&View=Y'); return false;");
                recimg.Visible = lb2.Text.ToUpper().Equals("1") ? true : false;
            }
            if (!Filter.CurrentPurchaseVesselSendDateSelection.ToUpper().Equals("") && !(Filter.CurrentVesselConfiguration.Equals("0") || Filter.CurrentVesselConfiguration == null))
            {
                //db = (LinkButton)e.Row.FindControl("cmdDelete");
                //if (db != null) db.Enabled = false;
                db = (LinkButton)item["ACTION"].FindControl("cmdEdit");
                if (db != null) db.Enabled = false;
            }

            //CheckBox cb = (CheckBox)e.Row.FindControl("chkSelect");
            //if (cb != null) cb.Attributes.Add("onclick", "e.cancelBubble = true; if(this.checked == false) { this.checked = false; } else {this.checked = true; }");
            LinkButton cs = (LinkButton)item["ACTION"].FindControl("cmdCriticalItem");
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 && cs != null && Filter.CurrentPurchaseStockType.ToUpper().Equals("SPARE"))
                cs.Visible = SessionUtil.CanAccess(this.ViewState, cs.CommandName);
            else if (cs != null)
                cs.Visible = false;


            LinkButton cmdSpareTransfer = (LinkButton)item["ACTION"].FindControl("cmdSpareTransfer");
            if (cmdSpareTransfer != null && Filter.CurrentPurchaseStockType == "SPARE")
            {
                cmdSpareTransfer.Attributes.Add("onclick", "javascript:return openNewWindow('Component', '', 'Purchase/PurchaseSpareItemTransfer.aspx?orderlineid=" + item.GetDataKeyValue("FLDORDERLINEID").ToString() + "'); return false;");
                cmdSpareTransfer.Visible = SessionUtil.CanAccess(this.ViewState, cmdSpareTransfer.CommandName);
            }

        }
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            GridDataItem item = (GridDataItem)e.Item;
            LinkButton cmdAudit = (LinkButton)item["ACTION"].FindControl("cmdAudit");
            if (cmdAudit != null)
            {
                cmdAudit.Visible = SessionUtil.CanAccess(this.ViewState, cmdAudit.CommandName);
                if (Filter.CurrentPurchaseStockType.ToUpper().Equals("STORE"))
                    cmdAudit.Attributes.Add("onclick", "openNewWindow('Audit', '', 'Common/CommonPhoenixAuditChanges.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&shortcode=PUR-FRMSTORE-LINE');return false;");
                else
                    cmdAudit.Attributes.Add("onclick", "openNewWindow('Audit', '', 'Common/CommonPhoenixAuditChanges.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&shortcode=PUR-FRM-LINE');return false;");
            }
            LinkButton cmdVendor = (LinkButton)item["ACTION"].FindControl("cmdVendor");
            if (cmdVendor != null)
            {
                cmdVendor.Visible = PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 && drv["FLDPARTID"].ToString() != "99999999-9999-9999-9999-999999999999" ? SessionUtil.CanAccess(this.ViewState, cmdVendor.CommandName) : false;
                if (cmdVendor.Visible == true)
                {
                    //cmdVendor.Visible = true;
                    if (Filter.CurrentPurchaseStockType.ToUpper().Equals("STORE"))
                        cmdVendor.Attributes.Add("onclick", "openNewWindow('Vendor', '', 'Inventory/InventoryStoreItemVendor.aspx?STOREITEMID=" + drv["FLDPARTID"].ToString() + "&VESSELID=" + drv["FLDVESSELID"].ToString() + "');return false;");
                    else
                        cmdVendor.Attributes.Add("onclick", "openNewWindow('Vendor', '', 'Inventory/InventorySpareItemVendor.aspx?SPAREITEMID=" + drv["FLDPARTID"].ToString() + "&VESSELID=" + drv["FLDVESSELID"].ToString() + "');return false;");
                }
            }

            LinkButton remarks = (LinkButton)item["ACTION"].FindControl("cmdRemarks");
            if (remarks != null)
            {
                remarks.Visible = drv["FLDREMARKSREQUIRED"].ToString() == "1" ? SessionUtil.CanAccess(this.ViewState, remarks.CommandName) : false;
            }

        }
    }
    protected void rgvLine_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem item = (GridEditableItem)e.Item;

        UpdateOrderQuantity(
            item.GetDataKeyValue("FLDORDERLINEID").ToString(),
            ((UserControlNumber)item["ORDERQTY"].FindControl("txtOrderQtyEdit")).Text.Replace("_", "0"),
            null,
            null
         );
        //rgvLine.Rebind();
    }
    protected void rgvLine_InsertCommand(object sender, GridCommandEventArgs e)
    {
    }
    protected void rgvLine_EditCommand(object sender, GridCommandEventArgs e)
    {
        rgvLine.SelectedIndexes.Clear();
        GridDataItem item = (GridDataItem)e.Item;
        PhoenixPurchaseOrderLine.OrderLinePartNumber = (item["NUMBER"].FindControl("lblNumber") as RadLabel).Text;

        ViewState["orderlineid"] = item.GetDataKeyValue("FLDORDERLINEID").ToString();
        ViewState["DTKEY"] = item.GetDataKeyValue("FLDDTKEY").ToString();

        ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?orderid=" + ViewState["orderid"].ToString() + "&orderlineid=" + ViewState["orderlineid"].ToString();

        rgvLine.SelectedIndexes.Add(e.Item.ItemIndex);
    }
    protected void rgvLine_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        rgvLine.Rebind();
    }
    protected void rgvLine_SortCommand(object sender, GridSortCommandEventArgs e)
    {
    }
}

