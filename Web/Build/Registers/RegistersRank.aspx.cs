using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using Telerik.Web.UI;

public partial class RegistersRank : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersRank.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRank')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersRank.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersRank.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegistersRank.AccessRights = this.ViewState;
            MenuRegistersRank.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvRank.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
		string[] alColumns = { "FLDRANKCODE", "FLDRANKNAME", "FLDLEVEL", "FLDCREWSORT", "FLDGROUPRANK", "FLDGROUP", "FLDOFFICECREW", "FLDLICENCEREQ", "FLDACTIVE" };
        string[] alCaptions = { "Code","Name","Level","Crew Sort","Group Rank","Group","Office Crew","Licence req","Active Y/N" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersRank.RankSearch(0,txtSearch.Text,txtRankCode.Text, null, sortexpression, sortdirection,
                1,
                iRowCount,
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
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtRankCode.Text = "";
                txtSearch.Text = "";
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvRank.SelectedIndexes.Clear();
        gvRank.EditIndexes.Clear();
        gvRank.DataSource = null;
        gvRank.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
		string[] alColumns = { "FLDRANKCODE", "FLDRANKNAME", "FLDLEVEL", "FLDCREWSORT", "FLDGROUPRANK", "FLDGROUP", "FLDOFFICECREW", "FLDLICENCEREQ", "FLDTOPFOURYESNO", "FLDTOPTWOYESNO", "FLDACTIVE" };
		string[] alCaptions = { "Code", "Name", "Level", "Crew Sort", "Group Rank", "Group", "Office Crew", "Licence req", "Top4 Y/N", "Top2 Y/N", "Active Y/N" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixRegistersRank.RankSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtRankCode.Text, txtSearch.Text, null, sortexpression, sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvRank.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvRank", "Rank", alCaptions, alColumns, ds);

        gvRank.DataSource = ds;
        gvRank.VirtualItemCount = iRowCount;
     
        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvRank_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidRank(((RadTextBox)e.Item.FindControl("txtRankCodeAdd")).Text,
                     ((RadTextBox)e.Item.FindControl("txtRankNameAdd")).Text,
                        ((RadMaskedTextBox)e.Item.FindControl("txtNumberLevelMask")).Text,
                        ((RadMaskedTextBox)e.Item.FindControl("txtNumberCrewSortMask")).Text))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    InsertRank(
                        ((RadTextBox)e.Item.FindControl("txtRankCodeAdd")).Text,
                        ((RadTextBox)e.Item.FindControl("txtRankNameAdd")).Text,
                         Int32.Parse(((RadMaskedTextBox)e.Item.FindControl("txtNumberLevelMask")).Text),
                         Int32.Parse(((RadMaskedTextBox)e.Item.FindControl("txtNumberCrewSortMask")).Text),
                        General.GetNullableString(((RadComboBox)e.Item.FindControl("ddlGroupRankAdd")).SelectedValue),
                        ((UserControlHard)e.Item.FindControl("ucGroupAdd")).SelectedHard,
                        ((UserControlHard)e.Item.FindControl("ucOfficeCrewAdd")).SelectedHard,
                        (((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked.Equals(true)) ? 1 : 0,
						((UserControlHard)e.Item.FindControl("ucLevelTypeAdd")).SelectedHard,
						(((RadCheckBox)e.Item.FindControl("chkLicenceReqAdd")).Checked.Equals(true)) ? 1 : 0,
                        (byte?)General.GetNullableInteger(((RadDropDownList)e.Item.FindControl("ddlCBAApplicableAdd")).SelectedValue),
                         (((RadCheckBox)e.Item.FindControl("chkTop4YNAdd")).Checked.Equals(true)) ? 1 : 0,
                        (((RadCheckBox)e.Item.FindControl("chkTop2YNAdd")).Checked.Equals(true)) ? 1 : 0
                    );
                    Rebind();
                    ((RadTextBox)e.Item.FindControl("txtRankNameAdd")).Focus();
                }
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidRank(((RadTextBox)e.Item.FindControl("txtRankCodeEdit")).Text,
                  ((RadTextBox)e.Item.FindControl("txtRankNameEdit")).Text,
                  ((RadMaskedTextBox)e.Item.FindControl("txtNumberLevelMaskEdit")).Text,
                  ((RadMaskedTextBox)e.Item.FindControl("txtNumberCrewSortMaskEdit")).Text))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    UpdateRank(
                             Int32.Parse(((RadLabel)e.Item.FindControl("lblRankIDEdit")).Text),
                             ((RadTextBox)e.Item.FindControl("txtRankCodeEdit")).Text,
                             ((RadTextBox)e.Item.FindControl("txtRankNameEdit")).Text,
                             Int32.Parse(((RadMaskedTextBox)e.Item.FindControl("txtNumberLevelMaskEdit")).Text),
                             Int32.Parse(((RadMaskedTextBox)e.Item.FindControl("txtNumberCrewSortMaskEdit")).Text),
                             General.GetNullableString(((RadComboBox)e.Item.FindControl("ddlGroupRankEdit")).SelectedValue),
                             ((UserControlHard)e.Item.FindControl("ucGroupEdit")).SelectedHard,
                             ((UserControlHard)e.Item.FindControl("ucOfficeCrewEdit")).SelectedHard,
                             (((RadCheckBox)e.Item.FindControl("chkActiveYNEdit")).Checked.Equals(true)) ? 1 : 0,
							  ((UserControlHard)e.Item.FindControl("ucLevelTypeEdit")).SelectedHard,
							 (((RadCheckBox)e.Item.FindControl("chkLicenceReqEdit")).Checked.Equals(true)) ? 1 : 0,
                             (byte?)General.GetNullableInteger(((RadDropDownList)e.Item.FindControl("ddlCBAApplicable")).SelectedValue),
                               (((RadCheckBox)e.Item.FindControl("chkTop4YNEdit")).Checked.Equals(true)) ? 1 : 0,
                             (((RadCheckBox)e.Item.FindControl("chkTop2YNEdit")).Checked.Equals(true)) ? 1 : 0
                         );
                    Rebind();
                }
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["RankID"] = ((RadLabel)e.Item.FindControl("lblRankID")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
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
    protected void gvRank_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if(e.Item is GridEditableItem)
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

            LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
            if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);
        }
        if (e.Item.IsInEditMode)
        {
            UserControlHard ucHard = (UserControlHard)e.Item.FindControl("ucGroupEdit");
            DataRowView drvHard = (DataRowView)e.Item.DataItem;
            if (ucHard != null)
                ucHard.SelectedHard = DataBinder.Eval(e.Item.DataItem, "FLDGROUPID").ToString();

            UserControlHard ucCrew = (UserControlHard)e.Item.FindControl("ucOfficeCrewEdit");
            DataRowView drvCrew = (DataRowView)e.Item.DataItem;
            if (ucCrew != null) ucCrew.SelectedHard = DataBinder.Eval(e.Item.DataItem, "FLDOFFICECREWID").ToString();

            UserControlHard ucLevel = (UserControlHard)e.Item.FindControl("ucLevelTypeEdit");
            DataRowView drvLevel = (DataRowView)e.Item.DataItem;
            if (ucLevel != null) ucLevel.SelectedHard = DataBinder.Eval(e.Item.DataItem, "FLDLEVELTYPE").ToString();

            RadDropDownList cba = (RadDropDownList)e.Item.FindControl("ddlCBAApplicable");
            if (cba != null) cba.SelectedValue = DataBinder.Eval(e.Item.DataItem, "FLDCBAAPPLICABLE").ToString();

            RadComboBox gre = (RadComboBox)e.Item.FindControl("ddlGroupRankEdit");
            DataRowView drvGroupRank = (DataRowView)e.Item.DataItem;
            if (gre != null)
            {
                DataSet ds = new DataSet();
                ds = PhoenixRegistersGroupRank.ListGroupRank(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                gre.DataSource = ds;
                gre.DataTextField = "FLDGROUPRANK";
                gre.DataValueField = "FLDGROUPRANK";
                gre.DataBind();
                gre.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                gre.SelectedValue = DataBinder.Eval(e.Item.DataItem, "FLDGROUPRANK").ToString();
            }
        }

        if (e.Item is GridFooterItem)
        {
            RadComboBox gra = (RadComboBox)e.Item.FindControl("ddlGroupRankAdd");
            if (gra != null)
            {
                DataSet ds = new DataSet();
                ds = PhoenixRegistersGroupRank.ListGroupRank(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                gra.DataSource = ds;
                gra.DataTextField = "FLDGROUPRANK";
                gra.DataValueField = "FLDGROUPRANK";
                gra.DataBind();
                gra.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            }
        }
    }
    private void InsertRank(string Rankcode, string Rankname, Int32 level, Int32 crewsort
        , string grouprank, string group, string officecrew, int activeyn,string leveltype,int? licencereqyn, byte? cbaapplicable,int top4yn, int top2yn)
    {
        PhoenixRegistersRank.InsertRank(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            Rankcode, Rankname, level, crewsort, grouprank, General.GetNullableInteger(group), General.GetNullableInteger(officecrew), activeyn
			, General.GetNullableInteger(leveltype), licencereqyn, cbaapplicable, top4yn, top2yn);
    }

	private void UpdateRank(int rankid, string Rankcode, string Rankname, Int32 level, Int32 crewsort
        , string grouprank, string group, string officecrew, int activeyn, string leveltype,int? licencereqyn, byte? cbaapplicable,int top4yn, int top2yn)
    {
        PhoenixRegistersRank.UpdateRank(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            rankid, Rankcode, Rankname, level, crewsort, grouprank, General.GetNullableInteger(group), General.GetNullableInteger(officecrew), activeyn
			, General.GetNullableInteger(leveltype), licencereqyn, cbaapplicable, top4yn, top2yn);
        ucStatus.Text = "Rank information updated";
    }

    private bool IsValidRank(string rankcode, string Rankname, string level, string crewsort)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvRank;

        if (rankcode.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (Rankname.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        if (level.Trim().Equals(""))
            ucError.ErrorMessage = "Level is required.";

        if (crewsort.Trim().Equals(""))
            ucError.ErrorMessage = "Crew Sort is required.";


        return (!ucError.IsError);
    }

    private void DeleteRank(int Rankcode)
    {
        PhoenixRegistersRank.DeleteRank(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Rankcode);
    }

    protected void gvRank_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRank.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRank_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DeleteRank(Int32.Parse(ViewState["RankID"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
