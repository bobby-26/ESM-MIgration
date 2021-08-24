using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewOffshore;
using Telerik.Web.UI;

public partial class OptionsOffshoreCrewDocumentCheckList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Submit", "SUBMIT",ToolBarDirection.Right);

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
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SUBMIT"))
            {
           
                bool result;
                result = true;
                Insertdocumentchecklist(ref result);
                if (!result)
                {
                    ucError.Visible = true;
                    return;
                }
         
                ucStatus.Text = "Document information submitted successfully";
            }
           
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
        count = gvDocumentChecklist.Items.Count;

        foreach (GridDataItem gv in gvDocumentChecklist.Items)
        {
            if (count > i)// && gvDocumentChecklist.Rows[0].Cells[0].Text.ToUpper() != "NO RECORDS FOUND")
            {

                if (!IsValiddocument(((CheckBox)gv.FindControl("ckbYesOrNo")).Checked ? "1" : "2",
                                General.GetNullableString(((RadTextBox)gv.FindControl("lblifnoremarksItem")).Text)))
                {
                    result = false;
                    return;
                }
                table.Rows.Add(General.GetNullableGuid(((RadLabel)gv.FindControl("lblDocumentchecklistiditem")).Text),
                            General.GetNullableInteger(((CheckBox)gv.FindControl("ckbYesOrNo")).Checked ? "1" : "2"),
                            General.GetNullableString(((RadTextBox)gv.FindControl("lblifnoremarksItem")).Text));
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
        gvDocumentChecklist.DataSource = ds;


    }


    //protected void gvDocumentChecklist_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow
    //        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
    //        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
    //    {
    //        e.Row.TabIndex = -1;
    //        e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvDocumentChecklist, "Edit$" + e.Row.RowIndex.ToString(), false);
    //    }

    //    SetKeyDownScroll(sender, e);
    //}


    private void Updatedocument(Guid? crewplanid, int? DocumentTypeId, Guid? Documentchecklistid, int? availabledocumentid, int? holdingyn, string remarks)
    {
        PhoenixCrewOffshoreDocumentChecklist.Updatedocumentstatus(1, crewplanid, DocumentTypeId, Documentchecklistid, availabledocumentid, holdingyn, remarks);
        ucStatus.Text = "Document information updated";
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {

        BindData();
        gvDocumentChecklist.Rebind();
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

    protected void gvDocumentChecklist_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvDocumentChecklist_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "UPDATE")
        {
            try
            {

                Updatedocument(General.GetNullableGuid(ViewState["CREWPLANID"].ToString()),
                    General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblDocumentTypeId")).Text),
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDocumentchecklistidedit")).Text),
                            General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblAvailableDocumentIdEdit")).Text)
                            , General.GetNullableInteger(((DropDownList)e.Item.FindControl("ddlHoldingynEdit")).SelectedValue)
                            , General.GetNullableString(((RadTextBox)e.Item.FindControl("lblifnoremarksEdit")).Text));


             
                BindData();
                gvDocumentChecklist.Rebind();

            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
    }

    protected void gvDocumentChecklist_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvDocumentChecklist_ItemDataBound(object sender, GridItemEventArgs e)
    {


        if (e.Item is GridDataItem)
        {


            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);


            CheckBox ckbYesOrNo = (CheckBox)e.Item.FindControl("ckbYesOrNo");
            DropDownList holdingyesno = (DropDownList)e.Item.FindControl("ddlHoldingynEdit");

            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (holdingyesno != null) holdingyesno.SelectedValue = drv["FLDHOLDINGYN"].ToString();
            if (ckbYesOrNo != null)
            {
                if (drv["FLDHOLDINGYN"].ToString() == "1")
                {
                    ckbYesOrNo.Checked = true;
                }
            }

            RadTextBox lblifnoremarksItem = (RadTextBox)e.Item.FindControl("lblifnoremarksItem");

        }
        if (e.Item is GridFooterItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
}
