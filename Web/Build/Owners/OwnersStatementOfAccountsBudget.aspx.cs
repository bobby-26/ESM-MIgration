using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Owners;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Owners_OwnersStatementOfAccountsBudget : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            if (SessionUtil.CanAccess(this.ViewState, "Excel"))
                toolbargrid.AddFontAwesomeButton("../Owners/OwnersStatementOfAccountsBudget.aspx", "Export to Excel", "<i class=\"fa fa-file-excel\"></i>", "Excel");
            if (SessionUtil.CanAccess(this.ViewState, "PRINT"))
                toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvOwnersAccount')", "Print Grid", "<i class=\"fa fa-print\"></i>", "PRINT");

            if (SessionUtil.CanAccess(this.ViewState, "Excel") || SessionUtil.CanAccess(this.ViewState, "PRINT"))
            {
                MenuOrderForm.AccessRights = this.ViewState;
                MenuOrderForm.MenuList = toolbargrid.Show();
            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("SOA", "SOA");
            toolbarmain.AddButton("Budget Code", "VOUCHER");
            MenuGeneral.AccessRights = this.ViewState;
            MenuGeneral.MenuList = toolbarmain.Show();
            MenuGeneral.SelectedMenuIndex = 1;

            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

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

                if (Request.QueryString["ownerid"].ToString() != null)
                    ViewState["ownerid"] = Request.QueryString["ownerid"].ToString();
                else
                    ViewState["ownerid"] = "";
                gvOwnersAccount.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SOA"))
            {
                Response.Redirect("../Owners/OwnersStatementOfAccounts.aspx", true);
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

    protected void ShowExcel()
    {

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDHARDNAME", "FLDOWNERBUDGETCODE", "FLDBUDGETNAME", "FLDAMOUNT", "FLDINCREMAENTALADD" };
        string[] alCaptions = { "Budget Group", "Budget Code", "Budget Code Description", "Amount", "Accumulated Amount" };
        string prevBcodegroup = null;
        string curBcodegroup = null;

        ds = PhoenixOwnersStatementOfAccounts.StatementOfAccountsBudgetSearch(
            General.GetNullableInteger(ViewState["accountid"].ToString()), ViewState["accountcode"].ToString(),
            ViewState["debitnoteref"].ToString(), General.GetNullableInteger(ViewState["ownerid"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=Summary By Budget Code.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>Summary By Budget Code</center></h3></td>");
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
        ds = PhoenixOwnersStatementOfAccounts.StatementOfAccountsBudgetSearch(
                    General.GetNullableInteger(ViewState["accountid"].ToString()), ViewState["accountcode"].ToString(),
                    ViewState["debitnoteref"].ToString(), General.GetNullableInteger((ViewState["ownerid"].ToString())));

        if (ds.Tables[0].Rows.Count > 0)
        {
            lblAccountCOdeDescription.Text = ds.Tables[0].Rows[0]["FLDACCOUNTDESC"].ToString();
            lblStatementReference.Text = ds.Tables[0].Rows[0]["FLDREFERENCE"].ToString();
        }
        gvOwnersAccount.DataSource = ds;
        gvOwnersAccount.VirtualItemCount = ds.Tables[0].Rows.Count;

        General.SetPrintOptions("gvOwnersAccount", "Budget Codes", alCaptions, alColumns, ds);
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void gvOwnersAccount_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkBudgetCOde");
            RadLabel lblBudgetCodeId = (RadLabel)e.Item.FindControl("lblBudgetCodeId");
            if (lbr != null)
            {
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Owners/OwnersStatementOfAccountsVoucher.aspx?accountid="
                    + ViewState["accountid"].ToString() + "&debitnotereference=" + ViewState["debitnoteref"].ToString()
                    + "&Ownerid=" + ViewState["ownerid"].ToString() + "&ownerbudgetid=" + lblBudgetCodeId.Text + "'); return false;");
            }

        }
    }
    protected void gvOwnersAccount_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                RadLabel lblBudgetCodeId = (RadLabel)e.Item.FindControl("lblBudgetCodeId");
                Filter.CurrentOwnerBudgetCodeSelection = lblBudgetCodeId.Text;
            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOwnersAccount_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOwnersAccount.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


}
