using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Crew_CrewListSimple : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {            
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewListSimple.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            CrewQueryMenu.AccessRights = this.ViewState;
            CrewQueryMenu.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                string vessel = Request.QueryString["vslid"];
                ViewState["VESSELID"] = !string.IsNullOrEmpty(vessel) ? vessel : "";
                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void CrewQueryMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDROWNUMBER", "FLDEMPLOYEECODE", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDNATIONALITYNAME", "FLDPASSPORTNO", "FLDSEAMANBOOKNO", "FLDSIGNONDATE", "FLDRELIEFDUEDATE" };
                string[] alCaptions = { "S.No.", "File No.", "Name", "Rank", "Nationality", "Passport", "CDC No.", "Sign on", "Relief Due" };

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int sortdirection;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                else
                    sortdirection = 0;
                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataSet ds = PhoenixCrewManagement.SearchCrewOnboard(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                             , null
                                                             , null
                                                             , 1
                                                             , 0
                                                             , 1
                                                             , sortexpression, sortdirection
                                                             , 1, iRowCount
                                                             , ref iRowCount, ref iTotalPageCount
                                                             , null
                                                             , null
                                                             , null
                                                             , null);
                General.ShowExcel("Crew List", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    public void BindData()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDROWNUMBER", "FLDEMPLOYEECODE", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDNATIONALITYNAME", "FLDPASSPORTNO", "FLDSEAMANBOOKNO", "FLDSIGNONDATE", "FLDRELIEFDUEDATE" };
        string[] alCaptions = { "S.No.", "File No.", "Name", "Rank", "Nationality", "Passport", "CDC No.", "Sign on", "Relief Due" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {

            DataSet ds = PhoenixCrewManagement.SearchCrewOnboard(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                            , null
                                                            , null
                                                            , 1
                                                            , 0
                                                            , 1
                                                            , sortexpression, sortdirection
                                                            , gvCrewSearch.CurrentPageIndex + 1, gvCrewSearch.PageSize
                                                            , ref iRowCount, ref iTotalPageCount
                                                            , null
                                                            , null
                                                            , null
                                                            , null);

            
            General.SetPrintOptions("gvCrewSearch", "Crew List", alCaptions, alColumns, ds);
            gvCrewSearch.DataSource = ds;
            gvCrewSearch.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvCrewSearch_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;
            RadGrid _gridView = (RadGrid)sender;
            if (e.CommandName.ToUpper() == "EDIT")
            {
               
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {
        
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            GridDataItem item = (GridDataItem)e.Item;
            LinkButton crw = (LinkButton)item.FindControl("lnkEployeeName");
            if(crw != null)
                if(PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "OFFSHORE")
                    crw.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&launchedfrom=offshore'); return false;");

                else
                    crw.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&familyid=" + drv["FLDFAMILYID"].ToString() + "'); return false;");
        }
    }
  
}
