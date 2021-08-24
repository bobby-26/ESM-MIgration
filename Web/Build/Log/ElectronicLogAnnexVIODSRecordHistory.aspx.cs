using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Elog;
using System.Collections.Specialized;

public partial class Log_ElectronicLogAnnexVIODSRecordHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindLogStatus();
        }
        ShowToolBar();
    }

    private void BindLogStatus()
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
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("", "Search", "<i class=\"fa fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("", "Clear", "<i class=\"fa fa-eraser\"></i>", "CLEAR");


        MenuMain.MenuList = toolbar.Show();
        MenuMain.AccessRights = this.ViewState;
    }

    protected void MenuMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvODS.Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtFromDate.Text = "";
                txtToDate.Text = "";
                ddlStatus.SelectedValue = "";
                gvODS.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlStatus_ItemSelected(object sender, Telerik.Web.UI.DropDownListEventArgs e)
    {
        try
        { 
            gvODS.Rebind();
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvODS_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;


            DateTime? From = General.GetNullableDateTime(txtFromDate.Text);
            DateTime? To = General.GetNullableDateTime(txtToDate.Text);
            int? Status = General.GetNullableInteger(ddlStatus.SelectedValue);

            DataTable dt = PhoenixMarpolLogODS.ODSRecordHistorySearch(From, To, Status, gvODS.CurrentPageIndex + 1, gvODS.PageSize, ref iRowCount, ref iTotalPageCount);

            gvODS.DataSource = dt;
            gvODS.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    
}