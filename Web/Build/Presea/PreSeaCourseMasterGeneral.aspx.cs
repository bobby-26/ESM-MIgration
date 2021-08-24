using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;

public partial class PreSeaCourseMasterGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Course", "COURSE");
        toolbarmain.AddButton("Subjects", "COURSESUBJECT");
        toolbarmain.AddButton("Batch", "BATCH");
        PreSeaMenu.AccessRights = this.ViewState;
        PreSeaMenu.MenuList = toolbarmain.Show();
        PreSeaMenu.SelectedMenuIndex = 1;
        if (!Page.IsPostBack)
        {
          
            ViewState["ACCESS"] = "1";
            //PhoenixToolbar toolbarsub = new PhoenixToolbar();
            if (Filter.CurrentPreSeaCourseMasterSelection != "" && Filter.CurrentPreSeaCourseMasterSelection != null)
            {              
                ifMoreInfo.Attributes["src"] = "../PreSea/PreSeaCourseSubjects.aspx?CourseId=" + Filter.CurrentPreSeaCourseMasterSelection;
            }                   
        }
    }
    protected void PreSeaMenu_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (Filter.CurrentPreSeaCourseMasterSelection == null)
        {
            if (Filter.CurrentPreSeaCourseMasterSelection == null)
            {
                ucError.ErrorMessage = "  Please Select a Course .";
                ucError.Visible = true;
                PreSeaMenu.SelectedMenuIndex = 0;
            }
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            ifMoreInfo.Attributes["src"] = "PreSeaCourseSubjects.aspx";
        }
        if (Filter.CurrentPreSeaCourseMasterSelection == null)
        {
            ucError.ErrorMessage = " Please Select a Course ";
            ucError.Visible = true;
            Response.Redirect("../PreSea/PreSeaCourseMasterGeneral.aspx");
        }

        else if (dce.CommandName.ToUpper().Equals("BATCH"))
        {
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            ifMoreInfo.Attributes["src"] = "../PreSea/PreSeaBatch.aspx";        
        }
        else if (dce.CommandName.ToUpper().Equals("COURSESUBJECT"))
        {           
            ifMoreInfo.Attributes["src"] = "../PreSea/PreSeaCourseSubjects.aspx";
        }
        else if (dce.CommandName.ToUpper().Equals("COURSE"))
        {
            Response.Redirect("../PreSea/PreSeaCourseMaster.aspx", false);
        }

    }

    protected void PreSeaCourseGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if ((Filter.CurrentPreSeaCourseMasterSelection == null ))
        {
            ucError.ErrorMessage = " Please Select a Course ";
            ucError.Visible = true;
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../PreSea/PreSeaCourseSubjects.aspx.aspx";
            PreSeaMenu.SelectedMenuIndex = 0;
            PreSeaCourseGeneral.SelectedMenuIndex = 0;
        }
        else if (dce.CommandName.ToUpper().Equals("BATCH"))
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../PreSea/PreSeaBatch.aspx";
        }
        else if (dce.CommandName.ToUpper().Equals("COURSESUBJECT"))
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../PreSea/PreSeaCourseSubjects.aspx";
        }
        else if (dce.CommandName.ToUpper().Equals("BATCHENTRANCEDETAILS"))
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../PreSea/PreSeaBatchPlanDetails.aspx";
        }
        //else if (dce.CommandName.ToUpper().Equals("ENTRANCE"))
        //{
        //    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PreSea/PreSeaBatchPlanExamDetails.aspx";
        //}
        else if (dce.CommandName.ToUpper().Equals("BACK"))
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../PreSea/PreSeaCourseMaster.aspx";
        }

        ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString();

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
  
}
