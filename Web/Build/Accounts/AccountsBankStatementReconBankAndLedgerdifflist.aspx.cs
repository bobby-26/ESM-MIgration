using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using System.Collections;
using Telerik.Web.UI;


public partial class Accounts_AccountsBankStatementReconBankAndLedgerdifflist : PhoenixBasePage
{
    public string strTransactionAmountTotal;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // iCompanyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            SessionUtil.PageAccessRights(this.ViewState);
            if (Session["CHECKED_ITEMS"] != null)
                Session["CHECKED_ITEMS"] = null;
            if (Session["CHECKED_ITEMS_LEDGER"] != null)
                Session["CHECKED_ITEMS_LEDGER"] = null;
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["VALIDATEFLAG"] = "";
        }

        PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Match and Allocate", "MATCH");
            toolbar.AddButton("Validate", "VALIDATE");
            MenutravelInvoice.AccessRights = this.ViewState;
            MenutravelInvoice.MenuList = toolbar.Show();

            if (Request.QueryString["uploadid"] != null && Request.QueryString["uploadid"].ToString() != "")
                ViewState["uploadid"] = Request.QueryString["uploadid"].ToString();
            else
                ViewState["uploadid"] = "";


            //PhoenixToolbar toolbar1 = new PhoenixToolbar();
            //toolbar1.AddButton("Bank Statement", "BANKSTATEMENT");
            //toolbar1.AddButton("Bank Recon Status", "RECONSTATUS");
            //toolbar1.AddButton("Allocation/Bank Tag Report", "ALLOCATIONREPORT");
            //Bankupload.AccessRights = this.ViewState;
            //Bankupload.MenuList = toolbar1.Show();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsBankStatementReconBankAndLedgerdifflist.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvBankStatement')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsBankStatementReconBankAndLedgerdifflist.aspx", "Find", "search.png", "FIND");


            MenuAttachment.AccessRights = this.ViewState;
            MenuAttachment.MenuList = toolbargrid.Show();


        BindData();
        BindDataBankLedgerline();
        BindCurrentBankStatement();
        //SetPageNavigator();
    }
    //protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    //{
    //    DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
    //    try
    //    {
    //        if (dce.CommandName.ToUpper().Equals("BANKSTATEMENT"))
    //        {
    //            Response.Redirect("../Accounts/AccountsBankStatementExcelUpload.aspx", false);
    //        }

    //        if (dce.CommandName.ToUpper().Equals("RECONSTATUS"))
    //        {
    //            Response.Redirect("../Accounts/AccountsBankStatementReconStatusList.aspx", false);
    //        }

    //        if (dce.CommandName.ToUpper().Equals("ALLOCATIONREPORT"))
    //        {
    //            Response.Redirect("../Accounts/AccountsBankStatementReconExcelUploadAllocation.aspx", false);
    //        }

    //        //if (dce.CommandName.ToUpper().Equals("REMITTANCE"))
    //        //{
    //        //    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsRemittanceRequest.aspx?REMITTENCEID=" + ViewState["Remittenceid"];
    //        //}
    //        //if (dce.CommandName.ToUpper().Equals("LINEITEMS") && ViewState["Remittenceid"] != null && ViewState["Remittenceid"].ToString() != string.Empty)
    //        //{
    //        //    Response.Redirect("../Accounts/AccountsRemittanceRequestLineItem.aspx?REMITTENCEID=" + ViewState["Remittenceid"]);
    //        //}            
    //        //if (dce.CommandName.ToUpper().Equals("HISTORY") && ViewState["Remittenceid"] != null && ViewState["Remittenceid"].ToString() != string.Empty)
    //        //{
    //        //    Response.Redirect("../Accounts/AccountsRemittanceHistory.aspx?REMITTANCEID=" + ViewState["Remittenceid"]);
    //        //}
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void MenutravelInvoice_OnTabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("MATCH"))
        {
            try
            {
                string selectedstatements = ",";
                string selectedledgers = ",";
                if (Session["CHECKED_ITEMS"] != null)
                {
                    ArrayList SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];
                    if (SelectedPvs != null && SelectedPvs.Count > 0)
                    {
                        foreach (string index in SelectedPvs)
                        { selectedstatements = selectedstatements + index + ","; }
                    }
                }

                if (Session["CHECKED_ITEMS_LEDGER"] != null)
                {
                    ArrayList SelectedPvsLedger = (ArrayList)Session["CHECKED_ITEMS_LEDGER"];
                    if (SelectedPvsLedger != null && SelectedPvsLedger.Count > 0)
                    {
                        foreach (string index in SelectedPvsLedger)
                        { selectedledgers = selectedledgers + index + ","; }
                    }
                }


                PhoenixAccountsBankStatementReconUpload.Bankstatementselectedlineallocate(General.GetNullableString(selectedstatements), General.GetNullableString(selectedledgers));
                Session["CHECKED_ITEMS"] = null;
                Session["CHECKED_ITEMS_LEDGER"] = null;

                BindData();
                BindDataBankLedgerline();
                BindCurrentBankStatement();

                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1', null, null);";
                Script += "</script>" + "\n";


                // Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

                //String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
                //Session["New"] = "Y";
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
        }

        if (CommandName.ToUpper().Equals("VALIDATE"))
        {

            String scriptpopup = String.Format(
                "javascript:Openpopup('att', '', '../Reports/ReportsView.aspx?applicationcode=5&reportcode=BANKRECONCILATION&showmenu=0&Uploadid=" + Request.QueryString["uploadid"].ToString() + "');");

            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
        }

        //if (dce.CommandName.ToUpper().Equals("VALIDATE"))
        //{
        //    Response.Redirect("../Reports/ReportsView.aspx?&applicationcode=5&reportcode=BANKRECONCILATION&showmenu=0&Uploadid=" + Request.QueryString["uploadid"].ToString(), false);
        //}

        // Bankstatementselectedlineallocate(string Bankstatementlineitemidlist, string Bankvoucherlineitemidlist)
    }

    protected void MenuAttachment_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                //SetPageNavigator();
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        decimal dTotalAmount = 0;
        DataSet ds = new DataSet();



        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentBankStatementUploadSelection;

        ds = PhoenixAccountsBankStatementReconUpload.BankStatementReconLineItemList(General.GetNullableGuid(ViewState["uploadid"].ToString()),
                                                                                    General.GetNullableDecimal(txtAmountfrom.Text),
                                                                                    General.GetNullableDecimal(txtAmountTo.Text),
                                                                                    General.GetNullableDateTime(txtDateFrom.Text),
                                                                                    General.GetNullableDateTime(txtDateTo.Text),
                                                                                    txtTTref.Text,
                                                                                    sortexpression, sortdirection,
                                                                                    (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount, ref dTotalAmount);

        string[] alColumns = { "FLDTTREFERENCE", "FLDAMOUNT", "FLDVALUEDATE", "FLDBANKVOUCHERNUMBER", "FLDNARRATIVE" };
        string[] alCaptions = { "Ledger TT Ref", "Amount", "Value Date", "Customer Ref.", "Narrative" };

        General.SetPrintOptions("gvBankStatement", "Bank Statement line", alCaptions, alColumns, ds);



        //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //{
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Match and Allocate", "MATCH");
            MenutravelInvoice.AccessRights = this.ViewState;
            MenutravelInvoice.MenuList = toolbar.Show();
            gvBankStatement.DataSource = ds;
            gvBankStatement.DataBind();
      //  }
        //else if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
        //{
            //MenutravelInvoice.Visible = false;
            ViewState["VALIDATEFLAG"] = 1;
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("Validate", "VALIDATE");
            MenutravelInvoice.AccessRights = this.ViewState;
            MenutravelInvoice.MenuList = toolbar1.Show();
        //    ShowNoRecordsFound(ds.Tables[0], gvBankStatement);
        //}
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvBankStatement_RowDataBound(object sender, GridItemEventArgs e)
    {

     

        if (e.Item is GridDataItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                if (e.Item is GridDataItem)
                {

                    //Label lblexceluploadId = (Label)e.Row.FindControl("lblexceluploadId");
                    ImageButton cmdNew = (ImageButton)e.Item.FindControl("cmdNew");
                    if (cmdNew != null)
                    {
                        if (drv["FLDAMOUNT"] != null && drv["FLDAMOUNT"].ToString() != "")
                        {
                            if (decimal.Parse(drv["FLDAMOUNT"].ToString()) < 0)
                            {
                                cmdNew.Attributes.Add("onclick", "javascript:Openpopup('filter', '', '../Accounts/AccountsBankPaymentVoucher.aspx?type=1&Lineitemid=" + drv["FLDLINEITEMID"].ToString() + "&ValueDate=" + drv["FLDVALUEDATE"].ToString() + "&TTRef=" + drv["FLDTTREFERENCE"].ToString() + "&Narrative=" + drv["FLDNARRATIVE"].ToString() + "&BankAccount=" + drv["FLDBANKACCOUNTID"].ToString() + "&Amount=" + drv["FLDAMOUNT"].ToString() + "');return false;");
                            }
                            if (decimal.Parse(drv["FLDAMOUNT"].ToString()) > 0)
                            {
                                cmdNew.Attributes.Add("onclick", "javascript:Openpopup('filter', '', '../Accounts/AccountsBankReceiptVoucher.aspx?type=1&Lineitemid=" + drv["FLDLINEITEMID"].ToString() + "&ValueDate=" + drv["FLDVALUEDATE"].ToString() + "&TTRef=" + drv["FLDTTREFERENCE"].ToString() + "&Narrative=" + drv["FLDNARRATIVE"].ToString() + "&BankAccount=" + drv["FLDBANKACCOUNTID"].ToString() + "&Amount=" + drv["FLDAMOUNT"].ToString() + "');return false;");
                            }
                        }                        

                    }

                }
                //ImageButton cmdLineItems = (ImageButton)e.Row.FindControl("cmdLineItems");
                //if (cmdLineItems != null)
                //{
                //    DataRowView drv = (DataRowView)e.Row.DataItem;
                //    //ed.Visible = false;
                //    cmdLineItems.Attributes["onclick"] = "javascript:Openpopup('NATD','','../Accounts/AccountsERMVoucherDetail.aspx?exceluploadid="
                //       + drv["FLDUPLOADID"].ToString() + "'); return false;";
                //}
            }

        }

    }

    protected void gvBankStatement_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            BindData();
            BindDataBankLedgerline();
            BindCurrentBankStatement();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDataBankLedgerline()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentBankStatementUploadSelection;
        ds = PhoenixAccountsBankStatementReconUpload.BankStatementReconBankLedgerList(General.GetNullableGuid(ViewState["uploadid"].ToString()),
                                                                                    sortexpression, sortdirection,
                                                                                    (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount
                                                                                        );

        //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //{

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Match and Allocate", "MATCH");
            MenutravelInvoice.AccessRights = this.ViewState;
            MenutravelInvoice.MenuList = toolbar.Show();
            gvBankLedger.DataSource = ds;
            gvBankLedger.DataBind();
      //  }
        //else if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0 && ViewState["VALIDATEFLAG"].ToString() == "1")
        //{

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("Validate", "VALIDATE");
            MenutravelInvoice.AccessRights = this.ViewState;
            MenutravelInvoice.MenuList = toolbar1.Show();
        //    ShowNoRecordsFound(ds.Tables[0], gvBankLedger);
        //}
        //else
        //{
        //    ShowNoRecordsFound(ds.Tables[0], gvBankLedger);
        //}
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    private void BindCurrentBankStatement()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentBankStatementUploadSelection;
        ds = PhoenixAccountsBankStatementReconUpload.CurrentBankStatementReconList(General.GetNullableGuid(ViewState["uploadid"].ToString()),
                                                                                    sortexpression, sortdirection,
                                                                                    (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount
                                                                                    );

        //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //{
            gvCurrentBankStatement.DataSource = ds;
            gvCurrentBankStatement.VirtualItemCount=iRowCount;
        //}
        //else if (ds.Tables.Count > 0)
        //{
        //    ShowNoRecordsFound(ds.Tables[0], gvCurrentBankStatement);
        //}
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }



    protected void gvBankLedger_RowDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            //// Get the LinkButton control in the first cell
            //LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
            //// Get the javascript which is assigned to this LinkButton
            //string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            //// Add this javascript to the onclick Attribute of the row
            //e.Row.Attributes["ondblclick"] = _jsDouble;
        }
        if (e.Item is GridDataItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                if (e.Item is GridDataItem)
                {

                    //Label lblexceluploadId = (Label)e.Row.FindControl("lblexceluploadId");
                    ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
                    if (cmdEdit != null)
                    {
                        if (drv["FLDVOUCHERTYPEID"].ToString() == "68")
                        {
                            cmdEdit.Attributes.Add("onclick", "javascript:Openpopup('filter', '', '../Accounts/AccountsBankPaymentVoucherMaster.aspx?type=2&voucherid=" + drv["FLDVOUCHERID"].ToString() + "');return false;");
                        }
                        if (drv["FLDVOUCHERTYPEID"].ToString() == "69")
                        {
                            cmdEdit.Attributes.Add("onclick", "javascript:Openpopup('filter', '', '../Accounts/AccountsBankReceiptVoucherMaster.aspx?type=2&voucherid=" + drv["FLDVOUCHERID"].ToString() + "');return false;");
                        }
                    }

                    ImageButton db = (ImageButton)e.Item.FindControl("cmdVouchershowing");
                    if (db != null)
                    {
                        if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                            db.Visible = false;
                    }

                }
                //ImageButton cmdLineItems = (ImageButton)e.Row.FindControl("cmdLineItems");
                //if (cmdLineItems != null)
                //{
                //    DataRowView drv = (DataRowView)e.Row.DataItem;
                //    //ed.Visible = false;
                //    cmdLineItems.Attributes["onclick"] = "javascript:Openpopup('NATD','','../Accounts/AccountsERMVoucherDetail.aspx?exceluploadid="
                //       + drv["FLDUPLOADID"].ToString() + "'); return false;";
                //}
            }

        }
     
    }

    protected void gvBankLedger_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            int type = 1;

            if (e.CommandName.ToUpper().Equals("VOUCHERSHOWING"))
            {
                RadLabel lblLedgerLineitemId = (RadLabel)e.Item.FindControl("lblLedgerLineitemId");
                PhoenixAccountsBankStatementReconUpload.BankStatementReconBankLedgerUpdate(type, new Guid(lblLedgerLineitemId.Text));
                ucStatus.Text = "Lineitem is moved.";
                BindData();
                BindDataBankLedgerline();
                BindCurrentBankStatement();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvCurrentBankStatement_RowDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            //// Get the LinkButton control in the first cell
            //LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
            //// Get the javascript which is assigned to this LinkButton
            //string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            //// Add this javascript to the onclick Attribute of the row
            //e.Row.Attributes["ondblclick"] = _jsDouble;
        }
        if (e.Item is GridDataItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                if (e.Item is GridDataItem)
                {

                    //Label lblexceluploadId = (Label)e.Row.FindControl("lblexceluploadId");
                    ImageButton db = (ImageButton)e.Item.FindControl("cmdVouchernotshowing");
                    if (db != null)
                    {
                        if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                            db.Visible = false;
                    }

                }

            }

        }
      
    }

    protected void gvCurrentBankStatement_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            int type = 0;

            if (e.CommandName.ToUpper().Equals("VOUCHERNOTSHOWING"))
            {
                RadLabel lblLineitemId = (RadLabel)e.Item.FindControl("lblLineitemId");
                PhoenixAccountsBankStatementReconUpload.CurrentBankStatementReconUpdate(type, new Guid(lblLineitemId.Text));
                ucStatus.Text = "Lineitem is moved.";
                BindData();
                BindDataBankLedgerline();
                BindCurrentBankStatement();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BankStatementSaveCheckedValues(Object sender, EventArgs e)
    {
        ArrayList SelectedPvs = new ArrayList();
        string index = "";
        foreach (GridDataItem gvrow in gvBankStatement.Items)
        {
            bool result = false;
            index = gvBankStatement.Items[gvrow.RowIndex].ToString();
            if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
            {
                result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
            }

            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];
            if (result)
            {
                if (!SelectedPvs.Contains(index))
                    SelectedPvs.Add(index);
            }
            else
                SelectedPvs.Remove(index);
        }
        if (SelectedPvs != null && SelectedPvs.Count > 0)
            Session["CHECKED_ITEMS"] = SelectedPvs;
    }


    protected void BankLedgerSaveCheckedValues(Object sender, EventArgs e)
    {
        ArrayList SelectedPvs = new ArrayList();
        string index = "";
        foreach (GridDataItem gvrow in gvBankLedger.Items)
        {
            bool result = false;
            index = gvBankLedger.Items[gvrow.RowIndex].ToString();
            if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
            {
                result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
            }

            // Check in the Session
            if (Session["CHECKED_ITEMS_LEDGER"] != null)
                SelectedPvs = (ArrayList)Session["CHECKED_ITEMS_LEDGER"];
            if (result)
            {
                if (!SelectedPvs.Contains(index))
                    SelectedPvs.Add(index);
            }
            else
                SelectedPvs.Remove(index);
        }
        if (SelectedPvs != null && SelectedPvs.Count > 0)
            Session["CHECKED_ITEMS_LEDGER"] = SelectedPvs;
    }

    //private void ShowNoRecordsFound(DataTable dt, GridView gv)
    //{
    //    dt.Rows.Add(dt.NewRow());
    //    gv.DataSource = dt;
    //    gv.DataBind();

    //    int colcount = gv.Columns.Count;
    //    gv.Rows[0].Cells.Clear();
    //    gv.Rows[0].Cells.Add(new TableCell());
    //    gv.Rows[0].Cells[0].ColumnSpan = colcount;
    //    gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    //    gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
    //    gv.Rows[0].Cells[0].Font.Bold = true;
    //    gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    //    gv.Rows[0].Attributes["ondblclick"] = "";
    //}


    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        decimal dTotalAmount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDTTREFERENCE", "FLDAMOUNT", "FLDVALUEDATE", "FLDBANKVOUCHERNUMBER", "FLDNARRATIVE" };
        string[] alCaptions = { "Ledger TT Ref", "Amount", "Value Date", "Customer Ref.", "Narrative" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsBankStatementReconUpload.BankStatementReconLineItemList(General.GetNullableGuid(ViewState["uploadid"].ToString()),
                                                                                   General.GetNullableDecimal(txtAmountfrom.Text),
                                                                                   General.GetNullableDecimal(txtAmountTo.Text),
                                                                                   General.GetNullableDateTime(txtDateFrom.Text),
                                                                                   General.GetNullableDateTime(txtDateTo.Text),
                                                                                   txtTTref.Text,
                                                                                   sortexpression, sortdirection,
                                                                                   (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount, ref dTotalAmount);


        strTransactionAmountTotal = String.Format("{0:n}", dTotalAmount);


        Response.AddHeader("Content-Disposition", "attachment; filename=BankandLedger.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Bank and Ledger</h3></td>");
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

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
        BindData();
        BindDataBankLedgerline();
        BindCurrentBankStatement();
    }


    protected void gvBankStatement_Sorting(object sender, GridSortCommandEventArgs e)
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

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int result;
    //        if (Int32.TryParse(txtnopage.Text, out result))
    //        {
    //            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


    //            if (0 >= Int32.Parse(txtnopage.Text))
    //                ViewState["PAGENUMBER"] = 1;

    //            if ((int)ViewState["PAGENUMBER"] == 0)
    //                ViewState["PAGENUMBER"] = 1;

    //            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //        }
    //        BindData();
    //        SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    try
    //    {
    //        gvBankStatement.SelectedIndex = -1;
    //        gvBankStatement.EditIndex = -1;
    //        if (ce.CommandName == "prev")
    //            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //        else
    //            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //        BindData();
    //        SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //private void SetPageNavigator()
    //{
    //    try
    //    {
    //        cmdPrevious.Enabled = IsPreviousEnabled();
    //        cmdNext.Enabled = IsNextEnabled();
    //        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //private Boolean IsPreviousEnabled()
    //{

    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //    {
    //        return true;
    //    }

    //    return false;

    //}

    //private Boolean IsNextEnabled()
    //{

    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    protected void gvBankStatement_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBankStatement.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBankLedger_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBankLedger.CurrentPageIndex + 1;
            BindDataBankLedgerline();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCurrentBankStatement_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCurrentBankStatement.CurrentPageIndex + 1;
            BindCurrentBankStatement();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
