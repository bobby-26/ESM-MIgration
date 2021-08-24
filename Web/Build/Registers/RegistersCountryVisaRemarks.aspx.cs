using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegisterCountryVisaRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        try
        {
            toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuComment.AccessRights = this.ViewState;
            MenuComment.MenuList = toolbarmain.Show();
            
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                if (Request.QueryString["id"] != null)
                {
                    ViewState["id"] = Request.QueryString["id"];
                }
                else
                    ViewState["id"] = "";
                RemoveEditorToolBarIcons(); // remove the unwanted icons in editor
                gvRemarks.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersCountryVisa.CountryVisaRemarksSearch(int.Parse(ViewState["id"].ToString()),
                                        sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvRemarks.PageSize, ref iRowCount, ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvRemarks.DataSource = ds;
            gvRemarks.VirtualItemCount = iRowCount;
        }
        else
        {
            gvRemarks.DataSource = "";
        }
    }

    private void BindDataEdit(string id)
    {
        DataSet ds = new DataSet();

        ds = PhoenixRegistersCountryVisa.EditCountryVisa(int.Parse(Request.QueryString["id"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtProcedure.Content = ds.Tables[0].Rows[0]["FLDREMARKS"].ToString();
        }
    }

    protected void MenuComment_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (txtProcedure.Content != "")
                {
                    PhoenixRegistersCountryVisa.CountryVisaRemarksInsert(int.Parse(Request.QueryString["id"]), txtProcedure.Content);
                }                
            }

            BindData();
            gvRemarks.Rebind();            
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvRemarks_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
     
        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            txtProcedure.DisableFilter(Telerik.Web.UI.EditorFilters.ConvertFontToSpan);
            txtProcedure.Content = ((LinkButton)e.Item.FindControl("lnkCOmments")).Text;
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvRemarks_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRemarks.CurrentPageIndex + 1;
        BindData();
    }
    protected void RemoveEditorToolBarIcons()
    {
        txtProcedure.EnsureToolsFileLoaded();
        RemoveButton("ImageManager");
        RemoveButton("DocumentManager");
        RemoveButton("FlashManager");
        RemoveButton("MediaManager");
        RemoveButton("TemplateManager");
        RemoveButton("XhtmlValidator");
        RemoveButton("InsertSnippet");
        RemoveButton("ModuleManager");
        RemoveButton("ImageMapDialog");
        RemoveButton("AboutDialog");
        RemoveButton("InsertFormElement");

        txtProcedure.EnsureToolsFileLoaded();
        txtProcedure.Modules.Remove("RadEditorHtmlInspector");
        txtProcedure.Modules.Remove("RadEditorNodeInspector");
        txtProcedure.Modules.Remove("RadEditorDomInspector");
        txtProcedure.Modules.Remove("RadEditorStatistics");
       

    }

    protected void RemoveButton(string name)
    {
        foreach (EditorToolGroup group in txtProcedure.Tools)
        {
            EditorTool tool = group.FindTool(name);
            if (tool != null)
            {
                group.Tools.Remove(tool);
            }
        }

    }

    protected void gvRemarks_SortCommand(object sender, GridSortCommandEventArgs e)
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
