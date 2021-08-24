using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PurchaseFalApprove : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseFalApprove.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseFalApprove.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("javascript:parent.openNewWindow('Add','','Purchase/PurchaseFalApproveAdd.aspx','false','620px','400px')", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseFalApprove.aspx", "Include All", "<i class=\"fa fa-check-circle\"></i>", "SELECTALL");
            MenuPurchaseFalApprove.AccessRights = this.ViewState;
            MenuPurchaseFalApprove.MenuList = toolbar.Show();

            PhoenixToolbar tool = new PhoenixToolbar();
            tool.AddButton("Rules Config", "RULESCONFIG", ToolBarDirection.Left);
            tool.AddButton("FAL Config", "FALAPPROVE", ToolBarDirection.Left);
            tool.AddButton("FAL Level", "FALLEVEL", ToolBarDirection.Left);
            tool.AddButton("User Approve", "USERAPPROVE", ToolBarDirection.Left);

            MenuPurchaseConfig.MenuList = tool.Show();
            MenuPurchaseConfig.SelectedMenuIndex = 1;

            menuload();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["APPROVEID"] = string.Empty;
                gvPurchaseFalApprove.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvFalApproveLimit.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void menuload()
    {
        PhoenixToolbar t = new PhoenixToolbar();
        t.AddFontAwesomeButton("javascript:parent.openNewWindow('Add','','Purchase/PurchaseFalApproveLimitAdd.aspx?APPROVALID=" +  ViewState["APPROVEID"] + "','false','620px','400px')", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
        MenuPurchaseFalApproveLimit.AccessRights = this.ViewState;
        MenuPurchaseFalApproveLimit.MenuList = t.Show();
    }
    protected void MenuPurchaseFalApprove_TabStripCommand(object sender, EventArgs e)
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

            if (CommandName.ToUpper().Equals("SELECTALL"))
            {

                PhoenixPurchaseFalApprove.PurchaseFalAllRequiredUpdate(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    Status());
                            
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


    protected void Rebind()
    {
        gvPurchaseFalApprove.SelectedIndexes.Clear();
        gvPurchaseFalApprove.EditIndexes.Clear();
        gvPurchaseFalApprove.DataSource = null;
        gvPurchaseFalApprove.Rebind();

        gvFalApproveLimit.SelectedIndexes.Clear();
        gvFalApproveLimit.EditIndexes.Clear();
        gvFalApproveLimit.DataSource = null;
        gvFalApproveLimit.Rebind();


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
            if (CommandName.ToUpper().Equals("FALAPPROVE"))
            {
                Response.Redirect("PurchaseFalApprove.aspx");

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



    protected void gvPurchaseFalApprove_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPurchaseFalApprove.CurrentPageIndex + 1;
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

            DataTable ds = PhoenixPurchaseFalApprove.PurchaseFalApproveSearch(General.GetNullableString(ddlStockType.SelectedValue),
                  gvPurchaseFalApprove.CurrentPageIndex + 1,
                  gvPurchaseFalApprove.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

            gvPurchaseFalApprove.DataSource = ds;
            gvPurchaseFalApprove.VirtualItemCount = iRowCount;

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
            gvPurchaseFalApprove.Rebind();
            gvFalApproveLimit.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvPurchaseFalApprove_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
               ViewState["APPROVEID"] = ((RadLabel)e.Item.FindControl("lblId")).Text;
                menuload();
                Rebind();
            }


            if (e.CommandName.ToUpper().Equals("FALDELETE"))
            {

                Guid? Id = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblId")).Text);

                PhoenixPurchaseFalApprove.PurchaseFalApproveDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Id);

                Rebind();
            }



        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvPurchaseFalApprove_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? ApprovalId = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblId")).Text);
                LinkButton edit = ((LinkButton)item.FindControl("cmdEdit"));
                if (edit != null)
                {
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','','Purchase/PurchaseFalApproveEdit.aspx?APPROVALID=" + ApprovalId + "','false','620px','400px');return false");
                }

                LinkButton appliesto = ((LinkButton)item.FindControl("btnappliesto"));
                if (appliesto != null)
                {
                    appliesto.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','List of Vessel','Purchase/PurchaseFalVesselList.aspx?APPROVALID=" + ApprovalId + "','false','400px','320px');return false");
                }

                RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkRequiredYN");
                cb.Enabled = SessionUtil.CanAccess(this.ViewState, "REQUIRED");
                cb.Checked = DataBinder.Eval(e.Item.DataItem, "FLDREQUIRED").ToString().Equals("0") ? false : true;


            }

            if (e.Item is GridEditableItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdFalDelete");
                if (db != null)
                {
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFalApproveLimit_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFalApproveLimit.CurrentPageIndex + 1;
            Bind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Bind()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

          
            DataTable ds = PhoenixPurchaseFalApprove.PurchaseFalApproveLimitSearch(
                   General.GetNullableGuid(ViewState["APPROVEID"].ToString()) == null ? null : General.GetNullableGuid(ViewState["APPROVEID"].ToString()),                 
                   gvFalApproveLimit.CurrentPageIndex + 1,
                   gvFalApproveLimit.PageSize,
                   ref iRowCount,
                   ref iTotalPageCount);

            gvFalApproveLimit.DataSource = ds;
            gvFalApproveLimit.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }

    protected void gvFalApproveLimit_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? ApprovealLimitId = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblApproveLimitId")).Text);
                LinkButton edit = ((LinkButton)item.FindControl("cmdEdit"));
                if (edit != null)
                {
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','','Purchase/PurchaseFalApproveLimitEdit.aspx?APPROVALLIMITID=" + ApprovealLimitId + "','false','620px','400px');return false");
                }
            }

            if (e.Item is GridEditableItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdLimitDelete");
                if (db != null)
                {
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                }
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFalApproveLimit_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("LIMITDELETE"))
            {

                Guid? Id = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblApproveLimitId")).Text);

                PhoenixPurchaseFalApprove.PurchaseFalApproveLimitDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Id);

                Rebind();
            }



        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    
}