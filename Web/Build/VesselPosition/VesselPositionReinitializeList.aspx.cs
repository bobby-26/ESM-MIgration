using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using System.Collections.Specialized;
using System.Web;
using Telerik.Web.UI;

public partial class VesselPositionReinitializeList : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("List", "LIST");
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            toolbarmain.AddButton("Re-Initialize", "REINITIALIZE");
        MenuReinitialize.AccessRights = this.ViewState;
        MenuReinitialize.MenuList = toolbarmain.Show();
        MenuReinitialize.SelectedMenuIndex = 0;

        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionReinitializeList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvReinitialize')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuReinitializeList.AccessRights = this.ViewState;
        MenuReinitializeList.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
			ViewState["SORTEXPRESSION"] = null;
			ViewState["SORTDIRECTION"] = null;
			ViewState["CURRENTINDEX"] = 1;
            gvReinitialize.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }
	}
    
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVESSELNAME", "FLDDATE", "FLDVOYAGENO", "FLDSEAPORTNAME", "FLDREPORTTYPE", "FLDRESONFORREINITIAL", "FLDREMARKS" };
        string[] alCaptions = { "Vessel", "Date", "Voyage", "Port", "Start From", "Reason", "Remarks" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixVesselPositionROBReInitialization.ReInitializationList(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
             int.Parse(ViewState["PAGENUMBER"].ToString()),
             gvReinitialize.PageSize,
             ref iRowCount,
             ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename= ReInitialize.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Request.Url.Scheme + "://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Re-Initialization</h3></td>");
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
    protected void gvReinitialize_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvReinitialize.CurrentPageIndex + 1;
        BindData();
    }
    private void BindData()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDDATE", "FLDVOYAGENO", "FLDSEAPORTNAME", "FLDREPORTTYPE", "FLDRESONFORREINITIAL", "FLDREMARKS" };
        string[] alCaptions = { "Vessel", "Date", "Voyage", "Port", "Start From", "Reason", "Remarks" };

        DataSet ds = PhoenixVesselPositionROBReInitialization.ReInitializationList(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
             int.Parse(ViewState["PAGENUMBER"].ToString()),
             gvReinitialize.PageSize,
             ref iRowCount,
             ref iTotalPageCount );

        General.SetPrintOptions("gvReinitialize", "Re-Initialization", alCaptions, alColumns, ds);
        gvReinitialize.DataSource = ds;
        gvReinitialize.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
		ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
	}


    protected void gvReinitialize_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                LinkButton lnkDate = (LinkButton)e.Item.FindControl("lnkDate");
                RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselId");
                Response.Redirect("../VesselPosition/VesselPositionROBReInitialization.aspx?Date=" + lnkDate.Text + "&Vesselid=" + lblVesselid.Text);
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


    protected void MenuReinitializeList_TabStripCommand(object sender, EventArgs e)
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
    protected void MenuReinitialize_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("REINITIALIZE"))
            {
                Response.Redirect("../VesselPosition/VesselPositionROBReInitialization.aspx?Date=&Vesselid=");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
}
