using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsOfficeFundsReceived : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddButton("Allocate", "ALLOCATE", ToolBarDirection.Right);
            toolbar.AddButton("Office Fund", "FUND", ToolBarDirection.Right);
            MenuOfficeFund.AccessRights = this.ViewState;
            MenuOfficeFund.MenuList = toolbar.Show();
            MenuOfficeFund.SelectedMenuIndex = 1;

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsOfficeFundsReceived.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvFundReceived')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsOfficeFundsReceived.aspx", "Find", "search.png", "FIND");
            toolbargrid.AddImageButton("../Accounts/AccountsOfficeFundsReceived.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuOfficeFundGrid.AccessRights = this.ViewState;
            MenuOfficeFundGrid.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {

                Session["New"] = "N";
                ViewState["OwnerOfficeFundId"] = "";
                if (Request.QueryString["OwnerOfficeFundId"] != null)
                    ViewState["OwnerOfficeFundId"] = Request.QueryString["OwnerOfficeFundId"];
                ViewState["PAGENUMBER"] = 1;
                gvFundReceived.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOfficeFundGrid_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
                // SetPageNavigator();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtVoucherNumber.Text = "";
                Rebind();
                //SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOfficeFund_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("ALLOCATE"))
            {
                Response.Redirect("../Accounts/AccountsOfficeFundsReceivedAllocate.aspx?OwnerOfficeFundId=" + ViewState["OwnerOfficeFundId"].ToString() + "&Source=2" + "&Ownerfundreceived=0");
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
        gvFundReceived.Rebind();
        //SetPageNavigator();
        //if (Session["New"].ToString() == "Y")
        //{
        //    gvFundReceived.SelectedIndex = 0;
        //    Session["New"] = "N";
        //    BindPageURL(gvFundReceived.SelectedIndex);
        //}
    }

    protected void gvFundReceived_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvFundReceived_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            //if (e.CommandName == "ChangePageSize")
            //{
            //    return;
            //}

            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;
          //  int nCurrentRow = e.Item.ItemIndex;          

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
            }
            if (e.CommandName.ToUpper().Equals("ALLOCATE"))
            {
               // ViewState["OwnerOfficeFundId"] = ((RadLabel)FindControl("lblOwnerOfficeFundId")).Text;
                Response.Redirect("../Accounts/AccountsOfficeFundsReceivedAllocate.aspx?OwnerOfficeFundId=" + ViewState["OwnerOfficeFundId"].ToString() + "&Source=2" + "&Ownerfundreceived=0");
            }
            if (e.CommandName.ToUpper().Equals("REPOST"))
            {
                PhoenixAccountsOwnerOfficeSingleDepartment.FundReceivedVoucherRepost(new Guid(ViewState["OwnerOfficeFundId"].ToString())
                                                                                           , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                Rebind();
                ucStatus.Text = "Reposted.";
            }
            if (e.CommandName.ToUpper().Equals("POST"))
            {
                if (((RadLabel)e.Item.FindControl("lblVoucherStatus")).Text != "Draft")
                {

                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Cannot Post. Voucher should be in Draft status.";
                    ucError.Visible = true;
                    return;
                }

                String scriptpopup = String.Format(
                   "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsOwnerOfficeSingleDepartmentPost.aspx?OwnerOfficeFundId=" + ((RadLabel)e.Item.FindControl("lblOwnerOfficeFundId")).Text + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvFundReceived_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        _gridView.EditIndex = -1;
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvFundReceived_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        _gridView.EditIndex = -1;
    //        BindData();
    //        SetPageNavigator();

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvFundReceived_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;

    //    _gridView.EditIndex = e.NewEditIndex;
    //    _gridView.SelectedIndex = e.NewEditIndex;

    //    BindData();
    //    SetPageNavigator();
    //}


    protected void gvFundReceived_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        GridView _gridView = sender as GridView;
        _gridView.SelectedIndex = se.NewSelectedIndex;
        string OwnerOfficeFundId = _gridView.DataKeys[se.NewSelectedIndex].Value.ToString();
        ViewState["OwnerOfficeFundId"] = OwnerOfficeFundId;
        Rebind();
    }

    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDDATE", "FLDBANKACCOUNT", "FLDLONGDESCRIPTION", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDALLOCATEDAMOUNT", "FLDVOUCHERNUMBER", "FLDVOUCHERSTATUS" };
            string[] alCaptions = { "Date", "Bank Account", "Long Description", "Currency", "Receipt Amount", "Allocated Amount", "Voucher Number", "Voucher Status" };

            DataSet ds = new DataSet();
            ds = PhoenixAccountsOwnerOfficeSingleDepartment.FundReceivedVoucherList(txtVoucherNumber.Text, 2, 1231,
                                                                                  gvFundReceived.CurrentPageIndex + 1,
                                                                                  gvFundReceived.PageSize,
                                                                                  ref iRowCount,
                                                                                  ref iTotalPageCount);

            General.SetPrintOptions("gvFundReceived", "Office Fund", alCaptions, alColumns, ds);
            gvFundReceived.DataSource = ds;
            gvFundReceived.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                if (ViewState["OwnerOfficeFundId"] == null)
                {
                    ViewState["OwnerOfficeFundId"] = dr["FLDOWNEROFFICEFUNDID"].ToString();
                    //       gvFundReceived.SelectedIndex = 0;
                }
                SetRowSelection();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                //     ShowNoRecordsFound(dt, gvFundReceived);
                //ifMoreInfo.Attributes["src"] = "../Accounts/AccountsOwnerOfficeSingleDepartment.aspx?OwnerOfficeFundId=";
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            //   SetPageNavigator();
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

        string[] alColumns = { "FLDDATE", "FLDBANKACCOUNT", "FLDLONGDESCRIPTION", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDALLOCATEDAMOUNT", "FLDVOUCHERNUMBER", "FLDVOUCHERSTATUS" };
        string[] alCaptions = { "Date", "Bank Account", "Long Description", "Currency", "Receipt Amount", "Allocated Amount", "Voucher Number", "Voucher Status" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsOwnerOfficeSingleDepartment.FundReceivedVoucherList(txtVoucherNumber.Text, 2, 1231,
                                                                              (int)ViewState["PAGENUMBER"],
                                                                              PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
                                                                              ref iRowCount,
                                                                              ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename= OfficeFund.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Office Fund</h3></td>");
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

    private void SetRowSelection()
    {
        gvFundReceived.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvFundReceived.Items)
        {
            if (item.GetDataKeyValue("FLDOWNEROFFICEFUNDID").ToString().Equals(ViewState["OwnerOfficeFundId"].ToString()))
            {
                gvFundReceived.SelectedIndexes.Add(item.ItemIndex);
            }
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblOwnerOfficeFundId = ((RadLabel)gvFundReceived.Items[rowindex].FindControl("lblOwnerOfficeFundId"));
            if (lblOwnerOfficeFundId != null)
                ViewState["OwnerOfficeFundId"] = lblOwnerOfficeFundId.Text;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //private void ShowNoRecordsFound(DataTable dt, GridView gv)
    //{
    //    dt.Rows.Add(dt.NewRow());
    //    gv.DataSource = dt;
    //    gv.DataBind();
    //    int colcount = gv.Columns.Count;
    //    gv.Rows[0].Cells.Clear();
    //    gv.Rows[0].Cells.Add(new TableCell());
    //    gv.Rows[0].Cells[0].ColumnSpan = colcount;
    //    gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    //    gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
    //    gv.Rows[0].Cells[0].Font.Bold = true;
    //    gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    //    gv.Rows[0].Attributes["onclick"] = "";
    //}

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    int result;
    //    if (Int32.TryParse(txtnopage.Text, out result))
    //    {
    //        ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //        if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


    //        if (0 >= Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = 1;

    //        if ((int)ViewState["PAGENUMBER"] == 0)
    //            ViewState["PAGENUMBER"] = 1;

    //        txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //    }
    //    BindData();
    //}

    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    gvFundReceived.SelectedIndex = -1;
    //    gvFundReceived.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //    else
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;
    //    BindData();
    //}

    //private void SetPageNavigator()
    //{
    //    cmdPrevious.Enabled = IsPreviousEnabled();
    //    cmdNext.Enabled = IsNextEnabled();
    //    lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //    lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //}

    //private Boolean IsPreviousEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //private Boolean IsNextEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    protected void Rebind()
    {
        gvFundReceived.SelectedIndexes.Clear();
        gvFundReceived.EditIndexes.Clear();
        gvFundReceived.DataSource = null;
        gvFundReceived.Rebind();
    }
    protected void gvFundReceived_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFundReceived.CurrentPageIndex + 1;
        BindData();
    }
}
