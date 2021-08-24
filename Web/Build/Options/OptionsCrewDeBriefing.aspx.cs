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

public partial class OptionsCrewDeBriefing : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        if (!IsPostBack)
        {
            BindData();
        }
        if (gvFeedBackQst.Controls.Count > 0)
        {
            Table gridTable = (Table)gvFeedBackQst.Controls[0];
            ViewState["Category"] = "";
            foreach (GridViewRow gv in gvFeedBackQst.Rows)
            {
                Label lblCategory = (Label)gv.FindControl("lblcategorynameG");
                string strCategory = lblCategory != null ? lblCategory.Text.Trim() : "";
                if (lblCategory != null)
                {
                    if (ViewState["Category"].ToString() != strCategory)
                    {
                        ViewState["Category"] = strCategory;
                        int rowIndex = gridTable.Rows.GetRowIndex(gv);
                        // Add new group header row  

                        GridViewRow headerRow = new GridViewRow(rowIndex, rowIndex, DataControlRowType.DataRow, DataControlRowState.Normal);

                        TableCell headerCell = new TableCell();

                        headerCell.ColumnSpan = gvFeedBackQst.Columns.Count;

                        headerCell.Text = @"<font size=""2"" ><b>" + string.Format("{0}", strCategory != null ? strCategory : "") + "</b></font>";

                        headerCell.CssClass = "GroupHeaderRowStyle";

                        // Add header Cell to header Row, and header Row to gridTable  

                        headerRow.Cells.Add(headerCell);
                        headerRow.HorizontalAlign = HorizontalAlign.Left;
                        gridTable.Controls.AddAt(rowIndex, headerRow);
                    }
                }
            }
            base.Render(writer);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Submit", "SAVEFEEDBACK");
            FeedBackTabs.AccessRights = this.ViewState;
            FeedBackTabs.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {

                ViewState["PAGENUMBERPTC"] = 1;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SignOnOffId"] = string.IsNullOrEmpty(Request.QueryString["signonoffid"]);
                
                divPrimarySection.Visible = true;
                ViewState["Trainingcoursetype"] = PhoenixCommonRegisters.GetHardCode(1, 103, "7");
                ucdoa.Text = string.Empty;
                SetEmployeePrimaryDetails();
                DOAAdressBind();
            }

            BindData();
            BindDatacourse();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlApprovalType_TextChanged(object sender, EventArgs e)
    {
        DropDownList dc = (DropDownList)sender;
        TextBox txtRemarks;
        GridViewRow row = ((GridViewRow)dc.Parent.Parent);

        txtRemarks = (TextBox)gvCrewRecommendedCourses.Rows[row.RowIndex].FindControl("txtRemarks");
        if (dc.SelectedValue == "0")
        {

            txtRemarks.CssClass = "input_mandatory";
        }
        else
        {
            txtRemarks.CssClass = "input";
        }
    }
    protected void gvFeedBackQst_PreRender(object sender, EventArgs e)
    {
        GridDecorator.MergeRows(gvFeedBackQst);
        
    }
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
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVEFEEDBACK"))
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
                txtGeneralComments.Text = dt.Rows[0]["FLDGENERALCOMMENTS"].ToString();
                txtEmail.Text = dt.Rows[0]["FLDEMAIL"].ToString();
                txtEmail.ToolTip = dt.Rows[0]["FLDEMAIL"].ToString();

                txtPhoneNumber.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                txtPhoneNumber.Text = dt.Rows[0]["FLDPHONENUMBER"].ToString();
                
                //txtMobileNumber.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                txtMobileNumber.Text = dt.Rows[0]["FLDMOBILENUMBER"].ToString();
                ddlPortofEngagement.SelectedSeaport = dt.Rows[0]["FLDPORTOFENGAGEMENT"].ToString();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidatereview(string Reviewyn)
    {
        ucError.HeaderMessage = null;
        if (Reviewyn != "" && Reviewyn != null)
            ucError.ErrorMessage = "Can  not make changes in this De-Briefing, It is already reviewed by office";

        return (!ucError.IsError);
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

    //protected void RegistersState_TabStripCommand(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
    //        //if (dce.CommandName.ToUpper().Equals("FIND"))
    //        //{
    //        //    gvState.EditIndex = -1;
    //        //    gvState.SelectedIndex = -1;
    //        //    ViewState["PAGENUMBER"] = 1;
    //        //    BindData();
    //        //    SetPageNavigator();
    //        //}
    //        //if (dce.CommandName.ToUpper().Equals("EXCEL"))
    //        //{
    //        //    ShowExcel();
    //        //}
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    private void BindData()
    {
        try
        {
            if (!string.IsNullOrEmpty(ViewState["SignOnOffId"].ToString()))
            {
                string mnu = Filter.CurrentMenuCodeSelection;
                DataSet ds = PhoenixCrewDeBriefing.GetdeBriefingQuestions(General.GetNullableInteger(ViewState["Rankid"].ToString()));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvFeedBackQst.DataSource = ds.Tables[0];
                    gvFeedBackQst.DataBind();
                }
                else
                {
                    DataTable dt = ds.Tables[0];
                    ShowNoRecordsFound(dt, gvFeedBackQst);
                }
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




    //private bool IsValidState(string statename)
    //{
    //    ucError.HeaderMessage = "Please provide the following required information";
    //    //Int16 result;
    //    GridView _gridView = gvState;

    //    //if (statename.Trim().Equals(""))
    //    //    ucError.ErrorMessage = "State Name is required.";

    //    //if (ddlcountrylist.SelectedCountry == "" || !Int16.TryParse(ddlcountrylist.SelectedCountry, out result))
    //    //    ucError.ErrorMessage = "First select a country";


    //    return (!ucError.IsError);
    //}

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
    private void InsertFeedBackDetails()
    {
        try
        {
            string dtkey = "";
            foreach (GridViewRow gv in gvCrewRecommendedCourses.Rows)
            {
                Label lblDtkey = (Label)gv.FindControl("lblDtkey");
                if (lblDtkey!=null)
                dtkey = dtkey + ',' + lblDtkey.Text.Trim();
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
                                                                                    , General.GetNullableString(dtkey)
                                                                                    , null
                                                                                    , General.GetNullableString(txtGeneralComments.Text)
                                                                                    );
            string sXmlData = "<Return>";
            foreach (GridViewRow gv in gvFeedBackQst.Rows)
            {
                Label lblQuestionId = (Label)gv.FindControl("lblQuestionId");
                RadioButtonList rblOptions = (RadioButtonList)gv.FindControl("rblOptions");
                TextBox txtComments = (TextBox)gv.FindControl("txtComments");
                Label lblCategoryId = (Label)gv.FindControl("lblCategoryId");
                if (rblOptions != null)
                {

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
           ,1
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

        if ((string.IsNullOrEmpty(txtMobileNumber.Text))
               && (string.IsNullOrEmpty(txtPhoneNumber.Text) ))
            ucError.ErrorMessage = "Mobile Number (or) Phone Number (Permanent) is required";

        return (!ucError.IsError);
    }
    protected void gvFeedBackQst_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                //Label lblCommentsyn = (Label)e.Row.FindControl("lblCommentsyn");
                HtmlTableRow trcomments = (HtmlTableRow)e.Row.FindControl("trcomments");
                //if (lblCommentsyn != null && lblCommentsyn.Text == "1")
                //{
                trcomments.Visible = true;
                //}
                //else
                //    trcomments.Visible = false;
                Label lblQuestionId = (Label)e.Row.FindControl("lblQuestionId");
                RadioButtonList rblOptions = (RadioButtonList)e.Row.FindControl("rblOptions");
                TextBox txtComments = (TextBox)e.Row.FindControl("txtComments");
                Label lblRequirRemark = (Label)e.Row.FindControl("lblRequirRemark");
                Label lblcomment = (Label)e.Row.FindControl("lblcomment");

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
                        , General.ShowRecords(null)
                        , ref iRowCount
                        , ref iTotalPageCount);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCrewRecommendedCourses.DataSource = ds;
                gvCrewRecommendedCourses.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvCrewRecommendedCourses);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

