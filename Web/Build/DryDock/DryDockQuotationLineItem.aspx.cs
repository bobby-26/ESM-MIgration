using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.DryDock;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class DryDockQuotationLineItem : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			cmdHiddenSubmit.Attributes.Add("style", "display:none");
			SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Jobs", "DETAIL", ToolBarDirection.Right);
            toolbarmain.AddButton("Quotation", "LIST",ToolBarDirection.Right);
          
            MenuQuotationLineItem.AccessRights = this.ViewState;
            MenuQuotationLineItem.MenuList = toolbarmain.Show();
            MenuQuotationLineItem.SelectedMenuIndex = 0;
			if (!IsPostBack)
			{

                ViewState["VSLID"] = Request.QueryString["vslid"];

				ViewState["PAGENUMBER"] = 1;
                ViewState["CURRENTINDEX"] = 1;
				ViewState["QuotationID"] = null;
				if (Request.QueryString["Quotationid"] != null)
				{
					ViewState["QuotationID"] = Request.QueryString["Quotationid"].ToString();

				}	
                gvQuotationLineItem.PageSize= int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

	protected void QuotationLineItem_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("LIST"))
			{
				if (ViewState["QuotationID"] == null)
				{
					ShowError();
					return;
				}
				Response.Redirect("../DryDock/DryDockQuotation.aspx?vslid=" + Request.QueryString["vslid"] + "&Quotationid=" + ViewState["QuotationID"].ToString(), false);
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
		ucError.ErrorMessage = "Please select Quotation.";
		ucError.Visible = true;

	}
	protected void gvQuotationLineItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		BindData();
	}
	//protected void gvQuotationLineItem_RowUpdating(object sender, GridViewUpdateEventArgs e)
	//{
	//	try
	//	{
	//		GridView _gridView = (GridView)sender;
	//		int nCurrentRow = e.RowIndex;


 //           PhoenixDryDockOrder.UpdateDryDockQuotationLine(int.Parse(ViewState["VSLID"].ToString()),
 //                new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblOrderidEdit")).Text),
 //                new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblQuotationIdEdit")).Text),
 //                new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblQuotationLineIdEdit")).Text),
 //                General.GetNullableDecimal(((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtUnitPriceEdit")).Text),
 //                General.GetNullableDecimal(((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtOrderQuantityEdit")).Text),                                  
 //                General.GetNullableDecimal(((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtDiscountEdit")).Text),
 //                General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtRemarks")).Text));

	//		_gridView.EditIndex = -1;
	//		BindData();

	//	}
	//	catch (Exception ex)
	//	{
	//		ucError.ErrorMessage = ex.Message;
	//		ucError.Visible = true;
	//	}
	//}

	protected void gvQuotationLineItem_RowDataBound(object sender, GridViewCommandEventArgs e)
	{

		if (e.CommandName.ToString().ToUpper() == "SORT")
			return;

		GridView _gridView = (GridView)sender;
		int nCurrentRow = int.Parse(e.CommandArgument.ToString());

		if (e.CommandName.ToString().ToUpper() == "SELECT" )
		{
			
		}
	}

	protected void ShowExcel()
	{
		try
		{
			int iRowCount = 0;
			int iTotalPageCount = 0;

			string[] alColumns = { "FLDNUMBER", "FLDTITLE", "FLDJOBTYPE", "FLDJOBDESCRIPTION" };
			string[] alCaptions = { "Number", "Title", "Job Type", "Job Description" };
			
		
			if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
				iRowCount = 10;
			else
				iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

			
			DataSet ds;

            ds = PhoenixDryDockOrder.ListDryDockQuotationLineItem
				(int.Parse(ViewState["VSLID"].ToString()),
				new Guid(ViewState["QuotationID"].ToString()), 
				 1,
				  iRowCount, ref iRowCount, ref iTotalPageCount);

			General.ShowExcel("Repair Job", ds.Tables[0], alColumns, alCaptions, null, null);
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
			int iTotalPageCount = 0;

			string[] alColumns = { "FLDNUMBER", "FLDTITLE", "FLDJOBTYPE", "FLDJOBDESCRIPTION" };
			string[] alCaptions = { "Number", "Title", "Job Type", "Job Description" };

           // ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>FreezeGridViewHeader('gvQuotationLineItem', 300, false,'cmdSave');</script>", false);
			DataSet ds = PhoenixDryDockOrder.ListDryDockQuotationLineItem
				(int.Parse(ViewState["VSLID"].ToString()),
				new Guid(ViewState["QuotationID"].ToString()),
				 gvQuotationLineItem.CurrentPageIndex+1,
				 gvQuotationLineItem.PageSize, ref iRowCount, ref iTotalPageCount);

			General.SetPrintOptions("gvQuotationLineItem", "Dry Dock Quotation", alCaptions, alColumns, ds);

            gvQuotationLineItem.DataSource = ds;
            gvQuotationLineItem.VirtualItemCount = iRowCount;
          
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

	protected void gvQuotationLineItem_RowEditing(object sender, GridViewEditEventArgs de)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = de.NewEditIndex;

		BindData();
	}

	protected void gvQuotationLineItem_Sorting(object sender, GridViewSortEventArgs se)
	{
		ViewState["SORTEXPRESSION"] = se.SortExpression;

		if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
			ViewState["SORTDIRECTION"] = 1;
		else
			ViewState["SORTDIRECTION"] = 0;

		BindData();
	}

	//protected void gvQuotationLineItem_SelectIndexChanging(object sender, GridViewSelectEventArgs se)
	//{
	//	gvQuotationLineItem.SelectedIndex = se.NewSelectedIndex;
	//	BindData();
	//}

	//protected void gvQuotationLineItem_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
	//{
 //       DataRowView drv = (DataRowView)e.Row.DataItem;

 //       if (e.Row.RowType == DataControlRowType.DataRow)
 //       {
 //           ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
 //           if (db != null)
 //           {
 //               db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
 //               db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
 //               if (drv["FLDJOBDETAILID"].ToString() != string.Empty)
 //                   db.Visible = false;
 //           }
 //           Label Componentcount = (Label)e.Row.FindControl("lblComponent");
 //           if (drv["FLDCOMPONENTCOUNT"].ToString() == "0")
 //           {
 //               Componentcount.Text = null;
 //           }

 //           if (drv["FLDJOBDETAILID"].ToString().Equals(""))
 //           {
 //               e.Row.CssClass = "datagrid_footerstyle";
 //           }

 //           UserControlUnit ucUnit = (UserControlUnit)e.Row.FindControl("ucUnitEdit");
 //           if (ucUnit != null) ucUnit.SelectedUnit = drv["FLDUNIT"].ToString();
 //       }
	//}
    //protected void gvQuotationLineItem_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;

    //        PhoenixDryDockQuotation.DeleteDryDockQuotationLineItem(int.Parse(ViewState["VSLID"].ToString()),                
    //             new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblQuotationId")).Text),
    //              new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblOrderid")).Text),
    //             new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblJobId")).Text)
    //            );

    //        _gridView.EditIndex = -1;
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
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

	private void ShowNoRecordsFound(DataTable dt, GridView gv)
	{
		try
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
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		try
		{
			BindData();
			//SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	//private void SetRowSelection()
	//{
 //       foreach (GridDataItem item in gvQuotationLineItem.MasterTableView.Items)
 //       {
 //           if (gvQuotationLineItem.MasterTableView.Items[0].GetDataKeyValue("JOBID").ToString().Equals(ViewState[""].ToString()))
	//		{
				
	//			ViewState["DTKEY"] = ((Label)item.FindControl("lbldtkey")).Text;

	//		}
	//	}
	//}



    protected void gvQuotationLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvQuotationLineItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if(e.CommandName.ToUpper().Equals("DELETE"))
        {
            PhoenixDryDockQuotation.DeleteDryDockQuotationLineItem(int.Parse(ViewState["VSLID"].ToString()),
               new Guid(((RadLabel)e.Item.FindControl("lblQuotationId")).Text),
                new Guid(((RadLabel)e.Item.FindControl("lblOrderid")).Text),
               new Guid(((RadLabel)e.Item.FindControl("lblJobId")).Text)
              );


            //  BindData();
            gvQuotationLineItem.Rebind();
        }
    }

    protected void gvQuotationLineItem_ItemDataBound1(object sender, GridItemEventArgs e)
    {
      

        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                if (DataBinder.Eval(e.Item.DataItem, "FLDJOBDETAILID").ToString() != string.Empty)
                    db.Visible = false;
            }
            RadLabel Componentcount = (RadLabel)e.Item.FindControl("lblComponent");
            if (DataBinder.Eval(e.Item.DataItem, "FLDCOMPONENTCOUNT").ToString() == "0")
            {
                Componentcount.Text = null;
            }

          

            UserControlUnit ucUnit = (UserControlUnit)e.Item.FindControl("ucUnitEdit");
            if (ucUnit != null) ucUnit.SelectedUnit = DataBinder.Eval(e.Item.DataItem, "FLDUNIT").ToString();

           
        }

       
    }

    protected void gvQuotationLineItem_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
           

            PhoenixDryDockOrder.UpdateDryDockQuotationLine(int.Parse(ViewState["VSLID"].ToString()),
                 new Guid(((RadLabel)e.Item.FindControl("lblOrderidEdit")).Text),
                 new Guid(((RadLabel)e.Item.FindControl("lblQuotationIdEdit")).Text),
                 new Guid(((RadLabel)e.Item.FindControl("lblQuotationLineIdEdit")).Text),
                 General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("txtUnitPriceEdit")).Text),
                 General.GetNullableDecimal(((UserControlNumber)e.Item.FindControl("txtOrderQuantityEdit")).Text),
                 General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("txtDiscountEdit")).Text),
                 General.GetNullableString(((RadTextBox)e.Item.FindControl("txtRemarks")).Text)
                );



            // BindData();
            gvQuotationLineItem.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   

}
