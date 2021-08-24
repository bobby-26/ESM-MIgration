using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionChapter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionChapter.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvInspectionChapter')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");            
            MenuRegistersInspectionChapter.AccessRights = this.ViewState;
            MenuRegistersInspectionChapter.MenuList = toolbar.Show();            

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }

                BindDefaultData();
                gvInspectionChapter.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void InspectionType_Changed(object sender, EventArgs e)
    {
        if (ucInspectionType.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 148, "AUD") && ucInspectionCategory.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 144, "EXT"))
        {
            ucExternalAuditType.bind();
            ucExternalAuditType.SelectedHard = "";
        }

        ucInspection.DataSource = PhoenixInspection.ListInspectionByCompany(
                                        General.GetNullableInteger(ucInspectionType.SelectedHard)
                                        , General.GetNullableInteger(ucInspectionCategory.SelectedHard)
                                        , General.GetNullableInteger(ucExternalAuditType.SelectedHard)
                                        , 2
                                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        ucInspection.DataTextField = "FLDSHORTCODE";
        ucInspection.DataValueField = "FLDINSPECTIONID";
        ucInspection.DataBind();
        

        ExternalAuditType_Changed(ucExternalAuditType, new EventArgs());
        Inspection_Changed(ucInspection, new EventArgs());
    }

    protected void ExternalAuditType_Changed(object sender, EventArgs e)
    {
        if (ucInspectionType.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 148, "AUD") && ucInspectionCategory.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 144, "EXT"))
            ucExternalAuditType.Enabled = true;
        else
        {
            ucExternalAuditType.Enabled = false;
            ucExternalAuditType.SelectedHard = "";
        }
        ucInspection.DataSource = PhoenixInspection.ListInspectionByCompany(
                                        General.GetNullableInteger(ucInspectionType.SelectedHard)
                                        , General.GetNullableInteger(ucInspectionCategory.SelectedHard)
                                        , General.GetNullableInteger(ucExternalAuditType.SelectedHard)
                                        , 2
                                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        ucInspection.DataTextField = "FLDSHORTCODE";
        ucInspection.DataValueField = "FLDINSPECTIONID";
        ucInspection.DataBind();
        
        Inspection_Changed(ucInspection, new EventArgs());
    }

    protected void setDefaultExternalAuditType()
    {
        if (ucInspectionType.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 148, "AUD") && ucInspectionCategory.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 144, "EXT"))
        {
            ucExternalAuditType.bind();
            ucExternalAuditType.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 190, "INM");
        }
    }

    protected void Inspection_Changed(object sender, EventArgs e)
    {
        BindData();
        gvInspectionChapter.Rebind();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDINSPECTIONCATEGORYNAME", "FLDINSPECTIONNAME", "FLDCHAPTERNUMBER", "FLDCHAPTERNAME", "FLDDEFICIENCYCATEGORYNAME", "FLDSORTORDER" };
        string[] alCaptions = { "Category", "Name", "Chapter Number", "Chapter", "Deficiency Category", "Sort Order" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixInspectionChapter.InspectionChapterSearch(
            General.GetNullableInteger(ucInspectionType.SelectedHard)
            , General.GetNullableInteger(ucInspectionCategory.SelectedHard)
            , General.GetNullableGuid(ucInspection.SelectedValue)
            , ""
            , sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount
            , General.GetNullableInteger(ucExternalAuditType.SelectedHard));

        Response.AddHeader("Content-Disposition", "attachment; filename=InspectionChapter.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Chapter</h3></td>");
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

    protected void RegistersInspectionChapter_TabStripCommand(object sender, EventArgs e)
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

    private void BindDefaultData()
    {
        ViewState["defaulttype"] = General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 148, "INS"));
        ViewState["defaultcategory"] = General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 144, "EXT"));
        ucInspectionType.SelectedHard = ViewState["defaulttype"].ToString();
        ucInspectionType.bind();
        ucInspectionType.Enabled = true;
        ucExternalAuditType.bind();
        ucExternalAuditType.SelectedHard = "";

        ucInspection.DataSource = PhoenixInspection.ListInspectionByCompany(
                                        General.GetNullableInteger(ucInspectionType.SelectedHard) == null ? General.GetNullableInteger(ViewState["defaulttype"].ToString()) : General.GetNullableInteger(ucInspectionType.SelectedHard)
                                        , General.GetNullableInteger(ucInspectionCategory.SelectedHard) == null ? General.GetNullableInteger(ViewState["defaultcategory"].ToString()) : General.GetNullableInteger(ucInspectionCategory.SelectedHard)
                                        , General.GetNullableInteger(ucExternalAuditType.SelectedHard)
                                        , 2
                                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        ucInspection.DataTextField = "FLDSHORTCODE";
        ucInspection.DataValueField = "FLDINSPECTIONID";
        ucInspection.DataBind();
        
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDINSPECTIONCATEGORYNAME", "FLDINSPECTIONNAME", "FLDCHAPTERNUMBER", "FLDCHAPTERNAME", "FLDDEFICIENCYCATEGORYNAME", "FLDSORTORDER" };
        string[] alCaptions = { "Category", "Name", "Chapter Number", "Chapter", "Deficiency Category", "Sort Order" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixInspectionChapter.InspectionChapterSearch(
            General.GetNullableInteger(ucInspectionType.SelectedHard)
            , General.GetNullableInteger(ucInspectionCategory.SelectedHard)
            , General.GetNullableGuid(ucInspection.SelectedValue)
            , ""
            , sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvInspectionChapter.PageSize,
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableInteger(ucExternalAuditType.SelectedHard));

        General.SetPrintOptions("gvInspectionChapter", "Chapter", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvInspectionChapter.DataSource = ds;
            gvInspectionChapter.VirtualItemCount = iRowCount;
        }
        else
        {
            gvInspectionChapter.DataSource="";
        }
        ViewState["ROWCOUNT"] = iRowCount;
    }
    

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

    }
    private void InsertInspectionChapter(string inspectionid, string chapternumber, string chaptername, string sortorder, string deficiencycategory)
    {

        PhoenixInspectionChapter.InsertInspectionChapter(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , new Guid(inspectionid)
            , chapternumber
            , chaptername
            , int.Parse(sortorder)
            , General.GetNullableInteger(deficiencycategory));
    }

    private void UpdateInspectionChapter(string chapterid, string inspectionid, string chapternumber, string chaptername, string sortorder, string deficiencycategory)
    {
        PhoenixInspectionChapter.UpdateInspectionChapter(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , new Guid(chapterid)
            , new Guid(inspectionid)
            , chapternumber
            , chaptername
            , int.Parse(sortorder)
            , General.GetNullableInteger(deficiencycategory));

        ucStatus.Text = "Chapter information updated";
    }

    private bool IsValidInspectionChapter(string inspectionid, string chapternumber, string chaptername, string sortorder)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (inspectionid.Trim().Equals("Dummy") || inspectionid.Trim().Equals(""))
            ucError.ErrorMessage = "Inspection is required.";

        if (chapternumber.Trim().Equals(""))
            ucError.ErrorMessage = "Chapter Number is required.";

        if (chaptername.Trim().Equals(""))
            ucError.ErrorMessage = "Chapter Name is required.";

        if (sortorder.Trim().Equals(""))
            ucError.ErrorMessage = "Sort Order is required.";

        return (!ucError.IsError);
    }

    private void DeleteInspectionChapter(Guid chapterid)
    {
        PhoenixInspectionChapter.DeleteInspectionChapter(PhoenixSecurityContext.CurrentSecurityContext.UserCode, chapterid);
    }
    
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvInspectionChapter.Rebind();
    }
    
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvInspectionChapter_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidInspectionChapter(((RadComboBox)e.Item.FindControl("ucInspectionAdd")).SelectedValue
                    , ((RadTextBox)e.Item.FindControl("txtChapterNumberAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtChapterAdd")).Text
                    , ((UserControlMaskNumber)e.Item.FindControl("txtSortOrderAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                InsertInspectionChapter(
                       ((RadComboBox)e.Item.FindControl("ucInspectionAdd")).SelectedValue
                    , ((RadTextBox)e.Item.FindControl("txtChapterNumberAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtChapterAdd")).Text
                    , ((UserControlMaskNumber)e.Item.FindControl("txtSortOrderAdd")).Text
                    , ((UserControlQuick)e.Item.FindControl("ucCategoryAdd")).SelectedQuick);
                BindData();
                gvInspectionChapter.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteInspectionChapter(new Guid(((RadLabel)e.Item.FindControl("lblChapterId")).Text));
                BindData();
                gvInspectionChapter.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidInspectionChapter(((RadComboBox)e.Item.FindControl("ucInspectionEdit")).SelectedValue
                    , ((RadTextBox)e.Item.FindControl("txtChapterNumberEdit")).Text
                    , ((RadTextBox)e.Item.FindControl("txtChapterEdit")).Text
                    , ((UserControlMaskNumber)e.Item.FindControl("txtSortOrderEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateInspectionChapter(
                        ((RadLabel)e.Item.FindControl("lblChapterIdEdit")).Text
                        , ((RadComboBox)e.Item.FindControl("ucInspectionEdit")).SelectedValue
                        , ((RadTextBox) e.Item.FindControl("txtChapterNumberEdit")).Text
                        , ((RadTextBox) e.Item.FindControl("txtChapterEdit")).Text
                        , ((UserControlMaskNumber)e.Item.FindControl("txtSortOrderEdit")).Text
                        , ((UserControlQuick)e.Item.FindControl("ucCategoryEdit")).SelectedQuick);
                
                BindData();
                gvInspectionChapter.Rebind();
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

    protected void gvInspectionChapter_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInspectionChapter.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvInspectionChapter_ItemDataBound(object sender, GridItemEventArgs e)
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

            RadComboBox ucInspectionEdit = (RadComboBox)e.Item.FindControl("ucInspectionEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucInspectionEdit != null)
            {
                ucInspectionEdit.DataSource = PhoenixInspection.ListInspectionByCompany(
                                        General.GetNullableInteger(ucInspectionType.SelectedHard)
                                        , General.GetNullableInteger(ucInspectionCategory.SelectedHard)
                                        , General.GetNullableInteger(ucExternalAuditType.SelectedHard)
                                        , 2
                                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
                ucInspectionEdit.DataTextField = "FLDSHORTCODE";
                ucInspectionEdit.DataValueField = "FLDINSPECTIONID";
                ucInspectionEdit.DataBind();
                //  ucInspectionEdit.Items.Insert(0, new ListItem("--Select--", "Dummy"));

                if (drv["FLDINSPECTIONID"] != null)
                {
                    ucInspectionEdit.SelectedValue = drv["FLDINSPECTIONID"].ToString();
                    if (General.GetNullableGuid(ucInspection.SelectedValue) != null) ucInspectionEdit.Enabled = false;
                }
            }

            UserControlQuick ucCategoryEdit = (UserControlQuick)e.Item.FindControl("ucCategoryEdit");
            if (ucCategoryEdit != null)
            {
                ucCategoryEdit.QuickList = PhoenixRegistersQuick.ListQuick(1, 47);
                ucCategoryEdit.DataBind();
                ucCategoryEdit.SelectedQuick = drv["FLDDEFICIENCYCATEGORY"].ToString();
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

            RadComboBox ucInspectionAdd = (RadComboBox)e.Item.FindControl("ucInspectionAdd");
            if (ucInspectionAdd != null)
            {
                ucInspectionAdd.DataSource = PhoenixInspection.ListInspectionByCompany(
                                        General.GetNullableInteger(ucInspectionType.SelectedHard)
                                        , General.GetNullableInteger(ucInspectionCategory.SelectedHard)
                                        , General.GetNullableInteger(ucExternalAuditType.SelectedHard)
                                        , 2
                                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
                ucInspectionAdd.DataTextField = "FLDSHORTCODE";
                ucInspectionAdd.DataValueField = "FLDINSPECTIONID";
                ucInspectionAdd.DataBind();

                ucInspectionAdd.SelectedValue = ucInspection.SelectedValue;
                if (General.GetNullableGuid(ucInspection.SelectedValue) != null) ucInspectionAdd.Enabled = false;
            }

            UserControlQuick ucCategoryAdd = (UserControlQuick)e.Item.FindControl("ucCategoryAdd");

            if (ucCategoryAdd != null)
            {
                ucCategoryAdd.QuickList = PhoenixRegistersQuick.ListQuick(1, 47);
                ucCategoryAdd.DataBind();
            }
        }
    }

    protected void gvInspectionChapter_SortCommand(object sender, GridSortCommandEventArgs e)
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
}
