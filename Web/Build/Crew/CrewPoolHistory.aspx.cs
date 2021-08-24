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


public partial class CrewPoolHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            CrewPoolChangeHistory.AccessRights = this.ViewState;
            CrewPoolChangeHistory.MenuList = toolbarmain.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewPoolHistory.aspx" + (Request.QueryString.ToString() != string.Empty ? "?" + Request.QueryString.ToString() : string.Empty), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvPoolHistory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            PoolHistoryList.AccessRights = this.ViewState;
            PoolHistoryList.MenuList = toolbar.Show();
            
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                txtTransferDate.Text = DateTime.Now.ToString();

                gvPoolHistory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewPoolChangeHistory_TabStripCommand(object sender, EventArgs e)
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
                    PhoenixCrewPersonal.UpdateEmployeePoolUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , int.Parse(Request.QueryString["empid"].ToString()), txtRemarks.Text.Trim(), int.Parse(ucPool.SelectedPool), DateTime.Parse(txtTransferDate.Text));

                    ucStatus.Text = "Pool is updated succesfully";
                    BindData();
                    gvPoolHistory.Rebind();                     
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

        if (General.GetNullableInteger(ucPool.SelectedPool) == null)
            ucError.ErrorMessage = "Pool is required.";
        if (General.GetNullableString(txtRemarks.Text) == null)
            ucError.ErrorMessage = "Remarks is required.";

        return (!ucError.IsError);
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDROWNUMBER", "FLDFROMPOOLNAME", "FLDTOPOOLNAME", "FLDREMARKS", "FLDDATE", "FLDCREATEDBY" };
        string[] alCaptions = { "S.No.", "Pool From", "Pool To", "Remarks", "Transfer Date", "Changed By" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewPersonal.PoolHistoryList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , int.Parse(Filter.CurrentCrewSelection)
                        , sortexpression, sortdirection
                        , 1
                        , iRowCount
                        , ref iRowCount
                        , ref iTotalPageCount);
        
        if (ds.Tables.Count > 0)
            General.ShowExcel("Pool Change History", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        
    }

    protected void PoolHistoryList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDROWNUMBER", "FLDFROMPOOLNAME", "FLDTOPOOLNAME", "FLDREMARKS", "FLDDATE", "FLDCREATEDBY" };
        string[] alCaptions = { "S.No.", "Pool From", "Pool To", "Remarks", "Transfer Date", "Changed By" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewPersonal.PoolHistoryList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , int.Parse(Filter.CurrentCrewSelection)
                        , sortexpression, sortdirection
                        , gvPoolHistory.CurrentPageIndex + 1
                        , gvPoolHistory.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);

        General.SetPrintOptions("gvPoolHistory", "Pool Change History", alCaptions, alColumns, ds);
        
        gvPoolHistory.DataSource = ds;
        gvPoolHistory.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
    }

    protected void gvPoolHistory_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvPoolHistory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;

            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPoolHistory_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();    
    }
    
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvPoolHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
