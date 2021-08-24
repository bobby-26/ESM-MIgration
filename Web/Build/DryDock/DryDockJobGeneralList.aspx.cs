using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.DryDock;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class DryDockJobGeneralList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlGenericControl html2 = new HtmlGenericControl();
            html2.InnerHtml = "<span class=\"icon\" ><i class=\"fas fa-user\"></i></span>";
            Img1.Controls.Add(html2);
            //ownerorvessel.Visible = true;

            HtmlGenericControl html3 = new HtmlGenericControl();
            html3.InnerHtml = "<span class=\"icon\" ><i class=\"fab fa-docker\"></i></span>";
            Img2.Controls.Add(html3);


            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            
            toolbargrid.AddFontAwesomeButton("../DryDock/DryDockJobGeneralList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvJobGeneral')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../DryDock/DryDockJobGeneralList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                toolbargrid.AddFontAwesomeButton("../DryDock/DryDockJobGeneralList.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            toolbargrid.AddFontAwesomeButton("../DryDock/DryDockJobGeneralList.aspx", "Include All", "<i class=\"fa fa-check-circle\"></i>", "SELECTALL");
            toolbargrid.AddFontAwesomeButton("../DryDock/DryDockJobGeneralList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                toolbargrid.AddFontAwesomeButton("../DryDock/DryDockJobGeneralList.aspx", "Distribute All", "<i class=\"fas fa-shipping-fast\"></i>", "DISTRIBUTEALL");
            MenuJobGeneral.AccessRights = this.ViewState;
            MenuJobGeneral.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {

                PhoenixToolbar toolbarmain = new PhoenixToolbar();

                toolbarmain.AddButton("List", "LIST", ToolBarDirection.Right);
                toolbarmain.AddButton("Details", "DETAIL", ToolBarDirection.Right);
                
                MenuStandardJobGeneral.AccessRights = this.ViewState;
                MenuStandardJobGeneral.MenuList = toolbarmain.Show();

                ViewState["PAGENUMBER"] = string.IsNullOrEmpty(Request.QueryString["pno"]) ? 1 : int.Parse(Request.QueryString["pno"]);
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["JOBID"] = null;
                if (Request.QueryString["STANDARDJOBID"] != null)
                {
                    ViewState["JOBID"] = Request.QueryString["STANDARDJOBID"].ToString();
                }
                ddlJobType.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(7);
                ddlJobType.DataTextField = "FLDNAME";
                ddlJobType.DataValueField = "FLDMULTISPECID";
                ddlJobType.DataBind();
                ddlJobType.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
                gvJobGeneral.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                //BindData();
            }
            MenuStandardJobGeneral.SelectedMenuIndex = 0;
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

	protected void StandardJobGeneral_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("DETAIL"))
            {
                if (ViewState["JOBID"] == null || ViewState["JOBID"].ToString() == "")
                {
                    throw new Exception("Select a standard Job to view details.");                    
                }
                
                Response.Redirect("../DryDock/DryDockJobGeneral.aspx?STANDARDJOBID=" + ViewState["JOBID"].ToString() + "&pno=" + ViewState["PAGENUMBER"], false);
                
            }
          
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
            MenuStandardJobGeneral.SelectedMenuIndex = 0;
		}
	}

    //protected void gvJobGeneral_RowCommand(object sender, GridViewCommandEventArgs e)
    //{

    //    if (e.CommandName.ToString().ToUpper() == "SORT")
    //        return;

    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = int.Parse(e.CommandArgument.ToString());

    //    if (e.CommandName.ToString().ToUpper().Equals("SELECTJOB"))
    //    {
    //        string jobid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblJobid")).Text;
    //        int jobregister = 2; // Standard Jobs
    //        int selectedyn = (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkSelectedYN")).Checked == true) ? 1 : 0;
    //        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
    //        {
    //            PhoenixDryDockJobGeneral.UpdateDryDockJobGeneralVessel(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
    //                jobregister, General.GetNullableGuid(jobid), selectedyn);
    //        }
    //        BindData();
    //    }
    //    if (e.CommandName.ToString().ToUpper() == "VIEW")
    //    {
    //        try
    //        {
    //            string jobid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblJobid")).Text;
    //            Response.Redirect("../DryDock/DryDockJobGeneral.aspx?STANDARDJOBID=" + jobid + "&pno=" + ViewState["PAGENUMBER"], false);
    //        }
    //        catch (Exception ex)
    //        {
    //            ucError.ErrorMessage = ex.Message;
    //            ucError.Visible = true;
    //        }
    //    }
    //    if (e.CommandName.ToString().ToUpper().Equals("ATTACHMENT"))
    //    {
    //        string jobid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblJobid")).Text;
    //        string dtkey = ((Label)_gridView.Rows[nCurrentRow].FindControl("lbldtkey")).Text;
  
    //        Response.Redirect("../DryDock/DryDockJobAttachments.aspx?STANDARDJOBID=" + jobid + "&DTKEY=" + dtkey + "&MOD=" + PhoenixModule.DRYDOCK + "&pno=" + ViewState["PAGENUMBER"], false);

    //    }
    //}

    private bool IsValidMapping(RadListBox cblVesselType)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (GetVesselType(cblVesselType).Equals(""))
        {
            ucError.ErrorMessage = "Select the Vessel Type that needs to be mapped to the Job.";
        }
        return (!ucError.IsError);
    }

	protected void ShowExcel()
	{
		try
		{
			int iRowCount = 0;
			int iTotalPageCount = 0;

			string[] alColumns = { "FLDNUMBER", "FLDTITLE", "FLDJOBTYPE", "FLDJOBDESCRIPTION" };
			string[] alCaptions = { "Job Number", "Title", "Job Type", "Job Description" };
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
				(2,//standardjob
                 General.GetNullableString(txtJobNumber.Text),
                 General.GetNullableString(txtJobTitle.Text),
				 General.GetNullableInteger(ddlJobType.SelectedValue),
                 PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                 sortexpression,
                 sortdirection,
				 1,
				 iRowCount, ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("Standard Job", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
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

            if (CommandName.ToUpper().Equals("EXCEL"))
			{
				ShowExcel();
			}
			if (CommandName.ToUpper().Equals("FIND"))
			{
				ViewState["PAGENUMBER"] = 1;
                gvJobGeneral.Rebind();
				//SetPageNavigator();
			}

            if (CommandName.ToUpper().Equals("ADD"))
			{
				Response.Redirect("../DryDock/DryDockJobGeneralAdd.aspx?STANDARDJOBID=", false);
			}
            if (CommandName.ToUpper().Equals("SELECTALL"))
            {
                int jobregister = 2;
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    PhoenixDryDockJobGeneral.UpdateDryDockJobGeneralVessel(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                        jobregister, General.GetNullableGuid(""), ToggleStatus());
                }                
                gvJobGeneral.Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                txtJobNumber.Text = "";
                txtJobTitle.Text = "";
                ddlJobType.ClearSelection(); 
                gvJobGeneral.Rebind();
               // SetPageNavigator();
            }
            if (CommandName.ToUpper().Equals("DISTRIBUTEALL"))
            {
                PhoenixDryDockJobGeneral.DryDockJobDistributeAll(2,PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                gvJobGeneral.Rebind();
            }
        }
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

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


           // ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>FreezeGridViewHeader('gvJobGeneral', 300, false);</script>", false);
            DataSet ds = PhoenixDryDockJobGeneral.DryDockJobGeneralSearch
                (2,//standardjob
                 General.GetNullableString(txtJobNumber.Text),
                 General.GetNullableString(txtJobTitle.Text),
                 General.GetNullableInteger(ddlJobType.SelectedValue),
                 PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                 sortexpression,
                 sortdirection,
                 (int)ViewState["PAGENUMBER"],
                 gvJobGeneral.PageSize, ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvJobGeneral", "Standard Job", alCaptions, alColumns, ds);

            gvJobGeneral.DataSource = ds;
            // gvJobGeneral.DataBind();
            gvJobGeneral.VirtualItemCount = iRowCount;
          

			ViewState["ROWCOUNT"] = iRowCount;
			ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
			//SetPageNavigator();
            //SetRowSelection();
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
            {                
                gvJobGeneral.Columns[6].Visible = false;
            }
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {                
                gvJobGeneral.Columns[5].Visible = false;
            }
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
    protected void gvJobGeneral_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvJobGeneral_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
        {

            _gridView.EditIndex = e.NewEditIndex;
            BindData();
        }
        else
        {
            string jobid = ((Label)_gridView.Rows[e.NewEditIndex].FindControl("lblJobid")).Text;
            Response.Redirect("../DryDock/DryDockJobGeneral.aspx?STANDARDJOBID=" + jobid + "&pno=" + ViewState["PAGENUMBER"], false);
        }
        //CheckBoxList cbl = (CheckBoxList)_gridView.Rows[e.NewEditIndex].FindControl("cblVesselType");
        //cbl.DataSource = PhoenixRegistersVesselType.ListVesselType(0);
        //cbl.DataBind();
    }
    //protected void gvJobGeneral_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = e.RowIndex;
    //    try
    //    {
    //        string jobid = _gridView.DataKeys[nCurrentRow].Value.ToString();
    //        CheckBoxList cblVesselType = ((CheckBoxList)_gridView.Rows[nCurrentRow].FindControl("cblVesselType"));
    //        if (!IsValidMapping(cblVesselType))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }
            
    //        PhoenixDryDockJobGeneral.UpdateDryDockJobGeneralVesselType(General.GetNullableGuid(jobid), GetVesselType(cblVesselType));            
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //    _gridView.EditIndex = -1;
    //    BindData();
    //}
	protected void gvJobGeneral_Sorting(object sender, GridViewSortEventArgs se)
	{
		ViewState["SORTEXPRESSION"] = se.SortExpression;

		if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
			ViewState["SORTDIRECTION"] = 1;
		else
			ViewState["SORTDIRECTION"] = 0;

        BindData();
	}

	protected void gvJobGeneral_SelectIndexChanging(object sender, GridViewSelectEventArgs se)
	{
        GridView _gridview = (GridView)sender;
        _gridview.SelectedIndex = se.NewSelectedIndex;
        ViewState["JOBID"] = ((Label)_gridview.Rows[se.NewSelectedIndex].FindControl("lblJobId")).Text;
	}

 //   protected void gvJobGeneral_OnRowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

 //           if (e.Row.RowType == DataControlRowType.DataRow)
 //           {
 //               DataRowView drv = (DataRowView)e.Row.DataItem;
 //               if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
 //               {
 //                   ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
 //                   if (db != null)
 //                   {
 //                       db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
 //                       db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
 //                   }
 //               }
 //               CheckBox cb = (CheckBox)e.Row.FindControl("chkSelectedYN");
 //               Button b = (Button)e.Row.FindControl("cmdSelectedYN");
 //               string jvscript = "";
 //               if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
 //               {
 //                   cb.Enabled = SessionUtil.CanAccess(this.ViewState, "SELECTJOB");
 //                   cb.Checked = drv["FLDSELECTEDYN"].ToString().Equals("0") ? false : true;

 //                   Label lbl = (Label)e.Row.FindControl("lblJobid");

 //                   //if (b != null) jvscript = "javascript:checkSelectedYN('" + b.ClientID + "');";
 //                   if (b != null) jvscript = "javascript:selectJob('" + lbl.Text + "',this);";
 //                   if (cb != null && b != null) cb.Attributes.Add("onclick", jvscript);
 //               }                
 //               if (b != null) b.Attributes.Add("style", "display:none");

 //               CheckBoxList cbl = (CheckBoxList)e.Row.FindControl("cblVesselType");
 //               if (cbl != null)
 //               {                   
 //                   SetVesselType(cbl, drv["FLDVESSELTYPELIST"].ToString());
 //               }

 //               ImageButton attachments = (ImageButton)e.Row.FindControl("cmdAttachments");
 //               if (drv["FLDISATTACHMENT"].ToString() == "0")
 //                   attachments.ImageUrl = Session["images"] + "/no-attachment.png";
 //               else
 //                   attachments.ImageUrl = Session["images"] + "/attachment.png";

 //               if(drv["FLDCHANGEDYN"].ToString() == "1")
 //               {
 //                   e.Row.Attributes.CssStyle.Add("font-style", "italic");
 //                   e.Row.Attributes.CssStyle.Add("font-weight", "bold");
 //               }

 //               Image img = (Image)e.Row.FindControl("imgChangedBy");
 //               if (img != null && drv["FLDCHANGEDBY"].ToString() != string.Empty && drv["FLDCHANGEDYN"].ToString() == "1")
 //               {
 //                   img.ImageUrl = Session["images"] + "/" + (drv["FLDCHANGEDBY"].ToString() == "0" ? "owner" : "vessel") + ".png";
 //                   img.Visible = true;
 //               }
 //           }

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
    //private void SetRowSelection()
    //{
    //    gvJobGeneral.SelectedIndex = -1;
    //    for (int i = 0; i < gvJobGeneral.Rows.Count; i++)
    //    {
    //        if (gvJobGeneral.DataKeys[i].Value.ToString().Equals(ViewState["JOBID"]))
    //        {
    //            gvJobGeneral.SelectedIndex = i;                
    //            break;
    //        }
    //    }
    //}
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
	//		//gvJobGeneral.SelectedIndex = -1;
	//		//gvJobGeneral.EditIndex = -1;
	//		if (ce.CommandName == "prev")
	//			ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
	//		else
	//			ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

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
    protected string GetVesselType(RadListBox cblVesselType)
    {
        StringBuilder strvesseltype = new StringBuilder();
        foreach (RadListBoxItem item in cblVesselType.Items)
        {
            if (item.Checked)
            {
                strvesseltype.Append(item.Value.ToString());
                strvesseltype.Append(",");
            }
        }

        if (strvesseltype.Length > 1)
            strvesseltype.Remove(strvesseltype.Length - 1, 1);
      
        return strvesseltype.ToString();
    }
    protected void SetVesselType(RadListBox cblVesselType, string csvVesselType)
    {
        if (csvVesselType.Trim().Equals("")) return;
        foreach (RadListBoxItem item in cblVesselType.Items)
        {
          
            if (csvVesselType.Contains("," + item.Value + ","))
            {
                item.Checked = true;
            }
        }
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvJobGeneral_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvJobGeneral.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvJobGeneral_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {          

            if (e.Item is GridDataItem)
            {
              
                    LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                    if (db != null)
                    {
                        db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                        db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                    }
              
                RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkSelectedYN");
                RadButton b = (RadButton)e.Item.FindControl("cmdSelectedYN");
                string jvscript = "";
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    cb.Enabled = SessionUtil.CanAccess(this.ViewState, "SELECTJOB");
                    cb.Checked = DataBinder.Eval(e.Item.DataItem, "FLDSELECTEDYN").ToString().Equals("0") ? false : true;

                    RadLabel lbl = (RadLabel)e.Item.FindControl("lblJobid");

                    //if (b != null) jvscript = "javascript:checkSelectedYN('" + b.ClientID + "');";
                    if (b != null) jvscript = "javascript:selectJob('" + lbl.Text + "',this);";
                    if (cb != null && b != null) cb.Attributes.Add("onclick", jvscript);
                }
                if (b != null) b.Attributes.Add("style", "display:none");

                RadListBox cbl = (RadListBox)e.Item.FindControl("cblVesselType");
                if (cbl != null)
                {
                    SetVesselType(cbl, DataBinder.Eval(e.Item.DataItem, "FLDVESSELTYPELIST").ToString());
                }

                ImageButton attachments = (ImageButton)e.Item.FindControl("cmdAttachments");

              
                if (DataBinder.Eval(e.Item.DataItem, "FLDISATTACHMENT").ToString() == "0")
                    attachments.ImageUrl = Session["images"] + "/no-attachment.png";
                else
                    attachments.ImageUrl = Session["images"] + "/attachment.png";
                //{
                //    HtmlGenericControl html = new HtmlGenericControl();
                //    html.InnerHtml = "<span class=\"icon\" ><i class=\"fas fa-paperclip\"></i></span>";
                //    attachments.Controls.Add(html);
                //}

                Label ownerorvessel = (Label)e.Item.FindControl("lnkbtn11");
                
                if (DataBinder.Eval(e.Item.DataItem, "FLDCHANGEDYN").ToString() == "1")
                {
                    e.Item.Attributes.CssStyle.Add("font-style", "italic");
                    e.Item.Attributes.CssStyle.Add("font-weight", "bold");
                }
                
                  if (ownerorvessel != null && DataBinder.Eval(e.Item.DataItem,"FLDCHANGEDBY").ToString() != string.Empty && DataBinder.Eval(e.Item.DataItem,"FLDCHANGEDYN").ToString() == "1")
                   {
                               if (DataBinder.Eval(e.Item.DataItem, "FLDCHANGEDBY").ToString() == "0")
                               {
                                  HtmlGenericControl html1 = new HtmlGenericControl();
                                  html1.InnerHtml = "<span class=\"icon\" ><i class=\"fas fa-user\"></i></span>";
                                  ownerorvessel.Controls.Add(html1);
                                  //ownerorvessel.Visible = true;
                               }
                             
                               else
                               {
                                 HtmlGenericControl html1 = new HtmlGenericControl();
                                 html1.InnerHtml = "<span class=\"icon\" ><i class=\"fab fa-docker\"></i></span>";
                                 ownerorvessel.Controls.Add(html1);
                                 //ownerorvessel.Visible = true;
                               }
                   // ownerorvessel.ImageUrl = Session["images"] + "/" + (DataBinder.Eval(e.Item.DataItem, "FLDCHANGEDBY").ToString() == "0" ? "owner" : "vessel") + ".png";
                   // ownerorvessel.Visible = true;
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

    protected void gvJobGeneral_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToString().ToUpper() == "SORT")
            return;

        //GridView _gridView = (GridView)sender;
        //int nCurrentRow = int.Parse(e.CommandArgument.ToString());

        if (e.CommandName.ToString().ToUpper().Equals("SELECTJOB"))
        {
            string jobid = ((RadLabel)e.Item.FindControl("lblJobid")).Text;
            int jobregister = 2; // Standard Jobs
            int selectedyn = (((RadCheckBox)e.Item.FindControl("chkSelectedYN")).Checked == true) ? 1 : 0;
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                PhoenixDryDockJobGeneral.UpdateDryDockJobGeneralVessel(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    jobregister, General.GetNullableGuid(jobid), selectedyn);
            }
            gvJobGeneral.Rebind();
        }
        if (e.CommandName.ToString().ToUpper() == "VIEW")
        {
            try
            {
                string jobid = ((RadLabel)e.Item.FindControl("lblJobid")).Text;
                Response.Redirect("../DryDock/DryDockJobGeneral.aspx?STANDARDJOBID=" + jobid + "&pno=" + ViewState["PAGENUMBER"], false);
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
        if (e.CommandName.ToString().ToUpper().Equals("ATTACHMENT"))
        {
            string jobid = ((RadLabel)e.Item.FindControl("lblJobid")).Text;
            string dtkey = ((RadLabel)e.Item.FindControl("lbldtkey")).Text;

            Response.Redirect("../DryDock/DryDockJobAttachments.aspx?STANDARDJOBID=" + jobid + "&DTKEY=" + dtkey + "&MOD=" + PhoenixModule.DRYDOCK + "&pno=" + ViewState["PAGENUMBER"], false);
        }
        if(e.CommandName.ToString().ToUpper().Equals("PAGE"))
                ViewState["PAGENUMBER"]=null;
    }

    protected void gvJobGeneral_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem eeditedItem = e.Item as GridEditableItem;

       try
        {
            string jobid = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDJOBID"].ToString();
            RadListBox cblVesselType = ((RadListBox)e.Item.FindControl("cblVesselType"));
            if (!IsValidMapping(cblVesselType))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixDryDockJobGeneral.UpdateDryDockJobGeneralVesselType(General.GetNullableGuid(jobid), GetVesselType(cblVesselType));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
       // _gridView.EditIndex = -1;
        gvJobGeneral.Rebind();
    }
}
