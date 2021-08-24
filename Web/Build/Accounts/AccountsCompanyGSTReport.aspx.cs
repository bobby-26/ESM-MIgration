using System;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Collections.Specialized;

public partial class Accounts_AccountsCompanyGSTReport : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuGST.AccessRights = this.ViewState;
            MenuGST.MenuList = toolbargrid.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                ViewState["PAGEURL"] = "../Reports/ReportsView.aspx";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuGST_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                NameValueCollection criteria = new NameValueCollection();

                criteria.Add("txtFromDate", txtFromDate.Text);
                criteria.Add("txtToDate", txtToDate.Text);
                Filter.CurrentAnnualLedgerSelection = criteria;
                if (!IsValidData(txtFromDate.Text, txtToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=MONTHLYGSTREPORT&fromdate=" + criteria.Get("txtFromDate") + "&todate=" + criteria.Get("txtToDate") + "&showmenu=0";

                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidData(string fromdate, string todate)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";

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

        return (!ucError.IsError);
    }

}