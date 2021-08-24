using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class VesselAccountsEmployeeAppraisalQuery :  PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsEmployeeAppraisalQuery.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsEmployeeAppraisalQuery.aspx", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsEmployeeAppraisalQuery.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            CrewQueryMenu.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["SIGNONOFFID"] = null;
                ViewState["RANKLEVEL"] = null;
                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
               
            }
             if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
                {
                    DataTable dt = PhoenixVesselAccountsCrewFeedback.VesselCrewSignonDetailsEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                    , PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                    PhoenixToolbar toolbarmain = new PhoenixToolbar();
                    if (dt.Rows.Count > 0)
                    {

                        DataRow dr = dt.Rows[0];
                        ViewState["SIGNONOFFID"] = dr["FLDSIGNONOFFID"].ToString();
                        ViewState["RANKLEVEL"] = dr["FLDRANKLEVEL"].ToString();
                        if (General.GetNullableInteger(dr["FLDSIGNONOFFID"].ToString()) != null)
                        {
                            toolbarmain.AddButton("Sign off FeedBack", "FEEDBACK");
                        }
                    }
                    toolbarmain.AddButton("De-Briefing", "DE-BRIEFING");
                    MenuFeedBackHeader.AccessRights = this.ViewState;
                    MenuFeedBackHeader.MenuList = toolbarmain.Show();
                }
         
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuFeedBackHeader_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FEEDBACK"))
            {
                Response.Redirect("../Crew/CrewSignoffFeedback.aspx", false);
            }
            if (CommandName.ToUpper().Equals("DE-BRIEFING"))
            {
                //if (ViewState["RANKLEVEL"] != null && ViewState["RANKLEVEL"].ToString()!="" && int.Parse(ViewState["RANKLEVEL"].ToString()) < 5)
                //{
                    Response.Redirect("../Options/OptionsCrewDeBriefing.aspx?signonoffid=" + ViewState["SIGNONOFFID"].ToString(), false);
                //}
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
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDSIGNONRANKNAME", "FLDSIGNONDATE", "FLDRELIEFDUEDATE" };
                string[] alCaptions = { "File No.", "Name", "Rank", "Sign On", "Relief Due" };

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

                DataTable dt = PhoenixCommonVesselAccounts.SearchVesselEmployeeAppraisalSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                       , General.GetNullableDateTime(txtFromDate.Text)
                                                                                       , General.GetNullableDateTime(txtToDate.Text)
                                                                                       , null
                                                                                       , sortexpression, sortdirection
                                                                                       , 1, iRowCount
                                                                                       , ref iRowCount, ref iTotalPageCount);
                General.ShowExcel("Crew List", dt, alColumns, alCaptions, sortdirection, sortexpression);
            }
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                if (!IsValidFilter())
                {
                    ucError.Visible = true;
                    return;
                }
                BindData();
                gvCrewSearch.Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtFromDate.Text = string.Empty;
                txtToDate.Text = string.Empty;
                BindData();
                gvCrewSearch.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindData()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDSIGNONRANKNAME", "FLDSIGNONDATE", "FLDRELIEFDUEDATE" };
        string[] alCaptions = { "File No.", "Name", "Rank", "Sign On", "Relief Due" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {

            DataTable dt = PhoenixCommonVesselAccounts.SearchVesselEmployeeAppraisalSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                       , General.GetNullableDateTime(txtFromDate.Text)
                                                                       , General.GetNullableDateTime(txtToDate.Text)
                                                                       , null
                                                                       , sortexpression, sortdirection
                                                                       , (int)ViewState["PAGENUMBER"], gvCrewSearch.PageSize
                                                                       , ref iRowCount, ref iTotalPageCount);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvCrewSearch", "Crew List", alCaptions, alColumns, ds);

            gvCrewSearch.DataSource = dt;
            gvCrewSearch.VirtualItemCount = iRowCount;
            
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvCrewSearch_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

   

    //protected void gvCrewSearch_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Row.RowType == DataControlRowType.Header)
    //        {
    //            if (ViewState["SORTEXPRESSION"] != null)
    //            {
    //                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //                if (img != null)
    //                {
    //                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                        img.Src = Session["images"] + "/arrowUp.png";
    //                    else
    //                        img.Src = Session["images"] + "/arrowDown.png";

    //                    img.Visible = true;
    //                }

    //            }
    //        }
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            ImageButton cmdCrewExport2XL = (ImageButton)e.Row.FindControl("cmdCrewExport2XL");
    //            Label lblShowxl = (Label)e.Row.FindControl("lblShowxl");
    //            if (SessionUtil.CanAccess(this.ViewState, cmdCrewExport2XL.CommandName) && lblShowxl.Text.Trim().Equals("1")
    //                    //&& PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0
    //                    )
    //            {
    //                cmdCrewExport2XL.Visible = true;
    //            }
    //            else
    //                cmdCrewExport2XL.Visible = false;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvCrewSearch_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    GridView _gridView = sender as GridView;
    //    Filter.CurrentCrewSelection = ((Label)_gridView.Rows[e.NewEditIndex].FindControl("lblEmployeeid")).Text;
    //    if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "PHOENIX")
    //    {
    //        Response.Redirect("..\\Crew\\CrewAppraisal.aspx?vslid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString(), false);
    //    }
    //    else
    //    {
    //        Response.Redirect("..\\CrewOffshore\\CrewOffshoreAppraisal.aspx?vslid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "&cmd=" + "1", false);
    //    }
    //}

      
  
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
      //  ViewState["PAGENUMBER"] = 1;
        BindData();
        
        
    }

    private bool IsValidFilter()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(txtFromDate.Text) == null)
            ucError.ErrorMessage = "From date is required";
        if (General.GetNullableDateTime(txtToDate.Text) == null)
            ucError.ErrorMessage = "To date is required";

        return (!ucError.IsError);
    }

    protected void gvCrewSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewSearch.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewSearch_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
         

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            //Label lblAssessmentId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblAssessmentId");
            RadLabel lblSignonOffId = (RadLabel)e.Item.FindControl("lblSignonOffId");
            RadLabel lblRank = (RadLabel)e.Item.FindControl("lblRank");
            RadLabel lblEmployeeid = (RadLabel)e.Item.FindControl("lblEmployeeid");
            if (e.CommandName.ToUpper().Equals("CREWEXPORT2XL"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsEmployeeOC28Assessment.aspx?Employeeid=" + lblEmployeeid.Text.Trim(), false);
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

    protected void gvCrewSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    if (ViewState["SORTEXPRESSION"] != null)
            //    {
            //        HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
            //        if (img != null)
            //        {
            //            if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
            //                img.Src = Session["images"] + "/arrowUp.png";
            //            else
            //                img.Src = Session["images"] + "/arrowDown.png";

            //            img.Visible = true;
            //        }

            //    }
            //}
            if (e.Item is GridDataItem)
            {
                ImageButton cmdCrewExport2XL = (ImageButton)e.Item.FindControl("cmdCrewExport2XL");
                RadLabel lblShowxl = (RadLabel)e.Item.FindControl("lblShowxl");
                if (SessionUtil.CanAccess(this.ViewState, cmdCrewExport2XL.CommandName) && lblShowxl.Text.Trim().Equals("1")
                        //&& PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0
                        )
                {
                    cmdCrewExport2XL.Visible = true;
                }
                else
                    cmdCrewExport2XL.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewSearch_EditCommand(object sender, GridCommandEventArgs e)
    {
       
        Filter.CurrentCrewSelection = ((RadLabel)e.Item.FindControl("lblEmployeeid")).Text;
        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "PHOENIX")
        {
            Response.Redirect("..\\Crew\\CrewAppraisal.aspx?vslid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString(), false);
        }
        else
        {
            Response.Redirect("..\\CrewOffshore\\CrewOffshoreAppraisal.aspx?vslid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "&cmd=" + "1", false);
        }
    }
}
