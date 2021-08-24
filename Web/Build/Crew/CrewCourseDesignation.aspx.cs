using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;

public partial class CrewCourseDesignation : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewCourseDesignation.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDesignation')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Crew/CrewCourseDesignation.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");            
			MenuCrewCourseDesignation.AccessRights = this.ViewState;
			MenuCrewCourseDesignation.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();
            if (!IsPostBack)
			{
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
                gvDesignation.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
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
		string[] alColumns = { "FLDDESIGNATIONCODE", "FLDDESIGNATIONNAME" };
		string[] alCaptions = { "Code", "Name" };
		string sortexpression;
		int? sortdirection = null;

		sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
			iRowCount = 10;
		else
			iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

		ds = PhoenixCrewCourseDesignation.DesignationSearch(txtCode.Text, txtName.Text, sortexpression, sortdirection,
			Int32.Parse(ViewState["PAGENUMBER"].ToString()),
			gvDesignation.PageSize,
			ref iRowCount,
			ref iTotalPageCount);
		Response.AddHeader("Content-Disposition", "attachment; filename=Designation.xls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		Response.Write("<tr>");
		Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
		Response.Write("<td><h3> CourseDesignation Register</h3></td>");
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

	protected void CrewCourseDesignation_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
			{				
				ViewState["PAGENUMBER"] = 1;
				BindData();
                gvDesignation.Rebind();
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
		string[] alColumns = { "FLDDESIGNATIONCODE", "FLDDESIGNATIONNAME" };
		string[] alCaptions = { "Code", "Name" };
		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;
		DataSet ds = new DataSet();
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		ds = PhoenixCrewCourseDesignation.DesignationSearch(txtCode.Text, txtName.Text, sortexpression, sortdirection,
			Int32.Parse(ViewState["PAGENUMBER"].ToString()),
			gvDesignation.PageSize,
			ref iRowCount,
			ref iTotalPageCount);

		General.SetPrintOptions("gvDesignation", "CourseDesignation Register", alCaptions, alColumns, ds);

		if (ds.Tables[0].Rows.Count > 0)
		{
			gvDesignation.DataSource = ds;
            gvDesignation.VirtualItemCount = iRowCount;
		}
		else
		{
            gvDesignation.DataSource = "";
        }
	}
    
	protected void cmdSearch_Click(object sender, EventArgs e)
	{		
		ViewState["PAGENUMBER"] = 1;
		BindData();
        gvDesignation.Rebind();
	}
    
	private void InsertDesignation(string code, string name)
	{
		PhoenixCrewCourseDesignation.InsertDesignation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
			 code, name);
	}

	private void UpdateDesignation(int designationid, string code, string name)
	{
		if (!IsValidDesignation(code, name))
		{
			ucError.Visible = true;
			return;
		}
		PhoenixCrewCourseDesignation.UpdateDesignation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
			 designationid, code, name);
		ucStatus.Text = "Information updated";
	}

	private bool IsValidDesignation(string code, string name)
	{
		ucError.HeaderMessage = "Please provide the following required information";
        
		if (code.Trim().Equals(""))
			ucError.ErrorMessage = "Code is required.";

		if (name.Trim().Equals(""))
			ucError.ErrorMessage = "Name is required.";

		return (!ucError.IsError);
	}

	private void DeleteDesignation(int Designationcode)
	{
		PhoenixCrewCourseDesignation.DeleteDesignation(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Designationcode);
	}

	public StateBag ReturnViewState()
	{
		return ViewState;
	}

    protected void gvDesignation_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidDesignation(((RadTextBox)e.Item.FindControl("txtCodeAdd")).Text,
                                        ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertDesignation(
                    ((RadTextBox)e.Item.FindControl("txtCodeAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text
                );
                BindData();
                gvDesignation.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidDesignation(((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text,
                                    ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateDesignation(
                    Int32.Parse(((RadLabel)e.Item.FindControl("lblDesignationIdEdit")).Text),
                     ((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text,
                     ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text
                 );
                BindData();
                gvDesignation.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteDesignation(Int32.Parse(((RadLabel)e.Item.FindControl("lblDesignationId")).Text));
                BindData();
                gvDesignation.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                UpdateDesignation(
                    Int32.Parse(((RadLabel)e.Item.FindControl("lblDesignationIdEdit")).Text),
                     ((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text,
                     ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text
                 );
                BindData();
                gvDesignation.Rebind();
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

    protected void gvDesignation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDesignation.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvDesignation_ItemDataBound(object sender, GridItemEventArgs e)
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

    protected void gvDesignation_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
}
