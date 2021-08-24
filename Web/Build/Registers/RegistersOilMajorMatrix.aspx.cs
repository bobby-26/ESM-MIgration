using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class RegistersOilMajorMatrix : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersOilMajorMatrix.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewOilMajorMatrix')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuCrewOilMajorMatrix.AccessRights = this.ViewState;
            MenuCrewOilMajorMatrix.MenuList = toolbar.Show();

            PhoenixToolbar toolbar2 = new PhoenixToolbar();
            toolbar2.AddButton("Oil Major", "OILMAJOR");
            toolbar2.AddButton("Mapping", "MAPPING");
            oilmajor.AccessRights = this.ViewState;
            oilmajor.MenuList = toolbar2.Show();
            oilmajor.SelectedMenuIndex = 0;
            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersOilMajorMatrix.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL1");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewOilMajorMatrixSub')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuCrewOilMajorMatrixSub.AccessRights = this.ViewState;
            MenuCrewOilMajorMatrixSub.MenuList = toolbar.Show();            
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["SUBPAGENUMBER"] = 1;
                ViewState["SUBSORTEXPRESSION"] = null;
                ViewState["SUBSORTDIRECTION"] = null;
                ViewState["SUBCURRENTINDEX"] = 1;
                ViewState["MATRIXID"] = "";
                gvCrewOilMajorMatrix.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvCrewOilMajorMatrixSub.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            BindData();
            BindSubData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void oilmajor_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("MAPPING"))
            {
                Response.Redirect("RegistersOilMajorVesselMapping.aspx", false);
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
            string[] alColumns = { "FLDOILMAJORNAME", "FLDRANKNAMELIST", "FLDCONTRACTNAME", "FLDOPERATOREXP", "FLDRANKEXP", "FLDTHISTYPEEXP", "FLDALLTYPEEXP", "FLDJOININGDATEDIFF" };
            string[] alCaptions = { "Oil Major", "Ranks", "Contract", "Operator Exp", "Rank Exp", "This Type Exp", "All Type Exp", "Joining Date Diff" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewsOilMajorMatrix.OilMajorMatrixSearch(General.GetNullableInteger(ddlOilMajor.SelectedHard)
                        , General.GetNullableInteger(ddlContract.SelectedHard)
                        , sortexpression, sortdirection
                        , (int)ViewState["PAGENUMBER"]
                        , gvCrewOilMajorMatrix.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);
            General.SetPrintOptions("gvCrewOilMajorMatrix", "Oil MajorMatrix", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCrewOilMajorMatrix.DataSource = ds;
                gvCrewOilMajorMatrix.VirtualItemCount = iRowCount;
            }
            else
            {
                gvCrewOilMajorMatrix.DataSource = "";
            };
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewOilMajorMatrix_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDOILMAJORNAME", "FLDRANKNAMELIST", "FLDCONTRACTNAME", "FLDOPERATOREXP", "FLDRANKEXP", "FLDTHISTYPEEXP", "FLDALLTYPEEXP", "FLDJOININGDATEDIFF" };
                string[] alCaptions = { "Oil Major", "Ranks", "Contract", "Operator Exp", "Rank Exp", "This Type Exp", "All Type Exp", "Joining Date Diff" };
                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
                DataSet ds = PhoenixCrewsOilMajorMatrix.OilMajorMatrixSearch(General.GetNullableInteger(ddlOilMajor.SelectedHard)
                        , General.GetNullableInteger(ddlContract.SelectedHard)
                        , sortexpression, sortdirection
                        , 1
                        , gvCrewOilMajorMatrix.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Oil Major", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            else if (CommandName.ToUpper().Equals("EXCEL1"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDRANKNAME", "FLDOPERATOREXP", "FLDRANKEXP", "FLDTHISTYPEEXP", "FLDALLTYPEEXP" };
                string[] alCaptions = { "Rank", "Operator Exp", "Rank Exp", "This Type Exp", "All Type Exp" };
                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int? sortdirection = null;
                int? matrixid = null;
                if (ViewState["SUBSORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SUBSORTDIRECTION"].ToString());
                if (ViewState["MATRIXID"] != null)
                {
                    matrixid = General.GetNullableInteger(ViewState["MATRIXID"].ToString());
                }
                DataSet ds = PhoenixCrewsOilMajorMatrix.OilMajorMatrixSubSearch(
                            matrixid, null, null, null, null, null
                            , sortexpression, sortdirection
                            , (int)ViewState["SUBPAGENUMBER"]
                            , gvCrewOilMajorMatrixSub.PageSize
                            , ref iRowCount
                            , ref iTotalPageCount);
                if (ds.Tables.Count > 0)
                    General.ShowExcel("Oil Major Sub", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidOilMajor(string oilmajor, string contract, string operatorexp, string rankexp, string thistypeexp, string alltypeexp, string joiningdatediff)
    {
        Int16 resultInt;

        ucError.HeaderMessage = "Please provide the following required information";

        if (!Int16.TryParse(oilmajor, out resultInt))
            ucError.ErrorMessage = "Oil Major is required";

        if (!Int16.TryParse(contract, out resultInt))
            ucError.ErrorMessage = "Contract is required";

        if (operatorexp != "" && !Int16.TryParse(operatorexp, out resultInt))
            ucError.ErrorMessage = "Not a valid month";

        if (rankexp != "" && !Int16.TryParse(rankexp, out resultInt))
            ucError.ErrorMessage = "Not a valid month";

        if (thistypeexp != "" && !Int16.TryParse(thistypeexp, out resultInt))
            ucError.ErrorMessage = "Not a valid month";

        if (alltypeexp != "" && !Int16.TryParse(alltypeexp, out resultInt))
            ucError.ErrorMessage = "Not a valid month";

        if (joiningdatediff != "" && !Int16.TryParse(joiningdatediff, out resultInt))
            ucError.ErrorMessage = "Not a valid joining days diff";

        return (!ucError.IsError);
    }


    public StateBag ReturnViewState()
    {
        return ViewState;
    }


    private void BindSubData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDRANKNAME", "FLDOPERATOREXP", "FLDRANKEXP", "FLDTHISTYPEEXP", "FLDALLTYPEEXP" };
            string[] alCaptions = { "Rank", "Operator Exp", "Rank Exp", "This Type Exp", "All Type Exp" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            int? matrixid = null;
            if (ViewState["SUBSORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SUBSORTDIRECTION"].ToString());
            if (ViewState["MATRIXID"] != null)
            {
                matrixid = General.GetNullableInteger(ViewState["MATRIXID"].ToString());
            }
            DataSet ds = PhoenixCrewsOilMajorMatrix.OilMajorMatrixSubSearch(
                        matrixid, null, null, null, null, null
                        , sortexpression, sortdirection
                        , (int)ViewState["SUBPAGENUMBER"]
                        , gvCrewOilMajorMatrixSub.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);

            General.SetPrintOptions("gvCrewOilMajorMatrixSub", "Oil Major Sub", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCrewOilMajorMatrixSub.DataSource = ds;
                gvCrewOilMajorMatrixSub.VirtualItemCount = iRowCount;
            }
            else
            {
                gvCrewOilMajorMatrixSub.DataSource = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidOilMajorSub(string rankid, string operatorexp, string rankexp, string thistypeexp, string alltypeexp, string matrixid)
    {
        Int16 resultInt;

        ucError.HeaderMessage = "Please provide the following required information";

        if (matrixid == "")
            ucError.ErrorMessage = "Please select the Oil Major";

        if (!Int16.TryParse(rankid, out resultInt))
            ucError.ErrorMessage = "Rank is required";

        if (operatorexp != "" && !Int16.TryParse(operatorexp, out resultInt))
            ucError.ErrorMessage = "Not a valid month";

        if (rankexp != "" && !Int16.TryParse(rankexp, out resultInt))
            ucError.ErrorMessage = "Not a valid month";

        if (thistypeexp != "" && !Int16.TryParse(thistypeexp, out resultInt))
            ucError.ErrorMessage = "Not a valid month";

        if (alltypeexp != "" && !Int16.TryParse(alltypeexp, out resultInt))
            ucError.ErrorMessage = "Not a valid month";



        return (!ucError.IsError);
    }

    protected void gvCrewOilMajorMatrix_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "ADD")
            {
                string oilmajor = ddlOilMajor.SelectedHard;
                string contract = ddlContract.SelectedHard;
                string operatorexp = ((RadTextBox)e.Item.FindControl("txtOperatorExpAdd")).Text;
                string rankexp = ((RadTextBox)e.Item.FindControl("txtRankExpAdd")).Text;
                string thistypeexp = ((RadTextBox)e.Item.FindControl("txtThisTypeExpAdd")).Text;
                string alltypeexp = ((RadTextBox)e.Item.FindControl("txtAllTypeExpAdd")).Text;
                string JoiningDate = ((RadTextBox)e.Item.FindControl("txtJoiningDateAdd")).Text;

                if (!IsValidOilMajor(oilmajor, contract, operatorexp, rankexp, thistypeexp, alltypeexp, JoiningDate))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewsOilMajorMatrix.InsertOilMajorMatrix(
                      PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Convert.ToInt32(oilmajor)
                    , Convert.ToInt32(contract)
                    , General.GetNullableInteger(operatorexp)
                    , General.GetNullableInteger(rankexp)
                    , General.GetNullableInteger(thistypeexp)
                    , General.GetNullableInteger(alltypeexp)
                    , General.GetNullableInteger(JoiningDate)
                    );

                BindData();
                gvCrewOilMajorMatrix.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "UPDATE")
            {
                string matrixid = ((RadLabel)e.Item.FindControl("lblMatrixIdEdit")).Text;
                string operatorexp = ((RadTextBox)e.Item.FindControl("txtOperatorExpEdit")).Text;
                string rankexp = ((RadTextBox)e.Item.FindControl("txtRankExpEdit")).Text;
                string thistypeexp = ((RadTextBox)e.Item.FindControl("txtThisTypeExpEdit")).Text;
                string alltypeexp = ((RadTextBox)e.Item.FindControl("txtAllTypeExpEdit")).Text;
                string JoiningDate = ((RadTextBox)e.Item.FindControl("txtJoiningDateEdit")).Text;

                if (!IsValidOilMajor("1", "1", operatorexp, rankexp, thistypeexp, alltypeexp, JoiningDate))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewsOilMajorMatrix.UpdateOilMajorMatrix(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Convert.ToInt32(matrixid)
                    , General.GetNullableInteger(operatorexp)
                    , General.GetNullableInteger(rankexp)
                    , General.GetNullableInteger(thistypeexp)
                    , General.GetNullableInteger(alltypeexp)
                    , General.GetNullableInteger(JoiningDate)
                   );
                BindData();
                gvCrewOilMajorMatrix.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "DELETE")
            {
                string matrixid = ((RadLabel)e.Item.FindControl("lblMatrixId")).Text;
                PhoenixCrewsOilMajorMatrix.DeleteOilMajorMatrix(
                    Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
                    , Convert.ToInt32(matrixid));
                BindData();
                gvCrewOilMajorMatrix.Rebind();
                BindSubData();
                gvCrewOilMajorMatrixSub.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "SELECT" || e.CommandName.ToString().ToUpper() == "ROWCLICK")
            {
                ViewState["MATRIXID"] = ((RadLabel)e.Item.FindControl("lblMatrixId")).Text;
                BindSubData();
                gvCrewOilMajorMatrixSub.Rebind();
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

    protected void gvCrewOilMajorMatrix_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewOilMajorMatrix.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvCrewOilMajorMatrix_ItemDataBound(object sender, GridItemEventArgs e)
    {
        //if (e.Item is GridDataItem)
        //{
        LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null)
        {
            del.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
        }

        LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

        LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
        if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

        //if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState != DataControlRowState.Edit && e.Row.RowState != (DataControlRowState.Alternate | DataControlRowState.Edit))
        //{

        //    LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
        //    string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
        //    e.Row.Attributes["onclick"] = _jsDouble;

        //}
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
        //       && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
        //    {
        //        ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");


        //    }
        //    else
        //    {
        //        e.Row.Attributes["onclick"] = "";
        //    }

        //}
        //}
    }

    protected void gvCrewOilMajorMatrixSub_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["SUBPAGENUMBER"] = ViewState["SUBPAGENUMBER"] != null ? ViewState["SUBPAGENUMBER"] : gvCrewOilMajorMatrixSub.CurrentPageIndex + 1;
        BindSubData();
    }

    protected void gvCrewOilMajorMatrixSub_ItemDataBound(object sender, GridItemEventArgs e)
    {
        LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null)
        {
            del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            del.Attributes.Add("onclick", "return fnConfirmDelete(event)");
        }

        LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

        LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
        if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

        UserControlRank ucRank = (UserControlRank)e.Item.FindControl("ucRankEdit");
        DataRowView drvRank = (DataRowView)e.Item.DataItem;
        if (ucRank != null) ucRank.SelectedRank = drvRank["FLDRANKID"].ToString();
    }

    protected void gvCrewOilMajorMatrixSub_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "ADD")
            {
                string matrixid = ViewState["MATRIXID"].ToString();
                string rankid = ((UserControlRank)e.Item.FindControl("ucRankAdd")).SelectedRank;
                string operatorexp = ((RadTextBox)e.Item.FindControl("txtOperatorExpSubAdd")).Text;
                string rankexp = ((RadTextBox)e.Item.FindControl("txtRankExpSubAdd")).Text;
                string thistypeexp = ((RadTextBox)e.Item.FindControl("txtThisTypeExpSubAdd")).Text;
                string alltypeexp = ((RadTextBox)e.Item.FindControl("txtAllTypeExpSubAdd")).Text;

                if (!IsValidOilMajorSub(rankid, operatorexp, rankexp, thistypeexp, alltypeexp, matrixid))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewsOilMajorMatrix.InsertOilMajorMatrixSub(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    Convert.ToInt32(ViewState["MATRIXID"].ToString())
                    , Convert.ToInt32(rankid)
                    , General.GetNullableInteger(operatorexp)
                    , General.GetNullableInteger(rankexp)
                    , General.GetNullableInteger(thistypeexp)
                    , General.GetNullableInteger(alltypeexp)
                    );
                BindSubData();
                gvCrewOilMajorMatrixSub.Rebind();
                BindData();
                gvCrewOilMajorMatrix.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "UPDATE")
            {
                string matrixid = ViewState["MATRIXID"].ToString();
                string submatrixid = ((RadLabel)e.Item.FindControl("lblMatrixIdEdit")).Text;
                string rankid = ((UserControlRank)e.Item.FindControl("ucRankEdit")).SelectedRank;
                string operatorexpsub = ((RadTextBox)e.Item.FindControl("txtOperatorExpSubEdit")).Text;
                string rankexpsub = ((RadTextBox)e.Item.FindControl("txtRankExpSubEdit")).Text;
                string thistypeexpsub = ((RadTextBox)e.Item.FindControl("txtThisTypeExpSubEdit")).Text;
                string alltypeexpsub = ((RadTextBox)e.Item.FindControl("txtAllTypeExpSubEdit")).Text;

                if (!IsValidOilMajorSub(rankid, operatorexpsub, rankexpsub, thistypeexpsub, alltypeexpsub, matrixid))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewsOilMajorMatrix.UpdateOilMajorMatrixSub(1,
                      int.Parse(submatrixid), Convert.ToInt32(ViewState["MATRIXID"].ToString())
                    , Convert.ToInt32(rankid)
                    , General.GetNullableInteger(operatorexpsub)
                    , General.GetNullableInteger(rankexpsub)
                    , General.GetNullableInteger(thistypeexpsub)
                    , General.GetNullableInteger(alltypeexpsub)
                    );
                BindSubData();
                gvCrewOilMajorMatrixSub.Rebind();
                BindData();
                gvCrewOilMajorMatrix.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "DELETE")
            {
                string submatrixid = ((RadLabel)e.Item.FindControl("lblSubMatrixId")).Text;
                PhoenixCrewsOilMajorMatrix.DeleteOilMajorMatrixSub(Convert.ToInt32(submatrixid));
                BindSubData();
                gvCrewOilMajorMatrixSub.Rebind();
                BindData();
                gvCrewOilMajorMatrix.Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["SUBPAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewOilMajorMatrix_DataBound(object sender, EventArgs e)
    {
        //if(gvCrewOilMajorMatrix.MasterTableView.Items.Count>0)
        //{
        //    gvCrewOilMajorMatrix.MasterTableView.Items[0].Selected = true;

        //    BindSubData();
        //    gvCrewOilMajorMatrixSub.Rebind();
        //}
    }

    protected void gvCrewOilMajorMatrix_SortCommand(object sender, GridSortCommandEventArgs e)
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
      