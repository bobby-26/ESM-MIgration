using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersPoolOtherDocMapping : PhoenixBasePage
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersPoolOtherDocMapping.aspx?" + Request.QueryString.ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvPoolDocMap')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");        
        MenuRegistersPoolMaster.AccessRights = this.ViewState;
        MenuRegistersPoolMaster.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();       
        toolbar.AddButton("Medical", "MEDICAL", ToolBarDirection.Right);
        toolbar.AddButton("Other Doc", "OTHERDOC", ToolBarDirection.Right);
        MenuNav.AccessRights = this.ViewState;
        MenuNav.MenuList = toolbar.Show();
        MenuNav.SelectedMenuIndex = 1;

        if (!IsPostBack)
        {
            ViewState["POOLID"] = Request.QueryString["pool"];
        }
    }

    protected void ShowExcel()
    {      
        string[] alColumns = { "FLDPOOLNAME","FLDDOCUMENTNAME"};
        string[] alCaptions = { "Pool", "Document" };
        DataTable dt = PhoenixRegistersPoolOtherDocMapping.ListPoolOtherDoc(General.GetNullableInteger(ViewState["POOLID"] + ""));
        General.ShowExcel("Other Document Mapping", dt, alColumns, alCaptions, null, null);
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

    private void BindData()
    {

        string[] alColumns = { "FLDPOOLNAME", "FLDDOCUMENTNAME" };
        string[] alCaptions = { "Pool", "Document" };

        DataTable dt = PhoenixRegistersPoolOtherDocMapping.ListPoolOtherDoc(General.GetNullableInteger(ViewState["POOLID"] + ""));
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvPoolDocMap", "Other Document Mapping", alCaptions, alColumns, ds);

        if (dt.Rows.Count > 0)
        {
            gvPoolDocMap.DataSource = dt;           
        }
        else
        {
            gvPoolDocMap.DataSource = "";
        }        
    }
    private bool IsValidPool(string pool, string document)
    {
        ucError.HeaderMessage = "Please provide the following required information";       

        if (!General.GetNullableInteger(pool).HasValue)
            ucError.ErrorMessage = "Pool is required.";

        if (!General.GetNullableInteger(document).HasValue)
            ucError.ErrorMessage = "Other Document is required.";

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
                string documentid = ((UserControlDocumentType)e.Item.FindControl("ucDocumentTypeAdd")).SelectedDocumentType;
                if (!IsValidPool(ViewState["POOLID"] + "", documentid))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersPoolOtherDocMapping.InsertPoolOtherDoc(int.Parse(ViewState["POOLID"] + ""), int.Parse(documentid));
                BindData();
                gvPoolDocMap.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string documentid = ((UserControlDocumentType)e.Item.FindControl("ucDocumentTypeEdit")).SelectedDocumentType;
                string dtkey = ((RadLabel)e.Item.FindControl("lblDtkeyEdit")).Text;
                if (!IsValidPool(ViewState["POOLID"] + "", documentid))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersPoolOtherDocMapping.UpdatePoolOtherDoc(General.GetNullableGuid(dtkey).Value, int.Parse(documentid), int.Parse(ViewState["POOLID"] + ""));              
                BindData();
                gvPoolDocMap.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string dtkey = ((RadLabel)e.Item.FindControl("lblDtkey")).Text;
                PhoenixRegistersPoolOtherDocMapping.DeletePoolOtherDoc(General.GetNullableGuid(dtkey).Value);
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

            UserControlDocumentType ucDocument = (UserControlDocumentType)e.Item.FindControl("ucDocumentTypeEdit");
            if (ucDocument != null) ucDocument.SelectedDocumentType = drv["FLDDOCUMENTID"].ToString();
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
