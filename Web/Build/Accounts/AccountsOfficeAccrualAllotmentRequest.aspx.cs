using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class AccountsOfficeAccrualAllotmentRequest : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
            MenuPB.AccessRights = this.ViewState;
            MenuPB.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                gvPB.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("BACK")) Response.Redirect("AccountsOfficeAccrual.aspx?" + Request.QueryString.ToString(), true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {
        try
        {
            NameValueCollection nvc = Filter.CurrentOfficeAccrualCrew;
            DataTable dt = PhoenixAccountsOfficeAccrual.ListOfficeAccrualAllotmentRequest(int.Parse(nvc.Get("empid")));
            gvPB.DataSource = dt;
            gvPB.VirtualItemCount = dt.Rows.Count;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPB_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DRAFTVIEW"))
            {
                NameValueCollection nvc = new NameValueCollection();
                string empid = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text;
                string cid = ((RadLabel)e.Item.FindControl("lblComponentId")).Text;
                string aid = ((RadLabel)e.Item.FindControl("lblAccountId")).Text;
                string amt = ((LinkButton)e.Item.FindControl("lnkAllotmentReq")).Text;
                string vslid = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                string sgid = ((RadLabel)e.Item.FindControl("lblSignonoffId")).Text;

                nvc.Add("empid", empid);
                nvc.Add("cid", cid);
                nvc.Add("aid", aid);
                nvc.Add("amt", amt);
                nvc.Add("vslid", vslid);
                nvc.Add("sgid", sgid);
                Filter.CurrentOfficeAccrualCrew = nvc;
                string Script = "<script language='javaScript' id='SelectionScript'>" + "\n";
                Script += "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsOfficeAccrualAllotmentRequestDraftView.aspx');";
                Script += "</script>" + "\n";
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "SelectionScript", Script, false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPB_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }
}
