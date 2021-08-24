using System;
using System.Data;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class Crew_CrewPlanList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewPlanList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvCCPlan')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewPlanList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewPlanList.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");            
            MenuCrewChangePlan.AccessRights = this.ViewState;
            MenuCrewChangePlan.MenuList = toolbargrid.Show();
            
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCCPlan.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
  
    protected void ChangePlan_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDROWNUMBER", "FLDVESSELNAME", "FLDOFFSIGNERCOUNT", "FLDONSIGNERCOUNT" };
                string[] alCaptions = { "S.No.", "Vessel",  "Off Signer", "On Signer" };

                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataSet ds = PhoenixCrewChangePlan.CrewChangePlanSearch(General.GetNullableInteger(ddlVessel.SelectedVessel), General.GetNullableString(txtEmployee.Text.Trim())
                                                                          , General.GetNullableDateTime(txtPlannedFrom.Text)
                                                                          , General.GetNullableDateTime(txtPlannedTo.Text)
                                                                          , sortexpression, sortdirection
                                                                          , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                          , gvCCPlan.PageSize
                                                                          , ref iRowCount
                                                                          , ref iTotalPageCount);

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Crew Change Travel", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                BindData();
                gvCCPlan.Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlVessel.SelectedVessel = "";
                txtEmployee.Text = "";
                txtPlannedFrom.Text = "";
                txtPlannedTo.Text = "";
                ViewState["PAGENUMBER"] = 1;
                gvCCPlan.CurrentPageIndex = 0;
                BindData();
                gvCCPlan.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDROWNUMBER", "FLDVESSELNAME", "FLDOFFSIGNERCOUNT", "FLDONSIGNERCOUNT" };
            string[] alCaptions = { "S.No.", "Vessel", "Off Signer", "On Signer" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewChangePlan.CrewChangePlanSearch(General.GetNullableInteger(ddlVessel.SelectedVessel), General.GetNullableString(txtEmployee.Text.Trim()),
                                                                      General.GetNullableDateTime(txtPlannedFrom.Text),
                                                                      General.GetNullableDateTime(txtPlannedTo.Text),
                                                                      sortexpression, sortdirection,
                                                                      int.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                      gvCCPlan.PageSize,
                                                                      ref iRowCount,
                                                                      ref iTotalPageCount);

            General.SetPrintOptions("gvCCPlan", "Crew Change Travel", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCCPlan.DataSource = ds;
                gvCCPlan.VirtualItemCount = iRowCount;
            }
            else
            {
                gvCCPlan.DataSource = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
 
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvCCPlan.Rebind();
    }
    protected void gvCCPlan_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {            
            if (e.CommandName.ToUpper() == "TRAVELEDIT")
            {

                string lblVesselId = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;

                if (lblVesselId != "" && lblVesselId != null)
                {
                    Response.Redirect("../Crew/CrewPlanTravel.aspx?Vesselid=" + lblVesselId, false);
                }
            }
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

    protected void gvCCPlan_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCCPlan.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

