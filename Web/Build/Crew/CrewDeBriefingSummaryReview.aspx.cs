using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Common;
using System.Web.Profile;
using Telerik.Web.UI;
public partial class CrewDeBriefingSummaryReview : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    if (!IsPostBack)
    //    {
    //        //BindData();
    //    }
    //    //if (gvFeedBackQst.Controls.Count > 0)
    //    //{
    //    //    Table gridTable = (Table)gvFeedBackQst.Controls[0];
    //    //    ViewState["Category"] = "";
    //    //    foreach (GridDataItem gv in gvFeedBackQst.Items)
    //    //    {
    //    //        RadLabel lblCategory = (RadLabel)gv.FindControl("lblcategorynameG");
    //    //        string strCategory = lblCategory != null ? lblCategory.Text.Trim() : "";
    //    //        if (lblCategory != null)
    //    //        {

    //    //            if (ViewState["Category"].ToString() != strCategory)
    //    //            {
    //    //                ViewState["Category"] = strCategory;
    //    //                int rowIndex = gridTable.Rows.GetRowIndex(gv);
    //    //                // Add new group header row  

    //    //                GridViewRow headerRow = new GridViewRow(rowIndex, rowIndex, DataControlRowType.DataRow, DataControlRowState.Normal);

    //    //                TableCell headerCell = new TableCell();

    //    //                headerCell.ColumnSpan = gvFeedBackQst.Columns.Count;

    //    //                headerCell.Text = @"<font size=""2"" ><b>" + string.Format("{0}", strCategory != null ? strCategory : "") + "</b></font>";

    //    //                headerCell.CssClass = "GroupHeaderRowStyle";

    //    //                // Add header Cell to header Row, and header Row to gridTable  

    //    //                headerRow.Cells.Add(headerCell);
    //    //                headerRow.HorizontalAlign = HorizontalAlign.Left;
    //    //                gridTable.Controls.AddAt(rowIndex, headerRow);
                        
    //    //            }
    //    //        }
    //    //    }
    //    //    base.Render(writer);
    //    //}
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Back", "BACK");
            FeedBackTabs.AccessRights = this.ViewState;
            FeedBackTabs.MenuList = toolbarmain.Show();
            divPrimarySection.Visible = true;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBERPTC"] = 1;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SignOnOffId"] = string.IsNullOrEmpty(Request.QueryString["signonoffid"]);
               
                ViewState["Trainingcoursetype"] = PhoenixCommonRegisters.GetHardCode(1, 103, "7");
                ucdoa.Text = string.Empty;
                SetEmployeePrimaryDetails();
                DOAAdressBind();

                gvCrewRecommendedCourses.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvOffshorePCourseTraining.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }


            //BindData();
            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "PHOENIX")
            {
                //BindDatacourse();
              
                txtGeneralComments.Visible = true;
                lblGeneralComments.Visible = true;
                lblPendingTrainingneeds.Text = "Recommended Course";
                lnkCrewDeBriefingForm.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '"+Session["sitepath"]+"/Options/OptionsCrewDeBriefing.aspx?signonoffid=" + Request.QueryString["Signonoffid"].ToString() + "');return false;");
            }
            else
            {
                BindDataPTC();
                lnkCrewDeBriefingForm.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Options/OptionsOffshoreCrewDeBriefing.aspx?signonoffid=" + Request.QueryString["Signonoffid"].ToString() + "');return false;");
                lnkCrewDeBriefingForm.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindDataPTC()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDVESSELNAME", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME",
                             "FLDTYPEOFTRAININGNAME", "FLDREQUESTEDCOURSESNAME", "FLDTOBEDONEBYNAME", "FLDDETAILSOFTRAINING", "FLDNAMEOFTRAINERINSTITUTE", "FLDDATECOMPLETED" };
            string[] alCaptions = { "Vessel", "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
                              "Type of Training", "Course/CBT", "To be done by", "Details of Training Imparted / Course Attended", "Name of Trainer / Institute","Date Completed" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixCrewOffshoreTrainingNeeds.SearchTrainingNeeds(General.GetNullableInteger(ViewState["Empid"].ToString()),
                    null,
                    sortexpression
                    , sortdirection,
                    Int32.Parse(ViewState["PAGENUMBERPTC"].ToString()),
                   gvOffshorePCourseTraining.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount,
                    null,
                    null,
                    null,
                    1,
                    General.GetNullableInteger(ViewState["Trainingcoursetype"].ToString())
                    );

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvOffshorePCourseTraining", "Pending Training Course", alCaptions, alColumns, ds);

            gvOffshorePCourseTraining.DataSource = ds;

          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOffshorePCourseTraining_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
 
    protected void ddlApprovalType_TextChanged(object sender, EventArgs e)
    {
        RadComboBox dc = (RadComboBox)sender;
        RadTextBox txtRemarks = new RadTextBox();
        RadGrid row = ((RadGrid)dc.Parent.Parent);
        if (ViewState["nCurrentRow"] != null)
        {
            GridDataItem item = row.Items[int.Parse(ViewState["nCurrentRow"].ToString()) - 2];
            txtRemarks = (RadTextBox)item.FindControl("txtRemarks");
        }
     
        if (dc.SelectedValue == "0")
        {

            txtRemarks.CssClass = "input_mandatory";
        }
        else
        {
            txtRemarks.CssClass = "input";
        }
    }
 
    public class GridDecorator
    {
        public static void MergeRows(RadGrid gridView)
        {
            for (int rowIndex = gridView.Items.Count - 2; rowIndex >= 0; rowIndex--)
            {
                //GridViewRow row = gridView.Items[rowIndex];
                //GridViewRow previousRow = gridView.Items[rowIndex + 1];

            }

        }
    }
    protected void FeedBackTabs_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("BACK"))
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode=="PHOENIX")
                {
                    Response.Redirect("../Crew/CrewDeBriefingSummary.aspx");
                }
                else
                {
                    Response.Redirect("../CrewOffshore/CrewOffshoreDeBriefingSummary.aspx");
                }
                
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void DOAAdressBind()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = PhoenixCrewDeBriefing.EmployeeDOAAddress(General.GetNullableInteger((Request.QueryString["Signonoffid"].ToString())));

            if (dt.Rows.Count > 0)
            {
                ucdoa.Text = dt.Rows[0]["FLDDOADATE"].ToString();

                txtaddress1.Text = dt.Rows[0]["FLDADDRESS1"].ToString();
                txtaddress2.Text = dt.Rows[0]["FLDADDRESS2"].ToString();
                txtaddress3.Text = dt.Rows[0]["FLDADDRESS3"].ToString();
                txtaddress4.Text = dt.Rows[0]["FLDADDRESS4"].ToString();
                txtcountry.Text = dt.Rows[0]["FLDCOUNTRYNAME"].ToString();
                txtState.Text = dt.Rows[0]["FLDSTATENAME"].ToString();
                txtCity.Text = dt.Rows[0]["FLDCITYNAME"].ToString();
                txtPostalCode.Text = dt.Rows[0]["FLDPOSTALCODE"].ToString();
                //PermanentAddress.Address1 = dt.Rows[0]["FLDADDRESS1"].ToString();
                //PermanentAddress.Address2 = dt.Rows[0]["FLDADDRESS2"].ToString();
                //PermanentAddress.Address3 = dt.Rows[0]["FLDADDRESS3"].ToString();
                //PermanentAddress.Address4 = dt.Rows[0]["FLDADDRESS4"].ToString();
                //PermanentAddress.City = dt.Rows[0]["FLDCITY"].ToString();
                //PermanentAddress.State = dt.Rows[0]["FLDSTATE"].ToString();
                //PermanentAddress.Country = dt.Rows[0]["FLDCOUNTRY"].ToString();
                //PermanentAddress.PostalCode = dt.Rows[0]["FLDPOSTALCODE"].ToString();
                txtGeneralComments.Text= dt.Rows[0]["FLDGENERALCOMMENTS"].ToString();
                txtEmail.Text = dt.Rows[0]["FLDEMAIL"].ToString();

                txtPhoneNumber.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                txtPhoneNumber.Text = dt.Rows[0]["FLDPHONENUMBER"].ToString();
                //txtPhoneNumber2.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                //txtPhoneNumber2.Text = dt.Rows[0]["FLDPHONENUMBER2"].ToString();

                //txtMobileNumber.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                txtMobileNumber.Text = dt.Rows[0]["FLDMOBILENUMBER"].ToString();

                //txtMobileNumber2.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                //txtMobileNumber2.Text = dt.Rows[0]["FLDMOBILENUMBER2"].ToString();

                //txtMobileNumber3.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                //txtMobileNumber3.Text = dt.Rows[0]["FLDMOBILENUMBER3"].ToString();
                ////txtMobileNumber2.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                //txtMobileNumber2.Text = dt.Rows[0]["FLDMOBILENUMBER2"].ToString();
                txtSeaport.Text = dt.Rows[0]["FLDSEAPORTNAME"].ToString();
                //ddlPortofEngagement.SelectedSeaport = dt.Rows[0]["FLDPORTOFENGAGEMENT"].ToString();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = new DataTable();
            if (Request.QueryString["Signonoffid"] != null)
                dt = PhoenixCrewDeBriefing.EmployeeList(General.GetNullableInteger((Request.QueryString["Signonoffid"].ToString())));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDNAME"].ToString();
                txtFirstName.ToolTip= dt.Rows[0]["FLDNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtvesselName.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
                txtSignOnDate.Text = dt.Rows[0]["FLDSIGNONDATE"].ToString().Substring(0, 10);
                txtSignOffDate.Text = dt.Rows[0]["FLDSIGNOFFDATE"].ToString();

                if (dt.Rows[0]["FLDSIGNONRANKID"] != null)
                {
                    ViewState["Rankid"] = dt.Rows[0]["FLDSIGNONRANKID"].ToString();
                    ViewState["FLDRANKNAME"] = dt.Rows[0]["FLDRANKNAME"].ToString();
                }
                if (dt.Rows[0]["FLDEMPLOYEEID"] != null)
                {
                    ViewState["Empid"] = dt.Rows[0]["FLDEMPLOYEEID"].ToString();
                    ViewState["FLDNAME"] = dt.Rows[0]["FLDNAME"].ToString();
                }
                if (dt.Rows[0]["FLDVESSELID"] != null)
                {
                    ViewState["Vesselid"] = dt.Rows[0]["FLDVESSELID"].ToString();
                }

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
            if (!string.IsNullOrEmpty(ViewState["SignOnOffId"].ToString()))
            {
                string mnu = Filter.CurrentMenuCodeSelection;
                DataSet ds = PhoenixCrewDeBriefing.GetdeBriefingQuestions(General.GetNullableInteger(ViewState["Rankid"].ToString()));

                gvFeedBackQst.DataSource = ds.Tables[0];
              
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }





    protected void gvState_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            //UpdateState(
            //            Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblStateCodeEdit")).Text),
            //           General.GetNullableInteger(ddlcountrylist.SelectedCountry),
            //            ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtStateNameEdit")).Text,
            //             (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkActiveYNedit")).Checked) ? 1 : 0);
            //_gridView.EditIndex = -1;
            //BindData();
            //SetPageNavigator();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


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
        {
            return true;
        }

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
    private void DeleteState(int statecode)
    {
        PhoenixRegistersState.DeleteState(0, statecode);
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewFeedBackMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void txtDOA_TextChanged(object sender, EventArgs e)
    { }
    //private void InsertFeedBackDetails()
    //{
    //    try
    //    {

    //        int Permanentaddressid = PhoenixCrewDeBriefing.InsertEmployeeAddressandDOA(PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                                                                                , int.Parse(ViewState["Vesselid"].ToString())
    //                                                                                , int.Parse(ViewState["Empid"].ToString())
    //                                                                                , int.Parse(ViewState["Rankid"].ToString())
    //                                                                                , int.Parse(Request.QueryString["Signonoffid"].ToString())
    //                                                                                , General.GetNullableDateTime(ucdoa.Text)
    //                                                                                , PermanentAddress.Address1
    //                                                                                , General.GetNullableString(PermanentAddress.Address2)
    //                                                                                , General.GetNullableString(PermanentAddress.Address3)
    //                                                                                , General.GetNullableString(PermanentAddress.Address4)
    //                                                                                , General.GetNullableInteger(PermanentAddress.City)
    //                                                                                , General.GetNullableInteger(PermanentAddress.State)
    //                                                                                , Convert.ToInt32(PermanentAddress.Country)
    //                                                                                , PermanentAddress.PostalCode
    //                                                                                , General.GetNullableString(string.Empty)
    //                                                                                , txtPhoneNumber.Text
    //                                                                                , txtMobileNumber.Text
    //                                                                                , txtEmail.Text
    //                                                                                , txtPhoneNumber2.Text
    //                                                                                , txtMobileNumber2.Text
    //                                                                                , txtMobileNumber3.Text
    //                                                                                , General.GetNullableInteger(ddlPortofEngagement.SelectedSeaport)
    //                                                                                );
    //        string sXmlData = "<Return>";
    //        foreach (GridViewRow gv in gvFeedBackQst.Rows)
    //        {
    //            Label lblQuestionId = (Label)gv.FindControl("lblQuestionId");
    //            RadioButtonList rblOptions = (RadioButtonList)gv.FindControl("rblOptions");
    //            TextBox txtComments = (TextBox)gv.FindControl("txtComments");
    //            Label lblCategoryId = (Label)gv.FindControl("lblCategoryId");


    //            if (txtComments.Visible == true)
    //            {

    //                if (!IsValidatequestion(txtComments.Text))
    //                {
    //                    ucError.Visible = true;
    //                    return;
    //                }
    //            }


    //            if (!IsValidateoption(rblOptions.SelectedValue))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }


    //            sXmlData += "<Data CATEGORYID=\"" + lblCategoryId.Text.Trim() + "\"" +
    //                " QUESTIONID=\"" + lblQuestionId.Text.Trim() + "\"" +
    //                " OPTIONID=\"" + rblOptions.SelectedValue.Trim() + "\"" +
    //                " COMMENTS=\"" + txtComments.Text.Trim() + "\" />" + Environment.NewLine;
    //        }
    //        sXmlData += "</Return>" + Environment.NewLine;

    //        PhoenixCrewDeBriefing.InsertFeedBack(int.Parse(ViewState["Empid"].ToString())
    //       , int.Parse(ViewState["Rankid"].ToString())
    //       , int.Parse(ViewState["Vesselid"].ToString())
    //       , sXmlData
    //       , int.Parse(Request.QueryString["Signonoffid"].ToString()));

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    private bool IsValidatequestion(string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (remarks == null || remarks == "")
            ucError.ErrorMessage = "Comment is required";

        return (!ucError.IsError);
    }
    private bool IsValidateoption(string option)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (option == null || option == "")
            ucError.ErrorMessage = " please select the answers for all qustions";


        return (!ucError.IsError);
    }

    private bool IsValidate(string address1, string city, string state, string county
                           , string email, string mobilenumber, string prefix)
    {
        int employeecountry;
        ucError.HeaderMessage = "Please provide the following required information";

        if (ucdoa.Text == "" || ucdoa.Text == null)
            ucError.ErrorMessage = "Date Of Availability is required";

        if (address1.Trim() == "")
            ucError.ErrorMessage = prefix + " Address1 is required";

        if (!int.TryParse(city, out employeecountry))
            ucError.ErrorMessage = prefix + " City is required";

        //if (state.Trim() == "")
        //    ucError.ErrorMessage = "State is required";

        if (Int32.TryParse(county, out employeecountry) == false)
            ucError.ErrorMessage = prefix + " Country is required";

        if (email.Trim() == "")
            ucError.ErrorMessage = "E-Mail is required";
        else if (!General.IsvalidEmail(txtEmail.Text) && txtEmail.Text.ToUpper() != "NA")
        {
            ucError.ErrorMessage = "Please enter valid E-Mail";
        }

        //if ((string.IsNullOrEmpty(txtMobileNumber.Text) && string.IsNullOrEmpty(txtMobileNumber2.Text) && string.IsNullOrEmpty(txtMobileNumber3.Text))
        //       && (string.IsNullOrEmpty(txtPhoneNumber.Text) && string.IsNullOrEmpty(txtPhoneNumber2.Text)))
        //    ucError.ErrorMessage = "Mobile Number (or) Phone Number (Permanent) is required";

        return (!ucError.IsError);
    }
 
    private void BindDatacourse()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewRecommendedCourse.CrewRecommendedCourseSearch(General.GetNullableInteger(ViewState["Empid"].ToString())
                        , sortexpression, sortdirection
                        , (int)ViewState["PAGENUMBER"]
                        , gvCrewRecommendedCourses.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);

            gvCrewRecommendedCourses.DataSource = ds;
            gvCrewRecommendedCourses.VirtualItemCount = iRowCount;

            

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFeedBackQst_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvFeedBackQst_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            try
            {
                //RadLabel lblCommentsyn = (RadLabel)e.Item.FindControl("lblCommentsyn");
                HtmlTableRow trcomments = (HtmlTableRow)e.Item.FindControl("trcomments");
                //if (lblCommentsyn != null && lblCommentsyn.Text == "1")
                //{
                trcomments.Visible = true;
                //}
                //else
                //    trcomments.Visible = false;
                RadLabel lblQuestionId = (RadLabel)e.Item.FindControl("lblQuestionId");
                RadioButtonList rblOptions = (RadioButtonList)e.Item.FindControl("rblOptions");
                RadTextBox txtComments = (RadTextBox)e.Item.FindControl("txtComments");
                RadLabel lblRequirRemark = (RadLabel)e.Item.FindControl("lblRequirRemark");
                RadLabel lblcomment = (RadLabel)e.Item.FindControl("lblcomment");

                if (lblRequirRemark.Text.Trim() == "0")
                {
                    lblcomment.Visible = false;
                    txtComments.Visible = false;
                }

                DataSet ds = PhoenixCrewDeBriefing.GetdeBriefingoptions(General.GetNullableInteger(lblQuestionId.Text));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["FLDCONCERNREMARK"].ToString().Trim() == "1")
                        {
                            lblcomment.Text = "";
                            lblcomment.Text = "Comments (If " + dr["FLDOPTIONNAME"].ToString() + ")";
                            //txtComments.CssClass = "input_mandatory";
                        }

                    }
                }

                DataSet ds1 = PhoenixCrewDeBriefing.CrewSignoffFeedBackSearch(General.GetNullableInteger(Request.QueryString["Signonoffid"].ToString()));
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr1 in ds1.Tables[0].Rows)
                    {
                        if (lblQuestionId.Text.Trim() == dr1["FLDQUESTIONID"].ToString().Trim())
                        {
                            rblOptions.SelectedValue = dr1["FLDOPTIONID"].ToString().Trim();
                            txtComments.Text = dr1["FLDREMARKS"].ToString().Trim();
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
    }

    protected void gvCrewRecommendedCourses_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewRecommendedCourses.CurrentPageIndex + 1;
            BindDatacourse();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewRecommendedCourses_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        else if (e.CommandName.ToUpper() == "SELECT")
        {
            ViewState["nCurrentRow"] = e.Item.RowIndex;// Int32.Parse(e.CommandArgument.ToString());

            e.Item.Selected = true;
            //BindFEData();
            gvCrewRecommendedCourses.Rebind();
        }
    }

    protected void gvOffshorePCourseTraining_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOffshorePCourseTraining.CurrentPageIndex + 1;
            BindDataPTC();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOffshorePCourseTraining_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        else if (e.CommandName.ToUpper() == "SELECT")
        {
            ViewState["nCurrentRow"] = e.Item.RowIndex;
            e.Item.Selected = true;
            gvOffshorePCourseTraining.Rebind();
        }
    }

    protected void gvexpiredoc_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindExpiryDocument();
    }

    public void BindExpiryDocument()
    {
        try
        {
            if (!string.IsNullOrEmpty(ViewState["Empid"].ToString()))
            {
                string mnu = Filter.CurrentMenuCodeSelection;
                DataTable dt = PhoenixCrewDeBriefing.ExpiryDocumentList(General.GetNullableInteger(ViewState["Empid"].ToString()));

                gvexpiredoc.DataSource = dt;

            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

