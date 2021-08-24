using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Accounts;
using System.Web.UI;
using Telerik.Web.UI;

public partial class AccountsOwnerStatementOfAccountBudget : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            if (SessionUtil.CanAccess(this.ViewState, "Excel"))
                toolbargrid.AddImageButton("../Accounts/AccountsOwnerStatementOfAccountBudget.aspx", "Export to Excel", "icon_xls.png", "Excel");
            if (SessionUtil.CanAccess(this.ViewState, "PRINT"))
                toolbargrid.AddImageLink("javascript:CallPrint('gvOwnersAccount')", "Print Grid", "icon_print.png", "PRINT");
            //toolbargrid.AddImageButton("../Accounts/AccountsSoaCheckingBudgetSearch.aspx", "Find", "search.png", "FIND");
            if (SessionUtil.CanAccess(this.ViewState, "Excel") || SessionUtil.CanAccess(this.ViewState, "PRINT"))
            {
                MenuOrderForm.AccessRights = this.ViewState;
                MenuOrderForm.MenuList = toolbargrid.Show();
            }
            //MenuOrderForm.AccessRights = this.ViewState;
            //MenuOrderForm.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SUPPORTINGSYN"] = null;

                if (Request.QueryString["accountid"].ToString() != null)
                    ViewState["accountid"] = Request.QueryString["accountid"].ToString();
                else
                    ViewState["accountid"] = "";

                if (Request.QueryString["accountid"].ToString() != null)
                    ViewState["accountcode"] = Request.QueryString["accountcode"].ToString();
                else
                    ViewState["accountcode"] = "";

                if (Request.QueryString["debitnoteref"].ToString() != null)
                    ViewState["debitnoteref"] = Request.QueryString["debitnoteref"].ToString();
                else
                    ViewState["debitnoteref"] = "";

                ViewState["ownerid"] = "";

                if (Request.QueryString["SUPPORTINGSYN"] != null && Request.QueryString["SUPPORTINGSYN"].ToString() != null)
                    ViewState["SUPPORTINGSYN"] = Request.QueryString["SUPPORTINGSYN"].ToString();
                else
                    ViewState["SUPPORTINGSYN"] = "";

                if (Request.QueryString["debitnoterefID"] != null)
                    ViewState["debitnoterefID"] = Request.QueryString["debitnoterefID"].ToString();
                else
                    ViewState["debitnoterefID"] = "";

                if (Request.QueryString["from"] != null && Request.QueryString["from"].ToString() != string.Empty)
                    ViewState["qfrom"] = Request.QueryString["from"];
                else
                    ViewState["qfrom"] = "";
                SetRedirectURL();
            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("SOA", "SOA");
            toolbarmain.AddButton("Budget Code", "VOUCHER");
            toolbarmain.AddButton("Back", "BACK");

            MenuGeneral.AccessRights = this.ViewState;
            MenuGeneral.MenuList = toolbarmain.Show();
            MenuGeneral.SelectedMenuIndex = 1;


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;


        try
        {
            if (CommandName.ToUpper().Equals("SOA"))
            {
                if (ViewState["SUPPORTINGSYN"].ToString() == "NO")
                    Response.Redirect("../Accounts/AccountsOwnersStatementOfAccounts.aspx", true);
                else
                    Response.Redirect("../Accounts/AccountsOwnersSOAWithSupportings.aspx?debitnoteid=" + ViewState["debitnoterefID"], true);

            }
            if (CommandName.ToUpper().Equals("BACK"))
            {
                if (Request.QueryString["from"] != null)
                {
                    Response.Redirect(ViewState["URL"].ToString() + "?debitnoteid=" + ViewState["debitnoterefID"], true);
                }
                else
                {
                    Response.Redirect("../Accounts/AccountsOwnersStatementOfAccounts.aspx?debitnoteid=" + ViewState["debitnoterefID"], true);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvOwnersAccount.SelectedIndexes.Clear();
        gvOwnersAccount.EditIndexes.Clear();
        gvOwnersAccount.DataSource = null;
        gvOwnersAccount.Rebind();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        //int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDHARDNAME", "FLDOWNERBUDGETCODE", "FLDBUDGETNAME", "FLDAMOUNT", "FLDINCREMAENTALADD" };
        string[] alCaptions = { "Budget Group", "Budget Code", "Budget Code Description", "Amount", "Accumulated Amount" };
        string prevBcodegroup = null;
        string curBcodegroup = null;
        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsOwnerStatementOfAccounts.StatementOfAccountsBudgetSearch(
            General.GetNullableInteger(ViewState["accountid"].ToString()),
            ViewState["accountcode"].ToString(),
            ViewState["debitnoteref"].ToString(),
            General.GetNullableInteger(ViewState["ownerid"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=Summary By Budget Code.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>Summary By Budget Code</center></h3></td>");
        //Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><center> " + ds.Tables[0].Rows[0]["FLDACCOUNTCODE"].ToString() + "</center><td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><center> " + ds.Tables[0].Rows[0]["FLDACCOUNTDESC"].ToString() + "</center><td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><center> " + ds.Tables[0].Rows[0]["FLDREFERENCE"].ToString() + "</center><td></tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='15%'>");
            Response.Write("<b>" + alCaptions[i].ToString().Trim() + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");


        DataTable dt = ds.Tables[0];
        for (int dr = 0; dr < dt.Rows.Count; dr++)
        {
            DataRow previousdatarow;
            DataRow datarow;
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                if (i == 4)
                    Response.Write("<td colspan='2'>");
                else
                    Response.Write("<td width='15%'>");
                decimal s;

                if (dr > 0)
                {

                    previousdatarow = dt.Rows[dr - 1];
                    datarow = dt.Rows[dr];
                    prevBcodegroup = datarow[alColumns[0]].ToString();
                    curBcodegroup = previousdatarow[alColumns[0]].ToString();

                    if (curBcodegroup == prevBcodegroup)
                    {
                        if (i == 0)
                            Response.Write("<font color='white'>");
                        else
                            Response.Write("<font color='black'>");
                    }
                    else
                    {
                        Response.Write("<font color='black'>");
                    }
                }
                else
                {
                    datarow = dt.Rows[dr];
                    Response.Write("<font color='black'>");
                }


                Response.Write(decimal.TryParse(datarow[alColumns[i]].ToString(), out s) ? String.Format("{0:#,###,###.##}", datarow[alColumns[i]]) : datarow[alColumns[i]]);
                Response.Write("</font>");
                Response.Write("</td>");

            }
            Response.Write("</tr>");
        }

        Response.Write("</TABLE>");
        Response.End();

    }

    private void BindData()
    {
        string[] alColumns = { "FLDHARDNAME", "FLDTOTALAMOUNT", "FLDOWNERBUDGETCODE", "FLDBUDGETNAME", "FLDACCOUNTCODE", "FLDACCOUNTDESC", "FLDREFERENCE", "FLDAMOUNT", "FLDINCREMAENTALADD" };
        string[] alCaptions = { "Budget Group", "Amount (USD)", "Budget Code", "Budget Code Description", "Account Code", "Account Code Description", "Statement Reference", "Amount", "Total Amount" };

        DataSet ds = new DataSet();

        ds = PhoenixAccountsOwnerStatementOfAccounts.StatementOfAccountsBudgetSearch(
            General.GetNullableInteger(ViewState["accountid"].ToString()),
            ViewState["accountcode"].ToString(),
            ViewState["debitnoteref"].ToString(),
           General.GetNullableInteger((ViewState["ownerid"].ToString())));

        gvOwnersAccount.DataSource = ds;


        if (ds.Tables[0].Rows.Count > 0)
        {
            //lblAccountCOde.Text = ds.Tables[0].Rows[0]["FLDACCOUNTCODE"].ToString();
            lblAccountCOdeDescription.Text = ds.Tables[0].Rows[0]["FLDACCOUNTDESC"].ToString();
            lblStatementReference.Text = ds.Tables[0].Rows[0]["FLDREFERENCE"].ToString();
            ViewState["ownerid"] = ds.Tables[0].Rows[0]["FLDOWNERID"].ToString();
            //gvOwnersAccount.DataSource = ds;
            //gvOwnersAccount.DataBind();
        }

        General.SetPrintOptions("gvOwnersAccount", "Budget Codes", alCaptions, alColumns, ds);
    }

    protected void gvOwnersAccount_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                LinkButton lbr = (LinkButton)e.Item.FindControl("lnkBudgetCOde");

                RadLabel lblBudgetCodeId = (RadLabel)e.Item.FindControl("lblBudgetCodeId");

                if (lbr != null)
                {
                    if (ViewState["SUPPORTINGSYN"] != null && ViewState["SUPPORTINGSYN"].ToString() == "NO")
                        lbr.Attributes.Add("onclick", "javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Owners/OwnersStatementOfAccountsVoucher.aspx?accountid="
                            + ViewState["accountid"].ToString()
                            + "&debitnotereference=" + ViewState["debitnoteref"].ToString()
                            + "&Ownerid=" + ViewState["ownerid"].ToString()
                            + "&ownerbudgetid=" + lblBudgetCodeId.Text
                            + "'); return false;");
                    else
                        lbr.Attributes.Add("onclick", "javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Owners/OwnersSOAWithSupportingsVoucher.aspx?accountid="
                            + ViewState["accountid"].ToString()
                            + "&debitnotereference=" + ViewState["debitnoteref"].ToString()
                            + "&Ownerid=" + ViewState["ownerid"].ToString()
                            + "&ownerbudgetid=" + lblBudgetCodeId.Text
                            + "'); return false;");
                }
            }

            GridDecorator.MergeRows(gvOwnersAccount, e);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvOwnersAccount_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                RadLabel lblBudgetCodeId = (RadLabel)e.Item.FindControl("lblBudgetCodeId");

                //Filter.CurrentOwnerBudgetCodeSelection = lblBudgetCodeId.Text;

                //String scriptpopupclose = String.Format("javascript:fnReloadList('Filter', null,null);");
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    //protected void gvOwnersAccount_PreRender(object sender, GridItemEventArgs e)
    //{
    //    GridDecorator.MergeRows(gvOwnersAccount, e);
    //}

    public class GridDecorator
    {
        public static void MergeRows(RadGrid gridView, GridItemEventArgs e)
        {
            for (int rowIndex = gridView.Items.Count - 2; rowIndex >= 0; rowIndex--)
            {
                int index = e.Item.ItemIndex;

                GridDataItem row = gridView.Items[rowIndex];
                GridDataItem previousRow = gridView.Items[rowIndex + 1];

                string strCurrentBudgetGroup = ((RadLabel)gridView.Items[rowIndex].FindControl("lblBudgetGroup")).Text;
                string strPreviousBudgetGroup = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblBudgetGroup")).Text;

                if (strCurrentBudgetGroup == strPreviousBudgetGroup)
                {
                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                     previousRow.Cells[2].RowSpan + 1;
                    previousRow.Cells[2].Visible = false;
                }

            }
        }
    }

    private void SetRedirectURL()
    {
        if (ViewState["qfrom"] != null)
        {
            if (ViewState["qfrom"].ToString() == "SOADebit")
            {
                ViewState["URL"] = "../Accounts/AccountsOwnersSOAWithSupportings.aspx";
            }
            if (ViewState["qfrom"].ToString() == "SOAGeneration")
            {
                ViewState["URL"] = "../Accounts/AccountsSoaGenerationReport.aspx";
            }
        }
    }
    protected void gvOwnersAccount_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

}
