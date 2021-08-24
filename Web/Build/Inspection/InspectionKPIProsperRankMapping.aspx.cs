using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class Inspection_InspectionKPIProsperRankMapping : PhoenixBasePage
{


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            //   toolbar.AddLinkButton("../Inspection/InspectionKPIProsperRankMapping.aspx", "Export to Excel", "icon_xls.png", "Excel");
            //  toolbar.AddImageLink("javascript:CallPrint('gvkpirankmapping')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionKPIProsperRankMapping.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuRegistersInspection.AccessRights = this.ViewState;
            MenuRegistersInspection.MenuList = toolbar.Show();
            // MenuRegistersInspection.SetTrigger(pnlInspectionEntry);

            ViewState["COMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                ViewState["COMPANYID"] = nvc.Get("QMS");

            if (!IsPostBack)
            {

                if (ddlsourcetype.SelectedIndex <= 0)
                    ddlsourcetype.SelectedIndex = 1;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                //ViewState["CURRENTINDEX"] = 1;

                Bindcategory();
                gvkpirankmapping.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }



        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        //int typeid = 0;
        string docname = "";

        string[] alColumns = { "FLDCATEGORYID", "FLDSUBCATEGORYID" };
        string[] alCaptions = { "Category", "Sub Category" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());



        DataSet ds = PhoenixKPIRegisters.RankMappingSearch(General.GetNullableInteger(ddlsourcetype.SelectedValue.ToString())
        , General.GetNullableGuid(ddlcategory.SelectedValue.ToString())
        , General.GetNullableInteger(ddlscoretype.SelectedValue.ToString())
        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
        , gvkpirankmapping.PageSize
        , ref iRowCount
        , ref iTotalPageCount);



        General.SetPrintOptions("gvkpirankmapping", docname, alCaptions, alColumns, ds);
        DataTable dt = ds.Tables[0];
        gvkpirankmapping.DataSource = dt;
        gvkpirankmapping.VirtualItemCount = iRowCount;


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }


    protected void RegistersInspection_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {

                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvkpirankmapping.Rebind();
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



    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int typeid = 0;
        string docname = "";

        string[] alColumns = { "FLDCATEGORYID", "FLDSUBCATEGORYID" };
        string[] alCaptions = { "Category", "Sub Category" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        typeid = Convert.ToInt32(PhoenixCommonRegisters.GetHardCode(1, 148, "AUD"));
        docname = "Audit / Inspection";

        DataSet ds = PhoenixKPIRegisters.RankMappingSearch(General.GetNullableInteger(ddlsourcetype.SelectedValue.ToString())
         , General.GetNullableGuid(ddlcategory.SelectedValue.ToString())
         , General.GetNullableInteger(ddlscoretype.SelectedValue.ToString())
         , Int32.Parse(ViewState["PAGENUMBER"].ToString())
         , General.ShowRecords(null)
         , ref iRowCount
         , ref iTotalPageCount);



        General.SetPrintOptions("gvkpirankmapping", docname, alCaptions, alColumns, ds);
        gvkpirankmapping.DataSource = ds.Tables[0];


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ddlsourcetype_Changed(object sender, EventArgs e)
    {
        Bindcategory();
    }
    protected void ddlcategory_Changed(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvkpirankmapping.Rebind();
    }



    public void Bindcategory()
    {
        DataTable categorydt = PhoenixKPIRegisters.kpiCategoryList(General.GetNullableInteger(ddlsourcetype.SelectedValue.ToString()), General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        if (categorydt.Rows.Count > 0)
        {
            ddlcategory.Items.Clear();
            ddlcategory.DataSource = categorydt;
            ddlcategory.DataTextField = "FLDCATEGORYDNAME";
            ddlcategory.DataValueField = "FLDCATEGORYID";
            ddlcategory.DataBind();

        }
        ddlcategory.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }

    protected void gvkpirankmapping_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvkpirankmapping.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvkpirankmapping_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

          

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                RadComboBox ddlsubcategoryadd = (RadComboBox)e.Item.FindControl("ddlsubcategoryadd");
                CheckBoxList chk = (CheckBoxList)e.Item.FindControl("chkRankApplicableAdd");
                string RnList = "";
                string Ranklist = "";
                foreach (ListItem li in chk.Items)
                {
                    if (li.Selected)
                    {
                        RnList += li.Value + ",";
                    }
                }

                if (RnList != "")
                {
                    Ranklist = "," + RnList;
                }
                else
                {
                    ucError.ErrorMessage = "Applicable Rank Is Required";
                }

                PhoenixKPIRegisters.kpirankmappinginsert(null
                    , General.GetNullableGuid(ddlcategory.SelectedValue.ToString())
                    , General.GetNullableGuid(ddlsubcategoryadd.SelectedValue.ToString())
                    , General.GetNullableString(Ranklist)
                    , General.GetNullableInteger(ddlsourcetype.SelectedValue.ToString())
                    , General.GetNullableInteger(ddlscoretype.SelectedValue.ToString())
                    );

                BindData();
                gvkpirankmapping.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {

                CheckBoxList chk = (CheckBoxList)e.Item.FindControl("chkRankApplicableEdit");
                chk.Visible = true;
                string RList = "";
                string RankList = "";
                foreach (ListItem li in chk.Items)
                {
                    if (li.Selected)
                    {
                        RList += li.Value + ",";
                    }
                }

                if (RList != "")
                {
                    RankList = "," + RList;
                }
                else
                {
                    ucError.ErrorMessage = "RankList Is Required";
                }

                PhoenixKPIRegisters.kpirankmappinginsert(
                      General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblrankmappingid")).Text)
                    , null
                    , null
                    , General.GetNullableString(RankList)
                    , null
                    , null);

                BindData();
                gvkpirankmapping.Rebind();
            }
            if (e.CommandName.ToUpper() == "DELETE")
            {

                PhoenixKPIRegisters.kpirankmappingdelete(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblrankmappingid")).Text));
                BindData();
                gvkpirankmapping.Rebind();

            }
            if (e.CommandName == "Page")
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

    protected void gvkpirankmapping_ItemDataBound1(object sender, GridItemEventArgs e)
    {


        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            CheckBoxList chkUserGroupEdit = (CheckBoxList)e.Item.FindControl("chkRankApplicableEdit");
            if (chkUserGroupEdit != null)
            {
                chkUserGroupEdit.DataSource = PhoenixRegistersRank.ListRank();
                chkUserGroupEdit.DataTextField = "FLDRANKNAME";
                chkUserGroupEdit.DataValueField = "FLDRANKID";
                chkUserGroupEdit.DataBind();

                CheckBoxList chk = (CheckBoxList)e.Item.FindControl("chkRankApplicableEdit");
                if (chk != null)
                {
                    foreach (ListItem li in chk.Items)
                    {
                        string[] slist = drv["FLDRANKAPPLIES"].ToString().Split(',');
                        foreach (string s in slist)
                        {
                            if (li.Value.Equals(s))
                            {
                                li.Selected = true;
                            }
                        }
                    }
                }
            }

        }
        if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
        {
            RadLabel lblUserGroup = (RadLabel)e.Item.FindControl("lblRankApplicable");

            LinkButton ImgUserGroup = (LinkButton)e.Item.FindControl("ImglblRankApplicable");

            if (ImgUserGroup != null)
            {
                if (lblUserGroup != null)
                {
                    if (lblUserGroup.Text != "")
                    {
                        ImgUserGroup.Visible = true;
                        UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucRankApplicable");
                        if (uct != null)
                        {
                            uct.Position = ToolTipPosition.TopCenter;
                            uct.TargetControlId = ImgUserGroup.ClientID;
                            //ImgUserGroup.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                            //ImgUserGroup.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                        }
                    }
                    else
                        ImgUserGroup.Visible = false;
                }
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

            CheckBoxList chkRankAdd = (CheckBoxList)e.Item.FindControl("chkRankApplicableAdd");
            if (chkRankAdd != null)
            {
                chkRankAdd.DataSource = PhoenixRegistersRank.ListRank();
                chkRankAdd.DataTextField = "FLDRANKNAME";
                chkRankAdd.DataValueField = "FLDRANKID";
                chkRankAdd.DataBind();
                chkRankAdd.Enabled = true;
            }
            RadComboBox ddlsubcategoryadd = (RadComboBox)e.Item.FindControl("ddlsubcategoryadd");
            if (ddlsubcategoryadd != null)
            {
                DataTable categorydt = PhoenixKPIRegisters.kpiSubCategoryList(General.GetNullableGuid(ddlcategory.SelectedValue.ToString()), ddlcategory.SelectedItem.Text);
                if (categorydt.Rows.Count > 0)
                {
                    ddlsubcategoryadd.Items.Clear();
                    ddlsubcategoryadd.DataSource = categorydt;
                    ddlsubcategoryadd.DataTextField = "FLDCATEGORYDSUBNAME";
                    ddlsubcategoryadd.DataValueField = "FLDCATEGORYSUBID";
                    ddlsubcategoryadd.DataBind();

                }
                ddlsubcategoryadd.Items.Insert(0, new RadComboBoxItem("--Select--", ""));

            }


        }
    }

    protected void gvkpirankmapping_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
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
}
