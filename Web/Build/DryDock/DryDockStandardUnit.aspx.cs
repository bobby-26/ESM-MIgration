using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.DryDock;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Text;
using Telerik.Web.UI;


public partial class DryDockStandardUnit : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

			PhoenixToolbar toolbar = new PhoenixToolbar();
			PhoenixToolbar toolbargrid = new PhoenixToolbar();

			toolbargrid.AddFontAwesomeButton("../DryDock/DryDockStandardUnit.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvStandardUnitsLineItem')","Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            
            //      MenuDryDockStandardUnitsLineItem.AccessRights = this.ViewState;
            //      MenuDryDockStandardUnitsLineItem.MenuList = toolbargrid.Show();

            if (!IsPostBack)
			{
				ViewState["StandardUnitID"] = null;
				if (Request.QueryString["StandardUnitID"] != null)
				{
					ViewState["StandardUnitID"] = Request.QueryString["StandardUnitID"];
				}
                
                toolbar.AddButton("List", "LIST",ToolBarDirection.Right);

                toolbar.AddButton("Details", "DETAIL", ToolBarDirection.Right);

                MenuHeader.AccessRights = this.ViewState;
				MenuHeader.MenuList = toolbar.Show();

				toolbar = new PhoenixToolbar();
				toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
				MenuStandardUnitSpecification.AccessRights = this.ViewState;
				MenuStandardUnitSpecification.MenuList = toolbar.Show();
				MenuHeader.SelectedMenuIndex = 1;

				BindFields(new Guid(Request.QueryString["StandardUnitID"]));
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["CURRENTINDEX"] = 1;

               // gvStandardUnitsLineItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
			//BindData();

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void DryDockStandardUnitsLineItem_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
		if (dce.CommandName.ToUpper().Equals("EXCEL"))
		{
			ShowExcel();
		}
	}
	protected void ShowExcel()
	{
		try
		{

			string[] alColumns = { "FLDDESCRIPTION", "FLDUNITNAME" };
			string[] alCaptions = { "Description", "Unit" };

			int? sortdirection = null;

            DataSet ds = PhoenixDryDockStandardCost.ListDryDockStandardUnitDetail(
			General.GetNullableGuid(ViewState["StandardUnitID"].ToString()));

			General.ShowExcel("Material/Labor Tariff Line Item", ds.Tables[0], alColumns, alCaptions, sortdirection, null);

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	private void BindFields(Guid? StandardUnitID)
	{
		try
		{
			if ((StandardUnitID != null))
			{
                DataSet ds = PhoenixDryDockStandardCost.EditDryDockStandardUnit((StandardUnitID));
				DataRow dr = ds.Tables[0].Rows[0];
				txtNumber.Text = dr["FLDNUMBER"].ToString();
				txtTitle.Text = dr["FLDTITLE"].ToString();
				txtJobDescription.Text = dr["FLDJOBDESCRIPTION"].ToString();

				ViewState["OPERATIONMODE"] = "EDIT";
			}
			else
			{
				ViewState["OPERATIONMODE"] = "ADD";
			}

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}


	protected void StandardUnitSpecification_TabStripCommand(object sender, EventArgs e)
	{

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidRepairSpec())
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixDryDockStandardCost.UpdateDryDockStandardUnit(
                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                    new Guid(ViewState["StandardUnitID"].ToString()),
                                                    1,//StandardUnit
                                                    General.GetNullableString(txtNumber.Text.Trim()),
                                                    txtTitle.Text.Trim(),
                                                    General.GetNullableString(txtJobDescription.Text.Trim()));

                ucStatus.Text = "Details Updated.";
                BindFields(new Guid(ViewState["StandardUnitID"].ToString()));
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

		ucError.HeaderMessage = "Provide the following required information.";

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
            ucError.ErrorMessage = "Cannot make changes while you are in ship. Switch to Office to make changes.";

		if (txtNumber.Text.Trim().Equals(""))
			ucError.ErrorMessage = "Number cannot be blank.";

		if (txtTitle.Text.Trim().Equals(""))
			ucError.ErrorMessage = "Title cannot be blank.";

		if (txtJobDescription.Text.Trim().Equals(""))
			ucError.ErrorMessage = "Description cannot be blank.";


		return (!ucError.IsError);
	}
	protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
	{
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("DETAIL"))
		{
            Response.Redirect("../DryDock/DryDockStandardUnit.aspx?StandardUnitID=" + ViewState["StandardUnitID"].ToString(), false);
		}
		else if (CommandName.ToUpper().Equals("LIST"))
		{
			Response.Redirect("../DryDock/DryDockStandardUnitList.aspx?StandardUnitID=" + ViewState["StandardUnitID"].ToString(), false);
		}

	}

	private void BindData()
	{


        string[] alColumns = { "FLDDESCRIPTION", "FLDUNITNAME"};
		string[] alCaptions = { "Description", "Unit" };

		DataSet ds = PhoenixDryDockStandardCost.ListDryDockStandardUnitDetail(
			General.GetNullableGuid(ViewState["StandardUnitID"].ToString()));

       General.SetPrintOptions("gvStandardUnitsLineItem", " Material/Labor Tariff Line Item", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvStandardUnitsLineItem.PageSize = ds.Tables[0].Rows.Count;

        }
        gvStandardUnitsLineItem.DataSource = ds;

        //gvStandardUnitsLineItem.DataBind();



    }

	protected void cmdSort_Click(object sender, EventArgs e)
	{
		ImageButton ib = (ImageButton)sender;

		ViewState["SORTEXPRESSION"] = ib.CommandName;
		ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

	}

	protected void gvStandardUnitsLineItem_RowCreated(object sender, GridViewRowEventArgs e)
	{

	}

	//protected void gvStandardUnitsLineItem_Sorting(object sender, GridViewSortEventArgs se)
	//{
	//	gvStandardUnitsLineItem.SelectedIndex = -1;
	//	gvStandardUnitsLineItem.EditIndex = -1;

	//	ViewState["SORTEXPRESSION"] = se.SortExpression;

	//	if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
	//		ViewState["SORTDIRECTION"] = 1;
	//	else
	//		ViewState["SORTDIRECTION"] = 0;

	//	BindData();
	//}

	protected void gvStandardUnitsLineItem_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = e.RowIndex;
			if (!IsValidRepairJobDetail(ViewState["StandardUnitID"].ToString(),
						((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDetailEdit")).Text))
			{
				ucError.Visible = true;
				return;
			}

			PhoenixDryDockStandardCost.UpdateDryDockStandardUnitDetail(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
				 new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lbljobdetailidEdit")).Text),
				 new Guid(ViewState["StandardUnitID"].ToString()),
				 ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDetailEdit")).Text.Trim(),
				General.GetNullableInteger(((UserControlUnit)_gridView.Rows[nCurrentRow].FindControl("ucUnitEdit")).SelectedUnit));



			_gridView.EditIndex = -1;
			BindData();
            gvStandardUnitsLineItem.Rebind();

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	//protected void gvStandardUnitsLineItem_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
	//{
	//	gvStandardUnitsLineItem.SelectedIndex = e.NewSelectedIndex;
	//}

	protected void gvStandardUnitsLineItem_RowEditing(object sender, GridViewEditEventArgs de)
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

	//      protected void gvStandardUnitsLineItem_ItemCommand(object sender, GridCommandEventArgs e)
	//      {
	//      	try
	//      	{
	//      		if (e.CommandName.ToUpper().Equals("ADD"))
	//      		{
	//      			if (!IsValidRepairJobDetail(ViewState["StandardUnitID"].ToString(),
	//      				((RadTextBox)e.Item.FindControl("txtDetailAdd")).Text))
	//      			{
	//      				ucError.Visible = true;
	//      				return;
	//      			}
	//      			Guid? jobdetailid = null;
	//      			jobdetailid = PhoenixDryDockStandardCost.InsertDryDockStandardUnitDetail(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
	//      			 new Guid(ViewState["StandardUnitID"].ToString()),
	//      			 ((RadTextBox)e.Item.FindControl("txtDetailAdd")).Text.Trim(),
	//      			General.GetNullableInteger(((UserControlUnit)e.Item.FindControl("ucUnitAdd")).SelectedUnit),	ref jobdetailid);
   //                  BindData();
   //                  gvStandardUnitsLineItem.Rebind();
	//      		}
	//      	}
	//      	catch (Exception ex)
	//      	{
	//      		ucError.ErrorMessage = ex.Message;
	//      		ucError.Visible = true;
	//      	}
	//      }

	//protected void gvStandardUnitsLineItem_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

	//	if (e.Row.RowType == DataControlRowType.Footer)
	//	{
	//		ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
	//		if (db != null)
	//		{
	//			if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
	//				db.Visible = false;
	//		}
	//	}

	//	if (e.Row.RowType == DataControlRowType.DataRow)
	//	{

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


	//		ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
	//		if (db != null)
	//		{
	//			db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
	//		}
	//		UserControlUnit ucUnit = (UserControlUnit)e.Row.FindControl("ucUnitEdit");
	//		DataRowView drv = (DataRowView)e.Row.DataItem;
	//		if (ucUnit != null) ucUnit.SelectedUnit = drv["FLDUNIT"].ToString();
	//	}
	//}

	protected void gvStandardUnitsLineItem_RowDeleting(object sender, GridViewDeleteEventArgs de)
	{
		BindData();

	}

	protected void gvStandardUnitsLineItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		BindData();
	}

	//protected void cmdSearch_Click(object sender, EventArgs e)
	//{
	//	gvStandardUnitsLineItem.SelectedIndex = -1;
	//	gvStandardUnitsLineItem.EditIndex = -1;
	//	BindData();

	//}

	public void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		BindData();
		BindFields(new Guid(ViewState["StandardUnitID"].ToString()));
	}

	private bool IsValidRepairJobDetail(string StandardUnitID, string jobdetail)
	{

		ucError.HeaderMessage = "Provide the following required information";

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
            ucError.ErrorMessage = "Cannot make changes while you are in ship. Switch to Office to make changes.";

		if (General.GetNullableGuid(StandardUnitID) == null)
			ucError.ErrorMessage = "Create a Standard job to amend the details";

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

    protected void gvStandardUnitsLineItem_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvStandardUnitsLineItem_InsertCommand(object sender, GridCommandEventArgs e)
    {
        var editableItem = ((GridEditableItem)e.Item);
        if (!IsValidRepairJobDetail(ViewState["StandardUnitID"].ToString(),
                    ((RadTextBox)editableItem.FindControl("txtDetailEdit")).Text))
        {
            ucError.Visible = true;
            e.Canceled = true;
            return;
        }
        Guid? jobdetailid = null;
        jobdetailid = PhoenixDryDockStandardCost.InsertDryDockStandardUnitDetail(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
         new Guid(ViewState["StandardUnitID"].ToString()),
         ((RadTextBox)editableItem.FindControl("txtDetailEdit")).Text.Trim(),
        General.GetNullableInteger(((UserControlUnit)editableItem.FindControl("ucUnitEdit")).SelectedUnit),
        ref jobdetailid);

        ucStatus.Text = "Information Added";
        e.Canceled = true;
        gvStandardUnitsLineItem.Rebind();
    }

    protected void gvStandardUnitsLineItem_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            // int nCurrentRow = e.RowIndex;
            if (!IsValidRepairJobDetail(ViewState["StandardUnitID"].ToString(),
                        ((RadTextBox)eeditedItem.FindControl("txtDetailEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixDryDockStandardCost.UpdateDryDockStandardUnitDetail(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                 new Guid(((RadLabel)eeditedItem.FindControl("lbljobdetailidEdit")).Text),
                 new Guid(ViewState["StandardUnitID"].ToString()),
                 ((RadTextBox)eeditedItem.FindControl("txtDetailEdit")).Text.Trim(),
                General.GetNullableInteger(((UserControlUnit)eeditedItem.FindControl("ucUnitEdit")).SelectedUnit));


            BindData();
            // _gridView.EditIndex = -1;
            gvStandardUnitsLineItem.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStandardUnitsLineItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            ImageButton edit = (ImageButton)item.FindControl("cmdEdit");
            if (edit != null)
            {
                edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
            }

            ImageButton save = (ImageButton)item.FindControl("cmdSave");
            if (save != null)
            {
                save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);
            }

            ImageButton cancel = (ImageButton)item.FindControl("cmdCancel");
            if (cancel != null)
            {
                cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
            }


            ImageButton db = (ImageButton)item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            UserControlUnit ucUnit = (UserControlUnit)item.FindControl("ucUnitEdit");
         
            if (ucUnit != null) ucUnit.SelectedUnit = DataBinder.Eval(e.Item.DataItem, "FLDUNIT").ToString();//  drv["FLDUNIT"].ToString();
        }
    }

    protected void gvStandardUnitsLineItem_PreRender(object sender, EventArgs e)
    {

    }

    protected void gvStandardUnitsLineItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.ExportToExcelCommandName)
        {
            ShowExcel();
        }
        if(e.CommandName.ToUpper().Equals("ADD"))
        {   
            if (!IsValidRepairJobDetail(ViewState["StandardUnitID"].ToString(),((RadTextBox)e.Item.FindControl("txtDetailAdd")).Text))
            {
                ucError.Visible = true;
                e.Canceled = true;
                return;
            }
            Guid? jobdetailid = null;
            jobdetailid = PhoenixDryDockStandardCost.InsertDryDockStandardUnitDetail(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
             new Guid(ViewState["StandardUnitID"].ToString()),((RadTextBox)e.Item.FindControl("txtDetailAdd")).Text.Trim(),
                                                                General.GetNullableInteger(((UserControlUnit)e.Item.FindControl("ucUnitAdd")).SelectedUnit),
            ref jobdetailid);
                                                                                    
            ucStatus.Text = "Information Added";
            e.Canceled = true;
            gvStandardUnitsLineItem.Rebind();
        }
    }
}
