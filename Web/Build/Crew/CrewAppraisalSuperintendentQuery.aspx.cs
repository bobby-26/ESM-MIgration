using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using System.Web.UI;
using Telerik.Web.UI;
public partial class CrewAppraisalSuperintendentQuery : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewAppraisalSuperintendentQuery.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Crew/CrewAppraisalSuperintendentQuery.aspx", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbar.AddFontAwesomeButton("../Crew/CrewAppraisalSuperintendentQuery.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            CrewQueryMenu.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                if (Request.QueryString["vslid"] != null)
                    ddlVessel.SelectedVessel = Request.QueryString["vslid"];
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
                string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDRANKCODE", "FLDSIGNONDATE", "FLDRELIEFDUEDATE" };
                string[] alCaptions = { "File No.", "Name", "Sign on Rank", "Sign on Date", "Relief Due Date" };

                DataTable dt = PhoenixCrewAppraisal.ListSuperintendentAppraisal(General.GetNullableInteger(ddlVessel.SelectedVessel));
                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());
                if (ds.Tables.Count > 0)
                    General.ShowExcel("Crew Appraisal", ds.Tables[0], alColumns, alCaptions, null, null);
            }
            if (CommandName.ToUpper().Equals("SEARCH"))
                Rebind();
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlVessel.SelectedVessel = "";
                Rebind();
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
        gvCrewSearch.SelectedIndexes.Clear();
        gvCrewSearch.EditIndexes.Clear();
        gvCrewSearch.DataSource = null;
        gvCrewSearch.Rebind();
    }
    public void BindData()
    {
        string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDRANKCODE", "FLDSIGNONDATE", "FLDRELIEFDUEDATE" };
        string[] alCaptions = { "File No.", "Name", "Sign on Rank", "Sign on Date", "Relief Due Date" };
        try
        {
            DataTable dt = PhoenixCrewAppraisal.ListSuperintendentAppraisal(General.GetNullableInteger(ddlVessel.SelectedVessel));
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvCrewSearch", "Crew Appraisal", alCaptions, alColumns, ds);
            gvCrewSearch.DataSource = dt;
            gvCrewSearch.VirtualItemCount = dt.Rows.Count;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                Filter.CurrentCrewSelection = ((RadLabel)e.Item.FindControl("lblEmployeeid")).Text;
                Response.Redirect("..\\Crew\\CrewAppraisal.aspx?vslid=" + ((RadLabel)e.Item.FindControl("lblVesselid")).Text, false);
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvCrewSearch_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = sender as GridView;
    }
}
