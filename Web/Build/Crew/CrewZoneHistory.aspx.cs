using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using System.Web.UI;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewZoneHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            CrewZoneChangeHistory.AccessRights = this.ViewState;
            CrewZoneChangeHistory.MenuList = toolbarmain.Show();
            
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewZoneHistory.aspx" + (Request.QueryString.ToString() != string.Empty ? "?" + Request.QueryString.ToString() : string.Empty), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvZoneHistory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            ZoneHistoryList.AccessRights = this.ViewState;
            ZoneHistoryList.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                txtTransferDate.Text = DateTime.Now.ToString();

                gvZoneHistory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
         
         
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewZoneChangeHistory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidate())
                {
                    ucError.Visible = true;
                    return;
                }
                if (Filter.CurrentCrewSelection != null)
                {
                    PhoenixCrewZoneHistory.InsertZoneChangeHistory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Convert.ToInt32(Filter.CurrentCrewSelection), int.Parse(ucZone.SelectedZone), txtRemarks.Text, General.GetNullableDateTime(txtTransferDate.Text));
                    ucStatus.Text = "Zone is updated succesfully";
                    BindData();
                    gvZoneHistory.Rebind();
                 
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1','ifMoreInfo');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidate()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucZone.SelectedZone) == null)
            ucError.ErrorMessage = "Zone is required.";
        if (General.GetNullableString(txtRemarks.Text) == null)
            ucError.ErrorMessage = "Remarks is required.";

        return (!ucError.IsError);
    }

    protected void ZoneHistoryList_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDROWNUMBER", "FLDOLDZONE", "FLDNEWZONE", "FLDREMARKS", "FLDCREATEDDATE", "FLDTRANSFERDATE", "FLDCREATEDBYNAME" };
        string[] alCaptions = { "Sr.No", "Zone From", "Zone To", "Remarks", "Date Changed", "Transfer Date", "Changed by" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewZoneHistory.ZoneChangeHistorySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , int.Parse(Filter.CurrentCrewSelection)
                        , sortexpression, sortdirection
                        , 1
                        , iRowCount
                        , ref iRowCount
                        , ref iTotalPageCount);

        if (ds.Tables.Count > 0)
            General.ShowExcel("Zone Change History", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

   

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROWNUMBER", "FLDOLDZONE", "FLDNEWZONE", "FLDREMARKS", "FLDCREATEDDATE", "FLDTRANSFERDATE", "FLDCREATEDBYNAME" };
        string[] alCaptions = { "S.No", "Zone From", "Zone To", "Remarks", "Date Changed", "Transfer Date", "Changed by" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewZoneHistory.ZoneChangeHistorySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , int.Parse(Filter.CurrentCrewSelection)
                        , sortexpression, sortdirection
                        , gvZoneHistory.CurrentPageIndex + 1
                        , gvZoneHistory.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);

        General.SetPrintOptions("gvZoneHistory", "Zone Change History", alCaptions, alColumns, ds);

        gvZoneHistory.DataSource = ds;
        gvZoneHistory.VirtualItemCount = iRowCount;        
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

   
    protected void gvZoneHistory_Sorting(object sender, GridViewSortEventArgs se)
    {
     
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
    
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();    
    }
    

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvZoneHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvZoneHistory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvZoneHistory_ItemDataBound(object sender, GridItemEventArgs e)
    {
    }
}
