using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;

public partial class CrewOffshoreDocument : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        if (!IsPostBack)
        {
            UserControlDocuments dc = gvOffshoreDocumentLicence.FooterRow.FindControl("ddlLicenceAdd") as UserControlDocuments;
            dc.DocumentList = PhoenixRegistersDocumentLicence.ListDocumentLicence(null, null, 1, null, null);            
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreDocument.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvOffshoreDocumentLicence')", "Print Grid", "icon_print.png", "PRINT");
            MenuOffshoreDocumentLicence.AccessRights = this.ViewState;
            MenuOffshoreDocumentLicence.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreDocument.aspx", "Export to Excel", "icon_xls.png", "ExcelC");
            toolbar.AddImageLink("javascript:CallPrint('gvOffshoreDocumentCourse')", "Print Grid", "icon_print.png", "PRINTC");
            MenuOffshoreDocumentCourse.AccessRights = this.ViewState;
            MenuOffshoreDocumentCourse.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreDocument.aspx", "Export to Excel", "icon_xls.png", "ExcelO");
            toolbar.AddImageLink("javascript:CallPrint('gvOffshoreDocumentOther')", "Print Grid", "icon_print.png", "PRINTO");
            MenuOffshoreDocumentOther.AccessRights = this.ViewState;
            MenuOffshoreDocumentOther.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreDocument.aspx", "Export to Excel", "icon_xls.png", "ExcelU");
            toolbar.AddImageLink("javascript:CallPrint('gvOffshoreDocumentUser')", "Print Grid", "icon_print.png", "PRINTU");
            MenuOffshoreUserDocument.AccessRights = this.ViewState;
            MenuOffshoreUserDocument.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreDocument.aspx", "Export to Excel", "icon_xls.png", "ExcelE");
            toolbar.AddImageLink("javascript:CallPrint('gvOffshoreDocumentExp')", "Print Grid", "icon_print.png", "PRINTE");
            MenuOffshoreDocumentExp.AccessRights = this.ViewState;
            MenuOffshoreDocumentExp.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreDocument.aspx", "Export to Excel", "icon_xls.png", "ExcelT");
            toolbar.AddImageLink("javascript:CallPrint('gvOffshoreDocumentTravel')", "Print Grid", "icon_print.png", "PRINTT");
            MenuOffshoreDocumentTravel.AccessRights = this.ViewState;
            MenuOffshoreDocumentTravel.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["PAGENUMBERC"] = 1;
                ViewState["PAGENUMBERO"] = 1;
                ViewState["PAGENUMBERU"] = 1;
                ViewState["PAGENUMBERE"] = 1;
                ViewState["PAGENUMBERT"] = 1;

                BindData();
                SetPageNavigator();

                BindDataCourse();
                SetPageNavigatorC();

                BindDataOther();
                SetPageNavigatorO();

                BindDataUserDoc();
                SetPageNavigatorU();

                BindDataExp();
                SetPageNavigatorE();

                BindDataTravel();
                SetPageNavigatorT();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindOffshoreStages(DropDownList ddl)
    {
        ddl.Items.Clear();
        ddl.DataSource = PhoenixCrewOffshoreStage.ListOffshoreStage(null, null);
        ddl.DataTextField = "FLDSTAGE";
        ddl.DataValueField = "FLDSTAGEID";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSTAGE", "FLDDOCUMENTNAME", "FLDMANDATORYYNNAME", "FLDWAIVERYNNNAME", "FLDGROUPNAME" };
        string[] alCaptions = { "Stage", "Document", "Mandatory Y/N", "Waiver Y/N", "User group to allow waiver" };        

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixCrewOffshoreDocument.SearchOffshoreDocument(1,null, 
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=OffshoreDocumentCOC.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>COC</h3></td>");
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
        foreach (DataRow dr in dt.Rows)
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

    protected void MenuOffshoreDocumentLicence_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
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
        string[] alColumns = { "FLDSTAGE", "FLDDOCUMENTNAME", "FLDMANDATORYYNNAME", "FLDWAIVERYNNNAME", "FLDGROUPNAME" };
        string[] alCaptions = { "Stage", "Document", "Mandatory Y/N", "Waiver Y/N", "User group to allow waiver" };
        
        DataTable dt = PhoenixCrewOffshoreDocument.SearchOffshoreDocument(1,null,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshoreDocumentLicence", "COC", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvOffshoreDocumentLicence.DataSource = ds;
            gvOffshoreDocumentLicence.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvOffshoreDocumentLicence);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        SetPageNavigator();
    }

    protected void gvOffshoreDocumentLicence_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            BindData();
            ((DropDownList)_gridView.Rows[de.NewEditIndex].FindControl("ddlStageEdit")).Focus();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOffshoreDocumentLicence_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvOffshoreDocumentLicence_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dr = (DataRowView)e.Row.DataItem;
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            DropDownList ddlStageEdit = (DropDownList)e.Row.FindControl("ddlStageEdit");
            if(ddlStageEdit != null)
            {
                BindOffshoreStages(ddlStageEdit);
                ddlStageEdit.SelectedValue = dr["FLDSTAGEID"].ToString();
            }

            UserControlDocuments ddlLicenceEdit = (UserControlDocuments)e.Row.FindControl("ddlLicenceEdit");
            if (ddlLicenceEdit != null) ddlLicenceEdit.SelectedDocument = dr["FLDDOCUMENTID"].ToString();

            CheckBoxList chkUserGroupEdit = (CheckBoxList)e.Row.FindControl("chkUserGroupEdit");
            CheckBox chkMandatoryYNEdit = (CheckBox)e.Row.FindControl("chkMandatoryYNEdit");
            CheckBox chkWaiverYNEdit = (CheckBox)e.Row.FindControl("chkWaiverYNEdit");
            if (chkMandatoryYNEdit != null)
            {
                if (chkMandatoryYNEdit.Checked)
                {
                    if (chkWaiverYNEdit != null) chkWaiverYNEdit.Enabled = true;
                }
                else
                {
                    if (chkWaiverYNEdit != null) chkWaiverYNEdit.Enabled = false;
                }
            }

            if (chkWaiverYNEdit != null)
            {
                if (chkWaiverYNEdit.Checked)
                {
                    if (chkUserGroupEdit != null) chkUserGroupEdit.Enabled = true;
                }
                else
                {
                    if (chkUserGroupEdit != null) chkUserGroupEdit.Enabled = false;
                }
            }

            if (chkUserGroupEdit != null)
            {
                chkUserGroupEdit.DataSource = SessionUtil.UserGroupList();
                chkUserGroupEdit.DataTextField = "FLDGROUPNAME";
                chkUserGroupEdit.DataValueField = "FLDGROUPCODE";
                chkUserGroupEdit.DataBind();

                CheckBoxList chk = (CheckBoxList)e.Row.FindControl("chkUserGroupEdit");
                foreach (ListItem li in chk.Items)
                {
                    string[] slist = dr["FLDUSERGROUPTOALLOWWAIVER"].ToString().Split(',');
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
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton ab = (ImageButton)e.Row.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }

            DropDownList ddlStageAdd = (DropDownList)e.Row.FindControl("ddlStageAdd");
            if (ddlStageAdd != null)
            {
                BindOffshoreStages(ddlStageAdd);
            }

            CheckBoxList chkUserGroupAdd = (CheckBoxList)e.Row.FindControl("chkUserGroupAdd");
            if (chkUserGroupAdd != null)
            {
                chkUserGroupAdd.DataSource = SessionUtil.UserGroupList();
                chkUserGroupAdd.DataTextField = "FLDGROUPNAME";
                chkUserGroupAdd.DataValueField = "FLDGROUPCODE";
                chkUserGroupAdd.DataBind();
            }
        }
    }

    protected void gvOffshoreDocumentLicence_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                CheckBoxList chk = (CheckBoxList)_gridView.FooterRow.FindControl("chkUserGroupAdd");
                string UGList = "";
                string UserGroupList = "";
                foreach (ListItem li in chk.Items)
                {
                    if (li.Selected)
                    {
                        UGList += li.Value + ",";
                    }
                }

                if (UGList != "")
                {
                    UserGroupList = "," + UGList;
                }

                if (!IsValidData(((DropDownList)_gridView.FooterRow.FindControl("ddlStageAdd")).SelectedValue,
                    ((UserControlDocuments)_gridView.FooterRow.FindControl("ddlLicenceAdd")).SelectedDocument,
                    ((CheckBox)_gridView.FooterRow.FindControl("chkWaiverYNAdd")).Checked ? "1" : "0",
                    UserGroupList))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreDocument.InsertOffshoreDocument(
                    int.Parse(((DropDownList)_gridView.FooterRow.FindControl("ddlStageAdd")).SelectedValue)
                    ,int.Parse(((UserControlDocuments)_gridView.FooterRow.FindControl("ddlLicenceAdd")).SelectedDocument)
                    ,1
                    ,General.GetNullableInteger(((CheckBox)_gridView.FooterRow.FindControl("chkMandatoryYNAdd")).Checked ? "1" : "0")
                    ,General.GetNullableInteger(((CheckBox)_gridView.FooterRow.FindControl("chkWaiverYNAdd")).Checked ? "1" : "0")
                    , General.GetNullableString(UserGroupList));

                BindData();
                ((DropDownList)_gridView.FooterRow.FindControl("ddlStageAdd")).Focus();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                int offshoredocid = int.Parse(_gridView.DataKeys[nCurrentRow].Value.ToString());
                PhoenixCrewOffshoreDocument.DeleteOffshoreDocument(offshoredocid);
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                CheckBoxList chk = (CheckBoxList)_gridView.Rows[nCurrentRow].FindControl("chkUserGroupEdit");
                string UGList = "";
                string UserGroupList = "";
                foreach (ListItem li in chk.Items)
                {
                    if (li.Selected)
                    {
                        UGList += li.Value + ",";
                    }
                }

                if (UGList != "")
                {
                    UserGroupList = "," + UGList;
                }

                if (!IsValidData(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlStageEdit")).SelectedValue,
                    ((UserControlDocuments)_gridView.Rows[nCurrentRow].FindControl("ddlLicenceEdit")).SelectedDocument,
                    ((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkWaiverYNEdit")).Checked ? "1" : "0",
                    UserGroupList))
                {
                    ucError.Visible = true;
                    return;
                }
                int offshoredocid = int.Parse(_gridView.DataKeys[nCurrentRow].Value.ToString());

                PhoenixCrewOffshoreDocument.UpdateOffshoreDocument(offshoredocid
                    ,int.Parse(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlStageEdit")).SelectedValue)
                    ,int.Parse(((UserControlDocuments)_gridView.Rows[nCurrentRow].FindControl("ddlLicenceEdit")).SelectedDocument)
                    ,1
                    ,General.GetNullableInteger(((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkMandatoryYNEdit")).Checked ? "1" : "0")
                    ,General.GetNullableInteger(((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkWaiverYNEdit")).Checked ? "1" : "0")
                    , General.GetNullableString(UserGroupList));

                _gridView.EditIndex = -1;
                BindData();

            }
            SetPageNavigator();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOffshoreDocumentLicence_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOffshoreDocumentLicence_OnRowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }
    private bool IsValidData(string stageid, string documentid, string waiveryn, string usergroup)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(stageid) == null)
            ucError.ErrorMessage = "Stage is required.";

        if (General.GetNullableInteger(documentid) == null)
            ucError.ErrorMessage = "Document is required.";

        if (General.GetNullableInteger(waiveryn) != null && waiveryn.Equals("1"))
        {
            if (usergroup == "")
                ucError.ErrorMessage = "User group is required.";
        }

        return (!ucError.IsError);
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvOffshoreDocumentLicence.SelectedIndex = -1;
        gvOffshoreDocumentLicence.EditIndex = -1;
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvOffshoreDocumentLicence.SelectedIndex = -1;
        gvOffshoreDocumentLicence.EditIndex = -1;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindData();
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvOffshoreDocumentLicence.SelectedIndex = -1;
        gvOffshoreDocumentLicence.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
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

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void chkMandatoryYNEdit_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        GridViewRow gvRow = (GridViewRow)cb.Parent.Parent;
        CheckBox chkWaiverYNEdit = (CheckBox)gvRow.FindControl("chkWaiverYNEdit");
        CheckBoxList chkUserGroupEdit = (CheckBoxList)gvRow.FindControl("chkUserGroupEdit");
        if (chkWaiverYNEdit != null)
        {
            if (cb.Checked)
            {
                chkWaiverYNEdit.Enabled = true;
            }
            else
            {
                chkWaiverYNEdit.Checked = false;
                chkWaiverYNEdit.Enabled = false;

                chkUserGroupEdit.SelectedIndex = -1;
                chkUserGroupEdit.Enabled = false;
            }
        }
    }

    protected void chkMandatoryYNAdd_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        GridViewRow gvRow = (GridViewRow)cb.Parent.Parent;
        CheckBox chkWaiverYNAdd = (CheckBox)gvRow.FindControl("chkWaiverYNAdd");
        CheckBoxList chkUserGroupAdd = (CheckBoxList)gvRow.FindControl("chkUserGroupAdd");
        if (chkWaiverYNAdd != null)
        {
            if (cb.Checked)
                chkWaiverYNAdd.Enabled = true;
            else
            {
                chkWaiverYNAdd.Checked = false;
                chkWaiverYNAdd.Enabled = false;

                chkUserGroupAdd.SelectedIndex = -1;
                chkUserGroupAdd.Enabled = false;
            }
        }
    }

    protected void chkWaiverYNEdit_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        GridViewRow gvRow = (GridViewRow)cb.Parent.Parent;
        CheckBoxList chkUserGroupEdit = (CheckBoxList)gvRow.FindControl("chkUserGroupEdit");
        if (chkUserGroupEdit != null)
        {
            if (cb.Checked)
            {
                chkUserGroupEdit.Enabled = true;
            }
            else
            {
                chkUserGroupEdit.SelectedIndex = -1;
                chkUserGroupEdit.Enabled = false;
            }
        }
    }

    protected void chkWaiverYNAdd_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        GridViewRow gvRow = (GridViewRow)cb.Parent.Parent;
        CheckBoxList chkUserGroupAdd = (CheckBoxList)gvRow.FindControl("chkUserGroupAdd");

        if (chkUserGroupAdd != null)
        {
            if (cb.Checked)
            {
                chkUserGroupAdd.Enabled = true;
            }
            else
            {
                chkUserGroupAdd.SelectedIndex = -1;
                chkUserGroupAdd.Enabled = false;
            }
        }
    }

    // Course

    protected void BindSTCWCourses(DropDownList ddl)
    {
        string doctype = PhoenixCommonRegisters.GetHardCode(1, 103, "6"); // STCW
        ddl.Items.Clear();
        ddl.DataSource = PhoenixRegistersDocumentCourse.ListDocumentCourse(General.GetNullableInteger(doctype));
        ddl.DataTextField = "FLDDOCUMENTNAME";
        ddl.DataValueField = "FLDDOCUMENTID";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void MenuOffshoreDocumentCourse_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCELC"))
            {
                ShowExcelCourse();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDataCourse()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSTAGE", "FLDDOCUMENTNAME", "FLDMANDATORYYNNAME", "FLDWAIVERYNNNAME", "FLDGROUPNAME" };
        string[] alCaptions = { "Stage", "Document", "Mandatory Y/N", "Waiver Y/N", "User group to allow waiver" };

        DataTable dt = PhoenixCrewOffshoreDocument.SearchOffshoreDocument(2, null,
                Int32.Parse(ViewState["PAGENUMBERC"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshoreDocumentCourse", "STCW", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvOffshoreDocumentCourse.DataSource = ds;
            gvOffshoreDocumentCourse.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvOffshoreDocumentCourse);
        }

        ViewState["ROWCOUNTC"] = iRowCount;
        ViewState["TOTALPAGECOUNTC"] = iTotalPageCount;
        SetPageNavigatorC();
    }

    protected void ShowExcelCourse()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSTAGE", "FLDDOCUMENTNAME", "FLDMANDATORYYNNAME", "FLDWAIVERYNNNAME", "FLDGROUPNAME" };
        string[] alCaptions = { "Stage", "Document", "Mandatory Y/N", "Waiver Y/N", "User group to allow waiver" };

        if (ViewState["ROWCOUNTC"] == null || Int32.Parse(ViewState["ROWCOUNTC"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNTC"].ToString());

        DataTable dt = PhoenixCrewOffshoreDocument.SearchOffshoreDocument(2, null,
                Int32.Parse(ViewState["PAGENUMBERC"].ToString()),
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=OffshoreDocumentSTCW.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>STCW</h3></td>");
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
        foreach (DataRow dr in dt.Rows)
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

    protected void gvOffshoreDocumentCourse_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dr = (DataRowView)e.Row.DataItem;
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            DropDownList ddlStageEdit = (DropDownList)e.Row.FindControl("ddlStageEdit");
            if (ddlStageEdit != null)
            {
                BindOffshoreStages(ddlStageEdit);
                ddlStageEdit.SelectedValue = dr["FLDSTAGEID"].ToString();
            }

            DropDownList ddlCourseEdit = (DropDownList)e.Row.FindControl("ddlCourseEdit");
            if (ddlCourseEdit != null)
            {
                BindSTCWCourses(ddlCourseEdit);
                ddlCourseEdit.SelectedValue = dr["FLDDOCUMENTID"].ToString();
            }

            CheckBoxList chkUserGroupEdit = (CheckBoxList)e.Row.FindControl("chkUserGroupEdit");
            CheckBox chkMandatoryYNEdit = (CheckBox)e.Row.FindControl("chkMandatoryYNEdit");
            CheckBox chkWaiverYNEdit = (CheckBox)e.Row.FindControl("chkWaiverYNEdit");
            if (chkMandatoryYNEdit != null)
            {
                if (chkMandatoryYNEdit.Checked)
                {
                    if (chkWaiverYNEdit != null) chkWaiverYNEdit.Enabled = true;
                }
                else
                {
                    if (chkWaiverYNEdit != null) chkWaiverYNEdit.Enabled = false;
                }
            }

            if (chkWaiverYNEdit != null)
            {
                if (chkWaiverYNEdit.Checked)
                {
                    if (chkUserGroupEdit != null) chkUserGroupEdit.Enabled = true;
                }
                else
                {
                    if (chkUserGroupEdit != null) chkUserGroupEdit.Enabled = false;
                }
            }

            if (chkUserGroupEdit != null)
            {
                chkUserGroupEdit.DataSource = SessionUtil.UserGroupList();
                chkUserGroupEdit.DataTextField = "FLDGROUPNAME";
                chkUserGroupEdit.DataValueField = "FLDGROUPCODE";
                chkUserGroupEdit.DataBind();

                CheckBoxList chk = (CheckBoxList)e.Row.FindControl("chkUserGroupEdit");
                foreach (ListItem li in chk.Items)
                {
                    string[] slist = dr["FLDUSERGROUPTOALLOWWAIVER"].ToString().Split(',');
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
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton ab = (ImageButton)e.Row.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }

            DropDownList ddlStageAdd = (DropDownList)e.Row.FindControl("ddlStageAdd");
            if (ddlStageAdd != null)
            {
                BindOffshoreStages(ddlStageAdd);
            }

            CheckBoxList chkUserGroupAdd = (CheckBoxList)e.Row.FindControl("chkUserGroupAdd");
            if (chkUserGroupAdd != null)
            {
                chkUserGroupAdd.DataSource = SessionUtil.UserGroupList();
                chkUserGroupAdd.DataTextField = "FLDGROUPNAME";
                chkUserGroupAdd.DataValueField = "FLDGROUPCODE";
                chkUserGroupAdd.DataBind();
            }

            DropDownList ddlCourseAdd = (DropDownList)e.Row.FindControl("ddlCourseAdd");
            if (ddlCourseAdd != null) BindSTCWCourses(ddlCourseAdd);
        }
    }

    protected void gvOffshoreDocumentCourse_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADDC"))
            {
                CheckBoxList chk = (CheckBoxList)_gridView.FooterRow.FindControl("chkUserGroupAdd");
                string UGList = "";
                string UserGroupList = "";
                foreach (ListItem li in chk.Items)
                {
                    if (li.Selected)
                    {
                        UGList += li.Value + ",";
                    }
                }

                if (UGList != "")
                {
                    UserGroupList = "," + UGList;
                }

                if (!IsValidData(((DropDownList)_gridView.FooterRow.FindControl("ddlStageAdd")).SelectedValue,
                    ((DropDownList)_gridView.FooterRow.FindControl("ddlCourseAdd")).SelectedValue,
                    ((CheckBox)_gridView.FooterRow.FindControl("chkWaiverYNAdd")).Checked ? "1" : "0",
                    UserGroupList))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreDocument.InsertOffshoreDocument(
                    int.Parse(((DropDownList)_gridView.FooterRow.FindControl("ddlStageAdd")).SelectedValue)
                    , int.Parse(((DropDownList)_gridView.FooterRow.FindControl("ddlCourseAdd")).SelectedValue)
                    , 2
                    , General.GetNullableInteger(((CheckBox)_gridView.FooterRow.FindControl("chkMandatoryYNAdd")).Checked ? "1" : "0")
                    , General.GetNullableInteger(((CheckBox)_gridView.FooterRow.FindControl("chkWaiverYNAdd")).Checked ? "1" : "0")
                    , General.GetNullableString(UserGroupList));

                BindDataCourse();
                ((DropDownList)_gridView.FooterRow.FindControl("ddlStageAdd")).Focus();
            }

            else if (e.CommandName.ToUpper().Equals("DELETEC"))
            {
                int offshoredocid = int.Parse(_gridView.DataKeys[nCurrentRow].Value.ToString());
                PhoenixCrewOffshoreDocument.DeleteOffshoreDocument(offshoredocid);
                BindDataCourse();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATEC"))
            {
                CheckBoxList chk = (CheckBoxList)_gridView.Rows[nCurrentRow].FindControl("chkUserGroupEdit");
                string UGList = "";
                string UserGroupList = "";
                foreach (ListItem li in chk.Items)
                {
                    if (li.Selected)
                    {
                        UGList += li.Value + ",";
                    }
                }

                if (UGList != "")
                {
                    UserGroupList = "," + UGList;
                }

                if (!IsValidData(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlStageEdit")).SelectedValue,
                    ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlCourseEdit")).SelectedValue,
                    ((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkWaiverYNEdit")).Checked ? "1" : "0",
                    UserGroupList))
                {
                    ucError.Visible = true;
                    return;
                }
                int offshoredocid = int.Parse(_gridView.DataKeys[nCurrentRow].Value.ToString());

                PhoenixCrewOffshoreDocument.UpdateOffshoreDocument(offshoredocid
                    , int.Parse(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlStageEdit")).SelectedValue)
                    , int.Parse(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlCourseEdit")).SelectedValue)
                    , 2
                    , General.GetNullableInteger(((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkMandatoryYNEdit")).Checked ? "1" : "0")
                    , General.GetNullableInteger(((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkWaiverYNEdit")).Checked ? "1" : "0")
                    , General.GetNullableString(UserGroupList));

                _gridView.EditIndex = -1;
                BindDataCourse();

            }
            else if (e.CommandName.ToUpper().Equals("CANCELC"))
            {
                _gridView.EditIndex = -1;
                BindDataCourse();
            }
            else if (e.CommandName.ToUpper().Equals("EDITC"))
            {
                _gridView.EditIndex = nCurrentRow;
                _gridView.SelectedIndex = nCurrentRow;
                BindDataCourse();
                ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlStageEdit")).Focus();
            }
            SetPageNavigatorC();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdGoC_Click(object sender, EventArgs e)
    {
        int result;
        gvOffshoreDocumentCourse.SelectedIndex = -1;
        gvOffshoreDocumentCourse.EditIndex = -1;
        if (Int32.TryParse(txtnopageC.Text, out result))
        {
            ViewState["PAGENUMBERC"] = Int32.Parse(txtnopageC.Text);

            if ((int)ViewState["TOTALPAGECOUNTC"] < Int32.Parse(txtnopageC.Text))
                ViewState["PAGENUMBERC"] = ViewState["TOTALPAGECOUNTC"];


            if (0 >= Int32.Parse(txtnopageC.Text))
                ViewState["PAGENUMBERC"] = 1;

            if ((int)ViewState["PAGENUMBERC"] == 0)
                ViewState["PAGENUMBERC"] = 1;

            txtnopageC.Text = ViewState["PAGENUMBERC"].ToString();
        }
        BindDataCourse();
        SetPageNavigatorC();
    }

    protected void PagerButtonClickC(object sender, CommandEventArgs ce)
    {
        gvOffshoreDocumentCourse.SelectedIndex = -1;
        gvOffshoreDocumentCourse.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBERC"] = (int)ViewState["PAGENUMBERC"] - 1;
        else
            ViewState["PAGENUMBERC"] = (int)ViewState["PAGENUMBERC"] + 1;

        BindDataCourse();
        SetPageNavigatorC();
    }

    private void SetPageNavigatorC()
    {
        cmdPreviousC.Enabled = IsPreviousEnabledC();
        cmdNextC.Enabled = IsNextEnabledC();
        lblPagenumberC.Text = "Page " + ViewState["PAGENUMBERC"].ToString();
        lblPagesC.Text = " of " + ViewState["TOTALPAGECOUNTC"].ToString() + " Pages. ";
        lblRecordsC.Text = "(" + ViewState["ROWCOUNTC"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabledC()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERC"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTC"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabledC()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERC"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTC"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    // Other documents

    protected void MenuOffshoreDocumentOther_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCELO"))
            {
                ShowExcelOther();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDataOther()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSTAGE", "FLDDOCUMENTNAME", "FLDMANDATORYYNNAME", "FLDWAIVERYNNNAME", "FLDGROUPNAME" };
        string[] alCaptions = { "Stage", "Document", "Mandatory Y/N", "Waiver Y/N", "User group to allow waiver" };

        DataTable dt = PhoenixCrewOffshoreDocument.SearchOffshoreDocument(3, null,
                Int32.Parse(ViewState["PAGENUMBERO"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshoreDocumentOther", "Other documents", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvOffshoreDocumentOther.DataSource = ds;
            gvOffshoreDocumentOther.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvOffshoreDocumentOther);
        }

        ViewState["ROWCOUNTO"] = iRowCount;
        ViewState["TOTALPAGECOUNTO"] = iTotalPageCount;
        SetPageNavigatorO();
    }

    protected void ShowExcelOther()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSTAGE", "FLDDOCUMENTNAME", "FLDMANDATORYYNNAME", "FLDWAIVERYNNNAME", "FLDGROUPNAME" };
        string[] alCaptions = { "Stage", "Document", "Mandatory Y/N", "Waiver Y/N", "User group to allow waiver" };

        if (ViewState["ROWCOUNTO"] == null || Int32.Parse(ViewState["ROWCOUNTO"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNTO"].ToString());

        DataTable dt = PhoenixCrewOffshoreDocument.SearchOffshoreDocument(3, null,
                Int32.Parse(ViewState["PAGENUMBERO"].ToString()),
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=OffshoreDocumentOther.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Other documents</h3></td>");
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
        foreach (DataRow dr in dt.Rows)
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

    protected void gvOffshoreDocumentOther_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dr = (DataRowView)e.Row.DataItem;
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            DropDownList ddlStageEdit = (DropDownList)e.Row.FindControl("ddlStageEdit");
            if (ddlStageEdit != null)
            {
                BindOffshoreStages(ddlStageEdit);
                ddlStageEdit.SelectedValue = dr["FLDSTAGEID"].ToString();
            }

            UserControlDocumentType ucDocumentOtherEdit = (UserControlDocumentType)e.Row.FindControl("ucDocumentOtherEdit");
            if (ucDocumentOtherEdit != null) ucDocumentOtherEdit.SelectedDocumentType = dr["FLDDOCUMENTID"].ToString();

            CheckBoxList chkUserGroupEdit = (CheckBoxList)e.Row.FindControl("chkUserGroupEdit");
            CheckBox chkMandatoryYNEdit = (CheckBox)e.Row.FindControl("chkMandatoryYNEdit");
            CheckBox chkWaiverYNEdit = (CheckBox)e.Row.FindControl("chkWaiverYNEdit");
            if (chkMandatoryYNEdit != null)
            {
                if (chkMandatoryYNEdit.Checked)
                {
                    if (chkWaiverYNEdit != null) chkWaiverYNEdit.Enabled = true;
                }
                else
                {
                    if (chkWaiverYNEdit != null) chkWaiverYNEdit.Enabled = false;
                }
            }

            if (chkWaiverYNEdit != null)
            {
                if (chkWaiverYNEdit.Checked)
                {
                    if (chkUserGroupEdit != null) chkUserGroupEdit.Enabled = true;
                }
                else
                {
                    if (chkUserGroupEdit != null) chkUserGroupEdit.Enabled = false;
                }
            }

            if (chkUserGroupEdit != null)
            {
                chkUserGroupEdit.DataSource = SessionUtil.UserGroupList();
                chkUserGroupEdit.DataTextField = "FLDGROUPNAME";
                chkUserGroupEdit.DataValueField = "FLDGROUPCODE";
                chkUserGroupEdit.DataBind();

                CheckBoxList chk = (CheckBoxList)e.Row.FindControl("chkUserGroupEdit");
                foreach (ListItem li in chk.Items)
                {
                    string[] slist = dr["FLDUSERGROUPTOALLOWWAIVER"].ToString().Split(',');
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
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton ab = (ImageButton)e.Row.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }

            DropDownList ddlStageAdd = (DropDownList)e.Row.FindControl("ddlStageAdd");
            if (ddlStageAdd != null)
            {
                BindOffshoreStages(ddlStageAdd);
            }

            CheckBoxList chkUserGroupAdd = (CheckBoxList)e.Row.FindControl("chkUserGroupAdd");
            if (chkUserGroupAdd != null)
            {
                chkUserGroupAdd.DataSource = SessionUtil.UserGroupList();
                chkUserGroupAdd.DataTextField = "FLDGROUPNAME";
                chkUserGroupAdd.DataValueField = "FLDGROUPCODE";
                chkUserGroupAdd.DataBind();
            }
        }
    }

    protected void gvOffshoreDocumentOther_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADDO"))
            {
                CheckBoxList chk = (CheckBoxList)_gridView.FooterRow.FindControl("chkUserGroupAdd");
                string UGList = "";
                string UserGroupList = "";
                foreach (ListItem li in chk.Items)
                {
                    if (li.Selected)
                    {
                        UGList += li.Value + ",";
                    }
                }

                if (UGList != "")
                {
                    UserGroupList = "," + UGList;
                }

                if (!IsValidData(((DropDownList)_gridView.FooterRow.FindControl("ddlStageAdd")).SelectedValue,
                    ((UserControlDocumentType)_gridView.FooterRow.FindControl("ucDocumentOtherAdd")).SelectedDocumentType,
                    ((CheckBox)_gridView.FooterRow.FindControl("chkWaiverYNAdd")).Checked ? "1" : "0",
                    UserGroupList))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreDocument.InsertOffshoreDocument(
                    int.Parse(((DropDownList)_gridView.FooterRow.FindControl("ddlStageAdd")).SelectedValue)
                    , int.Parse(((UserControlDocumentType)_gridView.FooterRow.FindControl("ucDocumentOtherAdd")).SelectedDocumentType)
                    , 3
                    , General.GetNullableInteger(((CheckBox)_gridView.FooterRow.FindControl("chkMandatoryYNAdd")).Checked ? "1" : "0")
                    , General.GetNullableInteger(((CheckBox)_gridView.FooterRow.FindControl("chkWaiverYNAdd")).Checked ? "1" : "0")
                    , General.GetNullableString(UserGroupList));

                BindDataOther();
                ((DropDownList)_gridView.FooterRow.FindControl("ddlStageAdd")).Focus();
            }

            else if (e.CommandName.ToUpper().Equals("DELETEO"))
            {
                int offshoredocid = int.Parse(_gridView.DataKeys[nCurrentRow].Value.ToString());
                PhoenixCrewOffshoreDocument.DeleteOffshoreDocument(offshoredocid);
                BindDataOther();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATEO"))
            {
                CheckBoxList chk = (CheckBoxList)_gridView.Rows[nCurrentRow].FindControl("chkUserGroupEdit");
                string UGList = "";
                string UserGroupList = "";
                foreach (ListItem li in chk.Items)
                {
                    if (li.Selected)
                    {
                        UGList += li.Value + ",";
                    }
                }

                if (UGList != "")
                {
                    UserGroupList = "," + UGList;
                }

                if (!IsValidData(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlStageEdit")).SelectedValue,
                    ((UserControlDocumentType)_gridView.Rows[nCurrentRow].FindControl("ucDocumentOtherEdit")).SelectedDocumentType,
                    ((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkWaiverYNEdit")).Checked ? "1" : "0",
                    UserGroupList))
                {
                    ucError.Visible = true;
                    return;
                }
                int offshoredocid = int.Parse(_gridView.DataKeys[nCurrentRow].Value.ToString());

                PhoenixCrewOffshoreDocument.UpdateOffshoreDocument(offshoredocid
                    , int.Parse(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlStageEdit")).SelectedValue)
                    , int.Parse(((UserControlDocumentType)_gridView.Rows[nCurrentRow].FindControl("ucDocumentOtherEdit")).SelectedDocumentType)
                    , 3
                    , General.GetNullableInteger(((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkMandatoryYNEdit")).Checked ? "1" : "0")
                    , General.GetNullableInteger(((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkWaiverYNEdit")).Checked ? "1" : "0")
                    , General.GetNullableString(UserGroupList));

                _gridView.EditIndex = -1;
                BindDataOther();

            }
            else if (e.CommandName.ToUpper().Equals("CANCELO"))
            {
                _gridView.EditIndex = -1;
                BindDataOther();
            }
            else if (e.CommandName.ToUpper().Equals("EDITO"))
            {
                _gridView.EditIndex = nCurrentRow;
                _gridView.SelectedIndex = nCurrentRow;
                BindDataOther();
                ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlStageEdit")).Focus();
            }
            SetPageNavigatorO();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdGoO_Click(object sender, EventArgs e)
    {
        int result;
        gvOffshoreDocumentOther.SelectedIndex = -1;
        gvOffshoreDocumentOther.EditIndex = -1;
        if (Int32.TryParse(txtnopageO.Text, out result))
        {
            ViewState["PAGENUMBERO"] = Int32.Parse(txtnopageO.Text);

            if ((int)ViewState["TOTALPAGECOUNTO"] < Int32.Parse(txtnopageO.Text))
                ViewState["PAGENUMBERO"] = ViewState["TOTALPAGECOUNTO"];


            if (0 >= Int32.Parse(txtnopageO.Text))
                ViewState["PAGENUMBERO"] = 1;

            if ((int)ViewState["PAGENUMBERO"] == 0)
                ViewState["PAGENUMBERO"] = 1;

            txtnopageO.Text = ViewState["PAGENUMBERO"].ToString();
        }
        BindDataOther();
        SetPageNavigatorO();
    }

    protected void PagerButtonClickO(object sender, CommandEventArgs ce)
    {
        gvOffshoreDocumentOther.SelectedIndex = -1;
        gvOffshoreDocumentOther.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBERO"] = (int)ViewState["PAGENUMBERO"] - 1;
        else
            ViewState["PAGENUMBERO"] = (int)ViewState["PAGENUMBERO"] + 1;

        BindDataOther();
        SetPageNavigatorO();
    }

    private void SetPageNavigatorO()
    {
        cmdPreviousO.Enabled = IsPreviousEnabledO();
        cmdNextO.Enabled = IsNextEnabledO();
        lblPagenumberO.Text = "Page " + ViewState["PAGENUMBERO"].ToString();
        lblPagesO.Text = " of " + ViewState["TOTALPAGECOUNTO"].ToString() + " Pages. ";
        lblRecordsO.Text = "(" + ViewState["ROWCOUNTO"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabledO()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERO"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTO"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabledO()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERO"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTO"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    // User defined documents

    protected void MenuOffshoreUserDocument_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCELU"))
            {
                ShowExcelUser();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDataUserDoc()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSTAGE", "FLDDOCUMENTNAME", "FLDMANDATORYYNNAME", "FLDWAIVERYNNNAME", "FLDGROUPNAME" };
        string[] alCaptions = { "Stage", "Document", "Mandatory Y/N", "Waiver Y/N", "User group to allow waiver" };

        DataTable dt = PhoenixCrewOffshoreDocument.SearchOffshoreDocument(4, null,
                Int32.Parse(ViewState["PAGENUMBERU"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshoreDocumentUser", "User defined documents", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvOffshoreDocumentUser.DataSource = ds;
            gvOffshoreDocumentUser.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvOffshoreDocumentUser);
        }

        ViewState["ROWCOUNTU"] = iRowCount;
        ViewState["TOTALPAGECOUNTU"] = iTotalPageCount;
        SetPageNavigatorU();
    }

    protected void ShowExcelUser()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSTAGE", "FLDDOCUMENTNAME", "FLDMANDATORYYNNAME", "FLDWAIVERYNNNAME", "FLDGROUPNAME" };
        string[] alCaptions = { "Stage", "Document", "Mandatory Y/N", "Waiver Y/N", "User group to allow waiver" };

        if (ViewState["ROWCOUNTU"] == null || Int32.Parse(ViewState["ROWCOUNTU"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNTU"].ToString());

        DataTable dt = PhoenixCrewOffshoreDocument.SearchOffshoreDocument(4, null,
                Int32.Parse(ViewState["PAGENUMBERU"].ToString()),
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=OffshoreDocumentUser.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>User defined documents</h3></td>");
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
        foreach (DataRow dr in dt.Rows)
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

    protected void gvOffshoreDocumentUser_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dr = (DataRowView)e.Row.DataItem;
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            DropDownList ddlStageEdit = (DropDownList)e.Row.FindControl("ddlStageEdit");
            if (ddlStageEdit != null)
            {
                BindOffshoreStages(ddlStageEdit);
                ddlStageEdit.SelectedValue = dr["FLDSTAGEID"].ToString();
            }

            UserControlQuick ddlUserDocEdit = (UserControlQuick)e.Row.FindControl("ddlUserDocEdit");
            if (ddlUserDocEdit != null) ddlUserDocEdit.SelectedValue = dr["FLDDOCUMENTID"].ToString();

            CheckBoxList chkUserGroupEdit = (CheckBoxList)e.Row.FindControl("chkUserGroupEdit");
            CheckBox chkMandatoryYNEdit = (CheckBox)e.Row.FindControl("chkMandatoryYNEdit");
            CheckBox chkWaiverYNEdit = (CheckBox)e.Row.FindControl("chkWaiverYNEdit");
            if (chkMandatoryYNEdit != null)
            {
                if (chkMandatoryYNEdit.Checked)
                {
                    if (chkWaiverYNEdit != null) chkWaiverYNEdit.Enabled = true;
                }
                else
                {
                    if (chkWaiverYNEdit != null) chkWaiverYNEdit.Enabled = false;
                }
            }

            if (chkWaiverYNEdit != null)
            {
                if (chkWaiverYNEdit.Checked)
                {
                    if (chkUserGroupEdit != null) chkUserGroupEdit.Enabled = true;
                }
                else
                {
                    if (chkUserGroupEdit != null) chkUserGroupEdit.Enabled = false;
                }
            }

            if (chkUserGroupEdit != null)
            {
                chkUserGroupEdit.DataSource = SessionUtil.UserGroupList();
                chkUserGroupEdit.DataTextField = "FLDGROUPNAME";
                chkUserGroupEdit.DataValueField = "FLDGROUPCODE";
                chkUserGroupEdit.DataBind();

                CheckBoxList chk = (CheckBoxList)e.Row.FindControl("chkUserGroupEdit");
                foreach (ListItem li in chk.Items)
                {
                    string[] slist = dr["FLDUSERGROUPTOALLOWWAIVER"].ToString().Split(',');
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
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton ab = (ImageButton)e.Row.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }

            DropDownList ddlStageAdd = (DropDownList)e.Row.FindControl("ddlStageAdd");
            if (ddlStageAdd != null)
            {
                BindOffshoreStages(ddlStageAdd);
            }

            CheckBoxList chkUserGroupAdd = (CheckBoxList)e.Row.FindControl("chkUserGroupAdd");
            if (chkUserGroupAdd != null)
            {
                chkUserGroupAdd.DataSource = SessionUtil.UserGroupList();
                chkUserGroupAdd.DataTextField = "FLDGROUPNAME";
                chkUserGroupAdd.DataValueField = "FLDGROUPCODE";
                chkUserGroupAdd.DataBind();
            }
        }
        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
        {
            Label lblUserGroup = (Label)e.Row.FindControl("lblUserGroup");
            ImageButton ImgUserGroup = (ImageButton)e.Row.FindControl("ImgUserGroup");
            if (ImgUserGroup != null)
            {
                if (lblUserGroup != null)
                {
                    if (lblUserGroup.Text != "")
                    {
                        ImgUserGroup.Visible = true;
                        UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucUserGroup");
                        if (uct != null)
                        {
                            ImgUserGroup.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                            ImgUserGroup.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                        }
                    }
                    else
                        ImgUserGroup.Visible = false;
                }
            }
        }
    }

    protected void gvOffshoreDocumentUser_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADDU"))
            {
                CheckBoxList chk = (CheckBoxList)_gridView.FooterRow.FindControl("chkUserGroupAdd");
                string UGList = "";
                string UserGroupList = "";
                foreach (ListItem li in chk.Items)
                {
                    if (li.Selected)
                    {
                        UGList += li.Value + ",";
                    }
                }

                if (UGList != "")
                {
                    UserGroupList = "," + UGList;
                }

                if (!IsValidData(((DropDownList)_gridView.FooterRow.FindControl("ddlStageAdd")).SelectedValue,
                    ((UserControlQuick)_gridView.FooterRow.FindControl("ddlUserDocAdd")).SelectedQuick,
                    ((CheckBox)_gridView.FooterRow.FindControl("chkWaiverYNAdd")).Checked ? "1" : "0",
                    UserGroupList))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreDocument.InsertOffshoreDocument(
                    int.Parse(((DropDownList)_gridView.FooterRow.FindControl("ddlStageAdd")).SelectedValue)
                    , int.Parse(((UserControlQuick)_gridView.FooterRow.FindControl("ddlUserDocAdd")).SelectedQuick)
                    , 4
                    , General.GetNullableInteger(((CheckBox)_gridView.FooterRow.FindControl("chkMandatoryYNAdd")).Checked ? "1" : "0")
                    , General.GetNullableInteger(((CheckBox)_gridView.FooterRow.FindControl("chkWaiverYNAdd")).Checked ? "1" : "0")
                    , General.GetNullableString(UserGroupList));

                BindDataUserDoc();
                ((DropDownList)_gridView.FooterRow.FindControl("ddlStageAdd")).Focus();
            }

            else if (e.CommandName.ToUpper().Equals("DELETEU"))
            {
                int offshoredocid = int.Parse(_gridView.DataKeys[nCurrentRow].Value.ToString());
                PhoenixCrewOffshoreDocument.DeleteOffshoreDocument(offshoredocid);
                BindDataUserDoc();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATEU"))
            {
                CheckBoxList chk = (CheckBoxList)_gridView.Rows[nCurrentRow].FindControl("chkUserGroupEdit");
                string UGList = "";
                string UserGroupList = "";
                foreach (ListItem li in chk.Items)
                {
                    if (li.Selected)
                    {
                        UGList += li.Value + ",";
                    }
                }

                if (UGList != "")
                {
                    UserGroupList = "," + UGList;
                }

                if (!IsValidData(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlStageEdit")).SelectedValue,
                    ((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ddlUserDocEdit")).SelectedQuick,
                    ((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkWaiverYNEdit")).Checked ? "1" : "0",
                    UserGroupList))
                {
                    ucError.Visible = true;
                    return;
                }
                int offshoredocid = int.Parse(_gridView.DataKeys[nCurrentRow].Value.ToString());

                PhoenixCrewOffshoreDocument.UpdateOffshoreDocument(offshoredocid
                    , int.Parse(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlStageEdit")).SelectedValue)
                    , int.Parse(((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ddlUserDocEdit")).SelectedQuick)
                    , 4
                    , General.GetNullableInteger(((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkMandatoryYNEdit")).Checked ? "1" : "0")
                    , General.GetNullableInteger(((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkWaiverYNEdit")).Checked ? "1" : "0")
                    , General.GetNullableString(UserGroupList));

                _gridView.EditIndex = -1;
                BindDataUserDoc();

            }
            else if (e.CommandName.ToUpper().Equals("CANCELU"))
            {
                _gridView.EditIndex = -1;
                BindDataUserDoc();
            }
            else if (e.CommandName.ToUpper().Equals("EDITU"))
            {
                _gridView.EditIndex = nCurrentRow;
                _gridView.SelectedIndex = nCurrentRow;
                BindDataUserDoc();
                ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlStageEdit")).Focus();
            }
            SetPageNavigatorU();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdGoU_Click(object sender, EventArgs e)
    {
        int result;
        gvOffshoreDocumentUser.SelectedIndex = -1;
        gvOffshoreDocumentUser.EditIndex = -1;
        if (Int32.TryParse(txtnopageU.Text, out result))
        {
            ViewState["PAGENUMBERU"] = Int32.Parse(txtnopageU.Text);

            if ((int)ViewState["TOTALPAGECOUNTU"] < Int32.Parse(txtnopageU.Text))
                ViewState["PAGENUMBERU"] = ViewState["TOTALPAGECOUNTU"];


            if (0 >= Int32.Parse(txtnopageU.Text))
                ViewState["PAGENUMBERU"] = 1;

            if ((int)ViewState["PAGENUMBERU"] == 0)
                ViewState["PAGENUMBERU"] = 1;

            txtnopageU.Text = ViewState["PAGENUMBERU"].ToString();
        }
        BindDataUserDoc();
        SetPageNavigatorU();
    }

    protected void PagerButtonClickU(object sender, CommandEventArgs ce)
    {
        gvOffshoreDocumentUser.SelectedIndex = -1;
        gvOffshoreDocumentUser.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBERU"] = (int)ViewState["PAGENUMBERU"] - 1;
        else
            ViewState["PAGENUMBERU"] = (int)ViewState["PAGENUMBERU"] + 1;

        BindDataUserDoc();
        SetPageNavigatorU();
    }

    private void SetPageNavigatorU()
    {
        cmdPreviousU.Enabled = IsPreviousEnabledU();
        cmdNextU.Enabled = IsNextEnabledU();
        lblPagenumberU.Text = "Page " + ViewState["PAGENUMBERU"].ToString();
        lblPagesU.Text = " of " + ViewState["TOTALPAGECOUNTU"].ToString() + " Pages. ";
        lblRecordsU.Text = "(" + ViewState["ROWCOUNTU"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabledU()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERU"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTU"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabledU()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERU"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTU"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }
    //Exp

    protected void BindExpCourses(DropDownList ddl)
    {
        string doctype = PhoenixCommonRegisters.GetHardCode(1, 103, "6"); // STCW
        ddl.Items.Clear();
        ddl.DataSource = PhoenixRegistersDocumentCourse.ListDocumentCourse(General.GetNullableInteger(doctype));
        ddl.DataTextField = "FLDDOCUMENTNAME";
        ddl.DataValueField = "FLDDOCUMENTID";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void MenuOffshoreDocumentExp_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCELE"))
            {
                ShowExcelExp();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDataExp()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSTAGE", "FLDDOCUMENTNAME", "FLDMANDATORYYNNAME", "FLDWAIVERYNNNAME", "FLDGROUPNAME" };
        string[] alCaptions = { "Stage", "Document", "Mandatory Y/N", "Waiver Y/N", "User group to allow waiver" };

        DataTable dt = PhoenixCrewOffshoreDocument.SearchOffshoreExpDocument(null, null,
                Int32.Parse(ViewState["PAGENUMBERE"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshoreDocumentExp", "Experience", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvOffshoreDocumentExp.DataSource = ds;
            gvOffshoreDocumentExp.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvOffshoreDocumentExp);
        }

        ViewState["ROWCOUNTE"] = iRowCount;
        ViewState["TOTALPAGECOUNTE"] = iTotalPageCount;
        SetPageNavigatorE();
    }

    protected void ShowExcelExp()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSTAGE", "FLDDOCUMENTNAME", "FLDMANDATORYYNNAME", "FLDWAIVERYNNNAME", "FLDGROUPNAME" };
        string[] alCaptions = { "Stage", "Document", "Mandatory Y/N", "Waiver Y/N", "User group to allow waiver" };

        if (ViewState["ROWCOUNTE"] == null || Int32.Parse(ViewState["ROWCOUNTE"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNTE"].ToString());

        DataTable dt = PhoenixCrewOffshoreDocument.SearchOffshoreExpDocument(null, null,
                Int32.Parse(ViewState["PAGENUMBERE"].ToString()),
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=OffshoreDocumentExp.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Experience</h3></td>");
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
        foreach (DataRow dr in dt.Rows)
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

    protected void gvOffshoreDocumentExp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dr = (DataRowView)e.Row.DataItem;
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            DropDownList ddlStageEdit = (DropDownList)e.Row.FindControl("ddlStageEdit");
            if (ddlStageEdit != null)
            {
                BindOffshoreStages(ddlStageEdit);
                ddlStageEdit.SelectedValue = dr["FLDSTAGEID"].ToString();
            }



            CheckBoxList chkUserGroupEdit = (CheckBoxList)e.Row.FindControl("chkUserGroupEdit");
            CheckBox chkMandatoryYNEdit = (CheckBox)e.Row.FindControl("chkMandatoryYNEdit");
            CheckBox chkWaiverYNEdit = (CheckBox)e.Row.FindControl("chkWaiverYNEdit");
            if (chkMandatoryYNEdit != null)
            {
                if (chkMandatoryYNEdit.Checked)
                {
                    if (chkWaiverYNEdit != null) chkWaiverYNEdit.Enabled = true;
                }
                else
                {
                    if (chkWaiverYNEdit != null) chkWaiverYNEdit.Enabled = false;
                }
            }

            if (chkWaiverYNEdit != null)
            {
                if (chkWaiverYNEdit.Checked)
                {
                    if (chkUserGroupEdit != null) chkUserGroupEdit.Enabled = true;
                }
                else
                {
                    if (chkUserGroupEdit != null) chkUserGroupEdit.Enabled = false;
                }
            }

            if (chkUserGroupEdit != null)
            {
                chkUserGroupEdit.DataSource = SessionUtil.UserGroupList();
                chkUserGroupEdit.DataTextField = "FLDGROUPNAME";
                chkUserGroupEdit.DataValueField = "FLDGROUPCODE";
                chkUserGroupEdit.DataBind();

                CheckBoxList chk = (CheckBoxList)e.Row.FindControl("chkUserGroupEdit");
                foreach (ListItem li in chk.Items)
                {
                    string[] slist = dr["FLDUSERGROUPTOALLOWWAIVER"].ToString().Split(',');
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
        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
        {
            Label lblUserGroup = (Label)e.Row.FindControl("lblUserGroup");
            ImageButton ImgUserGroup = (ImageButton)e.Row.FindControl("ImgUserGroup");
            if (ImgUserGroup != null)
            {
                if (lblUserGroup != null)
                {
                    if (lblUserGroup.Text != "")
                    {
                        ImgUserGroup.Visible = true;
                        UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucUserGroup");
                        if (uct != null)
                        {
                            ImgUserGroup.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                            ImgUserGroup.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                        }
                    }
                    else
                        ImgUserGroup.Visible = false;
                }
            }
        }
    }

    protected void gvOffshoreDocumentExp_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADDE"))
            {
                CheckBoxList chk = (CheckBoxList)_gridView.FooterRow.FindControl("chkUserGroupAdd");
                string UGList = "";
                string UserGroupList = "";
                foreach (ListItem li in chk.Items)
                {
                    if (li.Selected)
                    {
                        UGList += li.Value + ",";
                    }
                }

                if (UGList != "")
                {
                    UserGroupList = "," + UGList;
                }

                PhoenixCrewOffshoreDocument.InsertOffshoreDocument(
                    int.Parse(((DropDownList)_gridView.FooterRow.FindControl("ddlStageAdd")).SelectedValue)
                    , int.Parse(((DropDownList)_gridView.FooterRow.FindControl("ddlCourseAdd")).SelectedValue)
                    , 2
                    , General.GetNullableInteger(((CheckBox)_gridView.FooterRow.FindControl("chkMandatoryYNAdd")).Checked ? "1" : "0")
                    , General.GetNullableInteger(((CheckBox)_gridView.FooterRow.FindControl("chkWaiverYNAdd")).Checked ? "1" : "0")
                    , General.GetNullableString(UserGroupList));

                 BindDataExp();
                ((DropDownList)_gridView.FooterRow.FindControl("ddlStageAdd")).Focus();
            }

            else if (e.CommandName.ToUpper().Equals("DELETEE"))
            {
                int offshoredocid = int.Parse(_gridView.DataKeys[nCurrentRow].Value.ToString());
                PhoenixCrewOffshoreDocument.DeleteOffshoreDocument(offshoredocid);
                BindDataExp();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATEE"))
            {
                CheckBoxList chk = (CheckBoxList)_gridView.Rows[nCurrentRow].FindControl("chkUserGroupEdit");
                string UGList = "";
                string UserGroupList = "";
                foreach (ListItem li in chk.Items)
                {
                    if (li.Selected)
                    {
                        UGList += li.Value + ",";
                    }
                }

                if (UGList != "")
                {
                    UserGroupList = "," + UGList;
                }

                if (!IsValidData(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlStageEdit")).SelectedValue,
                    ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDocumentType")).Text,
                    ((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkWaiverYNEdit")).Checked ? "1" : "0",
                    UserGroupList))
                {
                    ucError.Visible = true;
                    return;
                }
                int? offshoredocid = General.GetNullableInteger(_gridView.DataKeys[nCurrentRow].Value.ToString());

                if (offshoredocid == null)
                {
                    PhoenixCrewOffshoreDocument.InsertOffshoreDocument(
                         int.Parse(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlStageEdit")).SelectedValue)
                        , null
                        , int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblDocumentType")).Text)
                        , General.GetNullableInteger(((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkMandatoryYNEdit")).Checked ? "1" : "0")
                        , General.GetNullableInteger(((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkWaiverYNEdit")).Checked ? "1" : "0")
                        , General.GetNullableString(UserGroupList));
                    _gridView.EditIndex = -1;
                    BindDataExp();
                   
                }
                else
                {

                    PhoenixCrewOffshoreDocument.UpdateOffshoreDocument(offshoredocid
                        , int.Parse(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlStageEdit")).SelectedValue)
                        , null
                        , int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblDocumentType")).Text)
                        , General.GetNullableInteger(((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkMandatoryYNEdit")).Checked ? "1" : "0")
                        , General.GetNullableInteger(((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkWaiverYNEdit")).Checked ? "1" : "0")
                        , General.GetNullableString(UserGroupList));

                    _gridView.EditIndex = -1;
                    BindDataExp();
                }

            }
            else if (e.CommandName.ToUpper().Equals("CANCELE"))
            {
                _gridView.EditIndex = -1;
                BindDataExp();
            }
            else if (e.CommandName.ToUpper().Equals("EDITE"))
            {
                _gridView.EditIndex = nCurrentRow;
                _gridView.SelectedIndex = nCurrentRow;
                BindDataExp();
                ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlStageEdit")).Focus();
            }
            SetPageNavigatorE();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdGoE_Click(object sender, EventArgs e)
    {
        int result;
        gvOffshoreDocumentExp.SelectedIndex = -1;
        gvOffshoreDocumentExp.EditIndex = -1;
        if (Int32.TryParse(txtnopageE.Text, out result))
        {
            ViewState["PAGENUMBERE"] = Int32.Parse(txtnopageE.Text);

            if ((int)ViewState["TOTALPAGECOUNTE"] < Int32.Parse(txtnopageE.Text))
                ViewState["PAGENUMBERE"] = ViewState["TOTALPAGECOUNTE"];


            if (0 >= Int32.Parse(txtnopageE.Text))
                ViewState["PAGENUMBERE"] = 1;

            if ((int)ViewState["PAGENUMBERE"] == 0)
                ViewState["PAGENUMBERE"] = 1;

            txtnopageE.Text = ViewState["PAGENUMBERE"].ToString();
        }
        BindDataExp();
        SetPageNavigatorE();
    }

    protected void PagerButtonClickE(object sender, CommandEventArgs ce)
    {
        gvOffshoreDocumentExp.SelectedIndex = -1;
        gvOffshoreDocumentExp.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBERE"] = (int)ViewState["PAGENUMBERE"] - 1;
        else
            ViewState["PAGENUMBERE"] = (int)ViewState["PAGENUMBERE"] + 1;

        BindDataExp();
        SetPageNavigatorE();
    }

    private void SetPageNavigatorE()
    {
        cmdPreviousE.Enabled = IsPreviousEnabledE();
        cmdNextE.Enabled = IsNextEnabledE();
        lblPagenumberE.Text = "Page " + ViewState["PAGENUMBERE"].ToString();
        lblPagesE.Text = " of " + ViewState["TOTALPAGECOUNTE"].ToString() + " Pages. ";
        lblRecordsE.Text = "(" + ViewState["ROWCOUNTE"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabledE()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERE"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTE"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabledE()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERE"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTE"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    //Travel Documents
    protected void MenuOffshoreDocumentTravel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCELT"))
            {
                ShowExcelTravel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDataTravel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSTAGE", "FLDCATEGORYNAME", "FLDDOCUMENTNAME", "FLDMANDATORYYNNAME", "FLDWAIVERYNNNAME", "FLDGROUPNAME" };
        string[] alCaptions = { "Stage", "Document Category", "Document", "Mandatory Y/N", "Waiver Y/N", "User group to allow waiver" };

        DataTable dt = PhoenixCrewOffshoreDocument.SearchOffshoreTravelDocument(null, null,
                Int32.Parse(ViewState["PAGENUMBERE"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshoreDocumentTravel", "Travel Documents", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvOffshoreDocumentTravel.DataSource = ds;
            gvOffshoreDocumentTravel.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvOffshoreDocumentTravel);
        }

        ViewState["ROWCOUNTT"] = iRowCount;
        ViewState["TOTALPAGECOUNTT"] = iTotalPageCount;
        SetPageNavigatorT();
    }

    protected void ShowExcelTravel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSTAGE","FLDCATEGORYNAME", "FLDDOCUMENTNAME", "FLDMANDATORYYNNAME", "FLDWAIVERYNNNAME", "FLDGROUPNAME" };
        string[] alCaptions = { "Stage", "Document Category", "Document", "Mandatory Y/N", "Waiver Y/N", "User group to allow waiver" };

        if (ViewState["ROWCOUNTT"] == null || Int32.Parse(ViewState["ROWCOUNTT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNTT"].ToString());

        DataTable dt = PhoenixCrewOffshoreDocument.SearchOffshoreTravelDocument(null, null,
                Int32.Parse(ViewState["PAGENUMBERT"].ToString()),
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=OffshoreDocumentTravel.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel Documents</h3></td>");
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
        foreach (DataRow dr in dt.Rows)
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

    protected void gvOffshoreDocumentTravel_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dr = (DataRowView)e.Row.DataItem;
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            DropDownList ddlStageEdit = (DropDownList)e.Row.FindControl("ddlStageEdit");
            if (ddlStageEdit != null)
            {
                BindOffshoreStages(ddlStageEdit);
                ddlStageEdit.SelectedValue = dr["FLDSTAGEID"].ToString();
            }



            CheckBoxList chkUserGroupEdit = (CheckBoxList)e.Row.FindControl("chkUserGroupEdit");
            CheckBox chkMandatoryYNEdit = (CheckBox)e.Row.FindControl("chkMandatoryYNEdit");
            CheckBox chkWaiverYNEdit = (CheckBox)e.Row.FindControl("chkWaiverYNEdit");
            if (chkMandatoryYNEdit != null)
            {
                if (chkMandatoryYNEdit.Checked)
                {
                    if (chkWaiverYNEdit != null) chkWaiverYNEdit.Enabled = true;
                }
                else
                {
                    if (chkWaiverYNEdit != null) chkWaiverYNEdit.Enabled = false;
                }
            }

            if (chkWaiverYNEdit != null)
            {
                if (chkWaiverYNEdit.Checked)
                {
                    if (chkUserGroupEdit != null) chkUserGroupEdit.Enabled = true;
                }
                else
                {
                    if (chkUserGroupEdit != null) chkUserGroupEdit.Enabled = false;
                }
            }

            if (chkUserGroupEdit != null)
            {
                chkUserGroupEdit.DataSource = SessionUtil.UserGroupList();
                chkUserGroupEdit.DataTextField = "FLDGROUPNAME";
                chkUserGroupEdit.DataValueField = "FLDGROUPCODE";
                chkUserGroupEdit.DataBind();

                CheckBoxList chk = (CheckBoxList)e.Row.FindControl("chkUserGroupEdit");
                foreach (ListItem li in chk.Items)
                {
                    string[] slist = dr["FLDUSERGROUPTOALLOWWAIVER"].ToString().Split(',');
                    foreach (string s in slist)
                    {
                        if (li.Value.Equals(s))
                        {
                            li.Selected = true;
                        }
                    }
                }
            }

            UserControlDocumentCategory ucCategory = (UserControlDocumentCategory)e.Row.FindControl("ucCategoryEdit");
            if (ucCategory != null)
            {
                ucCategory.DocumentCategoryList = PhoenixRegistersDocumentCategory.ListDocumentCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , null
                    , null
                    , null
                    );
                ucCategory.DataBind();
                if (General.GetNullableInteger(dr["FLDDOCUMENTCATEGORYID"].ToString()) != null)
                    ucCategory.SelectedDocumentCategoryID = dr["FLDDOCUMENTCATEGORYID"].ToString();
            }

        }
        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
        {
            Label lblUserGroup = (Label)e.Row.FindControl("lblUserGroup");
            ImageButton ImgUserGroup = (ImageButton)e.Row.FindControl("ImgUserGroup");
            if (ImgUserGroup != null)
            {
                if (lblUserGroup != null)
                {
                    if (lblUserGroup.Text != "")
                    {
                        ImgUserGroup.Visible = true;
                        UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucUserGroup");
                        if (uct != null)
                        {
                            ImgUserGroup.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                            ImgUserGroup.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                        }
                    }
                    else
                        ImgUserGroup.Visible = false;
                }
            }
            UserControlDocumentCategory ucCategoryAdd = (UserControlDocumentCategory)e.Row.FindControl("ucCategoryAdd");
            if (ucCategoryAdd != null)
            {
                ucCategoryAdd.DocumentCategoryList = PhoenixRegistersDocumentCategory.ListDocumentCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , null
                    , null
                    , null
                    );
                ucCategoryAdd.DataBind();                
            }
        }
    }

    protected void gvOffshoreDocumentTravel_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());            
            if (e.CommandName.ToUpper().Equals("DELETET"))
            {
                int offshoredocid = int.Parse(_gridView.DataKeys[nCurrentRow].Value.ToString());
                PhoenixCrewOffshoreDocument.DeleteOffshoreDocument(offshoredocid);
                BindDataTravel();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATET"))
            {
                CheckBoxList chk = (CheckBoxList)_gridView.Rows[nCurrentRow].FindControl("chkUserGroupEdit");
                string UGList = "";
                string UserGroupList = "";
                foreach (ListItem li in chk.Items)
                {
                    if (li.Selected)
                    {
                        UGList += li.Value + ",";
                    }
                }

                if (UGList != "")
                {
                    UserGroupList = "," + UGList;
                }

                if (!IsValidData(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlStageEdit")).SelectedValue,
                    ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDocumentType")).Text,
                    ((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkWaiverYNEdit")).Checked ? "1" : "0",
                    UserGroupList))
                {
                    ucError.Visible = true;
                    return;
                }
                int? offshoredocid = General.GetNullableInteger(_gridView.DataKeys[nCurrentRow].Value.ToString());

                if (offshoredocid == null)
                {
                    PhoenixCrewOffshoreDocument.InsertOffshoreDocument(
                         int.Parse(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlStageEdit")).SelectedValue)
                        , null
                        , int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblDocumentType")).Text)
                        , General.GetNullableInteger(((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkMandatoryYNEdit")).Checked ? "1" : "0")
                        , General.GetNullableInteger(((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkWaiverYNEdit")).Checked ? "1" : "0")
                        , General.GetNullableString(UserGroupList)
                        , General.GetNullableInteger(((UserControlDocumentCategory)_gridView.FooterRow.FindControl("ucCategoryAdd")).SelectedDocumentCategoryID));
                    _gridView.EditIndex = -1;
                    BindDataTravel();

                }
                else
                {
                    PhoenixCrewOffshoreDocument.UpdateOffshoreDocument(offshoredocid
                        , int.Parse(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlStageEdit")).SelectedValue)
                        , null
                        , int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblDocumentType")).Text)
                        , General.GetNullableInteger(((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkMandatoryYNEdit")).Checked ? "1" : "0")
                        , General.GetNullableInteger(((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkWaiverYNEdit")).Checked ? "1" : "0")
                        , General.GetNullableString(UserGroupList)
                        , General.GetNullableInteger(((UserControlDocumentCategory)_gridView.Rows[nCurrentRow].FindControl("ucCategoryEdit")).SelectedDocumentCategoryID));

                    _gridView.EditIndex = -1;
                    BindDataTravel();
                }

            }
            else if (e.CommandName.ToUpper().Equals("CANCELT"))
            {
                _gridView.EditIndex = -1;
                BindDataTravel();
            }
            else if (e.CommandName.ToUpper().Equals("EDITT"))
            {
                _gridView.EditIndex = nCurrentRow;
                _gridView.SelectedIndex = nCurrentRow;
                BindDataTravel();
                ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlStageEdit")).Focus();
            }
            SetPageNavigatorT();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdGoT_Click(object sender, EventArgs e)
    {
        int result;
        gvOffshoreDocumentTravel.SelectedIndex = -1;
        gvOffshoreDocumentTravel.EditIndex = -1;
        if (Int32.TryParse(txtnopageT.Text, out result))
        {
            ViewState["PAGENUMBERT"] = Int32.Parse(txtnopageT.Text);

            if ((int)ViewState["TOTALPAGECOUNTT"] < Int32.Parse(txtnopageT.Text))
                ViewState["PAGENUMBERT"] = ViewState["TOTALPAGECOUNTT"];


            if (0 >= Int32.Parse(txtnopageT.Text))
                ViewState["PAGENUMBERT"] = 1;

            if ((int)ViewState["PAGENUMBERT"] == 0)
                ViewState["PAGENUMBERT"] = 1;

            txtnopageT.Text = ViewState["PAGENUMBERT"].ToString();
        }
        BindDataTravel();
        SetPageNavigatorT();
    }

    protected void PagerButtonClickT(object sender, CommandEventArgs ce)
    {
        gvOffshoreDocumentTravel.SelectedIndex = -1;
        gvOffshoreDocumentTravel.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBERT"] = (int)ViewState["PAGENUMBERT"] - 1;
        else
            ViewState["PAGENUMBERT"] = (int)ViewState["PAGENUMBERT"] + 1;

        BindDataTravel();
        SetPageNavigatorT();
    }

    private void SetPageNavigatorT()
    {
        cmdPreviousT.Enabled = IsPreviousEnabledT();
        cmdNextT.Enabled = IsNextEnabledT();
        lblPagenumberT.Text = "Page " + ViewState["PAGENUMBERT"].ToString();
        lblPagesT.Text = " of " + ViewState["TOTALPAGECOUNTT"].ToString() + " Pages. ";
        lblRecordsT.Text = "(" + ViewState["ROWCOUNTT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabledT()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERT"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabledT()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERT"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }


}
