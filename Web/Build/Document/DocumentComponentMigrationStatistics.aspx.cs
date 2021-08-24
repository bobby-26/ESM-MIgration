using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Data;
using SouthNests.Phoenix.Document;
public partial class DocumentComponentMigrationStatistics : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Document/DocumentComponentMigrationStatistics.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        MenuMigrationStatistics.AccessRights = this.ViewState;
        MenuMigrationStatistics.MenuList = toolbar.Show();

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarMenu = new PhoenixToolbar();
        toolbarMenu.AddFontAwesomeButton("../Document/DocumentComponentMigrationStatistics.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbarMenu.AddImageButton("../Document/DocumentComponentMigrationStatistics.aspx", "Bulk Export to Vessel", "data-export.png", "BULKINSERT");
        toolbarMenu.AddImageButton("../Document/DocumentComponentMigrationStatistics.aspx", "Bulk Export to Vessel", "data-export.png", "BULKUPDATE");
        MenuStatistics.AccessRights = this.ViewState;
        MenuStatistics.MenuList = toolbarMenu.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvMigrationStatistics.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    private void BindData()
    {
        try
        {
            DataSet ds = PhoenixDocumentComponentMigrationStatistics.MigrationStatisticsCountSearch
                (General.GetNullableInteger(ucVessel.SelectedVessel.ToString()));

            gvMigrationStatisticsCount.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void MigrationStatisticsBind()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDHARDNAME" };
            string[] alCaptions = { "Component Number", "Component Name", "Status" };

            DataSet ds = PhoenixDocumentComponentMigrationStatistics.MigrationStatisticsSearch
            (
                General.GetNullableInteger(ucVessel.SelectedVessel.ToString())
                , sortexpression
                , sortdirection
                , int.Parse(ViewState["PAGENUMBER"].ToString())
                , gvMigrationStatistics.PageSize
                , ref iRowCount
                , ref iTotalPageCount
            );

            General.SetPrintOptions("gvMigrationStatistics", "Migration Statistics", alCaptions, alColumns, ds);

            gvMigrationStatistics.DataSource = ds;
            gvMigrationStatistics.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvMigrationStatistics.EditIndexes.Clear();
        gvMigrationStatistics.SelectedIndexes.Clear();
        gvMigrationStatistics.DataSource = null;
        gvMigrationStatistics.Rebind();
    }
    protected void gvMigrationStatistics_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMigrationStatistics.CurrentPageIndex + 1;
            MigrationStatisticsBind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMigrationStatisticsCount_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuMigrationStatistics_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvMigrationStatistics.CurrentPageIndex = 0;
                gvMigrationStatisticsCount.Rebind();
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMigrationStatistics_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        else if (e.CommandName == "COMPONENTINSERT")
        {
            PhoenixDocumentComponentMigrationStatistics.MigrationStatisticsInsert
                (
                int.Parse(ucVessel.SelectedVessel.ToString())
                ,General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblComponentId")).Text.ToString())
                );
            ucStatus.Text = "Component Exported";
        }
        else if (e.CommandName == "COMPONENTUPDATE")
        {
            PhoenixDocumentComponentMigrationStatistics.MigrationStatisticsUpdate
                (
                int.Parse(ucVessel.SelectedVessel.ToString())
                , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblComponentId")).Text.ToString())
                );
            ucStatus.Text = "Component Exported";
        }
    }

    protected void gvMigrationStatistics_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void MenuStatistics_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("BULKINSERT"))
            {
                ComponentInsert();
            }
            else if (CommandName.ToUpper().Equals("BULKUPDATE"))
            {
                ComponentUpdate();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void ComponentInsert()
    {
        PhoenixDocumentComponentMigrationStatistics.MigrationStatisticsInsert
                (
                int.Parse(ucVessel.SelectedVessel.ToString())
                , null
                );
        ucStatus.Text = "Component Exported";
    }
    protected void ComponentUpdate()
    {
        PhoenixDocumentComponentMigrationStatistics.MigrationStatisticsUpdate
                (
                int.Parse(ucVessel.SelectedVessel.ToString())
                , null
                );
        ucStatus.Text = "Component Exported";
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDHARDNAME" };
        string[] alCaptions = { "Component Number", "Component Name", "Status" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixDocumentComponentMigrationStatistics.MigrationStatisticsSearch
            (
                General.GetNullableInteger(ucVessel.SelectedVessel.ToString())
                , sortexpression
                , sortdirection
                , 1
                , iRowCount
                , ref iRowCount
                , ref iTotalPageCount
            );

        Response.AddHeader("Content-Disposition", "attachment; filename=Migration Statistics.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Migration Statistics</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
}