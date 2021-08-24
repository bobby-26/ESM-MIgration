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
public partial class OptionsOffshoreCrewDeBriefing : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    if (!IsPostBack)
    //    {
    //        BindData();
    //        BindDataAppraisal();
    //    }
    //    if (gvFeedBackQst.Controls.Count > 0)
    //    {
    //        Table gridTable = (Table)gvFeedBackQst.Controls[0];
    //        ViewState["Category"] = "";
    //        foreach (GridViewRow gv in gvFeedBackQst.Rows)
    //        {
    //            Label lblCategory = (RadLabel)gv.FindControl("lblcategorynameG");
    //            string strCategory = lblCategory != null ? lblCategory.Text.Trim() : "";
    //            if (lblCategory != null)
    //            {
    //                if (ViewState["Category"].ToString() != strCategory)
    //                {
    //                    ViewState["Category"] = strCategory;
    //                    int rowIndex = gridTable.Rows.GetRowIndex(gv);
    //                    // Add new group header row  

    //                    GridViewRow headerRow = new GridViewRow(rowIndex, rowIndex, DataControlRowType.DataRow, DataControlRowState.Normal);

    //                    TableCell headerCell = new TableCell();

    //                    headerCell.ColumnSpan = gvFeedBackQst.Columns.Count;

    //                    headerCell.Text = @"<font size=""2"" ><b>" + string.Format("{0}", strCategory != null ? strCategory : "") + "</b></font>";

    //                    headerCell.CssClass = "GroupHeaderRowStyle";

    //                    // Add header Cell to header Row, and header Row to gridTable  

    //                    headerRow.Cells.Add(headerCell);
    //                    headerRow.HorizontalAlign = HorizontalAlign.Left;
    //                    gridTable.Controls.AddAt(rowIndex, headerRow);
    //                }
    //            }
    //        }
    //        base.Render(writer);
    //    }
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            toolbarmain.AddButton("Submit", "SAVEFEEDBACK", ToolBarDirection.Right);

            FeedBackTabs.AccessRights = this.ViewState;
            FeedBackTabs.Title = "De-Briefing";
            FeedBackTabs.MenuList = toolbarmain.Show();
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

               
                gvOffshorePCourseTraining.PageSize = 25;

            }
            //BindDataAppraisal();
            //BindData();
            //BindDataPTC();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvFeedBackQst_PreRender(object sender, EventArgs e)
    //{
    //    GridDecorator.MergeRows(gvFeedBackQst);
    //}
    public class GridDecorator
    {
        public static void MergeRows(GridView gridView)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

            }

        }
    }
    protected void FeedBackTabs_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVEFEEDBACK"))
            {
                if (!IsValidate(PermanentAddress.Address1, PermanentAddress.City, PermanentAddress.State, PermanentAddress.Country,
                       txtEmail.Text, txtMobileNumber.Text, "Permanent"))
                {
                    ucError.Visible = true;
                    return;
                }
                if (!IsValidatereview(lblreviewd.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                InsertFeedBackDetails();
                DOAAdressBind();

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
                lblreviewd.Text = dt.Rows[0]["FLDREVIEWEDBY"].ToString();
                ucdoa.Text = dt.Rows[0]["FLDDOADATE"].ToString();
                PermanentAddress.Address1 = dt.Rows[0]["FLDADDRESS1"].ToString();
                PermanentAddress.Address2 = dt.Rows[0]["FLDADDRESS2"].ToString();
                PermanentAddress.Address3 = dt.Rows[0]["FLDADDRESS3"].ToString();
                PermanentAddress.Address4 = dt.Rows[0]["FLDADDRESS4"].ToString();
                PermanentAddress.Country = dt.Rows[0]["FLDCOUNTRY"].ToString();
                PermanentAddress.State = dt.Rows[0]["FLDSTATE"].ToString();
                PermanentAddress.City = dt.Rows[0]["FLDCITY"].ToString();
                PermanentAddress.PostalCode = dt.Rows[0]["FLDPOSTALCODE"].ToString();

                txtEmail.Text = dt.Rows[0]["FLDEMAIL"].ToString();
                txtEmail.ToolTip = dt.Rows[0]["FLDEMAIL"].ToString();
                txtPhoneNumber.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                txtPhoneNumber.Text = dt.Rows[0]["FLDPHONENUMBER"].ToString();
                //txtPhoneNumber2.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                //txtPhoneNumber2.Text = dt.Rows[0]["FLDPHONENUMBER2"].ToString();

                txtMobileNumber.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                txtMobileNumber.Text = dt.Rows[0]["FLDMOBILENUMBER"].ToString();

                //txtMobileNumber2.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                //txtMobileNumber2.Text = dt.Rows[0]["FLDMOBILENUMBER2"].ToString();

                //txtMobileNumber3.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                //txtMobileNumber3.Text = dt.Rows[0]["FLDMOBILENUMBER3"].ToString();
                //txtMobileNumber2.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                //txtMobileNumber2.Text = dt.Rows[0]["FLDMOBILENUMBER2"].ToString();
                ddlPortofEngagement.SelectedSeaport = dt.Rows[0]["FLDPORTOFENGAGEMENT"].ToString();
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
                txtFirstName.ToolTip = dt.Rows[0]["FLDNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtvesselName.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
                txtSignOnDate.Text = dt.Rows[0]["FLDSIGNONDATE"].ToString().Substring(0, 10);
                txtSignOffDate.Text = dt.Rows[0]["FLDSIGNOFFDATE"].ToString();

                if (dt.Rows[0]["FLDSIGNONRANKID"] != null)
                {
                    ViewState["Rankid"] = dt.Rows[0]["FLDSIGNONRANKID"].ToString();
                }
                if (dt.Rows[0]["FLDEMPLOYEEID"] != null)
                {
                    ViewState["Empid"] = dt.Rows[0]["FLDEMPLOYEEID"].ToString();
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
    {
    }
    private void InsertFeedBackDetails()
    {
        try
        {
            string trainingneedid = "";
            foreach (GridDataItem gv in gvOffshorePCourseTraining.Items)
            {
                RadLabel lbltrainingneedid = (RadLabel)gv.FindControl("lbltrainingneedid");
                if (lbltrainingneedid != null)
                    trainingneedid = trainingneedid + ',' + lbltrainingneedid.Text.Trim();
            }

            int Permanentaddressid = PhoenixCrewDeBriefing.InsertEmployeeAddressandDOA(1
                                                                                    , int.Parse(ViewState["Empid"].ToString())
                                                                                    , int.Parse(Request.QueryString["Signonoffid"].ToString())
                                                                                    , General.GetNullableDateTime(ucdoa.Text)
                                                                                    , PermanentAddress.Address1
                                                                                    , General.GetNullableString(PermanentAddress.Address2)
                                                                                    , General.GetNullableString(PermanentAddress.Address3)
                                                                                    , General.GetNullableString(PermanentAddress.Address4)
                                                                                    , General.GetNullableInteger(PermanentAddress.City)
                                                                                    , General.GetNullableInteger(PermanentAddress.State)
                                                                                    , Convert.ToInt32(PermanentAddress.Country)
                                                                                    , PermanentAddress.PostalCode
                                                                                    , General.GetNullableString(string.Empty)
                                                                                    , txtPhoneNumber.Text
                                                                                    , txtMobileNumber.Text
                                                                                    , txtEmail.Text
                                                                                    //, txtPhoneNumber2.Text
                                                                                    //, txtMobileNumber2.Text
                                                                                    //, txtMobileNumber3.Text
                                                                                    , General.GetNullableInteger(ddlPortofEngagement.SelectedSeaport)
                                                                                    , null
                                                                                    , General.GetNullableString(trainingneedid)
                                                                                    );
            string sXmlData = "<Return>";
            foreach (GridDataItem gv in gvFeedBackQst.Items)
            {
                RadLabel lblQuestionId = (RadLabel)gv.FindControl("lblQuestionId");
                RadioButtonList rblOptions = (RadioButtonList)gv.FindControl("rblOptions");
                RadTextBox txtComments = (RadTextBox)gv.FindControl("txtComments");
                RadLabel lblCategoryId = (RadLabel)gv.FindControl("lblCategoryId");
                if (rblOptions != null)
                {

                    //if (txtComments.Visible == true && txtComments.CssClass == "input_mandatory")
                    //{

                    //    if (!IsValidatequestion(txtComments.Text))
                    //    {
                    //        ucError.Visible = true;
                    //        return;
                    //    }
                    //}


                    if (rblOptions.Items.Count > 0)
                    {
                        if (!IsValidateoption(rblOptions.SelectedValue))
                        {
                            ucError.Visible = true;
                            return;
                        }
                    }


                    sXmlData += "<Data CATEGORYID=\"" + lblCategoryId.Text.Trim() + "\"" +
                        " QUESTIONID=\"" + lblQuestionId.Text.Trim() + "\"" +
                        " OPTIONID=\"" + rblOptions.SelectedValue.Trim() + "\"" +
                        " COMMENTS=\"" + txtComments.Text.Trim() + "\" />" + Environment.NewLine;
                }
            }
            sXmlData += "</Return>" + Environment.NewLine;

            PhoenixCrewDeBriefing.InsertFeedBack(int.Parse(ViewState["Empid"].ToString())
          , int.Parse(ViewState["Rankid"].ToString())
          , int.Parse(ViewState["Vesselid"].ToString())
          , sXmlData
          , int.Parse(Request.QueryString["Signonoffid"].ToString())
          , 1
          );

            ucStatus.Text = "Updated Sucessfully";
            ucStatus.Visible = true;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
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
            ucError.ErrorMessage = "Please select the answers for all qustions";


        return (!ucError.IsError);
    }

    private bool IsValidatereview(string Reviewyn)
    {
        ucError.HeaderMessage = null;
        if (Reviewyn != "" && Reviewyn != null)
            ucError.ErrorMessage = "Can  not make changes in this De-Briefing, It is already reviewed by office";

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

        if (Int32.TryParse(county, out employeecountry) == false)
            ucError.ErrorMessage = prefix + " Country is required";

        if (email.Trim() == "")
            ucError.ErrorMessage = "E-Mail is required";
        else if (!General.IsvalidEmail(txtEmail.Text) && txtEmail.Text.ToUpper() != "NA")
        {
            ucError.ErrorMessage = "Please enter valid E-Mail";
        }

        if ((string.IsNullOrEmpty(txtMobileNumber.Text) && (string.IsNullOrEmpty(txtPhoneNumber.Text))))
            ucError.ErrorMessage = "Mobile Number (or) Phone Number (Permanent) is required";


        DataSet ds = PhoenixCrewDeBriefing.SearchPAppraisal(
             General.GetNullableInteger(ViewState["Empid"].ToString())
            , null
           );
        if(ds.Tables[0].Rows.Count >0)
            ucError.ErrorMessage = "Fill the seafarer comments for your appraisals.";

        return (!ucError.IsError);
    }
    //protected void gvFeedBackQst_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {

    //            HtmlTableRow trcomments = (HtmlTableRow)e.Row.FindControl("trcomments");

    //            trcomments.Visible = true;

    //            RadLabel lblQuestionId = (RadLabel)e.Row.FindControl("lblQuestionId");
    //            RadioButtonList rblOptions = (RadioButtonList)e.Row.FindControl("rblOptions");
    //            TextBox txtComments = (RadTextBox)e.Row.FindControl("txtComments");
    //            Label lblRequirRemark = (RadLabel)e.Row.FindControl("lblRequirRemark");
    //            Label lblcomment = (RadLabel)e.Row.FindControl("lblcomment");

    //            if (lblRequirRemark.Text.Trim() == "0")
    //            {
    //                lblcomment.Visible = false;
    //                txtComments.Visible = false;
    //            }

    //            DataSet ds = PhoenixCrewDeBriefing.GetdeBriefingoptions(General.GetNullableInteger(lblQuestionId.Text));
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                foreach (DataRow dr in ds.Tables[0].Rows)
    //                {
    //                    if (dr["FLDCONCERNREMARK"].ToString().Trim() == "1")
    //                    {
    //                        lblcomment.Text = "";
    //                        lblcomment.Text = "Comments (If " + dr["FLDOPTIONNAME"].ToString() + ")";
    //                        //txtComments.CssClass = "input_mandatory";
    //                    }

    //                }
    //            }

    //            DataSet ds1 = PhoenixCrewDeBriefing.CrewSignoffFeedBackSearch(General.GetNullableInteger(Request.QueryString["Signonoffid"].ToString()));
    //            if (ds1.Tables[0].Rows.Count > 0)
    //            {
    //                foreach (DataRow dr1 in ds1.Tables[0].Rows)
    //                {
    //                    if (lblQuestionId.Text.Trim() == dr1["FLDQUESTIONID"].ToString().Trim())
    //                    {
    //                        rblOptions.SelectedValue = dr1["FLDOPTIONID"].ToString().Trim();
    //                        txtComments.Text = dr1["FLDREMARKS"].ToString().Trim();
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
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
    protected void gvOffshorePCourseTraining_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
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
                    General.ShowRecords(null),
                    ref iRowCount,
                    ref iTotalPageCount,
                    null,
                    null,
                    null,
                    1,
                    General.GetNullableInteger(ViewState["Trainingcoursetype"].ToString())
                    , 1
                    );

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvOffshorePCourseTraining", "Pending Training Course", alCaptions, alColumns, ds);
            gvOffshorePCourseTraining.DataSource = ds;
            gvOffshorePCourseTraining.VirtualItemCount = iRowCount;
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDataAppraisal()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDEMPLOYEEID", "FLDAPPRAISALID", "FLDVESSELNAME", "FLDFROMDATE", "FLDTODATE", "FLDOCCASSIONFORREPORT", "FLDMASTERCOMMENT", "FLDHEADDEPTCOMMENT", "FLDSEAMANCOMMENT", };
            string[] alCaptions = { "Employeeid", "AppraisalID", "Vessel", "From", "To", "Occassion","mastercomment", "HODcomment", "Seamancomment", };
            int? sortdirection = 1; //DEFAULT DESC SORT
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            {
               // ViewState["VSLID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

                DataSet ds = PhoenixCrewDeBriefing.SearchPAppraisal(
                     General.GetNullableInteger(ViewState["Empid"].ToString())
                    , null
                   );
                General.SetPrintOptions("gvOffshorePappraisal", "Crew Appraisal", alCaptions, alColumns, ds);
                gvOffshorePappraisal.DataSource = ds.Tables[0];
             
                ViewState["ROWCOUNT"] = iRowCount;
                ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            }
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

    protected void gvOffshorePappraisal_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataAppraisal();
    }

    protected void gvOffshorePappraisal_ItemCommand(object sender, GridCommandEventArgs e)
    {
        //GridView _gridView = (GridView)sender;
        //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        try
        {
            RadLabel lblAppraisalId = (RadLabel)e.Item.FindControl("lblAppraisalId");
            RadLabel lblCrewAppraisalId = (RadLabel)e.Item.FindControl("lblCrewAppraisalId");
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string seafarercomment = ((RadTextBox)e.Item.FindControl("txtSeafarerComment")).Text;

                PhoenixCrewAppraisal.UpdateAppraisalSeamanComment(1,
                   new Guid(lblCrewAppraisalId.Text)
                   , seafarercomment);
            }
            //if (e.CommandName.ToUpper().Equals("VIEWACTIVITY"))
            //{
            //    ImageButton cmdCrewExport2XL = (ImageButton)e.Item.FindControl("cmdCrewExport2XL");

            //    cmdCrewExport2XL.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Options/OptionOffshoreAppraisalDetailforDebrifing.aspx?aprid=" + lblCrewAppraisalId.Text + "');");

            //    //String scriptpopup = String.Format("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Options/OptionOffshoreAppraisalDetailforDebrifing.aspx?aprid=" + lblCrewAppraisalId.Text + " ');");
            //    //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            //}

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        gvOffshorePappraisal.Rebind();
        //gvOffshorePappraisal.EditIndex = -1;
        //BindDataAppraisal();
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

    protected void gvOffshorePappraisal_ItemDataBound(object sender, GridItemEventArgs e)
    {
        ImageButton cmdCrewExport2XL = (ImageButton)e.Item.FindControl("cmdCrewExport2XL");
        RadLabel lblCrewAppraisalId = (RadLabel)e.Item.FindControl("lblCrewAppraisalId");
        if (cmdCrewExport2XL!=null)
            cmdCrewExport2XL.Attributes.Add("onclick", "openNewWindow('codehelp1','','../Options/OptionOffshoreAppraisalDetailforDebrifing.aspx?aprid=" + lblCrewAppraisalId.Text + "', false, null, null, true);return false;");

    }
}
