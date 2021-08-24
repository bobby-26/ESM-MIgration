using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using Telerik.Web.UI;

public partial class PurchaseUserApprovalLimit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseUserApprovalLimit.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseUserApprovalLimit.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("javascript:parent.openNewWindow('Add','','Purchase/PurchaseUserApprovalLimitAdd.aspx','false','820px','500px')", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            MenuUserApprovalLimit.AccessRights = this.ViewState;
            MenuUserApprovalLimit.MenuList = toolbar.Show();

            PhoenixToolbar tool = new PhoenixToolbar();
            tool.AddButton("Rules Config", "RULESCONFIG", ToolBarDirection.Left);
            tool.AddButton("FAL Level", "FALLEVEL", ToolBarDirection.Left);
            tool.AddButton("User Approve", "USERAPPROVE", ToolBarDirection.Left);
            MenuPurchaseConfig.AccessRights = this.ViewState;
            MenuPurchaseConfig.MenuList = tool.Show();
            MenuPurchaseConfig.SelectedMenuIndex = 2;


            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                gvUserApprovalLimit.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void MenuUserApprovalLimit_TabStripCommand(object sender, EventArgs e)
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
                ddlStockType.SelectedValue = "";
                Rebind();
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
        gvUserApprovalLimit.SelectedIndexes.Clear();
        gvUserApprovalLimit.EditIndexes.Clear();
        gvUserApprovalLimit.DataSource = null;
        gvUserApprovalLimit.Rebind();

    }
    protected void gvUserApprovalLimit_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvUserApprovalLimit.CurrentPageIndex + 1;
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

            DataTable ds = PhoenixPurchaseUserApprovalLimit.PurchaseUserApprovalLimitSearch(
                General.GetNullableString(ddlStockType.SelectedValue),
                  gvUserApprovalLimit.CurrentPageIndex + 1,
                  gvUserApprovalLimit.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

            gvUserApprovalLimit.DataSource = ds;
            gvUserApprovalLimit.VirtualItemCount = iRowCount;

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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvUserApprovalLimit.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }





    protected void gvUserApprovalLimit_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["ID"] = ((RadLabel)e.Item.FindControl("lblId")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
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

            PhoenixPurchaseUserApprovalLimit.PurchaseUserApproveDelete
                        (PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableGuid(ViewState["ID"].ToString())
                        );

            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvUserApprovalLimit_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? UserApprovalId = General.GetNullableGuid(((RadLabel)item.FindControl("lblId")).Text);
                LinkButton edit = ((LinkButton)item.FindControl("cmdEdit"));
                if (edit != null)
                {
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','','Purchase/PurchaseUserApprovalLimitEdit.aspx?USERAPPROVALLIMITID=" + UserApprovalId + "','false','820px','500px');return false");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
