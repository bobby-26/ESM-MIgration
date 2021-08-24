using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersCrewOTLimitConfiguration : PhoenixBasePage
{
  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersCrewOTLimitConfiguration.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOTConfiguration')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCrewOTLimitConfiguration.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");            
            MenuRegistersOT.AccessRights = this.ViewState;
            MenuRegistersOT.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvOTConfiguration.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        string[] alColumns = { "FLDRANKGROUPNAME", "FLDMAXLIMIT" };
        string[] alCaptions = { "Rank Group", "Max Limit for OT" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersOverTimeReason.OverTimeLimitConfigurationSearch(
                General.GetNullableInteger(ucRankGroup.SelectedHard),
                sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvOTConfiguration.PageSize,
                ref iRowCount,
                ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=OTLimitConfig.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>OT Limit Configuration</h3></td>");
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

    protected void RegistersOT_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvOTConfiguration.Rebind();
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

        string[] alColumns = { "FLDRANKGROUPNAME", "FLDMAXLIMIT" };
        string[] alCaptions = { "Rank Group", "Max Limit for OT" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersOverTimeReason.OverTimeLimitConfigurationSearch(
                    General.GetNullableInteger(ucRankGroup.SelectedHard),
                    sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvOTConfiguration.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        General.SetPrintOptions("gvOTConfiguration", "OT Limit Configuration", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvOTConfiguration.DataSource = ds;
            gvOTConfiguration.VirtualItemCount = iRowCount;
        }
        else
        {
            gvOTConfiguration.DataSource = "";
        }
    }
    
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvOTConfiguration.Rebind();
    }    

    private bool IsValidReason(string rankgroup, string maxlimit)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if(General.GetNullableInteger(rankgroup) == null)
            ucError.ErrorMessage = "Rank Group is required.";

        if (General.GetNullableInteger(maxlimit) == null)
            ucError.ErrorMessage = "Max OT Limit is required.";

        return (!ucError.IsError);
    }

  
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvOTConfiguration_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidReason(((UserControlHard)e.Item.FindControl("ucRankGroupAdd")).SelectedHard,
                    ((UserControlMaskNumber)e.Item.FindControl("ucMaxLimitAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersOverTimeReason.InsertOverTimeLimitConfiguration(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        int.Parse(((UserControlHard)e.Item.FindControl("ucRankGroupAdd")).SelectedHard),
                        int.Parse(((UserControlMaskNumber)e.Item.FindControl("ucMaxLimitAdd")).Text));

                BindData();
                gvOTConfiguration.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersOverTimeReason.DeleteOverTimeLimitConfiguration(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(((RadLabel)e.Item.FindControl("lblconfigurationid")).Text));
                BindData();
                gvOTConfiguration.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidReason(((UserControlHard)e.Item.FindControl("ucRankGroupEdit")).SelectedHard,
                  ((UserControlMaskNumber)e.Item.FindControl("ucMaxLimitEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersOverTimeReason.UpdateOverTimeLimitConfiguration(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(((RadLabel)e.Item.FindControl("lblconfigurationidedit")).Text)
                    , int.Parse(((UserControlHard)e.Item.FindControl("ucRankGroupEdit")).SelectedHard)
                    , int.Parse(((UserControlMaskNumber)e.Item.FindControl("ucMaxLimitEdit")).Text));
                
                BindData();
                gvOTConfiguration.Rebind();
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

    protected void gvOTConfiguration_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOTConfiguration.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvOTConfiguration_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
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

            DataRowView dr = (DataRowView)e.Item.DataItem;
            UserControlHard ucRankGroupEdit = (UserControlHard)e.Item.FindControl("ucRankGroupEdit");
            if (ucRankGroupEdit != null)
            {
                ucRankGroupEdit.bind();
                ucRankGroupEdit.SelectedHard = dr["FLDRANKGROUPID"].ToString();
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
            UserControlHard uct = (UserControlHard)e.Item.FindControl("ucRankGroupAdd");
            if (uct != null)
                uct.bind();
        }
    }
}
