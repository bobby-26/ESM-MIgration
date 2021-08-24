using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;


public partial class AccountsOffSettingContraVocLineDetailsWOSubAccount : PhoenixBasePage
{
    public decimal TransactionAmountTotal = 0;
    public decimal BaseAmountTotal = 0;
    public decimal ReportAmountTotal = 0;
    public string strTransactionAmountTotal = string.Empty;
    public string strBaseAmountTotal = string.Empty;
    public string strReportAmountTotal = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsOffSettingContraVocLineDetailsWOSubAccount.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvContraVoucherLineItem')", "Print Grid", "icon_print.png", "PRINT");

            MenuOrderLineItem.AccessRights = this.ViewState;
            MenuOrderLineItem.MenuList = toolbargrid.Show();
           // MenuOrderLineItem.SetTrigger(pnlStockItemEntry);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Line Items", "LINEITEMS", ToolBarDirection.Right);
            toolbarmain.AddButton("Contra Voucher", "CONTRAVOUCHER", ToolBarDirection.Right);
            toolbarmain.AddButton("Off-Setting Entries", "OFFSETTINGENTRIES", ToolBarDirection.Right);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            MenuLineItem.AccessRights = this.ViewState;
            MenuLineItem.MenuList = toolbarmain.Show();

            MenuLineItem.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
               

                ViewState["PAGENUMBER"] = 1;
                ViewState["TOTALPAGECOUNT"] = 1;
                ViewState["ROWCOUNT"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["QACCOUNTCODE"] = "";


                gvLineItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                if (Request.QueryString["voucherid"] != null && Request.QueryString["voucherid"].ToString() != string.Empty)
                    ViewState["voucherid"] = Request.QueryString["voucherid"].ToString();

               // Title1.Text = "Contra Vocher Items    (  " + PhoenixAccountsContraVoucher.ContraVoucherNumber + "     )";
                
            }
            if (Request.QueryString["VOUCHERPOSTSTATUS"] != null && Request.QueryString["VOUCHERPOSTSTATUS"] != string.Empty)
                ViewState["VOUCHERPOSTSTATUS"] = Request.QueryString["VOUCHERPOSTSTATUS"];
            if (Request.QueryString["qvoucherlineitemcode"] != null && Request.QueryString["qvoucherlineitemcode"] != string.Empty)
                ViewState["VOUCHERLINEITEMCODE"] = Request.QueryString["qvoucherlineitemcode"];
            if (Request.QueryString["offsettinglineitemid"] != null && Request.QueryString["offsettinglineitemid"] != string.Empty)
                ViewState["OFFSETTINGLINEITEMID"] = Request.QueryString["offsettinglineitemid"];
            //if (Request.QueryString["offsetisposted"] != null && Request.QueryString["offsetisposted"] != string.Empty)
            //    ViewState["offsetisposted"] = Request.QueryString["offsetisposted"];

            if (Request.QueryString["qvouchercode"] != null && Request.QueryString["qvouchercode"].ToString() != string.Empty)
            {
                ViewState["voucherid"] = Request.QueryString["qvouchercode"].ToString();
                ifMoreInfo.Attributes["src"] = "AccountsOffSettingContraVocLineWOSubAccount.aspx?qvouchercode=" + Request.QueryString["qvouchercode"].ToString() + "&qvoucherlineitemcode" + ViewState["VOUCHERLINEITEMCODE"] + "&offsettinglineitemid=" + ViewState["OFFSETTINGLINEITEMID"] + "&offsetisposted=" + ViewState["offsetisposted"] + "&newvoucherid=" + ViewState["newvoucherid"];
            }
            else
            {
                ifMoreInfo.Attributes["src"] = "AccountsOffSettingContraVocLineWOSubAccount.aspx?offsetisposted=" + ViewState["offsetisposted"];
            }
            if (Request.QueryString["contravoucherid"] != null && Request.QueryString["contravoucherid"] != string.Empty)
                ViewState["contravoucherid"] = Request.QueryString["contravoucherid"];
            //BindData();

            //SetPageNavigator();
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
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("CONTRAVOUCHER"))
            {
                Response.Redirect("../Accounts/AccountsOffSettingContraVocMasterWOSubAccount.aspx?voucherid=" + ViewState["contravoucherid"] + "&offsettinglineitemid=" + ViewState["OFFSETTINGLINEITEMID"] + "&contravoucherid=" + ViewState["contravoucherid"]);
            }
            if (CommandName.ToUpper().Equals("OFFSETTINGENTRIES"))
            {
                Response.Redirect("../Accounts/AccountsOffSettingEntriesListWOSubAccount.aspx?voucherid=" + ViewState["contravoucherid"] + "&offsettinglineitemid=" + ViewState["OFFSETTINGLINEITEMID"]);
            }
            if (ViewState["voucherlineitemcode"] != null)
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + ViewState["voucherid"] + "&voucherlineitemcode=" + ViewState["voucherlineitemcode"].ToString() + "&offsettinglineitemid=" + ViewState["OFFSETTINGLINEITEMID"] + "&offsetisposted=" + ViewState["offsetisposted"] + "&newvoucherid=" + ViewState["newvoucherid"];
            else
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + ViewState["voucherid"].ToString() + "&offsettinglineitemid=" + ViewState["OFFSETTINGLINEITEMID"] + "&offsetisposted=" + ViewState["offsetisposted"];

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
                BindData();
            }
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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = {"FLDACCOUNTCODE", "FLDDESCRIPTION","FLDBUDGETCODE","FLDCURRENCYNAME",
                                 "FLDTRANSACTIONAMOUNT","FLDBASEAMOUNT","FLDREPORTAMOUNT"};
        string[] alCaptions = {"Account Code", "Account Description","Sub Account Code","Transaction Currency",
                                 "Prime Amount","Base Amount", "Report Amount"};
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
       
            ds = PhoenixAccountsContraVoucher.ContraVoucherLineItemSearch(
                                                                    int.Parse(ViewState["voucherid"].ToString())
                                                                   , null
                                                                   , null
                                                                   , string.Empty
                                                                   , string.Empty
                                                                   , null
                                                                   , null
                                                                   , sortdirection
                                                                   , sortexpression
                                                                   , (int)ViewState["PAGENUMBER"]
                                                                   , General.ShowRecords(null)
                                                                   , ref iRowCount
                                                                   , ref iTotalPageCount
                                                                   , ref TransactionAmountTotal
                                                                   , ref BaseAmountTotal
                                                                   , ref ReportAmountTotal
                                                              );
        strTransactionAmountTotal = String.Format("{0:n}", TransactionAmountTotal);
        strBaseAmountTotal = String.Format("{0:n}", BaseAmountTotal);
        strReportAmountTotal = String.Format("{0:n}", ReportAmountTotal);
        DataTable dt = ds.Tables[0];
        foreach (DataRow row in dt.Rows)
        {
            row["FLDTRANSACTIONAMOUNT"] = string.Format(String.Format("{0:#####.00}", row["FLDTRANSACTIONAMOUNT"].ToString()));
            row["FLDBASEAMOUNT"] = string.Format(String.Format("{0:#####.00}", row["FLDBASEAMOUNT"]));
            row["FLDREPORTAMOUNT"] = string.Format(String.Format("{0:#####.00}", row["FLDREPORTAMOUNT"]));
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=VoucherLineItem.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Voucher Line Item Details</h3></td>");
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["voucherid"] != null)
        {           
            ds = PhoenixAccountsContraVoucher.ContraVoucherLineItemSearchForOffsetting(
                                                                    int.Parse(ViewState["contravoucherid"].ToString())
                                                                   , null
                                                                   , null
                                                                   , string.Empty
                                                                   , string.Empty
                                                                   , null
                                                                   , null
                                                                   , sortdirection
                                                                   , sortexpression
                                                                   , (int)ViewState["PAGENUMBER"]
                                                                   , gvLineItem.PageSize
                                                                   , ref iRowCount
                                                                   , ref iTotalPageCount
                                                                   , ref TransactionAmountTotal
                                                                   , ref BaseAmountTotal
                                                                   , ref ReportAmountTotal
                                                              );
            strTransactionAmountTotal = String.Format("{0:n}", TransactionAmountTotal);
            strBaseAmountTotal = String.Format("{0:n}", BaseAmountTotal);
            strReportAmountTotal = String.Format("{0:n}", ReportAmountTotal);

            gvLineItem.DataSource = ds;
            gvLineItem.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
               

                DataRow dr = ds.Tables[0].Rows[0];
                ViewState["ISPERIODLOCKED"] = dr["FLDISPERIODLOCKED"].ToString();
                ViewState["offsetisposted"] = dr["FLDISPOSTED"].ToString();
                ViewState["newvoucherid"] = dr["FLDNEWVOUCHERID"].ToString();
                if (ViewState["voucherlineitemcode"] == null)
                {
                    ViewState["voucherlineitemcode"] = ds.Tables[0].Rows[0]["FLDVOUCHERLINEITEMID"].ToString();
                    //gvLineItem.SelectedIndex = 0;
                }

                if (ViewState["PAGEURL"] == null)
                {
                    ViewState["PAGEURL"] = "../Accounts/AccountsOffSettingContraVocLineWOSubAccount.aspx?offsettinglineitemid=" + ViewState["OFFSETTINGLINEITEMID"]+"&qvouchercode=";
                }
                {
                    if (ViewState["voucherlineitemcode"] != null)
                    {
                        string strRowno = string.Empty;
                        if (ViewState["rowno"] != null) { strRowno = ViewState["rowno"].ToString(); } else { strRowno = "10"; }
                        ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + ViewState["voucherid"] + "&voucherlineitemcode=" + ViewState["voucherlineitemcode"].ToString() + "&rowno=" + strRowno + "&offsetisposted=" + ViewState["offsetisposted"] + "&newvoucherid=" + ViewState["newvoucherid"];
                    }
                    else
                        ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + ViewState["voucherid"].ToString() + "&offsetisposted=" + ViewState["offsetisposted"];
                }

                DataTable dt1 = ds.Tables[0];
                foreach (DataRow row in dt1.Rows)
                {
                    row["FLDTRANSACTIONAMOUNT"] = string.Format(String.Format("{0:#####.00}", row["FLDTRANSACTIONAMOUNT"].ToString()));
                    row["FLDBASEAMOUNT"] = string.Format(String.Format("{0:#####.00}", row["FLDBASEAMOUNT"]));
                    row["FLDREPORTAMOUNT"] = string.Format(String.Format("{0:#####.00}", row["FLDREPORTAMOUNT"]));
                }
            }
            else
            {
               // DataTable dt = ds.Tables[0];
              //  ShowNoRecordsFound(dt, gvLineItem);
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            if (ViewState["ISPERIODLOCKED"] != null)
            {
                if (int.Parse(ViewState["ISPERIODLOCKED"].ToString()) == 1)
                {

                    foreach (GridDataItem item in gvLineItem.Items)
                    {
                       // GridViewRow gvRow = gvLineItem.Rows[i];
                        ((ImageButton)gvLineItem.FindControl("cmdEdit")).Visible = false;
                        ((ImageButton)gvLineItem.FindControl("cmdDelete")).Visible = false;
                        ((Label)gvLineItem.FindControl("lblIsPeriodLocked")).Visible = true;
                    }
                    //for (int i = 0; i < gvLineItem.Rows.Count; i++)
                    //{
                    //    GridViewRow gvRow = gvLineItem.Rows[i];
                    //    ((ImageButton)gvRow.FindControl("cmdEdit")).Visible = false;
                    //    ((ImageButton)gvRow.FindControl("cmdDelete")).Visible = false;
                    //    ((Label)gvRow.FindControl("lblIsPeriodLocked")).Visible = true;
                    //}
                }
            }
            string[] alColumns = {"FLDACCOUNTCODE", "FLDDESCRIPTION","FLDBUDGETCODE","FLDCURRENCYNAME",
                                 "FLDTRANSACTIONAMOUNT","FLDBASEAMOUNT","FLDREPORTAMOUNT"};
            string[] alCaptions = {"Account Code", "Account Description","Sub Account Code","Transaction Currency",
                                 "Prime Amount","Base Amount", "Report Amount"};
            General.SetPrintOptions("gvContraVoucherLineItem", "Voucher Line Item", alCaptions, alColumns, ds);
        }
    }

    //protected void cmdSort_Click(object sender, EventArgs e)
    //{

    //    ImageButton ib = (ImageButton)sender;
    //    try
    //    {
    //        ViewState["SORTEXPRESSION"] = ib.CommandName;
    //        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvLineItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        _gridView.EditIndex = -1;
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}


    protected void gvLineItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

        if (e.CommandName.ToUpper().Equals("DELETE"))
        {


            string strVoucherLineId = ((RadLabel)e.Item.FindControl("lblVoucherLineId")).Text;
            try
            {
                PhoenixAccountsContraVoucher.ContraVoucherLineItemDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(strVoucherLineId), int.Parse(ViewState["voucherid"].ToString()));
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }

            BindData();
            ifMoreInfo.Attributes["src"] = "AccountsOffSettingContraVocLineSubAccount.aspx?qvouchercode=" + Request.QueryString["qvouchercode"].ToString() + "&offsetisposted=" + ViewState["offsetisposted"] + "&newvoucherid=" + ViewState["newvoucherid"];
        }

        if (e.CommandName.ToUpper().Equals("EDIT"))
        {

            try
            {
                ViewState["voucherlineitemcode"] = ((RadLabel)e.Item.FindControl("lblVoucherLineId")).Text;
                ViewState["offsetisposted"] = ((RadLabel)e.Item.FindControl("lblIsPosted")).Text;
                ViewState["newvoucherid"] = ((RadLabel)e.Item.FindControl("lblNewVoucherId")).Text;
                //  ifMoreInfo.Attributes["src"] = "../Accounts/AccountsPurchaseInvoiceVoucherLineItem.aspx?voucherid=" + ViewState["voucherid"].ToString() + "&voucherlineitemcode=" + ViewState["voucherlineitemcode"].ToString();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
        if (e.CommandName.ToUpper().Equals("SAVE"))
        {
            // int iRowno = Int32.Parse(e.CommandArgument.ToString());
            try
            {
                PhoenixAccountsContraVoucher.ContraVoucherLineItemAccountUpdate(new Guid(((RadLabel)e.Item.FindControl("lblVoucherLineId")).Text.ToString()), ((RadTextBox)e.Item.FindControl("txtAccountCode")).Text.ToString(), PhoenixSecurityContext.CurrentSecurityContext.UserCode);

                BindData();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }


    }
    //protected void gvLineItem_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;

    //    if (e.CommandName.ToUpper().Equals("DELETE"))
    //    {
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //        string strVoucherLineId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVoucherLineId")).Text;
    //        try
    //        {
    //            PhoenixAccountsContraVoucher.ContraVoucherLineItemDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(strVoucherLineId), int.Parse(ViewState["voucherid"].ToString()));
    //        }
    //        catch (Exception ex)
    //        {
    //            ucError.HeaderMessage = "";
    //            ucError.ErrorMessage = ex.Message;
    //            ucError.Visible = true;
    //            return;
    //        }
    //        _gridView.EditIndex = -1;
    //        BindData();
    //        ifMoreInfo.Attributes["src"] = "AccountsOffSettingContraVocLineWOSubAccount.aspx?qvouchercode=" + Request.QueryString["qvouchercode"].ToString() + "&offsetisposted=" + ViewState["offsetisposted"] + "&newvoucherid=" + ViewState["newvoucherid"];
    //    }

    //    if (e.CommandName.ToUpper().Equals("EDIT"))
    //    {
    //        int iRowno = Int32.Parse(e.CommandArgument.ToString());
    //        try
    //        {
    //            ViewState["voucherlineitemcode"] = ((Label)gvLineItem.Rows[iRowno].FindControl("lblVoucherLineId")).Text;
    //            ViewState["offsetisposted"] = ((Label)gvLineItem.Rows[iRowno].FindControl("lblIsPosted")).Text;
    //            ViewState["newvoucherid"] = ((Label)gvLineItem.Rows[iRowno].FindControl("lblNewVoucherId")).Text;
    //            //  ifMoreInfo.Attributes["src"] = "../Accounts/AccountsPurchaseInvoiceVoucherLineItem.aspx?voucherid=" + ViewState["voucherid"].ToString() + "&voucherlineitemcode=" + ViewState["voucherlineitemcode"].ToString();
    //        }
    //        catch (Exception ex)
    //        {
    //            ucError.ErrorMessage = ex.Message;
    //            ucError.Visible = true;
    //        }
    //    }
    //    if (e.CommandName.ToUpper().Equals("SAVE"))
    //    {
    //        int iRowno = Int32.Parse(e.CommandArgument.ToString());
    //        try
    //        {
    //            PhoenixAccountsContraVoucher.ContraVoucherLineItemAccountUpdate(new Guid(((Label)gvLineItem.Rows[iRowno].FindControl("lblVoucherLineId")).Text.ToString()), ((TextBox)gvLineItem.Rows[iRowno].FindControl("txtAccountCode")).Text.ToString(), PhoenixSecurityContext.CurrentSecurityContext.UserCode);
    //            _gridView.EditIndex = -1;
    //            BindData();
    //        }
    //        catch (Exception ex)
    //        {
    //            ucError.ErrorMessage = ex.Message;
    //            ucError.Visible = true;
    //        }
    //    }


    //}

    //protected void gvLineItem_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    try
    //    {
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}


    //protected void gvLineItem_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        _gridView.SelectedIndex = de.NewEditIndex;
    //        ViewState["voucherlineitemcode"] = ((Label)gvLineItem.Rows[de.NewEditIndex].FindControl("lblVoucherLineId")).Text;
    //        ViewState["newvoucherid"] = ((Label)gvLineItem.Rows[de.NewEditIndex].FindControl("lblNewVoucherId")).Text;           
    //        ViewState["rowno"] = ((LinkButton)gvLineItem.Rows[de.NewEditIndex].FindControl("lnkVoucherLineItemNo")).Text;
    //        if (ViewState["ISPERIODLOCKED"] != null && ViewState["rowno"].ToString()!="10")
    //        {
    //            if (int.Parse(ViewState["ISPERIODLOCKED"].ToString()) == 0)
    //            {
    //                _gridView.EditIndex = de.NewEditIndex;
    //            }
    //            else
    //            {
    //                _gridView.EditIndex = -1;
    //            }
    //        }
    //        BindData();

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}


    //protected void gvLineItem_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow
    //        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
    //        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
    //    {
    //        e.Row.TabIndex = -1;
    //        e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvLineItem, "Edit$" + e.Row.RowIndex.ToString(), false);
    //    }
    //}

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
    //protected void gvLineItem_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //   Label lblIsPosted = (Label)e.Row.FindControl("lblIsPosted");
    //    int posted = int.Parse((lblIsPosted !=null && !string.IsNullOrEmpty(lblIsPosted.Text)) ? lblIsPosted.Text : "0");
    //    LinkButton lnkVoucherLineItemNo = (LinkButton)e.Row.FindControl("lnkVoucherLineItemNo");
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
    //        if (db != null)
    //        {
    //            if (posted == 1)
    //                db.Visible = false;
    //            db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");                
    //            if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
    //        }
    //    }

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
    //        if (cmdEdit != null)
    //        {

    //            if (posted == 1 || lnkVoucherLineItemNo.Text=="10")
    //                cmdEdit.Visible = false;
    //            if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
    //        }

    //        ImageButton cmdDelete = (ImageButton)e.Row.FindControl("cmdDelete");
    //        if (cmdDelete != null)
    //        {
    //            if (posted == 1)
    //                cmdDelete.Visible = false;
    //            if (!SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName)) cmdDelete.Visible = false;
    //        }

    //        ImageButton hlnkSplit = (ImageButton)e.Row.FindControl("hlnkSplit");
    //        if (hlnkSplit != null)
    //            if (!SessionUtil.CanAccess(this.ViewState, hlnkSplit.CommandName)) hlnkSplit.Visible = false;

    //        string strAccountActive = string.Empty;
    //        TextBox tb1 = (TextBox)e.Row.FindControl("txtAccountDescription");
    //        if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
    //        tb1 = (TextBox)e.Row.FindControl("txtAccountId");
    //        if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");

    //        ImageButton ib1 = (ImageButton)e.Row.FindControl("btnShowAccountEdit");
    //        if (ib1 != null)
    //        {
    //            ib1.Attributes.Add("onclick", "return showPickList('spnPickListCompanyAccountEdit', 'codehelp1', '', '../Common/CommonPickListCompanyAccount.aspx?ignoreiframe=true', true); ");
    //            if (!SessionUtil.CanAccess(this.ViewState, ib1.CommandName)) ib1.Visible = false;
    //        }
    //        if (ViewState["ISPERIODLOCKED"] != null)
    //        {
    //            if (ViewState["ISPERIODLOCKED"].ToString() == "1")
    //            {
    //                if (cmdEdit != null)
    //                    cmdEdit.Visible = false;

    //                if (cmdDelete != null)
    //                    cmdDelete.Visible = false;
    //            }
    //        }

    //        Label lblAccountActiveYN = (Label)e.Row.FindControl("lblAccountActiveYN");
    //        if (lblAccountActiveYN != null)
    //        {
    //            strAccountActive = lblAccountActiveYN.Text;
    //        }
    //        if (strAccountActive == "0")
    //        {
    //            if (cmdEdit != null)
    //                cmdEdit.Visible = false;

    //            if (cmdDelete != null)
    //                cmdDelete.Visible = false;
    //        }

    //        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
    //        {
    //            Label lblVoucherLineId = (Label)e.Row.FindControl("lblVoucherLineId");              
    //            if (lnkVoucherLineItemNo != null)
    //            {
    //                if (!SessionUtil.CanAccess(this.ViewState, lnkVoucherLineItemNo.CommandName)) lnkVoucherLineItemNo.Visible = false;
    //            }
    //            if (hlnkSplit != null)
    //            {
    //                if (!SessionUtil.CanAccess(this.ViewState, hlnkSplit.CommandName)) hlnkSplit.Visible = false;
    //                hlnkSplit.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'AccountsVoucherLineItemSplit.aspx?qLineItemId=" + lblVoucherLineId.Text + "&qRowno=" + lnkVoucherLineItemNo.Text + "');return false;");
    //            }
    //        }
    //        if (posted == 1)
    //            gvLineItem.Columns[8].Visible = false;
    //    }
    //}
    protected void gvLineItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadLabel lblIsPosted = (RadLabel)e.Item.FindControl("lblIsPosted");
        int posted = int.Parse((lblIsPosted != null && !string.IsNullOrEmpty(lblIsPosted.Text)) ? lblIsPosted.Text : "0");
        LinkButton lnkVoucherLineItemNo = (LinkButton)e.Item.FindControl("lnkVoucherLineItemNo");
        if (e.Item is GridDataItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                if (posted == 1)
                    db.Visible = false;
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
        }

        if (e.Item is GridDataItem)
        {
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
            {

                if (posted == 1 || lnkVoucherLineItemNo.Text == "10")
                    cmdEdit.Visible = false;
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
            }

            ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null)
            {
                if (posted == 1)
                    cmdDelete.Visible = false;
                if (!SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName)) cmdDelete.Visible = false;
            }

            ImageButton hlnkSplit = (ImageButton)e.Item.FindControl("hlnkSplit");
            if (hlnkSplit != null)
                if (!SessionUtil.CanAccess(this.ViewState, hlnkSplit.CommandName)) hlnkSplit.Visible = false;

            string strAccountActive = string.Empty;
            RadTextBox tb1 = (RadTextBox)e.Item.FindControl("txtAccountDescription");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtAccountId");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");

            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowAccountEdit");
            if (ib1 != null)
            {
                ib1.Attributes.Add("onclick", "return showPickList('spnPickListCompanyAccountEdit', 'codehelp1', '', '../Common/CommonPickListCompanyAccount.aspx?ignoreiframe=true', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, ib1.CommandName)) ib1.Visible = false;
            }
            if (ViewState["ISPERIODLOCKED"] != null)
            {
                if (ViewState["ISPERIODLOCKED"].ToString() == "1")
                {
                    if (cmdEdit != null)
                        cmdEdit.Visible = false;

                    if (cmdDelete != null)
                        cmdDelete.Visible = false;
                }
            }

            RadLabel lblAccountActiveYN = (RadLabel)e.Item.FindControl("lblAccountActiveYN");
            if (lblAccountActiveYN != null)
            {
                strAccountActive = lblAccountActiveYN.Text;
            }
            if (strAccountActive == "0")
            {
                if (cmdEdit != null)
                    cmdEdit.Visible = false;

                if (cmdDelete != null)
                    cmdDelete.Visible = false;
            }

            if (!e.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Equals(DataControlRowState.Edit))
            {
                RadLabel lblVoucherLineId = (RadLabel)e.Item.FindControl("lblVoucherLineId");
                if (lnkVoucherLineItemNo != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, lnkVoucherLineItemNo.CommandName)) lnkVoucherLineItemNo.Visible = false;
                }
                if (hlnkSplit != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, hlnkSplit.CommandName)) hlnkSplit.Visible = false;
                    hlnkSplit.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'AccountsVoucherLineItemSplit.aspx?qLineItemId=" + lblVoucherLineId.Text + "&qRowno=" + lnkVoucherLineItemNo.Text + "');return false;");
                }
            }
            if (posted == 1)
                gvLineItem.Columns[8].Visible = false;
        }
    }

    protected void txtAccountCode_changed(object sender, EventArgs e)
    {
        //ImageButton imgbtn = (ImageButton)sender;
        //int rownumber = int.Parse(imgbtn.CommandArgument);
        //TextBox tb1 = (TextBox)gvLineItem.Rows[rownumber].FindControl("txtAccountCode");
        //Session["sOffsetAccountCode"] = tb1.Text;
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

   
}
