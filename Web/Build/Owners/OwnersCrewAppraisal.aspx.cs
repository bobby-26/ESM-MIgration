using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class CrewAppraisalMidtenureactivity : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{

    //    ViewState["CATEGORY"] = "";
    //    foreach (GridDataItem gv in gvmidturn.Items)
    //    {
    //        Table gridTable1 = (Table)gvmidturn.Controls[0];
    //        Label lblCATEGORYcode = (Label)gv.FindControl("lblcategory");
    //        Label lblCATEGORYname = (Label)gv.FindControl("lblcategoryDesc");
    //        if (ViewState["CATEGORY"].ToString().Trim().Equals("") || !ViewState["CATEGORY"].ToString().Trim().Equals(lblCATEGORYcode.Text.Trim()))
    //        {
    //            if (lblCATEGORYcode != null)
    //            {
    //                //if (lblCATEGORYcode.Text != null && lblCATEGORYcode.Text != "")
    //                //{
    //                //    ViewState["CATEGORY"] = lblCATEGORYcode.Text.Trim();
    //                //    int rowIndex = gridTable1.Rows.GetRowIndex(gv);
    //                //    // Add new group header row  
    //                //    GridViewRow headerRow = new GridViewRow(rowIndex, rowIndex, DataControlRowType.DataRow, DataControlRowState.Normal);
    //                //    TableCell headerCell = new TableCell();
    //                //    headerCell.ColumnSpan = gvmidturn.Columns.Count;
    //                //    headerCell.Text = @"<font size=""2"" ><b>" + string.Format("{0}", lblCATEGORYname != null ? lblCATEGORYname.Text : "") + "</b></font>";
    //                //    headerCell.CssClass = "GroupHeaderRowStyle";
    //                //    // Add header Cell to header Row, and header Row to gridTable  
    //                //    headerRow.Cells.Add(headerCell);
    //                //    headerRow.HorizontalAlign = HorizontalAlign.Left;
    //                //    gridTable1.Controls.AddAt(rowIndex, headerRow);
    //                //}
    //            }
    //        }
    //    }
    //    base.Render(writer);
    //}
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ViewState["Rcategory"] = 0;
            ViewState["SIGNONOFFID"] = "";
            ViewState["SCORE"] = "";
            ViewState["OCCASSIONNAME"] = "";
            ViewState["Category"] = 0;
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            if (Request.QueryString["appraisalid"] != null)
                Filter.CurrentAppraisalSelection = Request.QueryString["appraisalid"].ToString();

            SetEmployeePrimaryDetails();
            EditAppraisal();

            GetRankCategory();
            DataSet occassionds = PhoenixRegistersMiscellaneousAppraisalOccasion.ListMiscellaneousAppraisalOccasion(General.GetNullableInteger(ViewState["Rcategory"].ToString()) == null ? 0 : int.Parse(ViewState["Rcategory"].ToString()), 1);
            ddlOccassion.OccassionList = occassionds;
            ddlOccassion.Category = ViewState["Rcategory"].ToString();
            ddlOccassion.DataBind();
            foreach (DataRow dr in occassionds.Tables[0].Rows)
            {
                if (dr["FLDOCCASION"].ToString().Contains("Owner"))
                {
                    ddlOccassion.SelectedOccassion = dr["FLDOCCASIONID"].ToString();
                    break;
                }
            }
            ddlOccassion.Enabled = false;
        }
        CreateTab();
    }
    private void CreateTab()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
        if (Filter.CurrentAppraisalSelection != null)
        {
            toolbarmain.AddButton("Complete", "CONFIRM", ToolBarDirection.Right);
        }
        CrewAppraisal.AccessRights = this.ViewState;
        CrewAppraisal.MenuList = toolbarmain.Show();

        toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Appraisal", "APPRAISAL");
        toolbarmain.AddButton("Form", "FORM");

        if (Filter.CurrentAppraisalSelection != null)
        {
                toolbarmain.AddButton("Appraisal Report", "APPRAISALREPORT");                       
        }

        AppraisalTabs.AccessRights = this.ViewState;
        AppraisalTabs.MenuList = toolbarmain.Show();
        AppraisalTabs.SelectedMenuIndex = 1;

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        EditAppraisal();
    }
    protected void AppraisalTabs_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("APPRAISAL"))
            {
                Response.Redirect("../Crew/CrewAppraisal.aspx", false);
            }
            if (CommandName.ToUpper().Equals("APPRAISALREPORT"))
            {
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=APPRAISALOWNERFEEDBACK&appraisalid=" + Filter.CurrentAppraisalSelection + "&showmenu=0&showword=no&showexcel=no", false);
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
        gvmidturn.SelectedIndexes.Clear();
        gvmidturn.EditIndexes.Clear();
        gvmidturn.DataSource = null;
        gvmidturn.Rebind();
    }
    protected void CrewAppraisal_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            string vessel = ddlVessel.SelectedVessel;
            string fromdate = txtFromDate.Text;
            string todate = txtToDate.Text;
            string appraisaldate = txtdate.Text;
            string occassion = ddlOccassion.SelectedOccassion;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string iAppraisalId = "";

                if (!IsValidateAppraisal(vessel, fromdate, todate, occassion, appraisaldate))
                {
                    ucError.Visible = true;
                    return;
                }
                if (Filter.CurrentAppraisalSelection == null)
                {
                    PhoenixCrewAppraisal.InsertAppraisal(
                                                    int.Parse(Filter.CurrentCrewSelection)
                                                   , DateTime.Parse(fromdate)
                                                   , DateTime.Parse(todate)
                                                   , int.Parse(vessel)
                                                   , General.GetNullableDateTime(appraisaldate)
                                                   , int.Parse(ddlOccassion.SelectedOccassion)
                                                   , ref iAppraisalId
                                                   , General.GetNullableInteger("0")
                                                   );

                    Filter.CurrentAppraisalSelection = iAppraisalId.ToString();

                    PhoenixCrewAppraisal.InsertAppraisalOwnerFeedBack(new Guid(iAppraisalId.ToString())
                                                                     , int.Parse(Filter.CurrentCrewSelection)
                                                                     , int.Parse(vessel)
                                                                     , int.Parse(ddlOccassion.SelectedOccassion)
                                                                     , General.GetNullableInteger(ViewState["Rankid"].ToString()));


                    divOtherSection.Visible = true;
                    EditAppraisal();
                    Rebind();
                    CreateTab();
                }
                else
                {
                    PhoenixCrewAppraisal.UpdateAppraisal(new Guid(Filter.CurrentAppraisalSelection)
                                                , DateTime.Parse(fromdate)
                                                , DateTime.Parse(todate)
                                                , int.Parse(vessel)
                                                , General.GetNullableDateTime(appraisaldate)
                                                , int.Parse(occassion)
                                                , General.GetNullableByte("1")
                                                , General.GetNullableInteger("0"));

                    foreach (GridDataItem gv in gvmidturn.Items)
                    {
                        string score = ((RadDropDownList)gv.FindControl("ddlscore")).SelectedValue;
                        string ScoreId = ((RadLabel)gv.FindControl("lblAppraisalScoreid")).Text;

                        if (!IsValidRemarks(score))
                        {
                            ucError.Visible = true;
                            return;
                        }
                        PhoenixCrewAppraisal.UpdateAppraisalMidTenureQuestion(new Guid(ScoreId.ToString()), int.Parse(score), null, null);
                    }
                }

                PhoenixCrewAppraisal.UpdateAppraisalYesNoQuestion(new Guid(Filter.CurrentAppraisalSelection)
                                                   , null, null, null, null, null, null, null
                                                   , 0, null, null
                                                   , General.GetNullableString(txtHeadOfDeptComment.Text)
                                                   , General.GetNullableString("")
                                                   , General.GetNullableString("")
                                                   , null, null
                                                   , General.GetNullableString("")
                                                   , General.GetNullableInteger("0")
                                                   );

                ucStatus.Text = "Appraisal Information updated.";
                EditAppraisal();
                Rebind();
                
            }
            if (CommandName.ToUpper().Equals("CONFIRM"))
            {
                foreach (GridDataItem gv in gvmidturn.Items)
                {
                    string score = ((RadDropDownList)gv.FindControl("ddlscore")).SelectedValue;
                    string ScoreId = ((RadLabel)gv.FindControl("lblAppraisalScoreid")).Text;


                    if (!IsValidRemarks(score))
                    {
                        ucError.Visible = true;
                        return;
                    }
                }

                PhoenixCrewAppraisal.UpdateAppraisal(new Guid(Filter.CurrentAppraisalSelection)
                              , DateTime.Parse(fromdate)
                              , DateTime.Parse(todate)
                              , int.Parse(vessel)
                              , General.GetNullableDateTime(appraisaldate)
                              , int.Parse(occassion)
                              , 0
                              , General.GetNullableInteger("0")
                  );

                foreach (GridDataItem gv in gvmidturn.Items)
                {
                    string score = ((RadDropDownList)gv.FindControl("ddlscore")).SelectedValue;
                    string ScoreId = ((RadLabel)gv.FindControl("lblAppraisalScoreid")).Text;

                    if (!IsValidRemarks(score))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixCrewAppraisal.UpdateAppraisalMidTenureQuestion(new Guid(ScoreId.ToString()), int.Parse(score), null, null);

                }

                PhoenixCrewAppraisal.UpdateAppraisalYesNoQuestion(new Guid(Filter.CurrentAppraisalSelection)
                                                                   , null, null, null, null, null, null, null
                                                                   , 0, null, null, General.GetNullableString(txtHeadOfDeptComment.Text)
                                                                   , General.GetNullableString(""), General.GetNullableString("")
                                                                   , null, null, General.GetNullableString(""), General.GetNullableInteger("0")
                                                                   );

                ucStatus.Text = "This appraisal is finalised.";
                EditAppraisal();
                          
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void EditAppraisal()
    {

        if (Filter.CurrentAppraisalSelection != null)
        {
            DataSet ds = PhoenixCrewAppraisal.EditAppraisal(new Guid(Filter.CurrentAppraisalSelection));

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlVessel.SelectedVessel = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                txtFromDate.Text = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDFROMDATE"].ToString());
                txtToDate.Text = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDTODATE"].ToString());
                txtdate.Text = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDAPPRAISALDATE"].ToString());
                ddlOccassion.SelectedOccassion = ds.Tables[0].Rows[0]["FLDOCCASSIONFORREPORT"].ToString();
                                                
                txtSignOnDate.Text = ds.Tables[0].Rows[0]["FLDSIGNONDATE"].ToString();
                if (ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString() == "0")
                {
                    CrewAppraisal.Visible = false;
                }
                txtHeadOfDeptComment.Text = HttpUtility.HtmlDecode(ds.Tables[0].Rows[0]["FLDHEADDEPTCOMMENT"].ToString());
                ViewState["AppraisalStatus"] = int.Parse(ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString());
                ViewState["OCCASIONID"] = ds.Tables[0].Rows[0]["FLDOCCASSIONFORREPORT"].ToString();
            }
        }
    }

    public void GetRankCategory()
    {
        string Rcategory = null;

        PhoenixCrewAppraisalProfile.GetRankCategory(int.Parse(ViewState["Rankid"].ToString()), ref Rcategory);

        if (Rcategory == System.DBNull.Value.ToString())
            Rcategory = "0";

        ViewState["Rcategory"] = Rcategory.ToString();
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {

            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtEmployeeName.Text = dt.Rows[0]["FLDEMPLOYEENAME"].ToString();           
                txtRank.Text = dt.Rows[0]["FLDRANKPOSTEDNAME"].ToString();
                ddlVessel.SelectedVessel = dt.Rows[0]["FLDPRESENTVESSELID"].ToString();
                if (Filter.CurrentAppraisalSelection == null)
                {
                    divOtherSection.Visible = false;                    
                }
                ViewState["Rankid"] = dt.Rows[0]["FLDRANKPOSTED"].ToString();                
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
        try
        {
            if (Filter.CurrentAppraisalSelection == null) return;
            DataSet ds = PhoenixCrewAppraisal.AppraisalOwnerFeedbackSearch(new Guid(Filter.CurrentAppraisalSelection), int.Parse(ViewState["Rankid"].ToString()));

                gvmidturn.DataSource = ds.Tables[0];
         
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvmidturn_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {           
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvmidturn_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem; 

            RadDropDownList ddlScore = (RadDropDownList)e.Item.FindControl("ddlscore");
            if (ddlScore != null) ddlScore.SelectedValue = drv["FLDSCORE"].ToString();

        }

    }
    private bool IsValidRemarks(string score)
    {
        ucError.HeaderMessage = "Please provide the following required  Primary Details information";

        if (string.IsNullOrEmpty(score))
        {
            ucError.ErrorMessage = "Score is required.";
        }
        if (General.GetNullableString(txtHeadOfDeptComment.Text) == null && score == "3")
        {
            ucError.ErrorMessage = "Comment is mandatory if score is Below Expectation (BE).";
        }        
        return (!ucError.IsError);

    }
    private bool IsValidateAppraisal(string vessel, string fromdate, string todate, string occassion, string appraisaldate)
    {
        ucError.HeaderMessage = "Please provide the following required  Primary Details information";

        int result;
        DateTime resultdate;
        if (!int.TryParse(vessel, out result))
            ucError.ErrorMessage = "Vessel is required.";
        if (fromdate == null || !DateTime.TryParse(fromdate, out resultdate))
            ucError.ErrorMessage = "From Date is required";
        if (todate == null || !DateTime.TryParse(todate, out resultdate))
            ucError.ErrorMessage = "To Date is required";
        else if (!string.IsNullOrEmpty(fromdate)
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
        if (occassion.ToUpper() == "DUMMY" || occassion == "")
        {
            ucError.Text = "Please Select Occassion For Report";
        }
        if (appraisaldate == null || !DateTime.TryParse(appraisaldate, out resultdate))
            ucError.ErrorMessage = "Appraisal On is required";
        else if (!string.IsNullOrEmpty(todate)
              && DateTime.TryParse(appraisaldate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(todate)) < 0)
        {
            ucError.ErrorMessage = "'Appraisal On' should be later than or equal to 'To Date'";
        }
        if (!string.IsNullOrEmpty(appraisaldate)
            && DateTime.Compare(DateTime.Today, DateTime.Parse(appraisaldate)) < 0)
        {
            ucError.ErrorMessage = "'Appraisal date' should not be future date";
        }

        return (!ucError.IsError);
    }
    public void GetOccasionName(int OccasionId, Guid AppraisalId)
    {
        string OccasionName = null;
        string Appraisalnew = "0";

        PhoenixCrewAppraisal.AppraisalOccasionName(OccasionId, AppraisalId, ref OccasionName, ref Appraisalnew);

        if (OccasionName == System.DBNull.Value.ToString())
            OccasionName = "";

        if (Appraisalnew == System.DBNull.Value.ToString())
            Appraisalnew = "0";

        ViewState["OCCASSIONNAME"] = OccasionName.ToString();
    }
}
