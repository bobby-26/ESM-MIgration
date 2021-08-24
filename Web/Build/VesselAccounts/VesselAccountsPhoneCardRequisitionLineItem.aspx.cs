using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;


public partial class VesselAccountsPhoneCardRequisitionLineItem :  PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../VesselAccounts/VesselAccountsPhoneCardRequisitionLineItem.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvPhoneReqLine')", "Print Grid", "icon_print.png", "PRINT");
            MenuPhoneReq.AccessRights = this.ViewState;
            MenuPhoneReq.MenuList = toolbargrid.Show();
            MenuPhoneReq.SetTrigger(pnlPhonReqLine);
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
                ViewState["REQUESTID"] = Request.QueryString["REQUESTID"];
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["REQSTATUS"] ="";
            }
            BindData();
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
            string[] alColumns = { "FLDEMPNAME", "FLDNAME", "FLDQUANTITY", "FLDISSUECARDNO" };
            string[] alCaptions = { "Employee", "Card Type", "Quantity", "Card Numbers" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

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
            General.ShowExcel(title, ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuPhoneReq_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            else if (dce.CommandName.ToUpper().Equals("EXCEL"))
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

                UserControlVesselEmployee empl = ((UserControlVesselEmployee)e.Row.FindControl("ddlEmployeeEdit"));
                if (empl != null)
                {
                    empl.bind();
                    empl.SelectedEmployee = drv["FLDEMPLOYEEID"].ToString();
                }
                           
                //if (empl != null) empl.Enabled = !General.IsEditableInOffice();
                //if (empl != null) empl.Enabled = General.IsEditableInShip();
               
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                ImageButton ab = (ImageButton)e.Row.FindControl("cmdApprove");
                ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
                Label lbStatus = (Label)e.Row.FindControl("lblStatus");
                Label lbOrderStatus = (Label)e.Row.FindControl("lblOrderStatus");         

                if (db != null)
                {
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    if (!String.IsNullOrEmpty(drv["FLDISSUEDDATE"].ToString())) db.Visible = false;
                    if (drv["FLDACTIVEYN"].ToString() == "0") db.Visible = false;
                }
                if (eb != null)
                {
                    eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                    if (drv["FLDACTIVEYN"].ToString() == "0") eb.Visible = false;
                }
                if (ab != null)
                {
                    ab.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to confirm Issue details?')");
                    ab.Visible = SessionUtil.CanAccess(this.ViewState, ab.CommandName);
                    //if (lbOrderStatus == null || String.IsNullOrEmpty(lbOrderStatus.Text) || lbOrderStatus.Text.Contains("Pen"))
                    //{
                    //    ab.Visible = false;
                    //}
                    //else
                    //{
                    //    ab.Visible = true;
                    //}
                    if (drv["FLDACTIVEYN"].ToString() == "0") ab.Visible = false;
                }
                if (lbStatus != null)
                    ViewState["REQSTATUS"] = lbStatus.Text;
                if (!String.IsNullOrEmpty(ViewState["REQSTATUS"].ToString()) && e.Row.RowState == DataControlRowState.Edit)
                {
                    TextBox txtCard = (TextBox)e.Row.FindControl("txtCardNumberEdit");
                    UserControlMaskNumber txtQty = (UserControlMaskNumber)e.Row.FindControl("txtQuantityEdit");
                    UserControlDate txtissuedate = (UserControlDate)e.Row.FindControl("txtIssueDateEdit");
                    if (ViewState["REQSTATUS"].ToString().Contains("Pen"))
                        if (txtQty != null) txtQty.ReadOnly = true;
        
                    if (ViewState["REQSTATUS"].ToString().Contains("Req"))
                    {
                        if (txtCard != null) txtCard.Visible = false;
                        if (txtissuedate != null) txtissuedate.Visible = false;
                        if (txtQty != null) txtQty.ReadOnly = false;
                        if (db != null) db.Visible = true;
                    }
                    else
                    {
                        if (txtQty != null) txtQty.ReadOnly = true;
                        if (txtissuedate != null) txtissuedate.Visible = true;
                        if (txtCard != null) txtCard.Visible = true;
                        if (db != null) db.Visible = false;
                    }
                }

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {

                ImageButton ab = (ImageButton)e.Row.FindControl("cmdAdd");
                if (ab != null)
                {
                    ab.Visible = SessionUtil.CanAccess(this.ViewState, ab.CommandName);
                    if (ViewState["REQSTATUS"].ToString() != String.Empty)
                    {
                        if (ViewState["REQSTATUS"].ToString().Contains("Req"))
                        {
                            if (ab != null) ab.Enabled = true;
                        }
                        else
                        {
                            if (ab != null) ab.Enabled = false;
                        }
                    }
                }
                UserControlVesselEmployee emp = ((UserControlVesselEmployee)e.Row.FindControl("ddlEmployee"));
                emp.bind();
                TextBox txtstoreid = (TextBox)e.Row.FindControl("txtStoreId");
                txtstoreid.Attributes.Add("style", "visibility:hidden");
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
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidRequest(
                    (((UserControlVesselEmployee)_gridView.FooterRow.FindControl("ddlEmployee")).SelectedEmployee),
                    (((TextBox)_gridView.FooterRow.FindControl("txtStoreId")).Text.Trim()),
                    (((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtQuantity")).Text)))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsPhoneCardRequisition.InsertPhoneCradRequestLineItem(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                    , new Guid(ViewState["REQUESTID"].ToString())
                                    , int.Parse(((UserControlVesselEmployee)_gridView.FooterRow.FindControl("ddlEmployee")).SelectedEmployee)
                                    , new Guid(((TextBox)_gridView.FooterRow.FindControl("txtStoreId")).Text.Trim())
                                    , decimal.Parse(((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtQuantity")).Text));
                BindData();
            }
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {

                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

                string requestlineid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRequestLineIdEdit")).Text;
                TextBox txtCard = (TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCardNumberEdit");
                if (txtCard != null && txtCard.Visible == true)
                {
                    UserControlMaskNumber txtQty = (UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtQuantityEdit");
                    UserControlDate txtissuedate = (UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtIssueDateEdit");
                    UserControlVesselEmployee employeeid = (UserControlVesselEmployee)_gridView.Rows[nCurrentRow].FindControl("ddlEmployeeEdit");
                    decimal qty = decimal.Parse(txtQty.Text.Trim());
                    string CardNumber = txtCard.Text.Trim();
                    if (ViewState["REQSTATUS"].ToString().Contains("Req"))
                    {
                        if (!IsValidRequestUpdate(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtQuantityEdit")).Text))
                        {
                            ucError.Visible = true;
                            return;
                        }
                        PhoenixVesselAccountsPhoneCardRequisition.UpdatePhoneCradRequestLineItem(new Guid(requestlineid)
                                      , decimal.Parse(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtQuantityEdit")).Text));
                    }
                    else
                    {
                        if (!IsValidCardUpdate(CardNumber, qty, employeeid.SelectedEmployee))
                        {
                            ucError.Visible = true;
                            return;
                        }
                        PhoenixVesselAccountsPhoneCardRequisition.UpdatePhoneCradRequestLineItemCardNumber(new Guid(requestlineid), CardNumber,
                            General.GetNullableDateTime(txtissuedate.Text), General.GetNullableInteger(employeeid.SelectedEmployee));
                    }
                }
                else
                {
                    if (!IsValidRequestUpdate(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtQuantityEdit")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixVesselAccountsPhoneCardRequisition.UpdatePhoneCradRequestLineItem(new Guid(requestlineid)
                                  , decimal.Parse(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtQuantityEdit")).Text));
                }
                _gridView.EditIndex = -1;
                BindData();

            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

                Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
                PhoenixVesselAccountsPhoneCardRequisition.DeletePhoneCradRequestLineItem(id);
                _gridView.EditIndex = -1;
                BindData();
            }
            if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

                Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
                Label lblCard = (Label)_gridView.Rows[nCurrentRow].FindControl("lblCardNumber");
                Label lblissuedate = (Label)_gridView.Rows[nCurrentRow].FindControl("lblIssueDate");
                if (!IsValidConfirmIssue(lblCard.Text, lblissuedate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsPhoneCardRequisition.ConfirmIssueDetails(id);
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

    private bool IsValidRequest(string empid, string storeid, string qty)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (String.IsNullOrEmpty(empid))
        {
            ucError.ErrorMessage = "Employee is Required";
        }
        if (String.IsNullOrEmpty(storeid))
        {
            ucError.ErrorMessage = "Card type is Required";
        }
        if (String.IsNullOrEmpty(qty))
        {
            ucError.ErrorMessage = "Quantity is Required";
        }
        else if (decimal.Parse(qty) <= 0)
        {
            ucError.ErrorMessage = "Quantity should be greater than Zero";
        }
        return (!ucError.IsError);
    }

    private bool IsValidCardUpdate(string cardNumber, decimal count, string employeeid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (String.IsNullOrEmpty(cardNumber))
        {
            ucError.ErrorMessage = "Card Number is Required";
        }
        else if (cardNumber.Split(',').Length != Convert.ToInt32(count))
        {
            ucError.ErrorMessage = "Entered Card Numbers mismatched with Quantity";
        }
        if (string.IsNullOrEmpty(employeeid))
        {
            ucError.ErrorMessage = "Employee is required.";
        }
        return (!ucError.IsError);
    }

    private bool IsValidConfirmIssue(string cardNumber, string Issuedate)
    {
        ucError.HeaderMessage = "Please provide the following required information before Confirm";

        if (ViewState["REQSTATUS"].ToString().Contains("Req"))
        {
            ucError.ErrorMessage = "Approve the Requition";
        }
        else
        {
            if (String.IsNullOrEmpty(cardNumber))
            {
                ucError.ErrorMessage = "Card Number is Required";
            }
            if (String.IsNullOrEmpty(Issuedate))
            {
                ucError.ErrorMessage = "Issue Date is Required";
            }
        }
        return (!ucError.IsError);
    }

    private bool IsValidRequestUpdate(string qty)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (String.IsNullOrEmpty(qty))
        {
            ucError.ErrorMessage = "Quantity is Required";
        }
        else if (decimal.Parse(qty) <= 0)
        {
            ucError.ErrorMessage = "Quantity should be greater than Zero";
        }
        return (!ucError.IsError);
    }

}
