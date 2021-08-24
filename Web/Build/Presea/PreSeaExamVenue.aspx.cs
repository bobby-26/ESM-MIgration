using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using System.Collections.Specialized;


public partial class PreSeaExamVenue : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../PreSea/PreSeaExamVenue.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvPreSea')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageLink("javascript:Openpopup('Filter','','../PreSea/PreSeaExamVenueFilter.aspx'); return false;", "Filter", "search.png", "FIND");
            toolbargrid.AddImageButton("../PreSea/PreSeaExamVenue.aspx", "Clear", "clear-filter.png", "CLEAR");
            toolbargrid.AddImageLink("javascript:Openpopup('AddExamVenue','','PreSeaExamVenueGeneral.aspx')", "Add", "add.png", "ADDVENUE");
            MenuPreSea.AccessRights = this.ViewState;
            MenuPreSea.MenuList = toolbargrid.Show();
            MenuPreSea.SetTrigger(pnlPreSeaRegister);
           
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
                ViewState["EXAMVENUEID"] = String.Empty;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;               
            }
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDEXAMVENUENAME", "FLDZONE", "FLDVENUEADDRESS", "FLDCONTACTPERSONNAME", "FLDPRESEAZONENAME" };
            string[] alCaptions = { "Venue Name", "Zone", "Address of the Venue", "Contacat Person", "SIMS Zone" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentPreSeaExamVenueFilter;
            DataSet ds = new DataSet();
            if (nvc != null)
            {
                ds = PhoenixPreSeaExamVenue.SearchExamVenue(General.GetNullableString(nvc["txtVenueName"])
                                , General.GetNullableString(nvc["txtAddress"]) 
                                , General.GetNullableInteger(nvc["ucCountry"])
                                , General.GetNullableInteger(nvc["ucState"])
                                , General.GetNullableInteger(nvc["ddlCity"])
                                , General.GetNullableInteger(nvc["ddlZone"].ToString())
                                , General.GetNullableString(nvc["txtPhone1"])
                                , General.GetNullableString(nvc["txtEmail1"])
                                , General.GetNullableString(nvc["txtContactName"])
                                , General.GetNullableString(nvc["txtContactPhone"])
                                , General.GetNullableString(nvc["txtContactMobile"])
                                , General.GetNullableString(nvc["txtContactMail"])
                                , sortexpression, sortdirection
                                , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
            }
            else
            {
                string NullValue = General.GetNullableString("");
                ds = PhoenixPreSeaExamVenue.SearchExamVenue(NullValue
                                , NullValue
                                , null
                                , null
                                , null
                                , null
                                , NullValue
                                , NullValue
                                , NullValue
                                , NullValue
                                , NullValue
                                , NullValue
                                , sortexpression, sortdirection
                                , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
            }

            General.ShowExcel("Pre-Sea Exam Venue", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuPreSea_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["EXAMVENUEID"] = String.Empty;
                Filter.CurrentPreSeaExamVenueFilter = null;
                BindData();
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDEXAMVENUENAME", "FLDZONE", "FLDVENUEADDRESS", "FLDCONTACTPERSONNAME", "FLDPRESEAZONENAME" };
            string[] alCaptions = { "Venue Name", "Zone", "Address of the Venue", "Contacat Person","SIMS Zone" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            NameValueCollection nvc = Filter.CurrentPreSeaExamVenueFilter;
            DataSet ds = new DataSet();
            if (nvc != null)
            {
                ds = PhoenixPreSeaExamVenue.SearchExamVenue(General.GetNullableString(nvc["txtVenueName"])
                                , General.GetNullableString(nvc["txtAddress"]) 
                                , General.GetNullableInteger(nvc["ucCountry"])
                                , General.GetNullableInteger(nvc["ucState"])
                                , General.GetNullableInteger(nvc["ddlCity"])
                                , General.GetNullableInteger(nvc["ddlZone"].ToString())
                                , General.GetNullableString(nvc["txtPhone1"])
                                , General.GetNullableString(nvc["txtEmail1"])
                                , General.GetNullableString(nvc["txtContactName"])
                                , General.GetNullableString(nvc["txtContactPhone"])
                                , General.GetNullableString(nvc["txtContactMobile"])
                                , General.GetNullableString(nvc["txtContactMail"])
                                , sortexpression, sortdirection
                                , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
            }
            else
            {
                string NullValue = General.GetNullableString("");
                ds = PhoenixPreSeaExamVenue.SearchExamVenue(NullValue
                                , NullValue
                                , null
                                , null
                                , null
                                , null
                                , NullValue
                                , NullValue
                                , NullValue
                                , NullValue
                                , NullValue
                                , NullValue
                                , sortexpression, sortdirection
                                , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
            }
            General.SetPrintOptions("gvPreSea", "Pre-Sea Exam Venue", alCaptions, alColumns,ds);

            if (ds.Tables[0].Rows.Count > 0)
            {                
                gvPreSea.DataSource = ds;
                gvPreSea.DataBind();
                if (String.IsNullOrEmpty(ViewState["EXAMVENUEID"].ToString()))
                {
                    ViewState["EXAMVENUEID"] = ds.Tables[0].Rows[0]["FLDEXAMVENUEID"].ToString();
                    gvPreSea.SelectedIndex = 0;
                }
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvPreSea);
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

    protected void gvPreSea_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;        
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToString().ToUpper() == "DELETE")
            {
                string VenueId = _gridView.DataKeys[nCurrentRow].Value.ToString();
                PhoenixPreSeaExamVenue.DeleteExamVenue(int.Parse(VenueId));
                ViewState["EXAMVENUEID"] = String.Empty;
                BindData();
                ucStatus.Text = "Exam Venue information Deleted Successfully.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPreSea_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["STORETYPEID"] = null;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        ViewState["EXAMVENUEID"] = String.Empty;
        BindData();
    }

    protected void gvPreSea_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        GridView _gridView = sender as GridView;
        gvPreSea.SelectedIndex = se.NewSelectedIndex;
        string venueid = ((Label)_gridView.Rows[se.NewSelectedIndex].FindControl("lblVenueId")).Text;
        ViewState["EXAMVENUEID"] = venueid;
        BindData();
    }

    protected void gvPreSea_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
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
                LinkButton lbtn = (LinkButton)e.Row.FindControl("lnkVenueName");
                if(lbtn != null)
                    lbtn.Attributes.Add("onclick", "Openpopup('EditCourse', '', '../PreSea/PreSeaExamVenueGeneral.aspx?venueid=" + lbtn.CommandArgument + "'); return false;");

                ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
                if (edit != null)
                {
                    edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
                    edit.Attributes.Add("onclick", "Openpopup('AddAddress', '', '../PreSea/PreSeaExamVenueGeneral.aspx?venueid=" + lbtn.CommandArgument + "'); return false;");
                }

                ImageButton del = (ImageButton)e.Row.FindControl("cmdDel");
                if (del != null)
                {
                    del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                    del.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to Delete ?'); return false;");
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        try
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
            ViewState["EXAMVENUEID"] = String.Empty;
            BindData();
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
            gvPreSea.SelectedIndex = -1;
            gvPreSea.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            ViewState["EXAMVENUEID"] = String.Empty;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetPageNavigator()
    {

        try
        {
            cmdPrevious.Enabled = IsPreviousEnabled();
            cmdNext.Enabled = IsNextEnabled();
            lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
            lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
            lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

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
            return true;

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
        try
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["EXAMVENUEID"] = String.Empty;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPreSea_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
