using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersDocumentCategory : PhoenixBasePage
{

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersDocumentCategory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCategory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersDocumentCategory.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");            
            MenuRegistersDocumentCategory.AccessRights = this.ViewState;
            MenuRegistersDocumentCategory.MenuList = toolbar.Show();

			if (!IsPostBack)
			{
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
                gvCategory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            toolbar = new PhoenixToolbar();
            //MenuTitle.AccessRights = this.ViewState;
            //MenuTitle.MenuList = toolbar.Show();
        }
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
    
	protected void ShowExcel()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		DataSet ds = new DataSet();
        string[] alColumns = { "FLDORDERNUMBER", "FLDCATEGORYCODE", "FLDCATEGORYNAME" };
		string[] alCaptions = {"Sort order", "Code", "Name" };
		string sortexpression;
		int? sortdirection = null;

		sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
			iRowCount = 10;
		else
			iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

		ds = PhoenixRegistersDocumentCategory.DocumentCategorySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            ,General.GetNullableString(txtCategoryCode.Text)
            ,General.GetNullableString(txtCategoryName.Text)
			,sortexpression, sortdirection,
			Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvCategory.PageSize,
			ref iRowCount,
			ref iTotalPageCount);

		Response.AddHeader("Content-Disposition", "attachment; filename=DocumentCategory.xls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		Response.Write("<tr>");
		Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3> Document Category </h3></td>");
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

    protected void DocumentCategory_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
			{
				ViewState["PAGENUMBER"] = 1;
				BindData();
                gvCategory.Rebind();
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
		int iRowCount = 0;
		int iTotalPageCount = 0;
        string[] alColumns = { "FLDORDERNUMBER", "FLDCATEGORYCODE", "FLDCATEGORYNAME" };
        string[] alCaptions = { "Sort order", "Code", "Name" };
		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;
		DataSet ds = new DataSet();
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixRegistersDocumentCategory.DocumentCategorySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableString(txtCategoryCode.Text.Trim())
                , General.GetNullableString(txtCategoryName.Text.Trim())
                , sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvCategory.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        General.SetPrintOptions("gvCategory", " Document Category ", alCaptions, alColumns, ds);

		if (ds.Tables[0].Rows.Count > 0)
		{
			gvCategory.DataSource = ds;
            gvCategory.VirtualItemCount = iRowCount;		
		}
		else
		{
            gvCategory.DataSource = "";
        }
	}
	private void InsertDocumentCategory(string categorycode, string categoryname,int? Orderno)
	{

        PhoenixRegistersDocumentCategory.InsertDocumentCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode
        , General.GetNullableString(categorycode)
        , General.GetNullableString(categoryname)
        , Orderno
        );
        ucStatus.Text = "Document  category added";
	}
    private void UpdateDocumentCategory(string categoryid, string categorycode, string categoryname, int? Orderno)
    {

        PhoenixRegistersDocumentCategory.UpdateDocumentCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode
        , int.Parse(categoryid)
        , General.GetNullableString(categorycode)
        , General.GetNullableString(categoryname)
        ,Orderno
        );
        ucStatus.Text = "Document category updated";
    }

    private bool IsValidDocumentCategory(string code,string name)
	{
		ucError.HeaderMessage = "Please provide the following required information";
        
        if(General.GetNullableString(code) == null)		
		    ucError.ErrorMessage = "Category code is required.";

        if (General.GetNullableString(name) == null)
            ucError.ErrorMessage = "Category name is required.";

		return (!ucError.IsError);
	}

    private void DeleteDocumentCategory(int categoryid)
	{
        PhoenixRegistersDocumentCategory.DeleteDocumentCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,categoryid);
	}


	public StateBag ReturnViewState()
	{
		return ViewState;
	}

    protected void gvCategory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;           
           else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidDocumentCategory(((RadTextBox)e.Item.FindControl("txtCategoryCodeAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtCategoryNameAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertDocumentCategory(
                    ((RadTextBox)e.Item.FindControl("txtCategoryCodeAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtCategoryNameAdd")).Text,
                    General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtortorderAdd")).Text.Trim())
                );
                BindData();
                gvCategory.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidDocumentCategory(((RadTextBox)e.Item.FindControl("txtCategoryCodeEdit")).Text
                    , ((RadTextBox)e.Item.FindControl("txtCategoryNameEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateDocumentCategory(
                     ((RadLabel)e.Item.FindControl("lblCategoryIdEdit")).Text,
                     ((RadTextBox)e.Item.FindControl("txtCategoryCodeEdit")).Text.Trim(),
                     ((RadTextBox)e.Item.FindControl("txtCategoryNameEdit")).Text.Trim(),
                     General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtsortorderEdit")).Text.Trim())
                 );               
                BindData();
                gvCategory.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteDocumentCategory(Int32.Parse(((RadLabel)e.Item.FindControl("lblCategoryId")).Text));
                BindData();
                gvCategory.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidDocumentCategory(((RadTextBox)e.Item.FindControl("txtCategoryCodeEdit")).Text
                 , ((RadTextBox)e.Item.FindControl("txtCategoryNameEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateDocumentCategory(
                        ((RadLabel)e.Item.FindControl("lblCategoryIdEdit")).Text,
                        ((RadTextBox)e.Item.FindControl("txtCategoryCodeEdit")).Text,
                        ((RadTextBox)e.Item.FindControl("txtCategoryNameEdit")).Text,
                        General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtsortorderEdit")).Text.Trim())
                    );
                BindData();
                gvCategory.Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCategory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCategory.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvCategory_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    protected void gvCategory_SortCommand(object sender, GridSortCommandEventArgs e)
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
}
