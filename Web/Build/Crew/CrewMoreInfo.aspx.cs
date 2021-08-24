using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using System.Web.UI;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class CrewMoreInfo : PhoenixBasePage
{      
    protected void Page_Load(object sender, EventArgs e)
    {
        
        try
        {
			SessionUtil.PageAccessRights(this.ViewState);
            if (string.IsNullOrEmpty(Request.QueryString["s"]))
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
                MenuComment.AccessRights = this.ViewState;
                MenuComment.MenuList = toolbarmain.Show();
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["type"] == "MEDICALCASE")
                {
                    txtComment.Visible=false;
                    txtAreaComment.Visible = true;
                    MenuComment.Visible = false;                                                               
                }
                if (Request.QueryString["id"] != null && Request.QueryString["id"] != string.Empty)
                {
                    ViewState["id"] = Request.QueryString["id"];
                    BindDataEdit(ViewState["id"].ToString());
                }
				if (Request.QueryString["edityn"] != null)
				{
					MenuComment.Visible = false;
				}
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDataEdit(string id)
    {
        DataTable dt = new DataTable();
        if (Request.QueryString["type"].ToString() == PhoenixCrewAttachmentType.DOCUMENTS.ToString())
        {
            dt = PhoenixCrewTravelDocument.ListEmployeeTravelDocument(null, General.GetNullableInteger(id));
            if (dt.Rows.Count > 0)
            {
                txtComment.Content = dt.Rows[0]["FLDREMARKS"].ToString();
            }
        }
        else if (Request.QueryString["type"].ToString() == PhoenixCrewAttachmentType.FAMILY.ToString())
        {
            dt = PhoenixCrewFamilyNok.ListEmployeeFamilyTravelDocument(null, General.GetNullableInteger(id));
            if (dt.Rows.Count > 0)
            {
                txtComment.Content = dt.Rows[0]["FLDREMARKS"].ToString();
            }
        }
        else if (Request.QueryString["type"].ToString() == PhoenixCrewAttachmentType.REFERENCE.ToString())
        {
            dt = PhoenixCrewReference.EditCrewReference(int.Parse(id));

            if (dt.Rows.Count > 0)
            {
                txtComment.Content = dt.Rows[0]["FLDCOMMENTS"].ToString();
            }
        }
        else if (Request.QueryString["type"].ToString() == PhoenixCrewAttachmentType.LICENCE.ToString())
        {
            dt = PhoenixCrewLicence.EditCrewLicence(int.Parse(id));

            if (dt.Rows.Count > 0)
            {
                txtComment.Content = dt.Rows[0]["FLDREMARKS"].ToString();
            }
        }
		else if (Request.QueryString["type"].ToString() == PhoenixCrewAttachmentType.COURSE.ToString())
		{
			dt = PhoenixCrewCourseCertificate.EditCrewCourseCertificate(int.Parse(id));

			if (dt.Rows.Count > 0)
			{
				txtComment.Content = dt.Rows[0]["FLDREMARKS"].ToString();
			}
		}
		else if (Request.QueryString["type"].ToString() == "COURSEFLAG")
		{
			dt = PhoenixNewApplicantCourse.EditEmployeelCourseFlag(int.Parse(id));

			if (dt.Rows.Count > 0)
			{
				txtComment.Content = dt.Rows[0]["FLDREMARKS"].ToString();
			}
		}
        else if (Request.QueryString["type"].ToString() == "MEDICALCASE")
        {
            dt = PhoenixInspectionPNI.EditMeidicalCaseRemarks(new Guid(id));

            if (dt.Rows.Count > 0)
            {
                txtAreaComment.Text = dt.Rows[0]["FLDILLNESSDESCRIPTION"].ToString();
            }
        }
        else if (Request.QueryString["type"].ToString() == "BRIEFING")
        {
            dt = PhoenixCrewBriefingDebriefing.EditBriefingDebriefing(int.Parse(id)).Tables[0];

            if (dt.Rows.Count > 0)
            {
                txtAreaComment.Text = dt.Rows[0]["FLDSUBJECT"].ToString();
                txtAreaComment.Visible = true;
                txtComment.Visible = false;
            }
        }
    }


    protected void MenuComment_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixCrewManagement.UpdateCrewRelatedRemarks(int.Parse(Request.QueryString["id"]), Request.QueryString["type"], txtComment.Content);
            }
       
            //string Script = "";
            //Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            //Script += "parent.CloseCodeHelpWindow('MoreInfo');";
            ////Script += "fnReloadList('CI','MoreInfo');";
            //Script += "</script>" + "\n";
            //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                         "BookMarkScript", "parent.fnReloadList('MoreInfo', 'ifMoreInfo', null);", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
