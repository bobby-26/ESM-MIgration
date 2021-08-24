using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Elog;
using System.Collections.Specialized;

public partial class Log_ElectronicLogAnnexVIEngineStatus : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            
            BindLogStatus();
            gvEngineStatus.PageSize = 12;
           
        }
        ShowToolBar();
    }
    public void BindLogStatus()
    {
        try
        {

            DataSet ds = PhoenixElog.LogStatusList();
            ddlStatus.DataSource = ds;
            ddlStatus.DataTextField = "FLDSTATUS";
            ddlStatus.DataValueField = "FLDLOGSTATUSID";
            ddlStatus.DataBind();
            DropDownListItem item = new DropDownListItem("All", string.Empty);
            item.Selected = true;
            ddlStatus.Items.Insert(0, item);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("", "Search", "<i class=\"fa fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("", "Clear", "<i class=\"fa fa-eraser\"></i>", "CLEAR");
        toolbar.AddFontAwesomeButton("", "PDF Export", "<i class=\"fa fa-file-pdf\"></i>", "PDF");
        toolbar.AddFontAwesomeButton("", "Add New Record", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        toolbar.AddFontAwesomeButton("", "History", "<i class=\"fa fa-copy-requisition\"></i>", "TRANSCATIONHISTORY");
        toolbar.AddFontAwesomeButton("", "User Guide", "<i class=\"fas fa-question\"></i>", "HELP", ToolBarDirection.Right);

        Tabstrip.MenuList = toolbar.Show();
        Tabstrip.AccessRights = this.ViewState;
    }
    protected void Tabstrip_TabStripCommand(object sender, EventArgs e)
    {
        try { }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try { }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void ddlStatus_ItemSelected(object sender, Telerik.Web.UI.DropDownListEventArgs e)
    {
        try { }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvEngineStatus_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {

            RadGrid grid = (RadGrid)sender;
            grid.Columns.Clear();
            grid.MasterTableView.ColumnGroups.Clear();

            int iRowCount = 0;
            int iTotalPageCount = 0;

            DateTime? From = General.GetNullableDateTime(txtFromDate.Text);
            DateTime? To = General.GetNullableDateTime(txtToDate.Text);
            int? Status = General.GetNullableInteger(ddlStatus.SelectedValue);

            DataSet ds = PhoenixMarpolLogEngineStatus.EngineStatusRecordSearch(From,To,Status, gvEngineStatus.CurrentPageIndex + 1, gvEngineStatus.PageSize, ref iRowCount , ref iTotalPageCount);

            gvEngineStatus.DataSource = ds;
            gvEngineStatus.VirtualItemCount = iRowCount;
           
            



        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvEngineStatus_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void gvEngineStatus_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    private void AddColumn(string Heading, string Name, HorizontalAlign align, string ColumnGroupName, RadGrid Grid, int width)
    {
        GridTemplateColumn CL = new GridTemplateColumn();
        Grid.Columns.Add(CL);

        CL.HeaderText = Heading;
        CL.UniqueName = Name;
        CL.HeaderStyle.HorizontalAlign = align;
        CL.ItemStyle.HorizontalAlign = align;
        CL.HeaderStyle.Width = width;
        CL.HeaderStyle.Font.Bold = true;
        CL.ColumnGroupName = ColumnGroupName;

    }

    private void AddColumnGroup(string Heading, string Name, string ParentCG , RadGrid Grid , HorizontalAlign align)
    {
        GridColumnGroup CG = new GridColumnGroup();
        Grid.MasterTableView.ColumnGroups.Add(CG);
        CG.Name = Name;
        CG.HeaderText = Heading;
        CG.HeaderStyle.Font.Bold = true;
        CG.HeaderStyle.HorizontalAlign = align;
        if(General.GetNullableString(ParentCG) != null)
            CG.ParentGroupName = ParentCG;
    }
}