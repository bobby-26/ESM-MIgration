using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using System.Web.UI;

public partial class Presea_PreSeaCandidateFees : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (Request.QueryString["empid"] != null)
            ViewState["empid"] = Request.QueryString["empid"].ToString();
        else
            ViewState["empid"] = "0";

        if (Request.QueryString["batchid"] != null)
            ViewState["batchid"] = Request.QueryString["batchid"].ToString();
        else
            ViewState["batchid"] = "0";

        if (Request.QueryString["interviewid"] != null)
        {
            ViewState["interviewid"] = Request.QueryString["interviewid"].ToString();
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Save", "SAVE");
            toolbarsub.AddButton("Print", "PRINT");
            PreSeaCandidateFees.AccessRights = this.ViewState;
            PreSeaCandidateFees.MenuList = toolbarsub.Show();
        }
        else
        {
            ViewState["interviewid"] = "";
           
        }

    }

    protected void PreSeaCandidateFees_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidFilter())
            {
                ucError.Visible = true;
                return;
            }
            else
            {
                PhoenixPreSeaFees.CandidateFeesInsert(int.Parse(ViewState["empid"].ToString()), int.Parse(ViewState["interviewid"].ToString()), int.Parse(ucFeesType.SelectedFees), decimal.Parse(ucAmount.Text), txtBankDetails.Text);
                ucStatus.Text = "Information Updated.";
                ucStatus.Visible = true;
                
            }
        }

        if (dce.CommandName.ToUpper().Equals("PRINT"))
        {
            if (ucFeesType.SelectedFees == "" || ucFeesType.SelectedFees.ToUpper().Equals("DUMMY"))
            {
                ucError.ErrorMessage = "Please Select Fees Type";
                ucError.Visible = true;
                return;
            }
            else
            {
                String scriptpopup = String.Format(
                            "javascript:Openpopup('codehelp1', '', '../Reports/ReportsView.aspx?applicationcode=10&reportcode=CASHRECEIPT&employeeid=" + ViewState["empid"].ToString() + "&interviewid=" + ViewState["interviewid"].ToString() + "&batchid=" + ViewState["batchid"].ToString() + "&feestype=" + ucFeesType.SelectedFees + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
        }
    }

    public bool IsValidFilter()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ucFeesType.SelectedFees == "" || ucFeesType.SelectedFees.ToUpper() == "DUMMY")
            ucError.ErrorMessage = "Select Fees Type";

        if (rblPaymentType.SelectedValue == "0")
        {
            if (ucAmount.Text == "")
                ucError.ErrorMessage = "Amount is required.";
        }

        if (rblPaymentType.SelectedValue == "1")
        {
            if (txtBankDetails.Text == "")
                ucError.ErrorMessage = "Bank Details is required.";
        }

        return (!ucError.IsError);
    }
}
