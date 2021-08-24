using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;

public partial class OwnersVesselCertificatesArchived : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvVesselCertificatesArchieve.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvVesselCertificatesArchieve.UniqueID, "Select$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

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
            BindData();
            SetPageNavigator();
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

        string[] alColumns = { "FLDCERTIFICATENAME", "FLDCERTIFICATENO", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDISSUINGAUTHORITYNAME" };
        string[] alCaptions = { "Certificate", "Number", "Issue Date", "Expiry Date", "Authority" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixOwnersVesselCertificates.VesselCertificatesSearch(Int16.Parse(Filter.CurrentOwnerVesselMasterFilter), null, "", null, null, null, "", 0, sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvVesselCertificates", "Certificates", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvVesselCertificatesArchieve.DataSource = ds;
            gvVesselCertificatesArchieve.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvVesselCertificatesArchieve);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
    protected void gvVesselCertificates_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvVesselCertificatesArchieve.SelectedIndex = e.NewSelectedIndex;
    }
    protected void gvVesselCertificates_ItemDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
               && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {

                Label lblDTKey = (Label)e.Row.FindControl("lblDTKey");
                ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");                
                Label lblIsAtt = (Label)e.Row.FindControl("lblIsAtt");
                               
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;
                if (lblIsAtt.Text == string.Empty) att.ImageUrl = Session["images"] + "/no-attachment.png";

                att.Attributes.Add("onclick", "javascript:Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                    + PhoenixModule.REGISTERS + "&U=1'); return false;");

                Label expdate = e.Row.FindControl("lblDateOfExpiry") as Label;
                System.Web.UI.WebControls.Image imgFlag = e.Row.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
                DateTime? d = General.GetNullableDateTime(expdate.Text);
                if (d.HasValue)
                {
                    TimeSpan t = d.Value - DateTime.Now;
                    if (t.Days >= 0 && t.Days < 120)
                    {
                        //e.Row.CssClass = "rowyellow";
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                    }
                    else if (t.Days < 0)
                    {
                        //e.Row.CssClass = "rowred";
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/red.png";
                    }
                }
            }
            else
            {
                e.Row.Attributes["onclick"] = "";
            }
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
            gvVesselCertificatesArchieve.SelectedIndex = -1;
            gvVesselCertificatesArchieve.EditIndex = -1;
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
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
