using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class AccountsInvoiceLineItemDetailsRWAForPurchase : PhoenixBasePage
{


    public decimal InvoicePayableAmount = 0;
    public string TotalPOPayableAmount = "";
    public decimal POPayableAmount = 0.00m;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            confirm.Attributes.Add("style", "display:none;");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Invoice", "INVOICE");
            toolbarmain.AddButton("PO", "PO");
            toolbarmain.AddButton("Attachments", "ATTACHMENTS");
            toolbarmain.AddButton("History", "HISTORY");
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            MenuLineItem.AccessRights = this.ViewState;
            MenuLineItem.MenuList = toolbarmain.Show();

            MenuLineItem.SelectedMenuIndex = 1;
            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                gvLineItem.PageSize = General.ShowRecords(gvLineItem.PageSize);

                if (Request.QueryString["qvoucherlineitemcode"] != null && Request.QueryString["qvoucherlineitemcode"] != string.Empty)
                    ViewState["invoicelineitemcode"] = Request.QueryString["qvoucherlineitemcode"];
                if (Request.QueryString["qcallfrom"] != null && Request.QueryString["qcallfrom"] != string.Empty)
                    ViewState["callfrom"] = Request.QueryString["qcallfrom"];

                ViewState["invoicecode"] = Request.QueryString["qinvoicecode"];
                InvoiceEdit();
            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Purchase/AccountsInvoiceLineItemDetailsRWAForPurchase.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvInvoice')", "Print Grid", "icon_print.png", "PRINT");
            if (ViewState["SHORTNAME"] != null && ViewState["SHORTNAME"].ToString() != "AVN")
              //  toolbargrid.AddImageLink("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsInvoiceLineItemDetailsRWAForPurchase.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString() + "'); return false;", "Add", "Add.png", "ADD");
            //toolbargrid.AddImageButton("../Accounts/AccountsInvoiceLineItemDetails.aspx", "Post Voucher", "pr.png", "CREATEPR");

            MenuOrderLineItem.AccessRights = this.ViewState;
            MenuOrderLineItem.MenuList = toolbargrid.Show();

            // BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvLineItem.SelectedIndexes.Clear();
        gvLineItem.EditIndexes.Clear();
        gvLineItem.DataSource = null;
        gvLineItem.Rebind();
    }
    protected void InvoiceEdit()
    {
        if (ViewState["invoicecode"] != null)
        {

            DataSet dsInvoice = PhoenixAccountsInvoice.InvoiceEdit(new Guid(ViewState["invoicecode"].ToString()));
            if (dsInvoice.Tables.Count > 0)
            {
                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                ViewState["SHORTNAME"] = drInvoice["FLDSHORTNAME"].ToString();
                ViewState["DTKEY"] = drInvoice["FLDDTKEY"].ToString();
                if (drInvoice["FLDISDUPLICATEYN"] != null && drInvoice["FLDISDUPLICATEYN"].ToString() == "1")
                {
                    HlinkRefDuplicate.NavigateUrl = "~/Accounts/AccountsVoucherDuplicateList.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qcallfrom=" + ViewState["callfrom"].ToString();
                    HlinkRefDuplicate.Visible = true;
                }
            }
        }
    }
    protected void MenuLineItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("PO"))
            {
                Response.Redirect("../Purchase/AccountsInvoiceLineItemDetailsRWAForPurchase.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qcallfrom=" + ViewState["callfrom"]);
            }
            else if (CommandName.ToUpper().Equals("INVOICE"))
            {
                if (ViewState["callfrom"].ToString() == "PR")
                {
                    Response.Redirect("../Accounts/AccountsPostInvoiceMaster.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString());
                }
                else if (ViewState["callfrom"].ToString() == "invoice")
                {
                    Response.Redirect("../Accounts/AccountsInvoiceMaster.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString());
                }
                else if (ViewState["callfrom"].ToString() == "invoiceforpurchase")
                {
                    Response.Redirect("../Accounts/AccountsInvoiceMasterForPurchase.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString());
                }
                else if (ViewState["callfrom"].ToString() == "AD")
                {
                    Response.Redirect("../Purchase/AccountsInvoiceMasterRWAForPurchase.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString());
                }
            }

            else if (CommandName.ToUpper().Equals("ATTACHMENTS"))
            {
                Response.Redirect("../Purchase/AccountsInvoiceAttachmentsRWAForPurchase.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qfrom=" + ViewState["callfrom"]);
            }
            else if (CommandName.ToUpper().Equals("HISTORY"))
            {
                Response.Redirect("../Purchase/AccountsInvoiceHistoryRWAForPurchase.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qfrom=" + ViewState["callfrom"]);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLineItem_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridEditableItem)
        {
            ImageButton cmdOrder = (ImageButton)e.Item.FindControl("cmdOrder");
            if (cmdOrder != null)
            {
                cmdOrder.Visible = SessionUtil.CanAccess(this.ViewState, cmdOrder.CommandName);
                if (ViewState["SHORTNAME"] != null && ViewState["SHORTNAME"].ToString() == "AVN")
                    cmdOrder.Visible = false;
            }

            ImageButton cmdReceipt = (ImageButton)e.Item.FindControl("cmdReceipt");
            if (cmdReceipt != null)
            {
                cmdReceipt.Visible = SessionUtil.CanAccess(this.ViewState, cmdReceipt.CommandName);
                if (ViewState["SHORTNAME"] != null && ViewState["SHORTNAME"].ToString() == "AVN")
                    cmdReceipt.Visible = false;
            }

            RadLabel lblOrdertype = (RadLabel)e.Item.FindControl("lblOrdertype");
            if (lblOrdertype != null && lblOrdertype.Text != "1")
            {
                cmdOrder.Visible = false;
                cmdReceipt.Visible = false;
            }
            RadLabel lblPOReceived = (RadLabel)e.Item.FindControl("lblPOReceived");
            if (lblOrdertype != null && lblOrdertype.Text == "1")
            {
                if (lblPOReceived != null && lblPOReceived.Text == "1")
                    cmdReceipt.Visible = true;
                else
                    cmdReceipt.Visible = false;
            }
            RadTextBox txtOrderId = (RadTextBox)e.Item.FindControl("txtOrderId");
            if (cmdReceipt != null && txtOrderId != null)
            {
                cmdReceipt.Attributes.Add("onclick", "openNewWindow('VesselReceipt', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=3&reportcode=VESSELRECEIPT&orderid=" + txtOrderId.Text + "&showmenu=0&showword=no&showexcel=no');return false;");
            }

            ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null)
            {
                cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
                //if (ViewState["SHORTNAME"] != null && ViewState["SHORTNAME"].ToString() == "AVN")
                //    cmdDelete.Visible = false;
            }

            ImageButton cmdPOPostInvoiceStaging = (ImageButton)e.Item.FindControl("cmdPOPostInvoiceStaging");

            if (cmdPOPostInvoiceStaging != null)
                cmdPOPostInvoiceStaging.Visible = SessionUtil.CanAccess(this.ViewState, cmdPOPostInvoiceStaging.CommandName);

            ImageButton cmdRefresh = (ImageButton)e.Item.FindControl("cmdRefresh");

            if (cmdRefresh != null)
                if (lblOrdertype != null && (lblOrdertype.Text == "2" || lblOrdertype.Text == "3"))// for the bugid 9132 refresh is not for direct po and amos po
                    cmdRefresh.Visible = false;
                else
                    cmdRefresh.Visible = SessionUtil.CanAccess(this.ViewState, cmdRefresh.CommandName);

        }

        if (ViewState["callfrom"].ToString() == "invoice")
        {
            gvLineItem.Columns[0].Visible = false;
            gvLineItem.Columns[1].Visible = false;
            //gvLineItem.Columns[4].Visible = false;
            gvLineItem.Columns[5].Visible = false;
            gvLineItem.Columns[6].Visible = false;
            gvLineItem.Columns[7].Visible = true;
            gvLineItem.Columns[8].Visible = true;
            gvLineItem.Columns[9].Visible = false;
            gvLineItem.Columns[10].Visible = false;
            gvLineItem.Columns[11].Visible = false;
            gvLineItem.Columns[12].Visible = false;
            gvLineItem.Columns[13].Visible = false;
            gvLineItem.Columns[14].Visible = false;
            RadTextBox txtInvPayableAmountEdit = (RadTextBox)e.Item.FindControl("txtInvPayableAmountEdit");
            if (txtInvPayableAmountEdit != null) txtInvPayableAmountEdit.ReadOnly = true;
        }
        else if (ViewState["callfrom"].ToString() == "invoiceforpurchase")
        {
            gvLineItem.Columns[0].Visible = false;
            gvLineItem.Columns[1].Visible = false;
            //gvLineItem.Columns[4].Visible = false;
            gvLineItem.Columns[5].Visible = false;
            gvLineItem.Columns[6].Visible = false;
            gvLineItem.Columns[7].Visible = true;
            gvLineItem.Columns[8].Visible = true;
            gvLineItem.Columns[9].Visible = false;
            gvLineItem.Columns[10].Visible = false;
            gvLineItem.Columns[11].Visible = false;
            gvLineItem.Columns[12].Visible = false;
            gvLineItem.Columns[13].Visible = false;
            gvLineItem.Columns[14].Visible = false;
            RadTextBox txtInvPayableAmountEdit = (RadTextBox)e.Item.FindControl("txtInvPayableAmountEdit");
            if (txtInvPayableAmountEdit != null) txtInvPayableAmountEdit.ReadOnly = true;
        }

        else if (ViewState["callfrom"].ToString() == "AD")
        {
            gvLineItem.Columns[0].Visible = false;
            //gvLineItem.Columns[8].Visible = false;
            //gvLineItem.Columns[9].Visible = false;
            gvLineItem.Columns[10].Visible = false;
            gvLineItem.Columns[11].Visible = false;
            gvLineItem.Columns[12].Visible = false;
            gvLineItem.Columns[13].Visible = false;
            gvLineItem.Columns[14].Visible = false;
        }
        else if (ViewState["callfrom"].ToString() == "PR")
        {
            //gvLineItem.Columns[4].Visible = false;
            gvLineItem.Columns[1].Visible = false;
            gvLineItem.Columns[5].Visible = false;
            gvLineItem.Columns[6].Visible = false;
            gvLineItem.Columns[7].Visible = false;
        }
        if (e.Item is GridEditableItem)
        {
            //ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            //if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            ImageButton db1 = (ImageButton)e.Item.FindControl("cmdPOReconsilationStaging");
            ImageButton db2 = (ImageButton)e.Item.FindControl("cmdPOPostInvoiceStaging");
            RadLabel lbl = (RadLabel)e.Item.FindControl("lblApproval");
            ImageButton db3 = (ImageButton)e.Item.FindControl("cmdApproved");
            ImageButton db4 = (ImageButton)e.Item.FindControl("cmdAwaitingForApproval");
            ImageButton db5 = (ImageButton)e.Item.FindControl("cmdPOApprove");
            //db5.Attributes.Add("onclick", "parent.Openpopup('approval', '', '../Common/CommonApproval.aspx?docid=" + drv["FLDINVOICELINEITEMCODE"].ToString() + "&mod=PURCHASE"
            //        + "&type=" + drv["FLDINVOICEAPPROVALTYPE"].ToString() + "&user=" + drv["FLDTECHDIRECTOR"].ToString() + "," + drv["FLDFLEETMANAGER"].ToString() + "," + drv["FLDSUPT"].ToString()
            //        + "&v=invoice&vslid=" + drv["FLDVESSELID"].ToString() + "');return false;");


            RadLabel lblPOPayableAmount = (RadLabel)e.Item.FindControl("lblPOPayableAmount");
            if (lblPOPayableAmount != null && lblPOPayableAmount.Text != "")
            {

                POPayableAmount = POPayableAmount + Convert.ToDecimal(lblPOPayableAmount.Text);
                String.Format("{0:0.00}", POPayableAmount);
            }

            if (db1 != null && ViewState["callfrom"].ToString() == "invoice" && (db2 != null))
            {
                // db1.Attributes.Add("style", "visibility:hidden");
                db2.Attributes.Add("style", "visibility:hidden");
            }
            if (db1 != null && ViewState["callfrom"].ToString() == "invoiceforpurchase" && (db2 != null))
            {
                // db1.Attributes.Add("style", "visibility:hidden");
                db2.Attributes.Add("style", "visibility:hidden");
            }
            if (db2 != null && ViewState["callfrom"].ToString() == "AD")
            {
                db2.Attributes.Add("style", "visibility:hidden");
            }
            if (db1 != null && ViewState["callfrom"].ToString() == "PR")
            {
                db1.Attributes.Add("style", "visibility:hidden");
                PhoenixToolbar toolbargrid = new PhoenixToolbar();
                toolbargrid.AddImageButton("../Accounts/AccountsInvoiceLineItemDetails.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbargrid.AddImageLink("javascript:CallPrint('gvInvoice')", "Print Grid", "icon_print.png", "PRINT");

                toolbargrid.AddImageLink("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsInvoiceLineItem.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString() + "'); return false;", "Add", "Add.png", "ADD");
                short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
                if (showcreditnotedisc == 1)
                    toolbargrid.AddImageButton("../Accounts/AccountsInvoiceLineItemDetails.aspx", "Post Voucher", "pr.png", "CREATEPR");

                MenuOrderLineItem.AccessRights = this.ViewState;
                MenuOrderLineItem.MenuList = toolbargrid.Show();
            }
            if (lbl != null && lbl.Text != "" && db3 != null && db4 != null)
            {
                int i = Convert.ToInt32(lbl.Text);
                if (i == 0)
                {
                    db3.Attributes.Add("style", "visibility:hidden");
                    //db5.Enabled = true;
                    //db5.Visible = true;
                }
                else if (i == 1)
                    db4.Attributes.Add("style", "visibility:hidden");
                else
                {
                    db3.Attributes.Add("style", "visibility:hidden");
                    db4.Attributes.Add("style", "visibility:hidden");
                }
            }
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkAdvanceAmount");
            RadLabel lblPayment = (RadLabel)e.Item.FindControl("lblAdvPayment");
            if (lb != null && lb.Text != "" && lblPayment != null)
            {
                if (Convert.ToDouble(lb.Text) == 0.00)
                {
                    lb.Visible = false;
                    lblPayment.Visible = true;
                }
                else
                {
                    lb.Visible = true;
                    lblPayment.Visible = false;
                }
            }
            //if (db5 != null && ViewState["callfrom"].ToString() == "PR")
            //{
            //    db5.Attributes.Add("style", "visibility:hidden");
            //}
            //if (db5 != null && ViewState["callfrom"].ToString() == "invoice")
            //{
            //    db5.Attributes.Add("style", "visibility:hidden");
            //}

        }

        if (e.Item is GridEditableItem)
        {
            RadLabel lblOffset = (RadLabel)e.Item.FindControl("lblOffset");
            ImageButton db = (ImageButton)e.Item.FindControl("cmdOffset");

            RadLabel lblInvoiceCurrencyId = (RadLabel)e.Item.FindControl("lblInvoiceCurrencyId");
            RadLabel lblPOCurrencyId = (RadLabel)e.Item.FindControl("lblPOCurrencyId");
            RadLabel lblPOCurrency = (RadLabel)e.Item.FindControl("lblPOCurrency");
            RadLabel lblPurPayableAmount = (RadLabel)e.Item.FindControl("lblPurPayableAmount");
            RadLabel lblInvoicePayableAmount = (RadLabel)e.Item.FindControl("lblInvoicePayableAmount");

            if (lblInvoicePayableAmount.Text != string.Empty)
            {
                InvoicePayableAmount = decimal.Parse(lblInvoicePayableAmount.Text);
            }

            if (lblInvoiceCurrencyId.Text != lblPOCurrencyId.Text)
            {
                lblPOCurrency.ForeColor = System.Drawing.Color.Red;
            }

            // if (lblOffset.Text != "1")
            // {
            //     db.Visible = false;
            // }

            ImageButton imgpo = new ImageButton();
            imgpo = (ImageButton)e.Item.FindControl("imgReceivedBeforeInvoice");
            RadTextBox txtOrderId = (RadTextBox)e.Item.FindControl("txtOrderId");
            RadLabel lnkPurchaseOrderNumber = (RadLabel)e.Item.FindControl("lnkPurchaseOrderNumber");

            if (drv["FLDISINVOICEREGISTEREDBEFOREPO"].ToString() == "1" && imgpo != null)
                imgpo.Visible = true;
            if (imgpo != null)
            {
                imgpo.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','../Accounts/AccountsInvoiceLineItemPOReceivedBeforeInvoice.aspx?InvoiceCode=" + ViewState["invoicecode"].ToString()
                    + "&OrderID=" + txtOrderId.Text + "&PoNumber=" + lnkPurchaseOrderNumber.Text + "'); return true;");
            }
        }
        if (e.Item is GridEditableItem)
        {
            LinkButton lb = ((LinkButton)e.Item.FindControl("lnkAdvanceAmount"));

            RadTextBox txtOrderId = ((RadTextBox)e.Item.FindControl("txtOrderId"));
            RadLabel lbl = ((RadLabel)e.Item.FindControl("lnkPurchaseOrderNumber"));
            ViewState["ponumber"] = lbl.Text;

            RadLabel lblVesselName = ((RadLabel)e.Item.FindControl("lblVesselName"));
            RadLabel lblStockType = ((RadLabel)e.Item.FindControl("lblStockType"));
            RadLabel lblVesselId = ((RadLabel)e.Item.FindControl("lblVesselId"));

            if (General.GetNullableInteger(lblVesselId.Text) != null)
            {
                NameValueCollection criteria = new NameValueCollection();

                PhoenixSecurityContext.CurrentSecurityContext.VesselID = int.Parse(lblVesselId.Text);
                Filter.CurrentPurchaseVesselSelection = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                PhoenixSecurityContext.CurrentSecurityContext.VesselName = lblVesselName.Text;

                criteria.Clear();
                criteria.Add("ucVessel", lblVesselId.Text);
                criteria.Add("ddlStockType", lblStockType.Text);
                criteria.Add("txtNumber", lbl.Text);
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

                DataSet ds = PhoenixGeneralSettings.GetUserOptions(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                PhoenixGeneralSettings.CurrentGeneralSetting = new GeneralSetting(ds);

                ImageButton cmdViewPOForm = (ImageButton)e.Item.FindControl("cmdViewPOForm");
                if (cmdViewPOForm != null)
                {
                    cmdViewPOForm.Visible = SessionUtil.CanAccess(this.ViewState, cmdViewPOForm.CommandName);
                    if (ViewState["SHORTNAME"] != null && ViewState["SHORTNAME"].ToString() == "AVN")
                        cmdViewPOForm.Visible = false;

                    //cmdViewPOForm.Attributes.Add("onclick", "javascript:parent.Openpopup('chml','','../Purchase/PurchaseForm.aspx?launchedfrom=ACCOUNTS'); return false;");

                    if (drv["FLDORDERFORMTYPE"].ToString() == "1")
                    {
                        cmdViewPOForm.Visible = true;
                    }
                    else
                        cmdViewPOForm.Visible = false;
                }

                ImageButton cmdPurAttachment = (ImageButton)e.Item.FindControl("cmdPurAttachment");
                if (cmdPurAttachment != null)
                {
                    cmdPurAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdPurAttachment.CommandName);
                    if (ViewState["SHORTNAME"] != null && ViewState["SHORTNAME"].ToString() == "AVN")
                        cmdPurAttachment.Visible = false;

                    cmdPurAttachment.Attributes.Add("onclick", "javascript:parent.Openpopup('chml','','../Purchase/PurchaseAttachments.aspx?orderid=" + txtOrderId.Text + "&copydtkey=" + drv["FLDDTKEY"].ToString() + "&launchedfrom=ACCOUNTS&MOD=PURCHASE'); return false;");

                    if (drv["FLDORDERFORMTYPE"].ToString() == "1")
                    {
                        cmdPurAttachment.Visible = true;
                    }
                    else
                        cmdPurAttachment.Visible = false;
                }
            }
            //cmdMoreInfo.Attributes.Add("onclick", "openNewWindow('codeHelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsInvoiceMoreInfo.aspx?invoiceCode=" + txtInvoiceCode.Text + "');return false;");

            lb.Attributes.Add("onclick", "parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsAdvancePaymentDetails.aspx?orderid=" + txtOrderId.Text + "&ponumber=" + lbl.Text + "');return false;");

        }
    }

    protected void MenuOrderLineItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CREATEPR"))
            {
                if (ViewState["invoicestatus"].ToString() == "243")
                {
                    string dtKey = ViewState["DTKEY"].ToString();
                    BindAttachment(dtKey);
                }
                else
                {
                    ucError.ErrorMessage = " Unable to Post Invoice, Invoice status is not Accounts Checking";
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

    protected void BindAttachment(string dtkey)
    {
        DataSet dsattachment = new DataSet();
        DataSet dsvoucherlevelattachment = new DataSet();

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string Attachmenttype = "Invoice";
        dsattachment = PhoenixCommonFileAttachment.AttachmentSearch(new Guid(dtkey), null, Attachmenttype, null, null, 1, General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
        if (dsattachment.Tables[0].Rows.Count > 0)
        {
            ReconcileInvoiceWithPurchaseOrder(ViewState["invoicecode"].ToString());
        }
        else
        {

            ucError.ErrorMessage = "Invalid Invoice";
            ucError.Visible = true;
            return;
        }
    }
    private void ReconcileInvoiceWithPurchaseOrder(string strInvoiceCode)
    {
        try
        {
            Response.Redirect("../Accounts/AccountsInvoiceLedgerPostingConfirmationcheck.aspx?qinvoicecode=" + strInvoiceCode);
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }
    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;
        if (ViewState["callfrom"].ToString() == "invoice" || ViewState["callfrom"].ToString() == "invoiceforpurchase")
        {
            string[] alCaptions = {
                                "PO Number",
                                "Vessel Name",
                                "PO Currency",
                                "PO Additional Charges",
                                "PO Payable Amount",
                                "Invoice Payable Amount",
                                "Receipt Last Updated By"
                              };

            string[] alColumns = {
                                "FLDPURCHASEORDERNUMBER",
                                "FLDVESSELNAME",
                                "FLDPOCURRENCYCODE",
                                "FLDADDITIONALCOST",
                                "FLDPURCHASEPAYABLEAMOUNT",
                                "FLDINVOICEPAYABLEAMOUNT",
                                "FLDPORECEIPTLASTUPDATEDBY"
                             };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            ds = PhoenixAccountsInvoice.InvoiceLineItemSearch(
                                                                    new Guid(ViewState["invoicecode"].ToString())
                                                                    , null
                                                                    , string.Empty
                                                                    , sortexpression
                                                                    , sortdirection
                                                                    , (int)ViewState["PAGENUMBER"]
                                                                    , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount
                                                                );

            Response.AddHeader("Content-Disposition", "attachment; filename=AccountsInvoicePO.xls");

            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Line Items</h3></td>");
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
        else if (ViewState["callfrom"].ToString() == "AD")
        {
            string[] alCaptions = {
                                    "PO Number",
                                    "Vessel Name",
                                    "PO Currency",
                                    "PO Committed Amount",
                                    "PO Short Delivery Amount",
                                    "PO Additional Charges",
                                    "PO Payable Amount",
                                    "Advance Payment",
                                    "Invoice Payable Amount",
                                    "Receipt Last Updated By"
                              };

            string[] alColumns = {
                                "FLDPURCHASEORDERNUMBER",
                                "FLDVESSELNAME",
                                "FLDPOCURRENCYCODE",
                                "FLDCOMMITTEDAMOUNT",
                                "FLDSHORTDELIVERYAMOUNT",
                                "FLDADDITIONALCOST",
                                "FLDPURCHASEPAYABLEAMOUNTWITHINVOICEDIFF",
                                "FLDPAYMENT",
                                "FLDINVOICEPAYABLEAMOUNT",
                                "FLDPORECEIPTLASTUPDATEDBY"
                             };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            ds = PhoenixAccountsInvoice.InvoiceLineItemSearch(
                                                                    new Guid(ViewState["invoicecode"].ToString())
                                                                    , null
                                                                    , string.Empty
                                                                    , sortexpression
                                                                    , sortdirection
                                                                    , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount
                                                                );

            Response.AddHeader("Content-Disposition", "attachment; filename=AccountsInvoicePO.xls");

            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Line Items</h3></td>");
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
        else if (ViewState["callfrom"].ToString() == "PR")
        {
            string[] alCaptions = {
                                "PO Number",
                                "Vessel Name",
                                "PO Currency",
                                "PO Payable Amount.",
                                 "Advance Payment",
                                "GST On PO Payable",
                                "GST On Discount",
                                "Total Discount Amount.",
                                "Vessel Allocated Discount ",
                                "Service Charge",
                                "Invoice Payable Amount.",
                                "Receipt Last Updated By"
                                //"PO Payable Amount",                               
                                //"Total Payable Amount"                                                      
                              };

            string[] alColumns = {
                                "FLDPURCHASEORDERNUMBER",
                                "FLDVESSELNAME",
                                "FLDPOCURRENCYCODE",
                                "FLDPURCHASEPAYABLEAMOUNT",
                                "FLDPAYMENT",
                                "FLDGSTAMOUNT",
                                "FLDGSTDISCOUNTAMOUNT",
                                "FLDORDERFORMTOTALDISCOUNTAMOUNT",
                                "FLDVESSELALLOCATEDDISCOUNTAMOUNT",
                                "FLDSERVICECHARGE",
                                "FLDINVOICEPAYABLEAMOUNT",
                                "FLDPORECEIPTLASTUPDATEDBY"
                             };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            ds = PhoenixAccountsInvoice.InvoiceLineItemSearch(
                                                                    new Guid(ViewState["invoicecode"].ToString())
                                                                    , null
                                                                    , string.Empty
                                                                    , sortexpression
                                                                    , sortdirection
                                                                    , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount
                                                                );

            Response.AddHeader("Content-Disposition", "attachment; filename=AccountsInvoicePO.xls");

            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Line Items</h3></td>");
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
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["invoicecode"] != null)
        {
            ds = PhoenixAccountsInvoice.InvoiceLineItemSearch(
                                                           new Guid(ViewState["invoicecode"].ToString())
                                                           , null
                                                           , string.Empty
                                                           , sortexpression
                                                           , sortdirection
                                                           , (int)ViewState["PAGENUMBER"], gvLineItem.PageSize
                                                           , ref iRowCount
                                                           , ref iTotalPageCount
                                                       );

            gvLineItem.DataSource = ds;
            gvLineItem.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                TotalPOPayableAmount = string.Format(String.Format("{0:#####.00}", ds.Tables[0].Rows[0]["FLDTOTALPURCHASEPAYABLEAMOUNT"]));
                ViewState["invoicestatus"] = ds.Tables[0].Rows[0]["FLDINVOICESTATUS"].ToString();
                if (ViewState["invoicelineitemcode"] == null)
                {
                    ViewState["invoicelineitemcode"] = ds.Tables[0].Rows[0]["FLDINVOICELINEITEMCODE"].ToString();
                }

                if (ViewState["PAGEURL"] == null)
                {
                    ViewState["PAGEURL"] = "../Accounts/AccountsInvoiceLineItem.aspx?qinvoicecode=";
                }
                {
                    if (ViewState["invoicelineitemcode"] != null)
                    {
                        string strRowno = string.Empty;
                    }
                }
            }



            if (ViewState["callfrom"].ToString() == "invoice" || ViewState["callfrom"].ToString() == "invoiceforpurchase")
            {
                string[] alCaptions = {
                                "PO Number",
                                "Vessel Name",
                                "PO Currency",
                                "PO Additional Charges",
                                "PO Payable Amount",
                                "Invoice Payable Amount",
                                "Receipt Last Updated By"
                              };

                string[] alColumns = {
                                "FLDPURCHASEORDERNUMBER",
                                "FLDVESSELNAME",
                                "FLDPOCURRENCYCODE",
                                "FLDADDITIONALCOST",
                                "FLDPURCHASEPAYABLEAMOUNT",
                                "FLDINVOICEPAYABLEAMOUNT",
                                "FLDPORECEIPTLASTUPDATEDBY"
                             };
                General.SetPrintOptions("gvInvoice", "Invoice Line Item", alCaptions, alColumns, ds);
            }
            else if (ViewState["callfrom"].ToString() == "AD")
            {
                string[] alCaptions = {
                                    "PO Number",
                                    "Vessel Name",
                                    "PO Currency",
                                    "PO Committed Amount",
                                    "PO Short Delivery Amount",
                                    "PO Additional Charges",
                                    "PO Payable Amount",
                                    "Advance Payment",
                                    "Invoice Payable Amount",
                                    "Receipt Last Updated By"
                              };

                string[] alColumns = {
                                "FLDPURCHASEORDERNUMBER",
                                "FLDVESSELNAME",
                                "FLDPOCURRENCYCODE",
                                "FLDCOMMITTEDAMOUNT",
                                "FLDSHORTDELIVERYAMOUNT",
                                "FLDADDITIONALCOST",
                                "FLDPURCHASEPAYABLEAMOUNTWITHINVOICEDIFF",
                                "FLDPAYMENT",
                                "FLDINVOICEPAYABLEAMOUNT",
                                "FLDPORECEIPTLASTUPDATEDBY"
                             };
                General.SetPrintOptions("gvInvoice", "Invoice Line Item", alCaptions, alColumns, ds);
            }
            else if (ViewState["callfrom"].ToString() == "PR")
            {
                string[] alCaptions = {
                                "PO Number",
                                "Vessel Name",
                                "PO Currency",
                                "PO Payable Amount.",
                                 "Advance Payment",
                                "GST On PO Payable",
                                "GST On Discount",
                                "Total Discount Amount.",
                                "Vessel Allocated Discount ",
                                "Service Charge",
                                "Invoice Payable Amount.",
                                "Receipt Last Updated By"
                                //"PO Payable Amount",                               
                                //"Total Payable Amount"                                                      
                              };

                string[] alColumns = {
                                "FLDPURCHASEORDERNUMBER",
                                "FLDVESSELNAME",
                                "FLDPOCURRENCYCODE",
                                "FLDPURCHASEPAYABLEAMOUNTWITHINVOICEDIFF",
                                "FLDPAYMENT",
                                "FLDGSTAMOUNT",
                                "FLDGSTDISCOUNTAMOUNT",
                                "FLDORDERFORMTOTALDISCOUNTAMOUNT",
                                "FLDVESSELALLOCATEDDISCOUNTAMOUNT",
                                "FLDSERVICECHARGE",
                                "FLDINVOICEPAYABLEAMOUNT",
                                "FLDPORECEIPTLASTUPDATEDBY" };
                General.SetPrintOptions("gvInvoice", "Invoice Line Item", alCaptions, alColumns, ds);

                short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
                gvLineItem.Columns[10].Visible = (showcreditnotedisc == 1) ? true : false;
                gvLineItem.Columns[11].Visible = (showcreditnotedisc == 1) ? true : false;
                gvLineItem.Columns[12].Visible = (showcreditnotedisc == 1) ? true : false;
                gvLineItem.Columns[13].Visible = (showcreditnotedisc == 1) ? true : false;
                gvLineItem.Columns[14].Visible = (showcreditnotedisc == 1) ? true : false;

            }
        }
    }
    protected void gvLineItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        RadGrid _gridView = (RadGrid)sender;
        string[] strValues = new string[2];
        strValues = e.CommandArgument.ToString().Split('^');

        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName.ToUpper().Equals("OFFSET"))
            {
                return;
            }

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["txtInvoiceLineItemCode"] = ((RadTextBox)e.Item.FindControl("txtInvoiceLineItemCode")).Text.ToString();
                ViewState["txtOrderId"] = ((RadTextBox)e.Item.FindControl("txtOrderId")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to delete this record?", "confirmdelete", 420, 150, null, "Confirm Message");
                gvLineItem.Rebind();

            }
            if (e.CommandName.ToUpper().Equals("PORECONSILATIONSTAGING"))
            {
                Filter.CurrentPurchaseStockType = ((RadLabel)e.Item.FindControl("lblStockType")).Text;
                string strOrderId = ((RadTextBox)e.Item.FindControl("txtOrderId")).Text;
                Response.Redirect("AccountsReconcilationStagingRWAForPurchase.aspx?ORDERID=" + strOrderId + "&qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qcallfrom=" + ViewState["callfrom"].ToString() + "&qfrom=" + ViewState["callfrom"].ToString());
            }
            if (e.CommandName.ToUpper().Equals("POPOSTINVOICESTAGING"))
            {
                Filter.CurrentPurchaseStockType = ((RadLabel)e.Item.FindControl("lblStockType")).Text;
                string strOrderId = ((RadTextBox)e.Item.FindControl("txtOrderId")).Text;
                Response.Redirect("AccountsPostInvoiceStaging.aspx?ORDERID=" + strOrderId + "&qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qcallfrom=PR");
            }
            if (e.CommandName.ToUpper().Equals("ORDER"))
            {
                Filter.CurrentPurchaseStockType = ((RadLabel)e.Item.FindControl("lblStockType")).Text;
                string strOrderId = ((RadTextBox)e.Item.FindControl("txtOrderId")).Text;
                if (Filter.CurrentPurchaseStockType == "MEDICAL")
                {
                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=MEDICALSLIP&emailyn=1&medicalinvoiceyn=1&reqid=" + strOrderId + "&qinvoicecode=" + ViewState["invoicecode"] + "&qcallfrom=" + ViewState["callfrom"] + "&showactual=0", false);
                }
                else if (Filter.CurrentPurchaseStockType == "TRAVEL")
                {
                    return;
                }
                else
                {
                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=3&reportcode=ACCOUNTSORDERFORM&quotationid=&orderid=" + strOrderId + "&qinvoicecode=" + ViewState["invoicecode"] + "&qcallfrom=" + ViewState["callfrom"] + "&showactual=0");
                }
            }
            if (e.CommandName.ToUpper().Equals("POAPPROVE"))
            {
                string strOrderId = ((RadTextBox)e.Item.FindControl("txtOrderId")).Text;
                UpdateApprovalStatus(strOrderId);
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("POREFRESH"))
            {
                string strOrderId = ((RadTextBox)e.Item.FindControl("txtOrderId")).Text;
                RadLabel lblPurchaseOrderNumber = (RadLabel)e.Item.FindControl("lnkPurchaseOrderNumber");
                ViewState["orderid"] = strOrderId;
                RadWindowManager1.RadConfirm(lblPurchaseOrderNumber.Text + " - " + "All the Matching changes will be reset when the PO gets refreshed. Do you want to continue.?", "confirm ", 420, 150, null, "Confirm Message");

                //ucConfirm.Text = lblPurchaseOrderNumber.Text + " - " + "All the Reconciliation changes will be reset when the PO gets refreshed. Do you want to continue.?";
            }
            if (e.CommandName.ToUpper().Equals("PURCHASEFORM"))
            {
                RadTextBox txtOrderId = ((RadTextBox)e.Item.FindControl("txtOrderId"));
                RadLabel lbl = ((RadLabel)e.Item.FindControl("lnkPurchaseOrderNumber"));
                ViewState["ponumber"] = lbl.Text;

                RadLabel lblVesselName = ((RadLabel)e.Item.FindControl("lblVesselName"));
                RadLabel lblStockType = ((RadLabel)e.Item.FindControl("lblStockType"));
                RadLabel lblVesselId = ((RadLabel)e.Item.FindControl("lblVesselId"));

                if (General.GetNullableInteger(lblVesselId.Text) != null)
                {
                    NameValueCollection criteria = new NameValueCollection();

                    PhoenixSecurityContext.CurrentSecurityContext.VesselID = int.Parse(lblVesselId.Text);
                    Filter.CurrentPurchaseVesselSelection = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                    PhoenixSecurityContext.CurrentSecurityContext.VesselName = lblVesselName.Text;

                    criteria.Clear();
                    criteria.Add("ucVessel", lblVesselId.Text);
                    criteria.Add("ddlStockType", lblStockType.Text);
                    criteria.Add("txtNumber", lbl.Text);
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

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "openNewWindow('chml','','" + Session["sitepath"] + "/Purchase/PurchaseForm.aspx?launchedfrom=ACCOUNTS');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void gvLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLineItem.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLineItem_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvLineItem, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }


    private void UpdateApprovalStatus(string orderid)
    {
        PhoenixAccountsPOStaging.POStagingApprovalStatusUpdate(new Guid(orderid), PhoenixSecurityContext.CurrentSecurityContext.UserCode);
    }
    private void PORefresh(string orderid)
    {
        PhoenixAccountsInvoice.InvoicePORefresh(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(orderid));
    }

    protected void confirm_Click(object sender, EventArgs e)
    {
        try
        {

            if (ViewState["orderid"] != null)
                PORefresh(ViewState["orderid"].ToString());
            POPayableAmount = 0;

            Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void confirmdelete_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixAccountsInvoice.InvoiceLineItemDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["txtInvoiceLineItemCode"].ToString()));
            PhoenixAccountsPOStaging.OrderFormStagingDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["txtOrderId"].ToString()));
            Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
}
