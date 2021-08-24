using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class CommonViewAllAttachments : PhoenixBasePage
{
    //const int TimedOutExceptionCode = -2147467259;
    string attachmentcode = string.Empty;
    PhoenixModule module;


    string cmdname = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["dtkey"]))
        {
            attachmentcode = Request.QueryString["dtkey"];
            module = (PhoenixModule)(Enum.Parse(typeof(PhoenixModule), Request.QueryString["mod"]));
        }

        if (!IsPostBack)
        {
            if (Request.QueryString["VESSELID"] != null)
            {
                ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
            }
        }
        BindData();

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
                                                                        1,
                                                                        General.ShowRecords(null),
                                                                        ref iRowCount, ref iTotalPageCount);
        }


        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            gvAttachments.DataSource = ds;
            gvAttachments.DataBind();
        }
        else if (ds.Tables.Count > 0)
        {
            ShowNoRecordsFound(ds.Tables[0], gvAttachments);
        }

    }

    protected void gvAttachments_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblFileName = ((Label)e.Row.FindControl("lblFileName"));
            System.Web.UI.WebControls.Image imgtype = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgfiletype");
            HtmlControl htm = (HtmlControl)e.Row.FindControl("ifMoreInfo");
            if (lblFileName.Text != string.Empty)
            {
                Label lblFilePath = (Label)e.Row.FindControl("lblFilePath");                
                if (IsImage(lblFileName.Text.Substring(lblFileName.Text.LastIndexOf('.'))))
                {
                    imgtype.Visible = true;
                    htm.Visible = false;
                    imgtype.ImageUrl = "../Common/Download.aspx?dtkey=" + drv["FLDDTKEY"].ToString();
                    string filepath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + drv["FLDFILEPATH"].ToString().Replace('/', '\\');
                    if (File.Exists(filepath))
                    {
                        System.Drawing.Image im = System.Drawing.Image.FromFile(filepath);
                        if (im != null)
                        {
                            if (im.Width > 720 && im.Height > 540 && im.Width > im.Height)
                            {
                                imgtype.Width = 640;
                                imgtype.Height = 420;
                            }
                            else if (im.Width > 540 && im.Height > 720 && im.Width < im.Height)
                            {
                                imgtype.Width = 480;
                                imgtype.Height = 620;
                            }
                        }
                    }
                }
                else
                {
                    htm.Visible = true;
                    htm.Attributes.Add("src", "../Common/Download.aspx?dtkey=" + drv["FLDDTKEY"].ToString());
                    imgtype.Visible = false;
                }
            }

        }
    }

    private bool IsImage(string extn)
    {
        switch (extn.ToLower())
        {
            case ".jpg":
            case ".png":
            case ".gif":
            case ".bmp":
                return true;
            default:
                return false;
        }
    }


    private int? VesselId()
    {
        int? VesselId = 0;
        string menucode = Filter.CurrentMenuCodeSelection;
        if (menucode == "REG-GEN-VLM" || menucode.StartsWith("INV") || menucode.StartsWith("PMS")
            || menucode.StartsWith("PUR") || menucode.StartsWith("CRW-PBL") || menucode.StartsWith("VAC"))
        {
            if (menucode == "REG-GEN-VLM")
                VesselId = General.GetNullableInteger(Filter.CurrentVesselMasterFilter != null ? Filter.CurrentVesselMasterFilter : "0");
            else
                VesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        }
        return VesselId;
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
        gv.Rows[0].Cells[0].Text = "NO VALID IMAGE FOR PREVIEW";
        gv.Rows[0].Attributes["ondblclick"] = "";
    }
    protected void gvAttachments_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvAttachments.PageIndex = e.NewPageIndex;
            BindData();
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
}
