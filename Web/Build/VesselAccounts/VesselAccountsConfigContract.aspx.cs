using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class VesselAccountsConfigContract : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../VesselAccounts/VesselAccountsConfigContract.aspx", "Find", "<i class=\"fas fa-search\"></i>", "Search");
            MenuBF.AccessRights = this.ViewState;
            MenuBF.MenuList = toolbar1.Show();
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Configuration", "CONFIGURATION");
            toolbar.AddButton("Contract", "CONTRACT");
            toolbar.AddButton("Brought Forward", "BF");
            //  toolbar.AddButton("Reimbursement/Deduction", "REIMBURSEMENT");
            Mainmenu.AccessRights = this.ViewState;
            Mainmenu.MenuList = toolbar.Show();
            Mainmenu.SelectedMenuIndex = 1;
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCC.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvCC.SelectedIndexes.Clear();
        gvCC.EditIndexes.Clear();
        gvCC.DataSource = null;
        gvCC.Rebind();
    }
    protected void MenuBF_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Mainmenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("CONFIGURATION"))
            {
                Response.Redirect("VesselAccountsConfiguration.aspx", false);
            }
            if (CommandName.ToUpper().Equals("CONTRACT"))
            {
                Response.Redirect("VesselAccountsConfigContract.aspx", false);
            }
            if (CommandName.ToUpper().Equals("BF"))
            {
                Response.Redirect("VesselAccountsConfigBoughtForward.aspx", false);
            }
            if (CommandName.ToUpper().Equals("REIMBURSEMENT"))
            {
                Response.Redirect("VesselAccountsConfigReimbursMonthChange.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindDataCrewContract()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = 0; //DEFAULT DESC SORT
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixVesselAccountsEmployee.SearchVesselCrewContract(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                , txtfileno.Text, General.GetNullableInteger(lstRank.SelectedRank), sortexpression, sortdirection
                , (int)ViewState["PAGENUMBER"], gvCC.PageSize, ref iRowCount, ref iTotalPageCount);

            gvCC.DataSource = dt;
            gvCC.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCC_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton de = (LinkButton)e.Item.FindControl("cmdDelete");
            if (de != null)
            {
                de.Visible = SessionUtil.CanAccess(this.ViewState, de.CommandName);
                de.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            }
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkCrew");
            if (lb != null)
            {
                lb.Attributes.Add("onclick", "javascript:openNewWindow('ContractComp','','" + Session["sitepath"] + "/VesselAccounts/VesselAccountsContractComponent.aspx?cid=" + drv["FLDCONTRACTID"].ToString() + "'); return false;");
            }
            LinkButton re = (LinkButton)e.Item.FindControl("cmdResend");
            if (re != null)
            {
                re.Visible = SessionUtil.CanAccess(this.ViewState, re.CommandName);
                re.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want Resend Contract ?')");
            }
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            RadComboBox ddlactiveyn = (RadComboBox)e.Item.FindControl("ddlactiveyn");
            if (ddlactiveyn != null)
            {
                ddlactiveyn.SelectedValue = drv["FLDACTIVEYN"].ToString();
            }
        }


    }
    protected void gvCC_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCC.CurrentPageIndex + 1;
            BindDataCrewContract();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCC_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("RESEND"))
            {
                string contractid = ((RadLabel)e.Item.FindControl("lblContractId")).Text;
                PhoenixVesselAccountsEmployee.ResendCrewContract(new Guid(contractid), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string LPaydate = ((UserControlDate)e.Item.FindControl("lblPayDate")).Text;
                string LActiveYn = ((RadLabel)e.Item.FindControl("lblActiveYn")).Text;
                string LPbdate = ((UserControlDate)e.Item.FindControl("lblPBDate")).Text;
                string Paydate = ((UserControlDate)e.Item.FindControl("txtPayDate")).Text;
                string ActiveYn = ((RadComboBox)e.Item.FindControl("ddlactiveyn")).SelectedValue;
                string PBDate = ((UserControlDate)e.Item.FindControl("txtPBDate")).Text;
                Guid id = new Guid(((RadLabel)e.Item.FindControl("lblContractId")).Text);
                if (!IsValidPayDate(Paydate, ActiveYn))
                {
                    ucError.Visible = true;
                    return;
                }
                if (LPaydate != Paydate || LActiveYn != ActiveYn || LPbdate != PBDate)
                {
                    PhoenixVesselAccountsEmployee.UpdateCrewContractPayDate(id, PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , DateTime.Parse(Paydate), int.Parse(ActiveYn), General.GetNullableDateTime(PBDate));
                }
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {

                Guid id = new Guid(((RadLabel)e.Item.FindControl("lblContractId")).Text);

                string empid = ((RadLabel)e.Item.FindControl("lblEmpId")).Text;
                PhoenixVesselAccountsEmployee.DeleteCrewContract(id, int.Parse(empid), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidPayDate(string Paydate, string ActiveYn)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableDateTime(Paydate).HasValue)
        {
            ucError.ErrorMessage = "Paydate is required.";
        }
        if (!General.GetNullableInteger(ActiveYn).HasValue)
        {
            ucError.ErrorMessage = "ActiveYn is required.";
        }

        return (!ucError.IsError);
    }

}
