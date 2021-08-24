using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersContractCrewComponentTrackMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersContractCrewComponentTrackMapping.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewComponentMapping')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersComponentMapping.AccessRights = this.ViewState;
            MenuRegistersComponentMapping.MenuList = toolbar.Show();
            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersContractCrewComponentTrackMapping.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel1");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvReimbursment')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT1");
            MenuRegistersComponentMapping1.AccessRights = this.ViewState;
            MenuRegistersComponentMapping1.MenuList = toolbar.Show();
            if (!IsPostBack)
                ViewState["trackid"] = Request.QueryString["trackid"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewComponentMapping_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
    private void BindData()
    {

        string[] alColumns = { "FLDCOMPONENTNAME" };
        string[] alCaptions = { "Name" };
        DataSet ds = PhoenixRegistersContractComponentTracking.ListCrewComponentTrack(General.GetNullableInteger(ViewState["trackid"].ToString()));
        gvCrewComponentMapping.DataSource = ds.Tables[0];
        gvReimbursment.DataSource = ds.Tables[1];
        General.SetPrintOptions("gvCrewComponentMapping", "Component Track Mapping", alCaptions, alColumns, ds);
        DataSet ds1 = new DataSet();
        ds1.Tables.Add(ds.Tables[1].Copy());
        General.SetPrintOptions("gvReimbursment", "Component Track Mapping", alCaptions, alColumns, ds1);
    }
    protected void RegistersComponentMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                string[] alColumns = { "FLDCOMPONENTNAME" };
                string[] alCaptions = { "Name" };
                DataSet ds = PhoenixRegistersContractComponentTracking.ListCrewComponentTrack(General.GetNullableInteger(ViewState["trackid"].ToString()));
                General.ShowExcel("Component Track Mapping", ds.Tables[0], alColumns, alCaptions, null, string.Empty);
            }
            if (CommandName.ToUpper().Equals("EXCEL1"))
            {
                string[] alColumns = { "FLDCOMPONENTNAME" };
                string[] alCaptions = { "Name" };
                DataSet ds = PhoenixRegistersContractComponentTracking.ListCrewComponentTrack(General.GetNullableInteger(ViewState["trackid"].ToString()));
                General.ShowExcel("Component Track Mapping", ds.Tables[1], alColumns, alCaptions, null, string.Empty);
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
        gvCrewComponentMapping.SelectedIndexes.Clear();
        gvCrewComponentMapping.EditIndexes.Clear();
        gvCrewComponentMapping.DataSource = null;
        gvCrewComponentMapping.Rebind();
        gvReimbursment.SelectedIndexes.Clear();
        gvReimbursment.EditIndexes.Clear();
        gvReimbursment.DataSource = null;
        gvReimbursment.Rebind();
    }
    protected void gvCrewComponentMapping_ItemDataBound(Object sender, GridItemEventArgs e)
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
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
            RadComboBox ddl = (RadComboBox)e.Item.FindControl("ddlComponentAdd");
            if (ddl != null)
            {
                DataTable dt = PhoenixRegistersContract.ListContractCrew(null);
                ddl.DataSource = dt;
                ddl.DataTextField = "FLDCOMPONENTNAME";
                ddl.DataValueField = "FLDCOMPONENTID";
                ddl.DataBind();
                ddl.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            }
        }
    }
    protected void gvCrewComponentMapping_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {

                string componentid = ((RadComboBox)e.Item.FindControl("ddlComponentAdd")).SelectedValue;
                if (!IsValidComponent(componentid))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersContractComponentTracking.InsertCrewComponentTrackMapping(
                                                General.GetNullableInteger(ViewState["trackid"].ToString()),
                                                General.GetNullableGuid(componentid.ToString()), 1);
                Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string dtkey = ((RadLabel)e.Item.FindControl("lbldtkey")).Text;
                PhoenixRegistersContractComponentTracking.DeleteCrewComponentTrackMapping(new Guid(dtkey));
                Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvReimbursment_ItemDataBound(Object sender, GridItemEventArgs e)
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
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
            RadComboBox ddl = (RadComboBox)e.Item.FindControl("ddlComponentAdd");
            if (ddl != null)
            {
                DataSet ds = PhoenixRegistersReimbursementRecovery.ListReimbursementRecovery(0, null, null);
                ddl.DataSource = ds.Tables[0];
                ddl.DataTextField = "FLDHARDNAME";
                ddl.DataValueField = "FLDDTKEY";
                ddl.DataBind();
                ddl.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            }
        }
    }
    protected void gvReimbursment_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {

                string componentid = ((RadComboBox)e.Item.FindControl("ddlComponentAdd")).SelectedValue;
                if (!IsValidComponent(componentid))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersContractComponentTracking.InsertCrewComponentTrackMapping(
                                                General.GetNullableInteger(ViewState["trackid"].ToString()),
                                                General.GetNullableGuid(componentid.ToString()), 2);
                Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string dtkey = ((RadLabel)e.Item.FindControl("lbldtkey")).Text;
                PhoenixRegistersContractComponentTracking.DeleteCrewComponentTrackMapping(new Guid(dtkey));
                Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidComponent(string id)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (id.Equals("Dummy"))
            ucError.ErrorMessage = " Componenet Name is required.";
        return (!ucError.IsError);
    }

}
