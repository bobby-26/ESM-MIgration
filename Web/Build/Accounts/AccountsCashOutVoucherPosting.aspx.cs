using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;


public partial class AccountsCashOutVoucherPosting : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            //PhoenixToolbar toolbargrid = new PhoenixToolbar();
            //toolbargrid.AddImageButton("../Accounts/AccountsCashOutRequest.aspx", "Export to Excel", "icon_xls.png", "Excel");
            //toolbargrid.AddImageLink("javascript:CallPrint('gvCashOut')", "Print Grid", "icon_print.png", "PRINT");
            //toolbargrid.AddImageButton("../Accounts/AccountsCashOutFilter.aspx", "Find", "search.png", "FIND");

            //MenuOrderForm.AccessRights = this.ViewState;
            //MenuOrderForm.MenuList = toolbargrid.Show();
            //MenuOrderForm.SetTrigger(pnlCashOut);

            if (!IsPostBack)
            {
                Session["New"] = "N";
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

    protected void gvCashOut_Sorting(object sender, GridViewSortEventArgs se)
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

        string[] alCaptions = { "Remittance Number", "Supplier Code", "Supplier Name", "Status", "Payment mode", "Bank Charge Basis", "Account Code", "Account Description", "Currency", "Remittance Amount" };
        string[] alColumns = { "FLDREMITTANCENUMBER", "FLDSUPPLIERCODE", "FLDSUPPLIERNAME", "FLDREMITTANCESTATUS", "FLDREMITTANCEPAYMENTMODENAME", "FLDREMITTANCEBANKCHARGENAME", "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDCURRENCYCODE", "FLDREMITTANCEAMOUNT" };


        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsCashOut.CashOutSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, "", null, null, null
                                                   , General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 222, "APT"))
                                                   , sortexpression, sortdirection
                                                   , (int)ViewState["PAGENUMBER"]
                                                   , General.ShowRecords(null)
                                                   , ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=AccountRemittance.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Accounts Remittance</h3></td>");
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

        if (Filter.CurrentCashOutSelection != null)
        {
            NameValueCollection nvc = Filter.CurrentCashOutSelection;
            ds = PhoenixAccountsCashOut.CashOutSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, General.GetNullableString(nvc.Get("txtCashOutNumberSearch").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("ddlCurrencyCode").ToString().Trim())
                    , General.GetNullableString(nvc.Get("txtVoucherFromdateSearch").ToString().Trim())
                    , General.GetNullableString(nvc.Get("txtVoucherTodateSearch").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("ucRemittanceStatus").ToString().Trim())
                    , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
        }
        else
        {

            ds = PhoenixAccountsCashOut.CashOutSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, "", null, null, null
                                                       , General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 222, "APT"))
                                                       , sortexpression, sortdirection
                                                       , (int)ViewState["PAGENUMBER"]
                                                       , General.ShowRecords(null)
                                                       , ref iRowCount, ref iTotalPageCount);
        }

        string[] alCaptions = { "Remittance Number", "Supplier Code", "Supplier Name", "Status", "Payment mode", "Bank Charge Basis", "Account Code", "Account Description", "Currency", "Remittance Amount" };
        string[] alColumns = { "FLDREMITTANCENUMBER", "FLDSUPPLIERCODE", "FLDSUPPLIERNAME", "FLDREMITTANCESTATUS", "FLDREMITTANCEPAYMENTMODENAME", "FLDREMITTANCEBANKCHARGENAME", "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDCURRENCYCODE", "FLDREMITTANCEAMOUNT" };

        General.SetPrintOptions("gvCashOut", "Remittance", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCashOut.DataSource = ds;
            gvCashOut.DataBind();

            if (ViewState["cashpaymentid"] == null)
            {
                ViewState["cashpaymentid"] = ds.Tables[0].Rows[0]["FLDCASHPAYMENTID"].ToString();
                gvCashOut.SelectedIndex = 0;
            }

            SetRowSelection();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvCashOut);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            gvCashOut.SelectedIndex = -1;
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

    protected void gvCashOut_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvCashOut_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvCashOut_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvCashOut_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
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
                ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
                if (att != null)
                {
                    att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                    if (drv["FLDISATTACHMENT"].ToString() == "0")
                        att.ImageUrl = Session["images"] + "/no-attachment.png";
                    att.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                        + PhoenixModule.ACCOUNTS + "'); return false;");
                }

                ImageButton db = (ImageButton)e.Row.FindControl("cmdEdit");
                if (db != null)
                {
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                    db.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','AccountsCashOutPaidDate.aspx?cashpaymentid=" + drv["FLDCASHPAYMENTID"].ToString() + "&r=1');");
                    if (drv["FLDSTATUSID"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 222, "PST")) db.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvCashOut.SelectedIndex = -1;
            gvCashOut.EditIndex = -1;
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

    private void SetRowSelection()
    {
        gvCashOut.SelectedIndex = -1;
        for (int i = 0; i < gvCashOut.Rows.Count; i++)
        {
            if (gvCashOut.DataKeys[i].Value.ToString().Equals(ViewState["cashpaymentid"].ToString()))
            {
                gvCashOut.SelectedIndex = i;
                PhoenixAccountsVoucher.VoucherNumber = ((LinkButton)gvCashOut.Rows[i].FindControl("lnkCashOutid")).Text;
            }
        }
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

    protected void gvCashOut_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                int iRowno;
                iRowno = int.Parse(e.CommandArgument.ToString());
                SetRowSelection();
            }
            if (e.CommandName.ToUpper().Equals("POST"))
            {
                PhoenixAccountsCashOut.CashOutPostUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID, new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblCashPaymentId")).Text));
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
        if (Session["New"].ToString() == "Y")
        {
            gvCashOut.SelectedIndex = 0;
            Session["New"] = "N";
        }
    }
}
