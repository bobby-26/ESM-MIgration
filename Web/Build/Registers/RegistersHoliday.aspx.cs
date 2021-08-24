using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersHoliday : PhoenixBasePage
{

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersHoliday.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvHoliday')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersHoliday.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");           
            MenuRegistersHoliday.AccessRights = this.ViewState;
			MenuRegistersHoliday.MenuList = toolbar.Show();

			if (!IsPostBack)
			{
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
                gvHoliday.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
		string[] alColumns = { "FLDDATE", "FLDREASON" };
		string[] alCaptions = { "Date", "Reason" };
		string sortexpression;
		int? sortdirection = null;

		sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
			iRowCount = 10;
		else
			iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

		ds = PhoenixRegistersHoliday.HolidaySearch(General.GetNullableDateTime(txtDate.Text),General.GetNullableString(txtReason.Text), 
			sortexpression, sortdirection,
			Int32.Parse(ViewState["PAGENUMBER"].ToString()),
			gvHoliday.PageSize,
			ref iRowCount,
			ref iTotalPageCount);

		Response.AddHeader("Content-Disposition", "attachment; filename=Holiday.xls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		Response.Write("<tr>");
		Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
		Response.Write("<td><h3>Holiday</h3></td>");
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

	protected void RegistersHoliday_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
			{
				ViewState["PAGENUMBER"] = 1;
				BindData();				
                gvHoliday.Rebind();
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
		string[] alColumns = { "FLDDATE", "FLDREASON" };
		string[] alCaptions = { "Date", "Reason" };
		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;
		DataSet ds = new DataSet();
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
		
		ds = PhoenixRegistersHoliday.HolidaySearch(General.GetNullableDateTime(txtDate.Text),General.GetNullableString(txtReason.Text), 
			sortexpression, sortdirection,
			Int32.Parse(ViewState["PAGENUMBER"].ToString()),
			gvHoliday.PageSize,
			ref iRowCount,
			ref iTotalPageCount);

		General.SetPrintOptions("gvHoliday", "Holiday", alCaptions, alColumns, ds);

		if (ds.Tables[0].Rows.Count > 0)
		{
			gvHoliday.DataSource = ds;
            gvHoliday.VirtualItemCount = iRowCount;
		}
		else
		{
            gvHoliday.DataSource = "";
        }
	}
	protected void cmdSearch_Click(object sender, EventArgs e)
	{
		ViewState["PAGENUMBER"] = 1;
		BindData();
        gvHoliday.Rebind();
	}    
	private void InsertHoliday(string date, string reason)
	{

		PhoenixRegistersHoliday.InsertHoliday(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
			Convert.ToDateTime(date),General.GetNullableString(reason));
		ucStatus.Text = "Information updated";
	}

	
	private bool IsValidHoliday(string date)
	{
		ucError.HeaderMessage = "Please provide the following required information";

		

		//if (code.Trim().Equals(""))
		//    ucError.ErrorMessage = "Code is required.";

		//if (name.Trim().Equals(""))
		//    ucError.ErrorMessage = "Name is required.";

		return (!ucError.IsError);
	}

	private void DeleteHoliday(int Holidaycode)
	{
		PhoenixRegistersHoliday.DeleteHoliday(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Holidaycode);
	}


	public StateBag ReturnViewState()
	{
		return ViewState;
	}

    protected void gvHoliday_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidHoliday(((UserControlDate)e.Item.FindControl("txtDateAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertHoliday(
                    ((UserControlDate)e.Item.FindControl("txtDateAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtReasonAdd")).Text
                );
                BindData();
                gvHoliday.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidHoliday(((UserControlDate)e.Item.FindControl("txtDateEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertHoliday(
                     ((UserControlDate)e.Item.FindControl("txtDateEdit")).Text,
                     ((RadTextBox)e.Item.FindControl("txtReasonEdit")).Text
                 );               
                BindData();
                gvHoliday.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteHoliday(Int32.Parse(((RadLabel)e.Item.FindControl("lblHolidayId")).Text));
                BindData();
                gvHoliday.Rebind();
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

    protected void gvHoliday_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvHoliday.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvHoliday_ItemDataBound(object sender, GridItemEventArgs e)
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
}
