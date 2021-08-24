using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Integration;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
using System.Configuration;
using SouthNests.Phoenix.Registers;
using System.Collections;

public partial class Inspection_InspectionTMSAMatrixCategoryContentArchivedList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        MenuDocumentCategoryMain.AccessRights = this.ViewState;
        MenuDocumentCategoryMain.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["CATEGORYID"] = "";
            ViewState["INSPECTIONID"] = "";
            ViewState["INSPECTIONSCHEDULEID"] = "";            

            if (Request.QueryString["categoryid"] != null && Request.QueryString["categoryid"].ToString() != string.Empty)
                ViewState["CATEGORYID"] = Request.QueryString["categoryid"].ToString();

            if (Request.QueryString["inspectionid"] != null && Request.QueryString["inspectionid"].ToString() != string.Empty)
                ViewState["INSPECTIONID"] = Request.QueryString["inspectionid"].ToString();
            BindData();
            BindInspectionReferenceNumber();            
        }
        
        setMenu();      

    }

    protected void BindInspectionReferenceNumber()
    {
        DataSet ds = PhoenixInspectionTMSAMatrix.ListTMSAMatrixReferenceNumberList(General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()));

        ViewState["INSPECTIONSCHEDULEID"] = ddlRefNo.SelectedValue;
        ddlRefNo.DataSource = ds.Tables[0];
        ddlRefNo.DataTextField = "FLDREFERENCENUMBER";
        ddlRefNo.DataValueField = "FLDREVIEWSCHEDULEID";
        ddlRefNo.DataBind();
        ddlRefNo.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    private void setMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddFontAwesomeButton("../Inspection/InspectionTMSAMatrixCategoryContentArchivedList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");

        MenuPhoenixQuery.AccessRights = this.ViewState;
        MenuPhoenixQuery.MenuList = toolbar.Show();
        MenuPhoenixQuery.Visible = true;
    }

    protected void MenuPhoenixQuery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;


        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }

    }

    protected void ShowExcel()
    {

        DataTable dt = new DataTable();
        dt = PhoenixInspectionTMSAMatrix.EditTMSAMatrixColumnHeaders(General.GetNullableGuid(ViewState["CATEGORYID"].ToString()));

        string captions = "";
        string columns = "";
        string columnname = "FLDCOLUMN";
        int noofcolumns = int.Parse(dt.Rows[0]["FLDNOOFCOLUMNS"].ToString());

        if (dt.Rows.Count > 0)
        {
            for (int i = 1; i <= noofcolumns; i++)
            {
                string colheader = "FLDCOLUMNTEXT";

                foreach (DataRow dr in dt.Rows)
                {
                    colheader = colheader + i.ToString();
                    if (captions.Equals(""))
                        captions = dr[colheader].ToString();
                    else
                        captions = captions + "," + dr[colheader].ToString();

                    if (columns.Equals(""))
                        columns = columnname + i.ToString();
                    else
                        columns = columns + "," + columnname + i.ToString();
                }
            }
        }

        columns = columns + ",FLDPROCEDURES,FLDOBJECTIVEEVIDENCE,FLDDEPARTMENTNAMELIST,FLDCOMPLY,FLDISOFFICEACCEPTED";
        captions = captions + ",Procedures, Objective Evidence,Responsibility,Comply,Accepted";

        string[] alColumns = columns.Split(',');
        string[] alCaptions = captions.Split(',');


        DataTable dtcolumn = new DataTable();

        dtcolumn = PhoenixInspectionTMSAMatrix.TMSAMatrixCategoryContentArchivedSearch(General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()),
                                                                                General.GetNullableGuid(ddlRefNo.SelectedValue)
                                                                                );

        if (dtcolumn.Rows.Count > 0)
        {
            ShowQuery(General.GetNullableGuid(ViewState["CATEGORYID"].ToString()), dt, dtcolumn);
        }
        else
        {
            gvTMSAMatrix.DataSource = dtcolumn;
            gvTMSAMatrix.DataBind();
        }

 
        ShowExcel("TMSA Matrix - Archived", dtcolumn, alColumns, alCaptions, null, null);
    }
    public static void ShowExcel(string strHeading, DataTable dt, string[] alColumns, string[] alCaptions, int? strSortDirection, string strSortExpression)
    {

        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + (string.IsNullOrEmpty(strHeading) ? "Attachment" : strHeading.Replace(" ", "_")) + ".xls");
        HttpContext.Current.Response.ContentType = "application/vnd.msexcel";
        HttpContext.Current.Response.Write("<style>.text{ mso-number-format:\"\\@\";}</style>");
        HttpContext.Current.Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");

        HttpContext.Current.Response.Write("<tr>");
        HttpContext.Current.Response.Write("<td ><img src='http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        HttpContext.Current.Response.Write("<td colspan='" + (alColumns.Length - 1).ToString() + "'><h3>&nbsp&nbsp&nbsp&nbsp " + strHeading + "-" + dt.Rows[0]["FLDCATEGORYNAME"].ToString() + "</h3></td>");
        HttpContext.Current.Response.Write("</tr>");
        HttpContext.Current.Response.Write("</TABLE>");
        HttpContext.Current.Response.Write("<br />");
        HttpContext.Current.Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        HttpContext.Current.Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            HttpContext.Current.Response.Write("<td width='20%'>");
            HttpContext.Current.Response.Write("<b>" + alCaptions[i] + "</b>");
            HttpContext.Current.Response.Write("</td>");
        }
        HttpContext.Current.Response.Write("</tr>");
        foreach (DataRow dr in dt.Rows)
        {
            HttpContext.Current.Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                HttpContext.Current.Response.Write(dr[alColumns[i]].GetType().Equals(typeof(string)) ? "<td  class='text'>" : "<td>");
                HttpContext.Current.Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                HttpContext.Current.Response.Write("</td>");
            }
            HttpContext.Current.Response.Write("</tr>");
        }
        HttpContext.Current.Response.Write("</TABLE>");
        HttpContext.Current.Response.Write("<br/>");
        HttpContext.Current.Response.Write("<br/>");
        HttpContext.Current.Response.Write(ConfigurationManager.AppSettings["softwarename"].ToString());
        HttpContext.Current.Response.End();
    }

    //public override void VerifyRenderingInServerForm(Control control)
    //{
    //}


    protected void gvTMSAMatrix_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
            //BindData();
    }

    private void BindData()
    {
        try
        {

            DataTable dt = new DataTable();
            dt = PhoenixInspectionTMSAMatrix.EditTMSAMatrixColumnHeaders(General.GetNullableGuid(ViewState["CATEGORYID"].ToString()));

            DataTable dtcolumn = new DataTable();

            dtcolumn = PhoenixInspectionTMSAMatrix.TMSAMatrixCategoryContentArchivedSearch(General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()),
                                                                                    General.GetNullableGuid(ddlRefNo.SelectedValue)
                                                                                    );

            if (dtcolumn.Rows.Count > 0)
            {
                ShowQuery(General.GetNullableGuid(ViewState["CATEGORYID"].ToString()), dt, dtcolumn);
            }
            else
            {
                gvTMSAMatrix.DataSource = dtcolumn;
                gvTMSAMatrix.DataBind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTMSAMatrix_RowCommand(object sender, GridCommandEventArgs e)
    {

        GridEditableItem eeditedItem = e.Item as GridEditableItem;
        RadGrid _gridView = (RadGrid)sender;

        int nCurrentRow = e.Item.ItemIndex;

        if (e.CommandName.ToUpper().Equals("EDIT"))
        {

        }

        if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            Guid lblid = new Guid(item.GetDataKeyValue("FLDCONTENTID").ToString());
            BindData();
        }

        if (e.CommandName.ToUpper().Equals("BPGATTACHMENT"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            Guid lblid = new Guid(item.GetDataKeyValue("FLDCONTENTID").ToString());
            BindData();
        }

        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
        }

        if (e.CommandName.ToUpper().Equals("COMMENTS"))
        {
            GridDataItem item = (GridDataItem)e.Item;

            Guid lblid = new Guid(item.GetDataKeyValue("FLDCONTENTID").ToString());
        }

        if (e.CommandName.ToUpper().Equals("ADD"))
        {
        }
    }

    protected void gvTMSAMatrix_ItemDataBound(Object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            HtmlTable tbl = (HtmlTable)e.Item.FindControl("tblForms");
            HtmlTable tblReports = (HtmlTable)e.Item.FindControl("tblReports");

            RadLabel lblid = (RadLabel)e.Item.FindControl("lblid");
            if (lblid != null)
            {
                ViewState["CONTENTID"] = lblid.Text;
            }
            else
            {
                ViewState["CONTENTID"] = null;
            }

            LinkButton cmd = (LinkButton)e.Item.FindControl("cmdComments");
            if (cmd != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmd.CommandName))
                    cmd.Visible = false;

                cmd.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionTMSAMatrixComments.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&contentid=" + General.GetNullableGuid(ViewState["CONTENTID"].ToString()) + "'); return false;");
            }

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            LinkButton BPGAtt = (LinkButton)e.Item.FindControl("BPGAtt");
            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                HtmlGenericControl html = new HtmlGenericControl();
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                    att.Controls.Add(html);
                }
                else
                {
                    html.InnerHtml = "<span class=\"icon\" style=\"color:skyblue\"><i class=\"fas fa-paperclip\"></i></span>";
                    att.Controls.Add(html);
                }
                //    att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.QUALITY + "&type=SHIPBOARDEVIDENCE&cmdname=SHIPBOARDEVIDENCEUPLOAD&VESSELID=0" + "'); return false;");
            }

            if (BPGAtt != null)
            {
                BPGAtt.Visible = SessionUtil.CanAccess(this.ViewState, BPGAtt.CommandName);
                HtmlGenericControl html = new HtmlGenericControl();
                if (drv["FLDISBPGATTACHMENT"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                    BPGAtt.Controls.Add(html);
                }
                else
                {
                    html.InnerHtml = "<span class=\"icon\" style=\"color:skyblue\"><i class=\"fas fa-paperclip\"></i></span>";
                    BPGAtt.Controls.Add(html);
                }
                //    att.ImageUrl = Session["images"] + "/no-attachment.png";
                BPGAtt.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.QUALITY + "&type=BPGEXAMPLE&cmdname=SHIPBOARDEVIDENCEUPLOAD&VESSELID=0" + "'); return false;");
            }

            LinkButton cmdClient = (LinkButton)e.Item.FindControl("cmdClientComments");
            if (cmdClient != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdClient.CommandName))
                    cmdClient.Visible = false;

                cmdClient.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionTMSAClientBPGComments.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&contentid=" + General.GetNullableGuid(ViewState["CONTENTID"].ToString()) + "'); return false;");
            }

            LinkButton cmdOfficeChecks = (LinkButton)e.Item.FindControl("cmdOfficeChecks");
            LinkButton cmdarchive = (LinkButton)e.Item.FindControl("cmdarchive");
            RadCheckBox chkSelect = (RadCheckBox)e.Item.FindControl("chkSelect");
            if (cmdOfficeChecks != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdOfficeChecks.CommandName))
                    cmdOfficeChecks.Visible = false;

                cmdOfficeChecks.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionTMSAMatrixOfficeChecks.aspx?Archived=1&categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&contentid=" + General.GetNullableGuid(ViewState["CONTENTID"].ToString()) + "&inspectionid=" + General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()) + "&inspectionscheduleid=" + General.GetNullableGuid(drv["FLDINSPECTIONSCHEDULEID"].ToString()) + "',false,600,350); return false;");
            }

            LinkButton lnkprocedure = (LinkButton)e.Item.FindControl("lnkprocedure");

            if (lnkprocedure != null)
            {
                if (drv["FLDFORMPOSTERCHECKLISTS"].ToString() != "" || drv["FLDPROCEDURES"].ToString() != "")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    //lnkprocedure.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionTMSAMatrixMappedProcedure.aspx?contentid=" + General.GetNullableGuid(ViewState["CONTENTID"].ToString()) + "'); return false;");
                    lnkprocedure.Attributes.Add("onclick", "openNewWindow('Filter1', 'Procedure', '" + Session["sitepath"] + "/Inspection/InspectionTMSAMatrixMappedProcedure.aspx?contentid=" + General.GetNullableGuid(ViewState["CONTENTID"].ToString()) + "&revisionid=" + drv["FLDREVISIONID"].ToString() + "',false,500,400); return true;");
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-file-contract-af\"></i></span>";
                    lnkprocedure.Controls.Add(html);
                    lnkprocedure.ToolTip = "Procedure";
                    lnkprocedure.Enabled = false;
                }
                else
                    lnkprocedure.Visible = false;
            }

            //if (lblid != null)
            //{

            //    DataSet dss = PhoenixInspectionTMSAMatrix.ArchivedFormPosterList(ViewState["CONTENTID"] == null ? null : General.GetNullableGuid(ViewState["CONTENTID"].ToString()),
            //                                            General.GetNullableGuid(ViewState["CATEGORYID"].ToString()), 0
            //                                               , General.GetNullableGuid(drv["FLDREVISIONID"].ToString()));
            //    int number = 1;
            //    if (dss.Tables[0].Rows.Count > 0 && dss.Tables[0].Columns.Count > 1)
            //    {
            //        tbl.Rows.Clear();
            //        foreach (DataRow dr in dss.Tables[0].Rows)
            //        {
            //            HyperLink hl = new HyperLink();
            //            hl.Text = dr["FLDNAME"].ToString();
            //            hl.ID = "hlink" + number.ToString();
            //            hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");

            //            int type = 0;
            //            PhoenixIntegrationQuality.GetSelectedeTreeNodeType(General.GetNullableGuid(dr["FLDFORMPOSTERID"].ToString()), ref type);
            //            if (type == 2)
            //                hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'DocumentManagement/DocumentManagementDocumentGeneral.aspx?showall=0&DOCUMENTID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");
            //            else if (type == 3)
            //                hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?callfrom=documenttree&SECTIONID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");
            //            else if (type == 5)
            //            {
            //                    hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + dr["FLDFORMPOSTERID"].ToString() + "&FORMREVISIONID=" + "');return false;");
            //            }
            //            else if (type == 6)
            //            {
            //                hl.Target = "_blank";
            //                hl.NavigateUrl = "../Common/download.aspx?formid=" + dr["FLDFORMPOSTERID"].ToString();
            //            }

            //            HtmlTableRow tr = new HtmlTableRow();
            //            HtmlTableCell tc = new HtmlTableCell();
            //            //tc.Controls.Add(cb);
            //            //tr.Cells.Add(tc);
            //            tc = new HtmlTableCell();
            //            tc.Controls.Add(hl);
            //            tr.Cells.Add(tc);
            //            tbl.Rows.Add(tr);
            //            tc = new HtmlTableCell();
            //            tc.InnerHtml = "<br/>";
            //            tr.Cells.Add(tc);
            //            tbl.Rows.Add(tr);
            //            number = number + 1;
            //        }
            //        tbl.Visible = true;
            //    }


            //    if (drv["FLDFORMREPORTLIST"] != null)
            //    {
            //        DataSet dsreports = PhoenixInspectionTMSAMatrix.FormReportList(ViewState["CONTENTID"] == null ? null : General.GetNullableGuid(ViewState["CONTENTID"].ToString()),
            //                                                General.GetNullableGuid(ViewState["CATEGORYID"].ToString()));
            //        int no = 1;
            //        if (dsreports.Tables[0].Rows.Count > 0 && dsreports.Tables[0].Columns.Count > 1)
            //        {
            //            tblReports.Rows.Clear();
            //            foreach (DataRow dr in dsreports.Tables[0].Rows)
            //            {
            //                HyperLink hlReports = new HyperLink();
            //                hlReports.Text = dr["FLDNAME"].ToString();
            //                hlReports.ID = "hlReportslink" + no.ToString();
            //                hlReports.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");
            //                hlReports.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&ReportId=" + dr["FLDFORMREPORTID"].ToString() + "');return false;");
            //                HtmlTableRow tr = new HtmlTableRow();
            //                HtmlTableCell tc = new HtmlTableCell();
            //                //tc.Controls.Add(cb);
            //                //tr.Cells.Add(tc);
            //                tc = new HtmlTableCell();
            //                tc.Controls.Add(hlReports);
            //                tr.Cells.Add(tc);
            //                tblReports.Rows.Add(tr);
            //                tc = new HtmlTableCell();
            //                tc.InnerHtml = "<br/>";
            //                tr.Cells.Add(tc);
            //                tblReports.Rows.Add(tr);
            //                no = no + 1;
            //            }
            //            tblReports.Visible = true;
            //        }
            //    }
            //}
        }

        if (e.Item is GridFooterItem)
        {
            GridFooterItem item = (GridFooterItem)e.Item;
            RadCheckBoxList chkDeptList = ((RadCheckBoxList)e.Item.FindControl("chkDeptList"));

            LinkButton db = (LinkButton)item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            if (chkDeptList != null)
            {
                DataSet ds = PhoenixRegistersDepartment.Listdepartment(1, null);
                DataTable dt = ds.Tables[0];
                chkDeptList.DataSource = dt;
                chkDeptList.DataBindings.DataTextField = "FLDDEPARTMENTNAME";
                chkDeptList.DataBindings.DataValueField = "FLDDEPARTMENTID";
                chkDeptList.DataBind();
            }

        }

    }


    private void ShowNoRecordsFound(DataTable dt, RadGrid gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Items[0].Cells.Clear();
        gv.Items[0].Cells.Add(new TableCell());
        gv.Items[0].Cells[0].ColumnSpan = colcount;
        gv.Items[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Items[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Items[0].Cells[0].Font.Bold = true;
        gv.Items[0].Cells[0].Text = "NO RECORDS FOUND";
    }

    private void ShowQuery(Guid? categoryid, DataTable dt, DataTable dtcolumn)
    {
        int columncount = AddTemplateColumn(categoryid, dt, dtcolumn);
        int noofcolumns = int.Parse(dt.Rows[0]["FLDNOOFCOLUMNS"].ToString());



        if (columncount > 0)
        {
            if (dtcolumn.Rows.Count > 0)
            {
                gvTMSAMatrix.DataSource = dt;
                gvTMSAMatrix.DataSource = dtcolumn;
            }
            else
            {
                gvTMSAMatrix.DataSource = dt;
            }
            for (int i = noofcolumns + 7; i < gvTMSAMatrix.Columns.Count; i++)
            {
                gvTMSAMatrix.Columns[i].Visible = false;
            }
            gvTMSAMatrix.DataBind();

        }
    }

    private int AddTemplateColumn(Guid? queryid, DataTable dt, DataTable dtcolumn)
    {

        int noofcolumns = int.Parse(dt.Rows[0]["FLDNOOFCOLUMNS"].ToString());
        string columnheader = "";
        string columnheadervalue = "";
        string columnheadertext = "";
        //string column = "";

        if (dtcolumn.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < noofcolumns; i++)
                {
                    int j = i + 1;
                    GridTemplateColumn field = new GridTemplateColumn();
                    columnheader = "FLDCOLUMN" + (j).ToString();
                    columnheadertext = "FLDCOLUMNTEXT" + (j).ToString();
                    columnheadervalue = columnheader;
                    field.HeaderTemplate = new GridViewTemplate(ListItemType.Header, dr[columnheadertext].ToString());
                    if (dr[columnheadertext].ToString() == "Stage")
                    {
                        field.HeaderStyle.Width = 50;
                        field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    }
                    else
                    {
                        field.HeaderStyle.Width = 250;
                        field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    }
                    field.ItemTemplate = new GridViewTemplate(ListItemType.Item, columnheadervalue, "Label");
                    gvTMSAMatrix.Columns.AddAt(i, field);
                }

            }
        }
        else
        {
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 1; i <= noofcolumns; i++)
                {
                    int j = i + 1;
                    GridTemplateColumn field = new GridTemplateColumn();
                    columnheader = "FLDCOLUMN" + (j).ToString();
                    columnheadertext = "FLDCOLUMNTEXT" + (j).ToString();
                    columnheadervalue = columnheader;
                    field.HeaderTemplate = new GridViewTemplate(ListItemType.Header, dr[columnheadertext].ToString());
                    gvTMSAMatrix.Columns.Add(field);
                }

            }
        }

        return dt.Rows.Count;
    }

    public class GridViewTemplate : ITemplate
    {
        ListItemType _templatetype;
        string _columnname;
        string _fieldtype;
        int _textboxid;


        public GridViewTemplate(ListItemType type, string columnname)
        {
            _templatetype = type;
            _columnname = columnname;
            _fieldtype = "Label";
        }

        public GridViewTemplate(ListItemType type, int i, string columnname, string fieldtype)
        {
            _templatetype = type;
            _columnname = columnname;
            _fieldtype = "RadTextBox";
            _textboxid = i;
        }

        public GridViewTemplate(ListItemType type, string columnname, string fieldtype)
        {
            _templatetype = type;
            _columnname = columnname;
            _fieldtype = fieldtype;
        }

        private void OnDataBinding(object sender, EventArgs e)
        {
            Control ctl = (Control)sender;

            GridDataItem dataitemcontainer = (GridDataItem)ctl.NamingContainer;
            object boundvalue = DataBinder.Eval(dataitemcontainer.DataItem, _columnname);

            switch (_templatetype)
            {
                case ListItemType.Item:
                    PopulateField(sender, _fieldtype, boundvalue);
                    break;

                case ListItemType.Header:
                    PopulateField(sender, _fieldtype, boundvalue);
                    break;

                case ListItemType.Footer:
                    PopulateField(sender, _fieldtype, boundvalue);
                    break;
            }
        }

        private void PopulateField(object sender, string fieldtype, object value)
        {
            switch (fieldtype.ToUpper())
            {
                case "LABEL":
                    Label lbl = (Label)sender;
                    lbl.Text = value.ToString();
                    break;
                case "LINKBUTTON":
                    LinkButton lnk = (LinkButton)sender;
                    lnk.Text = "add";
                    lnk.ID = "cmdEdit";
                    lnk.Attributes.Add("runat", "server");
                    break;
                case "RADTEXTBOX":
                    RadTextBox txt = (RadTextBox)sender;
                    txt.ID = "txtbox" + _textboxid;
                    //txt.Text = value.ToString();
                    break;
            }
        }

        void ITemplate.InstantiateIn(System.Web.UI.Control container)
        {
            switch (_templatetype)
            {
                case ListItemType.Header:
                    Literal ltl = new Literal();
                    ltl.Text = _columnname;
                    container.Controls.Add(ltl);
                    break;

                case ListItemType.Item:
                    switch (_fieldtype.ToUpper())
                    {
                        case "LABEL":
                            Label lbl = new Label();
                            lbl.DataBinding += new EventHandler(OnDataBinding);
                            container.Controls.Add(lbl);
                            break;
                        case "LINKBUTTON":
                            LinkButton lnk = new LinkButton();
                            lnk.DataBinding += new EventHandler(OnDataBinding);
                            container.Controls.Add(lnk);
                            break;
                        default:
                            ltl = new Literal();
                            ltl.DataBinding += new EventHandler(OnDataBinding);
                            container.Controls.Add(ltl);
                            break;
                    }
                    break;

                case ListItemType.EditItem:
                    break;

                case ListItemType.Footer:
                    RadTextBox ltl1 = new RadTextBox();
                    ltl1.ID = "txtbox" + _textboxid;
                    ltl1.Attributes.Add("runat", "server");
                    container.Controls.Add(ltl1);
                    break;
            }
        }
    }

    protected void MenuDocumentCategoryMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;

        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("NEW"))
            {

            }

            if (CommandName.ToUpper().Equals("SAVE"))
            {

            }
            if (CommandName.ToUpper().Equals("DELETE"))
            {

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlRefNo_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {        
        BindData();
    }


    protected void chkSelect_Click(object sender, EventArgs e)
    {
        ArrayList SelectedSections = new ArrayList();
        Guid index = new Guid();

        foreach (GridDataItem item in gvTMSAMatrix.Items)
        {
            if (item.GetDataKeyValue("FLDCONTENTID").ToString() != "")
            {
                index = new Guid(item.GetDataKeyValue("FLDCONTENTID").ToString());

                PhoenixInspectionTMSAMatrix.ComplianceUpdate(General.GetNullableGuid(index.ToString()),
                                                (((RadCheckBox)item.FindControl("chkSelect")).Checked == true) ? 1 : 0);
            }
        }
        if (Request.QueryString["categoryid"] != null || ViewState["CONTENTID"] != null)
        {
            BindData();
        }
    }

}