using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;


public partial class PreSeaInterviewScoreCardBTECHLE : PhoenixBasePage
{
    int AcademicCount, WrittenCount, GeneralCount;

    #region :   Page Events :

    protected override void Render(HtmlTextWriter writer)
    {        
        decimal pcmMax = 0, pcmAllot = 0;
        foreach (GridViewRow r in gvAcademicSection.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow && SessionUtil.CanAccess(this.ViewState, "EDIT"))
            {
                Page.ClientScript.RegisterForEventValidation(gvAcademicSection.UniqueID, "Edit$" + r.RowIndex.ToString());
            }

            int rowIndex = r.RowIndex;

            if (r.RowType == DataControlRowType.DataRow)
            {
                Label lblFieldDesc = (Label)gvAcademicSection.Rows[rowIndex].FindControl("lblFieldDesc");
                Label lblScoredPercentage = (Label)gvAcademicSection.Rows[rowIndex].FindControl("lblScoredPercentage");
                Label lblMax = (Label)gvAcademicSection.Rows[rowIndex].FindControl("lblMax");
                Label lblAllot = (Label)gvAcademicSection.Rows[rowIndex].FindControl("lblAlloted");
                UserControlMaskNumber ucMax = (UserControlMaskNumber)gvAcademicSection.Rows[rowIndex].FindControl("ucMax");
                UserControlMaskNumber ucAlotted = (UserControlMaskNumber)gvAcademicSection.Rows[rowIndex].FindControl("ucAlotted");
                Label lblPercentage = (Label)gvAcademicSection.Rows[rowIndex].FindControl("lblPercentage");

                if ((lblFieldDesc.Text.ToUpper() == "HSC PHYSICS" ||
                        lblFieldDesc.Text.ToUpper() == "HSC CHEMISTRY" ||
                        lblFieldDesc.Text.ToUpper() == "HSC MATHS"))
                {
                    if (lblMax != null && !string.IsNullOrEmpty(lblMax.Text.Trim()))
                        pcmMax += decimal.Parse(lblMax.Text.Trim());
                    if (lblAllot != null && !string.IsNullOrEmpty(lblAllot.Text.Trim()))
                        pcmAllot += decimal.Parse(lblAllot.Text.Trim());
                }

                if (((Label)gvAcademicSection.Rows[rowIndex].FindControl("lblFieldDesc")).Text.ToUpper() == "HSC PCM")
                {

                    //if (lblMax != null)
                    //    lblMax.Text = pcmMax.ToString();
                    //if (lblAllot != null)
                    //    lblAllot.Text = pcmAllot.ToString()
                    //if (ViewState["EDITINDEX"].ToString() == "0")
                    //{                            
                    if (lblScoredPercentage != null && pcmMax > 0)
                        lblScoredPercentage.Text = string.Format("{0:0.00}", (pcmAllot / pcmMax) * 100).ToString();
                    if (lblAllot != null)
                        lblAllot.Text = pcmAllot.ToString();
                    if (lblMax != null)
                        lblMax.Text = pcmMax.ToString();

                    if (lblPercentage != null && pcmMax > 0)
                        lblPercentage.Text = string.Format("{0:0.00}", (pcmAllot / pcmMax) * 100).ToString();
                    if (ucAlotted != null)
                        ucAlotted.Text = pcmAllot.ToString();
                    if (ucMax != null)
                        ucMax.Text = pcmMax.ToString();                   
                }
            }
        }

        foreach (GridViewRow r in gvWrittenSection.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow && SessionUtil.CanAccess(this.ViewState, "EDIT"))
            {
                Page.ClientScript.RegisterForEventValidation(gvWrittenSection.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        foreach (GridViewRow r in gvGeneralSection.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow && SessionUtil.CanAccess(this.ViewState, "EDIT"))
            {
                Page.ClientScript.RegisterForEventValidation(gvGeneralSection.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }

        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["mode"] != null)
            {
                cmdPrint.Visible = true;
            }
            if (!IsPostBack)
            {
                ViewState["INTERVIEWID"] = String.Empty;
                ViewState["DATAENATERED"] = String.Empty;
                ViewState["FINALIZEDYN"] = String.Empty;

                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                cmdHiddenConfirm.Attributes.Add("style", "display:none");

                ddl16PFResult.DataSource = PhoenixPreSeaQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 118);
                ddl16PFResult.DataBind();
                ListItem li = new ListItem("--Select--", "DUMMY");
                ddl16PFResult.Items.Insert(0, li);

                if (Request.QueryString["SCORECARDID"] != null)
                    ViewState["SCORECARDID"] = Request.QueryString["SCORECARDID"];
                if (Request.QueryString["INTERVIEWID"] != null)
                    ViewState["INTERVIEWID"] = Request.QueryString["INTERVIEWID"];

                LoadInterviewData(General.GetNullableInteger(ViewState["INTERVIEWID"].ToString()));
            }
            BindScoreCardTemplateData(General.GetNullableInteger(ViewState["INTERVIEWID"].ToString()), "");
            SetOverallTotal();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    #endregion

    #region :   Methods :

    private void LoadInterviewData(int? INTERVIEWID)
    {
        try
        {
            string name = PhoenixSecurityContext.CurrentSecurityContext.FirstName + " " + PhoenixSecurityContext.CurrentSecurityContext.MiddleName + " " + PhoenixSecurityContext.CurrentSecurityContext.LastName;
            DataTable dt = PhoenixPreSeaEntranceExam.ListPreSeaEntranceInterviewSummary(INTERVIEWID);
            if (dt.Rows.Count > 0)
            {
                txtCandidateName.Text = dt.Rows[0]["FLDCANDIDATENAME"].ToString();
                txtDOB.Text = dt.Rows[0]["FLDDATEOFBIRTH"].ToString();
                txtVenueName.Text = dt.Rows[0]["FLDEXAMVENUENAME"].ToString();
                txtRollNo.Text = dt.Rows[0]["FLDENTRANCEROLLNO"].ToString();
                txtInterviewBy1.Text = String.IsNullOrEmpty(dt.Rows[0]["FLDINTERVIEDBY"].ToString()) ? name : dt.Rows[0]["FLDINTERVIEDBY"].ToString();
                txtMarkCompiledBy.Text = String.IsNullOrEmpty(dt.Rows[0]["FLDMARKCOMPILEDBY"].ToString()) ? name : dt.Rows[0]["FLDMARKCOMPILEDBY"].ToString();
                txtInterviewBy2.Text = dt.Rows[0]["FLDINTERVIEDBY2"].ToString();
                ddl16PFResult.SelectedValue = dt.Rows[0]["FLD16PFRESULT"].ToString();
                txt16PFRemarks.Text = dt.Rows[0]["FLD16PFREMARKS"].ToString();
                if (General.GetNullableInteger(dt.Rows[0]["FLDINTERVIEWRESULTS"].ToString()) != null)
                    rdoRecommend.SelectedValue = dt.Rows[0]["FLDINTERVIEWRESULTS"].ToString();

                ViewState["FINALIZEDYN"] = dt.Rows[0]["FLDFINALIZEDYN"].ToString();
            }
            else
            {
                txtInterviewBy1.Text = name;
                txtMarkCompiledBy.Text = name;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindScoreCardTemplateData(int? INTERVIEWID, string gvid)
    {
        try
        {

            if (String.IsNullOrEmpty(gvid) || gvid == "gvAcademicSection")
            {
                DataTable dtAcademic = PhoenixPreSeaEntranceExam.ListPreSeaInterviewScoreCardData(INTERVIEWID, General.GetNullableByte("1"));
                if (dtAcademic.Rows.Count > 0)
                {
                    gvAcademicSection.DataSource = dtAcademic;
                    gvAcademicSection.DataBind();
                }
                else
                    ShowNoRecordsFound(dtAcademic, gvAcademicSection);
            }

            if (String.IsNullOrEmpty(gvid) || gvid == "gvWrittenSection")
            {
                DataTable dtWritten = PhoenixPreSeaEntranceExam.ListPreSeaInterviewScoreCardData(INTERVIEWID, General.GetNullableByte("2"));

                if (dtWritten.Rows.Count > 0)
                {
                    gvWrittenSection.DataSource = dtWritten;
                    gvWrittenSection.DataBind();
                }
                else
                    ShowNoRecordsFound(dtWritten, gvWrittenSection);
            }
            if (String.IsNullOrEmpty(gvid) || gvid == "gvGeneralSection")
            {
                DataTable dtGeneral = PhoenixPreSeaEntranceExam.ListPreSeaInterviewScoreCardData(INTERVIEWID, General.GetNullableByte("3"));
                if (dtGeneral.Rows.Count > 0)
                {
                    gvGeneralSection.DataSource = dtGeneral;
                    gvGeneralSection.DataBind();
                }
                else
                    ShowNoRecordsFound(dtGeneral, gvGeneralSection);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();

            int colcount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = colcount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataTable)_gridView.DataSource).Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }

    }

    private void SetOverallTotal()
    {
        decimal academic = General.GetNullableDecimal(lblAcademicTotal.Text).HasValue ? decimal.Parse(lblAcademicTotal.Text) : decimal.Parse("0");
        decimal written = General.GetNullableDecimal(lblWrittenTotal.Text).HasValue ? decimal.Parse(lblWrittenTotal.Text) : decimal.Parse("0");
        decimal general = General.GetNullableDecimal(lblGeneralTotal.Text).HasValue ? decimal.Parse(lblGeneralTotal.Text) : decimal.Parse("0");
        //txtFinalTotal.Text = (academic + written + general).ToString();
        txtFinalTotal.Text = (academic + written + general).ToString();

        decimal academicmax = General.GetNullableDecimal(lblAcademicMaxTotal.Text).HasValue ? decimal.Parse(lblAcademicMaxTotal.Text) : decimal.Parse("0");
        decimal writtenmax = General.GetNullableDecimal(lblWrittenMaxTotal.Text).HasValue ? decimal.Parse(lblWrittenMaxTotal.Text) : decimal.Parse("0");
        decimal generalmax = General.GetNullableDecimal(lblGeneralMaxTotal.Text).HasValue ? decimal.Parse(lblGeneralMaxTotal.Text) : decimal.Parse("0");
        txtOverallTotal.Text = (academicmax + writtenmax + generalmax).ToString();

        if (General.GetNullableDecimal(txtOverallTotal.Text).Value > 0)
            txtAvg.Text = Math.Round(((General.GetNullableDecimal(txtFinalTotal.Text).Value / General.GetNullableDecimal(txtOverallTotal.Text).Value) * 100), 2).ToString();
    }

    private bool IsValidScoreCardFinalise()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableDecimal(txtFinalTotal.Text).HasValue || General.GetNullableDecimal(txtFinalTotal.Text) == 0)
            ucError.ErrorMessage = "Overall Total should not be zero or empty.";

        if (General.GetNullableString(ddl16PFResult.SelectedValue) == null)
            ucError.ErrorMessage = "16 PF result is required.";

        if (String.IsNullOrEmpty(txtMarkCompiledBy.Text.Trim()))
            ucError.ErrorMessage = "Marks Compiled by is required.";

        if (String.IsNullOrEmpty(txtInterviewBy1.Text.Trim()))
            ucError.ErrorMessage = "Interviewd by 1 is required.";

        return (!ucError.IsError);
    }

    #endregion

    #region : Grid Events   :

    int evaluationCount;
    decimal sectiontotal,SectionMaxTotal;

    protected void gvScore_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
            evaluationCount = 0;
            sectiontotal = 0;
            SectionMaxTotal = 0;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            UserControlMaskNumber ucAllot = (UserControlMaskNumber)e.Row.FindControl("ucAlotted");
            Label lblAllot = (Label)e.Row.FindControl("lblAlloted");
            if (e.Row.RowState == DataControlRowState.Edit && ucAllot != null && !String.IsNullOrEmpty(ucAllot.Text))
            {
                evaluationCount += 1;
                sectiontotal += decimal.Parse(ucAllot.Text.Trim());
            }
            else if (e.Row.RowState != DataControlRowState.Edit && lblAllot != null && !String.IsNullOrEmpty(lblAllot.Text))
            {
                evaluationCount += 1;
                sectiontotal += decimal.Parse(lblAllot.Text.Trim());
            }
            UserControlMaskNumber ucMax = (UserControlMaskNumber)e.Row.FindControl("ucMax");
            Label lblMax = (Label)e.Row.FindControl("lblMax");
            if (e.Row.RowState == DataControlRowState.Edit && ucMax != null && !String.IsNullOrEmpty(ucMax.Text))
            {
                SectionMaxTotal += decimal.Parse(ucMax.Text.Trim());
            }
            else if (e.Row.RowState != DataControlRowState.Edit && lblMax != null && !String.IsNullOrEmpty(lblMax.Text))
            {
                SectionMaxTotal += decimal.Parse(lblMax.Text.Trim());
            }
            ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
            if (edit != null)
            {
                if (ViewState["FINALIZEDYN"].ToString() == "1")
                    edit.Visible = false;
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            switch (_gridView.ID.ToString())
            {
                case "gvAcademicSection":
                    AcademicCount = evaluationCount;
                    lblAcademicTotal.Text = sectiontotal.ToString();
                    lblAcademicMaxTotal.Text = SectionMaxTotal.ToString();
                    break;
                case "gvWrittenSection":
                    WrittenCount = evaluationCount;
                    lblWrittenTotal.Text = sectiontotal.ToString();
                    lblWrittenMaxTotal.Text = SectionMaxTotal.ToString();
                    break;
                case "gvGeneralSection":
                    GeneralCount = evaluationCount;
                    lblGeneralTotal.Text = sectiontotal.ToString();
                    lblGeneralMaxTotal.Text = SectionMaxTotal.ToString();
                    SetOverallTotal();
                    break;
            }

        }

    }

    protected void gvScore_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        BindScoreCardTemplateData(General.GetNullableInteger(ViewState["INTERVIEWID"].ToString()), "");
    }

    protected void gvScore_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        if (_gridView.EditIndex > -1)
            _gridView.UpdateRow(_gridView.EditIndex, false);

        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;

        BindScoreCardTemplateData(General.GetNullableInteger(ViewState["INTERVIEWID"].ToString()), "");
        if (_gridView.ID == "gvAcademicSection")
        //    ((UserControlMaskNumber)_gridView.Rows[e.NewEditIndex].FindControl("ucScredPercent")).FindControl("txtNumber").Focus();
        //else
            ((UserControlMaskNumber)_gridView.Rows[e.NewEditIndex].FindControl("ucMax")).FindControl("txtNumber").Focus();
    }

    protected void gvScore_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindScoreCardTemplateData(General.GetNullableInteger(ViewState["INTERVIEWID"].ToString()), "");
    }

    protected void gvScore_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            if (!String.IsNullOrEmpty(ViewState["INTERVIEWID"].ToString()))
            {
                string scoredpercent = "";
                string maximum = "";
                string allotted = "";

                Label lblIntervieDataEdit = (Label)_gridView.Rows[nCurrentRow].FindControl("lblIntervieDataEdit");
                Label lblField = (Label)_gridView.Rows[nCurrentRow].FindControl("lblFieldIdEdit");
                Label lblPercentage = (Label)_gridView.Rows[nCurrentRow].FindControl("lblPercentage");
                UserControlMaskNumber ucMax = (UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucMax");
                UserControlMaskNumber ucAlotted = (UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucAlotted");

                if (lblPercentage != null) scoredpercent = lblPercentage.Text.Trim();
                if (ucMax != null) maximum = ucMax.Text.Trim();
                if (ucAlotted != null) allotted = ucAlotted.Text.Trim();

                if (lblIntervieDataEdit != null && !String.IsNullOrEmpty(lblIntervieDataEdit.Text))
                {
                    PhoenixPreSeaEntranceExam.UpdatePreSeaEntranceScoreCardData(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                        , int.Parse(lblIntervieDataEdit.Text)
                                                        , General.GetNullableDecimal(scoredpercent)
                                                        , General.GetNullableDecimal(maximum)
                                                        , General.GetNullableDecimal(allotted)
                                                        , "");
                }
                else
                {
                    PhoenixPreSeaEntranceExam.InsertPreSeaEntranceScoreCardData(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , int.Parse(ViewState["INTERVIEWID"].ToString())
                                                                            , int.Parse(ViewState["SCORECARDID"].ToString())
                                                                            , int.Parse(lblField.Text)
                                                                            , General.GetNullableDecimal(scoredpercent)
                                                                            , General.GetNullableDecimal(maximum)
                                                                            , General.GetNullableDecimal(allotted)
                                                                            , "");
                }
            }
            _gridView.EditIndex = -1;
            BindScoreCardTemplateData(General.GetNullableInteger(ViewState["INTERVIEWID"].ToString()), "");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvScore_RowCreated(object sender, GridViewRowEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        if (e.Row.RowType == DataControlRowType.DataRow
    && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
    && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
        SetKeyDownScroll(sender, e);
    }

    #endregion

    #region : Other Events   :

    protected void InterviewScoreCard_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            if (!IsValidScoreCardFinalise())
            {
                ucError.Visible = true;
                return;
            }
            if (ViewState["FINALIZEDYN"].ToString() == "1")
            {
                ucError.ErrorMessage = "Scorecard already finalized ";
                ucError.Visible = true;
                return;
            }
            else
            {
                PhoenixPreSeaEntranceExam.UpdatePreSeaInterviewResults(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                      , int.Parse(ViewState["INTERVIEWID"].ToString())
                                                                      , General.GetNullableByte(rdoRecommend.SelectedValue)
                                                                      , General.GetNullableDecimal(txtFinalTotal.Text)
                                                                      , txtMarkCompiledBy.Text.Trim()
                                                                      , txtInterviewBy1.Text.Trim()
                                                                      , txtInterviewBy2.Text.Trim()
                                                                      , General.GetNullableInteger(ddl16PFResult.SelectedValue)
                                                                      , txt16PFRemarks.Text.Trim());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }

    protected void InterviewScoreCardConfirm_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            if (!IsValidScoreCardFinalise())
            {
                ucError.Visible = true;
                return;
            }
            if (String.IsNullOrEmpty(rdoRecommend.SelectedValue))
            {
                ucError.ErrorMessage = "To confirm select either 'Recommend/Not Recommend' option";
                ucError.Visible = true;
                return;
            }
            if (ViewState["FINALIZEDYN"].ToString() == "1")
            {
                ucError.ErrorMessage = "Scorecard already finalized ";
                ucError.Visible = true;
                return;
            }
            else
            {
                ucFinalize.Visible = true;
                ucFinalize.Text = "You will not be able to make any changes. Are you sure you want to finalize the scorecard?";

                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);               
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void btnFinalize_Click(object sender, EventArgs e)
    {
        UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
        if (ucCM.confirmboxvalue == 1)
        {
            PhoenixPreSeaEntranceExam.UpdatePreSeaInterviewResults(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                               , int.Parse(ViewState["INTERVIEWID"].ToString())
                                                               , General.GetNullableByte(rdoRecommend.SelectedValue)
                                                               , General.GetNullableDecimal(txtFinalTotal.Text)
                                                               , txtMarkCompiledBy.Text.Trim()
                                                               , txtInterviewBy1.Text.Trim()
                                                               , txtInterviewBy2.Text.Trim()
                                                               , General.GetNullableInteger(ddl16PFResult.SelectedValue)
                                                               , txt16PFRemarks.Text.Trim());

            String script = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
    }
    #endregion

    protected void ucMax_TextChangedEvent(object o, EventArgs e)
    {
        UserControlMaskNumber txt = (UserControlMaskNumber)o;
        GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;
        decimal pcmMax = 0;
        //decimal pcmAllot = 0;

        UserControlMaskNumber txtMaxMark = (UserControlMaskNumber)gvRow.FindControl("ucMax");
        UserControlMaskNumber txtAllotMark = (UserControlMaskNumber)gvRow.FindControl("ucAlotted");
        Label lblFieldDesc = (Label)gvRow.FindControl("lblFieldDesc");
        Label lblPercentage = (Label)gvRow.FindControl("lblPercentage");
        if (lblFieldDesc.Text.ToUpper() == "HSC PHYSICS")
        {
            if (txtMaxMark != null && txtAllotMark != null && !string.IsNullOrEmpty(txtMaxMark.Text) && !string.IsNullOrEmpty(txtAllotMark.Text))
            {
                pcmMax += General.GetNullableDecimal(txtMaxMark.Text).Value;
                pcmMax += General.GetNullableDecimal(txtMaxMark.Text).Value;
            }
        }
        else
        {
            if (txtMaxMark != null && txtAllotMark != null && !string.IsNullOrEmpty(txtMaxMark.Text) && !string.IsNullOrEmpty(txtAllotMark.Text))
                lblPercentage.Text = string.Format("{0:0.00}", (Convert.ToDecimal(txtAllotMark.Text.Trim()) / Convert.ToDecimal(txtMaxMark.Text.Trim())) * 100).ToString();
        }
    }
}
