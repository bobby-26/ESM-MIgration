using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using Telerik.Web.UI;
public partial class CrewEmployeeSuperintendentAssessment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewEmployeeSuperintendentAssessment.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvOC28')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuSupdtConcerns.AccessRights = this.ViewState;
            MenuSupdtConcerns.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                SetEmployeePrimaryDetails();

                gvOC28.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (PhoenixGeneralSettings.CurrentGeneralSetting.ShowSupdtFeedback == 1
                && (string.IsNullOrEmpty(Request.QueryString["IsCalledFrom"]) || !Request.QueryString["IsCalledFrom"].ToUpper().Equals("VSL"))
                )
            {
                toolbar.AddButton("Events", "EVENTS");
            }
            toolbar.AddButton("OC 28", "OC28");
            toolbar.AddButton("Score", "SCORE");
            MainMenuSupdtConcerns.AccessRights = this.ViewState;
            MainMenuSupdtConcerns.MenuList = toolbar.Show();

            if (PhoenixGeneralSettings.CurrentGeneralSetting.ShowSupdtFeedback == 1
              && (string.IsNullOrEmpty(Request.QueryString["IsCalledFrom"]) || !Request.QueryString["IsCalledFrom"].ToUpper().Equals("VSL"))
                )
            {
                MainMenuSupdtConcerns.SelectedMenuIndex = 1;
            }
            else
                MainMenuSupdtConcerns.SelectedMenuIndex = 0;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MainMenuSupdtConcerns_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EVENTS"))
            {
                Response.Redirect("../Crew/CrewInspectionSupdtConcernsList.aspx", false);
            }
            if (CommandName.ToUpper().Equals("SCORE"))
            {
                Response.Redirect("../Inspection/InspectionProsperScorePersonnelMaster.aspx?empid=" + General.GetNullableInteger(Filter.CurrentCrewSelection), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvOC28.Rebind();
    }


    protected void MenuSupdtConcerns_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

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


    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string[] alColumns = { "FLDVESSELNAME", "FLDTYPEDESCRIPTION", "FLDDATEOFJOINING", "FLDSIGNOFFDATE", "FLDASSESSMENTDATE", "FLDTOTALSCORE", "FLDTOTALSUPTSCORE", "FLDSCOREVARIENCE", "FLDSUPTNAME", "FLDSUPTCOMMENTS" };
            string[] alCaptions = { "Vessel", "Vsl Type", "Date Joined", "Tentative s/off", "Assessment Date", "Score", "Supt Score", "Difference", "Supt", "Supt Comments" };

            DataSet ds = PhoenixCrewEmployeeSuptAssessment.SearchEmployeeSuptAssessment(Convert.ToInt32(Filter.CurrentCrewSelection),
                                                                                1,
                                                                                iRowCount,
                                                                                ref iRowCount,
                                                                                ref iTotalPageCount);
            General.ShowExcel("OC 28", ds.Tables[0], alColumns, alCaptions, null, null);
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
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDVESSELNAME", "FLDTYPEDESCRIPTION", "FLDDATEOFJOINING", "FLDSIGNOFFDATE", "FLDASSESSMENTDATE", "FLDTOTALSCORE", "FLDTOTALSUPTSCORE", "FLDSCOREVARIENCE", "FLDSUPTNAME", "FLDSUPTCOMMENTS" };
            string[] alCaptions = { "Vessel", "Vsl Type", "Date Joined", "Tentative s/off", "Assessment Date", "Score", "Supt Score", "Difference", "Supt", "Supt Comments" };


            DataSet ds = PhoenixCrewEmployeeSuptAssessment.SearchEmployeeSuptAssessment(Convert.ToInt32(Filter.CurrentCrewSelection)
                                                                               , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                               , gvOC28.PageSize
                                                                               , ref iRowCount
                                                                               , ref iTotalPageCount);

            General.SetPrintOptions("gvOC28", "OC 28", alCaptions, alColumns, ds);


            gvOC28.DataSource = ds;
            gvOC28.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOC28_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOC28.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvOC28_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadLabel lblAssessmentId = (RadLabel)e.Item.FindControl("lblAssessmentId");
            RadLabel lblSignonOffId = (RadLabel)e.Item.FindControl("lblSignonOffId");
            RadLabel lblRank = (RadLabel)e.Item.FindControl("lblRank");
            if (e.CommandName.ToUpper().Equals("CREWEXPORT2XL"))
            {
                PhoenixCrew2XL.Export2XLCrewAssessment(Convert.ToInt32(Filter.CurrentCrewSelection)
                                                        , int.Parse(lblSignonOffId.Text.Trim())
                                                        , General.GetNullableGuid(lblAssessmentId.Text.Trim())
                                                        , lblRank.Text.Trim());

            }
            if (e.CommandName.ToUpper().Equals("SUPTEXPORT2XL"))
            {
                PhoenixCrew2XL.Export2XLSupdtAssessment(Convert.ToInt32(Filter.CurrentCrewSelection)
                                                        , int.Parse(lblSignonOffId.Text.Trim())
                                                        , General.GetNullableGuid(lblAssessmentId.Text.Trim())
                                                        , lblRank.Text.Trim());
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOC28_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucRemarks");
            RadLabel lb = (RadLabel)e.Item.FindControl("lblRemarks");
            RadLabel lblRank = (RadLabel)e.Item.FindControl("lblRank");

            LinkButton cmdCrewExport2XL = (LinkButton)e.Item.FindControl("cmdCrewExport2XL");
            LinkButton cmdSuptExport2XL = (LinkButton)e.Item.FindControl("cmdSuptExport2XL");
            
            if (lb != null)
            {
                lb.Attributes.Add("onmouseover", "showTooltip(ev,'" + uct.ToolTip + "', 'visible');");
                lb.Attributes.Add("onmouseout", "showTooltip(ev,'" + uct.ToolTip + "', 'hidden');");
            }
            if (SessionUtil.CanAccess(this.ViewState, cmdCrewExport2XL.CommandName))
            {
                cmdCrewExport2XL.Visible = false;
            }
            else
                cmdCrewExport2XL.Visible = false;

            if (SessionUtil.CanAccess(this.ViewState, cmdSuptExport2XL.CommandName)
                && PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0
                )
            {
                cmdSuptExport2XL.Visible = true;
            }
            else
                cmdSuptExport2XL.Visible = false;
            
            LinkButton cmdEmail = (LinkButton)e.Item.FindControl("cmdEmail");
            RadLabel lblSignonOffId = (RadLabel)e.Item.FindControl("lblSignonOffId");
            RadLabel lblAssessmentId = (RadLabel)e.Item.FindControl("lblAssessmentId");

            if (cmdEmail != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdEmail.CommandName)) cmdEmail.Visible = false;

                cmdEmail.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Crew/CrewEmail.aspx?empid=" + Filter.CurrentCrewSelection 
                    + "&SignonOffId=" + (lblSignonOffId != null ? lblSignonOffId.Text.Trim() : "")
                    + "&AssessmentId=" + (lblAssessmentId != null ? lblAssessmentId.Text.Trim() : "")
                    + "&Rank=" + (lblRank != null ? lblRank.Text.Trim() : "") + "&Assessment=0" + "'); return true;");
            }            
        }
        
    }

}
