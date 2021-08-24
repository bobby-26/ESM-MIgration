using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using Telerik.Web.UI;

public partial class PreSeaTraineePersonalGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Personal", "PERSONAL", CreatePersonalSubTab(string.Empty, 0, string.Empty));
        toolbarmain.AddButton("History ", "HISTORY", CreateHistorySubTab(string.Empty, 0, string.Empty));
        toolbarmain.AddButton("Training", "TRAINING", CreateTrainingSubTab(string.Empty, 0, string.Empty));
        toolbarmain.AddButton("Corres.", "CORRESPONDENCE", CreateCorrespondenceSubTab(string.Empty, 0, string.Empty));
        if (Request.QueryString["back"] != null)
        {
            toolbarmain.AddButton("Back", "LIST");
        }
        PreSeaMenu.AccessRights = this.ViewState;
        PreSeaMenu.MenuList = toolbarmain.Show();
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        if (!Page.IsPostBack)
        {
            ViewState["ACCESS"] = "1";
            if (ViewState["ACCESS"].ToString() == "1")
            {
                CreatePersonalSubTab("../PreSea/PreSeaTraineePersonal.aspx", 0, "SUBPERSONAL");
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                {
                    Filter.CurrentPreSeaTraineeSelection = Request.QueryString["empid"];
                }
            }
        }
    }
	protected void PreSeaMenu_TabStripCommand(object sender, EventArgs e)
	{
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (Filter.CurrentPreSeaTraineeSelection == null || CommandName.ToUpper().Equals("PERSONAL"))
        {
            if (Filter.CurrentPreSeaTraineeSelection == null && Request.QueryString["t"] == null)
            {
                ucError.ErrorMessage = "  Please Select a Person .";
                ucError.Visible = true;
                PreSeaMenu.SelectedMenuIndex = 0;
            }
            CreatePersonalSubTab("PreSeaTraineePersonal.aspx", 0, "SUBPERSONAL");
        }
        else if (CommandName.ToUpper().Equals("LIST"))
        {
            Response.Redirect("PreSeaTraineeQueryActivity.aspx?p=" + Request.QueryString["p"], false);
        }
        else
        {
            PreSeaMenuGeneral_TabStripCommand(sender, e);
        }       
    }

	protected void PreSeaMenuGeneral_TabStripCommand(object sender, EventArgs e)
	{
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
               
        if (CommandName.ToUpper().Equals("SUBPERSONAL"))
        {
            CreatePersonalSubTab("../PreSea/PreSeaTraineePersonal.aspx" + (Request.QueryString["r"] != null ? ("?r=n") : string.Empty), 0, "SUBPERSONAL");        
        }
        else if (CommandName.ToUpper().Equals("ADDRESS"))
        {
            CreatePersonalSubTab("../PreSea/PreSeaTraineePersonalAddress.aspx", 1, "ADDRESS");
        }
        else if (CommandName.ToUpper().Equals("FAMILY/NOK"))
        {
            CreatePersonalSubTab("../PreSea/PreSeaTraineeFamilyNok.aspx", 1, "FAMILY/NOK");
        }
        else if (CommandName.ToUpper().Equals("ACADEMICS"))
        {
            CreateHistorySubTab("../PreSea/PreSeaTraineeAcademicQualification.aspx", 1, "ACADEMICS");
        }
        else if (CommandName.ToUpper().Equals("OTHERS"))
        {
            CreatePersonalSubTab("../PreSea/PreSeaTraineeOthers.aspx", 1, "OTHERS");
        }
        else if (CommandName.ToUpper().Equals("COURSE&CERTIFICATES"))
        {
            CreateTrainingSubTab("../PreSea/PreSeaTraineeCourseAndCertificate.aspx", 1, "COURSE&CERTIFICATES");
        }
        else if (CommandName.ToUpper().Equals("CORRESPONDENCE"))
        {
            CreateCorrespondenceSubTab("../PreSea/PreSeaTraineeCorrespondence.aspx", 1, "CORRESPONDENCE");
        }
        else if (CommandName.ToUpper().Equals("EMAIL"))
        {
            CreateCorrespondenceSubTab("../PreSea/PreSeaTraineeCorrespondence.aspx", 2, "EMAIL");
        }
        else if (CommandName.ToUpper().Equals("TRAVELDOCUMENTS"))
        {
            CreatePersonalSubTab("../PreSea/PreSeaTraineeTravelDocument.aspx", 1, "TRAVELDOCUMENTS");
        }
        else if (CommandName.ToUpper().Equals("OTHERDOCUMENTS"))
        {
            CreatePersonalSubTab("../PreSea/PreSeaTraineeOtherDocument.aspx", 1, "OTHERDOCUMENTS");
        }
        else if (CommandName.ToUpper().Equals("PLACEMENT"))
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../Presea/PreSeaPlacementDetails.aspx";
        }
        else if (CommandName.ToUpper().Equals("EXAM"))
        {
            CreateHistorySubTab("../Presea/PreSeaExaminationResults.aspx", 2, "EXAM");
        }
	}

	private Guid GetCurrentEmployeeDTkey()
	{
		Guid dtkey = Guid.Empty;
		try
		{

			DataTable dt = PhoenixPreSeaTrainee.PreSeaTraineeEdit(General.GetNullableInteger(Filter.CurrentPreSeaTraineeSelection));
			if (dt.Rows.Count > 0)
			{
				dtkey = new Guid(dt.Rows[0]["FLDDTKEY"].ToString());
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
		return dtkey;
	}
    private PhoenixToolbar CreatePersonalSubTab(string Page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();

        toolbarsub.AddButton("Address ", "ADDRESS");
        toolbarsub.AddButton("Family/Nok", "FAMILY/NOK");
        toolbarsub.AddButton("Documents", "TRAVELDOCUMENTS");
        toolbarsub.AddButton("Othr. Documents", "OTHERDOCUMENTS");
        ifMoreInfo.Attributes["src"] = SessionUtil.CanAccess(this.ViewState, Cmd) ? Page : "../PhoenixAccessDenied.aspx";
        return toolbarsub;
    }

    private PhoenixToolbar CreateHistorySubTab(string Page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Academics", "ACADEMICS");        
        toolbarsub.AddButton("Exam Marks/Results", "EXAM");     
        //ifMoreInfo.Attributes["src"] = "PreSeaTraineeAcademicQualification.aspx";        
        ifMoreInfo.Attributes["src"] = Page;
        return toolbarsub;
    }

    private PhoenixToolbar CreateTrainingSubTab(string Page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Course", "COURSE&CERTIFICATES");
        ifMoreInfo.Attributes["src"] = Page;
        return toolbarsub;        
    }
    private PhoenixToolbar CreateCorrespondenceSubTab(string Page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Corres.", "CORRESPONDENCE");
        toolbarsub.AddButton("Email", "EMAIL");      
        ifMoreInfo.Attributes["src"] = SessionUtil.CanAccess(this.ViewState, Cmd) ? Page : "../PhoenixAccessDenied.aspx";
        return toolbarsub;
    }
}
