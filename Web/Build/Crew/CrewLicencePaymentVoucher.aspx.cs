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
public partial class Crew_CrewLicencePaymentVoucher : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Pending Approval", "PENDING");
            toolbarmain.AddButton("Approved Payment Requests", "APPROVED");
            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            MenuOrderFormMain.SelectedMenuIndex = 1;

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Crew/CrewLicencePaymentVoucher.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvAdvancePayment')", "Print Grid", "icon_print.png", "PRINT");
            //toolbargrid.AddImageLink("javascript:Openpopup('Filter','','CrewLicenceRequestsApprovedFilter.aspx'); return false;", "Filter", "search.png", "FIND");
            toolbargrid.AddImageLink("javascript:Openpopup('Filter','','CrewLicenceRequestsApprovedFilter.aspx'); return false;", "Filter", "search.png", "FIND");
            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
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

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVOUCHERNUMBER", "FLDAPPROVEDDATE", "FLDSHORTCODE", "FLDRANKEMPNAME", "FLDVESSELCODE", "FLDJOINEDVESSEL", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDPAYMENTMETHOD", "PAYMENTDATE", "PAYMENTREFERENCE" };
        string[] alCaptions = { "Request Number", "Approved Date", "Bill to Company", "Rank/Crew Name", "Charged Vessel", "Joined Vessel", "Currency", "Amount", "Payment Method", "Payment Date", "Payment Reference" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentSelectedLicencePaymentFilter;

        ds = PhoenixCrewLicencePaymentRequests.CrewLicencePaymentVoucher(
                                                           PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                           , sortexpression
                                                           , sortdirection
                                                           , (int)ViewState["PAGENUMBER"]
                                                           , General.ShowRecords(null)
                                                           , ref iRowCount
                                                           , ref iTotalPageCount
                                                           , (nvc != null) ? General.GetNullableString(nvc.Get("txtVoucherNumber")) : string.Empty
                                                           , (nvc != null) ? General.GetNullableString(nvc.Get("txtLicenceRequestNumber")) : string.Empty
                                                           , (nvc != null) ? General.GetNullableInteger(nvc.Get("ucCurrency")) : null
                                                           , (nvc != null) ? General.GetNullableInteger(nvc.Get("ucAddress")) : null
                                                           , (nvc != null) ? General.GetNullableInteger(nvc.Get("ucComapny")) : null
                                                           , (nvc != null) ? General.GetNullableInteger(nvc.Get("ucVessel")) : null
                                                           , (nvc != null) ? General.GetNullableString(nvc.Get("txtCrewName")) : null
                                                           , (nvc != null) ? General.GetNullableInteger(nvc.Get("ucRank")) : null
                                                           , (nvc != null) ? General.GetNullableDateTime(nvc.Get("ucFromDate")) : null
                                                           , (nvc != null) ? General.GetNullableDateTime(nvc.Get("ucToDate")) : null
                                                           );


        Response.AddHeader("Content-Disposition", "attachment; filename=LicencePaymentVoucher.xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Licence Payment Voucher</h3></td>");
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

        string[] alColumns = { "FLDVOUCHERNUMBER", "FLDAPPROVEDDATE", "FLDSHORTCODE", "FLDRANKEMPNAME", "FLDVESSELCODE", "FLDJOINEDVESSEL", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDPAYMENTMETHOD", "PAYMENTDATE", "PAYMENTREFERENCE" };
        string[] alCaptions = { "Request Number", "Approved Date", "Bill to Company", "Rank/Crew Name", "Charged Vessel", "Joined Vessel", "Currency", "Amount", "Payment Method", "Payment Date", "Payment Reference" };
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentSelectedLicencePaymentFilter;

        ds = PhoenixCrewLicencePaymentRequests.CrewLicencePaymentVoucher(
                                                          PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                          , sortexpression
                                                          , sortdirection
                                                          , (int)ViewState["PAGENUMBER"]
                                                          , General.ShowRecords(null)
                                                          , ref iRowCount
                                                          , ref iTotalPageCount
                                                          , (nvc != null) ? General.GetNullableString(nvc.Get("txtVoucherNumber")) : string.Empty
                                                          , (nvc != null) ? General.GetNullableString(nvc.Get("txtLicenceRequestNumber")) : string.Empty
                                                          , (nvc != null) ? General.GetNullableInteger(nvc.Get("ucCurrency")) : null
                                                          , (nvc != null) ? General.GetNullableInteger(nvc.Get("ucAddress")) : null
                                                          , (nvc != null) ? General.GetNullableInteger(nvc.Get("ucComapny")) : null
                                                          , (nvc != null) ? General.GetNullableInteger(nvc.Get("ucVessel")) : null
                                                          , (nvc != null) ? General.GetNullableString(nvc.Get("txtCrewName")) : null
                                                          , (nvc != null) ? General.GetNullableInteger(nvc.Get("ucRank")) : null
                                                          , (nvc != null) ? General.GetNullableDateTime(nvc.Get("ucFromDate")) : null
                                                          , (nvc != null) ? General.GetNullableDateTime(nvc.Get("ucToDate")) : null
                                                          );


        General.SetPrintOptions("gvAdvancePayment", "Licence Payment Voucher", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvAdvancePayment.DataSource = ds;
            gvAdvancePayment.DataBind();

        }
        else
        {
            ShowNoRecordsFound(ds.Tables[0], gvAdvancePayment);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvAdvancePayment_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            Label lblProcessId = ((Label)e.Row.FindControl("lblProcessId"));

            LinkButton lnk = (LinkButton)e.Row.FindControl("lnkName");
            lnk.Attributes.Add("onclick", "parent.Openpopup('codehelp', '', 'CrewLicenceEmployeeDetails.aspx?pid=" + lblProcessId.Text + "');return false;");
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

    protected void gvAdvancePayment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvAdvancePayment_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvAdvancePayment_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
        SetPageNavigator();
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
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            GridView _gridView = (GridView)sender;

            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToString().ToUpper() == "SELECT")
            {
                _gridView.SelectedIndex = nCurrentRow;

                BindData();
                SetPageNavigator();
            }

            string processid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblProcessId")).Text;
            string joinedvesselid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblJoinedVesselId")).Text;
            string advancepaymentid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAdvancePaymentid")).Text;

            if (e.CommandName.ToUpper().Equals("REVOKE"))
            {
                PhoenixCrewLicencePaymentRequests.RevokeApproval(General.GetNullableGuid(advancepaymentid));

                BindData();
                SetPageNavigator();
            }

            if (e.CommandName.ToUpper().Equals("CHANGE"))
            {
                //if (joinedvesselid == "" || joinedvesselid == null)
                //{
                //    ucError.ErrorMessage = "Joined vessel to be taken.";
                //    ucError.Visible = true;
                //    return;
                //}

                PhoenixCrewLicencePaymentRequests.ChangeChargedVessel(General.GetNullableGuid(processid), General.GetNullableInteger(joinedvesselid));

                BindData();
                SetPageNavigator();
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvAdvancePayment_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvAdvancePayment.SelectedIndex = e.NewSelectedIndex;

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvAdvancePayment_PreRender(object sender, EventArgs e)
    {
        GridDecorator.MergeRows(gvAdvancePayment);
    }

    public class GridDecorator
    {
        public static void MergeRows(GridView gridView)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                string strCurrentBilltoCompany = ((Label)gridView.Rows[rowIndex].FindControl("lblBillToCompany")).Text;
                string strPreviousBilltoCompany = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblBillToCompany")).Text;

                string strCurrentCurrency = ((Label)gridView.Rows[rowIndex].FindControl("lblCurrency")).Text;
                string strPreviousCurrency = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblCurrency")).Text;

                string strCurrentRefNo = ((Label)gridView.Rows[rowIndex].FindControl("lblRequestNumber")).Text;
                string strPreviousRefNo = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblRequestNumber")).Text;

                string strCurrentSupplier = ((Label)gridView.Rows[rowIndex].FindControl("lblSupplierCode")).Text;
                string strPreviousSupplier = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblSupplierCode")).Text;

                string strCurrentApprovedDate = ((Label)gridView.Rows[rowIndex].FindControl("lblApprovedDate")).Text;
                string strPreviousApprovedDate = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblApprovedDate")).Text;

                if (strCurrentBilltoCompany == strPreviousBilltoCompany && strCurrentCurrency == strPreviousCurrency && strCurrentRefNo == strPreviousRefNo && strPreviousSupplier == strCurrentSupplier)
                {
                    row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 :
                    previousRow.Cells[0].RowSpan + 1;
                    previousRow.Cells[0].Visible = false;

                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                    previousRow.Cells[1].RowSpan + 1;
                    previousRow.Cells[1].Visible = false;

                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                    previousRow.Cells[2].RowSpan + 1;
                    previousRow.Cells[2].Visible = false;

                    row.Cells[6].RowSpan = previousRow.Cells[6].RowSpan < 2 ? 2 :
                    previousRow.Cells[6].RowSpan + 1;
                    previousRow.Cells[6].Visible = false;
                    
                }
            }
        }
    }
}
