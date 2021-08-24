using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class AccountsInvoiceDirectPO : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Invoice", "INVOICE");
        toolbarmain.AddButton("PO", "PO");
        toolbarmain.AddButton("Direct PO", "DIRECTPO");
        toolbarmain.AddButton("Attachments", "ATTACHMENTS");
        MenuDirectPO.AccessRights = this.ViewState;
        MenuDirectPO.MenuList = toolbarmain.Show();
        MenuDirectPO.SelectedMenuIndex = 2;

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Invoice Summary", "INVOICESUMMARY", ToolBarDirection.Right);
        toolbar.AddButton("Line Item", "LINEITEM", ToolBarDirection.Right);
        toolbar.AddButton("General", "GENERAL", ToolBarDirection.Right);


        MenuDPO.AccessRights = this.ViewState;
        MenuDPO.MenuList = toolbar.Show();
        MenuDPO.SelectedMenuIndex = 2;


        if (!IsPostBack)
        {
            ViewState["INVOICECODE"] = Request.QueryString["qinvoicecode"];
            if (Request.QueryString["qcallfrom"] != null && Request.QueryString["qcallfrom"] != string.Empty)
            {
                ViewState["ReceivedFrom"] = Request.QueryString["qcallfrom"];
            }
            //Title1.Text = "Direct PO  (  " + PhoenixAccountsVoucher.InvoiceNumber + "     )";
            ifMoreInfo.Attributes["src"] = "AccountsInvoiceDirectPOGeneral.aspx?qinvoicecode=" + ViewState["INVOICECODE"];
            ViewState["VESSELID"] = null;
            ViewState["SETCURRENTNAVIGATIONTAB"] = null;
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["ORDERID"] = null;

            gvDPO.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            DataSet dsInvoice = PhoenixAccountsInvoice.InvoiceEdit(new Guid(ViewState["INVOICECODE"].ToString()));
            if (dsInvoice.Tables.Count > 0)
            {
                if (dsInvoice.Tables[0].Rows.Count > 0)
                {
                    DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                    ViewState["InvoiceType"] = drInvoice["FLDINVOICETYPE"].ToString();
                }
            }

        }

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddImageButton("../Accounts/AccountsInvoiceDirectPO.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbargrid.AddImageLink("javascript:CallPrint('gvDPO')", "Print Grid", "icon_print.png", "PRINT");
        // if (ViewState["InvoiceType"].ToString() != "584")
        //{
        toolbargrid.AddImageLink("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsAmosInvoiceLineItem.aspx?qinvoicecode=" + ViewState["INVOICECODE"].ToString() + "'); return false;", "Add", "Add.png", "ADD");
        //  }
        MenuAddAmosPO.AccessRights = this.ViewState;
        MenuAddAmosPO.MenuList = toolbargrid.Show();

        // BindData();
    }
    protected void MenuDirectPO_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("INVOICE"))
            {
                if (ViewState["ReceivedFrom"].ToString() == "invoice")
                {
                    Response.Redirect("../Accounts/AccountsInvoiceMaster.aspx?qinvoicecode=" + ViewState["INVOICECODE"].ToString());
                }
                else if (ViewState["ReceivedFrom"].ToString() == "invoiceforpurchase")
                {
                    Response.Redirect("../Accounts/AccountsInvoiceMasterForPurchase.aspx?qinvoicecode=" + ViewState["INVOICECODE"].ToString());
                }
            }
            if (CommandName.ToUpper().Equals("PO") && ViewState["INVOICECODE"] != null && ViewState["INVOICECODE"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsInvoiceLineItemDetails.aspx?qinvoicecode=" + ViewState["INVOICECODE"] + "&qcallfrom=" + ViewState["ReceivedFrom"]);
            }
            else if (CommandName.ToUpper().Equals("ATTACHMENTS"))
            {
                Response.Redirect("../Accounts/AccountsInvoiceAttachments.aspx?qinvoicecode=" + ViewState["INVOICECODE"].ToString() + "&qfrom=" + ViewState["ReceivedFrom"]);
            }
            else if (CommandName.ToUpper().Equals("DIRECTPO") && ViewState["INVOICECODE"] != null && ViewState["INVOICECODE"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsInvoiceDirectPO.aspx?qinvoicecode=" + ViewState["INVOICECODE"] + "&qcallfrom=" + ViewState["ReceivedFrom"]);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuDPO_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                ViewState["SETCURRENTNAVIGATIONTAB"] = "AccountsInvoiceDirectPOGeneral.aspx";
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"] + "?qinvoicecode=" + ViewState["INVOICECODE"] + (ViewState["ORDERID"] != null ? "&orderid=" + ViewState["ORDERID"] + "&vslid=" + ViewState["VESSELID"] : string.Empty);
                MenuDPO.SelectedMenuIndex = 2;
            }
            else if (CommandName.ToUpper().Equals("LINEITEM") && ViewState["ORDERID"] != null)
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                ViewState["SETCURRENTNAVIGATIONTAB"] = "AccountsInvoiceDirectPOLineItem.aspx";
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"] + "?qinvoicecode=" + ViewState["INVOICECODE"] + (ViewState["ORDERID"] != null ? "&orderid=" + ViewState["ORDERID"] + "&vslid=" + ViewState["VESSELID"] : string.Empty);
                MenuDPO.SelectedMenuIndex = 1;
            }
            else if (CommandName.ToUpper().Equals("INVOICESUMMARY"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                ViewState["SETCURRENTNAVIGATIONTAB"] = "AccountsInvoiceSummary.aspx";
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"] + "?qinvoicecode=" + ViewState["INVOICECODE"] + (ViewState["ORDERID"] != null ? "&orderid=" + ViewState["ORDERID"] + "&vslid=" + ViewState["VESSELID"] : string.Empty);
                MenuDPO.SelectedMenuIndex = 0;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuAddAmosPO_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {

        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alCaptions = {
                                "PO Number",
                                "Vendor",
                                "Vessel",
                                "Currency",
                                "PO Received Date",
                              };

            string[] alColumns = {  "FLDFORMNO",
                                "FLDNAME",
                                "FLDVESSELNAME",
                                "FLDCURRENCYNAME",
                                "FLDRECEIVEDDATE",
                             };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixAccountsInvoice.InvoiceDirectPOSearch(General.GetNullableGuid(ViewState["INVOICECODE"].ToString())
                            , sortexpression, sortdirection
                            , (int)ViewState["PAGENUMBER"]
                            , gvDPO.PageSize, ref iRowCount, ref iTotalPageCount);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvDPO", "Direct PO", alCaptions, alColumns, ds);
            gvDPO.DataSource = dt;
            gvDPO.VirtualItemCount = iRowCount;
            if (dt.Rows.Count > 0)
            {
                if (ViewState["ORDERID"] == null)
                {
                    ViewState["VESSELID"] = dt.Rows[0]["FLDVESSELID"].ToString();
                    ViewState["ORDERID"] = dt.Rows[0]["FLDORDERID"].ToString();
                }
                if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "AccountsInvoiceDirectPOGeneral.aspx";
                }
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"] + "?qinvoicecode=" + ViewState["INVOICECODE"] + "&orderid=" + ViewState["ORDERID"] + "&vslid=" + ViewState["VESSELID"];
                SetTabHighlight();
            }
            else
            {
                ifMoreInfo.Attributes["src"] = "AccountsInvoiceDirectPOGeneral.aspx?qinvoicecode=" + ViewState["INVOICECODE"];
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = {
                                "PO Number",
                                "Vendor",
                                "Vessel",
                                "Currency",
                                "PO Received Date",
                              };

        string[] alColumns = {  "FLDFORMNO",
                                "FLDNAME",
                                "FLDVESSELNAME",
                                "FLDCURRENCYNAME",
                                "FLDRECEIVEDDATE",
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

        DataTable dt = PhoenixAccountsInvoice.InvoiceDirectPOSearch(General.GetNullableGuid(ViewState["INVOICECODE"].ToString())
                           , sortexpression, sortdirection
                           , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                           , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount, ref iRowCount, ref iTotalPageCount);

        ds.Tables.Add(dt.Copy());

        Response.AddHeader("Content-Disposition", "attachment; filename=DirectPO.xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Direct PO</h3></td>");
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

    protected void gvDPO_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridHeaderItem)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Item.FindControl(ViewState["SORTEXPRESSION"].ToString());
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
        if (e.Item is GridDataItem)
        {
            RadLabel lblIsPoTaggedToInvoice = (RadLabel)e.Item.FindControl("lblIsPoTaggedToInvoice");
            ImageButton cmdPOApprove = (ImageButton)e.Item.FindControl("cmdPOApprove");
            RadLabel lblOrderId = (RadLabel)e.Item.FindControl("lblOrderId");
            RadLabel lblVendorId = (RadLabel)e.Item.FindControl("lblVendorId");
            DataSet dsInvoice = PhoenixAccountsInvoice.InvoiceEdit(new Guid(ViewState["INVOICECODE"].ToString()));
            if (dsInvoice.Tables.Count > 0)
            {
                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                ViewState["InvoiceType"] = drInvoice["FLDINVOICETYPE"].ToString();
            }

            if (lblIsPoTaggedToInvoice != null)
            {
                if (lblIsPoTaggedToInvoice.Text.Equals("0"))
                {
                    if (cmdPOApprove != null)
                        cmdPOApprove.Visible = true;
                    string type = "";
                    if (ViewState["InvoiceType"].ToString() == "238")
                    {
                        type = "1050";
                    }
                    else if (ViewState["InvoiceType"].ToString() == "239")
                    {
                        type = "1051";
                    }
                    else if (ViewState["InvoiceType"].ToString() == "240")
                    {
                        type = "1052";
                    }
                    else if (ViewState["InvoiceType"].ToString() == "584")
                    {
                        type = "1053";
                    }
                    else if (ViewState["InvoiceType"].ToString() == "585")
                    {
                        type = "1054";
                    }
                    else if (ViewState["InvoiceType"].ToString() == "969")
                    {
                        type = "1055";
                    }
                    //cmdPOApprove.Attributes.Add("onclick", "parent.Openpopup('PaymentVoucherApproval', '', '../Common/CommonApproval.aspx?docid=" + lblOrderId.Text + "&mod=" + PhoenixModule.ACCOUNTS + "&type=" + type + "&vouchertype=0"+"&suppliercode=0"  + "&user=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "');return false;");
                    //For Invoice type supplier, PO Quotation approval is used for Direct PO approval also.
                    if (ViewState["InvoiceType"] != null && ViewState["InvoiceType"].ToString() == "239")
                    {
                        type = drv["FLDQUOTATIONAPPROVAL"].ToString();
                        cmdPOApprove.Attributes.Add("onclick", "parent.Openpopup('approval', '', '../Common/CommonApproval.aspx?docid=" + lblOrderId.Text.ToString() + "&mod=" + PhoenixModule.ACCOUNTS + "&directpoapproval=1"
                                            + "&type=" + type + "&user=" + drv["FLDTECHDIRECTOR"].ToString() + "," + drv["FLDFLEETMANAGER"].ToString() + "," + drv["FLDSUPT"].ToString() + "');return false;");
                    }
                }
            }
            if (!SessionUtil.CanAccess(this.ViewState, cmdPOApprove.CommandName)) cmdPOApprove.Visible = false;
        }
    }
    protected void gvDPO_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvDPO_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvDPO.SelectedIndexes.Add(e.NewSelectedIndex);
        ViewState["ORDERID"] = ((RadLabel)gvDPO.Items[e.NewSelectedIndex].FindControl("lblOrderId"));
        Rebind();
    }
    protected void Rebind()
    {
        gvDPO.SelectedIndexes.Clear();
        gvDPO.EditIndexes.Clear();
        gvDPO.DataSource = null;
        gvDPO.Rebind();
    }
    protected void gvDPO_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDPO.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDPO_ItemCommand(object sender, GridCommandEventArgs e)
    {
        
        //int iRowCount = 0;
        //int iTotalPageCount = 0;
        //DataSet dsAttachment = new DataSet();
        //GridView _gridView = (GridView)sender;
        // int nCurrentRow = int.Parse(e.CommandArgument.ToString());
        //DataSet dsInvoice = PhoenixAccountsInvoice.InvoiceEdit(new Guid(ViewState["INVOICECODE"].ToString()));
        //RadLabel lblPOType = (RadLabel)e.Item.FindControl("lblPOType");
        //  if (lblPOType != null && lblPOType.Text != "3")
        //  {
        //      if (dsInvoice.Tables.Count > 0)
        //      {
        //          DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
        //          string dtKey = drInvoice["FLDDTKEY"].ToString();
        //          ViewState["InvoiceType"] = drInvoice["FLDINVOICETYPE"].ToString();
        //          dsAttachment = PhoenixCommonFileAttachment.AttachmentSearch(new Guid(dtKey), null, "MST Acknowledgement", null, null, 1, General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
        //          if (dsAttachment == null || dsAttachment.Tables[0].Rows.Count <= 0)
        //          {
        //              ucError.ErrorMessage = "Attachment is required of MST acknowledgement type to approve the Direct PO";
        //              ucError.Visible = true;
        //              return;
        //          }
        //      }
        //  }
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            ViewState["ORDERID"] = ((RadLabel)e.Item.FindControl("lblOrderId")).Text;
            int iRowno;
            iRowno = e.Item.ItemIndex;
            Rebind();
        }
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        if (e.CommandName.ToUpper().Equals("POAPPROVE"))
        {
            try
            {
                RadLabel lblOrderId = (RadLabel)e.Item.FindControl("lblOrderId");
                if (lblOrderId != null)
                {
                    PhoenixAccountsInvoice.InvoiceDirectPOInvoiceLineItemInsert(new Guid(lblOrderId.Text));
                    ucStatus.Text = "PO is approved and tagged to invoice successfully.";
                    gvDPO.Rebind();
                }
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
            //string strOrderId = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOrderId")).Text;
            //UpdateApprovalStatus(strOrderId);            
        }
      
    }


    protected void SetTabHighlight()
    {
        try
        {
            // DataList dl = (DataList)MenuDPO.FindControl("dlstTabs");
            // if (dl.Items.Count > 0)
            {
                if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("AccountsInvoiceDirectPOGeneral.aspx"))
                {
                    MenuDPO.SelectedMenuIndex = 2;
                }
                else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("AccountsInvoiceDirectPOLineItem.aspx"))
                {
                    MenuDPO.SelectedMenuIndex = 1;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["ORDERID"] = null;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
