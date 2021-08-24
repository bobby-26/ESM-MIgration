using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class CrewOffshore_CrewOffshorePersonalGeneralOverview : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlAnchor adoc = (HtmlAnchor)this.FindControl("adoc");
        // adoc.Attributes.Add("HRef", "javascript:openNewWindow('jobaudit','','CrewOffshore/CrewOffshoreDocumentsList.aspx?empid=" + Filter.CurrentCrewSelection + "')");
        aPersonal.HRef = "javascript:top.openNewWindow('CompCategory','Personal Details','Crew/CrewPersonal.aspx')";
        aAdd.HRef = "javascript:top.openNewWindow('CompCategory','Address Details','Crew/CrewAddress.aspx')";
        adoc.HRef = "javascript:top.openNewWindow('CompCategory','Travel Details','CrewOffshore/CrewOffshoreDocumentsList.aspx?empid=" + Filter.CurrentCrewSelection + "')";
        aAppraisal.HRef = "javascript:top.openNewWindow('CompCategory','Appraisal Details','Crew/CrewAppraisal.aspx?empid=" + Filter.CurrentCrewSelection + "')";
        atraining.HRef = "javascript:top.openNewWindow('CompCategory','Training Details','CrewOffshore/CrewOffshoreTrainingNeedsCBTDetail.aspx?employeeid=" + Filter.CurrentCrewSelection + "&coursetype=451')";
       // aCourse.HRef = "javascript:top.openNewWindow('CompCategory','Course Details','CrewOffshore/CrewOffshoreDocumentsList.aspx?empid=" + Filter.CurrentCrewSelection + "')";
        aexp.HRef = "javascript:top.openNewWindow('CompCategory','Experience Details',' Crew/CrewCompanyExperience.aspx')";

        btnpropose.Attributes.Add("Onclick", "javascript:top.openNewWindow('CompCategory','Experience Details','CrewOffshore/CrewOffshoreSuitabilityCheckWithDocument.aspx?personalmaster=1')");
        btnupdate.Attributes.Add("Onclick", "javascript:top.openNewWindow('CompCategory','Experience Details','Crew/CrewPersonal.aspx')");
        btnpdemail.Attributes.Add("onclick", "javascript:top.openNewWindow('PDForm', '', 'CrewOffshore/CrewOffshoreEmployeeDocumentList.aspx?empid=" + Filter.CurrentCrewSelection + "&pdform=1');return false;");


        if (!IsPostBack)
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeList(Convert.ToInt32(Filter.CurrentCrewSelection));
            DataTable dts = PhoenixCommonFileAttachment.AttachmentList(new Guid(dt.Rows[0]["FLDDTKEY"].ToString()), "CREWRESUME");
            if (dts.Rows.Count > 0)
            {
                btncv.Enabled = true;
                btncv.Attributes.Add("Onclick", "javascript:top.openNewWindow('CompCategory','Experience Details','Common/Download.aspx?dtkey=" + dts.Rows[0]["FLDDTKEY"].ToString() + "')");

            }
            else
            {
                btncv.Enabled = false;
            }
            personaliframe.Attributes["src"] = "../CrewOffshore/CrewOffshorePersonalDetailsFrame.aspx";
            AddressFrame.Attributes["src"] = "../CrewOffshore/CrewOffshoreAddressDetailsFrame.aspx";
            DocumentsFrame.Attributes["src"] = "../CrewOffshore/CrewOffshoreDocumentsDetailsFrame.aspx";
            Appraisalframe.Attributes["src"]= "../CrewOffshore/CrewOffshoreAppraisalDetailsFrame.aspx";
            TrainingFrame.Attributes["src"]= "../CrewOffshore/CrewOffshoreTraningNeedsDetailsFrame.aspx";
            //CourseFrame.Attributes["src"] = "../CrewOffshore/CrewOffshoreCourseDetailsFrame.aspx";
            ExpFrame.Attributes["src"] = "../CrewOffshore/CrewOffshoreExperienceDetailsFrame.aspx";

        }
    }

    protected void btnCrewList_Click(object sender, EventArgs e)
    {

    }
}