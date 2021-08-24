using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.ShippingKPI;
using System.Globalization;
using Telerik.Web.UI;

public partial class Dashboard_DashboardBSCMap : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ViewState["STRATEGYID"] = General.GetNullableGuid(Request.QueryString["strategyid"]);

            Guid? BSSMID = General.GetNullableGuid(ViewState["STRATEGYID"].ToString());

            DataTable dt = PhoenixDashboardBSSP.BSSMEditList(BSSMID);
            if (dt.Rows.Count > 0)
            {
                radlblstrategyname.Text = dt.Rows[0]["FLDBSSMNAME"].ToString();
                radlblstrategyvision.Text = "<b>Vision :</b> " + dt.Rows[0]["FLDVISION"].ToString();

            }
        }
    }

    protected void gvBSCMap_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid grid = (RadGrid)sender;
            grid.Columns.Clear();
            grid.MasterTableView.ColumnGroups.Clear();
            int rowcount = 0;


            DataSet ds = data( ref rowcount);

            GridTemplateColumn field1 = new GridTemplateColumn();
            grid.Columns.Add(field1);

            field1.HeaderText = "Perspectives";
            field1.UniqueName = "FLDBSDESCRIPTION";
            field1.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            field1.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            field1.HeaderStyle.Width = 75;
            field1.HeaderStyle.Font.Bold = true;

            DataTable themes = ds.Tables[1];

            for (int j = 0; j < themes.Rows.Count; j++)
            {
                GridTemplateColumn field = new GridTemplateColumn();
                grid.Columns.Add(field);
                field.HeaderText = themes.Rows[j]["FLDSPITITLE"].ToString();
                field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                field.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                field.HeaderStyle.Width = 75;
                field.HeaderStyle.Font.Bold = true;
                field.UniqueName = themes.Rows[j]["FLDSHIPPINGSPIID"].ToString();
                field.HeaderStyle.Wrap = true;

            }
            grid.DataSource = null;
            grid.DataSource = ds.Tables[0];
            grid.VirtualItemCount = rowcount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }

    protected void gvBSCMap_ItemDataBound(object sender, GridItemEventArgs e)
    {
        //RadGrid gv = (RadGrid)sender;

        //if (e.Item is GridDataItem)
        //{
        //    GridDataItem item = (GridDataItem)e.Item;
        //    DataRowView drv = (DataRowView)item.DataItem;
        //    item["FLDBSDESCRIPTION"].Text = drv["FLDBSDESCRIPTION"].ToString();
        //    int rowcount = 0;
        //    DataSet ds = data( ref rowcount);
        //    DataTable themes = ds.Tables[1];
        //    Guid? BSSMID = General.GetNullableGuid(ViewState["STRATEGYID"].ToString());
        //    Guid? sspid = General.GetNullableGuid(item.OwnerTableView.DataKeyValues[item.ItemIndex]["FLDBSSTRATEGICPERSPECTIVEID"].ToString());

        //    for (int i = 0; i < themes.Rows.Count; i++)
        //    {
        //        Guid? spiid = General.GetNullableGuid(themes.Rows[i]["FLDSHIPPINGSPIID"].ToString());

        //        DataTable dt = PhoenixDashboardBSSP.BSSMISearch(BSSMID, sspid, spiid);
        //        if (dt.Rows.Count > 0)
        //        {
        //            string html = string.Empty;
        //            for (int j = 0; j < dt.Rows.Count; j++)
        //            {
        //                html = html + "<div>"+ dt.Rows[j]["KPI"].ToString() + "</div>";

        //            }

        //            item[themes.Rows[i]["FLDSHIPPINGSPIID"].ToString()].Text = html;

                    

        //        }
        //    }


        //}

    }

    protected void gvBSCMap_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void ContextMenu1_ItemClick(object sender, RadMenuEventArgs e)
    {
        RadContextMenu rmenu = (RadContextMenu)sender;
        RadGrid rg = (RadGrid)rmenu.Parent.FindControl("gvBSCMap");
        if (!IsValidSelection(rg))
        {
            ucError.Visible = true;
            return;
        }
        Guid? spiid = General.GetNullableGuid(rg.SelectedCells[0].Column.UniqueName);
        int rowindex = rg.SelectedCells[0].Item.RowIndex;

        GridDataItem item = (GridDataItem)(rg.SelectedCells[0]).Item;
        Guid? sspid = General.GetNullableGuid(item.OwnerTableView.DataKeyValues[item.ItemIndex]["FLDBSSTRATEGICPERSPECTIVEID"].ToString());
        Guid? strategyid = General.GetNullableGuid(ViewState["STRATEGYID"].ToString());
        Response.Redirect("javascript:parent.openNewWindow('filter', 'Strategy Elements', 'Dashboard/DashboardBSCElementsAdd.aspx?spiid=" + spiid + "&sspid=" + sspid + "&strategyid=" + strategyid + "', 'false', '450px', '380px'); ");

    }

    public DataSet data( ref int  rowcount )
    {
        
        Guid? BSSMID = General.GetNullableGuid(ViewState["STRATEGYID"].ToString());
        DataSet ds = null;
        return ds;
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            gvBSCMap.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidSelection(RadGrid rg)
    {
        ucError.HeaderMessage = "Provide the following required information";

        //if (kpi == null)
        //{
        //    ucError.ErrorMessage = "KPI .";
        //}

        if (rg.SelectedCells.Count == 0)
        {

            ucError.ErrorMessage = "Select a Cell.";
        }
        else
        {
            if ((rg.SelectedCells[0].Column.UniqueName == "FLDBSDESCRIPTION") )
            {
                ucError.ErrorMessage = "Select a Valid Cell.";
            }

           
        }



        return (!ucError.IsError);
    }
}