using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.OwnerBudget;
using Telerik.Web.UI;


public partial class OwnerBudgetRankOwnerScaleMapping : PhoenixBasePage
{
	 //protected override void Render(HtmlTextWriter writer)
	 //{
		// foreach (GridViewRow r in gvQuick.Rows)
		// {
		//	 if (r.RowType == DataControlRowType.DataRow)
		//	 {
		//		 Page.ClientScript.RegisterForEventValidation(gvQuick.UniqueID, "Edit$" + r.RowIndex.ToString());
		//	 }
		// }

		// base.Render(writer);
	 //}

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            //toolbar.AddImageButton("../OwnerBudget/OwnerBudgetRankOwnerScaleMapping.aspx", "Export to Excel", "icon_xls.png", "Excel");
            //toolbar.AddImageLink("javascript:CallPrint('gvQuick')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddFontAwesomeButton("../OwnerBudget/OwnerBudgetRankOwnerScaleMapping.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvQuick')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersQuick.AccessRights = this.ViewState;
            MenuRegistersQuick.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                if (Request.QueryString["proposalid"] != null)
                {
                    ViewState["PROPOSALID"] = Request.QueryString["proposalid"].ToString();
                }
            }
            
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        //int iRowCount = 0;
        //int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alCaptions = { "Group Rank", "Owner Scale"};
        string[] alColumns = { "FLDGROUPRANK", "FLDOWNERSCALE" };
		
        ds = PhoenixOwnerBudgetRegisters.ProposalManningScaleList(new Guid (ViewState["PROPOSALID"].ToString())
                                                ,General.GetNullableInteger(ddlLevel.SelectedValue));

            Response.AddHeader("Content-Disposition", "attachment; filename=\"OwnerScale.xls\"");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Owner Scale</h3></td>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");

            Response.Write("</tr>");
            Response.Write("</TABLE>");
            Response.Write("<br />");
            Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
            Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();   
  
    }

    protected void RegistersQuick_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                //gvQuick.EditIndex = -1;
     //           gvQuick.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
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
        string[] alCaptions = { "Group Rank", "Owner Scale" };
        string[] alColumns = { "FLDGROUPRANK", "FLDOWNERSCALE" };
        
        DataSet ds = new DataSet();
        ds = PhoenixOwnerBudgetRegisters.ProposalManningScaleList(new Guid(ViewState["PROPOSALID"].ToString())
                                                , General.GetNullableInteger(ddlLevel.SelectedValue));

            General.SetPrintOptions("gvQuick", "Owner Scale", alCaptions, alColumns, ds);

        //if (ds.Tables[0].Rows.Count > 0)
        //{

            gvQuick.DataSource = ds;
            gvQuick.DataBind();
        //}
        //else
        //{

        //    DataTable dt = ds.Tables[0];
        //    ShowNoRecordsFound(dt, gvQuick);
        //}

    }


    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindData();
        
    }

    //protected void gvQuick_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    BindData();
    //}

    protected void gvQuick_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvQuick_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
             
            }
           
            
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvQuick_DeleteCommand(object sender, GridCommandEventArgs de)
    {
        BindData();
        
    }
    protected void gvQuick_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
			e.Row.TabIndex = -1;
			e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvQuick, "Edit$" + e.Row.RowIndex.ToString(), false);
		}
    }
    //protected void gvQuick_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;

    //        _gridView.EditIndex = de.NewEditIndex;
    //        _gridView.SelectedIndex = de.NewEditIndex;
    //        BindData();
            
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    protected void gvQuick_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = e.RowIndex;

            if (!IsValidQuick(((UserControlMaskNumber)e.Item.FindControl("ucOwnerScale")).Text
                                    ))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixOwnerBudgetRegisters.ProposalManningScaleUpdate(
                                                new Guid(((RadLabel)e.Item.FindControl("lblManningScaleId")).Text.ToString())
                                                , int.Parse(((UserControlMaskNumber)e.Item.FindControl("ucOwnerScale")).Text)
                                                );
            //_gridView.EditIndex = -1;
            BindData();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null,true);", true);
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }
    protected void gvQuick_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null && del.Visible == true) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

  
        //if (e.Row.RowType == DataControlRowType.Header)
        if(e.Item is GridHeaderItem)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Item.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
		//if (e.Row.RowType == DataControlRowType.Header)
        if(e.Item is GridHeaderItem)
		{
			
		}

        //if (e.Row.RowType == DataControlRowType.DataRow)
        if(e.Item is GridEditableItem)
        {
			//if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
			{
				LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
				if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
			
			}
            
        }
        //if (e.Row.RowType == DataControlRowType.Footer)
        //if(e.Item is GridFooterItem)
        //{
        //    ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
        //    if (db != null)
        //    {
        //        if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
        //            db.Visible = false;
        //    }
            
        //}
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
       	//gvQuick.SelectedIndex = -1;
//		ViewState["PAGENUMBER"] = 1;
		BindData();
		
    }

    
    private bool IsValidQuick(string ownerscale)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvQuick;

        if (General.GetNullableInteger(ownerscale) == null)
            ucError.ErrorMessage = "Owner Scale is required.";

        return (!ucError.IsError);
    }

    //private void ShowNoRecordsFound(DataTable dt, GridView gv)
    //{
    //    dt.Rows.Add(dt.NewRow());
    //    gv.DataSource = dt;
    //    gv.DataBind();

    //    int colcount = gv.Columns.Count;
    //    gv.Rows[0].Cells.Clear();
    //    gv.Rows[0].Cells.Add(new TableCell());
    //    gv.Rows[0].Cells[0].ColumnSpan = colcount;
    //    gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    //    gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
    //    gv.Rows[0].Cells[0].Font.Bold = true;
    //    gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    //    gv.Rows[0].Attributes["onclick"] = "";
    //}

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
