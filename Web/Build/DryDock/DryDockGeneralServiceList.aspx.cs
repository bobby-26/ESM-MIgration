using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.DryDock;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class DryDockGeneralServiceList : PhoenixBasePage
{

	protected void Page_Load(object sender, EventArgs e)
	{

		try
		{
            
			cmdHiddenSubmit.Attributes.Add("style", "display:none");
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbargrid = new PhoenixToolbar();
			toolbargrid.AddFontAwesomeButton("../DryDock/DryDockGeneralServiceList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
			toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvGeneralService')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
			toolbargrid.AddFontAwesomeButton("../DryDock/DryDockGeneralServiceList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            if(PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
			    toolbargrid.AddFontAwesomeButton("../DryDock/DryDockGeneralServiceList.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            toolbargrid.AddFontAwesomeButton("../DryDock/DryDockGeneralServiceList.aspx", "Include All", "<i class=\"fa fa-check-circle\"></i>", "SELECTALL");
            toolbargrid.AddFontAwesomeButton("../DryDock/DryDockGeneralServiceList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                toolbargrid.AddFontAwesomeButton("../DryDock/DryDockGeneralServiceList.aspx", "Distribute All", "<i class=\"fas fa-shipping-fast\"></i>", "DISTRIBUTEALL");
            MenuGeneralService.AccessRights = this.ViewState;
			MenuGeneralService.MenuList = toolbargrid.Show();


			if (!IsPostBack)
			{

				PhoenixToolbar toolbarmain = new PhoenixToolbar();
              
                toolbarmain.AddButton("List", "LIST",ToolBarDirection.Right);
                toolbarmain.AddButton("Details", "DETAIL", ToolBarDirection.Right);


                MenuStandardGeneralService.AccessRights = this.ViewState;
				MenuStandardGeneralService.MenuList = toolbarmain.Show();

                ViewState["PAGENUMBER"] = string.IsNullOrEmpty(Request.QueryString["pno"]) ? 1 : int.Parse(Request.QueryString["pno"]);
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["CURRENTINDEX"] = 1;
				ViewState["JOBID"] = null;
				if (Request.QueryString["GENERALSERVICEID"] != null)
				{
					ViewState["JOBID"] = Request.QueryString["GENERALSERVICEID"].ToString();

				}
                ddlJobType.Items.Clear();

                ddlJobType.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(7);
                ddlJobType.DataTextField = "FLDNAME";
                ddlJobType.DataValueField = "FLDMULTISPECID";
                ddlJobType.DataBind();
                ddlJobType.Items.Add(new RadComboBoxItem("--Select--", ""));
                gvGeneralService.PageSize= int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            MenuStandardGeneralService.SelectedMenuIndex = 0;
		  //  BindData();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void StandardGeneralService_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("DETAIL"))
			{
                if (ViewState["JOBID"] == null || ViewState["JOBID"].ToString() =="")
                    throw new Exception("Select General service to view details.");

                Response.Redirect("../DryDock/DryDockGeneralService.aspx?GENERALSERVICEID=" + ViewState["JOBID"].ToString() + "&pno=" + ViewState["PAGENUMBER"], false);
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

 //   protected void gvGeneralService_RowCommand(object sender, GridViewCommandEventArgs e)
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
 //               Response.Redirect("../DryDock/DryDockGeneralService.aspx?GENERALSERVICEID=" + jobid + "&pno=" + ViewState["PAGENUMBER"], false);
	//		}
	//		catch (Exception ex)
	//		{
	//			ucError.ErrorMessage = ex.Message;
	//			ucError.Visible = true;
	//		}
	//	}

 //       if (e.CommandName.ToString().ToUpper().Equals("SELECTJOB"))
 //       {
 //           string jobid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblJobid")).Text;
 //           int jobregister = 1; // Standard Jobs
 //           int selectedyn = (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkSelectedYN")).Checked == true) ? 1 : 0;
 //           PhoenixDryDockJobGeneral.UpdateDryDockJobGeneralVessel(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
 //               jobregister, General.GetNullableGuid(jobid), selectedyn);
 //       }

 //       if (e.CommandName.ToString().ToUpper() == "DISTRIBUTE")
 //       {
 //           string jobid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblJobid")).Text;

 //           PhoenixDryDockJobGeneral.DryDockJobDistribute(new Guid(jobid), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
 //       }

 //       BindData();

	//}


	protected void ShowExcel()
	{
		try
		{
			int iRowCount = 0;
			int iTotalPageCount = 0;

			string[] alColumns = { "FLDNUMBER", "FLDTITLE", "FLDJOBTYPE", "FLDJOBDESCRIPTION" };
			string[] alCaptions = { "Number", "Title", "Job Type", "Job Description" };
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

			ds = PhoenixDryDockJobGeneral.DryDockJobGeneralSearch
				(1,//generalservice
                 General.GetNullableString(txtJobNumber.Text),
                 General.GetNullableString(txtJobTitle.Text),
                 General.GetNullableInteger(ddlJobType.SelectedValue),
                 PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                 sortexpression, sortdirection,
				 1,
				 iRowCount, ref iRowCount, ref iTotalPageCount);

			General.ShowExcel("General Services", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void GeneralService_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
			{
				ShowExcel();
			}
			if (CommandName.ToUpper().Equals("FIND"))
			{
				ViewState["PAGENUMBER"] = 1;
                gvGeneralService.CurrentPageIndex = 0;
				gvGeneralService.Rebind();
				//SetPageNavigator();
			}
			if (CommandName.ToUpper().Equals("ADD"))
			{
				Response.Redirect("../DryDock/DryDockGeneralServiceAdd.aspx?GENERALSERVICEID=", false);
			}
            if (CommandName.ToUpper().Equals("SELECTALL"))
            {
                int jobregister = 1;

                PhoenixDryDockJobGeneral.UpdateDryDockJobGeneralVessel(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    jobregister, General.GetNullableGuid(""), ToggleStatus());
                gvGeneralService.Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                txtJobNumber.Text = "";
                txtJobTitle.Text = "";
                ddlJobType.ClearSelection();
                gvGeneralService.Rebind();
                //SetPageNavigator();
            }
            if (CommandName.ToUpper().Equals("DISTRIBUTEALL"))
            {
                PhoenixDryDockJobGeneral.DryDockJobDistributeAll(1,PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                gvGeneralService.Rebind();
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

			string[] alColumns = { "FLDNUMBER", "FLDTITLE", "FLDJOBTYPE", "FLDJOBDESCRIPTION" };
			string[] alCaptions = { "Number", "Title", "Job Type", "Job Description" };

			string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
			byte? sortdirection = null;
			if (ViewState["SORTDIRECTION"] != null)
                sortdirection = byte.Parse(ViewState["SORTDIRECTION"].ToString());

           // ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>FreezeGridViewHeader('gvGeneralService', 300, false);</script>", false);
			
            DataSet ds = PhoenixDryDockJobGeneral.DryDockJobGeneralSearch
				(1,//generalservice
                 General.GetNullableString(txtJobNumber.Text),
                 General.GetNullableString(txtJobTitle.Text),
                 General.GetNullableInteger(ddlJobType.SelectedValue),
                 PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                 sortexpression, sortdirection,
				 gvGeneralService.CurrentPageIndex+1,
				  gvGeneralService.PageSize, ref iRowCount, ref iTotalPageCount);

			General.SetPrintOptions("gvGeneralService", "General Services", alCaptions, alColumns, ds);
            gvGeneralService.DataSource = ds;
            //gvGeneralService.DataBind();
            gvGeneralService.VirtualItemCount = iRowCount;
            
			ViewState["ROWCOUNT"] = iRowCount;
			ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
			//SetPageNavigator();
            SetRowSelection();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
    private void SetRowSelection()
    {
        //gvGeneralService.SelectedIndex = -1;
        //for (int i = 0; i < gvGeneralService.Rows.Count; i++)
        //{
        //    if (gvGeneralService.DataKeys[i].Value.ToString().Equals(ViewState["JOBID"]))
        //    {
        //        gvGeneralService.SelectedIndex = i;
        //        break;
        //    }
        //}
    }
	protected void gvGeneralService_RowEditing(object sender, GridViewEditEventArgs de)
	{
		GridView _gridView = (GridView)sender;

	}

	//protected void gvGeneralService_Sorting(object sender, GridViewSortEventArgs se)
	//{
	//	ViewState["SORTEXPRESSION"] = se.SortExpression;

	//	if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
	//		ViewState["SORTDIRECTION"] = 1;
	//	else
	//		ViewState["SORTDIRECTION"] = 0;


	//	BindData();
	//}

	//protected void gvGeneralService_SelectIndexChanging(object sender, GridViewSelectEventArgs se)
	//{
 //       GridView _gridView = (GridView)sender;
 //       _gridView.SelectedIndex = se.NewSelectedIndex;
 //       string jobid = ((Label)_gridView.Rows[_gridView.SelectedIndex].FindControl("lblJobid")).Text;
 //       Response.Redirect("../DryDock/DryDockGeneralService.aspx?GENERALSERVICEID=" + jobid + "&pno=" + ViewState["PAGENUMBER"], false);
	//}

    //protected void gvGeneralService_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    try
    //    {

    //        if (e.Row.RowType == DataControlRowType.Header)
    //        {
    //            if (ViewState["SORTEXPRESSION"] != null)
    //            {
    //                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //                if (img != null)
    //                {
    //                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                        img.Src = Session["images"] + "/arrowUp.png";
    //                    else
    //                        img.Src = Session["images"] + "/arrowDown.png";

    //                    img.Visible = true;
    //                }
    //            }
    //        }

    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
    //            {
    //                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
    //                if (db != null)
    //                {
    //                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
    //                }
    //            }

    //            CheckBox cb = (CheckBox)e.Row.FindControl("chkSelectedYN");
    //            cb.Enabled = SessionUtil.CanAccess(this.ViewState, "SELECTJOB");
    //            Button b = (Button)e.Row.FindControl("cmdSelectedYN");
    //            DataRowView drv = (DataRowView)e.Row.DataItem;
    //            cb.Checked = drv["FLDSELECTEDYN"].ToString().Equals("0") ? false : true;

    //            Label lbl = (Label)e.Row.FindControl("lblJobid");
    //            string jvscript = "";
    //            if (b != null) jvscript = "javascript:checkSelectedYN('" + b.ClientID + "');";
    //            if (b != null) jvscript = "javascript:selectJob('" + lbl.Text + "',this);";
    //            if (cb != null && b != null) cb.Attributes.Add("onclick", jvscript);
    //            if (b != null) b.Attributes.Add("style", "display:none");
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    private int ToggleStatus()
    {
        if (ViewState["SELECTALL"] == null)
        {
            ViewState["SELECTALL"] = 0;
        }

        int status = int.Parse(ViewState["SELECTALL"].ToString());
        if (status == 0)
        {
            ViewState["SELECTALL"] = 1;
            return 1;
        }
        else
        {
            ViewState["SELECTALL"] = 0;
            return 0;
        }
    }

	protected void cmdSearch_Click(object sender, EventArgs e)
	{
		try
		{
            gvGeneralService.Rebind();
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

 //   protected void PagerButtonClick(object sender, CommandEventArgs ce)
 //   {
 //       //gvGeneralService.SelectedIndex = -1;
 //       //gvGeneralService.EditIndex = -1;
 //       //if (ce.CommandName == "prev")
 //       //    ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
 //       //else
 //       //    ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

 //       //BindData();
 //       //SetPageNavigator();
 //   }

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

	//private Boolean IsPreviousEnabled()
	//{
	//	int iCurrentPageNumber;
	//	int iTotalPageCount;

	//	iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
	//	iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

	//	if (iTotalPageCount == 0)
	//		return false;

	//	if (iCurrentPageNumber > 1)
	//		return true;

	//	return false;
	//}

	//private Boolean IsNextEnabled()
	//{
	//	int iCurrentPageNumber;
	//	int iTotalPageCount;

	//	iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
	//	iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

	//	if (iCurrentPageNumber < iTotalPageCount)
	//	{
	//		return true;
	//	}
	//	return false;
	//}

	//private void ShowNoRecordsFound(DataTable dt, GridView gv)
	//{

	//	try
	//	{
	//		dt.Rows.Add(dt.NewRow());
	//		gv.DataSource = dt;
	//		gv.DataBind();

	//		int colcount = gv.Columns.Count;
	//		gv.Rows[0].Cells.Clear();
	//		gv.Rows[0].Cells.Add(new TableCell());
	//		gv.Rows[0].Cells[0].ColumnSpan = colcount;
	//		gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
	//		gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
	//		gv.Rows[0].Cells[0].Font.Bold = true;
	//		gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
	//		gv.Rows[0].Attributes["onclick"] = "";
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

            BindData();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

    protected void gvGeneralService_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvGeneralService_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

           
            if (e.Item is GridDataItem)
            {

                RadImageButton db = (RadImageButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                }
                

                RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkSelectedYN");
                cb.Enabled = SessionUtil.CanAccess(this.ViewState, "SELECTJOB");
                RadButton b = (RadButton)e.Item.FindControl("cmdSelectedYN");
               
                cb.Checked = DataBinder.Eval(e.Item.DataItem, "FLDSELECTEDYN").ToString().Equals("0") ? false : true;

             
                RadLabel lbl = (RadLabel)e.Item.FindControl("lblJobid");
                string jvscript = "";
                //if (b != null) jvscript = "javascript:checkSelectedYN('" + b.ClientID + "');";
                if (b != null) jvscript = "javascript:selectJob('" + lbl.Text + "',this);";
                if (cb != null && b != null) cb.Attributes.Add("onclick", jvscript);
                if (b != null) b.Attributes.Add("style", "display:none");

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

    protected void gvGeneralService_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToString().ToUpper() == "SORT")
            return;

        //GridView _gridView = (GridView)sender;
        //int nCurrentRow = int.Parse(e.CommandArgument.ToString());

        if (e.CommandName.ToString().ToUpper() == "EDIT" || e.CommandName.ToString().ToUpper()=="SELECT")
        {
            try
            {
                string jobid = ((RadLabel)e.Item.FindControl("lblJobid")).Text;
                Response.Redirect("../DryDock/DryDockGeneralService.aspx?GENERALSERVICEID=" + jobid + "&pno=" + ViewState["PAGENUMBER"], false);
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }

        if (e.CommandName.ToString().ToUpper().Equals("SELECTJOB"))
        {
            string jobid = ((RadLabel)e.Item.FindControl("lblJobid")).Text;
            int jobregister = 1; // Standard Jobs
            int selectedyn = (((RadCheckBox)e.Item.FindControl("chkSelectedYN")).Checked == true) ? 1 : 0;
            PhoenixDryDockJobGeneral.UpdateDryDockJobGeneralVessel(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                jobregister, General.GetNullableGuid(jobid), selectedyn);
        }

        if (e.CommandName.ToString().ToUpper() == "DISTRIBUTE")
        {
            string jobid = ((RadLabel)e.Item.FindControl("lblJobid")).Text;

            PhoenixDryDockJobGeneral.DryDockJobDistribute(new Guid(jobid), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        }

       
    }
}
