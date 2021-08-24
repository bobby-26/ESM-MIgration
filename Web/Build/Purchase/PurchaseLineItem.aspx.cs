using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class PurchaseLineItem : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["orderid"] != null)
                {
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                }
                else
                {
                    ViewState["orderid"] = "";
                }
                if(Request.QueryString["VesselId"] !=null)
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

                if (ViewState["orderid"] != null && ViewState["orderid"].ToString() != "")
                {
                    DataSet ds = new DataSet();
                    ds = PhoenixPurchaseOrderForm.EditOrderForm(new Guid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        NameValueCollection criteria = new NameValueCollection();
                        criteria.Clear();
                        criteria.Add("ucVessel", ViewState["VesselId"].ToString());
                        criteria.Add("ddlStockType", ViewState["StockType"].ToString());
                        criteria.Add("txtNumber", ds.Tables[0].Rows[0]["FLDFORMNO"].ToString());
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
                        Response.Redirect("../Purchase/PurchaseFormItemDetails.aspx?orderid=" + ViewState["orderid"].ToString() + "&pageno=0&COMPONENTID=");
                    }
                }
                 

                   


                ucReciptstatus.QuickTypeCode = ((int)PhoenixQuickTypeCode.RECEIPTSTATUS).ToString();
                PhoenixToolbar toolbargrid = new PhoenixToolbar();
                toolbargrid.AddImageButton("../Purchase/PurchaseLineItem.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbargrid.AddImageLink("javascript:CallPrint('gvLineItem')", "Print Grid", "icon_print.png", "Print");
                if (ViewState["StockType"].ToString().ToUpper().Equals("SERVICE"))
                    toolbargrid.AddImageLink("../Purchase/PurchaseOrderLineItemServiceSelect.aspx?orderid=" + ViewState["orderid"], "Add", "add.png", "ADD");
                else
                    toolbargrid.AddImageLink("../Purchase/PurchaseOrderLineItemSelect.aspx?orderid=" + ViewState["orderid"], "Add", "add.png", "ADD");
                MenuOrderLineItem.AccessRights = this.ViewState;
                MenuOrderLineItem.MenuList = toolbargrid.Show();
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE");
                //toolbar.AddButton("Back", "BACK");
                MenuLineItem.AccessRights = this.ViewState;
                MenuLineItem.MenuList = toolbar.Show();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                //ViewState["pageno"] = Request.QueryString["pageno"].ToString();
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOrderLineItem_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns={"FLDSERIALNO", "FLDPARTNUMBER", "FLDMAKERREF","FLDDRAWINGNO", "FLDNAME", "FLDROBQUANTITY", "FLDREQUESTEDQUANTITY",
                                 "FLDUNITNAME", "FLDORDEREDQUANTITY", "FLDQUICKNAME", "FLDCHKREMARKS" };
        string[] alCaptions= {"S. No.", "Number", "Maker Reference","Drawing No", "Name", "ROB", "Requested Qty",
                                 "Unit", "Order Qty", "Receipt Status", "Received Remarks" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        ds = PhoenixPurchaseSearch.OrderLineItemSearch(
            General.GetNullableGuid(ViewState["orderid"].ToString()), Convert.ToInt32(ViewState["VesselId"]),
            sortexpression, sortdirection, 1, iRowCount, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=FormLineItem.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><h3>Order Line Item</h3></td>");
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
                //Response.Write((alColumns[i] == "FLDNOTES") ? RemoveHTMLTags(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns={ "FLDSERIALNO", "FLDPARTNUMBER", "FLDMAKERREF","FLDDRAWINGNO", "FLDNAME", "FLDROBQUANTITY", "FLDREQUESTEDQUANTITY","FLDUNITNAME", "FLDORDEREDQUANTITY", "FLDQUICKNAME", "FLDCHKREMARKS" };
        string[] alCaptions= { "S. No.", "Number", "Maker Reference", "Drawing No", "Name", "ROB", "Requested Qty", "Unit", "Order Qty", "Receipt Status", "Received Remarks" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPurchaseSearch.OrderLineItemSearch(
            General.GetNullableGuid(ViewState["orderid"].ToString()), Convert.ToInt32(ViewState["VesselId"]),
            sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvLineItem.DataSource = ds.Tables[0];
            gvLineItem.DataBind();

            if (ViewState["orderlineid"] == null)
            {
                DataTable dt = ds.Tables[0];
                ViewState["orderlineid"] = dt.Rows[0]["FLDORDERLINEID"].ToString();
                ViewState["DTKEY"] = dt.Rows[0]["FLDDTKEY"].ToString();
                txtNumber.Text = dt.Rows[0]["FLDPARTNUMBER"].ToString();
                txtStatus.Text = dt.Rows[0]["FLDHARDNAME"].ToString();
                txtMakerReference.Text = dt.Rows[0]["FLDMAKERREF"].ToString();
                txtPartName.Text = dt.Rows[0]["FLDNAME"].ToString();
                ucReciptstatus.SelectedQuick = dt.Rows[0]["FLDRECEIPTSTATUSID"].ToString();
                txtPrice.Text = dt.Rows[0]["FLDPRICE"].ToString();
                txtRequestedQty.Text = dt.Rows[0]["FLDREQUESTEDQUANTITY"].ToString();
                txtOrderQty.Text = dt.Rows[0]["FLDORDEREDQUANTITY"].ToString();
                ucUnit.SelectedValue = dt.Rows[0]["FLDUNITID"].ToString();
                txtRecivedQty.Text = dt.Rows[0]["FLDRECEIVEDQUANTITY"].ToString();
                txtCanceledQty.Text = dt.Rows[0]["FLDCANCELLEDQUANTITY"].ToString();
                txtFormNumber.Text = dt.Rows[0]["FLDFORMNO"].ToString();
                txtFormName.Text = dt.Rows[0]["FLDTITLE"].ToString();
                gvLineItem.SelectedIndex = 0;
                SetRowSelection();
            }
        }
        else
        {
            ShowNoRecordsFound(ds.Tables[0], gvLineItem);
            ViewState["PAGEURL"] = "../Purchase/PurchaseLineItem.aspx";
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvLineItem", "Order Line Item", alCaptions, alColumns, ds);
        SetPageNavigator();
    }

    private void SetRowSelection()
    {
        gvLineItem.SelectedIndex = -1;
        for (int i = 0; i < gvLineItem.Rows.Count; i++)
        {
            if (gvLineItem.DataKeys[i].Value.ToString().Equals(ViewState["orderlineid"].ToString()))
            {
                gvLineItem.SelectedIndex = i;
                PhoenixPurchaseOrderLine.OrderLinePartNumber = ((Label)gvLineItem.Rows[i].FindControl("lblNumber")).Text;

                ViewState["DTKEY"] = ((Label)gvLineItem.Rows[gvLineItem.SelectedIndex].FindControl("lbldtkey")).Text;
            }
        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {

        ImageButton ib = (ImageButton)sender;
        try
        {
            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvLineItem.SelectedIndex = -1;
        gvLineItem.EditIndex = -1;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvLineItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        
    }

    protected void gvLineItem_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int rowIndex = int.Parse(e.CommandArgument.ToString());
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixPurchaseLineItem.OrderLineItemDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,new Guid(_gridView.DataKeys[rowIndex].Value.ToString()));
                gvLineItem.SelectedIndex = -1;
                ViewState["DTKEY"] = null;
                txtNumber.Text="";
                txtStatus.Text = "";
                txtMakerReference.Text = "";
                txtPartName.Text = "";
                ucReciptstatus.SelectedQuick = "";
                txtPrice.Text = "";
                txtRequestedQty.Text = "";
                txtOrderQty.Text = "";
                ucUnit.SelectedValue = "0";
                txtRecivedQty.Text = "";
                txtCanceledQty.Text = "";
                ViewState["orderlineid"] = null;
            }
            GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
            if (e.CommandName.ToUpper().Equals("DETAILS"))
            {
                string OrderLineItem = _gridView.DataKeys[rowIndex].Value.ToString();
                String scriptpopup = String.Format("javascript:Openpopup('Details', '', '../Purchase/PurchaseFormItemComment.aspx?orderid=" + e.CommandArgument.ToString() + "&orderlineid="
                    + OrderLineItem + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (e.CommandName.ToUpper().Equals("ATTACHMENTS") || e.CommandName.ToUpper().Equals("NOATTACHMENT"))
            {
                string dtkey = ((Label)row.FindControl("lbldtkey")).Text;
                String scriptpopup = String.Format("javascript:Openpopup('Details', '', '../Common/CommonFileAttachment.aspx?DTKEY=" + dtkey + "&MOD=PURCHASE');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvLineItem_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            DataSet ds = PhoenixPurchaseLineItem.OrderLineItemEdit(new Guid(_gridView.DataKeys[de.NewEditIndex].Value.ToString()));
            DataTable dt = new DataTable();
            dt = ds.Tables[0];

            txtNumber.Text = dt.Rows[0]["FLDPARTNUMBER"].ToString();
            txtStatus.Text = dt.Rows[0]["FLDHARDNAME"].ToString();
            txtMakerReference.Text = dt.Rows[0]["FLDMAKERREF"].ToString();
            txtPartName.Text = dt.Rows[0]["FLDNAME"].ToString();
            ucReciptstatus.SelectedQuick = dt.Rows[0]["FLDRECEIPTSTATUSID"].ToString();
            txtPrice.Text = dt.Rows[0]["FLDPRICE"].ToString();
            txtRequestedQty.Text = dt.Rows[0]["FLDREQUESTEDQUANTITY"].ToString();
            txtOrderQty.Text = dt.Rows[0]["FLDORDEREDQUANTITY"].ToString();
            ucUnit.SelectedValue = dt.Rows[0]["FLDUNITID"].ToString();
            txtRecivedQty.Text = dt.Rows[0]["FLDRECEIVEDQUANTITY"].ToString();
            txtCanceledQty.Text = dt.Rows[0]["FLDCANCELLEDQUANTITY"].ToString();
            ViewState["orderlineid"]= _gridView.DataKeys[de.NewEditIndex].Value.ToString();
            BindData();
            SetRowSelection();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;

        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvLineItem_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            {
                lbm.Text = "Product Code";
                gvLineItem.Columns[5].Visible = false;
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton cmddelete = (ImageButton)e.Row.FindControl("cmdDelete");
            if (cmddelete != null)
            {
                cmddelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                cmddelete.Visible = SessionUtil.CanAccess(this.ViewState, cmddelete.CommandName);
            }
            ImageButton db = (ImageButton)e.Row.FindControl("cmdEdit");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }


            Label lblcomponentName = (Label)e.Row.FindControl("lblComponentName");

            if (Filter.CurrentPurchaseStockType.Equals("STORE"))
            {
                lblcomponentName.Visible = false;
            }

            DataRowView drv = (DataRowView)e.Row.DataItem;
            ImageButton cmdAudit = (ImageButton)e.Row.FindControl("cmdAudit");
            if (cmdAudit != null)
            {
                cmdAudit.Visible = SessionUtil.CanAccess(this.ViewState, cmdAudit.CommandName);
                if (Filter.CurrentPurchaseStockType.ToUpper().Equals("STORE"))
                    Response.Redirect("../Common/CommonPhoenixAuditChanges.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&shortcode=PUR-FRMSTORE-LINE");
                else
                    Response.Redirect("../Common/CommonPhoenixAuditChanges.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&shortcode=PUR-FRM-LINE");
            }
            
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

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        try
        {
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvLineItem.SelectedIndex = -1;
            gvLineItem.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

    public static string RemoveHTMLTags(string source)
    {
        HtmlGenericControl htmlDiv = new HtmlGenericControl("div");
        htmlDiv.InnerHtml = source;
        String plainText = htmlDiv.InnerText;

        return plainText;
    }

    private bool IsValidForm()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtPartName.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Part name is required.";
        if (ViewState["orderid"] == null)
            ucError.ErrorMessage = "Order number is required.";
        if (ucUnit.SelectedUnit.ToUpper().Trim() == "DUMMY" || ucUnit.SelectedUnit.Trim() == "")
            ucError.ErrorMessage = "Unit is Required.";
        return (!ucError.IsError);
    }

    protected void MenuLineItem_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidForm())
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPurchaseLineItem.OrderLineItemUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , new Guid(ViewState["orderlineid"].ToString())
                                                    , General.GetNullableInteger(ucUnit.SelectedUnit)
                                                    , General.GetNullableDecimal(txtOrderQty.Text)
                                                    , General.GetNullableInteger(ucReciptstatus.SelectedQuick));
                BindData();
            }
            if(dce.CommandName.ToUpper().Equals("BACK"))
            {
                if (Request.QueryString["pagecode"] == "code1")
                {
                    Response.Redirect("../Purchase/PurchaseNewRequisition.aspx");
                }
                if (Request.QueryString["pagecode"] == "code2")
                {
                    Response.Redirect("../Purchase/PurchaseAwaitingQuotes.aspx");
                }
                if (Request.QueryString["pagecode"] == "code3")
                {
                    Response.Redirect("../Purchase/PurchaseAwaitingPOApproval.aspx");
                }
                if (Request.QueryString["pagecode"] == "code4")
                {
                    Response.Redirect("../Purchase/PurchasePOToBeIssued.aspx");
                }
                if (Request.QueryString["pagecode"] == "code5")
                {
                    Response.Redirect("../Purchase/PurchasePOIssued.aspx");
                }
                if (Request.QueryString["pagecode"] == "code6")
                {
                    Response.Redirect("../Purchase/PurchasePOFiled.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }
}
