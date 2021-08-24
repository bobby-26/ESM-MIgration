using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Accounts;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class Accounts_AccountsReportSubsidiaryLedger : PhoenixBasePage
{
    public static string strtext;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "ANNUALEDGER",ToolBarDirection.Right);
            AnnualReport.Title = "Subsidiary Ledger";
            AnnualReport.AccessRights = this.ViewState;
            AnnualReport.MenuList = toolbar.Show();

            if (!IsPostBack)
            {                

                ViewState["SUBACCOUNT"] = "";
                ViewState["DEPT"] = "";
                BindCheckBoxList();

                EditUserAccessLevel();
                BindFilterCriteria();
                strtext = "";
            }
           

            //imgShowAccount.Attributes.Add("onclick", "return showAccountPickList('spnPickListExpenseAccount', 'codehelp1', '', '../Common/CommonPickListAccount.aspx',true); ");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void AnnualReport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("ANNUALEDGER"))
            {
                if (!IsValidAccountFilter(ViewState["SUBACCOUNT"].ToString(), ucFromDate.Text, ucToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    NameValueCollection criteria = new NameValueCollection();
                    criteria.Add("rblAccount", rblAccount.SelectedValue);
                    criteria.Add("ucFromDate", ucFromDate.Text);
                    criteria.Add("ucToDate", ucToDate.Text);
                    criteria.Add("cblSubAccount", ViewState["SUBACCOUNT"].ToString());
                    criteria.Add("ddlType", ddlType.SelectedValue);
                    criteria.Add("txtAccountSearch", txtAccountSearch.Text);
                    criteria.Add("ucCurrency", ucCurrency.SelectedCurrency);
                    criteria.Add("ddlStatus", ddlStatus.SelectedValue);


                    Filter.CurrentSubsideryLedgerSelection = criteria;

                    if (ddlType.SelectedValue == "1")
                    {
                        Response.Redirect("../Reports/ReportsView.aspx?applicationcode=5&reportcode=SUBSIDARYLEDGER&accountcode=" + criteria.Get("rblAccount") + "&fromdate=" + criteria.Get("ucFromDate") + "&todate=" + criteria.Get("ucToDate") + "&subaccount=" + criteria.Get("cblSubAccount") + "&Status=" + criteria.Get("ddlStatus"), false);
                    }
                    else if (ddlType.SelectedValue == "2")
                    {
                        Response.Redirect("../Reports/ReportsView.aspx?applicationcode=5&reportcode=SUBSIDARYLEDGERREPORTCURRENCY&accountcode=" + criteria.Get("rblAccount") + "&fromdate=" + criteria.Get("ucFromDate") + "&todate=" + criteria.Get("ucToDate") + "&subaccount=" + criteria.Get("cblSubAccount") + "&Status=" + criteria.Get("ddlStatus"), false);
                    }
                    else if (ddlType.SelectedValue == "3")
                    {
                        if (ucCurrency.SelectedCurrency == "Dummy")
                        {
                            Response.Redirect("../Reports/ReportsView.aspx?applicationcode=5&reportcode=SUBSIDARYLEDGERREPORTPRIMECURRENCY&accountcode=" + criteria.Get("rblAccount") + "&fromdate=" + criteria.Get("ucFromDate") + "&todate=" + criteria.Get("ucToDate") + "&subaccount=" + criteria.Get("cblSubAccount") + "&currency=" + criteria.Get("ucCurrency"), false);
                        }
                        else
                        {
                            Response.Redirect("../Reports/ReportsView.aspx?applicationcode=5&reportcode=SUBSIDARYLEDGERREPORTPRIMEWITHCURRENCY&accountcode=" + criteria.Get("rblAccount") + "&fromdate=" + criteria.Get("ucFromDate") + "&todate=" + criteria.Get("ucToDate") + "&subaccount=" + criteria.Get("cblSubAccount") + "&currency=" + criteria.Get("ucCurrency"), false);
                        }
                    }


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
        if(Filter.CurrentSubsideryLedgerSelection != null)
        {
            NameValueCollection nvc = Filter.CurrentSubsideryLedgerSelection;
            ucFromDate.Text = nvc.Get("ucFromDate");
            ucToDate.Text = nvc.Get("ucToDate");
            txtAccountSearch.Text = nvc.Get("txtAccountSearch");
            BindCheckBoxList();
            //ViewState["ACCOUNTSEARCH"] = nvc.Get("txtAccountSearch");
            ddlType.SelectedValue = nvc.Get("ddlType");
            rblAccount.SelectedValue = nvc.Get("rblAccount");
            DataSet ds1 = new DataSet();
            ds1 = PhoenixRegistersAccount.SubAccountListForReport(General.GetNullableInteger(rblAccount.SelectedValue));
            cblSubAccount.DataTextField = "FLDSUBACCOUNT";
            cblSubAccount.DataValueField = "FLDSUBACCOUNTMAPID";
            cblSubAccount.DataSource = ds1;
            cblSubAccount.DataBind();

            string subaccount = "," + nvc.Get("cblSubAccount");

            DataSet ds2 = new DataSet();
            ds2 = PhoenixRegistersAccount.SubAccountListSplit(General.GetNullableString(subaccount.ToString()));

            StringBuilder strsubaccount = new StringBuilder();
            strsubaccount.Append("," + ViewState["SUBACCOUNT"].ToString());

            foreach (DataRow dr in ds2.Tables[0].Rows)
            {
                foreach (ListItem item in cblSubAccount.Items)
                {
                    if(dr["SUBACCOUNT"].ToString() == item.Value)
                    {
                        item.Selected = true;
                        strsubaccount.Append(item.Value.ToString());
                        strsubaccount.Append(",");                        
                    }
                }
            }
            ViewState["SUBACCOUNT"] = strsubaccount.ToString().StartsWith(",") ? strsubaccount.ToString().Remove(0, 1) : strsubaccount.ToString();
            strtext = "";
        }
    }

    protected void BindCheckBoxList()
    {
        DataSet ds = new DataSet();

        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            ds = PhoenixRegistersAccount.AccountListforReport(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                , txtAccountSearch.Text 
                , null
                , null
                , null
                , null
                , null
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

            rblAccount.DataTextField = "FLDaccoandept";
            rblAccount.DataValueField = "FLDACCOUNTID";
            rblAccount.DataSource = ds;

            rblAccount.DataBind();

           // SelectAccount();
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
            ds = PhoenixRegistersAccount.SubAccountListForReport(General.GetNullableInteger(ViewState["ACCOUNT"].ToString()));

            cblSubAccount.DataTextField = "FLDSUBACCOUNT";
            cblSubAccount.DataValueField = "FLDSUBACCOUNTMAPID";
            cblSubAccount.DataSource = ds;

            cblSubAccount.DataBind();
            strtext = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SelectAccount()
    {
        string str = "," + ViewState["ACCOUNT"].ToString();

        foreach (ListItem item in rblAccount.Items)
        {
            if (str.Contains("," + item.Value.ToString() + ","))
            {
                item.Selected = true;
            }
        }
        strtext = "";
    }

    protected void AccountSelection(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();

        try
        {
            ds = PhoenixRegistersAccount.SubAccountListForReport(General.GetNullableInteger(rblAccount.SelectedValue));

            cblSubAccount.DataTextField = "FLDSUBACCOUNT";
            cblSubAccount.DataValueField = "FLDSUBACCOUNTMAPID";
            cblSubAccount.DataSource = ds;

            cblSubAccount.DataBind();
            strtext = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SubAccountSelection(object sender, EventArgs e)
    {
        StringBuilder straccount = new StringBuilder();

        straccount.Append("," + ViewState["SUBACCOUNT"].ToString());

        foreach (ListItem item in cblSubAccount.Items)
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

        ViewState["SUBACCOUNT"] = straccount.ToString().StartsWith(",") ? straccount.ToString().Remove(0, 1) : straccount.ToString();
        strtext = "";
    }

    private bool IsValidAccountFilter(string acc, string fromdate, string todate)
    {
        DateTime resultdate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (acc.Trim().Length == 0)
            ucError.ErrorMessage = "Select atleast one  Sub account.";

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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        
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
                //txtCompany.Text = PhoenixSecurityContext.CurrentSecurityContext.CompanyName;
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
    protected void gvSelectedAccount_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            if (strtext == "Bind")
            {

                BindSelectedSubAccount();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindSelectedSubAccount()
    {
        DataSet ds = new DataSet();

        try
        {
            if (strtext == "Bind")
            {
                if (ViewState["SUBACCOUNT"].ToString() != "")
                {
                    ds = PhoenixRegistersAccount.SelectedSubAccountListForReport(General.GetNullableInteger(rblAccount.SelectedValue), General.GetNullableString(ViewState["SUBACCOUNT"].ToString()));
                    gvSelectedAccount.DataSource = ds;
                }
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
        gvSelectedAccount.SelectedIndexes.Clear();
        gvSelectedAccount.EditIndexes.Clear();
        gvSelectedAccount.DataSource = null;
        gvSelectedAccount.Rebind();
    }
    protected void gvSelectedAccount_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string str = "," + ViewState["SUBACCOUNT"].ToString();

                RadLabel lblSubAccountId = (RadLabel)e.Item.FindControl("lblSubAccountId");

                str = str.Replace("," + lblSubAccountId.Text + ",", ",");
                ViewState["SUBACCOUNT"] = str.StartsWith(",") ? str.Remove(0, 1) : str;

                cblSubAccount.SelectedIndex = -1;
                string subaccount = "," + ViewState["SUBACCOUNT"];

                DataSet ds2 = new DataSet();
                ds2 = PhoenixRegistersAccount.SubAccountListSplit(General.GetNullableString(subaccount.ToString()));

                StringBuilder strsubaccount = new StringBuilder();
                strsubaccount.Append("," + ViewState["SUBACCOUNT"].ToString());

                foreach (DataRow dr in ds2.Tables[0].Rows)
                {
                    foreach (ListItem item in cblSubAccount.Items)
                    {
                        if (dr["SUBACCOUNT"].ToString() == item.Value)
                        {
                            item.Selected = true;
                         }
                    }
                }
                ViewState["SUBACCOUNT"] = strsubaccount.ToString().StartsWith(",") ? strsubaccount.ToString().Remove(0, 1) : strsubaccount.ToString();


                //if (ViewState["SUBACCOUNT"].ToString() == "")
                //{
                //    strtext = "";
                //    Rebind();
                //    SubAccountSelection(ViewState["SUBACCOUNT"], e);
                //}
                 Rebind();

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void btnselectedsubaccount_Click(object sender, EventArgs e)
    {
        strtext = "Bind";
        Rebind();
    }
}
