using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOperation;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Data;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class Crew_CrewSignoffFeedback : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE");
        toolbarmain.AddButton("Back", "APPRAISAL");
        CrewFeedBackMain.AccessRights = this.ViewState;
        CrewFeedBackMain.MenuList = toolbarmain.Show();
        if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
        {
            Filter.CurrentCrewSelection = Request.QueryString["empid"];
        }
        if (!IsPostBack)
        {
            ViewState["VESSELID"] = string.Empty;
            ViewState["RANKID"] = string.Empty;
            ViewState["FLDSIGNONOFFID"] = string.Empty;
            ViewState["FLDEMPLOYEEID"] = string.Empty;
            SetEmployeePrimaryDetails();
        }
        BindData();
    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dtEmployee = PhoenixVesselAccountsCrewFeedback.VesselCrewSignonDetailsEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , PhoenixSecurityContext.CurrentSecurityContext.VesselID);

            if (dtEmployee.Rows.Count > 0)
            {
                ViewState["RANKID"] = dtEmployee.Rows[0]["FLDRANKID"].ToString();
                DataRow drEmp = dtEmployee.Rows[0];
                if (General.GetNullableInteger(drEmp["FLDSIGNONOFFID"].ToString()) != null)
                {
                    ViewState["FLDSIGNONOFFID"] = drEmp["FLDSIGNONOFFID"].ToString();
                    ViewState["FLDEMPLOYEEID"] = drEmp["FLDEMPLOYEEID"].ToString();
                    DataTable dt = PhoenixCrewSignOffFeedBack.VesselCrewFeedbackList(General.GetNullableInteger(drEmp["FLDVESSELID"].ToString())
                        , General.GetNullableInteger(drEmp["FLDEMPLOYEEID"].ToString())
                        , General.GetNullableInteger(drEmp["FLDSIGNONOFFID"].ToString()));

                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        txtName.Text = dr["FLDNAME"].ToString();
                        txtRank.Text = dr["FLDRANKNAME"].ToString();
                        txtVessel.Text = dr["FLDVESSELNAME"].ToString();
                        txtSignonDate.Text = dr["FLDSIGNONDATE"].ToString();

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
    private void BindData()
    {
        try
        {
            string mnu = Filter.CurrentMenuCodeSelection;
            DataSet ds = PhoenixCrewSignOffFeedBack.GetRankWiseFeedBackQuestions(General.GetNullableInteger(ViewState["RANKID"].ToString()));
               
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

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                InsertFeedBackDetails();
                ucStatus.Text = "FeedBack Updated Sucessfully";
                ucStatus.Visible = true;
            }
            else if (dce.CommandName.ToUpper().Equals("APPRAISAL"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsEmployeeAppraisalQuery.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void InsertFeedBackDetails()
    {
        string sXmlData = "<Return>";
        foreach (GridViewRow gv in gvFeedBackQst.Rows)
        {
            Label lblQuestionId = (Label)gv.FindControl("lblQuestionId");
            RadioButtonList rblOptions = (RadioButtonList)gv.FindControl("rblOptions");
            TextBox txtComments = (TextBox)gv.FindControl("txtComments");
            Label lblOrder=(Label)gv.FindControl("lblOrder");
            sXmlData +="<Data ORDERNO=\"" + lblOrder.Text.Trim() + "\"" +
                " QUESTIONID=\"" + lblQuestionId.Text.Trim() + "\"" +
                " OPTIONID=\"" + rblOptions.SelectedValue.Trim() + "\"" +
                " COMMENTS=\"" + txtComments.Text.Trim() + "\" />" + Environment.NewLine;
        }
        sXmlData += "</Return>" + Environment.NewLine;
        PhoenixCrewSignOffFeedBack.InsertFeedBack(General.GetNullableInteger(ViewState["FLDEMPLOYEEID"].ToString())
            , General.GetNullableInteger(ViewState["RANKID"].ToString()), General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
            , sXmlData,General.GetNullableInteger(ViewState["FLDSIGNONOFFID"].ToString()));
    }
    protected void gvFeedBackQst_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblCommentsyn = (Label)e.Row.FindControl("lblCommentsyn");
            HtmlTableRow trcomments = (HtmlTableRow)e.Row.FindControl("trcomments");
            if (lblCommentsyn != null && lblCommentsyn.Text == "1")
            {
                trcomments.Visible = true;
            }
            else
                trcomments.Visible = false;
            Label lblQuestionId = (Label)e.Row.FindControl("lblQuestionId");
            RadioButtonList rblOptions = (RadioButtonList)e.Row.FindControl("rblOptions");
            TextBox txtComments = (TextBox)e.Row.FindControl("txtComments");
            DataSet ds = PhoenixCrewSignOffFeedBack.CrewSignoffFeedBackSearch(General.GetNullableInteger(ViewState["FLDEMPLOYEEID"].ToString())
                , General.GetNullableInteger(ViewState["FLDSIGNONOFFID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (lblQuestionId.Text.Trim() == dr["FLDQUESTIONID"].ToString().Trim())
                    {
                        rblOptions.SelectedValue = dr["FLDOPTIONID"].ToString().Trim();
                        txtComments.Text = dr["FLDCOMMENTS"].ToString().Trim();
                    }
                }
            }
           
        }
    }
}
