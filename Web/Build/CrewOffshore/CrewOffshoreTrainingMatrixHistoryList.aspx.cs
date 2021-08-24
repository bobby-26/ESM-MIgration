using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CrewOffshoreTrainingMatrixHistoryList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreTrainingMatrixHistoryList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarsub.AddFontAwesomeButton("javascript:CallPrint('gvTrainingMatrix')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            CrewTrainingMenu.AccessRights = this.ViewState;
            CrewTrainingMenu.MenuList = toolbarsub.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                if (Request.QueryString["matrixid"] != null && Request.QueryString["matrixid"].ToString() != "")
                    ViewState["matrixid"] = Request.QueryString["matrixid"].ToString();

                gvTrainingMatrix.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
           
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void CrewTrainingMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
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

    protected void gvTrainingMatrix_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
  


    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDMATRIXNAME", "FLDUPDATEDBY", "FLDUPDATEDDATE" };
        string[] alCaptions = { "Name", "Updated By", "Updated Date" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixHistorySearch(int.Parse(ViewState["matrixid"].ToString())
                                                                   , sortexpression, sortdirection
                                                                   , 1, iRowCount
                                                                   , ref iRowCount, ref iTotalPageCount
                                                               );

        General.ShowExcel("Training and Qualifications Matrix History", dt, alColumns, alCaptions, sortdirection, sortexpression);
    }

    public void BindData()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDMATRIXNAME", "FLDUPDATEDBY", "FLDUPDATEDDATE" };
        string[] alCaptions = { "Name", "Updated By", "Updated Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {
            DataTable dt = PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixHistorySearch(int.Parse(ViewState["matrixid"].ToString())
                                                                  , sortexpression, sortdirection
                                                                  , (int)ViewState["PAGENUMBER"], gvTrainingMatrix.PageSize
                                                                  , ref iRowCount, ref iTotalPageCount
                                                              );
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvTrainingMatrix", "Training and Qualifications Matrix History", alCaptions, alColumns, ds);
            gvTrainingMatrix.DataSource = dt;
            gvTrainingMatrix.VirtualItemCount = iRowCount;
         

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
           
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTrainingMatrix_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTrainingMatrix.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTrainingMatrix_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;
          

            if (e.CommandName.ToUpper().Equals("DETAILS"))
            {
                string matrixid = ((RadLabel)e.Item.FindControl("lblMatrixId")).Text;
                string historyid = ((RadLabel)e.Item.FindControl("lblHistoryId")).Text;

                Response.Redirect("..\\CrewOffshore\\CrewOffshoreTrainingMatrixHistoryDetails.aspx?matrixid=" + matrixid + "&historyid=" + historyid);
            }
            if (e.CommandName == "Page")
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

    protected void gvTrainingMatrix_ItemDataBound(object sender, GridItemEventArgs e)
    {
        
        if (e.Item is GridDataItem)
        {
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdDetails");
            if (ed != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ed.CommandName))
                    ed.Visible = false;
            }
        }
    }

    protected void gvTrainingMatrix_SortCommand(object sender, GridSortCommandEventArgs e)
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
}
