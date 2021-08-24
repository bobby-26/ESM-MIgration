using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class Crew_CrewLicenceRequestsPayment : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {

        foreach (GridViewRow r in gvAdvancePayment.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvAdvancePayment.UniqueID, "Select$" + r.RowIndex.ToString());
            }
        }

        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Pending Approval", "PENDING");
            toolbarmain.AddButton("Approved Payment Requests", "APPROVED");
            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            MenuOrderFormMain.SelectedMenuIndex = 0;

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Approve", "APPROVE");
            MenuOrderFormSub.AccessRights = this.ViewState;
            MenuOrderFormSub.MenuList = toolbarsub.Show();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Crew/CrewLicenceRequestsPayment.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvAdvancePayment')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Crew/CrewLicenceRequestsPayment.aspx", "Find", "search.png", "FIND");
            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
          
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["BILLTOCOMPANY"] = "";
                ViewState["CURRENCY"] = "";
                ViewState["CONSULATE"] = "";
                ViewState["BANKID"] = "";
                ViewState["PID"] = "";
                ViewState["ADVANCEPAYMENTID"] = "";
                BindData();
                SetPageNavigator();
            }
            
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAdvancePayment_Sorting(object sender, GridViewSortEventArgs se)
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
           if (dce.CommandName.ToUpper().Equals("PENDING"))
            {
                Response.Redirect("../Crew/CrewLicenceRequestsPayment.aspx",true);
            }
            if (dce.CommandName.ToUpper().Equals("APPROVED"))
            {
                Response.Redirect("../Crew/CrewLicencePaymentVoucher.aspx", true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOrderFormSub_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("APPROVE"))
        {
            try
            {
                if (Filter.CurrentLicenceSelectedForPayment == null || Filter.CurrentLicenceSelectedForPayment == "")
                {
                    ucError.ErrorMessage = "Please select atleast one request.";
                    ucError.Visible = true;
                }
                else
                {
                    //PhoenixCrewLicencePaymentRequests.UpdatePaymentOnReceiveofInvoice(Filter.CurrentLicenceSelectedForPayment,General.GetNullableInteger(chkPayment.Checked ? "1" : "0"));

                    string strreceiveinv = chkPayment.Checked == true ? "1" : "0"; 

                    String scriptpopup = String.Format(
                        "javascript:Openpopup('codehelp1', '', 'CrewLicencePaymentRequestsApprove.aspx?ADDRESSCODE=" + ViewState["CONSULATE"].ToString() + "&CURRENCY=" + ViewState["CURRENCY"] + "&RECEIVEINVOICE=" + strreceiveinv + "');");
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
    }

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVOUCHERNUMBER", "FLDSUPPLIERNAME", "FLDSHORTCODE", "FLDREFERENCEDOCUMENT", "FLDNAME", "FLDVESSELNAME", "FLDDOCUMENTNAME", "FLDBANKNAME", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDREIMBURSABLE", "FLDSTATUSNAME", "FLDREMARKS" };
        string[] alCaptions = { "Voucher Number", "Consulate", "Bill to Company", "Request Number", "Rank/Crew Name", "Vessel", "Licence Applied", "Bank", "Currency", "Amount", "Reimbursable to Crew", "Status", "Remarks" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewLicencePaymentRequests.CrewLicenceRequestSearch(
                                                           PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                           , sortexpression
                                                           , sortdirection
                                                           , (int)ViewState["PAGENUMBER"]
                                                           , General.ShowRecords(null)
                                                           , ref iRowCount
                                                           , ref iTotalPageCount
                                                           , null
                                                           , General.GetNullableInteger(ucAddress.SelectedAddress)
                                                           , General.GetNullableInteger(null)
                                                           , General.GetNullableInteger(chkShowAll.Checked == true ? "1" : "0")
                                                           );


        Response.AddHeader("Content-Disposition", "attachment; filename=LicencePayment.xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Licence Payment Requests</h3></td>");
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
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PID"] = "";
                ViewState["ADVANCEPAYMENTID"] = "";
                BindData();
                SetPageNavigator();
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

        string[] alColumns = { "FLDVOUCHERNUMBER", "FLDSUPPLIERNAME", "FLDSHORTCODE", "FLDREFERENCEDOCUMENT", "FLDNAME", "FLDVESSELNAME", "FLDDOCUMENTNAME", "FLDBANKNAME", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDREIMBURSABLE", "FLDSTATUSNAME", "FLDREMARKS" };
        string[] alCaptions = { "Voucher Number","Consulate","Bill to Company", "Request Number", "Rank/Crew Name", "Vessel", "Licence Applied", "Bank" ,"Currency", "Amount", "Reimbursable to Crew", "Status" , "Remarks" };
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCrewLicencePaymentRequests.CrewLicenceRequestSearch(
                                                           PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                           , sortexpression
                                                           , sortdirection
                                                           , (int)ViewState["PAGENUMBER"]
                                                           , General.ShowRecords(null)
                                                           , ref iRowCount
                                                           , ref iTotalPageCount
                                                           , null
                                                           , General.GetNullableInteger(ucAddress.SelectedAddress)
                                                           , General.GetNullableInteger(null)
                                                           , General.GetNullableInteger(chkShowAll.Checked == true ? "1" : "0")
                                                           );


        General.SetPrintOptions("gvAdvancePayment", "Licence Payment Requests", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["ADVANCEPAYMENTID"].ToString() == "")
            {
                ViewState["PID"] = ds.Tables[0].Rows[0]["FLDORDERID"].ToString();
                ViewState["CONSULATE"] = ds.Tables[0].Rows[0]["FLDSUPPLIERCODE"].ToString();
                ViewState["BILLTOCOMPANY"] = ds.Tables[0].Rows[0]["FLDCOMPANYID"].ToString();
                ViewState["CURRENCY"] = ds.Tables[0].Rows[0]["FLDCURRENCY"].ToString();
                ViewState["BANKID"] = ds.Tables[0].Rows[0]["FLDBANKID"].ToString();
                ViewState["ADVANCEPAYMENTID"] = ds.Tables[0].Rows[0]["FLDADVANCEPAYMENTID"].ToString();
                chkPayment.Checked = ds.Tables[0].Rows[0]["FLDRECEIVINGINVOICE"].ToString() == "1" ? true : false;
                gvAdvancePayment.SelectedIndex = 0;
            }
            gvAdvancePayment.DataSource = ds;
            gvAdvancePayment.DataBind();

            SetRowSelection();
            getSelectedRequests();
            
        }
        else
        {
            ShowNoRecordsFound(ds.Tables[0], gvAdvancePayment);
        }
     
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void SetRowSelection()
    {
        gvAdvancePayment.SelectedIndex = -1;
        for (int i = 0; i < gvAdvancePayment.Rows.Count; i++)
        {
            if (gvAdvancePayment.DataKeys[i].Value.ToString().Equals(ViewState["ADVANCEPAYMENTID"].ToString()))
            {
                gvAdvancePayment.SelectedIndex = i;
                ViewState["CONSULATE"] = ((Label)gvAdvancePayment.Rows[i].FindControl("lblCOnsulate")).Text;
                ViewState["BILLTOCOMPANY"] = ((Label)gvAdvancePayment.Rows[i].FindControl("lblBillToCompany")).Text;
                ViewState["CURRENCY"] = ((Label)gvAdvancePayment.Rows[i].FindControl("lblCurrency")).Text;
                ViewState["BANKID"] = ((Label)gvAdvancePayment.Rows[i].FindControl("lblBankid")).Text;
                chkPayment.Checked = ((Label)gvAdvancePayment.Rows[i].FindControl("lblReceivingInvoice")).Text == "1" ? true : false;
            }
        }
    }

    protected void gvAdvancePayment_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GridView _gridview = (GridView)sender;

        int nCurrentRow = _gridview.SelectedIndex;

        ViewState["ADVANCEPAYMENTID"] = ((Label)_gridview.Rows[nCurrentRow].FindControl("lblAdvanceId")).Text;
        ViewState["CONSULATE"] = ((Label)gvAdvancePayment.Rows[nCurrentRow].FindControl("lblCOnsulate")).Text;
        ViewState["BILLTOCOMPANY"] = ((Label)gvAdvancePayment.Rows[nCurrentRow].FindControl("lblBillToCompany")).Text;
        ViewState["CURRENCY"] = ((Label)gvAdvancePayment.Rows[nCurrentRow].FindControl("lblCurrency")).Text;
        ViewState["BANKID"] = ((Label)gvAdvancePayment.Rows[nCurrentRow].FindControl("lblBankid")).Text;
        chkPayment.Checked = ((Label)gvAdvancePayment.Rows[nCurrentRow].FindControl("lblReceivingInvoice")).Text == "1" ? true : false;
    }

    protected void gvAdvancePayment_ItemDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
               
            }
            CheckBox cb;
            cb = (CheckBox)e.Row.FindControl("chkSelection");

            string strconsulate = ((Label)e.Row.FindControl("lblCOnsulate")).Text;
            string strbankid = ((Label)e.Row.FindControl("lblBankid")).Text;
            string billtocompany = ((Label)e.Row.FindControl("lblBillToCompany")).Text;
            string currency = ((Label)e.Row.FindControl("lblCurrency")).Text;
            string status = ((Label)e.Row.FindControl("lblStatus")).Text;

            if (ViewState["CONSULATE"].ToString().Equals(strconsulate) && ViewState["BILLTOCOMPANY"].ToString().Equals(billtocompany)
                        && ViewState["CURRENCY"].ToString().Equals(currency) && ViewState["BANKID"].ToString().Equals(strbankid) && cb != null && status == PhoenixCommonRegisters.GetHardCode(1, 127, "APD"))
                {
                    cb.Visible = true;
                    cb.Checked = true;
                }
                else
                    if (cb != null && status != "629")
                        cb.Visible = false;
                    else
                        cb.Enabled = false;
                //cb.Visible = false;

            Label lblAdvancePaymentId = ((Label)e.Row.FindControl("lblAdvanceId"));
            //ViewState["ADVANCEPAYMENTID"] = ((Label)e.Row.FindControl("lblAdvanceId")).Text;
            LinkButton lnk = (LinkButton)e.Row.FindControl("lnkRequestNumber");
            lnk.Attributes.Add("onclick", "parent.Openpopup('codehelp', '', 'CrewLicencePaymentDatailsEdit.aspx?ADVANCEPAYMENTID=" + lblAdvancePaymentId.Text + "');return false;");

            
        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            gvAdvancePayment.SelectedIndex = -1;
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

    protected void gvAdvancePayment_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
           e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvAdvancePayment, "Select$" + e.Row.RowIndex.ToString(), false);
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
            gvAdvancePayment.SelectedIndex = -1;
            gvAdvancePayment.EditIndex = -1;
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

    protected void gvAdvancePayment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        GridView _gridView = (GridView)sender;

        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

        if (e.CommandName.ToString().ToUpper() == "SELECT")
        {
            _gridView.SelectedIndex = nCurrentRow;

            Label lblPID = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblProcessId"));
            
            Label lblcompanyid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBillToCompany"));
            Label lblCurrency = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCurrency"));
            Label lblConsulate = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCOnsulate"));
            Label lblBankid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBankid"));
            ViewState["PID"] = lblPID.Text;
            ViewState["BILLTOCOMPANY"] = lblcompanyid.Text;
            ViewState["CURRENCY"] = lblCurrency.Text;
            ViewState["CONSULATE"] = lblConsulate.Text;
            ViewState["BANKID"] = lblBankid.Text;

            
            BindData();
            SetPageNavigator();
        }

    }

    protected void chkSelection_OnCheckedChanged(object sender, EventArgs e)
    {
        getSelectedRequests();
    }
    protected void getSelectedRequests()
    {
        string selitems = "";
        Filter.CurrentLicenceSelectedForPayment = "";
        for (int i = 0; i < gvAdvancePayment.Rows.Count; i++)
        {
            CheckBox cb = (CheckBox)gvAdvancePayment.Rows[i].FindControl("chkSelection");
            Label lblproid = (Label)gvAdvancePayment.Rows[i].FindControl("lblProcessId");
            Label lblAdvancepaymentid = (Label)gvAdvancePayment.Rows[i].FindControl("lblAdvanceId");
            if (cb != null && lblAdvancepaymentid != null && (cb.Checked == true) && (cb.Visible == true) && (cb.Enabled == true))
            {
                selitems += lblAdvancepaymentid.Text;
                selitems += ",";
            }
        }
        if (selitems.Length > 0)
        {
            selitems = selitems.Remove(selitems.Length - 1);
            Filter.CurrentLicenceSelectedForPayment = selitems.ToString();
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
