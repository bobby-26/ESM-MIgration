using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DryDock;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;
using System.Web.UI;

public partial class DryDockStandardUnitList : PhoenixBasePage
{

	protected void Page_Load(object sender, EventArgs e)
	{

		try
		{

			cmdHiddenSubmit.Attributes.Add("style", "display:none");
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbargrid = new PhoenixToolbar();
			toolbargrid.AddFontAwesomeButton("../DryDock/DryDockStandardUnitList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
			toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvStandardUnit')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
			toolbargrid.AddFontAwesomeButton("../DryDock/DryDockStandardUnitList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../DryDock/DryDockStandardUnitList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
			    toolbargrid.AddFontAwesomeButton("../DryDock/DryDockStandardUnitList.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            toolbargrid.AddFontAwesomeButton("../DryDock/DryDockStandardUnitList.aspx", "Include All", "<i class=\"fa fa-check-circle\"></i>", "SELECTALL");

			MenuStandardUnit.AccessRights = this.ViewState;
			MenuStandardUnit.MenuList = toolbargrid.Show();


			if (!IsPostBack)
			{

				PhoenixToolbar toolbarmain = new PhoenixToolbar();

                toolbarmain.AddButton("List", "LIST", ToolBarDirection.Right);
                toolbarmain.AddButton("Details", "DETAIL", ToolBarDirection.Right);
                
             
				MenuStandardStandardUnit.AccessRights = this.ViewState;
				MenuStandardStandardUnit.MenuList = toolbarmain.Show();
                

				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["CURRENTINDEX"] = 1;
				ViewState["JOBID"] = null;
				if (Request.QueryString["StandardUnitID"] != null)
				{
					ViewState["JOBID"] = Request.QueryString["StandardUnitID"].ToString();

				}
                gvStandardUnit.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                //BindData();
            }
            MenuStandardStandardUnit.SelectedMenuIndex = 0;
		    
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void StandardStandardUnit_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("DETAIL"))
			{
                if (ViewState["JOBID"] == null || ViewState["JOBID"].ToString() == "")
                    throw new Exception("Select Material/Labor to view details.");

                Response.Redirect("../DryDock/DryDockStandardUnit.aspx?StandardUnitID=" + ViewState["JOBID"].ToString(), false);
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

 //   protected void gvStandardUnit_RowCommand(object sender, GridViewCommandEventArgs e)
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
	//			Response.Redirect("../DryDock/DryDockStandardUnit.aspx?StandardUnitID=" + jobid, false);
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
 //           int jobregister = 4; // Standard Jobs
 //           int selectedyn = (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkSelectedYN")).Checked == true) ? 1 : 0;
 //           PhoenixDryDockStandardCost.UpdateDryDockStandardUnitVessel(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
 //               jobregister, General.GetNullableGuid(jobid), selectedyn);
 //       }
 //       BindData();

	//}


	protected void ShowExcel()
	{
		try
		{
			int iRowCount = 0;
			int iTotalPageCount = 0;

			string[] alColumns = { "FLDNUMBER", "FLDTITLE", "FLDJOBDESCRIPTION" };
			string[] alCaptions = { "Number", "Title", "Description" };
			string sortexpression;

			sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

            byte? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = byte.Parse(ViewState["SORTDIRECTION"].ToString());

			if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
				iRowCount = 10;
			else
				iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds;
			
            ds = PhoenixDryDockStandardCost.DryDockStandardUnitSearch
                (4,//StandardUnit
                 General.GetNullableString(txtJobTitle.Text),
                 General.GetNullableString(txtJobDesc.Text),
                 sortexpression,
                 sortdirection,
                 1,
                  iRowCount, ref iRowCount, ref iTotalPageCount);
            General.ShowExcel("Material/Labor Tariff", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void StandardUnit_TabStripCommand(object sender, EventArgs e)
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
                gvStandardUnit.CurrentPageIndex = 0;
                gvStandardUnit.Rebind();
				//SetPageNavigator();
			}
			if (CommandName.ToUpper().Equals("ADD"))
			{
				Response.Redirect("../DryDock/DryDockStandardUnitAdd.aspx?StandardUnitID=", false);
			}
            if (CommandName.ToUpper().Equals("SELECTALL"))
            {
                int jobregister = 4;

                PhoenixDryDockStandardCost.UpdateDryDockStandardUnitVessel(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    jobregister, General.GetNullableGuid(""), ToggleStatus());
                //BindData();
                gvStandardUnit.Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                txtJobTitle.Text = "";
                txtJobDesc.Text = "";
                //indData();
                gvStandardUnit.Rebind();
                //SetPageNavigator();
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

			string[] alColumns = { "FLDNUMBER", "FLDTITLE", "FLDJOBDESCRIPTION" };
			string[] alCaptions = { "Number", "Title", "Description" };

			string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            byte? sortdirection = null;
			if (ViewState["SORTDIRECTION"] != null)
                sortdirection = byte.Parse(ViewState["SORTDIRECTION"].ToString());

			DataSet ds;

			ds = PhoenixDryDockStandardCost.DryDockStandardUnitSearch
				(4,//StandardUnit
                 General.GetNullableString(txtJobTitle.Text),
				 General.GetNullableString(txtJobDesc.Text),
                 sortexpression,
                 sortdirection,
				 gvStandardUnit.CurrentPageIndex+1,
				  gvStandardUnit.PageSize, ref iRowCount, ref iTotalPageCount);

			General.SetPrintOptions("gvStandardUnit", "Material/Labor Tariff", alCaptions, alColumns, ds);
            gvStandardUnit.DataSource = ds;
            //gvStandardUnit.DataBind();

            gvStandardUnit.VirtualItemCount = iRowCount;

			//ViewState["ROWCOUNT"] = iRowCount;
			//ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
			//SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	//protected void gvStandardUnit_RowEditing(object sender, GridViewEditEventArgs de)
	//{
	//	GridView _gridView = (GridView)sender;

	//}

	//protected void gvStandardUnit_Sorting(object sender, GridViewSortEventArgs se)
	//{
	//	ViewState["SORTEXPRESSION"] = se.SortExpression;

	//	if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
	//		ViewState["SORTDIRECTION"] = 1;
	//	else
	//		ViewState["SORTDIRECTION"] = 0;


	//	BindData();
	//}

	//protected void gvStandardUnit_SelectIndexChanging(object sender, GridViewSelectEventArgs se)
	//{
 //       GridView _gridView = (GridView)sender;
 //       _gridView.SelectedIndex = se.NewSelectedIndex;
 //       string jobid = ((Label)_gridView.Rows[_gridView.SelectedIndex].FindControl("lblJobid")).Text;
 //       Response.Redirect("../DryDock/DryDockStandardUnit.aspx?StandardUnitID=" + jobid, false);
	//}

	//protected void gvStandardUnit_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

 //               CheckBox cb = (CheckBox)e.Row.FindControl("chkSelectedYN");
 //               Button b = (Button)e.Row.FindControl("cmdSelectedYN");
 //               DataRowView drv = (DataRowView)e.Row.DataItem;
 //               cb.Checked = drv["FLDSELECTEDYN"].ToString().Equals("0") ? false : true;

 //               Label lbl = (Label)e.Row.FindControl("lblJobid");
 //               string jvscript = "";
 //               //if (b != null) jvscript = "javascript:checkSelectedYN('" + b.ClientID + "');";
 //               if (b != null) jvscript = "javascript:selectJob('" + lbl.Text + "',this);";
 //               if (cb != null && b != null) cb.Attributes.Add("onclick", jvscript);
 //               if (b != null) b.Attributes.Add("style", "visibility:hidden");
	//		}

	//	}
	//	catch (Exception ex)
	//	{
	//		ucError.ErrorMessage = ex.Message;
	//		ucError.Visible = true;
	//	}
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
    //    gvStandardUnit.SelectedIndex = -1;
    //    gvStandardUnit.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //    else
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //    BindData();
    //    SetPageNavigator();
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

    protected void gvStandardUnit_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvStandardUnit_ItemCommand(object sender, GridCommandEventArgs e)
    {
        
        GridEditableItem eeditedItem = e.Item as GridEditableItem;

       // int nCurrentRow = e.Item.RowIndex;
       

        if (e.CommandName.ToString().ToUpper() == "EDIT" || e.CommandName.ToString().ToUpper()=="SELECT")
        {
            try
            {
                string jobid = ((RadLabel)eeditedItem.FindControl("lblJobid")).Text;
                Response.Redirect("../DryDock/DryDockStandardUnit.aspx?StandardUnitID=" + jobid, false);
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }

        if (e.CommandName.ToString().ToUpper().Equals("SELECTJOB"))
        {
            string jobid = ((Label)eeditedItem.FindControl("lblJobid")).Text;
            int jobregister = 4; // Standard Jobs
            int selectedyn = (((RadCheckBox)eeditedItem.FindControl("chkSelectedYN")).Checked == true) ? 1 : 0;
            PhoenixDryDockStandardCost.UpdateDryDockStandardUnitVessel(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                jobregister, General.GetNullableGuid(jobid), selectedyn);
        }
        if (e.CommandName.ToUpper().ToString().Equals("PAGE"))
        {
            ViewState["PAGENUMBER"] = null;
        }
       // BindData();
    }
        
    protected void gvStandardUnit_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

           

            if (e.Item is GridDataItem)
            {

                GridEditableItem editedItem = e.Item as GridEditableItem;
                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                }
               
               

                RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkSelectedYN");
                RadButton b = (RadButton)e.Item.FindControl("cmdSelectedYN");
               // DataRowView drv = (DataRowView)e.Row.DataItem;
                cb.Checked = DataBinder.Eval(e.Item.DataItem, "FLDSELECTEDYN").ToString().Equals("0") ? false : true;// drv["FLDSELECTEDYN"].ToString()

                RadLabel lbl = (RadLabel)e.Item.FindControl("lblJobid");
                string jvscript = "";
                //if (b != null) jvscript = "javascript:checkSelectedYN('" + b.ClientID + "');";
                if (b != null) jvscript = "javascript:selectJob('" + lbl.Text + "',this);";
                if (cb != null && b != null) cb.Attributes.Add("onclick", jvscript);
                if (b != null) b.Attributes.Add("style", "visibility:hidden");

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

    protected void gvStandardUnit_Sorting(object sender, GridSortCommandEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        
        BindData();
    }

}
