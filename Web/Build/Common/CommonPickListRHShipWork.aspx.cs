using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CommonPickListRHShipWork : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
            ViewState["PAGENUMBER"] = 1;
			ViewState["SORTEXPRESSION"] = null;
			ViewState["SORTDIRECTION"] = null;
			ViewState["CURRENTINDEX"] = 1;
			ViewState["SHIPCALENDARID"] = "";
			BindYear();
            if (Request.QueryString["framename"] != null)
            {
                ViewState["framename"] = Request.QueryString["framename"].ToString();
                gvShipCalendar.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
	}
	private void BindYear()
	{
		for (int i = 2012; i <= DateTime.Now.Year; i++)
		{
			ddlYear.Items.Add(i.ToString());
		}

        ddlYear.DataBind();
        ddlmonth.SelectedValue = DateTime.Now.Month.ToString();
		txtVesselname.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
	}
	public void BindData()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;
		DataSet ds = new DataSet();
		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
		try
		{
			ds = PhoenixVesselAccountsRH.RestHourShipWorkCalendarSearch(
				   PhoenixSecurityContext.CurrentSecurityContext.VesselID,
				   int.Parse(ddlYear.SelectedItem.Text),
				   int.Parse(ddlmonth.SelectedValue),
				   (int)ViewState["PAGENUMBER"],
                   gvShipCalendar.PageSize,
				   ref iRowCount,
				   ref iTotalPageCount);

            gvShipCalendar.DataSource = ds;
            gvShipCalendar.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
			{
                if (ViewState["SHIPCALENDARID"].ToString() == "")
				{
					ViewState["SHIPCALENDARID"] = ds.Tables[0].Rows[0][0].ToString();
                    gvShipCalendar.SelectedIndexes.Clear();
				}
            }
			else
			{
                ViewState["SHIPCALENDARID"] = "";
			}
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
    }
	protected void ddlmonth_OnSelectedIndexChanged(object sender, EventArgs e)
	{
        RebindShipCalendar();
        RebindShipWork();
	}
	protected void gvShipCalendar_OnItemCommand(object sender, GridCommandEventArgs e)
	{
	    if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            ViewState["SHIPCALENDARID"] = ((RadLabel)e.Item.FindControl("lblCalenderid")).Text;

            RebindShipCalendar();
            RebindShipWork();
        }
    }
    public void BindReportingDate()
	{
	    DataSet ds = new DataSet();
		try
		{
			ds = PhoenixVesselAccountsRH.ListRestHourWork(
				   PhoenixSecurityContext.CurrentSecurityContext.VesselID,
				   General.GetNullableInteger(ViewState["SHIPCALENDARID"].ToString()));
            
            gvShipWork.DataSource = ds;
        }
        catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
    }
    protected void gvShipWork_OnItemCommand(object sender, GridCommandEventArgs e)
	{
        if(e.CommandName.ToUpper().Equals("PICK"))
        {
            string Script = "";
            NameValueCollection nvc;

            if (Request.QueryString["mode"] == "custom")
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";

                if (ViewState["framename"] != null)
                    Script += "fnReloadList('codehelp1','" + ViewState["framename"].ToString() + "');";
                else
                    Script += "fnReloadList('codehelp1');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                RadLabel lblcalendarid = (RadLabel)e.Item.FindControl("lblShipcalendarid");
                nvc.Add(lblcalendarid.ID, lblcalendarid.Text);
                RadLabel lbldate = (RadLabel)e.Item.FindControl("lblDate");
                nvc.Add(lblcalendarid.ID, lbldate.Text);
                LinkButton lnkHour = (LinkButton)e.Item.FindControl("lnkReportingHour");
                nvc.Add(lblcalendarid.ID, lnkHour.Text.ToString());
            }
            else
            {
                if (Request.QueryString["ignoreiframe"] != null)
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnClosePickList('codehelp1','ifMoreInfo', true);";
                    Script += "</script>" + "\n";
                }
                else
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    if (ViewState["framename"] != null)
                        Script += "fnClosePickList('codehelp1','" + ViewState["framename"].ToString() + "');";
                    else
                        Script += "fnClosePickList('codehelp1');";
                    Script += "</script>" + "\n";
                }

                nvc = Filter.CurrentPickListSelection;

                RadLabel lbldate = (RadLabel)e.Item.FindControl("lblDate");
                nvc.Set(nvc.GetKey(1), lbldate.Text.ToString());
                RadLabel lblcalendarid = (RadLabel)e.Item.FindControl("lblShipcalendarid");
                nvc.Set(nvc.GetKey(2), lblcalendarid.Text);
                LinkButton lnkHour = (LinkButton)e.Item.FindControl("lnkReportingHour");
                nvc.Set(nvc.GetKey(3), lnkHour.Text.ToString());
            }

            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        }
	}
    protected void gvShipCalendar_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvShipCalendar.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RebindShipCalendar()
    {
        gvShipCalendar.SelectedIndexes.Clear();
        gvShipCalendar.EditIndexes.Clear();
        gvShipCalendar.DataSource = null;
        gvShipCalendar.Rebind();
    }
    protected void RebindShipWork()
    {
        gvShipWork.SelectedIndexes.Clear();
        gvShipWork.EditIndexes.Clear();
        gvShipWork.DataSource = null;
        gvShipWork.Rebind();
    }
    protected void gvShipWork_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvShipWork.CurrentPageIndex + 1;
            BindReportingDate();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlYear_DataBound(object sender, EventArgs e)
    {
        ddlYear.Items.Sort();
    }
}
