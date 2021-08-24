using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;
using System.Configuration;
using System.Collections.Specialized;

public partial class PurchaseQuotation : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        try
        {

            foreach (GridViewRow r in gvVendor.Rows)
            {
                if (r.RowType == DataControlRowType.DataRow)
                {
                    Page.ClientScript.RegisterForEventValidation(gvVendor.UniqueID, "Edit$" + r.RowIndex.ToString());
                }
            }
            base.Render(writer);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {


        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            txtVendorID.Attributes.Add("style", "display:none");
            if (!IsPostBack)
            {
                if (Request.QueryString["VesselId"] != null)
                {
                    ViewState["VesselId"] = Request.QueryString["VesselId"].ToString();
                    Filter.CurrentPurchaseVesselSelection = Convert.ToInt32(ViewState["VesselId"]);
                }
                else
                {
                    ViewState["VesselId"] = "";
                }
                if (Request.QueryString["StockType"] != null)
                {
                    ViewState["StockType"] = Request.QueryString["StockType"].ToString();
                    Filter.CurrentPurchaseStockType = ViewState["StockType"].ToString();
                }
                else
                {
                    ViewState["StockType"] = "";
                }
                if (Request.QueryString["StockClass"] != null)
                {
                    ViewState["StockClass"] = Request.QueryString["StockClass"].ToString();
                    Filter.CurrentPurchaseStockClass = ViewState["StockClass"].ToString();
                }
                else
                {
                    ViewState["StockClass"] = "";
                }
                ViewState["FormStatus"] = "";
                short showcreditnote = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
                lblCreditNoteDisc.Visible = (showcreditnote == 1) ? true : false;
                txtDiscount.Visible = (showcreditnote == 1) ? true : false;
                cmdDiscount.Visible = (showcreditnote == 1) ? true : false;
                lblTotalDisc.Visible = (showcreditnote == 1) ? true : false;
                ucTotalDiscount.Visible = (showcreditnote == 1) ? true : false;
                lblUSDTotalDisc.Visible = (showcreditnote == 1) ? true : false;
                txtTotalDiscount.Visible = (showcreditnote == 1) ? true : false;
                txtSentById.Attributes.Add("style", "display:none");
                txtRejectedById.Attributes.Add("style", "display:none");

                UCDeliveryTerms.QuickTypeCode = ((int)PhoenixQuickTypeCode.DELIVERYTERM).ToString();
                UCPaymentTerms.QuickTypeCode = ((int)PhoenixQuickTypeCode.PAYMENTTERM).ToString();
                ViewState["Lock"] = "N";
                ucCurrency.SelectedCurrency = PhoenixPurchaseOrderForm.DefaultCurrency;
                
                
                

                if (Request.QueryString["FormNo"] != null)
                {
                    ViewState["FormNo"] = Request.QueryString["FormNo"].ToString();
                    PhoenixPurchaseOrderForm.FormNumber = ViewState["FormNo"].ToString();
                }
                else
                {
                    ViewState["FormNo"] = "";
                }

                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("ucVessel", ViewState["VesselId"].ToString());
                criteria.Add("ddlStockType", ViewState["StockType"].ToString());
                criteria.Add("txtNumber", ViewState["FormNo"].ToString());
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
                criteria.Add("ddlDepartment", "");
                criteria.Add("ddlReqStatus", "");
                criteria.Add("ucReason4Requisition", "");


                Filter.CurrentOrderFormFilterCriteria = criteria;
                Filter.CurrentPurchaseDashboardCode = null;

                Title1.Text = "Quotation    (  " + PhoenixPurchaseOrderForm.FormNumber + "     )";
                if (Request.QueryString["orderid"] != null)
                {
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                    Response.Redirect("../Purchase/PurchaseQuotationVendorDetail.aspx?orderid=" + ViewState["orderid"].ToString() + "&pageno=0");
                    if (Request.QueryString["quotationid"] != null)
                    {
                        ViewState["quotationid"] = Request.QueryString["quotationid"].ToString();
                        
                        BindFields(ViewState["quotationid"].ToString());
                        ViewState["SaveStatus"] = "Edit";
                    }
                    else
                    {
                        ViewState["quotationid"] = "";
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
                
            }

            PhoenixToolbar toolbarsave = new PhoenixToolbar();
            toolbarsave.AddButton("Save", "SAVE");
            toolbarsave.AddButton("Close", "CLOSE");
            if (Request.QueryString["quotationid"] != null && Request.QueryString["quotationid"].ToString() != "")
            {
                toolbarsave.AddButton("Delivery", "DELIVERYINSTRUCTION");
                toolbarsave.AddButton("Remarks", "REMARKS");
                toolbarsave.AddButton("Comments", "COMMENTS");
                toolbarsave.AddButton("Attachment", "ATTACHMENT");
                toolbarsave.AddButton("Contacts", "CONTACTS");
            }
            MenuVendor.AccessRights = this.ViewState;
            MenuVendor.MenuList = toolbarsave.Show();

            short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Purchase/PurchaseQuotation.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvVendor')", "Print Grid", "icon_print.png", "Print");
            toolbargrid.AddImageButton("../Purchase/PurchaseQuotation.aspx", "Send Query", "Email.png", "RFQ");
            toolbargrid.AddImageButton("../Purchase/PurchaseQuotation.aspx", "Send Reminder", "remainder-mail.png", "RFQREMAINDER");
            toolbargrid.AddImageButton("../Purchase/PurchaseQuotation.aspx", "Compare Quotations", "query.png", "QTNCOMPARE");
            toolbargrid.AddImageButton("../Purchase/PurchaseQuotation.aspx", "Send  Approval", "send-approval.png", "SENDAPPROVAL");
            if (showcreditnotedisc == 1)
                toolbargrid.AddImageButton("../Purchase/PurchaseQuotation.aspx", "Quotation Compare Report", "incident-report.png", "QUOTATIONCOMPARE"); ;
            toolbargrid.AddImageButton("../Purchase/PurchaseQuotation.aspx", "Create PO", "task-list.png", "ORDER");
            toolbargrid.AddImageButton("../Purchase/PurchaseQuotation.aspx", "Add Vendor", "add.png", "MULTIPLEVENDOR");
            toolbargrid.AddImageButton("../Purchase/PurchaseQuotation.aspx", "Refresh", "refresh.png", "REFRESH");
            
            MenuVendorList.AccessRights = this.ViewState;
            MenuVendorList.MenuList = toolbargrid.Show();
            BindData();
            BindDataTax();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void LoadBudgetCode()
    {
        DropDownList ddl1 = (DropDownList)gvTax.FooterRow.FindControl("ddlBudgetCode");
        DataSet ds = new DataSet();

        ds = PhoenixPurchaseBudgetCode.ListBudgetCode();
        ddl1.DataSource = ds.Tables[0];
        ddl1.DataTextField = "FLDSUBACCOUNT";
        ddl1.DataValueField = "FLDBUDGETID";
        ddl1.DataBind();
    }

    private void BindFields(string quotationid)
    {
        DataSet quotationdataset = new DataSet();
        quotationdataset = PhoenixPurchaseQuotation.EditQuotation(new Guid(quotationid));
        if (quotationdataset.Tables[0].Rows.Count > 0)
        {
            DataRow dr = quotationdataset.Tables[0].Rows[0];
            txtVendorCode.Text = dr["FLDVENDORCODE"].ToString();
            txtVendorID.Text = dr["FLDVENDORID"].ToString();
            txtVendorName.Text = dr["FLDNAME"].ToString();
            PhoenixPurchaseQuotation.VendorName = dr["FLDNAME"].ToString();
            txtQtnRefenceno.Text = dr["FLDVENDORQUOTATIONREF"].ToString();
            txtRejectedDate.Text = General.GetDateTimeToString(dr["FLDREJECTEDDATE"].ToString());
            txtRecivedDate.Text = General.GetDateTimeToString(dr["FLDRECEIVEDDATE"].ToString());
            txtOrderDate.Text = General.GetDateTimeToString(dr["FLDORDERBEFOREDELIVERYDATE"].ToString());
            txtDeliveryTime.Text = dr["FLDDELIVERYTIME"].ToString();
            txtSentDate.Text = General.GetDateTimeToString(dr["FLDSENTDATE"].ToString());
            ucApprovalBy.Text = dr["FLDAPPROVALBYNAME"].ToString();
            ucApprovalBy.SelectedValue = dr["FLDAPPROVALBY"].ToString();
            ViewState["PURCHASEAPPROVEDBY"] = dr["FLDPURCHASEAPPROVEDBY"];
            txtSentById.Text = dr["FLDSENTBY"].ToString();
            txtNameSentBy.Text = dr["FLDSENTBYNAME"].ToString();
            txtRejectedById.Text = dr["FLDREJECTEDBY"].ToString();
            txtNameRejectedBy.Text = dr["FLDREJECTEDBYNAME"].ToString();
            txtSupplierDiscount.Text = dr["FLDSUPPLIERDISCOUNT"].ToString();
            //txtusername.Text = dr["FLDAPPROVALBYNAME"].ToString();
            //txtusercode.Text = dr["FLDAPPROVALBY"].ToString();

            if (dr["FLDQUOTEDCURRENCYID"] != null && dr["FLDQUOTEDCURRENCYID"].ToString().Trim() != "")
                ucCurrency.SelectedCurrency = dr["FLDQUOTEDCURRENCYID"].ToString();
            else
                ucCurrency.SelectedCurrency = PhoenixPurchaseOrderForm.DefaultCurrency;
            ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();

            ViewState["FormStatus"] = dr["FLDFORMSTATUS"].ToString();
            txtDiscount.Text = String.Format("{0:##.0000000}", dr["FLDDISCOUNT"]);
            txtRecivedDate.Enabled = true;
            txtOrderDate.Enabled = true;
            txtExpirationDate.Text = General.GetDateTimeToString(dr["FLDEXPIRYDATE"].ToString());
            txtExpirationDate.Enabled = true;

            if (dr["FLDSENTDATE"].ToString().Equals(""))
            {
                txtRecivedDate.Enabled = false;
                txtOrderDate.Enabled = true;
            }

            if (!dr["FLDRECEIVEDDATE"].ToString().Equals(""))
            {
                txtRecivedDate.Enabled = false;
                txtOrderDate.Enabled = false;


                txtRate.Text = String.Format("{0:###,###,###,##0.00}", dr["FLDQUOTEDPRICE"]);
                lblExchangeRate.Text = String.Format("{0:###,###,###,###,###.000000000}", dr["EXCHANGERATE"]);
                ucTotalDiscount.Text = String.Format("{0:###,###,###,###,###.00}", dr["FLDTOTALDISCOUNT"]);
                ucTotalAmount.Text = String.Format("{0:###,###,###,###,###.00}", dr["FLDTOTALAMOUNT"]);

                txtUsdPrice.Text = String.Format("{0:###,###,###,##0.00}", (decimal.Parse(dr["FLDUSDPRICE"].ToString())));
                txtTotalDiscount.Text = String.Format("{0:###,###,###,###.00}", (decimal.Parse(dr["FLDUSDDISCOUNT"].ToString())));
                txtTotalInUSD.Text = String.Format("{0:###,###,###,###.00}", (decimal.Parse(dr["TOTALUSDAMOUNT"].ToString())));


            }
            if (!dr["FLDORDEREDDATE"].ToString().Equals(""))
            {
                ViewState["Lock"] = "Y";
            }
            if (!dr["FLDEMAIL1"].ToString().Equals(""))
                lblEmailIds.Text = dr["FLDEMAIL1"].ToString() + ",";
            if (!dr["FLDEMAIL2"].ToString().Equals(""))
                lblEmailIds.Text = lblEmailIds.Text + dr["FLDEMAIL2"].ToString() + ",";
            if (!dr["FLDSUPPLIEREMAIL"].ToString().Equals(""))
                lblEmailIds.Text = lblEmailIds.Text + dr["FLDSUPPLIEREMAIL"].ToString();

            txtDiscount.Enabled = true;
            txtDeliveryTime.Enabled = true;
            

            UCDeliveryTerms.SelectedQuick = dr["FLDVENDORDELIVERYTERMID"].ToString();
            UCPaymentTerms.SelectedQuick = dr["FLDVENDORPAYMENTTERMID"].ToString();
            ucModeOfTransport.SelectedQuick = dr["FLDMODEOFTRANSPORT"].ToString();

            if (dr["FLDISSELECTEDFORORDER"].ToString().ToUpper().Equals("TRUE"))
            {
                ucCurrency.Enabled = false;
                txtDeliveryTime.Attributes.Add("CssClass", "readonlytextbox");
                txtDeliveryTime.Enabled = false;
                txtDiscount.Enabled = false;
            }

            if (dr["FLDPURCHASEAPPROVEDBY"].ToString().Equals("1"))
            {
                ddlAccountDetails.Enabled = false;
            }
            else
            {
                if (!IsPostBack)
                {
                    PhoenixPurchaseApprovedVesselAccount.DefaultVesselAccountInsert(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["orderid"].ToString()),
                        new Guid(ViewState["quotationid"].ToString()),
                        int.Parse(dr["FLDVENDORID"].ToString()),
                        Filter.CurrentPurchaseVesselSelection);
                }
            }

            DataTable dt = PhoenixPurchaseApprovedVesselAccount.VesselAccountSearch(
                new Guid(ViewState["quotationid"].ToString()),
                new Guid(ViewState["orderid"].ToString()));

            if (dt.Rows.Count > 0)
                ddlAccountDetails.SelectedValue = dt.Rows[0]["FLDVESSELACCOUNTID"].ToString();
            else
                ddlAccountDetails.SelectedValue = "";

            ddlType.SelectedHard = dr["FLDITEMTYPE"].ToString();


            if (dr["FLDISSELECTEDFORORDER"].ToString().ToUpper().Equals("TRUE"))
            {
                txtPartPaid.Text = String.Format("{0:##,###,###.00}", dr["FLDPARTPAYMENT"]);
            }
            hdnprincipalId.Value = dr["FLDPRINCIPALID"].ToString();
        }
    }
    private void BindDataTax()
    {
        if (gvTax.FooterRow != null)
        {
            UserControlOwnerBudgetCode ddl = (UserControlOwnerBudgetCode)gvTax.FooterRow.FindControl("ucOwnerBudgetCode");
            ViewState["OwnerBudgetId"] = ddl.SelectedBudgetCode;
        }


        int iRowCount = 0;
        int iTotalPageCount = 0;
       
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["quotationid"].ToString().Equals(""))
        {
            ds = PhoenixPurchaseQuotation.QuotationTaxMapListFinalize(null, null);
        }
        else
        {
            ds = PhoenixPurchaseQuotation.QuotationTaxMapListFinalize(null, new Guid(ViewState["quotationid"].ToString()));
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvTax.DataSource = ds;
            gvTax.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvTax);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        //LoadBudgetCode();
    }

    private void BindData()
    {
        if (string.IsNullOrEmpty(ucApprovalBy.Text))
            cmdMap.Enabled = false;
        else
            cmdMap.Enabled = true;

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = {"FLDVENDORID", "FLDNAME","FLDRECEIVEDDATE","FLDTOTALPRICE","FLDTOTALDISCOUNT","TOTALAMOUNT",
                                 "FLDDELIVERYTIME","STATUS" };
        string[] alCaptions = {"Vendor ID", "Vendor","Received Date","Quoted Price","Discount","Total Amount",
                                 "Delivery Time","Quoted" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = PhoenixPurchaseQuotation.QuotationSearch(General.GetNullableGuid(ViewState["orderid"].ToString()), sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvVendor.DataSource = ds;
            gvVendor.DataBind();
            if (ViewState["quotationid"] == null || ViewState["quotationid"].ToString() == "")
            {
                ViewState["quotationid"] = ds.Tables[0].Rows[0]["FLDQUOTATIONID"].ToString();
                ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                BindFields(ViewState["quotationid"].ToString());
                gvVendor.SelectedIndex = 0;
                SetRowSelection();
            }
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvVendor);
        }

        short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
        gvVendor.Columns[7].Visible = (showcreditnotedisc == 1) ? true : false;
        gvVendor.Columns[8].Visible = (showcreditnotedisc == 1) ? true : false;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvVendor", "Purchase Vendor List - " + PhoenixPurchaseOrderForm.FormNumber, alCaptions, alColumns, ds);
        SetPageNavigator();
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
    }

    protected void MenuVendor_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                UpdateOrderVender();
                BindData();
            }

            if (dce.CommandName.ToUpper().Equals("CLOSE"))
            {
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += " fnReloadList();";
                Script += "</script>" + "\n";
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void UpdateOrderVender()
    {
        PhoenixPurchaseQuotation.UpdateQuotation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            new Guid(ViewState["quotationid"].ToString()),
            General.GetNullableInteger(txtRejectedById.Text), General.GetNullableDateTime(txtRejectedDate.Text),
            General.GetNullableDateTime(txtSentDate.Text), General.GetNullableDateTime(txtRecivedDate.Text),
            General.GetNullableDateTime(txtExpirationDate.Text),
            General.GetNullableInteger(ucCurrency.SelectedCurrency), General.GetNullableDecimal(txtDiscount.Text),
            General.GetNullableDecimal(txtDeliveryTime.Text), txtQtnRefenceno.Text,
            General.GetNullableDateTime(txtOrderDate.Text),
            General.GetNullableInteger(ucModeOfTransport.SelectedQuick),
            General.GetNullableInteger(UCDeliveryTerms.SelectedQuick),
            General.GetNullableInteger(UCPaymentTerms.SelectedQuick),
            General.GetNullableDecimal(txtSupplierDiscount.Text),
            General.GetNullableString(lblEmailIds.Text),
            General.GetNullableInteger(ddlType.SelectedHard));
    }

    protected void cmdMap_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void ddlAccountDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["quotationid"] == null)
            {
                ucError.ErrorMessage = "Please Add a Vendor";
                ucError.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTax_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
              
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidTax(
                    ((TextBox)_gridView.FooterRow.FindControl("txtDescriptionAdd")).Text.ToString().Trim(),
                    ((UserControlTaxType)_gridView.FooterRow.FindControl("ucTaxTypeAdd")).TaxType.ToString(),
                    ((UserControlDecimal)_gridView.FooterRow.FindControl("txtValueAdd")).Text.ToString(),
                    ((UserControlBudgetCode)_gridView.FooterRow.FindControl("ucBudgetCode")).SelectedBudgetSubAccount.ToString(),
                    ((UserControlOwnerBudgetCode)_gridView.FooterRow.FindControl("ucOwnerBudgetCode")).SelectedValue.ToString(),
                    ViewState["quotationid"].ToString()
                    ))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertQuotationTax(
                                     new Guid(ViewState["quotationid"].ToString())
                                    , ((TextBox)_gridView.FooterRow.FindControl("txtDescriptionAdd")).Text.ToString().Trim()
                                    , int.Parse(((UserControlTaxType)_gridView.FooterRow.FindControl("ucTaxTypeAdd")).TaxType)
                                    , decimal.Parse(((UserControlDecimal)_gridView.FooterRow.FindControl("txtValueAdd")).Text.ToString())
                                    , 0
                                    , ((UserControlBudgetCode)_gridView.FooterRow.FindControl("ucBudgetCode")).SelectedBudgetSubAccount.ToString().Trim()
                                    , ((UserControlOwnerBudgetCode)_gridView.FooterRow.FindControl("ucOwnerBudgetCode")).SelectedValue.ToString().Trim()
                                );
                BindDataTax();
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidTax(
                              ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDescriptionEdit")).Text.ToString().Trim(),
                              ((UserControlTaxType)_gridView.Rows[nCurrentRow].FindControl("ucTaxTypeEdit")).TaxType.ToString(),
                              ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtValueEdit")).Text.ToString(),
                              ((UserControlBudgetCode)_gridView.Rows[nCurrentRow].FindControl("ucBudgetCodeEdit")).SelectedBudgetSubAccount.ToString().Trim(),
                              ((UserControlOwnerBudgetCode)_gridView.Rows[nCurrentRow].FindControl("ucOwnerBudgetCodeEdit")).SelectedValue.ToString().Trim(),
                              ViewState["quotationid"].ToString()
                                )
                    )
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateQuotationTax(
                                     new Guid(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTaxMapCodeEdit")).Text.ToString())
                                    , new Guid(ViewState["quotationid"].ToString())
                                    , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDescriptionEdit")).Text.ToString().Trim()
                                    , int.Parse(((UserControlTaxType)_gridView.Rows[nCurrentRow].FindControl("ucTaxTypeEdit")).TaxType)
                                    , decimal.Parse(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtValueEdit")).Text.ToString())
                                    , 0
                                    , ((UserControlBudgetCode)_gridView.Rows[nCurrentRow].FindControl("ucBudgetCodeEdit")).SelectedBudgetSubAccount.ToString().Trim()
                                    , ((UserControlOwnerBudgetCode)_gridView.Rows[nCurrentRow].FindControl("ucOwnerBudgetCodeEdit")).SelectedValue.ToString().Trim()
                                );
                _gridView.EditIndex = -1;
                BindDataTax();
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                _gridView.EditIndex = Int32.Parse(e.CommandArgument.ToString());
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteQuotationTax(new Guid(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTaxMapCode")).Text.ToString())
                                     , new Guid(ViewState["quotationid"].ToString()));
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
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

    protected bool IsValidTax(string strDescription, string strValueType, string strValue, string budgetid, string ownerBudgetCode, string quotationid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (strDescription.Trim() == string.Empty)
            ucError.ErrorMessage = "Description is required.";
        if (strValue.Trim() == string.Empty)
            ucError.ErrorMessage = "Value is required.";
        if (strValueType.Trim() == string.Empty)
            ucError.ErrorMessage = "Type is required.";
        if (General.GetNullableGuid(quotationid) == null)
            ucError.ErrorMessage = "Invalid Quotation .";


        return (!ucError.IsError);
    }

    protected void InsertQuotationTax(Guid uQuotationId, string strDescription, int iTaxType, decimal dValue, int iOffsetVessel, string strBudgetCode, string ownerbudgetcode)
    {

        PhoenixPurchaseQuotation.QuotationTaxMapInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, uQuotationId, strDescription, iTaxType, dValue, iOffsetVessel, General.GetNullableString(strBudgetCode), 1, General.GetNullableGuid(ownerbudgetcode));

    }

    protected void UpdateQuotationTax(Guid uTaxMapCode, Guid uQuotationId, string strDescription, int iTaxType, decimal dValue, int iOffsetVessel, string strBudgetCode, string ownerbudgetcode)
    {

        PhoenixPurchaseQuotation.QuotationTaxMapUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, uTaxMapCode, uQuotationId, strDescription, iTaxType, dValue, iOffsetVessel, General.GetNullableString(strBudgetCode), General.GetNullableGuid(ownerbudgetcode));
    }

    protected void DeleteQuotationTax(Guid uTaxMapCode, Guid uQuotationId)
    {
        PhoenixPurchaseQuotation.QuotationTaxMapDelete(uTaxMapCode, uQuotationId);
    }

    protected void gvTax_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindDataTax();
    }

    protected void gvTax_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = e.NewEditIndex;
        BindDataTax();
    }

    protected void gvTax_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        BindDataTax();
    }

    protected void gvTax_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (ViewState["Lock"].ToString().Equals("Y"))
            {
                e.Row.Enabled = false;
            }

            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            TextBox tb = (TextBox)e.Row.FindControl("txtBudgetName");
            if (tb != null) tb.Attributes.Add("style", "visibility:hidden");
            tb = (TextBox)e.Row.FindControl("txtBudgetId");
            TextBox txtBudgetId = (TextBox)e.Row.FindControl("txtBudgetId");
            if (tb != null) tb.Attributes.Add("style", "visibility:hidden");
            tb = (TextBox)e.Row.FindControl("txtBudgetgroupId");
            if (tb != null) tb.Attributes.Add("style", "visibility:hidden");
            ImageButton ib = (ImageButton)e.Row.FindControl("btnShowBudget");


            UserControlBudgetCode ucBudget = (UserControlBudgetCode)e.Row.FindControl("ucBudgetCode");
            if (ucBudget != null)
            {
                ucBudget.BudgetCodeList = PhoenixRegistersBudget.ListBudget();
                ucBudget.DataBind();
            }

            UserControlOwnerBudgetCode ucOwnerBudget = (UserControlOwnerBudgetCode)e.Row.FindControl("ucOwnerBudgetCode");
            if (ucOwnerBudget != null)
            {
                if (General.GetNullableGuid(ucOwnerBudget.SelectedBudgetCode) != null)
                    ViewState["OwnerBudgetId"] = ucOwnerBudget.SelectedBudgetCode;

                ucOwnerBudget.BudgetCodeList = PhoenixPurchaseBudgetCode.ListOwnerBudgetGroup(null, null, General.GetNullableInteger("757"), General.GetNullableInteger(Filter.CurrentPurchaseVesselSelection.ToString()), General.GetNullableInteger(ucBudget.SelectedBudgetCode)); 
                ucOwnerBudget.DataBind();
                
                if (ViewState["OwnerBudgetId"] != null && General.GetNullableGuid(ViewState["OwnerBudgetId"].ToString()) != null)
                    ucOwnerBudget.SelectedValue = ViewState["OwnerBudgetId"].ToString();
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox tb1 = (TextBox)e.Row.FindControl("txtBudgetNameEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "display:none");
            tb1 = (TextBox)e.Row.FindControl("txtBudgetIdEdit");
            TextBox txtBudgetIdedit = (TextBox)e.Row.FindControl("txtBudgetIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "display:none");
            tb1 = (TextBox)e.Row.FindControl("txtBudgetgroupIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "display:none");
            ImageButton ib1 = (ImageButton)e.Row.FindControl("btnShowBudgetEdit");
            if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SPARE"))
            {
                if (ib1 != null) ib1.Attributes.Add("onclick", "return showPickList('spnPickListTaxBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + Filter.CurrentPurchaseVesselSelection + "&budgetdate=" + txtSentDate.Text + "', true); ");
            }
            else
            {
                if (ib1 != null) ib1.Attributes.Add("onclick", "return showPickList('spnPickListTaxBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=105&hardtypecode=30&vesselid=" + Filter.CurrentPurchaseVesselSelection + "&budgetdate=" + txtSentDate.Text + "', true); ");
            }

            tb1 = (TextBox)e.Row.FindControl("txtOwnerBudgetNameEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "display:none");
            tb1 = (TextBox)e.Row.FindControl("txtOwnerBudgetIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "display:none");
            tb1 = (TextBox)e.Row.FindControl("txtOwnerBudgetgroupIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "display:none");
            ImageButton ib2 = (ImageButton)e.Row.FindControl("btnShowOwnerBudgetEdit");
            if (ib2 != null)
            {
                if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SPARE"))
                    ib2.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?vesselid=" + Filter.CurrentPurchaseVesselSelection + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetIdedit.Text + "', true); ");
                else
                    ib2.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?vesselid=" + Filter.CurrentPurchaseVesselSelection + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetIdedit.Text + "', true); ");

            }
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (drv["FLDISGST"].ToString() == "1")
            {
                UserControlTaxType uctTaxType = (UserControlTaxType)e.Row.FindControl("ucTaxTypeEdit");
                if (uctTaxType != null) uctTaxType.Enabled = false;
            }


            UserControlBudgetCode ucBudget = (UserControlBudgetCode)e.Row.FindControl("ucBudgetCodeEdit");
            if (ucBudget != null)
            {
                ucBudget.BudgetCodeList = PhoenixRegistersBudget.ListBudget();
                ucBudget.DataBind();
                ucBudget.SelectedBudgetSubAccount = drv["FLDSUBACCOUNT"].ToString();
            }

            UserControlOwnerBudgetCode ucOwnerBudget = (UserControlOwnerBudgetCode)e.Row.FindControl("ucOwnerBudgetCodeEdit");
            if (ucOwnerBudget != null)
            {
                //if (General.GetNullableGuid(ucOwnerBudget.SelectedBudgetCode) != null)
                //    ViewState["OwnerBudgetId"] = ucOwnerBudget.SelectedBudgetCode;

                ucOwnerBudget.BudgetCodeList = PhoenixPurchaseBudgetCode.ListOwnerBudgetGroup(null, null, General.GetNullableInteger("757"), General.GetNullableInteger(Filter.CurrentPurchaseVesselSelection.ToString()), General.GetNullableInteger(ucBudget.SelectedBudgetCode));
                ucOwnerBudget.DataBind();

                //if (ViewState["OwnerBudgetId"] != null && General.GetNullableGuid(ViewState["OwnerBudgetId"].ToString()) != null)
                //    ucOwnerBudget.SelectedValue = ViewState["OwnerBudgetId"].ToString();
                ucOwnerBudget.SelectedValue = drv["FLDOWNERBUDGETID"].ToString();
            }
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = {"FLDVENDORID", "FLDNAME","FLDRECEIVEDDATE","FLDTOTALPRICE","FLDTOTALDISCOUNT","TOTALAMOUNT",
                                 "FLDDELIVERYTIME","STATUS" };
        string[] alCaptions = {"Vendor ID", "Vendor","Received Date","Quoted Price","Discount","Total Amount",
                                 "Delivery Time","Quoted" };
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

    protected void ddlAccountDetails_DataBound(object sender, EventArgs e)
    {
        ddlAccountDetails.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void MenuVendorList_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("MULTIPLEVENDOR"))
            {
                Response.Redirect("../Purchase/PurchaseMultipleVendorSelection.aspx?orderid=" + ViewState["orderid"].ToString() + "&addresstype=130,131,132");
            }
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (dce.CommandName.ToUpper().Equals("QTNCOMPARE"))
            {
                String scriptpopup = String.Format("");
                if (ViewState["orderid"] != null)
                {
                    string selectedvendors = ",";
                    foreach (GridViewRow gvr in gvVendor.Rows)
                    {
                        if (gvr.FindControl("chkSelect") != null && ((CheckBox)(gvr.FindControl("chkSelect"))).Checked)
                        {
                            selectedvendors = selectedvendors + ((Label)(gvr.FindControl("lblQuotationId"))).Text + ",";
                        }
                    }

                    if (selectedvendors.Length > 1)
                        scriptpopup = String.Format(
                            "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Purchase/PurchaseQuotationCompare.aspx?orderid=" + ViewState["orderid"].ToString() + "&vendors=" + selectedvendors +"');");
                    
                    //ifMoreInfo.Attributes["src"] = "PurchaseQuotationCompare.aspx?orderid=" + ViewState["orderid"].ToString() + "&vendors=" + selectedvendors;
                    else
                    {
                        ucError.ErrorMessage = "There are no quotations to compare.";
                        ucError.Visible = true;
                    }
                }
                else
                {
                    scriptpopup = String.Format(
                            "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Purchase/PurchaseQuotationCompare.aspx');");
                    //ifMoreInfo.Attributes["src"] = "PurchaseQuotationCompare.aspx";
                }
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (dce.CommandName.ToUpper().Equals("QUOTATIONCOMPARE"))
            {
                quotationcomparereport();
            }
            if (dce.CommandName.ToUpper().Equals("RFQ"))
            {
                if (ViewState["FormStatus"] != null && ViewState["FormStatus"].ToString() != "" && ViewState["FormStatus"].ToString() == "53")
                    SendForQuotation();
                else
                {
                    ucError.ErrorMessage = "You cannot send RFQ. Requisition is not in Active status.";
                    ucError.Visible = true;
                    return;
                }
            }
            if (dce.CommandName.ToUpper().Equals("RFQREMAINDER"))
            {
                SendRemindorForQuotation();
            }
            if (dce.CommandName.ToUpper().Equals("ORDER"))
            {

                if (!OrderApprove())
                {
                    ucError.Visible = true;
                    return;
                }
                InsertOrderFormHistory();
                ucConfirmMessage.Visible = true;   
                ucConfirmMessage.Text = "Select which detail to be shown in PO";
            }
            if (dce.CommandName.ToUpper().Equals("SENDAPPROVAL"))
            {
                SendApproval();
            }
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
            foreach (GridViewRow gvr in gvVendor.Rows)
            {
                if (gvr.FindControl("chkSelect") != null && ((CheckBox)(gvr.FindControl("chkSelect"))).Checked)
                {
                    selectedvendors = selectedvendors + ((Label)(gvr.FindControl("lblQuotationId"))).Text + ",";
                }
                if (gvr.FindControl("lblSTATUS") != null && (((Label)(gvr.FindControl("lblSTATUS"))).Text.ToString().ToUpper() == "PARTIAL" || ((Label)(gvr.FindControl("lblSTATUS"))).Text.ToString().ToUpper() == "FULL"))
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
        sbemailbody.AppendLine("For and on behalf of "+ConfigurationManager.AppSettings.Get("companyname").ToString());
        sbemailbody.AppendLine();

        return sbemailbody.ToString();

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

    private void SendRemindorForQuotation()
    {
        string emailid;
        try
        {
            string selectedvendors = ",";
            foreach (GridViewRow gvr in gvVendor.Rows)
            {
                if (gvr.FindControl("chkSelect") != null && ((CheckBox)(gvr.FindControl("chkSelect"))).Checked)
                {
                    selectedvendors = selectedvendors + ((Label)(gvr.FindControl("lblQuotationId"))).Text + ",";
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

    protected string PrepareEmailBodyTextForRemainder(Guid quotationid, string orderformnumber, string frommailid)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataSet ds = PhoenixPurchaseQuotation.GetQuotationDataForEmailBody(PhoenixSecurityContext.CurrentSecurityContext.UserCode, quotationid, "RFQ");
        DataRow dr = ds.Tables[0].Rows[0];

        sbemailbody.Append("This is an automated message. DO NOT REPLY to "+ConfigurationManager.AppSettings.Get("FromMail").ToString() +". Kindly use the \"reply all\" button if you are responding to this message.");
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
        sbemailbody.AppendLine("Please note "+ConfigurationManager.AppSettings.Get("FromMail").ToString() +" is NOT monitored.");

        return sbemailbody.ToString();

    }

    private void SendForQuotation()
    {
        string emailid;
        try
        {
            string selectedvendors = ",";
            foreach (GridViewRow gvr in gvVendor.Rows)
            {
                if (gvr.FindControl("chkSelect") != null && ((CheckBox)(gvr.FindControl("chkSelect"))).Checked)
                {
                    try
                    {
                        DataTable dt = null;
                        Guid? quotationid = General.GetNullableGuid(((Label)gvr.FindControl("lblQuotationId")).Text);

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

                    selectedvendors = selectedvendors + ((Label)(gvr.FindControl("lblQuotationId"))).Text + ",";
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

    protected string PrepareEmailBodyText(Guid quotationid, string orderformnumber, string frommailid)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataSet ds = PhoenixPurchaseQuotation.GetQuotationDataForEmailBody(PhoenixSecurityContext.CurrentSecurityContext.UserCode, quotationid, "RFQ");
        DataRow dr = ds.Tables[0].Rows[0];

        sbemailbody.Append("This is an automated message. DO NOT REPLY to "+ConfigurationManager.AppSettings.Get("FromMail").ToString() +". Kindly use the \"reply all\" button if you are responding to this message.");
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
        sbemailbody.AppendLine("Please note "+ ConfigurationManager.AppSettings.Get("FromMail").ToString() + " is NOT monitored.");

        return sbemailbody.ToString();

    }

    private void quotationcomparereport()
    {
        try
        {
            string selectedvendors = ",";

            foreach (GridViewRow gvr in gvVendor.Rows)
            {
                if (gvr.FindControl("chkSelect") != null && ((CheckBox)(gvr.FindControl("chkSelect"))).Checked)
                {
                    selectedvendors = selectedvendors + ((Label)(gvr.FindControl("lblQuotationId"))).Text + ",";
                }
            }
            if (selectedvendors.Length <= 1)
                selectedvendors = null;

            String scriptpopup = String.Format(
                            "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Purchase/PurchaseQuotationComparePrint.aspx?quotationid=" + selectedvendors + "');");
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVendor_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
        }

        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton cmdVendor = (ImageButton)e.Row.FindControl("cmdVendor");
            Label lblVendorId = (Label)e.Row.FindControl("lblVendorId");
            Label lblQuotationId = (Label)e.Row.FindControl("lblQuotationId");
            ImageButton cmdExcelRFQ = (ImageButton)e.Row.FindControl("cmdExcelRFQ");

            string jvscript = "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Purchase/PurchaseExport2XL.aspx?quotationid=" + lblQuotationId.Text + "'); return false;";

            if (cmdExcelRFQ != null)
            {
                cmdExcelRFQ.Attributes.Add("onclick", jvscript);
                //cmdExcelRFQ.Visible = SessionUtil.CanAccess(this.ViewState, cmdExcelRFQ.CommandName);
                cmdExcelRFQ.Visible = false;
            }

            cmdVendor.Attributes.Add("onclick", "openNewWindow('AddAddress', '', '" + Session["sitepath"] + "/Registers/Registersoffice.aspx?addresscode=" + lblVendorId.Text + "&VIEWONLY=Y'); return false;");
            cmdVendor.Visible = SessionUtil.CanAccess(this.ViewState, cmdVendor.CommandName);

            ImageButton cmdViewQuery = (ImageButton)e.Row.FindControl("cmdViewQuery");

            Label lblFormType = (Label)e.Row.FindControl("lblFormType");
            Label lblWebSession = (Label)e.Row.FindControl("lblWebSession");
            ImageButton imgRequote = (ImageButton)e.Row.FindControl("imgRequote");

            if (imgRequote != null)
                imgRequote.Attributes.Add("onclick", "openNewWindow('ReQuote', '', '../Purchase/PurchaseVendorRequoteRemarks.aspx?quotationid=" + lblQuotationId.Text + "'); return false;");

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

            cmdViewQuery.Attributes.Add("onclick", "openNewWindow('ViewQuerySent', '', '" + Session["sitepath"] + "/Purchase/PurchaseQuotationItems.aspx?SESSIONID=" + lblQuotationId.Text + "&VIEWONLY=Y'); return false;");
            cmdViewQuery = (ImageButton)e.Row.FindControl("cmdWhatIfQty");
            cmdViewQuery.Attributes.Add("onclick", "javascript:openNewWindow('Filters', '', '" + Session["sitepath"] + "/Purchase/PurchaseWhatifQuotationItems.aspx?quotationid=" + lblQuotationId.Text + "'); return false;");
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblIsSelected = (Label)e.Row.FindControl("lblIsSelected");
            Label lblIsApproved = (Label)e.Row.FindControl("lblIsApproved");
            Label lblApprovalExists = (Label)e.Row.FindControl("lblApprovalExists");
            Label lblActiveCurrecy = (Label)e.Row.FindControl("lblActiveCur");
            ImageButton imgFlag = (ImageButton)e.Row.FindControl("imgFlag");
            ImageButton cmdDeSelect = (ImageButton)e.Row.FindControl("cmdDeSelect");
            ImageButton cmdSelect = (ImageButton)e.Row.FindControl("cmdSelect");
            ImageButton imgRemarks = (ImageButton)e.Row.FindControl("imgRemarks");
            

            if (cmdDeSelect != null)
            {
                if (lblIsSelected != null && lblIsApproved != null)
                {
                    if(lblIsSelected.Text.ToUpper().Equals("TRUE"))
                    {
                        cmdDeSelect.Visible = true;
                    }
                    else
                    {
                        cmdDeSelect.Visible = false;
                        cmdDeSelect.Attributes.Add("style", "display:none;");
                    }
                }

                if (cmdDeSelect.Visible == true)
                    cmdDeSelect.Visible = SessionUtil.CanAccess(this.ViewState, cmdDeSelect.CommandName);
            }
            if (cmdSelect != null)
            {

                if (lblIsSelected != null && lblIsApproved != null)
                {
                    if (lblIsSelected.Text.ToUpper().Equals("TRUE"))
                    {
                        cmdSelect.Visible = false;
                        cmdSelect.Attributes.Add("style", "display:none;");
                    }
                    else
                    {
                        cmdSelect.Visible = true;
                    }
                }

                if (cmdSelect.Visible == true)
                    cmdSelect.Visible = SessionUtil.CanAccess(this.ViewState, cmdSelect.CommandName);
                if (lblActiveCurrecy.Text == "0")
                {
                    LinkButton suppliername = (LinkButton)e.Row.FindControl("lnkVendorName");
                    string msg = "Exchange Rate for Quotation Currency from " + suppliername.Text + " is outdated. Please convert to USD manually when evaluating the quotations.Please do note that only quotation with Active currency can be approved";
                    cmdSelect.Attributes.Add("onclick", "return fnConfirmDelete(event,'" + msg + "')");
                }
            }
            if (!string.IsNullOrEmpty(drv["FLDREMARKS"].ToString()))
            {
                imgRemarks.Visible = true;
            }

            imgFlag.ImageUrl = lblIsSelected.Text.ToUpper().Equals("TRUE") ? Session["images"] + "/14.png" : Session["images"] + "/spacer.gif";

            ImageButton cmdApprove = (ImageButton)e.Row.FindControl("cmdApprove");
            if (lblIsSelected.Text.ToUpper().Equals("TRUE"))
            {
                Label lblTotalAmount = (Label)e.Row.FindControl("lblPrice");
                cmdApprove.ImageUrl = Session["images"] + "/approve.png";
                cmdApprove.Enabled = true;
                cmdApprove.ToolTip = "Approve";
                if(cmdApprove!=null)
                {
                    if (General.GetNullableInteger(drv["FLDISBUDGETED"].ToString()) == 1)
                        cmdApprove.Attributes.Add("onclick", "openNewWindow('approval', '', '" + Session["sitepath"] + "/Purchase/PurchaseRemainingBudget.aspx?quotationid=" + drv["FLDQUOTATIONID"].ToString() + "&type=" + drv["FLDQUOTATIONAPPROVAL"].ToString() + "&user=" + drv["FLDTECHDIRECTOR"].ToString() + "," + drv["FLDFLEETMANAGER"].ToString() + "," + drv["FLDSUPT"].ToString() + "&currentorder=" + lblTotalAmount.Text + "&directapprovalyn=Y');return false;");
                    
                    cmdApprove.Visible = General.GetNullableInteger(drv["FLDFORMBUDGETCODE"].ToString()) != null ? SessionUtil.CanAccess(this.ViewState, cmdApprove.CommandName) : false;
                    cmdApprove.Width = 16;
                    cmdApprove.Height = 16;
                    
                }
                
            }
            else
            {
                cmdApprove.Attributes.Add("style", "display:none;");
            }

            ImageButton cmdDeApprove = (ImageButton)e.Row.FindControl("cmdDeApprove");
            if (lblIsSelected.Text.ToUpper().Equals("TRUE"))
            {
                cmdDeApprove.Enabled = true;
                cmdDeApprove.ToolTip = "Revoke approval";
            }
            else
            {
                cmdDeApprove.Attributes.Add("style", "display:none;");
            }

            bool visible = lblIsSelected.Text.ToUpper().Equals("TRUE") ? (lblApprovalExists.Text.ToUpper().Equals("1") ? true : false) : false;

            if (visible)
                visible = SessionUtil.CanAccess(this.ViewState, cmdDeApprove.CommandName);

            cmdDeApprove.Visible = visible;
            cmdDeApprove.Width = 16;
            cmdDeApprove.Height = 16;

            ImageButton cmdWhatIfQty = (ImageButton)e.Row.FindControl("cmdWhatIfQty");
            if(lblIsSelected.Text.ToUpper().Equals("TRUE"))
            {
                cmdWhatIfQty.ImageUrl = Session["images"] + "/edit-quantity.png";
                cmdWhatIfQty.Enabled = true;
                cmdWhatIfQty.Visible = SessionUtil.CanAccess(this.ViewState, cmdWhatIfQty.CommandName);
                cmdWhatIfQty.Width = 16;
                cmdWhatIfQty.Height = 16;
            }
            else
            {
                cmdWhatIfQty.Attributes.Add("style", "display:none;");
            }

            if (lblIsSelected.Text.ToUpper().Equals("TRUE") && lblIsApproved.Text.ToUpper().Equals("1"))
            {
                cmdWhatIfQty.Attributes.Add("style", "display:none;");
                cmdApprove.Attributes.Remove("onclick");
            }

            Label lblAppStatus = (Label)e.Row.FindControl("lblAppStatus");
            UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucToolTipAddress");
            lblAppStatus.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lblAppStatus.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            lblAppStatus.Visible = (lblIsSelected.Text.ToUpper().Equals("TRUE") && drv["FLDAPPROVALSTATUS"].ToString() != string.Empty) ? true : false;

            Label lblSendDateCode = (Label)e.Row.FindControl("lblSendDateCode");
            ImageButton cmdViewQuery = (ImageButton)e.Row.FindControl("cmdViewQuery");
            if (!lblSendDateCode.Text.ToUpper().Equals(""))
                cmdViewQuery.ImageUrl = Session["images"] + "/48.png";
            else
                cmdViewQuery.Attributes.Add("style", "display:none;");
            //cmdViewQuery.ImageUrl = !lblSendDateCode.Text.ToUpper().Equals("") ? Session["images"] + "/48.png" : Session["images"] + "/spacer.gif";
            cmdViewQuery.Visible = SessionUtil.CanAccess(this.ViewState, cmdViewQuery.CommandName);

            if (lblSendDateCode.Text.ToUpper().Equals(""))
            {
                cmdViewQuery.Width = 16;
                cmdViewQuery.Height = 16;
                cmdViewQuery.Enabled = false;
            }

            ImageButton imgRequote = (ImageButton)e.Row.FindControl("imgRequote");
            if (drv["APPOVEDSTATUS"].ToString() != string.Empty && drv["APPOVEDSTATUS"].ToString() == "1")
            {
                imgRequote.Visible = false;
            }

            ImageButton cmdAudit = (ImageButton)e.Row.FindControl("cmdAudit");
            cmdAudit.Visible = SessionUtil.CanAccess(this.ViewState, cmdAudit.CommandName);
            if (Filter.CurrentPurchaseStockType.ToUpper().Equals("STORE"))
                cmdAudit.Attributes.Add("onclick", "openNewWindow('Audit', '', '" + Session["sitepath"] + "/Common/CommonPhoenixAuditChanges.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&shortcode=PUR-QTNSTORE');return false;");
            else
                cmdAudit.Attributes.Add("onclick", "openNewWindow('Audit', '', '" + Session["sitepath"] + "/Common/CommonPhoenixAuditChanges.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&shortcode=PUR-QTN');return false;");

            ImageButton iab = (ImageButton)e.Row.FindControl("cmdAttachment");
            ImageButton inab = (ImageButton)e.Row.FindControl("cmdNoAttachment");
            if (iab != null) iab.Visible = true;
            if (inab != null) inab.Visible = false;
            int? n = General.GetNullableInteger(drv["FLDATTACHMENTCOUNT"].ToString());
            if (n == 0)
            {
                if (iab != null) iab.Visible = false;
                if (inab != null) inab.Visible = true;
            }
        }
    }

    protected void gvVendor_Sorting(object sender, GridViewSortEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvVendor_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void gvVendor_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        ViewState["quotationid"] = ((Label)_gridView.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblQuotationId")).Text;
        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            try
            {
                PhoenixPurchaseQuotation.DeleteQuotation(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(((Label)_gridView.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblQuotationId")).Text));
                ViewState["quotationid"] = "0";
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
        }
        else if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            UpdateSelectVendorForPo(((Label)_gridView.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblQuotationId")).Text);
            InsertOrderFormHistory();

        }
        else if (e.CommandName.ToUpper().Equals("DESELECT"))
        {
            DeSelectVendor(((Label)_gridView.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblQuotationId")).Text);
            //InsertOrderFormHistory();            
        }
        else if (e.CommandName.ToUpper().Equals("REQUOTE"))
        {
        }
        else if (e.CommandName.ToUpper().Equals("APPROVE"))
        {
            try
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string quotationid = ((Label)_gridView.Rows[index].FindControl("lblQuotationId")).Text;

                PhoenixCommonPurchase.UpdateQuotationApproval(new Guid(quotationid), 0);
                ucStatus.Text = "Approved";
                ucStatus.Visible = true;
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
                            dr["FLDSUBJECT"].ToString() + "     " + PhoenixPurchaseOrderForm.FormNumber,
                            emailbodytext,
                            true,
                            System.Net.Mail.MailPriority.Normal,
                            "",
                            null,
                            null);
                    }
                    Session["POQAPPROVE"] = null;
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
        else if (e.CommandName.ToUpper().Equals("DEAPPROVE"))
        {
            try
            {
                DataSet dsApproval = PhoenixPurchaseQuotation.QuotationApprovalEdit(
                    new Guid(ViewState["orderid"].ToString()),
                    new Guid(((Label)_gridView.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblQuotationId")).Text));

                if (dsApproval.Tables.Count > 0 && dsApproval.Tables[0].Rows.Count > 0)
                {
                    DataRow drApproval = dsApproval.Tables[0].Rows[0];

                    if (drApproval["FLDFULLAPPROVAL"].ToString().Equals("1"))
                    {
                        string emailbodytext = "";

                        DataSet ds = PhoenixPurchaseOrderForm.ApproveOrderForm(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["orderid"].ToString())
                            , General.GetNullableGuid(((Label)_gridView.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblQuotationId")).Text)
                            , General.GetNullableDateTime(DateTime.Now.ToString()),
                            Int32.Parse(((Label)_gridView.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblIsApproved")).Text));

                        if (Int32.Parse(((Label)_gridView.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblIsApproved")).Text) == 1)
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
                            }
                            ucConfirm.ErrorMessage = "Purchase approval is cancelled.";
                        }
                        ucConfirm.Visible = true;
                    }
                    else if (drApproval["FLDAPPROVALEXISTS"].ToString().Equals("1"))
                    {
                        PhoenixPurchaseQuotation.DeletePartialApproval(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            new Guid(((Label)_gridView.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblQuotationId")).Text));

                        ucConfirm.ErrorMessage = "Purchase approval is cancelled.";
                        ucConfirm.Visible = true;
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
        GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
        string QuotationID = ((Label)row.FindControl("lblQuotationId")).Text;
        if (e.CommandName.ToUpper().Equals("REMARKS"))
        {
            String scriptpopup = String.Format("javascript:openNewWindow('Remarks', '', '" + Session["sitepath"] + "/Purchase/PurchaseVendorDetail.aspx?QUOTATIONID=" + QuotationID + "&editable=false');");
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
        }
        if (e.CommandName.ToUpper().Equals("ATTACHMENTS") || e.CommandName.ToUpper().Equals("NOATTACHMENT"))
        {
            string dtkey = ((Label)row.FindControl("lbldtkey")).Text;
            String scriptpopup = String.Format("javascript:openNewWindow('Remarks', '', '" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?DTKEY=" + dtkey + "&MOD=PURCHASE');");
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
        }
        if (e.CommandName.ToUpper().Equals("DELIVERY"))
        {
            String scriptpopup = String.Format("javascript:openNewWindow('Remarks', '', '" + Session["sitepath"] + "/Purchase/PurchaseQuotationDeliveryInstruction.aspx?QUOTATIONID=" + QuotationID + "&editable=true');");
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
        }
        BindData();
        SetPageNavigator();
    }

    private void UpdateSelectVendorForPo( string quotationid)
    {
        try
        {
            PhoenixPurchaseQuotation.UpdateQuotation(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["orderid"].ToString()), new Guid(quotationid), Convert.ToInt32(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.FORMTYPE), "PO")));

            PhoenixPurchaseQuotation.SendApprovalInsert
                    (PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["orderid"].ToString()),"yes", 1);
       }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void InsertOrderFormHistory()
    {
        PhoenixPurchaseOrderForm.InsertOrderFormHistory(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection);
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
        sbemailbody.AppendLine("For and on behalf of "+ConfigurationManager.AppSettings.Get("companyname").ToString());
        sbemailbody.AppendLine();

        return sbemailbody.ToString();

    }

    protected void onPurchaseQuotation(object sender, CommandEventArgs e)
    {
        ViewState["quotationid"] = ((Label)gvVendor.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblQuotationId")).Text;
        PhoenixPurchaseQuotation.VendorName = ((LinkButton)gvVendor.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lnkVendorName")).Text;
        if (ViewState["orderid"] != null)
        {
            if (ViewState["quotationid"] != null)
                Response.Redirect("../Purchase/PurchaseQuotationLineItem.aspx?quotationid=" + ViewState["quotationid"].ToString() + "&orderid=" + ViewState["orderid"].ToString());
            else
                Response.Redirect("../Purchase/PurchaseQuotationLineItem.aspx?quotationid=" + ViewState["orderid"].ToString());
        }
        else
        {
            if (ViewState["quotationid"] != null)
                Response.Redirect("../Purchase/PurchaseQuotationLineItem.aspx?quotationid=" + ViewState["quotationid"].ToString());
            else
                Response.Redirect("../Purchase/PurchaseQuotationLineItem.aspx");
        }
    }

    public void CopyForm_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
            String scriptpopup = String.Format("");
            if (ucCM.confirmboxvalue == 1)
            {
                scriptpopup = String.Format(
                            "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=3&reportcode=PURCHASEORDERFORM&quotationid=" + ViewState["quotationid"].ToString() + "&orderid=" + ViewState["orderid"].ToString() + "&showactual=1&showword=no&showexcel=no');");
            }
            else
            {
                scriptpopup = String.Format(
                            "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=3&reportcode=PURCHASEORDERFORM&quotationid=" + ViewState["quotationid"].ToString() + "&orderid=" + ViewState["orderid"].ToString() + "&showactual=0&showword=no&showexcel=no');");
            }
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void onPurchaseQuotationEdit(object sender, CommandEventArgs e)
    {
        try
        {
            ViewState["quotationid"] = ((Label)gvVendor.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblQuotationId")).Text;
            ViewState["DTKEY"] = ((Label)gvVendor.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblDtKey")).Text;
            BindDataTax();
            //LoadBudgetCode();
            SetRowSelection();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
        for (int i = 0; i < gvVendor.Rows.Count; i++)
        {
            if (ViewState["quotationid"] != null)
            {
                if (gvVendor.DataKeys[i].Value.ToString().Equals(ViewState["quotationid"].ToString()))
                {
                    gvVendor.SelectedIndex = i;
                    BindFields(ViewState["quotationid"].ToString());
                }
            }
        }
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindData();
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvVendor.SelectedIndex = -1;
        gvVendor.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    protected void ucBudgetCode_TextChangedEvent(object sender, EventArgs e)
    {
        UserControlBudgetCode ucBudget = (UserControlBudgetCode)sender;
        GridViewRow row = (GridViewRow)ucBudget.NamingContainer;
        if (row != null)
        {
            if ((row.RowState & DataControlRowState.Edit) > 0)
            {
                UserControlOwnerBudgetCode ddl = (UserControlOwnerBudgetCode)gvTax.Rows[row.RowIndex].FindControl("ucOwnerBudgetCodeEdit");

                if (General.GetNullableGuid(ddl.SelectedBudgetCode) != null)
                    ViewState["OwnerBudgetId"] = ddl.SelectedBudgetCode;

                //ddl.OwnerId = "757";
                //ddl.VesselId = Filter.CurrentPurchaseVesselSelection.ToString();
                //ddl.BudgetId = ucBudget.SelectedBudgetCode;

                ddl.BudgetCodeList = PhoenixPurchaseBudgetCode.ListOwnerBudgetGroup(null, null, General.GetNullableInteger("757"), General.GetNullableInteger(Filter.CurrentPurchaseVesselSelection.ToString()), General.GetNullableInteger(ucBudget.SelectedBudgetCode));
                ddl.DataBind();

                if (ViewState["OwnerBudgetId"] != null && General.GetNullableGuid(ViewState["OwnerBudgetId"].ToString()) != null)
                    ddl.SelectedBudgetCode = ViewState["OwnerBudgetId"].ToString();
            }
            else
            {
                UserControlOwnerBudgetCode ddl = (UserControlOwnerBudgetCode)gvTax.FooterRow.FindControl("ucOwnerBudgetCode");

                if (General.GetNullableGuid(ddl.SelectedBudgetCode) != null)
                    ViewState["OwnerBudgetId"] = ddl.SelectedBudgetCode;

                //ddl.OwnerId = "757";
                //ddl.VesselId = Filter.CurrentPurchaseVesselSelection.ToString();
                //ddl.BudgetId = ucBudget.SelectedBudgetCode;

                ddl.BudgetCodeList = PhoenixPurchaseBudgetCode.ListOwnerBudgetGroup(null, null, General.GetNullableInteger("757"), General.GetNullableInteger(Filter.CurrentPurchaseVesselSelection.ToString()), General.GetNullableInteger(ucBudget.SelectedBudgetCode));
                ddl.DataBind();

                if (ViewState["OwnerBudgetId"] != null && General.GetNullableGuid(ViewState["OwnerBudgetId"].ToString()) != null)
                    ddl.SelectedBudgetCode = ViewState["OwnerBudgetId"].ToString();
            }
        }
    }

    protected void cmdDiscount_Click(object sender, ImageClickEventArgs e)
    {
       String scriptpopup = String.Format("javascript:openNewWindow('codehelp', '', '" + Session["sitepath"] + "/Purchase/PurchaseQuotationEsmDiscount.aspx?quotationid=" + ViewState["quotationid"] + "&discount=" + txtDiscount.Text + "');");
        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
    }

    protected void cmdMapUser_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (ViewState["quotationid"].ToString() == null || ViewState["quotationid"].ToString() == "")
            {
                ucError.ErrorMessage = "Please Add a Vendor";
                ucError.Visible = true;
                return;
            }

            int? approvalby = General.GetNullableInteger(ucApprovalBy.SelectedValue);
            string stocktype = Filter.CurrentPurchaseStockType;

            PhoenixPurchaseQuotation.QuotaitionApprovalMap(new Guid(ViewState["orderid"].ToString()), new Guid(ViewState["quotationid"].ToString()), approvalby, stocktype);
            ucStatus.Text = "Successfully Mapped";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
