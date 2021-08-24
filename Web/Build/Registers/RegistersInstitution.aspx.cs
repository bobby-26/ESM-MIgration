using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class RegistersInstitution : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvInstitution.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvInstitution.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Registers/RegistersInstitution.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvInstitution')", "Print Grid", "icon_print.png", "PRINT");
        toolbar.AddImageButton("../Registers/RegistersInstitution.aspx", "Find", "search.png", "FIND");
        toolbar.AddImageLink("javascript:Openpopup('AddInstitution','','RegistersInstitutionList.aspx')", "Add", "add.png", "ADDINSTITUTION");
        MenuInstitution.AccessRights = this.ViewState;
        MenuInstitution.MenuList = toolbar.Show();
        MenuInstitution.SetTrigger(pnlInstitutionEntry);
        
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
           
        }
        BindData();
        SetPageNavigator();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void MenuInstitution_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (dce.CommandName.ToUpper().Equals("FIND"))
        {
            gvInstitution.EditIndex = -1;
            gvInstitution.SelectedIndex = -1;
            ViewState["PAGENUMBER"] = 1;
            BindData();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNAME", "FLDADDRESS1", "FLDCITY", "FLDPHONENO1", "FLDEMAIL" };
        string[] alCaptions = { "Name", "Address1", "City", "Phone 1", "Email" };
       
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersInstitution.InstitutionSearch( null,txtInstitutionName.Text, sortexpression, sortdirection,
                          (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);
        General.SetPrintOptions("gvInstitution", "Registers", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvInstitution.DataSource = ds;
            gvInstitution.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvInstitution);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }   

    protected void gvInstitution_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            PhoenixRegistersInstitution.DeleteInstitution(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                 Convert.ToInt32(((Label)_gridView.Rows[nCurrentRow].FindControl("lblInstitutionid")).Text));           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        BindData();
        SetPageNavigator();
    }

    protected void gvInstitution_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if( del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                LinkButton lbtn = (LinkButton)e.Row.FindControl("lnkInstitutionName");
                lbtn.Attributes.Add("onclick", "Openpopup('AddInstitution', '', 'RegistersInstitutionList.aspx?institutionid=" + lbtn.CommandArgument + "'); return false;");

                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if(db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete()");                   
                }

            }
        }
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
    }

    protected void gvInstitution_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvInstitution.SelectedIndex = -1;
        gvInstitution.EditIndex = -1;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvInstitution_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvInstitution.SelectedIndex = e.NewSelectedIndex;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDNAME","FLDADDRESS1","FLDCITY", "FLDPHONENO1","FLDEMAIL"};
        string[] alCaptions = { "Name", "Address1","City","Phone 1","Email" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int sortdirection;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            sortdirection = 0;


        DataSet ds = PhoenixRegistersInstitution.InstitutionSearch(null, txtInstitutionName.Text, sortexpression, sortdirection
                      , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                      , ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Institution.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Institution </h3></td>");
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

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvInstitution.SelectedIndex = -1;
        gvInstitution.EditIndex = -1;

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
        gvInstitution.SelectedIndex = -1;
        gvInstitution.EditIndex = -1;
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
}
