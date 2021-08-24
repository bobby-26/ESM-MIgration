using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class PurchaseQuotationLineItem : PhoenixBasePage
{
    public string strTotalAmount = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                PhoenixToolbar toolSave = new PhoenixToolbar();
                toolSave.AddButton("Save", "SAVE");
                toolSave.AddButton("Back", "BACK");
                //if (Filter.CurrentPurchaseStockType != null && Filter.CurrentPurchaseStockType == "STORE")
                //{
                //    toolSave.AddButton("Show Contract Price", "CONTRACTPRICE");
                //}
                MenuSaveDetails.MenuList = toolSave.Show();

                Title1.Text = "Quotation Lineitem [" + PhoenixPurchaseOrderForm.FormNumber + "   Vendor :   " + PhoenixPurchaseQuotation.VendorName + "     ]";
                if (Request.QueryString["quotationid"] != null)
                {
                    ViewState["quotationid"] = Request.QueryString["quotationid"].ToString();
                }
                else
                {
                    ViewState["quotationid"] = "";
                }
                if (Request.QueryString["orderid"] != null)
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();


                PhoenixToolbar toolbargrid = new PhoenixToolbar();
                toolbargrid.AddImageButton("../Purchase/PurchaseQuotationLineItem.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbargrid.AddImageLink("javascript:CallPrint('gvVendorItem')", "Print Grid", "icon_print.png", "Print");
                toolbargrid.AddImageButton("javascript:Openpopup('Report','',' ../Reports/ReportsViewWithSubReport.aspx?applicationcode=3&reportcode=VENDORQUOTATION&quotationid=" + ViewState["quotationid"].ToString() + "&orderid=" + ViewState["orderid"].ToString() + "'); return false;", "RFQ", "task-list.png", "ORDER");
                toolbargrid.AddImageLink("../Purchase/PurchaseQuotationLineItemSaveBulk.aspx?quotationid=" + ViewState["quotationid"].ToString() + "&orderid=" + ViewState["orderid"].ToString(), "Save", "bulk_save.png", "BULKSAVE");
                MenuRegistersStockItem.AccessRights = this.ViewState;
                MenuRegistersStockItem.MenuList = toolbargrid.Show();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                //ViewState["pageno"] = Request.QueryString["pageno"].ToString();
                BindVendorInfo();
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

    private void BindVendorInfo()
    {
        if (ViewState["quotationid"].ToString() != "")
        {
            DataSet dsVendor = PhoenixPurchaseQuotation.EditQuotation(new Guid(ViewState["quotationid"].ToString()));
            if (dsVendor.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsVendor.Tables[0].Rows[0];
                //ucCurrency.SelectedCurrency = dr["FLDQUOTEDCURRENCYID"].ToString();
                txtCurrency.Text = dr["FLDCURRENCYCODE"].ToString();
                ViewState["vendorid"] = dr["FLDVENDORID"].ToString();
            }
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns;
        string[] alCaptions;

        if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SPARE") || Filter.CurrentPurchaseStockType.ToUpper().Equals("SERVICE"))
        {
            alColumns = new string[12] {"FLDROWNUMBER","FLDNUMBER", "FLDNAME", "FLDMAKERREFERENCE","FLDROBQUANTITY", "FLDORDEREDQUANTITY", "FLDUNITNAME",
                                 "FLDQUANTITY", "FLDQUOTEDPRICE", "FLDDISCOUNT", "TOTALPRICE", "FLDDELIVERYTIME" };
            alCaptions = new string[12] {"S.No","Part Number", "Part Name", "Maker Reference","ROB", "Quantity", "Unit",
                                 "Quoted Qty", "Price", "Discount(%)", "Total Price", "Del. Time (Days)" };
        }
        else
        {
            alColumns = new string[12] {"FLDROWNUMBER","FLDNUMBER", "FLDNAME", "FLDMAKERREFERENCE","FLDROBQUANTITY", "FLDORDEREDQUANTITY", "FLDUNITNAME",
                                 "FLDQUANTITY", "FLDQUOTEDPRICE", "FLDDISCOUNT", "TOTALPRICE", "FLDDELIVERYTIME" };
            alCaptions = new string[12] {"S.No","Part Number", "Part Name", "Product Code","ROB", "Quantity", "Unit",
                                 "Quoted Qty", "Price", "Discount(%)", "Total Price", "Del. Time (Days)" };
        }

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixPurchaseQuotationLine.QuotationLineSearch("", ViewState["quotationid"].ToString(), sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvVendorItem.DataSource = ds;
            gvVendorItem.DataBind();
            if (ViewState["quotationlineid"] == null)
            {
                ViewState["quotationlineid"] = ds.Tables[0].Rows[0]["FLDQUOTATIONLINEID"].ToString();
                bindQuotationLine();
                gvVendorItem.SelectedIndex = 0;
            }
            strTotalAmount = String.Format("{0:0.00}", ds.Tables[0].Rows[0]["FLDTOTALAMOUNT"].ToString());
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvVendorItem);
            strTotalAmount = "0.00";
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvVendorItem", "Quotation Line item - " + PhoenixPurchaseOrderForm.FormNumber, alCaptions, alColumns, ds);
    }

    private void bindQuotationLine()
    {
        DataSet quotationlinedataset = new DataSet();
        quotationlinedataset = PhoenixPurchaseQuotationLine.EditQuotationLine(new Guid(ViewState["quotationlineid"].ToString()));
        if (quotationlinedataset.Tables[0].Rows.Count > 0)
        {
            DataRow dr = quotationlinedataset.Tables[0].Rows[0];
            txtPartName.Text = dr["FLDNAME"].ToString();
            txtPartNumber.Text = dr["FLDNUMBER"].ToString();
            txtMakerReference.Text = dr["FLDMAKERREFERENCE"].ToString();
            txtContent.Text = "Goods";
            txtStatus.Text = "Active";
            txtRemarks.Content = dr["FLDVENDORNOTES"].ToString();
            txtPosition.Text = dr["FLDPOSITION"].ToString();
            txtDrawingNo.Text = dr["FLDDRAWINGNUMBER"].ToString();
            if (Filter.CurrentPurchaseStockType.ToUpper().Equals("STORE"))
                lblMakerRef.Text = "Product Code";
            else
                lblMakerRef.Text = "Maker Ref";
        }

    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        try
        {
            GetSelectedCheckbox();

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
            SetSelectedCheckbox();
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
            GetSelectedCheckbox();

            gvVendorItem.SelectedIndex = -1;
            gvVendorItem.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;
            BindData();
            SetPageNavigator();
            SetSelectedCheckbox();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void GetSelectedCheckbox()
    {
        List<String> SelectedPvs = new List<string>(); ;
        string lineid;

        foreach (GridViewRow gvrow in gvVendorItem.Rows)
        {
            bool result = false;
            lineid = ((Label)gvrow.FindControl("lblLineid")).Text;

            if (((CheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
            {
                result = true;
            }

            if (ViewState["SelectedLineItems"] != null)
                SelectedPvs = (List<String>)ViewState["SelectedLineItems"];

            if (result)
            {
                if (!SelectedPvs.Contains(lineid))
                    SelectedPvs.Add(lineid);
            }
            else
                SelectedPvs.Remove(lineid);
        }
        if (SelectedPvs != null && SelectedPvs.Count > 0)
            ViewState["SelectedLineItems"] = SelectedPvs;
    }

    private void SetSelectedCheckbox()
    {
        if (ViewState["SelectedLineItems"] != null)
        {
            List<String> SelectedPvs = (List<String>)ViewState["SelectedLineItems"];
            string lineid;
            if (SelectedPvs != null && SelectedPvs.Count > 0)
            {
                foreach (GridViewRow row in gvVendorItem.Rows)
                {
                    CheckBox chk = (CheckBox)(row.Cells[0].FindControl("chkSelect"));
                    lineid = ((Label)row.FindControl("lblLineid")).Text;
                    if (SelectedPvs.Contains(lineid))
                    {
                        CheckBox myCheckBox = (CheckBox)row.FindControl("chkSelect");
                        myCheckBox.Checked = true;
                    }
                }
            }
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

    protected void MenuSaveDetails_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidRemark())
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateVendorNotes();
                BindData();
                SetPageNavigator();
            }

            if (dce.CommandName.ToUpper().Equals("BACK"))
            {
                if (ViewState["orderid"] != null)
                {
                    Response.Redirect("../Purchase/PurchaseQuotation.aspx?orderid=" + ViewState["orderid"].ToString());
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidRemark()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (ViewState["quotationlineid"] == null || ViewState["quotationlineid"].ToString().Trim().Equals(""))
            ucError.ErrorMessage = "Line item selection is required. ";
        return (!ucError.IsError);
    }

    private void UpdateVendorNotes()
    {
        PhoenixPurchaseQuotationLine.UpdateQuotationLine(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["quotationlineid"].ToString()), 0, txtRemarks.Content);
    }

    protected void MenuRegistersStockItem_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }
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

        string[] alColumns;
        string[] alCaptions;

        if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SPARE") || Filter.CurrentPurchaseStockType.ToUpper().Equals("SERVICE"))
        {
            alColumns = new string[15] {"FLDROWNUMBER","FLDNUMBER", "FLDMAKERNAME", "FLDNAME", "FLDMAKERREFERENCE","FLDROBQUANTITY", "FLDORDEREDQUANTITY", "FLDUNITNAME",
                                 "FLDQUANTITY", "FLDQUOTEDPRICE", "FLDDISCOUNT", "TOTALPRICE", "FLDDELIVERYTIME", "FLDNOTES", "FLDVENDORNOTES" };
            alCaptions = new string[15] {"S.No","Part Number", "Maker","Part Name", "Maker Reference","ROB", "Quantity", "Unit",
                                 "Quoted Qty", "Price", "Discount(%)", "Total Price", "Del. Time (Days)", "Vessel Details", "Vendor Remarks" };
        }
        else
        {
            alColumns = new string[15] {"FLDROWNUMBER","FLDNUMBER", "FLDMAKERNAME", "FLDNAME", "FLDMAKERREFERENCE","FLDROBQUANTITY", "FLDORDEREDQUANTITY", "FLDUNITNAME",
                                 "FLDQUANTITY", "FLDQUOTEDPRICE", "FLDDISCOUNT", "TOTALPRICE", "FLDDELIVERYTIME", "FLDNOTES", "FLDVENDORNOTES" };
            alCaptions = new string[15] {"S.No","Part Number", "Maker", "Part Name", "Product Code","ROB", "Quantity", "Unit",
                                 "Quoted Qty", "Price", "Discount(%)", "Total Price", "Del. Time (Days)", "Vessel Details", "Vendor Remarks" };
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

        ds = PhoenixPurchaseQuotationLine.QuotationLineSearch("", ViewState["quotationid"].ToString(), sortexpression, sortdirection, 1,
           iRowCount,
           ref iRowCount,
           ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=VendorLineItem - " + PhoenixPurchaseOrderForm.FormNumber + ".xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='2'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        //Response.Write("<td><img src='" + HttpContext.Current.Session["sitepath"] + "/images/esmlogo4_small.png' /></td>");
        Response.Write("<td colspan='8'><h4>Quotation Lineitem [" + PhoenixPurchaseOrderForm.FormNumber + "   Vendor :   " + PhoenixPurchaseQuotation.VendorName + "     ]</h4></td>");
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
                Response.Write((alColumns[i] == "FLDNOTES" || alColumns[i] == "FLDVENDORNOTES") ? RemoveHTMLTags(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    public static string RemoveHTMLTags(string source)
    {
        HtmlGenericControl htmlDiv = new HtmlGenericControl("div");
        htmlDiv.InnerHtml = source;
        String plainText = htmlDiv.InnerText;

        return plainText;
    }

    protected void gvVendorItem_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixPurchaseQuotationLine.DeleteQuotationLine(1, new Guid(((Label)_gridView.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblQuotationLineId")).Text));
                _gridView.EditIndex = -1;
                BindData();

            }
            else if (e.CommandName.ToUpper().Equals("CANCEL"))
            {
                _gridView.EditIndex = -1;
                BindData();

            }
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVendorItem_RowDataBound(object sender, GridViewRowEventArgs e)
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

            Label lblComponentName = (Label)e.Row.FindControl("lblComponentName");


            Label lblvesselid = (Label)e.Row.FindControl("lblVesselid");
            Label lblcomponentid = (Label)e.Row.FindControl("lblComponentId");
            ImageButton img = (ImageButton)e.Row.FindControl("imgComponentDetails");
            if (img != null) img.Attributes.Add("onclick", "javascript:return Openpopup('Component', '', 'PurchaseFormItemComponentDetails.aspx?COMPONENTID=" + lblcomponentid.Text + "&VESSELID=" + lblvesselid.Text + "'); return false;");

            if (Filter.CurrentPurchaseStockType.Equals("STORE"))
            {
                lblComponentName.Visible = false;
                if (img != null) img.Visible = false;
            }
            Label lblIsdefault = (Label)e.Row.FindControl("lblIsNotes");
            ImageButton imgFlag = (ImageButton)e.Row.FindControl("imgFlag");
            imgFlag.ImageUrl = lblIsdefault.Text.ToUpper().Equals("1") ? Session["images"] + "/detail-flag.png" : Session["images"] + "/spacer.gif";

            Label lblIsContract = (Label)e.Row.FindControl("lblIsContract");
            ImageButton imgContractFlag = (ImageButton)e.Row.FindControl("imgContractFlag");
            imgContractFlag.ImageUrl = lblIsContract.Text.ToUpper().Equals("1") ? Session["images"] + "/contract-exist.png" : Session["images"] + "/spacer.gif";

            Label item = (Label)e.Row.FindControl("lblItemid");
            UserControlPurchaseUnit unit = (UserControlPurchaseUnit)e.Row.FindControl("ucUnit");
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (unit != null)
            {
                unit.PurchaseUnitList = PhoenixRegistersUnit.ListPurchaseUnit(item.Text, Filter.CurrentPurchaseStockType
                                                                     , Filter.CurrentPurchaseVesselSelection);
                unit.SelectedUnit = drv["FLDUNITID"].ToString();
            }
            ImageButton cmdAudit = (ImageButton)e.Row.FindControl("cmdAudit");
            if (cmdAudit != null)
            {
                cmdAudit.Visible = SessionUtil.CanAccess(this.ViewState, cmdAudit.CommandName);
                if (Filter.CurrentPurchaseStockType.ToUpper().Equals("STORE"))
                    cmdAudit.Attributes.Add("onclick", "parent.Openpopup('Audit', '', '../Common/CommonPhoenixAuditChanges.aspx?dtkey=" + drv["FLDQUOTATIONLINEID"].ToString() + "&shortcode=PUR-QTNSTORE-LINE');return false;");
                else
                    cmdAudit.Attributes.Add("onclick", "parent.Openpopup('Audit', '', '../Common/CommonPhoenixAuditChanges.aspx?dtkey=" + drv["FLDQUOTATIONLINEID"].ToString() + "&shortcode=PUR-QTN-LINE');return false;");
            }

            if (General.GetNullableDecimal(drv["FLDORDEREDQUANTITY"].ToString()) != General.GetNullableDecimal(drv["FLDQUANTITY"].ToString()))
            {
                e.Row.Cells[11].BackColor = Color.Red;
            }
        }
    }

    protected void gvVendorItem_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            if (_gridView.EditIndex > -1)
                _gridView.UpdateRow(_gridView.EditIndex, false);
            _gridView.EditIndex = e.NewEditIndex;
            _gridView.SelectedIndex = e.NewEditIndex;


            ViewState["quotationlineid"] = ((Label)_gridView.Rows[e.NewEditIndex].FindControl("lblQuotationLineId")).Text;
            BindData();
            bindQuotationLine();
            SetPageNavigator();
            ((AjaxControlToolkit.HTMLEditor.Editor)(txtRemarks.FindControl("ucCustomEditor"))).AutoFocus = false;

            ((UserControlMaskNumber)_gridView.Rows[e.NewEditIndex].FindControl("txtQuotedPriceEdit")).Focus();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVendorItem_Sorting(object sender, GridViewSortEventArgs e)
    {
        gvVendorItem.SelectedIndex = -1;
        gvVendorItem.EditIndex = -1;

        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvVendorItem_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;

        if (!IsValidRate(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtQuotedPriceEdit")).Text, ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtQuantityEdit")).Text))
        {
            ucError.Visible = true;
            return;
        }
        UpdateQuotationLineItem(((Label)(_gridView.Rows[nCurrentRow].FindControl("lblQuotationLineId"))).Text,
                ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtQuantityEdit")).Text.Replace("_", "0"),
                ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtQuotedPriceEdit")).Text.Replace("_", "0")
                , ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtDiscountEdit")).Text.Replace("_", "0")
                , ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtDeliveryTimeEdit")).Text
                , ((UserControlPurchaseUnit)_gridView.Rows[nCurrentRow].FindControl("ucUnit")).SelectedUnit
                );
        _gridView.EditIndex = -1;
        BindData();
        SetPageNavigator();
    }

    private void UpdateQuotationLineItem(string quotaitionlineid, string quantity, string rate, string discount, string deliveryitem, string unitid)
    {
        try
        {
            PhoenixPurchaseQuotationLine.UpdateQuotationLine(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(quotaitionlineid), new Guid(ViewState["quotationid"].ToString()), General.GetNullableDecimal(quantity), General.GetNullableDecimal(rate)
                , General.GetNullableDecimal(discount), General.GetNullableDecimal(deliveryitem)
                , General.GetNullableInteger(unitid));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidRate(string rate, string quantity)
    {

        if (rate.Trim().Equals("") || rate == "0")
            ucError.ErrorMessage = "Item Rate is required.";
        if (quantity.Trim().Equals("") || quantity == "0")
            ucError.ErrorMessage = "Quantity  is required.";
        if (General.GetNullableGuid(ViewState["quotationid"].ToString()) == null)
            ucError.ErrorMessage = "Quotationid is required.";
        return (!ucError.IsError);
    }

    protected void gvVendorItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
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

    protected void gvVendorItem_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvVendorItem.SelectedIndex = e.NewSelectedIndex;

        ViewState["quotationlineid"] = ((Label)gvVendorItem.Rows[e.NewSelectedIndex].FindControl("lblQuotationLineId")).Text;

        bindQuotationLine();
        if (gvVendorItem.EditIndex > -1)
            gvVendorItem.UpdateRow(gvVendorItem.EditIndex, false);

        gvVendorItem.EditIndex = -1;
        BindData();
    }
}
