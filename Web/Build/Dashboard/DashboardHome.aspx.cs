using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Reports;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DashboardHome : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (!IsPostBack)
            {
                ViewState["empid"] = string.Empty;
                if (Filter.CurrentCrewSelection == null)
                    Filter.CurrentCrewSelection = Filter.CurrentNewApplicantSelection;

                ViewState["empid"] = Filter.CurrentCrewSelection;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdplan_Click1(object sender, EventArgs e)
    {
        try
        {


            cmdplan.Attributes.Add("onclick", "javascript:openNewWindow('Filters', '', '" + Session["sitepath"] + "/Portal/PortalSeafarerPlanProposed.aspx?empid=" + ViewState["empid"] + "');return false;");

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdProfile_Click(object sender, EventArgs e)
    {
        try
        {
            cmdProfile.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Plan','Crew/CrewPersonal.aspx?portal=1&empid=" + ViewState["empid"] + "');return false");


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        try
        {


            LinkButton2.Attributes.Add("onclick", "javascript:openNewWindow('Document', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePortalDocumentsList.aspx?empid=" + ViewState["empid"] + "');return false;");


        
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdEdit_Click(object sender, EventArgs e)
    {
        try
        {
        
            cmdEdit.Attributes.Add("onclick", "javascript:openNewWindow('Document', '', '" + Session["sitepath"] + "/Portal/PortalSeafarerTraining.aspx?empid=" + ViewState["empid"] + "');return false;");

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdDebriefing_Click(object sender, EventArgs e)
    {
        try
        {
          
            cmdDebriefing.Attributes.Add("onclick", "javascript:openNewWindow('Document', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePortalDeBriefing.aspx?empid=" + ViewState["empid"] + "');return false;");

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdTravel_Click(object sender, EventArgs e)
    {
        try
        {
            cmdTravel.Attributes.Add("onclick", "javascript:openNewWindow('Document', '', '" + Session["sitepath"] + "/Crew/CrewPortalTravelPlanDocument.aspx?empid=" + ViewState["empid"] + "');return false;");


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void CmdAppraisal_Click(object sender, EventArgs e)
    {
        try
        {
           
            CmdAppraisal.Attributes.Add("onclick", "javascript:openNewWindow('Filters', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePortalPendingAppraisal.aspx?empid=" + ViewState["empid"] + "');return false;");


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSettings_Click(object sender, EventArgs e)
    {
        try
        {
            cmdSettings.Attributes.Add("onclick", "javascript:openNewWindow('Filters', '', '" + Session["sitepath"] + "/Options/OptionsChangePassword.aspx?empid=" + ViewState["empid"] + "');return false;");


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }



    protected void CmdNews_Click(object sender, EventArgs e)
    {
        CmdNews.Attributes.Add("onclick", "javascript:openNewWindow('Filters', 'News', 'https://www.executiveship.com/news.asp');return false;");

    }
}