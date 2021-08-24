using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;

public partial class VesselAccountsEarningDeductionPurpose : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE");
        MenuPurpose.AccessRights = this.ViewState;
        MenuPurpose.MenuList = toolbarmain.Show();
        if (!IsPostBack)
        {
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
                txtPurpose.Text = dt.Rows[0]["FLDPURPOSE"].ToString();
        }
    }

    protected void MenuPurpose_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (General.GetNullableGuid(ViewState["earningdeductionid"].ToString()) == null)
            {
                lblMessage.Text = "Please update the amount before updating the purpose.";
                return;
            }

            if (General.GetNullableString(txtPurpose.Text) == null)
            {
                lblMessage.Text = "Purpose is required.";
                return;
            }

            string Script = "";

            if (ViewState["earningdeductionid"] != null && ViewState["earningdeductionid"] != null)
            {
                if (dce.CommandName.ToUpper().Equals("SAVE"))
                {
                    PhoenixVesselAccountsEarningDeduction.UpdateEarningDeductionPurpose(General.GetNullableGuid(ViewState["earningdeductionid"].ToString()), txtPurpose.Text);

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