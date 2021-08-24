using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersPoolHardMapping : PhoenixBasePage
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersPoolHardMapping.aspx?" + Request.QueryString.ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvPoolDocMap')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");        
        MenuRegistersPoolMaster.AccessRights = this.ViewState;
        MenuRegistersPoolMaster.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Medical", "MEDICAL", ToolBarDirection.Right);
        toolbar.AddButton("Other Doc", "OTHERDOC", ToolBarDirection.Right);       
        MenuNav.AccessRights = this.ViewState;
        MenuNav.MenuList = toolbar.Show();
        MenuNav.SelectedMenuIndex = 0;

        if (!IsPostBack)
        {
            ViewState["POOLID"] = Request.QueryString["pool"];
            ViewState["HTYPE"] = Request.QueryString["type"];
        }
    }

    protected void ShowExcel()
    {
      
        string[] alColumns = { "FLDPOOLNAME","FLDHARDNAME"};
        string[] alCaptions = { "Pool", "Document" };

        DataTable dt = PhoenixRegistersPoolHardMapping.ListPoolHardMapping(General.GetNullableInteger(ViewState["POOLID"] + ""));
        General.ShowExcel("Medical Mapping", dt, alColumns, alCaptions, null, null);
    }

    protected void MenuNav_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("OTHERDOC"))
        {
            Response.Redirect("RegistersPoolOtherDocMapping.aspx?" + Request.QueryString.ToString(), true);
        }
        else if (CommandName.ToUpper().Equals("MEDICAL"))
        {
            Response.Redirect("RegistersPoolHardMapping.aspx?" + Request.QueryString.ToString(), true);
        }
    }

    protected void RegistersRegistersPoolMaster_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindData()
    {

        string[] alColumns = { "FLDPOOLNAME", "FLDHARDNAME" };
        string[] alCaptions = { "Pool", "Document" };

        DataTable dt = PhoenixRegistersPoolHardMapping.ListPoolHardMapping(General.GetNullableInteger(ViewState["POOLID"] + ""));
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvPoolDocMap", "Medical Mapping", alCaptions, alColumns, ds);

        if (dt.Rows.Count > 0)
        {
            gvPoolDocMap.DataSource = dt;            
        }
        else
        {
            gvPoolDocMap.DataSource = "";
        }        
    }

    protected void gvPoolDocMap_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    private bool IsValidPool(string pool, string document)
    {
        ucError.HeaderMessage = "Please provide the following required information";       

        if (!General.GetNullableInteger(pool).HasValue)
            ucError.ErrorMessage = "Pool is required.";

        if (!General.GetNullableInteger(document).HasValue)
            ucError.ErrorMessage = "Medical is required.";

        return (!ucError.IsError);
    }
  
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvPoolDocMap_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT")) return;
           
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string documentid = ((UserControlHard)e.Item.FindControl("ucHardAdd")).SelectedHard;
                if (!IsValidPool(ViewState["POOLID"] + "", documentid))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersPoolHardMapping.InsertPoolHardMapping(int.Parse(ViewState["POOLID"] + ""), int.Parse(documentid));
                BindData();
                gvPoolDocMap.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string documentid = ((UserControlHard)e.Item.FindControl("ucHardEdit")).SelectedHard;
                string dtkey = ((RadLabel)e.Item.FindControl("lblDtkeyEdit")).Text;
                if (!IsValidPool(ViewState["POOLID"] + "", documentid))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersPoolHardMapping.UpdatePoolHardMapping(General.GetNullableGuid(dtkey).Value, int.Parse(documentid), int.Parse(ViewState["POOLID"] + ""));                
                BindData();
                gvPoolDocMap.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string dtkey = ((RadLabel)e.Item.FindControl("lblDtkey")).Text;
                PhoenixRegistersPoolHardMapping.DeletePoolHardMapping(General.GetNullableGuid(dtkey).Value);
                BindData();
                gvPoolDocMap.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPoolDocMap_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvPoolDocMap_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {       
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            UserControlHard ucDocument = (UserControlHard)e.Item.FindControl("ucHardEdit");
            if (ucDocument != null) ucDocument.SelectedHard = drv["FLDHARDCODE"].ToString();

        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
}
