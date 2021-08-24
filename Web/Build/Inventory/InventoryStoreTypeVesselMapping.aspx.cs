using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;

public partial class InventoryStoreTypeVesselMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvStoreTypeVesselMapping')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListVessel.aspx?mode=custom', true);", "Vessel List", "<i class=\"fas fa-ship\"></i>", "ADDVESSEL");
            MenuGridStoreType.AccessRights = this.ViewState;  
            MenuGridStoreType.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                gvStoreTypeVesselMapping.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDVESSELCODE", "FLDVESSELNAME" };
            string[] alCaptions = { "Vessel Code", "Vessel Name" };


            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixInventoryStoreTypeVesselMapping.StoreTypeVesselMappingSearch(Request.QueryString["STORETYPEID"].ToString(),
                "", "", PhoenixSecurityContext.CurrentSecurityContext.VesselID, sortexpression, sortdirection,
                gvStoreTypeVesselMapping.CurrentPageIndex + 1,
                gvStoreTypeVesselMapping.PageSize,
                ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvStoreTypeVesselMapping", "StoreTypeVesselMapping", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvStoreTypeVesselMapping.DataSource = ds;
                gvStoreTypeVesselMapping.VirtualItemCount = iRowCount;
            }
            else
            {
                DataTable dt = ds.Tables[0];
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void DeleteStoreTypeVesselMapping(string storetypevesselmappingid)
    {
        try
        {
            PhoenixInventoryStoreTypeVesselMapping.DeleteStoreTypeVesselMapping(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(storetypevesselmappingid));
            BindData();
            gvStoreTypeVesselMapping.Rebind();
  
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            NameValueCollection nvc = Filter.CurrentPickListSelection;
            string vesselid = nvc.Get("lblVesselId").ToString();

            if ((Request.QueryString["STORETYPEID"] != null) && (Request.QueryString["STORETYPEID"] != ""))
            {
                PhoenixInventoryStoreTypeVesselMapping.InsertStoreTypeVesselMapping
                    (
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                     , Request.QueryString["STORETYPEID"].ToString()
                     , vesselid
                    );
            }
            BindData();
            gvStoreTypeVesselMapping.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStoreTypeVesselMapping_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvStoreTypeVesselMapping_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                }

                ElasticButton _doubleClickButton = (ElasticButton)e.Item.Cells[0].Controls[0];
                string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
                e.Item.Attributes["ondblclick"] = _jsDouble;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStoreTypeVesselMapping_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteStoreTypeVesselMapping(((RadLabel)e.Item.FindControl("lblStoreTypeVesselMappingId")).Text);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStoreTypeVesselMapping_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
}
