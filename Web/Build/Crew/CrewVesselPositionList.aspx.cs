using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewVesselPositionList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"] != "")
                    ViewState["VESSELID"] = Request.QueryString["vesselid"];
                else
                    ViewState["VESSELID"] = "";
                gvVesselPosition.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
            MenuVesselPosition.AccessRights = this.ViewState;
            MenuVesselPosition.MenuList = toolbarmain.Show();
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewVesselPositionList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvVesselPosition')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuVessel.AccessRights = this.ViewState;
            MenuVessel.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuVesselPosition_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("BACK"))
            {
                if (Request.QueryString["launched"] == "CrewList")
                    Response.Redirect("../Crew/CrewList.aspx?vesselid=" + ViewState["VESSELID"].ToString(), false);
                else if (Request.QueryString["launched"] == "CrewChangeFilter")
                    Response.Redirect("../Crew/CrewChangePlanFilter.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "&launched=" + "CrewVesselPositionList", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuVessel_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {

                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDVESSELNAME", "FLDSEQUENCE", "FLDSEAPORTNAME", "FLDETA", "FLDETD", "FLDVOYAGENUMBER", "FLDPORTCALLID" };

                string[] alCaptions = { "Vessel", "Sequence", "Port", "ETA", "ETD", "Voyage Number", "Port Call ID" };


                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int? sortdirection = null;

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                DataTable dt = PhoenixCrewVesselPosition.SearchVesselPositionCrewListPlan(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                                                , sortexpression, sortdirection
                                                                                , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                                                                                , ref iRowCount, ref iTotalPageCount);

                if (dt.Rows.Count > 0)
                    General.ShowExcel("Vessel Position", dt, alColumns, alCaptions, null, null);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDSEQUENCE", "FLDSEAPORTNAME", "FLDETA", "FLDETD", "FLDVOYAGENUMBER", "FLDPORTCALLID" };

        string[] alCaptions = { "Vessel", "Sequence", "Port", "ETA", "ETD", "Voyage Number", "Port Call ID" };


        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {

            DataTable dt = PhoenixCrewVesselPosition.SearchVesselPositionCrewListPlan(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                                                        , sortexpression, sortdirection
                                                                                        , (int)ViewState["PAGENUMBER"], gvVesselPosition.PageSize
                                                                                        , ref iRowCount, ref iTotalPageCount);


            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvVesselPosition", "Vessel Position", alCaptions, alColumns, ds);

            gvVesselPosition.DataSource = dt;
            gvVesselPosition.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvVesselPosition_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVesselPosition.CurrentPageIndex + 1;

        BindData();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();

    }

}