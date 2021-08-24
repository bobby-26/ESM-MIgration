using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Registers_RegistersCrewExperienceMappingAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersCrewExperienceMappingAdd.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRank')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCrewExperienceMappingAdd.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCrewExperienceMappingAdd.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");            
            MenuRegistersRank.AccessRights = this.ViewState;
            MenuRegistersRank.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvRank.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();
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
        string[] alColumns = { "FLDRANKCODE", "FLDRANKNAME", "FLDLEVEL", "FLDGROUPRANK", "FLDGROUP", "FLDOFFICECREW" };
        string[] alCaptions = { "Code", "Name", "Level", "Group Rank", "Group", "Office Crew" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixRegistersRank.RankSearch(0, txtSearch.Text, txtRankCode.Text, null, sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvRank.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=Rank.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0'>");
        Response.Write("<tr>");
        Response.Write("<td width=\"150px\"><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Rank</h3></td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width=\"150px\">");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td width=\"150px\">");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void RegistersRank_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvRank.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtRankCode.Text = "";
                txtSearch.Text = "";
                BindData();
                gvRank.Rebind();
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
        string[] alColumns = { "FLDRANKCODE", "FLDRANKNAME", "FLDLEVEL", "FLDGROUPRANK", "FLDGROUP", "FLDOFFICECREW" };
        string[] alCaptions = { "Code", "Name", "Level",  "Group Rank", "Group", "Office Crew" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixRegistersRank.RankSearch(0, txtRankCode.Text, txtSearch.Text, null, sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvRank.PageSize,
            ref iRowCount,
            ref iTotalPageCount);
        General.SetPrintOptions("gvRank", "Rank", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvRank.DataSource = ds;
            gvRank.VirtualItemCount = iRowCount;
        }
        else
        {
            gvRank.DataSource = "";
        }
    }
    protected void gvRank_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRank.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvRank_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;        
            //else if (e.CommandName.ToUpper().Equals("EXPMAP"))
            //{
            //    string rankId = (e.Item as GridDataItem).GetDataKeyValue("FLDRANKID").ToString();
            //    string group = ((RadLabel)e.Item.FindControl("lblGroupId")).Text;
            //    string officecrew = ((RadLabel)e.Item.FindControl("lblOfficeCrewId")).Text;
            //    RadScriptManager.RegisterStartupScript(Page, Page.GetType(),
            //                     "Script", "javascript:openNewWindow('codehelp1', '', " + Session["sitepath"] + "/Registers/'RegistersCrewExperienceMapping.aspx?rankId=" + rankId + "&group=" + group + "&officecrew=" + officecrew + "');", true);
            //    //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
            //      //               "Script", "javascript:openNewWindow('codehelp1', '', " + Session["sitepath"] + "/Registers/'RegistersCrewExperienceMapping.aspx?rankId=" + rankId + "&group=" + group + "&officecrew=" + officecrew + "');", true);
            //}
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

    protected void gvRank_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if(e.Item is GridDataItem)
        {
            string rankId = (e.Item as GridDataItem).GetDataKeyValue("FLDRANKID").ToString();
            string group = ((RadLabel)e.Item.FindControl("lblGroupId")).Text;
            string officecrew = ((RadLabel)e.Item.FindControl("lblOfficeCrewId")).Text;
            LinkButton expMap = (LinkButton)e.Item.FindControl("cmdExpMap");
            if (expMap != null)
            {
                expMap.Visible = SessionUtil.CanAccess(this.ViewState, expMap.CommandName);
                expMap.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Registers/RegistersCrewExperienceMapping.aspx?rankId=" + rankId + "&group=" + group + "&officecrew=" + officecrew + "');");
            }
        }
    }
}
