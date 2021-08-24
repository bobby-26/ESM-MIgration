using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsStatementOfAccountsSummary : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarReport = new PhoenixToolbar();
            toolbarReport.AddButton("Show Report", "SHOW",ToolBarDirection.Right);
            MenuReports.AccessRights = this.ViewState;
            MenuReports.MenuList = toolbarReport.Show();
            BindCompany();
            txtAccountId.Attributes.Add("style", "display:none;");
            txtVesselId.Attributes.Add("style", "display:none;");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindCompany()
    {
        try
        {
            DataSet ds = PhoenixRegistersCompany.EditCompany(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    ucCurrency.SelectedCurrency =dr["FLDREPORTINGCURRENCY"].ToString();
                    ucCurrency.Enabled = false;
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void txtAccountCode_changed(object sender, EventArgs e)
    {
        
    }
    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
       
    }
    
    protected void MenuReports_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SHOW"))
            {
                int ACC =Convert.ToInt32(txtAccountId.Text);
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?AccountId="+txtAccountId.Text+ "&fromdate=" + ucfromDate.Text+ "&Todate=" + uctoDate.Text + "&applicationcode=5&reportcode=STATEMENTOFACCOUNTSUMMARY", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    
    }


