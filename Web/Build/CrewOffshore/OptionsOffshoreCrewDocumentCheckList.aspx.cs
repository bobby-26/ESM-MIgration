using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewOffshore;

public partial class OptionsOffshoreCrewDocumentCheckList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Submit", "SUBMIT");

            MenuChecklist.AccessRights = this.ViewState;
            MenuChecklist.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
                


                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                if (Request.QueryString["Crewplanid"] != null)
                    ViewState["CREWPLANID"] = Request.QueryString["Crewplanid"].ToString();
                if (Request.QueryString["Office"] != null && Request.QueryString["Office"] != "")
                    ViewState["OFFICE"] = Request.QueryString["Office"].ToString();
            }
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindData();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void Checklist_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SUBMIT"))
            {
                

                    //if (ViewState["OFFICE"] == null)
                    //{
                        bool result;
                        result = true;
                        Insertdocumentchecklist(ref result);
                        if (!result)
                        {
                            ucError.Visible = true;
                            return;
                        }

                        //PhoenixCrewOffshoreDocumentChecklist.Submitdocumentchecklist(1, General.GetNullableGuid(ViewState["CREWPLANID"].ToString()));
                    //}
                    //else
                    //{
                    //    //PhoenixCrewOffshoreDocumentChecklist.Submitdocumentchecklist(1, General.GetNullableGuid(ViewState["CREWPLANID"].ToString()), 1,);
                    //}

                    ucStatus.Text = "Document information submitted successfully";
                }
            //}
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Insertdocumentchecklist(ref bool result)
    {
        result = true;

        DataSet ds = new DataSet();

        DataTable table = new DataTable();
        table.Columns.Add("FLDDOCUMENTCHECKLISTID", typeof(Guid));
        table.Columns.Add("FLDHOLDINGYN", typeof(int));
        table.Columns.Add("FLDREMARKS", typeof(string));

        int count = 0, i = 0;
        count = gvDocumentChecklist.Rows.Count;

        foreach (GridViewRow gv in gvDocumentChecklist.Rows)
        {
            if (count > i && gvDocumentChecklist.Rows[0].Cells[0].Text.ToUpper() != "NO RECORDS FOUND")
            {

                if (!IsValiddocument(((CheckBox)gv.FindControl("ckbYesOrNo")).Checked ? "1" : "2",
                                General.GetNullableString(((TextBox)gv.FindControl("lblifnoremarksItem")).Text)))
                {
                    result = false;
                    return;
                }
                table.Rows.Add(General.GetNullableGuid(((Label)gv.FindControl("lblDocumentchecklistiditem")).Text),
                            General.GetNullableInteger(((CheckBox)gv.FindControl("ckbYesOrNo")).Checked ? "1" : "2"),
                            General.GetNullableString(((TextBox)gv.FindControl("lblifnoremarksItem")).Text));
            }
            i++;
        }
        ds.Tables.Add(table);

        System.IO.StringWriter sw = new System.IO.StringWriter();
        ds.WriteXml(sw);
        string resultstring = sw.ToString();
        if (ViewState["CREWPLANID"] != null && ViewState["CREWPLANID"].ToString() != "")
        {
            //if (ViewState["OFFICE"] == null)
            //{
            //    PhoenixCrewOffshoreDocumentChecklist.Submitdocumentchecklist(1, General.GetNullableGuid(ViewState["CREWPLANID"].ToString()), 1, resultstring);
            //}
            //else
            //{
                PhoenixCrewOffshoreDocumentChecklist.Submitdocumentchecklist(1, General.GetNullableGuid(ViewState["CREWPLANID"].ToString()), 1, resultstring);
            //}
        }
    }

    private void BindData()
    {


        DataSet ds = new DataSet();
        ds = PhoenixCrewOffshoreDocumentChecklist.SearchDocumentChecklist(1, null, General.GetNullableGuid(ViewState["CREWPLANID"].ToString()));



        //General.SetPrintOptions("gvDocumentChecklist", "Crew Document Checklist", alCaptions, alColumns, ds);

        if (ds.Tables[1].Rows.Count > 0)
        {
            txtFirstName.Text = ds.Tables[1].Rows[0]["FLDNAME"].ToString();
            txtFirstName.ToolTip = ds.Tables[1].Rows[0]["FLDNAME"].ToString();

            txtEmployeeNumber.Text = ds.Tables[1].Rows[0]["FLDFILENO"].ToString();
            txtRank.Text = ds.Tables[1].Rows[0]["FLDRANKNAME"].ToString();
            txtRank.ToolTip = ds.Tables[1].Rows[0]["FLDRANKNAME"].ToString();
            txtVessel.Text = ds.Tables[1].Rows[0]["FLDVESSELNAME"].ToString();
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDocumentChecklist.DataSource = ds;
            gvDocumentChecklist.DataBind();
        }
        else
        {
            ShowNoRecordsFound(ds.Tables[0], gvDocumentChecklist);
        }

    }

    protected void gvDocumentChecklist_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();

    }

    protected void gvDocumentChecklist_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvDocumentChecklist.EditIndex = -1;
        gvDocumentChecklist.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvDocumentChecklist_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvDocumentChecklist, "Edit$" + e.Row.RowIndex.ToString(), false);
        }

        SetKeyDownScroll(sender, e);
    }

    protected void gvDocumentChecklist_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
           

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvDocumentChecklist_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
    }

    protected void gvDocumentChecklist_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
    }
    protected void gvDocumentChecklist_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            Updatedocument(General.GetNullableGuid(ViewState["CREWPLANID"].ToString()),
                General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblDocumentTypeId")).Text),
                General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblDocumentchecklistidedit")).Text),
                        General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAvailableDocumentIdEdit")).Text)
                        , General.GetNullableInteger(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlHoldingynEdit")).SelectedValue)
                        , General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("lblifnoremarksEdit")).Text));


            _gridView.EditIndex = -1;
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Updatedocument(Guid? crewplanid, int? DocumentTypeId, Guid? Documentchecklistid, int? availabledocumentid, int? holdingyn, string remarks)
    {
        PhoenixCrewOffshoreDocumentChecklist.Updatedocumentstatus(1, crewplanid, DocumentTypeId, Documentchecklistid, availabledocumentid, holdingyn, remarks);
        ucStatus.Text = "Document information updated";
    }
    protected void gvDocumentChecklist_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {


            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {

            }
            CheckBox ckbYesOrNo = (CheckBox)e.Row.FindControl("ckbYesOrNo");
            DropDownList holdingyesno = (DropDownList)e.Row.FindControl("ddlHoldingynEdit");

            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (holdingyesno != null) holdingyesno.SelectedValue = drv["FLDHOLDINGYN"].ToString();
            if (ckbYesOrNo != null)
            {
                if (drv["FLDHOLDINGYN"].ToString() == "1")
                {
                    ckbYesOrNo.Checked = true;
                }
            }

            TextBox lblifnoremarksItem = (TextBox)e.Row.FindControl("lblifnoremarksItem");

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvDocumentChecklist.EditIndex = -1;
        gvDocumentChecklist.SelectedIndex = -1;
        BindData();
    }


    private void InsertFlag(int countrycode, int medicalrequires, int flagsibyn, int ssoyn, int pscrbyn, int medicareyn, int reportcodeid)
    {
        PhoenixRegistersFlag.InsertFlag(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
             countrycode
             , medicalrequires
             , flagsibyn
             , ssoyn
             , pscrbyn
            , medicareyn
            , reportcodeid);
    }

    private void UpdateFlag(int flagid, int countrycode, int medicalrequires, int flagsibyn, int ssoyn, int pscrbyn, int medicareyn, int reportcodeid)
    {
        PhoenixRegistersFlag.UpdateFlag(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            flagid, countrycode, medicalrequires, flagsibyn
            , ssoyn
            , pscrbyn
            , medicareyn
            , reportcodeid);

        //ucStatus.Text = "Flag information updated";        
    }

    private bool IsValiddocument(string holdingyn, string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(holdingyn) == null || holdingyn == "")
            ucError.ErrorMessage = "Holding Original Y/N is required.";

        if ((General.GetNullableString(remarks) == null || General.GetNullableString(remarks) == "") && holdingyn == "2")
            ucError.ErrorMessage = "If 'No'. Specify where is the original document.";

        return (!ucError.IsError);
    }

    private void DeleteFlag(int flagid)
    {
        PhoenixRegistersFlag.DeleteFlag(PhoenixSecurityContext.CurrentSecurityContext.UserCode, flagid);
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

    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }

    }
}
