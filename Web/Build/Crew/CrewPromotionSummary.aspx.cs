using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Web.UI;
using SouthNests.Phoenix.CrewManagement;

public partial class CrewPromotionSummary : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            //toolbarsub.AddFontAwesomeButton("../Crew/CrewPromotionSummary.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            //toolbarsub.AddFontAwesomeButton("javascript:CallPrint('gvPromotion')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            //toolbarsub.AddFontAwesomeButton("../Crew/CrewPromotionSummary.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            //toolbarsub.AddFontAwesomeButton("../Crew/CrewPromotionSummary.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbarsub.AddFontAwesomeButton("../Crew/CrewPromotionSummary.aspx", "Promote Seafarer", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            CrewPromotionMenu.AccessRights = this.ViewState;
            CrewPromotionMenu.MenuList = toolbarsub.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewPromotionSummary.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvPromotionPlan')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Crew/CrewPromotionSummary.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            CrewPromotionPlanMenu.AccessRights = this.ViewState;
            CrewPromotionPlanMenu.MenuList = toolbar.Show();


            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["FLDRANKFROM"] = "";
                SetEmployeePrimaryDetails();

                gvPromotionPlan.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ChkRankProm()
    {
        ucRankTo.Items.Clear();
        ucRankTo.SelectedValue = "";
        ucRankTo.Text = "";

        ucRankTo.DataSource = PhoenixRegistersRank.ListRankFilter(null, null, General.GetNullableInteger(ViewState["FLDRANKFROM"].ToString()), 0);
        ucRankTo.DataTextField = "FLDRANKNAME";
        ucRankTo.DataValueField = "FLDRANKID";
        ucRankTo.DataBind();
        ucRankTo.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                //txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDEMPLOYEENAME"].ToString();
                //txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();       
                ViewState["FLDRANKFROM"] = dt.Rows[0]["FLDRANK"].ToString();
                ChkRankProm();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewPromotionMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("ADD"))
            {
                int? empid = General.GetNullableInteger(Filter.CurrentCrewSelection);

                String scriptpopup = String.Format("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewPromotionDemotion.aspx?empid="+ empid + "',false,800,500);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopup, true);

            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;               
                gvPromotion.Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                ucRankTo.SelectedValue = "";
                ucRankTo.Text = "";
                BindData();
                gvPromotion.Rebind();
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
        //int iRowCount = 0;
        //int iTotalPageCount = 0;

        //string[] alColumns = {  };
        //string[] alCaptions = {  };

        //string sortexpression;
        //int? sortdirection = null;

        //sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        //if (ViewState["SORTDIRECTION"] != null)
        //    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        //if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
        //    iRowCount = 10;
        //else
        //    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        //DataSet ds = PhoenixCrewPromotionSummary.SearchEmployeePromotionSummary(General.GetNullableInteger(Filter.CurrentCrewSelection)
        //                                                            , General.GetNullableInteger(ViewState["FLDRANKFROM"].ToString())
        //                                                            , General.GetNullableInteger(ucRankTo.SelectedValue)
        //                                                           , sortexpression, sortdirection
        //                                                           , 1, iRowCount
        //                                                           , ref iRowCount, ref iTotalPageCount
        //                                                       );


        DataSet ds = PhoenixCrewPromotionSummary.ListEmployeePromotionSummary(General.GetNullableInteger(Filter.CurrentCrewSelection)
                                                                    , General.GetNullableInteger(ViewState["FLDRANKFROM"].ToString())
                                                                    , General.GetNullableInteger(ucRankTo.SelectedValue)
                                                                   
                                                               );

        //General.ShowExcel("Crew Promotion", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    public void BindData()
    {

        //int iRowCount = 0;
        //int iTotalPageCount = 0;

        //string[] alColumns = { "FLDROWNUMBER", "FLDRANKFROMNAME", "FLDRANKTONAME" };
        //string[] alCaptions = { "S.No", "Rank From", "Rank To" };

        //string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        //int? sortdirection = null;
        //if (ViewState["SORTDIRECTION"] != null)
        //    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {
            //DataSet ds = PhoenixCrewPromotionSummary.SearchEmployeePromotionSummary(General.GetNullableInteger(Filter.CurrentCrewSelection)
            //                                                       , General.GetNullableInteger(ViewState["FLDRANKFROM"].ToString())
            //                                                       , General.GetNullableInteger(ucRankTo.SelectedValue)
            //                                                      , sortexpression, sortdirection
            //                                                      ,(int)ViewState["PAGENUMBER"]
            //                                                      ,gvPromotion.PageSize
            //                                                      , ref iRowCount, ref iTotalPageCount
            //                                                  );

            DataSet ds = PhoenixCrewPromotionSummary.ListEmployeePromotionSummary(General.GetNullableInteger(Filter.CurrentCrewSelection)
                                                                        , General.GetNullableInteger(ViewState["FLDRANKFROM"].ToString())
                                                                        , General.GetNullableInteger(ucRankTo.SelectedValue)

                                                                   );

            //General.SetPrintOptions("gvPromotion", "Crew Promotion", alCaptions, alColumns, ds);
            
            gvPromotion.DataSource = ds;

            //ViewState["ROWCOUNT"] = iRowCount;
            //ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvPromotion_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPromotion.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvPromotion_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;

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


    protected void gvPromotion_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;


            RadLabel lblMissingYN = (RadLabel)e.Item.FindControl("lblMissingYN");
            RadLabel lblExpiredYN = (RadLabel)e.Item.FindControl("lblExpiredYN");
            RadLabel lblAvailableDocTask = (RadLabel)e.Item.FindControl("lblAvailableDocTask");
            RadLabel lblReqDocTask = (RadLabel)e.Item.FindControl("lblReqDocTask");


            if (lblExpiredYN.Text.Trim() == "1" || lblMissingYN.Text.Trim() == "1" )
            {
                lblReqDocTask.Attributes.Add("style", "color:red !important;");
                if (lblMissingYN.Text.Trim() == "1")
                {
                    
                    if (lblAvailableDocTask != null) lblAvailableDocTask.Visible = false;
                }
            }
        }

    }

    protected void gvPromotion_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
        BindData();
    }


    protected void ucRankTo_TextChanged(object sender, EventArgs e)
    {
        BindData();
        gvPromotion.Rebind();

        BindRankData();
        gvPromotionRank.Rebind();
    }

    public void BindRankData()
    {
        try
        {
            
            DataSet ds = PhoenixCrewPromotionSummary.ListEmployeePromotionRankSummary(General.GetNullableInteger(Filter.CurrentCrewSelection)
                                                                        , General.GetNullableInteger(ViewState["FLDRANKFROM"].ToString())
                                                                        , General.GetNullableInteger(ucRankTo.SelectedValue)

                                                                   );

            
            gvPromotionRank.DataSource = ds;

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvPromotionRank_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindRankData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPromotionRank_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvPromotionRank_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }

    protected void gvPromotionRank_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridGroupHeaderItem)
        {
            RadLabel lblCmbRank = (RadLabel)e.Item.FindControl("lblCmbRank");
            RadLabel lblIssuitable = (RadLabel)e.Item.FindControl("lblIssuitable");

            if (lblCmbRank != null)
            {
                if (lblIssuitable.Text.Trim() != "1")
                {
                    lblCmbRank.Attributes.Add("style", "color:red !important;");

                }

            }
        }

    }
    public void BindAppraisalData()
    {
        try
        {

            DataSet ds = PhoenixCrewPromotionSummary.ListEmployeePromotionAppraisalSummary(General.GetNullableInteger(Filter.CurrentCrewSelection)
                                                                        , General.GetNullableInteger(ViewState["FLDRANKFROM"].ToString())
                                                                        , General.GetNullableInteger(ucRankTo.SelectedValue)

                                                                   );
            
            gvPromotionAppraisal.DataSource = ds;

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvPromotionAppraisal_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindAppraisalData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPromotionAppraisal_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvPromotionAppraisal_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridGroupHeaderItem)
        {
            RadLabel lblCount = (RadLabel)e.Item.FindControl("lblCount");
            RadLabel lblIssuitable = (RadLabel)e.Item.FindControl("lblIssuitable");

            if ( lblCount != null)
            {
                if (lblIssuitable.Text.Trim() != "1" )
                {
                    lblCount.Attributes.Add("style", "color:red !important;");
                    
                }

            }
        }

    }

    protected void gvPromotionAppraisal_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }

    protected void CrewPromotionPlanMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("ADD"))
            {

                String scriptpopup = String.Format("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewPromotionPlanEdit.aspx" + "',false,800,500);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopup, true);

            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    public void BindPromotionPlanData()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCURRENTRANKNAME", "FLDPROMOTIONPLANDATE", "FLDRANKTONAME", "FLDACTIVEYESNO" };
        string[] alCaptions = { "Rank From", "Proposed Date", "Remarks", "Active" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {
            DataTable dt = PhoenixCrewPromotionSummary.CrewPromotionPlanSearch(General.GetNullableInteger(Filter.CurrentCrewSelection)
                                                                  , sortexpression, sortdirection
                                                                 , (int)ViewState["PAGENUMBER"], gvPromotion.PageSize
                                                                  , ref iRowCount, ref iTotalPageCount
                                                              );

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvPromotionPlan", "Promotion Plan", alCaptions, alColumns, ds);

            gvPromotionPlan.DataSource = dt;
            gvPromotionPlan.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvPromotionPlan_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPromotionPlan.CurrentPageIndex + 1;
            BindPromotionPlanData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvPromotionPlan_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        string id = ((RadLabel)e.Item.FindControl("lblId")).Text;
        PhoenixCrewPromotionSummary.CrewPromotionPlanDelete(new Guid(id));

        BindPromotionPlanData();
        gvPromotionPlan.Rebind();

    }

    protected void gvPromotionPlan_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;

            else if (e.CommandName.ToUpper().Equals("NAVIGATEEDIT"))
            {
                string id = ((RadLabel)e.Item.FindControl("lblId")).Text;

                String scriptpopup = String.Format("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewPromotionPlanEdit.aspx?iframIgnore=True&id=" + id + "',false,800,300);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopup, true);


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

    protected void gvPromotionPlan_ItemDataBound(object sender, GridItemEventArgs e)
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
            

        }

    }

    protected void gvPromotionPlan_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
        BindPromotionPlanData();

    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindPromotionPlanData();
        gvPromotionPlan.Rebind();
    }


}