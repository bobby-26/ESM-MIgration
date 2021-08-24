using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Registers_RegistersCrewExperienceMapping : PhoenixBasePage
{
    string ranklist = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Add", "SAVE",ToolBarDirection.Right);
        MenuTitle.AccessRights = this.ViewState;        
        MenuTitle.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvRankList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        BindData();
    }

    protected void MenuRank_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvRankList.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                BindData();
                gvRankList.Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtRankCode.Text = "";                
                BindData();
                gvRankList.Rebind();
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
        int? group = null;
        int? officecrew = null;
        int? rank = null;

        string[] alColumns = { "FLDRANKCODE", "FLDRANKNAME" };
        string[] alCaptions = { "Code", "Name"};        
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (Request.QueryString["group"] != null)
            group = General.GetNullableInteger(Request.QueryString["group"]);

        if (Request.QueryString["officecrew"] != null)
            officecrew = General.GetNullableInteger(Request.QueryString["officecrew"]);

        if (Request.QueryString["rankId"] != null)
            rank = General.GetNullableInteger(Request.QueryString["rankId"]);

        DataSet ds = PhoenixRegistersRank.RankSearch(0, 
                                                    txtRankCode.Text, 
                                                    null, 
                                                    null,
                                                    group,
                                                    officecrew, 
                                                    sortexpression, 
                                                    sortdirection,
                                                    (int)ViewState["PAGENUMBER"],
                                                    gvRankList.PageSize,
                                                    ref iRowCount,
                                                    ref iTotalPageCount);
        DataTable dtranklist = PhoenixRegistersExperienceMapping.ListExperienceMapping(rank);
       
        General.SetPrintOptions("gvRankList", "Rank List", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvRankList.DataSource = ds;
            gvRankList.VirtualItemCount = iRowCount;

            if (dtranklist != null && dtranklist.Rows.Count > 0)
            {
                ranklist = dtranklist.Rows[0]["FLDMAPPINGRANK"].ToString();
                hdnExperienceMappingId.Value = dtranklist.Rows[0]["FLDEXPERIENCEMAPPINGID"].ToString();
            }
        }
        else
        {
            gvRankList.DataSource = "";
        }        
    }

    
    protected void MenuTitle_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string csvRank = string.Empty;
                foreach (GridDataItem gv in gvRankList.Items)
                {
                    RadCheckBox ck = (RadCheckBox)gv.FindControl("chkSelect");
                    if (ck.Checked==true && ck.Enabled)
                    {                        
                        csvRank += gv.GetDataKeyValue("FLDRANKID").ToString()+ ",";
                    }
                }
                if (string.IsNullOrEmpty(csvRank))
                {
                    ucError.Text = "select atleast one rank";
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    int rankId = 0;
                    if (Request.QueryString["rankId"] != null)
                        rankId = General.GetNullableInteger(Request.QueryString[rankId]).Value;

                    string[] ranks = csvRank.Split(',');
                    int count=0;
                    foreach (string rank in ranks)
                    {
                        if (rank == rankId.ToString())
                            count++;
                    }
                    if (count == 0)
                        csvRank = csvRank + "," + rankId;

                   // if (string.IsNullOrEmpty(hdnExperienceMappingId.Value))
                        PhoenixRegistersExperienceMapping.InsertExperienceMapping(rankId, csvRank);
                    //else
                    //    PhoenixRegistersExperienceMapping.UpdateExperienceMapping(rankId
                    //                                                    , General.GetNullableInteger(hdnExperienceMappingId.Value).Value
                    //                                                    , csvRank);                    
                }
                String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', '');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRankList_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
    protected void gvRankList_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRankList.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvRankList_ItemDataBound(object sender, GridItemEventArgs e)
    { 
        foreach (GridDataItem gvr in gvRankList.Items)
        {
            RadCheckBox chk = ((RadCheckBox)gvr.FindControl("chkSelect"));
            string rnk = gvr.GetDataKeyValue("FLDRANKID").ToString();
            string[] ranks = ranklist.Split(',');
            foreach (string r in ranks)
            {
                if (chk != null && r == rnk)
                    chk.Checked = true;
            }
        }
    }
}