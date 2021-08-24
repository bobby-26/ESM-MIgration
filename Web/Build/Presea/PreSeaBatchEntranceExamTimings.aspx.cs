using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Framework;

public partial class PreSeaBatchEntranceExamTimings : PhoenixBasePage
{
    string strBatchId;

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        try
        {
            strBatchId = Request.QueryString["batchid"];
            if (!IsPostBack)
            {
                ResetFields();
                SetPrimaryBatchDetails();
                if (Request.QueryString["examplanid"] != null)
                {
                    ViewState["EXAMPLANID"] = Request.QueryString["examplanid"];
                    SetExamPlanDetails(int.Parse(ViewState["EXAMPLANID"].ToString()));
                }
                else
                {
                    ViewState["EXAMPLANID"] = String.Empty;
                    ResetFields();
                }

                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Save", "SAVE");
                MenuPreSea.AccessRights = this.ViewState;
                MenuPreSea.MenuList = toolbarmain.Show();

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
                PhoenixPreSeaBatchPlanner.UpdateBatchEntranceExamTimingDetails(General.GetNullableInteger(ViewState["EXAMPLANID"].ToString())
                                                                    , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , Editor1.Content.ToString()
                                                                    , txtReportTime.Text.Trim());
                ucStatus.Text = "Entrance Exam Timing Details updated Successfully.";
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
                txtBatchName.Text = ds.Tables[0].Rows[0]["FLDBATCH"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetExamPlanDetails(int planid)
    {
        DataTable dt = PhoenixPreSeaBatchPlanner.EditBatchEntranceExamPlan(planid);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ViewState["EXAMPLANID"] = dr["FLDENTRANCEEXAMPLANID"].ToString();
            txtExamVenue.Text = dr["FLDEXAMVENUENAME"].ToString();
            txtStartDate.Text = dr["FLDSTARTDATE"].ToString();
            Editor1.Content = dr["FLDPLANSUMMARY"].ToString();
            txtReportTime.Text = dr["FLDREPORTINGTIME"].ToString();
        }
    }

    private void ResetFields()
    {
        string empty = String.Empty;
        txtExamVenue.Text = empty;
        txtReportTime.Text = empty;
    }

    public bool IsValidExamPlanDetails()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (General.GetNullableInteger(strBatchId) == null)
            ucError.ErrorMessage = "Select a Batch from Batch Planner";
        if (String.IsNullOrEmpty(txtReportTime.Text))
            ucError.ErrorMessage = "Reporting Time is required.";
        return (!ucError.IsError);

    }
}
