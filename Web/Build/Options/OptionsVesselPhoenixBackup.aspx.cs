using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.DataTransfer;
using Telerik.Web.UI;

public partial class OptionsVesselPhoenixBackup : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvPhoenixBackup.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Options/OptionsVesselPhoenixBackup.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvVesselImportList')", "Print Grid", "icon_print.png", "Print");
            toolbargrid.AddImageButton("../Options/OptionsVesselPhoenixBackup.aspx", "Find", "search.png", "FIND");
            toolbargrid.AddImageButton("../Options/OptionsVesselPhoenixBackup.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            MenuPhoenixbackup.AccessRights = this.ViewState;
            MenuPhoenixbackup.MenuList = toolbargrid.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    
    protected void MenuPhoenixbackup_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["VesselName"] = txtvesselname.Text;
                Rebind();

            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {

                txtvesselname.Text = "";
                ViewState["VesselName"] = txtvesselname.Text;
                Rebind();

            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELID", "FLDVESSELNAME", "FLDBACKUPDATE" };
        string[] alCaptions = { "Vessel Id", "Vessel Name", "Last Backup Date" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixDataTransferImport.PhoenixVesselBackup(General.GetNullableString(txtvesselname.Text)
                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                    , gvPhoenixBackup.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount);
        
        Response.AddHeader("Content-Disposition", "attachment; filename=VesselImportList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Phoenix Backup</h3></td>");
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
        foreach (DataRow dr in dt.Rows)
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

    protected void Rebind()
    {
        gvPhoenixBackup.SelectedIndexes.Clear();
        gvPhoenixBackup.EditIndexes.Clear();
        gvPhoenixBackup.DataSource = null;
        gvPhoenixBackup.Rebind();
    }

    private void BindData()
    {

        string[] alColumns = { "FLDVESSELID", "FLDVESSELNAME", "FLDBACKUPDATE" };
        string[] alCaptions = { "Vessel Id", "Vessel Name", "Last Backup Date" };

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixDataTransferImport.PhoenixVesselBackup(General.GetNullableString(txtvesselname.Text)
            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
            , gvPhoenixBackup.PageSize
            , ref iRowCount
            , ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvVesselImportList", " Vessel Import ", alCaptions, alColumns, ds);

        gvPhoenixBackup.DataSource = dt;
        gvPhoenixBackup.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvPhoenixBackup_ItemDataBound(Object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DateTime? lastbackupdate = null;
            DateTime? lastbackup = null;

            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lb = (RadLabel)e.Item.FindControl("lblbackupdate");
            Image imgbackup = e.Item.FindControl("imgbackupdelay") as Image;
            imgbackup.Visible = false;

            string lastdate = drv["FLDBACKUPDATE"].ToString();

            if (!string.IsNullOrEmpty(lastdate))
            {
                lastbackupdate = Convert.ToDateTime(lastdate);
                lastbackup = DateTime.Now.AddDays(-5);
            }

            if (lastbackupdate < lastbackup)
            {
                imgbackup.Visible = true;
                imgbackup.ImageUrl = Session["images"] + "/" + (drv["FLDBACKUPDATE"].ToString() == "1" ? "deficiency-action.png" : "highPriority.png");
            }
        }
    }

    protected void gvPhoenixBackup_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;
            if (e.CommandName.ToUpper().Equals("MAIL"))
            {
                RadLabel vesselid = ((RadLabel)e.Item.FindControl("lblvesselid"));
                PhoenixDataTransferImport.PhoenixVesselBackupMail(int.Parse(vesselid.Text));
                ucStatus.Text = "Mail Sent";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvPhoenixBackup_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPhoenixBackup.CurrentPageIndex + 1;
        BindData();
    }    
}
