using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;

public partial class Purchase_PurchaseOrderFormPartPaid : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["ORDERID"] = "";

                if (Request.QueryString["vendorid"] != null)
                    ViewState["VENDORID"] = Request.QueryString["vendorid"].ToString();
                else
                    ViewState["VENDORID"] = "";

                if (Request.QueryString["orderId"] != null)
                    ViewState["ORDERID"] = Request.QueryString["orderId"].ToString();

                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddImageLink("javascript:Openpopup('NAFA','','../Reports/ReportsView.aspx?applicationcode=3&reportcode=PARTPAID&orderid=" + ViewState["ORDERID"].ToString() + "'); return false;", "Request Form", "", "REPORT");
                toolbarmain.AddButton("Back", "BACK");
                MenuPurchasePartPaid.AccessRights = this.ViewState;
                MenuPurchasePartPaid.MenuList = toolbarmain.Show();

                PhoenixToolbar toolbargrid = new PhoenixToolbar();
                toolbargrid.AddImageButton("../Purchase/PurchaseOrderFormPartPaid.aspx?OrderId=" + Request.QueryString["OrderId"].ToString(), "Export to Excel", "icon_xls.png", "Excel");
                toolbargrid.AddImageLink("javascript:CallPrint('gvOrderPartPaid')", "Print Grid", "icon_print.png", "");
                MenuOrderForm.AccessRights = this.ViewState;
                MenuOrderForm.MenuList = toolbargrid.Show();
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

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindData();
        SetPageNavigator();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDESCRIPTION", "FLDAMOUNT", "FLDEXCHANGERATE", "FLDVOUCHERNUMBER", "FLDVOUCHERDATE" };
        string[] alCaptions = { "Description", "Amount", "Exchange Rate", "Voucher Number", "Voucher Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string strorderid = Request.QueryString["OrderId"] != null ? Request.QueryString["OrderId"].ToString() : "";

        Guid orderid = new Guid(strorderid);

        DataSet dsordno = PhoenixPurchaseOrderForm.EditOrderForm(orderid, Filter.CurrentPurchaseVesselSelection);

        if (dsordno.Tables[0].Rows.Count > 0)
        {
            DataRow dr = dsordno.Tables[0].Rows[0];

            txtOrderNumber.Text = dr["FLDFORMNO"].ToString();
        }

        DataSet ds = PhoenixPurchaseOrderPartPaid.OrderPartPaidSearch(orderid, sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvOrderPartPaid.DataSource = ds;
            gvOrderPartPaid.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvOrderPartPaid);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvOrderPartPaid", "Part Paid", alCaptions, alColumns, ds);
    }

    protected void gvOrderPartPaid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();

    }

    protected void gvOrderPartPaid_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvOrderPartPaid.EditIndex = -1;
        gvOrderPartPaid.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvOrderPartPaid_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow
        //    && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
        //    && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        //{
        //    e.Row.TabIndex = -1;
        //    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvOrderPartPaid, "Edit$" + e.Row.RowIndex.ToString(), false);
        //}

        SetKeyDownScroll(sender, e);
    }

    protected void gvOrderPartPaid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            string Script = "";
            Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
            Script += "fnReloadList(null, 'yes', 'yes');";
            Script += "</script>" + "\n";

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            string strorderid = Request.QueryString["OrderId"] != null ? Request.QueryString["OrderId"].ToString() : "";

            Guid orderid = new Guid(strorderid);

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidOrderPartPaid(
                    ((TextBox)_gridView.FooterRow.FindControl("txtAmountAdd")).Text,
                    ((TextBox)_gridView.FooterRow.FindControl("txtDescriptionAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertOrderPartPaid(
                    orderid,
                    decimal.Parse(((TextBox)_gridView.FooterRow.FindControl("txtAmountAdd")).Text),
                    ((TextBox)_gridView.FooterRow.FindControl("txtDescriptionAdd")).Text
                );
                ((TextBox)_gridView.FooterRow.FindControl("txtDescriptionAdd")).Focus();
                BindData();

                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidOrderPartPaid(
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAmountEdit")).Text,
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDescriptionEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                string strorderpartpaidid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblOrderPartPaidIdEdit")).Text;

                Guid orderpartpaidid = new Guid(strorderpartpaidid);

                UpdateOrderPartPaid(
                    orderpartpaidid,
                     orderid,
                    decimal.Parse(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAmountEdit")).Text),
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDescriptionEdit")).Text
                 );
                _gridView.EditIndex = -1;
                BindData();
                InsertOrderFormHistory();

                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteOrderPartPaid(new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblOrderPartPaidId")).Text));
                InsertOrderFormHistory();
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }

            else if (e.CommandName.ToUpper().Equals("PAYMENTCANCEL"))
            {
                PhoenixPurchaseOrderPartPaid.PartPaidCancel(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                     , General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblOrderPartPaidId")).Text));

            }

            //else if (e.CommandName.ToUpper().Equals("APPROVE"))
            //{
            //    ApprovedRequestAdvance(new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblOrderPartPaidId")).Text));
            //    BindData();
            //}

            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvOrderPartPaid_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvOrderPartPaid_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
        SetPageNavigator();

    }

    protected void gvOrderPartPaid_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            string Script = "";
            Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
            Script += "fnReloadList(null, 'yes', 'yes');";
            Script += "</script>" + "\n";

            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (!IsValidOrderPartPaid(
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAmountEdit")).Text,
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDescriptionEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }
            string strorderid = Request.QueryString["OrderId"] != null ? Request.QueryString["OrderId"].ToString() : "";
            string strorderpartpaidid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblOrderPartPaidIdEdit")).Text;

            Guid orderid = new Guid(strorderid);
            Guid orderpartpaidid = new Guid(strorderpartpaidid);

            UpdateOrderPartPaid(
                orderpartpaidid,
                 orderid,
                decimal.Parse(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAmountEdit")).Text),
                ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDescriptionEdit")).Text
             );

            _gridView.EditIndex = -1;
            BindData();

            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOrderPartPaid_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            DataRowView drv = (DataRowView)e.Row.DataItem;

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            DropDownList ddlBankEdit = (DropDownList)e.Row.FindControl("ddlBankEdit");

            if (ddlBankEdit != null)
            {
                BankBind(ddlBankEdit);

                ddlBankEdit.SelectedValue = drv["FLDBANKID"].ToString();
            }

            Label lb = (Label)e.Row.FindControl("lblstatus");
            ImageButton imgb = (ImageButton)e.Row.FindControl("cmdApprove");
            ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
            ImageButton cmdDelete = (ImageButton)e.Row.FindControl("cmdDelete");
            LinkButton lnkDescription = (LinkButton)e.Row.FindControl("lnkDescription");
            ImageButton imgCancel = (ImageButton)e.Row.FindControl("imgCancel");
            ImageButton imgApprove = (ImageButton)e.Row.FindControl("cmdApprove");
            ImageButton cmdApprovalHistory = (ImageButton)e.Row.FindControl("cmdApprovalHistory");

            if (imgb != null && lb != null && lb.Text == "APP")
            {
                imgb.Visible = false;

                if (cmdDelete != null)
                    cmdDelete.Visible = false;
                if (cmdEdit != null)
                    cmdEdit.Visible = false;
                if (lnkDescription != null)
                    lnkDescription.CommandName = "";
                e.Row.Attributes["onclick"] = "";

                if (cmdApprovalHistory != null)
                {
                    cmdApprovalHistory.Visible = true;

                    cmdApprovalHistory.Attributes.Add("onclick", "javascript:Openpopup('codehelp','','../Accounts/AccountsApprovalHistory.aspx?docid=" + drv["FLDORDERPARTPAIDID"].ToString() + "'); return false;");
                }
            }
            else
            {
                if (imgCancel != null && lb != null && lb.Text == "PAV")
                {
                    if (cmdDelete != null)
                        cmdDelete.Visible = false;

                    if (cmdEdit != null)
                        cmdEdit.Visible = false;

                    if (imgb != null)
                    {
                        imgb.ToolTip = "Approve";
                        imgb.Attributes.Add("onclick", "Openpopup('approval', '', '../Common/CommonApproval.aspx?docid=" + drv["FLDORDERPARTPAIDID"].ToString() + "&mod=" + PhoenixModule.PURCHASE
                            + "&type=" + drv["FLDAPPROVALTYPE"].ToString() + "&user=" + drv["FLDTECHDIRECTOR"].ToString() + "," + drv["FLDFLEETMANAGER"].ToString() + "," + drv["FLDSUPT"].ToString() + "&subtype=PARTPAID');return false;");
                        imgb.Visible = SessionUtil.CanAccess(this.ViewState, imgb.CommandName);
                    }

                    imgCancel.Visible = true;

                    if (cmdApprovalHistory != null)
                        cmdApprovalHistory.Visible = true;
                    cmdApprovalHistory.Attributes.Add("onclick", "javascript:Openpopup('codehelp','','../Accounts/AccountsApprovalHistory.aspx?docid=" + drv["FLDORDERPARTPAIDID"].ToString() + "'); return false;");

                }
                else if (lb != null && lb.Text == "CNL")
                {
                    if (cmdApprovalHistory != null)
                        cmdApprovalHistory.Visible = false;

                    imgb.Visible = false;
                    if (cmdDelete != null)
                        cmdDelete.Visible = false;
                    if (cmdEdit != null)
                        cmdEdit.Visible = false;
                    if (lnkDescription != null)
                        lnkDescription.CommandName = "";
                    if (imgCancel != null)
                        imgCancel.Visible = false;
                    if (imgApprove != null)
                        imgApprove.Visible = false;
                    e.Row.Attributes["onclick"] = "";
                }
                else
                {
                    if (imgb != null)
                    {
                        imgb.ToolTip = "Approve";
                        imgb.Attributes.Add("onclick", "Openpopup('approval', '', '../Common/CommonApproval.aspx?docid=" + drv["FLDORDERPARTPAIDID"].ToString() + "&mod=" + PhoenixModule.PURCHASE
                            + "&type=" + drv["FLDAPPROVALTYPE"].ToString() + "&user=" + drv["FLDTECHDIRECTOR"].ToString() + "," + drv["FLDFLEETMANAGER"].ToString() + "," + drv["FLDSUPT"].ToString() + "&subtype=PARTPAID');return false;");
                        imgb.Visible = SessionUtil.CanAccess(this.ViewState, imgb.CommandName);
                    }

                    if (cmdApprovalHistory != null)
                        cmdApprovalHistory.Visible = false;
                }

                Label lbtn = (Label)e.Row.FindControl("lblRemarks");
                UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucRemarksTT");
                if (lbtn != null)
                {
                    lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                    lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                }
            }
            ImageButton cmdAtt = (ImageButton)e.Row.FindControl("cmdAtt");
            if (cmdAtt != null)
            {
                cmdAtt.Visible = SessionUtil.CanAccess(this.ViewState, cmdAtt.CommandName);

                Label lblDTKey = (Label)e.Row.FindControl("lblDTKey");
                Label lblIsAtt = (Label)e.Row.FindControl("lblIsAtt");

                if (lblIsAtt.Text == string.Empty)
                    cmdAtt.ImageUrl = Session["images"] + "/no-attachment.png";

                if (General.GetNullableInteger(drv["FLDPAYMENTVOUCHERYN"].ToString()) == 1)
                {
                    cmdAtt.Attributes.Add("onclick", "javascript:Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                           + PhoenixModule.PURCHASE + "&type=ADVANCEPAYMENT&U=t" + "'); return false;");
                }
                else
                {
                    cmdAtt.Attributes.Add("onclick", "javascript:Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.PURCHASE + "&type=ADVANCEPAYMENT" + "'); return false;");
                }
            }

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            DropDownList ddlBankAdd = (DropDownList)e.Row.FindControl("ddlBankAdd");

            if (ddlBankAdd != null)
            {
                BankBind(ddlBankAdd);
            }
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvOrderPartPaid.EditIndex = -1;
        gvOrderPartPaid.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvOrderPartPaid.EditIndex = -1;
        gvOrderPartPaid.SelectedIndex = -1;
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
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvOrderPartPaid.SelectedIndex = -1;
        gvOrderPartPaid.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
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

    private void InsertOrderPartPaid(Guid orderid, decimal amount, string description)
    {
        PhoenixPurchaseOrderPartPaid.InsertOrderPartPaid(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            orderid, amount, description, Filter.CurrentPurchaseVesselSelection);
    }

    private void UpdateOrderPartPaid(Guid orderpartpaidid, Guid orderid, decimal amount, string description)
    {
        PhoenixPurchaseOrderPartPaid.UpdateOrderPartPaid(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            orderpartpaidid, orderid, amount, description, Filter.CurrentPurchaseVesselSelection);

        ucStatus.Text = "Order Part Paid information updated";
    }

    private bool IsValidOrderPartPaid(string amount, string description)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (amount.Trim().Equals(""))
            ucError.ErrorMessage = "Amount is required.";

        if (description.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";

        return (!ucError.IsError);
    }

    private void DeleteOrderPartPaid(Guid orderpartpaidid)
    {
        PhoenixPurchaseOrderPartPaid.DeleteOrderPartPaid(PhoenixSecurityContext.CurrentSecurityContext.UserCode, orderpartpaidid);
    }

    private void ApprovedRequestAdvance(Guid orderpartpaidid)
    {
        string strorderid = Request.QueryString["OrderId"] != null ? Request.QueryString["OrderId"].ToString() : "";

        DataSet ds = PhoenixPurchaseOrderPartPaid.ApprovedRequestAdvance(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            orderpartpaidid,
            new Guid(strorderid),
            Filter.CurrentPurchaseVesselSelection);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            PhoenixAccountsAdvancePayment.AdvancePaymentInsert(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , int.Parse(dr["FLDVENDORID"].ToString())
                , decimal.Parse(dr["FLDAMOUNT"].ToString())
                , DateTime.Parse(DateTime.Now.ToString())
                , int.Parse(dr["FLDCURRENCYID"].ToString())
                , null
                , dr["FLDFORMNO"].ToString()
                , null
                , new Guid(strorderid)
                , int.Parse(PhoenixCommonRegisters.GetHardCode(1, 124, "SAS")),
                 General.GetNullableInteger(dr["FLDBILLTOCOMPANYID"].ToString()), // companyid
                 General.GetNullableInteger(dr["FLDBUDGETCODEID"].ToString()), // budget code
                 General.GetNullableInteger(dr["FLDVESSELID"].ToString()), // vessel id
                 General.GetNullableInteger(dr["FLDBANKID"].ToString()),// bankid
                 General.GetNullableGuid(dr["FLDORDERPARTPAIDID"].ToString()) // part paid id
                 );
        }
    }

    protected void BankBind(DropDownList ddlBank)
    {
        DataSet ds = PhoenixRegistersAddress.ListBankAddress(
            General.GetNullableInteger(ViewState["VENDORID"].ToString()), null);

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlBank.DataSource = ds;
            ddlBank.DataTextField = "FLDBANKNAME";
            ddlBank.DataValueField = "FLDBANKID";
            ddlBank.DataBind();
        }
        else
        {
            ddlBank.Items.Clear();
            ddlBank.DataBind();
            ddlBank.Items.Insert(0, new ListItem("--Select--", "Dummy"));
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

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }

    }

    private void InsertOrderFormHistory()
    {
        PhoenixPurchaseOrderForm.InsertOrderFormHistory(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableGuid(Request.QueryString["OrderId"] != null ? Request.QueryString["OrderId"].ToString() : ""),
            Filter.CurrentPurchaseVesselSelection);
    }

    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {

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

    protected void MenuPurchasePartPaid_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("BACK"))
            {
                if (ViewState["ORDERID"] != null)
                {
                    Response.Redirect("../Purchase/PurchaseFormDetails.aspx?orderid=" + ViewState["ORDERID"].ToString());
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDDESCRIPTION", "FLDAMOUNT", "FLDEXCHANGERATE", "FLDVOUCHERNUMBER", "FLDVOUCHERDATE" };
        string[] alCaptions = { "Description", "Amount", "Exchange Rate", "Voucher Number", "Voucher Date" };
        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string strorderid = Request.QueryString["OrderId"] != null ? Request.QueryString["OrderId"].ToString() : "";

        Guid orderid = new Guid(strorderid);

        ds = PhoenixPurchaseOrderPartPaid.OrderPartPaidSearch(orderid, sortexpression, sortdirection,
               (int)ViewState["PAGENUMBER"],
               General.ShowRecords(null),
               ref iRowCount,
               ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Part_Paid.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Part Paid</h3></td>");
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
}
