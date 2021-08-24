using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;
using Telerik.Web.UI;
using System.Configuration;
using System.Collections.Specialized;
using SouthNests.Phoenix.Reports;
using System.Web;
using System.Xml;
using System.IO;
public partial class PurchaseQuotationVendorDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Vessel Form", "FORMS", ToolBarDirection.Left);
            toolbarmain.AddButton("Vendor Quotations", "GENERAL", ToolBarDirection.Left);
            toolbarmain.AddButton("Line Items", "QTNITEM", ToolBarDirection.Left);
            toolbarmain.AddButton("Multiple Reqs", "MULTIPLEREQ", ToolBarDirection.Left);

            MenuVendor.AccessRights = this.ViewState;
            MenuVendor.MenuList = toolbarmain.Show();
            MenuVendor.SelectedMenuIndex = 1;
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            if (!IsPostBack)
            {
                //Title1.Text = "Quotation (  " + PhoenixPurchaseOrderForm.FormNumber + "     )";
                if (Request.QueryString["orderid"] != null)
                {
                    //ifMoreInfo.Attributes["src"] = "PurchaseQuotationVendor.aspx?orderid=" + Request.QueryString["orderid"].ToString();
                    ifMoreInfo.Attributes["src"] = "PurchaseQuotationVendor.aspx?orderid=" + Request.QueryString["orderid"].ToString();
                }
                else
                {
                    //ifMoreInfo.Attributes["src"] = "PurchaseQuotationVendor.aspx";
                    ifMoreInfo.Attributes["src"] = "PurchaseQuotationVendor.aspx";
                }

                ViewState["FormStatus"] = "";

                if (Request.QueryString["orderid"] != null)
                {
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                    if (Request.QueryString["pageno"] != null)
                    {
                        ViewState["pageno"] = Request.QueryString["pageno"].ToString();
                    }
                    else
                    {
                        ViewState["pageno"] = "0";
                    }

                }
                else
                {
                    ViewState["orderid"] = 0;
                }
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["quotationid"] = null;
                if (Request.QueryString["quotationid"] != null)
                {
                    ViewState["quotationid"] = Request.QueryString["quotationid"].ToString();
                }

            }
            short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseQuotationVendorDetail.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvVender')", "Print", "<i class=\"fas fa-print\"></i>", "Print");
            toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseQuotationVendorDetail.aspx", "Send Query", "<i class=\"fas fa-envelope\"></i>", "RFQ");
            toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseQuotationVendorDetail.aspx", "Send Reminder", "<i class=\"fas fa-bell\"></i>", "RFQREMAINDER");
            toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseQuotationVendorDetail.aspx", "Compare Quotations", "<i class=\"fab fa-quora\"></i>", "QTNCOMPARE");
            toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseQuotationVendorDetail.aspx", "Send  Approval", "<i class=\"fas fa-user-md\"></i>", "SENDAPPROVAL");
            if (showcreditnotedisc == 1)
                toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseQuotationVendorDetail.aspx", "Quotation Compare Report", "<i class=\"fas fa-clipboard\"></i>", "QUOTATIONCOMPARE");
            toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseQuotationVendorDetail.aspx", "Create PO", "<i class=\"fas fa-file-alt\"></i>", "ORDER");
            //toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseQuotationVendorDetail.aspx", "Add Multiple Vendor", "<i class=\"fas fa-plus\"></i>", "MULTIPLEVENDOR");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','Purchase/PurchaseMultipleVendorSelect.aspx?orderid=" + ViewState["orderid"].ToString() + "&addresstype=132,130,131,128');return false;", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseQuotationVendorDetail.aspx", "Submit to Supdt", "<i class=\"fas fa-arrow-alt-circle-right\"></i>", "SUBMITTOSUPDT");
            toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseQuotationVendorDetail.aspx", "Verify", "<i class=\"fas fa-check\"></i>", "VERIFY");

            MenuVendorList.AccessRights = this.ViewState;
            MenuVendorList.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {



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
        if (PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount == 1)
        {
            alColumns = new string[]{"FLDVENDORID", "FLDNAME","FLDRECEIVEDDATE","FLDTOTALPRICE","FLDTOTALDISCOUNT","TOTALAMOUNT",
                                 "FLDDELIVERYTIME","STATUS" };
            alCaptions = new string[]{"Vendor ID", "Vendor","Received Date","Quoted Price","Discount","Total Amount",
                                 "Delivery Time","Quoted" };
        }
        else
        {
            alColumns = new string[] {"FLDVENDORID", "FLDNAME","FLDRECEIVEDDATE","FLDTOTALPRICE",
                                 "FLDDELIVERYTIME","STATUS" };
            alCaptions = new string[]{"Vendor ID", "Vendor","Received Date","Quoted Price",
                                 "Delivery Time","Quoted" };
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

        ds = PhoenixPurchaseQuotation.QuotationSearch(General.GetNullableGuid(ViewState["orderid"].ToString()), sortexpression, sortdirection, 1, iRowCount, ref iRowCount, ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=VendorList - " + PhoenixPurchaseOrderForm.FormNumber + ".xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Quotation Vendor List</h3></td>");
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
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void MenuVendorList_TabStripCommand(object sender, EventArgs e)
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
            if (CommandName.ToUpper().Equals("RFQ"))
            {
                if (ViewState["FormStatus"] != null && ViewState["FormStatus"].ToString() != "" && ViewState["FormStatus"].ToString() == "53")
                {
                    SendForQuotation();
                    rgvQuotation.Rebind();
                }


                else
                {
                    ucError.ErrorMessage = "You cannot send RFQ. Requisiton is not in Active status.";
                    ucError.Visible = true;
                    return;
                }

            }
            if (CommandName.ToUpper().Equals("RFQREMAINDER"))
            {
                SendRemindorForQuotation();
            }
            if (CommandName.ToUpper().Equals("QTNCOMPARE"))
            {
                if (ViewState["orderid"] != null)
                {
                    string selectedvendors = ",";
                    foreach (GridDataItem item in rgvQuotation.Items)
                    {
                        if (item["CHECKBOX"].FindControl("chkSelect") != null && ((RadCheckBox)(item["CHECKBOX"].FindControl("chkSelect"))).Checked == true)
                        {
                            selectedvendors = selectedvendors + ((RadLabel)(item["FLAG"].FindControl("lblQuotationId"))).Text + ",";
                        }
                    }

                    if (selectedvendors.Length > 1)
                        //ifMoreInfo.Attributes["src"] = "PurchaseQuotationComparison.aspx?orderid=" + ViewState["orderid"].ToString() + "&vendors=" + selectedvendors;
                        //ifMoreInfo.Attributes["src"] = "PurchaseQuotationComparison.aspx?orderid=" + ViewState["orderid"].ToString() + "&vendors=" + selectedvendors;
                        Response.Redirect("../Purchase/PurchaseQuotationComparison.aspx?orderid=" + ViewState["orderid"].ToString() + "&vendors=" + selectedvendors);
                    else
                    {
                        ucError.ErrorMessage = "There are no quotations to compare.";
                        ucError.Visible = true;
                    }
                }
                else
                {
                    //ifMoreInfo.Attributes["src"] = "PurchaseQuotationComparison.aspx";
                    ifMoreInfo.Attributes["src"] = "PurchaseQuotationComparison.aspx";
                }

            }
            if (CommandName.ToUpper().Equals("ORDER"))
            {

                if (!OrderApprove())
                {
                    ucError.Visible = true;
                    return;
                }
                InsertOrderFormHistory();

                RadWindowManager1.Localization.OK = "Hide PO Amount";
                RadWindowManager1.Localization.Cancel = "Show PO Amount";
                RadWindowManager1.RadConfirm("Select which detail to be shown in PO", "order", 400, 100, null, "Create PO");

                //ucConfirmMessage.Visible = true;
                //ucConfirmMessage.Text = "Select which detail to be shown in PO";

            }
            if (CommandName.ToUpper().Equals("SENDAPPROVAL"))
            {
                SendApproval();
            }

            if (CommandName.ToUpper().Equals("QUOTATIONCOMPARE"))
            {
                quotationcomparereport();
            }
            else if (CommandName.ToUpper().Equals("SUBMITTOSUPDT"))
            {
                if (ViewState["orderid"] != null && General.GetNullableGuid(ViewState["orderid"].ToString()) != null)
                    PhoenixPurchaseQuotation.SubmitToSupdt(new Guid(ViewState["orderid"].ToString()));
            }
            if (CommandName.ToUpper().Equals("VERIFY"))
            {
                if (ViewState["orderid"] != null && General.GetNullableGuid(ViewState["orderid"].ToString()) != null)
                    PhoenixPurchaseFalRules.PurchaseManagerVerification(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["orderid"].ToString()));
                radnotificationstatus.Show("Quotation verified.");

            }
            //if (CommandName.ToUpper().Equals("MULTIPLEVENDOR"))
            //{
            //    String scriptpopup = String.Format(
            //                "javascript:openNewWindow('codehelp1', '', 'Purchase/PurchaseMultipleVendorSelect.aspx?orderid=" + ViewState["orderid"].ToString() + "&addresstype=132,130,131,128');");
            //    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void quotationcomparereport()
    {
        try
        {
            string selectedvendors = ",";

            foreach (GridDataItem item in rgvQuotation.Items)
            {
                if (item["CHECKBOX"].FindControl("chkSelect") != null && ((RadCheckBox)(item["CHECKBOX"].FindControl("chkSelect"))).Checked == true)
                {
                    selectedvendors = selectedvendors + ((RadLabel)(item["FLAG"].FindControl("lblQuotationId"))).Text + ",";
                }
            }
            if (selectedvendors.Length <= 1)
                selectedvendors = null;

            String scriptpopup = String.Format(
                            "javascript:Openpopup('codehelp1', '', 'PurchaseQuotationComparePrint.aspx?quotationid=" + selectedvendors + "');");
            RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SendApproval()
    {
        try
        {
            string selectedvendors = ",";
            bool quoted = false;
            foreach (GridDataItem item in rgvQuotation.Items)
            {
                if (item["CHECKBOX"].FindControl("chkSelect") != null && ((RadCheckBox)(item["CHECKBOX"].FindControl("chkSelect"))).Checked == true)
                {
                    selectedvendors = selectedvendors + ((RadLabel)(item["FLAG"].FindControl("lblQuotationId"))).Text + ",";
                }
                if (item["FLAG"].FindControl("lblSTATUS") != null && (((RadLabel)(item["FLAG"].FindControl("lblSTATUS"))).Text.ToString().ToUpper() == "PARTIAL" || ((RadLabel)(item["FLAG"].FindControl("lblSTATUS"))).Text.ToString().ToUpper() == "FULL"))
                    quoted = true;
            }
            if (selectedvendors.Length <= 1)
                selectedvendors = null;

            if (!quoted && !string.IsNullOrEmpty(selectedvendors))
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = "you can not send for approval before receive the quotation from vendor.";
                ucError.Visible = true;
                return;
            }

            PhoenixPurchaseQuotation.SendApprovalInsert
                    (PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["orderid"].ToString()), selectedvendors, 1);

            DataSet dsapprovalsend = PhoenixPurchaseQuotation.SendApprovalMail(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["orderid"].ToString()));

            if (dsapprovalsend.Tables[0].Rows.Count > 0)
            {
                string emailbodytext1 = "";
                emailbodytext1 = PreparePurchaseApprovalSendText(dsapprovalsend.Tables[0]);
                DataRow dr = dsapprovalsend.Tables[0].Rows[0];
                PhoenixMail.SendMail(dr["FLDSUPTEMAIL"].ToString().Replace(";", ",").TrimEnd(','),
                    dr["FLDSUPPLIEREMAIL"].ToString().Replace(";", ",").TrimEnd(','),
                    null,
                    "APPROVAL AWAITED -" + " " + dr["FLDFORMNO"].ToString() + " - " + dr["FLDTITLE"].ToString(),
                    emailbodytext1,
                    true,
                    System.Net.Mail.MailPriority.Normal,
                    "",
                    null,
                    null);
                ucConfirm.ErrorMessage = "Awaited PO approval Email Sent.";
                ucConfirm.Visible = true;

            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void rgvQuotation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns;
        string[] alCaptions;
        if (PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount == 1)
        {
            alColumns = new string[]{"FLDVENDORID", "FLDNAME","FLDRECEIVEDDATE","FLDTOTALPRICE","FLDTOTALDISCOUNT","TOTALAMOUNT",
                                 "FLDDELIVERYTIME","STATUS" };
            alCaptions = new string[]{"Vendor ID", "Vendor","Received Date","Quoted Price","Discount","Total Amount",
                                 "Delivery Time","Quoted" };
        }
        else
        {
            alColumns = new string[] {"FLDVENDORID", "FLDNAME","FLDRECEIVEDDATE","FLDTOTALPRICE",
                                 "FLDDELIVERYTIME","STATUS" };
            alCaptions = new string[]{"Vendor ID", "Vendor","Received Date","Quoted Price",
                                 "Delivery Time","Quoted" };
        }
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPurchaseQuotation.QuotationSearch(General.GetNullableGuid(ViewState["orderid"].ToString()), sortexpression, sortdirection, rgvQuotation.CurrentPageIndex + 1, rgvQuotation.PageSize, ref iRowCount, ref iTotalPageCount);
        rgvQuotation.DataSource = ds;
        rgvQuotation.VirtualItemCount = iRowCount;



        if (ds.Tables[0].Rows.Count > 0)
        {

            ViewState["FormStatus"] = ds.Tables[0].Rows[0]["FLDFORMSTATUS"].ToString();
            if (ViewState["quotationid"] == null || ViewState["quotationid"].ToString() == "0")
            {
                ViewState["quotationid"] = ds.Tables[0].Rows[0]["FLDQUOTATIONID"].ToString();
                ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                ViewState["SEAPROCFILENAME"] = ds.Tables[0].Rows[0]["FLDSEAPROCFILENAME"].ToString();
            }
            ifMoreInfo.Attributes["src"] = "PurchaseQuotationVendor.aspx?orderid=" + ViewState["orderid"].ToString() + "&quotationid=" + ViewState["quotationid"].ToString();
        }
        else
        {
            //ifMoreInfo.Attributes["src"] = "PurchaseQuotationVendor.aspx?orderid=" + ViewState["orderid"].ToString();
            ifMoreInfo.Attributes["src"] = "PurchaseQuotationVendor.aspx?orderid=" + ViewState["orderid"].ToString();
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            ViewState["SeaProcRFQEnable"] = ds.Tables[1].Rows[0]["FLDPATH"].ToString();
        }
        if (ds.Tables[2].Rows.Count > 0)
        {
            ViewState["SEAPROCFOLDERPATH"] = ds.Tables[2].Rows[0]["FLDFOLDERPATH"].ToString();
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvVender", "Purchase Vendor List - " + PhoenixPurchaseOrderForm.FormNumber, alCaptions, alColumns, ds);
    }

    private void SetRowSelection()
    {
        foreach (GridDataItem item in rgvQuotation.Items)
        {
            if (ViewState["quotationid"] != null && General.GetNullableGuid(ViewState["quotationid"].ToString()) != null && item.GetDataKeyValue("FLDQUOTATIONID").ToString().Equals(ViewState["quotationid"].ToString()))
            {
                rgvQuotation.SelectedIndexes.Add(item.ItemIndex);
                //ifMoreInfo.Attributes["src"] = "PurchaseQuotationVendor.aspx?orderid=" + ViewState["orderid"].ToString() + "&quotationid=" + ViewState["quotationid"].ToString();
            }
        }

        short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
        rgvQuotation.MasterTableView.GetColumn("DISCOUNT").Visible = (showcreditnotedisc == 1) ? true : false;
        rgvQuotation.MasterTableView.GetColumn("TOTAL").Visible = (showcreditnotedisc == 1) ? true : false;
    }

    protected void rgvQuotation_SortCommand(object sender, GridSortCommandEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        rgvQuotation.Rebind();
    }

    protected void rgvQuotation_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                PhoenixPurchaseQuotation.DeleteQuotation(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(item.GetDataKeyValue("FLDQUOTATIONID").ToString()));
                ViewState["quotationid"] = "0";
            }
            else if (e.CommandName.ToUpper().Equals("SELECTVENDOR"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string port = "";
                string vendor = "";
                RadLabel lblport = (RadLabel)item["FLAG"].FindControl("lblPortId");
                RadLabel lblvendor = (RadLabel)item["FLAG"].FindControl("lblVendorId");

                if (lblport != null)
                    port = lblport.Text;
                if (lblvendor != null)
                    vendor = lblvendor.Text;


                DataSet ds = PhoenixPurchaseQuotation.QuotationsValidate(General.GetNullableGuid(ViewState["orderid"].ToString()), General.GetNullableGuid(item.GetDataKeyValue("FLDQUOTATIONID").ToString()));
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr["FLDRFQLESSTHAN3VENDOR"].ToString() == "1" || dr["FLDSELECTEDHIGHERQUOTE"].ToString() == "1" || dr["FLDOEMREASON"].ToString() == "0")
                {

                    String scriptpopup = String.Format("javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Purchase/PurchaseQuotationReason.aspx?orderid=" + ViewState["orderid"].ToString() + "&quoationid=" + item.GetDataKeyValue("FLDQUOTATIONID").ToString() + "&minvendor=" + dr["FLDRFQLESSTHAN3VENDOR"].ToString() + "&higquote=" + dr["FLDSELECTEDHIGHERQUOTE"].ToString() + "&configquote=" + dr["FLDCONFIGUREDQUOTES"].ToString() + "&OEM=" + dr["FLDOEMREASON"].ToString() + "');");
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
                else
                {
                    UpdateSelectVendorForPo(item.GetDataKeyValue("FLDQUOTATIONID").ToString());
                    InsertOrderFormHistory();


                    if (Filter.CurrentPurchaseStockType != null && Filter.CurrentPurchaseStockType == "STORE")
                    {

                        if (General.GetNullableInteger(port) != null && General.GetNullableInteger(vendor) != null)
                        {
                            DataTable dt = PhoenixPurchaseQuotation.ValidatePriorityVendor(General.GetNullableGuid(ViewState["orderid"].ToString())
                                                             , General.GetNullableGuid(item.GetDataKeyValue("FLDQUOTATIONID").ToString())
                                                             , Filter.CurrentPurchaseVesselSelection
                                                             , int.Parse(port)
                                                             , int.Parse(vendor)
                                                             );

                            if (dt.Rows.Count > 0)
                            {
                                DataRow drRow = dt.Rows[0];
                                string EmailTo1 = drRow["FLDEMAILTO1"].ToString() + "," + drRow["FLDEMAILTO2"].ToString();
                                string EmailCC = drRow["FLDEMAILCC"].ToString();
                                string Title = drRow["FLDVESSELNAME"].ToString() + " - Quotation Review for " + drRow["FLDFORMNO"].ToString() + "" + (drRow["FLDTITLE"].ToString() == "" ? "" : "-") + drRow["FLDTITLE"].ToString()
                                + "" + (drRow["FLDSEAPORTNAME"].ToString() == "" ? "" : "-") + drRow["FLDSEAPORTNAME"].ToString();

                                //string Emailbodytext = "Preferred Vendor is not selected. Kindly review the Quotation.";
                                StringBuilder sbemailbody = new StringBuilder();
                                sbemailbody.Append("This is an automated message. DO NOT REPLY to " + ConfigurationManager.AppSettings.Get("FromMail").ToString() + ". Kindly use the \"reply all\" button if you are responding to this message.");
                                sbemailbody.AppendLine();
                                sbemailbody.AppendLine();
                                sbemailbody.Append("Dear Sir");
                                sbemailbody.AppendLine();
                                sbemailbody.AppendLine();
                                sbemailbody.AppendLine("Preferred Vendor is not selected. Kindly review the Quotation.");
                                sbemailbody.AppendLine();
                                sbemailbody.AppendLine();
                                sbemailbody.AppendLine("Thank you,");
                                sbemailbody.AppendLine();
                                sbemailbody.Append(PhoenixSecurityContext.CurrentSecurityContext.FirstName.ToString() + " " + General.GetNullableString(PhoenixSecurityContext.CurrentSecurityContext.LastName) != null ? PhoenixSecurityContext.CurrentSecurityContext.LastName : "");
                                sbemailbody.AppendLine();


                                PhoenixCommoneProcessing.PrepareEmailMessage
                                    (EmailTo1, "RFQ", new Guid(ViewState["quotationid"].ToString()), "", EmailCC, null, Title, sbemailbody.ToString(), "", "");
                            }



                            //Sending mail for quotation review end
                        }

                    }


                    rgvQuotation.Rebind();
                }

            }
            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                ViewState["quotationid"] = item.GetDataKeyValue("FLDQUOTATIONID").ToString();
                ViewState["DTKEY"] = item.GetDataKeyValue("FLDDTKEY").ToString();
                rgvQuotation.SelectedIndexes.Add(item.ItemIndex);
                ifMoreInfo.Attributes["src"] = "PurchaseQuotationVendor.aspx?orderid=" + ViewState["orderid"].ToString() + "&quotationid=" + ViewState["quotationid"].ToString();

            }
            else if (e.CommandName.ToUpper().Equals("DESELECT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                DeSelectVendor(item.GetDataKeyValue("FLDQUOTATIONID").ToString());
                //InsertOrderFormHistory();         
                rgvQuotation.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("REQUOTE"))
            {
            }
            else if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                try
                {
                    GridDataItem item = (GridDataItem)e.Item;

                    string quotationid = item.GetDataKeyValue("FLDQUOTATIONID").ToString();
                    PhoenixCommonPurchase.UpdateQuotationApproval(new Guid(quotationid.ToString()), 0);
                    radnotificationstatus.Show("Approved");
                    //ucStatus.Visible = true;
                    if (Session["POQAPPROVE"] != null && ((DataSet)Session["POQAPPROVE"]).Tables.Count > 0)
                    {
                        DataSet ds = (DataSet)Session["POQAPPROVE"];
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string emailbodytext = PrepareApprovalText(ds.Tables[0], 0);
                            DataRow dr = ds.Tables[0].Rows[0];
                            PhoenixMail.SendMail(dr["FLDEMAIL"].ToString().Replace(";", ",").TrimEnd(','),
                                dr["FLDFROMEMAIL"].ToString().Replace(";", ",").TrimEnd(','),
                                null,
                                dr["FLDSUBJECT"].ToString() + "     " + dr["FLDFORMNO"].ToString(),
                                emailbodytext,
                                true,
                                System.Net.Mail.MailPriority.Normal,
                                "",
                                null,
                                null);
                        }
                        Session["POQAPPROVE"] = null;
                    }
                    rgvQuotation.Rebind();

                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
            }
            else if (e.CommandName.ToUpper().Equals("DEAPPROVE"))
            {
                try
                {
                    GridDataItem item = (GridDataItem)e.Item;

                    DataSet dsApproval = PhoenixPurchaseQuotation.QuotationApprovalEdit(
                        new Guid(ViewState["orderid"].ToString()),
                        new Guid(item.GetDataKeyValue("FLDQUOTATIONID").ToString()));

                    ViewState["QuotationId"] = item.GetDataKeyValue("FLDQUOTATIONID").ToString();
                    ViewState["lblIsApproved"] = ((RadLabel)item["FLAG"].FindControl("lblIsApproved")).Text;

                    if (dsApproval.Tables.Count > 0 && dsApproval.Tables[0].Rows.Count > 0)
                    {
                        DataRow drApproval = dsApproval.Tables[0].Rows[0];
                        int PurchaseFALYN = 0;
                        PhoenixPurchaseFalRules.PurchaseFALYN(General.GetNullableGuid(ViewState["orderid"].ToString()), 1, int.Parse(drApproval["FLDVESSELID"].ToString()), ",PUR-FAL,", ref PurchaseFALYN);
                        if (PurchaseFALYN == 1)
                        {
                            PhoenixPurchaseFalApprovalLevel.RevokeAccess(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["QuotationId"].ToString()));
                        }
                        if (drApproval["FLDPARTPAID"].ToString().Equals("1"))

                        {
                            RadWindowManager1.Localization.OK = "Yes";
                            RadWindowManager1.Localization.Cancel = "No";
                            RadWindowManager1.RadConfirm("Advance Payment already been made,are you sure this PO is to be revoked?", "revokeconfirm", 400, 150, null, "Confirm");
                            return;
                        }
                        else
                        {
                            if (drApproval["FLDFULLAPPROVAL"].ToString().Equals("1"))
                            {
                                string emailbodytext = "";

                                DataSet ds = PhoenixPurchaseOrderForm.ApproveOrderForm(
                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["orderid"].ToString())
                                    , General.GetNullableGuid(item.GetDataKeyValue("FLDQUOTATIONID").ToString())
                                    , General.GetNullableDateTime(DateTime.Now.ToString()),
                                    Int32.Parse(((RadLabel)item["FLAG"].FindControl("lblIsApproved")).Text));

                                if (Int32.Parse(((RadLabel)item["FLAG"].FindControl("lblIsApproved")).Text) == 1)
                                {
                                    PhoenixPurchaseOrderForm.OrderFormDeletePurchaseBudgetCommitment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["orderid"].ToString()));
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in ds.Tables[0].Rows)
                                        {
                                            PhoenixCommonBudgetGroupAllocation.UpdateBudgetCommittedPaidAmount(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(dr["FLDVESSELID"].ToString())
                                               , int.Parse(dr["FLDBUDGETCODE"].ToString()), Convert.ToDateTime(dr["FLDCREATEDDATE"].ToString()), decimal.Parse(dr["FLDCOMMITEDAMOUNTLOCAL"].ToString())
                                               , 0, char.Parse("D"), dr["FLDFORMNO"].ToString(), ViewState["orderid"].ToString(), General.GetNullableInteger(dr["FLDCURRENCY"].ToString()), decimal.Parse(dr["FLDCOMMITEDAMOUNTUSD"].ToString())
                                               , null, null, General.GetNullableDateTime(DateTime.Now.ToString("yyyy/MM/dd")), null, 583, General.GetNullableInteger(dr["FLDVENDORID"].ToString()), General.GetNullableInteger(dr["FLDACCOUNTID"].ToString()), "Approval Revoked"); // bug id 12326 reason for reversal
                                        }
                                    }
                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        emailbodytext = PrepareApprovalText(ds.Tables[1], 1);
                                        DataRow dr = ds.Tables[1].Rows[0];
                                        PhoenixMail.SendMail(dr["FLDEMAIL"].ToString().Replace(";", ",").TrimEnd(','),
                                            dr["FLDFROMEMAIL"].ToString().Replace(";", ",").TrimEnd(','),
                                            null,
                                            dr["FLDSUBJECT"].ToString() + "     " + PhoenixPurchaseOrderForm.FormNumber,
                                            emailbodytext,
                                            true,
                                            System.Net.Mail.MailPriority.Normal,
                                            "", null,
                                            null);
                                        PhoenixPurchaseFalApprovalLevel.QuotationRevoke(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(item.GetDataKeyValue("FLDQUOTATIONID").ToString()), General.GetNullableGuid(ViewState["orderid"].ToString()));

                                    }
                                    //RadWindowManager1.RadAlert("Purchase approval is cancelled.", 200, 100, "Revoke","");
                                    ucConfirm.ErrorMessage = "Purchase approval is cancelled.";
                                }
                                //ucConfirm.Visible = true;
                                rgvQuotation.Rebind();
                            }
                            else if (drApproval["FLDAPPROVALEXISTS"].ToString().Equals("1"))
                            {
                                PhoenixPurchaseQuotation.DeletePartialApproval(
                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                    new Guid(item.GetDataKeyValue("FLDQUOTATIONID").ToString()));
                                PhoenixPurchaseFalApprovalLevel.QuotationRevoke(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(item.GetDataKeyValue("FLDQUOTATIONID").ToString()), General.GetNullableGuid(ViewState["orderid"].ToString()));

                                ucConfirm.ErrorMessage = "Purchase approval is cancelled.";
                                //RadWindowManager1.RadAlert("Purchase approval is cancelled.", 200, 100, "Revoke", "");
                                //ucConfirm.Visible = true;
                                rgvQuotation.Rebind();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
            }
            else if (e.CommandName.ToUpper().Equals("SENDACCEPTANCE"))
            {
                try
                {

                    GridDataItem item = (GridDataItem)e.Item;
                    RadLabel lblVendorId = (RadLabel)item["FLAG"].FindControl("lblVendorId");
                    RadLabel lblQuotationId = (RadLabel)item["FLAG"].FindControl("lblQuotationId");

                    DataSet ds;
                    ds = PhoenixPurchaseOrderForm.ListOrderformAcceptancemail(new Guid(ViewState["orderid"].ToString()), int.Parse(lblVendorId.Text));
                    DataRow dr = ds.Tables[0].Rows[0];

                    if (General.GetNullableInteger(dr["FLDACCEPTANCEMAILSENTYN"].ToString()) != 1)
                    {
                        StringBuilder mailbody = new StringBuilder();
                        string toEmail = dr["FLDVENDORMAIL"].ToString();
                        string ccEmail = dr["FLDSUPTEMAIL"].ToString().Trim() + dr["FLDSUPTEMAIL"].ToString().Trim() != "" ? "," : "" + dr["FLDPERSONALOFFICEREMAIL"].ToString().Trim();
                        string vesselname = dr["FLDVESSELNAME"].ToString();

                        string subject = vesselname + "- Quotation Acceptance'";

                        mailbody.AppendLine(PrepareEmailBodyTextForAcceptance(new Guid(lblQuotationId.Text), dr["FLDVESSELNAME"].ToString()));

                        PhoenixCommoneProcessing.PrepareEmailMessage(toEmail.Replace(";", ",").TrimEnd(','),
                                                                    "ACT",
                                                                     new Guid(ViewState["orderid"].ToString()),
                                                                     "", ccEmail.Replace(";", ",").TrimEnd(','),
                                                                     null,
                                                                     subject,
                                                                     mailbody.ToString(),
                                                                     "",
                                                                     "");


                        PhoenixPurchaseOrderForm.UpdateOrderformAcceptancemail(
                                       new Guid(ViewState["orderid"].ToString())
                                       , int.Parse(dr["FLDVESSELID"].ToString())
                                       );

                        ucError.HeaderMessage = "Mail sent.";
                        ucError.Visible = true;
                        return;
                    }
                    else
                    {
                        ucError.HeaderMessage = "Acceptance Mail already sent.";
                        ucError.Visible = true;
                        return;
                    }
                }

                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }

            }
            else if (e.CommandName.ToUpper().Equals("PDF"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string Tmpfilelocation = string.Empty; string[] reportfile = new string[0];
                Guid quotationid = new Guid(item.GetDataKeyValue("FLDQUOTATIONID").ToString());
                Guid orderid = new Guid(item.GetDataKeyValue("FLDORDERID").ToString());


                DataSet ds = PhoenixPurchaseOrderForm.PurchaseOrderProvisionalService(quotationid, orderid);

                if (ds.Tables[0].Rows.Count > 0 && PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
                {

                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("applicationcode", "3");
                    nvc.Add("reportcode", "PROVISIONALSERVICE");
                    nvc.Add("CRITERIA", "");
                    Session["PHOENIXREPORTPARAMETERS"] = nvc;

                    Tmpfilelocation = System.Web.HttpContext.Current.Request.MapPath("~/");
                    string filename = "ProvisionalServiceOrder.pdf";
                    Tmpfilelocation = HttpContext.Current.Server.MapPath("../Attachments/TEMP/" + filename);

                    PhoenixPurchaseOrderForm.LoadImage(ds);
                    PhoenixSSRSReportClass.ExportSSRSReport(reportfile, ds, PhoenixSSRSReportClass.ExportFileFormat.PDF, null, ref Tmpfilelocation);
                    Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + Tmpfilelocation + "&type=pdf");
                }
            }
            else if (e.CommandName.ToUpper().Equals("RFQXML"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                ViewState["FLDVESSELID"] = ((RadLabel)item["FLAG"].FindControl("lblVesselId")).Text;
                ViewState["VendorId"] = ((RadLabel)item["FLAG"].FindControl("lblVendorId")).Text;
                try
                {
                    RadWindowManager1.RadConfirm("Are you sure you want to Export RFQ to SeaProc?", "ConfirmRFQXML", 320, 150, null, "Confirm");
                    return;
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
            else if (e.CommandName.ToUpper().Equals("POXML"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                ViewState["FLDVESSELID"] = ((RadLabel)item["FLAG"].FindControl("lblVesselId")).Text;
                ViewState["VendorId"] = ((RadLabel)item["FLAG"].FindControl("lblVendorId")).Text;
                try
                {
                    RadWindowManager1.RadConfirm("Are you sure you want to Export PO to SeaProc?", "ConfirmPOXML", 320, 150, null, "Confirm");
                    return;
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
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected string PrepareApprovalText(DataTable dt, int approved)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataRow dr = dt.Rows[0];
        sbemailbody.Append("<pre>");
        sbemailbody.AppendLine();
        sbemailbody.Append("Dear Purchaser");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        if (approved == 1)
            sbemailbody.AppendLine("Purchase approval is cancelled.");
        else
            sbemailbody.AppendLine("Purchase order is approved");

        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Thank you,");
        sbemailbody.AppendLine();
        sbemailbody.Append(dr["FLDAPPROVEDBY"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("For and on behalf of " + ConfigurationManager.AppSettings.Get("companyname").ToString());
        sbemailbody.AppendLine();

        return sbemailbody.ToString();

    }

    protected string PreparePurchaseApprovalSendText(DataTable dt)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataRow dr = dt.Rows[0];
        sbemailbody.Append("<pre>");
        sbemailbody.AppendLine();
        sbemailbody.Append("Dear Mr.Superintendent,");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Approval awaited for the following requisition");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine(dr["FLDFORMNO"].ToString() + " - " + dr["FLDTITLE"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Thank you,");
        sbemailbody.AppendLine();
        sbemailbody.Append(dr["FLDSENDBY"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("For and on behalf of " + ConfigurationManager.AppSettings.Get("companyname").ToString());
        sbemailbody.AppendLine();

        return sbemailbody.ToString();

    }

    protected void rgvQuotation_DeleteCommand(object sender, GridCommandEventArgs de)
    {
        try
        {
            rgvQuotation.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void rgvQuotation_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            GridDataItem item = (GridDataItem)e.Item;

            if (ViewState["quotationid"] != null && General.GetNullableGuid(ViewState["quotationid"].ToString()) != null && item.GetDataKeyValue("FLDQUOTATIONID").ToString().Equals(ViewState["quotationid"].ToString()))
            {
                rgvQuotation.SelectedIndexes.Add(item.ItemIndex);
            }

            LinkButton db = (LinkButton)item["ACTION"].FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
            if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;

            LinkButton cmdVendor = (LinkButton)item["ACTION"].FindControl("cmdVendor");
            RadLabel lblVendorId = (RadLabel)item["FLAG"].FindControl("lblVendorId");
            RadLabel lblQuotationId = (RadLabel)item["FLAG"].FindControl("lblQuotationId");
            LinkButton cmdExcelRFQ = (LinkButton)item["ACTION"].FindControl("cmdExcelRFQ");

            string jvscript = "javascript:parent.openNewWindow('codehelp1','','Purchase/PurchaseExport2XL.aspx?quotationid=" + lblQuotationId.Text + "'); return false;";

            if (cmdExcelRFQ != null)
            {
                cmdExcelRFQ.Attributes.Add("onclick", jvscript);
                //cmdExcelRFQ.Visible = SessionUtil.CanAccess(this.ViewState, cmdExcelRFQ.CommandName);
                cmdExcelRFQ.Visible = false;
            }

            cmdVendor.Attributes.Add("onclick", "openNewWindow('AddAddress', '', 'Registers/Registersoffice.aspx?addresscode=" + lblVendorId.Text + "&VIEWONLY=Y'); return false;");
            cmdVendor.Visible = SessionUtil.CanAccess(this.ViewState, cmdVendor.CommandName);

            LinkButton cmdViewQuery = (LinkButton)item["ACTION"].FindControl("cmdViewQuery");
            RadLabel lblRemarksflag = (RadLabel)item["FLAG"].FindControl("lblremarksflag");
            RadLabel lblFormType = (RadLabel)item["FLAG"].FindControl("lblFormType");
            RadLabel lblWebSession = (RadLabel)item["FLAG"].FindControl("lblWebSession");
            ImageButton imgRequote = (ImageButton)item["ACTION"].FindControl("imgRequote");

            ImageButton imgdetailsFlag = (ImageButton)item["FLAG"].FindControl("imgdetailsFlag");
            imgdetailsFlag.Visible = lblRemarksflag.Text.ToUpper().Equals("1") ? true : false;

            if (imgRequote != null)
                imgRequote.Attributes.Add("onclick", "openNewWindow('ReQuote', '', 'Purchase/PurchaseVendorRequoteRemarks.aspx?quotationid=" + lblQuotationId.Text + "'); return false;");

            if (lblFormType != null)
            {
                if (lblFormType.Text == PhoenixCommonRegisters.GetHardCode(1, 20, "PO"))
                {
                    imgRequote.Visible = false;
                }
                else if (lblWebSession.Text == "N")
                    imgRequote.Visible = true;
                else
                    imgRequote.Visible = false;
            }

            cmdViewQuery.Attributes.Add("onclick", "openNewWindow('ViewQuerySent', '', 'Purchase/PurchaseQuotationItems.aspx?SESSIONID=" + lblQuotationId.Text + "&VIEWONLY=Y'); return false;");



            RadLabel lblIsSelected = (RadLabel)item["FLAG"].FindControl("lblIsSelected");
            RadLabel lblIsApproved = (RadLabel)item["FLAG"].FindControl("lblIsApproved");
            RadLabel lblApprovalExists = (RadLabel)item["FLAG"].FindControl("lblApprovalExists");
            RadLabel lblActiveCurrecy = (RadLabel)item["FLAG"].FindControl("lblActiveCur");
            ImageButton imgFlag = (ImageButton)item["FLAG"].FindControl("imgFlag");
            ImageButton cmdDeSelect = (ImageButton)item["ACTION"].FindControl("cmdDeSelect");
            LinkButton cmdSelect = (LinkButton)item["ACTION"].FindControl("cmdSelect");

            if (cmdDeSelect != null)
            {
                if (lblIsSelected != null && lblIsApproved != null)
                {
                    cmdDeSelect.Visible = lblIsSelected.Text.ToUpper().Equals("TRUE") ? true : false;
                }

                //if (cmdDeSelect.Visible == true)
                //    cmdDeSelect.Visible = SessionUtil.CanAccess(this.ViewState, cmdDeSelect.CommandName);
            }
            if (cmdSelect != null)
            {
                if (lblIsSelected != null && lblIsApproved != null)
                {
                    cmdSelect.Visible = lblIsSelected.Text.ToUpper().Equals("TRUE") ? false : SessionUtil.CanAccess(this.ViewState, cmdSelect.CommandName);
                }

                if (lblActiveCurrecy.Text == "0")
                {
                    LinkButton suppliername = (LinkButton)item["NAME"].FindControl("lnkVendorName");
                    string msg = "Exchange Rate for Quotation Currency from " + suppliername.Text + " is outdated. Please convert to USD manually when evaluating the quotations.Please do note that only quotation with Active currency can be approved";
                    cmdSelect.Attributes.Add("onclick", "return fnConfirmDelete(event,'" + msg + "')");
                }
            }

            imgFlag.ImageUrl = lblIsSelected.Text.ToUpper().Equals("TRUE") ? Session["images"] + "/14.png" : Session["images"] + "/spacer.gif";

            RadLabel lblTotalAmount = (RadLabel)item["AMOUNT"].FindControl("lblPrice");
            ImageButton cmdApprove = (ImageButton)item["ACTION"].FindControl("cmdApprove");
            cmdApprove.Enabled = lblIsSelected.Text.ToUpper().Equals("TRUE") ? true : false;
            cmdApprove.ToolTip = "Approve";
            ImageButton cmdFalapprove = (ImageButton)item["ACTION"].FindControl("btnfalapprove");

            if (cmdApprove != null)
            {
                int PurchaseFALYN = 0;
                PhoenixPurchaseFalRules.PurchaseFALYN(General.GetNullableGuid(ViewState["orderid"].ToString()), 1, int.Parse(drv["FLDVESSELID"].ToString()), ",PUR-FAL,", ref PurchaseFALYN);
                if (PurchaseFALYN == 1)
                {

                    cmdFalapprove.Attributes.Add("onclick", "javascript:parent.openNewWindow('approval', '', 'Purchase/PurchaseFalApprovalLevel.aspx?quotationid=" + drv["FLDQUOTATIONID"].ToString() + "&orderid=" + ViewState["orderid"].ToString() + "&Vesselid=" + Filter.CurrentPurchaseVesselSelection + "');return false;");
                }
                else
                {
                    if (General.GetNullableInteger(drv["FLDISBUDGETED"].ToString()) == 1)
                        cmdApprove.Attributes.Add("onclick", "parent.openNewWindow('approval', '', 'Purchase/PurchaseRemainingBudget.aspx?quotationid=" + drv["FLDQUOTATIONID"].ToString() + "&type=" + drv["FLDQUOTATIONAPPROVAL"].ToString() + "&user=" + drv["FLDTECHDIRECTOR"].ToString() + "," + drv["FLDFLEETMANAGER"].ToString() + "," + drv["FLDSUPT"].ToString() + "&currentorder=" + lblTotalAmount.Text + "&directapprovalyn=Y');return false;");

                }
                cmdApprove.Visible = General.GetNullableInteger(drv["FLDFORMBUDGETCODE"].ToString()) != null && lblIsSelected.Text.ToUpper().Equals("TRUE") && lblIsApproved.Text.ToUpper() != "1" ? SessionUtil.CanAccess(this.ViewState, cmdApprove.CommandName) : false;
                if (PurchaseFALYN == 1)
                {
                    cmdApprove.Visible = false;
                    cmdFalapprove.Visible = General.GetNullableInteger(drv["FLDFORMBUDGETCODE"].ToString()) != null && lblIsSelected.Text.ToUpper().Equals("TRUE") && lblIsApproved.Text.ToUpper() != "1" ? SessionUtil.CanAccess(this.ViewState, cmdFalapprove.CommandName) : false;
                }
            }



            ImageButton cmdDeApprove = (ImageButton)item["ACTION"].FindControl("cmdDeApprove");
            cmdDeApprove.Enabled = lblIsSelected.Text.ToUpper().Equals("TRUE") ? true : false;
            cmdDeApprove.ToolTip = "Revoke approval";

            bool visible = lblIsSelected.Text.ToUpper().Equals("TRUE") ? (lblApprovalExists.Text.ToUpper().Equals("1") ? true : false) : false;

            if (visible)
                visible = SessionUtil.CanAccess(this.ViewState, cmdDeApprove.CommandName);

            cmdDeApprove.Visible = visible;
            //cmdDeApprove.Visible = SessionUtil.CanAccess(this.ViewState, cmdDeApprove.CommandName);
            //cmdDeApprove.Width = 16;
            //cmdDeApprove.Height = 16;


            LinkButton cmdAcceptancemail = (LinkButton)item["ACTION"].FindControl("cmdAcceptancemail");
            if (cmdAcceptancemail != null)
            {
                bool mailvisible = lblIsSelected.Text.ToUpper().Equals("TRUE") ? (lblApprovalExists.Text.ToUpper().Equals("1") ? true : false) : false;
                if (mailvisible)
                    mailvisible = SessionUtil.CanAccess(this.ViewState, cmdAcceptancemail.CommandName);
                cmdAcceptancemail.Visible = mailvisible;
            }
            ImageButton cmdWhatIfQty = (ImageButton)item["ACTION"].FindControl("cmdWhatIfQty");
            cmdWhatIfQty.Attributes.Add("onclick", "javascript:openNewWindow('Filter', '', 'Purchase/PurchaseWhatifQuotationItems.aspx?quotationid=" + lblQuotationId.Text + "'); return false;");
            cmdWhatIfQty.ImageUrl = lblIsSelected.Text.ToUpper().Equals("TRUE") ? Session["images"] + "/edit-quantity.png" : Session["images"] + "/spacer.png";
            cmdWhatIfQty.Enabled = lblIsSelected.Text.ToUpper().Equals("TRUE") ? true : false;
            cmdWhatIfQty.Visible = SessionUtil.CanAccess(this.ViewState, cmdWhatIfQty.CommandName);
            //cmdWhatIfQty.Width = 16;
            //cmdWhatIfQty.Height = 16;

            if (lblIsSelected.Text.ToUpper().Equals("TRUE") && lblIsApproved.Text.ToUpper().Equals("1"))
            {
                //cmdWhatIfQty.ImageUrl = Session["images"] + "/spacer.png";
                cmdWhatIfQty.Visible = false;
                cmdApprove.Visible = false;
            }

            RadLabel lblAppStatus = (RadLabel)item["APPROVALSTATUS"].FindControl("lblAppStatus");
            lblAppStatus.Visible = (lblIsSelected.Text.ToUpper().Equals("TRUE") && drv["FLDAPPROVALSTATUS"].ToString() != string.Empty) ? true : false;
            if (lblIsSelected.Text.ToUpper().Equals("TRUE") && lblAppStatus != null && drv["FLDAPPROVALSTATUS"].ToString() != string.Empty)
            {
                lblAppStatus.ToolTip = drv["FLDAPPROVALSTATUS"].ToString();
                RadToolTipManager1.TargetControls.Add(lblAppStatus.ClientID, true);
            }
            //UserControlToolTip uct = (UserControlToolTip)item["APPROVALSTATUS"].FindControl("ucToolTipAddress");
            //lblAppStatus.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            //lblAppStatus.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");


            //LinkButton lnkVendor = (LinkButton)e.Row.FindControl("lnkVendorName");
            //UserControlToolTip uct1 = (UserControlToolTip)e.Row.FindControl("ucCommentsToolTip");
            //lnkVendor.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct1.ToolTip + "', 'visible');");
            //lnkVendor.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct1.ToolTip + "', 'hidden');");

            RadLabel lblSendDateCode = (RadLabel)item["SENDDATE"].FindControl("lblSendDateCode");

            //cmdViewQuery.Visible = SessionUtil.CanAccess(this.ViewState, cmdViewQuery.CommandName);


            if (lblSendDateCode.Text.ToUpper().Equals(""))
            {
                cmdViewQuery.Enabled = false;
                cmdViewQuery.Visible = false;
            }

            if (imgRequote != null)
            {
                imgRequote.Visible = SessionUtil.CanAccess(this.ViewState, imgRequote.CommandName);
            }
            if (imgRequote != null && drv["APPOVEDSTATUS"].ToString() != string.Empty && drv["APPOVEDSTATUS"].ToString() == "1")
            {
                imgRequote.Visible = false;
            }

            ImageButton cmdAudit = (ImageButton)item["ACTION"].FindControl("cmdAudit");
            cmdAudit.Visible = SessionUtil.CanAccess(this.ViewState, cmdAudit.CommandName);
            if (Filter.CurrentPurchaseStockType.ToUpper().Equals("STORE"))
                cmdAudit.Attributes.Add("onclick", "parent.openNewWindow('Audit', '', 'Common/CommonPhoenixAuditChanges.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&shortcode=PUR-QTNSTORE');return false;");
            else
                cmdAudit.Attributes.Add("onclick", "parent.openNewWindow('Audit', '', 'Common/CommonPhoenixAuditChanges.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&shortcode=PUR-QTN');return false;");

            ImageButton cmdRFQXML = (ImageButton)item["ACTION"].FindControl("cmdRFQXML");
            if (ViewState["SeaProcRFQEnable"].ToString() == "Y")
            {
                cmdRFQXML.Visible = true;
            }
            ImageButton cmdPOXML = (ImageButton)item["ACTION"].FindControl("cmdPOXML");
            if (ViewState["SeaProcRFQEnable"].ToString() == "Y")
            {
                cmdPOXML.Visible = true;
            }
        }
    }

    protected void MenuVendor_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                if (Request.QueryString["orderid"] != null)
                {
                    if (ViewState["quotationid"] != null)
                        //ifMoreInfo.Attributes["src"] ="PurchaseQuotationVendor.aspx?quotationid=" + ViewState["quotationid"].ToString() + "&orderid=" + ViewState["orderid"].ToString();
                        ifMoreInfo.Attributes["src"] = "PurchaseQuotationVendor.aspx?quotationid=" + ViewState["quotationid"].ToString() + "&orderid=" + ViewState["orderid"].ToString();
                    else
                        //ifMoreInfo.Attributes["src"] ="PurchaseQuotationVendor.aspx?orderid=" + ViewState["orderid"].ToString();                    
                        ifMoreInfo.Attributes["src"] = "PurchaseQuotationVendor.aspx?orderid=" + ViewState["orderid"].ToString();
                }
                else
                {
                    if (ViewState["quotationid"] != null)
                        //ifMoreInfo.Attributes["src"] ="PurchaseQuotationVendor.aspx?quotationid=" + ViewState["quotationid"].ToString();
                        ifMoreInfo.Attributes["src"] = "PurchaseQuotationVendor.aspx?quotationid=" + ViewState["quotationid"].ToString();
                    else
                        //ifMoreInfo.Attributes["src"] ="PurchaseQuotationVendor.aspx";
                        ifMoreInfo.Attributes["src"] = "PurchaseQuotationVendor.aspx";
                }

            }
            if (CommandName.ToUpper().Equals("FORMS"))
            {
                if (ViewState["orderid"] != null)
                {
                    if (ViewState["quotationid"] != null)
                        Response.Redirect("../Purchase/PurchaseForm.aspx?quotationid=" + ViewState["quotationid"].ToString() + "&orderid=" + ViewState["orderid"].ToString() + "&pageno=" + ViewState["pageno"].ToString());
                    else
                        Response.Redirect("../Purchase/PurchaseForm.aspx?orderid=" + ViewState["orderid"].ToString() + "&pageno=" + ViewState["pageno"].ToString());
                }
                else
                {
                    if (ViewState["quotationid"] != null)
                        Response.Redirect("../Purchase/PurchaseForm.aspx?quotationid=" + ViewState["quotationid"].ToString() + "&pageno=" + ViewState["pageno"].ToString());
                    else
                        Response.Redirect("../Purchase/PurchaseForm.aspx?pageno=" + ViewState["pageno"].ToString());
                }
                //if (ViewState["orderid"] != null)
                //    Response.Redirect("../Purchase/PurchaseForm.aspx?orderid=" + ViewState["orderid"].ToString() + "&pageno=" + ViewState["pageno"].ToString());
                //else
                //    Response.Redirect("../Purchase/PurchaseForm.aspx?pageno=" + ViewState["pageno"].ToString());
            }
            if (CommandName.ToUpper().Equals("QTNITEM"))
            {
                if (ViewState["orderid"] != null)
                {
                    if (ViewState["quotationid"] != null)
                        Response.Redirect("../Purchase/PurchaseQuotationVendorItems.aspx?quotationid=" + ViewState["quotationid"].ToString() + "&orderid=" + ViewState["orderid"].ToString() + "&pageno=" + ViewState["pageno"].ToString());
                    else
                        Response.Redirect("../Purchase/PurchaseQuotationVendorItems.aspx?orderid=" + ViewState["orderid"].ToString() + "&pageno=" + ViewState["pageno"].ToString());
                }
                else
                {
                    if (ViewState["quotationid"] != null)
                        Response.Redirect("../Purchase/PurchaseQuotationVendorItems.aspx?quotationid=" + ViewState["quotationid"].ToString());
                    else
                        Response.Redirect("../Purchase/PurchaseQuotationVendorItems.aspx");
                }
            }
            if (CommandName.ToUpper().Equals("MULTIPLEREQ"))
            {
                if (ViewState["orderid"] != null)
                {
                    if (ViewState["quotationid"] != null)
                        Response.Redirect("../Purchase/PurchaseMultipleQuotationVendor.aspx?quotationid=" + ViewState["quotationid"].ToString() + "&orderid=" + ViewState["orderid"].ToString() + "&pageno=" + ViewState["pageno"].ToString());
                    else
                        Response.Redirect("../Purchase/PurchaseMultipleQuotationVendor.aspx?orderid=" + ViewState["orderid"].ToString() + "&pageno=" + ViewState["pageno"].ToString());
                }
                else
                {
                    if (ViewState["quotationid"] != null)
                        Response.Redirect("../Purchase/PurchaseMultipleQuotationVendor.aspx?quotationid=" + ViewState["quotationid"].ToString());
                    else
                        Response.Redirect("../Purchase/PurchaseMultipleQuotationVendor.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private bool OrderApprove()
    {
        try
        {
            ucError.HeaderMessage = "Please provide the following required information";
            DataTable dt = PhoenixPurchaseOrderForm.OrderFormSelectVendor(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["orderid"].ToString()));
            ViewState["quotationid"] = dt.Rows[0]["FLDQUOTATIONID"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
        }
        return (!ucError.IsError);
    }

    private void UpdateSelectVendorForPo(string quotationid)
    {
        try
        {
            PhoenixPurchaseQuotation.UpdateQuotation(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["orderid"].ToString()), new Guid(quotationid), Convert.ToInt32(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.FORMTYPE), "PO")));

            PhoenixPurchaseQuotation.SendApprovalInsert
                    (PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["orderid"].ToString()), "yes", 1);

            PhoenixPurchaseFalLevel.FalApprovalLevelsInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(quotationid), Filter.CurrentPurchaseStockType, "PCH"); // 865 is Quick code for Purchase Form
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void DeSelectVendor(string quotationid)
    {
        try
        {
            PhoenixPurchaseQuotation.UpdateQuotationDeSelect(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(ViewState["orderid"].ToString()), new Guid(quotationid));

        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void SendForQuotation()
    {
        string emailid;
        try
        {
            string selectedvendors = ",";
            foreach (GridDataItem item in rgvQuotation.Items)
            {
                if (item["CHECKBOX"].FindControl("chkSelect") != null && ((RadCheckBox)(item["CHECKBOX"].FindControl("chkSelect"))).Checked == true)
                {
                    try
                    {
                        DataTable dt = null;
                        Guid? quotationid = General.GetNullableGuid(((RadLabel)item["FLAG"].FindControl("lblQuotationId")).Text);

                        dt = PhoenixPurchaseQuotation.QuotationXml(quotationid);

                        if (dt.Rows.Count > 0)
                        {

                            string orderformxml = dt.Rows[0]["FLDORDERFORMXML"].ToString();
                            string orderlinexml = dt.Rows[0]["FLDORDERLINEXML"].ToString();
                            string addressxml = dt.Rows[0]["FLDADDRESSXML"].ToString();

                            PhoenixServiceWrapper.SubmitPurchaseQuery(orderformxml, orderlinexml, addressxml);
                        }
                    }
                    catch { }

                    selectedvendors = selectedvendors + ((RadLabel)(item["FLAG"].FindControl("lblQuotationId"))).Text + ",";
                }
            }


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
                        ucConfirm.ErrorMessage = "Email sent to " + dr["FLDNAME"].ToString() + "\n";
                    }
                    catch (Exception ex)
                    {
                        ucConfirm.ErrorMessage = ex.Message + " for  " + dr["FLDNAME"].ToString() + "\n";
                    }
                }
                InsertOrderFormHistory();
                ucConfirm.Visible = true;
            }
            else
            {
                ucConfirm.ErrorMessage = "Email already sent";
                ucConfirm.Visible = true;
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

    private void SendRemindorForQuotation()
    {
        string emailid;
        try
        {
            string selectedvendors = ",";
            foreach (GridDataItem item in rgvQuotation.Items)
            {
                if (item["CHECKBOX"].FindControl("chkSelect") != null && ((RadCheckBox)(item["CHECKBOX"].FindControl("chkSelect"))).Checked == true)
                {
                    selectedvendors = selectedvendors + ((RadLabel)(item["FLAG"].FindControl("lblQuotationId"))).Text + ",";
                }
            }

            if (selectedvendors.Length <= 1)
                selectedvendors = null;
            DataSet dsvendor = PhoenixPurchaseQuotation.ListQuotationToSendRemainder(General.GetNullableGuid(ViewState["orderid"].ToString()), selectedvendors);
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
                    /* Bug Id: 9143 - No need to cc the Supdt. mail id while sending RFQ..
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
                            emailbodytext = PrepareEmailBodyTextForRemainder(new Guid(dr["FLDQUOTATIONID"].ToString()), dr["FLDFORMNO"].ToString(), dr["FLDFROMEMAIL"].ToString());
                            PhoenixCommoneProcessing.PrepareEmailMessage(
                                emailid, "RFQ", new Guid(dr["FLDQUOTATIONID"].ToString()),
                                "", supemail.Equals("") ? dr["FLDFROMEMAIL"].ToString() : supemail + "," + dr["FLDFROMEMAIL"].ToString(),
                                dr["FLDNAME"].ToString(), dr["FLDVESSELNAME"].ToString() + " - RFQ Reminder for " + dr["FLDFORMNO"].ToString() + "" + (dr["FLDTITLE"].ToString() == "" ? "" : "-") + dr["FLDTITLE"].ToString(),
                                emailbodytext, "", "");
                        }
                        else
                        {
                            PhoenixPurchaseQuotation.QuotationRFQExcelInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(dr["FLDORDERID"].ToString()), new Guid(dr["FLDQUOTATIONID"].ToString()), int.Parse(dr["FLDVENDORID"].ToString()));
                        }
                        ucConfirm.ErrorMessage = "Reminder email sent to " + dr["FLDNAME"].ToString() + "\n";
                    }
                    catch (Exception ex)
                    {
                        ucConfirm.ErrorMessage = ex.Message + " for  " + dr["FLDNAME"].ToString() + "\n";
                    }
                }
                ucConfirm.Visible = true;
            }
            else
            {
                ucConfirm.ErrorMessage = "There are no vendors to whom you need to send a reminder. All the vendors have quoted";
                ucConfirm.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

    protected string PrepareEmailBodyTextForRemainder(Guid quotationid, string orderformnumber, string frommailid)
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
        sbemailbody.AppendLine("Reminder: Awaiting your Quotation.");
        sbemailbody.AppendLine("Reply as soon as possible");
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
        sbemailbody.Append("--------------------------------------------------------------------");

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
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("This is an automated message.");
        sbemailbody.AppendLine("If you need personal attention, use \"reply all\" to get your communication across to an email id that is monitored.");
        sbemailbody.AppendLine("Please note " + ConfigurationManager.AppSettings.Get("FromMail").ToString() + " is NOT monitored.");

        return sbemailbody.ToString();

    }

    protected string PrepareEmailBodyTextForAcceptance(Guid quotationid, string orderformnumber)
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
        sbemailbody.AppendLine(dr["FLDCOMPANYNAME"].ToString() + " hereby requests you to Accept or Rject the Purchase Order, for our vessel");
        sbemailbody.AppendLine(dr["FLDVESSELNAME"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.Append("Request your IT department to kindly allow access to this URL for Accept or Rject the Purchase Order.");
        sbemailbody.AppendLine();
        sbemailbody.Append("Please click on the link below and Accept or Rject the Purchase Order. If the link is wrapped, please copy and paste it on the address bar of your browser");
        sbemailbody.AppendLine();
        string url = "\"<" + Session["sitepath"] + "/" + dr["URL"].ToString();
        if (Filter.CurrentPurchaseStockType == null || Filter.CurrentPurchaseStockType.Equals(string.Empty))
            sbemailbody.AppendLine(url + "?SESSIONID=" + dr["SESSIONID"].ToString());
        else
            sbemailbody.AppendLine(url + "?SESSIONID=" + dr["SESSIONID"].ToString() + "&STOCKTYPE=" + Filter.CurrentPurchaseStockType + ">\"");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();

        sbemailbody.AppendLine("This is an automated message.");
        sbemailbody.AppendLine("If you need personal attention, use \"reply all\" to get your communication across to an email id that is monitored.");
        sbemailbody.AppendLine("Please note " + ConfigurationManager.AppSettings.Get("FromMail").ToString() + " is NOT monitored.");

        return sbemailbody.ToString();

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            //this for processing the commited cost paid and update once the approval is done.
            if (Session["POQAPPROVE"] != null && ((DataSet)Session["POQAPPROVE"]).Tables.Count > 0)
            {
                DataSet ds = (DataSet)Session["POQAPPROVE"];
                ucConfirm.ErrorMessage = "Purchase order is approved";
                ucConfirm.Visible = true;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string emailbodytext = PrepareApprovalText(ds.Tables[0], 0);
                    DataRow dr = ds.Tables[0].Rows[0];
                    PhoenixMail.SendMail(dr["FLDEMAIL"].ToString().Replace(";", ",").TrimEnd(','),
                        dr["FLDFROMEMAIL"].ToString().Replace(";", ",").TrimEnd(','),
                        null,
                        dr["FLDSUBJECT"].ToString() + "     " + dr["FLDFORMNO"].ToString(),
                        emailbodytext,
                        true,
                        System.Net.Mail.MailPriority.Normal,
                        "",
                        null,
                        null);
                }
                Session["POQAPPROVE"] = null;
            }
            rgvQuotation.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //public void CopyForm_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
    //        if (ucCM.confirmboxvalue == 1)
    //        {
    //            Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=3&reportcode=PURCHASEORDERFORM&quotationid=" + ViewState["quotationid"].ToString() + "&orderid=" + ViewState["orderid"].ToString() + "&showactual=1&showword=no&showexcel=no");

    //        }
    //        else
    //        {
    //            Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=3&reportcode=PURCHASEORDERFORM&quotationid=" + ViewState["quotationid"].ToString() + "&orderid=" + ViewState["orderid"].ToString() + "&showactual=0&showword=no&showexcel=no");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void ucConfirmRevoke_ConfirmMesage(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        DataSet dsApproval = PhoenixPurchaseQuotation.QuotationApprovalEdit(
    //               new Guid(ViewState["orderid"].ToString()),
    //               new Guid(ViewState["QuotationId"].ToString()));

    //        DataRow drApproval = dsApproval.Tables[0].Rows[0];

    //        UserControlConfirmMessage uc = (UserControlConfirmMessage)sender;
    //        if (uc.confirmboxvalue == 1)
    //        {
    //            int PurchaseFALYN = 0;
    //            PhoenixPurchaseFalRules.PurchaseFALYN(General.GetNullableGuid(ViewState["orderid"].ToString()), 1, int.Parse(drApproval["FLDVESSELID"].ToString()), ",PUR-FAL,", ref PurchaseFALYN);
    //            if (PurchaseFALYN == 1)
    //            {
    //                PhoenixPurchaseFalApprovalLevel.RevokeAccess(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["QuotationId"].ToString()));
    //            }
    //            if (drApproval["FLDFULLAPPROVAL"].ToString().Equals("1"))
    //            {
    //                string emailbodytext = "";

    //                DataSet ds = PhoenixPurchaseOrderForm.ApproveOrderForm(
    //                    PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["orderid"].ToString())
    //                    , General.GetNullableGuid(ViewState["QuotationId"].ToString())
    //                    , General.GetNullableDateTime(DateTime.Now.ToString()),
    //                    Int32.Parse(ViewState["lblIsApproved"].ToString()));

    //                if (Int32.Parse(ViewState["lblIsApproved"].ToString()) == 1)
    //                {
    //                    PhoenixPurchaseOrderForm.OrderFormDeletePurchaseBudgetCommitment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["orderid"].ToString()));
    //                    if (ds.Tables[0].Rows.Count > 0)
    //                    {
    //                        foreach (DataRow dr in ds.Tables[0].Rows)
    //                        {
    //                            PhoenixCommonBudgetGroupAllocation.UpdateBudgetCommittedPaidAmount(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(dr["FLDVESSELID"].ToString())
    //                               , int.Parse(dr["FLDBUDGETCODE"].ToString()), Convert.ToDateTime(dr["FLDCREATEDDATE"].ToString()), decimal.Parse(dr["FLDCOMMITEDAMOUNTLOCAL"].ToString())
    //                               , 0, char.Parse("D"), dr["FLDFORMNO"].ToString(), ViewState["orderid"].ToString(), General.GetNullableInteger(dr["FLDCURRENCY"].ToString()), decimal.Parse(dr["FLDCOMMITEDAMOUNTUSD"].ToString())
    //                               , null, null, General.GetNullableDateTime(DateTime.Now.ToString("yyyy/MM/dd")), null, 583, General.GetNullableInteger(dr["FLDVENDORID"].ToString()), General.GetNullableInteger(dr["FLDACCOUNTID"].ToString()), "Approval Revoked"); // bug id 12326 reason for reversal
    //                        }
    //                    }
    //                    if (ds.Tables[1].Rows.Count > 0)
    //                    {
    //                        emailbodytext = PrepareApprovalText(ds.Tables[1], 1);
    //                        DataRow dr = ds.Tables[1].Rows[0];
    //                        PhoenixMail.SendMail(dr["FLDEMAIL"].ToString().Replace(";", ",").TrimEnd(','),
    //                            dr["FLDFROMEMAIL"].ToString().Replace(";", ",").TrimEnd(','),
    //                            null,
    //                            dr["FLDSUBJECT"].ToString() + "     " + PhoenixPurchaseOrderForm.FormNumber,
    //                            emailbodytext,
    //                            true,
    //                            System.Net.Mail.MailPriority.Normal,
    //                            "", null,
    //                            null);
    //                        if (PurchaseFALYN == 1)
    //                        {
    //                            PhoenixPurchaseFalApprovalLevel.QuotationRevoke(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["QuotationId"].ToString()), General.GetNullableGuid(ViewState["orderid"].ToString()));
    //                        }
    //                    }
    //                    ucConfirm.ErrorMessage = "Purchase approval is cancelled.";
    //                }
    //                //ucConfirm.Visible = true;
    //            }
    //            else if (drApproval["FLDAPPROVALEXISTS"].ToString().Equals("1"))
    //            {
    //                PhoenixPurchaseQuotation.DeletePartialApproval(
    //                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
    //                    new Guid(ViewState["QuotationId"].ToString()));
    //                if (PurchaseFALYN == 1)
    //                {
    //                    PhoenixPurchaseFalApprovalLevel.QuotationRevoke(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["QuotationId"].ToString()), General.GetNullableGuid(ViewState["orderid"].ToString()));
    //                }
    //                ucConfirm.ErrorMessage = "Purchase approval is cancelled.";
    //                //ucConfirm.Visible = true;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucConfirm.Visible = false;
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //private void SendForRequote(Guid quotaionid)
    //{
    //     DataSet dsvendor = PhoenixPurchaseQuotation.ListQuotationToSendRequote(General.GetNullableGuid(ViewState["orderid"].ToString()), General.GetNullableGuid(quotaionid));
    //        if (dsvendor.Tables[0].Rows.Count > 0)
    //        {

    //            foreach (DataRow dr in dsvendor.Tables[0].Rows)
    //            {
    //                string emailbodytext = "";
    //                string supemail = "";
    //                if (dr["FLDEMAIL1"].ToString().Contains(";"))
    //                    emailid = dr["FLDEMAIL1"].ToString().Replace(";", ",");
    //                else
    //                    emailid = dr["FLDEMAIL1"].ToString();

    //                if (!dr["FLDEMAIL2"].ToString().Equals(""))
    //                {
    //                    emailid = emailid + "," + dr["FLDEMAIL2"].ToString().Replace(";", ",");
    //                }

    //                if (dr["FLDSUPTEMAIL"].ToString().Equals("") || dr["FLDSUPPLIEREMAIL"].ToString().Equals(""))
    //                    supemail = dr["FLDSUPTEMAIL"].ToString() + dr["FLDSUPPLIEREMAIL"].ToString();
    //                else
    //                    supemail = dr["FLDSUPTEMAIL"].ToString() + "," + dr["FLDSUPPLIEREMAIL"].ToString();

    //                supemail = dr["FLDSUPPLIEREMAIL"].ToString();

    //                DataSet dscontact;
    //                dscontact = PhoenixPurchaseQuotation.QuotationContactsGetEmail(General.GetNullableInteger(dr["FLDVENDORID"].ToString()), Filter.CurrentPurchaseStockType.ToString(), Filter.CurrentPurchaseVesselSelection);
    //                if (!dscontact.Tables[0].Rows[0]["FLDEMALTO"].ToString().Trim().Equals(""))
    //                {
    //                    emailid = emailid + dscontact.Tables[0].Rows[0]["FLDEMALTO"].ToString().Trim();
    //                }
    //                if (!dscontact.Tables[0].Rows[0]["FLDEMALCC"].ToString().Trim().Equals(""))
    //                {
    //                    supemail = supemail + dscontact.Tables[0].Rows[0]["FLDEMALCC"].ToString().Trim();
    //                }

    //                try
    //                {
    //                    emailbodytext = PrepareEmailBodyText(new Guid(dr["FLDQUOTATIONID"].ToString()), dr["FLDFORMNO"].ToString(), dr["FLDFROMEMAIL"].ToString());
    //                    PhoenixCommoneProcessing.PrepareEmailMessage(
    //                            emailid, "RFQ", new Guid(dr["FLDQUOTATIONID"].ToString()),
    //                            "", supemail.Equals("") ? dr["FLDFROMEMAIL"].ToString() : supemail + "," + dr["FLDFROMEMAIL"].ToString(),
    //                            dr["FLDNAME"].ToString(), dr["FLDVESSELNAME"].ToString() + " - RFQ for " + dr["FLDFORMNO"].ToString() + "" + (dr["FLDTITLE"].ToString() == "" ? "" : "-") + dr["FLDTITLE"].ToString(), emailbodytext, "", "");

    //                    ucConfirm.ErrorMessage = "Email sent to " + dr["FLDNAME"].ToString() + "\n";
    //                }
    //                catch (Exception ex)
    //                {
    //                    ucConfirm.ErrorMessage = ex.Message + " for  " + dr["FLDNAME"].ToString() + "\n";
    //                }
    //            }

    //            ucConfirm.Visible = true;
    //        }

    //}

    //protected string PrepareEmailBodyTextForRequote(Guid quotationid, string orderformnumber, string frommailid)
    //{
    //    StringBuilder sbemailbody = new StringBuilder();
    //    DataSet ds = PhoenixPurchaseQuotation.GetQuotationDataForEmailBody(PhoenixSecurityContext.CurrentSecurityContext.UserCode, quotationid, "RFQ");
    //    DataRow dr = ds.Tables[0].Rows[0];

    //    sbemailbody.Append("This is an automated message. DO NOT REPLY to "+ConfigurationManager.AppSettings.Get("FromMail").ToString()+". Kindly use the \"reply all\" button if you are responding to this message.");
    //    sbemailbody.AppendLine();
    //    sbemailbody.AppendLine();
    //    sbemailbody.Append(dr["FLDVENDORNAME"].ToString() + "             " + orderformnumber);
    //    sbemailbody.AppendLine();
    //    sbemailbody.AppendLine();
    //    sbemailbody.Append("Dear Sir");
    //    sbemailbody.AppendLine();
    //    sbemailbody.AppendLine();
    //    sbemailbody.AppendLine("Reminder: Awaiting your Quotation.");
    //    sbemailbody.AppendLine("Reply as soon as possible");
    //    sbemailbody.AppendLine();
    //    sbemailbody.AppendLine();
    //    sbemailbody.AppendLine("Thank you,");
    //    sbemailbody.AppendLine();
    //    sbemailbody.Append(dr["FLDUSERNAME"].ToString());
    //    sbemailbody.AppendLine();
    //    sbemailbody.AppendLine("For and on behalf of " + dr["FLDCOMPANYNAME"].ToString());
    //    sbemailbody.AppendLine();
    //    sbemailbody.AppendLine("(As Agents for Owners)");
    //    sbemailbody.AppendLine();
    //    sbemailbody.AppendLine("Contact: " + frommailid);
    //    sbemailbody.AppendLine();
    //    sbemailbody.Append("--------------------------------------------------------------------");

    //    sbemailbody.Append(dr["FLDVENDORNAME"].ToString() + "             " + orderformnumber);
    //    sbemailbody.AppendLine();
    //    sbemailbody.AppendLine();
    //    sbemailbody.Append("Dear Sir");
    //    sbemailbody.AppendLine();
    //    sbemailbody.AppendLine();
    //    sbemailbody.AppendLine("Executive Ship Management Pte Ltd hereby requests you to provide your BEST quotation for the following items to be delivered to our vessel");
    //    sbemailbody.AppendLine(dr["FLDVESSELNAME"].ToString());
    //    sbemailbody.AppendLine();
    //    sbemailbody.Append("Request your IT department to kindly allow access to this URL for submitting quotes.");
    //    sbemailbody.AppendLine();
    //    sbemailbody.Append("Please click on the link below and key in the relevant fields indicated. If the link is wrapped, please copy and paste it on the address bar of your browser");
    //    sbemailbody.AppendLine();
    //    string url = "\"<" + Session["sitepath"] + "/" + dr["URL"].ToString();
    //    if (Filter.CurrentPurchaseStockType == null || Filter.CurrentPurchaseStockType.Equals(string.Empty))
    //        sbemailbody.AppendLine(url + "?SESSIONID=" + dr["SESSIONID"].ToString());
    //    else
    //        sbemailbody.AppendLine(url + "?SESSIONID=" + dr["SESSIONID"].ToString() + "&STOCKTYPE=" + Filter.CurrentPurchaseStockType + ">\"");
    //    sbemailbody.AppendLine();
    //    sbemailbody.AppendLine();

    //    if (dr["FLDEXPIRYDATE"].ToString() != "")
    //    {
    //        sbemailbody.AppendLine("We request you to submit your bid by");
    //        sbemailbody.Append(dr["FLDEXPIRYDATE"].ToString());
    //        sbemailbody.Append(", failing which your offer will NOT be accepted. If you wish to decline to bid, please advise us by email with your reasons for declining.");
    //    }
    //    else
    //    {
    //        sbemailbody.AppendLine("We request you to submit your bid, failing which your offer will NOT be accepted. If you wish to decline to bid, please advise us by email with your reasons for declining.");
    //    }

    //    sbemailbody.AppendLine();
    //    sbemailbody.Append("Note: In our continual effort to keep correct records of your address and contact information, we appreciate your time to verify and correct it where necessary. Please click on the link below to view/correct the address.");
    //    sbemailbody.AppendLine();

    //    DataSet dsvendorid = PhoenixPurchaseQuotation.EditQuotation(quotationid);
    //    DataRow drvendorid = dsvendorid.Tables[0].Rows[0];
    //    string vendorid = drvendorid["FLDVENDORID"].ToString();

    //    DataSet dsaddress = PhoenixRegistersAddress.EditAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(vendorid));
    //    DataRow draddress = dsaddress.Tables[0].Rows[0];

    //    sbemailbody.AppendLine("\n" + "\"<" + Session["sitepath"] + "/Purchase/PurchaseVendorAddressEdit.aspx?sessionid=" + draddress["FLDDTKEY"].ToString() + ">\"");

    //    sbemailbody.AppendLine();
    //    sbemailbody.AppendLine();
    //    sbemailbody.AppendLine("Thank you,");
    //    sbemailbody.AppendLine();
    //    sbemailbody.Append(dr["FLDUSERNAME"].ToString());
    //    sbemailbody.AppendLine();
    //    sbemailbody.AppendLine("For and on behalf of " + dr["FLDCOMPANYNAME"].ToString());
    //    sbemailbody.AppendLine();
    //    sbemailbody.AppendLine("(As Agents for Owners)");
    //    sbemailbody.AppendLine();
    //    sbemailbody.AppendLine();
    //    sbemailbody.AppendLine("This is an automated message.");
    //    sbemailbody.AppendLine("If you need personal attention, use \"reply all\" to get your communication across to an email id that is monitored.");
    //    sbemailbody.AppendLine("Please note "+ConfigurationManager.AppSettings.Get("FromMail").ToString()+" is NOT monitored.");

    //    return sbemailbody.ToString();

    //}

    protected void hideamount_Click(object sender, EventArgs e)
    {
        RadWindowManager1.Localization.OK = "OK";
        RadWindowManager1.Localization.Cancel = "Cancel";
        Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=3&reportcode=PURCHASEORDERFORM&quotationid=" + ViewState["quotationid"].ToString() + "&orderid=" + ViewState["orderid"].ToString() + "&showactual=1&showword=no&showexcel=no");
    }

    protected void showamount_Click(object sender, EventArgs e)
    {
        RadWindowManager1.Localization.OK = "OK";
        RadWindowManager1.Localization.Cancel = "Cancel";
        Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=3&reportcode=PURCHASEORDERFORM&quotationid=" + ViewState["quotationid"].ToString() + "&orderid=" + ViewState["orderid"].ToString() + "&showactual=0&showword=no&showexcel=no");
    }

    protected void ConfirmRFQXML_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["SEAPROCFOLDERPATH"] != null)
            {
                if (Path.IsPathRooted(ViewState["SEAPROCFOLDERPATH"].ToString()))
                {
                    string FileName = ViewState["SEAPROCFOLDERPATH"].ToString() + "RFQ" + ViewState["SEAPROCFILENAME"].ToString() + "_" + ViewState["VendorId"].ToString() + ".xml";
                    string xml = "";
                    DataSet dssummary = PhoenixPurchaseQuotation.SeaProcRFQXMLGenerate(int.Parse(ViewState["FLDVESSELID"].ToString()), new Guid(ViewState["orderid"].ToString()), new Guid(ViewState["quotationid"].ToString()));
                    XmlDocument doc = new XmlDocument();
                    for (int i = 0; i < dssummary.Tables[0].Rows.Count; i++)
                    {
                        string value = dssummary.Tables[0].Rows[i][0].ToString();
                        xml = xml + value;
                    }

                    doc.LoadXml(xml);

                    if (File.Exists(FileName))
                        File.Delete(FileName);
                    using (XmlTextWriter writer = new XmlTextWriter(FileName, null))
                    {
                        writer.Formatting = Formatting.Indented;
                        doc.Save(writer);
                    }

                    //Purchase current Order attachment path

                    DataSet dsatt = PhoenixPurchaseQuotation.SeaProcAttachment(new Guid(ViewState["orderid"].ToString()), int.Parse(ViewState["FLDVESSELID"].ToString()));

                    if (dsatt.Tables.Count > 0 && dsatt.Tables[0].Rows.Count > 0)
                    {
                        string FolderName = dsatt.Tables[0].Rows[0]["FLDFOLDERNAME"].ToString() + "_" + ViewState["VendorId"].ToString() + ".xml_ATTACHMENT";  //ATTACHMENT FOLER NAME
                        string path = ViewState["SEAPROCFOLDERPATH"].ToString() + FolderName;
                        if (!Directory.Exists(path))
                        {
                            DirectoryInfo di = Directory.CreateDirectory(path);
                        }
                        foreach (DataRow dr in dsatt.Tables[0].Rows)
                        {
                            string SourceDir = dr["FLDATTACHMENTPATH"] + "\\" + dr["FLDFILEPATH"].ToString().Replace("/", "\\");
                            string Filename = dr["FLDFILENAME"].ToString();
                            if (File.Exists(SourceDir))
                            {
                                string newFilePath = path + "\\" + Filename.Replace("/", "\\");
                                if (File.Exists(newFilePath))
                                {
                                    File.Delete(newFilePath);
                                }
                                File.Copy(SourceDir, newFilePath);  // copying the RFQ attachment
                            }
                        }
                    }
                    string attachment = "attachment; filename=" + "RFQ" + ViewState["SEAPROCFILENAME"].ToString() + "_" + ViewState["VendorId"].ToString() + ".xml";
                    Response.ClearContent();
                    Response.ContentType = "application/xml";
                    Response.AddHeader("content-disposition", attachment);
                    Response.Write(xml);
                    Response.End();
                }
            }
            if (ViewState["SEAPROCFOLDERPATH"] == null)
            {
                ucError.ErrorMessage = "Please give the folder Path to generate RFQ xml.";
                ucError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirmPOXML_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["SEAPROCFOLDERPATH"] != null)
            {
                if (ViewState["SEAPROCFOLDERPATH"].ToString() != "")
                {
                    if (Path.IsPathRooted(ViewState["SEAPROCFOLDERPATH"].ToString())) //Checking the given path 
                    {
                        string FileName = ViewState["SEAPROCFOLDERPATH"].ToString() + "PO" + ViewState["SEAPROCFILENAME"].ToString() + "_" + ViewState["VendorId"].ToString() + ".xml";
                        string xml = "";
                        DataSet dssummary = PhoenixPurchaseQuotation.SeaProcPOXMLGenerate(int.Parse(ViewState["FLDVESSELID"].ToString()), new Guid(ViewState["orderid"].ToString()), new Guid(ViewState["quotationid"].ToString()));
                        XmlDocument doc = new XmlDocument();
                        for (int i = 0; i < dssummary.Tables[0].Rows.Count; i++)
                        {
                            string value = dssummary.Tables[0].Rows[i][0].ToString();
                            xml = xml + value;
                        }

                        doc.LoadXml(xml);

                        if (File.Exists(FileName))
                            File.Delete(FileName);
                        using (XmlTextWriter writer = new XmlTextWriter(FileName, null))
                        {
                            writer.Formatting = Formatting.Indented;
                            doc.Save(writer);
                        }

                        string attachment = "attachment; filename=" + "PO" + ViewState["SEAPROCFILENAME"].ToString() + "_" + ViewState["VendorId"].ToString() + ".xml";
                        Response.ClearContent();
                        Response.ContentType = "application/xml";
                        Response.AddHeader("content-disposition", attachment);
                        Response.Write(xml);
                        Response.End();
                    }
                }
            }
            if (ViewState["SEAPROCFOLDERPATH"] == null)
            {
                ucError.ErrorMessage = "Please give the folder Path to generate PO xml.";
                ucError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ucConfirmRevoke_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsApproval = PhoenixPurchaseQuotation.QuotationApprovalEdit(
                   new Guid(ViewState["orderid"].ToString()),
                   new Guid(ViewState["QuotationId"].ToString()));

            DataRow drApproval = dsApproval.Tables[0].Rows[0];

            //UserControlConfirmMessage uc = (UserControlConfirmMessage)sender;
            //if (uc.confirmboxvalue == 1)
            //{
                int PurchaseFALYN = 0;
                PhoenixPurchaseFalRules.PurchaseFALYN(General.GetNullableGuid(ViewState["orderid"].ToString()), 1, int.Parse(drApproval["FLDVESSELID"].ToString()), ",PUR-FAL,", ref PurchaseFALYN);
                if (PurchaseFALYN == 1)
                {
                    PhoenixPurchaseFalApprovalLevel.RevokeAccess(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["QuotationId"].ToString()));
                }
                if (drApproval["FLDFULLAPPROVAL"].ToString().Equals("1"))
                {
                    string emailbodytext = "";

                    DataSet ds = PhoenixPurchaseOrderForm.ApproveOrderForm(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["orderid"].ToString())
                        , General.GetNullableGuid(ViewState["QuotationId"].ToString())
                        , General.GetNullableDateTime(DateTime.Now.ToString()),
                        Int32.Parse(ViewState["lblIsApproved"].ToString()));

                    if (Int32.Parse(ViewState["lblIsApproved"].ToString()) == 1)
                    {
                        PhoenixPurchaseOrderForm.OrderFormDeletePurchaseBudgetCommitment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["orderid"].ToString()));
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                PhoenixCommonBudgetGroupAllocation.UpdateBudgetCommittedPaidAmount(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(dr["FLDVESSELID"].ToString())
                                   , int.Parse(dr["FLDBUDGETCODE"].ToString()), Convert.ToDateTime(dr["FLDCREATEDDATE"].ToString()), decimal.Parse(dr["FLDCOMMITEDAMOUNTLOCAL"].ToString())
                                   , 0, char.Parse("D"), dr["FLDFORMNO"].ToString(), ViewState["orderid"].ToString(), General.GetNullableInteger(dr["FLDCURRENCY"].ToString()), decimal.Parse(dr["FLDCOMMITEDAMOUNTUSD"].ToString())
                                   , null, null, General.GetNullableDateTime(DateTime.Now.ToString("yyyy/MM/dd")), null, 583, General.GetNullableInteger(dr["FLDVENDORID"].ToString()), General.GetNullableInteger(dr["FLDACCOUNTID"].ToString()), "Approval Revoked"); // bug id 12326 reason for reversal
                            }
                        }
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            emailbodytext = PrepareApprovalText(ds.Tables[1], 1);
                            DataRow dr = ds.Tables[1].Rows[0];
                            PhoenixMail.SendMail(dr["FLDEMAIL"].ToString().Replace(";", ",").TrimEnd(','),
                                dr["FLDFROMEMAIL"].ToString().Replace(";", ",").TrimEnd(','),
                                null,
                                dr["FLDSUBJECT"].ToString() + "     " + PhoenixPurchaseOrderForm.FormNumber,
                                emailbodytext,
                                true,
                                System.Net.Mail.MailPriority.Normal,
                                "", null,
                                null);
                            if (PurchaseFALYN == 1)
                            {
                                PhoenixPurchaseFalApprovalLevel.QuotationRevoke(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["QuotationId"].ToString()), General.GetNullableGuid(ViewState["orderid"].ToString()));
                            }
                        }
                        ucConfirm.ErrorMessage = "Purchase approval is cancelled.";
                    }
                    //ucConfirm.Visible = true;
                }
                else if (drApproval["FLDAPPROVALEXISTS"].ToString().Equals("1"))
                {
                    PhoenixPurchaseQuotation.DeletePartialApproval(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["QuotationId"].ToString()));
                    if (PurchaseFALYN == 1)
                    {
                        PhoenixPurchaseFalApprovalLevel.QuotationRevoke(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["QuotationId"].ToString()), General.GetNullableGuid(ViewState["orderid"].ToString()));
                    }
                    ucConfirm.ErrorMessage = "Purchase approval is cancelled.";
                    //ucConfirm.Visible = true;
                }
            //}
        }
        catch (Exception ex)
        {
            ucConfirm.Visible = false;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void rgvQuotation_PreRender(object sender, EventArgs e)
    {

        short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
        rgvQuotation.MasterTableView.GetColumn("DISCOUNT").Visible = (showcreditnotedisc == 1) ? true : false;
        rgvQuotation.MasterTableView.GetColumn("TOTAL").Visible = (showcreditnotedisc == 1) ? true : false;
    }
}

