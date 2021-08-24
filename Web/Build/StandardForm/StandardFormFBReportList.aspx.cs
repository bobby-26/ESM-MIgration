using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.StandardForm;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class StandardFormFBReportList : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    try
    //    {
    //        foreach (GridViewRow r in gvFormList.Rows)
    //        {
    //            if (r.RowType == DataControlRowType.DataRow)
    //            {
    //                Page.ClientScript.RegisterForEventValidation(gvFormList.UniqueID, "Edit$" + r.RowIndex.ToString());
    //            }
    //        }
    //        //if (!IsPostBack && ViewState["COMPONENTID"] != null)
    //        //{
    //        //    tvwComponent.FindNodeByValue(tvwComponent.ThisTreeView.Nodes, ViewState["COMPONENTID"].ToString());
    //        //}
    //        base.Render(writer);
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Reports", "REPORTS", ToolBarDirection.Right);
            toolbarmain.AddButton("Forms", "FORMS",ToolBarDirection.Right);
       
            //toolbarmain.AddButton("Search", "SEARCH");
            //toolbarmain.AddButton("Close", "CLOSE");
			MenuStoreItemInOutTransaction.AccessRights = this.ViewState;
			MenuStoreItemInOutTransaction.MenuList = toolbarmain.Show();
            MenuStoreItemInOutTransaction.SelectedMenuIndex=1;


			cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

			PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../StandardForm/StandardFormFBReportList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvFormList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../StandardForm/StandardFormFBReportList.aspx", "Search", "<i class=\"fas fa-search\"></i>", "FIND");
            //toolbargrid.AddImageButton("../StandardForm/StandardFormFBReportList.aspx", "Refresh", "refresh.png", "REFRESH");
            MenuGridItem.MenuList = toolbargrid.Show();
            //toolbargrid.AddImageButton("../StandardForm/StandardFormFBformsList.aspx", "Add", "add.png", "ADD");
            MenuGridItem.MenuList = toolbargrid.Show();

            if (!IsPostBack)
			{


				ViewState["CategoryId"] = "";
                BindTreeData();
                ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["CURRENTINDEX"] = 1;

				ViewState["FIRSTINITIALIZED"] = true;
                //if (Request.QueryString["COMPONENTID"] != null)
                //    ViewState["COMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();
                //else
                //    ViewState["COMPONENTID"] = "";
                gvFormList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
            //BindData();
            //SetPageNavigator();
        }
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
    protected void BindTreeData()
    {
        try
        {
            DataSet ds = new DataSet();

            ds = PhoenixFormBuilder.CategoryList(0);

            tvwCategory.DataTextField = "FLDFORMCATEGORYNAME";
            tvwCategory.DataValueField = "FLDFORMCATEGORYID";
            tvwCategory.DataFieldParentID = "FLDPARENTCATEGORYID";
            //tvwCategory.XPathField = "xpath";
            tvwCategory.RootText = "Category";
            tvwCategory.PopulateTree(ds.Tables[0]);
            //TreeView tvw = tvwCategory.ThisTreeView;
            //((TreeNode)tvw.Nodes[0]).Expand();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

	protected void StoreItemInOutTransaction_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

			if (dce.CommandName.ToUpper().Equals("CLOSE"))
			{
				string Script = "";
				Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
				Script += " fnReloadList();";
				Script += "</script>" + "\n";
				Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
			}

			if (dce.CommandName.ToUpper().Equals("SEARCH"))
			{
				ViewState["PAGENUMBER"] = 1;
				BindData();
				gvFormList.Rebind();
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
    //protected void BindTreeData()
    //{
    //    try
    //    {
    //        DataSet ds = new DataSet();

    //        ds = PhoenixFormBuilder.CategoryList(0);

    //        tvwCategory.DataTextField = "FLDFORMCATEGORYNAME";
    //        tvwCategory.DataValueField = "FLDFORMCATEGORYID";
    //        tvwCategory.ParentNodeField = "FLDPARENTCATEGORYID";
    //        tvwCategory.XPathField = "xpath";
    //        tvwCategory.RootText = "Category";
    //        tvwCategory.PopulateTree(ds);
    //        TreeView tvw = tvwCategory.ThisTreeView;
    //        ((TreeNode)tvw.Nodes[0]).Expand();

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
	protected void ShowExcel()
	{
		try
		{
			int iRowCount = 0;
			int iTotalPageCount = 0;
			string[] alColumns = { "FLDFORMNAME", "FLDSTATUSNAME" };
            string[] alCaptions = { "Name", "Status" };
			string sortexpression;
			int? sortdirection = null;

			sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
			if (ViewState["SORTDIRECTION"] != null)
				sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

			if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
				iRowCount = 10;
			else
				iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            //DataSet ds = PhoenixFormBuilder.ReportSearch(null, null, sortexpression
            //                                            , sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString())
            //                                            , General.ShowRecords(null)
            //                                            , ref iRowCount
            //                                            , ref iTotalPageCount, General.GetNullableInteger(ViewState["CategoryId"].ToString()));
            DataSet ds = PhoenixFormBuilder.FormsSearch(General.GetNullableInteger(ViewState["CategoryId"].ToString()), txtFormName.Text
                                                       , 1, sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                       , General.ShowRecords(null)
                                                       , ref iRowCount
                                                       , ref iTotalPageCount, 1);



            General.ShowExcel("Form List", ds.Tables[0], alColumns, alCaptions, null, null);

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
    protected void gvFormList_UpdateCommand(object sender, GridCommandEventArgs e)
    {

    }
    protected void gvFormList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvFormList_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
	protected void MenuGridStoreItemInOutTransaction_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
			if (CommandName.ToUpper().Equals("FIND"))
			{
				ViewState["PAGENUMBER"] = 1;
				BindData();
				gvFormList.Rebind();
			}
            if (CommandName.ToUpper().Equals("REPORTS"))
            {
                Response.Redirect("../StandardForm/StandardFormFBFormReports.aspx");
            }
			if (CommandName.ToUpper().Equals("EXCEL"))
			{
				ShowExcel();
			}
            if (CommandName.ToUpper().Equals("REFRESH"))
            {
                Response.Redirect("../StandardForm/StandardFormFBReportList.aspx");
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
			string[] alColumns = { "FLDFORMNAME", "FLDSTATUSNAME"};
            string[] alCaptions = { "Name", "Status" };

			string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
			int? sortdirection = null;
			if (ViewState["SORTDIRECTION"] != null)
				sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            //DataSet ds = PhoenixFormBuilder.ReportSearch(null, null, sortexpression
            //                                            , sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString())
            //                                            , General.ShowRecords(null)
            //                                            , ref iRowCount
            //                                            , ref iTotalPageCount, General.GetNullableInteger(ViewState["CategoryId"].ToString()));

            DataSet ds = PhoenixFormBuilder.FormsSearch(General.GetNullableInteger(ViewState["CategoryId"].ToString()), txtFormName.Text
                                                        , 1, sortexpression, sortdirection, gvFormList.CurrentPageIndex + 1, gvFormList.PageSize
                                                        , ref iRowCount
                                                        , ref iTotalPageCount, 1);

			General.SetPrintOptions("gvFormList", "Form List", alCaptions, alColumns, ds);

		
				gvFormList.DataSource = ds;
                gvFormList.VirtualItemCount = iRowCount;
			
			
			ViewState["ROWCOUNT"] = iRowCount;
			ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
    protected void gvFormList_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }
	protected void gvFormList_Sorting(object sender, GridViewSortEventArgs se)
	{
		ViewState["SORTEXPRESSION"] = se.SortExpression;

		if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
			ViewState["SORTDIRECTION"] = 1;
		else
			ViewState["SORTDIRECTION"] = 0;
		BindData();
		gvFormList.Rebind();
	}

	protected void cmdSort_Click(object sender, EventArgs e)
	{
		try
		{
			ImageButton ib = (ImageButton)sender;
			ViewState["SORTEXPRESSION"] = ib.CommandName;
			ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
			BindData();
			gvFormList.Rebind();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvFormList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			_gridView.EditIndex = -1;
			BindData();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvFormList_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{

	}
    protected void gvFormList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
         
            {
                DataRowView dv = (DataRowView)e.Item.DataItem;
                //string reportId = ((Label)e.Row.FindControl("lblReportId")).Text;
                string formId = ((Label)e.Item.FindControl("lblFormId")).Text;
                string statusname = ((Label)e.Item.FindControl("lblStatusname")).Text;

                LinkButton lnkReportName = (LinkButton)e.Item.FindControl("lnkReportName");
                if (lnkReportName != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, lnkReportName.CommandName))
                    {
                        lnkReportName.Visible = false;
                    }

                    lnkReportName.Attributes.Add("onclick", "javascript:return openNewWindow('codehelp1','','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FormId=" + formId + "'); return false;");

                }

                LinkButton cmddelete = (LinkButton)e.Item.FindControl("cmdDelete");
                if (cmddelete != null)
                {
                    cmddelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    cmddelete.Visible = SessionUtil.CanAccess(this.ViewState, cmddelete.CommandName);
                }
                LinkButton ce = (LinkButton)e.Item.FindControl("cmdEdit");
                if (ce != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, ce.CommandName)) ce.Visible = false;

                }

            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvFormList_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void OnDeleteCommand(object sender, GridCommandEventArgs de)
	{
		try
		{
        
            string formId = ((Label)de.Item.FindControl("lblFormId")).Text;
            PhoenixFormBuilder.FormDelete(new Guid(formId));

            BindData();
            gvFormList.Rebind();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvFormList_RowCreated(object sender, GridViewRowEventArgs e)
	{
		//if (e.Row.RowType == DataControlRowType.DataRow
		//	&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
		//	&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
		//{
		//	e.Row.TabIndex = -1;
		//	e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvFormList, "Edit$" + e.Row.RowIndex.ToString(), false);
		//}
		//SetKeyDownScroll(sender, e);
	}

	protected void gvFormList_RowEditing(object sender, GridViewEditEventArgs de)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			if (_gridView.EditIndex > -1)
				_gridView.UpdateRow(_gridView.EditIndex, false);

			_gridView.EditIndex = de.NewEditIndex;
			_gridView.SelectedIndex = de.NewEditIndex;

			BindData();
			gvFormList.Rebind();
			//((CheckBox)_gridView.Rows[de.NewEditIndex].FindControl("chkEditActive")).();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

    //protected void gvFormList_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
    //            DataRowView dv = (DataRowView)e.Row.DataItem;
    //            //string reportId = ((Label)e.Row.FindControl("lblReportId")).Text;
    //            string formId = ((Label)e.Row.FindControl("lblFormId")).Text;
    //            string statusname =((Label)e.Row.FindControl("lblStatusname")).Text;

    //            LinkButton lnkReportName = (LinkButton)e.Row.FindControl("lnkReportName");
    //            if (lnkReportName != null)
    //            {
    //                if (!SessionUtil.CanAccess(this.ViewState, lnkReportName.CommandName))
    //                {
    //                    lnkReportName.Visible = false;
    //                }

    //                lnkReportName.Attributes.Add("onclick", "javascript:return Openpopup('codehelp1','','../StandardForm/StandardFormFBformView.aspx?FormId=" + formId + "'); return false;");

    //            }

    //            ImageButton cmddelete = (ImageButton)e.Row.FindControl("cmdDelete");
    //            if (cmddelete != null)
    //            {
    //                cmddelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //                cmddelete.Visible = SessionUtil.CanAccess(this.ViewState, cmddelete.CommandName);
    //            }
    //            ImageButton ce = (ImageButton)e.Row.FindControl("cmdEdit");
    //            if (ce != null)
    //            {
    //                if (!SessionUtil.CanAccess(this.ViewState, ce.CommandName)) ce.Visible = false;

    //            }

    //        }
	

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void cmdSearch_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        BindData();
    //        Set;
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int result;
    //        if (Int32.TryParse(txtnopage.Text, out result))
    //        {
    //            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

    //            if (0 >= Int32.Parse(txtnopage.Text))
    //                ViewState["PAGENUMBER"] = 1;

    //            if ((int)ViewState["PAGENUMBER"] == 0)
    //                ViewState["PAGENUMBER"] = 1;

    //            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //        }
    //        BindData();
    //        SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    try
    //    {
    //        gvFormList.SelectedIndex = -1;
    //        gvFormList.EditIndex = -1;
    //        if (ce.CommandName == "prev")
    //            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //        else
    //            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //        BindData();
    //        SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //private void SetPageNavigator()
    //{
    //    try
    //    {
    //        cmdPrevious.Enabled = IsPreviousEnabled();
    //        cmdNext.Enabled = IsNextEnabled();
    //        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //private Boolean IsPreviousEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //        return true;

    //    return false;
    //}


	

	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		try
		{
			BindData();
			gvFormList.Rebind();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	

	protected void ucTree_SelectNodeEvent(object sender, EventArgs e)
	{
		try
		{
            RadTreeNodeEventArgs tvsne = (RadTreeNodeEventArgs)e;

			if (tvsne.Node.Value != null)
			{
				string selectednode = tvsne.Node.Value.ToString();
				string selectedvalue = tvsne.Node.Text.ToString();
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["CategoryId"] = selectednode;
			}
			//string script = "var ar = document.getElementById(\"tvwComponent_tvwTreet0\"); ar.href=\"#\"; ar.onclick=null;\r\n;resizew();";
			//ScriptManager.RegisterStartupScript(this, typeof(Page), "disableScript", script, true);
			BindData();
			gvFormList.Rebind();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	private void formUpdate(string formId,string frmname,int activeYn,string publishDate)
	{
		try
		{
			PhoenixFormBuilder.FormUpdate(new Guid(formId), frmname, activeYn, General.GetNullableDateTime(publishDate));
		}
		catch(Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	
	protected void MenuGridItem_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
			if (CommandName.ToUpper().Equals("FIND"))
			{
				ViewState["PAGENUMBER"] = 1;
				BindData();
				gvFormList.Rebind();
			}
			if (CommandName.ToUpper().Equals("EXCEL"))
			{
				ShowExcel();
			}
			if(CommandName.ToUpper().Equals("ADD"))
			{
				if (!string.IsNullOrEmpty(ViewState["CategoryId"].ToString()))
				{
					Response.Redirect("StandardFormFBFormCreate.aspx?CategoryId="+ViewState["CategoryId"]);
				}
				else
				{
					ucError.ErrorMessage = "Please select category";
					ucError.Visible = true;
				}
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	private void InsertForm(string fromName)
	{
		if(!IsValidForms(fromName))
		{
			ucError.Visible = true;
			return;
		}
		PhoenixFormBuilder.FormCreate(int.Parse(ViewState["CategoryId"].ToString()), fromName, null, 1);
	}
	private bool IsValidForms(string frmName)
	{

		ucError.HeaderMessage = "Please provide the following required information";

		if (frmName.Trim().Equals(""))
			ucError.ErrorMessage = "Name is required.";
		if (string.IsNullOrEmpty(ViewState["CategoryId"].ToString()))
			ucError.ErrorMessage = "Category is required.";

		return (!ucError.IsError);
	}
}