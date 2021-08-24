using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;


public partial class PurchaseRulesConfiguration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseRulesConfiguration.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseRulesConfiguration.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("javascript:parent.openNewWindow('Add','','Purchase/PurchaseRulesConfigAdd.aspx','false','550px','500px')", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseRulesConfiguration.aspx", "Include All", "<i class=\"fa fa-check-circle\"></i>", "SELECTALL");

            MenuPurchaseRulesConfig.AccessRights = this.ViewState;
            MenuPurchaseRulesConfig.MenuList = toolbar.Show();

            PhoenixToolbar tool = new PhoenixToolbar();
            tool.AddButton("Rules Config", "RULESCONFIG", ToolBarDirection.Left);          
            tool.AddButton("FAL Level", "FALLEVEL", ToolBarDirection.Left);
            tool.AddButton("User Approve", "USERAPPROVE", ToolBarDirection.Left);
            MenuPurchaseConfig.AccessRights = this.ViewState;
            MenuPurchaseConfig.MenuList = tool.Show();
            MenuPurchaseConfig.SelectedMenuIndex = 0;


            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvPurchaseRuleConfig.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);


            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuPurchaseRulesConfig_TabStripCommand(object sender, EventArgs e)
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
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucRules.SelectedPurchaseRules = "";
                ddlStockType.SelectedValue = "";
                Rebind();
            }

            if (CommandName.ToUpper().Equals("SELECTALL"))
            {
                PhoenixPurchaseRules.PurchaseRulesAllRequiredUpdate(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    Status()
                   );

                Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    private int Status()
    {
        if (ViewState["SELECTALL"] == null)
        {
            ViewState["SELECTALL"] = 0;
        }

        int status = int.Parse(ViewState["SELECTALL"].ToString());
        if (status == 0)
        {
            ViewState["SELECTALL"] = 1;
            return 1;
        }
        else
        {
            ViewState["SELECTALL"] = 0;
            return 0;
        }
    }



    protected void gvPurchaseRuleConfig_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPurchaseRuleConfig.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataTable ds = PhoenixPurchaseRules.PurchaseRulesConfigSearch(
                    General.GetNullableGuid(ucRules.SelectedPurchaseRules),
                    General.GetNullableString(ddlStockType.SelectedValue),
                    gvPurchaseRuleConfig.CurrentPageIndex + 1,
                    gvPurchaseRuleConfig.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

            gvPurchaseRuleConfig.DataSource = ds;
            gvPurchaseRuleConfig.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvPurchaseRuleConfig.SelectedIndexes.Clear();
        gvPurchaseRuleConfig.EditIndexes.Clear();
        gvPurchaseRuleConfig.DataSource = null;
        gvPurchaseRuleConfig.Rebind();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvPurchaseRuleConfig.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPurchaseRuleConfig_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["ID"] = ((RadLabel)e.Item.FindControl("lblConfigId")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
            }

            if (e.CommandName.ToString().ToUpper().Equals("REQUIRED"))
            {
                int RequiredYN = (((RadCheckBox)e.Item.FindControl("chkRequiredYN")).Checked == true) ? 1 : 0;
                int NextLevelYN = (((RadCheckBox)e.Item.FindControl("chkNextLevelYN")).Checked == true) ? 1 : 0;
                Guid? Config = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblConfigId")).Text);

                PhoenixPurchaseRules.PurchaseRulesRequiredUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    Config,
                    RequiredYN,
                    NextLevelYN
                    );

                Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }





    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixPurchaseRules.DeletePurchaseRulesConfig(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid(ViewState["ID"].ToString()));

            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvPurchaseRuleConfig_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? ConfigId = General.GetNullableGuid(((RadLabel)item.FindControl("lblConfigId")).Text);
                LinkButton edit = ((LinkButton)item.FindControl("cmdEdit"));
                if (edit != null)
                {
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','','Purchase/PurchaseRulesConfigEdit.aspx?CONFIGID=" + ConfigId + "','false','550px','500px');return false");
                }

                RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkRequiredYN");
                cb.Enabled = SessionUtil.CanAccess(this.ViewState, "REQUIRED");
                cb.Checked = DataBinder.Eval(e.Item.DataItem, "FLDREQUIRED").ToString().Equals("0") ? false : true;

                RadCheckBox nl = (RadCheckBox)e.Item.FindControl("chkNextLevelYN");
                nl.Enabled = SessionUtil.CanAccess(this.ViewState, "REQUIRED");
                nl.Checked = DataBinder.Eval(e.Item.DataItem, "FLDNEXTLEVELREQUIRED").ToString().Equals("0") ? false : true;

                LinkButton appliesto = ((LinkButton)item.FindControl("btnappliesto"));
                if (appliesto != null)
                {
                    appliesto.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','List of Vessel','Purchase/PurchaseRuleConfigVesselList.aspx?CONFIGID=" + ConfigId + "','false','400px','320px');return false");
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuPurchaseConfig_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
           
            if (CommandName.ToUpper().Equals("RULESCONFIG"))
            {
                Response.Redirect("PurchaseRulesConfiguration.aspx");
            }

            if (CommandName.ToUpper().Equals("FALLEVEL"))
            {
                Response.Redirect("PurchaseFalLevel.aspx");
            }

            if (CommandName.ToUpper().Equals("USERAPPROVE"))
            {
                Response.Redirect("PurchaseUserApprovalLimit.aspx");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}