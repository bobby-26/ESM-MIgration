using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix .Registers ;
using Telerik.Web.UI;

public partial class CrewOffshoreTQMatrixStandard : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreTQMatrixStandard.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvTMatrix')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            TabMenu.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvTMatrix.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
       }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void TabMenu_TabStripCommand(object sender, EventArgs e)
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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDCHARTERNAME" };
        string[] alCaptions = { "Training and Qualification Matrix Standard" };

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
        string Stdname = null; 
        DataSet ds = new DataSet();

        ds = PhoenixCrewOffshoreTrainingMatrixStandard.TrainingMatrixSearch(
                                                                 PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableString(Stdname)
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , General.ShowRecords(null)
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);
        DataTable dt = ds.Tables[0];
        General.ShowExcel("Training and Qualification Matrix Standard", dt, alColumns, alCaptions, sortdirection, sortexpression);
    }

    public void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDCHARTERNAME" };
        string[] alCaptions = { "Training and Qualification Matrix Standard" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        string Stdname = null;

        try
        {

            DataSet ds = new DataSet();

            ds = PhoenixCrewOffshoreTrainingMatrixStandard.TrainingMatrixSearch(
                                                                     PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , General.GetNullableString(Stdname)
                                                                    , sortexpression
                                                                    , sortdirection
                                                                    , (int)ViewState["PAGENUMBER"]
                                                                    , gvTMatrix.PageSize
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount);

            General.SetPrintOptions("gvTMatrix", "Training and Qualification Matrix Standard", alCaptions, alColumns, ds);
            gvTMatrix.DataSource = ds;
            gvTMatrix.VirtualItemCount = iRowCount;
          
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
  
   
 

    private bool IsValidStandardMatrix(int? charter)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (charter == null)
            ucError.ErrorMessage = "Charterer Name is required.";

        return (!ucError.IsError);
    }

   
  
    protected void gvTMatrix_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTMatrix.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTMatrix_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT")) return;


            if (e.CommandName.ToUpper().Equals("ADD"))
            {
              
                int? CharterAdd = General.GetNullableInteger(((UserControlAddressType)e.Item.FindControl("ddlCharterAdd")).SelectedAddress);

                string StrSname = null;
                if (!IsValidStandardMatrix(CharterAdd))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewOffshoreTrainingMatrixStandard.TrainingMtrixInsert(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    StrSname,
                    CharterAdd);

                BindData();
                gvTMatrix.Rebind();


            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if(e.CommandName.ToUpper()=="UPDATE")
            {
                try
                {
                    string STDName = null;
                    int? Charter = General.GetNullableInteger(((UserControlAddressType)e.Item.FindControl("ddlCharterEdit")).SelectedAddress);
                    // int Charterid = int.Parse(Charter);
                    if (!IsValidStandardMatrix(Charter))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    string StdId = ((RadLabel)e.Item.FindControl("lblTMSTDIdEdit")).Text;
                    Guid MatrixStdId = new Guid(StdId);


                    PhoenixCrewOffshoreTrainingMatrixStandard.TrainingMatrinxUpdate(
                      PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                      MatrixStdId,
                      STDName,
                      Charter
                      );
                   

                    BindData();
                    gvTMatrix.Rebind();
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
            if(e.CommandName.ToUpper()=="DELETE")
            {
                try
                {
                  
                    string StdId = ((RadLabel)e.Item.FindControl("lblTMSTDId")).Text;
                    Guid MatrixStdId = new Guid(StdId);

                    PhoenixCrewOffshoreTrainingMatrixStandard.TrainingMatrixDelete(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                      MatrixStdId);
                    BindData();
                    gvTMatrix.Rebind();

                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;

                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvTMatrix_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
          

            if (e.Item is GridDataItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                }
                UserControlAddressType ucAddress = (UserControlAddressType)e.Item.FindControl("ddlCharterEdit");
                Label lblcbtortc = (Label)e.Item.FindControl("lblCharterEdit");
                if (ucAddress != null)
                {
                    ucAddress.SelectedAddress = dr["FLDCHARTERID"].ToString();
                }
            }

            if (e.Item is GridFooterItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvTMatrix_SortCommand(object sender, GridSortCommandEventArgs e)
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

