using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;

public partial class AccountsPaymentVoucherVesselVisitTravelClaim : PhoenixBasePage
{
    public string strAmountTotal = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (Request.QueryString["voucherid"] != null && Request.QueryString["voucherid"] != string.Empty)
                ViewState["voucherid"] = Request.QueryString["voucherid"];
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Voucher", "VOUCHER");
            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBERADV"] = 1;
                ViewState["PAGENUMBEROTHERADV"] = 1;
                ViewState["PVStatuscode"] = null;
                ViewState["PVType"] = null;
                ViewState["SuppCode"] = null;

                ViewState["callfrom"] = null;
            }

            PhoenixToolbar toolbarLine = new PhoenixToolbar();
                toolbarLine.AddImageButton("../Accounts/AccountsPaymentVoucherVesselVisitTravelClaim.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbarLine.AddImageLink("javascript:CallPrint('gvLineItem')", "Print Grid", "icon_print.png", "PRINT");
                MenuTravelClaim.AccessRights = this.ViewState;
                MenuTravelClaim.MenuList = toolbarLine.Show();

                PhoenixToolbar toolbarAdv = new PhoenixToolbar();
                toolbarAdv.AddImageButton("../Accounts/AccountsPaymentVoucherVesselVisitTravelClaim.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbarAdv.AddImageLink("javascript:CallPrint('gvTravelAdvance')", "Print Grid", "icon_print.png", "PRINT");
                MenuTravelAdvance.AccessRights = this.ViewState;
                MenuTravelAdvance.MenuList = toolbarAdv.Show();

                PhoenixToolbar toolbarAdvOther = new PhoenixToolbar();
                toolbarAdvOther.AddImageButton("../Accounts/AccountsPaymentVoucherVesselVisitTravelClaim.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbarAdvOther.AddImageLink("javascript:CallPrint('gvTravelAdvanceOther')", "Print Grid", "icon_print.png", "PRINT");
                MenuTravelAdvanceOther.AccessRights = this.ViewState;
                MenuTravelAdvanceOther.MenuList = toolbarAdvOther.Show();
          
            if ((Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"] != string.Empty))
                ViewState["callfrom"] = Request.QueryString["callfrom"];

            PaymentVoucherEdit();
            BindDataAdv();
            BindDataOtherAdv();

            //if (ViewState["PVStatuscode"].ToString() != "48")
            //{
            //    string vouchertype = ViewState["PVType"].ToString() == "239" ? "0" : "1";
            //    cmdApprove.Attributes.Add("onclick", "parent.Openpopup('PaymentVoucherApproval', '', '../Common/CommonApproval.aspx?docid=" + ViewState["voucherid"].ToString() + "&mod=" + PhoenixModule.ACCOUNTS + "&type=" + PhoenixCommonRegisters.GetHardCode(1, 98, "VVA") + "&suppliercode=" + ViewState["SuppCode"].ToString() + "&vouchertype=" + vouchertype + "&user=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "');return false;");
            //}
            //else
            if (ViewState["PVStatuscode"].ToString() == "Approved")
            {
                cmdApprove.Attributes.Add("style", "visibility:hidden");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("VOUCHER"))
            {
                if (ViewState["callfrom"] != null && ViewState["callfrom"].ToString() == "ZEROPV")
                    Response.Redirect("../Accounts/AccountsInvoiceZeroPaymentVoucherMaster.aspx?voucherid=" + ViewState["voucherid"].ToString());
                else
                    Response.Redirect("../Accounts/AccountsInvoicePaymentVoucherMaster.aspx?voucherid=" + ViewState["voucherid"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void PaymentVoucherEdit()
    {
        try
        {
            string[] alColumns = { "FLDFORMNUMBER", "FLDVOUCHERNUMBER", "FLDVOUCHERDATE", "FLDVESSELNAME", "FLDPURPOSE", "FLDCURRENCYCODE", "FLDAMOUNT" };
            string[] alCaptions = { "Vessel Visit Id", "Voucher Number", "Voucher Date", "Vessel Name", "Purpose", "Payable Currency", "Reimbursement Amount" };

            DataSet ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimPaymentVoucherEdit(new Guid(ViewState["voucherid"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtVoucherNo.Text = dr["FLDPAYMENTVOUCHERNUMBER"].ToString();
                ucVoucherDate.Text = dr["FLDCREATEDDATE"].ToString();
                txtEmployeeCode.Text = dr["FLDUSERNAME"].ToString();
                txtEmployeeName.Text = dr["FLDEMPLOYEENAME"].ToString();
                txtCurrency.Text = dr["FLDCURRENCYCODE"].ToString();
                txtpaymentAmount.Text = string.Format(String.Format("{0:#####.00}", dr["FLDAMOUNT"])); ;
                txtPurpose.Text = dr["FLDPURPOSE"].ToString();
                txtStatus.Text = dr["FLDPAYMENTVOUCHERSTATUS"].ToString();
                txtBankName.Text = dr["FLDBANKNAME"].ToString();
                txtBeneficiaryName.Text = dr["FLDACCOUNTNAME"].ToString();
                txtBankAccountNo.Text = dr["FLDACCOUNTNUMBER"].ToString();
                ucFromDate.Text = dr["FLDFROMDATE"].ToString();
                ucToDate.Text = dr["FLDTODATE"].ToString();

                ViewState["SuppCode"] = dr["FLDSUPPLIERCODE"].ToString();
                ViewState["PVStatuscode"] = dr["FLDPAYMENTVOUCHERSTATUS"].ToString();
                ViewState["PVType"] = dr["FLDPAYMENTVOUCHERTYPE"].ToString();

                gvLineItem.DataSource = ds;
                gvLineItem.DataBind();
                strAmountTotal = string.Format(String.Format("{0:#####.00}", dr["FLDAMOUNT"])); ;
                ViewState["VisitId"] = dr["FLDVESSELVISITID"].ToString();
            }
            General.SetPrintOptions("gvLineItem", "Travel Claim ", alCaptions, alColumns, ds);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_ItemDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravelAdvance_ItemDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
    }

    public void BindDataAdv()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            decimal iBalanceInSGD = 0;

            string[] alColumns = { "FLDTRAVELADVANCENUMBER", "FLDVESSELNAME", "FLDREQUESTEDDATE", "FLDREQUESTAMOUNT", "FLDCLAIMAMOUNT", "FLDREMARKS" };
            string[] alCaptions = { "Travel Advance Id", "Vessel", "Date", "Advance Amount", "Amount Allocated", "	Advance Remarks" };

            DataSet ds = new DataSet();

            ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimAdvanceList(new Guid(ViewState["VisitId"].ToString()),
                    (int)ViewState["PAGENUMBERADV"],
                    General.ShowRecords(null),
                    ref iRowCount,
                    ref iTotalPageCount,
                    ref iBalanceInSGD);

            //strBalanceInSGD = String.Format("{0:n}", iBalanceInSGD);
            General.SetPrintOptions("gvTravelAdvance", "Travel Advance", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvTravelAdvance.DataSource = ds;
                gvTravelAdvance.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvTravelAdvance);
            }

            ViewState["ROWCOUNTADV"] = iRowCount;
            ViewState["TOTALPAGECOUNTADV"] = iTotalPageCount;
            SetPageNavigatorAdv();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindDataOtherAdv()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDTRAVELADVANCENUMBER", "FLDVESSELNAME", "FLDREQUESTEDDATE", "FLDREQUESTAMOUNT", "FLDTRAVELCLAIMAMOUNT", "FLDBALANCEAMOUNT", "FLDREMARKS" };
            string[] alCaptions = { "Travel Advance Id", "Vessel", "Date", "Advance Amount", "Amount Allocated", "Balance Amount", "Advance Remarks" };

            DataSet ds = new DataSet();

            ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimAdvanceOtherList(new Guid(ViewState["VisitId"].ToString()),
                    (int)ViewState["PAGENUMBEROTHERADV"],
                    General.ShowRecords(null),
                    ref iRowCount,
                    ref iTotalPageCount);

            General.SetPrintOptions("gvTravelAdvanceOther", "Travel Advance", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvTravelAdvanceOther.DataSource = ds;
                gvTravelAdvanceOther.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvTravelAdvanceOther);
            }

            ViewState["ROWCOUNTOTHERADV"] = iRowCount;
            ViewState["TOTALPAGECOUNTOTHERADV"] = iTotalPageCount;
            SetPageNavigatorOtherAdv();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

    private void SetPageNavigatorAdv()
    {
        cmdPreviousadv.Enabled = IsPreviousEnabledAdv();
        cmdNextadv.Enabled = IsNextEnabledAdv();
        lblPagenumberadv.Text = "Page " + ViewState["PAGENUMBERADV"].ToString();
        lblPagesadv.Text = " of " + ViewState["TOTALPAGECOUNTADV"].ToString() + " Pages. ";
        lblRecordsadv.Text = "(" + ViewState["ROWCOUNTADV"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabledAdv()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERADV"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTADV"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabledAdv()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERADV"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTADV"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    protected void cmdGoAdv_Click(object sender, EventArgs e)
    {
        int result;
        if (Int32.TryParse(txtnopageadv.Text, out result))
        {
            ViewState["PAGENUMBERADV"] = Int32.Parse(txtnopageadv.Text);

            if ((int)ViewState["TOTALPAGECOUNTADV"] < Int32.Parse(txtnopageadv.Text))
                ViewState["PAGENUMBERADV"] = ViewState["TOTALPAGECOUNTADV"];


            if (0 >= Int32.Parse(txtnopageadv.Text))
                ViewState["PAGENUMBERADV"] = 1;

            if ((int)ViewState["PAGENUMBERADV"] == 0)
                ViewState["PAGENUMBERADV"] = 1;

            txtnopageadv.Text = ViewState["PAGENUMBERADV"].ToString();
        }
        BindDataAdv();
    }

    protected void PagerButtonClickadv(object sender, CommandEventArgs ce)
    {
        gvTravelAdvance.SelectedIndex = -1;
        gvTravelAdvance.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBERADV"] = (int)ViewState["PAGENUMBERADV"] - 1;
        else
            ViewState["PAGENUMBERADV"] = (int)ViewState["PAGENUMBERADV"] + 1;
        BindDataAdv();
    }

    protected void gvTravelAdvanceOther_ItemDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
    }

    private void SetPageNavigatorOtherAdv()
    {
        cmdPreviousOtheradv.Enabled = IsPreviousEnableOtherdAdv();
        cmdNextOtheradv.Enabled = IsNextEnabledOtherAdv();
        lblPagenumberOtheradv.Text = "Page " + ViewState["PAGENUMBEROTHERADV"].ToString();
        lblPagesOtheradv.Text = " of " + ViewState["TOTALPAGECOUNTOTHERADV"].ToString() + " Pages. ";
        lblRecordsOtheradv.Text = "(" + ViewState["ROWCOUNTOTHERADV"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnableOtherdAdv()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBEROTHERADV"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTOTHERADV"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabledOtherAdv()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBEROTHERADV"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTOTHERADV"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    protected void cmdGoOtherAdv_Click(object sender, EventArgs e)
    {
        int result;
        if (Int32.TryParse(txtnopageOtheradv.Text, out result))
        {
            ViewState["PAGENUMBEROTHERADV"] = Int32.Parse(txtnopageOtheradv.Text);

            if ((int)ViewState["TOTALPAGECOUNTOTHERADV"] < Int32.Parse(txtnopageOtheradv.Text))
                ViewState["PAGENUMBEROTHERADV"] = ViewState["TOTALPAGECOUNTOTHERADV"];


            if (0 >= Int32.Parse(txtnopageOtheradv.Text))
                ViewState["PAGENUMBEROTHERADV"] = 1;

            if ((int)ViewState["PAGENUMBEROTHERADV"] == 0)
                ViewState["PAGENUMBEROTHERADV"] = 1;

            txtnopageOtheradv.Text = ViewState["PAGENUMBEROTHERADV"].ToString();
        }
        BindDataOtherAdv();
    }

    protected void PagerButtonClickOtheradv(object sender, CommandEventArgs ce)
    {
        gvTravelAdvanceOther.SelectedIndex = -1;
        gvTravelAdvanceOther.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBEROTHERADV"] = (int)ViewState["PAGENUMBEROTHERADV"] - 1;
        else
            ViewState["PAGENUMBEROTHERADV"] = (int)ViewState["PAGENUMBEROTHERADV"] + 1;
        BindDataOtherAdv();
    }

    protected void MenuTravelClaim_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelLine();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuTravelAdvance_TabStripCommand(object sender, EventArgs e)
    {

        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelAdv();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuTravelAdvanceOther_TabStripCommand(object sender, EventArgs e)
    {

        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelOtherAdv();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcelLine()
    {
        string[] alColumns = { "FLDFORMNUMBER", "FLDVOUCHERNUMBER", "FLDVOUCHERDATE", "FLDVESSELNAME", "FLDPURPOSE", "FLDCURRENCYCODE", "FLDAMOUNT"};
        string[] alCaptions = { "Vessel Visit Id", "Voucher Number", "Voucher Date", "Vessel Name", "Purpose", "Payable Currency", "Reimbursement Amount" };

        DataSet ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimPaymentVoucherEdit(new Guid(ViewState["voucherid"].ToString()));
        
        Response.AddHeader("Content-Disposition", "attachment; filename= TravelClaim.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel Claim</h3></td>");
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
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                Response.Write("</td>");

            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void ShowExcelAdv()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        decimal iBalanceInSGD = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDTRAVELADVANCENUMBER", "FLDVESSELNAME", "FLDREQUESTEDDATE", "FLDREQUESTAMOUNT", "FLDCLAIMAMOUNT", "FLDREMARKS"};
        string[] alCaptions = { "Travel Advance Id", "Vessel", "Date", "Advance Amount", "Amount Allocated", "	Advance Remarks"};

        if (ViewState["ROWCOUNTADV"] == null || Int32.Parse(ViewState["ROWCOUNTADV"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNTADV"].ToString());

        ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimAdvanceList(new Guid(ViewState["VisitId"].ToString()),
                    (int)ViewState["PAGENUMBERADV"],
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount,
                    ref iBalanceInSGD);

        Response.AddHeader("Content-Disposition", "attachment; filename= TravelAdvance.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel Advance</h3></td>");
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
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                Response.Write("</td>");

            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void ShowExcelOtherAdv()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDTRAVELADVANCENUMBER", "FLDVESSELNAME", "FLDREQUESTEDDATE", "FLDREQUESTAMOUNT", "FLDTRAVELCLAIMAMOUNT", "FLDBALANCEAMOUNT", "FLDREMARKS" };
        string[] alCaptions = { "Travel Advance Id", "Vessel", "Date", "Advance Amount", "Amount Allocated", "Balance Amount", "Advance Remarks" };
        if (ViewState["ROWCOUNTOTHERADV"] == null || Int32.Parse(ViewState["ROWCOUNTOTHERADV"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNTOTHERADV"].ToString());

        ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimAdvanceOtherList(new Guid(ViewState["VisitId"].ToString()),
                    (int)ViewState["PAGENUMBEROTHERADV"],
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename= TravelAdvance.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel Advance</h3></td>");
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
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                Response.Write("</td>");

            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    private void DirectApproval(int ApprovalType)
    {

        int iApprovalStatusAccounts;
        int? onbehaalf = null;
        string vouchertype = ViewState["PVType"].ToString() == "239" ? "0" : "1";
        DataTable dt = PhoenixCommonApproval.ListApprovalOnbehalf(ApprovalType, PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString(), null);

        if (dt.Rows.Count > 0)
        {
            onbehaalf = General.GetNullableInteger(dt.Rows[0]["FLDUSERCODE"].ToString());
        }
        string Status = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 49, "APP");
        DataTable dt1 = PhoenixCommonApproval.InsertApprovalRecord(ViewState["voucherid"].ToString(), ApprovalType, onbehaalf, int.Parse(Status), ".",int.Parse(ViewState["SuppCode"].ToString()) ,int.Parse(vouchertype));
        iApprovalStatusAccounts = int.Parse(dt1.Rows[0][0].ToString());

        byte bAllApproved = 0;
        DataTable dts = PhoenixCommonApproval.ListApprovalRecord(ViewState["voucherid"].ToString(), ApprovalType, PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString(), null, 1, ref bAllApproved);

        PhoenixCommonApproval.Approve((PhoenixModule)Enum.Parse(typeof(PhoenixModule), PhoenixModule.ACCOUNTS.ToString()), ApprovalType, ViewState["voucherid"].ToString(), iApprovalStatusAccounts, bAllApproved == 1 ? true : false, PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString());

    }

    protected void cmdApprove_OnClientClick(object sender, EventArgs e)
    {
        try
        {
            DirectApproval(int.Parse(PhoenixCommonRegisters.GetHardCode(1, 98, "VVA")));
            PaymentVoucherEdit();
            if (ViewState["PVStatuscode"].ToString() == "48")
            {
                cmdApprove.Attributes.Add("style", "visibility:hidden");
            }           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



}
