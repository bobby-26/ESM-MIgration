using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class CrewOffshore_CrewoffshoreTrainingNeedTaskOverride : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        confirm.Attributes.Add("style", "display:none");
        if (!IsPostBack)
        {

            rbtoption.SelectedIndex = 2;
            ViewState["PAGENUMBER"] = 1;
            ViewState["PAGENUMBER1"] = 1;
            ViewState["PAGENUMBER2"] = 1;
            ViewState["PAGENUMBER3"] = 1;
            ViewState["employeeid"] = "";
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["coursetype"] = "";
            ViewState["examrequestid"] = "";
            ViewState["employeeid"] = null;
            gvTask.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        if (Request.QueryString["employeeid"].ToString() != null && Request.QueryString["employeeid"].ToString() != "")
        {
            ViewState["employeeid"] = Request.QueryString["employeeid"].ToString();

        }
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("CBT", "CBT", ToolBarDirection.Left);
        toolbarsub.AddButton("Training Course", "TRAINING COURSE", ToolBarDirection.Left);
        toolbarsub.AddButton("Task", "TASK", ToolBarDirection.Left);
        toolbarsub.AddButton("Back", "BACK", ToolBarDirection.Left);

        TrainingNeed.AccessRights = this.ViewState;
        TrainingNeed.MenuList = toolbarsub.Show();
        TrainingNeed.SelectedMenuIndex = 2;
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = new string[0];
        string[] alCaptions = new string[0];

        alColumns = new string[] { "FLDTRAININGNEEDSTATUSNAME", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDFILENO", "FLDVESSELNAME" };
        //"FLDTYPEOFTRAININGNAME", "FLDREQUESTEDCOURSESNAME", "FLDTOBEDONEBYNAME", "FLDDATECOMPLETED", "FLDEXAMNAME","FLDNOOFATTEMPTS","FLDSCORE","FLDEXAMRESULT","FLDDATEATTENDED"};
        alCaptions = new string[] { "Status", "Name", "Rank", "File No", "Vessel" };
        //, "Category", "SubCategory", "Identified Training Need", "Level of Improvement",
        //            "Type of Training", "Course/CBT", "To be done by", "Date Completed", "Test", "No Of Attempts","Score %","Result","Date Attended"};



        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentOffshoreTrainingNeedSearch;
        int? vesselid = null;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        }
        else if (nvc != null)
        {
            if (General.GetNullableInteger(nvc.Get("ucVessel")) != null)
                vesselid = General.GetNullableInteger(nvc.Get("ucVessel"));
        }

        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.TrainingneedsEmployeeList(General.GetNullableInteger(ViewState["employeeid"].ToString()));

        lblfname.Text = dt.Rows[0]["FLDEMPLOYEENAME"].ToString();
        txtfileno.Text = dt.Rows[0]["FLDFILENO"].ToString();
        lblrname.Text = dt.Rows[0]["FLDRANKNAME"].ToString();

        dt = PhoenixCrewOffshoreCompetencyTask.SearchTrainingNeeds(General.GetNullableInteger(ViewState["employeeid"].ToString()),
                          sortexpression, sortdirection,
                          gvTask.CurrentPageIndex + 1,
                          gvTask.PageSize,
                          ref iRowCount,
                          ref iTotalPageCount, null, 1);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshoreTraining", "Pending CBT", alCaptions, alColumns, ds);


        gvTask.DataSource = ds;
        gvTask.VirtualItemCount = iRowCount;


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void TrainingNeed_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        string coursetype;
        if (CommandName.ToUpper().Equals("CBT"))
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "4");
            Response.Redirect("../CrewOffshore/CrewOffshoreTrainingNeedsCBTDetail.aspx?employeeid=" + ViewState["employeeid"].ToString() + "&coursetype=" + coursetype, true);
        }
        else if (CommandName.ToUpper().Equals("TRAINING COURSE"))
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "7");
            Response.Redirect("../CrewOffshore/CrewOffshoreTrainingNeedsDetail.aspx?employeeid=" + ViewState["employeeid"].ToString() + "&coursetype=" + coursetype, true);
        }
        else if (CommandName.ToUpper().Equals("BACK"))
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "4");
            Response.Redirect("../CrewOffshore/CrewOffshoreTrainingNeeds.aspx?coursetype=" + coursetype, true);
        }
        else if (CommandName.ToUpper().Equals("TASK"))
        {
            Response.Redirect("../CrewOffshore/CrewoffshoreTrainingNeedTasksDetails.aspx?employeeid=" + ViewState["employeeid"].ToString(), true);
        }
    }

    protected void gvTask_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTask.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void btnCrewApprove_Click(object sender, EventArgs e)
    {
        try
        {
           
            PhoenixCrewOffshoreTrainingNeeds.UpdateTrainingNeedOverride(General.GetNullableGuid(ViewState["trainingneedid"].ToString())
                   , 0        
                   , General.GetNullableString(""));


            ucStatus.Text = "Overrided cancel successfully.";
            BindData();
            gvTask.Rebind();

           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOffshoreTraining1_TabStripCommand(object sender, EventArgs e)
    {

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void rbtoption_SelectedIndexChanged(object sender, EventArgs e)
    {
        string coursetype;
        if (rbtoption.SelectedIndex == 0)
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "4");
            Response.Redirect("../CrewOffshore/CrewoffshoreTrainingNeedTasksDetails.aspx?employeeid=" + ViewState["employeeid"].ToString() + "&coursetype=" + coursetype, true);
        }
        else if (rbtoption.SelectedIndex == 1)
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "4");
            Response.Redirect("../CrewOffshore/CrewoffshoreTrainingNeedTasksCompleted.aspx?employeeid=" + ViewState["employeeid"].ToString() + "&coursetype=" + coursetype, true);

        }
        else if (rbtoption.SelectedIndex == 2)
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "4");
            Response.Redirect("../CrewOffshore/CrewoffshoreTrainingNeedTaskOverride.aspx?employeeid=" + ViewState["employeeid"].ToString() + "&coursetype=" + coursetype, true);
        }
    }

    protected void gvTask_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblEmployeeid = (RadLabel)e.Item.FindControl("lblEmployeeid");
            LinkButton cmdOverride = (LinkButton)e.Item.FindControl("cmdOverride");
            if (cmdOverride != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdOverride.CommandName))
                    cmdOverride.Visible = false;

                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    cmdOverride.ToolTip = "Show Override Remarks";
            }
        }

    }

    protected void gvTask_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToString().ToUpper() == "OVERRIDE")
        {

            RadLabel trainingneedid = ((RadLabel)e.Item.FindControl("lblTrainingNeedId"));
            ViewState["trainingneedid"] = trainingneedid.Text;
            //UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
            //ucConfirmCrew.Visible = true;
            //ucConfirmCrew.Text = "Do you want to cancel override for this course?";

            RadWindowManager1.RadConfirm("Do you want to cancel override for this course?", "confirm", 320, 150, null, "Confirm");

        }
    }
}