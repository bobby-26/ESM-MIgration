using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class RegistersOwnerBudgetCodePBMapPortageBillStandardComponent : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersPortageBillStandardComponent.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvPBStdComp')", "Print Grid", "icon_print.png", "PRINT");
            MenuPBStandardComponent.AccessRights = this.ViewState;
            MenuPBStandardComponent.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            }
            BindData();
            BindOwnerPBMapData();

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {


        string[] alColumns = { "FLDLOGTYPENAME", "FLDHARDNAME", "FLDCODE", "FLDDESCRIPTION", "FLDBUDGETCODE" };
        string[] alCaptions = { "Component Type", "Component Sub Type", "Code", "Description", "Budget Code" };

        DataTable dt = PhoenixRegistersPortageBillStandardComponent.ListPortageBillComponent(null);

        General.ShowExcel("Portage Bill Standard Component", dt, alColumns, alCaptions, null, string.Empty);
    }

    protected void PBStandardComponent_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvPBStdComp.SelectedIndex = -1;
                gvPBStdComp.EditIndex = -1;

                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
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

        string[] alColumns = { "FLDAIRLINESCODE", "FLDAIRLINESNAME", "FLDCOUNTRYNAME", "FLDACTIVE" };
        string[] alCaptions = { "Code", "Name", "Country", "Active Y/N" };

        DataTable dt = PhoenixRegistersPortageBillStandardComponent.ListPortageBillComponent(null);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvPBStdComp", "Airlines", alCaptions, alColumns, ds);

        if (dt.Rows.Count > 0)
        {
            gvPBStdComp.DataSource = dt;
            gvPBStdComp.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvPBStdComp);
        }
    }

    protected void gvPBStdComp_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string logtype = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlComponentTypeEdit")).SelectedValue;
            string wagehead = ((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ddlHardEdit")).SelectedHard;
            string code = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCodeEdit")).Text;
            string desc = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDescEdit")).Text;
            string budget = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtBudgetIdEdit")).Text;
            Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
            if (!IsValidPBStandardComponent(logtype, desc, budget))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixRegistersPortageBillStandardComponent.UpdatePortageBillComponent(id, int.Parse(logtype), General.GetNullableInteger(wagehead), code, desc, General.GetNullableInteger(budget));

            string strPrincipal = ucOwner.SelectedAddress;
            string strBudgetId = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtBudgetIdEdit")).Text;
            string strComponentId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDTKey")).Text;
            string strOwnerBudgetId = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOwnerBudgetCodeIdEdit")).Text;


            if (!IsValidOwnerBudgetCodeMap(strPrincipal, strOwnerBudgetId))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixRegistersOwnerBudgetCodePBMap.InsertOwnerBudgetCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , int.Parse(strPrincipal)
                                                                        , null
                                                                        , new Guid(strComponentId)
                                                                        , General.GetNullableInteger(strBudgetId)
                                                                        , new Guid(strOwnerBudgetId));

            _gridView.EditIndex = -1;
            BindData();
            BindOwnerPBMapData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidOwnerBudgetCodeMap(string principal, string ownerbudgetid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableInteger(principal).HasValue)
        {
            ucError.ErrorMessage = "Principal is required.";
        }
        else if (!General.GetNullableGuid(ownerbudgetid).HasValue)
        {
            ucError.ErrorMessage = "Onwer Budget Code is required.";
        }

        return (!ucError.IsError);
    }

    protected void gvPBStdComp_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = de.NewEditIndex;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPBStdComp_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string logtype = ((DropDownList)_gridView.FooterRow.FindControl("ddlComponentTypeAdd")).SelectedValue;
                string wagehead = ((UserControlHard)_gridView.FooterRow.FindControl("ddlHardAdd")).SelectedHard;
                string code = ((TextBox)_gridView.FooterRow.FindControl("txtCodeAdd")).Text;
                string desc = ((TextBox)_gridView.FooterRow.FindControl("txtDescAdd")).Text;
                string budget = ((TextBox)_gridView.FooterRow.FindControl("txtBudgetIdAdd")).Text;
                if (!IsValidPBStandardComponent(logtype, desc, budget))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersPortageBillStandardComponent.InsertPortageBillComponent(int.Parse(logtype), General.GetNullableInteger(wagehead), code, desc, General.GetNullableInteger(budget));
                BindData();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPBStdComp_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = de.RowIndex;
            Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
            PhoenixRegistersPortageBillStandardComponent.DeleteAirlines(id);
            _gridView.EditIndex = -1;
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPBStdComp_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            ImageButton ibp = (ImageButton)e.Row.FindControl("btnShowOwnerBudgetEdit");

            if (ibp != null)
                ibp.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetCodeEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudgetCodeTree.aspx?iframignore=false&OWNERID=" + ucOwner.SelectedAddress + "', true); ");

            DataRowView drv = (DataRowView)e.Row.DataItem;
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);


            DropDownList ct = (DropDownList)e.Row.FindControl("ddlComponentTypeEdit");
            if (ct != null)
            {
                DataSet dsCL = PhoenixRegistersPortageBillStandardComponent.ListPortageBillComponentHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1, 0, "OED,OBI,OCA,OAT,ORL,OBF,OSA,SOA,OPC,OFB,AWT,ODP,ORO,OSO,OFA");
                ct.DataSource = dsCL;
                ct.DataBind();
                ct.Items.Insert(0, new ListItem("--Select--", ""));
                ct.SelectedValue = drv["FLDLOGTYPE"].ToString();
                ddlComponentTypeEdit_SelectedIndexChanged(ct, null);
            }

            UserControlHard hrd = (UserControlHard)e.Row.FindControl("ddlHardEdit");
            if (hrd != null)
                hrd.SelectedHard = drv["FLDWAGEHEADID"].ToString();


            ImageButton ib1 = (ImageButton)e.Row.FindControl("btnShowBudgetEdit");
            if (ib1 != null) ib1.Attributes.Add("onclick", "showPickList('spnPickListBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudget.aspx?budgetgroup=102&hardtypecode=30', false); return false;");
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {

            DropDownList at = (DropDownList)e.Row.FindControl("ddlComponentTypeAdd");
            if (at != null)
            {
                DataSet dsCL = PhoenixRegistersPortageBillStandardComponent.ListPortageBillComponentHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1, 0, "OED,OBI,OCA,OAT,ORL,OBF,OSA,SOA,OPC,OFB,AWT,ODP,ORO,OSO,OFA");
                at.DataSource = dsCL;
                at.DataBind();
                at.Items.Insert(0, new ListItem("--Select--", ""));
            }

            UserControlHard hrd = (UserControlHard)e.Row.FindControl("ddlHardAdd");

            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
            ImageButton ib1 = (ImageButton)e.Row.FindControl("btnShowBudgetAdd");
            if (ib1 != null) ib1.Attributes.Add("onclick", "showPickList('spnPickListBudgetAdd', 'codehelp1', '', '../Common/CommonPickListBudget.aspx?budgetgroup=102&hardtypecode=30', false); return false;");

        }
    }

    protected void gvPBStdComp_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }


    private bool IsValidPBStandardComponent(string logtype, string desc, string budgetid)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableInteger(logtype).HasValue)
            ucError.ErrorMessage = "Component Type is required.";

        if (desc.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";

        if (!General.GetNullableInteger(budgetid).HasValue)
            ucError.ErrorMessage = "Budget Code is required.";

        return (!ucError.IsError);
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
    protected void ddlComponentTypeAdd_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlComponentTypeEdit_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddl.Parent.Parent;
        UserControlHard subtype = row.FindControl(row.RowType == DataControlRowType.Footer ? "ddlHardAdd" : "ddlHardEdit") as UserControlHard;

        if (General.GetNullableInteger(ddl.SelectedValue).HasValue && (General.GetNullableInteger(ddl.SelectedValue).Value == 1))
        {
            subtype.HardList = PhoenixRegistersHard.ListHard(1, 128, 0, "BSH,BSU,HRA,BSU,REM,BRF,AOT");
            subtype.Enabled = true;
        }
        else if (General.GetNullableInteger(ddl.SelectedValue).HasValue && (General.GetNullableInteger(ddl.SelectedValue).Value == 23 || General.GetNullableInteger(ddl.SelectedValue).Value == 24))
        {
            subtype.HardList = PhoenixRegistersHard.ListHard(1, 128, 0, "CWB,CWO");
            subtype.Enabled = true;
        }
        else
        {
            subtype.SelectedHard = "";
            subtype.Enabled = false;
        }

    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    public void BindOwnerPBMapData()
    {

        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;


            DataSet ds = PhoenixRegistersOwnerBudgetCodePBMap.OwnerBudgetCodeSearch(4
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvOwner.DataSource = ds;
                gvOwner.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvOwner);
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            SetPageNavigator();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOwner_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void gvOwner_Sorting(object sender, GridViewSortEventArgs se)
    {

    }
    protected void gvOwner_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {

    }
    protected void gvOwner_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
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
        BindOwnerPBMapData();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvOwner.SelectedIndex = -1;
        gvOwner.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;
        BindOwnerPBMapData();
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
}
