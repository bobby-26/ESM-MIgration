using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using Telerik.Web.UI;
public partial class CrewAppraisalReceivedFromVessel : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewAppraisalReceivedFromVessel.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvAQ')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Crew/CrewAppraisalReceivedFromVessel.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuCrewAppraisal.AccessRights = this.ViewState;
            MenuCrewAppraisal.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["Rcategory"] = 0;
                gvAQ.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ddlAppraisalReceivedyn.SelectedValue = "1";
                chkOnBoardYN.Checked = true;
                ucToDate.Text = General.GetDateTimeToString(DateTime.Now);
                ucFromDate.Text = General.GetDateTimeToString(DateTime.Now.AddYears(-1));
                //BindOccasion();

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
        gvAQ.SelectedIndexes.Clear();
        gvAQ.EditIndexes.Clear();
        gvAQ.DataSource = null;
        gvAQ.Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDROWNUMBER", "FLDFILENO", "FLDNAME", "FLDRANKNAME", "FLDVESSELNAME", "FLDSIGNONDATE", "FLDSIGNOFFDATE", "FLDAPPRAISALDATE", "FLDMODIFIEDDATE", "FLDREVIEWDUEDATE", "FLDLASTAPPRAISAL", "FLDSTATUS", "FLDPROMOTIONYESNO", "FLDOCCASION" };
        string[] alCaptions = { "S.No", "File No", "Name", "Rank", "Vessel", "Sign On Date", "Sign Off Date", "Appraisal Date", "Updated Date", "Review Due Date", "Last Appraisal On/Occassion", "Status", "Promotion YN", "Occaison For Report" };
        string sortexpression;
        int? sortdirection = null;
        int? onboardyn = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        int appraisalyn = 0;
        //if (string.IsNullOrEmpty(General.GetNullableString(ddlOccassion.SelectedOccassionId))
        //iOccassion = General.GetNullableInteger(ddlOccassion.SelectedOccassion);
        if (ddlAppraisalReceivedyn.SelectedIndex <= 0)
            appraisalyn = 1;
        else
            appraisalyn = General.GetNullableInteger(ddlAppraisalReceivedyn.SelectedValue).Value;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (chkOnBoardYN.Checked == true)
            onboardyn = 1;
        else if (!chkOnBoardYN.Checked == true)
            onboardyn = 0;
        else
            onboardyn = null;

        DataSet ds = PhoenixCrewAppraisal.ReportAppraisalReceivedFromVessel(General.GetNullableDateTime(ucFromDate.Text)
               , General.GetNullableDateTime(ucToDate.Text)
               , General.GetNullableString(ucVesselType.SelectedVesseltype)
               , General.GetNullableString(ucVessel.SelectedVessel)
               , General.GetNullableString(ucPool.SelectedPool)
               , sortexpression
               , sortdirection
               , (int)ViewState["PAGENUMBER"]
               , General.ShowRecords(null)
               , ref iRowCount
               , ref iTotalPageCount
               , txtName.Text
               , General.GetNullableInteger(ddlOccassion.SelectedOccassionId)
               , appraisalyn
               , onboardyn
               );

        General.ShowExcel("Appraisal Report", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void MenuCrewAppraisal_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucFromDate.Text = "";
                ucToDate.Text = "";
                ucVesselType.SelectedVesseltype = "";
                ucVessel.SelectedVessel = "";
                ucPool.SelectedPool = "";
                //ddlOccasion.sele = "";
                ddlOccassion.SelectedOccassionId = "";
                chkOnBoardYN.Checked = false;
                txtName.Text = "";
                ddlAppraisalReceivedyn.SelectedIndex = 0;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter(ucFromDate.Text, ucToDate.Text, ddlOccassion.SelectedOccassionId))
                {
                    ucError.Visible = true;
                    return;
                }
                Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAQ_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

        }
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    Label lblAppraisalId = (Label)e.Row.FindControl("lblAppraisalId");
        //    Label lblCrewId = (Label)e.Row.FindControl("lblCrewId");
        //    LinkButton lb = (LinkButton)e.Row.FindControl("lnkName");
        //    lb.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewAppraisalActivity.aspx?appraisalid=" + lblAppraisalId.Text + "&empid=" + lblCrewId.Text + "'); return false;");
        //}
    }

    protected void gvAQ_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAQ.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvAQ_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvAQ_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

            if (e.Item is GridDataItem)
            {



                if (e.CommandName.ToString().ToUpper() == "SORT")
                    return;
                if (e.CommandName.ToString().ToUpper() == "NAME")
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    //GridViewRow row = gvAQ.Rows[index];
                    RadLabel lblCrewId = (RadLabel)e.Item.FindControl("lblCrewId");
                    RadLabel lblAppraisalId = (RadLabel)e.Item.FindControl("lblAppraisalId");
                    RadLabel lblOccasionId = (RadLabel)e.Item.FindControl("lblOccassionId");
                    Filter.CurrentCrewSelection = lblCrewId.Text;
                    if (!string.IsNullOrEmpty(lblAppraisalId.Text))
                    {
                        string OccasionName = null;
                        string Appraisalnew = "0";
                        PhoenixCrewAppraisal.AppraisalOccasionName(General.GetNullableInteger(lblOccasionId.Text).Value
                                                                   , General.GetNullableGuid(lblAppraisalId.Text).Value
                                                                   , ref OccasionName
                                                                   , ref Appraisalnew);
                        if (OccasionName.ToString().ToUpper().Trim() == "MID TENURE REVIEW" && Appraisalnew == "1")
                        {
                            string scriptpopup = String.Format("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewAppraisalMidtenureactivity.aspx?appraisalId=" + lblAppraisalId.Text + "&empid=" + lblCrewId.Text + " ');");
                            //string scriptpopup = String.Format("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewAppraisalMidtenureactivity.aspx?appraisalId=" + lblAppraisalId.Text + "&empid=" + lblCrewId.Text + " ');");
                            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                                                             
                        }
                        else
                        {
                            string scriptpopup = String.Format("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/Crew/CrewAppraisalActivity.aspx?appraisalId=" + lblAppraisalId.Text + "&empid=" + lblCrewId.Text + " ');");
                            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                        }
                    }
                    else
                    {
                        string scriptpopup = String.Format("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewAppraisal.aspx');");
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }



    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? onboardyn = null;
        string[] alColumns = { "FLDROWNUMBER", "FLDFILENO", "FLDNAME", "FLDRANKNAME", "FLDVESSELNAME", "FLDSIGNONDATE", "FLDSIGNOFFDATE", "FLDAPPRAISALDATE", "FLDMODIFIEDDATE", "FLDREVIEWDUEDATE", "FLDLASTAPPRAISAL", "FLDSTATUS", "FLDPROMOTIONYESNO", "FLDOCCASION" };
        string[] alCaptions = { "S.No", "File No", "Name", "Rank", "Vessel", "Sign On Date", "Sign Off Date", "Appraisal Date", "Updated Date", "Review Due Date", "Last Appraisal On/Occassion", "Status", "Promotion YN", "Occaison For Report" };
        int? sortdirection = 1; //DEFAULT DESC SORT
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        int appraisalyn = 0;
        //if (string.IsNullOrEmpty(General.GetNullableString(ddlOccassion.SelectedOccassionId))
        //iOccassion = General.GetNullableInteger(ddlOccassion.SelectedOccassion);
        if (ddlAppraisalReceivedyn.SelectedIndex <= 0)
            appraisalyn = 1;
        else
            appraisalyn = General.GetNullableInteger(ddlAppraisalReceivedyn.SelectedValue).Value;
        string sortexpression;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (chkOnBoardYN.Checked == true)
            onboardyn = 1;
        else if (!chkOnBoardYN.Checked == true)
            onboardyn = 0;
        else
            onboardyn = null;

        if (!string.IsNullOrEmpty(ucToDate.Text) && !string.IsNullOrEmpty(ucFromDate.Text))
        {
            int yeardiff = (General.GetNullableDateTime(ucToDate.Text).Value).Year - (General.GetNullableDateTime(ucFromDate.Text).Value).Year;

            if (yeardiff > 1)
            {
                //ucFromDate.Text = "";
                ucToDate.Text = General.GetDateTimeToString(General.GetNullableDateTime(ucFromDate.Text).Value.AddYears(1));
                ucError.ErrorMessage = "Cannot get the record for more than a year";
                ucError.Visible = true;
            }
        }
        try
        {

            DataSet ds = PhoenixCrewAppraisal.ReportAppraisalReceivedFromVessel(General.GetNullableDateTime(ucFromDate.Text)
               , General.GetNullableDateTime(ucToDate.Text)
               , General.GetNullableString(ucVesselType.SelectedVesseltype)
               , General.GetNullableString(ucVessel.SelectedVessel)
               , General.GetNullableString(ucPool.SelectedPool)
               , sortexpression
               , sortdirection
               , (int)ViewState["PAGENUMBER"]
               , gvAQ.PageSize
               , ref iRowCount
               , ref iTotalPageCount
               , txtName.Text
               , General.GetNullableInteger(ddlOccassion.SelectedOccassionId)
               , appraisalyn
               , onboardyn
               );

            General.SetPrintOptions("gvAQ", "Appraisal Report", alCaptions, alColumns, ds);

            gvAQ.DataSource = ds;
            gvAQ.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    private bool IsValidFilter(string fromdate, string todate, string occasion)
    {
        ucError.HeaderMessage = "Please provide the following filter criteria";
        DateTime resultdate;

        if (fromdate == null || !DateTime.TryParse(fromdate, out resultdate))
            ucError.ErrorMessage = "From Date is required";
        if (todate == null || !DateTime.TryParse(todate, out resultdate))
            ucError.ErrorMessage = "To Date is required";
        if (string.IsNullOrEmpty(occasion) || occasion.ToUpper() == "DUMMY")
            ucError.ErrorMessage = "Occasion for report is mandatory";
        if ((fromdate == null || !DateTime.TryParse(fromdate, out resultdate) && todate == null || !DateTime.TryParse(todate, out resultdate)) && chkOnBoardYN.Checked == false)
        {
            ucError.ErrorMessage = "From Date and To Date is required or Onboard Yes is required";
        }

        if (!string.IsNullOrEmpty(fromdate)
              && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "'To Date' should be later than 'From Date'";
        }
        if (!string.IsNullOrEmpty(fromdate)
            && DateTime.Compare(DateTime.Today, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "'From date' should not be future date";
        }
        if (!string.IsNullOrEmpty(todate)
            && DateTime.Compare(DateTime.Today, DateTime.Parse(todate)) < 0)
        {
            ucError.ErrorMessage = "'To date' should not be future date";
        }

        return (!ucError.IsError);
    }



    //protected void ddlRank_TextChangedEvent(object sender, EventArgs e)
    //{
    //    string Rcategory = null;
    //    if (!string.IsNullOrEmpty(General.GetNullableString(ddlRank.SelectedRank)))
    //        PhoenixCrewAppraisalProfile.GetRankCategory(ddlRank.SelectedValue, ref Rcategory);

    //    if (Rcategory == System.DBNull.Value.ToString())
    //        Rcategory = "0";
    //    ViewState["Rcategory"] = Rcategory.ToString();

    //    BindOccasion();
    //}
    //private void BindOccasion()
    //{
    //    ddlOccassion.OccassionList = PhoenixRegistersMiscellaneousAppraisalOccasion.ListMiscellaneousAppraisalOccasion(General.GetNullableInteger(ViewState["Rcategory"].ToString()) == null ? 0 : int.Parse(ViewState["Rcategory"].ToString()), 1);
    //    ddlOccassion.Category = ViewState["Rcategory"].ToString();
    //    ddlOccassion.DataBind();
    //}

    protected void gvAQ_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell;

            HeaderCell = new TableCell();
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 5;
            HeaderGridRow.Cells.Add(HeaderCell);
            HeaderCell = new TableCell();
            HeaderCell.Text = "Date of";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 4;
            HeaderGridRow.Cells.Add(HeaderCell);
            //HeaderCell = new TableCell();
            //HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            //HeaderCell.ColumnSpan = 2;
            //HeaderGridRow.Cells.Add(HeaderCell);
            //HeaderCell.Text = "Request";

            HeaderCell = new TableCell();
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 4;
            HeaderGridRow.Cells.Add(HeaderCell);

            gvAQ.Controls[0].Controls.AddAt(0, HeaderGridRow);
            GridViewRow row = ((GridViewRow)gvAQ.Controls[0].Controls[0]);
            row.Attributes.Add("style", "position:static");
        }
    }
}
