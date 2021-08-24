using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Integration;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsCrewBankAccount : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsCrewBankAccount.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvAccountsCrewBankAccount')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Accounts/AccountsCrewBankAccount.aspx", "Search", "search.png", "FIND");
            toolbar.AddImageButton("../Accounts/AccountsCrewBankAccount.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuAccountsCrewBankAccount.AccessRights = this.ViewState;
            MenuAccountsCrewBankAccount.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvAccountsCrewBankAccount.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvAccountsCrewBankAccount.SelectedIndexes.Clear();
        gvAccountsCrewBankAccount.EditIndexes.Clear();
        gvAccountsCrewBankAccount.DataSource = null;
        gvAccountsCrewBankAccount.Rebind();
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataSet ds = new DataSet();
            string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDPASSPORTNO", "FLDBANKSWIFTCODE", "FLDBANKIFSCCODE", "FLDACCOUNTNAME", "FLDACCOUNTNUMBER", "FLDACCOUNTTYPENAME", "FLDBANKNAME", "FLDCURRENTVESSEL", "FLDVERIFIEDYN", "FLDACTIVEINACTIVENAME", "FLDLASTEDITEDBY", "FLDMODIFIEDDATE", "FLDCURRENCYCODE", "FLDVERIFIEDBY", "FLDVERIFIEDDATE", "FLDINACTIVEREMARKS", "FLDACCONTOPENEDBY" };
            string[] alCaptions = { "File No", "Employee Name", "Passport Number", "Bank SWIFT Code", "IFSC Code", "Beneficiary Name", "Account Number", "Account Type", "Seafarer Bank", "Current Vessel", "Verified YN", "Status", "Last Edited By", "Last Edited Date", "Currency", "Verified By", "Verified On", "Remarks", "Account Opened By" };
            string sortexpression;
            int? sortdirection = null;
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            ds = PheonixAccountCrewBankAccount.CrewBankAccountSearch(
                null
                , sortexpression, sortdirection
                , int.Parse(ViewState["PAGENUMBER"].ToString())
                , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                , ref iRowCount
                , ref iTotalPageCount
                , General.GetNullableString(txtFileNumber.Text)
                , General.GetNullableString(txtName.Text)
                , General.GetNullableString(txtPassortNo.Text)
                , General.GetNullableString(txtBankAccountNo.Text)
                , General.GetNullableDateTime(txtFrom.Text)
                , General.GetNullableDateTime(txtTo.Text)
                , General.GetNullableInteger(chkonboard.Checked == true ? "" : "1")
                , General.GetNullableInteger(chkVerifiedbank.Checked == true ? "" : "0"));
            General.ShowExcel("Bank Account", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void AccountsCrewBankAccount_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
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
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                txtBankAccountNo.Text = string.Empty;
                txtFileNumber.Text = string.Empty;
                txtFrom.Text = string.Empty;
                txtName.Text = string.Empty;
                txtPassortNo.Text = string.Empty;
                txtTo.Text = string.Empty;
                chkonboard.Checked = false;
                chkVerifiedbank.Checked = false;
                Rebind();
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
        Rebind();
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDPASSPORTNO", "FLDBANKSWIFTCODE", "FLDBANKIFSCCODE", "FLDACCOUNTNAME", "FLDACCOUNTNUMBER", "FLDACCOUNTTYPENAME", "FLDBANKNAME", "FLDCURRENTVESSEL", "FLDVERIFIEDYN", "FLDACTIVEINACTIVENAME", "FLDLASTEDITEDBY", "FLDMODIFIEDDATE", "FLDCURRENCYCODE", "FLDVERIFIEDBY", "FLDVERIFIEDDATE", "FLDINACTIVEREMARKS", "FLDACCONTOPENEDBY" };
            string[] alCaptions = { "File No", "Employee Name", "Passport Number", "Bank SWIFT Code", "IFSC Code", "Beneficiary Name", "Account Number", "Account Type", "Seafarer Bank", "Current Vessel", "Verified YN", "Status", "Last Edited By", "Last Edited Date", "Currency", "Verified By", "Verified On", "Remarks", "Account Opened By" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PheonixAccountCrewBankAccount.CrewBankAccountSearch(
                null, sortexpression, sortdirection, int.Parse(ViewState["PAGENUMBER"].ToString())
                , gvAccountsCrewBankAccount.PageSize, ref iRowCount, ref iTotalPageCount
                , General.GetNullableString(txtFileNumber.Text), General.GetNullableString(txtName.Text)
                , General.GetNullableString(txtPassortNo.Text), General.GetNullableString(txtBankAccountNo.Text)
                , General.GetNullableDateTime(txtFrom.Text), General.GetNullableDateTime(txtTo.Text)
                , General.GetNullableInteger(chkonboard.Checked == true ? "" : "1")
                , General.GetNullableInteger(chkVerifiedbank.Checked == true ? "" : "0"));
            General.SetPrintOptions("gvAccountsCrewBankAccount", "Bank Account", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataColumn GroupBy = new DataColumn();
                GroupBy.ColumnName = "FLDGROUPBY";
                ds.Tables[0].Columns.Add(GroupBy);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["FLDEMPLOYEEID"].ToString() != "") {
                        string Vessel = ds.Tables[0].Rows[i]["FLDCURRENTVESSEL"].ToString() == "" ? "" : " / Current Vessel - " + ds.Tables[0].Rows[i]["FLDCURRENTVESSEL"].ToString();
                        ds.Tables[0].Rows[i]["FLDGROUPBY"] = ds.Tables[0].Rows[i]["FLDFILENO"].ToString() + " / " + ds.Tables[0].Rows[i]["FLDNAME"].ToString() + " / " + ds.Tables[0].Rows[i]["FLDPASSPORTNO"].ToString() + Vessel;
                    }
                }         
            }
            gvAccountsCrewBankAccount.DataSource = ds;
            gvAccountsCrewBankAccount.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void gvAccountsCrewBankAccount_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {              
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                RadLabel l = (RadLabel)e.Item.FindControl("lblAccountId");
                eb.Attributes.Add("onclick", "parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsCrewBankAccountList.aspx?id=" + l.Text + "');return false;"); 
            }
        }

    }
    protected void gvAccountsCrewBankAccount_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
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
    protected void gvAccountsCrewBankAccount_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAccountsCrewBankAccount.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
