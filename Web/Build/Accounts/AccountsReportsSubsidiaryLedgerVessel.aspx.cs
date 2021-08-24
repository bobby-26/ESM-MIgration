using System;
using System.Data;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class AccountsReportsSubsidiaryLedgerVessel : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Summary Expense", "EXPENSE", ToolBarDirection.Right);
            toolbar.AddButton("SOA Sub A/C", "SOA", ToolBarDirection.Right);
            MenuVesselReport.AccessRights = this.ViewState;
            MenuVesselReport.MenuList = toolbar.Show();

            if (!IsPostBack)
            {

                ViewState["SUBACCOUNT"] = "";
                ViewState["DEPT"] = "";
                BindCheckBoxList();
                EditUserAccessLevel();
                BindFilterCriteria();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuVesselReport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            NameValueCollection criteria = new NameValueCollection();
            criteria.Add("rblAccount", rblAccount.SelectedValue);
            criteria.Add("ucFromDate", ucFromDate.Text);
            criteria.Add("ucToDate", ucToDate.Text);
            criteria.Add("txtFromAccount", txtFromAccount.Text);
            criteria.Add("txtToAccount", txtToAccount.Text);
            criteria.Add("txtAccountSearch", txtAccountSearch.Text);
            criteria.Add("rblVoucherSelection", rblVoucherSelection.SelectedValue);

            Filter.CurrentSubsideryLedgerVesselSelection = criteria;


            if (CommandName.ToUpper().Equals("SOA"))
            {
                if (!IsValidAccountFilter(rblAccount.SelectedValue, txtFromAccount.Text, txtToAccount.Text, ucFromDate.Text, ucToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                Response.Redirect("../Reports/ReportsView.aspx?applicationcode=5&reportcode=SUBSIDARYVESSELLEDGER&accountcode=" + criteria.Get("rblAccount") + "&fromdate=" + criteria.Get("ucFromDate") + "&todate=" + criteria.Get("ucToDate") +
                    "&fromsubaccount=" + criteria.Get("txtFromAccount") + "&tosubaccount=" + criteria.Get("txtToAccount") + "&voucherSelection=" + criteria.Get("rblVoucherSelection"), false);
            }
            if (CommandName.ToUpper().Equals("EXPENSE"))
            {
                if (!IsValidAccountFilter(rblAccount.SelectedValue, txtFromAccount.Text, txtToAccount.Text, ucFromDate.Text, ucToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                Response.Redirect("../Reports/ReportsView.aspx?applicationcode=5&reportcode=SUBSIDARYSUMMARYEXPENSE&accountcode=" + criteria.Get("rblAccount") + "&fromdate=" + criteria.Get("ucFromDate") + "&todate=" + criteria.Get("ucToDate") +
                    "&fromsubaccount=" + criteria.Get("txtFromAccount") + "&tosubaccount=" + criteria.Get("txtToAccount") + "&voucherSelection=" + criteria.Get("rblVoucherSelection"), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindFilterCriteria()
    {
        if (Filter.CurrentSubsideryLedgerVesselSelection != null)
        {
            NameValueCollection nvc = Filter.CurrentSubsideryLedgerVesselSelection;
            ucFromDate.Text = nvc.Get("ucFromDate");
            ucToDate.Text = nvc.Get("ucToDate");
            txtAccountSearch.Text = nvc.Get("txtAccountSearch");
            BindCheckBoxList();
            rblAccount.SelectedValue = nvc.Get("rblAccount");
            txtFromAccount.Text = nvc.Get("txtFromAccount");
            txtToAccount.Text = nvc.Get("txtToAccount");
            rblVoucherSelection.SelectedValue = nvc.Get("rblVoucherSelection");
        }
    }

    protected void BindCheckBoxList()
    {
        DataSet ds = new DataSet();

        try
        {
            ds = PhoenixRegistersAccount.VesselAccountSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                , txtAccountSearch.Text
                , ""
                , null
                , null
                , null
                , null
                , PhoenixSecurityContext.CurrentSecurityContext.UserCode);

            ds.Tables[0].Columns.Add("FLDACCOANDEPT");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dr["FLDACCOANDEPT"] = dr["FLDACCOUNTCODE"] + "-" + dr["FLDDESCRIPTION"];
            }
            rblAccount.DataSource = ds;
            rblAccount.DataBind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidAccountFilter(string acc, string from, string to, string fromdate, string todate)
    {
        DateTime resultdate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (acc.Trim().Length == 0)
            ucError.ErrorMessage = "Select one account.";

        if (General.GetNullableString(from) == null)
            ucError.ErrorMessage = "From sub account is required.";

        if (General.GetNullableString(to) == null)
            ucError.ErrorMessage = "To sub account is required.";

        if (string.IsNullOrEmpty(fromdate))
        {
            ucError.ErrorMessage = "From date is required";
        }
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From date should be earlier than current date";
        }

        if (string.IsNullOrEmpty(todate))
        {
            ucError.ErrorMessage = "To date is required";
        }
        else if (!string.IsNullOrEmpty(fromdate)
            && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "To date should be later than 'From Date'";
        }

        return (!ucError.IsError);
    }

    protected void cmdSearchAccount_Click(object sender, EventArgs e)
    {
        BindCheckBoxList();
    }

    protected void EditUserAccessLevel()
    {
        DataTable dt = new DataTable();

        try
        {
            dt = PheonixAccountsUserAccessList.UserAccountAccessEdit();

            if (dt.Rows.Count > 0)
            {
                txtUserAccess.Text = dt.Rows[0]["FLDACCESS"].ToString();
            }
            else
                txtUserAccess.Text = "Normal";
            txtCompany.Text = PhoenixSecurityContext.CurrentSecurityContext.CompanyName;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
