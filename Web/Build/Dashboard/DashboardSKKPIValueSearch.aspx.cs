using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.ShippingKPI;
using Telerik.Web.UI;



public partial class Dashboard_DashboardSKKPIValueSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {      
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

            gvKPIlist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            ViewState["FILTERID"] = General.GetNullableString(Request.QueryString["filterid"]);
            ViewState["SCOPEID"] = General.GetNullableString(Request.QueryString["scopeid"]);
            ViewState["SPICODE"] = General.GetNullableString(Request.QueryString["spicode"]);
            ViewState["YEAR"] = General.GetNullableInteger(Request.QueryString["year"]);
        }

    }

    protected void ShowExcel()
    {
    } 

    protected void gvKPIlist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        try
        {
            RadGrid grid = (RadGrid)sender;
            grid.Columns.Clear();
            grid.MasterTableView.ColumnGroups.Clear();

            int iRowCount = 0;
            int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
            int? year = General.GetNullableInteger(ViewState["YEAR"].ToString());
            int? vesselid = General.GetNullableInteger(ViewState["FILTERID"].ToString());
            string spicode = General.GetNullableString(ViewState["SPICODE"].ToString());
            int? filterid = General.GetNullableInteger(ViewState["FILTERID"].ToString());
            int? scopeid = General.GetNullableInteger(ViewState["SCOPEID"].ToString());

            DataSet ds = PhoenixDashboardSKKPIValue.VesselKPIValueSearch(year, vesselid, spicode, gvKPIlist.CurrentPageIndex +1 , gvKPIlist.PageSize, ref iRowCount,filterid,scopeid);

            GridColumnGroup kpicolumngroup = new GridColumnGroup();
            grid.MasterTableView.ColumnGroups.Add(kpicolumngroup);
            kpicolumngroup.Name = "KPI";
            kpicolumngroup.HeaderText = "KPI";
            kpicolumngroup.HeaderStyle.Width = 400;
            kpicolumngroup.HeaderStyle.Font.Bold = true;
            kpicolumngroup.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;



            GridTemplateColumn field1 = new GridTemplateColumn();
            grid.Columns.Add(field1);

            field1.HeaderText = "Code";
            field1.UniqueName = "FLDKPICODE";
            field1.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            field1.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            field1.HeaderStyle.Width = 75;
            field1.HeaderStyle.Font.Bold = true;
            field1.ColumnGroupName = kpicolumngroup.Name;



            GridTemplateColumn field2 = new GridTemplateColumn();
            grid.Columns.Add(field2);
            field2.HeaderText = "Name";
            field2.UniqueName = "FLDKPINAME";
            field2.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            field2.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            field2.HeaderStyle.Width = 175;
            field2.HeaderStyle.Font.Bold = true;
            field2.ColumnGroupName = kpicolumngroup.Name;

            GridTemplateColumn field3 = new GridTemplateColumn();
            grid.Columns.Add(field3);
            field3.HeaderText = "Target -" +ViewState["YEAR"].ToString();
            field3.UniqueName = "FLDKPITARGETVALUE";
            field3.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            field3.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            field3.HeaderStyle.Width = 75;
            field3.HeaderStyle.Font.Bold = true;
            field3.ColumnGroupName = kpicolumngroup.Name;



            DataTable scopes = ds.Tables[2];
                DataTable quarters = ds.Tables[1];


                for (int i = 0; i < scopes.Rows.Count; i++)
                {
                    GridColumnGroup columngroup = new GridColumnGroup();
                    grid.MasterTableView.ColumnGroups.Add(columngroup);
                    columngroup.Name = scopes.Rows[i]["FLDID"].ToString();
                    columngroup.HeaderText = scopes.Rows[i]["FLDNAME"].ToString();
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
                        field.UniqueName = quarters.Rows[j]["FLDQUARTER"].ToString() + scopes.Rows[i]["FLDID"].ToString();
                        field.ColumnGroupName = scopes.Rows[i]["FLDID"].ToString();
                    }
                }
            grid.DataSource = null;
            grid.DataSource = ds;
            grid.VirtualItemCount = iRowCount;
        }       
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvKPIlist_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadGrid gv = (RadGrid)sender;
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)item.DataItem;
            item["FLDKPICODE"].Text = drv["FLDKPICODE"].ToString();
            item["FLDKPINAME"].Text = drv["FLDKPINAME"].ToString();
            item["FLDKPITARGETVALUE"].Text = drv["FLDKPITARGETVALUE"].ToString();



            DataSet ds = (DataSet)gv.DataSource;
    
            DataTable scopes = ds.Tables[2];
            DataTable quarters = ds.Tables[1];
            
            for (int j = 0; j < scopes.Rows.Count; j++)
            {
                for (int k = 0; k < quarters.Rows.Count; k++)
                {
                    DataRow[] values = ds.Tables[0].Select("FLDKPIID ='" + drv["FLDKPIID"].ToString() + "'AND FLDPERIODVALUE = '" + quarters.Rows[k]["FLDQUARTER"].ToString() + "'AND FLDFILTERID ='" + scopes.Rows[j]["FLDID"].ToString() + "'");
                   
                        string value = (values.Length == 0) ? "0" : values[0]["FLDKPIVALUE"].ToString();
                        string scopeid =  ViewState["SCOPEID"].ToString();
                        item[quarters.Rows[k]["FLDQUARTER"].ToString() + scopes.Rows[j]["FLDID"].ToString()].Text = "<a id=" + quarters.Rows[k]["FLDQUARTER"].ToString() + scopes.Rows[j]["FLDID"].ToString() + " style='color: #000' href=" + "javascript:parent.openNewWindow('Filter','','Dashboard/DashboardSKPIValueSearch.aspx?filterid=" + ViewState["FILTERID"].ToString() + "&kpicode=" + drv["FLDKPICODE"].ToString() + "&year=" + ViewState["YEAR"].ToString() + "&scopeid=" + scopeid + "')" + ">" + value + " <a/>";                    
                   
                }

            }
        }
    }
    protected void gvKPIlist_ItemCommand(object sender, GridCommandEventArgs e)
    {
    }
}