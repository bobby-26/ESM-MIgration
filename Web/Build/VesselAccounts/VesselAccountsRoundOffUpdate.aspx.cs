using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;

public partial class VesselAccountsRoundOffUpdate :  PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {

                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
                ViewState["ORDERID"] = null;
                ViewState["CURRENTTAB"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuBondReq_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void OrderForm_TabStripCommand(object sender, EventArgs e)
    {

    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
    
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            NameValueCollection nvc = Filter.CurrentVesselOrderFormFilter;
            DataSet ds = PhoenixVesselAccountsOrderForm.SearchOrderForm(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                            , nvc != null ? nvc.Get("txtRefNo") : null
                            , General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : string.Empty)
                            , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : string.Empty)
                            , General.GetNullableInteger(nvc != null ? nvc["ddlStockType"] : string.Empty)
                            , General.GetNullableInteger(nvc != null ? nvc["ddlStatus"] : string.Empty)
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvBondReq.DataSource = ds;
                gvBondReq.DataBind();
                if (ViewState["ORDERID"] == null)
                {
                    ViewState["ORDERID"] = ds.Tables[0].Rows[0]["FLDORDERID"].ToString();
                    gvBondReq.SelectedIndex = 0;
                }
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvBondReq);
            }
            SetTabHighlight();
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvBondReq_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = e.NewEditIndex;
        BindData();
    }

    protected void gvBondReq_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = sender as GridView;
            if (e.CommandName.ToUpper().Equals("CORRECT"))
            {
                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
                Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
                TextBox txtPrice = (TextBox)_gridView.Rows[nCurrentRow].FindControl("txtUnitPrice");
                TextBox txtDiscount = (TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDiscount");
                string Orderdate = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOrderDate")).Text;
                string Receiveddate = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtReceivedDate")).Text;
                string CurrencyId = ((UserControlCurrency)_gridView.Rows[nCurrentRow].FindControl("ddlCurrency")).SelectedCurrency;
                if (!IsValidPrice(txtPrice.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                if (CurrencyId == "Dummy")
                {
                    CurrencyId = null;
                }

                PhoenixVesselAccountsCorrections.UpdateVesselAccountRoundOffAmount(PhoenixSecurityContext.CurrentSecurityContext.VesselID, id, decimal.Parse(txtPrice.Text)
                                                                                    , General.GetNullableDecimal(txtDiscount.Text)
                                                                                    , General.GetNullableDateTime(Orderdate)
                                                                                    , General.GetNullableDateTime(Receiveddate)
                                                                                    , General.GetNullableInteger(CurrencyId));
                _gridView.EditIndex = -1;
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                _gridView.EditIndex = Int32.Parse(e.CommandArgument.ToString());
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvBondReq_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
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
    private bool IsValidPrice(string price)
    {
        ucError.HeaderMessage = "Please provide the following required information before Confirm";

        if (!General.GetNullableDecimal(price).HasValue)
        {
            ucError.ErrorMessage = "Rount Off Amount is Required";
        }
        return (!ucError.IsError);
    }
    protected void gvBondReq_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["STORETYPEID"] = null;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        ViewState["ORDERID"] = null;
        BindData();
    }

    protected void gvBondReq_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        GridView _gridView = sender as GridView;
        gvBondReq.SelectedIndex = se.NewSelectedIndex;
        string orderid = ((Label)_gridView.Rows[se.NewSelectedIndex].FindControl("lblOrderId")).Text;
        ViewState["ORDERID"] = orderid;
        BindData();
    }

    protected void gvBondReq_RowDataBound(object sender, GridViewRowEventArgs e)
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
                ImageButton ed = (ImageButton)e.Row.FindControl("cmdCorrect");
                if (ed != null)
                {
                    ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
                }

                ImageButton et = (ImageButton)e.Row.FindControl("cmdEdit");
                if (et != null)
                {
                    et.Visible = SessionUtil.CanAccess(this.ViewState, et.CommandName);
                }

                UserControlCurrency ddlCurrency = (UserControlCurrency)e.Row.FindControl("ddlCurrency");
                if (ddlCurrency != null) ddlCurrency.SelectedCurrency = drv["FLDCURRENCYID"].ToString();

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

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        try
        {
            int result;
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
            ViewState["ORDERID"] = null;
            BindData();
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
            gvBondReq.SelectedIndex = -1;
            gvBondReq.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            ViewState["ORDERID"] = null;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetPageNavigator()
    {

        try
        {
            cmdPrevious.Enabled = IsPreviousEnabled();
            cmdNext.Enabled = IsNextEnabled();
            lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
            lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
            lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

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
        try
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["ORDERID"] = null;
            BindData();
            for (int i = 0; i < gvBondReq.DataKeyNames.Length; i++)
            {
                if (gvBondReq.DataKeyNames[i] == ViewState["ORDERID"].ToString())
                {
                    gvBondReq.SelectedIndex = i;
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void SetTabHighlight()
    {
        try
        {
            DataList dl = (DataList)MenuOrderForm.FindControl("dlstTabs");
            if (dl.Items.Count > 0)
            {
                if (ViewState["CURRENTTAB"].ToString().Trim().Contains("VesselAccountsOrderFormGeneral.aspx"))
                {
                    MenuOrderForm.SelectedMenuIndex = 0;
                }
                else if (ViewState["CURRENTTAB"].ToString().Trim().Contains("VesselAccountsOrderFormLineItem.aspx"))
                {
                    MenuOrderForm.SelectedMenuIndex = 1;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


}
