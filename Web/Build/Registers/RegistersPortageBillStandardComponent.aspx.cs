using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersPortageBillStandardComponent : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersPortageBillStandardComponent.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvPBStdComp')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersPortageBillStandardComponentAdd.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuPBStandardComponent.AccessRights = this.ViewState;
            MenuPBStandardComponent.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvPBStdComp.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        string[] alColumns = { "FLDCODE", "FLDLOGTYPENAME", "FLDHARDNAME", "FLDDESCRIPTION", "FLDBUDGETCODE" };
        string[] alCaptions = { "Code", "Type", "Sub Type", "Description", "Budget Code" };
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixRegistersPortageBillStandardComponent.SearchPortageBillComponent(null, null, sortexpression, sortdirection
                                                                            , 1
                                                                            , iRowCount
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount);

        General.ShowExcel("Portage Bill Standard Component", dt, alColumns, alCaptions, null, string.Empty);
    }
    protected void Rebind()
    {
        gvPBStdComp.SelectedIndexes.Clear();
        gvPBStdComp.EditIndexes.Clear();
        gvPBStdComp.DataSource = null;
        gvPBStdComp.Rebind();
    }
    protected void PBStandardComponent_TabStripCommand(object sender, EventArgs e)
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
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
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

        string[] alColumns = { "FLDCODE", "FLDLOGTYPENAME", "FLDHARDNAME", "FLDDESCRIPTION", "FLDBUDGETCODE" };
        string[] alCaptions = { "Code", "Type", "Sub Type", "Description", "Budget Code" };
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? sortdirection = null;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataTable dt = PhoenixRegistersPortageBillStandardComponent.SearchPortageBillComponent(null, null, sortexpression, sortdirection
                                                                            , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                            , gvPBStdComp.PageSize
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvPBStdComp", "Portage Bill Standard Component", alCaptions, alColumns, ds);

        gvPBStdComp.DataSource = dt;
        gvPBStdComp.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    protected void gvPBStdComp_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPBStdComp.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPBStdComp_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                string id = ((RadLabel)e.Item.FindControl("lbldtkeyId")).Text;
                Response.Redirect("RegistersPortageBillStandardComponentAdd.aspx?PortageBillId=" + id, true);
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string id = ((RadLabel)e.Item.FindControl("lbldtkeyId")).Text;
                PhoenixRegistersPortageBillStandardComponent.DeleteAirlines(new Guid(id));
                Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPBStdComp_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

        }
    }

}
