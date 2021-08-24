using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class AccountsCreditPurchaseOfBondProvisions : PhoenixBasePage
{
  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsCreditPurchaseOfBondProvisions.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvCreditPurchase')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Accounts/AccountsCreditPurchaseOfBondProvisions.aspx?committedyn=0", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Accounts/AccountsCreditPurchaseOfBondProvisions.aspx", "Clear", "clear-filter.png", "CLEAR");
            MenuOffice.AccessRights = this.ViewState;
            MenuOffice.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
           
            toolbar.AddButton("Commitment", "COMMIT", ToolBarDirection.Right);
            toolbar.AddButton("Credit Purchase", "CREDITPURCHASE", ToolBarDirection.Right);
            MenuGeneral.AccessRights = this.ViewState;
            MenuGeneral.MenuList = toolbar.Show();
          //  MenuOffice.SetTrigger(pnlAddressEntry);

            MenuGeneral.SelectedMenuIndex = 1;

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
           
            toolbarsub.AddButton("Committed", "COMMITTED", ToolBarDirection.Right);
            toolbarsub.AddButton("Not Yet Committed", "NOTCOMMITTED", ToolBarDirection.Right);
            MenuGeneralSub.AccessRights = this.ViewState;
            MenuGeneralSub.MenuList = toolbarsub.Show();

            MenuGeneralSub.SelectedMenuIndex = 1;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["orderid"] = "";
                ViewState["vesselid"] = "";
                ViewState["vesselsupplierid"] = "";
                gvCreditPurchase.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ddlStatus.HardList = PhoenixRegistersHard.ListHard(1, 41, byte.Parse("1"), "PEN,RCD,CNL");
                ddlStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 41, "RCD"); 
            }

          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvCreditPurchase.SelectedIndexes.Clear();
        gvCreditPurchase.EditIndexes.Clear();
        gvCreditPurchase.DataSource = null;
        gvCreditPurchase.Rebind();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
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


    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds=new DataSet();
        string[] alColumns = { "FLDVESSELCODE", "FLDREFERENCENO", "FLDORDERSTATUSNAME", "FLDORDERDATE", "FLDSUPPLIERNAME", "FLDCODE", "FLDCURRENCYCODE",
                                 "FLDAMTBEFOREDISCOUNT", "FLDAMTAFTERDISCOUNT", "FLDINVOICENUMBER" };
        string[] alCaptions = { "Vessel", "Order Number", "Status", "Order Date", "Supplier Name", "Supplier Code", "Invoice Currency",
                                 "Invoice Amount Before Discount", "Invoice Amount After Discount", "Invoice Number" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        NameValueCollection nvc = Filter.CurrentCreditPurchaseFilter;
        ds = PhoenixAccountsVesselAccounting.CreditPurchaseSearch(General.GetNullableInteger(ucVessel.SelectedVessel)
                , General.GetNullableString(txtOrderNo.Text)
                , General.GetNullableDateTime(txtfromdate.Text)
                , General.GetNullableDateTime(ToDate.Text)
                , General.GetNullableInteger(txtVendorId.Text)
                , General.GetNullableInteger(ucCurrency.SelectedCurrency)
                , sortexpression, sortdirection
                , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                , gvCreditPurchase.PageSize
                , ref iRowCount
                , ref iTotalPageCount, General.GetNullableInteger(ddlStatus.SelectedHard));

        General.SetPrintOptions("gvCreditPurchase", "Credit Purchase Of Bond/Provision", alCaptions, alColumns, ds);

        gvCreditPurchase.DataSource = ds;
        gvCreditPurchase.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvCreditPurchase_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

  
   
    //protected void gvCreditPurchase_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        GridView HeaderGrid = (GridView)sender;
    //        GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

    //        TableCell HeaderCell;
    //        HeaderCell = new TableCell();
    //        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
    //        HeaderGridRow.Cells.Add(HeaderCell);

    //        HeaderCell = new TableCell();
    //        HeaderCell.Text = "Order";
    //        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
    //        HeaderCell.ColumnSpan = 3;
    //        HeaderGridRow.Cells.Add(HeaderCell);

    //        HeaderCell = new TableCell();
    //        HeaderCell.Text = "Supplier";
    //        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
    //        HeaderCell.ColumnSpan = 2;
    //        HeaderGridRow.Cells.Add(HeaderCell);

    //        HeaderCell = new TableCell();
    //        HeaderCell.Text = "Invoice";
    //        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
    //        HeaderCell.ColumnSpan = 2;
    //        HeaderGridRow.Cells.Add(HeaderCell);

    //        HeaderCell = new TableCell();
    //        HeaderCell.Text = "Invoice Amount Discount";
    //        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
    //        HeaderCell.ColumnSpan = 2;
    //        HeaderGridRow.Cells.Add(HeaderCell);

    //        HeaderCell = new TableCell();
    //        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
    //        HeaderGridRow.Cells.Add(HeaderCell);

    //        gvCreditPurchase.Controls[0].Controls.AddAt(0, HeaderGridRow);
    //        GridViewRow row1 = ((GridViewRow)gvCreditPurchase.Controls[0].Controls[0]);
    //        row1.Attributes.Add("style", "position:static");
    //    }
    //    if (e.Row.RowType == DataControlRowType.DataRow
    //        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
    //        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
    //    {
    //        e.Row.TabIndex = -1;
    //        e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvCreditPurchase, "Select$" + e.Row.RowIndex.ToString(), false);
    //    }
    //}

   
   
    private bool IsValidCreditPurchaseMapping(string budgetid, string billtocomp)
    {
        ucError.HeaderMessage = "Please provide the following information.";

        if (General.GetNullableInteger(budgetid) == null)
            ucError.ErrorMessage = "Budget code is required.";

        if (General.GetNullableInteger(billtocomp) == null)
            ucError.ErrorMessage = "Bill to company is required.";

        return (!ucError.IsError);
    }

   

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDVESSELCODE", "FLDREFERENCENO", "FLDORDERSTATUSNAME", "FLDORDERDATE", "FLDSUPPLIERNAME", "FLDCODE", "FLDCURRENCYCODE",
                                 "FLDAMTBEFOREDISCOUNT", "FLDAMTAFTERDISCOUNT", "FLDINVOICENUMBER" };
        string[] alCaptions = { "Vessel", "Order Number", "Status", "Order Date", "Supplier Name", "Supplier Code", "Invoice Currency",
                                 "Invoice Amount Before Discount", "Invoice Amount After Discount", "Invoice Number" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsVesselAccounting.CreditPurchaseSearch(General.GetNullableInteger(ucVessel.SelectedVessel)
               , General.GetNullableString(txtOrderNo.Text)
               , General.GetNullableDateTime(txtfromdate.Text)
               , General.GetNullableDateTime(ToDate.Text)
               , General.GetNullableInteger(txtVendorId.Text)
               , General.GetNullableInteger(ucCurrency.SelectedCurrency)
              , sortexpression, sortdirection
               , Int32.Parse(ViewState["PAGENUMBER"].ToString()), iRowCount
               , ref iRowCount
               , ref iTotalPageCount, General.GetNullableInteger(ddlStatus.SelectedHard));

        Response.AddHeader("Content-Disposition", "attachment; filename=CreditPurchase.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Vessel Credit Purchase</h3></td>");
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



    protected void MenuOffice_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucCurrency.SelectedCurrency = "";
                txtfromdate.Text = string.Empty;
                txtOrderNo.Text = string.Empty;
                txtVendorCode.Text = string.Empty;
                txtVendorId.Text = string.Empty;
                txtVendorName.Text = string.Empty;                
                ToDate.Text = string.Empty;
                ddlStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 41, "RCD");
                ucVessel.SelectedVessel = "Dummy";

                Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuGeneralSub_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("NOTCOMMITTED"))
            {
                Response.Redirect("../Accounts/AccountsCreditPurchaseOfBondProvisions.aspx");
            }
            else if (CommandName.ToUpper().Equals("COMMITTED"))
            {
                Response.Redirect("../Accounts/AccountsCreditPurchaseOfBondProvisionsCommitted.aspx");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("CREDITPURCHASE"))
            {
                Response.Redirect("../Accounts/AccountsVesselSupplierMapping.aspx");
            }
            else if (CommandName.ToUpper().Equals("COMMIT"))
            {
                if (!string.IsNullOrEmpty(ViewState["orderid"].ToString()) && !string.IsNullOrEmpty(ViewState["vesselid"].ToString()) && !string.IsNullOrEmpty(ViewState["vesselsupplierid"].ToString()))
                {
                    Response.Redirect("../Accounts/AccountsVesselSupplierMapping.aspx?vesselid=" + ViewState["vesselid"] + "&vesselsupplierid=" + ViewState["vesselsupplierid"] + "&orderid=" + ViewState["orderid"].ToString(), false);
                }
                else
                {
                    ucError.ErrorMessage = "Please select an order.";
                    ucError.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCreditPurchase_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;


            else if (e.CommandName.ToUpper().Equals("SUPPLIERMAPPING"))
            {
                RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblVesselId");
                RadLabel lblvesselsupplierid = (RadLabel)e.Item.FindControl("lblVesselsupplierid");
                Response.Redirect("../Accounts/AccountsVesselSupplierMapping.aspx?vesselid=" + lblvesselid.Text + "&vesselsupplierid=" + lblvesselsupplierid.Text, false);
            }
            else if (e.CommandName.ToUpper().Equals("COMMITTPO"))
            {
                RadLabel lblOrderid = (RadLabel)e.Item.FindControl("lblOrderid");
                RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblVesselId");
                RadLabel lblvesselsupplierid = (RadLabel)e.Item.FindControl("lblVesselsupplierid");
                Response.Redirect("../Accounts/AccountsVesselSupplierMapping.aspx?vesselid=" + lblvesselid.Text + "&vesselsupplierid=" + lblvesselsupplierid.Text + "&orderid=" + lblOrderid.Text, false);
                //Response.Redirect("../Accounts/AccountsCreditPurchaseAdvancePayment.aspx?orderid=" + lblOrderid.Text, false);
            }
            else if (e.CommandName.ToUpper().Equals("SUPPLIERUNMAP"))
            {
                RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblVesselId");
                RadLabel lblvesselsupplierid = (RadLabel)e.Item.FindControl("lblVesselsupplierid");
                PhoenixAccountsVesselAccounting.VesselSupplierUnmap(int.Parse(lblvesselid.Text), new Guid(lblvesselsupplierid.Text));

                ucStatus.Text = "Supplier has been Unmapped Successfully.";
            }

            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                ViewState["orderid"] = ((RadLabel)e.Item.FindControl("lblOrderid")).Text;
                ViewState["vesselid"] = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                ViewState["vesselsupplierid"] = ((RadLabel)e.Item.FindControl("lblVesselsupplierid")).Text;
                ViewState["status"] = ((RadLabel)e.Item.FindControl("lblOrderStatus")).Text;
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

  

    protected void gvCreditPurchase_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            ImageButton edit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            ImageButton save = (ImageButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            ImageButton cancel = (ImageButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

            ImageButton imgSupplier = (ImageButton)e.Item.FindControl("imgSupplier");
            if (imgSupplier != null) imgSupplier.Visible = SessionUtil.CanAccess(this.ViewState, imgSupplier.CommandName);

            ImageButton cmdAdvancePayment = (ImageButton)e.Item.FindControl("cmdAdvancePayment");
            if (cmdAdvancePayment != null) cmdAdvancePayment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdvancePayment.CommandName);


            {
                UserControlBudgetCode ucBudgetCodeEdit = (UserControlBudgetCode)e.Item.FindControl("ucBudgetCodeEdit");
                RadLabel lblStockType = (RadLabel)e.Item.FindControl("lblStockType");
                DataRowView drv = (DataRowView)e.Item.DataItem;

                UserControlCompany ucCompany = (UserControlCompany)e.Item.FindControl("ddlCompany");
                if (ucCompany != null && drv["FLDBILLTOCOMPANY"].ToString() != "")
                    ucCompany.SelectedCompany = drv["FLDBILLTOCOMPANY"].ToString();
                else if (ucCompany != null && drv["FLDBILLTOCOMPANY"].ToString() == "")
                    ucCompany.SelectedCompany = "16";

                if (ucBudgetCodeEdit != null && drv["FLDBUDGETCODE"].ToString() != "")
                    ucBudgetCodeEdit.SelectedBudgetSubAccount = drv["FLDBUDGETCODE"].ToString();
                else if (ucBudgetCodeEdit != null && drv["FLDBUDGETCODE"].ToString() == "")
                {
                    if (lblStockType != null && lblStockType.Text == PhoenixCommonRegisters.GetHardCode(1, 97, "PRV")) //PROVISION
                        ucBudgetCodeEdit.SelectedBudgetSubAccount = "2723";
                    if (lblStockType != null && lblStockType.Text == PhoenixCommonRegisters.GetHardCode(1, 97, "BND")) //BOND
                        ucBudgetCodeEdit.SelectedBudgetSubAccount = "2720";
                }

                LinkButton lb = (LinkButton)e.Item.FindControl("lnkOrderNumber");
                LinkButton supcode = (LinkButton)e.Item.FindControl("lnkSupplierCode");
                RadLabel lr = (RadLabel)e.Item.FindControl("lblOrderid");
                RadLabel lm = (RadLabel)e.Item.FindControl("lblIsMapped");
                //ImageButton ib = (ImageButton)e.Row.FindControl("imgSupplier");
                ImageButton ap = (ImageButton)e.Item.FindControl("cmdAdvancePayment");
                RadLabel bc = (RadLabel)e.Item.FindControl("lblBudgetcode");
                RadLabel ci = (RadLabel)e.Item.FindControl("lblBilltoCompany");
                RadLabel lblSupplierCode = (RadLabel)e.Item.FindControl("lblSupplierCode");
                RadLabel lblMappedSupplierCode = (RadLabel)e.Item.FindControl("lblMappedSupplierCode");
                if (lb != null)
                   
                    lb.Attributes.Add("onclick", "openNewWindow('codehelp', '','" + Session["sitepath"] + "/Accounts/AccountsCreditPurchaseLineItems.aspx?orderid=" + lr.Text + "');return true;");
                if (lm != null && lm.Text == "0")
                {
                    if (supcode != null && lblSupplierCode != null)
                        supcode.Attributes.Add("onclick", "openNewWindow('codehelp','','" + Session["sitepath"] + "/Registers/RegistersVesselSupplierList.aspx?saveyn=n&SUPPLIERCODE=" + lblSupplierCode.Text + "');return true;");
                }
                else
                {
                    if (supcode != null && lblMappedSupplierCode != null)
                      
                        supcode.Attributes.Add("onclick", "openNewWindow('codehelp', '','" + Session["sitepath"] + "/Registers/RegistersOffice.aspx?VIEWONLY=Y&ADDRESSCODE=" + lblMappedSupplierCode.Text + "');return true;");
                }
                ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
                if (att != null)
                {
                    att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                    if (drv["FLDISATTACHMENT"].ToString() == "0")
                        att.ImageUrl = Session["images"] + "/no-attachment.png";
                    //att.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?U=1&dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    //    + PhoenixModule.VESSELACCOUNTS + "'); return false;");

                    att.Attributes.Add("onclick", "openNewWindow('att', '','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?U=1&dtkey=" + drv["FLDDTKEY"].ToString() + "&mod=" + PhoenixModule.VESSELACCOUNTS + "'); return false;");
                }
                string Status = drv["FLDORDERSTATUSNAME"].ToString();
                //if (Status.ToUpper() == "PENDING")
                //{
                //    ap.Visible = false;
                //}
            }
            //if (e.Item is GridHeaderItem)
            //{
            //    RadGrid HeaderGrid = (RadGrid)sender;
            //    GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            //    TableCell HeaderCell;
            //    HeaderCell = new TableCell();
            //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            //    HeaderGridRow.Cells.Add(HeaderCell);

            //    HeaderCell = new TableCell();
            //    HeaderCell.Text = "Order";
            //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            //    HeaderCell.ColumnSpan = 3;
            //    HeaderGridRow.Cells.Add(HeaderCell);

            //    HeaderCell = new TableCell();
            //    HeaderCell.Text = "Supplier";
            //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            //    HeaderCell.ColumnSpan = 2;
            //    HeaderGridRow.Cells.Add(HeaderCell);

            //    HeaderCell = new TableCell();
            //    HeaderCell.Text = "Invoice";
            //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            //    HeaderCell.ColumnSpan = 2;
            //    HeaderGridRow.Cells.Add(HeaderCell);

            //    HeaderCell = new TableCell();
            //    HeaderCell.Text = "Invoice Amount Discount";
            //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            //    HeaderCell.ColumnSpan = 2;
            //    HeaderGridRow.Cells.Add(HeaderCell);

            //    HeaderCell = new TableCell();
            //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            //    HeaderGridRow.Cells.Add(HeaderCell);

            //    gvCreditPurchase.Controls[0].Controls.AddAt(0, HeaderGridRow);
            //    GridViewRow row1 = ((GridViewRow)gvCreditPurchase.Controls[0].Controls[0]);
            //    row1.Attributes.Add("style", "position:static");
            //}
            ////if (e.Row.RowType == DataControlRowType.DataRow
            ////    && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            ////    && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
            ////{
            ////    e.Row.TabIndex = -1;
            ////    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvCreditPurchase, "Select$" + e.Row.RowIndex.ToString(), false);
            ////}
        
        }
    }

    protected void gvCreditPurchase_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCreditPurchase.CurrentPageIndex + 1;
        BindData();
    }
}
