using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewAppraisalMail : PhoenixBasePage
{
    string vesselid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            vesselid = Request.QueryString["vslid"];
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Send Mail", "MAIL");
            MenuAppraisal.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Crew/CrewList.aspx?vslid=" + vesselid + "&" + (Request.QueryString["access"] != null ? "?access=1" : string.Empty), "Find", "search.png", "FIND");
            MenuCrewList.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                txtVessel.Text = PhoenixRegistersVessel.EditVessel(int.Parse(vesselid)).Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;                                
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
    protected void MenuAppraisal_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("MAIL"))
        {
            string empid = string.Empty;
            int i = 1;
            string str = "<table cellspacing='1' cellpadding='1'>";
            foreach(GridViewRow r in gvCrewList.Rows)
            {                
                if (((CheckBox)r.FindControl("chkSelect")).Checked)
                {
                    empid += (((Label)r.FindControl("lblSGId")).Text + ",");
                    str += "<tr><td>"+(i++)+".&nbsp;</td><td>" + ((LinkButton)r.FindControl("lnkCrew")).Text + "</td><td>" + ((Label)r.FindControl("lblRank")).Text + "</td>" +
                        "<td>" + (string.IsNullOrEmpty(((Label)r.FindControl("lblSignOff")).Text) ? "&nbsp;" : ((Label)r.FindControl("lblSignOff")).Text) + "</td>" +
                        "<td>" + (string.IsNullOrEmpty(((Label)r.FindControl("lblSeaPort")).Text) ? "&nbsp;" : ((Label)r.FindControl("lblSeaPort")).Text) + "</td></tr>";
                }
            }
            str += "</table>";
            if (empid.TrimEnd(',') == string.Empty)
            {
                ucError.ErrorMessage = "Select atleast one or more seafarer";
                ucError.Visible = true;
                return;
            }
            Session["AppraisalMail"] = str;
            Response.Redirect("CrewEmail.aspx?csvsgid=" + empid.TrimEnd(',') + "&vslid=" + vesselid);
        }
    }
    protected void CrewList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
                SetPageNavigator();
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

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewManagement.SearchCrewOnboard(General.GetNullableInteger(vesselid), null
                                                        , General.GetNullableDateTime(txtOnDate.Text), 1, 0, null
                                                            , sortexpression, sortdirection
                                                            , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                                                            , ref iRowCount, ref iTotalPageCount,null,null,null);
        

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCrewList.DataSource = ds;
            gvCrewList.DataBind();          
        }
        else
        {          
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvCrewList);          
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvCrewList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                DataRowView drv = (DataRowView)e.Row.DataItem; 
                Label lblId = (Label)e.Row.FindControl("lblOverDue");
                Label type = (Label)e.Row.FindControl("lblType");
                if (lblId.Text == "1") e.Row.CssClass = "redfont";
                if (type.Text == "2") e.Row.CssClass = "bluefont";
                Label lblCrewId = (Label)e.Row.FindControl("lblCrewId");
                LinkButton lb = (LinkButton)e.Row.FindControl("lnkCrew");
                lb.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewPersonalGeneral.aspx?empid=" + lblCrewId.Text + "'); return false;");
                if (drv["FLDEMAILYN"].ToString() == "1") ((CheckBox)e.Row.FindControl("chkSelect")).Enabled = false;
                Label empid = (Label)e.Row.FindControl("lblEmployeeid");
                Label newapp = (Label)e.Row.FindControl("lblNewApp");
                ImageButton sg = (ImageButton)e.Row.FindControl("imgActivity");                
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

    protected void gvCrewList_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
        SetPageNavigator();
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
            gvCrewList.SelectedIndex = -1;
            gvCrewList.EditIndex = -1;
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
