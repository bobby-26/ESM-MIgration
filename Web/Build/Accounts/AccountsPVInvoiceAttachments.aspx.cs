using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized; 

public partial class AccountsPVInvoiceAttachments : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            Page.ClientScript.RegisterStartupScript(typeof(Page), "scroller", "<script>scrollToVal('divScroll', 'hdnScroll');</script>");
            if (!IsPostBack)
            {
                //PhoenixToolbar toolbar = new PhoenixToolbar();

                //toolbar.AddButton("General", "GENERAL");
                //toolbar.AddButton("Line Item", "LINEITEM");
                //toolbar.AddButton("History", "HISTORY");
                //MenuDPO.AccessRights = this.ViewState;
                //MenuDPO.MenuList = toolbar.Show();
                //MenuDPO.SelectedMenuIndex = 0;

                //Title1.Text = "Direct PO";
                ////ifMoreInfo.Attributes["src"] = "AccountsInvoiceDirectPurchaseOrderGeneral.aspx?qinvoicecode=" + ViewState["INVOICECODE"];
                //ViewState["VESSELID"] = null;
                //ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                //ViewState["ORDERID"] = null;

                //DateTime now = DateTime.Now;

                //string FromDate = now.Date.AddMonths(-1).ToShortDateString();
                //string ToDate = DateTime.Now.ToShortDateString();

                //ViewState["FROMDATE"] = FromDate.ToString();
                //ViewState["TODATE"] = ToDate.ToString();

                //PhoenixToolbar toolbargrid = new PhoenixToolbar();
                //toolbargrid.AddImageButton("../Accounts/AccountsInvoiceDirectPurchaseOrder.aspx", "Export to Excel", "icon_xls.png", "Excel");
                //toolbargrid.AddImageLink("javascript:CallPrint('gvDPO')", "Print Grid", "icon_print.png", "PRINT");
                //toolbargrid.AddImageButton("../Accounts/AccountsDirectPOFilter.aspx", "Find", "search.png", "FIND");
                ////toolbargrid.AddImageLink("javascript:parent.Openpopup('codehelp1','','AccountsAmosInvoiceLineItem.aspx?qinvoicecode=" + ViewState["INVOICECODE"].ToString() + "'); return false;", "Add", "Add.png", "ADD");
                //MenuAddAmosPO.AccessRights = this.ViewState;
                //MenuAddAmosPO.MenuList = toolbargrid.Show();

                if (Request.QueryString["voucherid"] != null)
                {
                    ViewState["PaymentVoucherID"] = Request.QueryString["voucherid"].ToString();
                }
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void MenuDPO_TabStripCommand(object sender, EventArgs e)
    //{
    //    DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
    //     try
    //       {
    //           if (dce.CommandName.ToUpper().Equals("GENERAL"))
    //           {
    //               ViewState["SETCURRENTNAVIGATIONTAB"] = null;
    //               ViewState["SETCURRENTNAVIGATIONTAB"] = "AccountsInvoiceDirectPurchaseOrderGeneral.aspx";
    //               ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"] + "?qinvoicecode=" + ViewState["INVOICECODE"] + (ViewState["ORDERID"] != null ? "&orderid=" + ViewState["ORDERID"] + "&vslid=" + ViewState["VESSELID"] : string.Empty);
    //           }
    //           else if (dce.CommandName.ToUpper().Equals("LINEITEM") && ViewState["ORDERID"] != null)
    //           {
    //               ViewState["SETCURRENTNAVIGATIONTAB"] = null;
    //               ViewState["SETCURRENTNAVIGATIONTAB"] = "AccountsInvoiceDirectPurchaseOrderLineItem.aspx";
    //               ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"] + "?qinvoicecode=" + ViewState["INVOICECODE"] + (ViewState["ORDERID"] != null ? "&orderid=" + ViewState["ORDERID"] + "&vslid=" + ViewState["VESSELID"] : string.Empty);
    //           }
    //           else if (dce.CommandName.ToUpper().Equals("HISTORY") && ViewState["ORDERID"] != null)
    //           {
    //               ViewState["SETCURRENTNAVIGATIONTAB"] = null;
    //               ViewState["SETCURRENTNAVIGATIONTAB"] = "AccountsInvoiceDirectPurchaseHistory.aspx";
    //               ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"] + "?orderid=" + ViewState["ORDERID"];
    //           }
    //           else
    //               MenuDPO.SelectedMenuIndex = 0;
    //       }
    //       catch (Exception ex)
    //       {
    //           ucError.ErrorMessage = ex.Message;
    //           ucError.Visible = true;
    //       }
        
    //}
    //protected void MenuAddAmosPO_TabStripCommand(object sender, EventArgs e)
    //{
    //    DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
    //    try
    //    {

    //        if (dce.CommandName.ToUpper().Equals("EXCEL"))
    //        {
    //            ShowExcel();
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
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
                                "Po Status",
                                "Amount",
                                "Description",
                                "Invoice Number",
                                "Invoice Status"
                              };

            string[] alColumns = {  "FLDFORMNO",
                                "FLDNAME",
                                "FLDVESSELNAME",
                                "FLDCURRENCYNAME", 
                                "FLDRECEIVEDDATE", 
                                "FLDPOSTATUS",
                                "FLDESTIMATEAMOUNT",
                                "FLDDESCRIPTION",
                                "FLDINVOICENUMBER",
                                "FLDHARDNAME"
                             };
            

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            //NameValueCollection nvc = Filter.CurrentDirectPOSelection;

            //DataTable dt = PhoenixAccountsInvoice.InvoiceDirectPurchaseOrderFilterSearch(null
            //    , nvc != null ? General.GetNullableInteger(nvc.Get("ddlInvoiceType")) : null
            //    , nvc != null ? General.GetNullableString(nvc.Get("txtOrderNumber")) : null
            //    , nvc != null ? General.GetNullableInteger(nvc.Get("txtVendorId")) : null
            //    , nvc != null ? General.GetNullableInteger(nvc.Get("ddlCurrencyCode")) : null
            //    , nvc != null ? General.GetNullableString(nvc.Get("ucVessel")) : null
            //    , nvc != null ? General.GetNullableString(nvc.Get("txtReceivedFromdateSearch").ToString().Trim()) : ViewState["FROMDATE"].ToString()
            //    , nvc != null ? General.GetNullableString(nvc.Get("txtReceivedTodateSearch").ToString().Trim()) : ViewState["TODATE"].ToString()
            //    , nvc != null ? General.GetNullableInteger(nvc.Get("ddlStatus")) : null
            //    , sortexpression, sortdirection
            //    , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
            //    , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount
            //    , nvc != null ? General.GetNullableInteger(nvc.Get("ucPortMulti")) : null
            //    , nvc != null ? General.GetNullableDateTime(nvc.Get("ucETAFrom")) : null
            //    , nvc != null ? General.GetNullableDateTime(nvc.Get("ucETATO")) : null
            //    , nvc != null ? General.GetNullableDateTime(nvc.Get("ucETDFrom")) : null
            //    , nvc != null ? General.GetNullableDateTime(nvc.Get("ucETDTO")) : null
            //    , nvc != null ? General.GetNullableString(nvc.Get("txtDescription")) : null
            //    );

            DataSet ds = new DataSet();
            //ds.Tables.Add(dt.Copy());

            ds = PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherInvoiceListAttachments(new Guid(ViewState["PaymentVoucherID"].ToString()), Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                                                                            , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);


            General.SetPrintOptions("gvDPO", "Direct PO", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDPO.DataSource = ds;
                gvDPO.DataBind();                
            }
            else
            {
                ShowNoRecordsFound(ds.Tables[0], gvDPO);                
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            SetPageNavigator();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void ShowExcel()
    //{
    //    DataSet ds = new DataSet();
    //    int iRowCount = 0;
    //    int iTotalPageCount = 0;

    //    string[] alCaptions = {                                 
    //                            "PO Number",
    //                            "Vendor",
    //                            "Vessel",                             
    //                            "Currency",
    //                            "PO Received Date",   
    //                            "Po Status",
    //                            "Amount",
    //                            "Description",
    //                            "Invoice Number",
    //                            "Invoice Status"
    //                          };

    //    string[] alColumns = {  "FLDFORMNO",
    //                            "FLDNAME",
    //                            "FLDVESSELNAME",
    //                            "FLDCURRENCYNAME", 
    //                            "FLDRECEIVEDDATE", 
    //                            "FLDPOSTATUS",
    //                            "FLDESTIMATEAMOUNT",
    //                            "FLDDESCRIPTION",
    //                            "FLDINVOICENUMBER",
    //                            "FLDHARDNAME"
    //                         };

    //    string sortexpression;
    //    int? sortdirection = null;

    //    sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
    //    if (ViewState["SORTDIRECTION"] != null)
    //        sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

    //    if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
    //        iRowCount = 10;
    //    else
    //        iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

    //    NameValueCollection nvc = Filter.CurrentDirectPOSelection;

    //    DataTable dt = PhoenixAccountsInvoice.InvoiceDirectPurchaseOrderFilterSearch(null
    //            , nvc != null ? General.GetNullableInteger(nvc.Get("ddlInvoiceType")) : null
    //            , nvc != null ? General.GetNullableString(nvc.Get("txtOrderNumber")) : null
    //            , nvc != null ? General.GetNullableInteger(nvc.Get("txtVendorId")) : null
    //            , nvc != null ? General.GetNullableInteger(nvc.Get("ddlCurrencyCode")) : null
    //            , nvc != null ? General.GetNullableString(nvc.Get("ucVessel")) : null
    //            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtReceivedFromdateSearch")) : General.GetNullableDateTime(ViewState["FROMDATE"].ToString())
    //            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtReceivedTodateSearch")) : General.GetNullableDateTime(ViewState["TODATE"].ToString())
    //            , nvc != null ? General.GetNullableInteger(nvc.Get("ddlStatus")) : null
    //            , sortexpression, sortdirection
    //            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
    //            , iRowCount, ref iRowCount, ref iTotalPageCount
    //            , nvc != null ? General.GetNullableInteger(nvc.Get("ucPortMulti")) : null
    //            , nvc != null ? General.GetNullableDateTime(nvc.Get("ucETAFrom")) : null
    //            , nvc != null ? General.GetNullableDateTime(nvc.Get("ucETATO")) : null
    //            , nvc != null ? General.GetNullableDateTime(nvc.Get("ucETDFrom")) : null
    //            , nvc != null ? General.GetNullableDateTime(nvc.Get("ucETDTO")) : null
    //            , nvc != null ? General.GetNullableString(nvc.Get("txtDescription")) : null
    //            );

    //    ds.Tables.Add(dt.Copy());

    //    Response.AddHeader("Content-Disposition", "attachment; filename=DirectPO.xls");

    //    Response.ContentType = "application/vnd.msexcel";
    //    Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
    //    Response.Write("<tr>");
    //    Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
    //    Response.Write("<td><h3>Direct PO</h3></td>");
    //    Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
    //    Response.Write("</tr>");
    //    Response.Write("</TABLE>");
    //    Response.Write("<br />");
    //    Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
    //    Response.Write("<tr>");
    //    for (int i = 0; i < alCaptions.Length; i++)
    //    {
    //        Response.Write("<td width='20%'>");
    //        Response.Write("<b>" + alCaptions[i] + "</b>");
    //        Response.Write("</td>");
    //    }
    //    Response.Write("</tr>");
    //    foreach (DataRow dr in ds.Tables[0].Rows)
    //    {
    //        Response.Write("<tr>");
    //        for (int i = 0; i < alColumns.Length; i++)
    //        {
    //            Response.Write("<td>");
    //            Response.Write(dr[alColumns[i]]);
    //            Response.Write("</td>");
    //        }
    //        Response.Write("</tr>");
    //    }
    //    Response.Write("</TABLE>");
    //    Response.End();
    //}

    protected void gvDPO_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        try
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
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    Label lblIsPoTaggedToInvoice = (Label)e.Row.FindControl("lblIsPoTaggedToInvoice");
            //    Label lblOrderId = (Label)e.Row.FindControl("lblOrderId");
            //    ImageButton cmdPOApprove = (ImageButton)e.Row.FindControl("cmdPOApprove");
            //    Label lblVendorId = (Label)e.Row.FindControl("lblVendorId");
            //}
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblFileName = ((Label)e.Row.FindControl("lblFileName"));
                Image imgtype = (Image)e.Row.FindControl("imgfiletype");
                if (lblFileName.Text != string.Empty)
                {
                    imgtype.ImageUrl = ResolveImageType(lblFileName.Text.Substring(lblFileName.Text.LastIndexOf('.')));

                    Label lblFilePath = (Label)e.Row.FindControl("lblFilePath");
                    HyperLink lnk = (HyperLink)e.Row.FindControl("lnkfilename");
                    lnk.NavigateUrl = "../common/download.aspx?dtkey=" + drv["FLDDTKEY"].ToString(); //Session["sitepath"] + "/attachments/" + lblFilePath.Text;                
                    //lnk.NavigateUrl = Session["sitepath"] + "/attachments/" + lblFilePath.Text;                    
                }                
                Label lblAttachmenttype = (Label)e.Row.FindControl("lblAttachmenttype");
                lblAttachmenttype.Visible = true;
                imgtype.Visible = false;
                
                //Label lblDTKey = (Label)e.Row.FindControl("lblInvoiceRef");
                //ImageButton lnkInvoiceNumber = (ImageButton)e.Row.FindControl("cmdAttachment");
                //if (lnkInvoiceNumber != null)
                //{
                //    lnkInvoiceNumber.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../Accounts/AccountsFileAttachment.aspx?DTKEY=" + lblDTKey.Text + "&MOD=" + PhoenixModule.ACCOUNTS + "&U=1" + "');return false;");
                //    if (!SessionUtil.CanAccess(this.ViewState, lnkInvoiceNumber.CommandName)) lnkInvoiceNumber.Visible = false;
                //}
                //    ImageButton cmdAttachment = (ImageButton)e.Row.FindControl("cmdAttachment");
                //    if (cmdAttachment != null)
                //    {
                //        if (attachmentFlag == 1)
                //        {
                //            cmdAttachment.Attributes.Add("onclick", "Openpopup('codehelp','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                //                                + PhoenixModule.ACCOUNTS + "&U=" + attachmentFlag.ToString() + "');return true;");
                //        }
                //        else
                //        {
                //            cmdAttachment.Attributes.Add("onclick", "Openpopup('codehelp','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                //                                + PhoenixModule.ACCOUNTS + "');return true;");
                //        }
                //        cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
                //    }
                //    ImageButton cmdNoAttachment = (ImageButton)e.Row.FindControl("cmdNoAttachment");
                //    if (cmdNoAttachment != null)
                //    {
                //        if (attachmentFlag == 1)
                //        {
                //            cmdNoAttachment.Attributes.Add("onclick", "Openpopup('codehelp','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                //                                + PhoenixModule.ACCOUNTS + "');return true;");
                //            cmdNoAttachment.Attributes.Add("onclick", "Openpopup('codehelp','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                //                                + PhoenixModule.ACCOUNTS + "&U=" + attachmentFlag.ToString() + "');return true;");
                //        }
                //        else
                //        {
                //            cmdNoAttachment.Attributes.Add("onclick", "Openpopup('codehelp','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                //                                + PhoenixModule.ACCOUNTS + "');return true;");
                //        }
                //        cmdNoAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdNoAttachment.CommandName);
            }


            //    ImageButton iab = (ImageButton)e.Row.FindControl("cmdAttachment");
            //    ImageButton inab = (ImageButton)e.Row.FindControl("cmdNoAttachment");
            //    if (iab != null) iab.Visible = true;
            //    if (inab != null) inab.Visible = false;
            //    int? n = General.GetNullableInteger(drv["FLDATTACHMENTCOUNT"].ToString());
            //    if (n == 0)
            //    {
            //        if (iab != null) iab.Visible = false;
            //        if (inab != null) inab.Visible = true;
            //    }
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }


    }
    protected void gvDPO_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        ViewState["ORDERID"] = null;
        BindData();
    }
    protected void gvDPO_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        GridView _gridView = sender as GridView;
        _gridView.SelectedIndex = se.NewSelectedIndex;
        string orderid = _gridView.DataKeys[se.NewSelectedIndex].Value.ToString();
        ViewState["ORDERID"] = orderid;

        BindData();
    }
    protected void gvDPO_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            //ViewState["ORDERID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblOrderId")).Text;
            //ViewState["VESSELID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselId")).Text;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void SetTabHighlight()
    //{
    //    try
    //    {
    //        DataList dl = (DataList)MenuDPO.FindControl("dlstTabs");
    //        if (dl.Items.Count > 0)
    //        {
    //            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("AccountsInvoiceDirectPurchaseOrderGeneral.aspx"))
    //            {
    //                MenuDPO.SelectedMenuIndex = 0;
    //            }
    //            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("AccountsInvoiceDirectPurchaseOrderLineItem.aspx"))
    //            {
    //                MenuDPO.SelectedMenuIndex = 1;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
        ViewState["ORDERID"] = null;
        BindData();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvDPO.SelectedIndex = -1;
        gvDPO.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;
        ViewState["ORDERID"] = null;
        BindData();
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
        {
            return true;
        }

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

    private string ResolveImageType(string extn)
    {
        string imagepath = Session["images"] + "/";
        if (string.IsNullOrEmpty(extn)) extn = string.Empty;
        switch (extn.ToLower())
        {
            case ".jpg":
            case ".png":
            case ".gif":
            case ".bmp":
                imagepath += "imagefile.png";
                break;
            case ".doc":
                imagepath += "word.png";
                break;
            case ".xls":
                imagepath += "xls.png";
                break;
            case ".pdf":
                imagepath += "pdf.png";
                break;
            case ".rar":
            case ".zip":
                imagepath += "rar.png";
                break;
            case ".txt":
                imagepath += "notepad.png";
                break;
            default:
                imagepath += "anyfile.png";
                break;
        }
        return imagepath;
    }

}
