using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class InspectionMOCRegisterDesignation : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
            SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbar = new PhoenixToolbar();
            
            toolbar.AddFontAwesomeButton("../Inspection/InspectionMOCRegisterDesignation.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMOCDesignation')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionMOCRegisterDesignation.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionMOCRegisterDesignation.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuRegistersDesignation.AccessRights = this.ViewState;
			MenuRegistersDesignation.MenuList = toolbar.Show();
            
			if (!IsPostBack)
			{
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
                BindMOCApprovalRole();
                ddlApproverRole.SelectedIndex = 1;
            }
			//BindData();
            //BindMOCDesignationData();
		    	
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}


	protected void ShowExcel()
	{
		DataSet ds = new DataSet();
		string[] alColumns = { "FLDDESIGNATIONCODE", "FLDDESIGNATIONNAME" };
		string[] alCaptions = { "Code", "Name" };

        ds = PhoenixInspectionMOCApproverRole.MOCDesignationRoleMappingList(txtCode.Text, txtName.Text, General.GetNullableGuid(ddlApproverRole.SelectedValue));

		Response.AddHeader("Content-Disposition", "attachment; filename=UserRoleDesignation.xls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		Response.Write("<tr>");
		Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
		Response.Write("<td><h3> User Role Designation</h3></td>");
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

	protected void RegistersDesignation_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
			if (CommandName.ToUpper().Equals("FIND"))
			{
			
				ViewState["PAGENUMBER"] = 1;
				BindData();
                BindMOCDesignationData();
                gvMOCDesignation.Rebind();
			}
			else if (CommandName.ToUpper().Equals("EXCEL"))
			{
				ShowExcel();
			}
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                txtCode.Text = "";
                txtName.Text = "";
                BindData();
                BindMOCDesignationData();
                gvDesignation.Rebind();
                gvMOCDesignation.Rebind();
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

        ds = PhoenixInspectionMOCApproverRole.MOCDesignationSearch(txtCode.Text, txtName.Text, General.GetNullableGuid(ddlApproverRole.SelectedValue), sortexpression, sortdirection,
			Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvDesignation.PageSize,
			ref iRowCount,
			ref iTotalPageCount);	

		General.SetPrintOptions("gvDesignation", "User Role Designation", alCaptions, alColumns, ds);
        gvDesignation.DataSource = ds;
        gvDesignation.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
		ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
	}
    private void BindMOCDesignationData()
    {

        string[] alColumns = { "FLDDESIGNATIONCODE", "FLDDESIGNATIONNAME" };
        string[] alCaptions = { "Code", "Name" };

        DataSet ds = new DataSet();

        ds = PhoenixInspectionMOCApproverRole.MOCDesignationRoleMappingList(txtCode.Text, txtName.Text, General.GetNullableGuid(ddlApproverRole.SelectedValue));

        General.SetPrintOptions("gvMOCDesignation", "User Role Designation", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvMOCDesignation.DataSource = ds;
        }
    }

    protected void gvMOCDesignation_Sorting(object sender, GridSortCommandEventArgs se)
    {
     
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindMOCDesignationData();
    }
    protected void gvMOCDesignation_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DESELECT"))
            {
                PhoenixInspectionMOCApproverRole.MOCApproverDesignationRoleDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , int.Parse(((RadLabel)e.Item.FindControl("lblDesignationId")).Text)
                                                                        , new Guid(ddlApproverRole.SelectedValue));
                BindData();
                BindMOCDesignationData();
                gvMOCDesignation.Rebind();
                gvDesignation.Rebind();
            }
            else if(e.CommandName.ToUpper().Equals("EDIT"))
                ((RadTextBox)e.Item.FindControl("txtNameEdit")).Focus();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
  
    protected void gvMOCDesignation_ItemDataBound(Object sender, GridItemEventArgs e)
    {  
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }

            LinkButton cmdMap = (LinkButton)e.Item.FindControl("cmdInspectionMapping");

                if (cmdMap != null)
                {
                    cmdMap.Visible = SessionUtil.CanAccess(this.ViewState, cmdMap.CommandName);
                    cmdMap.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionMOCApproverRoleMapping.aspx?Designationid=" + drv["FLDDESIGNATIONID"].ToString() + "');return false;");
                
             
            }

        }
        if (e.Item is GridFooterItem )
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
    
	protected void gvDesignation_Sorting(object sender, GridSortCommandEventArgs se)
	{

		ViewState["SORTEXPRESSION"] = se.SortExpression;

		if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
			ViewState["SORTDIRECTION"] = 1;
		else
			ViewState["SORTDIRECTION"] = 0;

		BindData();
	}
	protected void gvDesignation_ItemCommand(object sender, GridCommandEventArgs e)
	{
		try
		{
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                PhoenixInspectionMOCApproverRole.MOCApproverDesignationRoleInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , int.Parse(((RadLabel)e.Item.FindControl("lblDesignationId")).Text)
                                                                        , new Guid(ddlApproverRole.SelectedValue));
                BindData();
                BindMOCDesignationData();
                gvDesignation.Rebind();
                gvMOCDesignation.Rebind();
            }
     
            else if (e.CommandName.ToUpper().Equals("PAGE"))
                ViewState["PAGENUMBER"] = null;
        }

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	

    protected void gvDesignation_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }

            LinkButton cmdMap = (LinkButton)e.Item.FindControl("cmdInspectionMapping");

                if (cmdMap != null)
                {
                    cmdMap.Visible = SessionUtil.CanAccess(this.ViewState, cmdMap.CommandName);
                    cmdMap.Attributes.Add("onclick", "OpenNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionMOCApproverRoleMapping.aspx?Designationid=" + drv["FLDDESIGNATIONID"].ToString() + "');return false;");

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
        }
    }

	protected void cmdSearch_Click(object sender, EventArgs e)
	{
	
		ViewState["PAGENUMBER"] = 1;
		BindData();
        BindMOCDesignationData();
	
	}
    protected void BindMOCApprovalRole()
    {
        ddlApproverRole.DataSource = PhoenixInspectionMOCApproverRole.MOCApproverRoleList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        ddlApproverRole.DataTextField = "FLDMOCAPPROVERROLE";
        ddlApproverRole.DataValueField = "FLDMOCAPPROVERROLEID";
        ddlApproverRole.DataBind();
      
    }
    protected void ddlApproverRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
        BindMOCDesignationData();
    }

    protected void gvDesignation_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDesignation.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMOCDesignation_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindMOCDesignationData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
