using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersTrainingDepartment : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);

			PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersTrainingDepartment.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvTrainingDepartment')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT"); 
			MenuTrainingDepartment.AccessRights = this.ViewState;
			MenuTrainingDepartment.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Department", "DEPARTMENT");
            toolbar.AddButton("Staff", "STAFF");
            MenuTraining.AccessRights = this.ViewState;
            MenuTraining.MenuList = toolbar.Show();
            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();

            if (!IsPostBack)
			{
				ViewState["deptid"] = "";
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["departmentname"] = "";

				if (Request.QueryString["departmentid"] != null)

					ViewState["departmentid"] = Request.QueryString["departmentid"].ToString();
				else
					ViewState["departmentid"] = "";
                gvTrainingDepartment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
			MenuTraining.SelectedMenuIndex = 0;
		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void Training_TabStripCommand(object sender, EventArgs e)
	{
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (ViewState["deptid"].ToString()=="")
			{
				ucError.HeaderMessage = "Navigation Error";
				ucError.ErrorMessage = "Please select a department and navigate to Staff page.";
				ucError.Visible = true;
				return;
			}
		else
		{
			if (CommandName.ToUpper().Equals("STAFF"))
			{
				Response.Redirect("../Registers/RegistersTrainingStaff.aspx?departmentid=" + ViewState["deptid"].ToString(),false);
			}
		}
	}

	protected void ShowExcel()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		DataSet ds = new DataSet();
		string[] alColumns = { "FLDDEPARTMENTNAME" };
		string[] alCaptions = { "Department Name" };
		string sortexpression;
		int? sortdirection = null;

		sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
			iRowCount = 10;
		else
			iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

		ds = PhoenixRegistersTrainingDepartment.TrainingDepartmentSearch(sortexpression, sortdirection,
				(int)ViewState["PAGENUMBER"], gvTrainingDepartment.PageSize, ref iRowCount, ref iTotalPageCount);

		Response.AddHeader("Content-Disposition", "attachment; filename=TrainingDepartment.xls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		Response.Write("<tr>");
		Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
		Response.Write("<td><h3>Training Department</h3></td>");
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


	protected void TrainingDepartment_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

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

		string[] alColumns = { "FLDDEPARTMENTNAME" };
		string[] alCaptions = { "Department Name" };

		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		DataSet ds = new DataSet();

		ds = PhoenixRegistersTrainingDepartment.TrainingDepartmentSearch(sortexpression, sortdirection,
				(int)ViewState["PAGENUMBER"], gvTrainingDepartment.PageSize, ref iRowCount, ref iTotalPageCount);

		General.SetPrintOptions("gvTrainingDepartment", "Training Department", alCaptions, alColumns, ds);

		if (ds.Tables[0].Rows.Count > 0)
		{
			gvTrainingDepartment.DataSource = ds;
            gvTrainingDepartment.VirtualItemCount = iRowCount;
		}
		else
		{
            gvTrainingDepartment.DataSource = "";
        }
	}
	

	protected void cmdSearch_Click(object sender, EventArgs e)
	{		
		BindData();
        gvTrainingDepartment.Rebind();
	}
    
	private bool IsValidDepartment(string departmentid)
	{
		ucError.HeaderMessage = "Please provide the following required information";
        
		if (departmentid.Trim().Equals(""))
			ucError.ErrorMessage = "Select Department";

		return (!ucError.IsError);
	}

    protected void gvTrainingDepartment_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
         
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidDepartment(((RadTextBox)e.Item.FindControl("txtDepartmentIdAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersTrainingDepartment.InsertTrainingDepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                    Convert.ToInt32(((RadTextBox)e.Item.FindControl("txtDepartmentIdAdd")).Text));

                BindData();
                gvTrainingDepartment.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersTrainingDepartment.DeleteTrainingDepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , int.Parse(((RadLabel)e.Item.FindControl("lblTrainingId")).Text));
                BindData();
                gvTrainingDepartment.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                ViewState["deptid"] = ((RadLabel)e.Item.FindControl("lblDepartmentId")).Text;
                Response.Redirect("../Registers/RegistersTrainingStaff.aspx?departmentid=" + ViewState["deptid"], false);
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

    protected void gvTrainingDepartment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTrainingDepartment.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvTrainingDepartment_ItemDataBound(object sender, GridItemEventArgs e)
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

            RadTextBox tb1 = (RadTextBox)e.Item.FindControl("txtDepartmentIdAdd");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");

            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnDepartmentAdd");
            if (ib1 != null) ib1.Attributes.Add("onclick", "return showPickList('spnPickListDepartmentAdd', 'codehelp1', '', '../Common/CommonPickListDepartment.aspx')");
        }
    }
}
