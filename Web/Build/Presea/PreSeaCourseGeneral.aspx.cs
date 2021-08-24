using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaCourseGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE");

            MenuPreSea.AccessRights = this.ViewState;
            MenuPreSea.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                txtCourseName.Focus();             

                if (Request.QueryString["courseid"] != null)
                    ViewState["PRESEACOURSEID"] = Request.QueryString["courseid"];
                else
                    ViewState["PRESEACOURSEID"] = String.Empty;

                if (!String.IsNullOrEmpty(ViewState["PRESEACOURSEID"].ToString()))
                    EditPreSeaCourse(int.Parse(ViewState["PRESEACOURSEID"].ToString()));
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
                if (!IsValidCourse(txtCourseName.Text.Trim(), ucDuration.Text.Trim(), ucNoOfSemester.Text.Trim(), null, txtShortName.Text.Trim()
                    ,txtMaxMark.Text,txtPassMark.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                if (String.IsNullOrEmpty(ViewState["PRESEACOURSEID"].ToString()))
                {
                    PhoenixPreSeaCourse.InsertPreSeaCourse(txtCourseName.Text.Trim()
                        , General.GetNullableDecimal(ucDuration.Text.Trim())
                        , General.GetNullableByte(ucNoOfSemester.Text.Trim())
                        , null
                        , txtCourseSummary.Text.Trim()
                        , txtQualificationSummary.Text.Trim()
                        , txtShortName.Text.Trim()
                        , null
                        , General.GetNullableDecimal(txtMaxMark.Text)
                        , General.GetNullableDecimal(txtPassMark.Text));
                    ucStatus.Text = "Course information Saved Successfully.";
                }
                else
                {
                    PhoenixPreSeaCourse.UpdatePreSeaCourse(int.Parse(ViewState["PRESEACOURSEID"].ToString())
                        , txtCourseName.Text.Trim()
                         , General.GetNullableDecimal(ucDuration.Text.Trim())
                        , General.GetNullableByte(ucNoOfSemester.Text.Trim())
                        , null
                        , txtCourseSummary.Text.Trim()
                        , txtQualificationSummary.Text.Trim()
                        , txtShortName.Text.Trim()
                        , null
                        , General.GetNullableDecimal(txtMaxMark.Text)
                        , General.GetNullableDecimal(txtPassMark.Text));

                    ucStatus.Text = "Course information Updated Successfully.";
                    EditPreSeaCourse(int.Parse(ViewState["PRESEACOURSEID"].ToString()));
                }
                //String script = String.Format("javascript:fnReloadList('codehelp1',null,null);");               
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                    "BookMarkScript", "fnReloadList('codehelp1', null, null);", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void EditPreSeaCourse(int PreSeaCourseId)
    {
        DataTable dt = PhoenixPreSeaCourse.EditPreSeaCourse(PreSeaCourseId);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtCourseName.Text = dr["FLDPRESEACOURSENAME"].ToString();
            ucDuration.Text = dr["FLDDURATION"].ToString();
            ucNoOfSemester.Text = dr["FLDNOOFSEMESTERS"].ToString();            
            txtCourseSummary.Text = dr["FLDCOURSESUMMARY"].ToString();
            txtQualificationSummary.Text = dr["FLDQUALIFICATIONSUMMARY"].ToString();
            txtShortName.Text = dr["FLDSHORTNAME"].ToString();
            txtMaxMark.Text = dr["FLDMAXMARK"].ToString();
            txtPassMark.Text = dr["FLDPASSMARK"].ToString();
            txtCourseName.Focus();
        }
    }

    private bool IsValidCourse(string CourseName, string Duration, string NoOfSem, string refCourse, string ShortName,string maxmark, string passmark)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (String.IsNullOrEmpty(CourseName))
        {
            ucError.ErrorMessage = "Course Name is required.";
        }
        if (String.IsNullOrEmpty(Duration))
        {
            ucError.ErrorMessage = "Duration is required.";
        }
        if (String.IsNullOrEmpty(NoOfSem))
        {
            ucError.ErrorMessage = "No of Semesters is required.";
        }
        if (String.IsNullOrEmpty(ShortName))
        {
            ucError.ErrorMessage = "Abbreviation is required";
        }
        if (string.IsNullOrEmpty(maxmark))
        {
            ucError.ErrorMessage = "Max Mark is required";
        }
        if (string.IsNullOrEmpty(passmark))
        {
            ucError.ErrorMessage = "Pass Mark is required";
        }
        else if ((General.GetNullableDecimal(maxmark) < General.GetNullableDecimal(passmark)))
        {
            ucError.ErrorMessage = "Pass mark should be less than or equal to max mark.";
        }
        return (!ucError.IsError);
    }

}
