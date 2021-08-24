using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Accounts;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class Accounts_AccountsReportAnnualLedgerDetailed : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Annual Ledger Detailed", "ANNUALLEDGERDETAILED", ToolBarDirection.Right);
            toolbarmain.AddButton("Annual Ledger", "ANNUALLEDGER", ToolBarDirection.Right);
            MenuMainFilter.AccessRights = this.ViewState;
            MenuMainFilter.MenuList = toolbarmain.Show();
            MenuMainFilter.SelectedMenuIndex = 0;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "ANNUALEDGER", ToolBarDirection.Right);
            AnnualReport.AccessRights = this.ViewState;
            AnnualReport.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["ACCOUNT"] = "";
                ViewState["DEPT"] = "";
                BindCheckBoxList();
                BindFilterCriteria();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindCheckBoxList()
    {
        DataSet ds = new DataSet();

        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            ds = PhoenixRegistersAccount.AccountSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                 , txtAccountSearch.Text
                 , null
                 , null
                 , null
                 , null
                 , 1
                 , null, null,
                 1,
                 1000,
                 ref iRowCount,
                 ref iTotalPageCount);

            ds.Tables[0].Columns.Add("FLDaccoandept");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dr["FLDaccoandept"] = dr["FLDACCOUNTCODE"] + "-" + dr["FLDDESCRIPTION"];
            }

            cblAccount.DataSource = ds;
            cblAccount.DataBind();
            SelectAccount();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindSelected()
    {
        DataSet ds = new DataSet();

        try
        {
            ds = PhoenixRegistersAccount.SelectedAccountSearch(ViewState["ACCOUNT"].ToString());
            gvSelectedAccount.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSelectedAccount_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindSelected();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvSelectedAccount.SelectedIndexes.Clear();
        gvSelectedAccount.EditIndexes.Clear();
        gvSelectedAccount.DataSource = null;
        gvSelectedAccount.Rebind();
    }

    protected void AccountSelection(object sender, EventArgs e)
    {
        StringBuilder straccount = new StringBuilder();

        straccount.Append("," + ViewState["ACCOUNT"].ToString());

        foreach (ButtonListItem item in cblAccount.Items)
        {
            if (item.Selected == true && !straccount.ToString().Contains("," + item.Value.ToString() + ","))
            {
                straccount.Append(item.Value.ToString());
                straccount.Append(",");
            }
            if (item.Selected == false && straccount.ToString().Contains("," + item.Value.ToString() + ","))
            {
                straccount.Replace("," + item.Value.ToString() + ",", ",");
            }
        }

        ViewState["ACCOUNT"] = straccount.ToString().StartsWith(",") ? straccount.ToString().Remove(0, 1) : straccount.ToString();
        Rebind();
    }

    protected void SelectAccount()
    {
        string str = "," + ViewState["ACCOUNT"].ToString();

        foreach (ButtonListItem item in cblAccount.Items)
        {
            if (str.Contains("," + item.Value.ToString() + ","))
            {
                item.Selected = true;
            }
        }
    }

    protected void cmdSearchAccount_Click(object sender, EventArgs e)
    {
        BindCheckBoxList();
    }

    protected void AnnualReport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("ANNUALEDGER"))
            {

                if (IsValidAccountFilter(ViewState["ACCOUNT"].ToString(), txtFromDate.Text, txtToDate.Text, ddlType.SelectedValue))
                {
                    NameValueCollection criteria = new NameValueCollection();

                    criteria.Add("txtFromDate", txtFromDate.Text);
                    criteria.Add("txtToDate", txtToDate.Text);
                    criteria.Add("ddlType", ddlType.SelectedValue);
                    criteria.Add("cblAccount", ViewState["ACCOUNT"].ToString());
                    criteria.Add("txtAccountSearch", txtAccountSearch.Text);

                    Filter.CurrentAnnualLedgerSelection = criteria;

                    if (ddlType.SelectedValue.ToString().Equals("1"))
                    {
                        Response.Redirect("../Reports/ReportsView.aspx?applicationcode=5&reportcode=ANNUALEDGERDETAILEDBASE&account=" + criteria.Get("cblAccount") + "&fromdate=" + criteria.Get("txtFromDate") + "&todate=" + criteria.Get("txtToDate"), false);
                    }
                    else
                    {
                        Response.Redirect("../Reports/ReportsView.aspx?applicationcode=5&reportcode=ANNUALEDGERDETAILEDREPORT&account=" + criteria.Get("cblAccount") + "&fromdate=" + criteria.Get("txtFromDate") + "&todate=" + criteria.Get("txtToDate"), false);
                    }
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
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
        if (Filter.CurrentAnnualLedgerSelection != null)
        {
            NameValueCollection nvc = Filter.CurrentAnnualLedgerSelection;
            txtFromDate.Text = nvc.Get("txtFromDate");
            txtToDate.Text = nvc.Get("txtToDate");
            ddlType.SelectedValue = nvc.Get("ddlType");
            txtAccountSearch.Text = nvc.Get("txtAccountSearch");

            string account = "," + nvc.Get("cblAccount");

            DataSet ds2 = new DataSet();
            ds2 = PhoenixRegistersAccount.AccountCodeListSplit(General.GetNullableString(account.ToString()));

            StringBuilder straccount = new StringBuilder();
            straccount.Append("," + ViewState["ACCOUNT"].ToString());

            foreach (DataRow dr in ds2.Tables[0].Rows)
            {
                foreach (ButtonListItem item in cblAccount.Items)
                {
                    if (dr["SUBACCOUNT"].ToString() == item.Value)
                    {
                        item.Selected = true;
                        straccount.Append(item.Value.ToString());
                        straccount.Append(",");
                    }
                }
            }
            ViewState["ACCOUNT"] = straccount.ToString().StartsWith(",") ? straccount.ToString().Remove(0, 1) : straccount.ToString();
            Rebind();
        }
    }

    protected void MenuMainFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("ANNUALLEDGER"))
            {
                Response.Redirect("../Accounts/AccountsReportAnnualLedgerAccount.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidAccountFilter(string acc, string fromdate, string todate, string ddltype)
    {
        DateTime resultdate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (acc.Trim().Length == 0)
            ucError.ErrorMessage = "Select atleast one account.";

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
            ucError.ErrorMessage = "To date should be later than from date";
        }

        if (ddltype.ToString().Equals("0"))
            ucError.ErrorMessage = "Please select type";

        return (!ucError.IsError);
    }

    protected void gvSelectedAccount_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string str = "," + ViewState["ACCOUNT"].ToString();

                RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");

                str = str.Replace("," + lblAccountId.Text + ",", ",");
                ViewState["ACCOUNT"] = str.StartsWith(",") ? str.Remove(0, 1) : str;
                Rebind();
                BindCheckBoxList();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
