using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;


public partial class VesselAccountsPhoneCardPriceCorrection :  PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //PhoenixToolbar toolbargrid = new PhoenixToolbar();
            //toolbargrid.AddImageButton("../VesselAccounts/VesselAccountsPhoneCardRequisitionLineItem.aspx", "Export to Excel", "icon_xls.png", "Excel");
            //toolbargrid.AddImageLink("javascript:CallPrint('gvPhoneReqLine')", "Print Grid", "icon_print.png", "PRINT");
            //MenuPhoneReq.AccessRights = this.ViewState;
            //MenuPhoneReq.MenuList = toolbargrid.Show();
            //MenuPhoneReq.SetTrigger(pnlPhonReqLine);
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
                ViewState["REQUESTID"] = Request.QueryString["REQUESTID"];
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["REQSTATUS"] = "";
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void ShowExcel()
    //{
    //    try
    //    {
    //        int iRowCount = 0;
    //        int iTotalPageCount = 0;
    //        string[] alColumns = { "FLDEMPNAME", "FLDNAME", "FLDQUANTITY", "FLDISSUECARDNO" };
    //        string[] alCaptions = { "Employee", "Card Type", "Quantity", "Card Numbers" };
    //        string sortexpression;
    //        int? sortdirection = null;

    //        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
    //        if (ViewState["SORTDIRECTION"] != null)
    //            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

    //        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
    //            iRowCount = 10;
    //        else
    //            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

    //        NameValueCollection nvc = Filter.CurrentVesselPhoneCardRequestFilter;
    //        DataSet ds = PhoenixVesselAccountsPhoneCardRequisition.SearchPhoneCradRequestLineItem(new Guid(ViewState["REQUESTID"].ToString())
    //                                  , sortexpression, sortdirection
    //                                  , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
    //                                  , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
    //        string title = "Phone Card Requisition ";
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            title += "<br/> Request No : " + ds.Tables[0].Rows[0]["FLDREFERENCENO"].ToString() + "<br/> Request Date : " + DateTime.Parse(ds.Tables[0].Rows[0]["FLDREQUESTDATE"].ToString()).ToString("dd/MM/yyyy");
    //        }
    //        General.ShowExcel(title, ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void MenuPhoneReq_TabStripCommand(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
    //        if (dce.CommandName.ToUpper().Equals("FIND"))
    //        {
    //            ViewState["PAGENUMBER"] = 1;
    //            BindData();
    //        }
    //        else if (dce.CommandName.ToUpper().Equals("EXCEL"))
    //        {
    //            ShowExcel();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDEMPNAME", "FLDNAME", "FLDQUANTITY", "FLDISSUECARDNO" };
            string[] alCaptions = { "Employee", "Card Type", "Quantity", "Card Numbers" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            NameValueCollection nvc = Filter.CurrentVesselPhoneCardRequestFilter;
            DataSet ds = PhoenixVesselAccountsPhoneCardRequisition.SearchPhoneCradRequestLineItem(new Guid(ViewState["REQUESTID"].ToString())
                             , sortexpression, sortdirection
                             , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                             , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
            string title = "Phone Card Requisition ";
            if (ds.Tables[0].Rows.Count > 0)
            {
                title += "<br/> Request No : " + ds.Tables[0].Rows[0]["FLDREFERENCENO"].ToString() + "<br/> Request Date : " + DateTime.Parse(ds.Tables[0].Rows[0]["FLDREQUESTDATE"].ToString()).ToString("dd/MM/yyyy");
            }
            General.SetPrintOptions("gvPhoneReqLine", title, alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvPhoneReqLine.DataSource = ds;
                gvPhoneReqLine.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvPhoneReqLine);

            }
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

    protected void gvPhoneReqLine_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["STORETYPEID"] = null;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
    }

    protected void gvPhoneReqLine_RowDataBound(object sender, GridViewRowEventArgs e)
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
                ImageButton up = (ImageButton)e.Row.FindControl("cmdEdit");
                if (up != null)
                    up.Visible = false;
                UserControlVesselEmployee empl = ((UserControlVesselEmployee)e.Row.FindControl("ddlEmployeeEdit"));
                if (empl != null)
                {
                    empl.bind();
                    empl.SelectedEmployee = drv["FLDEMPLOYEEID"].ToString();
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvPhoneReqLine_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
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

    protected void gvPhoneReqLine_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

                Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
                UserControlMaskNumber txtPrice = (UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtUnitPrice");
                if (!IsValidPrice(txtPrice.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsCorrections.UpdateVesselAccountPhoneCardPrice(PhoenixSecurityContext.CurrentSecurityContext.VesselID, id, decimal.Parse(txtPrice.Text));
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

    protected void gvPhoneReqLine_RowEditing(object sender, GridViewEditEventArgs e)
    {
        BindData();
    }

    protected void gvPhoneReqLine_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView _gridView = (GridView)sender;

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
        BindData();

    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvPhoneReqLine.SelectedIndex = -1;
            gvPhoneReqLine.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            ViewState["REQUESTLINEID"] = null;
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
            gvPhoneReqLine.EditIndex = -1;
            gvPhoneReqLine.SelectedIndex = -1;
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
            ucError.ErrorMessage = "Unit Price is Required";
        }
        return (!ucError.IsError);
    }

}
