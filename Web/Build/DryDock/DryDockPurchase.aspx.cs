using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DryDock;
using Telerik.Web.UI;


public partial class DryDockPurchase : PhoenixBasePage
{


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbarmain.AddButton("Project", "PROJECT",ToolBarDirection.Right);
            
            MenuStockItem.MenuList = toolbarmain.Show();
            MenuStockItem.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["VSLID"] = Request.QueryString["vslid"].ToString();
                ViewState["PROJECTID"] = Request.QueryString["projectid"].ToString();
            }
            //BindData();
            //BindDataFormItem();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        
    }

 	protected void MenuStockItem_TabStripCommand(object sender, EventArgs e)
	{
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;


            if (CommandName.ToUpper().Equals("PROJECT"))
            {
                Response.Redirect("../DryDock/DryDockProject.aspx", false);
            }

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                foreach (GridDataItem gvr in gvPurchaseRequisition.Items)
                {
                    
                    RadDropDownList ddl = (RadDropDownList)gvr.FindControl("ddlCostClassification");
                    RadLabel lbl = (RadLabel)gvr.FindControl("lblOrderId");
                    if (ddl != null && lbl != null && General.GetNullableInteger(ddl.SelectedValue).HasValue)
                    {
                        PhoenixDryDockPurchase.PurchaseRequisitionCostClassification(int.Parse(ViewState["VSLID"].ToString()),
                            General.GetNullableGuid(ViewState["PROJECTID"].ToString()),
                            General.GetNullableGuid(lbl.Text), General.GetNullableInteger(ddl.SelectedValue));
                    }
                }
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
			int iRowCount = 0;
			int iTotalPageCount = 10;
			DataTable dt;
            int vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            dt = PhoenixDryDockPurchase.PurchaseRequisitionList(int.Parse(ViewState["VSLID"].ToString()),
                General.GetNullableGuid(ViewState["PROJECTID"].ToString()), 
                "2330",
                General.GetNullableString(ddlStockType.SelectedValue));
            gvPurchaseRequisition.DataSource = dt;
            

			ViewState["ROWCOUNT"] = iRowCount;
			ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

    private void BindDataFormItem()
    {
        try
        {
            DataTable dt;

            dt = PhoenixDryDockPurchase.AdditionalFormlineitemlist(General.GetNullableGuid(ViewState["PROJECTID"].ToString()));
            gvformitem.DataSource = dt;
        
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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

    protected void ddlStockType_TextChanged(object sender, EventArgs e)
    {
        BindData();
    }

	//protected void gvPurchaseRequisition_RowCommand(object sender, GridViewCommandEventArgs e)
	//{
 //       GridView _gridView = (GridView)sender;
 //       int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
 //       try
 //       {
 //           if (e.CommandName.ToUpper().Equals("VIEW"))
 //           {
 //               Label lblStockType = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblStockType"));
 //               Label lblFormNo = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblFormNo"));
 //               NameValueCollection criteria = new NameValueCollection();
 //               criteria.Clear();
 //               criteria.Add("ucVessel", ViewState["VSLID"].ToString());
 //               criteria.Add("ddlStockType", lblStockType.Text);
 //               criteria.Add("txtNumber", lblFormNo.Text);
 //               criteria.Add("txtTitle", "");
 //               criteria.Add("txtVendorid", "");
 //               criteria.Add("txtDeliveryLocationId", "");
 //               criteria.Add("txtBudgetId", "");
 //               criteria.Add("txtBudgetgroupId", "");
 //               criteria.Add("ucFinacialYear", "");
 //               criteria.Add("ucFormState", "");
 //               criteria.Add("ucApproval", "");
 //               criteria.Add("UCrecieptCondition", "");
 //               criteria.Add("UCPeority", "");
 //               criteria.Add("ucFormStatus", "");
 //               criteria.Add("ucFormType", "");
 //               criteria.Add("ucComponentclass", "");
 //               criteria.Add("txtMakerReference", "");
 //               criteria.Add("txtOrderedDate", "");
 //               criteria.Add("txtOrderedToDate", "");
 //               criteria.Add("txtCreatedDate", "");
 //               criteria.Add("txtCreatedToDate", "");
 //               criteria.Add("txtApprovedDate", "");
 //               criteria.Add("txtApprovedToDate", "");
 //               Filter.CurrentOrderFormFilterCriteria = criteria;
 //               ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PurchaseFormView", "javascript:Openpopup('chml','','../Purchase/PurchaseForm.aspx?launchedfrom=DRYDOCKING');", true);
 //           }
 //       }
 //       catch (Exception ex)
 //       {
 //           ucError.ErrorMessage = ex.Message;
 //           ucError.Visible = true;
 //       }

	//}

	protected void gvPurchaseRequisition_RowEditing(object sender, GridViewEditEventArgs e)
	{
	}

    //protected void gvPurchaseRequisition_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        DataRowView drv = (DataRowView)e.Row.DataItem;
    //        DataSet ds = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(10);

    //        DropDownList ddl = (DropDownList)e.Row.FindControl("ddlCostClassification");
    //        if (ddl != null)
    //        {
    //            ddl.DataSource = ds;
    //            ddl.DataTextField = "FLDNAME";
    //            ddl.DataValueField = "FLDMULTISPECID";
    //            ddl.DataBind();
    //            ddl.SelectedValue = drv["FLDCOSTCLASSIFICATION"].ToString();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvformitem_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //        if (e.CommandName.ToUpper().Equals("ADD"))
    //        {
    //            if (!IsValidDetail(((TextBox)_gridView.FooterRow.FindControl("txtFormTitleAdd")).Text, ((UserControlDate)_gridView.FooterRow.FindControl("txtOrderedDateAdd")).Text,
    //                                ((UserControlCurrency)_gridView.FooterRow.FindControl("ucCurrency")).SelectedCurrency, ((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtActualCostAdd")).Text,
    //                                ((TextBox)_gridView.FooterRow.FindControl("txtVendorId")).Text))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }
    //            PhoenixDryDockPurchase.AdditionalFormlineitemlistInsert(General.GetNullableGuid(ViewState["PROJECTID"].ToString()), ((TextBox)_gridView.FooterRow.FindControl("txtFormTitleAdd")).Text,
    //                                                                    General.GetNullableDateTime(((UserControlDate)_gridView.FooterRow.FindControl("txtOrderedDateAdd")).Text),
    //                                                                    General.GetNullableInteger(((UserControlCurrency)_gridView.FooterRow.FindControl("ucCurrency")).SelectedCurrency),
    //                                                                    General.GetNullableDecimal(((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtActualCostAdd")).Text),
    //                                                                    General.GetNullableInteger(((TextBox)_gridView.FooterRow.FindControl("txtVendorId")).Text),
    //                                                                    General.GetNullableInteger(((DropDownList)_gridView.FooterRow.FindControl("ddlCostClassification")).SelectedValue));


    //            BindDataFormItem();
    //            ((UserControlCurrency)_gridView.FooterRow.FindControl("ucCurrency")).DataBind();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    private bool IsValidDetail(string formtitle,string ordereddate,string currency,string actualcost,string vendorid)
    {

        ucError.HeaderMessage = "Please provide the following required information";


        if (formtitle.Trim().Equals(""))
            ucError.ErrorMessage = "Title is required.";

        if (ordereddate == null || ordereddate.Trim().Equals(""))
            ucError.ErrorMessage = "Order Date is required";
        else if (General.GetNullableDateTime(ordereddate) == null)
            ucError.ErrorMessage = "Invalid Order Date";

        if (General.GetNullableInteger(currency) == null)
            ucError.ErrorMessage = "Currency is required";

        if (actualcost.Trim().Equals(""))
            ucError.ErrorMessage = "Actual Cost is required";
        else if (General.GetNullableDecimal(actualcost) == null)
            ucError.ErrorMessage = "Invalid Actual Cost";

        if (General.GetNullableInteger(vendorid) == null)
            ucError.ErrorMessage = "Vendor is required";

        return (!ucError.IsError);
    }
   
    protected void gvformitem_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.SelectedIndex = de.NewEditIndex;
            _gridView.EditIndex = de.NewEditIndex;

            BindDataFormItem();
            //SetPageNavigator();
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvformitem_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            DataRowView drv = (DataRowView)e.Row.DataItem;
    //            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
    //            if (del != null)
    //            {
    //                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
    //            }

    //            UserControlCurrency currency = (UserControlCurrency)e.Row.FindControl("ucCurrencyEdit");
    //            if (currency != null)
    //            {
    //                currency.SelectedCurrency = drv["FLDCURRENCYID"].ToString();
    //            }
    //            DataSet ds = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(10);

    //            DropDownList ddl1 = (DropDownList)e.Row.FindControl("ddlCostClassificationEdit");
    //            if (ddl1 != null)
    //            {
    //                ddl1.DataSource = ds;
    //                ddl1.DataTextField = "FLDNAME";
    //                ddl1.DataValueField = "FLDMULTISPECID";
    //                ddl1.DataBind();
    //                ddl1.SelectedValue = drv["FLDCOSTCLASSIFICATION"].ToString();
    //            }

    //            ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
    //            if (edit != null)
    //            {
    //                edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
    //            }

    //            ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
    //            if (save != null)
    //            {
    //                save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);
    //            }

    //            ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
    //            if (cancel != null)
    //            {
    //                cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
    //            }
    //            if (e.Row.RowType == DataControlRowType.Footer)
    //            {
    //                ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
    //                if (db != null)
    //                {
    //                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
    //                        db.Visible = false;

    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}



    //protected void gvformitem_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;

    //        PhoenixDryDockPurchase.AdditionalFormlineitemlistDelete(General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblorderid")).Text),
    //                                                                General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblformid")).Text));

    //        _gridView.EditIndex = -1;
    //        BindDataFormItem();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvformitem_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;

    //        if (!IsValidDetail(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtFormTitleEdit")).Text, ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtOrderedDateEdit")).Text,
    //                                ((UserControlCurrency)_gridView.Rows[nCurrentRow].FindControl("ucCurrencyEdit")).SelectedCurrency, ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtActualCostEdit")).Text,
    //                                ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtVendorIdEdit")).Text))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }

    //        PhoenixDryDockPurchase.AdditionalFormlineitemlistUpdate(General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblorderid")).Text),
    //                                                                General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblformid")).Text),
    //                                                                ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtFormTitleEdit")).Text,
    //                                                                General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtOrderedDateEdit")).Text),
    //                                                                General.GetNullableInteger(((UserControlCurrency)_gridView.Rows[nCurrentRow].FindControl("ucCurrencyEdit")).SelectedCurrency),
    //                                                                General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtActualCostEdit")).Text),
    //                                                                General.GetNullableInteger(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtVendorIdEdit")).Text),
    //                                                                General.GetNullableInteger(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlCostClassificationEdit")).SelectedValue));
    //        _gridView.EditIndex = -1;
    //        BindDataFormItem();
            
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

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
	}



    protected void gvPurchaseRequisition_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
       
        BindData();
    }

	protected void cmdSort_Click(object sender, EventArgs e)
	{
		ImageButton ib = (ImageButton)sender;

		ViewState["SORTEXPRESSION"] = ib.CommandName;
		ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

		BindData();
	}

    protected void gvPurchaseRequisition_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvPurchaseRequisition_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        try
        {
           
            DataSet ds = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(10);

            RadDropDownList ddl = (RadDropDownList)e.Item.FindControl("ddlCostClassification");
            if (ddl != null)
            {
                ddl.DataSource = ds;
                ddl.DataTextField = "FLDNAME";
                ddl.DataValueField = "FLDMULTISPECID";
                ddl.DataBind();
                ddl.SelectedValue = DataBinder.Eval(e.Item.DataItem, "FLDCOSTCLASSIFICATION").ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPurchaseRequisition_ItemCommand(object sender, GridCommandEventArgs e)
    {
       
        try
        {
            if (e.CommandName.ToUpper().Equals("VIEW"))
            {
                RadLabel lblStockType = ((RadLabel)e.Item.FindControl("lblStockType"));
                RadLabel lblFormNo = ((RadLabel)e.Item.FindControl("lblFormNo"));
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("ucVessel", ViewState["VSLID"].ToString());
                criteria.Add("ddlStockType", lblStockType.Text);
                criteria.Add("txtNumber", lblFormNo.Text);
                criteria.Add("txtTitle", "");
                criteria.Add("txtVendorid", "");
                criteria.Add("txtDeliveryLocationId", "");
                criteria.Add("txtBudgetId", "");
                criteria.Add("txtBudgetgroupId", "");
                criteria.Add("ucFinacialYear", "");
                criteria.Add("ucFormState", "");
                criteria.Add("ucApproval", "");
                criteria.Add("UCrecieptCondition", "");
                criteria.Add("UCPeority", "");
                criteria.Add("ucFormStatus", "");
                criteria.Add("ucFormType", "");
                criteria.Add("ucComponentclass", "");
                criteria.Add("txtMakerReference", "");
                criteria.Add("txtOrderedDate", "");
                criteria.Add("txtOrderedToDate", "");
                criteria.Add("txtCreatedDate", "");
                criteria.Add("txtCreatedToDate", "");
                criteria.Add("txtApprovedDate", "");
                criteria.Add("txtApprovedToDate", "");
                Filter.CurrentOrderFormFilterCriteria = criteria;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PurchaseFormView", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Purchase/PurchaseForm.aspx?launchedfrom=DRYDOCKING');", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvformitem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataFormItem();
    }

    protected void gvformitem_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                LinkButton ImgSupplierPickList = (LinkButton)e.Item.FindControl("ImgSupplierPickList");
                if (ImgSupplierPickList != null)
                {
                    ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMakerEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131,132',true);");
                }

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null)
                {
                    del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                }

                UserControlCurrency currency = (UserControlCurrency)e.Item.FindControl("ucCurrencyEdit");
                if (currency != null)
                {
                    currency.SelectedCurrency = DataBinder.Eval(e.Item.DataItem, "FLDCURRENCYID").ToString();// drv[""].ToString();
                }
                DataSet ds = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(10);

                RadDropDownList ddl1 = (RadDropDownList)e.Item.FindControl("ddlCostClassificationEdit");
                if (ddl1 != null)
                {
                    ddl1.DataSource = ds;
                    ddl1.DataTextField = "FLDNAME";
                    ddl1.DataValueField = "FLDMULTISPECID";
                    ddl1.DataBind();
                    ddl1.SelectedValue = DataBinder.Eval(e.Item.DataItem, "FLDCOSTCLASSIFICATION").ToString();// drv["FLDCOSTCLASSIFICATION"].ToString();
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
               
            }
            if (e.Item is GridFooterItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Visible = false;

                }
                //  onclick = "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131,132', true);"
                LinkButton ImgSupplierPickListadd = (LinkButton)e.Item.FindControl("ImgSupplierPickListadd");
                if (ImgSupplierPickListadd != null)
                {
                    ImgSupplierPickListadd.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131,132',true);");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvformitem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
           

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidDetail(((RadTextBox)e.Item.FindControl("txtFormTitleAdd")).Text, ((UserControlDate)e.Item.FindControl("txtOrderedDateAdd")).Text,
                                    ((UserControlCurrency)e.Item.FindControl("ucCurrency")).SelectedCurrency, ((UserControlNumber)e.Item.FindControl("txtActualCostAdd")).Text,
                                    ((RadTextBox)e.Item.FindControl("txtVendorId")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixDryDockPurchase.AdditionalFormlineitemlistInsert(General.GetNullableGuid(ViewState["PROJECTID"].ToString()), ((RadTextBox)e.Item.FindControl("txtFormTitleAdd")).Text,
                                                                        General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtOrderedDateAdd")).Text),
                                                                        General.GetNullableInteger(((UserControlCurrency)e.Item.FindControl("ucCurrency")).SelectedCurrency),
                                                                        General.GetNullableDecimal(((UserControlNumber)e.Item.FindControl("txtActualCostAdd")).Text),
                                                                        General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtVendorId")).Text),
                                                                        General.GetNullableInteger(((RadDropDownList)e.Item.FindControl("ddlCostClassification")).SelectedValue));


                BindDataFormItem();
                gvformitem.Rebind();
                //((UserControlCurrency)e.Item.FindControl("ucCurrency")).DataBind();
            }
            if(e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixDryDockPurchase.AdditionalFormlineitemlistDelete(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblorderid")).Text),
                                                                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblformid")).Text));


                 BindDataFormItem();
                gvformitem.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvformitem_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (!IsValidDetail(((RadTextBox)e.Item.FindControl("txtFormTitleEdit")).Text, ((UserControlDate)e.Item.FindControl("txtOrderedDateEdit")).Text,
                                    ((UserControlCurrency)e.Item.FindControl("ucCurrencyEdit")).SelectedCurrency, ((UserControlNumber)e.Item.FindControl("txtActualCostEdit")).Text,
                                    ((RadTextBox)e.Item.FindControl("txtVendorIdEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixDryDockPurchase.AdditionalFormlineitemlistUpdate(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblorderid")).Text),
                                                                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblformid")).Text),
                                                                    ((RadTextBox)e.Item.FindControl("txtFormTitleEdit")).Text,
                                                                    General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtOrderedDateEdit")).Text),
                                                                    General.GetNullableInteger(((UserControlCurrency)e.Item.FindControl("ucCurrencyEdit")).SelectedCurrency),
                                                                    General.GetNullableDecimal(((UserControlNumber)e.Item.FindControl("txtActualCostEdit")).Text),
                                                                    General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtVendorIdEdit")).Text),
                                                                    General.GetNullableInteger(((RadDropDownList)e.Item.FindControl("ddlCostClassificationEdit")).SelectedValue));
            //_gridView.EditIndex = -1;
           BindDataFormItem();
           
            gvformitem.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

  
}
