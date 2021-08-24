using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderReportLogFileAttachment : PhoenixBasePage
{
    string attachmentcode = string.Empty;
    PhoenixModule module;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!string.IsNullOrEmpty(Request.QueryString["dtkey"]) && !string.IsNullOrEmpty(Request.QueryString["mod"]))
        {
            attachmentcode = Request.QueryString["dtkey"];
            module = (PhoenixModule)(Enum.Parse(typeof(PhoenixModule), Request.QueryString["mod"]));
        }
        else
        {
            string msg = string.IsNullOrEmpty(Request.QueryString["dtkey"]) ? "Select a record to add attachment" : string.Empty;
            msg += string.IsNullOrEmpty(Request.QueryString["mod"]) ? "<br/>* Module is required for attachment" : string.Empty;
            ucError.ErrorMessage = msg;
            ucError.Visible = true;
        }
        if (!IsPostBack)
        {

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Upload", "UPLOAD",ToolBarDirection.Right);
            AttachmentList.AccessRights = this.ViewState;
            AttachmentList.MenuList = toolbarmain.Show();


            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
        }
        BindData();
        SetPageNavigator();
    }
    protected void AttachmentList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToString().ToUpper() == "UPLOAD")
        {
            if (!string.IsNullOrEmpty(attachmentcode))
            {
                try
                {
                    int? vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                    string type = Request.QueryString["type"] != null ? Request.QueryString["type"] : string.Empty;
                                        
                    PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(attachmentcode), module, null, string.Empty, string.Empty, General.GetNullableString(type), vesselid);
                  
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
                BindData();
                SetPageNavigator();
            }
            else
            {
                string msg = "Select a record to add attachment";
                msg += string.IsNullOrEmpty(Request.QueryString["mod"]) ? "<br/>* Module is required for attachment" : string.Empty;
                ucError.ErrorMessage = msg;
                ucError.Visible = true;
            }

        }

    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (attachmentcode != string.Empty)
        {
            string type = Request.QueryString["type"] != null ? Request.QueryString["type"] : string.Empty;
            ds = PhoenixCommonFileAttachment.AttachmentSearch(new Guid(attachmentcode), null, type, sortexpression, sortdirection,
                                                                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                        General.ShowRecords(null),
                                                                        ref iRowCount, ref iTotalPageCount);
        }


        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            gvAttachment.DataSource = ds;
            gvAttachment.DataBind();
        }
        else if (ds.Tables.Count > 0)
        {
            ShowNoRecordsFound(ds.Tables[0], gvAttachment);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

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
        gv.Rows[0].Attributes["ondblclick"] = "";
    }


    protected void gvAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);

                Label lblFileName = ((Label)e.Row.FindControl("lblFileName"));
                Image imgtype = (Image)e.Row.FindControl("imgfiletype");
                if (lblFileName.Text != string.Empty)
                {
                    imgtype.ImageUrl = ResolveImageType(lblFileName.Text.Substring(lblFileName.Text.LastIndexOf('.')));

                    Label lblFilePath = (Label)e.Row.FindControl("lblFilePath");
                    HyperLink lnk = (HyperLink)e.Row.FindControl("lnkfilename");
                    lnk.NavigateUrl = Session["sitepath"] + "/attachments/" + lblFilePath.Text;
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

    protected void gvAttachment_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            string dtkey = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDTKey")).Text;
            PhoenixCommonFileAttachment.AttachmentDelete(new Guid(dtkey));
            Label lblFilePath = (Label)_gridView.Rows[nCurrentRow].FindControl("lblFilePath");
            String path = MapPath("~/attachments/" + lblFilePath.Text);
            System.IO.File.Delete(path);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        BindData();
        SetPageNavigator();
    }

    protected void gvAttachment_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    private string ResolveImageType(string extn)
    {
        string imagepath = Session["images"] + "/";
        if (string.IsNullOrEmpty(extn)) extn = string.Empty;
        switch (extn.ToLower())
        {
            case ".jpg":
            case ".png":
            case ".gif":
            case ".bmp":
                imagepath += "imagefile.png";
                break;
            case ".doc":
                imagepath += "word.png";
                break;
            case ".xls":
                imagepath += "xls.png";
                break;
            case ".pdf":
                imagepath += "pdf.png";
                break;
            case ".rar":
            case ".zip":
                imagepath += "rar.png";
                break;
            case ".txt":
                imagepath += "notepad.png";
                break;
            default:
                imagepath += "anyfile.png";
                break;
        }
        return imagepath;
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
            gvAttachment.SelectedIndex = -1;
            gvAttachment.EditIndex = -1;
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
