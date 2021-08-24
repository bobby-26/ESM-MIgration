using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.VesselAccounts;
using System.Data;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class Crew_CrewSignoffFeedBackOfficeAdd : PhoenixBasePage
{
    //string canedit = "0";
    string mode = "ADD";
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
       

        if (!IsPostBack)
        {
            try
            {
                ViewState["Rankid"] = "";
                ViewState["SignOnOffId"] = "";
                ViewState["canedit"] = "0";

                ddlVessel.SelectedVessel = string.IsNullOrEmpty(Request.QueryString["VesselId"]) ? "" : Request.QueryString["VesselId"];
                txtSignOnDate.Text = string.IsNullOrEmpty(Request.QueryString["SignOnDate"]) ? "" : Request.QueryString["SignOnDate"];
                ViewState["SignOnOffId"] = string.IsNullOrEmpty(Request.QueryString["SignonoffId"]) ? "" : Request.QueryString["SignonoffId"];
                ViewState["Rankid"] = string.IsNullOrEmpty(Request.QueryString["Rankid"]) ? "" : Request.QueryString["Rankid"];
             
                SetEmployeePrimaryDetails();
               
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
        mode = string.IsNullOrEmpty(Request.QueryString["mode"]) ? "ADD" : Request.QueryString["mode"];
        if (ViewState["canedit"].ToString().Equals("1") && mode.ToUpper().Equals("ADD"))
        {
            toolbarmain.AddButton("Save Feedback", "SAVEFEEDBACK",ToolBarDirection.Right);
            dvFeedBackQst.Visible = true;
            divPrimarySection.Visible = true;
            FeedBackTabs.AccessRights = this.ViewState;
            FeedBackTabs.MenuList = toolbarmain.Show();
        }
        else if (mode.ToUpper().Equals("ADD"))
        {
            toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
            dvFeedBackQst.Visible = false;
            divPrimarySection.Visible = true;
            FeedBackTabs.AccessRights = this.ViewState;
            FeedBackTabs.MenuList = toolbarmain.Show();
        }
        if (mode.ToUpper().Equals("EDIT"))
        {
            toolbarmain.AddButton("Save Feedback", "SAVEFEEDBACK",ToolBarDirection.Right);
            dvFeedBackQst.Visible = true;
            divPrimarySection.Visible = true;
            FeedBackTabs.AccessRights = this.ViewState;
            FeedBackTabs.MenuList = toolbarmain.Show();
        }

        BindData();
    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKPOSTEDNAME"].ToString();
                ddlVessel.SelectedVessel = string.IsNullOrEmpty(Request.QueryString["VesselId"]) ? "" : Request.QueryString["VesselId"];
                txtSignOnDate.Text = string.IsNullOrEmpty(Request.QueryString["SignOnDate"]) ? "" : Request.QueryString["SignOnDate"];
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
                InsertFeedBackDetails();
                ucStatus.Text = "FeedBack Updated Sucessfully";
                ucStatus.Visible = true;
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string sSignonDate = string.IsNullOrEmpty(txtSignOnDate.Text) ? "" : txtSignOnDate.Text;
                if (!IsValidFilters(ddlVessel.SelectedVessel, sSignonDate))
                {
                    ucError.Visible=true;
                    return;
                }
                DataSet ds = PhoenixCrewSignoffFeedBackDetail.SearchCrewSignonoffDetails(General.GetNullableInteger(Filter.CurrentCrewSelection)
                            , General.GetNullableInteger(ddlVessel.SelectedVessel)
                            , General.GetNullableDateTime(txtSignOnDate.Text));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["SignOnOffId"] = ds.Tables[0].Rows[0]["FLDSIGNONOFFID"].ToString();
                    ViewState["Rankid"]=ds.Tables[0].Rows[0]["FLDSIGNONRANKID"].ToString();
                }
                ViewState["canedit"] = "1";
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Save Feedback", "SAVEFEEDBACK",ToolBarDirection.Right);
                dvFeedBackQst.Visible = true;
                divPrimarySection.Visible = true;
                FeedBackTabs.AccessRights = this.ViewState;
                FeedBackTabs.MenuList = toolbarmain.Show();
                gvFeedBackQst.Rebind();
                ucStatus.Text = "FeedBack Updated Sucessfully";
                ucStatus.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidFilters(string VesselId, string SignonDate)
    {
        ucError.HeaderMessage = "Please Provide Following Information.";
        if (General.GetNullableInteger(VesselId.Trim()) == null)
        {
            ucError.ErrorMessage = "Please Select Vessel Name";
        }
        if (General.GetNullableDateTime(SignonDate.Trim()) == null)
        {
            ucError.ErrorMessage = "Please Provide SignOn Date";
        }
        return (!ucError.IsError);
    }
  
    private void BindData()
    {
        try
        {
            if (!string.IsNullOrEmpty(ViewState["SignOnOffId"].ToString()))
            {
                string mnu = Filter.CurrentMenuCodeSelection;
                DataSet ds = PhoenixCrewSignOffFeedBack.GetRankWiseFeedBackQuestions(General.GetNullableInteger(ViewState["Rankid"].ToString()));

                gvFeedBackQst.DataSource = ds.Tables[0];
               
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
        foreach (GridDataItem gv in gvFeedBackQst.Items)
        {
            RadLabel lblQuestionId = (RadLabel)gv.FindControl("lblQuestionId");
            RadioButtonList rblOptions = (RadioButtonList)gv.FindControl("rblOptions");
            RadTextBox txtComments = (RadTextBox)gv.FindControl("txtComments");
            RadLabel lblOrder = (RadLabel)gv.FindControl("lblOrder");
            sXmlData += "<Data ORDERNO=\"" + lblOrder.Text.Trim() + "\"" +
                " QUESTIONID=\"" + lblQuestionId.Text.Trim() + "\"" +
                " OPTIONID=\"" + rblOptions.SelectedValue.Trim() + "\"" +
                " COMMENTS=\"" + txtComments.Text.Trim() + "\" />" + Environment.NewLine;
        }
        sXmlData += "</Return>" + Environment.NewLine;
        PhoenixCrewSignOffFeedBack.InsertFeedBack(General.GetNullableInteger(Filter.CurrentCrewSelection)
            , General.GetNullableInteger(string.IsNullOrEmpty(ViewState["Rankid"].ToString()) ? "" : ViewState["Rankid"].ToString())
            , General.GetNullableInteger(ddlVessel.SelectedVessel)
            , sXmlData, General.GetNullableInteger(string.IsNullOrEmpty(ViewState["SignOnOffId"].ToString()) ? "" : ViewState["SignOnOffId"].ToString()));
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
                RadLabel lblCommentsyn = (RadLabel)e.Item.FindControl("lblCommentsyn");
                HtmlTableRow trcomments = (HtmlTableRow)e.Item.FindControl("trcomments");
                if (lblCommentsyn != null && lblCommentsyn.Text == "1")
                {
                    trcomments.Visible = true;
                }
                else
                    trcomments.Visible = false;
                RadLabel lblQuestionId = (RadLabel)e.Item.FindControl("lblQuestionId");
                RadioButtonList rblOptions = (RadioButtonList)e.Item.FindControl("rblOptions");
                RadTextBox txtComments = (RadTextBox)e.Item.FindControl("txtComments");
                DataSet ds = PhoenixCrewSignOffFeedBack.CrewSignoffFeedBackSearch(General.GetNullableInteger(Filter.CurrentCrewSelection)
                    , General.GetNullableInteger(string.IsNullOrEmpty(ViewState["SignOnOffId"].ToString()) ? "" : ViewState["SignOnOffId"].ToString()));
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
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }

        }
    }
}
