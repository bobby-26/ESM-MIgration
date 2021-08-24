using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Accounts;
using System.Data;
using System.Text;
using System.Web.UI;

public partial class Accounts_AccountsSoaCheckingReopenRemarks : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE");
        toolbar.AddButton("Cancel", "CANCEL");

        //if (!IsPostBack)
        //{
            if (Request.QueryString["questionanswerid"] != null)
            {
                ViewState["questionanswerid"] = Request.QueryString["questionanswerid"].ToString();
            }
            else
            {
                ViewState["questionanswerid"] = "";
            }

            if (Request.QueryString["redirect"] != null)
            {
                ViewState["redirect"] = Request.QueryString["redirect"].ToString();
            }
            if (Request.QueryString["voucherid"] != null)
            {
                ViewState["voucherid"] = Request.QueryString["voucherid"].ToString();
            }
        //}

        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.MenuList = toolbar.Show();
    }

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {
            if (string.IsNullOrEmpty(txtRemarks.Text))
            {
                ucError.ErrorMessage = "Remarks has to be entered";
                ucError.Visible = true;
                return;
            }
            else
            {
                try
                {
                    PhoenixAccountsSoaChecking.SoaCheckingQueryAnswerReopen(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["questionanswerid"].ToString()), txtRemarks.Text);
                   // String scriptpopupclose = String.Format("javascript:fnReloadList('Filter', null,null);");
                   // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
                    string pganame = ViewState["redirect"].ToString();
                    Response.Redirect(pganame.ToString() + "?voucherid=" + ViewState["voucherid"].ToString());

                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
        }
        else if (dce.CommandName.ToUpper().Equals("CANCEL"))
        {
            String scriptpopupclose = String.Format("javascript:fnReloadList('Filter', null,null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
        }
    }    
}
