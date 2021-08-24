using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceJob : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceJob.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvPlannedMaintenanceJob')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceJobFilter.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");

            MenuPlannedMaintenanceJob.AccessRights = this.ViewState;
            MenuPlannedMaintenanceJob.MenuList = toolbargrid.Show();
            //MenuPlannedMaintenanceJob.SetTrigger(pnlPlannedMaintenanceJob);

            if (!IsPostBack)
            {

                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("General", "GENERAL");
                toolbarmain.AddButton("Job Description", "DETAILS");
                toolbarmain.AddButton("Detail of Checks", "CHECKS");
                toolbarmain.AddButton("Components", "COMPONENTS");
                toolbarmain.AddButton("Attachment", "ATTACHMENT");
                toolbarmain.AddButton("History Template", "HISTORYTEMPLATE");
                toolbarmain.AddButton("JHA", "JHA");
                toolbarmain.AddButton("Remarks", "REMARKS");
                toolbarmain.AddButton("Parameter", "PARAMETER");
                //toolbarmain.AddButton("Add to Component", "ASIGN");
                MenuPlannedMaintenance.AccessRights = this.ViewState;
                MenuPlannedMaintenance.MenuList = toolbarmain.Show();
                //MenuPlannedMaintenance.SetTrigger(pnlPlannedMaintenanceJob);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["ASSIGNCOMPONENT"] = 0;
                ViewState["JOBID"] = null;

                ViewState["SETCURRENTNAVIGATIONTAB"] = null;

                if (Request.QueryString["JOBID"] != null && Request.QueryString["SETCURRENTNAVIGATIONTAB"] != null && Request.QueryString["JOBID"].ToString() != "")
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = Request.QueryString["SETCURRENTNAVIGATIONTAB"].ToString();
                    ViewState["JOBID"] = Request.QueryString["JOBID"].ToString();
                }
                if (Request.QueryString["JOBID"] != null)
                {
                    ViewState["JOBID"] = Request.QueryString["JOBID"].ToString();

                }
                MenuPlannedMaintenance.SelectedMenuIndex = 0;
                gvPlannedMaintenanceJob.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void PlannedMaintenance_TabStripCommand(object sender, EventArgs e)
    {

        try
        {
            ViewState["ASSIGNCOMPONENT"] = 0;
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceJobFilter.aspx");
            }
            else if (CommandName.ToUpper().Equals("GENERAL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceJobGeneral.aspx?JOBID=";
            }
            else if (CommandName.ToUpper().Equals("DETAILS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceJobDetail.aspx?JOBID=";
            }
            else if (CommandName.ToUpper().Equals("CHECKS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceJobDetailCheck.aspx?JOBID=";
            }
            else if (CommandName.ToUpper().Equals("COMPONENTS"))
            {
                ViewState["ASSIGNCOMPONENT"] = 1;
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentList.aspx?JOBID=";
            }
            else if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                GetAttachmentDtkey();
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&mod=" + PhoenixModule.PLANNEDMAINTENANCE;
            }
            else if (CommandName.ToUpper().Equals("HISTORYTEMPLATE"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceJobHistoryTemplateList.aspx?JOBID=";
            }
            else if (CommandName.ToUpper().Equals("JHA"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceJobJHAList.aspx?JOBID=";
            }
            else if (CommandName.ToUpper().Equals("REMARKS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Common/CommonRemarks.aspx?dtkey=" + ViewState["DTKEY"] + "&Applncode=6";
            }
            else if (CommandName.ToUpper().Equals("PARAMETER"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceJobParameterMapping.aspx?JOBID=";
            }
            //else if (dce.CommandName.ToUpper().Equals("ASIGN"))
            //{
            //    string selectedjobs = ",";
            //    foreach (GridViewRow gvr in gvPlannedMaintenanceJob.Rows)
            //    {
            //        if (gvr.FindControl("chkSelect") != null && ((CheckBox)(gvr.FindControl("chkSelect"))).Checked)
            //        {
            //            selectedjobs = selectedjobs + ((Label)(gvr.FindControl("lbljobid"))).Text + ",";
            //        }
            //    }

            //    if (selectedjobs.Length > 1)
            //        Response.Redirect("../PlannedMaintenance/PlannedMaintenanceJobAsignComponent.aspx?JOBIDS=" + selectedjobs);
            //    else
            //    {
            //        ucError.ErrorMessage = "You are not selected any jobs to Assign Component.";
            //        ucError.Visible = true;
            //    }
            //}

            if (CommandName.ToUpper().Equals("ATTACHMENT") || CommandName.ToUpper().Equals("REMARKS"))
            {
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString();
            }
            else
            {
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["JOBID"].ToString();
            }
            SetTabHighlight();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void ShowExcel()
    {
        try
        {

            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDJOBCODE", "FLDJOBTITLE", "FLDCLASS", "FLDFREQUENCYNAME" };
            string[] alCaptions = { "Code", "Title", "Job Class", "Frequency" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentJobFilter;
            DataSet ds = PhoenixCommonPlannedMaintenance.JobSearch(nvc != null ? nvc["txtJobCode"] : null,
                                                                    nvc != null ? nvc["txtJobTitle"] : null,
                                                                    General.GetNullableInteger(nvc != null ? nvc["ucJobClass"] : null),
                                                                    General.GetNullableInteger(nvc != null ? nvc["ucJobCategory"] : null),
                                                                    sortexpression, sortdirection,
                                gvPlannedMaintenanceJob.CurrentPageIndex + 1, gvPlannedMaintenanceJob.PageSize, ref iRowCount, ref iTotalPageCount);
            General.ShowExcel("Job", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void PlannedMaintenanceJob_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
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

    private void BindData()
    {
        try
        {
            int iRowCount = 10;
            int iTotalPageCount = 10;

            string[] alColumns = { "FLDJOBCODE", "FLDJOBTITLE", "FLDCLASS", "FLDFREQUENCYNAME" };
            string[] alCaptions = { "Code", "Title", "Job Class", "Frequency" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            NameValueCollection nvc = Filter.CurrentJobFilter;
            DataSet ds;

            ds = PhoenixCommonPlannedMaintenance.JobSearch(nvc != null ? nvc["txtJobCode"] : null,
                                                           nvc != null ? nvc["txtJobTitle"] : null,
                                                           General.GetNullableInteger(nvc != null ? nvc["ucJobClass"] : null),
                                                           General.GetNullableInteger(nvc != null ? nvc["ucJobCategory"] : null),
                                                           sortexpression, sortdirection,
                                                           gvPlannedMaintenanceJob.CurrentPageIndex + 1,
                                                           gvPlannedMaintenanceJob.PageSize, ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvPlannedMaintenanceJob", "Job", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvPlannedMaintenanceJob.DataSource = ds;
                gvPlannedMaintenanceJob.VirtualItemCount = iRowCount;

                if (ViewState["JOBID"] == null)
                {
                    ViewState["JOBID"] = ds.Tables[0].Rows[0][0].ToString();
                    ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                }

                if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceJobGeneral.aspx?JOBID=";
                }
                if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("CommonFileAttachment.aspx"))
                {
                    GetAttachmentDtkey();
                    ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&mod=" + PhoenixModule.PLANNEDMAINTENANCE;
                }
                if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("CommonRemarks.aspx"))
                {
                    GetAttachmentDtkey();
                    ifMoreInfo.Attributes["src"] = "../Common/CommonRemarks.aspx?dtkey=" + ViewState["DTKEY"] + "&Applncode=6";
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["JOBID"];
                }

                SetTabHighlight();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                gvPlannedMaintenanceJob.DataSource = "";

                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceJobGeneral.aspx?";
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void GetAttachmentDtkey()
    {
        if (ViewState["JOBID"].ToString() != null && ViewState["JOBID"] != null)
        {
            DataSet ds = PhoenixPlannedMaintenanceJob.EditJob(new Guid(ViewState["JOBID"].ToString()));
            DataRow dr = ds.Tables[0].Rows[0];
            ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["JOBID"] = null;
            if (ViewState["ASSIGNCOMPONENT"].ToString() == "1")
            {
                AssignComponent();
            }

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void AssignComponent()
    {
        //try
        //{
        //    string selectedjobs = ",";
        //    foreach (GridViewRow gvr in gvPlannedMaintenanceJob.Rows)
        //    {
        //        if (gvr.FindControl("chkSelect") != null && ((CheckBox)(gvr.FindControl("chkSelect"))).Checked)
        //        {
        //            selectedjobs = selectedjobs + ((Label)(gvr.FindControl("lbljobid"))).Text + ",";
        //        }
        //    }
        //    selectedjobs = selectedjobs + ViewState["JOBID"].ToString() + ",";

        //    NameValueCollection nvc = Filter.CurrentPickListSelection;
        //    string componentid = nvc.Get("lblComponentId").ToString();
        //    if (General.GetNullableGuid(componentid) != null)
        //    {
        //        PhoenixPlannedMaintenanceComponentJob.AsignComponentJob(new Guid(componentid),
        //        selectedjobs, PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        //    }
        //}
        //catch (Exception ex)
        //{
        //    ucError.ErrorMessage = ex.Message;
        //    ucError.Visible = true;
        //}
    }

    protected void SetTabHighlight()
    {
        try
        {
            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceJobGeneral.aspx"))
            {
                MenuPlannedMaintenance.SelectedMenuIndex = 0;

            }
            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceJobDetail.aspx"))
            {
                MenuPlannedMaintenance.SelectedMenuIndex = 1;

            }
            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceJobDetailCheck.aspx"))
            {
                MenuPlannedMaintenance.SelectedMenuIndex = 2;

            }
            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceComponentList.aspx"))
            {
                MenuPlannedMaintenance.SelectedMenuIndex = 3;

            }
            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("CommonFileAttachment.aspx"))
            {
                MenuPlannedMaintenance.SelectedMenuIndex = 4;
            }
            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceJobHistoryTemplateList.aspx"))
            {
                MenuPlannedMaintenance.SelectedMenuIndex = 5;
            }
            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceJobJHAList.aspx"))
            {
                MenuPlannedMaintenance.SelectedMenuIndex = 6;
            }
            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("CommonRemarks.aspx"))
            {
                MenuPlannedMaintenance.SelectedMenuIndex = 7;
            }
            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceJobParameterMapping.aspx"))
            {
                MenuPlannedMaintenance.SelectedMenuIndex = 8;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPlannedMaintenanceJob_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvPlannedMaintenanceJob_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPlannedMaintenanceJob_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixPlannedMaintenanceJob.DeleteJob(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(((RadLabel)e.Item.FindControl("lbljobid")).Text));
                ViewState["JOBID"] = null;
                gvPlannedMaintenanceJob.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                ViewState["JOBID"] = ((RadLabel)e.Item.FindControl("lbljobid")).Text;
                ViewState["DTKEY"] = ((RadLabel)e.Item.FindControl("lbldtkey")).Text;
                gvPlannedMaintenanceJob.Rebind();
            }
            if (e.CommandName == RadGrid.RebindGridCommandName)
            {
                gvPlannedMaintenanceJob.CurrentPageIndex = 0;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
