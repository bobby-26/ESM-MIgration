using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;

using System.Web.Profile;

public partial class RegistersSetDetails : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvSetDetails.Rows)
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
        PhoenixToolbar toolbar = new PhoenixToolbar();

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            toolbar.AddImageButton("../Registers/RegistersSetDetails.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvSetDetails')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Registers/RegistersSetDetails.aspx", "<b>Find</b>", "search.png", "FIND");
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
        string[] alColumns = { "FLDSETDETAILNAME","FLDSETDESCRIPTION","FLDRANKNAME"};
        string[] alCaptions = {"Set Name","Description", "Rank name" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        ds = PhoenixRegistersSetDetails.SetDetailsSearch(General.GetNullableInteger(ucAddress.SelectedAddress), General.GetNullableInteger(ucSerchRank.SelectedRank), General.GetNullableInteger(null), sortexpression, sortdirection,
                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=Rank.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Session["sitepath"] + "/images/esmlogo4_small.png' /></td>");
        Response.Write("<td><h3>Rank Register</h3></td>");
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
        try
        {
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
        string[] alColumns = { "FLDSETDETAILNAME", "FLDSETDESCRIPTION", "FLDRANKNAME" };
        string[] alCaptions = { "Set Name", "Description", "Rank name" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersSetDetails.SetDetailsSearch(General.GetNullableInteger(ucAddress.SelectedAddress), General.GetNullableInteger(ucSerchRank.SelectedRank), General.GetNullableInteger(null), sortexpression, sortdirection,
                        Int32.Parse(ViewState["PAGENUMBER"].ToString()), General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {

            gvSetDetails.DataSource = ds;
            gvSetDetails.DataBind();
        }
        else
        {

            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvSetDetails);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvSetDetails", "Working Ghearset", alCaptions, alColumns, ds);  
    }


    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSetDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvSetDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        try
        {
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                InsertSetDetails(ucAddress.SelectedAddress.ToString(),  
                     //((TextBox)_gridView.FooterRow.FindControl("txtSetDetailsNameAdd")).Text,
                    ((UserControlRank)_gridView.FooterRow.FindControl("ucRankAdd")).SelectedRank,
                     ((UserControlQuick)_gridView.FooterRow.FindControl("ucSetDescriptionAdd")).SelectedQuick
                );
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                UpdateSetDetails(
                     Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblSetDetailsIdEdit")).Text),
                     ucAddress.SelectedAddress.ToString(), 
                     //((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSetDetailsNameEdit")).Text,
                    ((UserControlRank)_gridView.Rows[nCurrentRow].FindControl("ucRankEdit")).SelectedRank,
                     ((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucSetDescriptionEdit")).SelectedQuick
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
                DeleteSetDetails(Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblSetDetailId")).Text));
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

    protected void gvSetDetails_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvSetDetails_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvSetDetails_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

        ImageButton add = (ImageButton)e.Row.FindControl("cmdAdd");
        if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                db.Attributes.Add("onclick", "return fnConfirmDelete()");

                //Label l = (Label)e.Row.FindControl("lblSetDetailId");

                //LinkButton lb = (LinkButton)e.Row.FindControl("lnkSetDetailName");
                //lb.Attributes.Add("onclick", "OpenDataForm('Registers/RegistersRankEntry.aspx?Rankcode=" + l.Text + "');");
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
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
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
        gvSetDetails.SelectedIndex = -1;
        gvSetDetails.EditIndex = -1;
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


    private void InsertSetDetails(string setdetailsname, string rank, string setdetailsdescription)
    {
        if (!IsValidSetDetails(setdetailsname, rank, setdetailsdescription))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersSetDetails.InsertSetDetails(0, Int64.Parse(setdetailsname), int.Parse(rank), int.Parse(setdetailsdescription));
    }

    private void UpdateSetDetails(int setdetailscode, string setdetailsname, string rank, string setdetailsdescription)
    {
        if (!IsValidSetDetails( setdetailsname, rank, setdetailsdescription))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersSetDetails.UpdateSetDetails(0, setdetailscode,  Int64.Parse(setdetailsname),int.Parse(rank), int.Parse(setdetailsdescription));
    }

    private bool IsValidSetDetails( string setdetailsname, string rank, string setdetailsdescription)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        GridView _gridView = gvSetDetails;

        if (setdetailsname.Trim().Equals("")|| General.GetNullableInteger(setdetailsname)==null  )
            ucError.ErrorMessage = "Manager or Owner name is required.";
        if (setdetailsdescription.Trim().Equals("") || General.GetNullableInteger(setdetailsdescription)==null )
            ucError.ErrorMessage = "Set Description is required.";
        if (rank.Trim().Equals("") || General.GetNullableInteger(rank)==null )
            ucError.ErrorMessage = "Rank is required.";        
        return (!ucError.IsError);
    }

    private void DeleteSetDetails(int setdetailscode)
    {
        PhoenixRegistersSetDetails.DeleteSetDetails(0, setdetailscode);
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
    protected void rdListOwnerManager_SelectedIndexChanged(object sender, EventArgs e)
    {   
      ucAddress.AddressList = PhoenixRegistersAddress.ListAddress( rdListOwnerManager.SelectedValue);
    }
}
