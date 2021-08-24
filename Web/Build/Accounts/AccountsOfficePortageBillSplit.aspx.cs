using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using Telerik.Web.UI;

public partial class AccountsOfficePortageBillSplit : PhoenixBasePage
{
    string vslid, empid, pbid, dtkey;
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            vslid = Request.QueryString["vslid"];
            empid = Request.QueryString["empid"];
            pbid = Request.QueryString["pbid"];
            dtkey = Request.QueryString["dtkey"];
            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Split", "SAVE",ToolBarDirection.Right);                
                MenuPB.AccessRights = this.ViewState;
                MenuPB.MenuList = toolbar.Show();
                txtSplitAmount.Text = Request.QueryString["amt"].ToString();
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void MenuPB_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string date = string.Empty;
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                {
                if (!IsValidSplit())
                {
                    ucError.Visible = true;
                    return;
                }
                string splitamt = string.Empty;
                UserControlMaskNumber number;
                for (int i = 1; i <= 5; i++)
                {
                    number = ((UserControlMaskNumber)this.Page.FindControl("txtAmount" + i));
                    if (number != null && General.GetNullableDecimal(number.Text).HasValue)
                    {
                        splitamt += number.Text + ",";
                    }
                }
                splitamt = splitamt.TrimEnd(',');
                PhoenixAccountsOfficePortageBill.UpdateOfficePortageBillPostingSplit(int.Parse(vslid), new Guid(pbid)
                    , int.Parse(empid), new Guid(dtkey), splitamt, decimal.Parse(txtSplitAmount.Text));
                ucStatus.Text = "Amount Splitting Done.";
            }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1','ifMoreInfo',null);", true);
               
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidSplit()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        decimal splitamt = decimal.Parse(txtSplitAmount.Text);
        decimal amt = 0;
        UserControlMaskNumber number;
        for (int i = 1; i <= 5; i++)
        {
            number = ((UserControlMaskNumber)this.Page.FindControl("txtAmount" + i));
            if (number !=  null && General.GetNullableDecimal(number.Text).HasValue)
            {
                amt += General.GetNullableDecimal(number.Text).Value;
            }
        }
        if (amt != splitamt)
        {
            ucError.ErrorMessage = "Amount added is not equal to original amount";
        }
        return (!ucError.IsError);
    }
}
