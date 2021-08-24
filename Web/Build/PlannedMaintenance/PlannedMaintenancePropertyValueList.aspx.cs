using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Framework;
using System.Web.Profile;
using Telerik.Web.UI;
public partial class PlannedMaintenancePropertyValueList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Close", "CLOSE", ToolBarDirection.Right);
        MenuRankList.AccessRights = this.ViewState;
        MenuRankList.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            if (Request.QueryString["Componentid"] != null)
            {
                ViewState["COMPONENTID"] = Request.QueryString["Componentid"].ToString();
            }

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvLuboillist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

    }
    protected void Rebind()
    {
        gvLuboillist.SelectedIndexes.Clear();
        gvLuboillist.EditIndexes.Clear();
        gvLuboillist.DataSource = null;
        gvLuboillist.Rebind();
    }
    protected void Rebind1()
    {
        gvElementList.SelectedIndexes.Clear();
        gvElementList.EditIndexes.Clear();
        gvElementList.DataSource = null;
        gvElementList.Rebind();
    }

    private void BindData()
    {

        DataSet ds = PhoenixPlannedMaintenanceLubOilStoreAnalysis.PropertyList(1,General.GetNullableGuid(ViewState["COMPONENTID"].ToString()));
        gvLuboillist.DataSource = ds;
    }

    private void BindData1()
    {

        DataSet ds = PhoenixPlannedMaintenanceLubOilStoreAnalysis.PropertyList(2, General.GetNullableGuid(ViewState["COMPONENTID"].ToString()));
        gvElementList.DataSource = ds;
    }

    protected void gvLuboillist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
    protected void MenuRankList_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("CLOSE"))
        {

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Codehelp2', 'Codehelp1');", true);

        }
    }






    protected void gvElementList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData1();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLuboillist_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidconfigurationupdate(
                 ((RadTextBox)e.Item.FindControl("txtvalue")).Text.Trim()))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                Propertyvalueinsert((General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblvalueidedit")).Text)),
                   (Guid.Parse(((RadLabel)e.Item.FindControl("lblpropertyidedit")).Text)),
                   (Guid.Parse(ViewState["COMPONENTID"].ToString())),
                   (General.GetNullableString(((RadTextBox)e.Item.FindControl("txtvalue")).Text.Trim()))

                  );
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["VALUEID"] = ((RadLabel)e.Item.FindControl("lblvalueid")).Text;
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
    private void Propertyvalueinsert(Guid? valueid,Guid propertyid, Guid componentid, string value)
    {
        PhoenixPlannedMaintenanceLubOilStoreAnalysis.Propertyvalueinsert(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, valueid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, propertyid, componentid, value);
    }
    private void Propertyvaluedelete(Guid id)
    {
        PhoenixPlannedMaintenanceLubOilStoreAnalysis.Propertyvaluedelete(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, id);
    }

    protected void gvLuboillist_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            ImageButton del = (ImageButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);


            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton save = (ImageButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            ImageButton cancle = (ImageButton)e.Item.FindControl("cmdCancel");
            if (cancle != null) cancle.Visible = SessionUtil.CanAccess(this.ViewState, cancle.CommandName);

        }

    }
    protected void gvElementList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidconfigurationupdate(
               ((RadTextBox)e.Item.FindControl("txtvalue")).Text.Trim()))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                Propertyvalueinsert((General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblvalueidedit")).Text)),
                   (Guid.Parse(((RadLabel)e.Item.FindControl("lblpropertyidedit")).Text)),
                   (Guid.Parse(ViewState["COMPONENTID"].ToString())),
                   (General.GetNullableString(((RadTextBox)e.Item.FindControl("txtvalue")).Text.Trim()))

                  );
                Rebind1();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["VALUEID"] = ((RadLabel)e.Item.FindControl("lblvalueid")).Text;
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
            Propertyvaluedelete(Guid.Parse(ViewState["VALUEID"].ToString()));
            Rebind();
            Rebind1();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidconfigurationupdate(string txtvalue)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtvalue.Trim().Equals(""))
            ucError.ErrorMessage = "Value is required.";

        return (!ucError.IsError);
    }

    protected void gvElementList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            ImageButton del = (ImageButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);


            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton save = (ImageButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            ImageButton cancle = (ImageButton)e.Item.FindControl("cmdCancel");
            if (cancle != null) cancle.Visible = SessionUtil.CanAccess(this.ViewState, cancle.CommandName);

        }

    }


}