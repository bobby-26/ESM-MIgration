using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.ShippingKPI;
using Telerik.Web.UI;

public partial class Dashboard_DashboardSKSPIValueSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

            gvSPIlist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            ViewState["FILTER"] = General.GetNullableString(Request.QueryString["filter"]);
            ViewState["FILTERID"] = General.GetNullableInteger(Request.QueryString["filterid"]);
            ViewState["SPICODE"] = General.GetNullableString(Request.QueryString["spicode"]);
            ViewState["YEAR"] = General.GetNullableInteger(Request.QueryString["year"]);
        }
    }

   


    protected void gvSPIlist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid grid = (RadGrid)sender;
            grid.Columns.Clear();
            grid.MasterTableView.ColumnGroups.Clear();

            int iRowCount = 0;
            int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
            string filter = General.GetNullableString(ViewState["FILTER"].ToString());
            int? year = General.GetNullableInteger(ViewState["YEAR"].ToString());
            int? filterid = General.GetNullableInteger(ViewState["FILTERID"].ToString());
            string spicode = General.GetNullableString(ViewState["SPICODE"].ToString());

            DataSet ds = PhoenixDashboardSKSPIValue.VesselSPIValueSearch(year, filter,filterid, spicode, gvSPIlist.CurrentPageIndex + 1, gvSPIlist.PageSize, ref iRowCount);
            GridColumnGroup spicolumngroup = new GridColumnGroup();
            grid.MasterTableView.ColumnGroups.Add(spicolumngroup);
            spicolumngroup.Name = "SPI";
            spicolumngroup.HeaderText = "SPI";
            spicolumngroup.HeaderStyle.Width = 400;
            spicolumngroup.HeaderStyle.Font.Bold = true;
            spicolumngroup.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;



            GridTemplateColumn field1 = new GridTemplateColumn();
            grid.Columns.Add(field1);

            field1.HeaderText = "Code";
            field1.UniqueName = "FLDSPIID";
            field1.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            field1.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            field1.HeaderStyle.Width = 75;
            field1.HeaderStyle.Font.Bold = true;
            field1.ColumnGroupName = spicolumngroup.Name;



            GridTemplateColumn field2 = new GridTemplateColumn();
            grid.Columns.Add(field2);
            field2.HeaderText = "Name";
            field2.UniqueName = "FLDSPITITLE";
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
            grid.DataSource = ds.Tables[0];
            grid.VirtualItemCount = iRowCount;
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


        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)item.DataItem;
            item["FLDSPIID"].Text = drv["FLDSPIID"].ToString();
            item["FLDSPITITLE"].Text = drv["FLDSPITITLE"].ToString();
            

            DataSet ds = SPIdata();

            DataTable scopes = ds.Tables[2];
            DataTable quarters = ds.Tables[1];

            for (int j = 0; j < scopes.Rows.Count; j++)
            {

                for (int k = 0; k < quarters.Rows.Count; k++)
                {


                    item[quarters.Rows[k]["FLDQUARTER"].ToString() + scopes.Rows[j]["FLDID"].ToString()].Text = "<a id=" + quarters.Rows[k]["FLDQUARTER"].ToString() + scopes.Rows[j]["FLDID"].ToString() + " style='color: #000' href=" + "javascript:parent.openNewWindow('Filter','','Dashboard/DashboardSKKPIValueSearch.aspx?vesselid=" + scopes.Rows[j]["FLDID"].ToString() + "&spicode=" + drv["FLDSPIID"].ToString() + "&year=" + ViewState["YEAR"].ToString() + "')" + ">0<a/>";


                }

            }



        }


    }

    protected void gvSPIlist_ItemCommand(object sender, GridCommandEventArgs e)
    { }

    public DataSet SPIdata()
    {

        int iRowCount = 0;
        int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        string filter = General.GetNullableString(ViewState["FILTER"].ToString());
        int? year = General.GetNullableInteger(ViewState["YEAR"].ToString());
        int? filterid = General.GetNullableInteger(ViewState["FILTERID"].ToString());
        string spicode = General.GetNullableString(ViewState["SPICODE"].ToString());

        DataSet ds = PhoenixDashboardSKSPIValue.VesselSPIValueSearch(year, filter, filterid, spicode, gvSPIlist.CurrentPageIndex + 1, gvSPIlist.PageSize, ref iRowCount);

        return ds;

    }
}