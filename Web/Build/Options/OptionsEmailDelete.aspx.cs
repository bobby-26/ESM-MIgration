using System;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;

public partial class OptionsEmailDelete : PhoenixBasePage
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

    private void BindGridView()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixMail.MailSearch(General.GetNullableInteger(Filter.CurrentCrewSelection).Value, PhoenixMail.Folder.Delete
                                    , sortexpression, sortdirection
                                    , 1
                                    , General.ShowRecords(null)
                                    , ref iRowCount
                                    , ref iTotalPageCount);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDelete.DataSource = ds;
            gvDelete.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvDelete);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
    protected void gvDelete_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
        //    {
        //        ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
        //        db.Attributes.Add("onclick", "return fnConfirmDelete()");
        //    }
        //}
        string strAttachmentPath = HttpContext.Current.Server.MapPath("~/Attachments/EmailAttachments/");
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState != DataControlRowState.Edit && e.Row.RowState != (DataControlRowState.Alternate | DataControlRowState.Edit))
        {
            Image imgattach = (Image)(e.Row.FindControl("imgAttachment") as Image);
            Image imgpriority = (Image)(e.Row.FindControl("imgPriority") as Image);
            Label lblsessionId = (Label)(e.Row.FindControl("lblSessionId") as Label);
            Label lblpriority = (Label)(e.Row.FindControl("lblPriority") as Label);
            Label lblfileSize = (Label)(e.Row.FindControl("lblFileSize") as Label);
            LinkButton lnkBtnTo = (LinkButton)(e.Row.FindControl("lnkBtnTo") as LinkButton);

            if (!string.IsNullOrEmpty(lnkBtnTo.Text) && lnkBtnTo.Text.Length >= 30)
            {
                lnkBtnTo.Text = lnkBtnTo.Text.Substring(0, 27) + "...";
            }

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
    protected void ShowDeleteMail(object sender, CommandEventArgs e)
    {
        try
        {
            string[] values = Convert.ToString(e.CommandArgument).Split(','); ;
            string mailid = values[0].ToString();
            string sessionid = values[1].ToString();
            Response.Redirect("OptionsEmail.aspx?mailId=" + mailid + "&sessionId=" + sessionid + "&mailFolderId=" + PhoenixMail.Folder.Delete);
        }
        catch (Exception ex)
        {
            throw ex;
            //lblError.Text = ex.Message;
        }
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvDelete.SelectedIndex = -1;
            gvDelete.EditIndex = -1;
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
