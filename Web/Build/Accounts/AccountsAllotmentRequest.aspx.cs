using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using SouthNests.Phoenix.Accounts;

public partial class AccountsAllotmentRequest : PhoenixBasePage
{
    string strMAL, strSPA, strSOF, strORR, strSLT;
    NameValueCollection nvc = new NameValueCollection();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Request", "REQUEST");
            toolbar.AddButton("Line Item", "LINEITEM");
            MenuRequest.AccessRights = this.ViewState;
            MenuRequest.MenuList = toolbar.Show();
            MenuRequest.SelectedMenuIndex = 0;
            MenuRequest.Visible = true;

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Generate Payment Voucher", "PAYMENYVOUCHER");
            MenuPV.AccessRights = this.ViewState;
            MenuPV.MenuList = toolbarsub.Show();

            if (!IsPostBack)
            {

                PhoenixToolbar toolbargrid = new PhoenixToolbar();

                toolbargrid.AddImageButton("../Accounts/AccountsAllotmentRequest.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbargrid.AddImageLink("javascript:CallPrint('gvAllotment')", "Print Grid", "icon_print.png", "PRINT");
                toolbargrid.AddImageButton("../Accounts/AccountsAllotmentRequest.aspx", "Filter", "search.png", "FIND");
                toolbargrid.AddImageButton("../Accounts/AccountsAllotmentRequest.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

                MenuAllotment.AccessRights = this.ViewState;
                MenuAllotment.MenuList = toolbargrid.Show();

                BindAllotmentType();
                BindYear();
                BindVessel();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["PAGEURL"] = null;

                ViewState["ALLOTMENTTYPE"] = "";
                ViewState["ALLOTMENTID"] = "";
                ViewState["PVVOUCHERYN"] = "0";

                if (Request.QueryString["ALLOTMENTID"] != null)
                    ViewState["ALLOTMENTID"] = Request.QueryString["ALLOTMENTID"].ToString();
                if (Request.QueryString["ALLOTMENTTYPE"] != null)
                    ViewState["ALLOTMENTTYPE"] = Request.QueryString["ALLOTMENTTYPE"].ToString();

                if (Filter.CurrentAllotmentRequestFilter != null)
                {
                    BindFilterData();
                }
                else
                {
                    ClearFilter();
                }

                BindData();
                SetPageNavigator();
            }
            if (ucError.Visible == true)
            {
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
    protected void BindVessel()
    {        
        clearCheckBoxSelection(cblVesselList);
        DataTable dt = PhoenixAccountsAllotmentRequest.AllotmentRequestVesselList();

        cblVesselList.DataSource = dt;
        cblVesselList.DataBind();
    }
    protected void BindYear()
    {
        ddlYear.Items.Clear();

        ListItem li = new ListItem("--Select--", "DUMMY");
        ddlYear.Items.Add(li);

        for (int i = (DateTime.Today.Year - 4); i <= (DateTime.Today.Year); i++)
        {
            li = new ListItem(i.ToString(), i.ToString());
            ddlYear.Items.Add(li);
        }
        ddlYear.DataBind();        
    }
    private void BindAllotmentType()
    {
        ddlAllotmentType.Items.Clear();
        ListItem li = new ListItem("--Select--", "DUMMY");
        ddlAllotmentType.Items.Add(li);
        ddlAllotmentType.AppendDataBoundItems = true;
        DataTable dt = PhoenixAccountsAllotmentRequest.AllotmentTypeList();
        ddlAllotmentType.DataSource = dt;
        ddlAllotmentType.DataBind();
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string[] alColumns = { "FLDREQUESTDATE", "FLDREQUESTNUMBER", "FLDVESSELNAME", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDALLOTMENTTYPENAME", "FLDAMOUNT", "FLDREQUESTSTATUSNAME" };
            string[] alCaptions = { "Request Date", "Request No.", "Vessel Name", "File No.", "Employee Name", "Rank Name", "Allotment Type", "Amount", "Request Staus" };

            string[] alColumns1 = { "FLDREQUESTDATE", "FLDREQUESTNUMBER", "FLDVESSELNAME", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDALLOTMENTTYPENAME", "FLDAMOUNT", "FLDREQUESTSTATUSNAME", "FLDPAYMENTVOUCHERNUMBER", "FLDPAYMENTDATE", "FLDPAYMENTREFERENCE" };
            string[] alCaptions1 = { "Request Date", "Request No.", "Vessel Name", "File No.", "Employee Name", "Rank Name", "Allotment Type", "Amount", "Request Staus", "Payment Voucher", "Payment Date", "Payment Reference" };

            string sortexpression;
            int? sortdirection = null;
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());                 

            if (Filter.CurrentAllotmentRequestFilter != null)
                nvc = Filter.CurrentAllotmentRequestFilter;

            ds = PhoenixAccountsAllotmentRequest.AllotmentRequestSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
               , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDateFrom") : string.Empty)
               , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDateTo") : string.Empty)
               , General.GetNullableString(nvc != null ? nvc.Get("txtFileNo") : string.Empty)
               , General.GetNullableString(nvc != null ? nvc.Get("txtName") : string.Empty)
               , General.GetNullableInteger(nvc != null ? nvc.Get("ucRank") : string.Empty)
               , General.GetNullableString(nvc != null ? nvc.Get("cblVesselList") : string.Empty)
               , General.GetNullableString(nvc != null ? nvc.Get("cblOfficeTypeList") : string.Empty)
               , General.GetNullableInteger(nvc != null ? nvc.Get("ddlMonth") : string.Empty)
               , General.GetNullableInteger(nvc != null ? nvc.Get("ddlYear") : string.Empty)
               , General.GetNullableString(nvc != null ? nvc.Get("ddlAllotmentType") : string.Empty)
               , General.GetNullableInteger(nvc != null ? nvc.Get("ucRequestStatus") : string.Empty)
               , General.GetNullableInteger(nvc != null ? (nvc.Get("chkIsNotPaymentVoucherYN") == null ? "1" : nvc.Get("chkIsNotPaymentVoucherYN")) : "1")
               , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                if (General.GetNullableString(dr["FLDPAYMENTVOUCHERNUMBER"].ToString()) != null)
                {
                    ViewState["PVVOUCHERYN"] = "1";
                }
                else
                {
                    ViewState["PVVOUCHERYN"] = "0";
                }
                if (General.GetNullableGuid(ViewState["ALLOTMENTID"].ToString()) == null)
                {
                    ViewState["ALLOTMENTTYPE"] = dr["FLDALLOTMENTTYPE"].ToString();
                    ViewState["ALLOTMENTID"] = dr["FLDALLOTMENTID"].ToString();

                    gvAllotment.SelectedIndex = 0;
                }
                SetAllotmentTypeHard();
                gvAllotment.DataSource = ds;
                gvAllotment.DataBind();

                SetRowSelection();
            }
            else
            {
                ViewState["ALLOTMENTID"] = "";
                ViewState["ALLOTMENTTYPE"] = "";

                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvAllotment);
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            if (ViewState["PVVOUCHERYN"].ToString() == "0")
                General.SetPrintOptions("gvAllotment", "Allotment Request", alCaptions, alColumns, ds);
            else
                General.SetPrintOptions("gvAllotment", "Allotment Request", alCaptions1, alColumns1, ds);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAllotment_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string allotmenttype = drv["FLDALLOTMENTTYPE"].ToString();

                ImageButton ulb = (ImageButton)e.Row.FindControl("cmdUnlock");
                ImageButton reb = (ImageButton)e.Row.FindControl("cmdReimRec");
                ImageButton slb = (ImageButton)e.Row.FindControl("cmdSideLetter");

                if (allotmenttype == strMAL || allotmenttype == strSPA)
                {
                    if (ulb != null)
                    {
                        string unlockyn = ((Label)e.Row.FindControl("lblUnlockYN")).Text;

                        if (unlockyn == "1")
                        {
                            ulb.Visible = true;
                            ulb.Visible = SessionUtil.CanAccess(this.ViewState, ulb.CommandName);
                            ulb.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to unlock the request?')");
                        }
                        else
                            ulb.Visible = false;
                    }
                }
                ImageButton cbtn = (ImageButton)e.Row.FindControl("cmdCancelRequest");
                Label status = (Label)e.Row.FindControl("lblRequestStatus");

                if (allotmenttype == strORR || allotmenttype == strSLT)
                {
                    if (status != null)
                        if (status.Text.Trim() == PhoenixCommonRegisters.GetHardCode(1, 238, "ACC") || status.Text.Trim() == PhoenixCommonRegisters.GetHardCode(1, 238, "CBA"))
                        {
                            if (cbtn != null)
                            {
                                cbtn.Visible = true;
                                cbtn.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to cancel the request?')");
                            }
                        }
                }

                if (reb != null)
                {
                    //reb.Attributes.Add("onclick", "javascript:Openpopup('Accounts','','../Accounts/AccountsAllotmentRequesPendingReimbRecoveries.aspx?allotmentid=" + drv["FLDALLOTMENTID"].ToString() + "'); return true;");
                    reb.Attributes.Add("onclick", "javascript:Openpopup('Accounts','','../Accounts/AccountsAllotmentRequestAllDetails.aspx?allotmentid=" + drv["FLDALLOTMENTID"].ToString() + "&fileNo=" + drv["FLDFILENO"].ToString() + "'); return true;");
                }
                if (slb != null)
                {
                    slb.Attributes.Add("onclick", "javascript:Openpopup('Accounts','','../Accounts/AccountsAllotmentRequestSideLetter.aspx?allotmentid=" + drv["FLDALLOTMENTID"].ToString() + "'); return true;");
                }
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuRequest_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("LINEITEM"))
            {
                if (General.GetNullableGuid(ViewState["ALLOTMENTID"].ToString()) != null)
                    Response.Redirect("AccountsAllotmentRequestDetails.aspx?ALLOTMENTID=" + ViewState["ALLOTMENTID"].ToString(), false);
                else
                {
                    BindData();
                    ucError.ErrorMessage = "Please select allotment request";
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
    protected void MenuAllotment_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["ALLOTMENTID"] = "";
                ViewState["ALLOTMENTTYPE"] = "";

                nvc.Clear();
                nvc.Add("txtDateFrom", txtDateFrom.Text);
                nvc.Add("txtDateTo", txtDateTo.Text);
                nvc.Add("txtFileNo", txtFileNo.Text);
                nvc.Add("txtName", txtName.Text);
                nvc.Add("ucRank", ucRank.SelectedRank);
                nvc.Add("cblVesselList", getCheckboxListValues(cblVesselList));
                nvc.Add("cblOfficeTypeList", getCheckboxListValues(cblOfficeTypeList));
                nvc.Add("ddlMonth", ddlMonth.SelectedValue);
                nvc.Add("ddlYear", ddlYear.SelectedValue);
                nvc.Add("ddlAllotmentType", ddlAllotmentType.SelectedValue);
                nvc.Add("ucRequestStatus", ucRequestStatus.SelectedHard);
                nvc.Add("chkIsNotPaymentVoucherYN", chkIsNotPaymentVoucherYN.Checked == true ? "1" : "0");
                Filter.CurrentAllotmentRequestFilter = nvc;

                BindData();
                SetPageNavigator();
            }
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                ClearFilter();
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
    protected void MenuPV_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("PAYMENYVOUCHER"))
            {
                string allotmentidlist = ",";
                SelectedAllotment(ref allotmentidlist);
                if (allotmentidlist == ",")
                {
                    ucError.ErrorMessage = "please select aleast one allotment";
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixAccountsAllotmentRequest.AllotmentPaymentVoucherGenerate(allotmentidlist
                        ,null
                        , null
                        );

                    ucStatus.Visible = true;
                    ucStatus.Text = "Payment Voucher generated";

                    BindData();
                    SetPageNavigator();
                }
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string[] alColumns = { "FLDREQUESTDATE", "FLDREQUESTNUMBER", "FLDVESSELNAME", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDALLOTMENTTYPENAME", "FLDAMOUNT", "FLDREQUESTSTATUSNAME" };
            string[] alCaptions = { "Request Date", "Request No.", "Vessel Name", "File No.", "Employee Name", "Rank Name", "Allotment Type", "Amount", "Request Staus" };

            string[] alColumns1 = { "FLDREQUESTDATE", "FLDREQUESTNUMBER", "FLDVESSELNAME", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDALLOTMENTTYPENAME", "FLDAMOUNT", "FLDREQUESTSTATUSNAME", "FLDPAYMENTVOUCHERNUMBER", "FLDPAYMENTDATE", "FLDPAYMENTREFERENCE" };
            string[] alCaptions1 = { "Request Date", "Request No.", "Vessel Name", "File No.", "Employee Name", "Rank Name", "Allotment Type", "Amount", "Request Staus", "Payment Voucher", "Payment Date", "Payment Reference" };

            string sortexpression;
            int? sortdirection = null;
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            if (Filter.CurrentAllotmentRequestFilter != null)
                nvc = Filter.CurrentAllotmentRequestFilter;

            ds = PhoenixAccountsAllotmentRequest.AllotmentRequestSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
              , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDateFrom") : string.Empty)
              , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDateTo") : string.Empty)
              , General.GetNullableString(nvc != null ? nvc.Get("txtFileNo") : string.Empty)
              , General.GetNullableString(nvc != null ? nvc.Get("txtName") : string.Empty)
              , General.GetNullableInteger(nvc != null ? nvc.Get("ucRank") : string.Empty)
              , General.GetNullableString(nvc != null ? nvc.Get("cblVesselList") : string.Empty)
              , General.GetNullableString(nvc != null ? nvc.Get("cblOfficeTypeList") : string.Empty)
              , General.GetNullableInteger(nvc != null ? nvc.Get("ddlMonth") : string.Empty)
              , General.GetNullableInteger(nvc != null ? nvc.Get("ddlYear") : string.Empty)
              , General.GetNullableString(nvc != null ? nvc.Get("ddlAllotmentType") : string.Empty)
              , General.GetNullableInteger(nvc != null ? nvc.Get("ucRequestStatus") : string.Empty)
              , General.GetNullableInteger(nvc != null ? (nvc.Get("chkIsNotPaymentVoucherYN") == null ? "1" : nvc.Get("chkIsNotPaymentVoucherYN")) : "1")
              , sortexpression, sortdirection, 1, General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

            Response.AddHeader("Content-Disposition", "attachment; filename= AllotmentRequest.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
            Response.Write("<h3><center>Allotment Request </center></h3></td>");
            Response.Write("</tr>");
            Response.Write("</TABLE>");
            Response.Write("<br />");
            Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
            Response.Write("<tr>");
            if (ViewState["PVVOUCHERYN"].ToString() == "0")
            {
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
            }
            else
            {
                for (int i = 0; i < alCaptions1.Length; i++)
                {
                    Response.Write("<td width='20%'>");
                    Response.Write("<b>" + alCaptions1[i] + "</b>");
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Response.Write("<tr>");
                    for (int i = 0; i < alColumns1.Length; i++)
                    {
                        Response.Write("<td>");
                        Response.Write(dr[alColumns1[i]]);
                        Response.Write("</td>");
                    }
                    Response.Write("</tr>");
                }
            }
            Response.Write("</TABLE>");
            Response.End();
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
            gvAllotment.SelectedIndex = -1;
            gvAllotment.EditIndex = -1;
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

    protected void gvAllotment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        try
        {
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("UNLOCK"))
            {
                string allotmentid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAllotmentId")).Text;
                string allotmenttype = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAllotmentTypeId")).Text;
                string vesselid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselID")).Text;
                string month = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblMonth")).Text;
                string year = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblYear")).Text;

                PhoenixAccountsAllotmentRequest.AllotmentRequestUnlock(new Guid(allotmentid));

                BindData();

                ucStatus.Visible = true;
                ucStatus.Text = "Allotment Request Unlocked";
            }
            if (e.CommandName.ToUpper().Equals("CANCELREQUEST"))
            {
                string allotmentid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAllotmentId")).Text;
                string allotmenttype = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAllotmentTypeId")).Text;
                string vesselid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselID")).Text;
                string employeeid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeId")).Text;

                PhoenixAccountsAllotmentRequest.AllotmentRequestCancel(new Guid(allotmentid)
                    , int.Parse(vesselid)
                    , int.Parse(employeeid)
                    , int.Parse(allotmenttype));

                BindData();

                ucStatus.Visible = true;
                ucStatus.Text = "Allotment Request Cancelled";
            }
            if (e.CommandName.ToUpper() == "SELECT")
            {
                LinkButton lnkRequestNo = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkRequestNo");
                Label lblAllotmentId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblAllotmentId");                

                Response.Redirect("AccountsAllotmentRequestDetails.aspx?ALLOTMENTID=" + lblAllotmentId.Text, false);
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
    }
    protected void gvAllotment_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
    }   
    private void SetAllotmentTypeHard()
    {
        strMAL = PhoenixCommonRegisters.GetHardCode(1, 239, "MAL");
        strSPA = PhoenixCommonRegisters.GetHardCode(1, 239, "SPA");
        strSOF = PhoenixCommonRegisters.GetHardCode(1, 239, "SOF");
        strORR = PhoenixCommonRegisters.GetHardCode(1, 239, "ORR");
        strSLT = PhoenixCommonRegisters.GetHardCode(1, 239, "SLT");
    }
    private void BindFilterData()
    {
        nvc = Filter.CurrentAllotmentRequestFilter;
        txtDateFrom.Text = nvc != null ? nvc.Get("txtDateFrom") : "";
        txtDateTo.Text = nvc != null ? nvc.Get("txtDateTo") : "";
        txtFileNo.Text = nvc != null ? nvc.Get("txtFileNo") : "";
        txtName.Text = nvc != null ? nvc.Get("txtName") : "";
        ucRank.SelectedRank = nvc != null ? (nvc.Get("ucRank").ToUpper() == "DUMMY" ? "" : nvc.Get("ucRank")) : "DUMMY";
        

        
        
        //ddlVessel.SelectedValue = nvc != null ? nvc.Get("ddlVessel") : "DUMMY";
        string vessellist;
        vessellist = nvc != null ? nvc.Get("cblVesselList") : "";
        SetCheckboxListValues(vessellist, cblVesselList);
        ddlMonth.Text = nvc != null ? nvc.Get("ddlMonth") : "DUMMY";
        ddlYear.Text = nvc != null ? nvc.Get("ddlYear") : "DUMMY";
        ddlAllotmentType.SelectedValue = nvc != null ? nvc.Get("ddlAllotmentType") : "";
        ucRequestStatus.SelectedHard = nvc != null ? (nvc.Get("ucRequestStatus").ToUpper()=="DUMMY"? "" : nvc.Get("ucRequestStatus")) : "DUMMY";
        if (nvc != null)
        {
            if (nvc.Get("chkIsNotPaymentVoucherYN") != "0")
                chkIsNotPaymentVoucherYN.Checked = true;
            else
                chkIsNotPaymentVoucherYN.Checked = false;
        }
        else
        {
            chkIsNotPaymentVoucherYN.Checked = true;
        }
    }
    private void ClearFilter()
    {
        Filter.CurrentAllotmentRequestFilter = null;
        nvc.Clear();
        txtDateFrom.Text = "";        
        clearCheckBoxSelection(cblVesselList);
        clearCheckBoxSelection(cblOfficeTypeList);
        ddlMonth.SelectedValue = "DUMMY";
        ddlYear.SelectedValue = "DUMMY";
        ddlAllotmentType.SelectedValue = "DUMMY";
        ucRequestStatus.SelectedHard = "";
        txtFileNo.Text = "";
        ucRank.SelectedRank = "";
        txtDateFrom.Text = "";
        txtDateTo.Text = "";
        txtName.Text = "";
        chkIsNotPaymentVoucherYN.Checked = true;
    }

    private void SetRowSelection()
    {
        gvAllotment.SelectedIndex = -1;
        for (int i = 0; i < gvAllotment.Rows.Count; i++)
        {
            if (gvAllotment.DataKeys[i].Value.ToString().Equals(ViewState["ALLOTMENTID"].ToString()))
            {
                gvAllotment.SelectedIndex = i;
                ViewState["ALLOTMENTID"] = ((Label)gvAllotment.Rows[gvAllotment.SelectedIndex].FindControl("lblAllotmentId")).Text;
                ViewState["ALLOTMENTTYPEID"] = ((Label)gvAllotment.Rows[gvAllotment.SelectedIndex].FindControl("lblAllotmentTypeId")).Text;
            }
        }
    }

    private void SelectedAllotment(ref string allotmentidlist)
    {
        if (gvAllotment.Rows.Count > 0)
        {
            foreach (GridViewRow row in gvAllotment.Rows)
            {
                Label lblAllotmentId = (Label)row.FindControl("lblAllotmentId");

                CheckBox cb = (CheckBox)row.FindControl("chkItem");

                if (cb.Checked == true)
                {
                    allotmentidlist += lblAllotmentId.Text + ",";
                }
            }
        }
    }
    private string getCheckboxListValues(CheckBoxList cbl)
    {
        string str="";
        foreach (ListItem li in cbl.Items)
        {
            if (li.Selected == true)
                str = str + ',' + li.Value.ToString();
        }
        return str;
    }
    private void SetCheckboxListValues(string list, CheckBoxList cbl)
    {        
        clearCheckBoxSelection(cbl);
        string[] vessel = list.Split(',');
        foreach (string item in vessel)
        {
            if (item.Trim() != "")
            {
                cblVesselList.Items.FindByValue(item).Selected = true;
            }
        }
    }
    private void clearCheckBoxSelection(CheckBoxList cbl)
    {
        foreach (ListItem li in cbl.Items)
        {
            li.Selected = false;
        }
    }
}
