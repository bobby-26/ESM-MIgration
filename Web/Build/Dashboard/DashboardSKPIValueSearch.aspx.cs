using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.ShippingKPI;
using System.Globalization;
using Telerik.Web.UI;


public partial class Dashboard_DashboardSKPIValueSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

            gvPIlist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            ViewState["FILTERID"] = General.GetNullableString(Request.QueryString["filterid"]);
            ViewState["SCOPEID"] = General.GetNullableString(Request.QueryString["scopeid"]);
            ViewState["KPICODE"] = General.GetNullableString(Request.QueryString["kpicode"]);
            ViewState["YEAR"] = General.GetNullableInteger(Request.QueryString["year"]);
        }
    }

    protected void ShowExcel()
    { }





    protected void gvPIlist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
            string kpicode = General.GetNullableString(ViewState["KPICODE"].ToString());
            int? filterid = General.GetNullableInteger(ViewState["FILTERID"].ToString());
            int? scopeid = General.GetNullableInteger(ViewState["SCOPEID"].ToString());

            DataSet ds = PhoenixDashboardSKPIValue.VesselPIValueSearch(year, vesselid, kpicode, gvPIlist.CurrentPageIndex + 1, gvPIlist.PageSize, ref iRowCount,filterid,scopeid);

            GridColumnGroup spicolumngroup = new GridColumnGroup();
            grid.MasterTableView.ColumnGroups.Add(spicolumngroup);
            spicolumngroup.Name = "PI";
            spicolumngroup.HeaderText = "PI";
            spicolumngroup.HeaderStyle.Width = 400;
            spicolumngroup.HeaderStyle.Font.Bold = true;
            spicolumngroup.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;



            GridTemplateColumn field1 = new GridTemplateColumn();
            grid.Columns.Add(field1);

            field1.HeaderText = "Code";
            field1.UniqueName = "FLDPICODE";
            field1.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            field1.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            field1.HeaderStyle.Width = 75;
            field1.HeaderStyle.Font.Bold = true;
            field1.ColumnGroupName = spicolumngroup.Name;



            GridTemplateColumn field2 = new GridTemplateColumn();
            grid.Columns.Add(field2);
            field2.HeaderText = "Name";
            field2.UniqueName = "FLDPINAME";
            field2.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            field2.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            field2.HeaderStyle.Width = 175;
            field2.HeaderStyle.Font.Bold = true;
            field2.ColumnGroupName = spicolumngroup.Name;




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

    protected void gvPIlist_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try { 
        RadGrid gv = (RadGrid)sender;


        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)item.DataItem;
            item["FLDPICODE"].Text = drv["FLDPICODE"].ToString();
            item["FLDPINAME"].Text = drv["FLDPINAME"].ToString();


            DataSet ds = (DataSet)gv.DataSource;

            DataTable scopes = ds.Tables[2];
            DataTable quarters = ds.Tables[1];

            for (int j = 0; j < scopes.Rows.Count; j++)
            {
                for (int k = 0; k < quarters.Rows.Count; k++)
                {
                        DataRow[] values = ds.Tables[3].Select("FLDPIID ='" + drv["FLDPIID"].ToString() + "'AND FLDPERIODVALUE = '" + quarters.Rows[k]["FLDQUARTER"].ToString() + "'AND FLDFILTERID ='" + scopes.Rows[j]["FLDID"].ToString() + "'");
                        string value = (values.Length == 0) ? "0.0" : values[0]["FLDPIVALUE"].ToString();
                        item[quarters.Rows[k]["FLDQUARTER"].ToString() + scopes.Rows[j]["FLDID"].ToString()].Text = value;
                }
            }

        }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }
    protected void gvPIlist_ItemCommand(object sender, GridCommandEventArgs e)
    { }


   
}