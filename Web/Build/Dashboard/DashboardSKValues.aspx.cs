using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.ShippingKPI;
using System.Globalization;
using Telerik.Web.UI;
using System.Text;

public partial class Dashboard_DashboardSKValues : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Performance Data", "value", ToolBarDirection.Left);
        toolbar.AddButton("KPI Target", "KPITarget", ToolBarDirection.Left);
        //toolbar.AddButton("Refresh Data", "REFRESH", ToolBarDirection.Right);
        Tabkpi.MenuList = toolbar.Show();
        Tabkpi.SelectedMenuIndex = 0;

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Dashboard/DashboardSKValues.aspx", "Search", "<i class=\"fa fa-search\"></i>", "SEARCH");
        Tabstripspivalues.MenuList = toolbargrid.Show();

        
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            gvSPIlist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            radcbyear.SelectedYear = DateTime.Now.Year;
            radcbindicator.SelectedValue = "KPI";
            DataTable dt1 = PhoenixDashboardSKSPIValue.SPIValueScopeList();
            Radcbscope.DataSource = dt1;
            Radcbscope.DataTextField = "FLDSCOPE";
            Radcbscope.DataValueField = "FLDSCOPEID";
            Radcbscope.DataBind();
            Radcbscope.SelectedValue = "2";
            if (dt1.Rows.Count > 0)
            {
                radlblscopeselect.Text = Radcbscope.SelectedItem.Text;
                radcbscopeselect.Visible = true;
            }
            int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
            int? scope = General.GetNullableInteger(Radcbscope.SelectedValue);
            DataTable dt = PhoenixDashboardSKPIValue.Scopedetailedlist(rowusercode, scope);
            radcbscopeselect.DataSource = dt;
            radcbscopeselect.DataTextField = "FLDNAME";
            radcbscopeselect.DataValueField = "FLDID";
            radcbscopeselect.DataBind();
            radcbscopeselect.Items[0].Checked = true;
            
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
                gvSPIlist.Rebind();
            }
            if (CommandName.ToUpper().Equals("KPITARGET"))
            {
                Response.Redirect("~/Dashboard/DashboardSKKPITargetSearch.aspx");
            }
            if (CommandName.ToUpper().Equals("REFRESH"))
            {
                PhoenixDashboardSKPIValue.CalculateKPI();
                gvSPIlist.Rebind();
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }



    protected void Tabstripspivalues_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            int? scope = General.GetNullableInteger(Radcbscope.SelectedValue);
            int? year = General.GetNullableInteger(radcbyear.SelectedYear.ToString());
            string filterid = string.Empty;
            filterid = General.GetNullableString(GetCheckedItemsvalues(radcbscopeselect, filterid));
            string indicator = General.GetNullableString(radcbindicator.SelectedValue);

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                if (!IsValidSearch( indicator,scope,filterid,year))
                {
                    ucError.Visible = true;
                    return;
                }
                gvSPIlist.Rebind();
            }


        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvSPIlist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        try
        {
            RadGrid grid = (RadGrid)sender;
            grid.Columns.Clear();
            grid.MasterTableView.ColumnGroups.Clear();
            int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
            int? scope = General.GetNullableInteger(Radcbscope.SelectedValue);
            int? year = General.GetNullableInteger(radcbyear.SelectedYear.ToString());
            string indicator = General.GetNullableString(radcbindicator.SelectedValue);
            string filterid = string.Empty;
            filterid = General.GetNullableString(GetCheckedItemsvalues(radcbscopeselect,filterid));
            int rowcount = 0;


            DataSet ds = PhoenixDashboardSKPIValue.SKValueSearch(rowusercode, scope, year, indicator, filterid, gvSPIlist.CurrentPageIndex+1, gvSPIlist.PageSize,  ref rowcount);

            GridColumnGroup spicolumngroup = new GridColumnGroup();
            grid.MasterTableView.ColumnGroups.Add(spicolumngroup);
            spicolumngroup.Name = indicator;
            spicolumngroup.HeaderText = indicator;
            spicolumngroup.HeaderStyle.Width = 400;
            spicolumngroup.HeaderStyle.Font.Bold = true;
            spicolumngroup.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;



            GridTemplateColumn field1 = new GridTemplateColumn();
            grid.Columns.Add(field1);

            field1.HeaderText = "Code";
            field1.UniqueName = "FLDCODE";
            field1.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            field1.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            field1.HeaderStyle.Width = 75;
            field1.HeaderStyle.Font.Bold = true;
            field1.ColumnGroupName = spicolumngroup.Name;



            GridTemplateColumn field2 = new GridTemplateColumn();
            grid.Columns.Add(field2);
            field2.HeaderText = "Name";
            field2.UniqueName = "FLDNAME";
            field2.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            field2.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            field2.HeaderStyle.Width = 175;
            field2.HeaderStyle.Font.Bold = true;
            field2.ColumnGroupName = spicolumngroup.Name;

            GridTemplateColumn field3 = new GridTemplateColumn();
            grid.Columns.Add(field3);
            field3.UniqueName = "FLDID";
            field3.Visible = false;
            


            if (scope != null)
            {

                DataTable filter = ds.Tables[2];
                DataTable quarters = ds.Tables[1];


                for (int i = 0; i < filter.Rows.Count; i++)
                {
                    GridColumnGroup columngroup = new GridColumnGroup();
                    grid.MasterTableView.ColumnGroups.Add(columngroup);
                    columngroup.Name = filter.Rows[i]["FLDID"].ToString();
                    columngroup.HeaderText = filter.Rows[i]["FLDNAME"].ToString();
                    columngroup.HeaderStyle.Width = 400;
                    columngroup.HeaderStyle.Font.Bold = true;
                    columngroup.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;


                    for (int j = 0; j < quarters.Rows.Count; j++)
                    {
                        GridTemplateColumn field = new GridTemplateColumn();
                        grid.Columns.Add(field);
                        field.HeaderText = quarters.Rows[j]["FLDQUARTER"].ToString();
                        field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        field.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                        field.HeaderStyle.Width = 75;
                        field.HeaderStyle.Font.Bold = true;
                        field.UniqueName = quarters.Rows[j]["FLDQUARTER"].ToString() + filter.Rows[i]["FLDID"].ToString();
                        field.ColumnGroupName = filter.Rows[i]["FLDID"].ToString();
                    }
                }

            }
            
            grid.DataSource = null;
            if (ds.Tables[0].Rows.Count > 0)
            {
                grid.DataSource = ds;
            }
            grid.VirtualItemCount = rowcount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }
    protected void gvSPIlist_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadGrid gv = (RadGrid)sender;
        string indicator = General.GetNullableString(radcbindicator.SelectedValue);


       

        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)item.DataItem;
            item["FLDCODE"].Text = drv["FLDCODE"].ToString();
            item["FLDNAME"].Text =  " <div class='tooltip2' ID="+ drv["FLDCODE"].ToString() + "> "+drv["FLDNAME"].ToString() + "<span class='tooltiptext'>"+ drv["FLDDESCRIPTION"].ToString() + " </span></div>";
            item["FLDID"].Text = drv["FLDID"].ToString();
            DataSet ds = (DataSet)gv.DataSource;
            DataTable filter = ds.Tables[2];
            DataTable quarters = ds.Tables[1];
 
            for (int j = 0; j < filter.Rows.Count; j++)
            {
                for (int k = 0; k < quarters.Rows.Count; k++)
                {
                        DataRow[] values = ds.Tables[3].Select("FLDID ='" + drv["FLDID"].ToString() + "'AND FLDPERIODVALUE = '" + quarters.Rows[k]["FLDQUARTER"].ToString() + "'AND FLDFILTERID ='"+ filter.Rows[j]["FLDID"].ToString() + "'");
                        string value = (values.Length == 0) ? "0.00" : values[0]["FLDVALUE"].ToString();
                        string scopeid = Radcbscope.SelectedValue;
                    if (string.IsNullOrEmpty(value))
                    {
                        value = "0.00";
                    }
                        if (indicator == "SPI")
                        {        
                            item[quarters.Rows[k]["FLDQUARTER"].ToString() + filter.Rows[j]["FLDID"].ToString()].Text = "<a id=" + quarters.Rows[k]["FLDQUARTER"].ToString() + filter.Rows[j]["FLDID"].ToString() + " style='color: #000' href=" + "javascript:parent.openNewWindow('Filter','','Dashboard/DashboardSKKPIValueSearch.aspx?filterid=" + filter.Rows[j]["FLDID"].ToString() + "&spicode=" + drv["FLDCODE"].ToString() + "&year=" + radcbyear.SelectedYear + "&scopeid=" + scopeid+"')" + ">"+value+" <a/>"; 
                        }
                        else if (indicator == "KPI")
                        {
                            item[quarters.Rows[k]["FLDQUARTER"].ToString() + filter.Rows[j]["FLDID"].ToString()].Text = "<a id=" + quarters.Rows[k]["FLDQUARTER"].ToString() + filter.Rows[j]["FLDID"].ToString() + " style='color: #000' href=" + "javascript:parent.openNewWindow('Filter','','Dashboard/DashboardSKPIValueSearch.aspx?filterid=" + filter.Rows[j]["FLDID"].ToString() + "&kpicode=" + drv["FLDCODE"].ToString() + "&year=" + radcbyear.SelectedYear + "&scopeid=" + scopeid + "')" + ">" + value + " <a/>";
                        }
                        else
                        {
                            item[quarters.Rows[k]["FLDQUARTER"].ToString() + filter.Rows[j]["FLDID"].ToString()].Text = value;
                        }                   
                }
            }
        }


    }

    protected void gvSPIlist_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }
    protected void Radcbscope_TextChanged(object sender, EventArgs e)
    {
        radlblscopeselect.Text = Radcbscope.SelectedItem.Text;
        radcbscopeselect.Text = string.Empty;
        radcbscopeselect.ClearSelection();

        int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        int? scope = General.GetNullableInteger(Radcbscope.SelectedValue);
        DataTable dt = PhoenixDashboardSKPIValue.Scopedetailedlist(rowusercode, scope);
        radcbscopeselect.DataSource = dt;
        radcbscopeselect.DataTextField = "FLDNAME";
        radcbscopeselect.DataValueField = "FLDID";
        radcbscopeselect.DataBind();
        radcbscopeselect.Items[0].Checked = true;
        //foreach (RadComboBoxItem item in radcbscopeselect.Items)
        //{
        //    item.Checked = true;
        //}

        GridSearch();

    }

    protected static string GetCheckedItemsvalues(RadComboBox comboBox, string checkednames)
    {
        var sb = new StringBuilder();
        var collection = comboBox.CheckedItems;

        if (collection.Count != 0)
        {
            foreach (var item in collection)
                sb.Append(item.Value + ",");
            checkednames = sb.ToString();
        }
        return checkednames;
    }

    private bool IsValidSearch(string indicator , int? scope , string filterid , int? year)
    {
        ucError.HeaderMessage = "Provide the following required information";
        if (indicator == null)
        {
            ucError.ErrorMessage = "Indicator.";
        }
        if (scope == null)
        {
            ucError.ErrorMessage = "Scope.";
        }
        if (filterid == null)
        {
            ucError.ErrorMessage = "Filter.";
        }
        if (year == null)
        {
            ucError.ErrorMessage = "Year.";
        }
        return (!ucError.IsError);
    }

    protected void radcbindicator_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridSearch();
    }

    public void GridSearch()
    {
        int? scope = General.GetNullableInteger(Radcbscope.SelectedValue);
        int? year = General.GetNullableInteger(radcbyear.SelectedYear.ToString());
        string filterid = string.Empty;
        filterid = General.GetNullableString(GetCheckedItemsvalues(radcbscopeselect, filterid));
        string indicator = General.GetNullableString(radcbindicator.SelectedValue);

        if (!IsValidSearch(indicator, scope, filterid, year))
        {
            ucError.Visible = true;
            return;
        }
        gvSPIlist.Rebind();

    }

    protected void radcbyear_TextChangedEvent(object sender, EventArgs e)
    {
        GridSearch();
    }

    protected void radcbscopeselect_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridSearch();
    }
}