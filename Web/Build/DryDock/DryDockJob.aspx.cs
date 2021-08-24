using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.DryDock;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web;
using System.Text;
using Telerik.Web.UI;


public partial class DryDockJob : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);

			cmdHiddenPick.Attributes.Add("style", "visibility:hidden");
			txtComponentId.Attributes.Add("style", "display:none");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("List", "LIST");
            toolbar.AddButton("Details", "DETAIL");
            toolbar.AddButton("Supt Remarks", "SUPTREMARKS");
            toolbar.AddButton("Work Requests", "WORKREQUESTS");
            toolbar.AddButton("Attachment", "ATTACHMENT");

            imgComponent.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx',true);");


            MenuHeader.AccessRights = this.ViewState;
            MenuHeader.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            //toolbar.AddButton("Cancel", "CANCEL");
            toolbar.AddLinkButton("javascript:parent.Openpopup('codehelp1','','../DryDock/DryDockJobDiff.aspx?jobid=" + Request.QueryString["REPAIRJOBID"] + "')", "History", "HISTORY");
            MenuRepairJobSpecification.AccessRights = this.ViewState;
            MenuRepairJobSpecification.MenuList = toolbar.Show();
            MenuHeader.SelectedMenuIndex = 1;

			if (!IsPostBack)
			{
				ViewState["REPAIRJOBID"]  = null;
				if (Request.QueryString["REPAIRJOBID"] != null)
				{
					ViewState["REPAIRJOBID"]  = Request.QueryString["REPAIRJOBID"];
				}
               				
				BindCheckBox();
				BindFields();

				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
			}
			BindData();			
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void DryDockRepairJobLineItem_TabStripCommand(object sender, EventArgs e)
	{
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("EXCEL"))
		{
			ShowExcel();
		}
	}

	private void BindFields()
	{
		try
		{
            if (ViewState["REPAIRJOBID"] != null)
            {
                Guid jobid = new Guid(ViewState["REPAIRJOBID"].ToString());
                DataSet ds = PhoenixDryDockJob.EditDryDockJob(PhoenixSecurityContext.CurrentSecurityContext.VesselID, jobid);
                DataRow dr = ds.Tables[0].Rows[0];

                txtComponentName.Text = dr["FLDCOMPONENTNAME"].ToString();
                txtComponentCode.Text = dr["FLDCOMPONENTNUMBER"].ToString();
                txtComponentId.Text = dr["FLDCOMPONENTID"].ToString();
                txtLocation.Text = dr["FLDLOCATION"].ToString();
                txtNumber.Text = dr["FLDNUMBER"].ToString();
                lblStatus.Text = dr["FLDSTATUSNAME"].ToString();
                txtTitle.Text = dr["FLDTITLE"].ToString();
                txtTechnicalSpec.Text = General.SanitizeHtml(dr["FLDSPECIFICATION"].ToString());
                txtDuration.Text = dr["FLDESTIMATEDHOURS"].ToString();
                if (dr["FLDESTIMATEDHOURS"].ToString() != "")
                {
                    txtDuration.Text = Math.Round(Convert.ToDecimal(txtDuration.Text)).ToString();
                }
                if (dr["FLDJOBTYPE"].ToString() != string.Empty)
                    ddlJobType.SelectedValue = dr["FLDJOBTYPE"].ToString();
                ucStartDate.Text = dr["FLDPLANNEDSTARTDATE"].ToString();
                txtJobDescription.Text = General.SanitizeHtml(dr["FLDJOBDESCRIPTION"].ToString());
                lblTempJobDescription.Text = General.SanitizeHtml(dr["FLDJOBDESCRIPTION"].ToString());

                BindCheckBoxList(cblEnclosed, dr["FLDENCLOSED"].ToString());
                BindCheckBoxList(cblMaterial, dr["FLDMATERIAL"].ToString());
                BindCheckBoxList(cblWorkSurvey, dr["FLDWORKSURVEYBY"].ToString());
                BindCheckBoxList(cblInclude, dr["FLDINCLUDE"].ToString());
                BindCheckBoxList(cblResponsibilty, dr["FLDRESPONSIBILITY"].ToString());
                if (dr["FLDESTIMATEDDAYS"].ToString() != string.Empty)
                    rblEstimatedDays.SelectedValue = dr["FLDESTIMATEDDAYS"].ToString();
                if (dr["FLDCOSTCLASSIFICATION"].ToString() != string.Empty)
                    ddlCostClassification.SelectedValue = dr["FLDCOSTCLASSIFICATION"].ToString();
                ViewState["OPERATIONMODE"] = "EDIT";
                ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
            }
            else
            {
                BindCheckBoxList(cblResponsibilty, "1");
            }
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}



    private string ReadCheckBoxList(RadListBox cbl)
    {
        string list = string.Empty;

        foreach (RadListBoxItem item in cbl.Items)
        {
            if (item.Checked == true)
            {
                list = list + item.Value.ToString() + ",";
            }
        }
        list = list.TrimEnd(',');
        return list;
    }
    private void BindCheckBoxList(RadListBox cbl, string list)
    {
        foreach (RadListBoxItem li in cbl.Items)
        {
            li.Checked = false;
        }
        foreach (string item in list.Split(','))
        {
            if (item.Trim() != "")
            {
                if (cbl.FindItemByValue(item) != null)
                    cbl.FindItemByValue(item).Checked = true;
            }
        }

    }
    protected void BindCheckBox()
	{
		string type = string.Empty;

		cblEnclosed.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(4);
		cblEnclosed.DataTextField = "FLDNAME";
		cblEnclosed.DataValueField = "FLDMULTISPECID";
		cblEnclosed.DataBind();

		cblMaterial.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(1);
		cblMaterial.DataTextField = "FLDNAME";
		cblMaterial.DataValueField = "FLDMULTISPECID";
		cblMaterial.DataBind();

        cblInclude.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(3);
        cblInclude.DataTextField = "FLDNAME";
        cblInclude.DataValueField = "FLDMULTISPECID";
        cblInclude.DataBind();


        cblWorkSurvey.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(2);
        cblWorkSurvey.DataTextField = "FLDNAME";
        cblWorkSurvey.DataValueField = "FLDMULTISPECID";
        cblWorkSurvey.DataBind();      

        cblResponsibilty.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(5);
        cblResponsibilty.DataTextField = "FLDNAME";
        cblResponsibilty.DataValueField = "FLDMULTISPECID";
        cblResponsibilty.DataBind();

        ddlJobType.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(7);
        ddlJobType.DataTextField = "FLDNAME";
        ddlJobType.DataValueField = "FLDMULTISPECID";
        ddlJobType.DataBind();

        ddlCostClassification.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(8);
        ddlCostClassification.DataTextField = "FLDNAME";
        ddlCostClassification.DataValueField = "FLDMULTISPECID";
        ddlCostClassification.DataBind();

        rblEstimatedDays.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(9);
        rblEstimatedDays.DataBindings.DataTextField = "FLDNAME";
        rblEstimatedDays.DataBindings.DataValueField = "FLDMULTISPECID";
        rblEstimatedDays.DataBind();
	}
    
    protected void ucConfirmMessage_ConfirmMessage(object sender, EventArgs e)
    {
        UserControlConfirmMessage uc = (UserControlConfirmMessage)sender;
        if (uc.confirmboxvalue == 1)
        {
            if (ViewState["REPAIRJOBID"] == null)
            {
                ucError.ErrorMessage = "You have not created the Repair Job. You can cancel only an already existing Repair Job";
                ucError.Visible = true;
            }
            else
            {
                PhoenixDryDockJob.UpdateDryDockJobStatus(
                    General.GetNullableGuid(ViewState["REPAIRJOBID"].ToString()),
                    0,
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID); //0 - Cancel.
            }
        }

        ViewState["CANCELYN"] = "NO";
        BindFields();
    }

    protected void RepairJobSpecification_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("CANCEL"))
            {
                if (ViewState["CANCELYN"] == null || ViewState["CANCELYN"].ToString() == "NO")
                {
                    ucConfirmMessage.ErrorMessage = "Are you sure you want to cancel the repair job?";
                    ucConfirmMessage.Visible = true;
                    ViewState["CANCELYN"] = "YES";
                    BindFields();
                    return;
                }
            }

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidRepairSpec())
                {
                    ucError.Visible = true;
                    return;
                }
                string strEnc = ReadCheckBoxList(cblEnclosed);
                string strMat = ReadCheckBoxList(cblMaterial);
                string strWrk = ReadCheckBoxList(cblWorkSurvey);
                string strInc = ReadCheckBoxList(cblInclude);
                string strEst = rblEstimatedDays.SelectedValue;
                string strRes = ReadCheckBoxList(cblResponsibilty);               

                if (ViewState["REPAIRJOBID"] == null)
                {
                    Guid? jobid = Guid.NewGuid();

                    PhoenixDryDockJob.InsertDryDockJob(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableString(txtNumber.Text.Trim()),
                        txtTitle.Text.Trim(),
                        strRes,
                        General.GetNullableDecimal(txtDuration.Text),
                        General.GetNullableDateTime(ucStartDate.Text),
                        General.GetNullableInteger(ddlJobType.SelectedValue),
                        General.GetNullableString(txtJobDescription.Text.Trim()),
                        General.GetNullableString(txtLocation.Text.Trim()),
                        strWrk, strInc, strMat, strEnc,
                        PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                        General.GetNullableInteger(ddlCostClassification.SelectedValue),
                        General.GetNullableGuid(txtComponentId.Text), strEst,
                        ref jobid);

                    ViewState["REPAIRJOBID"] = jobid;
                    BindData();
                    BindFields();
                    ucStatus.Text = "Job added.";
                }
                else
                {

                    PhoenixDryDockJob.UpdateDryDockJob(
                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                        new Guid(ViewState["REPAIRJOBID"].ToString()),
                                                        General.GetNullableString(txtNumber.Text.Trim()),
                                                        txtTitle.Text.Trim(),
                                                        strRes,
                                                        General.GetNullableDecimal(txtDuration.Text),
                                                        General.GetNullableDateTime(ucStartDate.Text),
                                                        General.GetNullableInteger(ddlJobType.SelectedValue),
                                                        General.GetNullableString(txtJobDescription.Text.Trim()),
                                                        General.GetNullableString(txtLocation.Text.Trim()),
                                                        strWrk, strInc, strMat, strEnc,
                                                        PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                        General.GetNullableInteger(ddlCostClassification.SelectedValue), strEst);

                    PhoenixDryDockJob.UpdateDryDockJobComponent(
                            General.GetNullableGuid(ViewState["REPAIRJOBID"].ToString()),
                            General.GetNullableGuid(txtComponentId.Text),
                            0,
                            PhoenixSecurityContext.CurrentSecurityContext.VesselID, null, txtTechnicalSpec.Text);

                    PhoenixCommonDiffUtil.Item[] diffs = PhoenixCommonDiffUtil.DiffText(lblTempJobDescription.Text, txtJobDescription.Text, false, true, false);
                    if (diffs.Length > 0)
                    {
                        PhoenixDryDockJob.InsertDryDockJobDescriptionChange(General.GetNullableGuid(ViewState["REPAIRJOBID"].ToString()).Value
                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                            , PhoenixCommonDiffUtil.DiffReport(diffs, lblTempJobDescription.Text, txtJobDescription.Text)
                        );
                    }
                    ucStatus.Text = "Job Updated.";
                }
                BindFields();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidRepairSpec()
    {

        ucError.HeaderMessage = "Please provide the following required information.";      

        if (txtTitle.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Title cannot be blank.";

        if (!General.GetNullableGuid(txtComponentId.Text).HasValue)
            ucError.ErrorMessage = "Component is required.";

        if (txtJobDescription.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Job Description cannot be blank.";

        if (General.GetNullableInteger(ddlJobType.SelectedValue) == null)
            ucError.ErrorMessage = "Job Type cannot be blank.";

        if (General.GetNullableInteger(ddlCostClassification.SelectedValue) == null)
            ucError.ErrorMessage = "Cost Classification is required.";
        return (!ucError.IsError);
    }
	protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
	{
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("LIST"))
        {
            Response.Redirect("../DryDock/DryDockJobList.aspx?pno=" + Request.QueryString["pno"], false);
        }
        else if (ViewState["REPAIRJOBID"] != null && ViewState["REPAIRJOBID"].ToString() != "")
        {
            if (CommandName.ToUpper().Equals("DETAIL"))
            {
                Response.Redirect("../DryDock/DryDockJob.aspx?REPAIRJOBID=" + ViewState["REPAIRJOBID"].ToString() + "&pno=" + Request.QueryString["pno"], false);
            }
            else if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                Response.Redirect("../DryDock/DryDockJobAttachments.aspx?REPAIRJOBID=" + ViewState["REPAIRJOBID"].ToString() + "&DTKEY=" + ViewState["DTKEY"].ToString() + "&MOD=" + PhoenixModule.DRYDOCK + "&pno=" + Request.QueryString["pno"], false);
            }
            else if (CommandName.ToUpper().Equals("SUPTREMARKS"))
            {
                Response.Redirect("../DryDock/DryDockDiscussion.aspx?REPAIRJOBID=" + ViewState["REPAIRJOBID"].ToString() + "&pno=" + Request.QueryString["pno"] + "&DTKEY=" + ViewState["DTKEY"].ToString(), false);
            }
            else if (CommandName.ToUpper().Equals("WORKREQUESTS"))
            {
                Response.Redirect("../DryDock/DryDockStandardJobWorkRequest.aspx?REPAIRJOBID=" + ViewState["REPAIRJOBID"].ToString() + "&pno=" + Request.QueryString["pno"], false);
            }
        }
        else
        {
            Response.Redirect("../DryDock/DryDockJob.aspx?pno=" + Request.QueryString["pno"], false);

        }


	}

    protected void ShowExcel()
    {
        try
        {

            string[] alColumns = { "FLDDESCRIPTION", "FLDUNITNAME" };
            string[] alCaptions = { "Job detail", "Unit" };

            int? sortdirection = null;

            DataSet ds = PhoenixDryDockJob.ListDryDockJobDetail(
            new Guid(ViewState["REPAIRJOBID"].ToString()),
            PhoenixSecurityContext.CurrentSecurityContext.VesselID);

            General.ShowExcel("Repair Job Line Item", ds.Tables[0], alColumns, alCaptions, sortdirection, null);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

	private void BindData()
	{

        if (ViewState["REPAIRJOBID"] == null)
            return;

        if (ViewState["REPAIRJOBID"] != null)
        {
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../DryDock/DryDockJob.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvRepairJobLineItem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuDryDockRepairJobLineItem.AccessRights = this.ViewState;
            MenuDryDockRepairJobLineItem.MenuList = toolbargrid.Show();
        }


        string[] alColumns = { "FLDDESCRIPTION", "FLDUNITNAME" };
		string[] alCaptions = {"Job Detail", "Unit"};

		DataSet ds = PhoenixDryDockJob.ListDryDockJobDetail(
			new Guid(ViewState["REPAIRJOBID"].ToString()),
			PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvRepairJobLineItem.PageSize = ds.Tables[0].Rows.Count;
        }
		General.SetPrintOptions("gvRepairJobLineItem", "Repair Job Line Item", alCaptions, alColumns, ds);
        gvRepairJobLineItem.DataSource = ds;
      

	}

	protected void cmdSort_Click(object sender, EventArgs e)
	{
		ImageButton ib = (ImageButton)sender;

		ViewState["SORTEXPRESSION"] = ib.CommandName;
		ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

	}

	protected void gvRepairJobLineItem_RowCreated(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow
			&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
			&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
		{
			e.Row.TabIndex = -1;
			e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvRepairJobLineItem, "Select$" + e.Row.RowIndex.ToString(), false);
		}
	}

	//protected void gvRepairJobLineItem_Sorting(object sender, GridViewSortEventArgs se)
	//{
	//	gvRepairJobLineItem.SelectedIndex = -1;
	//	gvRepairJobLineItem.EditIndex = -1;

	//	ViewState["SORTEXPRESSION"] = se.SortExpression;

	//	if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
	//		ViewState["SORTDIRECTION"] = 1;
	//	else
	//		ViewState["SORTDIRECTION"] = 0;

	//	BindData();
	//}

	//protected void gvRepairJobLineItem_RowUpdating(object sender, GridViewUpdateEventArgs e)
	//{
	//	try
	//	{
	//		GridView _gridView = (GridView)sender;
	//		int nCurrentRow = e.RowIndex;
	//		if (!IsValidRepairJobDetail(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDetailEdit")).Text))
	//		{
	//			ucError.Visible = true;
	//			return;
	//		}

	//		PhoenixDryDockJob.UpdateDryDockJobDetail(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
	//			 new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lbljobdetailidEdit")).Text),
	//		     new Guid(ViewState["REPAIRJOBID"].ToString()),
	//			 PhoenixSecurityContext.CurrentSecurityContext.VesselID,
	//			 ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDetailEdit")).Text.Trim(),
	//			General.GetNullableInteger (((UserControlUnit)_gridView.Rows[nCurrentRow].FindControl("ucUnitEdit")).SelectedUnit));



	//		_gridView.EditIndex = -1;
	//		BindData();
			
	//	}
	//	catch (Exception ex)
	//	{
	//		ucError.ErrorMessage = ex.Message;
	//		ucError.Visible = true;
	//	}
	//}
    
	//protected void gvRepairJobLineItem_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
	//{
	//	gvRepairJobLineItem.SelectedIndex = e.NewSelectedIndex;
	//}

	protected void gvRepairJobLineItem_RowEditing(object sender, GridViewEditEventArgs de)
	{
		try
		{
			GridView _gridView = (GridView)sender;

			_gridView.EditIndex = de.NewEditIndex;
			_gridView.SelectedIndex = de.NewEditIndex;

			BindData();
			
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	//protected void gvRepairJobLineItem_RowCommand(object sender, GridViewCommandEventArgs e)
	//{
	//	try
	//	{
	//		if (e.CommandName.ToUpper().Equals("SORT"))
	//			return;

	//		GridView _gridView = (GridView)sender;
	//		int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

	//		if (e.CommandName.ToUpper().Equals("ADD"))
	//		{
	//			if (!IsValidRepairJobDetail(((TextBox)_gridView.FooterRow.FindControl("txtDetailAdd")).Text))
	//			{
	//				ucError.Visible = true;
	//				return;
	//			}
	//			Guid? jobdetailid=null;
	//			jobdetailid=PhoenixDryDockJob.InsertDryDockJobDetail(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
	//			 new Guid(ViewState["REPAIRJOBID"].ToString()),
	//			 PhoenixSecurityContext.CurrentSecurityContext.VesselID,
	//			 ((TextBox)_gridView.FooterRow.FindControl("txtDetailAdd")).Text.Trim(),
	//			General.GetNullableInteger(((UserControlUnit)_gridView.FooterRow.FindControl("ucUnitAdd")).SelectedUnit),
	//			ref jobdetailid);

	//			BindData();

	//		}

 //           if (e.CommandName.ToUpper().Equals("MOVEUP"))
 //           {

 //               PhoenixDryDockJob.MoveDryDockJobDetailUpOrDown(PhoenixSecurityContext.CurrentSecurityContext.UserCode
 //                   , new Guid(ViewState["REPAIRJOBID"].ToString())
 //                   , General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblDTKey")).Text), 1 // UP
 //                   , PhoenixSecurityContext.CurrentSecurityContext.VesselID
 //                   );

 //               BindData();
 //           }

 //           if (e.CommandName.ToUpper().Equals("MOVEDOWN"))
 //           {
 //               PhoenixDryDockJob.MoveDryDockJobDetailUpOrDown(PhoenixSecurityContext.CurrentSecurityContext.UserCode
 //                   , new Guid(ViewState["REPAIRJOBID"].ToString())
 //                   , General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblDTKey")).Text), -1 // DOWN
 //                   , PhoenixSecurityContext.CurrentSecurityContext.VesselID
 //                   );

 //               BindData();
 //           }

			
	//	}
	//	catch (Exception ex)
	//	{
	//		ucError.ErrorMessage = ex.Message;
	//		ucError.Visible = true;
	//	}
	//}

	//protected void gvRepairJobLineItem_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
	//{
	//	if (e.Row.RowType == DataControlRowType.Header)
	//	{
	//		if (ViewState["SORTEXPRESSION"] != null)
	//		{
	//			HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
	//			if (img != null)
	//			{
	//				if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
	//					img.Src = Session["images"] + "/arrowUp.png";
	//				else
	//					img.Src = Session["images"] + "/arrowDown.png";

	//				img.Visible = true;
	//			}
	//		}
	//	}

 //       if (e.Row.RowType == DataControlRowType.Footer)
 //       {
 //           ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
 //           if (db != null)
 //           {
 //               db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
 //           }
 //       }

	//	if (e.Row.RowType == DataControlRowType.DataRow)
	//	{
	//		ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
	//		if (del != null)
	//		{
 //               del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
	//			del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
	//		}

	//		ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
	//		if (edit != null)
	//		{
	//			edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
	//		}

	//		ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
	//		if (save != null)
	//		{
	//			save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);
	//		}

	//		ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
	//		if (cancel != null)
	//		{
	//			cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
	//		}
			
	//		UserControlUnit ucUnit = (UserControlUnit)e.Row.FindControl("ucUnitEdit");
	//		DataRowView drv = (DataRowView)e.Row.DataItem;
	//		if (ucUnit != null) ucUnit.SelectedUnit = drv["FLDUNIT"].ToString();
	//	}
	//}

    //protected void gvRepairJobLineItem_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;

    //        PhoenixDryDockJob.DeleteDryDockJobDetail(new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lbljobdetailid")).Text),
    //         new Guid(ViewState["REPAIRJOBID"].ToString()),
    //         PhoenixSecurityContext.CurrentSecurityContext.VesselID);

    //        _gridView.EditIndex = -1;
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

	protected void gvRepairJobLineItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		BindData();
	}

	protected void cmdSearch_Click(object sender, EventArgs e)
	{
		
		gvRepairJobLineItem.Rebind();
		
	}

	public void cmdHiddenPick_Click(object sender, EventArgs e)
	{
        if (ViewState["REPAIRJOBID"] != null)
        {
            PhoenixDryDockJob.UpdateDryDockJobComponent(
                General.GetNullableGuid(ViewState["REPAIRJOBID"].ToString()),
                General.GetNullableGuid(txtComponentId.Text),
                0,
                PhoenixSecurityContext.CurrentSecurityContext.VesselID, null);
        }
        BindFields();		
	}

	private bool IsValidRepairJobDetail(string jobdetail)
	{

		ucError.HeaderMessage = "Please provide the following required information";


		if (jobdetail.Trim().Equals(""))
			ucError.ErrorMessage = "Job Detail is required.";

		return (!ucError.IsError);
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

    protected void gvRepairJobLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvRepairJobLineItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

           

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidRepairJobDetail(((RadTextBox)e.Item.FindControl("txtDetailAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                Guid? jobdetailid = null;
                jobdetailid = PhoenixDryDockJob.InsertDryDockJobDetail(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                 new Guid(ViewState["REPAIRJOBID"].ToString()),
                 PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                 ((RadTextBox)e.Item.FindControl("txtDetailAdd")).Text.Trim(),
                General.GetNullableInteger(((UserControlUnit)e.Item.FindControl("ucUnitAdd")).SelectedUnit),
                ref jobdetailid);
                BindData();
                gvRepairJobLineItem.Rebind();

            }

            if (e.CommandName.ToUpper().Equals("MOVEUP"))
            {

                PhoenixDryDockJob.MoveDryDockJobDetailUpOrDown(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["REPAIRJOBID"].ToString())
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDTKey")).Text), 1 // UP
                    , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    );

                BindData();
                gvRepairJobLineItem.Rebind();
            }

            if (e.CommandName.ToUpper().Equals("MOVEDOWN"))
            {
                PhoenixDryDockJob.MoveDryDockJobDetailUpOrDown(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["REPAIRJOBID"].ToString())
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDTKey")).Text), -1 // DOWN
                    , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    );

                BindData();
                gvRepairJobLineItem.Rebind();
            }
            if(e.CommandName.ToUpper().Equals("DELETE"))
            {
                
                PhoenixDryDockJob.DeleteDryDockJobDetail(new Guid(((RadLabel)e.Item.FindControl("lbljobdetailid")).Text),
                 new Guid(ViewState["REPAIRJOBID"].ToString()),
                 PhoenixSecurityContext.CurrentSecurityContext.VesselID);

                BindData();
                gvRepairJobLineItem.Rebind();
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRepairJobLineItem_ItemDataBound1(object sender, GridItemEventArgs e)
    {
       

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }

        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            }

            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null)
            {
                edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
            }

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null)
            {
                save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);
            }

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null)
            {
                cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
            }

            UserControlUnit ucUnit = (UserControlUnit)e.Item.FindControl("ucUnitEdit");
          
            if (ucUnit != null) ucUnit.SelectedUnit = DataBinder.Eval(e.Item.DataItem, "FLDUNIT").ToString();
        }
    }

    protected void gvRepairJobLineItem_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = e.RowIndex;
            if (!IsValidRepairJobDetail(((RadTextBox)e.Item.FindControl("txtDetailEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixDryDockJob.UpdateDryDockJobDetail(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                 new Guid(((RadLabel)e.Item.FindControl("lbljobdetailidEdit")).Text),
                 new Guid(ViewState["REPAIRJOBID"].ToString()),
                 PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                 ((RadTextBox)e.Item.FindControl("txtDetailEdit")).Text.Trim(),
                General.GetNullableInteger(((UserControlUnit)e.Item.FindControl("ucUnitEdit")).SelectedUnit));



         
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
