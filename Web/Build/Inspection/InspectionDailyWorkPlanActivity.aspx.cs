using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionDailyWorkPlanActivity : PhoenixBasePage
{

    

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Page.Header.DataBind(); 
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSectionmit.Attributes.Add("style", "display:none;");

            if (!IsPostBack)
            {
                ViewState["Page"] = string.Empty;
                VesselConfiguration();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;


                ViewState["PAGENUMBERSECTION"] = 1;
                ViewState["SORTEXPRESSIONSECTION"] = null;
                ViewState["SORTDIRECTIONSECTION"] = null;



                ViewState["PAGENUMBERFORM"] = 1;
                ViewState["SORTEXPRESSIONFORM"] = null;
                ViewState["SORTDIRECTIONFORM"] = null;



                ViewState["PAGENUMBERRISKASSESSMENT"] = 1;
                ViewState["SORTEXPRESSIONRISKASSESSMENT"] = null;
                ViewState["SORTDIRECTIONRISKASSESSMENT"] = null;

                PhoenixToolbar toolbarmain = new PhoenixToolbar();

                toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
                toolbarmain.AddButton("List", "LIST", ToolBarDirection.Right);
                //toolbarmain.AddButton("Daily Work Plan", "WORKPLAN");
                MenuDeficiencyGeneral.AccessRights = this.ViewState;
                MenuDeficiencyGeneral.MenuList = toolbarmain.Show();

                //MenuDeficiencyGeneral.SelectedMenuIndex = 1;

                //PhoenixToolbar toolbar = new PhoenixToolbar();

                //WorkPlanMain.AccessRights = this.ViewState;
                //WorkPlanMain.MenuList = toolbar.Show();
                ucVessel.bind();
                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                    ucVessel.Company = nvc.Get("QMS");
                    ucVessel.bind();
                }

                if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
                {
                    ucVessel.SelectedVessel = Filter.CurrentVesselConfiguration.ToString();
                    ucVessel.Enabled = false;
                }

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }

                if (Request.QueryString["WORKPLANID"] != null && Request.QueryString["WORKPLANID"].ToString() != "")
                {
                    ViewState["WORKPLANID"] = Request.QueryString["WORKPLANID"].ToString();
                    EditDailyWorkPlan();
                }
                else
                    ViewState["WORKPLANID"] = "";

                gvOperations.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvDeck.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvEngine.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvCatering.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            ucError.ErrorMessage = "";
            if (Request.QueryString["Page"] != null && Request.QueryString["Page"].ToString() != "")
            {
                ViewState["Page"] = Request.QueryString["Page"].ToString();
            }
            //BindOperations();
            //  //SetPageNavigator();
            //BindDeck();
            //SetPageNavigatorDeck();
            //BindEngine();
            //SetPageNavigatorEngine();
            // BindCatering();
            //SetPageNavigatorCatering();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    // ----- DOCUMENT ----- //

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        LinkButton ib = (LinkButton)sender;
        BindOperations();
        // //SetPageNavigator();
    }

    protected void DeficiencyGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Inspection/InspectionDailyWorkPlanList.aspx?Page="+ ViewState["Page"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidDailyWorkPlan())
                {
                    if (ViewState["WORKPLANID"] == null || ViewState["WORKPLANID"].ToString() == "")
                    {
                        Guid workplanid = Guid.Empty;
                        PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanInsert(int.Parse(ucVessel.SelectedVessel)
                                                        , General.GetNullableDateTime(ucDate.Text)
                                                        , null
                                                        , null
                                                        , null
                                                        , ref workplanid
                                                        , General.GetNullableString(txtRemarks.Text));

                        ViewState["WORKPLANID"] = workplanid;
                        ucStatus.Text = "Daily Work Plan Added";
                        EditDailyWorkPlan();
                    }
                    else
                    {
                        PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanUpdate(new Guid(ViewState["WORKPLANID"].ToString())
                                                                , int.Parse(ucVessel.SelectedVessel)
                                                                , General.GetNullableDateTime(ucDate.Text)
                                                                , null
                                                                , null
                                                                , null
                                                                , General.GetNullableString(txtRemarks.Text)
                                                                );

                        ucStatus.Text = "Daily Work Plan Updated";

                        EditDailyWorkPlan();
                    }


                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void WorkPlanMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            Guid pniid = Guid.Empty;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidDailyWorkPlan())
                {
                    if (ViewState["WORKPLANID"] == null || ViewState["WORKPLANID"].ToString() == "")
                    {
                        Guid workplanid = Guid.Empty;
                        PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanInsert(int.Parse(ucVessel.SelectedVessel)
                                                        , General.GetNullableDateTime(ucDate.Text)
                                                        , null
                                                        , null
                                                        , null
                                                        , ref workplanid
                                                        , General.GetNullableString(txtRemarks.Text));

                        ViewState["WORKPLANID"] = workplanid;
                        ucStatus.Text = "Daily Work Plan Added";
                        EditDailyWorkPlan();
                    }
                    else
                    {
                        PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanUpdate(new Guid(ViewState["WORKPLANID"].ToString())
                                                                , int.Parse(ucVessel.SelectedVessel)
                                                                , General.GetNullableDateTime(ucDate.Text)
                                                                , null
                                                                , null
                                                                , null
                                                                , General.GetNullableString(txtRemarks.Text)
                                                                );

                        ucStatus.Text = "Daily Work Plan Updated";

                        EditDailyWorkPlan();
                    }


                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private bool IsValidDailyWorkPlan()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is Required.";

        if (General.GetNullableDateTime(ucDate.Text) == null)
            ucError.ErrorMessage = "Date is Required.";
        //else if (DateTime.TryParse(ucDate.Text, out resultdate) && DateTime.Compare(DateTime.Parse(DateTime.Now.ToLongDateString()),resultdate ) > 0)
        //    ucError.ErrorMessage = "Date Should not be a Past Date";

        return (!ucError.IsError);
    }

    private bool IsValidDailyWorkPlanActivity(string sno, string activity, string workplanactivityid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(sno) == null && General.GetNullableGuid(workplanactivityid) == null)
            ucError.ErrorMessage = "SNo is Required.";

        if (General.GetNullableString(activity) == null && General.GetNullableGuid(workplanactivityid) == null)
            ucError.ErrorMessage = "Activity is Required.";

        //if (General.GetNullableGuid(hazard) == null)
        //    ucError.ErrorMessage = "Hazard is Required.";

        //if (General.GetNullableGuid(workpermit) == null)
        //    ucError.ErrorMessage = "Work Permit is Required.";

        //if (General.GetNullableGuid(ra) == null)
        //    ucError.ErrorMessage = "Risk Assessment is Required.";

        return (!ucError.IsError);
    }
    private void EditDailyWorkPlan()
    {
        if (ViewState["WORKPLANID"] != null)
        {
            DataSet ds = PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanEdit(General.GetNullableGuid(ViewState["WORKPLANID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ucVessel.Company = ViewState["COMPANYID"].ToString();
                ucVessel.bind();
                ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                ucVessel.Enabled = false;
                ucDate.Text = dr["FLDDATE"].ToString();
                txtRemarks.Text = dr["FLDWORKPLANREMARKS"].ToString();

            }
        }
    }
    private void BindOperations()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanActivitySearch(General.GetNullableGuid(ViewState["WORKPLANID"].ToString())
                                                                , 1
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , gvOperations.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        gvOperations.DataSource = ds;
        gvOperations.VirtualItemCount = iRowCount;


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    //protected void gvOperations_Sorting(object sender, GridViewSortEventArgs se)
    //{
    //    gvOperations.EditIndex = -1;
    //    gvOperations.SelectedIndex = -1;
    //    ViewState["SORTEXPRESSION"] = se.SortExpression;

    //    if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
    //        ViewState["SORTDIRECTION"] = 1;
    //    else
    //        ViewState["SORTDIRECTION"] = 0;

    //    BindOperations();
    //}

    protected void gvOperations_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvOperations, "Select$" + e.Row.RowIndex.ToString(), false);
        }
    }

    

    protected void gvOperations_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        //gvOperations.SelectedIndex = se.NewSelectedIndex;

    }

    

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        //gvOperations.EditIndex = -1;
        //gvOperations.SelectedIndex = -1;
        BindOperations();
        //SetPageNavigator();
    }





    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }



    protected void cmdHiddenSectionmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        //gvOperations.EditIndex = -1;
        //gvOperations.SelectedIndex = -1;
        BindOperations();
        //SetPageNavigator();
    }

    // ----- SECTION ----- //

    private void BindDeck()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSIONSECTION"] == null) ? null : (ViewState["SORTEXPRESSIONSECTION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTIONSECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONSECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanActivitySearch(General.GetNullableGuid(ViewState["WORKPLANID"].ToString())
                                                                , 2
                                                                , gvDeck.CurrentPageIndex + 1
                                                                , gvDeck.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);
        gvDeck.DataSource = ds;
        gvDeck.VirtualItemCount = iRowCount;


        ViewState["ROWCOUNTSECTION"] = iRowCount;
        ViewState["TOTALPAGECOUNTSECTION"] = iTotalPageCount;
    }

    protected void gvDeck_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvDeck, "Select$" + e.Row.RowIndex.ToString(), false);
        }
    }

    

    private void BindEngine()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSIONFORM"] == null) ? null : (ViewState["SORTEXPRESSIONFORM"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTIONFORM"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONFORM"].ToString());


            DataSet ds = PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanActivitySearch(General.GetNullableGuid(ViewState["WORKPLANID"].ToString())
                                                                 , 3
                                                                 , gvEngine.CurrentPageIndex + 1
                                                                 , gvEngine.PageSize
                                                                 , ref iRowCount
                                                                 , ref iTotalPageCount);

            gvEngine.DataSource = ds;
            gvDeck.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNTFORM"] = iRowCount;
            ViewState["TOTALPAGECOUNTFORM"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    

    protected void gvEngine_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        //gvEngine.SelectedIndex = se.NewSelectedIndex;

        //Label lblFormId = ((Label)gvEngine.Rows[se.NewSelectedIndex].FindControl("lblFormId"));
        //if (lblFormId != null)
        //    ViewState["FORMID"] = lblFormId.Text;
    }

    protected void gvEngine_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvEngine, "Select$" + e.Row.RowIndex.ToString(), false);
        }
    }



    //protected void PagerButtonClickEngine(object sender, CommandEventArgs ce)
    //{
    //    try
    //    {
    //        gvEngine.SelectedIndex = -1;
    //        gvEngine.EditIndex = -1;
    //        if (ce.CommandName == "prev")
    //            ViewState["PAGENUMBERFORM"] = (int)ViewState["PAGENUMBERFORM"] - 1;
    //        else
    //            ViewState["PAGENUMBERFORM"] = (int)ViewState["PAGENUMBERFORM"] + 1;

    //        BindEngine();
    //        //SetPageNavigatorEngine();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}


    private Boolean IsPreviousEnabledEngine()
    {

        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERFORM"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTFORM"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;

    }

    private Boolean IsNextEnabledEngine()
    {

        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERFORM"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTFORM"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    //protected string GetParentIframeURL(string referenceid)
    //{
    //    string strURL = "";
    //    int type = 0;
    //    DataSet ds = PhoenixDocumentManagementDocument.GetSelectedeTreeNodeType(General.GetNullableGuid(referenceid), ref type);
    //    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //    {
    //        DataRow dr = ds.Tables[0].Rows[0];
    //        strURL = dr["FLDURL"].ToString();
    //    }

    //    return strURL;
    //}


    // ----- RA ----- //

    private void BindCatering()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSIONRISKASSESSMENT"] == null) ? null : (ViewState["SORTEXPRESSIONRISKASSESSMENT"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTIONRISKASSESSMENT"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONRISKASSESSMENT"].ToString());


            DataSet ds = PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanActivitySearch(General.GetNullableGuid(ViewState["WORKPLANID"].ToString())
                                                               , 4
                                                               , gvCatering.CurrentPageIndex + 1
                                                               , gvCatering.PageSize
                                                               , ref iRowCount
                                                               , ref iTotalPageCount);

            gvCatering.DataSource = ds;
            gvCatering.VirtualItemCount = iRowCount;


            ViewState["ROWCOUNTRISKASSESSMENT"] = iRowCount;
            ViewState["TOTALPAGECOUNTRISKASSESSMENT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    

    protected void gvCatering_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        //gvCatering.SelectedIndex = se.NewSelectedIndex;

        //Label lblProcessId = ((Label)gvCatering.Rows[se.NewSelectedIndex].FindControl("lblProcessId"));
        //if (lblProcessId != null)
        //    ViewState["PROCESSID"] = lblProcessId.Text;
    }

    protected void gvCatering_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvCatering, "Select$" + e.Row.RowIndex.ToString(), false);
        }
    }






    private Boolean IsPreviousEnabledCatering()
    {

        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERRISKASSESSMENT"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTRISKASSESSMENT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;

    }

    private Boolean IsNextEnabledCatering()
    {

        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERRISKASSESSMENT"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTRISKASSESSMENT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    // ----- JHA ----- //

    //private void BindJobHazard()
    //{
    //    try
    //    {
    //        int iRowCount = 0;
    //        int iTotalPageCount = 0;

    //        string sortexpression = (ViewState["SORTEXPRESSIONJOBHAZARD"] == null) ? null : (ViewState["SORTEXPRESSIONJOBHAZARD"].ToString());
    //        int? sortdirection = null;
    //        if (ViewState["SORTDIRECTIONJOBHAZARD"] != null)
    //            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONJOBHAZARD"].ToString());


    //        DataSet ds = PhoenixDocumentManagementDocument.KeyWordSearchResultInJHA(
    //                                                              General.GetNullableString(ViewState["keyword"].ToString())
    //                                                            , sortexpression
    //                                                            , sortdirection
    //                                                            , (int)ViewState["PAGENUMBERJOBHAZARD"]
    //                                                            , General.ShowRecords(null)
    //                                                            , ref iRowCount
    //                                                            , ref iTotalPageCount);

    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            gvJobHazard.DataSource = ds;
    //            gvJobHazard.DataBind();
    //            if (General.GetNullableGuid(ViewState["JOBHAZARDID"].ToString()) == null)
    //            {
    //                lblJobHazardTitle.Text = ds.Tables[0].Rows[0]["FLDSEARCHRESULT"].ToString();
    //                ViewState["JOBHAZARDID"] = ds.Tables[0].Rows[0]["FLDJOBHAZARDID"].ToString();
    //                gvJobHazard.SelectedIndex = 0;
    //            }
    //            //SetRowSelection();
    //        }
    //        else
    //        {
    //            ShowNoRecordsFound(ds.Tables[0], gvJobHazard);
    //        }

    //        ViewState["ROWCOUNTJOBHAZARD"] = iRowCount;
    //        ViewState["TOTALPAGECOUNTJOBHAZARD"] = iTotalPageCount;

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvJobHazard_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;

    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //        if (e.CommandName.ToUpper().Equals("SELECT"))
    //        {

    //            Label lblJobHazardId = ((Label)gvJobHazard.Rows[nCurrentRow].FindControl("lblJobHazardId"));
    //            if (lblJobHazardId != null)
    //                ViewState["JOBHAZARDID"] = lblJobHazardId.Text;
    //        }
    //        SetPageNavigatorJobHazard();

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.HeaderMessage = "Please make the required correction";
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvJobHazard_RowDataBound(object sender, GridViewRowEventArgs e)
    //{

    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        if (ViewState["SORTEXPRESSIONJOBHAZARD"] != null)
    //        {
    //            HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSIONJOBHAZARD"].ToString());
    //            if (img != null)
    //            {
    //                if (ViewState["SORTDIRECTIONJOBHAZARD"] == null || ViewState["SORTDIRECTIONJOBHAZARD"].ToString() == "0")
    //                    img.Src = Session["images"] + "/arrowUp.png";
    //                else
    //                    img.Src = Session["images"] + "/arrowDown.png";

    //                img.Visible = true;
    //            }
    //        }
    //    }
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {            
    //        UserControlToolTip ucJob = (UserControlToolTip)e.Row.FindControl("ucJob");
    //        Label lblJob = (Label)e.Row.FindControl("lblJob");
    //        if (lblJob != null)
    //        {
    //            lblJob.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucJob.ToolTip + "', 'visible');");
    //            lblJob.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucJob.ToolTip + "', 'hidden');");
    //        }
    //    }

    //}

    //protected void gvJobHazard_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    //{
    //    gvJobHazard.SelectedIndex = se.NewSelectedIndex;

    //    Label lblJobHazardId = ((Label)gvJobHazard.Rows[se.NewSelectedIndex].FindControl("lblJobHazardId"));
    //    if (lblJobHazardId != null)
    //        ViewState["JOBHAZARDID"] = lblJobHazardId.Text;
    //}

    //protected void gvJobHazard_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow
    //        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
    //        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
    //    {
    //        e.Row.TabIndex = -1;
    //        e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvJobHazard, "Select$" + e.Row.RowIndex.ToString(), false);
    //    }
    //}

    //protected void cmdGoJobHazard_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int result;
    //        if (Int32.TryParse(txtnopageJobHazard.Text, out result))
    //        {
    //            ViewState["PAGENUMBERJOBHAZARD"] = Int32.Parse(txtnopageRiskAssessment.Text);

    //            if ((int)ViewState["TOTALPAGECOUNTJOBHAZARD"] < Int32.Parse(txtnopageJobHazard.Text))
    //                ViewState["PAGENUMBERJOBHAZARD"] = ViewState["TOTALPAGECOUNTJOBHAZARD"];


    //            if (0 >= Int32.Parse(txtnopageJobHazard.Text))
    //                ViewState["PAGENUMBERJOBHAZARD"] = 1;

    //            if ((int)ViewState["PAGENUMBERJOBHAZARD"] == 0)
    //                ViewState["PAGENUMBERJOBHAZARD"] = 1;

    //            txtnopageJobHazard.Text = ViewState["PAGENUMBERJOBHAZARD"].ToString();
    //        }
    //        BindJobHazard();
    //        SetPageNavigatorJobHazard();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void PagerButtonClickJobHazard(object sender, CommandEventArgs ce)
    //{
    //    try
    //    {
    //        gvJobHazard.SelectedIndex = -1;
    //        gvJobHazard.EditIndex = -1;
    //        if (ce.CommandName == "prev")
    //            ViewState["PAGENUMBERJOBHAZARD"] = (int)ViewState["PAGENUMBERJOBHAZARD"] - 1;
    //        else
    //            ViewState["PAGENUMBERJOBHAZARD"] = (int)ViewState["PAGENUMBERJOBHAZARD"] + 1;

    //        BindJobHazard();
    //        SetPageNavigatorJobHazard();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //private void SetPageNavigatorJobHazard()
    //{
    //    try
    //    {
    //        cmdPreviousJobHazard.Enabled = IsPreviousEnabledJobHazard();
    //        cmdNextJobHazard.Enabled = IsNextEnabledJobHazard();
    //        lblPagenumberJobHazard.Text = "Page " + ViewState["PAGENUMBERJOBHAZARD"].ToString();
    //        lblPagesJobHazard.Text = " of " + ViewState["TOTALPAGECOUNTJOBHAZARD"].ToString() + " Pages. ";
    //        lblRecordsJobHazard.Text = "(" + ViewState["ROWCOUNTJOBHAZARD"].ToString() + " records found)";
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //private Boolean IsPreviousEnabledJobHazard()
    //{

    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBERJOBHAZARD"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNTJOBHAZARD"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //    {
    //        return true;
    //    }

    //    return false;

    //}

    //private Boolean IsNextEnabledJobHazard()
    //{

    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBERJOBHAZARD"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNTJOBHAZARD"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //} 

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }
    protected void gvOperations_PreRender(object sender, EventArgs e)
    {
        GridView gridView = (GridView)sender;

        for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
        {
            GridViewRow row = gridView.Rows[rowIndex];
            GridViewRow previousRow = gridView.Rows[rowIndex + 1];

            string currentWorkPlanactivityId = ((Label)gridView.Rows[rowIndex].FindControl("lblWorkPlanActivityId")).Text;
            string previousWorkPlanactivityId = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblWorkPlanActivityId")).Text;


            if (currentWorkPlanactivityId == previousWorkPlanactivityId)
            {
                row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                    previousRow.Cells[1].RowSpan + 1;
                previousRow.Cells[1].Visible = false;

                row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                    previousRow.Cells[2].RowSpan + 1;
                previousRow.Cells[2].Visible = false;

                row.Cells[3].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
                                    previousRow.Cells[3].RowSpan + 1;
                previousRow.Cells[3].Visible = false;

                row.Cells[4].RowSpan = previousRow.Cells[4].RowSpan < 2 ? 2 :
                                    previousRow.Cells[4].RowSpan + 1;
                previousRow.Cells[4].Visible = false;

                row.Cells[8].RowSpan = previousRow.Cells[8].RowSpan < 2 ? 2 :
                                    previousRow.Cells[8].RowSpan + 1;
                previousRow.Cells[8].Visible = false;

                row.Cells[9].RowSpan = previousRow.Cells[9].RowSpan < 2 ? 2 :
                                    previousRow.Cells[9].RowSpan + 1;
                previousRow.Cells[9].Visible = false;
            }

        }
    }

    protected void gvDeck_PreRender(object sender, EventArgs e)
    {

        GridView gridView = (GridView)sender;

        for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
        {
            GridViewRow row = gridView.Rows[rowIndex];
            GridViewRow previousRow = gridView.Rows[rowIndex + 1];

            string currentWorkPlanactivityId = ((Label)gridView.Rows[rowIndex].FindControl("lblWorkPlanActivityId")).Text;
            string previousWorkPlanactivityId = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblWorkPlanActivityId")).Text;


            if (currentWorkPlanactivityId == previousWorkPlanactivityId)
            {
                row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                    previousRow.Cells[1].RowSpan + 1;
                previousRow.Cells[1].Visible = false;

                row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                    previousRow.Cells[2].RowSpan + 1;
                previousRow.Cells[2].Visible = false;

                row.Cells[3].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
                                    previousRow.Cells[3].RowSpan + 1;
                previousRow.Cells[3].Visible = false;

                row.Cells[4].RowSpan = previousRow.Cells[4].RowSpan < 2 ? 2 :
                                    previousRow.Cells[4].RowSpan + 1;
                previousRow.Cells[4].Visible = false;

                row.Cells[8].RowSpan = previousRow.Cells[8].RowSpan < 2 ? 2 :
                                    previousRow.Cells[8].RowSpan + 1;
                previousRow.Cells[8].Visible = false;

                row.Cells[9].RowSpan = previousRow.Cells[9].RowSpan < 2 ? 2 :
                                    previousRow.Cells[9].RowSpan + 1;
                previousRow.Cells[9].Visible = false;
            }

        }
    }

    protected void gvEngine_PreRender(object sender, EventArgs e)
    {

        GridView gridView = (GridView)sender;

        for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
        {
            GridViewRow row = gridView.Rows[rowIndex];
            GridViewRow previousRow = gridView.Rows[rowIndex + 1];


            string currentWorkPlanactivityId = ((Label)gridView.Rows[rowIndex].FindControl("lblWorkPlanActivityId")).Text;
            string previousWorkPlanactivityId = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblWorkPlanActivityId")).Text;


            if (currentWorkPlanactivityId == previousWorkPlanactivityId)
            {
                row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                    previousRow.Cells[1].RowSpan + 1;
                previousRow.Cells[1].Visible = false;

                row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                    previousRow.Cells[2].RowSpan + 1;
                previousRow.Cells[2].Visible = false;

                row.Cells[3].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
                                    previousRow.Cells[3].RowSpan + 1;
                previousRow.Cells[3].Visible = false;

                row.Cells[4].RowSpan = previousRow.Cells[4].RowSpan < 2 ? 2 :
                                    previousRow.Cells[4].RowSpan + 1;
                previousRow.Cells[4].Visible = false;

                row.Cells[8].RowSpan = previousRow.Cells[8].RowSpan < 2 ? 2 :
                                    previousRow.Cells[8].RowSpan + 1;
                previousRow.Cells[8].Visible = false;

                row.Cells[9].RowSpan = previousRow.Cells[9].RowSpan < 2 ? 2 :
                                    previousRow.Cells[9].RowSpan + 1;
                previousRow.Cells[9].Visible = false;
            }

        }
    }

    protected void gvCatering_PreRender(object sender, EventArgs e)
    {

        GridView gridView = (GridView)sender;

        for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
        {
            GridViewRow row = gridView.Rows[rowIndex];
            GridViewRow previousRow = gridView.Rows[rowIndex + 1];

            string currentWorkPlanactivityId = ((Label)gridView.Rows[rowIndex].FindControl("lblWorkPlanActivityId")).Text;
            string previousWorkPlanactivityId = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblWorkPlanActivityId")).Text;


            if (currentWorkPlanactivityId == previousWorkPlanactivityId)
            {
                row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                    previousRow.Cells[1].RowSpan + 1;
                previousRow.Cells[1].Visible = false;

                row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                    previousRow.Cells[2].RowSpan + 1;
                previousRow.Cells[2].Visible = false;

                row.Cells[3].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
                                    previousRow.Cells[3].RowSpan + 1;
                previousRow.Cells[3].Visible = false;

                row.Cells[4].RowSpan = previousRow.Cells[4].RowSpan < 2 ? 2 :
                                    previousRow.Cells[4].RowSpan + 1;
                previousRow.Cells[4].Visible = false;

                row.Cells[8].RowSpan = previousRow.Cells[8].RowSpan < 2 ? 2 :
                                    previousRow.Cells[8].RowSpan + 1;
                previousRow.Cells[8].Visible = false;

                row.Cells[9].RowSpan = previousRow.Cells[9].RowSpan < 2 ? 2 :
                                    previousRow.Cells[9].RowSpan + 1;
                previousRow.Cells[9].Visible = false;
            }

        }
    }

    //protected void RadioButton_CheckedChanged(object sender, EventArgs e)
    //{
    //    RadioButton rdbtn = (RadioButton)sender;
    //    GridView gv = (GridView)rdbtn.Parent.Parent.Parent.Parent;
    //    TextBox txtActivityAdd = (TextBox)gv.FooterRow.FindControl("txtActivityAdd");
    //    if (txtActivityAdd != null)
    //    {
    //        txtActivityAdd.CssClass = "readonlytextbox";
    //        txtActivityAdd.ReadOnly = true;
    //    }

    //    UserControlMaskedTextBox ucSNoAdd = (UserControlMaskedTextBox)gv.FooterRow.FindControl("ucSNoAdd");
    //    if (ucSNoAdd != null)
    //    {
    //        ucSNoAdd.CssClass = "readonlytextbox";
    //        ucSNoAdd.ReadOnly = true;
    //    }

    //}


    protected void gvOperations_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindOperations();
    }

    protected void gvOperations_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            Guid? workplanactivityid = null;
            foreach (GridDataItem row in gvOperations.MasterTableView.Items)
            {
                RadioButton rdbtn = (RadioButton)row.FindControl("rdbUser");
                if (rdbtn != null)
                {
                    if (rdbtn.Checked)
                    {
                        workplanactivityid = General.GetNullableGuid(((RadLabel)row.FindControl("lblWorkPlanActivityId")).Text);
                    }

                }
            }
            //if (e.CommandName.ToUpper().Equals("OPERATIONSEDIT"))
            //{
            //    //if (_gridView.EditIndex > -1)
            //    //    _gridView.UpdateRow(_gridView.EditIndex, false);
            //    //_gridView.EditIndex = nCurrentRow;
            //    //_gridView.SelectedIndex = de.NewEditIndex;

            //    BindOperations();
            //    //SetPageNavigator();
            //}
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (IsValidDailyWorkPlanActivity(((UserControlMaskedTextBox)e.Item.FindControl("ucSNoEdit")).Text
                    , ((RadTextBox)e.Item.FindControl("txtActivityEdit")).Text
                    , workplanactivityid.ToString()
                    ))
                {
                    string timeofstart = (((UserControlMaskedTextBox)e.Item.FindControl("txtStartTimeEdit")).TextWithLiterals);
                    timeofstart = (timeofstart.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : timeofstart;

                    string timeofend = (((UserControlMaskedTextBox)e.Item.FindControl("txtEndTimeEdit")).TextWithLiterals);
                    timeofend = (timeofend.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : timeofend;

                    PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanActivityUpdate(
                                        General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblWorkPlanActivityLineItemId")).Text)
                                        , General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtHazardIdEdit")).Text)
                                        , General.GetNullableGuid(((UserControlWorkPermitByCompany)e.Item.FindControl("ucWorkPermitEdit")).SelectedWorkPermit)
                                        , General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtRAIdEdit")).Text)
                                        , General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtRaTypeEdit")).Text)
                                        , General.GetNullableInteger(((UserControlMaskedTextBox)e.Item.FindControl("ucSNoEdit")).Text)
                                        , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtActivityEdit")).Text)
                                        , General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtCrewIdEdit")).Text)
                                        , General.GetNullableDateTime(Convert.ToDateTime(ucDate.Text).ToString("dd/MM/yyyy") + " " + timeofstart)
                                        , General.GetNullableDateTime(Convert.ToDateTime(ucDate.Text).ToString("dd/MM/yyyy") + " " + timeofend)
                                        , int.Parse(ucVessel.SelectedVessel)
                                        );

                    BindOperations();
                    gvOperations.Rebind();
                    //SetPageNavigator();
                }
                else
                {
                    ucError.Visible = true;
                    return;

                }
            }
            else if (e.CommandName.ToUpper().Equals("OPERATIONSADD"))
            {
                if (IsValidDailyWorkPlanActivity(((UserControlMaskedTextBox)e.Item.FindControl("ucSNoAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtActivityAdd")).Text
                    , workplanactivityid.ToString()
                    ))
                {
                    if (ViewState["WORKPLANID"] != null && ViewState["WORKPLANID"].ToString() != "")
                    {
                        string timeofstart = (((UserControlMaskedTextBox)e.Item.FindControl("txtStartTimeAdd")).TextWithLiterals);
                        timeofstart = (timeofstart.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : timeofstart;

                        string timeofend = (((UserControlMaskedTextBox)e.Item.FindControl("txtEndTimeAdd")).TextWithLiterals);
                        timeofend = (timeofend.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : timeofend;
                        PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanActivityInsert(
                                            General.GetNullableGuid(ViewState["WORKPLANID"].ToString())
                                            , workplanactivityid
                                            , int.Parse(ucVessel.SelectedVessel)
                                            , General.GetNullableInteger(((UserControlMaskedTextBox)e.Item.FindControl("ucSNoAdd")).Text)
                                            , 1
                                            , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtActivityAdd")).Text)
                                            , General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtHazardIdAdd")).Text)
                                            , General.GetNullableGuid(((UserControlWorkPermitByCompany)e.Item.FindControl("ucWorkPermitAdd")).SelectedWorkPermit)
                                            , General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtRAIdAdd")).Text)
                                            , General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtRaTypeAdd")).Text)
                                            , General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtCrewIdAdd")).Text)
                                            , General.GetNullableDateTime(Convert.ToDateTime(ucDate.Text).ToString("dd/MM/yyyy") + " " + timeofstart)
                                            , General.GetNullableDateTime(Convert.ToDateTime(ucDate.Text).ToString("dd/MM/yyyy") + " " + timeofend)
                                            );
                        BindOperations();
                        gvOperations.Rebind();
                        //SetPageNavigator();
                    }
                    else
                    {
                        ucError.ErrorMessage = "Save before adding Activity.";
                        ucError.Visible = true;
                    }
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
            else if (e.CommandName.ToUpper().Equals("OPERATIONSDELETE"))
            {
                PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanActivityDelete(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblWorkPlanActivityLineItemId")).Text));
                BindOperations();
                gvOperations.Rebind();
                //SetPageNavigator();
            }
            else if (e.CommandName.ToUpper().Equals("OPERATIONSCANCEL"))
            {
                //_gridView.EditIndex = -1;
                BindOperations();
                //SetPageNavigator();
            }
            else if (e.CommandName.ToUpper().Equals("OPERATIONSCLEAR"))
            {
                foreach (GridDataItem row in gvOperations.MasterTableView.Items)
                {
                    RadioButton rdbtn = (RadioButton)row.FindControl("rdbUser");
                    if (rdbtn != null)
                    {
                        if (rdbtn.Checked)
                        {
                            rdbtn.Checked = false;
                        }

                    }
                }
            }

            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvOperations_ItemDataBound1(object sender, GridItemEventArgs e)
    {


        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);


            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }






            LinkButton lnkHazardNumber = (LinkButton)e.Item.FindControl("lnkHazardNumber");
            RadLabel lbljobhazardid = (RadLabel)e.Item.FindControl("lblHazardId");
            RadLabel lblHazardNumber = (RadLabel)e.Item.FindControl("lblHazardNumber");

            if (lblHazardNumber != null && lblHazardNumber.Text.ToUpper().Equals("N/A"))
            {
                lblHazardNumber.Visible = true;
                lnkHazardNumber.Visible = false;
            }
            LinkButton lnkRA = (LinkButton)e.Item.FindControl("lnkRA");
            RadLabel lnkRaId = (RadLabel)e.Item.FindControl("lblRAId");
            RadLabel lblRA = (RadLabel)e.Item.FindControl("lblRA");
            LinkButton JHAList = (LinkButton)e.Item.FindControl("imgOperationJHAList");
            RadLabel lblImportedJHA = (RadLabel)e.Item.FindControl("lblOperationJHAList");
            // Check that the RA has imported JHA

            if (lblRA != null && lblRA.Text.ToUpper().Equals("N/A"))
            {
                lblRA.Visible = true;
                lnkRA.Visible = false;
                if (JHAList != null)
                {
                    JHAList.Visible = false;
                }
            }

            if (lblImportedJHA == null || lblImportedJHA.Text == "")
            {
                if (JHAList != null)
                {
                    JHAList.Visible = false;
                }
            }
            if (lnkHazardNumber != null && lbljobhazardid.Text != null)
            {
                //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                //{
                //    lnkHazardNumber.Attributes.Add("onclick", "javascript:parent.openNewWindow('JobHazard','','" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=9&reportcode=JOBHAZARD&showmenu=0&showword=NO&showexcel=NO&showpdf=NO&showprint=YES&jobhazardid=" + lbljobhazardid.Text + "'); return false;");
                //}
                //else
                {
                    lnkHazardNumber.Attributes.Add("onclick", "javascript:parent.openNewWindow('JobHazard','','" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=9&reportcode=JOBHAZARD&showmenu=0&showword=NO&showexcel=NO&jobhazardid=" + lbljobhazardid.Text + "'); return false;");
                }
            }

            if (JHAList != null)
            {
                JHAList.Attributes.Add("onclick", "javascript:parent.openNewWindow('JobHazard','','" + Session["sitepath"] + "/Inspection/InspectionRAProcessImportedJHAList.aspx?RAPROCESSID=" + lnkRaId.Text + "');return false;");
            }

            if (DataBinder.Eval(e.Item.DataItem, "FLDRISKASSESSMENTTYPE").ToString() == "1")
            {
                if (lnkRA != null)
                {
                    //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    //{
                    //    lnkRA.Attributes.Add("onclick", "javascript:parent.openNewWindow('JobHazard','','" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=9&reportcode=RAPROCESS&showmenu=0&showword=NO&showexcel=NO&showpdf=NO&showprint=YES&processid=" + lnkRaId.Text + "'); return false;");
                    //}
                    //else
                    {
                        lnkRA.Attributes.Add("onclick", "javascript:parent.openNewWindow('JobHazard','','" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=9&reportcode=RAPROCESS&showmenu=0&showword=NO&showexcel=NO&processid=" + lnkRaId.Text + "'); return false;");
                    }
                }
            }
            else if (DataBinder.Eval(e.Item.DataItem, "FLDRISKASSESSMENTTYPE").ToString() == "2")
            {
                if (lnkRA != null)
                {
                    //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    //{
                    //    lnkRA.Attributes.Add("onclick", "openNewWindow('RAGeneric', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=9&reportcode=RAGENERIC&genericid=" + lnkRaId.Text + "&showmenu=0&showword=NO&showexcel=NO&showpdf=NO&showprint=YES');return false;");
                    //}
                    //else
                    {
                        lnkRA.Attributes.Add("onclick", "openNewWindow('RAGeneric', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAGENERIC&genericid=" + lnkRaId.Text + "&showmenu=0&showexcel=NO');return false;");
                    }
                }
            }
            else if (DataBinder.Eval(e.Item.DataItem, "FLDRISKASSESSMENTTYPE").ToString() == "3")
            {
                if (lnkRA != null)
                {
                    //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    //{
                    //    lnkRA.Attributes.Add("onclick", "openNewWindow('RAMachinery', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + lnkRaId.Text + "&showmenu=0&showword=NO&showexcel=NO&showpdf=NO&showprint=YES');return false;");
                    //}
                    //else
                    {
                        lnkRA.Attributes.Add("onclick", "openNewWindow('RAMachinery', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + lnkRaId.Text + "&showmenu=0&showexcel=NO');return false;");
                    }
                }
            }
            else if (DataBinder.Eval(e.Item.DataItem, "FLDRISKASSESSMENTTYPE").ToString() == "4")
            {
                if (lnkRA != null)
                {
                    //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    //{
                    //    lnkRA.Attributes.Add("onclick", "openNewWindow('RANavigation', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=9&reportcode=RANAVIGATION&navigationid=" + lnkRaId.Text + "&showmenu=0&showword=NO&showexcel=NO&showpdf=NO&showprint=YES');return false;");
                    //}
                    //else
                    {
                        lnkRA.Attributes.Add("onclick", "openNewWindow('RANavigation', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RANAVIGATION&navigationid=" + lnkRaId.Text + "&showmenu=0&showexcel=NO');return false;");
                    }
                }
            }
            else if (DataBinder.Eval(e.Item.DataItem, "FLDRISKASSESSMENTTYPE").ToString() == "5")
            {
                if (lnkRA != null)
                    lnkRA.Attributes.Add("onclick", "openNewWindow('RAcargo', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RACARGO&genericid=" + lnkRaId.Text + "&showmenu=0&showexcel=NO');return false;");
            }


            HyperLink lnkWorkPermit = (HyperLink)e.Item.FindControl("lnkWorkPermit");
            RadLabel lblWorkPermit = (RadLabel)e.Item.FindControl("lblWorkPermit");

            if (lblWorkPermit != null && lblWorkPermit.Text.ToUpper().Equals("N/A"))
            {
                lblWorkPermit.Visible = true;
                lnkWorkPermit.Visible = false;
            }

            if (!string.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "FLDDTKEY").ToString()))
            {
                DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(DataBinder.Eval(e.Item.DataItem, "FLDDTKEY").ToString()));
                if (dt.Rows.Count > 0)
                {
                    DataRow drRow = dt.Rows[0];
                    if (lnkWorkPermit != null)
                        lnkWorkPermit.NavigateUrl = "../Common/download.aspx?dtkey=" + drRow["FLDDTKEY"].ToString();
                }
            }

            HyperLink lnkworkpermit = (HyperLink)e.Item.FindControl("lnkWorkPermit");
            //if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            if (lnkworkpermit != null)
            {

                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipWorkPermit");
                lnkworkpermit.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lnkworkpermit.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }

            RadLabel lblIncharge = (RadLabel)e.Item.FindControl("lblIncharge");
            if (lblIncharge != null)
            {

                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipPICName");
                lblIncharge.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lblIncharge.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
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

        if (e.Item is GridDataItem)
        {
            //ucSNoEdit


            RadTextBox txtHazardIdEdit = (RadTextBox)e.Item.FindControl("txtHazardIdEdit");
            if (txtHazardIdEdit != null) txtHazardIdEdit.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtRAIdEdit = (RadTextBox)e.Item.FindControl("txtRAIdEdit");
            if (txtRAIdEdit != null) txtRAIdEdit.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtRaTypeEdit = (RadTextBox)e.Item.FindControl("txtRaTypeEdit");
            if (txtRaTypeEdit != null) txtRaTypeEdit.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtCrewIdEdit = (RadTextBox)e.Item.FindControl("txtCrewIdEdit");
            if (txtCrewIdEdit != null) txtCrewIdEdit.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtActivityEdit = (RadTextBox)e.Item.FindControl("txtActivityEdit");

            LinkButton imgShowHazardEdit = (LinkButton)e.Item.FindControl("imgShowHazardEdit");
            if (imgShowHazardEdit != null)
            {
                imgShowHazardEdit.Attributes.Add("onclick", "return showPickList('spnHazardEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListJHA.aspx?&vesselid=" + ucVessel.SelectedVessel + "&activity=" + txtActivityEdit.Text + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, imgShowHazardEdit.CommandName)) imgShowHazardEdit.Visible = false;

            }
            LinkButton imgShowRAEdit = (LinkButton)e.Item.FindControl("imgShowRAEdit");
            if (imgShowRAEdit != null)
            {
                imgShowRAEdit.Attributes.Add("onclick", "return showPickList('spnRAEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListDailyWorkPlanRA.aspx?&vesselid=" + ucVessel.SelectedVessel + "&activity=" + txtActivityEdit.Text + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, imgShowRAEdit.CommandName)) imgShowRAEdit.Visible = false;

            }

            LinkButton imgShowCrewInChargeEdit = (LinkButton)e.Item.FindControl("imgShowCrewInChargeEdit");
            if (imgShowCrewInChargeEdit != null)
            {
                imgShowCrewInChargeEdit.Attributes.Add("onclick", "return showPickList('spnCrewInChargeEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListVesselEmployeeOnboard.aspx?VesselId=" + ucVessel.SelectedVessel + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, imgShowCrewInChargeEdit.CommandName)) imgShowCrewInChargeEdit.Visible = false;

            }


            UserControlWorkPermitByCompany ucworkpermit = (UserControlWorkPermitByCompany)e.Item.FindControl("ucWorkPermitEdit");
            if (ucworkpermit != null)
            {
                DataSet ds = PhoenixInspectionDailyWorkPlanActivity.WorkPermitListByCompany(3, General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
                ucworkpermit.WorkPermitList = ds;
                ucworkpermit.DataBind();
                ucworkpermit.SelectedWorkPermit = DataBinder.Eval(e.Item.DataItem, "FLDWORKPERMITID").ToString();// drv["FLDWORKPERMITID"].ToString();
            }
            UserControlMaskedTextBox txtStartTimeEdit = (UserControlMaskedTextBox)e.Item.FindControl("txtStartTimeEdit");
            if (txtStartTimeEdit != null)
                txtStartTimeEdit.TextWithLiterals = String.Format("{0:dd/MM/yyyy hh:mm tt}", DataBinder.Eval(e.Item.DataItem, "FLDSTARTDATETIME"));

            UserControlMaskedTextBox txtEndTimeEdit = (UserControlMaskedTextBox)e.Item.FindControl("txtEndTimeEdit");
            if (txtEndTimeEdit != null)
                txtEndTimeEdit.TextWithLiterals = String.Format("{0:dd/MM/yyyy hh:mm tt}", DataBinder.Eval(e.Item.DataItem, "FLDENDDATETIME"));

            UserControlMaskedTextBox ucSNoEdit = (UserControlMaskedTextBox)e.Item.FindControl("ucSNoEdit");
            if (ucSNoEdit != null) ucSNoEdit.Text = DataBinder.Eval(e.Item.DataItem, "FLDSERIALNUMBER").ToString();

        }

        if (e.Item is GridFooterItem)
        {
            RadTextBox txtHazardIdAdd = (RadTextBox)e.Item.FindControl("txtHazardIdAdd");
            if (txtHazardIdAdd != null) txtHazardIdAdd.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtRAIdAdd = (RadTextBox)e.Item.FindControl("txtRAIdAdd");
            if (txtRAIdAdd != null) txtRAIdAdd.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtRaTypeAdd = (RadTextBox)e.Item.FindControl("txtRaTypeAdd");
            if (txtRaTypeAdd != null) txtRaTypeAdd.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtActivityAdd = (RadTextBox)e.Item.FindControl("txtActivityAdd");

            RadTextBox txtCrewIdAdd = (RadTextBox)e.Item.FindControl("txtCrewIdAdd");
            if (txtCrewIdAdd != null) txtCrewIdAdd.Attributes.Add("style", "visibility:hidden");

            LinkButton imgShowHazardAdd = (LinkButton)e.Item.FindControl("imgShowHazardAdd");
            if (imgShowHazardAdd != null)
            {
                imgShowHazardAdd.Attributes.Add("onclick", "return showPickList('spnHazardAdd', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListJHA.aspx?&vesselid=" + ucVessel.SelectedVessel + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, imgShowHazardAdd.CommandName)) imgShowHazardAdd.Visible = false;

            }
            LinkButton imgShowRAAdd = (LinkButton)e.Item.FindControl("imgShowRAAdd");
            if (imgShowRAAdd != null)
            {
                imgShowRAAdd.Attributes.Add("onclick", "return showPickList('spnRAAdd', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListDailyWorkPlanRA.aspx?&vesselid=" + ucVessel.SelectedVessel + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, imgShowRAAdd.CommandName)) imgShowRAAdd.Visible = false;

            }
            LinkButton imgShowCrewInChargeAdd = (LinkButton)e.Item.FindControl("imgShowCrewInChargeAdd");
            if (imgShowCrewInChargeAdd != null)
            {
                imgShowCrewInChargeAdd.Attributes.Add("onclick", "return showPickList('spnCrewInChargeAdd', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListVesselEmployeeOnboard.aspx?VesselId=" + ucVessel.SelectedVessel + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, imgShowCrewInChargeAdd.CommandName)) imgShowCrewInChargeAdd.Visible = false;

            }
            UserControlWorkPermitByCompany ucworkpermit = (UserControlWorkPermitByCompany)e.Item.FindControl("ucWorkPermitAdd");
            if (ucworkpermit != null)
            {
                ucworkpermit.Company = ViewState["COMPANYID"].ToString();
                ucworkpermit.WorkPermitList = PhoenixInspectionDailyWorkPlanActivity.WorkPermitListByCompany(3, General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
                ucworkpermit.DataBind();

            }
        }
    }

    protected void gvDeck_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDeck();
    }

    protected void gvDeck_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            Guid? workplanactivityid = null;
            foreach (GridDataItem row in gvDeck.Items)
            {
                RadioButton rdbtn = (RadioButton)row.FindControl("rdbUser");
                if (rdbtn != null)
                {
                    if (rdbtn.Checked)
                    {
                        workplanactivityid = General.GetNullableGuid(((RadLabel)row.FindControl("lblWorkPlanActivityId")).Text);
                    }

                }
            }

            //if (e.CommandName.ToUpper().Equals("EDIT"))
            //{
            //    if (_gridView.EditIndex > -1)
            //        _gridView.UpdateRow(_gridView.EditIndex, false);
            //    _gridView.EditIndex = nCurrentRow;
            //    //_gridView.SelectedIndex = de.NewEditIndex;

            //    BindDeck();
            //    //SetPageNavigatorDeck();
            //}

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (IsValidDailyWorkPlanActivity(((UserControlMaskedTextBox)e.Item.FindControl("ucSNoEdit")).Text
                    , ((RadTextBox)e.Item.FindControl("txtActivityEdit")).Text
                    , workplanactivityid.ToString()
                    ))
                {
                    string timeofstart = (((UserControlMaskedTextBox)e.Item.FindControl("txtStartTimeEdit")).TextWithLiterals);
                    timeofstart = (timeofstart.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : timeofstart;

                    string timeofend = (((UserControlMaskedTextBox)e.Item.FindControl("txtEndTimeEdit")).TextWithLiterals);
                    timeofend = (timeofend.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : timeofend;

                    PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanActivityUpdate(
                                        General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblWorkPlanActivityLineItemId")).Text)
                                        , General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtHazardIdEdit")).Text)
                                        , General.GetNullableGuid(((UserControlWorkPermitByCompany)e.Item.FindControl("ucWorkPermitEdit")).SelectedWorkPermit)
                                        , General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtRAIdEdit")).Text)
                                        , General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtRaTypeEdit")).Text)
                                        , General.GetNullableInteger(((UserControlMaskedTextBox)e.Item.FindControl("ucSNoEdit")).Text)
                                        , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtActivityEdit")).Text)
                                        , General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtCrewIdEdit")).Text)
                                        , General.GetNullableDateTime(Convert.ToDateTime(ucDate.Text).ToString("dd/MM/yyyy") + " " + timeofstart)
                                        , General.GetNullableDateTime(Convert.ToDateTime(ucDate.Text).ToString("dd/MM/yyyy") + " " + timeofend)
                                        , int.Parse(ucVessel.SelectedVessel)
                                        );
                    // _gridView.EditIndex = -1;
                    BindDeck();
                    gvDeck.Rebind();
                    //SetPageNavigatorDeck();
                }
                else
                {
                    ucError.Visible = true;
                    return;

                }
            }

            else if (e.CommandName.ToUpper().Equals("DECKADD"))
            {
                if (IsValidDailyWorkPlanActivity(((UserControlMaskedTextBox)e.Item.FindControl("ucSNoAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtActivityAdd")).Text
                    , workplanactivityid.ToString()
                    ))
                {
                    if (ViewState["WORKPLANID"] != null && ViewState["WORKPLANID"].ToString() != "")
                    {
                        string timeofstart = (((UserControlMaskedTextBox)e.Item.FindControl("txtStartTimeAdd")).TextWithLiterals);
                        timeofstart = (timeofstart.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : timeofstart;

                        string timeofend = (((UserControlMaskedTextBox)e.Item.FindControl("txtEndTimeAdd")).TextWithLiterals);
                        timeofend = (timeofend.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : timeofend;
                        PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanActivityInsert(
                                            General.GetNullableGuid(ViewState["WORKPLANID"].ToString())
                                            , workplanactivityid
                                            , int.Parse(ucVessel.SelectedVessel)
                                            , General.GetNullableInteger(((UserControlMaskedTextBox)e.Item.FindControl("ucSNoAdd")).Text)
                                            , 2
                                            , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtActivityAdd")).Text)
                                            , General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtHazardIdAdd")).Text)
                                            , General.GetNullableGuid(((UserControlWorkPermitByCompany)e.Item.FindControl("ucWorkPermitAdd")).SelectedWorkPermit)
                                            , General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtRAIdAdd")).Text)
                                            , General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtRaTypeAdd")).Text)
                                            , General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtCrewIdAdd")).Text)
                                            , General.GetNullableDateTime(Convert.ToDateTime(ucDate.Text).ToString("dd/MM/yyyy") + " " + timeofstart)
                                            , General.GetNullableDateTime(Convert.ToDateTime(ucDate.Text).ToString("dd/MM/yyyy") + " " + timeofend)
                                            );
                        BindDeck();
                        gvDeck.Rebind();
                        //SetPageNavigatorDeck();
                    }
                    else
                    {
                        ucError.ErrorMessage = "Save befor adding Activity.";
                        ucError.Visible = true;
                    }
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
            else if (e.CommandName.ToUpper().Equals("DECKDELETE"))
            {
                PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanActivityDelete(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblWorkPlanActivityLineItemId")).Text));
                BindDeck();
                //SetPageNavigatorDeck();
            }

            else if (e.CommandName.ToUpper().Equals("DECKCLEAR"))
            {
                foreach (GridDataItem row in gvDeck.Items)
                {
                    RadioButton rdbtn = (RadioButton)row.FindControl("rdbUser");
                    if (rdbtn != null)
                    {
                        if (rdbtn.Checked)
                        {
                            rdbtn.Checked = false;
                        }

                    }
                }
            }


            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvDeck_ItemDataBound(object sender, GridItemEventArgs e)
    {


        if (e.Item is GridDataItem)
        {
            UserControlMaskedTextBox ucSNoEdit = (UserControlMaskedTextBox)e.Item.FindControl("ucSNoEdit");
            if (ucSNoEdit != null) ucSNoEdit.Text = DataBinder.Eval(e.Item.DataItem, "FLDSERIALNUMBER").ToString();

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);


            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }




            LinkButton lnkHazardNumber = (LinkButton)e.Item.FindControl("lnkHazardNumber");
            RadLabel lbljobhazardid = (RadLabel)e.Item.FindControl("lblHazardId");
            RadLabel lblHazardNumber = (RadLabel)e.Item.FindControl("lblHazardNumber");

            if (lblHazardNumber != null && lblHazardNumber.Text.ToUpper().Equals("N/A"))
            {
                lblHazardNumber.Visible = true;
                lnkHazardNumber.Visible = false;
            }

            LinkButton lnkRA = (LinkButton)e.Item.FindControl("lnkRA");
            RadLabel lnkRaId = (RadLabel)e.Item.FindControl("lblRAId");
            RadLabel lblRA = (RadLabel)e.Item.FindControl("lblRA");
            LinkButton JHAList = (LinkButton)e.Item.FindControl("imgDeckJHAList");
            RadLabel lblImportedJHA = (RadLabel)e.Item.FindControl("lblDeckJHAList");

            if (lblRA != null && lblRA.Text.ToUpper().Equals("N/A"))
            {
                lblRA.Visible = true;
                lnkRA.Visible = false;
                if (JHAList != null)
                {
                    JHAList.Visible = false;
                }
            }
            // Check that the RA has imported JHA

            if (lblImportedJHA == null || lblImportedJHA.Text == "")
            {
                if (JHAList != null)
                {
                    JHAList.Visible = false;
                }
            }
            if (JHAList != null)
            {
                JHAList.Attributes.Add("onclick", "javascript:parent.openNewWindow('JobHazard','','" + Session["sitepath"] + "/Inspection/InspectionRAProcessImportedJHAList.aspx?RAPROCESSID=" + lnkRaId.Text + "');return false;");
            }
            if (lnkHazardNumber != null && lbljobhazardid.Text != null)
                lnkHazardNumber.Attributes.Add("onclick", "javascript:parent.openNewWindow('JobHazard','','" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=9&reportcode=JOBHAZARD&showmenu=0&showword=NO&showexcel=NO&jobhazardid=" + lbljobhazardid.Text + "'); return false;");

            if (DataBinder.Eval(e.Item.DataItem, "FLDRISKASSESSMENTTYPE").ToString() == "1")
            {
                if (lnkRA != null)
                    lnkRA.Attributes.Add("onclick", "javascript:parent.openNewWindow('JobHazard','','" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=9&reportcode=RAPROCESS&showmenu=0&showword=NO&showexcel=NO&processid=" + lnkRaId.Text + "'); return false;");
            }
            else if (DataBinder.Eval(e.Item.DataItem, "FLDRISKASSESSMENTTYPE").ToString() == "2")
            {
                if (lnkRA != null)
                    lnkRA.Attributes.Add("onclick", "openNewWindow('RAGeneric', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAGENERIC&genericid=" + lnkRaId.Text + "&showmenu=0&showexcel=NO');return false;");
            }
            else if (DataBinder.Eval(e.Item.DataItem, "FLDRISKASSESSMENTTYPE").ToString() == "3")
            {
                if (lnkRA != null)
                    lnkRA.Attributes.Add("onclick", "openNewWindow('RAMachinery', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + lnkRaId.Text + "&showmenu=0&showexcel=NO');return false;");
            }
            else if (DataBinder.Eval(e.Item.DataItem, "FLDRISKASSESSMENTTYPE").ToString() == "4")
            {
                if (lnkRA != null)
                    lnkRA.Attributes.Add("onclick", "openNewWindow('RANavigation', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RANAVIGATION&navigationid=" + lnkRaId.Text + "&showmenu=0&showexcel=NO');return false;");
            }

            else if (DataBinder.Eval(e.Item.DataItem, "FLDRISKASSESSMENTTYPE").ToString() == "5")
            {
                if (lnkRA != null)
                    lnkRA.Attributes.Add("onclick", "openNewWindow('RAcargo', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RACARGO&genericid=" + lnkRaId.Text + "&showmenu=0&showexcel=NO');return false;");
            }

            HyperLink lnkWorkPermit = (HyperLink)e.Item.FindControl("lnkWorkPermit");
            RadLabel lblWorkPermit = (RadLabel)e.Item.FindControl("lblWorkPermit");

            if (lblWorkPermit != null && lblWorkPermit.Text.ToUpper().Equals("N/A"))
            {
                lblWorkPermit.Visible = true;
                lnkWorkPermit.Visible = false;
            }

            if (!string.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "FLDDTKEY").ToString()))
            {
                if (!string.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "FLDDTKEY").ToString()))
                {
                    DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(DataBinder.Eval(e.Item.DataItem, "FLDDTKEY").ToString()));
                    if (dt.Rows.Count > 0)
                    {
                        DataRow drRow = dt.Rows[0];
                        if (lnkWorkPermit != null)
                            lnkWorkPermit.NavigateUrl = "../Common/download.aspx?dtkey=" + drRow["FLDDTKEY"].ToString();
                    }
                }
            }
            HyperLink lnkworkpermit = (HyperLink)e.Item.FindControl("lnkWorkPermit");
            //if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            if (lnkworkpermit != null)
            {

                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipWorkPermit");
                lnkworkpermit.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lnkworkpermit.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }

            RadLabel lblIncharge = (RadLabel)e.Item.FindControl("lblIncharge");
            if (lblIncharge != null)
            {

                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipPICName");
                lblIncharge.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lblIncharge.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
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

        if (e.Item is GridDataItem)
        {
            RadTextBox txtHazardIdEdit = (RadTextBox)e.Item.FindControl("txtHazardIdEdit");
            if (txtHazardIdEdit != null) txtHazardIdEdit.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtRAIdEdit = (RadTextBox)e.Item.FindControl("txtRAIdEdit");
            if (txtRAIdEdit != null) txtRAIdEdit.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtRaTypeEdit = (RadTextBox)e.Item.FindControl("txtRaTypeEdit");
            if (txtRaTypeEdit != null) txtRaTypeEdit.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtCrewIdEdit = (RadTextBox)e.Item.FindControl("txtCrewIdEdit");
            if (txtCrewIdEdit != null) txtCrewIdEdit.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtActivityEdit = (RadTextBox)e.Item.FindControl("txtActivityEdit");

            LinkButton imgShowHazardEdit = (LinkButton)e.Item.FindControl("imgShowHazardEdit");
            if (imgShowHazardEdit != null)
            {
                imgShowHazardEdit.Attributes.Add("onclick", "return showPickList('spnHazardEditSection', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListJHA.aspx?&vesselid=" + ucVessel.SelectedVessel + "&activity=" + txtActivityEdit.Text + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, imgShowHazardEdit.CommandName)) imgShowHazardEdit.Visible = false;

            }
            LinkButton imgShowRAEdit = (LinkButton)e.Item.FindControl("imgShowRAEdit");
            if (imgShowRAEdit != null)
            {
                imgShowRAEdit.Attributes.Add("onclick", "return showPickList('spnRAEditSection', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListDailyWorkPlanRA.aspx?&vesselid=" + ucVessel.SelectedVessel + "&activity=" + txtActivityEdit.Text + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, imgShowRAEdit.CommandName)) imgShowRAEdit.Visible = false;

            }

            LinkButton imgShowCrewInChargeEdit = (LinkButton)e.Item.FindControl("imgShowCrewInChargeEdit");
            if (imgShowCrewInChargeEdit != null)
            {
                imgShowCrewInChargeEdit.Attributes.Add("onclick", "return showPickList('spnCrewInChargeEditSection', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListVesselEmployeeOnboard.aspx?VesselId=" + ucVessel.SelectedVessel + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, imgShowCrewInChargeEdit.CommandName)) imgShowCrewInChargeEdit.Visible = false;

            }


            UserControlWorkPermitByCompany ucworkpermit = (UserControlWorkPermitByCompany)e.Item.FindControl("ucWorkPermitEdit");
            if (ucworkpermit != null)
            {
                DataSet ds = PhoenixInspectionDailyWorkPlanActivity.WorkPermitListByCompany(3, General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
                ucworkpermit.WorkPermitList = ds;
                ucworkpermit.DataBind();
                ucworkpermit.SelectedWorkPermit = DataBinder.Eval(e.Item.DataItem, "FLDWORKPERMITID").ToString();
            }
            UserControlMaskedTextBox txtStartTimeEdit = (UserControlMaskedTextBox)e.Item.FindControl("txtStartTimeEdit");
            if (txtStartTimeEdit != null)
                txtStartTimeEdit.TextWithLiterals = String.Format("{0:dd/MM/yyyy hh:mm tt}", DataBinder.Eval(e.Item.DataItem, "FLDSTARTDATETIME").ToString());

            UserControlMaskedTextBox txtEndTimeEdit = (UserControlMaskedTextBox)e.Item.FindControl("txtEndTimeEdit");
            if (txtEndTimeEdit != null)
                txtEndTimeEdit.TextWithLiterals = String.Format("{0:dd/MM/yyyy hh:mm tt}", DataBinder.Eval(e.Item.DataItem, "FLDENDDATETIME").ToString());
        }

        if (e.Item is GridFooterItem)
        {
            RadTextBox txtHazardIdAdd = (RadTextBox)e.Item.FindControl("txtHazardIdAdd");
            if (txtHazardIdAdd != null) txtHazardIdAdd.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtRAIdAdd = (RadTextBox)e.Item.FindControl("txtRAIdAdd");
            if (txtRAIdAdd != null) txtRAIdAdd.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtRaTypeAdd = (RadTextBox)e.Item.FindControl("txtRaTypeAdd");
            if (txtRaTypeAdd != null) txtRaTypeAdd.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtCrewIdEdit = (RadTextBox)e.Item.FindControl("txtCrewIdAdd");
            if (txtCrewIdEdit != null) txtCrewIdEdit.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtActivityAdd = (RadTextBox)e.Item.FindControl("txtActivityAdd");

            LinkButton imgShowHazardAdd = (LinkButton)e.Item.FindControl("imgShowHazardAdd");
            if (imgShowHazardAdd != null)
            {
                imgShowHazardAdd.Attributes.Add("onclick", "return showPickList('spnHazardAddSection', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListJHA.aspx?&vesselid=" + ucVessel.SelectedVessel + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, imgShowHazardAdd.CommandName)) imgShowHazardAdd.Visible = false;

            }
            LinkButton imgShowRAAdd = (LinkButton)e.Item.FindControl("imgShowRAAdd");
            if (imgShowRAAdd != null)
            {
                imgShowRAAdd.Attributes.Add("onclick", "return showPickList('spnRAAddSection', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListDailyWorkPlanRA.aspx?&vesselid=" + ucVessel.SelectedVessel + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, imgShowRAAdd.CommandName)) imgShowRAAdd.Visible = false;

            }

            LinkButton imgShowCrewInChargeAdd = (LinkButton)e.Item.FindControl("imgShowCrewInChargeAdd");
            if (imgShowCrewInChargeAdd != null)
            {
                imgShowCrewInChargeAdd.Attributes.Add("onclick", "return showPickList('spnCrewInChargeAddSection', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListVesselEmployeeOnboard.aspx?VesselId=" + ucVessel.SelectedVessel + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, imgShowCrewInChargeAdd.CommandName)) imgShowCrewInChargeAdd.Visible = false;

            }

            UserControlWorkPermitByCompany ucworkpermit = (UserControlWorkPermitByCompany)e.Item.FindControl("ucWorkPermitAdd");
            if (ucworkpermit != null)
            {
                ucworkpermit.Company = ViewState["COMPANYID"].ToString();
                ucworkpermit.WorkPermitList = PhoenixInspectionDailyWorkPlanActivity.WorkPermitListByCompany(3, General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
                ucworkpermit.DataBind();
            }
        }
    }

    protected void gvEngine_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindEngine();
    }

    protected void gvEngine_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);


            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }




            LinkButton lnkHazardNumber = (LinkButton)e.Item.FindControl("lnkHazardNumber");
            RadLabel lbljobhazardid = (RadLabel)e.Item.FindControl("lblHazardId");
            RadLabel lblHazardNumber = (RadLabel)e.Item.FindControl("lblHazardNumber");

            if (lblHazardNumber != null && lblHazardNumber.Text.ToUpper().Equals("N/A"))
            {
                lblHazardNumber.Visible = true;
                lnkHazardNumber.Visible = false;
            }
            LinkButton lnkRA = (LinkButton)e.Item.FindControl("lnkRA");
            RadLabel lnkRaId = (RadLabel)e.Item.FindControl("lblRAId");
            RadLabel lblRA = (RadLabel)e.Item.FindControl("lblRA");
            LinkButton JHAList = (LinkButton)e.Item.FindControl("imgEngineJHAList");
            RadLabel lblImportedJHA = (RadLabel)e.Item.FindControl("lblEngineJHAList");

            if (lblRA != null && lblRA.Text.ToUpper().Equals("N/A"))
            {
                lblRA.Visible = true;
                lnkRA.Visible = false;
                if (JHAList != null)
                {
                    JHAList.Visible = false;
                }
            }
            // Check that the RA has imported JHA
            if (lblImportedJHA == null || lblImportedJHA.Text == "")
            {
                if (JHAList != null)
                {
                    JHAList.Visible = false;
                }
            }

            if (JHAList != null)
            {
                JHAList.Attributes.Add("onclick", "javascript:parent.openNewWindow('JobHazard','','" + Session["sitepath"] + "/Inspection/InspectionRAProcessImportedJHAList.aspx?RAPROCESSID=" + lnkRaId.Text + "');return false;");
            }

            if (lnkHazardNumber != null && lbljobhazardid.Text != null)
                lnkHazardNumber.Attributes.Add("onclick", "javascript:parent.openNewWindow('JobHazard','','" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=9&reportcode=JOBHAZARD&showmenu=0&showword=NO&showexcel=NO&jobhazardid=" + lbljobhazardid.Text + "'); return false;");

            if (DataBinder.Eval(e.Item.DataItem, "FLDRISKASSESSMENTTYPE").ToString() == "1")
            {
                if (lnkRA != null)
                    lnkRA.Attributes.Add("onclick", "javascript:parent.openNewWindow('JobHazard','','" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=9&reportcode=RAPROCESS&showmenu=0&showword=NO&showexcel=NO&processid=" + lnkRaId.Text + "'); return false;");
            }
            else if (DataBinder.Eval(e.Item.DataItem, "FLDRISKASSESSMENTTYPE").ToString() == "2")
            {
                if (lnkRA != null)
                    lnkRA.Attributes.Add("onclick", "openNewWindow('RAGeneric', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAGENERIC&genericid=" + lnkRaId.Text + "&showmenu=0&showexcel=NO');return false;");
            }
            else if (DataBinder.Eval(e.Item.DataItem, "FLDRISKASSESSMENTTYPE").ToString() == "3")
            {
                if (lnkRA != null)
                    lnkRA.Attributes.Add("onclick", "openNewWindow('RAMachinery', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + lnkRaId.Text + "&showmenu=0&showexcel=NO');return false;");
            }
            else if (DataBinder.Eval(e.Item.DataItem, "FLDRISKASSESSMENTTYPE").ToString() == "4")
            {
                if (lnkRA != null)
                    lnkRA.Attributes.Add("onclick", "openNewWindow('RANavigation', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RANAVIGATION&navigationid=" + lnkRaId.Text + "&showmenu=0&showexcel=NO');return false;");
            }
            else if (DataBinder.Eval(e.Item.DataItem, "FLDRISKASSESSMENTTYPE").ToString() == "5")
            {
                if (lnkRA != null)
                    lnkRA.Attributes.Add("onclick", "openNewWindow('RAcargo', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RACARGO&genericid=" + lnkRaId.Text + "&showmenu=0&showexcel=NO');return false;");
            }

            HyperLink lnkWorkPermit = (HyperLink)e.Item.FindControl("lnkWorkPermit");
            if (!string.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "FLDDTKEY").ToString()))
            {
                DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(DataBinder.Eval(e.Item.DataItem, "FLDDTKEY").ToString()));
                if (dt.Rows.Count > 0)
                {
                    DataRow drRow = dt.Rows[0];
                    if (lnkWorkPermit != null)
                        lnkWorkPermit.NavigateUrl = "../Common/download.aspx?dtkey=" + drRow["FLDDTKEY"].ToString();
                }
            }

            HyperLink lnkworkpermit = (HyperLink)e.Item.FindControl("lnkWorkPermit");
            RadLabel lblWorkPermit = (RadLabel)e.Item.FindControl("lblWorkPermit");

            if (lblWorkPermit != null && lblWorkPermit.Text.ToUpper().Equals("N/A"))
            {
                lblWorkPermit.Visible = true;
                lnkWorkPermit.Visible = false;
            }
            //if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            if (lnkworkpermit != null)
            {

                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipWorkPermit");
                lnkworkpermit.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lnkworkpermit.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }

            RadLabel lblIncharge = (RadLabel)e.Item.FindControl("lblIncharge");
            if (lblIncharge != null)
            {

                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipPICName");
                lblIncharge.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lblIncharge.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
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

        if (e.Item is GridDataItem)
        {
            RadTextBox txtHazardIdEdit = (RadTextBox)e.Item.FindControl("txtHazardIdEdit");
            if (txtHazardIdEdit != null) txtHazardIdEdit.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtRAIdEdit = (RadTextBox)e.Item.FindControl("txtRAIdEdit");
            if (txtRAIdEdit != null) txtRAIdEdit.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtRaTypeEdit = (RadTextBox)e.Item.FindControl("txtRaTypeEdit");
            if (txtRaTypeEdit != null) txtRaTypeEdit.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtCrewIdEdit = (RadTextBox)e.Item.FindControl("txtCrewIdEdit");
            if (txtCrewIdEdit != null) txtCrewIdEdit.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtActivityEdit = (RadTextBox)e.Item.FindControl("txtActivityEdit");

            LinkButton imgShowHazardEdit = (LinkButton)e.Item.FindControl("imgShowHazardEdit");
            if (imgShowHazardEdit != null)
            {
                imgShowHazardEdit.Attributes.Add("onclick", "return showPickList('spnHazardEditForm', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListJHA.aspx?&vesselid=" + ucVessel.SelectedVessel + "&activity=" + txtActivityEdit.Text + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, imgShowHazardEdit.CommandName)) imgShowHazardEdit.Visible = false;

            }
            LinkButton imgShowRAEdit = (LinkButton)e.Item.FindControl("imgShowRAEdit");
            if (imgShowRAEdit != null)
            {
                imgShowRAEdit.Attributes.Add("onclick", "return showPickList('spnRAEditForm', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListDailyWorkPlanRA.aspx?&vesselid=" + ucVessel.SelectedVessel + "&activity=" + txtActivityEdit.Text + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, imgShowRAEdit.CommandName)) imgShowRAEdit.Visible = false;

            }

            LinkButton imgShowCrewInChargeEdit = (LinkButton)e.Item.FindControl("imgShowCrewInChargeEdit");
            if (imgShowCrewInChargeEdit != null)
            {
                imgShowCrewInChargeEdit.Attributes.Add("onclick", "return showPickList('spnCrewInChargeEditForm', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListVesselEmployeeOnboard.aspx?VesselId=" + ucVessel.SelectedVessel + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, imgShowCrewInChargeEdit.CommandName)) imgShowCrewInChargeEdit.Visible = false;

            }


            UserControlWorkPermitByCompany ucworkpermit = (UserControlWorkPermitByCompany)e.Item.FindControl("ucWorkPermitEdit");
            if (ucworkpermit != null)
            {
                DataSet ds = PhoenixInspectionDailyWorkPlanActivity.WorkPermitListByCompany(3, General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
                ucworkpermit.WorkPermitList = ds;
                ucworkpermit.DataBind();
                ucworkpermit.SelectedWorkPermit = DataBinder.Eval(e.Item.DataItem, "FLDWORKPERMITID").ToString();
            }
            UserControlMaskedTextBox txtStartTimeEdit = (UserControlMaskedTextBox)e.Item.FindControl("txtStartTimeEdit");
            if (txtStartTimeEdit != null)
                txtStartTimeEdit.TextWithLiterals = String.Format("{0:dd/MM/yyyy hh:mm tt}", DataBinder.Eval(e.Item.DataItem, "FLDSTARTDATETIME").ToString());

            UserControlMaskedTextBox txtEndTimeEdit = (UserControlMaskedTextBox)e.Item.FindControl("txtEndTimeEdit");
            if (txtEndTimeEdit != null)
                txtEndTimeEdit.TextWithLiterals = String.Format("{0:dd/MM/yyyy hh:mm tt}", DataBinder.Eval(e.Item.DataItem, "FLDENDDATETIME").ToString());
        }

        if (e.Item is GridFooterItem)
        {
            RadTextBox txtHazardIdAdd = (RadTextBox)e.Item.FindControl("txtHazardIdAdd");
            if (txtHazardIdAdd != null) txtHazardIdAdd.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtRAIdAdd = (RadTextBox)e.Item.FindControl("txtRAIdAdd");
            if (txtRAIdAdd != null) txtRAIdAdd.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtRaTypeAdd = (RadTextBox)e.Item.FindControl("txtRaTypeAdd");
            if (txtRaTypeAdd != null) txtRaTypeAdd.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtCrewIdEdit = (RadTextBox)e.Item.FindControl("txtCrewIdAdd");
            if (txtCrewIdEdit != null) txtCrewIdEdit.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtActivityAdd = (RadTextBox)e.Item.FindControl("txtActivityAdd");

            LinkButton imgShowHazardAdd = (LinkButton)e.Item.FindControl("imgShowHazardAdd");
            if (imgShowHazardAdd != null)
            {
                imgShowHazardAdd.Attributes.Add("onclick", "return showPickList('spnHazardAddForm', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListJHA.aspx?&vesselid=" + ucVessel.SelectedVessel + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, imgShowHazardAdd.CommandName)) imgShowHazardAdd.Visible = false;

            }
            LinkButton imgShowRAAdd = (LinkButton)e.Item.FindControl("imgShowRAAdd");
            if (imgShowRAAdd != null)
            {
                imgShowRAAdd.Attributes.Add("onclick", "return showPickList('spnRAAddForm', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListDailyWorkPlanRA.aspx?&vesselid=" + ucVessel.SelectedVessel + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, imgShowRAAdd.CommandName)) imgShowRAAdd.Visible = false;

            }

            LinkButton imgShowCrewInChargeAdd = (LinkButton)e.Item.FindControl("imgShowCrewInChargeAdd");
            if (imgShowCrewInChargeAdd != null)
            {
                imgShowCrewInChargeAdd.Attributes.Add("onclick", "return showPickList('spnCrewInChargeAddForm', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListVesselEmployeeOnboard.aspx?VesselId=" + ucVessel.SelectedVessel + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, imgShowCrewInChargeAdd.CommandName)) imgShowCrewInChargeAdd.Visible = false;

            }

            UserControlWorkPermitByCompany ucworkpermit = (UserControlWorkPermitByCompany)e.Item.FindControl("ucWorkPermitAdd");
            if (ucworkpermit != null)
            {
                ucworkpermit.Company = ViewState["COMPANYID"].ToString();
                ucworkpermit.WorkPermitList = PhoenixInspectionDailyWorkPlanActivity.WorkPermitListByCompany(3, General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
                ucworkpermit.DataBind();

            }
        }
    }

    protected void gvEngine_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            Guid? workplanactivityid = null;
            foreach (GridDataItem row in gvEngine.Items)
            {
                RadioButton rdbtn = (RadioButton)row.FindControl("rdbUser");
                if (rdbtn != null)
                {
                    if (rdbtn.Checked)
                    {
                        workplanactivityid = General.GetNullableGuid(((RadLabel)row.FindControl("lblWorkPlanActivityId")).Text);
                    }

                }
            }



            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (IsValidDailyWorkPlanActivity(((UserControlMaskedTextBox)e.Item.FindControl("ucSNoEdit")).Text
                    , ((RadTextBox)e.Item.FindControl("txtActivityEdit")).Text
                    , workplanactivityid.ToString()
                    ))
                {
                    string timeofstart = (((UserControlMaskedTextBox)e.Item.FindControl("txtStartTimeEdit")).TextWithLiterals);
                    timeofstart = (timeofstart.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : timeofstart;

                    string timeofend = (((UserControlMaskedTextBox)e.Item.FindControl("txtEndTimeEdit")).TextWithLiterals);
                    timeofend = (timeofend.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : timeofend;

                    PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanActivityUpdate(
                                        General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblWorkPlanActivityLineItemId")).Text)
                                        , General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtHazardIdEdit")).Text)
                                        , General.GetNullableGuid(((UserControlWorkPermitByCompany)e.Item.FindControl("ucWorkPermitEdit")).SelectedWorkPermit)
                                        , General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtRAIdEdit")).Text)
                                        , General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtRaTypeEdit")).Text)
                                        , General.GetNullableInteger(((UserControlMaskedTextBox)e.Item.FindControl("ucSNoEdit")).Text)
                                        , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtActivityEdit")).Text)
                                        , General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtCrewIdEdit")).Text)
                                        , General.GetNullableDateTime(Convert.ToDateTime(ucDate.Text).ToString("dd/MM/yyyy") + " " + timeofstart)
                                        , General.GetNullableDateTime(Convert.ToDateTime(ucDate.Text).ToString("dd/MM/yyyy") + " " + timeofend)
                                        , int.Parse(ucVessel.SelectedVessel)
                                        );
                    //  _gridView.EditIndex = -1;
                    BindEngine();
                    gvEngine.Rebind();
                    //SetPageNavigatorEngine();
                }
                else
                {
                    ucError.Visible = true;
                    return;

                }
            }
            else if (e.CommandName.ToUpper().Equals("ENGINEADD"))
            {
                if (IsValidDailyWorkPlanActivity(((UserControlMaskedTextBox)e.Item.FindControl("ucSNoAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtActivityAdd")).Text
                    , workplanactivityid.ToString()
                    ))
                {
                    if (ViewState["WORKPLANID"] != null && ViewState["WORKPLANID"].ToString() != "")
                    {
                        string timeofstart = (((UserControlMaskedTextBox)e.Item.FindControl("txtStartTimeAdd")).TextWithLiterals);
                        timeofstart = (timeofstart.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : timeofstart;

                        string timeofend = (((UserControlMaskedTextBox)e.Item.FindControl("txtEndTimeAdd")).TextWithLiterals);
                        timeofend = (timeofend.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : timeofend;
                        PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanActivityInsert(
                                            General.GetNullableGuid(ViewState["WORKPLANID"].ToString())
                                            , workplanactivityid
                                            , int.Parse(ucVessel.SelectedVessel)
                                            , General.GetNullableInteger(((UserControlMaskedTextBox)e.Item.FindControl("ucSNoAdd")).Text)
                                            , 3
                                            , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtActivityAdd")).Text)
                                            , General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtHazardIdAdd")).Text)
                                            , General.GetNullableGuid(((UserControlWorkPermitByCompany)e.Item.FindControl("ucWorkPermitAdd")).SelectedWorkPermit)
                                            , General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtRAIdAdd")).Text)
                                            , General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtRaTypeAdd")).Text)
                                            , General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtCrewIdAdd")).Text)
                                            , General.GetNullableDateTime(Convert.ToDateTime(ucDate.Text).ToString("dd/MM/yyyy") + " " + timeofstart)
                                            , General.GetNullableDateTime(Convert.ToDateTime(ucDate.Text).ToString("dd/MM/yyyy") + " " + timeofend)
                                            );
                        BindEngine();
                        gvEngine.Rebind();
                        //SetPageNavigatorEngine();
                    }
                    else
                    {
                        ucError.ErrorMessage = "Save befor adding Activity.";
                        ucError.Visible = true;
                    }
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }

            else if (e.CommandName.ToUpper().Equals("ENGINEDELETE"))
            {
                PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanActivityDelete(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblWorkPlanActivityLineItemId")).Text));
                BindEngine();
                //SetPageNavigatorEngine();
            }

            else if (e.CommandName.ToUpper().Equals("ENGINECLEAR"))
            {
                foreach (GridDataItem row in gvEngine.Items)
                {
                    RadioButton rdbtn = (RadioButton)row.FindControl("rdbUser");
                    if (rdbtn != null)
                    {
                        if (rdbtn.Checked)
                        {
                            rdbtn.Checked = false;
                        }

                    }
                }
            }

            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Please make the required correction";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvCatering_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindCatering();
    }

    protected void gvCatering_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            Guid? workplanactivityid = null;
            foreach (GridDataItem row in gvCatering.Items)
            {
                RadioButton rdbtn = (RadioButton)row.FindControl("rdbUser");
                if (rdbtn != null)
                {
                    if (rdbtn.Checked)
                    {
                        workplanactivityid = General.GetNullableGuid(((RadLabel)row.FindControl("lblWorkPlanActivityId")).Text);
                    }

                }
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (IsValidDailyWorkPlanActivity(((UserControlMaskedTextBox)e.Item.FindControl("ucSNoEdit")).Text
                    , ((RadTextBox)e.Item.FindControl("txtActivityEdit")).Text
                    , workplanactivityid.ToString()
                    ))
                {
                    string timeofstart = (((UserControlMaskedTextBox)e.Item.FindControl("txtStartTimeEdit")).TextWithLiterals);
                    timeofstart = (timeofstart.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : timeofstart;

                    string timeofend = (((UserControlMaskedTextBox)e.Item.FindControl("txtEndTimeEdit")).TextWithLiterals);
                    timeofend = (timeofend.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : timeofend;

                    PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanActivityUpdate(
                                        General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblWorkPlanActivityLineItemId")).Text)
                                        , General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtHazardIdEdit")).Text)
                                        , General.GetNullableGuid(((UserControlWorkPermitByCompany)e.Item.FindControl("ucWorkPermitEdit")).SelectedWorkPermit)
                                        , General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtRAIdEdit")).Text)
                                        , General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtRaTypeEdit")).Text)
                                        , General.GetNullableInteger(((UserControlMaskedTextBox)e.Item.FindControl("ucSNoEdit")).Text)
                                        , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtActivityEdit")).Text)
                                        , General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtCrewIdEdit")).Text)
                                        , General.GetNullableDateTime(Convert.ToDateTime(ucDate.Text).ToString("dd/MM/yyyy") + " " + timeofstart)
                                        , General.GetNullableDateTime(Convert.ToDateTime(ucDate.Text).ToString("dd/MM/yyyy") + " " + timeofend)
                                        , int.Parse(ucVessel.SelectedVessel)
                                        );

                    BindCatering();
                    gvCatering.Rebind();
                    //SetPageNavigatorCatering();
                }
                else
                {
                    ucError.Visible = true;
                    return;

                }
            }

            else if (e.CommandName.ToUpper().Equals("CATERINGADD"))
            {
                if (IsValidDailyWorkPlanActivity(((UserControlMaskedTextBox)e.Item.FindControl("ucSNoAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtActivityAdd")).Text
                    , workplanactivityid.ToString()
                    ))
                {
                    if (ViewState["WORKPLANID"] != null && ViewState["WORKPLANID"].ToString() != "")
                    {
                        string timeofstart = (((UserControlMaskedTextBox)e.Item.FindControl("txtStartTimeAdd")).TextWithLiterals);
                        timeofstart = (timeofstart.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : timeofstart;

                        string timeofend = (((UserControlMaskedTextBox)e.Item.FindControl("txtEndTimeAdd")).TextWithLiterals);
                        timeofend = (timeofend.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : timeofend;
                        PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanActivityInsert(
                                            General.GetNullableGuid(ViewState["WORKPLANID"].ToString())
                                            , workplanactivityid
                                            , int.Parse(ucVessel.SelectedVessel)
                                            , General.GetNullableInteger(((UserControlMaskedTextBox)e.Item.FindControl("ucSNoAdd")).Text)
                                            , 4
                                            , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtActivityAdd")).Text)
                                            , General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtHazardIdAdd")).Text)
                                            , General.GetNullableGuid(((UserControlWorkPermitByCompany)e.Item.FindControl("ucWorkPermitAdd")).SelectedWorkPermit)
                                            , General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtRAIdAdd")).Text)
                                            , General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtRaTypeAdd")).Text)
                                            , General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtCrewIdAdd")).Text)
                                              , General.GetNullableDateTime(Convert.ToDateTime(ucDate.Text).ToString("dd/MM/yyyy") + " " + timeofstart)
                                            , General.GetNullableDateTime(Convert.ToDateTime(ucDate.Text).ToString("dd/MM/yyyy") + " " + timeofend)
                                            );
                        BindCatering();
                        gvCatering.Rebind();
                        //SetPageNavigatorCatering();
                    }
                    else
                    {
                        ucError.ErrorMessage = "Save befor adding Activity.";
                        ucError.Visible = true;
                    }
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
            else if (e.CommandName.ToUpper().Equals("CATERINGDELETE"))
            {
                PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanActivityDelete(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblWorkPlanActivityLineItemId")).Text));
                BindCatering();
                gvCatering.Rebind();
                //SetPageNavigatorCatering();
            }

            else if (e.CommandName.ToUpper().Equals("CATERINGCLEAR"))
            {
                foreach (GridDataItem row in gvCatering.Items)
                {
                    RadioButton rdbtn = (RadioButton)row.FindControl("rdbUser");
                    if (rdbtn != null)
                    {
                        if (rdbtn.Checked)
                        {
                            rdbtn.Checked = false;
                        }

                    }
                }
            }


            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Please make the required correction";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvCatering_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);


            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }


         

            LinkButton lnkHazardNumber = (LinkButton)e.Item.FindControl("lnkHazardNumber");
            RadLabel lbljobhazardid = (RadLabel)e.Item.FindControl("lblHazardId");
            RadLabel lblHazardNumber = (RadLabel)e.Item.FindControl("lblHazardNumber");

            if (lblHazardNumber != null && lblHazardNumber.Text.ToUpper().Equals("N/A"))
            {
                lblHazardNumber.Visible = true;
                lnkHazardNumber.Visible = false;
            }

            LinkButton lnkRA = (LinkButton)e.Item.FindControl("lnkRA");
            RadLabel lnkRaId = (RadLabel)e.Item.FindControl("lblRAId");
            RadLabel lblRA = (RadLabel)e.Item.FindControl("lblRA");
            LinkButton JHAList = (LinkButton)e.Item.FindControl("imgCateringJHAList");
            RadLabel lblImportedJHA = (RadLabel)e.Item.FindControl("lblCateringJHAList");

            if (lblRA != null && lblRA.Text.ToUpper().Equals("N/A"))
            {
                lblRA.Visible = true;
                lnkRA.Visible = false;
                if (JHAList != null)
                {
                    JHAList.Visible = false;
                }
            }
            // Check that the RA has imported JHA
            if (lblImportedJHA == null || lblImportedJHA.Text == "")
            {
                if (JHAList != null)
                {
                    JHAList.Visible = false;
                }
            }

            if (JHAList != null)
            {
                JHAList.Attributes.Add("onclick", "javascript:parent.openNewWindow('JobHazard','','"+Session["sitepath"]+"/Inspection/InspectionRAProcessImportedJHAList.aspx?RAPROCESSID=" + lnkRaId.Text + "');return false;");
            }
            if (lnkHazardNumber != null && lbljobhazardid.Text != null)
                lnkHazardNumber.Attributes.Add("onclick", "javascript:parent.openNewWindow('JobHazard','','" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=9&reportcode=JOBHAZARD&showmenu=0&showword=NO&showexcel=NO&jobhazardid=" + lbljobhazardid.Text + "'); return false;");

            if (DataBinder.Eval(e.Item.DataItem, "FLDRISKASSESSMENTTYPE").ToString()  == "1")
            {
                if (lnkRA != null)
                    lnkRA.Attributes.Add("onclick", "javascript:parent.openNewWindow('JobHazard','','" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=9&reportcode=RAPROCESS&showmenu=0&showword=NO&showexcel=NO&processid=" + lnkRaId.Text + "'); return false;");
            }
            else if (DataBinder.Eval(e.Item.DataItem, "FLDRISKASSESSMENTTYPE").ToString() == "2")
            {
                if (lnkRA != null)
                    lnkRA.Attributes.Add("onclick", "openNewWindow('RAGeneric', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAGENERIC&genericid=" + lnkRaId.Text + "&showmenu=0&showexcel=NO');return false;");
            }
            else if (DataBinder.Eval(e.Item.DataItem, "FLDRISKASSESSMENTTYPE").ToString() == "3")
            {
                if (lnkRA != null)
                    lnkRA.Attributes.Add("onclick", "openNewWindow('RAMachinery', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + lnkRaId.Text + "&showmenu=0&showexcel=NO');return false;");
            }
            else if (DataBinder.Eval(e.Item.DataItem, "FLDRISKASSESSMENTTYPE").ToString() == "4")
            {
                if (lnkRA != null)
                    lnkRA.Attributes.Add("onclick", "openNewWindow('RANavigation', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RANAVIGATION&navigationid=" + lnkRaId.Text + "&showmenu=0&showexcel=NO');return false;");
            }

            else if (DataBinder.Eval(e.Item.DataItem, "FLDRISKASSESSMENTTYPE").ToString() == "5")
            {
                if (lnkRA != null)
                    lnkRA.Attributes.Add("onclick", "openNewWindow('RAcargo', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RACARGO&genericid=" + lnkRaId.Text + "&showmenu=0&showexcel=NO');return false;");
            }

            HyperLink lnkWorkPermit = (HyperLink)e.Item.FindControl("lnkWorkPermit");
            RadLabel lblWorkPermit = (RadLabel)e.Item.FindControl("lblWorkPermit");

            if (lblWorkPermit != null && lblWorkPermit.Text.ToUpper().Equals("N/A"))
            {
                lblWorkPermit.Visible = true;
                lnkWorkPermit.Visible = false;
            }
            if (!string.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "FLDDTKEY").ToString()))
            {
                DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(DataBinder.Eval(e.Item.DataItem, "FLDDTKEY").ToString()));
                if (dt.Rows.Count > 0)
                {
                    DataRow drRow = dt.Rows[0];
                    if (lnkWorkPermit != null)
                        lnkWorkPermit.NavigateUrl = "../Common/download.aspx?dtkey=" + drRow["FLDDTKEY"].ToString();
                }
            }

            HyperLink lnkworkpermit = (HyperLink)e.Item.FindControl("lnkWorkPermit");
            //if (!e.Item.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.RowState.Equals(DataControlRowState.Edit))
            if (lnkworkpermit != null)
            {

                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipWorkPermit");
                lnkworkpermit.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lnkworkpermit.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }

            RadLabel lblIncharge = (RadLabel)e.Item.FindControl("lblIncharge");
            if (lblIncharge != null)
            {

                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipPICName");
                lblIncharge.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lblIncharge.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
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

        if (e.Item is GridDataItem)
        {
            UserControlMaskedTextBox ucSNoEdit = (UserControlMaskedTextBox)e.Item.FindControl("ucSNoEdit");
            if (ucSNoEdit != null) ucSNoEdit.Text = DataBinder.Eval(e.Item.DataItem, "FLDSERIALNUMBER").ToString();

            RadTextBox txtHazardIdEdit = (RadTextBox)e.Item.FindControl("txtHazardIdEdit");
            if (txtHazardIdEdit != null) txtHazardIdEdit.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtRAIdEdit = (RadTextBox)e.Item.FindControl("txtRAIdEdit");
            if (txtRAIdEdit != null) txtRAIdEdit.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtRaTypeEdit = (RadTextBox)e.Item.FindControl("txtRaTypeEdit");
            if (txtRaTypeEdit != null) txtRaTypeEdit.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtCrewIdEdit = (RadTextBox)e.Item.FindControl("txtCrewIdEdit");
            if (txtCrewIdEdit != null) txtCrewIdEdit.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtActivityEdit = (RadTextBox)e.Item.FindControl("txtActivityEdit");

            LinkButton imgShowHazardEdit = (LinkButton)e.Item.FindControl("imgShowHazardEdit");
            if (imgShowHazardEdit != null)
            {
                imgShowHazardEdit.Attributes.Add("onclick", "return showPickList('spnHazardEditRiskAssessment', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListJHA.aspx?&vesselid=" + ucVessel.SelectedVessel + "&activity=" + txtActivityEdit.Text + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, imgShowHazardEdit.CommandName)) imgShowHazardEdit.Visible = false;

            }
            LinkButton imgShowRAEdit = (LinkButton)e.Item.FindControl("imgShowRAEdit");
            if (imgShowRAEdit != null)
            {
                imgShowRAEdit.Attributes.Add("onclick", "return showPickList('spnRAEditRiskAssessment', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListDailyWorkPlanRA.aspx?&vesselid=" + ucVessel.SelectedVessel + "&activity=" + txtActivityEdit.Text + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, imgShowRAEdit.CommandName)) imgShowRAEdit.Visible = false;

            }

            LinkButton imgShowCrewInChargeEdit = (LinkButton)e.Item.FindControl("imgShowCrewInChargeEdit");
            if (imgShowCrewInChargeEdit != null)
            {
                imgShowCrewInChargeEdit.Attributes.Add("onclick", "return showPickList('spnCrewInChargeEditRiskAssessment', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListVesselEmployeeOnboard.aspx?VesselId=" + ucVessel.SelectedVessel + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, imgShowCrewInChargeEdit.CommandName)) imgShowCrewInChargeEdit.Visible = false;

            }

            DataRowView drv = (DataRowView)e.Item.DataItem;
            UserControlWorkPermitByCompany ucworkpermit = (UserControlWorkPermitByCompany)e.Item.FindControl("ucWorkPermitEdit");
            if (ucworkpermit != null)
            {
                DataSet ds = PhoenixInspectionDailyWorkPlanActivity.WorkPermitListByCompany(3, General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
                ucworkpermit.WorkPermitList = ds;
                ucworkpermit.DataBind();
                ucworkpermit.SelectedWorkPermit = drv["FLDWORKPERMITID"].ToString();
            }
            UserControlMaskedTextBox txtStartTimeEdit = (UserControlMaskedTextBox)e.Item.FindControl("txtStartTimeEdit");
            if (txtStartTimeEdit != null)
                txtStartTimeEdit.TextWithLiterals = String.Format("{0:dd/MM/yyyy hh:mm tt}", drv["FLDSTARTDATETIME"]);

            UserControlMaskedTextBox txtEndTimeEdit = (UserControlMaskedTextBox)e.Item.FindControl("txtEndTimeEdit");
            if (txtEndTimeEdit != null)
                txtEndTimeEdit.TextWithLiterals = String.Format("{0:dd/MM/yyyy hh:mm tt}", drv["FLDENDDATETIME"]);
        }

        if (e.Item is GridFooterItem)
        {
            RadTextBox txtHazardIdAdd = (RadTextBox)e.Item.FindControl("txtHazardIdAdd");
            if (txtHazardIdAdd != null) txtHazardIdAdd.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtRAIdAdd = (RadTextBox)e.Item.FindControl("txtRAIdAdd");
            if (txtRAIdAdd != null) txtRAIdAdd.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtRaTypeAdd = (RadTextBox)e.Item.FindControl("txtRaTypeAdd");
            if (txtRaTypeAdd != null) txtRaTypeAdd.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtCrewIdEdit = (RadTextBox)e.Item.FindControl("txtCrewIdAdd");
            if (txtCrewIdEdit != null) txtCrewIdEdit.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtActivityAdd = (RadTextBox)e.Item.FindControl("txtActivityAdd");

            LinkButton imgShowHazardAdd = (LinkButton)e.Item.FindControl("imgShowHazardAdd");
            if (imgShowHazardAdd != null)
            {
                imgShowHazardAdd.Attributes.Add("onclick", "return showPickList('spnHazardAddRiskAssessment', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListJHA.aspx?&vesselid=" + ucVessel.SelectedVessel + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, imgShowHazardAdd.CommandName)) imgShowHazardAdd.Visible = false;

            }
            LinkButton imgShowRAAdd = (LinkButton)e.Item.FindControl("imgShowRAAdd");
            if (imgShowRAAdd != null)
            {
                imgShowRAAdd.Attributes.Add("onclick", "return showPickList('spnRAAddRiskAssessment', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListDailyWorkPlanRA.aspx?&vesselid=" + ucVessel.SelectedVessel + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, imgShowRAAdd.CommandName)) imgShowRAAdd.Visible = false;

            }

            LinkButton imgShowCrewInChargeAdd = (LinkButton)e.Item.FindControl("imgShowCrewInChargeAdd");
            if (imgShowCrewInChargeAdd != null)
            {
                imgShowCrewInChargeAdd.Attributes.Add("onclick", "return showPickList('spnCrewInChargeAddRiskAssessment', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListVesselEmployeeOnboard.aspx?VesselId=" + ucVessel.SelectedVessel + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, imgShowCrewInChargeAdd.CommandName)) imgShowCrewInChargeAdd.Visible = false;

            }

            UserControlWorkPermitByCompany ucworkpermit = (UserControlWorkPermitByCompany)e.Item.FindControl("ucWorkPermitAdd");
            if (ucworkpermit != null)
            {
                ucworkpermit.Company = ViewState["COMPANYID"].ToString();
                ucworkpermit.WorkPermitList = PhoenixInspectionDailyWorkPlanActivity.WorkPermitListByCompany(3, General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
                ucworkpermit.DataBind();
            }
        }
    }
}
