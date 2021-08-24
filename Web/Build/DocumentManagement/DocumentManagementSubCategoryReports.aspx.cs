using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.DocumentManagement;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.StandardForm;
using System.Web;
using Telerik.Web.UI;

public partial class DocumentManagementSubCategoryReports : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["VESSELID"] = null;
                ViewState["FTYPE"] = null;

                if (Request.QueryString["FORMID"] != null && Request.QueryString["FORMID"].ToString() != "")
                {
                    ViewState["FORMID"] = Request.QueryString["FORMID"].ToString();
                }
                if (Request.QueryString["VESSELID"] != null && Request.QueryString["VESSELID"].ToString() != "")
                {
                    ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
                }
                if (Request.QueryString["typeid"] != null && Request.QueryString["typeid"].ToString() != "")
                {
                    ViewState["FTYPE"] = Request.QueryString["typeid"].ToString();
                }

                ViewState["FIRSTINITIALIZED"] = true;
                gvFormList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                if (ViewState["FTYPE"].ToString() == "Upload")
                {
                    PhoenixToolbar toolbar = new PhoenixToolbar();
                    //toolbar.AddLinkButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementFMSFileNoAttachmentUpload.aspx?DRAWINGID=" + ViewState["DRAWINGID"].ToString() + "');", "Upload File", "<i class=\"fas fa-plus-circle\"></i>", "CREATE");
                    toolbar.AddLinkButton("javascript:openNewWindow('codehelp1','','DocumentManagement/DocumentManagementFMSShipboardFormsAttachmentUpload.aspx?FORMID=" + Request.QueryString["FORMID"].ToString() + "')", "Upload File", "UPLOAD", ToolBarDirection.Right);

                    MenuFomsUpload.AccessRights = this.ViewState;
                    MenuFomsUpload.MenuList = toolbar.Show();
                    cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuFomsUpload_TabStripCommand(object sender, EventArgs e)
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

    protected void gvFormList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFormList.CurrentPageIndex + 1;
        BindData();
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 10;
            int iTotalPageCount = 10;
            string[] alColumns = { "FLDREPORTNAME", "FLDCREATEDDATE" };
            string[] alCaptions = { "Name", "Created On" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            string jsonschema = null;
            Guid? Formid = General.GetNullableGuid(ViewState["FORMID"].ToString());
            int? vesselid = (ViewState["VESSELID"] == null) ? null : General.GetNullableInteger(ViewState["VESSELID"].ToString());
            DataSet ds = PhoenixFormBuilder.DmsDashboardReportSearch(Formid
                                                        , sortexpression
                                                        , sortdirection
                                                        , gvFormList.CurrentPageIndex + 1
                                                        , gvFormList.PageSize
                                                        , ref iRowCount
                                                        , ref iTotalPageCount
                                                        , jsonschema
                                                        , vesselid);

            General.SetPrintOptions("gvFormList", "Form List", alCaptions, alColumns, ds);
            gvFormList.DataSource = ds;
            gvFormList.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvFormList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
    protected void gvFormList_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
        gvFormList.Rebind();
    }

    protected void gvFormList_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                DataRowView dr = (DataRowView)e.Item.DataItem;
                string reportId = ((Label)e.Item.FindControl("lblReportId")).Text;
                string formId = ((Label)e.Item.FindControl("lblFormId")).Text;
                string statusname = ((Label)e.Item.FindControl("lblStatusname")).Text;
                HyperLink lnkfile = (HyperLink)e.Item.FindControl("lnkReportName");

                //LinkButton lnkReportName = (LinkButton)e.Item.FindControl("lnkReportName");
                if (lnkfile != null)
                {
                    if (ViewState["FTYPE"].ToString() != null && ViewState["FTYPE"].ToString() == "Upload")
                    {
                        DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(dr["FLDREPORTID"].ToString()));
                        if (dt.Rows.Count > 0)
                        {
                            DataRow drRow = dt.Rows[0];
                            if (lnkfile != null)
                            {
                                lnkfile.NavigateUrl = "../Common/download.aspx?dtkey=" + drRow["FLDDTKEY"].ToString();
                                //hlnkfilename.NavigateUrl = Session["sitepath"] + "/attachments/" + drRow["FLDFILEPATH"].ToString();
                            }
                        }
                        //hlnkfilename.NavigateUrl = "../Common/download.aspx?dtkey=" + drRow["FLDDTKEY"].ToString();
                    }
                    else
                    {
                        lnkfile.Attributes.Add("onclick", "javascript:return openNewWindow('codehelp1','','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?ReportId=" + reportId + "&FORMTYPE=DMSForm'); return false;");
                    }
                }

                LinkButton cmddelete = (LinkButton)e.Item.FindControl("cmdDelete");
                if (cmddelete != null)
                {
                    cmddelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    cmddelete.Visible = SessionUtil.CanAccess(this.ViewState, cmddelete.CommandName);
                }
                LinkButton ce = (LinkButton)e.Item.FindControl("cmdEdit");
                if (ce != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, ce.CommandName)) ce.Visible = false;
                }

            }
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
            BindData();
            gvFormList.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucTree_SelectNodeEvent(object sender, EventArgs e)
    {
        try
        {
            RadTreeNodeEventArgs tvsne = (RadTreeNodeEventArgs)e;

            if (tvsne.Node.Value != null)
            {
                string selectednode = tvsne.Node.Value.ToString();
                string selectedvalue = tvsne.Node.Text.ToString();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CategoryId"] = selectednode;
            }
            //string script = "var ar = document.getElementById(\"tvwComponent_tvwTreet0\"); ar.href=\"#\"; ar.onclick=null;\r\n;resizew();";
            //ScriptManager.RegisterStartupScript(this, typeof(Page), "disableScript", script, true);
            BindData();
            gvFormList.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void formUpdate(string formId, string frmname, int activeYn, string publishDate)
    {
        try
        {
            PhoenixFormBuilder.FormUpdate(new Guid(formId), frmname, activeYn, General.GetNullableDateTime(publishDate));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}