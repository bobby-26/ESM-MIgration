using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using System.Text;
using SouthNests.Phoenix.ShippingKPI;

public partial class Dashboard_DashboardSKKPITargetSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Dashboard/DashboardSKKPITargetSearch.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvKPITargetlist')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("javascript:parent.openNewWindow('Add KPI Target','','Dashboard/DashboardSKKPITargetAdd.aspx','false','700px','300px')", "Add KPI Target", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
        toolbargrid.AddButton("Save", "SAVE", ToolBarDirection.Right);
        TabstripMenu.MenuList = toolbargrid.Show();

        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Performance Data", "value", ToolBarDirection.Left);
        toolbar.AddButton("KPI Target", "KPITarget", ToolBarDirection.Left);

        Tabkpi.MenuList = toolbar.Show();
        Tabkpi.SelectedMenuIndex = 1;
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

           
            radcbyear.SelectedYear = DateTime.Now.Year;

            gvKPITargetlist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void ShowExcel()
    { }

    protected void SPI_TabStripMenuCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                foreach (GridDataItem item in gvKPITargetlist.Items)
                {
                    int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                    int? year = General.GetNullableInteger(radcbyear.SelectedYear.ToString());
                    Guid? kpiid = General.GetNullableGuid(item.GetDataKeyValue("FLDKPIID").ToString());
                    Decimal? KPIminimum = General.GetNullableDecimal(((RadTextBox)item.FindControl("radtxtminimum")).Text);
                    Decimal? KPItarget = General.GetNullableDecimal(((RadTextBox)item.FindControl("radtxttarget")).Text);
                    int? objectiveowner = General.GetNullableInteger(((RadMultiColumnComboBox)item.FindControl("Radcombodesignationlist")).Value);
                    string referencenumber = General.GetNullableString(((RadTextBox)item.FindControl("radtxtreferenceno")).Text);
                    if (!IsValidShippingKPITargetDetails(year, kpiid, KPIminimum, KPItarget))
                    {
                       
                        return;
                    }

                    PhoenixDashboardSKKPITarget.KPITargetInsert(rowusercode, kpiid, KPIminimum, KPItarget, year, objectiveowner, referencenumber);

                }
                ucNotification.Show("Saved Successfully");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }

    private bool IsValidShippingKPITargetDetails(int? year, Guid? kpiid, Decimal? KPIminimum, Decimal? KPItarget)
    {
        ucError.HeaderMessage = "Provide the following required information";
        if (year == null)
        {
            ucError.ErrorMessage = "Year.";
        }
        if (KPIminimum == null)
        {
            ucError.ErrorMessage = "KPI Minimum Value.";
        }
        if (KPItarget == null)
        {
            ucError.ErrorMessage = "KPI Target Value.";
        }



        return (!ucError.IsError);
    }
    protected void gvKPITargetlist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

    



        DataTable dt = PhoenixDashboardSKKPITarget.KPITargetSearch(General.GetNullableInteger(radcbyear.SelectedYear.ToString()),
                                                    gvKPITargetlist.CurrentPageIndex + 1,
                                                gvKPITargetlist.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount);




        gvKPITargetlist.DataSource = dt;
        gvKPITargetlist.VirtualItemCount = iRowCount;

        DataSet ds = dt.DataSet;

      
    }

    public void gvKPITargetlist_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = (DataRowView)item.DataItem;


                RadMultiColumnComboBox Radcombodesignationlist = (RadMultiColumnComboBox)e.Item.FindControl("Radcombodesignationlist");
                DataTable dt = PheonixDashboardSKKPI.ObjectiveOwnerList();
                Radcombodesignationlist.DataSource = dt;
                Radcombodesignationlist.DataBind();
                if (drv["FLDOBJOWNER"] != null)
                {
                    Radcombodesignationlist.Value = drv["FLDOBJOWNER"].ToString();
                }
                
            }

            if (e.Item is GridFilteringItem)
            {
                GridFilteringItem filterItem = (GridFilteringItem)e.Item;
                TextBox textBox = (TextBox)filterItem["FLDDRILLNAME"].Controls[0];
                textBox.MaxLength = 198;

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void KPI_TabStripMenuCommand(object sender, EventArgs e)
    {

        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("VALUE"))
            {
                Response.Redirect("~/Dashboard/DashboardSKValues.aspx");
            }
            if (CommandName.ToUpper().Equals("KPITARGET"))
            {
                Response.Redirect("~/Dashboard/DashboardSKKPITargetSearch.aspx");
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
        try
        {

            gvKPITargetlist.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void radcbyear_TextChangedEvent(object sender, EventArgs e)
    {
        gvKPITargetlist.Rebind();
    }
}