using System;
using System.Web.UI;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Framework;

public partial class PreSeaEntranceExamDetails : PhoenixBasePage
{
    string strCandidatesId, strVenue;    
    PhoenixToolbar toolbarmain;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
        MenuPreSea.SetTrigger(pnlDOA);
        try
        {
            strCandidatesId = Request.QueryString["candidateId"];
            if (!IsPostBack)
            {
                ViewState["CANDIDATEID"] = string.Empty;
                ViewState["CANDIDATENAME"] = String.Empty;
                ViewState["PERSONALMAIL"] = String.Empty;
                ViewState["DATEOFBIRTH"] = String.Empty;

                ViewState["SCORECARDID"] = String.Empty;
                ViewState["EXAMVENUEID"] = String.Empty;
                ViewState["INTERVIEWID"] = String.Empty;
                ViewState["SCORECARDFILLED"] = String.Empty;
                ViewState["DUPLICATEEXISTS"] = String.Empty;
                ViewState["BATCH"] = string.Empty;

                if (Request.QueryString["candidateId"] != null)
                    ViewState["CANDIDATEID"] = Request.QueryString["candidateId"].ToString();
                if (!string.IsNullOrEmpty(Request.QueryString["interviewid"]))
                    ViewState["INTERVIEWID"] = Request.QueryString["interviewid"];

                SetPrimaryCandidatesDetails();
                SetInterviewDeatils();
              

                if (String.IsNullOrEmpty(txtInterviewBy.Text.Trim()))
                    txtInterviewBy.Text = PhoenixSecurityContext.CurrentSecurityContext.UserName;

                if (!string.IsNullOrEmpty(ViewState["SCORECARDID"].ToString()))
                {
                    DataTable dt = PhoenixPreSeaTemplate.ListScoreCardTemplate(General.GetNullableInteger(ViewState["SCORECARDID"].ToString()));
                    if (dt.Rows.Count > 0)
                    {
                        h1default.Visible = false;

                        if (!String.IsNullOrEmpty(ViewState["INTERVIEWID"].ToString()) && !String.IsNullOrEmpty(ViewState["SCORECARDID"].ToString()))
                            ifMoreInfo.Attributes["src"] = dt.Rows[0]["FLDSCORECARDURL"].ToString() + "?INTERVIEWID=" + ViewState["INTERVIEWID"].ToString() + "&SCORECARDID=" + ViewState["SCORECARDID"].ToString();
                        else if (!String.IsNullOrEmpty(ViewState["INTERVIEWID"].ToString()))
                            ifMoreInfo.Attributes["src"] = dt.Rows[0]["FLDSCORECARDURL"].ToString() + "?INTERVIEWID=" + ViewState["INTERVIEWID"].ToString();
                        else
                            ifMoreInfo.Attributes["src"] = dt.Rows[0]["FLDSCORECARDURL"].ToString();
                    }
                    else
                    {
                        ifMoreInfo.Visible = false;
                        h1default.Visible = true;
                    }
                }
                else
                {
                    ifMoreInfo.Visible = false;
                    h1default.Visible = true;
                }
                            
            }
            ScoreCardMenu();
            BindCandidateCertificate();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }    

    private void SetPrimaryCandidatesDetails()
    {
        try
        {
            DataTable dt = PhoenixPreSeaNewApplicantPersonal.PreSeaNewApplicantList(General.GetNullableInteger(ViewState["CANDIDATEID"].ToString()));

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtFirstName.Text = dr["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dr["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dr["FLDLASTNAME"].ToString();
                txtBatch.Text = dr["FLDBATCHNAME"].ToString();
                txtCourse.Text = dr["FLDCOURSENAME"].ToString();
                txtRollNo.Text = dr["FLDENTRANCEROLLNO"].ToString();
                ucExamVenue.SelectedExamVenue = dr["FLDCALLEDVENUE"].ToString();
                ViewState["EXAMVENUEID"] = dr["FLDCALLEDVENUE"].ToString();
                strVenue = dr["FLDCALLEDVENUE"].ToString();

                ViewState["CANDIDATENAME"] = dr["FLDNAME"].ToString(); ;
                ViewState["PERSONALMAIL"] = dr["FLDPERSONALMAIL"].ToString();
                ViewState["DATEOFBIRTH"] = dr["FLDDATEOFBIRTH"].ToString();
                ViewState["BATCH"] = dr["FLDAPPLIEDBATCH"].ToString();
                if (General.GetNullableInteger(ViewState["INTERVIEWID"].ToString()) == null)  // for create interview
                {
                    ViewState["SCORECARDID"] = dr["FLDSCORECARDID"].ToString();
                    ucExamdate.Text = dr["FLDSTARTDATE"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetInterviewDeatils()
    {
        try
        {
            if (!String.IsNullOrEmpty(ViewState["INTERVIEWID"].ToString()))
            {

                DataTable dt = PhoenixPreSeaEntranceExam.EditPreSeaEntranceInterviewSummary(General.GetNullableInteger(ViewState["INTERVIEWID"].ToString()));                

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    string[] chkvalues = new string[] { };
                    string[] chknames = new string[] { };
                    ucExamdate.Text = dr["FLDINTERVIEWDATE"].ToString();
                    txtInterviewBy.Text = dr["FLDINTERVIEDBY"].ToString();
                    //txtRemarks.Text = dr["FLDREMARKS"].ToString();
                    ucExamVenue.SelectedExamVenue = dr["FLDINTERVIEWVENUE"].ToString();
                    //if (General.GetNullableString(dr["FLDCRETIFICATESCHECKED"].ToString()) != null)
                    //    chkvalues = dr["FLDCRETIFICATESCHECKED"].ToString().Split(',');
                    //if (General.GetNullableString(dr["FLDCHECKEDBY"].ToString()) != null)
                    //    chknames = dr["FLDCHECKEDBY"].ToString().Split(',');
                    ViewState["SCORECARDFILLED"] = dr["FLDSCORECARDFILLED"].ToString();
                    ViewState["DUPLICATEEXISTS"] = dr["FLDDUPLICATEYN"].ToString();
                    ViewState["SCORECARDID"] = dr["FLDSCORECARDID"].ToString();

                    //if (chkvalues.Length > 0)
                    //{
                    //    for (int i = 0; i < chkvalues.Length; i++)
                    //    {
                    //        if (General.GetNullableString(chkvalues[i].ToString()) != null)

                    //            switch (i.ToString())
                    //            {
                    //                case "0":
                    //                    chkSSC.SelectedValue = chkvalues[i];
                    //                    txtSSCVerifiedBy.Text = chknames[i];
                    //                    break;
                    //                case "1":
                    //                    chkHSC.SelectedValue = chkvalues[i];
                    //                    txtHSCVerifiedBy.Text = chknames[i];
                    //                    break;
                    //                case "2":
                    //                    chkBSC.SelectedValue = chkvalues[i];
                    //                    txtBSCVerifiedBy.Text = chknames[i];
                    //                    break;
                    //                case "3":
                    //                    chkBE.SelectedValue = chkvalues[i];
                    //                    txtBEVerifiedBy.Text = chknames[i];
                    //                    break;
                    //            }
                    //    }
                    //}

                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetExamPlanDetails(int? venueid)
    {
        if (venueid != null)
        {
            DataTable dt = PhoenixPreSeaBatchPlanner.ListBatchEntranceExamPlan(int.Parse(strCandidatesId), int.Parse(venueid.ToString()), null);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ViewState["EXAMPLANID"] = dr["FLDENTRANCEEXAMPLANID"].ToString();
            }
            else
            {
                // Set Default Contact Details
                DataTable dtVenue = PhoenixPreSeaExamVenue.EditExamVenue(int.Parse(venueid.ToString()));
                if (dtVenue.Rows.Count > 0)
                {
                    DataRow drVenue = dtVenue.Rows[0];
                }
            }
        }
    }

   

    protected void MenuPreSea_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            string Script = "";
            Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
            Script += "fnReloadList();";
            Script += "</script>" + "\n";

             if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidExamPlanDetails())
                {
                    ucError.Visible = true;
                    return;
                }
                //RadWindowManager1.RadConfirm("Your medical request will be saved when you click on Ok and you will not be able to make any changes. Click on Ok to  initiate medical request Or click on Cancel to continue editing?", "confirm", 320, 150, null, "Confirm");
                //string chkvalues = "", checkbynames = "";

                //chkvalues += String.IsNullOrEmpty(chkSSC.SelectedValue) ? "" : chkSSC.SelectedValue + ",";
                //checkbynames += String.IsNullOrEmpty(chkSSC.SelectedValue) ? "" : txtSSCVerifiedBy.Text + ",";

                //chkvalues += String.IsNullOrEmpty(chkHSC.SelectedValue) ? "" : chkHSC.SelectedValue + ",";
                //checkbynames += String.IsNullOrEmpty(chkHSC.SelectedValue) ? "" : txtHSCVerifiedBy.Text + ",";

                //chkvalues += String.IsNullOrEmpty(chkBSC.SelectedValue) ? "" : chkBSC.SelectedValue + ",";
                //checkbynames += String.IsNullOrEmpty(chkBSC.SelectedValue) ? "" : txtBSCVerifiedBy.Text + ",";

                //chkvalues += String.IsNullOrEmpty(chkBE.SelectedValue) ? "" : chkBE.SelectedValue + ",";
                //checkbynames += String.IsNullOrEmpty(chkBE.SelectedValue) ? "" : txtBEVerifiedBy.Text;

                if (String.IsNullOrEmpty(ViewState["INTERVIEWID"].ToString()))
                    {
                        long refInterviewId = 0;
                        string refScorecardFilled = "0";
                        PhoenixPreSeaEntranceExam.InsertPreSeaEntranceInterviewSummary(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                        , General.GetNullableInteger(ucExamVenue.SelectedExamVenue)
                                                                                        , int.Parse(ViewState["BATCH"].ToString())
                                                                                        , General.GetNullableInteger("")
                                                                                        , int.Parse(ViewState["CANDIDATEID"].ToString())
                                                                                        , DateTime.Parse(ucExamdate.Text)
                                                                                        , txtInterviewBy.Text.Trim()
                                                                                        , null
                                                                                        , null
                                                                                        , General.GetNullableInteger(ViewState["SCORECARDID"].ToString())
                                                                                        , General.GetNullableByte("")
                                                                                        , General.GetNullableByte("")
                                                                                        , null
                                                                                        , txtRollNo.Text.Trim()
                                                                                        , ref refInterviewId
                                                                                        , ref refScorecardFilled
                                                                                        );
                        if (refInterviewId != 0)
                        {
                            ViewState["INTERVIEWID"] = refInterviewId.ToString();
                            ViewState["SCORECARDFILLED"] = refScorecardFilled.ToString();
                            if (ifMoreInfo.Attributes["src"]!=null &&  !ifMoreInfo.Attributes["src"].Contains("?INTERVIEWID"))
                                ifMoreInfo.Attributes["src"] = ifMoreInfo.Attributes["src"] + "?INTERVIEWID=" + refInterviewId.ToString();
                        }

                        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                    }
                    else
                    {
                        PhoenixPreSeaEntranceExam.UpdatePreSeaEntranceInterviewSummary(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , int.Parse(ViewState["INTERVIEWID"].ToString())
                                                                    , General.GetNullableInteger(ucExamVenue.SelectedExamVenue)
                                                                    , DateTime.Parse(ucExamdate.Text)
                                                                    , txtInterviewBy.Text.Trim()
                                                                    , null
                                                                    , null
                                                                    , General.GetNullableInteger(ViewState["SCORECARDID"].ToString())
                                                                    , General.GetNullableByte("")
                                                                    , General.GetNullableByte("")
                                                                    , null
                                                                    , txtRollNo.Text.Trim());
                    }
                    ScoreCardMenu();
                    //ucConfirm.Visible = true;
                    //ucConfirm.Text = "A mail will send to candidate to fill additional information in Online Application. Do you want to send";
                
            }
            else if (dce.CommandName.ToUpper().Equals("SAVESCORE"))
            {
            }
            else if (dce.CommandName.ToUpper().Equals("SAVESCORECARD"))
            {
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
            else if (dce.CommandName.ToUpper().Equals("BACK"))
            {
                string type = "";
                if (Request.QueryString["type"] != null)
                {
                    type = Request.QueryString["type"].ToString();
                }
                Response.Redirect("../PreSea/PreSeaEntranceExamInterview.aspx?type=" + type + "&candidateid=" + ViewState["CANDIDATEID"].ToString(), false);
            }
            if (dce.CommandName.ToUpper().Equals("DELETE"))
            {
                ucConfirmDelete.Visible = true;
                ucConfirmDelete.Text = "Are you sure you want to delete this scorecard";
            }
            else
            {
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }    

    protected void gvPreSea_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvPreSea_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int nCurrentRow = int.Parse(e.CommandArgument.ToString());
            GridView _gridview = (GridView)sender;
            Label lblcode = (Label)_gridview.Rows[nCurrentRow].FindControl("lblExamVenueId");
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                if (lblcode != null)
                    SetExamPlanDetails(General.GetNullableInteger(lblcode.Text));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidExamPlanDetails()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucExamVenue.SelectedExamVenue) == null)
            ucError.ErrorMessage = "Select a Batch from Batch Planner";
        if (string.IsNullOrEmpty(ucExamdate.Text))
            ucError.ErrorMessage = "Exam date is Required";
        if (string.IsNullOrEmpty(txtRollNo.Text))
            ucError.ErrorMessage = "Entrance Roll No is Required";

        return (!ucError.IsError);

    }

    protected void btnConfirmDelete_Click(object sender, EventArgs e)
    {
    }
    
    protected void btnSendMail_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucSendMail = sender as UserControlConfirmMessage;
            if (ucSendMail.confirmboxvalue == 1)
            {
                SendMailToCandtdate();
            }
            else
                ucStatus.Text = "Interview Details updated Successfully.";
            ScoreCardMenu();
            SetInterviewDeatils();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected string PrepareEmailBodyText()
    {
        StringBuilder sbemailbody = new StringBuilder();

        sbemailbody.Append("Dear " + ViewState["CANDIDATENAME"].ToString() + ",");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Please note ,we are in the process of finalising the results and hence we require some Additional information from your end");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.Append("In view of the above,would request you to kindly log in into your account and update the details under  ' Additional Information '");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Please find ,below Log in details for your easy reference");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("User Name : " + ViewState["PERSONALMAIL"].ToString());
        sbemailbody.AppendLine("Password  : " + ViewState["DATEOFBIRTH"].ToString());
        sbemailbody.AppendLine();
        string url = "\"<" + Session["sitepath"] + "/" + "Presea/PreSeaRegisterdCandidatesLogin.aspx";
        sbemailbody.AppendLine(url + ">\"");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Thanks and Best regards,");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Admissions Team");
        sbemailbody.AppendLine("Samundra Institute of Maritime Studies");
        sbemailbody.AppendLine();

        return sbemailbody.ToString();
    }

    private void SendMailToCandtdate()
    {
        string emailbodytext = "";

        emailbodytext = PrepareEmailBodyText();

        PhoenixMail.SendMail(ViewState["PERSONALMAIL"].ToString()
            , ""
            , null
            , "Online Application Additional Information Filling"
            , emailbodytext
            , false
            , System.Net.Mail.MailPriority.Normal
            , "");

        ucStatus.Visible = true;
        ucStatus.Text = "Interview Details updated and Mail Sent Successfully";

    }
    private void DeleteScoreCard()
    {
        if (!String.IsNullOrEmpty(ViewState["INTERVIEWID"].ToString()))
            PhoenixPreSeaEntranceExam.DeletePreSeaIndividualScoreCard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , int.Parse(ViewState["INTERVIEWID"].ToString()));
    }
    private void ScoreCardMenu()
    {
        toolbarmain = new PhoenixToolbar();

        if (Request.QueryString["type"].ToString() != "0")
        {
            if (!String.IsNullOrEmpty(ViewState["INTERVIEWID"].ToString()))
            {
                if (!String.IsNullOrEmpty(ViewState["INTERVIEWID"].ToString()))
                    toolbarmain.AddImageLink("javascript:var save=document.getElementById('ifMoreInfo').contentDocument.getElementById('cmdHiddenSubmit'); if(save!=null) save.click(); return false;"
                        , "Save Score Card", string.Empty, "SAVESCORE");
                if (!String.IsNullOrEmpty(ViewState["SCORECARDFILLED"].ToString()))
                    toolbarmain.AddImageLink("javascript:var confirm=document.getElementById('ifMoreInfo').contentDocument.getElementById('cmdHiddenConfirm'); if(confirm!=null) confirm.click(); return false;"
                    , "Finalise", string.Empty, "CONFIRM");
            }
        }
        toolbarmain.AddButton("Save", "SAVE");
        toolbarmain.AddButton("Back", "BACK");
        MenuPreSea.AccessRights = this.ViewState;
        MenuPreSea.MenuList = toolbarmain.Show();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
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
            gv.Rows[0].Attributes["onclick"] = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvcertificateverify_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string certificate = null, verificationstatus = null;
        
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridView _gridView = (GridView)sender;

                certificate = ((UserControlPreSeaQualification)_gridView.FooterRow.FindControl("ddlCertificateInsert")).SelectedQualification;
                verificationstatus= ((DropDownList)_gridView.FooterRow.FindControl("ddlverifiedInsert")).SelectedValue;
                if (!IsValidFaculty(certificate, verificationstatus))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPreSeaEntranceExam.InsertPreSeaCandidatesCertificate(General.GetNullableInteger(ViewState["INTERVIEWID"].ToString()).Value
                                                                       , General.GetNullableInteger(ViewState["CANDIDATEID"].ToString()).Value
                                                                       , General.GetNullableInteger(ViewState["BATCH"].ToString()).Value
                                                                       , General.GetNullableInteger(certificate)
                                                                       , General.GetNullableInteger(verificationstatus)
                                                                       , ((TextBox)_gridView.FooterRow.FindControl("txtRemarksInsert")).Text);

                BindCandidateCertificate();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindCandidateCertificate();
        }

    }

    protected void gvcertificateverify_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                }

                UserControlPreSeaQualification ddlCertificateEdit = (UserControlPreSeaQualification)e.Row.FindControl("ddlCertificateEdit");                
                DropDownList ddlverifiedEdit = (DropDownList)e.Row.FindControl("ddlverifiedEdit");

                DataRowView drv = (DataRowView)e.Row.DataItem;
                if (ddlCertificateEdit != null)
                {
                    ddlCertificateEdit.bind();
                    ddlCertificateEdit.SelectedQualification = drv["FLDCERTIFICATE"].ToString();
                } 
                if (ddlverifiedEdit != null) ddlverifiedEdit.SelectedValue = drv["FLDVERIFIED"].ToString();

                ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
                if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

                ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
                if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

                ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
                if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Visible = false;
                }

                UserControlPreSeaQualification ddlQualification = (UserControlPreSeaQualification)e.Row.FindControl("ddlCertificateInsert");
                ddlQualification.bind();
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
      
    }

    protected void gvcertificateverify_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindCandidateCertificate();
    }

    protected void gvcertificateverify_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            BindCandidateCertificate();            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvcertificateverify_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            string certificate = null, verificationstatus = null;
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;


            certificate = ((UserControlPreSeaQualification)_gridView.Rows[nCurrentRow].FindControl("ddlCertificateEdit")).SelectedQualification;
            verificationstatus = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlverifiedEdit")).SelectedValue;
            string candidateCertificateId = _gridView.DataKeys[nCurrentRow].Value.ToString();

            if (!IsValidFaculty(certificate, verificationstatus))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixPreSeaEntranceExam.UpdatePreSeaCandidatesCertificate(General.GetNullableInteger(ViewState["INTERVIEWID"].ToString()).Value
                                                                     , General.GetNullableInteger(ViewState["CANDIDATEID"].ToString()).Value
                                                                     , General.GetNullableInteger(ViewState["BATCH"].ToString()).Value
                                                                     , General.GetNullableInteger(certificate)
                                                                     , General.GetNullableInteger(verificationstatus)
                                                                     , General.GetNullableInteger(candidateCertificateId).Value
                                                                     , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtRemarksEdit")).Text);

            _gridView.EditIndex = -1;
            BindCandidateCertificate();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindCandidateCertificate()
    {
        DataTable dt= PhoenixPreSeaEntranceExam.ListPreSeaCandidatesCertificate(General.GetNullableInteger(ViewState["INTERVIEWID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            gvcertificateverify.DataSource = dt;
            gvcertificateverify.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvcertificateverify);
        }
    }

    private bool IsValidFaculty(string certificate, string verificationstatus)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(certificate)==null)
            ucError.ErrorMessage = "Certificate is required";
        if (string.IsNullOrEmpty(verificationstatus))
            ucError.ErrorMessage = "Verification status is required";
                
        return (!ucError.IsError);
    }


    protected void gvcertificateverify_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentrow = de.RowIndex;
            string candidatecertificateid = _gridView.DataKeys[nCurrentrow].Value.ToString();
            PhoenixPreSeaEntranceExam.DeletePreSeaCandidatesCertificate(General.GetNullableInteger(candidatecertificateid).Value);
            _gridView.EditIndex = -1;
            BindCandidateCertificate();           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
