using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Dashboard;

public partial class Dashboard_DashboardInfoEPSS : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddFontAwesomeButton("../Dashboard/DashboardInfoEPSS.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDashboard')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:parent.openNewWindow('codehelp1','Add','" + Session["sitepath"] + "/Dashboard/DashboardInfoEPSSInsert.aspx" + "');", "New Dashboard", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        toolbar.AddFontAwesomeButton("../Dashboard/DashboardInfoEPSS.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");

        MenuDashboard.AccessRights = this.ViewState;
        MenuDashboard.MenuList = toolbar.Show();
        //  MenuFlagCourseDone.SetTrigger(pnlBatch);

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            
            gvDashboard.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        
        //BindData();
    }

    protected void MenuDashboard_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (CommandName.ToUpper().Equals("FIND"))
        {
            BindData();
            txtdashboardsearch.Text = "";            
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCONFIGID", "FLDINFORMATION", "FLDEPSSLINK" };
        string[] alCaptions = { "Config ID", "Information", "EPSS Link" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable ds = PhoenixDashboardInfoEPSS.Search(txtdashboardsearch.Text, sortexpression,sortdirection,Int32.Parse(ViewState["PAGENUMBER"].ToString()),gvDashboard.PageSize,ref iRowCount,ref iTotalPageCount);
        DataTable ds2 = ds.Copy();

        DataSet dt = new DataSet();
        dt.Tables.Add(ds2);
        General.SetPrintOptions("gvDashboard", "Dashboard Info EPSS", alCaptions, alColumns, dt);
        
        gvDashboard.DataSource = ds;
        gvDashboard.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        int iTotalPageCount = 0;
        int iRowCount = 0;
        
        string date = DateTime.Now.ToShortDateString();

        DataTable ds = new DataTable();
        string[] alColumns = { "FLDCONFIGID", "FLDINFORMATION", "FLDEPSSLINK" };
        string[] alCaptions = { "Config ID", "Information", "EPSS Link" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        
            ds = PhoenixDashboardInfoEPSS.Search(txtdashboardsearch.Text, sortexpression, sortdirection, 1, iRowCount, ref iRowCount, ref iTotalPageCount);

        General.ShowExcel("Dashboard Info EPSS",ds, alColumns,alCaptions,null,null );        
    }

    protected void gvDashboard_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null)
                edit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','Edit','" + Session["sitepath"] + "/Dashboard/DashboardInfoEPSSInsert.aspx?id=" + drv["FLDID"].ToString() + "'); return false;");

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDashboard_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDashboard.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDashboard_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("PAGE"))
            {
                ViewState["PAGENUMBER"] = null;
            }
            //if (e.CommandName.ToUpper().Equals("UPDATE"))
            //{
            //    RadTextBox information = (RadTextBox)e.Item.FindControl("txtinformation");
            //    RadTextBox dashboard = (RadTextBox)e.Item.FindControl("txtdashboardname");
            //    Guid id = Guid.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDID"].ToString());
                             
            //        PhoenixDashboardInfoEPSS.UpdateDashboardInfoEPSS(information.Text, dashboard.Text, id );
            //        BindData();                    
            //}
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid id = Guid.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDID"].ToString());
                PhoenixDashboardInfoEPSS.Delete(id);
                            
                BindData();
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvDashboard.Rebind();
    }
    
}