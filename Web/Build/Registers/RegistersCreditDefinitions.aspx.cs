using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;

public partial class RegistersCreditDefinitions : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvCreditDefinitions.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        try
        {
            toolbar.AddImageButton("../Registers/RegistersCreditDefinitions.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('divGrid')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Registers/RegistersCreditDefinitions.aspx", "<b>Find</b>", "search.png", "FIND");
            MenuRegistersRank.AccessRights = this.ViewState;
            MenuRegistersRank.MenuList = toolbar.Show();
            MenuRegistersRank.SetTrigger(pnlRankEntry);
            
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = {"FLDCREDITDEFINITIONSNAME","FLDRANKNAME","FLDPOOLNAME","FLDNATIONALITYNAME","FLDAMOUNT"};
        string[] alCaptions = { "Creadit Defination Name","Rank","Pool","Nationality","Amount" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        ds = PhoenixRegistersCreditDefinitions.CreditDefinitionsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtSearch.Text,
                                       General.GetNullableInteger(ucSerachRank.SelectedRank),General.GetNullableInteger(ucSerchPool.SelectedPool),
                                      General.GetNullableInteger(ucSearchNationality.SelectedNationality), General.GetNullableDecimal(""), 
                                      sortexpression, sortdirection,
                                    (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

         Response.AddHeader("Content-Disposition", "attachment; filename=CreditDefinitions.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Session["sitepath"] + "/images/esmlogo4_small.png' /></td>");
        Response.Write("<td><h3>Credit Definitions Register</h3></td>");
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

    protected void cmdTest_Click(object sender, EventArgs e)
    {

    }

    protected void RegistersRank_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            SetPageNavigator();
        }
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
       


        DataSet ds = PhoenixRegistersCreditDefinitions.CreditDefinitionsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtSearch.Text,
                                       General.GetNullableInteger(ucSerachRank.SelectedRank),General.GetNullableInteger(ucSerchPool.SelectedPool),
                                      General.GetNullableInteger(ucSearchNationality.SelectedNationality), General.GetNullableDecimal(""), 
                                      sortexpression, sortdirection,
                                    (int)ViewState["PAGENUMBER"],General.ShowRecords(null), ref iRowCount,ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {

            gvCreditDefinitions.DataSource = ds;
            gvCreditDefinitions.DataBind();
        }
        else
        {

            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvCreditDefinitions);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

        BindData();
        SetPageNavigator();
    }

    protected void gvCreditDefinitions_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvCreditDefinitions_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        try
        {
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                InsertCreditDefinitions(0,
                    ((TextBox)_gridView.FooterRow.FindControl("txtCreditDefinitionsNameAdd")).Text,
                    ((UserControlRank)_gridView.FooterRow.FindControl("ucRankAdd")).SelectedRank,
                    ((UserControlPool)_gridView.FooterRow.FindControl("ucPoolAdd")).SelectedPool,
                    ((UserControlNationality)_gridView.FooterRow.FindControl("ucNationalityAdd")).SelectedNationality,
                    ((TextBox)_gridView.FooterRow.FindControl("txtAmountAdd")).Text
                );
                BindData();

            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                UpdateCreditDefinitions(
                     Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblCreditDefinitionsIdEdit")).Text),
                     ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCreditDefinitionsNameEdit")).Text,
                     ((UserControlRank)_gridView.Rows[nCurrentRow].FindControl("ucRankEdit")).SelectedRank,
                    ((UserControlPool)_gridView.Rows[nCurrentRow].FindControl("ucPoolEdit")).SelectedPool,
                    ((UserControlNationality)_gridView.Rows[nCurrentRow].FindControl("ucNationalityEdit")).SelectedNationality,
                     ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAmountEdit")).Text
                 );
                _gridView.EditIndex = -1;
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                _gridView.EditIndex = Int32.Parse(e.CommandArgument.ToString());
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteRank(Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblCreditDefinitionsId")).Text));
            }
            else
            {
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

    protected void gvCreditDefinitions_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvCreditDefinitions_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvCreditDefinitions_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                db.Attributes.Add("onclick", "return fnConfirmDelete()");
                
                //Label l = (Label)e.Row.FindControl("lblCreditDefinitionsId");

                //LinkButton lb = (LinkButton)e.Row.FindControl("lnkCreditDefinitionsName");
               // lb.Attributes.Add("onclick", "OpenDataForm('Registers/RegistersCreditDefinitions.aspx?Rankcode=" + l.Text + "');");
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Get the LinkButton control in the first cell
            LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
            // Get the javascript which is assigned to this LinkButton
            string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            // Add this javascript to the onclick Attribute of the row
            e.Row.Attributes["ondblclick"] = _jsDouble;
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

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        try
        {
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvCreditDefinitions.SelectedIndex = -1;
            gvCreditDefinitions.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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


    private void InsertCreditDefinitions(int creditdefinitionsid, string creditdefinitionsname, string rank, string pool, string nationality, string amount)
    {
        if (!IsValidRank(creditdefinitionsname, rank, pool, nationality, amount))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersCreditDefinitions.InsertCreditDefinitions(0, creditdefinitionsname, Int32.Parse(rank), Int32.Parse(pool), Int32.Parse(nationality), Decimal.Parse(amount));
    }

    private void UpdateCreditDefinitions(int creditdefinitionsid, string creditdefinitionsname, string rank, string pool, string nationality, string amount)
    {
        if (!IsValidRank(creditdefinitionsname, rank, pool, nationality, amount))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersCreditDefinitions.UpdateCreditDefinitions(0, creditdefinitionsid, creditdefinitionsname, Int32.Parse(rank), Int32.Parse(pool), Int32.Parse(nationality), Decimal.Parse(amount));
    }

    private bool IsValidRank(string creditdefinitionsname, string rank, string pool, string nationality, string amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        GridView _gridView = gvCreditDefinitions;

        if (creditdefinitionsname.Trim().Equals(""))
            ucError.ErrorMessage = "Credit Definitions name is required.";

        if (rank.Trim().Equals("") || IsInteger(rank)==false)
            ucError.ErrorMessage = "Rank name is required.";

        if (pool.Trim().Equals("") || IsInteger(pool) == false)
            ucError.ErrorMessage = "Pool is required.";

        if (nationality.Trim().Equals("") || IsInteger(nationality) == false)
            ucError.ErrorMessage = "Nationality is required.";

        if (amount.Trim().Equals("") || IsNumeric(amount) == false)
            ucError.ErrorMessage = "Amount is required. with numeric values";

        return (!ucError.IsError);
    }

    private void DeleteRank(int Rankcode)
    {
        PhoenixRegistersCreditDefinitions.DeleteCreditDefinitions(0, Rankcode);
    }
    private bool IsInteger(string strVal)
    {
        try
        {
            int doub = Int32.Parse(strVal);
            return true;
        }
        catch
        {
            return false;
        }
    }
    private bool IsNumeric(string strVal)
    {
        try
        {
            double doub = double.Parse(strVal);
            return true;
        }
        catch
        {
            return false;
        }
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
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
