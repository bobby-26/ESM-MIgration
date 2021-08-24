using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class AccountsOfficePortageBillPosting : PhoenixBasePage
{
    string prevslid = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Finalized Portage Bill", "PORTAGEBILL");
            toolbar.AddButton("Voucher", "VOUCHER");
            MenuPB.AccessRights = this.ViewState;
            MenuPB.MenuList = toolbar.Show();
            MenuPB.SelectedMenuIndex = 1;

            ucConfirm.Attributes.Add("style", "display:none;");

            if (!IsPostBack)
            {
                NameValueCollection nvc = Filter.CurrentOfficePBFilter;
                ViewState["DATE"] = null;
                ViewState["PAGENUMBER"] = nvc != null && !string.IsNullOrEmpty(nvc["pno"]) ? int.Parse(nvc["pno"]) : 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ddlVessel.VesselList = PhoenixRegistersVessel.ListVessel(null, General.GetNullableInteger(""), 1);
                ddlVessel.SelectedVessel = nvc != null && !string.IsNullOrEmpty(nvc["filtervslid"]) ? nvc["filtervslid"] : string.Empty;
                gvPB.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvPB.SelectedIndexes.Clear();
        gvPB.EditIndexes.Clear();
        gvPB.DataSource = null;
        gvPB.Rebind();
    }
    protected void MenuPB_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string date = string.Empty;
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("PORTAGEBILL"))
            {
                Response.Redirect("AccountsOfficePortageBill.aspx", true);
            }
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
            int iRowCount = 0;
            int iTotalPageCount = 10;

           // DataSet dt = new DataSet();

            string[] alColumns = { "FLDFROMDATE", "FLDTODATE", "FLDOPENINGBALANCE", "FLDCLOSINGBALANCE" };
            string[] alCaptions = { "From Date", "To Date", "Opening Balance", "Closing Balance" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixAccountsOfficePortageBill.OfficePortageBillPostingSearch(General.GetNullableInteger(ddlVessel.SelectedVessel)
                            , sortexpression, sortdirection, Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), gvPB.PageSize, ref iRowCount, ref iTotalPageCount);

            gvPB.DataSource = dt;

            gvPB.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPB_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }

        if (e.Item is GridDataItem)
        {
            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdRepost");
            string vslid = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
            string vouchernumber = ((RadLabel)e.Item.FindControl("lblVoucherNo")).Text;

            if (prevslid != vslid)
            {
                if (vouchernumber != string.Empty)
                {
                    db1.Visible = true;
                    prevslid = vslid;
                }
            }
        }
    }
    protected void gvPB_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("VIEW"))
            {
                NameValueCollection nvc = new NameValueCollection();

                string pbid = ((RadLabel)e.Item.FindControl("lblPbId")).Text;
                string vslid = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;

                nvc.Add("filtervslid", ddlVessel.SelectedVessel);
                nvc.Add("vslid", vslid);
                nvc.Add("pbid", pbid);
                nvc.Add("date", "");
                nvc.Add("status", "");
                nvc.Add("pno", ViewState["PAGENUMBER"].ToString());
                Filter.CurrentOfficePBFilter = nvc;
                Response.Redirect("AccountsOfficePortageBillPostingBreakUp.aspx?pno=" + ViewState["PAGENUMBER"].ToString() + "&pbid=" + pbid + "&vslid=" + vslid, true);

            }
            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                string pbid = ((RadLabel)e.Item.FindControl("lblPbId")).Text;
                string vslid = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("filtervslid", ddlVessel.SelectedVessel);
                nvc.Add("vslid", vslid);
                nvc.Add("pbid", pbid);
                nvc.Add("date", "");
                nvc.Add("status", "");
                nvc.Add("pno", ViewState["PAGENUMBER"].ToString());
                Filter.CurrentOfficePBFilter = nvc;
                Response.Redirect("AccountsOfficePortageBillPostingBreakUp.aspx?pno=" + ViewState["PAGENUMBER"].ToString() + "&pbid=" + pbid + "&vslid=" + vslid, true);

            }
            else if (e.CommandName.ToUpper().Equals("DRAFT"))
            {
                string pbid = ((RadLabel)e.Item.FindControl("lblPbId")).Text;
                string vslid = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("filtervslid", ddlVessel.SelectedVessel);
                nvc.Add("vslid", vslid);
                nvc.Add("pbid", pbid);
                nvc.Add("date", "");
                nvc.Add("status", "");
                nvc.Add("pno", ViewState["PAGENUMBER"].ToString());
                Filter.CurrentOfficePBFilter = nvc;
                Response.Redirect("AccountsOfficePortageBillPostingDraft.aspx?pno=" + ViewState["PAGENUMBER"].ToString() + "&pbid=" + pbid + "&vslid=" + vslid, true);
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {

                string pbid = ((RadLabel)e.Item.FindControl("lblPbId")).Text;
                string vslid = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                PhoenixAccountsOfficePortageBill.DeleteOfficePortageBill(int.Parse(vslid), new Guid(pbid));
                Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName.ToUpper().Equals("REPOST"))
            {
                ViewState["PbId"] = ((RadLabel)e.Item.FindControl("lblPbId")).Text;
                ViewState["VoucherNo"] = ((RadLabel)e.Item.FindControl("lblVoucherNo")).Text;
                ucConfirm.Visible = true;
                RadWindowManager1.RadConfirm("Please confirm that you want to remove all the ledger entries for this Portage Bill. Standard Billing if done, will need to be revised manually.", "ucConfirm", 320, 150, null, "Confirm");
                return;
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
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPB.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirm_Click(object sender, EventArgs e)
    {

        PhoenixAccountsOfficePortageBill.OfficePortageBillReport(General.GetNullableGuid(ViewState["PbId"].ToString()));

        BindData();
        gvPB.Rebind();
    }
}
