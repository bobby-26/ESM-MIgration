using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewTrainingList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);
                PhoenixToolbar toolbar = new PhoenixToolbar();

                toolbar.AddButton("Save", "SAVE");
                MenuCrewTrainingList.AccessRights = this.ViewState;
                MenuCrewTrainingList.MenuList = toolbar.Show();

                ucType.HardTypeCode = ((int)PhoenixHardTypeCode.CREWDOCUMENTTYPE).ToString();
                ucType.ShortNameFilter = "COU,LIC";
                ucStatus.HardTypeCode = ((int)PhoenixHardTypeCode.TRAININGSTATUS).ToString();

                if (Request.QueryString["CrewTrainingId"] != null)
                {
                    EditCrewTraining(Int16.Parse(Request.QueryString["CrewTrainingId"].ToString()));
                }
                else
                {
                    BinducDocuments(General.GetNullableInteger(ucType.SelectedHard));
                    ResetCrewTraining();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucType_Changed(object sender, EventArgs e)
    {
        BinducDocuments(General.GetNullableInteger(ucType.SelectedHard));
    }

    private void BinducDocuments(int? documenttype)
    {      
         if (documenttype.HasValue)
         {
             PhoenixDocumentType dt = (PhoenixDocumentType)Enum.Parse(typeof(PhoenixDocumentType), documenttype.ToString());
             switch (dt)
             {
                 case PhoenixDocumentType.COURSE:
                     ucCourseLicence.DocumentList = PhoenixRegistersDocumentCourse.ListDocumentCourse(null);
                     break;
                 case PhoenixDocumentType.LICENCE:
                     ucCourseLicence.DocumentList = PhoenixRegistersDocumentLicence.ListDocumentLicence(null);
                     break;
                 default:
                     ucCourseLicence.DocumentList = null;
                     break;
             }
         }
        else
             ucCourseLicence.DocumentList = null;
    }

    protected void EditCrewTraining(int employeetrainingid)
    {
        DataSet ds = PhoenixCrewTraining.EditCrewTraining(employeetrainingid);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            ucType.SelectedHard = dr["FLDTYPEOFDEGREE"].ToString();
            BinducDocuments(Int32.Parse(dr["FLDTYPEOFDEGREE"].ToString()));

            ucCourseLicence.SelectedDocument = dr["FLDCOURSEORLICENCE"].ToString();
            ucStatus.SelectedHard = dr["FLDSTATUS"].ToString();
            ucInstitution.SelectedInstitution = dr["FLDINSTITUTION"].ToString();

            txtFromDate.Text = dr["FLDFROMDATE"].ToString();
            txtToDate.Text = dr["FLDTODATE"].ToString();
            txtRemarks.Text = dr["FLDREMARKS"].ToString();

            chkArchiveYN.Checked = (dr["FLDACTIVEYN"].ToString().Equals("1")) ? true : false;
        }
    }

    protected void ResetCrewTraining()
    {
        ucType.SelectedHard = "";
        ucCourseLicence.SelectedDocument = "";
        ucStatus.SelectedHard = "";
        ucInstitution.SelectedInstitution = "";

        txtFromDate.Text = "";
        txtToDate.Text = "";
        txtRemarks.Text = "";

        chkArchiveYN.Checked = false;
    }

    protected void CrewTrainingList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
            String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");

            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidCrewTraining())
                {
                    if (Request.QueryString["CrewTrainingId"] != null)
                    {
                        PhoenixCrewTraining.UpdateCrewTraining(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , Int32.Parse(Request.QueryString["CrewTrainingId"].ToString())
                            , Int32.Parse(Filter.CurrentCrewSelection.ToString())
                            , Int32.Parse(ucType.SelectedHard)
                            , Int32.Parse(ucCourseLicence.SelectedDocument)
                            , General.GetNullableInteger(ucStatus.SelectedHard)
                            , General.GetNullableInteger(ucInstitution.SelectedInstitution)
                            , General.GetNullableDateTime(txtFromDate.Text)
                            , General.GetNullableDateTime(txtToDate.Text)
                            , ((chkArchiveYN.Checked == true) ? 1 : 0)
                            , txtRemarks.Text);

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
                    }
                    else
                    {
                        PhoenixCrewTraining.InsertCrewTraining(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , Int32.Parse(Filter.CurrentCrewSelection.ToString())
                            , Int32.Parse(ucType.SelectedHard)
                            , Int32.Parse(ucCourseLicence.SelectedDocument)
                            , General.GetNullableInteger(ucStatus.SelectedHard)
                            , General.GetNullableInteger(ucInstitution.SelectedInstitution)
                            , General.GetNullableDateTime(txtFromDate.Text)
                            , General.GetNullableDateTime(txtToDate.Text)
                            , ((chkArchiveYN.Checked == true) ? 1 : 0)
                            , txtRemarks.Text);

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupopen, true);
                        ResetCrewTraining();
                    }
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidCrewTraining()
    {
        DateTime resultDate;
        Int32 resultInt;

        ucError.HeaderMessage = "Please provide the following required information";

        if ((!Int32.TryParse(ucType.SelectedHard, out resultInt)) || ucType.SelectedHard == "0")
            ucError.ErrorMessage = "Type is required.";
        if ((!Int32.TryParse(ucCourseLicence.SelectedDocument, out resultInt)) || ucCourseLicence.SelectedDocument == "0")
            ucError.ErrorMessage = "Course/Licence is required.";

        if (!DateTime.TryParse(txtFromDate.Text, out resultDate))
            ucError.ErrorMessage = "Valid From Date is required.";

        //if (string.IsNullOrEmpty(txtToDate.Text) && !DateTime.TryParse(txtToDate.Text, out resultDate) && !string.IsNullOrEmpty(txtFromDate.Text))
        //    ucError.ErrorMessage = "Valid To Date is required.";

        if (!string.IsNullOrEmpty(txtFromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
        {
            if ((DateTime.TryParse(txtFromDate.Text, out resultDate)) && (DateTime.TryParse(txtToDate.Text, out resultDate)))
                if ((DateTime.Parse(txtFromDate.Text)) >= (DateTime.Parse(txtToDate.Text)))
                    ucError.ErrorMessage = "'Valid To Date' should be greater than 'Valid From Date'";
        }

        return (!ucError.IsError);

    }
}
