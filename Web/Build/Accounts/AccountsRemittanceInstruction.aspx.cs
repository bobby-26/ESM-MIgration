using System;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class AccountsRemittanceInstruction : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            PhoenixToolbar toolbarmain = new PhoenixToolbar();           
            toolbarmain.AddButton("Export to CSV", "EXPORTCSV");
            MenuRemittanceInstructionMain.AccessRights = this.ViewState;
            MenuRemittanceInstructionMain.MenuList = toolbarmain.Show();


            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsRemittanceInstruction.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvRemittenceInstruction')", "Print Grid", "icon_print.png", "PRINT");
            //toolbargrid.AddImageButton("../Accounts/AccountsRemittanceFilter.aspx", "Find", "search.png", "FIND");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
            MenuOrderForm.SetTrigger(pnlRemittanceInstruction);
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["REMITTENCEID"] = null;
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

    protected void gvRemittenceInstruction_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;
        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
        SetPageNavigator();
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("NEW"))
            {

            }
            if (dce.CommandName.ToUpper().Equals("REMITTANCE"))
            {
                Response.Redirect("../Accounts/AccountsRemittanceLineItem.aspx", true);
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
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = { "Remittance Number", "Remittance Date", "Account Code", "Account Description", "Currency" };
        string[] alColumns = { "FLDREMITTANCENUMBER", "FLDCREATEDDATE", "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDCURRENCYCODE" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsRemittance.RemittanceInstructionSearch(null, "", null, null, null, null, null
                                                   , sortexpression, sortdirection
                                                   , (int)ViewState["PAGENUMBER"]
                                                   , General.ShowRecords(null)
                                                   , ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=AccountRemittenceInstruction.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Accounts Remittance Instruction</h3></td>");
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

    protected void MenuRemittanceInstructionMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("APPROVE"))
            {

                PhoenixAccountsRemittance.PostRemittance(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                          , ViewState["Remittenceid"].ToString(), PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

            }
            if (dce.CommandName.ToUpper().Equals("EXPORTCSV"))
            {
                string remittanceid = (ViewState["Remittenceid"] == null) ? null : (ViewState["Remittenceid"].ToString());
                DataSet ds = new DataSet();
                int iCurrentrow = 0;
                ds = PhoenixAccountsRemittance.RemittanceInstructionSupplierPayableAmountCSV(string.Empty);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    while (iCurrentrow < ds.Tables[0].Rows.Count)
                    {
                       // string strpath = HttpContext.Current.Request.MapPath("~/Attachments/ACCOUNTS/");
                        string strpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/ACCOUNTS/";
                        string filename = strpath + ds.Tables[0].Rows[iCurrentrow]["FLDBANKACCOUNTNUMBER"].ToString().Replace("/", "-") + "_" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
                        CreateCSVFile(ds.Tables[0].Rows[iCurrentrow]["FLDBANKACCOUNTNUMBER"].ToString(), filename);
                        InsertSupplierBankPayment(ds.Tables[1]);
                        iCurrentrow++;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (Filter.CurrentRemittenceSelection != null)
        {
            NameValueCollection nvc = Filter.CurrentRemittenceSelection;
            ds = PhoenixAccountsRemittance.RemittanceInstructionSearch(null, General.GetNullableString(nvc.Get("txtRemittenceNumberSearch").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("ddlAccountCode").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("ddlCurrencyCode").ToString().Trim())
                    , General.GetNullableString(nvc.Get("txtVoucherFromdateSearch").ToString().Trim())
                    , General.GetNullableString(nvc.Get("txtVoucherTodateSearch").ToString().Trim()), null
                    , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        }
        else
        {
            ds = PhoenixAccountsRemittance.RemittanceInstructionSearch(null, "", null, null, null, null, null
                                                       , sortexpression, sortdirection
                                                       , (int)ViewState["PAGENUMBER"]
                                                       , General.ShowRecords(null)
                                                       , ref iRowCount, ref iTotalPageCount);
        }
        string[] alCaptions = { "Remittance Number", "Remittance Date", "Account Code", "Account Description", "Currency" };
        string[] alColumns = { "FLDREMITTANCENUMBER", "FLDCREATEDDATE", "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDCURRENCYCODE" };
        General.SetPrintOptions("gvRemittenceInstruction", "RemittanceInstruction", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvRemittenceInstruction.DataSource = ds;
            gvRemittenceInstruction.DataBind();
            gvRemittenceInstruction.SelectedIndex = 0;
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvRemittenceInstruction);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            gvRemittenceInstruction.SelectedIndex = -1;
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

    protected void gvRemittenceInstruction_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvRemittenceInstruction_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvRemittenceInstruction_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvRemittenceInstruction_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
                Label lblRemittenceId = (Label)e.Row.FindControl("lblRemittenceId");
                cmdEdit.Attributes.Add("onclick", "Openpopup('AddAddress', '', 'AccountsRemittanceInstructionLineItem.aspx?REMITTENCEID=" + lblRemittenceId.Text + "&MODE=EDIT'); return false;");


                LinkButton lnkRemittenceid = (LinkButton)e.Row.FindControl("lnkRemittenceid");
                lnkRemittenceid.Attributes.Add("onclick", "Openpopup('AddAddress', '', 'AccountsRemittanceInstructionLineItem.aspx?REMITTENCEID=" + lblRemittenceId.Text + "&MODE=EDIT'); return false;");
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
                if (cmdEdit != null)
                    if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
            }
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
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


    private void InsertSupplierBankPayment(DataTable dt)
    {
        foreach (DataRow dr in dt.Rows)
        {
            PhoenixAccountsBankSupplierPayment.InsertBankSupplierPayment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null,
                            dr[1].ToString(), null,
                            dr[3].ToString(), dr[4].ToString(),
                            null, null,
                            General.GetNullableDateTime(dr[6].ToString()), dr[7].ToString(),
                            General.GetNullableDecimal(dr[8].ToString()), null);
        }
    }


    public void CreateCSVFile(string strBankAccountnumber, string strFilePath)
    {

        DataSet ds = new DataSet();

        ds = PhoenixAccountsRemittance.RemittanceInstructionSupplierPayableAmountCSV(strBankAccountnumber);

        DataTable dt = new DataTable();
        dt = ds.Tables[1];
        DataRow dr = dt.NewRow();

        dr[0] ="Transaction Type";
        dr[1] ="Beneficiary Bank Country Code";
        dr[2] = "Debit Account Number";
        dr[3] = "Payment Currency";
        dr[4] = "Transaction Amount";
        dr[5] = "Debit Account Currency";
        dr[6] = "Amount in Debit Account Currency";
        dr[7] = "Value Date (yyyymmdd)";
        dr[8] = "Charges";
        dr[9] = "Customer Reference";
        dr[10] = "Beneficiary Bank Identifier Type";
        dr[11] = "Beneficiary Bank ID or SWIFT Code";
        dr[12] = "Beneficiary Bank Name";
        dr[13] = "Beneficiary Bank Address Line 1";
        dr[14] = "Beneficiary Bank Address Line 2";
        dr[15] = "Beneficiary Bank Address Line 3";
        dr[16] = "Beneficiary Address Line 1";
        dr[17] = "Beneficiary Name";
        dr[18] = "Beneficiary Address Line 2";
        dr[19] = "Beneficiary Address Line 3";
        dr[20] = "Beneficiary Account Number";
        dr[21] = "RFB";
        dr[22] = "RFB";
        dr[23] = "RFB";
        dr[24] = "RFB";
        dr[25] = "REC";
        dr[26] = "REC";
        dr[27] = "REC";
        dr[28] = "REC";
        dr[29] = "REC";
        dr[30] = "Intermediary bank account number";
        dr[31] = "Intermediary Bank Country Code";
        dr[32] = "Intermediary Bank Identifier Type";
        dr[33] = "Intermediary Bank ID or SWIFT Code";
        dr[34] = "Intermediary Bank Name";
        dr[35] = "Intermediary Bank Address Line 1";
        dr[36] = "Intermediary Bank Address Line 2";
        dr[37] = "Intermediary Bank Address Line 3";
        dr[38] = "Email Add-1";
        dr[39] = "Email Add-2";
        dr[40] = "Email Add-3";
        dr[41] = "Exchange Contract Number";
        dr[42] = "Invoice No";
        dr[43] = "Date";
        dr[44] = "Amount";
        dr[45] = "Remark";
    


        StreamWriter sw = new StreamWriter(strFilePath, false);
        int iColCount = dt.Columns.Count;

        // write the headers.
        for (int i = 1; i < iColCount; i++)
        {
            sw.Write(dt.Columns[i]);
            if (i < iColCount - 1)
            {
                sw.Write(",");
            }
        }
        sw.Write(sw.NewLine);

        //write all the rows.

        foreach (DataRow dr1 in dt.Rows)
        {
            for (int i = 1; i < iColCount; i++)
            {
                if (!Convert.IsDBNull(dr[i]))
                {
                    sw.Write(dr1[i].ToString());
                }
                if (i < iColCount - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
        }
        sw.Close();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvRemittenceInstruction.SelectedIndex = -1;
            gvRemittenceInstruction.EditIndex = -1;
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

    protected void gvRemittenceInstruction_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            int iRowno;
            iRowno = int.Parse(e.CommandArgument.ToString());
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }
}
