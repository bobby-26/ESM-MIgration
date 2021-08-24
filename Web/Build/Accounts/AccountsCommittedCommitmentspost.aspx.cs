using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Accounts_AccountsCommittedCommitmentspost : PhoenixBasePage
{
    public decimal dSumReversed = 0;
    public decimal dSumCharged = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            PhoenixToolbar toolbarsave = new PhoenixToolbar();
            toolbarsave.AddButton("Post", "POST", ToolBarDirection.Right);
            toolbarsave.AddButton("View", "VIEW", ToolBarDirection.Right);
            MenuCommittedcostpostTab.AccessRights = this.ViewState;
            MenuCommittedcostpostTab.MenuList = toolbarsave.Show();

            //MenuCommittedcostsubacTab.AccessRights = this.ViewState;
            //MenuCommittedcostsubacTab.MenuList = toolbarsave.Show();
            PhoenixToolbar toolbargv = new PhoenixToolbar();
            toolbargv.AddImageButton("../Accounts/AccountsCommittedCommitmentspost.aspx", "Export to Excel", "icon_xls.png", "Excel");
            MenucommitedcostPo.AccessRights = this.ViewState;
            MenucommitedcostPo.MenuList = toolbargv.Show();
            if (!IsPostBack)
            {
                ucVesselAccount.DataTextField = "FLDVESSELACCOUNTNAME";
                ucVesselAccount.DataValueField = "FLDACCOUNTID";
                ucVesselAccount.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(null, 1);
                ucVesselAccount.DataBind();
                ViewState["PAGENUMBER"] = 1;
                ViewState["PAGENUMBER1"] = 1;
                ViewState["PAGENUMBER2"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["BUDGETID"] = "";

                gvcommitedcostposted.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvcommitedcostPoSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                Gvcommittedcostvouchers.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            //Bindsubac();
            //SetPageNavigator();
            //Bindchargedpo();
            //setpagenavigatorchargedpo();
            //Bindcommittedcostvoucher();
            //setpagenavigatorpostedvouchers();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenucommitedcostPo_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
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
    private void BindTotalFooter()
    {
        DataSet ds = new DataSet();
        ds = PhoenixAccountsCommittedCostPosting.CommittedcostsumTotal(General.GetNullableInteger(ucVesselAccount.SelectedValue));
        dSumReversed = Convert.ToDecimal(ds.Tables[0].Rows[0]["FLDSUMREVERSED"].ToString());
        dSumCharged = Convert.ToDecimal(ds.Tables[0].Rows[0]["FLDSUMCHARGED"].ToString());

        dSumReversed = Convert.ToDecimal(string.Format("{0:0.00}", dSumReversed));
        dSumCharged = Convert.ToDecimal(string.Format("{0:0.00}", dSumCharged));
    }

    protected void CommittedcostpostTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
    }
    protected void CommittedcostsubacTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("VIEW"))
        {
            try
            {
                if (!IsValidVesselacdesc())
                {
                    ucError.Visible = true;
                    return;
                }
                ViewState["PAGENUMBER"] = 1;
                Bindsubac();
                gvcommitedcostposted.Rebind();
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
        }

        if (CommandName.ToUpper().Equals("POST"))
        {
            if (!IsValidVesselacdesc())
            {
                ucError.Visible = true;
                return;
            }
            DataSet ds = new DataSet();
            ds = PhoenixAccountsCommittedCostPosting.Accountscommittedcostpost(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ucVesselAccount.SelectedValue));
            ucStatus.Text = "Committed cost po";
            Bindchargedpo();
        }
    }

    public void Bindcommittedcostvoucher()
    {
        string[] alColumns = { "FLDACCOUNTID", "FLDACCOUNTID", "FLDVOUCHERDATE", "FLDAMOUNTINUSD", "FLDCHARGED", "FLDVOUCHERNUMBER" };
        string[] alCaptions = { "Account id", "Account id", "Voucherdate", "Amount", "Reversed", "Voucher number" };
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        DataSet ds = new DataSet();
        ds = PhoenixAccountsCommittedCostPosting.Accountscommittedvoucherposted(
                      General.GetNullableInteger(ucVesselAccount.SelectedValue)
                    , sortexpression
                    , sortdirection
                    , (int)ViewState["PAGENUMBER2"]
                    , Gvcommittedcostvouchers.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount);

        General.SetPrintOptions("gvcommitedcostposted", "SUBACCODE", alCaptions, alColumns, ds);
        Gvcommittedcostvouchers.DataSource = ds;
        Gvcommittedcostvouchers.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    public void Bindsubac()
    {
        string[] alColumns = { "FLDSUBACCOUNT", "FLDBUDGETCODE", "FLDAMOUNTINUSD", "FLDCHARGED" };
        string[] alCaptions = { "Sub A/C", "Budget Code", "Reversed", "Charged" };
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = new DataSet();
        ds = PhoenixAccountsCommittedCostPosting.AccountCommittedCommitmentsPostSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                          , General.GetNullableInteger(ucVesselAccount.SelectedValue)
                          , General.GetNullableDateTime(ucAsOnDate.Text)
                          , sortexpression
                          , sortdirection
                          , (int)ViewState["PAGENUMBER"]
                          , gvcommitedcostposted.PageSize
                          , ref iRowCount
                          , ref iTotalPageCount);

        General.SetPrintOptions("gvcommitedcostposted", "SUBACCODE", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvcommitedcostposted.DataSource = ds;


            if (ViewState["BUDGETID"] == null)
            {
                ViewState["BUDGETID"] = ds.Tables[0].Rows[0]["FLDBUDGETID"].ToString();

            }
        }
        else
        {
            gvcommitedcostposted.DataSource = ds;

        }
        gvcommitedcostposted.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDPONUMBER", "FLDVESSELACCOUNTID", "FLDSUBACCOUNT", "FLDBUDGETCODE", "FLDDATEOFAPPROVAL", "FLDAMOUNTINUSD" };
        string[] alCaptions = { "PO Number", "Supplier", "Sub A/C", "Owner Budget Code", "Committed Date", "Amount(USD)" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsCommittedCostPosting.AccountCommittedCommitmentChargingPoSearch(
                         General.GetNullableInteger(ucVesselAccount.SelectedValue)
                        , General.GetNullableInteger(ViewState["BUDGETID"].ToString())
                        , General.GetNullableDateTime(ucAsOnDate.Text)
                        , sortexpression
                        , sortdirection
                        , (int)ViewState["PAGENUMBER"]
                        , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                        , ref iRowCount
                        , ref iTotalPageCount
                        );

        Response.AddHeader("Content-Disposition", "attachment; filename=ERMVoucher.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3> CHARGING PO </h3></td>");
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

    private Boolean IsPreviousEnabledChargedPO()
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

    private Boolean IsPreviousEnabledPostedvouchers()
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

    protected bool IsValidVesselacdesc()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (General.GetNullableInteger(ucVesselAccount.SelectedValue) == null)
            ucError.ErrorMessage = "Vessel account is required";
        if (General.GetNullableDateTime(ucAsOnDate.Text) == null)
            ucError.ErrorMessage = "As on date is required";
        return (!ucError.IsError);
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

    private Boolean IsNextEnabledChargedPO()
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

    private Boolean IsNextEnabledPostedvouchers()
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


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    private void Bindchargedpo()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = new DataSet();
        ds = PhoenixAccountsCommittedCostPosting.AccountCommittedCommitmentChargingPoSearch(
                     General.GetNullableInteger(ucVesselAccount.SelectedValue)
                    , General.GetNullableInteger(ViewState["BUDGETID"].ToString())
                    , General.GetNullableDateTime(ucAsOnDate.Text)
                    , sortexpression
                    , sortdirection
                    , (int)ViewState["PAGENUMBER1"]
                    , gvcommitedcostPoSearch.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount
                    );
        gvcommitedcostPoSearch.DataSource = ds;
        gvcommitedcostPoSearch.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }





    protected void ucVesselAccount_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void gvcommitedcostposted_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvcommitedcostposted.CurrentPageIndex + 1;
            Bindsubac();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    protected void gvcommitedcostposted_EditCommand(object sender, GridCommandEventArgs e)
    {
        Bindchargedpo();
    }

    protected void gvcommitedcostposted_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {

                ViewState["BUDGETID"] = ((RadLabel)e.Item.FindControl("lblBudgetId")).Text;
                Bindchargedpo();
                gvcommitedcostPoSearch.Rebind();
            }
            else if (e.CommandName == "Page")
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

    protected void gvcommitedcostPoSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER1"] = ViewState["PAGENUMBER1"] != null ? ViewState["PAGENUMBER1"] : gvcommitedcostposted.CurrentPageIndex + 1;
            Bindchargedpo();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvcommitedcostPoSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER1"] = null;
        }
    }

    protected void Gvcommittedcostvouchers_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER2"] = ViewState["PAGENUMBER2"] != null ? ViewState["PAGENUMBER2"] : gvcommitedcostposted.CurrentPageIndex + 1;
            Bindcommittedcostvoucher();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void Gvcommittedcostvouchers_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
}
