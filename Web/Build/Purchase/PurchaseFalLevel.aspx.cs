using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PurchaseFalLevel : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseFalLevel.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseFalLevel.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("javascript:parent.openNewWindow('Add','','Purchase/PurchaseFalLevelAdd.aspx','false','620px','400px')", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseFalLevel.aspx", "Include All", "<i class=\"fa fa-check-circle\"></i>", "SELECTALL");

            MenuFalLevel.AccessRights = this.ViewState;
            MenuFalLevel.MenuList = toolbar.Show();


            PhoenixToolbar tool = new PhoenixToolbar();
            tool.AddButton("Rules Config", "RULESCONFIG", ToolBarDirection.Left);
            tool.AddButton("FAL Level", "FALLEVEL", ToolBarDirection.Left);
            tool.AddButton("User Approve", "USERAPPROVE", ToolBarDirection.Left);
            MenuPurchaseConfig.MenuList = tool.Show();
            MenuPurchaseConfig.SelectedMenuIndex = 1;

            menuload();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["LEVELID"] = string.Empty;
                gvFalLevel.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvFalLevelMapping.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        t.AddFontAwesomeButton("javascript:parent.openNewWindow('Add','','Purchase/PurchaseFalLevelMappingAdd.aspx?LEVELID=" + ViewState["LEVELID"] + "','false','620px','400px')", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");

        //ScriptManager.GetCurrent(this).RegisterPostBackControl(MenuFalLevelMapping);
        MenuFalLevelMapping.AccessRights = this.ViewState;
        MenuFalLevelMapping.MenuList = t.Show();
    }

    protected void MenuFalLevel_TabStripCommand(object sender, EventArgs e)
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

                txtname.Text = "";
                Rebind();
            }
            if (CommandName.ToUpper().Equals("SELECTALL"))
            {

                PhoenixPurchaseFalLevel.PurchaseFalLevelAllRequiredUpdate
                  (
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

    protected void Rebind()
    {
        gvFalLevel.SelectedIndexes.Clear();
        gvFalLevel.EditIndexes.Clear();
        gvFalLevel.DataSource = null;
        gvFalLevel.Rebind();

        gvFalLevelMapping.SelectedIndexes.Clear();
        gvFalLevelMapping.EditIndexes.Clear();
        gvFalLevelMapping.DataSource = null;
        gvFalLevelMapping.Rebind();


    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvFalLevel.Rebind();
            gvFalLevelMapping.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvFalLevel_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {

        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFalLevel.CurrentPageIndex + 1;
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

            DataTable ds = PhoenixPurchaseFalLevel.PurchaseFalLevelSearch(
                General.GetNullableString(txtname.Text),
                  gvFalLevel.CurrentPageIndex + 1,
                  gvFalLevel.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

            gvFalLevel.DataSource = ds;
            gvFalLevel.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFalLevel_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;

                RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkRequiredYN");
                cb.Enabled = SessionUtil.CanAccess(this.ViewState, "REQUIRED");
                cb.Checked = DataBinder.Eval(e.Item.DataItem, "FLDREQUIRED").ToString().Equals("0") ? false : true;

                Guid? LevelId = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblId")).Text);
                LinkButton edit = ((LinkButton)item.FindControl("cmdEdit"));
                if (edit != null)
                {
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','','Purchase/PurchaseFalLevelEdit.aspx?LEVELID=" + LevelId + "','false','620px','400px');return false");
                }


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

    protected void gvFalLevel_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("FALDELETE"))
            {
                Guid? Id = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblId")).Text);
                PhoenixPurchaseFalLevel.PurchaseFalLevelDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Id);
                Rebind();
            }

            if (e.CommandName.ToUpper().Equals("LEVEL"))
            {
                ViewState["LEVELID"] = ((RadLabel)e.Item.FindControl("lblId")).Text;
                menuload();
                Rebind();
            }

            if (e.CommandName.ToString().ToUpper().Equals("REQUIRED"))
            {
                int RequiredYN = (((RadCheckBox)e.Item.FindControl("chkRequiredYN")).Checked == true) ? 1 : 0;
                Guid? LevelId = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblId")).Text);

                PhoenixPurchaseFalLevel.PurchaseFalLevelRequiredUpdate
                 (
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    RequiredYN,
                    LevelId
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

    protected void gvFalLevelMapping_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFalLevelMapping.CurrentPageIndex + 1;
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

            DataTable ds = PhoenixPurchaseFalLevel.PurchaseFalLevelMappingSearch(
               General.GetNullableGuid(ViewState["LEVELID"].ToString()),
                  gvFalLevel.CurrentPageIndex + 1,
                  gvFalLevel.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

            gvFalLevelMapping.DataSource = ds;
            gvFalLevelMapping.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvFalLevelMapping_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("MAPDELETE"))
            {
                Guid? Id = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblLevelMappingId")).Text);
                PhoenixPurchaseFalLevel.FalLevelMappingDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Id);
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFalLevelMapping_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdMapDelete");
                if (db != null)
                {
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                }
            }

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;

                Guid? LevelMappingId = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblLevelMappingId")).Text);
                Guid? LevelId = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblLevelId")).Text);
                LinkButton edit = ((LinkButton)item.FindControl("cmdEdit"));
                if (edit != null)
                {
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','','Purchase/PurchaseFalLevelMappingEdit.aspx?LEVELMAPPINGID=" + LevelMappingId + "&LEVELID="+ LevelId + "','false','620px','400px');return false");
                }

                LinkButton appliesto = ((LinkButton)item.FindControl("btnappliesto"));
                if (appliesto != null)
                {
                    appliesto.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','List of Vessel','Purchase/PurchaseFalLevelMappingVesselList.aspx?LEVELMAPPINGID=" + LevelMappingId + "','false','400px','320px');return false");
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
