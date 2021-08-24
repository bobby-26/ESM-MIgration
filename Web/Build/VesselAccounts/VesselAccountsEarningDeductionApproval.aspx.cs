using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;

public partial class VesselAccountsEarningDeductionApproval :  PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE");
            MenuApprovalRemarks.AccessRights = this.ViewState;
            MenuApprovalRemarks.MenuList = toolbarmain.Show();
            ViewState["earningdeductionid"] = "";
            if (Request.QueryString["earningdeductionid"] != null && Request.QueryString["earningdeductionid"].ToString() != string.Empty)
            {
                ViewState["earningdeductionid"] = Request.QueryString["earningdeductionid"].ToString();
                EditEarningDeduction();
            }
        }
    }

    protected void EditEarningDeduction()
    {
        if (General.GetNullableGuid(ViewState["earningdeductionid"].ToString()) != null)
        {
            DataTable dt = PhoenixVesselAccountsEarningDeduction.VesselEarningDeductionEdit(General.GetNullableGuid(ViewState["earningdeductionid"].ToString()));
            if (dt.Rows.Count > 0)
                txtApprovalRemarks.Text = dt.Rows[0]["FLDAPPROVALREMARKS"].ToString();
        }
    }

    protected void MenuApprovalRemarks_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (General.GetNullableGuid(ViewState["earningdeductionid"].ToString()) == null)
            {
                lblMessage.Text = "Please update the amount before approving.";
                return;
            }

            if (General.GetNullableString(txtApprovalRemarks.Text) == null)
            {
                lblMessage.Text = "Approval Remarks is required.";
                return;
            }

            string Script = "";

            if (ViewState["earningdeductionid"] != null && ViewState["earningdeductionid"] != null)
            {
                if (dce.CommandName.ToUpper().Equals("SAVE"))
                {
                    PhoenixVesselAccountsEarningDeduction.ApproveEarningDeduction(General.GetNullableGuid(ViewState["earningdeductionid"].ToString()), txtApprovalRemarks.Text);

                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnReloadList('codehelp1','ifMoreInfo',null);";
                    Script += "</script>" + "\n";
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }  
}