using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.DryDock;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;


public partial class DryDockJobList :PhoenixBasePage
{

	protected void Page_Load(object sender, EventArgs e)
	{

		try
		{

			cmdHiddenSubmit.Attributes.Add("style", "display:none");
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbargrid = new PhoenixToolbar();
			toolbargrid.AddFontAwesomeButton("../DryDock/DryDockJobList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
			toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvJob')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
			toolbargrid.AddFontAwesomeButton("../DryDock/DryDockJobList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                toolbargrid.AddFontAwesomeButton("../DryDock/DryDockJobList.aspx", "Refresh", "<i class=\"fas fa-sync\"></i>", "REFRESH");
                toolbargrid.AddFontAwesomeButton("../DryDock/DryDockjobList.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            }
            toolbargrid.AddFontAwesomeButton("../DryDock/DryDockjobList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
			MenuJob.AccessRights = this.ViewState;
			MenuJob.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("List", "LIST");
            toolbarmain.AddButton("Details", "DETAIL");
           
            MenuJobGeneral.AccessRights = this.ViewState;
            MenuJobGeneral.MenuList = toolbarmain.Show();

			if (!IsPostBack)
			{				
                ViewState["PAGENUMBER"] = string.IsNullOrEmpty(Request.QueryString["pno"]) ? 1 : int.Parse(Request.QueryString["pno"]);
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["CURRENTINDEX"] = 1;
				ViewState["JOBID"] = null;
				if (Request.QueryString["REPAIRJOBID"] != null)
				{
					ViewState["JOBID"] = Request.QueryString["REPAIRJOBID"].ToString();

				}
				

                ddlJobType.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(7);
                ddlJobType.DataTextField = "FLDNAME";
                ddlJobType.DataValueField = "FLDMULTISPECID";
                ddlJobType.DataBind();
                //ddlJobType.Items.Insert(0, new ListItem("--Select--", ""));
                gvJob.PageSize= int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            MenuJobGeneral.SelectedMenuIndex = 0;
			//BindData();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}

	protected void JobGeneral_TabStripCommand(object sender, EventArgs e)
	{

		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("DETAIL"))
			{
				if (ViewState["JOBID"] == null)
				{
					ShowError();
					return;
				}
                Response.Redirect("../DryDock/DryDockJob.aspx?REPAIRJOBID=" + ViewState["JOBID"].ToString() + "&pno=" + ViewState["PAGENUMBER"], false);
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	private void ShowError()
	{
		ucError.HeaderMessage = "Navigation Error";
		ucError.ErrorMessage = "Please select Repair Job and then click details.";
		ucError.Visible = true;

	}

	//protected void gvJob_RowDataBound(object sender, GridViewCommandEventArgs e)
	//{

	//	if (e.CommandName.ToString().ToUpper() == "SORT")
	//		return;

	//	GridView _gridView = (GridView)sender;
	//	int nCurrentRow = int.Parse(e.CommandArgument.ToString());

	//	if (e.CommandName.ToString().ToUpper() == "EDIT")
	//	{
	//		try
	//		{

	//			string jobid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblJobid")).Text;
 //               Response.Redirect("../DryDock/DryDockJob.aspx?REPAIRJOBID=" + jobid + "&pno=" + ViewState["PAGENUMBER"], false);

	//		}
	//		catch (Exception ex)
	//		{
	//			ucError.ErrorMessage = ex.Message;
	//			ucError.Visible = true;

	//		}
	//	}
 //       else if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
 //       {
 //           string jobid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblJobid")).Text;
 //           string dtkey = ((Label)_gridView.Rows[nCurrentRow].FindControl("lbldtkey")).Text;
 //           Response.Redirect("../DryDock/DryDockJobAttachments.aspx?REPAIRJOBID=" + jobid + "&DTKEY=" + dtkey + "&MOD=" + PhoenixModule.DRYDOCK + "&pno=" + ViewState["PAGENUMBER"], false);
 //       }
	//}


	protected void ShowExcel()
	{
		try
		{

			int iRowCount = 0;
			int iTotalPageCount = 0;

			string[] alColumns = { "FLDNUMBER", "FLDTITLE", "FLDJOBTYPE", "FLDJOBDESCRIPTION","FLDSTATUSNAME" };
			string[] alCaptions = { "Number", "Title", "Job Type", "Job Description","Status" };
			string sortexpression;
			byte? sortdirection = null;

			sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
			if (ViewState["SORTDIRECTION"] != null)
                sortdirection = byte.Parse(ViewState["SORTDIRECTION"].ToString());

			if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
				iRowCount = 10;
			else
				iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

			DataSet ds;

            ds = PhoenixDryDockJob.DryDockJobSearch(General.GetNullableString(txtJobNumber.Text),
                 General.GetNullableString(txtJobTitle.Text),
                 General.GetNullableInteger(ddlJobType.SelectedValue),
                 null,
				 PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                 sortexpression, sortdirection,
				 1,
				 iRowCount, ref iRowCount, ref iTotalPageCount);

			General.ShowExcel("Repair Job", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void Job_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
                        
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
			{
				ViewState["PAGENUMBER"] = 1;
				gvJob.Rebind();
				//SetPageNavigator();
			}
			if (CommandName.ToUpper().Equals("EXCEL"))
			{
				ShowExcel();
			}
			if (CommandName.ToUpper().Equals("REFRESH"))
			{
				PhoenixDryDockJob.UpdateDryDockWorkRequests(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
				gvJob.Rebind();
			}
            if (CommandName.ToUpper().Equals("ADD"))
            {
                Response.Redirect("../DryDock/DryDockJob.aspx",false);
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                txtJobNumber.Text = "";
                txtJobTitle.Text = "";
                ddlJobType.SelectedIndex = -1;
                gvJob.Rebind();
               // SetPageNavigator();
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

			string[] alColumns = { "FLDNUMBER", "FLDTITLE", "FLDJOBTYPE","FLDJOBDESCRIPTION","FLDSTATUSNAME" };
			string[] alCaptions = { "Number", "Title", "Job Type","Job Description","Status" };

			string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
			byte? sortdirection = null;
			if (ViewState["SORTDIRECTION"] != null)
                sortdirection = byte.Parse(ViewState["SORTDIRECTION"].ToString());

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>FreezeGridViewHeader('gvJobGeneral', 300, false);</script>", false);
			
            DataSet ds = PhoenixDryDockJob.DryDockJobSearch(General.GetNullableString(txtJobNumber.Text),
                 General.GetNullableString(txtJobTitle.Text),
                 General.GetNullableInteger(ddlJobType.SelectedValue),
                 null,
                 PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                 sortexpression, sortdirection,
                 gvJob.CurrentPageIndex+1,
                 gvJob.PageSize, ref iRowCount, ref iTotalPageCount);


			General.SetPrintOptions("gvJob", "Repair Job", alCaptions, alColumns, ds);

			if (ds.Tables[0].Rows.Count > 0)
			{
				gvJob.DataSource = ds;
				

				if (ViewState["JOBID"] == null || ViewState["JOBID"].ToString() == "")
				{
					ViewState["JOBID"] = ds.Tables[0].Rows[0][0].ToString();
					ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
					
				}


				//SetRowSelection();

			}
			else
			{
                gvJob.DataSource = ds;
            }

            gvJob.VirtualItemCount = iRowCount;
			ViewState["ROWCOUNT"] = iRowCount;
			ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
			//SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvJob_RowEditing(object sender, GridViewEditEventArgs de)
	{
		GridView _gridView = (GridView)sender;

		//_gridView.EditIndex = de.NewEditIndex;
		//_gridView.SelectedIndex = de.NewEditIndex;

		//BindData();
		//((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtCodeEdit")).Focus();
		//SetPageNavigator();

	}

	protected void gvJob_Sorting(object sender, GridViewSortEventArgs se)
	{
		ViewState["SORTEXPRESSION"] = se.SortExpression;

		if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
			ViewState["SORTDIRECTION"] = 1;
		else
			ViewState["SORTDIRECTION"] = 0;


		BindData();
	}

	//protected void gvJob_SelectIndexChanging(object sender, GridViewSelectEventArgs se)
	//{
	//	gvJob.SelectedIndex = se.NewSelectedIndex;
	//	ViewState["JOBID"] = ((Label)gvJob.Rows[se.NewSelectedIndex].FindControl("lblJobid")).Text;
	//	ViewState["DTKEY"] = ((Label)gvJob.Rows[se.NewSelectedIndex].FindControl("lbldtkey")).Text;
	//	BindData();
	//}

	//protected void gvJob_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
	//{
	//	try
	//	{

	//		if (e.Row.RowType == DataControlRowType.Header)
	//		{
	//			if (ViewState["SORTEXPRESSION"] != null)
	//			{
	//				HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
	//				if (img != null)
	//				{
	//					if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
	//						img.Src = Session["images"] + "/arrowUp.png";
	//					else
	//						img.Src = Session["images"] + "/arrowDown.png";

	//					img.Visible = true;
	//				}
	//			}
	//		}
 //           DataRowView drv = (DataRowView)e.Row.DataItem;
	//		if (e.Row.RowType == DataControlRowType.DataRow)
	//		{
	//			if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
	//			{
	//				ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
	//				if (db != null)
	//				{
	//					db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
	//					if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
	//				}
					
	//			}
 //               Label lblTitle = (Label)e.Row.FindControl("lblTitle");
 //               if (lblTitle != null)
 //               {
 //                   UserControlToolTip ucToolTipTitle = (UserControlToolTip)e.Row.FindControl("ucToolTipTitle");
 //                   lblTitle.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTipTitle.ToolTip + "', 'visible');");
 //                   lblTitle.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTipTitle.ToolTip + "', 'hidden');");
 //               }
 //               Label lblJobDesc = (Label)e.Row.FindControl("lblJobDesc");
 //               if (lblJobDesc != null)
 //               {
 //                   lblJobDesc.Text = (General.SanitizeHtml(drv["FLDJOBDESCRIPTION"].ToString()).Length > 100 ? General.SanitizeHtml(drv["FLDJOBDESCRIPTION"].ToString()).Substring(0, 100) : General.SanitizeHtml(drv["FLDJOBDESCRIPTION"].ToString()));
 //                   UserControlToolTip ucToolTipJobDesc = (UserControlToolTip)e.Row.FindControl("ucToolTipJobDesc");
 //                   ucToolTipJobDesc.Text = General.SanitizeHtml(drv["FLDJOBDESCRIPTION"].ToString());
 //                   lblJobDesc.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTipJobDesc.ToolTip + "', 'visible');");
 //                   lblJobDesc.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTipJobDesc.ToolTip + "', 'hidden');");
 //               }
               
 //               ImageButton attachments = (ImageButton)e.Row.FindControl("cmdAttachments");
 //               if (attachments != null)
 //               {
 //                   if (drv["FLDISATTACHMENT"].ToString() == "0")
 //                       attachments.ImageUrl = Session["images"] + "/no-attachment.png";
 //                   else
 //                       attachments.ImageUrl = Session["images"] + "/attachment.png";
 //               }
	//		}

	//	}
	//	catch (Exception ex)
	//	{
	//		ucError.ErrorMessage = ex.Message;
	//		ucError.Visible = true;
	//	}
	//}

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

	//protected void cmdGo_Click(object sender, EventArgs e)
	//{
	//	try
	//	{
	//		int result;
	//		if (Int32.TryParse(txtnopage.Text, out result))
	//		{
	//			ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

	//			if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
	//				ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

	//			if (0 >= Int32.Parse(txtnopage.Text))
	//				ViewState["PAGENUMBER"] = 1;

	//			if ((int)ViewState["PAGENUMBER"] == 0)
	//				ViewState["PAGENUMBER"] = 1;

	//			txtnopage.Text = ViewState["PAGENUMBER"].ToString();
	//		}
	//		ViewState["JOBID"] = null;
	//		BindData();
	//	}
	//	catch (Exception ex)
	//	{
	//		ucError.ErrorMessage = ex.Message;
	//		ucError.Visible = true;
	//	}
	//}

	//protected void PagerButtonClick(object sender, CommandEventArgs ce)
	//{
	//	try
	//	{
	//		gvJob.SelectedIndex = -1;
	//		gvJob.EditIndex = -1;
	//		if (ce.CommandName == "prev")
	//			ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
	//		else
	//			ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

	//		ViewState["JOBID"] = null;
	//		BindData();
	//	}
	//	catch (Exception ex)
	//	{
	//		ucError.ErrorMessage = ex.Message;
	//		ucError.Visible = true;
	//	}
	//}

	//private void SetPageNavigator()
	//{

	//	try
	//	{
	//		cmdPrevious.Enabled = IsPreviousEnabled();
	//		cmdNext.Enabled = IsNextEnabled();
	//		lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
	//		lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
	//		lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
	//	}
	//	catch (Exception ex)
	//	{
	//		ucError.ErrorMessage = ex.Message;
	//		ucError.Visible = true;
	//	}
	//}

	

	

	
	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		try
		{

			gvJob.Rebind();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	//private void SetRowSelection()
	//{
	//	//gvJob.SelectedIndex = -1;
	//	for (int i = 0; i < gvJob.Items.Count; i++)
	//	{
	//		if (gvJob.DataKeys[i].Value.ToString().Equals(ViewState["JOBID"].ToString()))
	//		{
	//			gvJob.SelectedIndexes = i;
	//			ViewState["DTKEY"] = ((Label)gvJob.Rows[gvJob.SelectedIndex].FindControl("lbldtkey")).Text;

	//		}
	//	}
	//}


    protected void gvJob_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvJob_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        try
        {

           
          //  DataRowView drv = (DataRowView)e.Row.DataItem;
            if (e.Item is GridDataItem)
            {
               
                    ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                    if (db != null)
                    {
                        db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                        if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                    }

               
                Label lblTitle = (Label)e.Item.FindControl("lblTitle");
                if (lblTitle != null)
                {
                    UserControlToolTip ucToolTipTitle = (UserControlToolTip)e.Item.FindControl("ucToolTipTitle");
                    lblTitle.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTipTitle.ToolTip + "', 'visible');");
                    lblTitle.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTipTitle.ToolTip + "', 'hidden');");
                }
                Label lblJobDesc = (Label)e.Item.FindControl("lblJobDesc");
                if (lblJobDesc != null)
                {
                    lblJobDesc.Text = (General.SanitizeHtml(DataBinder.Eval(e.Item.DataItem, "FLDJOBDESCRIPTION").ToString()).Length > 100 ? General.SanitizeHtml(DataBinder.Eval(e.Item.DataItem, "FLDJOBDESCRIPTION").ToString()).Substring(0, 100) : General.SanitizeHtml(DataBinder.Eval(e.Item.DataItem, "FLDJOBDESCRIPTION").ToString()));
                    UserControlToolTip ucToolTipJobDesc = (UserControlToolTip)e.Item.FindControl("ucToolTipJobDesc");
                    ucToolTipJobDesc.Text = General.SanitizeHtml(DataBinder.Eval(e.Item.DataItem, "FLDJOBDESCRIPTION").ToString());
                    lblJobDesc.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTipJobDesc.ToolTip + "', 'visible');");
                    lblJobDesc.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTipJobDesc.ToolTip + "', 'hidden');");
                }

                ImageButton attachments = (ImageButton)e.Item.FindControl("cmdAttachments");
                if (attachments != null)
                {
                    if (DataBinder.Eval(e.Item.DataItem, "FLDISATTACHMENT").ToString() == "0")
                        attachments.ImageUrl = Session["images"] + "/no-attachment.png";
                    else
                        attachments.ImageUrl = Session["images"] + "/attachment.png";
                }
                RadLabel lblJobDescription = (RadLabel)e.Item.FindControl("lblJobDescription");
                UserControlToolTip ucJobDescription = (UserControlToolTip)e.Item.FindControl("ucJobDescription");

                ucJobDescription.Position = ToolTipPosition.TopCenter;
                ucJobDescription.TargetControlId = lblJobDescription.ClientID;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvJob_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToString().ToUpper() == "SORT")
            return;
               

        if (e.CommandName.ToString().ToUpper() == "EDIT")
        {
            try
            {

                string jobid = ((Label)e.Item.FindControl("lblJobid")).Text;
                Response.Redirect("../DryDock/DryDockJob.aspx?REPAIRJOBID=" + jobid + "&pno=" + ViewState["PAGENUMBER"], false);

            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;

            }
        }
        else if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
        {
            string jobid = ((Label)e.Item.FindControl("lblJobid")).Text;
            string dtkey = ((Label)e.Item.FindControl("lbldtkey")).Text;
            Response.Redirect("../DryDock/DryDockJobAttachments.aspx?REPAIRJOBID=" + jobid + "&DTKEY=" + dtkey + "&MOD=" + PhoenixModule.DRYDOCK + "&pno=" + ViewState["PAGENUMBER"], false);
        }
    }
}
