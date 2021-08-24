using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;

public partial class InspectionDirectNonConformityCorrectiveAction : PhoenixBasePage
{
	protected override void Render(HtmlTextWriter writer)
	{
		foreach (GridViewRow r in gvCorrectiveAction.Rows)
		{
			if (r.RowType == DataControlRowType.DataRow)
			{
				Page.ClientScript.RegisterForEventValidation(gvCorrectiveAction.UniqueID, "Edit$" + r.RowIndex.ToString());
			}
		}
		foreach (GridViewRow r in gvPreventiveAction.Rows)
		{
			if (r.RowType == DataControlRowType.DataRow)
			{
				Page.ClientScript.RegisterForEventValidation(gvPreventiveAction.UniqueID, "Edit$" + r.RowIndex.ToString());
			}
		}
		foreach (GridViewRow r in gvFollowUpRemarks.Rows)
		{
			if (r.RowType == DataControlRowType.DataRow)
			{
				Page.ClientScript.RegisterForEventValidation(gvFollowUpRemarks.UniqueID, "Edit$" + r.RowIndex.ToString());
			}
		}
		imgShowSupt.Attributes.Add("onclick", "return showPickList('spnPickListSupt', 'codehelp1', '', '../Common/CommonPickListUser.aspx?departmentlist=7,8&mod="
			+ PhoenixModule.QUALITY + "', true);");
        gvCorrectiveAction.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
        gvPreventiveAction.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
        gvFollowUpRemarks.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
		base.Render(writer);
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		SessionUtil.PageAccessRights(this.ViewState);
		txtSupt.Attributes.Add("style", "visibility:hidden");
		txtSuptEmailHidden.Attributes.Add("style", "visibility:hidden");
		if (!IsPostBack)
		{
			ViewState["REVIEWDNC"] = null;
			ucConfirmComplete.Visible = false;
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar = new PhoenixToolbar();
			toolbar.AddButton("Verify", "VERIFY");
			if (Filter.CurrentSelectedIncidentMenu == null)		
				MenuCARGeneral.MenuList = toolbar.Show();
			
			if(Request.QueryString["REVIEWDNC"]!=null)			
				ViewState["REVIEWDNC"] = Request.QueryString["REVIEWDNC"];

            if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToString() != "" && Request.QueryString["callfrom"].ToString() == "directobservation")
                ViewState["callfrom"] = Request.QueryString["callfrom"].ToString();

			BindVerification();
			
		}

        BindCorrectiveAction();
		BindPreventiveAction();
		BindFollowUp();
	}

	private void BindVerification()
	{
        DataSet ds = new DataSet();

        if (ViewState["REVIEWDNC"] != null && ViewState["REVIEWDNC"].ToString() != "")
        {
            if (ViewState["callfrom"] != null && ViewState["callfrom"].ToString() != "" && ViewState["callfrom"].ToString() == "directobservation")
                ds = PhoenixInspectionAuditDirectNonConformity.EditDirectObservation(new Guid(ViewState["REVIEWDNC"].ToString()));
            else                
                ds = PhoenixInspectionAuditDirectNonConformity.EditAuditDirectNonConformity(new Guid(ViewState["REVIEWDNC"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtVerificationDate.Text = dr["FLDCLOSEOUTVERIFIEDDATE"].ToString();
                txtSupt.Text = dr["FLDCLOSEOUTVERIFIEDBY"].ToString();
                txtSuptName.Text = dr["FLDCLOSEOUTVERIFIEDBYNAME"].ToString();
                txtSuptDesignation.Text = dr["FLDCLOSEOUTVERIFIEDBYDESIGNATION"].ToString();
                ViewState["REVIEWDNC"] = dr["FLDREVIEWNONCONFORMITYID"].ToString();
                ucVerficationLevel.SelectedHard = dr["FLDVERIFICATIONLEVEL"].ToString();
            }
        }
	}
	protected void MenuCARGeneral_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("VERIFY"))
        {
            if (!IsValidClose())
            {
                ucError.Visible = true;
                return;
            }

            ucConfirmComplete.Visible = true;
            if (ViewState["callfrom"] != null && ViewState["callfrom"].ToString() != "" && ViewState["callfrom"].ToString() == "directobservation")
                ucConfirmComplete.Text = "Do you want to update the 'Verification Details' for this Direct Observation?";
            else
                ucConfirmComplete.Text = "Do you want to update the 'Verification Details' for this Direct NC?";
        }
	}

	protected void btnComplete_Click(object sender, EventArgs e)
	{
		try
		{
			UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

			if (ucCM.confirmboxvalue == 1)
			{
                if (ViewState["callfrom"] != null && ViewState["callfrom"].ToString() != "" && ViewState["callfrom"].ToString() == "directobservation")
                {
                    PhoenixInspectionAuditDirectNonConformity.UpdateReviewDirectObservationActionVerification(new Guid(ViewState["REVIEWDNC"].ToString()),
                                                                                    General.GetNullableDateTime(txtVerificationDate.Text),
                                                                                    General.GetNullableInteger(txtSupt.Text),
                                                                                    General.GetNullableInteger(ucVerficationLevel.SelectedHard));
                    ucStatus.Text = "Direct Observation has verified";	
                }
                else
                {
                    PhoenixInspectionAuditDirectNonConformity.UpdateReviewDirectNCActionVerification(new Guid(ViewState["REVIEWDNC"].ToString()),
                                                                                    General.GetNullableDateTime(txtVerificationDate.Text),
                                                                                    General.GetNullableInteger(txtSupt.Text),
                                                                                    General.GetNullableInteger(ucVerficationLevel.SelectedHard));
                    ucStatus.Text = "Direct NC has verified";	
                }
                							
                //String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
			return;
		}
	}

	private bool IsValidClose()
	{
		ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucVerficationLevel.SelectedHard) == null)
            ucError.ErrorMessage = "Verification level is required.";

		if (string.IsNullOrEmpty(txtSuptName.Text))
			ucError.ErrorMessage = "'Verification by Superintendent' is required.";

		if (string.IsNullOrEmpty(txtVerificationDate.Text))
			ucError.ErrorMessage = "'Verification Date' is required.";

		if (!string.IsNullOrEmpty(txtVerificationDate.Text))
		{
			if (General.GetNullableDateTime(txtVerificationDate.Text) != null && General.GetNullableDateTime(txtVerificationDate.Text) > DateTime.Today)
				ucError.ErrorMessage = "Verification Date can not be the future date.";
		}

		return (!ucError.IsError);
	}

	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		BindCorrectiveAction();
		BindPreventiveAction();
		BindFollowUp();
	}

	private void BindCorrectiveAction()
	{
		try
		{
			DataSet ds = PhoenixInspectionNonConformity.ListCorrectiveAction(
				General.GetNullableGuid(ViewState["REVIEWDNC"].ToString()));

			if (ds.Tables[0].Rows.Count > 0)
			{
				gvCorrectiveAction.DataSource = ds;
				gvCorrectiveAction.DataBind();               
			}
			else
			{
				DataTable dt = ds.Tables[0];
				ShowNoRecordsFound(dt, gvCorrectiveAction);
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	private void BindPreventiveAction()
	{
		try
		{
			DataSet ds = PhoenixInspectionNonConformity.ListPreventiveAction(
				General.GetNullableGuid(ViewState["REVIEWDNC"].ToString()));

			if (ds.Tables[0].Rows.Count > 0)
			{
				gvPreventiveAction.DataSource = ds;
				gvPreventiveAction.DataBind();                
			}
			else
			{
				DataTable dt = ds.Tables[0];
				ShowNoRecordsFound(dt, gvPreventiveAction);
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	private void BindFollowUp()
	{
		try
		{
			DataSet ds = PhoenixInspectionNonConformity.ListFollowUpRemarks(
				General.GetNullableGuid(ViewState["REVIEWDNC"].ToString()));

			if (ds.Tables[0].Rows.Count > 0)
			{
				gvFollowUpRemarks.DataSource = ds;
				gvFollowUpRemarks.DataBind();                
			}
			else
			{
				DataTable dt = ds.Tables[0];
				ShowNoRecordsFound(dt, gvFollowUpRemarks);
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	public StateBag ReturnViewState()
	{
		return ViewState;
	}

	private void ShowNoRecordsFound(DataTable dt, GridView gv)
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

	private bool IsValidCorrectiveAction(string action, bool extrequired, string targetdate, string completiondate, string extreason)
	{
		ucError.HeaderMessage = "Please provide the following required information";

		if (string.IsNullOrEmpty(action))
			ucError.ErrorMessage = "Corrective Action is required.";

		if (General.GetNullableDateTime(targetdate) == null)
			ucError.ErrorMessage = "Target Date is required.";

		if (extrequired)
		{
			if (string.IsNullOrEmpty(extreason))
                ucError.ErrorMessage = "Reschedule Reason is required.";

            else if (General.GetNullableString(extreason) == General.GetNullableString(ViewState["OLDEXTENSIONREASON"].ToString()))
                ucError.ErrorMessage = "You have not modified the Reschedule Reason.";

			if (General.GetNullableDateTime(targetdate) == General.GetNullableDateTime(ViewState["OLDTARGETDATE"].ToString()))
				ucError.ErrorMessage = "You have not modified the 'Target Date'.";
		}

		if (General.GetNullableDateTime(completiondate) != null && General.GetNullableDateTime(completiondate) > DateTime.Today)
			ucError.ErrorMessage = "Completion Date can not be the future date.";

		return (!ucError.IsError);
	}

	private bool IsValidCorrectiveActionAddition(string action, string targetdate)
	{
		ucError.HeaderMessage = "Please provide the following required information";

		if (string.IsNullOrEmpty(action))
			ucError.ErrorMessage = "Corrective Action is required.";

        if (string.IsNullOrEmpty(targetdate))
            ucError.ErrorMessage = "Target Date is required.";

        else if (General.GetNullableDateTime(targetdate) == null)
            ucError.ErrorMessage = "Please enter a valid Target Date.";

		return (!ucError.IsError);
	}

    private bool IsValidPreventiveAction(string action, bool extrequired, string targetdate, string completiondate, string extreason)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(action))
            ucError.ErrorMessage = "Preventive Action is required.";

        if (General.GetNullableDateTime(targetdate) == null)
            ucError.ErrorMessage = "Target Date is required.";

        if (extrequired)
        {
            if (string.IsNullOrEmpty(extreason))
                ucError.ErrorMessage = "Reschedule Reason is required.";

            else if (General.GetNullableString(extreason) == General.GetNullableString(ViewState["OLDPAEXTENSIONREASON"].ToString()))
                ucError.ErrorMessage = "You have not modified the Reschedule Reason.";

            if (General.GetNullableDateTime(targetdate) == General.GetNullableDateTime(ViewState["OLDPATARGETDATE"].ToString()))
                ucError.ErrorMessage = "You have not modified the 'Target Date'.";
        }

        if (General.GetNullableDateTime(completiondate) != null && General.GetNullableDateTime(completiondate) > DateTime.Today)
            ucError.ErrorMessage = "Completion Date can not be the future date.";

        return (!ucError.IsError);
    }

    private bool IsValidPreventiveActionAddition(string action, string targetdate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(action))
            ucError.ErrorMessage = "Preventive Action is required.";

        if (string.IsNullOrEmpty(targetdate))
            ucError.ErrorMessage = "Target Date is required.";

        else if (General.GetNullableDateTime(targetdate) == null)
            ucError.ErrorMessage = "Please enter a valid Target Date.";

        return (!ucError.IsError);
    }

	private bool IsValidFollowupremarks(string action)
	{
		ucError.HeaderMessage = "Please provide the following required information";

		if (string.IsNullOrEmpty(action))
			ucError.ErrorMessage = "Follow Up Remarks is required.";

		return (!ucError.IsError);
	}

	protected void gvCorrectiveAction_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		BindCorrectiveAction();
	}

	protected void gvCorrectiveAction_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (nCurrentRow != -1)
            {
                CheckBox c = (CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkExtensionRequiredEdit");
                UserControlDate targetdate = (UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucTargetDateEdit");
                TextBox extreason = (TextBox)_gridView.Rows[nCurrentRow].FindControl("txtExtensionReasonEdit");

                if (c != null && c.Checked == false)
                {
                    if (targetdate != null)
                    {
                        targetdate.Enabled = false;
                        targetdate.CssClass = "input";
                        if (ViewState["OLDTARGETDATE"] != null)
                            targetdate.Text = ViewState["OLDTARGETDATE"].ToString();
                    }
                    if (extreason != null)
                    {
                        extreason.Enabled = false;
                        extreason.CssClass = "input";
                        if (ViewState["OLDEXTENSIONREASON"] != null)
                            extreason.Text = ViewState["OLDEXTENSIONREASON"].ToString();
                    }
                }
            }

			if (e.CommandName.ToUpper().Equals("ADD"))
			{
                if (ViewState["REVIEWDNC"] == null || string.IsNullOrEmpty(ViewState["REVIEWDNC"].ToString()))
                {
                    ucError.Text = "'Non-Conformity'' details and the comments should be recorded first";
                    ucError.Visible = true;
                    return;
                }
				if (!IsValidCorrectiveActionAddition(((TextBox)_gridView.FooterRow.FindControl("txtCorrectActAdd")).Text,
                                                       ((UserControlDate)_gridView.FooterRow.FindControl("ucTargetDateAdd")).Text))
				{
					ucError.Visible = true;
					return;
				}
                int vesselid = 0;
                DataSet ds = PhoenixInspectionAuditDirectNonConformity.EditAuditDirectNonConformity(new Guid(ViewState["REVIEWDNC"].ToString()));
                if (ds.Tables[0].Rows.Count > 0)
                    vesselid = int.Parse(ds.Tables[0].Rows[0]["FLDVESSELID"].ToString());

                PhoenixInspectionAuditDirectNonConformity.InsertReviewDirectCorrectiveAction(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(ViewState["REVIEWDNC"].ToString())
                    , ((TextBox)_gridView.FooterRow.FindControl("txtCorrectActAdd")).Text
                    , DateTime.Parse(((UserControlDate)_gridView.FooterRow.FindControl("ucTargetDateAdd")).Text)
                    , vesselid
                    , null
                    , null);				

				BindCorrectiveAction();
				((TextBox)_gridView.FooterRow.FindControl("txtCorrectActAdd")).Focus();
			}
			else if (e.CommandName.ToUpper().Equals("DELETE"))
			{
				PhoenixInspectionNonConformity.DeleteCorrectiveAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode
					, new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblInsCorrectActid")).Text));
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvCorrectiveAction_RowDeleting(object sender, GridViewDeleteEventArgs de)
	{
		BindCorrectiveAction();
	}

	protected void gvCorrectiveAction_RowEditing(object sender, GridViewEditEventArgs de)
	{
		try
		{
			GridView _gridView = (GridView)sender;

			_gridView.EditIndex = de.NewEditIndex;
			_gridView.SelectedIndex = de.NewEditIndex;

			BindCorrectiveAction();			
            ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtCorrectActEdit")).Focus();

            UserControlDate ucTargetDateEdit = (UserControlDate)_gridView.Rows[de.NewEditIndex].FindControl("ucTargetDateEdit");
            TextBox txtExtensionReasonEdit = (TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtExtensionReasonEdit");
            if (ucTargetDateEdit != null)
                ViewState["OLDTARGETDATE"] = ucTargetDateEdit.Text;
            if (txtExtensionReasonEdit != null)
                ViewState["OLDEXTENSIONREASON"] = txtExtensionReasonEdit.Text;
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvCorrectiveAction_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = e.RowIndex;
			CheckBox chkextrequired = (CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkExtensionRequiredEdit");

			if (!IsValidCorrectiveAction(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCorrectActEdit")).Text
								, chkextrequired.Checked
								, ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucTargetDateEdit")).Text
								, ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucCompletionDateEdit")).Text
								, ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtExtensionReasonEdit")).Text))
			{
				ucError.Visible = true;
				return;
			}
            PhoenixInspectionAuditDirectNonConformity.UpdateReviewDirectCorrectiveAction(
			    PhoenixSecurityContext.CurrentSecurityContext.UserCode
				, new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblInsCorrectActidEdit")).Text)
				, ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCorrectActEdit")).Text
				, General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucTargetDateEdit")).Text)
				, General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucCompletionDateEdit")).Text)
				, ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtExtensionReasonEdit")).Text
                , null
                , null);

			if (chkextrequired != null && chkextrequired.Checked == true)
			{
                int vesselid = 0;
                DataSet ds = PhoenixInspectionAuditDirectNonConformity.EditAuditDirectNonConformity(new Guid(ViewState["REVIEWDNC"].ToString()));
                if (ds.Tables[0].Rows.Count > 0)
                    vesselid = int.Parse(ds.Tables[0].Rows[0]["FLDVESSELID"].ToString());

				PhoenixInspectionSchedule.ReScheduleHistoryInsert(
                                                                  PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblInsCorrectActidEdit")).Text)
																, ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtExtensionReasonEdit")).Text
																, General.GetNullableDateTime(ViewState["OLDTARGETDATE"].ToString())
																, General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucTargetDateEdit")).Text)
                                                                , vesselid
																);
			}
            gvCorrectiveAction.EditIndex = -1;
			BindCorrectiveAction();            
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvCorrectiveAction_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
	{
		gvCorrectiveAction.SelectedIndex = e.NewSelectedIndex;
	}

	protected void gvCorrectiveAction_RowCreated(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow
			&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
			&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
		{
			e.Row.TabIndex = -1;
			e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvCorrectiveAction, "Edit$" + e.Row.RowIndex.ToString(), false);
		}
	}

	protected void gvCorrectiveAction_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
			if (db != null)
			{
				db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");				
				if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
					db.Visible = false;
			}

			ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
			if (eb != null)
			{
				if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
					eb.Visible = false;
			}

			ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
			if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

			ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
			if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

			ImageButton cmdReschedule = (ImageButton)e.Row.FindControl("cmdReschedule");
			Label coractionid = (Label)e.Row.FindControl("lblInsCorrectActid");
			if (cmdReschedule != null)
			{
				cmdReschedule.Visible = SessionUtil.CanAccess(this.ViewState, cmdReschedule.CommandName);
				if (coractionid != null)
					cmdReschedule.Attributes.Add("onclick", "parent.Openpopup('codehelp','','../Inspection/InspectionIncidentCARExtensionReason.aspx?correctiveactionid=" + coractionid.Text + "');return true;");
			}

            //DataRowView dr = (DataRowView)e.Row.DataItem;
            //ViewState["OLDTARGETDATE"] = dr["FLDTARGETDATE"].ToString();
            //ViewState["OLDEXTENSIONREASON"] = dr["FLDEXTENSIONREASON"].ToString();
		}
		if (e.Row.RowType == DataControlRowType.Footer)
		{
			ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
			if (db != null)
            {				
				if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
					db.Visible = false;
			}
		}
	}

	protected void gvPreventiveAction_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		BindPreventiveAction();
	}

	protected void gvPreventiveAction_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (nCurrentRow != -1)
            {
                CheckBox c = (CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkPAExtensionRequiredEdit");
                UserControlDate targetdate = (UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucPATargetDateEdit");
                TextBox extreason = (TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPAExtensionReasonEdit");

                if (c != null && c.Checked == false)
                {
                    if (targetdate != null)
                    {
                        targetdate.Enabled = false;
                        targetdate.CssClass = "input";
                        if (ViewState["OLDPATARGETDATE"] != null)
                            targetdate.Text = ViewState["OLDPATARGETDATE"].ToString();
                    }
                    if (extreason != null)
                    {
                        extreason.Enabled = false;
                        extreason.CssClass = "input";
                        if (ViewState["OLDPAEXTENSIONREASON"] != null)
                            extreason.Text = ViewState["OLDPAEXTENSIONREASON"].ToString();
                    }
                }
            }
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (ViewState["REVIEWDNC"] == null || string.IsNullOrEmpty(ViewState["REVIEWDNC"].ToString()))
                {
                    ucError.Text = "'Non-Conformity'' details and the comments should be recorded first";
                    ucError.Visible = true;
                    return;
                }
                if (!IsValidPreventiveActionAddition(((TextBox)_gridView.FooterRow.FindControl("txtPreventActAdd")).Text,
                                                       ((UserControlDate)_gridView.FooterRow.FindControl("ucPATargetDateAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                
                int vesselid = 0;
                DataSet ds = PhoenixInspectionAuditDirectNonConformity.EditAuditDirectNonConformity(new Guid(ViewState["REVIEWDNC"].ToString()));
                if (ds.Tables[0].Rows.Count > 0)
                    vesselid = int.Parse(ds.Tables[0].Rows[0]["FLDVESSELID"].ToString());

                //PhoenixInspectionAuditDirectNonConformity.InsertReviewDirectPreventiveAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                //    , ViewState["REVIEWDNC"] != null ? General.GetNullableGuid(ViewState["REVIEWDNC"].ToString()) : null
                //    , ((TextBox)_gridView.FooterRow.FindControl("txtPreventActAdd")).Text
                //    , DateTime.Parse(((UserControlDate)_gridView.FooterRow.FindControl("ucPATargetDateAdd")).Text)
                //    , vesselid
                //    , null);

                BindPreventiveAction();
                ((TextBox)_gridView.FooterRow.FindControl("txtPreventActAdd")).Focus();
            }
			
			else if (e.CommandName.ToUpper().Equals("DELETE"))
			{
				PhoenixInspectionNonConformity.DeletePreventiveAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode
					, new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblInsPreventActid")).Text));
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvPreventiveAction_RowDeleting(object sender, GridViewDeleteEventArgs de)
	{
		BindPreventiveAction();
	}

	protected void gvPreventiveAction_RowEditing(object sender, GridViewEditEventArgs de)
	{
		try
		{
			GridView _gridView = (GridView)sender;

			_gridView.EditIndex = de.NewEditIndex;
			_gridView.SelectedIndex = de.NewEditIndex;

			BindPreventiveAction();
			((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtPreventActEdit")).Focus();

            UserControlDate ucPATargetDateEdit = (UserControlDate)_gridView.Rows[de.NewEditIndex].FindControl("ucPATargetDateEdit");
            TextBox txtPAExtensionReasonEdit = (TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtPAExtensionReasonEdit");
            if (ucPATargetDateEdit != null)
                ViewState["OLDPATARGETDATE"] = ucPATargetDateEdit.Text;
            if (txtPAExtensionReasonEdit != null)
                ViewState["OLDPAEXTENSIONREASON"] = txtPAExtensionReasonEdit.Text;
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvPreventiveAction_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = e.RowIndex;

            CheckBox chkextrequired = (CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkPAExtensionRequiredEdit");

            if (!IsValidPreventiveAction(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPreventActEdit")).Text
                                        , chkextrequired.Checked
                                        , ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucPATargetDateEdit")).Text
                                        , ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucPACompletionDateEdit")).Text
                                        , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPAExtensionReasonEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }

            //PhoenixInspectionAuditDirectNonConformity.UpdateReviewDirectPreventiveAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            //    , new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblInsPreventActidEdit")).Text)
            //    , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPreventActEdit")).Text
            //    , General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucPATargetDateEdit")).Text)
            //    , General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucPACompletionDateEdit")).Text)
            //    , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPAExtensionReasonEdit")).Text
            //    , null);

            if (chkextrequired != null && chkextrequired.Checked == true)
            {
                int vesselid = 0;
                DataSet ds = PhoenixInspectionAuditDirectNonConformity.EditAuditDirectNonConformity(new Guid(ViewState["REVIEWDNC"].ToString()));
                if (ds.Tables[0].Rows.Count > 0)
                    vesselid = int.Parse(ds.Tables[0].Rows[0]["FLDVESSELID"].ToString());

                PhoenixInspectionSchedule.ReScheduleHistoryInsert(
                                                                  PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblInsPreventActidEdit")).Text)
                                                                , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPAExtensionReasonEdit")).Text
                                                                , General.GetNullableDateTime(ViewState["OLDPATARGETDATE"].ToString())
                                                                , General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucPATargetDateEdit")).Text)
                                                                , vesselid
                                                                );
            }			

            gvPreventiveAction.EditIndex = -1;
			BindPreventiveAction();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvPreventiveAction_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
	{
		gvPreventiveAction.SelectedIndex = e.NewSelectedIndex;
	}

	protected void gvPreventiveAction_RowCreated(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow
			&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
			&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
		{
			e.Row.TabIndex = -1;
			e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvPreventiveAction, "Edit$" + e.Row.RowIndex.ToString(), false);
		}
	}

	protected void gvPreventiveAction_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
			if (db != null)
			{
				db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

				if (Filter.CurrentSelectedIncidentMenu == null)
					db.Visible = true;
				else
					db.Visible = false;

				if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
					db.Visible = false;
			}

			ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
			if (eb != null)
			{
				if (Filter.CurrentSelectedIncidentMenu == null)
					eb.Visible = true;
				else
					eb.Visible = false;

				if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
					eb.Visible = false;
			}

			ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
			if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

			ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
			if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            ImageButton cmdReschedule = (ImageButton)e.Row.FindControl("cmdReschedule");
            Label preactionid = (Label)e.Row.FindControl("lblInsPreventActid");
            if (cmdReschedule != null)
            {
                cmdReschedule.Visible = SessionUtil.CanAccess(this.ViewState, cmdReschedule.CommandName);
                if (preactionid != null)
                    cmdReschedule.Attributes.Add("onclick", "parent.Openpopup('codehelp','','../Inspection/InspectionNonConformityExtensionReason.aspx?ncid=" + preactionid.Text + "');return true;");
            }
		}
		if (e.Row.RowType == DataControlRowType.Footer)
		{
			ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
			if (db != null)
			{
				if (Filter.CurrentSelectedIncidentMenu == null)
					db.Visible = true;
				else
					db.Visible = false;

				if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
					db.Visible = false;
			}
		}
	}

	protected void gvFollowUpRemarks_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		BindFollowUp();
	}

	protected void gvFollowUpRemarks_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

			if (e.CommandName.ToUpper().Equals("ADD"))
			{
                if (ViewState["REVIEWDNC"] == null || string.IsNullOrEmpty(ViewState["REVIEWDNC"].ToString()))
                {
                    ucError.Text = "'Non-Conformity'' details and the comments should be recorded first";
                    ucError.Visible = true;
                    return;
                }

                if (!IsValidFollowupremarks(((TextBox)_gridView.FooterRow.FindControl("txtFollowUpAdd")).Text))
				{
					ucError.Visible = true;
					return;
				}
                
                int vesselid = 0;
                DataSet ds = PhoenixInspectionAuditDirectNonConformity.EditAuditDirectNonConformity(new Guid(ViewState["REVIEWDNC"].ToString()));
                if (ds.Tables[0].Rows.Count > 0)
                    vesselid = int.Parse(ds.Tables[0].Rows[0]["FLDVESSELID"].ToString());
                
                PhoenixInspectionAuditDirectNonConformity.InsertReviewDirectFollowUpRemarks(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , ViewState["REVIEWDNC"] != null ? General.GetNullableGuid(ViewState["REVIEWDNC"].ToString()) : null
                    , ((TextBox)_gridView.FooterRow.FindControl("txtFollowUpAdd")).Text
                    , vesselid
                    );

				BindFollowUp();
				((TextBox)_gridView.FooterRow.FindControl("txtFollowUpAdd")).Focus();
			}
			else if (e.CommandName.ToUpper().Equals("DELETE"))
			{
				PhoenixInspectionNonConformity.DeleteFollowUpRemarks(
					PhoenixSecurityContext.CurrentSecurityContext.UserCode
					, new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblInsFollowupid")).Text));
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvFollowUpRemarks_RowDeleting(object sender, GridViewDeleteEventArgs de)
	{
		BindFollowUp();
	}

	protected void gvFollowUpRemarks_RowEditing(object sender, GridViewEditEventArgs de)
	{
		try
		{
			GridView _gridView = (GridView)sender;

			_gridView.EditIndex = de.NewEditIndex;
			_gridView.SelectedIndex = de.NewEditIndex;

			BindFollowUp();
			((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtFollowUpEdit")).Focus();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvFollowUpRemarks_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = e.RowIndex;

			if (!IsValidFollowupremarks(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtFollowUpEdit")).Text))
			{
				ucError.Visible = true;
				return;
			}
			PhoenixInspectionNonConformity.UpdateFollowUpRemarks(
				PhoenixSecurityContext.CurrentSecurityContext.UserCode
				, new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblInsFollowupidEdit")).Text)
				, ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtFollowUpEdit")).Text);

            gvFollowUpRemarks.EditIndex = -1;
			BindFollowUp();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvFollowUpRemarks_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
	{
		gvFollowUpRemarks.SelectedIndex = e.NewSelectedIndex;
	}

	protected void gvFollowUpRemarks_RowCreated(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow
			&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
			&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
		{
			e.Row.TabIndex = -1;
			e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvFollowUpRemarks, "Edit$" + e.Row.RowIndex.ToString(), false);
		}
	}

	protected void gvFollowUpRemarks_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
			if (db != null)
			{
				db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

				if (Filter.CurrentSelectedIncidentMenu == null)
					db.Visible = true;
				else
					db.Visible = false;

				if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
					db.Visible = false;
			}

			ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
			if (eb != null)
			{
				if (Filter.CurrentSelectedIncidentMenu == null)
					eb.Visible = true;
				else
					eb.Visible = false;

				if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
					eb.Visible = false;
			}

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
				if (Filter.CurrentSelectedIncidentMenu == null)
					db.Visible = true;
				else
					db.Visible = false;

				if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
					db.Visible = false;
			}
		}
	}

	protected void chkExtensionRequiredEdit_checkedChanged(object sender, EventArgs e)
	{
		CheckBox chkextrequired = (CheckBox)sender;
		GridViewRow row = (GridViewRow)chkextrequired.Parent.Parent;
		UserControlDate targetdate = (UserControlDate)row.Cells[1].FindControl("ucTargetDateEdit");
		TextBox extreason = (TextBox)row.Cells[4].FindControl("txtExtensionReasonEdit");

		if (chkextrequired.Checked)
		{
			if (targetdate != null)
			{
				targetdate.Enabled = true;
				targetdate.CssClass = "input_mandatory";
			}
			if (extreason != null)
			{                
				extreason.Enabled = true;
				extreason.CssClass = "input_mandatory";
			}
		}
		else
		{
			if (targetdate != null)
			{
				targetdate.Enabled = false;
				targetdate.CssClass = "input";
				if (ViewState["OLDTARGETDATE"] != null)
					targetdate.Text = ViewState["OLDTARGETDATE"].ToString();
			}
			if (extreason != null)
			{
				extreason.Enabled = false;
				extreason.CssClass = "input";
				if (ViewState["OLDEXTENSIONREASON"] != null)
					extreason.Text = ViewState["OLDEXTENSIONREASON"].ToString();
			}
		}
	}

    protected void chkPAExtensionRequiredEdit_checkedChanged(object sender, EventArgs e)
    {
        CheckBox chkextrequired = (CheckBox)sender;
        GridViewRow row = (GridViewRow)chkextrequired.Parent.Parent;
        UserControlDate targetdate = (UserControlDate)row.Cells[1].FindControl("ucPATargetDateEdit");
        TextBox extreason = (TextBox)row.Cells[4].FindControl("txtPAExtensionReasonEdit");

        if (chkextrequired.Checked)
        {
            if (targetdate != null)
            {
                targetdate.Enabled = true;
                targetdate.CssClass = "input_mandatory";
            }
            if (extreason != null)
            {
                extreason.Enabled = true;
                extreason.CssClass = "input_mandatory";
            }
        }
        else
        {
            if (targetdate != null)
            {
                targetdate.Enabled = false;
                targetdate.CssClass = "input";
                if (ViewState["OLDPATARGETDATE"] != null)
                    targetdate.Text = ViewState["OLDPATARGETDATE"].ToString();
            }
            if (extreason != null)
            {
                extreason.Enabled = false;
                extreason.CssClass = "input";
                if (ViewState["OLDPAEXTENSIONREASON"] != null)
                    extreason.Text = ViewState["OLDPAEXTENSIONREASON"].ToString();
            }
        }
    }
}
