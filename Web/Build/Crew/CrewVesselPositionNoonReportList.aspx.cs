using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewVesselPositionNoonReportList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style","display:none;");

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Arrival", "ARRIVAL");
            toolbarmain.AddButton("Noon Report", "NOONREPORT");
            
            

            MenuVesselPosition.AccessRights = this.ViewState;
            MenuVesselPosition.MenuList = toolbarmain.Show();



            PhoenixToolbar toolbar = new PhoenixToolbar();



            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["VESSELID"] = Request.QueryString["vesselid"];
                if (Request.QueryString["vesselid"] == null || Request.QueryString["vesselid"] == string.Empty)
                    Response.Redirect("../Crew/CrewVesselPositionArrivalList.aspx?vesselid=" + ViewState["VESSELID"].ToString(), false);

                gvVesselPositionNoonReport.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
      
            }

            toolbar.AddFontAwesomeButton("javascript:parent.openNewWindow('code1','','"+Session["sitepath"]+"/Crew/CrewVesselPositionNoonReport.aspx?VESSELID=" + ViewState["VESSELID"] + "&OPERATIONMODE=ADD')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

            MenuCrewNoonReport.AccessRights = this.ViewState;
            MenuCrewNoonReport.MenuList = toolbar.Show();


          
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
            if (ViewState["VESSELID"] == null)
                return;

            if (CommandName.ToUpper().Equals("ARRIVAL"))
            {
                ViewState["TABNAME"] = "ARRIVAL";
                Response.Redirect("../Crew/CrewVesselPositionArrivalList.aspx?vesselid=" + ViewState["VESSELID"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("NOONREPORT"))
            {
                ViewState["TABNAME"] = "NOONREPORT";
                Response.Redirect("../Crew/CrewVesselPositionNoonReportList.aspx?vesselid=" + ViewState["VESSELID"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewNoonReport_TabStripCommand(object sender, EventArgs e)
    {

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvVesselPositionNoonReport.Rebind();
    }
    protected void BindData(object sender, EventArgs e)
    {
        BindData();
        ViewState["VESSELID"] = Request.QueryString["vesselid"];
    }

    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = 0;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        try
        {

            DataTable dt = PhoenixCrewVesselPosition.SearchVesselNoonReport(int.Parse(ViewState["VESSELID"].ToString()), sortexpression, sortdirection
                                                                        , (int)ViewState["PAGENUMBER"], gvVesselPositionNoonReport.PageSize
                                                                        , ref iRowCount, ref iTotalPageCount);

            gvVesselPositionNoonReport.DataSource = dt;
            gvVesselPositionNoonReport.VirtualItemCount = iRowCount;

           
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   


    protected void gvVesselPositionNoonReport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVesselPositionNoonReport.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVesselPositionNoonReport_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton lbtn = (LinkButton)e.Item.FindControl("lnkVesselName");
            RadLabel lbl = (RadLabel)e.Item.FindControl("lblNoonReportId");
            if (lbtn != null) lbtn.Attributes.Add("onclick", "javascript:parent.openNewWindow('code1','','"+Session["sitepath"]+"/Crew/CrewVesselPositionNoonReport.aspx?noonreportid=" + lbl.Text + "'); return false;");

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Attributes.Add("onclick", "javascript:parent.openNewWindow('code1','','" + Session["sitepath"] + "/Crew/CrewVesselPositionNoonReport.aspx?noonreportid=" + lbl.Text + "'); return false;");
        }
    }

    protected void gvVesselPositionNoonReport_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

    }
}
