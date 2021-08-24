using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsOwnerFundsReceived : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Allocate", "ALLOCATE", ToolBarDirection.Right);
            toolbar.AddButton("Owner Fund", "FUND", ToolBarDirection.Right);

            MenuOwnerFund.AccessRights = this.ViewState;
            MenuOwnerFund.MenuList = toolbar.Show();
            MenuOwnerFund.SelectedMenuIndex = 1;

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsOwnerFundsReceived.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvFundReceived')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsOwnerFundsReceived.aspx", "Find", "search.png", "FIND");
            toolbargrid.AddImageButton("../Accounts/AccountsOwnerFundsReceived.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuOwnerFundGrid.AccessRights = this.ViewState;
            MenuOwnerFundGrid.MenuList = toolbargrid.Show();

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


    protected void MenuOwnerFundGrid_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtVoucherNumber.Text = "";
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOwnerFund_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("ALLOCATE"))
            {
                Response.Redirect("../Accounts/AccountsOfficeFundsReceivedAllocate.aspx?OwnerOfficeFundId=" + ViewState["OwnerOfficeFundId"].ToString() + "&Source=1" + "&Ownerfundreceived=1");
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
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

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
               // ViewState["OwnerOfficeFundId"] = General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblOwnerOfficeFundId")).Text);
                Response.Redirect("../Accounts/AccountsOfficeFundsReceivedAllocate.aspx?OwnerOfficeFundId=" + ViewState["OwnerOfficeFundId"].ToString() + "&Source=1" + "&Ownerfundreceived=1");
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

            ds = PhoenixAccountsOwnerOfficeSingleDepartment.FundReceivedVoucherList(txtVoucherNumber.Text, 1, 1231,
                                                                                  (int)ViewState["PAGENUMBER"],
                                                                                  gvFundReceived.PageSize,
                                                                                  ref iRowCount,
                                                                                  ref iTotalPageCount);

            General.SetPrintOptions("gvFundReceived", "Owner Fund", alCaptions, alColumns, ds);

            gvFundReceived.DataSource = ds;
            gvFundReceived.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                if (ViewState["OwnerOfficeFundId"] == null)
                {
                    ViewState["OwnerOfficeFundId"] = dr["FLDOWNEROFFICEFUNDID"].ToString();
                }
                SetRowSelection();
            }
            else
            {
                DataTable dt = ds.Tables[0];
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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

        ds = PhoenixAccountsOwnerOfficeSingleDepartment.FundReceivedVoucherList(txtVoucherNumber.Text, 1, 1231,
                                                                              (int)ViewState["PAGENUMBER"],
                                                                              PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
                                                                              ref iRowCount,
                                                                              ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename= OwnerFund.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Owner Fund</h3></td>");
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
        //gvFundReceived.SelectedIndex = -1;
        //for (int i = 0; i < gvFundReceived.Rows.Count; i++)
        //{
        //    if (gvFundReceived.DataKeys[i].Value.ToString().Equals(ViewState["OwnerOfficeFundId"].ToString()))
        //    {
        //        gvFundReceived.SelectedIndex = i;
        //    }
        //}
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

