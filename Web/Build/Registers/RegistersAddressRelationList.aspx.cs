using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersAddressRelationList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                PhoenixToolbar toolbarAddressRelation = new PhoenixToolbar();
                toolbarAddressRelation.AddButton("Relation", "RELATION",ToolBarDirection.Right);
                toolbarAddressRelation.AddButton("Address", "ADDRESS",ToolBarDirection.Right);
                MenuAddressRelation.AccessRights = this.ViewState;
                MenuAddressRelation.MenuList = toolbarAddressRelation.Show();
                MenuAddressRelation.SelectedMenuIndex = 0;

                if (Request.QueryString["ADDRESSCODE"] != null)
                {
                    ViewState["ADDRESSCODE"] = Request.QueryString["ADDRESSCODE"];
                }
                //BindData();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = PhoenixRegistersAddressRelation.ListAddressRelation(
                int.Parse(ViewState["ADDRESSCODE"].ToString()));

            RadGrid1.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void BindData()
    //{
    //    try
    //    {
    //        DataSet ds = PhoenixRegistersAddressRelation.ListAddressRelation(
    //            int.Parse(ViewState["ADDRESSCODE"].ToString()));

    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            gvAddressRelaion.DataSource = ds;
    //            gvAddressRelaion.DataBind();
    //        }
    //        else
    //        {
    //            DataTable dt = ds.Tables[0];
    //            ShowNoRecordsFound(dt, gvAddressRelaion);
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void AddressRelation_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("ADDRESS"))
            {
                Response.Redirect("../Registers/RegistersAddressRelation.aspx?addresscode=" + ViewState["ADDRESSCODE"]);
            }
            if (CommandName.ToUpper().Equals("RELATION"))
            {
                Response.Redirect("../Registers/RegistersAddressRelationList.aspx?addresscode=" + ViewState["ADDRESSCODE"]);
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvAddressRelation_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //        if (e.CommandName.ToUpper().Equals("DELETEADDRESS"))
    //        {
    //            PhoenixRegistersAddressRelation.DeleteAddressRelation(PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                , int.Parse(ViewState["ADDRESSCODE"].ToString())
    //                , int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAddressCode")).Text)
    //                );
    //            ucStatus.Text = "Address relation is removed";
    //        }
    //        radg
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem eeditedItem = e.Item as GridEditableItem;
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETEADDRESS"))
            {
                PhoenixRegistersAddressRelation.DeleteAddressRelation(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , int.Parse(ViewState["ADDRESSCODE"].ToString())
                    , int.Parse(((RadLabel)eeditedItem.FindControl("lblAddressCode")).Text)
                    );
                ucStatus.Text = "Address relation is removed";
            }
            RadGrid1.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = e.Item.ItemIndex;

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
