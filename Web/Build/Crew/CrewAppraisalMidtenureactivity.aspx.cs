using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;
public partial class CrewAppraisalMidtenureactivity : PhoenixBasePage
{

    string canedit = "1", canpostmstcomment = "1", canpostsupcomment = "1";
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ViewState["Rcategory"] = 0;
            ViewState["Category"] = 0;
            ViewState["AppraisalStatus"] = 1;
            ViewState["APPVESSEL"] = "";
            ViewState["VSLID"] = "";
            ViewState["SIGNONOFFID"] = "";
            ViewState["SIGNONDATE"] = "";
            //  ViewState["SCORE"] = "";
            ViewState["APPRAISALDATE"] = "";
            ViewState["OCCASIONID"] = "";
            ViewState["OCCASSIONNAME"] = "";

            cmdHiddenPick.Attributes.Add("style", "display:none;");
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            if (Request.QueryString["appraisalid"] != null)
                Filter.CurrentAppraisalSelection = Request.QueryString["appraisalid"].ToString();

            if (Request.QueryString["empid"] != null)
                Filter.CurrentCrewSelection = Request.QueryString["empid"].ToString();
            if (Request.QueryString["vslid"] != null)
                ViewState["VSLID"] = Request.QueryString["vslid"].ToString();

            if (Filter.CurrentMenuCodeSelection == "VAC-CRW-APL")
                ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

            SetEmployeePrimaryDetails();
            GetRankCategory();
            ddlOccassion.OccassionList = PhoenixRegistersMiscellaneousAppraisalOccasion.ListMiscellaneousAppraisalOccasion(General.GetNullableInteger(ViewState["Rcategory"].ToString()) == null ? 0 : int.Parse(ViewState["Rcategory"].ToString()), 1);
            ddlOccassion.Category = ViewState["Rcategory"].ToString();
            ddlOccassion.DataBind();

            DataSet ds = PhoenixCrewAppraisal.AppraisalSecurityEdit(int.Parse(ViewState["Rankid"].ToString()), General.GetNullableGuid(Filter.CurrentAppraisalSelection));

            if (ds.Tables[0].Rows.Count > 0)
            {
                canedit = ds.Tables[0].Rows[0]["FLDCANEDIT"].ToString();
                canpostmstcomment = ds.Tables[0].Rows[0]["FLDCANPOSTMSTCOMMENT"].ToString();
                canpostsupcomment = ds.Tables[0].Rows[0]["FLDCANPOSTSUPCOMMENT"].ToString();

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FLDAPPRAISALACTIVEYN"].ToString()))
                    hdnappraisalcomplatedyn.Value = ds.Tables[0].Rows[0]["FLDAPPRAISALACTIVEYN"].ToString();
                else
                    hdnappraisalcomplatedyn.Value = "1";
            }
            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            if (canedit.Equals("1"))
            {

                if (Filter.CurrentAppraisalSelection != null)
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                        toolbarmain.AddButton("Complete", "CONFIRM", ToolBarDirection.Right);
                    toolbarmain.AddButton("Save Changes", "SAVECHANGES", ToolBarDirection.Right);
                    divOtherSection.Visible = true;
                    divSignondate.Visible = true;
                    divPrimarySection.Visible = true;
                }
                else
                {
                    toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
                    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    {
                        ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                        ViewState["APPVESSEL"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    }
                    divOtherSection.Visible = false;
                    divSignondate.Visible = false;
                    divPrimarySection.Visible = true;
                }
                CrewAppraisal.AccessRights = this.ViewState;
                CrewAppraisal.MenuList = toolbarmain.Show();
            }
            if (hdnappraisalcomplatedyn.Value == "0" && PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                toolbarmain.AddButton("UnLock", "UNLOCK", ToolBarDirection.Right);
                divOtherSection.Visible = true;
                divSignondate.Visible = true;
                divPrimarySection.Visible = true;
                CrewAppraisal.AccessRights = this.ViewState;
                CrewAppraisal.MenuList = toolbarmain.Show();
            }
            EditAppraisal();

            toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Appraisal", "APPRAISAL");
            toolbarmain.AddButton("Form", "FORM");
            if (Filter.CurrentAppraisalSelection != null)
            {
                toolbarmain.AddButton("Seafarer  Comment", "SEAMANCOMMENT");
                if (ViewState["AppraisalStatus"].ToString() == "0" || PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                {
                    toolbarmain.AddButton("Appraisal Report", "APPRAISALREPORT");
                    toolbarmain.AddButton("Promotion Report", "PROMOTION");
                }
            }
            AppraisalTabs.AccessRights = this.ViewState;
            AppraisalTabs.MenuList = toolbarmain.Show();
            AppraisalTabs.SelectedMenuIndex = 1;


            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                chkAttachedCopyYN.Enabled = false;

        }
        DataSet ds2 = PhoenixCrewAppraisal.AppraisalSecurityEdit(int.Parse(ViewState["Rankid"].ToString()), General.GetNullableGuid(Filter.CurrentAppraisalSelection));

        if (ds2.Tables[0].Rows.Count > 0)
        {
            canedit = ds2.Tables[0].Rows[0]["FLDCANEDIT"].ToString();
            //if (!string.IsNullOrEmpty(ds2.Tables[0].Rows[0]["FLDAPPRAISALATIVEYN"].ToString()))
            //    isappraisalactive = ds2.Tables[0].Rows[0]["FLDAPPRAISALATIVEYN"].ToString();
        }

        EnablePage();
    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        if (Filter.CurrentPickListSelection == null)
            return;
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        EditAppraisal();
    }
    protected void AppraisalTabs_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            string reportcode = "";
            PhoenixCrewAppraisal.getAppraisalReportCode("CRW-APR", int.Parse(ViewState["OCCASIONID"].ToString()), General.GetNullableDateTime(ViewState["APPRAISALDATE"].ToString()), ref reportcode);
            if (CommandName.ToUpper().Equals("APPRAISAL"))
            {
                if (Request.QueryString["vslid"] != null)
                    Response.Redirect("CrewAppraisal.aspx?occ=1&vslid=" + Request.QueryString["vslid"], false);
                else
                    Response.Redirect("CrewAppraisal.aspx?occ=1", false);
            }
            if (CommandName.ToUpper().Equals("SEAMANCOMMENT"))
            {
                Response.Redirect("CrewAppraisalSeamanComment.aspx?occasionId=" + ddlOccassion.SelectedOccassion.ToString(), false);
            }
            if (CommandName.ToUpper().Equals("APPRAISALREPORT"))
            {
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=" + reportcode + "&appraisalid=" + Filter.CurrentAppraisalSelection + "&showmenu=0&showword=no&showexcel=no", false);
            }
            if (CommandName.ToUpper().Equals("PROMOTION"))
            {
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PROMOTIONMIDTENURE&appraisalid=" + Filter.CurrentAppraisalSelection + "&showmenu=0&showword=no&showexcel=no", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewAppraisal_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            string vessel = ddlVessel.SelectedVessel;
            string fromdate = txtFromDate.Text;
            string todate = txtToDate.Text;
            string appraisaldate = txtdate.Text;
            string occassion = ddlOccassion.SelectedOccassion;
            string RemarksIdList = ",";
            string RemarksList = "";

            if (CommandName.ToUpper().Equals("SAVE") || CommandName.ToUpper().Equals("SAVECHANGES"))
            {
                string iAppraisalId = "";

                if (!IsValidateAppraisal(vessel, fromdate, todate, occassion, appraisaldate))
                {
                    ucError.Visible = true;
                    return;
                }
                if (Filter.CurrentAppraisalSelection == null)
                {
                    PhoenixCrewAppraisal.InsertAppraisal(
                                                    int.Parse(Filter.CurrentCrewSelection)
                                                   , DateTime.Parse(fromdate)
                                                   , DateTime.Parse(todate)
                                                   , int.Parse(vessel)
                                                   , General.GetNullableDateTime(appraisaldate)
                                                   , int.Parse(ddlOccassion.SelectedOccassion)
                                                   , ref iAppraisalId
                                                   , General.GetNullableInteger(chkAttachedCopyYN.Checked ? "1" : "0")
                                                   );

                    Filter.CurrentAppraisalSelection = iAppraisalId.ToString();
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    PhoenixCrewAppraisal.UpdateAppraisal(new Guid(Filter.CurrentAppraisalSelection)
                                                , DateTime.Parse(fromdate)
                                                , DateTime.Parse(todate)
                                                , int.Parse(vessel)
                                                , General.GetNullableDateTime(appraisaldate)
                                                , int.Parse(occassion)
                                                , (hdnappraisalcomplatedyn.Value == "0") ? General.GetNullableByte(hdnappraisalcomplatedyn.Value) : null
                                                , General.GetNullableInteger(chkAttachedCopyYN.Checked ? "1" : "0"));

                    foreach (GridDataItem gv in gvmidturn.Items)
                    {
                        string score = ((RadDropDownList)gv.FindControl("ddlscore")).SelectedValue;
                        string ScoreId = ((RadLabel)gv.FindControl("lblAppraisalScoreid")).Text;
                        RadCheckBoxList chk = (RadCheckBoxList)gv.FindControl("chkRemarks");
                        foreach (ButtonListItem item in chk.Items)
                        {
                            if (item.Selected)
                            {
                                RemarksIdList = RemarksIdList + item.Value.ToString() + ",";
                                RemarksList = RemarksList + "<br>" + item.Text.ToString() + "<br/>";
                            }
                        }

                        if (!IsValidRemarks(score, RemarksIdList))
                        {
                            ucError.Visible = true;
                            return;
                        }

                        PhoenixCrewAppraisal.UpdateAppraisalMidTenureQuestion(new Guid(ScoreId.ToString()), int.Parse(score), RemarksIdList.ToString(), RemarksList.ToString());
                        RemarksIdList = ",";
                        RemarksList = "";

                    }
                }

                PhoenixCrewAppraisal.UpdateAppraisalYesNoQuestion(new Guid(Filter.CurrentAppraisalSelection)
                                                   , null, null, null, null, null, null, null
                                                   , 0, null, null
                                                   , General.GetNullableString(txtHeadOfDeptComment.Text)
                                                   , General.GetNullableString(txtMasteComment.Text)
                                                   , General.GetNullableString(txtSuperintendentComment.Text)
                                                   , null, null
                                                   , General.GetNullableString(txtSeafarerComment.Text)
                                                   , General.GetNullableInteger(ddlNoOfDocVisits.SelectedValue)
                                                   );

                ucStatus.Text = "Appraisal Information updated.";
                EditAppraisal();
                Rebind();
                EnablePage();
            }
            if (CommandName.ToUpper().Equals("CONFIRM"))
            {
                foreach (GridDataItem gv in gvmidturn.Items)
                {
                    string score = ((RadDropDownList)gv.FindControl("ddlscore")).SelectedValue;
                    string ScoreId = ((RadLabel)gv.FindControl("lblAppraisalScoreid")).Text;
                    RadCheckBoxList chk = (RadCheckBoxList)gv.FindControl("chkRemarks");
                    foreach (ButtonListItem item in chk.Items)
                    {
                        if (item.Selected)
                        {
                            RemarksIdList = RemarksIdList + item.Value.ToString() + ",";
                            RemarksList = RemarksList + "<br>" + item.Text.ToString() + "<br/>";

                        }
                    }


                    if (!IsValidRemarks(score, RemarksIdList))
                    {
                        ucError.Visible = true;
                        return;
                    }
                }
                if (General.GetNullableString(txtSeafarerComment.Text) == null && ViewState["OCCASSIONNAME"].ToString() != "OWF")
                {
                    ucError.ErrorMessage = "Seafarer Comments should be posted before completing the Appraisal.";
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewAppraisal.UpdateAppraisal(new Guid(Filter.CurrentAppraisalSelection)
                              , DateTime.Parse(fromdate)
                              , DateTime.Parse(todate)
                              , int.Parse(vessel)
                              , General.GetNullableDateTime(appraisaldate)
                              , int.Parse(occassion)
                              , 0
                              , General.GetNullableInteger(chkAttachedCopyYN.Checked ? "1" : "0")
                  );

                foreach (GridDataItem gv in gvmidturn.Items)
                {
                    string score = ((RadDropDownList)gv.FindControl("ddlscore")).SelectedValue;
                    string ScoreId = ((RadLabel)gv.FindControl("lblAppraisalScoreid")).Text;
                    RadCheckBoxList chk = (RadCheckBoxList)gv.FindControl("chkRemarks");
                    foreach (ButtonListItem item in chk.Items)
                    {
                        if (item.Selected)
                        {
                            RemarksIdList = RemarksIdList + item.Value.ToString() + ",";
                            RemarksList = RemarksList + "<br>" + item.Text.ToString() + "<br/>";
                        }

                    }

                    if (!IsValidRemarks(score, RemarksIdList))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixCrewAppraisal.UpdateAppraisalMidTenureQuestion(new Guid(ScoreId.ToString()), int.Parse(score), RemarksIdList.ToString(), RemarksList.ToString());
                    RemarksIdList = ",";
                    RemarksList = "";

                }

                PhoenixCrewAppraisal.UpdateAppraisalYesNoQuestion(new Guid(Filter.CurrentAppraisalSelection)
                                                                   , null, null, null, null, null, null, null
                                                                   , 0, null, null
                                                                   , General.GetNullableString(txtHeadOfDeptComment.Text)
                                                                   , General.GetNullableString(txtMasteComment.Text)
                                                                   , General.GetNullableString(txtSuperintendentComment.Text)
                                                                   , null, null
                                                                   , General.GetNullableString(txtSeafarerComment.Text)
                                                                   , General.GetNullableInteger(ddlNoOfDocVisits.SelectedValue)
                                                                   );

                ucStatus.Text = "This appraisal is finalised.";
                EditAppraisal();
                Rebind();
                Response.Redirect(Request.RawUrl);
            }
            if (CommandName.ToUpper().Equals("UNLOCK"))
            {
                ViewState["UNLOCK"] = "1";
                ResetMenu();
                txtHeadOfDeptComment.Enabled = true;
                txtMasteComment.Enabled = true;
                txtSeafarerComment.ReadOnly = false;
                txtSeafarerComment.Enabled = true;
                txtSeafarerComment.CssClass = "input_mandatory";
            }
            if (CommandName.ToUpper().Equals("LOCK"))
            {
                ViewState["UNLOCK"] = "0";
                ResetMenu();
                txtHeadOfDeptComment.Enabled = false;
                txtMasteComment.Enabled = false;
                txtSeafarerComment.ReadOnly = true;
                txtSeafarerComment.CssClass = "readonlytextbox";
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {

        gvmidturn.DataSource = null;
        gvmidturn.Rebind();
        EnablePage();
    }
    protected void gvmidturn_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

    private void EnablePage()
    {
        bool editable = canedit.Equals("0") ? false : true; //Enable or disable all controls
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0 || Filter.CurrentMenuCodeSelection == "VAC-CRW-APL")
            ddlVessel.Enabled = false;
        else
            txtFromDate.Enabled = editable;
        txtToDate.Enabled = editable;
        txtdate.Enabled = editable;

        txtSuperintendentComment.Enabled = canpostsupcomment.Equals("0") ? false : true;
        txtMasteComment.Enabled = canpostmstcomment.Equals("0") ? false : true;

        if (hdnappraisalcomplatedyn.Value == "0")
        {
            //foreach (GridDataItem gvr in gvmidturn.Items)
            //{
            //    if (((RadDropDownList)gvr.FindControl("ddlscore")) != null)
            //        ((RadDropDownList)gvr.FindControl("ddlscore")).Enabled = false;
            //    if (((RadCheckBoxList)gvr.FindControl("chkRemarks")) != null)
            //        ((RadCheckBoxList)gvr.FindControl("chkRemarks")).Enabled = false;
            //}
            ddlNoOfDocVisits.Enabled = false;
        }
    }

    private void EditAppraisal()
    {

        if (Filter.CurrentAppraisalSelection != null)
        {
            DataSet ds = PhoenixCrewAppraisal.EditAppraisal(new Guid(Filter.CurrentAppraisalSelection));

            if (ds.Tables[0].Rows.Count > 0)
            {

                ddlVessel.SelectedVessel = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                ViewState["APPVESSEL"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                txtSignOnDate.Text = ds.Tables[0].Rows[0]["FLDSIGNONDATE"].ToString();
                ViewState["SIGNONOFFID"] = ds.Tables[0].Rows[0]["FLDSIGNONOFFID"].ToString();
                ViewState["SIGNONDATE"] = ds.Tables[0].Rows[0]["FLDSIGNONDATE"].ToString();

                if (ds.Tables[0].Rows[0]["FLDATTACHEDSCANCOPYYN"].ToString().Equals("1"))
                    chkAttachedCopyYN.Checked = true;
                else
                    chkAttachedCopyYN.Checked = false;

                hdnSeamen.Value = ds.Tables[0].Rows[0]["FLDSEAMANCOMMENT"].ToString();

                txtSeafarerComment.Text = HttpUtility.HtmlDecode(ds.Tables[0].Rows[0]["FLDSEAMANCOMMENT"].ToString());
                txtHeadOfDeptComment.Text = ds.Tables[0].Rows[0]["FLDHEADDEPTCOMMENT"].ToString();
                txtMasteComment.Text = ds.Tables[0].Rows[0]["FLDMASTERCOMMENT"].ToString();
                txtSuperintendentComment.Text = ds.Tables[0].Rows[0]["FLDSUPERINTENDENTCOMMENT"].ToString();

                ddlNoOfDocVisits.SelectedValue = ds.Tables[0].Rows[0]["FLDNOOFDOCTORVISIT"].ToString();

                ViewState["APPRAISALDATE"] = ds.Tables[0].Rows[0]["FLDAPPRAISALDATE"].ToString();
                ViewState["OCCASIONID"] = ds.Tables[0].Rows[0]["FLDOCCASSIONFORREPORT"].ToString();

                ViewState["AppraisalStatus"] = int.Parse(ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString());
                GetOccasionName(int.Parse(ViewState["OCCASIONID"].ToString()), new Guid(Filter.CurrentAppraisalSelection));
                if (ViewState["OCCASSIONNAME"].ToString() == "OWF")
                {
                    if (ds.Tables[0].Rows[0]["FLDNOOFDOCTORVISIT"].ToString() == string.Empty)
                    {
                        ddlNoOfDocVisits.SelectedIndex = 1;
                        ddlNoOfDocVisits.Enabled = false;
                    }

                    lblHeadOfDept.Text = "Comments";
                    txtMasteComment.ReadOnly = true;
                    txtMasteComment.CssClass = "readonlytextbox";
                    txtSuperintendentComment.ReadOnly = true;
                    txtSuperintendentComment.CssClass = "readonlytextbox";
                    lblSeafarerComment.Visible = false;
                }
            }
        }
    }

    public void GetRankCategory()
    {
        string Rcategory = null;

        PhoenixCrewAppraisalProfile.GetRankCategory(int.Parse(ViewState["Rankid"].ToString()), ref Rcategory);

        if (Rcategory == System.DBNull.Value.ToString())
            Rcategory = "0";

        ViewState["Rcategory"] = Rcategory.ToString();
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = new DataTable();
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                dt = PhoenixVesselAccountsEmployee.EditVesselCrew(PhoenixSecurityContext.CurrentSecurityContext.InstallCode, Convert.ToInt32(Filter.CurrentCrewSelection));
            else
                dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtEmployeeName.Text = dt.Rows[0]["FLDEMPLOYEENAME"].ToString();
                txtBatch.Text = dt.Rows[0]["FLDBATCH"].ToString();

                txtRank.Text = dt.Rows[0]["FLDRANKPOSTEDNAME"].ToString();
                ddlVessel.SelectedVessel = dt.Rows[0]["FLDPRESENTVESSELID"].ToString();
                if (Filter.CurrentAppraisalSelection == null)
                {
                    ViewState["Rankid"] = dt.Rows[0]["FLDRANKPOSTED"].ToString();
                    ViewState["empid"] = dt.Rows[0]["FLDEMPLOYEEID"].ToString();
                }
            }

            if (Filter.CurrentAppraisalSelection != null)
            {
                DataSet ds = PhoenixCrewAppraisal.EditAppraisal(new Guid(Filter.CurrentAppraisalSelection));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlVessel.SelectedValue = int.Parse(ds.Tables[0].Rows[0]["FLDVESSELID"].ToString());
                    txtRank.Text = ds.Tables[0].Rows[0]["FLDRANKNAME"].ToString();
                    txtFromDate.Text = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDFROMDATE"].ToString());
                    txtToDate.Text = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDTODATE"].ToString());
                    ddlOccassion.SelectedOccassion = ds.Tables[0].Rows[0]["FLDOCCASSIONFORREPORT"].ToString();
                    txtdate.Text = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDAPPRAISALDATE"].ToString());
                    ViewState["Rankid"] = ds.Tables[0].Rows[0]["FLDRANKID"].ToString();
                    ViewState["empid"] = ds.Tables[0].Rows[0]["FLDEMPLOYEEID"].ToString();
                    ViewState["SIGNONOFFID"] = ds.Tables[0].Rows[0]["FLDSIGNONOFFID"].ToString();
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindData()
    {
        try
        {
            DataSet ds = PhoenixCrewAppraisal.AppraisalMidTernSearch(new Guid(Filter.CurrentAppraisalSelection), int.Parse(ViewState["Rankid"].ToString()));
            gvmidturn.DataSource = ds.Tables[0];
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    //{
    //    int nextRow = 0;
    //    GridView _gridView = (GridView)sender;

    //    if (e.Row.RowType == DataControlRowType.DataRow
    //        && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
    //        || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
    //    {
    //        int nRows = ((DataTable)_gridView.DataSource).Rows.Count - 1;

    //        String script = "var keyValue = SelectSibling(event); ";
    //        script += " if(keyValue == 38) {";  //Up Arrow
    //        nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

    //        script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
    //        script += "}";

    //        script += " if(keyValue == 40) {";  //Down Arrow
    //        nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

    //        script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
    //        script += "}";
    //        script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
    //        e.Row.Attributes["onkeydown"] = script;
    //    }

    //}
    protected void gvmidturn_ItemDataBound(Object sender, GridItemEventArgs e)
    {


        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            // LinkButton db = (LinkButton)e.Item.FindControl("cmdattachedimg");
            RadDropDownList ddlScore = (RadDropDownList)e.Item.FindControl("ddlscore");
            if (ddlScore != null) ddlScore.SelectedValue = drv["FLDSCORE"].ToString();

            RadCheckBoxList chkUserGroupEdit = (RadCheckBoxList)e.Item.FindControl("chkRemarks");
            if (chkUserGroupEdit != null)
            {
                chkUserGroupEdit.DataSource = PhoenixRegistersAppraisalProfileQuestion.ListSelectedAppraisalRemarks(General.GetNullableInteger(drv["FLDAPPRAISALQUESTIONID"].ToString()), General.GetNullableInteger(drv["FLDRANKCLASSIFICATION"].ToString()), General.GetNullableInteger(drv["FLDPROFILECATEGORYID"].ToString()));
                chkUserGroupEdit.DataBindings.DataTextField = "FLDAPPRAISALREMARKS";
                chkUserGroupEdit.DataBindings.DataValueField = "FLDAPPRAISALREMARKSID";
                chkUserGroupEdit.DataBind();

                RadCheckBoxList chk = (RadCheckBoxList)e.Item.FindControl("chkRemarks");
                BindCheckBoxList(chk, drv["FLDREMARKSIDLIST"].ToString());

                if (hdnappraisalcomplatedyn.Value == "0")
                {                       
                        chkUserGroupEdit.Enabled = false;
                    ddlScore.Enabled = false;
                 }
            }
        }
    }
    public static void BindCheckBoxList(RadCheckBoxList cbl, string list)
    {
        foreach (string item in list.Split(','))
        {
            foreach (ButtonListItem li in cbl.Items)
            {
                if (li.Value == item.ToString())
                    li.Selected = true;
            }
        }
    }
    protected void gvmidturn_PreRender(object sender, EventArgs e)
    {
        MergeRows(gvmidturn);
    }
    public static void MergeRows(RadGrid gridView)
    {
        for (int rowIndex = gridView.Items.Count - 2; rowIndex >= 0; rowIndex--)
        {
            GridDataItem row = gridView.Items[rowIndex];
            GridDataItem previousRow = gridView.Items[rowIndex + 1];

            string currentCategoryName = ((RadLabel)gridView.Items[rowIndex].FindControl("lblJobrole")).Text;
            string previousCategoryName = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblJobrole")).Text;

            if (currentCategoryName == previousCategoryName)
            {
                row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                    previousRow.Cells[1].RowSpan + 1;
                previousRow.Cells[1].Visible = false;
                row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                  previousRow.Cells[2].RowSpan + 1;
                previousRow.Cells[2].Visible = false;
            }
        }

    }
    //protected void ddlscore_Changed(object sender, EventArgs e)
    //{
    //    ViewState["SCORE"] = ddlscore.SelectedValue;
    //}
    private bool IsValidRemarks(string score, string remarksIdList)
    {
        ucError.HeaderMessage = "Please provide the following required  Primary Details information";

        if (score == "3" && (remarksIdList.ToString() == string.Empty || remarksIdList.ToString() == ",") && ViewState["OCCASSIONNAME"].ToString() != "OWF")
        {
            ucError.ErrorMessage = "Remarks is mandatory if Rating is Below Expectation (BE).";
        }
        if (score == "3" && ViewState["OCCASSIONNAME"].ToString() == "OWF")
        {
            ucError.ErrorMessage = "Comment is mandatory if Rating is Below Expectation (BE).";
        }
        return (!ucError.IsError);
    }
    private bool IsValidateAppraisal(string vessel, string fromdate, string todate, string occassion, string appraisaldate)
    {
        ucError.HeaderMessage = "Please provide the following required  Primary Details information";

        int result;
        DateTime resultdate;
        if (!int.TryParse(vessel, out result))
            ucError.ErrorMessage = "Vessel is required.";
        if (fromdate == null || !DateTime.TryParse(fromdate, out resultdate))
            ucError.ErrorMessage = "From Date is required";
        if (todate == null || !DateTime.TryParse(todate, out resultdate))
            ucError.ErrorMessage = "To Date is required";
        else if (!string.IsNullOrEmpty(fromdate)
              && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "'To Date' should be later than 'From Date'";
        }
        if (!string.IsNullOrEmpty(fromdate)
            && DateTime.Compare(DateTime.Today, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "'From date' should not be future date";
        }
        if (!string.IsNullOrEmpty(todate)
            && DateTime.Compare(DateTime.Today, DateTime.Parse(todate)) < 0)
        {
            ucError.ErrorMessage = "'To date' should not be future date";
        }
        if (occassion.ToUpper() == "DUMMY" || occassion == "")
        {
            ucError.Text = "Please Select Occassion For Report";
        }
        if (appraisaldate == null || !DateTime.TryParse(appraisaldate, out resultdate))
            ucError.ErrorMessage = "Appraisal Report Date  is required";
        else if (!string.IsNullOrEmpty(todate)
              && DateTime.TryParse(appraisaldate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(todate)) < 0)
        {
            ucError.ErrorMessage = "'Appraisal Report Date' should be later than or equal to 'To Date'";
        }
        if (!string.IsNullOrEmpty(appraisaldate)
            && DateTime.Compare(DateTime.Today, DateTime.Parse(appraisaldate)) < 0)
        {
            ucError.ErrorMessage = "'Appraisal date' should not be future date";
        }
        if (hdnappraisalcomplatedyn.Value == "0" && General.GetNullableString(txtSeafarerComment.Text) == null)
        {
            ucError.ErrorMessage = "Sefarer comment is required";
        }

        return (!ucError.IsError);
    }
    public void GetOccasionName(int OccasionId, Guid AppraisalId)
    {
        string OccasionName = null;
        string Appraisalnew = "0";

        PhoenixCrewAppraisal.AppraisalOccasionName(OccasionId, AppraisalId, ref OccasionName, ref Appraisalnew);

        if (OccasionName == System.DBNull.Value.ToString())
            OccasionName = "";

        if (Appraisalnew == System.DBNull.Value.ToString())
            Appraisalnew = "0";

        ViewState["OCCASSIONNAME"] = OccasionName.ToString();
    }
    private void ResetMenu()
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (ViewState["UNLOCK"] != null && ViewState["UNLOCK"].ToString() == "1")
            {
                toolbar.AddButton("Lock", "LOCK", ToolBarDirection.Right);
                toolbar.AddButton("Save Changes", "SAVECHANGES", ToolBarDirection.Right);
                
            }
            if (ViewState["UNLOCK"] != null && ViewState["UNLOCK"].ToString() == "0")
                toolbar.AddButton("UnLock", "UNLOCK", ToolBarDirection.Right);

            CrewAppraisal.AccessRights = this.ViewState;
            CrewAppraisal.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
