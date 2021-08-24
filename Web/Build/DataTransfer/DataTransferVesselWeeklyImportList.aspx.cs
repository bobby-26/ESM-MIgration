using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.DataTransfer;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class DataTransferVesselWeeklyImportList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../DataTransfer/DataTransferVesselWeeklyImportList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvVesselWeeklyImportList')", "Print Grid", "icon_print.png", "Print");
            toolbargrid.AddImageButton("../DataTransfer/DataTransferVesselWeeklyImportList.aspx", "Find", "search.png", "FIND");
            toolbargrid.AddImageButton("../DataTransfer/DataTransferVesselWeeklyImportList.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            MenuVesselweeklyImport.AccessRights = this.ViewState;
            MenuVesselweeklyImport.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvVesselWeeklyImportList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvVesselWeeklyImportList.SelectedIndexes.Clear();
        gvVesselWeeklyImportList.EditIndexes.Clear();
        gvVesselWeeklyImportList.DataSource = null;
        gvVesselWeeklyImportList.Rebind();
    }
    private void BindData()
    {

        string[] alColumns = { "FLDVESSELNAME", "FLDDTENTITYSERIAL", "FLDDTIMPORTDATE", "FLDATENTITYSERIAL", "FLDATIMPORTDATE" };
        string[] alCaptions = { "Vessel Name", " Last DT Import Sequence", "Last DT Import Date", " Last AT Import Sequence", "Last AT Import Date" };


        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixDataTransferImport.DataTransferVesselWeeklyImportList(txtvesselName.Text
            , General.GetNullableInteger(ucTechFleet.SelectedFleet)
            , sortexpression
            , sortdirection
            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
            , gvVesselWeeklyImportList.PageSize
            , ref iRowCount
            , ref iTotalPageCount);


        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvVesselWeeklyImportList", " Vessel DMS Weekly Import ", alCaptions, alColumns, ds);

        gvVesselWeeklyImportList.DataSource = dt;
        gvVesselWeeklyImportList.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    protected void MenuVesselWeeklyImport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["VesselName"] = txtvesselName.Text;
                Rebind();

            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {

                txtvesselName.Text = "";
                ucTechFleet.SelectedFleet = "";
                ViewState["VesselName"] = txtvesselName.Text;
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

        string[] alColumns = { "FLDVESSELNAME", "FLDDTENTITYSERIAL", "FLDDTIMPORTDATE", "FLDATENTITYSERIAL", "FLDATIMPORTDATE" };
        string[] alCaptions = { "Vessel Name", " Last DT Import Sequence", "Last DT Import Date", " Last AT Import Sequence", "Last AT Import Date" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        DataTable dt = PhoenixDataTransferImport.DataTransferVesselWeeklyImportList(txtvesselName.Text
          , General.GetNullableInteger(ucTechFleet.SelectedFleet)
          , sortexpression
          , sortdirection
          , 1
          , iRowCount
          , ref iRowCount
          , ref iTotalPageCount);

        General.ShowExcel(" Vessel DMS Weekly Import ", dt, alColumns, alCaptions, sortdirection, sortexpression);

    }

    protected void gvVesselWeeklyImportList_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvVesselWeeklyImportList_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }

    protected void gvVesselWeeklyImportList_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVesselWeeklyImportList.CurrentPageIndex + 1;
        BindData();
    }
}




