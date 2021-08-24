using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class CrewSignOnExtendReduce : PhoenixBasePage
{

    string strEmployeeId = string.Empty;
    string OriginalReliefDate = string.Empty;
    string familyid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (string.IsNullOrEmpty(Filter.CurrentCrewSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = string.Empty;
            else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = Request.QueryString["empid"];
            else if (!string.IsNullOrEmpty(Filter.CurrentCrewSelection))
                strEmployeeId = Filter.CurrentCrewSelection;
            if (Request.QueryString["familyid"] != null)
            {
                familyid = Request.QueryString["familyid"];
            }
            if (!IsPostBack)
            {
                if (familyid != "")
                {
                    SetFamilyDetails(Convert.ToInt32(familyid));
                }
                else
                {
                    SetEmployeePrimaryDetails();
                    SetExtendReduce();
                }
                gvCrewExtendReduce.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (!string.IsNullOrEmpty(txtReliefDue.Text))
            {
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                ReliefDueER.AccessRights = this.ViewState;
                ReliefDueER.MenuList = toolbar.Show();
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
        string[] alColumns = { "FLDEXTENDREDUCE", "FLDRELIEFDUEDATE", "FLDDATEOFCHANGE", "FLDCREATEDBY", "FLDREMARKS" };
        string[] alCaptions = { "Extension/Reduction", " Revised Relief Due", "Date of Change", "Created by", "Remarks" };

        DataSet ds = PhoenixCrewSignOnOff.CrewExtendReduceList(General.GetNullableInteger(strEmployeeId),
               General.GetNullableInteger(familyid), General.GetNullableInteger(ViewState["VESSELID"].ToString()));

        General.SetPrintOptions("gvCrewExtendReduce", "Crew Extend/Reduce", alCaptions, alColumns, ds);

        gvCrewExtendReduce.DataSource = ds;
        gvCrewExtendReduce.VirtualItemCount = ds.Tables[0].Rows.Count;
    }
    protected void gvCrewExtendReduce_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void CrewExtendReduce_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        string[] alColumns = { "FLDEXTENDREDUCE", "FLDRELIEFDUEDATE", "FLDDATEOFCHANGE", "FLDCREATEDBY", "FLDREMARKS" };
        string[] alCaptions = { "Extension/Reduction", " Revised Relief Due", "Date of Change", "Created by", "Remarks" };

        DataSet ds;

        ds = PhoenixCrewSignOnOff.CrewExtendReduceList(General.GetNullableInteger(strEmployeeId),
            General.GetNullableInteger(familyid), General.GetNullableInteger(ViewState["VESSELID"].ToString()));


        Response.AddHeader("Content-Disposition", "attachment; filename=CrewExtendReduce.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Crew Extend/Reduce</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void ReliefDueER_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (!IsValidRemarks())
            {
                ucError.Visible = true;
                return;

            }
            if (familyid != "")
            {
                PhoenixCrewSignOnOff.UpdateFamilyExtendReduceRelief(int.Parse(strEmployeeId), int.Parse(ViewState["VESSELID"].ToString()),
                DateTime.Parse(txtReliefDueTo.Text), txtRemarks.Text, Convert.ToInt32(familyid), General.GetNullableInteger(rblExtendReduce.SelectedValue));
                SetFamilyDetails(Convert.ToInt32(familyid));
            }
            else
            {
                PhoenixCrewSignOnOff.CrewExtendReduceRelief(int.Parse(strEmployeeId), int.Parse(ViewState["VESSELID"].ToString()),
                    DateTime.Parse(txtReliefDueTo.Text), txtRemarks.Text, General.GetNullableInteger(rblExtendReduce.SelectedValue));
                SetEmployeePrimaryDetails();
                SetExtendReduce();
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "parent.fnReloadList(null,'ifMoreInfo','keepopen');", true);
            ucStatus.Text = "Crew relief date updated";
            BindData();
            gvCrewExtendReduce.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidRemarks()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        DateTime resultDate;

        if (string.IsNullOrEmpty(txtRemarks.Text))
            ucError.ErrorMessage = "Remarks is required.";
        if (General.GetNullableDateTime(txtReliefDueTo.Text).HasValue && DateTime.TryParse(txtSignOn.Text, out resultDate)
            && DateTime.Compare(resultDate, DateTime.Parse(txtReliefDueTo.Text)) > 0)
        {
            ucError.ErrorMessage = "Extend Relief Due should be greater than Sign On Date.";
        }
        return (!ucError.IsError);
    }
    private void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(strEmployeeId));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDMIDDLENAME"].ToString() + " " + dt.Rows[0]["FLDLASTNAME"].ToString();
                txtVessel.Text = dt.Rows[0]["FLDPRESENTVESSELNAME"].ToString();
                txtSignOn.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDSIGNONDATE"].ToString()));
                txtBtoD.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDBTOD"].ToString()));
                txtReliefDue.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDRELEFDUEDATE"].ToString()));
                txtReliefDueTo.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDRELEFDUEDATE"].ToString()));
                ViewState["VESSELID"] = dt.Rows[0]["FLDPRESENTVESSELID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetExtendReduce()
    {
        try
        {
            DataTable dt = PhoenixCrewSignOnOff.CrewSignOnEdit(General.GetNullableInteger(strEmployeeId));
            if (dt.Rows.Count > 0)
                txtRemarks.Text = dt.Rows[0]["FLDSIGNONREMARKS"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void txtMonths_TextChanged(object sender, EventArgs e)
    {
        if (rblExtendReduce.SelectedValue == "1" && !string.IsNullOrEmpty(txtReliefDue.Text) && (txtMonths.Text != ""))
        {
            double months = Convert.ToDouble(txtMonths.Text);
            int days = Convert.ToInt32(30 * months);
            txtReliefDueTo.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Parse(txtReliefDue.Text).AddDays(days));
        }
        else if (!string.IsNullOrEmpty(txtReliefDueTo.Text) && (txtMonths.Text != "") && !string.IsNullOrEmpty(txtReliefDue.Text))
        {
            double months = Convert.ToDouble(txtMonths.Text);
            int days = Convert.ToInt32(30 * months);
            txtReliefDueTo.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Parse(txtReliefDue.Text).AddDays(-(days)));
        }
    }
    protected void txtReliefDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(txtReliefDueTo.Text) && !String.IsNullOrEmpty(txtReliefDue.Text))
            {
                DateTime a = Convert.ToDateTime(txtReliefDueTo.Text);
                DateTime b = Convert.ToDateTime(txtReliefDue.Text);
                TimeSpan s = a - b;
                int isnegative = s.Days;
                if (isnegative < 0)
                {
                    rblExtendReduce.SelectedValue = "2";
                }
                else
                {
                    rblExtendReduce.SelectedValue = "1";
                }
                if (rblExtendReduce.SelectedValue == "1" && !string.IsNullOrEmpty(txtReliefDue.Text))
                {
                    double x = isnegative / 30.00;
                    txtMonths.Text = Convert.ToString(x);
                    txtMonths.Text = txtMonths.Text.Substring(0, txtMonths.Text.IndexOf('.') + 2);

                }
                else if (!string.IsNullOrEmpty(txtReliefDue.Text))
                {
                    double x = isnegative / 30.00;
                    txtMonths.Text = Convert.ToString(-x);
                    txtMonths.Text = txtMonths.Text.Substring(0, txtMonths.Text.IndexOf('.') + 2);
                }
            }

            gvCrewExtendReduce.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void SetFamilyDetails(int familyid)
    {
        DataTable dt = PhoenixCrewFamilyNok.EditEmployeeFamilySignOn(familyid, null, null);

        if (dt.Rows.Count > 0)
        {

            txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString() + "" + dt.Rows[0]["FLDMIDDLENAME"].ToString() + "" + dt.Rows[0]["FLDLASTNAME"].ToString();
            txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
            txtVessel.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
            txtSignOn.Text = string.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime((dt.Rows[0]["FLDSIGNONDATE"].ToString())));
            txtReliefDue.Text = string.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime((dt.Rows[0]["FLDRELIEFDUEDATE"].ToString())));
            txtBtoD.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDBTOD"].ToString()));
            txtReliefDueTo.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDRELIEFDUEDATE"].ToString()));
            txtRemarks.Text = dt.Rows[0]["FLDSIGNONREMARKS"].ToString();
            ViewState["VESSELID"] = dt.Rows[0]["FLDPRESENTVESSELID"].ToString();
        }
    }
}
