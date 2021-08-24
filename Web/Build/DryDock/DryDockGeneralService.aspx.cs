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


public partial class DryDockGeneralService : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

			PhoenixToolbar toolbar = new PhoenixToolbar();
			PhoenixToolbar toolbargrid = new PhoenixToolbar();

			toolbargrid.AddFontAwesomeButton("../DryDock/DryDockGeneralService.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
			toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvGeneralServicesLineItem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

			MenuDryDockGeneralServicesLineItem.AccessRights = this.ViewState;
			MenuDryDockGeneralServicesLineItem.MenuList = toolbargrid.Show();

			if (!IsPostBack)
			{
				ViewState["GENERALSERVICEID"] = null;
				if (Request.QueryString["GENERALSERVICEID"] != null)
				{
					ViewState["GENERALSERVICEID"] = Request.QueryString["GENERALSERVICEID"];
				}
                toolbar.AddButton("List", "LIST");
                toolbar.AddButton("Details", "DETAIL");
               
				
				MenuHeader.AccessRights = this.ViewState;
				MenuHeader.MenuList = toolbar.Show();

				toolbar = new PhoenixToolbar();
				toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
				MenuGeneralServiceSpecification.AccessRights = this.ViewState;
				MenuGeneralServiceSpecification.MenuList = toolbar.Show();
				MenuHeader.SelectedMenuIndex = 1;
				BindCheckBox();
				BindFields(new Guid(Request.QueryString["GENERALSERVICEID"]));
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["CURRENTINDEX"] = 1;
                BindData();
			}
			//BindData();

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void DryDockGeneralServicesLineItem_TabStripCommand(object sender, EventArgs e)
	{
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("EXCEL"))
		{
			ShowExcel();
		}
	}
	protected void ShowExcel()
	{
		try
		{

			string[] alColumns = { "FLDDESCRIPTION", "FLDUNITNAME" };
			string[] alCaptions = { "Job detail", "Unit" };

			int? sortdirection = null;

            DataSet ds = PhoenixDryDockJobGeneral.ListDryDockJobGeneralDetail(General.GetNullableGuid(ViewState["GENERALSERVICEID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

			General.ShowExcel("General Service Line Item", ds.Tables[0], alColumns, alCaptions, sortdirection, null);
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	private void BindFields(Guid? GENERALSERVICEID)
	{
		try
		{
			if ((GENERALSERVICEID != null))
			{
				DataSet ds = PhoenixDryDockJobGeneral.EditDryDockJobGeneral(GENERALSERVICEID, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
				DataRow dr = ds.Tables[0].Rows[0];
				txtNumber.Text = dr["FLDNUMBER"].ToString();
				txtTitle.Text = dr["FLDTITLE"].ToString();
				ddlResponsibilty.SelectedValue = dr["FLDRESPONSIBILITY"].ToString();
				txtDuration.Text = dr["FLDESTIMATEDHOURS"].ToString();
				if (dr["FLDESTIMATEDHOURS"].ToString() != "")
				{
					txtDuration.Text = Math.Round(Convert.ToDecimal(txtDuration.Text)).ToString();
				}
				ddlJobType.SelectedValue = dr["FLDJOBTYPE"].ToString();
				txtJobDescription.Text = dr["FLDJOBDESCRIPTION"].ToString();

                BindCheckBoxList(cblEnclosed, dr["FLDENCLOSED"].ToString());
                BindCheckBoxList(cblMaterial, dr["FLDMATERIAL"].ToString());
                BindCheckBoxList(cblWorkSurvey, dr["FLDWORKSURVEYBY"].ToString());
                BindCheckBoxList(cblInclude, dr["FLDINCLUDE"].ToString());
                if (dr["FLDESTIMATEDDAYS"].ToString() != string.Empty)
                    rblEstimatedDays.SelectedValue = dr["FLDESTIMATEDDAYS"].ToString();
                if (dr["FLDBUDGECODEID"].ToString() != string.Empty)
                    radddlbudgetcode.SelectedValue = dr["FLDBUDGECODEID"].ToString();
                ddlCostClassification.SelectedValue = dr["FLDCOSTCLASSIFICATION"].ToString();
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

    private void BindCheckBoxList(RadCheckBoxList cbl, string list)
    {
        //foreach (string item in list.Split(','))
        //{
        //    if (item.Trim() != "")
        //    {
        //        if (cbl.FindByValue(item) != null)
        //            cbl.Items.FindByValue(item).Selected = true;
        //    }
        //}
    }

	protected void BindCheckBox()
	{
		string type = string.Empty;

        cblMaterial.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(1);
        cblMaterial.DataBindings.DataTextField = "FLDNAME";
        cblMaterial.DataBindings.DataValueField = "FLDMULTISPECID";
        cblMaterial.DataBind();

		cblEnclosed.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(4);
		cblEnclosed.DataBindings.DataTextField = "FLDNAME";
		cblEnclosed.DataBindings.DataValueField = "FLDMULTISPECID";
		cblEnclosed.DataBind();


        cblInclude.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(3);
        cblInclude.DataBindings.DataTextField = "FLDNAME";
        cblInclude.DataBindings.DataValueField = "FLDMULTISPECID";
        cblInclude.DataBind();


        cblWorkSurvey.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(2);
        cblWorkSurvey.DataBindings.DataTextField = "FLDNAME";
        cblWorkSurvey.DataBindings.DataValueField = "FLDMULTISPECID";
        cblWorkSurvey.DataBind();

        ddlResponsibilty.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(5);
        ddlResponsibilty.DataTextField = "FLDNAME";
        ddlResponsibilty.DataValueField = "FLDMULTISPECID";
        ddlResponsibilty.DataBind();

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

        radddlbudgetcode.DataSource = PhoenixDryDockOrder.DrydockBudgetcodeList(null);
        radddlbudgetcode.DataTextField = "FLDBUDGETCODE";
        radddlbudgetcode.DataValueField = "FLDBUDGETID";
        radddlbudgetcode.DataBind();
    }

    protected void GeneralServiceSpecification_TabStripCommand(object sender, EventArgs e)
	{
        try
        {



            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidRepairSpec())
                {
                    ucError.Visible = true;
                    return;
                }

                string strMat = ReadCheckBoxList(cblMaterial);
                string strEnc = ReadCheckBoxList(cblEnclosed);
                string strWrk = ReadCheckBoxList(cblWorkSurvey);
                string strInc = ReadCheckBoxList(cblInclude);
                string strEst = rblEstimatedDays.SelectedValue;

                PhoenixDryDockJobGeneral.UpdateDryDockJobGeneral(
                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                    new Guid(ViewState["GENERALSERVICEID"].ToString()),
                                                    1,//generalservice
                                                    General.GetNullableString(txtNumber.Text).Trim(),
                                                    txtTitle.Text.Trim(),
                                                    ddlResponsibilty.SelectedValue,
                                                    General.GetNullableDecimal(txtDuration.Text),
                                                    null,
                                                    General.GetNullableInteger(ddlJobType.SelectedValue),
                                                    General.GetNullableString(txtJobDescription.Text).Trim(), null,
                                                    strWrk, strInc, strMat, strEnc,
                                                    General.GetNullableInteger(ddlCostClassification.SelectedValue), strEst);

                PhoenixDryDockJobGeneral.DrydockJobGenralBudgetcodeInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                   , General.GetNullableGuid(ViewState["GENERALSERVICEID"].ToString())
                   , General.GetNullableInteger(radddlbudgetcode.SelectedValue));

                ucStatus.Text = "Details Updated.";
                BindFields(new Guid(ViewState["GENERALSERVICEID"].ToString()));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


	}

    private string ReadCheckBoxList(RadCheckBoxList cbl)
    {
        string list = string.Empty;

        foreach (ButtonListItem item in cbl.Items)
        {
            if (item.Selected == true)
            {
                list = list + item.Value.ToString() + ",";
            }
        }
        list = list.TrimEnd(',');
        return list;
    }

	private bool IsValidRepairSpec()
	{

		ucError.HeaderMessage = "Provide the following required information.";

        if(PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
            ucError.ErrorMessage = "Cannot make changes while you are in ship. Switch to Office to make changes.";

		if (txtNumber.Text.Trim().Equals(""))
			ucError.ErrorMessage = "Number cannot be blank.";

		if (txtTitle.Text.Trim().Equals(""))
			ucError.ErrorMessage = "Title cannot be blank.";

		if (txtJobDescription.Text.Trim().Equals(""))
			ucError.ErrorMessage = "Job Description cannot be blank.";

		if (General.GetNullableInteger(ddlJobType.SelectedValue) == null)
			ucError.ErrorMessage = "Job Type cannot be blank.";

		return (!ucError.IsError);
	}
	protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
	{
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("DETAIL"))
		{
            Response.Redirect("../DryDock/DryDockGeneralService.aspx?GENERALSERVICEID=" + ViewState["GENERALSERVICEID"].ToString() + "&pno=" + Request.QueryString["pno"], false);
		}
		else if (CommandName.ToUpper().Equals("LIST"))
		{
            Response.Redirect("../DryDock/DryDockGeneralServiceList.aspx?GENERALSERVICEID=" + ViewState["GENERALSERVICEID"].ToString() + "&pno=" + Request.QueryString["pno"], false);
		}

	}

	private void BindData()
	{


        string[] alColumns = { "FLDDESCRIPTION", "FLDUNITNAME" };
		string[] alCaptions = {"Job Detail", "Unit"};

		DataSet ds = PhoenixDryDockJobGeneral.ListDryDockJobGeneralDetail(General.GetNullableGuid(ViewState["GENERALSERVICEID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

		General.SetPrintOptions("gvGeneralServicesLineItem", "General services Line Item", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvGeneralServicesLineItem.PageSize = ds.Tables[0].Rows.Count;

        }
        gvGeneralServicesLineItem.DataSource = ds;
        //gvGeneralServicesLineItem.DataBind();




    }

    protected void cmdSort_Click(object sender, EventArgs e)
	{
		ImageButton ib = (ImageButton)sender;

		ViewState["SORTEXPRESSION"] = ib.CommandName;
		ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

	}

	protected void gvGeneralServicesLineItem_RowCreated(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow
			&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
			&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
		{
			e.Row.TabIndex = -1;
			e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvGeneralServicesLineItem, "Select$" + e.Row.RowIndex.ToString(), false);
		}
	}

	//protected void gvGeneralServicesLineItem_Sorting(object sender, GridViewSortEventArgs se)
	//{
	//	gvGeneralServicesLineItem.SelectedIndex = -1;
	//	gvGeneralServicesLineItem.EditIndex = -1;

	//	ViewState["SORTEXPRESSION"] = se.SortExpression;

	//	if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
	//		ViewState["SORTDIRECTION"] = 1;
	//	else
	//		ViewState["SORTDIRECTION"] = 0;

	//	BindData();
	//}

	//protected void gvGeneralServicesLineItem_RowUpdating(object sender, GridViewUpdateEventArgs e)
	//{
	//	try
	//	{
	//		GridView _gridView = (GridView)sender;
	//		int nCurrentRow = e.RowIndex;
	//		if (!IsValidRepairJobDetail(ViewState["GENERALSERVICEID"].ToString(),
	//					((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtDetailEdit")).Text))
	//		{
	//			ucError.Visible = true;
	//			return;
	//		}

	//		PhoenixDryDockJobGeneral.UpdateDryDockJobGeneralDetail(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
	//			 new Guid(((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lbljobdetailidEdit")).Text),
	//			 new Guid(ViewState["GENERALSERVICEID"].ToString()),
	//			 ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtDetailEdit")).Text.Trim(),
	//			General.GetNullableInteger(((UserControlUnit)_gridView.Rows[nCurrentRow].FindControl("ucUnitEdit")).SelectedUnit));



	//		_gridView.EditIndex = -1;
	//		BindData();

	//	}
	//	catch (Exception ex)
	//	{
	//		ucError.ErrorMessage = ex.Message;
	//		ucError.Visible = true;
	//	}
	//}

	//protected void gvGeneralServicesLineItem_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
	//{
	//	gvGeneralServicesLineItem.SelectedIndex = e.NewSelectedIndex;
	//}

	protected void gvGeneralServicesLineItem_RowEditing(object sender, GridViewEditEventArgs de)
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

	//protected void gvGeneralServicesLineItem_RowCommand(object sender, GridViewCommandEventArgs e)
	//{
	//	try
	//	{
	//		if (e.CommandName.ToUpper().Equals("SORT"))
	//			return;

	//		GridView _gridView = (GridView)sender;
	//		int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

	//		if (e.CommandName.ToUpper().Equals("ADD"))
	//		{
	//			if (!IsValidRepairJobDetail(ViewState["GENERALSERVICEID"].ToString(),
	//				((RadTextBox)_gridView.FooterRow.FindControl("txtDetailAdd")).Text))
	//			{
	//				ucError.Visible = true;
	//				return;
	//			}
	//			Guid? jobdetailid = null;
	//			jobdetailid = PhoenixDryDockJobGeneral.InsertDryDockJobGeneralDetail(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
	//			 new Guid(ViewState["GENERALSERVICEID"].ToString()),
	//			 ((RadTextBox)_gridView.FooterRow.FindControl("txtDetailAdd")).Text.Trim(),
	//			General.GetNullableInteger(((UserControlUnit)_gridView.FooterRow.FindControl("ucUnitAdd")).SelectedUnit),
	//			ref jobdetailid);

	//			BindData();

	//		}

 //           if (e.CommandName.ToUpper().Equals("MOVEUP"))
 //           {

 //               PhoenixDryDockJobGeneral.MoveDryDockJobGeneralDetailUpOrDown(PhoenixSecurityContext.CurrentSecurityContext.UserCode
 //                   , new Guid(ViewState["GENERALSERVICEID"].ToString())
 //                   , General.GetNullableGuid(((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblDTKey")).Text), 1 // UP
 //                   );

 //               BindData();
 //           }

 //           if (e.CommandName.ToUpper().Equals("MOVEDOWN"))
 //           {
 //               PhoenixDryDockJobGeneral.MoveDryDockJobGeneralDetailUpOrDown(PhoenixSecurityContext.CurrentSecurityContext.UserCode
 //                   , new Guid(ViewState["GENERALSERVICEID"].ToString())
 //                   , General.GetNullableGuid(((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblDTKey")).Text), -1 // DOWN
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

	//protected void gvGeneralServicesLineItem_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
 //               db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
	//		}
	//		UserControlUnit ucUnit = (UserControlUnit)e.Row.FindControl("ucUnitEdit");
	//		DataRowView drv = (DataRowView)e.Row.DataItem;
	//		if (ucUnit != null) ucUnit.SelectedUnit = drv["FLDUNIT"].ToString();
 //           CheckBox cb = (CheckBox)e.Row.FindControl("chkSelectedYN");
 //           cb.Enabled = SessionUtil.CanAccess(this.ViewState, "SELECTDETAIL");
 //           Button b = (Button)e.Row.FindControl("cmdSelectedYN");        
 //           string jvscript = "";
 //           //if (b != null) jvscript = "javascript:checkSelectedYN('" + b.ClientID + "');";
 //           if (b != null) jvscript = "javascript:selectJobDetail('" + drv["FLDJOBDETAILID"].ToString() + "',this);";
 //           if (cb != null && b != null) { cb.Attributes.Add("onclick", jvscript); cb.Checked = drv["FLDSELECTEDYN"].ToString().Equals("0") ? false : true; }
 //           if (b != null) b.Attributes.Add("style", "visibility:hidden");
	//	}
	//}

	//protected void gvGeneralServicesLineItem_RowDeleting(object sender, GridViewDeleteEventArgs de)
	//{
 //       try
 //       {
 //           GridView _gridView = (GridView)sender;
 //           int nCurrentRow = de.RowIndex;

 //           PhoenixDryDockJobGeneral.DeleteDryDockJobGeneralDetail(new Guid(((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lbljobdetailid")).Text),
 //            new Guid(ViewState["GENERALSERVICEID"].ToString()));

 //           _gridView.EditIndex = -1;
 //           BindData();
 //       }
 //       catch (Exception ex)
 //       {
 //           ucError.ErrorMessage = ex.Message;
 //           ucError.Visible = true;
 //       }

	//}

	protected void gvGeneralServicesLineItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		BindData();
	}

	protected void cmdSearch_Click(object sender, EventArgs e)
	{
		gvGeneralServicesLineItem.Rebind();

	}

	public void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		BindData();
		BindFields(new Guid(ViewState["GENERALSERVICEID"].ToString()));
	}

	private bool IsValidRepairJobDetail(string GENERALSERVICEID, string jobdetail)
	{

		ucError.HeaderMessage = "Provide the following required information";
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
            ucError.ErrorMessage = "Cannot make changes while you are in ship. Switch to Office to make changes.";

		if (General.GetNullableGuid(GENERALSERVICEID) == null)
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

    protected void gvGeneralServicesLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvGeneralServicesLineItem_ItemDataBound1(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridFooterItem)
        {
            GridFooterItem footerItem = (GridFooterItem)gvGeneralServicesLineItem.MasterTableView.GetItems(GridItemType.Footer)[0];
            // Button btn = (Button)footerItem.FindControl("Button1");

            LinkButton db = (LinkButton)footerItem.FindControl("cmdAdd");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }

        if (e.Item is GridDataItem)
        {
           

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


            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null)
            {
                cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
            }
            UserControlUnit ucUnit = (UserControlUnit)e.Item.FindControl("ucUnitEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucUnit != null) ucUnit.SelectedUnit = drv["FLDUNIT"].ToString();
            CheckBox cb = (CheckBox)e.Item.FindControl("chkSelectedYN");
            cb.Enabled = SessionUtil.CanAccess(this.ViewState, "SELECTDETAIL");
            Button b = (Button)e.Item.FindControl("cmdSelectedYN");
            string jvscript = "";
            //if (b != null) jvscript = "javascript:checkSelectedYN('" + b.ClientID + "');";
            if (b != null) jvscript = "javascript:selectJobDetail('" + drv["FLDJOBDETAILID"].ToString() + "',this);";
            if (cb != null && b != null) { cb.Attributes.Add("onclick", jvscript); cb.Checked = drv["FLDSELECTEDYN"].ToString().Equals("0") ? false : true; }
            if (b != null) b.Attributes.Add("style", "visibility:hidden");
        }
    }

    protected void gvGeneralServicesLineItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem footerItem = (GridFooterItem)gvGeneralServicesLineItem.MasterTableView.GetItems(GridItemType.Footer)[0];
                if (!IsValidRepairJobDetail(ViewState["GENERALSERVICEID"].ToString(),
                    ((RadTextBox)footerItem.FindControl("txtDetailAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                Guid? jobdetailid = null;
                jobdetailid = PhoenixDryDockJobGeneral.InsertDryDockJobGeneralDetail(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                 new Guid(ViewState["GENERALSERVICEID"].ToString()),
                 ((RadTextBox)footerItem.FindControl("txtDetailAdd")).Text.Trim(),
                General.GetNullableInteger(((UserControlUnit)footerItem.FindControl("ucUnitAdd")).SelectedUnit),
                ref jobdetailid);

                gvGeneralServicesLineItem.Rebind();

            }

            if (e.CommandName.ToUpper().Equals("MOVEUP"))
            {

                PhoenixDryDockJobGeneral.MoveDryDockJobGeneralDetailUpOrDown(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["GENERALSERVICEID"].ToString())
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDTKey")).Text), 1 // UP
                    );

                gvGeneralServicesLineItem.Rebind();
            }

            if (e.CommandName.ToUpper().Equals("MOVEDOWN"))
            {
                PhoenixDryDockJobGeneral.MoveDryDockJobGeneralDetailUpOrDown(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["GENERALSERVICEID"].ToString())
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDTKey")).Text), -1 // DOWN
                    );

                gvGeneralServicesLineItem.Rebind();
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvGeneralServicesLineItem_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
           
            if (!IsValidRepairJobDetail(ViewState["GENERALSERVICEID"].ToString(),
                        ((RadTextBox)e.Item.FindControl("txtDetailEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixDryDockJobGeneral.UpdateDryDockJobGeneralDetail(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                 new Guid(((RadLabel)e.Item.FindControl("lbljobdetailidEdit")).Text),
                 new Guid(ViewState["GENERALSERVICEID"].ToString()),
                 ((RadTextBox)e.Item.FindControl("txtDetailEdit")).Text.Trim(),
                General.GetNullableInteger(((UserControlUnit)e.Item.FindControl("ucUnitEdit")).SelectedUnit));



            //_gridView.EditIndex = -1;
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvGeneralServicesLineItem_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;

            PhoenixDryDockJobGeneral.DeleteDryDockJobGeneralDetail(new Guid(((RadLabel)e.Item.FindControl("lbljobdetailid")).Text),
             new Guid(ViewState["GENERALSERVICEID"].ToString()));

           // _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
