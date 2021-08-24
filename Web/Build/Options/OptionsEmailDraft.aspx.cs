using System;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using System.Data;


public partial class OptionsEmailDraft : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                //string Script = "";
                //Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                //Script += "parent.selectTab('CrewMenuGeneral', 'Drafts');";
                //Script += "</script>" + "\n";
                //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
            BindGridView();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            throw ex;
            //lblError.Text = ex.Message;
        }
    }


    protected void DeleteMail(object sender, CommandEventArgs e)
    {
        try
        {
            int emailid = Convert.ToInt32(e.CommandArgument);
            PhoenixMail.EmailChangeFolder(emailid, PhoenixMail.Folder.Delete);
            Session["frmcontentpage"] = "OptionsEmailDraft.aspx";
            Response.Redirect("OptionsEmailDraft.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
            //lblError.Text = ex.Message;
        }
    }

    private void BindGridView()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixMail.MailSearch(General.GetNullableInteger(Filter.CurrentCrewSelection).Value, PhoenixMail.Folder.Draft
                                    , sortexpression, sortdirection
                                    , 1
                                    , General.ShowRecords(null)
                                    , ref iRowCount
                                    , ref iTotalPageCount);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDraft.DataSource = ds;
            gvDraft.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvDraft);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        foreach (GridViewRow row in gvDraft.Rows)
        {
            
        }
    }
    protected void gvDraft_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            }
        }
        string strAttachmentPath = HttpContext.Current.Server.MapPath("~/Attachments/EmailAttachments/");
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState != DataControlRowState.Edit && e.Row.RowState != (DataControlRowState.Alternate | DataControlRowState.Edit))
        {
            Image imgattach = (Image)(e.Row.FindControl("imgAttachment") as Image);
            Image imgpriority = (Image)(e.Row.FindControl("imgPriority") as Image);
            Label lblsessionId = (Label)(e.Row.FindControl("lblSessionId") as Label);
            Label lblpriority = (Label)(e.Row.FindControl("lblPriority") as Label);
            Label lblfileSize = (Label)(e.Row.FindControl("lblFileSize") as Label);

            string mailsessionid = lblsessionId.Text;
            if (!string.IsNullOrEmpty(lblpriority.Text))
            {
                int priority = Convert.ToInt16(lblpriority.Text);
                if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Attachments/EmailAttachments/" + mailsessionid)))
                {
                    imgattach.Visible = false;
                }
                else
                {
                    string unit = "KB";
                    string[] files = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Attachments/EmailAttachments/" + mailsessionid));
                    double fileSize = 0;
                    foreach (string name in files)
                    {
                        FileInfo info = new FileInfo(name);
                        fileSize += info.Length;
                    }

                    fileSize = Math.Round(fileSize / 1024);

                    if (fileSize > 1024)
                    {
                        fileSize = Math.Round(fileSize / 1024, 2);
                        unit = "MB";
                    }

                    lblfileSize.Text = fileSize.ToString() + " " + unit;
                }

                if (priority == 2)
                {
                    imgpriority.ImageUrl = Session["images"] + "/highPriority.png";
                }
                else
                {
                    imgpriority.Visible = false;
                }
            }
        }
    }
    protected void gvDraft_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string strEmailId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmailId")).Text;
            
            PhoenixMail.EmailChangeFolder(int.Parse(strEmailId), PhoenixMail.Folder.Delete);         
            BindGridView();
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
            gvDraft.SelectedIndex = -1;
            gvDraft.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindGridView();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
            BindGridView();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
