using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class CrewOffshoreTrainingMatrixList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            ucCharterer.AddressType = ((int)PhoenixAddressType.CHARTERER).ToString();
           //  ucConfirm.Visible = false;
            confirm.Attributes.Add("style", "display:none");
            PhoenixToolbar toolbarsub = new PhoenixToolbar();

            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreTrainingMatrixList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarsub.AddFontAwesomeButton("javascript:CallPrint('gvTrainingMatrix')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreTrainingMatrixList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreTrainingMatrixList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreTrainingMatrixEdit.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDMATRIX");
            CrewTrainingMenu.AccessRights = this.ViewState;
            CrewTrainingMenu.MenuList = toolbarsub.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvTrainingMatrix.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();
            //SetPageNavigator();
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
            if (CommandName.ToUpper().Equals("ADDMATRIX"))
            {
                Response.Redirect("../CrewOffshore/CrewOffshoreTrainingMatrixEdit.aspx");
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvTrainingMatrix.Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                ucVesselType.SelectedVesseltype = "";
                ucRank.SelectedRank = "";
                ucCharterer.SelectedAddress = "";
                BindData();
                gvTrainingMatrix.Rebind();
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

        string[] alColumns = { "FLDROWNUMBER", "FLDMATRIXNAME" };
        string[] alCaptions = { "No", "Name" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixSearch(
                                                                   sortexpression, sortdirection
                                                                   , 1, iRowCount
                                                                   , ref iRowCount, ref iTotalPageCount
                                                                   , General.GetNullableInteger(ucVesselType.SelectedVesseltype)
                                                                   , General.GetNullableInteger(ucRank.SelectedRank)
                                                                   , General.GetNullableInteger(ucCharterer.SelectedAddress)
                                                               );

        General.ShowExcel("Training and Qualifications Matrix", dt, alColumns, alCaptions, sortdirection, sortexpression);
    }

    public void BindData()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROWNUMBER", "FLDMATRIXNAME" };
        string[] alCaptions = { "No", "Name" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {
            DataTable dt = PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixSearch(sortexpression, sortdirection
                                                                  , (int)ViewState["PAGENUMBER"], gvTrainingMatrix.PageSize
                                                                  , ref iRowCount, ref iTotalPageCount
                                                                  , General.GetNullableInteger(ucVesselType.SelectedVesseltype)
                                                                  , General.GetNullableInteger(ucRank.SelectedRank)
                                                                  , General.GetNullableInteger(ucCharterer.SelectedAddress)
                                                              );
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvTrainingMatrix", "Training and Qualifications Matrix", alCaptions, alColumns, ds);

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

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = 1;
            //UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            //if (ucCM.confirmboxvalue == 1)
            //{
            if (ViewState["MATRIXID"] != null && ViewState["MATRIXID"].ToString() != "")
            {
                PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixCopy(int.Parse(ViewState["MATRIXID"].ToString()));
                ucStatus.Text = "Copied Successfully";
                BindData();
                gvTrainingMatrix.Rebind();

            }
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidMatrix(string matrixname)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(matrixname) == null)
            ucError.ErrorMessage = "Matrix Name is required.";

        return (!ucError.IsError);
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

    protected void gvTrainingMatrix_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;


            if (e.CommandName.ToUpper().Equals("DETAILS"))
            {
                string matrixid = ((RadLabel)e.Item.FindControl("lblMatrixId")).Text;
                Response.Redirect("..\\CrewOffshore\\CrewOffshoreTrainingMatrixDetails.aspx?matrixid=" + matrixid);
            }
            else if (e.CommandName.ToUpper().Equals("NAVIGATEEDIT"))
            {
                string matrixid = ((RadLabel)e.Item.FindControl("lblMatrixId")).Text;
                Response.Redirect("..\\CrewOffshore\\CrewOffshoreTrainingMatrixEdit.aspx?matrixid=" + matrixid);
            }
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidMatrix(((RadTextBox)e.Item.FindControl("txtMatrixNameAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixInsert(((RadTextBox)e.Item.FindControl("txtMatrixNameAdd")).Text);
                BindData();
                gvTrainingMatrix.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixDelete(int.Parse(((RadLabel)e.Item.FindControl("lblMatrixId")).Text));
                BindData();
                gvTrainingMatrix.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("COPY"))
            {
                RadLabel lbl = (RadLabel)e.Item.FindControl("lblMatrixId");
                ViewState["MATRIXID"] = lbl.Text;
                //ucConfirm.Visible = true;
                //ucConfirm.Text = "Are you sure to copy the matrix?";
                //return;
                RadWindowManager1.RadConfirm("Are you sure to copy the matrix?", "confirm", 320, 150, null, "Confirm");

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

    protected void gvTrainingMatrix_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ed.CommandName))
                    ed.Visible = false;
            }

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, del.CommandName))
                    del.Visible = false;
                del.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event)"); 

            }


            LinkButton history = (LinkButton)e.Item.FindControl("imgHistory");
            RadLabel lblMatrixId = (RadLabel)e.Item.FindControl("lblMatrixId");

            if (history != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, history.CommandName))
                    history.Visible = false;

                history.Attributes.Add("onclick", "javascript:openNewWindow('chml', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreTrainingMatrixHistoryList.aspx?matrixid=" + lblMatrixId.Text + "');return false;");
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

    protected void gvTrainingMatrix_ItemDeleted(object sender, GridDeletedEventArgs e)
    {
        BindData();
        gvTrainingMatrix.Rebind();
    }
}
