using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Framework;

public partial class PreSeaBatchPlanExamDetails : PhoenixBasePage
{
    string strBatchId;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        try
        {
            PhoenixToolbar MainToolbar = new PhoenixToolbar();

            MainToolbar.AddButton("List", "BATCH");
            MainToolbar.AddButton("Details", "DETAIL");
            MainToolbar.AddButton("Entrance Exam Plan", "ENTRANCE");

            MenuBatchPlanner.AccessRights = this.ViewState;
            //MenuBatchPlanner.MenuList = MainToolbar.Show();

            //MenuBatchPlanner.SelectedMenuIndex = 2;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE");
            MenuPreSea.AccessRights = this.ViewState;
            MenuPreSea.MenuList = toolbar.Show();
            strBatchId = Filter.CurrentPreSeaCourseMasterBatchSelection;
            if (!IsPostBack)
            {
                ResetFields();
                ViewState["EXAMPLANID"] = String.Empty;
                ViewState["EXAMVENUEID"] = String.Empty;
                if (Request.QueryString["ExamPlanId"] != null)
                    ViewState["EXAMPLANID"] = Request.QueryString["ExamPlanId"].ToString();
                if (Request.QueryString["ExamVenueID"] != null)
                    ViewState["EXAMVENUEID"] = Request.QueryString["ExamVenueID"].ToString();
                BindExamVenue();
                SetPrimaryBatchDetails();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void BatchPlanner_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            if (String.IsNullOrEmpty(Filter.CurrentPreSeaCourseMasterBatchSelection))
            {
                ucError.ErrorMessage = "Please select batch to view/modify plan details.";
                ucError.Visible = true;
                return;
            }
            else
            {

                DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
                if (dce.CommandName.ToUpper().Equals("BATCH"))
                {
                    Response.Redirect("../PreSea/PreSeaBatch.aspx");
                }
                else if (dce.CommandName.ToUpper().Equals("DETAIL"))
                {
                    Response.Redirect("../PreSea/PreSeaBatchPlanDetails.aspx");
                }
                else if (dce.CommandName.ToUpper().Equals("ENTRANCE"))
                {
                    Response.Redirect("../PreSea/PreSeaBatchPlanExamDetails.aspx");
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlExamVenue_changed(object sender, EventArgs e)
    {
        DataTable dt = PhoenixPreSeaExamVenue.EditExamVenue(int.Parse(ddlExamVenue.SelectedValue));
        txtContactPerson.Text = dt.Rows[0]["FLDCONTACTPERSONNAME"].ToString();
        txtContactNos.Text = dt.Rows[0]["FLDCONTACTPERSONMOBILE"].ToString();
        txtContactMail.Text = dt.Rows[0]["FLDCONTACTPERSONMAIL"].ToString();
        txtZoneName.Text = dt.Rows[0]["FLDZONE"].ToString();
        txtVenueAddress.Text = dt.Rows[0]["FLDVENUEADDRESS"].ToString();
    }
    protected void BindExamVenue()
    {
        ddlExamVenue.Items.Clear();
        ddlExamVenue.Items.Insert(0, new ListItem("--Select--", "Dummy"));
        DataSet ds = PhoenixPreSeaExamVenue.SearchExamVenueList();
        ddlExamVenue.DataSource = ds;
        ddlExamVenue.DataValueField = "FLDEXAMVENUEID";
        ddlExamVenue.DataTextField = "FLDEXAMVENUENAME";
        ddlExamVenue.DataBind();

        if (ViewState["EXAMVENUEID"].ToString() != null && ViewState["EXAMVENUEID"].ToString()!= "")
        {
            ddlExamVenue.SelectedValue = ViewState["EXAMVENUEID"].ToString();
            ddlExamVenue.Enabled = false;
            DataTable dt = PhoenixPreSeaExamVenue.EditExamVenue(int.Parse(ddlExamVenue.SelectedValue));
            txtZoneName.Text = dt.Rows[0]["FLDZONE"].ToString();
            txtVenueAddress.Text = dt.Rows[0]["FLDVENUEADDRESS"].ToString();
            txtContactPerson.Text = dt.Rows[0]["FLDCONTACTPERSONNAME"].ToString();
            txtContactNos.Text = dt.Rows[0]["FLDCONTACTPERSONMOBILE"].ToString();
            txtContactMail.Text = dt.Rows[0]["FLDCONTACTPERSONMAIL"].ToString();
        }
    }
    protected void MenuPreSea_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidExamPlanDetails())
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["EXAMPLANID"].ToString() == "")
                {
                    PhoenixPreSeaBatchPlanner.InsertBatchEntranceExamPlan(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                       , int.Parse(strBatchId)
                                                                       , int.Parse(ddlExamVenue.SelectedValue)
                                                                       , DateTime.Parse(txtStartDate.Text)
                                                                       , General.GetNullableInteger(txtNoofdays.Text)
                                                                       , General.GetNullableDecimal(txtMedfees.Text)
                                                                       , txtContactPerson.Text
                                                                       , txtContactNos.Text
                                                                       , txtContactMail.Text                                                                     
                                                                       , 1);                  
                  
                }
                else
                {
                    PhoenixPreSeaBatchPlanner.UpdateBatchEntranceExamPlan(General.GetNullableInteger(ViewState["EXAMPLANID"].ToString())
                                                                        , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , int.Parse(strBatchId)
                                                                        , int.Parse(ddlExamVenue.SelectedValue)
                                                                        , DateTime.Parse(txtStartDate.Text)
                                                                        , General.GetNullableInteger(txtNoofdays.Text)
                                                                        , General.GetNullableDecimal(txtMedfees.Text)
                                                                        , txtContactPerson.Text                                                                       
                                                                        , txtContactNos.Text
                                                                        , txtContactMail.Text
                                                                        , 1);
               
                    
                }
                ucStatus.Text = "Exam Plan updated Successfully.";
                string Script = "";
                Script += "<script language='JavaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1', 'ifMoreInfo');";
                Script += "</script>" + "\n";

                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", scriptpopupclose, false);
                ScriptManager.RegisterStartupScript(this,this.GetType(), "BookMarkScript", Script,false);  
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetPrimaryBatchDetails()
    {
        try
        {
            DataSet ds = PhoenixPreSeaBatch.EditBatch(int.Parse(strBatchId));

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtCourse.Text = ds.Tables[0].Rows[0]["FLDPRESEACOURSENAME"].ToString();
                txtBatchName.Text = ds.Tables[0].Rows[0]["FLDBATCH"].ToString();
            }
            if (ViewState["EXAMPLANID"].ToString() != "" && ViewState["EXAMPLANID"].ToString() != null)
            {
                DataTable dt = PhoenixPreSeaBatchPlanner.EditBatchEntranceExamPlan(int.Parse(ViewState["EXAMPLANID"].ToString()));
                txtStartDate.Text = dt.Rows[0]["FLDSTARTDATE"].ToString();
                txtMedfees.Text = dt.Rows[0]["FLDMEDICALFEES"].ToString();
                txtNoofdays.Text = dt.Rows[0]["FLDNOFDAYS"].ToString();              

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetExamPlanDetails(int? venueid)
    {
        if (venueid == null)
            ResetFields();
        else
        {

            DataTable dt = PhoenixPreSeaBatchPlanner.EditBatchEntranceExamPlan(int.Parse(ViewState["EXAMPLANID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ViewState["EXAMPLANID"] = dr["FLDENTRANCEEXAMPLANID"].ToString();
                ddlExamVenue.SelectedValue = dr["FLDEXAMVENUE"].ToString();
                txtStartDate.Text = dr["FLDSTARTDATE"].ToString();
                txtNoofdays.Text = dr["FLDNOFDAYS"].ToString();
                txtContactPerson.Text = dr["FLDCONTACTPESON"].ToString();
                txtContactMail.Text = dr["FLDCONTACTMAIL"].ToString();
                txtContactNos.Text = dr["FLDCONTACTNUMBERS"].ToString();
                txtMedfees.Text = dr["FLDMEDICALFEES"].ToString();               
                //imgbtnTiming.Visible = true;
                
            }
            else
            {
                // Set Default Contact Details
                DataTable dtVenue = PhoenixPreSeaExamVenue.EditExamVenue(int.Parse(venueid.ToString()));
                if (dtVenue.Rows.Count > 0)
                {
                    ViewState["EXAMPLANID"] = "";
                    DataRow drVenue = dtVenue.Rows[0];
                    ddlExamVenue.SelectedValue = drVenue["FLDEXAMVENUE"].ToString();
                    txtContactPerson.Text = drVenue["FLDCONTACTPERSONNAME"].ToString();
                    txtContactMail.Text = drVenue["FLDCONTACTPERSONMAIL"].ToString();
                    string phone = drVenue["FLDCONTACTPERSONPHONE"].ToString();
                    string mobile = drVenue["FLDCONTACTPERSONMOBILE"].ToString();
                    txtContactNos.Text = String.IsNullOrEmpty(phone) ? "" : phone + "," + mobile;
                    txtContactNos.Text = txtContactNos.Text.TrimEnd(',');                 
                    //imgbtnTiming.Visible = false;
                   
                }
            }
        }
    }

    private void ResetFields()
    {
        string empty = String.Empty;
        ddlExamVenue.SelectedValue = "Dummy";
        txtStartDate.Text = empty;
        txtNoofdays.Text = empty;
        txtContactPerson.Text = empty;
        txtContactMail.Text = empty;
        txtContactNos.Text = empty;
        txtMedfees.Text = empty;

    }

 

    public bool IsValidExamPlanDetails()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime dt;
        if (General.GetNullableInteger(strBatchId) == null)
            ucError.ErrorMessage = "Select a Batch from Batch Planner";
        if (string.IsNullOrEmpty(txtStartDate.Text))
            ucError.ErrorMessage = "Exam Start Date is required.";
        else if (DateTime.TryParse(txtStartDate.Text, out dt) == false)
            ucError.ErrorMessage = "Exam Start Date is not in correct format";               
        if (string.IsNullOrEmpty(txtNoofdays.Text))
            ucError.ErrorMessage = "No of days is required.";
        if (String.IsNullOrEmpty(txtMedfees.Text))
            ucError.ErrorMessage = "Medical fee is required.";
        if (General.GetNullableInteger(ddlExamVenue.SelectedValue) == null)
            ucError.ErrorMessage = "Exam venue is required.";
        return (!ucError.IsError);

    }
}
