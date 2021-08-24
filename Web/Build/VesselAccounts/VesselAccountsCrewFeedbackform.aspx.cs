using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using System.Text;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Registers;

public partial class VesselAccountsCrewFeedbackform :  PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE");
            // toolbar.AddButton("Report", "REPORT");
            MenuCrewFeedback.AccessRights = this.ViewState;
            MenuCrewFeedback.MenuList = toolbar.Show();           
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int feedbackfilledYN = 0;
        DataTable dtEmployee = PhoenixVesselAccountsCrewFeedback.VesselCrewSignonDetailsEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                 , PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        if (dtEmployee.Rows.Count > 0)
        {
            DataRow drEmp = dtEmployee.Rows[0];
            if (General.GetNullableInteger(drEmp["FLDSIGNONOFFID"].ToString()) != null)
            {
                DataTable dt = PhoenixVesselAccountsCrewFeedback.VesselCrewFeedbackList(General.GetNullableInteger(drEmp["FLDVESSELID"].ToString())
                    , General.GetNullableInteger(drEmp["FLDEMPLOYEEID"].ToString())
                    , General.GetNullableInteger(drEmp["FLDSIGNONOFFID"].ToString())
                    , ref feedbackfilledYN);

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    txtName.Text = dr["FLDNAME"].ToString();
                    txtRank.Text = dr["FLDRANKNAME"].ToString();
                    txtVessel.Text = dr["FLDVESSELNAME"].ToString();
                    txtSignonDate.Text = dr["FLDSIGNONDATE"].ToString();

                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt.Copy());
                    gvFeedback.DataSource = ds;
                    gvFeedback.DataBind();
                }
                else
                {
                    ShowNoRecordsFound(dt, gvFeedback);
                }
            }
        }
    }
    protected void gvFeedback_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            RadioButtonList rbl = (RadioButtonList)e.Row.FindControl("rblAnswermode");
            string objectivetypeyn = ((Label)e.Row.FindControl("lblIsObjectiveTypeYN")).Text;
            if (objectivetypeyn == "0")
            {
                if (rbl != null)
                {
                    rbl.Visible = false;
                }
            }
            else
            {
                if (rbl != null && rbl.Visible == true)
                {
                    string answermode = drv["FLDANSWERMODE"].ToString();
                    if (General.GetNullableInteger(answermode) != null)
                        rbl.Items.FindByValue(answermode).Selected = true;

                    if (answermode == "0")
                    {
                        TextBox txtDesc = (TextBox)e.Row.FindControl("txtDescription");
                        txtDesc.CssClass = "input_mandatory";
                    }
                }
            }
        }
    }
    protected void MenuCrewFeedback_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            bool flag = false;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                StringBuilder stremployeelist = new StringBuilder();
                GridView _gridView = gvFeedback;

                if (_gridView.Rows.Count > 0)
                {
                    foreach (GridViewRow gvr in gvFeedback.Rows)
                    {
                        Label lblQuestionNo = (Label)gvr.FindControl("lblOrderNo");
                        RadioButtonList rbl = (RadioButtonList)gvr.FindControl("rblAnswermode");
                        TextBox txtDesc = (TextBox)gvr.FindControl("txtDescription");

                        if (rbl != null && txtDesc != null)
                        {
                            if (rbl.Visible == true)
                            {
                                if (rbl.SelectedItem == null)
                                {
                                    ucError.ErrorMessage = "Question No." + lblQuestionNo.Text + " Answer is required";
                                    flag = true;
                                }
                                else if (rbl.SelectedItem.Value == "0" && General.GetNullableString(txtDesc.Text.Trim()) == null)
                                {
                                    ucError.ErrorMessage = "Question No." + lblQuestionNo.Text + " remarks is required";
                                    flag = true;
                                }
                            }
                        }
                    }
                }
                if (flag == true)
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    foreach (GridViewRow gvr in gvFeedback.Rows)
                    {
                        Label vesselid = (Label)gvr.FindControl("lblVesselId");
                        Label employeeid = (Label)gvr.FindControl("lblEmployeeId");
                        Label signonoffid = (Label)gvr.FindControl("lblSignonoffId");
                        Label feedbackid = (Label)gvr.FindControl("lblFeedbackId");

                        Label lblQuestionNo = (Label)gvr.FindControl("lblOrderNo");
                        RadioButtonList rbl = (RadioButtonList)gvr.FindControl("rblAnswermode");
                        TextBox txtDesc = (TextBox)gvr.FindControl("txtDescription");

                        PhoenixVesselAccountsCrewFeedback.UpdateFeedback(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , General.GetNullableInteger(vesselid.Text)
                            , General.GetNullableInteger(signonoffid.Text)
                            , new Guid(feedbackid.Text)
                            , General.GetNullableInteger(rbl.Visible == false ? "" : rbl.SelectedItem.Value)
                            , General.GetNullableString(txtDesc.Text)
                        );

                    }
                    ucStatus.Visible = true;
                    ucStatus.Text = "Feedback Updated";
                    BindData();
                    Response.Redirect("../Dashboard/DashboardBlank.aspx", false);
                }
            }
            if (dce.CommandName.ToUpper().Equals("REPORT"))
            {

                string prams = "";

                prams += "&debitnotereference=" + ViewState["debitnotereference"].ToString();
                prams += "&Ownerid=" + int.Parse(ViewState["Ownerid"].ToString());
                prams += "&BudgetCode=" + General.GetNullableInteger(Filter.CurrentOwnerBudgetCodeSelection);
                prams += "&accountid=" + int.Parse(ViewState["accountid"].ToString());
                prams += exceloptions();


                Response.Redirect("../Reports/ReportsView.aspx?applicationcode=7&reportcode=VESSELCREWFEEDBACK" + prams, false);
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
        gv.Rows[0].Attributes["onclick"] = "";
    }
    protected void rblAnswermode_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadioButtonList rbl = (RadioButtonList)sender;
        GridViewRow row = (GridViewRow)rbl.Parent.Parent;
        TextBox txtDesc = row.FindControl("txtDescription") as TextBox;
        if (rbl != null)
        {
            if (txtDesc != null)
            {
                if (rbl.SelectedItem.Value == "0")


                    txtDesc.CssClass = "input_mandatory";
                else

                    txtDesc.CssClass = "input";
            }
        }
    }
    protected string exceloptions()
    {
        DataSet ds = new DataSet();

        ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 181);

        string options = "";

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr["FLDSHORTNAME"].ToString().Equals("EXL"))
                options += "&showexcel=" + dr["FLDHARDNAME"].ToString();

            if (dr["FLDSHORTNAME"].ToString().Equals("WRD"))
                options += "&showword=" + dr["FLDHARDNAME"].ToString();

            if (dr["FLDSHORTNAME"].ToString().Equals("PDF"))
                options += "&showpdf=" + dr["FLDHARDNAME"].ToString();
        }
        return options;
    }
}
