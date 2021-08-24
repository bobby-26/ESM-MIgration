using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.PlannedMaintenance;
using System.Collections;
using Telerik.Web.UI;


public partial class DocumentManagementComponentJobFormList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            //toolbar.AddImageLink("javascript:Openpopup('Reset','','DocumentManagementFormRevisionReset.aspx'); return false;", "Revision Reset", "in-progress.png", "RESET");            
            if (Request.QueryString["vesselid"] != null)
                toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementComponentJobFormList.aspx?vesselid="+ Request.QueryString["vesselid"] + "", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            else
                toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementComponentJobFormList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvForm')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            if (Request.QueryString["vesselid"] != null)
            {
                toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementComponentJobFormList.aspx?vesselid=" + Request.QueryString["vesselid"] + "", "Find", "<i class=\"fas fa-search\"></i>", "SEARCH");
                toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementComponentJobFormList.aspx?vesselid=" + Request.QueryString["vesselid"] + "", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            }else
            {
                toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementComponentJobFormList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "SEARCH");
                toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementComponentJobFormList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            }
            MenuRegistersCountry.AccessRights = this.ViewState;
            MenuRegistersCountry.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
            

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["FORMID"] = "";
                ViewState["FORMREVISIONID"] = "";
                ViewState["FORMTYPE"] = "";
                if (Request.QueryString["vesselid"] != null)
                    ViewState["VESSELID"] = Request.QueryString["vesselid"];
                else
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

                Filter.CurrentSelectedForms = null;

                ViewState["CATEGORYID"] = "";

                if (Request.QueryString["FORMID"] != null && Request.QueryString["FORMID"].ToString() != "")
                    ViewState["FORMID"] = Request.QueryString["FORMID"].ToString();
                else
                    ViewState["FORMID"] = "";

                if (Request.QueryString["FORMNO"] != null && Request.QueryString["FORMNO"].ToString() != "")
                {
                    ViewState["FORMNO"] = Request.QueryString["FORMNO"].ToString();
                }
                else
                    ViewState["FORMNO"] = "";

                if (Request.QueryString["COMPONENTJOBID"] != null && Request.QueryString["COMPONENTJOBID"].ToString() != "")
                    ViewState["COMPJOBID"] = Request.QueryString["COMPONENTJOBID"].ToString();
                else
                    ViewState["COMPJOBID"] = "";

                btnShowCategory.Attributes.Add("onclick", "return showPickList('spnPickListCategory', 'codehelp1', '', 'Common/CommonPickListDocumentCategory.aspx', true); ");
                gvForm.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                if (Request.QueryString["Callfrom"] != null)
                {
                    Filter.DMSFormFilterCriteria = null;
                }

                if (Filter.DMSFormFilterCriteria != null)
                {
                    NameValueCollection nvc = Filter.DMSFormFilterCriteria;

                    txtFormNo.Text = nvc.Get("txtformno").ToString();
                    txtFormName.Text = nvc.Get("txtformname").ToString();
                    txtCategory.Text = nvc.Get("txtCategory").ToString();

                }

            }
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

        string[] alColumns = { "FLDFORMNO", "FLDCAPTION", "FLDTYPENAME", "FLDCATEGORYNAME", "FLDACTIVESTATUS", "FLDPURPOSE", "FLDCOMPANYSHORTCODE", "FLDDATE", "FLDAUTHORNAME", "FLDREVISIONDETAILS" };
        string[] alCaptions = { "Form No.", "Name", "Type", "Category", "Active", "Remarks", "Company", "Added", "Added By", "Revision" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        string formno = "";
        if (ViewState["FORMNO"].ToString() != "")

        {
            formno = ViewState["FORMNO"].ToString();
        }
        else
        {
            formno = txtFormNo.Text;
        }

        ds = PhoenixPlannedMaintenanceComponentJob.FormSearch(
                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , General.GetNullableString(txtFormName.Text)
                                                    , null
                                                    , General.GetNullableGuid(txtCategoryid.Text)
                                                    , null
                                                    , sortexpression
                                                    , sortdirection
                                                    , 1
                                                    , gvForm.VirtualItemCount
                                                    , ref iRowCount
                                                    , ref iTotalPageCount
                                                    , General.GetNullableString(formno)
                                                    , companyid
                                                    , General.GetNullableGuid(ViewState["COMPJOBID"].ToString())
                                                    , int.Parse(ViewState["VESSELID"].ToString())
                                                    );

        Response.AddHeader("Content-Disposition", "attachment; filename=FormList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Form List</h3></td>");
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

    protected void RegistersCountry_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                Filter.DMSFormFilterCriteria = null;

                gvForm.CurrentPageIndex = 0;

                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("txtformno", txtFormNo.Text);
                nvc.Add("txtformname", txtFormName.Text);
                nvc.Add("txtCategoryid", txtCategoryid.Text);
                nvc.Add("txtCategory", txtCategory.Text);
               // nvc.Add("txtcontent", txtcontent.Text);

                Filter.DMSFormFilterCriteria = nvc;
                gvForm.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentDMSDocumentFilter = null;
                Filter.DMSFormFilterCriteria = null;
                ViewState["FORMID"] = "";
                txtFormNo.Text = "";
                txtFormName.Text = "";
                txtCategory.Text = "";
                txtCategoryid.Text = "";
                gvForm.CurrentPageIndex = 0;
                gvForm.Rebind();
            }
            else if (CommandName.ToUpper().Equals("RESET"))
            {

            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    
    protected void gvForm_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDFORMNO", "FLDCAPTION", "FLDTYPENAME", "FLDCATEGORYNAME", "FLDACTIVESTATUS", "FLDPURPOSE", "FLDCOMPANYSHORTCODE", "FLDDATE", "FLDAUTHORNAME", "FLDREVISIONDETAILS", "FLDDRAFTREVISION" };
        string[] alCaptions = { "Form No.", "Name", "Type", "Category", "Active", "Remarks", "Company", "Added", "Added By", "Revision", "Draft" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        string formno = "";
        if (ViewState["FORMNO"].ToString() != "")
        {
            formno = ViewState["FORMNO"].ToString();
        }
        else
        {
            formno = txtFormNo.Text;
        }

        if (Filter.DMSFormFilterCriteria != null)
        {
            NameValueCollection nvc = Filter.DMSFormFilterCriteria;

            ds = PhoenixPlannedMaintenanceComponentJob.FormSearch(
                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , General.GetNullableString(nvc.Get("txtformname").ToString())
                                                            , null
                                                            , General.GetNullableGuid(nvc.Get("txtCategoryid").ToString())
                                                            , null
                                                            , sortexpression
                                                            , sortdirection
                                                            , gvForm.CurrentPageIndex + 1
                                                            , gvForm.PageSize
                                                            , ref iRowCount
                                                            , ref iTotalPageCount
                                                            , General.GetNullableString(nvc.Get("txtformno").ToString())
                                                            , companyid
                                                           , General.GetNullableGuid(ViewState["COMPJOBID"].ToString())
                                                    , int.Parse(ViewState["VESSELID"].ToString())
                                                    );
        }
        else
        {
            ds = PhoenixPlannedMaintenanceComponentJob.FormSearch(
                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , General.GetNullableString(txtFormName.Text)
                                                            , null
                                                            , General.GetNullableGuid(txtCategoryid.Text)
                                                            , null
                                                            , sortexpression
                                                            , sortdirection
                                                            , gvForm.CurrentPageIndex + 1
                                                            , gvForm.PageSize
                                                            , ref iRowCount
                                                            , ref iTotalPageCount
                                                            , General.GetNullableString(formno)
                                                            , companyid
                                                            , General.GetNullableGuid(ViewState["COMPJOBID"].ToString())
                                                            , int.Parse(ViewState["VESSELID"].ToString())
                                                    );

        }

        General.SetPrintOptions("gvForm", "Form List", alCaptions, alColumns, ds);
        gvForm.DataSource = ds;
        gvForm.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        if (General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) == null)
        {   
            if (ds.Tables[0].Rows.Count >0)
                    ViewState["CATEGORYID"] = ds.Tables[0].Rows[0]["FLDCATEGORYID"].ToString();
        }

        if (General.GetNullableGuid(ViewState["FORMID"].ToString()) == null)
        {
            if(ds.Tables[0].Rows.Count > 0)
                { 
            ViewState["FORMID"] = ds.Tables[0].Rows[0]["FLDFORMID"].ToString();
            ViewState["FORMREVISIONID"] = ds.Tables[0].Rows[0]["FLDFORMREVISIONID"].ToString();
            ViewState["FORMTYPE"] = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
            }
        }
    }

    protected void gvForm_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

            if (e.CommandName.ToUpper().Equals("CHANGEPAGESIZE"))
                return;

            int nCurrentRow = e.Item.ItemIndex;

            if (e.CommandName == "RowClick" || (e.CommandName == RadGrid.ExpandCollapseCommandName) && (!e.Item.Expanded))
            {
                bool lastState = e.Item.Expanded;

                if (e.CommandName == RadGrid.ExpandCollapseCommandName || e.CommandName == "RowClick")
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    if (item.Expanded == false)
                    {
                        lastState = !lastState;
                        e.Item.Expanded = !lastState;
                    }
                }
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    
    private void AddNewRow(RadGrid gv, int row, string id, int headercount)
    {
        GridViewRow newRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal);
        TableCell cell = new TableCell();
        cell.HorizontalAlign = HorizontalAlign.Center;
        cell.ColumnSpan = gv.Columns.Count;
        cell.Text = "<div id='div" + id + "'></div>";
        newRow.Cells.Add(cell);
        gv.Controls[0].Controls.AddAt(row + headercount + 1, newRow);
    }

    

    protected void gvForm_ItemDataBound(Object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)item.DataItem;

            if (ViewState["Formid"] != null)
            {
                RadLabel lblFormid = (RadLabel)item.FindControl("lblFormid");
                if (lblFormid != null)
                {
                    if (ViewState["Formid"].ToString() == lblFormid.Text)
                    {
                        AddNewRow(gvForm, e.Item.ItemIndex, lblFormid.Text, 1);
                        ImageButton cmdBDetails = (ImageButton)e.Item.FindControl("cmdBDetails");
                        if (cmdBDetails != null)
                            cmdBDetails.ImageUrl = Session["images"] + "/downarrow.png";
                    }
                }
            }

            DataRowView dr = (DataRowView)e.Item.DataItem;

    

            HyperLink hlnkfilename = (HyperLink)e.Item.FindControl("lnkfilename");
    

            if (dr["FLDTYPE"] != null && dr["FLDTYPE"].ToString() == "1")
            {
                if (dr["FLDAPPROVEDREVISIONDTKEY"] != null && General.GetNullableGuid(dr["FLDAPPROVEDREVISIONDTKEY"].ToString()) != null)
                {
                    DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(dr["FLDAPPROVEDREVISIONDTKEY"].ToString()));
                    if (dt.Rows.Count > 0)
                    {
                        DataRow drRow = dt.Rows[0];
                        if (hlnkfilename != null)
                        { 
                            hlnkfilename.NavigateUrl = "../Common/download.aspx?dtkey=" + drRow["FLDDTKEY"].ToString();
                            //hlnkfilename.NavigateUrl = Session["sitepath"] + "/attachments/" + drRow["FLDFILEPATH"].ToString();
                        }
                    }
                }
                    
            }
            else
            {
                if (dr["FLDAPPROVEDREVISIONDTKEY"] != null && General.GetNullableGuid(dr["FLDAPPROVEDREVISIONDTKEY"].ToString()) != null)
                {
                    if (hlnkfilename != null && dr["FLDFORMBUILDERID"].ToString() != null)
                        hlnkfilename.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FormId=" + dr["FLDFORMID"].ToString() + "&FORMTYPE=DMSForm&FormName=" + dr["FLDFORMDESIGNNAME"].ToString() + "&FORMREVISIONID=" + dr["FLDFORMREVISIONID"].ToString() + "'); return false;");
                        //hlnkfilename.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentMangementFormDesignView.aspx?FormId=" + dr["FLDFORMBUILDERID"].ToString() + "&FormName=" + dr["FLDFORMDESIGNNAME"].ToString() + "'); return false;");
                        
                }
            }

            DataRowView drvCategory = (DataRowView)e.Item.DataItem;

        }

    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        //gvForm.CurrentPageIndex = 0;
        gvForm.Rebind();
    }

    
}
