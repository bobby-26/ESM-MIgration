using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Reports;
using System.IO;
using Telerik.Web.UI;
public partial class CrewBriefingDebriefingList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        if (!IsPostBack)
        {

        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
		SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuCrewBriefingDebriefingList.AccessRights = this.ViewState;
        MenuCrewBriefingDebriefingList.MenuList = toolbar.Show();

        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddFontAwesomeButton("../Crew/CrewBriefingDebriefingList.aspx?" + Request.QueryString.ToString(), "Convert to Word", "<i class=\"fas fa-file-word\"></i>", "CTW");
        toolbar1.AddFontAwesomeButton("../Crew/CrewBriefingDebriefingList.aspx?" + Request.QueryString.ToString(), "Convert to PDF", "<i class=\"fas fa-file-pdf\"></i>", "CTPD");        
        Csvtow.AccessRights = this.ViewState;
        Csvtow.MenuList = toolbar1.Show();

        if (!IsPostBack)
        {
           
            if (Request.QueryString["vesselid"] != null)
                ucVessel.SelectedVessel = Request.QueryString["vesselid"];
			ddlUserAdd.ActiveYN = Convert.ToInt32( PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 44, "Y"));
            if (Request.QueryString["CrewBriefingDebriefingId"] != null)
            {
                MenuCrewBriefingDebriefingList.Visible = false;
                Csvtow.Visible = true;
                EditCrewBriefingDebriefing(Int16.Parse(Request.QueryString["CrewBriefingDebriefingId"].ToString()));
            }
            else
            {
                ResetCrewBriefingDebriefing();
                Csvtow.Visible = false;
            }
            
        }
    }
    protected void converttoword_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("CTW"))
        {
            AttachPD(Int32.Parse(Request.QueryString["CrewBriefingDebriefingId"].ToString()));
        }
        if (CommandName.ToUpper().Equals("CTPD"))
        {
            AttachPDF(Int32.Parse(Request.QueryString["CrewBriefingDebriefingId"].ToString()));
        }
    }
    protected void EditCrewBriefingDebriefing(int employeebriefingdebriefingid)
    {
        DataSet ds = PhoenixCrewBriefingDebriefing.EditBriefingDebriefing(employeebriefingdebriefingid);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();            
            txtSubject.Text = dr["FLDSUBJECT"].ToString();
            txtFromDate.Text = dr["FLDFROMDATE"].ToString();
            txtToDate.Text = dr["FLDTODATE"].ToString();            
            txtInstructorOwner.Text = dr["FLDINSTRUCTOR"].ToString();
            txtTopicsdiscussed.Text = dr["FLDTOPICSDISCUSSED"].ToString();
            txtCandidatesfeedback.Text = dr["FLDCANDIDATESFEEDBACK"].ToString();
            txtFieldofficefeedback.Text = dr["FLDFIELDOFFICEFEEDBACK"].ToString();
            txtConclusion.Text = dr["FLDCONCLUSION"].ToString();
            ddlUserAdd.SelectedUser = dr["FLDINSTRUCTORID"].ToString();
            ddlBriefingTopic.SelectedBriefingTopic = dr["FLDBRIEFINGTOPICID"].ToString();
            ucCompany.SelectedCompany = dr["FLDCOMPANYID"].ToString();
            ddlBriefingMode.SelectedValue = dr["FLDBRIEFINGMODE"].ToString();
            ddlUserAdd_TextChanged(null, null);
        }
    }

    protected void ResetCrewBriefingDebriefing()
    {
        ucVessel.SelectedVessel = "";        
        txtSubject.Text = "";
        txtFromDate.Text = "";
        txtToDate.Text = "";        
        txtInstructorOwner.Text = "";
        txtTopicsdiscussed.Text = "";
        txtCandidatesfeedback.Text = "";
        txtFieldofficefeedback.Text = "";
        txtConclusion.Text = "";
        ddlUserAdd.SelectedUser = "";
        ddlBriefingTopic.SelectedBriefingTopic = "";
    }

    protected void CrewBriefingDebriefingList_TabStripCommand(object sender, EventArgs e)
    {
        String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
        String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidCrewBriefingDebriefing())
                {
                    if (Request.QueryString["CrewBriefingDebriefingId"] != null)
                    {
                        PhoenixCrewBriefingDebriefing.UpdateBriefingDebriefing(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , Int32.Parse(Request.QueryString["CrewBriefingDebriefingId"].ToString())
                            , Int32.Parse(Filter.CurrentCrewSelection.ToString())
                            , Int32.Parse(ucVessel.SelectedVessel)
                            , DateTime.Parse(txtFromDate.Text)
                            , DateTime.Parse(txtToDate.Text)
                            , ""
                            , txtSubject.Text.Trim()
                            , txtTopicsdiscussed.Text.Trim()
                            , txtCandidatesfeedback.Text.Trim()
                            , txtFieldofficefeedback.Text.Trim()
                            , txtConclusion.Text.Trim()
                            , General.GetNullableInteger(ddlUserAdd.SelectedUser)
                            , General.GetNullableInteger(ddlBriefingTopic.SelectedBriefingTopic)
                            , General.GetNullableInteger(ucCompany.SelectedCompany)
                            , General.GetNullableInteger(ddlBriefingMode.SelectedValue)
                            , txtInstructorOwner.Text
                            );

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
                    }
                    else
                    {
                        PhoenixCrewBriefingDebriefing.InsertBriefingDebriefing(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , Int32.Parse(Filter.CurrentCrewSelection.ToString())
                            , Int32.Parse(ucVessel.SelectedVessel)
                            , DateTime.Parse(txtFromDate.Text)
                            , DateTime.Parse(txtToDate.Text)
                            , ""
                            , txtSubject.Text.Trim()
                            , txtTopicsdiscussed.Text.Trim()
                            , txtCandidatesfeedback.Text.Trim()
                            , txtFieldofficefeedback.Text.Trim()
                            , txtConclusion.Text
                            , General.GetNullableInteger(ddlUserAdd.SelectedUser)
                            , General.GetNullableInteger(ddlBriefingTopic.SelectedBriefingTopic)
                            , General.GetNullableGuid(Request.QueryString["appraisalid"])
                            , General.GetNullableInteger(ucCompany.SelectedCompany)
                            , General.GetNullableInteger(ddlBriefingMode.SelectedValue)
                            , txtInstructorOwner.Text
                            );

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
                        ResetCrewBriefingDebriefing();
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
        
    public void AttachPD(Int32 briefingid)
    {
        
        string Tmpfilelocation = string.Empty;
        string reportcode = string.Empty;
        DataSet ds = PhoenixCrewBriefingDebriefing.EditBriefingDebriefing(briefingid);
        string[] reportfile = new string[1];

            reportfile[0] = System.Web.HttpContext.Current.Server.MapPath("../Reports/ReportCrewBriefingDeBriefing.rpt");

        Tmpfilelocation = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath;

        //Tmpfilelocation = System.Web.HttpContext.Current.Request.MapPath("~/");
        string fileid = PhoenixCommonFileAttachment.GenerateDTKey();
        string filename = fileid + ".doc";
        Guid attachmentcode = new Guid(ds.Tables[0].Rows[0]["FLDDTKEY"].ToString());
        Tmpfilelocation = Tmpfilelocation + "/Temp/" + filename;
        PhoenixReportClass.ExportReportDoc(reportfile, ref Tmpfilelocation, ds);                
        Response.AddHeader("Content-Disposition", "attachment; filename=CrewBriefingDeBriefing.doc");
        Response.ContentType = "application/msword";
        Response.WriteFile(Tmpfilelocation);
        Response.End();
    }
    public void AttachPDF(Int32 briefingid)
    {
        string Tmpfilelocation = string.Empty;
        string reportcode = string.Empty;
        DataSet ds = PhoenixCrewBriefingDebriefing.EditBriefingDebriefing(briefingid);
        string[] reportfile = new string[1];

        reportfile[0] = System.Web.HttpContext.Current.Server.MapPath("../Reports/ReportCrewBriefingDeBriefing.rpt");
        Tmpfilelocation = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/Temp/";
        //Tmpfilelocation = System.Web.HttpContext.Current.Request.MapPath("~/");
        string fileid = PhoenixCommonFileAttachment.GenerateDTKey();
        string filename = fileid + ".pdf";
        Guid attachmentcode = new Guid(ds.Tables[0].Rows[0]["FLDDTKEY"].ToString());    
        Tmpfilelocation = Tmpfilelocation + filename;
        PhoenixReportClass.ExportReportPDF(reportfile, ref Tmpfilelocation, ds);
        Response.AddHeader("Content-Disposition", "attachment; filename=CrewBriefingDeBriefing.pdf");
        Response.ContentType = "application/pdf";
        Response.WriteFile(Tmpfilelocation);
        Response.End();
    }
    
    protected void ddlUserAdd_TextChanged(object sender, EventArgs e)
    {
        try
        {
            ucCompany.UserId = ddlUserAdd.SelectedUser;
            ucCompany.bind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidCrewBriefingDebriefing()
    {
        DateTime resultDate;
        Int32 resultInt;

        ucError.HeaderMessage = "Please provide the following required information";

        if ((!Int32.TryParse(ucVessel.SelectedVessel, out resultInt)) || ucVessel.SelectedVessel == "0")
            ucError.ErrorMessage = "Vessel is required.";       
        if (!DateTime.TryParse(txtFromDate.Text, out resultDate))
            ucError.ErrorMessage = "Briefing From Date is required.";
        if (!DateTime.TryParse(txtToDate.Text, out resultDate))
            ucError.ErrorMessage = "Briefing To Date is required.";
        if (txtSubject.Text.Trim() == "")
            ucError.ErrorMessage = "Subject is required.";
        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            if ((DateTime.TryParse(txtFromDate.Text, out resultDate)) && (DateTime.TryParse(txtToDate.Text, out resultDate)))
                if ((DateTime.Parse(txtFromDate.Text)) > (DateTime.Parse(txtToDate.Text)))
                    ucError.ErrorMessage = "'Briefing To Date' should be greater than or equal to 'Briefing From Date'";
        }
        if (ddlUserAdd.SelectedUser == "Dummy")
        {
            if (txtInstructorOwner.Text.Trim() == "")
                ucError.ErrorMessage = "Instructor is required.";
        }
        if (!General.GetNullableInteger(ddlBriefingTopic.SelectedBriefingTopic).HasValue)
        {
            ucError.ErrorMessage = "Briefing Details is required.";
        }

        return (!ucError.IsError);

    }
}
