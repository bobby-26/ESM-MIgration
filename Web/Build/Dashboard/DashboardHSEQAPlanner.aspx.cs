using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI.WebControls;
using System.Web.UI;

public partial class Dashboard_DashboardHSEQAPlanner : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar kpitab = new PhoenixToolbar();
        kpitab.AddButton("Planner", "Toggle1", ToolBarDirection.Left);
        
        Tabstripspiaddmenu.MenuList = kpitab.Show();
        Tabstripspiaddmenu.SelectedMenuIndex = 0;
        PhoenixToolbar tab = new PhoenixToolbar();
        tab.AddFontAwesomeButton("javascript:parent.openNewWindow('AddHSEQAPLan','','Dashboard/DashboardHSEQAPlanAdd.aspx','false','700px','420px')", "Add HSEQA PLan", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
        Tabstrip1.MenuList = tab.Show();
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            gvPlanner.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            radcbyear.SelectedYear = DateTime.Now.Year;
        }

    }

    protected void Tabstripspiaddmenu_TabStripCommand(object sender, EventArgs e)
    {

    }

    protected void gvPlanner_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid grid = (RadGrid)sender;
            grid.Columns.Clear();
            grid.MasterTableView.ColumnGroups.Clear();
            int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
            int rowcount = 0;
            int iTotalPageCount = 0;
            int? year = General.GetNullableInteger(radcbyear.SelectedYear.ToString());
            DataSet ds = PhoenixDashboardHSEQAPlanner.HSEQAPlanSearch(rowusercode, year, gvPlanner.CurrentPageIndex + 1, gvPlanner.PageSize, ref rowcount, ref iTotalPageCount);


            GridTemplateColumn field1 = new GridTemplateColumn();
            grid.Columns.Add(field1);

            field1.HeaderText = "Element";
            field1.UniqueName = "FLDTMSADESCRIPTION";
            field1.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            field1.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            field1.HeaderStyle.Width = 175;
            field1.HeaderStyle.Font.Bold = true;

            GridTemplateColumn field2 = new GridTemplateColumn();
            grid.Columns.Add(field2);
            field2.HeaderText = "Leading Indicator";
            field2.UniqueName = "FLDLINAME";
            field2.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            field2.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            field2.HeaderStyle.Width = 285;
            field2.HeaderStyle.Font.Bold = true;

            GridTemplateColumn field3 = new GridTemplateColumn();
            grid.Columns.Add(field3);
            field3.HeaderText = "Action by";
            field3.UniqueName = "FLDACTIONBY";
            field3.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            field3.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            field3.HeaderStyle.Width = 100;
            field3.HeaderStyle.Font.Bold = true;

            GridTemplateColumn field4 = new GridTemplateColumn();
            grid.Columns.Add(field4);
            field4.HeaderText = "Frequency";
            field4.UniqueName = "FLDFREQUENCY";
            field4.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            field4.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            field4.HeaderStyle.Width = 100;
            field4.HeaderStyle.Font.Bold = true;

            GridTemplateColumn field5 = new GridTemplateColumn();
            grid.Columns.Add(field5);
            field5.HeaderText = "ID";
            field5.UniqueName = "FLDLIID";
            field5.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            field5.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            field5.HeaderStyle.Width = 100;
            field5.HeaderStyle.Font.Bold = true;
            field5.Visible = false;

            DataTable months = ds.Tables[1];
            for (int i = 0; i < months.Rows.Count; i++)
            {
                GridColumnGroup columngroup = new GridColumnGroup();
                grid.MasterTableView.ColumnGroups.Add(columngroup);
                columngroup.Name = months.Rows[i]["FLDMONTH"].ToString();
                columngroup.HeaderText = months.Rows[i]["FLDMONTHNAME"].ToString();
                columngroup.HeaderStyle.Width = 100;
                columngroup.HeaderStyle.Font.Bold = true;
                columngroup.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;

                for (int j = 0; j < Int32.Parse(months.Rows[i]["NUMBEROFWEEKS"].ToString()); j++)
                {
                    GridTemplateColumn field = new GridTemplateColumn();
                    grid.Columns.Add(field);
                    field.HeaderText = (j + 1).ToString();
                    field.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                    field.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    field.HeaderStyle.Width = 25;
                    field.HeaderStyle.Font.Bold = true;
                    field.UniqueName = months.Rows[i]["FLDMONTH"].ToString() + (j +1).ToString();
                    field.ColumnGroupName = months.Rows[i]["FLDMONTH"].ToString();
                }
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

    protected void gvPlanner_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadGrid gv = (RadGrid)sender;
        DataSet ds = data();
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)item.DataItem;
            item["FLDTMSADESCRIPTION"].Text = drv["FLDTMSADESCRIPTION"].ToString();
            item["FLDLINAME"].Text = drv["FLDLINAME"].ToString();
            item["FLDACTIONBY"].Text = drv["FLDACTIONBY"].ToString();
            item["FLDFREQUENCY"].Text = drv["FLDFREQUENCY"].ToString() + "  " + drv["FLDFREQUENCYTYPE"].ToString();
            item["FLDLIID"].Text = drv["FLDLIID"].ToString() ;

            DataTable months = ds.Tables[1];
            DataTable d1 = ds.Tables[2];
            DataTable d2 = ds.Tables[3];
            for (int i = 0; i < months.Rows.Count; i++)
            {
                for (int j = 0; j < Int32.Parse(months.Rows[i]["NUMBEROFWEEKS"].ToString()); j++)
                {
                    
                    DataRow[] dr1 = ds.Tables[2].Select("X='"+ months.Rows[i]["FLDMONTH"].ToString() + (j+1).ToString()+"'"+ "AND\t" +"FLDLIID='" + item["FLDLIID"].Text+"'");
                    DataRow[] dr2 = d2.Select("X='" + months.Rows[i]["FLDMONTH"].ToString() + (j + 1).ToString() + "'" + "AND\t" + "FLDLIID='" + item["FLDLIID"].Text + "'");
                    string html = string.Empty;
                    html = "<table width='100%'>";
                    if (dr1.Length > 0)
                    {
                        html = html + "<tr><td bgcolor='#f8cbad'><a style='color: #000' href=" + "javascript:parent.openNewWindow('Filters','','Dashboard/DashboardHSEQAPlannerDetails.aspx?liid=" + item["FLDLIID"].Text + "&columnid=" + months.Rows[i]["FLDMONTH"].ToString() + (j + 1).ToString() + "')" + ">" + "&nbsp" + dr1[0]["NUMBER"].ToString() + "</a></td></tr> "; 
                    }
                    if (dr2.Length > 0)
                    {
                         html = html + "<tr><td bgcolor='#70ad47'><a style='color: #000' href=" + "javascript:parent.openNewWindow('Filter','','Dashboard/DashboardHSEQAPlannerReportedDetails.aspx?liid=" + item["FLDLIID"].Text + "&columnid=" + months.Rows[i]["FLDMONTH"].ToString() + (j + 1).ToString() + "')" + ">" + "&nbsp" + dr2[0]["REPORTEDNUMBER"].ToString() + "</a></td></tr>";
                    }
                    html = html + "</table>";
                    item[months.Rows[i]["FLDMONTH"].ToString() + (j + 1).ToString()].Text = html;
                }


            }


        }
    }

    protected void gvPlanner_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

   
    protected void Tabstrip1_TabStripCommand(object sender, EventArgs e)
    {

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            gvPlanner.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public DataSet data()
    {
        int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        int rowcount = 0;
        int iTotalPageCount = 0;
        int? year = General.GetNullableInteger(radcbyear.SelectedYear.ToString());
        DataSet ds = PhoenixDashboardHSEQAPlanner.HSEQAPlanSearch(rowusercode, year, gvPlanner.CurrentPageIndex + 1, gvPlanner.PageSize, ref rowcount, ref iTotalPageCount);

        return ds;
    }
}