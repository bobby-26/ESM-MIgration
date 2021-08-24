using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsOfficePortageBill : PhoenixBasePage
{
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
            MenuPB.SelectedMenuIndex = 0;
            if (!IsPostBack)
            {
                NameValueCollection nvc = Filter.CurrentOfficePBFilter;
                ViewState["DATE"] = null;
                ViewState["PAGENUMBER"] = nvc != null && !string.IsNullOrEmpty(nvc["pno"]) ? int.Parse(nvc["pno"]) : 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ddlStatus.SelectedValue = nvc != null && !string.IsNullOrEmpty(nvc["status"]) ? nvc["status"] : string.Empty;
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
    protected void MenuPB_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("VOUCHER"))
            {
                Response.Redirect("AccountsOfficePortageBillPosting.aspx", true);
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
    protected void FilterChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;       
        Rebind();
    }
    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;

            string[] alColumns = { "FLDFROMDATE", "FLDTODATE", "FLDOPENINGBALANCE", "FLDCLOSINGBALANCE" };
            string[] alCaptions = { "From Date", "To Date", "Opening Balance", "Closing Balance" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixAccountsOfficePortageBill.OfficePortageBillSearch(General.GetNullableInteger(ddlVessel.SelectedVessel)
                            , byte.Parse(ddlStatus.SelectedValue), sortexpression, sortdirection, Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , gvPB.PageSize, ref iRowCount, ref iTotalPageCount);
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

    protected void gvPB_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
          
            if (e.CommandName.ToUpper().Equals("VIEW"))
            {
                string date = ((RadLabel)e.Item.FindControl("lblDate")).Text;
                string vslid = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                string pbid = ((RadLabel)e.Item.FindControl("lblPortagebillId")).Text;
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("filtervslid", ddlVessel.SelectedVessel);
                nvc.Add("vslid", vslid);
                nvc.Add("pbid", pbid);
                nvc.Add("date", date);
                nvc.Add("status", ddlStatus.SelectedValue);
                nvc.Add("pno", ViewState["PAGENUMBER"].ToString());
                Filter.CurrentOfficePBFilter = nvc;
                Response.Redirect("AccountsOfficePortageBillList.aspx?vslid=" + vslid + "&pbid=" + pbid + "&date=" + date + "&pno=" + ViewState["PAGENUMBER"].ToString(), true);
            }
            else if (e.CommandName.ToUpper().Equals("CONFIRM"))
            {
                string date = ((RadLabel)e.Item.FindControl("lblDate")).Text;
                string vslid = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                string pbid = ((RadLabel)e.Item.FindControl("lblPortagebillId")).Text;
                PhoenixAccountsOfficePortageBill.PopulateVesselPortageBillData(int.Parse(vslid), new Guid(pbid));
                PhoenixAccountsOfficePortageBill.FinalizeOfficePortageBill(int.Parse(vslid), new Guid(pbid));
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("CREDITLEAVE"))
            {
                string date = ((RadLabel)e.Item.FindControl("lblDate")).Text;
                string vslid = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                string pbid = ((RadLabel)e.Item.FindControl("lblPortagebillId")).Text;
                PhoenixAccountsOfficePortageBill.CreditCrewLeaveWages(int.Parse(vslid), DateTime.Parse(date));
                ucStatus.Text = "Leave Days Credited";
                Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message.ToString() + "2";
            ucError.Visible = true;
        }
    }
    protected void gvPB_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton cf = (LinkButton)e.Item.FindControl("cmdConfirm");
            if (cf != null)
            {
                cf.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to Confirm ?'); return false;");
                cf.Visible = SessionUtil.CanAccess(this.ViewState, cf.CommandName);
                if (drv["FLDSTATUS"].ToString() == "1") cf.Visible = false;
            }
            Image nb = (Image)e.Item.FindControl("imgNegBal");
            if (nb != null)
            {
                if (drv["FLDNEGBALANCECREW"].ToString() != string.Empty)
                {
                    nb.Visible = true;
                    nb.ToolTip = drv["FLDNEGBALANCECREW"].ToString();
                }
                else
                {
                    nb.Visible = false;
                }
            }
            LinkButton cdl = (LinkButton)e.Item.FindControl("cmdCreditLeave");
            if (cdl != null)
            {
                if (drv["FLDSTATUS"].ToString() == "1" && drv["FLDLEAVECREDITED"].ToString() == string.Empty) cdl.Visible = true;
                else cdl.Visible = false;
            }
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
    
}
