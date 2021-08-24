using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using System.Text;
using Telerik.Web.UI;
using SouthNests.Phoenix.Export2XL;

public partial class PurchaseQuotationRFQ : PhoenixBasePage
{
    Label lblvesselid = new Label();
   
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            if (PhoenixSecurityContext.CurrentSecurityContext == null)
                PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;

            //CheckWebSessionStatus();

            

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseQuotationRFQ.aspx?SESSIONID=" + Request.QueryString["SESSIONID"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            DataSet d1 = PhoenixPurchaseQuotation.ArcVendorYN(new Guid(Request.QueryString["SESSIONID"].ToString()));
            string arcyn = "0";
            if (d1.Tables[0].Rows.Count > 0)
            {
                arcyn = d1.Tables[0].Rows[0]["FLDARCYN"].ToString();
            }
            if (arcyn == "1")
            {
                toolbargrid.AddImageButton("../Purchase/PurchaseQuotationRFQ.aspx?SESSIONID=" + Request.QueryString["SESSIONID"], "Import Items from XL", "icon_xls.png", "EXPORT2XL");
            }

            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvVendorItem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "Print");

            if (!IsPostBack)
            {
                
                if (PhoenixGeneralSettings.CurrentGeneralSetting != null)
                {
                    short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
                    lblCreditNoteDisc.Visible = (showcreditnotedisc == 1) ? true : false;
                    txtDiscount.Visible = (showcreditnotedisc == 1) ? true : false;
                }
                else
                {
                    DataSet dsgeneralsettings = PhoenixGeneralSettings.GetUserOptions(PhoenixSecurityContext.SystemSecurityContext.UserCode);
                    if (dsgeneralsettings.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = dsgeneralsettings.Tables[0].Rows[0];
                        short showcreditnotedisc = short.Parse(dr["FLDSHOWCREDITNOTE"].ToString());
                        lblCreditNoteDisc.Visible = (showcreditnotedisc == 1) ? true : false;
                        txtDiscount.Visible = (showcreditnotedisc == 1) ? true : false;
                    }
                }


                UCDeliveryTerms.QuickTypeCode = ((int)PhoenixQuickTypeCode.DELIVERYTERM).ToString();
                UCPaymentTerms.QuickTypeCode = ((int)PhoenixQuickTypeCode.PAYMENTTERM).ToString();

                if (Request.QueryString["STOCKTYPE"] != null)
                {
                    Filter.CurrentPurchaseStockType = Request.QueryString["STOCKTYPE"].ToString();
                }
                if (Request.QueryString["SESSIONID"] != null)
                {
                    ViewState["quotationid"] = Request.QueryString["SESSIONID"].ToString();
                }
                else
                {
                    ViewState["quotationid"] = "";
                }
                if (Filter.CurrentPurchaseStockType != null && General.GetNullableGuid(ViewState["quotationid"].ToString()) != null)
                    PhoenixPurchaseQuotationLine.CloseQuotationSession(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["quotationid"].ToString()), Filter.CurrentPurchaseStockType);
                CheckWebSessionStatus();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["FORMNO"] = "";
                bindPrice();
                BindVendorInfo();
                EnableFalse();
                ucConfirm.Visible = false;
            }
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','../Reports/ReportsViewWithSubReport.aspx?applicationcode=3&reportcode=PURCHASEORDERFORMVENDOR&quotationid=" + ViewState["quotationid"].ToString() + "&orderid=" + ViewState["ORDERID"] + "'); return false;", "RFQ", "<i class=\"fas fa-file-alt\"></i>", "ORDER");
            if ((Request.QueryString["VIEWONLY"] == null) && (ViewState["WEBSESSIONSTATUS"].ToString() == "Y"))
            {
                toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','../Purchase/PurchaseQuotationVendorBulkSave.aspx?quotationid="
                    + ViewState["quotationid"].ToString() + "&orderid=" + ViewState["ORDERID"].ToString() + "'); return false;", "Bulk Save", "<i class=\"fas fa-database\"></i>", "BULKSAVE");
            }

            MenuRegistersStockItem.MenuList = toolbargrid.Show();

            MenuQuotationLineItem.Title = "Quotation Details [" + ViewState["FORMNO"].ToString() + "]";
            if(General.GetNullableInteger(ViewState["ACCEPTANCEMAILSENTYN"].ToString()) == 1 && General.GetNullableInteger(ViewState["ISSELECTEDFORORDER"].ToString()) == 1 && General.GetNullableInteger(ViewState["ACCEPTREJECTYN"].ToString()) == null)
            {
                toolbarmain.AddButton("Reject", "REJECT", ToolBarDirection.Right);
                toolbarmain.AddButton("Accept", "ACCEPT", ToolBarDirection.Right);
            }
            if ((Request.QueryString["VIEWONLY"] == null) && (ViewState["WEBSESSIONSTATUS"].ToString() == "Y"))
            {
                toolbarmain.AddButton("Decline Quote", "DECLINEQUOTE", ToolBarDirection.Right);
                toolbarmain.AddButton("Attachment", "ATTACHMENT", ToolBarDirection.Right);
                toolbarmain.AddButton("Delivery Instruction", "DELIVERYINSTRUCTION", ToolBarDirection.Right);
                toolbarmain.AddButton("Remarks", "REMARKS", ToolBarDirection.Right);
                toolbarmain.AddButton("Details", "DETAILS", ToolBarDirection.Right);
                toolbarmain.AddButton("Send Quote", "CONFIRM", ToolBarDirection.Right);
                toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);

                MenuQuotationLineItem.MenuList = toolbarmain.Show();
            }
            else
            {
                toolbarmain.AddButton("Attachment", "ATTACHMENT", ToolBarDirection.Right);
                toolbarmain.AddButton("Deliv Instr", "DELIVERYINSTRUCTION", ToolBarDirection.Right);
                toolbarmain.AddButton("Remarks", "REMARKS", ToolBarDirection.Right);
                toolbarmain.AddButton("Details", "DETAILS", ToolBarDirection.Right);
                MenuQuotationLineItem.MenuList = toolbarmain.Show();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }

    private void EnableFalse()
    {
        if ((Request.QueryString["VIEWONLY"] != null) || (ViewState["WEBSESSIONSTATUS"].ToString() == "N"))
        {
            txtExpirationDate.Enabled = false;
            txtVessel.Enabled = false;
            txtIMONo.Enabled = false;
            txtHullNo.Enabled = false;
            txtContactNo.Enabled = false;
            txtSenderEmailId.Enabled = false;
            txtYearBuilt.Enabled = false;
            txtYard.Enabled = false;
            txtVendorName.Enabled = false;
            txtVendorAddress.Enabled = false;
            txtTelephone.Enabled = false;
            txtEmail.Enabled = false;
            txtFax.Enabled = false;
            txtDeliveryPlace.Enabled = false;
            //txtDeliveryLocation.Enabled = false;
            txtOrderDate.Enabled = false;
            txtVenderReference.Enabled = false;
            ucCurrency.Enabled = false;
            //txtDiscount.Enabled = false;
            txtDeliveryTime.Enabled = false;
            txtPriority.Enabled = false;
            txtSenderName.Enabled = false;
            txtSentDate.Enabled = false;
            txtPrice.Enabled = false;
            txtTotalCharges.Enabled = false;
            //txtTotalDiscount.Enabled = false;
            txtTotalPrice.Enabled = false;
            UCDeliveryTerms.Enabled = false;
            UCPaymentTerms.Enabled = false;
            txtPortName.Enabled = false;
            txtComponentName.Enabled = false;
            txtComponentModel.Enabled = false;
            txtComponentSerialNo.Enabled = false;
            //txtSupplierDiscount.Enabled = false;
            //txtVendorRemarks.Enabled = false; 
        }
    }

    private void CheckWebSessionStatus()
    {
        DataTable dt = PhoenixCommoneProcessing.GetSessionStatus(new Guid(Request.QueryString["SESSIONID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            ViewState["WEBSESSIONSTATUS"] = dt.Rows[0]["FLDACTIVE"].ToString();
        }
    }

    private void BindVendorInfo()
    {
        DataSet dsVendor = PhoenixPurchaseQuotation.QuotationHeader(new Guid(ViewState["quotationid"].ToString()));
        if (dsVendor.Tables[0].Rows.Count > 0)
        {
            DataRow dr = dsVendor.Tables[0].Rows[0];

            txtExpirationDate.Text = General.GetDateTimeToString(dr["FLDEXPIRYDATE"].ToString());

            ViewState["FORMNO"] = dr["FLDFORMNO"].ToString();
            txtVessel.Text = dr["FLDVESSELNAME"].ToString();
            hndVesselID.Value = dr["FLDVESSELID"].ToString();
            txtIMONo.Text = dr["FLDIMONUMBER"].ToString();
            txtHullNo.Text = dr["FLDHULLNUMBER"].ToString();
            txtVesseltype.Text = dr["FLDVESSELTYPE"].ToString();
            txtContactNo.Text = dr["FLDCONTACTNUMBER"].ToString();
            txtYard.Text = dr["FLDYARDNAME"].ToString();
            txtYearBuilt.Text = General.GetDateTimeToString(dr["FLDDATEENTERED"].ToString());
            txtSenderEmailId.Text = dr["FLDSENDEREMAILID"].ToString();
            txtVendorName.Text = dr["FLDNAME"].ToString();
            txtVendorAddress.Text = dr["FLDVENDORADDRESS"].ToString();
            txtTelephone.Text = dr["FLDVENDORPHONE"].ToString();
            txtEmail.Text = dr["FLDVENDOREMAIL"].ToString();
            txtFax.Text = dr["FLDVENDORFAX"].ToString();

            txtDeliveryPlace.Text = dr["FLDDELIVERYADDRESSCODE"].ToString() + "-" + dr["FLDDELIVERYADDRESS"].ToString();
            //txtDeliveryLocation.Text = dr["FLDDELIVERYCODE"].ToString() + "-" + dr["FLDDELIVERYPLACE"].ToString();
            //txtRemarksByPurchaser.Text = dr["FLDPURCHASERREMARKS"].ToString();

            txtOrderDate.Text = General.GetDateTimeToString(dr["FLDORDERBEFOREDELIVERYDATE"].ToString());
            txtVenderReference.Text = dr["FLDVENDORQUOTATIONREF"].ToString();

            ucCurrency.Vendor = General.GetNullableInteger(dr["FLDVENDORID"].ToString()) != null ? int.Parse(dr["FLDVENDORID"].ToString()) : 0;
            ucCurrency.bind();
            ucCurrency.DataBind();

            if (dr["FLDQUOTEDCURRENCYID"] != null && dr["FLDQUOTEDCURRENCYID"].ToString().Trim() != "" && dr["FLDQUOTEDCURRENCYID"].ToString().Trim() != "0")
                ucCurrency.SelectedCurrency = dr["FLDQUOTEDCURRENCYID"].ToString();
            //else
            //    ucCurrency.SelectedCurrency = PhoenixPurchaseOrderForm.DefaultCurrency;

            txtDiscount.Text = String.Format("{0:##.0000000}", dr["FLDDISCOUNT"]);
            txtDeliveryTime.Text = dr["FLDDELIVERYTIME"].ToString();
            txtPriority.Text = dr["FLDORDERPRIORITY"].ToString();
            txtSenderName.Text = dr["FLDQUOTATIONSENTBY"].ToString().ToUpper();
            txtSentDate.Text = General.GetNullableDateTime(dr["FLDQUOTATIONSENTDATE"].ToString()) != null ? General.GetDateTimeToString(dr["FLDQUOTATIONSENTDATE"].ToString()) : "";
            txtComponentName.Text = dr["FLDORDERFORMCOMPONENT"].ToString();
            txtComponentModel.Text = dr["FLDORDERFORMCOMPONENTMODEL"].ToString();
            txtComponentSerialNo.Text = dr["FLDORDERFORMCOMPONENTSERIALNO"].ToString();
            UCDeliveryTerms.SelectedQuick = dr["FLDVENDORDELIVERYTERMID"].ToString();
            UCPaymentTerms.SelectedQuick = dr["FLDVENDORPAYMENTTERMID"].ToString();
            ucModeOfTransport.SelectedQuick = dr["FLDMODEOFTRANSPORT"].ToString();
            txtTotalDiscount.Text = String.Format("{0:###,###,###,##0.00}", decimal.Parse(dr["FLDBULKDISCOUNT"].ToString()));
            //txtVendorRemarks.Text = dr["FLDVENDORREMARKS"].ToString();
            ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
            ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
            ViewState["ORDERID"] = dr["FLDORDERID"].ToString();
            ViewState["ACCEPTANCEMAILSENTYN"] = dr["FLDACCEPTANCEMAILSENTYN"].ToString();
            ViewState["ISSELECTEDFORORDER"] = dr["FLDISSELECTEDFORORDER"].ToString();
            ViewState["ACCEPTREJECTYN"] = dr["FLDACCEPTREJECTYN"].ToString();
            ddlType.SelectedHard = dr["FLDDRAFTITEMTYPE"].ToString();

            if(dr["FLDCLASSOYN"].ToString() == "1")
            {
                txtDiscount.Visible = false;
                lblCreditNoteDisc.Visible = false;
            }
        }

        DataSet dsQ = PhoenixPurchaseQuotation.EditQuotation(new Guid(ViewState["quotationid"].ToString()));
        if (dsQ.Tables[0].Rows.Count > 0)
        {
            DataRow drQ = dsQ.Tables[0].Rows[0];
            txtPortName.Text = drQ["FLDPORTNAME"].ToString();
            txtSupplierDiscount.Text = drQ["FLDSUPPLIERDISCOUNT"].ToString();
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
            alColumns = new string[13] {"FLDNUMBER","FLDMAKERREFERENCE", "FLDPOSITIONDRAWING", "FLDNAME", "FLDDETAIL", "FLDVENDORNOTES", "FLDORDEREDQUANTITY",
                                 "FLDQUANTITY", "FLDUNITNAME", "FLDQUOTEDPRICE", "FLDDISCOUNT", "TOTALPRICE", "FLDDELIVERYTIME" };
            alCaptions = new string[13] {"Number","Maker Reference", "Drawing No/Position", "Part Name", "Details", "Remark", "Order Qty",
                                 "Quoted Qty", "Unit", "Unit Price", "%Discount", "Total Price", "Del. Time(Days)" };
        }
        else
        {
            alColumns = new string[13] {"FLDNUMBER","FLDMAKERREFERENCE", "FLDPOSITIONDRAWING", "FLDNAME", "FLDDETAIL", "FLDVENDORNOTES", "FLDORDEREDQUANTITY",
                                 "FLDQUANTITY", "FLDUNITNAME", "FLDQUOTEDPRICE", "FLDDISCOUNT", "TOTALPRICE", "FLDDELIVERYTIME" };
            alCaptions = new string[13] {"Number","Product Code", "Drawing No/Position", "Part Name", "Details", "Remark", "Order Qty",
                                 "Quoted Qty", "Unit", "Unit Price", "%Discount", "Total Price", "Del. Time(Days)" };
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

        ds = PhoenixPurchaseQuotationLine.QuotationLineSearchForVendor("", ViewState["quotationid"].ToString(), sortexpression, sortdirection, 1,
           iRowCount,
           ref iRowCount,
           ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=VendorItems.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");

        Response.Write("<td><h3>Quotation Items</h3></td>");
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

    private void bindPrice()
    {
        if (ViewState["quotationid"] != null && ViewState["quotationid"].ToString() != string.Empty)
        {
            DataSet dsprice = PhoenixPurchaseQuotationLine.GetQuotationPrice(new Guid(ViewState["quotationid"].ToString()));
            if (dsprice.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsprice.Tables[0].Rows[0];
                txtPrice.Text = String.Format("{0:###,###,###,##0.00}", decimal.Parse(dr["FLDPRICE"].ToString()));
                txtTotalCharges.Text = dr["FLDTOTALCHARGES"].ToString();
                //txtTotalDiscount.Text = String.Format("{0:###,###,###,##0.00}", decimal.Parse(dr["FLDDISCOUNTVALUE"].ToString()));
                txtTotalPrice.Text = String.Format("{0:###,###,###,##0.00}", decimal.Parse(dr["FLDTOTALPRICE"].ToString()));
            }
        }

    }

    protected void RegistersStockItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvVendorItem.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("EXPORT2XL"))
            {
                PhoenixPurchase2XL.Export2XLPurchaseQuotationItems(new Guid(ViewState["quotationid"].ToString()), int.Parse(ViewState["VESSELID"].ToString()), new Guid(ViewState["ORDERID"].ToString()));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVendorItem_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            //if ((Request.QueryString["VIEWONLY"] == null) && (ViewState["WEBSESSIONSTATUS"].ToString() == "Y"))
            //{

            //    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvVendorItem, "Edit$" + e.Row.RowIndex.ToString(), false);
            //}
        }
        SetKeyDownScroll(sender, e);
    }
    
    protected void gvVendorItem_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
            Label lbm = (Label)e.Row.FindControl("lblMakerReferenceHeader");
            if (lbm != null && Filter.CurrentPurchaseStockType == "STORE")
                lbm.Text = "Product Code";

        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            Label lbl = (Label)e.Row.FindControl("lblQuotationLineId");

            ImageButton img = (ImageButton)e.Row.FindControl("imgVendorNotes");
            if (Request.QueryString["VIEWONLY"] == "Y")
                img.Attributes.Add("onclick", "javascript:showMoreInformation(ev, 'PurchaseFormItemMoreInfo.aspx?quotationlineid=" + lbl.Text + "&viewsession=N'); return false;");
            else
                img.Attributes.Add("onclick", "javascript:showMoreInformation(ev, 'PurchaseFormItemMoreInfo.aspx?quotationlineid=" + lbl.Text + "&viewsession=" + ViewState["WEBSESSIONSTATUS"].ToString() + "'); return false;");


            Label lbl1 = (Label)e.Row.FindControl("lbldtkey");
            ImageButton imgAttach = (ImageButton)e.Row.FindControl("cmdViewAttachment");
            if (imgAttach != null) imgAttach.Attributes.Add("onclick", "Openpopup('AddAddress', '', '../Common/CommonFileAttachment.aspx?DTKEY=" + lbl1.Text + "&MOD=" + PhoenixModule.PURCHASE + "&U=N'); return false;");

            Label lbl2 = (Label)e.Row.FindControl("lblOrderLineId");
            ImageButton imgDetails = (ImageButton)e.Row.FindControl("cmdDetails");
            if (imgDetails != null) imgDetails.Attributes.Add("onclick", "Openpopup('AddAddress', '', 'PurchaseFormItemComment.aspx?orderlineid=" + lbl2.Text + "&viewonly=Y'); return false;");


            Label lblComponentName = (Label)e.Row.FindControl("lblComponentName");
            lblvesselid = (Label)e.Row.FindControl("lblVesselid");
            Label lblcomponentid = (Label)e.Row.FindControl("lblComponentId");
            ImageButton imgComponentDetails = (ImageButton)e.Row.FindControl("imgComponentDetails");
            if (imgComponentDetails != null) imgComponentDetails.Attributes.Add("onclick", "javascript:Openpopup('Component', '', 'PurchaseFormItemComponentDetails.aspx?COMPONENTID=" + lblcomponentid.Text + "&VESSELID=" + lblvesselid.Text + "'); return false;");


            Label lblAttachmentFlag = (Label)e.Row.FindControl("lblAttachmentFlag");
            Label lblNotes = (Label)e.Row.FindControl("lblNotes");

            if (lblNotes.Text == "")
                if (imgDetails != null)
                {
                    // imgDetails.Visible = false;
                    imgDetails.ImageUrl = !lblNotes.Text.ToUpper().Equals("") ? Session["images"] + "/text-detail.png" : Session["images"] + "/spacer.gif";
                    imgDetails.Width = 14;
                    imgDetails.Height = 14;
                    imgDetails.Enabled = false;

                }

            if (lblAttachmentFlag.Text == "0")
                if (imgAttach != null) imgAttach.Visible = false;

            if (Filter.CurrentPurchaseStockType.Equals("SPARE"))
            {
                //LinkButton lb = (LinkButton)e.Row.FindControl("lnkStockItemCode");
                //UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucToolTipComponent");
                //lb.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                //lb.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }

            if (Filter.CurrentPurchaseStockType.Equals("STORE"))
            {
                lblComponentName.Visible = false;
                GridView _gv = (GridView)sender;
                _gv.Columns[4].Visible = false;
                if (imgComponentDetails != null) imgComponentDetails.Visible = false;
            }
            lblcomponentid = (Label)e.Row.FindControl("lblPartId");
            img = (ImageButton)e.Row.FindControl("cmdDetail");
            if (img != null)
            {
                if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SERVICE"))
                    img.Attributes.Add("onclick", "javascript:return Openpopup('Component', '', 'PurchaseServiceDetail.aspx?WORKORDERID=" + lblcomponentid.Text + "&VESSELID=" + lblvesselid.Text + "&STOCKTYPE=" + Filter.CurrentPurchaseStockType.ToString() + "'); return false;");
                else
                    img.Attributes.Add("onclick", "Openpopup('Component', '', 'PurchaseSpareItemDetail.aspx?SPAREITEMID=" + lblcomponentid.Text + "&VESSELID=" + lblvesselid.Text + "&STOCKTYPE=" + Filter.CurrentPurchaseStockType.ToString() + "'); return false;");
            }

            Label item = (Label)e.Row.FindControl("lblPartId");
            UserControlPurchaseUnit unit = (UserControlPurchaseUnit)e.Row.FindControl("ucUnit");
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (unit != null)
            {
                unit.PurchaseUnitList = PhoenixRegistersUnit.ListPurchaseUnit(item.Text, Filter.CurrentPurchaseStockType
                                                                     , int.Parse(lblvesselid.Text));
                unit.SelectedUnit = drv["FLDUNITID"].ToString();
            }

        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                if (Request.QueryString["VIEWONLY"] == "Y")
                {
                    LinkButton lnkPartNumber = (LinkButton)e.Row.FindControl("lnkStockItemCode");
                    lnkPartNumber.Enabled = false;
                    ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
                    cmdEdit.Enabled = false;
                }
                if (ViewState["WEBSESSIONSTATUS"].ToString() == "N")
                {
                    ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
                    if (cmdEdit != null) cmdEdit.Enabled = false;
                }
            }
        }

    }

    protected bool IsValidTax(string strDescription, string strValueType, string strValue)
    {
        if (ViewState["WEBSESSIONSTATUS"].ToString() == "N")
        {

            ucError.ErrorMessage = "Quotation has been finalized, you can not edit.";
            return (!ucError.IsError);
        }

        ucError.HeaderMessage = "Please provide the following required information";
        if (strDescription.Trim() == string.Empty)
            ucError.ErrorMessage = "Description is required.";
        if (strValue.Trim() == string.Empty)
            ucError.ErrorMessage = "Value is required.";
        if (strValueType.Trim() == string.Empty)
            ucError.ErrorMessage = "Type is required.";

        return (!ucError.IsError);
    }

    protected void InsertQuotationTax(Guid uQuotationId, string strDescription, int iTaxType, decimal dValue, int iOffsetVessel)
    {
        try
        {
            PhoenixPurchaseQuotation.QuotationTaxMapInsert(1, uQuotationId, strDescription, iTaxType, dValue, iOffsetVessel, null, null, null);
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void UpdateQuotationTax(Guid uTaxMapCode, Guid uQuotationId, string strDescription, int iTaxType, decimal dValue, int iOffsetVessel)
    {
        try
        {
            PhoenixPurchaseQuotation.QuotationTaxMapUpdate(
                1, uTaxMapCode, uQuotationId, strDescription, iTaxType, dValue, iOffsetVessel, null, null);
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void DeleteQuotationTax(Guid uTaxMapCode, Guid uQuotationId)
    {
        PhoenixPurchaseQuotation.QuotationTaxMapDelete(uTaxMapCode, uQuotationId);
    }

    private void UpdateQuotationLineItem(string quotaitionlineid, string quantity, string rate, string discount, string deliveryitem, string unit)
    {
        PhoenixPurchaseQuotationLine.UpdateQuotationLineForVendor(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(quotaitionlineid)
            , new Guid(ViewState["quotationid"].ToString()), General.GetNullableDecimal(quantity), General.GetNullableDecimal(rate)
            , General.GetNullableDecimal(discount), General.GetNullableDecimal(deliveryitem), int.Parse(unit));

    }

    private bool IsValidForm(string rate, string quantity, string unit)
    {
        if (ViewState["WEBSESSIONSTATUS"].ToString() == "N")
        {

            ucError.ErrorMessage = "Quotation has been finalized, you can not edit.";
            return (!ucError.IsError);
        }

        if (rate.Trim().Equals("") || rate == "0")
            ucError.ErrorMessage = "Unit Price is required.";
        if (quantity.Trim().Equals("") || quantity == "0")
            ucError.ErrorMessage = "Quantity  is required.";
        if (General.GetNullableGuid(ViewState["quotationid"].ToString()) == null)
            ucError.ErrorMessage = "Quotationid is required.";
        if (unit.ToUpper().ToString().Trim() == "DUMMY" || unit.Trim() == "")
            ucError.ErrorMessage = "Unit is Required.";
        return (!ucError.IsError);
    }

    protected void MenuQuotationLineItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (ViewState["WEBSESSIONSTATUS"].ToString() == "N")
                {
                    ucError.ErrorMessage = "Quotation has been finalized, you can not edit.";
                    ucError.Visible = true;
                    return;
                }

                UpdateQuotation();
                if (ucError.IsError)
                {
                    return;
                }
                InsertQuotationHistory();
                bindPrice();
                if (!ucError.IsError)
                    ucStatus.Text = "Quotation  updated";
            }
            if (CommandName.ToUpper().Equals("CONFIRM"))
            {

                CallSavebtn();
                if (ucError.IsError)
                {
                    return;
                }
                if (txtVenderReference.Text == "" || txtVenderReference.Text == null)
                {
                    RadWindowManager1.Localization.OK = "Yes";
                    RadWindowManager1.Localization.Cancel = "No";
                    RadWindowManager1.RadConfirm("You have not filled Quotation Ref. Please Click Yes To Continue", "confirm", 300, 100, null, "Send Quote");
                    //ucConfirm.Visible = true;
                    //ucConfirm.Text = "You have not filled Quotation Ref. Please Click Yes To Continue";
                }
                else
                {

                    PhoenixPurchaseQuotationLine.FinalizeQuotationForVendor(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["quotationid"].ToString()));
                    PhoenixCommoneProcessing.CloseUserSession(new Guid(ViewState["quotationid"].ToString()));
                    QuotationConfirmationSent();
                    CheckWebSessionStatus();
                    ucStatus.Text = "Quotation is confirmed";
                    Response.Redirect("..\\Purchase\\PurchaseQuotationConfirmation.aspx?QTNREFNO=" + txtVenderReference.Text + "&ORDERBEFOREDATE =" + txtOrderDate.Text);
                    InsertQuotationHistory();
                }
            }
            if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += " setTimeout(function(){ openNewWindow('codehelp1', '', '../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.PURCHASE + "&VESSELID=" + ViewState["VESSELID"].ToString() + "');},500)";
                Script += "</script>" + "\n";
                RadScriptManager.RegisterClientScriptBlock(Page,typeof(Page), "script", Script, false);

            }
            if (CommandName.ToUpper().Equals("REMARKS"))
            {
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += " setTimeout(function(){ openNewWindow('codehelp1', '', '../Purchase/PurchaseVendorDetail.aspx?QUOTATIONID=" + ViewState["quotationid"].ToString() +
                    "&VESSELID=" + ViewState["VESSELID"].ToString() + "&STOCKTYPE=" + Filter.CurrentPurchaseStockType.ToString() +
                    "&editable=true&launchedfrom=VENDOR')},500)";
                Script += "</script>" + "\n";
                RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "script", Script, false);
            }
            if (CommandName.ToUpper().Equals("COMMENTS"))
            {
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += " setTimeout(function(){ openNewWindow('codehelp1', '', '../Purchase/PurchaseQuotationComments.aspx?QUOTATIONID=" + ViewState["quotationid"].ToString() +
                    "&VESSELID=" + ViewState["VESSELID"].ToString() + "&STOCKTYPE=" + Filter.CurrentPurchaseStockType.ToString() + "&launchedfrom=VENDOR');},500)";
                Script += "</script>" + "\n";
                RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "script", Script, false);



            }
            if (CommandName.ToUpper().Equals("DELIVERYINSTRUCTION"))
            {
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += " setTimeout(function(){ openNewWindow('codehelp1', '', '../Purchase/PurchaseQuotationDeliveryInstruction.aspx?QUOTATIONID=" + ViewState["quotationid"].ToString() +
                    "&VESSELID=" + ViewState["VESSELID"].ToString() + "&STOCKTYPE=" + Filter.CurrentPurchaseStockType.ToString() +
                    "&editable=false&launchedfrom=VENDOR');},500)";
                Script += "</script>" + "\n";
                RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "script", Script, false);


            }
            if (CommandName.ToUpper().Equals("DECLINEQUOTE"))
            {
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += " setTimeout(function(){ openNewWindow('codehelp1', '', '../Purchase/PurchaseQuotationDeclineQuote.aspx?QUOTATIONID=" + ViewState["quotationid"].ToString() +
                    "&VESSELID=" + ViewState["VESSELID"].ToString() + "&STOCKTYPE=" + Filter.CurrentPurchaseStockType.ToString() +
                    "&editable=true&launchedfrom=VENDOR');},500)";
                Script += "</script>" + "\n";
                RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "script", Script, false);

            }
            if (CommandName.ToUpper().Equals("DETAILS"))
            {
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += " setTimeout(function(){ openNewWindow('codehelp1', '', '../Purchase/PurchaseFormDetail.aspx?orderid=" + ViewState["ORDERID"].ToString() +
                    "&launchedfrom=VENDOR');},500)";
                Script += "</script>" + "\n";
                RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "script", Script, false);

            }
            if (CommandName.ToUpper().Equals("ACCEPT") && General.GetNullableInteger(ViewState["ACCEPTREJECTYN"].ToString()) == null)
            {
                PhoenixPurchaseOrderForm.UpdateAcceptanceflag(new Guid(ViewState["ORDERID"].ToString()), 1, 1);
                ucStatus.Text = "Order Accepted Successfully.";
            }
            if (CommandName.ToUpper().Equals("REJECT") && General.GetNullableInteger(ViewState["ACCEPTREJECTYN"].ToString()) == null)
            {
                PhoenixPurchaseOrderForm.UpdateAcceptanceflag(new Guid(ViewState["ORDERID"].ToString()), 2, 1);
                ucStatus.Text = "Order Rejected.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void QuotationConfirmationSent()
    {
        string emailbodytext = "";
        DataSet ds = new DataSet();
        ds = PhoenixPurchaseQuotationLine.QuotationVendorEmailDetails(new Guid(ViewState["quotationid"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            emailbodytext = PrepareApprovalText(ds.Tables[0]);
            DataRow dr = ds.Tables[0].Rows[0];
            PhoenixMail.SendMail(dr["FLDEMAIL"].ToString().Replace(";", ",").TrimEnd(','),
                null,
                null,
                dr["FLDSUBJECT"].ToString(),
                emailbodytext,
                true,
                System.Net.Mail.MailPriority.Normal,
                "", null,
                null);
        }
        
    }

    protected string PrepareApprovalText(DataTable dt)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataRow dr = dt.Rows[0];
        sbemailbody.Append("<pre>");
        sbemailbody.AppendLine();
        sbemailbody.Append("Dear Purchaser");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Quotation is Received from  " + dr["FLDNAME"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Thank you,");

        return sbemailbody.ToString();

    }

    private void UpdateQuotation()
    {
        if (General.GetNullableInteger(ucCurrency.SelectedCurrency) == null)
        {
            ucError.ErrorMessage = "Quoted Currency is Required.";
            ucError.Visible = true;
            return;
        }
        if (ViewState["quotationid"].ToString() == "")
        {
            ucError.ErrorMessage = " ";
            ucError.Visible = true;
            return;
        }

        PhoenixPurchaseQuotation.UpdateVendorQuotation(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["quotationid"].ToString())
            , General.GetNullableDateTime(DateTime.Now.ToString()), General.GetNullableDateTime(txtExpirationDate.Text)
            , General.GetNullableDecimal(txtPrice.Text), General.GetNullableInteger(ucCurrency.SelectedCurrency)
            , General.GetNullableDecimal(txtDiscount.Text), General.GetNullableDecimal(txtDeliveryTime.Text)
            , txtVenderReference.Text, General.GetNullableDateTime(txtOrderDate.Text),
            //txtVendorRemarks.Text,
            General.GetNullableInteger(UCDeliveryTerms.SelectedQuick)
            , General.GetNullableInteger(UCPaymentTerms.SelectedQuick)
            , General.GetNullableInteger(ucModeOfTransport.SelectedQuick)
            , General.GetNullableDecimal(txtSupplierDiscount.Text)
            , General.GetNullableDecimal(txtTotalDiscount.Text)
            , General.GetNullableInteger(ddlType.SelectedHard));
        //ucStatus.Text = "Quotation updated successfully";
        InsertQuotationHistory();
    }

    protected void onPurchaseQuotationLine(object sender, CommandEventArgs e)
    {
        ViewState["quotationlineid"] = e.CommandArgument.ToString();
        bindQuotationLine();
    }

    private void bindQuotationLine()
    {
        DataSet quotationlinedataset = new DataSet();
        quotationlinedataset = PhoenixPurchaseQuotationLine.EditQuotationLine(new Guid(ViewState["quotationlineid"].ToString()));
        if (quotationlinedataset.Tables[0].Rows.Count > 0)
        {
            DataRow dr = quotationlinedataset.Tables[0].Rows[0];
        }

    }

    protected void txtDiscount_TextChanged(object sender, EventArgs e)
    {
        if (txtDiscount.Text.Trim() != "" && txtPrice.Text.Trim() != "")
        {
            //txtTotalDiscount.Text = Math.Round(((decimal.Parse(txtPrice.Text) * decimal.Parse(txtDiscount.Text)) / 100), 2).ToString();
            //txtTotalPrice.Text = Math.Round((decimal.Parse(txtPrice.Text) - decimal.Parse(txtTotalDiscount.Text) + decimal.Parse(txtTotalCharges.Text)), 2).ToString();
        }
    }

    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        //int nextRow = 0;
        //GridView _gridView = (GridView)sender;

        //if (e.Row.RowType == DataControlRowType.DataRow
        //    && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
        //    || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        //{
        //    int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

        //    String script = "var keyValue = SelectSibling(event); ";
        //    script += " if(keyValue == 38) {";  //Up Arrow
        //    nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

        //    script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
        //    script += "}";

        //    script += " if(keyValue == 40) {";  //Down Arrow
        //    nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

        //    script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
        //    script += "}";
        //    script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
        //    e.Row.Attributes["onkeydown"] = script;
        //}

    }
    public void CopyForm_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
            if (ucCM.confirmboxvalue == 1)
            {
                PhoenixPurchaseQuotationLine.FinalizeQuotationForVendor(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["quotationid"].ToString()));
                PhoenixCommoneProcessing.CloseUserSession(new Guid(ViewState["quotationid"].ToString()));
                QuotationConfirmationSent();
                CheckWebSessionStatus();
                ucStatus.Text = "Quotation is confirmed";
                Response.Redirect("..\\Purchase\\PurchaseQuotationConfirmation.aspx?QTNREFNO=" + txtVenderReference.Text + "&ORDERBEFOREDATE =" + txtOrderDate.Text);
                InsertQuotationHistory();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void InsertQuotationHistory()
    {
        PhoenixPurchaseQuotationLine.InsertQuotationHistory(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["quotationid"].ToString()), int.Parse(hndVesselID.Value));
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvVendorItem.Rebind();
        gvTax.Rebind();
        bindPrice();
    }
    private void CallSavebtn()
    {
        if (ViewState["WEBSESSIONSTATUS"].ToString() == "N")
        {
            ucError.ErrorMessage = "Quotation has been finalized, you can not edit.";
            ucError.Visible = true;
            return;
        }

        UpdateQuotation();
        InsertQuotationHistory();
        bindPrice();
//        gvVendorItem.Rebind();
    }

    protected void gvTax_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                DeleteQuotationTax(new Guid(item.GetDataKeyValue("FLDQUOTATIONTAXMAPCODE").ToString())
                                     , new Guid(ViewState["quotationid"].ToString()));

                bindPrice();
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        
        
    }

    protected void gvTax_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
       DataSet ds = PhoenixPurchaseQuotation.QuotationTaxMapList(null, new Guid(ViewState["quotationid"].ToString()));
            gvTax.DataSource = ds;

    }

    protected void gvTax_EditCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvTax_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem item = (GridFooterItem)e.Item;
                if (!IsValidTax(
                    ((RadTextBox)item.FindControl("txtDescriptionAdd")).Text.ToString().Trim(),
                    ((UserControlTaxType)item.FindControl("ucTaxTypeAdd")).TaxType.ToString(),
                    ((UserControlDecimal)item.FindControl("txtValueAdd")).Text.ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertQuotationTax(
                                     new Guid(ViewState["quotationid"].ToString())
                                    , ((RadTextBox)item.FindControl("txtDescriptionAdd")).Text.ToString().Trim()
                                    , int.Parse(((UserControlTaxType)item.FindControl("ucTaxTypeAdd")).TaxType)
                                    , decimal.Parse(((UserControlDecimal)item.FindControl("txtValueAdd")).Text.ToString())
                                    , 0
                                );
                bindPrice();
                gvTax.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTax_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            GridDataItem item = (GridDataItem)e.Item;

            if (!IsValidTax(
              ((RadTextBox)item.FindControl("txtDescriptionEdit")).Text.ToString().Trim(),
              ((UserControlTaxType)item.FindControl("ucTaxTypeEdit")).TaxType.ToString(),
              ((UserControlDecimal)item.FindControl("txtValueEdit")).Text.ToString()))
            {
                ucError.Visible = true;
                return;
            }

            UpdateQuotationTax(
                                 new Guid(item.GetDataKeyValue("FLDQUOTATIONTAXMAPCODE").ToString())
                                , new Guid(ViewState["quotationid"].ToString())
                                , ((RadTextBox)item.FindControl("txtDescriptionEdit")).Text.ToString().Trim()
                                , int.Parse(((UserControlTaxType)item.FindControl("ucTaxTypeEdit")).TaxType)
                                , decimal.Parse(((UserControlDecimal)item.FindControl("txtValueEdit")).Text.ToString())
                                , 0
                            );
            gvTax.Rebind();
            bindPrice();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTax_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            GridEditableItem item = (GridEditableItem)e.Item;
            LinkButton db = (LinkButton)item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
        }

        if (e.Item is GridEditableItem)
        {
            GridEditableItem item = (GridEditableItem)e.Item;
            LinkButton cmdEdit = (LinkButton)item.FindControl("cmdEdit");
            if (cmdEdit != null)
                cmdEdit.Visible = Request.QueryString["VIEWONLY"] == "Y" ? false : SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);
            LinkButton cmdSave = (LinkButton)item.FindControl("cmdSave");
            if (cmdSave != null)
                cmdSave.Visible = Request.QueryString["VIEWONLY"] == "Y" ? false : SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);
            LinkButton cmdCancel = (LinkButton)item.FindControl("cmdCancel");
            if (cmdCancel != null)
                cmdCancel.Visible = Request.QueryString["VIEWONLY"] == "Y" ? false : SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName);
            LinkButton cmdDelete = (LinkButton)item.FindControl("cmdDelete");
            if (cmdDelete != null)
                cmdDelete.Visible = Request.QueryString["VIEWONLY"] == "Y" ? false : SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);

        }

        if (e.Item is GridFooterItem)
        {
            GridFooterItem item = (GridFooterItem)e.Item;
            LinkButton cmdAdd = (LinkButton)item.FindControl("cmdAdd");
            if (cmdAdd != null)
                cmdAdd.Visible = Request.QueryString["VIEWONLY"] == "Y" ? false : SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
        }
    }

    protected void gvVendorItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns;
        string[] alCaptions;

        if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SPARE") || Filter.CurrentPurchaseStockType.ToUpper().Equals("SERVICE"))
        {
            alColumns = new string[12] {"FLDNUMBER","FLDMAKERREFERENCE", "FLDPOSITIONDRAWING", "FLDNAME", "FLDVENDORNOTES", "FLDORDEREDQUANTITY",
                                 "FLDQUANTITY", "FLDUNITNAME", "FLDQUOTEDPRICE", "FLDDISCOUNT", "TOTALPRICE", "FLDDELIVERYTIME" };
            alCaptions = new string[12] {"Number","Maker Reference", "Drawing No/Position", "Part Name", "Remark", "Order Qty",
                                 "Quoted Qty", "Unit", "Unit Price", "%Discount", "Total Price", "Del. Time(Days)" };
        }
        else
        {
            alColumns = new string[12] {"FLDNUMBER","FLDMAKERREFERENCE", "FLDPOSITIONDRAWING", "FLDNAME", "FLDVENDORNOTES", "FLDORDEREDQUANTITY",
                                 "FLDQUANTITY", "FLDUNITNAME", "FLDQUOTEDPRICE", "FLDDISCOUNT", "TOTALPRICE", "FLDDELIVERYTIME" };
            alCaptions = new string[12] {"Number","Product Code", "Drawing No/Position", "Part Name", "Remark", "Order Qty",
                                 "Quoted Qty", "Unit", "Unit Price", "%Discount", "Total Price", "Del. Time(Days)" };
        }

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPurchaseQuotationLine.QuotationLineSearchForVendor("", ViewState["quotationid"].ToString(), sortexpression, sortdirection, gvVendorItem.CurrentPageIndex+1,
            gvVendorItem.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        gvVendorItem.DataSource = ds;
        gvVendorItem.VirtualItemCount = iRowCount;
        
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvVendorItem", "Vendor Item List", alCaptions, alColumns, ds);
    }

    protected void gvVendorItem_EditCommand(object sender, GridCommandEventArgs e)
    {

    }
    protected void gvVendorItem_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem item = (GridEditableItem)e.Item;

            if (!IsValidForm(((UserControlDecimal)item.FindControl("txtQuotedPriceEdit")).Text
                , ((UserControlDecimal)item.FindControl("txtQuantityEdit")).Text
                , ((UserControlPurchaseUnit)item.FindControl("ucUnit")).SelectedUnit))
            {
                ucError.Visible = true;
                return;
            }

            UpdateQuotationLineItem(item.GetDataKeyValue("FLDQUOTATIONLINEID").ToString(),
                    ((UserControlDecimal)item.FindControl("txtQuantityEdit")).Text,
                    ((UserControlDecimal)item.FindControl("txtQuotedPriceEdit")).Text
                    , ((UserControlDecimal)item.FindControl("txtDiscountEdit")).Text
                    , ((UserControlDecimal)item.FindControl("txtDeliveryTimeEdit")).Text
                    , ((UserControlPurchaseUnit)item.FindControl("ucUnit")).SelectedUnit);
            
            
            bindPrice();
            gvTax.Rebind();
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = false;
        }
        
    }

    protected void gvVendorItem_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvVendorItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            GridHeaderItem item = (GridHeaderItem)e.Item;
            if (Filter.CurrentPurchaseStockType == "STORE")
                item["MAKERREF"].Text = "Product Code";
        }

        if (e.Item is GridDataItem)
        {
            GridEditableItem item = (GridEditableItem)e.Item;
            LinkButton db = (LinkButton)item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            
            LinkButton img = (LinkButton)item.FindControl("imgVendorNotes");
            //if (Request.QueryString["VIEWONLY"] == "Y")
            //    img.Attributes.Add("onclick", "javascript:showMoreInformation(ev, '../Purchase/PurchaseFormItemMoreInfo.aspx?quotationlineid=" + item.GetDataKeyValue("FLDQUOTATIONLINEID").ToString() + "&viewsession=N'); return false;");
            //else
            //    img.Attributes.Add("onclick", "javascript:showMoreInformation(ev, '../Purchase/PurchaseFormItemMoreInfo.aspx?quotationlineid=" + item.GetDataKeyValue("FLDQUOTATIONLINEID").ToString() + "&viewsession=" + ViewState["WEBSESSIONSTATUS"].ToString() + "'); return false;");

            if (Request.QueryString["VIEWONLY"] == "Y")
                img.Attributes.Add("onclick", "javascript:openNewWindow('ev','', '../Purchase/PurchaseFormItemMoreInfo.aspx?quotationlineid=" + item.GetDataKeyValue("FLDQUOTATIONLINEID").ToString() + "&viewsession=N'); return false;");
            else
                img.Attributes.Add("onclick", "javascript:openNewWindow('ev','', '../Purchase/PurchaseFormItemMoreInfo.aspx?quotationlineid=" + item.GetDataKeyValue("FLDQUOTATIONLINEID").ToString() + "&viewsession=" + ViewState["WEBSESSIONSTATUS"].ToString() + "'); return false;");



            LinkButton imgAttach = (LinkButton)item.FindControl("cmdViewAttachment");
            if (imgAttach != null) imgAttach.Attributes.Add("onclick", "openNewWindow('AddAddress', '', '../Common/CommonFileAttachment.aspx?DTKEY=" + item.GetDataKeyValue("FLDDTKEY").ToString() + "&MOD=" + PhoenixModule.PURCHASE + "&U=N'); return false;");

            
            LinkButton imgDetails = (LinkButton)item.FindControl("cmdDetails");
            if (imgDetails != null) imgDetails.Attributes.Add("onclick", "openNewWindow('AddAddress', '', '../Purchase/PurchaseFormItemComment.aspx?orderlineid=" + item.GetDataKeyValue("FLDORDERLINEID").ToString() + "&viewonly=Y'); return false;");


            //Label lblComponentName = (Label)e.Row.FindControl("lblComponentName");
            //lblvesselid = (Label)e.Row.FindControl("lblVesselid");
           // Label lblcomponentid = (Label)e.Row.FindControl("lblComponentId");
            LinkButton imgComponentDetails = (LinkButton)item.FindControl("imgComponentDetails");
            if (imgComponentDetails != null) imgComponentDetails.Attributes.Add("onclick", "javascript:openNewWindow('Component', '', '../Purchase/PurchaseFormItemComponentDetails.aspx?COMPONENTID=" + item.GetDataKeyValue("FLDCOMPONENTID").ToString() + "&VESSELID=" + item.GetDataKeyValue("FLDVESSELID").ToString() + "'); return false;");


            //Label lblAttachmentFlag = (Label)e.Row.FindControl("lblAttachmentFlag");
            //Label lblNotes = (Label)e.Row.FindControl("lblNotes");

            if (item.GetDataKeyValue("FLDNOTES").ToString() == "")
                if (imgDetails != null)
                {
                    imgDetails.Visible = false;
                }

            if (item.GetDataKeyValue("FLDATTACHMENTFLAG").ToString() == "0")
                if (imgAttach != null) imgAttach.Visible = false;

            
            
            img = (LinkButton)item.FindControl("cmdDetail");
            if (img != null)
            {
                if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SERVICE"))
                    img.Attributes.Add("onclick", "javascript:return openNewWindow('Component', '', '../Purchase/PurchaseServiceDetail.aspx?WORKORDERID=" + item.GetDataKeyValue("FLDPARTID").ToString()+ "&VESSELID=" + item.GetDataKeyValue("FLDVESSELID").ToString() + "&STOCKTYPE=" + Filter.CurrentPurchaseStockType.ToString() + "'); return false;");
                else
                    img.Attributes.Add("onclick", "openNewWindow('Component', '', '../Purchase/PurchaseSpareItemDetail.aspx?SPAREITEMID=" + item.GetDataKeyValue("FLDPARTID").ToString() + "&VESSELID=" + item.GetDataKeyValue("FLDVESSELID").ToString() + "&STOCKTYPE=" + Filter.CurrentPurchaseStockType.ToString() + "'); return false;");
            }

            //Label item = (Label)e.Row.FindControl("lblPartId");
            UserControlPurchaseUnit unit = (UserControlPurchaseUnit)item.FindControl("ucUnit");
            DataRowView drv = (DataRowView)item.DataItem;
            if (unit != null)
            {
                unit.PurchaseUnitList = PhoenixRegistersUnit.ListPurchaseUnit(item.GetDataKeyValue("FLDPARTID").ToString(), Filter.CurrentPurchaseStockType
                                                                     , int.Parse(item.GetDataKeyValue("FLDVESSELID").ToString()));
                unit.SelectedUnit = drv["FLDUNITID"].ToString();
            }

        }

        if (e.Item is GridEditableItem && e.Item.IsInEditMode )
        {
            GridEditableItem item = (GridEditableItem)e.Item;
            if (Request.QueryString["VIEWONLY"] == "Y")
            {
                
                LinkButton lnkPartNumber = (LinkButton)item.FindControl("lnkStockItemCode");
                lnkPartNumber.Enabled = false;
                LinkButton cmdEdit = (LinkButton)item.FindControl("cmdEdit");
                cmdEdit.Enabled = false;
            }
            if (ViewState["WEBSESSIONSTATUS"].ToString() == "N")
            {
                LinkButton cmdEdit = (LinkButton)item.FindControl("cmdEdit");
                if (cmdEdit != null) cmdEdit.Enabled = false;
            }
            
        }
    }

    protected void btnconfirm_Click(object sender, EventArgs e)
    {
        PhoenixPurchaseQuotationLine.FinalizeQuotationForVendor(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["quotationid"].ToString()));
        PhoenixCommoneProcessing.CloseUserSession(new Guid(ViewState["quotationid"].ToString()));
        QuotationConfirmationSent();
        CheckWebSessionStatus();
        ucStatus.Text = "Quotation is confirmed";
        Response.Redirect("..\\Purchase\\PurchaseQuotationConfirmation.aspx?QTNREFNO=" + txtVenderReference.Text + "&ORDERBEFOREDATE =" + txtOrderDate.Text);
        InsertQuotationHistory();
    }
}
