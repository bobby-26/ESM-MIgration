using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Registers_RegistersRequiredCourseAlternateRank : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Add", "SAVE", ToolBarDirection.Right);
        MenuTitle.AccessRights = this.ViewState;
        MenuTitle.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
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
                    if (ck.Checked == true)
                    {
                        csvRank += ((RadLabel)gv.FindControl("lblrankid")).Text + ",";
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
                    int vslDocId = 0;
                    if (Request.QueryString["vslDocId"] != null)
                        vslDocId = General.GetNullableInteger(Request.QueryString[vslDocId]).Value;
                    string[] ranks = csvRank.Split(',');

                    PhoenixRegistersVesselDocumentCourse.UpdateAlternatRank(vslDocId, csvRank);
                }
                //String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', '');");
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvRankList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRankList.CurrentPageIndex + 1;
            BindData();


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        int rank = 0;
      //  string ranklist;
      //  int vslDocId = 0;
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? group = null;
        int? officecrew = null;
        int? sortdirection = null;

        if (Request.QueryString["rankId"] != null)
            rank = General.GetNullableInteger(Request.QueryString["rankId"]).Value;

        //if (Request.QueryString["vslDocId"] != null)
        //    vslDocId = General.GetNullableInteger(Request.QueryString["vslDocId"]).Value;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        DataSet ds = PhoenixRegistersRank.EditRank(rank);
        if (ds != null && ds.Tables.Count > 0)
        {
            group = General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDGROUP"].ToString());
            officecrew = General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDOFFICECREW"].ToString());
        }
        DataSet dsrnk = PhoenixRegistersRank.RankSearch(0, null, null, null, group, officecrew, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvRankList.PageSize, ref iRowCount, ref iTotalPageCount);

       // DataTable dtranklist = PhoenixRegistersVesselDocumentCourse.EditDocumentsRequired(vslDocId);
        gvRankList.DataSource = dsrnk;
        gvRankList.VirtualItemCount = iRowCount;
        //if (dsrnk.Tables[0].Rows.Count > 0)
        //{
        //    if (dtranklist != null && dtranklist.Rows.Count > 0)
        //    {
        //        ranklist = dtranklist.Rows[0]["FLDALTERNATERANKLIST"].ToString();
        //        foreach (GridDataItem gv in gvRankList.Items)
        //        {
        //            RadCheckBox chk = (RadCheckBox)gv.FindControl("chkSelect");
        //            string rnk = ((RadLabel)gv.FindControl("lblrankid")).Text;
        //            string[] ranks = ranklist.Split(',');
        //            foreach (string r in ranks)
        //            {
        //                if (chk != null && r == rnk)
        //                    chk.Checked = true;
        //            }
        //        }
        //    }
        //}
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

 
    protected void gvRankList_ItemCreated(object sender, GridItemEventArgs e)
    {
        int vslDocId = 0; string ranklist;
        if (Request.QueryString["vslDocId"] != null)
            vslDocId = General.GetNullableInteger(Request.QueryString["vslDocId"]).Value;
        DataTable dtranklist = PhoenixRegistersVesselDocumentCourse.EditDocumentsRequired(vslDocId);
        if (dtranklist != null && dtranklist.Rows.Count > 0)
        {

            ranklist = dtranklist.Rows[0]["FLDALTERNATERANKLIST"].ToString();
            foreach (GridDataItem gv in gvRankList.Items)
            {
                RadCheckBox chk = (RadCheckBox)gv.FindControl("chkSelect");
                string rnk = ((RadLabel)gv.FindControl("lblrankid")).Text;
                string[] ranks = ranklist.Split(',');
                foreach (string r in ranks)
                {
                    if (chk != null && r == rnk)
                        chk.Checked = true;
                }
            }
        }

    }
}